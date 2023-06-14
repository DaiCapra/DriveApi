using DriveLib.Files;
using DriveLib.Serialization;
using DriveLib.Web.Communication;
using LogLib.Logging;
using SimpleInjector;

namespace DriveLib.Ioc
{
    public class Installer : Container
    {
        public Installer()
        {
            Options.ResolveUnregisteredConcreteTypes = true;
            Options.AllowOverridingRegistrations = true;
        }

        public void Install()
        {
            OnInstall();
            Verify();
        }

        protected virtual void OnInstall()
        {
            RegisterSingleton<ILogger, Logger>();
            
            RegisterSingleton<JsonManager>();
            RegisterSingleton<FileManager>();
            
            RegisterSingleton<IClientSettingsProvider, ClientSettingsProvider>();
            RegisterSingleton<IClientService, ClientService>();
        }
    }
}