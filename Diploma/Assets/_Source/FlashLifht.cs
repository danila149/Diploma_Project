using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLifht : MonoBehaviour
{


    [SerializeField] private float TimeLight = 80;

    bool FlashActive;
    public Light FlashLight;
    void Start()
    {
        
    }
    void ACtiveFlash()
    {
        FlashActive = !FlashActive;
        if(FlashActive == true)
        {
            FlashLight.enabled = true;
        }
        else
        {
            FlashLight.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ACtiveFlash();
        }

        if(FlashActive == true)
        {
            FlashLight.intensity = TimeLight/10;
            TimeLight -= Time.deltaTime;
        }
    }
}
