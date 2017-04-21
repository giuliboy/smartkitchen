using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace HSR.CloudSolutions.SmartKitchen.Devices
{
    [DataContract]
    public class DeviceDTO
    {
        /// <summary>
        /// gets the database generated Id for this <see cref="DeviceDTO"/> instance
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        [DataMember]
        public Guid DeviceId { get; set; }

        public DeviceKey Key => new DeviceKey(GetType(), DeviceId);
    }
}
