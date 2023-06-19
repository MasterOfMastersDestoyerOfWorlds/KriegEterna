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
    public RowEffected rowType;

    public Row(bool isPlayer, bool isScoring, RowEffected rowType){
        this.isPlayer = isPlayer;
        this.isScoring = isScoring;
        this.name = rowType.ToString();
        this.rowType = rowType;
    }

    public void activateTarget(){
        target.setFlashing();
    }

}
