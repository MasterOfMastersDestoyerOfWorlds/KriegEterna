using System.Collections.Generic;
using UnityEngine;
using System;
public class RowTargetController : EffectControllerInterface
{
    public void Target(Card c, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        RowEffected enemyPlayable = CardModel.getRowFromSide(player, RowEffected.EnemyPlayable);
        if (c.rowEffected == RowEffected.EnemyMax)
        {
            deck.activateAllRowsByType(true, false, deck.getMaxScoreRows(enemyPlayable));
        }
        if (CardModel.rowIsUnique(c.rowEffected))
        {
            deck.activateRowsByType(true, true, CardModel.getRowFromSide(player, c.rowEffected));
        }
        else
        {
            deck.activateRowsByType(true, false, CardModel.getRowFromSide(player, c.rowEffected));
        }
        
    }
    public bool TargetCondition(Card c, RowEffected player)
    {
        return  c.rowEffected != RowEffected.None;
    }

}