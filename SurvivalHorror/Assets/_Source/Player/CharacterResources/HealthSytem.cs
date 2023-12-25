using Photon.Pun;
using System.Collections;
using UnityEngine;

public class HealthSytem : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float healthPerRegen;
    [SerializeField] private float regenerationDelay;
    [SerializeField] private HealthView healthView;
    [SerializeField] private Inventory inventory;

    private RoomManager roomManager;
    private float _health;
    private bool onRegenerationCooldown;
    public bool IsHungry { get; set; }

    private void Start()
    {
        roomManager = RoomManager.Instance;
        healthView.HealthBar.maxValue = maxHealth;
        healthView.HealthBar.value = maxHealth;
        _health = healthView.HealthBar.value;
    }

    private void Update()
    {
        if (!onRegenerationCooldown && _health <= maxHealth && !IsHungry)
            StartCoroutine(Regeneration());
    }

    private IEnumerator Regeneration()
    {
        ChangeValueOn(healthPerRegen);
        onRegenerationCooldown = true;
        yield return new WaitForSeconds(regenerationDelay);
        onRegenerationCooldown = false;
    }

    public void ChangeValueOn(float value)
    {
        _health += value;
        if (_health >= maxHealth)
            _health = maxHealth;
        

        if (_health <= 0)
        {
            inventory.DropAll();
            PhotonNetwork.Destroy(gameObject);
            roomManager.RespawnPlayer();
        }
        else
        {
            healthView.HealthBar.value = _health;
        }
    }
}
