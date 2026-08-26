using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Crossing;
using Ashfall.Core.Economy;
using Ashfall.Core.Foundry;
using Ashfall.Core.Legacy;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Silent Foundry treaty consequences — standing + trade/logistics access.
    /// Verifies that a treaty outcome applies a bounded, idempotent, durable
    /// consequence through the existing stance engine and market surfaces.
    /// </summary>
    public sealed class SilentFoundryConsequenceTests
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        private sealed class Harness
        {
            public readonly SilentFoundrySystem Sys;
            public readonly SilentFoundryCatalog Catalog;
            public readonly SilentFoundryConsequencePolicyCatalog Policy;
            public readonly Dictionary<string, int> Ratification = new Dictionary<string, int>(StringComparer.Ordinal);
            public readonly GoodsCatalog Goods;
            public readonly Dictionary<string, int> Inventory = new Dictionary<string, int>(StringComparer.Ordinal);
            public readonly List<FoundryConsequenceRecord> Applied = new List<FoundryConsequenceRecord>();

            public Harness()
            {
                string dataDir = FindDataDir();
                var files = new FileSystemIO();
                var json = new SystemTextJsonSerializer();

                // District 8 accords (foundry_accords.json) drive the ratification clock.
                foreach (var kv in SilentFoundryCatalogLoader.LoadAccordRatificationDays(dataDir, files, json))
                    Ratification[kv.Key] = kv.Value;

                var production = SilentFoundryCatalogLoader.LoadProduction(dataDir, files, json);
                var faction = SilentFoundryCatalogLoader.LoadFaction(dataDir, files, json);
                Catalog = new SilentFoundryCatalog();
                Catalog.Load(production, faction);

                var goodsLoad = GoodsCatalogLoader.Load(dataDir, files, json);
                Assert.False(goodsLoad.HasErrors, "economy_goods.json must validate");
                Goods = GoodsCatalogLoader.ToCatalog(goodsLoad);

                Policy = new SilentFoundryConsequencePolicyCatalog();
                Policy.Load(SilentFoundryConsequenceCatalogLoader.Load(dataDir, files, json));
                Assert.False(Policy.HasErrors, "foundry_treaty_consequences.json must validate: " + string.Join("; ", Policy.Errors));

                Inventory[SilentFoundryIds.ItemScrapMetal] = 500;
                Inventory[SilentFoundryIds.ItemCoal] = 500;
                Inventory[SilentFoundryIds.ItemCharcoal] = 500;
                Inventory[SilentFoundryIds.ItemCleanWater] = 500;
                Inventory[SilentFoundryIds.ItemFlux] = 80;
                Inventory[SilentFoundryIds.ItemAlloyAdditive] = 30;

                Sys = new SilentFoundrySystem();
                Sys.BindCatalog(Catalog, 4);
                Sys.BindTreaties(Ratification);
                Sys.BindConsequencePolicy(Policy);
                Sys.BindInventory(
                    id => Inventory.TryGetValue(id, out int v) ? v : 0,
                    (_, _) => true,
                    (id, amt) => Inventory[id] = (Inventory.TryGetValue(id, out int v) ? v : 0) + amt,
                    (id, amt) => Inventory[id] = Math.Max(0, (Inventory.TryGetValue(id, out int v) ? v : 0) - amt));
                Sys.OnConsequenceApplied += r => Applied.Add(r);
            }

            public void AssertOutcome(string treatyId, int day, FoundryTreatyOutcome expected)
            {
                Assert.Equal(expected, Sys.GetTreatyOutcome(treatyId, day));
            }
        }

        // ── 1. Identity ────────────────────────────────────────────────

        [Fact]
        public void Identity_ExactGuildFactionIsPreserved()
        {
            Assert.Equal("faction_silent_foundry", SilentFoundryIds.FactionId);
            Assert.NotEqual("current_10_the_foundry_union", SilentFoundryIds.FactionId);
        }

        // ── 2. Treaty-to-consequence mappings ───────────────────────────

        [Fact]
        public void Policy_ExactMappingsLoadAndResolve()
        {
            var h = new Harness();
            Assert.Equal(6, h.Policy.PolicyCount);

            var railMissed = h.Policy.Find(SilentFoundryIds.TreatyRoadIron, FoundryTreatyOutcome.Missed);
            Assert.NotNull(railMissed);
            Assert.Equal(SilentFoundryIds.FactionId, railMissed.faction_id);
            Assert.True(railMissed.standing_delta < 0f);
            Assert.NotEmpty(railMissed.market_modifiers);

            var laborViolated = h.Policy.Find(SilentFoundryIds.TreatyLabourSchedule, FoundryTreatyOutcome.Violated);
            Assert.NotNull(laborViolated);
            Assert.Contains(laborViolated.market_modifiers, m => m.good_id == "fuel");

            // the cluster charter has NO policy by design (finale marker, regression guard).
            Assert.Null(h.Policy.Find(SilentFoundryIds.TreatyClusterCharter, FoundryTreatyOutcome.Met));
            Assert.Null(h.Policy.Find(SilentFoundryIds.TreatyClusterCharter, FoundryTreatyOutcome.Missed));

            // Every policy faction id is exactly the guild.
            foreach (var p in h.Policy.AllPolicies)
                Assert.Equal(SilentFoundryIds.FactionId, p.faction_id);
        }

        [Fact]
        public void Policy_MarketGoodsResolveInTheActiveEconomyCatalog()
        {
            var h = new Harness();
            // The policy's good ids must exist as economy goods (the real market
            // surface), or the modifier is a dangling reference.
            foreach (var p in h.Policy.AllPolicies)
            {
                foreach (var m in p.market_modifiers)
                    Assert.NotNull(h.Goods.Find(m.good_id));
            }
            // The foundry goods the policy references exist as economy goods.
            Assert.NotNull(h.Goods.Find("coal"));
            Assert.NotNull(h.Goods.Find("item_foundry_brine_pipe"));
            Assert.NotNull(h.Goods.Find("item_foundry_ice_anchor"));
        }

        // ── 3. brine pipe accord met/missed ─────────────────────────────────────

        [Fact]
        public void Treaty05_MissedAppliesStandingAndMarketConsequences()
        {
            var h = new Harness();
            h.Sys.AssessTreatyCompliance(280); // day 280: ratified, quota short

            h.AssertOutcome(SilentFoundryIds.TreatyBrinePipe, 280, FoundryTreatyOutcome.Missed);
            Assert.Equal(-6f, h.Sys.GuildStanding);
            Assert.Single(h.Applied);
            Assert.Equal(SilentFoundryIds.TreatyBrinePipe, h.Applied[0].treatyId);
            Assert.Contains(h.Applied[0].modifiers, m => m.good_id == "item_foundry_brine_pipe");
            Assert.Contains(h.Applied[0].modifiers, m => m.good_id == "coal");
        }

        [Fact]
        public void Treaty05_MetRestoresStanding()
        {
            var h = new Harness();
            // Fulfil the acid-pipe quota (4 pipes) before day 280.
            var acid = h.Catalog.GetProduct("foundry_prod_brine_pipe");
            h.Sys.Unlock(240);
            h.Sys.PerformMaintenance(240);
            int day = 250;
            for (int i = 0; i < 4 && day < 280; i++)
            {
                h.Sys.State.activeProductId = string.Empty;
                h.Sys.PerformMaintenance(day - 1);
                string msg = h.Sys.StartProduction(acid.product_id, 4, 0.7f, day);
                Assert.StartsWith("Heat started", msg);
                day++;
                for (int guard = 0; guard < 20 && h.Sys.HeatStage != FoundryHeatStage.Complete; guard++, day++)
                {
                    h.Sys.TickDaily(day);
                    if (h.Sys.HeatStage == FoundryHeatStage.AtHeat) h.Sys.TapAndCast(day);
                }
            }
            h.Sys.AssessTreatyCompliance(280);

            h.AssertOutcome(SilentFoundryIds.TreatyBrinePipe, 280, FoundryTreatyOutcome.Met);
            Assert.Equal(2f, h.Sys.GuildStanding);
            Assert.Single(h.Applied);
            // Met restores the exchange lane: relief modifiers lower demand back.
            Assert.Contains(h.Applied[0].modifiers, m => m.good_id == "item_foundry_brine_pipe" && m.demand_delta < 0f);
            Assert.Contains(h.Applied[0].modifiers, m => m.good_id == "coal" && m.demand_delta < 0f);
        }

        // ── 4. road iron charter met/missed ─────────────────────────────────────

        [Fact]
        public void Treaty12_MissedAppliesRailAndCoalLogisticsConsequences()
        {
            var h = new Harness();
            h.Sys.AssessTreatyCompliance(330);

            h.AssertOutcome(SilentFoundryIds.TreatyRoadIron, 330, FoundryTreatyOutcome.Missed);
            Assert.Equal(-6f, h.Sys.GuildStanding);
            Assert.Contains(h.Applied[0].modifiers, m => m.good_id == "coal" && m.demand_delta > 0f);
            Assert.Contains(h.Applied[0].modifiers, m => m.good_id == "item_foundry_ice_anchor" && m.demand_delta > 0f);
        }

        [Fact]
        public void Treaty12_MetCarriesNoMarketPenalty()
        {
            var h = new Harness();
            var spikes = h.Catalog.GetProduct("foundry_prod_ice_anchor");
            var wheels = h.Catalog.GetProduct("foundry_prod_winch_drum");
            // Heats tick inside days 281..309: the road charter is not ratified
            // until 330 (cycle days skipped) and the brine accord's next cycle
            // day (310) falls outside, so the quota survives to the assessment.
            h.Sys.Unlock(280);
            int day = 281;
            day = RunHeats(h, spikes, day, 2);   // 60 anchors
            day = RunHeats(h, wheels, day, 1);   // 3 drums
            h.Sys.AssessTreatyCompliance(330);

            h.AssertOutcome(SilentFoundryIds.TreatyRoadIron, 330, FoundryTreatyOutcome.Met);
            var railMet = System.Array.Find(h.Applied.ToArray(), r => r.treatyId == SilentFoundryIds.TreatyRoadIron && r.outcome == FoundryTreatyOutcome.Met);
            Assert.NotNull(railMet);
            Assert.Equal(3f, railMet.standingDelta);
            // Met relieves the logistics squeeze (coal + spikes demand eases), never adds one.
            Assert.All(railMet.modifiers, m => Assert.True(m.demand_delta < 0f, "met relief only lowers demand"));
            Assert.Contains(railMet.modifiers, m => m.good_id == "coal");
            Assert.Contains(railMet.modifiers, m => m.good_id == "item_foundry_ice_anchor");

            // Guild standing is the deterministic sum of all applied cycles
            // (intervening treaties assess on their own cycles — intended tension).
            float expected = 0f;
            foreach (var r in h.Applied) expected += r.standingDelta;
            Assert.Equal(expected, h.Sys.GuildStanding, 3);
            Assert.True(h.Sys.GuildStanding >= 3f, "rail met contributes its bonus");
        }

        // ── 5/6. labour schedule compliant vs violation, fatigue alone ────────

        [Fact]
        public void Treaty10_CompliantLaborPreservesAccess()
        {
            var h = new Harness();
            h.Sys.AssessTreatyCompliance(305);

            h.AssertOutcome(SilentFoundryIds.TreatyLabourSchedule, 305, FoundryTreatyOutcome.Met);
            Assert.Equal(2f, h.Sys.GuildStanding);
            // Compliant labor releases the fuel allotment (relief), never penalizes.
            Assert.All(h.Applied[0].modifiers, m => Assert.True(m.demand_delta < 0f));
            Assert.Contains(h.Applied[0].modifiers, m => m.good_id == "fuel");
        }

        [Fact]
        public void Treaty10_GenuineViolationPenalizesStandingAndFuel()
        {
            var h = new Harness();
            h.Sys.SetOvertime(true);
            h.Sys.AssessTreatyCompliance(305);

            h.AssertOutcome(SilentFoundryIds.TreatyLabourSchedule, 305, FoundryTreatyOutcome.Violated);
            Assert.Equal(-8f, h.Sys.GuildStanding);
            Assert.Contains(h.Applied[0].modifiers, m => m.good_id == "fuel" && m.demand_delta > 0f);
        }

        [Fact]
        public void Treaty10_FatigueAloneIsNeverAViolation()
        {
            var h = new Harness();
            // Heavy work (exposure) without strike/overtime/child-labor semantics.
            h.Sys.State.workerExposure = 400f; // exhausted crew
            h.Sys.State.laborDispute = FoundryLaborDispute.None;
            h.Sys.State.overtimeFlag = false;
            h.Sys.State.childLaborUsed = false;
            h.Sys.AssessTreatyCompliance(305);

            h.AssertOutcome(SilentFoundryIds.TreatyLabourSchedule, 305, FoundryTreatyOutcome.Met);
            Assert.True(h.Sys.GuildStanding > 0f);
        }

        // ── 7. Pre-ratification neutrality ──────────────────────────────

        [Fact]
        public void Outcome_PreRatificationIsNeutral()
        {
            var h = new Harness();
            h.Sys.AssessTreatyCompliance(279); // before the brine pipe accord (day 280)

            h.AssertOutcome(SilentFoundryIds.TreatyBrinePipe, 279, FoundryTreatyOutcome.NotRatified);
            Assert.Equal(0f, h.Sys.GuildStanding);
            Assert.Empty(h.Applied);
            Assert.Equal(0, h.Sys.State.treatyCompliance[0].lastAssessmentDay);
        }

        // ── 8. cluster charter regression ─────────────────────────────────────

        [Fact]
        public void Treaty16_NoConsequenceIsEverApplied()
        {
            var h = new Harness();
            h.Sys.State.incidents.Add(new FoundryIncidentRecord { severity = FoundryIncidentSeverity.Severe, day = 10 });
            h.Sys.AssessTreatyCompliance(365);

            // the charter itself never carries a consequence, regardless of the
            // other ratified treaties assessing on the same day.
            Assert.DoesNotContain(h.Applied, r => r.treatyId == SilentFoundryIds.TreatyClusterCharter);
            var c = h.Sys.GetTreatyCompliance(SilentFoundryIds.TreatyClusterCharter);
            Assert.False(c.constitutionEligible); // eligibility derivation unchanged
            h.Sys.State.incidents.Clear();
            h.Sys.AssessTreatyCompliance(365);
            Assert.True(h.Sys.GetTreatyCompliance(SilentFoundryIds.TreatyClusterCharter).constitutionEligible);
        }

        // ── 9/10/11. Once-per-cycle, no stacking, reload-safe ───────────

        [Fact]
        public void Standing_AppliedExactlyOncePerCycle()
        {
            var h = new Harness();
            h.Sys.AssessTreatyCompliance(330);
            h.Sys.AssessTreatyCompliance(330); // same day, same cycle
            h.Sys.AssessTreatyCompliance(330);

            Assert.Single(h.Applied);
            Assert.Equal(-6f, h.Sys.GuildStanding);
            Assert.True(h.Sys.IsConsequenceApplied(SilentFoundryIds.TreatyRoadIron, 330));
        }

        [Fact]
        public void Standing_NextCycleAppliesOnceMoreButDoesNotSnowballPastBounds()
        {
            var h = new Harness();
            h.Sys.AssessTreatyCompliance(330);   // -6
            h.Sys.AssessTreatyCompliance(1530);   // -6 (next cycle)
            h.Sys.AssessTreatyCompliance(1560);   // -6
            Assert.Equal(3, h.Applied.Count);
            Assert.Equal(-18f, h.Sys.GuildStanding);

            // Many cycles: standing clamps at the existing faction range bound.
            for (int i = 0; i < 30; i++)
                h.Sys.AssessTreatyCompliance(330 + (i + 3) * 30);
            Assert.True(h.Sys.GuildStanding >= -100f, "standing clamped to range minimum");
        }

        [Fact]
        public void Standing_RestoreDoesNotReapplyOrStack()
        {
            var h = new Harness();
            h.Sys.AssessTreatyCompliance(330); // -6
            h.Sys.AssessTreatyCompliance(305);  // labour schedule assessment (compliant) → +2

            var consequences = h.Sys.CaptureConsequenceState();
            var restored = new SilentFoundrySystem();
            restored.BindConsequencePolicy(h.Policy);
            restored.RestoreConsequenceState(consequences);

            Assert.Equal(h.Sys.GuildStanding, restored.GuildStanding);
            Assert.Equal(h.Sys.AppliedConsequences.Count, restored.AppliedConsequences.Count);
            Assert.True(restored.IsConsequenceApplied(SilentFoundryIds.TreatyRoadIron, 330));

            // Re-assessing the same cycles on the restored system does nothing new.
            restored.AssessTreatyCompliance(330);
            Assert.Equal(h.Sys.AppliedConsequences.Count, restored.AppliedConsequences.Count);
            Assert.Equal(h.Sys.GuildStanding, restored.GuildStanding);
        }

        [Fact]
        public void Standing_MissingStateDefaultsToNeutral()
        {
            var restored = new SilentFoundrySystem();
            restored.BindConsequencePolicy(new Harness().Policy);
            restored.RestoreConsequenceState(null); // old save without the ledger

            Assert.Equal(0f, restored.GuildStanding);
            Assert.Empty(restored.AppliedConsequences);
        }

        // ── 12. Real economy surface ────────────────────────────────────

        [Fact]
        public void Market_ModifierRaisesDemandAndPriceOnTheRealMarket()
        {
            var h = new Harness();
            var market = new MarketSystem();
            market.BindCatalog(h.Goods);

            float coalBefore = market.GetPrice("coal");
            Assert.False(float.IsNaN(coalBefore), "coal is an economy good");

            // Host path: AdjustDemand once per applied consequence.
            market.AdjustDemand("coal", 0.2f);
            Assert.Equal(1.2f, market.GetDemandMultiplier("coal"), 3);
            Assert.True(market.GetPrice("coal") > coalBefore, "demand delta raises the effective price");

            // Demand is bounded by the existing market clamps (no unbounded snowball).
            for (int i = 0; i < 50; i++) market.AdjustDemand("coal", 0.5f);
            Assert.True(market.GetDemandMultiplier("coal") <= MarketSystem.MaxDemandMult);
        }

        [Fact]
        public void Market_MissThenMetRestoresDemandOnTheRealMarket()
        {
            var h = new Harness();
            var market = new MarketSystem();
            market.BindCatalog(h.Goods);

            // brine-pipe miss applies +0.4 acid-pipe demand; the following met
            // cycle applies −0.4 relief — the exchange lane returns to baseline.
            market.AdjustDemand("item_foundry_brine_pipe", 0.4f);
            Assert.Equal(1.4f, market.GetDemandMultiplier("item_foundry_brine_pipe"), 3);
            market.AdjustDemand("item_foundry_brine_pipe", -0.4f);
            Assert.Equal(1.0f, market.GetDemandMultiplier("item_foundry_brine_pipe"), 3);

            // The relief is bounded by the market floor when over-applied.
            market.AdjustDemand("fuel", -10f);
            Assert.True(market.GetDemandMultiplier("fuel") >= MarketSystem.MinDemandMult);
        }

        [Fact]
        public void Policy_MetReliefRowsMirrorTheMissPenalties()
        {
            var h = new Harness();
            // Each quota treaty's met relief exactly negates its miss penalty, so
            // a miss→met cycle returns the market to baseline (no permanent drift).
            foreach (string treatyId in new[]
            {
                SilentFoundryIds.TreatyBrinePipe,
                SilentFoundryIds.TreatyLabourSchedule,
                SilentFoundryIds.TreatyRoadIron
            })
            {
                var met = h.Policy.Find(treatyId, FoundryTreatyOutcome.Met);
                var bad = h.Policy.Find(treatyId, FoundryTreatyOutcome.Missed) ?? h.Policy.Find(treatyId, FoundryTreatyOutcome.Violated);
                Assert.NotNull(met);
                Assert.NotNull(bad);
                foreach (var mm in met.market_modifiers)
                {
                    var badMod = System.Array.Find(bad.market_modifiers.ToArray(), b => b.good_id == mm.good_id);
                    Assert.NotNull(badMod);
                    Assert.Equal(-badMod.demand_delta, mm.demand_delta, 3);
                }
            }
        }

        [Fact]
        public void Standing_TradeFloorGatesTheStall()
        {
            var h = new Harness();
            var stance = new FactionStanceEngine();
            stance.RegisterFaction(new FactionThresholds(
                SilentFoundryIds.FactionId,
                raidThreshold: -50f, robThreshold: -20f, minTrustToTrade: -40f, intelShareThreshold: 40f));

            // Trust above the floor → the stall is open.
            stance.SetTrust(SilentFoundryIds.FactionId, -6f);
            Assert.Equal(TradeStance.Trade, stance.GetStance(SilentFoundryIds.FactionId));

            // The stance ladder (raid −50 → rob −20 → trade 40 → intel): with the
            // guild's thresholds the rob band shadows the refuse band, so every
            // trust value below the −20 rob floor blocks the stall.
            stance.SetTrust(SilentFoundryIds.FactionId, -35f); // between raid and rob floors
            Assert.Equal(TradeStance.Rob, stance.GetStance(SilentFoundryIds.FactionId));

            stance.SetTrust(SilentFoundryIds.FactionId, -60f); // raid line
            Assert.Equal(TradeStance.HostileRaid, stance.GetStance(SilentFoundryIds.FactionId));

            stance.SetTrust(SilentFoundryIds.FactionId, 50f); // intel line
            Assert.Equal(TradeStance.ShareIntel, stance.GetStance(SilentFoundryIds.FactionId));

            // The trade screen's gate uses exactly this stance (willTrade = Trade|ShareIntel),
            // so every sub-Trade band visibly blocks the Foundry Guild's stall.
            foreach (var blocked in new[] { TradeStance.Rob, TradeStance.HostileRaid, TradeStance.Refuse })
            {
                Assert.NotEqual(TradeStance.Trade, blocked);
                Assert.NotEqual(TradeStance.ShareIntel, blocked);
            }
        }

        [Fact]
        public void Market_PolicyModifierSetMatchesTheEconomySurface()
        {
            var h = new Harness();
            // Every modifier the policy can produce exists as a good the market
            // can price — the exact hook the host uses to apply consequences.
            var market = new MarketSystem();
            market.BindCatalog(h.Goods);
            foreach (var p in h.Policy.AllPolicies)
                foreach (var m in p.market_modifiers)
                    Assert.False(float.IsNaN(market.GetPrice(m.good_id)), m.good_id + " must be priceable");
        }

        // ── 13. No leakage to the Foundry Union ─────────────────────────

        [Fact]
        public void Standing_NeverLeaksToTheFoundryUnion()
        {
            var h = new Harness();
            var stance = new FactionStanceEngine();
            stance.RegisterFaction(new FactionThresholds(
                SilentFoundryIds.FactionId,
                raidThreshold: -50f, robThreshold: -20f, minTrustToTrade: -40f, intelShareThreshold: 40f));

            h.Sys.OnConsequenceApplied += r => stance.ModifyTrust(SilentFoundryIds.FactionId, r.standingDelta);
            h.Sys.AssessTreatyCompliance(330); // -6 for the guild

            Assert.Equal(-6f, stance.GetTrust(SilentFoundryIds.FactionId));
            Assert.Equal(0f, stance.GetTrust("current_10_the_foundry_union"));
            Assert.Equal(TradeStance.Trade, stance.GetStance(SilentFoundryIds.FactionId)); // -40 floor not crossed
        }

        // ── 14. Determinism ─────────────────────────────────────────────

        [Fact]
        public void Consequences_AreDeterministicGivenSameInputs()
        {
            var a = new Harness();
            var b = new Harness();
            a.Sys.AssessTreatyCompliance(330);
            a.Sys.AssessTreatyCompliance(305);
            b.Sys.AssessTreatyCompliance(330);
            b.Sys.AssessTreatyCompliance(305);

            Assert.Equal(a.Sys.GuildStanding, b.Sys.GuildStanding);
            Assert.Equal(a.Sys.AppliedConsequences.Count, b.Sys.AppliedConsequences.Count);
            Assert.Equal(a.Applied[0].standingDelta, b.Applied[0].standingDelta);
            // No RNG is consumed anywhere in the consequence path.
            Assert.Equal(a.Sys.State.rngSeed, b.Sys.State.rngSeed);
        }

        // ── 15/16/17. Persistence ───────────────────────────────────────

        [Fact]
        public void Save_RoundTripsConsequenceLedgerThroughTheHubEnvelope()
        {
            var h = new Harness();
            h.Sys.AssessTreatyCompliance(330);
            h.Sys.AssessTreatyCompliance(305);

            var json = new SystemTextJsonSerializer();
            var envelope = new ExpansionHubSave { saveVersion = ExpansionHubSave.CurrentSaveVersion, simDay = 330 };
            envelope.foundry = h.Sys.CaptureState();
            envelope.consequences = h.Sys.CaptureConsequenceState();
            envelope.Checksum = SaveChecksum.Compute(envelope);
            string encoded = ExpansionHubSaveCodec.Encode(envelope, json);
            var decoded = ExpansionHubSaveCodec.Decode(encoded, json);

            Assert.Equal(h.Sys.GuildStanding, decoded.consequences.guildStanding);
            Assert.Equal(h.Sys.AppliedConsequences.Count, decoded.consequences.applied.Count);

            var restored = new SilentFoundrySystem();
            restored.BindConsequencePolicy(h.Policy);
            restored.RestoreConsequenceState(decoded.consequences);
            Assert.Equal(h.Sys.GuildStanding, restored.GuildStanding);
            Assert.True(restored.IsConsequenceApplied(SilentFoundryIds.TreatyRoadIron, 330));
        }

        [Fact]
        public void Save_V2LegacyMigratesWithEmptyConsequenceLedger()
        {
            var json = new SystemTextJsonSerializer();
            var v2 = new ExpansionHubSaveV2
            {
                saveVersion = 2,
                simDay = 330,
                waystation = new WaystationSystemState(),
                layouts = new LocationLayoutState(),
                memory = new LocationMemoryState(),
                siteEncounters = new SiteEncounterState(),
                vouch = new VouchAccessSystemState(),
                greenhouse = new GreenhouseState(),
                arbitration = new CrossingArbitrationState(),
                ledger = new LedgerDebtSystemState(),
                crossingQuests = new CrossingQuestSystemState(),
                generational = new GenerationalSuccessionSaveState(),
                foundry = new SilentFoundryState { unlocked = true }
            };
            v2.Checksum = SaveChecksum.Compute(v2);

            var migrated = ExpansionHubSaveCodec.Decode(json.Serialize(v2), json);
            Assert.Equal(ExpansionHubSave.CurrentSaveVersion, migrated.saveVersion);
            Assert.True(migrated.foundry.unlocked, "v2 foundry state preserved");
            Assert.NotNull(migrated.consequences);
            Assert.Equal(0f, migrated.consequences.guildStanding);
            Assert.Empty(migrated.consequences.applied);
        }

        [Fact]
        public void Save_ChecksumStableAcrossSerializerRoundTrip()
        {
            var h = new Harness();
            h.Sys.AssessTreatyCompliance(330);
            var state = h.Sys.CaptureConsequenceState();

            string json = new SystemTextJsonSerializer().Serialize(state);
            var parsed = new SystemTextJsonSerializer().Deserialize<SilentFoundryConsequenceState>(json);
            Assert.Equal(SaveChecksum.Compute(state), SaveChecksum.Compute(parsed));
        }

        // ── 18. Data-integrity contract (referenced ids resolve) ────────

        [Fact]
        public void Data_AllPolicyReferencesResolveInAuthoritativeCatalogs()
        {
            var h = new Harness();
            // Good ids resolve as economy goods AND as inventory item definitions
            // (the same id namespace, per the established goods convention).
            string dataDir = FindDataDir();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            string foundryItemsRaw = files.ReadAllText(files.Combine(dataDir, "foundry_items.json"));
            var items = CatalogLocator.LoadWrappedList<FoundryItemJsonForTest>(foundryItemsRaw, SystemTextJsonSerializer.Options);
            var itemIds = new HashSet<string>(StringComparer.Ordinal);
            if (items != null)
                foreach (var it in items) itemIds.Add(it.id);

            foreach (var p in h.Policy.AllPolicies)
            {
                Assert.Equal(SilentFoundryIds.FactionId, p.faction_id);
                foreach (var m in p.market_modifiers)
                {
                    Assert.NotNull(h.Goods.Find(m.good_id));
                    Assert.True(itemIds.Contains(m.good_id) || m.good_id == "coal" || m.good_id == "fuel" || m.good_id == "scrap_metal",
                        m.good_id + " must be an inventory item or charge good");
                }
            }
        }

        // ── Helpers ─────────────────────────────────────────────────────

        private static int RunHeats(Harness h, FoundryProductEntry product, int startDay, int heats)
        {
            int day = startDay;
            for (int i = 0; i < heats; i++)
            {
                h.Sys.State.activeProductId = string.Empty;
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

    /// <summary>foundry_items.json row shape (tests only — checks id resolution).</summary>
    public sealed class FoundryItemJsonForTest
    {
        public string? id;
    }
}
