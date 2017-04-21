using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using HSR.CloudSolutions.SmartKitchen.Devices;
using HSR.CloudSolutions.SmartKitchen.Devices.Communication;

namespace HSR.CloudSolutions.SmartKitchen.Service.Interface
{
    [ServiceContract]
    [ServiceKnownType(typeof(FridgeDTO))]
    [ServiceKnownType(typeof(OvenDTO))]
    [ServiceKnownType(typeof(StoveDTO))]
    [ServiceKnownType(typeof(NullCommand))]
    [ServiceKnownType(typeof(DeviceCommand<FridgeDTO>))]
    [ServiceKnownType(typeof(DeviceCommand<OvenDTO>))]
    [ServiceKnownType(typeof(DeviceCommand<StoveDTO>))]
    [ServiceKnownType(typeof(NullNotification))]
    [ServiceKnownType(typeof(DeviceNotification<FridgeDTO>))]
    [ServiceKnownType(typeof(DeviceNotification<OvenDTO>))]
    [ServiceKnownType(typeof(DeviceNotification<StoveDTO>))]
    public interface ISmartKitchenControlPanelService
    {
        [OperationContract]
        Task<IEnumerable<DeviceDTO>> GetRegisteredDevicesAsync();

        [OperationContract]
        Task SendCommandAsync(ICommand<DeviceDTO> command);

        [OperationContract]
        Task<INotification<DeviceDTO>> PeekNotificationAsync(DeviceDTO device);
    }
}
