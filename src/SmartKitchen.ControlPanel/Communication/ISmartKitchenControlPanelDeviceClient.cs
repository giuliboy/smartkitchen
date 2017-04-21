using System;
using System.Threading.Tasks;
using HSR.CloudSolutions.SmartKitchen.Devices;
using HSR.CloudSolutions.SmartKitchen.Devices.Communication;

namespace HSR.CloudSolutions.SmartKitchen.ControlPanel.Communication
{
    public interface ISmartKitchenControlPanelDeviceClient<T> : IDisposable where T : DeviceDTO
    {
        Task InitAsync(T device);
        bool IsInitialized { get; }

        Task SendCommandAsync(ICommand<T> command);
        Task<INotification<T>> CheckNotificationsForAsync(T device);
    }
}
