using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using HSR.CloudSolutions.SmartKitchen.ControlPanel.Communication;
using HSR.CloudSolutions.SmartKitchen.ControlPanel.ViewModels;
using HSR.CloudSolutions.SmartKitchen.Devices;
using HSR.CloudSolutions.SmartKitchen.UI.ViewModels;
using Microsoft.Practices.ServiceLocation;

namespace HSR.CloudSolutions.SmartKitchen.ControlPanel
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly IServiceLocator _serviceLocator;
        private readonly ISmartKitchenControlPanelInfoClient _client;
        private readonly Timer _devicesUpdateTimer = new Timer(5000);

        public MainWindowViewModel(IServiceLocator serviceLocator, ISmartKitchenControlPanelInfoClient client)
        {
            _serviceLocator = serviceLocator;
            _client = client;
            _devicesUpdateTimer.Elapsed += OnRequestDeviceUpdate;
        }

        public async Task InitAsync()
        {
            await _client.InitAsync();
            _devicesUpdateTimer.Start();
        }

        private async void OnRequestDeviceUpdate(object sender, ElapsedEventArgs e)
        {
            var devices = (await _client.LoadDevicesAsync()).ToList();
            foreach (var device in devices)
            {
                if (_controllerViewModels.Any(vm => vm.IsControllerFor(device)))
                {
                    continue;
                }
                var controllerVm = await CreateControllerViewModelForAsync(device);
                Add(controllerVm);
            }
            foreach (var controller in _controllerViewModels.ToList())
            {
                if (devices.Any(d => controller.IsControllerFor(d)))
                {
                    continue;
                }
                Remove(controller);
                controller.Dispose();
            }
        }

        private async Task<IDeviceControllerViewModel> CreateControllerViewModelForAsync(DeviceDTO device)
        {
            var viewModelType = typeof (IDeviceControllerViewModel<>).MakeGenericType(device.GetType());
            var viewModel = (IDeviceControllerViewModel)_serviceLocator.GetService(viewModelType);
            await viewModel.InitAsync(device);
            return viewModel;
        }

        private readonly ObservableCollection<IDeviceControllerViewModel> _controllerViewModels  = new ObservableCollection<IDeviceControllerViewModel>();

        private void Add(IDeviceControllerViewModel viewModel)
        {
            Application.Current.Dispatcher.Invoke(() => _controllerViewModels.Add(viewModel));
        }

        private void Remove(IDeviceControllerViewModel viewModel)
        {
            Application.Current.Dispatcher.Invoke(() => _controllerViewModels.Remove(viewModel));
        }

        public IEnumerable<IDeviceControllerViewModel> DeviceControllers => _controllerViewModels;

        protected override void OnDispose()
        {
            _devicesUpdateTimer.Stop();
            _client.Dispose();
            base.OnDispose();
        }
    }
}
