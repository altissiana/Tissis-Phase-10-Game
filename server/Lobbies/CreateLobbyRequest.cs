namespace server.Lobbies
{
    public class CreateLobbyRequest
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public string PlayerName{ get; set; }
        public string PlayerId { get; set; }
    }
}