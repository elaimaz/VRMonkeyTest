using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour {

	public void StartGame()
    {
        HUDManager.instance.FadeInBlack(1.0f);
        StartCoroutine(startGameRoutine());
    }

    IEnumerator startGameRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(1);
    }
}
