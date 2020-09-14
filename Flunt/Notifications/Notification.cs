using System;

namespace Flunt.Notifications
{
    public sealed class Notification
    {
        public Notification(string property, string message)
        {
            NotificationId = Guid.NewGuid();
            Property = property;
            Message = message;
        }

        public Guid NotificationId { get; set; }
        public string Property { get; private set; }
        public string Message { get; private set; }
    }
}