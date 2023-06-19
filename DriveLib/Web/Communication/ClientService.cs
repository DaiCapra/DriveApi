using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DriveLib.Files;
using DriveLib.Web.Errors;
using DriveLib.Web.Handles;
using DriveLib.Web.Results;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Http;
using Google.Apis.Services;
using Google.Apis.Upload;
using LogLib.Logging;
using File = Google.Apis.Drive.v3.Data.File;

namespace DriveLib.Web.Communication
{
    public class ClientService : IClientService
    {
        private const string Fields = "files(id,name,size,mimeType,parents)";
        private readonly IClientSettingsProvider _clientSettingsProvider;
        private readonly FileManager _fileManager;
        private readonly ILogger _logger;
        private DriveService _driveService;

        public ClientService(
            ILogger logger,
            IClientSettingsProvider clientSettingsProvider,
            FileManager fileManager
        )
        {
            _logger = logger;
            _clientSettingsProvider = clientSettingsProvider;
            _fileManager = fileManager;
        }

        public bool Initialized { get; set; }

        private ClientSettings Settings => _clientSettingsProvider.Settings;

        public async Task<ListResult> ListAsync(CancellationTokenSource cts = null)
        {
            var result = new ListResult();
            try
            {
                var request = _driveService.Files.List();
                request.Fields = Fields;

                var response = await request.ExecuteAsync(GetToken(cts));

                result.Files.AddRange(response.Files);
                result.Success = true;
            }
            catch (Exception e)
            {
                _logger.Error(e);
                result.Errors.Add(new Error { Message = e.Message });
            }

            return result;
        }

        public async Task<Result> DeleteAsync(string fileId, CancellationTokenSource cts = null)
        {
            var result = new Result();

            try
            {
                await _driveService.Files
                    .Delete(fileId)
                    .ExecuteAsync(GetToken(cts));

                result.Success = true;
            }
            catch (Exception e)
            {
                _logger.Error(e);
                result.Errors.Add(new Error { Message = e.Message });
            }

            return result;
        }


        public async Task<Result> UpdateAsync(Stream stream,
            File metadata,
            string fileId,
            UploadHandle handle = null
        )
        {
            var result = new Result();

            try
            {
                var request = _driveService.Files.Update(metadata, fileId, stream, metadata.MimeType);
                InitUploadHandle(handle, request);

                var response = await request.UploadAsync(GetToken(handle));

                result.Success = response.Status == UploadStatus.Completed;
                result.AddException(response.Exception);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                result.Errors.Add(new Error { Message = e.Message });
            }

            return result;
        }


        public async Task<Result> UploadAsync(
            Stream stream,
            File metadata,
            UploadHandle handle = null
        )
        {
            var result = new Result();

            try
            {
                var request = _driveService.Files.Create(metadata, stream, metadata.MimeType);
                InitUploadHandle(handle, request);

                var response = await request.UploadAsync(GetToken(handle));

                result.Success = response.Status == UploadStatus.Completed;
                result.AddException(response.Exception);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                result.Errors.Add(new Error { Message = e.Message });
            }

            return result;
        }

        public async Task<DownloadResult> DownloadFileAsync(
            Stream stream,
            string fileId,
            DownloadHandle handle = null
        )
        {
            var result = new DownloadResult();

            try
            {
                var request = _driveService.Files.Get(fileId);
                InitDownloadHandle(handle, request);

                var response = await request.DownloadAsync(stream, GetToken(handle));
                if (response != null)
                {
                    result.Status = response.Status;
                    result.Success = response.Status == DownloadStatus.Completed;

                    if (response.Exception != null)
                    {
                        result.Errors.Add(new Error { Message = response.Exception.Message });
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error(e);
                result.Errors.Add(new Error { Message = e.Message });
            }


            return result;
        }


        public bool Initialize(string secretPath = null)
        {
            if (!string.IsNullOrEmpty(secretPath))
            {
                Settings.SecretPath = secretPath;
            }

            var credentials = CreateCredentials();
            if (credentials == null)
            {
                return false;
            }

            _driveService = CreateService(credentials);
            Initialized = true;
            return true;
        }

        private void InitUploadHandle(UploadHandle handle, FilesResource.UpdateMediaUpload request)
        {
            if (handle == null)
            {
                return;
            }

            request.ProgressChanged += obj =>
            {
                handle.Exception = obj.Exception;
                handle.Status = obj.Status;
                handle.BytesSent = obj.BytesSent;
                handle?.ProgressChanged?.Invoke(handle);
            };
        }

        private void InitUploadHandle(UploadHandle handle, FilesResource.CreateMediaUpload request)
        {
            if (handle == null)
            {
                return;
            }

            request.ProgressChanged += obj =>
            {
                handle.Exception = obj.Exception;
                handle.Status = obj.Status;
                handle.BytesSent = obj.BytesSent;
                handle?.ProgressChanged?.Invoke(handle);
            };
        }

        private void InitDownloadHandle(DownloadHandle handle, FilesResource.GetRequest request)
        {
            if (handle == null)
            {
                return;
            }

            request.MediaDownloader.ProgressChanged += obj =>
            {
                handle.Exception = obj.Exception;
                handle.Status = obj.Status;
                handle.BytesDownloaded = obj.BytesDownloaded;
                handle?.ProgressChanged?.Invoke(handle);
            };
        }

        private GoogleCredential CreateCredentials()
        {
            if (string.IsNullOrEmpty(Settings.SecretPath))
            {
                _logger.Error("Invalid secret path");
                return null;
            }

            if (!System.IO.File.Exists(Settings.SecretPath))
            {
                _logger.Error($"Secret file does not exist: {Settings.SecretPath}");
                return null;
            }

            if (!_fileManager.TryReadAllText(Settings.SecretPath, out var json))
            {
                _logger.Error($"Could not read secret file: {Settings.SecretPath}");
                return null;
            }

            try
            {
                var credential = GoogleCredential
                    .FromJson(json)
                    .CreateScoped(DriveService.ScopeConstants.Drive);

                return credential;
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return null;
            }
        }

        private CancellationToken GetToken(CancellationTokenSource cts)
        {
            return cts?.Token ?? new CancellationToken();
        }

        private DriveService CreateService(IConfigurableHttpClientInitializer clientInitializer)
        {
            return new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = clientInitializer
            });
        }
    }
}