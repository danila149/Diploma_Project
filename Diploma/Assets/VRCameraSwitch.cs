using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class VRCameraSwitch : MonoBehaviour
{
    [SerializeField] private TrackedPoseDriver trackedPoseDriver;

    void Start()
    {
        if (UnityEngine.XR.XRSettings.enabled)
        {
            trackedPoseDriver.enabled = true;
        }
        else
        {
            trackedPoseDriver.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
