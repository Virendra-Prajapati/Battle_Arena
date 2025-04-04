using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private PlayerInput _playerInput;
    private StarterAssetsInputs starterAssetsInputs;

    private void Awake()
    {
        Instance = this; 
        _playerInput = GetComponent<PlayerInput>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }

    private void Start()
    {
        HideMouseCurshor();
    }

    public PlayerInput GetPlayerInput() { return _playerInput; }

    public StarterAssetsInputs GetStarterAssetsInputs() { return starterAssetsInputs; }

    public void ShowMouseCursor()
    {
        starterAssetsInputs.cursorLocked = false;
    }

    public void HideMouseCurshor()
    {
        starterAssetsInputs.cursorLocked = true;
    }
}
