using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using server.Models.Dtos;

namespace server.Hubs
{
    public interface ILobbyHubClient
    {
        Task LobbyUpdate(LobbyResourceModel lobby);
        Task StartGame();
    }

    public class LobbyHub : Hub<ILobbyHubClient>
    {
        public async Task JoinLobby(string lobbyId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyId);
        }
    }
}