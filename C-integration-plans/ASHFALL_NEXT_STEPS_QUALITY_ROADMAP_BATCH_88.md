# ASHFALL — Quality Roadmap Batch 88

## Theme: Cross-System Validation Framework — Invariant Checking at Runtime

**Priority:** HIGH — catches logical inconsistencies before they corrupt saves<br>
**Risk:** Low — assertions only, no state mutation<br>
**Batch:** 88<br>
**Depends on:** Core systems operational (verified: ~1,110 public types in `Assets/Ashfall.Core/`, not "82+ systems" — see Review Notes), save pipeline functional (22 stores per `AGENTS.md`, not independently re-verified in this pass)<br>
**Blocked by:** Nothing — purely additive validation layer<br>

---

## Motivation

The project has roughly 1,110 public types across ~300 files in `Assets/Ashfall.Core/` (not a clean count of "82+ systems" — see Review Notes) that interact through shared state (survivor roster, inventory, quest flags, economy ledger, expedition teams, radiation dose). Cross-system invariants exist but are only enforced *implicitly* — violations silently accumulate until they corrupt a save or produce nonsensical gameplay. **Two of the seven motivating examples below were checked against the real code and are wrong or need correction — see inline notes:**

- A survivor assigned to an expedition can simultaneously appear on the duty roster (double-booking) — **plausible, unverified**: `ExpeditionSystem.Start()` already guards `if (_active.ContainsKey(survivorId)) return false;` (one expedition per survivor), but nothing was found cross-checking expedition membership against `DutyRosterSystem` occupants. This example holds up as a real, currently-unenforced gap.
- ~~Total items in inventory can drift from the sum across all containers (phantom items)~~ — **does not apply to the current codebase.** `Ashfall.Core.Inventory.Inventory` (`Assets/Ashfall.Core/Inventory/Inventory.cs`) is a single flat `List<InventorySlot> _slots` with no sub-container concept at all — there is nothing to "sum across containers" because there is only one container. `Count()`/`CountById()`/`CountByType()` iterate the same single slot list. This invariant as worded is checking something that cannot happen because the data structure doesn't have that shape. A real inventory invariant here would instead be something like "no slot has `Amount <= 0`" or "no slot references a null/unknown `ItemDefinition`."
- ~~A dead survivor can continue accumulating radiation dose in NeedsSystem/RadiationSystem (zombie state)~~ — **already prevented by existing code.** `RadiationSystem.Tick()` (`Assets/Ashfall.Core/Radiation/RadiationSystem.cs:188`) explicitly does `if (survivor == null || !survivor.IsAlive) continue;` before any exposure/dose accumulation. `NeedsSystem.EvaluateDeath()` sets `IsDead = true; IsAlive = false;` on death, and `RadiationSystem` checks `IsAlive` per-tick. This is not an unenforced invariant to catch after the fact — it is already enforced at the point of mutation. A checker could still exist as a defense-in-depth regression guard (catching a future code change that removes the `IsAlive` check), but the motivation text should not claim this currently happens.
- Active quest flags can reference quests that don't exist in the catalog (orphan flags) — plausible, not independently verified in this pass; worth checking against `InMemoryFlagLedger` and the actual quest catalog loaders before committing to the checker design.
- Economy prices can go negative or zero (market collapse without narrative justification) — plausible; `MarketSystem` exists and its price-bounds behavior was not independently verified in this pass.
- Expedition team members can reference survivor IDs not in the master roster (ghost crew) — **confirmed real gap.** `SurvivorRosterSystem` (`Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs`) exists and is the actual roster, but no code path was found cross-checking `ExpeditionSystem`'s `survivorId` values against it (`grep` for `SurvivorRosterSystem` combined with `Expedition` found zero hits outside this plan). This is the strongest, best-supported example in the whole batch.
- Weather radiation minus shelter shielding minus personal gear can yield negative net exposure (impossible physics) — **already handled, not a gap.** `RadiationSystem.ComputeEffectiveAmbient()` and the exposure-per-hour path both clamp with `MathfCompat.Max(0f, ...)` — negative exposure cannot occur today because it's clamped at the arithmetic site, not just observable after the fact. Same caveat as the dead-survivor example: fine as a regression guard, not accurate as "a bug that currently happens."

A formal invariant-checking framework catches these at the earliest possible moment — after each tick and after each load — without mutating state or affecting performance in release builds. **Given the corrections above, this batch's real, well-supported value is the roster/expedition cross-check and the orphan-flag/economy-bounds checks — not the inventory-sum or dead-survivor-radiation checks, which duplicate protection that already exists in the mutation path.** Recommend re-scoping Step 2 to drop or rewrite the two invalidated checkers (see Step 2 corrections below) rather than implement them as originally specified.

---

## Step 1: Design IInvariantChecker Interface

### Goal
Define the contract for all cross-system invariant checkers: a non-mutating inspection that returns structured violation reports.

### Implementation

**File:** `Assets/Ashfall.Core/Validation/IInvariantChecker.cs`

```csharp
namespace Ashfall.Core.Validation
{
    /// <summary>
    /// A single cross-system invariant check. Implementations MUST NOT mutate state.
    /// </summary>
    public interface IInvariantChecker
    {
        /// <summary>Human-readable name for logging/reporting.</summary>
        string Name { get; }

        /// <summary>
        /// Severity tier. Critical violations reject save loads; warnings are logged only.
        /// </summary>
        InvariantSeverity Severity { get; }

        /// <summary>
        /// Run the check against current system state. Returns empty list if invariant holds.
        /// MUST be pure — no side effects, no state mutation.
        /// </summary>
        IReadOnlyList<InvariantViolation> Check();
    }
}
```

**File:** `Assets/Ashfall.Core/Validation/InvariantViolation.cs`

```csharp
namespace Ashfall.Core.Validation
{
    public sealed class InvariantViolation
    {
        public string CheckerName { get; }
        public InvariantSeverity Severity { get; }
        public string Message { get; }
        public string Context { get; } // e.g., "survivor_id=surv_elena, expedition=exp_northern_reach"

        public InvariantViolation(string checkerName, InvariantSeverity severity, string message, string context = "")
        {
            CheckerName = checkerName;
            Severity = severity;
            Message = message;
            Context = context;
        }
    }
}
```

**File:** `Assets/Ashfall.Core/Validation/InvariantSeverity.cs`

```csharp
namespace Ashfall.Core.Validation
{
    public enum InvariantSeverity
    {
        /// <summary>Logged but gameplay continues. Data inconsistency that self-heals or is cosmetic.</summary>
        Warning,

        /// <summary>Logged prominently. Gameplay continues but state is suspect.</summary>
        Error,

        /// <summary>Blocks save-load. State is corrupt beyond recovery without rollback.</summary>
        Critical
    }
}
```

Design constraints:
- Zero engine references (`UnityEngine`, `Godot`, etc.) — lives in `Ashfall.Core`
- `Check()` is idempotent and pure — safe to call multiple times, safe to call in tests
- Violations carry enough context for a developer to diagnose the issue from logs alone
- Severity tiers allow the runtime to decide policy (log vs. reject) without checker knowledge

### Verification
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` compiles with new types
- Interface has no engine dependencies (grep for `UnityEngine`, `Godot` — zero hits in `Validation/`)
- `Check()` signature enforces `IReadOnlyList` (immutable return, caller can't accidentally mutate)

### Done-when
- `IInvariantChecker`, `InvariantViolation`, and `InvariantSeverity` exist in `Assets/Ashfall.Core/Validation/`
- Types compile in `netstandard2.1` with zero engine coupling
- XML-doc comments explain contract (non-mutating, pure, idempotent)

---

## Step 2: Implement Five Cross-System Invariant Checkers

### Goal
Create concrete checkers for the five most impactful cross-system invariants identified in the project.

### Implementation

**Checker 1 — Survivor Uniqueness (`SurvivorAssignmentChecker.cs`)**

Invariant: A survivor can be in at most one assignment slot at a time (expedition OR duty roster OR medical bay OR idle). Never two simultaneously.

**Correction:** the four constructor dependencies below (`ISurvivorRoster`, `IExpeditionRegistry`, `IDutyRoster`, `IMedicalBay`) do not exist anywhere in the codebase — they were invented for this plan and don't map to real types. Replace with the actual concrete classes: `SurvivorRosterSystem` (`Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs`), `ExpeditionSystem` (`Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` — has an internal `_active` dictionary keyed by `survivorId`; check whether it exposes a public read accessor, or add one, before this checker can be written), `DutyRosterSystem` (`Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs`), and `MedicalWardSystem` (`Assets/Ashfall.Core/Medical/MedicalWardSystem.cs` — not `MedicalSystem`, which doesn't exist). If read-only query methods for "which survivors are currently assigned" don't already exist on these concrete classes, this checker's first sub-task is adding them — that's real, uncosted work this batch currently doesn't account for.

```csharp
public class SurvivorAssignmentChecker : IInvariantChecker
{
    public string Name => "survivor_assignment_uniqueness";
    public InvariantSeverity Severity => InvariantSeverity.Critical;

    private readonly SurvivorRosterSystem _roster;
    private readonly ExpeditionSystem _expeditions;
    private readonly DutyRosterSystem _dutyRoster;
    private readonly MedicalWardSystem _medicalBay;

    // Check: for each survivor, count how many assignment systems claim them.
    // Violation if count > 1.
    // PREREQUISITE: verify each of the four classes above exposes a public,
    // read-only way to enumerate "survivor IDs currently claimed" — if not,
    // add that accessor as part of this step, not as a silent assumption.
}
```

**Checker 2 — Inventory Slot Integrity (`InventorySlotIntegrityChecker.cs`)**

**Corrected from the original "Inventory Integrity" invariant, which does not apply to this codebase.** `Ashfall.Core.Inventory.Inventory` has no multi-container structure — it is one flat `List<InventorySlot>`. There is no `TotalCount` field to drift from a "sum across containers" because there are no separate containers to sum. The original invariant ("Total item count... must equal sum of items across all containers/slots") is checking a bug class that cannot exist in the current data model. A meaningful replacement invariant, based on what the real `Inventory` class actually looks like:

Invariant: No inventory slot has a non-positive `Amount`, and no slot references a null/unresolved `ItemDefinition`.

```csharp
public class InventorySlotIntegrityChecker : IInvariantChecker
{
    public string Name => "inventory_slot_integrity";
    public InvariantSeverity Severity => InvariantSeverity.Error;

    // Check: for each Inventory instance, for each slot in _slots (via the public
    // Slots accessor), verify slot.Amount > 0 and slot.Item != null.
    // If Capacity/MaxWeight bounds matter, also verify Slots.Count <= Capacity
    // and total weight <= MaxWeight (both fields exist on Inventory today).
}
```

**Checker 3 — Dead Survivor Regression Guard (`DeadSurvivorGuardChecker.cs`)**

**Reframed from the original "Dead Survivor Guard."** This is *not* catching a currently-unenforced bug — `RadiationSystem.Tick()` already skips dead survivors (`if (survivor == null || !survivor.IsAlive) continue;` at `Assets/Ashfall.Core/Radiation/RadiationSystem.cs:188`), and `NeedsSystem.EvaluateDeath()` is the single place `IsDead`/`IsAlive` get set. Keep this checker, but describe it honestly in code comments and the PR as a **regression guard** — it exists so that if someone removes the `IsAlive` check in `RadiationSystem.Tick()` in the future, the invariant checker catches it immediately rather than requiring a human to notice radiation numbers climbing on a dead survivor. Do not claim in commit messages or PR descriptions that this fixes an active bug; it does not.

```csharp
public class DeadSurvivorGuardChecker : IInvariantChecker
{
    public string Name => "dead_survivor_guard";
    public InvariantSeverity Severity => InvariantSeverity.Critical;

    // Check: for each dead survivor (IsDead == true on SurvivorNeedsState /
    // SurvivorRadState), verify they appear in zero active-system registrations
    // (expedition, duty roster, medical bay) AND that RadiationSystem is not
    // still tracking them in an "active exposure" state.
    // This is a regression guard for protection that already exists in
    // RadiationSystem.Tick() and NeedsSystem.EvaluateDeath() — it should never
    // fire in current code, and a test asserting that is part of Step 7.
}
```

**Checker 4 — Quest Flag Consistency (`QuestFlagConsistencyChecker.cs`)**

Invariant: Every active flag in the flag ledger must reference a quest ID that exists in the quest catalog. Orphan flags indicate state corruption or catalog desync.

```csharp
public class QuestFlagConsistencyChecker : IInvariantChecker
{
    public string Name => "quest_flag_consistency";
    public InvariantSeverity Severity => InvariantSeverity.Warning;

    // Check: for each flag with quest_ prefix, verify quest_id resolves in catalog
    // Warning only — orphan flags don't corrupt gameplay but indicate data drift
}
```

**Checker 5 — Economy Bounds (`EconomyBoundsChecker.cs`)**

Invariant: All economy prices must be positive (> 0). Trade quantities must be non-negative. Debt entries must reference valid faction IDs.

```csharp
public class EconomyBoundsChecker : IInvariantChecker
{
    public string Name => "economy_bounds";
    public InvariantSeverity Severity => InvariantSeverity.Error;

    // Check: iterate price table, verify all > 0
    // Check: iterate trade ledger quantities, verify >= 0
    // Check: iterate debt entries, verify faction_id resolves
}
```

All five checkers:
- Live in `Assets/Ashfall.Core/Validation/Checkers/`
- Accept dependencies via constructor injection (no service locator)
- Return empty list when invariant holds (common path is allocation-free if using a cached empty list)
- Include survivor/item/quest IDs in violation `Context` for diagnostics

### Verification
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — all checkers compile
- Each checker's `Check()` method touches only read-only accessors on injected dependencies
- grep confirms zero `UnityEngine`/`Godot` references in `Validation/Checkers/`

### Done-when
- Five checker classes exist in `Assets/Ashfall.Core/Validation/Checkers/`: `SurvivorAssignmentChecker`, `InventorySlotIntegrityChecker` (renamed/redesigned — see correction above), `DeadSurvivorGuardChecker` (regression guard — see correction above), `QuestFlagConsistencyChecker`, `EconomyBoundsChecker`
- Each implements `IInvariantChecker` with appropriate severity
- Each is constructor-injectable using real concrete classes (`SurvivorRosterSystem`, `ExpeditionSystem`, `DutyRosterSystem`, `MedicalWardSystem`, etc.) — not the fictional `ISurvivorRoster`/`IExpeditionRegistry`/`IDutyRoster`/`IMedicalBay` interfaces from the original draft
- No hidden dependencies; if a needed read-only query method doesn't yet exist on the target class, it is added as part of this step and called out explicitly in the PR
- No engine coupling in any checker

---

## Step 3: Create InvariantValidationSuite (Aggregator)

### Goal
A single entry point that runs all registered checkers, aggregates violations, and reports results through `ILog`.

### Implementation

**File:** `Assets/Ashfall.Core/Validation/InvariantValidationSuite.cs`

```csharp
namespace Ashfall.Core.Validation
{
    public sealed class InvariantValidationSuite
    {
        private readonly IReadOnlyList<IInvariantChecker> _checkers;
        private readonly ILog _log;

        public InvariantValidationSuite(IEnumerable<IInvariantChecker> checkers, ILog log)
        {
            _checkers = checkers.ToList().AsReadOnly();
            _log = log;
        }

        /// <summary>
        /// Run all checkers. Returns aggregated violations (empty = all invariants hold).
        /// </summary>
        public InvariantReport RunAll()
        {
            var violations = new List<InvariantViolation>();
            foreach (var checker in _checkers)
            {
                var results = checker.Check();
                violations.AddRange(results);
            }
            return new InvariantReport(violations);
        }

        /// <summary>
        /// Run all checkers and log results. Returns true if no Critical violations found.
        /// </summary>
        public bool ValidateAndLog()
        {
            var report = RunAll();
            foreach (var v in report.Violations)
            {
                switch (v.Severity)
                {
                    case InvariantSeverity.Warning:
                        _log.Warn($"[INVARIANT] {v.CheckerName}: {v.Message} ({v.Context})");
                        break;
                    case InvariantSeverity.Error:
                        _log.Error($"[INVARIANT] {v.CheckerName}: {v.Message} ({v.Context})");
                        break;
                    case InvariantSeverity.Critical:
                        _log.Error($"[INVARIANT CRITICAL] {v.CheckerName}: {v.Message} ({v.Context})");
                        break;
                }
            }
            if (report.HasCritical)
                _log.Error($"[INVARIANT] {report.CriticalCount} CRITICAL violation(s) detected — state may be corrupt");
            return !report.HasCritical;
        }
    }
}
```

**File:** `Assets/Ashfall.Core/Validation/InvariantReport.cs`

```csharp
namespace Ashfall.Core.Validation
{
    public sealed class InvariantReport
    {
        public IReadOnlyList<InvariantViolation> Violations { get; }
        public bool HasCritical => Violations.Any(v => v.Severity == InvariantSeverity.Critical);
        public bool HasErrors => Violations.Any(v => v.Severity >= InvariantSeverity.Error);
        public int CriticalCount => Violations.Count(v => v.Severity == InvariantSeverity.Critical);
        public int ErrorCount => Violations.Count(v => v.Severity >= InvariantSeverity.Error);
        public bool IsClean => Violations.Count == 0;

        public InvariantReport(IReadOnlyList<InvariantViolation> violations)
        {
            Violations = violations;
        }
    }
}
```

Design decisions:
- Suite is constructed with all checkers upfront (no runtime registration/deregistration)
- `RunAll()` is side-effect-free (returns data); `ValidateAndLog()` adds logging for convenience
- `InvariantReport` provides queryable summary without re-iterating the violation list
- Thread safety: not required (game tick is single-threaded)
- Performance: checkers run sequentially; can be parallelized later if tick budget is tight

### Verification
- `dotnet build` — suite compiles with checkers
- Unit test: construct suite with a mock checker that returns 1 violation, verify `RunAll()` aggregates it
- Unit test: construct suite with no checkers, verify `IsClean == true`

### Done-when
- `InvariantValidationSuite` and `InvariantReport` exist in `Assets/Ashfall.Core/Validation/`
- Suite accepts `IEnumerable<IInvariantChecker>` and `ILog`
- `RunAll()` returns `InvariantReport` with queryable violation counts
- `ValidateAndLog()` logs violations by severity and returns bool (false if critical)

---

## Step 4: Wire into TickSimDay (Post-Tick Validation)

### Goal
Run the invariant suite after all system ticks complete but before the save pipeline writes state to disk. Violations are logged; critical violations prevent auto-save.

### Implementation

Integration point in the Godot host's tick pipeline (`src/Main.cs` or the relevant tick orchestrator):

```csharp
// In the day-tick orchestration (after all system Tick() calls, before SaveAll)
private void TickSimDay()
{
    // ... all system ticks ...
    _weatherSystem.Tick(day);
    _needsSystem.Tick(day);
    _radiationSystem.Tick(day);
    _economySystem.Tick(day);
    _expeditionSystem.Tick(day);
    // ... remaining ticks ...

    // POST-TICK INVARIANT CHECK
    bool invariantsHold = _invariantSuite.ValidateAndLog();
    if (!invariantsHold)
    {
        _log.Error("[TICK] Critical invariant violation — auto-save suppressed for this tick");
        // Do NOT call SaveAll() — state is suspect
        return;
    }

    SaveAll();
}
```

Policy decisions:
- **Critical violation = suppress auto-save** for that tick (prevents writing corrupt state)
- **Error/Warning violations = log only** (gameplay continues, developer is notified)
- Validation runs every tick in debug/development builds; can be gated behind `#if DEBUG` or a config flag for release
- Performance budget: initial target is < 1ms per tick for all 5 checkers combined (they iterate small collections)

Conditional execution for release builds:

```csharp
#if DEBUG || INVARIANT_CHECKS
    bool invariantsHold = _invariantSuite.ValidateAndLog();
    if (!invariantsHold) { /* suppress save */ }
#endif
```

### Verification
- `dotnet build Ashfall.csproj` — host compiles with invariant wiring
- Manual test: introduce a deliberate invariant violation (assign dead survivor to expedition), verify log message appears and save is suppressed
- Performance: measure tick time with/without checkers — must be < 1ms overhead

### Done-when
- Invariant suite runs after all system ticks in the day-tick pipeline
- Critical violations suppress auto-save (state is not persisted when corrupt) — **risk/rollback note (added):** this is a real behavior change with player-facing impact — a player could lose a day's progress if a false-positive critical violation fires. Mitigate by (a) landing this behind the `#if DEBUG || INVARIANT_CHECKS` gate from day one rather than enabling it unconditionally in a release build, and (b) requiring the five checkers to run clean against the existing save-round-trip test suite before this step is considered done, not just against synthetic test fixtures. Rollback is a one-line revert (remove the call site in `TickSimDay`); the suite itself has no side effects to unwind.
- Error/Warning violations are logged but don't block gameplay
- Conditional compilation flag exists for release-build opt-out
- No engine coupling in the suite itself (host only calls `ValidateAndLog()`)

---

## Step 5: Wire into RestoreState (Post-Load Validation)

### Goal
Run the invariant suite immediately after loading a save and restoring all system state. Critical violations reject the save (rollback to previous known-good state or show error screen).

### Implementation

**Correction:** `src/Main.cs` has no single `LoadSave(string savePath)` method — load/restore is scattered across many per-subsystem methods (e.g. `LoadMedicalWard()`, `LoadMemorial()`, `LoadDailyBriefing()`, plus inline `RestoreState` calls at multiple points such as `JournalSaveStore.Load()` / `_journal.RestoreState(...)` and `_economy.Market.RestoreState(...)`), consistent with `AGENTS.md`'s H7 note that `Main.cs` is organized as per-subsystem `SetupXxx`/`SaveXxx`/`FlushXxxIfDirty` triads rather than one unified load/save entry point. There is no existing single point "immediately after all `RestoreState` calls complete" to hook into — that point doesn't exist in the current architecture and must be created.

Two real options, either of which is legitimate but should be chosen deliberately rather than assumed away:
1. **Add a new orchestration point.** Introduce an explicit `LoadAll()` method (mirroring the existing `SaveAll()` at `src/Main.cs:1771`) that calls each subsystem's individual load method in sequence, then runs the invariant suite once at the end. This is more invasive — it changes the current scattered-load architecture — but gives one clean hook.
2. **Run the suite at first-tick-after-load instead of at load time.** Since `TickSimDay(int day)` (`src/Main.cs:1643`, confirmed real) already runs after all state is in memory and ticks every subsystem, gate a one-shot "first tick after load" invariant check there instead of inventing a new load choke point. Lower risk (reuses an existing method), but conflates two different triggers (post-load vs. post-tick) if not clearly flagged in logs.

Pick one before implementing Step 5 — the code sample below assumes option 1 exists; adjust accordingly if option 2 is chosen.

```csharp
// Illustrative only — assumes a new LoadAll() orchestration method is added
// per option 1 above. Does not correspond to an existing method in src/Main.cs.
private bool LoadAll(string savePath)
{
    // ... deserialize save file ...
    // ... restore all system states (call each subsystem's existing LoadXxx()/RestoreState) ...
    _needsSystem.RestoreState(saveData.Needs);
    _radiationSystem.RestoreState(saveData.Radiation);
    // NOTE: no InventorySystem class exists — use the real
    // Ashfall.Core.Inventory.Inventory instance's RestoreState if one exists,
    // verify the actual method name before using this sample.
    _expeditionSystem.RestoreState(saveData.Expeditions);
    // ... remaining restores ...

    // POST-LOAD INVARIANT CHECK
    var report = _invariantSuite.RunAll();
    if (report.HasCritical)
    {
        _log.Error($"[LOAD] Save rejected — {report.CriticalCount} critical invariant violation(s)");
        foreach (var v in report.Violations.Where(v => v.Severity == InvariantSeverity.Critical))
        {
            _log.Error($"  {v.CheckerName}: {v.Message} ({v.Context})");
        }
        // Revert to default/empty state or show error UI
        ResetToSafeState();
        return false;
    }

    if (report.HasErrors)
    {
        _log.Warn($"[LOAD] Save loaded with {report.ErrorCount} non-critical violation(s) — state may be inconsistent");
    }

    return true;
}
```

Load-rejection policy:
- **Critical → reject save entirely.** Player sees "Save file corrupted" message with details.
- **Error → load but warn.** Player can continue but a warning indicator appears.
- **Warning → load silently.** Only visible in developer logs.
- Rejected saves are NOT deleted — they're preserved for debugging (copy to `saves/quarantined/`)

Edge cases:
- Very old saves (pre-invariant-checker era) may have accumulated violations — provide a one-time `--repair-save` CLI option that attempts automated repair before validation
- Saves from older versions that lack certain fields should not trigger false-positive critical violations — checkers must handle missing data gracefully

### Verification
- Create a test save with a known critical violation (dead survivor on expedition), attempt load, verify rejection
- Create a test save with a warning-level violation (orphan quest flag), verify it loads with warning logged
- Create a clean save, verify it loads without any violations
- `dotnet test` — round-trip save tests still pass (no false positives)

### Done-when
- Invariant suite runs immediately after all `RestoreState` calls complete
- **Precondition (added):** the single post-restore hook point this step assumes does not exist yet in `src/Main.cs` — either a new `LoadAll()` orchestrator is added, or the check is moved to first-tick-after-load (see the two options under Step 5's Implementation). State which option was chosen in the PR description; "immediately after all `RestoreState` calls complete" is not currently a real moment in the code and must be created or approximated.
- Critical violations reject the save and trigger safe-state fallback
- Error violations allow load but log warnings
- Old saves without violations load cleanly (no false positives) — verify this against at least one real pre-existing save fixture from `Ashfall.Core.Tests/Fixtures/saves/` if such fixtures exist, or note if none currently exist and one must be created for this test
- Rejected saves are quarantined, not deleted — a `saves/quarantined/` directory and the copy-on-reject logic do not exist yet; this is new behavior, not wiring onto something already there

---

## Step 6: Add --invariant-check Headless CLI Command

### Goal
Provide a standalone CLI verb that loads a save file (or creates a default game state) and runs the full invariant suite — useful for CI, batch validation of test saves, and developer debugging.

### Implementation

**Correction:** existing CLI verbs (`--data-integrity-selftest`, `--bridge-selftest`, etc.) are parsed in `src/Host/HostCli.cs` (see `Has(args, "--data-integrity-selftest")` at line 239), not in `src/Main.cs` directly. Wire `--invariant-check` into the same `HostCliAction` dispatch pattern in `HostCli.cs` — follow the existing `Has(args, "...")` convention and add a corresponding `HostCliAction.InvariantCheck` case, plus a `--help` line in the same style as the ones at `HostCli.cs:304-308`.

**CLI verb:** `godot --headless --path . -- --invariant-check [save_path]`

Behavior:
1. If `save_path` provided: load that save, restore all system state, run invariant suite
2. If no `save_path`: create a fresh default game state (day 1, default survivors, empty inventory), run invariant suite (should always pass — verifies checkers don't false-positive on clean state)
3. Print results to stdout in structured format
4. Exit code: 0 if clean, 1 if errors, 2 if critical violations

**Output format:**

```
[INVARIANT CHECK] Running 5 checkers against save: saves/test_save_001.json
  [PASS] survivor_assignment_uniqueness (0 violations)
  [PASS] inventory_slot_integrity (0 violations)
  [FAIL] dead_survivor_guard (1 violation, severity: CRITICAL)
    - Survivor surv_elena is dead but assigned to expedition exp_northern_reach
      (NOTE: this checker is a regression guard — RadiationSystem.Tick() and
      ExpeditionSystem already prevent this in normal operation; a FAIL here
      in real usage indicates something bypassed those guards directly, e.g.
      a hand-edited save file or a new code path that skipped the IsAlive check)
  [PASS] quest_flag_consistency (0 violations)
  [PASS] economy_bounds (0 violations)

RESULT: 1 CRITICAL, 0 ERROR, 0 WARNING — FAIL (exit 2)
```

Integration with existing CLI verbs:
- Follows the pattern of `--data-integrity-selftest` and `--bridge-selftest`
- Can be composed: `godot --headless --path . -- --invariant-check --data-integrity-selftest` (both run)
- Listed in the verification checklist as an optional sixth step

**Batch mode for CI:**

`Ashfall.Core.Tests/Fixtures/saves/` does not currently exist — this directory and its `.json` fixtures must be created as part of this step, they are not a pre-existing asset this batch can assume. Once created:

```bash
# Validate all test fixtures (directory + fixtures created by this step)
for save in Ashfall.Core.Tests/Fixtures/saves/*.json; do
    godot --headless --path . -- --invariant-check "$save"
done
```

### Verification
- `godot --headless --path . -- --invariant-check` exits 0 on fresh state
- Create a deliberately broken save fixture, verify exit code 2
- Verify output format matches specification above
- Verify the verb is listed when running `--help` or invalid args

### Done-when
- `--invariant-check` CLI verb exists and is handled in the Godot host entry point
- Accepts optional save path argument
- Prints per-checker pass/fail with violation details
- Exit codes: 0 (clean), 1 (errors), 2 (critical)
- Fresh default state always exits 0 (no false positives)

---

## Step 7: Write Invariant Violation Tests

### Goal
Comprehensive test coverage that deliberately creates invalid cross-system state and verifies each checker detects the violation. Also tests that valid state produces zero violations (no false positives).

### Implementation

**File:** `Ashfall.Core.Tests/Validation/InvariantCheckerTests.cs`

```csharp
namespace Ashfall.Core.Tests.Validation
{
    public class SurvivorAssignmentCheckerTests
    {
        [Fact]
        public void Clean_state_no_violations()
        {
            // Arrange: survivor on exactly one assignment
            // Act: check
            // Assert: violations.Count == 0
        }

        [Fact]
        public void Survivor_on_expedition_and_duty_roster_reports_critical()
        {
            // Arrange: same survivor_id in expedition team AND duty roster
            // Act: check
            // Assert: 1 critical violation with both assignments in Context
        }

        [Fact]
        public void Multiple_survivors_double_assigned_reports_each()
        {
            // Arrange: 3 survivors each double-booked
            // Act: check
            // Assert: 3 violations
        }
    }

    public class InventorySlotIntegrityCheckerTests
    {
        [Fact]
        public void Consistent_inventory_no_violations() { /* ... */ }

        [Fact]
        public void Negative_amount_slot_detected() { /* slot.Amount < 0 */ }

        [Fact]
        public void Zero_amount_slot_detected() { /* slot.Amount == 0, should have been removed */ }

        [Fact]
        public void Null_item_reference_detected() { /* slot.Item == null but slot present */ }

        // NOTE: corrected from the original "Phantom_item_detected" test, which
        // asserted total != sum(containers). Ashfall.Core.Inventory.Inventory has
        // no multi-container structure — there is nothing to sum across. Removed.
    }

    public class DeadSurvivorGuardCheckerTests
    {
        // NOTE: these are regression-guard tests. In current code, none of these
        // scenarios can be constructed through normal system APIs because
        // RadiationSystem.Tick() and NeedsSystem.EvaluateDeath() already prevent
        // them — these tests construct the invalid state directly (bypassing the
        // normal APIs) to prove the checker would catch it if the guard regressed.
        [Fact]
        public void Dead_survivor_in_no_systems_clean() { /* ... */ }

        [Fact]
        public void Dead_survivor_accumulating_radiation_critical() { /* ... */ }

        [Fact]
        public void Dead_survivor_on_expedition_critical() { /* ... */ }

        [Fact]
        public void Dead_survivor_on_duty_roster_critical() { /* ... */ }
    }

    public class QuestFlagConsistencyCheckerTests
    {
        [Fact]
        public void All_flags_reference_valid_quests_clean() { /* ... */ }

        [Fact]
        public void Orphan_flag_reports_warning() { /* ... */ }

        [Fact]
        public void Non_quest_flags_ignored() { /* flags without quest_ prefix skipped */ }
    }

    public class EconomyBoundsCheckerTests
    {
        [Fact]
        public void Positive_prices_clean() { /* ... */ }

        [Fact]
        public void Zero_price_reports_error() { /* ... */ }

        [Fact]
        public void Negative_price_reports_error() { /* ... */ }

        [Fact]
        public void Negative_trade_quantity_reports_error() { /* ... */ }
    }

    public class InvariantValidationSuiteTests
    {
        [Fact]
        public void Empty_suite_is_always_clean() { /* 0 checkers → IsClean */ }

        [Fact]
        public void Aggregates_violations_from_multiple_checkers() { /* ... */ }

        [Fact]
        public void HasCritical_true_when_any_checker_reports_critical() { /* ... */ }

        [Fact]
        public void ValidateAndLog_returns_false_on_critical() { /* ... */ }

        [Fact]
        public void ValidateAndLog_returns_true_on_warnings_only() { /* ... */ }
    }
}
```

Test categories:
- **Happy path:** valid state → zero violations (prevents false positives)
- **Single violation:** one broken invariant → exactly one violation with correct severity
- **Multiple violations:** compound broken state → all violations reported (no short-circuit)
- **Boundary:** edge cases like empty inventory (valid), single survivor (no double-booking possible)
- **Suite aggregation:** multiple checkers, mixed severities, correct totals

### Verification
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "Validation"` — all tests pass
- No test mutates state that other tests depend on (each test creates its own state)
- Tests construct real instances of the concrete classes (`SurvivorRosterSystem`, `ExpeditionSystem`, `DutyRosterSystem`, `MedicalWardSystem`, `Inventory`, `MarketSystem`) directly — **corrected**: the original text said "mock/stub implementations of system interfaces," but Step 2's corrected checker designs inject concrete classes, not interfaces (`ISurvivorRoster` etc. don't exist), so there is nothing to mock via an interface. Use real lightweight instances seeded with minimal test data instead. No `GameBootstrap` class exists in this codebase (confirmed — only a stale reference in `_quarantine_legacy/`) and none of these tests should reference or require one.

### Done-when
- At minimum 20 tests across all 5 checkers + suite aggregation
- Every checker has at least: one clean-state test, one violation-detection test
- Suite aggregation tested with 0, 1, and multiple checkers
- All tests pass in `dotnet test`
- No engine dependencies in test code

---

## Summary Table

| Step | Deliverable | Files | Depends On | Risk |
|------|-------------|-------|------------|------|
| 1 | `IInvariantChecker` interface + types | 3 new in `Validation/` | — | None |
| 2 | 5 concrete invariant checkers | 5 new in `Validation/Checkers/` (2 of the original 5 designs were corrected — see Step 2 and Review Notes) | Step 1 | Low — read-only inspection, but Checker 1 may require adding new read-only query methods to `ExpeditionSystem`/`DutyRosterSystem`/`MedicalWardSystem` if none exist yet — uncosted work, verify before estimating |
| 3 | `InvariantValidationSuite` aggregator | 2 new in `Validation/` | Steps 1-2 | None |
| 4 | Post-tick wiring in host | 1 modified (`src/Main.cs`, specifically inside `TickSimDay(int day)` at line 1643 — confirmed real method) | Steps 1-3 | Low-Medium — conditional, no state mutation, but suppressing `SaveAll()` on a false positive has real player-facing impact (lost progress); see Step 4 risk note |
| 5 | Post-load wiring in host | 1 modified — **no single load pipeline exists to modify**; requires either adding a new `LoadAll()` orchestrator or hooking first-tick-after-load (see Step 5 correction) — this is closer to "1 new + N call-site changes" than "1 modified" | Steps 1-3 | Low, but scope is larger than "1 modified file" implies |
| 6 | `--invariant-check` CLI verb | 1 modified: `src/Host/HostCli.cs` (confirmed real location of existing CLI verb dispatch — not "host entry point" generically) | Steps 1-3 | None — additive CLI verb |
| 7 | Invariant violation tests | 1+ new in `Ashfall.Core.Tests/Validation/` (directory does not exist yet — create it) | Steps 1-3 | None |

**Total new files:** ~12 (unchanged from original estimate; still a reasonable count for the corrected scope)
**Modified files:** ~3 (`src/Main.cs`, `src/Host/HostCli.cs`, and either a new load-orchestration point or an existing `LoadXxx()` method per subsystem — the original "~2" undercounts because "load pipeline" isn't one file)
**Estimated effort:** Originally "2-3 sessions" — plausible for Steps 1, 2 (partially), 3, 6, 7; Step 4's risk note and Step 5's missing load-hook add real scope not accounted for in the original estimate. Revise to 3-4 sessions, with Step 5 as the most likely session to overrun.
**Breaking changes:** None — entirely additive. **Rollback (added):** every step is a net-new file or an additive call site (`TickSimDay`, `HostCli.cs` dispatch) — reverting is a straightforward file deletion + call-site removal at any step boundary. The one step needing care on rollback is Step 4 if it ships to a release build and a false-positive critical violation is already suppressing saves for players — the fastest field fix is flipping the `#if DEBUG || INVARIANT_CHECKS` gate off remotely (if build-configurable) rather than a full revert-and-redeploy cycle; confirm this is possible before shipping Step 4 to production.
**Performance impact:** < 1ms per tick in debug builds (unverified estimate — measure against the real checker implementations once Step 2 is done, given the corrected checker designs touch different data structures than originally assumed); zero in release (conditional compilation)

---

## Review Notes (Corrected)

Adversarial pass against the live codebase at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`. All fixes above were applied in place; this section documents what was wrong, why, and what still needs a human decision.

### Factual corrections applied

| Claim in original batch | Verified reality | Evidence |
|---|---|---|
| "82+ systems ticked" | ~1,110 public types in `Assets/Ashfall.Core/`, no clean "systems ticked" count verified | grep-based type count, same methodology as Batch 87 review |
| "A dead survivor can continue accumulating radiation dose... (zombie state)" | **False as stated.** `RadiationSystem.Tick()` already skips `!survivor.IsAlive` survivors (`Assets/Ashfall.Core/Radiation/RadiationSystem.cs:188`); `NeedsSystem.EvaluateDeath()` is the single place death is set. This is already enforced, not a live gap. | direct code read of both files |
| "Total items in inventory can drift from the sum across all containers (phantom items)" | **Does not apply.** `Ashfall.Core.Inventory.Inventory` (`Assets/Ashfall.Core/Inventory/Inventory.cs`) is one flat `List<InventorySlot>` with no sub-container concept — nothing to sum across. No `InventorySystem` class or `Container` class exists anywhere in Core. | `grep "class.*Container"` → zero matches; direct read of `Inventory.cs` |
| "Weather radiation minus shelter shielding minus personal gear can yield negative net exposure (impossible physics)" | **Already clamped.** `RadiationSystem.ComputeEffectiveAmbient()` and the exposure computation both wrap in `MathfCompat.Max(0f, ...)` — negative exposure is prevented at the arithmetic site. | `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` exposure computation methods |
| "Expedition team members can reference survivor IDs not in the master roster (ghost crew)" | **Confirmed real, unenforced gap.** `SurvivorRosterSystem` exists; no code path cross-checks `ExpeditionSystem.survivorId` values against it. | grep for `SurvivorRosterSystem` combined with `Expedition` → zero cross-references found |
| Checker constructor dependencies `ISurvivorRoster`, `IExpeditionRegistry`, `IDutyRoster`, `IMedicalBay` | **None of these interfaces exist anywhere in the codebase.** They were invented for this plan. Real concrete classes: `SurvivorRosterSystem`, `ExpeditionSystem`, `DutyRosterSystem`, `MedicalWardSystem` (not `MedicalSystem`, which also doesn't exist). | repo-wide grep for each interface name → zero matches |
| Step 4: single `LoadSave(string savePath)` integration point | **Does not exist.** `src/Main.cs` has no unified load method — restore logic is scattered across many per-subsystem `LoadXxx()` methods and inline `RestoreState` calls, consistent with `AGENTS.md`'s H7 note about `Main.cs`'s per-subsystem triad structure (which appears to extend to load, not just save). | grep for `RestoreState`/`LoadSave` in `src/Main.cs`; confirmed scattered call sites, no single entry point |
| Step 6: CLI verbs handled in "Godot host entry point" (implied `src/Main.cs`) | Existing CLI verbs (`--data-integrity-selftest`, `--bridge-selftest`) are parsed in `src/Host/HostCli.cs`, a distinct file from `src/Main.cs`. | `src/Host/HostCli.cs:239` |
| Step 6: `Ashfall.Core.Tests/Fixtures/saves/*.json` referenced as existing fixtures | **Directory does not exist.** Must be created as part of this batch, not assumed as a pre-existing asset. | `find` for the directory → no results |
| `TickSimDay` post-tick integration point | **Confirmed accurate.** `src/Main.cs:1643` has `private void TickSimDay(int day)`, called from `src/Main.cs:1632`, ending with `SaveAll()` at line 1771. This part of the plan was correct as written. | direct grep + read of `src/Main.cs` |

### Attack findings (vagueness, scope, risk)

- **Vague Done-when (Step 7 original):** "Tests use mock/stub implementations of system interfaces" is inconsistent with the corrected Step 2 checker designs, which depend on concrete classes because no interfaces exist to mock. Fixed inline.
- **Missing risk/rollback (Step 4):** the original had no mention that suppressing auto-save on a false-positive critical violation is a real player-facing risk (potential lost progress). Added a risk/rollback note recommending the debug/conditional-compile gate be treated as load-bearing, not cosmetic.
- **Missing risk/rollback (Step 5):** compounded by the missing single load hook — without deciding between the two options (new `LoadAll()` orchestrator vs. first-tick-after-load), this step cannot be implemented as originally written. Flagged as a blocking scope decision, not a nice-to-have.
- **File-count / effort underestimate:** "Modified files: ~2" and "2-3 sessions" undercount the real work once Checker 1's dependency on possibly-nonexistent read-only query methods (on `ExpeditionSystem`, `DutyRosterSystem`, `MedicalWardSystem`) and Step 5's missing load hook are accounted for. Revised estimate: 3-4 sessions, with explicit warning that Step 5 is the most likely to overrun.
- **Ordering:** Steps 1→2→3 are correctly sequenced. Steps 4, 5, and 6 all depend on 1-3 and are independent of each other — the plan's "Depends On: Steps 1-3" for all three is accurate, and there's no reason they couldn't be implemented in a different relative order (e.g., 6 before 4/5) if that's more convenient. No illogical ordering found, but note Step 5 is likely to take the longest given the missing load hook, so sequencing it last (after 4 and 6) rather than by number would reduce overall batch risk.
- **No CI/build reference errors found** beyond the `HostCli.cs` vs. `src/Main.cs` mislocation — the `godot --headless` and `dotnet test`/`dotnet build` commands referenced throughout match the project's actual verification commands per `AGENTS.md`.
