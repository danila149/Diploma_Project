using Photon.Pun;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] private Transform hand;
    [SerializeField] private LayerMask itemLayerMask;
    [SerializeField] private float distanceToPickUp;
    [SerializeField] private GameObject itemDestroyer;
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject aim;
    [SerializeField] private Transform pickupItemInfoParent;
    [SerializeField] private GameObject pickupItemInfoPrefab;

    public bool IsLocal { get; set; }

    private Item _item = null;

    private void Awake()
    {
        itemDestroyer = GameObject.FindGameObjectWithTag("itemDestroyer");
    }

    void Update()
    {
        if (IsLocal)
        {
            RaycastHit hit;
            if (Physics.Raycast(head.position, head.forward, out hit, distanceToPickUp, itemLayerMask))
            {
                aim.SetActive(true);
                _item = hit.transform.gameObject.GetComponent<Item>();
                if (Input.GetKeyDown(KeyCode.E))
                    PickUp(_item);
            }
            else
            {
                aim.SetActive(false);
            }
        }
    }

    public void PickUp(Item currentItem)
    {
        if (currentItem.GetType() == typeof(Equipment))
            if (currentItem.ToEquipment().IsEquiped)
                return;

        if (inventory.AddItem(currentItem))
        {
            PickUpItemInfo pickUpItemInfo;
            GetComponent<PhotonView>().RPC("DisableItem", RpcTarget.AllBuffered, currentItem.transform.position);

            if (currentItem.GetType() == typeof(Equipment))
            {
                GetComponent<PhotonView>().RPC("TakeEquipment", RpcTarget.AllBuffered, currentItem.transform.position, transform.position);

                pickUpItemInfo = Instantiate(pickupItemInfoPrefab, pickupItemInfoParent).GetComponent<PickUpItemInfo>();
                pickUpItemInfo.SetInfo("1", currentItem.ItemIcon, currentItem.ToEquipment().EquipmentType.ToString());
                return;
            }

            pickUpItemInfo = Instantiate(pickupItemInfoPrefab, pickupItemInfoParent).GetComponent<PickUpItemInfo>();
            pickUpItemInfo.SetInfo(currentItem.ToResource().Amount.ToString(), currentItem.ItemIcon, currentItem.ToResource().ResourceType.ToString());
        }
    }

    [PunRPC]
    private void DisableItem(Vector3 itemPos)
    {
        itemDestroyer.SetActive(true);
        itemDestroyer.transform.position = itemPos;
    }

    [PunRPC]
    private void TakeEquipment(Vector3 itemPos, Vector3 playerPos)
    {
        itemDestroyer.GetComponent<ItemDestroyer>().TakeItem(playerPos);
    }
}
