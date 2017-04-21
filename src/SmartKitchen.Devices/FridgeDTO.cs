using System.Runtime.Serialization;

namespace HSR.CloudSolutions.SmartKitchen.Devices
{
    [DataContract]
    public class FridgeDTO : DeviceDTO
    {
        [DataMember]
        public double Temperature { get; set; }
        [DataMember]
        public DoorState Door { get; set; }
    }
}
