using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button createLobbyButton;
    [SerializeField] private Button quickJoinButton;
    [SerializeField] private Button joinCodeButton;

    [SerializeField] private TMP_InputField lobbyCodeInputField;
    [SerializeField] private TMP_InputField playerNameInputField;

    [SerializeField] private LobbyCreateUI lobbyCreateUI;

    [SerializeField] private Transform lobbyContainer;
    [SerializeField] private Transform lobbyTemplate;


    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            BattleArenaLobby.Instance.LeaveLobby();
            Loader.Load(Loader.Scene.MainScene);
        });
        createLobbyButton.onClick.AddListener(() =>
        {
            lobbyCreateUI.Show();
            lobbyContainer.gameObject.SetActive(false);
        });
        quickJoinButton.onClick.AddListener(() =>
        {
            BattleArenaLobby.Instance.QuickJoin();
        });
        joinCodeButton.onClick.AddListener(() =>
        {
            BattleArenaLobby.Instance.JoinWithCode(lobbyCodeInputField.text);
        });
        lobbyTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        playerNameInputField.text = BattleArenaMultiplayer.Instance.GetPlayerName();
        playerNameInputField.onValueChanged.AddListener((string newText) =>
        {
            BattleArenaMultiplayer.Instance.SetPlayerName(newText);
        });

        BattleArenaLobby.Instance.OnLobbyListChanged += BattleArenaLobby_OnLobbyListChanged;
        UpdateLobbyList(new List<Lobby>());
    }

    private void BattleArenaLobby_OnLobbyListChanged(object sender, BattleArenaLobby.OnLobbyListChangedEventArgs e)
    {
        UpdateLobbyList(e.lobbyList);
    }

    private void UpdateLobbyList(List<Lobby> lobbyList)
    {

        foreach(Transform child in lobbyContainer)
        {
            if(child == lobbyTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach(Lobby lobby in lobbyList)
        {
            Transform lobbyTransform = Instantiate(lobbyTemplate, lobbyContainer);
            lobbyTransform.gameObject.SetActive(true);
            lobbyTransform.GetComponent<LobbyListSingleUI>().SetLobby(lobby);
        }
    }

    private void OnDestroy()
    {
        BattleArenaLobby.Instance.OnLobbyListChanged -= BattleArenaLobby_OnLobbyListChanged;
        
    }
}
