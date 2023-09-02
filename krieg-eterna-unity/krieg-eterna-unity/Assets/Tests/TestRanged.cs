
namespace KriegTests
{

    using TestCase = TestCards.TestCase;
    using Click = TestCards.Click;
    using ClickRow = TestCards.ClickRow;
    using System.Collections;
    using System.Collections.Generic;
    public class TestRanged : TestCaseCollection
    {
        public override List<TestCase> getCases()
        {
            return new List<TestCase>(){
            new TestCase
            {
                testName = "Archer",
                clicks = new List<Click>{
                    new Click("Archer", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Archer", RowEffected.PlayerHand, RowEffected.PlayerRanged, RowEffected.PlayerRanged, true),
                }
            },
            new TestCase
            {
                testName = "Archer2",
                clicks = new List<Click>{
                    new Click("Archer2", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Archer2", RowEffected.PlayerHand, RowEffected.PlayerRanged, RowEffected.PlayerRanged, true),
                }
            },
            new TestCase
            {
                testName = "Bowman",
                clicks = new List<Click>{
                    new Click("Bowman", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Bowman", RowEffected.PlayerHand, RowEffected.PlayerRanged, RowEffected.PlayerRanged, true),
                }
            },
            new TestCase
            {
                testName = "Bowman2",
                clicks = new List<Click>{
                    new Click("Bowman2", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Bowman2", RowEffected.PlayerHand, RowEffected.PlayerRanged, RowEffected.PlayerRanged, true),
                }
            },
            new TestCase
            {
                testName = "Grenadier",
                clicks = new List<Click>{
                    new Click("Grenadier", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Grenadier", RowEffected.PlayerHand, RowEffected.PlayerRanged, RowEffected.PlayerRanged, true),
                }
            },
            new TestCase
            {
                testName = "Grenadier2",
                clicks = new List<Click>{
                    new Click("Grenadier2", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Grenadier2", RowEffected.PlayerHand, RowEffected.PlayerRanged, RowEffected.PlayerRanged, true),
                }
            },
            new TestCase
            {
                testName = "Militia",
                clicks = new List<Click>{
                    new Click("Militia", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Militia", RowEffected.PlayerHand, RowEffected.PlayerRanged, RowEffected.PlayerRanged, true),
                }
            },
            new TestCase
            {
                testName = "Musketeer3",
                clicks = new List<Click>{
                    new Click("Musketeer3", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Musketeer3", RowEffected.PlayerHand, RowEffected.PlayerRanged, RowEffected.PlayerRanged, true),
                }
            },
            new TestCase
            {
                testName = "Musketeer",
                clicks = new List<Click>{
                    new Click("Musketeer", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Musketeer", RowEffected.PlayerHand, RowEffected.PlayerRanged, RowEffected.PlayerRanged, true),
                }
            },
            new TestCase
            {
                testName = "Musketeer2",
                clicks = new List<Click>{
                    new Click("Musketeer2", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Musketeer2", RowEffected.PlayerHand, RowEffected.PlayerRanged, RowEffected.PlayerRanged, true),
                }
            },
            new TestCase
            {
                testName = "Officer2",
                clicks = new List<Click>{
                    new Click("Officer2", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Officer2", RowEffected.PlayerHand, RowEffected.PlayerRanged, RowEffected.PlayerRanged, true),
                }
            },
            new TestCase
            {
                testName = "Privateer",
                clicks = new List<Click>{
                    new Click("Privateer", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Privateer", RowEffected.PlayerHand, RowEffected.PlayerRanged, RowEffected.PlayerRanged, true),
                }
            },
            new TestCase
            {
                testName = "Privateer2",
                clicks = new List<Click>{
                    new Click("Privateer2", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Privateer2", RowEffected.PlayerHand, RowEffected.PlayerRanged, RowEffected.PlayerRanged, true),
                }
            },
            new TestCase
            {
                testName = "Privateer3",
                clicks = new List<Click>{
                    new Click("Privateer3", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Privateer3", RowEffected.PlayerHand, RowEffected.PlayerRanged, RowEffected.PlayerRanged, true),
                }
            },
            new TestCase
            {
                testName = "Privateer4",
                clicks = new List<Click>{
                    new Click("Privateer4", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Privateer4", RowEffected.PlayerHand, RowEffected.PlayerRanged, RowEffected.PlayerRanged, true),
                }
            },
            new TestCase
            {
                testName = "Riflemen",
                clicks = new List<Click>{
                    new Click("Riflemen", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Riflemen", RowEffected.PlayerHand, RowEffected.PlayerRanged, RowEffected.PlayerRanged, true),
                }
            },
            new TestCase
            {
                testName = "Soldier",
                clicks = new List<Click>{
                    new Click("Soldier", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Soldier", RowEffected.PlayerHand, RowEffected.PlayerRanged, RowEffected.PlayerRanged, true),
                }
            },
            new TestCase
            {
                testName = "Soldier2",
                clicks = new List<Click>{
                    new Click("Soldier2", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Soldier2", RowEffected.PlayerHand, RowEffected.PlayerRanged, RowEffected.PlayerRanged, true),
                }
            },
            new TestCase
            {
                testName = "Telescope",
                clicks = new List<Click>{
                    new Click("Telescope", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Telescope", RowEffected.PlayerHand, RowEffected.PlayerRanged, RowEffected.PlayerRanged, true),
                }
            },
            new TestCase
            {
                testName = "TelescopeOneCard",
                scoreRows = new List<(RowEffected, int)>(){(RowEffected.PlayerRanged, 4)},
                clicks = new List<Click>{
                    new Click("Telescope", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Soldier2", RowEffected.PlayerRanged, RowEffected.PlayerRanged, RowEffected.PlayerRanged, true),
                }
            },
            new TestCase
            {
                testName = "TelescopeTwoCards",
                scoreRows = new List<(RowEffected, int)>(){(RowEffected.PlayerRanged, 13)},
                clicks = new List<Click>{
                    new Click("Telescope", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Soldier2", RowEffected.PlayerRanged, RowEffected.PlayerRanged, RowEffected.PlayerRanged, true),
                    new Click("Soldier", RowEffected.PlayerRanged, RowEffected.PlayerRanged, RowEffected.PlayerRanged, true),
                }
            },
            new TestCase
            {
                testName = "TelescopeSkipOne",
                scoreRows = new List<(RowEffected, int)>(){(RowEffected.PlayerRanged, 6)},
                clicks = new List<Click>{
                    new Click("Telescope", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Soldier2", RowEffected.PlayerRanged, RowEffected.PlayerRanged, RowEffected.PlayerRanged, true),
                    new ClickRow("Skip", RowEffected.Skip),
                    new Click("Soldier", RowEffected.PlayerRanged, RowEffected.PlayerRanged, RowEffected.PlayerRanged, false),
                }
            },
            new TestCase
            {
                testName = "Waterworks",
                clicks = new List<Click>{
                    new Click("Waterworks", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Waterworks", RowEffected.PlayerHand, RowEffected.PlayerRanged, RowEffected.PlayerRanged, true),
                }
            },
        };
        }
    }
}