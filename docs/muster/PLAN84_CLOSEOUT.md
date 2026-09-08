# Plan 84 — Muster Witness Testimonies Expansion Closeout Report

> **Theme:** Muster Witness Testimonies Expansion: 3 → 15 Investigation Witnesses (27 total in catalog)
> **Authority:** `Assets/StreamingAssets/Data/muster_witnesses.json` (and `builds/linux/Assets/StreamingAssets/Data/muster_witnesses.json`)
> **Host / Engine:** `Assets/Ashfall.Core/Muster/WitnessCatalog.cs`
> **Test Suite:** `Ashfall.Core.Tests/Plan84WitnessExpansionTests.cs`
> **Status:** **COMPLETE** (100% Verified across full CI matrix)

---

## 1. Executive Summary

Plan 84 expands `muster_witnesses.json` by adding **12 new investigation witnesses** across three distinct investigation threads (4 witnesses each), evolving the Muster witness system from a single Voss-disappearance thread into an intricate, multi-thread testimony network.

### Roster Architecture
- **Voss Disappearance Thread (3 Founding Witnesses):** Preserved 100% intact (`witness_1_checkpoint_conscript`, `witness_2_quartermaster_paperwork`, `witness_3_signals_intercept`).
- **Plan 25 Faction Ecology Witnesses (12 Witnesses):** Preserved 100% intact (`witness_scavenger_claimant`, `witness_messengers_keeper`, `witness_claimant_auditor`, etc.).
- **Plan 84 Investigation Witnesses (12 Witnesses):**
  1. *Coastal Evacuation* (4 witnesses: Harbor Master, Trawler Captain, Refugee Nurse, Naval Conscript)
  2. *Grain Convoy Massacre* (4 witnesses: Convoy Driver, Rebuilder Medic, Garrison Picket, Wayside Mechanic)
  3. *Silent Foundry Accord* (4 witnesses: Foundry Molder, Ice-Road Hauler, Arbitration Clerk, Terrace Elder)
- **Total Witness Catalog Count:** **27 entries** (schema version 2).

---

## 2. Plan 84 New Investigation Witnesses Roster

| # | Witness ID | Name | Location ID | Knowledge Key | Day Min | Priority | Thread |
|---|---|---|---|---|---|---|---|
| 1 | `witness_harbor_master_kell` | Harbor Master Kell | `loc_abandoned_tide_gauge` | `history_evacuation_harbor_clearance` | 242 | 40 | Coastal Evacuation |
| 2 | `witness_trawler_captain_maren` | Trawler Captain Maren | `loc_maritime_icebreaker_dock` | `history_evacuation_trawler_sighting` | 244 | 40 | Coastal Evacuation |
| 3 | `witness_coastal_refugee_nurse` | Refugee Nurse Maeve | `loc_st_brigids_almshouse` | `history_evacuation_triage_intake` | 246 | 40 | Coastal Evacuation |
| 4 | `witness_naval_conscript_brant` | Naval Conscript Brant | `loc_clifftop_observation_bunker` | `history_evacuation_conscript_orders` | 248 | 40 | Coastal Evacuation |
| 5 | `witness_convoy_driver_tomas` | Convoy Driver Tomas | `loc_grain_silo` | `history_grain_convoy_route_deviation` | 243 | 40 | Grain Convoy Massacre |
| 6 | `witness_rebuilder_field_medic` | Rebuilder Field Medic Kora | `loc_denial_cut_substation` | `history_grain_convoy_casualty_patterns` | 245 | 40 | Grain Convoy Massacre |
| 7 | `witness_garrison_picket_vaughn` | Garrison Picket Vaughn | `loc_border_checkpoint_ruins` | `history_grain_convoy_warning_hail` | 247 | 40 | Grain Convoy Massacre |
| 8 | `witness_wayside_mechanic_yorin` | Wayside Mechanic Yorin | `loc_weighbridge` | `history_grain_convoy_flank_muzzle_flash` | 249 | 40 | Grain Convoy Massacre |
| 9 | `witness_foundry_molder_hask` | Foundry Molder Hask | `loc_vulcan_works_chemical` | `history_foundry_pressure_drop` | 244 | 40 | Silent Foundry Accord |
| 10 | `witness_iceroad_hauler_sula` | Ice-Road Hauler Sula | `loc_north_freight_yard` | `history_foundry_checkpoint_diverted_seals` | 246 | 40 | Silent Foundry Accord |
| 11 | `witness_arbitration_clerk_moran` | Arbitration Clerk Moran | `loc_conscription_office` | `history_foundry_unregistered_amendment` | 248 | 40 | Silent Foundry Accord |
| 12 | `witness_terrace_elder_marit` | Terrace Elder Marit | `loc_river_gauging_station` | `history_foundry_unrecorded_verbal_clause` | 250 | 40 | Silent Foundry Accord |

---

## 3. Thematic Strands & Dialectics

### Thread 1: The Coastal Evacuation
- **Harbor Master Kell** (`loc_abandoned_tide_gauge`, Day 242): Claims three heavy barges cleared the outer breakwater before the radar mast buckled, but notes manifests were signed in purple ink by quartermasters who never boarded and holds were empty.
- **Trawler Captain Maren** (`loc_maritime_icebreaker_dock`, Day 244): Contradicts Kell; saw two transport hulls listed on the north reef burned down from internal engine room scuttling charges, with oar-less lifeboats drifting into the brine shoals.
- **Refugee Nurse Maeve** (`loc_st_brigids_almshouse`, Day 246): Corroborates scuttling; treated 22 naval ratings with internal steam blister burns (not combat shrapnel) whose officer bribed the staff with penicillin to burn the intake register.
- **Naval Conscript Brant** (`loc_clifftop_observation_bunker`, Day 248): Reveals teleprinter targeting coordinates were aimed inward at the harbor mouth to fire on their own transports; sergeant took the firing keys and abandoned his post into the pines.

### Thread 2: The Grain Convoy Massacre
- **Convoy Driver Tomas** (`loc_grain_silo`, Day 243): Testifies the lead truck hit a dropped barricade and was shredded by heavy machine guns without warning hail, despite green Rebuilder tarps tied down in plain view.
- **Rebuilder Field Medic Kora** (`loc_denial_cut_substation`, Day 245): Complicates the garrison blame; extracted projectiles from survivors were hunting carbine lead balls and scavenged copper-jacketed hand loads, not military service carbines.
- **Garrison Picket Vaughn** (`loc_border_checkpoint_ruins`, Day 247): Defends the garrison; lead truck accelerated through tripwires and sprayed submachine gun fire into the sandbags before garrison gunners touched spade grips in sleet and darkness.
- **Wayside Mechanic Yorin** (`loc_weighbridge`, Day 249): Reveals third-party provocation; saw muzzle flashes from the scrub oaks on the north ridge shoot out the checkpoint dynamo, killing lights and triggering blind reciprocal slaughter.

### Thread 3: The Silent Foundry Accord
- **Foundry Molder Hask** (`loc_vulcan_works_chemical`, Day 244): Brine pressure dropped before shift end; crawled culvert and discovered the supply gate padlocked from the foundry side with fresh red lead.
- **Ice-Road Hauler Sula** (`loc_north_freight_yard`, Day 246): Chemical carboys never stopped leaving the freight depot; they were diverted west under emergency agricultural redistricting into private bunker complexes.
- **Arbitration Clerk Moran** (`loc_conscription_office`, Day 248): An uncataloged administrative rider was stapled to the docket forty-eight hours prior to ratification, forcing chemical quotas and labor distraint that broke the accord.
- **Terrace Elder Marit** (`loc_river_gauging_station`, Day 250): Foundry took thirty thousand barrels of pure spring runoff, sent back corrosive mineral sediment that froze valves, then trained rifles on the terrace delegation when they brought damaged gaskets to prove it.

---

## 4. Integration Anchors with Plan 82 Verdict Sites

Three of the new investigation witnesses are stationed directly at Plan 82 Verdict investigation sites:
1. `witness_harbor_master_kell` → `loc_abandoned_tide_gauge` (Verdict Site 1: Abandoned Tide Gauge)
2. `witness_garrison_picket_vaughn` → `loc_border_checkpoint_ruins` (Verdict Site 2: Border Checkpoint Ruins)
3. `witness_terrace_elder_marit` → `loc_river_gauging_station` (Verdict Site 3: River Gauging Station)

---

## 5. Verification Matrix Results

| Verification Gate | Command | Result | Details |
|---|---|---|---|
| **Unit Test Suite** | `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **PASS** (Exit 0) | **9,711 passed**, 0 failed, 0 skipped across entire suite |
| **Plan 84 Specific** | `dotnet test --filter "FullyQualifiedName~Plan84WitnessExpansionTests"` | **PASS** (Exit 0) | **27/27 passed** |
| **Godot Host Build** | `dotnet build Ashfall.csproj` | **PASS** (Exit 0) | 0 errors, 0 warnings |
| **Data Integrity** | `godot --headless --path . -- --data-integrity-selftest` | **PASS** (Exit 0) | 0 errors, 0 warnings across 298 catalogs |
| **Content Utilization** | `godot --headless --path . -- --content-utilization-selftest` | **PASS** (Exit 0) | CI Content Utilization Gate: PASS |
| **Scene Bindings** | `godot --headless --path . -- --scene-binding-selftest` | **PASS** (Exit 0) | 25 passed, 0 failed |
| **Scene Lint** | `python3 scripts/ci/scene-lint.py` | **PASS** (Exit 0) | 30 production scenes checked; 0 errors, 0 warnings |
