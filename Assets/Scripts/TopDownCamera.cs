using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour {

    Transform playerTransform;
    Vector3 lastPlayerPosition;
    Vector3 playerDelta;
	// Use this for initialization
	void Start () {
        playerTransform = StealthPlayerController.getInstance().transform;
        lastPlayerPosition = playerTransform.position;
	}
	
	// Update is called once per frame
	void Update () {
        playerDelta = playerTransform.position - lastPlayerPosition;
        transform.position += playerDelta;
        lastPlayerPosition = playerTransform.position;
	}
}
