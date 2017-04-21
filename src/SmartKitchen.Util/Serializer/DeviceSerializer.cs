using System;
using System.Collections.Generic;
using HSR.CloudSolutions.SmartKitchen.Devices;
using HSR.CloudSolutions.SmartKitchen.Devices.Communication;

namespace HSR.CloudSolutions.SmartKitchen.Util.Serializer
{
    public class DeviceSerializer : IDeviceSerializer
    {
        private readonly IEnumerable<Type> KnownTypes = new List<Type>
        {
            typeof (FridgeDTO),
            typeof (OvenDTO),
            typeof (StoveDTO),
            typeof (NullCommand),
            typeof (DeviceCommand<FridgeDTO>),
            typeof (DeviceCommand<OvenDTO>),
            typeof (DeviceCommand<StoveDTO>),
            typeof (NullNotification),
            typeof (DeviceNotification<FridgeDTO>),
            typeof (DeviceNotification<OvenDTO>),
            typeof (DeviceNotification<StoveDTO>),
        };

        private readonly ISerializer _serializer;

        public DeviceSerializer(ISerializer serializer)
        {
            this._serializer = serializer;
        }


        public string Serialize<T>(T dto) where T : DeviceDTO
        {
            return this._serializer.Serialize(dto, this.KnownTypes);
        }

        public T DeserializeDto<T>(string serializedDto) where T : DeviceDTO
        {
            return this._serializer.Deserialize<T>(serializedDto, this.KnownTypes);
        }

        public string Serialize<T>(ICommand<T> command) where T : DeviceDTO
        {
            return this._serializer.Serialize(command, this.KnownTypes);
        }

        public ICommand<T> DeserializeCommand<T>(string serializedCommand) where T : DeviceDTO
        {
            return this._serializer.Deserialize<ICommand<T>>(serializedCommand, this.KnownTypes);
        }

        public string Serialize<T>(INotification<T> notification) where T : DeviceDTO
        {
            return this._serializer.Serialize(notification, this.KnownTypes);
        }

        public INotification<T> DeserializeNotification<T>(string serializedNotification) where T : DeviceDTO
        {
            return this._serializer.Deserialize<INotification<T>>(serializedNotification, this.KnownTypes);
        }

    }
}
