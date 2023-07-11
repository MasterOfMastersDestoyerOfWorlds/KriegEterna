
namespace KriegTests
{

    using TestCase = TestCards.TestCase;
    using Click = TestCards.Click;
    using ClickRow = TestCards.ClickRow;
    using System.Collections;
    using System.Collections.Generic;
    public class TestWeather
    {
        public static List<TestCase> cases = new List<TestCase>(){
            new TestCase
            {
                testName = "ClearSkies",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.EnemySiege, 2),
                    (RowEffected.PlayerSiege, 6),
                    (RowEffected.PlayerRanged, 8),
                },
                clicks = new List<Click>{
                    new Click("Storm", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("Player Siege", RowEffected.PlayerSiege),                   
                    new Click("Siege", RowEffected.EnemySiege, RowEffected.EnemySiege, RowEffected.EnemySiege, false),
                    new Click("Mortar", RowEffected.PlayerSiege, RowEffected.PlayerSiege, RowEffected.PlayerSiege, false),
                    new Click("Fog", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("Enemy Ranged", RowEffected.EnemyRanged),
                    new Click("Soldier", RowEffected.PlayerRanged, RowEffected.PlayerRanged, RowEffected.PlayerRanged, false),
                    new Click("Soldier2", RowEffected.PlayerRanged, RowEffected.PlayerRanged, RowEffected.PlayerRanged, false),
                    new Click("ClearSkies", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("Player Melee", RowEffected.PlayerMelee),
                }
            },
            new TestCase
            {
                testName = "Fog",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.PlayerRanged, 4),
                },
                clicks = new List<Click>{
                    new Click("Fog", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new ClickRow("Player Ranged", RowEffected.PlayerRanged),
                    new Click("Fog", RowEffected.PlayerHand, RowEffected.PlayerRanged, RowEffected.EnemyRanged, false),
                    new Click("Soldier", RowEffected.PlayerRanged, RowEffected.PlayerRanged, RowEffected.PlayerRanged, false),
                    new Click("Soldier2", RowEffected.PlayerRanged, RowEffected.PlayerRanged, RowEffected.PlayerRanged, false),
                }
            },
            new TestCase
            {
                testName = "Frost",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.EnemyMelee, 2),
                },
                clicks = new List<Click>{
                    new Click("Frost", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new ClickRow("Player Melee", RowEffected.PlayerMelee),
                    new Click("Frost", RowEffected.PlayerHand, RowEffected.PlayerMelee, RowEffected.EnemyMelee, false),
                    new Click("Knight3", RowEffected.EnemyMelee, RowEffected.EnemyMelee, RowEffected.EnemyMelee, false),
                    new Click("Officer", RowEffected.EnemyMelee, RowEffected.EnemyMelee, RowEffected.EnemyMelee, false),
                }
            },
            new TestCase
            {
                testName = "Storm",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.EnemySiege, 1),
                    (RowEffected.PlayerSiege, 3),
                },
                clicks = new List<Click>{
                    new Click("Storm", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true), 
                    new ClickRow("Enemy Siege", RowEffected.EnemySiege),          
                    new Click("Storm", RowEffected.PlayerHand, RowEffected.PlayerSiege, RowEffected.EnemySiege, false),
                    new Click("Siege", RowEffected.EnemySiege, RowEffected.EnemySiege, RowEffected.EnemySiege, false),
                    new Click("Mortar", RowEffected.PlayerSiege, RowEffected.PlayerSiege, RowEffected.PlayerSiege, false),
                }
            },
        };
    }
}