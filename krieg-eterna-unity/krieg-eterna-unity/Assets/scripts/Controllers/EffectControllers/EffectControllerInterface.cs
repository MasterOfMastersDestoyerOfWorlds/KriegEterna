using System;
public interface EffectControllerInterface
{
    public void Play(Card c, Row targetRow, Card targetCard, RowEffected player) => throw new NotImplementedException();
    public void Target(Card c, RowEffected player) => throw new NotImplementedException();
    public bool PlayCondition(Card c, RowEffected player) => false;
    public bool TargetCondition(Card c, RowEffected player) => false;
}