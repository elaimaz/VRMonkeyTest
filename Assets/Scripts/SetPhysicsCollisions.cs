using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPhysicsCollisions : MonoBehaviour {

    public int playerLayer;
    public int enemyLayer;

    public int playerBarrierLayer;
    public int enemyBarrierLayer;
    public int numLayers;
	// Use this for initialization
	void Awake () {

        for(int i = 0; i < numLayers; i++)
        {
            Physics.IgnoreLayerCollision(playerBarrierLayer, i);
            Physics.IgnoreLayerCollision(enemyBarrierLayer, i);
        }
        Physics.IgnoreLayerCollision(playerBarrierLayer, enemyLayer, false);
        Physics.IgnoreLayerCollision(enemyBarrierLayer, playerLayer, false);
	}

}
