using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VR
{
    public class ActivateGrabRay : MonoBehaviour
    {
        [SerializeField] private GameObject leftGrabRay;
        [SerializeField] private GameObject rightGrabRay;

        [SerializeField] private XRDirectInteractor leftDirectGrab;
        [SerializeField] private XRDirectInteractor rightDirectGrab;

        void Update()
        {
            leftGrabRay.SetActive(leftDirectGrab.interactablesSelected.Count == 0);
            rightGrabRay.SetActive(rightDirectGrab.interactablesSelected.Count == 0);
        }
    }
}