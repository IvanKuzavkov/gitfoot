using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace BitTorrent.WP7.Extensions
{
    public class TaskCompleteSummary<T>
    {
        public string TaskKey { get; set; }
        public TaskResult Result { get; set; }
        public string Message { get; set; }
        public T ResultObject { get; set; }
        public Exception Exception { get; set; }

        public TaskCompleteSummary(TaskResult result, string message, string taskKey = null)
        {
            Result = result;
            TaskKey = taskKey;
            Message = message;
        }
        public TaskCompleteSummary(T obj, string message = "")
            : this(TaskResult.Success, message)
        {
            ResultObject = obj;
        }

        public TaskCompleteSummary(Exception ex, string message = null)
            : this(TaskResult.Error, message ?? ex.Message)
        {
            Exception = ex;
        }
    }
}
