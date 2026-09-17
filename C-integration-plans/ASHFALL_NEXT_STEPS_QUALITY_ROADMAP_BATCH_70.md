# ASHFALL — Quality Roadmap Batch 70: Error Budget & Observability

**Theme:** Runtime Health Monitoring for Production<br>
**Priority:** MEDIUM-HIGH<br>
**Risk:** Low for Steps 1, 2, 4, 5, 7 (additive, opt-in, no changes to existing behavior). **Medium for Step 3** (instruments the live `TickSimDay` day-tick loop directly — a bug in `ITickInstrumenter` wiring could throw inside the tick loop and break day-advancement, which is core gameplay, not a side channel). **Medium for Step 6** (introduces a new `[input]` binding that doesn't exist yet in `project.godot`, and must be proven not to activate during the project's `godot --headless` verification runs — see Step 6's corrections).<br>
**Layer:** `Assets/Ashfall.Core/Observability/` (engine-agnostic) + `src/Host/` (Godot overlay)

---

## Rationale

ASHFALL currently has compile-time verification (the 5-step CLI pipeline) and startup-time validation (CatalogIntegrityValidator), but **zero runtime observability** (confirmed: `src/Main.cs`'s `TickSimDay` and `_Process` contain no `Stopwatch`, no timing calls, and no metrics collection of any kind as of this review). Once the game is running, developers and playtesters are blind to:

- **Silent performance degradation** — `TickSimDay` advances at least 38 `SetupXxx`-gated subsystems per day-tick (confirmed count of `private void SetupXxx()` methods in `src/Main.cs`; AGENTS.md's H7 cites 31, which is stale — recount before relying on either number), but if one system quietly grows from 0.2ms to 40ms, nobody knows until the frame rate drops noticeably. The dirty-flag flush loop compounds this: `_Process` throttles diagnostics/flush work to `DiagnosticsRefreshSeconds = 0.25` (confirmed constant, i.e. 4Hz — the original "4Hz flush loop" claim was accurate), and 13 of the 16 total `FlushXxxIfDirty` methods run inside that 4Hz batch (`FlushJournalIfDirty` through `FlushPhase0IfDirty`, confirmed by reading `_Process`); the remaining 3 (`FlushDoseLedgerIfDirty`, `FlushCombatIfDirty`, `FlushEconomyIfDirty`) fire on-demand at specific event call sites, not on the timer. None of the 16 currently record latency, meaning a single slow serializer — on the timer or off it — can stall the main thread without attribution.

- **State corruption timing** — SaveChecksum detects corruption *after the fact*, but not *when* it entered the state graph. `Assets/Ashfall.Core/` contains 122 files with a `CaptureState` method and 211 total `CaptureState`-related occurrences (confirmed by grep; this count includes DTOs/interfaces alongside the systems that implement them, so treat it as an upper bound, not a precise "systems" count — AGENTS.md's "82+ systems" figure could not be independently verified against this and should not be repeated without a clearer counting methodology). A bit-flip or logic error in one system's RestoreState can propagate through multiple save cycles before anyone notices the checksum changed. Without drift detection between saves, the corruption window is unbounded.

- **Playtester feedback gaps** — When a tester reports "the game felt laggy around day 45" or "my save seemed broken after the storm event," there is no structured record to correlate against. No timestamps on warnings, no system attribution on errors, no session-level summary of what happened.

- **Catalog drift during gameplay** — CatalogIntegrityValidator runs once at startup. If a mod, hot-reload, or runtime data mutation introduces a dangling reference mid-session, it goes undetected until the next restart.

- **Development velocity** — Without per-system timing, optimization work is guesswork. Without structured logs, reproducing intermittent bugs requires manual printf archaeology. Without session summaries, balancing decisions lack quantitative grounding.

The game is singleplayer and offline. "Production monitoring" here means local diagnostics: rotating log files, in-memory ring buffers, and an opt-in developer overlay. No cloud telemetry, no network calls, no player tracking. The goal is developer and playtester visibility into runtime health — the same information a backend service would get from its APM stack, adapted for a single-process game.

---

## Step 1 — Design Structured Log Format

**Goal:** Replace the flat string-based `ILog` contract with a structured format that supports filtering, correlation, and machine-readable output without breaking existing adapters.

**Implementation:**

1. Define `LogSeverity` enum in `Assets/Ashfall.Core/Observability/LogSeverity.cs`:
   ```csharp
   namespace Ashfall.Core.Observability
   {
       public enum LogSeverity
       {
           Trace = 0,   // Per-tick details, off by default
           Debug = 1,   // Development diagnostics
           Info = 2,    // Normal operation milestones
           Warn = 3,    // Recoverable anomalies
           Error = 4,   // Failures requiring attention
           Fatal = 5    // Unrecoverable, session should end
       }
   }
   ```

2. Define `StructuredLogEntry` record in `Assets/Ashfall.Core/Observability/StructuredLogEntry.cs`:
   ```csharp
   namespace Ashfall.Core.Observability
   {
       public sealed class StructuredLogEntry
       {
           public long TimestampUtcTicks { get; init; }
           public LogSeverity Severity { get; init; }
           public string System { get; init; }        // e.g. "RadiationSystem", "SaveStore"
           public string Message { get; init; }
           public string CorrelationId { get; init; } // groups related entries (e.g. one save cycle)
           public int SimDay { get; init; }           // current in-game day for temporal correlation
           public string Detail { get; init; }        // optional structured payload (JSON fragment)
       }
   }
   ```

3. Define `IStructuredLog` interface in `Assets/Ashfall.Core/Observability/IStructuredLog.cs`:
   ```csharp
   namespace Ashfall.Core.Observability
   {
       public interface IStructuredLog
       {
           void Write(StructuredLogEntry entry);
           void Write(LogSeverity severity, string system, string message,
                      string correlationId = null, string detail = null);
           LogSeverity MinimumSeverity { get; set; }
       }
   }
   ```

4. Implement `ILogAdapter` in `Assets/Ashfall.Core/Observability/ILogAdapter.cs` — a bridge that wraps `IStructuredLog` and exposes the existing `ILog` interface so legacy call sites remain unchanged. **Confirmed against the real interface** (`Assets/Ashfall.Core/Ports.cs`): `ILog` has exactly three members — `Info(string)`, `Warn(string)`, `Error(string)`. There is no `Fatal` on `ILog`, and there never will be one through this bridge (a `LogSeverity.Fatal` entry can only be produced by code that talks to `IStructuredLog` directly, never by legacy `ILog` call sites funneled through this adapter — say so explicitly in the class doc-comment so a future reader doesn't assume the bridge is a complete `LogSeverity` mapping):
   ```csharp
   namespace Ashfall.Core.Observability
   {
       /// <summary>
       /// Adapts legacy Info/Warn/Error call sites onto the structured log.
       /// ILog has no Fatal member — Fatal-severity entries can only be produced
       /// by code written directly against IStructuredLog, never via this bridge.
       /// </summary>
       public sealed class LegacyLogBridge : ILog
       {
           private readonly IStructuredLog _structured;
           private readonly string _defaultSystem;

           public LegacyLogBridge(IStructuredLog structured, string defaultSystem = "General")
           {
               _structured = structured;
               _defaultSystem = defaultSystem;
           }

           public void Info(string msg) => _structured.Write(LogSeverity.Info, _defaultSystem, msg);
           public void Warn(string msg) => _structured.Write(LogSeverity.Warn, _defaultSystem, msg);
           public void Error(string msg) => _structured.Write(LogSeverity.Error, _defaultSystem, msg);
       }
   }
   ```
   Existing implementations to reconcile with, not duplicate: `ConsoleLog` and `NullLog` (`Assets/Ashfall.Core/HostDefaults.cs`) and `GodotLog` (`src/Host/GodotLog.cs`, uses `GD.Print`/`GD.PushWarning`/`GD.PrintErr`) all implement `ILog` today. This step does not replace them — `LegacyLogBridge` is a *fourth* `ILog` implementation, opt-in, that a host can construct in place of `GodotLog`/`ConsoleLog` if it wants structured output. Existing call sites that already hold an `ILog` reference (the overwhelming majority of the codebase) are unaffected either way.

5. All new types go in `Assets/Ashfall.Core/Observability/` — zero engine references, zero external dependencies.

**Verification:**
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` compiles cleanly
- Unit tests assert `LegacyLogBridge` correctly maps Info/Warn/Error to the structured severity levels
- Unit tests assert `MinimumSeverity` filtering suppresses entries below threshold
- Unit test explicitly asserts `LegacyLogBridge` has no code path that emits `LogSeverity.Fatal` (documents the intentional gap rather than leaving it implicit)
- No `UnityEngine.*` or `Godot.*` references in any file under `Observability/`

**Done when:** `IStructuredLog` is defined, `LegacyLogBridge` passes round-trip tests, the existing `ILog` contract remains intact for all current call sites, and `ConsoleLog`/`NullLog`/`GodotLog` continue to compile and pass their existing usages unchanged (no existing `ILog` call site is required to change for this step to be complete).

---

## Step 2 — Implement StructuredLog with Rotating File Output

**Goal:** Provide a concrete `IStructuredLog` implementation that writes JSON-lines to a rotating log file, suitable for post-session analysis by developers and playtesters.

**Implementation:**

1. Create `Assets/Ashfall.Core/Observability/StructuredFileLog.cs`:
   - Implements `IStructuredLog`
   - Writes one JSON object per line (JSON-lines format) to a configurable directory
   - Uses `IFileIO` for all file operations (no direct `System.IO`)
   - Rotation policy: new file per session (named `ashfall_log_{yyyyMMdd_HHmmss}.jsonl`), cap at N files (default 10), delete oldest on overflow
   - Thread-safe write via a simple lock (single-threaded game, but future-proofs for background saves)
   - Flush policy: immediate flush on Warn/Error/Fatal; buffered flush every 5 seconds for Trace/Debug/Info

2. JSON serialization uses `IJsonSerializer` (the core port) — no `JsonUtility`, no `Godot.JSON`:
   ```csharp
   public void Write(StructuredLogEntry entry)
   {
       if (entry.Severity < MinimumSeverity) return;
       var json = _serializer.Serialize(entry);
       _buffer.Add(json);
       if (entry.Severity >= LogSeverity.Warn || ShouldFlush())
           Flush();
   }
   ```

3. Create `Assets/Ashfall.Core/Observability/LogRotation.cs` — pure function that lists existing log files, sorts by creation timestamp embedded in filename, and returns paths to delete.

4. Configuration via `ObservabilityConfig` (plain C# class, loaded from `StreamingAssets/Data/config_observability.json`):
   ```json
   {
       "schema_version": 1,
       "min_severity": "Info",
       "max_log_files": 10,
       "flush_interval_seconds": 5.0,
       "log_directory": "user://logs"
   }
   ```

5. Godot host adapter (`src/Host/Observability/GodotStructuredLog.cs`) resolves `user://logs` to the actual OS path. **Match the codebase's existing convention rather than inventing a new one**: every current save store (confirmed in `src/Host/CaravanSaveStore.cs`, `CombatSaveStore.cs`, `CraftingSaveStore.cs`, `DailyBriefingSaveStore.cs`, and 21 others under `src/Host/*SaveStore.cs`) resolves its path inline via `Path.Combine(ProjectSettings.GlobalizePath("user://"), FileName)` at the point of use, not through a dedicated wrapper class. If `GodotStructuredLog` introduces the first dedicated path-resolution adapter in the project, that's a legitimate improvement, but call it out as a deliberate deviation from the established per-store inline pattern rather than presenting it as "the way this is already done" — a reviewer familiar with the SaveStore files will otherwise flag the inconsistency.

**Risk / rollback:** Low, but note one concrete failure mode: if `StructuredFileLog` is constructed and wired into the boot path (Step 6 of this plan implies it eventually is) before its file-write path is verified writable, a permissions or disk-full error during construction could throw during game boot. Wrap construction in a try/catch that falls back to `NullLog`-equivalent behavior (log to nothing, or to `GD.Print` only) rather than crashing the game — this fallback path needs its own test, not just the happy-path rotation tests below.

**Verification:**
- Unit tests: write 100 entries, verify JSON-lines format parses back into `StructuredLogEntry` objects
- Unit tests: rotation deletes oldest file when cap exceeded (mock `IFileIO`)
- Unit tests: entries below `MinimumSeverity` produce zero writes
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — 0 errors
- `dotnet build Ashfall.csproj` — 0 errors (Godot host adapter compiles)

**Done when:** `StructuredFileLog` passes all tests, log files are human-readable JSON-lines, rotation works, and the Godot host can resolve the output directory.

---

## Step 3 — System Tick Timing Metrics

**Goal:** Instrument `TickSimDay` to record per-system execution duration, exposing a queryable in-memory ring buffer for the developer overlay and session reports.

**Implementation:**

1. Create `Assets/Ashfall.Core/Observability/TickMetrics.cs`:
   ```csharp
   namespace Ashfall.Core.Observability
   {
       public sealed class TickMetrics
       {
           private readonly RingBuffer<TickSnapshot> _history;

           public TickMetrics(int capacity = 256) => _history = new RingBuffer<TickSnapshot>(capacity);

           public void Record(TickSnapshot snapshot) => _history.Add(snapshot);
           public ReadOnlySpan<TickSnapshot> Recent(int count) => _history.Recent(count);
           public TickSnapshot Latest => _history.Latest;
       }

       public sealed class TickSnapshot
       {
           public int SimDay { get; init; }
           public long TotalElapsedUs { get; init; } // microseconds
           public SystemTiming[] Systems { get; init; }
       }

       public sealed class SystemTiming
       {
           public string Name { get; init; }
           public long ElapsedUs { get; init; }
       }
   }
   ```

2. Create `Assets/Ashfall.Core/Observability/RingBuffer.cs` — generic fixed-capacity circular buffer, allocation-free after construction:
   - `Add(T item)` — overwrites oldest when full
   - `Recent(int count)` — returns last N items in order
   - `Latest` — returns most recent item
   - `Count`, `Capacity` properties

3. Instrument the tick loop. In Core, define `ITickInstrumenter`:
   ```csharp
   public interface ITickInstrumenter
   {
       void BeginTick(int simDay);
       void BeginSystem(string systemName);
       void EndSystem(string systemName);
       void EndTick();
   }
   ```
   The Godot host's `TickSimDay` method calls these at the appropriate points. A no-op `NullTickInstrumenter` is the default when observability is disabled.

4. `TickInstrumenter` (concrete implementation) uses `Stopwatch` for timing (not `DateTime.Now` — we need microsecond precision without allocation):
   ```csharp
   public sealed class TickInstrumenter : ITickInstrumenter
   {
       private readonly TickMetrics _metrics;
       private readonly Stopwatch _tickWatch = new();
       private readonly Stopwatch _systemWatch = new();
       private readonly List<SystemTiming> _currentTimings = new();
       // ...
   }
   ```

5. Emit a structured log entry (severity Trace) for any system exceeding a configurable threshold (default: 5ms). This gives automatic slow-system alerts without polling.

6. Wire into Godot host: `Main.cs` constructs `TickInstrumenter` during setup, passes it to the tick loop, and exposes `TickMetrics` for the overlay (Step 6).

**Risk / rollback:** This step directly instruments `TickSimDay`, the method that drives every survival mechanic in the game (38 `SetupXxx` calls per day per the Rationale section). Wrap every `BeginSystem`/`EndSystem` call pair around each subsystem's tick call in try/finally so an instrumentation failure (e.g. mismatched Begin/End calls if a subsystem throws mid-tick) cannot itself throw and abort the day-tick — an exception from the *instrumenter* must never be indistinguishable from an exception in actual gameplay logic when a playtester reports a crash. Rollback is a single-line change: constructing `NullTickInstrumenter` instead of `TickInstrumenter` in `Main.cs`'s setup disables all Step 3 behavior without touching any other step, so keep that swap trivial and call it out in the PR description.

**Verification:**
- Unit tests: `RingBuffer` overwrites correctly, `Recent` returns correct order
- Unit tests: `TickInstrumenter` records accurate timings (within 1ms tolerance using `Thread.Sleep` in test)
- Unit tests: `NullTickInstrumenter` has zero overhead (no allocations, no side effects)
- Unit tests: threshold alert emits structured log entry only when exceeded
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — 0 errors
- `dotnet build Ashfall.csproj` — 0 errors

**Done when:** Every system tick records per-system timing to a ring buffer, slow systems emit trace-level alerts, and the `NullTickInstrumenter` path adds zero overhead when observability is disabled.

---

## Step 4 — Save Health Metrics

**Goal:** Instrument the save/load pipeline to track serialization time per store, total save size, checksum computation time, and save frequency — enabling detection of save bloat, slow stores, and corruption timing.

**Implementation:**

1. Create `Assets/Ashfall.Core/Observability/SaveMetrics.cs`:
   ```csharp
   namespace Ashfall.Core.Observability
   {
       public sealed class SaveMetrics
       {
           private readonly RingBuffer<SaveSnapshot> _history;

           public SaveMetrics(int capacity = 64) => _history = new RingBuffer<SaveSnapshot>(capacity);

           public void Record(SaveSnapshot snapshot) => _history.Add(snapshot);
           public SaveSnapshot Latest => _history.Latest;
           public IReadOnlyList<SaveSnapshot> All => _history.ToList();
       }

       public sealed class SaveSnapshot
       {
           public int SimDay { get; init; }
           public long TotalElapsedUs { get; init; }
           public long ChecksumElapsedUs { get; init; }
           public long TotalSizeBytes { get; init; }
           public string Checksum { get; init; }
           public string PreviousChecksum { get; init; }  // drift detection
           public StoreMetric[] Stores { get; init; }
       }

       public sealed class StoreMetric
       {
           public string StoreName { get; init; }
           public long SerializeUs { get; init; }
           public long SizeBytes { get; init; }
           public bool ChecksumChanged { get; init; } // vs previous save
       }
   }
   ```

2. Create `ISaveInstrumenter` interface (mirrors tick pattern):
   ```csharp
   public interface ISaveInstrumenter
   {
       void BeginSave(int simDay);
       void BeginStore(string storeName);
       void EndStore(string storeName, long sizeBytes, string storeChecksum);
       void EndSave(string totalChecksum);
   }
   ```

3. Implement `SaveInstrumenter` using `Stopwatch`, recording into `SaveMetrics`.

4. Instrument the Godot host's `SaveAll` method (the orchestrator for all save stores in `src/Main.cs`, defined at line 6227): **corrected count** — there are 25 distinct `*SaveStore.cs` files under `src/Host/` (confirmed via `ls src/Host/*SaveStore.cs`), and `src/Main.cs` has 30 `private void Save[A-Z]...` methods including `SaveAll` itself. The "24 save stores" figure from AGENTS.md's SAVE/LOAD section is close but not exact — recount at implementation time rather than hardcoding either the AGENTS.md figure or this review's figure, since both are snapshots that will drift as new stores are added.
   - Call `BeginSave` before the loop
   - Call `BeginStore`/`EndStore` around each store's `CaptureState` + serialize
   - Call `EndSave` after checksum computation

5. Emit structured log entries:
   - Info: every save (day, total size, total time, checksum)
   - Warn: if any store's size grew >50% since last save (potential state leak)
   - Warn: if total save time exceeds 100ms (potential frame hitch on autosave)
   - Error: if checksum matches previous despite state changes (possible serialization bug)

6. `NullSaveInstrumenter` for disabled observability (zero overhead).

**Verification:**
- Unit tests: `SaveInstrumenter` records correct per-store timings and sizes
- Unit tests: size-growth warning triggers at >50% threshold
- Unit tests: checksum-unchanged error triggers correctly
- Unit tests: `NullSaveInstrumenter` allocates nothing
- **Integration test — corrected to be runnable:** as with Step 7's report verification, name an actual CLI verb rather than a generic "run a save cycle" instruction. Reuse or extend an existing save-focused selftest verb (`--holdfast-save-selftest` already exists per `HostCli.cs` and exercises a save/load round trip) to additionally assert a `SaveMetrics` entry was recorded, or add a new `--observability-save-selftest` verb following the same convention. State which one explicitly at implementation time.
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — 0 errors
- `dotnet build Ashfall.csproj` — 0 errors

**Done when:** Every save cycle records per-store metrics, anomalies emit warnings/errors to the structured log, and the metrics are queryable by the overlay and session report.

---

## Step 5 — Runtime Integrity Checks

**Goal:** Extend integrity validation beyond startup to detect catalog drift and state corruption during gameplay, with configurable frequency to avoid performance impact.

**Implementation:**

1. Create `Assets/Ashfall.Core/Observability/RuntimeIntegrityMonitor.cs`:
   ```csharp
   namespace Ashfall.Core.Observability
   {
       public sealed class RuntimeIntegrityMonitor
       {
           private readonly CatalogIntegrityValidator _catalogValidator;
           private readonly IStructuredLog _log;
           private readonly int _checkIntervalDays; // how often to run (default: every 7 sim-days)
           private int _lastCheckDay;

           public RuntimeIntegrityMonitor(
               CatalogIntegrityValidator catalogValidator,
               IStructuredLog log,
               int checkIntervalDays = 7)
           { /* ... */ }

           /// <summary>Called each sim-day tick. Runs validation when interval elapsed.</summary>
           public IntegrityResult MaybeCheck(int currentSimDay) { /* ... */ }
       }
   }
   ```

2. **Catalog spot-checks** — Instead of running the full 5-tier validation every N days (too expensive), run one randomly-selected tier per check using `ISeededRng`:
   - Day 7: run REGISTRY check on a random 20% of files
   - Day 14: run TIER-1 (reference resolution) on 10% of definitions
   - Day 21: run UNIQUENESS check (full, it's fast)
   - Cycle repeats

3. **State drift detection** — Compare checksums of `CaptureState()` output between consecutive save cycles:
   ```csharp
   public sealed class StateDriftDetector
   {
       private readonly Dictionary<string, string> _previousHashes = new();

       public DriftReport DetectDrift(IReadOnlyDictionary<string, string> currentHashes)
       {
           var drifted = new List<string>();
           var newSystems = new List<string>();
           foreach (var (system, hash) in currentHashes)
           {
               if (_previousHashes.TryGetValue(system, out var prev))
               {
                   if (prev != hash) drifted.Add(system);
               }
               else newSystems.Add(system);
           }
           _previousHashes.Clear();
           foreach (var kv in currentHashes) _previousHashes[kv.Key] = kv.Value;
           return new DriftReport(drifted, newSystems);
       }
   }
   ```

4. **Unexpected-no-drift alert** — If 10+ sim-days pass with zero state changes across all tracked systems, emit a warning (likely a bug: the game is running but nothing is changing). Do not hardcode "82 systems" into this alert's logic or messaging — this plan could not independently verify that figure (see Rationale section); the alert should compare against the *actual* count of systems registered with `StateDriftDetector` at runtime, which will self-correct as systems are added or removed.

5. **Configuration** in `config_observability.json`:
   ```json
   {
       "runtime_integrity": {
           "enabled": true,
           "catalog_check_interval_days": 7,
           "catalog_sample_percent": 20,
           "drift_stale_threshold_days": 10
       }
   }
   ```

6. Wire into the Godot host's day-tick: call `RuntimeIntegrityMonitor.MaybeCheck(currentSimDay)` at the end of `TickSimDay`, after all systems have ticked.

**Risk / rollback:** Low overall (the check is sampled and interval-gated), but flag one real risk: `CatalogIntegrityValidator` (`Assets/Ashfall.Core/CatalogIntegrityValidator.cs`, 603 lines per AGENTS.md — re-verified during this review via `wc -l`, the file is actually 657 lines today, another instance of an AGENTS.md figure that has drifted since it was last updated; recount at implementation time rather than citing either number as current) was designed and tested as a one-shot startup check; running a *subset* of its tiers mid-session is new usage this class was not originally built for. Before wiring `MaybeCheck` into the live tick loop, verify `CatalogIntegrityValidator`'s tier methods are safely re-entrant / side-effect-free when called repeatedly with different sample sets — if the validator mutates any internal cache assuming "called exactly once at boot," this step could introduce false-positive integrity failures. Rollback: `RuntimeIntegrityMonitor` construction can be skipped entirely (feature-flagged via `config_observability.json`'s `runtime_integrity.enabled`) without touching `TickSimDay`'s other behavior.

**Verification:**
- Unit tests: `MaybeCheck` skips when interval not elapsed
- Unit tests: `MaybeCheck` runs the correct tier on the correct cycle
- Unit tests: `StateDriftDetector` correctly identifies changed/unchanged/new systems
- Unit tests: stale-state warning fires after threshold days with no drift
- Unit tests: no `UnityEngine.*` or `Godot.*` in any file
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — 0 errors
- `dotnet build Ashfall.csproj` — 0 errors
- `godot --headless --path . -- --data-integrity-selftest` — still 0 errors (runtime checks don't affect startup validator)

**Done when:** Periodic catalog spot-checks and state drift detection run during gameplay, anomalies are logged with system attribution, and the checks add negligible overhead (< 1ms per triggered check).

---

## Step 6 — Developer Console Overlay

**Goal:** Provide an in-game F12 panel (Godot UI) displaying live metrics, recent warnings, system timing breakdown, and save health — enabling real-time debugging during playtesting without external tools.

**Corrections before implementing:**
- **`project.godot` currently has no `[input]` section at all** (confirmed by reading the full file — it only has `[application]`, `[display]`, `[dotnet]`, `[rendering]`, `[gui]`). The `toggle_debug_overlay` InputMap action referenced below does not exist and must be added as an explicit sub-step (edit `project.godot` to add an `[input]` section with the F12 binding, or configure it via the Godot editor and let it serialize the section) — the plan cannot simply say "Toggle visibility with F12 (InputMap action `toggle_debug_overlay`)" as if the action already exists.
- **`OS.IsDebugBuild()` usage was not found anywhere in the current codebase** (confirmed via grep across `src/`). This may still be the correct Godot 4.7 C# API for this purpose, but since nothing in this project currently exercises it, verify it against the actual Godot 4.7 `OS` class API during implementation rather than assuming — if it doesn't behave as expected in exported headless/CI builds (the project's `--headless` verification path), the overlay could accidentally activate in a build meant to be silent. Add an explicit test/manual-check for this interaction with headless mode, since `godot --headless` is the project's primary verification path and step 6's UI work must not break it.
- **`src/UI/` is a flat directory today** — confirmed via `ls src/UI/`, it contains ~dozens of panel `.cs` files (`AchievementDetailPanel.cs`, `AshfallDashboardShell.cs`, `AshfallSidebar.cs`, etc.) with no subdirectories. This step's `src/UI/DebugOverlay/` subdirectory would be the first subdirectory under `src/UI/`. That's not necessarily wrong, but flag it as a convention change rather than assuming it matches existing practice — check whether the project has an unstated "flat `src/UI/`" convention worth preserving before introducing the first nested folder.
- **No `.tscn` file in the repository currently uses a `CanvasLayer` node** (confirmed via grep across all `.tscn` files — zero matches). The existing panels are evidently wired some other way (likely as children of the main scene tree or via direct `Control` instancing from `src/Main.cs`, matching the flat `src/UI/` file list above). Before committing to `CanvasLayer` as the overlay's root, check how `AshfallDashboardShell.cs` or another existing top-level panel actually gets attached to the scene tree, and follow that pattern unless there's a specific reason `CanvasLayer` (an always-on-top rendering layer) is genuinely needed here — it likely is, given the "always on top" requirement, but the plan should show that reasoning rather than presenting it as the obvious/only choice.

**Implementation:**

1. Create `src/UI/DebugOverlay/DebugOverlayPanel.tscn` — a `CanvasLayer` (layer 100, always on top) with:
   - `PanelContainer` with semi-transparent dark background
   - Tab bar: **Timing** | **Saves** | **Log** | **Integrity**
   - Toggle visibility with F12 (InputMap action `toggle_debug_overlay`)
   - Hidden by default in release builds (check `OS.IsDebugBuild()`)

2. Create `src/UI/DebugOverlay/DebugOverlayController.cs` (Godot Node script):
   ```csharp
   namespace AtomicWar.GodotApp.UI
   {
       public partial class DebugOverlayController : Control
       {
           private TickMetrics _tickMetrics;
           private SaveMetrics _saveMetrics;
           private IStructuredLog _log;

           public override void _Input(InputEvent @event)
           {
               if (@event.IsActionPressed("toggle_debug_overlay"))
                   Visible = !Visible;
           }

           public override void _Process(double delta)
           {
               if (!Visible) return;
               UpdateActiveTab();
           }
       }
   }
   ```

3. **Timing tab** — displays:
   - Last tick total duration (bar, color-coded: green < 5ms, yellow < 16ms, red > 16ms)
   - Per-system breakdown (sorted descending by duration, top 10)
   - Rolling average over last 30 ticks
   - Sparkline of total tick duration over last 256 ticks

4. **Saves tab** — displays:
   - Last save: time, size, checksum (truncated)
   - Per-store size breakdown (bar chart)
   - Save frequency (saves per real-minute)
   - Size trend (growing/stable/shrinking)

5. **Log tab** — displays:
   - Last 50 log entries (Warn and above by default, filterable)
   - Color-coded by severity
   - Clickable to expand Detail field
   - Filter by system name

6. **Integrity tab** — displays:
   - Last integrity check: day, tier, result (pass/fail count)
   - State drift summary: which systems changed since last save
   - Stale-state alert if applicable

7. Performance constraint: the overlay itself must not exceed 0.5ms per frame when visible. Use dirty-flag updates (only redraw when new data arrives), not per-frame full rebuilds.

**Verification:**
- `dotnet build Ashfall.csproj` — 0 errors (overlay script compiles)
- Manual test: launch game, press F12, verify panel appears with placeholder data
- Manual test: verify panel hidden when the debug-build check returns false (or, if that check proves unreliable per the correction above, whatever gating mechanism is actually implemented)
- Manual test: verify F12 toggle works, tab switching works
- **Headless regression check (concrete, not vague):** run `godot --headless --path . -- --data-integrity-selftest` and `godot --headless --path . -- --bridge-selftest` after this step lands; both must still exit 0 and must not attempt to construct `DebugOverlayController` or any `Control`/`CanvasLayer` node in headless mode (headless has no renderer — a UI node construction attempt would be the actual failure mode, not just "FPS check," which doesn't apply to a headless run at all)
- Performance: overlay visible does not reduce measured FPS by more than 5 frames relative to overlay-hidden, measured via the existing `_diagnosticsLabel`/`Engine.GetFramesPerSecond()` readout already wired in `_Process` (confirmed to exist at `src/Main.cs` — reuse it rather than inventing a separate benchmark harness for this one check). The original "does not drop below 55 FPS on reference hardware" criterion is unrunnable as written: there is no defined "reference hardware" for this project, and an absolute FPS floor conflates overlay cost with baseline scene cost, which varies by day/system load.
- No Core references to Godot UI types (overlay is purely host-side)

**Done when:** F12 opens a tabbed debug panel showing live tick timing, save health, recent log entries, and integrity status; the panel does not activate in the two existing `godot --headless` selftest verbs; and overlay-visible vs. overlay-hidden FPS delta is measured and recorded (not just asserted to be "negligible").

---

## Step 7 — Session Summary Report

**Goal:** On game-over or voluntary quit, generate a structured session report summarizing gameplay metrics, system health, and anomalies — providing playtesters and developers with a post-mortem artifact for balancing and debugging.

**Implementation:**

1. Create `Assets/Ashfall.Core/Observability/SessionReport.cs`:
   ```csharp
   namespace Ashfall.Core.Observability
   {
       public sealed class SessionReport
       {
           // Gameplay
           public int DaysSurvived { get; init; }
           public string CauseOfDeath { get; init; }  // null if quit voluntarily
           public float TotalPlayTimeMinutes { get; init; }
           public int SaveCount { get; init; }

           // System interaction
           public Dictionary<string, int> SystemTickCounts { get; init; }  // which systems ticked
           public List<string> MilestoneEvents { get; init; }  // first-time triggers

           // Health
           public int TotalWarnings { get; init; }
           public int TotalErrors { get; init; }
           public List<string> TopWarningsBySystem { get; init; }  // top 5 warning sources
           public float AvgTickDurationUs { get; init; }
           public float MaxTickDurationUs { get; init; }
           public string SlowestSystem { get; init; }
           public long FinalSaveSizeBytes { get; init; }

           // Integrity
           public int IntegrityChecksRun { get; init; }
           public int IntegrityFailures { get; init; }
           public int DriftEventsDetected { get; init; }
       }
   }
   ```

2. Create `Assets/Ashfall.Core/Observability/SessionReportBuilder.cs` — accumulates data throughout the session via method calls from the host:
   ```csharp
   public sealed class SessionReportBuilder
   {
       private readonly Stopwatch _sessionClock = Stopwatch.StartNew();
       private int _saveCount;
       private int _warnings;
       private int _errors;
       // ...

       public void OnSave() => _saveCount++;
       public void OnWarning(string system) { _warnings++; /* track per-system */ }
       public void OnError(string system) { _errors++; }
       public void OnSystemTick(string system, long elapsedUs) { /* accumulate */ }
       public void OnIntegrityCheck(bool passed) { /* ... */ }
       public void OnDriftDetected() => _driftEvents++;

       public SessionReport Build(int daysSurvived, string causeOfDeath)
       {
           _sessionClock.Stop();
           return new SessionReport { /* ... */ };
       }
   }
   ```

3. On session end (game-over or quit), the Godot host calls `Build()` and writes the report:
   - JSON file: `user://reports/session_{yyyyMMdd_HHmmss}.json`
   - Also append a human-readable summary to the structured log (Info severity)
   - Rotate reports (keep last 20)

4. **Human-readable summary** format (written to log):
   ```
   === SESSION REPORT ===
   Days survived: 47 | Cause: radiation_poisoning
   Play time: 2h 14m | Saves: 12
   Avg tick: 1.2ms | Max tick: 18.4ms (WeatherSystem, day 31)
   Warnings: 7 (RadiationSystem: 3, SaveStore: 2, NeedsSystem: 2)
   Errors: 0 | Integrity failures: 0 | Drift events: 2
   Final save: 847KB
   ========================
   ```

5. Wire into Godot host: `Main.cs` constructs `SessionReportBuilder` at session start, passes it to the structured log (as a listener for warn/error counts), tick instrumenter, and save instrumenter. On session end, calls `Build()` and writes. **Confirmed hook point exists:** `src/Main.cs:611` already implements `public override void _Notification(int what)` with a check for `NotificationWMCloseRequest` at line 613 — wire the report write into this existing method rather than adding a second, competing `_Notification` override (Godot Nodes should have exactly one `_Notification` override; adding a second would either fail to compile or silently shadow the first, so this must extend the existing method body, not add a new one).

6. Configuration in `config_observability.json`:
   ```json
   {
       "session_reports": {
           "enabled": true,
           "max_reports": 20,
           "report_directory": "user://reports"
       }
   }
   ```

**Verification:**
- Unit tests: `SessionReportBuilder` correctly accumulates warnings, errors, tick counts
- Unit tests: `Build()` produces correct totals and identifies slowest system
- Unit tests: report JSON round-trips through `IJsonSerializer`
- **Integration test — corrected to be runnable:** the original "play 5 sim-days in headless mode, quit, verify report file exists" step names no actual command. This project's headless verification is entirely driven by CLI verbs dispatched in `src/Host/HostCli.cs` (confirmed: dozens of `Has(args, "--xxx-selftest")` checks, e.g. `--holdfast-selftest`, `--day1-to-day2-selftest`, `--core-selftest`). Add a new verb following that exact convention — e.g. `--observability-selftest` — that calls `TickSimDay` a fixed number of times (reusing the existing internal self-test pattern already visible in `src/Main.cs` around line 4292's `TickSimDay(277)`–`TickSimDay(280)` sequence used by the treaty-assessment self-test), then forces a `SessionReportBuilder.Build()` + write, then asserts the report file exists and parses as valid JSON. Register it in `HostCli.cs`'s help text alongside the other verbs. Run it as: `godot --headless --path . -- --observability-selftest`.
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — 0 errors
- `dotnet build Ashfall.csproj` — 0 errors
- `godot --headless --path . -- --observability-selftest` — new verb added by this step, must exit 0

**Done when:** Every session produces a JSON report and human-readable log summary capturing gameplay duration, system health, anomaly counts, and performance metrics; reports rotate; and the new `--observability-selftest` CLI verb is wired into `HostCli.cs` and its help text, exiting 0 in headless mode.

---

## Summary Table

| Step | Deliverable | Layer | Key Files | Risk |
|------|-------------|-------|-----------|------|
| 1 | Structured log format + `IStructuredLog` | Core | `Observability/LogSeverity.cs`, `StructuredLogEntry.cs`, `IStructuredLog.cs`, `LegacyLogBridge.cs` | None — additive interfaces |
| 2 | `StructuredFileLog` with rotation | Core + Host | `Observability/StructuredFileLog.cs`, `LogRotation.cs`, `src/Host/Observability/GodotStructuredLog.cs` | Low — file I/O through `IFileIO` port |
| 3 | Per-system tick timing | Core + Host | `Observability/TickMetrics.cs`, `RingBuffer.cs`, `ITickInstrumenter.cs`, `TickInstrumenter.cs` | Low — opt-in instrumentation, null path for disabled |
| 4 | Save health metrics | Core + Host | `Observability/SaveMetrics.cs`, `ISaveInstrumenter.cs`, `SaveInstrumenter.cs` | Low — wraps existing save path |
| 5 | Runtime integrity checks | Core + Host | `Observability/RuntimeIntegrityMonitor.cs`, `StateDriftDetector.cs` | Low — periodic, sampled, configurable |
| 6 | Developer console overlay | Host (Godot) | `src/UI/DebugOverlay/DebugOverlayPanel.tscn`, `DebugOverlayController.cs` | Low — debug-only UI, hidden in release |
| 7 | Session summary report | Core + Host | `Observability/SessionReport.cs`, `SessionReportBuilder.cs` | None — write-only output on session end |

---

## Dependencies & Ordering

```
Step 1 (format) ─┬─► Step 2 (file output) ──────────────────┐
                 │                                           │
                 ├─► Step 3 (tick metrics) ──┐                │
                 │                            ├──► Step 6 (overlay)
                 ├─► Step 4 (save metrics) ──┘         │
                 │                                     │
                 └─► Step 5 (integrity) ───────────────┘
                                                       │
                                                       ▼
                                               Step 7 (session report)
```

Steps 3, 4, and 5 are independent of each other and can be parallelized after Step 1+2.<br>
Step 6 requires 3+4+5 (displays their timing/save/integrity data) **and Step 2** (the Log tab in Step 6's implementation reads from `IStructuredLog`/`StructuredFileLog` output, not just from Steps 3–5's metrics — the original diagram omitted this edge even though Step 6's own "Log tab" bullet describes exactly this dependency).<br>
Step 7 requires all prior steps (aggregates everything).

---

## Non-Goals (Explicit Exclusions)

- **Cloud telemetry** — No network calls, no analytics services, no player tracking
- **Crash reporting services** — No Sentry, Bugsnag, or equivalent; local logs suffice
- **Performance profiling** — This is observability, not a profiler; use Godot's built-in profiler for deep frame analysis
- **Save file repair** — Detect corruption, don't auto-fix it; repair is a separate feature
- **Mod support** — Integrity checks may flag mod-introduced data; mod-aware validation is future work
- **UI polish** — The debug overlay is developer-facing; it does not need to match the game's art direction


---

## Review Notes (Corrected)

This plan was adversarially reviewed against the live repository at
`/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War` before editing. Unlike Batch 69,
this plan's core premise holds up: `src/Main.cs`'s `TickSimDay` and `_Process` genuinely contain
zero timing/metrics infrastructure (no `Stopwatch`, no ring buffers, no structured logging) as of
this review, so "zero runtime observability" is an accurate starting claim. Findings requiring
correction:

1. **`ILog` was verified against `Assets/Ashfall.Core/Ports.cs`** — it has exactly `Info(string)`,
   `Warn(string)`, `Error(string)`. No `Fatal`. The original `LegacyLogBridge` example silently
   implied a complete severity mapping by only showing the three methods without comment; corrected
   to explicitly document that `Fatal`-severity entries can never originate from a legacy `ILog`
   call site through this bridge — a real, if minor, design gap that a reviewer would otherwise
   have to notice on their own.
2. **Existing `ILog` implementations were enumerated and are unaffected by this plan**: `ConsoleLog`
   and `NullLog` in `Assets/Ashfall.Core/HostDefaults.cs`, and `GodotLog` in `src/Host/GodotLog.cs`
   (uses `GD.Print`/`GD.PushWarning`/`GD.PrintErr`). The original plan didn't acknowledge these
   existed, which reads as if `LegacyLogBridge` might replace them; corrected to state it's a
   fourth, opt-in implementation that doesn't require touching existing call sites.
3. **The "4Hz flush loop" and "14 dirty-flag flushes" claims were checked line-by-line against
   `src/Main.cs`.** The 4Hz figure is accurate (`DiagnosticsRefreshSeconds = 0.25`, confirmed
   constant). The flush count was off: there are 16 total `FlushXxxIfDirty` methods, but only 13
   run inside the `_Process` 4Hz batch (`FlushJournalIfDirty` through `FlushPhase0IfDirty`); the
   other 3 (`FlushDoseLedgerIfDirty`, `FlushCombatIfDirty`, `FlushEconomyIfDirty`) fire at specific
   event call sites elsewhere in `Main.cs`, not on the timer. Corrected in the Rationale section.
4. **"82+ systems implementing CaptureState" could not be verified.** `Assets/Ashfall.Core/`
   contains 122 files matching `CaptureState` and 211 total occurrences of the term (methods,
   interfaces, and DTOs combined) — neither number cleanly resolves to "82." AGENTS.md's own text
   uses "82+ systems" without a documented counting method. Corrected to report the actual grep
   results as an upper bound and flag the AGENTS.md figure as unverifiable rather than repeating it
   as fact; Step 5's no-drift-alert threshold was changed from a hardcoded "82" to "compare against
   the actual runtime-registered count."
5. **Save store count was off by one and imprecisely labeled.** AGENTS.md's SAVE/LOAD section says
   "24 save stores" (in the context of `SaveAll`); the actual count of `src/Host/*SaveStore.cs`
   files is 25, and `src/Main.cs` has 30 methods matching `private void Save[A-Z]...` (including
   `SaveAll` itself, confirmed at line 6227). Corrected Step 4 to cite the verified numbers and warn
   against hardcoding any of them into new code, since they will drift.
6. **`GodotStructuredLog`'s proposed `user://` resolution via a dedicated adapter class does not
   match the codebase's actual convention.** Every existing `*SaveStore.cs` (25 files, spot-checked
   4) resolves its path inline via `Path.Combine(ProjectSettings.GlobalizePath("user://"), FileName)`
   at the point of use — there is no existing dedicated path-resolution wrapper class anywhere in
   `src/Host/`. The original Step 2 phrased the adapter as if it were following an established
   pattern; corrected to call out that it would be the first such wrapper, which may still be the
   right call but should be presented as a deliberate deviation, not business-as-usual.
7. **Step 6 (Developer Console Overlay) had the most unverified assumptions in the plan:**
   - `project.godot` has **no `[input]` section** (confirmed by reading the full file). The
     `toggle_debug_overlay` InputMap action does not exist; the original plan referenced it as if
     it were already configured.
   - `OS.IsDebugBuild()` has zero usages anywhere in the current codebase (confirmed via grep) —
     not proven wrong, but proven *unestablished*, and its interaction with the project's primary
     `godot --headless` verification path was never addressed by the original plan. This is a real
     risk: if the debug-build check doesn't behave as expected headless, the overlay's UI
     construction could break the two existing `--data-integrity-selftest`/`--bridge-selftest`
     headless verbs that this project's entire verification pipeline depends on.
   - `src/UI/` is confirmed flat (no subdirectories today); `src/UI/DebugOverlay/` would be the
     first nested folder. Not wrong, but the original plan didn't flag it as a convention change.
   - No `.tscn` file anywhere in the repository uses `CanvasLayer` (confirmed via grep, zero
     matches) — existing panels are wired some other way. The original plan presented `CanvasLayer`
     as the obvious choice without checking how existing top-level panels (e.g.
     `AshfallDashboardShell.cs`) actually attach to the scene tree.
   - The original performance criterion ("does not drop below 55 FPS on reference hardware") is
     unrunnable — there's no defined reference hardware for this project, and it doesn't isolate
     overlay cost from baseline scene cost. Replaced with a relative FPS-delta check that reuses the
     `_diagnosticsLabel`/`Engine.GetFramesPerSecond()` instrumentation already present in `Main.cs`.
8. **Two verification steps referenced integration tests with no actual runnable command:**
   Step 4's "run a save cycle in Godot headless, verify log file contains save metrics entry" and
   Step 7's "play 5 sim-days in headless mode, quit, verify report file exists" both named no CLI
   verb. This project's headless verification is entirely built on named `--xxx-selftest` verbs
   dispatched in `src/Host/HostCli.cs` (confirmed: dozens of existing examples, e.g.
   `--holdfast-selftest`, `--day1-to-day2-selftest`, `--core-selftest`). Corrected both steps to
   either extend an existing verb (`--holdfast-save-selftest`, confirmed to exist) or specify adding
   a new one (`--observability-selftest`) following the established pattern, registered in
   `HostCli.cs`'s help text — matching how every other headless-testable feature in this project is
   actually exercised.
9. **The Dependencies & Ordering diagram omitted an edge.** Step 6's own "Log tab" bullet describes
   reading from the structured log (Step 2's output), but the dependency diagram only showed Step 6
   depending on Steps 3, 4, and 5. Corrected to add the Step 2 → Step 6 edge explicitly.
10. **Risk was blanket-labeled "Low" for the entire batch**, which undersells Step 3 (instruments
    the live `TickSimDay` day-tick loop that drives every survival mechanic — a wiring bug here can
    break core gameplay, not just observability) and Step 6 (introduces a new input binding and an
    unverified headless-mode interaction). Corrected the top-level Risk line to differentiate by
    step, and added explicit risk/rollback notes to Steps 2, 3, 5, and 6 (Steps 1, 4, and 7 remain
    genuinely low-risk additive work and did not need a rollback note beyond what's already
    documented).
11. **Two numeric claims from an earlier pass of this same document were themselves slightly off
    and are corrected here:** the `CaptureState`-occurrence grep count was recorded as 203 in an
    earlier draft; re-running the exact same `grep -ro "CaptureState" Assets/Ashfall.Core/ | wc -l`
    command during this pass returns 211 — corrected in both the Rationale section and this notes
    section. Separately, `CatalogIntegrityValidator.cs`'s "603 lines per AGENTS.md" citation in
    Step 5's risk note was accurate as an *attribution* (AGENTS.md does say 603), but the file's
    actual current length is 657 lines (`wc -l`) — added a parenthetical noting the drift rather
    than silently leaving a citation that reads as if it were independently verified when it
    wasn't. Neither error changes any conclusion in this plan, but both are corrected for the same
    reason the rest of this batch insists on re-deriving numbers from source: stale figures compound
    if left uncorrected once copied forward.
