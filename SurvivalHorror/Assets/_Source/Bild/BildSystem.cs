using UnityEngine;
using Photon.Pun;

public class BildSystem : MonoBehaviour
{
    [SerializeField] private GameObject buildableObject; // Префаб объекта для строительства
    [SerializeField] private Inventory inventory;

    private void Update()
    {
        if (inventory.SearchItemBy(ResourceType.Log, 1))
        {
            if (Input.GetMouseButtonDown(0) && !Inventory.Instance.IsCrafting && !CraftingSystem.Instance.IsInventoryOpen) 
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 10))
                {
                    Vector3 spawnPosition = hit.point + hit.normal; // Определите позицию, где нужно разместить объект

                    PhotonNetwork.Instantiate("Cube", spawnPosition, Quaternion.identity); // Создайте объект для строительства
                    inventory.UseItemBy(ResourceType.Log, 1);
                }

            }
        }
    }
}
