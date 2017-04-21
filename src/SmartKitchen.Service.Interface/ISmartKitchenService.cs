using System.ServiceModel;

namespace HSR.CloudSolutions.SmartKitchen.Service.Interface
{
    [ServiceContract]

    public interface ISmartKitchenService : ISmartKitchenControlPanelService, ISmartKitchenSimulatorService
    {
        
    }
}
