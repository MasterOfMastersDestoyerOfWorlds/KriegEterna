using UnityEngine;
public class VoidController : EffectControllerInterface
{
    public void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        Debug.Log("Round Advance");
        if (!Game.enemyPassed && !Game.playerPassed)
        {
            Game.turnsLeft = 3;
        }
        else
        {
            Game.turnsLeft = 2;
        }
    }
    public bool PlayCondition(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        return c.strengthModType == StrengthModType.RoundAdvance;
    }
    public bool IsSideEffect(Card c, RowEffected player) => true;
}