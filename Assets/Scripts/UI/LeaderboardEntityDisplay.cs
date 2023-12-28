using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class LeaderboardEntityDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text disPlayText;
    [SerializeField] private Color ownerColor;
    private FixedString32Bytes playerName;
    public ulong ClientId { get; private set; }
    public int Coins { get; private set; }
    public void Initiailise(ulong clientId, FixedString32Bytes playerName, int coins)
    {
        ClientId = clientId;
        this.playerName = playerName;
        if(clientId == NetworkManager.Singleton.LocalClientId)
        {
            disPlayText.color = ownerColor;
        }
        UpdateCoins(coins);
    }
    public void UpdateCoins(int coins)
    {
        Coins = coins;
        UpdateText();
    }
    public void UpdateText()
    {
        disPlayText.text = $"{transform.GetSiblingIndex() + 1}. {playerName} ({Coins})";
    }
}
