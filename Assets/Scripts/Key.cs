using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

    public void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            GameLogic.instance.addKey();
            Destroy(gameObject);
        }
    }
}
