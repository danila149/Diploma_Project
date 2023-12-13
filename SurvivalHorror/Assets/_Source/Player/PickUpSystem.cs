using Photon.Pun;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] private LayerMask itemLayerMask;
    [SerializeField] private float distanceToPickUp;
    [SerializeField] private GameObject itemDestroyer;
    [SerializeField] private Inventory inventory;

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
                PickUp(_item);
            }
            else
            {
                if (_item != null)
                    _item.ShowText(false);
            }
        }
    }

    public void PickUp(Item currentItem)
    {
        currentItem.ShowText(true);
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(inventory.AddItem(currentItem))
                GetComponent<PhotonView>().RPC("DisableItem", RpcTarget.AllBuffered, currentItem.transform.position);
        }
    }

    [PunRPC]
    private void DisableItem(Vector3 itemPos)
    {
        itemDestroyer.SetActive(true);
        itemDestroyer.transform.position = itemPos;
    }
}
