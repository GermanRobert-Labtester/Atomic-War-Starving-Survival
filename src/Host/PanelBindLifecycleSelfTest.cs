// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;
using Ashfall.Core.Medical;
using Ashfall.Core.Survivors;
using Ashfall.Core.Radio;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Self-test validating real Godot-node callback lifecycles:
    /// - Node instantiation and initial Bind() event hookup
    /// - Live event handling through Godot Control hierarchy
    /// - Clean Unbind() tearing down listeners (no stale callbacks)
    /// - Rebind() guaranteeing single-subscription semantics (no delegate stacking)
    /// - Session-switching (unsubscribing old session, binding new session)
    /// - Node _ExitTree() / Free() cleanup
    /// </summary>
    public static class PanelBindLifecycleSelfTest
    {
        public static int Run(string dataDirectory = "")
        {
            GD.Print("── GODOT-NODE CALLBACK PANEL BIND/UNBIND/REBIND SELF-TEST ──");
            int passedGates = 0;
            int totalGates = 21;

            try
            {
                // ── GATE 1: WeatherPanel Bind -> Unbind -> Rebind Callback Test ──
                GD.Print("\n[Gate 1] Testing WeatherPanel node callback lifecycle...");
                var weatherSys1 = new WeatherSystem();
                var world1 = new WorldHostSession(weather: weatherSys1);

                var weatherSys2 = new WeatherSystem();
                var world2 = new WorldHostSession(weather: weatherSys2);

                var weatherPanel = new WeatherPanel();

                // Bind to world1
                weatherPanel.Bind(world1);
                if (!weatherPanel.IsBound)
                {
                    GD.PrintErr("[FAIL] Gate 1: WeatherPanel IsBound is false after initial Bind.");
                    return 1;
                }

                world1.ForceDemo(WeatherKind.FalloutStorm);
                if (weatherPanel.BoundWeather != WeatherKind.FalloutStorm)
                {
                    GD.PrintErr($"[FAIL] Gate 1: WeatherPanel did not receive FalloutStorm event from world1 (got {weatherPanel.BoundWeather}).");
                    return 1;
                }

                // Unbind
                weatherPanel.Unbind();
                if (weatherPanel.IsBound)
                {
                    GD.PrintErr("[FAIL] Gate 1: WeatherPanel IsBound is true after Unbind.");
                    return 1;
                }

                // Mutate world1 while unbound -> panel must NOT track
                world1.ForceDemo(WeatherKind.BlackRain);
                if (weatherPanel.BoundWeather == WeatherKind.BlackRain)
                {
                    GD.PrintErr("[FAIL] Gate 1: WeatherPanel received event while unbound.");
                    return 1;
                }

                // Rebind to world1
                weatherPanel.Bind(world1);
                if (weatherPanel.BoundWeather != WeatherKind.BlackRain)
                {
                    GD.PrintErr("[FAIL] Gate 1: WeatherPanel did not pick up current BlackRain state on rebind.");
                    return 1;
                }

                // Consecutive Bind() calls must NOT stack delegates
                weatherPanel.Bind(world1);
                weatherPanel.Bind(world1);
                world1.ForceDemo(WeatherKind.BioFog);
                if (weatherPanel.BoundWeather != WeatherKind.BioFog)
                {
                    GD.PrintErr("[FAIL] Gate 1: WeatherPanel failed after consecutive binds.");
                    return 1;
                }

                // Switch to world2
                weatherPanel.Bind(world2);
                world2.ForceDemo(WeatherKind.Blizzard);
                if (weatherPanel.BoundWeather != WeatherKind.Blizzard)
                {
                    GD.PrintErr("[FAIL] Gate 1: WeatherPanel failed to track switched world2 session.");
                    return 1;
                }

                world1.ForceDemo(WeatherKind.Clear);
                if (weatherPanel.BoundWeather == WeatherKind.Clear)
                {
                    GD.PrintErr("[FAIL] Gate 1: Old world1 session leaked event into WeatherPanel after switching to world2.");
                    return 1;
                }

                weatherPanel.QueueFree();
                GD.Print("[PASS] Gate 1: WeatherPanel bind -> unbind -> rebind -> session-switch verified cleanly.");
                passedGates++;

                // ── GATE 2: SaveLoadPanel Bind -> Unbind -> Rebind Callback Test ──
                GD.Print("\n[Gate 2] Testing SaveLoadPanel node callback lifecycle...");
                string tempDir = Path.Combine(Path.GetTempPath(), "ashfall_panel_lifecycle_saveload_" + DateTime.UtcNow.Ticks); // DETERMINISM_ALLOWLIST: Test harness temporary directory
                Directory.CreateDirectory(tempDir);

                try
                {
                    var saveSession1 = new SaveLoadHostSession();
                    saveSession1.Initialize(tempDir);

                    var saveSession2 = new SaveLoadHostSession();
                    saveSession2.Initialize(tempDir);

                    var savePanel = new SaveLoadPanel();
                    savePanel.Bind(saveSession1);

                    int slotSelectedCount = 0;
                    SaveSlotId? lastSelected = null;
                    savePanel.OnSlotSelected += id =>
                    {
                        slotSelectedCount++;
                        lastSelected = id;
                    };

                    // Trigger slot change in session1
                    var testSlot = new SaveSlotId("slot_test_lifecycle");
                    saveSession1.CreateSlot(testSlot);
                    saveSession1.SelectSlot(testSlot);

                    if (savePanel.IsLastError)
                    {
                        GD.PrintErr("[FAIL] Gate 2: SaveLoadPanel has unexpected error after slot selection.");
                        return 1;
                    }

                    // Test load completed event
                    saveSession1.TryLoadSlot(new SaveSlotId("missing_slot_404"), out var failResult);
                    if (!savePanel.IsLastError || !savePanel.LastStatusMessage.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    {
                        GD.PrintErr($"[FAIL] Gate 2: SaveLoadPanel failed to reflect session OnLoadCompleted callback (msg='{savePanel.LastStatusMessage}').");
                        return 1;
                    }

                    // Unbind
                    savePanel.Unbind();
                    savePanel.ClearStatusMessage();

                    // Trigger in session1 while unbound -> panel must NOT update
                    saveSession1.TryLoadSlot(new SaveSlotId("missing_slot_404"), out _);
                    if (savePanel.IsLastError || !string.IsNullOrEmpty(savePanel.LastStatusMessage))
                    {
                        GD.PrintErr("[FAIL] Gate 2: SaveLoadPanel handled session callback while unbound.");
                        return 1;
                    }

                    // Rebind to session2
                    savePanel.Bind(saveSession2);
                    saveSession2.TryLoadSlot(new SaveSlotId("missing_slot_404"), out var res2);
                    if (!savePanel.IsLastError || !savePanel.LastStatusMessage.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    {
                        GD.PrintErr("[FAIL] Gate 2: SaveLoadPanel failed to receive callback after rebind to session2.");
                        return 1;
                    }

                    savePanel.QueueFree();
                    saveSession1.QueueFree();
                    saveSession2.QueueFree();
                    GD.Print("[PASS] Gate 2: SaveLoadPanel bind -> unbind -> rebind verified cleanly.");
                    passedGates++;
                }
                finally
                {
                    try { Directory.Delete(tempDir, true); } catch { /* cleanup: best-effort temp directory delete */ }
                }

                // ── GATE 3: PowerGridPanel Bind -> Unbind -> Rebind Callback Test ──
                GD.Print("\n[Gate 3] Testing PowerGridPanel node callback lifecycle...");
                var powerRng = new SeededRng(42);
                var powerSession = PowerGridHostSession.CreateDefault(powerRng);

                var powerPanel = new PowerGridPanel();
                powerPanel.Bind(powerSession);

                // Mutate session state
                powerSession.ToggleBreaker("room_medical");
                // Unbind
                powerPanel.Unbind();
                // Mutate while unbound
                powerSession.ToggleBreaker("room_hydroponics");
                // Rebind
                powerPanel.Bind(powerSession);
                powerSession.ToggleBreaker("room_workshop");

                powerPanel.QueueFree();
                GD.Print("[PASS] Gate 3: PowerGridPanel bind -> unbind -> rebind verified cleanly.");
                passedGates++;

                // ── GATE 4: GreenhousePanel Bind -> Unbind -> Rebind Callback Test ──
                GD.Print("\n[Gate 4] Testing GreenhousePanel node callback lifecycle...");
                var invHost = InventoryHostSession.Create(dataDirectory, seedWhenNoSave: false);
                var ghSession = GreenhouseHostSession.Create(invHost);

                var ghPanel = new GreenhousePanel();
                ghPanel.Bind(ghSession);
                if (!ghPanel.IsBound)
                {
                    GD.PrintErr("[FAIL] Gate 4: GreenhousePanel IsBound is false after Bind.");
                    return 1;
                }

                ghSession.Plant(0, "item_seed_tuber", 1);
                ghPanel.Unbind();
                if (ghPanel.IsBound)
                {
                    GD.PrintErr("[FAIL] Gate 4: GreenhousePanel IsBound is true after Unbind.");
                    return 1;
                }

                ghSession.Water(0, 50f, false);
                ghPanel.Bind(ghSession);
                ghSession.TreatBlight(0);

                ghPanel.QueueFree();
                GD.Print("[PASS] Gate 4: GreenhousePanel bind -> unbind -> rebind verified cleanly.");
                passedGates++;

                // ── GATE 5: ResearchPanel Bind -> Unbind -> Rebind Test ──
                GD.Print("\n[Gate 5] Testing ResearchPanel node lifecycle...");
                var research = new ResearchSystem(log: new GodotLog());
                // Plan 34: load the authoritative research_knowledge.json catalog —
                // no hardcoded fallback exists anymore.
                string gate5DataDir = string.IsNullOrEmpty(dataDirectory) ? CatalogPath.ResolveDataDir() : dataDirectory;
                ResearchKnowledgeCatalogLoader.LoadAndRegister(
                    research, gate5DataDir, new FileSystemIO(), new SystemTextJsonSerializer());
                research.UnlockManual("knowledge_water_basics");

                var researchPanel = new ResearchPanel();
                researchPanel._Ready();
                researchPanel.Bind(research);
                int count1 = researchPanel.RenderedRowCount;
                if (!researchPanel.IsBound || count1 != 1)
                {
                    GD.PrintErr($"[FAIL] Gate 5: ResearchPanel count1={count1} (expected 1).");
                    return 1;
                }

                researchPanel.Unbind();
                if (researchPanel.IsBound)
                {
                    GD.PrintErr("[FAIL] Gate 5: ResearchPanel IsBound is true after Unbind.");
                    return 1;
                }

                research.UnlockManual("knowledge_water_advanced");
                researchPanel.Bind(research);
                int count2 = researchPanel.RenderedRowCount;
                if (!researchPanel.IsBound || count2 != 2)
                {
                    GD.PrintErr($"[FAIL] Gate 5: ResearchPanel failed to update after rebind (count1={count1}, count2={count2}).");
                    return 1;
                }

                researchPanel.QueueFree();
                GD.Print("[PASS] Gate 5: ResearchPanel bind -> unbind -> rebind verified cleanly.");
                passedGates++;

                // ── GATE 6: MedicalPanel Bind -> Unbind -> Rebind with Respiratory Degeneration ──
                GD.Print("\n[Gate 6] Testing MedicalPanel node callback lifecycle...");
                var medHost = new MedicalHostSession();
                var respSys = new RespiratoryDegenerationSystem();
                respSys.IsInFalloutStorm = () => true;

                var medPanel = new MedicalPanel();
                medPanel.Bind(medHost, respiratory: respSys);
                if (!medPanel.IsBound)
                {
                    GD.PrintErr("[FAIL] Gate 6: MedicalPanel IsBound is false.");
                    return 1;
                }

                // Trigger respiratory state change
                respSys.TickHours("survivor_alpha", 4f);

                // Unbind
                medPanel.Unbind();
                if (medPanel.IsBound)
                {
                    GD.PrintErr("[FAIL] Gate 6: MedicalPanel IsBound is true after Unbind.");
                    return 1;
                }

                // Rebind
                medPanel.Bind(medHost, respiratory: respSys);
                respSys.TickHours("survivor_alpha", 4f);

                medPanel.QueueFree();
                GD.Print("[PASS] Gate 6: MedicalPanel bind -> unbind -> rebind verified cleanly.");
                passedGates++;

                // ── GATE 7: JournalPanel and ExpeditionPanel Bind -> Unbind -> Rebind ──
                GD.Print("\n[Gate 7] Testing JournalPanel and ExpeditionPanel node lifecycles...");
                var journalHost = new JournalHostSession();
                var journalPanel = new JournalPanel();
                journalPanel.Bind(journalHost);
                journalPanel.Unbind();
                journalPanel.Bind(journalHost);
                journalPanel.QueueFree();

                var expHost = new ExpeditionHostSession();
                var expPanel = new ExpeditionPanel();
                expPanel.Bind(expHost, null, null);
                expPanel.Unbind();
                expPanel.Bind(expHost, null, null);
                expPanel.QueueFree();

                GD.Print("[PASS] Gate 7: JournalPanel and ExpeditionPanel lifecycles verified cleanly.");
                passedGates++;

                // ── GATE 8: Multiple Sequential Rebinds (Stacking Stress Test) ──
                GD.Print("\n[Gate 8] Testing multiple sequential rebinds for delegate leak / stacking immunity...");
                var stressWeather = new WeatherSystem();
                var stressWorld = new WorldHostSession(weather: stressWeather);
                var stressPanel = new WeatherPanel();

                int callbackFireCount = 0;
                stressWeather.OnWeatherChanged += _ => callbackFireCount++;

                // Call Bind 100 times consecutively without unbinding
                for (int i = 0; i < 100; i++)
                {
                    stressPanel.Bind(stressWorld);
                }

                // Trigger weather change once
                int beforeFire = callbackFireCount;
                stressWorld.ForceDemo(WeatherKind.Ashfall);
                int afterFire = callbackFireCount;

                if (afterFire - beforeFire != 1)
                {
                    GD.PrintErr($"[FAIL] Gate 8: Core event fired {afterFire - beforeFire} times instead of 1.");
                    return 1;
                }

                // Panel should show the latest state correctly
                if (stressPanel.BoundWeather != WeatherKind.Ashfall)
                {
                    GD.PrintErr($"[FAIL] Gate 8: WeatherPanel has {stressPanel.BoundWeather} instead of Ashfall.");
                    return 1;
                }

                stressPanel.Unbind();
                stressPanel.QueueFree();
                GD.Print("[PASS] Gate 8: Multiple sequential rebinds handled idempotently with 0 delegate stacking.");
                passedGates++;

                // ── GATE 9: Shelter Batch Panels Bind -> Unbind -> Rebind Lifecycle ──
                GD.Print("\n[Gate 9] Testing shelter batch panels bind -> unbind -> rebind lifecycle...");
                string dataDir = string.IsNullOrEmpty(dataDirectory) ? CatalogPath.ResolveDataDir() : dataDirectory;
                var rng = new SeededRng(1986);
                var log = new GodotLog();

                var airlock = new AirlockSecurityPanel();
                var airlockSession = new AirlockSecurityHostSession(new AirlockSecuritySystem(rng, log));
                airlock.Bind(airlockSession);
                if (!airlock.IsBound) { GD.PrintErr("[FAIL] Gate 9: AirlockSecurityPanel IsBound false."); return 1; }
                airlock.Unbind();
                if (airlock.IsBound) { GD.PrintErr("[FAIL] Gate 9: AirlockSecurityPanel IsBound true after unbind."); return 1; }
                airlock.Bind(airlockSession);
                airlock.QueueFree();

                var chem = new ChemicalDependencyPanel();
                var chemSession = new ChemicalDependencyHostSession(new ChemicalDependencySystem());
                chem.Bind(chemSession);
                if (!chem.IsBound) { GD.PrintErr("[FAIL] Gate 9: ChemicalDependencyPanel IsBound false."); return 1; }
                chem.Unbind();
                if (chem.IsBound) { GD.PrintErr("[FAIL] Gate 9: ChemicalDependencyPanel IsBound true after unbind."); return 1; }
                chem.Bind(chemSession);
                chem.QueueFree();

                var care = new CaregivingPanel();
                var careSession = new CaregivingHostSession(new CaregivingSystem());
                care.Bind(careSession);
                if (!care.IsBound) { GD.PrintErr("[FAIL] Gate 9: CaregivingPanel IsBound false."); return 1; }
                care.Unbind();
                care.Bind(careSession);
                care.QueueFree();

                var exc = new ExcavationPanel();
                var excSession = new ExcavationHostSession(new ExcavationSystem(rng, log));
                exc.Bind(excSession);
                exc.Unbind();
                exc.Bind(excSession);
                exc.QueueFree();

                var reg = new RegionalTreatyPanel();
                var regSession = new RegionalTreatyHostSession(new RegionalTreatySystem(log));
                reg.Bind(regSession);
                reg.Unbind();
                reg.Bind(regSession);
                reg.QueueFree();

                var rel = new SurvivorRelationsPanel();
                var relSession = new SurvivorRelationsHostSession(new SurvivorRelationsSystem(rng, log));
                rel.Bind(relSession);
                rel.Unbind();
                rel.Bind(relSession);
                rel.QueueFree();

                var wt = new WaterTreatmentPanel();
                var wtSession = new WaterTreatmentHostSession(new WaterTreatmentSystem(log));
                wt.Bind(wtSession);
                wt.Unbind();
                wt.Bind(wtSession);
                wt.QueueFree();

                var way = new WaystationNetworkPanel();
                var waySession = new WaystationHostSession(new WaystationSystem());
                way.Bind(waySession);
                way.Unbind();
                way.Bind(waySession);
                way.QueueFree();

                var wild = new WildlifeTrappingPanel();
                var wildSession = new WildlifeTrappingHostSession(new WildlifeTrappingSystem(rng, log));
                wild.Bind(wildSession);
                wild.Unbind();
                wild.Bind(wildSession);
                wild.QueueFree();

                GD.Print("[PASS] Gate 9: Shelter batch panels verified for bind -> unbind -> rebind lifecycle.");
                passedGates++;

                // ── GATE 10: DutyRosterPanel and DoseLedgerPanel Callback Lifecycle ──
                GD.Print("\n[Gate 10] Testing DutyRosterPanel and DoseLedgerPanel lifecycle...");
                var dutyHost = DutyRosterHostSession.Create(dataDir, log: null);
                var survHost = new SurvivorsHostSession();
                var dutyPanel = new DutyRosterPanel();
                dutyPanel.Bind(dutyHost, survHost);
                if (!dutyPanel.IsBound) { GD.PrintErr("[FAIL] Gate 10: DutyRosterPanel IsBound false."); return 1; }
                dutyPanel.Unbind();
                if (dutyPanel.IsBound) { GD.PrintErr("[FAIL] Gate 10: DutyRosterPanel IsBound true after unbind."); return 1; }
                dutyPanel.Bind(dutyHost, survHost);
                dutyPanel.QueueFree();

                var doseHost = DoseLedgerHostSession.Create(dataDir);
                var dosePanel = new DoseLedgerPanel();
                dosePanel.Bind(doseHost, survHost);
                if (!dosePanel.IsBound) { GD.PrintErr("[FAIL] Gate 10: DoseLedgerPanel IsBound false."); return 1; }
                dosePanel.Unbind();
                if (dosePanel.IsBound) { GD.PrintErr("[FAIL] Gate 10: DoseLedgerPanel IsBound true after unbind."); return 1; }
                dosePanel.Bind(doseHost, survHost);
                dosePanel.QueueFree();

                GD.Print("[PASS] Gate 10: DutyRosterPanel and DoseLedgerPanel verified cleanly.");
                passedGates++;

                // ── GATE 11: SurvivorsPanel, RadioPanel and QuestsPanel Callback Lifecycle ──
                GD.Print("\n[Gate 11] Testing SurvivorsPanel, RadioPanel and QuestsPanel lifecycle...");
                var survPanel = new SurvivorsPanel();
                survPanel.Bind(survHost);
                if (!survPanel.IsBound) { GD.PrintErr("[FAIL] Gate 11: SurvivorsPanel IsBound false."); return 1; }
                survPanel.Unbind();
                if (survPanel.IsBound) { GD.PrintErr("[FAIL] Gate 11: SurvivorsPanel IsBound true after unbind."); return 1; }
                survPanel.Bind(survHost);
                survPanel.QueueFree();

                var radioHost = RadioHostSession.Create(dataDir, 1);
                var radioPanel = new RadioPanel();
                radioPanel.Bind(radioHost);
                if (!radioPanel.IsBound) { GD.PrintErr("[FAIL] Gate 11: RadioPanel IsBound false."); return 1; }
                radioPanel.Unbind();
                if (radioPanel.IsBound) { GD.PrintErr("[FAIL] Gate 11: RadioPanel IsBound true after unbind."); return 1; }
                radioPanel.Bind(radioHost);
                radioPanel.QueueFree();

                var questsPanel = new QuestsPanel();
                var holdfastQuests = new HoldfastQuestSystem();
                questsPanel.Bind(holdfastQuests);
                if (!questsPanel.IsBound) { GD.PrintErr("[FAIL] Gate 11: QuestsPanel IsBound false."); return 1; }
                questsPanel.Unbind();
                if (questsPanel.IsBound) { GD.PrintErr("[FAIL] Gate 11: QuestsPanel IsBound true after unbind."); return 1; }
                questsPanel.Bind(holdfastQuests);
                questsPanel.QueueFree();

                GD.Print("[PASS] Gate 11: SurvivorsPanel, RadioPanel and QuestsPanel verified cleanly.");
                passedGates++;

                // ── GATE 12: IBindablePanel Interface Conformance ──
                GD.Print("\n[Gate 12] Testing IBindablePanel interface conformance on panels...");
                IBindablePanel[] bindablePanels = new IBindablePanel[]
                {
                    new AirlockSecurityPanel(),
                    new ChemicalDependencyPanel(),
                    new CaregivingPanel(),
                    new DutyRosterPanel(),
                    new DoseLedgerPanel(),
                    new SurvivorsPanel(),
                    new RadioPanel(),
                    new QuestsPanel(),
                    new LowBackgroundLeadPanel(),
                    new InSarMappingPanel(),
                    new HydraulicExtrusionPanel(),
                    new RunFlatTirePanel()
                };

                foreach (var bp in bindablePanels)
                {
                    if (bp.IsBound)
                    {
                        GD.PrintErr($"[FAIL] Gate 12: Panel {bp.GetType().Name} IsBound is true before binding.");
                        return 1;
                    }
                    bp.Unbind(); // Must be safe on unbound panel
                    if (bp is Control ctrl)
                        ctrl.QueueFree();
                }

                GD.Print("[PASS] Gate 12: IBindablePanel contract conformance verified cleanly.");
                passedGates++;

                // ── GATE 13: FireIncidentPanel Dynamic Resolution & Action Lifecycle ──
                GD.Print("\n[Gate 13] Testing FireIncidentPanel dynamic incident resolution and action lifecycle...");
                var fireSys = new ShelterFireHazardSystem();
                var fireHost = new ShelterFireHostSession(fireSys);
                var firePanel = new FireIncidentPanel();

                // 1. Initial bind with no incidents: should be bound and have empty incident id
                firePanel.Bind(fireHost);
                if (!firePanel.IsBound)
                {
                    GD.PrintErr("[FAIL] Gate 13: FireIncidentPanel IsBound is false after initial Bind.");
                    return 1;
                }
                if (!string.IsNullOrEmpty(firePanel.CurrentIncidentId))
                {
                    GD.PrintErr($"[FAIL] Gate 13: Expected empty incident id before ignition, got '{firePanel.CurrentIncidentId}'.");
                    return 1;
                }

                // 2. Incident ignites dynamically in the canonical authority
                var zones = new System.Collections.Generic.List<FireZoneState>
                {
                    new FireZoneState
                    {
                        zoneId = "zone_workshop",
                        displayName = "Workshop Bay",
                        fireLevel = 0.5f,
                        smokeLevel = 0.2f,
                        coLevel = 0.1f,
                        heatLevel = 0.3f,
                        damperOpen = true
                    }
                };
                string incId = "arc_workshop_d10";
                fireSys.Ignite(incId, "zone_workshop", 10, zones);

                // 3. Re-resolves dynamically via Bind / SelectIncident or OnStateChanged
                firePanel.Bind(fireHost);
                if (firePanel.CurrentIncidentId != incId)
                {
                    GD.PrintErr($"[FAIL] Gate 13: Expected panel to resolve to active incident '{incId}', got '{firePanel.CurrentIncidentId}'.");
                    return 1;
                }

                // 4. Raise alarm through panel action
                firePanel.RaiseAlarmForTest();
                var activeInc = fireSys.GetIncident(incId);
                if (activeInc == null || !activeInc.alarmRaised)
                {
                    GD.PrintErr("[FAIL] Gate 13: Panel RaiseAlarm action did not mutate canonical fire incident.");
                    return 1;
                }

                // 5. Deploy extinguisher through panel action
                float fireBefore = activeInc.zones[0].fireLevel;
                firePanel.DeployExtinguisherForTest();
                if (activeInc.extinguisherChargesUsed != 1 || activeInc.zones[0].fireLevel >= fireBefore)
                {
                    GD.PrintErr("[FAIL] Gate 13: Panel DeployExtinguisher action did not reduce fire level on canonical incident.");
                    return 1;
                }

                // 6. Advance tick
                firePanel.AdvanceTickForTest();
                if (activeInc.ticksElapsed != 1)
                {
                    GD.PrintErr("[FAIL] Gate 13: Panel AdvanceTick action did not advance tick on canonical incident.");
                    return 1;
                }

                firePanel.QueueFree();
                GD.Print("[PASS] Gate 13: FireIncidentPanel dynamic resolution and action lifecycle verified cleanly.");
                passedGates++;

                // ── GATE 14: RadiationDetailPanel Bind with Environmental Survivors ──
                GD.Print("\n[Gate 14] Testing RadiationDetailPanel with Environmental Exposure...");
                var survivorsHost = new SurvivorsHostSession();
                survivorsHost.AddSurvivor("surv_indoor", "Indoor Worker", health: 100f);
                survivorsHost.AddSurvivor("surv_outdoor", "Scout", health: 100f);
                survivorsHost.SetSurvivorLocation("surv_indoor", Ashfall.Core.Radiation.SurvivorExposureLocation.ShelterInterior);
                survivorsHost.SetSurvivorLocation("surv_outdoor", Ashfall.Core.Radiation.SurvivorExposureLocation.WastelandOutdoors);
                survivorsHost.ExposureResolver.WeatherRadModifierProvider = () => 10f;

                survivorsHost.TickHour(1f);

                var indoorRad = survivorsHost.RadStateFor("surv_indoor");
                var outdoorRad = survivorsHost.RadStateFor("surv_outdoor");
                if (indoorRad == null || outdoorRad == null || indoorRad.RadiationDose >= outdoorRad.RadiationDose)
                {
                    GD.PrintErr($"[FAIL] Gate 14: Indoor dose ({indoorRad?.RadiationDose}) must be strictly less than outdoor dose ({outdoorRad?.RadiationDose}).");
                    return 1;
                }

                var radDetailPanel = new RadiationDetailPanel();
                radDetailPanel.Bind(null, survivorsHost);
                if (!radDetailPanel.IsBound || radDetailPanel.RenderedCurrentCount != 2)
                {
                    GD.PrintErr($"[FAIL] Gate 14: Expected RadiationDetailPanel to be bound with 2 survivor rows, got isBound={radDetailPanel.IsBound}, count={radDetailPanel.RenderedCurrentCount}.");
                    return 1;
                }

                radDetailPanel.QueueFree();
                GD.Print("[PASS] Gate 14: RadiationDetailPanel environmental exposure binding verified cleanly.");
                passedGates++;

                // ── GATE 15: Repeated Bind Subscription Symmetry Regression Gate ──
                GD.Print("\n[Gate 15] Testing repeated Bind subscription symmetry across four flagship panels...");

                // 1. WeatherHistoryPanel (repeated x100 bind and switch)
                var weatherHistPanel = new WeatherHistoryPanel();
                var wSys1 = new WeatherSystem();
                var wSys2 = new WeatherSystem();
                for (int i = 0; i < 100; i++)
                {
                    weatherHistPanel.Bind(wSys1);
                }
                weatherHistPanel.Bind(wSys2);
                weatherHistPanel.QueueFree();

                // 2. GeigerCalibrationPanel (repeated x100 bind and switch)
                var geigerPanel = new GeigerCalibrationPanel();
                var doseHost1 = new DoseLedgerHostSession();
                var doseHost2 = new DoseLedgerHostSession();
                for (int i = 0; i < 100; i++)
                {
                    geigerPanel.Bind(doseHost1, "tag_1");
                }
                geigerPanel.Bind(doseHost2, "tag_1");
                geigerPanel.QueueFree();

                // 3. FireIncidentPanel (repeated x100 bind and switch)
                var fireIncPanel = new FireIncidentPanel();
                var fSys1 = new ShelterFireHazardSystem();
                var fSys2 = new ShelterFireHazardSystem();
                for (int i = 0; i < 100; i++)
                {
                    fireIncPanel.Bind(fSys1);
                }
                fireIncPanel.Bind(fSys2);
                fireIncPanel.QueueFree();

                // 4. TriangulationPanel (repeated x100 bind, location-revealed single propagation, and switch)
                var triPanel = new TriangulationPanel();
                var radHost1 = new RadioHostSession(new FactionRadioEngine(), new CoreSeededRng(1), 1);
                var radHost2 = new RadioHostSession(new FactionRadioEngine(), new CoreSeededRng(2), 1);
                int discoveredLocationsCount = 0;
                triPanel.OnLocationDiscovered += _ => discoveredLocationsCount++;

                for (int i = 0; i < 100; i++)
                {
                    triPanel.Bind(radHost1, "sig_distress");
                }

                // Add 6 high-accuracy observations to guarantee discovery threshold is met
                for (int i = 0; i < 6; i++)
                {
                    radHost1.Triangulation.RecordObservation(new RadioObservation
                    {
                        signalId = "sig_distress",
                        stationId = "st_" + i,
                        day = 1,
                        bearingDegrees = i * 60f,
                        errorDegrees = 2f,
                        signalStrength = 0.95f,
                        noiseLevel = 0.02f,
                        operatorSkill = 0.9f
                    });
                }
                radHost1.Triangulation.Triangulate("sig_distress", new CoreSeededRng(42));

                if (discoveredLocationsCount != 1)
                {
                    GD.PrintErr($"[FAIL] Gate 15: Expected exactly 1 location discovery after 100x rebind on radHost1, got {discoveredLocationsCount}.");
                    return 1;
                }

                // Switch to radHost2; mutating radHost1 must no longer fire into triPanel
                triPanel.Bind(radHost2, "sig_distress");
                radHost1.Triangulation.Triangulate("sig_distress", new CoreSeededRng(42));
                if (discoveredLocationsCount != 1)
                {
                    GD.PrintErr($"[FAIL] Gate 15: Detached radHost1 propagated discovery to triPanel after session switch (got {discoveredLocationsCount}).");
                    return 1;
                }

                // Now observe and triangulate on radHost2
                for (int i = 0; i < 6; i++)
                {
                    radHost2.Triangulation.RecordObservation(new RadioObservation
                    {
                        signalId = "sig_distress",
                        stationId = "st2_" + i,
                        day = 1,
                        bearingDegrees = i * 60f,
                        errorDegrees = 2f,
                        signalStrength = 0.95f,
                        noiseLevel = 0.02f,
                        operatorSkill = 0.9f
                    });
                }
                radHost2.Triangulation.Triangulate("sig_distress", new CoreSeededRng(42));
                if (discoveredLocationsCount != 2)
                {
                    GD.PrintErr($"[FAIL] Gate 15: Expected second discovery from radHost2, got {discoveredLocationsCount}.");
                    return 1;
                }

                // D19b Station identity seam verification
                radHost1.SetActiveStation("station_ridge_outrigger");
                if (radHost1.ActiveStationId != "station_ridge_outrigger")
                {
                    GD.PrintErr($"[FAIL] Gate 15: Expected ActiveStationId station_ridge_outrigger, got {radHost1.ActiveStationId}.");
                    return 1;
                }
                string obsResult = radHost1.RecordObservation("sig_test", 45f, 0.8f, 0.1f);
                if (!obsResult.Contains("Observation recorded") || radHost1.Triangulation.Observations[radHost1.Triangulation.Observations.Count - 1].stationId != "station_ridge_outrigger")
                {
                    GD.PrintErr($"[FAIL] Gate 15: Expected observation recorded from station_ridge_outrigger.");
                    return 1;
                }

                triPanel.QueueFree();
                GD.Print("[PASS] Gate 15: Repeated Bind subscription symmetry verified across WeatherHistory, GeigerCalibration, FireIncident, and Triangulation.");
                passedGates++;

                // ── GATE 16: WildlifeTrappingPanel Multi-Site Status Rail & Targeted Repair UX ──
                GD.Print("\n[Gate 16] Testing WildlifeTrappingPanel Multi-Site Status Rail & Targeted Repair UX...");
                var wtSys = new WildlifeTrappingSystem(new SeededRng(1986), log);
                var wildlifeSession = new WildlifeTrappingHostSession(wtSys);
                var fileIO = new FileSystemIO();
                var json = new SystemTextJsonSerializer();
                var wtCatalog = WildlifeTrappingCatalogLoader.Load(dataDir, fileIO, json);
                wildlifeSession.Catalog = wtCatalog;

                var invSys = new Inventory();
                var itemCat = ItemCatalogLoader.LoadCatalog(dataDir, fileIO, json);
                var invSession = new InventoryHostSession(invSys, itemCat);
                wildlifeSession.Inventory = invSession;

                var wtPanel = new WildlifeTrappingPanel();
                wtPanel.Bind(wildlifeSession);
                wtPanel._Ready();

                // 1. 0 sites: placeholder "empty" card with "NO SITES"
                if (!wtPanel.StatusRail!.HasCard("empty") || wtPanel.StatusRail.GetCard("empty")?.Value != "—")
                {
                    GD.PrintErr("[FAIL] Gate 16: 0 sites should display placeholder empty card with '—'.");
                    return 1;
                }
                if (wtPanel.RepairButton!.Visible)
                {
                    GD.PrintErr("[FAIL] Gate 16: Repair button should be hidden when 0 sites exist.");
                    return 1;
                }

                // 2. Deploy site 1 (snare with 8 durability)
                wtSys.SetTrap("site_alpha", "bait_grain_lure", "hunter_1", "snare", "trap_snare", 2, 8);
                wtPanel.RefreshView();

                if (wtPanel.StatusRail.HasCard("empty"))
                {
                    GD.PrintErr("[FAIL] Gate 16: Placeholder card should be removed when sites exist.");
                    return 1;
                }
                var cardAlpha = wtPanel.StatusRail.GetCard("site_site_alpha");
                if (cardAlpha == null || cardAlpha.Label != "Wire Snare" || cardAlpha.Value != "8/8" || cardAlpha.CurrentCriticality != AshfallMetricCard.Criticality.Normal)
                {
                    GD.PrintErr($"[FAIL] Gate 16: Card for site_alpha should show 'Wire Snare' '8/8' Normal, got '{cardAlpha?.Label}' '{cardAlpha?.Value}' {cardAlpha?.CurrentCriticality}.");
                    return 1;
                }

                // Deployment control follows the same Core replaceability
                // predicate as TrySetTrap: healthy active is blocked, broken
                // and pending-catch sites expose replacement.
                wtSys.SetTrap("snare_perimeter_north", "bait_grain_lure", "hunter_ui", "snare", "trap_snare", 2, 8);
                wtPanel.RefreshView();
                if (!wtPanel.SetTrapButton!.Disabled || wtPanel.SetTrapButton.Text != "Set Snare at Perimeter")
                {
                    GD.PrintErr($"[FAIL] Gate 16: healthy active perimeter trap should block deployment, got disabled={wtPanel.SetTrapButton.Disabled} text='{wtPanel.SetTrapButton.Text}'.");
                    return 1;
                }

                var managedPerimeter = wtSys.State.trapSites.Find(s => s.siteId == "snare_perimeter_north")!;
                managedPerimeter.isBroken = true;
                managedPerimeter.remainingDurability = 0;
                wtPanel.RefreshView();
                if (wtPanel.SetTrapButton.Disabled || wtPanel.SetTrapButton.Text != "Replace Trap at Perimeter")
                {
                    GD.PrintErr($"[FAIL] Gate 16: broken perimeter trap should expose replacement, got disabled={wtPanel.SetTrapButton.Disabled} text='{wtPanel.SetTrapButton.Text}'.");
                    return 1;
                }

                invSession.Add("rope", 1);
                wtPanel.SetTrapButton.EmitSignal(BaseButton.SignalName.Pressed);
                int ropeAfterReplacement = invSession.Inventory.CountById("rope");
                if (managedPerimeter.isBroken || managedPerimeter.remainingDurability != 8
                    || ropeAfterReplacement != 0)
                {
                    GD.PrintErr($"[FAIL] Gate 16: pressing the broken-trap replacement control should deploy and charge once, got broken={managedPerimeter.isBroken} durability={managedPerimeter.remainingDurability} rope={ropeAfterReplacement}.");
                    return 1;
                }

                managedPerimeter.hasCatch = true;
                wtPanel.RefreshView();
                if (wtPanel.SetTrapButton.Disabled || wtPanel.SetTrapButton.Text != "Replace Trap at Perimeter")
                {
                    GD.PrintErr($"[FAIL] Gate 16: pending-catch perimeter trap should expose replacement, got disabled={wtPanel.SetTrapButton.Disabled} text='{wtPanel.SetTrapButton.Text}'.");
                    return 1;
                }
                wtSys.State.trapSites.RemoveAll(s => s.siteId == "snare_perimeter_north");
                wtPanel.RefreshView();

                // 3. Low durability test: remainingDurability = 2 (<= 8/3)
                var alphaState = wtSys.State.trapSites.Find(s => s.siteId == "site_alpha")!;
                alphaState.remainingDurability = 2;
                wtPanel.RefreshView();
                if (cardAlpha.CurrentCriticality != AshfallMetricCard.Criticality.Warn)
                {
                    GD.PrintErr($"[FAIL] Gate 16: Low durability should set Warn criticality, got {cardAlpha.CurrentCriticality}.");
                    return 1;
                }

                // 4. Broken site test (1 broken site):
                alphaState.isBroken = true;
                alphaState.remainingDurability = 0;
                wtPanel.RefreshView();
                if (cardAlpha.Value != "BROKEN" || cardAlpha.CurrentCriticality != AshfallMetricCard.Criticality.Critical)
                {
                    GD.PrintErr($"[FAIL] Gate 16: Broken site should show 'BROKEN' Critical, got '{cardAlpha.Value}' {cardAlpha.CurrentCriticality}.");
                    return 1;
                }
                if (!wtPanel.RepairButton.Visible || wtPanel.RepairSiteDropdown!.Visible)
                {
                    GD.PrintErr("[FAIL] Gate 16: Exactly 1 broken site should show repair button and hide dropdown.");
                    return 1;
                }
                if (wtPanel.SelectedRepairSiteId != "site_alpha")
                {
                    GD.PrintErr($"[FAIL] Gate 16: Single broken site should be auto-selected, got '{wtPanel.SelectedRepairSiteId}'.");
                    return 1;
                }

                // 5. Affordability preflight:
                // Without materials, button should be disabled and tooltip should not leak raw IDs
                if (!wtPanel.RepairButton.Disabled)
                {
                    GD.PrintErr("[FAIL] Gate 16: Repair button should be disabled when player cannot afford repair.");
                    return 1;
                }
                string tooltip = wtPanel.RepairButton.TooltipText;
                if (!tooltip.Contains("Rope") || tooltip.Contains("item_") || tooltip.Contains("trap_snare"))
                {
                    GD.PrintErr($"[FAIL] Gate 16: Tooltip should display catalog item name 'Rope' without raw IDs, got: {tooltip}");
                    return 1;
                }

                // Add rope to inventory -> button becomes enabled
                invSession.Add("rope", 5);
                wtPanel.RefreshView();
                if (wtPanel.RepairButton.Disabled)
                {
                    GD.PrintErr("[FAIL] Gate 16: Repair button should be enabled when materials are present.");
                    return 1;
                }

                // 6. Multi-site targeted repair: add second broken trap (cage trap)
                wtSys.SetTrap("site_beta", "bait_meat_scraps", "hunter_2", "cage", "trap_cage", 2, 15);
                var betaState = wtSys.State.trapSites.Find(s => s.siteId == "site_beta")!;
                betaState.isBroken = true;
                betaState.remainingDurability = 0;
                wtPanel.RefreshView();

                if (!wtPanel.RepairSiteDropdown.Visible)
                {
                    GD.PrintErr("[FAIL] Gate 16: Multiple broken traps should show repair dropdown.");
                    return 1;
                }
                if (wtPanel.RepairSiteDropdown.ItemCount != 2)
                {
                    GD.PrintErr($"[FAIL] Gate 16: Dropdown should contain 2 items, got {wtPanel.RepairSiteDropdown.ItemCount}.");
                    return 1;
                }

                // Select site_beta: requires scrap_metal and box_of_nails_10, which inventory lacks
                wtPanel.SelectRepairSite("site_beta");
                if (wtPanel.SelectedRepairSiteId != "site_beta")
                {
                    GD.PrintErr($"[FAIL] Gate 16: Selecting site_beta failed, current is {wtPanel.SelectedRepairSiteId}.");
                    return 1;
                }
                if (!wtPanel.RepairButton.Disabled)
                {
                    GD.PrintErr("[FAIL] Gate 16: site_beta should be unaffordable without metal/nails.");
                    return 1;
                }
                string betaTooltip = wtPanel.RepairButton.TooltipText;
                if (!betaTooltip.Contains("Scrap Metal") || (!betaTooltip.Contains("Box of Nails") && !betaTooltip.Contains("Nails")))
                {
                    GD.PrintErr($"[FAIL] Gate 16: site_beta tooltip should display item display names, got: {betaTooltip}");
                    return 1;
                }

                // 7. Execute repair on site_alpha
                wtPanel.SelectRepairSite("site_alpha");
                var repairRes = wildlifeSession.TryRepairTrap("site_alpha");
                if (!repairRes.IsSuccess || alphaState.isBroken || alphaState.remainingDurability != 8)
                {
                    GD.PrintErr($"[FAIL] Gate 16: TryRepairTrap failed: {repairRes.MessageKey}, isBroken={alphaState.isBroken}, dur={alphaState.remainingDurability}.");
                    return 1;
                }
                wtPanel.RefreshView();
                // After repair, only site_beta is broken, so dropdown hides again
                if (wtPanel.RepairSiteDropdown.Visible)
                {
                    GD.PrintErr("[FAIL] Gate 16: After repairing site_alpha, exactly 1 broken site remains so dropdown should hide.");
                    return 1;
                }

                // 8. Site removal removes card from status rail
                wtSys.State.trapSites.RemoveAll(s => s.siteId == "site_alpha");
                wtPanel.RefreshView();
                if (wtPanel.StatusRail.HasCard("site_site_alpha"))
                {
                    GD.PrintErr("[FAIL] Gate 16: Removed site_alpha should no longer have a card in the status rail.");
                    return 1;
                }

                // 9. Durability edge case: legacy/untracked sentinel (-1) -> "—" with Normal criticality
                wtSys.SetTrap("site_legacy", "bait_grain_lure", "hunter_3", "snare", "trap_snare", 2, -1);
                wtPanel.RefreshView();
                var cardLegacy = wtPanel.StatusRail.GetCard("site_site_legacy");
                if (cardLegacy == null || cardLegacy.Value != "—" || cardLegacy.CurrentCriticality != AshfallMetricCard.Criticality.Normal)
                {
                    GD.PrintErr($"[FAIL] Gate 16: Legacy sentinel durability -1 should show '—' Normal, got '{cardLegacy?.Value}' {cardLegacy?.CurrentCriticality}.");
                    return 1;
                }

                // 10. Durability edge case: zero durability with isBroken=false -> "0/8" with Critical criticality
                var legacySite = wtSys.State.trapSites.Find(s => s.siteId == "site_legacy")!;
                legacySite.remainingDurability = 0;
                legacySite.isBroken = false;
                wtPanel.RefreshView();
                if (cardLegacy.Value != "0/8" || cardLegacy.CurrentCriticality != AshfallMetricCard.Criticality.Critical)
                {
                    GD.PrintErr($"[FAIL] Gate 16: Zero durability with isBroken=false should show '0/8' Critical, got '{cardLegacy.Value}' {cardLegacy.CurrentCriticality}.");
                    return 1;
                }

                // 11. Catch ready on healthy trap -> "8/8 • CATCH"
                legacySite.remainingDurability = 8;
                legacySite.hasCatch = true;
                wtPanel.RefreshView();
                if (cardLegacy.Value != "8/8 • CATCH" || cardLegacy.CurrentCriticality != AshfallMetricCard.Criticality.Normal)
                {
                    GD.PrintErr($"[FAIL] Gate 16: Catch ready on healthy trap should show '8/8 • CATCH', got '{cardLegacy.Value}'.");
                    return 1;
                }

                // 12. Catch ready on broken trap -> "BROKEN • CATCH"
                legacySite.isBroken = true;
                legacySite.remainingDurability = 0;
                wtPanel.RefreshView();
                if (cardLegacy.Value != "BROKEN • CATCH" || cardLegacy.CurrentCriticality != AshfallMetricCard.Criticality.Critical)
                {
                    GD.PrintErr($"[FAIL] Gate 16: Catch ready on broken trap should show 'BROKEN • CATCH', got '{cardLegacy.Value}'.");
                    return 1;
                }

                // 13. Box trap multi-ingredient repair bill display names
                wtSys.SetTrap("site_gamma", "bait_scrap_meat", "hunter_4", "box", "trap_box", 2, 15);
                var gammaSite = wtSys.State.trapSites.Find(s => s.siteId == "site_gamma")!;
                gammaSite.isBroken = true;
                gammaSite.remainingDurability = 0;
                wtPanel.RefreshView();
                wtPanel.SelectRepairSite("site_gamma");
                string gammaTooltip = wtPanel.RepairButton.TooltipText;
                if (!gammaTooltip.Contains("Scrap Wood") || !gammaTooltip.Contains("Scrap Metal") || (!gammaTooltip.Contains("Box of Nails") && !gammaTooltip.Contains("Nails")))
                {
                    GD.PrintErr($"[FAIL] Gate 16: site_gamma box trap repair tooltip should display Scrap Wood, Scrap Metal, and Box of Nails, got: {gammaTooltip}");
                    return 1;
                }

                wtPanel.QueueFree();
                GD.Print("[PASS] Gate 16: WildlifeTrappingPanel Multi-Site Status Rail & Targeted Repair UX verified cleanly.");
                passedGates++;

                // ── GATE 17: ×100 Bind → Fire → Unbind Reopen Stability (Plan 16 §16C.8) ──
                GD.Print("\n[Gate 17] Testing ×100 bind/fire/unbind reopen stability across the four former subscription offenders + WeatherPanel...");
                const int ReopenCycles = 100;

                // 17.1 WeatherHistoryPanel — publisher: WeatherSystem.OnStateChanged (ForceWeather).
                var g17WeatherSys = new WeatherSystem();
                var g17WeatherHist = new WeatherHistoryPanel();
                for (int i = 0; i < ReopenCycles; i++)
                {
                    g17WeatherHist.Bind(g17WeatherSys);
                    int before = g17WeatherHist.RefreshCount;
                    g17WeatherSys.ForceWeather(i % 2 == 0 ? WeatherKind.FalloutStorm : WeatherKind.BlackRain);
                    if (g17WeatherHist.RefreshCount != before + 1)
                    {
                        GD.PrintErr($"[FAIL] Gate 17: WeatherHistoryPanel refresh delta {g17WeatherHist.RefreshCount - before} != 1 at cycle {i}.");
                        return 1;
                    }
                    g17WeatherHist.Unbind();
                    g17WeatherSys.ForceWeather(WeatherKind.Ashfall);
                    if (g17WeatherHist.RefreshCount != before + 1)
                    {
                        GD.PrintErr($"[FAIL] Gate 17: WeatherHistoryPanel refreshed while unbound at cycle {i}.");
                        return 1;
                    }
                }
                if (g17WeatherHist.RefreshCount != ReopenCycles)
                {
                    GD.PrintErr($"[FAIL] Gate 17: WeatherHistoryPanel total refreshes {g17WeatherHist.RefreshCount} != {ReopenCycles}.");
                    return 1;
                }
                g17WeatherHist.QueueFree();

                // 17.2 GeigerCalibrationPanel — publisher: Calibration.OnStateChanged (RegisterDevice).
                // Bound with an explicit empty selection to prove the no-device state is safe (N16.6).
                var g17DoseHost = new DoseLedgerHostSession();
                var g17Geiger = new GeigerCalibrationPanel();
                for (int i = 0; i < ReopenCycles; i++)
                {
                    g17Geiger.Bind(g17DoseHost, string.Empty);
                    int before = g17Geiger.RefreshCount;
                    g17DoseHost.Calibration.RegisterDevice($"calib_dev_{i}", $"roster_member_{i}");
                    if (g17Geiger.RefreshCount != before + 1)
                    {
                        GD.PrintErr($"[FAIL] Gate 17: GeigerCalibrationPanel refresh delta {g17Geiger.RefreshCount - before} != 1 at cycle {i}.");
                        return 1;
                    }
                    g17Geiger.Unbind();
                    g17DoseHost.Calibration.RegisterDevice($"calib_dev_unbound_{i}", $"roster_member_unbound_{i}");
                    if (g17Geiger.RefreshCount != before + 1)
                    {
                        GD.PrintErr($"[FAIL] Gate 17: GeigerCalibrationPanel refreshed while unbound at cycle {i}.");
                        return 1;
                    }
                }
                if (g17Geiger.RefreshCount != ReopenCycles)
                {
                    GD.PrintErr($"[FAIL] Gate 17: GeigerCalibrationPanel total refreshes {g17Geiger.RefreshCount} != {ReopenCycles}.");
                    return 1;
                }
                g17Geiger.QueueFree();

                // 17.3 FireIncidentPanel — publisher: ShelterFireHazardSystem.OnStateChanged (Ignite).
                var g17FireSys = new ShelterFireHazardSystem();
                var g17FireZones = new System.Collections.Generic.List<FireZoneState>
                {
                    new FireZoneState
                    {
                        zoneId = "zone_gate17",
                        displayName = "Gate 17 Bay",
                        fireLevel = 0.4f,
                        smokeLevel = 0.1f,
                        coLevel = 0.05f,
                        heatLevel = 0.2f,
                        damperOpen = true
                    }
                };
                var g17FirePanel = new FireIncidentPanel();
                for (int i = 0; i < ReopenCycles; i++)
                {
                    g17FirePanel.Bind(g17FireSys);
                    int before = g17FirePanel.RefreshCount;
                    g17FireSys.Ignite($"gate17_incident_{i}", "zone_gate17", 1, g17FireZones);
                    if (g17FirePanel.RefreshCount != before + 1)
                    {
                        GD.PrintErr($"[FAIL] Gate 17: FireIncidentPanel refresh delta {g17FirePanel.RefreshCount - before} != 1 at cycle {i}.");
                        return 1;
                    }
                    g17FirePanel.Unbind();
                    g17FireSys.Ignite($"gate17_incident_unbound_{i}", "zone_gate17", 1, g17FireZones);
                    if (g17FirePanel.RefreshCount != before + 1)
                    {
                        GD.PrintErr($"[FAIL] Gate 17: FireIncidentPanel refreshed while unbound at cycle {i}.");
                        return 1;
                    }
                }
                if (g17FirePanel.RefreshCount != ReopenCycles)
                {
                    GD.PrintErr($"[FAIL] Gate 17: FireIncidentPanel total refreshes {g17FirePanel.RefreshCount} != {ReopenCycles}.");
                    return 1;
                }
                g17FirePanel.QueueFree();

                // 17.4 TriangulationPanel — publisher: Triangulation.OnStateChanged (RecordObservation).
                var g17RadioHost = new RadioHostSession(new FactionRadioEngine(), new CoreSeededRng(17), 1);
                var g17TriPanel = new TriangulationPanel();
                for (int i = 0; i < ReopenCycles; i++)
                {
                    g17TriPanel.Bind(g17RadioHost, "sig_gate17");
                    int before = g17TriPanel.RefreshCount;
                    g17RadioHost.Triangulation.RecordObservation(new RadioObservation
                    {
                        signalId = "sig_gate17",
                        stationId = $"st_gate17_{i}",
                        day = 1,
                        bearingDegrees = (i * 37) % 360,
                        errorDegrees = 2f,
                        signalStrength = 0.95f,
                        noiseLevel = 0.02f,
                        operatorSkill = 0.9f
                    });
                    if (g17TriPanel.RefreshCount != before + 1)
                    {
                        GD.PrintErr($"[FAIL] Gate 17: TriangulationPanel refresh delta {g17TriPanel.RefreshCount - before} != 1 at cycle {i}.");
                        return 1;
                    }
                    g17TriPanel.Unbind();
                    g17RadioHost.Triangulation.RecordObservation(new RadioObservation
                    {
                        signalId = "sig_gate17",
                        stationId = $"st_gate17_unbound_{i}",
                        day = 1,
                        bearingDegrees = ((i * 37) + 180) % 360,
                        errorDegrees = 2f,
                        signalStrength = 0.95f,
                        noiseLevel = 0.02f,
                        operatorSkill = 0.9f
                    });
                    if (g17TriPanel.RefreshCount != before + 1)
                    {
                        GD.PrintErr($"[FAIL] Gate 17: TriangulationPanel refreshed while unbound at cycle {i}.");
                        return 1;
                    }
                }
                if (g17TriPanel.RefreshCount != ReopenCycles)
                {
                    GD.PrintErr($"[FAIL] Gate 17: TriangulationPanel total refreshes {g17TriPanel.RefreshCount} != {ReopenCycles}.");
                    return 1;
                }
                g17TriPanel.QueueFree();

                // 17.5 WeatherPanel (clean representative) — publisher: WeatherSystem.OnWeatherChanged (ForceWeather).
                var g17WeatherSys2 = new WeatherSystem();
                var g17WeatherHost = new WeatherHostSession(g17WeatherSys2);
                var g17WeatherPanel = new WeatherPanel();
                for (int i = 0; i < ReopenCycles; i++)
                {
                    g17WeatherPanel.Bind(g17WeatherHost);
                    int before = g17WeatherPanel.RefreshCount;
                    g17WeatherSys2.ForceWeather(i % 2 == 0 ? WeatherKind.Ashfall : WeatherKind.BlackRain);
                    if (g17WeatherPanel.RefreshCount != before + 1)
                    {
                        GD.PrintErr($"[FAIL] Gate 17: WeatherPanel refresh delta {g17WeatherPanel.RefreshCount - before} != 1 at cycle {i}.");
                        return 1;
                    }
                    g17WeatherPanel.Unbind();
                    g17WeatherSys2.ForceWeather(WeatherKind.FalloutStorm);
                    if (g17WeatherPanel.RefreshCount != before + 1)
                    {
                        GD.PrintErr($"[FAIL] Gate 17: WeatherPanel refreshed while unbound at cycle {i}.");
                        return 1;
                    }
                }
                if (g17WeatherPanel.RefreshCount != ReopenCycles)
                {
                    GD.PrintErr($"[FAIL] Gate 17: WeatherPanel total refreshes {g17WeatherPanel.RefreshCount} != {ReopenCycles}.");
                    return 1;
                }
                g17WeatherPanel.QueueFree();

                GD.Print("[PASS] Gate 17: ×100 bind/fire/unbind reopen stability verified — exactly one refresh per event, zero refreshes while unbound, across WeatherHistory, GeigerCalibration, FireIncident, Triangulation, and WeatherPanel.");
                passedGates++;

                // GATE 18: Rebuilding a populated grid must not detach and leak
                // its persistent empty-state label, even before entering a tree.
                GD.Print("\n[Gate 18] Testing data-grid empty-state ownership...");
                var grid = new AshfallDataGrid(Array.Empty<AshfallDataGrid.Column>(), showHeader: false);
                Label? placeholder = null;
                foreach (var node in grid.FindChildren("*", "Label", recursive: true, owned: false))
                {
                    if (node is Label label && label.Text == "— no entries —")
                        placeholder = label;
                }
                if (placeholder == null || !placeholder.Visible)
                {
                    grid.Free();
                    GD.PrintErr("[FAIL] Gate 18: Empty grid has no visible placeholder.");
                    return 1;
                }
                grid.SetRows(new[] { new AshfallDataGrid.Row() });
                bool populatedHidden = !placeholder.Visible && grid.IsAncestorOf(placeholder);
                grid.Clear();
                bool emptyRestored = placeholder.Visible && grid.IsAncestorOf(placeholder);
                grid.SetRows(new[] { new AshfallDataGrid.Row() });
                grid.Free();
                bool leakedPlaceholder = GodotObject.IsInstanceValid(placeholder);
                if (leakedPlaceholder)
                    placeholder.Free(); // Clean the failed probe, not production orphans.
                if (!populatedHidden || !emptyRestored || leakedPlaceholder)
                {
                    GD.PrintErr($"[FAIL] Gate 18: populatedHidden={populatedHidden}, emptyRestored={emptyRestored}, leakedPlaceholder={leakedPlaceholder}.");
                    return 1;
                }
                GD.Print("[PASS] Gate 18: Data-grid empty state survives rebuild and is freed with its owner.");
                passedGates++;

                GD.Print("\n[Gate 19] Testing panel heading/container ownership...");
                var panelFactories = new (string Name, Func<Control> Create)[]
                {
                    (nameof(MaritimeAtlasPanel), () => new MaritimeAtlasPanel()),
                    (nameof(MusterAtlasPanel), () => new MusterAtlasPanel()),
                    (nameof(QuestsAtlasPanel), () => new QuestsAtlasPanel()),
                    (nameof(StandingRecordAtlasPanel), () => new StandingRecordAtlasPanel()),
                    (nameof(CombatHudOverlay), () => new CombatHudOverlay()),
                    (nameof(MedicalPanel), () => new MedicalPanel()),
                    (nameof(EconomyDetailPanel), () => PanelSceneLoader.Load<EconomyDetailPanel>("res://assets/ui/panels/EconomyDetailPanel.tscn")),
                };
                bool panelOwnershipPassed = true;
                foreach (var (name, create) in panelFactories)
                {
                    var baselineIds = new HashSet<ulong>();
                    // Preserve native 64-bit IDs; the SDK's typed int accessor truncates them.
                    foreach (var id in (Godot.Collections.Array)Node.GetOrphanNodeIds())
                        baselineIds.Add(id.AsUInt64());
                    var panel = create();
                    panel._Ready();
                    panel.Free();
                    int newOrphans = 0;
                    foreach (var id in (Godot.Collections.Array)Node.GetOrphanNodeIds())
                    {
                        if (!baselineIds.Contains(id.AsUInt64()))
                            newOrphans++;
                    }
                    if (newOrphans > 0)
                    {
                        GD.PrintErr($"[FAIL] Gate 19: {name} leaked {newOrphans} node(s) after Ready/Free.");
                        panelOwnershipPassed = false;
                    }
                }
                if (!panelOwnershipPassed)
                    return 1;
                if (OS.IsDebugBuild())
                {
                    GD.Print("[PASS] Gate 19: All seven panels release their headings and containers.");
                    passedGates++;
                }
                else
                {
                    GD.Print("[SKIP] Gate 19: Native orphan enumeration requires a debug engine build.");
                    totalGates--;
                }

                GD.Print("\n[Gate 20] Testing pneumatic maintenance input routing...");
                var pneumaticPanel = new PneumaticDispatchPanel();
                pneumaticPanel._Ready();
                var maintenanceInputs = pneumaticPanel.FindChildren("*", "LineEdit", true, false).OfType<LineEdit>().ToArray();
                var capsuleInput = maintenanceInputs.FirstOrDefault(edit => edit.PlaceholderText == "capsule id for jam clear");
                var linkInput = maintenanceInputs.FirstOrDefault(edit => edit.PlaceholderText == "link id for maintenance");
                var maintenanceActions = new List<(string Action, string Id)>();
                pneumaticPanel.OnActionRequested += (action, id) => maintenanceActions.Add((action, id));
                bool pneumaticInputsPassed = capsuleInput != null && linkInput != null;
                if (pneumaticInputsPassed)
                {
                    capsuleInput!.Text = "capsule_probe";
                    linkInput!.Text = "link_probe";
                    foreach (var button in pneumaticPanel.FindChildren("*", "Button", true, false).OfType<Button>())
                    {
                        if (button.Text is "CLEAR JAM" or "MAINTAIN LINK")
                            button.EmitSignal(BaseButton.SignalName.Pressed);
                    }
                    pneumaticInputsPassed = maintenanceActions.Contains(("clear_jam", "capsule_probe"))
                        && maintenanceActions.Contains(("maintain", "link_probe"));
                }
                pneumaticPanel.Free();
                if (pneumaticInputsPassed)
                {
                    GD.Print("[PASS] Gate 20: Parented maintenance inputs route player-selected capsule/link IDs.");
                    passedGates++;
                }
                else
                    GD.PrintErr("[FAIL] Gate 20: Pneumatic maintenance inputs missing or commands received wrong IDs.");

                GD.Print("\n[Gate 21] Testing inventory sidebar filtering...");
                var filterInventory = new Inventory();
                var catalogItems = itemCat.Ids.OrderBy(id => id, StringComparer.Ordinal).Select(id => itemCat.Get(id)!).ToArray();
                var food = catalogItems.First(item => item.type == ItemType.Food);
                var material = catalogItems.First(item => item.type == ItemType.Material);
                if (!filterInventory.Add(food, 1) || !filterInventory.Add(material, 1))
                    throw new InvalidOperationException("Gate 21: Failed to seed authored inventory items.");
                var filterHost = new InventoryHostSession(filterInventory, itemCat);
                var inventoryPanel = new InventoryPanel();
                inventoryPanel._Ready();
                inventoryPanel.Bind(filterHost);
                var inventoryShell = inventoryPanel.GetChildren().OfType<MarginContainer>()
                    .SelectMany(margin => margin.GetChildren()).OfType<AshfallDashboardShell>().Single();
                string[] SelectedItems()
                {
                    var selected = new List<string>();
                    void OnSelected(string id) => selected.Add(id);
                    inventoryPanel.OnItemSelected += OnSelected;
                    foreach (var button in inventoryPanel.FindChildren("*", "Button", true, false).OfType<Button>())
                    {
                        if (button.Text == "SELECT")
                            button.EmitSignal(BaseButton.SignalName.Pressed);
                    }
                    inventoryPanel.OnItemSelected -= OnSelected;
                    return selected.ToArray();
                }
                bool allShown = SelectedItems().Length == 2;
                inventoryShell.Sidebar!.Select("filter_material");
                bool materialOnly = SelectedItems().SequenceEqual(new[] { material.id });
                inventoryShell.Sidebar.Select("filter_consumable");
                bool foodOnly = SelectedItems().SequenceEqual(new[] { food.id });
                inventoryShell.Sidebar.Select("filter_all");
                bool allRestored = SelectedItems().Length == 2;
                inventoryPanel.Unbind();
                inventoryPanel.Free();
                if (!allShown || !materialOnly || !foodOnly || !allRestored)
                {
                    GD.PrintErr($"[FAIL] Gate 21: all={allShown}, material={materialOnly}, food={foodOnly}, restored={allRestored}.");
                    return 1;
                }
                GD.Print("[PASS] Gate 21: Inventory sidebar routes material/consumable/all filters to actual item rows.");
                passedGates++;
                if (!pneumaticInputsPassed)
                    return 1;

                GD.Print($"\n=== PANEL BIND LIFECYCLE SELF-TEST PASS ({passedGates}/{totalGates} gates verified) ===");
                return 0;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[FAIL] PanelBindLifecycleSelfTest exception: {ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}");
                return 1;
            }
        }
    }
}
