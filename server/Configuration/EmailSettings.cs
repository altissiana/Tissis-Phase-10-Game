namespace server.Configuration
{
    public class EmailSettings : IEmailSettings
    {
        public string ApiKey { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string FromEmail { get; set; }

    }

    public interface IEmailSettings
    {
        string ApiKey { get; set; }
        string ToEmail { get; set; }
        string Subject { get; set; }
        string FromEmail { get; set; }
    }

}