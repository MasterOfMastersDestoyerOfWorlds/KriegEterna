
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
                testName = "JesterEnemy",
                playerHandCount = 1,
                clicks = new List<Click>{
                    new Click("Jester", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.EnemyMelee, true),
                    new Click("Knight", RowEffected.PlayerMelee, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, true),
                    new Click("Knight2", RowEffected.EnemyMelee, RowEffected.PlayerHand, RowEffected.PlayerHand, true)
                }
            },
             new TestCase
            {
                testName = "JesterPlayer",
                playerHandCount = 1,
                clicks = new List<Click>{
                    new Click("Jester", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.EnemyMelee, true),
                    new Click("Knight", RowEffected.PlayerMelee, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, true),
                    new Click("Knight2", RowEffected.PlayerMelee, RowEffected.PlayerHand, RowEffected.PlayerHand, true)
                }
            },

             new TestCase
            {
                testName = "Retreat",
                playerHandCount = 1,
                clicks = new List<Click>{
                    new Click("Retreat", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Knight", RowEffected.PlayerMelee, RowEffected.PlayerSetAside, RowEffected.PlayerMelee, true),
                    new Click("Soldier", RowEffected.PlayerRanged, RowEffected.PlayerSetAside, RowEffected.PlayerRanged, true),
                    new Click("Soldier2", RowEffected.PlayerRanged, RowEffected.PlayerSetAside, RowEffected.PlayerRanged, true),
                    new ClickRow("Pass", RowEffected.Pass),
                }
            },

            new TestCase
            {
                testName = "RetreatSkip",
                playerHandCount = 1,
                clicks = new List<Click>{
                    new Click("Retreat", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Soldier", RowEffected.PlayerRanged, RowEffected.PlayerSetAside, RowEffected.PlayerRanged, true),
                    new Click("Soldier2", RowEffected.PlayerRanged, RowEffected.PlayerRanged, RowEffected.PlayerRanged, false),
                    new ClickRow("Skip", RowEffected.Skip),
                    new ClickRow("Pass", RowEffected.Pass),
                }
            },
             new TestCase
            {
                testName = "RetreatHalfAvailable",
                playerHandCount = 1,
                clicks = new List<Click>{
                    new Click("Retreat", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Knight", RowEffected.PlayerMelee, RowEffected.PlayerSetAside, RowEffected.PlayerMelee, true),
                    new Click("Soldier", RowEffected.PlayerRanged, RowEffected.PlayerSetAside, RowEffected.PlayerRanged, true),
                    new ClickRow("Pass", RowEffected.Pass),
                }
            },

             new TestCase
            {
                testName = "SackEnemy",
                playerHandCount = 1,
                clicks = new List<Click>{
                    new Click("Sack", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.EnemyMelee, true),
                    new Click("Knight", RowEffected.PlayerMelee, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, true),
                    new Click("Knight2", RowEffected.EnemyMelee, RowEffected.PlayerHand, RowEffected.PlayerHand, true)
                }
            },
             new TestCase
            {
                testName = "SackPlayer",
                playerHandCount = 1,
                clicks = new List<Click>{
                    new Click("Sack", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.EnemyMelee, true),
                    new Click("Knight", RowEffected.PlayerMelee, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, true),
                    new Click("Knight2", RowEffected.PlayerMelee, RowEffected.PlayerHand, RowEffected.PlayerHand, true)
                }
            },

             new TestCase
            {
                testName = "Shipwreck",
                playerHandCount = 2,
                clicks = new List<Click>{
                    new Click("Shipwreck", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Knight", RowEffected.PlayerMelee, RowEffected.PlayerSetAside, RowEffected.PlayerMelee, true),
                    new Click("Soldier", RowEffected.PlayerRanged, RowEffected.PlayerSetAside, RowEffected.PlayerRanged, true),
                    new Click("Soldier2", RowEffected.PlayerRanged, RowEffected.PlayerSetAside, RowEffected.PlayerRanged, true),
                    new ClickRow("Pass", RowEffected.Pass),
                }
            },
            new TestCase
            {
                testName = "ShipwreckSkip",
                playerHandCount = 1,
                clicks = new List<Click>{
                    new Click("Shipwreck", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Soldier", RowEffected.PlayerRanged, RowEffected.PlayerSetAside, RowEffected.PlayerRanged, true),
                    new Click("Soldier2", RowEffected.PlayerRanged, RowEffected.PlayerRanged, RowEffected.PlayerRanged, false),
                    new ClickRow("Skip", RowEffected.Skip),
                    new ClickRow("Pass", RowEffected.Pass),
                }
            },
             new TestCase
            {
                testName = "ShipwreckHalfAvailable",
                playerHandCount = 1,
                clicks = new List<Click>{
                    new Click("Shipwreck", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Knight", RowEffected.PlayerMelee, RowEffected.PlayerSetAside, RowEffected.PlayerMelee, true),
                    new Click("Soldier", RowEffected.PlayerRanged, RowEffected.PlayerSetAside, RowEffected.PlayerRanged, true),
                    new ClickRow("Pass", RowEffected.Pass),
                }
            },

            new TestCase
            {
                testName = "Feint",
                playerHandCount = 1,
                clicks = new List<Click>{
                    new Click("Feint", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Knight", RowEffected.PlayerMelee, RowEffected.PlayerSetAside, RowEffected.PlayerMelee, true),
                    new Click("Soldier", RowEffected.PlayerRanged, RowEffected.PlayerSetAside, RowEffected.PlayerRanged, true),
                    new ClickRow("Pass", RowEffected.Pass),
                }
            },
            new TestCase
            {
                testName = "FeintSkip",
                playerHandCount = 1,
                clicks = new List<Click>{
                    new Click("Feint", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Soldier", RowEffected.PlayerRanged, RowEffected.PlayerSetAside, RowEffected.PlayerRanged, true),
                    new Click("Soldier2", RowEffected.PlayerRanged, RowEffected.PlayerRanged, RowEffected.PlayerRanged, false),
                    new ClickRow("Skip", RowEffected.Skip),
                    new ClickRow("Pass", RowEffected.Pass),
                }
            },
             new TestCase
            {
                testName = "FeintHalfAvailable",
                playerHandCount = 1,
                clicks = new List<Click>{
                    new Click("Feint", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Knight", RowEffected.PlayerMelee, RowEffected.PlayerSetAside, RowEffected.PlayerMelee, true),
                    new Click("Soldier", RowEffected.PlayerRanged, RowEffected.PlayerSetAside, RowEffected.PlayerRanged, true),
                    new ClickRow("Pass", RowEffected.Pass),
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