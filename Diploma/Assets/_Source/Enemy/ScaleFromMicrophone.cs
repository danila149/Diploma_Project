using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ScaleFromMicrophone : NetworkBehaviour
{

    public Vector3 minScale;
    public Vector3 maxScale;
    public AudioLounge detection;


    public float loundsSensebility = 100;
    public float threshold = 0.1f;
    public float max = 5;

    public Transform player;

    private bool chek;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float loudness = detection.GetLoudnessFromMicrophone() * loundsSensebility;
        if (loudness >= max)
        {
            chek = true;
        }
        if (chek)
        {
            transform.position = Vector3.Lerp(transform.position, player.position, 5 * Time.deltaTime);
        }

        if (loudness < threshold)
            loudness = 0;

        //transform.localScale = Vector3.Lerp(minScale, maxScale, loudness);
    }
}
