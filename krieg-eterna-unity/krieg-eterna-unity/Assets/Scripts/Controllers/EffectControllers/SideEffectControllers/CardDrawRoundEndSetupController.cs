using UnityEngine;
public class CardDrawRoundEndSetupController : EffectControllerInterface
{
    public void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        Game.roundEndCards.Add(c);
    }
    public bool PlayCondition(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        return c.playerCardDrawRemain > 0 && c.cardDrawType == CardDrawType.RoundEnd && Game.state != State.ROUND_END;
    }
    public bool IsSideEffect(Card c, RowEffected player) => true;
}