using System;
using System.ServiceModel;
using System.Threading.Tasks;
using HSR.CloudSolutions.SmartKitchen.Devices;
using HSR.CloudSolutions.SmartKitchen.Service.Interface;
using HSR.CloudSolutions.SmartKitchen.Simulator.Communication.Core;

namespace HSR.CloudSolutions.SmartKitchen.Simulator.Communication.WCF
{
    public class WCFSmartKitchenSimulatorInfoClient<T> : BaseClient, ISmartKitchenSimulatorInfoClient<T>
        where T : DeviceDTO
    {
        private ChannelFactory<ISmartKitchenSimulatorService> _channelFactory;
        private ISmartKitchenSimulatorService _client;

        public async Task InitAsync()
        {
            await Task.Run(() =>
            {
                _channelFactory = new ChannelFactory<ISmartKitchenSimulatorService>("SmartKitchenSimulatorService");
                _client = _channelFactory.CreateChannel();
            });
        }

        public async Task RegisterDeviceAsync(T device)
        {
            if (device == null)
            {
                return;
            }
            try
            {
                await _client.RegisterDeviceAsync(device);
            }
            catch (Exception ex)
            {
                LogException("Register device failed.", ex);
            }
        }

        public async Task UnregisterDeviceAsync(T device)
        {
            if (device == null)
            {
                return;
            }
            try
            {
                await _client.UnregisterDeviceAsync(device);
            }
            catch (Exception ex)
            {
                LogException("Unregister device failed.", ex);
            }
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
