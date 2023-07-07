using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

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
    public bool wide;
    public bool flipped;

    public  bool isButton;

    public int score;

    public int chooseNRemain;

    public bool cardTargetsActivated = false;

    public Row(bool isPlayer, bool isScoring, bool wide, bool flipped, RowEffected uniqueType, List<RowEffected> rowType, System.Func<Vector3> centerMethod)
    {
        this.isPlayer = isPlayer;
        this.isScoring = isScoring;
        this.name = uniqueType.ToString();
        this.uniqueType = uniqueType;
        this.rowType = rowType;
        this.centerMethod = centerMethod;
        this.wide = wide;
        this.flipped = flipped;
        this.isButton = false;
    }

    public Row(RowEffected uniqueType, string ButtonText, System.Func<Vector3> centerMethod)
    {
        this.isPlayer = false;
        this.isScoring = false;
        this.isButton = true;
        this.name = uniqueType.ToString();
        this.uniqueType = uniqueType;
        this.rowType = new List<RowEffected>(){uniqueType};
        this.centerMethod = centerMethod;
        this.wide = false;
        this.flipped = false;
    }

    public void setVisibile(bool state)
    {
        foreach (Card c in this)
        {
            c.setVisible(state);
        }
        if(isButton){
            
        }

    }
    public bool isVisible(){
        if(this.Count == 0){
            return false;
        }
        foreach (Card c in this)
        {
            if(!c.isVisible()){
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
            if (card.rowMultiple != 0){
                rowMultiple = rowMultiple * card.rowMultiple;
                if(card.rowMultiple < 1){
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
        foreach (Card card in this){
            int strength = card.calculateBaseStrength();
            card.calculatedStrength = strength;
            if( strength <= 3 && strength > 0){
                strengthGroupingList[strength-1].Add(card);
            }
        }
        foreach (List<Card> strengthGrouping in strengthGroupingList){
            int numAdjacent = strengthGrouping.Count - strengthGrouping.Count % 2;
            for(int i =0; i < numAdjacent; i ++){
                Card card = strengthGrouping[i];
                card.calculatedStrength = card.calculatedStrength *2;
            }
        }
        foreach (Card card in this)
        {
            card.calculatedStrength =  (int)Mathf.Floor( ((float)card.calculatedStrength) * rowMultiple);
            if(card.calculatedStrength < 1 && card.strength > 0){
                card.calculatedStrength = 1;
            }
        }
        foreach(Card card in this){
            score += card.calculatedStrength;
        }
        this.score = (int)score;
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



    public void setActivateRowCardTargets(bool state, bool individualCards)
    {
        if (individualCards)
        {
            for (int i = 0; i < this.Count; i++)
            {
                this[i].setTargetActive(state);
            }
            cardTargetsActivated = state;
        }
        else
        {
            this.target.setFlashing(state);
        }
    }
    public void centerRow()
    {
        this.center = centerMethod.Invoke();
    }
    public void setTarget(Target target)
    {
        this.target = target;
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
}
