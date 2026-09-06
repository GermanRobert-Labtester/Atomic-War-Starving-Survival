# ASHFALL — Plans 46–49 Authority & Architecture Matrix
## Precision Workshop, Radio Intelligence, Shelter Social Dynamics & Subterranean Hazard Operations

**Milestone:** Plans 46–49 Flagship Implementation Wave
**Primary Systems:**
1. **Plan 46:** Precision Workshop Tooling, Armory Service & Munitions Press (`ShelterWorkshopSystem`)
2. **Plan 47:** Wasteland Radio Triangulation, Encrypted Intercepts & SOS Relays (`ShelterRadioStationSystem`)
3. **Plan 48:** Survivor Room Privacy, Bunk Friction & Communal Hall Quests (`ShelterSocialDynamicsSystem`)
4. **Plan 49:** Subterranean Hazard Remediation & Cave-In Recovery Operations (`ExcavationHazardSystem`)

---

## 1. Executive Architecture Map & Domain Authority

| Responsibility Domain | System Authority | Interface / Contract | Delegated Operations |
|---|---|---|---|
| **Weapon Condition / Durability** | `EquipmentConditionSystem` | `RegisterItem`, `UseItem`, `RepairItem`, `State.items` | Workshop submits repair/refurbish requests; never mutates condition DTOs directly. |
| **Shelter Inventory** | `Inventory.Inventory` | `Add`, `Remove`, `CountById`, `TryProduce` | All operations (ammo reload, parts refit, hazard mitigation) consume materials atomically. |
| **Expedition Vehicle Maintenance** | `ExpeditionVehicleSystem` | `GetVehicle`, `AcquireVehicle`, `LoadCatalog` | Heavy workshop engine rebuilds and chassis servicing delegate directly to vehicle authority. |
| **Radio Intercepts & SOS** | `ShelterRadioStationSystem` | `TuneTo`, `ScanFrequency`, `RecordBearing` | Scans frequencies, resolves ciphers, tracks SOS expiry, and triggers location discoveries. |
| **World Map Discovery** | `WastelandMap` / `WorldSession` | `Discover(locationId)` | Triangulated bearings unlock map nodes through canonical world authority. |
| **Orbital Early Warning** | `OrbitalHarrowTelemetrySystem` (Plan 39) | `GetTelemetryEvent`, `UpcomingStrikeWindow` | Radio room decodes early strike warnings without rescheduling the authoritative strike. |
| **Survivor Social & Affinity** | `SurvivorRelationsSystem` / `NeedsSystem` | `GetOrCreateRelationship`, `affinity`, `morale` | Social dynamics evaluates room pressure and emits deltas to relationship & needs authorities. |
| **Memorial & Collective Grief** | `MemorialSystem` | `MemorialState`, `AddPlaque` | Mess hall vigil events hook memorial authority with cooldown protection. |
| **Subterranean Strata & Sites** | `ExcavationSystem` | `ExcavationState`, `GetActiveSites` | Hazard system monitors methane, flooding, spores, and shoring without duplicating dig progress. |
| **Fortifications & Blast Matting** | `SkyLayerArmorSystem` (Plan 38) | `State.installedPanels` | Subterranean blast matting synergies integrate Plan 38 protective infrastructure. |
| **Deterministic Randomness** | `ISeededRng` / `CampaignRngStream` | `Fork(subsystemId)` | Subsystems fork stable determinism streams (`shelter_workshop`, `radio_station`, etc.). |
| **Persistence Orchestration** | `SaveSectionRegistry` / `Main.Plans46_49.cs` | `CaptureSection`, `RestoreState` | Checksummed envelope save stores register with the campaign save coordinator. |

---

## 2. Authoritative Data Catalogs & Schemas

### 2.1 `workshop_recipes.json`
- **Location:** `Assets/StreamingAssets/Data/workshop_recipes.json`
- **Schema Version:** 1
- **Content:** Authoritative catalog containing fabrication, ammunition reload (`9x19`, `.357`, `5.56`, `12ga`), firearm decoking & precision refurbishing, electronic micro-soldering, radio antenna coil fabrication, and lathe tooling overhauls.
- **Room Gates:** `room_workshop_precision`, `room_armory_munitions`, `room_workshop_heavy`.
- **Assignment Rules:** `rule_workshop_precision`, `rule_armory_service`.

### 2.2 `radio_intercepts.json`
- **Location:** `Assets/StreamingAssets/Data/radio_intercepts.json`
- **Schema Version:** 1
- **Content:** Exactly 16 distinct broadcasts across HF, VHF, and UHF bands:
  1. `radio_intercept_meridian_supply_column_01` (Logistics chatter, 7115 kHz HF, field cipher)
  2. `radio_intercept_sos_quarry_shelter_02` (SOS distress, 3850 kHz HF, unencrypted, 3-day expiry)
  3. `radio_intercept_dead_hand_silo_beacon_03` (Military telemetry, 14220 kHz HF, machine cipher)
  4. `radio_intercept_weather_ionosphere_bulletin_04` (Scientific weather, 5100 kHz HF)
  5. `radio_intercept_orbital_harrow_early_warning_05` (Orbital Harrow warning, 14850 kHz HF)
  6. `radio_intercept_raider_ambush_chatter_06` (Faction ambush chatter, 27150 kHz HF)
  7. `radio_intercept_flotilla_coastal_relay_07` (Maritime navigation, 2182 kHz HF)
  8. `radio_intercept_spoofed_distress_trap_08` (False/spoofed distress signal, 4420 kHz HF)
  9. `radio_intercept_scientific_ice_core_relay_09` (Research telemetry, 10125 kHz HF)
  10. `radio_intercept_cutters_bunker_assault_10` (Tactical combat traffic, 144200 kHz VHF)
  11. `radio_intercept_pumphouse_distress_11` (Aquifer distress, 7240 kHz HF)
  12. `radio_intercept_grain_silo_cache_12` (Survivor resource rumor, 3910 kHz HF)
  13. `radio_intercept_high_mountain_relay_13` (Atmospheric observatory, 146520 kHz VHF)
  14. `radio_intercept_lock_gate_sabotage_14` (Canal sabotage chatter, 7080 kHz HF)
  15. `radio_intercept_snowline_researcher_sos_15` (Researcher distress, 3720 kHz HF)
  16. `radio_intercept_grange_community_hymn_16` (Diegetic community broadcast, 14280 kHz HF)

### 2.3 `shelter_social_events.json`
- **Location:** `Assets/StreamingAssets/Data/shelter_social_events.json`
- **Schema Version:** 1
- **Content:** Dynamic incident templates covering high-density bunk friction, privacy boundary disputes, private quarters solace, communal meal bonding, ideological rationing conflicts, memorial vigils, crafting synergies, and expedition reconciliation.
- **Mediation Gates:** Authored with mediation eligibility flags and required mediation skills (`skill_watchful`, `skill_cold_analysis`, `skill_workshop_sense`).

### 2.4 `excavation_hazard_mitigation.json`
- **Location:** `Assets/StreamingAssets/Data/excavation_hazard_mitigation.json`
- **Schema Version:** 1
- **Content:** Mitigation definitions for forced-air ventilation blowers (methane), emergency flare burn-offs, subterranean sump drainage pumps (flood), biocide spore scrubbing (spores, requiring respiratory protection), heavy timber strut shoring (shoring repair), emergency bulkhead seals, blast matting sandbag curtains, and trapped-miner clearance.

---

## 3. Atomic Transaction Contracts

All four systems enforce atomic inventory and state transactions:
1. **Validation Phase:** Check target existence, operational room capability, worker assignment, and material counts.
2. **Commit Phase:** Deduct all required items simultaneously. If inventory space or material quantity fails, zero items are deducted and the job/mitigation is aborted.
3. **Execution Phase:** Labor ticks advance deterministically. Cancellation prior to commit never duplicates materials.
4. **Collection Phase:** Outputs waiting in completion buffers are transferred to shelter inventory without risk of loss.

---

## 4. Daily Simulation Lifecycle & Execution Order

At the daily simulation boundary (`TickDay`), systems execute in strict deterministic order to prevent race conditions or temporal drift:

```text
1. Environmental & Atmospheric Ducting (Weather effects on radio noise floor)
2. Shelter Infrastructure & Power Check (Operational state of workshop, radio, pumps)
3. Subterranean Hazard Evolution (Methane buildup, flood seepage, shoring degradation, rescue deadlines)
4. Workshop Labor Progression (Queued fabrication, ammo press, refurbishing)
5. Radio Frequency Sweeps & Distress Expiry (Expiry ticks, telemetry sync)
6. Living Space Dynamics (Crowded bunk friction, private quarters rest, communal meals)
7. Mediation & Incident Resolution (Counselor skill evaluation, relationship affinity updates)
8. Campaign Persistence Capture Point (Dirty-flag synchronization)
```

---

## 5. Persistence & Save Compatibility Invariants

- **Zero-Default Catastrophe Prevention:** Default initialization for tooling health is `1.0` (100% operational) and shoring health is `1000` permille. Restoring older saves never spawns broken machinery or instant subterranean cave-ins.
- **State Machine Resumption:** Active jobs, locked radio bearings, pending social mediations, and trapped-miner rescue operations restore their exact tick progress without re-rolling RNG outcomes.
- **Deterministic Pair Ordering:** Survivor relationship pairs are keyed canonically (`dweller_a|dweller_b` where ID ordering is invariant), eliminating dictionary order divergence.
