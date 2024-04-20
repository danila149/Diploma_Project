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
    [Header("Filter")]
    [SerializeField] private Toggle passwordToggle;
    [SerializeField] private Toggle fullServerToggle;
    [SerializeField] private Button filtreByNameBtn;
    [SerializeField] private Button filtreByPlayerCountBtn;

    private string _currentPassword;
    private List<RoomInfo> _cachedRoomList = new List<RoomInfo>();
    private Dictionary<RoomInfo, GameObject> shownList = new Dictionary<RoomInfo, GameObject>();

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
        filtreByNameBtn.onClick.AddListener(GroupByName);
        filtreByPlayerCountBtn.onClick.AddListener(GroupByPlayerCount);
        passwordToggle.onValueChanged.AddListener(FilterByPassword);
        fullServerToggle.onValueChanged.AddListener(FilterByPlayerCount);
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

    public void GroupByName()
    {
        _cachedRoomList.Sort(delegate (RoomInfo x, RoomInfo y)
        {
            if (x.Name == null && y.Name == null) return 0;
            else if (x.Name == null) return -1;
            else if (y.Name == null) return 1;
            else return x.Name.CompareTo(y.Name);
        });
        UpdateUI();
    }

    public void GroupByPlayerCount()
    {
        _cachedRoomList.Sort(delegate (RoomInfo x, RoomInfo y)
        {
            return x.PlayerCount.CompareTo(y.PlayerCount);
        });
        UpdateUI();
    }

    private void FilterByPassword(bool value)
    {
        List<RoomInfo> extraList = new List<RoomInfo>(_cachedRoomList);
        if (value)
        {
            foreach (var room in extraList)
            {
                if ((bool)room.CustomProperties["private"])
                {
                    shownList[room].SetActive(false);
                }
            }
        }
        else
        {
            foreach (var room in extraList)
            {
                if ((bool)room.CustomProperties["private"])
                {
                    shownList[room].SetActive(true);
                }
            }
        }
    }

    private void FilterByPlayerCount(bool value)
    {
        List<RoomInfo> extraList = new List<RoomInfo>(_cachedRoomList);
        if (!value)
        {
            foreach (var room in extraList)
            {
                if (room.MaxPlayers == room.PlayerCount)
                {
                    shownList[room].SetActive(false);
                }
            }
        }
        else
        {
            foreach (var room in extraList)
            {
                if (room.MaxPlayers == room.PlayerCount)
                {
                    shownList[room].SetActive(true);
                }
            }
        }
    }

    private void UpdateUI()
    {
        foreach (Transform roomItem in roomListParent)
        {
            Destroy(roomItem.gameObject);
            shownList.Clear();
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
            shownList.Add(room, _roomitem);
            FilterByPassword(passwordToggle.isOn);
            FilterByPlayerCount(fullServerToggle.isOn);
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
