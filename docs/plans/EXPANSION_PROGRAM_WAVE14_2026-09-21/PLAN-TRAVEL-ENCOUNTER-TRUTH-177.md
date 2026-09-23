# PLAN-TRAVEL-ENCOUNTER-TRUTH-177 — Encounters en Route: Selection, Avoidance & Resolution

**Wave 14 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-TRANSPORT-EXPEDITION-30, PLAN-JOURNEY-CONTEXT-TRUTH-156, PLAN-RUMOR (Plan 120), PLAN-COMPATIBILITY (Plan 82).
**Non-goals:** no route topology (Plan 95), no travel math (Plan 30), no door
arrivals (Plan 155).

## 1. Outcome
`Narrative/TravelEncounterSystem.cs` (**1,093 lines**; unaddressed by any plan)
selects and resolves what a party meets on the road. Travel (Plan 30) and the
journey context (Plan 156) exist; nothing states encounter selection, avoidance
skills, or outcome routing — so road travel is either empty or a dice table.

| Deliverable | Detail |
|---|---|
| Encounter table | encounter classes per terrain/route class with weights; source data validated (Plan 90) |
| Selection | seeded selection drawing from a registered stream; day/hour and journey context inputs; no wall clock |
| Avoidance | documented avoidance inputs (scouting, speed, noise) that can skip or pre-empt a class; outcomes recorded |
| Resolution routing | combat routes to Plan 61/62, trade to Plan 96, discovery to Plan 70/108, conversation to Plan 18 — no local resolver |
| Save truth | resolved/avoided encounters restore; a load never re-rolls the leg |

## 2. Evidence
- `Assets/Ashfall.Core/Narrative/TravelEncounterSystem.cs` (1,093 lines; unaddressed — Wave 13 audit).
- Plan 156's context is the documented input surface; this plan consumes it.
- Plan 155 handles arrivals at the holdfast; road encounters are distinct.
- Plan 120's rumor state can bias social encounters; routing stated.

## 3. Packages
- **TET-177A** encounter table + data validation.
- **TET-177B** seeded selection determinism test (paired legs).
- **TET-177C** avoidance input table + skip/pre-empt tests.
- **TET-177D** resolution routing audit to the five owners above (no local resolver).
- **TET-177E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Same seed + same context → identical encounter sequence across runs.
- Avoidance changes the sequence only through documented inputs.
- Each resolution type appears in its owner's state; no outcome text is generated.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/`.

## 5. Risks
Dice-table travel → avoidance and context inputs make it legible; the paired-run test proves determinism.
Duplicate resolvers → routing audit forbids local resolution.

---

## 6. Expanded census (4 files · 1,669 lines)

Scope: `Assets/Ashfall.Core/Narrative/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · Demo 1 · Support 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `TravelEncounterCatalog.cs` | 355 | Catalog | — | 0 | 0 | 0 |
| `TravelEncounterHeadlessDemo.cs` | 159 | Demo | — | 0 | 0 | 2 |
| `TravelEncounterSelectionContext.cs` | 62 | Support | — | 0 | 0 | 0 |
| `TravelEncounterSystem.cs` | 1093 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 2 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** `TravelEncounterHeadlessDemo.cs`, `TravelEncounterSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Narrative/` |
| Test references | 38 name references across the test tree |
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

Domain files: 4. Other plans referencing their names: **0**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| — | no other plan references these files |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `TET-177A` | `TravelEncounterCatalog.cs`, `TravelEncounterHeadlessDemo.cs`, `TravelEncounterSelectionContext.cs` |
| `TET-177B` | `TravelEncounterSelectionContext.cs` |
| `TET-177C` | no name match — resolve at claim time |
| `TET-177D` | no name match — resolve at claim time |
| `TET-177E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 4. Host files: **6** · Test files: **19** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 6 | `src/Host/ExpeditionHostSession.cs`, `src/Host/HostCli.SelfTests.cs`, `src/Host/HostCli.WastelandInhabitants.cs`, `src/Host/TravelEncounterSaveStore.cs`, `src/Main.Expeditions.cs` |
| Tests (`Ashfall.Core.Tests/`) | 19 | `Ashfall.Core.Tests/CaravanPatrolIntegrationTests.cs`, `Ashfall.Core.Tests/PatrolBountyHandoffTests.cs`, `Ashfall.Core.Tests/PatrolCampaignCrossSystemSmokeTests.cs`, `Ashfall.Core.Tests/PatrolCostAndRequirementTests.cs`, `Ashfall.Core.Tests/PatrolEncounterFullRegressionTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 4 files; intra-domain edges: **4**; isolated: **0**.

| From | → To |
|---|---|
| `TravelEncounterHeadlessDemo` | `TravelEncounterCatalog` |
| `TravelEncounterHeadlessDemo` | `TravelEncounterSystem` |
| `TravelEncounterSystem` | `TravelEncounterCatalog` |
| `TravelEncounterSystem` | `TravelEncounterSelectionContext` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **2** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `encounter_choice` |
| `travel_encounters` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **4** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--expedition-encounter-bridge-selftest` |
| `--ice-road-tick-demo` |
| `--patrol-encounter-selftest` |
| `--travel-encounter-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **11**.

| Event | First declaration |
|---|---|
| `OnCampEncounterResolved` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnCampEncounterSurfaced` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnEncounterArrived` | `Assets/Ashfall.Core/YearOfAsh/DoorEncounterSystem.cs` |
| `OnEncounterEnded` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnEncounterResolved` | `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs` |
| `OnEncounterSelected` | `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs` |
| `OnEncounterTriggered` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnShelterEncounterResolved` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterEncounterStarted` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnSiteEncounterResolved` | `Assets/Ashfall.Core/StandingRecord/SiteEncounterSystem.cs` |
| `OnSiteEncounterStarted` | `Assets/Ashfall.Core/StandingRecord/SiteEncounterSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **2**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/narrative/wildlife_field_encounter_logs.json` |
| `Assets/StreamingAssets/Data/travel_encounters.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **0** (0 files, 0 cases).

| Region | Files | Cases |
|---|---:|---:|
| — | no test region shares a token with this domain |

**Verdict:** no test region shares a token with this domain. Region coverage is directory-based, so check root-level test files too (560 exist) before concluding coverage is absent.

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **7**
(0 of them panels/HUD).

| Host file |
|---|
| `src/Host/CoreDemoSession.cs` |
| `src/Host/EncounterChoiceSaveStore.cs` |
| `src/Host/TravelEncounterSaveStore.cs` |
| `src/Journal/JournalDemoHarness.cs` |
| `src/Muster/ApproachSelectionModal.cs` |
| `src/UI/SceneBindingHeadlessProbe.cs` |
| `src/YearOfAsh/DoorEncounterModal.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **2**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `encounter_choice` | no |
| `travel_encounters` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **0**.

| Stream |
|---|
| — | no seeded stream shares a token with this domain |

**Verdict:** no seeded stream shares a token — either the domain is deterministic without randomness (fine) or it draws from an unlisted source (check `System.Random`/time seeding before shipping).

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **2**
(CODEX_ONLY 1, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `narrative/wildlife_field_encounter_logs.json` | CODEX_ONLY |
| `travel_encounters.json` | UNRESOLVED |

**Verdict:** 1 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** free-start · **Coupling (incoming plans):** 0
**Surface:** save sections 2 (laddered 0) · RNG streams 0 · host files 7 · catalogs 4 · test regions 0 · flags 4

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-TRAVEL-ENCOUNTER-TRUTH-177
wave: 14
status: PROPOSED — foreman claim required
packages: TET-177A, TET-177B, TET-177C, TET-177D, TET-177E
claim paths:
  - src/Host/CoreDemoSession.cs  # §19 candidate host surface
  - src/Host/EncounterChoiceSaveStore.cs  # §19 candidate host surface
  - src/Host/TravelEncounterSaveStore.cs  # §19 candidate host surface
  - src/Journal/JournalDemoHarness.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/narrative/wildlife_field_encounter_logs.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/travel_encounters.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --expedition-encounter-bridge-selftest
dependencies:
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
