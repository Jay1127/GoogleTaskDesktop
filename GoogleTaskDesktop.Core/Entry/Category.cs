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
            var tasks = (await service.GetTasksAsync(Id)).Items;

            if (tasks != null)
            {
                _tasks.AddRange(
                    tasks.Select(task =>
                    {
                        return new TaskItem(Id, task.Id, task.Title, 
                                            GoogleTaskStatus.CheckIsCompleted(task.Status));
                    }));
            }
        }

        /// <summary>
        /// 해당 제목으로 할일 목록을 생성해서 추가함.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<TaskItem> WithTaskItemAsync(string title)
        {
            var service = new GoogleTaskService();
            var newTask = await service.InsertTaskAsync(new Task() { Title = title }, Id);
            var taskItem = new TaskItem(Id, newTask.Id, newTask.Title);

            _tasks.Add(taskItem);

            return taskItem;
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

            var newTaskItem = new TaskItem(Id, newTask.Id, newTask.Title);

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
            await service.DeleteTaskAsync(taskId, Id);

            _tasks.Remove(FindTask(taskId));
        }

        /// <summary>
        /// 해당 할일 데이터 업데이트
        /// </summary>
        /// <param name="taskItem"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task UpdateTaskAsync(TaskItem taskItem)
        {
            var service = new GoogleTaskService();
            var newTask = await service.UpdateTaskAsync(taskItem.ToTask(), Id);

            var index = _tasks.FindIndex(t => t.Id == newTask.Id);
            _tasks[index] = new TaskItem(Id, newTask.Id, newTask.Title, 
                                         GoogleTaskStatus.CheckIsCompleted(newTask.Status));
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