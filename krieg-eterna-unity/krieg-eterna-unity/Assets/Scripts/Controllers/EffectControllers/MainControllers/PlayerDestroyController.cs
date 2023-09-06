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
        else if (c.destroyType == DestroyType.PlayerKing)
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
            List<RowEffected> enemyMaxRows = deck.getMaxScoreRows(CardModel.getRowFromSide(player, RowEffected.PlayerPlayable));
            List<RowEffected> playerMaxRows = deck.getMaxScoreRows(CardModel.getRowFromSide(player, RowEffected.EnemyPlayable));
            destroyMaxInRows(deck, c, playerMaxRows);
            destroyMaxInRows(deck, c, enemyMaxRows);
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
        Deck deck = Game.activeDeck;
        return c.playerCardDestroyRemain > 0;
    }
    public void Target(Card c, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        RowEffected playerPlayable = CardModel.getRowFromSide(player, RowEffected.PlayerPlayable);
        switch (c.destroyType)
        {
            case DestroyType.Unit: deck.activateRowsByType(true, true, true, playerPlayable); break;
            default: c.setTargetActive(true); break;
        }
    }
    public bool TargetCondition(Card c, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        return c.playerCardDestroyRemain > 0;
    }


    private void destroyMaxInRows(Deck deck, Card c, List<RowEffected> rows)
    {
        if (rows.Count > 0)
        {
            RowEffected rowType = rows[0];
            Row row = deck.getRowByType(rowType);
            List<Card> graveyardList = new List<Card>();
            int max = deck.maxStrength(rowType);
            foreach (Card destroyCard in row)
            {
                if (destroyCard.calculatedStrength == max)
                {
                    graveyardList.Add(destroyCard);
                }
            }
            for (int i = 0; i < graveyardList.Count; i++)
            {
                deck.sendCardToGraveyard(row, RowEffected.None, graveyardList[i]);
            }
        }
    }

}