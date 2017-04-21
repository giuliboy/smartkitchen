using System;
using System.Collections.Generic;

namespace HSR.CloudSolutions.SmartKitchen.Util.Serializer
{
    public interface ISerializer
    {
        string Serialize<T>(T obj, IEnumerable<Type> knownTypes = null);
        T Deserialize<T>(string serializedDevice, IEnumerable<Type> knownTypes = null);
    }
}
