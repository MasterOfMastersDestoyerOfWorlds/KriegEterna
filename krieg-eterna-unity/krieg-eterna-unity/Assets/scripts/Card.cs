using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public string cardName;
    public float rowMultiple;
    public RowEffected rowEffected;
    public int index;
    public CardType cardType;
    private bool active = false;
    public int isSpecial;
    public bool weatherEffect = false;
    public Vector3 baseLoc;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D cardColider;

    private GameObject cardModelGameObject;
    private CardModel cardModel;
    
    void Awake()
    {
        cardModelGameObject = GameObject.Find("CardModel");
        cardModel = cardModelGameObject.GetComponent<CardModel>();
		cardModel.readTextFile();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cardColider = GetComponent<BoxCollider2D>();
        baseLoc = this.transform.position;
    }

    public void setBaseLoc(){
        baseLoc = this.transform.position;
    }
    public void resetTransform(){
        this.transform.position = baseLoc;
    }

    public string getCardName()
    {
        return this.cardName;
    }
    public void setCardName(string cardName)
    {
        this.cardName = cardName;
    }

    public float getRowMultiple()
    {
        return this.rowMultiple;
    }

    public void setRowMultiple(float power)
    {
        this.rowMultiple = power;
    }

    public RowEffected getRowEffected()
    {
        return this.rowEffected;
    }

    public void setRowEffected(RowEffected row)
    {
        this.rowEffected = row;
    }

    public void setIndex(int index)
    {
        this.index = index;
        this.cardType = cardModel.cardTypes[index];
    }


    public int getIndex()
    {
        return this.index;
    }

    public void setActive(bool state)
    {
        this.active = state;
    }
    public bool isActive()
    {
        return this.active;
    }

    /// <summary>
    /// Get card's collision bounds
    /// </summary>
    /// <returns>Card's collision bounds</returns>
    public Bounds getBounds()
    {
        return this.cardColider.bounds;
    }

    /// <summary>
    /// Get card's name and it's power in string
    /// </summary>
    /// <returns>card's name and it's power in string</returns>
    public string toString()
    {
        return this.cardName + " card with power " + this.rowMultiple;
    }

    /// <summary>
    /// Set new card's front image
    /// </summary>
    /// <param name="index">New card's front image</param>
    public void setFront(int index)
    {
		GameObject child = this.transform.GetChild(0).gameObject;
		GameObject childOfChild = child.transform.GetChild(0).gameObject;
		MeshRenderer meshRend = childOfChild.GetComponent<MeshRenderer>();
		Material material = meshRend.materials[2];
		material.SetTexture("_MainTex", cardModel.getSmallFront(index));
        //spriteRenderer.sprite = cardModel.getSmallFront(index);
    }

    /// <summary>
    /// Set new card's front image from big cards set
    /// </summary>
    /// <param name="index">index of new big front image</param>
    public void setBigFront(int index)
    {
        //if (index == 0)
          //  spriteRenderer.sprite = null;
        //else
            //spriteRenderer.sprite = cardModel.getBigFront(index - 1);
    }

    public void setCardType(CardType cardType)
    {
        this.cardType = cardType;
    }

    public CardType getCardType()
    {
        return this.cardType;
    }

    /// <summary>
    /// Get a cardModel object
    /// </summary>
    /// <returns>cardModel object</returns>
    public CardModel getCardModel()
    {
        return this.cardModel;
    }

    /// <summary>
    /// Get is Special status of card
    /// </summary>
    /// <returns>special group value of card ([0] - normal, [1] - gold, [2] - spy, [3] - manekin, [4] - destroy, [5] - weather)</returns>
    public int getIsSpecial()
    {
        return this.isSpecial;
    }

    /// <summary>
    /// Set a isSpecial attribute
    /// </summary>
    /// <param name="isSpecial">true if card is special type</param>
    public void setIsSpecial(int isSpecial)
    {
        this.isSpecial = isSpecial;
    }

    /// <summary>
    /// Flip card
    /// </summary>
    /// <param name="x">true if you want to flip in x axis</param>
    /// <param name="y">true if you want to flip in y axis</param>
    public void flip(bool x, bool y)
    {
        if (x == true)
        {
            if (spriteRenderer.flipX == true)
                spriteRenderer.flipX = false;
            else
                spriteRenderer.flipX = true;
        }
        if (y == true)
        {
            if (spriteRenderer.flipY == true)
                spriteRenderer.flipY = false;
            else
                spriteRenderer.flipY = true;
        }
    }

    /// <summary>
    /// Mirror transformation around (0,0,0) point of Desk
    /// </summary>
    public void mirrorTransform()
    {
        transform.position = new Vector3(transform.position.x * -1 + 4.39f, transform.position.y * -1 + 1.435f, transform.position.z);
    }
}