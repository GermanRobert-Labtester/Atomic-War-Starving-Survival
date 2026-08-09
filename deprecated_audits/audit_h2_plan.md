========================================================================================
DEPRECATED AND FOLDED — ALL AUDIT WORK VERIFIED & COMPLETED
Status: RESOLVED & CLOSED
Date: 2026-08-08
========================================================================================

# H-2 Remediation Plan — EventBus Lifecycle

## Goal

Multiple systems subscribe to `EventBus` and to class-level `event Action`
handlers but never unsubscribe. In a long PlayMode session, the static
`EventBus._subscribers` dictionary grows without bound. Worse, when a
`SaveSystem` (or any other subscriber) is replaced (e.g. via a test fixture
or a future "new game" flow), the old reference is held by the EventBus
forever, preventing GC of the related state.

## Audit Findings

| Subscriber | Subscribe site | Unsubscribe site | Status |
| --- | --- | --- | --- |
| `SaveSystem` | `_gameState.OnPhaseChanged += OnPhaseChanged;` (ctor line 158) | none | **LEAK** |
| `ExpeditionSystem` | `EventBus.Subscribe<FlashpointInterceptSignal>(...)` (line 119-120) | none | **LEAK** |
| `AudioEventBus` | `EventBus.Subscribe<WeatherKind>(...)` (line 85-86) | `Teardown()` exists | OK |
| `MoralChronicleBridge` | `EventBus.Subscribe<CampaignEndedEvent>(...)` (line 58-60) | `OnDestroy()` unsubscribes | OK |
| `GameBootstrap` | `EventBus.Subscribe<FlashpointEmptiedDevices>(...)` (line 1468) | none | **LEAK** |
| `GameBootstrap` | `WorldPhaseSystem.OnPhaseChanged += phase => {...}` (line 1202) | none | **LEAK** |
| `GameBootstrap` | `GameState.OnPhaseChanged += phase => {...}` (line 1262) | none | **LEAK** |
| `CorpseManagementSystem` | `_needs.OnDied += HandleDeath;` (line 94) | none | **LEAK** |
| `GameBootstrap` | `NeedsSystem.OnDied += deceased => {...}` (line 745) | none | **LEAK** |
| `GameBootstrap` | `NeedsSystem.OnNeedChanged += (sv, kind, value) => {...}` (line 3609) | none | **LEAK** |

## Changes

1. **Make `SaveSystem` `IDisposable`.** Add a `Dispose()` method that
   unsubscribes from `_gameState.OnPhaseChanged` and clears the
   `EventBus` delegate cache. Document the contract: "call Dispose when
   you replace the SaveSystem."

2. **Add `ExpeditionSystem.UnsubscribeAll()`.** Mirror the `AudioEventBus`
   pattern: keep a list of (Type, Delegate) pairs that were subscribed,
   and unsubscribe them all in a single call.

3. **Add `GameBootstrap.OnDestroy()`.** Unsubscribe from the 5
   subscriptions GameBootstrap makes directly. Also call
   `SaveSystem?.Dispose()` and `ExpeditionSystem?.UnsubscribeAll()` so
   the test fixtures and the future "new game" flow don't leak.

4. **Add a public `EventBus.SubscribersCount` diagnostic** so tests can
   verify that subscriptions are removed. This is the only new EventBus
   API; the test relies on it for assertions.

5. **Test coverage in `Assets/Tests/EditMode/EventBusLifecycleTests.cs`:**
   - **Subscribe_IncrementsCount** — basic smoke test.
   - **Unsubscribe_DecrementsCount** — verifies the count drops after
     unsubscribe.
   - **DuplicateSubscribe_DoesNotIncrement** — verifies the
     `EventBus` dedup (current behavior).
   - **SubscribeSameHandlerTwice_IsIdempotent** — same as above.
   - **SaveSystem_Dispose_UnsubscribesFromGameState** — creates a
     SaveSystem, captures the count, disposes, asserts the count drops.
   - **ExpeditionSystem_UnsubscribeAll_ClearsAllSubscriptions** — same.
   - **GameBootstrap_OnDestroy_ClearsBootstrapSubscriptions** —
     constructs a GameBootstrap, calls OnDestroy, asserts the count drops.
   - **ReplacingSaveSystem_DoesNotLeakOldInstance** — the real bug:
     create SaveSystem A, attach to GameBootstrap, create SaveSystem B,
     destroy the bootstrap, verify no EventBus subscriptions remain.

## Why not `IDisposable` everywhere?

The convention in the codebase is `MonoBehaviour.OnDestroy` for MonoBehaviours
and `Teardown()` for plain C# services. I'll follow the existing convention:
- `SaveSystem` is plain C# → `IDisposable` is the right idiom
  (production code calls `using` or `Dispose`).
- `ExpeditionSystem` is plain C# → `UnsubscribeAll()` matches the existing
  `AudioEventBus.Teardown()` pattern.
- `GameBootstrap` is a `MonoBehaviour` → `OnDestroy()` is the right idiom.

## Risk

The Dispose/Unsubscribe is additive — the only behavior change is that
subsequent `EventBus.Raise<T>()` calls no longer invoke the dead subscribers.
This is a strict improvement; no code path can break.
