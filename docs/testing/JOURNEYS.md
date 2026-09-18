# ASHFALL — Five Canonical Campaign Journeys (Plan 27C / Wave 10)

> **Authority:** `C1_planintegration[6].md` & `Plan_27_Tests_That_Mean_It_Fidelity_Coverage_Journeys.md`\
> **Status:** ACTIVE\
> **Last Updated:** 2026-09-17\
> **Test Harness:** `src/Main.UiTests.RealCampaignJourney.cs`, `src/Host/UiAccessibilitySelfTest.cs`, `src/Host/PanelBindLifecycleSelfTest.cs`, `Ashfall.Core.Tests/Journeys/`, `Ashfall.Core.Tests/Endgame/`\

---

## 1. Executive Summary & Purpose

A test suite may report thousands of passing unit tests while a game remains unplayable due to broken inter-system seams. The historical ASHFALL suite contained 5,303 passing tests yet permitted an unreachable hardcoded epilogue, inert inventory consume callbacks, 30 unbacked console panels, and gear that never wore out.

**The Five Canonical Journeys** replace isolated synthetic assertions with end-to-end operational proofs. A journey fails when **any system in the chain fails to affect the next system's state**.

```
                   ┌─────────────────────────────────────────────────────────┐
                   │             THE FIVE CANONICAL JOURNEYS                 │
                   └────────────────────────────┬────────────────────────────┘
                                                │
         ┌───────────────────┬──────────────────┼──────────────────┬───────────────────┐
         ▼                   ▼                  ▼                  ▼                   ▼
   JOURNEY 1           JOURNEY 2          JOURNEY 3          JOURNEY 4           JOURNEY 5
   First-Hour          Epilogue &         Persistence        Accessibility       Panel Authority
   Continuity          Ending Matrix      & Parity           & Navigation        Identity
   (Boot→Survival)     (200-Day Choices)  (Atomic Envelope)  (Keyboard Focus)    (No Fake Consoles)
```

---

## 2. Journey 1: First-Hour Continuity Journey

### Overview & Operational Chain
Validates the complete onboarding, early survival loop, and first monthly milestone of a fresh campaign. Proves that every gameplay action propagates state forward into dependent systems rather than dropping silently.

### Step Sequence
1. **New Game Bootstrap:** Player invokes `StartNewGame()`. `ComposeCampaign()` instantiates real services (`CampaignDayCoordinator`, `SurvivorsHostSession`, `InventoryHostSession`, `WorldHostSession`). Initial roster of 3 survivors instantiated with day 1 calendar clock.
2. **Player Guidance & Onboarding:** Player reads early holdfast briefing via Guidance panel (Plan 17B), establishing initial survival goals.
3. **Rationing Modification:** Shelter diet policy updated; daily rationing consumes canned food and fresh water from the real inventory.
4. **Item Crafting & Inventory Mutation:** Survivor crafts an item (e.g. protective gear or basic tools); ingredients are deducted and the crafted instance enters inventory.
5. **Expedition Dispatch:** Living survivor equipped with tracked gear is dispatched via `StartExpedition()` to a danger zone.
6. **Hazard / Environmental Storm:** `WorldHostSession` / `WeatherSystem` rolls Fallout Storm or Black Rain, driving up environmental ambient radiation.
7. **Dose Accumulation & Gear Degradation:** Survivor in transit incurs radiation dose; protective gear incurs condition wear via `EquipmentConditionSystem` / `WeaponEquipmentBridge`.
8. **Medical Triage & Treatment:** Survivor returns contaminated; anti-rad or medical clinic treatment administered (`AdministerAntiRad`, `AdministerIodine`), reducing acute dose without erasing separate lifetime exposure ledger.
9. **Duty Shift & Survivor Casualty:** In severe casualties, fallen survivor's duty station is vacated (`DutyRosterSystem` shift reassignment), preventing ghost staffing.
10. **Memorialization:** Lost survivor recorded in `MemorialSystem` with cause and day of death.
11. **Day 30 Briefing Legibility:** `CampaignDayCoordinator` advances through monthly cycle; dawn briefing reflects roster loss, resource deficit, and memorial entries accurately.

### Invariants Asserted
- **INV-JOURNEY-1.1:** Every action must mutate downstream consumer state (e.g., combat trauma mutates Phase 0 hypervigilance; expedition triggers combat; combat resolution degrades equipped weapon and deposits loot into shared inventory).
- **INV-JOURNEY-1.2:** Zero silent no-ops on core callbacks (null consume callback defect permanently closed).

### Historical Gaps Retired
- Wave 1 Gaps 1–5: Inert consume call, unwired combat victory loot, un-updated hypervigilance.
- Wave 2 Plans 21A, 22C, 24A: Weapon wear failure, unlinked clinic triage, ghost shift assignments.

### Test Harness & Invocation
- Headless Godot: `godot --headless --path . -- --real-campaign-journey-selftest`
- Host CLI: `godot --headless --path . -- --day1-selftest`
- xUnit Suites:
  - `Ashfall.Core.Tests/Journeys/EndToEndPlayerJourneyTests.cs`
  - `Ashfall.Core.Tests/OnboardingJourneyTests.cs`
  - `Ashfall.Core.Tests/Inventory/Plan21EndToEndJourneyTests.cs`
  - `Ashfall.Core.Tests/Medical/Plan24SurvivorJourneyTests.cs`

---

## 3. Journey 2: Ending & Epilogue Matrix Journey

### Overview & Operational Chain
Proves that the campaign's conclusion, epilogue prose, and chronicled verdict derive entirely from player choices, faction standing, treaty ratifications, and survivor mortality, rather than static strings.

### Step Sequence
1. **Extended Campaign Progression:** Campaign advances across 200+ simulation days under a defined policy stance (e.g., Expansionist, Isolationist, Pragmatist, or Martyr).
2. **Moral Choice Invocations:** Player encounters systemic moral dilemmas (`TryResolveMoralChoice`); choices record permanent flags in `CampaignFlags` and affect faction standings.
3. **Faction Treaties & Reckoning:** Regional treaties ratified or repudiated (`RegionalTreatyState`); debt ledgers settled or burned; tribunal evidence compiled.
4. **Terminal Campaign Trigger:** Campaign reaches victory or collapse condition (e.g., Tempest decommissioned, resource exhaustion, or community survival threshold).
5. **Epilogue Projection:** `EpilogueMatrixRuntime` evaluates the accumulated campaign facts:
   - Living roster survivor count & memorial loss list.
   - Ratified treaties and regional power balance.
   - Resource independence vs external dependency.
   - Reckoning phase and debt disposition.
6. **Verdict & Chronicle Export:** Terminal briefing generated with distinct historical text tailored to the specific trajectory; save file marked concluded.

### Invariants Asserted
- **INV-JOURNEY-2.1:** Epilogue projection is deterministic and sensitive to historical flags (divergent choice policies yield demonstrably divergent terminal texts).
- **INV-JOURNEY-2.2:** Ending state survives save/load round-trip before final dismissal (`Plan19SessionContinuityJourneyTests`).

### Historical Gaps Retired
- Hardcoded ending string literal in `src/Main.GameFlow.cs:444`.
- `TryResolveMoralChoice` having zero callers in production gameflow.
- Unconnected memorial records in endgame summaries.

### Test Harness & Invocation
- xUnit Suites:
  - `Ashfall.Core.Tests/Endgame/Plan19SessionContinuityJourneyTests.cs`
  - `Ashfall.Core.Tests/Journeys/MoralChoiceJourneyTests.cs`
  - `Ashfall.Core.Tests/MoralChoice/Plan144MoralChoiceStubClosureTests.cs`

---

## 4. Journey 3: State Persistence & Reload Parity Journey

### Overview & Operational Chain
Guarantees that a complete campaign session can be saved into a single atomic envelope, completely cleared from process memory, and restored with 100% bitwise and relational fidelity.

### Step Sequence
1. **Live State Mutation:** In a running campaign, execute multi-system mutations:
   - Barter and inventory trades (e.g. Holdfast trade buying items into shared storage).
   - Combat encounters causing ammo expenditure and trauma gains.
   - Radiation exposure and partial medical anti-rad treatments.
   - Room power allocations and grid load variations.
2. **Atomic Envelope Commit:** Invoke `SaveAll(playCue: false)`. All active host sessions capture their state into `AggregateSaveEnvelope` (`campaign.json`) with CRC32 checksum.
3. **Simulated Process Reset:** Invoke `ResetAllSessionsInMemory()`—the production reset executed during game restart or loading a different slot. Clears all in-memory hosts, caches, and session pointers.
4. **Production Continue:** Invoke `TryLoadAndRestoreGame(slotId)`. Envelope deserialized, checksum validated, and each registered session restores its domain section.
5. **Post-Load State Assertion:** Validate exact state match against pre-save values:
   - Calendar day and world clock match.
   - Inventory counts match exactly (no items erased).
   - Radiation dose and lifetime exposure match exactly.
   - Holdfast trade session held-counts match shared inventory.
6. **Post-Load Liveness Proof:** Execute a subsequent typed mutation against the restored session (e.g., consume item, administer iodine, or sell goods). The restored system must accept the action and mutate correctly.

### Invariants Asserted
- **INV-JOURNEY-3.1:** Restoring a subsystem must never corrupt or clear unrelated shared systems (e.g., permanently preventing `HoldfastTradeSession` from invoking `Inventory.Clear()` on shared inventory).
- **INV-JOURNEY-3.2:** Post-reload liveness: restored objects are active simulation entities, not inert mock records.

### Historical Gaps Retired
- Subsystem save restore wiping shared inventory (`HoldfastTradeSession` bug).
- Unpersisted radiation dose states (AGENTS.md H10 gap).
- Detached save stores failing to write through `SaveSlotService`.

### Test Harness & Invocation
- Headless Godot: `godot --headless --path . -- --real-campaign-journey-selftest`
- Headless Godot: `godot --headless --path . -- --7-day-smoke-selftest`
- xUnit Suites:
  - `Ashfall.Core.Tests/Save/GoldenSaveFixtureTests.cs`
  - `Ashfall.Core.Tests/Tooling/SaveStateRoundTripCoverageGateTests.cs`

---

## 5. Journey 4: Keyboard Navigation & Accessibility Journey

### Overview & Operational Chain
Certifies that the entire player-facing UI can be operated entirely through keyboard navigation without a mouse, maintaining focus clarity, zero modal traps, and predictable tab ordering.

### Step Sequence
1. **UI Hierarchy Instantiation:** Boot production UI canvas containing all standard and expanded overlay panels.
2. **Sequential Panel Traversal:** For every player-routable panel registered in `PanelRegistry`:
   - Route to and open the panel.
   - Verify non-empty labels and accessible text representations (no color-only status indicators).
   - Walk keyboard focus through interactive elements (`Control.FocusMode = FocusModeEnum.All`).
   - Validate that Tab / Shift-Tab cycle deterministically through controls.
3. **Escape / Close Handling:** Send keyboard `ui_cancel` / `Esc` event.
4. **Modal Trap Verification:** Verify panel closes cleanly, returns focus to previous surface, and does not trap focus in an invisible or disabled control.
5. **No Visual Clipping / Overflow:** Ensure text containers and scroll views maintain readable boundaries at canonical 1920x1080 resolution.

### Invariants Asserted
- **INV-JOURNEY-4.1:** Zero modal traps: every opened surface must have at least one keyboard-accessible path to close or return.
- **INV-JOURNEY-4.2:** Complete keyboard reachability: all interactive actions (craft, trade, treat, dispatch) can be triggered without mouse click events.

### Historical Gaps Retired
- Orphaned dialog panels that could not be dismissed without mouse clicks.
- Unfocusable expanded panels.
- Color-only alerts violating contrast and accessibility guidelines.

### Test Harness & Invocation
- Headless Godot: `godot --headless --path . -- --ui-accessibility-selftest`
- Headless Godot: `godot --headless --path . -- --accessibility-selftest`

---

## 6. Journey 5: Runtime Panel Authority Identity Journey

### Overview & Operational Chain
Enforces the project's single-authority architectural rule: UI panels are presentation-only adapters that bind directly to Core campaign references. No panel may construct a parallel simulation or display mock state.

### Step Sequence
1. **Panel Registry Audit:** Enumerate all live player-navigable panels in `PanelRegistry`.
2. **Authority Binding Assertion:** When `Main` binds panels during startup or campaign compose:
   - Panel `Bind(...)` must accept the exact singleton reference from `Main`'s campaign service graph (e.g. `_world`, `_survivors`, `_inventory`).
   - Panel `IsBound` must be true.
3. **Node Callback Lifecycle Validation:** For each panel type:
   - Initial Bind: events hooked up; UI reflects current Core state.
   - Unbind: listeners detached; mutations in Core do not fire events on unbound panel.
   - Rebind: single-subscription semantics preserved; no duplicate event delegates stacked.
   - Session Switch: cleanly unsubscribes old session and binds new session.
   - Cleanup: `_ExitTree()` / `Free()` tears down delegates cleanly without memory leaks.
4. **No Fresh Systems in Routes:** Source scanner verifies that route handlers in `Main.PlayerSurfaces.cs` and panel constructors do not instantiate fresh `new *System()` instances.

### Invariants Asserted
- **INV-JOURNEY-5.1:** Identity Parity: `panel.BoundSystem == campaign.System` (reference equality, not value clone).
- **INV-JOURNEY-5.2:** Anti-Fabrication: Shelved prototypes remain un-navigable; live panels must have genuine Core data sources.

### Historical Gaps Retired
- 30 fake consoles marked `IsBound = true` without underlying systems.
- Fresh `new WeatherSystem()` / `new NeedsSystem()` inside UI callbacks creating detached islands.
- Delegate stacking on repeated panel open/close cycles causing quadratic event execution.

### Test Harness & Invocation
- Headless Godot: `godot --headless --path . -- --panel-bind-lifecycle-selftest`
- xUnit Suites:
  - `Ashfall.Core.Tests/UI/PlayerSurfaceLivenessGateTests.cs`
  - `Ashfall.Core.Tests/UI/PlayerSurfaceCoverageGateTests.cs`
  - `Ashfall.Core.Tests/Tooling/NoFreshCampaignSystemGateTests.cs`

---

## 7. Flake Control & Determinism Discipline

To ensure journeys remain trusted CI gates rather than ignored nuisances:

1. **Explicit Seed Pinning:** All journey runs must receive an explicit seed (e.g. `seed: 4242`). Never seed from system clock (`DateTime.Now`) or memory addresses.
2. **Culture-Invariant Formatting:** All numeric parses, floating-point string serializations, and checksums use `CultureInfo.InvariantCulture`.
3. **Zero-Flake Policy:** Any journey test that flakes in CI must be diagnosed and resolved immediately. Tests are never tagged as "flaky" to be bypassed.
4. **Tier Partitioning:**
   - **Tier 1 (Fast Critical Path):** Unit tests, fidelity gates, source scanners, and coverage gate (< 60s total).
   - **Tier 2 (Deep Runtime Journeys):** Headless Godot journeys (`--real-campaign-journey-selftest`, `--ui-accessibility-selftest`, `--panel-bind-lifecycle-selftest`).
