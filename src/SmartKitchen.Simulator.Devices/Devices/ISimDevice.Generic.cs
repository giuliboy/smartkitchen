using System.Threading.Tasks;
using HSR.CloudSolutions.SmartKitchen.Devices;

namespace HSR.CloudSolutions.SmartKitchen.Simulator.Simulation.Devices
{
    public interface ISimDevice<T> : ISimDevice
        where T : DeviceDTO
    {
        T ToDto();
        Task RegisterAsync(IDeviceController<T> controller);
    }
}
