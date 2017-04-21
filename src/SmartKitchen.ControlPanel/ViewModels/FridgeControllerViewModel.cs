using System.Timers;
using HSR.CloudSolutions.SmartKitchen.ControlPanel.Communication;
using HSR.CloudSolutions.SmartKitchen.Devices;
using HSR.CloudSolutions.SmartKitchen.Devices.Communication;

namespace HSR.CloudSolutions.SmartKitchen.ControlPanel.ViewModels
{
    public class FridgeControllerViewModel : BaseDeviceControllerViewModel<FridgeDTO>
    {
        public FridgeControllerViewModel(ISmartKitchenControlPanelDeviceClient<FridgeDTO> client) : base(client, d => (FridgeDTO)d)
        {
            
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

        protected override void Configure(FridgeDTO config)
        {
            Door = config.Door;
            Temperature = config.Temperature;
            CurrentTemperature = config.Temperature;
        }

        protected override void OnUpdate(FridgeDTO update)
        {
            Door = update.Door;
            CurrentTemperature = update.Temperature;
        }

        protected override void Prepare(FridgeDTO dto)
        {
            dto.Temperature = Temperature;
        }
    }
}
