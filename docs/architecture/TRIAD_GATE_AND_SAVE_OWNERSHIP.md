# ASHFALL — Triad Drift Gate & Subsystem Save Ownership

**Date:** 2026-08-27
**Scope:** Documents the architectural rationale, save store bindings, and domain ownership for all intentional exceptions to the `SetupXxx` ↔ `SaveXxx` naming convention checked by `scripts/ci/triad-drift-gate.sh`.

---

## 1. Background: The Triad Pattern & Triad Drift Gate

In the ASHFALL Godot host architecture ([`src/Main.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/src/Main.cs) and its domain partial files), persistent game subsystems follow the **Triad Pattern**:

```
SetupXxx()          ── Constructs & wires the domain host session and dependencies
SaveXxx()           ── Captures state snapshot into a versioned, checksummed SaveStore
FlushXxxIfDirty()   ── (Optional) Performs deferred write-to-disk when dirty flags trip
```

### The Declarative Save Section Authority (`Invariant H7`)
If a developer implements a `SetupXxx()` method without a corresponding `SaveXxx()` method declared in [`Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Assets/Ashfall.Core/Save/SaveSectionRegistry.cs), that subsystem will operate during runtime but silently drop its state upon save or shutdown.

The CI script [`scripts/ci/triad-drift-gate.sh`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/scripts/ci/triad-drift-gate.sh) runs in **Tier 1 Mandatory CI** to enforce that:
1. Every save section declared in `SaveSectionRegistry.cs` has a matching `SaveXxx()` method in `src/Main*.cs`.
2. Every declared save section requiring setup has its matching `SetupXxx()` method in `src/Main*.cs`.
3. `Main.SaveOrchestrator.cs` consumes `SaveSectionRegistry.SectionKeys` for all section aggregation.
4. No un-registered rogue `SaveXxx()` methods exist in the Godot host.

---

## 2. Declarative Triad-Gate Section Mappings & Exemptions

Save sections, save methods, setup initializers, and exemptions are declaratively registered in [`Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Assets/Ashfall.Core/Save/SaveSectionRegistry.cs) with explicit metadata (`SectionKey`, `SaveMethod`, `SetupMethod`, `Owner`, `Description`, `RequiresSetup`):

| # | Save Method | Nominal Setup Mismatch | Actual Setup Location & Wiring | Save Store & Section Key | Domain Owner |
|---|---|---|---|---|---|
| **1** | `SaveChemicalDependency()` | *Inline in Crisis Setup* | `SetupMentalHealthCrisis()` in [`src/Main.ShelterBatch3.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/src/Main.ShelterBatch3.cs) | [`ChemicalDependencySaveStore`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/src/Host/ChemicalDependencySaveStore.cs)<br>Key: `"chemical_dependency"` | Medical & Shelter Subsystem Team (`Ashfall.Core.Medical`) |
| **2** | `SaveDailyBriefing()` | *No `SetupDailyBriefing`* | `SetupDailyBriefingModal()` in [`src/Main.Campaign.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/src/Main.Campaign.cs) | [`DailyBriefingSaveStore`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/src/Host/DailyBriefingSaveStore.cs)<br>Key: `"daily_briefing"` | Campaign & Progression Team (`AtomicWar.GodotApp.Host`) |
| **3** | `SaveExpansionHub()` | *No `SetupExpansionHub`* | `SetupExpansions()` in [`src/Main.ExpansionHub.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/src/Main.ExpansionHub.cs) | [`ExpansionHubSaveStore`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/src/Host/ExpansionHubSaveStore.cs)<br>Key: `"expansion_hub"` | Expansion Framework Team (`AtomicWar.GodotApp.Host`) |
| **4** | `SaveHoldfast()` | *No `SetupHoldfast`* | `SetupHoldfastRuntime()` in [`src/Main.Holdfast.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/src/Main.Holdfast.cs) | [`HoldfastSaveStore`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/src/Host/HoldfastSaveStore.cs)<br>Key: `"holdfast"` | Holdfast / Exp 01 Team (`Ashfall.Core` / `Host`) |
| **5** | `SavePhantomMemory()` | *No `SetupPhantomMemory`* | `SetupPhantom()` in [`src/Main.Phase0.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/src/Main.Phase0.cs) | [`PhantomMemorySaveStore`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/src/Host/PhantomMemorySaveStore.cs)<br>Key: `"phantom_memory"` | Phase 0 & Lineage Team (`Ashfall.Core.StandingRecord`) |

*(Note: `SaveWastelandMap()` is a related 6th entry whose initialization is delegated to [`WorldHostSession.Create()`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/src/Host/WorldHostSession.cs) rather than a standalone `SetupWastelandMap` in `Main`.)*

---

## 3. Detailed Forensic Analysis & Save Ownership

### 1. `SaveChemicalDependency` (Inline Initialization)
- **Architectural Rationale:** The chemical dependency ledger tracks physiological substance addictions, cold-turkey penalties, and supervised detox lockboxes. Because chemical dependencies trigger mental breakdown events, the dependency system is created inline during `SetupMentalHealthCrisis()` and shared between medical and crisis presenters.
- **Persistence Target:** `user://saves/save_chemical_dependency.json`
- **Envelope Integrity:** Wrapped in a versioned `{ State, Checksum }` envelope verified by [`Ashfall.Core.Tests/BareSaveStoreSealTests.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Ashfall.Core.Tests/BareSaveStoreSealTests.cs).
- **Save Trigger:** Called directly during `SaveAll()` and on daily rollover flush.

### 2. `SaveDailyBriefing` (Nominal Mismatch)
- **Architectural Rationale:** The daily briefing UI represents both an informational modal dialog and a stateful log of morning radio intercepts and weather alerts. The initializer is named `SetupDailyBriefingModal()` to indicate that it instantiates the UI container alongside the host session.
- **Persistence Target:** `user://saves/save_daily_briefing.json`
- **Envelope Integrity:** Sealed with `DailyBriefingSaveStore` checksum contract.
- **Save Trigger:** Called on morning transition and during global `SaveAll()`.

### 3. `SaveExpansionHub` (Nominal Mismatch)
- **Architectural Rationale:** The Expansion Hub orchestrates unlocking, status queries, and prerequisites for all expansions (01 through 14). The setup routine is named `SetupExpansions()` because it registers the entire expansion master catalog in addition to the hub presenter.
- **Persistence Target:** `user://saves/save_expansion_hub.json`
- **Envelope Integrity:** Versioned expansion flag codec with checksum envelope.
- **Save Trigger:** Called whenever an expansion state changes and during `SaveAll()`.

### 4. `SaveHoldfast` (Nominal Mismatch)
- **Architectural Rationale:** The Holdfast domain comprises two distinct save facets: the core expansion storyline/locations (`SaveHoldfast` → `"holdfast"`) and the live trade ledger / salt tariff engine (`SaveHoldfastRuntime` → `"holdfast_trade"`). Both are initialized together in `SetupHoldfastRuntime()`.
- **Persistence Target:** `user://saves/save_holdfast.json`
- **Envelope Integrity:** Validated by `HoldfastSaveCodec` and `HoldfastTradeSaveStoreSelfTest`.
- **Save Trigger:** Called on expedition resolution, tariff collection, and global `SaveAll()`.

### 5. `SavePhantomMemory` (Nominal Mismatch)
- **Architectural Rationale:** Phase 0 lineage tracks phantom memories of deceased shelter dwellers across generations. The initialization method is shorthand `SetupPhantom()` in `src/Main.Phase0.cs`.
- **Persistence Target:** `user://saves/save_phantom_memory.json`
- **Envelope Integrity:** Verified by `PhantomMemorySaveStore` and roundtrip test suite.
- **Save Trigger:** Called on dweller death, memorial inscription, and global `SaveAll()`.

---

## 4. Tracked Architectural Debt & Resolution Plan

While all five exceptions are fully functional, save-safe, and gated against regressions in CI, the nominal mismatches represent minor cognitive debt.

### Future Cleanup Strategy (`Phase 12 Main.cs Domain Extraction`):
1. **Rename to 1:1 Pairs:** When `Main.cs` is separated into dedicated domain partials (`Main.Campaign.cs`, `Main.ExpansionHub.cs`, `Main.Holdfast.cs`, `Main.Phase0.cs`), normalize method names to exact 1:1 pairs:
   - `SetupDailyBriefingModal()` → `SetupDailyBriefing()`
   - `SetupExpansions()` → `SetupExpansionHub()`
   - `SetupPhantom()` → `SetupPhantomMemory()`
2. **Preserve Wire Compatibility:** Retain the existing section keys in `AllSaveSections` (`"daily_briefing"`, `"expansion_hub"`, `"phantom_memory"`) so existing save files load seamlessly.
3. **Automated Enforcement:** Once normalized, remove each entry from `NO_SETUP_NEEDED` in `scripts/ci/triad-drift-gate.sh` to enforce zero-exception strict 1:1 symmetry.
