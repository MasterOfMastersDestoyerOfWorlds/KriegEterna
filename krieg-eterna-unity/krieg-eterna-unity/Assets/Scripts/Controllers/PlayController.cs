using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class PlayController
{
    public static void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        if (!c.isVisible())
        {
            c.setVisible(true);
        }
        SteamNetworkingTest net = Game.steamManager.NetworkingTest;
        if (net.isNetworkGame && player == RowEffected.Player)
        {
            Move move = new Move(c, targetCard, targetRow.uniqueType, player, false, false);
            net.sendNextMessage(PacketType.MOVE, move.id);
        }
        MoveLogger.logMove(c, targetRow, targetCard, player);
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
        if (Game.state == State.CHOOSE_N)
        {
            Debug.Log("ChooseN selected!" + c.cardName);
            PowerController.PlayPower(c, targetRow, targetCard, player);
        }
        else if (CardModel.isUnit(c.cardType) && c.strengthModType == StrengthModType.Adjacent && targetRow != null)
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
                default: PowerController.PlayPower(c, targetRow, targetCard, player); break;
            }
        }
        updateStateBasedOnCardState(c, player);
        if (Game.state != State.MULTISTEP)
        {
            if (c.doneMultiSelection(player))
            {
                deck.getRowByType(CardModel.getRowFromSide(player, RowEffected.PlayerHand)).Remove(c);
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
        c.playerPlayed = player;
        Game.reorganizeGroup();
    }

    public static void updateStateBasedOnCardState(Card c, RowEffected player)
    {
        c.LogSelectionsRemaining();
        if (Game.state == State.CHOOSE_N || Game.state == State.REVEAL)
        {
            return;
        }
        if (c.doneMultiSelection(player))
        {
            Debug.Log("Done with card: " + c.cardName);
            Game.setLastCardPlayed(c, player);
            Game.state = State.FREE;
        }
        else
        {
            Game.state = State.MULTISTEP;
        }
        Debug.Log(Game.state);
    }

    internal static void Play(Move nextMove)
    {
        Play(nextMove.c, Game.activeDeck.getRowByType(nextMove.targetRow), nextMove.targetCard, nextMove.player);
        nextMove.executedMove = true;
    }
}