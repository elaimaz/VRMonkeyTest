using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockDamageArea : MonoBehaviour {

    public bool friend = false;
    public float stunTime = 1.5f;

    public float lifetime = 1.0f;
    float currentTime = 0;

    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > lifetime)
        {
            DestroyBullet(false);
        }
    }


    void DestroyBullet(bool effect=true)
    {
        Destroy(gameObject);
    }

	void OnTriggerEnter(Collider col)
    {
        AIAgent colAIagent = col.GetComponent<AIAgent>();
        if(colAIagent != null)
        {
            colAIagent.OnShock(0);
        }
    }
}
