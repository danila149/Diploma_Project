using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemuAiMovment : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float attackDelay;
    [SerializeField] private NavMeshAgent Enemy;
    [SerializeField] private GameObject Player;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private AudioLounge detection;

    public float loundsSensebility = 100;
    public float threshold = 0.1f;
    public float max = 5;
    public float maxDistance = 10f;

    private float Dist_player;
    private int _playerLayer;
    public bool chek = false;
    private bool onAttackCooldown;

    private float loudness;

    private void Start()
    {
        Enemy = gameObject.GetComponent<NavMeshAgent>();
        _playerLayer = (int)Mathf.Log(playerLayerMask.value, 2);
    }

    private void Update()
    {
        if (Player == null)
            Player = GameObject.FindGameObjectWithTag("Player");
        else
        {
            loudness = detection.GetLoudnessFromMicrophone() * loundsSensebility;
            Dist_player = Vector3.Distance(Player.transform.position, gameObject.transform.position);
            //Debug.Log(loudness);
            if (loudness >= max)
            {
                chek = true;
            }
            if (Dist_player < maxDistance && chek == true)
            {
                GetComponent<PhotonView>().RPC("FollowPlayer", RpcTarget.AllBuffered, Player.transform.position);
            }
            if (Dist_player > maxDistance)
            {
                chek = false;
                GetComponent<PhotonView>().RPC("StopWalking", RpcTarget.AllBuffered);
            }
            if (loudness < threshold)
                loudness = 0;
        }   
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == _playerLayer && !onAttackCooldown)
            StartCoroutine(Attack(collision.gameObject.GetComponent<HealthSytem>()));
    }

    [PunRPC]
    public void FollowPlayer(Vector3 playerPos)=>
        Enemy.SetDestination(playerPos);

    [PunRPC]
    public void StopWalking()=>
        Enemy.ResetPath();

    private IEnumerator Attack(HealthSytem playerHealth)
    {
        onAttackCooldown = true;
        playerHealth.ChangeValueOn(-damage);
        yield return new WaitForSeconds(attackDelay);
        onAttackCooldown = false;
    }
}