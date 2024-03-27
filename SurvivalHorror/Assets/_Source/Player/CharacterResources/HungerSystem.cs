using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerSystem : MonoBehaviour
{
    [SerializeField] private float maxHunger;
    [SerializeField] private float starveDelay;
    [SerializeField] private float hungerPerStarve;
    [SerializeField] private HungerView hungerView;
    [SerializeField] private HealthSytem healthSytem;
    [SerializeField] private PlayerMovement playerMovement;

    private float _hunger;
    private bool onStarveCooldown;

    public bool IsFullHunger;

    private void Start()
    {
        hungerView.FoodBar.maxValue = maxHunger;
        hungerView.FoodBar.value = maxHunger;
        _hunger = hungerView.FoodBar.value;
    }

    void Update()
    {
        if (!onStarveCooldown && _hunger > 0)
            StartCoroutine(Starve());
    }

    public void ChangeValue(float value)
    {
        _hunger += value;
        if (_hunger >= maxHunger)
        {
            _hunger = maxHunger;
            IsFullHunger = true;
        }
        else
            IsFullHunger = false;

        if (_hunger >= 80)
            healthSytem.IsHungry = false;
        else
            healthSytem.IsHungry = true;

        if (_hunger <= 0)
        {
            playerMovement.IsHungry = true;
            hungerView.FoodBar.value = 0;
        }
        else
        {
            playerMovement.IsHungry = false;
            hungerView.FoodBar.value = _hunger;
        }
    }

    public void ResetHunger()
    {
        _hunger = maxHunger;
        hungerView.FoodBar.value = _hunger;
    }

    private IEnumerator Starve()
    {
        ChangeValue(-hungerPerStarve);
        onStarveCooldown = true;
        yield return new WaitForSeconds(starveDelay);
        onStarveCooldown = false;
    }
}
