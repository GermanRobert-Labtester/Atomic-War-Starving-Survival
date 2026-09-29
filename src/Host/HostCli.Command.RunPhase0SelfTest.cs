// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Clock;
using Ashfall.Core.Crafting;
using Ashfall.Core.Economy;
using Ashfall.Core.Endgame;
using Ashfall.Core.Events;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Flags;
using Ashfall.Core.Legacy;
using Ashfall.Core.Medical;
using Ashfall.Core.Muster;
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;
using Ashfall.Core.Settings;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Ashfall.Core.UtilityAI;
using Ashfall.Core.Verdict;
using Ashfall.Core.Warlords;
using Ashfall.Core.World;
using Ashfall.Core.YearOfAsh;
using AtomicWar.GodotApp.Narrative;
using AtomicWar.GodotApp.Settings;
using AtomicWar.GodotApp.UI;
using AtomicWar.GodotApp.YearOfAsh;
using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {

        /// <summary>
        /// Phase-0 effects gate: phantom work-efficiency/refusal, somatic flashback
        /// work penalty, trade specialty mastery, final-wish permanent shelter
        /// buff, respiratory stamina penalty + ash-zone exposure, and a save
        /// write → reload → restore round-trip through the Phase0 save store.
        /// </summary>
        public static int RunPhase0SelfTest()
        {
            CatalogLocator.UseInvariantCulture();

            int failures = 0;
            void Check(bool condition, string name)
            {
                if (condition) GD.Print("[PASS] " + name);
                else { GD.Print("[FAIL] " + name); failures++; }
            }

            try
            {
                var session = new Phase0HostSession();
                session.SeedDemoRoster();
                session.RegisterDefaultRules();

                // The specialty catalog is the authority for milestone/mastery
                // narrative ids; without it the assertions below would see zero fires.
                bool foundPhase0Data =
                    CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out string phase0DataDir);
                if (!foundPhase0Data) { phase0DataDir = CatalogPath.ResolveDataDir(); foundPhase0Data = !string.IsNullOrEmpty(phase0DataDir); }
                Check(foundPhase0Data, "phase-0 selftest located StreamingAssets data directory");
                if (foundPhase0Data)
                    session.LoadTradeSpecialties(phase0DataDir);

                // ── 1. Phantom memory: motivation → work efficiency ─────
                session.ScavengeItem("survivor_gunner_mikhail", "item_dog_tags");
                float workMult = session.Phantom.GetWorkEfficiencyMultiplier("survivor_gunner_mikhail");
                Check(workMult == 1f || workMult == 1f + Ashfall.Core.PhantomMemoryEngine.MotivationWorkSpeedBonus,
                    "phantom work-efficiency multiplier is 1.0 or boosted");
                session.Phantom.TickHour("survivor_gunner_mikhail", 9f);
                Check(session.Phantom.GetWorkEfficiencyMultiplier("survivor_gunner_mikhail") == 1f,
                    "phantom work-efficiency decays back to 1.0");

                // Host view must track the decay too (aggregate is derived, not stale).
                session.TickHour(1f);
                float hostMult = session.GetEffects("survivor_gunner_mikhail").workEfficiencyMultiplier;
                Check(Math.Abs(hostMult - 1f) < 1e-4f,
                    "host work-efficiency view recomputes after boost decays");

                // ── 1b. PhantomMemoryHostSession.ScavengeItem direct probe ─────
                var phantomHost = new PhantomMemoryHostSession(new Ashfall.Core.PhantomMemoryEngine(), loadDefaults: true, rng: new Ashfall.Core.SeededRng(42));
                bool phantomStateChanged = false;
                phantomHost.StateChanged += () => phantomStateChanged = true;

                // A. Invalid item ID
                string invalidItemResult = phantomHost.ScavengeItem("survivor_gunner_mikhail", "");
                Check(invalidItemResult == "Invalid relic item ID.",
                    "PhantomMemoryHostSession.ScavengeItem rejects empty item ID");

                // B. Category token passed as item ID
                string catTokenResult = phantomHost.ScavengeItem("survivor_gunner_mikhail", "military");
                Check(catTokenResult == "Invalid item ID 'military': category tokens are not item IDs.",
                    "PhantomMemoryHostSession.ScavengeItem rejects category token 'military'");

                // C. Unknown survivor
                string unknownSvResult = phantomHost.ScavengeItem("nonexistent_survivor", "item_dog_tags");
                Check(unknownSvResult == "Unknown survivor.",
                    "PhantomMemoryHostSession.ScavengeItem rejects unknown survivor");

                // D. Deceased survivor
                var deceasedSv = new Ashfall.Core.PhantomSurvivorSnapshot
                {
                    survivorId = "sv_deceased_tester",
                    displayName = "Fallen Soldier",
                    backgroundId = "former_soldier",
                    isAlive = false
                };
                phantomHost.DemoSurvivors.Add(deceasedSv);
                string deceasedResult = phantomHost.ScavengeItem("sv_deceased_tester", "item_dog_tags");
                Check(deceasedResult == "Survivor is deceased and cannot inspect relics.",
                    "PhantomMemoryHostSession.ScavengeItem rejects deceased survivor");

                // E. Non-matching item (TriggerOutcome.None)
                string nonMatchingResult = phantomHost.ScavengeItem("survivor_gunner_mikhail", "clean_water");
                Check(nonMatchingResult == "No memory triggered. The item is just an object."
                    && phantomHost.LastEvent == nonMatchingResult,
                    "PhantomMemoryHostSession.ScavengeItem handles non-matching relic without memory trigger");

                // F. Matching relic with guaranteed trigger
                phantomHost.Engine.TriggerChanceOverride = 1.0f;
                phantomStateChanged = false;
                string triggeredResult = phantomHost.ScavengeItem("survivor_gunner_mikhail", "item_dog_tags");
                bool validTriggerText = triggeredResult.Contains("Gunner Mikhail (Heavy Artillery Loader)")
                    && (triggeredResult.Contains("pockets the tags") || triggeredResult.Contains("reads the name on the tag"));
                Check(validTriggerText
                    && phantomHost.LastEvent == triggeredResult
                    && phantomStateChanged
                    && phantomHost.IsDirty,
                    "PhantomMemoryHostSession.ScavengeItem resolves vignette, updates LastEvent, marks dirty, and fires StateChanged");

                // G. Inventory binding check: missing item
                var invSystem = new Ashfall.Core.Inventory.Inventory();
                var invCatalog = new Ashfall.Core.Inventory.ItemCatalog();
                var invDesc = new Ashfall.Core.Inventory.ItemDescriptionCatalog();
                var invSession = new InventoryHostSession(invSystem, invCatalog, invDesc);
                phantomHost.BindInventory(invSession);
                string invMissingResult = phantomHost.ScavengeItem("survivor_gunner_mikhail", "item_dog_tags");
                Check(invMissingResult == "Item 'item_dog_tags' not present in shelter inventory.",
                    "PhantomMemoryHostSession.ScavengeItem enforces inventory presence when InventorySession bound");

                // H. Inventory binding check: present item without consumption
                invSystem.Add(new Ashfall.Core.Inventory.ItemDefinition { id = "item_dog_tags", displayName = "Dog Tags", stackMax = 10 }, 1);
                string invPresentResult = phantomHost.ScavengeItem("survivor_gunner_mikhail", "item_dog_tags");
                Check(invPresentResult.Contains("Gunner Mikhail (Heavy Artillery Loader)")
                    && invSystem.CountById("item_dog_tags") == 1,
                    "PhantomMemoryHostSession.ScavengeItem succeeds without consuming item from shelter inventory");

                // I. PhantomMemoryHostSession.TickDemo direct probe
                phantomStateChanged = false;
                long preTickVersion = phantomHost.StateVersion;
                string tickResult = phantomHost.TickDemo();
                Check(tickResult == "Phantom timers ticked."
                    && phantomHost.LastEvent == "Phantom timers ticked."
                    && phantomStateChanged
                    && phantomHost.IsDirty
                    && phantomHost.StateVersion > preTickVersion,
                    "PhantomMemoryHostSession.TickDemo updates LastEvent, marks dirty, and fires StateChanged");

                // J. TickDemo timer decrement across multiple hours
                // Force a known motivation boost on Mikhail (8 hours)
                phantomHost.Engine.RegisterRule("former_soldier", "military", 1.0f, "desc",
                    "{name} stands tall.", "{name} falters.");
                phantomHost.Engine.TriggerChanceOverride = 1.0f;
                phantomHost.ScavengeItem("survivor_gunner_mikhail", "item_dog_tags");
                Check(phantomHost.Engine.HasMotivationBoost("survivor_gunner_mikhail"),
                    "PhantomMemoryHostSession motivation boost active before tick decay");

                // Tick 8 hours so the motivation boost fully decays to 0
                for (int t = 0; t < 8; t++)
                {
                    phantomHost.TickDemo();
                }
                Check(!phantomHost.Engine.HasMotivationBoost("survivor_gunner_mikhail")
                    && phantomHost.Engine.GetWorkEfficiencyMultiplier("survivor_gunner_mikhail") == 1.0f,
                    "PhantomMemoryHostSession.TickDemo decays motivation boost and restores base efficiency");

                // K. LoadData error path probe: missing directory
                bool missingLoad = phantomHost.LoadData("invalid/nonexistent/dir");
                Check(!missingLoad, "PhantomMemoryHostSession.LoadData returns false for non-existent directory");

                // L. LoadRulesFromJson error path probe: simulated I/O fault caught, reported, and returns false
                var faultIo = new PanelTestFaultyFileIo();
                string capturedError = string.Empty;
                bool faultResult = PhantomMemoryHostSession.LoadRulesFromJson(
                    new Ashfall.Core.PhantomMemoryEngine(),
                    phase0DataDir,
                    faultIo,
                    new Ashfall.Core.SystemTextJsonSerializer(),
                    err => capturedError = err);
                Check(!faultResult && capturedError.Contains("Simulated I/O disk error"),
                    "PhantomMemoryHostSession.LoadRulesFromJson catches I/O exception, invokes onError, and returns false");

                // M. LoadRulesFromJson error path probe: malformed JSON syntax caught, reported, and returns false
                var corruptIo = new PanelTestCorruptJsonFileIo();
                string capturedCorruptError = string.Empty;
                bool corruptResult = PhantomMemoryHostSession.LoadRulesFromJson(
                    new Ashfall.Core.PhantomMemoryEngine(),
                    phase0DataDir,
                    corruptIo,
                    new Ashfall.Core.SystemTextJsonSerializer(),
                    err => capturedCorruptError = err);
                Check(!corruptResult && capturedCorruptError.Contains("[PhantomMemory] Failed to load rules:"),
                    "PhantomMemoryHostSession.LoadRulesFromJson catches malformed JSON and returns false");

                // N. Create fallback probe: invalid path falls back to default built-in rules
                var fallbackHost = PhantomMemoryHostSession.Create("invalid/nonexistent/path");
                Check(fallbackHost != null && fallbackHost.Engine.GetRules("former_soldier").Count > 0,
                    "PhantomMemoryHostSession.Create falls back to default rules when JSON loading fails");

                // ── 2. Somatic flashback: real shelter proximity grounding ─
                var shelterAssignments = new Ashfall.Core.Shelter.ShelterAssignmentSystem(
                    new Ashfall.Core.Shelter.ShelterAssignmentState(),
                    new System.Collections.Generic.List<Ashfall.Core.Shelter.ShelterRoom>
                    {
                        new Ashfall.Core.Shelter.ShelterRoom("room_bunks", "Bunks", 4),
                        new Ashfall.Core.Shelter.ShelterRoom("room_kitchen", "Kitchen", 2)
                    },
                    new CoreSeededRng(99));

                session.BindShelterAssignment(shelterAssignments);

                var flash = session.Flashbacks;
                flash.GetAliveSurvivorIds = () => new[] { "sv_a", "sv_b", "sv_c" };

                // A. Apart / Unassigned: sv_a in bunks, sv_c in kitchen, sv_b unassigned
                shelterAssignments.Assign("sv_a", "room_bunks");
                shelterAssignments.Assign("sv_c", "room_kitchen");

                flash.IncreaseSusceptibility("sv_a", 1f);
                flash.OnAudioEvent("siren", 10f);
                float ungroundedPenalty = flash.GetWorkEfficiencyPenalty("sv_a");
                Check(Math.Abs(ungroundedPenalty - Ashfall.Core.Survivors.SomaticFlashbackSystem.FlashbackWorkEfficiencyPenalty) < 1e-4f,
                    "ungrounded flashback has full work efficiency penalty (0.60) when companions apart/unassigned");

                // B. Reassignment: assign sv_b into room_bunks (together with sv_a)
                shelterAssignments.Assign("sv_b", "room_bunks");
                flash.OnAudioEvent("siren", 10f);
                float groundedPenalty = flash.GetWorkEfficiencyPenalty("sv_a");
                Check(Math.Abs(groundedPenalty - Ashfall.Core.Survivors.SomaticFlashbackSystem.GroundedWorkEfficiencyPenalty) < 1e-4f,
                    "flashback is grounded by companion in same room (penalty reduced to 0.10)");

                // ── 3. Trade specialty: milestones → mastery ───────────
                var firedNarratives = new List<string>();
                session.TradeSpecialty.FireNarrativeEvent = (id, sv) => firedNarratives.Add(id);
                session.CraftItem("elena_vasquez", "machinist", "wrench_standard");
                session.CraftItem("elena_vasquez", "machinist", "gear_standard");
                Check(session.TradeSpecialty.GetMasteryTier("elena_vasquez") == 2,
                    "trade specialty tier 2 after two matching crafts");
                session.CraftItem("elena_vasquez", "machinist", "lever_standard");
                Check(session.TradeSpecialty.HasMasteredTrade("elena_vasquez"),
                    "trade specialty mastered at 3 crafts");
                Check(firedNarratives.SequenceEqual(new[]
                    {
                        "narrative_machinist_milestone_1",
                        "narrative_machinist_milestone_2",
                        "narrative_machinist_mastery",
                        "narrative_trade_mastery_machinist"
                    }),
                    "each craft fired its authored milestone narrative, mastery fired both tier-3 and mastery ids");

                // ── 4. Final wish: permanent shelter morale buff ────────
                float buffBefore = session.PermanentShelterMoraleBuff;
                session.FinalWish.RegisterWish("parent", Ashfall.Core.Survivors.FinalWishSystem.WishBuildMemorial);
                session.FinalWish.DeclareTerminalPrognosis("survivor_dr_sarah_chen", "parent", true);
                session.FinalWish.AdvanceWishStep("survivor_dr_sarah_chen", "step_1");
                session.FinalWish.AdvanceWishStep("survivor_dr_sarah_chen", "step_2");
                session.FinalWish.AdvanceWishStep("survivor_dr_sarah_chen", "step_3");
                Check(session.PermanentShelterMoraleBuff >
                      buffBefore + Ashfall.Core.Survivors.FinalWishSystem.WishCompletedMoraleBuff - 0.5f,
                    "final wish completion applied permanent shelter morale buff");

                // ── 5. Respiratory: ash zone + stamina penalty ─────────
                session.IsInAshZone = true;
                session.Respiratory.GetOrCreate("survivor_gunner_mikhail");
                session.Respiratory.TickHours("survivor_gunner_mikhail", 24f);
                Check(session.Respiratory.RespiratoryDegradation("survivor_gunner_mikhail") > 0f,
                    "ash-zone exposure accumulates respiratory degradation");
                session.IsInAshZone = false;

                // ── 6. Guilt insomnia ──────────────────────────────────
                session.RecordGuilt("elena_vasquez", "choice_left_ally_behind", 0.9f);
                Check(session.Guilt.GetInsomniaSeverity("elena_vasquez") >= Ashfall.Core.Survivors.GuiltInsomniaSystem.HighSeverityThreshold,
                    "high-severity guilt raises insomnia severity");
                Check(session.GetEffects("elena_vasquez").guiltInsomniaSeverity > 0f,
                    "guilt insomnia severity reaches the derived host view");

                // ── 7. Combat trauma: survival raises hypervigilance ───
                session.RegisterCombatSurvived("survivor_gunner_mikhail");
                session.RegisterCombatSurvived("survivor_gunner_mikhail");
                float hyper = session.CombatTrauma.GetHypervigilanceLevel("survivor_gunner_mikhail");
                Check(hyper > 0f && hyper == session.GetEffects("survivor_gunner_mikhail").hypervigilance,
                    "combat survival raises hypervigilance in core and host view");

                // ── 8. Moral branching: choice decides a branch ────────
                session.RecordMoralChoice("survivor_dr_sarah_chen", true);
                session.RecordMoralChoice("survivor_dr_sarah_chen", true);
                session.RecordMoralChoice("survivor_dr_sarah_chen", true);
                session.RecordMoralChoice("survivor_dr_sarah_chen", true);
                session.RecordMoralChoice("survivor_dr_sarah_chen", true);
                var moralState = session.Moral.CaptureState();
                Check(moralState.Survivors.Exists(s => s.SurvivorId == "survivor_dr_sarah_chen"
                        && s.BranchDirection != Ashfall.Core.Survivors.MoralBranchDirection.Neutral),
                    "five moral choices decide a branch");

                // ── 9. Radiation phase progression: exposure → phase ──
                session.RadiationPhase.OnExposure("survivor_dr_sarah_chen", 120f);
                var phase = session.RadiationPhase.GetPhase("survivor_dr_sarah_chen");
                Check(phase != Ashfall.Core.Radiation.RadiationSicknessPhase.Healthy,
                    "radiation exposure moves survivor out of Healthy phase");
                Check(session.GetEffects("survivor_dr_sarah_chen").radiationPhase != "Healthy",
                    "radiation phase reaches the derived host view");

                // ── 10. Chemical dependency: substance → withdrawal penalty ──
                session.ConsumeSubstance("survivor_gunner_mikhail", "item_morphine", Ashfall.Core.Medical.ChemicalDependencyKind.Opioid);
                session.ConsumeSubstance("survivor_gunner_mikhail", "item_morphine", Ashfall.Core.Medical.ChemicalDependencyKind.Opioid);
                session.ConsumeSubstance("survivor_gunner_mikhail", "item_morphine", Ashfall.Core.Medical.ChemicalDependencyKind.Opioid);
                Check(session.Dependency.DependencyLevel("survivor_gunner_mikhail", "item_morphine") >= Ashfall.Core.Medical.ChemicalDependencySystem.DependencyThreshold,
                    "repeated doses form a dependency");
                bool detox = session.Dependency.BeginColdTurkey("survivor_gunner_mikhail", "item_morphine");
                Check(detox, "cold-turkey withdrawal begins for a dependent survivor");
                session.Dependency.TickHours("survivor_gunner_mikhail", 6f);
                Check(session.GetEffects("survivor_gunner_mikhail").dependencyCombatPenalty > 0f,
                    "withdrawal tremor penalty reaches the derived host view");

                // ── 11. Real-consumer wiring ────────────────────────────
                float moraleApplied = 0f;
                float staminaMult = 0f;
                float craftPenalty = 0f;
                int narratives = 0;
                session.Consumers = Phase0EffectConsumers.NoOp(
                    applyMoraleDelta: (sv, d) => moraleApplied += d,
                    applyStaminaDrainMultiplier: (sv, m) => staminaMult += m,
                    applyCraftingPenaltyFactor: (sv, f) => craftPenalty += f,
                    fireNarrativeEvent: (id, sv) => narratives++);
                session.Phantom.RegisterRule("former_soldier", "military", 0.40f, "d", "boost", "break");
                session.Phantom.TriggerChanceOverride = 1.0f; // force a trigger
                session.ScavengeItem("survivor_gunner_mikhail", "item_dog_tags");
                Check(moraleApplied != 0f, "phantom memory morale delta reaches the real consumer");
                session.Phantom.TriggerChanceOverride = -1f; // restore default
                // Use a fresh survivor (elena already mastered trade in section 3).
                session.TradeSpecialty.FireNarrativeEvent = (id, sv) => narratives++;
                session.CraftItem("survivor_gunner_mikhail", "electrician", "battery_cell");
                session.CraftItem("survivor_gunner_mikhail", "electrician", "wire_standard");
                session.CraftItem("survivor_gunner_mikhail", "electrician", "circuit_board");
                Check(narratives >= 1, "trade mastery narrative event reaches the real consumer");
                Check(craftPenalty == 0f, "dependency crafting penalty idle until withdrawal tick");

                // ── 12. Save round-trip ──────────────────────────────────
                var save = session.CaptureSave();
                Check(save != null && save.effects.Count >= 3, "phase-0 save captured effects");
                var fresh = new Phase0HostSession();
                fresh.RestoreSave(save!);
                Check(Math.Abs(fresh.PermanentShelterMoraleBuff - session.PermanentShelterMoraleBuff) < 1e-4f,
                    "permanent shelter morale buff restored");
                Check(fresh.TradeSpecialty.HasMasteredTrade("elena_vasquez"),
                    "trade mastery restored");
                Check(fresh.Guilt.GetInsomniaSeverity("elena_vasquez") > 0f,
                    "guilt insomnia restored");
                Check(fresh.CombatTrauma.GetHypervigilanceLevel("survivor_gunner_mikhail") == hyper,
                    "combat trauma restored");
                // Dependency ledger is owned by MedicalHostSession (MedicalSaveStore);
                // Phase0 restores the shared instance only, verified separately.
                Check(session.Dependency.HasActiveWithdrawal("survivor_gunner_mikhail"),
                    "chemical dependency withdrawal active on the shared authority");
                Check(fresh.RadiationPhase.GetPhase("survivor_dr_sarah_chen") == phase,
                    "radiation phase restored");
            }
            catch (Exception e)
            {
                Check(false, "phase0 selftest threw: " + e.Message);
            }

            return EmitSummary("phase0_selftest", failures == 0, failures == 0 ? 0 : 1, details: failures == 0 ? "PASS" : $"FAIL ({failures})");
        }

    }

    internal sealed class PanelTestFaultyFileIo : Ashfall.Core.IFileIO
    {
        public bool DirectoryExists(string path) => true;
        public bool FileExists(string path) => true;
        public string ReadAllText(string path) => throw new System.IO.IOException("Simulated I/O disk error");
        public void WriteAllText(string path, string contents) { }
        public string Combine(params string[] parts) => System.IO.Path.Combine(parts);
    }

    internal sealed class PanelTestCorruptJsonFileIo : Ashfall.Core.IFileIO
    {
        public bool DirectoryExists(string path) => true;
        public bool FileExists(string path) => true;
        public string ReadAllText(string path) => "{ not valid json syntax !!!";
        public void WriteAllText(string path, string contents) { }
        public string Combine(params string[] parts) => System.IO.Path.Combine(parts);
    }
}
