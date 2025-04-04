using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button yesExitButton;
    [SerializeField] private Button noExitButton;


    [SerializeField] private GameObject exitPopupObject;

    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(()  =>
        {
            Loader.Load(Loader.Scene.LobbyScene);
        });
        exitButton.onClick.AddListener(() =>
        {
            exitPopupObject.gameObject.SetActive(true);
        });
        noExitButton.onClick.AddListener(() =>
        {
            exitPopupObject.SetActive(false);
        });
        yesExitButton.onClick.AddListener(() =>
        {
            QuitGame();
        });
    }

    public void QuitGame()
    {
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
	}
}
