# ASHFALL — Quality Roadmap Batch 83

## Theme: Automated Balance Testing — Statistical Simulation for Game Design Validation

| Field | Value |
|-------|-------|
| **Priority** | MEDIUM — but currently **BLOCKED**, see below |
| **Risk** | Low, not None — see Risks & Mitigations (updated) |
| **Depends on** | Deterministic simulation (ISeededRng, Invariant 4) — real and verified; **`GameSessionTestHarness` (Batch 63) — NOT YET BUILT, see Blocking Dependency below** |
| **Touches** | `Ashfall.Core.Tests/` (new balance test project or folder), `Assets/Ashfall.Core/` (read-only metrics interfaces) |
| **Engine coupling** | Zero — all simulation runs through Core systems with `ISeededRng` |

---

## ⚠️ BLOCKING DEPENDENCY — VERIFIED NOT TO EXIST

This batch's entire foundation (Step 1's `BalanceTestRunner` "constructs a `GameSessionTestHarness` per seed") assumes `GameSessionTestHarness` from Batch 63 is a real, working class. **It is not.** Verified directly against the repository at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`:

- `grep -r "GameSessionTestHarness"` across the entire tree returns **zero matches**. The class does not exist anywhere — not in `Ashfall.Core.Tests/`, not in `Assets/Ashfall.Core/`, not in `src/`.
- No `Ashfall.Core.Tests/Integration/` directory exists at all (confirmed subfolders in `Ashfall.Core.Tests/` today: `Campaign/`, `Economy/`, `Endgame/`, `Expeditions/`, `Foundry/`, `Medical/`, `Memorial/`, `Radio/`, `Shelter/`, `Survivors/`, `Warlords/`, `World/` — no `Integration/`).
- Reading Batch 63's own plan file confirms this is expected, not a surprise: Batch 63 Step 1 ("Design GameSessionTestHarness") is a *design* step, and Step 2 ("Implement Minimal GameSessionTestHarness") — the step that would actually produce working code — is explicitly called out in Batch 63's own text as **"the highest-complexity item in the entire batch,"** budgeted at "2-3x the effort of any other single step," with an open risk that some of the ~20-24 systems "may not construct cleanly outside the Godot host without adaptation," which could turn Batch 63 from a test-only batch into one that also requires production-code changes. Batch 63's realistic total effort estimate is 5-9 sessions, and nothing in this repository indicates any of it has shipped.

**Conclusion: Batch 83 depends on speculative, unimplemented infrastructure from another still-open plan, not a completed prerequisite.** This is not a paperwork gap — `BalanceTestRunner.RunAll()` (Step 1) literally cannot compile or run without a real `GameSessionTestHarness` class exposing at minimum a constructible instance and a `TickDay(int)`-shaped method. Every downstream step in this batch (Steps 2-7) inherits the same block, since they all consume `BalanceTestRunner`'s output.

**Required action before starting this batch:** Batch 63 Steps 1-2 must ship and merge first (harness compiles, `CreateMinimal()`/`CreateFull()` both construct without throwing, per Batch 63's own Done-when criteria). Do not start Batch 83 Step 1 until that lands. If Batch 63 stalls or its `CreateFull()` ships as a documented partial subset (a real possibility per Batch 63's own risk notes), Batch 83's strategy (`GreedySurvivalStrategy`, Step 3) must be re-scoped to whichever subset of systems the harness actually supports — "exercises at least 5 distinct Core systems" (Step 3's Done-when) needs re-validation against whatever `CreateFull()`/`CreateMinimal()` actually construct once Batch 63 lands, not the aspirational ~20-24-system list.

---

## Motivation

With 82+ interacting systems simulating 300+ days of post-nuclear survival, emergent balance issues are invisible without statistical analysis. The project already guarantees determinism (same seed = same outcome), which means automated playthroughs can be repeated with different seeds to gather distributions. Currently all tuning is manual — no one knows:

- What the average survival duration is across seeds
- Whether certain strategies dominate all others
- Whether specific seeds are unwinnable
- How death cause distributions shift when parameters change
- Whether difficulty scaling produces the intended survival curves

The Integration Test Harness (`GameSessionTestHarness`) from Batch 63 is intended to provide the foundation, but **as of this review it exists only as a design in Batch 63's plan document, not as code.** This batch builds the statistical layer on top of infrastructure that must be built first, by a different batch, before any of the steps below are executable.

---

## Step 1 — Design BalanceTestRunner

### Goal

Create a `BalanceTestRunner` class that orchestrates N simulated game sessions with different seeds, collects per-session outcome metrics, and aggregates results into a statistical summary.

### Prerequisite gate (do not skip)

Before writing any code in this step, confirm Batch 63's `GameSessionTestHarness` actually exists and exposes a constructible factory (`CreateMinimal()`/`CreateFull()`) plus a `TickDay(int)`-shaped method, by re-running `grep -r "GameSessionTestHarness" Ashfall.Core.Tests/`. If it still returns zero matches, stop — this step is not implementable and should not be attempted. Do not write a placeholder/mock harness to unblock this step; that would silently decouple the balance runner from the real simulation, defeating the entire purpose of the batch (statistically validating actual game balance, not a stub's balance).

### Implementation

- New file: `Ashfall.Core.Tests/Balance/BalanceTestRunner.cs`
- Constructor accepts: `int seedCount`, `int maxDays`, `IBalanceStrategy strategy`, `BalanceMetricsCollector collector`
- `RunAll()` iterates seeds 1..N, constructs a `GameSessionTestHarness` per seed (via whichever factory Batch 63 actually shipped — `CreateMinimal()` or `CreateFull()`; confirm which one is appropriate before assuming `CreateFull()`, since Batch 63's own risk notes admit `CreateFull()` may ship as a documented partial subset if some systems can't construct standalone), ticks the session `maxDays` times (or until survivor death), records metrics per session
- Uses `ISeededRng` with sequential seeds for reproducibility
- Returns `BalanceRunResult` containing per-session records and aggregate statistics
- Must be single-threaded to preserve determinism guarantees (parallelism optional future enhancement with seed partitioning)
- No engine references — pure Core + test infrastructure
- "Survivor death" as an early-termination condition assumes a single-survivor-death-ends-session model. Verify this matches the real Core survival model before implementing: the project supports multiple survivors per holdfast (see `Ashfall.Core.Tests/Survivors/`), so "session ends on death" may need to be "session ends when the holdfast has zero living survivors" or some other project-specific terminal condition — confirm the actual end-condition semantics against the harness's exposed state before hardcoding this.

### Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~BalanceTestRunner"
```

These commands are only runnable once `GameSessionTestHarness` exists (see Prerequisite gate above) — running them against the current repository state will fail at compile time with "type or namespace not found," not a test failure.

### Done when

- `GameSessionTestHarness` (Batch 63) is confirmed present and constructible in the current tree (re-verified, not assumed from this document)
- `BalanceTestRunner` compiles and can execute 10 seeds against a minimal harness configuration, producing 10 distinct `SessionMetrics` records with 10 distinct seed values (not just "runs without throwing" — assert the seed field on each record matches 1..10)
- Each session produces a `SessionMetrics` record with at minimum: seed, survival_days, death_cause, final_state — verified by a unit test that asserts none of these four fields are null/default after a single-session run
- Runner respects `maxDays` ceiling and early-termination on the project's actual terminal condition (see implementation note above), verified by one test that runs to natural death before `maxDays` and one that hits the `maxDays` ceiling with the session still alive

### Risk / Rollback

Low, contingent entirely on Batch 63 landing cleanly. This step adds only new test-only files with no production callers — if `BalanceTestRunner` needs to change after Batch 63's harness API stabilizes, the fix is contained to this one file with no downstream production impact. The real risk is schedule risk, not code risk: if Batch 63's harness API changes after this step is written against an earlier version, this file (and only this file, since nothing outside `Ashfall.Core.Tests/Balance/` depends on it) needs updating.

---

## Step 2 — Define Balance Metrics Schema

### Goal

Define the complete set of per-session and aggregate metrics that the balance system collects, covering survival duration, resource economy, faction dynamics, and system engagement.

### Implementation

- New file: `Ashfall.Core.Tests/Balance/BalanceMetrics.cs`
- Per-session metrics DTO (`SessionMetrics`):
  - `int Seed`
  - `int SurvivalDays` (day of death or maxDays if survived)
  - `string DeathCause` (hunger, thirst, radiation, combat, exposure, morale_collapse, none)
  - `float ResourceSurplusDay30` (net resource balance at day 30: food + water + fuel weighted)
  - `float FactionStandingDay100` (average faction reputation at day 100, or at death if earlier)
  - `int SystemsInteracted` (count of distinct Core systems that fired at least one meaningful event)
  - `int CraftsCompleted`
  - `int ExpeditionsLaunched`
  - `float PeakRadiation` (highest accumulated dose before death/end)
  - `int MedicalEventsTriggered`
  - `bool SurvivedToEnd` (reached maxDays alive)
- Aggregate metrics DTO (`BalanceRunResult`):
  - `int TotalSessions`
  - `float MedianSurvivalDays`
  - `float MeanSurvivalDays`
  - `float P10SurvivalDays` / `float P90SurvivalDays`
  - `Dictionary<string, int> DeathCauseDistribution`
  - `float SurvivalRate` (percentage that reached maxDays)
  - `List<SessionMetrics> AllSessions`
  - `List<int> OutlierSeeds` (sessions below P5 or above P95)
- All DTOs are plain C#, `[Serializable]`, no engine references

### Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
```

### Done when

- Metrics DTOs compile with no engine coupling
- `BalanceRunResult` can compute all aggregate fields from a list of `SessionMetrics`
- Percentile calculation uses sorted array indexing (no LINQ dependency on external stats library)

---

## Step 3 — Implement AI Player Strategy

### Goal

Create a simple greedy AI strategy that makes survival decisions each tick, providing a baseline "reasonably competent player" for balance testing. This is not game AI — it is a test fixture that exercises the systems.

### Implementation

- New file: `Ashfall.Core.Tests/Balance/GreedySurvivalStrategy.cs`
- Implements `IBalanceStrategy` interface:
  ```csharp
  public interface IBalanceStrategy
  {
      void DecideActions(GameState state, IActionQueue queue);
  }
  ```
- **Unverified API assumption:** this interface references a `GameState` type and an `IActionQueue` type. Neither name was found in a repository search performed for this review (`grep -r "class GameState\|interface IActionQueue" Assets/Ashfall.Core/` returns no matches). Before implementing, determine what state/action surface `GameSessionTestHarness` (once it exists per Batch 63) actually exposes — it may be per-system method calls (e.g. call `_needs.Eat(...)` directly) rather than a unified `GameState`/`IActionQueue` abstraction. Design this interface against the harness's real exposed surface, not against a hypothetical unified state object that may not exist and would itself be new production-shaped code requiring its own design step.
- Greedy priority order each tick:
  1. If hunger > 70% → eat best available food
  2. If thirst > 70% → drink best available water
  3. If radiation > threshold → take anti-rad if available
  4. If fatigue > 80% → rest
  5. If warmth < 30% → equip warmest gear or seek shelter
  6. If craftable recipe available with materials on hand → craft highest-priority item
  7. If expedition available and health > 50% → launch expedition
  8. Otherwise → idle (advance time)
- Strategy never makes "perfect" decisions — it uses the first valid option, not the optimal one
- Strategy must work entirely through Core system APIs (no engine calls)
- Add `RandomizedStrategy` variant that adds noise to thresholds (±15%) using the session's `ISeededRng`

### Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~GreedySurvivalStrategy"
```

Blocked on the same `GameSessionTestHarness` prerequisite as Step 1 — these commands will fail to compile until Batch 63 lands.

### Done when

- `GreedySurvivalStrategy` can drive a test harness session for 30+ days without crashing
- Strategy exercises at least 5 distinct Core systems (needs, inventory, crafting, expedition, medical) — verified by asserting on the harness's actual system-engagement tracking (once designed; this batch's Step 2 defines `SystemsInteracted`, so this criterion is circular with Step 2's deliverable and should be verified after Step 2 lands, not before)
- `RandomizedStrategy` produces different action sequences for different seeds — verified directly (e.g. hash the action log per seed and assert the hashes differ across at least 3 of 3 test seeds)
- The "converges on similar survival outcomes" claim (below) is removed from this Done-when list; see Risk note

### Risk / Rollback

Low code risk (test-only), but one design claim is unverifiable in advance and should not be a Done-when gate: "`RandomizedStrategy` produces different action sequences for different seeds but converges on similar survival outcomes" is an empirical claim about the actual tuned game balance, not something that can be asserted as a pass/fail criterion before the balance data exists (that's what Steps 4-5 exist to discover). Treat convergence as an observation to make after running Step 4's simulation, not a precondition this step must satisfy before merging.

---

## Step 4 — Run 1000-Seed Simulation

### Goal

Execute the balance runner with 1000 seeds, collect the full distribution of outcomes, and produce a raw results file that can be analyzed offline.

### Implementation

- New file: `Ashfall.Core.Tests/Balance/BalanceSimulationTests.cs`
- Test method `[Fact] RunFullBalanceSimulation_1000Seeds()`:
  - Configures `BalanceTestRunner` with seedCount=1000, maxDays=365, strategy=GreedySurvivalStrategy
  - Runs all sessions (wall time is unknown and unverifiable until the harness exists and its per-tick cost is measured; do not carry forward a specific "under 60 seconds" estimate with no basis — replace with "measure actual wall time on first successful run and record it as the baseline for the 120-second regression threshold below")
  - Serializes `BalanceRunResult` to JSON at `Ashfall.Core.Tests/Balance/Results/latest_run.json`
  - Logs summary to test output: median survival, survival rate, top 3 death causes
- Test is marked `[Trait("Category", "Balance")]` — **note:** Batch 63's own review found zero existing uses of `[Trait]` anywhere in this test suite (confirmed by repo-wide grep as part of that batch's review). Adopting `[Trait]` here introduces a new convention inconsistent with the rest of the suite, which organizes exclusively by namespace/class via `--filter "FullyQualifiedName~..."` (as this very document's other verification commands do). Either commit to `[Trait]` consistently for every new Balance-suite test in this batch, or drop it and use `--filter "FullyQualifiedName~BalanceSimulationTests"` like every other step in this document already does — do not mix both conventions within one batch.
- If wall time exceeds 120 seconds, the test fails with a performance regression message — this number is currently an assumption with no measurement behind it (Core-only simulation could still be slow if e.g. JSON catalog files are re-read from disk every tick rather than cached; Batch 63's own risk notes flag exactly this possibility). Treat 120s as a placeholder to replace with a real measured threshold once the harness exists.
- Results JSON uses `System.Text.Json` (engine-agnostic), snake_case property naming

### Verification

```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~Category=Balance"
```

Note: xUnit trait filters use `--filter "Category=Balance"`, not `FullyQualifiedName~Category=Balance` — corrected below. Also blocked on Steps 1-3 landing first, which are themselves blocked on Batch 63.

```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "Category=Balance"
```

### Done when

- 1000-seed run completes and its actual wall time is recorded (not assumed); a regression threshold is set at 2x that measured value, not a pre-guessed "120 seconds"
- `latest_run.json` contains 1000 session records with all metrics populated (verified: none of the `SessionMetrics` fields are default/null across all 1000 records — spot-checking a sample is not sufficient given this is meant to be a statistically rigorous tool)
- No session throws an unhandled exception (all failures are captured as death causes)
- Console output shows human-readable summary statistics

### Risk / Rollback

Medium, not previously called out: a 1000-seed run against ~20-24 systems per Batch 63's harness is a meaningfully expensive test to run in CI on every commit. If it turns out to be too slow for regular CI (plausible per Batch 63's own performance risk notes), it must stay gated behind the opt-in `Category=Balance` trait (or equivalent) rather than running by default — confirm this gating actually works (a `dotnet test` with no filter should NOT run this test) before considering this step done, since an accidentally-always-on 1000-seed test would make ordinary `dotnet test` runs unacceptably slow for every future batch's own verification commands.

---

## Step 5 — Add Statistical Assertions

### Goal

Define quantitative balance invariants that encode design intent, and assert them against the simulation results. These catch regressions where a code change inadvertently makes the game too easy, too hard, or degenerate.

### Implementation

- New file: `Ashfall.Core.Tests/Balance/BalanceAssertions.cs`
- Static assertion methods — **the specific numeric thresholds below are illustrative placeholders, not tuned values; they must be derived from Step 4's actual 1000-seed output, not invented in advance.** Setting `AssertMedianSurvival(result, minDays: 45, maxDays: 200)` before any real simulation has run risks locking in an arbitrary band that has no relationship to the game's actual tuning intent:
  - `AssertMedianSurvival(result, minDays, maxDays)` — game shouldn't be trivial or impossible; set bounds from Step 4's observed distribution plus design-intent headroom, not a guessed 45-200
  - `AssertNoSingleDeathCauseDominates(result, maxPercent: 60)` — if >60% die to one cause, that system is overtuned
  - `AssertNoUnwinnableSeedsInFirstNDays(result, days: 30, maxUnwinnable: 0)` — every seed must be survivable for 30 days with greedy play
  - `AssertSurvivalRateInRange(result, min, max)` — neither everyone dies nor everyone lives to 365; set from Step 4's data
  - `AssertResourceEconomyNotDegenerate(result, maxSurplusDay30)` — hoarding shouldn't be trivial; set from Step 4's data
  - `AssertSystemEngagement(result, minSystemsMedian: 4)` — the AI should be forced to interact with multiple systems
- New test: `[Fact] BalanceInvariants_HoldAcrossSeeds()` runs 100 seeds (fast CI) and asserts all invariants
- Thresholds are constants in a `BalanceThresholds` static class for easy tuning

### Verification

```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~BalanceInvariants"
```

### Done when

- All assertions pass against the 1000-seed baseline from Step 4 (not an assumed baseline — this step must run after Step 4 has produced real data, and its threshold constants must be filled in from that data, not left as this document's placeholder numbers)
- If an assertion fails, the error message names the specific threshold violated and the actual value
- Thresholds are documented with rationale comments explaining why each bound exists, sourced from the Step 4 run that justified them (cite the run's date/commit in the comment)

### Risk / Rollback

Medium: this step is where "the game is unbalanced" and "the test is wrong" are easy to confuse. If a threshold fails on first run, the default response must be to inspect whether the threshold was set correctly from real Step 4 data (per the note above) before concluding the game itself needs tuning — a rushed threshold guess turning into a false CI failure is the most likely early failure mode here.

---

## Step 6 — Add Balance Regression Test for CI

### Goal

Add a fast (100-seed) balance regression test to the standard CI pipeline that detects when a code change shifts the survival distribution beyond acceptable tolerance, without requiring the full 1000-seed run.

### Implementation

- New file: `Ashfall.Core.Tests/Balance/BalanceRegressionTests.cs`
- `[Fact] BalanceRegression_100Seeds_WithinTolerance()`:
  - Runs 100 seeds (deterministic: seeds 1-100, always the same)
  - Compares results against a checked-in baseline: `Ashfall.Core.Tests/Balance/Baselines/regression_baseline.json`
  - Tolerance bands:
    - Median survival: ±10 days from baseline
    - Survival rate: ±10 percentage points
    - Each death cause: ±15 percentage points
    - No new "unwinnable in 30 days" seeds that weren't in baseline
  - If distribution shifts beyond tolerance, test fails with a diff report
- Baseline update workflow:
  - Run `dotnet test --filter "FullyQualifiedName~UpdateBaseline"` to regenerate baseline after intentional balance changes
  - `[Fact][Trait("Category", "BaselineUpdate")] UpdateRegressionBaseline()` writes new baseline and skips assertions
- CI integration: the regression test runs as part of the normal `dotnet test` pass (Category=Balance is opt-in for the full 1000-seed; regression is always-on)

### Ordering note (fixes a circular dependency in the original plan)

`BalanceRegression_100Seeds_WithinTolerance()` compares against `regression_baseline.json`, but that file doesn't exist until `UpdateRegressionBaseline()` runs once, successfully, against a version of the game considered "correctly balanced." This step therefore has an inherent bootstrap order: (1) implement both tests, (2) run `UpdateRegressionBaseline()` once by hand and commit the resulting `regression_baseline.json`, (3) only then does `BalanceRegression_100Seeds_WithinTolerance()` have anything to compare against. The original plan's Done-when list implicitly assumed the baseline already existed ("Baseline file is checked in and the test passes against it") without stating this bootstrap step — added explicitly below. Also: because `[Trait("Category", "BaselineUpdate")]` introduces yet another new trait category (in addition to Step 4's `Category=Balance`), confirm both trait categories are excluded from a default `dotnet test` run — `UpdateRegressionBaseline()` writing over the committed baseline as a side effect of an ordinary CI run would be a silent, hard-to-notice bug that corrupts the regression baseline.

### Verification

```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~BalanceRegression"
```

### Done when

- The bootstrap sequence above has been performed at least once and `regression_baseline.json` is committed (this is a precondition, not something the test itself can satisfy)
- 100-seed regression test's actual run time is measured and recorded (do not assume "under 15 seconds" — Step 4 established that wall time for this simulation is unmeasured until the harness exists; carry the same caveat here)
- Baseline file is checked in and the test passes against it
- Intentionally breaking a balance parameter (e.g., doubling hunger rate) causes the test to fail with a clear diff
- Baseline update mechanism works, produces a reproducible file, and is confirmed excluded from default `dotnet test` runs (verify by running plain `dotnet test` with no filter and confirming `regression_baseline.json`'s git diff is empty afterward)

### Risk / Rollback

Medium: an incorrectly-generated first baseline (e.g. captured against a build with an undiscovered bug) becomes the "ground truth" every future run is compared against, silently normalizing a bug as expected behavior. Mitigation: generate the initial baseline only after Step 5's statistical assertions pass against the same run (i.e. the baseline-generating run must itself be a run with no known balance defects), and have a second person/tool spot-check the generated `regression_baseline.json`'s summary numbers against Step 4's report before committing it.

---

## Step 7 — Add Balance Report Generator

### Goal

Generate a human-readable markdown report from simulation results, including distribution visualizations (ASCII histograms), percentile tables, outlier analysis, and actionable recommendations for designers.

### Implementation

- New file: `Ashfall.Core.Tests/Balance/BalanceReportGenerator.cs`
- `GenerateReport(BalanceRunResult result, string outputPath)`:
  - Outputs markdown with sections:
    1. **Summary** — seed count, median/mean/P10/P90 survival, survival rate
    2. **Survival Distribution** — ASCII histogram (10-day buckets, 50-char width)
    3. **Death Cause Breakdown** — table with count, percentage, median day-of-death per cause
    4. **Resource Economy** — day-30 surplus distribution (P10/P25/P50/P75/P90)
    5. **System Engagement** — which systems get used most/least
    6. **Outlier Seeds** — list of seeds below P5 and above P95, with their death cause and day
    7. **Recommendations** — auto-generated notes (e.g., "radiation accounts for 45% of deaths — consider reducing fallout frequency or increasing anti-rad availability")
  - ASCII histogram implementation: bucket counts → normalized bar lengths using `#` characters
  - Recommendations are rule-based (if death_cause > 40% → flag, if median_survival < 60 → flag "too hard", etc.)
- New test: `[Fact] ReportGenerator_ProducesValidMarkdown()` — generates from 50-seed run, asserts output contains all 7 sections
- Report output path: `Ashfall.Core.Tests/Balance/Results/balance_report.md` (gitignored)

### Verification

```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~ReportGenerator"
```

### Done when

- Report generates successfully from any `BalanceRunResult` with ≥10 sessions
- ASCII histograms render correctly with proper alignment
- Recommendations fire appropriately (testable: feed synthetic data with one dominant death cause, verify recommendation appears)
- Report is human-readable and useful without external tooling

---

## Summary Table

| Step | Deliverable | New Files | Key Metric |
|------|-------------|-----------|------------|
| 1 | BalanceTestRunner | `Balance/BalanceTestRunner.cs` | Runs N seeds, collects metrics — **blocked on Batch 63** |
| 2 | Balance Metrics Schema | `Balance/BalanceMetrics.cs` | 10 per-session fields, 8 aggregate fields — not blocked (data shapes only) |
| 3 | AI Player Strategy | `Balance/GreedySurvivalStrategy.cs` | Exercises 5+ systems, survives 30+ days — **blocked on Batch 63** |
| 4 | 1000-Seed Simulation | `Balance/BalanceSimulationTests.cs` | Wall time measured on first run, not assumed; regression threshold = 2x measured; 1000 complete sessions |
| 5 | Statistical Assertions | `Balance/BalanceAssertions.cs` | 6 balance invariants, thresholds derived from Step 4's real data |
| 6 | CI Regression Test | `Balance/BalanceRegressionTests.cs` | 100 seeds, wall time measured not assumed, tolerance-band comparison, baseline bootstrapped once by hand |
| 7 | Report Generator | `Balance/BalanceReportGenerator.cs` | 7-section markdown with histograms + recommendations |

---

## Architecture Diagram

```
┌─────────────────────────────────────────────────────────┐
│                  BalanceTestRunner                        │
│  seeds 1..N  ──►  GameSessionTestHarness (per seed)     │
│                    + IBalanceStrategy (greedy AI)         │
│                    + BalanceMetricsCollector              │
└──────────────────────────┬──────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────┐
│                  BalanceRunResult                         │
│  List<SessionMetrics>  +  Aggregate statistics           │
└──────────┬───────────────────────┬──────────────────────┘
           │                       │
           ▼                       ▼
┌──────────────────┐    ┌─────────────────────────┐
│ BalanceAssertions │    │ BalanceReportGenerator   │
│ (CI invariants)  │    │ (markdown + histograms)  │
└──────────────────┘    └─────────────────────────┘
```

---

## Risks & Mitigations

| Risk | Likelihood | Mitigation |
|------|-----------|------------|
| **`GameSessionTestHarness` (Batch 63) is not built yet — this batch cannot start** | **Certain, today** | **Do not begin any step until Batch 63 Steps 1-2 ship and merge; re-verify with `grep -r "GameSessionTestHarness"` before starting, don't trust this document's word for it by the time work begins** |
| Simulation too slow for 1000 seeds | Unknown (was previously stated as "Low," but no measurement exists — Batch 63's own risk notes suggest per-tick cost could be high if catalog data reloads from disk every tick) | Profile on first real run; batch in 100-seed increments; fail-fast on timeout; do not assume Core-only implies fast |
| AI strategy doesn't exercise enough systems | Medium | Track system engagement metric; tune thresholds iteratively |
| Balance thresholds too tight (false failures) | Medium | Start with wide bands derived from real Step 4 data, not guessed numbers; tighten after 3+ stable runs |
| Determinism violation in some system | Low (Invariant 4 enforced) | If two runs of same seed differ, fail loudly — that's a real bug |
| Batch 63's `CreateFull()` ships as a documented partial subset, not all ~20-24 systems | Medium (explicitly flagged as a real possibility in Batch 63's own plan) | Re-scope Step 3's "exercises at least 5 distinct Core systems" against whatever subset actually ships; do not treat the aspirational full-system list as guaranteed |
| Incorrectly-generated regression baseline (Step 6) normalizes a bug as "expected" | Medium | Generate the initial baseline only after Step 5's assertions pass clean; spot-check summary numbers before committing |

---

## Notes

- All code lives in `Ashfall.Core.Tests/Balance/` — zero production code changes required, **conditional on Batch 63's harness not requiring any production-code change to construct standalone** (Batch 63's own Step 2 risk notes flag this as a real possibility — if it happens, that finding belongs to Batch 63, but this batch inherits the same conditional until Batch 63 resolves it)
- No engine coupling: simulation runs through Core systems only
- Results files (`latest_run.json`, `balance_report.md`) are gitignored; only the baseline is committed
- The 1000-seed test is opt-in (`Category=Balance`, adopted here as this batch's first use of the `[Trait]` convention — see Step 4's note on trait-vs-filter consistency); the 100-seed regression is always-on, but only after its baseline is bootstrapped once (see Step 6's ordering note)
- Future enhancement: parameterized strategies (aggressive, cautious, pacifist) to test strategy diversity

---

## Review Notes (Corrected)

This plan was adversarially reviewed against the real repository at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War` and corrected in place. Findings:

1. **Critical blocking dependency, previously understated.** The original header listed "GameSessionTestHarness (Batch 63)" as a dependency with **Risk: None**. Verified via repo-wide search (`grep -r "GameSessionTestHarness"`) that this class does not exist anywhere in the codebase — no `Integration/` folder even exists under `Ashfall.Core.Tests/` yet. Reading Batch 63's own plan document confirms this is expected: Batch 63's Step 2 (the step that would actually implement the harness) is explicitly flagged in that document as "the highest-complexity item in the entire batch," not yet done, with its own open risk that some systems may not construct standalone without production-code changes. Batch 83 as originally written treated a speculative, unbuilt dependency as a solid foundation. Added a top-of-document blocking-dependency section, changed Priority to reflect the block, changed Risk from "None" to "Low" (contingent on Batch 63), and added prerequisite gates to Steps 1 and 3 specifically (the two steps that directly reference the harness).

2. **Unverifiable performance numbers presented as fact.** Step 4 stated "expected wall time: under 60 seconds" and "if wall time exceeds 120 seconds, fail" with no measurement behind either number (impossible to measure — the harness and systems under test don't exist yet). Step 6 stated "runs in under 15 seconds" with the same problem. Corrected both to require measuring actual wall time on first successful run and deriving the regression threshold from that measurement, rather than carrying forward numbers with no basis.

3. **Circular/bootstrap-order bug in Step 6.** The regression test compares against `regression_baseline.json`, which only exists after `UpdateRegressionBaseline()` is run once by hand. The original Done-when list ("baseline file is checked in and the test passes against it") implicitly assumed the baseline already existed without ever stating how it gets created the first time. Added an explicit bootstrap sequence and a risk note about an incorrectly-generated first baseline silently normalizing a bug as "expected behavior."

4. **Unverified type assumptions in Step 3.** `IBalanceStrategy.DecideActions(GameState state, IActionQueue queue)` references `GameState` and `IActionQueue` types that do not exist anywhere in the current codebase (confirmed via search). These may not match whatever surface `GameSessionTestHarness` actually exposes once built. Added a note requiring the interface to be designed against the harness's real exposed API once it exists, not against invented type names.

5. **Unfalsifiable Done-when criterion removed.** Step 3's original Done-when included "`RandomizedStrategy` ... converges on similar survival outcomes," which is an empirical claim about tuned game balance that cannot be verified before any real simulation data exists (that data is what Steps 4-5 are for). Replaced with a directly-testable claim (differing action-sequence hashes across seeds) and moved the convergence claim to an observation to make later, not a merge gate.

6. **Arbitrary numeric thresholds presented as tuned values.** Step 5's `AssertMedianSurvival(result, minDays: 45, maxDays: 200)` and similar thresholds had specific numbers with no stated derivation — they read as if someone had already run the simulation and tuned these, but nothing in the codebase supports that (no prior balance run exists). Corrected to require these numbers be derived from Step 4's actual output, with rationale comments citing the specific run.

7. **Trait convention inconsistency.** Step 4 introduces `[Trait("Category", "Balance")]`; Batch 63's own review (verified by reading that file) found zero existing `[Trait]` usage anywhere in the ~2120-test suite, which is organized entirely by `--filter "FullyQualifiedName~..."` — consistent with how every other verification command in this very document is written. Flagged the inconsistency and required an explicit decision (adopt `[Trait]` consistently or drop it) rather than silently introducing a one-off convention.

8. **Missing risk/rollback notes per step.** The original plan had one document-level Risks table but no per-step risk/rollback guidance, unlike the sibling Batches 51/53/63 reviewed alongside this one (which include a "Risk / Rollback" subsection per step). Added per-step Risk/Rollback notes to Steps 1, 3, 4, 5, and 6, and updated the document-level Risks & Mitigations table with two new rows (the blocking dependency itself, and the partial-`CreateFull()` possibility) plus corrected the "Low (Core-only, no rendering)" likelihood on the performance risk, which was an unsupported assertion of speed with no profiling behind it.

9. **Confirmed accurate, no change needed:** `ISeededRng`/Invariant 4 as a real, verified determinism mechanism (unrelated to the harness gap); the zero-engine-coupling design intent for this batch's own new files; the general statistical-metrics design in Step 2 (DTOs only, no dependency on the harness's specific API, so this step is NOT blocked by the Batch 63 gap and can proceed independently — clarified in Step 2 is unaffected since it only defines data shapes).
</content>
</file>
