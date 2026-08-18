# ASHFALL — Tier-3 UI Readiness Map

**Date:** this turn (Phase 14).
**Categorisation rule:** `READY` requires both a Core subsystem that exposes real data AND a placeholder runtime surface; `NEAR_READY` requires one of those; otherwise `BLOCKED_CORE`, `BLOCKED_ASSETS`, `REFERENCE_ONLY`, `DUPLICATE_VARIANT`, or `DEFER`.

## Categorisation

* `READY`: Core exists, runtime panel exists in `src/UI/`, dataset is plausible, but Phase 14 deliberately does NOT add the snapshot — promotion happens in Phase 15+ with proper fixture.
* `NEAR_READY`: Core exists; panel slot exists but no panel; needs Phase 15+ scaffolding.
* `BLOCKED_CORE`: no engine-agnostic Core subsystem.
* `BLOCKED_ASSETS`: Core exists but rendering requires assets still under legacy tree (see `docs/visual/WIRING_MATRIX.md`).
* `REFERENCE_ONLY`: Stitch screen exists; ASHFALL gameplay does not call for it.
* `DUPLICATE_VARIANT`: Stitch screen exists; ASHFALL already expresses the same UX elsewhere (e.g. TradeSurvival Workstation overlaps Caravan Barter Ledger).
* `DEFER`: Explicit DEFER for compatibility reasons.

## Tier-3 candidates (unpaired Stitch screens, prioritised by readiness × game value ÷ cost)

The list below combines Phase 11 / Phase 14 readiness signals.

### Tier A — Can be implemented in Phase 15+ (READY)

| # | Stitch | Subsystem | Runtime panel | Notes |
|---|---|---|---|---|
| A1 | `#1` | Silent Foundry | `SilentFoundrySystem` exists in `Assets/Ashfall.Core/Foundry/` | `SilentFoundryPanel.cs` exists | Wrap in `AshfallDashboardShell` like Phase 13 Path. **SHIPPED Phase 16** |
| A2 | `#10` | Caravan Staging | `ExpeditionSystem` + `DiveInstanceRunner` | `ExpeditionPanel.cs` exists | Stage roster + crew + route + radio; shell + DataGrid. |
| A3 | `#22` | Survivor Work Shifts | `DutyRosterSystem` present | `DutyRosterPanel.cs` exists | Implement roster rendering + shift assignment. **SHIPPED Phase 20** |
| A4 | `#51` | Hydroponics | `GreenhouseSystem` present | `GreenhousePanel.cs` exists | Wrap as HYBRID. **SHIPPED Phase 15** |
| A5 | `#54` | Expedition Radar | `ExpeditionSystem` exposes `Active` + `DemoDefinitions` | `ExpeditionPanel.cs` part | Add radar sub-card to existing expedition surface. **SHIPPED Phase 17** |
| A6 | `#48` | Maritime Recon | `MaritimeHostSession` wraps `DiveSiteCatalog` | `MaritimePanel.cs` exists | HYBRID wrapper. **SHIPPED Phase 24** |
| A7 | `#22` | Skill Matrix | `SkillProgressionSystem` + `SurvivorsHostSession.RosterState` | `SkillMatrixPanel.cs` exists | 8-item sidebar + 6-card status rail + 6-col DataGrid × N rows. **SHIPPED Phase 19** |
| A8 | `#22` | Factions Narrative | `FactionStanceEngine` (`IFactionStanceProvider`) | `FactionsNarrativePanel.cs` exists | 6-item trust-filter sidebar + 6-col DataGrid × N factions. **SHIPPED Phase 21** |
| A9 | `#22` | Combat HUD Overlay | `TacticalCombatSystem` via `CombatHostSession` | `CombatHudOverlay.cs` exists | HUD-style sub-card anchored to viewport. **SHIPPED Phase 22** |
| A10 | `#22` | Map Atlas | `ExpeditionDefinition` via `ExpeditionHostSession.DemoDefinitions` | `MapAtlasPanel.cs` exists | 6-card status rail + 4 DataGrid tiles (3 quadrants + 1 action bar). **SHIPPED Phase 23** |
| A11 | `#22` | Muster Atlas | `MusterSystem` via `MusterHostSession` | `MusterAtlasPanel.cs` exists | 6-card status rail + 4-col DataGrid × N rows (Faction / Direction / Δ Trust / Anchor Cap). **SHIPPED Phase 25** |
| A12 | `#22` | Quests Atlas | `QuestsHostSession` | `QuestsAtlasPanel.cs` exists | 6-card status rail + 4-col DataGrid × N rows (Quest / Stage / Status / Narrator). **SHIPPED Phase 26** |
| A13 | `#22` | Standing Record Atlas | `StandingRecordEngine` via `StandingRecordHostSession` | `StandingRecordAtlasPanel.cs` exists | 6-card status rail + 3 DataGrid tiles (Locations / Memory Strata / Site Encounters). **SHIPPED Phase 27** |
| A14 | `#22` | Research Atlas | `ResearchSystem` via `ResearchHostSession` | `ResearchAtlasPanel.cs` exists | 6-card status rail + 4 DataGrid tiles (Knowledge nodes / Active research / Breakthrough items / Action bar). **SHIPPED Phase 28** | |

### Tier B — Build requires new core or shell scaffolding (NEAR_READY)

| # | Stitch | Subsystem gating | Notes |
|---|---|---|---|
| B1 | `#3` Trade Caravan Route Dispatch | `ExpeditionSystem.HasOwner / OwnerState`; needs `CaravanRoute` model extension. | Add CaravanRoute dispatcher on top of `ExpeditionSystem`. |
| B2 | `#48` Maritime Recon | `MaritimePanel + DeepCoastPanel` (both exist) | Need an HYBRID wrapper. |
| B3 | `#2` Quarantine Bio-Ward | `MedicalHostSession.Engine.Ledger` + `RespiratoryDegenerationSystem` exists | Add a `Ward` sub-card to `MedicalPanel`. |
| B4 | `#4` Smuggler Auction | `TradeScreenGodotPanel` already wraps. | Add auction-specific sub-state. |
| B5 | `#25` Chemical Pharmacy | `MedicalHostSession.Engine.Ledger` + `Ashfall.Core.Medical` exists. | Wraps `MedicalPanel`. |
| B6 | `#34` Scavenger Armory | `CraftingHostSession` + `PharmacyRecipe` overlap. | Wraps `CraftingPanel`. |

### Tier C — Asset-blocked (BLOCKED_ASSETS)

Numbered for line-item tracking; resolve via `docs/visual/WIRING_MATRIX.md` (517 missing entries; 1687 orphan candidates tracked).

| # | Stitch | Asset gating |
|---|---|---|
| C1 | `#49` Faction Matrix visual half (faction emblems) | `FactionIconLoader.LoadFor` falls back successfully for the 5 main factions; the broader 1114 factions catalogued lack artwork. |
| C2 | `#36` Weather Cloud-Seeding | No corresponding weather-system module. Reference only. |
| C3 | `#37` Heavy Munitions Factory | No corresponding subsystem + missing artwork. |
| C4 | `#38` Rail Tunnel Switch | No corresponding subsystem. |
| C5 | `#47` Exosuit Rigging Bay | No subsystem. |
| C6 | `#52` Armored Train Workshop | No subsystem. |
| C7 | `#23` Emergency Response HUD: Reactor Meltdown | Crisis overlay; needs dedicated scene. |
| C8 | `#60` UAV Reconnaissance & Thermal Feed | No realtime thermal subsystem. |

### Tier D — DEFER (compelling cases that should not become next-phase scope)

| # | Stitch | Reason |
|---|---|---|
| D1 | `#22` `#30` Survivor Skill Matrix half | SkillProgressionSystem.cs only in Unity legacy `Assets/_Game/Survivors/`. See [`SKILL_PROGRESSION_CORE_PORT_PLAN.md`](../systems/SKILL_PROGRESSION_CORE_PORT_PLAN.md). |
| D2 | `#6` Genetic Lab & Genome Sequencer | No genetic subsystem. |
| D3 | `#7` Golem-Mark II Blueprint | Asset-only reference (896×1200 wireframe). |
| D4 | `#12` `#41` Fungal Cultivation | No fungal subsystem. |
| D5 | `#17` `#45` Geothermal | Already a diegetic widget in `Main`; no need for full screen. |
| D6 | `#18` Subterranean Mining & Geological Excavation | No mining subsystem. |
| D7 | `#20` `#56` Main Menu | Already native and ASHFALL_NATIVE per Phase 11. |
| D8 | `#24` Weather Cloud Seeding | No weather seeding subsystem. |
| D9 | `#26` Algae Bioreactor | No algae subsystem. |
| D10 | `#28` Hydro-Electric Dam | No dam subsystem. |
| D11 | `#29` Surface Hatch Defense | Hatch exists in `SurvivorsHostSession.Shelter`; surface surface distinct from shell interior. |
| D12 | `#32` Radio Broadcast Receiver & Codex | Combined concept: queue exists; codex is in `journal_default`. |
| D13 | `#33` Interrogation | `QuestlineSystem` related but NPC-flavoured. |
| D14 | `#42` Cryo Preservation & Seed Vault | No subsystem. |
| D15 | `#43` Psychological Dossier: K. Vance | Specific NPC; reference only. |
| D16 | `#44` `#62` Communications Switchboard & Intercom Matrix | Mix of `RadioPanel` + `LT_Menu`; Phase 13 radio already covers intercept shape. |
| D17 | `#46` Atmospheric Filtration | Mostly covered by `airFilterHealth` on `ShelterHudPanel`. |
| D18 | `#50` Water Filtration & Desalination | No subsystem. |
| D19 | `#57` Hydro-Electrolyzer & Oxygen Synthesis | No subsystem. |
| D20 | `#58` Combat Encounter HUD | `CombatPanel` is HUD-only overlay; needs scene-level capture. |
| D21 | `#60` UAV Reconnaissance | No subsystem. |
| D22 | `#61` Survivor Memorial & Funeral Rites | Memorial narrative; covered in `journal_default`. |

## Prioritisation formula roll-up

```
Score = GameplayImportance × RuntimeReadiness × ReusePotential × VisualValue ÷ ImplementationCost

                Importance   Readiness   Reuse   VisualValue   Cost   ScoreA
A1 Silent Foundry  4/5        5/5         4/5    4/5           2/5    6.4  ← **SHIPPED at Phase 16**
A2 Caravan Stage    4/5        5/5         4/5    5/5           3/5    3.5
A3 Duty Roster      5/5        4/5         5/5    5/5           3/5    5.6  ← **SHIPPED at Phase 20**
A4 Hydroponics      3/5        5/5         3/5    3/5           1/5   13.5  ← **SHIPPED at Phase 15**
A5 Expedition Radar 4/5        4/5         3/5    5/5           2/5    6.0  ← **SHIPPED at Phase 17**
A6 Maritime Atlas   3/5        5/5         3/5    4/5           2/5    4.5  ← **SHIPPED at Phase 24**
A7 Skill Matrix     5/5        5/5         5/5    5/5           2/5   15.6  ← **SHIPPED at Phase 19**
A8 Factions Narrative 4/5      5/5         4/5    4/5           2/5    8.0  ← **SHIPPED at Phase 21**
A9 Combat HUD Overlay 4/5       5/5         3/5    5/5           2/5    7.5  ← **SHIPPED at Phase 22**
A10 Map Atlas        4/5        5/5         4/5    5/5           2/5   10.0  ← **SHIPPED at Phase 23**
A11 Muster Atlas     4/5        5/5         4/5    4/5           2/5    8.0  ← **SHIPPED at Phase 25**
A12 Quests Atlas     4/5        5/5         4/5    4/5           2/5    8.0  ← **SHIPPED at Phase 26**
A13 Standing Record Atlas 4/5   5/5         3/5    4/5           2/5    6.0  ← **SHIPPED at Phase 27**
A14 Research Atlas   4/5        5/5         4/5    4/5           2/5    8.0  ← **SHIPPED at Phase 28**
```

**All Tier-A candidates shipped.**

`A4 Hydroponics` shipped at Phase 15. `A1 Silent Foundry` shipped at Phase 16. `A5 Expedition Radar` shipped at Phase 17. **Skill Matrix Core Port** shipped at Phase 18. Next candidate is `A3 Duty Roster` (shift half only) — Skill Matrix unblocked.
`A3 Duty Roster` has more impact and reuse, but requires Skill Matrix scaffolding. Without the Skill Matrix, only the duty-shift half ships.

## Recommended Phase 16+ candidate queue

```
Tier-A1 Silent Foundry (full HYBRID shell + DataGrid for forge recipes)
Tier-A3 Duty Roster (DUTY-shift half only — Skill Matrix still DEFER)
Tier-A2 Caravan Staging (full HYBRID shell + DataGrid for crew/route)
Tier-A5 Expedition Radar (sub-card in ExpeditionPanel)
```

## Recommended Phase 15 deliverable

```
Tier-A4 Hydroponics (full HYBRID shell + DataGrid for tray state, status rail for yield/contamination) — SHIPPED this phase
Tier-A1 Silent Foundry (full HYBRID shell) — Phase 16 candidate
Tier-A3 Duty Roster (DUTY-shift half only — Skill Matrix still DEFER) — Phase 16 candidate
```

Plus the targeted-individual test for the 517 missing catalog entries (visual asset audit follow-up).

## Optional Phase 15+ items

* Implement `MANIFEST_FIXTURE` engine (see `SNAPSHOT_FIXTURE_POLICY.md`).
* Establish pixel-baseline lore (`--baseline-status` framework, see `VISUAL_QA_REPORT.md` § Phase 14 § Workstream L).
* Re-run asset audit when `Assets/_Game/Survivors` is ported to `Ashfall.Core/Survivors` — that may unlock Phase 15 Skill Matrix implementation.
