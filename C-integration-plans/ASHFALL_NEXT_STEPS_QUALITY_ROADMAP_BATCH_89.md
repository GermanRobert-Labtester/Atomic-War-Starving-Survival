# ASHFALL — Quality Roadmap Batch 89

## Theme: Expansion Content Pipeline — Standardized Workflow for Adding New Expansions

**Priority:** MEDIUM — accelerates content development, reduces errors and onboarding time<br>
**Risk:** Low — process/tooling/templates, not changes to running gameplay code<br>
**Batch:** 89<br>
**Depends on:** Expansion system architecture stable (`ExpansionMasterSession`, `ExpansionHostSession`, `src/Main.cs` Setup/Save/Flush triads, save stores)<br>
**Blocked by:** Nothing — can be done in parallel with feature work<br>

> **CORRECTED (see Review Notes at bottom):** This plan originally named `GameBootstrap` as the
> wiring target for Phase 4 and the checklist. **`GameBootstrap` does not exist anywhere in this
> codebase** (`AGENTS.md` H7 and the repo itself confirm the real god-object is `src/Main.cs`,
> a single ~6.5k-line partial class with per-subsystem `SetupXxx`/`SaveXxx`/`FlushXxxIfDirty`
> triads — 31 Setup / 24 Save+SaveAll / 17 Flush methods). Every reference to `GameBootstrap`
> below has been corrected to `src/Main.cs` and, where the expansion is a numbered one,
> `ExpansionMasterSession`. See Step 4 and Step 6 corrections.

---

## Motivation

The project documents a five-phase implementation workflow for expansions:
1. System classes in domain-specific namespaces (CaptureState/RestoreState DTOs)
2. Data: update items.json, locations.json, survivors.json, recipes.json
3. New IDs into static classes, trait constants, quest runtime classes
4. Wire into the host: properties, construction, event wiring, init, tick registration, save fields — via `src/Main.cs` Setup/Save/Flush triads and, for numbered expansions, `ExpansionMasterSession` construction/`TickDaily()`
5. Tests: behavior, save round-trips, canonical-IDs, integration smoke

Four numbered expansions exist: Holdfast (01), Duty Roster (02), Standing Record (03), Nobody's Charter/Crossing (04). Plus standalone expansions: Verdict, YearOfAsh, Greenhouse, SilentFoundry (10), Warlords, Disease, Combat. Each was built ad-hoc — no scaffold, no template, no shared interface contract. Adding a new expansion today requires:

- Manually creating 8-15 files across 4 directories
- Touching `src/Main.cs` (one ~6.5k-line partial class, per AGENTS.md H7 — **not** `GameBootstrap`, which does not exist) for host wiring
- Creating a catalog loader that mirrors existing patterns (but no reference template)
- Adding a save store with checksum envelope (pattern learned by reading existing stores)
- Wiring into `src/Main.cs`'s host session logic, and — separately — into whichever of the two existing host-level aggregators applies: `ExpansionMasterSession` (Core-level orchestrator, currently hard-codes exactly 4 numbered expansions + Silent Foundry + Disease, constructed via `ExpansionMasterSession.Load()`) or `ExpansionHostSession` (`src/Host/ExpansionHostSession.cs`, the thin Godot-host wrapper actually constructed by `Main.cs.SetupExpansions()`, which covers a *different* and *larger* subsystem set: Waystation, Layouts, Memory, SiteEncounters, RecordQuests, Vouch, Greenhouse, Arbitration, Ledger, CrossingQuests, Generational, Epilogue, SilentFoundry, Disease). These two classes are not interchangeable and this plan must not conflate them.
- Creating a self-test CLI verb

This batch creates the missing infrastructure: templates, checklists, a generator tool, and standardized patterns so that expansion N+1 takes hours instead of days.

---

## Step 1: Create Expansion Scaffold Template (Directory Structure)

### Goal
Define the canonical directory layout and file set for a new expansion. Every expansion follows this structure — deviations are bugs, not creativity.

### Implementation

**File:** `docs/expansion-scaffold-template.md`

```
Expansion: {ExpansionName} (ID: {nn})
Prefix: {prefix_}  (e.g., holdfast_, dutyroster_, standing_, crossing_)

Directory layout:
├── Assets/Ashfall.Core/{ExpansionName}/
│   ├── {ExpansionName}System.cs          # Main system (ITickable, CaptureState/RestoreState)
│   ├── {ExpansionName}State.cs           # Serializable state DTO
│   ├── {ExpansionName}Config.cs          # Static configuration / tuning constants
│   ├── {ExpansionName}Ids.cs             # Static class with all expansion-specific IDs
│   ├── {ExpansionName}Events.cs          # Event types raised by the system
│   └── (optional subsystems)
│
├── Assets/StreamingAssets/Data/{expansion_name}/
│   ├── {expansion_name}_items.json       # Expansion-specific items
│   ├── {expansion_name}_locations.json   # Expansion-specific locations (if any)
│   ├── {expansion_name}_recipes.json     # Expansion-specific recipes (if any)
│   └── {expansion_name}_config.json      # Runtime configuration (schema_version required)
│
├── src/Host/{ExpansionName}HostSession.cs  # Godot host wiring (thin adapter)
├── src/Stores/{ExpansionName}SaveStore.cs  # Save store (checksummed envelope)
│
├── Ashfall.Core.Tests/{ExpansionName}/
│   ├── {ExpansionName}SystemTests.cs     # Core behavior tests
│   ├── {ExpansionName}SaveRoundTripTests.cs  # Save/load integrity
│   └── {ExpansionName}IdTests.cs         # Canonical ID validation
│
└── (integration wiring — see checklist)
```

Naming conventions:
- **Namespace:** `Ashfall.Core.{ExpansionName}` (Core), `AtomicWar.GodotApp.{ExpansionName}` (host)
- **ID prefix:** `{expansion_name_abbreviated}_` (max 12 chars, snake_case)
- **Save key:** `"{expansion_name}"` in save envelope
- **CLI verb:** `--{expansion-name-kebab}-selftest`
- **JSON schema_version:** starts at `1`

### Verification
- Template document reviewed against all 4 existing numbered expansions — confirm they match.
  Concretely: for Holdfast, list the actual files under `Assets/Ashfall.Core/Holdfast*` (or
  wherever the real Holdfast system files live — **verify this path exists before writing the
  template**; do not assume a `{ExpansionName}/` subfolder naming pattern is already followed)
  and diff against the template's file list by hand.
- Template document covers all artifacts that Holdfast (most complete) has
- No engine-specific paths in Core portion of template

### Done-when
- `docs/expansion-scaffold-template.md` exists with complete directory layout
- File naming conventions documented with examples
- Namespace conventions documented
- ID prefix rules documented
- Template validated against at least 2 existing expansions, with the specific two named in the
  document's own changelog/footer (e.g. "validated against Holdfast and Disease on <date>") so a
  reviewer can re-check the claim later without re-deriving which expansions were used

---

## Step 2: Create Expansion Implementation Checklist

### Goal
A step-by-step checklist (checkbox format) that an implementer follows when building a new expansion. Covers all five phases, all artifacts, all wiring points. Nothing is left to memory or guesswork.

### Implementation

**File:** `docs/expansion-implementation-checklist.md`

```markdown
# Expansion Implementation Checklist: {ExpansionName}

## Phase 1: Core System (no engine dependencies)
- [ ] Create `Assets/Ashfall.Core/{ExpansionName}/` directory
- [ ] Implement `{ExpansionName}System.cs` with ITickable
- [ ] Implement `{ExpansionName}State.cs` (serializable DTO, all fields)
- [ ] Add `CaptureState()` → returns populated `{ExpansionName}State`
- [ ] Add `RestoreState({ExpansionName}State state)` — full reconstitution
- [ ] Implement `{ExpansionName}Config.cs` (tuning constants)
- [ ] Implement `{ExpansionName}Ids.cs` (static string constants, prefixed)
- [ ] Implement `{ExpansionName}Events.cs` (C# events for state changes)
- [ ] Verify: `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` passes
- [ ] Verify: zero references to UnityEngine/Godot in new files

## Phase 2: Data Authority
- [ ] Create `Assets/StreamingAssets/Data/{expansion_name}/` directory
- [ ] Add `{expansion_name}_items.json` with schema_version
- [ ] Add `{expansion_name}_config.json` with schema_version
- [ ] Add items to master `items.json` OR use expansion-scoped file (choose one)
- [ ] All IDs use correct prefix (`{prefix_}`)
- [ ] All IDs registered in CatalogIntegrityValidator prefix list
- [ ] Verify: `godot --headless --path . -- --data-integrity-selftest` passes

## Phase 3: ID Registration
- [ ] Add all new IDs to `{ExpansionName}Ids.cs` as `public const string`
- [ ] Add trait constants (if expansion adds traits)
- [ ] Add quest IDs (if expansion adds quests)
- [ ] Verify: no duplicate IDs across project (CatalogIntegrityValidator UNIQUENESS tier)

## Phase 4: Host Wiring (Godot)
- [ ] Create `src/Host/{ExpansionName}HostSession.cs` (thin adapter) — OR add the module to the
      existing `src/Host/ExpansionHostSession.cs` if it belongs in the expansion-hub subsystem set
      (Waystation/Layouts/Vouch/Greenhouse/Arbitration/Ledger/etc.) rather than standing alone
- [ ] Create `src/Stores/{ExpansionName}SaveStore.cs` (checksummed envelope)
- [ ] Add Setup{ExpansionName}() in `src/Main.cs` (construct + wire) — `src/Main.cs` is the real
      host god-object (~6.5k lines, one `partial class Main`, per AGENTS.md H7). **There is no
      `GameBootstrap` in this codebase — do not create one and do not wire into one.**
- [ ] Add Save{ExpansionName}() in `src/Main.cs` (capture + flush)
- [ ] Add Flush{ExpansionName}IfDirty() in `src/Main.cs`
- [ ] Register in `SaveAll()` orchestration in `src/Main.cs`
- [ ] Register tick in the day-tick pipeline (`Main.cs` day-advance method that calls the other
      `TickXxx`/`SetupExpansions()`-style hooks — confirm the exact call site before wiring; it is
      not a generic "pipeline" registration list, it is a manually-ordered sequence of calls)
- [ ] Add CLI verb `--{expansion-name}-selftest`
- [ ] If numbered expansion (Holdfast/DutyRoster/StandingRecord/Crossing-family): add construction
      + `TickDaily()` participation in `Assets/Ashfall.Core/ExpansionMasterSession.cs`. This session
      currently hard-codes exactly 4 numbered expansions plus Silent Foundry and Disease by direct
      property/constructor reference — there is no dynamic registry today. Adding a 5th expansion
      means editing `ExpansionMasterSession`'s constructor, `Load()`, and `TickDaily()` by hand
      unless Step 4 of this batch (`IExpansionModule`/`ExpansionRegistry`) has landed first.
- [ ] Verify: `dotnet build Ashfall.csproj` passes (0 errors, 0 warnings)

## Phase 5: Tests
- [ ] `{ExpansionName}SystemTests.cs` — core behavior (≥5 tests)
- [ ] `{ExpansionName}SaveRoundTripTests.cs` — CaptureState→serialize→deserialize→RestoreState
- [ ] `{ExpansionName}IdTests.cs` — all IDs resolve, correct prefix, no duplicates
- [ ] Integration: system interacts correctly with survivor roster / economy / etc.
- [ ] Verify: `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — all pass
- [ ] Verify: `godot --headless --path . -- --{expansion-name}-selftest` exits 0

## Final Validation
- [ ] All 5 standard verification steps pass
- [ ] No new compiler warnings
- [ ] State DTO has matching schema_version in JSON config
- [ ] Expansion documented in AGENTS.md expansion list
```

### Verification
- Walk through checklist against Holdfast expansion — confirm every box would be checked
- Walk through checklist against a newer expansion (Verdict/YearOfAsh) — confirm coverage
- No step references Unity-specific tooling

### Done-when
- `docs/expansion-implementation-checklist.md` exists with all 5 phases
- Every artifact from the scaffold template has a corresponding checkbox
- Verification steps embedded at each phase boundary
- Checklist validated against 2+ existing expansions

---

## Step 3: Implement Expansion Generator Script

### Goal
A `dotnet` console tool that scaffolds all expansion files from a name and ID prefix, so implementers start with compilable stubs rather than a blank slate.

### Implementation

**Project:** `tools/ExpansionGenerator/ExpansionGenerator.csproj` (console app, `net8.0`)

```bash
# Usage:
dotnet run --project tools/ExpansionGenerator -- \
  --name "SalvageRun" \
  --prefix "salvage_" \
  --id 05 \
  --has-items \
  --has-recipes \
  --has-locations
```

Generated output:
- `Assets/Ashfall.Core/SalvageRun/SalvageRunSystem.cs` — compilable stub with `Tick()`, `CaptureState()`, `RestoreState()`
- `Assets/Ashfall.Core/SalvageRun/SalvageRunState.cs` — empty DTO with `[Serializable]`
- `Assets/Ashfall.Core/SalvageRun/SalvageRunConfig.cs` — static class with placeholder constants
- `Assets/Ashfall.Core/SalvageRun/SalvageRunIds.cs` — static class with example ID
- `Assets/Ashfall.Core/SalvageRun/SalvageRunEvents.cs` — empty events class
- `Assets/StreamingAssets/Data/salvage_run/salvage_run_items.json` — minimal valid JSON with schema_version
- `Assets/StreamingAssets/Data/salvage_run/salvage_run_config.json` — minimal valid JSON
- `src/Host/SalvageRunHostSession.cs` — Godot host stub
- `src/Stores/SalvageRunSaveStore.cs` — save store with checksum envelope pattern
- `Ashfall.Core.Tests/SalvageRun/SalvageRunSystemTests.cs` — one passing test stub
- `Ashfall.Core.Tests/SalvageRun/SalvageRunSaveRoundTripTests.cs` — round-trip test template

Template engine (simple string replacement, no heavy dependencies):
- `{ExpansionName}` → PascalCase name
- `{expansion_name}` → snake_case name
- `{expansion-name}` → kebab-case name
- `{prefix_}` → the ID prefix
- `{nn}` → two-digit expansion number
- `{Namespace}` → `Ashfall.Core.{ExpansionName}`

Flags:
- `--has-items` — generates item JSON template
- `--has-recipes` — generates recipe JSON template
- `--has-locations` — generates location JSON template
- `--numbered` — marks the expansion as numbered for documentation/ID-prefix purposes only.
  **Correction:** this flag cannot "register in ExpansionMasterSession" as originally stated.
  `ExpansionMasterSession` has no dynamic registration mechanism today (see Step 4 correction) —
  it hard-codes 4 named properties and manual constructor wiring. Until Step 4's
  `IExpansionModule`/`ExpansionRegistry` exists, `--numbered` can only emit a code comment /
  TODO block showing the exact `ExpansionMasterSession.cs` edits a human must make by hand
  (constructor parameter, `Load()` construction call, `TickDaily()` call site). Auto-editing
  `ExpansionMasterSession.cs` from the generator is out of scope for this step — flag it instead.
  **Ordering fix:** because of this dependency, Step 3 (generator) should not promise
  `ExpansionMasterSession` integration until Step 4 has landed; sequence Step 4 before Step 3 in
  execution even though it is numbered after it in this document, or explicitly scope Step 3's
  first pass to standalone (non-numbered) expansions only.
- `--dry-run` — print what would be generated without writing files

### Verification
- `dotnet build tools/ExpansionGenerator/ExpansionGenerator.csproj` — tool compiles
- `dotnet run --project tools/ExpansionGenerator -- --name Test --prefix test_ --id 99 --dry-run` — prints file list
- Generate a test expansion, then `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — generated stubs compile
- Generated JSON passes `--data-integrity-selftest`

### Done-when
- Generator tool exists at `tools/ExpansionGenerator/`
- Generates all scaffold files from name + prefix + ID
- Generated stubs compile without errors
- Generated JSON files have `schema_version` and valid structure
- `--dry-run` mode works
- No engine dependencies in the generator tool itself

---

## Step 4: Standardize Expansion Registration Interface

### Goal
Define `IExpansionModule` — a formal contract that every expansion implements for consistent lifecycle management by `ExpansionMasterSession` and the host.

> **CORRECTED:** `IExpansionModule` does not exist anywhere in the codebase today (confirmed by
> full-repo search — zero hits). This is **new infrastructure being proposed from scratch**, not
> a standardization of something already partially built. Treat every claim below of "existing
> expansion wrapped with adapter" as future work this step creates, not work already done.
> `ExpansionMasterSession.cs` today hard-codes its 4 numbered expansions (+ Silent Foundry +
> Disease) as named properties (`Holdfast`, `DutyRoster`, `StandingRecord`, `Crossing`,
> `SilentFoundry`, `Disease`) constructed directly in its constructor and `Load()`, and its
> `TickDaily()` calls each subsystem's own tick method by name (e.g.
> `Holdfast.IceRoad.TickDaily(...)`, `DutyRoster.TickMorning(...)`, `SilentFoundry.TickDaily(day)`,
> `Disease.TickDaily(day, diseaseCandidates)`) — there is no loop over a generic collection today.
> Retrofitting `IExpansionModule` onto these 4 concrete systems means writing 4 adapter classes
> whose `Tick(int day)` forwards to non-uniform underlying signatures (different parameter lists:
> weather+temp for Holdfast, occupant lists for DutyRoster, survivor-candidate lists for Disease).
> This is real adapter-writing work, not thin wrapping — size this step accordingly (see Summary
> Table correction at the bottom).

### Implementation

**File:** `Assets/Ashfall.Core/Expansions/IExpansionModule.cs`

```csharp
namespace Ashfall.Core.Expansions
{
    /// <summary>
    /// Contract for a self-contained expansion module. Enables uniform lifecycle
    /// management without src/Main.cs or ExpansionMasterSession knowing internal details.
    /// </summary>
    public interface IExpansionModule
    {
        /// <summary>Two-digit expansion ID (01-99). Unique across all expansions.</summary>
        int ExpansionId { get; }

        /// <summary>Human-readable name for UI and logging.</summary>
        string Name { get; }

        /// <summary>snake_case ID prefix for all assets in this expansion.</summary>
        string IdPrefix { get; }

        /// <summary>Whether this expansion is currently active (data loaded, systems ticking).</summary>
        bool IsActive { get; }

        /// <summary>IDs of expansions that must be active before this one can activate.</summary>
        IReadOnlyList<int> Dependencies { get; }

        /// <summary>Initialize internal state. Called once at boot after dependencies are met.</summary>
        void Initialize();

        /// <summary>Per-day tick. Called only when IsActive.</summary>
        void Tick(int day);

        /// <summary>Capture all internal state for serialization.</summary>
        object CaptureState();

        /// <summary>Restore internal state from deserialized data.</summary>
        void RestoreState(object state);

        /// <summary>
        /// Self-test: verify internal consistency without side effects.
        /// Returns list of issues (empty = pass).
        /// </summary>
        IReadOnlyList<string> SelfTest();
    }
}
```

**File:** `Assets/Ashfall.Core/Expansions/ExpansionRegistry.cs`

```csharp
namespace Ashfall.Core.Expansions
{
    public sealed class ExpansionRegistry
    {
        private readonly Dictionary<int, IExpansionModule> _modules = new();

        public void Register(IExpansionModule module) { /* validate unique ID, store */ }
        public IExpansionModule Get(int expansionId) { /* lookup */ }
        public IReadOnlyList<IExpansionModule> GetActive() { /* filter IsActive */ }
        public IReadOnlyList<IExpansionModule> GetAll() { /* all registered */ }

        /// <summary>
        /// Resolve activation order respecting dependencies.
        /// Throws if circular dependency detected.
        /// </summary>
        public IReadOnlyList<IExpansionModule> ResolveActivationOrder() { /* topological sort */ }
    }
}
```

Migration path for existing expansions:
- Existing expansions (Holdfast, DutyRoster, etc.) are wrapped with `IExpansionModule` adapters
- Adapters delegate to existing system classes — no rewrite required
- New expansions implement `IExpansionModule` directly from the generator scaffold
- `ExpansionMasterSession` delegates to `ExpansionRegistry` for ordering and lifecycle

### Verification
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — interfaces compile
- Unit test: register 3 modules with dependencies, verify `ResolveActivationOrder()` is correct
- Unit test: circular dependency throws
- Unit test: duplicate ID throws
- Existing expansion tests still pass (adapters don't change behavior)

### Done-when
- `IExpansionModule` and `ExpansionRegistry` exist in `Assets/Ashfall.Core/Expansions/`
- Interface covers full lifecycle (init, tick, save, restore, self-test)
- Dependencies are declarative (list of expansion IDs)
- Registry resolves activation order via topological sort
- At least one existing expansion wrapped with adapter (proof of compatibility)

---

## Step 5: Add Expansion Self-Test Pattern

### Goal
Standardize the `--{expansion-name}-selftest` CLI verb so every expansion provides consistent, automated health checks invokable from CI.

> **CORRECTED — false premise:** `--expansions-selftest` **already exists** today, in
> `src/Host/HostCli.cs:139` (`Has(args, "--expansions-selftest") || Has(args, "--all-expansions-selftest")`
> → `HostCliAction.ExpansionsSelfTest`). This step does not need to create the aggregate verb; it
> needs to make the *existing* aggregate verb consume `IExpansionModule.SelfTest()` results once
> Step 4 lands, without breaking the current hard-coded dispatch. Also already present as
> individual verbs in the same file: `--holdfast-selftest`, `--duty-roster-selftest`,
> `--expedition-selftest`, `--expedition-encounter-bridge-selftest`, `--medical-selftest`,
> `--verdict-selftest`, `--year-of-ash-save-selftest`, and more (grep `HostCli.cs` for the full,
> already-large list before adding anything, to avoid clashing with a name already in use).
> **Naming collision risk:** the plan's proposed convention `--{module.Name.ToKebabCase()}-selftest`
> must not silently shadow or duplicate an existing hand-written verb of the same name — add a
> build-time or test-time check that the generated verb list has no duplicates against
> `HostCli.cs`'s existing `Has(args, ...)` table.

### Implementation

**Self-test contract (part of `IExpansionModule.SelfTest()`):**

Each expansion's self-test verifies:
1. **Data integrity:** all expansion IDs resolve in catalogs
2. **State validity:** `CaptureState()` on default-initialized system produces valid serializable state
3. **Round-trip:** `CaptureState()` → serialize → deserialize → `RestoreState()` → `CaptureState()` produces identical state
4. **Dependency satisfaction:** all declared dependencies are registered and active
5. **Config loaded:** expansion configuration JSON parsed without errors

**CLI integration in Godot host:**

```csharp
// In the existing CLI argument handler — src/Host/HostCli.cs (NOT a hypothetical
// "dedicated CLI router" to be created; it already exists and already has ~40+ verbs)
if (args.Contains($"--{module.Name.ToKebabCase()}-selftest"))
{
    var issues = module.SelfTest();
    if (issues.Count == 0)
    {
        GD.Print($"[{module.Name}] SELF-TEST PASSED");
        // exit 0
    }
    else
    {
        foreach (var issue in issues)
            GD.PrintErr($"[{module.Name}] FAIL: {issue}");
        // exit 1
    }
}
```

**Aggregate self-test verb (already exists — this step wires it to the new registry, it does not create it):**

```bash
# Run ALL expansion self-tests in one pass — verb already dispatches today via HostCli.cs;
# this step changes what runs *behind* the verb, not the verb itself.
godot --headless --path . -- --expansions-selftest
```

This should iterate `ExpansionRegistry.GetAll()` and run each module's `SelfTest()`, reporting
aggregate pass/fail — replacing (or supplementing, during migration) whatever the current
`HostCliAction.ExpansionsSelfTest` handler does today. **Before writing this, read the current
handler for `HostCliAction.ExpansionsSelfTest` to know what it already covers, so this step
doesn't regress existing coverage.**

**CI script integration:**

```bash
#!/bin/bash
# scripts/ci/expansion-selftest-all.sh
set -e
godot --headless --path . -- --expansions-selftest
echo "All expansion self-tests passed."
```

### Verification
- Each existing expansion's CLI verb still works — concretely, run
  `godot --headless --path . -- --holdfast-selftest` and `--duty-roster-selftest` (both confirmed
  to exist in `HostCli.cs` today) before and after this change and diff the output; a regression
  here is a hard blocker since these are relied on by the current CI/verification checklist
- Aggregate verb `--expansions-selftest` (already exists) runs all and reports — verify it still
  reports the *same or greater* set of checks as it does today, not a smaller replacement set
- Exit code 0 when all pass, 1 when any fail
- Self-test output format is consistent across all expansions

### Done-when
- `IExpansionModule.SelfTest()` is the canonical self-test entry point for *newly generated*
  expansions; existing expansions' current selftest implementations are not required to migrate
  in this batch (that is separate, larger adapter work — see Step 4 correction)
- Each expansion provides meaningful self-test checks (not just `return empty`)
- `--expansions-selftest` (pre-existing verb) is confirmed to still run every check it ran before
  this change, plus any newly-registered `IExpansionModule` checks
- Individual `--{name}-selftest` verbs still work for targeted debugging, with no name collisions
  against the existing verb table in `HostCli.cs`
- CI script exists for batch self-test execution

---

## Step 6: Add Expansion Dependency Declaration

### Goal
Expansions can declare dependencies on other expansions. The system validates dependencies at activation time and provides clear errors when prerequisites are missing.

### Implementation

**Dependency declaration (in `IExpansionModule`):**

> **CORRECTED — unverified example replaced:** the original example asserted "Crossing (04)
> depends on Holdfast (01)" as if this were an existing, confirmed fact. It is not — inspection of
> `Assets/Ashfall.Core/CrossingSession.cs` shows its constructor takes only `VouchAccessSystem` and
> `CrossingCatalog`; it has **no runtime or data dependency on Holdfast today**. `ExpansionMasterSession`
> constructs all 4 numbered expansions independently and unconditionally in `Load()` — none of them
> currently gate on another's activation state. Do not encode a fictional dependency graph. If this
> batch wants a worked example, use a genuinely-planned *future* dependency (e.g., a new expansion
> that explicitly reads Holdfast state) and label it as illustrative/hypothetical, not as a
> description of the current 4 expansions.

```csharp
// Hypothetical example ONLY — no numbered expansion today declares a real dependency on
// another. Illustrates the shape of the API, not existing behavior.
public class HypotheticalFutureModule : IExpansionModule
{
    public int ExpansionId => 5;
    public string Name => "HypotheticalFuture";
    public IReadOnlyList<int> Dependencies => new[] { 1 }; // e.g. Holdfast, IF a real need exists
    // ...
}
```

**Validation in `ExpansionRegistry`:**

```csharp
public void ActivateAll()
{
    var order = ResolveActivationOrder(); // topological sort
    foreach (var module in order)
    {
        // Verify all dependencies are active
        foreach (var depId in module.Dependencies)
        {
            var dep = Get(depId);
            if (!dep.IsActive)
                throw new ExpansionDependencyException(
                    $"Cannot activate '{module.Name}' — dependency '{dep.Name}' (ID {depId}) is not active");
        }
        module.Initialize();
    }
}
```

**Error scenarios handled:**
- Missing dependency (not registered): clear error naming the missing expansion
- Inactive dependency (registered but not enabled): clear error with activation hint
- Circular dependency (A depends on B, B depends on A): detected at registration time
- Self-dependency: rejected at registration time
- Transitive dependency (A → B → C): resolved by topological sort, activated in correct order

**Save/load interaction:**
- Saves store which expansions were active at save time
- On load, if a required expansion is now missing, warn but attempt partial load
- On load, activate expansions in dependency order before restoring state

### Verification
- Unit test: A depends on B, activate in correct order — B initializes before A
- Unit test: A depends on B, B not registered — throws `ExpansionDependencyException`
- Unit test: circular dependency A↔B — throws at registration
- Unit test: 4 expansions with diamond dependency — activation order is valid topological sort
- Integration: load a save with expansion dependency, verify activation order is correct

### Done-when
- Dependencies declared as `IReadOnlyList<int>` on `IExpansionModule`
- `ExpansionRegistry.ResolveActivationOrder()` returns valid topological sort
- Circular dependencies detected and rejected with clear error
- Missing dependencies produce actionable error messages
- Save/load respects dependency order
- At least 5 unit tests cover dependency scenarios

---

## Step 7: Write Expansion Scaffold Tests

### Goal
Verify that the expansion generator produces output that compiles, passes data integrity checks, and conforms to the scaffold template. This is a meta-test: testing the tool that generates code.

### Implementation

**File:** `Ashfall.Core.Tests/Expansions/ExpansionScaffoldTests.cs`

```csharp
namespace Ashfall.Core.Tests.Expansions
{
    public class ExpansionScaffoldTests
    {
        [Fact]
        public void Generated_core_system_compiles()
        {
            // Arrange: generate scaffold for "TestExpansion" with prefix "testexp_"
            // Act: attempt to compile generated .cs files
            // Assert: no compilation errors
        }

        [Fact]
        public void Generated_state_dto_is_serializable()
        {
            // Arrange: generate scaffold, instantiate state DTO
            // Act: serialize to JSON, deserialize back
            // Assert: round-trip produces equal object
        }

        [Fact]
        public void Generated_json_has_schema_version()
        {
            // Arrange: generate scaffold
            // Act: parse generated JSON files
            // Assert: every JSON file has "schema_version" field with value >= 1
        }

        [Fact]
        public void Generated_ids_use_correct_prefix()
        {
            // Arrange: generate scaffold with prefix "testexp_"
            // Act: extract all string constants from generated Ids class
            // Assert: every constant starts with "testexp_"
        }

        [Fact]
        public void Generated_save_store_has_checksum_envelope()
        {
            // Arrange: generate scaffold
            // Act: inspect generated save store code
            // Assert: contains checksum computation and envelope wrapping
        }

        [Fact]
        public void Duplicate_expansion_id_rejected()
        {
            // Arrange: register expansion with ID 01 (Holdfast)
            // Act: attempt to register generated expansion with same ID 01
            // Assert: throws with clear error
        }

        [Fact]
        public void Generated_selftest_verb_is_valid_kebab_case()
        {
            // Arrange: generate scaffold for "SalvageRun"
            // Act: extract CLI verb name
            // Assert: equals "--salvage-run-selftest"
        }
    }

    public class ExpansionRegistryTests
    {
        [Fact]
        public void Register_and_retrieve_module()
        {
            // Arrange/Act/Assert: basic CRUD on registry
        }

        [Fact]
        public void Activation_order_respects_dependencies()
        {
            // 3 modules: C depends on B, B depends on A
            // Expected order: A, B, C
        }

        [Fact]
        public void Circular_dependency_detected()
        {
            // A depends on B, B depends on A → exception
        }

        [Fact]
        public void Diamond_dependency_resolved()
        {
            // D depends on B and C; B depends on A; C depends on A
            // Valid orders: A, B, C, D or A, C, B, D
        }

        [Fact]
        public void Missing_dependency_throws_with_context()
        {
            // Module declares dependency on ID 99 (not registered)
            // Exception message includes both module name and missing ID
        }
    }
}
```

Testing strategy:
- **Generator output tests:** verify generated files meet project standards (compilation, JSON validity, naming)
- **Registry tests:** verify dependency resolution logic (topological sort, cycle detection, error messages)
- **Integration tests:** full pipeline from generation to compilation to self-test execution
- Tests do NOT require Godot runtime — all Core-level, `dotnet test` only

### Verification
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "Expansion"` — all pass
- Generator tool itself builds cleanly
- Generated code from a test run compiles without modification
- No test leaves generated files behind (cleanup in `Dispose`)

### Done-when
- At least 12 tests covering generator output and registry logic
- Generator output verified for: compilation, serialization, schema_version, ID prefixes, checksum pattern
- Registry verified for: registration, retrieval, dependency resolution, error cases
- All tests pass in `dotnet test`
- Tests are self-contained (no filesystem side effects persist after test run)

---

## Summary Table

| Step | Deliverable | Files | Depends On | Risk |
|------|-------------|-------|------------|------|
| 1 | Expansion scaffold template | 1 doc (`docs/expansion-scaffold-template.md`) | — | None |
| 2 | Implementation checklist | 1 doc (`docs/expansion-implementation-checklist.md`) | Step 1 | None |
| 3 | Generator script (dotnet tool) | ~10 (tool project + templates) | Steps 1-2, **and Step 4 for the `--numbered` flag specifically (ordering corrected above)** | Low — tooling only, but see ordering note |
| 4 | `IExpansionModule` + `ExpansionRegistry` + 4 real adapters over Holdfast/DutyRoster/StandingRecord/Crossing | 2 new in `Ashfall.Core/Expansions/` + 4 adapter classes (non-trivial — see Step 4 correction) | — | **Medium** — touches `ExpansionMasterSession.cs`'s constructor/`Load()`/`TickDaily()`, which is live, save-relevant orchestration code, not pure addition |
| 5 | Self-test pattern wired into the *existing* aggregate CLI verb | 1-2 modified (`src/Host/HostCli.cs`, not a new router) | Step 4 | Low-Medium — modifying an existing, working CLI dispatch path risks regressing current selftest coverage if not diffed carefully |
| 6 | Dependency declaration + validation | 1 modified (`ExpansionRegistry`) | Step 4 | Low — validation logic, but only meaningful once a real dependency exists (none do today) |
| 7 | Scaffold + registry tests | 1-2 new in `Ashfall.Core.Tests/Expansions/` | Steps 3-4-6 | None |

**Total new files:** ~15 (tool project + docs + Core interfaces + tests)<br>
**Modified files:** ~7 — corrected upward from the original "~3": `src/Main.cs` (CLI-adjacent wiring
for any newly generated expansion), `src/Host/HostCli.cs` (self-test dispatch, not "Main.cs CLI
routing" as originally stated — that file doesn't own CLI parsing), `Assets/Ashfall.Core/ExpansionMasterSession.cs`
(constructor + `Load()` + `TickDaily()` edits for adapter wiring, not a one-line "adapter" add),
plus 4 new adapter classes for the 4 existing numbered expansions, plus the test project.<br>
**Estimated effort:** 3-4 sessions — **likely understated.** Step 4 alone (writing 4 non-trivial
adapters over non-uniform `Tick` signatures, then safely retrofitting `ExpansionMasterSession`
without breaking `TickDaily()`'s existing call order or `RunAllSelfTests()`) is realistically
1-2 sessions by itself. Recommend budgeting 5-7 sessions total, or explicitly descoping Step 4's
adapter migration to "interface + registry only, zero existing-expansion adapters" for this batch
and tracking the 4 adapters as a follow-up batch.<br>
**Breaking changes:** None planned — existing expansions continue working as long as
`ExpansionMasterSession.cs` edits in Step 4 are additive (new optional registry alongside existing
hard-coded properties, not a replacement of them in this batch).<br>
**Migration cost:** Each existing expansion needs a real `IExpansionModule` adapter that reconciles
a non-uniform `Tick(int day, ...)` signature (weather/temp for Holdfast, occupant lists for Duty
Roster, survivor-candidate lists for Disease) down to the interface's `Tick(int day)` — this is not
"~50 lines each" trivial wrapping in every case; Holdfast and Disease specifically need call-site
data threaded in from outside the module, which the interface as currently drafted has no slot for
(see Risk/Rollback below).<br>

## Risk & Rollback

**Risk this batch under-addressed originally (no Risk/Rollback section existed):**

- **Touching `ExpansionMasterSession.cs` is not risk-free tooling work.** It is the live
  orchestrator for save/tick behavior across 4 numbered expansions + Silent Foundry + Disease.
  Any edit to its constructor signature, `Load()`, or `TickDaily()` is a change to
  production gameplay code, not a template/checklist/generator addition. Steps 1-3 and 7 are
  genuinely low-risk (docs, a new standalone tool, new tests); Steps 4-6 are not, and this batch's
  original "Risk: Low" framing at the top of the document undersells that split.
- **Interface signature gap:** `IExpansionModule.Tick(int day)` cannot represent Holdfast's
  `TickDaily(day, weather, outdoorTemp)` or Disease's `TickDaily(day, diseaseCandidates)` without
  either (a) widening the interface to accept a context object, or (b) having adapters cache
  external state from a separate injection point before `Tick` is called. Resolve this design
  question explicitly in Step 4 before writing adapters — do not discover it mid-implementation.
- **Rollback plan:** because Steps 4-6 modify `ExpansionMasterSession.cs`, land them as their own
  commit(s) separate from Steps 1-3/7, behind no feature flag is needed *if* the registry is
  additive-only (existing named properties `Holdfast`/`DutyRoster`/`StandingRecord`/`Crossing`
  keep working unchanged; the registry is a second, parallel access path). If a regression in
  `TickDaily()` ordering or `RunAllSelfTests()` output is discovered after merge, revert the
  `ExpansionMasterSession.cs` commit specifically — it should not require reverting Steps 1-3/7's
  tooling/docs, since those have no runtime coupling to it.
- **Test coverage before touching orchestration code:** before Step 4 lands any
  `ExpansionMasterSession.cs` edit, capture the current output of
  `ExpansionMasterSession.RunAllSelfTests()` and the current `--expansions-selftest` CLI output as
  a golden baseline, so a diff can catch silent coverage loss.

---

## Review Notes (Corrected)

This document was adversarially reviewed against the actual ASHFALL codebase
(`/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`). Findings and fixes applied:

1. **`GameBootstrap` does not exist anywhere in the codebase** (confirmed by full-repo search —
   zero matches). The original plan's Motivation section, Phase 4 of the checklist, and the
   `IExpansionModule` XML doc comment all referenced it as the wiring target. All references have
   been corrected to point at the real host god-object, `src/Main.cs` (a single ~6.5k-line
   `partial class Main` with per-subsystem `SetupXxx`/`SaveXxx`/`FlushXxxIfDirty` triads, per
   `AGENTS.md` H7), and, for numbered expansions specifically, `Assets/Ashfall.Core/ExpansionMasterSession.cs`.
2. **Two distinct host-level aggregators exist and were conflated in the original plan:**
   `ExpansionMasterSession` (Core-level, hard-codes exactly Holdfast/DutyRoster/StandingRecord/
   Crossing + Silent Foundry + Disease, constructed via `.Load()`) and `ExpansionHostSession`
   (`src/Host/ExpansionHostSession.cs`, the thin Godot-host wrapper actually instantiated by
   `Main.cs.SetupExpansions()`, covering a different, larger set: Waystation, Layouts, Memory,
   SiteEncounters, RecordQuests, Vouch, Greenhouse, Arbitration, Ledger, CrossingQuests,
   Generational, Epilogue, SilentFoundry, Disease). The scaffold template and checklist now call
   out that a new expansion must be placed deliberately into one or the other, not both by default.
3. **`IExpansionModule` from "Batch 62" was checked and does not exist anywhere in the repository**
   (zero search hits for the type name). The plan's framing as "standardize" implied prior partial
   implementation; corrected to state plainly this is new infrastructure proposed from scratch, and
   sized Step 4's risk/effort accordingly (it requires real adapter-writing over non-uniform `Tick`
   signatures, not thin wrapping).
4. **The four numbered expansions were verified against `ExpansionMasterSession.cs`:** Holdfast (01),
   Duty Roster (02), Standing Record (03), and Nobody's Charter/Crossing (04) are confirmed present
   as named properties (`Holdfast`, `DutyRoster`, `StandingRecord`, `Crossing`) with direct,
   non-generic construction and tick calls — there is no dynamic registry today, matching the
   `IExpansionModule` correction above.
5. **Step 6's worked example ("Crossing depends on Holdfast") was unverified and false.**
   `CrossingSession`'s constructor takes only `VouchAccessSystem` and `CrossingCatalog` — no
   Holdfast dependency exists in code today. Replaced with an explicitly hypothetical example.
6. **Step 5 proposed creating an aggregate `--expansions-selftest` CLI verb that already exists**
   in `src/Host/HostCli.cs:139`, alongside individual verbs like `--holdfast-selftest` and
   `--duty-roster-selftest`. Corrected Step 5's Goal/Implementation/Verification/Done-when to
   describe *rewiring* the existing verb rather than *creating* it, and added an explicit
   collision check against the existing verb table before adding new per-module verbs.
7. **Illogical ordering:** Step 3 (generator) originally claimed its `--numbered` flag would
   "register in ExpansionMasterSession," but that requires Step 4's registry, which is defined
   two steps later in the same document. Corrected to either sequence Step 4 before Step 3 in
   execution or descope Step 3's first pass to standalone expansions only.
8. **Missing Risk & Rollback section entirely** — the original document's header claimed
   "Risk: Low — process/tooling/templates, not changes to running gameplay code" for the whole
   batch, which is true for Steps 1-3 and 7 but false for Steps 4-6 (they edit live orchestration
   code in `ExpansionMasterSession.cs`). Added a dedicated Risk & Rollback section above,
   distinguishing the two risk tiers and specifying a golden-baseline-before-edit rollback strategy.
9. **Vague/unrunnable verification tightened:** "Template validated against 2+ existing
   expansions" now requires naming which two, concretely, in the template's own footer, so the
   claim is checkable later rather than asserted once and forgotten.
10. **Effort estimate corrected:** the original "3-4 sessions" and "~50 lines each" adapter
    estimate did not account for the interface-signature mismatch between `IExpansionModule.Tick(int day)`
    and the real, non-uniform tick signatures of Holdfast/Disease. Revised to 5-7 sessions with an
    explicit design decision required before adapter-writing begins, or a recommendation to descope
    the 4 adapters into a follow-up batch.
