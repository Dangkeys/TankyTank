using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTime : MonoBehaviour
{
    [SerializeField] float lifeTime = 2f;
    private void Start() {
        Destroy(gameObject, lifeTime);
    }
}
