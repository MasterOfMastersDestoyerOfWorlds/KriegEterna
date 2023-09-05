using System.Collections.Generic;
using UnityEngine;
public class WeatherController : EffectControllerInterface
{
    public void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        Debug.Log("Weather Effects");
        if (c.cardType == CardType.Power)
        {
            targetRow.Add(c);
        }
        if (c.clearWeather)
        {
            Debug.Log("Clearing all weather");
            deck.sendAllToGraveYard(RowEffected.All, (Row r) =>  r.FindAll((x)=> x.cardType == CardType.Weather) );
        }
        else if (c.cardType == CardType.Weather)
        {
            targetRow.Add(c);
            Card clone = GameObject.Instantiate(c) as Card;
            clone.isClone = true;
            deck.getRowByType(CardModel.getRowFromSide(RowEffected.Enemy, targetRow.uniqueType)).Add(clone);
        }
    }
    public bool PlayCondition(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        return (c.rowMultiple > 0 || c.clearWeather) && Game.state != State.ROUND_END;
    }
    public bool IsSideEffect(Card c, RowEffected player) => true;
}