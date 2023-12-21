using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class LeaderboardEntityDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text disPlayText;
    private FixedString32Bytes playerName;
    public ulong ClientId { get; private set; }
    public int Coins { get; private set; }
    public void Initiailise(ulong clientId, FixedString32Bytes playerName, int coins)
    {
        ClientId = clientId;
        this.playerName = playerName;
        UpdateCoins(coins);
    }
    public void UpdateCoins(int coins)
    {
        Coins = coins;
        UpdateText();
    }
    private void UpdateText()
    {
        disPlayText.text = $"{ClientId}. {playerName} ({Coins})";
    }
}
