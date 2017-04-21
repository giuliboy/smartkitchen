using System;
using System.Windows.Input;
using HSR.CloudSolutions.SmartKitchen.ControlPanel.Commands;
using HSR.CloudSolutions.SmartKitchen.ControlPanel.Communication;
using HSR.CloudSolutions.SmartKitchen.Devices;
using HSR.CloudSolutions.SmartKitchen.Devices.Communication;

namespace HSR.CloudSolutions.SmartKitchen.ControlPanel.ViewModels
{
    public class OvenControllerViewModel : BaseDeviceControllerViewModel<OvenDTO>
    {
        public OvenControllerViewModel(ISmartKitchenControlPanelDeviceClient<OvenDTO> client) : base(client, d => (OvenDTO)d)
        {
        }

        private ICommand _emergencyShutdownCommand;

        public ICommand EmergencyShutdownCommand => _emergencyShutdownCommand ??
                                                    (_emergencyShutdownCommand = new RelayCommand(Shutdown));

        private void Shutdown(object obj)
        {
            _temperature = 0;
            SendCommand(DeviceCommands.EmergencyStop);
            OnPropertyChanged(nameof(Temperature));
        }

        private DoorState _door;
        public DoorState Door
        {
            get { return _door; }
            private set
            {
                if (_door == value)
                {
                    return;
                }
                _door = value;
                OnPropertyChanged(nameof(Door));
            }
        }

        private double _temperature;
        public double Temperature
        {
            get { return _temperature; }
            set
            {
                if (_temperature == value)
                {
                    return;
                }
                _temperature = value;
                SendCommand(DeviceCommands.ChangeTemperature);
                OnPropertyChanged(nameof(Temperature));
            }
        }

        private double _currentTemperature;

        public double CurrentTemperature
        {
            get { return _currentTemperature; }
            private set
            {
                if (_currentTemperature == value)
                {
                    return;
                }
                _currentTemperature = value;
                OnPropertyChanged(nameof(CurrentTemperature));
            }
        }

        protected override void Configure(OvenDTO config)
        {
            Door = config.Door;
            Temperature = config.Temperature;
            CurrentTemperature = config.Temperature;
        }

        protected override void OnUpdate(OvenDTO update)
        {
            Door = update.Door;
            CurrentTemperature = update.Temperature;
        }

        protected override void Prepare(OvenDTO dto)
        {
            dto.Temperature = Temperature;
        }
    }
}
