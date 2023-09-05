using System.Collections.Generic;
using UnityEngine;
using System;
public class ClearWeatherTargetController : EffectControllerInterface
{
    public void Target(Card c, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        List<Card> weatherCards = deck.getCardsInRowsByCardType(RowEffected.All, CardType.Weather);
        c.setTargetActive(true);
        foreach(Card card in weatherCards){
            card.setTargetActive(true);
        }
    }
    public bool TargetCondition(Card c, RowEffected player)
    {
        return  c.rowEffected == RowEffected.Weather;
    }

}