using System;
using System.Threading.Tasks;
using System.Timers;
using HSR.CloudSolutions.SmartKitchen.Devices;
using HSR.CloudSolutions.SmartKitchen.Devices.Communication;
using HSR.CloudSolutions.SmartKitchen.Simulator.Communication;
using HSR.CloudSolutions.SmartKitchen.Simulator.Simulation.Devices;

namespace HSR.CloudSolutions.SmartKitchen.Simulator.Devices
{
    public class SimulatorDeviceController<T> : IDeviceController<T>
        where T : DeviceDTO
    {
        public event EventHandler<ICommand<T>> CommandReceived;

        private readonly ISmartKitchenSimulatorInfoClient<T> _infoClient;
        private readonly ISmartKitchenSimulatorDeviceClient<T> _deviceClient; 
        private readonly Timer _commandTimer = new Timer(500);

        public SimulatorDeviceController(ISmartKitchenSimulatorInfoClient<T> infoClient, ISmartKitchenSimulatorDeviceClient<T> deviceClient)
        {
            _infoClient = infoClient;
            _deviceClient = deviceClient;
            _commandTimer.Elapsed += CheckCommands;
        }

        private T _dto;

        public async Task InitAsync(T device)
        {
            _dto = device;
            await _infoClient.InitAsync();
            await _deviceClient.InitAsync(device);

            await _infoClient.RegisterDeviceAsync(_dto);

            _commandTimer.Start();

            IsInitialized = true;
        }

        public bool IsInitialized { get; private set; } = false;

        private bool _checking;

        private async void CheckCommands(object sender, ElapsedEventArgs e)
        {
            if (_checking)
            {
                return;
            }
            try
            {
                _checking = true;
                if (_dto == null)
                {
                    return;
                }
                var command = await _deviceClient.CheckCommandsAsync(_dto);
                if (command is NullCommand<T>)
                {
                    return;
                }
                CommandReceived?.Invoke(this, command);
            }
            finally
            {
                _checking = false;
            }
        }

        public async void Send(INotification<T> notification)
        {
            await _deviceClient.SendNotificationAsync(notification);
        }

        public void Dispose()
        {
            if (_dto != null)
            {
                Task.Run(() => _infoClient.UnregisterDeviceAsync(_dto)).Wait();
            }
            _commandTimer.Stop();

            _deviceClient.Dispose();
            _infoClient.Dispose();
        }
    }
}
