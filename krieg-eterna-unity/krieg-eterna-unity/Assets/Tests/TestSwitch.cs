
namespace KriegTests
{

    using TestCase = TestCards.TestCase;
    using Click = TestCards.Click;
    using ClickRow = TestCards.ClickRow;
    using System.Collections.Generic;
    public class TestSwitch : TestCaseCollection
    {
        public override List<TestCase> getCases()
        {
            return new List<TestCase>(){

            new TestCase
            {
                testName = "Juggernaut",
                clicks = new List<Click>{
                    new Click("Juggernaut", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new ClickRow("PlayerRanged", RowEffected.PlayerRanged),
                }
            },
            new TestCase
            {
                testName = "OfficerMelee",
                clicks = new List<Click>{
                    new Click("Officer", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new ClickRow("PlayerMelee", RowEffected.PlayerMelee),
                }
            },
            new TestCase
            {
                testName = "OfficerSiege",
                clicks = new List<Click>{
                    new Click("Officer", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new ClickRow("PlayerSiege", RowEffected.PlayerSiege),
                }
            },
            new TestCase
            {
                testName = "Officer2Melee",
                clicks = new List<Click>{
                    new Click("Officer2", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new ClickRow("PlayerMelee", RowEffected.PlayerMelee),
                }
            },
            new TestCase
            {
                testName = "Officer2Ranged",
                clicks = new List<Click>{
                    new Click("Officer2", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new ClickRow("PlayerRanged", RowEffected.PlayerRanged),
                }
            },
            new TestCase
            {
                testName = "Officer3Melee",
                clicks = new List<Click>{
                    new Click("Officer3", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new ClickRow("PlayerMelee", RowEffected.PlayerMelee),
                                    }
            },
            new TestCase
            {
                testName = "Officer3Siege",
                clicks = new List<Click>{
                    new Click("Officer3", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new ClickRow("PlayerSiege", RowEffected.PlayerSiege),
                                    }
            },
            new TestCase
            {
                testName = "Officer4Ranged",
                clicks = new List<Click>{
                    new Click("Officer4", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new ClickRow("PlayerRanged", RowEffected.PlayerRanged),
                                    }
            },
                        new TestCase
            {
                testName = "Officer4Siege",
                clicks = new List<Click>{
                    new Click("Officer4", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new ClickRow("PlayerSiege", RowEffected.PlayerSiege),
                                    }
            },
            new TestCase
            {
                testName = "Juggernaut",
                clicks = new List<Click>{
                    new Click("Juggernaut", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new ClickRow("PlayerMelee", RowEffected.PlayerMelee),
                }
            },
        };
        }
    }
}