namespace iDevWorks.Ticket
{
    public class App(string appName, string username, Guid apiKey, int timeZone)
    {
        public string AppName { get; } = appName;
        public string Username { get; } = username;
        public int TimeZone { get; } = timeZone;
        public Guid ApiKey { get; } = apiKey;
    }
}
