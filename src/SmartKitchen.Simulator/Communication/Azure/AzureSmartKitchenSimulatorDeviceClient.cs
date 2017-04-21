using System.Threading.Tasks;
using HSR.CloudSolutions.SmartKitchen.Devices;
using HSR.CloudSolutions.SmartKitchen.Devices.Communication;
using HSR.CloudSolutions.SmartKitchen.Simulator.Communication.Core;
using HSR.CloudSolutions.SmartKitchen.Util.Serializer;
using SmartKitchen.Azure;
using System;

namespace HSR.CloudSolutions.SmartKitchen.Simulator.Communication.Azure
{
    /// <summary>
    /// This class is used to receive commands and send notifications.
    /// </summary>
    /// <typeparam name="T">The device this client is used for.</typeparam>
    public class AzureSmartKitchenSimulatorDeviceClient<T> : BaseClient, ISmartKitchenSimulatorDeviceClient<T>
        where T : DeviceDTO
    {
        private readonly IDialogService _dialogService; // Can display exception in a dialog.
        private readonly IDeviceSerializer _serializer; // Can be used to serialize/deserialize DTO's, commands and notifications.
        private readonly ICommandTopicProvider _commandServiceBus;
        private readonly INotificationTopicProvider _notificationServiceBus;

        public AzureSmartKitchenSimulatorDeviceClient(IDialogService dialogService, IDeviceSerializer serializer,
            ICommandTopicProvider commandServiceBus,
            INotificationTopicProvider notificationServiceBus)
        {
            _dialogService = dialogService;
            _serializer = serializer;
            _commandServiceBus = commandServiceBus;
            _notificationServiceBus = notificationServiceBus;
        }

        /// <summary>
        /// Establishes the connections used to talk to the Cloud.
        /// </summary>
        /// <param name="device">The device this client is used for.</param>
        public async Task InitAsync(T device)
        {
            //communication is done via topicProviders. this dependency will be injected and already initialized
            //do nothing
            await Task.Yield();
        }

        /// <summary>
        /// Checks if a command should be executed.
        /// </summary>
        /// <param name="device">The device to check for commands.</param>
        /// <returns>A received command or NullCommand&lt;T&gt;</returns>
        public async Task<ICommand<T>> CheckCommandsAsync(T device)
        {
            if (device == null)
            {
                return new NullCommand<T>();
            }
            try
            {
                var message = await _commandServiceBus.ReceiveMessage(device.Key.ToString());
                return _serializer.DeserializeCommand<T>(message.GetBody<string>()) ?? new NullCommand<T>();

            }
            catch (Exception ex)
            {
                LogException("Checking for commands failed.", ex);
            }
            return new NullCommand<T>();
        }

        /// <summary>
        /// Sends a notification to the control panel.
        /// </summary>
        /// <param name="notification">The notification to send.</param>
        public async Task SendNotificationAsync(INotification<T> notification)
        {
            if (notification == null)
            {
                return;
            }
            try
            {
                var message = _serializer.Serialize(notification);
                var filterKey = notification.DeviceInfo.Key.ToString();
                await _notificationServiceBus.SendMessage(message, filterKey);
            }
            catch (Exception ex)
            {
                LogException("Send notification failed.", ex);
                _dialogService.ShowException(ex);
            }
        }

        /// <summary>
        /// Use this method to tear down established connections.
        /// </summary>
        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
