
namespace KriegTests
{

    using TestCase = TestCards.TestCase;
    using Click = TestCards.Click;
    using ClickRow = TestCards.ClickRow;
    using System.Collections;
    using System.Collections.Generic;
    public class TestSpys : TestCaseCollection
    {
        public override List<TestCase> getCases()
        {
            return new List<TestCase>(){

            new TestCase
            {
                testName = "Assassin",
                playerHandCount = 2,
                clicks = new List<Click>{
                    new Click("Assassin", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.EnemySiege, true),
                    new ClickRow("EnemySiege", RowEffected.EnemySiege),
                    new ClickRow("UnitDeck", RowEffected.UnitDeck),
                    new ClickRow("UnitDeck", RowEffected.UnitDeck),
                }
            },
            new TestCase
            {
                testName = "AssassinRoundEnd",
                playerHandCount = 2,
                enemyHandCount = 1,
                round = RoundType.RoundTwo,
                clicks = new List<Click>{
                    new Click("Assassin", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.EnemyHand, true),
                    new ClickRow("EnemyRanged", RowEffected.EnemyRanged),
                    new ClickRow("UnitDeck", RowEffected.UnitDeck),
                    new ClickRow("PowerDeck", RowEffected.PowerDeck),
                    new ClickRow("Pass", RowEffected.Pass)
                }
            },
            new TestCase
            {
                testName = "AssassinRoundEndGraveyard",
                playerHandCount = 2,
                round = RoundType.RoundTwo,
                clicks = new List<Click>{
                    new Click("Assassin", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("EnemyRanged", RowEffected.EnemyRanged),
                    new ClickRow("UnitDeck", RowEffected.UnitDeck),
                    new ClickRow("PowerDeck", RowEffected.PowerDeck),
                    new Click("Death", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new Click("Assassin", RowEffected.EnemyRanged, RowEffected.PowerGraveyard, RowEffected.PowerGraveyard, true),
                    new ClickRow("Pass", RowEffected.Pass)
                }
            },
            new TestCase
            {
                testName = "Minister",
                playerHandCount = 2,
                clicks = new List<Click>{
                    new Click("Minister", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.EnemySiege, true),
                    new Click("Minister", RowEffected.PlayerHand, RowEffected.EnemySiege, RowEffected.EnemySiege, true),
                    new ClickRow("UnitDeck", RowEffected.UnitDeck),
                    new ClickRow("UnitDeck", RowEffected.UnitDeck),
                }
            },
            new TestCase
            {
                testName = "Saboteur",
                playerHandCount = 2,
                clicks = new List<Click>{
                    new Click("Saboteur", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.EnemyRanged, true),
                    new Click("Saboteur", RowEffected.PlayerHand, RowEffected.EnemyRanged, RowEffected.EnemyRanged, true),
                    new ClickRow("UnitDeck", RowEffected.UnitDeck),
                    new ClickRow("UnitDeck", RowEffected.UnitDeck),
                }
            },
            new TestCase
            {
                testName = "Zealot",
                playerHandCount = 2,
                clicks = new List<Click>{
                    new Click("Zealot", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.EnemyMelee, true),
                    new Click("Zealot", RowEffected.PlayerHand, RowEffected.EnemyMelee, RowEffected.EnemyMelee, true),
                    new ClickRow("UnitDeck", RowEffected.UnitDeck),
                    new ClickRow("PowerDeck", RowEffected.PowerDeck),
                }
            },
            new TestCase
            {
                testName = "Smuggler",
                playerHandCount = 2,
                enemyHandCount = 1,
                clicks = new List<Click>{
                    new Click("Smuggler", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("PowerDeck", RowEffected.PowerDeck),
                    new ClickRow("PowerDeck", RowEffected.PowerDeck),
                }
            },
            new TestCase
            {
                testName = "Spy",
                playerHandCount = 2,
                clicks = new List<Click>{
                    new Click("Spy", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.EnemyMelee, true),
                    new Click("Spy", RowEffected.PlayerHand, RowEffected.EnemyMelee, RowEffected.EnemyMelee, true),
                    new ClickRow("UnitDeck", RowEffected.UnitDeck),
                    new ClickRow("PowerDeck", RowEffected.PowerDeck),
                }
            }
        };
        }
    }
}