using UnityEngine;

public class Builds : MonoBehaviour
{
    public GameObject[] objects;
    public int selected;

    private void Update()
    {
        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKey(i.ToString()))
            {
                SetPrefab(i);
            }
        }
    }

    public void SetPrefab(int id)
    {
	if (FindObjectOfType<BuildManager>().created != null)
		Destroy(FindObjectOfType<BuildManager>().created);
        FindObjectOfType<BuildManager>().prefab = objects[id];
    }
}
