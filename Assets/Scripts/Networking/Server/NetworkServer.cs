using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class NetworkServer : IDisposable
{
    private NetworkManager networkManager;
    public Action<string> OnclientLeft;

    private Dictionary<ulong, string> clientIdToAuth = new Dictionary<ulong, string>();
    private Dictionary<string, UserData> authIdToUserData = new Dictionary<string, UserData>();

    public NetworkServer(NetworkManager networkManager)
    {
        this.networkManager = networkManager;

        networkManager.ConnectionApprovalCallback += ApprovalCheck;
        networkManager.OnServerStarted += OnNetworkReady;
    }
    public bool OpenConnection(string ip, int port)
    {
        UnityTransport transport = networkManager.gameObject.GetComponent<UnityTransport>();
        transport.SetConnectionData(ip, (ushort)port);
        return networkManager.StartServer();
    }
    private void ApprovalCheck(
        NetworkManager.ConnectionApprovalRequest request,
        NetworkManager.ConnectionApprovalResponse response)
    {
        string payload = System.Text.Encoding.UTF8.GetString(request.Payload);
        UserData userData = JsonUtility.FromJson<UserData>(payload);

        clientIdToAuth[request.ClientNetworkId] = userData.userAuthId;
        authIdToUserData[userData.userAuthId] = userData;

        response.Approved = true;
        response.Position = SpawnPoint.GetRandomSpawnPos();
        response.Rotation = Quaternion.identity;
        response.CreatePlayerObject = true;
    }

    private void OnNetworkReady()
    {
        networkManager.OnClientDisconnectCallback += OnClientDisconnect;

    }

    private void OnClientDisconnect(ulong clientId)
    {
        if (clientIdToAuth.TryGetValue(clientId, out string authId))
        {
            clientIdToAuth.Remove(clientId);
            authIdToUserData.Remove(authId);
            OnclientLeft?.Invoke(authId);
        }
    }
    public UserData GetUserDataByClientId(ulong clientId)
    {
        if(!clientIdToAuth.TryGetValue(clientId, out string authId)) return null;
        if(!authIdToUserData.TryGetValue(authId, out UserData userData)) return null;
        return userData;
    }
    public void Dispose()
    {
        if (networkManager == null) return;
        networkManager.ConnectionApprovalCallback -= ApprovalCheck;
        networkManager.OnClientDisconnectCallback -= OnClientDisconnect;
        networkManager.OnServerStarted -= OnNetworkReady;
        if(networkManager.IsListening)
        {
            networkManager.Shutdown();
        }
    }

}