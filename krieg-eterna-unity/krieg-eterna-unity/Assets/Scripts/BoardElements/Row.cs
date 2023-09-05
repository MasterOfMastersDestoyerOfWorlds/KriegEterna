using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Row : List<Card>
{
    public bool isPlayer;
    public bool isScoring;
    public Target target;
    public Vector3 center;
    public string name;
    public RowEffected uniqueType;
    public List<RowEffected> rowType;
    public System.Func<Vector3> centerMethod;
    public System.Func<Vector3> scoreDisplayCenterMethod;
    public bool wide;
    public bool flipped;

    public bool isButton;
    public string buttonText;
    public System.Action buttonAction;
    public System.Func<bool> canButton;

    public int score;

    public GameObject scoreDisplayObject;
    public TMP_Text scoreDisplay;

    public int chooseNRemain;


    private static GameObject targetGameObject;
    private static Target baseTarget;

    bool cardVisibility;



    public bool cardTargetsActivated = false;

    public Row(bool isPlayer, bool wide, bool flipped, bool visibility, RowEffected uniqueType, List<RowEffected> rowType, System.Func<Vector3> centerMethod)
    {
        this.isPlayer = isPlayer;
        this.isScoring = false;
        this.name = uniqueType.ToString();
        this.uniqueType = uniqueType;
        this.rowType = rowType;
        this.centerMethod = centerMethod;
        this.wide = wide;
        this.flipped = flipped;
        this.isButton = false;
        this.cardVisibility = visibility;
        setupTarget();
    }
    public Row(bool isPlayer, bool wide, bool flipped, bool visibility, RowEffected uniqueType, List<RowEffected> rowType, System.Func<Vector3> centerMethod, System.Func<Vector3> scoreDisplayCenterMethod)
    {
        this.isPlayer = isPlayer;
        this.isScoring = true;
        this.name = uniqueType.ToString();
        this.uniqueType = uniqueType;
        this.rowType = rowType;
        this.centerMethod = centerMethod;
        this.scoreDisplayCenterMethod = scoreDisplayCenterMethod;
        this.wide = wide;
        this.flipped = flipped;
        this.cardVisibility = visibility;
        this.isButton = false;
        scoreDisplayObject = GameObject.Instantiate(Resources.Load("Prefabs/Score") as GameObject, scoreDisplayCenterMethod.Invoke(), new Quaternion(0f, 0f, 0f, 0f));
        scoreDisplay = scoreDisplayObject.GetComponent<TMP_Text>();
        setupTarget();
    }

    public Row(bool visible, RowEffected uniqueType, string buttonText, System.Func<Vector3> centerMethod, System.Action buttonAction, System.Func<bool> canButton)
    {
        this.isPlayer = false;
        this.isScoring = false;
        this.isButton = true;
        this.buttonText = buttonText;
        this.buttonAction = buttonAction;
        this.canButton = canButton;
        this.name = uniqueType.ToString();
        this.uniqueType = uniqueType;
        this.rowType = new List<RowEffected>() { uniqueType, RowEffected.Button };
        this.centerMethod = centerMethod;
        this.wide = false;
        this.flipped = false;
        setupTarget();
        target.setLayer("UI");
        setVisibile(visible);
    }

    public new void Add(Card card)
    {
        if (this.Contains(card))
        {
            Debug.LogError("Row: " + this + " Already Contains Card! " + card + " iscurrentClone? " + card.isClone + " isOtherClone? " + this.Find((x) => x.Equals(card)).isClone);
        }
        else
        {

            if (!this.hasType(RowEffected.Deck) && !this.hasType(RowEffected.ChooseN) && !this.hasType(RowEffected.Graveyard))
            {
                MoveLogger.logRowAdd(card, uniqueType, Game.player);
            }
            card.currentRow = this.uniqueType;
            base.Add(card);
            card.setVisible(cardVisibility);
        }
    }
    public new bool Remove(Card card)
    {
        if (!this.hasType(RowEffected.Deck) && !this.hasType(RowEffected.ChooseN) && !this.hasType(RowEffected.Graveyard))
        {
            MoveLogger.logRowRemove(card, uniqueType, Game.player);
        }
        return base.Remove(card);
    }

    public void setVisibile(bool state)
    {
        Debug.Log("Setting Row Visible: " + state + " " + uniqueType);
        foreach (Card c in this)
        {
            c.setVisible(state);
        }
        if (isButton)
        {
            if (state)
            {
                target.setText(buttonText);
                target.setButtonVisible();
            }
            else
            {
                target.setText("");
            }
        }

    }
    public bool isVisible()
    {

        if (isButton)
        {
            return target.isVisible();
        }

        if (this.Count == 0)
        {
            return false;
        }
        foreach (Card c in this)
        {
            if (!c.isVisible())
            {
                return false;
            }
        }
        return true;
    }

    public Card getCardByName(string cardName)
    {
        foreach (Card c in this)
        {
            if (c.cardName.Equals(cardName))
            {
                return c;
            }
        }
        return null;
    }

    public bool ContainsCard(string cardName)
    {
        foreach (Card c in this)
        {
            if (c.cardName.Equals(cardName))
            {
                return true;
            }
        }
        return false;
    }

    public bool ContainsIncludeAttachments(Card card)
    {
        foreach (Card c in this)
        {
            if (c.Equals(card) || c.attachments.Contains(card))
            {
                return true;
            }
        }
        return false;
    }

    public int maxStrength(Deck deck, RowEffected player)
    {
        this.scoreRow(deck, player);
        int max = 0;
        foreach (Card c in this)
        {
            if (c.calculatedStrength > max)
            {
                max = c.calculatedStrength;
            }
        }
        return max;
    }

    public Card maxStrengthCard()
    {
        int max = 0;
        Card maxCard = null;
        foreach (Card c in this)
        {
            if (c.strength > max)
            {
                max = c.strength;
                maxCard = c;
            }
        }
        return maxCard;
    }

    public float scoreRow(Deck deck, RowEffected player)
    {
        float score = 0f;
        float rowMultiple = 1f;
        RowEffected enemy = CardModel.getEnemy(player);

        bool halfFlag = false;
        foreach (Card card in this)
        {
            if (card.rowMultiple != 0 && card.shouldScoreThisRound())
            {
                rowMultiple = rowMultiple * card.rowMultiple;
                if (card.rowMultiple < 1)
                {
                    halfFlag = true;
                }
            }
        }
        RowEffected playerKingRowType = deck.getKingRow(player);
        RowEffected enemyKingRowType = deck.getKingRow(enemy);
        if (this.hasType(RowEffected.All))
        {
            Row playerKingRow = deck.getRowByType(playerKingRowType);
            if (playerKingRowType != RowEffected.None && !halfFlag)
            {
                RowEffected fullRow = CardModel.getFullRow(this.uniqueType);

                rowMultiple = rowMultiple * (playerKingRow.hasType(fullRow) ? 2 : 1);
                Card playerKing = playerKingRow[0];
                if (playerKing.rowMultiple > 0 && this.hasType(playerKing.rowEffected))
                {
                    rowMultiple = rowMultiple * playerKing.rowMultiple;
                    halfFlag = true;
                }
            }
            if (enemyKingRowType != RowEffected.None && !halfFlag)
            {
                Card enemyKing = deck.getRowByType(enemyKingRowType)[0];
                if (enemyKing.rowMultiple > 0 && this.hasType(enemyKing.rowEffected))
                {
                    rowMultiple = rowMultiple * enemyKing.rowMultiple;
                    halfFlag = true;
                }
            }
        }
        List<List<Card>> strengthGroupingList = new List<List<Card>>(){
            new List<Card>(),
            new List<Card>(),
            new List<Card>(),
        };
        foreach (Card card in this)
        {
            card.calculatedStrength = card.strength;
            card.calculateBaseStrength(StrengthModType.Set, this);
            card.calculateBaseStrength(StrengthModType.Add, this);
            card.calculateBaseStrength(StrengthModType.Subtract, this);
            card.calculateBaseStrength(StrengthModType.AddRowCount, this);
            card.calculateBaseStrength(StrengthModType.AddMultiple, this);
            card.calculateBaseStrength(StrengthModType.Adjacent, this);
            card.calculateBaseStrength(StrengthModType.Multiply, this);
            card.calculateBaseStrength(StrengthModType.RoundAdvance, this);
        }
        foreach (Card card in this)
        {
            if (card.calculatedStrength <= 3 && card.calculatedStrength > 1)
            {
                strengthGroupingList[card.calculatedStrength - 1].Add(card);
            }
        }
        foreach (List<Card> strengthGrouping in strengthGroupingList)
        {
            int numAdjacent = strengthGrouping.Count - strengthGrouping.Count % 2;
            for (int i = 0; i < numAdjacent; i++)
            {
                Card card = strengthGrouping[i];
                card.calculatedStrength = card.calculatedStrength * 2;
            }
        }
        foreach (Card card in this)
        {
            card.calculatedStrength = (int)Mathf.Floor(((float)card.calculatedStrength) * rowMultiple);
            if (card.calculatedStrength < 1 && card.strength > 0)
            {
                card.calculatedStrength = 1;
            }
            card.updateStrengthText(card.calculatedStrength);
        }
        foreach (Card card in this)
        {
            score += card.calculatedStrength;
        }
        this.score = (int)score;
        if (scoreDisplay != null)
        {
            scoreDisplay.text = score + "";
        }
        return score;
    }

    public RowEffected getFullRowType()
    {
        if (rowType.Contains(RowEffected.MeleeFull))
        {
            return RowEffected.MeleeFull;
        }
        if (rowType.Contains(RowEffected.RangedFull))
        {
            return RowEffected.RangedFull;
        }
        if (rowType.Contains(RowEffected.SiegeFull))
        {
            return RowEffected.SiegeFull;
        }
        return RowEffected.None;
    }
    public RowEffected getPlayer()
    {
        if (rowType.Contains(RowEffected.Player))
        {
            return RowEffected.Player;
        }
        if (rowType.Contains(RowEffected.Enemy))
        {
            return RowEffected.Enemy;
        }
        return RowEffected.None;
    }



    public void setActivateRowCardTargets(bool state, bool individualCards, bool unitsOnly)
    {
        if (individualCards)
        {
            for (int i = 0; i < this.Count; i++)
            {
                Card c = this[i];
                if (!unitsOnly || CardModel.isUnitOrSpy(c.cardType))
                {
                    c.setTargetActive(state);
                }
            }
            cardTargetsActivated = state;
        }
        else
        {
            this.target.setTargetActive(state);
        }
    }
    public void centerRow()
    {
        this.center = centerMethod.Invoke();
    }
    public void setupTarget()
    {
        if (baseTarget == null)
        {
            targetGameObject = GameObject.Instantiate(Resources.Load("Prefabs/Target") as GameObject, new Vector3(0f, 0f, 0f), new Quaternion(0, 0, 0, 0));
            baseTarget = targetGameObject.GetComponent<Target>();
        }

        this.center = centerMethod.Invoke();
        Target clone = GameObject.Instantiate(baseTarget) as Target;
        clone.setNotFlashing();
        clone.setTargetActive(false);
        clone.tag = "CloneTarget";
        if (this.wide)
        {
            clone.scale(Mathf.Max(this.Count + 1, 8), 1);
        }
        else
        {
            clone.scale(1, 1);
        }
        this.target = clone;
        target.transform.position = new Vector3(center.x, center.y, -1f);
        target.setBaseLoc();

        Debug.Log("Setting Target: " + this.target + " On: " + this.name);
    }
    public Bounds getTargetBounds()
    {
        return this.target.getBounds();
    }
    public override string ToString()
    {
        string str = this.name + " [ ";
        foreach (Card c in this)
        {
            str += " " + c.cardName + " ";
        }
        str += " ]";
        return str;
    }
    public bool hasType(RowEffected type)
    {
        return this.rowType.Contains(type);
    }


    public bool hasAllTypes(List<RowEffected> types)
    {
        bool flag = true;
        foreach (RowEffected type in types)
        {
            flag = flag & rowType.Contains(type);
        }
        return flag;
    }
    public Card getKing()
    {
        foreach (Card c in this)
        {
            if (c.cardType == CardType.King)
            {
                return c;
            }
        }
        return null;
    }
    public bool isTypeUnique(RowEffected type)
    {
        return this.uniqueType == type;
    }

    internal bool isButtonClickable(RowEffected player)
    {
        if (player == RowEffected.Player)
        {
            return this.isButton && this.isVisible();
        }
        else
        {
            return this.canButton.Invoke();
        }
    }
}
