using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Unity.Netcode;

public class Test : MonoBehaviour
{
    [SerializeField] private GameObject VrPrefab;
    [SerializeField] private GameObject prefab;
    [SerializeField] private NetworkManager networkManager;

    private void Awake()
    {
        if (UnityEngine.XR.XRSettings.enabled)
        {
            networkManager.NetworkConfig.PlayerPrefab = VrPrefab;

        }
        else
        {
            networkManager.NetworkConfig.PlayerPrefab = prefab;

        }
    }
}
