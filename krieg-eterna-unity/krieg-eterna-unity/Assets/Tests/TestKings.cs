
namespace KriegTests
{

    using TestCase = TestCards.TestCase;
    using Click = TestCards.Click;
    using ClickAltEffect = TestCards.ClickAltEffect;
    using ClickRow = TestCards.ClickRow;
    using System.Collections.Generic;
    public class TestKings : TestCaseCollection
    {
        public override List<TestCase> getCases()
        {
            return new List<TestCase>(){

            new TestCase
            {
                testName = "LionKingSetAsideTwo",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.PlayerMelee, 16),
                },
                clicks = new List<Click>{
                    new Click("LionKing", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMeleeKing, true),
                    new ClickAltEffect("LionKingSetAside", RowEffected.PlayerAltEffectRow),
                    new ClickRow("PlayerMeleeKing", RowEffected.PlayerMeleeKing),
                    new Click("Siege", RowEffected.EnemySiege, RowEffected.EnemySetAside, RowEffected.EnemySetAside, true),
                    new Click("Mortar", RowEffected.EnemySiege, RowEffected.EnemySetAside, RowEffected.EnemySetAside, true),
                    new Click("Knight", RowEffected.EnemyMelee, RowEffected.EnemyMelee, RowEffected.EnemyMelee, false),
                    new Click("Knight3", RowEffected.PlayerMelee, RowEffected.PlayerMelee, RowEffected.PlayerMelee, false),
                    new Click("Knight7", RowEffected.PlayerMelee, RowEffected.PlayerMelee, RowEffected.PlayerMelee, false),
                }
            },
            new TestCase
            {
                testName = "LionKingSetAsideOne",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.PlayerMelee, 4),
                },
                clicks = new List<Click>{
                    new Click("LionKing", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMeleeKing, true),
                    new ClickAltEffect("LionKingSetAside", RowEffected.PlayerAltEffectRow),
                    new ClickRow("PlayerMeleeKing", RowEffected.PlayerMeleeKing),
                    new Click("Siege", RowEffected.EnemySiege, RowEffected.EnemySetAside, RowEffected.EnemySetAside, true),
                    new Click("Knight3", RowEffected.PlayerMelee, RowEffected.PlayerMelee, RowEffected.PlayerMelee, false),
                }
            },
            new TestCase
            {
                testName = "LionKingSetAsideZero",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.PlayerMelee, 4),
                },
                clicks = new List<Click>{
                    new Click("LionKing", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMeleeKing, true),
                    new ClickAltEffect("LionKingSetAside", RowEffected.PlayerAltEffectRow),
                    new ClickRow("PlayerMeleeKing", RowEffected.PlayerMeleeKing),
                    new Click("Knight3", RowEffected.PlayerMelee, RowEffected.PlayerMelee, RowEffected.PlayerMelee, false),
                }
            },
            new TestCase
            {
                testName = "LionKingHalveRanged",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.PlayerMelee, 16),
                    (RowEffected.EnemySiege, 8),
                    (RowEffected.EnemyRanged, 1),
                },
                clicks = new List<Click>{
                    new Click("LionKing", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMeleeKing, true),
                    new ClickAltEffect("LionKingHalveRanged", RowEffected.PlayerAltEffectRow),
                    new ClickRow("PlayerMeleeKing", RowEffected.PlayerMeleeKing),
                    new Click("Siege", RowEffected.EnemySiege, RowEffected.EnemySiege, RowEffected.EnemySiege, true),
                    new Click("Mortar", RowEffected.EnemySiege, RowEffected.EnemySiege, RowEffected.EnemySiege, true),
                    new Click("Soldier", RowEffected.EnemyRanged, RowEffected.EnemyRanged, RowEffected.EnemyRanged, true),
                    new Click("Knight", RowEffected.EnemyMelee, RowEffected.EnemyMelee, RowEffected.EnemyMelee, false),
                    new Click("Knight3", RowEffected.PlayerMelee, RowEffected.PlayerMelee, RowEffected.PlayerMelee, false),
                    new Click("Knight7", RowEffected.PlayerMelee, RowEffected.PlayerMelee, RowEffected.PlayerMelee, false),
                }
            },
            new TestCase
            {
                testName = "LionKingNone",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.PlayerMelee, 4),
                    (RowEffected.EnemySiege, 2),
                },
                clicks = new List<Click>{
                    new Click("LionKing", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMeleeKing, true),
                    new ClickAltEffect("LionKingNone", RowEffected.PlayerAltEffectRow),
                    new ClickRow("PlayerMeleeKing", RowEffected.PlayerMeleeKing),
                    new Click("Siege", RowEffected.EnemySiege, RowEffected.EnemySiege, RowEffected.EnemySiege, true),
                    new Click("Knight3", RowEffected.PlayerMelee, RowEffected.PlayerMelee, RowEffected.PlayerMelee, false),
                }
            },
            new TestCase
            {
                testName = "SunKingClearWeather",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.PlayerSiege, 16),
                },
                clicks = new List<Click>{
                    new Click("Storm", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("Player Siege", RowEffected.PlayerSiege),
                    new Click("SunKing", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiegeKing, true),
                    new ClickAltEffect("SunKingClearWeather", RowEffected.PlayerAltEffectRow),
                    new ClickRow("PlayerSiegeKing", RowEffected.PlayerSiegeKing),
                    new Click("Siege", RowEffected.PlayerSiege, RowEffected.PlayerSiege, RowEffected.PlayerSiege, false),
                    new Click("Mortar", RowEffected.PlayerSiege, RowEffected.PlayerSiege, RowEffected.PlayerSiege, false),
                }
            },
            new TestCase
            {
                testName = "SunKingMoveWeather",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.PlayerSiege, 8),
                },
                clicks = new List<Click>{
                    new Click("Storm", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new ClickRow("PlayerMelee", RowEffected.PlayerMelee),
                    new Click("SunKing", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiegeKing, true),
                    new ClickAltEffect("SunKingMoveWeather", RowEffected.PlayerAltEffectRow),
                    new ClickRow("PlayerSiegeKing", RowEffected.PlayerSiegeKing),
                    new Click("Storm", RowEffected.PlayerMelee, RowEffected.PlayerMelee, RowEffected.PlayerSiege, true),
                    new ClickRow("PlayerSiege", RowEffected.PlayerSiege),
                    new Click("Siege", RowEffected.PlayerSiege, RowEffected.PlayerSiege, RowEffected.PlayerSiege, false),
                    new Click("Mortar", RowEffected.PlayerSiege, RowEffected.PlayerSiege, RowEffected.PlayerSiege, false),
                }
            },
            new TestCase
            {
                testName = "SunKingDrawOneUnit",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.PlayerSiege, 16),
                },
                playerHandCount = 1,
                clicks = new List<Click>{
                    new Click("SunKing", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiegeKing, true),
                    new ClickAltEffect("SunKingDraw", RowEffected.PlayerAltEffectRow),
                    new ClickRow("PlayerSiegeKing", RowEffected.PlayerSiegeKing),
                    new ClickRow("Unit Deck", RowEffected.UnitDeck),
                    new Click("Siege", RowEffected.PlayerSiege, RowEffected.PlayerSiege, RowEffected.PlayerSiege, false),
                    new Click("Mortar", RowEffected.PlayerSiege, RowEffected.PlayerSiege, RowEffected.PlayerSiege, false),
                }
            },
            
            new TestCase
            {
                testName = "SunKingDrawOnePower",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.PlayerSiege, 16),
                },
                playerHandCount = 1,
                clicks = new List<Click>{
                    new Click("SunKing", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiegeKing, true),
                    new ClickAltEffect("SunKingDraw", RowEffected.PlayerAltEffectRow),
                    new ClickRow("PlayerSiegeKing", RowEffected.PlayerSiegeKing),
                    new ClickRow("Power Deck", RowEffected.PowerDeck),
                    new Click("Siege", RowEffected.PlayerSiege, RowEffected.PlayerSiege, RowEffected.PlayerSiege, false),
                    new Click("Mortar", RowEffected.PlayerSiege, RowEffected.PlayerSiege, RowEffected.PlayerSiege, false),
                }
            },
            new TestCase
            {
                testName = "TerrorKing",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.PlayerSiege, 6),
                },
                clicks = new List<Click>{
                    new Click("TerrorKing", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiegeKing, true),
                    new ClickRow("PlayerSiegeKing", RowEffected.PlayerSiegeKing),
                    new Click("Siege", RowEffected.EnemySiege, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, true),
                    new Click("Mortar", RowEffected.PlayerSiege, RowEffected.PlayerSiege, RowEffected.PlayerSiege, false),
                }
            },
            new TestCase
            {
                testName = "TraitorKingTwoSwaps",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.PlayerSiege, 6),
                    (RowEffected.EnemyMelee, 8),
                    (RowEffected.PlayerRanged, 4),
                },
                clicks = new List<Click>{
                    new Click("TerrorKing", RowEffected.EnemySiegeKing, RowEffected.EnemySiegeKing, RowEffected.EnemySiegeKing, false),
                    new Click("TraitorKing", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiegeKing, true),
                    new ClickRow("PlayerSiegeKing", RowEffected.PlayerSiegeKing),
                    new Click("Knight3", RowEffected.PlayerMelee, RowEffected.PlayerMelee, RowEffected.EnemyMelee, true),
                    new Click("Mortar", RowEffected.EnemySiege, RowEffected.PlayerSiege, RowEffected.PlayerSiege, true),
                    new Click("Knight", RowEffected.PlayerMelee, RowEffected.PlayerMelee, RowEffected.EnemyMelee, true),
                    new Click("Grenadier", RowEffected.EnemyRanged, RowEffected.PlayerRanged, RowEffected.PlayerRanged, true),
                }
            },
            new TestCase
            {
                testName = "TraitorKingOneSwapPossible",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.EnemyRanged, 4),
                    (RowEffected.PlayerMelee, 2),
                },
                clicks = new List<Click>{
                    new Click("TraitorKing", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRangedKing, true),
                    new ClickRow("PlayerRangedKing", RowEffected.PlayerRangedKing),
                    new Click("Grenadier", RowEffected.PlayerRanged, RowEffected.PlayerRanged, RowEffected.EnemyRanged, true),
                    new Click("Knight", RowEffected.EnemyMelee, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                }
            },
            new TestCase
            {
                testName = "TraitorKingOneSwap",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.PlayerMelee, 4),
                    (RowEffected.EnemyRanged, 4),
                },
                clicks = new List<Click>{
                    new Click("TerrorKing", RowEffected.EnemySiegeKing, RowEffected.EnemySiegeKing, RowEffected.EnemySiegeKing, false),
                    new Click("TraitorKing", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMeleeKing, true),
                    new ClickRow("PlayerMeleeKing", RowEffected.PlayerMeleeKing),
                    new Click("Grenadier", RowEffected.PlayerRanged, RowEffected.PlayerRanged, RowEffected.EnemyRanged, true),
                    new Click("Knight", RowEffected.EnemyMelee, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                }
            },
            new TestCase
            {
                testName = "WinterKing",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.EnemyMelee, 1),
                    (RowEffected.PlayerSiege, 12),
                },
                clicks = new List<Click>{
                    new Click("WinterKing", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiegeKing, true),
                    new ClickRow("PlayerSiegeKing", RowEffected.PlayerSiegeKing),
                    new Click("Knight", RowEffected.EnemyMelee, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, true),
                    new Click("Knight3", RowEffected.EnemyMelee, RowEffected.EnemyMelee, RowEffected.EnemyMelee, false),
                    new Click("Mortar", RowEffected.PlayerSiege, RowEffected.PlayerSiege, RowEffected.PlayerSiege, false),
                }
            },
        };
        }
    }
}