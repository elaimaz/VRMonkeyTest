using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HUDManager : MonoBehaviour {

    public bool startWithFadeInWhite = false;
    public bool startWithFadeIn = true;
    public Image blackScreen;
    public Image whiteScreen;
    [Header("Death")]
    public Image whiteOverlay;
    public Image blackOverlay;

    private static HUDManager _instance;
    public static HUDManager instance
    {
        get
        {
            //If _instance hasn't been set yet, we grab it from the scene!
            //This will only happen the first time this reference is used.
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<HUDManager>();
            return _instance;
        }
    }

    void Start()
    {
        if (startWithFadeIn)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 1);
            FadeOutBlack(1);
        }
        if (startWithFadeInWhite)
        {
            whiteScreen.color = Color.white;
            FadeOutWhite(1);
        }
    }

    public void FadeInBlack(float time)
    {
        StartCoroutine(FadeInBlackRoutine(time));
    }

    public IEnumerator FadeInBlackRoutine(float time)
    {
        float currentTime = 0;
        while (currentTime < time)
        {
            currentTime += Time.unscaledDeltaTime;
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, currentTime / time);
            yield return null;
        }
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 1);
    }
    public void FadeOutBlack(float time)
    {
        StartCoroutine(FadeOutBlackRoutine(time));
    }

    public IEnumerator FadeOutBlackRoutine(float time)
    {
        float currentTime = 0;
        while (currentTime < time)
        {
            currentTime += Time.unscaledDeltaTime;
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 1.0f - (currentTime / time));
            yield return null;
        }
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 0);
    }

    public void FadeInWhite(float time)
    {
        StartCoroutine(FadeInWhiteRoutine(time));
    }

    public IEnumerator FadeInWhiteRoutine(float time)
    {
        float currentTime = 0;
        while (currentTime < time)
        {
            currentTime += Time.unscaledDeltaTime;
            whiteScreen.color = new Color(whiteScreen.color.r, whiteScreen.color.g, whiteScreen.color.b, currentTime / time);
            yield return null;
        }
        whiteScreen.color = new Color(whiteScreen.color.r, whiteScreen.color.g, whiteScreen.color.b, 1);
    }

    public void FadeOutWhite(float time)
    {
        StartCoroutine(FadeOutWhiteRoutine(time));
    }

    public IEnumerator FadeOutWhiteRoutine(float time)
    {
        float currentTime = 0;
        while (currentTime < time)
        {
            currentTime += Time.unscaledDeltaTime;
            whiteScreen.color = new Color(whiteScreen.color.r, whiteScreen.color.g, whiteScreen.color.b, 1.0f - (currentTime / time));
            yield return null;
        }
        whiteScreen.color = new Color(whiteScreen.color.r, whiteScreen.color.g, whiteScreen.color.b, 0);
    }
    public void FlashWhite(float time = 0.5f)
    {
        StartCoroutine(FlashWhiteRoutine(time));
    }

    public IEnumerator FlashWhiteRoutine(float time)
    {
        float initialTime = Time.realtimeSinceStartup;
        float currentTime = initialTime;

        while (currentTime < initialTime + time / 2.0f)
        {
            currentTime = Time.realtimeSinceStartup;
            whiteScreen.color = new Color(whiteScreen.color.r, whiteScreen.color.g, whiteScreen.color.b, (currentTime - initialTime) / time);
            yield return null;
        }
        initialTime = Time.realtimeSinceStartup;
        currentTime = initialTime;
        while (currentTime < initialTime + time / 2.0f)
        {
            currentTime = Time.realtimeSinceStartup;
            whiteScreen.color = new Color(whiteScreen.color.r, whiteScreen.color.g, whiteScreen.color.b, 1.0f - ((currentTime - initialTime) / time));
            yield return null;
        }
        whiteScreen.color = new Color(whiteScreen.color.r, whiteScreen.color.g, whiteScreen.color.b, 0);
    }



}
