using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using System;
using System.IO;
using System.Reflection;

public class Card : MonoBehaviour
{
    public int index;
    public string cardName;

    public Texture2D tex;
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
    public bool strengthModRow;
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
    public bool clearWeather;
    public int setAside;
    public int setAsideRemain;
    public RowEffected setAsideReturnRow;
    public SetAsideType setAsideType;
    public bool attach;
    public int attachmentsRemaining;
    public int strengthCondition;
    public bool strengthConditionPassed;
    public int chooseN;
    public ChooseNAction chooseNAction;
    public RowEffected chooseRow;
    public int chooseShowN;
    public int chooseNRemain;
    public ChooseCardType chooseCardType;
    public bool chooseSkippable;
    public bool playInRow;
    public bool playNextRound;
    public bool isAltEffect;
    public bool canAutoPlayAltEffect;
    public string mainCardName;
    public string effectDescription;
    public bool isEffectSet = false;
    public bool protect;
    public RowEffected autoPlaceRow;
    public int moveRemain;
    public Card moveCard;
    public RowEffected moveRow;
    private bool targetActive = false;
    private bool visible = false;
    public bool isBig = false;
    public float bigFac = 2;
    public int isSpecial;
    public bool weatherEffect = false;
    public bool flipped = false;
    public bool beenRevealed;
    public RoundEndRemoveType roundEndRemoveType;
    public Vector3 baseLoc;

    public List<Card> attachments;

    public List<Card> altEffects;

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

    public static CardModel cardModel;

    public RowEffected playerPlayed;

    public TMP_Text strengthText;
    public TMP_Text effectDescriptionText;
    
    Transform effectDescriptionTextObj;
    private GameObject targetObj;
    public bool textureLoaded;

    public LayerMask defaultLayerMask;
    private static FileStream moveLogFile;
    private static StreamWriter sw;
    public RowEffected currentRow;

    public bool isClone = false;

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
        Transform strengthTextObj = cardObj.Find("Score");
        effectDescriptionTextObj = cardObj.Find("EffectDescription");
        targetObj = cardObj.Find("Target").gameObject;
        strengthText = strengthTextObj.GetComponent<TMP_Text>();
        effectDescriptionText = effectDescriptionTextObj.GetComponent<TMP_Text>();
        this.calculatedStrength = this.strength;
        updateStrengthText(this.strength);
        textureLoaded = false;
        defaultLayerMask = LayerMask.NameToLayer("Default");
    }


    public void scaleBig()
    {
        if (!isBig)
        {
            isBig = true;
            Transform cardObj = this.transform.Find("Card 1");
            bigFac = 4;

            this.setLayer("Big", false);
            Debug.Log("big scale: " + this.cardName + "bigFac" + bigFac + "scalex " + bigFac * baseScalex + " scaley " + bigFac * baseScalez);
            cardObj.transform.localScale = new Vector3(bigFac * baseScalex, 1, bigFac * baseScalez);

            this.transform.position = Game.activeDeck.areas.getCenterFrontBig();
        }
    }
    public void resetScale()
    {
        Debug.Log("resetting scale: " + this.cardName + "scalex " + baseScalex + " scaley " + baseScalez);
        isBig = false;
        Transform cardObj = this.transform.Find("Card 1");
        this.resetLayerToDefault();
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


    public void setEffectFromCardModel(int index)
    {
        this.cardName = CardModel.names[index];
        this.name = cardName;
        this.cardType = CardModel.cardTypes[index];
        this.strength = CardModel.strength[index];
        this.playerCardDraw = CardModel.playerCardDraw[index];
        this.cardDrawType = CardModel.cardDrawType[index];
        this.playerCardDestroy = CardModel.playerCardDestroy[index];
        this.destroyType = CardModel.destroyType[index];
        this.playerCardReturn = CardModel.playerCardReturn[index];
        this.cardReturnType = CardModel.cardReturnType[index];
        this.strengthModifier = CardModel.strengthModifier[index];
        this.strengthModRow = CardModel.strengthModRow[index];
        this.strengthModType = CardModel.strengthModType[index];
        this.graveyardCardDraw = CardModel.graveyardCardDraw[index];
        this.enemyCardDraw = CardModel.enemyCardDraw[index];
        this.enemyCardDestroy = CardModel.enemyCardDestroy[index];
        this.enemyReveal = CardModel.enemyReveal[index];
        this.rowMultiple = CardModel.rowMultiple[index];
        this.rowEffected = CardModel.rowEffected[index];
        this.clearWeather = CardModel.clearWeather[index];
        this.setAside = CardModel.setAside[index];
        this.setAsideType = CardModel.setAsideType[index];
        this.attach = CardModel.attach[index];
        this.strengthCondition = CardModel.strengthCondition[index];
        this.chooseN = CardModel.chooseN[index];
        this.chooseNAction = CardModel.chooseNAction[index];
        this.chooseRow = CardModel.chooseRow[index];
        this.chooseShowN = CardModel.chooseShowN[index];
        this.chooseCardType = CardModel.chooseCardType[index];
        this.chooseSkippable = CardModel.chooseSkippable[index];
        this.playInRow = CardModel.playInRow[index];
        this.playNextRound = CardModel.playNextRound[index];
        this.isAltEffect = CardModel.isAltEffect[index];
        this.canAutoPlayAltEffect = CardModel.canAutoPlayAltEffect[index];
        this.mainCardName = CardModel.mainCardName[index];
        this.effectDescription = CardModel.effectDescription[index];
        this.protect = CardModel.protect[index];
        this.autoPlaceRow = CardModel.autoPlaceRow[index];
        this.strengthText.colorGradient = ColorGradientType.getTextGradient(this.cardType);
        this.strengthText.outlineColor = ColorGradientType.getTextOutline(this.cardType);
        this.strengthText.fontSharedMaterial.SetColor(ShaderUtilities.ID_Outline2Color, ColorGradientType.getTextOutline2(this.cardType));
        this.effectDescriptionText.colorGradient = ColorGradientType.getTextGradient(this.cardType);
        this.effectDescriptionText.outlineColor = ColorGradientType.getTextOutline(this.cardType);
        this.effectDescriptionText.fontSharedMaterial.SetColor(ShaderUtilities.ID_Outline2Color, ColorGradientType.getTextOutline2(this.cardType));
        updateStrengthText(this.strength);
        this.resetSelectionCounts();
    }

    public void initCard(int index)
    {
        this.index = index;
        if (index > CardModel.names.Count)
        {
            Debug.LogError("Out of Bounds! expected max index: " + (cardModel.numCardEffects - 1) + " got: " + index + " num card names:" + CardModel.names.Count);
        }
        setEffectFromCardModel(index);
        if (this.isAltEffect)
        {
            this.strengthText.text = "";
            this.setVisible(false);
            this.effectDescriptionText.text = this.effectDescription;
            this.effectDescriptionText.alpha = 0;
            this.cardColider = effectDescriptionTextObj.GetComponent<BoxCollider2D>();
            this.targetObj.transform.localScale = new Vector3(14.2f, 3.81f, 1);
            Material targetMaterial = this.getTargetMaterial();
            targetMaterial.SetFloat("_RectWidth", 0.79f);
            targetMaterial.SetFloat("_RectHeight", 0.67f);
        }
        else
        {
            this.effectDescriptionText.text = "";
        }
        this.loadCardBackMaterial();
    }


    public int getIndex()
    {
        return this.index;
    }

    public void setEffect(Card effect)
    {
        this.setEffectFromCardModel(effect.index);
        this.isEffectSet = true;

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
        this.isEffectSet = this.isAltEffect;
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
        if (Game.state == State.CHOOSE_N)
        {
            return this.chooseSkippable;
        }
        return this.playerCardDestroyRemain <= 0 &&
        (this.setAsideRemain <= 0 || (this.setAsideType != SetAsideType.King && this.setAsideType != SetAsideType.Player))
        && this.moveRemain <= 0
        && (this.attachmentsRemaining <= 0 || strengthModType == StrengthModType.Adjacent || strengthModType == StrengthModType.AddMultiple);
    }

    public bool doneMultiSelection(RowEffected player)
    {
        return (this.playerCardDrawRemain <= 0 || this.cardDrawType != CardDrawType.Either) && this.playerCardDestroyRemain <= 0
        && (this.playerCardReturnRemain <= 0 || this.cardReturnType == CardReturnType.SwitchSidesRoundEnd || this.cardReturnType == CardReturnType.LastPlayedCard)
        && (this.enemyCardDestroyRemain <= 0 || this.destroyType == DestroyType.RoundEnd)
        && this.setAsideRemain <= 0 && this.moveRemain <= 0 && this.attachmentsRemaining <= 0 && this.chooseNRemain <= 0;
    }

    public void LogSelectionsRemaining()
    {
        /*
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
        Debug.Log("strengthConditionPassed: " + this.strengthConditionPassed);*/
    }

    public bool shouldScoreThisRound()
    {
        return !this.playNextRound || this.roundEndRemoveType == RoundEndRemoveType.Remove;
    }

    public void setTargetActive(bool state)
    {
        //Debug.Log("Setting Target Active : " + this);
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
        if (!this.isAltEffect)
        {
            Material material = getCardFrontMaterial();
            material.SetInt("_Transparent", state ? 0 : 1);
            updateStrengthText(this.calculatedStrength);
        }
        else
        {
            Material material = getCardFrontMaterial();
            material.SetInt("_Transparent", 1);
            this.effectDescriptionText.alpha = state ? 1 : 0;
        }
        this.visible = state;
    }

    public bool isVisible()
    {
        return this.visible;
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
        List<Row> returnRows = deck.getRowsByType(CardModel.getRowFromSide(player, this.rowEffected));
        int returnRowsSum = 0;
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
        for (int i = 0; i < returnRows.Count; i++)
        {
            if (!returnRows[i].hasType(CardModel.getRowFromSide(player, RowEffected.PlayerPlayable)))
                returnRowsSum += returnRows[i].Count;
        }

        Debug.Log("Player Rows Sum: " + playerRowsSum);
        Debug.Log("Enemy Rows Sum: " + enemyRowsSum);
        Debug.Log("Return Rows Sum: " + returnRowsSum);
        if (this.destroyType == DestroyType.Unit && (this.playerCardDestroy > playerRowsSum || (((playerRowsSum - this.playerCardDestroy) + returnRowsSum) - this.playerCardReturn < 0)))
        {
            Debug.Log("Cannot Play Cond 1! : Destroy Type Unit : " + (this.playerCardDestroy + this.playerCardReturn) + " > " + playerRowsSum);
            return false;
        }
        if (this.destroyType == DestroyType.MaxAll && playerRowsSum == 0)
        {
            Debug.Log("Cannot Play Cond 2! : Destroy Type Max : " + playerRowsSum + " == 0");
            return false;
        }
        if (this.setAsideType == SetAsideType.Player && this.setAside > playerRowsSum)
        {
            Debug.Log("Cannot Play Cond 3! : Set Aside Type Player : " + this.setAside + " > " + playerRowsSum);
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
    private void applyOperation(StrengthModType operation, Card c, Row row)
    {
        if (strength > 0)
        {
            switch (operation)
            {
                case StrengthModType.Set: calculatedStrength = (int)c.strengthModifier; break;
                case StrengthModType.Add: calculatedStrength += (int)c.strengthModifier; break;
                case StrengthModType.Adjacent: calculatedStrength += (int)c.strengthModifier; break;
                case StrengthModType.AddMultiple: calculatedStrength += (int)(c.strengthModifier * c.strengthMultiple); break;
                case StrengthModType.AddRowCount: calculatedStrength += row.Count; break;
                case StrengthModType.Subtract: calculatedStrength -= (int)c.strengthModifier; break;
                case StrengthModType.Multiply: calculatedStrength = (int)Mathf.Max(calculatedStrength * c.strengthModifier, 1); break;
                case StrengthModType.None: break;
                default: break;
            }
            if (calculatedStrength < 1 && this.strength > 0)
            {
                calculatedStrength = 1;
            }
            updateStrengthText(calculatedStrength);
        }
    }
    public int calculateBaseStrength(StrengthModType operation, Row row)
    {
        if (operation == this.strengthModType && this.strengthModRow)
        {
            foreach (Card c in row)
            {
                c.applyOperation(operation, this, row);
            }
        }
        foreach (Card attachment in attachments)
        {
            if (operation == attachment.strengthModType)
            {
                applyOperation(operation, attachment, row);
                if (attachment.strengthModRow)
                {
                    foreach (Card c in row)
                    {
                        if (!c.Equals(this))
                        {
                            c.applyOperation(operation, attachment, row);
                        }
                    }
                }
                Debug.Log("Op: " + operation + " " + this.calculatedStrength);
            }
        }
        return calculatedStrength;
    }
    public void updateStrengthText(int strength)
    {
        if (this.isVisible() && strength > 0)
        {
            this.strengthText.text = strength.ToString();
        }
        else
        {
            this.strengthText.text = "";
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
        return "Type: " + this.cardType + " Name: " + this.cardName + " card with strength: " + this.strength + " clone: " + isClone;
    }

    public override bool Equals(object c)
    {
        if (c == null)
        {
            return false;
        }
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
            this.targetMaterial = meshRend.materials[0];
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
    public IEnumerator loadCardFrontAsync()
    {

        this.textureLoaded = false;
        ResourceRequest resourceRequest = Resources.LoadAsync<Texture2D>("Images/" + CardModel.smallFronts[index]);
        yield return resourceRequest;
        tex = resourceRequest.asset as Texture2D;
        while (!resourceRequest.isDone)
        {
            yield return null;
        }
        if (resourceRequest.isDone)
        {
            this.textureLoaded = true;
            this.getCardFrontMaterial().SetTexture("_Texture2D", tex);
        }
    }
    public void loadCardFront()
    {
        this.getCardFrontMaterial().SetTexture("_Texture2D", cardModel.getSmallFront(index));
        this.textureLoaded = true;
    }
    public void loadCardBackMaterial()
    {
        Material backMaterial = this.getCardBackMaterial();
        backMaterial.SetTexture("_Texture2D", cardModel.getCardBack(index));
        backMaterial.SetInt("_Flash", 0);
    }

    public void setLayer(string layerName, bool defaultLayer)
    {
        LayerMask l = LayerMask.NameToLayer(layerName);
        foreach (Transform g in transform.GetComponentsInChildren<Transform>())
        {
            g.gameObject.layer = l;
        }
        if (defaultLayer)
        {
            defaultLayerMask = l;
        }
        gameObject.layer = l;
    }

    public void resetLayerToDefault()
    {
        LayerMask l = defaultLayerMask;
        foreach (Transform g in transform.GetComponentsInChildren<Transform>())
        {
            g.gameObject.layer = l;
        }
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