# ASHFALL — GENERATION WAVE 12 — IMPLEMENTATION UNBLOCKER MASTER PLAN — PART 10
// SPDX-License-Identifier: MIT

**Document role:** execution-grade Part 10 master plan for Generation Wave 12, completing the foundational engineering and runtime verification series.

**Purpose:** convert the eight highest-priority unsealed runtime testing, live reload, contract enforcement, null safety, immutable state, change tracking, rate limiting, and achievement ledger roadmaps into an execution-grade unblocking package. Each task provides 20 procedural execution instructions and structured mini-tasks with 4 additional mini-substeps each, eliminating guesswork for coding agents and foremen.

**Wave 12 Part 10 Premise:** Parts 1 through 9 established the entire gameplay surface, late progression, and platform production infrastructure. Part 10 establishes the **core code health, contract rigor, and regression-proof test gates**: automated end-to-end UI smoke tests in headless Godot, live runtime data hot-reloading, design-by-contract precondition assertions in Core, elimination of 4,002 suppressed nullability warnings, immutable state DTO patterns, save snapshot diffing, deterministic action rate limiting, and cryptographic offline achievement tracking.

**Part 10 Execution Set (Exactly 8 Tasks):**
1. **Task S1 — Roadmap Batch 97: End-to-End Smoke Test Suite & Headless Godot UI Flow Automation**
2. **Task S2 — Roadmap Batch 98: Live Data Hot-Reload & Runtime Parameter Tuning Pipeline**
3. **Task S3 — Roadmap Batch 99: Core Design-by-Contract & Precondition Invariant Enforcement**
4. **Task S4 — Roadmap Batch 100: Systematic Null-Safety Remediation & Compiler Warning De-Suppression**
5. **Task T1 — Roadmap Batch 101: Immutable Save DTOs & Anti-Mutation State Isolation**
6. **Task T2 — Roadmap Batch 102: Save Snapshot Diffing, Change Tracking & Forensic State Audits**
7. **Task T3 — Roadmap Batch 103: Deterministic Rate Limiting, Action Cooldowns & Anti-Exploit Throttles**
8. **Task T4 — Roadmap Batch 104: Offline Achievement Verification & Cryptographic Milestone Ledger**

---

# 0. OPERATING CONTRACT

## 0.1 Allowed Terminal States

Every task in Wave 12 Part 10 must reach one of these terminal states:

- **IMPLEMENTED** — Missing mechanism, consumer, contract, or surface is authored, wired to canonical owners, and verified green across all required test gates.
- **DECIDED-DEFERRED** — Product/architecture authority requires a signature; an implementation-selective decision memo is signed and recorded, leaving the remainder explicitly deferred without claiming false completion.
- **RETIRED** — An obsolete or duplicate file/shim/mechanism is deleted and unregistered with reference proof and architecture map regeneration.
- **VERIFIED-RESOLVED** — Re-verification at repository `HEAD` proves the blocker was already sealed by concurrent work; evidence is recorded and redundant implementation is skipped.
- **ROUTED-REPAIR** — Investigation exposes a genuine production defect outside the task's bounded scope; an isolated repair package with reproducible characterization test is registered.

## 0.2 Non-Negotiable Hard Rules

1. **Godot is authoritative; Unity is retired.** No Unity dependencies, shims, or references may be added.
2. **Core stays engine-free.** `Assets/Ashfall.Core/` (`netstandard2.1`) must never reference Godot or engine types.
3. **JSON data is authoritative.** Authoritative data resides in `Assets/StreamingAssets/Data/` with valid schema policy.
4. **Preserve determinism and persistence.** No `System.Random` or unseeded wall-clock RNG in Core domain logic.
5. **One authority per concern.** Never create duplicate registries, parallel save stores, or shadow managers.
6. **Claims before edits.** Check and record file path claims in `WORKTREE_OWNERSHIP.md` before touching code.
7. **Substeps are instructions, not tasks.** The 20 substeps per task represent ordered procedural instructions.
8. **Mini-tasks require 4 mini-substeps.** Any mini-task (e.g. `.1`, `.2`) must contain exactly 4 subsequent execution instructions.
9. **Focused testing first.** Use `scripts/run_test.sh` for bounded xUnit runs; do not run broad suites unprompted.
10. **Zero warning tolerance.** Production code edits must maintain a 0-error, 0-warning baseline on build.

---

# 1. DETAILED TASK SPECIFICATIONS

## TASK S1 — Roadmap Batch 97: End-to-End Smoke Test Suite & Headless Godot UI Flow Automation

- **Source Plan:** `C-integration-plans/ASHFALL_NEXT_STEPS_QUALITY_ROADMAP_BATCH_97.md`
- **Blocker Class:** HEADLESS UI SMOKE / INTERACTION AUTOMATION GAP
- **Canonical Owner:** `src/Tests/`, `scripts/`
- **Target Subsystem:** Godot headless UI navigation tests, panel opening/closing assertions, button click automation

### 20 Procedural Substeps:
1. Review existing test runners in `scripts/run_test.sh` and Godot headless invocation scripts to establish baseline CLI flags.
2. Enforce core invariant: UI smoke tests simulate player inputs programmatically without mutating Core domain rules directly.
3. Claim `src/Tests/HeadlessUiSmokeRunner.cs` and `scripts/run_ui_smoke_tests.sh`.
4. Author headless Godot test scene `res://scenes/tests/UiSmokeTestRunner.tscn` launching panels without windowed displays.
5. Implement programmatic input simulation: emit synthetic mouse click and keyboard navigation events to active UI controls.
6. Build panel lifecycle assertions: open each of the 83 UI panels, verify successful instantiation, and close cleanly.
7. Assert that opening and closing panels produces zero unhandled exceptions, memory leaks, or orphaned Node orphans in tree.
8. Test end-to-end new game flow: boot game, click New Campaign, select difficulty preset, and confirm shelter spawn.
9. Test airlock screening UI flow: click incoming visitor card, toggle inspection option, and confirm admission/rejection.
10. Test market trade UI flow: open merchant screen, drag trade items into barter queue, and execute exchange transaction.
11. Test duty roster assignment flow: open roster panel, reassign survivor to hydroponics duty, and assert roster updates.
12. Assert that pause menu and modal dialogs trap focus correctly and dismiss cleanly upon pressing the Escape key.
13. Capture automated headless screenshot artifacts at key UI flow steps to verify visual rendering correctness.
14. Ensure smoke test suite completes execution within 60 seconds to support rapid CI pre-merge gate validation.
15. Author `ui_smoke_scenarios.json` declaring sequence steps and expected UI node paths with `schema_version: 1`.
16. Author unit tests in `src/Tests/UiNavigationTests.cs` verifying synthetic event dispatcher mechanics.
17. Verify that UI smoke tests execute cleanly in Linux Docker containers with dummy display drivers (`--display-driver headless`).
18. Validate that all UI panels properly unregister event bus listeners during node destruction, avoiding memory leaks.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing smoke logs, screenshot artifacts, and updated documentation in `docs/testing/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### S1.1 Programmatic Input Simulator
- (a) Author `SyntheticInputDispatcher` generating Godot `InputEventMouseButton` and `InputEventKey` actions.
- (b) Implement `ClickControl(Control target)` computing global center coordinates and dispatching down/up press events.
- (c) Provide gamepad focus navigation helper simulating D-pad direction navigation and accept button presses.
- (d) Write unit tests verifying that synthetic click events successfully fire button pressed signals.

#### S1.2 83-Panel Instantiation & Destruction Gate
- (a) Enumerate all 83 panel types in `src/UI/` using reflection during automated test discovery.
- (b) Instantiate each panel in a mock viewport, advance engine process frames by 3 ticks, and destroy the node.
- (c) Assert that `Node.GetOrphanCount()` remains exactly 0 after panel disposal.
- (d) Author tests validating that zero panel classes throw null reference exceptions during headless `_Ready()` execution.

#### S1.3 Core Gameplay Flow Scenarios
- (a) Script new game flow: TitleScreen -> NewGameDialog -> DifficultySelect -> WorldMap -> ShelterScreen.
- (b) Script daily transition flow: trigger NextDay button, wait for day event popup, and click Acknowledge.
- (c) Script emergency response flow: trigger raid alarm, open defense panel, and click LockdownAirlock.
- (d) Author characterization tests confirming that scripted flows execute to completion without softlocks.

#### S1.4 Headless CI Harness & Screenshot Capture
- (a) Configure Godot `--display-driver headless` execution parameters in `scripts/run_ui_smoke_tests.sh`.
- (b) Capture viewport viewport textures to PNG files at major flow milestones (`artifacts/smoke_step_*.png`).
- (c) Establish CI failure exit codes if any scripted step fails to find expected UI node elements.
- (d) Verify through test automation that the entire smoke suite finishes in under 60 seconds on standard CI runners.

---

## TASK S2 — Roadmap Batch 98: Live Data Hot-Reload & Runtime Parameter Tuning Pipeline

- **Source Plan:** `C-integration-plans/ASHFALL_NEXT_STEPS_QUALITY_ROADMAP_BATCH_98.md`
- **Blocker Class:** RUNTIME TUNING / LIVE DATA HOT-RELOAD GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Data/`, `src/Host/`
- **Target Subsystem:** File system watcher for `StreamingAssets/Data/`, runtime JSON reload, balance live-tuning

### 20 Procedural Substeps:
1. Review `CatalogIntegrityValidator.cs` and data loading routines across all 333+ JSON catalogs in `Assets/StreamingAssets/Data/`.
2. Enforce core invariant: hot-reload refreshes static definition catalogs; it never mutates active simulation state entities.
3. Claim `Assets/Ashfall.Core/Data/DataHotReloadCoordinator.cs` and `src/Host/DataFileWatcher.cs`.
4. Implement asynchronous file system watcher monitoring `Assets/StreamingAssets/Data/*.json` for modification events.
5. Apply file debouncing: coalesce rapid write events within 500ms into a single atomic reload request.
6. Validate modified JSON files against `CatalogIntegrityValidator.cs` before applying reloads, rejecting invalid syntax.
7. Reload catalog dictionaries atomically: swap memory pointers so readers always observe coherent catalog snapshots.
8. Emit `DataCatalogReloadedEvent` across the semantic event bus alerting UI panels and systems to refresh cached displays.
9. Support live tuning of item barter prices: modifying `items.json` immediately updates shop prices without game restart.
10. Support live tuning of weather damage: modifying `weather_parameters.json` immediately updates active storm formulas.
11. Support live tuning of recipe ingredients: modifying `recipes.json` immediately updates workshop craftability checks.
12. Ensure hot-reload is disabled automatically in Release and Export builds to eliminate unnecessary file I/O overhead.
13. Provide in-game developer overlay displaying reload notifications (e.g. `[HotReload] items.json reloaded in 4ms`).
14. Guard against race conditions: ensure hot-reload never executes while the simulation is actively processing a day tick.
15. Author unit tests in `Ashfall.Core.Tests/Data/DataHotReloadTests.cs` verifying pointer-swap thread safety.
16. Verify that syntax errors in authored JSON files trigger clear log warnings without crashing the running game.
17. Verify that live data reload functions cleanly in both desktop editor sessions and headless Godot runs.
18. Validate that memory usage remains stable after 100 consecutive file modifications, proving zero memory leaks.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/tooling/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### S2.1 File System Watcher & Debouncing
- (a) Author `DataFileWatcher` utilizing `System.IO.FileSystemWatcher` targeting `Assets/StreamingAssets/Data/`.
- (b) Filter watch events strictly for `.json` file modifications, creations, and renames.
- (c) Implement 500ms timer debouncing preventing half-written file reads from editor save actions.
- (d) Write unit tests verifying that rapid burst saves coalesce into exactly one reload invocation.

#### S2.2 Schema Pre-Validation & Atomic Swap
- (a) Ingest modified file text and execute schema validation in an isolated staging sandbox.
- (b) Abort reload if JSON syntax is malformed or required schema fields are missing, logging error details.
- (c) Execute atomic pointer swap replacing global catalog references when validation succeeds.
- (d) Author tests validating that malformed JSON edits do not corrupt active in-memory catalogs.

#### S2.3 Subsystem Refresh Dispatch
- (a) Emit `CatalogReloadedEvent` specifying the exact file and catalog types that were updated.
- (b) Wire `MarketSystem` to re-read item prices upon receiving `items.json` reload notifications.
- (c) Wire `CraftingSystem` to re-parse available recipe lists upon `recipes.json` reload notifications.
- (d) Author tests proving that updated item trade values reflect immediately in active barter calculations.

#### S2.4 Developer HUD & Export Guardrails
- (a) Author lightweight developer notification toast displaying reload duration and status in top-right screen corner.
- (b) Enforce `#if DEBUG` conditional compilation ensuring file watchers are excluded from production release builds.
- (c) Provide CLI override `--disable-hot-reload` for deterministic benchmarking and soak tests.
- (d) Author characterization tests confirming that hot-reload incurs zero performance cost when dormant.

---

## TASK S3 — Roadmap Batch 99: Core Design-by-Contract & Precondition Invariant Enforcement

- **Source Plan:** `C-integration-plans/ASHFALL_NEXT_STEPS_QUALITY_ROADMAP_BATCH_99.md`
- **Blocker Class:** CODE CONTRACTS / PRECONDITION INVARIANT GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Contracts/`, `Assets/Ashfall.Core/`
- **Target Subsystem:** Lightweight precondition assertion library, state invariants, defensive exception boundaries

### 20 Procedural Substeps:
1. Review error handling across Core simulation coordinators to identify silent failure patterns and unvalidated arguments.
2. Enforce core invariant: contracts enforce preconditions and invariants in Core; they must never allocate strings in hot paths.
3. Claim `Assets/Ashfall.Core/Contracts/Requires.cs` and `Assets/Ashfall.Core/Contracts/ContractException.cs`.
4. Author high-performance precondition helper `Requires.NotNull<T>(T value, string paramName)` throwing `ArgumentNullException`.
5. Author numeric range precondition `Requires.InRange(int value, int min, int max, string paramName)` throwing `ArgumentOutOfRangeException`.
6. Author string argument precondition `Requires.NotNullOrWhiteSpace(string value, string paramName)`.
7. Author state invariant assertion helper `Asserts.State(bool condition, string message)` throwing `InvalidOperationException`.
8. Apply precondition checks to survivor state constructors, ensuring non-null IDs, non-negative health, and valid life stages.
9. Apply precondition checks to inventory transaction methods, ensuring positive item quantities and non-empty slot IDs.
10. Apply precondition checks to campaign calendar updates, ensuring positive day stepping increments.
11. Apply state invariant checks to save/restore methods, asserting that restored state checksums match expected schemas.
12. Ensure all contract methods utilize `[MethodImpl(MethodImplOptions.AggressiveInlining)]` to eliminate call-stack overhead.
13. Integrate caller argument expression attributes (`[CallerArgumentExpression]`) to automatically capture variable names.
14. Ensure contracts compile cleanly under `netstandard2.1` with zero external dependency requirements.
15. Author unit tests in `Ashfall.Core.Tests/Contracts/ContractTests.cs` verifying that contract violations throw expected exceptions.
16. Benchmark contract assertion overhead in tight simulation loops, asserting <1% performance difference.
17. Verify that contract exceptions provide descriptive parameter names and out-of-range bounds in error messages.
18. Validate that newly authored Wave 12 coordinators strictly implement `Requires` precondition guards on all public APIs.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/architecture/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### S3.1 Contract Library Core Implementation
- (a) Author `Requires.cs` static class with generic argument validation methods.
- (b) Implement zero-allocation fast paths for successful precondition checks.
- (c) Create custom `PreconditionViolationException` and `StateInvariantException` classes.
- (d) Write unit tests verifying that passing valid arguments incurs zero exception throws.

#### S3.2 Parameter Range & Null Guards
- (a) Implement `Requires.Positive(int value, string paramName)` validating strictly positive integers.
- (b) Implement `Requires.NonNegative(int value, string paramName)` validating values >= 0.
- (c) Implement `Requires.ValidPercentage(float value, string paramName)` validating values between 0.0f and 1.0f.
- (d) Author tests validating that negative item quantities or null survivor refs throw immediate exceptions.

#### S3.3 Simulation Invariant Assertions
- (a) Author `Asserts.State(bool condition, string message)` for internal state machine sanity checks.
- (b) Assert that survivor health never exceeds maximum biological hitpoints during healing updates.
- (c) Assert that shelter food reserves never drop below zero during consumption calculations.
- (d) Author tests proving that corrupt internal states are caught at the exact mutation site rather than later in execution.

#### S3.4 Performance Benchmarking & Inlining Verification
- (a) Benchmark 1,000,000 iterations of contract checks using BenchmarkDotNet harness.
- (b) Verify that JIT compiler inlines precondition checks cleanly without pushing extra stack frames.
- (c) Assert that contract validation adds less than 2 milliseconds to a 100-day simulation run.
- (d) Author characterization tests confirming that contracts compile cleanly with zero warnings under netstandard2.1.

---

## TASK S4 — Roadmap Batch 100: Systematic Null-Safety Remediation & Compiler Warning De-Suppression

- **Source Plan:** `C-integration-plans/ASHFALL_NEXT_STEPS_QUALITY_ROADMAP_BATCH_100.md`
- **Blocker Class:** NULL SAFETY / WARNING SUPPRESSION DEBT
- **Canonical Owner:** `Ashfall.Core/`, `Ashfall.Core.Tests/`, `Ashfall.csproj`
- **Target Subsystem:** De-suppression of CS8618/CS8603 warning codes, nullable annotation cleanup, null-safe accessors

### 20 Procedural Substeps:
1. Inspect `Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` line 10 to inventory currently suppressed nullable warning codes.
2. Enforce core invariant: nullable reference types are enabled across all projects; warnings must be fixed, not suppressed.
3. Claim `Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` and affected Core domain classes.
4. Measure exact baseline warning count across the solution using `dotnet build -t:Rebuild` without warning suppressions.
5. Systematically remove suppression codes `CS8618;CS8603;CS8600;CS8601;CS8602;CS8604;CS8625` from `Ashfall.Core.Tests.csproj`.
6. Annotate non-nullable string and collection properties with `required` modifiers or `= default!;` in test fixture DTOs.
7. Annotate nullable return types explicitly with `?` (e.g. `SurvivorId? FindSurvivorByName(string name)`).
8. Replace unsafe dereference patterns with null-conditional operators (`?.`) and null-coalescing operators (`??`).
9. Initialize collection fields directly with empty instances (`= new List<T>()`, `= new Dictionary<K, V>()`) rather than null.
10. Annotate event handlers and delegates with explicit nullable signatures ensuring listeners are checked before invoke.
11. Update dictionary lookup patterns to use `TryGetValue` rather than unverified indexer lookups.
12. Resolve constructor initialization warnings (CS8618) by providing explicit parameter-based constructors or factory methods.
13. Audit generic type parameters, adding `where T : class` or `where T : notnull` constraints where appropriate.
14. Ensure all save state capture and restore DTOs declare explicit nullable annotations matching schema contracts.
15. Verify that test helper classes in `Ashfall.Core.Tests` initialize mock objects safely without relying on null bypasses.
16. Run full clean rebuilds asserting a reduction of nullable warnings toward the target zero baseline.
17. Verify that nullable annotations do not alter runtime IL semantics or degrade simulation performance.
18. Validate that all public APIs in `Assets/Ashfall.Core/` document nullability expectations clearly in docstrings.
19. Inspect build output to confirm zero compiler warnings across `Ashfall.Core.csproj`, `Ashfall.csproj`, and test suites.
20. Hand off the task with verified before/after warning tallies, clean test execution logs, and updated entries in `docs/architecture/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### S4.1 Test Project Warning De-Suppression
- (a) Edit `Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` to remove `<NoWarn>CS8618;CS8603;...</NoWarn>` suppressions.
- (b) Execute `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` and log all newly visible compiler warnings.
- (c) Categorize warnings by defect type (uninitialized constructor properties vs unsafe dereferences).
- (d) Write unit tests verifying that test projects compile cleanly with nullable warnings enabled.

#### S4.2 Constructor Initialization & Required Modifiers
- (a) Add `= string.Empty;` default initializers to required non-null string properties in data DTOs.
- (b) Add `= new();` initializers to all list, dictionary, and hash set fields in entity classes.
- (c) Utilize `required` keyword on immutable DTO properties to mandate initialization at construction.
- (d) Author tests validating that DTO instantiation enforces required field assignments at compile time.

#### S4.3 Dictionary & Collection Null-Safe Accessors
- (a) Refactor direct dictionary indexer calls `dict[key]` into safe `dict.TryGetValue(key, out var val)` patterns.
- (b) Provide extension methods `GetValueOrDefault(key, fallback)` for common read-heavy catalog dictionaries.
- (c) Annotate method return types returning null when items are not found with explicit `T?` question marks.
- (d) Author tests proving that querying non-existent keys returns null safely without throwing `KeyNotFoundException`.

#### S4.4 Full Solution Rebuild & Zero-Warning Gate
- (a) Run `dotnet clean && dotnet build -t:Rebuild` across the entire solution.
- (b) Assert that `Ashfall.Core.dll` builds with exactly 0 Warnings and 0 Errors.
- (c) Assert that `Ashfall.dll` (Godot host) builds with exactly 0 Warnings and 0 Errors.
- (d) Author characterization tests confirming that test suites run and pass 100% with full nullable checking active.

---

## TASK T1 — Roadmap Batch 101: Immutable Save DTOs & Anti-Mutation State Isolation

- **Source Plan:** `C-integration-plans/ASHFALL_NEXT_STEPS_QUALITY_ROADMAP_BATCH_101.md`
- **Blocker Class:** STATE MUTATION / SAVE ISOLATION GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Save/`, `Assets/Ashfall.Core/`
- **Target Subsystem:** Immutable state capture DTOs, record structs, read-only collection wrappers, defensive copying

### 20 Procedural Substeps:
1. Review all `CaptureState()` and `RestoreState()` implementations across Core simulation coordinators.
2. Enforce core invariant: captured state DTOs must be completely immutable; mutating a DTO must not corrupt live simulation state.
3. Claim `Assets/Ashfall.Core/Save/ImmutableSaveDto.cs` and refactor target save state records.
4. Convert mutable class DTOs into C# `readonly record struct` or immutable `record` types with `init`-only properties.
5. Wrap serialized collection properties with `IReadOnlyList<T>` and `IReadOnlyDictionary<K, V>` interfaces.
6. Implement defensive copying in `CaptureState()`: clone lists and arrays so external modifications cannot leak back into live memory.
7. Implement defensive copying in `RestoreState()`: ensure coordinators instantiate fresh private collections from incoming DTOs.
8. Author automated mutation characterization tests asserting that modifying captured DTO instances has zero effect on domain state.
9. Ensure immutable records support fast structural equality (`IEquatable<T>`) for snapshot diffing and change tracking.
10. Ensure JSON serializers can deserialize into immutable record types using primary constructors or `init` properties.
11. Eliminate shared reference leaks where two entity instances inadvertently point to the same mutable child object.
12. Convert enum collections in DTOs to immutable arrays to avoid heap allocation overhead during state capture.
13. Benchmark state capture speed, verifying that defensive copying introduces <3ms overhead per save operation.
14. Ensure all newly authored immutable DTOs declare explicit `schema_version: 1` properties.
15. Author unit tests in `Ashfall.Core.Tests/Save/ImmutableSaveDtoTests.cs` verifying anti-mutation isolation.
16. Verify that save file serialization produces identical JSON outputs before and after DTO immutability refactoring.
17. Verify that multi-threaded or background save operations can safely read captured DTOs without synchronization locks.
18. Validate that restoring state from an immutable DTO produces bitwise-identical simulation behavior.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/architecture/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### T1.1 Record Struct Conversion & Init Properties
- (a) Convert mutable `SurvivorSaveData` class into an immutable `readonly record struct`.
- (b) Convert mutable `InventorySaveData` class into an immutable `readonly record struct`.
- (c) Mark all property accessors as `{ get; init; }` to prohibit post-construction mutation.
- (d) Write unit tests verifying that attempting to modify DTO properties post-initialization fails compilation.

#### T1.2 Defensive Collection Cloning
- (a) Author collection cloning helpers `ToImmutableList()` and `ToImmutableDictionary()` for DTO mapping.
- (b) Ensure child objects within lists are deeply cloned or mapped to immutable value structs.
- (c) Prohibit passing live domain collection references directly into save payload constructors.
- (d) Author tests validating that clearing a live inventory list after state capture leaves the captured DTO unchanged.

#### T1.3 Background Thread Save Safety
- (a) Verify that captured immutable DTO trees can be passed to background worker threads for JSON disk writing.
- (b) Eliminate UI thread stutter during autosaves by moving file compression and writing off the main simulation thread.
- (c) Assert that live simulation can advance to the next day while background threads serialize prior state DTOs.
- (d) Author tests proving that concurrent simulation updates do not throw collection modification exceptions during saves.

#### T1.4 Structural Equality & Deserialization Parity
- (a) Verify that record structs automatically provide value-based equality operator (`==`) implementations.
- (b) Configure `System.Text.Json` options to deserialize directly into immutable constructors without reflection hacks.
- (c) Assert that serializing and deserializing a captured state yields an equal immutable DTO instance.
- (d) Author characterization tests confirming that save file round-trips preserve 100% data fidelity.

---

## TASK T2 — Roadmap Batch 102: Save Snapshot Diffing, Change Tracking & Forensic State Audits

- **Source Plan:** `C-integration-plans/ASHFALL_NEXT_STEPS_QUALITY_ROADMAP_BATCH_102.md`
- **Blocker Class:** STATE DIFFING / CHANGE TRACKING GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Diagnostics/`, `Assets/Ashfall.Core/Save/`
- **Target Subsystem:** Turn-by-turn state diffing, delta tracking, save comparison diagnostics, forensic audit logging

### 20 Procedural Substeps:
1. Review `SavePayload.cs` and `StateSnapshot.cs` to map existing simulation state structures.
2. Enforce core invariant: snapshot diffing is a diagnostic analysis tool; it observes state deltas without altering simulation logic.
3. Claim `Assets/Ashfall.Core/Diagnostics/SaveSnapshotDiffCoordinator.cs` and `Assets/Ashfall.Core/Diagnostics/StateDeltaRecord.cs`.
4. Define `DeltaOperationKind` enum: Added, Removed, Modified, Unchanged.
5. Author structural diff engine comparing two consecutive campaign save snapshots (Day N vs Day N+1).
6. Track population deltas: detect born survivors, deceased casualties, recruited wanderers, and exiled traitors.
7. Track inventory deltas: compute net additions and deductions across food, ammunition, medicine, and building scrap.
8. Track health and condition deltas: identify survivors who contracted diseases, suffered trauma, or healed from wounds.
9. Track faction relationship deltas: highlight factions whose trust or hostility shifted across the turn.
10. Generate human-readable turn summary logs detailing exact causes for major resource shifts.
11. Build an automated forensic auditor detecting impossible state jumps (e.g. food jumped from 10 to 1,000 in one day).
12. Support automated bug characterization: when an unexpected failure occurs, diff the failing save against a known good baseline.
13. Author `state_diff_rules.json` in `Assets/StreamingAssets/Data/` with standard `schema_version: 1`.
14. Ensure snapshot diffing runs in memory with zero disk I/O overhead during automated test suites.
15. Expose snapshot diff summaries to developer debug consoles and headless CLI commands (`--diff-saves <A> <B>`).
16. Author unit tests in `Ashfall.Core.Tests/Diagnostics/SaveSnapshotDiffTests.cs` verifying delta calculation accuracy.
17. Verify deterministic seed isolation ensuring identical campaign turns produce identical snapshot diff reports.
18. Validate that diffing large 100-survivor shelter states executes rapidly (<10ms) without frame drops.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/diagnostics/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### T2.1 Structural Diff Engine & Delta Records
- (a) Author `SaveSnapshotDiffCalculator` comparing two normalized immutable save DTO instances.
- (b) Compare scalar properties, nested child objects, and dictionary collections recursively.
- (c) Emit structured `StateDeltaRecord` lists capturing exact property paths, old values, and new values.
- (d) Write unit tests verifying that identical state snapshots generate exactly zero delta records.

#### T2.2 Turn-by-Turn Resource Delta Auditing
- (a) Calculate net daily consumption and production rates across all tracked shelter stockpile categories.
- (b) Correlate resource drops with active consumer subsystems (e.g. food drop attributed to meal consumption + spoilage).
- (c) Flag unexplained inventory depletion events to help developers pinpoint resource leak defects.
- (d) Author tests validating that consuming 15 rations produces an inventory delta of exactly -15.

#### T2.3 Anomaly & Impossible Jump Detection
- (a) Author validation rules checking for statistically impossible state jumps across single campaign turns.
- (b) Detect instant healing of fatal afflictions occurring without medicine or medical intervention.
- (c) Detect instant survivor teleportation between remote wasteland nodes without elapsed travel days.
- (d) Author tests proving that corrupt or tampered save state jumps are flagged as anomalous in diff logs.

#### T2.4 CLI Tooling & Formatted Diff Export
- (a) Implement CLI switch `--diff-saves <SAVE_A> <SAVE_B>` outputting colorized text diffs to terminal stdout.
- (b) Support `--export-diff-json <PATH>` outputting machine-readable JSON diff trees for CI analysis.
- (c) Render concise summary tables highlighting major macro changes (Population, Reserves, Faction Stance).
- (d) Author characterization tests confirming that CLI diff commands execute cleanly across all test saves.

---

## TASK T3 — Roadmap Batch 103: Deterministic Rate Limiting, Action Cooldowns & Anti-Exploit Throttles

- **Source Plan:** `C-integration-plans/ASHFALL_NEXT_STEPS_QUALITY_ROADMAP_BATCH_103.md`
- **Blocker Class:** ACTION RATE LIMITING / EXPLOIT THROTTLING GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Governance/`, `Assets/Ashfall.Core/Economy/`
- **Target Subsystem:** Campaign-day cooldown registries, action rate limiters, anti-spam barter guards, interaction throttling

### 20 Procedural Substeps:
1. Review player interaction surfaces across trade, diplomacy, decree enactment, and medical triage to map unthrottled actions.
2. Enforce core invariant: cooldowns track elapsed campaign time deterministically; never rely on real-world wall clocks.
3. Claim `Assets/Ashfall.Core/Governance/ActionCooldownCoordinator.cs` and `Assets/Ashfall.Core/Governance/CooldownRecord.cs`.
4. Define `ThrottledActionKind` enum: EnactDecree, RequestFactionLoan, RadioBroadcastPlea, BribeGuard, RerollMerchantStock.
5. Attach campaign-day timestamps to action execution events, enforcing mandatory cooldown durations (e.g. 7 days between decrees).
6. Implement barter transaction rate limiting: prevent infinite trade relationship farming through rapid single-item trades.
7. Implement diplomacy request throttling: foreign factions reject repeated identical alliance demands sent within 14 days.
8. Wire medical treatment cooldowns: repeated surgery attempts on the same survivor require stabilization recovery periods.
9. Implement radio broadcast cooldowns: transmitters require recharging cool-down cycles before re-broadcasting distress signals.
10. Ensure action cooldowns display clear countdown timers in UI tooltips (e.g. "Available in 3 days").
11. Guard against save-scumming exploits: saving and reloading immediately after action execution preserves remaining cooldown days.
12. Support decree urgency overrides: emergency decrees may bypass cooldowns at the cost of double political legitimacy penalties.
13. Author `action_cooldown_rules.json` in `Assets/StreamingAssets/Data/` with standard `schema_version: 1`.
14. Enforce catalog integrity validation confirming all throttled action kinds declare valid minimum cooldown durations.
15. Save and restore active cooldown registries and remaining throttle days cleanly inside `CampaignSaveSection`.
16. Author unit tests in `Ashfall.Core.Tests/Governance/ActionCooldownTests.cs` verifying cooldown enforcement.
17. Verify deterministic seed isolation ensuring identical action sequences encounter identical throttle boundaries.
18. Validate that throttled UI action buttons render disabled states with informative cooldown countdown tooltips.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/governance/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### T3.1 Cooldown Registry & Action Throttling
- (a) Author `ActionCooldownRegistry` storing active action keys and their expiration campaign day timestamps.
- (b) Implement `IsActionAllowed(ThrottledActionKind action, string targetId)` query method.
- (c) Provide `RegisterActionExecution(ThrottledActionKind action, string targetId, int cooldownDays)` API.
- (d) Write unit tests verifying that actions executed within active cooldown windows are strictly rejected.

#### T3.2 Barter & Trade Farm Prevention
- (a) Track consecutive trades between the shelter and the same merchant caravan within a single trade session.
- (b) Cap relationship affinity gains to at most one gain per trade visit, neutralizing single-item spam farming.
- (c) Apply diminishing barter valuation returns if players repeatedly buy and resell the same inventory stack.
- (d) Author tests validating that executing 50 small trades yields identical relationship gains as a single bulk trade.

#### T3.3 Diplomatic Petition & Faction Demand Throttling
- (a) Intercept diplomatic petition dispatches in `FactionDiplomacyCoordinator`.
- (b) Enforce 14-day refusal cooldown: if a faction rejects a loan request, they refuse to entertain new requests for 14 days.
- (c) Impose diplomatic annoyance penalties if players repeatedly spam rejected petitions to foreign warlords.
- (d) Author tests proving that respecting cooldown periods prevents negative diplomatic relationship decay.

#### T3.4 UI Countdown Presenter & Save Durability
- (a) Author UI presenter helper formatting remaining cooldown days into readable string badges ("Cooldown: 4 Days").
- (b) Disable action execution buttons and display explanatory tooltips when actions are actively throttled.
- (c) Serialize active cooldown dictionaries cleanly within `CampaignSaveSection` with schema validation.
- (d) Author characterization tests confirming that reloading saves maintains exact remaining cooldown durations.

---

## TASK T4 — Roadmap Batch 104: Offline Achievement Verification & Cryptographic Milestone Ledger

- **Source Plan:** `C-integration-plans/ASHFALL_NEXT_STEPS_QUALITY_ROADMAP_BATCH_104.md`
- **Blocker Class:** ACHIEVEMENT SYSTEM / PROGRESS RECOGNITION GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Achievements/`, `Assets/Ashfall.Core/Security/`
- **Target Subsystem:** Offline milestone recognition, cryptographic unlock tokens, progress tracking, achievement catalog

### 20 Procedural Substeps:
1. Review `CampaignCalendar.cs`, `JournalSystem.cs`, and `SaveIntegrityCoordinator.cs` to map achievement milestone seams.
2. Enforce core invariant: achievements are offline-first and cryptographically verifiable; never require online server telemetry.
3. Claim `Assets/Ashfall.Core/Achievements/AchievementCoordinator.cs` and `Assets/Ashfall.Core/Achievements/AchievementMilestone.cs`.
4. Define `AchievementCategory` enum: SurvivalEndurance, MasterEngineering, DiplomaticUnity, MedicalMiracle, WastelandCartography.
5. Author authored achievement definitions in `achievements.json` with standard `schema_version: 1` and localized text keys.
6. Implement milestone progress listeners subscribing to semantic day events across the core event bus.
7. Track progression criteria: surviving 100 days in Nuclear Winter, conducting 50 successful surgeries, synthesizing 5 master blueprints.
8. Generate cryptographic HMAC-SHA256 unlock tokens upon milestone completion, validating legitimate achievement unlocks.
9. Guard against achievement save-file spoofing: verify campaign provenance and state checksums before awarding unlock tokens.
10. Build an in-game achievement showcase UI panel displaying unlocked badges, progress bars, and historical unlock timestamps.
11. Support hidden achievements: conceal spoiler-heavy endgame milestones until the player naturally achieves them.
12. Store unlocked achievements in a dedicated, tamper-resistant `user://achievements.dat` file independent of save slots.
13. Integrate platform-agnostic achievement interfaces allowing future mapping to Steamworks or GOG Galaxy adapters in `src/Host/`.
14. Ensure achievement progress evaluation runs on milestone trigger events, completely avoiding per-frame update checks.
15. Support multi-campaign cumulative achievements (e.g. Total Expeditions Completed Across All Campaigns).
16. Author unit tests in `Ashfall.Core.Tests/Achievements/AchievementCoordinatorTests.cs` verifying unlock criteria evaluations.
17. Verify deterministic seed isolation ensuring identical milestone progress produces identical cryptographic unlock signatures.
18. Validate that achievement notification popups render unobtrusively in the UI without interrupting gameplay focus.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/achievements/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### T4.1 Achievement Catalog & Milestone Criteria
- (a) Define `AchievementDefinition` schema capturing AchievementId, Category, TitleKey, DescriptionKey, and TargetThreshold.
- (b) Author authored milestone entries in `achievements.json` celebrating notable survival and tactical accomplishments.
- (c) Provide validation ensuring all achievement IDs use standard snake_case naming and declare valid localized strings.
- (d) Write unit tests verifying that all authored achievements load cleanly without schema validation errors.

#### T4.2 Event Bus Listener & Progress Tracking
- (a) Subscribe `AchievementCoordinator` to core semantic event dispatches (`DayPassedEvent`, `SurgeryCompletedEvent`).
- (b) Increment internal achievement counters when corresponding gameplay milestones are achieved.
- (c) Emit `AchievementUnlockedEvent` across the event bus upon reaching target thresholds.
- (d) Author tests validating that reaching day 100 triggers the "Century of Survival" achievement unlock.

#### T4.3 Cryptographic Token Signing & Tamper Proofing
- (a) Author `AchievementTokenSigner` generating HMAC-SHA256 signatures combining AchievementId, CampaignSeed, and Timestamp.
- (b) Embed signed tokens into the persistent achievement ledger file.
- (c) Verify token cryptographic integrity on boot, ignoring manually hand-edited achievement entries.
- (d) Author tests proving that forged achievement entries with invalid signatures are rejected during loading.

#### T4.4 In-Game Showcase UI & Host Platform Seam
- (a) Author `AchievementShowcasePanel.cs` presenting grid layouts of earned medals, dates unlocked, and locked silhouettes.
- (b) Provide non-modal visual toast notifications displaying achievement badge icons upon unlock.
- (c) Define clean `IPlatformAchievementBridge` interface in Core allowing host adapters to forward unlocks to platform SDKs.
- (d) Author characterization tests confirming that achievements function 100% offline without network connections.

---

# 2. QUALITY GATE & VERIFICATION MATRIX

| Gate ID | Target System | Focused Verification Command | Passing Criterion |
| :--- | :--- | :--- | :--- |
| **QG-S1** | Headless UI Smoke Tests | `bash scripts/run_ui_smoke_tests.sh` | 100% pass; all 83 panels open/close cleanly in headless mode |
| **QG-S2** | Live Data Hot-Reload | `bash scripts/run_test.sh Ashfall.Core.Tests/Data/DataHotReloadTests.cs` | 100% pass; debounced file watching and atomic swaps verified |
| **QG-S3** | Design-by-Contract | `bash scripts/run_test.sh Ashfall.Core.Tests/Contracts/ContractTests.cs` | 100% pass; precondition guards and zero-allocation checks green |
| **QG-S4** | Null-Safety Remediation | `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | 0 Warnings; all suppressed nullable codes fixed and enabled |
| **QG-T1** | Immutable Save DTOs | `bash scripts/run_test.sh Ashfall.Core.Tests/Save/ImmutableSaveDtoTests.cs` | 100% pass; record structs and defensive copying verified |
| **QG-T2** | Save Snapshot Diffing | `bash scripts/run_test.sh Ashfall.Core.Tests/Diagnostics/SaveSnapshotDiffTests.cs` | 100% pass; recursive delta calculations and anomaly detection green |
| **QG-T3** | Action Rate Limiting | `bash scripts/run_test.sh Ashfall.Core.Tests/Governance/ActionCooldownTests.cs` | 100% pass; day-based cooldowns and anti-spam throttles verified |
| **QG-T4** | Offline Achievements | `bash scripts/run_test.sh Ashfall.Core.Tests/Achievements/AchievementCoordinatorTests.cs` | 100% pass; cryptographic tokens and milestone tracking green |
| **QG-INT** | Catalog Schema & Integrity | `dotnet test --filter "FullyQualifiedName~CatalogIntegrity"` | 100% pass; all newly added JSON catalogs pass schema |
| **QG-BLD** | Engine-Free Compilation | `dotnet build Ashfall.csproj` | 0 Errors, 0 Warnings across entire C# solution |

---

# 3. HANDOFF & CLOSURE CHECKLIST

- [ ] All 8 tasks (S1–S4, T1–T4) have documented terminal states with evidence recorded in commit history.
- [ ] No Unity dependencies, shims, or references were added to any files.
- [ ] `Assets/Ashfall.Core/` remains strictly engine-free (`netstandard2.1`).
- [ ] All authored JSON data contains `schema_version: 1` and adheres to snake_case field naming.
- [ ] No `System.Random` or unseeded `Guid.NewGuid()` calls exist in deterministic Core logic.
- [ ] No duplicate managers, shadow registries, or parallel save stores were introduced.
- [ ] File claims in `WORKTREE_OWNERSHIP.md` are audited and cleared upon task completion.
- [ ] Focused verification passes for all modified subsystems using `scripts/run_test.sh`.
- [ ] Build succeeds with 0 Errors and 0 Warnings across the entire repository.
