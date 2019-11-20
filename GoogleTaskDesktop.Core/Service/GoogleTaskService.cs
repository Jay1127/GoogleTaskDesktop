using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;
using System;

namespace GoogleTaskDesktop.Core
{
    public class GoogleTaskService : IDisposable
    {
        private TasksService _taskService;

        public GoogleTaskService()
        {
            _taskService = ServiceLoader.Service;
        }

        public void Dispose()
        {
            _taskService?.Dispose();
        }

        #region TaskList

        /// <summary>
        /// 전체 TaskList 가져오기
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<TaskLists> GetTaskListsAsync()
        {
            TasklistsResource.ListRequest request = _taskService.Tasklists.List();
            request.MaxResults = 100;

            return await request.ExecuteAsync();
        }

        /// <summary>
        /// 특정 TaskList 가져오기
        /// </summary>
        /// <param name="taskListId">찾을 TaskList의 Id</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<Google.Apis.Tasks.v1.Data.TaskList> FindTaskListAsync(string taskListId)
        {
            return await _taskService.Tasklists.Get(taskListId).ExecuteAsync();
        }

        /// <summary>
        /// TaskList 추가
        /// </summary>
        /// <param name="taskList">추가할 taskList</param>
        /// <returns>추가된 TaskList</returns>
        public async System.Threading.Tasks.Task<Google.Apis.Tasks.v1.Data.TaskList> InsertTaskListAsync(Google.Apis.Tasks.v1.Data.TaskList taskList)
        {
            return await _taskService.Tasklists.Insert(taskList).ExecuteAsync();
        }

        /// <summary>
        /// Tasklist 삭제
        /// </summary>
        /// <param name="taskListId">삭제할 taskList의 Id</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task DeleteTaskListAsync(string taskListId)
        {
            await _taskService.Tasklists.Delete(taskListId).ExecuteAsync();
        }

        /// <summary>
        /// Tasklist 업데이트
        /// </summary>
        /// <param name="taskList">추가할 taskList(해당 taskList의 id가 업데이트할 taskList를 지정해야함.)</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<Google.Apis.Tasks.v1.Data.TaskList> UpdateTaskListAsync(Google.Apis.Tasks.v1.Data.TaskList taskList)
        {
            return await _taskService.Tasklists.Update(taskList, taskList.Id).ExecuteAsync();
        }

        #endregion

        #region Task

        /// <summary>
        /// TaskList내 모든 Task가져오기
        /// </summary>
        /// <param name="taskListId">가져올 TaskList의 Id</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<Tasks> GetTasksAsync(string taskListId)
        {
            return await _taskService.Tasks.List(taskListId).ExecuteAsync();
        }

        /// <summary>
        /// 특정 Task 가져오기
        /// </summary>
        /// <param name="taskListId">가져올 Task가 속한 TaskList의 아이디</param>
        /// <param name="taskId">가져올 Task 아이디</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<Task> FindTaskAsync(string taskListId, string taskId)
        {
            return await _taskService.Tasks.Get(taskListId, taskId).ExecuteAsync();
        }

        /// <summary>
        /// Task 추가하기
        /// </summary>
        /// <param name="task">추가할 Task</param>
        /// <param name="taskListId">추가할 TaskList의 아이디</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<Task> InsertTaskAsync(Task task, string taskListId)
        {
            return await _taskService.Tasks.Insert(task, taskListId).ExecuteAsync();
        }

        /// <summary>
        /// 특정 task를 특정 위치로 이동
        /// </summary>
        /// <param name="task"></param>
        /// <param name="taskListId"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<Task> MoveTaskAsync(Task task, string taskListId)
        {
            var moveRequest = _taskService.Tasks.Move(taskListId, task.Id);
            moveRequest.Parent = task.Parent;

            return await moveRequest.ExecuteAsync();
        }

        /// <summary>
        /// Task 삭제하기
        /// </summary>
        /// <param name="taskId">삭제할 Task의 아이디</param>
        /// <param name="taskListId">삭제할 Task가 속한 TaskList의 아이디</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task DeleteTaskAsync(string taskId, string taskListId)
        {
            await _taskService.Tasks.Delete(taskListId, taskId).ExecuteAsync();
        }

        /// <summary>
        /// Task 정보 업데이트
        /// </summary>
        /// <param name="task">Task업데이트</param>
        /// <param name="taskListId">속한 TaskList의 아이디</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<Task> UpdateTaskAsync(Task task, string taskListId)
        {
            return await _taskService.Tasks.Update(task, taskListId, task.Id).ExecuteAsync();
        }

        #endregion
    }
}
