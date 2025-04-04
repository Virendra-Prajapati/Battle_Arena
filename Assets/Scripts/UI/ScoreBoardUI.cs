using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoardUI : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private Transform singleBoardUI;

    private void Start()
    {
        PlayAreaManager.Instance.OnPlayerScoreUpdate += PlayAreaManager_OnPlayerScoreUpdate;
        singleBoardUI.gameObject.SetActive(false);
    }

    private void PlayAreaManager_OnPlayerScoreUpdate(object sender, System.EventArgs e)
    {
        UpdateBoard();
    }

    private void UpdateBoard()
    {
        foreach (Transform child in content)
        {
            if(child != singleBoardUI)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (KeyValuePair<ulong, int> keyValuePair in PlayAreaManager.Instance.GetPlayerScoreList())
        {
            Transform scoreUIObject = Instantiate(singleBoardUI, content);
            scoreUIObject.gameObject.SetActive(true);
            scoreUIObject.GetComponent<SingleScoreUI>().UpdateValues(keyValuePair.Key, keyValuePair.Value);
        }
    }
}
