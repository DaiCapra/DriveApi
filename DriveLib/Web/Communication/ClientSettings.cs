namespace DriveLib.Web.Communication
{
    public class ClientSettings
    {
        public ClientSettings()
        {
            SecretPath = $"client-secret.json";
        }

        public string SecretPath { get; set; }
    }
}