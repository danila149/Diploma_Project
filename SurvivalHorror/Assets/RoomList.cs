using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomList : MonoBehaviourPunCallbacks
{
    public static RoomList Instance { get; private set; }

    [SerializeField] private GameObject roomManagerObjcet;
    [SerializeField] private RoomManager roomManager;
    [SerializeField] private Transform roomListParent;
    [SerializeField] private GameObject roomListItemPrefab;
    [Header("UI")]
    [SerializeField] private TMP_InputField inputLobbyName;

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
        Instance = this;
        inputLobbyName.onValueChanged.AddListener(ChangeRoomToCreateName);
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
            _roomitem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = room.PlayerCount + "/16";
            _roomitem.GetComponent<RoomitemButton>().RoomName = room.Name;
        }
    }

    public void JoinRoomByName(string name)
    {
        roomManager.roomNameToJoin = name;
        roomManagerObjcet.SetActive(true);
        inputLobbyName.onValueChanged.RemoveListener(ChangeRoomToCreateName);
        gameObject.SetActive(false);
    }

    public void ChangeRoomToCreateName(string roomName)
    {
        roomManager.roomNameToJoin = roomName;
    }
}
