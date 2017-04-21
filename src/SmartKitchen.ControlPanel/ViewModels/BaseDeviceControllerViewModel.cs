using System;
using System.Threading.Tasks;
using System.Timers;
using HSR.CloudSolutions.SmartKitchen.ControlPanel.Communication;
using HSR.CloudSolutions.SmartKitchen.Devices;
using HSR.CloudSolutions.SmartKitchen.Devices.Communication;
using HSR.CloudSolutions.SmartKitchen.UI.ViewModels;

namespace HSR.CloudSolutions.SmartKitchen.ControlPanel.ViewModels
{
    public abstract class BaseDeviceControllerViewModel<T> : BaseViewModel, IDeviceControllerViewModel<T>
        where T : DeviceDTO, new()
    {
        private readonly ISmartKitchenControlPanelDeviceClient<T> _client;
        private readonly Func<DeviceDTO, T> _cast;
        private readonly Timer _notificationUpdateTimer = new Timer(500);
        protected BaseDeviceControllerViewModel(ISmartKitchenControlPanelDeviceClient<T> client, Func<DeviceDTO, T> cast)
        {
            _client = client;
            _cast = cast;

            _notificationUpdateTimer.Elapsed += OnCheckForNotifications;
        }

        protected T Cast(DeviceDTO device)
        {
            return _cast(device);
        }

        protected T Device { get; private set; }

        private DeviceKey _key;
        protected DeviceKey Key
        {
            get { return _key; }
            set
            {
                _key = value;
                Id = _key.Id;
            }
        }

        private Guid _id;
        public Guid Id
        {
            get { return _id; }
            private set
            {
                if (_id == value)
                {
                    return;
                }
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public bool IsControllerFor(DeviceDTO device)
        {
            if (device == null || Key == null)
            {
                return false;
            }
            return Key.Equals(device.Key);
        }

        Task IDeviceControllerViewModel.InitAsync(DeviceDTO config)
        {
            return InitAsync(Cast(config));
        }

        public async Task InitAsync(T config)
        {
            Device = config;
            Key = config?.Key;
            Configure(config);
            await _client.InitAsync(Device);
            _notificationUpdateTimer.Start();
        }

        protected abstract void Configure(T config);

        protected async Task SendCommand(string command)
        {
            if (!_client.IsInitialized)
            {
                return;
            }
            var deviceCommand = new DeviceCommand<T>(command, ToDto());
            await _client.SendCommandAsync(deviceCommand);
        }

        private bool _checking;

        private async void OnCheckForNotifications(object sender, ElapsedEventArgs e)
        {
            if (_checking || !_client.IsInitialized)
            {
                return;
            }
            try
            {
                _checking = true;
                var notification = await _client.CheckNotificationsForAsync(ToDto());
                if (notification is NullNotification<T>)
                {
                    return;
                }
                if (notification.HasDeviceInfo)
                {
                    Update(notification.DeviceInfo);
                }
            }
            finally
            {
                _checking = false;
            }
        }

        public void Update(T update)
        {
            if (!IsControllerFor(update))
            {
                throw new InvalidOperationException("This controller is not responsible for the update.");
            }
            OnUpdate(update);
        }

        protected abstract void OnUpdate(T update);

        protected T ToDto()
        {
            var dto = new T();
            dto.DeviceId = Id;
            Prepare(dto);
            return dto;
        }

        protected abstract void Prepare(T dto);

        protected override void OnDispose()
        {
            _notificationUpdateTimer.Stop();
            _client.Dispose();
            base.OnDispose();
        }
    }
}
