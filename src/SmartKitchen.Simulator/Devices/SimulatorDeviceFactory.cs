using HSR.CloudSolutions.SmartKitchen.Simulator.Simulation.Devices;
using HSR.CloudSolutions.SmartKitchen.Simulator.ViewModels;

namespace HSR.CloudSolutions.SmartKitchen.Simulator.Devices
{
    public class SimulatorDeviceFactory : ISimulatorDeviceFactory
    {
        public ISimulatorDevice CreateViewModelFor<T>(T device) where T : ISimDevice
        {
            return new SimulatorDeviceViewModel<T>(device);
        }
    }
}
