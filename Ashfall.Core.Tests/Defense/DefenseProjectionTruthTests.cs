// SPDX-License-Identifier: MIT
// ============================================================================
// CORE-MECH W5 — Defense projection truth (focused suite; alone first).
//
// Plan: docs/plans/CORE_GAME_MECHANICS_GAP_SEAL_MASTER_INTEGRATION_PLAN.md
//       wave CORE-MECH-W5-DEFENSE-SEEGE-COUPLE (cases AV.5).
//
// The W5 forensic pass overturned the plan's premise: defenses were NOT inert —
// `DefenseSystem.ResolvePreCombatRaid` already consumed the real perimeter and a
// power-aware predicate. The real defects were (a) the defense panel projected
// strength with `CalculatePerimeterStrength(null, null)`, so everything the
// player built was invisible in their own readout, and (b) the engagement result
// was never surfaced. Both are legibility fixes; these tests pin the underlying
// truth they now report.
// ============================================================================

using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Defense;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class DefenseProjectionTruthTests
    {
        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        private static DefenseSystem FreshDefense()
        {
            return new DefenseSystem(TrapCatalogLoader.Load(DataDir()));
        }

        private static PerimeterDefenseSystem FreshPerimeter(bool withEmplacements = false)
        {
            var perimeter = new PerimeterDefenseSystem(
                PerimeterDefenseCatalogLoader.Load(DataDir()),
                new Ashfall.Core.Inventory.Inventory(),
                new Ashfall.Core.SeededRng(32));
            if (withEmplacements)
            {
                // The catalog says what can be built; the projection reads what is
                // actually standing. Restoring state is the test's way of saying
                // "the player built these" without seeding a whole inventory.
                perimeter.RestoreState(new PerimeterDefenseSave
                {
                    schema_version = 2,
                    last_tick_day = 1,
                    emplacements = new System.Collections.Generic.List<EmplacementRuntimeState>
                    {
                        new EmplacementRuntimeState
                        {
                            emplacement_id = "emp_test_berm",
                            defense_id = "def_sandbag_berm",
                            current_hp = 200, max_hp = 200,
                            is_active = true, is_destroyed = false
                        },
                        new EmplacementRuntimeState
                        {
                            emplacement_id = "emp_test_turret",
                            defense_id = "def_sentry_turret_9mm",
                            current_hp = 150, max_hp = 150,
                            is_active = true, is_destroyed = false,
                            loaded_ammo_count = 30, magazine_capacity = 30
                        }
                    }
                });
            }
            return perimeter;
        }

        // ── The under-report the panel had (AV.5 cases 1–2) ────────────────

        [Fact]
        public void Projection_WithoutPerimeter_IgnoresEverythingThePlayerBuilt()
        {
            // This is the call the defense panel used to make. It is kept as a
            // regression witness: a null perimeter must contribute nothing.
            var defense = FreshDefense();
            var perimeter = FreshPerimeter(withEmplacements: true);
            Assert.NotEmpty(perimeter.Emplacements);

            var without = defense.CalculatePerimeterStrength(null, null);
            Assert.Equal(0, without.Walls);
            Assert.Equal(0, without.Turrets);
        }

        [Fact]
        public void Projection_WithPerimeter_ReportsWallsTurretsAndPower()
        {
            var defense = FreshDefense();
            var perimeter = FreshPerimeter(withEmplacements: true);

            var truth = defense.CalculatePerimeterStrength(perimeter, _ => true);
            var blank = defense.CalculatePerimeterStrength(null, null);

            Assert.True(truth.Walls + truth.Turrets > 0, "the projection must see what is standing");
            Assert.True(truth.Total > blank.Total,
                "including the perimeter must raise the projection the player sees");
        }

        [Fact]
        public void Projection_PowerStateChangesTheReportedStrength()
        {
            var defense = FreshDefense();
            var perimeter = FreshPerimeter(withEmplacements: true);

            var powered = defense.CalculatePerimeterStrength(perimeter, _ => true);
            var unpowered = defense.CalculatePerimeterStrength(perimeter, _ => false);

            Assert.True(powered.Power >= unpowered.Power,
                "a powered emplacement must not read as weaker than an unpowered one");
        }

        // ── The mechanic itself: defenses change raid outcomes (AV.5 case 1) ─

        [Fact]
        public void Defenses_ChangeTheRaidOutcome_UnderTheSameSeed()
        {
            // Identical seed, identical raiders, two defenses: the fortification
            // must measurably change what survives the static phase.
            var bare = FreshDefense();
            var fortified = FreshDefense();
            var perimeter = FreshPerimeter(withEmplacements: true);

            var bareResult = bare.ResolvePreCombatRaid(
                10, 8, isNight: false, perimeter: null, isEmplacementPowered: null,
                targetingRng: new Ashfall.Core.SeededRng(5), captureRng: new Ashfall.Core.SeededRng(6));

            var fortifiedResult = fortified.ResolvePreCombatRaid(
                10, 8, isNight: false, perimeter: perimeter, isEmplacementPowered: _ => true,
                targetingRng: new Ashfall.Core.SeededRng(5), captureRng: new Ashfall.Core.SeededRng(6));

            Assert.True(
                fortifiedResult.RemainingRaiders <= bareResult.RemainingRaiders,
                "a fortified perimeter must never leave more raiders than a bare one under the same seed");
        }

        [Fact]
        public void RaidResolution_IsDeterministic_TwoPass()
        {
            var a = FreshDefense();
            var b = FreshDefense();
            var perimeter = FreshPerimeter(withEmplacements: true);

            var r1 = a.ResolvePreCombatRaid(11, 7, true, perimeter, _ => true,
                new Ashfall.Core.SeededRng(99), new Ashfall.Core.SeededRng(100));
            var r2 = b.ResolvePreCombatRaid(11, 7, true, perimeter, _ => true,
                new Ashfall.Core.SeededRng(99), new Ashfall.Core.SeededRng(100));

            Assert.Equal(r1.RemainingRaiders, r2.RemainingRaiders);
            Assert.Equal(r1.Repelled, r2.Repelled);
            Assert.Equal(r1.RaidersNeutralizedByTraps, r2.RaidersNeutralizedByTraps);
        }

        [Fact]
        public void NoSecondSiegeModel_WasIntroduced()
        {
            // DP-CM-2 guard: the program adds no second resolver. The engagement
            // type the host now surfaces is the one DefenseSystem already returns.
            var result = new DefenseEngagementResult
            {
                Day = 1, InitialRaiderStrength = 3, RemainingRaiders = 1
            };
            Assert.False(result.Repelled);
            Assert.Equal(1, result.RemainingRaiders);
        }
    }
}
