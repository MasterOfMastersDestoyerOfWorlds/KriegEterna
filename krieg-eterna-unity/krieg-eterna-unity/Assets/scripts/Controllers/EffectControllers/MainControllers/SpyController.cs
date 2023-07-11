using System.Collections.Generic;
using UnityEngine;
using System;
public class SpyController : EffectControllerInterface
{
    public void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        targetRow.Add(c);
        Debug.Log("Added Spy to Row: " + targetRow);
    }
    public bool PlayCondition(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        return targetRow != null && targetRow.hasType(RowEffected.Enemy) && c.cardType == CardType.Spy;
    }
}