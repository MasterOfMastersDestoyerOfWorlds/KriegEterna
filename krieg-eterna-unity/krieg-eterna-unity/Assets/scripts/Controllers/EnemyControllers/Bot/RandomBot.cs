using UnityEngine;
using System.Collections.Generic;
public class RandomBot : EnemyControllerInterface{
    public Move NextMove(List<Move> possibleMoves){
        return possibleMoves[Random.Range(0, possibleMoves.Count)];
    }
}