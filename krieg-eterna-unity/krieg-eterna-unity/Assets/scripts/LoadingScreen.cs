using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{

    public static float FADE_SPEED = 1.5f;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public bool FadeOut()
    {
        float maxalpha = 1;
        foreach (Image g in gameObject.GetComponentsInChildren<Image>()){
           Material m = g.material;
           float a = m.GetFloat("_Alpha");
           float newAlpha =  Mathf.Clamp(a - Time.deltaTime * FADE_SPEED, 0, 1);
           m.SetFloat("_Alpha",  newAlpha);
           maxalpha = newAlpha;
        }
        if(maxalpha <= 0){
            return true;
        }
        return false;
    }

    public void setVisibile(bool state)
    {
        foreach (Image g in gameObject.GetComponentsInChildren<Image>()){
           Material m = g.material;
           m.SetInt("_Transparent", state? 0 : 1);
           m.SetFloat("_Alpha", state? 1 : 0);
        }
    }

    public bool FadeIn()
    {
        float minalpha = 0;
        foreach (Image g in gameObject.GetComponentsInChildren<Image>()){
           Material m = g.material;
           float a = m.GetFloat("_Alpha");
           Debug.Log("MatAlpha: " + a);
           float newAlpha =  Mathf.Clamp(a + Time.deltaTime * FADE_SPEED, 0 ,1);
           m.SetFloat("_Alpha", newAlpha);
           minalpha = newAlpha;
        }
        if(minalpha >= 1){
            return true;
        }
        return false;
    }
}
