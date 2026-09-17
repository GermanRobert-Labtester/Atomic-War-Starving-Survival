# ASHFALL — Quality Roadmap Batch 84

## Theme: Code Quality Metrics & Technical Debt Dashboard

| Field | Value |
|-------|-------|
| **Priority** | LOW-MEDIUM |
| **Risk** | Low overall (tooling/reporting only, no production code touched), but see Step 6's Risk/Rollback note: shipping this batch with the original plan's stale baseline constants would cause immediate, guaranteed CI failures, not a hypothetical risk |
| **Depends on** | None (reads existing source files) |
| **Touches** | `scripts/metrics/` (new), `Ashfall.Core.Tests/` (metric assertion tests) |
| **Engine coupling** | Zero — static analysis of .cs files, no runtime dependency |

---

## Motivation

The project tracks technical debt manually in `AGENTS.md` and `REPO_REVIEW_REPORT.md`. With 82+ systems, 195 test files, ~2120 tests (verified directly against the repository — see corrected table below; the commonly-cited "1941" figure is a stale historical snapshot, per Batch 63's own review, which traces it to `10LOOP_AUDIT_REPORT.md` documenting one past incremental change, not the current total), 22 save stores, and 20+ catalog loaders, there is no automated way to:

- Detect when `Main.cs` grows instead of shrinking
- Verify test count never decreases
- Track how many data files have `schema_version`
- Monitor bare `catch {}` block count trending toward zero
- Measure cyclomatic complexity hotspots
- Identify high-churn files that need refactoring

Key monitored files and their expected trajectories — **all baseline figures below were re-measured directly against the repository at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War` for this review; several figures inherited from AGENTS.md were stale and are corrected here (see Review Notes at the end of this document for the full derivation of each correction):**

| File/Metric | Current (corrected) | Originally stated | Target Direction |
|-------------|---------|---------|-----------------|
| `src/Main.cs` LOC | **7014** (`wc -l src/Main.cs`, verified) | ~6640 (stale) | ↓ decrease (extract to partials) |
| `GameBootstrap` partials | **Does not exist in the active codebase.** `GameBootstrap` as a real class is found only in `_quarantine_legacy/` (excluded legacy code) plus two doc-comments in `Assets/Ashfall.Core/CrossingSession.cs` and `HoldfastSession.cs` that explicitly state their code does *not* depend on it. AGENTS.md's own "Bridge Shim — Removed" section confirms the legacy Unity host (`Assets/_Game/`, where `GameBootstrap` lived) was fully deleted. | 82 files, 1225 LOC (stale — describes a deleted class) | N/A — metric should be **removed**, not tracked, since there is nothing left to measure |
| Test count | **~2120** (`[Fact]`/`[Theory]` count via direct grep, verified; matches Batch 63's independently-verified figure) | 1941 (stale) | ↑ only increase |
| Save store count | 22 | 22 | → stable (not independently re-verified in this review; carried forward from AGENTS.md, flagged as unverified below) |
| Data files with `schema_version` | **45 of 296** (12/98 top-level + 31/196 narrative + 2 exempt `documents/`/`whitelists/`; verified directly and cross-checked against Batch 51's independent audit) | ~35 (stale) | ↑ reach 130+ (also reconsider this target — see Review Notes; Batch 51's own corrected end-state target is 296/296, not 130+, once the exemption for `documents/`/`whitelists/` is accounted for) |
| Bare `catch {}` blocks | **19 silent catches across 4 files** (10 truly-empty-body + 9 `catch { return <default>; }` with no logging; verified directly and matches Batch 53's independent audit) | 10 (stale, and even the "bare/empty" subset alone is undercounted — see below) | ↓ reach 0 |

**On the bare-catch number specifically:** a plain grep for literally-empty `catch { }` bodies finds 15 instances outside `_quarantine_legacy/`: 6 in `Assets/Ashfall.Core/YearOfAsh/YearOfAshCatalogLoader.cs`, 1 in `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs`, 1 in `Assets/Ashfall.Core/Warlords/WarlordDoctrineCatalog.cs`, 2 in `Assets/Ashfall.Core/DoseContentCatalog.cs`, and 5 more in the Godot host (`src/Host/ResearchHostSession.cs`, `src/UI/FactionMatrixPanel.cs`, `src/UI/SnapshotOrchestrator.cs`, `src/Settings/UserSettings.cs`). Batch 53's own from-source audit (which reads the actual method bodies, not just brace-matching) found the behaviorally-relevant category — any catch that swallows an exception with **no logging**, whether the body is literally empty or does `return <default>;` — totals **19** across exactly 4 files in `Assets/Ashfall.Core/` (`VerdictCatalogLoader.cs`: 4, `YearOfAshCatalogLoader.cs`: 12, `WarlordDoctrineCatalog.cs`: 1, `DoseContentCatalog.cs`: 2). This document adopts Batch 53's 19-count as the tracked Core metric, since it matches the actual intent of AGENTS.md's H4 finding (silent exception swallowing), not the narrower "brace-only" pattern. The 5 host-layer (`src/`) empty catches found by this review's own direct grep are additional and were not in Batch 53's Core-only scope — see Step 2 below for how this batch's metrics collector should treat host-layer catches versus Core-layer catches as separate counters rather than conflating them into one number.

---

## Step 1 — Define Health Metrics Schema

### Goal

Define the complete set of code quality metrics to track, with clear definitions, measurement methods, and directionality (should this number go up, down, or stay stable).

### Implementation

- New file: `scripts/metrics/MetricsSchema.md` (human-readable definition)
- New file: `scripts/metrics/MetricDefinitions.cs` (C# enums/constants for programmatic use)
- Metric categories:
  1. **Size metrics:**
     - Total LOC (all `.cs` files in `Assets/Ashfall.Core/` + `src/` + `Ashfall.Core.Tests/`)
     - Max file LOC (identifies god objects)
     - Average file LOC
     - File count per layer (Core, Godot host, Tests)
  2. **Complexity metrics:**
     - Max method length (lines) per file
     - Cyclomatic complexity estimate (branch count: `if` + `else if` + `case` + `&&` + `||` + `??` + `?.` per method)
     - Nesting depth max (deepest indentation level)
  3. **Test metrics:**
     - Total test count (methods with `[Fact]` or `[Theory]`)
     - Test file count
     - Tests-per-system ratio (test count / system count)
     - Approximate coverage (files in Core with at least one corresponding test file)
  4. **Debt metrics:**
     - Silent `catch {}` blocks — count both the literally-empty-body pattern AND the `catch { return <default>; }` no-log pattern as one combined "silent catch" metric (per Batch 53's finding that the empty-body-only pattern undercounts the real problem by more than half — 10 empty-body vs. 19 total silent catches in Core alone); track Core (`Assets/Ashfall.Core/`) and host (`src/`) as separate sub-counters, since they have different owners and different remediation batches
     - `// TODO` / `// HACK` / `// FIXME` marker count
     - `System.Random` usage count (should be 0 in Core)
     - `JsonUtility` usage count (should be 0 in Core — note: verify this pattern doesn't false-positive on `Assets/_Game/`-style legacy paths if any remain; confirmed via this review that `Assets/_Game/` no longer exists in the tree, so this scan should return 0 hits total today, not just 0 in Core specifically)
     - Files without namespace declaration
  5. **Data metrics:**
     - JSON files with `schema_version` field / total JSON files
     - Duplicate definition IDs across data files
     - Mixed case property names (camelCase in snake_case files)
  6. **Architecture metrics:**
     - Engine references in Core (`UnityEngine` or `Godot` in `Assets/Ashfall.Core/`)
     - Cross-layer dependency violations
     - Partial file count per class (identifies fragmentation)
- Each metric has: name, description, measurement_method, direction (↑/↓/→), threshold_red, threshold_yellow

### Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
```

### Done when

- Schema document covers all 6 categories with ≥20 individual metrics
- Each metric has a clear programmatic measurement method described
- Direction and thresholds are documented with rationale
- **The "GameBootstrap partials" metric from the original plan's Motivation table is not included in this schema** — it described a class that no longer exists in the active codebase (see Motivation correction above); `src/Main.cs`'s own LOC/complexity metrics already cover the closest real equivalent god-object risk

### Risk / Rollback

None — this step produces only documentation and constant definitions with no runtime behavior. If a metric definition later proves impractical to implement in Step 2, removing it from the schema is a documentation-only edit.

---

## Step 2 — Implement Metrics Collector

### Goal

Build a C# tool that parses all `.cs` and `.json` files in the repository and computes every metric defined in Step 1, outputting structured results.

### Implementation

- New file: `scripts/metrics/MetricsCollector.cs` (standalone console tool or test-hosted)
- Approach: file-system scanning + regex/line-based parsing (no Roslyn dependency to keep it lightweight)
- Parsing strategies:
  - LOC: count non-empty, non-comment lines
  - Method detection: lines matching `(public|private|protected|internal)\s+\w+.*\(` that aren't in comments
  - Method length: lines between method signature and matching closing brace (heuristic: track brace depth)
  - Cyclomatic complexity: count `if `, `else if`, `case `, `&&`, `||`, `??`, `?.` within each method body
  - Test count: count lines matching `\[Fact\]` or `\[Theory\]`
  - Debt markers: grep for **two separate patterns** for silent catches (`catch\s*\{\s*\}` for empty-body, and `catch\s*\{[^}]*return[^}]*\}` with no `log\.` or `log\?\.` call inside for the silent-return variant — a single "bare catch" pattern undercounts by more than half per the Motivation section's correction), plus `// TODO`, `System.Random`, `JsonUtility`, `UnityEngine`
  - JSON schema_version: parse each `.json` file, check for top-level `schema_version` key
- Output: `MetricsSnapshot` object serialized to JSON
- `MetricsSnapshot` fields:
  ```csharp
  public class MetricsSnapshot
  {
      public DateTime Timestamp { get; set; }
      public string GitCommitHash { get; set; }
      public Dictionary<string, FileMetrics> Files { get; set; }
      public AggregateMetrics Aggregates { get; set; }
  }
  ```
- Runs in under 10 seconds for the full repository — **unverified estimate; the repository was not measured for collector runtime in this review since the collector doesn't exist yet. Treat this as a target to validate on first implementation, not a guarantee** (the repo has 296 JSON files and several hundred `.cs` files across Core/host/tests, which is plausibly fast for line-based regex scanning, but should be measured rather than assumed, consistent with this review's correction of similar unmeasured wall-time claims in Batch 83)
- No engine dependency — uses only `System.IO`, `System.Text.RegularExpressions`, `System.Text.Json`

### Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~MetricsCollector"
```

### Done when

- Collector runs against the actual repository and produces a valid `MetricsSnapshot`
- File-level metrics (LOC, method count, max method length) are within 5% of manual spot-checks
- Test count matches the verified current value (**~2120** as of this review, via direct `[Fact]`/`[Theory]` count — not the stale "1941" figure; re-run the count at implementation time since the number will have drifted further by then, and assert the collector's own count is self-consistent with a fresh manual grep rather than hardcoding either number as a magic constant in a test)
- Execution completes in under 10 seconds — **treat as unverified until measured; if it exceeds 10s, that is a finding to report, not a silent scope change to a larger number**

### Risk / Rollback

Low: this tool only reads files, never writes to production source. The one real risk is a regex false-positive/false-negative rate high enough to make the metrics untrustworthy (e.g. the cyclomatic-complexity heuristic miscounting `??`/`?.` inside string literals or comments) — mitigate by spot-checking against 3-5 manually-counted files before trusting the aggregate output, and treat the "within 5% of manual spot-checks" Done-when criterion as a hard gate, not a nice-to-have. Rollback is trivial: this is a new standalone tool with no callers outside itself and Step 3's baseline generation, so a bad implementation can be deleted and rewritten without affecting anything else.

---

## Step 3 — Add Metrics Baseline Snapshot

### Goal

Capture the current state of all metrics as the v1.0 baseline, establishing the reference point for all future trend analysis and regression detection.

### Implementation

- New file: `scripts/metrics/baselines/v1.0_baseline.json` (checked into git)
- Baseline captures:
  - Timestamp and git commit hash
  - All aggregate metrics
  - Per-file metrics for monitored hotspot files (`src/Main.cs`; save stores — **`GameBootstrap` partials removed from this list, see correction below**)
  - Debt counters (silent catches — Core and host sub-counters separately, per Step 1/2's correction — TODOs, engine coupling violations)
- **Correction — `GameBootstrap` does not exist and must not be a tracked hotspot.** The original plan listed "`GameBootstrap` partials" as a monitored hotspot file group. Verified directly against the repository: `GameBootstrap` as a real, compiled class exists only under `_quarantine_legacy/` (excluded legacy code, not part of the active build) and in two doc-comments in `Assets/Ashfall.Core/` that explicitly disclaim depending on it. There is no `GameBootstrap.*.cs` partial-class set anywhere in `src/` or the active `Assets/Ashfall.Core/` tree to measure. This aligns with AGENTS.md's own "Bridge Shim — Removed" section, which confirms the legacy Unity host (`Assets/_Game/`, where `GameBootstrap` lived) was fully deleted during the Godot migration. Tracking a metric for a deleted class would always report "0 files, 0 LOC" and provide no signal — remove it from the hotspot list entirely rather than baselining a phantom metric. If a genuinely analogous god-object risk exists in the active codebase, `src/Main.cs` itself is the closest real equivalent (a single ~7014-line partial class per AGENTS.md's own H7 finding) and is already tracked separately above.
- Baseline generation command (test-hosted):
  ```bash
  dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~GenerateBaseline"
  ```
- Baseline format includes metadata:
  ```json
  {
    "version": "1.0",
    "generated_at": "2025-01-15T00:00:00Z",
    "git_hash": "abc123...",
    "metrics": { ... }
  }
  ```
- Document known starting values prominently — **all corrected below against direct repository measurement; do not carry forward the stale figures from the original plan or from AGENTS.md without re-verifying at implementation time, since even this review's numbers will drift further by then:**
  - `src/Main.cs`: **7014 LOC** (verified via `wc -l src/Main.cs`; was stated as "~6640," stale by ~374 lines)
  - Test count: **~2120** (verified via direct `[Fact]`/`[Theory]` grep across `Ashfall.Core.Tests/`; was stated as "1941," stale — matches Batch 63's independently-verified figure)
  - Save stores: 22 (carried forward from AGENTS.md, **not independently re-verified in this review** — flag for confirmation at implementation time rather than treating as settled)
  - Silent catches: **19** across 4 files in `Assets/Ashfall.Core/` (10 empty-body + 9 catch-and-return-with-no-log; verified directly and matches Batch 53's independent audit), **plus 5 more literally-empty catches in `src/` not covered by that count** (was stated as "10," stale and scoped to Core only even at that undercounted value)
  - `schema_version` coverage: **45/296 files (15.2%)** — 12/98 top-level, 31/196 narrative, 2 exempt (`documents/`, `whitelists/`) (verified directly and matches Batch 51's independent audit; was stated as "~35/280 files (12.5%)," stale on both the numerator and the denominator — the real total file count is 296, not 280)
  - Engine violations in Core: 0 (not independently re-verified with a full automated scan in this review, but consistent with AGENTS.md's Invariant 1 claim of "0 violations" and with this review's own targeted checks finding no `UnityEngine`/`Godot` references in the sampled Core files touched)

### Verification

```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~GenerateBaseline"
# Verify file exists and is valid JSON
```

### Done when

- Baseline file is generated, valid JSON, and checked into `scripts/metrics/baselines/`
- All metric values are populated (no nulls or zeros where values are expected) — **except the removed `GameBootstrap` metric, which should not appear in the schema at all per Step 1's correction, rather than appearing as a zero value that could be misread as "0 violations" instead of "not tracked, class doesn't exist"**
- Baseline matches manually verified spot-checks for key metrics (against the corrected values in this document's Motivation section, not the stale figures the original plan carried forward from AGENTS.md)
- File is small enough to review in a PR (< 500 lines of JSON)

### Risk / Rollback

Low, but this is the step where every stale number in the original plan would have been permanently baked into a committed file if not corrected here first. Rollback if a bad baseline is committed anyway: regenerate and force-overwrite `v1.0_baseline.json` in a follow-up commit — since this is the *first* baseline (no prior trend history depends on it yet), there is no cascading data loss from replacing it, unlike a later baseline update after Step 4's trend tracker has accumulated history against it.

---

## Step 4 — Add Trend Tracking

### Goal

Store metrics snapshots over time (per-commit or per-date) and compute deltas between any two snapshots, enabling detection of gradual degradation or improvement.

### Implementation

- New file: `scripts/metrics/MetricsTrendTracker.cs`
- Storage: `scripts/metrics/history/` directory with one JSON file per snapshot (named by date: `2025-01-15.json`)
- `TrendTracker` class methods:
  - `RecordSnapshot(MetricsSnapshot snapshot)` — saves to history directory
  - `ComputeDelta(MetricsSnapshot baseline, MetricsSnapshot current)` — returns `MetricsDelta` with per-metric change values
  - `DetectRegressions(MetricsDelta delta, RegressionThresholds thresholds)` — returns list of violated metrics
- `MetricsDelta` includes:
  - Absolute change (current - baseline)
  - Percentage change
  - Direction compliance (did the metric move in the expected direction?)
  - Severity (info/warning/critical based on thresholds)
- Regression thresholds (configurable):
  - `Main.cs` LOC increase > 50 lines → critical
  - Test count decrease by any amount → critical
  - New bare catch blocks → warning
  - New `System.Random` in Core → critical
  - `schema_version` coverage decrease → warning
- History files are gitignored except for the baseline (local development tracking)

### Verification

```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~TrendTracker"
```

### Done when

- Trend tracker can record snapshots and compute deltas between any two
- Delta correctly identifies direction violations (e.g., `Main.cs` LOC increased when it should decrease)
- Regression detection fires appropriately for synthetic test data (inject a snapshot with +100 LOC to Main.cs, verify critical regression detected)
- History directory structure works for 30+ snapshots without performance issues

### Risk / Rollback

Low. Sequencing note: this step's `ComputeDelta` needs at least two snapshots to be meaningful, and the only one that exists after Step 3 is the single v1.0 baseline — the first real trend delta can't be computed until a second snapshot is taken at some later point in normal development. This is expected, not a blocker (the synthetic-injection test above validates the *mechanism* without needing real history), but avoid writing a Done-when criterion that implies real multi-point trend data exists immediately after this step, since it won't. Rollback: history files are explicitly gitignored per this batch's own Notes section, so a corrupted or malformed history directory can be deleted locally with no shared-state impact.

---

## Step 5 — Generate Technical Debt Dashboard

### Goal

Produce a human-readable markdown dashboard that summarizes current codebase health with traffic-light indicators, trend arrows, and actionable highlights.

### Implementation

- New file: `scripts/metrics/DashboardGenerator.cs`
- `GenerateDashboard(MetricsSnapshot current, MetricsSnapshot baseline, string outputPath)`:
- Dashboard sections:
  1. **Health Summary** — overall score (green/yellow/red) based on weighted metric compliance
  2. **Key Indicators** — table with metric name, current value, baseline value, delta, direction arrow (↑↓→), status (🟢🟡🔴)
  3. **Hotspot Files** — top 10 files by LOC, top 10 by complexity, top 10 by method count
  4. **Debt Inventory** — silent catches (count + file list, using the combined empty-body + catch-and-return-with-no-log pattern from Step 1/2's correction, not the narrower "bare catch" pattern that undercounts by more than half), TODO markers (count + top files), engine violations
  5. **Test Health** — test count, tests-per-system ratio, files without test coverage
  6. **Data Compliance** — schema_version coverage, naming convention violations, duplicate IDs
  7. **Trend** — comparison to baseline with percentage changes and direction compliance
- Traffic light rules:
  - 🟢 Green: metric at or better than target
  - 🟡 Yellow: metric worse than baseline but within tolerance
  - 🔴 Red: metric worse than tolerance threshold
- Output: markdown file at specified path
- ASCII-art sparkline for metrics with 5+ historical data points (optional future enhancement)

### Verification

```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~DashboardGenerator"
```

### Done when

- Dashboard generates successfully from current snapshot + baseline
- All 7 sections are present and formatted correctly
- Traffic lights correctly reflect metric status (testable: inject a snapshot with known good/bad values)
- Dashboard is readable without external tooling (plain markdown, renders in any viewer)
- File size is reasonable (< 200 lines for the summary view)

### Risk / Rollback

None — pure read-and-render, no state mutation. If the dashboard's traffic-light thresholds turn out to be miscalibrated once real trend data accumulates (a real possibility, since Step 4's note above establishes there's no real multi-point history yet at this point in the batch), that's a follow-up tuning task, not a rollback — the dashboard generator itself has no persistent state to corrupt.

---

## Step 6 — Add CI Gate for Metric Regressions

### Goal

Add an always-on test that fails the CI build when critical metrics regress beyond tolerance, preventing accidental quality degradation from merging.

### Implementation

- New file: `Ashfall.Core.Tests/Metrics/MetricRegressionTests.cs`
- Tests:
  - `[Fact] MainCs_LOC_MustNotIncrease()`:
    - Counts lines in `src/Main.cs`
    - Asserts ≤ baseline value (**7014**, verified current — was stated as derived from "~6640" in the original plan, which would make this test fail immediately on a clean checkout today, since the real file is already 374 lines past that stale baseline) + 50 (tolerance for in-progress work)
    - Error message: "Main.cs grew from {baseline} to {current} lines. Extract logic to a partial or domain file."
  - `[Fact] TestCount_MustNotDecrease()`:
    - Counts `[Fact]` + `[Theory]` across all test files
    - Asserts ≥ baseline value (**~2120**, verified current — was stated as "1941" in the original plan; using the stale lower number would still technically pass today since 2120 ≥ 1941, but defeats the purpose of a regression gate by baselining 179 tests below the real floor, meaning up to 179 tests could be silently deleted before this gate would ever fire)
    - Error message: "Test count dropped from {baseline} to {current}. Tests must not be deleted without replacement."
  - `[Fact] CoreEngine_ZeroViolations()`:
    - Scans `Assets/Ashfall.Core/**/*.cs` for `UnityEngine` or `Godot` references
    - Asserts count == 0
    - Error message: "Engine coupling found in Core: {file}:{line}"
  - `[Fact] SilentExceptionHandlers_MustNotIncrease()` (renamed from `BareExceptionHandlers_MustNotIncrease` to match the corrected, broader definition):
    - Counts silent `catch` blocks (empty-body OR catch-and-return-with-no-log, per Step 1/2's correction) across `Assets/Ashfall.Core/**/*.cs`
    - Asserts ≤ baseline value (**19**, verified current across 4 files — was stated as "10" in the original plan, which is both stale and scoped to the wrong pattern; a test asserting `≤ 10` against a codebase that already has 19 would fail on the very first run, immediately after being added, with zero code changes in between — this is the single most severe stale-baseline bug in the original plan, since every other stale number merely under-protects, but this one actively breaks CI on day one)
    - Track `src/` (host-layer) silent catches as a separate, informational counter rather than folding them into the same assertion, since Batch 53's remediation work (which establishes what "the baseline" means) scoped to Core only — decide explicitly whether host-layer catches are in scope for this gate before writing the assertion, rather than silently including or excluding them
    - Error message: "Silent catch count increased from {baseline} to {current}."
  - `[Fact] SystemRandom_NotInCore()`:
    - Scans Core for `System.Random` (excluding comments)
    - Asserts count ≤ known offender count (decreasing allowed) — **note AGENTS.md's Invariant 4 claims this was fully resolved ("migrated to `ISeededRng`; verified by `Ashfall.Core.Tests`"); if true, the baseline here should be 0, not a nonzero "known offender count" — confirm which is currently accurate before writing this assertion, since the two claims (Invariant 4's "resolved" vs. this metric's "known offender count") are in tension and only one can be the current truth**
  - `[Fact] SchemaVersion_Coverage_MustNotDecrease()`:
    - Counts JSON files with `schema_version` / total data JSON files
    - Asserts percentage ≥ baseline percentage (**45/296 = 15.2%**, verified current — was implicitly "~35/280 = 12.5%" in the original plan's Step 3 baseline; using the stale lower percentage as the floor would allow real coverage to regress by roughly 30 files before this gate ever fires)
- Baseline values stored in a `MetricBaselines.cs` constants file (easy to update after intentional changes)
- All tests scan the actual filesystem — they are integration tests but fast (< 3 seconds total) — **unverified estimate, same caveat as Step 2's "<10s" collector runtime: measure on first implementation rather than assuming**

### Risk / Rollback

High for this step specifically, more than the document-level Risks table below suggests: shipping this step with the original plan's stale baselines (`Main.cs` ≤ 6690, silent catches ≤ 10) would make `MainCs_LOC_MustNotIncrease` and `SilentExceptionHandlers_MustNotIncrease` **fail immediately on merge**, since the real repository is already past both thresholds (7014 > 6690; 19 > 10). This is not a hypothetical regression risk — it is a guaranteed false-positive CI failure baked into the plan as originally written. Rollback for this specific mistake, if it ships anyway: revert the constants file to the corrected values in this document (7014 / ~2120 / 19 / 15.2%) as a single follow-up commit; do not attempt to "fix" it by loosening the tolerance bands instead, since that would mask the real Main.cs and silent-catch debt that Batches 53 and future Main.cs-splitting batches are meant to pay down.

### Verification

```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~MetricRegression"
```

### Done when

- All 6 regression tests pass against the current repository state **using the corrected baselines above (7014 / ~2120 / 0 / 19 / offender-count-TBD / 15.2%), re-verified at implementation time since every one of these numbers will have drifted further by then** — do not copy the original plan's numbers into `MetricBaselines.cs` without re-running the verification commands from this document's corrected Motivation section first
- Intentionally adding a `UnityEngine` reference to a Core file causes `CoreEngine_ZeroViolations` to fail
- Intentionally deleting a test file causes `TestCount_MustNotDecrease` to fail
- Tests run in under 5 seconds total (measure, don't assume)
- Error messages are actionable (tell the developer what to fix, not just what failed)

---

## Step 7 — Add Quarterly Debt Review Template

### Goal

Create a structured template and process for quarterly technical debt reviews, ensuring the team regularly evaluates metrics trends, prioritizes debt paydown, and updates targets.

### Implementation

- New file: `scripts/metrics/templates/quarterly_review_template.md`
- Template sections:
  1. **Review Period** — date range, commits covered, major features shipped
  2. **Metrics Delta** — auto-populated table comparing start-of-quarter to end-of-quarter
  3. **Debt Paydown Accomplished** — what was fixed this quarter (with ticket/PR references)
  4. **New Debt Introduced** — what grew worse (with justification if intentional)
  5. **Top 5 Hotspots** — files/systems most in need of attention based on metrics
  6. **Next Quarter Targets** — specific numeric goals (e.g., "reduce Main.cs by 500 LOC", "add schema_version to 20 more files")
  7. **Baseline Update** — decision on whether to update the metrics baseline
  8. **Process Improvements** — what to change about the metrics/review process itself
- New file: `scripts/metrics/ReviewGenerator.cs`:
  - `GenerateQuarterlyReview(MetricsSnapshot startOfQuarter, MetricsSnapshot endOfQuarter, string outputPath)`
  - Auto-fills sections 2, 4, 5 from metric data
  - Leaves sections 1, 3, 6, 7, 8 as prompts for human completion
- Review cadence documentation:
  - Run metrics collector at start and end of quarter
  - Generate review template with auto-populated data
  - Team fills in human sections
  - Update baseline if targets were met
  - Archive completed review in `scripts/metrics/reviews/Q{N}_{year}.md`

### Verification

```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~ReviewGenerator"
```

### Done when

- Template generates with all 8 sections from two snapshots
- Auto-populated sections correctly compute deltas and identify top hotspots
- Human-fillable sections have clear prompts and examples
- Process documentation explains the full quarterly workflow
- Template is usable standalone (no external tooling required beyond running the metrics collector)

### Risk / Rollback

None — this step generates a document, not executable gates; a bad quarterly review template is a documentation fix, not a code rollback. Sequencing note (same class of issue as Step 4): this step requires two real snapshots taken roughly a quarter apart to be useful for its intended purpose. Immediately after this batch ships, only Step 3's single baseline snapshot exists, so `GenerateQuarterlyReview` can technically run (e.g. against the baseline and itself) for testing purposes, but the first *meaningful* quarterly review can't happen until real time has passed and a second snapshot has been captured. Don't treat "generates a report" as proof the quarterly process works end-to-end — that can only be confirmed after one real quarter cycle.

---

## Summary Table

| Step | Deliverable | New Files | Key Metric |
|------|-------------|-----------|------------|
| 1 | Health Metrics Schema | `scripts/metrics/MetricsSchema.md`, `MetricDefinitions.cs` | 20+ metrics across 6 categories defined (no `GameBootstrap` metric — removed, class doesn't exist) |
| 2 | Metrics Collector | `scripts/metrics/MetricsCollector.cs` | Parses full repo, computes all metrics; runtime unmeasured, target <10s |
| 3 | Baseline Snapshot | `scripts/metrics/baselines/v1.0_baseline.json` | Current state captured as reference point using corrected values: Main.cs=7014, tests≈2120, silent catches=19, schema_version=45/296 |
| 4 | Trend Tracking | `scripts/metrics/MetricsTrendTracker.cs` | Detects regressions with configurable thresholds; no real multi-point history exists until a second snapshot is taken post-launch |
| 5 | Debt Dashboard | `scripts/metrics/DashboardGenerator.cs` | 7-section markdown with traffic-light indicators |
| 6 | CI Regression Gate | `Ashfall.Core.Tests/Metrics/MetricRegressionTests.cs` | 6 always-on tests, runtime unmeasured (target <5s), actionable error messages — **must use corrected baselines (7014/~2120/19/15.2%) or it fails immediately on merge** |
| 7 | Quarterly Review Template | `scripts/metrics/templates/quarterly_review_template.md`, `ReviewGenerator.cs` | 8-section structured review with auto-populated data; first meaningful use is one quarter after launch |

---

## Architecture Diagram

```
┌──────────────────────────────────────────────────────┐
│              MetricsCollector                          │
│  Scans: Assets/Ashfall.Core/**/*.cs                  │
│         src/**/*.cs                                   │
│         Ashfall.Core.Tests/**/*.cs                   │
│         Assets/StreamingAssets/Data/**/*.json         │
└─────────────────────┬────────────────────────────────┘
                      │
                      ▼
┌──────────────────────────────────────────────────────┐
│              MetricsSnapshot (JSON)                    │
│  Per-file metrics + Aggregate metrics + Timestamp     │
└──────┬──────────────────┬────────────────┬───────────┘
       │                  │                │
       ▼                  ▼                ▼
┌─────────────┐  ┌────────────────┐  ┌──────────────────┐
│ TrendTracker │  │ DashboardGen   │  │ CI Regression    │
│ (history)    │  │ (markdown)     │  │ Tests (xUnit)    │
└──────┬───────┘  └───────┬────────┘  └──────────────────┘
       │                  │
       ▼                  ▼
┌─────────────┐  ┌────────────────┐
│ ReviewGen   │  │ Dashboard.md   │
│ (quarterly) │  │ (traffic lights)│
└─────────────┘  └────────────────┘
```

---

## Risks & Mitigations

| Risk | Likelihood | Mitigation |
|------|-----------|------------|
| Regex-based parsing misidentifies code constructs | Medium | Validate against manual spot-checks; accept ±5% accuracy for complexity metrics |
| Metrics collector too slow on large repo | Unknown (was stated "Low" with no measurement behind it) | Scan only tracked `.cs`/`.json` files; skip `obj/` `bin/` `.godot/`; measure actual runtime on first implementation rather than assuming |
| **CI gate ships with stale baselines and fails immediately on merge, not during future refactoring** | **Certain if the original plan's numbers (Main.cs≤6690, catches≤10) are used as-is; avoided entirely if this document's corrected numbers (7014/~2120/19/15.2%) are used** | **Re-verify all four baseline numbers against the live repository immediately before writing `MetricBaselines.cs` — do not trust this document's numbers by the time of implementation either, since they will have drifted further; see Step 6's Risk/Rollback note** |
| CI gate too strict during legitimate future refactoring (the originally-intended version of this risk) | Medium | Tolerance bands on all metrics; baseline update mechanism for intentional changes |
| Dashboard becomes stale if not regenerated | Medium | CI step that regenerates on every merge to main; or on-demand via test command |
| Quarterly reviews not performed | High (process risk) | Calendar reminder; review is low-effort if auto-populated sections work |
| `GameBootstrap` metric silently reintroduced by a future contributor copying from AGENTS.md's stale tables without reading this batch's corrections | Low-Medium | This document's Step 1/3 corrections should be the canonical source for this batch; AGENTS.md itself should ideally be corrected too (out of scope for this batch, but worth flagging to whoever maintains it) |

---

## Notes

- All tooling lives in `scripts/metrics/` (new directory) and `Ashfall.Core.Tests/Metrics/`
- No production code changes required — this is pure observability
- No engine coupling — all analysis uses file I/O and regex, no Godot or Unity references
- Metrics history is gitignored (local); only baselines and the CI gate constants are committed
- Future enhancement: integrate with PR comments (auto-post metrics delta on each PR)
- Future enhancement: Roslyn-based analysis for precise complexity (replaces regex heuristics)
- The collector does NOT require building the project — it's static analysis of source text
- **Every numeric baseline in this document (Main.cs LOC, test count, silent-catch count, schema_version coverage) was measured directly against the repository at review time and will continue to drift as development proceeds. Whoever implements this batch must re-run the verification commands in the Motivation section immediately before writing `MetricBaselines.cs` and `v1.0_baseline.json` — treat this document's numbers as "known-correct as of this review," not "permanently correct."**

---

## Review Notes (Corrected)

This plan was adversarially reviewed against the real repository at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War` and corrected in place. Findings:

1. **`src/Main.cs` LOC was stale.** Original plan and AGENTS.md both cite "~6640." Verified directly via `wc -l src/Main.cs`: the real current count is **7014** — 374 lines past the stale figure. This matters concretely for Step 6's `MainCs_LOC_MustNotIncrease` test: implementing it with the stale baseline plus a 50-line tolerance (≤6690) against a real file of 7014 lines would fail on the very first CI run, with zero code changes in between. Corrected throughout the Motivation table, Step 3's baseline documentation, and Step 6's test.

2. **`GameBootstrap` does not exist in the active codebase and was incorrectly listed as a monitored hotspot.** Verified via repo-wide search: `GameBootstrap` as a real, compiled class exists only under `_quarantine_legacy/` (excluded legacy code, not part of the active build), plus two doc-comments in `Assets/Ashfall.Core/CrossingSession.cs` and `HoldfastSession.cs` that explicitly state their code does *not* depend on it. This is consistent with AGENTS.md's own "Bridge Shim — Removed" section, which confirms the legacy Unity host (`Assets/_Game/`, where `GameBootstrap` lived) was fully deleted — verified independently: `Assets/_Game/` does not exist in the current tree. Tracking "82 files, 1225 LOC" for a deleted class would produce a permanently-zero, meaningless metric. Removed from Step 1's schema and Step 3's baseline hotspot list entirely, with a note that `src/Main.cs` is the closest real analogue for the god-object risk this metric was originally trying to capture.

3. **Test count was stale.** Original plan and AGENTS.md-adjacent sources cite "1941." Verified directly via `[Fact]`/`[Theory]` grep across `Ashfall.Core.Tests/`: the real current count is **~2120**, matching Batch 63's own independently-verified figure (obtained by reading that plan's Motivation section, which performed the same count). Using the stale 1941 figure as a regression floor in Step 6's `TestCount_MustNotDecrease` would still technically pass today (2120 ≥ 1941) but would allow up to 179 tests to be silently deleted before the gate ever fires — defeating the purpose of a "must not decrease" gate. Corrected throughout.

4. **Bare/silent catch count was stale and undercounted twice over.** Original plan and AGENTS.md cite "10 bare catch blocks... unchanged." Cross-referencing Batch 53's own from-source audit (which reads actual method bodies, not just brace-matching) and re-verifying directly against the repository: the real count of catches that swallow an exception with no logging (empty-body OR `catch { return <default>; }`) is **19**, spread across 4 files in `Assets/Ashfall.Core/` (`VerdictCatalogLoader.cs`: 4, `YearOfAshCatalogLoader.cs`: 12, `WarlordDoctrineCatalog.cs`: 1, `DoseContentCatalog.cs`: 2) — not 10, and not confined to the 2 files AGENTS.md names. This review additionally found 5 more literally-empty catches in the Godot host (`src/`) not counted by Batch 53's Core-only scope. This is the single most severe stale-number bug in the original plan: Step 6's `BareExceptionHandlers_MustNotIncrease` test, if implemented with the stale baseline of 10, would fail immediately on merge against a real repository that already has 19 (Core alone). Renamed the test to `SilentExceptionHandlers_MustNotIncrease` to match the corrected, broader definition, corrected the baseline to 19, and required Core vs. host catches to be tracked as separate counters since they have different owners and remediation batches.

5. **`schema_version` coverage was stale on both the numerator and the denominator.** Original plan and AGENTS.md cite "~35/280 files (12.5%)." Verified directly and cross-referenced against Batch 51's independent audit: the real data authority contains **296** total JSON files (98 top-level + 196 narrative + 1 documents + 1 whitelists), of which **45** currently have `schema_version` (12/98 top-level + 31/196 narrative + 2 exempt) — **45/296 = 15.2%**, not 35/280 = 12.5%. The "reach 130+" target was also stale; Batch 51's own corrected end-state target is 296/296 (100%, minus the 2 exempt files), not an arbitrary 130+ figure that doesn't correspond to any real milestone in the data tree's structure. Corrected throughout the Motivation table, Step 3's baseline, and Step 6's regression test.

6. **Missing risk/rollback notes per step.** Like Batch 83, the original plan had only a document-level Risks table with no per-step guidance. Added Risk/Rollback subsections to Steps 1-7, with particular emphasis on Step 6 (where shipping stale baselines causes a guaranteed, not hypothetical, CI failure) and Steps 4 and 7 (where a sequencing/ordering gap means the "trend" and "quarterly review" concepts can't be meaningfully exercised until a second real snapshot exists sometime after this batch ships).

7. **Illogical ordering / premature completeness claims.** Step 4 ("Trend Tracking") and Step 7 ("Quarterly Review Template") both implicitly assume multiple historical snapshots exist, but only Step 3's single v1.0 baseline exists immediately after this batch. Neither step's original Done-when criteria flagged this. Added explicit sequencing notes: the synthetic-injection tests validate the mechanism, but real trend/quarterly value can't be demonstrated until real time has passed and a second snapshot has been captured.

8. **Unmeasured wall-time claims presented as fact, consistent with the same pattern found in Batch 83.** Step 2's "<10 seconds" and Step 6's "<3/<5 seconds total" have no measurement behind them (the tools don't exist yet). Corrected to "target, to be measured on first implementation" rather than asserted fact, and flagged as a finding to report rather than silently absorb if exceeded.

9. **Header `Risk: None` was inconsistent with Step 6's actual finding.** The document-level header claimed zero risk for this "tooling/reporting only" batch, but Step 6 as originally written contains a guaranteed-failure bug (stale baselines that break CI on day one) — that is a real risk, not a hypothetical one, even though it's a CI/process risk rather than a production-code risk. Corrected the header's Risk field to acknowledge this while still correctly noting no production code is touched.

10. **Confirmed accurate, no change needed:** the "22 save stores" and "0 engine violations in Core" figures were carried forward from AGENTS.md without independent full re-verification in this review (both are plausible and consistent with targeted checks performed, but a full automated sweep was out of scope for this review) — flagged explicitly in the Motivation table and Step 6 as "not independently re-verified" rather than either asserting or silently trusting them. The overall six-category metric taxonomy (size/complexity/test/debt/data/architecture), the `scripts/metrics/` directory structure, and the general CI-gate design pattern were all sound and required no correction.
