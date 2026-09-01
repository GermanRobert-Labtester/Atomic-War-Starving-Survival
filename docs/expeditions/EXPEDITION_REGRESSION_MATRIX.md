# Expedition Regression Matrix

## 1. Regression Scenarios & Verifications

| Scenario # | Description | Target / Input | Expected Behavior | Status |
| :--- | :--- | :--- | :--- | :--- |
| **01** | Existing Destination 1 | `loc_the_allotments` | Retains exact original ID, distance (5), danger (2), loot lines | **PASS** |
| **02** | Existing Destination 2 | `loc_denial_cut_substation` | Retains exact original ID, distance (8), danger (4), loot lines | **PASS** |
| **03** | Scavenge Tier Dispatch | `suburban_house` | 2-tick travel, looting entry, auto-retreat, return | **PASS** |
| **04** | Standard Tier Dispatch | `checkpoint_kilo_armory` | 6-tick travel, stamina drain (2.5/hr), ammo/MRE loot | **PASS** |
| **05** | Hazardous Tier Dispatch | `abandoned_hospital` | 4-tick travel, danger 6 encounter scaling, med supplies | **PASS** |
| **06** | Deep Tier Dispatch | `government_bunker` | 8-tick travel, danger 8 encounters, high-tier loot | **PASS** |
| **07** | Extreme Distance Dispatch | `location_the_dead_hand_core` | 18-tick travel, danger 10 rolls, rad-away/anti-rad loot | **PASS** |
| **08** | Loot Thematic Identity | `hospital_pharmacy` | Loot yields antibiotics/medical_kit/anti_rad/iodine_pills | **PASS** |
| **09** | Mid-Expedition Save/Load | `location_arcology_sector_4` | State captures/restores exact locationId and travelTicks | **PASS** |
| **10** | Vehicle Sortie Integration | Quad / Van on Deep routes | Speed multipliers & breakdown chances evaluate cleanly | **PASS** |
| **11** | Catalog ID Uniqueness | All 50 records | 0 duplicate IDs, 50/50 resolve in `locations.json` | **PASS** |
| **12** | Numeric Bounds Gate | All 50 records | Distances $\ge 1$, Danger $1..10$, Enc $0.05..0.50$, Drain $1..5$ | **PASS** |
| **13** | Loot Categories Gate | All 50 records | 0 invalid items, all items exist in `items.json` | **PASS** |
| **14** | Headless Demo Gate | `--expedition-selftest` | 10/10 Core demo tests + 9/9 vehicle gates pass | **PASS** |
| **15** | Data Integrity Gate | `--data-integrity-selftest` | 0 findings across all catalogs | **PASS** |
| **16** | Full Unit Test Suite | `dotnet test Ashfall.Core.Tests` | 5,750+ tests pass with 0 failures | **PASS** |

## 2. Invariant Compliance Confirmation

- **Invariant 1 (Engine Agnostic Core):** Zero `UnityEngine` or `Godot` dependencies in Core models and loader.
- **Invariant 2 (Ports & Adapters):** Standardized on `IFileIO` and `IJsonSerializer`.
- **Invariant 3 (Save Compatibility):** Zero save schema changes; persistence remains string-ID based.
- **Invariant 4 (Determinism):** Seeded PRNG (`ISeededRng`) for all encounter and loot rolls.
- **Invariant 5 (No Gameplay Logic in Hosts):** Host is a thin presentation and garage dispatch wrapper.
- **Invariant 6 (Data Authority is JSON):** `locations.json` and `expeditions.json` are authoritative.
