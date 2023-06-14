using Google.Apis.Drive.v3.Data;

namespace DriveLib.Web
{
    public class MetadataBuilder
    {
        private readonly File _metadata;

        public MetadataBuilder()
        {
            _metadata = new File();
        }

        public MetadataBuilder Name(string filename)
        {
            if (!string.IsNullOrEmpty(filename))
            {
                _metadata.Name = filename;
            }

            return this;
        }

        public MetadataBuilder MimeType(string type)
        {
            if (!string.IsNullOrEmpty(type))
            {
                _metadata.MimeType = type;
            }

            return this;
        }

        public MetadataBuilder Parent(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                _metadata.Parents = new[] { id };
            }

            return this;
        }


        public File Build()
        {
            return _metadata;
        }
    }
}