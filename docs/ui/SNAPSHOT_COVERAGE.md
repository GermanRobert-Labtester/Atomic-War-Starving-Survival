# Snapshot Coverage — Post-Audit State

**Generated:** Phase 26 close (2026-08-18). Refreshed after `SURFACE_GAP_REPORT.md` audit; reconciled post-Phase 28.

This file lists every **player-facing runtime UI surface** the audit admits as
meaningful — drill-down, brief-modal, and dev-only surfaces are not tracked.
For each surface, the table reports the snapshot target, six data-state flags
(`default`, `populated`, `selected`, `warning`, `error`, plus a category
column removed in audit reduction), the Stitch reference, and a final
classification (`COVERED` / `PARTIAL` / `REGRESSION_ONLY` / `MISSING`).

For full audit reasoning, see
[`SURFACE_GAP_REPORT.md`](SURFACE_GAP_REPORT.md).

---

## State header

```
COVERED:           27 surfaces (30 targets)   ← +1 Plan 12C (Shelter Interior)   ← +1 Phase 28 (Research)
PARTIAL:            1 surface  (TradeScreen, INTENTIONAL_CHILD)
REGRESSION_ONLY:    1 surface  (CraftingPanel drill-down)
MISSING:            0 surfaces  (ALL SURFACES COVERED)
DELETED:           17 surfaces (drill-downs, brief modals, dev-only)
Total tracked:     29 player-facing runtime surfaces (ALL COVERED)
```

**Snapshot fixtures on disk:** 30 distinct byte-distinct target PNGs (+ 5 gallery baselines = 35 total),
0 duplicate MD5 groups, 0 blank captures — verified by direct RGBA8 pixel-decode check
after the Phase 26 SubViewport pipeline fix.

---

## State-class legend

| Bucket | Meaning |
|---|---|
| `COVERED` | A snapshot target exists and is approved (`MATCH` or `DESIGN_INTENT`). |
| `PARTIAL` | Snapshot covers a focused child variant of a parent surface. Sub-card sibling would double-wrap. |
| `REGRESSION_ONLY` | Drill-down of an already-covered parent; not targeted standalone. |
| `MISSING` | No snapshot target — pending Core engine / host adapter / data sidecar. |

---

## Surface table

| Runtime surface | Snapshot target(s) | default | Stitch ref | Coverage |
|---|---|:---:|---|---|
| `MedicalPanel` | `medical_default` | ✔ | `#9` | COVERED |
| `ShelterPanel` | `shelter_default` | ✔ | `#11` | COVERED |
| `JournalPanel` | `journal_default` | ✔ | `#55` | COVERED |
| `InventoryPanel` | `inventory_default` | ✔ | `#19` (storage half) | COVERED (light) |
| `SurvivorsPanel` | `survivors_default` | ✔ | `#22` (roster half) | COVERED (light); Skill Matrix DEFER to roster host adapter |
| `RadioPanel` | `radio_default` | ✔ | `#31` | COVERED |
| `WeatherPanel` | `weather_default`, `weather_dashboard_default` | ✔ | `#24` | COVERED (legacy + dashboard pair) |
| `VerdictPanel` (legacy), `VerdictDashboardPanel` (Phase 13) | `verdict_default`, `verdict_dashboard_default` | ✔ | `#15` | COVERED (dashboard variant); legacy unbroken |
| `TradeScreenGodotPanel` | `trade_default` | ✔ | `#35` | PARTIAL — INTENTIONAL_CHILD of CaravanBarterLedgerPanel |
| `CaravanBarterLedgerPanel` | `caravan_barter_default` | ✔ | `#35` | COVERED |
| `SurvivalWorkstationPanel` | `survival_workstation_default` | ✔ | `#19` | COVERED |
| `ShelterHudPanel` | `shelter_hud_default` | ✔ | `#40` | COVERED |
| `ShelterDecorPanel` | `shelter_decor_default` | ✔ | original Godot continuation | COVERED — populated real catalog/inventory/assignment/memorial fixture |
| `FactionMatrixPanel` | `faction_matrix_default` | ✔ | `#49`/`#53` | COVERED |
| `DoseLedgerPanel` | `dose_ledger_default` | ✔ | `#59` | COVERED |
| `GreenhousePanel` | `greenhouse_default` | ✔ | `#51` | COVERED (Phase 15 Tier-A4 full rewrite) |
| `SilentFoundryPanel` | `silent_foundry_default` | ✔ | `#1` (Cupola) | COVERED (Phase 16 Tier-A1 full rewrite) |
| `ExpeditionPanel` | `expedition_radar_default` (via `ExpeditionRadarPanel` sub-card) | ✔ | `#10` | COVERED (Phase 17 Tier-A5 sub-card sibling) |
| `SkillMatrixPanel` | `skill_matrix_default` | ✔ | `#22` | COVERED (Phase 19 Tier-2 HYBRID) |
| `DutyRosterPanel` | `duty_roster_default` | ✔ | `#22` (matrix half) | COVERED (Phase 20 Tier-2 full rewrite) |
| `FactionsPanel` | `factions_narrative_default` (via `FactionsNarrativePanel` sub-card) | ✔ | `#49` (narrative-heavy halve) | COVERED (Phase 21 Tier-3 sub-card sibling) |
| `CombatPanel` | `combat_hud_default` (via `CombatHudOverlay` sub-card) | ✔ | `#58` | COVERED (Phase 22 Tier-3 HUD sub-card) |
| `MapPanel` | `map_atlas_default` (via `MapAtlasPanel` sub-card) | ✔ | `#5` | COVERED (Phase 23 Tier-3 sub-card sibling) |
| `MaritimePanel` (+ `DeepCoastPanel`) | `maritime_atlas_default` (via `MaritimeAtlasPanel` sub-card) | ✔ | `#48` | COVERED (Phase 24 Tier-3 sub-card sibling) |
| `MusterPanel` | `muster_atlas_default` (via `MusterAtlasPanel` sub-card) | ✔ | (Currents/Roster, Expansion 06) | COVERED (Phase 25 Tier-3 sub-card sibling) |
| `QuestsPanel` | `quests_atlas_default` (via `QuestsAtlasPanel` sub-card) | ✔ | (Quest/Story) | COVERED (Phase 26 Tier-3 sub-card sibling) |
| `CraftingPanel` | — | — | (consumed via `SurvivalWorkstationPanel` #19) | REGRESSION_ONLY — drill-down of `survival_workstation_default` |
| `StandingRecordPanel` | `standing_record_atlas_default` (via `StandingRecordAtlasPanel` sub-card) | ✔ | (Expansion 03) | COVERED (Phase 27 Tier-3 sub-card sibling) |
| `ResearchPanel` | `research_atlas_default` (via `ResearchAtlasPanel` sub-card) | ✔ | (R&D / Library) | COVERED (Phase 28 Tier-3 sub-card sibling) |

29 tracked surfaces total: 27 COVERED + 1 PARTIAL + 1 REGRESSION_ONLY + 0 MISSING.

---

## Surfaces removed from tracking (audit decision)

For full rationale, see [`SURFACE_GAP_REPORT.md`](SURFACE_GAP_REPORT.md)
section A. Brief reasons:

| Surface | Reason deleted |
|---|---|
| `CombatHostSession panel` | No `src/UI/CombatHostPanel.cs` exists; live combat surface is `CombatHudOverlay`. |
| `EconomyPanel` | Anchored inside `CaravanBarterLedgerPanel` #35; not a separate modal. |
| `EconomyMarketPanel` (overlay) | Overlay variant inside the HUD. |
| `GameDashboardPanel` | Live HUD variant; live-tested, not snapshotted. |
| `GameHudOverlay` | Live HUD overlay; live-tested. |
| `Phase0Panel` | Pre-foundation core slice; never mounted in production scene tree. |
| `SaveLoadPanel` | "Dialog only" — `MainMenuPanel` carries Save/Load buttons. |
| `SettingsPanel` | "Settings dialog" — same situation as SaveLoadPanel. |
| `StatusPanel` | HUD overlay; live-tested. |
| `TutorialPanel` | Transient onboarding overlay; first-page content of `JournalPanel`. |
| `MainMenuPanel` | Brief modal on launch. |
| `GameOverPanel` | Brief modal at Verdict endgame paths. |
| `EpiloguePanel` | Brief modal at the ending. |
| `CenturySeedPanel` | Brief modal at century seed creation. |
| `CrossingQuestPanel` | Brief modal; sub-step of `QuestsAtlasPanel`. |
| `OpeningProtocolModal` | Brief modal at game start. |
| `MapDetailPanel` | Drill-down of `MapAtlasPanel`; never instantiated standalone. |

---

## Coverage holes (`MISSING`) — All Resolved

All former `MISSING` surfaces have been fully resolved with dedicated Core engines, host sessions, Tier-3 atlas panels, and snapshot targets:

| Surface | Resolved Phase | Core Engine & Host Adapter | Snapshot Target | Status |
|---|---|---|---|---|
| `StandingRecordPanel` | Phase 27 | `StandingRecordEngine.cs`, `StandingRecordHostSession.cs`, `StandingRecordAtlasPanel.cs` | `standing_record_atlas_default` | COVERED |
| `ResearchPanel` | Phase 28 | `ResearchSystem.cs`, `ResearchHostSession.cs`, `ResearchAtlasPanel.cs` | `research_atlas_default` | COVERED |

**0 MISSING surfaces remain.** All 29 tracked player-facing runtime surfaces are fully covered across 30 snapshot targets (27 COVERED + 1 PARTIAL + 1 REGRESSION_ONLY).
