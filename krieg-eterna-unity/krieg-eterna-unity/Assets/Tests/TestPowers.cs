
namespace KriegTests
{

    using TestCase = TestCards.TestCase;
    using Click = TestCards.Click;
    using ClickRow = TestCards.ClickRow;
    using System.Collections;
    using System.Collections.Generic;
    public class TestPowers : TestCaseCollection
    {
        public override List<TestCase> getCases()
        {
            return new List<TestCase>(){
            new TestCase
            {
                testName = "BurdenRoundOne",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.PlayerMelee, 2),
                    (RowEffected.EnemyRanged, 8),
                },
                clicks = new List<Click>{
                    new Click("Burden", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.EnemyRanged, true),
                    new ClickRow("EnemyRanged", RowEffected.EnemyRanged),
                    new Click("Grenadier2", RowEffected.EnemyRanged, RowEffected.EnemyRanged, RowEffected.EnemyRanged, false),
                    new Click("Knight3", RowEffected.PlayerMelee, RowEffected.PlayerMelee, RowEffected.PlayerMelee, false),
                    new Click("Knight", RowEffected.EnemyMelee, RowEffected.EnemyMelee, RowEffected.EnemyMelee, false),
                }
            },
            new TestCase
            {
                testName = "BurdenRoundTwo",
                playerHandCount = 1,
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.PlayerRanged, 4),
                },
                round = RoundType.RoundTwo,
                clicks = new List<Click>{
                    new Click("Burden", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new ClickRow("EnemyRanged", RowEffected.EnemyRanged),
                    new Click("Grenadier2", RowEffected.EnemyRanged, RowEffected.EnemyRanged, RowEffected.UnitGraveyard, false),
                    new Click("Knight3", RowEffected.PlayerMelee, RowEffected.PlayerMelee, RowEffected.UnitGraveyard, false),
                    new ClickRow("Pass", RowEffected.Pass),
                    new Click("Soldier2", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerHand, false),
                    new Click("Soldier", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Soldier", RowEffected.PlayerHand, RowEffected.PlayerRanged, RowEffected.PlayerRanged, true),
                }
            },
            new TestCase
            {
                testName = "Crusade",
                clicks = new List<Click>{
                    new Click("Crusade", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new Click("Crusade", RowEffected.PlayerHand, RowEffected.PowerGraveyard, RowEffected.PowerGraveyard, true),
                    new Click("Knight3", RowEffected.PlayerMelee, RowEffected.PlayerMelee, RowEffected.UnitGraveyard, false),
                    new Click("Knight11", RowEffected.EnemyMelee, RowEffected.EnemyMelee, RowEffected.EnemyMelee, false),
                    new Click("Knight12", RowEffected.PlayerMelee, RowEffected.PlayerMelee, RowEffected.UnitGraveyard, false),
                    new Click("Calvary", RowEffected.EnemyMelee, RowEffected.EnemyMelee, RowEffected.UnitGraveyard, false),
                    new Click("Calvary2", RowEffected.PlayerMelee, RowEffected.PlayerMelee, RowEffected.UnitGraveyard, false),
                    new Click("Calvary3", RowEffected.EnemyMelee, RowEffected.EnemyMelee, RowEffected.UnitGraveyard, false),
                    new Click("Grenadier2", RowEffected.PlayerRanged, RowEffected.PlayerRanged, RowEffected.PlayerRanged, false),
                }
            },
            new TestCase
            {
                testName = "EmperorDemisePlayer",
                clicks = new List<Click>{
                    new Click("EmperorDemise", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new Click("LionKing", RowEffected.PlayerRangedKing, RowEffected.PlayerSetAside, RowEffected.PlayerSetAside, true),
                }
            },
            new TestCase
            {
                testName = "EmperorDemiseEnemy",
                clicks = new List<Click>{
                    new Click("EmperorDemise", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new Click("LionKing", RowEffected.EnemyRangedKing, RowEffected.EnemySetAside, RowEffected.EnemySetAside, true),
                }
            },

            new TestCase
            {
                testName = "EmperorDemiseChoice",
                clicks = new List<Click>{
                    new Click("EmperorDemise", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new Click("LionKing", RowEffected.EnemyRangedKing, RowEffected.EnemySetAside, RowEffected.EnemySetAside, true),
                    new Click("TerrorKing", RowEffected.PlayerRangedKing, RowEffected.PlayerRangedKing, RowEffected.PlayerRangedKing, false),
                }
            },
            new TestCase
            {
                testName = "EnlightenmentRevealOne",
                enemyHandCount = 1,
                playerHandCount = 1,
                clicks = new List<Click>{
                    new Click("Enlightenment", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new Click("Enlightenment", RowEffected.PlayerHand, RowEffected.PowerGraveyard, RowEffected.PowerGraveyard, true),
                    new Click(false, "Fate", RowEffected.EnemyHand, RowEffected.EnemyHand, RowEffected.PlayerChooseN, true),
                }
            },
            new TestCase
            {
                testName = "EnlightenmentRevealTwo",
                enemyHandCount = 3,
                playerHandCount = 1,
                clicks = new List<Click>{
                    new Click("Enlightenment", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new Click("Enlightenment", RowEffected.PlayerHand, RowEffected.PowerGraveyard, RowEffected.PowerGraveyard, true),
                    new Click(true, "Fate", RowEffected.EnemyHand, RowEffected.EnemyHand, RowEffected.EnemyHand, false),
                    new Click(false, "Execution", RowEffected.EnemyHand, RowEffected.EnemyHand, RowEffected.PlayerChooseN, true),
                    new Click(false, "Famine", RowEffected.EnemyHand, RowEffected.EnemyHand, RowEffected.PlayerChooseN, false),
                }
            },
            new TestCase
            {
                testName = "ExecutionInHand",
                playerHandCount = 4,
                clicks = new List<Click>{
                    new Click("Execution", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new Click("Execution", RowEffected.PlayerHand, RowEffected.PowerGraveyard, RowEffected.PowerGraveyard, true),
                    new Click("LionKing", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.KingGraveyard, false),
                }
            },
            new TestCase
            {
                testName = "ExecutionOnField",
                playerHandCount = 3,
                clicks = new List<Click>{
                    new Click("Execution", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new Click("Execution", RowEffected.PlayerHand, RowEffected.PowerGraveyard, RowEffected.PowerGraveyard, true),
                    new Click("LionKing", RowEffected.PlayerRangedKing, RowEffected.PlayerRangedKing, RowEffected.KingGraveyard, false),
                }
            },
            new TestCase
            {
                testName = "Famine",
                playerHandCount = 1,
                enemyHandCount = 1,
                clicks = new List<Click>{
                    new Click("Famine", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new Click("Famine", RowEffected.PlayerHand, RowEffected.PowerGraveyard, RowEffected.PowerGraveyard, true),
                    new Click("Calvary", RowEffected.EnemyHand, RowEffected.EnemyHand, RowEffected.UnitGraveyard, false),
                    new Click("Calvary2", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerHand, false),
                    new Click("Mortar", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.UnitGraveyard, false),
                    new Click("Calvary3", RowEffected.EnemyHand, RowEffected.EnemyHand, RowEffected.EnemyHand, false),
                }
            },
            new TestCase
            {
                testName = "FamineAdjacency",
                playerHandCount = 2,
                clicks = new List<Click>{
                    new Click("Famine", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new Click("Famine", RowEffected.PlayerHand, RowEffected.PowerGraveyard, RowEffected.PowerGraveyard, true),
                    new Click("Knight", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerHand, false),
                    new Click("Knight3", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerHand, false),
                    new Click("Officer", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.UnitGraveyard, false),
                }
            },
            new TestCase
            {
                testName = "FateGraveyard",
                playerHandCount = 1,
                clicks = new List<Click>{
                    new Click("Death", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerHand, true),
                    new Click("Knight", RowEffected.EnemyMelee, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, true),
                    new Click("Fate", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new Click("Fate", RowEffected.PlayerHand, RowEffected.PowerGraveyard, RowEffected.PowerGraveyard, true),
                }
            },
            new TestCase
            {
                testName = "OfferingStrengthConditionFail",
                playerHandCount = 2,
                clicks = new List<Click>{
                    new Click("Offering", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new Click("Knight", RowEffected.PlayerMelee, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, true),
                    new ClickRow("UnitDeck", RowEffected.UnitDeck),
                    new ClickRow("UnitDeck", RowEffected.UnitDeck),
                }
            },

            new TestCase
            {
                testName = "OfferingStrengthConditionPass",
                playerHandCount = 3,
                clicks = new List<Click>{
                    new Click("Offering", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new Click("Mortar", RowEffected.PlayerSiege, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, true),
                    new ClickRow("UnitDeck", RowEffected.UnitDeck),
                    new ClickRow("UnitDeck", RowEffected.UnitDeck),
                    new ClickRow("PowerDeck", RowEffected.PowerDeck),
                }
            },
            new TestCase
            {
                testName = "PlagueEnemy",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.EnemyMelee, 1),
                },
                clicks = new List<Click>{
                    new Click("Plague", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.EnemyMelee, true),
                    new Click("Knight11", RowEffected.EnemyMelee, RowEffected.EnemyMelee, RowEffected.EnemyMelee, true),
                }
            },
            new TestCase
            {
                testName = "PlaguePlayer",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.PlayerMelee, 3),
                },
                clicks = new List<Click>{
                    new Click("Plague", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Knight11", RowEffected.PlayerMelee, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                    new Click("Knight12", RowEffected.PlayerMelee, RowEffected.PlayerMelee, RowEffected.PlayerMelee, false),
                }
            },
            new TestCase
            {
                testName = "Redemption",
                playerHandCount = 1,
                clicks = new List<Click>{
                    new Click("Redemption", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new Click("Redemption", RowEffected.PlayerHand, RowEffected.PowerGraveyard, RowEffected.PowerGraveyard, true),
                    new Click("LionKing", RowEffected.PlayerRangedKing, RowEffected.PlayerHand, RowEffected.PlayerHand, false),
                }
            },
            new TestCase
            {
                testName = "RedemptionNoKing",
                playerHandCount = 1,
                clicks = new List<Click>{
                    new Click("Redemption", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new Click("Redemption", RowEffected.PlayerHand, RowEffected.PowerGraveyard, RowEffected.PowerGraveyard, true),
                    new Click("Knight12", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            },
            new TestCase
            {
                testName = "Relic",
                clicks = new List<Click>{
                    new Click("Relic", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new Click("Knight3", RowEffected.PlayerMelee, RowEffected.PlayerMelee, RowEffected.EnemyRanged, true),
                    new ClickRow("EnemyRanged", RowEffected.EnemyRanged),
                }
            },
            new TestCase
            {
                testName = "Resurrection",
                playerHandCount = 1,
                clicks = new List<Click>{
                    new Click("Resurrection", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new Click("Resurrection", RowEffected.PlayerHand, RowEffected.PowerGraveyard, RowEffected.PowerGraveyard, true),
                    new Click("Knight", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Soldier", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                    new Click("Soldier", RowEffected.PlayerChooseN, RowEffected.PlayerHand, RowEffected.PlayerHand, true),
                }
            },
            new TestCase
            {
                testName = "Ruin",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.PlayerMelee, 11),
                },
                clicks = new List<Click>{
                    new Click("Ruin", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Ruin", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Knight", RowEffected.PlayerHand, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Knight3", RowEffected.PlayerHand, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Officer", RowEffected.PlayerHand, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Knight", RowEffected.PlayerChooseN, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, true),
                    new Click("Knight3", RowEffected.PlayerChooseN, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, true),
                    new Click("Officer", RowEffected.PlayerChooseN, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, true),
                    new Click("Knight11", RowEffected.PlayerMelee, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                }
            },

            new TestCase
            {
                testName = "RuinSkip",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.PlayerMelee, 8),
                },
                playerHandCount = 1,
                clicks = new List<Click>{
                    new Click("Ruin", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Ruin", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Knight", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerHand, false),
                    new Click("Knight3", RowEffected.PlayerHand, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Officer", RowEffected.PlayerHand, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Knight3", RowEffected.PlayerChooseN, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, true),
                    new Click("Officer", RowEffected.PlayerChooseN, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, true),
                    new ClickRow("Skip", RowEffected.Skip),
                    new Click("Knight11", RowEffected.PlayerMelee, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                }
            },
            new TestCase
            {
                testName = "Styx",
                playerHandCount = 1,
                clicks = new List<Click>{
                    new Click("Styx", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new Click("Styx", RowEffected.PlayerHand, RowEffected.PowerGraveyard, RowEffected.PowerGraveyard, true),
                    new Click("Feast2", RowEffected.PowerGraveyard, RowEffected.PowerGraveyard, RowEffected.PowerGraveyard, false),
                    new Click("Feast3", RowEffected.PowerGraveyard, RowEffected.PowerGraveyard, RowEffected.PowerGraveyard, false),
                    new Click("Usury", RowEffected.PowerGraveyard, RowEffected.PowerGraveyard, RowEffected.PlayerHand, false),
                    new Click("Usury", RowEffected.PlayerChooseN, RowEffected.PlayerHand, RowEffected.PlayerHand, true),
                }
            },
            new TestCase
            {
                testName = "Usury",
                playerHandCount = 4,
                round = RoundType.RoundTwo,
                clicks = new List<Click>{
                    new Click("Usury", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new Click("Knight3", RowEffected.PlayerMelee, RowEffected.PlayerSetAside, RowEffected.PlayerHand, true),
                    new Click("Knight", RowEffected.PlayerMelee, RowEffected.PlayerSetAside, RowEffected.PlayerHand, true),
                    new ClickRow("Pass", RowEffected.Pass),
                    new ClickRow("UnitDeck", RowEffected.UnitDeck),
                    new ClickRow("PowerDeck", RowEffected.PowerDeck),
                }
            },
            new TestCase
            {
                testName = "Void",
                round = RoundType.RoundTwo,
                clicks = new List<Click>{
                    new Click("Void", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new Click("Void", RowEffected.PlayerHand, RowEffected.PowerGraveyard, RowEffected.PowerGraveyard, true),
                    new Click("Knight11", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.UnitGraveyard, true),
                    new Click("Knight11", RowEffected.PlayerHand, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, true),
                }
            },

            new TestCase
            {
                testName = "VoidEndEarly",
                round = RoundType.RoundTwo,
                clicks = new List<Click>{
                    new Click("Void", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new Click("Void", RowEffected.PlayerHand, RowEffected.PowerGraveyard, RowEffected.PowerGraveyard, true),
                    new ClickRow("Pass", RowEffected.Pass),
                    new Click("Knight11", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Knight11", RowEffected.PlayerHand, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                }
            },
            new TestCase
            {
                testName = "War",
                clicks = new List<Click>{
                    new Click("War", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new Click("War", RowEffected.PlayerHand, RowEffected.PowerGraveyard, RowEffected.PowerGraveyard, true),
                    new Click("Knight3", RowEffected.PlayerMelee, RowEffected.PlayerMelee, RowEffected.PlayerMelee, false),
                    new Click("Soldier2", RowEffected.EnemyRanged, RowEffected.EnemyRanged, RowEffected.UnitGraveyard, false),
                    new Click("Soldier", RowEffected.EnemyRanged, RowEffected.EnemyRanged, RowEffected.UnitGraveyard, false),
                    new Click("Grenadier", RowEffected.EnemyRanged, RowEffected.EnemyRanged, RowEffected.UnitGraveyard, false),
                    new Click("Grenadier2", RowEffected.PlayerRanged, RowEffected.PlayerRanged, RowEffected.UnitGraveyard, false),
                }
            },
            new TestCase
            {
                testName = "Wrath",
                clicks = new List<Click>{
                    new Click("Wrath", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new Click("Knight", RowEffected.PlayerMelee, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, true),
                    new Click("Knight3", RowEffected.EnemyMelee, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, true),
                    new Click("Mortar", RowEffected.EnemySiege, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, true),
                }
            },

            new TestCase
            {
                testName = "WrathOneForOne",
                clicks = new List<Click>{
                    new Click("Wrath", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new Click("Knight", RowEffected.PlayerMelee, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, true),
                    new Click("Knight3", RowEffected.EnemyMelee, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, true),
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
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.PlayerMelee, 4),
                },
                clicks = new List<Click>{
                    new Click("Feast2", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Knight", RowEffected.PlayerMelee, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                }
            },
            new TestCase
            {
                testName = "Feast3",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.EnemyMelee, 4),
                },
                clicks = new List<Click>{
                    new Click("Feast3", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.EnemyMelee, true),
                    new Click("Knight", RowEffected.EnemyMelee, RowEffected.EnemyMelee, RowEffected.EnemyMelee, true),
                }
            },
            new TestCase
            {
                testName = "Grail",
                playerHandCount = 2,
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
                playerHandCount = 3,
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
                playerHandCount = 1,
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
}