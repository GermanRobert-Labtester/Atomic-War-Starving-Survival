# ASHFALL — Quality Roadmap Batch 62

## Theme: Main.cs (Godot Host) Decomposition & Expansion Wiring Cleanup

**Priority:** MEDIUM (architectural debt, single 7014-line orchestrator file)
**Risk:** Medium — structural refactor of expansion initialization in the live, active Godot host
**Batch:** 62
**Depends on:** Existing `ExpansionMasterSession` coordination (in Core), `Ashfall.Core.Events.IEventBus`/`SimpleEventBus`, existing per-domain save stores under `src/Host/`

---

## ⚠️ CORRECTED PREMISE — read before doing any work

This batch as originally written targets **`GameBootstrap`, which does not exist anywhere in the current codebase.** Direct search (`file_search` for `GameBootstrap`, plus a full-tree grep) confirms:

- No `GameBootstrap.cs` or `GameBootstrap.*.cs` file exists in `Assets/`, `src/`, or anywhere else in the tree.
- `GameBootstrap` only appears in historical planning/audit markdown (`ASHFALL_GAME_MASTER_DOCUMENT.md`, `CODE_AUDIT_REPORT.md`, `INTEGRATION_MASTER_PLAN.md`, `AUDIT_FINDINGS_AND_FIX_PLAN.md`, old `docs/superpowers/plans/`) — all describing the **Unity-era** `Assets/_Game/Core/GameBootstrap.*.cs` structure.
- Per AGENTS.md's own non-negotiable rules ("Migration direction is Unity → Godot, always"; "Unity host — inactive, migrating out") and prior findings, `Assets/_Game/` — where `GameBootstrap` lived — has been **fully deleted**. AGENTS.md's Invariant 5 section still lists `GameBootstrap` line/file counts as if the class were current; that section is stale and describes a deleted structure.
- The "82 partial files" and "1225-line god object" figures come from that same stale description and do not describe anything that exists today.

**The real equivalent in the active Godot host is `src/Main.cs`:**
- One file, **7014 lines**, `public partial class Main : Control` — but there are **zero other `Main.*.cs` partials** (`find src -iname "Main.*.cs"` returns nothing beyond `Main.cs` itself). The `partial` keyword is present but unused for file-splitting; this is a single-file god object, not a spread-across-82-files problem.
- Confirmed by grep: **38** `private void SetupXxx()` methods, **30** `private void SaveXxx()` methods (including a `SaveAll()` orchestrator at line 6227), covering expansions (Holdfast, DutyRoster, ExpansionHub, PhantomMemory, Phase0, Muster, Verdict, Maritime, Expeditions, Combat, Narrative, Medical, World, Crafting, Caravans, YearOfAsh, PowerGrid, MedicalWard, Memorial, Greenhouse, Disease, SilentFoundry, and more).
- AGENTS.md's H7 entry ("`Main.cs` ... one `partial class Main` in a single ~6.5k-line file ... 31 Setup / 24 Save + SaveAll / 17 Flush methods") is directionally correct about the single-file shape but its counts are now out of date (actual: 7014 lines, 38 Setup, 30 Save) — the file has grown since that note was written.
- `ExpansionMasterSession` (`Assets/Ashfall.Core/ExpansionMasterSession.cs`) is real and exists in Core, exposing `Holdfast` and other expansion sessions as properties — this part of the original plan's premise is accurate and is retained.
- `IEventBus`/`SimpleEventBus` (`Assets/Ashfall.Core/Events/IEventBus.cs`) is real and matches the string-based, `Publish`/subscribe shape the original plan's `WireEvents(IEventBus bus)` signature assumed.

**This batch is retargeted wholesale: every "GameBootstrap" reference below becomes "`Main.cs`" (the Godot host orchestrator, `src/Main.cs`, namespace `AtomicWar.GodotApp`). The `IExpansionModule`/`ExpansionRegistry` design is still sound as an approach — it's now aimed at the real 7014-line file instead of a phantom one. Numbers, verification commands, and done-when criteria are corrected accordingly.**

---

## Context

`src/Main.cs` is a 7014-line orchestrator in a single file (`public partial class Main : Control`, no sibling partials). It directly constructs, wires, ticks, and saves every domain system via `SetupXxx()`/`SaveXxx()` method pairs, and coordinates the four expansions surfaced through `ExpansionMasterSession`:

| # | Expansion | Codename | Status (per AGENTS.md Expansion System section) |
|---|-----------|----------|--------|
| 01 | Holdfast | `holdfast` | Active, most mature |
| 02 | Duty Roster | `duty_roster` | Active, wiring complete |
| 03 | Standing Record | `standing_record` | Partial — Phase 11 stubs |
| 04 | Nobody's Crossing / Nobody's Charter | `crossing` | Partial — Phase 11 stubs |

Confirm the current status of Standing Record and Crossing wiring by reading `Main.cs`'s `SetupExpansions()` (line 1527) directly before starting Step 1 — do not assume the table above is still current without checking, since this same batch just found one stale status table (GameBootstrap) already.

**Current problems:**
- `GameBootstrap.Phase0Expansion.cs` is cited in AGENTS.md's Invariant 4/Expansion sections as containing "six systems constructed/registered/ticked but key effects are stubs ... wired in Phase 11" — that file does not exist; if this problem is still real, it now lives somewhere in `Main.cs`'s `SetupPhase0()` (line 2155) or `SetupExpansions()` (line 1527). **Step 1 of this batch must locate the actual current "Phase 11" stub markers by grep before assuming they still exist at all** — this is itself an unverified claim inherited from stale documentation.
- No formal expansion lifecycle contract. Each expansion's wiring is scattered across `SetupXxx`/`SaveXxx` method pairs inside one file, with initialization order implicit in call order rather than declared.
- Adding a new expansion requires editing `Main.cs` in multiple places (construction, event wiring, init call order, tick registration, `SaveAll()`).
- The single 7014-line file makes navigation and reasoning about initialization order difficult — not because of file-count sprawl (there is only one file) but because of sheer length and the number of unrelated concerns interleaved in it.

**Target state:** `Main.cs` becomes a thin orchestrator (target: significantly smaller — see revised sizing note in Step 6) that discovers and initializes `IExpansionModule` implementations. Each expansion owns its own lifecycle in a single module class in Core. Non-expansion systems (world, medical, crafting, etc. that are not part of the four numbered expansions) are explicitly out of scope for this batch — decomposing those is a separate, larger effort and must not be silently folded into "expansion cleanup."

---

## Step 1 — Audit Main.cs Expansion-Related Sections

**Goal:** Produce a complete classification of every expansion-related `SetupXxx`/`SaveXxx` method (and any related tick/event-wiring code) inside `src/Main.cs`, establishing the real scope of work — replacing the original "classify 82 partial files" step, which described files that don't exist.

**Implementation:**
- Read `src/Main.cs` in full (7014 lines — use `read_file` with offset/limit in chunks, or grep first to jump to relevant line ranges; the Setup/Save method line numbers found during this review are a starting point: `SetupExpansions` at 1527, `SetupHoldfastRuntime` at 1445, `SetupDutyRoster` at 1463, `SaveHoldfast`/`SaveHoldfastRuntime`/`SaveDutyRoster`/`SaveExpansionHub` at 2019-2117, `SetupPhase0`/`SavePhase0` at 2155/2276 — re-verify these line numbers before relying on them, since editing the file during earlier steps will shift them).
- Classify each expansion-relevant method into one of three categories:
  - **(a) Active wiring** — constructs systems, registers ticks, wires events, manages save fields for Holdfast or Duty Roster. These contain logic that must be preserved.
  - **(b) Phase 11 / deferred stubs** — grep for `Phase 11`, `wired in Phase 11`, `// TODO`, `// stub` near Standing Record / Crossing code paths. Confirm whether these markers still exist at all before assuming AGENTS.md's description is current (see Corrected Premise above).
  - **(c) Dead code** — unreachable logic, commented-out blocks, duplicate registrations.
- Produce an audit document (`docs/main-cs-expansion-audit.md`) listing:
  - Method name, line range, category, systems it touches, dependencies on other methods/fields.
  - For stubs: what the stub was intended to do (from comments or naming) — or "no stub markers found, AGENTS.md claim could not be confirmed" if that's the actual finding.
  - For dead code: evidence it's unreachable.
- Identify initialization order dependencies (call-order dependencies in `_Ready()` or equivalent entry point — locate and document that entry point explicitly, since `Main.cs` being a `Control`-derived Godot node means lifecycle is driven by Godot's node tree, not a plain constructor).
- Explicitly scope this audit to expansion-related methods only (Holdfast, DutyRoster, ExpansionHub, Phase0, StandingRecord/Muster/Verdict if those map to expansion 03/04, Crossing). Do not expand the audit to the other ~30+ non-expansion Setup/Save pairs (World, Medical, Crafting, etc.) — that is out of scope per the Context section above.

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # no changes yet, must still compile
dotnet build Ashfall.csproj
```

**Done when:** `docs/main-cs-expansion-audit.md` exists with every expansion-related Setup/Save method classified. Each entry has a category, line range, and dependency note. No code changes in this step. The audit explicitly states whether "Phase 11" stub markers were actually found (do not carry AGENTS.md's claim forward unverified).

---

## Step 2 — Define Expansion Registration Interface

**Goal:** Create a formal lifecycle contract that any expansion module must implement, replacing the implicit "scatter Setup/Save method pairs across Main.cs" pattern.

**Implementation:**
- Create `Assets/Ashfall.Core/Expansions/IExpansionModule.cs`:
  ```csharp
  public interface IExpansionModule
  {
      string ExpansionId { get; }          // e.g. "holdfast", "duty_roster"
      int ExpansionNumber { get; }         // 01, 02, 03, 04
      bool IsEnabled { get; }

      void Initialize(ExpansionContext context);
      void OnTick(int currentDay);
      void WireEvents(Ashfall.Core.Events.IEventBus bus);
      object CaptureState();
      void RestoreState(object state);
      void Dispose();
  }
  ```
  **Correction — `SystemState` is not a real type in this codebase and must not be used here.** The original design's `SystemState CaptureState()` / `RestoreState(SystemState state)` signature assumes a shared generic state base type. A symbol search across `Assets/Ashfall.Core/` confirms no such type exists: every stateful system defines its own uniquely-named state DTO returned from a same-shaped `CaptureState()` method — `BrineWaterSystemState`, `CensusClaimSystemState`, `CohortSystemState`, `CombatState`, `CraftingSystemSave`, `DiseaseSystemState`, `DoseLedgerSystemState`, `DutyRosterSystemState`, `HoldfastSave`, and dozens more, each specific to its own system (confirmed by direct grep of `public \w+ CaptureState()` across Core — no two systems share a state type, and there is no common interface or base class among them). AGENTS.md's own Save/Load section describes the pattern as if `SystemState` were a real shared type ("Every stateful system implements: `public SystemState CaptureState()`") — that phrasing in AGENTS.md is itself imprecise/aspirational, not a literal type name, and this batch must not copy it into a compilable interface. Since each of the four expansion modules (`HoldfastModule`, `DutyRosterModule`, `StandingRecordModule`, `CrossingModule`) will wrap *multiple* systems that each have their own distinct state type, `IExpansionModule.CaptureState()` returns `object` (a composite, module-defined container — e.g. a small `HoldfastModuleState` DTO holding the several system-specific state objects Holdfast's `SetupHoldfastRuntime`/`SaveHoldfast`/`SaveHoldfastRuntime` currently capture) and `RestoreState(object state)` casts back to that module-specific container type internally. This is corrected in the interface above and must be carried through Steps 3, 4, and 7 (which referenced `SystemState` implicitly via `CaptureAllState()`/`RestoreAllState()` — those now operate on `IReadOnlyDictionary<string, object>` keyed by `ExpansionId`, not a `SystemState` collection).
  Note: `Ashfall.Core.Events.IEventBus` already exists (`Assets/Ashfall.Core/Events/IEventBus.cs`) with a string-based `Publish(string eventName, object payload = null)` signature and a `SimpleEventBus` default implementation — confirmed real, use it as-is rather than inventing a new bus. Per AGENTS.md's Event System section this bus is "defined, underused" — this batch is one of the first real consumers of it, which is a reasonable and desirable direction, but note it explicitly in the module classes' doc comments so future readers understand why `IEventBus` usage is appearing here for the first time in an expansion context.
- Create `Assets/Ashfall.Core/Expansions/ExpansionContext.cs` — a read-only bag of shared dependencies (`IJsonSerializer`, `IFileIO`, `ILog`, `ISeededRng`, `IClock`, references to core systems the expansion may interact with). Use the real `Ports.cs` interface shapes (confirmed: `ISeededRng.Next(int, int)`, `NextFloat()`, `NextDouble()`, `Seed` — no `Fork`/`DeriveChild`; `IFileIO` has `DirectoryExists`, `FileExists`, `ReadAllText`, `WriteAllText`, `Combine` — no `FlushToDisk`/`MoveReplace` unless Batch 65 adds them first, see cross-batch note below).
- Create `Assets/Ashfall.Core/Expansions/ExpansionRegistry.cs` — maintains an ordered list of registered modules, provides `InitializeAll()`, `TickAll()`, `CaptureAllState()` (returns `IReadOnlyDictionary<string, object>` keyed by `ExpansionId`, not a `SystemState` collection — see correction above), `RestoreAllState(IReadOnlyDictionary<string, object> state)`, `DisposeAll()`.
- Ensure no engine coupling: no `UnityEngine.*`, no `Godot.*`.
- **Cross-batch note:** if Batch 65 (Defensive Save Validation) has already landed `AtomicFileWriter`/backup rotation by the time this step runs, `ExpansionContext`'s `IFileIO` reference should be the same instance the save pipeline uses — do not construct a second `IFileIO` implementation here.

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

**Done when:** `IExpansionModule`, `ExpansionContext`, and `ExpansionRegistry` compile in Core. The registry can register, enumerate, and invoke lifecycle methods on modules. At least one unit test proves registration and ordered initialization.

---

## Step 3 — Create Per-Expansion Module Classes

**Goal:** Consolidate each expansion's scattered `Main.cs` wiring into a single cohesive module class that implements `IExpansionModule`.

**Implementation:**
- Create four module classes in `Assets/Ashfall.Core/Expansions/Modules/`:
  ```
  HoldfastModule.cs
  DutyRosterModule.cs
  StandingRecordModule.cs
  CrossingModule.cs
  ```
- Each module class:
  - Implements `IExpansionModule`.
  - Contains all system construction that was previously in `Main.cs`'s `SetupHoldfastRuntime`/`SetupDutyRoster`/`SetupExpansions`/`SetupPhase0` (and related `SaveXxx` counterparts) for that expansion — confirm the exact current method names and line ranges via the Step 1 audit before migrating, since this review found the originally-cited method/location names (`GameBootstrap.Phase0Expansion.cs` etc.) do not exist and the real names differ.
  - Owns its event subscriptions (wired in `WireEvents`, using the confirmed-real `Ashfall.Core.Events.IEventBus`).
  - Owns its tick logic (delegating to the expansion's systems).
  - Owns its save state (delegating to its systems' individual `CaptureState()`/`RestoreState(...)` methods internally, then composing their distinct state types into one module-specific state DTO returned as `object` from `IExpansionModule.CaptureState()` — see Step 2's `SystemState` correction; there is no shared state type to delegate to directly).
- `HoldfastModule` and `DutyRosterModule` are full implementations (migrate from `Main.cs`'s active-wiring methods for those expansions).
- `StandingRecordModule` and `CrossingModule` are partial — they contain whatever active wiring exists plus clearly marked TODOs for any confirmed-real stubs found in Step 1's audit (do not invent "Phase 11" TODOs if Step 1 found no such markers).
- Each module has a `// Systems owned by this module:` header comment listing every system it constructs.

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

**Done when:** Four module classes exist, compile, and contain the equivalent logic from their respective `Main.cs` Setup/Save methods. Existing tests still pass (no behavioral change yet — wiring just moved).

---

## Step 4 — Migrate Active Wiring from Main.cs into Modules

**Goal:** Remove expansion-specific logic from `src/Main.cs` and route it through the `ExpansionRegistry` instead.

**Implementation:**
- In `Main.cs` (the Godot host orchestrator — a `Control`-derived node, so this wiring likely lives in or near its `_Ready()`/init path; confirm the exact entry point during Step 1's audit before writing this code), replace per-expansion construction/wiring with:
  ```csharp
  _expansionRegistry = new ExpansionRegistry();
  _expansionRegistry.Register(new HoldfastModule());
  _expansionRegistry.Register(new DutyRosterModule());
  _expansionRegistry.Register(new StandingRecordModule());
  _expansionRegistry.Register(new CrossingModule());
  _expansionRegistry.InitializeAll(context);
  ```
- Replace per-expansion tick calls with `_expansionRegistry.TickAll(currentDay)`.
- Replace per-expansion save capture with `_expansionRegistry.CaptureAllState()` (returns `IReadOnlyDictionary<string, object>` keyed by `ExpansionId` — see Step 2's `SystemState` correction) — but note `Main.cs`'s existing `SaveAll()` (line 6227, confirmed) calls 29 `SaveXxx()` methods total (confirmed by direct count of the method's body), not just the 4-5 expansion-related ones. This step only replaces the expansion-related subset inside `SaveAll()`; the other ~24 non-expansion save calls stay exactly as they are. Do not refactor `SaveAll()` wholesale — that is scope creep beyond this batch's stated goal (expansion wiring cleanup, not general save-pipeline refactor).
- Replace per-expansion restore with `_expansionRegistry.RestoreAllState(savedState)` where `savedState` is the `IReadOnlyDictionary<string, object>` produced by the matching `CaptureAllState()` call (or reconstructed from the save file's expansion-keyed sections on load).
- Delete the content of `Main.cs` methods that are now empty (the specific `SetupXxx`/`SaveXxx` methods for Holdfast/DutyRoster/StandingRecord/Crossing whose logic moved to modules) — but since `Main.cs` is one file, "deleting empty partial file shells" (as the original plan assumed) does not apply; instead, delete the now-empty method bodies and their call sites.
- Preserve initialization order: expansions initialize in number order (01→04) — confirm this matches the current call order in `SetupExpansions()`/`SetupPhase0()` before changing it, since silently reordering initialization is itself a behavior change with save/load risk.

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # all existing tests pass
dotnet build Ashfall.csproj
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --data-integrity-selftest
```

**Done when:** `Main.cs` no longer contains Holdfast/DutyRoster/StandingRecord/Crossing-specific construction, tick, event, or save logic. All of it routes through `ExpansionRegistry`. Full test suite passes. Save round-trips are unchanged — specifically verify this with a manual save-in-old-code / load-in-new-code (or vice versa) smoke test, since save-field layout changes are the highest-risk part of this step.

---

## Step 5 — Remove Confirmed Stub Markers or Implement Them

**Goal:** Eliminate any ambiguous deferred-work markers found in Step 1's audit by either implementing the intended behavior or explicitly removing the dead stubs — but only for markers Step 1 actually confirmed exist. Do not manufacture "Phase 11" work if the audit found none.

**Implementation:**
- For each confirmed stub marker identified in the Step 1 audit (if any — Step 1 must state explicitly whether AGENTS.md's "wired in Phase 11" claim was verified or refuted):
  - **If the stub's intent is clear and scoped**: implement it in the appropriate module class. Write a test.
  - **If the stub's intent is unclear or would require significant new systems**: remove the stub entirely. Add a TODO comment in the module referencing the original intent and linking to a tracking issue or design doc.
  - **If the stub is a no-op wrapper** (method exists but does nothing): delete it.
- Document decisions in `docs/expansion-stub-resolution.md`:
  - For each stub: original location (real file/line in `Main.cs`, not the fictional `GameBootstrap.Phase0Expansion.cs`), what it said, decision (implemented / removed / deferred with rationale).
- Ensure no silent behavior changes: if a stub was being called (even as a no-op), removing it must not break callers. Verify call sites.
- **If Step 1 finds zero stub markers**, this step's done-when is trivially satisfied — record that finding in `docs/expansion-stub-resolution.md` and move on. Do not treat "no stubs found" as a failure requiring you to invent work.

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
```

**Done when:** `docs/expansion-stub-resolution.md` explicitly states whether stub markers were found and, for each one found, records implemented/removed/deferred with rationale. No test regressions.

---

## Step 6 — Reduce Main.cs's Expansion-Related Footprint to a Thin Orchestrator Call

**Goal:** The expansion-related portion of `src/Main.cs` shrinks to a handful of lines (registry construction + registration + lifecycle delegation calls). This step does NOT claim to shrink the whole 7014-line file to under 300 lines — that original target assumed all 7014 lines were expansion-related, which is false. Of the 30 total `SaveXxx` methods in `Main.cs` (confirmed count, listed at line numbers 1374–3975 plus `SaveAll` itself at 6227), only 5 are unambiguously expansion-related by name — `SaveHoldfast` (2019), `SaveHoldfastRuntime` (2029), `SaveDutyRoster` (2042), `SaveExpansionHub` (2057), `SavePhase0` (2276) — leaving 25 non-expansion `SaveXxx` methods (World, Medical, Crafting, Combat, Narrative, PowerGrid, Memorial, and more) explicitly out of scope for this batch. Whether `SaveMuster` (3127) and `SaveVerdict` (3244) also belong to Standing Record (expansion 03) is exactly the ambiguity Step 1's audit must resolve — do not assume either way before checking.

**Implementation:**
- After Steps 3–5, the expansion-specific portions of `Main.cs` (previously spread across `SetupHoldfastRuntime`, `SetupDutyRoster`, `SetupExpansions`, `SetupPhase0`, `SaveHoldfast`, `SaveHoldfastRuntime`, `SaveDutyRoster`, `SaveExpansionHub`, `SavePhase0`, and their call sites in `SaveAll()`) should collapse to:
  - Registry construction + 4 `Register()` calls + `InitializeAll()` call in the setup path.
  - A single `_expansionRegistry.TickAll(currentDay)` call wherever per-day tick dispatch happens.
  - A single `_expansionRegistry.CaptureAllState()` call inside `SaveAll()`, replacing the 5 unambiguous expansion-specific save calls it previously made (`SaveHoldfast`, `SaveHoldfastRuntime`, `SaveDutyRoster`, `SaveExpansionHub`, `SavePhase0` — plus `SaveMuster`/`SaveVerdict` if Step 1's audit confirms they belong to Standing Record).
  - A single `_expansionRegistry.RestoreAllState(...)` call in the load path.
- Realistic sizing target: the ~10 expansion-related Setup/Save methods currently spanning roughly 600–800 lines (estimate — confirm actual span from the Step 1 audit) collapse to roughly 20–30 lines of registry orchestration. This does not materially change `Main.cs`'s total line count relative to its ~30 other non-expansion Setup/Save pairs, and that is expected and fine — decomposing the rest of `Main.cs` is a separate future batch, not this one.
- Update any `using` directives and namespace imports.
- Run full verification to confirm no regressions.

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --data-integrity-selftest
```

**Done when:** Every expansion-specific Setup/Save method pair for Holdfast/DutyRoster/StandingRecord/Crossing has been removed from `Main.cs` and replaced by `ExpansionRegistry` calls. The net line reduction in `Main.cs` is measured and reported (expected: a few hundred lines removed from a 7014-line file — not a reduction to "<300 lines total," which was never an achievable target given the file's actual non-expansion content). Full verification passes.

---

## Step 7 — Write Expansion Lifecycle Tests

**Goal:** Prove that the module system correctly handles the full expansion lifecycle: initialization, ticking, event wiring, state capture/restore, and disposal.

**Implementation:**
- Create `Ashfall.Core.Tests/ExpansionLifecycleTests.cs` with test groups:
  - **Group A — Registration & Init:**
    - Modules register in order.
    - `InitializeAll` calls each module's `Initialize` exactly once.
    - Duplicate registration throws.
    - Disabled modules are skipped during init.
  - **Group B — Tick:**
    - `TickAll` invokes each enabled module in registration order.
    - A module that throws during tick does not prevent subsequent modules from ticking (fault isolation).
    - Tick with no registered modules is a no-op (no throw).
  - **Group C — Save/Restore Round-Trip:**
    - `CaptureAllState` returns an `IReadOnlyDictionary<string, object>` keyed by each module's `ExpansionId`, where each value is that module's own composite state DTO (see Step 2's `SystemState` correction — there is no shared state type, so this test asserts dictionary keys and per-key type identity, not a single shared shape).
    - `RestoreAllState` with the captured dictionary restores each module to its prior state — asserted per-module by re-capturing after restore and comparing the module-specific DTO's fields (test helper equality, not a generic comparer, since each module's DTO differs).
    - Round-trip: capture → mutate → restore → re-capture → assert re-captured state equals the original captured state for each module individually.
    - Missing module state in the restore dictionary (expansion added after save was created) → that module's `RestoreState` receives `null`/absent and initializes fresh instead of throwing.
  - **Group D — Disposal:**
    - `DisposeAll` calls each module's `Dispose` in reverse order.
    - Double-dispose is safe (idempotent).
    - Disposed registry rejects new operations.
  - **Group E — Integration (per-module smoke):**
    - Instantiate each of the 4 real modules with a test `ExpansionContext`.
    - Run: init → tick(day=1) → capture → tick(day=2) → restore(day=1 state) → assert day-1 state.
    - Covers `HoldfastModule`, `DutyRosterModule`, `StandingRecordModule`, `CrossingModule`.

**Verification:**
```
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "ExpansionLifecycle"
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # full suite
dotnet build Ashfall.csproj
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --data-integrity-selftest
```

**Done when:** At least 20 tests pass across groups A–E. Fault isolation is proven. Save round-trips work per-module and compositely. All 5 verification steps pass.

---

## Summary

| Step | Deliverable | Location | Test Coverage |
|------|-------------|----------|---------------|
| 1 | Audit document classifying expansion-related Setup/Save methods in `Main.cs` | `docs/main-cs-expansion-audit.md` | none (analysis only) |
| 2 | `IExpansionModule`, `ExpansionContext`, `ExpansionRegistry` | `Assets/Ashfall.Core/Expansions/` | registration + init test |
| 3 | 4 module classes (Holdfast, DutyRoster, StandingRecord, Crossing) | `Assets/Ashfall.Core/Expansions/Modules/` | compile + existing tests |
| 4 | `Main.cs` migration to registry-based wiring (expansion-scoped subset of `SaveAll()` only) | `src/Main.cs` | full suite regression |
| 5 | Confirmed-stub resolution (or explicit "none found" record) + `docs/expansion-stub-resolution.md` | modules + docs | per-stub tests where implemented |
| 6 | `Main.cs` expansion footprint reduced to registry calls (NOT a <300-line total-file target) | `src/Main.cs` | full suite regression |
| 7 | Expansion lifecycle tests (20+ tests, 5 groups) | `Ashfall.Core.Tests/ExpansionLifecycleTests.cs` | A–E groups pass |

**Total estimated effort:** 5–6 focused sessions (up from the original 4–5 — Step 1 now requires reading and mapping a 7014-line single file rather than skimming 82 small ones, which is not necessarily faster)
**Risk mitigation:** Step 1 is read-only (no code changes). Steps 2–3 are additive (new files, no deletions). Step 4 is the critical swap — run full verification after each expansion's migration (migrate and verify Holdfast, then DutyRoster, then StandingRecord, then Crossing — one at a time, not all four at once, since `Main.cs` is a single file and a bad edit is harder to isolate than in a multi-file structure). Steps 5–6 are cleanup after the architecture is proven. Step 7 locks in the contract.
**Rollback:** Since all edits in Steps 4 and 6 land in one file (`src/Main.cs`), commit after each expansion's migration is verified (per the "Git Rules" in AGENTS.md: "Commit after each accepted deliverable... one system per task"). If a migration step breaks save/load compatibility, `git revert` the single commit for that expansion rather than attempting to hand-unpick changes from a large diff.

**Dependency note:** This batch can run in parallel with Batch 61 (Save Migration Framework) if that batch exists and touches only Core save codecs, not `Main.cs`'s save orchestration directly. If Batch 65 (Defensive Save Validation, reviewed alongside this one) lands first, this batch's `ExpansionContext.IFileIO` should reuse whatever atomic-write-capable `IFileIO` Batch 65 produces rather than wiring a separate instance — see the cross-batch note in Step 2.

---

## Review Notes (Corrected)

This batch was adversarially reviewed against the live codebase at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`. Findings:

1. **The batch's entire premise was false.** `GameBootstrap` does not exist anywhere in the current tree. A `file_search` for `GameBootstrap` returns zero source files; a full-tree grep returns hits only in historical planning/audit markdown files (`ASHFALL_GAME_MASTER_DOCUMENT.md`, `CODE_AUDIT_REPORT.md`, `INTEGRATION_MASTER_PLAN.md`, `AUDIT_FINDINGS_AND_FIX_PLAN.md`, old `docs/superpowers/plans/2026-08-09-first-playable.md`), every one of which describes the Unity-era `Assets/_Game/Core/GameBootstrap.*.cs` structure. Per AGENTS.md's own rules, `Assets/_Game/` has been fully deleted as part of the Unity→Godot migration. AGENTS.md's own Invariant 5 / Expansion System sections still describe `GameBootstrap` as if current — that documentation is stale and should be flagged to the user/project maintainer separately from this batch.
2. **This batch was rewritten wholesale**, per the task instructions, to retarget the real equivalent structure: `src/Main.cs`, the active Godot host orchestrator. Confirmed by direct inspection: one file, 7014 lines, `public partial class Main : Control`, zero sibling `Main.*.cs` partials (there is no 82-file sprawl — the problem is a single oversized file, a different shape of problem than the original plan described).
3. **Corrected counts:** original plan said "1225-line god object spread across 82 partial files." Actual: 7014 lines, 1 file, 38 `SetupXxx()` methods, 30 `SaveXxx()` methods (including `SaveAll()` at line 6227). AGENTS.md's own H7 note ("~6.5k-line file... 31 Setup / 24 Save + SaveAll / 17 Flush methods") is closer to reality but is itself now stale — the file has grown to 7014 lines / 38 Setup / 30 Save since that note was written.
4. **Scope correction:** the original plan's "GameBootstrap becomes a thin orchestrator (<300 lines)" goal is unachievable as stated once retargeted to `Main.cs`, because the vast majority of `Main.cs`'s bulk is non-expansion system wiring. Precisely: of the 30 total `SaveXxx` methods, only 5 are unambiguously expansion-related by name (`SaveHoldfast`, `SaveHoldfastRuntime`, `SaveDutyRoster`, `SaveExpansionHub`, `SavePhase0`), leaving 25 non-expansion `SaveXxx` methods (World, Medical, Crafting, Combat, Narrative, PowerGrid, Memorial, and more) out of scope — this corrects an internal arithmetic slip in an earlier draft of this same corrected document, which said "roughly 26 of 30" in one place while also citing "the 5 expansion-specific save calls" in another (30 − 5 = 25, not 26; `SaveMuster`/`SaveVerdict`'s Standing-Record status is still an open question for Step 1 to resolve, not folded into either count here). The rewritten Step 6 scopes the size-reduction goal to only the expansion-related methods, and explicitly states that decomposing the rest of `Main.cs` is future, separate work — this prevents the batch from silently mutating into "rewrite the entire Godot host," which would be dangerous scope creep for a MEDIUM-priority batch.
5. **Unverified downstream claim flagged, not assumed:** the original plan's "Six systems are constructed/registered/ticked but key effects are stubs marked 'wired in Phase 11'" claim (from AGENTS.md, itself citing the deleted `GameBootstrap.Phase0Expansion.cs`) could not be verified against `Main.cs` in this review pass. Steps 1 and 5 were rewritten to require the implementer to explicitly confirm or refute this claim by grepping the real file, rather than carrying it forward as an assumed fact — this is exactly the kind of stale, unverified claim this review is meant to catch, and it appears twice in the same AGENTS.md paragraph (once for GameBootstrap's existence, once for the stub markers within it).
6. **`IExpansionModule`'s `WireEvents(IEventBus bus)` signature is retained** — confirmed real: `Ashfall.Core.Events.IEventBus`/`SimpleEventBus` exist in Core with the expected string-based `Publish` shape, so this part of the original design is sound and needed no change beyond namespace-qualifying the reference and confirming it's usable from a new Core subdirectory.
7. **Verification commands (dotnet build/test, godot --headless selftest flags) were already correct and runnable** — no changes needed there; they follow AGENTS.md's canonical 5-step checklist correctly and are retained.
8. **Risk/rollback was previously thin ("run full verification after each partial migration")** — this assumed many small files where a bad migration could be isolated per-file. With a single-file target, the rewritten Summary section adds an explicit per-expansion-commit strategy so a bad migration can be reverted with `git revert` on one commit rather than requiring a hand-unpick from one large diff touching `Main.cs` in four places at once.

### Second review pass — additional finding not caught by the first pass

This document already contained the eight corrections above before this review began. Independently re-verifying every claim against the live codebase (not trusting the prior pass's findings on faith) surfaced one additional, previously unflagged defect and confirmed everything else:

9. **`SystemState` is not a real type — Step 2's original `IExpansionModule` interface would not compile.** The prior review pass retained `SystemState CaptureState()` / `void RestoreState(SystemState state)` in `IExpansionModule` without checking whether `SystemState` exists anywhere in Core. It does not: a symbol search across `Assets/Ashfall.Core/` confirms every stateful system defines its own uniquely-named state DTO (`BrineWaterSystemState`, `CensusClaimSystemState`, `CohortSystemState`, `CombatState`, `CraftingSystemSave`, `DiseaseSystemState`, `DoseLedgerSystemState`, `DutyRosterSystemState`, `HoldfastSave`, and dozens more — no two share a type, no common base or interface exists). AGENTS.md's own Save/Load section states the convention generically ("Every stateful system implements: `public SystemState CaptureState()`"), and both the original plan and the first review pass copied that generic phrasing into a compilable interface signature without checking it against an actual `grep` of `CaptureState()` return types. This has been corrected throughout the document: `IExpansionModule.CaptureState()` now returns `object` (a module-composed container of its constituent systems' distinct state types), `RestoreState(object state)` takes the matching container, and `ExpansionRegistry.CaptureAllState()`/`RestoreAllState(...)` operate on `IReadOnlyDictionary<string, object>` keyed by `ExpansionId` rather than an implied `SystemState` collection. Step 7's Group C test descriptions were also corrected to assert per-module state equality rather than a generic comparison, since no shared shape exists to compare generically.
10. **Everything else the first review pass claimed was independently re-verified and confirmed accurate on this pass**, including: `src/Main.cs` is exactly 7014 lines with zero sibling `Main.*.cs` partials (`find src -iname "Main.*.cs"` returns nothing besides re-finding `Main.cs` itself); exactly 38 `SetupXxx()` and 30 `SaveXxx()` methods; `SaveAll()` at line 6227 with exactly 29 call sites in its body (30 `SaveXxx` methods minus `SaveAll` itself, all called); the specific cited line numbers for `SetupHoldfastRuntime`, `SetupDutyRoster`, `SetupExpansions`, `SetupPhase0`, `SaveHoldfast`/`SaveHoldfastRuntime`/`SaveDutyRoster`/`SaveExpansionHub`, and `SavePhase0` all match their stated locations exactly; `ExpansionMasterSession` (`Assets/Ashfall.Core/ExpansionMasterSession.cs`) is real and exposes a `Holdfast` property; `Ashfall.Core.Events.IEventBus`/`SimpleEventBus` are real with the exact string-based `Publish(string, object)` shape cited; `Ports.cs`'s `ISeededRng`/`IFileIO` shapes match exactly as cited (no `Fork`/`DeriveChild`, no `FlushToDisk`/`MoveReplace`); `GameBootstrap` genuinely does not exist as a source file anywhere outside `_quarantine_legacy/` (a separately quarantined legacy copy, not part of the active `Assets/`, `src/`, or build-participating tree) and two references inside real Core files (`CrossingSession.cs`, `HoldfastSession.cs`) are comments explicitly stating those sessions *replace* `GameBootstrap`, not evidence it still exists.
11. **"Phase 11" grep result nuance:** re-running the grep independently found 4 hits for "Phase 11" in `src/` — all in `src/Host/AssetRegistry.cs`, `src/UI/GreenhousePanel.cs`, `src/UI/DutyRosterPanel.cs`, and `src/UI/SilentFoundryPanel.cs` — but every one refers to a "Phase 11 dashboard shell" (a UI/panel-routing concept) or "Phase 11/12 baseline," not a "wired in Phase 11" deferred-stub marker of the kind AGENTS.md's Expansion System section describes. This confirms (does not merely leave open, as the first pass stated) that the specific stub-marker claim inherited from AGENTS.md/the deleted `GameBootstrap.Phase0Expansion.cs` does not match anything findable in `Main.cs` today. Step 1 and Step 5 already instruct the implementer to re-run this grep themselves and record the actual finding rather than trust this document — that instruction is correct and is left as-is; this note just confirms the likely outcome without pre-deciding it for the implementer, since a fresh grep at implementation time could still find something a keyword-based review missed.
12. **Process note:** unlike Batch 61 (whose prior "corrected" pass undercounted save stores and codecs and required a second, deeper correction), this batch's prior corrected pass held up on every re-checked factual claim except the `SystemState` fabrication in finding 9. That is now fixed; no other changes were needed to this document's substance.
