using System.Runtime.Serialization;

namespace HSR.CloudSolutions.SmartKitchen.Devices
{
    [DataContract]
    public class StoveDTO : DeviceDTO
    {
        [DataMember]
        public bool HasPan { get; set; }

        [DataMember]
        public double Temperature { get; set; }
    }
}
