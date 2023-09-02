
namespace KriegTests
{

    using TestCase = TestCards.TestCase;
    using Click = TestCards.Click;
    using ClickRow = TestCards.ClickRow;
    using System.Collections;
    using System.Collections.Generic;
    public class TestMelee : TestCaseCollection
    {
        public override List<TestCase> getCases()
        {
            return new List<TestCase>(){
            new TestCase
            {
                testName = "Calvary",
                clicks = new List<Click>{
                    new Click("Calvary", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Calvary", RowEffected.PlayerHand, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                }
            },
            new TestCase
            {
                testName = "Calvary2",
                clicks = new List<Click>{
                    new Click("Calvary2", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Calvary2", RowEffected.PlayerHand, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                }
            },
            new TestCase
            {
                testName = "Calvary3",
                clicks = new List<Click>{
                    new Click("Calvary3", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Calvary3", RowEffected.PlayerHand, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                }
            },
            new TestCase
            {
                testName = "Halberdier",
                clicks = new List<Click>{
                    new Click("Halberdier", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Halberdier", RowEffected.PlayerHand, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                }
            },
            new TestCase
            {
                testName = "Knight",
                clicks = new List<Click>{
                    new Click("Knight", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Knight", RowEffected.PlayerHand, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                }
            },
            new TestCase
            {
                testName = "Knight10",
                clicks = new List<Click>{
                    new Click("Knight10", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Knight10", RowEffected.PlayerHand, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                }
            },
            new TestCase
            {
                testName = "Knight11",
                clicks = new List<Click>{
                    new Click("Knight11", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Knight11", RowEffected.PlayerHand, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                }
            },
            new TestCase
            {
                testName = "Knight12",
                clicks = new List<Click>{
                    new Click("Knight12", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Knight12", RowEffected.PlayerHand, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                }
            },
            new TestCase
            {
                testName = "Knight9",
                clicks = new List<Click>{
                    new Click("Knight9", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Knight9", RowEffected.PlayerHand, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                }
            },
            new TestCase
            {
                testName = "Knight3",
                clicks = new List<Click>{
                    new Click("Knight3", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Knight3", RowEffected.PlayerHand, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                }
            },
            new TestCase
            {
                testName = "Knight4",
                clicks = new List<Click>{
                    new Click("Knight4", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Knight4", RowEffected.PlayerHand, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                }
            },
            new TestCase
            {
                testName = "Knight5",
                clicks = new List<Click>{
                    new Click("Knight5", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Knight5", RowEffected.PlayerHand, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                }
            },
            new TestCase
            {
                testName = "Knight6",
                clicks = new List<Click>{
                    new Click("Knight6", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Knight6", RowEffected.PlayerHand, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                }
            },
            new TestCase
            {
                testName = "Knight7",
                clicks = new List<Click>{
                    new Click("Knight7", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Knight7", RowEffected.PlayerHand, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                }
            },
            new TestCase
            {
                testName = "Landsknecht",
                clicks = new List<Click>{
                    new Click("Landsknecht", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Landsknecht", RowEffected.PlayerHand, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                }
            },
            new TestCase
            {
                testName = "MusketLine",
                clicks = new List<Click>{
                    new Click("MusketLine", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("MusketLine", RowEffected.PlayerHand, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                }
            },
            new TestCase
            {
                testName = "Scout",
                clicks = new List<Click>{
                    new Click("Scout", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Scout", RowEffected.PlayerHand, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                }
            },

            new TestCase
            {
                testName = "ScoutReveal",
                enemyHandCount = 3,
                clicks = new List<Click>{

                    new Click("Scout", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Scout", RowEffected.PlayerHand, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                    new Click(true, "Fate", RowEffected.EnemyHand, RowEffected.EnemyHand, RowEffected.EnemyHand, false),
                    new Click(false, "Execution", RowEffected.EnemyHand, RowEffected.EnemyHand, RowEffected.PlayerChooseN, true),
                    new Click(false, "Spy", RowEffected.EnemyHand, RowEffected.EnemyHand, RowEffected.EnemyHand, false),
                }
            },
            new TestCase
            {
                testName = "Square",
                clicks = new List<Click>{
                    new Click("Square", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Square", RowEffected.PlayerHand, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                }
            },
        };
        }
    }
}