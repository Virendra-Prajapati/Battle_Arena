using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class BaseWeapon : NetworkBehaviour, IInteractable
{
    [SerializeField] protected Transform shootTransform;
    [SerializeField] protected LayerMask playerLayerMask;

    protected NetworkVariable<int> currentAmmo = new NetworkVariable<int>(0);

    private ThirdPersonShooterController shooterController;
    private Collider attachedCollider;
    private Transform holder;

    public InteractableItemName ItemName => InteractableItemName.Pistol;

    private void Awake()
    {
        attachedCollider = GetComponent<Collider>();
    }

    public override void OnNetworkSpawn()
    {
        Reload();
    }

    public void Reload()
    {
        currentAmmo.Value = GetMaxAmmoCount();
    }

    public abstract void Shoot(Vector3 targetPosition);

    public void Interact(NetworkObject playerNetworkObject)
    {
        playerNetworkObject.TryGetComponent(out shooterController);
        shooterController.SetBaseWeapon(this);
        currentAmmo.OnValueChanged += shooterController.OnWeaponAmmunationUpdate;
        OnPickupServerRpc(playerNetworkObject);
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnPickupServerRpc(NetworkObjectReference playerNetworkObjectReference)
    {
        OnPickupClientRpc(playerNetworkObjectReference);
    }

    [ClientRpc]
    private void OnPickupClientRpc(NetworkObjectReference playerNetworkObjectReference)
    {
        playerNetworkObjectReference.TryGet(out NetworkObject playerNetworkObject);
        playerNetworkObject.TryGetComponent(out shooterController);
        holder = shooterController.GetWeaponHolder();
        attachedCollider.enabled = false;
    }

    public void OnDropWeapon()
    {
        currentAmmo.OnValueChanged -= shooterController.OnWeaponAmmunationUpdate;
        shooterController.DropHoldingWeapon();
        shooterController = null;
        OnDropServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnDropServerRpc()
    {
        OnDropClientRpc();
    }

    [ClientRpc]
    private void OnDropClientRpc()
    {
        holder = null;
        attachedCollider.enabled = true;
        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, Mathf.Infinity))
        {
            transform.position = hitInfo.point; 
            transform.eulerAngles = new Vector3(0,0, 90);
        }

    }

    protected ulong GetPlayerClientId()
    {
        return NetworkManager.Singleton.LocalClientId;
    }
    
    private void Update()
    {
        if(holder == null) return;

        transform.SetPositionAndRotation(holder.position, holder.rotation);
    }

    public int GetCurrentAmmoCount()
    {
        return currentAmmo.Value;
    }

    public abstract int GetMaxAmmoCount();
}
