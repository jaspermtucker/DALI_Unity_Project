using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private MeshRenderer mr;
    private new Collider collider;

    [SerializeField] private Transform cam;
    [SerializeField] private GameObject cube;

    [SerializeField] private float movementSpeed = 0.1f;
    private Vector3 direction;
    private Vector3 normal;

    private float gravityValue = -80; 
    private Vector3 gravityUp;

    public static GameObject sphere;
    [SerializeField] private ParticleSystem killedPs;
    public static bool dead;
    

    [SerializeField] private Text bestScoreText;
    [SerializeField] private GameObject retryButton;

    private SoundManager soundManager;

    public static bool canPlayAgain;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        mr = GetComponentInChildren<MeshRenderer>();
        soundManager = GetComponent<SoundManager>();

        direction = transform.forward;
        sphere = GameObject.Find("Sphere");

    }


    // Update is called once per frame
    void Update()
    {
        CalculateMovementAngles();
    }

    void CalculateMovementAngles()
    {
        //get necessary input for movement and rotation
        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");

        direction = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;

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
        

        //stop movement when there's no input
        if (direction.magnitude < 0.1f)
            rb.velocity = Vector3.zero;

        //general rotation for on sphere
        Quaternion targetR = Quaternion.FromToRotation(transform.up, gravityUp) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetR, 6 * Time.fixedDeltaTime);


        //apply movement
        if (direction.magnitude > 0.1f && !dead)
        {

            rb.MovePosition(rb.position + transform.TransformDirection(direction) * movementSpeed);

            //rotation to face forward correct way
            cube.transform.rotation = Quaternion.Slerp(cube.transform.rotation,
            Quaternion.LookRotation(transform.TransformDirection(direction), normal), Time.fixedDeltaTime * 10);

        }
        else
        {
             cube.transform.rotation = Quaternion.Slerp(cube.transform.rotation,
             Quaternion.LookRotation(cube.transform.forward, normal), Time.fixedDeltaTime * 10);

        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        //"Die" when hitting a red object
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Projectile") && !dead)
        {
            dead = true;
            killedPs.Play();
            mr.enabled = false;
            collider.enabled = false;
            soundManager.SoundEffects("die");

            retryButton.SetActive(true);
            bestScoreText.text = "Best Score: " + ProjectileSpawner.bestScore.ToString();
            bestScoreText.gameObject.SetActive(true);

            PlayerPrefs.SetInt("BestScore", ProjectileSpawner.bestScore);

            //hitting "retry" immediately may result in bug, force player to wait 1 second
            StartCoroutine(PlayAgain());
        }
    }

    IEnumerator PlayAgain()
    {
        yield return new WaitForSeconds(1);
        canPlayAgain = true;
    }

}
