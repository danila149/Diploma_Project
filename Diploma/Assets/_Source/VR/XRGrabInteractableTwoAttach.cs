using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractableTwoAttach : XRGrabInteractable
{
    private const string LEFT_HAND_TAG = "Left Hand";
    private const string RIGHT_HAND_TAG = "Right Hand";

    [SerializeField] private Transform leftAttachTransform;
    [SerializeField] private Transform rightAttachTransform;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactorObject.transform.CompareTag(LEFT_HAND_TAG))
            attachTransform = leftAttachTransform;
        else if (args.interactorObject.transform.CompareTag(RIGHT_HAND_TAG))
            attachTransform = rightAttachTransform;

        base.OnSelectEntered(args);
    }
}
