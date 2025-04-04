using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class BulletProjectile : NetworkBehaviour
{
    [SerializeField] private NetworkObject networkObject;
    [SerializeField] private Transform hitParticle;

    private float moveSpeed;
    private Vector3 targetPosition;
    
    public void Setup(Vector3 targetPosition, float moveSpeed)
    {
        SetupServerRpc(targetPosition, moveSpeed);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetupServerRpc(Vector3 targetPosition, float moveSpeed)
    {
        SetupClientRpc(targetPosition, moveSpeed);
    }
    [ClientRpc]
    private void SetupClientRpc(Vector3 targetPosition, float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
        this.targetPosition = targetPosition;
    }

    private void FixedUpdate()
    {
        if(!IsServer) return;
        
        transform.position += (moveSpeed * Time.deltaTime * transform.forward);

        if(Vector3.Distance(transform.position, this.targetPosition) < moveSpeed * Time.deltaTime)
        {
            BulletSpawner.Instance.DespawnBullet(networkObject);
            Instantiate(hitParticle, transform.position, Quaternion.identity);
        }
    }
}
