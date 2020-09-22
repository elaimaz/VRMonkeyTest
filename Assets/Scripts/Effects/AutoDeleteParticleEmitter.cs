using UnityEngine;
using System.Collections;

public class AutoDeleteParticleEmitter : MonoBehaviour {

    ParticleSystem particles;
	// Use this for initialization
	void Start () {
        particles = GetComponent<ParticleSystem>();
        StartCoroutine(waitForDeath());
	}
	
    IEnumerator waitForDeath()
    {
        while (particles.IsAlive())
        {
            yield return null;
        }
        Destroy(gameObject);
    }
	
}
