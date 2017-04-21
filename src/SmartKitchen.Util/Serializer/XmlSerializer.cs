using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace HSR.CloudSolutions.SmartKitchen.Util.Serializer
{
    public class XmlSerializer : ISerializer
    {
        private static readonly Encoding Encoding = Encoding.Unicode;

        public string Serialize<T>(T obj, IEnumerable<Type> knownTypes = null)
        {
            var serializer = new DataContractSerializer(typeof(T), knownTypes ?? new List<Type>());
            using (var stream = new MemoryStream())
            {
                using (var writer = XmlWriter.Create(stream, new XmlWriterSettings {Encoding = Encoding}))
                {
                    serializer.WriteObject(writer, obj);
                }
                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public T Deserialize<T>(string serializedDevice, IEnumerable<Type> knownTypes = null)
        {
            var serializer = new DataContractSerializer(typeof(T), knownTypes ?? new List<Type>());
            using (var stream = new MemoryStream(Encoding.GetBytes(serializedDevice)))
            {
                return (T) serializer.ReadObject(stream);
            }
        }
    }
}
