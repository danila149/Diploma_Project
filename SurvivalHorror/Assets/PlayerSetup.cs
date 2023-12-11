using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private GameObject camera;
    [SerializeField] private TextMeshPro nicknameText;

    private string nickname;

    public void IsLocalPlayer()
    {
        movement.enabled = true;
        camera.SetActive(true);
        nicknameText.gameObject.SetActive(false);
    }

    [PunRPC]
    public void SetNickname(string newNickname)
    {
        nickname = newNickname;

        nicknameText.text = nickname;
    }
}