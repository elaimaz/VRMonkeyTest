using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour {

    public GameObject boxEffect;
    public GameObject itemEffect;
    public GameObject shootEffect;
    public GameObject damageEffect;

    static EffectsManager _instance;

    public static EffectsManager getInstance()
    {
        if (_instance == null)
        {
            _instance = GameObject.FindObjectOfType<EffectsManager>();
        }

        return _instance;
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
