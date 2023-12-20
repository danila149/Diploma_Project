using UnityEngine;

public class BildSystem : MonoBehaviour
{
    public GameObject buildableObject; // Префаб объекта для строительства

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Если нажата левая кнопка мыши
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 10))
            {
                Vector3 spawnPosition = hit.point + hit.normal; // Определите позицию, где нужно разместить объект

                Instantiate(buildableObject, spawnPosition, Quaternion.identity); // Создайте объект для строительства
            }

        }
    }
}
