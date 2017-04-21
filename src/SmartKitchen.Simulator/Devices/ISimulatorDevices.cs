using System.Collections.Generic;
using System.Collections.Specialized;

namespace HSR.CloudSolutions.SmartKitchen.Simulator.Devices
{
    public interface ISimulatorDevices : IEnumerable<ISimulatorDevice>, INotifyCollectionChanged
    {
        
    }
}
