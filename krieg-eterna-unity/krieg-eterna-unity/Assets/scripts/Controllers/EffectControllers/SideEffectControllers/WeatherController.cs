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
        if (c.rowMultiple == 1 && c.rowEffected == RowEffected.All)
        {
            deck.clearAllWeatherEffects();
            if(c.cardType == CardType.Weather){
                deck.getRowByType(RowEffected.PowerGraveyard).Add(c);
            }
        }
        else if(c.cardType == CardType.Weather){
            RowEffected row = CardModel.getPlayableRow(player, c.rowEffected);
            deck.getRowByType(row).Add(c); 
            deck.getRowByType(CardModel.getRowFromSide(RowEffected.Enemy, row)).Add(c);
        }
    }
    public bool PlayCondition(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        return c.rowMultiple > 0 && Game.state != State.ROUND_END;
    }
    public bool IsSideEffect(Card c, RowEffected player) => true;
}