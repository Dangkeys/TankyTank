using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnContract : MonoBehaviour
{
    [SerializeField] GameObject dustPrefab;
    void OnDestroy()
    {
        Instantiate(dustPrefab, transform.position, Quaternion.identity);
    }
}
    