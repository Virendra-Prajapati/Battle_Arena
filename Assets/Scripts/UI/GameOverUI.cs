using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerWinText;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainScene);
        });
    }


    private void Start()
    {
        PlayAreaManager.Instance.OnStateChanged += PlayAreaManager_OnStateChanged;
        Hide();
    }

    private void PlayAreaManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (PlayAreaManager.Instance.IsGameOver())
        {
            foreach (KeyValuePair<ulong, int> keyValuePair in PlayAreaManager.Instance.GetPlayerScoreList())
            {
                if(keyValuePair.Value == PlayAreaManager.MAX_SCORE_COUNT)
                {
                    playerWinText.text = BattleArenaMultiplayer.Instance.GetPlayerDataFromClientId(keyValuePair.Key).playerName.ToString() + " <color=green>WIN</color>";
                    break;
                }
                    
            }
            
            Show();
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);   
    }

    private void Show()
    {
        gameObject.SetActive(true);
        GameInput.Instance.ShowMouseCursor();
    }
}
