using UnityEngine;
public class TelescopeController : EffectControllerInterface
{
    public void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        Debug.Log("Unit Attach");
        targetRow.Add(c);
    }
    public bool PlayCondition(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        return c.attach && CardModel.isUnit(c.cardType) && c.attachmentsRemaining <= 0;
    }
    public bool IsSideEffect(Card c, RowEffected player) => true;
}