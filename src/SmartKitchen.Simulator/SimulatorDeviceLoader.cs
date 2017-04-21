using System.Collections.Generic;
using System.Threading.Tasks;
using HSR.CloudSolutions.SmartKitchen.Devices;
using HSR.CloudSolutions.SmartKitchen.Simulator.Devices;
using HSR.CloudSolutions.SmartKitchen.Simulator.Simulation.Devices;
using HSR.CloudSolutions.SmartKitchen.Util;
using Microsoft.Practices.ServiceLocation;

namespace HSR.CloudSolutions.SmartKitchen.Simulator
{
    public class SimulatorDeviceLoader : IDeviceLoader
    {
        private readonly IServiceLocator _serviceLocator;
        private readonly IDeviceIdFactory _idFactory;

        public SimulatorDeviceLoader(IServiceLocator serviceLocator, IDeviceIdFactory idFactory)
        {
            _serviceLocator = serviceLocator;
            _idFactory = idFactory;
        }

        public async Task<IEnumerable<ISimDevice>> LoadDevicesAsync()
        {
            var devices = new List<ISimDevice>();

            devices.Add(await CreateFridgeAsync());
            devices.Add(await CreateOvenAsync());
            devices.Add(await CreateStoveAsync());

            return devices;
        }

        private async Task<ISimDevice> CreateFridgeAsync()
        {
            var topLeft = new Point(.096, .281);
            var bottomright = new Point(.226, .658);
            var size = GetSize(topLeft, bottomright);

            var fridge = new SimFridge(_idFactory.CreateFridgeId(), topLeft, size);
            fridge.ConfigureWith(new FridgeDTO {Temperature = 5});
            await fridge.RegisterAsync(_serviceLocator);

            return fridge;
        }

        private async Task<ISimDevice> CreateOvenAsync()
        {
            var topLeft = new Point(.470, .615);
            var bottomRight = new Point(.610, .890);
            var size = GetSize(topLeft, bottomRight);

            var oven = new SimOven(_idFactory.CreateOvenId(), topLeft, size);
            await oven.RegisterAsync(_serviceLocator);

            return oven;
        }

        private async Task<ISimDevice> CreateStoveAsync()
        {
            var topLeft = new Point(.430, .420);
            var bottomRight = new Point(.615, .597);
            var size = GetSize(topLeft, bottomRight);

            var stove = new SimStove(_idFactory.CreateStoveId(), topLeft, size);
            await stove.RegisterAsync(_serviceLocator);

            return stove;
        }

        private Point GetSize(Point topLeft, Point bottomRight)
        {
            return new Point(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);
        }
    }
}
