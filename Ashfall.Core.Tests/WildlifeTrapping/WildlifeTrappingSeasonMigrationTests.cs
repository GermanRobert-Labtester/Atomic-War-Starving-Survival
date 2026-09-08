using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.IO;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Workstreams B + C — seasonal boundary eligibility and the migration
    /// presence gate (flagship trapping tranche).
    ///
    /// AUTHORITATIVE SEASON MAP (weather_seasons.json, schema_version 1 —
    /// resolved by WildlifeSeasonalCalendar.SeasonWindowForDay as "the last
    /// window whose startDay &lt;= day", so each startDay is the FIRST day of
    /// its window and the previous window's last day is startDay - 1):
    ///
    /// Plan 83 ten-window schedule (weather_seasons.json is the authority):
    ///   window_first_thaw     days   0..29
    ///   window_ash_settling   days  30..59
    ///   window_deep_freeze    days  60..89
    ///   window_spring_storms  days  90..119
    ///   window_dry_ash        days 120..149
    ///   window_first_fallout  days 150..179
    ///   window_false_spring   days 180..199
    ///   window_deep_ash       days 200..239
    ///   window_long_winter    days 240..279
    ///   window_black_rain_season days 280..∞ (no wrap; last authored window)
    ///
    /// AUTHORED SEASON/PREY MATRIX (wildlife_trapping_catalog.json):
    ///
    ///   | prey         | Deep Freeze | Thaw | Black Bloom | Turning | High Cold | Ashfall | migration              |
    ///   |--------------|-------------|------|-------------|---------|-----------|---------|------------------------|
    ///   | rabbit       | yes         | yes  | yes         | yes     | yes       | yes     | —                      |
    ///   | fox          | yes         | yes  | yes         | yes     | yes       | yes     | —                      |
    ///   | cotton_hare  | no          | yes  | yes         | yes     | no        | no      | species_cotton_hare    |
    ///   | mirror_carp  | no          | yes  | yes         | no      | no        | no      | species_mirror_carp    |
    ///   | hedgehog     | no          | yes  | yes         | no      | no        | no      | —                      |
    ///   | rat          | yes         | yes  | yes         | yes     | yes       | yes     | species_blight_rat     |
    ///
    /// MIGRATION MAPPING (prey speciesId → PreyDefinition.migrationSpeciesId,
    /// audited from the catalog — keep this comment in sync):
    ///
    ///   cotton_hare → species_cotton_hare     pheasant  → species_ash_gull
    ///   boar        → species_ash_boar        ash_crow  → species_iron_crow
    ///   rat         → species_blight_rat      mirror_carp → species_mirror_carp
    ///   rad_dog     → species_rad_dog
    ///
    /// GATE COMPOSITION: Eligible = SkillGate && SeasonGate && MigrationGate
    /// (logical AND of independent filters — proven by the 2×2 matrix below).
    /// Empty activeSeasons means YEAR-ROUND, not "eligible nowhere".
    /// </summary>
    public sealed class WildlifeTrappingSeasonMigrationTests
    {
        private static string FindDataDir()
        {
            var dir = Directory.GetCurrentDirectory();
            for (int i = 0; i < 10; i++)
            {
                string candidate = Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                dir = Path.GetDirectoryName(dir) ?? dir;
            }
            return "Assets/StreamingAssets/Data";
        }

        private static WildlifeTrappingCatalog Catalog()
        {
            var catalog = WildlifeTrappingCatalogLoader.Load(
                FindDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.NotNull(catalog);
            return catalog!;
        }

        private static SeasonProfileDef SeasonProfile()
        {
            var profile = WeatherProfileLoader.Load(
                FindDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.NotNull(profile);
            return profile!;
        }

        private static WildlifeTrappingSystem MakeSystem(WildlifeTrappingCatalog catalog)
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            catalog.RegisterWith(sys);
            return sys;
        }

        private static WildlifeSelectionContext Context(string seasonWindowId, params string[] presentMigration)
        {
            return new WildlifeSelectionContext
            {
                SeasonWindowId = seasonWindowId,
                PresentMigrationSpecies = new HashSet<string>(presentMigration, StringComparer.Ordinal)
            };
        }

        private static List<string> Eligible(WildlifeTrappingSystem sys, string trapType, float skill = 100f)
            => sys.GetEligibleQuarryIds(baitType: "", trapType: trapType, hunterSkillLevel: skill);

        private static readonly string[] AllWindows =
        {
            WildlifeSeasonalCalendar.SeasonFirstThaw,
            WildlifeSeasonalCalendar.SeasonAshSettling,
            WildlifeSeasonalCalendar.SeasonDeepFreeze,
            WildlifeSeasonalCalendar.SeasonSpringStorms,
            WildlifeSeasonalCalendar.SeasonDryAsh,
            WildlifeSeasonalCalendar.SeasonFirstFallout,
            WildlifeSeasonalCalendar.SeasonFalseSpring,
            WildlifeSeasonalCalendar.SeasonDeepAsh,
            WildlifeSeasonalCalendar.SeasonLongWinter,
            WildlifeSeasonalCalendar.SeasonBlackRainSeason
        };

        private static readonly string[] AllMigrationSpecies =
        {
            "species_cotton_hare", "species_ash_boar", "species_blight_rat",
            "species_iron_crow", "species_ash_gull", "species_mirror_carp", "species_rad_dog"
        };

        // ── B1/B3: authoritative calendar boundaries ────────────────────────

        [Fact]
        public void Calendar_DeepFreezeToSpringStormsBoundary_IsExactlyDay90()
        {
            var profile = SeasonProfile();
            Assert.Equal(WildlifeSeasonalCalendar.SeasonDeepFreeze,
                WildlifeSeasonalCalendar.SeasonWindowForDay(profile, 88).id);
            Assert.Equal(WildlifeSeasonalCalendar.SeasonDeepFreeze,
                WildlifeSeasonalCalendar.SeasonWindowForDay(profile, 89).id); // last deep-freeze day
            Assert.Equal(WildlifeSeasonalCalendar.SeasonSpringStorms,
                WildlifeSeasonalCalendar.SeasonWindowForDay(profile, 90).id); // first wet-storm day
            Assert.Equal(WildlifeSeasonalCalendar.SeasonSpringStorms,
                WildlifeSeasonalCalendar.SeasonWindowForDay(profile, 91).id);
        }

        [Fact]
        public void Calendar_AllWindowBoundaries_NoOffByOne()
        {
            var profile = SeasonProfile();
            // Plan 83 boundaries: 30, 60, 90, 120, 150, 180, 200, 240, 280.
            Assert.Equal(WildlifeSeasonalCalendar.SeasonFirstThaw,
                WildlifeSeasonalCalendar.SeasonWindowForDay(profile, 29).id);
            Assert.Equal(WildlifeSeasonalCalendar.SeasonAshSettling,
                WildlifeSeasonalCalendar.SeasonWindowForDay(profile, 30).id);
            Assert.Equal(WildlifeSeasonalCalendar.SeasonAshSettling,
                WildlifeSeasonalCalendar.SeasonWindowForDay(profile, 59).id);
            Assert.Equal(WildlifeSeasonalCalendar.SeasonDeepFreeze,
                WildlifeSeasonalCalendar.SeasonWindowForDay(profile, 60).id);
            Assert.Equal(WildlifeSeasonalCalendar.SeasonSpringStorms,
                WildlifeSeasonalCalendar.SeasonWindowForDay(profile, 119).id);
            Assert.Equal(WildlifeSeasonalCalendar.SeasonDryAsh,
                WildlifeSeasonalCalendar.SeasonWindowForDay(profile, 120).id);
            Assert.Equal(WildlifeSeasonalCalendar.SeasonDryAsh,
                WildlifeSeasonalCalendar.SeasonWindowForDay(profile, 149).id);
            Assert.Equal(WildlifeSeasonalCalendar.SeasonFirstFallout,
                WildlifeSeasonalCalendar.SeasonWindowForDay(profile, 150).id);
            Assert.Equal(WildlifeSeasonalCalendar.SeasonFirstFallout,
                WildlifeSeasonalCalendar.SeasonWindowForDay(profile, 179).id);
            Assert.Equal(WildlifeSeasonalCalendar.SeasonFalseSpring,
                WildlifeSeasonalCalendar.SeasonWindowForDay(profile, 180).id);
            Assert.Equal(WildlifeSeasonalCalendar.SeasonFalseSpring,
                WildlifeSeasonalCalendar.SeasonWindowForDay(profile, 199).id);
            Assert.Equal(WildlifeSeasonalCalendar.SeasonDeepAsh,
                WildlifeSeasonalCalendar.SeasonWindowForDay(profile, 200).id);
            Assert.Equal(WildlifeSeasonalCalendar.SeasonDeepAsh,
                WildlifeSeasonalCalendar.SeasonWindowForDay(profile, 239).id);
            Assert.Equal(WildlifeSeasonalCalendar.SeasonLongWinter,
                WildlifeSeasonalCalendar.SeasonWindowForDay(profile, 240).id);
            Assert.Equal(WildlifeSeasonalCalendar.SeasonLongWinter,
                WildlifeSeasonalCalendar.SeasonWindowForDay(profile, 279).id);
            Assert.Equal(WildlifeSeasonalCalendar.SeasonBlackRainSeason,
                WildlifeSeasonalCalendar.SeasonWindowForDay(profile, 280).id);
            Assert.Equal(WildlifeSeasonalCalendar.SeasonBlackRainSeason,
                WildlifeSeasonalCalendar.SeasonWindowForDay(profile, 360).id);
        }

        // ── B2 + C5: cotton hare 2×2 gate composition ───────────────────────

        [Fact]
        public void CottonHare_WrongSeason_PresentMigration_Excluded()
        {
            var catalog = Catalog();
            var sys = MakeSystem(catalog);
            sys.SetSelectionContext(Context(WildlifeSeasonalCalendar.SeasonDeepFreeze, "species_cotton_hare"));
            Assert.DoesNotContain("cotton_hare", Eligible(sys, "snare"));
        }

        [Fact]
        public void CottonHare_CorrectSeason_AbsentMigration_Excluded()
        {
            var catalog = Catalog();
            var sys = MakeSystem(catalog);
            sys.SetSelectionContext(Context(WildlifeSeasonalCalendar.SeasonThaw));
            Assert.DoesNotContain("cotton_hare", Eligible(sys, "snare"));
        }

        [Fact]
        public void CottonHare_CorrectSeason_PresentMigration_Eligible()
        {
            var catalog = Catalog();
            var sys = MakeSystem(catalog);
            sys.SetSelectionContext(Context(WildlifeSeasonalCalendar.SeasonThaw, "species_cotton_hare"));
            Assert.Contains("cotton_hare", Eligible(sys, "snare"));
        }

        // ── C1/C2: migration mapping + empty-presence exclusion ────────────

        [Fact]
        public void EmptyMigrationPresence_ExcludesEveryMigrationLinkedPrey_ButKeepsNonMigration()
        {
            var catalog = Catalog();
            var sys = MakeSystem(catalog);
            sys.SetSelectionContext(Context(WildlifeSeasonalCalendar.SeasonThaw)); // most permissive season

            var eligible = Eligible(sys, "snare");
            foreach (var prey in catalog.Prey.Values)
            {
                if (!string.IsNullOrEmpty(prey.migrationSpeciesId))
                    Assert.False(eligible.Contains(prey.speciesId),
                        $"migration-linked prey '{prey.speciesId}' must be absent when no migration pack is present");
            }
            // Non-migration prey keep flowing with empty presence.
            Assert.Contains("rabbit", eligible);
            Assert.Contains("fox", eligible);
            Assert.Contains("hedgehog", eligible);
        }

        [Fact]
        public void EmptyMigrationPresence_ExcludesLinkedPrey_ForEveryTrapType()
        {
            var catalog = Catalog();
            var sys = MakeSystem(catalog);
            sys.SetSelectionContext(Context(WildlifeSeasonalCalendar.SeasonThaw));

            foreach (var trapDef in catalog.Traps.Values)
            {
                var eligible = Eligible(sys, trapDef.trapType);
                foreach (var prey in catalog.Prey.Values)
                {
                    if (!string.IsNullOrEmpty(prey.migrationSpeciesId))
                        Assert.False(eligible.Contains(prey.speciesId),
                            $"trap '{trapDef.trap_id}': migration-linked prey '{prey.speciesId}' must require presence");
                }
            }
        }

        // ── C3: single-species presence ─────────────────────────────────────

        [Fact]
        public void SinglePresence_CottonHare_EnablesHareOnly_NotRat()
        {
            var catalog = Catalog();
            var sys = MakeSystem(catalog);
            sys.SetSelectionContext(Context(WildlifeSeasonalCalendar.SeasonThaw, "species_cotton_hare"));

            var eligible = Eligible(sys, "snare");
            Assert.Contains("cotton_hare", eligible);
            Assert.DoesNotContain("rat", eligible); // rat is migration-linked to species_blight_rat
        }

        [Fact]
        public void SinglePresence_BlightRat_EnablesRat_NotCottonHare()
        {
            var catalog = Catalog();
            var sys = MakeSystem(catalog);
            // Rat is year-round (empty activeSeasons), so deep freeze isolates nothing —
            // thaw keeps the other gates identical for both species.
            sys.SetSelectionContext(Context(WildlifeSeasonalCalendar.SeasonThaw, "species_blight_rat"));

            var eligible = Eligible(sys, "snare");
            Assert.Contains("rat", eligible);
            Assert.DoesNotContain("cotton_hare", eligible);
        }

        // ── C4: non-migration prey are independent of the gate ──────────────

        [Fact]
        public void NonMigrationPrey_RabbitFoxHedgehog_EligibleWithEmptyPresence()
        {
            var catalog = Catalog();
            var sys = MakeSystem(catalog);
            sys.SetSelectionContext(Context(WildlifeSeasonalCalendar.SeasonThaw));

            var snareEligible = Eligible(sys, "snare");
            Assert.Contains("rabbit", snareEligible);
            var deadfallEligible = Eligible(sys, "deadfall");
            Assert.Contains("fox", deadfallEligible);
            Assert.Contains("hedgehog", deadfallEligible);
        }

        // ── B4: year-round prey across every window ─────────────────────────

        [Fact]
        public void Rabbit_YearRound_EligibleInEverySeasonWindow()
        {
            var catalog = Catalog();
            Assert.Empty(catalog.Prey["rabbit"].activeSeasons); // empty = year-round, per catalog
            var sys = MakeSystem(catalog);

            foreach (var window in AllWindows)
            {
                sys.SetSelectionContext(Context(window));
                Assert.True(Eligible(sys, "snare").Contains("rabbit"),
                    $"rabbit (year-round) must stay eligible in {window}");
            }
        }

        // ── B5: mirror carp ──────────────────────────────────────────────────

        [Fact]
        public void MirrorCarp_EligibleThawAndBloom_ExcludedInDeepFreeze_WithPresence()
        {
            var catalog = Catalog();
            var sys = MakeSystem(catalog);

            sys.SetSelectionContext(Context(WildlifeSeasonalCalendar.SeasonDeepFreeze, "species_mirror_carp"));
            Assert.DoesNotContain("mirror_carp", Eligible(sys, "fish_trap"));

            sys.SetSelectionContext(Context(WildlifeSeasonalCalendar.SeasonThaw, "species_mirror_carp"));
            Assert.Contains("mirror_carp", Eligible(sys, "fish_trap"));

            sys.SetSelectionContext(Context(WildlifeSeasonalCalendar.SeasonBlackBloom, "species_mirror_carp"));
            Assert.Contains("mirror_carp", Eligible(sys, "fish_trap"));
        }

        // ── B6: hedgehog (non-migration, season-only) ────────────────────────

        [Fact]
        public void Hedgehog_EligibleThawAndBloom_ExcludedInDeepFreezeAndHighCold()
        {
            var catalog = Catalog();
            Assert.True(string.IsNullOrEmpty(catalog.Prey["hedgehog"].migrationSpeciesId));
            var sys = MakeSystem(catalog);

            sys.SetSelectionContext(Context(WildlifeSeasonalCalendar.SeasonThaw));
            Assert.Contains("hedgehog", Eligible(sys, "deadfall"));

            sys.SetSelectionContext(Context(WildlifeSeasonalCalendar.SeasonBlackBloom));
            Assert.Contains("hedgehog", Eligible(sys, "deadfall"));

            sys.SetSelectionContext(Context(WildlifeSeasonalCalendar.SeasonDeepFreeze));
            Assert.DoesNotContain("hedgehog", Eligible(sys, "deadfall"));

            sys.SetSelectionContext(Context(WildlifeSeasonalCalendar.SeasonHighCold));
            Assert.DoesNotContain("hedgehog", Eligible(sys, "deadfall"));
        }

        // ── B7: no-prey-gap invariant ────────────────────────────────────────

        [Fact]
        public void NoPreyGap_EverySeasonWindow_KeepsSnareCandidateSetNonEmpty()
        {
            var catalog = Catalog();
            var sys = MakeSystem(catalog);

            foreach (var window in AllWindows)
            {
                sys.SetSelectionContext(Context(window, AllMigrationSpecies));
                var eligible = Eligible(sys, "snare");
                Assert.True(eligible.Count > 0,
                    $"season '{window}' produced an empty snare candidate pool — a content edit created a dead zone");
            }
        }

        // ── B8 (comment matrix is encoded above) / integration smoke ───────

        private static List<string> RunSeasonCatchSmoke(
            WildlifeTrappingCatalog catalog, string seasonWindowId, string[] presentMigration, int days)
        {
            var sys = MakeSystem(catalog);
            sys.SetHunterSkill(50f); // cotton_hare needs skill >= 5; default fallback skill is 0
            sys.SetSelectionContext(Context(seasonWindowId, presentMigration));
            var caught = new List<string>();
            var def = catalog.Traps["trap_snare"];

            sys.SetTrap("site_smoke", "bait_scrap_meat", "hunter_1",
                def.trapType, def.trap_id, def.checkIntervalDays, def.durabilityChecks);
            for (int day = 2; day <= days; day++)
            {
                sys.TickDay(day);
                var site = sys.State.trapSites.Single(s => s.siteId == "site_smoke");
                if (site.hasCatch)
                {
                    caught.Add(site.catchSpecies);
                    sys.Butcher("site_smoke");
                    // Re-arm through the replacement contract (pending-catch sites are replaceable).
                    sys.SetTrap("site_smoke", "bait_scrap_meat", "hunter_1",
                        def.trapType, def.trap_id, def.checkIntervalDays, def.durabilityChecks);
                }
                else if (site.isBroken)
                {
                    sys.SetTrap("site_smoke", "bait_scrap_meat", "hunter_1",
                        def.trapType, def.trap_id, def.checkIntervalDays, def.durabilityChecks);
                }
            }
            return caught;
        }

        [Fact]
        public void SeasonSmoke_ThawWithPresence_CottonHareCaughtAtLeastOnce()
        {
            var catalog = Catalog();
            var caught = RunSeasonCatchSmoke(catalog, WildlifeSeasonalCalendar.SeasonThaw,
                new[] { "species_cotton_hare" }, days: 240);
            Assert.NotEmpty(caught);
            Assert.Contains("cotton_hare", caught);
        }

        [Fact]
        public void SeasonSmoke_ThawWithoutPresence_CottonHareNeverCaught()
        {
            var catalog = Catalog();
            var caught = RunSeasonCatchSmoke(catalog, WildlifeSeasonalCalendar.SeasonThaw,
                Array.Empty<string>(), days: 240);
            Assert.NotEmpty(caught);
            Assert.DoesNotContain("cotton_hare", caught);
        }
    }
}
