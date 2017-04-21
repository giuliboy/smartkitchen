using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HSR.CloudSolutions.SmartKitchen.Util.Serializer
{
    public class JsonSerializer : ISerializer
    {
        private static readonly Encoding Encoding = Encoding.Unicode;

        private static readonly DataContractJsonSerializerSettings _settings = new DataContractJsonSerializerSettings
        {
            EmitTypeInformation = EmitTypeInformation.Always
        };

        public string Serialize<T>(T obj, IEnumerable<Type> knownTypes = null)
        {
            var settings = this.GetSettingsWith(knownTypes);
            var serializer = new DataContractJsonSerializer(typeof(T), settings);
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, obj);
                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public T Deserialize<T>(string serializedDevice, IEnumerable<Type> knownTypes = null)
        {
            var settings = this.GetSettingsWith(knownTypes);
            var serializer = new DataContractJsonSerializer(typeof(T), settings);
            using (var stream = new MemoryStream(Encoding.GetBytes(serializedDevice)))
            {
                return (T)serializer.ReadObject(stream);
            }
        }

        private DataContractJsonSerializerSettings GetSettingsWith(IEnumerable<Type> knownTypes)
        {
            return new DataContractJsonSerializerSettings()
            {
                KnownTypes = knownTypes ?? new List<Type>(),
                EmitTypeInformation = _settings.EmitTypeInformation
            };
        }
    }
}
