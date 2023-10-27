using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using System.Collections.Concurrent;

public class TestServer : NetworkBehaviour
{
    public static TestServer Instance { get; set; }

    public Transform lobbyList;
    public GameObject theLobby;
    public GameObject Menu;
    public GameObject Server;
    public TextMeshProUGUI ServerId;
    public TMP_InputField JoinById;

    private string temporaryId;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            return;
        }
    }
    private async void Start()
    {
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("signed in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
    
    public async void CreateLobby(string lobbyName,bool isPrivate,int capacity)
    {
        try
        {
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, capacity ,new CreateLobbyOptions { 
                IsPrivate = isPrivate
            });
            if (lobby.Players.Count != 0)
            {
                DisableMenu();
                ServerId.text = "Lobby Code : " + lobby.LobbyCode;
                StartCoroutine(SendHeartBeat(lobby.Id, 5 ,lobby));

                NetworkManager.Singleton.StartHost();
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    IEnumerator SendHeartBeat(string id, float seconds,Lobby theLobby)
    {
        var delay =  new WaitForSeconds(seconds);

        while (true)
        {
            LobbyService.Instance.SendHeartbeatPingAsync(id);
            yield return delay;
        }
    }
    public async void QuickJoin()
    {
        try
        {
            Lobby lobby = await Lobbies.Instance.QuickJoinLobbyAsync();
            ServerId.text = "Lobby Code : " + lobby.LobbyCode;
            var callBacks = new LobbyEventCallbacks();
            DisableMenu();

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    public async void ListLobbies()
    {
        QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
        foreach (Transform childs in lobbyList)
        {
            Destroy(childs.gameObject);
        }
        foreach (var result in queryResponse.Results)
        {
            var instantiatedLobby = Instantiate(theLobby, lobbyList);
            instantiatedLobby.GetComponentInChildren<serverButton>().SetLobby(result, result.Name, result.Id);
        }
    }
    public async void JoinLobbyById()
    {
        try
        {
            await LobbyService.Instance.JoinLobbyByCodeAsync(JoinById.text.Substring(0, 6));
            ServerId.text = "Lobby Code : " + JoinById.text;
            DisableMenu();
            NetworkManager.Singleton.StartClient();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    
    public void DisableMenu()
    {
        Menu.SetActive(false);
        Server.SetActive(true);
    }
   
}
