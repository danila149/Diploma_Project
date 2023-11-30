using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;



public class GetPlayerCamera : MonoBehaviour
{
    [SerializeField] Transform playerCamera;
    private void Start()
    {
        NetworkObject thisObject = GetComponent<NetworkObject>();

        if (thisObject.HasInputAuthority)
        {
            GameObject virtualCamera = GameObject.Find("PlayerFollowCamera");
            virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = playerCamera;

            GetComponent<MovePerson>().enabled = true;
        }
    }
}
