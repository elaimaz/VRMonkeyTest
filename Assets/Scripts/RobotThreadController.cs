using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotThreadController : MonoBehaviour {

    Renderer renderer;
    public int materialNumber;
    public float speed = 5;
    public bool moving = false;

    Vector2 textureOffset = Vector2.zero;

    public AudioSource audioSource;

    Material[] rendererMaterials;
	// Use this for initialization
	void Start () {
        renderer = GetComponent<Renderer>();
        rendererMaterials = renderer.materials;
        textureOffset= rendererMaterials[materialNumber].GetTextureOffset("_MainTex");

        audioSource = GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {
        if (GameLogic.instance.gameState != GameLogic.GameStates.gameplay)
        {
            return;
        }
        if (moving)
        {
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
            Vector2 newOffset = textureOffset + new Vector2(speed * Time.deltaTime, 0);
            rendererMaterials[materialNumber].SetTextureOffset("_MainTex", newOffset);
            rendererMaterials[materialNumber].SetTextureOffset("_BumpMap", newOffset);
            rendererMaterials[materialNumber].SetTextureOffset("_ParallaxMap", newOffset);
            textureOffset = newOffset;
        }
        else
        {
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
	}
}
