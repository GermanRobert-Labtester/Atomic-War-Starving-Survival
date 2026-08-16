using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Crossing;
using Ashfall.Core.Foundry;
using Ashfall.Core.Legacy;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// The Silent Foundry (Expansion 10) — Core behavior tests.
    /// Identity, catalog, blueprint/treaty anchors, smelter repair, the 4-day
    /// maintenance cycle, green-sand casting, heavy-alloy fabrication, quality,
    /// safety/incidents (same-seed deterministic), treaty compliance, labor
    /// dispute/strike, journal triggers + dedup, save round-trip + migration.
    /// </summary>
    public sealed class SilentFoundrySystemTests
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        // -----------------------------------------------------------------
        // Helpers
        // -----------------------------------------------------------------

        private sealed class TestHarness
        {
            public readonly Dictionary<string, int> Inventory = new Dictionary<string, int>();
            public readonly SilentFoundrySystem Sys;
            public readonly SilentFoundryCatalog Catalog;
            public readonly List<FoundryJournalTrigger> JournalTriggers = new List<FoundryJournalTrigger>();
            public readonly List<FoundryIncidentRecord> Incidents = new List<FoundryIncidentRecord>();
            public readonly List<FoundryTreatyCompliance> QuotaMet = new List<FoundryTreatyCompliance>();
            public readonly List<FoundryTreatyCompliance> QuotaMissed = new List<FoundryTreatyCompliance>();
            public readonly List<FoundryProductionRecord> Completed = new List<FoundryProductionRecord>();
            public readonly List<FoundryFailedCastRecord> Failed = new List<FoundryFailedCastRecord>();

            public TestHarness(int seed = 1009, bool wireInventory = true)
            {
                string dataDir = FindDataDir();
                var files = new FileSystemIO();
                var json = new SystemTextJsonSerializer();
                var production = SilentFoundryCatalogLoader.LoadProduction(dataDir, files, json);
                var faction = SilentFoundryCatalogLoader.LoadFaction(dataDir, files, json);
                Catalog = new SilentFoundryCatalog();
                Catalog.Load(production, faction);

                if (wireInventory)
                {
                    Inventory[SilentFoundryIds.ItemScrapMetal] = 500;
                    Inventory[SilentFoundryIds.ItemCoal] = 500;
                    Inventory[SilentFoundryIds.ItemCleanWater] = 500;
                    Inventory[SilentFoundryIds.ItemGreenSand] = 50;
                    Inventory[SilentFoundryIds.ItemFirebrick] = 50;
                    Inventory[SilentFoundryIds.ItemFlux] = 80;
                    Inventory[SilentFoundryIds.ItemAlloyAdditive] = 30;
                }

                Sys = new SilentFoundrySystem(rng: new SeededRng(seed));
                Sys.BindCatalog(Catalog, 4); // default blueprint cycle; BindBlueprintAndTreaties may override
                if (wireInventory)
                {
                    Sys.BindInventory(
                        id => Inventory.TryGetValue(id, out int v) ? v : 0,
                        (_, _) => true,
                        (id, amt) => Inventory[id] = (Inventory.TryGetValue(id, out int v) ? v : 0) + amt,
                        (id, amt) => Inventory[id] = Math.Max(0, (Inventory.TryGetValue(id, out int v) ? v : 0) - amt));
                }
                Sys.OnJournalTriggered += t => JournalTriggers.Add(t);
                Sys.OnIncident += i => Incidents.Add(i);
                Sys.OnTreatyQuotaMet += t => QuotaMet.Add(t);
                Sys.OnTreatyQuotaMissed += t => QuotaMissed.Add(t);
                Sys.OnProductionCompleted += r => Completed.Add(r);
                Sys.OnCastFailed += f => Failed.Add(f);
            }

            public void BindBlueprintAndTreaties()
            {
                string dataDir = FindDataDir();
                var files = new FileSystemIO();
                var json = new SystemTextJsonSerializer();

                int cycle = 4;
                var blueprints = new BunkerBlueprintCatalog();
                string bpJson = files.ReadAllText(files.Combine(dataDir, "narrative", "bunker_blueprints_codex.json"));
                blueprints.Load(bpJson, json);
                var bp = blueprints.GetById(SilentFoundryIds.BlueprintRoomId);
                if (bp != null && bp.maintenance_cycle_days > 0) cycle = bp.maintenance_cycle_days;
                Sys.BindCatalog(Catalog, cycle);

                var treaties = new RegionalTreatyCatalog();
                string treatyJson = files.ReadAllText(files.Combine(dataDir, "narrative", "regional_treaty_protocols.json"));
                treaties.Load(treatyJson, json);
                var ratification = new Dictionary<string, int>(StringComparer.Ordinal);
                for (int i = 0; i < treaties.AllTreaties.Count; i++)
                    if (treaties.AllTreaties[i] != null && treaties.AllTreaties[i].ratified_day > 0)
                        ratification[treaties.AllTreaties[i].treaty_id] = treaties.AllTreaties[i].ratified_day;
                Sys.BindTreaties(ratification);

                // Authored consequence policy.
                var policy = new SilentFoundryConsequencePolicyCatalog();
                policy.Load(SilentFoundryConsequenceCatalogLoader.Load(dataDir, files, json));
                Sys.BindConsequencePolicy(policy);
            }

            /// <summary>Run a plowshare heat to completion. Returns the start message.</summary>
            public string RunFirstHeat(int startDay, int workers = 4, float skill = 0.6f)
            {
                BindBlueprintAndTreaties();
                Sys.Unlock(startDay - 1);
                string start = Sys.StartProduction("foundry_prod_plowshare", workers, skill, startDay);
                int d = startDay + 1;
                for (int guard = 0; guard < 20 && Sys.HeatStage != FoundryHeatStage.Complete; guard++, d++)
                {
                    Sys.TickDaily(d);
                    if (Sys.HeatStage == FoundryHeatStage.AtHeat)
                        Sys.TapAndCast(d);
                }
                return start;
            }
        }

        // -----------------------------------------------------------------
        // Identity & catalog
        // -----------------------------------------------------------------

        [Fact]
        public void Identity_ExactIdsResolve()
        {
            Assert.Equal("exp_10_the_silent_foundry", SilentFoundryIds.ExpansionId);
            Assert.Equal("current_10_the_silent_foundry_guild", SilentFoundryIds.FactionId);
            Assert.Equal("room_bp_11_the_silent_foundry_smelter_bay", SilentFoundryIds.BlueprintRoomId);
            Assert.Equal("jrnl_foundry_first_heat", SilentFoundryIds.JournalFirstHeat);
            Assert.Equal("jrnl_foundry_strike", SilentFoundryIds.JournalStrike);
        }

        [Fact]
        public void Catalog_LoadsAllAuthoredProductsAndFaction()
        {
            string dataDir = FindDataDir();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var production = SilentFoundryCatalogLoader.LoadProduction(dataDir, files, json);
            Assert.NotNull(production);
            Assert.Equal(1, production.schema_version);
            Assert.Equal(11, production.products.Count);

            var faction = SilentFoundryCatalogLoader.LoadFaction(dataDir, files, json);
            Assert.NotNull(faction);
            Assert.Equal(SilentFoundryIds.FactionId, faction.faction_id);

            var catalog = new SilentFoundryCatalog();
            catalog.Load(production, faction);
            Assert.Equal(11, catalog.ProductCount);
            Assert.NotNull(catalog.GetProduct("foundry_prod_railway_spike"));
            Assert.NotNull(catalog.GetProduct("foundry_prod_acid_pipe"));
            Assert.Null(catalog.GetProduct("foundry_prod_missing"));
            Assert.Equal(3, catalog.GetQuotaProducts().Count); // spikes + wheels + acid pipes
            Assert.Contains(catalog.GetQuotaProducts(), p => p.treaty_id == SilentFoundryIds.TreatyRailway);
            Assert.Contains(catalog.GetQuotaProducts(), p => p.treaty_id == SilentFoundryIds.TreatySulfur);
        }

        [Fact]
        public void Catalog_QuotaProductsMapToExactTreaties()
        {
            var h = new TestHarness();
            Assert.Equal(3, h.Catalog.GetQuotaProducts().Count); // spikes, wheels, acid pipes
            foreach (var p in h.Catalog.GetQuotaProducts())
                Assert.True(p.quota_amount > 0 && !string.IsNullOrEmpty(p.treaty_id));
        }

        // -----------------------------------------------------------------
        // Journal template loading + expansion isolation
        // -----------------------------------------------------------------

        [Fact]
        public void JournalTemplates_LoadWithExpansionIsolation()
        {
            string dataDir = FindDataDir();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var batch = new NarrativeBatchCatalog();
            string jrnlJson = files.ReadAllText(files.Combine(dataDir, "narrative", "jrnl_templates_cycle_d.json"));
            batch.LoadJournalBatch(jrnlJson, json);

            Assert.Equal(4, batch.JournalTemplates.Count);

            var firstHeat = batch.JournalTemplates[SilentFoundryIds.JournalFirstHeat];
            Assert.NotNull(firstHeat);
            Assert.Equal("exp_10_the_silent_foundry", firstHeat.expansion_id);
            Assert.Equal("foundryman", firstHeat.author_role);
            Assert.Equal("First Cast Iron", firstHeat.title);
            Assert.Equal(-5.0f, firstHeat.stress_delta);
            Assert.Equal(5.0f, firstHeat.hope_earned);
            Assert.Contains("plowshares", firstHeat.body_template);

            var strike = batch.JournalTemplates[SilentFoundryIds.JournalStrike];
            Assert.NotNull(strike);
            Assert.Equal("exp_10_the_silent_foundry", strike.expansion_id);
            Assert.Equal("schoolmistress", strike.author_role);
            Assert.Equal("The Tools on the Floor", strike.title);
            Assert.Equal(7.0f, strike.stress_delta);
            Assert.Equal(2.0f, strike.hope_earned);

            // Cycle isolation: Year 11 / Year 12 templates stay assigned elsewhere.
            Assert.Equal("exp_11_the_orbital_harrow", batch.JournalTemplates["jrnl_harpoon_first_strike"].expansion_id);
            Assert.Equal("exp_12_the_century_seed", batch.JournalTemplates["jrnl_constitution_ratified"].expansion_id);
            Assert.DoesNotContain(batch.JournalTemplates.Keys,
                k => k == "jrnl_harpoon_first_strike" && batch.JournalTemplates[k].expansion_id == "exp_10_the_silent_foundry");
            Assert.DoesNotContain(batch.JournalTemplates.Keys,
                k => k == "jrnl_constitution_ratified" && batch.JournalTemplates[k].expansion_id == "exp_10_the_silent_foundry");
        }

        [Fact]
        public void JournalDeltas_MatchAuthoredTemplate()
        {
            string dataDir = FindDataDir();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var batch = new NarrativeBatchCatalog();
            batch.LoadJournalBatch(files.ReadAllText(files.Combine(dataDir, "narrative", "jrnl_templates_cycle_d.json")), json);

            Assert.True(SilentFoundrySystem.TryGetJournalDeltas(SilentFoundryIds.JournalFirstHeat, out float fs, out float fh));
            Assert.Equal(batch.JournalTemplates[SilentFoundryIds.JournalFirstHeat].stress_delta, fs);
            Assert.Equal(batch.JournalTemplates[SilentFoundryIds.JournalFirstHeat].hope_earned, fh);

            Assert.True(SilentFoundrySystem.TryGetJournalDeltas(SilentFoundryIds.JournalStrike, out float ss, out float sh));
            Assert.Equal(batch.JournalTemplates[SilentFoundryIds.JournalStrike].stress_delta, ss);
            Assert.Equal(batch.JournalTemplates[SilentFoundryIds.JournalStrike].hope_earned, sh);
        }

        // -----------------------------------------------------------------
        // Blueprint + treaties
        // -----------------------------------------------------------------

        [Fact]
        public void Blueprint_ResolvesAndAnchorsMaintenanceCycle()
        {
            string dataDir = FindDataDir();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var blueprints = new BunkerBlueprintCatalog();
            blueprints.Load(files.ReadAllText(files.Combine(dataDir, "narrative", "bunker_blueprints_codex.json")), json);

            var bp = blueprints.GetById(SilentFoundryIds.BlueprintRoomId);
            Assert.NotNull(bp);
            Assert.Equal("The Silent Foundry Blast Furnace & Casting Bay", bp.room_name);
            Assert.Equal("Heavy Metallurgy", bp.category);
            Assert.Equal(-25.0f, bp.optimal_depth_meters);
            Assert.Equal(8, bp.max_dweller_capacity);
            Assert.Equal(45.0f, bp.base_power_draw_kw);
            Assert.Equal(40.0f, bp.water_flow_lpm);
            Assert.Equal(4, bp.maintenance_cycle_days);
            Assert.Contains("firebrick cupola", bp.structural_header_spec);
            Assert.Contains("water-slag steam vapor explosion", bp.catastrophic_failure_mode);

            // Blueprint → runtime facility mapping.
            var h = new TestHarness();
            h.BindBlueprintAndTreaties();
            Assert.Equal(4, h.Sys.State.maintenanceCycleDays);
        }

        [Fact]
        public void Treaties_GuildIsExactSignatoryOfFourFoundryTreaties()
        {
            string dataDir = FindDataDir();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var treaties = new RegionalTreatyCatalog();
            treaties.Load(files.ReadAllText(files.Combine(dataDir, "narrative", "regional_treaty_protocols.json")), json);

            var foundry = treaties.GetByExactSignatoryFaction(SilentFoundryIds.FactionId);
            Assert.Equal(4, foundry.Count);
            Assert.Contains(foundry, t => t.treaty_id == SilentFoundryIds.TreatySulfur);
            Assert.Contains(foundry, t => t.treaty_id == SilentFoundryIds.TreatyLabor);
            Assert.Contains(foundry, t => t.treaty_id == SilentFoundryIds.TreatyRailway);
            Assert.Contains(foundry, t => t.treaty_id == SilentFoundryIds.TreatyConstitution);

            // The exact match must not leak the unrelated currents-pamphlet faction.
            Assert.DoesNotContain(foundry, t => t.treaty_id == "treaty_06_the_standing_record_archivists_charter");

            // Ratification-day anchors (authored).
            Assert.Equal(280, treaties.GetById(SilentFoundryIds.TreatySulfur).ratified_day);
            Assert.Equal(950, treaties.GetById(SilentFoundryIds.TreatyLabor).ratified_day);
            Assert.Equal(1500, treaties.GetById(SilentFoundryIds.TreatyRailway).ratified_day);
            Assert.Equal(3650, treaties.GetById(SilentFoundryIds.TreatyConstitution).ratified_day);
        }

        // -----------------------------------------------------------------
        // Unlock, repair, maintenance
        // -----------------------------------------------------------------

        [Fact]
        public void Unlock_IsIdempotentAndRaisesOnce()
        {
            var h = new TestHarness();
            int unlockEvents = 0;
            h.Sys.OnEventRaised += id => { if (id == SilentFoundrySystem.EventUnlocked) unlockEvents++; };
            Assert.True(h.Sys.Unlock(4));
            Assert.False(h.Sys.Unlock(5));
            Assert.True(h.Sys.IsUnlocked);
            Assert.Equal(1, unlockEvents);
        }

        [Fact]
        public void Repair_ConsumesFirebrickAndRestoresComponent()
        {
            var h = new TestHarness();
            h.Sys.Unlock(4);
            // Worn furnace.
            h.Sys.TickDaily(5); h.Sys.TickDaily(6); h.Sys.TickDaily(7); h.Sys.TickDaily(8);
            h.Sys.State.hearthTuyeres = 12f; // simulate heavy wear
            int before = h.Inventory[SilentFoundryIds.ItemFirebrick];

            string msg = h.Sys.StartRepair(FoundryFacilityComponent.HearthTuyeres, 9);
            Assert.StartsWith("Repair complete", msg);
            Assert.Equal(100f, h.Sys.GetComponentCondition(FoundryFacilityComponent.HearthTuyeres));
            Assert.Equal(before - 10, h.Inventory[SilentFoundryIds.ItemFirebrick]);

            // Not enough firebrick blocks the repair with a visible reason.
            h.Inventory[SilentFoundryIds.ItemFirebrick] = 1;
            string blocked = h.Sys.StartRepair(FoundryFacilityComponent.RefractoryLining, 10);
            Assert.Contains("Not enough firebrick", blocked);
        }

        [Fact]
        public void Maintenance_FourDayCycleAndOverdueConsequences()
        {
            var h = new TestHarness();
            h.BindBlueprintAndTreaties();
            h.Sys.Unlock(4);
            string done = h.Sys.PerformMaintenance(4);
            Assert.Contains("Maintenance performed", done);
            Assert.Equal(8, h.Sys.State.maintenanceDueDay);
            Assert.False(h.Sys.IsMaintenanceOverdue);

            // Advance past the due day without servicing.
            for (int d = 5; d <= 17; d++) h.Sys.TickDaily(d);
            Assert.True(h.Sys.IsMaintenanceOverdue);
            Assert.True(h.Sys.DaysOverdue >= 4);
            Assert.True(h.Sys.OverdueCycles >= 1);
            // Neglect must shorten the eventual quality, not just log.
            float qualityPenalty = Math.Min(15f, h.Sys.DaysOverdue * 2.5f);
            Assert.True(qualityPenalty > 0f);

            // Servicing resets the cycle.
            h.Sys.PerformMaintenance(14);
            Assert.False(h.Sys.IsMaintenanceOverdue);
        }

        // -----------------------------------------------------------------
        // Green-sand casting bed
        // -----------------------------------------------------------------

        [Fact]
        public void SandPrep_ConsumesSandAndWaterAndImprovesBed()
        {
            var h = new TestHarness();
            h.Sys.Unlock(4);
            h.Inventory[SilentFoundryIds.ItemCleanWater] = 100;
            float sandBefore = h.Sys.State.sandQuality;
            string msg = h.Sys.PrepareSand(40);
            Assert.StartsWith("Sand bed refreshed", msg);
            Assert.True(h.Sys.State.sandQuality > sandBefore);
            Assert.Equal(0, h.Sys.State.moldReuseCount);
            Assert.Equal(100 - 40, h.Inventory[SilentFoundryIds.ItemCleanWater]);
        }

        [Fact]
        public void SandPrep_BlocksWithoutSandOrWater()
        {
            var h = new TestHarness();
            h.Sys.Unlock(4);
            h.Inventory[SilentFoundryIds.ItemGreenSand] = 0;
            Assert.Contains("Not enough green sand", h.Sys.PrepareSand(10));
            h.Inventory[SilentFoundryIds.ItemGreenSand] = 5;
            h.Inventory[SilentFoundryIds.ItemCleanWater] = 0;
            Assert.Contains("Not enough clean water", h.Sys.PrepareSand(10));
        }

        // -----------------------------------------------------------------
        // Production lifecycle
        // -----------------------------------------------------------------

        [Fact]
        public void Production_StartValidatesChargeAndConsumesResources()
        {
            var h = new TestHarness();
            h.Sys.Unlock(4);
            var product = h.Catalog.GetProduct("foundry_prod_plowshare");

            int scrapBefore = h.Inventory[SilentFoundryIds.ItemScrapMetal];
            int coalBefore = h.Inventory[SilentFoundryIds.ItemCoal];
            int waterBefore = h.Inventory[SilentFoundryIds.ItemCleanWater];

            string msg = h.Sys.StartProduction(product.product_id, 4, 0.6f, 6);
            Assert.StartsWith("Heat started", msg);
            Assert.Equal(FoundryHeatStage.ChargeLoaded, h.Sys.HeatStage);
            Assert.Equal(scrapBefore - 6, h.Inventory[SilentFoundryIds.ItemScrapMetal]);
            Assert.Equal(coalBefore - 4, h.Inventory[SilentFoundryIds.ItemCoal]);
            Assert.Equal(waterBefore - 50, h.Inventory[SilentFoundryIds.ItemCleanWater]);

            // Second heat cannot start while one is active.
            Assert.Contains("already in progress", h.Sys.StartProduction(product.product_id, 4, 0.6f, 6));
        }

        [Fact]
        public void Production_MissingChargeGivesVisibleReason()
        {
            var h = new TestHarness();
            h.Sys.Unlock(4);
            h.Inventory[SilentFoundryIds.ItemScrapMetal] = 0;
            string msg = h.Sys.StartProduction("foundry_prod_plowshare", 4, 0.6f, 6);
            Assert.Contains("Missing charge material", msg);
        }

        [Fact]
        public void Production_FirstHeatCompletesAndTriggersJournalOnce()
        {
            var h = new TestHarness();
            string start = h.RunFirstHeat(6);
            Assert.StartsWith("Heat started", start);
            Assert.Equal(FoundryHeatStage.Complete, h.Sys.HeatStage);
            Assert.Equal(1, h.Sys.TotalProductionCount);
            Assert.Equal("foundry_prod_plowshare", h.Sys.CompletedProduction[0].productId);

            // Journal: once, with authored deltas.
            Assert.Single(h.JournalTriggers);
            Assert.Equal(SilentFoundryIds.JournalFirstHeat, h.JournalTriggers[0].TemplateId);
            Assert.Equal(-5f, h.Sys.CumulativeStress);
            Assert.Equal(5f, h.Sys.CumulativeHope);
            Assert.True(h.Sys.IsJournalTriggered(SilentFoundryIds.JournalFirstHeat));

            // Output landed in inventory.
            Assert.Equal(1, h.Inventory["item_foundry_plowshare"]);
        }

        [Fact]
        public void Production_SecondHeatDoesNotRetriggerJournal()
        {
            var h = new TestHarness();
            h.RunFirstHeat(6);
            Assert.Single(h.JournalTriggers);

            // Run a second heat.
            string start2 = h.Sys.StartProduction("foundry_prod_t_beam", 4, 0.6f, 40);
            Assert.StartsWith("Heat started", start2);
            int d = 41;
            for (int guard = 0; guard < 20 && h.Sys.HeatStage != FoundryHeatStage.Complete; guard++, d++)
            {
                h.Sys.TickDaily(d);
                if (h.Sys.HeatStage == FoundryHeatStage.AtHeat) h.Sys.TapAndCast(d);
            }
            Assert.Equal(2, h.Sys.TotalProductionCount);
            Assert.Single(h.JournalTriggers); // still exactly one first-heat entry
        }

        [Fact]
        public void Production_UntappedHeatBurnsOutAndRecordsFailure()
        {
            var h = new TestHarness();
            h.BindBlueprintAndTreaties();
            h.Sys.Unlock(5);
            h.Sys.StartProduction("foundry_prod_plowshare", 4, 0.6f, 6);
            // Never tap; stage machine runs through AtHeat (3 days) and burns out.
            for (int d = 7; d <= 16 && h.Sys.HeatStage != FoundryHeatStage.Idle; d++) h.Sys.TickDaily(d);
            Assert.Equal(FoundryHeatStage.Idle, h.Sys.HeatStage);
            Assert.Equal(1, h.Sys.TotalFailedCount);
            Assert.Contains("burned out untapped", h.Sys.FailedCasts[0].reason);
            Assert.True(h.Sys.FailedCasts[0].materialsLost > 0);
            Assert.Single(h.Failed);
        }

        [Fact]
        public void Production_QualityTiersAreDeterministicPerSeed()
        {
            var h = new TestHarness(seed: 42);
            h.RunFirstHeat(6);
            var h2 = new TestHarness(seed: 42);
            h2.RunFirstHeat(6);
            Assert.Equal(h.Sys.CompletedProduction[0].tier, h2.Sys.CompletedProduction[0].tier);
            Assert.Equal(h.Sys.State.pendingQuality, h2.Sys.State.pendingQuality);

            var h3 = new TestHarness(seed: 7);
            h3.RunFirstHeat(6);
            // Different seeds may differ; this documents determinism vs divergence.
            Assert.NotEqual(h.Sys.State.rngSeed, h3.Sys.State.rngSeed);
        }

        // -----------------------------------------------------------------
        // Safety & incidents
        // -----------------------------------------------------------------

        [Fact]
        public void Safety_WarningsSurfaceBeforeIrreversibleTap()
        {
            var h = new TestHarness();
            h.BindBlueprintAndTreaties();
            h.Sys.Unlock(5);
            h.Sys.State.hearthTuyeres = 20f;
            h.Sys.State.refractoryLining = 20f;
            h.Sys.State.safetyExhaust = 15f;
            h.Sys.StartProduction("foundry_prod_plowshare", 4, 0.6f, 6);
            for (int d = 7; d <= 9; d++) h.Sys.TickDaily(d);

            var warnings = h.Sys.GetSafetyWarnings();
            Assert.NotEmpty(warnings);
            Assert.Contains(warnings, w => w.Contains("Hearth brick"));
            Assert.Contains(warnings, w => w.Contains("Refractory lining"));
            Assert.True(h.Sys.ComputeIncidentChance() >= 20);
        }

        [Fact]
        public void Incident_SameSeedSameOutcome()
        {
            var h1 = new TestHarness(seed: 5);
            h1.BindBlueprintAndTreaties();
            h1.Sys.Unlock(5);
            h1.Sys.State.hearthTuyeres = 8f;
            h1.Sys.State.refractoryLining = 10f;
            h1.Sys.State.safetyExhaust = 5f;
            h1.Sys.StartProduction("foundry_prod_plowshare", 4, 0.6f, 6);
            for (int d = 7; d <= 9 && h1.Sys.HeatStage != FoundryHeatStage.AtHeat; d++) h1.Sys.TickDaily(d);
            string tap1 = h1.Sys.TapAndCast(9);

            var h2 = new TestHarness(seed: 5);
            h2.BindBlueprintAndTreaties();
            h2.Sys.Unlock(5);
            h2.Sys.State.hearthTuyeres = 8f;
            h2.Sys.State.refractoryLining = 10f;
            h2.Sys.State.safetyExhaust = 5f;
            h2.Sys.StartProduction("foundry_prod_plowshare", 4, 0.6f, 6);
            for (int d = 7; d <= 9 && h2.Sys.HeatStage != FoundryHeatStage.AtHeat; d++) h2.Sys.TickDaily(d);
            string tap2 = h2.Sys.TapAndCast(9);

            Assert.Equal(tap1, tap2);
            Assert.Equal(h1.Incidents.Count, h2.Incidents.Count);
            if (h1.Incidents.Count > 0)
            {
                Assert.Equal(h1.Incidents[0].severity, h2.Incidents[0].severity);
                Assert.Equal(h1.Incidents[0].downtimeDays, h2.Incidents[0].downtimeDays);
            }
        }

        [Fact]
        public void Incident_IsNeverHiddenAndLeavesARecord()
        {
            var h = new TestHarness(seed: 5);
            h.BindBlueprintAndTreaties();
            h.Sys.Unlock(5);
            // Near-certain incident conditions.
            h.Sys.State.hearthTuyeres = 5f;
            h.Sys.State.refractoryLining = 5f;
            h.Sys.State.safetyExhaust = 5f;
            h.Sys.State.sandMoisture = 95f;
            h.Sys.StartProduction("foundry_prod_plowshare", 4, 0.6f, 6);
            for (int d = 7; d <= 9 && h.Sys.HeatStage != FoundryHeatStage.AtHeat; d++) h.Sys.TickDaily(d);
            string result = h.Sys.TapAndCast(9);
            Assert.StartsWith("INCIDENT", result);
            Assert.Single(h.Incidents);
            Assert.Equal(FoundryHeatStage.Idle, h.Sys.HeatStage);
            Assert.True(h.Sys.State.workerExposure > 0f);
            // Damage is recorded, not silent.
            Assert.True(h.Sys.GetComponentCondition(FoundryFacilityComponent.HearthTuyeres) < 100f);
        }

        [Fact]
        public void Incident_WellMaintainedFurnaceNeverIncidents()
        {
            var h = new TestHarness(seed: 5);
            h.BindBlueprintAndTreaties();
            h.Sys.Unlock(5);
            h.Sys.PerformMaintenance(5);
            string start = h.Sys.StartProduction("foundry_prod_plowshare", 4, 0.7f, 6);
            Assert.StartsWith("Heat started", start);
            for (int d = 7; d <= 12; d++)
            {
                h.Sys.TickDaily(d);
                if (h.Sys.HeatStage == FoundryHeatStage.AtHeat) h.Sys.TapAndCast(d);
            }
            Assert.Empty(h.Incidents);
            Assert.Equal(FoundryHeatStage.Complete, h.Sys.HeatStage);
            Assert.Equal(0, h.Sys.ComputeIncidentChance());
        }

        // -----------------------------------------------------------------
        // Treaty compliance
        // -----------------------------------------------------------------

        [Fact]
        public void Treaty_RailQuotaMetOnDeadline()
        {
            var h = new TestHarness(seed: 42);
            h.BindBlueprintAndTreaties();
            h.Sys.Unlock(949);
            // Fulfil the full railway quota before the day-1500 assessment.
            h.Sys.State.activeProductId = string.Empty;
            var spikes = h.Catalog.GetProduct("foundry_prod_railway_spike");
            var wheels = h.Catalog.GetProduct("foundry_prod_rail_wheel");
            int neededSpikes = spikes.quota_amount;   // 60 (3 heats of 20)
            int neededWheels = wheels.quota_amount;   // 3
            // 3 spike heats + 3 wheel heats, run sequentially (a furnace pours one heat at a time).
            int day = 1450;
            day = RunHeat(h, spikes, day, 3);
            day = RunHeat(h, wheels, day, 3);
            var rail = h.Sys.GetTreatyCompliance(SilentFoundryIds.TreatyRailway);
            Assert.True(rail.quotaFulfilled >= neededSpikes + neededWheels,
                $"quota {rail.quotaFulfilled} >= {neededSpikes + neededWheels}");

            // Assessment closes the cycle: the next window starts fresh.
            h.Sys.AssessTreatyCompliance(1500);
            Assert.Equal(1, rail.metCount);
            // The labor accord (treaty_10, day 950) also assesses during the heats.
            Assert.Contains(h.QuotaMet, c => c.treatyId == SilentFoundryIds.TreatyRailway);
            Assert.Contains(h.QuotaMet, c => c.treatyId == SilentFoundryIds.TreatyLabor);
            Assert.Empty(h.QuotaMissed);
            Assert.Equal(0, rail.quotaFulfilled);
            Assert.Equal(1500, rail.lastAssessmentDay);
        }

        [Fact]
        public void Treaty_RailQuotaMissedOnDeadline()
        {
            var h = new TestHarness();
            h.BindBlueprintAndTreaties();
            h.Sys.Unlock(1490);
            h.Sys.AssessTreatyCompliance(1500);
            var rail = h.Sys.GetTreatyCompliance(SilentFoundryIds.TreatyRailway);
            Assert.False(rail.currentCycleMet);
            Assert.Equal(1, rail.missedCount);
            Assert.True(h.Sys.GuildStanding < 0f, "rail miss carries a negative standing consequence");
            Assert.Single(h.QuotaMissed);
        }

        [Fact]
        public void Treaty_LaborShiftViolationWhenStrikeOrOvertime()
        {
            var h = new TestHarness();
            h.BindBlueprintAndTreaties();
            h.Sys.Unlock(900);
            h.Sys.SetOvertime(true);
            h.Sys.AssessTreatyCompliance(950);
            var labor = h.Sys.GetTreatyCompliance(SilentFoundryIds.TreatyLabor);
            Assert.Equal(1, labor.missedCount);

            h.Sys.SetOvertime(false);
            h.Sys.AssessTreatyCompliance(980);
            Assert.Equal(1, labor.metCount);
        }

        [Fact]
        public void Treaty_RatificationDaysAreNotAssessedBeforeRatification()
        {
            var h = new TestHarness();
            h.BindBlueprintAndTreaties();
            h.Sys.Unlock(4);
            h.Sys.AssessTreatyCompliance(279); // before treaty_05 (day 280)
            var sulfur = h.Sys.GetTreatyCompliance(SilentFoundryIds.TreatySulfur);
            Assert.Equal(0, sulfur.missedCount);
            Assert.Equal(0, sulfur.lastAssessmentDay);

            h.Sys.AssessTreatyCompliance(280);
            Assert.Equal(280, sulfur.lastAssessmentDay);
        }

        // -----------------------------------------------------------------
        // Labor dispute & strike
        // -----------------------------------------------------------------

        [Fact]
        public void Strike_FatigueAloneDoesNotTriggerDispute()
        {
            var h = new TestHarness();
            h.BindBlueprintAndTreaties();
            h.Sys.Unlock(5);
            // No overtime, no child labour, no missed quota → no dispute.
            string msg = h.Sys.BeginLaborDispute(10);
            Assert.Contains("No genuine dispute conditions", msg);
            Assert.Equal(FoundryLaborDispute.None, h.Sys.LaborDispute);
        }

        [Fact]
        public void Strike_ProductionPressurePlusShiftGrievanceTriggersAndEscalates()
        {
            var h = new TestHarness();
            h.BindBlueprintAndTreaties();
            h.Sys.Unlock(5);
            h.Sys.SetOvertime(true);
            h.Sys.StartProduction("foundry_prod_plowshare", 4, 0.6f, 6); // active heat

            string opened = h.Sys.BeginLaborDispute(7);
            Assert.StartsWith("Labor tensions opened", opened);
            Assert.Equal(FoundryLaborDispute.Tensions, h.Sys.LaborDispute);

            // Escalation next day via TickDaily.
            h.Sys.TickDaily(8);
            Assert.Equal(FoundryLaborDispute.StrikeActive, h.Sys.LaborDispute);

            // Strike journal: once, authored deltas.
            Assert.Single(h.JournalTriggers);
            Assert.Equal(SilentFoundryIds.JournalStrike, h.JournalTriggers[0].TemplateId);
            Assert.Equal(7f, h.Sys.CumulativeStress - 0f);
            Assert.Equal(2f, h.Sys.CumulativeHope);
            Assert.True(h.Sys.IsJournalTriggered(SilentFoundryIds.JournalStrike));

            // Strike blocks new heats.
            string blocked = h.Sys.StartProduction("foundry_prod_plowshare", 4, 0.6f, 9);
            Assert.Contains("strike has shut", blocked);
        }

        [Fact]
        public void Strike_ResolutionIsTypedAndOnceOnly()
        {
            var h = new TestHarness();
            h.BindBlueprintAndTreaties();
            h.Sys.Unlock(5);
            h.Sys.SetOvertime(true);
            h.Sys.StartProduction("foundry_prod_plowshare", 4, 0.6f, 6);
            h.Sys.BeginLaborDispute(7);
            h.Sys.TickDaily(8);

            string resolved = h.Sys.ResolveStrike(FoundryStrikeResolution.Mediation, 9);
            Assert.Contains("Strike resolved", resolved);
            Assert.Equal(FoundryLaborDispute.Resolved, h.Sys.LaborDispute);
            Assert.False(h.Sys.State.overtimeFlag);

            // No duplicate journal entry after resolution.
            Assert.Single(h.JournalTriggers);
        }

        // -----------------------------------------------------------------
        // Persistence
        // -----------------------------------------------------------------

        [Fact]
        public void Save_RoundTripPreservesAllFoundryState()
        {
            var h = new TestHarness(seed: 42);
            h.RunFirstHeat(6);
            h.Sys.State.refractoryLining = 55f;
            h.Sys.State.overtimeFlag = true;

            var save = h.Sys.CaptureState();
            Assert.Equal(SilentFoundryState.CurrentVersion, save.stateVersion);

            var restored = new SilentFoundrySystem(rng: new SeededRng(1));
            restored.BindCatalog(h.Catalog, 4);
            restored.RestoreState(save);

            Assert.Equal(h.Sys.IsUnlocked, restored.IsUnlocked);
            Assert.Equal(h.Sys.HeatStage, restored.HeatStage);
            Assert.Equal(h.Sys.State.refractoryLining, restored.State.refractoryLining);
            Assert.Equal(h.Sys.State.overtimeFlag, restored.State.overtimeFlag);
            Assert.Equal(h.Sys.TotalProductionCount, restored.TotalProductionCount);
            Assert.Equal(h.Sys.TotalFailedCount, restored.TotalFailedCount);
            Assert.Equal(h.Sys.CumulativeStress, restored.CumulativeStress);
            Assert.Equal(h.Sys.CumulativeHope, restored.CumulativeHope);
            Assert.Equal(h.Sys.IsJournalTriggered(SilentFoundryIds.JournalFirstHeat),
                restored.IsJournalTriggered(SilentFoundryIds.JournalFirstHeat));
            Assert.Equal(h.Sys.State.rngSeed, restored.State.rngSeed);
        }

        [Fact]
        public void Save_ActiveFurnaceSurvivesRoundTrip()
        {
            var h = new TestHarness();
            h.BindBlueprintAndTreaties();
            h.Sys.Unlock(5);
            h.Sys.StartProduction("foundry_prod_plowshare", 4, 0.6f, 6);
            h.Sys.TickDaily(7); // ChargeLoaded → Preheat

            var save = h.Sys.CaptureState();
            var restored = new SilentFoundrySystem();
            restored.BindCatalog(h.Catalog, 4);
            restored.RestoreState(save);

            Assert.Equal(FoundryHeatStage.Preheat, restored.HeatStage);
            Assert.Equal("foundry_prod_plowshare", restored.State.activeProductId);
        }

        [Fact]
        public void Save_MissingFoundryStateDefaultsSafely()
        {
            var h = new TestHarness();
            h.BindBlueprintAndTreaties();
            var blank = new SilentFoundryState(); // simulates an old save with no foundry
            var sys = new SilentFoundrySystem();
            sys.BindCatalog(h.Catalog, 4);
            sys.RestoreState(blank);
            Assert.False(sys.IsUnlocked);
            Assert.Equal(FoundryHeatStage.Idle, sys.HeatStage);
            Assert.Equal(100f, sys.GetComponentCondition(FoundryFacilityComponent.HearthTuyeres));
            Assert.Empty(sys.CompletedProduction);
        }

        [Fact]
        public void Save_ChecksumStableAcrossHostSerializers()
        {
            var h = new TestHarness(seed: 42);
            h.RunFirstHeat(6);
            var save = h.Sys.CaptureState();

            string json = new SystemTextJsonSerializer().Serialize(save);
            var parsed = new SystemTextJsonSerializer().Deserialize<SilentFoundryState>(json);
            Assert.Equal(SaveChecksum.Compute(save), SaveChecksum.Compute(parsed));
        }

        [Fact]
        public void Save_ExpansionHubEnvelopeRoundTripsWithMigration()
        {
            var h = new TestHarness(seed: 42);
            h.RunFirstHeat(6);
            var json = new SystemTextJsonSerializer();

            // Current-version envelope.
            var envelope = new ExpansionHubSave { saveVersion = ExpansionHubSave.CurrentSaveVersion, simDay = 12 };
            envelope.foundry = h.Sys.CaptureState();
            envelope.consequences = h.Sys.CaptureConsequenceState();
            envelope.Checksum = SaveChecksum.Compute(envelope);
            string encoded = ExpansionHubSaveCodec.Encode(envelope, json);
            var decoded = ExpansionHubSaveCodec.Decode(encoded, json);
            Assert.Equal(ExpansionHubSave.CurrentSaveVersion, decoded.saveVersion);
            Assert.Equal(12, decoded.simDay);
            Assert.Equal(h.Sys.TotalProductionCount, decoded.foundry.completed.Count);
            Assert.NotNull(decoded.consequences);

            // v1 legacy file: validated against the frozen shape, foundry starts fresh.
            var v1 = new ExpansionHubSaveV1
            {
                saveVersion = 1,
                simDay = 3,
                waystation = new WaystationSystemState(),
                layouts = new LocationLayoutState(),
                memory = new LocationMemoryState(),
                siteEncounters = new SiteEncounterState(),
                vouch = new VouchAccessSystemState(),
                greenhouse = new GreenhouseState(),
                arbitration = new CrossingArbitrationState(),
                ledger = new LedgerDebtSystemState(),
                crossingQuests = new CrossingQuestSystemState(),
                generational = new GenerationalSuccessionSaveState()
            };
            v1.Checksum = SaveChecksum.Compute(v1);
            string legacy = json.Serialize(v1);
            var migrated = ExpansionHubSaveCodec.Decode(legacy, json);
            Assert.Equal(3, migrated.saveVersion);
            Assert.Equal(3, migrated.simDay);
            Assert.NotNull(migrated.foundry);
            Assert.False(migrated.foundry.unlocked);
        }

        // -----------------------------------------------------------------
        // Events
        // -----------------------------------------------------------------

        [Fact]
        public void Events_EmitExactlyOncePerOutcome()
        {
            var h = new TestHarness();
            var counts = new Dictionary<string, int>();
            h.Sys.OnEventRaised += id => counts[id] = (counts.TryGetValue(id, out int c) ? c : 0) + 1;
            h.RunFirstHeat(6);

            Assert.Equal(1, counts[SilentFoundrySystem.EventUnlocked]);
            Assert.Equal(1, counts[SilentFoundrySystem.EventHeatPrepared]);
            Assert.Equal(1, counts[SilentFoundrySystem.EventHeatStarted]);
            Assert.Equal(1, counts[SilentFoundrySystem.EventHeatCompleted]);
            Assert.Equal(1, counts[SilentFoundrySystem.EventCastCompleted]);
            Assert.Equal(1, counts[SilentFoundrySystem.EventJournalTriggered]);
            Assert.Equal(1, counts[SilentFoundrySystem.EventBlueprintReferenced]);
        }

        // -----------------------------------------------------------------
        // Internal helpers
        // -----------------------------------------------------------------

        private static int RunHeat(TestHarness h, FoundryProductEntry product, int startDay, int heats)
        {
            int day = startDay;
            for (int i = 0; i < heats; i++)
            {
                h.Sys.State.activeProductId = string.Empty;
                // Keep the furnace serviced so neglect does not drive incidents.
                h.Sys.PerformMaintenance(day - 1);
                string msg = h.Sys.StartProduction(product.product_id, 4, 0.6f, day);
                Assert.StartsWith("Heat started", msg);
                day++;
                for (int guard = 0; guard < 20 && h.Sys.HeatStage != FoundryHeatStage.Complete; guard++, day++)
                {
                    h.Sys.TickDaily(day);
                    if (h.Sys.HeatStage == FoundryHeatStage.AtHeat) h.Sys.TapAndCast(day);
                }
            }
            return day;
        }
    }
}
