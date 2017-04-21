using System;
using HSR.CloudSolutions.SmartKitchen.ControlPanel.Communication;
using HSR.CloudSolutions.SmartKitchen.Devices;
using HSR.CloudSolutions.SmartKitchen.Devices.Communication;

namespace HSR.CloudSolutions.SmartKitchen.ControlPanel.ViewModels
{
    public class StoveControllerViewModel : BaseDeviceControllerViewModel<StoveDTO>
    {
        public StoveControllerViewModel(ISmartKitchenControlPanelDeviceClient<StoveDTO> client) : base(client, d => (StoveDTO)d)
        {
        }

        private bool _hasPan;

        public bool HasPan
        {
            get { return _hasPan; }
            private set
            {
                if (_hasPan == value)
                {
                    return;
                }
                _hasPan = value;
                OnPropertyChanged(nameof(HasPan));
            }
        }

        private const double TemperatureStepSize = 15.0;
        public int TemperatureStep
        {
            get { return (int) Math.Round(Temperature / TemperatureStepSize); }
            set
            {
                if (TemperatureStep == value)
                {
                    return;
                }
                Temperature = value*TemperatureStepSize;
            }
        }

        private double _temperature;
        public double Temperature
        {
            get { return _temperature; }
            private set
            {
                if (_temperature == value)
                {
                    return;
                }
                _temperature = value;
                SendCommand(DeviceCommands.ChangeTemperature);
                OnPropertyChanged(nameof(Temperature));
                OnPropertyChanged(nameof(TemperatureStep));
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



        protected override void Configure(StoveDTO config)
        {
            HasPan = config.HasPan;
            Temperature = config.Temperature;
        }

        protected override void OnUpdate(StoveDTO update)
        {
            HasPan = update.HasPan;
            CurrentTemperature = update.Temperature;
        }

        protected override void Prepare(StoveDTO dto)
        {
            dto.Temperature = Temperature;
        }
    }
}
