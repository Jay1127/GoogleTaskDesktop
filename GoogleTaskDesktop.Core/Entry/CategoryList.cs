using Google.Apis.Tasks.v1.Data;
using System.Collections.Generic;
using System.Linq;

namespace GoogleTaskDesktop.Core
{
    /// <summary>
    /// 카테고리 관리
    /// </summary>
    public class CategoryList : ICategoryList, ICategory
    {
        /// <summary>
        /// 카테고리 리스트
        /// </summary>
        private List<Category> _categories;

        /// <summary>
        /// 할일 목록들
        /// </summary>
        private List<TaskItem> _tasks;

        private const string ALL_CATEGORY_ID = "ALL_CATEGORY_ID";
        private const string DELETED_CATEGORY_ID = "DELETED_CATEGORY_ID";
        private const string ALL_CATEGORY_TITLE = "ALL";

        public static Category Deleted { get; set; }

        public string Id => ALL_CATEGORY_ID;
        public string Title => ALL_CATEGORY_TITLE;
        public bool CanDelete => false;

        public CategoryList()
        {
            _categories = new List<Category>();
            _tasks = new List<TaskItem>();
            Deleted = new Category(DELETED_CATEGORY_ID, "DELETED");
        }

        /// <summary>
        /// 서버에서 카테고리 로드
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task LoadAsync()
        {
            var service = new GoogleTaskService();
            var taskLists = (await service.GetTaskListsAsync()).Items;

            _categories.AddRange(taskLists.Select(t => new Category(t.Id, t.Title)));

            foreach(var category in _categories)
            {
                await category.LoadAsync();
                _tasks.AddRange(category.GetTasks());
            }
        }

        /// <summary>
        /// 해당 카테고리명으로 카테고리를 생성하여 추가함.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<Category> WithTaskListItemAsync(string title)
        {
            var service = new GoogleTaskService();
            var newTaskList = await service.InsertTaskListAsync(new TaskList() { Title = title });
            var taskListItem = new Category(newTaskList.Id, newTaskList.Title);

            _categories.Add(taskListItem);

            return taskListItem;
        }

        /// <summary>
        /// 카테고리 추가
        /// </summary>
        /// <param name="category"></param>
        public async System.Threading.Tasks.Task AddCategoryAsync(Category category)
        {
            // 처음 생성하는 경우 ID를 부여안함.
            // ID는 서버에서 자동으로 부여함.(미리 부여하면 에러발생함.)
            var service = new GoogleTaskService();
            var newTaskList = await service.InsertTaskListAsync(new TaskList() { Title = category.Title });

            _categories.Add(category);
        }

        /// <summary>
        /// 카테고리 리스트 반환
        /// </summary>
        /// <returns></returns>
        public List<Category> GetCategories()
        {
            return _categories.ToList();
        }

        /// <summary>
        /// 카테고리 찾기
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public Category FindCategory(string categoryId)
        {
            return _categories.Find(t => t.Id == categoryId);
        }

        /// <summary>
        /// 카테고리 삭제
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task RemoveCategoryAsync(string categoryId)
        {
            var taskList = FindCategory(categoryId);

            var service = new GoogleTaskService();
            await service.DeleteTaskListAsync(categoryId);

            _categories.Remove(taskList);
        }

        /// <summary>
        /// 카테고리 업데이트
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task UpdateCategoryAsync(Category category)
        {
            var service = new GoogleTaskService();

            var newTaskList = await service.UpdateTaskListAsync(category.ToTaskList());

            var index = _categories.FindIndex(t => t.Id == category.Id);
            _categories[index] = new Category(newTaskList.Id, newTaskList.Title);
        }

        public async System.Threading.Tasks.Task AddTaskAsync(TaskItem taskItem)
        {
            Category category = FindCategory(taskItem.CategoryId);
            await category.AddTaskAsync(taskItem);

            _tasks.Add(category.FindTask(taskItem.Id));
        }

        public List<TaskItem> GetTasks()
        {
            return _tasks;
        }

        public TaskItem FindTask(string taskId)
        {
            return  _tasks.Find(t => t.Id == taskId);
        }

        public async System.Threading.Tasks.Task RemoveTaskAsync(string taskId)
        {
            Category category = FindCategory(FindTask(taskId).CategoryId);

            await category.RemoveTaskAsync(taskId);

            _tasks.Remove(FindTask(taskId));
        }

        public async System.Threading.Tasks.Task UpdateTaskAsync(TaskItem taskItem)
        {
            Category category = FindCategory(taskItem.CategoryId);

            await category.UpdateTaskAsync(taskItem);

            int index = _tasks.FindIndex(t => t.Id == taskItem.Id);
            _tasks[index] = category.FindTask(taskItem.Id);
        }
    }
}