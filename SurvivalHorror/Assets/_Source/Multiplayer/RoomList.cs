using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class RoomList : MonoBehaviourPunCallbacks
{
    public static RoomList Instance { get; private set; }

    [SerializeField] private RoomManager roomManager;
    [SerializeField] private Transform roomListParent;
    [SerializeField] private GameObject roomListItemPrefab;
    [Header("UI")]
    [SerializeField] private TMP_InputField inputLobbyName;
    [SerializeField] private Button createRoomBtn;
    [SerializeField] private GameObject passwordPanel;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button passwordConfirm;
    [SerializeField] private GameObject error;

    private string _currentPassword;
    private List<RoomInfo> _cachedRoomList = new List<RoomInfo>();

    IEnumerator Start()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }

        yield return new WaitUntil(() => !PhotonNetwork.IsConnected);

        PhotonNetwork.ConnectUsingSettings();
    }

    private void Awake()
    {
        passwordConfirm.onClick.AddListener(JoinRoomWithPassword);
        Instance = this;
        createRoomBtn.onClick.AddListener(delegate { ChangeRoomToCreateName(inputLobbyName.text); });
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        PhotonNetwork.JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if(_cachedRoomList.Count <= 0)
        {
            _cachedRoomList = roomList;
        }
        else
        {
            foreach (var room in roomList)
            {
                for (int i = 0; i < _cachedRoomList.Count; i++)
                {
                    if(_cachedRoomList[i].Name == room.Name)
                    {
                        List<RoomInfo> newList = _cachedRoomList;

                        if (room.RemovedFromList)
                        {
                            newList.Remove(newList[i]);
                        }
                        else
                        {
                            newList[i] = room;
                            Debug.Log(room.CustomProperties.Count);
                        }

                        _cachedRoomList = newList;
                    }
                }
            }
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        foreach (Transform roomItem in roomListParent)
        {
            Destroy(roomItem.gameObject);
        }

        foreach (var room in _cachedRoomList)
        {
            GameObject _roomitem = Instantiate(roomListItemPrefab, roomListParent);
            _roomitem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = room.Name;
            _roomitem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = room.PlayerCount + "/" + room.MaxPlayers;
            foreach (var key in room.CustomProperties.Keys)
            {
                if(key.Equals("private"))
                {
                    _roomitem.transform.GetChild(2).gameObject.SetActive((bool)room.CustomProperties[key]);
                }
            }
            _roomitem.GetComponent<RoomitemButton>().RoomName = room.Name;
        }
    }

    public void JoinRoomByName(string name)
    {
        roomManager.roomNameToJoin = name;

        foreach (var room in _cachedRoomList)
        {
            if(room.Name == name)
            {
                foreach (var key in room.CustomProperties.Keys)
                {
                    if (key.Equals("password"))
                    {
                        _currentPassword = room.CustomProperties[key].ToString();
                        Debug.Log(_currentPassword);
                        error.SetActive(false);
                        passwordPanel.SetActive(true);
                        return;
                    }
                }
            }
        }
        roomManager.JoinRoomButtonPressed();
        createRoomBtn.onClick.RemoveListener(delegate { ChangeRoomToCreateName(inputLobbyName.text); });
    }

    private void JoinRoomWithPassword()
    {
        Debug.Log(_currentPassword + " " + passwordInput.text);
        if(_currentPassword == passwordInput.text)
        {
            passwordPanel.SetActive(false);
            error.SetActive(false);
            roomManager.JoinRoomButtonPressed();
        }
        else
        {
            passwordInput.text = "";
            error.SetActive(true);
        }
    }

    public void ChangeRoomToCreateName(string roomName)
    {
        roomManager.roomNameToJoin = roomName;
    }
}
