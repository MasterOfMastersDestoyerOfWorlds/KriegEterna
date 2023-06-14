using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Areas : MonoBehaviour {
    BoxCollider [] colliders;
    BoxCollider deckCollider;
    BoxCollider swordCollider;
    BoxCollider bowCollider;
    BoxCollider trebuchetCollider;
    BoxCollider special1Collider;
    BoxCollider special2Collider;
    BoxCollider sword2Collider;

    void Awake()
    {
        colliders = GetComponents<BoxCollider>();
        
        deckCollider = colliders[(int)CardGroup.DECK];
        swordCollider = colliders[(int)CardGroup.SWORD];
        bowCollider = colliders[(int)CardGroup.BOW];
        trebuchetCollider = colliders[(int)CardGroup.TREBUCHET];
        special1Collider = colliders[(int)CardGroup.SPECIAL1];
        special2Collider = colliders[(int)CardGroup.SPECIAL2];
        sword2Collider = colliders[(int)CardGroup.SWORD2];
    }

    /// <summary>
    /// Get player deck's collision bounds
    /// </summary>
    /// <returns>Deck's collision bounds</returns>
    public Bounds getDeckColliderBounds()
    {
        return deckCollider.bounds;
    }

    /// <summary>
    /// Get sword group collision bounds
    /// </summary>
    /// <returns>Sword group collision bounds</returns>
    public Bounds getSwordColliderBounds()
    {
        return swordCollider.bounds;
    }

    /// <summary>
    /// Get bow group collision bounds
    /// </summary>
    /// <returns>Bow group collision bounds</returns>
    public Bounds getBowColliderBounds()
    {
        return bowCollider.bounds;
    }

    /// <summary>
    /// Get trebuchet group collision bounds
    /// </summary>
    /// <returns>Trebuchet group bounds</returns>
    public Bounds getTrebuchetColliderBounds()
    {
        return trebuchetCollider.bounds;
    }

    /// <summary>
    /// Get special 1 group collision bounds
    /// </summary>
    /// <returns>Special 1 group bounds</returns>
    public Bounds getSpecial1ColliderBounds()
    {
        return special1Collider.bounds;
    }

    /// <summary>
    /// Get special 2 group collision bounds
    /// </summary>
    /// <returns>Special 2 group bounds</returns>
    public Bounds getSpecial2ColliderBounds()
    {
        return special2Collider.bounds;
    }

    /// <summary>
    /// Get sword in player 2 group collision bounds
    /// </summary>
    /// <returns>Sword in player 2 group bounds</returns>
    public Bounds getSword2ColliderBounds()
    {
        return sword2Collider.bounds;
    }

    private Vector3 getCenterBottom()
    {

        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));
        Vector3 botLeft = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
        var height = Card.getBaseHeight();
        Vector3 centerBottom = new Vector3(
            0f,
            botLeft.y + height/2,
            0f);
        return centerBottom;
    }

    public Vector3 getDeckCenterVector()
    {
        return getCenterBottom();
    }



    public Vector3 getSwordsCenterVector()
    {
        return getCenterBottom() + new Vector3(0f, Card.getBaseHeight()*3, 0f);
    }

    public Vector3 getBowsCenterVector()
    {
        return getCenterBottom() + new Vector3(0f, Card.getBaseHeight()*2, 0f);
    }

    public Vector3 getTrebuchetsCenterVector()
    {
        return getCenterBottom() + new Vector3(0f, Card.getBaseHeight(), 0f);
    }

    public Vector3 getSpecial1CenterVector()
    {
        return special1Collider.center;
    }

    public Vector3 getSpecial2CenterVector()
    {
        return special2Collider.center;
    }

    public Vector3 getSword2CenterVector()
    {
        return getCenterBottom() + new Vector3(0f, Card.getBaseHeight()*4, 0f);
    }

    /// <summary>
    /// Defined typed of card groups
    /// </summary>
    private enum CardGroup { DECK, SWORD, BOW, TREBUCHET, SPECIAL1, SPECIAL2, SWORD2};
}