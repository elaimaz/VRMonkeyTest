using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperBattery : MonoBehaviour {

    Renderer thisRenderer;

 

    public void OnTriggerEnter(Collider col)
    {
        StealthPlayerController player = StealthPlayerController.getInstance();
        if (col.gameObject == player.gameObject)
        {
            ConsoleText.getInstance().ShowMessage("Prototype Battery acquired");
            GameObject.Instantiate(EffectsManager.getInstance().itemEffect, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
            GameLogic.instance.EndGame();
        }
    }
}