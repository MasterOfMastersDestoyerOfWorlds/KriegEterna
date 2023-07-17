using System.Collections.Generic;
using UnityEngine;
using System;
public class MoveController : EffectControllerInterface
{
    public void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        c.moveRemain--;
        Debug.Log("MOVING " + c.cardReturnType);
        if (c.cardReturnType == CardReturnType.Move)
        {
            deck.addCardToHand(deck.getRowByType(c.moveRow), targetRow.uniqueType, c.moveCard);
        }
        if (c.cardReturnType == CardReturnType.Swap)
        {

            Debug.Log("Swaping Cards: " + c.moveCard + " " + targetCard);
            deck.addCardToHand(deck.getRowByType(c.moveRow), CardModel.getRowFromSide(RowEffected.Enemy, c.moveRow), c.moveCard);
            deck.addCardToHand(deck.getRowByType(targetRow.uniqueType), CardModel.getRowFromSide(RowEffected.Enemy, targetRow.uniqueType), targetCard);
        }
    }
    public bool PlayCondition(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        return c.moveRemain > 0;
    }
    public void Target(Card c, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        RowEffected row = CardModel.getRowFromSide(player, c.rowEffected);
        if (c.cardReturnType == CardReturnType.Move)
        {
            deck.activateRowsByTypeExclude(true, false, row, c.moveRow);
        }
        else if (c.cardReturnType == CardReturnType.Swap)
        {
            deck.activateRowsByTypeExclude(true, true, row, c.moveRow);
        }
    }
    public bool TargetCondition(Card c, RowEffected player)
    {
        return c.moveRemain > 0;
    }
}