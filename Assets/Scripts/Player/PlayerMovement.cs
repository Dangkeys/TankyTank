using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class PlayerMovement : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] InputReader inputReader;
    [SerializeField] Transform bodyTransform;
    [SerializeField] Rigidbody2D rb;
    [Header("Settings")]
    [SerializeField] float movementSpeed = 4f;
    [SerializeField] float turningRate = 30f;
    [SerializeField] Vector2 previousMovementInput;
    // Update is called once per frame
    public override void OnNetworkSpawn()
    {
        if(!IsOwner) return;
        inputReader.MoveEvent += HandleMove;
    }

    private void HandleMove(Vector2 movementInput)
    {
        previousMovementInput = movementInput;
    }
    public override void OnNetworkDespawn()
    {
        if(!IsOwner) return;
        inputReader.MoveEvent -= HandleMove;
    }
    void Update()
    {
        if(!IsOwner) return;
        bodyTransform.Rotate(0f,0f, previousMovementInput.x * -turningRate * Time.deltaTime);
    }
    private void FixedUpdate() {
        if(!IsOwner) return;
        rb.velocity = (Vector2)bodyTransform.up * previousMovementInput.y * movementSpeed;
    }
}
