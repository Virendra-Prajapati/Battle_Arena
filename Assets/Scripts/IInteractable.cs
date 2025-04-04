using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public interface IInteractable
{
    public InteractableItemName ItemName { get; }
    public void Interact(NetworkObject networkObject);
}

public enum InteractableItemName
{
    Pistol,
    HealthPack
}
