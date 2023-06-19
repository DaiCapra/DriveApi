using System;
using Google.Apis.Download;

namespace DriveLib.Web.Handles
{
    public class DownloadHandle : ProgressHandle, IDownloadProgress
    {
        public DownloadStatus Status { get; set; }
        public long BytesDownloaded { get; set; }
        public Exception Exception { get; set; }

        public override float GetProgress()
        {
            var max = Size != 0
                ? Size
                : 1;

            return BytesDownloaded / (float)max;
        }
    }
}