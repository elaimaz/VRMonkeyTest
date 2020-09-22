using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    public Transform respawnPosition;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == StealthPlayerController.getInstance().gameObject)
        {
            StealthPlayerController.getInstance().ResetEnergy();
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject== StealthPlayerController.getInstance().gameObject)
        {
            GameLogic.instance.SetCheckpoint(respawnPosition.position);
        }
    }
}
