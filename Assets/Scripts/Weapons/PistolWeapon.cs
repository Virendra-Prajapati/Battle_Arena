using DG.Tweening;
using Unity.Netcode;
using UnityEngine;

public class PistolWeapon : BaseWeapon
{
    [SerializeField] private ParticleSystem shootParticles;
    [SerializeField] private Light pointLight;

    public override void Shoot(Vector3 targetPosition)
    {
        if(currentAmmo.Value <= 0)
            return;
        Vector3 aimDir = (targetPosition - shootTransform.position).normalized;
        if (Physics.Raycast(shootTransform.position, aimDir, out RaycastHit hitInfo, Mathf.Infinity, playerLayerMask))
        {
            if (hitInfo.transform.TryGetComponent(out IHealth healthInterface))
            {
                healthInterface.DealDamage(10, GetPlayerClientId());
            }
        }
        ParticleSpawner.Instance.SpawnHitParticle(targetPosition);
        ShowEffectServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void ShowEffectServerRpc()
    {
        //currentAmmo.Value -= 1;
        ShowEffectClientRpc();
    }

    [ClientRpc]
    private void ShowEffectClientRpc()
    {
        shootParticles.Play();
        DOTween.KillAll();
        pointLight.intensity = 4;
        pointLight.DOIntensity(0, 0.2f);
    }


    public override int GetMaxAmmoCount()
    {
        return 5;
    }
}
