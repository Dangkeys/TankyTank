using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColourDisplay : MonoBehaviour
{
    [SerializeField] private TeamColorLookup teamColorLookup;
    [SerializeField] private TankPlayer player;
    [SerializeField] private SpriteRenderer[] playerSprites;

    private void Start()
    {
        HandlePlayerColourDisplay(-1, player.TeamIndex.Value);
        player.TeamIndex.OnValueChanged += HandlePlayerColourDisplay;
    }
    private void OnDestroy() {
        player.TeamIndex.OnValueChanged -= HandlePlayerColourDisplay;
    }
    
    private void HandlePlayerColourDisplay(int previousValue, int newValue)
    {
        Color teamColor = teamColorLookup.GetTeamColour(newValue);
        foreach (SpriteRenderer spriteRenderer in playerSprites)
        {
            spriteRenderer.color = teamColor; 
        }
    }
}
