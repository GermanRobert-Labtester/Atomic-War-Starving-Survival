# ASHFALL — Quality Roadmap Batch 76

## Theme: Undo/Redo Framework for Player Decisions

| Field | Value |
|-------|-------|
| **Batch** | 76 |
| **Priority** | LOW-MEDIUM |
| **Risk** | HIGH |
| **Estimated Effort** | 10-14 working days |
| **Prerequisite Batches** | None (leverages existing CaptureState/RestoreState) |
| **Systems Touched** | CommandHistory (new), IPlayerCommand (new), ~94 systems in `Assets/Ashfall.Core/` that implement CaptureState/RestoreState (via a new snapshot abstraction layered on top — see Step 3 and Review Notes), Godot host UI layer |

---

## Motivation

In a permadeath survival game, a single misclick can cascade into game-over:
- Wrong ration allocation starves a survivor.
- Accidental expedition send loses your medic.
- Wrong trade confirmation spends irreplaceable medicine.
- Misassigned work shift leaves the air filter unmaintained.

No undo exists today (verified: no `ICommand`, `IPlayerCommand`, `CommandHistory`, or `IStateSnapshotProvider` symbol exists anywhere in the repo — this genuinely is new work, not a duplication of something already present). The `CaptureState/RestoreState` pattern exists on **approximately 94 systems** in `Assets/Ashfall.Core/` (verified by counting `public ... CaptureState(` method definitions directly — **not** the "82+" figure the original draft used; see Review Notes for why that number is unreliable) and already provides the architectural foundation — a full state snapshot before each player action would enable undo. But a complete undo/redo system requires:

1. **Command pattern** for all player actions (not just state mutations).
2. **State snapshotting** before each undoable action.
3. **Bounded history** (memory-conscious; 300-day sessions can't store unlimited snapshots).
4. **Irreversibility detection** (some actions genuinely can't be undone: days passing, random encounters resolving).
5. **Save system integration** (undo history is session-local, not persisted across save/load).

This is HIGH risk because it touches the boundary between every system's state and the player action layer, and — see the new performance-risk analysis in Step 3 and the Risks table — because capturing ~94 systems' state on every player action, not just once per day, is a fundamentally different and much more expensive operation than the "SaveAll()" analogy the plan draws elsewhere. The existing CaptureState/RestoreState contract means system *serialization* is solved, but each `RestoreState` implementation carries its own deep-copy discipline (verified: e.g. `BrineWaterSystem.cs`, `CensusClaimSystem.cs`, `DutyRosterSystem.cs` all have explicit "deep-copy: the deserialized DTO must not become the live state" comments) — a generic snapshot wrapper must preserve that discipline per system, not assume a single generic clone strategy works uniformly.

---

## Step 1 — Design IPlayerCommand Interface

### Goal
Define a clean command pattern interface that wraps every discrete player action, distinguishing undoable from irreversible actions.

### Implementation
- Create `Assets/Ashfall.Core/Undo/IPlayerCommand.cs`:
  ```csharp
  namespace Ashfall.Core.Undo;

  /// <summary>
  /// Represents a discrete player action that can be executed and potentially undone.
  /// </summary>
  public interface IPlayerCommand
  {
      /// <summary>Human-readable description (e.g., "Assigned Kim to water duty").</summary>
      string Description { get; }

      /// <summary>Whether this command can be undone after execution.</summary>
      bool IsUndoable { get; }

      /// <summary>Execute the command, mutating game state.</summary>
      CommandResult Execute();

      /// <summary>Category for grouping in UI (optional).</summary>
      CommandCategory Category { get; }
  }

  public enum CommandCategory
  {
      ResourceAllocation,   // Ration, fuel, water distribution
      Crafting,             // Queue craft, cancel craft
      Trade,                // Accept/reject trade offers
      Expedition,           // Send, recall, reassign
      Assignment,           // Work shifts, duty roster
      Medical,              // Administer treatment, triage
      Construction,         // Build, upgrade, repair
      Social,               // Leadership decisions, faction responses
      Other
  }

  public sealed class CommandResult
  {
      public bool Success { get; init; }
      public string? FailureReason { get; init; }

      public static CommandResult Ok() => new() { Success = true };
      public static CommandResult Fail(string reason) => new() { Success = true, FailureReason = reason };
  }
  ```
- Create `Assets/Ashfall.Core/Undo/ICommandInterceptor.cs`:
  ```csharp
  /// <summary>
  /// Hook point for systems that need to react before/after command execution.
  /// Used by CommandHistory to capture state before and validate after.
  /// </summary>
  public interface ICommandInterceptor
  {
      void BeforeExecute(IPlayerCommand command);
      void AfterExecute(IPlayerCommand command, CommandResult result);
  }
  ```
- Document the invariant: commands are pure player-initiated actions. System-internal state changes (day tick, weather roll, disease progression) are NOT commands — they are simulation steps that cannot be undone individually.

### Verification
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

### Done-when
- `IPlayerCommand` interface compiles in Core with zero engine references.
- `CommandCategory` enum covers all player action domains.
- `CommandResult` provides success/failure feedback.
- `ICommandInterceptor` enables pre/post hooks without coupling.
- Clear documentation distinguishes commands (player actions) from simulation ticks (not undoable).

---

## Step 2 — Implement CommandHistory

### Goal
Build the bounded state-snapshot history that stores pre-action game states and enables undo/redo traversal.

### Implementation
- Create `Assets/Ashfall.Core/Undo/CommandHistory.cs`:
  ```csharp
  namespace Ashfall.Core.Undo;

  public sealed class CommandHistory : ICommandInterceptor
  {
      private readonly int _maxDepth;
      private readonly IStateSnapshotProvider _snapshotProvider;
      private readonly List<HistoryEntry> _undoStack;
      private readonly List<HistoryEntry> _redoStack;

      public CommandHistory(IStateSnapshotProvider snapshotProvider, int maxDepth = 5)
      {
          _maxDepth = maxDepth;
          _snapshotProvider = snapshotProvider;
          _undoStack = new(maxDepth);
          _redoStack = new(maxDepth);
      }

      public bool CanUndo => _undoStack.Count > 0;
      public bool CanRedo => _redoStack.Count > 0;
      public int UndoCount => _undoStack.Count;
      public IReadOnlyList<string> UndoDescriptions => _undoStack.Select(e => e.Command.Description).ToList();
      public IReadOnlyList<string> RedoDescriptions => _redoStack.Select(e => e.Command.Description).ToList();

      public void BeforeExecute(IPlayerCommand command) { ... } // Snapshot if undoable
      public void AfterExecute(IPlayerCommand command, CommandResult result) { ... } // Commit or discard

      public UndoResult Undo() { ... }  // Restore previous snapshot
      public RedoResult Redo() { ... }  // Re-apply next snapshot

      public void Clear() { ... }  // Called on save/load (history is session-local)
  }

  public sealed class HistoryEntry
  {
      public IPlayerCommand Command { get; init; } = null!;
      public GameStateSnapshot BeforeState { get; init; } = null!;
      public GameStateSnapshot AfterState { get; init; } = null!;
  }
  ```
- Create `Assets/Ashfall.Core/Undo/IStateSnapshotProvider.cs`:
  ```csharp
  public interface IStateSnapshotProvider
  {
      GameStateSnapshot CaptureFullState();
      void RestoreFullState(GameStateSnapshot snapshot);
  }
  ```
- `GameStateSnapshot` is a dictionary of system names to their `SystemState` DTOs (already serializable via CaptureState).
- Bounded at configurable depth (default 5) — oldest entries evicted on overflow.
- Redo stack clears when a new command is executed (standard undo/redo semantics).
- History clears on save/load (undo is session-local; restoring a save starts fresh).

### Verification
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~CommandHistory"
```

### Done-when
- `CommandHistory` manages bounded undo/redo stacks.
- Undo restores the `BeforeState` snapshot.
- Redo restores the `AfterState` snapshot.
- Stack eviction works at max depth.
- New command clears redo stack.
- Clear() resets all history.
- All operations are O(1) except snapshot capture/restore (which is O(systems)).

---

## Step 3 — Define Snapshot Strategy

### Goal
Determine and implement the optimal state-capture approach balancing memory usage AND per-action latency against restore fidelity. **This step must produce an actual measurement before the rest of the batch proceeds — the original draft asserted a memory estimate with no empirical basis; do not repeat that mistake for the time-cost side.**

### Critical performance concern (was missing from the original draft — see Review Notes)
The original plan estimated only memory cost ("~50-200KB per snapshot x 5 depth = 250KB-1MB. Acceptable for a desktop game") and asserted this with **no supporting measurement** — no save-size fixtures or benchmarks exist anywhere in this repo to substantiate it (verified: no `.json` save fixtures are checked in; `SaveStoreChecksumSweepTests`/`SaveWireContractTests` assert checksum/round-trip correctness, not size or timing). More importantly, the plan never separately analyzes **CPU/wall-clock cost**, which is the actual risk for a HIGH-risk batch:

- A full snapshot means calling `CaptureState()` on **all ~94 systems**, each producing its own DTO (many involving `List<T>` allocations, deep copies, and in some cases JSON-serializable nested structures), followed by a full `IJsonSerializer` pass if snapshots are persisted in any serialized form.
- Unlike `SaveAll()` (invoked once per explicit save action, a rare, player-initiated, "loading spinner is acceptable" event), this system proposes capturing a snapshot **before every single undoable player action** — ration changes, craft-queue edits, trade decisions, work-shift reassignments. These are UI-frequency events, potentially many per minute during an active session, not once-per-day or once-per-save events.
- If capturing 94 systems' state takes even a few milliseconds, that cost is paid synchronously on the UI thread for every ration slider drag or shift reassignment click, which is a materially different performance profile than the once-per-day-tick or once-per-explicit-save operations the rest of the codebase already tolerates.
- No existing code path in this repo calls `CaptureState()` on all systems at UI-interaction frequency — the closest existing precedent, `SaveAll()` (`H7`, `src/Main.cs`), is explicitly a per-day/per-explicit-save operation, not a per-click operation. This plan is proposing a new performance regime, not reusing a proven one, and this must be treated as the primary technical risk of the batch, not a secondary one.

**Required before any implementation in this step is considered done:**
1. Write a throwaway benchmark (not shipped, or shipped as a `[Fact(Skip=...)]`/explicit perf test) that constructs a realistic mid-game session (all systems populated, day 50+) and measures wall-clock time to call `CaptureState()` on every registered system once, and again for `RestoreState()`.
2. If the measured cost exceeds roughly 1-2ms on typical desktop hardware (a reasonable per-click budget so the UI doesn't visibly stutter), the "full snapshot on every action" strategy in this step must be reconsidered before Steps 4-7 proceed — options include: snapshotting only the systems the command's `CommandCategory` plausibly touches (a scoped subset, not all 94), debouncing snapshot capture (e.g., only snapshot on button-release, not per-slider-tick), or moving capture off the main thread with a completion callback.
3. Document the measured number in this file (replace the placeholder estimate below) rather than leaving an unverified estimate in a plan that will be used to justify shipping the feature.

### Implementation
- **Full snapshot approach** (evaluate in this step; do NOT commit to it before the benchmark above is run):
  - Call `CaptureState()` on all ~94 registered systems.
  - Store all states in a `GameStateSnapshot` dictionary.
  - Restore calls `RestoreState()` on all systems.
  - Memory cost: unverified estimate carried over from the original draft (~50-200KB per snapshot); this number has no fixture or benchmark backing it in this repo and must be replaced with a measured figure before Step 3 is marked done. Time cost is the actual open question — see above.

- Create `Assets/Ashfall.Core/Undo/GameStateSnapshot.cs`:
  ```csharp
  public sealed class GameStateSnapshot
  {
      public Dictionary<string, object> SystemStates { get; init; } = new();
      public int Day { get; init; }
      public long Timestamp { get; init; }  // Monotonic tick for ordering
  }
  ```

- Create `Assets/Ashfall.Core/Undo/FullStateSnapshotProvider.cs`:
  ```csharp
  public sealed class FullStateSnapshotProvider : IStateSnapshotProvider
  {
      private readonly IReadOnlyList<IStatefulSystem> _systems;

      public GameStateSnapshot CaptureFullState()
      {
          var snapshot = new GameStateSnapshot { Day = _clock.CurrentDay };
          foreach (var system in _systems)
              snapshot.SystemStates[system.SystemName] = system.CaptureState();
          return snapshot;
      }

      public void RestoreFullState(GameStateSnapshot snapshot)
      {
          foreach (var system in _systems)
              if (snapshot.SystemStates.TryGetValue(system.SystemName, out var state))
                  system.RestoreState(state);
      }
  }
  ```

- Define `IStatefulSystem` (no shared interface exists today — every one of the ~94 systems has a bespoke `CaptureState()`/`RestoreState()` signature with its own concrete state DTO type, not a common `object`-typed signature; introducing `IStatefulSystem` means writing an adapter per system, not just declaring the interface):
  ```csharp
  public interface IStatefulSystem
  {
      string SystemName { get; }
      object CaptureState();
      void RestoreState(object state);
  }
  ```
  Each system's existing `CaptureState()` returns its own strongly-typed DTO (e.g. `DutyRosterSystemState`, `CombatState`) — wrapping ~94 of these behind a uniform `object`-returning interface means writing ~94 thin adapter methods (or extending each system directly), which is a non-trivial amount of mechanical work this step's estimate should account for explicitly, not wave away as "many systems already implement the pattern but lack a shared interface."

- **Future optimization (V2, not this batch)**: delta-only snapshots that only store systems whose state actually changed. Track dirty flags per system, only capture dirty ones, restore only those. **Given the performance concern above, seriously consider whether this "V2 optimization" needs to be pulled into V1** — if the full-snapshot benchmark in this step comes back too slow, a scoped/delta capture is not an optional future nicety, it is the blocking fix.

### Verification
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~Snapshot"
```

### Done-when
- A measured (not estimated) wall-clock benchmark for full-state capture and full-state restore is documented in this file, run against a realistic mid-game session, with the result compared against a stated per-action latency budget (recommend 1-2ms as a starting target; adjust if UI responsiveness testing says otherwise).
- If the measured cost is unacceptable, this step also documents which mitigation (scoped-by-category capture, debouncing, async capture) was chosen and why, before Steps 4+ proceed.
- `FullStateSnapshotProvider` captures and restores all registered stateful systems.
- Round-trip test: capture → mutate → restore → verify state matches original, run per-system (not just an aggregate check) so a single system's broken deep-copy doesn't hide behind an overall pass.
- Memory measurement test: snapshot of full game state (50-day simulation) size is measured and asserted (not just described in prose) with a concrete byte-count assertion in a test.
- Systems that have empty `CaptureState` (known issue: `LocationEvolutionSaveable`, `WildlifeSaveable`, `LandmarkSaveable`) are documented as undo-blind but don't block the framework.
- `IStatefulSystem` interface doesn't break existing system implementations — verified by confirming the adapter approach (wrapping, not modifying, each of the ~94 existing `CaptureState`/`RestoreState` signatures) compiles against every current system without changing their public signatures.

---

## Step 4 — Implement Undo for Resource Allocation Commands

### Goal
Create concrete command implementations for the most common misclick-prone player actions: ration allocation, crafting queue, and trade acceptance.

### Implementation
- Create `Assets/Ashfall.Core/Undo/Commands/SetRationLevelCommand.cs`:
  ```csharp
  public sealed class SetRationLevelCommand : IPlayerCommand
  {
      public string Description => $"Set rations to {_newLevel} for {_targetName}";
      public bool IsUndoable => true;
      public CommandCategory Category => CommandCategory.ResourceAllocation;

      private readonly NeedsSystem _needs;
      private readonly string _survivorId;
      private readonly string _targetName;
      private readonly RationLevel _newLevel;

      public CommandResult Execute()
      {
          _needs.SetRationLevel(_survivorId, _newLevel);
          return CommandResult.Ok();
      }
  }
  ```
- Create `QueueCraftCommand`, `CancelCraftCommand`, `AcceptTradeCommand`, `RejectTradeCommand`.
- Create `AllocateFuelCommand`, `AllocateWaterCommand` for resource distribution.
- Each command:
  - Is a plain C# class in Core (no engine dependencies).
  - Has a human-readable `Description` for the undo UI.
  - Returns `IsUndoable = true`.
  - The actual undo mechanism is state restoration (not inverse operations), so commands don't need their own undo logic.
- Create `Assets/Ashfall.Core/Undo/CommandExecutor.cs`:
  ```csharp
  public sealed class CommandExecutor
  {
      private readonly CommandHistory _history;

      public CommandResult Execute(IPlayerCommand command)
      {
          _history.BeforeExecute(command);
          var result = command.Execute();
          _history.AfterExecute(command, result);
          return result;
      }
  }
  ```

### Verification
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~Command"
```

### Done-when
- 6+ resource allocation commands implemented.
- `CommandExecutor` wires history capture around execution.
- Test: execute SetRationLevel → undo → verify ration is at previous value.
- Test: execute AcceptTrade → undo → verify inventory restored, trade still available.
- Test: execute QueueCraft → undo → verify craft queue empty, materials returned.
- Commands are pure data + action — no UI coupling.

---

## Step 5 — Implement Undo for Expedition and Assignment Commands

### Goal
Cover the highest-stakes player actions: sending survivors on expeditions and reassigning work shifts.

### Implementation
- Create `Assets/Ashfall.Core/Undo/Commands/SendExpeditionCommand.cs`:
  ```csharp
  public sealed class SendExpeditionCommand : IPlayerCommand
  {
      public string Description => $"Send {_survivorName} on expedition to {_destination}";
      public bool IsUndoable => true;  // Undoable ONLY before day advances
      public CommandCategory Category => CommandCategory.Expedition;
      // ...
  }
  ```
- Create `RecallExpeditionCommand` — undoable (survivor returns to available pool).
- Create `AssignWorkShiftCommand` — reassign a survivor's duty.
- Create `ReassignDutyRosterCommand` — bulk shift reassignment.
- Create `AdministerTreatmentCommand` — medical action (undoable if medicine not yet consumed by day tick). **Note (see Review Notes): there is no single `MedicalSystem` class in this codebase** — the medical domain is split across `Ashfall.Core.Medical.ChemicalDependencySystem`, `VigilStateMachine`, and `Ashfall.Core.Medical.MedicalWardSystem` (all in `Assets/Ashfall.Core/Medical/`), wired together by the host-side `MedicalHostSession`. When implementing this command, target the specific class that owns the treatment action in question rather than assuming a monolithic `MedicalSystem` exists.

- **Irreversibility boundary**: Once a day tick advances after the command, it becomes irreversible because simulation has consumed the action's effects. The command history does NOT span day ticks. **Correction (see Review Notes): there is no method literally named `OnDayAdvance()` anywhere in the codebase to hook.** The real, already-wired hook point is `CampaignDayCoordinator.Advance(int day, IDayAdvancePersistence persistence)` (`Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs`), which raises a genuine C# event `OnDayAdvanced` (`Action<DayAdvancedEventArgs>`, declared at line 40 of that file) after every owner has advanced. `CommandHistory` should subscribe to this existing event rather than the plan inventing a new method for the host to call:
  ```csharp
  // In CommandHistory, or wherever it's wired up in the host:
  campaignDayCoordinator.OnDayAdvanced += _ => Clear(); // Actions from previous days cannot be undone
  ```
  This is a deliberate design choice: undo covers misclicks within a decision phase, not retroactive day-reversal. Wiring via the existing `OnDayAdvanced` event (rather than requiring `Main.cs`/`HoldfastRuntimeSession` to remember to call a new bespoke method) also means the clear-on-advance behavior can't be silently skipped if a future day-advance path forgets to call it — it's automatic once subscribed.

### Verification
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~Command"
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~Expedition"
```

### Done-when
- 5+ expedition/assignment commands implemented.
- Test: send expedition → undo before day tick → survivor is back in shelter.
- Test: assign work shift → undo → survivor returns to previous assignment.
- Test: day advance clears history (cannot undo actions from previous day).
- Test: attempting undo after day advance returns `CanUndo = false`.
- Irreversibility boundary is clearly documented.

---

## Step 6 — Add Undo UI Integration

### Goal
Wire undo/redo into the Godot host UI layer with keyboard shortcuts and visual feedback.

### Implementation
- In Godot host (`src/UI/` or equivalent):
  - Register `Ctrl+Z` → `CommandHistory.Undo()`
  - Register `Ctrl+Y` / `Ctrl+Shift+Z` → `CommandHistory.Redo()`
  - Display undo button in action bar (greyed out when `!CanUndo`).
  - Show tooltip on undo button: last command description (e.g., "Undo: Set rations to half for Kim").

- **Action confirmation for irreversible commands**:
  - Commands where `IsUndoable = false` (if any are added later, e.g., "exile survivor") show a confirmation dialog before execution.
  - Confirmation text: "This action cannot be undone. Proceed?"

- **Undo feedback**:
  - On successful undo, show brief notification: "Undone: [description]"
  - On successful redo, show brief notification: "Redone: [description]"
  - Animate affected UI elements briefly to indicate state change.

- **History panel** (optional, lower priority):
  - List of recent undoable actions with descriptions.
  - Click any entry to undo back to that point (multi-step undo).

- Wire `CampaignDayCoordinator.OnDayAdvanced` event to call `CommandHistory.Clear()` in the day-advance flow (see Step 5 correction — this is the real event, not an invented `OnDayAdvance()` method), subscribed wherever the Godot host constructs/owns both objects (`Main.cs` or a dedicated host session).

### Verification
```bash
dotnet build Ashfall.csproj  # Godot host compiles
godot --headless --path . -- --bridge-selftest  # Stable CI verb exits 0
```

### Done-when
- Ctrl+Z/Ctrl+Y keybinds work in Godot host.
- Undo button shows/hides based on `CanUndo` state.
- Tooltip displays last command description.
- Day advance clears undo state and disables button.
- Notifications appear on undo/redo.
- No engine-specific code in Core (all UI is in Godot host layer).

---

## Step 7 — Write Undo/Redo Tests

### Goal
Comprehensive test suite verifying the complete undo/redo system works correctly across all scenarios.

### Implementation
- Create `Ashfall.Core.Tests/UndoRedoTests.cs`:
  ```csharp
  public class UndoRedoTests
  {
      [Fact]
      public void Undo_Restores_Exact_Previous_State()
      {
          // Execute command, capture state hash, undo, verify state hash matches pre-command
      }

      [Fact]
      public void Redo_Reapplies_Exact_Post_Command_State()
      {
          // Execute, undo, redo — verify state matches post-command state
      }

      [Fact]
      public void History_Depth_Limit_Evicts_Oldest()
      {
          // Execute 6 commands with depth=5, verify only last 5 are undoable
      }

      [Fact]
      public void New_Command_Clears_Redo_Stack()
      {
          // Execute A, undo, execute B — verify cannot redo A
      }

      [Fact]
      public void Day_Advance_Clears_All_History()
      {
          // Execute commands, advance day, verify CanUndo=false and CanRedo=false
      }

      [Fact]
      public void Non_Undoable_Command_Not_Added_To_History()
      {
          // Execute command with IsUndoable=false, verify history unchanged
      }

      [Fact]
      public void Failed_Command_Not_Added_To_History()
      {
          // Execute command that returns Fail, verify no snapshot taken
      }

      [Fact]
      public void Multiple_Undo_Steps_Restore_Progressively()
      {
          // Execute A, B, C — undo restores pre-C, undo again restores pre-B
      }

      [Fact]
      public void Undo_When_Empty_Returns_False()
      {
          // No commands executed, Undo() returns failure gracefully
      }

      [Fact]
      public void Full_Integration_Ration_Expedition_Trade()
      {
          // Wire real systems, execute ration+expedition+trade, undo all three in reverse
      }
  }
  ```

- Additional edge case tests:
  - Concurrent command execution (if applicable — likely single-threaded but verify).
  - Snapshot memory stays bounded (5 snapshots max, old ones GC'd).
  - RestoreState on all systems doesn't throw even with partial snapshots (graceful degradation for systems added after snapshot was taken).

### Verification
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~UndoRedo"
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj  # Full suite
dotnet build Ashfall.csproj
```

### Done-when
- All 10+ undo/redo tests pass.
- Full integration test exercises real systems (`NeedsSystem`, `ExpeditionSystem`, trade via `HoldfastTradeSession`/`MarketSystem` — verify exact class names against current codebase before writing the test, do not assume a generic "trade system" exists).
- Edge cases covered: empty history, depth overflow, failed commands, day boundary.
- Full test suite (current count: 2116 `[Fact]` + 4 `[Theory]` = 2120 total, verified directly against the repo at review time — **not** "1941+", which is a stale figure from an earlier point in the project's history; re-verify the count at execution time since it will have grown further + new) passes.
- No flaky tests (all deterministic, no timing dependencies).
- The Step 3 performance benchmark result (measured capture/restore latency) is re-confirmed against the final integrated implementation, not just the standalone prototype from Step 3 — command execution adds interceptor overhead (`BeforeExecute`/`AfterExecute`) on top of raw snapshot cost.

---

## Summary Table

| Step | Description | Key Deliverable | Risk | Dependencies |
|------|-------------|-----------------|------|--------------|
| 1 | Design IPlayerCommand interface | Command pattern contract + categories | Low | None |
| 2 | Implement CommandHistory | Bounded undo/redo stack with snapshots | Medium | Step 1 |
| 3 | Define snapshot strategy + **measure performance** | `FullStateSnapshotProvider` + `IStatefulSystem` + **benchmark result** | **High** (corrected from Medium — this step now carries the batch's core performance-viability question; see Motivation and Risks) | Step 2 |
| 4 | Undo for resource allocation commands | 6+ resource commands + executor | Medium | Steps 1-3 |
| 5 | Undo for expedition/assignment commands | 5+ high-stakes commands + day boundary (via real `OnDayAdvanced` event) | Medium | Steps 1-3 |
| 6 | Undo UI integration (Godot host) | Keybinds, button, notifications | Low-Med | Steps 1-5 |
| 7 | Undo/redo tests | 10+ comprehensive behavior tests | Low | Steps 1-6 |

**Note on effort estimate:** the original "10-14 working days" estimate did not separately budget for (a) writing and interpreting a real performance benchmark in Step 3, (b) potentially redesigning the snapshot strategy if that benchmark fails its latency budget, or (c) writing ~94 per-system adapters for `IStatefulSystem` (not a single interface declaration). Given a HIGH risk rating, the estimate should carry explicit contingency for a Step 3 pivot rather than assuming the "full snapshot" approach ships as first designed. Treat 10-14 days as the optimistic case; if the Step 3 benchmark forces a scoped/delta capture redesign, add a discrete follow-up estimate at that point rather than absorbing it silently into the same batch.

---

## Risks & Mitigations

| Risk | Likelihood | Impact | Mitigation |
|------|-----------|--------|------------|
| **Full-state snapshot per action is too slow for UI-frequency operations** (see Step 3) | **High** — no proven precedent exists in this codebase for calling ~94 systems' `CaptureState()` at per-click frequency; the closest analog (`SaveAll()`) only runs once per day/explicit save | **High** — a snapshot that takes even single-digit milliseconds, paid synchronously per ration-slider tick or shift-reassignment click, is a directly player-visible stutter, not a background cost | Mandatory benchmark in Step 3 before Steps 4+ proceed (see corrected Step 3); fallback to category-scoped capture (only snapshot systems the command's `CommandCategory` plausibly touches) if the full-snapshot benchmark fails the latency budget; this must be resolved with a measured number, not an estimate, before this risk can be downgraded |
| Full state snapshot too large (memory pressure in 300-day sessions) | Medium | Medium | Bounded depth (5); clear on day advance; measure in Step 3 with an actual test assertion, not a prose estimate |
| Systems with empty CaptureState cause silent data loss on undo | Known | Medium | Document known offenders (3 systems: `LocationEvolutionSaveable`, `WildlifeSaveable`, `LandmarkSaveable`); fix them as prerequisite or accept partial undo |
| RestoreState has side effects (triggers events, UI updates, sounds) | Medium | High | Suppress event bus during restore (`IsSuppressed` pattern from Unity `EventBus`, if that pattern is ported to Godot — note this pattern is currently Unity-only per AGENTS.md's Event System section, so a Godot-side equivalent needs to be built, not assumed to already exist); add restore mode flag |
| Player confusion about what can/can't be undone | Medium | Medium | Clear UI: greyed button, "cannot undo" toast, confirmation on irreversible actions |
| **`IStatefulSystem` interface requires ~94 adapter implementations, not a single interface declaration** | High (mechanical certainty, not a probability) | Medium — this is scope, not a failure risk, but the original draft's "Medium" likelihood for "breaks existing systems" undersold the sheer volume of adapter code needed | Use adapter pattern — don't require systems to implement `IStatefulSystem` directly; wrap existing CaptureState/RestoreState per system; budget explicit time for ~94 thin adapters in the effort estimate, not "wire it in" as a one-line afterthought |
| Redo after system added/removed (schema mismatch) | Low | Medium | Graceful skip: if system not in snapshot, leave current state; log warning |
| Deep-copy discipline varies per system and a generic wrapper could silently violate it | Medium | High | Every sampled `RestoreState` (`BrineWaterSystem`, `CensusClaimSystem`, `DutyRosterSystem`) has an explicit "must deep-copy, not alias the live state" comment; the snapshot wrapper must call each system's own `RestoreState`, never attempt a generic/reflection-based clone that could bypass this discipline |

## Rollback Plan

- The feature is additive and opt-in at the wiring layer: `CommandExecutor`/`CommandHistory` are new classes that the Godot host must explicitly construct and route player actions through. If a regression is found post-merge, the fastest rollback is to stop routing UI actions through `CommandExecutor` and call the underlying system methods directly again (as today) — this requires no data migration since undo history is never persisted (see Design Principle 6).
- Because undo history is explicitly session-local and never written to a save file, there is no save-compatibility rollback concern — reverting this feature cannot corrupt or orphan any persisted save data.
- If the Step 3 performance benchmark comes back unacceptable and no mitigation is viable in the estimated timeframe, the recommended fallback is to descope this batch to a narrower "single-action undo" (only the most recent action, depth=1, only for `ResourceAllocation` category commands) rather than shipping a full 5-deep, all-category undo system with a known UI stutter. This is a scope-reduction rollback, not a full revert, and should be flagged to the user before being chosen unilaterally.
- If `IStatefulSystem` adapter work (Step 3) turns out to be substantially larger than estimated (e.g., if several of the ~94 systems have `CaptureState`/`RestoreState` signatures that resist generic wrapping), the fallback is to scope the V1 undo system to an explicit allow-list of ~15-20 systems most relevant to the misclick scenarios in the Motivation section (ration, expedition, trade, work shift, crafting) rather than attempting universal coverage — this should be raised as a scope question, not silently decided.

---

## Design Principles

1. **Undo is for misclicks, not time travel** — History clears on day advance. Players cannot rewind simulation ticks, only their own actions within a decision phase.
2. **State restoration, not inverse operations** — Undo works by restoring a full snapshot, not by computing the reverse of each command. This is simpler, more reliable, and leverages the existing CaptureState/RestoreState infrastructure.
3. **Bounded and predictable** — Maximum 5 undo levels, session-local, cleared on day advance and save/load. No unbounded memory growth.
4. **Transparent to game logic** — Systems don't know about undo. They just implement CaptureState/RestoreState as they already do. The framework wraps around them.
5. **Graceful degradation** — Systems with broken CaptureState still work; they just lose state on undo. The framework logs a warning but doesn't crash.
6. **No save persistence** — Undo history is ephemeral. Loading a save starts with empty history. This prevents save-scumming via undo.

---

## Relationship to Existing Systems

| Existing Pattern | How Undo Leverages It |
|---|---|
| `CaptureState()` / `RestoreState()` on ~94 systems in `Assets/Ashfall.Core/` (verified count; corrects the original "82+" figure — see Review Notes) | Direct foundation — full state snapshots use these, but note this is a fundamentally different *call frequency* than existing usage (per-action vs. per-day/per-save) — see Step 3 performance analysis |
| `SaveAll()` orchestrating per-day/per-explicit-save persistence (per AGENTS.md H7, described as 24 save stores across the Godot host — not independently re-verified in this review since it's a host-layer count, not a Core count) | Similar *pattern* — `FullStateSnapshotProvider` captures all systems like `SaveAll` captures all stores — but `SaveAll` runs at a much lower frequency (once per day/save) than the per-action frequency this batch proposes; do not treat them as equivalent-cost operations |
| `EventBus.IsSuppressed` in Unity | This pattern is currently Unity-only per AGENTS.md's Event System section (the Godot host has no event bus at all today — it uses direct method calls). A Godot-side suppression mechanism needs to be **built new**, not ported, since there's nothing to port from on the Godot side to suppress in the first place. |
| `SaveChecksum` integrity hash | Could verify snapshot integrity but overkill for session-local ephemeral state |
| Save stores with checksummed envelopes (5 stores specifically upgraded per AGENTS.md's Save/Load section: `ExpeditionSaveStore`, `MedicalSaveStore`, `NarrativeSaveStore`, `WorldSaveStore`, `JournalSaveStore`) | Undo does NOT persist to save stores — intentionally session-only |

---

## Exit Criteria

This batch is COMPLETE when:
1. `IPlayerCommand` interface and `CommandCategory` enum exist in Core.
2. `CommandHistory` manages bounded undo/redo with snapshot capture/restore.
3. `FullStateSnapshotProvider` captures and restores all registered stateful systems, **and its measured per-action latency (see Step 3) is documented and within an agreed budget** — this criterion did not exist in the original draft and is the single most important addition given the HIGH risk rating.
4. 11+ concrete command implementations cover resource allocation, expedition, and assignment actions.
5. `CommandExecutor` wires history capture around every command execution.
6. Godot host has Ctrl+Z/Ctrl+Y keybinds and undo button.
7. Day advance clears undo history via subscription to `CampaignDayCoordinator.OnDayAdvanced` (the real event — not an invented `OnDayAdvance()` method; documented irreversibility boundary).
8. 10+ comprehensive undo/redo tests pass.
9. All pre-existing tests pass unchanged (current count: 2116 `[Fact]` + 4 `[Theory]` = 2120 — re-verify at execution time; **not** "1941+", a stale figure — see Review Notes).
10. Godot host builds cleanly (`dotnet build Ashfall.csproj` — 0 errors, 0 warnings).
11. Memory per snapshot measured and documented (target: under 500KB for full game state) **and** wall-clock capture/restore time measured and documented (target: proposed 1-2ms per action; if unmet, a mitigation from the Rollback Plan is chosen and documented before this batch is considered complete).


---

## Review Notes (Corrected)

This plan was adversarially reviewed against the actual codebase at
`/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War` prior to execution. The following
factual errors and gaps in the original draft were found and corrected in place above:

1. **"82+ systems" is not a verified count and appears to have the wrong provenance.** A direct
   grep for `public ... CaptureState(` method definitions in `Assets/Ashfall.Core/` returns **94**
   matches (93-94 matching `RestoreState` defs, with one asymmetry in `HoldfastTradeSession.cs`,
   which has `CaptureState()` but no corresponding `RestoreState()` — flag this specific class as a
   blocker/special-case for the snapshot provider, since a system that can be captured but not
   restored breaks the undo contract for anything touching Holdfast trade). The "82" figure does
   not match this count and, more importantly, does not appear to derive from a CaptureState/
   RestoreState count at all — AGENTS.md separately describes `GameBootstrap` as "a 1225-line god
   object across 82 partial files" (H7-adjacent), and that class **does not exist anywhere in the
   active tree** (the only hit in the entire repo is a single non-partial file under
   `_quarantine_legacy/`). It looks like "82" was carried over from that unrelated, now-defunct
   figure rather than from an actual count of stateful systems. This plan now uses the verified
   figure of ~94 throughout, with an explicit note to re-run the grep before executing since the
   number will drift as systems are added.

2. **`AGENTS.md`'s Unity-legacy architecture tables describe a repo state that does not exist in
   this checkout.** `Assets/_Game/` (referenced by AGENTS.md for `PersonalQuestSystem.cs`,
   `MedicalSystem.cs`, `SurvivorWorkShiftSystem.cs`, `DynamicEconomySystem.cs`) does not exist at
   all — `Assets/` contains only `Ashfall.Core/`, `StreamingAssets/`, `art/`, `audio/`, `sprites/`,
   `ui/`. Do not cite AGENTS.md's legacy-tree file paths or class names as present-tense fact
   without independently verifying against the current tree — this review found several (see
   Batch 75's notes for the parallel `DynamicEconomySystem`/`CombatSystem` errors) and this plan's
   "82+" figure is very likely another symptom of the same stale-source problem.

3. **No pre-existing command-pattern infrastructure exists — this part of the plan's premise is
   correct.** Repo-wide search for `ICommand`, `IPlayerCommand`, `CommandHistory`,
   `CommandPattern`, `IStatefulSystem`, `IStateSnapshotProvider` returned zero matches anywhere in
   `Assets/`, `src/`, or `Ashfall.Core.Tests/`. This is genuinely new work with no naming collision
   risk. The only adjacent existing seam is `IDayAdvancePersistence`
   (`Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs`), a narrow interface for flushing dirty
   save stores before the daily briefing — not a command/undo abstraction, and not something to
   build on top of directly, but worth knowing about since it establishes the existing
   "day-boundary hook" precedent this plan's Step 5 needs.

4. **`OnDayAdvance()` does not exist as a method anywhere in the codebase — the plan invented a
   name.** The actual, already-wired hook is `CampaignDayCoordinator.Advance(int day,
   IDayAdvancePersistence persistence)`, which raises a real C# event, `OnDayAdvanced`
   (`Action<DayAdvancedEventArgs>`, declared in `CampaignDayCoordinator.cs`), after every registered
   owner has advanced for the day. Steps 5 and 6 were corrected to subscribe `CommandHistory.Clear()`
   to this real event rather than instructing the host to call an invented
   `CommandHistory.OnDayAdvance()`/`Main.cs`-side `OnDayAdvance()` method that does not exist and
   was never going to be automatically invoked by anything.

5. **The single largest gap: the performance risk was analyzed for memory only, not for CPU/latency
   cost, and the memory estimate itself had no empirical basis.** The original Step 3 asserted
   "~50-200KB per snapshot x 5 depth = 250KB-1MB. Acceptable for a desktop game" with no supporting
   measurement — verified: no `.json` save fixtures are checked into this repo, and the existing
   size-adjacent tests (`SaveStoreChecksumSweepTests`, `SaveWireContractTests`) assert checksum/
   round-trip correctness, not byte size. More importantly, the original plan never separately
   analyzed the CPU/wall-clock cost of calling `CaptureState()` on ~94 systems **at UI-interaction
   frequency** — i.e., once per player click (ration change, craft-queue edit, work-shift
   reassignment), not once per day or once per explicit save. The existing `SaveAll()` precedent
   the plan draws on for its "similar pattern" comparison runs at a much lower frequency (once per
   day/explicit save, where a brief pause is tolerable) — there is no existing precedent in this
   codebase for full-system-state capture at per-click frequency, and given the HIGH risk rating
   already assigned to this batch, this should have been the headline technical risk, not an
   afterthought buried in a "Medium/Medium" risk-table row with a vague "profile in Step 3, if too
   slow" mitigation. Step 3 was rewritten to require an actual measured benchmark (capture + restore
   wall-clock time against a realistic mid-game session) before the rest of the batch proceeds, with
   a concrete latency budget (1-2ms) to evaluate against, and the Risks table's corresponding row was
   upgraded to High/High with a firmer mitigation path (category-scoped capture, debounced capture,
   or async capture) rather than deferring the entire question to "V2, not this batch."

6. **`IStatefulSystem` is not a "shared interface many systems already implement" — it requires
   ~94 new adapter implementations.** Each of the ~94 systems returns its own strongly-typed DTO
   from `CaptureState()` (e.g., `DutyRosterSystemState`, `CombatState`), not a common `object`-typed
   signature. Wrapping all of them behind a uniform `IStatefulSystem.CaptureState() : object`
   requires writing (or extending) ~94 adapters — this is real, non-trivial mechanical work that the
   original draft's "many systems already implement the pattern but lack a shared interface" phrasing
   understated. The effort estimate should carry explicit budget for this, and the Summary Table's
   risk rating for Step 3 was raised from Medium to High to reflect both this and the performance
   question above.

7. **Deep-copy discipline per system was not addressed.** Verified: `RestoreState` implementations
   across the codebase (e.g. `BrineWaterSystem.cs`, `CensusClaimSystem.cs`, `DutyRosterSystem.cs`)
   carry explicit comments enforcing that the deserialized DTO must not become the live state
   (i.e., must be deep-copied, not aliased). A generic snapshot/restore wrapper built for undo must
   preserve this discipline per system — it cannot assume a single reflection-based or shallow-clone
   strategy works uniformly across all ~94 systems. This constraint is now called out explicitly in
   Step 3's Done-when criteria and the Risks table.

8. **Stale test count: "1941+ existing tests" is outdated, same issue as Batch 75.** Verified count:
   2116 `[Fact]` + 4 `[Theory]` = **2120**. "1941" traces to a historical entry in
   `10LOOP_AUDIT_REPORT.md` describing an earlier point in the suite's growth, not its current size.
   Corrected in Step 7's Done-when and the Exit Criteria.

9. **Missing/thin rollback plan.** The original plan had no dedicated rollback section (the Risks
   table's "mitigations" column is not the same as an explicit rollback procedure). Added one,
   covering: the feature's additive/opt-in wiring (routing through `CommandExecutor` is easy to
   revert since it wraps rather than replaces existing system calls), the fact that undo history is
   never persisted (so reverting cannot corrupt save data), and an explicit scope-reduction fallback
   (single-action, single-category undo) if the Step 3 performance benchmark or the Step 3 adapter
   work turns out to be substantially larger than estimated — with an explicit instruction that this
   fallback should be raised to the user rather than decided unilaterally mid-implementation.

10. **`HoldfastTradeSession.CaptureState()` has no matching `RestoreState()`.** This is a real,
    specific blocker for the "capture all ~94 systems" premise this batch depends on — flagged as a
    special case in Note 1 above and should be resolved (either by adding `RestoreState` to that
    class, or by explicitly excluding Holdfast trade state from the snapshot scope and documenting
    that trades made during an undo window cannot be undone) before Step 3 is considered complete.

11. **`AdministerTreatmentCommand` (Step 5) implied a monolithic `MedicalSystem` that doesn't exist.**
    The medical domain in this codebase is split across `ChemicalDependencySystem`,
    `VigilStateMachine`, and `MedicalWardSystem` (all in `Assets/Ashfall.Core/Medical/`), wired by
    the host-side `MedicalHostSession`. Corrected the plan to name the actual classes rather than a
    generic "medical action" against an assumed single system.

Everything else in the plan — the `IPlayerCommand`/`CommandCategory`/`ICommandInterceptor` design,
the bounded undo/redo stack shape, the resource-allocation and expedition command catalogue, the UI
integration approach, and the design principles (undo-for-misclicks-not-time-travel,
state-restoration-over-inverse-operations, bounded/session-local history, no save persistence) were
verified as sound and left unchanged. The core architectural bet — that undo can be built on top of
the existing CaptureState/RestoreState contract rather than requiring per-command inverse-operation
logic — is reasonable and matches how the codebase already treats save/load. The primary corrections
in this review are about (a) getting the scale and hook-point facts right, and (b) elevating the
plan's treatment of per-action snapshot performance to match its own HIGH risk rating, which the
original draft's thin, unverified cost estimate did not do.
