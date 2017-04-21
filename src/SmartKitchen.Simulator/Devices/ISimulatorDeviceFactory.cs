using HSR.CloudSolutions.SmartKitchen.Simulator.Simulation.Devices;

namespace HSR.CloudSolutions.SmartKitchen.Simulator.Devices
{
    public interface ISimulatorDeviceFactory
    {
        ISimulatorDevice CreateViewModelFor<T>(T device) where T : ISimDevice;
    }
}
