using Photon.Pun;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] private LayerMask itemLayerMask;
    [SerializeField] private float distanceToPickUp;
    [SerializeField] private GameObject itemDestroyer;

    public bool IsLocal { get; set; }

    private int _itemLayer;
    private Item _item = null;

    void Start()
    {
        _itemLayer = 1 << itemLayerMask | 1 << 2;
    }

    private void Awake()
    {
        itemDestroyer = GameObject.FindGameObjectWithTag("itemDestroyer");
    }

    void Update()
    {
        if (IsLocal)
        {
            RaycastHit hit;
            if (Physics.Raycast(head.position, head.forward, out hit, distanceToPickUp, ~_itemLayer))
            {
                _item = hit.transform.gameObject.GetComponent<Item>();
                _item.ShowText(true);
                if (Input.GetKeyDown(KeyCode.E))
                    GetComponent<PhotonView>().RPC("PickUp", RpcTarget.AllBuffered, _item.transform.position);
            }
            else
            {
                if (_item != null)
                    _item.ShowText(false);
            }
        }
    }

    [PunRPC]
    public void PickUp(Vector3 itemPos)
    {
        itemDestroyer.transform.position = itemPos;
    }
}
