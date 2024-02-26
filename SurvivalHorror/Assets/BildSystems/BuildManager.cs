using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public GameObject prefab;
	[HideInInspector]
    public GameObject created;
    public Vector3 rot;
    [SerializeField] private Inventory inventory;
    Quaternion trot;

    void Update()
    {   
        if (created != null){
            trot = Quaternion.Lerp(created.transform.rotation, Quaternion.Euler(rot), 0.2f);
            if (Input.GetKeyDown(KeyCode.Mouse1)){
                Destroy(created.gameObject);
                prefab = null;
            }
        }
        RaycastHit hit;
        if (Physics.Raycast(transform.position + (transform.forward * 2f), Vector3.down, out hit))
        {
		    var pos = new Vector3(Mathf.Round(hit.point.x / 3f) * 3f,
                                 hit.point.y + 1.5f,
                                 Mathf.Round(hit.point.z / 3f) * 3f);

            if (prefab != null)
            {
                if (inventory.SearchItemBy(ResourceType.Log, 1))
                {
                    if (created == null)
                    {
                        created = Instantiate(prefab, pos, Quaternion.Euler(rot));
                        created.GetComponent<PrefabBuild>().IsHologram = true;
                    }
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.R))
                        {
                            rot += new Vector3(0, 90f, 0);
                        }
                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            if (created.GetComponent<PrefabBuild>().Place(pos, rot, prefab.name))
                            {
                                created = null;
                                inventory.UseItemBy(ResourceType.Log, 1);
                                return;
                            }

                        }
                        created.transform.rotation = trot;
                        created.transform.position = Vector3.Lerp(created.transform.position, pos, 10f * Time.deltaTime);

                        
                    }
                }
            }
        }
    }
}
