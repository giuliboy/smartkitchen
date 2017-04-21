using System;

namespace HSR.CloudSolutions.SmartKitchen.Simulator.Simulation.Engine
{
    public interface ISimulation : IDisposable
    {
        bool Executing { get; }
    }
}
