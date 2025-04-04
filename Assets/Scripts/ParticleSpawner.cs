using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ParticleSpawner : NetworkBehaviour
{
    public static ParticleSpawner Instance { get; private set; }

    [SerializeField] private GameObject hitParticlePrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnHitParticle(Vector3 position)
    {
        SpawnHitParticleServerRpc(position);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnHitParticleServerRpc(Vector3 position)
    {
        NetworkObjectPool.Instance.GetNetworkObject(hitParticlePrefab, position, Quaternion.identity);
    }

    public void DespawnHitParticle(NetworkObject networkObject)
    {
        DespawnHitParticleServerRpc(networkObject);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DespawnHitParticleServerRpc(NetworkObjectReference particleNetworkObjectReference)
    {
        particleNetworkObjectReference.TryGet(out NetworkObject networkObject);
        NetworkObjectPool.Instance.ReturnNetworkObject(networkObject, hitParticlePrefab);
    }

}
