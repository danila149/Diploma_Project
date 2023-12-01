using UnityEngine;
using Unity.Netcode;

[System.Serializable]
public class VRMap
{
    public Transform vrTarget;
    public Transform ikTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;
    public void Map()
    {
        ikTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        ikTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}

public class IKTargetFollowVRRig : NetworkBehaviour
{
    [Range(0,1)]
    public float turnSmoothness = 0.1f;
    public Transform root;
    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;

    public Vector3 headBodyPositionOffset;
    public float headBodyYawOffset;

    void LateUpdate()
    {
        if (IsOwner)
        {
            //root.position = VRRigReference.Singleton.root.position;
            //root.rotation = VRRigReference.Singleton.root.rotation;
            head.vrTarget = VRRigReference.Singleton.head;
            leftHand.vrTarget = VRRigReference.Singleton.leftHand;
            rightHand.vrTarget = VRRigReference.Singleton.rightHand;

            transform.position = head.ikTarget.position + headBodyPositionOffset;
            float yaw = head.vrTarget.eulerAngles.y;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, yaw, transform.eulerAngles.z), turnSmoothness);

            head.Map();
            leftHand.Map();
            rightHand.Map();
        }
    }
}
