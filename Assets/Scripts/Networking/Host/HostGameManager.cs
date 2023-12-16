using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HostGameManager
{
    private const string GAME_SCENE = "Game";
    private Allocation allocation;
    private String joinCode;
    private const int MAX_CONNECTIONS = 20;
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

       UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
       RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
       transport.SetRelayServerData(relayServerData);
       NetworkManager.Singleton.StartHost();
       NetworkManager.Singleton.SceneManager.LoadScene(GAME_SCENE, LoadSceneMode.Single);
    }
}