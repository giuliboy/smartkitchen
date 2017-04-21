using System.Runtime.Serialization;

namespace HSR.CloudSolutions.SmartKitchen.Devices.Communication
{
    [DataContract]
    public class NullNotification : INotification<DeviceDTO>
    {
        [DataMember]
        public DeviceDTO DeviceInfo { get; private set; } = null;

        [DataMember]
        public bool HasDeviceInfo { get; private set; } = false;

        [DataMember]
        public string Type { get; private set; } = "None";
    }
}
