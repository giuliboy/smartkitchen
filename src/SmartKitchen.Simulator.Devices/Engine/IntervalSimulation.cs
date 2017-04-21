using System;
using System.Diagnostics;
using System.Dynamic;

namespace HSR.CloudSolutions.SmartKitchen.Simulator.Simulation.Engine
{
    public class IntervalSimulation<T> : ISimulation
    {
        private readonly Action<T> _onChange;
        private readonly Func<T> _change;
        private readonly ITimer _timer;
        private readonly Func<bool> _isCompleted;
        private readonly Action _onStarted;
        private readonly Action _onCompleted;

        public IntervalSimulation(Action<T> onChange, Func<T> change, TimeSpan interval, Func<bool> isCompleted = null, Action onStarted = null, Action onCompleted = null)
        {
            if (onChange == null)
            {
                throw new ArgumentNullException(nameof(onChange));
            }
            if (change == null)
            {
                throw new ArgumentNullException(nameof(change));
            }
            if (interval <= TimeSpan.Zero)
            {
                throw new ArgumentException(nameof(interval));
            }
            this._onChange = onChange;
            this._change = change;
            this._isCompleted = isCompleted ?? (() => false);
            this._onStarted = onStarted ?? (() => { });
            this._onCompleted = onCompleted ?? (() => { });
            this._timer = new Timer(interval);
            this._timer.Tick += (s, a) =>
            {
                try
                {
                    this._onChange(this._change());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Simulation execution resulted in an exception!\r\n\t{ex.GetType()}: {ex.Message}");
                }
                finally
                {
                    if (this._isCompleted())
                    {
                        this._timer.Stop();
                        this.Executing = false;
                        this._onCompleted();
                    }
                }
            };
            this._timer.Start();
            this.Executing = true;
            this._onStarted();
        }

        public bool Executing { get; private set; }

        public void Dispose()
        {
            this._timer.Dispose();
            this.Executing = false;
        }
    }
}
