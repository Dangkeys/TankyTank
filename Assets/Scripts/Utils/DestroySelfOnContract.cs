using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelfOnContract : MonoBehaviour
{
    [SerializeField] private Projectile projectile;
    private const int SOLO_TEAM_INDEX = -1;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (projectile.TeamIndex != SOLO_TEAM_INDEX)
        {
            if (other.attachedRigidbody != null)
            {
                if (other.attachedRigidbody.TryGetComponent<TankPlayer>(out TankPlayer player))
                {
                    if (player.TeamIndex.Value == projectile.TeamIndex)
                    {
                        return;
                    }
                }
            }
        }

        Destroy(gameObject);
    }
}
