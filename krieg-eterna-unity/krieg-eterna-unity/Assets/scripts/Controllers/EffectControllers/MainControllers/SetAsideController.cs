using System.Collections.Generic;
using UnityEngine;
using System;
public class SetAsideController : EffectControllerInterface
{
    public void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        int cardsRemaining = deck.countCardsInRows(c.rowEffected);
        Debug.Log(" SetAside remaining: " + c.setAsideRemain + " cardsRemaining " + cardsRemaining + " row: " + c.rowEffected);
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
        RowEffected playerKing = CardModel.getRowFromSide(player, RowEffected.PlayerKing);
        RowEffected enemyKing = CardModel.getRowFromSide(player, RowEffected.EnemyKing);
        RowEffected eitherKing = CardModel.getRowFromSide(player, RowEffected.King);
        RowEffected enemy = CardModel.getEnemy(player);
        switch (c.setAsideType)
        {
            case SetAsideType.King: deck.activateRowsByType(true, true, playerKing); break;
            case SetAsideType.EnemyKing: deck.activateRowsByType(true, true, enemyKing); break;
            case SetAsideType.EitherKing: deck.activateRowsByType(true, true, eitherKing); break;
            case SetAsideType.Enemy: deck.activateRowsByType(true, true, enemy); break;
            case SetAsideType.Player: deck.activateRowsByType(true, true, player); break;
        }
    }
    public bool TargetCondition(Card c, RowEffected player)
    {
        return c.setAsideRemain > 0;
    }
    
}