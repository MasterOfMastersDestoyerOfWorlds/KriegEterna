
namespace KriegTests
{

    using TestCase = TestCards.TestCase;
    using Click = TestCards.Click;
    using ClickRow = TestCards.ClickRow;
    using System.Collections;
    using System.Collections.Generic;
    public class TestSpys
    {
        public static List<TestCase> cases = new List<TestCase>(){

            new TestCase
            {
                testName = "Assassin",
                clicks = new List<Click>{
                    new Click("Assassin", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("UnitGraveyard", RowEffected.UnitGraveyard),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Mortar", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            },
            new TestCase
            {
                testName = "Minister",
                clicks = new List<Click>{
                    new Click("Minister", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("UnitGraveyard", RowEffected.UnitGraveyard),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Mortar", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            },
            new TestCase
            {
                testName = "Saboteur",
                clicks = new List<Click>{
                    new Click("Saboteur", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("UnitGraveyard", RowEffected.UnitGraveyard),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Mortar", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            },
            new TestCase
            {
                testName = "Zealot",
                clicks = new List<Click>{
                    new Click("Zealot", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("UnitGraveyard", RowEffected.UnitGraveyard),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Mortar", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            },
            new TestCase
            {
                testName = "Smuggler",
                clicks = new List<Click>{
                    new Click("Smuggler", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("UnitGraveyard", RowEffected.UnitGraveyard),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Mortar", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            },
            new TestCase
            {
                testName = "Spy",
                clicks = new List<Click>{
                    new Click("Spy", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("UnitGraveyard", RowEffected.UnitGraveyard),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Mortar", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            }
        };
    }
}