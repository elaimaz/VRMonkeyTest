using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMessageBox : MonoBehaviour {


    public string text;

    public void OnTriggerEnter(Collider col)
    {
        StealthPlayerController player = StealthPlayerController.getInstance();
        if (col.gameObject == player.gameObject)
        {
            GameLogic.instance.ShowMessageBox(text);
            Destroy(gameObject);
        }
    }
}
