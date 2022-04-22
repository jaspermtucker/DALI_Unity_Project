using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    //disable play button, instructions text, and controls text; enable score text, enemy, and projectile spawner
    [SerializeField] GameObject[] itemsToDisable, itemsToEnable;

    //disable retry button, best score text when retry is clicked
    [SerializeField] GameObject[] itemsToDisableRetry;

    [SerializeField] private GameObject player, enemyStartLocation, enemy, playerStartLocation;
    [SerializeField] private ProjectileSpawner projectileSpawner;
    [SerializeField] private Text scoreText;
    private GameObject[] projectiles;

    private SoundManager soundManager;

    private void Start()
    {
        soundManager = GetComponent<SoundManager>();
    }

    public void Play()
    {
        soundManager.SoundEffects("clickStart");

        //begin game
        foreach (GameObject item in itemsToDisable)
            item.SetActive(false);

        foreach (GameObject item in itemsToEnable)
            item.SetActive(true);
    }

    public void Retry()
    {
        soundManager.SoundEffects("clickStart");

        //reset game, destroy all projectiles on sphere
        projectiles = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject projectile in projectiles)
        {
            projectile.SetActive(false);
            projectileSpawner.queuedProjectiles.Enqueue(projectile.transform.parent.gameObject);
        }

        //reset player
        Player.dead = false;
        player.transform.position = playerStartLocation.transform.position;
        player.GetComponentInChildren<MeshRenderer>().enabled = true;
        player.GetComponent<Collider>().enabled = true;

        //start spawning projecitles again
        StartCoroutine(projectileSpawner.SpawnProjectiles());

        //reset enemy
        enemy.transform.position = enemyStartLocation.transform.position;

        //disable unwanted items
        foreach (GameObject item in itemsToDisableRetry)
            item.SetActive(false);

        //reset score
        scoreText.text = "0";
        scoreText.enabled = true;

        

    }
}
