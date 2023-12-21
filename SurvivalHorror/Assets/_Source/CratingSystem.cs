using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CratingSystem : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] private Transform hand;
    [SerializeField] private LayerMask workbenchLayerMask;
    [SerializeField] private Inventory inventory;
    [SerializeField] private int distanceToUse;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject craftListItemPrefab;
    [SerializeField] private GameObject craftListParent;
    [SerializeField] private List<CraftData> craftables;

    public bool IsLocal { get; set; }

    private Workbench _workbench = null;

    void Update()
    {
        if (IsLocal)
        {
            RaycastHit hit;
            if (Physics.Raycast(head.position, head.forward, out hit, distanceToUse, workbenchLayerMask))
            {
                _workbench = hit.transform.gameObject.GetComponent<Workbench>();
                //PickUp(_item);
            }
            else
            {
                if (_workbench != null)
                    _workbench.ShowText(false);
            }
        }
    }
}
