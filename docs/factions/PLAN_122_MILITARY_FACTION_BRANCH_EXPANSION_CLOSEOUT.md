# Plan 122 — Military Faction Branch Expansion (8 → 15) — Completion Report

> **Expansion Mission:** Expand `military_faction_branch.json` from 8 verified baseline branches to 15 total point-of-no-return character arcs (+7 newly authored branches, +21 endings), providing rich military moral archetypes during the Year of Ash campaign.
> **Completion Date:** 2026-09-08
> **Completion Mode:** **COMPLETE — DATA + MINOR ID REGISTRATION**
> **Status:** All tests, selftests, and CI gates PASS.

---

## 1. Executive Summary

Plan 122 expands the Military faction character-ending layer from **8 branches (24 endings) to 15 branches (45 endings)**. The expansion equips military-affiliated survivors with distinct late-campaign trajectories spanning logistics ethics, battlefield care, conscription vs family loyalty, intelligence suppression, nonviolent crowd restraint, shelter sanctuary, and command insubordination.

### 1.1 Completion Mode & Architectural Truthfulness
In accordance with Sections 2 and 4 of the implementation plan:
- `MilitaryBranchIds.cs` acts as an authoritative lookup and registration contract: `MilitaryBranchSystem.LockPointOfNoReturn()` invokes `MilitaryBranchIds.PonrFlagFor(branchId)`, which throws an exception if the branch ID is not in its switch expression.
- `MilitaryBranchIds.BranchCount` and `MilitaryBranchIds.AllBranches` are asserted by existing test suites.
- Therefore, this expansion was executed under **DATA + MINOR ID REGISTRATION**:
  - `Assets/StreamingAssets/Data/military_faction_branch.json` expanded with 7 new branches (21 endings).
  - `builds/linux/Assets/StreamingAssets/Data/military_faction_branch.json` mirrored with exact parity.
  - `Assets/Ashfall.Core/Factions/MilitaryBranchIds.cs` updated with constants, `AllBranches` registration, and `PonrFlagFor` switch arms for branches 9..15.
  - Zero new gameplay execution logic or engine dependencies were introduced into Core.

---

## 2. Seven Authored Branches Summary

1. **`branch_mil_9_quartermaster` (The Quartermaster):**
   - **Role:** Logistics officer controlling military depot stores.
   - **PoNR Flag:** `flag_branch_mil_9_ponr`
   - **Trigger:** *"They falsify military store manifests to divert food and medical crates to starving civilians, making the shortage irreversible."*
   - **Entry Band:** `very_evil` .. `very_positive`
   - **Endings (3):**
     - `ending_mil_9a_keeper_of_stores` (`positive`..`very_positive`, "The Keeper of Stores")
     - `ending_mil_9b_civilian_quartermaster` (`slightly_evil`..`slightly_positive`, "The Civilian Quartermaster")
     - `ending_mil_9c_black_ledger` (`very_evil`..`evil`, "The Black Ledger")

2. **`branch_mil_10_combat_medic` (The Combat Medic):**
   - **Role:** Military medic balancing patient obligation against combat command.
   - **PoNR Flag:** `flag_branch_mil_10_ponr`
   - **Trigger:** *"They lower their weapon in front of their unit, claiming the wounded enemy as a patient instead of an operational target."*
   - **Entry Band:** `slightly_evil` .. `very_positive`
   - **Endings (3):**
     - `ending_mil_10a_field_healer` (`positive`..`very_positive`, "The Field Healer")
     - `ending_mil_10b_unit_outcast` (`slightly_evil`..`slightly_positive`, "The Unit Outcast")
     - `ending_mil_10c_hardened_triage` (`very_evil`..`evil`, "The Hardened Triage")

3. **`branch_mil_11_conscript_parent` (The Conscript Parent):**
   - **Role:** Conscripted soldier whose family resides in the shelter.
   - **PoNR Flag:** `flag_branch_mil_11_ponr`
   - **Trigger:** *"They refuse a punitive sweep order against their home settlement, identifying themselves openly as a defender of their own household."*
   - **Entry Band:** `evil` .. `very_positive`
   - **Endings (3):**
     - `ending_mil_11a_community_protector` (`positive`..`very_positive`, "The Community Protector")
     - `ending_mil_11b_divided_household` (`slightly_evil`..`slightly_positive`, "The Divided Household")
     - `ending_mil_11c_permanent_fugitive` (`very_evil`..`evil`, "The Permanent Fugitive")

4. **`branch_mil_12_intelligence_officer` (The Intelligence Officer):**
   - **Role:** Analyst handling intelligence reports, informant rolls, and targeting dossiers.
   - **PoNR Flag:** `flag_branch_mil_12_ponr`
   - **Trigger:** *"They burn a verified civilian informant register and purge the archive before command can dispatch retaliatory death squads."*
   - **Entry Band:** `very_evil` .. `very_positive`
   - **Endings (3):**
     - `ending_mil_12a_public_witness` (`positive`..`very_positive`, "The Public Witness")
     - `ending_mil_12b_trusted_analyst` (`slightly_evil`..`slightly_positive`, "The Trusted Analyst")
     - `ending_mil_12c_silenced_liability` (`very_evil`..`evil`, "The Silenced Liability")

5. **`branch_mil_13_peacekeeper` (The Peacekeeper):**
   - **Role:** Sentry assigned to crowd control and security enforcement.
   - **PoNR Flag:** `flag_branch_mil_13_ponr`
   - **Trigger:** *"They step between a firing squad and an unarmed crowd of protesters, locking their weapon's safety and barring the advance."*
   - **Entry Band:** `neutral` .. `very_positive`
   - **Endings (3):**
     - `ending_mil_13a_community_shield` (`positive`..`very_positive`, "The Community Shield")
     - `ending_mil_13b_disarmed_mediator` (`slightly_evil`..`slightly_positive`, "The Disarmed Mediator")
     - `ending_mil_13c_overrun` (`very_evil`..`evil`, "The Overrun Sentinel")

6. **`branch_mil_14_fugitive_deserter` (The Fugitive Deserter):**
   - **Role:** Line soldier choosing civilian shelter life over military recall.
   - **PoNR Flag:** `flag_branch_mil_14_ponr`
   - **Trigger:** *"They ignore the emergency muster klaxon, burn their uniform in the barracks stove, and disappear into the civilian crowd."*
   - **Entry Band:** `evil` .. `positive`
   - **Endings (3):**
     - `ending_mil_14a_integrated_civilian` (`positive`..`very_positive`, "The Integrated Civilian")
     - `ending_mil_14b_hunted_fugitive` (`slightly_evil`..`slightly_positive`, "The Hunted Fugitive")
     - `ending_mil_14c_armed_defector` (`very_evil`..`evil`, "The Armed Defector")

7. **`branch_mil_15_dissident_officer` (The Dissident Officer):**
   - **Role:** Operational commander challenging orders in front of troops.
   - **PoNR Flag:** `flag_branch_mil_15_ponr`
   - **Trigger:** *"They issue a written counter-order halting an artillery bombardment of refugee encampments, countersigning it before the battalion staff."*
   - **Entry Band:** `neutral` .. `very_positive`
   - **Endings (3):**
     - `ending_mil_15a_reform_command` (`positive`..`very_positive`, "The Reform Commander")
     - `ending_mil_15b_broken_officer` (`slightly_evil`..`slightly_positive`, "The Broken Officer")
     - `ending_mil_15c_mutineer` (`very_evil`..`evil`, "The Mutineer")

---

## 3. Cross-Plan Integration & Downstream Consumers

1. **Plan 114 (Year of Ash Questlines):**
   Crisis decision points in Year of Ash trigger the irreversible PoNR flags (`flag_branch_mil_9_ponr` through `flag_branch_mil_15_ponr`).
2. **Plan 89 (Epilogues):**
   All 21 new ending IDs (`ending_mil_9a` through `ending_mil_15c`) follow standard snake_case naming and are ready for epilogue matrix mapping.
3. **Plan 125 (Moral Choice Flags):**
   PoNR flags follow the canonical `flag_branch_mil_<n>_ponr` pattern, integrating cleanly into the persistent flag ledger.
4. **Plan 98 (Standing Record):**
   Branch commitment and ending resolution remain independent of raw standing math, which is modified externally by quest outcomes.

---

## 4. Verification Evidence Matrix

| Test Suite / Gate | Result | Notes |
|---|:---:|---|
| `MilitaryBranchCatalogTests` | **PASS** (4/4) | Verifies all 15 branches exist, match IDs, and cross-reference cleanly. |
| `MilitaryBranchSystemTests` | **PASS** (18/18) | Verifies commitment, PoNR locking, faction alignment, and ending resolution. |
| `MilitaryBranchExpansionTests` | **PASS** (21/21) | Dedicated suite testing baseline preservation, new IDs, band partitions, and saves. |
| `FactionBranchCoordinatorTests` | **PASS** (17/17) | Verifies 38 total faction branches (15 Mil + 8 Reb + 15 Ind). |
| `dotnet test Ashfall.Core.Tests` | **PASS** | Full suite execution: **9990 passed, 0 failed, 0 skipped**. |
| `godot --data-integrity-selftest` | **PASS** | 0 errors across 298 catalogs. |
| `godot --content-utilization-selftest` | **PASS** | CI gate PASS. |
| `python3 scripts/ci/scene-lint.py` | **PASS** | 30 production scenes, 0 errors, 0 warnings. |
| `dotnet build Ashfall.csproj` | **PASS** | 0 compilation errors. |
