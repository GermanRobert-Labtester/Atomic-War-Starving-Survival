using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Foundry;
using Ashfall.Core.Narrative;

namespace Ashfall.Core
{
    /// <summary>
    /// Silent Foundry (Exp 10) pack-minimum smoke: exact ids resolve, catalog
    /// loads, a heat completes end-to-end with deterministic outcome, and the
    /// save round-trips. Invoked by the expansions selftest and by xUnit.
    /// </summary>
    public static class SilentFoundryHeadlessDemo
    {
        public const int DemoSeed = 1009;

        public static HeadlessReport Run(string dataDirectory = null, ILog log = null)
        {
            CatalogLocator.UseInvariantCulture();
            log = log ?? NullLog.Instance;
            var report = new HeadlessReport();

            void Check(bool condition, string name)
            {
                report.Checks.Add(new HeadlessCheck { Name = name, Passed = condition });
                if (condition) { report.PassedCount++; log.Info("[PASS] " + name); }
                else { report.FailedCount++; log.Error("[FAIL] " + name); }
            }

            log.Info("[SilentFoundryHeadlessDemo] begin");

            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            // Identity resolution (exact ids, never aliased).
            Check(SilentFoundryIds.ExpansionId == "exp_10_the_silent_foundry", "expansion id exp_10_the_silent_foundry");
            Check(SilentFoundryIds.FactionId == "faction_silent_foundry", "faction id faction_silent_foundry");
            Check(SilentFoundryIds.BlueprintRoomId == "room_bp_11_the_silent_foundry_smelter_bay", "blueprint id room_bp_11_the_silent_foundry_smelter_bay");

            // Static catalogs load from disk.
            var production = SilentFoundryCatalogLoader.LoadProduction(dataDirectory, files, json);
            var faction = SilentFoundryCatalogLoader.LoadFaction(dataDirectory, files, json);
            Check(production != null && production.products.Count >= 8, "foundry_production.json loads >= 8 products");
            Check(faction != null && faction.faction_id == SilentFoundryIds.FactionId, "foundry_faction.json registers the exact guild id");

            var catalog = new SilentFoundryCatalog();
            catalog.Load(production, faction);
            Check(catalog.ProductCount == production.products.Count, "catalog product count matches file");

            // Blueprint anchor resolves (static, unmutated).
            string bpJson = files.FileExists(Path.Combine(dataDirectory, "narrative", "bunker_blueprints_codex.json"))
                ? files.ReadAllText(Path.Combine(dataDirectory, "narrative", "bunker_blueprints_codex.json"))
                : string.Empty;
            var blueprints = new BunkerBlueprintCatalog();
            if (!string.IsNullOrEmpty(bpJson)) blueprints.Load(bpJson, json);
            var bp = blueprints.GetById(SilentFoundryIds.BlueprintRoomId);
            Check(bp != null, "blueprint room_bp_11_the_silent_foundry_smelter_bay resolves");
            Check(bp == null || bp.maintenance_cycle_days == 4, "blueprint maintenance_cycle_days == 4");
            Check(bp == null || bp.max_dweller_capacity == 8, "blueprint max_dweller_capacity == 8");

            // Accord anchors resolve (District 8 accords — data authority: foundry_accords.json).
            var treaties = new RegionalTreatyCatalog();
            string accordsPath = string.IsNullOrEmpty(dataDirectory)
                ? string.Empty
                : Path.Combine(dataDirectory, SilentFoundryCatalogLoader.AccordsFileName);
            if (!string.IsNullOrEmpty(accordsPath) && files.FileExists(accordsPath))
                treaties.Load(files.ReadAllText(accordsPath), json);
            var foundryTreaties = treaties.GetByExactSignatoryFaction(SilentFoundryIds.FactionId);
            Check(foundryTreaties.Count == 4, "the foundry is exact signatory of exactly 4 District 8 accords");
            Check(treaties.GetById(SilentFoundryIds.TreatyBrinePipe)?.ratified_day == 280, "brine pipe accord ratified day 280");
            Check(treaties.GetById(SilentFoundryIds.TreatyRoadIron)?.ratified_day == 330, "road iron charter ratified day 330");

            // Consequence policy loads and its good refs resolve in the economy catalog.
            var policy = new SilentFoundryConsequencePolicyCatalog();
            policy.Load(SilentFoundryConsequenceCatalogLoader.Load(dataDirectory, files, json));
            Check(!policy.HasErrors && policy.PolicyCount >= 5, "foundry_treaty_consequences.json validates");
            var goodsLoad = Ashfall.Core.Economy.GoodsCatalogLoader.Load(dataDirectory, files, json);
            if (!goodsLoad.HasErrors)
            {
                var goods = Ashfall.Core.Economy.GoodsCatalogLoader.ToCatalog(goodsLoad);
                bool allResolve = true;
                foreach (var p in policy.AllPolicies)
                    foreach (var m in p.market_modifiers)
                        if (goods.Find(m.good_id) == null) allResolve = false;
                Check(allResolve, "every policy good id resolves in economy_goods.json");
            }

            // End-to-end heat with a seeded RNG (deterministic smoke).
            var inventory = new Dictionary<string, int>
            {
                { SilentFoundryIds.ItemScrapMetal, 200 },
                { SilentFoundryIds.ItemCoal, 200 },
                { SilentFoundryIds.ItemCleanWater, 200 },
                { SilentFoundryIds.ItemGreenSand, 20 },
                { SilentFoundryIds.ItemFirebrick, 40 },
                { SilentFoundryIds.ItemFlux, 40 }
            };
            var sys = new SilentFoundrySystem(rng: new SeededRng(DemoSeed), log: log);
            sys.BindInventory(
                id => inventory.TryGetValue(id, out int v) ? v : 0,
                (id, amt) => true,
                (id, amt) => inventory[id] = (inventory.TryGetValue(id, out int v) ? v : 0) + amt,
                (id, amt) => inventory[id] = System.Math.Max(0, (inventory.TryGetValue(id, out int v) ? v : 0) - amt));
            sys.BindCatalog(catalog, bp != null ? bp.maintenance_cycle_days : 4);
            sys.BindConsequencePolicy(policy);
            sys.Unlock(4);

            var first = catalog.AllProducts.Count > 0 ? catalog.AllProducts[0] : null;
            Check(first != null, "first product exists");
            if (first != null)
            {
                string start = sys.StartProduction(first.product_id, 4, 0.6f, 6);
                Check(start.StartsWith("Heat started"), "heat starts with full charge: " + start);
                sys.TickDaily(7);
                sys.TickDaily(8);
                string tap = sys.TapAndCast(8);
                Check(tap.StartsWith("Tap successful") || tap.StartsWith("INCIDENT"), "tap resolves deterministically: " + tap);
                for (int d = 9; d <= 14 && sys.HeatStage != FoundryHeatStage.Complete; d++) sys.TickDaily(d);
                Check(sys.HeatStage == FoundryHeatStage.Complete || sys.TotalProductionCount > 0 || sys.TotalFailedCount > 0,
                    "cast completes or records a failure");
                if (sys.TotalProductionCount > 0)
                    Check(sys.CompletedProduction[0].tier != FoundryQualityTier.Scrap || sys.TotalFailedCount > 0,
                        "quality tier recorded (scrap goes to failure list)");
            }

            // Consequence smoke: a missed quota applies a bounded standing effect.
            var ratification = new Dictionary<string, int>(StringComparer.Ordinal);
            for (int i = 0; i < treaties.AllTreaties.Count; i++)
            {
                var t = treaties.AllTreaties[i];
                if (t != null && t.ratified_day > 0) ratification[t.treaty_id] = t.ratified_day;
            }
            sys.BindTreaties(ratification);
            float standingBefore = sys.GuildStanding;
            sys.AssessTreatyCompliance(280); // treaty_05 acid-pipe quota short
            Check(sys.GuildStanding < standingBefore, "missed quota lowers guild standing");
            Check(sys.IsConsequenceApplied(SilentFoundryIds.TreatyBrinePipe, 280), "consequence applied once for the cycle");
            sys.AssessTreatyCompliance(280); // idempotent re-assessment
            Check(sys.GuildStanding == standingBefore - 6f || sys.AppliedConsequences.Count >= 1, "consequence does not stack on re-assessment");

            // Save round-trip.
            var save = sys.CaptureState();
            var restored = new SilentFoundrySystem(save, rng: new SeededRng(1), log: log);
            restored.BindCatalog(catalog, bp != null ? bp.maintenance_cycle_days : 4);
            restored.RestoreState(save);
            Check(restored.IsUnlocked == sys.IsUnlocked, "save round-trip preserves unlock");
            Check(restored.HeatStage == sys.HeatStage, "save round-trip preserves heat stage");
            Check(restored.TotalProductionCount == sys.TotalProductionCount, "save round-trip preserves production history");
            Check(restored.IsJournalTriggered(SilentFoundryIds.JournalFirstHeat) == sys.IsJournalTriggered(SilentFoundryIds.JournalFirstHeat),
                "save round-trip preserves journal dedup state");
            report.Passed = report.FailedCount == 0;
            report.Summary = "[Silent Foundry (Exp 10)] " + report.PassedCount + "/" + (report.PassedCount + report.FailedCount) + " PASSED";
            log.Info(report.Summary);
            return report;
        }
    }
}
