using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HSR.CloudSolutions.SmartKitchen.Devices;

namespace HSR.CloudSolutions.SmartKitchen.ControlPanel.Communication
{
    public interface ISmartKitchenControlPanelInfoClient : IDisposable
    {
        Task InitAsync();

        Task<IEnumerable<DeviceDTO>> LoadDevicesAsync();

    }
}
