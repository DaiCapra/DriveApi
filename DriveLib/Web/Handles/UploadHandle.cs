using System;
using Google.Apis.Upload;

namespace DriveLib.Web.Handles
{
    public class UploadHandle : ProgressHandle, IUploadProgress
    {
        protected override long TransferredBytes => BytesSent;
        public UploadStatus Status { get; }
        public long BytesSent { get; }
        public Exception Exception { get; }

        public Action<IUploadProgress> Callback { get; set; }
    }
}