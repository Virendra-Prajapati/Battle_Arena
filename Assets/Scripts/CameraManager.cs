using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera, followVirtualCamera;

    private void Awake()
    {
        Instance = this;
    }

    public void SetCameraFollowTransform(Transform target)
    {
        aimVirtualCamera.Follow = target;
        followVirtualCamera.Follow = target;
    }

    public CinemachineVirtualCamera GetAimVirtualCamera()
    {
        return aimVirtualCamera;
    }
}
