using System.Collections.Generic;
using System.Threading.Tasks;
using HSR.CloudSolutions.SmartKitchen.ControlPanel.Communication.Core;
using HSR.CloudSolutions.SmartKitchen.Devices;
using HSR.CloudSolutions.SmartKitchen.Util.Serializer;
using System;
using SmartKitchen.Azure;

namespace HSR.CloudSolutions.SmartKitchen.ControlPanel.Communication.Azure
{
    /// <summary>
    /// This class is used to receive the registered devices.
    /// </summary>
    public class AzureSmartKitchenControlPanelInfoClient : BaseClient, ISmartKitchenControlPanelInfoClient
    {
        private readonly IDialogService _dialogService; // Can display exception in a dialog.
        private readonly IDeviceSerializer _serializer; // Can be used to serialize/deserialize DTO's, commands and notifications.
        private readonly IDeviceDbContext _context;

        public AzureSmartKitchenControlPanelInfoClient(IDialogService dialogService, IDeviceSerializer serializer, 
            IDeviceDbContext context)
        {
            _dialogService = dialogService;
            _serializer = serializer;
            _context = context;
        }

        /// <summary>
        /// Used to establish the communication.
        /// </summary>
        public async Task InitAsync()
        {
            //do nothing. 
            await Task.Yield();
        }

        /// <summary>
        /// Loads the registerd devices from the simulator.
        /// </summary>
        /// <returns>The list of all known devices.</returns>
        public async Task<IEnumerable<DeviceDTO>> LoadDevicesAsync()
        {
            try
            {
                var devices = _context.GetDevices();

                return devices;
            }
            catch (Exception ex)
            {
                LogException("Loading devices failed.", ex);
            }
            return new List<DeviceDTO>();
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
