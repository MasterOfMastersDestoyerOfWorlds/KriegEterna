using System.Collections;
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

    public int chooseNRemain;

    public bool cardTargetsActivated = false;

    public Row(bool isPlayer, bool isScoring, bool wide, RowEffected uniqueType, List<RowEffected> rowType, System.Func<Vector3> centerMethod)
    {
        this.isPlayer = isPlayer;
        this.isScoring = isScoring;
        this.name = uniqueType.ToString();
        this.uniqueType = uniqueType;
        this.rowType = rowType;
        this.centerMethod = centerMethod;
        this.wide = wide;
    }

    public void setVisibile(bool state)
    {
        foreach (Card c in this)
        {
            c.setVisible(state);
        }

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

    public int maxStrength()
    {
        int max = 0;
        foreach (Card c in this)
        {
            if (c.strength > max)
            {
                max = c.strength;
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

    public float scoreRow(Deck deck)
    {
        float score = 0f;
        float rowMultiple = 1f;
        foreach (Card card in this)
        {
            if (card.rowMultiple != 0)
                rowMultiple = rowMultiple * card.rowMultiple;
        }
        Row kingRow = deck.getKingRowFromPlayRow(this);
        rowMultiple = rowMultiple * (kingRow.Count > 0 ? kingRow[0].rowMultiple : 1);
        foreach (Card card in this)
        {
            score += card.strength * rowMultiple;
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
        foreach(Card c in this){
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
        foreach(RowEffected type in types){
            flag = flag & rowType.Contains(type);
        }
        return flag;
    }
    public Card getKing(){
        foreach(Card c in this){
            if(c.cardType == CardType.King){
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
