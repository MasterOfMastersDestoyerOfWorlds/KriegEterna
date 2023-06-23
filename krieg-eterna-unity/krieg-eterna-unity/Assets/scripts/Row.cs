using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : List<Card>
{
    public bool isPlayer;
    public bool isScoring;
    public Target target;
    public Vector3 center;
    public string name;
    public RowEffected uniqueType;
    public List<RowEffected> rowType;
    public System.Func<Vector3> centerMethod;
    public bool wide;

    public bool cardTargetsActivated = false;

    public Row(bool isPlayer, bool isScoring, bool wide, RowEffected uniqueType, List<RowEffected> rowType, System.Func<Vector3> centerMethod)
    {
        this.isPlayer = isPlayer;
        this.isScoring = isScoring;
        this.name = uniqueType.ToString();
        this.uniqueType = uniqueType;
        this.rowType = rowType;
        this.centerMethod = centerMethod;
        this.wide = wide;
    }

    public void setActivateRowCardTargets(bool state, bool individualCards)
    {   
        Debug.Log(this.target);
        Debug.Log(this.name);
        if (individualCards)
        {
            for (int i = 0; i < this.Count; i++)
            {
                this[i].setTargetActive(state);
            }
            cardTargetsActivated = state;
        }
        else{
            this.target.setFlashing(state);
        }
    }
    public void centerRow(){
        this.center = centerMethod.Invoke();
    }
    public void setTarget(Target target)
    {
        this.target = target;
        Debug.Log("Setting Target: " + this.target + " On: " + this.name);
    }
    public Bounds getTargetBounds()
    {
        return this.target.getBounds();
    }
    public override string ToString()
    {
        return this.name;
    }
    public bool hasType(RowEffected type){
        return this.rowType.Contains(type);
    }

    public bool isTypeUnique(RowEffected type){
        return this.uniqueType == type;
    }
}
