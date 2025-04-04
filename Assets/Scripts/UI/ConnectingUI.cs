using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectingUI : MonoBehaviour
{

    private void Start()
    {
        BattleArenaMultiplayer.Instance.OnTryingToJoinGame += BattleArenaMultiplayer_OnTryingToJoinGame;
        BattleArenaMultiplayer.Instance.OnFailedToJoinGame += BattleArenaMultiplayer_OnFailedToJoinGame;
        Hide();
    }

    private void BattleArenaMultiplayer_OnFailedToJoinGame(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void BattleArenaMultiplayer_OnTryingToJoinGame(object sender, System.EventArgs e)
    {
        Show();
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
        BattleArenaMultiplayer.Instance.OnTryingToJoinGame -= BattleArenaMultiplayer_OnTryingToJoinGame;
        BattleArenaMultiplayer.Instance.OnFailedToJoinGame -= BattleArenaMultiplayer_OnFailedToJoinGame;
    }
}
