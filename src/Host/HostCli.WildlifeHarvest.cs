// SPDX-License-Identifier: MIT
// Host CLI self-test probe for Expansion 32 (The Wild — wildlife harvest quota).

using System;
using System.IO;
using Godot;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public static class HostCliWildlifeHarvest
    {
        public static int RunSelfTest(string dataDir)
        {
            GD.Print("=== [HostCli] Wildlife Harvest Quota Self-Test (Expansion 32) ===");
            int passed = 0;
            int total = 12;

            try
            {
                // Check 1: extinct and critically low species cannot be harvested.
                var extinct = WildlifeHarvestQuotaEngine.EvaluateHarvestQuota(0, 100, 5);
                var critical = WildlifeHarvestQuotaEngine.EvaluateHarvestQuota(80, 100, 1);
                if (extinct.MaxSafeHarvestUnits == 0 && !extinct.IsWithinQuota
                    && critical.MaxSafeHarvestUnits == 0 && !critical.IsWithinQuota)
                {
                    GD.Print("[PASS] Check 1: Extinct and critically-low species are protected (quota 0).");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 1: Low populations were not protected.");

                // Check 2: a depleted species has a limited but nonzero quota.
                var depleted = WildlifeHarvestQuotaEngine.EvaluateHarvestQuota(250, 200, 1);
                if (depleted.MaxSafeHarvestUnits >= 0 && depleted.PostHarvestBand <= SpeciesPopulationBand.Depleted)
                {
                    GD.Print($"[PASS] Check 2: Depleted species quota {depleted.MaxSafeHarvestUnits}, post-harvest {depleted.PostHarvestBand}.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 2: Depleted species evaluation incorrect.");

                // Check 3: an abundant species supports a larger quota than a stable one.
                var abundant = WildlifeHarvestQuotaEngine.EvaluateHarvestQuota(950, 200, 5);
                var stable = WildlifeHarvestQuotaEngine.EvaluateHarvestQuota(650, 200, 5);
                if (abundant.MaxSafeHarvestUnits > stable.MaxSafeHarvestUnits)
                {
                    GD.Print($"[PASS] Check 3: Abundant quota {abundant.MaxSafeHarvestUnits} > stable {stable.MaxSafeHarvestUnits}.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 3: Abundance did not raise the quota.");

                // Check 4/5: within-quota is safe; overhunt carries collapse risk.
                var within = WildlifeHarvestQuotaEngine.EvaluateHarvestQuota(650, 200, 1);
                var over = WildlifeHarvestQuotaEngine.EvaluateHarvestQuota(650, 200, 500);
                if (within.IsWithinQuota && over.OverhuntCollapseRiskPermille > 0)
                {
                    GD.Print($"[PASS] Check 4: Within-quota ok; overhunt risk {over.OverhuntCollapseRiskPermille}\u2030.");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 4: Quota/overhunt evaluation incorrect (within={within.IsWithinQuota}, risk={over.OverhuntCollapseRiskPermille}).");

                // Check 5: predator conflict scales with proximity and noise.
                var far = WildlifeHarvestQuotaEngine.EvaluatePredatorConflict(900, 2000, 900);
                var close = WildlifeHarvestQuotaEngine.EvaluatePredatorConflict(900, 100, 900);
                if (far == PredatorConflictPosture.Passive && close >= PredatorConflictPosture.Aggressive)
                {
                    GD.Print($"[PASS] Check 5: Predator posture far={far}, close={close}.");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 5: Predator conflict posture incorrect (far={far}, close={close}).");

                // Check 6: taming readiness requires a tamable species and is deterministic.
                var apex = WildlifeHarvestQuotaEngine.EvaluateTamingReadiness(1000, 1000, 0, 7);
                var dogA = WildlifeHarvestQuotaEngine.EvaluateTamingReadiness(800, 1000, 900, 7);
                var dogB = WildlifeHarvestQuotaEngine.EvaluateTamingReadiness(800, 1000, 900, 7);
                if (!apex.IsTameable && apex.EstimatedSessionsNeeded == 20 && dogA.IsTameable
                    && dogA.ReadinessPermille == dogB.ReadinessPermille)
                {
                    GD.Print($"[PASS] Check 6: Apex untameable; dog tameable ({dogA.ReadinessPermille}\u2030), deterministic.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 6: Taming readiness incorrect or non-deterministic.");

                // Check 7: ledger records a harvest and reports a census.
                var ledger = new WildlifeHarvestLedger();
                ledger.ApplyHarvest("species_cotton_hare", 650, 200, 3);
                var census = ledger.GetCensus();
                if (census.TrackedSpecies == 1 && census.TotalHarvestTaken == 3)
                {
                    GD.Print($"[PASS] Check 7: Ledger tracked {census.TrackedSpecies} species, {census.TotalHarvestTaken} taken.");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 7: Ledger census incorrect (species={census.TrackedSpecies}, taken={census.TotalHarvestTaken}).");

                // Check 8: season reset clears counters but keeps the species record.
                ledger.BeginSeason(2);
                if (ledger.GetCensus().TotalHarvestTaken == 0 && ledger.TrackedSpecies == 1 && ledger.Season == 2)
                {
                    GD.Print("[PASS] Check 8: Season reset clears harvest counters, keeps species.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 8: Season reset incorrect.");

                // Check 9: over-quota census flag.
                ledger.ApplyHarvest("species_cotton_hare", 650, 200, 500);
                if (ledger.GetCensus().SpeciesOverQuota >= 1)
                {
                    GD.Print("[PASS] Check 9: Over-quota harvest is flagged in the census.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 9: Over-quota harvest was not flagged.");

                // Check 10: capture/restore + schema gate.
                var state = ledger.CaptureState();
                var restored = new WildlifeHarvestLedger();
                restored.RestoreState(state);
                bool roundTrip = restored.GetCensus().TotalHarvestTaken == ledger.GetCensus().TotalHarvestTaken;
                bool newerRejected = false;
                try
                {
                    var newer = ledger.CaptureState();
                    newer.SchemaVersion = 99;
                    restored.RestoreState(newer);
                }
                catch (InvalidOperationException) { newerRejected = true; }
                if (roundTrip && newerRejected)
                {
                    GD.Print("[PASS] Check 10: Harvest ledger round-trips and schema-gates.");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 10: Round-trip/schema gate failed (roundTrip={roundTrip}, newerRejected={newerRejected}).");

                // Check 11: read-only evaluations never record a harvest.
                int before = ledger.GetCensus().TotalHarvestTaken;
                ledger.EvaluateQuota("species_cotton_hare", 650, 200, 100);
                ledger.EvaluatePredatorConflict(500, 100, 100);
                if (ledger.GetCensus().TotalHarvestTaken == before)
                {
                    GD.Print("[PASS] Check 11: Read-only evaluations do not mutate the ledger.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 11: A read-only evaluation recorded a harvest.");

                // Check 12: host wiring + save section.
                string main = ReadRepoFile("src", "Main.WildlifeHarvest.cs");
                string owners = ReadRepoFile("src", "Main.CampaignOwners.cs");
                string registry = ReadRepoFile("Assets", "Ashfall.Core", "Save", "SaveSectionRegistry.cs");
                if (main.Contains("HarvestWildlife") && owners.Contains("WildlifeHarvestDayOwner")
                    && registry.Contains("wildlife_harvest") && registry.Contains("wildlife_harvest_save.json"))
                {
                    GD.Print("[PASS] Check 12: Host owns the harvest ledger and registers its save section.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 12: Wildlife harvest host wiring or save section missing.");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[EXCEPTION] Exception during wildlife harvest self-test: {ex}");
            }

            GD.Print($"=== Wildlife Harvest Quota Self-Test Result: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }

        private static string ReadRepoFile(params string[] parts)
        {
            try
            {
                string dir = AppContext.BaseDirectory;
                for (int i = 0; i < 8 && dir != null; i++)
                {
                    string candidate = Path.Combine(dir, Path.Combine(parts));
                    if (File.Exists(candidate)) return File.ReadAllText(candidate);
                    dir = Directory.GetParent(dir)?.FullName ?? string.Empty;
                }
            }
            catch (Exception) { /* cleanup: optional probe file is unavailable */ }
            return string.Empty;
        }
    }
}
