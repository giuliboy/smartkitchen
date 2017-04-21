using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using HSR.CloudSolutions.SmartKitchen.Devices;
using System.Diagnostics;

namespace SmartKitchen.Azure
{
    public class DeviceDbContext : DbContext, IDeviceDbContext
    {
        public DeviceDbContext(string connectionString)
            : base(connectionString)
        {
            try
            {
                Database.CreateIfNotExists();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        public DbSet<DeviceDTO> Devices { get; set; }

        public DeviceDTO CreateDevice(DeviceDTO device)
        {
            var newDevice = Devices.Add(device);
            SaveChanges();
            return newDevice;
        }

        public DeviceDTO GetDevice(string id)
        {
            return Devices.FirstOrDefault(d => d.DeviceId.ToString() == id);
        }

        public IQueryable<DeviceDTO> GetDevices()
        {
            return Devices;
        }

        public void RemoveDevice(DeviceDTO device)
        {
            var dtr = Devices.FirstOrDefault(d => d.DeviceId.Equals(device.DeviceId));
            if (dtr != null)
            {
                Devices.Remove(dtr);
                SaveChanges();
            }
        }
    }
}
