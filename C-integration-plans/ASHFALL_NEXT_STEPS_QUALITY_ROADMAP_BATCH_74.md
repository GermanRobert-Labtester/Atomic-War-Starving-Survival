# ASHFALL — Quality Roadmap Batch 74

## Theme: Memory Management & Object Pooling for Long Sessions

| Field | Value |
|-------|-------|
| **Batch** | 74 |
| **Priority** | MEDIUM |
| **Risk** | Low-Medium |
| **Estimated Effort** | 5-7 working days |
| **Prerequisite Batches** | None for Steps 1, 2, 5, 6 (standalone). Steps 3 and 4 are now conditional on Step 1's profiling results (see corrections below) — do not implement pooling for `NarrativeEncounterSystem`/`WeatherSystem` until the baseline test confirms they are measurably hot; otherwise this is speculative optimization with real risk (stale-data bugs from pooling) and no measured benefit. |
| **Systems Touched** | ObjectPool (new), NarrativeEncounterSystem (conditional on Step 1 profiling confirming it's hot — see Motivation correction), WeatherSystem (RNG-instance pooling, not "event" pooling), SaveChecksum (field reflection, not property reflection), JournalSystem, EulogySystem, TradeTellSystem. `CatalogIntegrityValidator` removed from scope — see Motivation correction (it is not an allocation hotspot; its `IdPrefixes` array is allocated once, not per-check). |

---

## Motivation

The game supports 300+ day sessions. Some `TickSimDay`-adjacent work does allocate: save/load DTOs, journal/eulogy/trade text, and per-tick RNG instances. Godot's C# runs on .NET 8 with an excellent generational GC, but allocation-heavy hot paths can still produce observable stalls — especially during `SaveAll()` (`src/Main.cs:6227`, serializes 24 systems via per-system `SaveXxx` methods, allocates temporary strings/DTOs).

### Correction: claimed hot allocators, verified against source
The original motivation section listed five specific "known hot allocators." Reading the actual
files shows two of the five are mischaracterized and one is imprecise. Corrected list:

- **`NarrativeEncounterSystem`** (`Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs`) —
  **not a hot allocator as described.** `SelectEncounter()` (the per-encounter-check path) walks
  the existing `_catalog` list and allocates nothing per call — it's a weighted-roll accumulator
  over already-live objects. The only allocation is one `EncounterResolutionRecord` per
  `Resolve()` call, which fires once per *player choice*, not once per encounter roll — this is
  low-frequency (bounded by player decisions per day, not tick rate) and a poor target for
  pooling relative to its actual call frequency. Verify allocation frequency with Step 1's
  profiling *before* committing to Step 3's pooling work — it may not be a hotspot at all.
- **`WeatherSystem`** (`Assets/Ashfall.Core/World/WeatherSystem.cs`) — **there is no heap-allocated
  "weather event object."** `RollNextState()`/`PeekForecast()` return a `WeatherKind` enum (a
  value type — zero heap allocation) plus, for the forecast path, a `List<WeatherForecastEntry>`
  of small DTOs (used for UI display, called on-demand, not every tick). What *does* allocate on
  every roll (roughly every `weatherCheckIntervalHours`, default 6h — not literally "every day
  tick" as the plan states) is `new SeededRng(unchecked(_seed * 397 + _state.rollCount))` — a
  fresh 24-byte RNG instance per roll. That is real, but it's an RNG-instantiation cost, not an
  "event object" allocation, and pooling a `SeededRng` (an 8-byte mutable struct-like class with
  no cross-roll state to reset) is a much smaller, different fix than "pool `WeatherEvent`
  objects" — there is no `WeatherEvent` class in this codebase. Step 4 as originally written
  targets a type that doesn't exist; corrected below.
- **`SaveChecksum`** (`Assets/Ashfall.Core/SaveChecksum.cs`) — reflection cost is real but
  **the plan describes the wrong reflection API.** `WriteObject()` calls
  `type.GetFields(BindingFlags.Public | BindingFlags.Instance)` — **fields**, not
  `GetProperties()`. Save DTOs in this codebase are "deliberately all plain public fields" (see
  the class's own doc comment). There are no properties to fix boxing for via typed property
  getters as Step 5 proposes; the fix needs to target `FieldInfo.GetValue`/boxing on fields
  instead. This is corrected in Step 5 below — the caching strategy is still valid, but the
  reflection surface is different from what's described.
- **`CatalogIntegrityValidator`** — the 200+-entry `IdPrefixes` array
  (`Assets/Ashfall.Core/CatalogIntegrityValidator.cs:75`) is `static readonly`, allocated once at
  class load, not per check. `StartsWithAny()` iterates it calling `string.StartsWith` per
  prefix — this is CPU cost (string comparison), not an allocation hotspot as claimed. Not
  re-scoped as a pooling target in this batch; if it needs optimization at all, that's an
  algorithmic (e.g., prefix-trie) fix, not something `ObjectPool<T>` addresses. Recommend
  dropping `CatalogIntegrityValidator` from this batch's "Systems Touched" list.
- **Journal/Eulogy/TradeTell** — `StringBuilder`/string-concatenation allocation in text
  generation is plausible and not contradicted by anything read in this review, but was not
  independently confirmed by reading those three files. Treat as likely-true, not verified.

This batch introduces a reusable `ObjectPool<T>` in Core plus targeted pooling — scoped to what
Step 1's actual profiling shows is hot, not the assumed list above. The `ObjectPool<T>` and
`StringBuilderPool` infrastructure (Steps 2 and 6) are sound regardless of which specific systems
turn out to need them, and should proceed; Steps 3, 4, and 5 need re-scoping per the corrections
in their own sections below.

---

## Step 1 — Profile Allocation Hotspots

### Goal
Establish a quantitative baseline of allocations per simulated day so improvements can be measured and regressions caught.

### Feasibility correction (verified against this environment and this codebase)
The original implementation plan (`dotnet-counters` + `dotnet-trace` attached to `godot --headless`, plus a `--simulate-days 100` CLI flag) has two blocking problems, both confirmed directly, not assumed:

1. **`dotnet-counters` and `dotnet-trace` are not installed** (`which dotnet-counters dotnet-trace` → not found; `dotnet tool list -g` → empty) and are not referenced anywhere in `scripts/ci/` or `REPO_REVIEW_REPORT.md`. They are also non-trivial to attach to a Godot-embedded Mono/.NET runtime in headless mode — Godot 4.7's C# host is not a plain `dotnet run` process with a stable diagnostic IPC socket exposed the way a normal console app has; attaching requires extra environment variables (`DOTNET_EnableEventPipe`, etc.) that are unverified for this Godot build. Treat "attach dotnet-trace to godot --headless" as an R&D spike with unknown outcome, not a one-line verification command.
2. **`--simulate-days 100` does not exist.** Checked `src/Host/HostCli.cs`'s `HostCliAction` enum and its ~30 `Has(args, "--...")` dispatch entries directly — there is no day-simulation-loop verb today. Building it is real, scoped work (it needs to construct whatever the equivalent of a "full game session" is, which itself does not exist as a single object — see Step 7 correction below) and must be its own sub-step, not an assumed prerequisite the profiling script can just call.

### Recommended replacement approach
Given (1) and (2), prefer **`GC.GetAllocatedBytesForCurrentThread()` deltas inside an xUnit test** (the same primitive Step 7 already proposes) as the *primary* measurement mechanism, run via plain `dotnet test` — no external profiler, no Godot process, no new tool installation. This is strictly more portable (works in any CI runner) and sidesteps both blocking problems. Reserve `dotnet-trace`/`dotnet-counters` as an optional, manual, locally-run deep-dive for a developer who wants call-stack attribution — not as a scripted, CI-relied-upon step.

### Implementation
- Add a new Core-testable entry point that runs N simulated days against a real (not mocked) system graph — this does not exist yet; building it is part of this step's cost, not free.
- Add `Ashfall.Core.Tests/AllocationBaselineTests.cs` (or fold into Step 7's file) with a `[Fact]` that ticks the day loop 100 times and records `GC.GetAllocatedBytesForCurrentThread()` deltas per system, printed to test output — this becomes the "baseline document" source of truth, generated by running the test once and pasting output, not hand-authored.
- Document baseline in `docs/profiling/allocation_baseline_batch74.md` with per-system breakdown (per-system attribution requires calling `GC.GetAllocatedBytesForCurrentThread()` around each system's tick call individually, not just around the whole day — note this in the test design).
- Optional (manual, non-CI): a locally-run `dotnet-trace collect` session against `dotnet test`, if a developer wants flame-graph-level attribution. Do not script this into `scripts/ci/`.

### Verification
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~AllocationBaseline"
```
No Godot invocation, no external tool installation required for the CI-relied-upon path.

### Done-when
- Baseline document exists with top allocators ranked by bytes/day, generated from actual test
  output (attach the test output block, not an estimate).
- The measurement test runs unattended under plain `dotnet test` and needs no tools beyond the
  existing test SDK.
- No code changes to production systems in this step.

### Rollback
This step only adds a new test file and a markdown doc — no production code path changes.
Revert is a two-file delete with zero gameplay impact.

---

## Step 2 — Implement ObjectPool\<T\> in Core

### Goal
Provide a thread-safe, bounded, zero-allocation-on-rent/return object pool usable by any Core system without engine dependencies.

### Implementation
- Create `Assets/Ashfall.Core/Pooling/ObjectPool.cs`:
  ```csharp
  namespace Ashfall.Core.Pooling;

  public sealed class ObjectPool<T> where T : class, new()
  {
      private readonly T[] _buffer;
      private int _count;
      private readonly int _maxSize;
      private readonly Action<T>? _resetAction;

      public ObjectPool(int maxSize, Action<T>? resetAction = null) { ... }
      public T Rent() { ... }        // Returns pooled or new(); zero-alloc when pool non-empty
      public void Return(T obj) { ... } // Resets and returns to pool; drops if full
      public int CountInPool { get; }
  }
  ```
- Use `Interlocked.CompareExchange` for thread safety (no lock contention).
- Bounded size prevents unbounded memory growth — items beyond max are simply dropped for GC.
- `resetAction` callback zeroes fields on return so stale data never leaks.
- Create `Assets/Ashfall.Core/Pooling/IPoolable.cs` interface (optional — systems can use raw pool or implement `IPoolable.Reset()`).
- Add `Assets/Ashfall.Core/Pooling/StringBuilderPool.cs` — specialized pool for `StringBuilder` instances (clears on return, pre-sized capacity).

### Verification
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~ObjectPool"
```

### Done-when
- `ObjectPool<T>` compiles with zero engine references.
- Unit tests verify: rent returns instance, return+rent reuses same instance, bounded overflow drops gracefully, reset callback fires, thread-safety under parallel rent/return.
- No `UnityEngine.*` or `Godot.*` references anywhere in `Ashfall.Core/Pooling/`.

---

## Step 3 — Pool Narrative Encounter Result Objects (conditional — gate on Step 1 findings)

### Goal
Eliminate per-encounter allocations in `NarrativeEncounterSystem` by pooling result objects
— **if and only if Step 1's baseline shows this is a measurable allocation source.**

### Correctness caveat (read from source, not assumed)
`Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs` has no "encounter result object"
created per encounter *check*. `SelectEncounter(...)` (the frequent, roll-every-check path)
returns a reference to an existing `EncounterDefinition` already living in `_catalog` — zero
allocation. The only object created is `EncounterResolutionRecord`, and only inside `Resolve(...)`,
which fires once per **player choice acknowledgment**, not once per encounter roll. Player
choices are bounded by how often a human clicks through a narrative prompt — almost certainly
far below tick rate, and plausibly not worth pooling at all. Do not implement this step until
Step 1's profiling test reports `EncounterResolutionRecord` (or its equivalent call site) as a
top allocator; if it doesn't show up, skip this step and remove it from the batch rather than
pooling something with no measured benefit (pooling has a real cost: stale-data risk, extra
`Reset()`/`Return()` discipline burden on every call site — not free even when "safe").

### Implementation (only if gated condition is met)
- The result DTO is `EncounterResolutionRecord` (not "e.g. `EncounterResult`" as originally
  written — that type does not exist in this codebase; use the actual name).
- Add a `Reset()` method to `EncounterResolutionRecord` that zeroes all fields.
- Create a static or injected `ObjectPool<EncounterResolutionRecord>` sized to the *actual*
  measured concurrent-record count from Step 1, not a guessed "16" — `Resolve()` already copies
  each record into `_state.history` (a `List<EncounterResolutionRecord>` that is the durable save
  state), so pooling here only helps the transient record created inside `Resolve()` before it's
  copied — check whether `CaptureState()`'s deep-copy pattern (it already allocates a fresh
  `EncounterResolutionRecord` per history entry on every save) is actually the bigger cost before
  committing pool sizing.
- Modify `NarrativeEncounterSystem.Resolve(...)` to `Rent()` the transient record instead of `new`.
- Ensure the pooled instance is returned immediately after its fields are copied into
  `_state.history` — it must not be held or referenced beyond that copy, since `OnEncounterResolved?.Invoke(record)` currently passes the same instance to subscribers, who may retain it. **This is a real behavior-change risk**: any external subscriber holding onto the `record` reference from the event will see it mutated/reset once returned to the pool. Audit all `OnEncounterResolved` subscribers before pooling, or pass a defensive copy to the event instead of the pooled instance.

### Verification
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~NarrativeEncounter"
dotnet build Ashfall.csproj
```

### Done-when
- Step 1's profiling data justifies this step in writing (cite the measured bytes/call) before
  any code is written — otherwise mark this step "skipped, not justified by profiling" in the
  batch closeout.
- `NarrativeEncounterSystem.Resolve()` no longer allocates a new `EncounterResolutionRecord` for
  the transient instance after pool warm-up.
- Existing narrative encounter tests pass unchanged (pool is transparent to consumers).
- `OnEncounterResolved` subscribers are audited and confirmed not to retain the pooled instance
  past the event call, or the event is changed to pass a non-pooled copy.
- No stale data leaks between encounters (verified by test that rents, partially fills, returns,
  rents again, asserts clean state).

### Risk / Rollback
Pooling a record that's also handed to a public C# event (`OnEncounterResolved`) is a behavior
change, not a pure perf optimization — a subscriber that stores the reference will silently see
corrupted data days later when the pool recycles it. This is the highest-risk step in the batch
relative to its likely (unconfirmed) benefit. Rollback: revert to `new EncounterResolutionRecord()`
in `Resolve()`; no save-format or state-shape change is involved, so rollback is a single-method
revert with no data migration.

---

## Step 4 — Reduce Per-Roll `SeededRng` Allocation in WeatherSystem (retargeted)

### Goal
Reduce allocation from `WeatherSystem`'s weather-roll path — **retargeted from the original
"pool weather event objects," since no such object exists in this codebase.**

### Correctness caveat (read from source)
`Assets/Ashfall.Core/World/WeatherSystem.cs` has no `WeatherEvent`, `StormEvent`, or forecast
DTO class that gets created "every day tick." What actually happens:
- `RollNextState()` (called from `Tick(float gameHours)`, at most once per
  `weatherCheckIntervalHours` — default 6h, so up to 4x/day, not once per day) allocates
  `new SeededRng(unchecked(_seed * 397 + _state.rollCount))` — a fresh RNG instance per roll,
  immediately discarded after one `NextDouble()` call.
- `PeekForecast(daysAhead)` does the same per forecast day, but only when a caller asks for a
  forecast (UI-driven, on-demand — not a steady tick-rate cost).
- The result type is `WeatherKind`, a C# `enum` — a value type, zero heap cost regardless of
  pooling.
There is genuinely nothing to "pool" in the object-pool sense here: `SeededRng` is re-seeded
fresh every call *by design*, specifically so weather rolls don't need to persist RNG state
across saves (see the class doc comment: "each roll reseeds fresh from seed + rollCount instead
of persisting RNG state" — this is a deliberate determinism/save-simplicity tradeoff, not an
oversight). Pooling `SeededRng` instances would require resetting `_state` on reuse, which is a
few bytes of work and saves one small object allocation per roll (roughly every 1.5–6 hours of
game time) — likely a marginal win. Confirm with Step 1 profiling before spending time here;
this is far more likely to be noise relative to Journal/Eulogy/TradeTell string allocation
(Step 6) than the original plan assumed.

### Implementation (only if Step 1 profiling shows this matters)
- Add a `Reset(int seed)` (or similar reseed) method to `SeededRng` if the class doesn't already
  support reseeding in place — check `Assets/Ashfall.Core/HostDefaults.cs:96` first; currently
  the seed is set only in the constructor via SplitMix64 spreading, so reseeding in place means
  re-running that spreader, not just assigning `_state`.
- Pool `SeededRng` instances with a small `ObjectPool<SeededRng>`, `Reset` re-seeding on rent.
- **Determinism check is mandatory, not optional**: since `SeededRng` would now be a pooled,
  mutable, reused instance rather than a fresh one per roll, any test that captures a `SeededRng`
  reference across calls (none currently do, per the source read above) would break. Add an
  explicit test proving identical output sequences before/after pooling for the same seed
  sequence — this is Invariant 4 territory; a subtle bug here breaks save-compatibility and
  replay-fidelity work in Batch 73, not just performance.

### Verification
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~Weather"
dotnet build Ashfall.csproj
```

### Done-when
- Step 1's profiling data justifies this step in writing before any code is written; if the
  per-roll `SeededRng` allocation is not measurably significant, skip this step.
- If implemented: weather roll determinism is proven unchanged by a before/after sequence-equality
  test (same seed ⇒ same `WeatherKind` sequence across 1000+ rolls), not just "existing weather
  tests pass unchanged."
- `WeatherSystem` determinism is preserved (same seed = same weather sequence regardless of
  pooling) — this was already stated in the original plan and remains correct as a requirement,
  just now backed by an explicit test rather than an assertion.

### Risk / Rollback
Low risk if scoped to `SeededRng` reseeding only (no gameplay-visible type changes). Rollback is
reverting `RollNextState`/`PeekForecast` to construct `new SeededRng(...)` directly — a
same-file, same-method revert with no save-format impact (weather state's `rollCount` field is
unaffected either way).

---

## Step 5 — Reduce SaveChecksum Reflection Allocations

### Goal
Eliminate repeated reflection allocations in `SaveChecksum` by caching metadata and avoiding value-type boxing.

### Correctness caveat (read from source — reflection target was wrong)
`Assets/Ashfall.Core/SaveChecksum.cs` reflects over **public instance *fields*** via
`type.GetFields(BindingFlags.Public | BindingFlags.Instance)` (`WriteObject`, line ~154) — not
`GetProperties()`. The class's own doc comment states this explicitly: "Scope: public instance
fields only, matching the save DTOs, which are deliberately all plain public fields." There are
no properties in these DTOs to write typed getters for. The optimization is still valid in spirit
(cache reflection metadata, avoid boxing) but must target `FieldInfo`, not `PropertyInfo`.
`CatalogIntegrityValidator` (mentioned in the original Step 5 header as "603-line context") is
unrelated to `SaveChecksum` — it doesn't call `SaveChecksum` and isn't part of this reflection
path; drop that cross-reference, it's confusing and wrong.

### Implementation
- `SaveChecksum` currently:
  - Calls `type.GetFields(...)` on **every** `WriteObject` call for every nested object in the
    graph (allocates a new `FieldInfo[]` each time, for every save, for every nested DTO).
  - Also calls `Array.Sort(fields, CompareByName)` fresh every time, on top of the array
    allocation.
  - Boxes value types (`int`, `float`, `bool`, enums) when reading via `FieldInfo.GetValue(...)`.
  - Allocates a `StringBuilder` per `WriteSequence` call for the inner sequence buffer (see
    `WriteSequence`, line ~140) in addition to any boxing costs.
- Cache `FieldInfo[]` (already sorted) per type in a `ConcurrentDictionary<Type, FieldInfo[]>` —
  sort once per type, not once per call.
- For known value types (`int`, `float`, `double`, `bool`), typed field access via compiled
  expressions (`Expression.Lambda<Func<object, T>>`) or `System.Reflection.Emit`-based getters
  can avoid boxing — but note this is meaningfully more implementation complexity than "use a
  typed getter instead of `GetValue`" implies; budget for it. A simpler, lower-risk first pass:
  cache the `FieldInfo[]` (removing the sort-and-allocate-array cost, which is the dominant cost
  for small DTOs) and defer the boxing-elimination work as a follow-up if profiling still shows
  it matters after the caching fix alone.
- Pre-allocate a reusable buffer for float formatting, or use `TryFormat` with a stack-allocated
  `Span<char>` instead of `float.ToString("G9", ...)` (which allocates a string per call).
- The `StringBuilder items = new StringBuilder()` inside `WriteSequence` (one per sequence, at
  every depth, for every save) is also a real allocation the original plan didn't mention —
  consider passing the parent `StringBuilder` through instead of building a separate buffer per
  sequence and concatenating; this changes the string-building structure of `WriteValue` and
  needs its own before/after golden-hash test (see Verification) since it touches every code path
  that produces the canonical string.

### Verification
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~SaveChecksum"
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~SaveStore"
dotnet build Ashfall.csproj
```

### Done-when
- `SaveChecksum.Compute(...)` allocates zero new `FieldInfo[]` arrays after first call per type
  (corrected from "PropertyInfo[]").
- Value-type boxing eliminated for `int`, `float`, `double`, `bool` **fields** (corrected from
  "properties").
- All `SaveStoreChecksumSweepTests` (12 tests, per AGENTS.md's Known Issues section) and
  `SaveWireContractTests` (7 tests) pass unchanged.
- Checksum values are bit-identical before and after optimization (verified by a golden-value
  test: compute checksums for a representative set of real save states *before* touching this
  file, commit the expected hex strings into the test, then assert the optimized code produces
  the same strings — do this as the first commit in this step, before any optimization, so the
  golden values are captured pre-change).

### Risk / Rollback
This function is the integrity backbone for every save store in the project (`SaveChecksum` is
used by all 5+ save stores named in AGENTS.md's Known Issues, including the newly-checksummed
`ExpeditionSaveStore`/`MedicalSaveStore`/`NarrativeSaveStore`/`WorldSaveStore`/`JournalSaveStore`).
A subtle change to canonicalization (e.g., altering how sequences or nested objects are
delimited) silently changes every future checksum without necessarily breaking any existing test
if the golden-value test isn't captured first. This is the highest-blast-radius step in the
batch — a bug here doesn't crash, it silently produces a save file that a slightly-different
build then rejects as "corrupt" (per the stricter load guard AGENTS.md describes for the
five checksummed stores). Mitigation: capture golden hashes *before* changing anything (see
Done-when), and keep this step in its own commit/PR separate from Steps 2–4 and 6, so a
regression is trivially bisectable and revertible without touching unrelated pooling work.

---

## Step 6 — Add StringBuilder Pooling for Text Generation

### Goal
Pool `StringBuilder` instances used by Journal, Eulogy, TradeTell, and other text-generation systems to eliminate repeated allocation of large character buffers.

### Implementation
- Create `StringBuilderPool` in `Assets/Ashfall.Core/Pooling/StringBuilderPool.cs`:
  ```csharp
  public static class StringBuilderPool
  {
      private static readonly ObjectPool<StringBuilder> _pool =
          new(maxSize: 8, resetAction: sb => { sb.Clear(); sb.Capacity = Math.Min(sb.Capacity, 4096); });

      public static StringBuilder Rent() => _pool.Rent();
      public static void Return(StringBuilder sb) => _pool.Return(sb);
      public static string ToStringAndReturn(StringBuilder sb)
      {
          var result = sb.ToString();
          Return(sb);
          return result;
      }
  }
  ```
- Cap returned `StringBuilder` capacity at 4096 to prevent a single large generation from permanently bloating pool memory.
- Replace `new StringBuilder()` calls in:
  - `JournalSystem` — entry text assembly
  - `EulogySystem` — death summary generation
  - `TradeTellSystem` — trade narrative generation
  - `NarrativeEncounterSystem` — encounter text assembly (if separate from Step 3)
  - `RadioBroadcastSystem` — broadcast text composition
- Use `StringBuilderPool.ToStringAndReturn(sb)` as a convenience that extracts the string and returns the builder in one call.

### Verification
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

### Done-when
- All text-generation systems use `StringBuilderPool` instead of `new StringBuilder()`.
- No `new StringBuilder()` calls remain in hot paths (verified by code search).
- All existing tests pass (text output is identical — pooling is transparent).
- Pool capacity cap prevents unbounded growth.

---

## Step 7 — Add Allocation Regression Test

### Goal
Prevent future regressions by establishing a hard allocation budget per simulated day, enforced in CI.

### Correctness caveat (fabricated API — verified against source)
The original implementation used `TestSessionFactory.CreateFullSession(seed: 42)` and
`session.TickSimDay()` / `session.SaveAll()`. **None of these exist anywhere in this codebase.**
Confirmed by searching the full repo for `TestSessionFactory`, `CreateFullSession`, and
`GameBootstrap` (the nearest architectural analog some AGENTS.md sections reference) — zero
matches for the first two; `GameBootstrap` is described in `docs/` as dead/unlinked code (a
prior audit found "no `new GameBootstrap(...)`" anywhere in `src/`). The real equivalents,
`TickSimDay(int day)` and `SaveAll()`, are **private instance methods on the Godot host's `Main`
partial class** (`src/Main.cs:1643` and `src/Main.cs:6227`), not on any Core-testable session
object, and they operate on ~24 systems constructed and owned directly by `Main`'s many
`SetupXxx` methods (see AGENTS.md H7). There is no single object `Ashfall.Core.Tests` can new up
and call `.TickSimDay()`/`.SaveAll()` on today.

This means Step 7 as written is **not implementable without first building the harness it
assumes already exists** — either:
- (a) build a minimal `Ashfall.Core.Tests`-only test session that wires up a representative
  subset of systems (not all 24 — a full `Main.cs`-equivalent harness in test code is itself a
  significant undertaking and arguably out of scope for this batch), or
- (b) move this test to a Godot-side integration test that can reach the real `Main` instance
  (no such Godot-side C# test project currently exists in this repo, per the STACK table in
  AGENTS.md — Tests are xUnit under `Ashfall.Core.Tests` only, targeting Core, not the host).

Recommend (a), scoped down: measure allocation for 3–4 concrete, already-testable Core systems
individually (e.g., `NarrativeEncounterSystem`, `WeatherSystem`, `SaveChecksum` — the systems
this batch actually touches) rather than a full simulated "day" across a session that doesn't
exist as a test-callable unit. This is smaller, achievable, and directly measures the systems
Steps 3–5 modify.

### Implementation (rewritten to use only APIs that exist)
```csharp
[Fact]
public void WeatherSystem_RollNextState_AllocationBudget()
{
    var weather = new WeatherSystem();
    weather.BindProfile(SomeTestProfile(), seed: 42);

    // Warm up (JIT, etc.)
    for (int i = 0; i < 10; i++) weather.Tick(6f);

    long before = GC.GetAllocatedBytesForCurrentThread();
    for (int i = 0; i < 100; i++) weather.Tick(6f);
    long after = GC.GetAllocatedBytesForCurrentThread();

    long bytesPerRoll = (after - before) / 100;
    Assert.True(bytesPerRoll < /* budget from Step 1 baseline */ 500,
        $"Allocation regression: {bytesPerRoll} bytes/roll exceeds budget");
}

[Fact]
public void SaveChecksum_Compute_AllocationBudget()
{
    var state = BuildRepresentativeSaveState(); // reuse an existing test fixture/DTO
    SaveChecksum.Compute(state); // warm up (first-call type-metadata cache population)

    long before = GC.GetAllocatedBytesForCurrentThread();
    for (int i = 0; i < 100; i++) SaveChecksum.Compute(state);
    long after = GC.GetAllocatedBytesForCurrentThread();

    long bytesPerCall = (after - before) / 100;
    Assert.True(bytesPerCall < /* budget from Step 1 baseline */ 5_000,
        $"SaveChecksum allocation regression: {bytesPerCall} bytes/call exceeds budget");
}
```
Both examples operate on real, already-instantiable Core classes (`WeatherSystem`,
`SaveChecksum`) rather than a fictional session/bootstrap object. Budget numbers above are
placeholders — Step 1's actual baseline measurement must set them, per that step's corrected
design (not guessed as "50KB/day" and "200KB" the way the original draft did with no
measurement basis).

### Verification
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~AllocationRegression"
```

### Done-when
- Allocation regression tests target real, existing Core types (no `TestSessionFactory`/
  `GameBootstrap`/`session.TickSimDay()` — those don't exist).
- Budget thresholds are set from Step 1's measured baseline, not guessed, and documented inline
  with the measurement date and commit so future readers know when to re-baseline.
- Tests run in CI alongside the existing test suite (verify the actual current test count before
  citing it — the original plan's "1941+ tests" figure was not independently verified in this
  review; confirm via `dotnet test ... --list-tests | wc -l` or equivalent before publishing that
  number in a "Done when" or "Exit Criteria" bullet).
- Future PRs that regress allocation on the covered systems will fail CI automatically. This is
  narrower coverage than "per simulated day" implied — document that gap explicitly rather than
  implying full-session coverage that doesn't exist.

### Risk / Rollback
Low risk — additive test file only, no production code touched. If budgets prove too tight and
flake in CI (a real risk per the original plan's own risk table), loosen the threshold in the
same PR that reports the flake; no rollback of production code is ever required for this step.

---

## Summary Table

| Step | Description | Key Deliverable | Risk | Dependencies |
|------|-------------|-----------------|------|--------------|
| 1 | Profile allocation hotspots (via `GC.GetAllocatedBytesForCurrentThread()` in xUnit, not `dotnet-trace`/`dotnet-counters` — see correction) | Baseline test + `docs/profiling/allocation_baseline_batch74.md` | Low | None |
| 2 | Implement ObjectPool\<T\> in Core | `Ashfall.Core/Pooling/ObjectPool.cs` + tests | Low | None |
| 3 | Pool `EncounterResolutionRecord` — **conditional on Step 1 confirming it's hot** (see correction: original target type/frequency was wrong) | Reduced-alloc `Resolve()`, or explicitly skipped | Medium (event-reference retention risk, not "Low" as originally rated) | Steps 1, 2 |
| 4 | Reduce per-roll `SeededRng` allocation in WeatherSystem — **retargeted from "pool weather event objects," which don't exist** (see correction) | Pooled/reseedable `SeededRng`, or explicitly skipped | Medium (touches determinism-critical RNG path — higher than "Low" as originally rated) | Steps 1, 2 |
| 5 | Reduce SaveChecksum reflection allocs (targets `FieldInfo`, not `PropertyInfo` — see correction) | Cached metadata, no boxing, golden-hash test | **High** (integrity backbone for all save stores — raised from "Low-Med") | None |
| 6 | StringBuilder pooling for text gen | `StringBuilderPool` + integration | Low | Step 2 |
| 7 | Allocation regression test (targets real Core types, not a fictional session/bootstrap object — see correction) | Per-system allocation budget tests | Low | Steps 1, 2, and whichever of 3-6 actually ship |

Risk ratings raised for Steps 3, 4, and 5 relative to the original table — each touches either a
public event contract, the determinism-critical RNG path, or the save-integrity backbone, none of
which are "Low" risk regardless of how mechanically simple the pooling pattern itself is.

---

## Risks & Mitigations

| Risk | Likelihood | Impact | Mitigation |
|------|-----------|--------|------------|
| Pool size too small causes fallback to `new()` | Medium | Low (still correct, just slower) | Start generous (16-32), tune with profiling |
| Stale data leaks from improperly reset objects | Low | High (gameplay bugs) | Mandatory `Reset()` + test that verifies clean state after return |
| Allocation budget too tight, flakes in CI | Medium | Low (test noise) | Set budgets at 2x observed baseline, document update process |
| Thread-safety overhead slows single-threaded hot path | Low | Low | Use `Interlocked` (1 CAS), not `lock`; benchmark shows <1ns overhead |
| `GC.GetAllocatedBytesForCurrentThread()` not available on all runtimes | Low | Low | .NET 8 supports it; add `[SkippableFact]` guard if needed |
| **(added)** Pooling `EncounterResolutionRecord` corrupts data for any `OnEncounterResolved` subscriber that retains the event-passed reference | Unverified (no subscriber audit done in this review) | High (silent gameplay/narrative data corruption, hard to detect) | Audit all subscribers before Step 3; pass a defensive copy to the event instead of the pooled instance if any retain it |
| **(added)** Step 3/4 implemented on speculative allocation targets that Step 1 profiling doesn't actually confirm | Medium (plausible given this review found both original targets mischaracterized) | Medium (wasted effort, added complexity with no measured benefit) | Hard-gate Steps 3 and 4 on Step 1's measured output; explicitly skip either step if not justified |
| **(added)** `SaveChecksum` canonicalization change silently alters checksum output for existing saves without a pre-change golden-value baseline | Low likelihood if golden test is captured first, otherwise High | High (saves silently rejected as "corrupt" by future builds, per the stricter load guard in AGENTS.md) | Capture golden checksums for representative states in the same commit that starts Step 5, before any optimization code is written; keep Step 5 in its own PR separate from Steps 2-4/6 |
| **(added)** `dotnet-trace`/`dotnet-counters` unavailable in this environment, and attaching either to `godot --headless` is unverified | Confirmed — both tools absent locally, not referenced in any CI script | Low if Step 1's corrected `GC.GetAllocatedBytesForCurrentThread()` approach is used instead; High if the original profiler-based plan is followed as-is (blocks Step 1 entirely) | Use the in-test measurement approach as primary; treat trace/counters as optional manual follow-up only |

---

## Exit Criteria

This batch is COMPLETE when:
1. `ObjectPool<T>` exists in Core, fully tested, zero engine references.
2. For each of narrative-encounter pooling, weather-RNG pooling, and text-generation pooling:
   either implemented per the corrected Steps 3/4/6 above, or explicitly marked skipped with the
   Step 1 profiling evidence that justified skipping it. ("Weather events use pooling" is not a
   valid completion statement since no such object exists — see Step 4 correction.)
3. `SaveChecksum` field reflection (not property reflection) is cached and boxing-free, with a
   golden-hash test proving output is unchanged.
4. Allocation regression tests (Step 7, scoped to real Core types — see correction) pass in CI
   with documented, measurement-derived budgets.
5. All existing tests still pass — **verify and state the actual current test count** (run
   `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` and report the real number; do not
   carry forward "1941+" from the original plan without confirming it against this run).
6. Godot host builds cleanly (`dotnet build Ashfall.csproj` — 0 errors, 0 warnings).
7. Post-optimization measurement (via the Step 1 test-based approach) shows a measurable
   reduction vs. the Step 1 baseline for whichever systems were actually changed.


---

## Review Notes (Corrected)

Adversarial review performed against the actual repository at
`/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War` (source read directly for every
named class; `dotnet test --list-tests` run to check the cited test count; `which`/`dotnet tool
list -g` run to check profiler tooling availability). Corrections applied in place above; this
section summarizes what changed and why.

### Factual corrections — allocation targets were largely wrong
1. **`NarrativeEncounterSystem` does not create "encounter result objects every encounter."**
   Read `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs` directly: the frequent path
   (`SelectEncounter`) allocates nothing — it walks the existing `_catalog` list. The only
   allocation, `EncounterResolutionRecord`, happens in `Resolve()`, once per player choice
   acknowledgment, not once per encounter roll. Original Step 3 targeted the wrong call
   frequency and even referenced a nonexistent type name (`EncounterResult`). Corrected to gate
   this step on Step 1's actual measurement and to flag a real correctness risk the original
   missed entirely: `Resolve()` passes the same object to the `OnEncounterResolved` event that
   would be pooled — any subscriber retaining that reference would see it silently mutated after
   return-to-pool.
2. **`WeatherSystem` has no "weather event object" pooled per day tick.** Read
   `Assets/Ashfall.Core/World/WeatherSystem.cs` directly: `WeatherKind` is an enum (value type,
   zero heap cost); there is no `WeatherEvent`/`StormEvent` class anywhere in this codebase. The
   real, smaller allocation is `new SeededRng(...)` constructed fresh on every weather roll
   (roughly every 6 in-game hours by default, not "every day tick"), which the class's own doc
   comment says is deliberate — rolls reseed fresh instead of persisting RNG state, specifically
   to keep save/load simple. Retargeted Step 4 to the real allocation (RNG instantiation) and
   flagged that this touches the determinism-critical RNG path (Invariant 4, and directly
   relevant to Batch 73's replay-fidelity work), which is a materially different risk profile
   than "pool a UI event DTO."
3. **`SaveChecksum` reflects over fields, not properties.** Read
   `Assets/Ashfall.Core/SaveChecksum.cs` directly: `WriteObject` calls
   `type.GetFields(BindingFlags.Public | BindingFlags.Instance)`. The class's own doc comment
   states the DTOs are "deliberately all plain public fields." The original Step 5's code
   sketch (`prop.GetValue(obj)`, `ConcurrentDictionary<Type, PropertyInfo[]>`) targets an API
   surface this class does not use. Corrected throughout Step 5 to target `FieldInfo`. Also
   found and flagged an allocation the original plan missed: `WriteSequence` allocates a new
   `StringBuilder` per sequence at every recursion depth, on every canonicalize call — likely a
   bigger cost than the boxing the plan focused on.
4. **`CatalogIntegrityValidator`'s `IdPrefixes` array is `static readonly`, allocated once at
   class load — not "allocates comparison strings for 200+ prefix checks" per invocation.**
   Read `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` directly: `StartsWithAny` iterates a
   pre-existing array calling `string.StartsWith`; this is a CPU cost, not an allocation
   hotspot. Removed from the batch's scope entirely (see "Systems Touched" and header table
   corrections) rather than leaving a target that `ObjectPool<T>` cannot meaningfully address.

### Unrunnable verification tooling
5. **`dotnet-counters` and `dotnet-trace` are not installed in this environment** (confirmed via
   `which` and `dotnet tool list -g` — both empty) **and are not referenced anywhere in this
   repo's `scripts/ci/`.** Attaching either to a Godot-headless-hosted .NET runtime is also
   unverified — Godot 4.7's embedded host is not a plain `dotnet run` process with guaranteed
   diagnostic-IPC exposure. Step 1 as originally written is not a "verification command," it's
   an unscoped R&D spike with two unverified dependencies. Replaced with an in-test
   `GC.GetAllocatedBytesForCurrentThread()` approach that runs under plain `dotnet test` — no new
   tools, no Godot process, portable to any CI runner.
6. **`--simulate-days 100` does not exist as a CLI verb.** Checked `src/Host/HostCli.cs`'s
   `HostCliAction` enum and its ~30-entry `Has(args, "--...")` dispatch chain directly — no
   day-simulation verb exists today. Building one is real scoped work the original plan treated
   as a given.
7. **`TestSessionFactory.CreateFullSession(seed: 42)` and `session.TickSimDay()`/
   `session.SaveAll()` (Step 7) do not exist anywhere in this codebase.** Searched the full repo
   for `TestSessionFactory`, `CreateFullSession`, and `GameBootstrap` — zero matches for the
   first two, and `GameBootstrap` is documented elsewhere in this repo's own audit trail
   (`docs/ASHFALL_DEEP_CODE_AUDIT_2_2026-08-14.md`) as dead, unlinked code with no live
   instantiation anywhere in `src/`. The real `TickSimDay`/`SaveAll` are private methods on the
   Godot host's `Main` partial class (`src/Main.cs:1643`, `src/Main.cs:6227`), not on any
   Core-testable object, and `Ashfall.Core.Tests` has no path to construct a `Main` instance
   (it's a Godot `Node`, xUnit runs outside Godot). Step 7 was rewritten to measure real,
   already-instantiable Core types (`WeatherSystem`, `SaveChecksum`) instead of a fictional
   session object, and to explicitly note the resulting coverage is narrower than "per simulated
   day across the full game" — a real scope reduction, called out rather than hidden.

### Numerical claim corrected
8. **"1941+ existing tests" was unverified in the original plan.** Ran
   `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --list-tests` directly: the actual
   current count is **2134** tests. Corrected the Exit Criteria to require re-verifying this
   number at batch-close time rather than repeating a specific stale figure, since the count
   will keep changing as other batches land.

### Scope creep flagged, not silently absorbed
9. The original "Prerequisite Batches: None" was true for Steps 1/2/5/6 but false in spirit for
   Steps 3/4 once corrected: both are now explicitly gated on Step 1's profiling output, and
   should be skipped (not force-implemented) if the measured benefit doesn't materialize. This
   prevents the batch from doing speculative, risk-bearing pooling work (see the event-reference
   and RNG-determinism risks above) with no measured justification.

### Risk ratings corrected
10. Steps 3, 4, and 5 were all originally rated "Low" or "Low-Med" risk based on the mechanical
    simplicity of "add a pool." Re-rated based on what each step actually touches: Step 3 touches
    a public C# event contract with unaudited subscribers (Medium), Step 4 touches the
    determinism-critical RNG path that Batch 73's replay system also depends on (Medium), and
    Step 5 touches the save-integrity backbone used by every save store in the project (raised
    to High). Added a golden-hash-first requirement to Step 5 specifically because a silent
    canonicalization change there would corrupt save compatibility without failing any test that
    doesn't already pin exact hash output.

### What was already sound and required no fix
- `ObjectPool<T>`'s design (Step 2: bounded size, `Interlocked`-based thread safety, optional
  reset callback) is internally consistent and matches Invariant 1 (no engine references) as
  written — no correction needed there.
- `StringBuilderPool` (Step 6) is a reasonable, low-risk design and the target systems
  (Journal/Eulogy/TradeTell text generation) were not contradicted by anything read in this
  review, though they were also not independently re-verified line-by-line the way
  NarrativeEncounterSystem/WeatherSystem/SaveChecksum were — flagged as "plausible, not
  confirmed" in the Motivation section above.
