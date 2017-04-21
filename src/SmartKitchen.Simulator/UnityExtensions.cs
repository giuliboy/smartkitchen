using System;
using HSR.CloudSolutions.SmartKitchen.Simulator.Communication;
using HSR.CloudSolutions.SmartKitchen.Simulator.Communication.Azure;
using HSR.CloudSolutions.SmartKitchen.Simulator.Devices;
using HSR.CloudSolutions.SmartKitchen.Util.Serializer;
using Microsoft.Practices.Unity;
using SmartKitchen.Azure;
using System.Configuration;
using Microsoft.Azure;

namespace HSR.CloudSolutions.SmartKitchen.Simulator
{
    public static class UnityExtensions
    {
        public static void Setup(this IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            //EF dbconnectionstring
            string dbConnectionString = ConfigurationManager.AppSettings["EntityFrameworkConnectionString"];
            var context = new DeviceDbContext(dbConnectionString);
            container.RegisterInstance<IDeviceDbContext>(context);

            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            string commandTopic = ConfigurationManager.AppSettings["ServiceBus.CommandTopic"];
            string notificationTopic = ConfigurationManager.AppSettings["ServiceBus.NotificationTopic"];

            container.RegisterInstance<ICommandTopicProvider>(new CommandTopicProvider(connectionString, commandTopic));
            container.RegisterInstance<INotificationTopicProvider>(new NotificationTopicProvider(connectionString, notificationTopic));

            container.RegisterType<MainWindow>(new ContainerControlledLifetimeManager());
            container.RegisterType<MainWindowViewModel>(new ContainerControlledLifetimeManager());

            container.RegisterType<IDialogService, DialogService>();

            container.RegisterType(typeof(ISmartKitchenSimulatorInfoClient<>), typeof(AzureSmartKitchenSimulatorInfoClient<>));
            container.RegisterType(typeof(ISmartKitchenSimulatorDeviceClient<>), typeof(AzureSmartKitchenSimulatorDeviceClient<>));

            container.RegisterType<IDeviceSerializer, DeviceSerializer>();
            container.RegisterType<ISerializer, JsonSerializer>();

            container.RegisterType<ISimulatorDeviceCollection, SimulatorDeviceCollection>();
            container.RegisterType<ISimulatorDeviceFactory, SimulatorDeviceFactory>();
            container.RegisterType<IDeviceLoader, SimulatorDeviceLoader>();
            
            container.RegisterType<IDeviceIdFactory, DynamicDeviceIdeFactory>();
           
            
        }
    }
}
