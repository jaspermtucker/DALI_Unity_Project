using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    private AudioClip cylinderLand, cylinderBreak, shoot, die, clickStart;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        cylinderLand = Resources.Load<AudioClip>(@"Sounds\cylinderLand");
        cylinderBreak = Resources.Load<AudioClip>(@"Sounds\cylinderBreak");
        shoot = Resources.Load<AudioClip>(@"Sounds\shoot");
        die = Resources.Load<AudioClip>(@"Sounds\die");
        clickStart = Resources.Load<AudioClip>(@"Sounds\clickStart");

        audioSource = GetComponent<AudioSource>();
    }


    public void SoundEffects(string clip)
    {
        switch (clip)
        {
            case "cylinderLand":
                audioSource.PlayOneShot(cylinderLand);
                break;
            case "cylinderBreak":
                audioSource.PlayOneShot(cylinderBreak);
                break;
            case "shoot":
                audioSource.volume = 0.05f;
                audioSource.PlayOneShot(shoot);
                break;
            case "die":
                audioSource.volume = 1;
                audioSource.PlayOneShot(die);
                break;
            case "clickStart":
                audioSource.PlayOneShot(clickStart);
                break;
        }


    }
}
