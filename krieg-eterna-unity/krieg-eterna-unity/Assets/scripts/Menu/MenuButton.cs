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


    TMP_Text text;
    private string buttonText;
    private bool buttonVisible = false;

    private Material material;
    private Func<Vector3> centerFunction;
    private Func<Vector3> mouseOverFunction;
    private bool inMouseOverPos;
    public Action buttonAction;


    float lerpDuration = 0.1f;
    float timeElapsed;
    private Vector3 startLoc;
    private Vector3 targetLoc;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cardColider = GetComponent<BoxCollider2D>();
        Vector3 cardDims = cardColider.size;

        Transform textObj = this.transform.Find("Text");
        text = textObj.GetComponent<TMP_Text>();
        text.text = "";
        baseLoc = this.transform.position;
        timeElapsed = lerpDuration;
        flashing = false;
        this.setNotFlashing();
    }
    public void setText(string str)
    {
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

    public bool ContainsMouse(Vector3 mousePos)
    {
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
    public void setButtonNotVisible()
    {
        this.buttonVisible = false;
        this.text.alpha = 0f;
        this.getMaterial().SetInt("_Transparent", 1);
    }
    public void setButtonVisible()
    {
        this.buttonVisible = true;
        this.text.alpha = 100f;
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

    internal void setUp(string name, Func<Vector3> centerFunction, Func<Vector3> mouseOverFunction, Action buttonAction, bool visible)
    {
        this.name = name;
        this.text.text = name;
        this.buttonText = name;
        this.transform.position = centerFunction.Invoke();
        this.baseLoc = this.transform.position;
        this.startLoc = this.transform.position;
        this.targetLoc = this.transform.position;
        this.centerFunction = centerFunction;
        this.mouseOverFunction = mouseOverFunction;
        this.inMouseOverPos = false;
        this.buttonAction = buttonAction;
        if (visible)
        {
            this.setButtonVisible();
            this.setFlashing();
        }
        else
        {
            this.setButtonNotVisible();
            this.setNotFlashing();
        }
        setBaseLoc();
    }
    internal void mouseOver(bool mouseOver)
    {
        if (mouseOver)
        {
            if (!inMouseOverPos)
            {
                inMouseOverPos = true;
                startLoc = this.transform.position;
                targetLoc = mouseOverFunction.Invoke();
                timeElapsed = 0f;
            }
        }
        else
        {
            if (inMouseOverPos)
            {
                inMouseOverPos = false;
                startLoc = this.transform.position;
                targetLoc = centerFunction.Invoke();
                timeElapsed = 0f;
            }
        }
        if (timeElapsed < lerpDuration)
        {
            this.transform.position = Vector3.Lerp(startLoc, targetLoc, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
        }
    }
}
