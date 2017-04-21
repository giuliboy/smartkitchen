using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace SmartKitchen.Azure
{
    public interface ITopicProvider
    {
        Task<BrokeredMessage> ReceiveMessage(string filterKey);
        Task SendMessage(object msg, string filterKey);
    }

    /// <summary>
    /// marker interface used for DI
    /// </summary>
    public interface ICommandTopicProvider : ITopicProvider
    {
    }
    /// <summary>
    /// marker interface used for DI
    /// </summary>
    public interface INotificationTopicProvider : ITopicProvider
    {
    }
}