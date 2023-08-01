using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Transform mainCameraTransform;

    private void Awake()
    {
        // assuming the main camera is tagged as "MainCamera"
        mainCameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        // face the camera by rotating towards it
        transform.LookAt(mainCameraTransform);
    }
}
