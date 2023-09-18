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