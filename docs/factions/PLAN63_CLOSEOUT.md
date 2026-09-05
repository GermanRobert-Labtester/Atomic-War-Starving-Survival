# Plan 63 — Warlord Doctrines Expansion (8 → 24 Strategic Profiles) Closeout Report

## 1. Executive Summary

Plan 63 expands `warlord_doctrines.json` from the baseline of 8 doctrines to **24 distinct, authored strategic profiles**, establishing rich, recognizable behavioral variety across warlord-aligned forces in the ASHFALL wasteland.

Each doctrine defines a complete, bounded strategic profile:
- Distinct risk tolerance ($0.15$ to $0.85$).
- Explicit preferred goals and action weighting across all legal strategic actions (`demand_tribute`, `raid`, `defend`, `contest`, `annex`, `withdraw`).
- Authored resource priorities referencing canonical item definitions in `items.json`.
- Specific target selection rules (`nearest_undefended`, `highest_supply`, `nearest_chokepoint`, `isolated_convoy`, `none`).
- Authored journal and radio broadcast reference keys (`jrnl_warlord_*` / `journal_warlord_*` and `radio_warlord_*`), validated with 0 errors against `CatalogIntegrityValidator` Tier-1.
- Complete state transition networks responding to dynamic signals (`supply_ratio`, `failure_streak`, `success_streak`, `contested_count`, `player_tribute_reliability`, `environment_hazard`, `rival_pressure`).

---

## 2. Complete Doctrine Manifest (24 Profiles)

### Original 8 Preserved Doctrines
| # | Doctrine ID | Display Name | Archetype | Risk | Preferred Goal | Target Rule |
|---|---|---|---|---|---|---|
| 1 | `warlord_doctrine_toll` | The Toll | Extortionist | 0.60 | tribute | nearest_undefended |
| 2 | `warlord_doctrine_consolidation` | Holding the Line | Fortifier | 0.30 | stability | nearest_undefended |
| 3 | `warlord_doctrine_annexation` | The Long Reach | Expansionist | 0.80 | expansion | highest_supply |
| 4 | `warlord_doctrine_withdrawal` | Gone to Ground | Evasive | 0.15 | preservation | none |
| 5 | `warlord_doctrine_besiege` | The Cold Siege | Siege Specialist | 0.35 | patience | nearest_chokepoint |
| 6 | `warlord_doctrine_traffic` | The Slave Ledger | Slaver / Coercive Labor | 0.55 | apprehension | isolated_convoy |
| 7 | `warlord_doctrine_ashprophet` | The Ash Cant | Prophet Zealot | 0.70 | conversion | nearest_undefended |
| 8 | `warlord_doctrine_procedure` | The Pincer Manual | Military Disciplinarian | 0.45 | discipline | highest_supply |

### 16 New Authored Strategic Doctrines
| # | Doctrine ID | Display Name | Archetype | Risk | Preferred Goal | Key Actions | Primary Resources | Target Rule |
|---|---|---|---|---|---|---|---|---|
| 9 | `warlord_doctrine_lightning_raider` | Lightning Strike | Raider (Fast) | 0.85 | loot | raid (5), contest (2), withdraw (1) | `fuel`, `ammo_556` | nearest_undefended |
| 10 | `warlord_doctrine_vulture_raider` | The Scavenger's Talon | Raider (Scavenger) | 0.35 | scavenge | raid (4), demand_tribute (2), withdraw (2) | `scrap_metal`, `canned_food` | nearest_undefended |
| 11 | `warlord_doctrine_iron_perimeter` | The Iron Perimeter | Fortifier (Border) | 0.25 | defense | defend (5), contest (2), demand_tribute (1) | `ammo_556`, `scrap_metal` | nearest_chokepoint |
| 12 | `warlord_doctrine_layered_redoubt` | Layered Redoubts | Fortifier (Nested) | 0.20 | attrition | defend (5), withdraw (3), contest (1) | `canned_food`, `clean_water` | nearest_chokepoint |
| 13 | `warlord_doctrine_pressed_ranks` | Pressed Ranks | Recruiter | 0.65 | manpower | raid (4), contest (3), demand_tribute (2), defend (1) | `canned_food`, `bandage` | nearest_undefended |
| 14 | `warlord_doctrine_borrowed_voices` | Borrowed Voices | Infiltrator | 0.50 | subversion | contest (4), raid (2), demand_tribute (2), withdraw (1) | `ammo_556`, `fuel` | highest_supply |
| 15 | `warlord_doctrine_toll_kingdom` | The Toll Kingdom | Extortionist (Heavy) | 0.55 | tribute | demand_tribute (5), defend (3), raid (2), contest (1) | `canned_food`, `clean_water` | nearest_chokepoint |
| 16 | `warlord_doctrine_sacred_campaign` | The Sacred Crusade | Prophet Warlord | 0.85 | conversion | annex (4), contest (4), raid (2), defend (1) | `iodine_pills`, `clean_water` | highest_supply |
| 17 | `warlord_doctrine_salvage_supremacy` | Salvage Supremacy | Technologist | 0.60 | technology | contest (4), defend (3), annex (2), raid (2) | `fuel`, `scrap_metal` | highest_supply |
| 18 | `warlord_doctrine_chains_of_work` | Chains of Labor | Coercive Labor | 0.60 | labor | raid (5), contest (3), demand_tribute (1), defend (1) | `canned_food`, `clean_water` | isolated_convoy |
| 19 | `warlord_doctrine_many_knives` | Council of Many Knives | Warlord Council | 0.50 | coalition | contest (3), demand_tribute (3), raid (2), defend (2) | `ammo_556`, `canned_food` | nearest_undefended |
| 20 | `warlord_doctrine_scorched_earth` | Scorched Earth | Denial | 0.75 | denial | contest (4), withdraw (3), raid (2) | `fuel`, `ammo_556` | highest_supply |
| 21 | `warlord_doctrine_convoy_interdiction` | Road Interdiction | Interdictor | 0.55 | chokepoint | raid (4), demand_tribute (3), contest (2) | `fuel`, `canned_food` | isolated_convoy |
| 22 | `warlord_doctrine_silent_garrison` | The Silent Garrison | Ambush Fortifier | 0.30 | ambush | defend (5), contest (2), demand_tribute (1) | `ammo_556`, `scrap_metal` | nearest_chokepoint |
| 23 | `warlord_doctrine_proxy_provocation` | Shadow Proxies | Proxy Aggressor | 0.40 | subterfuge | contest (4), raid (3), demand_tribute (1) | `clean_water`, `ammo_556` | nearest_undefended |
| 24 | `warlord_doctrine_resource_stranglehold` | The Cistern Clamp | Resource Monopolist | 0.50 | stranglehold | annex (4), defend (3), contest (2), demand_tribute (2) | `clean_water`, `iodine_pills` | highest_supply |

---

## 3. Verification & Compliance Evidence

- **`WarlordCatalogValidator.Validate`:** Clean (0 errors, alias warnings preserved).
- **`CatalogIntegrityValidator`:**
  ```
  DATA_INTEGRITY_SELFTEST PASS — 0 findings (10575 ids authored, 3489 reuses reserved) — 0 errors, 0 warnings across 208 catalogs
  ```
- **`ContentUtilizationScanner`:**
  ```
  CI Content Utilization Gate: PASS (0 orphaned, 0 unparsed)
  ```
- **`SceneBindingSelfTest`:** 22/22 scenes passed.
- **`SceneLint`:** 27 production scenes checked; 0 errors; 0 warnings.
- **`WarlordPlan63DoctrineTests`:** 6/6 tests passed including:
  - 24 distinct doctrine verification.
  - Original 8 preservation test.
  - 16 new profile structural completeness test.
  - Action weights and transition network validity test.
  - Seeded multi-day replay simulation determinism test.
