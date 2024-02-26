using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using Photon.Voice.PUN;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    private const string NICKNAME = "NICKNAME";

    public static RoomManager Instance { get; private set; }

    [SerializeField] private GameObject playerPrefab;
    [Space]
    [SerializeField] private Transform spawnPos;
    [Header("UI")]
    [SerializeField] private GameObject connectionScreen;
    [SerializeField] private TMP_InputField inputNickname;

    private string nickname = "Player";
    public string roomNameToJoin = "test";
    public RoomOptions options = null;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        nickname = PlayerPrefs.GetString(NICKNAME);
        inputNickname.onValueChanged.AddListener(ChangeNickname);
    }


    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        connectionScreen.SetActive(false);
        PhotonNetwork.LoadLevel(1);
    }

    public void ChangeNickname(string newNickname)
    {
        nickname = newNickname;
        PlayerPrefs.SetString(NICKNAME, newNickname);
    }

    public void JoinRoomButtonPressed()
    {
        inputNickname.onValueChanged.RemoveListener(ChangeNickname);
        connectionScreen.SetActive(true);

        

        if (options != null)
        {
            PhotonNetwork.JoinOrCreateRoom(roomNameToJoin, options, null);
        }
        else
            PhotonNetwork.JoinOrCreateRoom(roomNameToJoin, null, null);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
