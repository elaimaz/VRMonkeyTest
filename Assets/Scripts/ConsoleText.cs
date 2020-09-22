using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleText : MonoBehaviour {

    public Text textObj;
    public float messageTime;

    static ConsoleText _instance;
    public static ConsoleText getInstance()
    {
        if (_instance == null)
        {
            _instance = GameObject.FindObjectOfType<ConsoleText>();
        }

        return _instance;
    }

	public void ShowMessage(string message)
    {
        StopAllCoroutines();
        textObj.text = message;
        textObj.enabled = true;
        StartCoroutine(consoleWaitRoutine());
    }

    IEnumerator consoleWaitRoutine()
    {
        yield return new WaitForSeconds(messageTime);
        textObj.enabled = false;
    }
}
