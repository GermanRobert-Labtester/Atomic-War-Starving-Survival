# ASHFALL — Quality Roadmap Batch 58

## Theme: Performance Profiling Framework & Hot Path Optimization

**Priority:** MEDIUM
**Risk:** Low-Medium — profiling is additive; optimizations need behavioral equivalence
**Batch:** 58

---

## Context

Key hot paths in the Godot host (all verified against `src/Main.cs` and `Assets/Ashfall.Core/` on the current checkout):

| Hot Path | Description | Concern |
|----------|-------------|---------|
| `TickSimDay(int day)` | Advances the day-tick subsystems sequentially (verified: `SetupWorld/TickDemo`, Caravans, Medical, Expeditions, Duty Roster hatch-return bridge, Crafting, Maritime, Deep Coast, Holdfast runtime, Starting Level, Inventory ration consumption, Verdict, Year of Ash (day 180–360), Muster (day ≥260), Expansions/Greenhouse/Ledger/Crossing, Duty Roster tick, Ice Road/Duty sync — roughly 20 conditional/unconditional subsystem calls, not a flat loop over "31+ systems") | Linear-ish cost grows with system count; player-facing latency during day-advance |
| `SaveAll()` | Calls 28 individual `SaveXxx()` methods (verified by direct count in `src/Main.cs`, not 24 — AGENTS.md's "24 subsystems" figure describes an earlier/different accounting and should not be treated as authoritative for this work) | `SaveChecksum.Compute()` uses reflection; blocking on main thread |
| `_Process(double delta)` | Runs every engine frame, but exits early via `if (_diagnosticsAccum < DiagnosticsRefreshSeconds) return;` (`DiagnosticsRefreshSeconds = 0.25`, i.e. ~4Hz) *before* reaching the 14 `FlushXxxIfDirty()` calls — **verified by direct read of `src/Main.cs` lines 517–559**: the flush calls are textually located after that early return, so they only execute when the accumulator has reached 0.25s, not on every frame | The flush calls ARE already gated to ~4Hz by the diagnostics throttle today — they do NOT run at full 60Hz frame rate. This inverts the batch's original premise; see Step 6 for the corrected implication |
| `BuildUserInterface()` | A single ~640-line method (`src/Main.cs` lines 640–1283, confirmed exact bounds) that directly constructs **60** `_field = new XxxPanel/Overlay/Modal/...()` assignments inline (verified: `grep -oE "_[a-zA-Z]+\s*=\s*new\s+[A-Za-z]+\(" ` over that line range, deduplicated) and wires their events — of these 60, two are not deferrable panels at all (`_audio = new AudioManager()`, which is not even a `Control`, and `_menuContainer = new VBoxContainer()`, a bare layout container), leaving **58 genuine panel/overlay/modal fields**, matching the batch's "~58" figure once the 2 non-panel fields are excluded. There are no per-panel `BuildXxxPanel()` factory methods for this eager path — the **one** method anywhere in the file matching that naming pattern, `BuildYearOfAshPanel()`, is unrelated deferred-construction logic called elsewhere | Long startup time; all 60 fields (58 real panels) built and wired whether visible or not |
| `CatalogIntegrityValidator` (657 lines, verified via `wc -l`, not 603) | Five-tier validation at startup | Reflection-heavy; sequential tier execution |
| `SaveChecksum.Compute()` | Reflection-based integrity hash — **verified**: `Assets/Ashfall.Core/SaveChecksum.cs` walks public instance fields via `Type.GetFields(BindingFlags.Public \| BindingFlags.Instance)`, sorts them ordinally, and writes a self-delimiting canonical string that is SHA256-hashed. This is a genuine, confirmed hot-path cost | Called on every save; cost scales with state size (field count × object graph depth, bounded by `MaxDepth = 32`) |

The Godot host runs at 1920×1080, 60 FPS target, `gl_compatibility` renderer. All systems are in `Ashfall.Core` (engine-agnostic) with thin Godot host wiring in `src/`.

**Correction to the original framing:** the biggest factual error in the initial version of this batch was Step 4's premise. `BuildUserInterface()` does not call "85+ `BuildXxxPanel()` methods" — it is one large method with inline `new` construction. Any lazy-init refactor must restructure this construction pattern itself (extract each inline block into a factory delegate), not merely redirect existing factory calls into a registry. This is materially more invasive than the original plan implied, and Step 4's effort estimate and risk level are revised accordingly below. A second factual error, found in this pass, concerned Step 6: the plan asserted the 14 `FlushXxxIfDirty()` calls in `_Process` run unconditionally on every rendered frame; reading the actual method body shows they are textually gated behind the same `_diagnosticsAccum` early-return that throttles the diagnostics label to ~4Hz, so they already run at ~4Hz today, not 60Hz. Step 6 has been rewritten around the corrected premise.

---

## Step 1 — Add Lightweight Profiling Infrastructure

### Goal

Provide a zero-allocation, scope-based profiling system that can measure any code path with minimal overhead. Disabled in release builds. Results queryable for automated benchmarks.

### Implementation

- Add `Ashfall.Core/Profiling/ProfileScope.cs`:
  - `ref struct` (stack-only, zero-allocation) implementing `IDisposable`.
  - Constructor: records `Stopwatch.GetTimestamp()`.
  - `Dispose()`: computes elapsed, reports to `ProfileRegistry`.
  - Usage: `using var _ = new ProfileScope("TickSimDay");`
- Add `Ashfall.Core/Profiling/ProfileRegistry.cs`:
  - Static class (or injectable singleton via `IProfileRegistry` port).
  - Stores per-scope: call count, total time, min, max, rolling average (last 64 samples).
  - Thread-safe via `Interlocked` operations (no locks).
  - `GetReport()` → returns `IReadOnlyList<ProfileEntry>` sorted by total time descending.
  - `Reset()` clears all accumulators.
  - Conditional compilation: `[Conditional("ASHFALL_PROFILE")]` on entry points — zero cost when profiling is disabled.
- Add `Ashfall.Core/Profiling/ProfileEntry.cs`:
  - Record: `string Name`, `long CallCount`, `double TotalMs`, `double MinMs`, `double MaxMs`, `double AvgMs`.
- Add Godot host integration `src/Host/ProfileOverlay.cs`:
  - Debug overlay (toggle with `F3`): renders top-20 scopes as a sorted table.
  - Updates every 500ms to avoid rendering overhead.
- Wire `ASHFALL_PROFILE` define:
  - **Correction:** `Ashfall.csproj` today has a single flat `<PropertyGroup>` with no `Configuration`-conditional groups at all (verified by reading the file — it contains only `TargetFramework`, `EnableDynamicLoading`, `RootNamespace`, `AllowUnsafeBlocks`, `Nullable`, `LangVersion`, `EnableDefaultCompileItems`, and `NoWarn`, no `'$(Configuration)'=='Debug'` condition exists to "add to"). This step must **create** a new conditional `PropertyGroup` (e.g. `<PropertyGroup Condition="'$(Configuration)'=='Debug'"><DefineConstants>$(DefineConstants);ASHFALL_PROFILE</DefineConstants></PropertyGroup>`), not add a line to an existing one.
  - Not present in `Release` — profiling compiles away entirely.

### Verification

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj                                  # 0 errors with ASHFALL_PROFILE
dotnet build Ashfall.csproj -c Release                       # 0 errors without ASHFALL_PROFILE
```

### Done-when

- `ProfileScope` compiles to zero-cost in Release.
- `ProfileRegistry.GetReport()` returns accurate timing data in Debug.
- Overlay renders in-game behind `F3` toggle.
- No allocations on the hot path (verified by struct constraint).

---

## Step 2 — Profile TickSimDay and Identify Bottleneck Systems

### Goal

Instrument every subsystem tick within `TickSimDay` to identify which subsystem calls consume the most time. **Correction:** `TickSimDay(int day)` in `src/Main.cs` spans lines 1643–1772 (**130 lines**, not "~90+" as originally estimated — verified by brace-matching the method body) and is not a flat loop over "31+ systems" — it calls `SetupXxx()`/`TickXxx()`/`Advance()`/`Escalate()` for **18 distinct `SetupXxx()` calls** plus additional direct calls to `TickPowerGrid(day)`, `TickVerdict(day, ...)`, and `_hostEventAdapter?.EvaluateTriggers(...)`, several gated by day-range conditionals (`if (day >= 180 && day <= 360)` for Year of Ash, `if (day >= 260)` for Muster). The method also calls `SaveAll()` as its last statement, meaning `TickSimDay` and `SaveAll` are not independent hot paths — profiling one without accounting for the other double-counts or miscounts total day-advance latency. Two subsystems present in the method — `SetupDisease()`/`_disease.TickDaily(day)` and `SetupSilentFoundry()`/`_silentFoundry.Engine.TickDaily(day)` — are absent from this batch's Context table and from the "known-heavy systems" list below; add them if profiling data shows they're non-trivial. Read the actual method body before instrumenting — do not assume a uniform loop structure exists to hook into generically; each call site needs its own `ProfileScope` wrap.

### Implementation

- Instrument `TickSimDay(int day)` in `src/Main.cs`:
  - Wrap each subsystem's `Tick(day)` / `Advance(day)` call with `ProfileScope`:
    ```csharp
    using var _ = new ProfileScope($"Tick.{systemName}");
    system.Tick(day);
    ```
  - Add outer scope: `using var _outer = new ProfileScope("TickSimDay.Total");`
  - **Note:** `TickSimDay` calls `SaveAll()` as its final statement (confirmed at the end of the method body, just before the closing brace at line 1772). `TickSimDay.Total` will therefore include the full `SaveAll` cost unless the outer scope is deliberately closed (or a nested `ProfileScope` opened) immediately before the `SaveAll()` call. Decide explicitly whether "TickSimDay.Total" is meant to include or exclude the save — Step 3 profiles `SaveAll` separately, so double-counting it inside `TickSimDay.Total` will make the two reports inconsistent with each other unless this is documented.
- Instrument at minimum these known-heavy systems:
  - `NeedsSystem.Tick`
  - `RadiationSystem.Tick`
  - `WeatherSystem.Advance`
  - `CombatTraumaSystem.Tick`
  - `DynamicEconomySystem.Tick`
  - `ExpeditionMasterSession.Tick`
  - `MedicalSystem.Tick` (if migrated to Core)
  - `PersonalQuestSystem.Tick` (if migrated to Core)
  - `NarrativeSystem.Advance`
  - `FinalWishSystem.Tick`
- Add `godot --headless --path . -- --profile-tick-report`:
  - Boots a minimal game state, advances 30 days, prints `ProfileRegistry.GetReport()` as a markdown table to stdout.
  - Exits 0.
- Document baseline results in `docs/profiling/tick-baseline.md`.

### Verification

```
dotnet build Ashfall.csproj
godot --headless --path . -- --profile-tick-report
# Produces ranked table; total TickSimDay time and per-system breakdown
# No crashes, no test regressions
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
```

### Done-when

- Every subsystem tick is individually profiled.
- Report identifies top-5 bottleneck systems by total time.
- Baseline document committed for future comparison.
- No behavioral changes — only measurement added.

---

## Step 3 — Profile SaveAll and Identify Serialization Bottlenecks

### Goal

Instrument the save pipeline to isolate where time is spent: state capture, JSON serialization, checksum computation, or file I/O. Determine if `SaveChecksum.Compute()` reflection cost dominates.

### Implementation

- Instrument `SaveAll()` in `src/Main.cs`:
  - Per-subsystem scopes: `ProfileScope($"Save.{name}.Capture")`, `ProfileScope($"Save.{name}.Serialize")`.
  - Dedicated scope for `SaveChecksum.Compute()`: `ProfileScope("Save.Checksum")`.
  - Dedicated scope for file write: `ProfileScope("Save.FileWrite")`.
  - Outer scope: `ProfileScope("SaveAll.Total")`.
- Instrument `SaveChecksum.Compute()` internals in `Assets/Ashfall.Core/SaveChecksum.cs`:
  - Scope for reflection field enumeration.
  - Scope for value normalization.
  - Scope for hash computation.
- Add `godot --headless --path . -- --profile-save-report`:
  - Boots game state, performs 5 consecutive saves, prints average profile report.
  - Reports: total save time, per-subsystem capture time, serialization time, checksum time, file I/O time.
  - Exits 0.
- Document baseline in `docs/profiling/save-baseline.md`.

### Verification

```
dotnet build Ashfall.csproj
godot --headless --path . -- --profile-save-report
# Produces timing breakdown; identifies dominant cost
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj    # No regressions
```

### Done-when

- Save pipeline fully instrumented with per-phase timing.
- Report clearly shows whether checksum reflection, serialization, or I/O dominates.
- Baseline document committed.
- Save/load behavioral equivalence confirmed by existing tests.

---

## Step 4 — Add Lazy Initialization for UI Panels

### Goal

Reduce startup time by deferring construction of non-visible panels. Only the initial panel/overlay the player actually sees (the dashboard + HUD) is built eagerly; the remaining panels/modals are built on first access.

### Implementation — REVISED (see Review Notes)

**Correction:** there are no pre-existing `BuildXxxPanel()` factory methods to redirect. `BuildUserInterface()` (src/Main.cs, lines 640–1283) directly does `_xPanel = new XPanel(); _xPanel.OnClose += ...; AddChild(_xPanel);` inline, once per field, for exactly **60** `_field = new Xxx()` assignments (verified by grep of `_\w+\s*=\s*new\s+[A-Za-z]+\(` over that line range). Of those 60, **2 are not panels at all** and are not lazy-init candidates: `_audio = new AudioManager()` (not even a `Control` — cannot be deferred the same way since nothing shows/hides it) and `_menuContainer = new VBoxContainer()` (a bare layout container that other eagerly-built UI depends on existing). That leaves **58 genuine panel/overlay/modal fields** as deferral candidates — matching the plan's "~58" only once those 2 non-candidates are explicitly excluded, not because "~58" was independently correct. This step must extract each of those inline blocks into a named factory function before it can be deferred — this is new refactoring work, not a redirect.

- Add `src/UI/LazyPanelRegistry.cs`:
  - Dictionary mapping panel ID → factory `Func<Control>`.
  - `GetOrCreate(string panelId)` — returns cached instance, or invokes the factory, adds the result to the scene tree via `AddChild`, wires its `OnClose`/other events, and caches it on first call.
  - `PrewarmPanel(string panelId)` — builds in background (for predictive preloading).
  - `IsBuilt(string panelId)` → bool.
- Refactor `BuildUserInterface()` in `src/Main.cs`:
  - For each of the 58 inline `new XxxPanel()`/`Overlay()`/`Modal()` blocks (the 60 total `_field = new Xxx()` assignments minus `_audio` and `_menuContainer`, which are not deferral candidates — see Implementation note above), extract the construction + event-wiring + `AddChild` into a private factory method (e.g. `private Control CreateInventoryOverlay()`), preserving the exact wiring logic verbatim.
  - Register each factory in `LazyPanelRegistry` instead of invoking it immediately.
  - Keep `_dashboard`, `_hudOverlay`, `_mainMenu`, and `_settingsPanel` eagerly constructed — these are shown unconditionally at startup and are not candidates for deferral. `_audio` (an `AudioManager`, not a `Control`) and `_menuContainer` (a `VBoxContainer` other eager UI is added to) must also remain eagerly constructed regardless of visibility, since they are infrastructure rather than switchable panels — this brings the true eager-construction floor to 6 fields, not 4.
  - Audit every call site that currently reads a `_xxxPanel` field directly (e.g. `AnyOverlayPanelOpen()`, which reads 42 panel fields into an array — verified exact count via direct read of `src/Main.cs:6261-6296`, batch's original "~40" estimate confirmed reasonably close) and route those reads through `LazyPanelRegistry.IsBuilt`/`GetOrCreate` so a not-yet-built panel doesn't produce a null reference.
  - Panel-switch logic (keyboard shortcut or button, e.g. `OpenPlayerPanel(string)`) calls `GetOrCreate` before showing.
- Add loading indicator:
  - If panel build takes >50ms, show a brief "Loading..." label in the panel area.
  - Subsequent accesses are instant (cached).
- Predictive preloading:
  - After the initial screen is shown and idle frames are available, prewarm the panels reachable from the dashboard's first-level navigation during `_Process` idle time.
  - Budget: max 8ms per frame for prewarming.
- Measure improvement:
  - Profile `BuildUserInterface` before/after with `ProfileScope`.
  - Expected: startup drops from constructing 60 fields to constructing 6 (dashboard, HUD, main menu, settings, audio manager, menu container) plus prewarming a handful during the first few seconds.

**Risk/Rollback:** This step touches `AnyOverlayPanelOpen()` and any other method that assumes all panel fields are non-null after `BuildUserInterface()` returns — a missed call site is a null-reference crash, not a silent bug, so it will surface in manual QA and in `--bridge-selftest`/panel smoke tests. Land this step behind a flag or on its own branch and keep the previous eager-construction code path easily revertible (e.g. a single boolean toggling eager-vs-lazy registration) for at least one release cycle before deleting the eager path.

### Verification

```
dotnet build Ashfall.csproj
godot --headless --path . -- --profile-tick-report
# Startup time (BuildUserInterface scope) should show significant reduction
# All panels still accessible — no missing UI
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
godot --headless --path . -- --bridge-selftest
```
Additionally: manually open every panel reachable from `OpenPlayerPanel` at least once per QA pass (there is no existing automated coverage that opens all 58 panels; add one if time allows, but do not treat its absence as a merge blocker for this step).

### Done-when

- Only the dashboard/HUD/main-menu/settings/audio-manager/menu-container (6 fields) are built at startup; the remaining 58 panels/overlays/modals are built on demand via `LazyPanelRegistry`.
- `AnyOverlayPanelOpen()` (42 direct field reads, confirmed exact count) and every other pre-existing direct-field read of a lazily-built panel is confirmed null-safe (either audited and fixed, or explicitly listed as out of scope with a follow-up ticket).
- No user-perceptible delay when switching panels (prewarming handles common paths).
- Startup time (measured via `ProfileScope` around `BuildUserInterface`) reduced by ≥60% relative to the pre-change baseline captured in Step 1/2 — do not claim a percentage without a recorded baseline number.
- All existing functionality preserved — no panels lost or broken; verified via `--bridge-selftest` plus a manual pass opening each panel at least once.

---

## Step 5 — Optimize CatalogIntegrityValidator

### Goal

Reduce startup validation time by caching reflection results, parallelizing independent tiers, and short-circuiting when data hasn't changed since last validation.

### Implementation

- Add reflection cache to `Assets/Ashfall.Core/CatalogIntegrityValidator.cs`:
  - On first run, cache: all definition IDs, all reference keys, all prefix patterns.
  - Store cache in a `Dictionary<string, HashSet<string>>` keyed by file path + last-modified timestamp.
  - On subsequent runs, skip files whose timestamp hasn't changed.
- Parallelize independent tiers:
  - Tier 1 (prefix resolution) and Tier 5 (uniqueness) are independent — run in parallel.
  - Tier 2 (reference resolution) depends on Tier 1 completing (needs the registry).
  - Tier 3 (ranges) and Tier 4 (custom rules) are independent of each other but need Tier 1.
  - Use `Task.WhenAll` for independent tiers; keep everything on the thread pool (no Godot main-thread requirement for validation).
- Add incremental mode:
  - Hash all JSON files at startup; compare against `user://catalog_validation_cache.json`.
  - If no files changed → skip validation entirely, report "cached: clean".
  - If files changed → validate only changed files + files that reference them.
- Preserve correctness:
  - Full validation still available via `--data-integrity-selftest --full` (CI always runs full).
  - Incremental mode is the default for game startup.
  - Any cache miss or corruption → falls back to full validation.

### Verification

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest         # Full mode: 0 errors
godot --headless --path . -- --data-integrity-selftest         # Second run: uses cache, faster
godot --headless --path . -- --data-integrity-selftest --full  # Forced full: 0 errors
```

### Done-when

- Full validation produces identical results (0 errors) before and after optimization.
- Cached/incremental mode reduces validation time by ≥70% on unchanged data.
- Parallel tier execution reduces full-validation time by ≥30%.
- CI always runs `--full`; game startup uses incremental.

---

## Step 6 — Add Frame Budget Monitoring to _Process

### Goal

Detect when the periodic flush work inside `_Process` exceeds its time budget (target: ≤4ms per invocation) and defer excess work to the next eligible invocation. Prevent frame-time spikes when many dirty flags accumulate between throttled flush passes.

**Correction (materially changes this step's premise):** `_Process(double delta)` is called by Godot every rendered frame (up to 60Hz), but its body opens with an accumulator-gated early return:
```csharp
_diagnosticsAccum += delta;
if (_diagnosticsAccum < DiagnosticsRefreshSeconds) return;   // DiagnosticsRefreshSeconds = 0.25 (~4Hz)
```
The 14 `FlushXxxIfDirty()` calls (`FlushJournalIfDirty`, `FlushHoldfastIfDirty`, `FlushDutyRosterIfDirty`, `FlushExpansionHubIfDirty`, `FlushVerdictIfDirty`, `FlushMaritimeIfDirty`, `FlushExpeditionIfDirty`, `FlushNarrativeIfDirty`, `FlushMedicalIfDirty`, `FlushWorldIfDirty`, `FlushCraftingIfDirty`, `FlushCaravanIfDirty`, `FlushYearOfAshIfDirty`, `FlushPhase0IfDirty` — verified via `grep -n "Flush" ` over `src/Main.cs:517–559`) sit **textually after** that early return, meaning **they already only run at ~4Hz, not on every frame.** The original version of this step asserted the opposite ("NOT currently gated... run at full frame rate today") — that assertion is factually wrong; it was likely inferred from the *comment* above the accumulator ("refresh ~4x/sec") without reading far enough to see that the same early return also gates the flush block below it, or without noticing the flush calls are inside the same method body past the guard.

This changes what "frame budget" work actually means here: there is no per-frame (60Hz) flush cost to control today — there is a per-4Hz-tick cost, already amortized to 4 times a second instead of 60. The real risk this step should address is different from the original framing: if the *cumulative* cost of all 14 flush checks plus any that are actually dirty exceeds ~4ms within a single throttled tick, that tick's frame will show a latency spike once every ~0.25s (still perceptible, just an order of magnitude rarer than a per-frame problem). Note also two `FlushXxxIfDirty` methods that exist elsewhere in the file — `FlushCombatIfDirty` and `FlushDoseLedgerIfDirty` (confirmed present via whole-file grep) — are **not** called from `_Process` at all; if their dirty state needs flushing, it currently happens through a different path (worth confirming before assuming all dirty-flag systems are covered by this step's target list).

### Implementation

- Add `src/Host/FrameBudgetMonitor.cs`:
  - Configurable budget: default 4ms, but scoped to a single throttled `_Process` pass (the ~4Hz tick where the early-return guard is satisfied), not every rendered frame — measuring per-rendered-frame cost would be measuring mostly the early-return path, which is already near-zero cost.
  - Tracks elapsed time within the current throttled tick's flush work.
  - Exposes `bool HasBudget()` — returns false when elapsed exceeds budget.
  - Exposes `FrameBudgetReport` with: throttled ticks over budget (count), worst-case tick time, average flush time per tick.
- Refactor the flush block in `_Process(double delta)` in `src/Main.cs` (the code after the `_diagnosticsAccum` early return, not the whole method):
  - Current: calls all 14 `FlushXxxIfDirty()` methods unconditionally once the throttle gate passes (each is a no-op if its own dirty flag is false, but the call/check itself still happens every passing tick).
  - New: iterates dirty flags in priority order; after each flush, checks `HasBudget()`.
  - If budget exhausted: remaining dirty flags deferred to the next throttled tick (~0.25s later, not literally "next frame" given the existing gate).
  - Priority order (highest first): save-critical state, UI-visible state, background state.
- Add starvation prevention:
  - If a dirty flag is deferred for >3 consecutive throttled ticks (~0.75s), force-flush it regardless of budget (prevents indefinite deferral).
  - Log a warning via `ILog` when starvation flush occurs — indicates a system is too expensive.
- Add `ProfileScope` instrumentation to each flush operation.
- Add debug overlay section (under `F3`): shows current tick budget usage, deferred count, starvation events.
- **Before writing any of the above**, profile the actual current cost of a full throttled flush pass (all 14 calls, worst case all dirty) using Step 1's infrastructure. If that cost is already comfortably under 4ms in practice (plausible, since it now only has to happen 4x/sec instead of 60x/sec), this step's value shifts from "prevent frame drops" to "regression guardrail + observability," and the Done-when criteria below should be read in that light rather than assuming a currently-broken hot path is being fixed.

### Verification

```
dotnet build Ashfall.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
godot --headless --path . -- --profile-tick-report
# Verify no behavioral regressions — all dirty flags still flushed within bounded time
# Frame budget violations logged but never cause data loss
```

### Risk / Rollback

Low-Medium — this step changes the *timing* of when dirty-flag flushes occur (deferring some to a later throttled tick under load) but not *whether* they occur; the starvation-prevention force-flush after 3 ticks bounds worst-case delay to ~0.75s. The main risk is a system whose "dirty" flush is order-dependent on another system's flush happening in the same tick — audit the 14 flush methods for any such cross-dependency before reordering by priority. Rollback: the priority-queue/deferral logic is confined to the flush block inside `_Process`; reverting to the original unconditional 14-call sequence (no `HasBudget()` checks) fully restores prior behavior, since `FrameBudgetMonitor` is a new additive file nothing else depends on.

### Done-when

- The flush block (not `_Process` as a whole, which is already correctly throttled) respects a 4ms budget per throttled tick.
- Dirty flags are prioritized and deferred gracefully within the existing ~4Hz cadence.
- Starvation prevention guarantees all flags flush within 3 throttled ticks (~0.75s) maximum.
- Debug overlay shows budget utilization in real-time.
- No behavioral changes — same data reaches the same systems, just potentially spread across throttled ticks instead of always completing within one.
- A baseline profiling run (per the note in Implementation) is recorded showing whether this step is fixing a real observed overrun or adding a guardrail against a currently-comfortable margin — Done does not require the former if the data shows the latter.

---

## Step 7 — Benchmark Suite for Performance Regression Detection

### Goal

Automated benchmark suite that runs in CI, detects performance regressions (>15% slowdown from baseline), and blocks merges that degrade hot paths.

### Implementation

- Add `Ashfall.Core.Tests/Benchmarks/` directory:
  - `TickSimDayBenchmark.cs`:
    - Constructs a representative game state (mid-game: 8 survivors, 50 items, day 45).
    - Runs `TickSimDay` for 100 iterations; records median, p95, p99.
    - Asserts median < baseline threshold (e.g., 15ms).
    - **Risk:** `TickSimDay` calls `SaveAll()` as its last statement (see Step 2's note) — running this benchmark for 100 iterations against the real `SaveAll()` means 100 real file writes to `user://` per benchmark run unless the harness substitutes a no-op/in-memory `IFileIO` for the benchmark's `Main`-equivalent host. Confirm the benchmark harness can construct a `TickSimDay`-equivalent path without a live Godot `Main` instance and without touching the real save directory before assuming this benchmark is a pure CPU measurement — if it isn't isolated, the "15ms" threshold will actually be dominated by disk I/O and will be flaky across CI runners with different disk speeds.
  - `SaveAllBenchmark.cs`:
    - Same representative state.
    - Runs `SaveAll` equivalent (CaptureState + serialize + checksum) for 50 iterations.
    - Asserts median < baseline threshold (e.g., 25ms).
  - `ChecksumBenchmark.cs`:
    - Constructs a large state DTO (worst-case: all **28** subsystems populated — corrected from "24"; see Context table and Review Notes item 3, which already established `SaveAll()` calls 28 `SaveXxx()` methods, not 24. Reusing the stale "24" figure here would reintroduce the exact error this batch corrected elsewhere).
    - Runs `SaveChecksum.Compute` for 200 iterations.
    - Asserts median < baseline threshold (e.g., 5ms).
  - `CatalogValidationBenchmark.cs`:
    - Runs full `CatalogIntegrityValidator` validation.
    - Asserts total time < baseline threshold (e.g., 500ms).
  - `FocusChainBuildBenchmark.cs`:
    - Constructs a mock tree of 500 controls.
    - Runs `FocusChainBuilder.BuildChain` for 100 iterations.
    - Asserts median < 2ms.
    - **Cross-batch dependency:** `FocusChainBuilder` does not exist yet — it is a deliverable of Batch 57 (Quality Roadmap Batch 57, Step 2, `src/UI/Accessibility/FocusChainBuilder.cs`), not this batch. This benchmark cannot be written until Batch 57 Step 2 has landed. If Batch 57 is not sequenced before this step, either drop `FocusChainBuildBenchmark.cs` from this batch's scope or explicitly add "Batch 57 Step 2" as a prerequisite in the Summary table below.
- Add `scripts/ci/benchmark-gate.sh`:
  - Runs all benchmark tests.
  - Compares against `docs/profiling/benchmark-baselines.json`.
  - Fails (exit 1) if any benchmark exceeds baseline by >15%.
  - On success, optionally updates baselines if `--update-baselines` flag is passed.
- Baseline management:
  - `docs/profiling/benchmark-baselines.json` — committed, updated manually after confirmed improvements.
  - Format: `{ "TickSimDay.MedianMs": 12.3, "SaveAll.MedianMs": 20.1, ... }`
- Integration:
  - CI runs `dotnet test --filter Category=Benchmark` after standard tests.
  - Benchmark tests tagged with `[Trait("Category", "Benchmark")]`.

### Verification

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter Category=Benchmark
# All benchmarks pass against initial baselines
# Intentionally degrade a hot path → verify benchmark fails
# Revert → verify benchmark passes again
```

### Done-when

- Benchmark suite covers all 5 hot paths.
- CI gate blocks merges that regress performance >15%.
- Baselines committed and documented.
- False-positive rate is low (benchmarks use median, warm-up iterations, sufficient samples).
- Adding a new benchmark requires only a new class + baseline entry.

---

## Summary

**Note on CLI verbs used in Verification sections below:** `--bridge-selftest` and `--data-integrity-selftest` already exist today (confirmed in `src/Host/HostCli.cs`) and can be run as-is. `--profile-tick-report`, `--profile-save-report`, and the `--full` flag on `--data-integrity-selftest` do **not** exist yet — they are new CLI verbs this batch proposes to add (Steps 2, 3, and 5 respectively). Each Verification block that references one of these new verbs is only runnable after the corresponding Step's implementation lands; do not treat them as pre-existing smoke tests.

| Step | Title | Files Touched/Created | Risk | Depends On |
|------|-------|----------------------|------|------------|
| 1 | Lightweight profiling infrastructure | `Ashfall.Core/Profiling/ProfileScope.cs`, `ProfileRegistry.cs`, `ProfileEntry.cs`, `src/Host/ProfileOverlay.cs` | None | — |
| 2 | Profile TickSimDay (~20 subsystem calls, several day-gated) | `src/Main.cs` (instrument), `docs/profiling/tick-baseline.md` | None | Step 1 |
| 3 | Profile SaveAll (28 SaveXxx calls, not 24) | `src/Main.cs` (instrument), `Assets/Ashfall.Core/SaveChecksum.cs` (instrument), `docs/profiling/save-baseline.md` | None | Step 1 |
| 4 | Lazy UI panel initialization (60 total inline-constructed fields; 58 are genuine panel/overlay/modal candidates once `_audio`/`_menuContainer` are excluded; requires extracting factory methods first — no `BuildXxxPanel()` methods exist yet) | `src/UI/LazyPanelRegistry.cs`, `src/Main.cs` (refactor BuildUserInterface + audit `AnyOverlayPanelOpen()` and similar direct-field readers) | Medium (revised up from Low-Med — see Step 4 notes) | Step 1 |
| 5 | Optimize CatalogIntegrityValidator (657 lines, not 603) | `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` | Low-Med | Steps 2–3 (to have baselines) |
| 6 | Frame budget monitoring for the flush block (`_Process` runs every frame, ~60Hz, but the 14 `FlushXxxIfDirty()` calls sit behind the same early-return that throttles diagnostics to ~4Hz — they already run at ~4Hz today, not every frame; see corrected Step 6) | `src/Host/FrameBudgetMonitor.cs`, `src/Main.cs` (refactor the throttled flush block inside _Process) | Low-Medium | Step 1 |
| 7 | Benchmark suite (28 subsystems for checksum worst-case, not 24; `FocusChainBuildBenchmark.cs` depends on Batch 57 Step 2) | `Ashfall.Core.Tests/Benchmarks/*.cs`, `scripts/ci/benchmark-gate.sh`, `docs/profiling/benchmark-baselines.json` | None | Steps 1–6 (+ Batch 57 Step 2 for `FocusChainBuildBenchmark.cs` only) |

**Total estimated effort:** 6–8 focused sessions (Step 4 likely needs the upper end of this range given the factory-extraction rework)
**Prerequisite:** None — profiling infrastructure is fully additive; optimizations are gated behind profiling data
**Exit criteria:** Benchmark suite passes in CI; no hot path exceeds its baseline threshold; `--profile-tick-report` and `--profile-save-report` produce actionable data; startup time reduced by lazy panel loading; `_Process` respects frame budget

---

## Review Notes (Corrected)

This batch was adversarially reviewed against the actual codebase at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`. Corrections made:

1. **`SaveChecksum.Compute()` reflection claim — CONFIRMED ACCURATE.** `Assets/Ashfall.Core/SaveChecksum.cs` genuinely walks `type.GetFields(BindingFlags.Public | BindingFlags.Instance)`, sorts ordinally by name, and writes a canonical self-delimiting string before SHA256 hashing. The original plan's characterization of this as a real, non-trivial reflection cost is correct and the profiling work in Steps 3 and 7 is well-targeted.

2. **`BuildUserInterface()` "85+ panels via `BuildXxxPanel()` methods" claim — FALSE, CORRECTED.** Verified by reading `src/Main.cs` lines 640–1283 and grepping for panel construction: `BuildUserInterface()` is one large method that inline-constructs exactly **60** `_field = new Xxx()` assignments (`new XxxPanel()`, `new XxxOverlay()`, `new XxxModal()`, plus `_audio = new AudioManager()` and `_menuContainer = new VBoxContainer()`, which are not panels) and wires their events directly — there is exactly **one** method anywhere in the file matching the `BuildXxxPanel()` naming pattern (`BuildYearOfAshPanel()`, used by an unrelated deferred/on-demand code path, not the eager startup path). Step 4 has been rewritten to describe the real refactor required: extracting each of the 58 genuine panel/overlay/modal blocks into a factory before it can be deferred (excluding `_audio` and `_menuContainer`, which are not deferral candidates), and auditing every direct field read (e.g. `AnyOverlayPanelOpen()`, which dereferences exactly 42 panel fields, verified by direct count) for null-safety once construction becomes lazy. Risk was raised from Low-Med to Medium and effort expectations adjusted accordingly.

3. **`SaveAll()` "24 subsystems" claim — CORRECTED to 28.** Direct count of `SaveXxx();` call lines inside `SaveAll()` in `src/Main.cs` is 28, not 24. (AGENTS.md's "24" figure appears to describe a different/earlier accounting and should not be relied on for this batch's baseline math.) Step 7's `ChecksumBenchmark.cs` originally re-used the stale "24" figure even after this correction was written elsewhere in the same document — fixed to 28 for consistency.

4. **`CatalogIntegrityValidator` "603 lines" claim — CORRECTED to 657** (verified via `wc -l Assets/Ashfall.Core/CatalogIntegrityValidator.cs`).

5. **`TickSimDay` "31+ subsystems" claim — CORRECTED.** The method spans lines 1643–1772 (**130 lines**, not "~90+"), calling 18 distinct `SetupXxx()` operations plus several additional direct calls (`TickPowerGrid`, `TickVerdict`, `_hostEventAdapter?.EvaluateTriggers`), some conditionally gated by day ranges (Year of Ash: days 180–360; Muster: day ≥260). It is not a uniform loop over 31+ systems, so any profiling instrumentation must wrap each call site individually rather than assume a generic iteration point exists. The method also ends by calling `SaveAll()` directly — Step 2 now flags that this makes `TickSimDay.Total` and `SaveAll.Total` (Step 3) overlapping measurements unless the outer scope is deliberately closed before the save call.

6. **`_Process` "flush calls run unconditionally every frame, NOT gated by the 4Hz throttle" claim — FALSE, CORRECTED (found in this pass; the batch's own prior "Review Notes" had this backwards).** Direct read of `src/Main.cs` lines 517–559 shows `_Process` opens with `_diagnosticsAccum += delta; if (_diagnosticsAccum < DiagnosticsRefreshSeconds) return;` (`DiagnosticsRefreshSeconds = 0.25`), and the 14 `FlushXxxIfDirty()` calls are textually located **after** that early return — meaning they are already gated to ~4Hz today, the opposite of what the batch asserted. This is a load-bearing error: the entire premise of Step 6 ("flushes run at 60Hz and need throttling") was inverted. Step 6 has been rewritten around the corrected premise — the real question is whether the *already-throttled* 4Hz flush pass ever exceeds its own budget, not whether flushes need throttling introduced for the first time. Also noted: `FlushCombatIfDirty` and `FlushDoseLedgerIfDirty` exist elsewhere in the file but are **not** among the 14 calls invoked from `_Process` — worth confirming their dirty state is flushed through a different path before assuming Step 6's target list is exhaustive.

7. **CLI verb existence.** `--bridge-selftest` and `--data-integrity-selftest` exist today and are runnable now. `--profile-tick-report`, `--profile-save-report`, and `--data-integrity-selftest --full` do not exist yet; they are deliverables of this batch, not pre-existing smoke tests. Verification blocks were annotated to avoid implying otherwise.

8. **Missing risk/rollback — added.** Step 4 (lazy panel init) is the highest-risk step in this batch because it changes an invariant (all panel fields non-null after `BuildUserInterface()`) that other code silently depends on. A rollback toggle and a manual QA pass opening every panel were added as explicit requirements, and the step's Done-when criteria now require the null-safety audit to be completed or explicitly deferred with a follow-up ticket, rather than assumed complete.

9. **Ordering is logically sound and unchanged:** Step 1 (infra) before Steps 2/3 (profiling) before Steps 4–6 (optimization) before Step 7 (regression gate) is the correct sequence and required no change.

### Second adversarial pass (this review) — errors that survived the first pass

The prior "Review Notes" above were re-verified against the live codebase rather than trusted at face value, because they themselves contained a significant, load-bearing factual error.

10. **The single most serious error in the entire batch: item 6 above (the "`_Process` runs at ~4Hz" correction) had the gating direction backwards.** The prior pass correctly established that `_Process` is called every rendered frame and that only the diagnostics label refresh is throttled — but then asserted the 14 `FlushXxxIfDirty()` calls run "unconditionally on every frame today, gated only by their own per-system dirty bit," independent of that throttle. Reading the literal code order in `src/Main.cs:517-559` shows this is false: the flush calls are placed *after* the `if (_diagnosticsAccum < DiagnosticsRefreshSeconds) return;` guard, in the same method body, with no other early return in between. They inherit the same ~4Hz gate as the diagnostics label. Step 6, which was built entirely on the false premise, has been rewritten: instead of "introduce throttling because flushes currently run at 60Hz," it now correctly frames the work as "add budget monitoring to a flush pass that is already throttled to ~4Hz, and profile first to check whether a real overrun exists before assuming one does."
11. **`TickSimDay`'s line count was still wrong after the first pass ("~90+ lines" vs. actual 130).** Brace-matching from the method's opening line (1643) to its closing brace lands at line 1772 — 130 lines, not "~90+." Fixed in the Context table and Step 2's correction text, along with noting two subsystems (`SetupDisease()`/`SetupSilentFoundry()`) that are ticked inside `TickSimDay` but were never listed anywhere in the batch's "known-heavy systems" or Context table.
12. **`BuildUserInterface()`'s panel-field count was imprecise even after correction ("~58" presented as if independently verified).** The actual count of `_field = new Xxx()` assignments in the method body is exactly **60**, not 58 — the "58" only becomes correct once `_audio` (an `AudioManager`, not a `Control`) and `_menuContainer` (a bare `VBoxContainer`) are explicitly excluded as non-panel infrastructure. The original correction never explained why 58 was the right number instead of 60; this pass adds that explanation and corrects Step 4's "eager fields kept" list from 4 to 6 (`_dashboard`, `_hudOverlay`, `_mainMenu`, `_settingsPanel`, plus `_audio` and `_menuContainer`, which must also stay eager for structural reasons, not because they're visible on the start screen).
13. **Step 7's `ChecksumBenchmark.cs` silently reused the stale "24 subsystems" figure that Review Notes item 3 (above) had already corrected to 28 — in the same document.** This is exactly the kind of drift the correction was meant to prevent; fixed to 28 in Step 7's Implementation.
14. **Step 7's `FocusChainBuildBenchmark.cs` has an undeclared cross-batch dependency.** `FocusChainBuilder.BuildChain` is a deliverable of a *different* roadmap batch (Quality Roadmap Batch 57, Step 2 — UI Accessibility Audit & Keyboard Navigation), not this one. Nothing in the original plan flagged that this benchmark cannot be written until that other batch's class exists. Added an explicit cross-batch dependency note and a Summary-table prerequisite.
15. **`Ashfall.csproj`'s "add to a Debug configuration property group" instruction (Step 1) assumed a conditional property group already exists.** Reading the actual `Ashfall.csproj` shows a single flat, unconditional `<PropertyGroup>` with no `Configuration`-based conditions anywhere in the file. The instruction has been corrected to say this step must *create* a new conditional `PropertyGroup`, not add a line to an existing one — a contributor following the original wording would search for a nonexistent section.
16. **`TickSimDay` calling `SaveAll()` directly, and the resulting overlap between Step 2's and Step 3's profiling scopes, was never mentioned anywhere in the original plan.** Added a note to Step 2 flagging that `TickSimDay.Total` will include the full cost of `SaveAll()` unless deliberately scoped to exclude it, which would otherwise make the Step 2 and Step 3 baseline reports double-count the same work without anyone noticing.
