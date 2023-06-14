using System.Collections.Generic;
using Google.Apis.Drive.v3.Data;

namespace DriveLib.Web.Results
{
    public class ListResult : Result
    {
        public ListResult()
        {
            Files = new List<File>();
        }

        public List<File> Files { get; set; }
    }
}