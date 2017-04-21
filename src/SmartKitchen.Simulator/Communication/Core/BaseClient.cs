using System;
using System.Diagnostics;
using HSR.CloudSolutions.SmartKitchen.Util;

namespace HSR.CloudSolutions.SmartKitchen.Simulator.Communication.Core
{
    public abstract class BaseClient : IDisposable
    {
        protected void LogException(string message, Exception ex)
        {
            Debug.WriteLine($"{DateTime.Now.ToString("yyyy MM dd - HH:mm:ss.ffff")}: {message}\r\n{ex.CreateExceptionDialogMessage()}");
        }

        public void Dispose()
        {
            OnDispose();
        }

        protected virtual void OnDispose()
        {
            
        }
    }
}
