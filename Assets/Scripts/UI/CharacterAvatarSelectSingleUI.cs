using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAvatarSelectSingleUI : MonoBehaviour
{
    [SerializeField] private int avatarId;
    [SerializeField] private TextMeshProUGUI textID;
    [SerializeField] private GameObject selectedGameObject;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            BattleArenaMultiplayer.Instance.ChangePlayerAvatar(avatarId);
        });
    }

    private void Start()
    {
        BattleArenaMultiplayer.Instance.OnPlayerDataNetworkListChanged += BattleArenaMultiplayer_OnPlayerDataNetworkListChanged;
        textID.text = (avatarId + 1).ToString();  
        UpdateIsSelected();
    }

    private void BattleArenaMultiplayer_OnPlayerDataNetworkListChanged(object sender, System.EventArgs e)
    {
        UpdateIsSelected();
    }

    private void UpdateIsSelected()
    {
        if(BattleArenaMultiplayer.Instance.GetPlayerData().avatarId == avatarId)
        {
            selectedGameObject.SetActive(true);
        }
        else
        {
            selectedGameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        BattleArenaMultiplayer.Instance.OnPlayerDataNetworkListChanged -= BattleArenaMultiplayer_OnPlayerDataNetworkListChanged;
        
    }
}
