# ASHFALL — Quality Roadmap Batch 86

## Theme: Dependency Injection Container — Replace Manual Wiring in Main.cs

**Priority:** MEDIUM-HIGH
**Risk:** Medium — changes initialization architecture; incorrect registration order could surface latent bugs
**Estimated Effort:** 4–5 focused sessions
**Depends On:** Nothing directly (but benefits from Batch 85 FSM work for lifecycle hooks)
**Unlocks:** Easier system addition, testable isolation, elimination of Main.cs triad drift (H7)

---

## Motivation

**CORRECTED (see Review Notes below): the line/method counts below were off; corrected figures verified this session are used throughout this document.**

`Main.cs` is a **7014-line** partial class (verified via `wc -l src/Main.cs`, not the originally stated 6640) that manually constructs and wires systems across **38 `SetupXxx` methods** (verified via `grep -c "void Setup" src/Main.cs`, not 31), **~29 `SaveXxx` methods** (verified via `grep -n "void Save[A-Z]" src/Main.cs` = 29 named methods plus the `SaveAll` orchestrator, not 24), and **17 `FlushXxxIfDirty` methods** (this count matches the original claim). Every new system requires editing Main.cs in at least three places:

1. **Setup** — construct the system, inject dependencies, wire events
2. **Save** — add `CaptureState()` call to `SaveAll` method
3. **Flush** — add dirty-check flush to the frame loop

This triad is undocumented and unenforced. Missing any one leg causes silent bugs:
- Missing Save → system state lost on save/load (data loss)
- Missing Flush → deferred writes never persist (silent corruption)
- Missing Setup → null reference at runtime (crash)

Additional problems:

| Problem | Impact |
|---------|--------|
| Order-dependent construction | System A depends on System B; if Setup order changes, null ref at boot |
| No dependency graph validation | Circular dependencies crash at runtime, not compile time |
| Testing requires full Main.cs | Integration tests must replicate the full wiring to test one system |
| 38 Setup methods with 3–8 parameters each (revised from the original "82+ constructor calls" — that figure was not independently verified this session and is not used elsewhere in this plan; do not rely on it for scoping Step 4) | Typo in parameter order compiles but misbehaves |
| Adding optional dependencies requires touching all call sites | Feature flags balloon parameter lists |
| No lifetime management | All systems are singletons by convention, not enforcement |

**Verified this session — no DI-like pattern exists anywhere in the codebase.** A repository-wide search for `IServiceProvider`, `ServiceCollection`, `ServiceRegistry`, and `IServiceContainer` returned zero matches. This confirms the plan's premise: there is nothing to conflict with or migrate away from except manual construction in `src/Main.cs`. (The project does already have `IEventBus`/`SimpleEventBus` at `Assets/Ashfall.Core/Events/IEventBus.cs`, which is unrelated to DI but worth knowing about — it's a pub/sub bus, not a container, and per AGENTS.md's Event System section it is "defined, underused." It has no bearing on this batch.)

**Also verified — `GameBootstrap` does not exist as an active type.** A `class GameBootstrap` was found only in `_quarantine_legacy/Assets/Scripts/Core/GameBootstrap.cs` — a quarantined legacy file outside both `Assets/_Game/` and `src/`. It is not part of the active Godot host and should not be referenced as a migration target or example anywhere in this plan. **The active Godot host's construction/wiring lives entirely in `src/Main.cs` via `_Ready()`** (Godot's actual lifecycle entry point — not a method literally named `Boot()`; see Step 5 correction below).

**Goal:** Introduce a lightweight, engine-agnostic `ServiceRegistry` in Core that:
- Explicitly registers all systems with their dependencies
- Validates the dependency graph at startup (cycle detection, missing registrations)
- Resolves systems in correct order automatically
- Eliminates manual construction — systems declare what they need, registry provides it
- Remains deterministic (no runtime reflection scanning, no assembly scanning)
- Supports the `CaptureState/RestoreState` contract via a `ISaveable` discovery mechanism

**Construction-order risk — explicitly called out per review request:** `Main.cs`'s 38 `SetupXxx` methods currently run in a fixed, hand-written order (whatever order they're called in `_Ready()`), and that order almost certainly encodes real dependencies that are *not* declared anywhere — a later `SetupXxx` may silently rely on an earlier one having already run (e.g., a system that reads another system's field during construction, not just via constructor injection). Migrating to a DI container that resolves order via topological sort of *declared* constructor dependencies is only safe if **every implicit ordering dependency gets made explicit as a constructor parameter first**. If even one hidden dependency is missed, the container may resolve systems in a different order than today's hand-written sequence, and a system that depended on running after some other undeclared system could construct against stale/default state instead of crashing loudly — a silent correctness bug, not a crash, which is *harder* to catch in testing than the null-ref crashes this plan calls out as today's failure mode. **Before Step 4 (CoreServiceModule), audit each of the 38 Setup methods for order-sensitive side effects beyond constructor parameters** (e.g., one system registering itself as a listener on another during Setup, or reading a static/shared field populated by an earlier Setup) — this audit is not currently a step in this plan and should be added.

---

## Step 1 — Evaluate DI Approaches and Select Strategy

**Goal:** Choose between Microsoft.Extensions.DependencyInjection, a custom lightweight container, or a pure-registration-function approach. Document the decision with rationale.

**Implementation:**

Evaluation matrix:

| Criterion | MS.DI | Custom Container | Pure Registration |
|-----------|-------|------------------|-------------------|
| Engine-agnostic | Yes (netstandard2.1) | Yes | Yes |
| No reflection scanning | Configurable | By design | By design |
| Deterministic resolution order | Yes (registration order) | Yes | Yes |
| Cycle detection at startup | No (throws at resolve) | Implementable | Implementable |
| Save/load integration | No built-in | Designable | Designable |
| Dependency count | 1 NuGet package | 0 | 0 |
| Godot .NET compatibility | Yes | Yes | Yes |
| Learning curve for contributors | Low (industry standard) | Medium | Low |
| Graph visualization | No | Implementable | No |

**Recommendation:** Custom lightweight container.

Rationale:
- MS.DI adds a NuGet dependency and its service provider is opaque (no graph introspection)
- We need: cycle detection at registration time, `ISaveable` enumeration, deterministic order, and graph dump for debugging
- Pure registration functions lack the ability to enforce singleton semantics or validate completeness
- Custom container is ~200 lines, zero dependencies, fully testable, fits `Ashfall.Core` constraints
- The container itself has `CaptureState/RestoreState` (serializes registration metadata for save-file diagnostics)

Decision document: write `Assets/Ashfall.Core/DI/DESIGN.md` (internal, not shipped) explaining the choice.

**Verification:**
- Decision is documented with pros/cons
- No code written yet (design phase)
- Confirm netstandard2.1 compatibility of chosen approach

**Done when:** Decision documented; team (user) has approved the approach; no code conflicts with existing architecture.

---

## Step 2 — Design ServiceRegistry API in Core

**Goal:** Define the public API for the DI container: registration, resolution, validation, and lifetime management. Engine-agnostic, deterministic, explicit-only.

**Implementation:**

```
Assets/Ashfall.Core/DI/
├── ServiceRegistry.cs          # The container
├── ServiceDescriptor.cs        # Registration metadata
├── ServiceLifetime.cs          # Singleton | Transient
├── DependencyGraphValidator.cs # Cycle detection, missing deps
├── ISaveable.cs                # Marker interface for save-participating systems
├── RegistrationModule.cs       # Base class for grouped registrations
└── ResolutionException.cs      # Typed exception for resolution failures
```

`ServiceRegistry` API:

```csharp
namespace Ashfall.Core.DI;

public sealed class ServiceRegistry
{
    // Registration
    public ServiceRegistry Register<TInterface, TImpl>(ServiceLifetime lifetime = ServiceLifetime.Singleton)
        where TImpl : TInterface;
    public ServiceRegistry RegisterInstance<TInterface>(TInterface instance);
    public ServiceRegistry RegisterFactory<TInterface>(Func<ServiceRegistry, TInterface> factory,
        ServiceLifetime lifetime = ServiceLifetime.Singleton);

    // Module registration (grouped)
    public ServiceRegistry ApplyModule(RegistrationModule module);

    // Validation (call after all registrations, before first resolve)
    public ValidationResult ValidateGraph();  // Returns errors, warnings, dependency order

    // Resolution
    public T Resolve<T>();
    public object Resolve(Type serviceType);
    public IReadOnlyList<T> ResolveAll<T>();  // All registrations implementing T

    // Discovery
    public IReadOnlyList<ISaveable> GetSaveables();  // All registered ISaveable systems, in registration order
    public IReadOnlyList<ServiceDescriptor> GetDescriptors();  // For diagnostics/debugging

    // Lifecycle
    public void Dispose();  // Disposes all IDisposable singletons in reverse registration order
}
```

`ServiceLifetime`:
```csharp
public enum ServiceLifetime
{
    Singleton,  // One instance, cached after first resolve
    Transient   // New instance per resolve (rare — most systems are singletons)
}
```

`ValidationResult`:
```csharp
public sealed class ValidationResult
{
    public bool IsValid { get; }
    public IReadOnlyList<string> Errors { get; }    // Missing deps, cycles
    public IReadOnlyList<string> Warnings { get; }  // Unused registrations, overshadowed
    public IReadOnlyList<Type> ResolutionOrder { get; }  // Topologically sorted
}
```

Design rules:
- **No reflection scanning.** Every registration is explicit via `Register<T,U>()` or a `RegistrationModule`
- **No lazy resolution.** `ValidateGraph()` verifies completeness before any `Resolve()` call
- **Deterministic order.** Singletons are created in topological dependency order; ties broken by registration order
- **Constructor injection only.** Systems declare dependencies as constructor parameters; registry matches by type
- **Single implementation per interface** (unless using `ResolveAll<T>` for multi-registration like event handlers)
- **Thread-safety not required** — game is single-threaded on main loop

**Verification:**
- API compiles in isolation (no implementation bodies yet — interfaces + stubs)
- `dotnet build Assets/Ashfall.Core/Ashfall.Core.csproj` succeeds
- Design covers: Core systems, host adapters (ILog, IFileIO), save participation, optional dependencies

**Done when:** All types in `Assets/Ashfall.Core/DI/` compile; XML docs describe behavior contracts; no engine references.

---

## Step 3 — Implement ServiceRegistry with Cycle Detection

**Goal:** Full working implementation of the container: registration storage, topological sort for resolution order, cycle detection via DFS, singleton caching, and factory invocation.

**Implementation:**

Core algorithms:

**Registration storage:**
- `Dictionary<Type, ServiceDescriptor>` for single registrations
- `ServiceDescriptor` holds: interface type, implementation type, lifetime, factory (if any), cached instance (if singleton resolved)

**Dependency discovery (constructor analysis):**
- For each registered `TImpl`, inspect its constructors via `typeof(TImpl).GetConstructors()`
- Select the constructor with the most parameters (greediest)
- Each parameter type becomes a dependency edge in the graph
- If a parameter type has no registration → error in `ValidateGraph()`
- Optional dependencies: parameters with a default value of `null` are soft edges (warning if missing, not error)

**Cycle detection (Kahn's algorithm):**
```
1. Build adjacency list from dependency edges
2. Compute in-degree for each node
3. Enqueue nodes with in-degree 0
4. Process queue: decrement in-degree of dependents, enqueue when 0
5. If not all nodes processed → cycle exists among remaining nodes
6. Report cycle path for diagnostics
```

**Resolution:**
```
1. Assert ValidateGraph() was called and passed
2. Look up ServiceDescriptor for requested type
3. If singleton and already cached → return cached
4. If factory → invoke factory(this), cache if singleton
5. If type → resolve all constructor parameters recursively, invoke constructor
6. Cache if singleton
7. Return instance
```

**ISaveable enumeration:**
- After all singletons resolved, filter for `ISaveable` implementations
- Return in registration order (deterministic save order)
- This replaces the manual `SaveAll` method's explicit list

**Error handling:**
- `ResolutionException` with clear message: "Cannot resolve IMarketSystem: no registration found. Did you forget to register MarketSystem in CoreServiceModule?"
- Cycle error: "Dependency cycle detected: RadiationSystem → NeedsSystem → RadiationSystem. Break the cycle by extracting shared logic into a separate service."

**Verification:**
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` compiles
- Unit tests (Step 7): registration, resolution, cycle detection, missing dep, singleton caching
- Memory: no unexpected allocations after initial resolution phase

**Done when:** `ServiceRegistry` resolves a 10-service dependency chain correctly; cycle detection catches circular deps; singletons are cached; validation reports all errors before first resolve.

---

## Step 4 — Create CoreServiceModule (All Core System Registrations)

**Goal:** Define a `RegistrationModule` that registers Core systems with their correct dependencies, replacing the implicit knowledge currently embedded in Main.cs Setup methods.

**CORRECTED SCOPE:** The "82+ Core systems" figure was not verified this session and should not be treated as reliable. What was verified: **38 `SetupXxx` methods exist in `src/Main.cs`** (some, like `SetupExpeditionCombatHandoff(CombatHostSession combat)`, take a parameter and aren't simple no-arg construction calls — audit each one individually rather than assuming uniform shape). Before writing `CoreServiceModule`, produce an actual enumerated list of the systems constructed across those 38 methods (by reading `src/Main.cs` fully, not estimating) — do not carry forward an unverified "82+" figure into a Done-when count, since "registers 82+ systems without validation errors" is unverifiable if the true number is unknown.

```csharp
namespace Ashfall.Core.DI;

public sealed class CoreServiceModule : RegistrationModule
{
    public override void Configure(ServiceRegistry registry)
    {
        // === Port interfaces (implementations provided by host module) ===
        // ILog, IFileIO, IJsonSerializer, IClock, ISeededRng — registered by host

        // === Foundation systems (no game dependencies) ===
        registry.Register<ICatalogIntegrityValidator, CatalogIntegrityValidator>();
        registry.Register<ISaveChecksum, SaveChecksum>();

        // === Data systems (depend on IFileIO, IJsonSerializer) ===
        registry.Register<ICatalogRegistry, CatalogRegistry>();
        registry.Register<IItemCatalog, ItemCatalog>();
        registry.Register<ILocationCatalog, LocationCatalog>();
        registry.Register<IRecipeCatalog, RecipeCatalog>();
        // ... all catalog systems

        // === Simulation systems (depend on catalogs + ports) ===
        registry.Register<INeedsSystem, NeedsSystem>();
        registry.Register<IRadiationSystem, RadiationSystem>();
        registry.Register<IWeatherSystem, WeatherSystem>();
        registry.Register<IMarketSystem, MarketSystem>();
        registry.Register<ICombatSystem, TacticalCombatSystem>();
        registry.Register<IExpeditionSystem, ExpeditionSystem>();
        registry.Register<IMedicalSystem, MedicalSystem>();
        registry.Register<INarrativeSystem, NarrativeSystem>();
        // ... all Core systems actually constructed by the 38 verified SetupXxx methods —
        // enumerate them from src/Main.cs directly; do not guess at a total count

        // === Expansion coordination ===
        registry.Register<IExpansionMasterSession, ExpansionMasterSession>();
    }
}
```

Migration strategy:
- Extract the constructor calls from each of the **38 verified** `SetupXxx` methods in Main.cs
- Each system's constructor parameters become its DI dependencies
- Systems that currently receive primitives (file paths, config values) get wrapped in a `GameConfig` DTO registered as a singleton
- Systems with circular dependencies are identified by `ValidateGraph()` and refactored (extract shared concern)
- **Also audit for order-sensitive side effects that are not constructor dependencies** — per the Motivation section's construction-order risk callout, this is a distinct failure mode from missing/circular dependencies and `ValidateGraph()` cannot catch it (it only sees declared constructor parameters, not runtime side effects during Setup)

Candidate circular dependencies to check for during this audit (**not independently verified this session** — these are plausible candidates based on domain knowledge in AGENTS.md, not confirmed code paths; verify each one by reading the actual constructors before relying on this list):
- `NeedsSystem` ↔ `RadiationSystem` (radiation affects needs, needs thresholds affect radiation sensitivity) — if confirmed, break via a shared `ISurvivorState` read-only interface
- `MarketSystem` ↔ `ExpeditionSystem` (expeditions generate trade goods, market prices affect expedition value) — if confirmed, break via event-based decoupling (market publishes price changes via the existing `IEventBus`/`SimpleEventBus`, expedition subscribes)

**Verification:**
- `CoreServiceModule` compiles and registers all systems enumerated from the actual `src/Main.cs` audit (not an assumed count)
- `registry.ValidateGraph()` returns `IsValid == true` (no cycles, no missing deps)
- Registration count matches the count produced by the Step 4 audit — state the actual number reached, don't carry forward "82+"
- Resolution order is deterministic (same order every run)
- **New verification not in the original draft:** for each of the 38 Setup methods, confirm (by reading, not assuming) whether it has any order-sensitive side effect beyond constructor injection (event subscription during Setup, shared/static field reads, etc.). Document findings even if none are found — "audited, none found" is a valid and useful result.

**Risk / Rollback:** This step doesn't change runtime behavior by itself (it's a registration module, not yet wired into `_Ready()` — that's Step 5), so it's low-risk in isolation. The risk surfaces in Step 5/6 when resolution order actually replaces the hand-written Setup order. Rollback here is trivial: delete `CoreServiceModule.cs`, nothing else references it yet.

**Done when:** All Core systems identified in the Step 4 audit are registered in `CoreServiceModule`; `ValidateGraph()` passes; no circular dependencies remain (all broken by extraction or events); the order-sensitive-side-effect audit is complete and its findings are written down (even if the finding is "none found").

---

## Step 5 — Create HostServiceModule (Godot-Specific Adapters)

**Goal:** Define the host-side registration module that provides Godot-specific implementations of port interfaces and host services (save stores, file paths, logging).

**CORRECTED SCOPE:** This session verified **27 save-store files** exist under the repo (via `find . -iname "*SaveStore*.cs"`, excluding tests) — not the 22 claimed below — and **27 host-session files** exist under `src/Host/` — not the 34 claimed below. Both counts happen to be 27; that is coincidental and should be re-verified independently rather than assumed to stay equal as the codebase changes. The code sketch below also calls the entry method `Boot()`, but **Godot's actual lifecycle entry point in this codebase is `public override void _Ready()`** (`src/Main.cs:274`) — there is no method literally named `Boot()` today. Any DI bootstrap code must be wired into `_Ready()`, not a new `Boot()` method invented for this plan (unless the team explicitly chooses to introduce one and call it from `_Ready()`, which is a naming choice, not a requirement).

**Implementation:**

```csharp
namespace AtomicWar.GodotApp.DI;

public sealed class GodotHostServiceModule : RegistrationModule
{
    private readonly Node _sceneRoot;
    private readonly string _savePath;
    private readonly string _dataPath;

    public GodotHostServiceModule(Node sceneRoot, string savePath, string dataPath)
    {
        _sceneRoot = sceneRoot;
        _savePath = savePath;
        _dataPath = dataPath;
    }

    public override void Configure(ServiceRegistry registry)
    {
        // === Port implementations ===
        registry.RegisterInstance<ILog>(new GodotLog());
        registry.RegisterInstance<IFileIO>(new CoreFileIO());  // System.IO (Godot uses regular filesystem)
        registry.RegisterInstance<IJsonSerializer>(new SystemTextJsonSerializer());
        registry.RegisterInstance<IClock>(new CoreClock());
        registry.RegisterFactory<ISeededRng>(r => new CoreSeededRng(GetSeedFromConfig()));

        // === Host configuration ===
        registry.RegisterInstance(new GamePaths(_savePath, _dataPath));
        registry.RegisterInstance(new GameConfig(LoadConfigFromFile()));

        // === Save stores (27 stores verified this session — enumerate via
        // `find . -iname "*SaveStore*.cs"` at implementation time, do not assume 22) ===
        registry.Register<IExpeditionSaveStore, ExpeditionSaveStore>();
        registry.Register<IMedicalSaveStore, MedicalSaveStore>();
        registry.Register<INarrativeSaveStore, NarrativeSaveStore>();
        registry.Register<IWorldSaveStore, WorldSaveStore>();
        registry.Register<IJournalSaveStore, JournalSaveStore>();
        // ... remaining verified save stores

        // === Host sessions (27 files verified under src/Host/ this session —
        // enumerate directly, do not assume 34) ===
        registry.Register<IHoldfastHostSession, HoldfastHostSession>();
        registry.Register<ISurvivorsHostSession, SurvivorsHostSession>();
        // ... remaining verified host sessions

        // === UI panels (resolved on-demand, transient) ===
        registry.Register<IInventoryPanel, InventoryPanel>(ServiceLifetime.Transient);
        // ... UI panels that need DI
    }
}
```

Integration in `Main.cs` (reduced):
```csharp
// Before: 38 SetupXxx methods (verified count), several hundred lines of manual construction
// After (wired from the existing _Ready(), not a new Boot() method):
private ServiceRegistry _services;

public override void _Ready()
{
    // ... existing CLI-arg handling and early-exit paths stay as-is (see src/Main.cs:274-314) ...

    _services = new ServiceRegistry();
    _services.ApplyModule(new CoreServiceModule());
    _services.ApplyModule(new GodotHostServiceModule(this, _savePath, _dataPath));

    var validation = _services.ValidateGraph();
    if (!validation.IsValid)
    {
        foreach (var error in validation.Errors)
            GD.PrintErr($"DI: {error}");
        GetTree().Quit(1);
        return;
    }

    // All systems resolved in correct dependency order
    _systems = _services.ResolveAll<IGameSystem>();
    _saveables = _services.GetSaveables();
}
```

Save integration:
```csharp
// Before: ~29 explicit SaveXxx methods (verified count) plus SaveAll orchestrator
// After:
private void SaveAll()
{
    foreach (var saveable in _saveables)
    {
        var state = saveable.CaptureState();
        var store = _services.Resolve(saveable.StoreType);
        store.Save(state);
    }
}
```

**Verification:**
- `dotnet build Ashfall.csproj` — Godot host compiles with new module
- `godot --headless --path . -- --bridge-selftest` — exits 0 (per AGENTS.md, this now only prints a removal notice and exits 0; it does not actually exercise DI resolution or boot into the app loop — this verifies "process exits cleanly," not "DI wiring works." Add a more meaningful check, e.g. asserting `_services.ValidateGraph().IsValid` via a dedicated self-test verb, before relying on `--bridge-selftest` as evidence this step works)
- All 27 save stores resolve correctly (verified count — re-run the `find` command above before finalizing, this drifts)
- All 27 host sessions resolve with their dependencies (verified count — same caveat)
- Boot time is not significantly impacted (< 100ms overhead from DI resolution) — **specify how this will be measured** (a `Stopwatch` around `ValidateGraph()`+`ResolveAll()`, logged at `_Ready()`); "not significantly impacted" with no measurement method is not verifiable as written

**Risk / Rollback:** This is where the construction-order risk from the Motivation section becomes real — replacing the hand-written `_Ready()` Setup sequence with topological-sort resolution order changes *when* each of the 38 systems gets constructed relative to the others. If the Step 4 audit missed an order-sensitive side effect, this step is where it surfaces, likely as a subtle runtime bug rather than a compile error. Rollback plan: keep the legacy manual-construction code path in `_Ready()` behind a feature flag (e.g. an env var or CLI arg) for at least one full verification pass, so a regression can be reverted by flipping the flag rather than re-writing removed Setup methods from git history.

**Done when:** `GodotHostServiceModule` registers all host-specific services (27 save stores, 27 host sessions — verified counts, re-confirm at implementation time); `_Ready()` boots via DI instead of manual construction for the migrated systems; the feature-flag rollback path exists and has been exercised at least once (flip it off, confirm the legacy path still boots).

---

## Step 6 — Migrate First 10 Setup Methods from Main.cs to DI

**Goal:** Convert 10 of the **38 verified** `SetupXxx` methods to DI-based resolution, proving the pattern works end-to-end and reducing Main.cs by an amount to be measured (see Verification below — do not commit to "~250 lines" without measuring the actual 10 methods selected, since method sizes vary; the original 31-method premise this estimate was based on is itself wrong).

**Implementation:**

Selected systems (chosen for low coupling and clear dependency graphs — **the dependency lists below are illustrative and were not verified against the real constructors this session; read each constructor before finalizing the registration**):

| # | Setup Method | System | Dependencies |
|---|-------------|--------|--------------|
| 1 | `SetupWeather` | `WeatherSystem` | `ISeededRng`, `IClock`, `GameConfig` |
| 2 | `SetupNeeds` | `NeedsSystem` | `IClock`, `ILog`, `GameConfig` |
| 3 | `SetupRadiation` | `RadiationSystem` | `ISeededRng`, `IClock`, `ILog` |
| 4 | `SetupMarket` | `MarketSystem` | `ISeededRng`, `IClock`, `IItemCatalog`, `ILog` |
| 5 | `SetupCrafting` | `CraftingSystem` | `IRecipeCatalog`, `IItemCatalog`, `ILog` |
| 6 | `SetupJournal` | `JournalSystem` | `IClock`, `ILog` |
| 7 | `SetupMedical` | `MedicalSystem` | `IClock`, `ISeededRng`, `ILog`, `INeedsSystem` |
| 8 | `SetupWeapons` | `WeaponSystem` | `IItemCatalog`, `ISeededRng` |
| 9 | `SetupHygiene` | `HygieneSystem` | `IClock`, `INeedsSystem` |
| 10 | `SetupMorale` | `MoraleSystem` | `INeedsSystem`, `IRadiationSystem`, `IClock` |

Migration per system:
1. Remove the `SetupXxx` method body (keep as empty stub with `// DI-resolved` comment)
2. Ensure the system's constructor declares its dependencies as interface parameters
3. Verify the system is registered in `CoreServiceModule`
4. Verify all dependencies are registered (either in Core or Host module)
5. Add `ISaveable` implementation to the system (if it has `CaptureState/RestoreState`)
6. Remove the corresponding `SaveXxx` call from the manual `SaveAll` (now handled by ISaveable enumeration)
7. Remove the corresponding `FlushXxxIfDirty` if the flush is now handled by the system internally
8. **New step, added per review:** confirm the migrated system's position in the *current* hand-written `_Ready()` call order, and check whether anything between the old position and where DI resolution will now construct it depends on side effects from the migrated system (or vice versa) — this is the concrete instance of the Motivation section's construction-order risk, applied to these specific 10 systems

Parallel operation:
- Non-migrated systems (**28 remaining**, corrected from the original "21" which was derived from the wrong 31-method total) still use manual Setup/Save/Flush
- `Main.cs` has both paths: DI-resolved systems via `_services.ResolveAll<IGameSystem>()` and manually-wired legacy systems
- This coexistence is temporary (remaining 28 migrated in future batches)

Triad drift prevention:
- `ISaveable` marker interface means: if a system is registered and implements `ISaveable`, it automatically participates in save — no manual `SaveXxx` needed
- `ITickable` marker interface means: if a system implements `ITickable`, it automatically receives ticks — no manual tick registration needed
- This structurally prevents the "Setup without Save" bug class

**Verification:**
- `dotnet build Ashfall.csproj` — compiles cleanly
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — all tests pass (baseline ~2120 tests verified this session — re-check the count before and after)
- `godot --headless --path . -- --data-integrity-selftest` — 0 errors
- `godot --headless --path . -- --bridge-selftest` — exits 0 (again, only confirms clean process exit post-shim-removal, not DI correctness — see Step 5's note)
- Save/load round-trip: save game, reload, verify all 10 systems restore correctly
- Main.cs line-count delta: measure actual lines removed for these specific 10 methods via `git diff --stat src/Main.cs` after the change — report the real number, do not pre-commit to an estimate

**Risk / Rollback:** Same underlying risk as Step 5 (construction-order change), scoped to 10 systems instead of all 38. Because this step runs both paths in parallel (10 DI-resolved, 28 manual), a regression can be isolated to one of the 10 migrated systems rather than the whole boot sequence — this parallel-operation design is a reasonable mitigation and should be preserved, not collapsed early for cleanliness. Rollback for any single system: move its `SetupXxx` body back out of the DI module and re-paste it into `_Ready()` in its original position (keep the removed code in the commit that deletes it, not squashed away, so this is a straightforward revert).

**Done when:** 10 systems resolve via DI; their Setup/Save/Flush methods are removed from Main.cs; save/load works identically; no regressions in existing tests; the per-system order-sensitivity check (step 8 above) has been performed and documented for each of the 10.

---

## Step 7 — Write DI Container Tests

**Goal:** Comprehensive test coverage for `ServiceRegistry`: registration, resolution, validation, error cases, singleton semantics, and integration with the save system.

**Implementation:**

Test file: `Ashfall.Core.Tests/DependencyInjectionTests.cs`

**Registration & Resolution tests:**

| Test | Asserts |
|------|---------|
| `Register_AndResolve_ReturnsSameType` | `Resolve<IFoo>()` returns instance of `FooImpl` |
| `RegisterSingleton_ResolveTwice_SameInstance` | Two `Resolve<T>()` calls return `ReferenceEquals` true |
| `RegisterTransient_ResolveTwice_DifferentInstances` | Two resolves return different objects |
| `RegisterInstance_ReturnsExactInstance` | Pre-created object returned as-is |
| `RegisterFactory_InvokedOnResolve` | Factory lambda called, result returned |
| `RegisterFactory_Singleton_CachedAfterFirst` | Factory called once, cached thereafter |
| `Resolve_Unregistered_ThrowsResolutionException` | Clear error message with type name |
| `Resolve_WithDependencies_InjectsCorrectly` | Constructor params satisfied from registry |
| `Resolve_DeepChain_WorksCorrectly` | A → B → C → D all resolved in order |

**Validation tests:**

| Test | Asserts |
|------|---------|
| `ValidateGraph_NoCycles_ReturnsValid` | Simple chain validates successfully |
| `ValidateGraph_DirectCycle_ReportsError` | A → B → A detected and reported |
| `ValidateGraph_IndirectCycle_ReportsError` | A → B → C → A detected with full path |
| `ValidateGraph_MissingDep_ReportsError` | A depends on unregistered B → error |
| `ValidateGraph_OptionalDep_ReportsWarning` | Nullable param missing → warning not error |
| `ValidateGraph_ResolutionOrder_Topological` | Order respects dependency edges |
| `ValidateGraph_DeterministicOrder_AcrossRuns` | Same registrations → same order always |

**Module tests:**

| Test | Asserts |
|------|---------|
| `ApplyModule_RegistersAllServices` | Module's Configure called, services available |
| `ApplyModule_TwoModules_NoConflict` | Core + Host modules compose correctly |
| `ApplyModule_DuplicateRegistration_LastWins` | Host overrides Core for same interface |

**ISaveable integration tests:**

| Test | Asserts |
|------|---------|
| `GetSaveables_ReturnsOnlyISaveableRegistrations` | Non-saveable systems excluded |
| `GetSaveables_OrderMatchesRegistration` | Deterministic save order |
| `SaveableRoundTrip_AllSystemsRestore` | CaptureState on all → RestoreState on all → state matches |

**Error recovery tests:**

| Test | Asserts |
|------|---------|
| `Resolve_AfterDispose_Throws` | Registry disposal prevents further resolution |
| `Dispose_DisposesInReverseOrder` | IDisposable singletons disposed last-first |
| `ConstructorThrows_WrapsInResolutionException` | System constructor failure → clear error |

**Verification:**
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — all 28+ new tests pass
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — 0 errors, 0 warnings
- No mocking framework required (use simple test doubles: `FakeLog`, `FakeFileIO`)
- Tests are fast (< 100ms total, no I/O)

**Done when:** All tests listed pass; DI container behavior is fully characterized; edge cases documented via test names; no existing tests broken.

---

## Summary

| Step | Deliverable | Files | Risk | Depends On |
|------|-------------|-------|------|------------|
| 1 | DI approach decision | Design doc (internal) | None | — |
| 2 | ServiceRegistry API | `Assets/Ashfall.Core/DI/` (6 files) | Low | Step 1 |
| 3 | Full implementation | Same files, logic complete | Medium | Step 2 |
| 4 | CoreServiceModule + order-sensitivity audit of all 38 Setup methods | `CoreServiceModule.cs` | Medium | Step 3 |
| 5 | GodotHostServiceModule | `GodotHostServiceModule.cs`, `_Ready()` refactor in `src/Main.cs` (not a new `Boot()` method) | **Medium-High** (construction order actually changes here — see Risk/Rollback in Step 5) | Steps 3–4 |
| 6 | First 10 of 38 systems migrated | `src/Main.cs` (line delta to be measured, not assumed), 10 system constructors | Medium | Steps 4–5 |
| 7 | Test suite | `DependencyInjectionTests.cs` (28+ tests) | Low | Steps 1–6 |

**Total new files:** ~8 in Core + 1 in Godot host + 1 test file
**Total modified files:** ~15 (`src/Main.cs`, 10 system classes, save stores, project files) — not re-verified against the real 38-method total; treat as an estimate
**Breaking changes:** None externally (internal architecture refactor) — **caveat added:** this holds only if construction order is preserved exactly; if the Step 4 audit surfaces an undeclared ordering dependency that can't be resolved cleanly, that is itself a finding requiring a design decision, not something to route around silently
**Rollback plan:** DI container is additive for Steps 1–4; non-migrated systems continue using manual wiring. Steps 5–6 carry real behavioral risk (construction order change) and should ship behind a feature flag for at least one verification cycle, per the Risk/Rollback notes in those steps — "both paths coexist" is necessary but not sufficient for safe rollback; the flag is what makes it actually reversible without a code-archaeology exercise.

---

## Migration Phases (Full Main.cs Reduction)

This batch migrates 10 of the **38 verified** Setup methods (corrected from the original "10/31"). Subsequent batches:

| Batch | Systems | Main.cs Reduction | Notes |
|-------|---------|-------------------|-------|
| 86 (this) | 10 foundation systems | To be measured (see Step 6) | Low-coupling systems first |
| Future A | ~9 expansion systems (revised: 38 total − 10 this batch − 10 Future B − 1 final-stub-removal-pass ≈ 17–18 remain across Future A/B; the original "11" and "10" split was based on the wrong 31-method total and should be re-planned once the full 38-method list is enumerated in Step 4) | TBD | Holdfast, YearOfAsh, Maritime, Muster |
| Future B | Remaining host session systems | TBD | Narrative, Economy, Combat, Medical |
| Final | Remove empty Setup stubs | TBD | Main.cs line target should be set only after the real reduction from batches 86/Future A/B is measured — "< 2000 lines" from a 7014-line starting point (not 6640) is a much larger claimed reduction than the original draft implies and should be sanity-checked, not carried forward unchanged |

---

## Exit Criteria

- [ ] `ServiceRegistry` exists in `Assets/Ashfall.Core/DI/` with full implementation
- [ ] Cycle detection catches all circular dependencies at registration time
- [ ] `CoreServiceModule` registers the systems enumerated by the Step 4 audit without validation errors (state the actual count reached — do not report "82+ systems" since that figure was never verified)
- [ ] `GodotHostServiceModule` provides all 5 port implementations + the verified save-store count (27, confirmed this session — re-verify at implementation time since this drifts)
- [ ] 10 of the 38 systems resolve via DI; their manual Setup/Save/Flush code removed from Main.cs
- [ ] `ISaveable` enumeration replaces manual `SaveAll` for migrated systems
- [ ] 28+ new tests pass; 0 existing tests broken (baseline ~2120 tests, verified this session)
- [ ] `dotnet build Ashfall.csproj` — 0 errors
- [ ] `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — all pass
- [ ] `godot --headless --path . -- --data-integrity-selftest` — 0 errors
- [ ] `godot --headless --path . -- --bridge-selftest` — exits 0 (process-exit check only; does not validate DI wiring — see Step 5 note)
- [ ] Main.cs reduction is measured and reported as an actual number (via `git diff --stat`), not the pre-committed "~250 lines" estimate
- [ ] The order-sensitivity audit from Step 4 has been performed and its findings (including "none found," if applicable) are documented before Step 5/6 relies on resolution-order changes being safe

## Review Notes (Corrected)

This plan was adversarially reviewed against the live codebase at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`. Summary of what changed and why:

1. **Every headline number in the Motivation section was wrong and has been corrected with verified figures:** `src/Main.cs` is **7014 lines** (not 6640, via `wc -l`), has **38 `SetupXxx` methods** (not 31, via `grep -c "void Setup"`), **~29 named `SaveXxx` methods plus a `SaveAll` orchestrator** (not 24, via `grep -n "void Save[A-Z]"`), and **17 `FlushXxxIfDirty` methods** (this one was already correct). The test-suite baseline is **~2120 tests** (`grep -rc "\[Fact\]\|\[Theory\]" Ashfall.Core.Tests/`), used throughout the corrected Done-when sections instead of leaving test counts unquantified.
2. **The "82+ systems" and "31 Setup methods" figures were never reconciled with each other in the original draft** (Step 4 said "82+ systems," the header said "31 SetupXxx methods" — implying either multiple systems per Setup method, which is plausible but never stated, or an inconsistency). This review does not have a verified system count and explicitly flags "82+" as unverified everywhere it appeared, rather than propagating it. **Before Step 4 is executed, produce a real enumerated list from reading all 38 Setup methods** — this is now a stated prerequisite, not an assumption.
3. **Save-store and host-session counts were wrong:** verified **27 save-store files** (not 22) and **27 host-session files under `src/Host/`** (not 34). Both corrected in Step 5.
4. **No DI-like pattern exists anywhere in the codebase — this premise held up.** A repository-wide search for `IServiceProvider`, `ServiceCollection`, `ServiceRegistry`, `IServiceContainer` found zero matches, confirming there's nothing to reconcile with or migrate away from except manual construction. The project's `IEventBus`/`SimpleEventBus` is unrelated (pub/sub, not a container) and doesn't conflict with this plan.
5. **`GameBootstrap` does not exist as an active type anywhere this plan's file paths reference.** The only match is `_quarantine_legacy/Assets/Scripts/Core/GameBootstrap.cs` — quarantined legacy code outside both `Assets/_Game/` and `src/`. The plan never explicitly named `GameBootstrap` as a migration target, but the Godot host's real entry point (`src/Main.cs` via `_Ready()`) needed to be stated explicitly, since Step 5's original code sketch invented a `Boot()` method that doesn't exist. All references to `Boot()` were corrected to wire into the real `_Ready()` method.
6. **The construction-order risk was underspecified for a plan rated Medium risk with "incorrect registration order could surface latent bugs" in its own header.** The original draft mentioned "order-dependent construction" once in a problem table but never turned it into an actionable step. This review adds: an explicit warning in the Motivation section describing *why* this is a silent-bug risk (not just a crash risk, which is easier to catch); a new audit sub-step in Step 4 (order-sensitive side effects beyond constructor injection); a Step 5 Risk/Rollback paragraph naming this as the step where the risk actually materializes; and a feature-flag rollback mechanism for Steps 5–6 so a regression is a flag flip, not a git-archaeology exercise.
7. **Every "~250 lines" / "-300 lines" / "-200 lines" / "-100 lines" reduction estimate in the original Summary and Migration Phases tables was unfounded** — no method-by-method line count was performed, and the totals didn't even reconcile against the corrected 38-method count. These are now marked "to be measured" with an explicit instruction to use `git diff --stat` rather than restating unverified estimates.
8. **Done-when criteria were tightened** to require reporting actual measured numbers (line reduction, system count, save-store count) rather than accepting pre-committed estimates as satisfied by inspection. Several Done-when clauses also gained an explicit audit/documentation deliverable (e.g., "findings written down even if none found") so that "I checked and it's fine" is falsifiable rather than assumed.
