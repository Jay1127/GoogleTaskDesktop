using Google.Apis.Tasks.v1.Data;
using System.Collections.Generic;
using System.Linq;

namespace GoogleTaskDesktop.Core
{
    /// <summary>
    /// 카테고리(할일 목록)
    /// </summary>
    public class Category : ICategory
    {
        /// <summary>
        /// 아이디
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 카테고리명
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 삭제 가능 여부(ALL, Deleted 삭제X)
        /// </summary>
        public bool CanDelete { get; set; } = true;

        /// <summary>
        /// 할일 목록들
        /// </summary>
        private List<TaskItem> _tasks;

        /// <summary>
        /// 카테고리 생성
        /// </summary>
        /// <param name="title"></param>
        public Category(string title)
            : this(null, title)
        {
        }

        /// <summary>
        /// 카테고리 생성
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        public Category(string id, string title)
        {
            Id = id;
            Title = title;
            _tasks = new List<TaskItem>();
        }

        /// <summary>
        /// 서버에서 할일리스트 로드
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task LoadAsync()
        {
            var service = new GoogleTaskService();
            var gTasks = (await service.GetTasksAsync(Id)).Items;

            if (gTasks != null)
            {
                // all tasks
                var mainTasks = gTasks.Select(t => new TaskItem(Id, t.Id, t.Title, GoogleTaskStatus.CheckIsCompleted(t.Status), t.Parent) { Note = t.Notes }).ToList();

                // main tasks
                _tasks.AddRange(mainTasks.Where(t => t.ParentTask == null));

                // sub tasks
                var taskDic = mainTasks.ToDictionary(t => t.Id);

                foreach(var task in taskDic)
                {
                    var parentId = task.Value.ParentTask;

                    if (parentId != null)
                    {
                        taskDic[parentId].SubItems.Add(task.Value);
                    }
                }
            }
        }

        /// <summary>
        /// 해당 할일목록을 추가
        /// </summary>
        /// <param name="taskItem"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task AddTaskAsync(TaskItem taskItem)
        {
            var task = taskItem.ToTask();

            var service = new GoogleTaskService();
            var newTask = await service.InsertTaskAsync(task, Id);

            if (!string.IsNullOrEmpty(taskItem.ParentTask))
            {
                newTask.Parent = taskItem.ParentTask;
                newTask = await service.MoveTaskAsync(newTask, taskItem.CategoryId);
            }

            var newTaskItem = new TaskItem(Id, newTask.Id, newTask.Title, false, newTask.Parent);

            _tasks.Add(newTaskItem);
        }
        
        /// <summary>
        /// 카테고리에 속한 할일 목록 반환
        /// </summary>
        /// <returns></returns>
        public List<TaskItem> GetTasks()
        {
            return _tasks.ToList();
        }

        /// <summary>
        /// 카테고리 내에서 해당 아이디의 할일 찾기
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public TaskItem FindTask(string taskId)
        {
            return _tasks.Find(t => t.Id == taskId);
        }

        /// <summary>
        /// 카테고리 내에서 해당 할일 삭제
        /// </summary>
        /// <param name="taskItem"></param>
        public async System.Threading.Tasks.Task RemoveTaskAsync(string taskId)
        {
            var service = new GoogleTaskService();

            var items = (await service.GetTasksAsync(Id)).Items;

            await service.DeleteTaskAsync(taskId, Id);

            items = (await service.GetTasksAsync(Id)).Items;
            _tasks.Remove(FindTask(taskId));
        }

        /// <summary>
        /// 해당 할일 데이터 업데이트
        /// </summary>
        /// <param name="taskItem"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task UpdateTaskAsync(TaskItem taskItem)
        {
            // 서버에 업데이트
            var service = new GoogleTaskService();
            var newTask = await service.UpdateTaskAsync(taskItem.ToTask(), Id);

            // 서버에서 데이터 가져와서 갱신
            var parent = _tasks.Find(t => t.Id == newTask.Parent);

            if (parent == null)
            {
                var index = _tasks.FindIndex(t => t.Id == newTask.Id);
                _tasks[index] = new TaskItem(Id, newTask.Id, newTask.Title, GoogleTaskStatus.CheckIsCompleted(newTask.Status))
                {
                    Note = newTask.Notes
                };
            }
            else
            {
                var index = parent.SubItems.FindIndex(t => t.Id == newTask.Id);
                parent.SubItems[index] = new TaskItem(Id, newTask.Id, newTask.Title,
                                         GoogleTaskStatus.CheckIsCompleted(newTask.Status))
                {
                    Note = newTask.Notes
                };
            }
        }

        /// <summary>
        /// 구글 할일목록리스트로 반환
        /// </summary>
        /// <returns></returns>
        internal TaskList ToTaskList()
        {
            return new Google.Apis.Tasks.v1.Data.TaskList()
            {
                Id = Id,
                Title = Title
            };
        }

    }
}