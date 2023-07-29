using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingScreen : MonoBehaviour
{

    public static float FADE_SPEED = 1.5f;

    public static float TEXT_FADE_SPEED = 0.5f;
    public TMP_Text roundText;
    public TMP_Text displayRowText;
    bool flashDisplayText = false;
    void Awake()
    {
        roundText = transform.Find("RoundText").GetComponent<TMP_Text>();
        displayRowText = transform.Find("DisplayText").GetComponent<TMP_Text>();
        DontDestroyOnLoad(gameObject);
    }

    public bool FadeOut()
    {
        float maxalpha = 1;
        foreach (Image g in gameObject.GetComponentsInChildren<Image>())
        {
            Material m = g.material;
            float a = m.GetFloat("_Alpha");
            float newAlpha = Mathf.Clamp(a - Time.deltaTime * FADE_SPEED, 0, 1);
            m.SetFloat("_Alpha", newAlpha);
            maxalpha = newAlpha;
        }
        if (maxalpha <= 0)
        {
            return true;
        }
        return false;
    }

    public void setVisibile(bool state)
    {
        foreach (Image g in gameObject.GetComponentsInChildren<Image>())
        {
            Material m = g.material;
            m.SetInt("_Transparent", state ? 0 : 1);
            m.SetFloat("_Alpha", state ? 1 : 0);
        }
    }

    public bool FadeIn()
    {
        float minalpha = 0;
        foreach (Image g in gameObject.GetComponentsInChildren<Image>())
        {
            Material m = g.material;
            float a = m.GetFloat("_Alpha");
            float newAlpha = Mathf.Clamp(a + Time.deltaTime * FADE_SPEED, 0, 1);
            m.SetFloat("_Alpha", newAlpha);
            minalpha = newAlpha;
        }
        if (minalpha >= 1)
        {
            return true;
        }
        return false;
    }

    public void setTextVisibile(TMP_Text text, bool state)
    {
        text.alpha = state ? 1 : 0;
    }
    public bool FadeInText(TMP_Text text, float maxAlpha = 1f)
    {
        float newAlpha = Mathf.Clamp(text.alpha + Time.deltaTime * TEXT_FADE_SPEED, 0, 1);
        text.alpha = newAlpha;
        if (newAlpha >= maxAlpha)
        {
            return true;
        }
        return false;
    }

    public bool FadeOutText(TMP_Text text, float minalpha = 0f)
    {
        float newAlpha = Mathf.Clamp(text.alpha - Time.deltaTime * TEXT_FADE_SPEED, 0, 1);
        text.alpha = newAlpha;
        if (newAlpha <= minalpha)
        {
            return true;
        }
        return false;
    }

    public IEnumerator roundTextFlash(RoundType round, bool playerWon, bool draw)
    {
        if (round != RoundType.GameFinished)
        {
            roundText.text = draw ? "Draw" : playerWon ? "Round Won" : "Round Lost";
        }
        else
        {
            roundText.text = draw ? "Draw" : playerWon ? "Battle Won" : "Battle Lost";
        }
        setTextVisibile(roundText, false);
        while (!FadeInText(roundText))
        {
            yield return null;
        }

        while (!FadeOutText(roundText))
        {
            yield return null;
        }
        if (round == RoundType.GameFinished)
        {
            Debug.Log("GAME OVER");
            Game.battleOver();
        }
    }

    internal Coroutine displayTextFlash()
    {
        setTextVisibile(displayRowText, false);
        return StartCoroutine(displayTextFlashCoroutine());
    }
    internal void stopDisplayTextFlash(Coroutine c)
    {
        StopCoroutine(c);
        setTextVisibile(displayRowText, false);
    }


    public IEnumerator displayTextFlashCoroutine()
    {
        while(true){
            while (!FadeInText(displayRowText, 1f))
            {
                yield return null;
            }

            while (!FadeOutText(displayRowText, 0.5f))
            {
                yield return null;
            }
            yield return null;
        }
    }
}
