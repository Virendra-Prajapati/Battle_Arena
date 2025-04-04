using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkSelectionUI : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    private void Start()
    {
        hostButton.onClick.AddListener(() =>
        {
            Debug.Log("StartHost");
            NetworkManager.Singleton.StartHost();
            Hide();
        });
        clientButton.onClick.AddListener(() =>
        {
            Debug.Log("StartClient");
            NetworkManager.Singleton.StartClient();
            Hide();
        });
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}
