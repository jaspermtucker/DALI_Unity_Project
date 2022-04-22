using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private ParticleSystem startGunPs, hasUpgradePs;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawnPoint;
    private SoundManager soundManager;

    private void Start()
    {
        soundManager = GetComponent<SoundManager>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Upgrade"))
        {
            StartCoroutine(collision.transform.GetComponentInParent<Projectile>().Remove());

            //in case player is already shooting, stop last coroutine and start again
            StopCoroutine("Shoot");
            StartCoroutine("Shoot");
        }
    }

    private IEnumerator Shoot()
    {
        startGunPs.Play();
        hasUpgradePs.Play();

        for (int i = 0; i < 10; i++)
        {
            if (!Player.dead)
            {
                soundManager.SoundEffects("shoot");
                Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            }
            yield return new WaitForSeconds(0.5f);
        }

        hasUpgradePs.Stop();
    }
}
