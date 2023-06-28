using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int index;
    public string cardName;
    public CardType cardType;
    public int strength;
    public int playerCardDraw;
    public CardDrawType cardDrawType;
    public int playerCardDrawRemain;
    public int playerCardDestroy;
    public uint playerCardDestroyRemain;
    public DestroyType destroyType;
    public int playerCardReturn;
    public uint playerCardReturnRemain;
    public CardReturnType cardReturnType;
    public float strengthModifier;
    public StrengthModType strengthModType;
    public int graveyardCardDraw;
    public uint graveyardCardDrawRemain;
    public int enemyCardDraw;
    public uint enemyCardDrawRemain;
    public int enemyCardDestroy;
    public uint enemyCardDestroyRemain;
    public int enemyReveal;
    public float rowMultiple;
    public RowEffected rowEffected;
    public int setAside;
    public uint setAsideRemain;
    public RowEffected setAsideReturnRow;
    public SetAsideType setAsideType;
    public bool attach;
    public int strengthCondition;
    public int chooseN;
    public RowEffected chooseRow;
    public int chooseShowN;
    public int moveRemain;
    public Card moveCard;
    public RowEffected moveRow;
    private bool targetActive = false;
    public bool isBig = false;
    public float bigFac = 2;
    public int isSpecial;
    public bool weatherEffect = false;
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

    private GameObject cardModelGameObject;
    private CardModel cardModel;

    void Awake()
    {
        cardModelGameObject = GameObject.Find("CardModel");
        cardModel = cardModelGameObject.GetComponent<CardModel>();
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
        Debug.Log("big scale: " + this.cardName + "bigFac" + bigFac+ "scalex " + bigFac*baseScalex + " scaley " + bigFac*baseScalez);
        cardObj.transform.localScale = new Vector3(bigFac * baseScalex, 1, bigFac* baseScalez);
    }
    public void resetScale()
    {
        Debug.Log("resetting scale: " + this.cardName + "scalex " + baseScalex + " scaley " + baseScalez);
        isBig = false;
        Transform cardObj = this.transform.Find("Card 1");
        cardObj.transform.localScale = new Vector3(baseScalex, 1, baseScalez);
    }
    
    public bool ContainsMouse(Vector3 mousePos){
        mousePos.z = this.transform.position.z;
        return this.cardColider.bounds.Contains(mousePos);
    }


    public void setBaseScale(){
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
        this.resetSelectionCounts();
    }


    public int getIndex()
    {
        return this.index;
    }

    public void resetSelectionCounts(){
        
        this.playerCardDrawRemain = playerCardDraw;
        this.playerCardDestroyRemain = (uint)this.playerCardDestroy;
        this.playerCardReturnRemain = (uint)this.playerCardReturn;
        this.enemyCardDestroyRemain = (uint)this.enemyCardDestroy;
        this.enemyCardDrawRemain = (uint) this.enemyCardDraw;
        this.setAsideRemain = (uint)this.setAside;
        this.graveyardCardDrawRemain = (uint)this.graveyardCardDraw;
    }

    public bool doneMultiSelection(){
        return  this.playerCardDrawRemain == 0 && this.playerCardDestroyRemain == 0 
        && this.playerCardReturnRemain ==0 && this.enemyCardDestroyRemain == 0
        && this.setAsideRemain == 0 && this.chooseN ==0 && this.moveRemain ==0;
    }

    public void LogSelectionsRemaining(){
        Debug.Log("Remaining Selections: " + cardName);
        Debug.Log("playerCardDraw: " + this.playerCardDrawRemain);
        Debug.Log("playerCardDestroy: " + this.playerCardDestroyRemain);
        Debug.Log("playerCardReturn: " + this.playerCardReturnRemain);
        Debug.Log("enemyCardDestroy: " + this.enemyCardDestroyRemain);
        Debug.Log("chooseN: " + this.chooseN);
        Debug.Log("chooseShowN: " + this.chooseShowN);
        Debug.Log("move: " + this.moveRemain);
        Debug.Log("setAside: " + this.setAsideRemain);
    }

    public void setTargetActive(bool state)
    {
        Debug.Log("Setting target: " + state);
        Material material = getMaterial();
        material.SetInt("_Flash", state ? 1 : 0);
        this.targetActive = state;
    }
    public void setVisible(bool state)
    {
        Material material = getMaterial();
        material.SetInt("_Transparent", state ? 0 : 1);
    }
    public bool isTargetActive()
    {
        return this.targetActive;
    }

    public bool isPlayable(Deck deck)
    {
        List<Row> playerRows = deck.getRowsByType(RowEffected.PlayerPlayable);
        int playerRowsSum = 0;
        List<Row> enemyRows = deck.getRowsByType(RowEffected.EnemyPlayable);
        int enemyRowsSum = 0;
        RowEffected playerKingRow = deck.getKingRow(true);
        RowEffected enemyKingRow = deck.getKingRow(false);
        for(int i = 0; i < playerRows.Count; i++){
            playerRowsSum += playerRows[i].Count;
        }
        for(int i = 0; i < enemyRows.Count; i++){
            enemyRowsSum += enemyRows[i].Count;
        }
        if(this.destroyType ==DestroyType.Unit && this.playerCardDestroy + this.playerCardReturn > playerRowsSum){
            return false;
        }
        if(this.setAsideType == SetAsideType.Player && this.setAside > playerRowsSum){
            return false;
        }
        if(this.setAsideType == SetAsideType.Enemy && this.setAside > enemyRowsSum){
            return false;
        }
        if(this.setAsideType == SetAsideType.EnemyKing && enemyKingRow == RowEffected.None){
            return false;
        }
        if(this.setAsideType == SetAsideType.King && playerKingRow == RowEffected.None){
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

    public void attachCard(Card c){
        attachments.Add(c);
    }
    public bool hasAttachments(){
        return attachments.Count > 0;
    }

    public void clearAttachments(Deck d){
        for(int i = 0; i < attachments.Count; i ++){
            Card c = attachments[i];
            if(CardModel.isPower(c.cardType)){
                d.getRowByType(RowEffected.PowerGraveyard).Add(c);
            }
            else if(CardModel.isUnit(c.cardType)){
                d.getRowByType(RowEffected.UnitGraveyard).Add(c);
            }
        }
        attachments.RemoveAll(delegate(Card c) { return true;});
    }

    public override string ToString()
    {
        return "Type: " + this.cardType + " Name: " + this.cardName + " card with strength: " + this.strength;
    }

    public override bool Equals(object c){
        if(c.GetType() == typeof(Card)){
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
    public void Destroy(){
         foreach (Transform child in transform) {
            foreach (Transform childC in transform) {
                Destroy(childC.gameObject);
            }
            Destroy(child.gameObject);
        }
        Destroy(this.gameObject);
        Destroy(this);
    }
}