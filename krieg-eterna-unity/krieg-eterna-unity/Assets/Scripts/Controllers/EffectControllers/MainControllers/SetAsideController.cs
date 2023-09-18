using System.Collections.Generic;
using UnityEngine;
using System;
public class SetAsideController : EffectControllerInterface
{
    public void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        RowEffected row = CardModel.getRowFromSide(player, c.rowEffected);
        int cardsRemaining;
        if (c.setAsideType == SetAsideType.King || c.setAsideType == SetAsideType.EitherKing || c.setAsideType == SetAsideType.EnemyKing)
        {
            cardsRemaining = deck.countCardsInRows(row);
        }
        else
        {
            cardsRemaining = deck.countUnitsInRows(row);
        }
        Debug.Log(" SetAside remaining: " + c.setAsideRemain + " cardsRemaining " + cardsRemaining + " row: " + row);
        if (c.setAsideRemain > cardsRemaining)
        {
            c.setAsideRemain = cardsRemaining - 1;
        }
        else
        {
            c.setAsideRemain--;
        }

        RowEffected playerHand = CardModel.getHandRow(player);
        Row playerHandRow = deck.getRowByType(playerHand);
        if (playerHandRow.Contains(c) && c.cardType == CardType.Decoy)
        {
            int index = targetRow.IndexOf(targetCard);
            if (index < 0 || index > targetRow.Count)
            {
                Debug.Log("REEEE: row:" + targetRow + " card:" + targetCard.cardName);
            }
            targetRow.Insert(index, c);
            playerHandRow.Remove(c);
        }


        deck.setCardAside(targetRow, targetCard, c.setAsideType, player);


    }
    public bool PlayCondition(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        return c.setAsideRemain > 0;
    }
    public void Target(Card c, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        switch (c.setAsideType)
        {
            case SetAsideType.King:
                deck.activateRowsByType(true, true, false,
            CardModel.getRowFromSide(player, RowEffected.PlayerKing));
                break;
            case SetAsideType.EnemyKing:
                deck.activateRowsByType(true, true, false,
            CardModel.getRowFromSide(player, RowEffected.EnemyKing));
                break;
            case SetAsideType.EitherKing:
                deck.activateRowsByType(true, true, false,
            CardModel.getRowFromSide(player, RowEffected.King));
                break;
            case SetAsideType.Enemy:
                int activated = deck.activateRowsByType(true, true, true,
            CardModel.getRowFromSide(player, RowEffected.EnemyPlayable));
                if (c.setAsideRemain > activated)
                {
                    c.setAsideRemain = activated; 
                }
                break;
            case SetAsideType.Player:
                deck.activateRowsByType(true, true, true,
            CardModel.getRowFromSide(player, RowEffected.PlayerPlayable));
                break;
            case SetAsideType.AutoPlay:
                deck.activateRowsByType(true, true, true,
            CardModel.getRowFromSide(player, RowEffected.PlayerPlayable));
                break;
        }
    }
    public bool TargetCondition(Card c, RowEffected player)
    {
        return c.setAsideRemain > 0;
    }

}