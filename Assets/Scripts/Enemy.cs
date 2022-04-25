using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject cube;

    [SerializeField] private float movementSpeed = 0.1f;
    private Vector3 direction;
    private Vector3 normal;

    private float gravityValue = -80;
    private Vector3 gravityUp;

    public static GameObject sphere;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        direction = transform.forward;
        sphere = GameObject.Find("Sphere");
    }


    // Update is called once per frame
    void Update()
    {
        direction = (player.position - transform.position).normalized;
        normal = -(sphere.transform.position - transform.position).normalized;
    }



    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        //constant gravity
        gravityUp = (transform.position - sphere.transform.position).normalized;
        rb.AddForce(gravityUp * gravityValue);

        //rotation is set to align with sphere and direction
        Vector3 forward = direction - normal * Vector3.Dot(direction, normal);
        Quaternion finalRotation = Quaternion.LookRotation(forward.normalized, normal);
        if (!Player.dead)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, Time.deltaTime * 4);

            //move in forward direction, since roation is aligned to face desired direction of movement
            rb.velocity = transform.forward * movementSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //remove projectiles on colliding with them
        if (collision.gameObject.CompareTag("Projectile") || collision.gameObject.CompareTag("Upgrade"))
        {
            StartCoroutine(collision.transform.gameObject.GetComponentInParent<Projectile>().Remove());
        }
    }





}
