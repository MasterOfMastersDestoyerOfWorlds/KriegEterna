using UnityEngine;
using TMPro;
using System;

public class MenuButton : MonoBehaviour
{
    public Vector3 baseLoc;

    private static float baseHeight;
    private static float baseWidth;

    private static float scaleHeight;
    private static float scaleWidth;

    private static float screenHeight;
    private static float screenWidth;

    public bool flashing;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D cardColider;

    
    TMP_Text  text;
    private bool buttonVisible = false;

    private Material material;
    private Func<Vector3> centerFunction;
    public Action buttonAction;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cardColider = GetComponent<BoxCollider2D>();
        Vector3 cardDims = cardColider.size;

        Transform textObj = this.transform.Find("Text");
        text = textObj.GetComponent<TMP_Text>();
        text.text = "";
        baseLoc = this.transform.position;
        flashing = false;
        this.setNotFlashing();
    }
    public void setText(string str){
        text.text = str;
    }
    public void setBaseLoc()
    {
        baseLoc = this.transform.position;
    }
    public void resetTransform()
    {
        this.transform.position = baseLoc;
    }
    public bool isVisible()
    {
        return this.buttonVisible;
    }

    public bool isFlashing()
    {
        return this.flashing;
    }

    public Bounds getBounds()
    {
        return this.cardColider.bounds;
    }

    public static float getBaseHeight()
    {
        if (baseHeight == 0f)
        {
            baseHeight = scaleHeight;
        }
        return baseHeight;
    }

    public static float getBaseWidth()
    {
        if (baseWidth == 0f)
        {
            baseWidth = scaleWidth;
        }
        return baseWidth;
    }

    public bool ContainsMouse(Vector3 mousePos){
        mousePos.z = this.transform.position.z;
        return this.cardColider.bounds.Contains(mousePos);
    }

    public static float getBaseThickness()
    {
        return 0.1f;
    }

    public string ToString()
    {
        return "MenuButton";
    }

    private Material getMaterial()
    {
        if (this.material == null)
        {
            GameObject child = this.transform.GetChild(0).gameObject;
            GameObject childOfChild = child.transform.GetChild(0).gameObject;
            MeshRenderer meshRend = childOfChild.GetComponent<MeshRenderer>();
            this.material = meshRend.materials[2];
        }
        return this.material;
    }
    public void setFlashing(bool state)
    {
        
        if (state)
        {
            this.setFlashing();
        }
        else
        {
            this.setNotFlashing();
        }
    }
    public void setButtonVisible()
    {
        this.buttonVisible = true;
        this.getMaterial().SetInt("_Transparent", 0);
    }
    public void setFlashing()
    {
        this.flashing = true;
        this.getMaterial().SetInt("_Flash", 1);
        this.getMaterial().SetInt("_TransparentFlash", 1);
        this.getMaterial().SetInt("_Transparent", 0);
    }
    public void setNotFlashing()
    {
        flashing = false;
        this.getMaterial().SetInt("_Flash", 1);
        this.getMaterial().SetInt("_TransparentFlash", 1);
        this.getMaterial().SetInt("_Transparent", 1);
    }

    internal void setUp(string name, Func<Vector3> centerFunction, Action buttonAction, bool visible)
    {
        this.name = name;
        this.text.text = name;
        this.transform.position = centerFunction.Invoke();
        this.centerFunction = centerFunction;
        this.buttonAction = buttonAction;
        if(visible){
            this.setButtonVisible();
            this.setFlashing();
        }
        setBaseLoc();
    }
}
