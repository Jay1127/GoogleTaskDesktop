using Google.Apis.Tasks.v1.Data;
using System.Collections.Generic;

namespace GoogleTaskDesktop.Core
{
    /// <summary>
    /// 할일
    /// </summary>
    public class TaskItem
    {
        /// <summary>
        /// 아이디
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 할일 제목
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 완료여부
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// 삭제여부
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 상위 할일의 아이디(없으면 NULL)
        /// </summary>
        public string ParentTask { get; set; } = null;
        
        /// <summary>
        /// 상위 카테고리의 아이디
        /// </summary>
        public string CategoryId { get; set; }

        /// <summary>
        /// 하위 할일 리스트
        /// </summary>
        public List<TaskItem> SubItems { get; set; } = null;

        /// <summary>
        /// 할일 생성
        /// </summary>
        public TaskItem(string title)
        {
            Title = title;
        }

        /// <summary>
        /// 할일 생성
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="title"></param>
        /// <param name="isCompleted"></param>
        public TaskItem(string categoryId, string taskId, string title, bool isCompleted = false)
        {
            CategoryId = categoryId;
            Id = taskId;
            Title = title;
            IsCompleted = isCompleted;
        }

        /// <summary>
        /// 구글 할일로 변환
        /// </summary>
        /// <returns></returns>
        internal Task ToTask()
        {
            return new Task()
            {
                Id = Id,
                Title = Title,
                Status = GoogleTaskStatus.GetTaskStatus(IsCompleted)
            };
        }
    }
}