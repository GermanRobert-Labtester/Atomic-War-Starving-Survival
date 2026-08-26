# Snapshot Manifest Consistency Audit — 2026-08-26

**Date:** 2026-08-26
**Scope:** `docs/ui/snapshot_manifest.json`, `docs/ui/snapshot_baseline_manifest.json`, and canonical `snapshots/*.png` corpus (29 approved targets).
**Method:** Report-only consistency audit (no snapshot regeneration).

---

## 1. Executive Summary

| Artifact | Tracked Targets | Status | Notes |
|---|---|---|---|
| `docs/ui/snapshot_manifest.json` | **29** | **MATCH** | Detailed target definitions, runtime surfaces, and classification |
| `docs/ui/snapshot_baseline_manifest.json` | **29** | **MATCH** | Fingerprint registry (MD5, file size, phase origin) |
| `snapshots/*.png` (non-gallery) | **29** | **MATCH** | Approved visual baselines committed on disk |

All 29 approved snapshot targets are fully consistent across both manifest files and physical disk assets.

---

## 2. 29 Approved Targets Inventory Table

| # | Snapshot Target ID | Runtime Surface | Phase | Disk Size (Bytes) | Status |
|---|---|---|---|---|---|
| 1 | `caravan_barter_default` | `src/Economy/CaravanBarterLedgerPanel.cs` | 12 | 211,018 | **MATCH** |
| 2 | `combat_hud_default` | `src/Combat/CombatHudOverlay.cs` | 22 | 47,434 | **MATCH** |
| 3 | `dose_ledger_default` | `src/UI/DoseLedgerPanel.cs` | 13 | 45,680 | **MATCH** |
| 4 | `duty_roster_default` | `src/UI/DutyRosterPanel.cs` | 20 | 63,434 | **MATCH** |
| 5 | `expedition_radar_default` | `src/UI/ExpeditionRadarPanel.cs` | 17 | 61,836 | **MATCH** |
| 6 | `faction_matrix_default` | `src/UI/FactionMatrixPanel.cs` | 13 | 50,230 | **MATCH** |
| 7 | `factions_narrative_default` | `src/UI/FactionsNarrativePanel.cs` | 21 | 65,936 | **MATCH** |
| 8 | `greenhouse_default` | `src/UI/GreenhousePanel.cs` | 15 | 51,405 | **MATCH** |
| 9 | `inventory_default` | `src/UI/InventoryPanel.cs` | 11 | 52,339 | **MATCH** |
| 10 | `journal_default` | `src/UI/JournalPanel.cs` | 11 | 32,169 | **MATCH** |
| 11 | `map_atlas_default` | `src/UI/MapAtlasPanel.cs` | 23 | 38,887 | **MATCH** |
| 12 | `maritime_atlas_default` | `src/UI/MaritimeAtlasPanel.cs` | 24 | 52,947 | **MATCH** |
| 13 | `medical_default` | `src/UI/MedicalPanel.cs` | 11 | 40,454 | **MATCH** |
| 14 | `muster_atlas_default` | `src/UI/MusterAtlasPanel.cs` | 25 | 76,350 | **MATCH** |
| 15 | `quests_atlas_default` | `src/UI/QuestsAtlasPanel.cs` | 26 | 55,189 | **MATCH** |
| 16 | `radio_default` | `src/UI/RadioPanel.cs` | 11 | 55,164 | **MATCH** |
| 17 | `research_atlas_default` | `src/UI/ResearchAtlasPanel.cs` | 28 | 74,573 | **MATCH** |
| 18 | `shelter_default` | `src/UI/ShelterPanel.cs` | 11 | 29,196 | **MATCH** |
| 19 | `shelter_hud_default` | `src/UI/ShelterHudPanel.cs` | 12 | 98,432 | **MATCH** |
| 20 | `silent_foundry_default` | `src/UI/SilentFoundryPanel.cs` | 16 | 64,780 | **MATCH** |
| 21 | `skill_matrix_default` | `src/UI/SkillMatrixPanel.cs` | 19 | 102,079 | **MATCH** |
| 22 | `standing_record_atlas_default` | `src/UI/StandingRecordAtlasPanel.cs` | 27 | 78,265 | **MATCH** |
| 23 | `survival_workstation_default` | `src/UI/SurvivalWorkstationPanel.cs` | 12 | 78,780 | **MATCH** |
| 24 | `survivors_default` | `src/UI/SurvivorsPanel.cs` | 11 | 31,174 | **MATCH** |
| 25 | `trade_default` | `src/UI/TradeScreenGodotPanel.cs` | 11 | 209,063 | **MATCH** |
| 26 | `verdict_dashboard_default` | `src/UI/VerdictDashboardPanel.cs` | 13 | 27,417 | **MATCH** |
| 27 | `verdict_default` | `src/UI/VerdictPanel.cs` | 11 | 23,654 | **MATCH** |
| 28 | `weather_dashboard_default` | `src/UI/WeatherDashboardPanel.cs` | 13 | 33,064 | **MATCH** |
| 29 | `weather_default` | `src/UI/WeatherPanel.cs` | 11 | 32,378 | **MATCH** |

---

## 3. Discrepancies Checked and Reconciled

1. **`quests_atlas_default` Target Alignment**:
   - `snapshot_manifest.json` previously listed 28 target objects despite stating `totals.targets: 29`.
   - Added the Phase 26 `quests_atlas_default` target definition to `snapshot_manifest.json`.
2. **Classification Array Hygiene**:
   - Deduplicated duplicate `greenhouse_default` entries in `snapshot_manifest.json.classification.BASELINE`.
   - Added missing Tier-3 atlas entries (`quests_atlas_default`, `standing_record_atlas_default`, `research_atlas_default`) to the `BASELINE` classification list.
3. **No Snapshot Regeneration**:
   - Verified that no PNG regeneration was executed, adhering strictly to the report-only constraint.
