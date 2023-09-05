using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemyDestroyController : EffectControllerInterface
{
    public void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        Debug.Log("Destroying Enemy Card");
        if (c.destroyType == DestroyType.Unit)
        {
            int cardsRemaining = deck.countUnitsInRows(CardModel.getRowFromSide(player, c.rowEffected));
            if (c.enemyCardDestroyRemain > cardsRemaining)
            {
                c.enemyCardDestroyRemain = cardsRemaining - 1;
            }
            else
            {
                c.enemyCardDestroyRemain--;
            }
            deck.sendCardToGraveyard(targetRow, RowEffected.None, targetCard);
        }
        else if (c.destroyType == DestroyType.RoundEnd && Game.state == State.ROUND_END)
        {
            if (targetRow.Count == 1)
            {
                Card strongest = deck.getStrongestCard(CardModel.getRowFromSide(player, RowEffected.EnemyPlayable), c);
                if (strongest != null)
                {
                    Row strongestRow = deck.getCardRow(strongest);
                    deck.sendCardToGraveyard(strongestRow, RowEffected.None, strongest);
                    c.enemyCardDestroyRemain--;
                }
                else
                {
                    c.enemyCardDestroyRemain = 0;
                }
            }
        }
    }
    public bool PlayCondition(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        return c.enemyCardDestroyRemain > 0 && (c.destroyType != DestroyType.RoundEnd || Game.state == State.ROUND_END);
    }
    public void Target(Card c, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        RowEffected enemyPlayable = CardModel.getRowFromSide(player, c.rowEffected);
        deck.activateRowsByType(true, true, true, enemyPlayable);
    }
    public bool TargetCondition(Card c, RowEffected player)
    {
        return c.enemyCardDestroyRemain > 0 && c.destroyType != DestroyType.RoundEnd;
    }

}