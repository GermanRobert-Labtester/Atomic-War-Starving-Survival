# ASHFALL Event Surface & Notification Architecture

**Status:** Canonical Architecture Policy
**Applies To:** `Assets/Ashfall.Core`, `src`, UI panels, and host controllers
**Authority Level:** Invariant 1 (Zero Engine Coupling), Invariant 2 (Ports and Adapters), Invariant 5 (No Gameplay Logic in Hosts)

---

## 1. Executive Standard

**Strongly typed C# events (`event Action<T>` / `event Action<T1, T2>`) are the canonical, sanctioned pattern for domain state mutation notifications across ASHFALL.**

```csharp
// Canonical domain pattern:
public event Action<JournalEntry>? OnEntryAdded;
public event Action<string>? OnCodexUnlocked;
public event Action<StudyJob>? OnJobCompleted;
```

### Core Invariants:
1. **Strong Typing:** Event payloads must be strongly typed domain entities, records, or DTOs.
2. **Direct Subscriptions:** Systems wire direct C# delegates or lambda handlers during composition root initialization (`Main.cs` / `GameBootstrap`).
3. **No Hidden Indirection:** Domain aggregates own their event surfaces; there is no global untyped mediator routing gameplay events.

---

## 2. Bounded Scope of `IEventBus`

The untyped, string-keyed event bus (`IEventBus` / `SimpleEventBus`) is strictly bounded and restricted to cross-boundary infrastructure:
- **Radio Broadcast Carriers:** Real-time audio carrier open/close and census broadcast synchronization (`VerdictRadioSystem`, `VerdictCensusBroadcast`).
- **Surface Boundary Runners:** Isolated external mini-campaign runners (`DiveInstanceRunner`).
- **Host Adapters:** Decoupled external triggers (`HostEventAdapter`).

> **FORBIDDEN:** Do not introduce `IEventBus` into core gameplay systems (`Inventory`, `NeedsSystem`, `WeatherSystem`, `MedicalWardSystem`, `JournalSystem`, `ResearchSystem`, `CraftingSystem`, etc.). Core systems communicate via direct typed C# events or explicit method calls.

---

## 3. Prohibited Event Mechanisms

To maintain deterministic replay, zero runtime overhead, and simple call stacks, the following patterns are prohibited in `Assets/Ashfall.Core`:
- **MediatR / Command Buses:** No reflection-based command/notification dispatchers.
- **Reactive Extensions (Rx):** No `IObservable` / `ISubject` pipelines.
- **Unity `UnityEvent` / Engine Signals:** Core has zero engine coupling; Godot signals exist strictly on host UI nodes for Godot lifecycle events.

---

## 4. Restore Suppression Contract

> **CRITICAL RESTORE INVARIANT: `RestoreState()` must NEVER emit state mutation events.**

When restoring state from a save game:
1. The state being loaded represents historical, already-committed truth.
2. Emitting mutation events (`OnEntryAdded`, `OnNotificationPing`, `OnTechUnlocked`, `OnActionCompleted`, etc.) during restore causes severe bugs:
   - Duplicate UI toasts and intrusive notification popups.
   - Secondary side-effects re-executing (e.g., granting bonus items twice, re-infecting dwellers).
   - Polluting historical logs or stats counters.
3. UI synchronization events (e.g. `OnWorkshopStateChanged` or `OnLibraryChanged`) may be invoked to refresh displays, but **never** events that signal new gameplay occurrences or grant rewards.

---

## 5. State Mutation → Event Ordering Contract

When a state mutation occurs:
1. The system must update its internal state (`_entries`, `_flags`, `_state`) **first**.
2. The event is raised **second**.
3. Subscribers that query the system inside their event handler must observe the complete, consistent post-mutation state.
