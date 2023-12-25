using UnityEngine;
using Photon.Pun;

public class BildSystem : MonoBehaviour
{
    [SerializeField] private GameObject buildableObject;
    [SerializeField] private Inventory inventory;

    private void Update()
    {
        if (inventory.SearchItemBy(ResourceType.Log, 1))
        {
            if (Input.GetMouseButtonDown(0) && !Inventory.Instance.IsInvetoryOpen && !CraftingSystem.Instance.IsCrafting) 
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
