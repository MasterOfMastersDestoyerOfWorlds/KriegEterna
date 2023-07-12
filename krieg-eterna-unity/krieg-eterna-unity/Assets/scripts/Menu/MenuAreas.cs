using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAreas : MonoBehaviour
{
    static Vector3 topRight;
    static Vector3 botLeft;
    static Vector3 center;


    void Awake() 
    {
        updateScreenBounds();
    }

    private void updateScreenBounds()
    {
        topRight = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));
        botLeft = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
        center = (topRight + botLeft) / 2;
    }

    public Vector3 getCenterFront()
    {
        updateScreenBounds();
        Vector3 centerFront = new Vector3(
            center.x,
            center.y,
            -5f);
        return centerFront;
    }

    public Vector3 getButtonLocFromBot(int spacer)
    {
        
        updateScreenBounds();
        Vector3 centerFront = new Vector3(
            botLeft.x,
            (botLeft.y * (0.5f) ) + (botLeft.y/5)*spacer,
            -5f);
        return centerFront;
    }
}