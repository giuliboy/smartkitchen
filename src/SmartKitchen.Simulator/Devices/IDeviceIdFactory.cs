using System;

namespace HSR.CloudSolutions.SmartKitchen.Simulator.Devices
{
    public interface IDeviceIdFactory
    {
        Guid CreateFridgeId();
        Guid CreateOvenId();
        Guid CreateStoveId();
    }
}
