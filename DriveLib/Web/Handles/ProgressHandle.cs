using System;
using System.Threading;

namespace DriveLib.Web.Handles
{
    public abstract class ProgressHandle : CancellationTokenSource, IDisposable
    {
        public long Size { get; set; }

        protected ProgressHandle()
        {
            Size = 0;
        }

        public Action<ProgressHandle> ProgressChanged { get; set; }

        public virtual float GetProgress()
        {
            return 0;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            ProgressChanged = null;
        }
    }
}