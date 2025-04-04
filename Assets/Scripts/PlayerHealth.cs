using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour, IHealth
{
    public static int PlayerMaxHealth = 100;

    public event EventHandler OnPlayerHealthUpdate;

    private int currentHealth = 0;

    public void DealDamage(int damage, ulong clientId)
    {
        if(currentHealth <= 0) return;
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            PlayAreaManager.Instance.AddPointInPlayer(clientId);
        }
        UpdateHealthServerRpc(currentHealth);
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateHealthServerRpc(int damage)
    {
        UpdateHealthClientRpc(damage);
    }

    [ClientRpc]
    private void UpdateHealthClientRpc(int damage)
    {
        currentHealth = damage;
        OnPlayerHealthUpdate?.Invoke(this, EventArgs.Empty);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void ResetHealth()
    {
        currentHealth = PlayerMaxHealth;
        UpdateHealthServerRpc(currentHealth);
    }
}
