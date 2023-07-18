using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public float strengthMultiple;
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
    public int chooseNRemain;
    public ChooseCardType chooseCardType;
    public bool playInRow;
    public int moveRemain;
    public Card moveCard;
    public RowEffected moveRow;
    private bool targetActive = false;
    public bool isBig = false;
    public float bigFac = 2;
    public int isSpecial;
    public bool weatherEffect = false;
    public bool flipped = false;
    public bool beenRevealed;
    public RoundEndRemoveType roundEndRemoveType;
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
    private Material backMaterial;
    private Material targetMaterial;

    private static GameObject cardModelGameObject;
    private static CardModel cardModel;

    public RowEffected playerPlayed;

    TMP_Text scoreText;
    private GameObject targetObj;

    void Awake()
    {
        if (cardModelGameObject == null)
        {
            cardModelGameObject = GameObject.Instantiate(Resources.Load("Prefabs/CardModel") as GameObject, transform.position, transform.rotation);
            cardModel = cardModelGameObject.GetComponent<CardModel>();
            cardModel.readTextFile();
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        cardColider = GetComponent<BoxCollider2D>();
        if (scaleHeight == 0f || scaleWidth == 0f)
        {
            setBaseScale();
        }
        baseLoc = this.transform.position;
        Transform cardObj = this.transform.Find("Card 1");
        Transform textObj = cardObj.Find("Score");
        targetObj = cardObj.Find("Target").gameObject;
        scoreText = textObj.GetComponent<TMP_Text>();
        updateStrengthText(this.strength);
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
            Vector3 screenvec = Camera.main.WorldToScreenPoint(transform.position);
            if (mousePos.x > transform.position.x - (cardColider.bounds.extents.x) && mousePos.x < transform.position.x + (cardColider.bounds.extents.x))
            {
                if (mousePos.y > transform.position.y - (cardColider.bounds.extents.y) && mousePos.y < transform.position.y + (cardColider.bounds.extents.y))
                {
                    return true;
                }
            }
            return false;
        }
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
        this.chooseCardType = cardModel.chooseCardType[index];
        this.playInRow = cardModel.playInRow[index];
        this.scoreText.colorGradient = ColorGradientType.getTextGradient(this.cardType);
        this.scoreText.outlineColor = ColorGradientType.getTextOutline(this.cardType);
        this.scoreText.fontSharedMaterial.SetColor(ShaderUtilities.ID_Outline2Color, ColorGradientType.getTextOutline2(this.cardType));
        updateStrengthText(this.strength);
        this.resetSelectionCounts();
        this.loadCardBackMaterial();
    }


    public int getIndex()
    {
        return this.index;
    }

    public void resetSelectionCounts()
    {
        this.roundEndRemoveType = RoundEndRemoveType.Remove;
        this.playerCardDrawRemain = playerCardDraw;
        this.playerCardDestroyRemain = this.playerCardDestroy;
        this.playerCardReturnRemain = this.playerCardReturn;
        this.enemyCardDestroyRemain = this.enemyCardDestroy;
        this.enemyCardDrawRemain = this.enemyCardDraw;
        this.setAsideRemain = this.setAside;
        this.graveyardCardDrawRemain = this.graveyardCardDraw;
        this.chooseNRemain = this.chooseN;
        if (attach && strengthModType == StrengthModType.Adjacent)
        {
            this.attachmentsRemaining = 2;
        }
        else if (attach)
        {
            this.attachmentsRemaining = 1;
        }
        this.strengthConditionPassed = false;
        this.strengthMultiple = 0;

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

    public void zeroSkipSelectionCounts()
    {
        this.chooseNRemain = 0;
        this.playerCardDrawRemain = 0;
        this.playerCardReturnRemain = 0;
        this.setAsideRemain = 0;
        this.graveyardCardDrawRemain = 0;
        if (strengthModType == StrengthModType.Adjacent)
        {
            this.attachmentsRemaining = 0;
        }
    }

    public bool canSkip()
    {
        return this.playerCardDestroyRemain <= 0 &&
        (this.setAsideRemain <= 0 || (this.setAsideType != SetAsideType.King && this.setAsideType != SetAsideType.Player))
        && this.moveRemain <= 0
        && (this.attachmentsRemaining <= 0 || strengthModType == StrengthModType.Adjacent || strengthModType == StrengthModType.Multiply);
    }

    public bool doneMultiSelection(RowEffected player)
    {
        return (this.playerCardDrawRemain <= 0 || this.cardDrawType != CardDrawType.Either) && this.playerCardDestroyRemain <= 0
        && (this.playerCardReturnRemain <= 0 || this.cardReturnType == CardReturnType.SwitchSidesRoundEnd)
        && (this.enemyCardDestroyRemain <= 0 || this.destroyType == DestroyType.RoundEnd)
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
        Debug.Log("chooseNRemain: " + this.chooseNRemain);
        Debug.Log("chooseShowN: " + this.chooseShowN);
        Debug.Log("move: " + this.moveRemain);
        Debug.Log("setAside: " + this.setAsideRemain);
        Debug.Log("reveal: " + this.enemyReveal);
        Debug.Log("attachmentsRemaining: " + this.attachmentsRemaining);
        Debug.Log("strengthConditionPassed: " + this.strengthConditionPassed);
    }

    public void setTargetActive(bool state)
    {
        if (Game.player == RowEffected.Player)
        {
            Material material = getTargetMaterial();
            material.SetInt("_Transparent", state ? 0 : 1);
        }
        this.targetActive = state;
        updateStrengthText(this.calculatedStrength);
    }
    public void setVisible(bool state)
    {
        Material material = getCardFrontMaterial();
        material.SetInt("_Transparent", state ? 0 : 1);
        updateStrengthText(this.calculatedStrength);
    }

    public bool isVisible()
    {
        Material material = getCardFrontMaterial();
        return material.GetInt("_Transparent") == 0 ? true : false;
    }
    public bool isTargetActive()
    {
        return this.targetActive;
    }

    public bool isPlayable(RowEffected player)
    {
        Deck deck = Game.activeDeck;
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
        if (this.destroyType == DestroyType.Unit && (this.playerCardDestroy > playerRowsSum || (this.playerCardReturn > 0 && playerRowsSum - this.playerCardDestroy <= 0)))
        {
            Debug.Log("Cannot Play Cond 1! : Destroy Type Unit : " + (this.playerCardDestroy + this.playerCardReturn) + " > " + playerRowsSum);
            return false;
        }
        if (this.setAsideType == SetAsideType.Player && this.setAside > playerRowsSum)
        {
            Debug.Log("Cannot Play Cond 2! : Set Aside Type Player : " + this.setAside + " > " + playerRowsSum);
            return false;
        }
        if (this.setAsideType == SetAsideType.EnemyKing && enemyKingRow == RowEffected.None)
        {
            Debug.Log("Cannot Play Cond 4! : Set Aside Type EnemyKing :  enemyKingRow is None! ");
            return false;
        }
        if (this.setAsideType == SetAsideType.King && playerKingRow == RowEffected.None)
        {
            Debug.Log("Cannot Play Cond 5! : Set Aside Type y+King :  playerKingRow  is None! ");
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
    public int calculateBaseStrength()
    {
        int calculatedStrength = strength;
        foreach (Card attachment in attachments)
        {
            switch (attachment.strengthModType)
            {
                case StrengthModType.Set: calculatedStrength = (int)attachment.strengthModifier; break;
                default: break;
            }
        }
        foreach (Card attachment in attachments)
        {
            switch (attachment.strengthModType)
            {
                case StrengthModType.Add: calculatedStrength += (int)attachment.strengthModifier; break;
                case StrengthModType.Adjacent: calculatedStrength += (int)attachment.strengthModifier; break;
                case StrengthModType.Multiply: calculatedStrength += (int)(attachment.strengthModifier * attachment.strengthMultiple); break;
                case StrengthModType.None: break;
                default: break;
            }
        }
        updateStrengthText(calculatedStrength);
        return calculatedStrength;
    }
    public void updateStrengthText(int strength)
    {
        if (this.isVisible() && strength > 0)
        {
            this.scoreText.text = strength.ToString();
        }
        else
        {
            this.scoreText.text = "";
        }
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

    private Material getCardBackMaterial()
    {
        if (this.backMaterial == null)
        {
            GameObject child = this.transform.GetChild(0).gameObject;
            GameObject childOfChild = child.transform.GetChild(0).gameObject;
            MeshRenderer meshRend = childOfChild.GetComponent<MeshRenderer>();
            this.backMaterial = meshRend.materials[0];
        }
        return this.backMaterial;
    }

    private Material getCardFrontMaterial()
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
    private Material getTargetMaterial()
    {
        if (this.targetMaterial == null)
        {
            GameObject child = this.transform.GetChild(0).gameObject;
            GameObject childOfChild = child.transform.Find("Target").gameObject;
            MeshRenderer meshRend = childOfChild.GetComponent<MeshRenderer>();
            this.targetMaterial = meshRend.materials[2];
        }
        return this.targetMaterial;
    }
    private GameObject getTargetObject()
    {
        if (this.targetObj == null)
        {
            GameObject child = this.transform.GetChild(0).gameObject;
            GameObject childOfChild = child.transform.Find("Target").gameObject;
            this.targetObj = childOfChild;
        }
        return this.targetObj;
    }
    public void loadCardFrontMaterial()
    {
        this.getCardFrontMaterial().SetTexture("_Texture2D", cardModel.getSmallFront(index));
    }
    public void loadCardBackMaterial()
    {
        Material backMaterial = this.getCardBackMaterial();
        backMaterial.SetTexture("_Texture2D", cardModel.getCardBack(index));
        backMaterial.SetInt("_Flash", 0);
    }

    public void setLayer(string layerName, string blurLayerName)
    {
        LayerMask l = LayerMask.NameToLayer(layerName);
        LayerMask bl = LayerMask.NameToLayer(blurLayerName);
        foreach (Transform g in transform.GetComponentsInChildren<Transform>())
        {
            g.gameObject.layer = l;
        }
        getTargetObject().layer = bl;
        gameObject.layer = l;
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