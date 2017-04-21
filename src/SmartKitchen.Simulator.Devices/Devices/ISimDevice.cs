using System;
using System.ComponentModel;
using System.Threading.Tasks;
using HSR.CloudSolutions.SmartKitchen.Util;

namespace HSR.CloudSolutions.SmartKitchen.Simulator.Simulation.Devices
{
    public interface ISimDevice : IDisposable, INotifyPropertyChanged
    {
        Point Coordinate { get; }
        Point Size { get; }


        Task UnregisterAsync();
    }
}
