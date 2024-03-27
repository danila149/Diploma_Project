using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button playBtn;
        [SerializeField] private Button settingsBtn;
        [SerializeField] private Button settingsBackBtn;
        [SerializeField] private Button quitBtn;
        [SerializeField] private Button joinBtn;
        [SerializeField] private Button joinBackBtn;
        [SerializeField] private Button createBtn;
        [SerializeField] private Button createBackBtn;
        [SerializeField] private Button createRoomBtn;

        [SerializeField] private GameObject settings;
        [SerializeField] private GameObject craeteOrJoinPanel;
        [SerializeField] private GameObject joinPanel;
        [SerializeField] private GameObject craetePanel;

        void Start()
        {
            quitBtn.onClick.AddListener(Quit);
            settingsBtn.onClick.AddListener(delegate { ShowPanel(settings); });
            settingsBackBtn.onClick.AddListener(delegate { ClosePanel(settings); });
            playBtn.onClick.AddListener(JoinOrCreate);
            joinBtn.onClick.AddListener(delegate { ShowPanel(joinPanel); });
            joinBackBtn.onClick.AddListener(delegate { ClosePanel(joinPanel); });
            createBtn.onClick.AddListener(delegate { ShowPanel(craetePanel); });
            createBackBtn.onClick.AddListener(delegate { ClosePanel(craetePanel); });
            createRoomBtn.onClick.AddListener(ResetPanels);
        }

        private void ResetPanels()
        {
            settings.SetActive(false);
            craeteOrJoinPanel.SetActive(false);
            joinPanel.SetActive(false);
            craetePanel.SetActive(false);
        }

        private void JoinOrCreate() =>
            craeteOrJoinPanel.SetActive(!craeteOrJoinPanel.activeInHierarchy);


        private void ShowPanel(GameObject panel) =>
            panel.SetActive(true);

        private void ClosePanel(GameObject panel) =>
            panel.SetActive(false);

        private void Quit() =>
            Application.Quit();
    }
}