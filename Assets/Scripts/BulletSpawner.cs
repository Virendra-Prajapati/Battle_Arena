using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletSpawner : NetworkBehaviour
{
    public static BulletSpawner Instance { get; private set; }

    [SerializeField] private GameObject bulletProjectile;

    private void Awake()
    {
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        if(!IsServer) return;
        NetworkObjectPool.Instance.Initialize();
    }

    public override void OnNetworkDespawn()
    {
        if(!IsServer) return;
        NetworkObjectPool.Instance.Terminate();
    }


    public void SpwanBullet(Vector3 targetPosition, Vector3 position , Quaternion rotation)
    {
        SpawnBulletServerRpc(targetPosition, position, rotation);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnBulletServerRpc(Vector3 targetPosition, Vector3 position , Quaternion rotation)
    {
        NetworkObject networkObject = NetworkObjectPool.Instance.GetNetworkObject(bulletProjectile, position, rotation);
        networkObject.GetComponent<BulletProjectile>().Setup(targetPosition, 150f);
    }

    public void DespawnBullet(NetworkObject networkObject)
    {
        DespawnBulletServerRpc(networkObject);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DespawnBulletServerRpc(NetworkObjectReference bulletNetworkObjectReference)
    {
        bulletNetworkObjectReference.TryGet(out NetworkObject networkObject);
        NetworkObjectPool.Instance.ReturnNetworkObject(networkObject, bulletProjectile);
    }
}
