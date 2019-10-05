using System;

namespace GoogleTaskDesktop.Core
{
    public class GoogleTaskStatus
    {
        public const string STATUS_WORKING = "needsAction";
        public const string STATUS_COMPLETED = "completed";

        public static string GetTaskStatus(bool isCompleted)
        {
            return isCompleted ? STATUS_COMPLETED : STATUS_WORKING;
        }

        public static bool CheckIsCompleted(string status)
        {
            if(status == STATUS_COMPLETED)
            {
                return true;
            }
            else if(status == STATUS_WORKING)
            {
                return false;
            }

            throw new ArgumentException("매개변수값이 잘못됨.");
        }
    }
}