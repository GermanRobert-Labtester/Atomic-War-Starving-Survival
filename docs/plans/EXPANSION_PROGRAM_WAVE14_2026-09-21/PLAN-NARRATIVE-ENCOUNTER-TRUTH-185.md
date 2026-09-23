# PLAN-NARRATIVE-ENCOUNTER-TRUTH-185 — Story Encounters: Gating, Branches & Exit States

**Wave 14 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-NARRATIVE-GRAPH-18, PLAN-MORAL-CHOICE-TRUTH-136, PLAN-NARRATIVE-CONTINUITY-TRUTH-170.
**Non-goals:** no graph storage (Plan 18), no moral flags (Plan 136), no
continuity validation (Plan 170).

## 1. Outcome
`Narrative/NarrativeEncounterSystem.cs` (**560 lines**) is reachable and
unaddressed: authored encounters with entry gates and branch exits. Plans 18,
136, and 170 own storage, flags, and validation — but the **encounter's own
contract** (when it may start, what it may assume, and what state it leaves
behind) is unstated, which is how encounters end up replayable or unresolvable.

| Deliverable | Detail |
|---|---|
| Entry contract | preconditions expressed over existing flags/state; an encounter never starts in an invalid world state |
| Branch exits | each exit is a typed outcome writing named flags/state via Plan 18/136 owners; no free-form mutations |
| Once-only semantics | repeatable vs once-per-campaign declared per encounter; a repeatable one has a bounded trigger |
| Abort path | a forced abort (death, crisis) leaves a documented state, never a half-applied branch |
| Save truth | in-progress encounters restore at the same node; a load never restarts or skips |

## 2. Evidence
- `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs` (560 lines; unaddressed — Wave 13 audit).
- Plan 170 catches contradictions these encounters could otherwise create.
- Plan 136 owns flag semantics exits write through.
- Plan 177 covers road encounters; this plan covers authored story encounters — the boundary is stated.

## 3. Packages
- **NET-185A** entry contract + precondition audit.
- **NET-185B** exit typing + flag-write audit (no free-form mutation).
- **NET-185C** once-only/repeatable matrix + trigger bound tests.
- **NET-185D** abort path fixture + documented state check.
- **NET-185E** save round-trip mid-encounter.

## 4. Acceptance & verification
- No encounter starts against an invalid state (fixture per invalid case).
- Every exit writes only through the named owners; a free-form mutation fails the audit.
- Abort leaves the documented state; reload resumes at the same node.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/`.

## 5. Risks
Replayability → once-only flags + trigger bounds are declared and tested.
Half-applied branches → abort path is a first-class fixture.

---

## 6. Expanded census (1 files · 560 lines)

Scope: `Assets/Ashfall.Core/Narrative/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `NarrativeEncounterSystem.cs` | 560 | System | **yes** | 0 | 0 | 5 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** `NarrativeEncounterSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Narrative/` |
| Test references | 25 name references across the test tree |
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

Domain files: 1. Other plans referencing their names: **2**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-NARRATIVE-GRAPH-18` | 1 |
| `PLAN-NARRATIVE-FAMILY-TRUTH-261` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `NET-185A` | no name match — resolve at claim time |
| `NET-185B` | no name match — resolve at claim time |
| `NET-185C` | no name match — resolve at claim time |
| `NET-185D` | no name match — resolve at claim time |
| `NET-185E` | `NarrativeEncounterSystem.cs` |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **14** · Test files: **25** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 14 | `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/ExpeditionHostSession.cs`, `src/Host/HostCli.NpcArcSelfTest.cs`, `src/Host/HostCli.PanelTests.cs`, `src/Host/NarrativeHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 25 | `Ashfall.Core.Tests/ContentUtilizationGraphTests.cs`, `Ashfall.Core.Tests/ExpeditionEncounterBridgeTests.cs`, `Ashfall.Core.Tests/Expeditions/MicroLocationLifecycleSmokeTests.cs`, `Ashfall.Core.Tests/Expeditions/MicroLocationRegressionMatrixTests.cs`, `Ashfall.Core.Tests/MicroLocationCatalogLoaderTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **7** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `encounter_choice` |
| `expedition` |
| `expedition_stealth` |
| `narrative` |
| `narrative_questlines` |
| `nuclear_core_lifecycle` |
| `procedural_narrative` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **15** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--7-day-smoke-selftest` |
| `--deterministic-smoke-selftest` |
| `--expedition-encounter-bridge-selftest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--expedition-playtest-selftest` |
| `--expedition-selftest` |
| `--journal-weather-panel-selftest` |
| `--narrative-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **24**.

| Event | First declaration |
|---|---|
| `OnBlightNarrative` | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` |
| `OnCampEncounterResolved` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnCampEncounterSurfaced` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnEncounterArrived` | `Assets/Ashfall.Core/YearOfAsh/DoorEncounterSystem.cs` |
| `OnEncounterEnded` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnEncounterResolved` | `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs` |
| `OnEncounterSelected` | `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs` |
| `OnEncounterTriggered` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnExpeditionCompleted` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnExpeditionFailed` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnExpeditionStarted` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnExpeditionTick` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/agriculture_items.json` |
| `Assets/StreamingAssets/Data/anomalous_expedition_encounters.json` |
| `Assets/StreamingAssets/Data/combat_catalog.json` |
| `Assets/StreamingAssets/Data/door_encounters.json` |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |
| `Assets/StreamingAssets/Data/faction_war_location_overrides.json` |
| `Assets/StreamingAssets/Data/micro_locations.json` |
| `Assets/StreamingAssets/Data/narrative/education_session_records.json` |
| `Assets/StreamingAssets/Data/narrative/expedition_briefs_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/expedition_field_reports.json` |
| `Assets/StreamingAssets/Data/narrative/expedition_field_reports_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/expedition_planning_briefs_batch_1.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **9** (95 files, 844 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Combat` | 10 | 84 |
| `Economy` | 41 | 329 |
| `Education` | 2 | 11 |
| `Foundry` | 8 | 73 |
| `Lifecycle` | 1 | 5 |
| `Narrative` | 26 | 293 |
| `NarrativeConsequence` | 1 | 20 |
| `PlayerCommand` | 1 | 1 |

**Verdict:** 844 cases sit under matching regions — run those first (`Audio`, `Combat`, `Economy`, `Education`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **439**
(230 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioSelfTest.cs` |
| `src/Disease/DiseaseHostSession.cs` |
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AgricultureSaveStore.cs` |
| `src/Host/AirlockSecurityHostSession.cs` |
| `src/Host/AmphibiousDraisineHostSession.cs` |
| `src/Host/ApprenticeshipHostSession.cs` |
| `src/Host/ArchiveDeskHostSession.cs` |
| `src/Host/AutopsyHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **32**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `airlock_security` | no |
| `amphibious_draisine` | no |
| `apprenticeship` | no |
| `archive_desk` | no |
| `autopsy` | no |
| `black_market` | no |
| `black_projects_archive` | no |
| `caravan_trade_network` | no |
| `combat` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **15**.

| Stream |
|---|
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `amphibious_draisine` |
| `aquaponics_disease` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `combat` |
| `cupola_foundry` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **345**
(CODEX_ONLY 279, GAMEPLAY_CONSUMED 49, OPTIONAL 5, UNRESOLVED 12).

| Catalog | Classification |
|---|---|
| `anomalous_expedition_encounters.json` | UNRESOLVED |
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `autopsy_procedures.json` | GAMEPLAY_CONSUMED |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `combat_catalog.json` | GAMEPLAY_CONSUMED |
| `crossing_encounters.json` | GAMEPLAY_CONSUMED |

**Verdict:** 12 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **3**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_chosen_faction_side` |
| `flag_preserved_archive` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 2
**Surface:** save sections 32 (laddered 0) · RNG streams 15 · host files 25 · catalogs 22 · test regions 9 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-NARRATIVE-ENCOUNTER-TRUTH-185
wave: 14
status: PROPOSED — foreman claim required
packages: NET-185A, NET-185B, NET-185C, NET-185D, NET-185E
claim paths:
  - src/Audio/AudioSelfTest.cs  # §19 candidate host surface
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Economy/TradeScreenGodotPanel.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/agriculture_items.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/anomalous_expedition_encounters.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --7-day-smoke-selftest
dependencies:
  - coordinate: 2 other plan(s) name these artifacts (§12)
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
