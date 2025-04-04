using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCreateUI : MonoBehaviour
{
    public static string DEFAULT_LOBBY_NAME = "Lobby Name";

    [SerializeField] private Button closeButton;
    [SerializeField] private Button createPublicButton;
    [SerializeField] private Button createPrivateButton;
    [SerializeField] private TMP_InputField lobbyNameInputField;
    [SerializeField] private Transform lobbyContainer;

    private void Awake()
    {
        closeButton.onClick.AddListener(() =>
        {
            Hide();
            lobbyContainer.gameObject.SetActive(true);
        });
        createPublicButton.onClick.AddListener(() =>
        {
            BattleArenaLobby.Instance.CreateLobby(lobbyNameInputField.text, false);
        });
        createPrivateButton.onClick.AddListener(() =>
        {
            BattleArenaLobby.Instance.CreateLobby(lobbyNameInputField.text, true);
        });
    }

    private void Start()
    {
        lobbyNameInputField.text = DEFAULT_LOBBY_NAME;
        Hide();   
    }


    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
