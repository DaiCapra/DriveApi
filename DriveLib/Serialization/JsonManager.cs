using System;
using LogLib.Logging;
using Newtonsoft.Json;

namespace DriveLib.Serialization
{
    public class JsonManager
    {
        private readonly ILogger _logger;
        private readonly JsonSerializerSettings _settings;

        public JsonManager(ILogger logger)
        {
            _logger = logger;
            _settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        public bool TrySerialize<T>(T t, out string json)
        {
            try
            {
                json = JsonConvert.SerializeObject(t, _settings);
                return true;
            }
            catch (Exception e)
            {
                json = string.Empty;
                _logger.Error(e);
                return false;
            }
        }

        public bool TryDeserialize<T>(string json, out T t)
        {
            try
            {
                t = JsonConvert.DeserializeObject<T>(json, _settings);
                return true;
            }
            catch (Exception e)
            {
                t = default;
                _logger.Error(e);
                return false;
            }
        }
    }
}