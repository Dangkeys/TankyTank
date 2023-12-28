using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using Unity.VisualScripting;

public class PlayerMovement : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] InputReader inputReader;
    [SerializeField] Transform bodyTransform;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] private ParticleSystem dustCloud;
    [Header("Settings")]
    [SerializeField] float movementSpeed = 4f;
    [SerializeField] float turningRate = 30f;
    [SerializeField] private float particleEmissionValue = 10;
    private ParticleSystem.EmissionModule emissionModule;
    [SerializeField] Vector2 previousMovementInput;
    private Vector3 previousPos;

    private const float  PARTICLE_STOP_THRESHOLD  = 0.005f;

    private void Awake()
    {
        emissionModule = dustCloud.emission;
    }
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
        if((transform.position - previousPos).sqrMagnitude > PARTICLE_STOP_THRESHOLD)
        {
            emissionModule.rateOverTime = particleEmissionValue;
        }else{
            emissionModule.rateOverTime = 0;
        }
        if(!IsOwner) return;
        rb.velocity = (Vector2)bodyTransform.up * previousMovementInput.y * movementSpeed;
    }
}
