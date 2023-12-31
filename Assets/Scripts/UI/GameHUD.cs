using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : NetworkBehaviour
{
    [SerializeField] private TMP_Text lobbyCodeText;
    private NetworkVariable<FixedString32Bytes> lobbyCode = new NetworkVariable<FixedString32Bytes>("");
    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            lobbyCode.OnValueChanged += HandleLobbyCodeChanged;
            HandleLobbyCodeChanged(string.Empty, lobbyCode.Value);
        }
        if (!IsHost) return;
        lobbyCode.Value = HostSingleton.Instance.GameManager.JoinCode;
    }
    public override void OnNetworkDespawn()
    {
        if (IsClient)
        {
            lobbyCode.OnValueChanged -= HandleLobbyCodeChanged;
        }
    }


    public void LeaveGame()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            HostSingleton.Instance.GameManager.Shutdown();
        }
        ClientSingleton.Instance.GameManager.Disconnect();
    }
        private void HandleLobbyCodeChanged(FixedString32Bytes previousValue, FixedString32Bytes newValue)
    {
        lobbyCodeText.text = newValue.ToString();
    }

}
