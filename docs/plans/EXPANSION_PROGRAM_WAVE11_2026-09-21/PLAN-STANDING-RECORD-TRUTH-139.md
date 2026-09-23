# PLAN-STANDING-RECORD-TRUTH-139 — What Each Site Remembers, Forgets & Triggers

**Wave 11 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SPATIAL-SIM-AUTHORITY-95, PLAN-DISCOVERY-STATE-108, PLAN-SAVE-MIGRATION-CORRIDOR-87.
**Implementation scaffold:** [`PLAN-STANDING-RECORD-TRUTH-139_APPENDIX-A_SCAFFOLD.md`](PLAN-STANDING-RECORD-TRUTH-139_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-DISCOVERY-STATE-108` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no place geometry (Plan 95 owns it), no knowledge-state machine
for route/place visibility (Plan 108 owns it), no encounter content authoring.

## 1. Outcome
`StandingRecord/` holds six types: `StandingRecordCatalog`, `StandingRecordEngine`,
`LocationLayoutSystem`, `LocationMemorySystem`, `SiteEncounterSystem`, and a
headless demo. The concept — a site's persistent record of what happened there
and what that does to future visits — is exactly the kind of state that must
have one owner and a memory-degradation rule, but no plan states either.

| Deliverable | Detail |
|---|---|
| Record model | what a standing record contains (events, dispositions, marks), keyed by site id, with the engine as sole writer |
| Memory semantics | `LocationMemorySystem` decay/fade rules in game days; what never fades (death sites, authored markers) |
| Layout stability | `LocationLayoutSystem` guarantees a returning visitor sees the persisted layout, not a re-roll |
| Encounter triggers | `SiteEncounterSystem` reads the record to gate encounters; a trigger table maps record state → encounter class |
| Save truth | records restore with their section; a re-visit after load matches a visit without a save |

## 2. Evidence
- `Assets/Ashfall.Core/StandingRecord/` file list (verified, six files).
- `StandingRecordHeadlessDemo.cs` provides a headless verification path.
- Plan 95 maps spatial owners; site ids come from there.
- Plan 108 owns discovery/knowledge states; the standing record is site-local history, a different axis.

## 3. Packages
- **SRT-139A** record model + writer rule (one writer).
- **SRT-139B** memory fade rules in game days + never-fade list tests.
- **SRT-139C** layout stability test (save → load → revisit equals continuous visit).
- **SRT-139D** encounter trigger table + fixture per class.
- **SRT-139E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Revisit parity: continuous run vs save/load run produce the same layout and record.
- Fade rules use game days; OS clock change has no effect.
- Every encounter trigger row has a fixture; unmatched state produces no encounter (typed no-op).
- `bash scripts/run_test.sh Ashfall.Core.Tests/StandingRecord/` + the headless demo.

## 5. Risks
Second knowledge system → discovery stays in Plan 108; this record is site history, and the boundary is tested.
Trigger sprawl → the table is closed per encounter class.

---

## 6. Expanded census (5 files · 1,387 lines)

Scope: `Assets/Ashfall.Core/StandingRecord/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · Demo 1 · System 3

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `LocationLayoutSystem.cs` | 517 | System | — | 0 | 0 | 2 |
| `LocationMemorySystem.cs` | 388 | System | **yes** | 0 | 0 | 2 |
| `StandingRecordCatalog.cs` | 161 | Catalog | — | 0 | 0 | 0 |
| `StandingRecordEngine.cs` | 177 | System | **yes** | 0 | 0 | 11 |
| `StandingRecordHeadlessDemo.cs` | 144 | Demo | — | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 4 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `crossing_locations.json` | object[2 keys] |
| `duty_roster_locations.json` | object[2 keys] |
| `locations_expansion3.json` | object[2 keys] |
| `standing_record_layouts.json` | array[14] |
| `standing_record_memory.json` | array[52] |
| `year_of_ash_locations.json` | object[2 keys] |

**State surfaces:** `LocationLayoutSystem.cs`, `LocationMemorySystem.cs`, `StandingRecordEngine.cs`, `StandingRecordHeadlessDemo.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/StandingRecord/` (create if absent) |
| Test references | 24 name references across the test tree |
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

Domain files: 5. Other plans referencing their names: **0**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| — | no other plan references these files |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `SRT-139A` | `StandingRecordCatalog.cs`, `StandingRecordEngine.cs`, `StandingRecordHeadlessDemo.cs` |
| `SRT-139B` | `LocationMemorySystem.cs` |
| `SRT-139C` | `LocationLayoutSystem.cs` |
| `SRT-139D` | no name match — resolve at claim time |
| `SRT-139E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 5. Host files: **8** · Test files: **14** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 8 | `src/Host/CoreDemoSession.cs`, `src/Host/ExpansionHostSession.cs`, `src/Host/HostCli.ExpansionDepth.cs`, `src/Host/HostCli.SelfTests.cs`, `src/Host/HostCli.cs` |
| Tests (`Ashfall.Core.Tests/`) | 14 | `Ashfall.Core.Tests/Content/Plan41_45MemoryAcceptanceIntegrationTests.cs`, `Ashfall.Core.Tests/CrossingQuestSystemTests.cs`, `Ashfall.Core.Tests/ExpansionAggregateCompletenessTests.cs`, `Ashfall.Core.Tests/ExpansionHubSaveTests.cs`, `Ashfall.Core.Tests/ExpansionHubSaveV5Tests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 5 files; intra-domain edges: **5**; isolated: **0**.

| From | → To |
|---|---|
| `StandingRecordEngine` | `LocationLayoutSystem` |
| `StandingRecordEngine` | `LocationMemorySystem` |
| `StandingRecordHeadlessDemo` | `LocationLayoutSystem` |
| `StandingRecordHeadlessDemo` | `LocationMemorySystem` |
| `StandingRecordHeadlessDemo` | `StandingRecordCatalog` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **1** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `phantom_memory` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **4** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--ice-road-tick-demo` |
| `--layout-selftest` |
| `--standing-record-selftest` |
| `--ui-layout-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **8**.

| Event | First declaration |
|---|---|
| `OnFactionStandingChanged` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` |
| `OnLayoutMutated` | `Assets/Ashfall.Core/StandingRecord/LocationLayoutSystem.cs` |
| `OnLocationMutated` | `Assets/Ashfall.Core/LocationEvolutionSystem.cs` |
| `OnLocationOwnerChanged` | `Assets/Ashfall.Core/LocationEvolutionSystem.cs` |
| `OnLocationRecast` | `Assets/Ashfall.Core/StandingRecord/LocationMemorySystem.cs` |
| `OnLocationRevealed` | `Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs` |
| `OnStandingCalled` | `Assets/Ashfall.Core/CrossingArbitrationSystem.cs` |
| `OnStandingPenalty` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **8**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/faction_war_location_overrides.json` |
| `Assets/StreamingAssets/Data/memory_decay_rates.json` |
| `Assets/StreamingAssets/Data/npc_memory_dialogue.json` |
| `Assets/StreamingAssets/Data/standing_gates.json` |
| `Assets/StreamingAssets/Data/standing_record_factions.json` |
| `Assets/StreamingAssets/Data/standing_record_layouts.json` |
| `Assets/StreamingAssets/Data/standing_record_memory.json` |
| `Assets/StreamingAssets/Data/standing_record_quests.json` |

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

Host files (`src/`) whose names share a domain token: **11**
(3 of them panels/HUD).

| Host file |
|---|
| `src/Host/CoreDemoSession.cs` |
| `src/Host/PhantomMemoryHostSession.cs` |
| `src/Host/PhantomMemorySaveStore.cs` |
| `src/Host/StandingRecordHostSession.cs` |
| `src/Journal/JournalDemoHarness.cs` |
| `src/UI/PhantomMemoryPanel.cs` |
| `src/UI/SceneBindingHeadlessProbe.cs` |
| `src/UI/StandingRecordAtlasPanel.cs` |
| `src/UI/StandingRecordPanel.cs` |
| `src/World/MapLocationMarkerView.cs` |
| `src/World/MapLocationMarkerView.tscn` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **1**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `phantom_memory` | no |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **5**
(GAMEPLAY_CONSUMED 5).

| Catalog | Classification |
|---|---|
| `faction_war_location_overrides.json` | GAMEPLAY_CONSUMED |
| `standing_record_factions.json` | GAMEPLAY_CONSUMED |
| `standing_record_layouts.json` | GAMEPLAY_CONSUMED |
| `standing_record_memory.json` | GAMEPLAY_CONSUMED |
| `standing_record_quests.json` | GAMEPLAY_CONSUMED |

**Verdict:** matched catalogs are classified as gameplay-consumed — the data is reachable.

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
**Surface:** save sections 1 (laddered 0) · RNG streams 0 · host files 11 · catalogs 13 · test regions 0 · flags 4

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-STANDING-RECORD-TRUTH-139
wave: 11
status: PROPOSED — foreman claim required
packages: SRT-139A, SRT-139B, SRT-139C, SRT-139D, SRT-139E
claim paths:
  - src/Host/CoreDemoSession.cs  # §19 candidate host surface
  - src/Host/PhantomMemoryHostSession.cs  # §19 candidate host surface
  - src/Host/PhantomMemorySaveStore.cs  # §19 candidate host surface
  - src/Host/StandingRecordHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/faction_war_location_overrides.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/memory_decay_rates.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --ice-road-tick-demo
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
