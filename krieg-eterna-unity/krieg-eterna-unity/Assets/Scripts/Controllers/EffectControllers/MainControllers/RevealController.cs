using System.Collections.Generic;
using UnityEngine;
using System;
public class RevealController : EffectControllerInterface
{


    public void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        RowEffected enemyHand = CardModel.getRowFromSide(player, c.rowEffected);
        int numShow = c.enemyReveal;
        if(numShow < 0){
            numShow = Game.activeDeck.getRowByType(enemyHand).Count;
        }
        ChooseNController.setChooseN(enemyHand, "", null, "", 0, numShow, CardModel.chooseToCardTypeExclude(ChooseCardType.Power), null, RowEffected.None, State.REVEAL, false);
    }
    public bool PlayCondition(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        return c.enemyReveal != 0 && Game.activeDeck.getRowByType(c.rowEffected).Count > 0;
    }

    public void Target(Card c, RowEffected player)
    {
        c.setTargetActive(true);
    }

    public bool TargetCondition(Card c, RowEffected player)
    {
        return c.enemyReveal != 0 && Game.activeDeck.getRowByType(c.rowEffected).Count > 0;
    }
}