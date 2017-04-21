using System;
using System.Threading.Tasks;
using HSR.CloudSolutions.SmartKitchen.Devices;

namespace HSR.CloudSolutions.SmartKitchen.ControlPanel.ViewModels
{
    public interface IDeviceControllerViewModel<in T> : IDeviceControllerViewModel
        where T : DeviceDTO
    {
        Guid Id { get; }

        Task InitAsync(T config);
        void Update(T update);
    }
}
