using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DriveLib.Web.Communication;
using Google.Apis.Download;
using Google.Apis.Upload;
using LogLib.Logging;
using File = Google.Apis.Drive.v3.Data.File;

namespace DriveLib.Web
{
    public class WebService
    {
        private readonly IClientService _clientService;
        private readonly ILogger _logger;

        public WebService(ILogger logger, IClientService clientService)
        {
            _logger = logger;
            _clientService = clientService;
        }

        public void Initialize()
        {
            _clientService.Initialize();
        }

        public async Task<bool> Upload(
            Stream stream,
            File metadata,
            Action<IUploadProgress> callback = null,
            CancellationTokenSource cts = null
        )
        {
            var result = await _clientService.UploadAsync(stream, metadata);
            return result.Success;
        }

        public async Task<List<File>> List(CancellationTokenSource cts = null)
        {
            var result = await _clientService.ListAsync(cts);
            return result.Files;
        }

        public async Task<bool> Delete(string id, CancellationTokenSource cts = null)
        {
            if (string.IsNullOrEmpty(id))
            {
                return false;
            }

            var result = await _clientService.DeleteAsync(id, cts);
            return result.Success;
        }

        public async Task<bool> Download(
            Stream stream,
            string fileId,
            Action<IDownloadProgress> callback = null,
            CancellationTokenSource cts = null
        )
        {
            var result = await _clientService.DownloadFileAsync(stream, fileId, callback, cts);
            return result.Success;
        }

        public async Task<bool> Update(
            Stream stream,
            File metadata,
            string fileId,
            Action<IUploadProgress> callback = null,
            CancellationTokenSource cts = null
        )
        {
            var result = await _clientService.UpdateAsync(stream, metadata, fileId, callback, cts);
            return result.Success;
        }
    }
}