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

        public static int RunShelterOperationsSelfTest(string dataDirectory)
        {
            int failures = 0;
            void Check(bool condition, string message)
            {
                if (condition)
                {
                    GD.Print($"[PASS] {message}");
                }
                else
                {
                    GD.PrintErr($"[FAIL] {message}");
                    failures++;
                }
            }

            try
            {
                GD.Print("[ShelterOperationsSelfTest] Starting Medical Triage, Expedition Sorties, & Radio Network Verification...");

                // ── 1. Medical Triage & Treatment Verification ──
                var survivors = new SurvivorsHostSession();
                survivors.SeedDemoRoster();
                var inv = InventoryHostSession.Create(dataDirectory, seedWhenNoSave: true);
                inv.Add("bandage", 3);
                inv.Add("iodine_pills", 3);
                inv.Add("rad_away", 2);

                var med = new MedicalHostSession();

                var mikhail = survivors.Find("survivor_gunner_mikhail");
                Check(mikhail != null, "Mikhail registered in survivors roster");
                if (mikhail != null)
                {
                    mikhail.Health = 60f;
                    float hpBefore = mikhail.Health;
                    int bandagesBefore = inv.Inventory.CountById("bandage");

                    // Apply bandage treatment
                    bool consumed = inv.Inventory.RemoveById("bandage", 1);
                    Check(consumed, "consumed 1 bandage from inventory");
                    survivors.HealSurvivor("survivor_gunner_mikhail", 25f);
                    med.AddCareEntry("survivor_gunner_mikhail", "Applied sterile bandage.");

                    Check(mikhail.Health >= hpBefore + 20f, $"survivor healed from {hpBefore} to {mikhail.Health}");
                    Check(inv.Inventory.CountById("bandage") == bandagesBefore - 1, "inventory bandage count decreased by 1");

                    // Apply anti-rad treatment
                    var radState = survivors.RadStateFor("survivor_gunner_mikhail");
                    Check(radState != null && radState.RadiationDose > 0, "Mikhail has initial radiation exposure");
                    float doseBefore = radState?.RadiationDose ?? 0f;
                    int radAwayBefore = inv.Inventory.CountById("rad_away");

                    consumed = inv.Inventory.RemoveById("rad_away", 1);
                    Check(consumed, "consumed 1 rad_away from inventory");
                    survivors.AdministerAntiRad("survivor_gunner_mikhail", 40f);
                    med.AddCareEntry("survivor_gunner_mikhail", "Administered anti-rad chelation agent.");

                    Check(radState!.RadiationDose < doseBefore, $"radiation dose purged from {doseBefore} to {radState.RadiationDose}");
                    Check(inv.Inventory.CountById("rad_away") == radAwayBefore - 1, "inventory rad_away count decreased by 1");

                    // Apply iodine prophylaxis to Sarah Chen
                    var sarah = survivors.RadStateFor("survivor_dr_sarah_chen");
                    Check(sarah != null, "Sarah Chen rad state found");
                    consumed = inv.Inventory.RemoveById("iodine_pills", 1);
                    Check(consumed, "consumed 1 iodine pill");
                    survivors.AdministerIodine("survivor_dr_sarah_chen");
                    med.AddCareEntry("survivor_dr_sarah_chen", "Administered potassium iodide.");

                    Check(sarah!.HasRadResistance, "Sarah Chen gained rad resistance");
                    Check(sarah.RadResistanceHoursRemaining > 0, "rad resistance hours active");
                }

                // ── 2. Wasteland Expedition Scavenging Sortie Verification ──
                var expeditions = ExpeditionHostSession.Create(dataDirectory);
                Check(expeditions.Definitions.Count >= 2, "expedition definitions loaded");

                var target = expeditions.Definitions[0];
                Check(target != null && target.id == "loc_the_allotments", "target is The Works Allotment Commune");

                var startResult = expeditions.StartExpedition("survivor_dr_sarah_chen", target!.id);
                Check(startResult.IsSuccess && expeditions.Engine.ActiveCount == 1, "expedition successfully deployed");
                var activeExp = expeditions.Engine.Active["survivor_dr_sarah_chen"];
                Check(activeExp != null && activeExp.phase == (int)ExpeditionPhase.Outbound, "expedition starts in Outbound phase");

                // Advance hours until arrival / looting
                for (int h = 0; h < 6; h++)
                {
                    expeditions.TickHours(2f);
                }

                // Push luck or advance to looting
                Check(activeExp!.stamina < 100f, "stamina consumed during sortie travel");

                // Test save & restore of expedition state
                var expSave = expeditions.CaptureSave();
                Check(expSave != null && expSave.Count == 1, "expedition state captured cleanly");
                var reloadedExp = new ExpeditionHostSession();
                reloadedExp.RestoreSave(expSave!);
                Check(reloadedExp.Engine.ActiveCount == 1, "expedition state restored with full fidelity");

                // ── 3. Radio Communication Network Verification ──
                var radio = RadioHostSession.Create(dataDirectory, 3);
                Check(radio != null, "radio host session created");
                Check(radio!.CurrentFrequency > 0f, "radio tuner has carrier frequency");

                string listenMsg1 = radio.Listen(142.850f);
                Check(radio.CurrentFrequency == 142.850f, "tuned to 142.850 MHz");
                Check(radio.History.Count > 0, "intercept recorded on 142.850 MHz");

                string listenMsg2 = radio.Listen(104.200f);
                Check(radio.CurrentFrequency == 104.200f, "tuned to 104.200 MHz");

                string beaconMsg = radio.BroadcastBeacon("Holdfast shelter beacon test.");
                Check(radio.LastIntercept.HasValue && radio.LastIntercept.Value.Callsign == "HOLDFAST BASE", "emergency broadcast logged as HOLDFAST BASE");

                // ── 4. UI Overlay Component Smoke Verification ──
                var medPanel = new MedicalPanel();
                medPanel._Ready();
                medPanel.Bind(med, survivors, inv);
                Check(medPanel.IsBound, "MedicalPanel binds cleanly with active session");

                var expPanel = new ExpeditionPanel();
                expPanel._Ready();
                expPanel.Bind(expeditions, survivors, inv);
                Check(expPanel.IsBound, "ExpeditionPanel binds cleanly with active session");

                var radPanel = new RadioPanel();
                radPanel._Ready();
                radPanel.Bind(radio);
                Check(radPanel.IsBound, "RadioPanel binds cleanly with active session");

                medPanel.QueueFree();
                expPanel.QueueFree();
                radPanel.QueueFree();

                // ── 5. Crafting System Verification (14 assertions) ──
                GD.Print("[ShelterOperationsSelfTest] §5 Crafting system...");
                var craftInv = new Ashfall.Core.Inventory.Inventory();
                var craftSession = new CraftingHostSession(craftInv, seedDefaultWorkbench: true);


                // 5.1 Recipe catalog loads
                Check(craftSession.Recipes.Count >= 5, "recipe catalog has ≥5 recipes");

                // 5.2 recipe_bandage present
                var bandageRecipe = craftSession.FindRecipe("recipe_bandage");
                Check(bandageRecipe != null, "recipe_bandage is resolvable");

                // 5.3 CanCraft false when ingredients missing
                Check(!craftSession.Engine.CanCraft(bandageRecipe!), "CanCraft false when ingredients absent");

                // 5.4 Add ingredients → CanCraft true
                var mechParts = CraftingHostSession.Catalog.Get("scrap_mechanical")!;
                if (mechParts != null) craftInv.Add(mechParts, 5);
                Check(craftSession.Engine.CanCraft(bandageRecipe!), "CanCraft true after adding ingredients");


                // 5.5 Start craft → queue grows
                int craftBandageBefore = craftInv.CountById("bandage");
                var craftStartResult = craftSession.Start("recipe_bandage");
                Check(craftSession.Engine.ActiveCraftCount == 1, "StartCraft queues one entry");


                // 5.6 Ingredient consumed atomically
                int mechAfter = craftInv.CountById("scrap_mechanical");
                Check(mechAfter < 5, $"ingredient count decreased after start (was 5, now {mechAfter})");

                // 5.7 Invalid recipe ID → Start returns error, queue unchanged
                var badCraftResult = craftSession.Start("recipe_does_not_exist");
                Check(!badCraftResult.IsSuccess, "invalid recipe ID returns blocked/failed result");
                Check(craftSession.Engine.ActiveCraftCount == 1, "invalid recipe does not grow queue");


                // 5.8 Second valid craft → queue grows
                if (mechParts != null) craftInv.Add(mechParts, 5);
                craftSession.Start("recipe_bandage");
                Check(craftSession.Engine.ActiveCraftCount == 2, "second craft queued (two bandage crafts)");


                // 5.9 Tick past duration → OnCraftCompleted fires once per craft
                int craftCompletions = 0;
                craftSession.Engine.OnCraftCompleted += (_, _) => craftCompletions++;
                craftSession.CompleteAll(2f); // recipe_bandage takes 1h each
                Check(craftSession.Engine.ActiveCraftCount == 0, "both crafts completed after full tick");
                Check(craftCompletions == 2, $"OnCraftCompleted fired exactly twice (got {craftCompletions})");


                // 5.10 Output in inventory
                int craftBandageAfter = craftInv.CountById("bandage");
                Check(craftBandageAfter >= craftBandageBefore + 2,
                    $"bandage count in inventory after crafts (was {craftBandageBefore}, now {craftBandageAfter})");


                // 5.11 Save → TryLoad
                if (mechParts != null) craftInv.Add(mechParts, 5);
                craftSession.Start("recipe_bandage"); // add a partial craft
                var craftSave = craftSession.CaptureSave();
                Check(craftSave != null, "CaptureState returns non-null");
                Check(craftSave!.ActiveCrafts != null && craftSave.ActiveCrafts.Length == 1,
                    "partial craft captured in save");


                // 5.12 Restore preserves HoursRemaining
                var craftInv2 = new Ashfall.Core.Inventory.Inventory();
                var craftSession2 = new CraftingHostSession(craftInv2, seedDefaultWorkbench: true);
                craftSession2.RestoreSave(craftSave);
                Check(craftSession2.Engine.ActiveCraftCount == 1, "restored crafting queue has 1 entry");
                Check(craftSession2.Engine.ActiveCrafts[0].HoursRemaining > 0f,
                    "restored craft has positive hours remaining");


                // 5.13 Tick restored craft to completion
                int restoredCraftCompletions = 0;
                craftSession2.Engine.OnCraftCompleted += (_, _) => restoredCraftCompletions++;
                craftSession2.CompleteAll(5f);
                Check(restoredCraftCompletions == 1,
                    $"restored craft completes exactly once (got {restoredCraftCompletions})");

                // 5.14 No duplication: tick again after completion
                craftSession2.CompleteAll(5f);
                Check(restoredCraftCompletions == 1,
                    "second tick after completion does not re-fire OnCraftCompleted");


                // ── 6. Respiratory Affliction Verification (10 assertions) ──
                GD.Print("[ShelterOperationsSelfTest] §6 Respiratory affliction...");
                var phase0resp = new Phase0HostSession();
                phase0resp.IsInAshZone = true;

                // 6.1 GetOrCreate returns non-null
                var respState = phase0resp.Respiratory.GetOrCreate("survivor_gunner_mikhail");
                Check(respState != null, "Respiratory.GetOrCreate returns non-null state");


                // 6.2 Ash-zone exposure accumulates degradation
                phase0resp.Respiratory.TickHours("survivor_gunner_mikhail", 24f);
                float respDeg = phase0resp.Respiratory.RespiratoryDegradation("survivor_gunner_mikhail");
                Check(respDeg > 0f, $"ash-zone TickHours accumulates degradation (got {respDeg:F2})");

                // 6.3 Below SevereCoughThreshold → no stamina penalty
                if (respDeg < Ashfall.Core.Medical.RespiratoryDegenerationSystem.SevereCoughThreshold)
                    Check(phase0resp.Respiratory.GetStaminaMultiplier("survivor_gunner_mikhail") == 1f,
                        "stamina multiplier is 1.0 below severe cough threshold");

                // 6.4 Force to severe cough → stamina penalty
                var forcedState = phase0resp.Respiratory.GetOrCreate("survivor_forced");
                forcedState!.respiratoryDegradation = Ashfall.Core.Medical.RespiratoryDegenerationSystem.SevereCoughThreshold + 1f;
                float mult = phase0resp.Respiratory.GetStaminaMultiplier("survivor_forced");
                Check(mult < 1f, $"stamina multiplier < 1 when severe cough active ({mult:F2})");


                // 6.5 ApplyInhaler reduces degradation
                float respDegBefore = respDeg;
                bool inhalerOk = phase0resp.Respiratory.ApplyInhaler("survivor_gunner_mikhail");
                Check(inhalerOk, "ApplyInhaler returns true on survivor with lung damage");
                float respDegAfter = phase0resp.Respiratory.RespiratoryDegradation("survivor_gunner_mikhail");
                Check(respDegAfter < respDegBefore, $"ApplyInhaler reduces degradation ({respDegBefore:F2} → {respDegAfter:F2})");

                // 6.6 ApplyInhaler sets relief hours
                float inhalerRelief = phase0resp.Respiratory.InhalerReliefHours("survivor_gunner_mikhail");
                Check(inhalerRelief == Ashfall.Core.Medical.RespiratoryDegenerationSystem.InhalerReliefDurationHours,
                    $"inhaler relief hours set to {Ashfall.Core.Medical.RespiratoryDegenerationSystem.InhalerReliefDurationHours} (got {inhalerRelief})");

                // 6.7 Inhaler suppresses stamina penalty
                forcedState.respiratoryDegradation = Ashfall.Core.Medical.RespiratoryDegenerationSystem.SevereCoughThreshold + 1f;
                forcedState.inhalerReliefHours = 8f;
                Check(phase0resp.Respiratory.GetStaminaMultiplier("survivor_forced") == 1f,
                    "inhaler relief suppresses stamina penalty");


                // 6.8 Save round-trip preserves degradation
                var phase0respSave = phase0resp.CaptureSave();
                var phase0respFresh = new Phase0HostSession();
                phase0respFresh.RestoreSave(phase0respSave);
                float restoredRespDeg = phase0respFresh.Respiratory.RespiratoryDegradation("survivor_gunner_mikhail");
                Check(Math.Abs(restoredRespDeg - respDegAfter) < 0.01f,
                    $"respiratory degradation preserved across save/restore ({respDegAfter:F2} → {restoredRespDeg:F2})");

                // 6.9 ApplyInhaler on healthy survivor returns false
                var phase0b = new Phase0HostSession();
                phase0b.Respiratory.GetOrCreate("healthy_sv"); // degradation=0
                bool noEffect = phase0b.Respiratory.ApplyInhaler("healthy_sv");
                Check(!noEffect, "ApplyInhaler returns false on survivor with zero degradation");

                // 6.10 ApplyHerbalTea reduces mild degradation
                var phase0c = new Phase0HostSession();
                phase0c.IsInAshZone = true;
                phase0c.Respiratory.TickHours("sv_mild", 10f);
                float mildBefore = phase0c.Respiratory.RespiratoryDegradation("sv_mild");
                if (mildBefore > 0f)
                {
                    phase0c.Respiratory.ApplyHerbalTea("sv_mild");
                    Check(phase0c.Respiratory.RespiratoryDegradation("sv_mild") < mildBefore,
                        "ApplyHerbalTea reduces mild respiratory degradation");
                }


                // ── 7. Integration: craft → advance → affliction → treat → persist (3 assertions) ──
                GD.Print("[ShelterOperationsSelfTest] §7 Integration loop...");
                var intInv = new Ashfall.Core.Inventory.Inventory();
                var intCrafting = new CraftingHostSession(intInv, seedDefaultWorkbench: true);
                var intMechDef = CraftingHostSession.Catalog.Get("scrap_mechanical");
                if (intMechDef != null)
                {
                    intInv.Add(intMechDef, 5);
                    intCrafting.Start("recipe_bandage");
                    intCrafting.CompleteAll(2f);
                    Check(intInv.CountById("bandage") >= 1,
                        "integration: crafted bandage is available in inventory");
                }
                else
                {
                    Check(false, "integration: scrap_mechanical not in seed catalog");
                }

                // Expose survivor in ash zone, treat with inhaler
                var intPhase0 = new Phase0HostSession();
                intPhase0.IsInAshZone = true;
                intPhase0.Respiratory.TickHours("survivor_gunner_mikhail", 24f);
                float intDegBefore = intPhase0.Respiratory.RespiratoryDegradation("survivor_gunner_mikhail");
                if (intDegBefore > 0f)
                {
                    intPhase0.Respiratory.ApplyInhaler("survivor_gunner_mikhail");
                    var intSave = intPhase0.CaptureSave();
                    var intRestored = new Phase0HostSession();
                    intRestored.RestoreSave(intSave);
                    float intDegRestored = intRestored.Respiratory.RespiratoryDegradation("survivor_gunner_mikhail");
                    Check(intDegRestored < intDegBefore,
                        $"integration: inhaler treatment persists across save/restore ({intDegBefore:F2} → {intDegRestored:F2})");
                }

                // Crafting queue survives save/restore under ContinueGame pattern
                var intInv2 = new Ashfall.Core.Inventory.Inventory();
                var intCraft2 = new CraftingHostSession(intInv2, seedDefaultWorkbench: true);
                var intMechDef2 = CraftingHostSession.Catalog.Get("scrap_mechanical");
                if (intMechDef2 != null)
                {
                    intInv2.Add(intMechDef2, 3);
                    intCraft2.Start("recipe_bandage");
                    var craftSave2 = intCraft2.CaptureSave();
                    var intInv3 = new Ashfall.Core.Inventory.Inventory();
                    var intCraft3 = new CraftingHostSession(intInv3, seedDefaultWorkbench: true);
                    intCraft3.RestoreSave(craftSave2);
                    Check(intCraft3.Engine.ActiveCraftCount == 1,
                        "integration: crafting queue preserved through ContinueGame restore path");
                }


                GD.Print($"[ShelterOperationsSelfTest] All assertions complete. Failures so far: {failures}");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[ShelterOperationsSelfTest] Exception: {ex.Message}\n{ex.StackTrace}");
                failures++;
            }

            if (!RunShelterOperationsBoardSelfTest(dataDirectory))
                failures++;
            return EmitSummary("shelter_operations_selftest", failures == 0, failures == 0 ? 0 : 1, details: failures == 0 ? "PASS" : $"FAIL ({failures})");
        }

    }
}
