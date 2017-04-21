using System.Security.Policy;
using HSR.CloudSolutions.SmartKitchen.ControlPanel.Communication;
using HSR.CloudSolutions.SmartKitchen.Devices;

namespace HSR.CloudSolutions.SmartKitchen.ControlPanel.ViewModels
{
    public class UnknownDeviceControllerViewModel : BaseDeviceControllerViewModel<DeviceDTO>
    {
        public UnknownDeviceControllerViewModel(ISmartKitchenControlPanelDeviceClient<DeviceDTO> client) : base(client, d => d)
        {
            
        }

        protected override void Configure(DeviceDTO config)
        {
            
        }

        protected override void OnUpdate(DeviceDTO update)
        {

        }

        protected override void Prepare(DeviceDTO dto)
        {
            
        }
    }
}
