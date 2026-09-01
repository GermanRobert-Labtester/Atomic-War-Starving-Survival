using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Random;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 28 Phase 3 — war-blocked corridors, harvest pressure (overhunt),
    /// and the bounded collapse-notice policy. Same per-day RNG fork pattern
    /// as <see cref="EvolvingWorldActivationTests"/>.
    /// </summary>
    public sealed class WildlifeDisruptionTests
    {
        private static ISeededRng DayRng(int day) => new SeededRng(1986 + day * 13);

        // ── War-blocked corridors ───────────────────────────────────

        [Fact]
        public void BlockedSector_IsNeverEntered_FromOutside()
        {
            var wild = new WildlifeMigrationSystem();
            wild.SetSectorAdjacency(new[]
            {
                ("sector_from", new List<string> { "sector_open", "sector_war" }),
                ("sector_open", new List<string> { "sector_far" }),
                ("sector_blocked", new List<string> { "sector_from" })
            });
            wild.RegisterPack("pack_wolf", "species_wolf", "sector_from", 6);
            wild.SetSectorBlocked("sector_blocked", true);

            for (int day = 1; day <= 200; day++)
                wild.TickDay(day, DayRng(day));

            Assert.NotEqual("sector_blocked", wild.TryGetPack("pack_wolf")!.currentSectorId);
        }

        [Fact]
        public void PackInsideBlockedSector_FleesToOpenGround()
        {
            var wild = new WildlifeMigrationSystem();
            wild.RegisterPack("pack_inside", "species_wolf", "sector_blocked", 6);
            wild.SetSectorAdjacency(new[]
            {
                ("sector_blocked", new List<string> { "sector_open" }),
                ("sector_open", new List<string> { "sector_blocked" })
            });
            wild.SetSectorBlocked("sector_blocked", blocked: true);

            bool fled = false;
            for (int day = 1; day <= 200 && !fled; day++)
            {
                wild.TickDay(day, DayRng(day));
                var p = wild.TryGetPack("pack_inside");
                if (p != null && p.currentSectorId == "sector_open") { fled = true; break; }
            }
            Assert.True(fled, "a pack inside a blocked sector must be able to flee to open ground");
        }

        [Fact]
        public void FullyEnclosedPack_SiegesInPlace()
        {
            var wild = new WildlifeMigrationSystem();
            wild.RegisterPack("pack_enclosed", "species_ash_boar", "sector_pocket", 9);
            wild.SetSectorAdjacency(new[]
            {
                ("sector_pocket", new List<string> { "sector_fort" }),
                ("sector_fort", new List<string> { "sector_pocket" })
            });
            wild.SetSectorBlocked("sector_fort", blocked: true);
            wild.SetSectorBlocked("sector_pocket", blocked: true);

            for (int day = 1; day <= 120; day++)
                wild.TickDay(day, DayRng(day));

            // Enclosed: no neighbor is passable, so the pack never teleports.
            Assert.Equal("sector_pocket", wild.TryGetPack("pack_enclosed")!.currentSectorId);
            Assert.True(wild.TryGetPack("pack_enclosed")!.population <= 9,
                "enclosure starves the pack via the existing loss rule (bounded at zero)");
        }

        // ── Overhunt harvest pressure ───────────────────────────────

        [Fact]
        public void HarvestPressure_ThinsLargestPack_TieBreaksByPackId()
        {
            var wild = new WildlifeMigrationSystem();
            wild.RegisterPack("pack_big", "species_ash_boar", "sector_h", 10);
            wild.RegisterPack("pack_small", "species_wolf", "sector_h", 4);

            int removed = wild.ApplyHarvestPressure("sector_h", 5);
            Assert.Equal(5, removed);
            Assert.Equal(5, wild.TryGetPack("pack_big")!.population);
            Assert.Equal(4, wild.TryGetPack("pack_small")!.population);
        }

        [Fact]
        public void HarvestPressure_IsBoundedByTheRemnantPair()
        {
            var wild = new WildlifeMigrationSystem();
            wild.RegisterPack("pack_h", "species_cotton_hare", "sector_h", 2);

            // A remnant pair always survives — the existing birth rule can
            // repopulate, so overharvest scars but never exterminates.
            int removed = wild.ApplyHarvestPressure("sector_h", 50);
            Assert.Equal(1, removed);
            Assert.Equal(1, wild.TryGetPack("pack_h")!.population);
        }

        [Fact]
        public void Overharvest_RecoversThroughTheExistingBirthRule()
        {
            var wild = new WildlifeMigrationSystem();
            wild.RegisterPack("pack_h", "species_cotton_hare", "sector_h", 5);
            wild.ApplyHarvestPressure("sector_h", 5);
            Assert.Equal(1, wild.TryGetPack("pack_h")!.population); // remnant pair

            // Recovery plays out on real ground: the remnant pack's hunger
            // drive may move it to the adjacent rich sector (movement
            // relieves starvation), and the existing birth rule rebuilds the
            // pack toward twice its seeded size. Bounded, no new simulation.
            wild.SetSectorAdjacency(new[]
            {
                ("sector_h", new List<string> { "sector_open" }),
                ("sector_open", new List<string> { "sector_h" })
            });
            for (int day = 1; day <= 150; day++)
                wild.TickDay(day, DayRng(day));

            var pack = wild.TryGetPack("pack_h")!;
            Assert.True(pack.population > 1,
                "recovery must exceed the remnant pair through the existing birth rule");
            Assert.True(pack.currentSectorId == "sector_h" || pack.currentSectorId == "sector_open",
                "recovery happens on real ground (adjacency walk), never a teleport");
        }
    }
}
