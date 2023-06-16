using System;
using System.IO;
using LogLib.Logging;

namespace DriveLib.Files
{
    public class FileManager
    {
        private readonly ILogger _logger;

        public FileManager(ILogger logger)
        {
            _logger = logger;
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
                File.WriteAllText(path, text);
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