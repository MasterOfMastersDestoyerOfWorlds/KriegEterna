using System.Collections.Generic;
using UnityEngine;
using System;
public static class PowerController
{
    public static List<EffectControllerInterface> controllerList = new List<EffectControllerInterface>(){
        new PlayerDestroyController(),
        new EnemyDestroyController(),
        new MoveController(),
        new ReturnController(),
        new SetAsideController(),
        new PlayerDrawController(),
        new ChooseNController(),
        new AttachController(),
    };
    public static void PlayPower(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        RowEffected playerHand = CardModel.getHandRow(player);
        RowEffected enemy = CardModel.getEnemy(player);
        Deck deck = Game.activeDeck;
        Debug.Log("Playing Power Card targetRow:" + targetRow);
        if (targetRow == null && c.rowEffected != RowEffected.None && c.playInRow && !c.attach && Game.state != State.ROUND_END)
        {
            RowEffected rowEffected = CardModel.getRowFromSide(player, c.rowEffected);
            Debug.Log("Playing Card in: " + rowEffected);
            deck.getRowByType(rowEffected).Add(c);
        }
        else if (targetRow != null && targetRow.hasType(RowEffected.Enemy) && c.cardType == CardType.Spy)
        {
            targetRow.Add(c);
            Debug.Log("Added Spy to Row: " + targetRow);
        }
        else
        {
            foreach (EffectControllerInterface controller in controllerList)
            {
                Debug.Log("Checking Controller: " + controller);
                if (controller.PlayCondition(c, player))
                {
                    Debug.Log("Controller: " + controller);
                    controller.Play(c, targetRow, targetCard, player);
                    break;
                }
            }
        }
        if (c.doneMultiSelection(player))
        {
            Debug.Log("Done MultiSelection, Doing Side Effects");
            if (c.rowMultiple > 0 && Game.state != State.ROUND_END)
            {
                Debug.Log("Weather Effects");
                if (c.cardType != CardType.King)
                {
                    targetRow.Add(c);
                }
                if (c.rowMultiple == 1 && c.rowEffected == RowEffected.All)
                {
                    deck.clearAllWeatherEffects();
                }
            }
            if (c.playerCardDrawRemain > 0)
            {
                Debug.Log("Card Draw");
                if (c.cardDrawType == CardDrawType.Unit || (c.cardDrawType == CardDrawType.RoundEnd && Game.state == State.ROUND_END))
                {
                    int cardsDrawn = 0;
                    for (int i = 0; i < c.playerCardDrawRemain; i++)
                    {
                        deck.drawCard(deck.getRowByType(RowEffected.UnitDeck), player);
                        cardsDrawn++;
                    }
                    c.playerCardDrawRemain -= cardsDrawn;
                }
                else if (c.cardDrawType == CardDrawType.RoundEnd)
                {
                    Game.roundEndCards.Add(c);
                }
            }
            if (c.enemyCardDrawRemain > 0)
            {
                Debug.Log("Enemy Card Draw");
                int cardsDrawn = 0;
                for (int i = 0; i < c.enemyCardDrawRemain; i++)
                {
                    deck.drawCard(deck.getRowByType(RowEffected.UnitDeck), enemy);
                    cardsDrawn++;
                }
                c.enemyCardDrawRemain -= cardsDrawn;
            }
            if (c.graveyardCardDrawRemain > 0)
            {
                Debug.Log("Graveyard Card Draw");
                deck.drawCardGraveyard(c, targetCard);
            }
            if (!c.attach && c.cardType == CardType.Power && !c.playInRow)
            {
                Debug.Log("Sending Card to Graveyard");
                deck.sendCardToGraveyard(deck.getRowByType(playerHand), RowEffected.None, c);
            }

            if (c.cardReturnType == CardReturnType.RoundEnd && Game.state != State.ROUND_END)
            {
                Debug.Log("Adding card to Round End List");
                Game.roundEndCards.Add(c);
                c.roundEndRemoveType = RoundEndRemoveType.Protect;

            }
            else if (c.cardReturnType == CardReturnType.RoundEnd && Game.state == State.ROUND_END)
            {
                Row oppositeRow = deck.getRowByType(CardModel.getRowFromSide(RowEffected.Enemy, targetRow.uniqueType));
                Debug.Log("Swaping Rows: " + c + " from row: " + targetRow + " to row" + oppositeRow);
                targetRow.Remove(c);
                oppositeRow.Add(c);
                Debug.Log("Swaping Rows: " + c + " from row: " + targetRow + " to row" + oppositeRow);
                c.playerCardReturnRemain = 0;
            }

            if (c.strengthModType == StrengthModType.RoundAdvance)
            {
                Debug.Log("Round Advance");
                if (Game.enemyPassed)
                {
                    Game.turnsLeft = 2;
                }
                else
                {
                    Game.turnsLeft = 3;
                }
            }

            if (c.attach && CardModel.isUnit(c.cardType) && c.attachmentsRemaining <= 0)
            {
                Debug.Log("Unit Attach");
                targetRow.Add(c);
            }
        }

        Debug.Log("Done Playing Power Card targetRow:" + targetRow);
    }

    public static void TargetPower(Card c, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        RowEffected enemyPlayable = CardModel.getRowFromSide(player, RowEffected.EnemyPlayable);
        Debug.Log("Setup Power Targets: " + c.cardName);
        bool flag = false;
        foreach (EffectControllerInterface controller in controllerList)
        {
            if (controller.TargetCondition(c, player))
            {
                controller.Target(c, player);
                flag = true;
                break;
            }
        }
        if (!flag && c.rowEffected != RowEffected.None)
        {
            if (c.rowEffected == RowEffected.EnemyMax)
            {
                deck.activateAllRowsByType(true, false, deck.getMaxScoreRows(enemyPlayable));
            }
            deck.activateRowsByType(true, true, c.rowEffected);
        }
        else
        {
            c.setTargetActive(true);
        }
    }
}