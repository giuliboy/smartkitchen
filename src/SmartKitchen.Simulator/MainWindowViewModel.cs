using System;
using System.Linq;
using System.Threading.Tasks;
using HSR.CloudSolutions.SmartKitchen.Simulator.Devices;
using HSR.CloudSolutions.SmartKitchen.Simulator.Simulation.Devices;
using HSR.CloudSolutions.SmartKitchen.UI.ViewModels;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace HSR.CloudSolutions.SmartKitchen.Simulator
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly ISimulatorDeviceCollection _devices;
        private readonly IDeviceLoader _loader;

        public MainWindowViewModel(IServiceLocator serviceLocator, IDeviceLoader loader)
        {
            if (serviceLocator == null)
            {
                throw new ArgumentNullException(nameof(serviceLocator));
            }
            if (loader == null)
            {
                throw new ArgumentNullException(nameof(loader));
            }
            _loader = loader;
            _devices = serviceLocator.GetInstance<ISimulatorDeviceCollection>();
        }

        public ISimulatorDevices Devices => _devices;

        public async Task LoadAsync()
        {
            var devices = await _loader.LoadDevicesAsync();
            if (devices == null)
            {
                return;
            }
            foreach (var device in devices)
            {
                _devices.Add(device);
            }
            OnPropertyChanged(nameof(Devices));
        }

        public async Task UnloadAsync()
        {
            foreach (var device in _devices.Devices)
            {
                await device.UnregisterAsync();
            }
        }
    }
}
