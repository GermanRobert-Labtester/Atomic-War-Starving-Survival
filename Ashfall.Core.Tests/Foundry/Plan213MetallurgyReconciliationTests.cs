// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Ashfall.Core;
using Ashfall.Core.Foundry;
using Xunit;

namespace Ashfall.Core.Tests.Foundry
{
    /// <summary>
    /// Plan 213 Phase 1 — metallurgy RECONCILIATION: the overlap contract is
    /// enforced by reflection (no duplicate metallurgy authority), the
    /// material catalog loads from real data with closed vocabularies,
    /// purity derives deterministically from the standard completion path,
    /// forging is a deterministic headless command sequence, provenance
    /// rides the existing production records (old saves = Standard, never
    /// recalculated), and the vehicle handoff is a read-only query.
    /// </summary>
    public sealed class Plan213MetallurgyReconciliationTests
    {
        private static string GetDataDir()
        {
            string candidate = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
            if (Directory.Exists(candidate)) return Path.GetFullPath(candidate);
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string check = Path.Combine(dir.FullName, "Assets/StreamingAssets/Data");
                if (Directory.Exists(check)) return check;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        // ── Reconciliation registry (§173.1 — enforced, not just documented) ──

        [Fact]
        public void NoDuplicateMetallurgyAuthority_ReflectionGate()
        {
            // Exactly one metallurgy aggregate may exist: the Silent Foundry
            // (with its partials). No second MetallurgySystem/Queue/SaveStore
            // type may ever be introduced.
            var coreTypes = typeof(SilentFoundrySystem).Assembly.GetTypes();
            // Forbidden: any NEW competing metallurgy aggregate. Pre-existing
            // sanctioned authorities (B66 heavy recipes, Plans 130-133 powder
            // system) keep their names; nothing else may claim the domain.
            string[] forbidden = { "MetallurgySystem", "FoundryProductionSystem", "MetallurgyQueue",
                "MetallurgySaveStore", "MetallurgyEngine", "AlloyCatalog", "ForgingSystem" };
            var offenders = coreTypes
                .Where(t => forbidden.Contains(t.Name, StringComparer.Ordinal))
                .Select(t => t.Name)
                .ToList();
            Assert.True(offenders.Count == 0,
                "duplicate metallurgy authority introduced: " + string.Join(", ", offenders));
        }

        [Fact]
        public void Plan213_Extends_TheExistingFoundryState_NoNewSaveStore()
        {
            // The forging session and provenance ride SilentFoundryState —
            // no Plan 213 save section was added to the registry.
            string[] forbiddenSections = { "alloys", "forging", "material_provenance", "plan213_metallurgy", "advanced_metallurgy" };
            Assert.DoesNotContain(Ashfall.Core.Save.SaveSectionRegistry.All,
                s => forbiddenSections.Contains(s.SectionKey, StringComparer.Ordinal));
        }

        // ── Material catalog ─────────────────────────────────────────

        [Fact]
        public void RealMaterialCatalog_Loads_SixProfiles_NoErrors()
        {
            var load = MaterialProfileCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.False(load.HasErrors, string.Join("; ", load.Errors));
            Assert.Equal(6, load.Materials.Count);
            foreach (var m in load.Materials)
            {
                Assert.InRange(m.durability_modifier_bp, MaterialProfileCatalogLoader.ModifierMinBp, MaterialProfileCatalogLoader.ModifierMaxBp);
                Assert.InRange(m.armor_modifier_bp, MaterialProfileCatalogLoader.ModifierMinBp, MaterialProfileCatalogLoader.ModifierMaxBp);
                Assert.InRange(m.corrosion_modifier_bp, MaterialProfileCatalogLoader.ModifierMinBp, MaterialProfileCatalogLoader.ModifierMaxBp);
                Assert.True(m.forging_sequence.Count > 0);
            }
        }

        [Fact]
        public void MaterialCatalog_IsPropertiesOnly_RecipesStayInMetallurgyRecipes()
        {
            // alloys_and_ores.json authors NO recipe fields (inputs/outputs/
            // fuel) — the single recipe authority remains metallurgy_recipes.json.
            var raw = File.ReadAllText(Path.Combine(GetDataDir(), MaterialProfileCatalogLoader.FileName));
            Assert.DoesNotContain("input_items", raw);
            Assert.DoesNotContain("result_item_id", raw);
            Assert.DoesNotContain("fuel_units", raw);
        }

        // ── Purity derivation ────────────────────────────────────────

        [Fact]
        public void PurityDerivation_IsBounded_Deterministic_AndSensible()
        {
            Assert.Equal(FoundryPurityTier.Exceptional, SilentFoundrySystem.DerivePurityTier(95f, 0f, 0f));
            Assert.Equal(FoundryPurityTier.High, SilentFoundrySystem.DerivePurityTier(80f, 10f, 10f));
            Assert.Equal(FoundryPurityTier.Standard, SilentFoundrySystem.DerivePurityTier(60f, 0f, 0f));
            Assert.Equal(FoundryPurityTier.Poor, SilentFoundrySystem.DerivePurityTier(40f, 0f, 0f));
            // Contamination and slag downgrade without unbounded effects.
            Assert.Equal(FoundryPurityTier.Standard, SilentFoundrySystem.DerivePurityTier(95f, 100f, 100f));
            // Extreme inputs stay clamped.
            Assert.Equal(FoundryPurityTier.Poor, SilentFoundrySystem.DerivePurityTier(-50f, 500f, 500f));
        }

        // ── End-to-end batch → purity → provenance → forging ────────

        /// <summary>
        /// Drive the standard heat machine through its authored stages:
        /// charge → preheat → AT HEAT (player taps here) → tapped → casting
        /// → cooling → complete. The tap is a player action; an untapped
        /// heat burns out after 3 days by design.
        /// </summary>
        private static void DriveHeatToCompletion(SilentFoundrySystem foundry, int startDay)
        {
            int day = startDay;
            int tapped = -1;
            for (int i = 0; i < 15; i++)
            {
                foundry.TickDaily(day);
                if (foundry.HeatStage == FoundryHeatStage.AtHeat && tapped < 0)
                {
                    foundry.TapAndCast(day);
                    tapped = day;
                }
                if (foundry.HeatStage == FoundryHeatStage.Complete) return;
                day++;
            }
        }

        private static (SilentFoundrySystem foundry, Func<int> day) CreateFoundry()
        {
            var state = new SilentFoundryState { unlocked = true };
            var foundry = new SilentFoundrySystem(state);
            // Sealed inventory (nothing held): charge checks must be satisfied
            // via the test's own counts, so wire a generous fake inventory.
            var held = new Dictionary<string, int>();
            foundry.BindInventory(
                getCount: id => held.TryGetValue(id, out var n) ? n : 0,
                canAdd: (_, _) => true,
                addItem: (id, n) => held[id] = held.TryGetValue(id, out var c) ? c + n : n,
                consume: (id, n) => { if (held.TryGetValue(id, out var c)) held[id] = Math.Max(0, c - n); });
            // Stock the standard charge materials.
            held["coal"] = 500; held["charcoal"] = 200; held["clean_water"] = 500;
            held["scrap_metal"] = 200; held["scrap_mechanical"] = 200;
            held["copper_wire_10m_of_10m"] = 100; held["scrap_electronic"] = 100;
            held["item_foundry_flux"] = 200;
            // Catalog + maintenance anchor (loader API matches FoundryExpansionProductTests).
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var production = SilentFoundryCatalogLoader.LoadProduction(GetDataDir(), files, json);
            var faction = SilentFoundryCatalogLoader.LoadFaction(GetDataDir(), files, json);
            var catalog = new SilentFoundryCatalog();
            catalog.Load(production, faction);
            foundry.BindCatalog(catalog, 4);
            var metallurgyCatalog = MetallurgyCatalogLoader.Load(GetDataDir(), files, json);
            foundry.BindMetallurgyCatalog(metallurgyCatalog);
            return (foundry, () => state.daysSinceMaintenance);
        }

        [Fact]
        public void HeavyBatch_Completes_WithPurityAndProvenanceStamps()
        {
            var (foundry, _) = CreateFoundry();
            var profiles = MaterialProfileCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            foundry.BindMaterialProfiles(MaterialProfileCatalogLoader.ToCatalog(profiles),
                new Dictionary<string, string> { { "item_metallurgy_iron_ingot", "material_reclaimed_iron" } });

            string result = foundry.StartHeavyBatch("metallurgy_iron_ingot", workers: 2, workerSkill: 0.8f, day: 1);
            Assert.DoesNotContain("not unlocked", result);
            Assert.DoesNotContain("Missing", result);
            DriveHeatToCompletion(foundry, startDay: 2);

            var record = foundry.CompletedProduction.LastOrDefault();
            Assert.NotNull(record);
            Assert.Equal("metallurgy_iron_ingot", record!.productId);
            // Provenance stamped from the real completion path.
            Assert.Equal("material_reclaimed_iron", record.materialProfileId);
            Assert.Contains(record.purity, new[] { "Poor", "Standard", "High", "Exceptional" });

            // The handoff query resolves the same provenance (D6 seam).
            Assert.True(foundry.TryGetLatestMaterialQuality("metallurgy_iron_ingot", out var quality));
            Assert.Equal("material_reclaimed_iron", quality.MaterialProfileId);
            Assert.Equal(900, quality.DurabilityModifierBp);
        }

        [Fact]
        public void ForgingSequence_IsDeterministicHeadlessCommandFlow()
        {
            var (foundry, _) = CreateFoundry();
            var profiles = MaterialProfileCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            foundry.BindMaterialProfiles(MaterialProfileCatalogLoader.ToCatalog(profiles),
                new Dictionary<string, string> { { "item_metallurgy_iron_ingot", "material_reclaimed_iron" } });

            foundry.StartHeavyBatch("metallurgy_iron_ingot", 2, 0.8f, 1);
            DriveHeatToCompletion(foundry, startDay: 2);
            Assert.NotNull(foundry.CompletedProduction.LastOrDefault());

            Assert.StartsWith("Forging started", foundry.BeginForging("metallurgy_iron_ingot", 13));
            // Perfect authored sequence for reclaimed iron: heat→shape→finish.
            foundry.SubmitForgingCommand(FoundryForgingCommand.Heat, 13);
            foundry.SubmitForgingCommand(FoundryForgingCommand.Shape, 13);
            foundry.SubmitForgingCommand(FoundryForgingCommand.Finish, 13);
            var result = foundry.CompleteForging(13);

            Assert.True(result.Accepted);
            Assert.Equal(3, result.MatchedCommands);
            Assert.Equal(3, result.ExpectedCommands);
            // Full sequence match × the batch's purity ladder factor
            // (Standard 1000, High 1100, Exceptional 1200) — never below 1000.
            Assert.InRange(result.FinalQualityPermille, 1000, 1300);

            // Paired determinism: an identical second run scores identically.
            var (foundry2, _) = CreateFoundry();
            foundry2.BindMaterialProfiles(MaterialProfileCatalogLoader.ToCatalog(profiles),
                new Dictionary<string, string> { { "item_metallurgy_iron_ingot", "material_reclaimed_iron" } });
            foundry2.StartHeavyBatch("metallurgy_iron_ingot", 2, 0.8f, 1);
            DriveHeatToCompletion(foundry2, startDay: 2);
            foundry2.BeginForging("metallurgy_iron_ingot", 13);
            foundry2.SubmitForgingCommand(FoundryForgingCommand.Heat, 13);
            foundry2.SubmitForgingCommand(FoundryForgingCommand.Shape, 13);
            foundry2.SubmitForgingCommand(FoundryForgingCommand.Finish, 13);
            var result2 = foundry2.CompleteForging(13);
            Assert.Equal(result.FinalQualityPermille, result2.FinalQualityPermille);
        }

        [Fact]
        public void Forging_WrongSequence_ScoresLower_ButStaysBounded()
        {
            var (foundry, _) = CreateFoundry();
            var profiles = MaterialProfileCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            foundry.BindMaterialProfiles(MaterialProfileCatalogLoader.ToCatalog(profiles),
                new Dictionary<string, string> { { "item_metallurgy_iron_ingot", "material_reclaimed_iron" } });

            foundry.StartHeavyBatch("metallurgy_iron_ingot", 2, 0.8f, 1);
            DriveHeatToCompletion(foundry, startDay: 2);

            foundry.BeginForging("metallurgy_iron_ingot", 13);
            foundry.SubmitForgingCommand(FoundryForgingCommand.Finish, 13);
            foundry.SubmitForgingCommand(FoundryForgingCommand.Finish, 13);
            foundry.SubmitForgingCommand(FoundryForgingCommand.Finish, 13);
            var bad = foundry.CompleteForging(13);
            Assert.True(bad.Accepted);
            // "finish" matches the authored third command; the first two miss.
            Assert.Equal(1, bad.MatchedCommands);
            // Bounded: 1000 × 1/3 matched × the batch's purity factor
            // (same integer order as the Core: sequence-permille first).
            int factor = bad.Purity switch
            {
                FoundryPurityTier.Exceptional => 1200,
                FoundryPurityTier.High => 1100,
                FoundryPurityTier.Standard => 1000,
                _ => 850
            };
            Assert.Equal((1000 * 1 / 3) * factor / 1000, bad.FinalQualityPermille);
        }

        [Fact]
        public void Forging_Gates_AreHonest()
        {
            var (foundry, _) = CreateFoundry();
            var profiles = MaterialProfileCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            foundry.BindMaterialProfiles(MaterialProfileCatalogLoader.ToCatalog(profiles),
                new Dictionary<string, string> { { "item_metallurgy_iron_ingot", "material_reclaimed_iron" } });
            // No batch yet → refused (after binding; unbound → honest message).
            Assert.Contains("No provenance-bearing batch", foundry.BeginForging("metallurgy_iron_ingot", 1));
            // Double-begin refused; complete without a pass refused.
            foundry.StartHeavyBatch("metallurgy_iron_ingot", 2, 0.8f, 1);
            DriveHeatToCompletion(foundry, startDay: 2);
            foundry.BeginForging("metallurgy_iron_ingot", 13);
            Assert.Contains("already in progress", foundry.BeginForging("metallurgy_iron_ingot", 13));
            foundry.CompleteForging(13);
            Assert.False(foundry.CompleteForging(14).Accepted);
        }

        // ── Save / restore + legacy migration ───────────────────────

        [Fact]
        public void SaveRestore_MidForging_PreservesSequenceExactly()
        {
            var (foundry, _) = CreateFoundry();
            var profiles = MaterialProfileCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            foundry.BindMaterialProfiles(MaterialProfileCatalogLoader.ToCatalog(profiles),
                new Dictionary<string, string> { { "item_metallurgy_iron_ingot", "material_reclaimed_iron" } });
            foundry.StartHeavyBatch("metallurgy_iron_ingot", 2, 0.8f, 1);
            DriveHeatToCompletion(foundry, startDay: 2);
            foundry.BeginForging("metallurgy_iron_ingot", 13);
            foundry.SubmitForgingCommand(FoundryForgingCommand.Heat, 13);
            foundry.SubmitForgingCommand(FoundryForgingCommand.Shape, 13);

            var saved = foundry.CaptureState();
            var restored = new SilentFoundrySystem(new SilentFoundryState());
            // Re-bind the composition-time material catalog on the new instance
            // (bindings are composition, not persisted state).
            restored.BindMaterialProfiles(MaterialProfileCatalogLoader.ToCatalog(profiles),
                new Dictionary<string, string> { { "item_metallurgy_iron_ingot", "material_reclaimed_iron" } });
            restored.RestoreState(saved);

            var session = restored.State.activeForging;
            Assert.NotNull(session);
            Assert.False(session!.completed);
            Assert.Equal(2, session.submitted.Count);
            Assert.Equal("heat", session.submitted[0]);
            Assert.Equal("shape", session.submitted[1]);
            // Finish the pass after restore — the sequence survives.
            restored.SubmitForgingCommand(FoundryForgingCommand.Finish, 14);
            var result = restored.CompleteForging(14);
            Assert.True(result.Accepted);
            Assert.Equal(3, result.MatchedCommands);
            Assert.InRange(result.FinalQualityPermille, 1000, 1300);
        }

        [Fact]
        public void OldItems_MigrateToDefaults_NeverRecalculated()
        {
            // A pre-213 save's completed records deserialize purity=Standard,
            // no profile, no craft pass — never recalculated randomly.
            var state = new SilentFoundryState
            {
                unlocked = true,
                completed = new List<FoundryProductionRecord>
                {
                    new FoundryProductionRecord { productId = "item_metallurgy_iron_ingot", amount = 2, tier = FoundryQualityTier.Good, completedDay = 40 }
                }
            };
            var foundry = new SilentFoundrySystem(state);
            var record = foundry.CompletedProduction[0];
            Assert.Equal(FoundryPurityNames.Standard, record.purity);
            Assert.Equal(string.Empty, record.materialProfileId);
            Assert.Equal(0, record.craftQualityPermille);
            // Without profiles bound, the handoff query reports absence.
            Assert.False(foundry.TryGetLatestMaterialQuality("item_metallurgy_iron_ingot", out _));
        }
    }
}
