using System;
using System.Threading;

namespace HSR.CloudSolutions.SmartKitchen.Util
{
    public class AutoWriteLock : IDisposable
    {
        private readonly ReaderWriterLockSlim _writeLock;

        public AutoWriteLock(ReaderWriterLockSlim writeLock)
        {
            this._writeLock = writeLock;
            this._writeLock?.EnterWriteLock();
        }

        public void Dispose()
        {
            this._writeLock?.ExitWriteLock();
        }
    }
}
