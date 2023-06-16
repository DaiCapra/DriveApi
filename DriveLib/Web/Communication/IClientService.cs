using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DriveLib.Web.Handles;
using DriveLib.Web.Results;
using Google.Apis.Download;
using Google.Apis.Upload;
using File = Google.Apis.Drive.v3.Data.File;

namespace DriveLib.Web.Communication
{
    public interface IClientService
    {
        Task<ListResult> ListAsync(CancellationTokenSource cts = null);
        Task<Result> DeleteAsync(string fileId, CancellationTokenSource cts = null);

        Task<Result> UpdateAsync(Stream stream,
            File metadata,
            string fileId,
            UploadHandle handle = null
        );

        Task<Result> UploadAsync(
            Stream stream,
            File metadata,
            UploadHandle handle = null
        );

        Task<DownloadResult> DownloadFileAsync(
            Stream stream,
            string fileId,
            DownloadHandle handle = null
        );

        bool Initialize(string secretPath = null);
    }
}