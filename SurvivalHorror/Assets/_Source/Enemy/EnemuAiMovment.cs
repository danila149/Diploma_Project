using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemuAiMovment : MonoBehaviour
{
    [SerializeField] private NavMeshAgent Enemy;
    [SerializeField] private GameObject Player;


    [SerializeField] private AudioLounge detection;

    public float loundsSensebility = 100;
    public float threshold = 0.1f;
    public float max = 5;
    public float maxDistance = 10f;

    private float Dist_player;

    public bool chek = false;

    private float loudness;

    private void Start()
    {
        Enemy = gameObject.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
            if (Player == null)
            {
                Player = GameObject.FindGameObjectWithTag("Player");
            }
            loudness = detection.GetLoudnessFromMicrophone() * loundsSensebility;
            Dist_player = Vector3.Distance(Player.transform.position, gameObject.transform.position);
            if (loudness >= max)
            {
                chek = true;
            }
            if (Dist_player < maxDistance && chek == true)
            {
                Enemy.SetDestination(Player.transform.position);
            }
            if (Dist_player > maxDistance)
            {
                chek = false;
                Enemy.ResetPath();
            }
            if (loudness < threshold)
                loudness = 0;
    }


}