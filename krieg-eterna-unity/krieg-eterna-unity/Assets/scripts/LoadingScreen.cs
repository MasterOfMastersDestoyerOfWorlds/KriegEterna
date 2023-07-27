using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingScreen : MonoBehaviour
{

    public static float FADE_SPEED = 1.5f;
    public TMP_Text roundText;
    void Awake()
    {
        roundText = transform.Find("RoundText").GetComponent<TMP_Text>();;
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
            Debug.Log("REEEEE: " + m.name + " " + newAlpha);
            minalpha = newAlpha;
        }
        if (minalpha >= 1)
        {
            return true;
        }
        return false;
    }

    public void setRoundTextVisibile(bool state)
    {
        TMP_Text text = transform.Find("RoundText").GetComponent<TMP_Text>();
        text.alpha = state ? 1 : 0;
    }
    public bool FadeInRoundText()
    {
        TMP_Text text = transform.Find("RoundText").GetComponent<TMP_Text>();
        float newAlpha = Mathf.Clamp(text.alpha + Time.deltaTime * FADE_SPEED, 0, 1);
        text.alpha = newAlpha;
        if (newAlpha >= 1)
        {
            return true;
        }
        return false;
    }

    public bool FadeOutRoundText()
    {
        float newAlpha = Mathf.Clamp(roundText.alpha - Time.deltaTime * FADE_SPEED, 0, 1);
        roundText.alpha = newAlpha;
        if( newAlpha <= 0){
            return true;
        }
        return false;
    }
}
