using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabBuild : MonoBehaviour
{
    Collider[] colliders;
    MeshFilter[] meshes;
    public List<Material> materials = new List<Material>();
    public Transform ray;
    public bool can;
    public Material canM,cantM;

    public bool IsHologram { get; set; }

    private void Start()
    {
        colliders = GetComponentsInChildren<Collider>();
        meshes = GetComponentsInChildren<MeshFilter>();
        for (int i = 0; i < meshes.Length; i++)
        {
            materials.Add(meshes[i].GetComponent<Renderer>().material);
        }
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
    }

    private void Update()
    {
        if(IsHologram)
        {
            RaycastHit hit;

            if (Physics.Raycast(ray.transform.position, ray.transform.forward, out hit, 1.5f))
            {
                if (hit.transform.tag != "Ground")
                    can = false;
                else
                    can = true;
            }
            else
            {
                can = true;
            }

            for (int i = 0; i < meshes.Length; i++)
            {
                meshes[i].GetComponent<Renderer>().material = can ? canM : cantM;
            }
        }
    }


    public bool Place(Vector3 pos, Vector3 rot, string prefabName)
    {
        if(can)
            PhotonNetwork.Instantiate(prefabName, pos, Quaternion.Euler(rot));

        if (can)
        {
            for (int i = 0; i < meshes.Length; i++)
            {
                meshes[i].GetComponent<Renderer>().material = materials[i];
            }
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = true;
            }
        }
        if (can){
            Destroy(this);
        }
	    return can;
    }
}
