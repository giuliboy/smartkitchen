using System;
using System.Threading.Tasks;
using HSR.CloudSolutions.SmartKitchen.Devices;

namespace HSR.CloudSolutions.SmartKitchen.Simulator.Communication
{
    public interface ISmartKitchenSimulatorInfoClient<in T> : IDisposable
        where T : DeviceDTO
    {
        Task InitAsync();

        Task RegisterDeviceAsync(T device);
        Task UnregisterDeviceAsync(T device);
    }
}
