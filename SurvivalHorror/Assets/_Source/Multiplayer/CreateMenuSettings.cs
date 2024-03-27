using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Realtime;

public class CreateMenuSettings : MonoBehaviour
{
    [SerializeField] private TMP_InputField lobbyNameInput;
    [SerializeField] private TMP_Dropdown playerCountInput;
    [SerializeField] private Toggle passwordToggle;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button createBtn;

    private void Start()
    {
        createBtn.onClick.AddListener(Create);
    }

    private void Update()
    {
        passwordInput.gameObject.SetActive(passwordToggle.isOn);
    }


    public void Create()
    {
        RoomOptions options = new RoomOptions();

        options.CustomRoomPropertiesForLobby = new string[] {"private", "password" };

        options.MaxPlayers = int.Parse(playerCountInput.captionText.text);
        options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
        options.CustomRoomProperties.Add("private", passwordToggle.isOn);
        if (passwordToggle.isOn)
            options.CustomRoomProperties.Add("password", passwordInput.text);
        RoomManager.Instance.roomNameToJoin = lobbyNameInput.text;
        RoomManager.Instance.options = options;
        createBtn.onClick.RemoveListener(Create);
        RoomManager.Instance.JoinRoomButtonPressed();
    }
}
