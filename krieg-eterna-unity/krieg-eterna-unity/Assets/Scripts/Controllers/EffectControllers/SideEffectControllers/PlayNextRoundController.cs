using UnityEngine;
public class PlayNextRoundController : EffectControllerInterface
{
    public void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        Debug.Log("Play Next Round Controller");
        Debug.Log("Adding card to Round End List");
        Game.roundEndCards.Add(c);
        c.roundEndRemoveType = RoundEndRemoveType.Protect;
    }
    public bool PlayCondition(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        return (c.cardReturnType == CardReturnType.SwitchSidesRoundEnd || c.playNextRound) && Game.state != State.ROUND_END;
    }
    public bool IsSideEffect(Card c, RowEffected player) => true;
}