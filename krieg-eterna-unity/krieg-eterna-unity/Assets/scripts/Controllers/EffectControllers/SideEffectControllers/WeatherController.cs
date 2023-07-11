using UnityEngine;
public class WeatherController : EffectControllerInterface
{
    public void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        Deck deck = Game.activeDeck;
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
    public bool PlayCondition(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        return c.rowMultiple > 0 && Game.state != State.ROUND_END;
    }
    public bool IsSideEffect(Card c, RowEffected player) => true;
}