# ASHFALL — Quality Roadmap Batch 85

## Theme: State Machine Formalization — Explicit FSM for Game Lifecycle & System Phases

**Priority:** HIGH
**Risk:** Medium — formalizes existing implicit behavior; regressions possible if transition guards miss edge cases
**Estimated Effort:** 3–4 focused sessions
**Depends On:** Nothing (foundational infrastructure)
**Unlocks:** Cleaner expedition/combat logic, debuggable state transitions, deterministic replay of lifecycle events

---

## Motivation

**CORRECTED (see Review Notes below): this section overstated how ad-hoc the current code is.** Two of the six systems this batch names already have real enums with save-versioned migration, not boolean-flag soup. The table below reflects verified reality, not the original claim.

| Domain | States | Current Encoding (verified) | Problem |
|--------|--------|------------------|---------|
| Game lifecycle | `Menu, Playing, GameOver` (3 states) | Private nested `enum GameState` in `src/Main.cs:271` | No transition guards; GameOver can re-enter Playing. Simpler than originally described — no 9-state lifecycle exists to migrate. |
| Day cycle | DayStart → PlayerActions → DayEnd → NightPhase | Implicit in tick flow | Order violations cause double-tick bugs — **not independently verified this session; re-check the actual tick method name before Step scoping (no `TickSimDay` symbol was found)** |
| Expedition | `Outbound, Looting, Inbound, Completed, Failed` (5 states) | **Real enum `ExpeditionPhase` already exists** in `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs`, with a `SetPhase()` helper that already no-ops on `exp.phase == (int)phase`, and the tick loop already skips `Completed`/`Failed` expeditions (`ExpeditionSystem.cs:188`) | Existing guard already prevents naive double-completion. Real risk is different: `ExpeditionPhase` name collides with this plan's proposed new enum of the same name but different values (`Idle, Preparing, Departing, Active, Returning, Debriefing, Completed, Failed`) |
| Medical vigil | `VigilStateMachine` — **not a state machine**: it is a single countdown timer (`IsActive`/`IsCompleted` bools + `ElapsedSeconds`), no enumerated states, no transition table | Dedicated class, `Assets/Ashfall.Core/Medical/VigilStateMachine.cs` | Named misleadingly; there is no state enum to migrate here, only a linear timer. Do not count this as one of the "state machines" being formalized — it doesn't fit the `StateMachine<TState>` shape at all |
| Combat | `Setup, PlayerTurn, EnemyTurn, Resolved, Won, Lost, Retreated` (7 states) | **Real enum `CombatPhase` already exists** in `Assets/Ashfall.Core/Combat/CombatTypes.cs:9`, stored as `int Phase` on `CombatState`, with save-version migration (`TacticalCombatSystem.Migrate`) that clamps out-of-range phase values | Genuine problem: turn-order enforcement is manual (`_state.Phase = (int)CombatPhase.EnemyTurn` assigned directly in `TacticalCombatSystem`, no guard against calling `PlayerFire` while `Phase == EnemyTurn` was found) — but the enum itself is not the gap, the *enforcement* is |
| Radiation phases | `Healthy, Prodromal, Latent, ManifestIllness, ChronicFibrosis, RecoveryOrDeath` (6 states) — **not** "Mild/Moderate/Severe/Critical" | `RadiationPhaseProgression` (`Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs`) already has a private `TransitionTo()` helper with a no-op guard (`if (sv.Phase == newPhase) return;`) and fires `OnPhaseChanged` events on every transition | This is already close to the target shape (guarded transition + event). Skip-levels-on-large-tick-delta risk is plausible for `Latent`/`ManifestIllness` since `OnsetTimer` is decremented per tick without a "did we skip a boundary" check — that part of the claim holds, but the "4 threshold-check states" framing does not |
| Survivor lifecycle | Healthy → Sick → Critical → Dead | NeedsSystem threshold comparisons | Not independently verified this session (no `NeedsSystem` read) — treat as unverified until confirmed |
| YearOfAsh timeline | 3 phases: `Phase4_DeepFreeze` (day ≤240), `Phase5_FactionSiege` (241–300), `Phase6_TheGreatThaw` (301–360) | `YearOfAshTimelineSystem.AdvanceDay()` computes phase from a day comparison every call; `RestoreState()` already clamps `currentDay` to `[180, 360]` via `Math.Max/Min` | The claimed "phase regression on save/load with clock skew" is partially mitigated already — day clamping exists. A real residual gap: `RestoreState` restores `phase` directly from the DTO without recomputing it from the clamped day, so a corrupted/stale `phase` field disagreeing with `currentDay` would load as-is (not regression exactly, but a phase/day desync bug) |

**Core issues (revised):**
- Two of six named systems (Combat, Expedition) already have enums and partial guards — the work here is narrower than "add state machines from scratch": it's retrofitting enforcement onto existing enums, or reconciling name collisions
- Radiation phase progression already has transition guards and events; the gap is tick-delta skip-over, not missing structure
- Game lifecycle genuinely lacks any transition table (verified — 3-state enum, no guards)
- `CaptureState/RestoreState` must reconstruct implicit state from derived data in some systems (YearOfAsh's phase/day desync case) — real but narrower than originally claimed
- Adding new states to any system requires touching scattered if/else blocks — true for game lifecycle and combat turn enforcement; not true for Expedition or Radiation phase transitions, which already centralize through one helper method

A shared `StateMachine<TState>` in Core gives: compile-time state enumeration, explicit transition tables, guard predicates, entry/exit actions, transition history for debugging, and serializable current-state for save/load. **Given the above, this batch's real value is (a) formalizing the game-lifecycle enum, which has no structure today, and (b) adding enforcement/history on top of the Combat and Radiation phase enums that already exist — not building six state machines from a blank slate.**

---

## Step 1 — Design Generic StateMachine<TState> in Core

**Goal:** Define the public API surface for a reusable, engine-agnostic finite state machine that supports: enumerated states, explicit transition declarations, guard predicates, entry/exit actions, and a transition history ring buffer.

**Implementation:**

```
Assets/Ashfall.Core/StateMachine/
├── StateMachine.cs          # Generic FSM implementation
├── StateMachineConfig.cs    # Builder for transition table
├── TransitionRecord.cs      # Immutable record of a transition (from, to, timestamp, context)
├── IStateMachineHost.cs     # Optional host callback interface
└── StateMachineState.cs     # Serializable DTO for CaptureState/RestoreState
```

API sketch:

```csharp
namespace Ashfall.Core.StateMachine;

public sealed class StateMachine<TState> where TState : struct, Enum
{
    public TState CurrentState { get; }
    public IReadOnlyList<TransitionRecord<TState>> History { get; }

    public StateMachine(TState initialState, StateMachineConfig<TState> config, ILog log);

    public bool CanTransition(TState target);
    public bool TryTransition(TState target, string context = null);
    public void ForceTransition(TState target, string context = null); // For restore only

    public StateMachineState<TState> CaptureState();
    public void RestoreState(StateMachineState<TState> state);
}
```

Design constraints:
- `TState` must be `struct, Enum` — compile-time safety, no allocations for state values
- Transition table is immutable after construction (built via `StateMachineConfig<TState>`)
- Guard predicates are `Func<bool>` registered per transition pair
- Entry/exit actions are `Action` registered per state
- History ring buffer has configurable capacity (default 32, capped at 256)
- `ILog` receives transition events at Info level, guard failures at Warn level
- No `UnityEngine.*`, no `Godot.*`, no allocations on tick-path (transitions are infrequent)
- `ForceTransition` bypasses guards — used only during `RestoreState` to reconstruct without re-triggering entry actions

**Verification:**
- Code compiles in `Ashfall.Core` with `dotnet build`
- No engine references in the file (grep for `UnityEngine`, `Godot`)
- API review: does it cover all 8 identified state machines?

**Done when:** `StateMachine.cs`, `StateMachineConfig.cs`, `TransitionRecord.cs`, `StateMachineState.cs` exist in Core, compile cleanly, and have XML doc comments on all public members.

---

## Step 2 — Implement StateMachine<TState> Core Logic

**Goal:** Full implementation of the FSM: transition resolution, guard evaluation, entry/exit action invocation, history recording, and error handling.

**Implementation:**

`StateMachineConfig<TState>` (builder pattern):
```csharp
public sealed class StateMachineConfig<TState> where TState : struct, Enum
{
    public StateMachineConfig<TState> AddTransition(TState from, TState to, Func<bool> guard = null);
    public StateMachineConfig<TState> OnEnter(TState state, Action action);
    public StateMachineConfig<TState> OnExit(TState state, Action action);
    public StateMachineConfig<TState> WithHistoryCapacity(int capacity);
    public StateMachineConfig<TState> Build(); // Freezes config, validates no orphan states
}
```

`TryTransition` algorithm:
1. Check transition `(CurrentState, target)` exists in table → if not, log warning, return false
2. Evaluate guard predicate (if any) → if fails, log warning with context, return false
3. Invoke `OnExit(CurrentState)` action (if registered)
4. Update `CurrentState` to `target`
5. Append `TransitionRecord` to history ring buffer (from, to, tick/day counter, context string)
6. Invoke `OnEnter(target)` action (if registered)
7. Return true

`TransitionRecord<TState>`:
```csharp
public readonly struct TransitionRecord<TState> where TState : struct, Enum
{
    public TState From { get; }
    public TState To { get; }
    public int Tick { get; }
    public string Context { get; }  // nullable, for debugging
}
```

Edge cases:
- Self-transitions (`from == to`): allowed only if explicitly added to the table
- Guard exceptions: catch, log error, return false (never crash the game loop)
- Entry/exit action exceptions: catch, log error, continue (transition committed)
- History overflow: oldest records dropped (ring buffer semantics)

**Verification:**
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` compiles
- Unit tests (see Step 7) exercise all paths
- No allocations in steady-state (only on actual transitions — acceptable)

**Done when:** All public methods implemented, internal state is consistent after every operation, exception paths log and recover gracefully.

---

## Step 3 — Add CaptureState/RestoreState to StateMachine

**Goal:** StateMachine instances are fully serializable via the project's standard save pattern, enabling save/load without state reconstruction heuristics.

**Implementation:**

`StateMachineState<TState>` (serializable DTO):
```csharp
[Serializable]
public sealed class StateMachineState<TState> where TState : struct, Enum
{
    public string CurrentState { get; set; }  // Enum name as string for JSON stability
    public List<TransitionRecordDto> History { get; set; }
}

[Serializable]
public sealed class TransitionRecordDto
{
    public string From { get; set; }
    public string To { get; set; }
    public int Tick { get; set; }
    public string Context { get; set; }
}
```

Serialization decisions:
- States stored as `Enum.GetName()` strings, not integers — resilient to enum reordering
- `RestoreState` uses `ForceTransition` (bypasses guards, skips entry/exit actions)
- History is restored from DTO list; capacity limit still applies (truncate oldest if needed)
- If `CurrentState` string doesn't parse to a valid `TState` value, log error and remain in initial state (corrupt save recovery)
- Compatible with `SaveChecksum` — `StateMachineState<T>` is a plain DTO with string/int/list fields

Integration with host save stores:
- Systems that embed a `StateMachine<TState>` include its `StateMachineState` as a field in their own state DTO
- Example: `ExpeditionSystem.ExpeditionState` gains a `StateMachineState<ExpeditionPhase> PhaseState` field
- Existing saves that lack this field → system reconstructs from legacy status enum on first load (migration path)

**Verification:**
- Round-trip test: create FSM, transition through N states, `CaptureState`, create new FSM, `RestoreState`, assert `CurrentState` and `History` match
- Corrupt state test: provide invalid state string, assert graceful fallback
- `SaveChecksum` compatibility: serialize `StateMachineState`, compute checksum, assert deterministic

**Done when:** `CaptureState()` and `RestoreState()` produce identical FSM state across serialization boundaries; legacy migration path documented in code comments.

---

## Step 4 — Migrate Game Lifecycle to StateMachine<GamePhase>

**Goal:** Replace the ad-hoc game lifecycle enum + if/else chains in `Main.cs` with a formal `StateMachine<GamePhase>`, making invalid transitions impossible and providing transition logging.

**CORRECTED SCOPE:** The real enum being replaced is `private enum GameState { Menu, Playing, GameOver }` at `src/Main.cs:271` — a 3-state private nested enum, not a 9-state lifecycle. It is used as `_state = GameState.GameOver` (see `ShowGameOver` at `src/Main.cs:6109`) and referenced at `src/Main.cs:5423` in a comment (`// Game flow: Menu → Playing → GameOver`). The 9-state `GamePhase` proposed below (`Initializing, MainMenu, NewGameSetup, Loading, Playing, Paused, DayTransition, GameOver, Exiting`) is **new scope invented by this plan, not a formalization of existing states**. Decide explicitly before starting:
- Option A (smaller, safer): formalize the real 3-state `GameState` as `StateMachine<GameState>` with guards. Matches "formalize existing implicit behavior" from the risk statement above.
- Option B (this plan's original proposal): introduce 6 new states that don't exist in the current game flow at all (`NewGameSetup`, `Loading`, `Paused`, `DayTransition`, `Exiting` have no corresponding code paths verified this session). This is scope creep beyond "formalize the existing state machine" — it's designing new lifecycle behavior. If chosen, treat it as a separate, explicitly-scoped feature addition, not a refactor, and get user sign-off first since it changes observable app behavior (e.g., a real Paused state pausing the tick loop is new behavior, not a refactor).

The steps below assume Option A unless the user has explicitly asked for Option B.

Define `GamePhase` enum in Core (or rename to match the real `GameState` name if Option A is chosen, to avoid a second unrelated enum called "phase" sitting next to `CombatPhase` and `ExpeditionPhase`):
```csharp
namespace Ashfall.Core;

public enum GamePhase
{
    Initializing,   // Boot, loading data catalogs
    MainMenu,       // Title screen, settings
    NewGameSetup,   // Difficulty, seed, scenario selection
    Loading,        // Deserializing save file
    Playing,        // Active simulation
    Paused,         // Simulation frozen, UI overlay
    DayTransition,  // End-of-day processing (save, events, needs tick)
    GameOver,       // Death/ending screen
    Exiting         // Cleanup, final save
}
```

Transition table (explicit allowed transitions):
```
Initializing → MainMenu
MainMenu → NewGameSetup | Loading | Exiting
NewGameSetup → Playing
Loading → Playing
Playing → Paused | DayTransition | GameOver | Exiting
Paused → Playing | Exiting
DayTransition → Playing | GameOver
GameOver → MainMenu | Exiting
```

Guards:
- `Playing → DayTransition`: guard checks all player actions resolved
- `Loading → Playing`: guard checks save loaded successfully (non-null state)
- `GameOver → MainMenu`: guard checks ending sequence completed

In `Main.cs` / Godot host:
- Replace `_currentPhase` enum field with `StateMachine<GamePhase> _lifecycle`
- Replace all `if (_currentPhase == GamePhase.Playing)` checks with `_lifecycle.CurrentState == GamePhase.Playing`
- Replace phase assignments with `_lifecycle.TryTransition(GamePhase.X)`
- Wire `OnEnter(Playing)` → start tick loop; `OnExit(Playing)` → stop tick loop
- Wire `OnEnter(GameOver)` → trigger ending UI; `OnExit(GameOver)` → cleanup

**Verification:**
- `dotnet build Ashfall.csproj` — Godot host compiles, 0 errors, 0 warnings (per project verification checklist — the original draft didn't specify the warning bar)
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — all ~2120 existing tests still pass (baseline count as of this review; re-run `grep -rc "\[Fact\]\|\[Theory\]" Ashfall.Core.Tests/` before and after to confirm no accidental deletions)
- Manual smoke: `godot --headless --path . -- --bridge-selftest` exits 0 (this now only prints a removal notice and exits 0 per the Bridge Shim removal — it does **not** exercise gameplay or the lifecycle enum, so it cannot verify this step's actual behavior. Treat this as a basic "host still boots" smoke check, not a functional verification of the state machine)
- No behavior change from user perspective if Option A chosen (pure refactor); if Option B chosen, behavior change is expected and must be described to the user, not hidden under "pure refactor" language
- Transition log visible in debug output — **name the exact log sink and how to inspect it** (e.g. `ILog.Info` output captured where, stdout vs a debug panel) before calling this done; "visible in debug output" is not independently verifiable without that detail

**Risk / Rollback:** This step changes the app's central lifecycle field, referenced from at least `ShowGameOver` (`src/Main.cs:6109`) and possibly the input/tick loop (not fully mapped this session — grep `_state` usages in `src/Main.cs` before starting, there may be more call sites than the two found). Rollback plan: keep the old `_state` field renamed (e.g. `_legacyState`) and dual-write both fields for one commit before deleting the old field, so a revert is a one-line diff rather than reconstructing removed code from git history.

**Done when:** `Main.cs` no longer directly assigns game phase; all phase changes go through `_lifecycle.TryTransition`; invalid transitions are logged and rejected; all `_state` read/write call sites in `src/Main.cs` have been enumerated (not just the 2 found in this review) and migrated.

---

## Step 5 — Migrate Expedition Lifecycle to StateMachine<ExpeditionPhase>

**Goal:** Replace the expedition status enum + scattered if/else in `ExpeditionSystem` with a formal FSM, eliminating the "complete an already-completed expedition" bug class.

**BLOCKING ISSUE — verify before starting:** `Ashfall.Core.Expeditions.ExpeditionPhase` **already exists** at `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` with values `Outbound, Looting, Inbound, Completed, Failed`. It is not an unstructured status enum — `ExpeditionSystem` already has a `SetPhase(exp, phase)` helper with a no-op guard (`if (exp.phase == (int)phase) return;`), and the tick loop already skips ticking expeditions whose phase is `Completed` or `Failed` (`ExpeditionSystem.cs:186-189`). **The "complete an already-completed expedition" bug this step claims to fix does not appear to exist in the current tick loop** — the tick already `continue`s past terminal phases before any completion logic runs.

The enum values and transition shape below (`Idle, Preparing, Departing, Active, Returning, Debriefing, Completed, Failed`) do not match the real enum at all — this section was written without checking the existing type. Do not add a second, differently-shaped `ExpeditionPhase` enum; it will not compile (duplicate type in the same namespace) or, if namespaced differently, will silently create two incompatible concepts called "expedition phase" in the same codebase.

**Before writing any code:** re-scope this step as one of:
- (a) Add guard/history/logging on top of the *existing* `ExpeditionPhase` (5 states), via `StateMachine<ExpeditionPhase>` wrapping the current enum — no new states, no renaming.
- (b) If the richer lifecycle (`Preparing`/`Departing`/`Debriefing` as separate states) is genuinely wanted, that's a design change to `ExpeditionSystem`'s phase model, not a mechanical FSM-formalization refactor. Requires updating `ExpeditionState.phase` (an `int` field used directly in save DTOs — see `ExpeditionState.phase = (int)ExpeditionPhase.Outbound` default), which is a save-format change requiring a migration path, not just a code refactor.

The steps below assume (a) unless the user explicitly wants the richer lifecycle from (b).

**Implementation (revised for option a):**

Reuse the existing `ExpeditionPhase` enum — do not redefine it:
```csharp
// Already defined in Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs — reference, don't recreate
public enum ExpeditionPhase
{
    Outbound,  // traveling to the target
    Looting,   // at the site, push-your-luck scavenging
    Inbound,   // returning to shelter
    Completed, // returned with loot unloaded
    Failed     // collapsed or killed
}
```

Transition table (matches the real 5-state enum; the `Idle/Preparing/Departing/Returning/Debriefing` states from the original draft do not exist and are dropped):
```
Outbound → Looting | Failed
Looting → Inbound | Failed
Inbound → Completed | Failed
Completed → (terminal, no outbound transitions)
Failed → (terminal, no outbound transitions)
```

Guards (only add what's not already covered by `SetPhase`'s existing no-op-on-same-phase check):
- `* → Failed`: guard checks TPK condition or abort flag
- `Inbound → Completed`: guard checks expedition has returned to shelter (arrival tick condition)

Per-expedition FSM instance:
- Each `ExpeditionState` gains a `StateMachineState<ExpeditionPhase> PhaseState` field alongside its existing `int phase` field
- `ExpeditionSystem` continues to manage a `Dictionary<string, ExpeditionState>` as it does today — no structural change to the collection type
- Ticking already only advances expeditions in non-terminal phases (verified: `ExpeditionSystem.cs:186-189`) — this behavior is preserved, not introduced
- Save: **the existing `int phase` field on `ExpeditionState` must stay as the primary save field for backward compatibility.** Adding `StateMachineState` as a second, redundant field risks the two disagreeing after a partial migration. Decide explicitly: either (i) `int phase` remains authoritative and the FSM is rebuilt from it on load (no new save field), or (ii) `StateMachineState` becomes authoritative and `int phase` becomes a derived mirror updated on every transition. Pick (i) for this batch — lower risk, no save-format change.

Migration from legacy:
- There is no separate legacy `ExpeditionStatus` enum to map from — `ExpeditionPhase` **is** the current and only status representation. Skip this sub-step; it was based on a mistaken premise (see BLOCKING ISSUE above).

**Verification:**
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — all existing expedition tests in `Ashfall.Core.Tests/ExpeditionSystemTests.cs` pass unchanged (behavior unchanged)
- New tests: cannot transition `Completed → Outbound`, cannot transition `Outbound → Completed` directly (must pass through `Looting` and `Inbound`)
- `godot --headless --path . -- --data-integrity-selftest` passes
- Regression check specifically for the claimed bug: write a test that calls the completion path twice on the same expedition and confirms it was already a no-op before this change (documents that this step is enforcement/history, not a bug fix)

**Risk / Rollback:** Because `ExpeditionState.phase` (int) remains authoritative per the decision above, this step is low-risk and additive — the `StateMachine<ExpeditionPhase>` instance can be removed without touching save compatibility if it doesn't pan out.

**Done when:** `ExpeditionSystem` wraps the existing `ExpeditionPhase` enum in `StateMachine<ExpeditionPhase>` per expedition, using the real 5 states; `int phase` stays the save-authoritative field; transition history is available for debugging; no second `ExpeditionPhase`-shaped type exists anywhere in the codebase.

---

## Step 6 — Migrate Combat to StateMachine<CombatPhase>

**Goal:** Add enforced turn-order guards and transition history on top of `TacticalCombatSystem`'s existing `CombatPhase` enum, making turn order violations impossible and enabling combat replay via transition history.

**BLOCKING ISSUE — verify before starting:** `Ashfall.Core.Combat.CombatPhase` **already exists** at `Assets/Ashfall.Core/Combat/CombatTypes.cs:9` with values `Setup, PlayerTurn, EnemyTurn, Resolved, Won, Lost, Retreated` (7 states — not the 10-state enum drafted below). It is stored as `int Phase` on `CombatState`, and `TacticalCombatSystem.Migrate()` already clamps out-of-range phase values on save migration (verified via `CombatSaveRoundTripTests.cs` and `CombatSystemTests.cs`, both asserting `migrated.Phase` is clamped to `[Setup, Retreated]`). The claim that combat uses "boolean flags" (`_isPlayerTurn`, `_isResolving`, `_combatActive`) is **not supported** — no such fields were found; `TacticalCombatSystem` assigns `_state.Phase` directly (e.g. `_state.Phase = (int)CombatPhase.EnemyTurn` and back to `PlayerTurn` after enemy turn resolution).

The real gap is narrower and different: **there is no guard preventing `PlayerFire`/`PlayerSuppress`/etc. from executing while `_state.Phase == EnemyTurn`.** Those methods check `_state.Resolved` but not `_state.Phase` against the acting side. That is the actual "invalid turn transition" risk — not a missing enum.

Do not redefine `CombatPhase` with new values (`NotInCombat, PlayerResolve, EnemyResolve, RoundEnd, Victory, Defeat, Fled`); it will collide with the existing 7-value enum used throughout `TacticalCombatSystem`, `CombatHeadlessDemo.cs`, `CombatHostSession.cs`, and three existing test files. Any rename or restructuring of `CombatPhase` is a breaking change to `CombatState`'s save format (the `int Phase` field's meaning shifts) and must go through the project's versioned save-migration path (`SaveVersion`/`Migrate()`), not a silent redefinition.

**Implementation (revised):**

Reuse the existing `CombatPhase` enum — do not redefine it:
```csharp
// Already defined in Assets/Ashfall.Core/Combat/CombatTypes.cs — reference, don't recreate
public enum CombatPhase
{
    Setup = 0,
    PlayerTurn = 1,
    EnemyTurn = 2,
    Resolved = 3,
    Won = 4,
    Lost = 5,
    Retreated = 6
}
```
    NotInCombat,    // No active encounter
    Setup,          // Positioning, initiative roll
    PlayerTurn,     // Player selecting actions
    PlayerResolve,  // Executing player actions (animations/effects)
    EnemyTurn,      // AI selecting actions
    EnemyResolve,   // Executing enemy actions
    RoundEnd,       // Apply end-of-round effects (bleed, poison, morale)
    Victory,        // All enemies defeated/fled
    Defeat,         // All survivors down
    Fled            // Player chose to flee
}
```

Transition table (built from the real 7-state enum; note `Setup` is the initial state per `CombatState.Phase = (int)CombatPhase.Setup` default in `CombatTypes.cs:169`):
```
Setup → PlayerTurn (BeginEncounter completes)
PlayerTurn → EnemyTurn (player ends turn, verified: TacticalCombatSystem.cs ~893)
PlayerTurn → Won | Lost | Retreated (resolution reached mid-turn, e.g. all enemies dead from PlayerFire)
EnemyTurn → PlayerTurn (enemy turn resolves, verified: TacticalCombatSystem.cs ~940)
EnemyTurn → Won | Lost | Retreated
Won → (terminal)
Lost → (terminal)
Retreated → (terminal)
```
Note: there is no separate `Resolved` step in the verified transition flow distinct from `Won`/`Lost`/`Retreated` — `Resolved` exists in the enum (value 3) but no assignment to it was found in `TacticalCombatSystem.cs` in this session's read (first 700 of 1352 lines). **Read the remainder of the file before finalizing the transition table** — this table may be incomplete.

Guards (the actual gap — see BLOCKING ISSUE above):
- `PlayerFire` / `PlayerSuppress` / `PlayerClearJam` / `PlayerReload` / `PlayerFieldRepair` / `PlayerMoveLane` / `PlayerDeployTrap`: **add a guard that these no-op with a clear message when `_state.Phase != PlayerTurn`** — this is the concrete, verified gap, not a missing enum
- `* → Won/Lost/Retreated`: guard checks win/loss/retreat condition met (partially exists already — `CheckResolution()` is called after actions, per code read)

In `TacticalCombatSystem`:
- **There are no `_isPlayerTurn`, `_isResolving`, `_combatActive` booleans to replace** — this claim was not verified and appears false based on the code read. `TacticalCombatSystem` already uses `_state.Phase` (the real `CombatPhase` int) as its only turn-tracking field.
- Real change: wrap `_state.Phase` reads/writes in `StateMachine<CombatPhase>.TryTransition`, and add the missing guard on player action methods (above) that don't currently check phase before executing
- `PlayerFire()` and sibling methods: add `if (_state.Phase != (int)CombatPhase.PlayerTurn) { res.Message = "Not your turn."; return res; }` at the top (or equivalent via the FSM guard)
- Transition history enables combat log reconstruction — note `CombatState.Events` (a `List<CombatEvent>`) **already exists and already serves this purpose** (see `AddEvent()` calls throughout `TacticalCombatSystem`); confirm before duplicating this as a second, separate history mechanism whether `TransitionRecord` history should feed into or replace `CombatEvent` logging

**Verification:**
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — all existing combat tests (`TacticalCombatSystemTests.cs`, `CombatSystemTests.cs`, `CombatSaveRoundTripTests.cs`) pass unchanged
- New tests: calling `PlayerFire` while `Phase == EnemyTurn` returns a failure result with `Success == false` (this is the actual bug being fixed — write the test to prove it fails today before the guard is added, then passes after)
- Combat round sequence is strictly enforced (no skipping phases)
- Run `godot --headless --path . -- --expedition-encounter-bridge-selftest` (per AGENTS.md, this smoke-tests the live `ExpeditionEncounterBridge` domain class and touches combat handoff — relevant here since `CombatHeadlessDemo.cs` already exercises phase transitions)

**Risk / Rollback:** Adding a phase guard to 7 player-action methods changes observable behavior (actions that previously silently executed during the wrong phase — if that was ever reachable — will now be rejected). Before shipping, confirm via the existing test suite whether any current test path relies on calling a player action during `EnemyTurn` and getting a result (as opposed to it being unreachable via normal play). If none do, the guard is safe. If one does, that test encodes the "bug" and must be updated deliberately, not treated as a silent regression.

**Done when:** `TacticalCombatSystem` wraps the existing `CombatPhase` enum in `StateMachine<CombatPhase>`; the verified missing guard is added to all player action methods; combat replay derivable from transition history (reconciled with or superseding `CombatState.Events`, not duplicating it); all three existing combat test files still pass.

---

## Step 7 — Write Comprehensive FSM Tests

**Goal:** Full test coverage for the generic `StateMachine<TState>` and all three concrete migrations, verifying: valid transitions, guard enforcement, history tracking, save/load round-trips, and error recovery.

**Implementation:**

Test file: `Ashfall.Core.Tests/StateMachineTests.cs`

Test categories:

**Generic FSM tests:**
| Test | Asserts |
|------|---------|
| `ValidTransition_Succeeds` | `TryTransition` returns true, `CurrentState` updated |
| `InvalidTransition_ReturnsFalse` | Undeclared transition rejected, state unchanged |
| `GuardFails_TransitionRejected` | Guard returns false → state unchanged, logged |
| `EntryAction_CalledOnTransition` | OnEnter fires for target state |
| `ExitAction_CalledOnTransition` | OnExit fires for source state |
| `History_RecordsTransitions` | Each successful transition appended to history |
| `History_RingBufferOverflow` | Oldest dropped when capacity exceeded |
| `SelfTransition_AllowedIfDeclared` | Same-state transition works when in table |
| `SelfTransition_RejectedIfNotDeclared` | Same-state transition fails when not in table |
| `ForceTransition_BypassesGuard` | ForceTransition ignores guard, skips entry/exit |
| `CaptureRestore_RoundTrip` | Full state equality after serialize/deserialize |
| `Restore_InvalidState_Recovers` | Corrupt state string → stays in initial state |
| `ConcurrentTransition_Safe` | No corruption if TryTransition called re-entrantly from OnEnter |

**GamePhase FSM tests:**
| Test | Asserts |
|------|---------|
| `CannotSkipFromMenuToPlaying` | MainMenu → Playing is rejected (must go through Loading or NewGameSetup) |
| `GameOverCanReturnToMenu` | GameOver → MainMenu succeeds |
| `ExitingIsReachableFromAnyActiveState` | Playing/Paused/GameOver → Exiting all succeed |

**ExpeditionPhase FSM tests:**
| Test | Asserts |
|------|---------|
| `CompletedIsTerminal` | No transitions out of Completed |
| `CannotSkipPreparing` | Idle → Active rejected |
| `CancelFromPreparing` | Preparing → Idle succeeds |
| `SaveLoadPreservesExpeditionPhase` | Round-trip through CaptureState/RestoreState |

**ExpeditionPhase FSM tests (revised to real 5-state enum: Outbound, Looting, Inbound, Completed, Failed):**
| Test | Asserts |
|------|---------|
| `CompletedIsTerminal` | No transitions out of Completed |
| `FailedIsTerminal` | No transitions out of Failed |
| `CannotSkipLooting` | Outbound → Inbound rejected (must pass through Looting) |
| `SaveLoadPreservesExpeditionPhase` | Round-trip through CaptureState/RestoreState; `int phase` field remains save-authoritative per Step 5 |

**CombatPhase FSM tests (revised to real 7-state enum: Setup, PlayerTurn, EnemyTurn, Resolved, Won, Lost, Retreated):**
| Test | Asserts |
|------|---------|
| `TurnOrderEnforced` | Calling a player action method while `Phase == EnemyTurn` is rejected (`Success == false`) — this is the actual verified gap, see Step 6 |
| `WonIsTerminal` | Won → no outbound transitions |
| `LostIsTerminal` | Lost → no outbound transitions |
| `RetreatedIsTerminal` | Retreated → no outbound transitions |
| `HistoryReconstructsCombatLog` | 5-round combat produces N+ history entries — reconcile against existing `CombatState.Events`; do not double-count if the FSM history and `CombatEvent` log end up being the same data represented twice |

**Verification:**
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — all new tests pass; baseline before this batch is ~2120 tests (verified via `grep -rc "\[Fact\]\|\[Theory\]" Ashfall.Core.Tests/` — confirm this count yourself before the batch, it will drift as other batches land)
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — 0 errors, 0 warnings
- Test count increases by 25+ (generic + domain-specific) — treat as a floor, not a target; don't pad with low-value tests to hit the number

**Done when:** All tests listed above pass; no existing tests broken; test names follow project conventions (`MethodName_Scenario_ExpectedResult`).

---

## Summary

| Step | Deliverable | Files | Risk | Depends On |
|------|-------------|-------|------|------------|
| 1 | FSM API design | `Assets/Ashfall.Core/StateMachine/` (4 files) | Low | — |
| 2 | FSM implementation | Same files, full logic | Low | Step 1 |
| 3 | Save/load integration | `StateMachineState.cs`, DTO additions | Low | Step 2 |
| 4 | Game lifecycle formalization (real 3-state `GameState`, Option A) | `src/Main.cs`, new enum in Core | Medium | Steps 1–3 |
| 5 | Expedition phase enforcement (wraps existing 5-state `ExpeditionPhase`) | `ExpeditionSystem.cs` | Low–Medium (no save format change, per Step 5's decision) | Steps 1–3 |
| 6 | Combat turn-guard enforcement (wraps existing 7-state `CombatPhase`) | `TacticalCombatSystem.cs` | Medium (changes behavior on a previously-unguarded path — see Step 6 risk note) | Steps 1–3 |
| 7 | Comprehensive test suite | `StateMachineTests.cs` | Low | Steps 1–6 |

**Total new files:** ~6 in Core + 1 test file (down from original estimate — Steps 5 and 6 no longer add new enum files since they reuse existing ones)
**Total modified files:** ~6 (`src/Main.cs`, `ExpeditionSystem.cs`, `TacticalCombatSystem.cs`, save stores as needed — no new expedition/combat DTOs required under the revised, lower-risk scope)
**Breaking changes:** None if Steps 5/6 keep the existing enums; Step 4 is a breaking *behavior* change only if Option B (9-state lifecycle) is chosen instead of Option A
**Rollback plan:** FSM is additive for Steps 1–3. Step 4: dual-write old and new lifecycle fields for one commit before removing the old field (see Step 4 risk note). Steps 5–6: the wrapped `StateMachine<TState>` can be deleted without touching save format since the underlying `int phase`/`int Phase` fields remain authoritative and untouched.

---

## Exit Criteria

- [ ] `StateMachine<TState>` exists in `Assets/Ashfall.Core/StateMachine/` with full implementation
- [ ] Game lifecycle (`GameState`, 3 states, Option A) is formalized; Expedition (`ExpeditionPhase`, 5 states) and Combat (`CombatPhase`, 7 states) gain enforcement/history wrapping their **existing** enums — no new enum definitions collide with the real ones
- [ ] All FSMs integrate with `CaptureState/RestoreState` (save/load works); existing `int phase`/`int Phase` save fields remain authoritative and unchanged in format
- [ ] Transition history is available for debug logging (reconciled with `CombatState.Events` for combat, not duplicated)
- [ ] The verified combat turn-order gap (player actions executable during `EnemyTurn`) is closed with an explicit guard and a test proving it
- [ ] Invalid transitions are structurally impossible (rejected by transition table)
- [ ] 25+ new tests pass; 0 existing tests broken (baseline ~2120 tests — re-verify count before claiming "0 broken")
- [ ] `dotnet build Ashfall.csproj` — 0 errors
- [ ] `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — all pass
- [ ] `godot --headless --path . -- --data-integrity-selftest` — 0 errors

## Review Notes (Corrected)

This plan was adversarially reviewed against the live codebase at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`. Summary of what changed and why:

1. **Two of the plan's six "state machines" already exist as real, structured enums, not implicit if/else chains.** `CombatPhase` (`Assets/Ashfall.Core/Combat/CombatTypes.cs:9`, 7 states: `Setup, PlayerTurn, EnemyTurn, Resolved, Won, Lost, Retreated`) and `ExpeditionPhase` (`Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs`, 5 states: `Outbound, Looting, Inbound, Completed, Failed`) both already ship with save-format integration (`int` fields on their state DTOs, versioned migration/clamping on load). The plan's original Steps 5 and 6 proposed *redefining* these enums with entirely different value sets (8-state and 10-state respectively), which would either fail to compile (duplicate type) or fork the concept into two incompatible representations. Both steps were rewritten to wrap the existing enums instead of replacing them.
2. **The claimed "complete an already-completed expedition" bug does not appear to exist.** `ExpeditionSystem`'s tick loop already `continue`s past `Completed`/`Failed` expeditions before any completion logic runs, and `SetPhase()` already no-ops on a same-phase transition. The real, verified gap in combat is different and narrower: player action methods (`PlayerFire`, `PlayerSuppress`, etc.) don't check `_state.Phase` before executing, so they are — as far as this session's read confirmed — callable during `EnemyTurn`. That is the concrete bug worth fixing; the plan now targets it directly instead of a bug that isn't there.
3. **`RadiationPhaseProgression` already has 6 phases with guarded transitions and change events** (`Healthy, Prodromal, Latent, ManifestIllness, ChronicFibrosis, RecoveryOrDeath`, via a `TransitionTo()` helper that no-ops on same-phase and fires `OnPhaseChanged`), not the claimed 4-level `Mild/Moderate/Severe/Critical` threshold checks. This system was named in the Motivation table but never had its own migration step — the correction narrows the claim without adding new scope, since no step existed to over-scope.
4. **`VigilStateMachine` is not a state machine in the sense this plan uses the term.** It's a single countdown timer with `IsActive`/`IsCompleted` booleans and no enumerated states or transition table. It should not be counted among the systems this plan formalizes; there's nothing here that fits `StateMachine<TState>`'s shape.
5. **The real game lifecycle enum is `private enum GameState { Menu, Playing, GameOver }`** at `src/Main.cs:271` — 3 states, matching the original Motivation table's top row, not the 9-state `GamePhase` invented in Step 4 (`Initializing, MainMenu, NewGameSetup, Loading, Playing, Paused, DayTransition, GameOver, Exiting`). Step 4 now requires an explicit choice between formalizing the real 3-state enum (Option A, matches this batch's stated risk level) or introducing substantial new lifecycle behavior (Option B, which is scope creep beyond "formalize existing implicit behavior" and needs separate sign-off).
6. **`--bridge-selftest` cannot verify this batch's behavior.** Per the project's own AGENTS.md, this command now only prints a removal notice and exits 0 (the Unity bridge shim is gone) — it does not boot into the app loop or exercise any gameplay state. Every step's verification section that cited it as a meaningful smoke test has been corrected to note this limitation.
7. **Numeric baseline for the test suite is ~2120 tests** (`grep -rc "\[Fact\]\|\[Theory\]" Ashfall.Core.Tests/` = 2120, verified this session), used consistently in the corrected Done-when/verification sections instead of leaving "existing tests" unquantified.
8. **Risk/rollback was previously thin for a Medium-risk plan.** Each of Steps 4–6 now has an explicit Risk/Rollback paragraph naming what could break and how to revert without reconstructing deleted code from git history (dual-write period for Step 4; save-field-authority decision for Step 5; behavior-change confirmation for Step 6's new guard).
