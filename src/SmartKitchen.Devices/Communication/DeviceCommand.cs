using System.Runtime.Serialization;

namespace HSR.CloudSolutions.SmartKitchen.Devices.Communication
{
    [DataContract]
    [KnownType(typeof(FridgeDTO))]
    [KnownType(typeof(OvenDTO))]
    [KnownType(typeof(StoveDTO))]
    public class DeviceCommand<T> : ICommand<T> 
        where T : DeviceDTO
    {
        public DeviceCommand(string command, T deviceConfig)
        {
            this.Command = command ?? "Missing";
            this.DeviceConfig = deviceConfig;
        }

        [DataMember]
        public string Command { get; private set; }
        [DataMember]
        public T DeviceConfig { get; private set; }

        public bool HasDeviceConfig => this.DeviceConfig != null;
    }
}
