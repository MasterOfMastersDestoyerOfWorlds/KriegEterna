
namespace KriegTests
{

    using TestCase = TestCards.TestCase;
    using Click = TestCards.Click;
    using ClickRow = TestCards.ClickRow;
    using System.Collections.Generic;
    public class TestDecoys : TestCaseCollection
    {
        public override List<TestCase> getCases()
        {
            return new List<TestCase>(){

            new TestCase
            {
                testName = "Jester",
                playerHandCount = 1,
                clicks = new List<Click>{
                    new Click("Jester", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.EnemyMelee, true),
                    new Click("Knight", RowEffected.EnemyMelee, RowEffected.PlayerHand, RowEffected.PlayerHand, true)
                }
            },

             new TestCase
            {
                testName = "Retreat",
                playerHandCount = 1,
                clicks = new List<Click>{
                    new Click("Retreat", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Knight", RowEffected.PlayerMelee, RowEffected.PlayerHand, RowEffected.PlayerHand, true)
                }
            },

             new TestCase
            {
                testName = "Sack",
                playerHandCount = 1,
                clicks = new List<Click>{
                    new Click("Sack", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.EnemyMelee, true),
                    new Click("Knight", RowEffected.EnemyMelee, RowEffected.PlayerHand, RowEffected.PlayerHand, true)
                }
            },

             new TestCase
            {
                testName = "Shipwreck",
                playerHandCount = 2,
                clicks = new List<Click>{
                    new Click("Shipwreck", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Knight", RowEffected.PlayerMelee, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, true),
                    new Click("Soldier", RowEffected.PlayerRanged, RowEffected.PlayerHand, RowEffected.PlayerHand, true),
                    new Click("Soldier2", RowEffected.PlayerRanged, RowEffected.PlayerHand, RowEffected.PlayerHand, true)
                }
            },

            new TestCase
            {
                testName = "ShipwreckSkip",
                playerHandCount = 1,
                clicks = new List<Click>{
                    new Click("Shipwreck", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Knight", RowEffected.PlayerMelee, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, true),
                    new Click("Soldier", RowEffected.PlayerRanged, RowEffected.PlayerHand, RowEffected.PlayerHand, true),
                    new Click("Soldier2", RowEffected.PlayerRanged, RowEffected.PlayerRanged, RowEffected.PlayerRanged, false),
                    new ClickRow("Skip", RowEffected.Skip)
                }
            },
             new TestCase
            {
                testName = "ShipwreckHalfAvailable",
                playerHandCount = 1,
                clicks = new List<Click>{
                    new Click("Shipwreck", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Knight", RowEffected.PlayerMelee, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, true),
                    new Click("Soldier", RowEffected.PlayerRanged, RowEffected.PlayerHand, RowEffected.PlayerHand, true)
                }
            },

            new TestCase
            {
                testName = "Feint",
                playerHandCount = 1,
                clicks = new List<Click>{
                    new Click("Feint", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Knight", RowEffected.PlayerMelee, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, true),
                    new Click("Soldier", RowEffected.PlayerRanged, RowEffected.PlayerHand, RowEffected.PlayerHand, true)
                }
            },
            new TestCase
            {
                testName = "Fortress",
                clicks = new List<Click>{
                    new Click("Fortress", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Knight", RowEffected.PlayerMelee, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                },
                scoreRows = new List<(RowEffected, int)>(){(RowEffected.PlayerMelee, 1)}
            },
            new TestCase
            {
                testName = "FortressScoring",
                clicks = new List<Click>{
                    new Click("Fortress", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new Click("Armada", RowEffected.PlayerSiege, RowEffected.PlayerSiege, RowEffected.PlayerSiege, true),
                },
                scoreRows = new List<(RowEffected, int)>(){(RowEffected.PlayerSiege, 2)}
            }
        };
        }
    }
}