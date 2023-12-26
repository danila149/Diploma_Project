using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using System.Collections;
using UnityEngine;

public class HealthSytem : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float healthPerRegen;
    [SerializeField] private float regenerationDelay;
    [SerializeField] private HealthView healthView;
    [SerializeField] private Inventory inventory;
    [SerializeField] private HungerSystem hungerSystem;

    private Transform spawnpoint;
    private float _health;
    private bool onRegenerationCooldown;
    public bool IsHungry { get; set; }
    public bool IsLocal { get; set; }

    private void Start()
    {
        spawnpoint = GameObject.FindGameObjectWithTag("Spawnpoint").transform;
        healthView.HealthBar.maxValue = maxHealth;
        healthView.HealthBar.value = maxHealth;
        _health = healthView.HealthBar.value;
    }

    private void Update()
    {
        if (!onRegenerationCooldown && _health <= maxHealth && !IsHungry)
            StartCoroutine(Regeneration());
    }

    public void ReseyHealth()
    {
        _health = maxHealth;
        healthView.HealthBar.value = _health;
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
        if (IsLocal)
        {
            _health += value;
            if (_health >= maxHealth)
                _health = maxHealth;


            if (_health <= 0)
            {
                inventory.DropAll();
                ReseyHealth();
                hungerSystem.ResetHunger();
                GetComponent<PhotonView>().RPC("RespawnPlayer", RpcTarget.All, spawnpoint.position);
            }
            else
            {
                healthView.HealthBar.value = _health;
            }
        }
    }

    [PunRPC]
    private void RespawnPlayer(Vector3 spawnpoint) =>
        transform.position = spawnpoint;
}
