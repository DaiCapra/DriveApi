using System;
using Google.Apis.Upload;

namespace DriveLib.Web.Handles
{
    public class UploadHandle : ProgressHandle, IUploadProgress
    {
        public Action<UploadHandle> ProgressChanged { get; set; }
        public UploadStatus Status { get; set; }
        public long BytesSent { get; set; }
        public Exception Exception { get; set; }

        public override float GetProgress()
        {
            var max = Size != 0
                ? Size
                : 1;

            return BytesSent / (float)max;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            ProgressChanged = null;
        }
    }
}