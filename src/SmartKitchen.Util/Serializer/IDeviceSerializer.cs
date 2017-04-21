using System;
using System.Threading.Tasks;
using HSR.CloudSolutions.SmartKitchen.Devices;
using HSR.CloudSolutions.SmartKitchen.Devices.Communication;

namespace HSR.CloudSolutions.SmartKitchen.Util.Serializer
{
    public interface IDeviceSerializer
    {
        string Serialize<T>(T dto) where T : DeviceDTO;
        T DeserializeDto<T>(string serializedDto) where T : DeviceDTO;

        string Serialize<T>(ICommand<T> command) where T : DeviceDTO;
        ICommand<T> DeserializeCommand<T>(string serializedCommand) where T : DeviceDTO;

        string Serialize<T>(INotification<T> notification) where T : DeviceDTO;
        INotification<T> DeserializeNotification<T>(string serializedNotification) where T : DeviceDTO;
    }
}
