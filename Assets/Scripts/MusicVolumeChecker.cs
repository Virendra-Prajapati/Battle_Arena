using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicVolumeChecker : MonoBehaviour
{
    private void Start()
    {
        if(TryGetComponent(out AudioSource audioSource))
        {
            audioSource.volume = AudioManager.Instance.MusicVolume;
        }
    }
}
