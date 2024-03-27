using UnityEngine;

namespace Character
{
    public class InputListener : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private PlayerMovement playerMovement;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseMenu.SetActive(true);
                playerMovement.PlayerInput = false;
            }
        }
    }
}