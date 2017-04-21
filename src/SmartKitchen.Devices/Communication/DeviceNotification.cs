using System.Runtime.Serialization;

namespace HSR.CloudSolutions.SmartKitchen.Devices.Communication
{
    [DataContract]
    [KnownType(typeof(FridgeDTO))]
    [KnownType(typeof(OvenDTO))]
    [KnownType(typeof(StoveDTO))]
    public class DeviceNotification<T> : INotification<T>
        where T : DeviceDTO
    {
        public DeviceNotification(T deviceInfo, string type = "Update")
        {
            this.DeviceInfo = deviceInfo;
            this.Type = type;
        } 

        [DataMember]
        public T DeviceInfo { get; private set; }
        public bool HasDeviceInfo => this.DeviceInfo != null;
        [DataMember]
        public string Type { get; private set; }
    }
}
