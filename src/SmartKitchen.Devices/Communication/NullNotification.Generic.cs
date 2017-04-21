using System.Runtime.Serialization;

namespace HSR.CloudSolutions.SmartKitchen.Devices.Communication
{
    [DataContract]
    public class NullNotification<T> : INotification<T>
        where T : DeviceDTO
    {
        [DataMember]
        public T DeviceInfo { get; private set; } = null;

        [DataMember]
        public bool HasDeviceInfo { get; private set; } = false;

        [DataMember]
        public string Type { get; private set; } = "None";
    }
}
