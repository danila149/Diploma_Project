using UnityEngine;
using Photon.Pun;

public class BildSystem : MonoBehaviour
{
    [SerializeField] private GameObject buildableObject;
    [SerializeField] private Inventory inventory;
    [SerializeField] private CraftingSystem craftingSystem;

    public bool IsLocal { get; set; }

    private void Update()
    {
        if (IsLocal)
        {
            if (inventory.SearchItemBy(ResourceType.Log, 1))
            {
                if (Input.GetMouseButtonDown(0) && !inventory.IsInvetoryOpen && !craftingSystem.IsCrafting)
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, 10))
                    {
                        Vector3 spawnPosition = hit.point + hit.normal;

                        PhotonNetwork.Instantiate("Cube", spawnPosition, Quaternion.identity);
                        inventory.UseItemBy(ResourceType.Log, 1);
                    }

                }
            }
        }
    }
}
