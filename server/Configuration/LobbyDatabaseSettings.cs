namespace server.Configuration
{
    public class LobbyDatabaseSettings : ILobbyDatabaseSettings
    {
        public string LobbyCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface ILobbyDatabaseSettings
    {
        string LobbyCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}