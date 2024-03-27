using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private Button continueBtn;
        [SerializeField] private Button settingsBtn;
        [SerializeField] private Button settingsBackBtn;
        [SerializeField] private Button leaveBtn;

        [SerializeField] private GameObject settings;
        [SerializeField] private PlayerMovement playerMovement;

        void Start()
        {
            continueBtn.onClick.AddListener(Continue);
            leaveBtn.onClick.AddListener(LeaveRoom);
            settingsBtn.onClick.AddListener(ShowSettings);
            settingsBackBtn.onClick.AddListener(CloseSettings);
        }

        private void Continue()
        {
            gameObject.SetActive(false);
            playerMovement.PlayerInput = true;
        }

        private void ShowSettings() =>
            settings.SetActive(true);

        private void CloseSettings() =>
            settings.SetActive(false);

        private void LeaveRoom()
        {
            PhotonNetwork.LeaveLobby();
            PhotonNetwork.LoadLevel(0);
        }
    }
}