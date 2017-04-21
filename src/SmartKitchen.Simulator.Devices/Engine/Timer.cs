using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HSR.CloudSolutions.SmartKitchen.Simulator.Simulation.Engine
{
    public class Timer : ITimer
    {
        public event EventHandler<TimerEventArgs> Tick;

        public bool IsRunning { get; private set; } = false;

        private TimeSpan _interval = TimeSpan.Zero;

        public TimeSpan Interval
        {
            get { return this._interval; }
            private set
            {
                if (value < TimeSpan.Zero)
                {
                    throw new ArgumentException(nameof(this.Interval));
                }
                this._interval = value;
            }
        }

        private Task _action;
        private CancellationTokenSource _cancellationHandler;
        

        public Timer()
        {
            
        }

        public Timer(TimeSpan interval)
        {
            this.Interval = interval;
        }

        public void Start()
        {
            if (this.IsRunning || this.Interval == TimeSpan.Zero)
            {
                return;
            }
            this.IsRunning = true;
            this._cancellationHandler = new CancellationTokenSource();
            this._action = Task.Run(async () =>
            {
                while (!this._cancellationHandler.Token.IsCancellationRequested)
                {
                    await Task.Delay(this.Interval);
                    this.OnTick();
                }
            }, this._cancellationHandler.Token);
        }

        public void Stop()
        {
            this._cancellationHandler?.Cancel();
            this.IsRunning = false;
        }

        public void Reset(TimeSpan interval)
        {
            this.Interval = interval;
        }

        private void OnTick()
        {
            if (this._cancellationHandler.IsCancellationRequested || !this.IsRunning)
            {
                return;
            }
            if (this.Tick == null)
            {
                return;
            }
            foreach (var handler in this.Tick.GetInvocationList().OfType<EventHandler<TimerEventArgs>>())
            {
                try
                {
                    handler.Invoke(this, new TimerEventArgs(this.Interval));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Tick handler resulted in exception:\r\n\t{ex.GetType().Name}: {ex.Message}");
                }
            }
        }

        public void Dispose()
        {
            this.Stop();
            try
            {
                this._action?.Wait();
            }
            catch
            {
                
            }
        }
    }
}
