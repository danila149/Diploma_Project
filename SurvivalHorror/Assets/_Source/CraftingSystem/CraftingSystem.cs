using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] private LayerMask workbenchLayerMask;
    [SerializeField] private Inventory inventory;
    [SerializeField] private int distanceToUse;
    [SerializeField] private GameObject craftingPanel;
    [SerializeField] private GameObject hudUI;
    [SerializeField] private InfoPanel infoPanel;
    [SerializeField] private GameObject craftListItemPrefab;
    [SerializeField] private Transform craftListParent;
    [SerializeField] private PickUpSystem pickUpSystem;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject aim;

    private Transform _spawnpoint;
    private List<CraftData> _craftables;
    private Dictionary<CraftData, CraftingListItem> _craftingData;

    public bool IsLocal { get; set; }
    public bool IsCrafting { get; set; }

    private Workbench _workbench = null;
    private bool isInit;

    void Update()
    {
        if (IsLocal)
        {

            RaycastHit hit;
            if (Physics.Raycast(head.position, head.forward, out hit, distanceToUse, workbenchLayerMask))
            {
                _workbench = hit.transform.gameObject.GetComponent<Workbench>();
                _craftables = _workbench.Craftables;
                _spawnpoint = _workbench.Spawnpoint;
                aim.SetActive(true);

                if (!inventory.IsInvetoryOpen && Input.GetKeyDown(KeyCode.E))
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
        playerMovement.PlayerInput = false;
    }

    private void CloseCraftingMenu()
    {
        for (int i = 0; i < craftListParent.childCount; i++)
        {
            Destroy(craftListParent.GetChild(i).gameObject);
        }
        IsCrafting = false;
        hudUI.SetActive(true);
        craftingPanel.SetActive(false);
        infoPanel.gameObject.SetActive(false);
        playerMovement.PlayerInput = true;
        isInit = false;
    }

    private void InitCraftingList()
    {
        _craftingData = new Dictionary<CraftData, CraftingListItem>();
        foreach (CraftData craftData in _craftables)
        {
            CraftingListItem craftingListItem = Instantiate(craftListItemPrefab, craftListParent).GetComponent<CraftingListItem>();
            craftingListItem.SetRecipe(craftData);
            craftingListItem.InfoPanel = infoPanel;
            craftingListItem.CraftingSystem = this;
            _craftingData.Add(craftData, craftingListItem);
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

        foreach (CraftData craftData in _craftingData.Keys)
        {
            if (_craftingData[craftData] == craftingListItem)
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
        Item craftedItem = PhotonNetwork.Instantiate(currentCraftData.ItemPrefab.name, _spawnpoint.position, Quaternion.identity).GetComponent<Item>();
        pickUpSystem.PickUp(craftedItem);
    }
}
