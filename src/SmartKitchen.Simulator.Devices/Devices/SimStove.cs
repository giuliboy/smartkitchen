using System;
using HSR.CloudSolutions.SmartKitchen.Devices;
using HSR.CloudSolutions.SmartKitchen.Devices.Communication;
using HSR.CloudSolutions.SmartKitchen.Simulator.Simulation.Devices.Core;
using HSR.CloudSolutions.SmartKitchen.Simulator.Simulation.Engine;
using HSR.CloudSolutions.SmartKitchen.Util;

namespace HSR.CloudSolutions.SmartKitchen.Simulator.Simulation.Devices
{
    public class SimStove : BaseSimDevice<StoveDTO>
    {
        public SimStove(Guid id, Point coordinate, Point size) : base(id, "Stove", coordinate, size)
        {
            this.HasPan = false;
            this.TargetTemperature = 0;
        }

        protected override void Prepare(StoveDTO dto)
        {
            dto.HasPan = this.HasPan;
            dto.Temperature = this.Temperature;
        }

        protected override void OnCommandReceived(ICommand<StoveDTO> command)
        {
            switch (command.Command)
            {
                case DeviceCommands.ChangeTemperature:
                    {
                        if (command.HasDeviceConfig)
                        {
                            this.ChangeTemperatureTo(command.DeviceConfig.Temperature);
                        }
                        break;
                    }
                case DeviceCommands.EmergencyStop:
                    {
                        this.ChangeTemperatureTo(0);
                        break;
                    }
            }
        }

        public void ChangeTemperatureTo(double temperature)
        {
            this.TargetTemperature = temperature;
        }

        private bool _hasPan;

        public bool HasPan
        {
            get { return this._hasPan; }
            set
            {
                if (this._hasPan == value)
                {
                    return;
                }
                this._hasPan = value;
                this.SimulateTemperatureChange();
                this.OnPropertyChanged(nameof(this.HasPan));
            }
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

        private ISimulation _temperatureSimulation;
        private DeviceTemperatureState _temperatureState = DeviceTemperatureState.None;

        private void SimulateTemperatureChange()
        {
            if (!this.ChangeSimulation() && this.HasPan)
            {
                return;
            }
            this._temperatureSimulation?.Dispose();

            if (this.SimulationTargetTemperature > this.Temperature)
            {
                this._temperatureSimulation = this.Heating(TimeSpan.FromMilliseconds(1000));
            }
            else if (this.SimulationTargetTemperature < this.Temperature)
            {
                this._temperatureSimulation = this.Cooling(TimeSpan.FromMilliseconds(2000));
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
                return this.HasPan
                    ? this.TargetTemperature
                    : 0;
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
            return new IntervalSimulation<double>(dt => this.Temperature += dt, () => 1.0, interval, () => this.Temperature >= this.SimulationTargetTemperature, () => this._temperatureState = DeviceTemperatureState.Heating, () => this._temperatureState = DeviceTemperatureState.None);
        }

        private ISimulation Cooling(TimeSpan interval)
        {
            if (this.Temperature.Equals(this.SimulationTargetTemperature))
            {
                return this.Idle();
            }
            return new IntervalSimulation<double>(dt => this.Temperature -= dt, () => .2, interval, () => this.Temperature <= this.SimulationTargetTemperature, () => this._temperatureState = DeviceTemperatureState.Cooling, () => this._temperatureState = DeviceTemperatureState.None);
        }

        private ISimulation Idle()
        {
            return new NullSimulation(() => this._temperatureState = DeviceTemperatureState.None);
        }
    }
}
