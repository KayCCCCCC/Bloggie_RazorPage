using Bloggie.Enums;

namespace Bloggie.Models.ViewModels
{
    public class Notification
    {
        public string Message { get; set; }
        public NotificationType Type { get; set; }
    }
}
