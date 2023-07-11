using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerDestroyController : EffectControllerInterface
{
    public void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        Debug.Log("Destroying Card");
        Deck deck = Game.activeDeck;
        RowEffected playerHand = CardModel.getHandRow(player);
        c.playerCardDestroyRemain--;
        Row playerHandRow = deck.getRowByType(playerHand);
        if (c.destroyType == DestroyType.Unit)
        {
            deck.sendCardToGraveyard(targetRow, RowEffected.None, targetCard);
            if (c.strengthCondition > 0 && targetCard.calculatedStrength > c.strengthCondition)
            {
                c.strengthConditionPassed = true;
            }
        }
        else if (c.destroyType == DestroyType.King)
        {
            Card king = playerHandRow.getKing();
            if (king != null)
            {
                c.playerCardDrawRemain++;
                deck.sendCardToGraveyard(playerHandRow, playerHand, king);
            }
            else
            {
                Row kingRow = deck.getRowByType(deck.getKingRow(player));
                deck.sendCardToGraveyard(kingRow, RowEffected.None, kingRow[0]);
            }

        }
        else if (c.destroyType == DestroyType.MaxAll)
        {
            List<Row> destroyRows = deck.getRowsByType(c.rowEffected);
            List<Card> graveyardList = new List<Card>();
            List<Row> graveyardRowList = new List<Row>();
            foreach (Row r in destroyRows)
            {
                int max = deck.maxStrength(r.uniqueType);
                foreach (Card destroyCard in r)
                {
                    if (destroyCard.calculatedStrength == max)
                    {
                        graveyardList.Add(destroyCard);
                        graveyardRowList.Add(r);
                    }
                }
            }
            for (int i = 0; i < graveyardList.Count; i++)
            {
                deck.sendCardToGraveyard(graveyardRowList[i], RowEffected.None, graveyardList[i]);
            }
            c.playerCardDestroyRemain = 0;
            c.enemyCardDestroyRemain = 0;
        }
        else if (c.destroyType == DestroyType.Max)
        {
            deck.sendAllToGraveYard(c.rowEffected, (Row r) => new List<Card>() { r.maxStrengthCard() });
            c.playerCardDestroyRemain = 0;
            c.enemyCardDestroyRemain = 0;
        }
        else if (c.destroyType == DestroyType.RoundEnd)
        {
            Game.roundEndCards.Add(c);
        }
    }
    public bool PlayCondition(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        return c.playerCardDestroyRemain > 0;
    }
    public void Target(Card c, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        RowEffected playerPlayable = CardModel.getRowFromSide(player, RowEffected.PlayerPlayable);
        switch (c.destroyType)
        {
            case DestroyType.Unit: deck.activateRowsByType(true, true, playerPlayable); break;
            default: c.setTargetActive(true); break;
        }
    }
    public bool TargetCondition(Card c, RowEffected player)
    {
        return c.playerCardDestroyRemain > 0;
    }

}