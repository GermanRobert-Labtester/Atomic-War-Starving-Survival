# PLAN-PRINT-MEDIA-TRUTH-128 — Broadsheets, Press Capacity & Distribution Reach

**Wave 10 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-RADIO-MEDIA-42, PLAN-VERTICAL-CULTURE-04, PLAN-RUMOR-PROPAGATION-TRUTH-120.
**Non-goals:** no radio work (Plan 42), no festival/museum content (Plan 4), no
second rumor store (Plan 120 owns rumor state).

## 1. Outcome
`Print/PublicBroadsheetPressEngine.cs` is host-unreachable (Plan 1 Appendix A)
and is the only print-media engine in Core. Plan 42 owns broadcast; Plan 120
owns rumor propagation. Print is a distinct medium with a physical constraint —
paper, ink, press time, and a distribution reach — and nothing states what a
broadsheet can do that radio cannot.

| Deliverable | Detail |
|---|---|
| Press inputs | paper/ink/press-hours consumed per issue; each maps to an existing inventory/service owner |
| Reach model | distribution by place type; a published issue raises knowledge of referenced facts within reach (rumor rows via Plan 120, never direct fact creation) |
| Content sourcing | issues draw from existing catalogs/records; no generated prose outside authored entries |
| Capacity truth | one press produces N issues per day at the press owner's rate; no panel-side arithmetic |
| Persistence | published issue list restores; an issue's effects never replay on load |

## 2. Evidence
- Plan 1 Appendix A/G: `PublicBroadsheetPressEngine` host-unreachable; candidate attach points listed.
- Plan 42 owns broadcast media; this plan's distinct medium is print.
- Plan 120 owns rumor lifecycle; print injects rumor rows, it does not own state.
- Plan 4 owns culture surfaces where an issue may be read again.

## 3. Packages
- **PMT-128A** press input model + consumption through the inventory seam.
- **PMT-128B** reach model + per-place-type test.
- **PMT-128C** content sourcing rules (authored entries only) + a fixture issue.
- **PMT-128D** capacity arithmetic at the owner + no-panel-math check.
- **PMT-128E** persistence + no-replay-on-load test.

## 4. Acceptance & verification
- Publishing one issue consumes the documented inputs exactly once; a second day's press resumes at the owner's rate.
- Effects appear within reach and not outside it (two-place fixture).
- Load after publishing shows the issue without re-applying effects.
- `bash scripts/run_test.sh` on the print/narrative regions.

## 5. Risks
Print becoming a second rumor engine → it creates rows in Plan 120's model through that owner.
Content generation → authored catalog entries only; the fixture proves the path.

---

## 6. Expanded census (1 files · 272 lines)

Scope: `Assets/Ashfall.Core/Print/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `PublicBroadsheetPressEngine.cs` | 272 | System | **yes** | 0 | 0 | 1 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `colony_blueprints.json` | object[3 keys] |
| `bunker_blueprints_codex.json` | object[3 keys] |

**State surfaces:** `PublicBroadsheetPressEngine.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Print/` |
| Test references | 1 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 0 empty-catch sites routed through Plan 35's rules |
| Premise | the **premise** file(s) above must match the plan's stated line counts before edits |

## 9. Rollout sequence

1. Premise re-check: premise files unchanged since authoring (hash/mtime), or update the plan.
2. Interfaces: wire through the named owner; do not add a parallel store.
3. State: if capture/restore exists, register per Plan 1 Appendix Q; else state the system is stateless.
4. Data: resolve domain catalogs or report the loader path.
5. Verification: focused region + the plan's own acceptance table.
6. Regression: re-run this census; a changed file is a finding.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Premise file | matches its stated surface; no scope drift |
| Interface/wiring | one owner per state, no parallel store |
| State/save | round-trip or explicit stateless verdict |
| Data binding | catalog resolves or loader path documented |
| Verification | focused region green; census delta recorded |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not widen the plan's scope.

---

## 12. Cross-plan coupling

Domain method: plan-body artifact list.
Governed artifacts: 4. Other plans referencing them: **6**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-VERTICAL-CULTURE-04` | 1 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 1 |
| `EVIDENCE` | 1 |
| `PLAN-TRANSPORT-EXPEDITION-30` | 1 |
| `PLAN-CREATIVE-WORKS-66` | 1 |
| `PLAN-CODEX-SURFACE-TRUTH-110` | 1 |

**Governed artifacts (first 12):**

| Artifact |
|---|
| `Print/PublicBroadsheetPressEngine.cs` |
| `PublicBroadsheetPressEngine.cs` |
| `bunker_blueprints_codex.json` |
| `colony_blueprints.json` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `PMT-128A` | `Print/PublicBroadsheetPressEngine.cs`, `PublicBroadsheetPressEngine.cs` |
| `PMT-128B` | no name match — resolve at claim time |
| `PMT-128C` | no name match — resolve at claim time |
| `PMT-128D` | no name match — resolve at claim time |
| `PMT-128E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 3. Host files: **2** · Test files: **4** · Data files: **7**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/Foundry/SilentFoundryHostSession.cs`, `src/Host/ExpansionHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 4 | `Ashfall.Core.Tests/BunkerBlueprintCatalogTests.cs`, `Ashfall.Core.Tests/Expeditions/Plan160ColonyIntegrationTests.cs`, `Ashfall.Core.Tests/Print/PublicBroadsheetPressEngineTests.cs`, `Ashfall.Core.Tests/SilentFoundrySystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 7 | `Assets/StreamingAssets/Data/mod_manifest_schema.json`, `Assets/StreamingAssets/Data/narrative/conflict_mediation_records.json`, `Assets/StreamingAssets/Data/narrative/council_meeting_minutes.json`, `Assets/StreamingAssets/Data/narrative/education_session_records.json`, `Assets/StreamingAssets/Data/narrative/equipment_failure_logs.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **4** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `equipment` |
| `equipment_condition` |
| `foundry` |
| `silent_foundry` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **7** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--save-load-failure-selftest` |
| `--save-load-failure-uitest` |
| `--save-load-ui-failure-selftest` |
| `--selftest-manifest` |
| `--silent-foundry-selftest` |
| `--silent-foundry-uitest` |
| `--test-manifest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **10**.

| Event | First declaration |
|---|---|
| `OnCodexUnlocked` | `Assets/Ashfall.Core/Journal/JournalSystem.cs` |
| `OnColonyDied` | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` |
| `OnColonyStressed` | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` |
| `OnColonySwarming` | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` |
| `OnConflictResolved` | `Assets/Ashfall.Core/SurvivorRelationsSystem.cs` |
| `OnConflictStarted` | `Assets/Ashfall.Core/SurvivorRelationsSystem.cs` |
| `OnDrillFailure` | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` |
| `OnEquipmentChanged` | `Assets/Ashfall.Core/EquipmentConditionSystem.cs` |
| `OnEquipmentDamaged` | `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` |
| `OnPumpFailure` | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/antigravity_survivor_fields.json` |
| `Assets/StreamingAssets/Data/audio_logs_expansion_05.json` |
| `Assets/StreamingAssets/Data/breaching_equipment_catalog.json` |
| `Assets/StreamingAssets/Data/bunker_graffiti_postings.json` |
| `Assets/StreamingAssets/Data/codex_entries.json` |
| `Assets/StreamingAssets/Data/colony_blueprints.json` |
| `Assets/StreamingAssets/Data/conflict_templates.json` |
| `Assets/StreamingAssets/Data/cupola_foundry_catalog.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |
| `Assets/StreamingAssets/Data/education_curriculum.json` |
| `Assets/StreamingAssets/Data/excavation_hazard_mitigation.json` |
| `Assets/StreamingAssets/Data/expansion_survivor_fields.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **9** (123 files, 983 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Codex` | 2 | 29 |
| `Education` | 2 | 11 |
| `Equipment` | 1 | 4 |
| `Excavation` | 1 | 5 |
| `Foundry` | 8 | 73 |
| `Integration` | 16 | 74 |
| `Print` | 1 | 5 |
| `Shelter` | 87 | 754 |

**Verdict:** 983 cases sit under matching regions — run those first (`Audio`, `Codex`, `Education`, `Equipment`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **210**
(24 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioConditionHostBridge.cs` |
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Disease/DiseaseHostSession.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AirlockSecurityHostSession.cs` |
| `src/Host/AmphibiousDraisineHostSession.cs` |
| `src/Host/AnomalyHazardSaveStore.cs` |
| `src/Host/ApprenticeshipHostSession.cs` |
| `src/Host/ArchiveDeskHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **42**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `airlock_security` | no |
| `amphibious_draisine` | no |
| `anomaly_hazard` | no |
| `apprenticeship` | no |
| `archive_desk` | no |
| `black_projects_archive` | no |
| `deep_well` | no |
| `disease` | no |
| `draisine_recovery` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **13**.

| Stream |
|---|
| `acoustic_detection` |
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `amphibious_draisine` |
| `anomaly_hazard` |
| `aquaponics_disease` |
| `cupola_foundry` |
| `deep_coast` |
| `disease` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **139**
(CODEX_ONLY 106, GAMEPLAY_CONSUMED 16, OPTIONAL 5, UNRESOLVED 12).

| Catalog | Classification |
|---|---|
| `acoustic_triangulation_catalog.json` | UNRESOLVED |
| `antigravity_survivor_fields.json` | OPTIONAL |
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `bunker_graffiti_postings.json` | UNRESOLVED |
| `codex_entries.json` | UNRESOLVED |
| `cultural_archive_tomes.json` | UNRESOLVED |
| `cupola_foundry_catalog.json` | UNRESOLVED |
| `deep_lore_locations.json` | GAMEPLAY_CONSUMED |

**Verdict:** 12 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **2**.

| Flag |
|---|
| `flag_expelled_survivor` |
| `flag_preserved_archive` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 6
**Surface:** save sections 42 (laddered 0) · RNG streams 13 · host files 24 · catalogs 22 · test regions 9 · flags 7

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-PRINT-MEDIA-TRUTH-128
wave: 10
status: PROPOSED — foreman claim required
packages: PMT-128A, PMT-128B, PMT-128C, PMT-128D, PMT-128E
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/antigravity_survivor_fields.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/audio_logs_expansion_05.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --save-load-failure-selftest
dependencies:
  - coordinate: 6 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
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
| verification | yes |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** none — claim-ready.
