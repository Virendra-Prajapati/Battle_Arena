using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private const string MUSIC_VOLUME_PLAYERPREFS = "MusicVolume";

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public float MusicVolume
    {
        get => PlayerPrefs.GetFloat(MUSIC_VOLUME_PLAYERPREFS, 0.7f);
        private set => PlayerPrefs.SetFloat(MUSIC_VOLUME_PLAYERPREFS, value);
    }

}
