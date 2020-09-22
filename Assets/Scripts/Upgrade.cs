using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour {

    public enum Type { shock,cloak,drain};
    public Type type = Type.shock;

    public void OnTriggerEnter(Collider col)
    {
        StealthPlayerController player = StealthPlayerController.getInstance();
        if (col.gameObject == player.gameObject)
        {
            GameLogic.instance.EnablePlayerSkill(type);

            Destroy(gameObject);
        }
    }
}
