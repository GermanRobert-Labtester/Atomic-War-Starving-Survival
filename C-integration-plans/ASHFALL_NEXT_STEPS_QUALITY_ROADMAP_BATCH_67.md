# ASHFALL — Quality Roadmap Batch 67

## Theme: Host Session Standardization — Extract Common Patterns into Base Class

**Priority:** MEDIUM (27 host sessions in `src/Host/` — 29 project-wide — with some repeated boilerplate; see Review Notes for the corrected pattern)
**Risk:** Medium — the actual shared pattern is different from what this batch originally assumed, and 2 of 27 sessions already have an `ILog` field the base class design must account for (not zero, as an earlier draft of this review claimed); see corrections below before starting
**Blocked by:** Nothing at the "start auditing" level, but **Step 3 cannot be marked done until the test-location question (Core-side testable logic vs. a Godot-host self-test target) is resolved** — this is a soft internal blocker between Step 3 and Step 7, not an external dependency.
**Unlocks:** Enforced save/load contract, uniform dirty-flag semantics, reduced onboarding friction for new systems

---

## Context

**Corrected against the actual codebase.** The original context section below claimed 34 host session files, each with a `private bool _dirty` field, a `MarkDirty()` helper, and an `ILog` field. Verified by reading `src/Host/CombatHostSession.cs`, `src/Host/DoseLedgerHostSession.cs`, `src/Host/HoldfastRuntimeSession.cs`, and grepping all of `src/Host/*.cs`: **none of this is accurate.**

- **File count is 27, not 34.** `ls src/Host/*HostSession.cs` (excluding `.uid` sidecars) returns exactly 27 files.
- **No host session has a `private bool _dirty;` field or a `MarkDirty()` method.** `grep -r "private bool _dirty;\|MarkDirty()" src/Host/*.cs` returns zero matches across the entire directory.
- **The actual pattern is a `public event Action StateChanged;` (or similarly named) event on the session**, raised from Core system event handlers wired in the constructor (e.g. `DoseLedgerHostSession`: `Ledger.OnStateChanged += _ => StateChanged?.Invoke();`). The dirty-flag *bool* itself lives on `Main.cs` (the host), not on the session — e.g. `_yearOfAshDirty`, `_phase0Dirty`, `_dailyBriefingDirty`, `_medicalWardDirty`, `_memorialDirty` are private fields declared directly in `src/Main.cs`, set to `true` inside a lambda subscribed to the session's `StateChanged` event, and cleared by the corresponding `FlushXxxIfDirty()` method. This is architecturally the opposite of what the original plan described: the session raises an event, the *host* owns the dirty bit, not the session.
- **Correction (previous draft of this review was wrong on this point): 2 of the 27 host sessions DO have an `ILog` field.** A precise re-check — `grep -n "ILog _log\|ILog Log\|readonly ILog" src/Host/*HostSession.cs` — finds `DutyRosterHostSession.cs:78: private readonly ILog _log;` (populated from a `log ?? NullLog.Instance` constructor default, `NullLog` being the null-object `ILog` implementation in `Assets/Ashfall.Core/HostDefaults.cs`) and `ShelterAssignmentHostSession.cs:82: private static readonly ILog s_log = new GodotLog();`. So "no session has a logger field" is false as a universal claim — it is true for most (25 of 27) but not all. Logging elsewhere does live in the paired `SaveStore` static classes as originally noted (e.g. `DutyRosterSaveStore.cs` has its own private static `s_log` and calls `s_log.Error(...)` on save/load failure) — that part holds — but the two session-level exceptions above must be accounted for before assuming the base class introduces logging where literally none existed. Treat this as "logging is inconsistently present today" (2 sessions have it, 25 don't, and the pattern differs — instance field with DI default vs. static field with a hardcoded `GodotLog`), not "logging is uniformly absent."
- **`CaptureSave()`/`RestoreSave()` naming is real but not universal.** `CombatHostSession` does have `CaptureSave()`/`RestoreSave(CombatState state)` methods that forward to `Engine.CaptureState()`/`Engine.RestoreState()`. `DoseLedgerHostSession` instead has `CaptureSave(int simDay)`/`RestoreSave(DoseLedgerSave save)` that forward to a **static codec** (`DoseLedgerSaveCodec.Capture(...)`/`.Restore(...)`), not directly to a single Core system's `CaptureState()`/`RestoreState()` — because `DoseLedgerHostSession` wraps four Core systems (`DoseLedgerSystem`, `SickListSystem`, `CohortSystem`, `VoluntaryRegisterSystem`), not one. The signatures, parameter counts, and even whether persistence goes through a codec versus a single system's state methods **vary per session** — this is a real source of complexity the original plan underestimated by assuming a uniform `TState CaptureSave()` / `void RestoreSave(TState)` shape fits every session.

**What this means for the batch:** the premise that a generic `HostSessionBase<TSystem, TState>` can mechanically absorb "the boilerplate" is only partly true. The dirty-flag half of the plan is describing state that currently lives in the wrong class (`Main.cs`, not the session) — moving it into the session is a legitimate architectural improvement, but it is a *behavior change* (dirty tracking moves from host to session), not a refactor-preserving-behavior change, and should be described and risked as such. The save/load half of the plan needs a base class flexible enough to handle both "forward to one Core system" and "forward to a static multi-system codec" shapes, which the original `HostSessionBase<TSystem, TState>` sketch (single `TSystem`, direct `CaptureStateCore()`/`RestoreStateCore()` calls) does not obviously support without the `MultiSystemHostSessionBase` variant doing most of the real work — meaning most sessions may end up needing the "complex" base class, not the "simple" one, inverting the plan's assumption that most sessions are simple.

The boilerplate that IS genuinely repeated across sessions, based on direct reading of `CombatHostSession`, `DoseLedgerHostSession`, and `HoldfastRuntimeSession`:
- Constructor default-construction pattern: `Ledger = ledger ?? new DoseLedgerSystem();` for each wrapped system, with optional-parameter constructors.
- Event subscription wiring in the constructor, forwarding Core system events to a single `StateChanged`/similarly-named C# event on the session.
- A `Create(string dataDir, ILog log = null)` static factory method pattern (seen in `DoseLedgerHostSession.Create`) that loads catalogs and constructs the session — not every session necessarily has this, but it recurs.
- `CaptureSave`/`RestoreSave` method *names* recur, but their signatures and whether they delegate to a system directly or to a static codec do not.

Each new host session re-implements this from scratch, risking subtle inconsistencies. A base class could still DRY up the parts that are genuinely uniform (constructor null-coalescing, event-to-`StateChanged` forwarding), but Step 1's audit — which this plan already correctly calls for — needs to be run for real, with correct starting assumptions, before Step 2's design proceeds. Do not proceed past this Context section using the original "verified" claims; they were not verified and were wrong.

---

## Step 1 — Audit 5 Representative Host Sessions: Identify Exact Shared Boilerplate

### Goal

Select five host sessions spanning simple (single-system) to complex (multi-system) and document every line of boilerplate they share. Produce a precise inventory of what the base class must provide.

**Correction:** this review already read `CombatHostSession.cs`, `DoseLedgerHostSession.cs`, and `HoldfastRuntimeSession.cs` directly (see Context section) and found no `_dirty` field, no `MarkDirty()`, and no `ILog` field in any of them — contradicting the original plan's Step 1 "expected" boilerplate list. Do not run this audit expecting to confirm those items; run it to characterize the real pattern (a `StateChanged`-style event on the session, dirty bits living on `Main.cs`, and save/load forwarding that sometimes targets one Core system directly and sometimes targets a static multi-system codec).

### Implementation

1. Select representative sessions (file existence re-verified against `src/Host/`):
   - **Simple:** `DoseLedgerHostSession.cs` (wraps 4 Core systems via a codec — despite the name suggesting "simple," this is actually a multi-system-via-codec case; read it first since it's already been reviewed here)
   - **Simple:** `GreenhouseHostSession.cs` (verify system count before assuming single-system)
   - **Moderate:** `CombatHostSession.cs` (single Core system — `Engine` — with direct `CaptureSave()`/`RestoreSave()` forwarding; already reviewed here and confirmed to match this simpler shape)
   - **Moderate:** `EconomyHostSession.cs` (verify sub-system composition before assuming complexity tier)
   - **Complex:** `HoldfastRuntimeSession.cs` (multi-system, expansion-aware, duplicates some Core logic — H1 issue; already reviewed here and confirmed to forward to two separate save stores, `HoldfastSaveStore` and `HoldfastTradeSaveStore`, via two separate `TrySave`/`TryLoad`-style methods rather than one uniform `CaptureSave`/`RestoreSave` pair)

   **Correction (previous draft of this review got this wrong): `DiseaseHostSession` exists — it is just not in `src/Host/`.** It lives at `src/Disease/DiseaseHostSession.cs` (confirmed by direct read). It matches the simple single-system shape closely: wraps one Core system (`DiseaseSystem`), forwards Core events (`OnInfection`, `OnQuarantineStarted`, `OnOutbreakDeclared`, etc., plus a generic `OnStateChanged`) to a single public `StateChanged` event, exactly like `CombatHostSession`. Add it as a genuine sixth candidate or swap it in for one of the five — it is a clean, low-risk example of the single-system shape. `WeatherHostSession` was searched for project-wide (`find . -iname WeatherHostSession.cs`, excluding `.git`) and genuinely does not exist under that name anywhere in the repository — that part of the original correction stands. Do not substitute a guess for it; if weather-related dirty tracking is needed, locate it by searching for `WeatherSystem` usage in `src/Main.cs` instead of assuming a session class exists.
   **Broader correction: the file-count audit (`ls src/Host/*HostSession.cs`) undercounts total host sessions project-wide.** Two more genuinely exist outside `src/Host/`: `src/Disease/DiseaseHostSession.cs` (above) and `src/Foundry/SilentFoundryHostSession.cs` and `src/YearOfAsh/YearOfAshHostSession.cs` (both confirmed to exist — see the Step 6 and Review Notes corrections below, which previously and wrongly listed both as fabricated/nonexistent). The true project-wide total is **29** `*HostSession.cs` files (27 in `src/Host/` + these 3 elsewhere), though this batch's primary scope (the `HostSessionBase` migration) can reasonably stay focused on `src/Host/`'s 27 if that boundary is stated explicitly — the point is the boundary must be *stated*, not implied by an incomplete `ls src/Host/` glob presented as if it were a whole-repository search.

2. For each, extract and annotate:
   - Field declarations (system refs, dirty flag **— expect none on the session itself**, logger **— expect none**, config)
   - Constructor pattern (parameter list, system construction via `??`-coalescing defaults, event wiring to a `StateChanged`-style event)
   - Event handler shape (what the session's public event actually is named, and what it forwards)
   - `CaptureSave()`/equivalent method body — confirm whether it forwards to one system's `CaptureState()` or to a static multi-system codec `Capture(...)` call, and what parameters it needs beyond `this` (e.g. `DoseLedgerHostSession.CaptureSave(int simDay)` needs the current day)
   - `RestoreSave()`/equivalent method body — same distinction
   - Any public action/demo methods that follow delegate-to-core pattern
   - Deviations from the common pattern (unique logic, multi-system coordination, multiple save stores per session as in `HoldfastRuntimeSession`)

3. Produce a diff-style comparison showing identical lines across all five — expect this to be a much shorter list than the original plan assumed (constructor null-coalescing and event-forwarding lines, not dirty-flag/logging boilerplate).
4. Identify the minimal abstract surface: what must every host session provide, given that "one system, direct forwarding" and "N systems, codec forwarding" are both common shapes?

### Verification

- Audit covers all five sessions completely, using their real, confirmed file names and contents (not the assumed boilerplate from the original plan).
- Shared boilerplate is quantified against what's actually there — expect this number to be smaller than "30-50 lines per session," since the dirty-flag/logging boilerplate that made up much of that estimate does not exist in the session classes.
- Deviations are explicitly documented with rationale, especially the codec-vs-direct-forwarding split and the multi-save-store case (`HoldfastRuntimeSession`).

### Done when

All of the following are true and checked off individually, not just "a specification exists":
- [ ] The exact shared boilerplate lines are written down verbatim (copy-pasted, not paraphrased) from the actual source of at least 4 of the 5 audited sessions, with file:line citations.
- [ ] The required abstract members for the base class are listed, and each one is checked against at least one confirmed direct-forwarding session (e.g. `CombatHostSession`) and at least one confirmed codec-forwarding session (e.g. `DoseLedgerHostSession`) to show it fits both.
- [ ] Optional override points for complex sessions are listed with the specific session (e.g. `HoldfastRuntimeSession`'s dual-save-store shape) that motivates each override point.
- [ ] The dirty-flag ownership question has an explicit written decision (not left open): either "moves into the base class" or "stays on `Main.cs`," with the one-paragraph rationale and the specific `Main.cs` fields (from the list of 25 confirmed by grep) that are affected if it moves.

---

## Step 2 — Design HostSessionBase<TSystem, TState> Abstract Class

### Goal

Design an abstract base class that encapsulates the host session contract while remaining flexible enough for both single-system and multi-system sessions.

### Implementation

1. Define the class in `src/Host/HostSessionBase.cs`:

```csharp
namespace AtomicWar.GodotApp;

/// <summary>
/// Base class for Godot host sessions that wrap one or more Core systems.
/// Enforces: dirty-flag management, save/load forwarding, structured logging.
/// </summary>
public abstract class HostSessionBase<TSystem, TState>
    where TSystem : class
    where TState : class, new()
{
    protected readonly TSystem System;
    protected readonly ILog Log;

    private bool _dirty;
    public bool IsDirty => _dirty;

    protected HostSessionBase(TSystem system, ILog log)
    {
        System = system ?? throw new ArgumentNullException(nameof(system));
        Log = log ?? throw new ArgumentNullException(nameof(log));
        WireEvents();
    }

    // --- Dirty flag management ---

    protected void MarkDirty()
    {
        _dirty = true;
        OnDirtyStateChanged();
    }

    public void ClearDirty() => _dirty = false;

    /// <summary>Optional hook when dirty state changes (e.g., schedule UI refresh).</summary>
    protected virtual void OnDirtyStateChanged() { }

    // --- Save/Load contract ---

    /// <summary>Capture current system state into a serializable DTO.</summary>
    public TState CaptureSave()
    {
        var state = CaptureStateCore();
        ClearDirty();
        return state;
    }

    /// <summary>Restore system state from a previously captured DTO.</summary>
    public void RestoreSave(TState state)
    {
        if (state == null)
        {
            Log.Warn($"[{GetType().Name}] RestoreSave called with null state — skipping.");
            return;
        }
        RestoreStateCore(state);
        ClearDirty();
    }

    // --- Abstract members (subclass must implement) ---

    /// <summary>Wire Core system events to MarkDirty() and UI refresh.</summary>
    protected abstract void WireEvents();

    /// <summary>Forward to Core system's CaptureState().</summary>
    protected abstract TState CaptureStateCore();

    /// <summary>Forward to Core system's RestoreState().</summary>
    protected abstract void RestoreStateCore(TState state);
}
```

2. Design a companion `MultiSystemHostSessionBase` for sessions wrapping 2+ Core systems:
   - Takes a params array or explicit system references.
   - `CaptureStateCore` returns a composite DTO.
   - `RestoreStateCore` distributes state to each sub-system.
   - **Correction (see Step 1):** based on the sessions already read during this review (`DoseLedgerHostSession` wraps 4 systems via a static codec call, `HoldfastRuntimeSession` wraps multiple systems across *two* separate save stores), expect the majority of real sessions to need this variant, not the single-system `HostSessionBase<TSystem, TState>`. Size the design effort accordingly — this is likely the primary base class in practice, not a secondary "complex case" variant.
   - Additionally design for the case where `CaptureStateCore`/`RestoreStateCore` need extra runtime parameters beyond `this` (e.g. `DoseLedgerHostSession.CaptureSave(int simDay)` needs the current sim day passed in). A parameterless abstract `TState CaptureStateCore()` signature, as sketched below, cannot represent this without an awkward workaround (stashing `simDay` in a field before calling `CaptureSave()`, which reintroduces the kind of implicit state the base class is meant to eliminate). Resolve this in the design, not by working around it later during migration.

3. Define `IHostSession` interface for polymorphic save orchestration:
```csharp
public interface IHostSession
{
    bool IsDirty { get; }
    void ClearDirty();
}
```

4. Ensure no `UnityEngine.*` references — this lives in `src/` (Godot host layer). **Note:** this base class is Godot-host-only C# (`AtomicWar.GodotApp` namespace, per the sketch above) — it is not part of `Ashfall.Core` and cannot be unit-tested from `Ashfall.Core.Tests`, which only compiles against the Core project. Plan test coverage accordingly (see Step 3 correction).
5. Document that `TState` must be `[Serializable]` and compatible with `IJsonSerializer`.

### Verification

- Base class compiles in the Godot host project (`dotnet build Ashfall.csproj`).
- No Core references to the base class (it lives in host, not Core).
- Design accommodates all five representative sessions from Step 1, including the codec-forwarding and extra-parameter cases identified above.

### Done when

`HostSessionBase<TSystem, TState>` and `MultiSystemHostSessionBase<TState>` are both defined (not just the single-system variant — see correction above), compile cleanly, and the design is reviewed against all identified patterns, including the parameterized-capture case.

---

## Step 3 — Implement Base Class with Dirty Flag, Event Wiring Infrastructure, Save/Load Forwarding, and ILog Threading

### Goal

Complete the production implementation including edge cases: re-entrant event handling, thread safety for dirty flag, structured logging with session name prefix, and integration with the existing `SaveStore` pattern.

**Correction:** the original plan's Step 3 title and content assume the base class introduces `ILog` "threading" into sessions that currently have loggers — they don't (see Context section: zero host sessions have an `ILog` field today; logging lives in the paired `SaveStore` classes). Adding `Log`/`LogInfo`/`LogWarn`/`LogError` to the base class is a legitimate new capability, but frame it as adding logging where none existed, not as consolidating existing per-session logging.

### Implementation

1. Finalize `HostSessionBase<TSystem, TState>`:
   - Add `[MethodImpl(MethodImplOptions.AggressiveInlining)]` to `MarkDirty()` for hot-path performance. (This is unlikely to matter in practice — `MarkDirty()` is called from event handlers on state mutation, not in a tight loop — treat this as a nice-to-have, not something to spend verification time confirming via disassembly.)
   - Add `SessionName` virtual property (defaults to `GetType().Name`) for log prefixing.
   - Add `LogInfo(string msg)` / `LogWarn(string msg)` / `LogError(string msg)` helpers that auto-prefix.

2. Add `HostSessionBase` save integration helpers:
   - `protected string ComputeChecksum(TState state)` — delegates to `SaveChecksum.Compute`.
   - `protected bool ValidateChecksum(TState state, string expected)` — integrity gate.

3. Add `UnwireEvents()` virtual method for cleanup/disposal.

4. Implement `MultiSystemHostSessionBase<TState>`:
   - No generic `TSystem` constraint (multiple heterogeneous systems).
   - Abstract `RegisterSystems()` called from constructor.
   - Dirty flag aggregated across all registered systems.

5. Write unit tests — **corrected location:** since `HostSessionBase<TSystem, TState>` lives in `src/Host/` (Godot host project, `AtomicWar.GodotApp` namespace) and not in `Ashfall.Core`, tests for it CANNOT live in `Ashfall.Core.Tests/Host/HostSessionBaseTests.cs` as the original plan stated — `Ashfall.Core.Tests.csproj` does not reference the Godot host assembly. Either:
   - (a) extract the base class's testable logic (dirty-flag semantics, checksum helpers) into a Core-side helper that both `Ashfall.Core.Tests` and the host can exercise, keeping only the Godot-specific wiring in `src/Host/`, or
   - (b) add a Godot-host-side test target (check whether one already exists — `HostCli.SelfTests.cs`/`HostCli.PanelTests.cs` in `src/Host/` appear to be the existing pattern for host-level self-tests run via `godot --headless --path . -- --some-selftest`, not `dotnet test`) and write the tests there instead.

   Whichever approach is chosen, the test file path and the command used to run it must actually work — verify this before writing the Done-when criteria. Test cases to cover regardless of location:
   - Test: constructing with null system throws.
   - Test: `MarkDirty()` sets `IsDirty` to true.
   - Test: `CaptureSave()` clears dirty flag.
   - Test: `RestoreSave(null)` logs warning and does not throw.
   - Test: subclass `WireEvents` is called during construction.

### Verification

```bash
dotnet build Ashfall.csproj                                  # Host compiles
```
The original plan's `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "HostSessionBase"` step is **not runnable as written** if the base class stays in `src/Host/` — that test project cannot see host-layer types. Replace with whichever of the two approaches above is chosen; if (b), the verification command is a `godot --headless` selftest invocation, not a `dotnet test` filter.

### Done when

Base class is fully implemented with logging, checksum helpers, and multi-system variant. 5+ tests pass, run via a command that is confirmed to actually execute them (see correction above — do not claim `dotnet test --filter "HostSessionBase"` passes without first confirming the tests are reachable from that project).

---

## Step 4 — Migrate 5 Simple Host Sessions to Inherit from Base

### Goal

Convert five single-system host sessions to inherit from `HostSessionBase<TSystem, TState>`, proving the pattern eliminates boilerplate without changing behavior.

### Implementation

Target sessions (single Core system, straightforward save):

**Correction: 3 of the original 5 target names do not exist anywhere in the repository (confirmed via project-wide `find`, not just `src/Host/`); `DiseaseHostSession` exists but lives in `src/Disease/`, not `src/Host/`.** Revised target list:
1. `DiseaseHostSession` (`src/Disease/DiseaseHostSession.cs`) — wraps `DiseaseSystem`, single-system, `StateChanged` event pattern confirmed by direct read; genuinely fits this "simple" step.
2. `DoseLedgerHostSession` — **reclassify:** confirmed in Step 1/Context to wrap **four** Core systems via a static codec (`DoseLedgerSaveCodec`), not one system — this does NOT belong in this "simple, single-system" step. Move it to Step 5.
3. ~~`WeatherHostSession`~~ — does not exist anywhere in the repository (confirmed by project-wide search). Do not assign; if weather-related save/dirty logic needs migrating, trace it from `WeatherSystem` usage in `src/Main.cs` first to find its actual host wrapper (if any exists) rather than assuming a dedicated session class.
4. ~~`JournalHostSession`~~ — does not exist anywhere in the repository (confirmed by project-wide search). Per AGENTS.md's H11 and the Save/Load section, journal persistence goes through `JournalSaveStore` directly; there may be no dedicated host session wrapper at all for this system. Confirm before assigning.
5. `GreenhouseHostSession` — confirmed to exist in `src/Host/`; verify system count directly (not yet read in this review) before assuming single-system.

Since 3 of the original 5 named targets are invalid, this step needs the real Step 1 audit's classification to actually pick 5 qualifying single-system sessions from the verified 27 (+2 outside `src/Host/`) list — do not proceed with a fixed pre-picked list; substitute two more genuinely single-system sessions (e.g. candidates to verify: `CraftingHostSession`, `RadioHostSession`, `ResearchHostSession`, `UtilityAiHostSession` — none of these four were read in this review, so confirm system count before assigning).

For each:
1. Change declaration: `public class DiseaseHostSession : HostSessionBase<...>` for genuinely single-system sessions (see revised target list above) — **`DoseLedgerHostSession` was removed from this step's target list** because it wraps four Core systems via a static codec (`DoseLedgerSaveCodec`), not one system with direct `CaptureState()`/`RestoreState()` calls, so it almost certainly needs `MultiSystemHostSessionBase<TState>`; it belongs in Step 5, not here. Re-verify which of the sessions chosen in Step 1 are genuinely single-system before assigning them to this "simple" step.
2. Simplify constructor: call `base(system, log)` (single-system case) or the multi-system base's constructor, remove manual field assignments.
3. Implement `WireEvents()`: move event subscriptions from constructor body — for sessions using the `StateChanged`-event pattern (verified in `DoseLedgerHostSession`), this means redirecting each Core system's `OnStateChanged`/similar event to call the base class's dirty-marking mechanism instead of (or in addition to) invoking the session's own public `StateChanged` event, if external consumers (e.g. `Main.cs`'s `_yearOfAshDirty`-style fields) still need to subscribe to it externally. Decide during Step 1/2 whether the public `StateChanged` event is kept for host consumers or fully replaced by the base class's `IsDirty`/`ClearDirty()` surface — removing it without checking `Main.cs`'s subscriptions will break compilation there.
4. Implement `CaptureStateCore()`: return `System.CaptureState()` for single-system sessions; for codec-backed sessions, this needs the codec's static `Capture(...)` call and any extra parameters it requires (see Step 2 correction re: `DoseLedgerSaveCodec.Capture(int simDay, ...)`).
5. Implement `RestoreStateCore(state)`: call `System.RestoreState(state)` or the codec's `Restore(...)`.
6. Remove: manual null checks, duplicated log field **if one existed** (per the Context correction, most sessions never had one — this line item may be a no-op for most of the five).
7. Verify all public API methods still compile and delegate correctly.
8. Run existing tests and selftest verbs for each session — **verify each session actually has a selftest verb before assuming one exists**; `HostCli.SelfTests.cs`/`HostCli.PanelTests.cs` register some but not necessarily all sessions' selftests (`HostCli.PanelTests.cs` was directly observed in this review to exercise `YearOfAshSaveStore` and `DutyRosterSaveStore` round-trips, for example — check the equivalent exists for each of the five sessions chosen here before treating "run its selftest" as a checkbox item).

### Verification

```bash
dotnet build Ashfall.csproj                                  # 0 errors, 0 warnings
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
```

### Done when

All five sessions inherit from the correct base class (single-system or multi-system, per their actual shape — not assumed), behavior is unchanged (dirty-flag ownership question from Step 2 resolved and consistently applied), and all existing tests and selftests pass unchanged. Do not claim a specific "lines saved" figure unless actually measured — the original "20-40 lines saved per session" estimate was based on boilerplate (a `_dirty` field, an `ILog` field) that this review found largely does not exist, so the real savings will likely be smaller and should be measured, not assumed.

---

## Step 5 — Migrate Remaining Multi-System / Complex Host Sessions

### Goal

Convert host sessions that manage multiple Core systems, forward through a static codec, or have complex event coordination to use `MultiSystemHostSessionBase`.

### Implementation

Target sessions — **corrected against `ls src/Host/*HostSession.cs`.** The original plan named `EconomyHostSession` (confirmed to exist), `CombatHostSession` (confirmed to exist, but per Step 1's correction this one is actually single-system, not multi — reclassify to Step 4 or here based on the real Step 1 audit output, not this list), `HoldfastRuntimeSession` (confirmed to exist and to be genuinely complex — it forwards to *two* separate save stores), `ExpeditionHostSession` (confirmed to exist), and `MedicalHostSession` (confirmed to exist). Use the Step 1 audit's actual classification, not a fixed pre-assigned list, to decide which five (or however many) sessions land in this step versus Step 4 — the boundary between "simple" and "complex" cannot be decided correctly before Step 1 runs.

1. Determine if `HostSessionBase<TSystem, TState>` with a composite system facade works, or if `MultiSystemHostSessionBase<TState>` is needed.
2. For multi-system sessions: create a lightweight facade class that groups the sub-systems, or use `MultiSystemHostSessionBase` with explicit system registration.
3. Move event wiring into `WireEvents()` override.
4. Implement composite `CaptureStateCore()` that aggregates sub-system states.
5. Implement composite `RestoreStateCore()` that distributes state to sub-systems.
6. Preserve any session-specific logic in clearly-named override methods.
7. Do NOT refactor `HoldfastRuntimeSession`'s duplicated core logic (that's H1, a separate task) — only refactor the host wrapper boilerplate. **Additionally:** `HoldfastRuntimeSession` persists through two independent save stores (`HoldfastSaveStore` for `World.CaptureSave()` and `HoldfastTradeSaveStore` for `Trade.CaptureState()`, confirmed by direct reading of `src/Host/HoldfastRuntimeSession.cs`). A `MultiSystemHostSessionBase<TState>` designed around one composite `TState` may not cleanly represent "two independent save files from one session" — resolve this in Step 2's design or treat `HoldfastRuntimeSession` as an explicitly-documented exception that keeps its current dual-save-store shape.

### Verification

```bash
dotnet build Ashfall.csproj
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
```

### Done when

All sessions classified as multi-system/complex in the Step 1 audit use the appropriate base class infrastructure, composite state capture/restore works correctly, `HoldfastRuntimeSession`'s dual-save-store shape is either accommodated or explicitly documented as an exception, and no behavioral regressions exist in tests or selftests.

---

## Step 6 — Migrate Remaining Host Sessions

### Goal

Complete the migration so that every host session in `src/Host/` inherits from the appropriate base class. No session manually implements ad-hoc save/load forwarding outside the base class contract.

### Implementation

**Correction: the file count is 27 within `src/Host/`, not 34 — but 29 project-wide (see Step 1's correction).** `ls src/Host/*HostSession.cs` (excluding `.uid` sidecars) returns exactly 27 files inside that directory. The corrected, verified full `src/Host/` list is:

`CombatHostSession`, `CraftingHostSession`, `DeepCoastHostSession`, `DoseLedgerHostSession`, `DutyRosterHostSession`, `EconomyHostSession`, `ExpansionHostSession`, `ExpeditionHostSession`, `GreenhouseHostSession`, `InventoryHostSession`, `MaritimeHostSession`, `MedicalHostSession`, `MusterHostSession`, `NarrativeHostSession`, `PhantomMemoryHostSession`, `Phase0HostSession`, `PowerGridHostSession`, `RadioHostSession`, `ResearchHostSession`, `ShelterAssignmentHostSession`, `StandingRecordHostSession`, `StartingLevelHostSession`, `SurvivorsHostSession`, `TravelingCaravanHostSession`, `UtilityAiHostSession`, `VerdictHostSession`, `WorldHostSession`.

**Three more `*HostSession.cs` files exist outside `src/Host/` — this step (and the batch generally) must decide explicitly whether they're in scope:** `src/Disease/DiseaseHostSession.cs`, `src/Foundry/SilentFoundryHostSession.cs`, `src/YearOfAsh/YearOfAshHostSession.cs`. All three were directly confirmed to exist by file search and, for `DiseaseHostSession`, by reading the file. **A previous draft of this review incorrectly listed `SilentFoundryHostSession` and `YearOfAshHostSession` among "sessions that do not exist" — that was wrong; both files are real, they are simply organized by feature directory (`src/Foundry/`, `src/YearOfAsh/`) rather than under the flat `src/Host/` tree.** This is exactly the kind of directory-scoping mistake this batch must not repeat: any "does not exist" claim must be backed by a project-wide search (`find . -iname "<Name>.cs"`, excluding `.git`), not a search scoped to `src/Host/` alone.

**Of the original plan's other nine remaining named sessions, a project-wide search (`find . -iname "<name>.cs"`, excluding `.git`) genuinely found no matches for:** `CrossingHostSession`, `WarlordDoctrineHostSession`, `DeepLoreHostSession`, `CurrentsHostSession`, `WitnessHostSession`, `DiveSiteHostSession`, `FinalWishHostSession`, `NobodysCharterHostSession`, `CoalitionHostSession`, `CensusHostSession`. These nine really do appear to be fabricated or renamed beyond recognition. Some of this functionality may exist under different session names (e.g. Year-of-Ash-related logic is confirmed to live in `YearOfAshHostSession`, not a separately-named session, and dirty fields for several of these areas live directly on `Main.cs` — see the 25 `*Dirty` boolean fields at `src/Main.cs` lines 51-233, confirmed by direct grep). **Do not plan migration work against a session name until a project-wide `find`/`ls`, not a directory-scoped one, confirms it exists.** Re-run the audit at the start of this step rather than trusting the count/list above, since the codebase will have changed between Steps 4/5 and this step.

For each batch of 4-5 (from the verified list above, minus whichever were already migrated in Steps 4/5):
1. Classify: single-system → `HostSessionBase<T,S>`, multi-system or codec-backed → `MultiSystemHostSessionBase<S>`.
2. Apply migration pattern from Steps 4/5.
3. Run tests after each batch.
4. Track line-count reduction (measured, not assumed).

### Verification

```bash
dotnet build Ashfall.csproj
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
```

### Done when

Every host session file confirmed to exist in `src/Host/` at the time this step runs inherits from a base class. Zero sessions manually implement ad-hoc dirty-flag/save-load forwarding outside the base class contract. Full verification green.

---

## Step 7 — Write Host Session Contract Tests (Verify Base Class Enforces Save/Load and Dirty Flags)

### Goal

Write a test suite that mechanically verifies the host session contract: every session correctly captures state, restores state, manages dirty flags, and integrates with its `SaveStore`.

### Implementation

**Correction (carries forward from Step 3):** `HostSessionBase<,>`/`MultiSystemHostSessionBase<>` live in `src/Host/` (`AtomicWar.GodotApp` namespace), which `Ashfall.Core.Tests.csproj` cannot see. `Ashfall.Core.Tests/Host/HostSessionContractTests.cs`, as originally specified, is not a buildable path for testing host-layer types. Either place these tests in whatever project/mechanism Step 3 settled on for testing the base class (a Godot-host test target, or a `godot --headless` selftest verb), or restructure so the reflection-discoverable, testable contract logic lives in `Ashfall.Core` and only the Godot-specific glue stays in `src/Host/`. Resolve this the same way Step 3 did — do not specify a test file path without confirming the containing project can compile it.

1. Create the contract test suite at whichever location Step 3 established as buildable.

2. Implement reflection-based discovery:
   - Find all non-abstract types inheriting from `HostSessionBase<,>` or `MultiSystemHostSessionBase<>`.
   - For each, verify it implements the required abstract members.

3. Write parameterized contract tests:
   - **Dirty flag contract:** Constructing a session starts clean → performing any action marks dirty → `CaptureSave()` clears dirty.
   - **Save round-trip:** `CaptureSave()` → `RestoreSave()` → `CaptureSave()` produces identical state.
   - **Null restore safety:** `RestoreSave(null)` does not throw, logs warning.
   - **Event wiring:** At least one event subscription exists (verified via `WireEvents` being called).

4. Write integration tests that exercise save/load through the `SaveStore`:
   - Serialize state via `SaveStore.TrySave()` (note: the actual method name observed on every `SaveStore` class in this review is `TrySave`/`TryLoad`, not `Save`/`TryLoad` — verify the exact method names per store rather than assuming a uniform `Save()` name), deserialize via `SaveStore.TryLoad()`.
   - Verify checksum is computed and validated.
   - Verify mutated state produces different checksum.

5. Add a sweep test (similar to `SaveStoreChecksumSweepTests`, confirmed to exist in `Ashfall.Core.Tests/` per AGENTS.md) that ensures no host session is accidentally skipped:
   - Discover all `*HostSession` classes via reflection.
   - Assert each inherits from a base class.
   - Fail if a new session is added without using the base (regression gate).

### Verification

```bash
dotnet build Ashfall.csproj                                  # Godot host: 0 errors, 0 warnings
godot --headless --path . -- --data-integrity-selftest       # Catalog integrity: 0 errors
godot --headless --path . -- --bridge-selftest               # Exits 0
```
Plus whichever `dotnet test` invocation is actually valid once Step 3's test-location question is resolved — do not copy the original plan's `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` line here without confirming it can see the host-layer types under test.

### Done when

- Contract test suite exists with 10+ tests covering dirty-flag, save/load, null safety, and event wiring, runnable via a confirmed-working command.
- Reflection-based sweep test catches any session not using the base class.
- All tests pass.
- Future host sessions that skip the base class will cause a test failure.

---

## Summary Table

| Step | Description | Tier | Est. Effort | Key Risk |
|------|-------------|------|-------------|----------|
| 1 | Audit representative host sessions (using verified file names/contents, not assumed boilerplate) | Analysis | 3-4 hours (revised up — starting assumptions were wrong, so this audit has more actual discovery work than a confirmation pass) | Missing edge cases in complex/codec-backed sessions |
| 2 | Design `HostSessionBase<TSystem, TState>` + `MultiSystemHostSessionBase<TState>` (both needed from the start, not the multi-system variant as an afterthought) | Design | 3-4 hours | Parameterized capture (e.g. `simDay`) not fitting a parameterless `CaptureStateCore()`; dirty-flag ownership move from `Main.cs` to session is a behavior change, not a pure refactor |
| 3 | Implement base class(es) with dirty flag, logging (new capability, not consolidation — see Context), save helpers, and a confirmed-buildable test location | Implementation | 4-5 hours (revised up to include resolving the test-location problem) | Thread safety edge cases; test project cannot see host-layer types without a resolved plan |
| 4 | Migrate simple (genuinely single-system) host sessions — count TBD by Step 1's real classification | Migration | 3-4 hours | Public API signature drift; `StateChanged` event removal breaking `Main.cs` subscribers |
| 5 | Migrate multi-system/codec-backed host sessions, including `HoldfastRuntimeSession`'s dual-save-store shape | Migration | 6-8 hours (revised up — this review found multi-system/codec-backed sessions to be more common than the original "5 complex" estimate implied) | Multi-system state aggregation complexity; dual-save-store sessions may not fit the base class cleanly |
| 6 | Migrate remaining sessions from the verified 27-file list (minus whatever Steps 4/5 covered) | Migration | 6-8 hours (revised down from "24 remaining" since the verified total is 27, not 34) | Session names from the original plan that don't exist in the codebase; re-audit needed at step start |
| 7 | Write contract tests (reflection sweep + parameterized), at a confirmed-buildable location | Testing | 4-5 hours | Test infrastructure for generic base; location problem carried from Step 3 |

**Total estimated effort:** 29-38 hours (revised from the original 27-35; the corrected picture has more audit/design work up front and a redistributed migration effort, not a net reduction, despite the lower total session count)
**Net outcome:** 27 host sessions (not 34) reduced to focused subclasses; the dirty-flag bit moves from ad-hoc fields on `Main.cs` into the session base class (an intentional behavior change, not a transparent refactor); save/load contract mechanically enforced across both direct-system and codec-backed forwarding shapes; new sessions get the pattern for free by inheriting from the base.

---

## Review Notes (Corrected)

This batch was adversarially reviewed against the actual codebase at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`. Corrections made:

1. **Host session file count — CORRECTED from 34 to 27 within `src/Host/`; 29 project-wide.** `ls src/Host/*HostSession.cs` (excluding `.uid` sidecars) returns exactly 27 files. The verified full `src/Host/` list is given in Step 6 above. **A re-review found this batch's own prior correction pass had made a scoping error: it treated `src/Host/`-only results as if they were project-wide.** Two of the twelve names it had flagged as "fabricated" — `SilentFoundryHostSession` (`src/Foundry/SilentFoundryHostSession.cs`) and `YearOfAshHostSession` (`src/YearOfAsh/YearOfAshHostSession.cs`) — genuinely exist, just outside `src/Host/`. A third, `DiseaseHostSession` (named in the original plan's Step 4, not Step 6), also exists at `src/Disease/DiseaseHostSession.cs` and was wrongly treated as unconfirmed. The remaining nine — `CrossingHostSession`, `WarlordDoctrineHostSession`, `DeepLoreHostSession`, `CurrentsHostSession`, `WitnessHostSession`, `DiveSiteHostSession`, `FinalWishHostSession`, `NobodysCharterHostSession`, `CoalitionHostSession`, `CensusHostSession` — were re-verified via project-wide `find . -iname "<name>.cs"` (excluding `.git`) and genuinely do not exist under those names anywhere in the repository. Lesson applied throughout this file: every "does not exist" claim now specifies whether the search was scoped to `src/Host/` or project-wide, and Step 1/Step 6 were corrected to require a project-wide search before excluding a session from scope.

2. **Core premise — the shared "boilerplate" (dirty flag + ILog field) does not exist — MOSTLY TRUE, one exception found on re-check.** Direct reading of `src/Host/CombatHostSession.cs`, `src/Host/DoseLedgerHostSession.cs`, and `src/Host/HoldfastRuntimeSession.cs`, plus a directory-wide grep for `private bool _dirty;` and `MarkDirty()` across all of `src/Host/*.cs`, found **zero matches** for either pattern in any host session file — that part of the original correction holds. **However, the "no `ILog` field" claim was too broad and is corrected here: `grep -n "ILog _log\|ILog Log\|readonly ILog" src/Host/*HostSession.cs` finds two exceptions — `DutyRosterHostSession.cs:78` (`private readonly ILog _log;`, defaulted via `log ?? NullLog.Instance`) and `ShelterAssignmentHostSession.cs:82` (`private static readonly ILog s_log = new GodotLog();`).** So 2 of 27 sessions already have a logger field, in two different shapes (instance field with DI default vs. static field with a hardcoded concrete type) — the base class design must decide whether these two get migrated to the new base-class logging surface (a behavior-preserving consolidation for these two) or coexist with it (risking two logging paths on the same class hierarchy). The actual pattern for dirty tracking is otherwise as previously found: Core systems raise their own `OnStateChanged`-style events; host sessions forward these to a single public `StateChanged` event (or similar) on the session; the dirty-flag *bool* itself is a private field declared directly on `Main.cs` (25 such fields confirmed by grep, e.g. `_yearOfAshDirty`, `_phase0Dirty`, `_dailyBriefingDirty`, `_medicalWardDirty`, `_memorialDirty`, `_encounterChoiceDirty`, `_doseLedgerDirty`, `_verdictDirty`, `_maritimeDirty`, `_expeditionDirty`, `_combatDirty`, `_narrativeDirty`, `_medicalDirty`, `_worldDirty`, `_craftingDirty`, `_caravansDirty`, `_economyDirty`, `_journalDirty`, `_holdfastDirty`, `_dutyRosterDirty`, `_expansionHubDirty`, `_foundryDirty`, `_startingLevelDirty`, `_powerGridDirty`, `_greenhouseDirty`), set inside a lambda subscribed to the session's event, and cleared by a `FlushXxxIfDirty()` method also on `Main.cs`. This means the batch's core deliverable — a base class that "mechanically enforces" a pattern that supposedly already exists per-session — is actually proposing to *move* dirty-flag ownership from the host into the session, which is a real architectural change with real behavioral risk, not a boilerplate-elimination refactor. Every step referencing `_dirty`/`MarkDirty()`/`ILog` removal was rewritten to describe this correctly and to flag the ownership-move as an explicit design decision requiring sign-off, not an assumed given.

3. **`CaptureSave()`/`RestoreSave()` signature uniformity — FALSE, CORRECTED.** The original plan's `HostSessionBase<TSystem, TState>` sketch assumes every session forwards to exactly one Core system's parameterless `CaptureState()`/`RestoreState(state)`. Verified counter-example: `DoseLedgerHostSession.CaptureSave(int simDay)` forwards to a **static codec** (`DoseLedgerSaveCodec.Capture(simDay, Ledger, SickList, Cohort, Voluntary, Quests)`) wrapping **four** Core systems, and requires an extra `simDay` parameter the base class's sketched `TState CaptureStateCore()` signature cannot accept. `HoldfastRuntimeSession` is a further counter-example: it persists through **two separate save stores** (`HoldfastSaveStore` and `HoldfastTradeSaveStore`), not one `TState`. Steps 2, 4, and 5 were rewritten to require resolving this before implementation, and to expect the multi-system/codec-backed shape to be the common case, not the "complex" exception the original plan treated it as.

4. **Unrunnable verification command — Step 3's and Step 7's `Ashfall.Core.Tests` test location, FIXED.** `HostSessionBase<TSystem, TState>` is specified (by the plan's own code sketch) to live in `src/` under the `AtomicWar.GodotApp` namespace — the Godot host project. `Ashfall.Core.Tests.csproj` (confirmed via AGENTS.md and this review's reading of test file paths) only compiles against `Ashfall.Core`; it cannot reference or test Godot-host types. The original plan's `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "HostSessionBase"` (Step 3) and the proposed file `Ashfall.Core.Tests/Host/HostSessionContractTests.cs` (Step 7) are not buildable as specified. Both steps were corrected to require resolving where these tests can actually live (either move testable logic into Core, or use the Godot-host self-test pattern already established by `HostCli.SelfTests.cs`/`HostCli.PanelTests.cs`) before writing Done-when criteria that assume a specific command works.

5. **Save store method naming — clarified.** Every `SaveStore` class read during this review (`DutyRosterSaveStore`, `HoldfastSaveStore`, `CombatSaveStore`, etc.) exposes `TrySave`/`TryLoad`, not a bare `Save`/`Load`. Step 7's integration-test description was corrected to use the real method names.

6. **Missing risk/rollback — added throughout.** The dirty-flag ownership move (Correction 2) and the `StateChanged` event's fate (kept for `Main.cs` consumers, or replaced by the base class's own dirty-tracking surface — Step 4 now flags this explicitly) are the two biggest risk items, since both can silently break `Main.cs` compilation or change when UI refresh actually happens. `HoldfastRuntimeSession`'s dual-save-store shape is called out in Step 5 as a case that may need to remain a documented exception rather than be forced into the base class.

7. **Ordering is logically sound but effort estimates were revised.** Step 1 (audit) → Step 2 (design) → Step 3 (implement) → Steps 4–6 (migrate) → Step 7 (contract tests) remains the correct sequence. However, because the starting assumptions were wrong (boilerplate that doesn't exist, a "5 simple / 5 complex / 24 remaining" split that doesn't match the real single-system-vs-multi-system-vs-codec-backed distribution), the effort table was revised: total estimated effort moved from 27-35 hours to 29-38 hours despite the corrected session count being lower (27 vs. 34) — the audit and design phases need more real discovery work, and more sessions than assumed will need the harder multi-system path.

8. **Second-pass correction (this review): the prior "corrected" draft itself contained two errors, now fixed above** — the false-universal "no session has an `ILog` field" claim (2 exceptions exist: `DutyRosterHostSession`, `ShelterAssignmentHostSession`) and the `src/Host/`-scoped "does not exist" claims for `SilentFoundryHostSession`, `YearOfAshHostSession`, and `DiseaseHostSession` (all three exist outside `src/Host/`). This is flagged explicitly because a plan that corrects itself once and is then trusted without a second independent check can still ship wrong migration targets — Step 1's audit is the actual authority, not this Review Notes section, and Step 1 must re-derive its own list from a fresh project-wide search rather than copying the list above.

### Risk & Rollback (added — previously missing)

- **Primary risk: the dirty-flag ownership move (host → session) is a behavior change, not a pure refactor.** If a session's `MarkDirty()`-equivalent fires at a different point in the event chain than the current `Main.cs` lambda does (e.g. missing an event subscription that the lambda currently has, or firing before a related field is fully updated), a save could silently miss a mutation. This is the single highest-risk item in the batch and the reason its overall Risk is rated Medium, not Low.
- **Rollback plan per step:**
  - Steps 1–3 (audit/design/implement) touch no existing call sites — trivially revertible by deleting the new `src/Host/HostSessionBase.cs` file(s) and any new test files.
  - Steps 4–6 (migration): migrate and verify **one session at a time**, commit after each, and keep the pre-migration session class diffable in version control. If a migrated session's selftest or `SaveStoreChecksumSweepTests`-style round-trip test fails, `git revert` that single session's commit rather than the batch — this is why "one system per task" (per AGENTS.md's Git Rules) matters more here than usual, since a bad migration that silently drops a `StateChanged` subscription may not fail loudly.
  - Step 7 (contract tests): if the reflection-based sweep test starts failing for a session added by unrelated concurrent work, that is a signal the base class contract is being bypassed — treat as a merge conflict to resolve, not a reason to weaken the sweep test's assertion.
- **Specific regression to watch for:** any place in `src/Main.cs` that subscribes to a session's public `StateChanged` event (this is how the 25 `*Dirty` fields get set today). If a migrated session's `WireEvents()` override stops raising `StateChanged` (because dirty tracking moved fully into the base class and the public event was removed), `Main.cs` will either fail to compile (best case — caught by `dotnet build Ashfall.csproj`) or, if `Main.cs` isn't updated in the same change, silently stop updating its own dirty bit (worst case — a save that should include fresh state doesn't). Step 4 already flags this; it is elevated here because Step 6's "migrate the remaining sessions" work is exactly where this kind of miss becomes likely once the pattern feels routine.
