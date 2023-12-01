using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkConnect : MonoBehaviour
{
    [SerializeField] private Button createBtn;
    [SerializeField] private Button joinBtn;

    private void Awake()
    {
        createBtn.onClick.AddListener(Create);
        joinBtn.onClick.AddListener(Join);
    }

    public void Create()=>
        NetworkManager.Singleton.StartHost();

    public void Join()=>
        NetworkManager.Singleton.StartClient();
}
