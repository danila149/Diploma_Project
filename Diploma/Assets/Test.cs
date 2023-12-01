#if ENABLE_VR || UNITY_GAMECORE
#define XR_MODULE_AVAILABLE
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        if (UnityEngine.XR.XRSettings.enabled)
            Debug.Log("vr");
    }
}
