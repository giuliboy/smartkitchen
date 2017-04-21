using System;
using System.Threading.Tasks;

namespace HSR.CloudSolutions.SmartKitchen.Simulator.Simulation.Engine
{
    public class TimerEventArgs : EventArgs
    {
        public TimerEventArgs(TimeSpan timespan)
        {
            this.ElapsedTime = timespan;
        }

        public TimeSpan ElapsedTime { get; }
    }
}
