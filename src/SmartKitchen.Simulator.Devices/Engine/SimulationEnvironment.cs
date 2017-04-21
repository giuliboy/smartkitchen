using System.ServiceModel;

namespace HSR.CloudSolutions.SmartKitchen.Simulator.Simulation.Engine
{
    public class SimulationEnvironment
    {
        private static SimulationEnvironment _instance;

        public static SimulationEnvironment Current => _instance ?? (_instance = new SimulationEnvironment());

        public double RoomTemperature => 27;
    }
}
