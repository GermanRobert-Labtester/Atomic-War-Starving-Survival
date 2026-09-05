using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;
using Ashfall.Core.Medical;
using Ashfall.Core.Survivors;
using Ashfall.Core.Radio;
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
            int totalGates = 15;

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
                var invHost = new InventoryHostSession();
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
                string gate5DataDir = string.IsNullOrEmpty(dataDirectory) ? "Assets/StreamingAssets/Data" : dataDirectory;
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

                // Call Bind 10 times consecutively without unbinding
                for (int i = 0; i < 10; i++)
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
                string dataDir = string.IsNullOrEmpty(dataDirectory) ? "Assets/StreamingAssets/Data" : dataDirectory;
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
                    new QuestsPanel()
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

                // 1. WeatherHistoryPanel (repeated x10 bind and switch)
                var weatherHistPanel = new WeatherHistoryPanel();
                var wSys1 = new WeatherSystem();
                var wSys2 = new WeatherSystem();
                for (int i = 0; i < 10; i++)
                {
                    weatherHistPanel.Bind(wSys1);
                }
                weatherHistPanel.Bind(wSys2);
                weatherHistPanel.QueueFree();

                // 2. GeigerCalibrationPanel (repeated x10 bind and switch)
                var geigerPanel = new GeigerCalibrationPanel();
                var doseHost1 = new DoseLedgerHostSession();
                var doseHost2 = new DoseLedgerHostSession();
                for (int i = 0; i < 10; i++)
                {
                    geigerPanel.Bind(doseHost1, "tag_1");
                }
                geigerPanel.Bind(doseHost2, "tag_1");
                geigerPanel.QueueFree();

                // 3. FireIncidentPanel (repeated x10 bind and switch)
                var fireIncPanel = new FireIncidentPanel();
                var fSys1 = new ShelterFireHazardSystem();
                var fSys2 = new ShelterFireHazardSystem();
                for (int i = 0; i < 10; i++)
                {
                    fireIncPanel.Bind(fSys1);
                }
                fireIncPanel.Bind(fSys2);
                fireIncPanel.QueueFree();

                // 4. TriangulationPanel (repeated x10 bind, location-revealed single propagation, and switch)
                var triPanel = new TriangulationPanel();
                var radHost1 = new RadioHostSession(new FactionRadioEngine(), new CoreSeededRng(1), 1);
                var radHost2 = new RadioHostSession(new FactionRadioEngine(), new CoreSeededRng(2), 1);
                int discoveredLocationsCount = 0;
                triPanel.OnLocationDiscovered += _ => discoveredLocationsCount++;

                for (int i = 0; i < 10; i++)
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
                    GD.PrintErr($"[FAIL] Gate 15: Expected exactly 1 location discovery after 10x rebind on radHost1, got {discoveredLocationsCount}.");
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

                triPanel.QueueFree();
                GD.Print("[PASS] Gate 15: Repeated Bind subscription symmetry verified across WeatherHistory, GeigerCalibration, FireIncident, and Triangulation.");
                passedGates++;

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
