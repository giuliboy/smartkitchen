using HSR.CloudSolutions.SmartKitchen.Util;

namespace HSR.CloudSolutions.SmartKitchen.Simulator.Devices
{
    public interface ISimulatorDevice
    {
        Point Coordinate { get; }
        Point Size { get; }

        object Device { get; }
    }
}
