using System.Collections.Generic;
using UnityEngine;
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
            PowerController.PlayPower(c, targetRow, targetCard, RowEffected.Player);
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
                default: PowerController.PlayPower(c, targetRow, targetCard, RowEffected.Player); break;
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
                ChooseNController.setChooseN(enemyHand, null, 0, c.enemyReveal, new List<CardType>() { CardType.King, CardType.Melee, CardType.Ranged, CardType.Siege }, RowEffected.None, State.REVEAL, false);

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
            PowerController.PlayPower(c, targetRow, targetCard, player);
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