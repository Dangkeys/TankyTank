using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Lobbies;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Data.SqlTypes;
using System.Text;

public class HostGameManager
{
    private string lobbyId;
    private const string GAME_SCENE = "Game";
    private Allocation allocation;
    private String joinCode;
    private const int MAX_CONNECTIONS = 20;
    private NetworkServer networkServer;
    public async Task StartHostAsync()
    {
        try
        {
            allocation = await Relay.Instance.CreateAllocationAsync(MAX_CONNECTIONS);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return;
        }
        try
        {
            joinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(joinCode);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return;
        }
        try
        {
            CreateLobbyOptions lobbyOptions = new CreateLobbyOptions();
            lobbyOptions.IsPrivate = false;
            lobbyOptions.Data = new Dictionary<string, DataObject>()
            {
                {
                    "JoinCode", new DataObject(
                        visibility: DataObject.VisibilityOptions.Member,
                        value: joinCode
                    )
                }

            };
            string lobbyName = PlayerPrefs.GetString(NameSelector.PLAYER_NAME_KEY, "Unknown");
            Lobby lobby =
            await Lobbies.Instance.CreateLobbyAsync($"{lobbyName}'s lobby", MAX_CONNECTIONS, lobbyOptions);
            lobbyId = lobby.Id;
            HostSingleton.Instance.StartCoroutine(HeartBeatLobby(15));
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            return;
        }
        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
        transport.SetRelayServerData(relayServerData);
        networkServer = new NetworkServer(NetworkManager.Singleton);
        UserData userData = new UserData
        {
            userName = PlayerPrefs.GetString(NameSelector.PLAYER_NAME_KEY, "Missing Name")
        };
        string payload = JsonUtility.ToJson(userData);
        byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);
        NetworkManager.Singleton.NetworkConfig.ConnectionData = payloadBytes;
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene(GAME_SCENE, LoadSceneMode.Single);
    }
    private IEnumerator HeartBeatLobby(float waitTimeSeconds)
    {
        WaitForSecondsRealtime delay = new WaitForSecondsRealtime(waitTimeSeconds); ;
        while (true)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return delay;
        }
    }
}

