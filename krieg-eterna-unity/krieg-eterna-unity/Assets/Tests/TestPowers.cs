
namespace KriegTests
{

    using TestCase = TestCards.TestCase;
    using Click = TestCards.Click;
    using ClickRow = TestCards.ClickRow;
    using System.Collections;
    using System.Collections.Generic;
    public class TestPowers
    {
        public static List<TestCase> cases = new List<TestCase>(){
            new TestCase
            {
                testName = "Burden",
                clicks = new List<Click>{
                    new Click("Burden", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("UnitGraveyard", RowEffected.UnitGraveyard),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Mortar", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            },
            new TestCase
            {
                testName = "Crusade",
                clicks = new List<Click>{
                    new Click("Crusade", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("UnitGraveyard", RowEffected.UnitGraveyard),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Mortar", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            },
            new TestCase
            {
                testName = "EmperorDemise",
                clicks = new List<Click>{
                    new Click("EmperorDemise", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("UnitGraveyard", RowEffected.UnitGraveyard),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Mortar", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            },
            new TestCase
            {
                testName = "Enlightenment",
                clicks = new List<Click>{
                    new Click("Enlightenment", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("UnitGraveyard", RowEffected.UnitGraveyard),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Mortar", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            },
            new TestCase
            {
                testName = "Execution",
                clicks = new List<Click>{
                    new Click("Execution", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("UnitGraveyard", RowEffected.UnitGraveyard),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Mortar", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            },
            new TestCase
            {
                testName = "Famine",
                clicks = new List<Click>{
                    new Click("Famine", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("UnitGraveyard", RowEffected.UnitGraveyard),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Mortar", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            },
            new TestCase
            {
                testName = "Fate",
                clicks = new List<Click>{
                    new Click("Fate", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("UnitGraveyard", RowEffected.UnitGraveyard),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Mortar", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            },
            new TestCase
            {
                testName = "Offering",
                clicks = new List<Click>{
                    new Click("Offering", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("UnitGraveyard", RowEffected.UnitGraveyard),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Mortar", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            },
            new TestCase
            {
                testName = "Plague",
                clicks = new List<Click>{
                    new Click("Plague", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("UnitGraveyard", RowEffected.UnitGraveyard),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Mortar", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            },
            new TestCase
            {
                testName = "Redemption",
                clicks = new List<Click>{
                    new Click("Redemption", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("UnitGraveyard", RowEffected.UnitGraveyard),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Mortar", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            },
            new TestCase
            {
                testName = "Relic",
                clicks = new List<Click>{
                    new Click("Relic", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("UnitGraveyard", RowEffected.UnitGraveyard),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Mortar", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            },
            new TestCase
            {
                testName = "Resurrection",
                clicks = new List<Click>{
                    new Click("Resurrection", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("UnitGraveyard", RowEffected.UnitGraveyard),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Mortar", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            },
            new TestCase
            {
                testName = "Ruin",
                clicks = new List<Click>{
                    new Click("Ruin", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("UnitGraveyard", RowEffected.UnitGraveyard),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Mortar", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            },
            new TestCase
            {
                testName = "Styx",
                clicks = new List<Click>{
                    new Click("Styx", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("UnitGraveyard", RowEffected.UnitGraveyard),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Mortar", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            },
            new TestCase
            {
                testName = "Usury",
                clicks = new List<Click>{
                    new Click("Usury", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("UnitGraveyard", RowEffected.UnitGraveyard),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Mortar", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            },
            new TestCase
            {
                testName = "Void",
                clicks = new List<Click>{
                    new Click("Void", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("UnitGraveyard", RowEffected.UnitGraveyard),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Mortar", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            },
            new TestCase
            {
                testName = "War",
                clicks = new List<Click>{
                    new Click("War", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("UnitGraveyard", RowEffected.UnitGraveyard),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Mortar", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            },
            new TestCase
            {
                testName = "Wrath",
                clicks = new List<Click>{
                    new Click("Wrath", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("UnitGraveyard", RowEffected.UnitGraveyard),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Mortar", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            },
            new TestCase
            {
                testName = "Death",
                clicks = new List<Click>{
                    new Click("Death", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new Click("Knight", RowEffected.EnemyMelee, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, true),
                }
            },
            new TestCase
            {
                testName = "Death2",
                clicks = new List<Click>{
                    new Click("Death2", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new Click("Knight", RowEffected.EnemyMelee, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, true),
                }
            },
            new TestCase
            {
                testName = "Feast2",
                clicks = new List<Click>{
                    new Click("Feast2", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Knight", RowEffected.PlayerMelee, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                }
            },
            new TestCase
            {
                testName = "Feast3",
                clicks = new List<Click>{
                    new Click("Feast3", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Knight", RowEffected.PlayerMelee, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                }
            },
            new TestCase
            {
                testName = "Grail",
                clicks = new List<Click>{
                    new Click("Grail", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("UnitGraveyard", RowEffected.UnitGraveyard),
                    new Click("Knight", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                    new Click("Soldier", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            },

            new TestCase
            {
                testName = "Crusader",
                clicks = new List<Click>{
                    new Click("Grail", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("UnitGraveyard", RowEffected.UnitGraveyard),
                    new Click("Soldier", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                    new Click("Crusader", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            },

            new TestCase
            {
                testName = "GrailStrengthConditionFail",
                clicks = new List<Click>{
                    new Click("Grail", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("UnitGraveyard", RowEffected.UnitGraveyard),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Mortar", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            },
        };
    }
}