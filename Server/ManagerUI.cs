using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ManagerUI : NetworkBehaviour
{
    [SerializeField] Button server;
    [SerializeField] Button host;
    [SerializeField] Button client;

    [SerializeField] GameObject panel;
    [SerializeField] GameObject servers;
    [SerializeField] GameObject buttons;

    [SerializeField] TMP_InputField serverName;
    [SerializeField] TMP_Dropdown isPublic;

    private Action<bool> MenuActions;

    private string _serverName;
    private bool _isPublic=false;

    private void Start()
    {
        MenuActions += servers.SetActive;
        MenuActions += panel.SetActive;

        host.onClick.AddListener(() => { TestServer.Instance.CreateLobby(_serverName,_isPublic,2); });
        client.onClick.AddListener(() => { TestServer.Instance.ListLobbies(); });
    }
    public void ReturnMenu()
    {
        MenuActions?.Invoke(false);
        buttons.SetActive(true);
    }
    public void CreateMenu()
    {
        panel.SetActive(true);
        buttons.SetActive(false);
    }
    public void ListLobbies() => servers.SetActive(true);
    public void SetPublic(int isPublic) => _isPublic = isPublic == 0 ? false : true;
    public void SetServerName(string serverName) => _serverName = serverName;
}
