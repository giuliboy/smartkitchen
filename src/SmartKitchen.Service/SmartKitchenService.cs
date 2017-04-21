using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using HSR.CloudSolutions.SmartKitchen.Devices;
using HSR.CloudSolutions.SmartKitchen.Devices.Communication;
using HSR.CloudSolutions.SmartKitchen.Service.Interface;
using HSR.CloudSolutions.SmartKitchen.Util;

namespace HSR.CloudSolutions.SmartKitchen.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class SmartKitchenService : ISmartKitchenService
    {
        private readonly ReaderWriterLockSlim _deviceLock = new ReaderWriterLockSlim();
        private readonly List<DeviceDTO> _devices = new List<DeviceDTO>(); 

        public async Task RegisterDeviceAsync(DeviceDTO device)
        {
            if (device == null)
            {
                return;
            }
            await Task.Run(() =>
            {
                using (new AutoWriteLock(this._deviceLock))
                {
                    this.Log("Register", device);
                    if(this._devices.Any(d => d.Key.Equals(device.Key)))
                    {
                        return;
                    }
                    this._devices.Add(device);
                }
            });
        }

        public async Task UnregisterDeviceAsync(DeviceDTO device)
        {
            if (device == null)
            {
                return;
            }
            await Task.Run(() =>
            {
                using (new AutoWriteLock(this._deviceLock))
                {
                    this.Log("Unregister", device);
                    this._devices.RemoveAll(d => d.Key.Equals(device.Key));
                }
            });
        }

        public async Task<IEnumerable<DeviceDTO>> GetRegisteredDevicesAsync()
        {
            return await Task.Run(() =>
            {
                using (new AutoReadLock(this._deviceLock))
                {
                    this.Log("Reading devices");
                    return this._devices.ToList();
                }
            });
        }

        private readonly ReaderWriterLockSlim _commandLock = new ReaderWriterLockSlim();
        private readonly IDictionary<DeviceKey, Queue<ICommand<DeviceDTO>>> _commandQueues = new Dictionary<DeviceKey, Queue<ICommand<DeviceDTO>>>(); 
        public async Task SendCommandAsync(ICommand<DeviceDTO> command)
        {
            if (command == null || !command.HasDeviceConfig)
            {
                return;
            }
            await Task.Run(() =>
            {
                using (new AutoWriteLock(this._commandLock))
                {
                    var queue = this.GetQueue(command.DeviceConfig, this._commandQueues);
                    this.Log("Sending command", command);
                    queue.Enqueue(command);
                }
            });
        }

        public async Task<ICommand<DeviceDTO>> ReceiveCommandAsync(DeviceDTO device)
        {
            if (device == null)
            {
                return new NullCommand();
            }
            return await Task.Run(() =>
            {
                using (new AutoWriteLock(this._commandLock))
                {
                    Queue<ICommand<DeviceDTO>> queue;
                    if (!this._commandQueues.TryGetValue(device.Key, out queue))
                    {
                        return new NullCommand();
                    }
                    this.Log("Checking commands", device);
                    return queue.Any() ? queue.Dequeue() : new NullCommand();
                }
            });
        }

        private readonly ReaderWriterLockSlim _notificaionLock = new ReaderWriterLockSlim();
        private readonly IDictionary<DeviceKey, Queue<INotification<DeviceDTO>>> _notificationQueues = new Dictionary<DeviceKey, Queue<INotification<DeviceDTO>>>(); 

        public async Task PublishNotificationAsync(INotification<DeviceDTO> update)
        {
            if (update == null || !update.HasDeviceInfo)
            {
                return;
            }
            await Task.Run(() =>
            {
                using (new AutoWriteLock(this._notificaionLock))
                {
                    var queue = this.GetQueue(update.DeviceInfo, this._notificationQueues);
                    this.Log("Publish notification", update);
                    queue.Enqueue(update);
                }
            });
        }

        public async Task<INotification<DeviceDTO>> PeekNotificationAsync(DeviceDTO device)
        {
            if (device == null)
            {
                return new NullNotification();
            }
            return await Task.Run(() =>
            {
                using (new AutoWriteLock(this._notificaionLock))
                {
                    Queue<INotification<DeviceDTO>> queue;
                    if (!this._notificationQueues.TryGetValue(device.Key, out queue))
                    {
                        return new NullNotification();
                    }
                    this.Log("Checking notifications", device);
                    return queue.Any() ? queue.Dequeue() : new NullNotification();
                }
            });
        }

        private Queue<T> GetQueue<T>(DeviceDTO device, IDictionary<DeviceKey, Queue<T>> queues)
        {
            Queue<T> queue;
            if (!queues.TryGetValue(device.Key, out queue))
            {
                queue = new Queue<T>();
                queues.Add(device.Key, queue);
            }
            return queue;
        }

        private void Log(string message, object obj)
        {
            var typeName = "{null}";
            if (obj != null)
            {
                typeName = obj.GetType().Name;
            }
            Console.WriteLine($"{DateTime.Now.ToString("yyyy MM dd - HH:mm:ss.ffff")}: {message} => {typeName}");
        }

        private void Log(string message)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy MM dd - HH:mm:ss.ffff")}: {message}");
        }
    }
}
