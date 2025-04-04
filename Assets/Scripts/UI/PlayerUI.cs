using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;

    [SerializeField] GameObject weaponAmmoStatusGameobject;
    [SerializeField] TextMeshProUGUI weaponAmmoText;
    [SerializeField] GameObject interactNotificationGameobject;

    public void Start()
    {
        PlayAreaManager.Instance.OnStateChanged += PlayAreaManager_OnStateChanged;
        weaponAmmoStatusGameobject.SetActive(false);  
        interactNotificationGameobject.SetActive(false);
    }

    private void PlayAreaManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (PlayAreaManager.Instance.IsCountDownToStartActive())
        {
            ThirdPersonShooterController.LocalInstance.playerHealth.OnPlayerHealthUpdate += ThirdPersonShooterController_OnPlayerGetDamage;
            ThirdPersonShooterController.LocalInstance.OnPlayerWeaponHolding += ThirdPersonShooterController_OnPlayerWeaponHolding;
            ThirdPersonShooterController.LocalInstance.OnPlayerAmmunationUpdate += ThirdPersonShooterController_OnPlayerAmmunationUpdate;
            ThirdPersonShooterController.LocalInstance.OnInteractItemChange += ThirdPersonShooterController_OnInteractItemChange;
        }
    }

    private void ThirdPersonShooterController_OnInteractItemChange(object sender, System.EventArgs e)
    {
        interactNotificationGameobject.SetActive(ThirdPersonShooterController.LocalInstance.FoundAnythingToPick());
    }

    private void ThirdPersonShooterController_OnPlayerAmmunationUpdate(object sender, ThirdPersonShooterController.OnPlayerAmmoUpdateEventArgs e)
    {
        weaponAmmoText.text = $"{e.currentCount} / {e.totalCount}";
    }

    private void ThirdPersonShooterController_OnPlayerWeaponHolding(object sender, System.EventArgs e)
    {
        weaponAmmoStatusGameobject.SetActive(ThirdPersonShooterController.LocalInstance.IsPlayerHoldingAnyWeapon());
    }

    private void ThirdPersonShooterController_OnPlayerGetDamage(object sender, System.EventArgs e)
    {
        healthBarImage.fillAmount = (float)ThirdPersonShooterController.LocalInstance.playerHealth.GetCurrentHealth() / PlayerHealth.PlayerMaxHealth;
    }
}
