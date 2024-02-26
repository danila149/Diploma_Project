using UnityEngine;
using UnityEngine.UI;

public class RoomitemButton : MonoBehaviour
{
    [SerializeField] private Button joinBtn;

    public string RoomName;

    private void Awake()
    {
        joinBtn.onClick.AddListener(OnButtonPresed);
    }

    public void OnButtonPresed()
    {
        RoomList.Instance.JoinRoomByName(RoomName);
        joinBtn.onClick.RemoveListener(OnButtonPresed);
    }
}
