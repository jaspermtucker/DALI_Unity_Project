using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float movementSpeed = 5;
    private Vector3 gravityUp;
    private float gravityValue = -10;

    public static GameObject sphere;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sphere = GameObject.Find("Sphere");
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

        Quaternion targetR = Quaternion.FromToRotation(transform.up, gravityUp) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetR, 600 * Time.fixedDeltaTime);

        rb.MovePosition(rb.position + transform.forward * movementSpeed);

    }

    private void OnCollisionEnter(Collision collision)
    {
        //remove projectiles on colliding with them
        if (collision.gameObject.CompareTag("Projectile") || collision.gameObject.CompareTag("Upgrade"))
        {
            StartCoroutine(collision.transform.GetComponentInParent<Projectile>().Remove());
            Destroy(gameObject);
        }
    }
}
