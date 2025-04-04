using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMessageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        closeButton.onClick.AddListener(Hide);
    }

    private void Start()
    {
        BattleArenaMultiplayer.Instance.OnFailedToJoinGame += BattleArenaMultiplayer_OnFailedToJoinGame;
        BattleArenaLobby.Instance.OnCreateLobbyStarted += BattleArenaLobby_OnCreateLobbyStarted;
        BattleArenaLobby.Instance.OnCreateLobbyFailed += BattleArenaLobby_OnCreateLobbyFailed;
        BattleArenaLobby.Instance.OnJoinStarted += BattleArenaLobby_OnJoinStarted;
        BattleArenaLobby.Instance.OnJoinFailed += BattleArenaLobby_OnJoinFailed;
        BattleArenaLobby.Instance.OnQuickJoinFailed += BattleArenaLobby_OnQuickJoinFailed;
        Hide();
    }

    private void BattleArenaLobby_OnJoinStarted(object sender, System.EventArgs e)
    {
        ShowMessage("Joining Lobby...");
        closeButton.gameObject.SetActive(false);    
    }
    private void BattleArenaLobby_OnJoinFailed(object sender, System.EventArgs e)
    {
        ShowMessage("Failed to Join Lobby!");
        closeButton.gameObject.SetActive(true);    
    }
    private void BattleArenaLobby_OnQuickJoinFailed(object sender, System.EventArgs e)
    {
        ShowMessage("Could not find a Lobby to Quick Join!");
    }

    private void BattleArenaLobby_OnCreateLobbyStarted(object sender, System.EventArgs e)
    {
        ShowMessage("Creating Lobby...");
        closeButton.gameObject.SetActive(false);    
    }
    private void BattleArenaLobby_OnCreateLobbyFailed(object sender, System.EventArgs e)
    {
        ShowMessage("Failed to create Lobby!");
        closeButton.gameObject.SetActive(true);    
    }

    private void BattleArenaMultiplayer_OnFailedToJoinGame(object sender, System.EventArgs e)
    {
        if(NetworkManager.Singleton.DisconnectReason == "")
        {
            ShowMessage("Failed to connect");
        }
        else
        {
            ShowMessage(NetworkManager.Singleton.DisconnectReason);
        }
    }

    private void ShowMessage(string message)
    {
        Show();
        messageText.text = message;
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
        BattleArenaMultiplayer.Instance.OnFailedToJoinGame -= BattleArenaMultiplayer_OnFailedToJoinGame;
        BattleArenaLobby.Instance.OnCreateLobbyStarted -= BattleArenaLobby_OnCreateLobbyStarted;
        BattleArenaLobby.Instance.OnCreateLobbyFailed -= BattleArenaLobby_OnCreateLobbyFailed;
        BattleArenaLobby.Instance.OnJoinStarted -= BattleArenaLobby_OnJoinStarted;
        BattleArenaLobby.Instance.OnJoinFailed -= BattleArenaLobby_OnJoinFailed;
        BattleArenaLobby.Instance.OnQuickJoinFailed -= BattleArenaLobby_OnQuickJoinFailed;
        
    }
}
