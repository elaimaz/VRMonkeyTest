using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    static AudioManager _instance;



    public static AudioManager getInstance()
    {
        if (_instance == null)
        {
            _instance = GameObject.FindObjectOfType<AudioManager>();
        }

        return _instance;
    }

    public AudioClip enemyAlert;
    public AudioClip playerHit;
}
