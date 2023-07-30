using System.Collections.Generic;
using UnityEngine;
using System;
public class ReturnController : EffectControllerInterface
{
    public void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        RowEffected playerHand = CardModel.getHandRow(player);
        Debug.Log("Returning Card");
        if (c.cardReturnType == CardReturnType.Unit)
        {
            RowEffected rowEffected = CardModel.getRowFromSide(player, c.rowEffected);
            Row playerHandRow = deck.getRowByType(playerHand);
            int cardsRemaining = deck.countUnitsInRows(rowEffected);
            if (c.playerCardReturnRemain > cardsRemaining)
            {
                c.playerCardReturnRemain = cardsRemaining - 1;
            }
            else
            {
                c.playerCardReturnRemain--;
            }
            if (playerHandRow.Contains(c) && c.cardType == CardType.Decoy)
            {
                int index = targetRow.IndexOf(targetCard);
                if(index < 0 || index > targetRow.Count){
                    Debug.Log("REEEE: row:" + targetRow + " card:" + targetCard.cardName);
                }
                targetRow.Insert(index, c);
                playerHandRow.Remove(c);
            }
            deck.addCardToHand(targetRow, playerHand, targetCard);
        }
        else if (c.cardReturnType == CardReturnType.Move || c.cardReturnType == CardReturnType.Swap)
        {
            c.playerCardReturnRemain--;
            c.moveRemain++;
            c.moveCard = targetCard;
            targetCard.setTargetActive(false);
            c.moveRow = targetRow.uniqueType;
        }
        else if (c.cardReturnType == CardReturnType.King)
        {
            RowEffected kingLoc = deck.getKingRow(player);
            c.playerCardReturnRemain--;
            if (kingLoc != RowEffected.None)
            {
                Row kingRow = deck.getRowByType(kingLoc);
                deck.addCardToHand(kingRow, playerHand, kingRow[0]);
            }
        }
        else if (c.cardReturnType == CardReturnType.LastPlayedCard)
        {
            c.playerCardReturnRemain--;
            Card lastPlayed = Game.getLastCardPlayed(player) ;
            if (lastPlayed != null)
            {
                //Todo: separate enemy and player last card played
                Row row = deck.getCardRow(lastPlayed);
                deck.addCardToHand(row, playerHand, lastPlayed);
            }

        }
    }
    public bool PlayCondition(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        return c.playerCardReturnRemain > 0;
    }
    public void Target(Card c, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        RowEffected playerPlayable = CardModel.getRowFromSide(player, RowEffected.PlayerPlayable);
        switch (c.cardReturnType)
        {
            case CardReturnType.King: c.setTargetActive(true); break;
            case CardReturnType.LastPlayedCard: c.setTargetActive(true); break;
            case CardReturnType.Unit: deck.activateRowsByType(true, true, true, CardModel.getRowFromSide(player, c.rowEffected)); break;
            default: deck.activateRowsByType(true, true, true, playerPlayable); break;
        }
    }
    public bool TargetCondition(Card c, RowEffected player)
    {
        if(c.cardReturnType == CardReturnType.LastPlayedCard){
            Card lastPlayed = Game.getLastCardPlayed(player);
            return lastPlayed != null;
        }
        return c.playerCardReturnRemain > 0 ;
    }
}