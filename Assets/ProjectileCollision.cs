using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
    private Rigidbody rb;
    private Projectile projectile;
    private SoundManager soundManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        projectile = GetComponentInParent<Projectile>();
        soundManager = GetComponentInParent<SoundManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //once projectile hits the sphere, stop moving
        if (collision.gameObject.CompareTag("Sphere"))
        {
            projectile.arrived = true;
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            soundManager.SoundEffects("cylinderLand");
        }
    }
}
