using System.Collections.Generic;
using UnityEngine;
using System;
public class KingController : EffectControllerInterface
{
    public void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        RowEffected playerHand = CardModel.getHandRow(player);
        Row playerHandRow = deck.getRowByType(playerHand);
        targetRow.Add(c);
        playerHandRow.Remove(c);

        RowEffected enemySide = CardModel.getRowFromSide(player, RowEffected.EnemyPlayable);
        RowEffected playerSide = CardModel.getRowFromSide(player, RowEffected.PlayerPlayable);
        int cardsRemaining = Game.activeDeck.countUnitsInRows(enemySide);
        int cardsRemainingPlayer = Game.activeDeck.countUnitsInRows(playerSide);
        Debug.Log(c.cardName + " setAside: " + c.setAsideRemain);
        if (c.setAsideRemain > cardsRemaining)
        {
            c.setAsideRemain = cardsRemaining;
        }
        if (c.enemyCardDestroyRemain > cardsRemaining)
        {
            c.enemyCardDestroyRemain = cardsRemaining;
        }
        if (c.playerCardReturnRemain > cardsRemaining)
        {
            c.playerCardReturnRemain = cardsRemaining;
        }
        if (c.playerCardReturnRemain > cardsRemainingPlayer && c.cardReturnType == CardReturnType.Swap)
        {
            c.playerCardReturnRemain = cardsRemainingPlayer;
        }
    }
    public bool PlayCondition(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        return c.cardType == CardType.King && CardModel.isHandRow(c.currentRow);
    }
    public void Target(Card c, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        RowEffected playerKing = CardModel.getRowFromSide(player, RowEffected.PlayerKing);
        deck.activateRowsByType(true, false, false, playerKing);
    }
    public bool TargetCondition(Card c, RowEffected player)
    {
        return c.cardType == CardType.King && CardModel.isHandRow(c.currentRow);
    }
}