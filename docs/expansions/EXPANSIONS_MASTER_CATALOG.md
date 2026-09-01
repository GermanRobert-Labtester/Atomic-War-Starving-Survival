# ASHFALL Expansions 01–11 Master Systems & Integration Atlas

**Authoritative Expansion Catalog** | **Generated:** 2026-08-30 | **Total Expansions:** 11

> [!IMPORTANT]
> **EXPANSION INTEGRATION RULES:**
> 1. **Core Gameplay Logic**: Lives strictly in `Assets/Ashfall.Core/<Domain>/` with 0 Godot/Unity dependencies.
> 2. **Persistence Boundary**: Each expansion must persist via its own section in `campaign.json` utilizing `SaveStoreHub` or Core codecs.
> 3. **Data Feeds**: Authoritative JSON catalogs live in `Assets/StreamingAssets/Data/` with integer `schema_version`.
> 4. **Headless Verification**: Every expansion maintains at least one headless Godot CLI verification verb.

---

## Master Expansions Summary Matrix

| Exp # | Expansion Title | Domain Systems | Host Session | Save Section Key | Self-Test Verbs |
|---|---|---|---|---|---|
| **01** | The Holdfast & The Ice Road | `Ashfall.Core.IceRoad / Ashfall.Core.Shelter` | `HoldfastRuntimeSession` | `holdfast_s1` | `--holdfast-selftest, --ice-road-selftest, --ice-road-tick-demo` |
| **02** | The Duty Roster | `Ashfall.Core.DutyRoster / Ashfall.Core.Survivors` | `DutyRosterHostSession` | `duty_roster` | `--duty-roster-selftest, --duty-roster-save-selftest` |
| **03** | The Standing Record | `Ashfall.Core.StandingRecord / Ashfall.Core.Factions` | `StandingRecordHostSession` | `standing_record` | `--standing-record-selftest, --factions-selftest` |
| **04** | Nobody's Charter & The Crossing | `Ashfall.Core.Crossing / Ashfall.Core.Quests` | `CrossingHostSession` | `crossing` | `--crossing-selftest, --arbitration-selftest` |
| **05** | The Year of Ash & The Greenhouse | `Ashfall.Core.Greenhouse / Ashfall.Core.World` | `YearOfAshHostSession` | `greenhouse` | `--greenhouse-selftest, --year-of-ash-selftest` |
| **06** | The Muster | `Ashfall.Core.Muster / Ashfall.Core.Combat` | `MusterHostSession` | `muster` | `--muster-selftest` |
| **07** | The Dose | `Ashfall.Core.Medical / Ashfall.Core.Radiation` | `DoseLedgerHostSession` | `dose_ledger` | `--dose-ledger-selftest, --radiation-selftest` |
| **08** | The Verdict | `Ashfall.Core.Verdict / Ashfall.Core.Narrative` | `VerdictHostSession` | `verdict` | `--verdict-selftest` |
| **09** | The Black Flotilla | `Ashfall.Core.Flotilla / Ashfall.Core.Maritime` | `BlackFlotillaHostSession` | `black_flotilla` | `--black-flotilla-selftest` |
| **10** | The Silent Foundry | `Ashfall.Core.Foundry / Ashfall.Core.Crafting` | `SilentFoundryHostSession` | `silent_foundry` | `--silent-foundry-selftest` |
| **11** | The Long Line | `Ashfall.Core.LongLine / Ashfall.Core.Logistics` | `LongLineHostSession` | `long_line` | `--long-line-selftest` |

---

## Detailed Subsystem & Data Seams

### Expansion 01: The Holdfast & The Ice Road

- **Overview:** Sub-zero survival loop, frozen supply road convoys, clerk requisition ledger, and starter survivor cohort.
- **Core Namespace:** `Ashfall.Core.IceRoad / Ashfall.Core.Shelter`
- **Core Systems:** `IceRoadSystem, ClerkLedgerSystem, HoldfastCatalog, HoldfastTradeSession`
- **Godot Host Session:** `HoldfastRuntimeSession`
- **Campaign Save Section:** `holdfast_s1` in `campaign.json`
- **Authoritative JSON Feeds:** `ice_road_catalog.json, holdfast_quests.json, items.json`
- **Headless CLI Verbs:** `--holdfast-selftest, --ice-road-selftest, --ice-road-tick-demo`

### Expansion 02: The Duty Roster

- **Overview:** 24-hour work shift scheduling, fatigue drift, burnout risks, night-shift penalties, and critical post assignments.
- **Core Namespace:** `Ashfall.Core.DutyRoster / Ashfall.Core.Survivors`
- **Core Systems:** `DutyRosterSystem, WorkShiftSystem, FatigueAccumulator, AssignmentMatrix`
- **Godot Host Session:** `DutyRosterHostSession`
- **Campaign Save Section:** `duty_roster` in `campaign.json`
- **Authoritative JSON Feeds:** `duty_roster.json, shifts.json, survivor_traits.json`
- **Headless CLI Verbs:** `--duty-roster-selftest, --duty-roster-save-selftest`

### Expansion 03: The Standing Record

- **Overview:** Faction reputation drift, historical grievance tracking, tribute pacts, regional border tensions, and ceasefire terms.
- **Core Namespace:** `Ashfall.Core.StandingRecord / Ashfall.Core.Factions`
- **Core Systems:** `StandingRecordSystem, TrustMomentumSystem, GrievanceLedger, TreatyMatrix`
- **Godot Host Session:** `StandingRecordHostSession`
- **Campaign Save Section:** `standing_record` in `campaign.json`
- **Authoritative JSON Feeds:** `standing_record.json, factions.json, treaties.json`
- **Headless CLI Verbs:** `--standing-record-selftest, --factions-selftest`

### Expansion 04: Nobody's Charter & The Crossing

- **Overview:** Neutral river crossing checkpoint, refugee arbitration disputes, contraband confiscation, and border security.
- **Core Namespace:** `Ashfall.Core.Crossing / Ashfall.Core.Quests`
- **Core Systems:** `CrossingArbitrationSystem, NobodysCharterSystem, BorderControlMatrix`
- **Godot Host Session:** `CrossingHostSession`
- **Campaign Save Section:** `crossing` in `campaign.json`
- **Authoritative JSON Feeds:** `crossing.json, arbitrations.json, contraband.json`
- **Headless CLI Verbs:** `--crossing-selftest, --arbitration-selftest`

### Expansion 05: The Year of Ash & The Greenhouse

- **Overview:** Hydroponic crop cultivation, atmospheric ash deposition, soil decontamination, grow-light power draw, and food security.
- **Core Namespace:** `Ashfall.Core.Greenhouse / Ashfall.Core.World`
- **Core Systems:** `GreenhouseSystem, YearOfAshSystem, AshContaminationSystem, SoilDegradationSystem`
- **Godot Host Session:** `YearOfAshHostSession`
- **Campaign Save Section:** `greenhouse` in `campaign.json`
- **Authoritative JSON Feeds:** `greenhouse.json, crops.json, soil_nutrients.json`
- **Headless CLI Verbs:** `--greenhouse-selftest, --year-of-ash-selftest`

### Expansion 06: The Muster

- **Overview:** Shelter militia mobilization, automated turrets, perimeter barricade maintenance, and raider defense sieges.
- **Core Namespace:** `Ashfall.Core.Muster / Ashfall.Core.Combat`
- **Core Systems:** `MusterSystem, MilitiaRosterSystem, FortificationSystem, DefenseWaveMatrix`
- **Godot Host Session:** `MusterHostSession`
- **Campaign Save Section:** `muster` in `campaign.json`
- **Authoritative JSON Feeds:** `muster.json, defensive_structures.json, raid_doctrines.json`
- **Headless CLI Verbs:** `--muster-selftest`

### Expansion 07: The Dose

- **Overview:** Cumulative radiation dosage tracking, bone marrow damage, chelation protocols, and thyroid saturation treatments.
- **Core Namespace:** `Ashfall.Core.Medical / Ashfall.Core.Radiation`
- **Core Systems:** `DoseLedgerSystem, AcuteRadiationSicknessSystem, ChelationTherapySystem`
- **Godot Host Session:** `DoseLedgerHostSession`
- **Campaign Save Section:** `dose_ledger` in `campaign.json`
- **Authoritative JSON Feeds:** `dose_ledger.json, radiation_treatments.json, dosimeters.json`
- **Headless CLI Verbs:** `--dose-ledger-selftest, --radiation-selftest`

### Expansion 08: The Verdict

- **Overview:** Community tribunal, evidence collection, pre-war guilt investigations, exile sentencing, and social cohesion shifts.
- **Core Namespace:** `Ashfall.Core.Verdict / Ashfall.Core.Narrative`
- **Core Systems:** `VerdictSystem, ReckoningPhaseSystem, EvidenceLockerSystem, CensusAuditSystem`
- **Godot Host Session:** `VerdictHostSession`
- **Campaign Save Section:** `verdict` in `campaign.json`
- **Authoritative JSON Feeds:** `verdict.json, evidence.json, tribunal_charges.json`
- **Headless CLI Verbs:** `--verdict-selftest`

### Expansion 09: The Black Flotilla

- **Overview:** Sunken naval wreckage salvage, submarine diving suits, air supply management, underwater radiation, and marine loot.
- **Core Namespace:** `Ashfall.Core.Flotilla / Ashfall.Core.Maritime`
- **Core Systems:** `BlackFlotillaSystem, ScavengeDiveSystem, MarineContaminationSystem, VesselConditionSystem`
- **Godot Host Session:** `BlackFlotillaHostSession`
- **Campaign Save Section:** `black_flotilla` in `campaign.json`
- **Authoritative JSON Feeds:** `black_flotilla.json, maritime_salvage.json, dive_zones.json`
- **Headless CLI Verbs:** `--black-flotilla-selftest`

### Expansion 10: The Silent Foundry

- **Overview:** Geothermal subterranean forge, military-grade alloy smelting, blueprint replication, and advanced machining.
- **Core Namespace:** `Ashfall.Core.Foundry / Ashfall.Core.Crafting`
- **Core Systems:** `SilentFoundrySystem, RelicReverseEngineeringSystem, HeavyFabricationMatrix`
- **Godot Host Session:** `SilentFoundryHostSession`
- **Campaign Save Section:** `silent_foundry` in `campaign.json`
- **Authoritative JSON Feeds:** `silent_foundry.json, foundry_recipes.json, metallurgical_alloys.json`
- **Headless CLI Verbs:** `--silent-foundry-selftest`

### Expansion 11: The Long Line

- **Overview:** Trans-continental telegraph cable network, relay maintenance, long-distance signal routing, and weather interference.
- **Core Namespace:** `Ashfall.Core.LongLine / Ashfall.Core.Logistics`
- **Core Systems:** `LongLineLogisticsSystem, RelayStationSystem, LongRangeTelegraphSystem`
- **Godot Host Session:** `LongLineHostSession`
- **Campaign Save Section:** `long_line` in `campaign.json`
- **Authoritative JSON Feeds:** `long_line.json, telegraph_cables.json, logistics_routes.json`
- **Headless CLI Verbs:** `--long-line-selftest`
