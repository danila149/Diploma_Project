using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem Instance { get; private set; }

    [SerializeField] private Transform head;
    [SerializeField] private Transform hand;
    [SerializeField] private LayerMask workbenchLayerMask;
    [SerializeField] private Inventory inventory;
    [SerializeField] private int distanceToUse;
    [SerializeField] private GameObject craftingPanel;
    [SerializeField] private GameObject hudUI;
    [SerializeField] private InfoPanel infoPanel;
    [SerializeField] private GameObject craftListItemPrefab;
    [SerializeField] private Transform craftListParent;
    
    private List<CraftData> craftables;
    private Dictionary<CraftData, CraftingListItem> craftingData;

    public bool IsLocal { get; set; }
    public bool IsCrafting { get; set; }

    private Workbench _workbench = null;
    private bool isInit;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (IsLocal)
        {

            RaycastHit hit;
            if (Physics.Raycast(head.position, head.forward, out hit, distanceToUse, workbenchLayerMask))
            {
                _workbench = hit.transform.gameObject.GetComponent<Workbench>();
                craftables = _workbench.Craftables;
                _workbench.ShowText(true);

                if (!Inventory.Instance.IsInvetoryOpen && Input.GetKeyDown(KeyCode.E))
                    ShowCraftingMenu();
            }

            if (craftingPanel.activeInHierarchy && isInit)
            {
                if(Input.GetKeyDown(KeyCode.Escape))
                    CloseCraftingMenu();
            }
        }
    }

    private void ShowCraftingMenu()
    {
        if(!isInit)
            InitCraftingList();

        IsCrafting = true;
        craftingPanel.SetActive(true);
        hudUI.SetActive(false);
        PlayerMovement.Instance.PlayerInput = false;
    }

    private void CloseCraftingMenu()
    {
        for (int i = 0; i < craftListParent.childCount; i++)
        {
            Destroy(craftListParent.GetChild(i).gameObject);
        }
        IsCrafting = false;
        hudUI.SetActive(true);
        _workbench.ShowText(false);
        craftingPanel.SetActive(false);
        infoPanel.gameObject.SetActive(false);
        PlayerMovement.Instance.PlayerInput = true;
        isInit = false;
    }

    private void InitCraftingList()
    {
        craftingData = new Dictionary<CraftData, CraftingListItem>();
        foreach (CraftData craftData in craftables)
        {
            CraftingListItem craftingListItem = Instantiate(craftListItemPrefab, craftListParent).GetComponent<CraftingListItem>();
            craftingListItem.SetRecipe(craftData);
            craftingListItem.InfoPanel = infoPanel;
            craftingListItem.CraftingSystem = this;
            craftingData.Add(craftData, craftingListItem);
        }
        isInit = true;
    }

    public void CheckForResources(CraftData craftData)
    {
        foreach (Resource resource in craftData.ResourceForCraft)
        {
            if (!inventory.SearchItemBy(resource.resourceType, resource.amount))
            {
                infoPanel.Activate(false);
                break;
            }

            infoPanel.Activate(true);
        }
    }

    public void Craft(CraftingListItem craftingListItem)
    {
        CraftData currentCraftData = null;

        foreach (CraftData craftData in craftingData.Keys)
        {
            if (craftingData[craftData] == craftingListItem)
            {
                currentCraftData = craftData;
                break;
            }
        }

        foreach (Resource resource in currentCraftData?.ResourceForCraft)
        {
            inventory.UseItemBy(resource.resourceType, resource.amount);
        }

        CheckForResources(currentCraftData);
        Item craftedItem = PhotonNetwork.Instantiate(currentCraftData.ItemPrefab.name, hand.position, Quaternion.identity).GetComponent<Item>();
        craftedItem.gameObject.SetActive(false);
        craftedItem.transform.parent = hand;
        craftedItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        inventory.AddItem(craftedItem);
    }
}
