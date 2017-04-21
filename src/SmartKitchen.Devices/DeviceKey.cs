using System;
using System.Runtime.Serialization;

namespace HSR.CloudSolutions.SmartKitchen.Devices
{
    [DataContract]
    public class DeviceKey
    {
        public DeviceKey(Type type, Guid id)
        {
            this.Type = type;
            this.Id = id;
        }

        [DataMember]
        public Type Type { get; }
        [DataMember]
        public Guid Id { get; }

        public override bool Equals(object obj)
        {
            var other = obj as DeviceKey;
            if (other == null)
            {
                return false;
            }
            return this.Type == other.Type 
                && this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return $"{this.Type.FullName}{this.Id}".GetHashCode();
        }

        public override string ToString()
        {
            return $"{this.Type.Name}_{this.Id}";
        }
    }
}
