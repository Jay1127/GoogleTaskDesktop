using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleTaskDesktop.Core
{
    public interface ICategory
    {
        string Id { get; }
        string Title { get; set; }
        bool CanDelete { get; }

        Task LoadAsync();
        Task AddTaskAsync(TaskItem taskItem);
        List<TaskItem> GetTasks();
        TaskItem FindTask(string taskId);
        Task RemoveTaskAsync(string taskId);
        Task UpdateTaskAsync(TaskItem taskItem);
    }
}
