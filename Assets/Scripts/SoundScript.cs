using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour
{


    AudioSource soundSource;
    

    // Start is called before the first frame update
    void Awake()
    {
        //soundTrack = Resources.Load<AudioClip>("Assets/Sounds/jump.ogg");
        soundSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SoundPlay(string soundName)
    {
        print("Hop");
        soundSource.clip = Resources.Load<AudioClip>("Sounds/" + soundName);
        soundSource.Play();
        print("Op");

    }

    public void SoundStop()
    {
        soundSource.Stop();
    }
}
