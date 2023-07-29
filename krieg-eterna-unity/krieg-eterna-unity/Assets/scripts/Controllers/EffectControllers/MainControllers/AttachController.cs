using System.Collections.Generic;
using UnityEngine;
using System;
public class AttachController : EffectControllerInterface
{
    public void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        int cardsRemaining = deck.countUnitsInRows(targetRow.uniqueType);
        Debug.Log(" attachments remaining: " + c.attachmentsRemaining + " cardsRemaining " + cardsRemaining + " row: " + targetRow);
        if (c.attachmentsRemaining > cardsRemaining)
        {
            c.attachmentsRemaining = cardsRemaining - 1;
        }
        else
        {
            c.attachmentsRemaining--;
        }

        targetCard.attachCard(c);
        if (c.cardReturnType == CardReturnType.ProtectAtRoundEnd)
        {
            targetCard.roundEndRemoveType = RoundEndRemoveType.Protect;
        }
    }
    public bool PlayCondition(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        return c.attach && c.attachmentsRemaining > 0;
    }
    public void Target(Card c, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        deck.activateRowsByType(true, true, true, RowEffected.All);
    }
    public bool TargetCondition(Card c, RowEffected player)
    {
        return c.attach;
    }
}