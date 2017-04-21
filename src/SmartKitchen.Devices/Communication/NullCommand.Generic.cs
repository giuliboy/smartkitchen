using System.Runtime.Serialization;

namespace HSR.CloudSolutions.SmartKitchen.Devices.Communication
{
    [DataContract]
    public class NullCommand<T> : ICommand<T> where T : DeviceDTO
    {
        [DataMember]
        public string Command { get; private set; } = "Nothing";
        [DataMember]
        public T DeviceConfig { get; private set; } = null;
        [DataMember]
        public bool HasDeviceConfig { get; private set; } = false;
    }
}
