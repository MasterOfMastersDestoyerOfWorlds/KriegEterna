using System.Collections.Generic;
using UnityEngine;
using System;
public class SetAsideController : EffectControllerInterface
{
    public void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        RowEffected row = CardModel.getRowFromSide(player, c.rowEffected);
        int cardsRemaining = deck.countCardsInRows(row);
        Debug.Log(" SetAside remaining: " + c.setAsideRemain + " cardsRemaining " + cardsRemaining + " row: " + row);
        if (c.setAsideRemain > cardsRemaining)
        {
            c.setAsideRemain = cardsRemaining - 1;
        }
        else
        {
            c.setAsideRemain--;
        }
        deck.setCardAside(targetRow, targetCard, c.setAsideType);
    }
    public bool PlayCondition(Card c, Row targetRow, Card targetCard, RowEffected player){
        return c.setAsideRemain > 0;
    }
    public void Target(Card c, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        switch (c.setAsideType)
        {
            case SetAsideType.King: deck.activateRowsByType(true, true, 
            CardModel.getRowFromSide(player, RowEffected.PlayerKing)); 
            break;
            case SetAsideType.EnemyKing: deck.activateRowsByType(true, true, 
            CardModel.getRowFromSide(player, RowEffected.EnemyKing)); 
            break;
            case SetAsideType.EitherKing: deck.activateRowsByType(true, true, 
            CardModel.getRowFromSide(player, RowEffected.King)); 
            break;
            case SetAsideType.Enemy: deck.activateRowsByType(true, true, 
            CardModel.getRowFromSide(player, RowEffected.EnemyPlayable)); 
            break;
            case SetAsideType.Player: deck.activateRowsByType(true, true, 
            CardModel.getRowFromSide(player, RowEffected.PlayerPlayable)); 
            break;
        }
    }
    public bool TargetCondition(Card c, RowEffected player)
    {
        return c.setAsideRemain > 0;
    }
    
}