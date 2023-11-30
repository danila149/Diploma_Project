using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VR
{
    public class FireBulletOnActivate : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float bulletSpeed = 20f;
        [SerializeField] private XRGrabInteractableTwoAttach grabbable;


        void Start()
        {
            grabbable.activated.AddListener(FireBullet);
        }

        private void FireBullet(ActivateEventArgs arg0)
        {
            GameObject spawnedBullet = Instantiate(bulletPrefab);
            spawnedBullet.transform.position = firePoint.position;
            spawnedBullet.GetComponent<Rigidbody>().velocity = firePoint.forward * bulletSpeed;
            Destroy(spawnedBullet, 5);
        }
    }
}