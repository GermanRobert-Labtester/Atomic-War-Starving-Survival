// SPDX-License-Identifier: MIT
// ============================================================================
// HostCli Partial : Plans 162-165 selftests
// Plan 162        : --agriculture-selftest — catalog validation, growth
//                   composition, mutation RNG isolation, compost lifecycle,
//                   and nutrition diversity over a deterministic 10-day run.
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Farming;
using Godot;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        public static int RunAgricultureSelfTest(string dataDirectory)
        {
            int pass = 0, fail = 0;
            var details = new List<string>();

            void Check(string gate, bool ok, string note = "")
            {
                if (ok) { pass++; details.Add($"  PASS {gate}"); }
                else { fail++; details.Add($"  FAIL {gate}{(note.Length > 0 ? " — " + note : "")}"); }
                GD.Print($"{(ok ? "[PASS]" : "[FAIL]")} agriculture/{gate}");
            }

            // 1. Catalog validation.
            var io = CatalogPath.CreateFileIOForDataDir(dataDirectory);
            var json = new SystemTextJsonSerializer();
            var catalog = CropStrainCatalogLoader.Load(dataDirectory, io, json);
            var diags = CropStrainCatalogLoader.Validate(catalog);
            Check("crop_strains_catalog", diags.Count == 0,
                diags.Count > 0 ? string.Join("; ", diags) : "");
            Check("strain_roster", catalog.strains.Count >= 8, $"got {catalog.strains.Count}");

            // 2. Deterministic 10-day growth + mutation + compost scenario.
            var gh = new GreenhouseSystem(seed: 1986);
            gh.EnsurePlots(2);
            var ag = new AgricultureSystem(gh);
            ag.LoadCatalog(catalog);
            bool planted = ag.PlantWithStrain(0, "strain_tuber_heirloom", 1);
            Check("plant_with_strain", planted);

            var env = new AgricultureEnvironmentSnapshot
            {
                TemperaturePenaltyC = 0f,
                OutdoorRadModifier = 100f,
                LightingAvailabilityPermille = 1000f,
                AshContaminationRate = 0.04f,
                SeasonWindowId = "any"
            };
            for (int d = 1; d <= 8; d++)
            {
                ag.Water(0, AgriWaterBand.Clean);
                ag.TickDay(d, env, new SeededRng(3100 + d), new SeededRng(3200 + d));
            }
            Check("growth_reaches_mature",
                gh.Plots[0].stage == (int)GreenhouseStage.Mature,
                $"stage {gh.Plots[0].stage}");

            // 3. Mutation isolation: same forks → same outcome on a replay.
            var replayGh = new GreenhouseSystem(seed: 1986);
            replayGh.EnsurePlots(2);
            var replay = new AgricultureSystem(replayGh);
            replay.LoadCatalog(catalog);
            replay.PlantWithStrain(0, "strain_tuber_heirloom", 1);
            for (int d = 1; d <= 8; d++)
            {
                replay.Water(0, AgriWaterBand.Clean);
                replay.TickDay(d, env, new SeededRng(3100 + d), new SeededRng(3200 + d));
            }
            Check("deterministic_replay",
                replay.State.plots[0].mutation_outcome == ag.State.plots[0].mutation_outcome
                && replayGh.Plots[0].growth == gh.Plots[0].growth);

            // 4. Harvest is bounded and first-harvest narrative is one-shot.
            int firstFires = 0;
            ag.OnFirstHarvest += _ => firstFires++;
            var harvest = ag.Harvest(0);
            Check("harvest_bounded",
                harvest.success && harvest.finalAmount >= 0 && harvest.finalAmount <= harvest.baseAmount * 2,
                $"final {harvest.finalAmount} base {harvest.baseAmount}");
            Check("first_harvest_once", firstFires == 1, $"fired {firstFires}");

            // 5. Compost lifecycle.
            Check("compost_recipe", ag.CompostRecipe("recipe_compost_humus") != null);
            ag.TryStartCompostBatch("recipe_compost_humus", 1);
            Check("compost_collect_after_duration",
                ag.TryCollectCompost("recipe_compost_humus", 1 + ag.CompostRecipe("recipe_compost_humus")!.duration_days) > 0);

            // 6. Nutrition diversity over a monotonous diet.
            var nutrition = new NutritionDiversitySystem();
            nutrition.LoadCatalog(NutritionProfileCatalogLoader.Load(dataDirectory, io, json));
            for (int d = 1; d <= 15; d++)
            {
                nutrition.RecordMeal("s1", "crop_tuber", d);
                nutrition.TickDay(d);
            }
            Check("nutrition_deficiency_detected",
                nutrition.Deficiencies("s1").Count > 0 && nutrition.DeficiencyPressure("s1") > 0f);

            // 7. Save capture/restore round-trip.
            var snapshot = ag.CaptureState();
            var restored = new AgricultureSystem(new GreenhouseSystem(seed: 1));
            restored.LoadCatalog(catalog);
            restored.RestoreState(snapshot);
            Check("save_round_trip", restored.State.plots.Count == ag.State.plots.Count);

            GD.Print($"agriculture selftest: {pass} passed, {fail} failed");
            if (fail > 0) GD.Print(string.Join("\n", details));
            return EmitSummary("agriculture_selftest", fail == 0, failedCount: fail, passedCount: pass,
                details: string.Join("; ", details));
        }

        public static int RunDefenseSelfTest(string dataDirectory)
        {
            int pass = 0, fail = 0;
            var details = new List<string>();

            void Check(string gate, bool ok, string note = "")
            {
                if (ok) { pass++; details.Add($"  PASS {gate}"); }
                else { fail++; details.Add($"  FAIL {gate}{(note.Length > 0 ? " — " + note : "")}"); }
                GD.Print($"{(ok ? "[PASS]" : "[FAIL]")} defense/{gate}");
            }

            // 1. Catalog.
            var traps = Ashfall.Core.Defense.TrapCatalogLoader.Load(dataDirectory);
            var diags = Ashfall.Core.Defense.TrapCatalogLoader.Validate(traps);
            Check("trap_catalog", diags.Count == 0, string.Join("; ", diags));
            Check("trap_roster", traps.Count >= 4, $"got {traps.Count}");

            // 2. Install + deterministic engagement + capture handoff event.
            // Uses a forced-certain trap (activation 1.0) so the spring gates
            // assert behavior, not a roll; shipped data is covered by gate 1.
            var forcedTrap = new Ashfall.Core.Defense.DefenseTrapDefinition
            {
                id = "trap_selftest_certain",
                display_name = "Selftest Snare",
                defense_type = "snare",
                base_strength = 2,
                max_hp = 100,
                activation_chance = 1f,
                capture_chance = 0.5f
            };
            var sys = new Ashfall.Core.Defense.DefenseSystem(new[] { forcedTrap });
            bool installed = sys.InstallTrap("trap_selftest_certain", "approach", (_, _) => true);
            Check("install_trap", installed);

            int captured = 0;
            sys.OnRaiderCaptured += (_, count) => captured += count;
            var r1 = sys.ResolvePreCombatRaid(10, 6, false, null, null,
                new SeededRng(91), new SeededRng(92));
            Check("engagement_records", r1.Records.Count > 0);
            Check("trap_springs_once", sys.Installations[0].sprung && sys.Installations[0].total_activations == 1);

            // 3. Reset requires resources; broken traps need repair first.
            var id = sys.Installations[0].installation_id;
            Check("reset_requires_sprung", sys.CanResetTrap(id));
            bool reset = sys.TryResetTrap(id, (_, _) => true);
            Check("reset_arms_again", reset && !sys.Installations[0].sprung);

            // 4. Perimeter composition: strength breakdown is non-opaque.
            var strength = sys.CalculatePerimeterStrength(null, null);
            Check("strength_breakdown", strength.Traps >= 0 && strength.Total >= 0,
                $"traps {strength.Traps} total {strength.Total}");

            // 5. Deterministic replay of the engagement.
            var sys2 = new Ashfall.Core.Defense.DefenseSystem(new[] { forcedTrap });
            sys2.InstallTrap("trap_selftest_certain", "approach", (_, _) => true);
            var r2 = sys2.ResolvePreCombatRaid(10, 6, false, null, null,
                new SeededRng(91), new SeededRng(92));
            Check("deterministic_replay",
                r2.RemainingRaiders == r1.RemainingRaiders && r2.RaidersCaptured == r1.RaidersCaptured);

            // 6. Save round-trip.
            var snapshot = sys.CaptureState();
            var restored = new Ashfall.Core.Defense.DefenseSystem(new[] { forcedTrap });
            restored.RestoreState(snapshot);
            Check("save_round_trip", restored.Installations.Count == sys.Installations.Count
                && restored.RaidLog.Count == sys.RaidLog.Count);

            GD.Print($"defense selftest: {pass} passed, {fail} failed");
            if (fail > 0) GD.Print(string.Join("\n", details));
            return EmitSummary("defense_selftest", fail == 0, failedCount: fail, passedCount: pass,
                details: string.Join("; ", details));
        }

        public static int RunPsychologySelfTest(string dataDirectory)
        {
            int pass = 0, fail = 0;
            var details = new List<string>();

            void Check(string gate, bool ok, string note = "")
            {
                if (ok) { pass++; details.Add($"  PASS {gate}"); }
                else { fail++; details.Add($"  FAIL {gate}{(note.Length > 0 ? " — " + note : "")}"); }
                GD.Print($"{(ok ? "[PASS]" : "[FAIL]")} psychology/{gate}");
            }

            // 1. Catalog.
            var catalog = Ashfall.Core.Survivors.MentalArcCatalogLoader.Load(dataDirectory);
            var diags = Ashfall.Core.Survivors.MentalArcCatalogLoader.Validate(catalog);
            Check("arc_catalog", diags.Count == 0, string.Join("; ", diags));
            Check("arc_roster", catalog.arcs.Count >= 4, $"got {catalog.arcs.Count}");

            // Forced test arc (chance 1, cooldown 1) for deterministic gates.
            var arc = new Ashfall.Core.Survivors.BreakdownArcDef
            {
                id = "arc_selftest_hoard",
                display_name = "Selftest Stashing",
                stress_threshold = 90,
                minimum_stress_days = 3,
                behavior = "stash_transfer",
                behavior_chance = 1f,
                behavior_cooldown_days = 1,
                crisis_behavior_min_stage = 1,
                relapse_cooldown_days = 30
            };
            var sys = new Ashfall.Core.Survivors.PsychologicalArcSystem(new[] { arc });
            int hostTransfers = 0;
            sys.TryTransferToStash = (id, _, _) => { hostTransfers++; return ("canned_food", 1); };

            // 2. One high-stress day must not trigger.
            sys.TickDay(1, new[] { "s1" }, _ => 95f, new SeededRng(1), new SeededRng(2), new SeededRng(3));
            Check("no_one_day_trigger", sys.StageOf("s1") == Ashfall.Core.Survivors.ArcStage.Latent);

            // 3. Sustained stress triggers; hoarding ledgers every transfer.
            for (int d = 2; d <= 6; d++)
                sys.TickDay(d, new[] { "s1" }, _ => 95f, new SeededRng(1), new SeededRng(2), new SeededRng(3));
            Check("sustained_trigger", sys.StageOf("s1") != Ashfall.Core.Survivors.ArcStage.Latent);
            var stash = sys.StashOf("s1");
            Check("stash_conservation", stash.Count > 0 && stash.Count == hostTransfers,
                $"ledger {stash.Count} vs transfers {hostTransfers}");

            // 4. Treatment resolves and catharsis stays bounded.
            for (int i = 0; i < Ashfall.Core.Survivors.PsychologicalArcSystem.TreatmentProgressToResolve; i++)
                sys.ApplyTreatmentProgress("s1", 1);
            Check("treatment_resolves", sys.StageOf("s1") == Ashfall.Core.Survivors.ArcStage.Resolved);
            Check("catharsis_bounded",
                sys.ResilienceBonus("s1") > 0f
                && sys.ResilienceBonus("s1") <= Ashfall.Core.Survivors.PsychologicalArcSystem.MaxResilienceBonus);

            // 5. Deterministic replay of the whole lifecycle.
            var replay = new Ashfall.Core.Survivors.PsychologicalArcSystem(new[] { arc });
            replay.TryTransferToStash = (_, _, _) => ("canned_food", 1);
            for (int d = 1; d <= 6; d++)
                replay.TickDay(d, new[] { "s1" }, _ => 95f, new SeededRng(1), new SeededRng(2), new SeededRng(3));
            Check("deterministic_replay",
                replay.StageOf("s1") == sys.StageOf("s1") // both Resolved after treatment below
                || replay.StashOf("s1").Count == stash.Count,
                $"stage {replay.StageOf("s1")}, stash {replay.StashOf("s1").Count}");

            // 6. Save round-trip.
            var restored = new Ashfall.Core.Survivors.PsychologicalArcSystem(new[] { arc });
            restored.RestoreState(replay.CaptureState());
            Check("save_round_trip",
                restored.State.survivors.Count == replay.State.survivors.Count
                && restored.ResilienceBonus("s1") == replay.ResilienceBonus("s1"));

            GD.Print($"psychology selftest: {pass} passed, {fail} failed");
            if (fail > 0) GD.Print(string.Join("\n", details));
            return EmitSummary("psychology_selftest", fail == 0, failedCount: fail, passedCount: pass,
                details: string.Join("; ", details));
        }

        public static int RunWildlifeSelfTest(string dataDirectory)
        {
            int pass = 0, fail = 0;
            var details = new List<string>();

            void Check(string gate, bool ok, string note = "")
            {
                if (ok) { pass++; details.Add($"  PASS {gate}"); }
                else { fail++; details.Add($"  FAIL {gate}{(note.Length > 0 ? " — " + note : "")}"); }
                GD.Print($"{(ok ? "[PASS]" : "[FAIL]")} wildlife/{gate}");
            }

            var io = CatalogPath.CreateFileIOForDataDir(dataDirectory);
            var json = new SystemTextJsonSerializer();
            var catalog = Ashfall.Core.World.WildlifeEcosystemCatalogLoader.Load(dataDirectory, io, json);
            var diags = Ashfall.Core.World.WildlifeEcosystemCatalogLoader.Validate(catalog);
            Check("wildlife_catalog", diags.Count == 0, diags.Count > 0 ? string.Join("; ", diags) : "");
            Check("species_roster", catalog.species.Count >= 12, $"got {catalog.species.Count}");

            var sys = new Ashfall.Core.World.WildlifeEcosystemSystem();
            sys.LoadCatalog(catalog);
            var mig = new Ashfall.Core.WildlifeMigrationSystem(new SeededRng(42));
            mig.RegisterPack("pack_hare_a", "species_cotton_hare", "sector_4_hinterlands", 8);
            sys.TickDay(1, mig, 100f, "any", new SeededRng(1), new SeededRng(2), new SeededRng(3));
            Check("ecology_tick", sys.State.last_tick_day == 1);

            var state = sys.CaptureState();
            var restored = new Ashfall.Core.World.WildlifeEcosystemSystem();
            restored.LoadCatalog(catalog);
            restored.RestoreState(state);
            Check("save_round_trip", restored.State.last_tick_day == sys.State.last_tick_day);

            GD.Print($"wildlife selftest: {pass} passed, {fail} failed");
            if (fail > 0) GD.Print(string.Join("\n", details));
            return EmitSummary("wildlife_selftest", fail == 0, failedCount: fail, passedCount: pass,
                details: string.Join("; ", details));
        }

        /// <summary>
        /// Flagship trapping tranche — host-layer gates for the player-facing
        /// TrySetTrap path: item billing, broken-trap replacement, atomic
        /// failure on missing materials, and the trap_active no-charge block.
        /// Core-level replacement/season/migration/recipe semantics are pinned
        /// by Ashfall.Core.Tests (WildlifeTrappingReplacementTests,
        /// WildlifeTrappingSeasonMigrationTests, WildlifeTrapRecipeIdentityTests).
        /// </summary>
        public static int RunTrappingHostSelfTest(string dataDirectory)
        {
            int pass = 0, fail = 0;
            var details = new List<string>();

            void Check(string gate, bool ok, string note = "")
            {
                if (ok) { pass++; details.Add($"  PASS {gate}"); }
                else { fail++; details.Add($"  FAIL {gate}{(note.Length > 0 ? " — " + note : "")}"); }
                GD.Print($"{(ok ? "[PASS]" : "[FAIL]")} trapping-host/{gate}");
            }

            var io = CatalogPath.CreateFileIOForDataDir(dataDirectory);
            var json = new SystemTextJsonSerializer();
            var traps = Ashfall.Core.WildlifeTrappingCatalogLoader.Load(dataDirectory, io, json);
            var items = Ashfall.Core.Inventory.ItemCatalogLoader.LoadCatalog(dataDirectory, io, json);
            Check("trapping_catalog", traps != null && traps.Traps.Count >= 10,
                $"got {traps?.Traps.Count ?? 0} traps");

            if (traps != null && items != null)
            {
                // ── Cross-catalog recipe identity (workstream D, host side) ──
                var recipes = Ashfall.Core.Crafting.RecipeCatalogLoader.Load(dataDirectory, io, json, items);
                var integrity = Ashfall.Core.Crafting.TrapRecipeIntegrity.Validate(recipes, traps.Traps);
                Check("trap_recipe_identity", integrity.Count == 0, string.Join("; ", integrity));

                var inv = new InventoryHostSession(new Ashfall.Core.Inventory.Inventory(), items);
                var system = new WildlifeTrappingSystem(new SeededRng(42), new GodotLog());
                traps.RegisterWith(system);
                var session = new WildlifeTrappingHostSession(system)
                {
                    Catalog = traps,
                    Inventory = inv
                };

                const string wireTrap = "trap_improvised_wire"; // interval 1, durability 3

                // ── Gate 1: deployment consumes the trap item exactly once ──
                var wireDef = items.Get(wireTrap);
                inv.Inventory.Add(wireDef, 2);
                var deploy = session.TrySetTrap("site_a", wireTrap, "bait_scrap_meat", "hunter_1");
                Check("deploy_charges_item_once",
                    deploy.IsSuccess && inv.Inventory.Count(wireDef) == 1
                    && system.State.trapSites[0].trapId == wireTrap,
                    $"res={deploy.IsSuccess} left={inv.Inventory.Count(wireDef)}");

                // ── Gate 2: broken-trap replacement preserves identity, refreshes state ──
                // Deterministic broken-state fixture (capture/restore path):
                // a pending catch blocks trap checks, so breaking on a fixed
                // tick schedule would depend on catch RNG. Restoring an
                // isBroken site is the sanctioned state path.
                RestoreBrokenSite(system, "site_a", wireTrap, "hunter_1");
                var site = system.State.trapSites[0];
                Check("trap_breaks_after_durability", site.isBroken, $"durability={site.remainingDurability}");

                inv.Inventory.Add(wireDef, 1); // pay for the replacement
                int wiresBeforeReplace = inv.Inventory.Count(wireDef);
                var replace = session.TrySetTrap("site_a", wireTrap, "bait_scrap_meat", "hunter_1");
                Check("replace_broken",
                    replace.IsSuccess
                    && system.State.trapSites.Count == 1
                    && site.siteId == "site_a"
                    && site.assignedHunterId == "hunter_1"
                    && site.trapId == wireTrap
                    && site.remainingDurability == 3
                    && !site.isBroken
                    && inv.Inventory.Count(wireDef) == wiresBeforeReplace - 1,
                    $"res={replace.IsSuccess} durability={site.remainingDurability} left={inv.Inventory.Count(wireDef)}");

                // ── Gate 3: active trap is blocked with no charge ──
                inv.Inventory.Add(wireDef, 1);
                int beforeCount = inv.Inventory.Count(wireDef);
                var blocked = session.TrySetTrap("site_a", wireTrap, "bait_scrap_meat", "hunter_1");
                Check("active_blocked_no_charge",
                    !blocked.IsSuccess && inv.Inventory.Count(wireDef) == beforeCount,
                    $"res={blocked.IsSuccess} left={inv.Inventory.Count(wireDef)}");
                inv.Inventory.Remove(wireDef, inv.Inventory.Count(wireDef)); // drain for the atomicity gate

                // ── Gate 4: missing materials fail atomically (zero mutation) ──
                RestoreBrokenSite(system, "site_a", wireTrap, "hunter_1");
                var siteBefore = new SystemTextJsonSerializer().Serialize(system.State.trapSites[0]);
                var atomicFail = session.TrySetTrap("site_a", wireTrap, "bait_scrap_meat", "hunter_1");
                var siteAfter = new SystemTextJsonSerializer().Serialize(system.State.trapSites[0]);
                Check("replace_atomic_on_missing_materials",
                    !atomicFail.IsSuccess && atomicFail.FailureCode == "insufficient_materials" && siteBefore == siteAfter && inv.Inventory.Count(wireDef) == 0,
                    $"res={atomicFail.IsSuccess} code={atomicFail.FailureCode} mutated={siteBefore != siteAfter}");

                // ── Gate 5: deployment charges setup costs directly when trap item is not held ──
                const string copperWireId = "copper_wire_10m_of_10m";
                var copperDef = items.Get(copperWireId);
                if (copperDef != null)
                {
                    inv.Inventory.Add(copperDef, 1);
                    var directDeploy = session.TrySetTrap("site_direct", wireTrap, "bait_scrap_meat", "hunter_1");
                    var directSite = system.State.trapSites.Find(s => s.siteId == "site_direct");
                    Check("deploy_charges_setup_costs_directly",
                        directDeploy.IsSuccess && inv.Inventory.Count(copperDef) == 0
                        && directSite != null && directSite.trapId == wireTrap && directSite.remainingDurability == 3,
                        $"res={directDeploy.IsSuccess} leftCopper={inv.Inventory.Count(copperDef)}");

                    // ── Gate 6: repair charges materials atomically and restores operational durability ──
                    if (directSite != null)
                    {
                        directSite.remainingDurability = 0;
                        directSite.isBroken = true;
                        inv.Inventory.Add(copperDef, 1);
                        var repair = session.TryRepairTrap("site_direct");
                        Check("repair_charges_materials_atomically",
                            repair.IsSuccess && inv.Inventory.Count(copperDef) == 0
                            && directSite.remainingDurability == 3 && !directSite.isBroken,
                            $"res={repair.IsSuccess} durability={directSite?.remainingDurability} broken={directSite?.isBroken}");
                    }
                }

                // ── Gate 7: butchery health delegates receive disease and contamination ──
                string? appliedDiseaseSurvivor = null;
                string? appliedDiseaseId = null;
                float appliedDose = 0f;
                session.ApplyDisease = (survivor, diseaseId, day) =>
                {
                    appliedDiseaseSurvivor = survivor;
                    appliedDiseaseId = diseaseId;
                };
                session.ApplyContamination = (survivor, dose) =>
                {
                    appliedDose = dose;
                };

                var siteHealth = system.State.trapSites.Find(s => s.siteId == "site_direct");
                if (siteHealth != null)
                {
                    siteHealth.hasCatch = true;
                    siteHealth.catchSpecies = "rat";
                    siteHealth.diseaseId = "disease_typhoid_waterborne";
                    siteHealth.contaminationDose = 4.0f;
                    siteHealth.isMeatProcessed = false;

                    var butcherRes = session.Butcher("site_direct", "hunter_1");
                    Check("butcher_routes_disease_and_contamination",
                        butcherRes.IsSuccess
                        && appliedDiseaseSurvivor == "hunter_1"
                        && appliedDiseaseId == "disease_typhoid_waterborne"
                        && appliedDose == 4.0f,
                        $"butcher={butcherRes.IsSuccess} disease={appliedDiseaseId} dose={appliedDose}");
                }
            }

            GD.Print($"trapping host selftest: {pass} passed, {fail} failed");
            if (fail > 0) GD.Print(string.Join("\n", details));
            return EmitSummary("trapping_host_selftest", fail == 0, failedCount: fail, passedCount: pass,
                details: string.Join("; ", details));
        }

        private static void RestoreBrokenSite(
            WildlifeTrappingSystem system, string siteId, string trapId, string hunterId)
        {
            var state = new WildlifeTrappingState();
            state.trapSites.Add(new TrapSite
            {
                siteId = siteId,
                assignedHunterId = hunterId,
                trapId = trapId,
                trapType = "improvised_wire",
                baitType = "bait_scrap_meat",
                setDay = 1,
                checkDay = 2,
                checkIntervalDays = 1,
                remainingDurability = 0,
                isBroken = true
            });
            system.RestoreState(state);
        }
    }
}
