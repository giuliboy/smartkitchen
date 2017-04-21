using System.Runtime.Serialization;

namespace HSR.CloudSolutions.SmartKitchen.Devices.Communication
{
    [DataContract]
    public class NullCommand : ICommand<DeviceDTO>
    {
        [DataMember]
        public string Command { get; private set; } = "Nothing";
        [DataMember]
        public DeviceDTO DeviceConfig { get; private set; } = null;
        [DataMember]
        public bool HasDeviceConfig { get; private set; } = false;
    }
}
