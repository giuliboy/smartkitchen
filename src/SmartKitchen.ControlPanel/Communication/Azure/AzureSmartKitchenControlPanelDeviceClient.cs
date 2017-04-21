using System.Threading.Tasks;
using HSR.CloudSolutions.SmartKitchen.ControlPanel.Communication.Core;
using HSR.CloudSolutions.SmartKitchen.Devices;
using HSR.CloudSolutions.SmartKitchen.Devices.Communication;
using HSR.CloudSolutions.SmartKitchen.Util.Serializer;
using SmartKitchen.Azure;
using System;

namespace HSR.CloudSolutions.SmartKitchen.ControlPanel.Communication.Azure
{
    /// <summary>
    /// This class is used to send commands to devices and for receiving their notifications.
    /// </summary>
    /// <typeparam name="T">The type of DeviceDTO this client is used for.</typeparam>
    public class AzureSmartKitchenControlPanelDeviceClient<T> : BaseClient, ISmartKitchenControlPanelDeviceClient<T>
        where T : DeviceDTO
    {
        private readonly IDialogService _dialogService; // Can display exception in a dialog.
        private readonly IDeviceSerializer _serializer; // Can be used to serialize/deserialize DTO's, commands and notifications.
        private readonly ICommandTopicProvider _commandServiceBus;
        private readonly INotificationTopicProvider _notificationServiceBus;

        public AzureSmartKitchenControlPanelDeviceClient(IDialogService dialogService, IDeviceSerializer serializer, 
            ICommandTopicProvider commandServiceBus, 
            INotificationTopicProvider notificationServiceBus)
        {
            _dialogService = dialogService;
            _serializer = serializer;
            _commandServiceBus = commandServiceBus;
            _notificationServiceBus = notificationServiceBus;
        } 

        /// <summary>
        /// Used to establish the communication.
        /// </summary>
        /// <param name="device">The device this client is responsible for.</param>
        public async Task InitAsync(T device)
        {
            IsInitialized = true;
        }

        /// <summary>
        /// True if InitAsync was called and client is initialized.
        /// </summary>
        public bool IsInitialized { get; private set; } = false;

        /// <summary>
        /// Send a command to the simulator.
        /// </summary>
        /// <param name="command">Command to send</param>
        public async Task SendCommandAsync(ICommand<T> command)
        {
            if (command == null)
            {
                return;
            }
            try
            {
                var message = _serializer.Serialize(command);
                var filterKey = command.DeviceConfig.Key.ToString();
                await _commandServiceBus.SendMessage(message, filterKey);
            }
            catch (Exception ex)
            {
                LogException("Sending command failed.", ex);
                _dialogService.ShowException(ex);
            }
        }

        /// <summary>
        /// Checks if a notification for the <paramref name="device" /> is pending.
        /// </summary>
        /// <param name="device">The device to check notifications for.</param>
        /// <returns>A received notification or NullNotification&lt;T&gt;</returns>
        public async Task<INotification<T>> CheckNotificationsForAsync(T device)
        {
            if (device == null)
            {
                return new NullNotification<T>();
            }
            try
            {
                var message = await _notificationServiceBus.ReceiveMessage(device.Key.ToString());
                return _serializer.DeserializeNotification<T>(message.GetBody<string>()) ?? new NullNotification<T>();
            }
            catch (Exception ex)
            {
                LogException("Checking for notifications failed.", ex);
            }
            return new NullNotification<T>();
        }

        /// <summary>
        /// Use this method to tear down any established connections.
        /// </summary>
        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
