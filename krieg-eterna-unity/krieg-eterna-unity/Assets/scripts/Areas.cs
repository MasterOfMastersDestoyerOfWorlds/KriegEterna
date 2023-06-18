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

    private Vector3 getCenterBottom()
    {

        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));
        Vector3 botLeft = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
        var height = Card.getBaseHeight();
        Vector3 centerBottom = new Vector3(
            0f,
            botLeft.y + height/2,
            -2f);
        return centerBottom;
    }

    public Vector3 getDeckCenterVector()
    {
        return getCenterBottom();
    }


    public Vector3 getUnitGraveyardCenterVector()
    {
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));
        Vector3 botLeft = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
        var height = Card.getBaseHeight();
        var width = Card.getBaseWidth();
        Vector3 center = (topRight + botLeft)/2;
        Vector3 unitGraveyard = new Vector3(
            topRight.x - width*1.5f,
            center.y + height/2,
            0f);
        return unitGraveyard;
    }

        public Vector3 getPowerGraveyardCenterVector()
    {
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));
        Vector3 botLeft = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
        var height = Card.getBaseHeight();
        var width = Card.getBaseWidth();
        Vector3 center = (topRight + botLeft)/2;
        Vector3 unitGraveyard = new Vector3(
            topRight.x - width*1.5f,
            center.y - height/2,
            0f);
        return unitGraveyard;
    }



    public Vector3 getMeleeRowCenterVector()
    {
        return getCenterBottom() + new Vector3(0f, Card.getBaseHeight()*3, 0f);
    }

    public Vector3 getRangedRowCenterVector()
    {
        return getCenterBottom() + new Vector3(0f, Card.getBaseHeight()*2, 0f);
    }

    public Vector3 getSiegeRowCenterVector()
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

    public Vector3 getEnemyMeleeRowCenterVector()
    {
        return getCenterBottom() + new Vector3(0f, Card.getBaseHeight()*4, 0f);
    }
    public Vector3 getEnemyRangedRowCenterVector()
    {
        return getCenterBottom() + new Vector3(0f, Card.getBaseHeight()*5, 0f);
    }
    public Vector3 getEnemySiegeRowCenterVector()
    {
        return getCenterBottom() + new Vector3(0f, Card.getBaseHeight()*6, 0f);
    }

    private enum CardGroup { DECK, SWORD, BOW, TREBUCHET, SPECIAL1, SPECIAL2, SWORD2};
}