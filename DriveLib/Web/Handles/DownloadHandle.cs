using System;
using Google.Apis.Download;

namespace DriveLib.Web.Handles
{
    public class DownloadHandle : ProgressHandle, IDownloadProgress
    {
        protected override long TransferredBytes => BytesDownloaded;
        public DownloadStatus Status { get; }
        public long BytesDownloaded { get; }
        public Exception Exception { get; }
        
        public Action<IDownloadProgress> Callback { get; set; }

    }
}