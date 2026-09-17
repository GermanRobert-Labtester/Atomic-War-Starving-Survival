# ASHFALL — Quality Roadmap Batch 64

## Theme: Event Bus Consolidation — Merge Dual Bus Architecture

| Field | Value |
|-------|-------|
| **Priority** | HIGH |
| **Risk** | Medium — changes cross-system communication patterns |
| **Category** | Architectural Debt Reduction |
| **Layer** | `Assets/Ashfall.Core/Events/` + `src/` (Godot host wiring) |
| **Depends on** | None (self-contained refactor) — this batch's own event-suppression logic (Step 4) does not require Batch 61 to complete; it references Batch 61's plan document only as the authority on the exact save-store count, which this batch does not need to know precisely (see Step 4's correction) |
| **Blocks** | Future UI decoupling, replay system, event-driven testing |

---

## Problem Statement

**Corrected against the actual codebase.** Two event mechanisms genuinely exist today, but the original problem statement mischaracterized both their file locations and their current usage:

1. **`IEventBus` / `SimpleEventBus`** — string-based, constructor-injected. Both types are defined **in a single file**, `Assets/Ashfall.Core/Events/IEventBus.cs` (there is no separate `SimpleEventBus.cs` file — `Assets/Ashfall.Core/Events/` contains exactly one `.cs` file). `SimpleEventBus` is used extensively in tests (`EventTriggerTests.cs`, `StandaloneCoreSystemTests.cs`, `VerdictRadioSystemTests.cs`, `VerdictSystemTests.cs`, and others) and in production code via `HostEventAdapter` (`src/Host/HostEventAdapter.cs`), which is instantiated in `src/Main.cs` and wired to Year-of-Ash narrative event triggers (`event_the_thin_margin_disclosure`, `event_the_thirsty_season`, `event_osteophage_explanation`, `event_measurement_broadcast`) — **not just journal events** as the original problem statement implied. `_eventBus` in `Main.cs` is lazily constructed as a `SimpleEventBus` the first time `SetupJournal()`/the event adapter path runs.

2. **`EventBus` static class** — genuinely existed in Unity `Assets/_Game/Core/EventBus.cs` and was type-safe generics with allocation-free dispatch and editor profiling (confirmed via `git log --oneline -- "Assets/_Game/Core/EventBus.cs"`, which shows commit `4629321a chore: object pooling + event bus profiler + fast-forward stability`). **This file — and the entire `Assets/_Game/` tree — has been deleted** as part of the Unity→Godot migration completion (per AGENTS.md's "Bridge Shim — Removed" section); `Assets/_Game/` does not exist on disk at all anymore, confirmed by `ls Assets/_Game` failing. So "was deleted with the Unity host migration" is accurate, but this batch should not describe the Unity `EventBus` as something still theoretically referenceable for comparison — it is gone, full stop, and cannot be inspected as a design reference during this work.

3. **Direct `Action` delegates** — the Godot host (`Main.cs`) does wire `Action` delegates on systems for dirty flags and panel refresh (verified: e.g. `_dashboard.OnMenuRequested += ReturnToMenu;`, `_dutyRosterPanel.OnAssignmentChanged += UpdateHud;`, dozens more in `BuildUserInterface()`). This part of the original claim is accurate.

**Stale-reference warning:** `REPO_REVIEW_REPORT.md` (dated 2026-08-16) contains a finding, C7, claiming "Godot: no event bus integration at all — uses direct method calls." This is now **outdated** — `HostEventAdapter` demonstrably bridges `IEventBus`/`SimpleEventBus` into the Godot host today (confirmed live in `src/Main.cs`). `REPO_REVIEW_REPORT.md` also describes `Assets/_Game/` as containing 1337 files, which no longer exist. Do not treat `REPO_REVIEW_REPORT.md` as current-state documentation for this batch — it reflects an earlier point in the migration. Use it only as historical context for *why* the dual-bus situation arose, not as a source of current facts.

This triple-pattern creates confusion about where to publish events, makes cross-system propagation untestable in isolation, and loses the suppression capability that Unity's `EventBus.IsSuppressed` provided during save/load cycles. Restoring state currently risks cascading re-fires that corrupt dirty flags or trigger unintended side effects. This underlying architectural concern is real and the plan's motivation stands even after the corrections above.

---

## Step 1 — Audit All Event Communication Patterns

### Goal

Produce a complete inventory of every event publish/subscribe site across Core systems and the Godot host, categorized by mechanism (delegate, `SimpleEventBus`, direct call).

### Implementation

- Grep `Assets/Ashfall.Core/` for all `public event Action`, `public event EventHandler`, and `SimpleEventBus.Publish` / `SimpleEventBus.Subscribe` calls.
- Grep `src/` for all `.OnXxx +=`, `Action` delegate wiring in `Main.cs` and host sessions.
- Grep for `HostEventAdapter` usage — identify every event it bridges.
- Document each event: source system, event name/type, subscriber(s), mechanism, whether it fires during `RestoreState`.
- Identify events that re-fire during save/load (candidates for suppression).

### Verification

- Inventory document exists (can be inline comments or a markdown table in `docs/`).
- Every `Action` delegate in `Main.cs` is accounted for.
- Every `SimpleEventBus` publish site is listed.

### Done when

Complete event map with ≥90% coverage of Core + Godot host event sites, reviewed for accuracy.

---

## Step 2 — Design Unified TypedEventBus in Core

### Goal

Design a single event bus API that is type-safe, suppressible, constructor-injectable, allocation-friendly, and testable without an engine.

### Implementation

- Define `ITypedEventBus` interface in `Assets/Ashfall.Core/Events/`:
  ```csharp
  public interface ITypedEventBus
  {
      void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : struct;
      void Unsubscribe<TEvent>(Action<TEvent> handler) where TEvent : struct;
      void Publish<TEvent>(TEvent evt) where TEvent : struct;
      IDisposable SuppressAll();           // returns a guard; Dispose re-enables
      bool IsSuppressed { get; }
  }
  ```
- Events are `readonly struct` — zero allocation on publish, value-type safety.
- Suppression returns an `IDisposable` guard so `using (bus.SuppressAll()) { RestoreState(...); }` is the standard pattern.
- No string keys — type identity is the routing key.
- Constructor-injectable via `ITypedEventBus`; host wires a single instance at boot.
- Thread-safety: single-threaded assumption (game loop); no locks needed.
- Document design decisions in a header comment block.

### Verification

- Interface compiles in `Ashfall.Core` with zero engine references.
- Design reviewed against all three current patterns (delegates, SimpleEventBus, direct calls).
- Suppression API covers the save/load cycle requirement.

### Done when

`ITypedEventBus.cs` merged into `Assets/Ashfall.Core/Events/` with full XML-doc comments; design rationale documented.

---

## Step 3 — Implement TypedEventBus

### Goal

Provide a concrete, tested implementation of `ITypedEventBus` that replaces both the string-based bus and direct delegate patterns.

### Implementation

- Create `Assets/Ashfall.Core/Events/TypedEventBus.cs`:
  ```csharp
  public sealed class TypedEventBus : ITypedEventBus
  {
      private readonly Dictionary<Type, object> _handlers = new();
      private int _suppressionDepth;

      public bool IsSuppressed => _suppressionDepth > 0;

      public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : struct { ... }
      public void Unsubscribe<TEvent>(Action<TEvent> handler) where TEvent : struct { ... }
      public void Publish<TEvent>(TEvent evt) where TEvent : struct
      {
          if (_suppressionDepth > 0) return;
          // dispatch to all registered handlers for TEvent
      }
      public IDisposable SuppressAll() => new SuppressionGuard(this);

      private sealed class SuppressionGuard : IDisposable { ... }
  }
  ```
- Suppression is reentrant (depth counter); only re-enables when outermost guard disposes.
- Unsubscribe during publish is safe (snapshot iteration or deferred removal).
- Add `SubscriptionCount<TEvent>()` for diagnostics/testing.

### Verification

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # compiles
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj    # unit tests pass (Step 7)
```

### Done when

`TypedEventBus` passes all unit tests; zero engine references; allocation-free publish path for the common case (no boxing of the struct event).

---

## Step 4 — Add Event Suppression for Save/Load Cycles

### Goal

Prevent cascading event re-fires during `RestoreState` calls, replacing the deleted Unity `EventBus.IsSuppressed` functionality.

### Implementation

- In every save store's `RestoreState` path, wrap restoration in a suppression guard:
  ```csharp
  using (eventBus.SuppressAll())
  {
      system.RestoreState(state);
  }
  ```
- Update `Main.cs` (Godot host) load pipeline to suppress the bus before restoring any system state.
- **Correction:** the original plan said "wrap restoration in every save store's `RestoreState` path" for "all 22 save stores." This conflates two different, unrelated batches: the save-store count/versioning work belongs to Batch 61, and most save stores do not currently route through `IEventBus`/`SimpleEventBus` at all (only the Year-of-Ash narrative path does, via `HostEventAdapter`). **The exact save-store count cited for cross-reference needs its own correction: Batch 61's plan document (`ASHFALL_NEXT_STEPS_QUALITY_ROADMAP_BATCH_61.md`, itself independently re-verified as part of this review) states the real count is 27 (as a floor, pending a full audit it explicitly calls for), not 25 — Batch 61's own text corrects an earlier "22 → 25" pass and then further corrects that second pass to 27, having found `src/Journal/JournalSaveStore.cs` and `src/YearOfAsh/YearOfAshSaveStore.cs` sitting outside the `src/Host/` directory the second pass search was scoped to.** A direct repo-wide grep for `public static class \w+SaveStore` performed during this review found 30 matching files (some via classes embedded inside otherwise-named host-session files, e.g. `GreenhouseHostSession.cs`, `StartingLevelHostSession.cs`, `ShelterAssignmentHostSession.cs`, which were not caught by a filename-only search for `*SaveStore.cs`). The precise number is Batch 61's concern to pin down, not this batch's — the only fact this batch needs is "more than a handful, and not all of them touch the event bus," which holds regardless of whether the true count is 27 or 30. Do not assume all save stores need bus-suppression wiring; audit which systems actually publish events during `RestoreState` (Step 1's inventory should answer this) and wrap only those. Retrofitting suppression into every save store as a blanket rule is scope creep beyond this batch's stated goal (event bus consolidation, not save-store hardening) and risks silently changing restore behavior for systems that have no event-cascade problem in the first place.
- Ensure `SaveAll` / `CaptureState` does NOT suppress (capturing should not block events; only restoration should).
- Add a `[SuppressDuringRestore]` attribute (optional, documentary) for systems whose events are known to cascade.
- Verify that UI dirty flags are not set during suppressed restore (they should be bulk-refreshed after restore completes).
- After restore completes and suppression lifts, fire a single `SaveRestoreCompleted` event so UI can do a one-time full refresh.
- **Risk/Rollback:** wrapping `RestoreState` calls in a suppression guard changes observable behavior for any code that currently relies on events firing during restore (e.g. a panel that refreshes itself reactively on a restore-triggered event, rather than via the planned bulk `SaveRestoreCompleted` refresh). Before merging, grep every current subscriber to events that fire during restore today and confirm each one either (a) doesn't need to run during restore, or (b) is migrated to also listen for `SaveRestoreCompleted`. This is manual audit work, not automatable, and should be budgeted as such.

### Verification

- Load a save with suppression enabled; verify zero spurious events fire during restore.
- Unit test: publish during suppression → handler NOT called; publish after guard disposal → handler called.
- Integration: save/load round-trip with event counters → counters are 0 during restore, correct after.

### Done when

Every save store confirmed by Step 1's audit to actually publish through `IEventBus`/`SimpleEventBus` during a restore-like operation restores under suppression (today, that is confirmed to be only the Year-of-Ash narrative path via `HostEventAdapter` — not "all 22," "all 25," or "all 27/30" save stores; the exact total store count is Batch 61's concern, not this one's); `SaveRestoreCompleted` event fires exactly once after a full load; no dirty-flag corruption observed.

---

## Step 5 — Migrate High-Value Systems to TypedEventBus

### Goal

Move the three highest-traffic event systems (Journal/Year-of-Ash narrative events, Economy, Expeditions) from direct delegates / `SimpleEventBus` to `TypedEventBus`, proving the pattern at scale.

### Implementation

- **Journal / Year-of-Ash narrative events** (`JournalSystem`, `HostEventAdapter`):
  - **Correction:** `HostEventAdapter` (`src/Host/HostEventAdapter.cs`) does not bridge generic journal entries — it subscribes to four specific, hardcoded Year-of-Ash narrative event IDs (`event_the_thin_margin_disclosure`, `event_the_thirsty_season`, `event_osteophage_explanation`, `event_measurement_broadcast`), and its handlers call `_journal?.TryAddRawEntry(...)` as one of several side effects (also raising `OnEventDispatched` and `StateChanged` C# events, and persisting trigger state via `CaptureState`/`RestoreState`). Treat this migration as "replace the Year-of-Ash narrative event bridge," not "replace all journal event delivery" — `JournalSystem` itself may have its own event surface independent of `HostEventAdapter` that needs to be inventoried separately (check Step 1's audit output before assuming scope).
  - Define event structs: `NarrativeEventDispatched` (carrying the event ID and description — mirrors what `HostEventAdapter.OnEventDispatched` already provides) rather than assuming generic `JournalEntryAdded`/`JournalCategoryUnlocked` structs match the real subscription surface; confirm against `JournalSystem`'s actual public API before naming these types.
  - Replace `HostEventAdapter` bridge with direct `TypedEventBus.Publish`.
  - Journal UI subscribes via `TypedEventBus.Subscribe<...>`.
  - Preserve `HostEventAdapter`'s save/load behavior (`CaptureState`/`RestoreState` of `HostEventState`, tracking which of the four events have already fired) — this state persistence is unrelated to the event *delivery* mechanism and must not be dropped during the migration.

- **Economy system** (`MarketSystem`, trade sessions):
  - **Correction:** the original plan named the migration target `DynamicEconomySystem`. That class does not exist in `Assets/Ashfall.Core/`. `DynamicEconomySystem` (`Assets/_Game/Economy/DynamicEconomySystem.cs`, 1797 lines) was the Unity-legacy host-side implementation named in AGENTS.md's Invariant 5 offender list — and `Assets/_Game/` no longer exists on disk at all (confirmed deleted, consistent with the completed Unity→Godot migration). The real, current Core economy system is `MarketSystem` (`Assets/Ashfall.Core/Economy/MarketSystem.cs:66`, `SystemId = "economy_market_system"`), wired in the Godot host as `_economy.Market` (referenced in `src/Main.cs`, e.g. `_economy.Market.GetDemandMultiplier(...)`). Use `MarketSystem`, not `DynamicEconomySystem`, throughout this migration.
  - Define event structs: `PriceChanged`, `TradeCompleted`, `LedgerDebtUpdated`.
  - Replace `Action` delegates in `Main.cs` economy wiring with bus subscriptions.
  - Economy panel refresh subscribes to `PriceChanged`.

- **Expedition system** (`ExpeditionSystem`, encounters):
  - **Correction:** the original plan named the migration target `ExpeditionSession`. That class does not exist anywhere in the repository. The real class is `ExpeditionSystem` (`Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs`), wired in the Godot host as `ExpeditionHostSession` (`src/Main.cs:106`, `private ExpeditionHostSession _expeditions`), which exposes the underlying engine as `_expeditions.Engine`. Use `ExpeditionSystem`/`ExpeditionHostSession`, not `ExpeditionSession`, throughout this migration. Note also that `DiveInstanceRunner` (`Assets/Ashfall.Core/Expeditions/DiveInstanceRunner.cs`) is a separate, already-`IEventBus`-coupled class in the same `Expeditions/` namespace (see Step 6's correction below) — decide whether it is included in this "Expedition system" migration or deliberately deferred, since leaving it on `IEventBus` while `ExpeditionSystem` moves to `TypedEventBus` would split the Expeditions domain across two bus mechanisms, which is the exact fragmentation this batch exists to undo.
  - Define event structs: `ExpeditionStarted`, `EncounterResolved`, `ExpeditionCompleted`.
  - Replace direct delegate callbacks with bus publishes.
  - Expedition UI subscribes via bus.

- All event structs live in `Assets/Ashfall.Core/Events/` grouped by domain subfolder.
- Each migration: remove old delegate/subscribe site, add `Publish`, verify no behavioral change.

### Verification

```
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj    # all existing tests still pass
godot --headless --path . -- --data-integrity-selftest      # catalog integrity: 0 errors
```
- Manual smoke: trigger each event path, confirm UI updates still fire. Specifically for the narrative migration: trigger each of the four Year-of-Ash events (`event_the_thin_margin_disclosure`, `event_the_thirsty_season`, `event_osteophage_explanation`, `event_measurement_broadcast`) via their existing gating conditions and confirm the journal entry is still written and the HUD notification still appears.

### Done when

Journal/narrative, `MarketSystem` (Economy), and `ExpeditionSystem` (Expeditions) publish all state-change events through `TypedEventBus`; zero direct `Action` delegates remain for these three systems in `Main.cs`; `HostEventState` save/load behavior (which of the four narrative events have fired, and on which day) is preserved exactly; a written decision exists on whether `DiveInstanceRunner` (same `Expeditions/` namespace, also `IEventBus`-coupled) is in or out of scope for this batch — "out of scope" is acceptable but must be explicit, not a silent omission; all tests green.

---

## Step 6 — Deprecate SimpleEventBus and HostEventAdapter

### Goal

Remove the legacy string-based bus infrastructure now that `TypedEventBus` handles all traffic.

### Implementation

- Mark `SimpleEventBus` and `IEventBus` (string-based) with `[Obsolete("Use ITypedEventBus")]`.
- Remove `HostEventAdapter` entirely — its bridging role is now unnecessary, **provided Step 5 has already migrated the four Year-of-Ash narrative event triggers it currently owns and confirmed `HostEventState` persistence (which triggers have fired, on which day) is preserved by the replacement.** Do not remove `HostEventAdapter` before Step 5 completes; it is the only production (non-test) consumer of `SimpleEventBus` found in this review, and removing it first would delete the last real caller before its replacement exists.
- Migrate any remaining `SimpleEventBus.Publish`/`Subscribe` call sites to `TypedEventBus`. **Correction — this is a larger scope-creep risk than "test files" implies.** A repo-wide grep for `IEventBus`/`SimpleEventBus` constructor usage finds it is a constructor dependency of three **production** Core classes, not just test fixtures: `DiveInstanceRunner` (`Assets/Ashfall.Core/Expeditions/DiveInstanceRunner.cs`), `VerdictRadioSystem` (`Assets/Ashfall.Core/Verdict/VerdictRadioSystem.cs`), and `VerdictCensusBroadcast` (`Assets/Ashfall.Core/Verdict/VerdictCensusBroadcast.cs`) all take `IEventBus`/`SimpleEventBus` as a constructor parameter and are shipped, non-test code. Test files (`EventTriggerTests.cs`, `StandaloneCoreSystemTests.cs`, `VerdictRadioSystemTests.cs`, `VerdictSystemTests.cs`, and others) construct `SimpleEventBus` directly specifically because they're instantiating these three production classes, not because the tests themselves publish/subscribe independently. **Deleting `IEventBus`/`SimpleEventBus` per this step's original plan therefore breaks production code, not merely test code**, unless `DiveInstanceRunner`, `VerdictRadioSystem`, and `VerdictCensusBroadcast` are also migrated to `ITypedEventBus` first. None of these three classes were in Step 5's "high-value systems" list (Journal/narrative, Economy, Expeditions) — decide explicitly whether they are in scope for this batch (as a required Step 5b) or whether this batch ships with `IEventBus` kept alive solely to serve these three call sites, in which case Step 6's "delete `IEventBus`/`SimpleEventBus` entirely" goal is not achievable within this batch and should be descoped to "deprecate but keep" until a follow-up batch migrates these three.
- Remove `SimpleEventBus` registration from `HostDefaults` / DI container — **this line item does not apply: confirmed via direct inspection of `Assets/Ashfall.Core/HostDefaults.cs` that it contains zero references to `IEventBus` or `SimpleEventBus`.** There is no DI/registration line for the event bus in that file to remove. `Main.cs` constructs `SimpleEventBus` directly and inline (`private Ashfall.Core.Events.SimpleEventBus _eventBus = new Ashfall.Core.Events.SimpleEventBus();`, `src/Main.cs:144`), not through `HostDefaults`. This line item should be removed from the plan rather than left as a to-verify note.
- After one release cycle with `[Obsolete]`, delete:
  - The `IEventBus` interface and `SimpleEventBus` class — **both currently live in the single file `Assets/Ashfall.Core/Events/IEventBus.cs`; there is no separate `SimpleEventBus.cs` file to delete.** Deleting "the file" means deleting this one file, or extracting-then-deleting only the `SimpleEventBus` class if `IEventBus` itself needs to be kept for some other consumer — re-check consumers before deciding which.
  - `src/Host/HostEventAdapter.cs` — confirmed to exist at this exact path.
- Update `Ports.cs` — **this claim does not hold up: `IEventBus` is not defined in, and has never been part of, `Assets/Ashfall.Core/Ports.cs`.** Direct inspection of `Ports.cs` shows it contains exactly five interfaces — `IJsonSerializer`, `IFileIO`, `ILog`, `IClock`, `ISeededRng` — all in the `Ashfall.Core` namespace, with no `IEventBus` anywhere in the file. `IEventBus` is defined separately in `Assets/Ashfall.Core/Events/IEventBus.cs`, in the `Ashfall.Core.Events` namespace. There is therefore no line to remove from `Ports.cs` for this batch — that file is unaffected by this migration. (The earlier "verify before editing" hedge in this step was the right instinct; the verification now confirms the claim itself was wrong, not just unconfirmed.)

### Verification

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # no obsolete-usage warnings from Core
dotnet build Ashfall.csproj                                 # Godot host: 0 errors
grep -r "SimpleEventBus\|IEventBus" Assets/Ashfall.Core/    # 0 non-obsolete references
grep -r "SimpleEventBus\|IEventBus" Ashfall.Core.Tests/     # tests must be migrated or explicitly exempted — do not leave this unchecked
grep -r "HostEventAdapter" src/                             # 0 references
```

### Done when

Zero references to `SimpleEventBus`, `IEventBus` (string-based), or `HostEventAdapter` remain in active code (Core, host, AND tests — the original Done-when omitted tests, where the majority of current `SimpleEventBus` usage actually lives); `TypedEventBus` is the sole event infrastructure. `HostEventState`'s save/load contract (which of the four Year-of-Ash events fired, and when) still round-trips correctly after the migration.

---

## Step 7 — Write Event Bus Integration Tests

### Goal

Comprehensive test coverage for the unified event bus: publish, subscribe, unsubscribe, suppression, cross-system propagation, and edge cases.

### Implementation

- Create `Ashfall.Core.Tests/Events/TypedEventBusTests.cs`:
  - `Publish_DeliversToSubscriber` — basic pub/sub.
  - `Publish_MultipleSubscribers_AllReceive` — fan-out.
  - `Publish_WhenSuppressed_HandlersNotCalled` — suppression blocks delivery.
  - `Publish_AfterSuppressionLifted_HandlersCalledAgain` — guard disposal re-enables.
  - `NestedSuppression_OnlyLiftsOnOutermostDispose` — reentrant depth.
  - `Unsubscribe_StopsDelivery` — clean removal.
  - `Unsubscribe_DuringPublish_DoesNotThrow` — concurrent modification safety.
  - `Publish_NoSubscribers_DoesNotThrow` — silent no-op.
  - `Publish_DifferentEventTypes_Isolated` — type routing correctness.

- Create `Ashfall.Core.Tests/Events/EventBusSaveLoadIntegrationTests.cs`:
  - `RestoreState_UnderSuppression_NoEventsFired` — simulates full load cycle.
  - `AfterRestore_SaveRestoreCompleted_FiredOnce` — UI refresh signal.
  - `CaptureState_NotSuppressed_EventsStillFire` — save path is not suppressed.

- Create `Ashfall.Core.Tests/Events/EventBusCrossSystemTests.cs`:
  - `JournalReceivesEconomyEvent_ViaTypedBus` — cross-domain propagation.
  - `ExpeditionCompletion_TriggersJournalEntry` — end-to-end event chain.
  - `SuppressedRestore_DoesNotCorruptDirtyFlags` — regression guard.

### Verification

```
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~Events"
```
All event bus tests pass; coverage ≥95% of `TypedEventBus` lines.

### Done when

≥15 tests covering publish, subscribe, unsubscribe, suppression (including nested), cross-system propagation, and save/load integration; all green; no flaky tests.

---

## Summary Table

| Step | Description | Risk | Key Deliverable |
|------|-------------|------|-----------------|
| 1 | Audit all event communication patterns | Low | Event inventory document |
| 2 | Design unified TypedEventBus in Core | Low | `ITypedEventBus.cs` interface |
| 3 | Implement TypedEventBus | Medium | `TypedEventBus.cs` concrete class |
| 4 | Add event suppression for save/load cycles | Medium | Suppression guards for systems confirmed (via Step 1's audit) to actually publish through `IEventBus` during restore — not a blanket "all save stores" change; only the Year-of-Ash/narrative path is confirmed to need this today |
| 5 | Migrate high-value systems (Journal/narrative via HostEventAdapter, `MarketSystem` Economy, `ExpeditionSystem` Expeditions) | Medium | 3 systems fully on TypedEventBus; explicit in/out-of-scope decision recorded for `DiveInstanceRunner` |
| 6 | Deprecate SimpleEventBus and HostEventAdapter (after Step 5; also migrate/exempt test-file AND production usages — see corrected Step 6) | Medium | Legacy bus removed from Core, host, and tests — contingent on `DiveInstanceRunner`/`VerdictRadioSystem`/`VerdictCensusBroadcast` disposition |
| 7 | Write event bus integration tests | Low | ≥15 tests, all green |

---

## Success Criteria

- Single event bus implementation (`TypedEventBus`) handles all inter-system communication.
- Zero references to `SimpleEventBus` or string-based event routing in active code.
- Save/load cycles are fully suppressed — no cascading re-fires during `RestoreState`.
- All existing tests continue to pass (no behavioral regression).
- Event bus is fully testable without engine dependencies.
- `Main.cs` has fewer direct `Action` delegate wirings (moved to bus subscriptions).

---

## Estimated Effort

| Step | Effort |
|------|--------|
| 1 | 2–3 hours |
| 2 | 1–2 hours |
| 3 | 3–4 hours |
| 4 | 4–6 hours |
| 5 | 6–8 hours |
| 6 | 2–3 hours |
| 7 | 3–4 hours |
| **Total** | **21–30 hours** |


---

## Review Notes (Corrected)

This batch was adversarially reviewed against the actual codebase at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`. Corrections made:

1. **`IEventBus`/`SimpleEventBus` existence and location — CONFIRMED with a location correction.** Both types genuinely exist, but in a single file: `Assets/Ashfall.Core/Events/IEventBus.cs`. There is no separate `SimpleEventBus.cs` file anywhere in the repo — Step 6's file-deletion list originally named one that doesn't exist. `Assets/Ashfall.Core/Events/` contains exactly one `.cs` file total.

2. **"Only the journal receives events through `HostEventAdapter`" — FALSE, CORRECTED.** `HostEventAdapter` (`src/Host/HostEventAdapter.cs`) is real, is constructed and wired live in `src/Main.cs` (`_hostEventAdapter = new AtomicWar.GodotApp.Host.HostEventAdapter(_eventBus, _journal);`), and subscribes to four specific Year-of-Ash narrative event IDs — not a generic journal event stream. Its handlers write to the journal as one side effect among several (also firing `OnEventDispatched`/`StateChanged` C# events and persisting trigger state). This is a materially different, narrower scope than "the journal receives events," and Step 5/6 were rewritten to describe the real subscription surface and to require preserving `HostEventState`'s save/load contract through the migration.

3. **Unity `EventBus` static class — CONFIRMED it existed, CONFIRMED it and `Assets/_Game/` are now fully deleted.** `git log --oneline -- "Assets/_Game/Core/EventBus.cs"` shows real commit history including `4629321a chore: object pooling + event bus profiler + fast-forward stability`, supporting the "type-safe generics, allocation-free, editor profiling" characterization. `Assets/_Game/` does not exist on disk at all today (`ls` fails) — fully consistent with AGENTS.md's "Bridge Shim — Removed" section. The original problem statement's characterization was accurate; no correction needed here beyond noting it can no longer be inspected as a live reference.

4. **`REPO_REVIEW_REPORT.md` is STALE — flagged, not used as a source of current fact.** That report (dated 2026-08-16) claims "Godot: no event bus integration at all — uses direct method calls," which is now false (`HostEventAdapter` demonstrably bridges the bus into the Godot host), and describes `Assets/_Game/` as containing 1337 files, which no longer exist. Per the project's own task workflow ("Check REPO_REVIEW_REPORT.md for known issues in the area you're touching"), this batch should read that report for historical motivation only, not as ground truth for the current state of the event system — the current codebase has moved on from what it describes.

5. **Scope creep — Step 4's "all 22 save stores" blanket suppression requirement removed.** The original Step 4 conflated this batch's event-bus work with Batch 61's save-store inventory (which itself needed correcting from 22 to a verified 25 stores — see that document) and assumed every save store publishes through `IEventBus` during restore. Only the Year-of-Ash/narrative path was confirmed during this review to actually touch `IEventBus` during a restore-like operation. Blanket-wrapping all save stores in a suppression guard they don't currently need is scope creep relative to this batch's stated goal and was corrected to be conditional on Step 1's audit findings.

6. **Missing risk/rollback — added to Step 4 and Step 6.** Step 4 now requires a manual audit of every subscriber to events that fire during restore today, since suppressing them changes observable behavior for any UI code relying on reactive refresh rather than the new `SaveRestoreCompleted` signal. Step 6 now explicitly sequences `HostEventAdapter` removal *after* Step 5 completes (removing the bus's only confirmed production caller before its replacement exists would be a straightforward regression), and calls out that test files are the largest current consumer of `SimpleEventBus` (`EventTriggerTests.cs`, `StandaloneCoreSystemTests.cs`, `VerdictRadioSystemTests.cs`, `VerdictSystemTests.cs`, and others construct it directly, including for non-"high-value" classes like `DiveInstanceRunner` and `VerdictRadioSystem` that Step 5 never planned to migrate) — the original Done-when criteria for Step 6 only checked `Assets/Ashfall.Core/` and `src/`, silently ignoring the test tree where most current usage lives.

7. **Unverified claims flagged rather than asserted.** This review did not directly inspect `HostDefaults.cs` for a `SimpleEventBus` DI registration, nor `Ports.cs`'s exact content/location. Step 6 now says to verify these before editing rather than presenting them as confirmed facts, since they could not be checked within this review's scope.

8. **Ordering is logically sound:** Step 1 (audit) → Step 2 (design) → Step 3 (implement) → Step 4 (suppression, now correctly scoped) → Step 5 (migrate real consumers) → Step 6 (deprecate/delete, now correctly sequenced after Step 5) → Step 7 (tests) is the right sequence and required no structural change — only the scope and factual claims within each step needed correction.

---

## Review Notes (Corrected) — Second Pass

An independent second adversarial pass re-verified this plan, including the first pass's own "Review Notes," directly against the live repository at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`. The first pass's corrections mostly held up, but several claims presented as confirmed were wrong or need real revision:

9. **CRITICAL — Step 6's "Update `Ports.cs`" instruction is factually wrong, not merely unverified.** The first pass flagged this as "verify before editing." This pass did verify it by reading `Ports.cs` directly: it contains exactly `IJsonSerializer`, `IFileIO`, `ILog`, `IClock`, `ISeededRng` — no `IEventBus`, and never has (the event-bus interface lives in a separate file, `Events/IEventBus.cs`, in a separate namespace, `Ashfall.Core.Events`). There is no line in `Ports.cs` for this batch to touch. Fixed: Step 6's `Ports.cs` bullet now states this plainly instead of deferring it as an open question.
10. **CRITICAL — Step 6 undersold the blast radius of deleting `IEventBus`/`SimpleEventBus`.** The first pass correctly found that test files construct `SimpleEventBus`, but described the risk as being about test-file usage. Direct inspection of the actual constructor call sites shows `IEventBus`/`SimpleEventBus` is a required constructor dependency of three **production, non-test** Core classes: `DiveInstanceRunner`, `VerdictRadioSystem`, and `VerdictCensusBroadcast`. The test files that construct `SimpleEventBus` do so specifically to instantiate these production classes under test — they are not independent test-only usages. Deleting `IEventBus` per the original Step 6 plan would therefore not compile the production Core assembly, not just the test assembly. Fixed: Step 6 now names these three classes explicitly, raises this step's risk rating from Low to Medium, and requires an explicit decision (migrate these three in an added Step 5b, or formally keep `IEventBus` alive for their sake and descope Step 6's "delete entirely" goal for this batch).
11. **The Batch 61 cross-reference count was wrong.** The first pass said Batch 61 "corrected the store count from 22 to a verified 25." Reading `ASHFALL_NEXT_STEPS_QUALITY_ROADMAP_BATCH_61.md` directly shows it corrects 22 → 25 → **27** (finding `JournalSaveStore` and `YearOfAshSaveStore` outside the originally-searched `src/Host/` directory), and explicitly calls 27 a floor pending further audit, not a final number. A repo-wide grep performed during this pass for `public static class \w+SaveStore` found 30 matches, including several defined inside otherwise-named host-session files that a `*SaveStore.cs` filename-only search would miss. Fixed: Step 4 and its Done-when no longer cite a specific wrong number; both now point to Batch 61 as the authority on the exact count and state that this batch's own logic (only bus-touching stores need suppression) doesn't depend on knowing the precise total.
12. **Step 5's Economy and Expedition migration targets were both wrong class names.** `DynamicEconomySystem` does not exist in `Assets/Ashfall.Core/` — it was the Unity-legacy class (`Assets/_Game/Economy/DynamicEconomySystem.cs`), and `Assets/_Game/` has been fully deleted as part of the completed migration (confirmed: `ls Assets/_Game` fails). `ExpeditionSession` does not exist anywhere in the repository under that name. Fixed: Step 5 now names the real classes — `MarketSystem` (`Assets/Ashfall.Core/Economy/MarketSystem.cs:66`) for Economy, and `ExpeditionSystem`/`ExpeditionHostSession` (`Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs`, `src/Main.cs:106`) for Expeditions — and flags that `DiveInstanceRunner` sits in the same `Expeditions/` namespace and is already `IEventBus`-coupled, creating a scope question Step 5 must answer explicitly rather than leave implicit.
13. **The first pass's other verifications held up under re-checking.** `IEventBus`/`SimpleEventBus` living in a single file (`Events/IEventBus.cs`, confirmed to contain exactly those two types and nothing else); `HostEventAdapter`'s four exact narrative event IDs and its `OnEventDispatched`/`StateChanged`/`CaptureState`/`RestoreState` surface (confirmed by reading `src/Host/HostEventAdapter.cs` in full); `Main.cs:144`'s inline `SimpleEventBus` construction and `Main.cs:1386`'s `HostEventAdapter` wiring; the Unity `EventBus.cs` git history (commit `4629321a`, plus an earlier introducing commit `03f8fcda` this pass additionally found); `Assets/_Game/` being fully absent from disk; and `REPO_REVIEW_REPORT.md`'s C7 finding text and 1337-file figure — all independently re-confirmed by direct file reads rather than by trusting the first pass's citations.
14. **`HostDefaults.cs` DI-registration claim: confirmed absent, not just unverified.** The first pass flagged this as something it hadn't checked. Direct inspection of `Assets/Ashfall.Core/HostDefaults.cs` confirms zero references to `IEventBus` or `SimpleEventBus` in that file — there is no registration line to remove. Fixed: Step 6 now states this as confirmed rather than an open item.
