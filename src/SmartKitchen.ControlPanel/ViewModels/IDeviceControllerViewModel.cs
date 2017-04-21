using System;
using System.Threading.Tasks;
using HSR.CloudSolutions.SmartKitchen.Devices;

namespace HSR.CloudSolutions.SmartKitchen.ControlPanel.ViewModels
{
    public interface IDeviceControllerViewModel : IDisposable
    {
        bool IsControllerFor(DeviceDTO device);
        Task InitAsync(DeviceDTO config);
    }
}
