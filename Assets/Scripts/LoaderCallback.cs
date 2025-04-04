using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoaderCallback : MonoBehaviour
{
    private const float LOAD_MAX_TIME = 2f;

    [SerializeField] private Slider progressBar;

    private float timer = 0f;
    private bool isFirstUpdate = true;


    private void Update()
    {
        if (timer >= LOAD_MAX_TIME && isFirstUpdate)
        {
            progressBar.value = 1f;
            isFirstUpdate = false;
            Loader.LoaderCallback();
        }
        else
        {
            timer += Time.deltaTime;
            progressBar.value = timer / LOAD_MAX_TIME;    
        }
    }
}
