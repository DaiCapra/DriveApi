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
    }
}