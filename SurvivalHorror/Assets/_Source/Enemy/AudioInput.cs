using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AudioInput : MonoBehaviour
{
    void Start()
    {
        foreach (var microphone in Microphone.devices)
        {
            Debug.Log("Name: " + microphone);
        }
    }

}
