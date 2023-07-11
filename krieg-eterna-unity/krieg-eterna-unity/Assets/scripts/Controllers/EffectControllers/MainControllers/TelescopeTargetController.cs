using System.Collections.Generic;
using UnityEngine;
using System;
public class TelescopeTargetController : EffectControllerInterface
{
    public void Target(Card c, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        RowEffected playableRow = CardModel.getPlayableRow(player, c.cardType);
        Row r = deck.getRowByType(playableRow);
        Debug.Log(r + " " + r.Count);
        if (r.Count > 0)
        {
            deck.getRowByType(playableRow).setActivateRowCardTargets(true, true);
        }
        else
        {
            c.setTargetActive(true);
        }

    }
    public bool TargetCondition(Card c, RowEffected player)
    {
        return CardModel.isUnit(c.cardType) && c.strengthModType == StrengthModType.Adjacent;
    }

}