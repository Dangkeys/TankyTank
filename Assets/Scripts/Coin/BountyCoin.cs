using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BountyCoin : Coin
{
    public override int Collect()
    {
        if (!IsServer)//do only in the client side
        {
            Show(false);
            return 0;
        }

        //do in the server side
        if (isCollected) return 0;
        isCollected = true;
        Destroy(gameObject);
        return coinValue;
    }
}
