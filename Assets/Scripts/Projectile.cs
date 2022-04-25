using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform sphere;
    [SerializeField] private Rigidbody rb;
    private SoundManager soundManager;
    public bool arrived;
    [SerializeField] private ParticleSystem ps, upgradePs;
    [SerializeField] private MeshRenderer mr;
    [SerializeField] private new Collider collider;
    [SerializeField] private GameObject projectile;
    private ProjectileSpawner projectileSpawner;

    [SerializeField] private bool isUpgradeProjectile;

    // Start is called before the first frame update
    void Start()
    {
        sphere = GameObject.Find("Sphere").transform;
        projectileSpawner = GameObject.Find("Projectile Spawner").GetComponent<ProjectileSpawner>();
        soundManager = GetComponent<SoundManager>();

        mr.enabled = true;
        collider.enabled = true;
        arrived = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //constantly move towards sphere
        if (!arrived)
        {
            rb.velocity = (sphere.position - projectile.transform.position).normalized * 5;
            projectile.transform.rotation = Quaternion.LookRotation((sphere.position - projectile.transform.position));
        }
    }

    public IEnumerator Remove()
    {
        //destroy projectile

        ps.Play();
        mr.enabled = false;
        collider.enabled = false;
        soundManager.SoundEffects("cylinderBreak");

        if (isUpgradeProjectile)
            upgradePs.Stop();

        yield return new WaitForSeconds(2);

        projectile.SetActive(false);

        //only requeue if this is a normal projectile, and move out of the way
        if (!isUpgradeProjectile)
        {
            projectileSpawner.queuedProjectiles.Enqueue(gameObject);
            transform.position += transform.up * 500;
        }
        else
            Destroy(gameObject);
    }

    public void Begin()
    {
        //called once projectile is dequeued

        mr.enabled = true;
        collider.enabled = true;
        arrived = false;
        rb.constraints = RigidbodyConstraints.None;
    }
}
