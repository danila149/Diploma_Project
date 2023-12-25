using UnityEngine;

public class ItemDestroyer : MonoBehaviour
{
    private Vector3 RESET_POSITION = new Vector3(0, 1000, 0);

    [SerializeField] private LayerMask itemLayerMask;
    [SerializeField] private LayerMask playerLayerMask;

    private int _itemLayer;
    private int _playerLayer;
    private GameObject currentItem;
    private bool _take;
    private Vector3 _playerPos;

    private void Awake()
    {
        _itemLayer = (int)Mathf.Log(itemLayerMask.value,2);
        _playerLayer = (int)Mathf.Log(playerLayerMask.value,2);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == _itemLayer)
        {
            currentItem = other.gameObject;
            other.gameObject.SetActive(false);
            if (_take)
                transform.position = _playerPos;
            else
                transform.position = RESET_POSITION;
        }

        if (other.gameObject.layer == _playerLayer)
        {
            Transform hand = other.transform.GetChild(0).GetChild(2);
            currentItem.transform.parent = hand;
            currentItem.transform.position = hand.position;
            currentItem.transform.rotation = hand.rotation;
            currentItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            transform.position = RESET_POSITION;
            _take = false;
        }
    }

    public void TakeItem(Vector3 playerPos)
    {
        _take = true;
        _playerPos = playerPos;
    }
}