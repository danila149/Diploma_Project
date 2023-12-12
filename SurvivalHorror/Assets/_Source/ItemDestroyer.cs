using UnityEngine;

public class ItemDestroyer : MonoBehaviour
{
    [SerializeField] private LayerMask itemLayerMask;
    private int _itemLayer;

    private void Awake()
    {
        _itemLayer = (int)Mathf.Log(itemLayerMask.value,2);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == _itemLayer)
            other.gameObject.SetActive(false);
    }
}
