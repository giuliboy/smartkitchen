using System;

namespace HSR.CloudSolutions.SmartKitchen.Simulator.Devices
{
    public class DynamicDeviceIdeFactory : IDeviceIdFactory
    {
        public Guid CreateFridgeId()
        {
            return Guid.NewGuid();
        }

        public Guid CreateOvenId()
        {
            return Guid.NewGuid();
        }

        public Guid CreateStoveId()
        {
            return Guid.NewGuid();
        }
    }
}
