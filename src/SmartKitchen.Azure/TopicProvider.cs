using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System.Threading.Tasks;

namespace SmartKitchen.Azure
{
    public class TopicProvider : ITopicProvider
    {
        private readonly string _connectionString;
        private readonly string _topicName;
        private NamespaceManager _namespaceManager;

        public TopicProvider(string connectionString, string topicName)
        {
            _connectionString = connectionString;
            _topicName = topicName;

            _namespaceManager = NamespaceManager.CreateFromConnectionString(_connectionString);

            if (!_namespaceManager.TopicExists(_topicName))
            {
                _namespaceManager.CreateTopicAsync(_topicName);
            }
        }

        public async Task SendMessage(object message, string filterKey)
        {
            var client = TopicClient.CreateFromConnectionString(_connectionString, _topicName);
            var brokeredMessage = new BrokeredMessage(message);
            brokeredMessage.Properties["dkey"] = filterKey;
            await client.SendAsync(brokeredMessage);
        }

        public async Task<BrokeredMessage> ReceiveMessage(string filterKey)
        {
            var subscription = new SubscriptionDescription(_topicName, filterKey);
            SqlFilter sqlFilter = new SqlFilter("dkey='" + filterKey + "'");

            if (!_namespaceManager.SubscriptionExists(subscription.TopicPath, subscription.Name))
            {
                var commandSubscription = _namespaceManager.CreateSubscription(subscription, sqlFilter);
            }

            var client = SubscriptionClient.CreateFromConnectionString( _connectionString, subscription.TopicPath, subscription.Name);
            return await client.ReceiveAsync();
        }
    }
}
