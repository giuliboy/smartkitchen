using System;
using System.ComponentModel;
using System.Threading.Tasks;
using HSR.CloudSolutions.SmartKitchen.Devices;
using HSR.CloudSolutions.SmartKitchen.Devices.Communication;
using HSR.CloudSolutions.SmartKitchen.Util;

namespace HSR.CloudSolutions.SmartKitchen.Simulator.Simulation.Devices.Core
{
    public abstract class BaseSimDevice<T> : ISimDevice<T>
        where T : DeviceDTO, new()
    {
        protected BaseSimDevice(Guid id, string label, Point coordinate, Point size)
        {
            this.Id = id;
            this.Label = label;
            this.Coordinate = coordinate;
            this.Size = size;
        }

        public Guid Id { get; }

        public string Label { get; }
        
        public Point Coordinate { get; }
        public Point Size { get; }

        public T ToDto()
        {
            var dto = new T {DeviceId = this.Id};
            this.Prepare(dto);
            return dto;
        }

        protected abstract void Prepare(T dto);

        private IDeviceController<T> _controller; 

        public async Task RegisterAsync(IDeviceController<T> controller)
        {
            if (this._controller == controller)
            {
                return;
            }
            await this.UnregisterAsync();
            this._controller = controller;
            this._controller.CommandReceived += this.OnCommandReceived;
            await this._controller.InitAsync(this.ToDto());
        }

        public async Task UnregisterAsync()
        {
            if (this._controller == null)
            {
                return;
            }
            this._controller.CommandReceived -= this.OnCommandReceived;
            this._controller.Dispose();
        }

        private void OnCommandReceived(object sender, ICommand<T> command)
        {
            this.OnCommandReceived(command);
        }

        protected abstract void OnCommandReceived(ICommand<T> command);

        protected bool Send(INotification<T> notification)
        {
            if (notification == null || this._controller?.IsInitialized != true)
            {
                return false;
            }
            this._controller.Send(notification);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.Send(new DeviceNotification<T>(this.ToDto()));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            Task.Run(this.UnregisterAsync).Wait();
            this.OnDispose();
        }

        protected virtual void OnDispose()
        {
            
        }
    }
}
