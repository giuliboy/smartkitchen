using System.Runtime.Serialization;

namespace HSR.CloudSolutions.SmartKitchen.Devices
{
    [DataContract]
    public class OvenDTO : DeviceDTO
    {
        [DataMember]
        public DoorState Door { get; set; }

        [DataMember]
        public double Temperature { get; set; }
    }
}
