using System;
using System.Threading.Tasks;
using HSR.CloudSolutions.SmartKitchen.Devices;
using HSR.CloudSolutions.SmartKitchen.Simulator.Communication;
using HSR.CloudSolutions.SmartKitchen.Simulator.Simulation.Devices;
using Microsoft.Practices.ServiceLocation;

namespace HSR.CloudSolutions.SmartKitchen.Simulator.Devices
{
    public static class SimulatorDeviceControllerExtensions
    {
        public static async Task RegisterAsync<T>(this ISimDevice<T> device, IServiceLocator serviceLocator)
            where T : DeviceDTO
        {
            if (device == null)
            {
                return;
            }
            if (serviceLocator == null)
            {
                throw new ArgumentNullException(nameof(serviceLocator));
            }
            var infoClient = serviceLocator.GetInstance<ISmartKitchenSimulatorInfoClient<T>>();
            var deviceClient = serviceLocator.GetInstance<ISmartKitchenSimulatorDeviceClient<T>>();
            await device.RegisterAsync(new SimulatorDeviceController<T>(infoClient, deviceClient));
        }
    }
}
