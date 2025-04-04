using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HitParticleScript : NetworkBehaviour
{
    [SerializeField] private NetworkObject networkObject;
    private void OnParticleSystemStopped()
    {
        if(!IsServer) return;
        ParticleSpawner.Instance.DespawnHitParticle(networkObject);
    }
}
