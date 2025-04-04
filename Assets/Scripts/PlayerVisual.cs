using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] GameObject[] charactersVisuals;

    public void SetPlayerAvatar(int avatarId)
    {
        for (int i = 0; i < charactersVisuals.Length; i++)
        {
            if(i == avatarId)
            {
                charactersVisuals[i].SetActive(true);
            }
            else
            {
                charactersVisuals[i].SetActive(false);
            }
        }
    }
}
