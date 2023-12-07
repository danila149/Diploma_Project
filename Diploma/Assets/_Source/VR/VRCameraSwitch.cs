using UnityEngine;
using UnityEngine.InputSystem.XR;

public class VRCameraSwitch : MonoBehaviour
{
    [SerializeField] private TrackedPoseDriver trackedPoseDriver;
    [SerializeField] private GameObject originXR;

    void Start()
    {
        if (UnityEngine.XR.XRSettings.enabled)
        {
            trackedPoseDriver.enabled = true;
        }
        else
        {
            trackedPoseDriver.enabled = false;
            transform.parent = originXR.transform.parent;
            originXR.SetActive(false);
        }
    }
}
