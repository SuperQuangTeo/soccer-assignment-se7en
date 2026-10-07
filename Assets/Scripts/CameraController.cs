using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineCamera;
   
    public void FollowObject(Transform obj)
    {
        cinemachineCamera.Follow = obj;
    }
}
