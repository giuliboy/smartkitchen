namespace SmartKitchen.Azure
{
    public class CommandTopicProvider : TopicProvider, ICommandTopicProvider
    {
        public CommandTopicProvider(string connectionString, string topicName) 
            : base(connectionString, topicName)
        {

        }
    }
}
