using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SingleScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI playerScoreText;


    public void UpdateValues(ulong clientId, int points)
    {
        playerScoreText.text = points.ToString();
        playerNameText.text = BattleArenaMultiplayer.Instance.GetPlayerDataFromClientId(clientId).playerName.ToString();
    }
}
