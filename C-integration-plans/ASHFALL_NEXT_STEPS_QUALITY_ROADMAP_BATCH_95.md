# ASHFALL — Quality Roadmap Batch 95

## Theme: Exception Safety & Graceful Degradation — No Single System Crash Kills the Game

| Field | Value |
|-------|-------|
| **Priority** | HIGH |
| **Risk** | Medium — changes error flow in the critical simulation path |
| **Blocking** | Playtesting stability, user confidence, save integrity |
| **Enables** | Resilient gameplay sessions, partial-save recovery, better error diagnostics |
| **Systems touched** | `TickSimDay` orchestrator (~23 top-level ticks, `src/Main.cs:1643`), `SaveAll` (29 store calls, `src/Main.cs:6227`), new `SystemGuard` in Core |
| **Estimated scope** | ~800 LOC Core + ~200 LOC Godot host + ~400 LOC tests |

---

## Motivation

**[CORRECTED — see Review Notes]** `TickSimDay` (`src/Main.cs:1643-1770`) advances **~23 top-level orchestration steps** sequentially (not "31+" — that figure was an unverified guess; the real count, cross-referenced against wave-3 findings, is the sequence enumerated in Step 3 below). Several of those steps are conditional (`Maritime` dive tick, `HoldfastRuntime`, `YearOfAsh` gated to days 180-360, `Muster` gated to day ≥260), so the *executed* step count on any given day is often lower than 23. If **any** ticked system throws an unhandled exception today, `TickSimDay` has **zero try/catch anywhere in its body** — the entire day advance fails and the game freezes or crashes to desktop. Similarly, `SaveAll()` (`src/Main.cs:6227-6244`) calls exactly **29 save methods** sequentially with no guarding around the sequence itself — if one store throws, subsequent stores are skipped and the player loses data from all downstream systems.

**Important existing-code correction:** this is not a greenfield problem. Three individual save/load methods elsewhere in `src/Main.cs` (`SaveMedicalWard`/`LoadMedicalWard`, `SaveMemorial`/`LoadMemorial`, `LoadDailyBriefing`, around lines 3779-3909) **already** wrap their own I/O in local `try/catch (Exception e) { GD.PushWarning(...) }` blocks. This plan does not need to invent the *concept* of catching exceptions around a save call from scratch — it needs to (a) generalize that ad hoc, per-store pattern into the reusable `SystemGuard` abstraction, and (b) extend guarding to the 26 other `SaveXxx` calls in `SaveAll` that currently have no protection at all, plus every tick call inside `TickSimDay`, none of which are guarded today.

Current failure modes (illustrative — exact exception types/sites are examples, not confirmed incidents):
- One malformed JSON entry in expedition data → an unhandled exception in `_expeditions.TickDemoHours(24f)` (`src/Main.cs:1653`) → the entire `TickSimDay` call aborts, so every step after it (crafting, maritime, verdict, duty roster, disease, greenhouse, phase0, `SaveAll`) never runs that day
- One arithmetic fault in radiation/dose math inside `_phase0.TickDay(day)` → the same total-abort behavior, and it happens near the *end* of the 23-step sequence, so the most work is lost
- One I/O hiccup during save → an unhandled exception in any of the first ~26 unguarded calls inside `SaveAll()` (e.g. `SaveInventory()`) → every call after it in that 29-call sequence never runs, silently dropping state for systems ticked later in the list (`SaveGreenhouse`, `SaveRadio`, `SavePowerGrid`, `SaveMedicalWard`, `SaveMemorial`, etc.)

Three save/load pairs (`MedicalWard`, `Memorial`, `DailyBriefing` load) already catch their own exceptions locally and log a warning instead of crashing — proof the mitigation is directionally correct, just not applied consistently or centrally.

The game should **degrade gracefully**: isolate the broken system, log the exception with full context, continue ticking other systems, and warn the player. In development mode, exceptions should still surface loudly to catch bugs early.

**Rollout posture:** because `TickSimDay` and `SaveAll` are the single most-executed code paths in the game (every day advance, every save), this plan does NOT ship as a single big-bang change that guards all 23 tick steps and all 29 save calls in one commit. See the phased rollout in "Risk & Rollback" below — guarding is introduced behind a kill switch, proven on save first (lower blast radius, easier to verify via checksum diffing), then extended to ticks only after the save path has run in production-shaped testing without regressions.

---

## Step 1 — Design SystemGuard Pattern

### Goal
Define an engine-agnostic guard abstraction that wraps individual system tick/save calls, catches exceptions, records failures, and allows the simulation to continue.

### Implementation

`Assets/Ashfall.Core/Resilience/SystemGuard.cs`:

```csharp
namespace Ashfall.Core.Resilience;

public enum SystemHealth
{
    Healthy,
    Degraded,    // threw last tick, will retry next tick
    Faulted      // threw N consecutive ticks, suspended until manual retry
}

public sealed class SystemGuard
{
    private readonly string _systemName;
    private readonly ILog _log;
    private int _consecutiveFailures;

    public string SystemName => _systemName;
    public SystemHealth Health { get; private set; } = SystemHealth.Healthy;
    public Exception LastException { get; private set; }
    public int ConsecutiveFailures => _consecutiveFailures;

    public const int FaultThreshold = 3; // 3 consecutive failures → Faulted

    public SystemGuard(string systemName, ILog log)
    {
        _systemName = systemName;
        _log = log;
    }

    /// <summary>
    /// Executes the action within a guard. Returns true if action succeeded.
    /// </summary>
    public bool Execute(Action action)
    {
        if (Health == SystemHealth.Faulted)
            return false; // suspended — skip until reset

        try
        {
            action();
            if (Health == SystemHealth.Degraded)
            {
                _log.Info($"[SystemGuard] {_systemName} recovered after {_consecutiveFailures} failures.");
            }
            _consecutiveFailures = 0;
            Health = SystemHealth.Healthy;
            return true;
        }
        catch (Exception ex)
        {
            _consecutiveFailures++;
            LastException = ex;

            if (_consecutiveFailures >= FaultThreshold)
            {
                Health = SystemHealth.Faulted;
                _log.Error($"[SystemGuard] {_systemName} FAULTED after {_consecutiveFailures} consecutive failures. " +
                           $"Suspended until reset. Last exception: {ex.Message}");
            }
            else
            {
                Health = SystemHealth.Degraded;
                _log.Warn($"[SystemGuard] {_systemName} degraded (failure {_consecutiveFailures}/{FaultThreshold}): {ex.Message}");
            }
            return false;
        }
    }

    public void Reset()
    {
        _consecutiveFailures = 0;
        Health = SystemHealth.Healthy;
        LastException = null;
    }
}
```

Design decisions:
- **3-strike rule:** A system that fails 3 consecutive ticks is suspended (Faulted) to prevent infinite error spam. It recovers automatically if a tick succeeds.
- **No swallowing in strict mode:** Handled in Step 6 via a separate flag.
- **Engine-agnostic:** Pure C#, no Godot/Unity references. Lives in Core.
- **Composable:** Each system gets its own guard instance, enabling per-system health tracking.

### Verification
- `SystemGuard` compiles with zero engine references
- Unit tests cover: healthy execution, single failure → Degraded, 3 failures → Faulted, recovery after success, Reset()
- No `catch { }` bare blocks — all exceptions are logged with context

### Done-when
- `SystemGuard.cs` exists in `Assets/Ashfall.Core/Resilience/`
- `SystemHealth` enum with 3 states
- 5+ unit tests covering all state transitions
- XML doc comments on public API

---

## Step 2 — Implement SystemGuardRegistry

### Goal
Centralize health tracking for all guarded systems, providing a single query point for UI, diagnostics, and save-time decisions.

### Implementation

`Assets/Ashfall.Core/Resilience/SystemGuardRegistry.cs`:

```csharp
namespace Ashfall.Core.Resilience;

public sealed class SystemGuardRegistry
{
    private readonly Dictionary<string, SystemGuard> _guards = new();
    private readonly ILog _log;

    public SystemGuardRegistry(ILog log)
    {
        _log = log;
    }

    public SystemGuard Register(string systemName)
    {
        if (_guards.ContainsKey(systemName))
            throw new InvalidOperationException($"System '{systemName}' already registered.");

        var guard = new SystemGuard(systemName, _log);
        _guards[systemName] = guard;
        return guard;
    }

    public SystemGuard Get(string systemName)
    {
        return _guards.TryGetValue(systemName, out var guard) ? guard : null;
    }

    public IReadOnlyList<SystemGuard> GetAll() =>
        _guards.Values.ToList();

    public IReadOnlyList<SystemGuard> GetDegraded() =>
        _guards.Values.Where(g => g.Health == SystemHealth.Degraded).ToList();

    public IReadOnlyList<SystemGuard> GetFaulted() =>
        _guards.Values.Where(g => g.Health == SystemHealth.Faulted).ToList();

    public bool AnyDegraded => _guards.Values.Any(g => g.Health != SystemHealth.Healthy);

    public int HealthyCount => _guards.Values.Count(g => g.Health == SystemHealth.Healthy);
    public int DegradedCount => _guards.Values.Count(g => g.Health == SystemHealth.Degraded);
    public int FaultedCount => _guards.Values.Count(g => g.Health == SystemHealth.Faulted);

    public void ResetAll()
    {
        foreach (var guard in _guards.Values)
            guard.Reset();
    }

    public void ResetSystem(string systemName)
    {
        if (_guards.TryGetValue(systemName, out var guard))
            guard.Reset();
    }

    /// <summary>
    /// Snapshot for diagnostics / save metadata.
    /// </summary>
    public SystemHealthReport GetReport() => new SystemHealthReport
    {
        TotalSystems = _guards.Count,
        Healthy = HealthyCount,
        Degraded = GetDegraded().Select(g => g.SystemName).ToList(),
        Faulted = GetFaulted().Select(g => new FaultEntry
        {
            SystemName = g.SystemName,
            ConsecutiveFailures = g.ConsecutiveFailures,
            LastError = g.LastException?.Message
        }).ToList()
    };
}

[Serializable]
public sealed class SystemHealthReport
{
    public int TotalSystems { get; set; }
    public int Healthy { get; set; }
    public List<string> Degraded { get; set; } = new();
    public List<FaultEntry> Faulted { get; set; } = new();
}

[Serializable]
public sealed class FaultEntry
{
    public string SystemName { get; set; }
    public int ConsecutiveFailures { get; set; }
    public string LastError { get; set; }
}
```

### Verification
- Registry correctly tracks multiple systems with different health states
- `GetReport()` produces accurate snapshot
- Duplicate registration throws
- `ResetAll()` and `ResetSystem()` correctly transition guards back to Healthy

### Done-when
- `SystemGuardRegistry.cs` exists in `Assets/Ashfall.Core/Resilience/`
- `SystemHealthReport` DTO is serializable (for save metadata / diagnostics)
- 6+ unit tests covering registration, queries, reset, and report generation
- No engine dependencies

---

## Step 3 — Wrap All System Ticks in TickSimDay with Guards

### Goal
Retrofit the existing `TickSimDay` orchestration (`src/Main.cs:1643-1770`) so that each top-level system step is individually guarded — a failure in one system does not prevent other systems from advancing.

### Real current order (verified against `src/Main.cs:1643-1770`, wave-3 cross-referenced)

`TickSimDay` is NOT "31+ systems." It is **23 top-level orchestration steps**, several of them conditional on day number or runtime state:

| # | Step | Call | Conditional? |
|---|------|------|---------------|
| 1 | World | `_world.TickDemo(24f)` | No |
| 2 | Caravans | `_caravans.TickDemo()` | No |
| 3 | Medical | `_medical.TickDemo(24f)` | No |
| 4 | Expeditions | `_expeditions.TickDemoHours(24f)` | No |
| 5 | Hatch-return bridge | `_dutyRoster.BridgeHatchReturn(...)` | Yes — only if an expedition completed |
| 6 | Crafting | `_crafting.CompleteAll(24f)` | No |
| 7 | Maritime dive | `_maritime.TickDiveDemo(60f)` | Yes — only if `_maritime.Dive.IsActive` |
| 8 | Deep Coast | `_deepCoast.TickDaily(day, _core.Weather)` | No |
| 9 | Holdfast runtime | `_holdfastRuntime.TickDay()` | Yes — only if not null and not dead |
| 10 | Starting level | `_startingLevel.TickDay()` | No |
| 11 | Ration consumption | `_inventory.Remove(...)` (food/water) | No |
| 12 | Verdict | `TickVerdict(day, ...)` | No |
| 13 | Year of Ash | `_yearOfAsh.TickDay(day)` | Yes — only days 180-360 |
| 14 | Muster | `_muster.Escalate(day)` | Yes — only day ≥ 260 |
| 15 | Expansions (Greenhouse/Ledger/Crossing) | `_expansions.TickGreenhouse/Ledger.TickDaily/TickCrossingQuests` | Partially — greenhouse tick gated on plot count |
| 16 | Duty Roster + Ice Road sync | `_dutyRoster.TickDay(...)`, `SyncHoldfastToDuty(...)` | No |
| 17 | Silent Foundry | `_silentFoundry.Engine.TickDaily(day)` | No |
| 18 | Disease | `_disease.TickDaily(day)` | No |
| 19 | Greenhouse (shelter) | `_greenhouse.TickDay(day, ...)` | No |
| 20 | Power Grid | `TickPowerGrid(day)` | No |
| 21 | Phase 0 (10 psych/medical systems) | `_phase0.TickDay(day)` | No |
| 22 | Event adapter triggers | `_hostEventAdapter.EvaluateTriggers(...)` | No |
| 23 | HUD update + SaveAll | `UpdateHud(); SaveAll();` | No |

This table — not the original "31+ systems" estimate — is the authoritative guard-wiring checklist for this step. Note step 21 (`Phase0.TickDay`) internally advances **ten** sub-systems in one call; whether those ten get their own sub-guards or are treated as one guarded unit is an open design decision (see Risk & Rollback below) — the original plan's "31+" figure most likely came from counting Phase0's internal sub-systems as if they were top-level `TickSimDay` steps, which they are not from the orchestrator's point of view.

### Implementation

Before (actual current state, `src/Main.cs:1643`):
```csharp
private void TickSimDay(int day)
{
    SetupWorld();
    _world.TickDemo(24f);       // if this throws...
    SetupCaravans();
    _caravans.TickDemo();       // ...this never runs
    SetupMedical();
    _medical.TickDemo(24f);     // ...nor this
    SetupExpeditions();
    _expeditions.TickDemoHours(24f); // ...nor any of the remaining 19 steps, including SaveAll()
    // ... 19 more steps per the verified table above
}
```

After:
```csharp
private void TickSimDay(int day)
{
    _guardRegistry.Get("World").Execute(() => { SetupWorld(); _world.TickDemo(24f); });
    _guardRegistry.Get("Caravans").Execute(() => { SetupCaravans(); _caravans.TickDemo(); });
    _guardRegistry.Get("Medical").Execute(() => { SetupMedical(); _medical.TickDemo(24f); });
    _guardRegistry.Get("Expeditions").Execute(() => { SetupExpeditions(); _expeditions.TickDemoHours(24f); });
    // ... all 23 steps guarded, each wrapping its existing Setup+Tick call(s) unchanged

    if (_guardRegistry.AnyDegraded)
    {
        var report = _guardRegistry.GetReport();
        _log.Warn($"[TickSimDay] Day {day} completed with {report.Degraded.Count} degraded, " +
                  $"{report.Faulted.Count} faulted systems.");
        OnSystemDegradation?.Invoke(report);
    }
}

public event Action<SystemHealthReport> OnSystemDegradation;
```

Key design principles:
- **Order preserved:** Systems still tick in dependency order. A guard simply skips a faulted system.
- **No silent loss:** The `OnSystemDegradation` event notifies the host (UI) when degradation occurs.
- **Audit trail:** Every failure is logged with system name, tick count, and exception details.
- **No cascading:** If `WeatherSystem` is faulted, `RadiationSystem` (which may depend on weather) should detect missing input gracefully (null checks on weather state).

### Verification
- Inject a throwing mock system → other systems still tick
- Verify 3 consecutive failures → system marked Faulted and skipped
- Verify `OnSystemDegradation` fires with correct report
- Verify day counter still advances even with faulted systems
- Run `godot --headless --path . -- --data-integrity-selftest` and `--bridge-selftest` after wiring guards into `TickSimDay` — both self-tests exercise a real tick and must still report 0 errors / exit 0, since these are the only headless smoke tests that touch the real orchestrator end-to-end.

### Done-when
- All 23 top-level steps in the verified table above are individually guarded in `src/Main.cs` (not "31+" — use the table as the literal checklist; also decide and document whether Phase 0's 10 internal sub-systems get individual sub-guards in this step or a future one)
- Existing tests still pass (guards are transparent when no exceptions)
- New integration test: 1 broken step among the 23 → the other 22 (or their applicable conditional subset) still execute correctly, and `SaveAll()` at step 23 still runs
- `OnSystemDegradation` event exposed for host/UI consumption
- Guarding is behind the rollout flag defined below — `ASHFALL_GUARDED_TICK=1` (or equivalent) — default OFF until Step 3 has run through at least one full internal playtest pass with the flag on

### Risk & Rollback (critical-path change)

`TickSimDay` runs on every single day advance in the game — it is the highest-traffic code path that exists. Rewriting all 23 call sites in one commit is a big-bang change against the game's most critical path, which the original plan did not address. This plan instead requires a phased rollout:

1. **Phase A — additive, dark launch.** Introduce `SystemGuard`/`SystemGuardRegistry` (Steps 1-2) with zero call sites wired into `TickSimDay` yet. Ship and verify in isolation (pure Core, no host risk).
2. **Phase B — guard `SaveAll` first, not `TickSimDay`.** `SaveAll`'s 29 calls are simpler to guard safely than `TickSimDay`'s 23 steps: each `SaveXxx` call is already independent (no shared mutable state between them beyond the dirty flags), whereas several `TickSimDay` steps read state produced by earlier steps in the same call (e.g. step 5's hatch-return bridge reads `_expeditions.Engine.CaptureState()` from step 4; step 16's `SyncHoldfastToDuty` depends on step 16's own `SetupIceRoad()`). Guarding `SaveAll` first gives a lower-risk proof of the pattern and is independently verifiable via `SaveChecksum` diffing (guarded save vs. unguarded save on the same state must produce identical checksums for stores that succeed).
3. **Phase C — guard `TickSimDay` behind a flag.** Add a boolean toggle (CLI flag or environment variable, e.g. `ASHFALL_GUARDED_TICK=1`) that switches between the legacy unguarded orchestration and the guarded one. Default OFF. This lets the guarded path be exercised in CI/internal builds without changing default player-facing behavior until confidence is established.
4. **Phase D — flip the default, keep the flag as an escape hatch for at least one release** in case a guard changes observable behavior in a way that breaks a downstream system's implicit ordering assumption (see the step-5/step-16 dependency note above — a guard must not silently swallow an exception from step 4 and then let step 5 run against stale/absent captured state; guards need to make each step's precondition failures visible, not just its own exceptions).
5. **Rollback:** because the flag preserves the original unguarded code path, rollback is flipping the flag back to OFF (or reverting the flag's default) rather than a code revert. The unguarded path must not be deleted until at least one full release cycle has run with guards on by default with no regressions reported.

This phased approach also resolves an ordering hazard the original plan did not call out: naively wrapping each step in a bare `try { step(); } catch { }` risks masking a step-4 failure and then having step-5 crash anyway on missing data, or — worse — silently proceeding with stale state. Each guard's `Execute()` call must be scoped tightly enough that a caught exception doesn't leave shared fields (like `_dutyRosterDirty`, `_expansionHubDirty`, `_foundryDirty`) in an inconsistent state relative to what actually ran.

---

## Step 4 — Wrap All Save Store Writes in SaveAll with Guards

### Goal
Ensure that a failure in one save store does not prevent the remaining stores from writing. Partial save is always better than no save.

### Real current state (verified against `src/Main.cs:6227-6244`)

`SaveAll()` calls exactly **29 methods** sequentially, not "22 stores":

```
SaveJournal, SaveHoldfast, SaveHoldfastRuntime, SaveDutyRoster, SaveExpansionHub,
SavePhantomMemory, SaveDoseLedger, SaveMuster, SaveInventory, SaveSurvivors,
SaveEconomy, SaveVerdict, SaveMaritime, SaveExpeditions, SaveCombat, SaveNarrative,
SaveMedical, SaveWorld, SaveCrafting, SaveCaravans, SaveYearOfAsh, SavePhase0,
SaveStartingLevel, SaveGreenhouse, SaveRadio, SaveDailyBriefing, SavePowerGrid,
SaveMedicalWard, SaveMemorial
```

None of these 29 calls are currently guarded *as a sequence* inside `SaveAll` — but note `SaveMedicalWard` and `SaveMemorial` already contain their own internal `try/catch (Exception e) { GD.PushWarning(...) }` blocks (`src/Main.cs:~3779` and `~3818`). This means for those two stores specifically, an exception is already caught *before* it would propagate up to `SaveAll`'s sequence — so wrapping them in a `SystemGuard` as well is not fixing a live bug for those two, it's making the existing ad hoc protection consistent with the other 27 stores and giving it uniform reporting via `SystemGuardRegistry`. Implementers should decide whether to remove the now-redundant inner try/catch in those two methods once the outer guard is in place, to avoid double-catching and inconsistent logging.

### Implementation

Before (actual current state, `src/Main.cs:6227`):
```csharp
private void SaveAll()
{
    SaveJournal();       // if this throws...
    SaveHoldfast();      // ...lost
    SaveHoldfastRuntime(); // ...lost
    // ... 26 more calls, in the exact order listed above
}
```

After:
```csharp
public SaveResult SaveAll()
{
    var results = new List<StoreSaveResult>();

    void GuardedSave(string storeName, Action saveAction)
    {
        var guard = _guardRegistry.Get($"Save:{storeName}");
        var success = guard.Execute(saveAction);
        results.Add(new StoreSaveResult
        {
            StoreName = storeName,
            Success = success,
            Error = success ? null : guard.LastException?.Message
        });
    }

    GuardedSave("Journal", SaveJournal);
    GuardedSave("Holdfast", SaveHoldfast);
    GuardedSave("HoldfastRuntime", SaveHoldfastRuntime);
    // ... all 29 calls, same order as today, each wrapped individually —
    // NOT refactored into a data-driven (name, action) tuple list/loop.
    // Scope note: introducing a `_saveStores` iterable collection would be a
    // structural refactor of SaveAll beyond "wrap existing calls with guards"
    // and is explicitly OUT of scope for this step. If a future batch wants
    // to make the store list data-driven, that is a separate, reviewable change.

    var saveResult = new SaveResult
    {
        TotalStores = results.Count,
        Succeeded = results.Count(r => r.Success),
        Failed = results.Where(r => !r.Success).ToList(),
        Timestamp = DateTime.UtcNow
    };

    if (saveResult.Failed.Any())
    {
        _log.Warn($"[SaveAll] Partial save: {saveResult.Succeeded}/{saveResult.TotalStores} stores succeeded. " +
                  $"Failed: {string.Join(", ", saveResult.Failed.Select(f => f.StoreName))}");
        OnPartialSave?.Invoke(saveResult);
    }

    return saveResult;
}

public event Action<SaveResult> OnPartialSave;

[Serializable]
public sealed class SaveResult
{
    public int TotalStores { get; set; }
    public int Succeeded { get; set; }
    public List<StoreSaveResult> Failed { get; set; } = new();
    public DateTime Timestamp { get; set; }
}

[Serializable]
public sealed class StoreSaveResult
{
    public string StoreName { get; set; }
    public bool Success { get; set; }
    public string Error { get; set; }
}
```

Key decisions:
- Save guards use a separate prefix `"Save:{name}"` in the registry to distinguish from tick guards.
- Partial save result is returned to the caller so UI can warn the player.
- Checksum is computed only for stores that succeeded (corrupt stores don't participate).
- On load, missing store data for a faulted store is handled by the existing legacy fallback path.
- `SaveAll()`'s return type changes from `void` to `SaveResult` — check the one existing call site (`OnExitGameClicked` at `src/Main.cs:2965`, currently `SaveAll(); GetTree().Quit();`) still compiles; it can simply discard the result.

### Verification
- Inject throwing mock store → other 28 stores still write successfully
- `SaveResult` correctly reports which stores failed
- `OnPartialSave` event fires with accurate failure list
- Checksum integrity maintained for stores that did write
- `SaveMedicalWard`/`SaveMemorial`'s existing internal try/catch does not double-report a failure through both the inner `GD.PushWarning` and the outer guard in a confusing way — decide and document the intended behavior (inner catch swallows before guard sees it, so the guard will report these two as "always succeeded" unless the inner catches are removed; this is a real gotcha, not a hypothetical)

### Done-when
- All 29 save calls in `SaveAll` are wrapped with individual guards, in the exact order verified above (not "22 stores")
- Partial save test: 1 broken store → 28 succeed → `SaveResult.Succeeded == 28`
- Event fires with correct failure metadata
- Existing save round-trip tests still pass (guards are transparent on success)
- Per the Risk & Rollback plan in Step 3: this step ships and is verified (including at least one internal playtest session with induced failures) *before* Step 3's `TickSimDay` guards are enabled by default
- The one call site of `SaveAll()` (`OnExitGameClicked`) is updated to compile against the new `SaveResult` return type

---

## Step 5 — Add Degraded-System UI Indicator

### Goal
Surface system health to the player in a non-intrusive way. The player should know that something is wrong (and which system is affected) without being overwhelmed by technical details.

### Implementation

New Godot UI component: `src/UI/SystemHealth/SystemHealthIndicator.tscn`

```
SystemHealthIndicator (Control, anchored to HUD corner)
├── HealthIcon (TextureRect — green checkmark / yellow warning / red X)
├── DegradedBanner (PanelContainer, hidden by default)
│   ├── WarningIcon (TextureRect)
│   ├── MessageLabel (Label: "Weather system experiencing issues")
│   └── DismissButton (Button: "X")
└── DetailPanel (expandable, hidden by default)
    ├── SystemList (VBoxContainer)
    │   └── SystemRow (per degraded/faulted system)
    │       ├── StatusIcon (color-coded)
    │       ├── SystemName (Label)
    │       └── RetryButton (Button, only for Faulted)
    └── HealthSummary (Label: "21/23 systems healthy")
```

Behavior:
- **All healthy:** Small green indicator in corner, no banner
- **Degraded (1-2 systems):** Yellow indicator + brief banner notification (auto-dismiss after 5s)
- **Faulted (any system):** Red indicator + persistent banner until player acknowledges
- **Detail panel:** Click indicator to expand full system health list
- **Retry button:** Resets a faulted system guard, allowing it to attempt ticking again

Host wiring in `src/Main.cs`:

Note: `SystemGuardRegistry.Get(string)` looks up one named guard (see Step 2's API) — there is no wildcard `"*"` lookup and no broadcast event on the registry itself. `OnSystemDegradation` is raised once per `TickSimDay` call (defined in Step 3 on the host, not on the registry), so wire the UI to that event directly:

```csharp
// Subscribed once during host setup, alongside the other event wiring in Main.cs:
OnSystemDegradation += report => _systemHealthIndicator.UpdateHealth(report);
```

Player-facing messages (no technical jargon) — using the real guard names from the Step 3 table, not placeholder system names:
| Internal guard name | Player message |
|---|---|
| `World` Degraded | "Weather forecasting experiencing interference" |
| `DutyRoster` Faulted | "Duty roster tracking temporarily offline" |
| `Expeditions` Degraded | "Expedition communications disrupted" |
| `Medical` Faulted | "Medical monitoring equipment malfunction" |

This table must be filled in for all 23 guard names before Step 5 ships — the four rows above are illustrative examples, not the complete mapping.

### Verification
- Indicator shows green when all systems healthy
- Indicator transitions to yellow/red on degradation event
- Banner text is player-friendly (no exception messages)
- Detail panel correctly lists affected systems
- Retry button resets guard and system resumes ticking

### Done-when
- `.tscn` and `.cs` files exist in `src/UI/SystemHealth/`
- Indicator wired to `SystemGuardRegistry` events
- All 3 health states (Healthy/Degraded/Faulted) have distinct visual presentation
- Player-facing strings use in-fiction language (not "NullReferenceException")
- Retry functionality works end-to-end

---

## Step 6 — Add `--strict-mode` Flag for Development

### Goal
In development/CI, exceptions should **not** be swallowed — they should crash immediately so bugs are caught early. The graceful degradation is for player-facing builds only.

### Implementation

Add a `StrictMode` configuration to the guard system:

```csharp
// In SystemGuard.cs
public sealed class SystemGuard
{
    public static bool StrictMode { get; set; } = false;

    public bool Execute(Action action)
    {
        if (Health == SystemHealth.Faulted && !StrictMode)
            return false;

        try
        {
            action();
            // ... recovery logic
            return true;
        }
        catch (Exception ex) when (!StrictMode)
        {
            // ... degradation logic (only catches when NOT strict)
            return false;
        }
        // In strict mode: exception propagates unhandled → crash with full stack trace
    }
}
```

CLI activation — **must go through the existing `src/Host/HostCli.cs` parser**, not raw `Args.Contains(...)` inline in `Main.cs`. Every other self-test flag in this codebase (`--bridge-selftest`, `--data-integrity-selftest`, etc.) is parsed via `HostCliAction` enum values added to `HostCli.cs` (see `src/Host/HostCli.cs:203-219` for the existing pattern: `if (Has(args, "--bridge-selftest")) return HostCliAction.BridgeSelfTest;`). `--strict-mode` should be added the same way — either as its own `HostCliAction` or (more likely, since it's a modifier rather than a distinct action) as a flag checked alongside action parsing, consistent with how the codebase already structures CLI arguments:

```bash
# Development: crash on any system exception (default for CI)
godot --headless --path . -- --strict-mode --data-integrity-selftest

# Player build: graceful degradation (default)
godot --path .
```

Environment detection:
```csharp
// In Godot host startup (src/Main.cs or src/Host/HostCli.cs, following existing
// argument-parsing conventions in that file)
SystemGuard.StrictMode = HasStrictModeFlag(args)
    || Environment.GetEnvironmentVariable("ASHFALL_STRICT") == "1"
    || Godot.OS.IsDebugBuild();  // real Godot 4 C# API; NOT currently used
                                  // anywhere in this codebase — this would be
                                  // its first call site, so confirm during
                                  // implementation that IsDebugBuild() reflects
                                  // the intended debug/release distinction for
                                  // this project's export presets before relying on it
```

### Verification
- `--strict-mode` causes immediate crash on system exception (stack trace in stderr)
- Without `--strict-mode`, exceptions are caught and systems degrade gracefully
- CI pipeline uses strict mode (all self-tests run strict)
- Debug builds default to strict; release builds default to graceful
- `Godot.OS.IsDebugBuild()` correctly detects Godot debug vs release for this project's actual export presets — verify this manually since the API has no existing call site in this codebase to confirm against
- **Caveat, corrected:** because `--data-integrity-selftest` and `--bridge-selftest` don't throw exceptions today, running them with `--strict-mode` produces the exact same PASS result as without it — strict mode is not exercised by the existing self-test suite at all. The "Existing CI self-tests pass with strict mode active" verification bullet is true but vacuous; it does not prove strict-mode's catch-vs-rethrow branching actually works. That must be proven by the new unit test (`StrictMode_RethrowsException`) in Step 7, not by the self-test gates.

### Done-when
- `StrictMode` static property on `SystemGuard`
- `--strict-mode` CLI flag parsed via `src/Host/HostCli.cs`, consistent with the existing `HostCliAction` pattern (not an ad hoc `Args.Contains` check bypassing that module)
- `ASHFALL_STRICT` environment variable supported (for CI)
- Debug builds auto-enable strict mode via `Godot.OS.IsDebugBuild()`, manually verified against both a debug and a release export of this project
- Existing CI self-tests still exit 0 with strict mode active (expected to be unchanged, since they don't throw — this is a non-regression check, not proof strict mode works)
- New test: strict mode + throwing system → exception propagates (this is the test that actually proves Step 6, per the caveat above)

---

## Step 7 — Write Exception Safety Tests

### Goal
Comprehensive test suite proving that the guard system works correctly: exceptions are caught, systems degrade, partial saves succeed, strict mode rethrows, and recovery works.

### Implementation

`Ashfall.Core.Tests/ResilienceTests.cs`:

```csharp
namespace Ashfall.Core.Tests;

public class SystemGuardTests
{
    [Fact]
    public void HealthyExecution_ReturnsTrue_RemainsHealthy()
    {
        var guard = new SystemGuard("TestSystem", new NullLog());
        var result = guard.Execute(() => { /* no-op */ });
        Assert.True(result);
        Assert.Equal(SystemHealth.Healthy, guard.Health);
    }

    [Fact]
    public void SingleFailure_ReturnsFalse_MarksDegraded()
    {
        var guard = new SystemGuard("TestSystem", new NullLog());
        var result = guard.Execute(() => throw new InvalidOperationException("test"));
        Assert.False(result);
        Assert.Equal(SystemHealth.Degraded, guard.Health);
        Assert.Equal(1, guard.ConsecutiveFailures);
    }

    [Fact]
    public void ThreeConsecutiveFailures_MarksFaulted()
    {
        var guard = new SystemGuard("TestSystem", new NullLog());
        for (int i = 0; i < 3; i++)
            guard.Execute(() => throw new Exception($"fail {i}"));
        Assert.Equal(SystemHealth.Faulted, guard.Health);
    }

    [Fact]
    public void FaultedSystem_SkippedOnExecute()
    {
        var guard = new SystemGuard("TestSystem", new NullLog());
        for (int i = 0; i < 3; i++)
            guard.Execute(() => throw new Exception());

        var callCount = 0;
        guard.Execute(() => callCount++);
        Assert.Equal(0, callCount); // action never invoked
    }

    [Fact]
    public void Recovery_AfterDegraded_SuccessResetsToHealthy()
    {
        var guard = new SystemGuard("TestSystem", new NullLog());
        guard.Execute(() => throw new Exception());
        Assert.Equal(SystemHealth.Degraded, guard.Health);

        guard.Execute(() => { }); // succeeds
        Assert.Equal(SystemHealth.Healthy, guard.Health);
        Assert.Equal(0, guard.ConsecutiveFailures);
    }

    [Fact]
    public void Reset_RestoresHealthFromFaulted()
    {
        var guard = new SystemGuard("TestSystem", new NullLog());
        for (int i = 0; i < 3; i++)
            guard.Execute(() => throw new Exception());
        guard.Reset();
        Assert.Equal(SystemHealth.Healthy, guard.Health);
    }

    [Fact]
    public void StrictMode_RethrowsException()
    {
        SystemGuard.StrictMode = true;
        try
        {
            var guard = new SystemGuard("TestSystem", new NullLog());
            Assert.Throws<InvalidOperationException>(() =>
                guard.Execute(() => throw new InvalidOperationException("strict!")));
        }
        finally
        {
            SystemGuard.StrictMode = false;
        }
    }
}

public class GuardedTickSimDayTests
{
    [Fact]
    public void OneSystemThrows_OthersStillTick()
    {
        var registry = new SystemGuardRegistry(new NullLog());
        var guardA = registry.Register("SystemA");
        var guardB = registry.Register("SystemB");
        var guardC = registry.Register("SystemC");

        var tickedSystems = new List<string>();

        guardA.Execute(() => tickedSystems.Add("A"));
        guardB.Execute(() => throw new Exception("B broke"));
        guardC.Execute(() => tickedSystems.Add("C"));

        Assert.Contains("A", tickedSystems);
        Assert.DoesNotContain("B", tickedSystems);
        Assert.Contains("C", tickedSystems);
    }

    [Fact]
    public void PartialSave_FailedStoreDoesNotBlockOthers()
    {
        var registry = new SystemGuardRegistry(new NullLog());
        var saves = new List<string>();

        var g1 = registry.Register("Save:Expedition");
        var g2 = registry.Register("Save:Medical");
        var g3 = registry.Register("Save:Narrative");

        g1.Execute(() => saves.Add("Expedition"));
        g2.Execute(() => throw new IOException("disk full"));
        g3.Execute(() => saves.Add("Narrative"));

        Assert.Equal(2, saves.Count);
        Assert.Contains("Expedition", saves);
        Assert.Contains("Narrative", saves);
        Assert.Equal(SystemHealth.Degraded, g2.Health);
    }

    [Fact]
    public void HealthReport_ReflectsCurrentState()
    {
        var registry = new SystemGuardRegistry(new NullLog());
        registry.Register("Healthy1");
        var degraded = registry.Register("Degraded1");
        var faulted = registry.Register("Faulted1");

        degraded.Execute(() => throw new Exception());
        for (int i = 0; i < 3; i++)
            faulted.Execute(() => throw new Exception());

        var report = registry.GetReport();
        Assert.Equal(3, report.TotalSystems);
        Assert.Equal(1, report.Healthy);
        Assert.Single(report.Degraded);
        Assert.Single(report.Faulted);
        Assert.Equal("Faulted1", report.Faulted[0].SystemName);
    }
}
```

### Verification
- `dotnet test --filter "SystemGuardTests|GuardedTickSimDayTests"` — all pass
- Tests cover: healthy path, degradation, faulting, recovery, reset, strict mode, partial save, health report
- No flaky tests (deterministic, no timing dependencies)
- Tests use only Core types (no engine dependencies)

### Done-when
- `ResilienceTests.cs` contains 10+ tests across `SystemGuardTests` and `GuardedTickSimDayTests`
- All tests pass on `dotnet test`
- Coverage includes: all state transitions, strict mode, partial save, registry operations
- No new warnings in test build

---

## Summary Table

| Step | Deliverable | Location | Tests | Risk |
|------|-------------|----------|-------|------|
| 1 | `SystemGuard` pattern + `SystemHealth` enum | `Assets/Ashfall.Core/Resilience/` | 5+ state transition tests | None — pure Core, no call sites wired yet |
| 2 | `SystemGuardRegistry` + `SystemHealthReport` | `Assets/Ashfall.Core/Resilience/` | 6+ registry tests | None — pure Core, no call sites wired yet |
| 3 | Guard-wrapped `TickSimDay` (23 verified steps) | `src/Main.cs:1643-1770` | Integration: broken step → others still run | **Medium-High — critical path, ships behind `ASHFALL_GUARDED_TICK` flag default OFF per Risk & Rollback plan** |
| 4 | Guard-wrapped `SaveAll` (29 verified calls) + `SaveResult` | `src/Main.cs:6227-6244` | Partial save test | Medium — save integrity; ships and soaks *before* Step 3 is enabled by default |
| 5 | Degraded-system UI indicator | `src/UI/SystemHealth/` | Manual + event test | Low — UI only, inert until Step 3's event fires |
| 6 | `--strict-mode` flag + debug auto-enable | `SystemGuard` + `src/Host/HostCli.cs` | Strict rethrow test (the only test that actually exercises strict mode — see Step 6 caveat) | Low |
| 7 | Comprehensive exception safety tests | `Ashfall.Core.Tests/ResilienceTests.cs` | 10+ tests | None |

---

## Dependencies & Constraints

- **No engine coupling:** `SystemGuard`, `SystemGuardRegistry`, `SystemHealthReport`, `SaveResult` all live in `Assets/Ashfall.Core/Resilience/` with zero engine references.
- **Existing port usage:** `ILog` for all guard logging. No new ports needed.
- **Backwards compatible:** Guards are transparent when no exceptions occur. Existing tests pass unchanged.
- **No performance regression:** Guard overhead is one try/catch per system per tick — negligible vs. the system logic itself.
- **Strict mode for CI, corrected:** the existing self-tests (`--data-integrity-selftest`, `--bridge-selftest`) do not throw exceptions today, so running them with or without `--strict-mode` is observably identical. Strict mode does **not** currently give CI any additional hidden-failure detection over what already exists — it only matters once real system exceptions start occurring in practice (e.g. after Step 3 ships and a genuinely broken system trips a guard). Don't oversell this constraint as an existing safety net; it's a safety net for a future state, not a currently-exercised one.
- **Save safety:** Partial saves use existing `SaveChecksum` per-store. A missing store on load triggers the existing legacy fallback path (already handles absent data).
- **Rollout gating:** Step 3 (`TickSimDay` guards) ships behind `ASHFALL_GUARDED_TICK`, default OFF, and is not flipped to default-ON until Step 4 (`SaveAll` guards, lower risk) has shipped, soaked, and been verified via checksum diffing with no regressions. See Step 3's Risk & Rollback subsection for the full phased plan.

## Design Trade-offs

| Decision | Alternative considered | Rationale |
|---|---|---|
| 3-strike faulting | Immediate fault on first throw | Many exceptions are transient (edge-case data). Allow recovery. |
| Static `StrictMode` flag | Per-guard config | Simpler; strict/graceful is a build-level choice, not per-system. |
| Separate save guards (`Save:X`) | Reuse tick guards | Save failures are orthogonal to tick failures; separate tracking. |
| Auto-strict in debug builds | Always graceful | Developers should see crashes immediately; players should not. |
| In-fiction degradation messages | Raw exception text | Players don't benefit from `NullReferenceException`; in-fiction language maintains immersion. |
| **Phased rollout with a kill switch (`ASHFALL_GUARDED_TICK`), `SaveAll` guarded before `TickSimDay`** | Big-bang: guard all 23 tick steps and all 29 save calls in a single commit/release | `TickSimDay` is the highest-traffic path in the game and several of its steps read state produced by earlier steps in the same call (see Step 3); an unguarded rollback path removes the risk of a guard subtly changing observable behavior in production before it's been exercised. The original plan had no rollback story for this — added here because Step 3/4 are the two riskiest deliverables in this batch. |

## Exit Criteria

All 5 verification steps pass. **Corrected from the original: `godot --headless --path .` requires a `--` before host-specific arguments are recognized by Godot's own CLI parser, and self-test flags in this codebase are parsed as `HostCliAction` values inside `src/Host/HostCli.cs`, not ad hoc string checks — confirm `--strict-mode` is correctly wired through that file (Step 6) before relying on the combined invocation below:**
```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # PASS
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj     # PASS (including ResilienceTests)
3. dotnet build Ashfall.csproj                                  # PASS (0 errors, 0 warnings)
4. godot --headless --path . -- --strict-mode --data-integrity-selftest  # PASS (strict mode active; note this is a non-regression check per Step 6's caveat, not proof strict mode's catch/rethrow branching works — that's proven by ResilienceTests.StrictMode_RethrowsException)
5. godot --headless --path . -- --bridge-selftest               # PASS
```

Additionally, before Step 3's guard wiring is flipped to default-ON in a release (see Risk & Rollback), run one full manual playtest session (advance at least 10 in-game days) with `ASHFALL_GUARDED_TICK=1` and no induced failures, confirming save/load round-trips and no observable behavior change versus the unguarded path on identical input.


## Review Notes (Corrected)

This file was adversarially reviewed against the real codebase at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War` and corrected in place. Summary of what was wrong and what was fixed:

### Factual errors found and corrected

1. **`TickSimDay` step count was wrong.** The original plan guessed "31+ systems." The real method, `src/Main.cs:1643-1770`, has **23 top-level orchestration steps**, several conditional (Maritime dive, HoldfastRuntime, Year of Ash gated to days 180-360, Muster gated to day ≥260). The likely source of "31+" is counting Phase 0's ten internal sub-systems as if they were top-level `TickSimDay` steps — they are ticked via a single `_phase0.TickDay(day)` call (step 21), not as 10 separate orchestrator-level calls. The plan now includes the full verified 23-step table in Step 3 and uses it as the literal wiring checklist.
2. **`SaveAll` store count was wrong.** The original plan said "22 save stores." The real method, `src/Main.cs:6227-6244`, calls exactly **29 `SaveXxx` methods**. The plan now lists all 29 by name and corrects every derived number ("21 succeed," "28 succeed," etc.) that depended on the wrong count.
3. **`GameBootstrap` does not exist in the active codebase.** It is a Unity-legacy concept (`Assets/_Game/Core/GameBootstrap.*.cs`, read-only per this project's `AGENTS.md`). The active Godot orchestrator is `src/Main.cs`'s `TickSimDay`/`SaveAll`, a single partial class, not a `GameBootstrap`. The original plan didn't invoke the name directly but implied a generic "orchestrator" abstraction that could be mistaken for one; all references in this file now point at the real `src/Main.cs` line numbers.
4. **Guarding is not a greenfield problem.** Three save/load methods (`SaveMedicalWard`, `SaveMemorial`, `LoadDailyBriefing`, around `src/Main.cs:3779-3909`) already contain local `try/catch (Exception e) { GD.PushWarning(...) }` blocks. The plan originally implied zero existing exception handling anywhere in the critical path; it did not check for or credit this prior art. Step 4 now calls out the double-catch hazard this creates once an outer `SystemGuard` wraps `SaveMedicalWard`/`SaveMemorial` (the inner catch will swallow the exception before the guard ever sees it, making those two "always report healthy" unless the inner catches are removed).
5. **Fictional class/method names used throughout code samples.** `_weatherSystem.Tick()`, `_radiationSystem.Tick()`, `_needsSystem.Tick()`, `_expeditionSystem.Tick()`, `WeatherSystem`/`NeedsSystem`/`ExpeditionSystem`/`RadiationSystem` guard names, and a `_guardRegistry.Get("*")` wildcard API do not exist anywhere in this codebase and don't match the real call shape (`SetupXxx()` + `_xxx.TickYyy(...)` pairs owned directly by `Main.cs`, not by a `Tick()`-per-system convention). All code samples were rewritten against the actual verified call sites and the actual `SystemGuardRegistry.Get(string)` API defined in this same plan's Step 2 (single-key lookup, no wildcard).
6. **`_saveStores` tuple/loop implied an undisclosed refactor.** The original "After" sample for `SaveAll` iterated a `_saveStores` collection that doesn't exist today — `SaveAll` calls 29 named methods directly. Introducing a data-driven store list is a structural refactor beyond "add a guard around each existing call" and was silently smuggled into the plan. Corrected to wrap each of the 29 calls individually via a local `GuardedSave(name, action)` helper, explicitly scoping the tuple/loop refactor as out of scope for this batch.
7. **`OS.IsDebugBuild()` and raw `Args.Contains("--strict-mode")` don't match this codebase's conventions.** `Godot.OS.IsDebugBuild()` is a real API but has zero existing call sites here — it needs to be verified against this project's actual export presets, not assumed. CLI flags in this codebase are parsed through `src/Host/HostCli.cs`'s `HostCliAction` enum (see `--bridge-selftest`, `--data-integrity-selftest`, etc. at lines 203-219), not inline `Args.Contains` checks in `Main.cs`. Step 6 now routes through that existing parser.
8. **The `--strict-mode` verification story was circular.** `--data-integrity-selftest` and `--bridge-selftest` don't throw exceptions today, so running them with `--strict-mode` produces an identical result to running them without it — that verification bullet was true but proved nothing about strict mode's actual catch/rethrow branching. Corrected to point at the real proof (the unit test `StrictMode_RethrowsException` in Step 7) and to flag the self-test-gate check as a non-regression check only.

### Missing risk/rollback for the critical-path change (the most important gap)

The original plan wraps `TickSimDay` (every day advance) and `SaveAll` (every save) — the two highest-traffic paths in the entire game — with **no phased rollout, no feature flag, and no rollback plan**. It read as a big-bang change: rewrite all tick call sites and all save call sites, ship. Given the project's own risk rating of "Medium — changes error flow in the critical simulation path," this was a real gap, now fixed:

- Introduced a mandatory phase order: (A) ship `SystemGuard`/`SystemGuardRegistry` inert in Core with zero call sites → (B) guard `SaveAll`'s 29 calls first, since they have no inter-step data dependencies unlike `TickSimDay`'s steps → (C) guard `TickSimDay` behind a new `ASHFALL_GUARDED_TICK` kill switch, default OFF → (D) flip the default only after a full internal playtest pass, keeping the flag as a rollback path for at least one release.
- Called out a genuine ordering hazard the original plan missed entirely: several `TickSimDay` steps consume state captured by earlier steps in the same call (step 5's hatch-return bridge reads step 4's `_expeditions.Engine.CaptureState()`; step 16 depends on its own `SetupIceRoad()` call succeeding). A naive per-step try/catch can silently let a later step run against stale or missing state after an earlier step's guard swallowed an exception — this needs explicit handling, not just "wrap each call."
- Added an explicit rollback mechanism (flag flip, not code revert) and a soak requirement (one full playtest day-cycle with induced failures) before the flag's default changes.

### Other issues fixed

- Step 5's UI mockup used player-facing system names that didn't correspond to any of the real 23 guard names from Step 3's table; corrected to use real guard names and flagged that the full 23-row mapping table needs to be completed before shipping (only 4 illustrative rows existed).
- Step 5's host-wiring sample called a nonexistent registry wildcard API; corrected to subscribe to the `OnSystemDegradation` event defined on the host in Step 3.
- `SaveAll()`'s return type change (`void` → `SaveResult`) has exactly one existing call site (`OnExitGameClicked`, `src/Main.cs:2965`) — the original plan never checked or mentioned this; now called out explicitly as a Done-when item.
- All internal cross-references to stale counts ("31+", "22 stores", "21 succeed") were swept and corrected throughout Steps 3-6, the Summary Table, and Exit Criteria.
