using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class serverButton : MonoBehaviour
{
    private Lobby lobby;
    private string lobbyId;
    [SerializeField] TextMeshProUGUI count;
    [SerializeField] TextMeshProUGUI lobbyName;
    public async void JoinLobby()
    {
        try
        {
            Lobby lobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
            TestServer.Instance.DisableMenu();
            NetworkManager.Singleton.StartClient();
            TestServer.Instance.ServerId.text = "Lobby Code : " + lobby.LobbyCode;

        }
        catch (LobbyServiceException e)
        {
            print(e);
        }
    }
    public void SetLobby(Lobby lobby,string name,string id)
    {
        this.lobby = lobby;
        lobbyName.text = name;
        lobbyId = id;
        print(lobby.Id);
    }
}
