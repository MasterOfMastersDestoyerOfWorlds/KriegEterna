
namespace KriegTests
{

    using TestCase = TestCards.TestCase;
    using Click = TestCards.Click;
    using ClickRow = TestCards.ClickRow;
    using System.Collections;
    using System.Collections.Generic;
    public class TestDecoys
    {
        public static List<TestCase> cases = new List<TestCase>(){

             new TestCase
            {
                testName = "Jester",
                clicks = new List<Click>{
                    new Click("Jester", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.EnemyMelee, true),
                    new Click("Knight", RowEffected.EnemyMelee, RowEffected.PlayerHand, RowEffected.PlayerHand, true)
                }
            },

             new TestCase
            {
                testName = "Retreat",
                clicks = new List<Click>{
                    new Click("Retreat", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Knight", RowEffected.PlayerMelee, RowEffected.PlayerHand, RowEffected.PlayerHand, true)
                }
            },

             new TestCase
            {
                testName = "Sack",
                clicks = new List<Click>{
                    new Click("Sack", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.EnemyMelee, true),
                    new Click("Knight", RowEffected.EnemyMelee, RowEffected.PlayerHand, RowEffected.PlayerHand, true)
                }
            },

             new TestCase
            {
                testName = "Shipwreck",
                clicks = new List<Click>{
                    new Click("Shipwreck", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Knight", RowEffected.PlayerMelee, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, true),
                    new Click("Soldier", RowEffected.PlayerRanged, RowEffected.PlayerHand, RowEffected.PlayerHand, true),
                    new Click("Soldier2", RowEffected.PlayerRanged, RowEffected.PlayerHand, RowEffected.PlayerHand, true)
                }
            },
             new TestCase
            {
                testName = "ShipwreckHalfAvailable",
                clicks = new List<Click>{
                    new Click("Shipwreck", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Knight", RowEffected.PlayerMelee, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, true),
                    new Click("Soldier", RowEffected.PlayerRanged, RowEffected.PlayerHand, RowEffected.PlayerHand, true)
                }
            },

            new TestCase
            {
                testName = "Feint",
                clicks = new List<Click>{
                    new Click("Feint", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("UnitGraveyard", RowEffected.UnitGraveyard),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Mortar", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            },
            new TestCase
            {
                testName = "Fortress",
                clicks = new List<Click>{
                    new Click("Fortress", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("UnitGraveyard", RowEffected.UnitGraveyard),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Mortar", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            }
        };
    }
}