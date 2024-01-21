using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using Photon.Voice.PUN;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance { get; private set; }

    [SerializeField] private GameObject playerPrefab;
    [Space]
    [SerializeField] private Transform spawnPos;
    [SerializeField] private GameObject roomCam;
    [Header("UI")]
    [SerializeField] private GameObject connectionScreen;
    [SerializeField] private GameObject nicknameScreen;
    [SerializeField] private Button joinBtn;
    [SerializeField] private TMP_InputField inputNickname;

    private string nickname = "Player";
    public string roomNameToJoin = "test";

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        joinBtn.onClick.AddListener(JoinRoomButtonPressed);
        inputNickname.onValueChanged.AddListener(ChangeNickname);
    }


    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        roomCam.SetActive(false);
        RespawnPlayer();
    }

    public void RespawnPlayer()
    {
        GameObject _player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPos.position, Quaternion.identity);
        _player.GetComponent<PhotonView>().RPC("SetNickname", RpcTarget.AllBuffered, nickname);
        _player.GetComponent<PlayerSetup>().IsLocalPlayer();
        _player.GetComponent<Flashlight>().IsLocal = true;
        _player.GetComponent<PickUpSystem>().IsLocal = true;
        _player.GetComponent<CraftingSystem>().IsLocal = true;
        _player.GetComponent<HealthSytem>().IsLocal = true;
        PunVoiceClient.Instance.ConnectAndJoinRoom();
    }

    public void ChangeNickname(string newNickname)
    {
        nickname = newNickname;
    }

    public void JoinRoomButtonPressed()
    {
        joinBtn.onClick.RemoveListener(JoinRoomButtonPressed);
        inputNickname.onValueChanged.RemoveListener(ChangeNickname);
        nicknameScreen.SetActive(false);
        connectionScreen.SetActive(true);

        PhotonNetwork.JoinOrCreateRoom(roomNameToJoin, null, null);
    }
}
