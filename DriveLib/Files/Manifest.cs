namespace DriveLib.Files
{
    public class Manifest
    {
        public Manifest()
        {
            LatestId = string.Empty;
            LatestVersion = string.Empty;
        }

        public string LatestVersion { get; set; }

        public string LatestId { get; set; }
    }
}