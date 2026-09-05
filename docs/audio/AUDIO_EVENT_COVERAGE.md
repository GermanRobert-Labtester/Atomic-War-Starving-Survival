# ASHFALL — Audio Event Coverage Contract & Matrix

> **Living Architecture Authority**: Documents the mapping between simulation domain events, audio bridges, target audio cues, continuous loops, and automated verification tests.

**Status:** Authoritative (Reconciled with Flagship Sensory Architecture)
**Last Audit:** 2026-09-02
**Verification Target:** `dotnet test Ashfall.Core.Tests` + `godot --headless --path .`

---

## 1. Architecture Overview

ASHFALL separates simulation rules from host audio presentation via three distinct layers:
1. **Core Systems (`Assets/Ashfall.Core/`)**: Engine-agnostic domain systems emitting strongly-typed C# events.
2. **Audio Bridges (`src/Audio/`)**: Thin host subscribers (`AudioEventBridge`, `ShelterAudioController`, `SurfaceAmbienceController`, `ExpansionAudioBridge`, `AudioConditionHostBridge`).
3. **Audio Manager & Mix Coordinator (`src/Audio/`)**: `AudioManager` managing 12 buses, pools, stream caching, and `AudioStateCoordinator` managing dynamic snapshot ducking.

---

## 2. Master System-to-Audio Coverage Matrix

| System | Domain Event / Trigger | Target Audio Cue | Cue Mode / Bus | Bridge Controller | Test Verification |
|---|---|---|---|---|---|
| `PowerGridSystem` | `OnPowerChanged` (Tripped) | `danger_alarm_klaxon` | One-shot / `Alerts` | `ShelterAudioController` | `ShelterAudioTests` |
| `PowerGridSystem` | `OnTickSummary` (IsBrownout) | `danger_alarm_klaxon` | One-shot / `Alerts` | `ShelterAudioController` | `ShelterAudioTests` |
| `PowerGridSystem` | Active Generation (>0W, Fuel>0) | `shelter_generator` | Loop / `Generator` | `ShelterAudioController` | `ShelterAudioTests` |
| `StartingLevelSystem` | `OnStateChanged` (AirHazardWarning) | `shelter_air_filter` | One-shot / `Alerts` | `ShelterAudioController` | `ShelterAudioTests` |
| `StartingLevelSystem` | Holdfast Air System Active | `shelter_ventilation` | Loop / `Ventilation` | `ShelterAudioController` | `ShelterAudioTests` |
| `RadiationSystem` | Acute Dose Incurred | `rad_alert_acute` | One-shot / `Alerts` | `AudioEventBridge` | `AudioEventIntegrationTests` |
| `RadiationSystem` | Chronic Exposure Limit Cross | `rad_alert_chronic` | One-shot / `Alerts` | `AudioEventBridge` | `AudioEventIntegrationTests` |
| `RadiationSystem` | Active Ambient Radiation Field | `rad_geiger_loop` | Loop / `Sfx` | `AudioManager` | `AudioEventIntegrationTests` |
| `RadiationSystem` | Radiation Burst Spike | `rad_geiger_burst` | One-shot / `Sfx` | `AudioEventBridge` | `AudioEventIntegrationTests` |
| `WeatherSystem` | `OnWeatherChanged` (Severe/Fallout) | `weather_alert` | One-shot / `Alerts` | `AudioEventBridge` | `AudioEventIntegrationTests` |
| `WeatherSystem` | Blizzard Precipitation | `weather_blizzard` | One-shot / `Sfx` | `AudioEventBridge` | `AudioEventIntegrationTests` |
| `WeatherSystem` | Black Rain Outbreak | `weather_black_rain` | One-shot / `Alerts` | `AudioEventBridge` | `AudioEventIntegrationTests` |
| `WeatherSystem` | Corrosive Precipitation | `weather_corrosive_precipitation` | One-shot / `Alerts` | `AudioEventBridge` | `AudioEventIntegrationTests` |
| `WeatherSystem` | EMP Storm Arrival | `weather_emp_storm` | One-shot / `Alerts` | `AudioEventBridge` | `AudioEventIntegrationTests` |
| `WeatherSystem` | Glass Storm Anomaly | `weather_glass_storm` | One-shot / `Alerts` | `AudioEventBridge` | `AudioEventIntegrationTests` |
| `WeatherSystem` | Active Surface Listening | `amb_surface` / `amb_surface_storm` | Loop / `Surface` | `SurfaceAmbienceController` | `AudioEventIntegrationTests` |
| `TacticalCombatSystem` | `OnCombatStarted` | `combat_start` | One-shot / `Alerts` | `AudioEventBridge` | `AudioEventIntegrationTests` |
| `TacticalCombatSystem` | Weapon Fire | `combat_fire` | One-shot / `Sfx` | `AudioEventBridge` | `AudioEventIntegrationTests` |
| `TacticalCombatSystem` | Weapon Hit Confirmed | `combat_hit` | One-shot / `Sfx` | `AudioEventBridge` | `AudioEventIntegrationTests` |
| `TacticalCombatSystem` | Weapon Jammed | `combat_jam` | One-shot / `Sfx` | `AudioEventBridge` | `AudioEventIntegrationTests` |
| `TacticalCombatSystem` | Weapon Reload Cycle | `combat_reload` | One-shot / `Sfx` | `AudioEventBridge` | `AudioEventIntegrationTests` |
| `TacticalCombatSystem` | Fighter Downed | `combat_downed` | One-shot / `Sfx` | `AudioEventBridge` | `AudioEventIntegrationTests` |
| `TacticalCombatSystem` | `OnCombatResolved` (Defeat) | `combat_defeat` | One-shot / `Music` | `AudioEventBridge` | `AudioEventIntegrationTests` |
| `TacticalCombatSystem` | `OnCombatResolved` (Victory) | `combat_victory` | One-shot / `Sfx` | `AudioEventBridge` | `AudioEventIntegrationTests` |
| `CraftingSystem` | `OnCraftCompleted` | `action_crafting` | One-shot / `Sfx` | `AudioEventBridge` | `AudioEventIntegrationTests` |
| `ExpeditionSystem` | `OnExpeditionDispatched` | `shelter_door_open` | One-shot / `Sfx` | `AudioEventBridge` | `AudioEventIntegrationTests` |
| `ExpeditionSystem` | `OnExpeditionReturned` | `shelter_door_seal` | One-shot / `Sfx` | `AudioEventBridge` | `AudioEventIntegrationTests` |
| `DiseaseSystem` | Contagion Contracted | `med_coughing` | One-shot / `Sfx` | `AudioEventBridge` | `AudioEventIntegrationTests` |
| `DiseaseSystem` | Quarantine Initiated | `med_quarantine_seal` | One-shot / `Medical` | `AudioEventBridge` | `AudioEventIntegrationTests` |
| `DiseaseSystem` | Quarantine Cleared | `med_quarantine_clear` | One-shot / `Medical` | `AudioEventBridge` | `AudioEventIntegrationTests` |
| `SurvivorFateSystem` | `OnSurvivorFate` (Death) | `med_survivor_death` | One-shot / `Medical` | `AudioEventBridge` | `AudioEventIntegrationTests` |
| `AudioConditionSystem` | `OnConditionStarted` | Condition-keyed cue | Dynamic / Configured | `AudioConditionHostBridge` | `AudioConditionTests` |
| `AudioConditionSystem` | `OnConditionsChanged` | Intensity gain | Loop / Attenuation | `AudioConditionHostBridge` | `AudioConditionTests` |
| `AudioConditionSystem` | `OnConditionStopped` | Stop loop | Loop / Stop | `AudioConditionHostBridge` | `AudioConditionTests` |
| `CrisisPresentationCoordinator` | `OnCrisisChanged` (Critical) | `crisis_critical_stinger` | One-shot + Snapshot duck | `EmergencyResponseHud` & `AudioStateCoordinator` | `CrisisPresentationCoordinatorTests` |
| `CrisisPresentationCoordinator` | `OnCrisisChanged` (Severe) | `crisis_severe_stinger` | One-shot + Snapshot duck | `EmergencyResponseHud` & `AudioStateCoordinator` | `CrisisPresentationCoordinatorTests` |
| `DesperationSystem` | `OnTabooBroken` | `action_interrogation_slam` | One-shot / `Sfx` | `ExpansionAudioBridge` | `ExpansionAudioBridgeTests` |
| `MutationSystem` | `OnMutationAcquired` | `bio_mutation_pulse` | One-shot / `Sfx` | `ExpansionAudioBridge` | `ExpansionAudioBridgeTests` |
| `ChemWarfareSystem` | `OnHazardDeployed` | `hazard_toxic_sizzle` | One-shot / `Sfx` | `ExpansionAudioBridge` | `ExpansionAudioBridgeTests` |
| `RailwaySystem` | `OnDerailment` | `train_screech_crash` | One-shot / `Sfx` | `ExpansionAudioBridge` | `ExpansionAudioBridgeTests` |
| `ExpeditionSystem` / World | Hospital Exploration | `amb_loc_abandoned_hospital` | Loop / `Ambience` | `SurfaceAmbienceController` | `AudioSelfTest` |
| `ExpeditionSystem` / World | Gas Station Scouting | `amb_loc_rural_gas_station` | Loop / `Surface` | `SurfaceAmbienceController` | `AudioSelfTest` |
| `ExpeditionSystem` / World | Suburban Ruins Salvage | `amb_loc_suburban_ruins` | Loop / `Surface` | `SurfaceAmbienceController` | `AudioSelfTest` |
| `ExpeditionSystem` / World | Military Bunker Infiltration | `amb_loc_military_bunker` | Loop / `Ambience` | `SurfaceAmbienceController` | `AudioSelfTest` |
| `ExpeditionSystem` / World | Geothermal Plant Breach | `amb_loc_geothermal_ruins` | Loop / `Surface` | `SurfaceAmbienceController` | `AudioSelfTest` |
| `ExpeditionSystem` / World | Arcology Sector Exploration | `amb_loc_arcology_sector` | Loop / `Ambience` | `SurfaceAmbienceController` | `AudioSelfTest` |
| `ExpeditionSystem` / World | Warzone / Frontline Sector | `amb_warzone_distant_shelling` | Loop / `Surface` | `SurfaceAmbienceController` | `AudioSelfTest` |
| `TacticalCombatSystem` | Distant Artillery Impact | `sfx_distant_artillery_barrage` | One-shot / `Sfx` | `AudioEventBridge` | `AudioSelfTest` |
| `TacticalCombatSystem` | Distant Skirmish Gunfire | `sfx_distant_gunfire_skirmish` | One-shot / `Sfx` | `AudioEventBridge` | `AudioSelfTest` |
| `TacticalCombatSystem` | Incoming Shell Whistle | `sfx_artillery_incoming_whistle` | One-shot / `Alerts` | `AudioEventBridge` | `AudioSelfTest` |
| `TacticalCombatSystem` | Distant Mortar Launch | `sfx_distant_mortar_launch` | One-shot / `Sfx` | `AudioEventBridge` | `AudioSelfTest` |
| `TacticalCombatSystem` | Pistol (CZ 75) Report | `sfx_weapon_cz75_report` | One-shot / `Sfx` | `AudioEventBridge` | `AudioSelfTest` |
| `TacticalCombatSystem` | Pipe Rifle Report | `sfx_weapon_pipe_rifle_report` | One-shot / `Sfx` | `AudioEventBridge` | `AudioSelfTest` |
| `TacticalCombatSystem` | Scrap Shotgun Blast | `sfx_weapon_scrap_shotgun_report` | One-shot / `Sfx` | `AudioEventBridge` | `AudioSelfTest` |
| `TacticalCombatSystem` | Bolt-Action Rifle Report | `sfx_weapon_bolt_rifle_report` | One-shot / `Sfx` | `AudioEventBridge` | `AudioSelfTest` |
| `TacticalCombatSystem` | Assault Rifle 3-Round Burst | `sfx_weapon_assault_rifle_burst` | One-shot / `Sfx` | `AudioEventBridge` | `AudioSelfTest` |
| `TacticalCombatSystem` | Light Machine Gun 5-Round Burst | `sfx_weapon_lmg_burst` | One-shot / `Sfx` | `AudioEventBridge` | `AudioSelfTest` |
| `TacticalCombatSystem` | Heavy Anti-Materiel Sniper | `sfx_weapon_sniper_heavy_report` | One-shot / `Sfx` | `AudioEventBridge` | `AudioSelfTest` |
| `TacticalCombatSystem` | Shotgun Pump Action Rack | `sfx_weapon_shotgun_rack` | One-shot / `Sfx` | `AudioEventBridge` | `AudioSelfTest` |
| `TacticalCombatSystem` | Ballistic Ricochet & Whiz | `sfx_bullet_whiz_ricochet` | One-shot / `Sfx` | `AudioEventBridge` | `AudioSelfTest` |
| `TacticalCombatSystem` | Structural Collapse & Rubble | `sfx_structural_collapse` | One-shot / `Sfx` | `AudioEventBridge` | `AudioSelfTest` |
| `TacticalCombatSystem` | Heavy Combatant Fall | `sfx_heavy_impact_fall` | One-shot / `Sfx` | `AudioEventBridge` | `AudioSelfTest` |
| `StartingLevelSystem` | Airlock Purge & Vent | `sfx_airlock_purge_cycle` | One-shot / `Sfx` | `AudioEventBridge` | `AudioSelfTest` |

---

## 3. Dynamic Mixing Snapshot Matrix

| Snapshot | Music Duck | Ambience Duck | SFX Duck | UI Duck | Triggering State |
|---|---:|---:|---:|---:|---|
| `Normal` | 0 dB | 0 dB | 0 dB | 0 dB | Routine shelter operations |
| `Menu` | -8 dB | -6 dB | -10 dB | 0 dB | Main menu, settings modal, pause screen |
| `RadioFocus` | -8 dB | -5 dB | -3 dB | 0 dB | Active incoming radio broadcast / dialogue |
| `VoiceFocus` | -8 dB | -5 dB | -3 dB | 0 dB | Survivor vocalization, briefing playback |
| `MedicalCritical` | -6 dB | -4 dB | -2 dB | 0 dB | Triage mode, terminal vital signs |
| `ShelterCrisis` | -10 dB | -7 dB | -3 dB | -2 dB | `EmergencyResponseHud` active, critical alert |
| `Combat` | -4 dB | -4 dB | 0 dB | 0 dB | Active tactical engagement |
| `Surface` | -2 dB | 0 dB | 0 dB | 0 dB | Surface listening mode |
| `GameOver` | 0 dB | -12 dB | -12 dB | 0 dB | Run end, campaign debrief |
| `Pause` | -8 dB | -6 dB | -10 dB | 0 dB | Game paused |

---

## 4. Invariants

1. **Zero Allocations in Steady State**: Audio playback uses pre-instantiated player pools and cached stream resources.
2. **Safe Fallbacks**: Missing or unmapped audio cues log a single diagnostic and fall back safely without crashing or hanging the host loop.
3. **Headless Safety**: `AudioManager` detects headless execution (`DisplayServer.WindowCanDraw() == false`) and suppresses hardware audio calls while maintaining logical state.
4. **Deterministic Simulation Boundary**: No audio randomizer may consume from or perturb the simulation `ISeededRng` streams.
