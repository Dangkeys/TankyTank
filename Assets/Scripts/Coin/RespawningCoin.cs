using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawningCoin : Coin
{
    public event Action<RespawningCoin> OnCollected;
    Vector3 previousPosition;
    private void Update() {
        if(previousPosition != transform.position)
        {
            Show(true);
        }
        previousPosition = transform.position;
    }
    public override int Collect()
    {
        if (!IsServer)
        {
            Show(false);
            return 0;
        }


        if (isCollected) return 0;
        isCollected = true;
        OnCollected?.Invoke(this);
        return coinValue;
    }

    internal void Reset()
    {
        isCollected = false;
    }
}
