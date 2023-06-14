namespace DriveLib.Providers
{
    public class Provider<T> where T : new()
    {
        protected Provider()
        {
            Resource = new T();
        }

        protected T Resource { get; set; }
    }
}