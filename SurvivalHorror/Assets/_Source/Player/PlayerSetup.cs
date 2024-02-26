using Character;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private CraftingSystem crafting;
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject hud;
    [SerializeField] private TextMeshPro nicknameText;
    [SerializeField] private InputListener inputListener;

    public void IsLocalPlayer()
    {
        movement.enabled = true;
        crafting.enabled = true;
        inventory.enabled = true;
        inputListener.gameObject.SetActive(true);
        camera.SetActive(true);
        hud.SetActive(true);
        nicknameText.gameObject.SetActive(false);
    }

    [PunRPC]
    public void SetNickname(string newNickname)
    {
        nicknameText.text = newNickname;
    }
}