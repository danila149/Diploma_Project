using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class Flashlight : MonoBehaviour
{
    [SerializeField] private Light flashlight;
    [SerializeField] private float lightPower;
    public bool IsLocal { get; set; }

    void Start()
    {
        flashlight.intensity = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(IsLocal)
                GetComponent<PhotonView>().RPC("Use", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void Use()
    {
        if (flashlight.intensity <= 0)
            flashlight.intensity = lightPower;
        else
            flashlight.intensity = 0;
    }
}
