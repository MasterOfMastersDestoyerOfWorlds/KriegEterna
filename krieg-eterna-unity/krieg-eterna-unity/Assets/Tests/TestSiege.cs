
namespace KriegTests
{

    using TestCase = TestCards.TestCase;
    using Click = TestCards.Click;
    using ClickRow = TestCards.ClickRow;
    using System.Collections;
    using System.Collections.Generic;
    public class TestSiege : TestCaseCollection
    {
        public override List<TestCase> getCases()
        {
            return new List<TestCase>(){
            new TestCase
            {
                testName = "Armada",
                clicks = new List<Click>{
                    new Click("Armada", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new Click("Armada", RowEffected.PlayerHand, RowEffected.PlayerSiege, RowEffected.PlayerSiege, true),
                                    }
            },
            new TestCase
            {
                testName = "Armada2",
                clicks = new List<Click>{
                    new Click("Armada2", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new Click("Armada2", RowEffected.PlayerHand, RowEffected.PlayerSiege, RowEffected.PlayerSiege, true),
                                    }
            },
            new TestCase
            {
                testName = "Assault",
                clicks = new List<Click>{
                    new Click("Assault", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new Click("Assault", RowEffected.PlayerHand, RowEffected.PlayerSiege, RowEffected.PlayerSiege, true),
                                    }
            },
            new TestCase
            {
                testName = "Assault2",
                clicks = new List<Click>{
                    new Click("Assault2", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new Click("Assault2", RowEffected.PlayerHand, RowEffected.PlayerSiege, RowEffected.PlayerSiege, true),
                                    }
            },
            new TestCase
            {
                testName = "Assault3",
                clicks = new List<Click>{
                    new Click("Assault3", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new Click("Assault3", RowEffected.PlayerHand, RowEffected.PlayerSiege, RowEffected.PlayerSiege, true),
                                    }
            },
            new TestCase
            {
                testName = "Cannon",
                clicks = new List<Click>{
                    new Click("Cannon", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new Click("Cannon", RowEffected.PlayerHand, RowEffected.PlayerSiege, RowEffected.PlayerSiege, true),
                                    }
            },
            new TestCase
            {
                testName = "Cannon2",
                clicks = new List<Click>{
                    new Click("Cannon2", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new Click("Cannon2", RowEffected.PlayerHand, RowEffected.PlayerSiege, RowEffected.PlayerSiege, true),
                                    }
            },
            new TestCase
            {
                testName = "Cannon3",
                clicks = new List<Click>{
                    new Click("Cannon3", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new Click("Cannon3", RowEffected.PlayerHand, RowEffected.PlayerSiege, RowEffected.PlayerSiege, true),
                                    }
            },
            new TestCase
            {
                testName = "Cannon4",
                clicks = new List<Click>{
                    new Click("Cannon4", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new Click("Cannon4", RowEffected.PlayerHand, RowEffected.PlayerSiege, RowEffected.PlayerSiege, true),
                                    }
            },
            new TestCase
            {
                testName = "Catapult",
                clicks = new List<Click>{
                    new Click("Catapult", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new Click("Catapult", RowEffected.PlayerHand, RowEffected.PlayerSiege, RowEffected.PlayerSiege, true),
                                    }
            },
            new TestCase
            {
                testName = "Mortar",
                clicks = new List<Click>{
                    new Click("Mortar", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new Click("Mortar", RowEffected.PlayerHand, RowEffected.PlayerSiege, RowEffected.PlayerSiege, true),
                                    }
            },
            new TestCase
            {
                testName = "Siege",
                clicks = new List<Click>{
                    new Click("Siege", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new Click("Siege", RowEffected.PlayerHand, RowEffected.PlayerSiege, RowEffected.PlayerSiege, true),
                                    }
            },
            new TestCase
            {
                testName = "Siege2",
                clicks = new List<Click>{
                    new Click("Siege2", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new Click("Siege2", RowEffected.PlayerHand, RowEffected.PlayerSiege, RowEffected.PlayerSiege, true),
                                    }
            },
            new TestCase
            {
                testName = "Siege3",
                clicks = new List<Click>{
                    new Click("Siege3", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new Click("Siege3", RowEffected.PlayerHand, RowEffected.PlayerSiege, RowEffected.PlayerSiege, true),
                                    }
            },

            new TestCase
            {
                testName = "TowerOneCard",
                scoreRows = new List<(RowEffected, int)>(){(RowEffected.PlayerSiege, 4)},
                clicks = new List<Click>{
                    new Click("Tower", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new Click("Siege3", RowEffected.PlayerSiege, RowEffected.PlayerSiege, RowEffected.PlayerSiege, true),
                }
            },
            new TestCase
            {
                testName = "TowerTwoCards",
                scoreRows = new List<(RowEffected, int)>(){(RowEffected.PlayerSiege, 13)},
                clicks = new List<Click>{
                    new Click("Tower", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new Click("Siege3", RowEffected.PlayerSiege, RowEffected.PlayerSiege, RowEffected.PlayerSiege, true),
                    new Click("Siege2", RowEffected.PlayerSiege, RowEffected.PlayerSiege, RowEffected.PlayerSiege, true),
                }
            },
            new TestCase
            {
                testName = "TowerSkipOne",
                scoreRows = new List<(RowEffected, int)>(){(RowEffected.PlayerSiege, 6)},
                clicks = new List<Click>{
                    new Click("Tower", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new Click("Siege3", RowEffected.PlayerSiege, RowEffected.PlayerSiege, RowEffected.PlayerSiege, true),
                    new ClickRow("Skip", RowEffected.Skip),
                    new Click("Siege2", RowEffected.PlayerSiege, RowEffected.PlayerSiege, RowEffected.PlayerSiege, false),
                }
            },
            new TestCase
            {
                testName = "Tower",
                clicks = new List<Click>{
                    new Click("Tower", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new Click("Tower", RowEffected.PlayerHand, RowEffected.PlayerSiege, RowEffected.PlayerSiege, true),
                }
            },
        };
        }
    }
}