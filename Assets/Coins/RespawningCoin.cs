using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawningCoin : Coin
{
    public override int Collect()
    {
        if(!IsServer) return 0;
        if(isCollected) return 0;
        isCollected = true;
        return coinValue;
    }

}
