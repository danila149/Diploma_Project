using Photon.Pun;
using System;
using System.Collections;
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
    [SerializeField] private InfoPanel infoPanel;
    [SerializeField] private GameObject craftListItemPrefab;
    [SerializeField] private Transform craftListParent;
    [SerializeField] private List<CraftData> craftables;

    private Dictionary<CraftData, CraftingListItem> craftingData;

    public bool IsLocal { get; set; }
    public bool IsInventoryOpen { get; set; }

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
            if(!isInit && craftingPanel.activeInHierarchy)
                InitCraftingList();

            RaycastHit hit;
            if (Physics.Raycast(head.position, head.forward, out hit, distanceToUse, workbenchLayerMask))
            {
                _workbench = hit.transform.gameObject.GetComponent<Workbench>();
                if(!IsInventoryOpen)
                    ShowCraftingMenu();
            }

            if (craftingPanel.activeInHierarchy && isInit)
            {
                CheckForResources();
                if(Input.GetKeyDown(KeyCode.Escape))
                    CloseCraftingMenu();
            }
        }
    }

    private void ShowCraftingMenu()
    {
        _workbench.ShowText(true);
        if (Input.GetKeyDown(KeyCode.E))
        {
            inventory.IsCrafting = true;
            craftingPanel.SetActive(true);
            PlayerMovement.Instance.PlayerInput = false;
        }
    }

    private void CloseCraftingMenu()
    {
        inventory.IsCrafting = false;
        _workbench.ShowText(false);
        craftingPanel.SetActive(false);
        infoPanel.gameObject.SetActive(false);
        PlayerMovement.Instance.PlayerInput = true;
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

    private void CheckForResources()
    {
        foreach (CraftData craftData in craftingData.Keys)
        {
            foreach (Resource resource in craftData.ResourceForCraft)
            {
                if(!inventory.SearchItemBy(resource.resourceType, resource.amount))
                {
                    craftingData[craftData].Activate(false);
                    break;
                }

                craftingData[craftData].Activate(true);
            }
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

        Item craftedItem = PhotonNetwork.Instantiate(currentCraftData.ItemPrefab.name, hand.position, Quaternion.identity).GetComponent<Item>();
        craftedItem.gameObject.SetActive(false);
        craftedItem.transform.parent = hand;
        inventory.AddItem(craftedItem);
    }
}
