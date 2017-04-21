using System.Collections.Generic;
using HSR.CloudSolutions.SmartKitchen.Simulator.Simulation.Devices;

namespace HSR.CloudSolutions.SmartKitchen.Simulator.Devices
{
    public interface ISimulatorDeviceCollection : ISimulatorDevices
    {
        void Add(ISimDevice device);
        void Remove(ISimDevice device);

        IEnumerable<ISimDevice> Devices { get; } 
    }
}
