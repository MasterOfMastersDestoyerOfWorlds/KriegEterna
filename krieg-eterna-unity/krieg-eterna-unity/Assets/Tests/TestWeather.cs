
namespace KriegTests
{

    using TestCase = TestCards.TestCase;
    using Click = TestCards.Click;
    using ClickRow = TestCards.ClickRow;
    using ClickAltEffect = TestCards.ClickAltEffect;
    using System.Collections.Generic;
    public class TestWeather : TestCaseCollection
    {
        public override List<TestCase> getCases()
        {
            return new List<TestCase>(){
            new TestCase
            {
                testName = "ClearSkiesClearWeather",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.EnemySiege, 2),
                    (RowEffected.PlayerSiege, 6),
                    (RowEffected.PlayerRanged, 8),
                },
                clicks = new List<Click>{
                    new Click("Storm", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("Player Siege", RowEffected.PlayerSiege),
                    new Click("Siege", RowEffected.EnemySiege, RowEffected.EnemySiege, RowEffected.EnemySiege, false),
                    new Click("Mortar", RowEffected.PlayerSiege, RowEffected.PlayerSiege, RowEffected.PlayerSiege, false),
                    new Click("Fog", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("Enemy Ranged", RowEffected.EnemyRanged),
                    new Click("Soldier", RowEffected.PlayerRanged, RowEffected.PlayerRanged, RowEffected.PlayerRanged, false),
                    new Click("Soldier2", RowEffected.PlayerRanged, RowEffected.PlayerRanged, RowEffected.PlayerRanged, false),
                    new Click("ClearSkies", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickAltEffect("ClearSkiesClearWeather", RowEffected.PlayerAltEffectRow),
                }
            },
            new TestCase
            {
                testName = "FogRanged",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.PlayerRanged, 4),
                },
                clicks = new List<Click>{
                    new Click("Fog", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new ClickRow("Player Ranged", RowEffected.PlayerRanged),
                    new Click("Fog", RowEffected.PlayerHand, RowEffected.PlayerRanged, RowEffected.EnemyRanged, false),
                    new Click("Soldier", RowEffected.PlayerRanged, RowEffected.PlayerRanged, RowEffected.PlayerRanged, false),
                    new Click("Soldier2", RowEffected.PlayerRanged, RowEffected.PlayerRanged, RowEffected.PlayerRanged, false),
                }
            },
                new TestCase
            {
                testName = "FogSiege",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.EnemySiege, 1),
                    (RowEffected.PlayerSiege, 3),
                },
                clicks = new List<Click>{
                    new Click("Fog", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new ClickRow("Enemy Siege", RowEffected.EnemySiege),
                    new Click("Fog", RowEffected.PlayerHand, RowEffected.PlayerSiege, RowEffected.EnemySiege, false),
                    new Click("Siege", RowEffected.EnemySiege, RowEffected.EnemySiege, RowEffected.EnemySiege, false),
                    new Click("Mortar", RowEffected.PlayerSiege, RowEffected.PlayerSiege, RowEffected.PlayerSiege, false),
                }
            },
            new TestCase
            {
                testName = "FrostMelee",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.EnemyMelee, 2),
                },
                clicks = new List<Click>{
                    new Click("Frost", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new ClickRow("Player Melee", RowEffected.PlayerMelee),
                    new Click("Frost", RowEffected.PlayerHand, RowEffected.PlayerMelee, RowEffected.EnemyMelee, false),
                    new Click("Knight3", RowEffected.EnemyMelee, RowEffected.EnemyMelee, RowEffected.EnemyMelee, false),
                    new Click("Officer", RowEffected.EnemyMelee, RowEffected.EnemyMelee, RowEffected.EnemyMelee, false),
                }
            },
            new TestCase
            {
                testName = "FrostRanged",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.PlayerRanged, 4),
                },
                clicks = new List<Click>{
                    new Click("Frost", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new ClickRow("Player Ranged", RowEffected.PlayerRanged),
                    new Click("Frost", RowEffected.PlayerHand, RowEffected.PlayerRanged, RowEffected.EnemyRanged, false),
                    new Click("Soldier", RowEffected.PlayerRanged, RowEffected.PlayerRanged, RowEffected.PlayerRanged, false),
                    new Click("Soldier2", RowEffected.PlayerRanged, RowEffected.PlayerRanged, RowEffected.PlayerRanged, false),
                }
            },
            new TestCase
            {
                testName = "StormSiege",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.EnemySiege, 1),
                    (RowEffected.PlayerSiege, 3),
                },
                clicks = new List<Click>{
                    new Click("Storm", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new ClickRow("Enemy Siege", RowEffected.EnemySiege),
                    new Click("Storm", RowEffected.PlayerHand, RowEffected.PlayerSiege, RowEffected.EnemySiege, false),
                    new Click("Siege", RowEffected.EnemySiege, RowEffected.EnemySiege, RowEffected.EnemySiege, false),
                    new Click("Mortar", RowEffected.PlayerSiege, RowEffected.PlayerSiege, RowEffected.PlayerSiege, false),
                }
            },
            new TestCase
            {
                testName = "StormMelee",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.EnemyMelee, 2),
                },
                clicks = new List<Click>{
                    new Click("Storm", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new ClickRow("Player Melee", RowEffected.PlayerMelee),
                    new Click("Storm", RowEffected.PlayerHand, RowEffected.PlayerMelee, RowEffected.EnemyMelee, false),
                    new Click("Knight3", RowEffected.EnemyMelee, RowEffected.EnemyMelee, RowEffected.EnemyMelee, false),
                    new Click("Officer", RowEffected.EnemyMelee, RowEffected.EnemyMelee, RowEffected.EnemyMelee, false),
                }
            },

            new TestCase
            {
                testName = "OmenNoRoundChange",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.EnemySiege, 2),
                    (RowEffected.PlayerSiege, 6),
                },
                clicks = new List<Click>{
                    new Click("Omen", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new ClickRow("Enemy Siege", RowEffected.EnemySiege),
                    new Click("Omen", RowEffected.PlayerHand, RowEffected.PlayerSiege, RowEffected.EnemySiege, false),
                    new Click("Siege", RowEffected.EnemySiege, RowEffected.EnemySiege, RowEffected.EnemySiege, false),
                    new Click("Mortar", RowEffected.PlayerSiege, RowEffected.PlayerSiege, RowEffected.PlayerSiege, false),
                }
            },
            new TestCase
            {
                testName = "OmenSiege",
                playerHandCount = 2,
                round = RoundType.RoundTwo,
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.EnemySiege, 6),
                    (RowEffected.PlayerSiege, 12),
                },
                clicks = new List<Click>{
                    new Click("Omen", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new ClickRow("Enemy Siege", RowEffected.EnemySiege),
                    new ClickRow("Pass", RowEffected.Pass),
                    new Click("Mortar", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new Click("Mortar", RowEffected.PlayerHand, RowEffected.PlayerSiege, RowEffected.PlayerSiege, true),
                    new ClickRow("Pass", RowEffected.Pass),
                    new Click("Minister", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.EnemySiege, true),
                    new Click("Minister", RowEffected.PlayerHand, RowEffected.EnemySiege, RowEffected.EnemySiege, true),
                    new ClickRow("UnitDeck", RowEffected.UnitDeck),
                    new ClickRow("UnitDeck", RowEffected.UnitDeck),
                }
            },
            new TestCase
            {
                testName = "OmenMelee",
                playerHandCount = 2,
                round = RoundType.RoundTwo,
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.PlayerMelee, 4),
                    (RowEffected.EnemySiege, 3),
                },
                clicks = new List<Click>{
                    new Click("Omen", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new ClickRow("Player Melee", RowEffected.PlayerMelee),
                    new ClickRow("Pass", RowEffected.Pass),
                    new Click("Knight", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Knight", RowEffected.PlayerHand, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                    new ClickRow("Pass", RowEffected.Pass),
                    new Click("Minister", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.EnemySiege, true),
                    new Click("Minister", RowEffected.PlayerHand, RowEffected.EnemySiege, RowEffected.EnemySiege, true),
                    new ClickRow("UnitDeck", RowEffected.UnitDeck),
                    new ClickRow("UnitDeck", RowEffected.UnitDeck),
                }
            },
            new TestCase
            {
                testName = "EclipseNoRoundChange",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.EnemyMelee, 2),
                    (RowEffected.PlayerMelee, 2),
                },
                clicks = new List<Click>{
                    new Click("Eclipse", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new ClickRow("Enemy Melee", RowEffected.EnemyMelee),
                    new Click("Eclipse", RowEffected.PlayerHand, RowEffected.PlayerMelee, RowEffected.EnemyMelee, false),
                    new Click("Knight", RowEffected.EnemyMelee, RowEffected.EnemyMelee, RowEffected.EnemyMelee, false),
                    new Click("Knight3", RowEffected.PlayerMelee, RowEffected.PlayerMelee, RowEffected.PlayerMelee, false),
                }
            },
            new TestCase
            {
                testName = "EclipseRanged",
                playerHandCount = 2,
                round = RoundType.RoundTwo,
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.EnemyRanged, 4),
                    (RowEffected.PlayerRanged, 4),
                },
                clicks = new List<Click>{
                    new Click("Eclipse", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new ClickRow("Enemy Ranged", RowEffected.EnemyRanged),
                    new ClickRow("Pass", RowEffected.Pass),
                    new Click("Soldier", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Soldier", RowEffected.PlayerHand, RowEffected.PlayerRanged, RowEffected.PlayerRanged, true),
                    new ClickRow("Pass", RowEffected.Pass),
                    new Click("Saboteur", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.EnemyRanged, true),
                    new Click("Saboteur", RowEffected.PlayerHand, RowEffected.EnemyRanged, RowEffected.EnemyRanged, true),
                    new ClickRow("UnitDeck", RowEffected.UnitDeck),
                    new ClickRow("UnitDeck", RowEffected.UnitDeck),
                }
            },
            new TestCase
            {
                testName = "EclipseMelee",
                playerHandCount = 2,
                round = RoundType.RoundTwo,
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.PlayerMelee, 4),
                    (RowEffected.EnemySiege, 3),
                },
                clicks = new List<Click>{
                    new Click("Eclipse", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new ClickRow("Player Melee", RowEffected.PlayerMelee),
                    new ClickRow("Pass", RowEffected.Pass),
                    new Click("Knight", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Knight", RowEffected.PlayerHand, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                    new ClickRow("Pass", RowEffected.Pass),
                    new Click("Minister", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.EnemySiege, true),
                    new Click("Minister", RowEffected.PlayerHand, RowEffected.EnemySiege, RowEffected.EnemySiege, true),
                    new ClickRow("UnitDeck", RowEffected.UnitDeck),
                    new ClickRow("UnitDeck", RowEffected.UnitDeck),
                }
            },

            new TestCase
            {
                testName = "SolsticeNoRoundChange",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.EnemySiege, 2),
                    (RowEffected.PlayerSiege, 6),
                },
                clicks = new List<Click>{
                    new Click("Solstice", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new ClickRow("Enemy Siege", RowEffected.EnemySiege),
                    new Click("Solstice", RowEffected.PlayerHand, RowEffected.PlayerSiege, RowEffected.EnemySiege, false),
                    new Click("Siege", RowEffected.EnemySiege, RowEffected.EnemySiege, RowEffected.EnemySiege, false),
                    new Click("Mortar", RowEffected.PlayerSiege, RowEffected.PlayerSiege, RowEffected.PlayerSiege, false),
                }
            },

                        new TestCase
            {
                testName = "SolsticeRanged",
                playerHandCount = 2,
                round = RoundType.RoundTwo,
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.EnemyRanged, 4),
                    (RowEffected.PlayerRanged, 4),
                },
                clicks = new List<Click>{
                    new Click("Solstice", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new ClickRow("Enemy Ranged", RowEffected.EnemyRanged),
                    new ClickRow("Pass", RowEffected.Pass),
                    new Click("Soldier", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Soldier", RowEffected.PlayerHand, RowEffected.PlayerRanged, RowEffected.PlayerRanged, true),
                    new ClickRow("Pass", RowEffected.Pass),
                    new Click("Saboteur", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.EnemyRanged, true),
                    new Click("Saboteur", RowEffected.PlayerHand, RowEffected.EnemyRanged, RowEffected.EnemyRanged, true),
                    new ClickRow("UnitDeck", RowEffected.UnitDeck),
                    new ClickRow("UnitDeck", RowEffected.UnitDeck),
                }
            },
            new TestCase
            {
                testName = "SolsticeSiege",
                playerHandCount = 2,
                round = RoundType.RoundTwo,
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.EnemySiege, 6),
                    (RowEffected.PlayerSiege, 12),
                },
                clicks = new List<Click>{
                    new Click("Solstice", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new ClickRow("Enemy Siege", RowEffected.EnemySiege),
                    new ClickRow("Pass", RowEffected.Pass),
                    new Click("Mortar", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new Click("Mortar", RowEffected.PlayerHand, RowEffected.PlayerSiege, RowEffected.PlayerSiege, true),
                    new ClickRow("Pass", RowEffected.Pass),
                    new Click("Minister", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.EnemySiege, true),
                    new Click("Minister", RowEffected.PlayerHand, RowEffected.EnemySiege, RowEffected.EnemySiege, true),
                    new ClickRow("UnitDeck", RowEffected.UnitDeck),
                    new ClickRow("UnitDeck", RowEffected.UnitDeck),
                }
            },
                        new TestCase
            {
                testName = "PlagueNoRoundChange",
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.EnemySiege, 2),
                    (RowEffected.PlayerSiege, 6),
                },
                clicks = new List<Click>{
                    new Click("Plague", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.EnemyMelee, true),
                    new ClickRow("Enemy Siege", RowEffected.EnemyMelee),
                    new Click("Siege", RowEffected.EnemySiege, RowEffected.EnemySiege, RowEffected.EnemySiege, false),
                    new Click("Mortar", RowEffected.PlayerSiege, RowEffected.PlayerSiege, RowEffected.PlayerSiege, false),
                }
            },
            new TestCase
            {
                testName = "PlagueRanged",
                round = RoundType.RoundTwo,
                playerHandCount = 2,
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.EnemyRanged, 1),
                    (RowEffected.PlayerRanged, 1),
                },
                clicks = new List<Click>{
                    new Click("Plague", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new ClickRow("Enemy Ranged", RowEffected.EnemyRanged),
                    new ClickRow("Pass", RowEffected.Pass),
                    new Click("Soldier", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Soldier", RowEffected.PlayerHand, RowEffected.PlayerRanged, RowEffected.PlayerRanged, true),
                    new ClickRow("Pass", RowEffected.Pass),
                    new Click("Saboteur", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.EnemyRanged, true),
                    new Click("Saboteur", RowEffected.PlayerHand, RowEffected.EnemyRanged, RowEffected.EnemyRanged, true),
                    new ClickRow("UnitDeck", RowEffected.UnitDeck),
                    new ClickRow("UnitDeck", RowEffected.UnitDeck),
                }
            },
            new TestCase
            {
                testName = "PlagueSiege",
                round = RoundType.RoundTwo,
                scoreRows = new List<(RowEffected, int)>{
                    (RowEffected.PlayerSiege, 2),
                },
                clicks = new List<Click>{
                    new Click("Plague", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new ClickRow("Enemy Siege", RowEffected.EnemySiege),
                    new ClickRow("Pass", RowEffected.Pass),
                    new Click("Mortar", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new Click("Mortar", RowEffected.PlayerHand, RowEffected.PlayerSiege, RowEffected.PlayerSiege, true),
                    new ClickRow("Pass", RowEffected.Pass),
                    new Click("Siege", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerSiege, true),
                    new Click("Siege", RowEffected.PlayerHand, RowEffected.PlayerSiege, RowEffected.PlayerSiege, true),
                }
            },
        };
        }
    }
}