namespace HSR.CloudSolutions.SmartKitchen.Devices.Communication
{
    public interface ICommand<out T>
        where T : DeviceDTO
    {
        string Command { get; }
        T DeviceConfig { get; }
        bool HasDeviceConfig { get; }
    }
}
