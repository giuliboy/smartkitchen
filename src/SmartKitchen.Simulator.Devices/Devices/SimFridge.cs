using System;
using HSR.CloudSolutions.SmartKitchen.Devices;
using HSR.CloudSolutions.SmartKitchen.Devices.Communication;
using HSR.CloudSolutions.SmartKitchen.Simulator.Simulation.Devices.Core;
using HSR.CloudSolutions.SmartKitchen.Simulator.Simulation.Engine;
using HSR.CloudSolutions.SmartKitchen.Util;

namespace HSR.CloudSolutions.SmartKitchen.Simulator.Simulation.Devices
{
    public class SimFridge : BaseSimDevice<FridgeDTO>
    {
        public SimFridge(Guid id, Point coordinate, Point size) 
            : base(id, "Fridge", coordinate, size)
        {
            this._targetTemperature = 0;
            this._temperature = 0;
            this._door = DoorState.Closed;
        }

        public void ConfigureWith(FridgeDTO dto)
        {
            if (dto == null)
            {
                return;
            }
            this.Temperature = dto.Temperature;
            this.TargetTemperature = dto.Temperature;
            this.Door = dto.Door;
        }

        protected override void Prepare(FridgeDTO dto)
        {
            dto.Temperature = this.Temperature;
            dto.Door = this.Door;
        }

        protected override void OnCommandReceived(ICommand<FridgeDTO> command)
        {
            if (command.Command == DeviceCommands.ChangeTemperature)
            {
                if (!command.HasDeviceConfig)
                {
                    return;
                }
                this.ChangeTemperatureTo(command.DeviceConfig.Temperature);
            }
        }

        public void ChangeTemperatureTo(double temperature)
        {
            this.TargetTemperature = temperature;
        }

        private double _targetTemperature;

        public double TargetTemperature
        {
            get { return this._targetTemperature; }
            private set
            {
                if (this._targetTemperature == value)
                {
                    return;
                }
                this._targetTemperature = value;
                this.SimulateTemperatureChange();
                this.OnPropertyChanged(nameof(this.TargetTemperature));
            }
        }

        private double _temperature;

        public double Temperature
        {
            get { return this._temperature; }
            private set
            {
                if (this._temperature == value)
                {
                    return;
                }
                this._temperature = value;
                this.OnPropertyChanged(nameof(this.Temperature));
            }
        }

        private DoorState _door;

        public DoorState Door
        {
            get { return this._door; }
            set
            {
                if (this._door == value)
                {
                    return;
                }
                this._door = value;
                this.SimulateTemperatureChange();
                this.OnPropertyChanged(nameof(this.Door));
            }
        }

        private ISimulation _temperatureSimulation;
        private DeviceTemperatureState _temperatureState = DeviceTemperatureState.None;

        private void SimulateTemperatureChange()
        {
            if (!this.ChangeSimulation() && this.Door == DoorState.Closed)
            {
                return;
            }
            this._temperatureSimulation?.Dispose();
            TimeSpan interval = TimeSpan.FromMilliseconds(1000);
            if (this.Door == DoorState.Open)
            {
                interval = TimeSpan.FromMilliseconds(2000);
            }
            
            if (this.SimulationTargetTemperature > this.Temperature)
            {
                this._temperatureSimulation = this.Heating(interval);
            }
            else if (this.SimulationTargetTemperature < this.Temperature)
            {
                this._temperatureSimulation = this.Cooling(interval);
            }
            else
            {
                this._temperatureSimulation = this.Idle();
            }
        }

        private double SimulationTargetTemperature
        {
            get
            {
                return this.Door == DoorState.Closed
                    ? this.TargetTemperature
                    : SimulationEnvironment.Current.RoomTemperature;
            }
        }

        private bool ChangeSimulation()
        {
            switch (this._temperatureState)
            {
                case DeviceTemperatureState.Heating:
                    return this.TargetTemperature <= this.Temperature;
                case DeviceTemperatureState.Cooling:
                    return this.TargetTemperature >= this.Temperature;
                default:
                    return true;
            }
        }

        private ISimulation Heating(TimeSpan interval)
        {
            if (this.Temperature.Equals(this.SimulationTargetTemperature))
            {
                return this.Idle();
            }
            return new IntervalSimulation<double>(dt => this.Temperature += dt, () => .1, interval, () => this.Temperature >= this.SimulationTargetTemperature, () => this._temperatureState = DeviceTemperatureState.Heating, () => this._temperatureState = DeviceTemperatureState.None);
        }

        private ISimulation Cooling(TimeSpan interval)
        {
            if (this.Temperature.Equals(this.SimulationTargetTemperature))
            {
                return this.Idle();
            }
            return new IntervalSimulation<double>(dt => this.Temperature -= dt, () => .1, interval, () => this.Temperature <= this.SimulationTargetTemperature, () => this._temperatureState = DeviceTemperatureState.Cooling, () => this._temperatureState = DeviceTemperatureState.None);
        }

        private ISimulation Idle()
        {
            return new NullSimulation(() => this._temperatureState = DeviceTemperatureState.None);
        }
    }
}
