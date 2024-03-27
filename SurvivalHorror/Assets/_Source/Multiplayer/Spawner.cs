using Photon.Pun;
using Photon.Voice.PUN;
using UnityEngine;

public class Spawner : MonoBehaviourPunCallbacks
{
    private const string NICKNAME = "NICKNAME";

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPos;

    private string _nickname;

    void Start()
    {
        _nickname = PlayerPrefs.GetString(NICKNAME);
        RespawnPlayer();
    }

    public void RespawnPlayer()
    {
        GameObject _player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPos.position, Quaternion.identity);
        _player.GetComponent<PhotonView>().RPC("SetNickname", RpcTarget.AllBuffered, _nickname);
        _player.GetComponent<PlayerSetup>().IsLocalPlayer();
        _player.GetComponent<Flashlight>().IsLocal = true;
        _player.GetComponent<PickUpSystem>().IsLocal = true;
        _player.GetComponent<CraftingSystem>().IsLocal = true;
        _player.GetComponent<HealthSytem>().IsLocal = true;
        PunVoiceClient.Instance.ConnectAndJoinRoom();
    }
}
