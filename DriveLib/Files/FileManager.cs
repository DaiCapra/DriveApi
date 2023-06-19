using System;
using System.IO;
using System.IO.Compression;
using DriveLib.Serialization;
using DriveLib.Web.Handles;
using LogLib.Logging;

namespace DriveLib.Files
{
    public class FileManager
    {
        private readonly JsonManager _jsonManager;
        private readonly ILogger _logger;

        public FileManager(
            ILogger logger,
            JsonManager jsonManager
        )
        {
            _logger = logger;
            _jsonManager = jsonManager;
        }

        public bool TrySave<T>(string path, T obj)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            if (!_jsonManager.TrySerialize(obj, out var json))
            {
                return false;
            }

            return TryWriteAllText(path, json);
        }

        public bool TryLoad<T>(string path, out T obj)
        {
            obj = default;

            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            if (!TryReadAllText(path, out var json))
            {
                return false;
            }

            return _jsonManager.TryDeserialize(json, out obj);
        }

        public bool TryReadAllText(string path, out string text)
        {
            try
            {
                text = File.ReadAllText(path);
                return true;
            }
            catch (Exception e)
            {
                text = string.Empty;
                _logger.Error(e);
                return false;
            }
        }

        public bool TryWriteAllText(string path, string text)
        {
            try
            {
                var dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                File.WriteAllText(path, text);
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return false;
            }
        }

        public bool TryExtractZip(string source, string destination)
        {
            try
            {
                if (File.Exists(destination))
                {
                    File.Delete(destination);
                }

                ZipFile.ExtractToDirectory(source, destination);
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return false;
            }
        }

        public bool TryCopyFiles(string sourceFolder, string destinationFolder, FileCopyHandle handle = null)
        {
            try
            {
                if (handle != null)
                {
                    handle.NumberOfFiles = Directory.GetFiles(sourceFolder, "*.*", SearchOption.AllDirectories).Length;
                }

                CopyFilesRecursively(sourceFolder, destinationFolder, handle);
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return false;
            }
        }

        private void CopyFilesRecursively(string sourcePath, string targetPath, FileCopyHandle handle = null)
        {
            //Now Create all of the directories
            foreach (string directoryPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(directoryPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string filepath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(filepath, filepath.Replace(sourcePath, targetPath), true);
                
                if (handle != null)
                {
                    handle.InstalledFiles++;
                    handle.ProgressChanged?.Invoke(handle);
                }
            }
        }

        public bool TryDeleteDirectory(string path)
        {
            try
            {
                Directory.Delete(path, true);
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return false;
            }
        }
    }
}