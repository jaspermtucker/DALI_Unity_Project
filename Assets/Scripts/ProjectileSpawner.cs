using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] private Transform sphere;
    private Vector3 location,randomDirection;
    [SerializeField] private GameObject projectile, upgrade;

    private bool tryAgain, stop, spawnProjectile, spawnUpgrade;
    private int numOfTries = 1;
    private List<float> randomNumbers = new List<float>();

    private int score;
    [SerializeField] private Text scoreText;
    public static int bestScore;

    public Queue<GameObject> queuedProjectiles = new Queue<GameObject>();


    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(SpawnProjectiles());
        bestScore = PlayerPrefs.GetInt("BestScore");
    }


    void PickSpot()
    {
        //function to spawn a projectile at a randomly chosen location

        tryAgain = false;

        //chose a random direction out from the sphere to spawn projectile (rounded to 5)
        for (int i = 0; i < 3; i++)
        {
            int randomInt = Random.Range(-180, 180);
            float roundedFloat = Mathf.Round(randomInt / 5) * 5f;

            randomNumbers.Add(roundedFloat);
        }

        randomDirection = new Vector3(randomNumbers[0], randomNumbers[1], randomNumbers[2]).normalized;
        location = sphere.transform.position + (randomDirection * 60);

        randomNumbers.Clear();

        //cast from the spawning point to sphere to check if anything is in the way
        RaycastHit[] hits = Physics.SphereCastAll(location, 1.5f, (sphere.position - location), 52.5f);

        //if path is blocked by anything, restart function with new location
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.tag == "BlockSphereCast")
            {
                tryAgain = true;
                numOfTries += 1;
                if (numOfTries >= 3000)
                {
                    stop = true;
                    return;
                }
                PickSpot();
                return;
            }
        }

        //instantiate projectile and collider to prevent any future projectiles from spawning in same spot
        if (!tryAgain)
        {
            score += 1;

            if (score > bestScore)
                bestScore = score;

            scoreText.text = score.ToString();

            Ray centerOfCast = new Ray(location, (sphere.position - location));
            Physics.Raycast(centerOfCast, out RaycastHit hit, Mathf.Infinity);

            ChooseWhatToSpawn();

            if (spawnProjectile)
            {
                if (queuedProjectiles.Count > 0)
                {
                    GameObject currentProjectile = queuedProjectiles.Dequeue();
                    currentProjectile.transform.position = hit.point;
                    currentProjectile.GetComponent<Projectile>().Begin();

                    GameObject cylinder = currentProjectile.transform.GetChild(0).transform.gameObject;
                    cylinder.transform.position = location;
                    cylinder.SetActive(true);
                }
                else
                {
                    Instantiate(projectile, hit.point, Quaternion.identity);

                    GameObject cylinder = projectile.transform.GetChild(0).transform.gameObject;
                    cylinder.transform.position = location;
                    cylinder.SetActive(true);
                }
            }
            else if (spawnUpgrade)
            {

                Instantiate(upgrade, hit.point, Quaternion.identity);

                GameObject cylinder = upgrade.transform.GetChild(0).transform.gameObject;
                cylinder.transform.position = location;
                cylinder.SetActive(true);

            }

            numOfTries = 1;
        }
              
    }

    public void ChooseWhatToSpawn()
    {
        int randomNum = Random.Range(0, 31);
        if (randomNum < 29)
        {
            spawnProjectile = true;
            spawnUpgrade = false;
        }
        else
        {
            spawnUpgrade = true;
            spawnProjectile = false;
        }

    }


    public IEnumerator SpawnProjectiles()
    {
        //continuously spawn projectiles on a timer
        
        PickSpot();
        yield return new WaitForSeconds(1);

        if (Player.dead || stop)
        {
            score = 0;
            yield break;
        }

        StartCoroutine(SpawnProjectiles());
    }


    private void OnDrawGizmos()
    {
        // function for visuals on where projectiles are being spawned, viewable in editor

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(location, 1.5f);
        Gizmos.DrawRay(location, (sphere.position - location).normalized * 52.5f);
    }

}
