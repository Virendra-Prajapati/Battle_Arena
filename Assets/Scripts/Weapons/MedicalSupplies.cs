using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MedicalSupplies : NetworkBehaviour, IInteractable
{
    public InteractableItemName ItemName => InteractableItemName.HealthPack;

    public void Interact(NetworkObject networkObject)
    {
        OnPickupServerRpc(networkObject);
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnPickupServerRpc(NetworkObjectReference playerNetworkObjectReference)
    {
        OnPickupClientRpc(playerNetworkObjectReference);
        if (IsServer)
        {
            GetComponent<NetworkObject>().Despawn();
            Destroy(gameObject);
        }
    }

    [ClientRpc]
    private void OnPickupClientRpc(NetworkObjectReference playerNetworkObjectReference)
    {
        playerNetworkObjectReference.TryGet(out NetworkObject playerNetworkObject);
        playerNetworkObject.TryGetComponent(out PlayerHealth playerHealth);
        if(playerHealth != null)
        {
            playerHealth.ResetHealth();
        }
    }
}
