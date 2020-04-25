using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace server.Hubs
{
    public interface IChatHubClient
    {
        Task ReceiveMessage(ChatMessage chatMessage);
    }
    
    public class ChatHub : Hub<IChatHubClient>
    {
        public async Task SendMessage(string lobbyId, string user, string message)
        {
            ChatMessage chatMessage = new ChatMessage
            {
                User = user,
                Message = message
            };
            
            await Clients.Group(lobbyId).ReceiveMessage(chatMessage);
        }      

        public async Task JoinLobby(string lobbyId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyId);
        }  
    }
}