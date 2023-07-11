using System.Collections.Generic;
using UnityEngine;
using System;
public class KingController : EffectControllerInterface
{
    public void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        targetRow.Add(c);
        RowEffected enemySide = CardModel.getRowFromSide(player, RowEffected.EnemyPlayable);
        RowEffected playerSide = CardModel.getRowFromSide(player, RowEffected.PlayerPlayable);
        int cardsRemaining = Game.activeDeck.countCardsInRows(enemySide);
        int cardsRemainingPlayer = Game.activeDeck.countCardsInRows(playerSide);
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
        return Game.state != State.MULTISTEP && c.cardType == CardType.King;
    }
}