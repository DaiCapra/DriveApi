namespace DriveLib.Web.Handles
{
    public class FileCopyHandle : ProgressHandle
    {
        public int NumberOfFiles { get; set; }
        public int InstalledFiles { get; set; }

        public override float GetProgress()
        {
            var max = NumberOfFiles != 0
                ? NumberOfFiles
                : 1;

            return InstalledFiles / (float)max;
        }
    }
}