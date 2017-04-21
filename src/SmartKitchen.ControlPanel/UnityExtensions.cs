using System;
using HSR.CloudSolutions.SmartKitchen.ControlPanel.Communication;
using HSR.CloudSolutions.SmartKitchen.ControlPanel.Communication.Azure;
using HSR.CloudSolutions.SmartKitchen.ControlPanel.Communication.WCF;
using HSR.CloudSolutions.SmartKitchen.ControlPanel.ViewModels;
using HSR.CloudSolutions.SmartKitchen.Devices;
using HSR.CloudSolutions.SmartKitchen.Util.Serializer;
using Microsoft.Practices.Unity;
using System.Configuration;
using SmartKitchen.Azure;
using Microsoft.Azure;

namespace HSR.CloudSolutions.SmartKitchen.ControlPanel
{
    public static class UnityExtensions
    {
        public static void Setup(this IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            //service bus
            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            string commandTopic = ConfigurationManager.AppSettings["ServiceBus.CommandTopic"];
            string notificationTopic = ConfigurationManager.AppSettings["ServiceBus.NotificationTopic"];

            container.RegisterInstance<ICommandTopicProvider>(new CommandTopicProvider(connectionString, commandTopic));
            container.RegisterInstance<INotificationTopicProvider>(new NotificationTopicProvider(connectionString, notificationTopic));

            //EF dbconnectionstring
            string dbConnectionString = ConfigurationManager.AppSettings["EntityFrameworkConnectionString"];
            var context = new DeviceDbContext(dbConnectionString);
            container.RegisterInstance<IDeviceDbContext>(context);

            container.RegisterType<MainWindow>(new ContainerControlledLifetimeManager());
            container.RegisterType<MainWindowViewModel>(new ContainerControlledLifetimeManager());

            container.RegisterType<IDialogService, DialogService>();

            container.RegisterType<ISmartKitchenControlPanelInfoClient, AzureSmartKitchenControlPanelInfoClient>();
            container.RegisterType(typeof(ISmartKitchenControlPanelDeviceClient<>), typeof(AzureSmartKitchenControlPanelDeviceClient<>));


            container.RegisterType<IDeviceSerializer, DeviceSerializer>();
            //using JSON serializer
            container.RegisterType<ISerializer, JsonSerializer>();

            //device controller
            container.RegisterType(typeof(IDeviceControllerViewModel<>), typeof(UnknownDeviceControllerViewModel));
            container.RegisterType<IDeviceControllerViewModel<FridgeDTO>, FridgeControllerViewModel>();
            container.RegisterType<IDeviceControllerViewModel<OvenDTO>, OvenControllerViewModel>();
            container.RegisterType<IDeviceControllerViewModel<StoveDTO>, StoveControllerViewModel>();
           
        }
    }
}
