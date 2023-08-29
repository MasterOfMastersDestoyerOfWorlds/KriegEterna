using System.Collections.Generic;
using UnityEngine;
using System;
public class SpyController : EffectControllerInterface
{
    public void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
            targetRow.Add(c);
            Debug.Log("Added Spy to Row: " + targetRow);
    }
    public bool PlayCondition(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        return Game.state != State.MULTISTEP && c.cardType == CardType.Spy && c.rowEffected != RowEffected.None;
    }
    public void Target(Card c, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        if (!CardModel.rowIsUnique(c.rowEffected))
        {
            deck.activateRowsByType(true, false, false, CardModel.getRowFromSide(player, c.rowEffected));
        }
        else
        {
            c.setTargetActive(true);
        }
    }
    public bool TargetCondition(Card c, RowEffected player)
    {
        return Game.state != State.MULTISTEP && c.cardType == CardType.Spy && c.rowEffected != RowEffected.None;
    }
}