using System.Threading;

namespace DriveLib.Web.Handles
{
    public abstract class ProgressHandle : CancellationTokenSource
    {
        public string name;
        public long size;

        protected ProgressHandle()
        {
            size = 0;
            name = string.Empty;
        }

        protected abstract long TransferredBytes { get; }

        public float GetProgress()
        {
            var max = size != 0
                ? size
                : 1;
            return TransferredBytes / (float)max;
        }
    }
}