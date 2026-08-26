# ASHFALL — Tier-3 UI Readiness Map

**Date:** Reconciled post-Phase 28 (Historical Phase 14 baseline).
**Categorisation rule:** `READY` requires both a Core subsystem that exposes real data AND a placeholder runtime surface; `NEAR_READY` requires one of those; otherwise `BLOCKED_CORE`, `BLOCKED_ASSETS`, `REFERENCE_ONLY`, `DUPLICATE_VARIANT`, or `DEFER`.

## Categorisation

* `READY`: Core exists, runtime panel exists in `src/UI/`, dataset is plausible, snapshot target approved and tracked.
* `NEAR_READY`: Core exists; panel slot exists but no panel; candidate for future sub-card / overlay expansion.
* `BLOCKED_CORE`: no engine-agnostic Core subsystem.
* `BLOCKED_ASSETS`: Core exists but rendering requires assets still under legacy tree (see `docs/visual/WIRING_MATRIX.md`).
* `REFERENCE_ONLY`: Stitch screen exists; ASHFALL gameplay does not call for it.
* `DUPLICATE_VARIANT`: Stitch screen exists; ASHFALL already expresses the same UX elsewhere (e.g. TradeSurvival Workstation overlaps Caravan Barter Ledger).
* `DEFER`: Explicit DEFER for compatibility reasons.

## Tier-3 candidates (unpaired Stitch screens, prioritised by readiness × game value ÷ cost)

The list below reflects the delivered status across all roadmap phases (Phases 15–28).

### Tier A — Tier-3 Dashboard Candidates (ALL SHIPPED)

| # | Stitch | Subsystem | Runtime panel | Notes |
|---|---|---|---|---|
| A1 | `#1` | Silent Foundry | `SilentFoundrySystem` in `Assets/Ashfall.Core/Foundry/` | `SilentFoundryPanel.cs` | **SHIPPED Phase 16** (`silent_foundry_default`) |
| A2 | `#10` | Caravan Staging / Barter | `ExpeditionSystem` + `CaravanBarterLedgerPanel` | `CaravanBarterLedgerPanel.cs` | **SHIPPED Phase 12** (`caravan_barter_default`) |
| A3 | `#22` | Survivor Work Shifts | `DutyRosterSystem` present | `DutyRosterPanel.cs` | **SHIPPED Phase 20** (`duty_roster_default`) |
| A4 | `#51` | Hydroponics | `GreenhouseSystem` present | `GreenhousePanel.cs` | **SHIPPED Phase 15** (`greenhouse_default`) |
| A5 | `#54` | Expedition Radar | `ExpeditionSystem` exposes `Active` + `DemoDefinitions` | `ExpeditionRadarPanel.cs` | **SHIPPED Phase 17** (`expedition_radar_default`) |
| A6 | `#48` | Maritime Recon | `MaritimeHostSession` wraps `DiveSiteCatalog` | `MaritimeAtlasPanel.cs` | **SHIPPED Phase 24** (`maritime_atlas_default`) |
| A7 | `#22` | Skill Matrix | `SkillProgressionSystem` + `SurvivorsHostSession.RosterState` | `SkillMatrixPanel.cs` | **SHIPPED Phase 19** (`skill_matrix_default`) |
| A8 | `#22` | Factions Narrative | `FactionStanceEngine` (`IFactionStanceProvider`) | `FactionsNarrativePanel.cs` | **SHIPPED Phase 21** (`factions_narrative_default`) |
| A9 | `#22` | Combat HUD Overlay | `TacticalCombatSystem` via `CombatHostSession` | `CombatHudOverlay.cs` | **SHIPPED Phase 22** (`combat_hud_default`) |
| A10 | `#22` | Map Atlas | `ExpeditionDefinition` via `ExpeditionHostSession.DemoDefinitions` | `MapAtlasPanel.cs` | **SHIPPED Phase 23** (`map_atlas_default`) |
| A11 | `#22` | Muster Atlas | `MusterSystem` via `MusterHostSession` | `MusterAtlasPanel.cs` | **SHIPPED Phase 25** (`muster_atlas_default`) |
| A12 | `#22` | Quests Atlas | `QuestsHostSession` | `QuestsAtlasPanel.cs` | **SHIPPED Phase 26** (`quests_atlas_default`) |
| A13 | `#22` | Standing Record Atlas | `StandingRecordEngine` via `StandingRecordHostSession` | `StandingRecordAtlasPanel.cs` | **SHIPPED Phase 27** (`standing_record_atlas_default`) |
| A14 | `#22` | Research Atlas | `ResearchSystem` via `ResearchHostSession` | `ResearchAtlasPanel.cs` | **SHIPPED Phase 28** (`research_atlas_default`) |

### Tier B — Sub-card & Overlay Candidates (NEAR_READY)

| # | Stitch | Subsystem gating | Notes |
|---|---|---|---|
| B1 | `#3` Trade Caravan Route Dispatch | `ExpeditionSystem.HasOwner / OwnerState`; needs `CaravanRoute` model extension. | Add CaravanRoute dispatcher on top of `ExpeditionSystem`. |
| B2 | `#48` Maritime Recon | `MaritimePanel + DeepCoastPanel` | **SHIPPED Phase 24** via `MaritimeAtlasPanel` (`maritime_atlas_default`). |
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
| D1 | `#22` `#30` Survivor Skill Matrix half | **SHIPPED Phase 19** via `SkillMatrixPanel.cs` (`skill_matrix_default`) after Core port in Phase 18. |
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
| D20 | `#58` Combat Encounter HUD | **SHIPPED Phase 22** via `CombatHudOverlay.cs` (`combat_hud_default`). |
| D21 | `#60` UAV Reconnaissance | No subsystem. |
| D22 | `#61` Survivor Memorial & Funeral Rites | Memorial narrative; covered in `journal_default`. |

## Prioritisation formula roll-up

```
Score = GameplayImportance × RuntimeReadiness × ReusePotential × VisualValue ÷ ImplementationCost

                Importance   Readiness   Reuse   VisualValue   Cost   ScoreA
A1 Silent Foundry  4/5        5/5         4/5    4/5           2/5    6.4  ← **SHIPPED at Phase 16**
A2 Caravan Stage    4/5        5/5         4/5    5/5           3/5    3.5  ← **SHIPPED at Phase 12**
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

**All 14 Tier-A candidates shipped.**

## Historical Delivery Sequence (Phases 15–28)

1. **Phase 15:** `A4 Hydroponics` (`GreenhousePanel.cs`, target `greenhouse_default`)
2. **Phase 16:** `A1 Silent Foundry` (`SilentFoundryPanel.cs`, target `silent_foundry_default`)
3. **Phase 17:** `A5 Expedition Radar` (`ExpeditionRadarPanel.cs`, target `expedition_radar_default`)
4. **Phase 18:** `SkillProgressionSystem` Core port (engine + catalog + state)
5. **Phase 19:** `A7 Skill Matrix` (`SkillMatrixPanel.cs`, target `skill_matrix_default`)
6. **Phase 20:** `A3 Duty Roster` (`DutyRosterPanel.cs`, target `duty_roster_default`)
7. **Phase 21:** `A8 Factions Narrative` (`FactionsNarrativePanel.cs`, target `factions_narrative_default`)
8. **Phase 22:** `A9 Combat HUD Overlay` (`CombatHudOverlay.cs`, target `combat_hud_default`)
9. **Phase 23:** `A10 Map Atlas` (`MapAtlasPanel.cs`, target `map_atlas_default`)
10. **Phase 24:** `A6 Maritime Recon` (`MaritimeAtlasPanel.cs`, target `maritime_atlas_default`)
11. **Phase 25:** `A11 Muster Atlas` (`MusterAtlasPanel.cs`, target `muster_atlas_default`)
12. **Phase 26:** `A12 Quests Atlas` (`QuestsAtlasPanel.cs`, target `quests_atlas_default`)
13. **Phase 27:** `A13 Standing Record Atlas` (`StandingRecordAtlasPanel.cs`, target `standing_record_atlas_default`)
14. **Phase 28:** `A14 Research Atlas` (`ResearchAtlasPanel.cs`, target `research_atlas_default`)

## Post-Phase 28 Status & Quality Goals

* All 29 runtime snapshot targets are active and passing regression checks in `SnapshotHarness.cs` and `docs/ui/snapshot_manifest.json`.
* Maintain pixel-baseline lore and fixture determinism under `--ui-snapshot-uitest`.
* Continue data-integrity and catalog-rule enforcement across all 129 data authority sidecars.
