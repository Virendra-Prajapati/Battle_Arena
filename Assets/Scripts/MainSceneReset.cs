using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MainSceneReset : MonoBehaviour
{
    private void Awake()
    {
        if(NetworkManager.Singleton != null)
        {
            Destroy(NetworkManager.Singleton.gameObject);
        }

        if(BattleArenaMultiplayer.Instance != null)
        {
            Destroy(BattleArenaMultiplayer.Instance.gameObject);
        }

        if(BattleArenaLobby.Instance != null)
        {
            Destroy(BattleArenaLobby.Instance.gameObject);
        }
    }
}
