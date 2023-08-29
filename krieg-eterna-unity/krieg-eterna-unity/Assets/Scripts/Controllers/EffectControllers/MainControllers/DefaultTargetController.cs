using System.Collections.Generic;
using UnityEngine;
using System;
public class DefaultTargetController : EffectControllerInterface
{
    public void Target(Card c, RowEffected player)
    {
        c.setTargetActive(true);
    }
    public bool TargetCondition(Card c, RowEffected player)
    {
        return true;
    }

}