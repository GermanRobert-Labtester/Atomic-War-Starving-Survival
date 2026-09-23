# PLAN-HOST-COMPOSITION-GOVERNANCE-71 — Partial Files, Setup Naming & Composition Order

**Wave 7 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-INTEGRATION-KIT-02, PLAN-TEMPORAL-AUTHORITY-33.
**Expanded appendix:** [`PLAN-HOST-COMPOSITION-GOVERNANCE-71_APPENDIX-A_PARTIAL_INVENTORY.md`](PLAN-HOST-COMPOSITION-GOVERNANCE-71_APPENDIX-A_PARTIAL_INVENTORY.md)
— the composition inventory: **146 Main partials** (100 with Setup/Save methods) and duplicate-name findings (HC-71B gate input).

**Non-goals:** no rewrite of `Main`, no new composition root.

## Outcome
The host is a giant partial class: **~200 `Main.*.cs` files**, 226 `Setup*`
methods, 780 host session/store files, 104 day-owner references. That scale is
workable only with discipline: unique names, one canonical order, no duplicate
setup, no orphan partials. This plan makes composition governance mechanical.

| Deliverable | Detail |
|---|---|
| Partial inventory | file → setup methods → fields → session types → ownership |
| Name contract | `Setup<Subsystem>`, `Save<Subsystem>`, `_<field>`; collisions fail |
| Order contract | `ComposeCampaign` order published and asserted (ties Plan 33) |
| Orphan partials | files whose Setup is never called and whose fields are unused are retired |
| Duplicate setup | two partials defining the same Setup/Field → fail |
| File budget | partials over N lines flagged with a split proposal |
| Docs | `docs/architecture/HOST_COMPOSITION.md` generated |

## Evidence
- 226 `Setup*` methods (grep audit), ~200 partials in `src/Main.*.cs`.
- Composition root: `src/Main.CampaignServices.cs` (40+ calls + manifest bootstrap).
- Duplicate-name risk: plan-number collisions produced `Main.Plans157.cs` (grain milling) vs ledger Plan 157 (comms).
- Lifecycle entries: 18 manifest entries vs 226 setups (PLAN-INTEGRATION-KIT-02 K2/K5 closes this).
- `MainLatency`/triad gate exists: "Setup/Save/AllSaveSections Triad Drift Gate".

## Packages
- **HC-71A** generated inventory: every partial, its setups, its fields, its session types, its save methods.
- **HC-71B** naming/collision gate: unique Setup/Save names; new duplicates fail with both file paths.
- **HC-71C** orphan partial sweep: unused fields/setups retired with evidence (do not delete a partial that owns a live session).
- **HC-71D** order publication: ordered composition list generated from source and asserted by a probe.
- **HC-71E** file budget + split proposals for the largest partials (report only; no auto-split).

## Acceptance & verification
- Zero duplicate Setup names; zero orphan partials without a reason; order probe green.
- `python3 scripts/ci/generate-host-composition.py --check`; the triad gate; `dotnet build Ashfall.csproj` 0/0.

## Risks
Renaming setups breaks references → gate reports all call sites; renames land with the mapping in one package.

---

## 6. Expanded census (bespoke: host composition)

This plan governs the host's partial composition, so the census covers
`src/Main.*` partials and their setup/save methods.

| Metric | Value |
|---|---:|
| `Main.*` partials | 146 |
| `Setup*` methods | 228 |
| `Save*` methods | 204 |
| Duplicate setup names | 0 |

**Largest partials:**

| File | Lines | Setups |
|---|---:|---:|
| `Main.UiPanels.cs` | 1715 | 0 |
| `Main.CampaignOwners.cs` | 1520 | 0 |
| `Main.World.cs` | 945 | 6 |
| `Main.GameFlow.cs` | 926 | 0 |
| `Main.Application.cs` | 923 | 0 |
| `Main.PlayerSurfaces.cs` | 875 | 0 |
| `Main.Plans146_149.cs` | 846 | 6 |
| `Main.Plans162_165.cs` | 831 | 4 |
| `Main.ExpandedShelterSystems.cs` | 826 | 1 |
| `Main.Expeditions.cs` | 731 | 7 |
| `Main.Narrative.cs` | 729 | 4 |
| `Main.ShelterInfrastructure.cs` | 629 | 9 |
| `Main.AdvancedShelterSystems.cs` | 610 | 7 |
| `Main.ShelterSocial.cs` | 603 | 7 |
| `Main.UiTests.Plans198_201.cs` | 599 | 0 |

## 7. Expanded surface: composition rules

| Rule | Detail |
|---|---|
| Naming | one `Setup<Feature>` per feature; duplicates are defects |
| Order | composition order documented; no hidden dependency on call order |
| Budget | per-partial size checked; a growing partial is flagged |
| Orphan partial | a partial with no setup or save is reported |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Duplicate names | zero (or each recorded with a reason) |
| Order probe | setup call order captured once and diffed |
| Budgets | per-partial line budget reported, not enforced blindly |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census (this section) — no edits.
2. Duplicate/orphan findings routed to owners.
3. Order probe recorded and reviewed.
4. Budgets documented from the census.
5. Regression: re-run the census; any drift is a finding.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Partial | named, budgeted, one feature |
| Setup/Save | unique name; maps to a section or action |
| Order | documented and probed |
| Orphan | reported, never silently kept |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not mandate a split.

---

## 12. Cross-plan coupling

Domain method: plan-body artifact list.
Governed artifacts: 19. Other plans referencing them: **10**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-ORPHAN-SEAL-01` | 6 |
| `PLAN-LAUNCH-FACE-06` | 3 |
| `EVIDENCE` | 3 |
| `PLAN-THREADING-ASYNCHRONY-72` | 3 |
| `PLAN-INTEGRATION-KIT-02` | 2 |
| `PLAN-RUNTIME-PERF-16` | 2 |
| `PLAN-VERTICAL-CULTURE-04` | 1 |
| `PLAN-UI-SURFACE-15` | 1 |

**Governed artifacts (first 12):**

| Artifact |
|---|
| `Main.AdvancedShelterSystems.cs` |
| `Main.Application.cs` |
| `Main.CampaignOwners.cs` |
| `Main.ExpandedShelterSystems.cs` |
| `Main.Expeditions.cs` |
| `Main.GameFlow.cs` |
| `Main.Narrative.cs` |
| `Main.Plans146_149.cs` |
| `Main.Plans157.cs` |
| `Main.Plans162_165.cs` |
| `Main.PlayerSurfaces.cs` |
| `Main.ShelterInfrastructure.cs` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `HC-71A` | `PLAN-HOST-COMPOSITION-GOVERNANCE-71_APPENDIX-A_PARTIAL_INVENTORY.md` |
| `HC-71B` | no name match — resolve at claim time |
| `HC-71C` | `PLAN-HOST-COMPOSITION-GOVERNANCE-71_APPENDIX-A_PARTIAL_INVENTORY.md` |
| `HC-71D` | `PLAN-HOST-COMPOSITION-GOVERNANCE-71_APPENDIX-A_PARTIAL_INVENTORY.md`, `docs/architecture/HOST_COMPOSITION.md` |
| `HC-71E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 19. Host files: **0** · Test files: **0** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 0 | — |
| Tests (`Ashfall.Core.Tests/`) | 0 | — |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no host attachment found for these symbols — candidate integration gap (confirm under alternate names); no test reference found — coverage risk; no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **26** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `campaign` |
| `campaign_day` |
| `expanded_shelter` |
| `expeditions` |
| `infrastructure` |
| `inventory` |
| `narrative` |
| `narrative_questlines` |
| `procedural_narrative` |
| `route_infrastructure` |
| `shelter` |
| `shelter_assignment` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **27** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--advanced-industrial-recon-selftest` |
| `--campaign-journey-selftest` |
| `--inventory-save-selftest` |
| `--inventory-selftest` |
| `--inventory-uitest` |
| `--narrative-selftest` |
| `--plans-122-125-balance-soak` |
| `--plans-122-125-selftest` |
| `--plans-139-141-selftest` |
| `--player-panels-ui-test` |
| `--player-panels-uitest` |
| `--propaganda-campaign-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **12**.

| Event | First declaration |
|---|---|
| `OnBlightNarrative` | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` |
| `OnChapterAdvanced` | `Assets/Ashfall.Core/Legacy/GenerationalSuccessionEngine.cs` |
| `OnDayAdvanced` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` |
| `OnInventoryChanged` | `Assets/Ashfall.Core/Inventory/Inventory.cs` |
| `OnNarrativeMarker` | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` |
| `OnNarrativeRequested` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnQuestStageAdvanced` | `Assets/Ashfall.Core/DutyRoster/DutyRosterQuestRuntime.cs` |
| `OnShelterEncounterResolved` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterEncounterStarted` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterFalseAlarm` | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` |
| `OnStageAdvanced` | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` |
| `OnStageNarrativeEmitted` | `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/agriculture_items.json` |
| `Assets/StreamingAssets/Data/black_market_inventory.json` |
| `Assets/StreamingAssets/Data/campaign_epilogues.json` |
| `Assets/StreamingAssets/Data/combat_catalog.json` |
| `Assets/StreamingAssets/Data/crossing_encounters.json` |
| `Assets/StreamingAssets/Data/crossing_factions.json` |
| `Assets/StreamingAssets/Data/crossing_items.json` |
| `Assets/StreamingAssets/Data/crossing_locations.json` |
| `Assets/StreamingAssets/Data/crossing_quests.json` |
| `Assets/StreamingAssets/Data/deep_lore_locations.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |
| `Assets/StreamingAssets/Data/duty_roles.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **14** (251 files, 2057 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Campaign` | 32 | 187 |
| `Combat` | 10 | 84 |
| `DutyRoster` | 5 | 49 |
| `Expeditions` | 41 | 322 |
| `Factions` | 10 | 72 |
| `Flagship11` | 7 | 63 |
| `Governance` | 5 | 27 |
| `InformationFlow` | 3 | 19 |
| `Inventory` | 15 | 114 |

**Verdict:** 2057 cases sit under matching regions — run those first (`Audio`, `Campaign`, `Combat`, `DutyRoster`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **253**
(34 of them panels/HUD).

| Host file |
|---|
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AgricultureSaveStore.cs` |
| `src/Host/CampaignDayPersistenceAdapter.cs` |
| `src/Host/CampaignDaySaveStore.cs` |
| `src/Host/CombatHostSession.cs` |
| `src/Host/CombatSaveStore.cs` |
| `src/Host/ContractorRosterHostSession.cs` |
| `src/Host/DeepCoastHostSession.cs` |
| `src/Host/DeepWellHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **47**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `black_market` | no |
| `black_projects_archive` | no |
| `campaign` | no |
| `campaign_day` | no |
| `combat` | no |
| `contractor_roster` | no |
| `crossing` | no |
| `deep_well` | no |
| `duty_roster` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **14**.

| Stream |
|---|
| `acoustic_detection` |
| `advanced_mfg_ebpvd_coating` |
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `combat` |
| `deep_coast` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **357**
(CODEX_ONLY 279, GAMEPLAY_CONSUMED 54, OPTIONAL 6, UNRESOLVED 18).

| Catalog | Classification |
|---|---|
| `acoustic_triangulation_catalog.json` | UNRESOLVED |
| `anomalous_expedition_encounters.json` | UNRESOLVED |
| `antigravity_survivor_fields.json` | OPTIONAL |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `combat_catalog.json` | GAMEPLAY_CONSUMED |
| `crossing_encounters.json` | GAMEPLAY_CONSUMED |
| `crossing_factions.json` | GAMEPLAY_CONSUMED |

**Verdict:** 18 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **3**.

| Flag |
|---|
| `flag_become_warlord` |
| `flag_expelled_survivor` |
| `flag_repaired_infrastructure` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY-WITH-NOTES (11/12) · **Class:** standard · **Coupling (incoming plans):** 10
**Surface:** save sections 47 (laddered 1) · RNG streams 14 · host files 25 · catalogs 22 · test regions 10 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-HOST-COMPOSITION-GOVERNANCE-71
wave: 7
status: PROPOSED — foreman claim required
packages: HC-71A, HC-71B, HC-71C, HC-71D, HC-71E
claim paths:
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - src/Host/AgricultureHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/agriculture_items.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/black_market_inventory.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --advanced-industrial-recon-selftest
dependencies:
  - coordinate: 10 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
  - touches 1 versioned save ladder(s) — extend, never fork
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | yes |
| depends | yes |
| non_goals | yes |
| outcome | yes |
| evidence | yes |
| packages | yes |
| acceptance | yes |
| risks | yes |
| verification | **no** |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** author or confirm: verification.
