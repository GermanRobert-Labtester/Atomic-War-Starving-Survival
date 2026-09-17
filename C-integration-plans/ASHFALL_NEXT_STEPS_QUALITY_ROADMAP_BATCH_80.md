# ASHFALL — Quality Roadmap Batch 80

## Theme: Thread Safety Audit — Concurrent Access Protection for Async Operations

**Priority:** LOW-MEDIUM (downgraded from MEDIUM-HIGH — see Corrected Framing note below)
**Risk:** Medium — adding synchronization where none exists
**Batch:** 80
**Systems affected:** All 30 save stores (corrected from 22 — see Batch 79's corrected count,
which applies equally here since it is the same physical set of stores), IEventBus/SimpleEventBus,
CatalogIntegrityValidator, Main.cs flush cycle, every system with public mutable state
**Depends on:** None (foundational audit)
**Unlocks:** Background save optimization, async catalog hot-reload, parallel tick subsystems —
**none of which are currently planned or scheduled anywhere in this roadmap** (see note below)

---

**⚠️ Corrected framing — this batch is speculative/preparatory work, not urgent, and its
priority label is overstated.** Direct verification of `src/` and `Assets/Ashfall.Core/` found:
- **Zero** uses of `Task.Run`, `Thread`, `ThreadPool`, `Parallel.*`, `lock`, `Monitor`,
  `ConcurrentDictionary`, `ConcurrentBag`, `SemaphoreSlim`, `Mutex`, or `volatile` anywhere in
  gameplay/simulation code.
- Exactly one `Interlocked.Increment` call
  (`Assets/Ashfall.Core/Inventory/ProceduralItemInstance.cs:53`), which guards a static counter
  used for deterministic ID generation — not evidence of concurrent access; nothing in the
  codebase calls it from more than one thread today.
- Exactly two `async`/`await` usages, both in `src/Main.cs` (`QuitUiTestAfterFrame`, awaiting
  `ToSignal(SceneTree.SignalName.ProcessFrame)`), both confined to a headless UI-smoke-test
  frame-wait helper that explicitly does not run during normal gameplay (per its own comment).
- No `_PhysicsProcess` overrides exist anywhere (only `_Process`), and every `_Process` override
  runs on Godot's standard single main thread with no worker-thread or `WorkerThreadPool` usage.
- No async file I/O, no background save, no async catalog loading exists or is stubbed anywhere.
- No other roadmap batch, doc, or plan in this repository references planned async save,
  background save, parallel tick subsystems, or async catalog hot-reload as scheduled work.

**Conclusion: the game is genuinely, unambiguously single-threaded today, and this batch is
solving a problem that does not yet exist.** That does not make it worthless — establishing a
threading contract *before* introducing concurrency is a legitimate and common practice, and the
`SimpleEventBus` thread-unsafety this document identifies (plain `Dictionary`/`List`, no locks)
is real and would need fixing the day any concurrency is introduced. But this is preparatory
groundwork for hypothetical future work, not an active bug fix or a response to a scheduled
async feature. It should be framed and prioritized accordingly: valuable to have on record before
undertaking any concurrency work (such as the async auto-save casually suggested in Batch 79
Step 4 — see that document's note flagging the exact same collision), but not urgent on its own,
and should not consume roadmap slots ahead of batches that fix real, currently-manifesting
defects. Recommend re-labeling this **"MEDIUM-HIGH" → "LOW-MEDIUM," and re-titling the theme
from "Thread Safety Audit" to something that signals its speculative nature, e.g. "Threading
Contract — Preparatory Groundwork for Future Async Work,"** so nobody mistakes this for an
active-bug-fix batch when triaging the roadmap.

---

## Context

The Godot host currently operates single-threaded: `_Process` at 4 Hz flushes dirty flags (writes to disk), `TickSimDay` advances the simulation on the main thread, and `SaveAll` writes 30 JSON files sequentially (corrected from 22 — see Batch 79's corrected save-store count of 30, which applies identically here since it is the same physical set of stores). This is safe *today*, and will remain safe for as long as no async/concurrent work is introduced — which, as of this review, is not scheduled anywhere in the project:

- Godot's file I/O *could* be made async for UX smoothness (no frame hitches on save) — this is a hypothetical future improvement, not a planned one.
- A future background save optimization *would* introduce races between `CaptureState()` and ongoing `Tick()` mutations — again hypothetical.
- `CatalogIntegrityValidator` at startup *could* overlap with early game actions if loading is deferred — deferred loading is not currently implemented.
- `IEventBus` / `SimpleEventBus` has no thread-safety guarantees — a cross-thread publish during save could tear subscriber lists **if such a cross-thread call ever occurred, which it does not today** (confirmed: `SimpleEventBus.Publish`/`Subscribe`/`Unsubscribe` are only ever called from the single main thread in the current codebase).
- Multiple systems expose public mutable fields (`List<T>`, `Dictionary<K,V>`) that are read during `CaptureState` and written during `Tick` — true today, but harmless in a single-threaded process; it only becomes a race once something calls `CaptureState()` from a different thread than `Tick()`, which nothing does yet.

If any save operation becomes async in the future, systems that read/write shared state during save could corrupt data silently. This batch establishes the threading contract *before* async work begins, making future optimizations safe by construction — but "before async work begins" should be read literally: there is no async work currently begun, scheduled, or approved.

---

## Step 1 — Audit All Shared Mutable State

**Goal:** Produce a complete inventory of public/internal mutable fields and collections across all Core systems that are both read during `CaptureState()` and written during `Tick()` or event handlers.

**Implementation:**
- Grep all `public` and `internal` fields in `Assets/Ashfall.Core/` that are `List<>`, `Dictionary<>`, `HashSet<>`, arrays, or non-readonly value types.
- Cross-reference with `CaptureState()` bodies — any field read there is a save-visible field.
- Cross-reference with `Tick()`, `RestoreState()`, and event handler bodies — any field written there is a mutation site.
- Produce a `THREAD_SAFETY_AUDIT.md` report: system name, field, read-sites, write-sites, current protection (none/lock/immutable copy).
- Flag fields that are both save-visible AND tick-mutable as "race candidates" — with the
  explicit caveat (stated in this batch's Corrected Framing note above) that a "race candidate"
  today is a *latent* pattern, not an active race: nothing in the codebase currently calls
  `CaptureState()` from any thread other than the main thread, so none of these fields are being
  raced in the shipped game. Label the report accordingly so readers don't mistake an inventory
  of theoretical exposure for a list of active bugs.
- Count total race candidates. The original estimate of "40–80 based on 82+ systems" is an
  unverified guess presented as a number — it was not derived from an actual grep count and
  should not be treated as a completion target. Do not gate this step's completion on hitting a
  specific number; gate it on coverage (every registered system checked), not a guessed count.

**Verification:**
- `THREAD_SAFETY_AUDIT.md` exists and lists every race candidate found, with file:line for both
  the save-visible read site and the tick-mutable write site for each one (not just a field name
  — a reviewer must be able to jump to the exact lines without re-deriving them).
- No Core system is omitted from the audit (cross-check against `GameBootstrap` registration
  list — note this list spans 82 partial files per `AGENTS.md`'s H7 entry; budget audit time
  accordingly rather than assuming a quick pass).
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` still compiles (audit is read-only).

**Done when:** Audit document is complete, peer-reviewed by someone other than the author (per
this project's Cross-Tool QA Rule for systems introducing ≥2 new coupled variables — arguably
applicable here since the audit itself introduces the `[SaveVisible]`-candidate concept used by
later steps), and every system registered in `GameBootstrap` has been checked and is listed with
either its race candidates or an explicit "none found" entry. "≥90% of mutable shared state" is
not verifiable without a denominator; replace it with "100% of registered systems checked" as
the actual measurable bar.

---

## Step 2 — Identify Potential Race Conditions

**Goal:** Classify each race candidate from Step 1 into severity tiers and document the specific interleaving that would cause corruption.

**Implementation:**
- For each race candidate, document the concrete race scenario **and explicitly note that this
  scenario does not occur in the shipped game today** — every scenario below describes what
  *would* happen if a future change introduced concurrent calls, not something observed or
  reproducible in the current build (see Corrected Framing note above):
  - **Save-during-tick:** `CaptureState()` reads a list while `Tick()` adds/removes elements → torn snapshot (partial state captured).
  - **Catalog-load-during-gameplay:** `CatalogIntegrityValidator` or hot-reload reads definition lists while a system modifies runtime caches derived from catalog data. Note: hot-reload does not currently exist in this codebase — this scenario is doubly hypothetical (requires both a future hot-reload feature AND concurrent access).
  - **Event-bus cross-thread publish:** A background thread publishes an event while the main thread iterates subscribers → `InvalidOperationException` or missed delivery. Confirmed today: no background thread publishes events; this would require a future feature to introduce the second thread first.
  - **Flush-during-tick:** `FlushXxxIfDirty()` serializes state while `Tick()` mutates the same state → corrupt JSON on disk.
- Assign severity: **Critical** (data loss/corrupt save), **High** (gameplay inconsistency), **Medium** (cosmetic/recoverable), **Low** (theoretical only with current architecture). Given the Corrected Framing above, essentially every item in this catalog is "Low" under this scheme's own definition, since none of them are reachable without a future architectural change. Consider adding a distinct "N/A — no reachable trigger today" tier rather than forcing hypothetical-only risks into the same four-tier scale used for reachable risks, so the catalog doesn't visually overstate current exposure.
- Document mitigations already in place (e.g., Unity's `EventBus.IsSuppressed` pattern, sequential flush order).
- Identify the 5–10 highest-risk race conditions that would manifest first if `SaveAll` became async — reiterate here that "became async" is a hypothetical precondition for all of them, not a scheduled change.

**Verification:**
- Each race candidate has a documented scenario with specific code paths.
- Severity tiers are justified with concrete failure modes, and each entry states its trigger
  precondition (e.g., "requires SaveAll to become async" or "requires a background thread to be
  introduced") rather than implying the race is reachable today.
- Top-10 risk list is prioritized by likelihood × impact.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` passes (analysis only, no code changes).

**Done when:** Race condition catalog is complete, top-10 risks are identified with explicit trigger preconditions, and the team agrees on priority order for mitigation — understanding that "priority" here means "priority if/when concurrency is introduced," not "priority to fix now."

---

## Step 3 — Add Threading Documentation Attributes

**Goal:** Introduce `[ThreadSafe]`, `[MainThreadOnly]`, and `[SaveLockRequired]` attributes to document the threading contract of every public API surface in Core.

**Implementation:**
- Create `Assets/Ashfall.Core/Threading/ThreadingAttributes.cs`:
  ```csharp
  namespace Ashfall.Core.Threading;

  /// <summary>Method/class is safe to call from any thread.</summary>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
  public sealed class ThreadSafeAttribute : Attribute { }

  /// <summary>Must only be called from the main game thread (Godot _Process / Tick).</summary>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
  public sealed class MainThreadOnlyAttribute : Attribute { }

  /// <summary>Caller must hold the save lock before invoking.</summary>
  [AttributeUsage(AttributeTargets.Method)]
  public sealed class SaveLockRequiredAttribute : Attribute { }

  /// <summary>Field is accessed during CaptureState — protect under save lock.</summary>
  [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
  public sealed class SaveVisibleAttribute : Attribute { }
  ```
- Annotate all `CaptureState()` and `RestoreState()` methods with `[SaveLockRequired]`.
- Annotate all `Tick()` methods with `[MainThreadOnly]`.
- Annotate thread-safe utilities (e.g., `SaveChecksum`, `CoreSeededRng` if stateless calls exist) with `[ThreadSafe]`.
- Annotate race-candidate fields (from Step 1) with `[SaveVisible]`.
- Add an analyzer test that verifies: no `[ThreadSafe]` method calls a `[MainThreadOnly]` method directly.

**Verification:**
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` compiles cleanly.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` passes.
- Attribute coverage: ≥80% of `CaptureState`/`RestoreState` methods are annotated. This
  percentage is unverifiable as written — the plan does not state the actual denominator (how
  many `CaptureState`/`RestoreState` method pairs exist across Core). Before treating 80% as a
  target, run `grep -rc "public.*CaptureState()\|public.*RestoreState(" Assets/Ashfall.Core/`
  (or equivalent) to get the real count, then express the bar as an absolute number, not a
  percentage of an unknown total.
- Analyzer test catches at least one intentional violation in a test fixture.

**Done when:** Threading attributes are defined, applied to all save/tick methods, and an analyzer test enforces the contract. Replace the "≥80%" bar with a concrete count once the denominator is known (see above).

---

## Step 4 — Implement SaveLock Mechanism

**Goal:** Create a cooperative lock that prevents tick advancement during save and prevents save initiation during tick, eliminating the primary race window.

**Implementation:**
- Create `Assets/Ashfall.Core/Threading/SaveLock.cs`:
  ```csharp
  namespace Ashfall.Core.Threading;

  public sealed class SaveLock
  {
      private volatile int _state; // 0=idle, 1=ticking, 2=saving

      public bool TryEnterTick();      // returns false if saving
      public void ExitTick();
      public bool TryEnterSave();      // returns false if ticking
      public void ExitSave();
      public bool IsSaving { get; }
      public bool IsTicking { get; }
  }
  ```
- Use `Interlocked.CompareExchange` for lock-free state transitions (no mutex overhead on the happy path).
- Wire into `Main.cs`:
  - `TickSimDay()` wraps logic in `TryEnterTick()`/`ExitTick()`.
  - `SaveAll()` wraps logic in `TryEnterSave()`/`ExitSave()`.
  - If `TryEnterSave()` fails (tick in progress), queue save for next frame.
  - If `TryEnterTick()` fails (save in progress), skip this tick (4 Hz means ≤250ms delay).
- Add `IDisposable` guard pattern: `using var guard = saveLock.EnterTickScope()` for exception safety.
- Expose `SaveLock` through a new `ISaveLock` port interface so Core tests can verify behavior without Godot.

**Verification:**
- `dotnet build Ashfall.csproj` compiles cleanly.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` passes.
- New unit tests:
  - `SaveLock_CannotSaveWhileTicking` — `TryEnterSave()` returns false when tick is active.
  - `SaveLock_CannotTickWhileSaving` — `TryEnterTick()` returns false when save is active.
  - `SaveLock_ConcurrentStress` — 100 threads alternating tick/save, no deadlock within 5s
    timeout. Note: this is a synthetic stress test exercising `SaveLock` in isolation via
    `Task.Run`/`Thread` from the test harness — it does not reflect real production load, since
    the actual game never spawns more than the single main thread. This is a valid way to
    exercise `SaveLock`'s own correctness under contention, but its pass does not by itself
    prove anything about real gameplay unless/until a real second thread is introduced
    elsewhere. State that distinction in the test's doc comment so future readers don't
    mistake this synthetic stress test for evidence of tested production concurrency.
- No gameplay regression (tick still fires at 4 Hz, save still completes all 30 stores —
  corrected from 22; see Batch 79's corrected count, same underlying store set).

**Done when:** SaveLock is integrated into Main.cs, unit tests pass, and no deadlock is observed under stress. Note: since `TickSimDay()` and `SaveAll()` are called from the same single main thread today, `TryEnterSave()`/`TryEnterTick()` will never actually contend with each other in production until a second thread exists — integrating the lock now is "safe by construction" preparatory work, not a fix for an active bug, consistent with this batch's Corrected Framing.

---

## Step 5 — Make IEventBus Thread-Safe

**Goal:** Ensure `SimpleEventBus` (and any future `IEventBus` implementation) supports concurrent subscribe/unsubscribe/publish without tearing, list mutation exceptions, or missed deliveries.

**Implementation:**
- Audit current `SimpleEventBus` implementation for thread-unsafe patterns:
  - Subscriber list iteration during publish while another thread subscribes → `InvalidOperationException`.
  - No memory barriers on subscriber list reference → stale reads on other threads.
- Implement copy-on-write subscriber lists:
  ```csharp
  // Publish iterates a snapshot; subscribe/unsubscribe replaces the list atomically
  private volatile IReadOnlyList<Action<string, object>> _subscribers = Array.Empty<...>();

  public void Subscribe(Action<string, object> handler)
  {
      // Interlocked swap with new list including handler
  }

  public void Publish(string topic, object payload)
  {
      var snapshot = _subscribers; // volatile read — stable reference
      foreach (var handler in snapshot) handler(topic, payload);
  }
  ```
- Alternative: use `ConcurrentBag<T>` or `ImmutableList<T>` (from `System.Collections.Immutable`, available in `netstandard2.1`).
- Add `[ThreadSafe]` attribute to the new implementation.
- Ensure `IsSuppressed` pattern (from Unity's `EventBus`) is also thread-safe if ported.
- Do NOT change the `IEventBus` interface — only the implementation.

**Verification:**
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` compiles cleanly.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` passes.
- New concurrency tests:
  - `EventBus_ConcurrentSubscribePublish` — 10 threads subscribing while 10 threads publish, no exceptions.
  - `EventBus_UnsubscribeDuringPublish` — unsubscribe from within a handler, no crash.
  - `EventBus_SuppressedPublishIsNoOp` — suppressed bus discards events even under concurrent pressure.
- No existing event-driven behavior regresses (expeditions, economy, narrative events still fire correctly).
- As with Step 4's stress test, document in each test's XML comment that this is synthetic
  contention exercised by the test harness itself (via `Task.Run`), not a reproduction of
  anything the shipped game does today — `SimpleEventBus.Publish`/`Subscribe`/`Unsubscribe`
  are confirmed to be called only from the single main thread in the current codebase (see
  Corrected Framing note at the top of this document). This makes the fix correct and
  forward-looking, not a regression fix for an observed bug.

**Done when:** `SimpleEventBus` is thread-safe, concurrent tests pass, and no gameplay behavior changes.

---

## Step 6 — Add Thread-Safety Integration Tests

**Goal:** Simulate realistic concurrent scenarios (save during tick, catalog load during gameplay) and verify no corruption occurs.

**Implementation:**
- Create `Ashfall.Core.Tests/Threading/ThreadSafetyIntegrationTests.cs`:
  - **Test: CaptureStateDuringTick** — Start a tight loop of `Tick()` on one thread and `CaptureState()` on another for 1000 iterations. Verify: no `InvalidOperationException`, no null fields in captured state, captured state is internally consistent (e.g., survivor count matches survivor list length).
  - **Test: SaveAllDuringTick_WithSaveLock** — Same as above but with `SaveLock` engaged. Verify: `TryEnterSave` correctly queues, no interleaving occurs, all 30 stores produce valid JSON (corrected from 22 — see Batch 79's corrected save-store count, same underlying set of 30 stores).
  - **Test: CatalogLoadDuringGameplay** — Load catalog definitions on a background thread while gameplay ticks mutate runtime caches. Verify: no torn reads, no missing definitions mid-tick. Note: catalog hot-reload does not exist in this codebase today (confirmed: all 21 remaining `*CatalogLoader.cs` files load synchronously at startup with zero async/threading code) — this test exercises a hypothetical future capability, not existing behavior.
  - **Test: EventBusCrossThread** — Publish events from a background save-completion callback while the main thread processes tick events. Verify: no missed deliveries, no duplicate deliveries. Note: no "background save-completion callback" exists today; this test scaffolds for a feature this batch does not itself implement.
  - **Test: FlushDuringTick_WithSaveLock** — `FlushXxxIfDirty()` on one thread, `Tick()` on another. Verify: flush either completes with consistent data or is deferred.
- Use `Task.Run` + `ManualResetEventSlim` for deterministic interleaving control. Note: this
  introduces `Task.Run` into the test project — confirmed there are currently zero `Task.Run`
  call sites anywhere in `src/` or `Ashfall.Core/` today, so this is new territory for the test
  suite (though a reasonable and standard way to write concurrency tests); flag it as the first
  use of that primitive in the codebase rather than treating it as an established pattern.
- Use `ConcurrentBag<string>` to collect errors across threads without synchronization overhead in the test harness.
- Set test timeout to 10s per test (detect deadlocks).

**Verification:**
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` compiles cleanly.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` passes (all new tests green).
- No flaky failures across 10 consecutive test runs (`dotnet test --count 10` or loop in CI).
  Note: `dotnet test --count 10` is not a real dotnet-test CLI flag as of current .NET SDKs —
  `dotnet test` has no built-in `--count` option. The intent (run repeatedly to catch flakiness)
  is right; the command is not runnable as written. Replace with an actual loop, e.g.
  `for i in $(seq 1 10); do dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj || break; done`
  or an equivalent CI script step, and verify it against the installed SDK before relying on it.
- Tests explicitly document the race they're preventing (XML doc comment with scenario description), including whether that race is reachable in the shipped game today (per the notes above, none of them currently are).

**Done when:** All 5 integration tests pass reliably, no flakiness across 10 runs (using a real repeat-loop command, not the unverified `--count` flag), and each test documents its target race condition and trigger precondition.

---

## Step 7 — Document Threading Contract

**Goal:** Produce a living document that defines what runs on which thread, what requires the save lock, and how future async work must integrate.

**Implementation:**
- Create `docs/THREADING_CONTRACT.md` with sections:
  1. **Thread Model** — Single main thread today; save lock enables future async save. Diagram: Main Thread (Tick + Flush + UI) ↔ Save Lock ↔ Background Thread (future: SaveAll, CatalogLoad).
  2. **Rules for System Authors:**
     - All `Tick()` methods run on main thread only.
     - All `CaptureState()` calls require save lock (caller responsibility).
     - All `RestoreState()` calls require save lock AND must not fire events.
     - Public mutable collections must be marked `[SaveVisible]` if read during `CaptureState`.
     - Event handlers must not block (no I/O, no locks, no `Thread.Sleep`).
  3. **Rules for Host Authors (Godot Main.cs):**
     - `TickSimDay()` → `saveLock.TryEnterTick()` guard.
     - `SaveAll()` / `FlushXxxIfDirty()` → `saveLock.TryEnterSave()` guard.
     - Background save: deep-copy state under save lock, release lock, serialize on background thread.
  4. **Escape Hatches:**
     - `[ThreadSafe]` methods can be called from anywhere (must be truly safe, reviewed).
     - `Volatile.Read`/`Interlocked` for simple counters/flags (document each use).
  5. **Migration Guide** — How to convert a system from "not thread-aware" to "save-lock compliant" (3-step checklist).
  6. **Known Violations** — Pointer to `THREAD_SAFETY_AUDIT.md` for systems not yet migrated.
- Add a CI lint (optional, future): Roslyn analyzer that flags `[SaveVisible]` fields accessed outside save lock scope.
- Update `AGENTS.md` Invariant section to reference the threading contract.

**Verification:**
- `docs/THREADING_CONTRACT.md` exists with all 6 sections.
- Document is internally consistent with the implemented `SaveLock` API.
- At least 2 example systems are shown in the migration guide with before/after code.
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` still compiles.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` still passes.
- The "Update `AGENTS.md` Invariant section" bullet above needs a specific target: `AGENTS.md`
  currently has six numbered invariants (Invariant 1 — Zero engine coupling, through Invariant 6
  — Data authority is JSON); none of them is about threading today. Either add a new
  "Invariant 7 — Threading Contract" entry, or append a cross-reference under an existing
  invariant (e.g., a note under Invariant 5 — No gameplay logic in hosts, since `Tick()`
  placement is closely related). State which, explicitly, rather than leaving "reference the
  threading contract" underspecified.

**Done when:** Threading contract is documented, references the SaveLock implementation, includes migration guide, and is linked from a specific, named location in `AGENTS.md` (see above — not just "the Invariant section" in the abstract).

---

## Summary

| Step | Deliverable | Risk | New Tests | Key Files |
|------|-------------|------|-----------|-----------|
| 1 | `THREAD_SAFETY_AUDIT.md` — mutable state inventory | None (read-only) | 0 | All Core systems (audit) |
| 2 | Race condition catalog + top-10 risk list | None (analysis) | 0 | `THREAD_SAFETY_AUDIT.md` (updated) |
| 3 | `ThreadingAttributes.cs` + annotations on all save/tick methods | Low | 1 (analyzer) | `Assets/Ashfall.Core/Threading/ThreadingAttributes.cs` |
| 4 | `SaveLock` implementation + Main.cs integration | Medium | 3 | `Assets/Ashfall.Core/Threading/SaveLock.cs`, `src/Main.cs` |
| 5 | Thread-safe `SimpleEventBus` (copy-on-write) | Medium | 3 | `Assets/Ashfall.Core/Events/SimpleEventBus.cs` |
| 6 | Concurrency integration test suite | Low | 5 | `Ashfall.Core.Tests/Threading/ThreadSafetyIntegrationTests.cs` |
| 7 | `docs/THREADING_CONTRACT.md` + AGENTS.md update | None (docs) | 0 | `docs/THREADING_CONTRACT.md`, `AGENTS.md` |

**Total new tests:** 12
**Total new files:** 3 (ThreadingAttributes.cs, SaveLock.cs, ThreadSafetyIntegrationTests.cs) + 2 docs
**Estimated effort:** 3–5 days
**Exit criteria:** All existing tests pass (the "1941+" figure in the original plan is stale —
the most recent count found in this repo's own audit trail is "1941 → 1949" per
`10LOOP_AUDIT_REPORT.md`, but that document is itself a point-in-time snapshot from an earlier
audit pass, not a live count; re-run `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`
at implementation time and report the actual pass count rather than citing an inherited number),
12 new threading tests pass, no deadlocks under stress, threading contract documented and
enforceable.

**Reiterating this document's Corrected Framing:** every deliverable above is legitimate
preparatory engineering, but none of it is fixing a bug that manifests in the shipped game
today. Sequence this batch as **low-urgency, pre-emptive groundwork** — schedule it before any
batch that actually introduces concurrency (such as the async auto-save floated in Batch 79 Step
4), not ahead of batches fixing currently-reachable defects.


---

## Review Notes (Corrected)

**Review method:** direct verification against the codebase at
`/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`. Ground truth gathered by
exhaustive search for concurrency primitives (`Task.Run`, `async`/`await`, `Thread`,
`ThreadPool`, `Parallel.*`, `lock`, `Monitor`, `Interlocked`, `ConcurrentDictionary`,
`ConcurrentBag`, `SemaphoreSlim`, `Mutex`, `volatile`, `WorkerThreadPool`, `Godot.Thread`) across
every file in `src/` and `Assets/Ashfall.Core/`, plus a targeted check of every `_Process`/
`_PhysicsProcess` override and every save/catalog-loader file for async plumbing.

### Central premise verified — with an important framing correction
The plan's premise, "currently single-threaded," is **confirmed accurate**. The verification
found:
- Zero real concurrency primitives anywhere in gameplay/simulation code.
- Exactly one `Interlocked.Increment` call, defensive/habitual rather than evidence of actual
  concurrent access (nothing calls it from more than one thread today).
- Exactly two `async`/`await` usages, both confined to a test-only headless-UI frame-wait helper
  in `src/Main.cs` that explicitly does not run during normal gameplay.
- No `_PhysicsProcess` overrides at all; every `_Process` override runs on Godot's standard
  single main thread.
- No async file I/O, background save, or async catalog loading exists or is stubbed anywhere.
- **No other document in this repository references planned async save, background save,
  parallel tick subsystems, or async catalog hot-reload as scheduled work.** This was checked
  explicitly, not assumed.

**This means the batch is not solving an active problem — it is preparatory groundwork for
hypothetical future work that is not currently scheduled anywhere.** That is a legitimate thing
to build (a threading contract written before concurrency exists is safer and cheaper than one
retrofitted after a race condition ships), but the original document's "MEDIUM-HIGH" priority
label and its framing ("this is safe today, but fragile") oversell the urgency. Nothing about
the current single-threaded architecture is fragile in itself; it only becomes a concern the
moment a second thread is introduced, which no other plan proposes. The priority has been
corrected to **LOW-MEDIUM**, and every step's Done-when/verification section now states
explicitly which races are reachable today (none) versus which require a future architectural
change to manifest (all of them).

### Cross-plan finding
Batch 79 (Multi-Save Slot System), reviewed in the same pass, casually suggests in its Step 4
("Add Auto-Save Trigger on Day Advance") that auto-save "consider async write... if >200ms" —
i.e., it proposes introducing this codebase's first-ever async I/O path without referencing this
batch's threading contract at all. Both documents have been cross-annotated: Batch 79 now flags
that its async suggestion collides with this batch's groundwork and should be sequenced after
it (or dropped until the 100ms/200ms budget is actually measured and exceeded), and this batch
now states explicitly that it should be scheduled *before* any batch that introduces real
concurrency, precisely because none exists yet to retrofit against.

### Factual errors found and corrected
1. **Save store count.** Same stale "22" figure identified in Batch 79 — this document
   independently repeats it in the header metadata, the Context section, and Step 4 and Step 6's
   verification bullets. All corrected to the verified count of **30** (see Batch 79's Review
   Notes for the full derivation and the list of the 5 stores added since that count was last
   corrected, in an earlier pass, from 22 to 25).
2. **`dotnet test --count 10`** (Step 6 verification) is not a real flag on current `dotnet test`
   CLI. Replaced with an actual shell loop as a runnable equivalent.
3. **"Exit criteria: All 1941+ existing tests pass"** cites a specific number without sourcing
   it or confirming it's current. The most recent figure found anywhere in this repo's own audit
   trail is "1941 → 1949" in `10LOOP_AUDIT_REPORT.md`, itself a point-in-time snapshot from an
   earlier pass, not a live count re-verified for this review. Corrected to instruct re-running
   the test suite at implementation time rather than trusting an inherited number.
4. **"Expect 40–80 race candidates based on 82+ systems"** (Step 1) is presented as if derived
   from analysis, but is an unverified guess — no grep or count backing "40-80" was found or
   reproducible. Corrected to remove the specific numeric target and gate completion on
   coverage (100% of registered systems checked) instead of a guessed count.
5. **"≥80% of CaptureState/RestoreState methods annotated"** (Step 3) has no stated denominator
   — 80% of what total is unverifiable without first counting the actual methods. Flagged with
   the exact grep needed to establish the denominator before treating this as a real bar.
6. **"Update AGENTS.md Invariant section"** (Step 7) doesn't specify where. `AGENTS.md` has six
   numbered invariants today, none about threading. Corrected to require an explicit decision
   (new Invariant 7, or a cross-reference under an existing invariant) rather than leaving the
   target ambiguous.

### Scope / logic issues attacked
- **Step 2's severity scale** (Critical/High/Medium/Low) has no tier for "not reachable without
  a future architectural change first" — under the plan's own "Low = theoretical only with
  current architecture" definition, nearly every item in the catalog would already qualify as
  Low, which undercuts the plan's own MEDIUM-HIGH priority framing. Flagged as an internal
  inconsistency between the severity scale's stated definition and the document's overall
  urgency framing.
- **Step 4 and Step 6's stress tests** (100 threads, `Task.Run`-based interleaving) are valid
  ways to test `SaveLock`/`SimpleEventBus` in isolation, but both are synthetic harness-driven
  concurrency, not a reproduction of real production load — the game itself never spawns a
  second thread. Both steps now carry an explicit note so a reader doesn't mistake "stress test
  passes" for "production concurrency has been tested."
- **Step 6's `Task.Run` usage** would be the first use of that primitive anywhere in this
  codebase (confirmed zero existing `Task.Run` call sites). Flagged, not blocked — using it in
  a test harness to simulate concurrency is standard and reasonable, but it's worth knowing this
  is new territory rather than an established pattern being reused.

### Risk/rollback
The original document had no explicit rollback section. Given every deliverable here is
additive (new attributes, a new lock class, a new event-bus implementation, new docs) and none
of it changes existing single-threaded behavior when unused, the practical rollback story is:
revert the introducing commits; `SaveLock` and the thread-safe `SimpleEventBus` are opt-in
infrastructure that nothing currently depends on, so reverting carries no data-loss or
save-compatibility risk (unlike Batch 79, whose Step 3 does carry real save-compatibility risk).
This is worth stating explicitly rather than leaving readers to infer it, since it is one of the
few genuinely low-risk aspects of either batch reviewed in this pass.
