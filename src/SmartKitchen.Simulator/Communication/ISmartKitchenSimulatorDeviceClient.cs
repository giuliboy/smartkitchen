using System;
using System.Threading.Tasks;
using HSR.CloudSolutions.SmartKitchen.Devices;
using HSR.CloudSolutions.SmartKitchen.Devices.Communication;

namespace HSR.CloudSolutions.SmartKitchen.Simulator.Communication
{
    public interface ISmartKitchenSimulatorDeviceClient<T> : IDisposable
        where T : DeviceDTO
    {
        Task InitAsync(T device);
        
        Task<ICommand<T>> CheckCommandsAsync(T device);
        Task SendNotificationAsync(INotification<T> notification);
    }
}
