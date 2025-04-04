using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.Animations.Rigging;
using Unity.Netcode;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.EventSystems;
using System;

public class ThirdPersonShooterController : NetworkBehaviour
{
    public static ThirdPersonShooterController LocalInstance { get; private set; }

    [HideInInspector]
    public PlayerHealth playerHealth;

    public event EventHandler OnInteractItemChange;
    public event EventHandler OnPlayerWeaponHolding;
    public event EventHandler<OnPlayerAmmoUpdateEventArgs> OnPlayerAmmunationUpdate;
    public class OnPlayerAmmoUpdateEventArgs : EventArgs
    {
        public int currentCount;
        public int totalCount;
    } 

    private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private PlayerVisual playerVisual;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimLayerMask, weaponLayerMask;
    [SerializeField] private Transform aimPosition;
    [SerializeField] private Rig aimLookRig;
    [SerializeField] private Transform cameraFollowTarget;
    [SerializeField] private Transform pistolHolder;
    [SerializeField] private BaseWeapon holdingWeapon;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Animator animator;
    private float respawningTimer;

    private IInteractable interactableItem;
    
    public bool IsDead { get; private set; }

    public StarterAssetsInputs StarterAssetsInputs { get { return starterAssetsInputs; }}

    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GameInput.Instance.GetStarterAssetsInputs();
        animator = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();    
    }

    private void Start()
    {
        respawningTimer = 5;
        playerHealth.ResetHealth();
        playerHealth.OnPlayerHealthUpdate += PlayerHealth_OnPlayerGetDamage;
        PlayerData playerData = BattleArenaMultiplayer.Instance.GetPlayerDataFromClientId(OwnerClientId);
        playerVisual.SetPlayerAvatar(playerData.avatarId);
    }

    public override void OnNetworkSpawn()
    {
        if(IsOwner)
        {
            LocalInstance = this;
            
            CameraManager.Instance.SetCameraFollowTransform(cameraFollowTarget);
            aimVirtualCamera = CameraManager.Instance.GetAimVirtualCamera();
        }
        transform.position = PlayAreaManager.Instance.GetSpawnPosition(BattleArenaMultiplayer.Instance.GetPlayerDataIndexFromClientId(OwnerClientId));
    }

    private void PlayerHealth_OnPlayerGetDamage(object sender, System.EventArgs e)
    {
        IsDead = playerHealth.GetCurrentHealth() <= 0;
        if(IsDead)
        {
            animator.SetTrigger("Death");
        }
    }

    private void Update()
    {
        if(!IsOwner) return;

        if(!PlayAreaManager.Instance.IsGamePlaying()) return;

        if (IsDead)
        {
            if(respawningTimer > 0)
            {
                respawningTimer -= Time.deltaTime;
                //Debug.Log(Mathf.CeilToInt(respawningTimer));
            }
            else
            {
                respawningTimer = 5;
                animator.Play("Idle Walk Run Blend");
                transform.position = PlayAreaManager.Instance.GetRandomSpwanPosition();
                playerHealth.ResetHealth();
                IsDead = false;
            }
            return;
        }

        Vector3 mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if(Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, aimLayerMask))
        {
            mouseWorldPosition = raycastHit.point;
            aimPosition.position = mouseWorldPosition;
        }

        Vector3 worldAimTarget = mouseWorldPosition;
        worldAimTarget.y = transform.position.y;
        Vector3 aimDirection = (worldAimTarget - transform.position).normalized;


        if (starterAssetsInputs.aim && IsPlayerHoldingAnyWeapon())
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensititvity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1, Time.deltaTime * 10f));
            if(aimLookRig.weight != 1)
            {
                OnRigWeightChangedServerRpc(1);
            }
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);

            
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensititvity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0, Time.deltaTime * 10f));
            if(aimLookRig.weight != 0)
            {
                OnRigWeightChangedServerRpc(0);
            }
        }

        Shoot(mouseWorldPosition);
        DropWeapon();
        Interact(aimDirection);
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnRigWeightChangedServerRpc(float value)
    {
        OnRigWeightChangedClientRpc(value);
    }

    [ClientRpc]
    private void OnRigWeightChangedClientRpc(float value)
    {
        aimLookRig.weight = value;
    }

    private void DropWeapon()
    {
        if (starterAssetsInputs.drop)
        {
            if(!IsPlayerHoldingAnyWeapon()) return;
            holdingWeapon.OnDropWeapon();
            starterAssetsInputs.drop = false;
        }
    }

    private void Shoot(Vector3 mouseWorldPosition)
    {
        if (starterAssetsInputs.shoot)
        {
            if (starterAssetsInputs.aim)
            {
                if(!IsPlayerHoldingAnyWeapon()) return;
                if(holdingWeapon.GetCurrentAmmoCount() > 0)
                {
                    holdingWeapon.Shoot(mouseWorldPosition);
                }
            }
            starterAssetsInputs.shoot = false;
        }
    }

    public void OnWeaponAmmunationUpdate(int previosValue, int currentValue)
    {
        OnPlayerAmmunationUpdate?.Invoke(this, new OnPlayerAmmoUpdateEventArgs
        {
            currentCount = currentValue,
            totalCount = holdingWeapon.GetMaxAmmoCount()
        });
    }

    public void DropHoldingWeapon()
    {
        holdingWeapon = null;
        OnPlayerWeaponHolding?.Invoke(this, EventArgs.Empty);
    }

    public void SetBaseWeapon(BaseWeapon baseWeapon)
    {
        holdingWeapon = baseWeapon;
        OnPlayerWeaponHolding?.Invoke(this, EventArgs.Empty);
        OnPlayerAmmunationUpdate?.Invoke(this, new OnPlayerAmmoUpdateEventArgs
        {
            currentCount = holdingWeapon.GetCurrentAmmoCount(),
            totalCount = holdingWeapon.GetMaxAmmoCount()
        });
    }

    public Transform GetWeaponHolder()
    {
        return pistolHolder;
    }

    public bool IsPlayerHoldingAnyWeapon()
    {
        return holdingWeapon != null;
    }

    public bool FoundAnythingToPick()
    {
        return interactableItem != null;
    }

    private void Interact(Vector3 aimDirection)
    {
        Vector3 offset = new(0, 0.8f, 0);
        Vector3 halfExtents = new(0.5f, 1f, 0.5f);
        float m_MaxDistance = 0.8f;
        //ExtDebug.DrawBoxCastBox(transform.position + offset, halfExtents, Quaternion.identity, aimDirection, m_MaxDistance, Color.red);
        if(Physics.BoxCast(transform.position + offset, halfExtents, aimDirection, out RaycastHit hitInfo, Quaternion.identity, m_MaxDistance, weaponLayerMask))
        {
            if(hitInfo.transform.TryGetComponent(out IInteractable interactableItem))
            {
                if(this.interactableItem != interactableItem)
                {
                    this.interactableItem = interactableItem;
                    OnInteractItemChange?.Invoke(this, EventArgs.Empty);
                }
                
            }
            else if(interactableItem != null)
            {
                this.interactableItem = null;
                OnInteractItemChange?.Invoke(this, EventArgs.Empty);
            }
        }
        else if(interactableItem != null)
        {
            this.interactableItem = null;
            OnInteractItemChange?.Invoke(this, EventArgs.Empty);
        }
        if (starterAssetsInputs.interact)
        {
            if(interactableItem as BaseWeapon != null && IsPlayerHoldingAnyWeapon())
            {
                holdingWeapon.OnDropWeapon();
            }
            interactableItem?.Interact(GetComponent<NetworkObject>());
            starterAssetsInputs.interact = false;
        }   
    }
}
