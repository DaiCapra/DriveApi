using System;
using Google.Apis.Upload;

namespace DriveLib.Web.Handles
{
    public class UploadHandle : ProgressHandle, IUploadProgress
    {
        public Action ProgressChanged { get; set; }
        public UploadStatus Status { get; set; }
        public long BytesSent { get; set; }
        public Exception Exception { get; set; }

        public float GetProgress()
        {
            var max = size != 0
                ? size
                : 1;

            return BytesSent / (float)max;
        }
    }
}