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