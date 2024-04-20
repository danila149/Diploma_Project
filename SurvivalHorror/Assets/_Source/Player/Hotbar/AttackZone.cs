using Photon.Pun;
using System.Collections;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    [SerializeField] private GameObject hit;

    private int _objectLayer;
    private int _damage;
    private Hotbar _hotbar;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == _objectLayer)
        {
            StartCoroutine(ShowHit());
            other.GetComponent<ADamagable>().GetDamage(_damage);
            if(other.GetComponent<ADamagable>().dropType != ResourceType.None)
            {
                Items.Resource collectedItem = PhotonNetwork.Instantiate(other.GetComponent<ADamagable>().dropType.ToString(), transform.position, Quaternion.identity).GetComponent<Items.Resource>();
                collectedItem.Amount = _damage;
                _hotbar.GetResource(collectedItem);
            }
        }
    }

    private IEnumerator ShowHit()
    {
        hit.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        hit.SetActive(false);
    }

    public void SetLayerMask(LayerMask layerMask)=>
        _objectLayer = (int)Mathf.Log(layerMask.value, 2);

    public void SetDamage(int damage) =>
        _damage = damage;

    public void SetHotbar(Hotbar hotbar) =>
        _hotbar = hotbar;
}
