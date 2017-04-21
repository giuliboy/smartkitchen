namespace SmartKitchen.Azure
{
    public class NotificationTopicProvider : TopicProvider, INotificationTopicProvider
    {
        public NotificationTopicProvider(string connectionString, string topicName) 
            : base(connectionString, topicName)
        {
        }
    }
}
