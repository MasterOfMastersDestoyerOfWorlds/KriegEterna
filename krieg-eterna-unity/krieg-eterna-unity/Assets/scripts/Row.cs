using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : List<Card>
{
    public bool isPlayer;
    public bool isScoring;

    public Row(bool isPlayer, bool isScoring){
        this.isPlayer = isPlayer;
        this.isScoring = isScoring;
    }

}
