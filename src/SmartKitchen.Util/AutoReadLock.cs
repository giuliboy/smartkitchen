using System;
using System.Threading;

namespace HSR.CloudSolutions.SmartKitchen.Util
{
    public class AutoReadLock : IDisposable
    {
        private readonly ReaderWriterLockSlim _readLock;

        public AutoReadLock(ReaderWriterLockSlim readLock)
        {
            this._readLock = readLock;
            this._readLock?.EnterReadLock();
        }

        public void Dispose()
        {
            this._readLock?.ExitReadLock();
        }
    }
}
