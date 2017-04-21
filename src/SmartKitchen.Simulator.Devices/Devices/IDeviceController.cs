using System;
using System.Threading.Tasks;
using HSR.CloudSolutions.SmartKitchen.Devices;
using HSR.CloudSolutions.SmartKitchen.Devices.Communication;

namespace HSR.CloudSolutions.SmartKitchen.Simulator.Simulation.Devices
{
    public interface IDeviceController<T> : IDisposable
        where T : DeviceDTO
    {
        Task InitAsync(T device);
        bool IsInitialized { get; }

        event EventHandler<ICommand<T>> CommandReceived; 
        void Send(INotification<T> notification);
    }
}
