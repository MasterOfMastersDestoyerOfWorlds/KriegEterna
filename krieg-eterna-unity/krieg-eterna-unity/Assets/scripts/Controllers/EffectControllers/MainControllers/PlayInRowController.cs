using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayInRowController : EffectControllerInterface
{
    public void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        RowEffected rowEffected = CardModel.getRowFromSide(player, c.rowEffected);
        Debug.Log("Playing Card in: " + rowEffected);
        deck.getRowByType(rowEffected).Add(c);
    }
    public bool PlayCondition(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        return targetRow == null && c.rowEffected != RowEffected.None && 
        c.playInRow && !c.attach && Game.state != State.ROUND_END && c.cardType != CardType.Weather;
    }
}