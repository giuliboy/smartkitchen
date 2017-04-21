using System.Threading.Tasks;
using HSR.CloudSolutions.SmartKitchen.Devices;
using HSR.CloudSolutions.SmartKitchen.Simulator.Communication.Core;
using HSR.CloudSolutions.SmartKitchen.Util.Serializer;
using SmartKitchen.Azure;
using System;
using System.IO;
using Microsoft.ServiceBus.Messaging;

namespace HSR.CloudSolutions.SmartKitchen.Simulator.Communication.Azure
{
    /// <summary>
    /// This class is used for registration and deregistration of devices.
    /// </summary>
    /// <typeparam name="T">The device this client is used for.</typeparam>
    public class AzureSmartKitchenSimulatorInfoClient<T> : BaseClient, ISmartKitchenSimulatorInfoClient<T>
        where T : DeviceDTO
    {
        private readonly IDialogService _dialogService; // Can be used to display dialogs when exceptions occur.
        private readonly IDeviceSerializer _serializer; // Can be used to serialize/deserialize DTO's, commands and notifications.
        private readonly IDeviceDbContext _context;
        private readonly INotificationTopicProvider _notificationServiceBus;

        public AzureSmartKitchenSimulatorInfoClient(
            IDialogService dialogService,
            IDeviceSerializer serializer, 
            IDeviceDbContext context, 
            INotificationTopicProvider notificationServiceBus)
        {
            _dialogService = dialogService;
            _serializer = serializer;
            _context = context;
            _notificationServiceBus = notificationServiceBus;
        } 

        /// <summary>
        /// Establishes the connections used to talk to the Cloud.
        /// </summary>
        public async Task InitAsync()
        {
            // Explicitly left empty
        }

        /// <summary>
        /// Registers a <paramref name="device"/> to be used with the control panel.
        /// </summary>
        /// <param name="device">The device to register.</param>
        public async Task RegisterDeviceAsync(T device)
        {
            try
            {
                var d = _context.GetDevice(device.DeviceId.ToString());
                if(d == null)
                {
                    d= _context.CreateDevice(device);
                }

                await _notificationServiceBus.SendMessage(d, d.Key.ToString());
            }
            catch (Exception ex)
            {
                LogException("Register device failed.", ex);
            }
        }

        /// <summary>
        /// Deregisters a <paramref name="device"/> to no longer be used with the control panel.
        /// </summary>
        /// <param name="device">The device to deregister.</param>
        public async Task UnregisterDeviceAsync(T device)
        {
            if (device == null)
            {
                return;
            }
            try
            {
                _context.RemoveDevice(device);
                await _notificationServiceBus.SendMessage(device, device.Key.ToString());
            }
            catch (Exception ex)
            {
                LogException("Unregister device failed.", ex);
            }
        }

        /// <summary>
        /// Use this method to tear down any established connection.
        /// </summary>
        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
