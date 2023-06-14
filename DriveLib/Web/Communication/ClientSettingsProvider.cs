using DriveLib.Providers;

namespace DriveLib.Web.Communication
{
    public class ClientSettingsProvider : Provider<ClientSettings>, IClientSettingsProvider
    {
        public ClientSettings Settings
        {
            get => Resource;
            set => Resource = value;
        }
    }
}