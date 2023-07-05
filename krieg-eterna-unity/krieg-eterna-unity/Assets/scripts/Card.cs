using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int index;
    public string cardName;
    public CardType cardType;
    public int strength;
    public int calculatedStrength;
    public int playerCardDraw;
    public CardDrawType cardDrawType;
    public int playerCardDrawRemain;
    public int playerCardDestroy;
    public int playerCardDestroyRemain;
    public DestroyType destroyType;
    public int playerCardReturn;
    public int playerCardReturnRemain;
    public CardReturnType cardReturnType;
    public float strengthModifier;
    private float strengthMultiple;
    public StrengthModType strengthModType;
    public int graveyardCardDraw;
    public int graveyardCardDrawRemain;
    public int enemyCardDraw;
    public int enemyCardDrawRemain;
    public int enemyCardDestroy;
    public int enemyCardDestroyRemain;
    public int enemyReveal;
    public float rowMultiple;
    public RowEffected rowEffected;
    public int setAside;
    public int setAsideRemain;
    public RowEffected setAsideReturnRow;
    public SetAsideType setAsideType;
    public bool attach;
    public int attachmentsRemaining;
    public int strengthCondition;
    public bool strengthConditionPassed;
    public int chooseN;
    public RowEffected chooseRow;
    public int chooseShowN;
    public bool playInRow;
    public int moveRemain;
    public Card moveCard;
    public RowEffected moveRow;
    private bool targetActive = false;
    public bool isBig = false;
    public float bigFac = 2;
    public int isSpecial;
    public bool weatherEffect = false;
    public bool beenRevealed;
    public Vector3 baseLoc;

    public List<Card> attachments;


    private static float baseHeight;
    private static float baseWidth;

    private static float baseScalex;
    private static float baseScalez;

    private static float scaleHeight;
    private static float scaleWidth;

    private static float screenHeight;
    private static float screenWidth;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D cardColider;

    private Material material;

    private static GameObject cardModelGameObject;
    private static CardModel cardModel;

    void Awake()
    {
        if (cardModelGameObject == null)
        {
            cardModelGameObject = GameObject.Instantiate(Resources.Load("Prefabs/CardModel") as GameObject, transform.position, transform.rotation);
            cardModel = cardModelGameObject.GetComponent<CardModel>();
        }
        cardModel.readTextFile();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cardColider = GetComponent<BoxCollider2D>();
        if (scaleHeight == 0f || scaleWidth == 0f)
        {
            setBaseScale();
        }
        baseLoc = this.transform.position;
    }

    public void scaleBig()
    {
        isBig = true;
        Transform cardObj = this.transform.Find("Card 1");
        bigFac = 4;
        Debug.Log("big scale: " + this.cardName + "bigFac" + bigFac + "scalex " + bigFac * baseScalex + " scaley " + bigFac * baseScalez);
        cardObj.transform.localScale = new Vector3(bigFac * baseScalex, 1, bigFac * baseScalez);
    }
    public void resetScale()
    {
        Debug.Log("resetting scale: " + this.cardName + "scalex " + baseScalex + " scaley " + baseScalez);
        isBig = false;
        Transform cardObj = this.transform.Find("Card 1");
        cardObj.transform.localScale = new Vector3(baseScalex, 1, baseScalez);
    }

    public bool ContainsMouse(Vector3 mousePos)
    {
        Vector3 pos = new Vector3(mousePos.x, mousePos.y, this.cardColider.bounds.center.z);
        this.cardColider.offset = new Vector2(0f, 0f);
        if (!transform.position.Equals(cardColider.bounds.center))
        {
            Debug.Log("REEEEEEEEE offcenter");
            Vector3 screenvec = Camera.main.WorldToScreenPoint(transform.position);
            Debug.Log("+++++++++++++++++++++++++++++");
            Debug.Log("Card Bounds Check: card center: " + transform.position + " mouse center: " + pos + " screenvec center: " + screenvec);
            Debug.Log("+++++++++++++++++++++++++++++");
            if (mousePos.x > transform.position.x - (cardColider.bounds.extents.x) && mousePos.x < transform.position.x + (cardColider.bounds.extents.x))
            {
                if (mousePos.y > transform.position.y - (cardColider.bounds.extents.y) && mousePos.y < transform.position.y + (cardColider.bounds.extents.y))
                {
                    return true;
                }
            }
            return false;
        }
        Debug.Log("+++++++++++++++++++++++++++++");
        Debug.Log("Card Bounds Check: card center: " + transform.position + " mouse center: " + pos + " collider center: " + this.cardColider.bounds.center);
        Debug.Log("+++++++++++++++++++++++++++++");
        return this.cardColider.bounds.Contains(pos);
    }


    public void setBaseScale()
    {
        BoxCollider2D cardColider = GetComponent<BoxCollider2D>();
        Vector3 cardDims = cardColider.size;
        Debug.Log("setting scale Hieght");
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));
        Vector3 botLeft = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
        screenHeight = Mathf.Abs(topRight.y - botLeft.y);
        screenWidth = Mathf.Abs(topRight.x - botLeft.x);
        scaleHeight = screenHeight / 7;
        scaleWidth = (cardDims.x / cardDims.y) * scaleHeight;
        Transform cardObj = this.transform.Find("Card 1");
        baseScalex = (scaleWidth / cardDims.x) * cardObj.transform.localScale.x;
        baseScalez = scaleHeight / cardDims.y * cardObj.transform.localScale.z;
        cardObj.transform.localScale = new Vector3(baseScalex, 1, baseScalez);
        cardColider.size = new Vector2(scaleWidth, scaleHeight);
    }

    public void setBaseLoc()
    {
        baseLoc = this.transform.position;
    }
    public void resetTransform()
    {
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

    public RowEffected getRowEffected()
    {
        return this.rowEffected;
    }


    public void setIndex(int index)
    {
        this.index = index;
        this.cardName = cardModel.names[index];
        this.cardType = cardModel.cardTypes[index];
        this.strength = cardModel.strength[index];
        this.playerCardDraw = cardModel.playerCardDraw[index];
        this.cardDrawType = cardModel.cardDrawType[index];
        this.playerCardDestroy = cardModel.playerCardDestroy[index];
        this.destroyType = cardModel.destroyType[index];
        this.playerCardReturn = cardModel.playerCardReturn[index];
        this.cardReturnType = cardModel.cardReturnType[index];
        this.strengthModifier = cardModel.strengthModifier[index];
        this.strengthModType = cardModel.strengthModType[index];
        this.graveyardCardDraw = cardModel.graveyardCardDraw[index];
        this.enemyCardDraw = cardModel.enemyCardDraw[index];
        this.enemyCardDestroy = cardModel.enemyCardDestroy[index];
        this.enemyReveal = cardModel.enemyReveal[index];
        this.rowMultiple = cardModel.rowMultiple[index];
        this.rowEffected = cardModel.rowEffected[index];
        this.setAside = cardModel.setAside[index];
        this.setAsideType = cardModel.setAsideType[index];
        this.attach = cardModel.attach[index];
        this.strengthCondition = cardModel.strengthCondition[index];
        this.chooseN = cardModel.chooseN[index];
        this.chooseRow = cardModel.chooseRow[index];
        this.chooseShowN = cardModel.chooseShowN[index];
        this.playInRow = cardModel.playInRow[index];
        this.resetSelectionCounts();
    }


    public int getIndex()
    {
        return this.index;
    }

    public void resetSelectionCounts()
    {

        this.playerCardDrawRemain = playerCardDraw;
        this.playerCardDestroyRemain = this.playerCardDestroy;
        this.playerCardReturnRemain = this.playerCardReturn;
        this.enemyCardDestroyRemain = this.enemyCardDestroy;
        this.enemyCardDrawRemain = this.enemyCardDraw;
        this.setAsideRemain = this.setAside;
        this.graveyardCardDrawRemain = this.graveyardCardDraw;
        if(attach && strengthModType == StrengthModType.Adjacent){
            this.attachmentsRemaining = 2;
        }else if (attach){
            this.attachmentsRemaining = 1;
        }       
        this.strengthConditionPassed = false;
    }

    public void zeroSelectionCounts()
    {

        this.playerCardDrawRemain = 0;
        this.playerCardDestroyRemain = 0;
        this.playerCardReturnRemain = 0;
        this.enemyCardDestroyRemain = 0;
        this.enemyCardDrawRemain = 0;
        this.setAsideRemain = 0;
        this.graveyardCardDrawRemain = 0;
        this.attachmentsRemaining = 0;
    }

    public bool doneMultiSelection()
    {
        return (this.playerCardDrawRemain <= 0 || this.cardDrawType != CardDrawType.Either) && this.playerCardDestroyRemain <= 0
        && this.playerCardReturnRemain <= 0 && (this.enemyCardDestroyRemain <= 0  || this.destroyType == DestroyType.RoundEnd)
        && this.setAsideRemain <= 0 && this.moveRemain <= 0 && this.attachmentsRemaining <= 0;
    }

    public void LogSelectionsRemaining()
    {
        Debug.Log("Remaining Selections: " + cardName);
        Debug.Log("playerCardDraw: " + this.playerCardDrawRemain);
        Debug.Log("playerCardDestroy: " + this.playerCardDestroyRemain);
        Debug.Log("playerCardReturn: " + this.playerCardReturnRemain);
        Debug.Log("enemyCardDestroy: " + this.enemyCardDestroyRemain);
        Debug.Log("chooseN: " + this.chooseN);
        Debug.Log("chooseShowN: " + this.chooseShowN);
        Debug.Log("move: " + this.moveRemain);
        Debug.Log("setAside: " + this.setAsideRemain);
        Debug.Log("reveal: " + this.enemyReveal);
        Debug.Log("attachmentsRemaining: " + this.attachmentsRemaining);
        Debug.Log("strengthConditionPassed: " + this.strengthConditionPassed);
    }

    public void setTargetActive(bool state)
    {
        Material material = getMaterial();
        material.SetInt("_Flash", state ? 1 : 0);
        this.targetActive = state;
    }
    public void setVisible(bool state)
    {
        Material material = getMaterial();
        material.SetInt("_Transparent", state ? 0 : 1);
    }

    public bool isVisible()
    {
        Material material = getMaterial();
        return material.GetInt("_Transparent") == 0 ? true: false;
    }
    public bool isTargetActive()
    {
        return this.targetActive;
    }

    public bool isPlayable(Deck deck, RowEffected player)
    {
        List<Row> playerRows = deck.getRowsByType(CardModel.getRowFromSide(player, RowEffected.PlayerPlayable));
        int playerRowsSum = 0;
        List<Row> enemyRows = deck.getRowsByType(CardModel.getRowFromSide(player, RowEffected.EnemyPlayable));
        int enemyRowsSum = 0;
        RowEffected playerKingRow = deck.getKingRow(player);
        RowEffected enemyKingRow = deck.getKingRow(CardModel.getEnemy(player));
        for (int i = 0; i < playerRows.Count; i++)
        {
            playerRowsSum += playerRows[i].Count;
        }
        for (int i = 0; i < enemyRows.Count; i++)
        {
            enemyRowsSum += enemyRows[i].Count;
        }
        if (this.destroyType == DestroyType.Unit && this.playerCardDestroy + this.playerCardReturn > playerRowsSum)
        {
            return false;
        }
        if (this.setAsideType == SetAsideType.Player && this.setAside > playerRowsSum)
        {
            return false;
        }
        if (this.setAsideType == SetAsideType.Enemy && this.setAside > enemyRowsSum)
        {
            return false;
        }
        if (this.setAsideType == SetAsideType.EnemyKing && enemyKingRow == RowEffected.None)
        {
            return false;
        }
        if (this.setAsideType == SetAsideType.King && playerKingRow == RowEffected.None)
        {
            return false;
        }

        return true;
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

    public static float getBaseThickness()
    {
        return 0.1f;
    }
    public int calculateBaseStrength(){
        int str = strength;
        foreach(Card attachment in attachments){
            switch(attachment.strengthModType){
                case StrengthModType.Set: str = (int)attachment.strengthModifier;break;
                default: break;
            }
        }
        foreach(Card attachment in attachments){
            switch(attachment.strengthModType){
                case StrengthModType.Add: str += (int)attachment.strengthModifier;break;
                case StrengthModType.Adjacent:str += (int)attachment.strengthModifier;break;
                case StrengthModType.Multiply:str += (int)(attachment.strengthModifier * attachment.strengthMultiple);break;
                case StrengthModType.None:break;
                default: break;
            }
        }
        return str;
    }
    public void attachCard(Card c)
    {
        attachments.Add(c);
    }
    public bool hasAttachments()
    {
        return attachments.Count > 0;
    }

    public void clearAttachments(Deck d)
    {
        for (int i = 0; i < attachments.Count; i++)
        {
            Card c = attachments[i];
            if (CardModel.isPower(c.cardType))
            {
                d.getRowByType(RowEffected.PowerGraveyard).Add(c);
            }
            else if (CardModel.isUnit(c.cardType))
            {
                d.getRowByType(RowEffected.UnitGraveyard).Add(c);
            }
        }
        attachments.RemoveAll(delegate (Card c) { return true; });
    }

    public override string ToString()
    {
        return "Type: " + this.cardType + " Name: " + this.cardName + " card with strength: " + this.strength;
    }

    public override bool Equals(object c)
    {
        if (c.GetType() == typeof(Card))
        {
            return this.cardName.Equals(((Card)c).cardName);
        }
        return false;
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
    public void loadMaterial()
    {
        this.getMaterial().SetTexture("_Texture2D", cardModel.getSmallFront(index));
    }

    public void setCardType(CardType cardType)
    {
        this.cardType = cardType;
    }

    public CardType getCardType()
    {
        return this.cardType;
    }

    public CardModel getCardModel()
    {
        return cardModel;
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
    public void Destroy()
    {
        foreach (Transform child in transform)
        {
            foreach (Transform childC in transform)
            {
                Destroy(childC.gameObject);
            }
            Destroy(child.gameObject);
        }
        Destroy(this.gameObject);
        Destroy(this);
    }
}