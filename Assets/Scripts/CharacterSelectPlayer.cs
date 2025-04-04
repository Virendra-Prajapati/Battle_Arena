using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectPlayer : MonoBehaviour
{
    [SerializeField] private int playerIndex;
    [SerializeField] private GameObject readyGameObject;

    [SerializeField] private PlayerVisual playerVisual;

    [SerializeField] private Button kickButton;

    [SerializeField] private TextMeshPro playerNameText;


    private void Awake()
    {
        kickButton.onClick.AddListener(() =>
        {
            PlayerData playerData = BattleArenaMultiplayer.Instance.GetPlayerDataFromPlayerIndex(playerIndex);
            BattleArenaLobby.Instance.KickPlayer(playerData.playerId.ToString());
            BattleArenaMultiplayer.Instance.KickPlayer(playerData.clientId);
        });
    }

    private void Start()
    {
        BattleArenaMultiplayer.Instance.OnPlayerDataNetworkListChanged += BattleArenaMultiplayer_OnPlayerDataNetworkListChanged;
        CharacterSelectReady.Instance.OnReadyChanged += CharacterSelectReady_OnReadChanged;

        kickButton.gameObject.SetActive(NetworkManager.Singleton.IsServer && playerIndex != 0);

        UpdatePlayer();
    }

    private void CharacterSelectReady_OnReadChanged(object sender, System.EventArgs e)
    {
        UpdatePlayer();
    }

    private void BattleArenaMultiplayer_OnPlayerDataNetworkListChanged(object sender, System.EventArgs e)
    {
        UpdatePlayer();
         
    }

    private void UpdatePlayer()
    {
        if (BattleArenaMultiplayer.Instance.IsPlayerIndexConnected(playerIndex))
        {
            Show();

            PlayerData playerData = BattleArenaMultiplayer.Instance.GetPlayerDataFromPlayerIndex(playerIndex);
            readyGameObject.SetActive(CharacterSelectReady.Instance.IsPlayerReady(playerData.clientId));
            playerNameText.text = playerData.playerName.ToString();

            playerVisual.SetPlayerAvatar(playerData.avatarId);
        }  
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        BattleArenaMultiplayer.Instance.OnPlayerDataNetworkListChanged -= BattleArenaMultiplayer_OnPlayerDataNetworkListChanged;
    }
}
