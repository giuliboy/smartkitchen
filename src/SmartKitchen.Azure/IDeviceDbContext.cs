using HSR.CloudSolutions.SmartKitchen.Devices;
using System.Linq;

namespace SmartKitchen.Azure
{
    /// <summary>
    /// CRUD Interface for devices
    /// </summary>
    public interface IDeviceDbContext
    {
        DeviceDTO CreateDevice(DeviceDTO device);
        void RemoveDevice(DeviceDTO device);
        DeviceDTO GetDevice(string id);
        IQueryable<DeviceDTO> GetDevices();
    }

}