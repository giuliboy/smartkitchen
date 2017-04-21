namespace HSR.CloudSolutions.SmartKitchen.Devices.Communication
{
    public interface INotification<out T>
        where T : DeviceDTO
    {
        T DeviceInfo { get; }
        bool HasDeviceInfo { get; }
       string Type { get; }
    }
}
