using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Areas : MonoBehaviour
{
    BoxCollider[] colliders;
    BoxCollider deckCollider;
    BoxCollider swordCollider;
    BoxCollider bowCollider;
    BoxCollider trebuchetCollider;
    BoxCollider special1Collider;
    BoxCollider special2Collider;
    BoxCollider sword2Collider;

    static Vector3 topRight;
    static Vector3 botLeft;
    static Vector3 center;
    static float height;
    static float width;

    float kingPadding = 4.5f;

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
        updateScreenBounds();

    }

    public Bounds getDeckColliderBounds()
    {
        return deckCollider.bounds;
    }

    private void updateScreenBounds()
    {
        topRight = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));
        botLeft = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
        height = Card.getBaseHeight();
        width = Card.getBaseWidth();
        center = (topRight + botLeft) / 2;
    }

    public Vector3 getCenterBottom()
    {
        updateScreenBounds();
        Vector3 centerBottom = new Vector3(
            0f,
            botLeft.y + height / 2,
            -2f);
        return centerBottom;
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

    public Vector3 getCenterFrontBig()
    {
        updateScreenBounds();
        Vector3 centerFront = new Vector3(
            center.x,
            center.y,
            -6f);
        return centerFront;
    }

    public Vector3 getDeckCenterVector()
    {
        return getCenterBottom();
    }


    public Vector3 getUnitGraveyardCenterVector()
    {
        updateScreenBounds();
        Vector3 unitGraveyard = new Vector3(
            topRight.x - width * 1.5f,
            center.y - height,
            0f);
        return unitGraveyard;
    }

    public Vector3 getPowerGraveyardCenterVector()
    {
        updateScreenBounds();
        Vector3 unitGraveyard = new Vector3(
            topRight.x - width * 1.5f,
            center.y,
            0f);
        return unitGraveyard;
    }

    public Vector3 getKingGraveyardCenterVector()
    {
        updateScreenBounds();
        Vector3 unitGraveyard = new Vector3(
            topRight.x - width * 1.5f,
            center.y + height,
            0f);
        return unitGraveyard;
    }

    public Vector3 getPowerDeckCenterVector()
    {
        updateScreenBounds();
        Vector3 unitGraveyard = new Vector3(
            topRight.x - width * 2.5f,
            center.y,
            0f);
        return unitGraveyard;
    }

    public Vector3 getUnitDeckCenterVector()
    {
        updateScreenBounds();
        Vector3 unitGraveyard = new Vector3(
            topRight.x - width * 2.5f,
            center.y - height,
            0f);
        return unitGraveyard;
    }

    public Vector3 getKingDeckCenterVector()
    {
        updateScreenBounds();
        Vector3 unitGraveyard = new Vector3(
            topRight.x - width * 2.5f,
            center.y + height,
            0f);
        return unitGraveyard;
    }


    public Vector3 getMeleeKingCenterVector()
    {
        updateScreenBounds();
        Vector3 kingCenter = new Vector3(
            botLeft.x + width * kingPadding,
            botLeft.y + height * 3.5f,
            0f);
        return kingCenter;
    }

    public Vector3 getRangedKingCenterVector()
    {
        updateScreenBounds();
        Vector3 kingCenter = new Vector3(
            botLeft.x + width * kingPadding,
            botLeft.y + height * 2.5f,
            0f);
        return kingCenter;
    }

    public Vector3 getSiegeKingCenterVector()
    {        
        updateScreenBounds();
        Vector3 kingCenter = new Vector3(
            botLeft.x + width * kingPadding,
            botLeft.y + height * 1.5f,
            0f);
        return kingCenter;
    }


    public Vector3 getMeleeRowCenterVector()
    {
        return getCenterBottom() + new Vector3(0f, Card.getBaseHeight() * 3, 0f);
    }

    public Vector3 getRangedRowCenterVector()
    {
        return getCenterBottom() + new Vector3(0f, Card.getBaseHeight() * 2, 0f);
    }

    public Vector3 getSiegeRowCenterVector()
    {   
        Debug.Log(getCenterBottom());
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
        return getCenterBottom() + new Vector3(0f, Card.getBaseHeight() * 4, 0f);
    }
    public Vector3 getEnemyRangedRowCenterVector()
    {
        return getCenterBottom() + new Vector3(0f, Card.getBaseHeight() * 5, 0f);
    }
    public Vector3 getEnemySiegeRowCenterVector()
    {
        return getCenterBottom() + new Vector3(0f, Card.getBaseHeight() * 6, 0f);
    }

    public Vector3 getEnemyMeleeKingCenterVector()
    {
        updateScreenBounds();
        Vector3 kingCenter = new Vector3(
            botLeft.x + width * kingPadding,
            botLeft.y + height * 4.5f,
            0f);
        return kingCenter;
    }

    public Vector3 getEnemyRangedKingCenterVector()
    {
        updateScreenBounds();
        Vector3 kingCenter = new Vector3(
            botLeft.x + width * kingPadding,
            botLeft.y + height * 5.5f,
            0f);
        return kingCenter;
    }

    public Vector3 getEnemySiegeKingCenterVector()
    {        
        updateScreenBounds();
        Vector3 kingCenter = new Vector3(
            botLeft.x + width * kingPadding,
            botLeft.y + height * 6.5f,
            0f);
        return kingCenter;
    }

    private enum CardGroup { DECK, SWORD, BOW, TREBUCHET, SPECIAL1, SPECIAL2, SWORD2 };
}