using System;
using Google.Apis.Download;

namespace DriveLib.Web.Handles
{
    public class DownloadHandle : ProgressHandle, IDownloadProgress
    {
        public Action ProgressChanged { get; set; }
        public DownloadStatus Status { get; set; }
        public long BytesDownloaded { get; set; }
        public Exception Exception { get; set; }

        public float GetProgress()
        {
            var max = size != 0
                ? size
                : 1;

            return BytesDownloaded / (float)max;
        }
    }
}