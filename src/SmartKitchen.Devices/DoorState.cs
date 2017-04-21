using System.Runtime.Serialization;

namespace HSR.CloudSolutions.SmartKitchen.Devices
{
    [DataContract]
    public enum DoorState
    {
        [EnumMember]
        Closed = 0,
        [EnumMember]
        Open = 1,
    }
}
