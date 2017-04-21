using System;

namespace HSR.CloudSolutions.SmartKitchen.Simulator.Simulation.Engine
{
    public class NullSimulation : ISimulation
    {
        public NullSimulation(Action action = null)
        {
            action?.Invoke();
        }

        public bool Executing => false;

        public void Dispose()
        {
            
        }

    }
}
