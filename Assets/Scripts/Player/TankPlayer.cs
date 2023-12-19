using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class TankPlayer : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachineVirtualCamera followCamera;
    [Header("Settings")]
    [SerializeField] private int ownerPriority = 15;
    [SerializeField] private int notOwnerPriority = 10;
    public NetworkVariable<FixedString32Bytes> PlayerName = new NetworkVariable<FixedString32Bytes>();
    public override void OnNetworkSpawn()
    {
        if(IsServer)
        {
            UserData userData =
            HostSingleton.Instance.GameManager.NetworkServer.GetUserDataFromClientId(OwnerClientId);
            PlayerName.Value = userData.userName;
        }
        if(ownerPriority < notOwnerPriority) Debug.LogWarning("ownerPriority is less than not the owner");
        if(!IsOwner)
        {
            followCamera.Priority =  notOwnerPriority;
        }else {
            followCamera.Priority = ownerPriority;
        }
    }
}
