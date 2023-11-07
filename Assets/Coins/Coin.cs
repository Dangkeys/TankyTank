using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Coin : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField] SpriteRenderer spriteRenderer;
    protected int coinValue = 10;
    protected bool isCollected;
    public abstract int Collect();
    public void SetValue(int value)
    {
        coinValue = value;
    }
    protected void Show(bool show)
    {
        spriteRenderer.enabled = show;
    }
}