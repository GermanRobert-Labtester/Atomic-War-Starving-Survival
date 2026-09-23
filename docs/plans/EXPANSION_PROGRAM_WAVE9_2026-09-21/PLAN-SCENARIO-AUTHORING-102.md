# PLAN-SCENARIO-AUTHORING-102 — Slice Scenario Authoring, Validation & Sandbox Entry

**Wave 9 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-AUTOMATED-QA-CAMPAIGNS-74, PLAN-DEV-TOOLING-TRUTH-75, PLAN-DATA-SCHEMA-COVERAGE-90.
**Non-goals:** no second campaign format, no scripting language, no runtime
authoring UI.

## 1. Outcome
`Assets/Ashfall.Core/Campaign/SliceScenario.cs` already models a bounded
scenario slice with zero engine references. What is missing is the authoring
loop around it: a validated scenario document (schema-checked, not free-form),
a sandbox entry that runs one slice headlessly and in-game, and a fixture
corpus the QA campaigns (Plan 74) can rotate. Today a scenario is only as good
as the code that constructs it in a test.

| Deliverable | Detail |
|---|---|
| Scenario document | schema-checked JSON: seed, start state, length, focus, expected invariants — validated by the Plan 90 schema stage |
| Loader path | loader through the existing data authority; no parallel state; scenario state is input, never authority |
| Sandbox runs | one verb: headless slice run with per-day dump; in-game entry for manual inspection |
| Fixture corpus | 3–5 named slices (quiet week, supply shock, casualty event) with recorded expected outcomes |
| Authoring guide | one page: what a slice may set, what it must not (no saves, no RNG streams, no real content ids beyond catalogs) |

## 2. Evidence
- `Assets/Ashfall.Core/Campaign/SliceScenario.cs` ("Zero engine references" in its own contract comment).
- Plan 74 owns the scenario matrix; this plan supplies validated documents and the sandbox entry it runs.
- `DutyRosterHeadlessDemo.cs` and `EconomyHeadlessDemo.cs` are precedent headless demos — the slice runner follows that pattern.
- Plan 90's validator stage is the schema home; this plan does not add one.

## 3. Packages
- **SCA-102A** scenario schema + two example documents.
- **SCA-102B** loader + validator wiring through the data authority.
- **SCA-102C** headless sandbox verb with per-day invariant dump.
- **SCA-102D** fixture corpus + expected-outcome records.
- **SCA-102E** guide + registration in Plan 74's rotation list.

## 4. Acceptance & verification
- A malformed scenario fails with the field path named; a valid one runs.
- Two runs of one fixture: identical dumps (seed-stable).
- A scenario cannot mutate leaves: run a slice, save, compare checksums with a control run where the slice was absent and no writes occurred.
- `bash scripts/run_test.sh` on the scenario test region + one headless slice run.

## 5. Risks
Scenario becoming a second save format → it is input-only; the no-writes check guards it.
Fixture sprawl → corpus is bounded and named; additions require an expected-outcome row.

---

## 6. Expanded census (1 files · 227 lines)

Scope: `Assets/Ashfall.Core/Campaign/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `SliceScenario.cs` | 227 | Support | **yes** | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `trade_screen_scenarios.json` | object[5 keys] |
| `slice_seven_days.json` | object[10 keys] |

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Campaign/` |
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

Domain files: 1. Other plans referencing their names: **2**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `EVIDENCE` | 1 |
| `PLAN-CAMPAIGN-FAMILY-TRUTH-272` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `SCA-102A` | `SliceScenario.cs` |
| `SCA-102B` | no name match — resolve at claim time |
| `SCA-102C` | no name match — resolve at claim time |
| `SCA-102D` | no name match — resolve at claim time |
| `SCA-102E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **3** · Test files: **3** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 3 | `src/Host/HostCli.PanelTests.cs`, `src/Host/HostCli.SelfTests.cs`, `src/Host/HostCli.cs` |
| Tests (`Ashfall.Core.Tests/`) | 3 | `Ashfall.Core.Tests/Campaign/Plan54SevenDaySliceIntegrationTests.cs`, `Ashfall.Core.Tests/ExpansionAggregateCompletenessTests.cs`, `Ashfall.Core.Tests/ExpansionsIntegrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 3 files; intra-domain edges: **0**; isolated: **3**.

| From | → To |
|---|---|
| — | no intra-domain references found |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **3** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `contractor_roster` |
| `duty_roster` |
| `economy` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **7** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--duty-roster-loop-selftest` |
| `--duty-roster-save-selftest` |
| `--duty-roster-selftest` |
| `--duty-roster-uitest` |
| `--economy-selftest` |
| `--economy-uitest` |
| `--ice-road-tick-demo` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **5**.

| Event | First declaration |
|---|---|
| `OnDutyVacated` | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` |
| `OnEconomyChanged` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnRosterBurned` | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` |
| `OnRosterChanged` | `Assets/Ashfall.Core/ContractorRosterSystem.cs` |
| `OnRosterUpdated` | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **9**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/duty_roles.json` |
| `Assets/StreamingAssets/Data/duty_roster_locations.json` |
| `Assets/StreamingAssets/Data/duty_roster_marks.json` |
| `Assets/StreamingAssets/Data/duty_roster_quests.json` |
| `Assets/StreamingAssets/Data/duty_roster_seasons.json` |
| `Assets/StreamingAssets/Data/economy_goods.json` |
| `Assets/StreamingAssets/Data/hardcore_economy_tuning.json` |
| `Assets/StreamingAssets/Data/radiation_economy_social.json` |
| `Assets/StreamingAssets/Data/slice_seven_days.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **2** (46 files, 378 cases).

| Region | Files | Cases |
|---|---:|---:|
| `DutyRoster` | 5 | 49 |
| `Economy` | 41 | 329 |

**Verdict:** 378 cases sit under matching regions — run those first (`DutyRoster`, `Economy`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **19**
(5 of them panels/HUD).

| Host file |
|---|
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Host/ContractorRosterHostSession.cs` |
| `src/Host/CoreDemoSession.cs` |
| `src/Host/DutyRosterHostSession.cs` |
| `src/Host/DutyRosterSaveStore.cs` |
| `src/Host/EconomyHostSession.cs` |
| `src/Host/EconomySaveStore.cs` |
| `src/Journal/JournalDemoHarness.cs` |
| `src/Main.DutyRoster.cs` |
| `src/Main.Economy.cs` |
| `src/Main.UiTests.DutyRoster.cs` |
| `src/Main.UiTests.Economy.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **3**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `contractor_roster` | no |
| `duty_roster` | no |
| `economy` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **2**.

| Stream |
|---|
| `duty_roster` |
| `economy` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **6**
(GAMEPLAY_CONSUMED 6).

| Catalog | Classification |
|---|---|
| `duty_roster_locations.json` | GAMEPLAY_CONSUMED |
| `duty_roster_marks.json` | GAMEPLAY_CONSUMED |
| `duty_roster_quests.json` | GAMEPLAY_CONSUMED |
| `duty_roster_seasons.json` | GAMEPLAY_CONSUMED |
| `economy_goods.json` | GAMEPLAY_CONSUMED |
| `hardcore_economy_tuning.json` | GAMEPLAY_CONSUMED |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 2
**Surface:** save sections 3 (laddered 0) · RNG streams 2 · host files 14 · catalogs 15 · test regions 2 · flags 7

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-SCENARIO-AUTHORING-102
wave: 9
status: PROPOSED — foreman claim required
packages: SCA-102A, SCA-102B, SCA-102C, SCA-102D, SCA-102E
claim paths:
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Host/ContractorRosterHostSession.cs  # §19 candidate host surface
  - src/Host/CoreDemoSession.cs  # §19 candidate host surface
  - src/Host/DutyRosterHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/duty_roles.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/duty_roster_locations.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/DutyRoster/
  - godot --headless --path . -- --duty-roster-loop-selftest
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
