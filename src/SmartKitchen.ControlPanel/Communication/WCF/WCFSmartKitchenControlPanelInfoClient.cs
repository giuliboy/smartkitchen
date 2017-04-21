using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.Threading.Tasks;
using HSR.CloudSolutions.SmartKitchen.ControlPanel.Communication.Core;
using HSR.CloudSolutions.SmartKitchen.Devices;
using HSR.CloudSolutions.SmartKitchen.Devices.Communication;
using HSR.CloudSolutions.SmartKitchen.Service.Interface;
using HSR.CloudSolutions.SmartKitchen.Util;

namespace HSR.CloudSolutions.SmartKitchen.ControlPanel.Communication.WCF
{
    public class WCFSmartKitchenControlPanelInfoClient : BaseClient, ISmartKitchenControlPanelInfoClient
    {
        private ChannelFactory<ISmartKitchenControlPanelService> _channelFactory;
        private ISmartKitchenControlPanelService _client;

        public async Task InitAsync()
        {
            await Task.Run(() =>
            {
                _channelFactory = new ChannelFactory<ISmartKitchenControlPanelService>("SmartKitchenControlPanelService");
                _client = _channelFactory.CreateChannel();
            });
        }

        public async Task<IEnumerable<DeviceDTO>> LoadDevicesAsync()
        {
            var devices = new List<DeviceDTO>();
            try
            {
                devices.AddRange(await _client.GetRegisteredDevicesAsync());
            }
            catch (Exception ex)
            {
                LogException("Loading devices failed.", ex);
            }
            return devices;
        }

        protected override void OnDispose()
        {
            try
            {
                (_client as ICommunicationObject)?.Close();
            }
            catch (Exception ex)
            {
                LogException("Closing client failed.", ex);
            }
            try
            {
                if (_channelFactory.State != CommunicationState.Faulted)
                {
                    _channelFactory.Close();
                }
            }
            catch (Exception ex)
            {
                LogException("Closing channelfactory failed.", ex);
            }
        }
    }
}
