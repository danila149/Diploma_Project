using UnityEngine;

public class ItemDestroyer : MonoBehaviour
{
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
                gameObject.SetActive(false);
        }

        if (other.gameObject.layer == _playerLayer)
        {
            Transform hand = other.transform.GetChild(0).GetChild(2);
            currentItem.transform.parent = hand;
            currentItem.transform.position = hand.position;
            currentItem.transform.rotation = hand.rotation;
            gameObject.SetActive(false);
            _take = false;
        }
    }

    public void TakeItem(Vector3 playerPos)
    {
        _take = true;
        _playerPos = playerPos;
    }
}
