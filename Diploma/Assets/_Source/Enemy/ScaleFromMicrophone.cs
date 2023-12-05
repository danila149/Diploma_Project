using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ScaleFromMicrophone : NetworkBehaviour
{

    public AudioLounge detection;


    public float loundsSensebility = 100;
    public float threshold = 0.1f;
    public float max = 5;

    public GameObject Player;

    private bool chek;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsServer)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            float loudness = detection.GetLoudnessFromMicrophone() * loundsSensebility;
            if (loudness >= max)
            {
                chek = true;
            }
            if (chek)
            {
                transform.position = Vector3.Lerp(transform.position, Player.transform.position, 5 * Time.deltaTime);
            }

            if (loudness < threshold)
                loudness = 0;
        }


    }
}
