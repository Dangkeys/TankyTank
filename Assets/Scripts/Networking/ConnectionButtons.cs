using System.Collections;
using System.Collections.Generic;
using QFSW.QC;
using Unity.Netcode;
using UnityEngine;

public class ConnectionButtons : MonoBehaviour
{
    [Command]
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }
    [Command]
    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}
