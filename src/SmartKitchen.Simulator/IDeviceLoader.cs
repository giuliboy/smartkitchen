using System.Collections.Generic;
using System.Threading.Tasks;
using HSR.CloudSolutions.SmartKitchen.Simulator.Simulation.Devices;

namespace HSR.CloudSolutions.SmartKitchen.Simulator
{
    public interface IDeviceLoader
    {
        Task<IEnumerable<ISimDevice>> LoadDevicesAsync();
    }
}
