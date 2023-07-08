using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
public class PlayController
{
    public static void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        Debug.Log("Playing: " + c.cardName + " Type: " + c.cardType);
        if (targetRow != null)
        {
            Debug.Log(" TargetRow: " + targetRow.name);
        }
        if (targetCard != null)
        {
            Debug.Log(" TargetCard: " + targetCard.cardName);
        }
        if (CardModel.isUnit(c.cardType) && c.strengthModType == StrengthModType.Adjacent && targetRow != null)
        {
            PlayPower(c, targetRow, targetCard, RowEffected.Player);
        }
        else
        {
            switch (c.cardType)
            {
                case CardType.Melee: deck.getRowByType(CardModel.getRowFromSide(player, RowEffected.PlayerMelee)).Add(c); c.zeroSelectionCounts(); break;
                case CardType.Ranged: deck.getRowByType(CardModel.getRowFromSide(player, RowEffected.PlayerRanged)).Add(c); c.zeroSelectionCounts(); break;
                case CardType.Siege: deck.getRowByType(CardModel.getRowFromSide(player, RowEffected.PlayerSiege)).Add(c); c.zeroSelectionCounts(); break;
                case CardType.Switch: targetRow.Add(c); c.zeroSelectionCounts(); break;
                case CardType.Weather: PlayWeather(c); break;
                case CardType.King: PlayKing(c, targetRow, targetCard, player); break;
                default: PlayPower(c, targetRow, targetCard, RowEffected.Player); break;
            }
        }
        updateStateBasedOnCardState(c, player);
        if (Game.state != State.MULTISTEP)
        {
            if (c.doneMultiSelection(player))
            {
                deck.getRowByType(CardModel.getRowFromSide(player, RowEffected.PlayerHand)).Remove(c);
            }
            RowEffected enemyHand = CardModel.getRowFromSide(player, RowEffected.EnemyHand);
            if (c.enemyReveal > 0 && deck.getRowByType(enemyHand).Count > 0)
            {
                Game.setChooseN(enemyHand, null, 0, c.enemyReveal, new List<CardType>() { CardType.King, CardType.Melee, CardType.Ranged, CardType.Siege }, RowEffected.None, State.REVEAL, false);

            }
        }
        if (Game.state == State.MULTISTEP)
        {
            Debug.Log("Setting skippable: " + c.canSkip());
            if (c.canSkip())
            {
                deck.getRowByType(RowEffected.Skip).setVisibile(true);
            }
        }
        Game.reorganizeGroup();
    }
    public static void PlayKing(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        if (Game.state == State.MULTISTEP)
        {
            PlayPower(c, targetRow, targetCard, player);
        }
        else
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
    }

    public static void PlayWeather(Card c)
    {
        Debug.Log("Playing Weather in: " + c.rowEffected);
        Deck deck = Game.activeDeck;
        switch (c.rowEffected)
        {
            case RowEffected.Melee: deck.getRowByType(RowEffected.PlayerMelee).Add(c); deck.getRowByType(RowEffected.EnemyMelee).Add(c); break;
            case RowEffected.Ranged: deck.getRowByType(RowEffected.PlayerRanged).Add(c); deck.getRowByType(RowEffected.EnemyRanged).Add(c); break;
            case RowEffected.Siege: deck.getRowByType(RowEffected.PlayerSiege).Add(c); deck.getRowByType(RowEffected.EnemySiege).Add(c); break;
            case RowEffected.All: deck.getRowByType(RowEffected.PowerGraveyard).Add(c); deck.clearAllWeatherEffects(); break;
            default: break;
        }
    }
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
        else if (c.playerCardDestroyRemain > 0)
        {
            Debug.Log("Destroying Card");
            c.playerCardDestroyRemain--;
            Row playerHandRow = deck.getRowByType(playerHand);
            if (c.destroyType == DestroyType.Unit)
            {
                deck.sendCardToGraveyard(targetRow, RowEffected.None, targetCard);
                Debug.Log("GTRRRRREEEERRRR " + c.strengthCondition + " " + targetCard.calculatedStrength);
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
                deck.sendAllToGraveYard(c.rowEffected, (Row r)=> new List<Card>(){r.maxStrengthCard()});
                c.playerCardDestroyRemain = 0;
                c.enemyCardDestroyRemain = 0;
            }
            else if (c.destroyType == DestroyType.RoundEnd)
            {
                Game.roundEndCards.Add(c);
            }
        }
        else if (c.enemyCardDestroyRemain > 0 && (c.destroyType != DestroyType.RoundEnd || Game.state == State.ROUND_END))
        {
            Debug.Log("Destroying Enemy Card");
            if (c.destroyType == DestroyType.Unit)
            {
                int cardsRemaining = deck.countCardsInRows(c.rowEffected);
                if (c.enemyCardDestroyRemain > cardsRemaining)
                {
                    c.enemyCardDestroyRemain = cardsRemaining - 1;
                }
                else
                {
                    c.enemyCardDestroyRemain--;
                }
                deck.sendCardToGraveyard(targetRow, RowEffected.None, targetCard);
            }
            else if (c.destroyType == DestroyType.RoundEnd && Game.state == State.ROUND_END)
            {
                if (targetRow.Count == 1)
                {
                    Card strongest = deck.getStrongestCard(CardModel.getRowFromSide(player, RowEffected.EnemyPlayable), c);
                    if (strongest != null)
                    {
                        Row strongestRow = deck.getCardRow(strongest);
                        deck.sendCardToGraveyard(strongestRow, RowEffected.None, strongest);
                        c.enemyCardDestroyRemain--;
                    }
                    else
                    {
                        c.enemyCardDestroyRemain = 0;
                    }
                }
            }
        }
        else if (c.moveRemain > 0)
        {
            c.moveRemain--;
            Debug.Log("MOVING " + c.cardReturnType);
            if (c.cardReturnType == CardReturnType.Move)
            {
                deck.addCardToHand(deck.getRowByType(c.moveRow), targetRow.uniqueType, c.moveCard);
            }
            if (c.cardReturnType == CardReturnType.Swap)
            {

                Debug.Log("Swaping Cards: " + c.moveCard + " " + targetCard);
                deck.addCardToHand(deck.getRowByType(c.moveRow), CardModel.getRowFromSide(RowEffected.Enemy, c.moveRow), c.moveCard);
                deck.addCardToHand(deck.getRowByType(targetRow.uniqueType), CardModel.getRowFromSide(RowEffected.Enemy, targetRow.uniqueType), targetCard);
            }
        }
        else if (c.playerCardReturnRemain > 0)
        {
            Debug.Log("Returning Card");
            if (c.cardReturnType == CardReturnType.Unit)
            {

                int cardsRemaining = deck.countCardsInRows(c.rowEffected);
                if (c.playerCardReturnRemain > cardsRemaining)
                {
                    c.playerCardReturnRemain = cardsRemaining - 1;
                }
                else
                {
                    c.playerCardReturnRemain--;
                }
                if (c.playerCardReturnRemain <= 0 && c.cardType == CardType.Decoy)
                {
                    int index = targetRow.IndexOf(targetCard);
                    targetRow.Insert(index, c);
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
            } else if(c.cardReturnType == CardReturnType.LastPlayedCard){
                c.playerCardReturnRemain--;
                if (Game.lastPlayedCard != null)
                {
                    Row row = deck.getCardRow(Game.lastPlayedCard);
                    deck.addCardToHand(row, playerHand, Game.lastPlayedCard);
                }

            }
        }
        else if (c.setAsideRemain > 0)
        {
            int cardsRemaining = deck.countCardsInRows(c.rowEffected);
            Debug.Log(" SetAside remaining: " + c.setAsideRemain + " cardsRemaining " + cardsRemaining + " row: " + c.rowEffected);
            if (c.setAsideRemain > cardsRemaining)
            {
                c.setAsideRemain = cardsRemaining - 1;
            }
            else
            {
                c.setAsideRemain--;
            }
            deck.setCardAside(targetRow, targetCard, c.setAsideType);
        }
        else if (c.playerCardDrawRemain > 0 && c.cardDrawType == CardDrawType.Either)
        {
            Debug.Log("Drawing Card");
            if (c.strengthConditionPassed)
            {
                c.strengthConditionPassed = false;
            }
            else
            {
                c.playerCardDrawRemain--;
            }
            deck.drawCard(targetRow, player);
        }
        else if (c.chooseNRemain > 0)
        {
            Debug.Log("Setting up Choose N");
            Row row = deck.getRowByType(c.chooseRow);
            if (row.Count > 0)
            {
                RowEffected sendRow = CardModel.getRowFromSide(player, c.rowEffected);
                Action<Row, RowEffected, Card> chooseAction = deck.addCardToHand;
                if (c.strengthModType == StrengthModType.Multiply)
                {
                    chooseAction = deck.addCardToHandMultiply;
                }
                Game.setChooseN(c.chooseRow, chooseAction, c.chooseN, c.chooseShowN > 0 ? c.chooseShowN : row.Count, CardModel.chooseToCardTypeExclude(c.chooseCardType), sendRow, State.CHOOSE_N, true);
            }
        }
        else if (c.attach && c.attachmentsRemaining > 0)
        {
            int cardsRemaining = deck.countCardsInRows(targetRow.uniqueType);
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
            if (c.cardReturnType == CardReturnType.Protect)
            {
                targetCard.roundEndRemoveType = RoundEndRemoveType.Protect;
            }
        }
        if (c.doneMultiSelection(player))
        {
            Debug.Log("Done MultiSelection, Doing Side Effects");
            if (c.rowMultiple > 0  && Game.state != State.ROUND_END)
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

    public static void updateStateBasedOnCardState(Card c, RowEffected player)
    {
        c.LogSelectionsRemaining();
        if (Game.state == State.CHOOSE_N)
        {
            return;
        }
        if (c.doneMultiSelection(player))
        {
            Debug.Log("Done with card: " + c.cardName);
            Game.lastPlayedCard = c;
            Game.state = State.FREE;
        }
        else
        {
            Game.state = State.MULTISTEP;
        }
        Debug.Log(Game.state);
    }


}