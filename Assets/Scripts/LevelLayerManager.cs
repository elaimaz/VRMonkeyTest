using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLayerManager : MonoBehaviour {

    public int scenarioLayer=9;
    public string scenarioTag="Wall";

	// Use this for initialization
	void Awake () {
		foreach(Transform child in transform)
        {
            child.tag = scenarioTag;
            child.gameObject.layer = scenarioLayer;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
