# ASHFALL — COMPREHENSIVE MASTER INTEGRATION PLAN

> **Status**: 50% complete (all C# systems + data + tests done; UI + new mechanics pending)
> **Last updated**: Phase 13 commit `0367f9d`
> **Target**: 90%+ integration ready for playtesting

---

## I. WHAT IS COMPLETE (Phases 0–13)

### A. Survivor Model Expansion
**File**: `Assets/_Game/Survivors/Survivor.cs`
- 4 new enums: `RadiationSicknessPhase`, `MoralBranchDirection`, `ChemicalDependencyKind`, `RiskBiasTrait` (existing)
- 3 new structs: `GuiltRecord`, `ChemicalDependency`, `TraumaBondRecord`
- ~35 new serialized fields covering all 40 original expansion mechanics
- Helper properties: `HasActiveRadiationSickness`, `HasChemicalDependency`, `HasMoralBranch`, `HasTraumaBondWith()`, `HasGrudgeAgainst()`

### B. 22 New C# Systems (plain C#, save-safe, event-driven)

| # | System | File | Assembly |
|---|--------|------|----------|
| 1 | RadiationPhaseProgression | `Radiation/RadiationPhaseProgression.cs` | Radiation |
| 2 | PhantomMemorySystem | `Survivors/PhantomMemorySystem.cs` | Survivors |
| 3 | GuiltInsomniaSystem | `Survivors/GuiltInsomniaSystem.cs` | Survivors |
| 4 | CombatTraumaSystem | `Survivors/CombatTraumaSystem.cs` | Survivors |
| 5 | SomaticFlashbackSystem | `Survivors/SomaticFlashbackSystem.cs` | Survivors |
| 6 | MoralBranchingSystem | `Survivors/MoralBranchingSystem.cs` | Survivors |
| 7 | RespiratoryDegenerationSystem | `Medical/RespiratoryDegenerationSystem.cs` | Medical |
| 8 | ChemicalDependencySystem | `Medical/ChemicalDependencySystem.cs` | Medical |
| 9 | TradeSpecialtySystem | `Survivors/TradeSpecialtySystem.cs` | Survivors |
| 10 | FinalWishSystem | `Survivors/FinalWishSystem.cs` | Survivors |
| 11 | PersonalKeepsakeSystem | `Survivors/PersonalKeepsakeSystem.cs` | Survivors |
| 12 | MemorialWallSystem | `Survivors/MemorialWallSystem.cs` | Survivors |
| 13 | TraumaBondSystem | `Survivors/TraumaBondSystem.cs` | Survivors |
| 14 | IdeologicalFrictionSystem | `Survivors/IdeologicalFrictionSystem.cs` | Survivors |
| 15 | RationConflictSystem | `Survivors/RationConflictSystem.cs` | Survivors |
| 16 | LeadershipSystem | `Survivors/LeadershipSystem.cs` | Survivors |
| 17 | CaregivingSystem | `Survivors/CaregivingSystem.cs` | Survivors |
| 18 | GarrisonConscriptionSystem | `Factions/GarrisonConscriptionSystem.cs` | Factions |
| 19 | AshSignCultSystem | `Factions/AshSignCultSystem.cs` | Factions |
| 20 | ScavengerRefugeSystem | `Factions/ScavengerRefugeSystem.cs` | Factions |
| 21 | PhantomTriggerCatalogLoader | `Data/PhantomTriggerCatalogLoader.cs` | Data |
| 22 | GameBootstrap.Phase11Hud | `Core/GameBootstrap.Phase11Hud.cs` | Core |

### C. GameBootstrap Wiring (7 partial class files)
- `GameBootstrap.Phase0Expansion.cs` — 10 system constructions + tick registration
- `GameBootstrap.Phase2Wiring.cs` — ExpeditionSystem, HatchDefenseSystem, EventRunner hooks
- `GameBootstrap.Phase3Wiring.cs` — AudioEventBus, Inventory, AddictionSystem hooks
- `GameBootstrap.Phases4to6Wiring.cs` — CraftingSystem, RadioTunerSystem, ExpeditionSystem hooks
- `GameBootstrap.Phases7to8Wiring.cs` — HatchDefenseSystem, CookingSystem hooks
- `GameBootstrap.Phases9to10Wiring.cs` — Daily faction ticks + endgame path scoring
- `GameBootstrap.Phase11Hud.cs` — UI widget wiring stubs

### D. JSON Data Catalogs (13 files in `StreamingAssets/Data/`)
| File | Records | Purpose |
|------|---------|---------|
| `phantom_triggers.json` | 7 backgrounds × categories | Item→memory trigger mapping |
| `guilt_sources.json` | 20 patterns | Choice→guilt severity mapping |
| `trade_specialties.json` | 4 professions × 3 tiers | Milestone crafting trees |
| `final_wishes.json` | 8 archetypes | Terminal questline definitions |
| `relic_recipes.json` | 6 relics | Component requirements + morale bonuses |
| `radio_distress_signals.json` | 5 frequencies | Multi-day message fragments |
| `cassette_sets.json` | 4 sets (14 parts) | Hidden cache locations |
| `damaged_map_zones.json` | 3 zones | Fragment assembly → hidden installations |
| `confession_secrets.json` | 8 archetypes | Forgiveness/grudge outcomes |
| `wall_carving_templates.json` | 15 templates | Morale-band-specific carvings |
| `chemical_dependency_items.json` | 13 items | Dependency kind + severity |
| `expansion_survivor_fields.json` | 72 survivors | Background/profession/belief/keepsake mapping |
| `expansion_item_tags.json` | 70+ items | Phantom/dependency/relic/milestone tags |

### E. EditMode Tests (29 tests in 2 files)
- `RadiationPhaseProgressionTests.cs` — 9 tests
- `ExpansionIntegrationTests.cs` — 20 tests across 7 systems

---

## II. WHAT REMAINS — Priority Order

### PRIORITY 0: Unity Compilation Verification
**Owner**: Developer with Unity Editor access
**Action**: Open project in Unity 6000.5.5f1, fix compilation errors.
**Common issues to check**:
1. Missing `using` statements in new partial GameBootstrap files
2. `AddictionSystem` property name mismatch (`Addiction` vs `AddictionSystem`)
3. `CookingSystem` property name mismatch
4. `HatchDefenseSystem.ForceRaid()` method existence
5. `Shelter.GetAvailableBedCount()` method existence
6. `SaveSystem.GetWorldFlag()` and `GetFactionStanding()` existence
7. Assembly definition reference chains — verify Radiation→Survivors, Medical→Survivors
8. `EventBus.Subscribe<>` calls — ensure type parameters match

### PRIORITY 1: UI Widget Implementation (8 widgets)
**Owner**: Cursor/Antigravity (see `INTEGRATION_PLAN_FOR_CURSOR.md`)
**Current state**: 8 C# MonoBehaviour stubs exist (~100 lines each, ~920 total lines). Need:
- UXML layout files (8 files)
- USS styling (reuse `DiegeticHud.uss` + `UIBatch20_A.uss` palette)
- Full `SetXxx()` method implementations
- Event subscription wiring in `GameBootstrap.Phase11Hud.cs`
- `[SerializeField]` declarations in `HUD.cs`
- Inspector prefab assignments in `Gameplay.unity`

### PRIORITY 2: "Antigravity" 40 New Mechanics (#41–80)
**Owner**: This developer (Pi)
**Scope**: 40 additional mechanics across Sections V–VIII (see Section III below)
**Estimated effort**: 8–12 new system files, expanded Survivor fields, JSON data, wiring, tests

### PRIORITY 3: Balance Pass
**Owner**: Designer
- Run 100 simulated campaigns
- Tune threshold constants so mechanics fire at appropriate rates
- Verify no system dominates gameplay

### PRIORITY 4: Writer Pass
**Owner**: Narrative designer
- Add 3–5 variations per phantom trigger background
- Add 2+ confession variations per archetype
- Write campfire/mess-table contextual dialogue fragments

### PRIORITY 5: PlayMode Testing
**Owner**: QA
- End-to-end radiation pipeline (expose→Prodromal→Latent→Manifest→Fibrosis→death→vigil)
- Cross-system save/load round-trip
- Performance: 50 survivors all systems ticking, no GC pressure

---

## III. "ANTIGRAVITY" 40 NEW MECHANICS (#41–80) — Implementation Plan

These are ADDITIONAL mechanics beyond the original 40 already implemented. They fall into 4 sections.

### Section V: Trauma-Driven Physical & Mental Quirks (#41–50)

| # | Mechanic | Implementation Approach | New Files Needed | Survivor Fields Needed |
|---|----------|------------------------|------------------|----------------------|
| 41 | Claustrophobic Tremors | Extend `SpatialPsychologySystem` | No new file — add to existing | `ClaustrophobiaLevel`, `OutdoorHoursNeeded` |
| 42 | Auditory Tinnitus | New `TinnitusSystem.cs` in Medical/ | 1 file | `HasTinnitus`, `TinnitusHoursRemaining`, `RaidWarningDeafness` |
| 43 | Somnambulism (Sleepwalking) | Extend `SleepDeprivationSystem` | No new file | `SleepwalkingRisk`, `LastSleepwalkLocation` |
| 44 | Phantom Hunger & Hoarding | New `HoardingBehaviorSystem.cs` in Survivors/ | 1 file | `HasHoardingCompulsion`, `HiddenFoodCount` |
| 45 | Hyper-Vigilant Scavenger's Eye | Extend `ExpeditionPerkSystem` | No new file | `HasScavengerEye` (add to existing perk list) |
| 46 | Nerve Damage & Steady Hand Strain | New `NerveDamageSystem.cs` in Medical/ | 1 file | `HasNerveDamage`, `NerveStabilizerHours`, `WeaponAccuracyModifier` |
| 47 | Survivor Bond Resilience (Lifeline) | Extend `TraumaBondSystem` | No new file | `LifelinePartnerId`, `HasLifelinePerk` |
| 48 | Anosmia (Loss of Smell) | New `AnosmiaSystem.cs` in Medical/ | 1 file | `HasAnosmia`, `GasLeakBlindness` |
| 49 | Survivor Faith / Philosophy | Extend `BeliefSystem` | No new file | `PhilosophicalStance` (enum: Stoicism/Faith/Rationalism) |
| 50 | Terminal Radiation Acceptance | Extend `PrognosisPipeline` or `FinalWishSystem` | No new file | `HasTerminalAcceptance`, `VolunteerMoraleImmunity` |

**Subtotal**: 4 new files, 6 extensions to existing systems, ~15 new Survivor fields

### Section VI: Environmental Narrative & World State Progression (#51–60)

| # | Mechanic | Implementation Approach | New Files Needed |
|---|----------|------------------------|------------------|
| 51 | Fallout Season Transitions | Extend `SeasonProfile` + `WeatherSystem` | No new file — data-driven |
| 52 | Ash Drift Room Burial | New `AshDriftBurialSystem.cs` in Environment/ | 1 file |
| 53 | Contaminated Rain Aquifer Seepage | Extend `WaterEconomySystem` or `WaterStorage` | No new file |
| 54 | Radio Frequency Station Evolution | Extend `RadioTunerSystem` + JSON data | No new file |
| 55 | Ghost Town Repopulation/Abandonment | New `LocationEvolutionSystem.cs` in World/ | 1 file |
| 56 | Structural Rust & Moisture Decay | Extend `ShelterDegradationSystem` | No new file |
| 57 | Wasteland Wildlife Behavior Cycles | New `WildlifeMigrationSystem.cs` in Environment/ | 1 file |
| 58 | Surface Landmark Degradation | New `LandmarkDegradationSystem.cs` in World/ | 1 file |
| 59 | Atmospheric Pressure & Ozone Spikes | Extend `WeatherSystem` + `OzoneScourgeSystem` | No new file |
| 60 | Survivor Graveyard & Memorial Growth | Extend `MemorialWallSystem` + add grave markers | No new file |

**Subtotal**: 5 new files, 5 extensions

### Section VII: Emergency Narrative Dilemmas & Crisis Events (#61–70)

These are primarily **JSON data + EventRunner integration** — narrative events with choice branches. Most need NO new C# systems, only event entries in `events.json` and narrative choice wiring.

| # | Event | Implementation |
|---|-------|---------------|
| 61 | Contaminated Water Trade | New event entry in `events.json` |
| 62 | Infected Refugee Isolation | New event entry + `quarantine_room` check |
| 63 | Sabotaged Air Filter Discovery | New event entry + `SuspicionTracker` integration |
| 64 | Faction Deserter's Asylum Request | New event entry + `GarrisonConscriptionSystem` trigger |
| 65 | Stranded Expedition Rescue Call | New event entry + expedition timer |
| 66 | Stolen Medication Accusation | New event entry + item search mechanic |
| 67 | Dying Trader's Last Bargain | New event entry + locked-crate item |
| 68 | Broken Generator Dilemma | New event entry + `PowerNetwork` routing |
| 69 | Unwanted Bounty Harvest | New event entry + faction relationship check |
| 70 | False Distress Signal Bait | New event entry + ambush encounter |

**Subtotal**: 10 new event entries in `events.json`, minor EventRunner condition wiring

### Section VIII: Legacy, Heritage & Long-Term Arcs (#71–80)

| # | Mechanic | Implementation Approach | New Files Needed |
|---|----------|------------------------|------------------|
| 71 | Survival Codex / Bunker Manifesto | New `BunkerManifestoSystem.cs` in Survivors/ | 1 file |
| 72 | Generational Knowledge Transfer | Extend `MentorshipSystem` + `ChildDevelopmentSystem` | No new file |
| 73 | Vault of Pre-War Culture | New `CulturalPreservationSystem.cs` in Narrative/ | 1 file |
| 74 | Radio Network Unification Path | Extend `RadioTunerSystem` — multi-station tracking | No new file |
| 75 | Deep Geothermal / Aquifer Project | New `DeepAquiferProjectSystem.cs` in Shelter/ | 1 file |
| 76 | Armored Command Vehicle Restoration | New `VehicleRestorationSystem.cs` in World/ | 1 file |
| 77 | Bio-Dome Hydroponics Upgrade | Extend `PlanterBox` / `HydroponicsCropHUD` | No new file |
| 78 | Faction Peace Treaty Negotiation | New `PeaceTreatySystem.cs` in Factions/ | 1 file |
| 79 | Final Fallout Cleansing Forecast | New `FalloutForecastSystem.cs` in Environment/ | 1 file |
| 80 | Campaign Legacy Summary (Chronicle of Ash) | Extend `MoralChronicleBridge` + `System_EpilogueStats` | No new file |

**Subtotal**: 6 new files, 4 extensions

---

## IV. FULL IMPLEMENTATION CHECKLIST (90% Target)

### Phase 14: Compilation Fixes
- [ ] Open project in Unity 6000.5.5f1
- [ ] Fix all compiler errors
- [ ] Verify all 22 new systems instantiate without exceptions
- [ ] Run existing test suite — ensure 0 regressions
- [ ] Run `RadiationPhaseProgressionTests` and `ExpansionIntegrationTests`

### Phase 15: UI Widget Implementation (see Cursor plan)
- [ ] 8 UXML files created
- [ ] USS styling applied
- [ ] Full C# implementations
- [ ] GameBootstrap wiring complete
- [ ] HUD.cs inspector references assigned
- [ ] Widgets visible and reactive in PlayMode

### Phase 16: Antigravity Mechanics #41–50 (Physical & Mental Quirks)
- [ ] New Survivor fields added
- [ ] 4 new system files created
- [ ] 6 existing systems extended
- [ ] JSON data populated
- [ ] GameBootstrap wiring
- [ ] 10+ EditMode tests

### Phase 17: Antigravity Mechanics #51–60 (Environmental Narrative)
- [ ] 5 new system files created
- [ ] 5 existing systems extended
- [ ] JSON data populated (season transitions, wildlife patterns)
- [ ] GameBootstrap wiring
- [ ] 8+ EditMode tests

### Phase 18: Antigravity Mechanics #61–70 (Emergency Dilemmas)
- [ ] 10 new event entries in `events.json`
- [ ] Choice wiring in EventRunner
- [ ] Any required system hooks
- [ ] 5+ EditMode tests

### Phase 19: Antigravity Mechanics #71–80 (Legacy & Long-Term Arcs)
- [ ] 6 new system files created
- [ ] 4 existing systems extended
- [ ] JSON data populated
- [ ] GameBootstrap wiring
- [ ] 8+ EditMode tests

### Phase 20: Full Integration Pass
- [ ] Cross-system save/load round-trip test
- [ ] 50-survivor performance test
- [ ] All UI widgets display correct state after load
- [ ] No orphaned event subscriptions

### Phase 21: Balance Tuning
- [ ] 100 simulated campaigns
- [ ] Threshold tuning document
- [ ] Phantom trigger rates: 10-20% per expedition (not too frequent)
- [ ] Combat trauma: meaningful after 5+ raids, not after 1
- [ ] Moral branching: typically triggers days 30-50
- [ ] Terminal prognosis: ~15-25% of heavily irradiated survivors

### Phase 22: Narrative Polish
- [ ] 3+ phantom trigger variations per background
- [ ] 2+ confession variations per archetype
- [ ] Wall carving variety pass
- [ ] Cassette set dialogue polish
- [ ] Radio distress signal polish

---

## V. FILE MANIFEST — EVERYTHING TOUCHED

### New System Files (C#)
```
Assets/_Game/Radiation/RadiationPhaseProgression.cs
Assets/_Game/Medical/RespiratoryDegenerationSystem.cs
Assets/_Game/Medical/ChemicalDependencySystem.cs
Assets/_Game/Survivors/PhantomMemorySystem.cs
Assets/_Game/Survivors/GuiltInsomniaSystem.cs
Assets/_Game/Survivors/CombatTraumaSystem.cs
Assets/_Game/Survivors/SomaticFlashbackSystem.cs
Assets/_Game/Survivors/MoralBranchingSystem.cs
Assets/_Game/Survivors/TradeSpecialtySystem.cs
Assets/_Game/Survivors/FinalWishSystem.cs
Assets/_Game/Survivors/PersonalKeepsakeSystem.cs
Assets/_Game/Survivors/MemorialWallSystem.cs
Assets/_Game/Survivors/TraumaBondSystem.cs
Assets/_Game/Survivors/IdeologicalFrictionSystem.cs
Assets/_Game/Survivors/RationConflictSystem.cs
Assets/_Game/Survivors/LeadershipSystem.cs
Assets/_Game/Survivors/CaregivingSystem.cs
Assets/_Game/Factions/GarrisonConscriptionSystem.cs
Assets/_Game/Factions/AshSignCultSystem.cs
Assets/_Game/Factions/ScavengerRefugeSystem.cs
Assets/_Game/Data/PhantomTriggerCatalogLoader.cs
```

### New Bootstrap Wiring Files
```
Assets/_Game/Core/GameBootstrap.Phase0Expansion.cs
Assets/_Game/Core/GameBootstrap.Phase2Wiring.cs
Assets/_Game/Core/GameBootstrap.Phase3Wiring.cs
Assets/_Game/Core/GameBootstrap.Phases4to6Wiring.cs
Assets/_Game/Core/GameBootstrap.Phases7to8Wiring.cs
Assets/_Game/Core/GameBootstrap.Phases9to10Wiring.cs
Assets/_Game/Core/GameBootstrap.Phase11Hud.cs
```

### New UI Widget Files (stubs — need implementation)
```
Assets/_Game/UI/RadiationPhaseIndicator.cs + .uxml + .uss
Assets/_Game/UI/PhantomMemoryVignette.cs + .uxml + .uss
Assets/_Game/UI/HypervigilanceIndicator.cs + .uxml + .uss
Assets/_Game/UI/MoralBranchDisplay.cs + .uxml + .uss
Assets/_Game/UI/KeepsakeSlotUI.cs + .uxml + .uss
Assets/_Game/UI/MemorialWallUI.cs + .uxml + .uss
Assets/_Game/UI/TerminalPrognosisBanner.cs + .uxml + .uss
Assets/_Game/UI/AddictionDetoxIndicator.cs + .uxml + .uss
```

### New Test Files
```
Assets/Tests/EditMode/RadiationPhaseProgressionTests.cs
Assets/Tests/EditMode/ExpansionIntegrationTests.cs
```

### New JSON Data Files
```
Assets/StreamingAssets/Data/phantom_triggers.json
Assets/StreamingAssets/Data/guilt_sources.json
Assets/StreamingAssets/Data/trade_specialties.json
Assets/StreamingAssets/Data/final_wishes.json
Assets/StreamingAssets/Data/relic_recipes.json
Assets/StreamingAssets/Data/radio_distress_signals.json
Assets/StreamingAssets/Data/cassette_sets.json
Assets/StreamingAssets/Data/damaged_map_zones.json
Assets/StreamingAssets/Data/confession_secrets.json
Assets/StreamingAssets/Data/wall_carving_templates.json
Assets/StreamingAssets/Data/chemical_dependency_items.json
Assets/StreamingAssets/Data/expansion_survivor_fields.json
Assets/StreamingAssets/Data/expansion_item_tags.json
```

### Modified Existing Files
```
Assets/_Game/Survivors/Survivor.cs (major — ~200 lines added)
Assets/_Game/Core/GameBootstrap.InitFoundation.cs (Phase0 call added)
Assets/_Game/Core/GameBootstrap.InitializeSystems.cs (7 init calls added)
Assets/_Game/Radiation/RadiationSystem.cs (minor)
Assets/_Game/Radiation/PrognosisPipeline.cs (may need phase transition hooks)
Assets/_Game/Shelter/HatchDefenseSystem.cs (RaidResolution used by wiring)
Assets/_Game/Core/SaveSystem.cs and SaveSystem.Wiring.cs (if new ISaveables registered)
```

---

## VI. KNOWN GAPS & GOTCHAS

1. **`AddictionSystem` property name**: The existing `AddictionSystem` in GameBootstrap may be named `Addiction` — wiring files use `AddictionSystem`. Verify and fix.
2. **`CookingSystem` event**: `OnMealCooked` exists but wiring uses it as placeholder — full mess-table integration needs it wired.
3. **`HatchDefenseSystem.ForceRaid()`**: Used in Phase 9 wiring but may not exist — needs `ForceRaid(string raidId)` or fallback.
4. **`Shelter.GetAvailableBedCount()`**: Used in Phase 9 wiring — verify or implement.
5. **`SaveSystem.GetWorldFlag()`**: Used in Phase 10 endgame scoring — verify or implement.
6. **`SaveSystem.GetFactionStanding()`**: Used in Phase 10 — verify or implement.
7. **`EventBus.Subscribe<T>()` with `RaidResolution`**: Only works if `RaidResolution` is a known type to the bus. May need `EventBus.Raise<RaidResolution>()` at the HatchDefenseSystem call site.
8. **UI stubs have no `.uxml` / `.uss` files**: All 8 widgets need UXML layouts created.
9. **UI stubs not wired in `HUD.cs`**: No `[SerializeField]` fields for the 8 new widgets.
10. **`PersonalKeepsakeSystem` and `MemorialWallSystem`**: These exist as stubs at filesystem level but may not have full implementations. Verify content.

---

## VII. NEXT ACTION

```prompt
Start with PRIORITY 0: Open the project in Unity 6000.5.5f1, fix all compilation errors, 
run the existing test suite, and report which files need fixes. Then proceed to Phase 16
(Antigravity mechanics #41-50 — Physical & Mental Quirks).
```
