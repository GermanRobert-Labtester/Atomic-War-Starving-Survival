using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Maritime;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 23 Task 23B — dive-site &amp; maritime mechanic utilization.
    /// Pins the 14-site catalog, catalog-driven dive state (rooms/air/noise),
    /// gear gates, data-driven safes through the live SafeCrackingSystem,
    /// site-scoped psychological contamination, deterministic loot, and
    /// dive-progress persistence.
    /// </summary>
    public class Plan23DiveMechanicCoverageTests
    {
        private static string DataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new InvalidOperationException("StreamingAssets/Data directory not found");
        }

        private static DiveSiteContainer LoadCatalog()
            => DiveSiteCatalogLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());

        [Fact]
        public void Sites_FourteenLive_NoDuplicateIdsOrNames()
        {
            var container = LoadCatalog();
            Assert.Equal(14, container.dive_sites.Count);
            var ids = container.dive_sites.Select(s => s.site_id).ToList();
            Assert.Equal(ids.Count, ids.Distinct(StringComparer.Ordinal).Count());
            var names = container.dive_sites.Select(s => s.name).ToList();
            Assert.Equal(names.Count, names.Distinct(StringComparer.Ordinal).Count());
        }

        [Fact]
        public void Sites_Plan10Profiles_RemainUntouched()
        {
            var container = LoadCatalog();
            var sovereign = DiveSiteCatalogLoader.FindById(container, "site_exp09_ss_sovereign")!;
            Assert.Equal(120, sovereign.oxygen_budget_ticks);
            Assert.Equal(0.5f, sovereign.base_noise_floor, 3);
            Assert.Equal(4, sovereign.rooms.Count);

            var submarine = DiveSiteCatalogLoader.FindById(container, "site_exp09_sunken_submarine")!;
            Assert.Equal(70, submarine.oxygen_budget_ticks);
            Assert.Equal(0.8f, submarine.base_noise_floor, 3);
        }

        [Fact]
        public void Sites_AllHaveLocationAnchorAndDiscovery()
        {
            var container = LoadCatalog();
            foreach (var site in container.dive_sites)
            {
                Assert.False(string.IsNullOrWhiteSpace(site.location_id),
                    $"{site.site_id} has no coastal location anchor");
                Assert.False(string.IsNullOrWhiteSpace(site.discovery),
                    $"{site.site_id} has no discovery path");
            }
        }

        [Fact]
        public void Sites_GearGates_ReferenceRealItems()
        {
            var container = LoadCatalog();
            var gated = container.dive_sites
                .Where(s => !string.IsNullOrEmpty(s.required_item_id)).ToList();
            Assert.True(gated.Count >= 2, "at least two sites should be gear-gated");

            var items = ItemCatalogLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            var known = new HashSet<string>(items.Select(i => i.id), StringComparer.Ordinal);
            foreach (var site in gated)
            {
                Assert.Contains(site.required_item_id, known);
                Assert.InRange(site.required_item_count, 1, 4);
            }
        }

        [Fact]
        public void Dive_StartDiveAtSite_SeedsStateFromCatalogDefinition()
        {
            var dive = new MaritimeDiveSystem(new SeededRng(11));
            dive.LoadCatalog(LoadCatalog());

            Assert.True(dive.StartDiveAtSite("diver", "operator", "site_exp09_sunken_submarine"));
            Assert.Equal("site_exp09_sunken_submarine", dive.CurrentSiteId);
            Assert.Equal(70f, dive.MaxAirSupplySeconds);
            Assert.Equal(4, dive.Rooms.Count);
            Assert.Equal(3, dive.Rooms[0].hazardLevel);

            // Unknown site: rejected, no dive.
            var fresh = new MaritimeDiveSystem(new SeededRng(12));
            fresh.LoadCatalog(LoadCatalog());
            Assert.False(fresh.StartDiveAtSite("diver", "operator", "site_exp09_does_not_exist"));
            Assert.False(fresh.IsActive);
        }

        [Fact]
        public void GearGate_BlocksDeepSiteWithoutCanister_AllowsWithIt()
        {
            var dive = new MaritimeDiveSystem(new SeededRng(13));
            dive.LoadCatalog(LoadCatalog());

            Assert.True(dive.CanStartDive("site_exp09_flooded_metro", Array.Empty<string>(), out var none));
            Assert.Equal(string.Empty, none);

            Assert.False(dive.CanStartDive("site_exp23_brine_cistern", new[] { "item_ash_ghillie" }, out var miss));
            Assert.Equal("item_rebreather_canister", miss);

            Assert.True(dive.CanStartDive("site_exp23_brine_cistern", new[] { "item_rebreather_canister" }, out _));
        }

        [Fact]
        public void Sites_SafeCrackingConsumers_AtLeastTwoSites()
        {
            var container = LoadCatalog();
            var withSafes = container.dive_sites.Where(s => s.safes.Count > 0).ToList();
            Assert.True(withSafes.Count >= 2, "at least two sites must carry real safes");

            foreach (var site in withSafes)
            {
                foreach (var safe in site.safes)
                {
                    Assert.False(string.IsNullOrWhiteSpace(safe.id));
                    Assert.InRange(safe.difficulty, 1, 6);
                    Assert.True(safe.maxAttempts >= 1);
                    Assert.True(safe.loot.Count > 0);
                }
            }
        }

        [Fact]
        public void SafeCracking_SiteSafes_ResolveThroughLiveRuntime_AndPersist()
        {
            var container = LoadCatalog();
            var system = new SafeCrackingSystem(seed: 4242);
            var strongroom = DiveSiteCatalogLoader.FindById(container, "site_exp23_payroll_strongroom")!;

            foreach (var def in strongroom.safes)
                Assert.True(system.RegisterSafe(def, "loc_maritime_icebreaker_dock"));

            var safe = system.GetSafe("safe_exp23_payroll_vault")!;
            Assert.Equal(5, safe.difficulty);

            var instance = system.GetSafe("safe_exp23_payroll_vault")!;
            var feedback = system.Attempt("safe_exp23_payroll_vault", (int[])instance.combination.Clone(), 1f, new SeededRng(7));
            Assert.Equal(SafeAttemptResult.Success, feedback.Result);
            Assert.True(system.IsOpened("safe_exp23_payroll_vault"));

            var loot = system.TransferLoot("safe_exp23_payroll_vault", new SeededRng(99));
            Assert.NotNull(loot);
            Assert.True(loot!.Count > 0);
            foreach (var e in loot)
                Assert.Contains(e.itemId, new[] { "item_claim_tag_stamped", "medical_kit", "ammo_9x19" });

            // Save/load cannot reroll an opened safe.
            var state = system.CaptureState();
            var restored = new SafeCrackingSystem(1);
            restored.RestoreState(state);
            Assert.True(restored.IsOpened("safe_exp23_payroll_vault"));
            Assert.Null(restored.TransferLoot("safe_exp23_payroll_vault", new SeededRng(2)));
        }

        [Fact]
        public void Loot_SiteTables_DeterministicAndVisitTracked()
        {
            var container = LoadCatalog();
            var table = container.dive_sites
                .First(s => s.site_id == "site_exp23_brine_cistern").loot_table;
            Assert.NotEmpty(table);

            var scavengeA = new ProceduralScavengeSystem(new SeededRng(55));
            var rollsA = scavengeA.RollLootTable("probe_site", table.ToList(), 0f, false);
            var scavengeB = new ProceduralScavengeSystem(new SeededRng(55));
            var rollsB = scavengeB.RollLootTable("probe_site", table.ToList(), 0f, false);
            Assert.Equal(rollsA.Count, rollsB.Count);
            for (int i = 0; i < rollsA.Count; i++)
            {
                Assert.Equal(rollsA[i].ItemId, rollsB[i].ItemId);
                Assert.Equal(rollsA[i].Quantity, rollsB[i].Quantity);
            }

            // Visit counts persist across save/load.
            var save = scavengeA.CaptureState();
            var fresh = new ProceduralScavengeSystem(new SeededRng(4242));
            fresh.RestoreState(save);
            Assert.Equal(scavengeA.GetVisitCount("probe_site"), fresh.GetVisitCount("probe_site"));
        }

        [Fact]
        public void Contamination_DiveKeysProduceGroundedEffects_OverworldStaysClean()
        {
            var quarantine = new PsychologicalContaminationSystem();
            quarantine.ApplyContamination("diver_one", "site_exp09_flooded_field_hospital", 50f);
            Assert.True(quarantine.HasContamination("diver_one", PsychologicalContaminationSystem.Contam_ThousandYardStare));

            var cistern = new PsychologicalContaminationSystem();
            cistern.ApplyContamination("diver_two", "site_exp23_brine_cistern", 50f);
            Assert.True(cistern.HasContamination("diver_two", PsychologicalContaminationSystem.Contam_DisgustCascade));

            var visitor = new PsychologicalContaminationSystem();
            visitor.ApplyContamination("visitor", "loc_settlement_cape_beacon", 50f);
            Assert.Null(visitor.GetContaminations("visitor"));
        }

        [Fact]
        public void Dive_ProgressRoundTrip_PreservesOutcomeAndSiteState()
        {
            var dive = new MaritimeDiveSystem(new SeededRng(5));
            dive.LoadCatalog(LoadCatalog());
            Assert.True(dive.StartDiveAtSite("diver", "op", "site_exp23_payroll_strongroom"));
            dive.AdvanceToNextRoom(5);
            dive.EndDive(success: true);

            var save = dive.CaptureState();
            var restored = new MaritimeDiveSystem(new SeededRng(1));
            restored.RestoreState(save);

            var site = restored.Sites.First(s => s.siteId == "site_exp23_payroll_strongroom");
            Assert.True(site.isExplored);
            Assert.Single(save.outcomes);
            Assert.Equal("site_exp23_payroll_strongroom", save.outcomes[0].siteId);
        }
    }
}
