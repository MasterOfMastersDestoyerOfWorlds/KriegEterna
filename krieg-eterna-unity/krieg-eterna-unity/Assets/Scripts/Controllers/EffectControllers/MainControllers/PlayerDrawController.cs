using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerDrawController : EffectControllerInterface
{
    public void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        Debug.Log("Drawing Card");
        if (c.strengthConditionPassed)
        {
            c.strengthConditionPassed = false;
        }
        else
        {
            c.playerCardDrawRemain--;
        }
        deck.drawCard(targetRow, player);
    }
    public bool PlayCondition(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        return c.playerCardDrawRemain > 0 && c.cardDrawType == CardDrawType.Either && Game.state != State.ROUND_END;
    }
    public void Target(Card c, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        switch (c.cardDrawType)
        {
            case CardDrawType.Either: deck.activateRowsByType(true, false, false, RowEffected.DrawableDeck); break;
            case CardDrawType.Unit: c.setTargetActive(true); break;
            case CardDrawType.Power: c.setTargetActive(true); break;
        }
    }
    public bool TargetCondition(Card c, RowEffected player)
    {
        return c.playerCardDrawRemain > 0;
    }
}