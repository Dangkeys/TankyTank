using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    private void Awake()
    {
        exitButton.onClick.AddListener(LeaveGame);
    }
    public void LeaveGame()
    {
        if(NetworkManager.Singleton.IsHost)
        {
            HostSingleton.Instance.GameManager.Shutdown();
        }
        ClientSingleton.Instance.GameManager.Disconnect();
    }
}
