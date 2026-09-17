# ASHFALL Quality Roadmap — Batch 50
## Theme: HoldfastRuntimeSession Refactor — Eliminate Invariant 5 Violation (H1)

**Priority:** HIGH (H1 — longest-standing architectural violation)
**Risk:** Medium — behavioral changes to survival mechanics routing
**Prerequisite:** Batch 49 Step 1-2 (partial extraction gives cleaner workspace)

---

## Rationale

`src/Host/HoldfastRuntimeSession.cs` (372 lines, namespace `AtomicWar.GodotApp`, confirmed via direct read) implements its own survival simulation (HP, hunger, thirst, radiation, death, consumption, location visits) with hardcoded constants (`MaxHealth`, `MaxHunger`, `MaxThirst`, `RadDamageThreshold`, `StarvationThreshold`, `DehydrationThreshold`). This duplicates what `NeedsSystem` (`Assets/Ashfall.Core/Survivors/NeedsSystem.cs`) and `RadiationSystem` (`Assets/Ashfall.Core/Radiation/RadiationSystem.cs`) in Core already provide. Per Invariant 5, gameplay logic belongs in Core — Godot host code should only handle presentation, input, and wiring.

**Correction:** there is no `InventorySystem` class in Core. `HoldfastRuntimeSession` interacts with inventory through `Trade.Inventory` (a `HoldfastTradeSession`/`HoldfastCatalog`-scoped object) and calls `RemoveItem` directly. Inventory duplication is out of scope for this batch; do not imply an `InventorySystem` refactor target that doesn't exist.

**Correction (critical — changes the whole plan):** `HoldfastRuntimeSession` is constructed from a `CoreDemoSession` (see `HoldfastRuntimeSession(CoreDemoSession world, ...)` and `HoldfastRuntimeSession.Create(CoreDemoSession world, ...)`), which has **no relationship** to `SurvivorsHostSession` (which owns the real `NeedsSystem`/`RadiationSystem` instances and is constructed independently in `src/Main.cs` around line 2497, for an unrelated survivor-roster demo panel). `CoreDemoSession` (`src/Host/CoreDemoSession.cs`) does not hold a `NeedsSystem` or `RadiationSystem` field. Both `HoldfastRuntimeSession` and `SurvivorsHostSession` are constructed independently in `src/Main.cs` (lines ~2497 and ~4670) with no cross-wiring today. This means Step 4's plan to "construct the adapter with the existing NeedsSystem/RadiationSystem instances from SurvivorsHostSession" is not possible as written — those instances are not reachable from `HoldfastRuntimeSession`'s constructor. The plan below is corrected to either (a) give the adapter its own `NeedsSystem`/`RadiationSystem` instances scoped to the Holdfast demo, or (b) thread a `SurvivorsHostSession` reference into `HoldfastRuntimeSession.Create` explicitly. Option (a) is recommended to avoid entangling two independently-evolving host sessions; this plan proceeds with (a).

**Correction:** `RadiationSystem` has no `ApplyDose` method. Its actual public API is `Tick(float gameHours)`, which iterates survivors registered via delegate-based `ExposureContext` (constructor takes `exposureContext: Func<SurvivorRadState, ExposureContext>` and `applyNeed: Action<SurvivorRadState, string, float>` delegates — see constructor call in `SurvivorsHostSession.cs`). The adapter cannot simply call `RadiationSystem.ApplyDose(...)`; it must either construct `ExposureContext` per call and invoke `Tick`, or add a new direct-dose method to `RadiationSystem` (a Core change, which should be called out explicitly as in-scope for this batch, not assumed to already exist).

The risk is that the Holdfast demo/tutorial flow relies on these hardcoded mechanics. The refactor must preserve external behavior while routing through Core systems.

**Risk/rollback:** this is a Medium-risk behavioral change to a working demo flow (`--holdfast-selftest` and the terminal UI depend on `HoldfastRuntimeSession`'s public method signatures and return-string formats). Before starting, tag the current commit or create a branch so the pre-refactor behavior can be restored if characterization tests reveal behavior that cannot be reproduced through `NeedsSystem`/`RadiationSystem` (e.g., `NeedsSystem`'s Hunger/Thirst are 0..100 "higher = worse" continuous decay per game-hour, whereas `HoldfastRuntimeSession` currently does discrete +8/+10 per day-tick — these are different time models and will not produce bit-identical numbers). If exact numeric parity is not achievable, this must be surfaced to the user as a deviation, not silently absorbed by "close enough" characterization tests.

---

## Step 1 — Map HoldfastRuntimeSession Public API

**Goal:** Document every public/internal method and property of `HoldfastRuntimeSession` and classify each as: (a) pure presentation/wiring (keep), (b) gameplay logic (move to Core), (c) dead code (remove).

**Implementation:**
1. Read `src/Host/HoldfastRuntimeSession.cs` completely (372 lines, confirmed).
2. List every method with signature, line count, and classification. Confirmed method inventory to classify: constructor, `Create()`, `TrySave()`, `TryReload()`, `SeedDevelopmentState()`, `TickDay()`, `ConsumeFood()`, `ConsumeWater()`, `ExposeRadiation()`, `UseAntiRad()`, `Heal()`, `VisitLocation()`, `FindLocation()` (private), `GetQuestSummary()`, `DetermineDeathCause()` (private), `ArchiveAndFreshStart()`. Expect classification: `TickDay`/`ConsumeFood`/`ConsumeWater`/`ExposeRadiation`/`UseAntiRad`/`Heal`/`VisitLocation`'s radiation math/`DetermineDeathCause` → gameplay logic (move to Core); `TrySave`/`TryReload`/`ArchiveAndFreshStart`/`GetQuestSummary`/`FindLocation`/constructor/`Create`/`SeedDevelopmentState` → presentation/wiring/persistence orchestration (keep in host, out of scope for this batch); no dead code identified in the initial read, but confirm during the full audit.
3. Identify which Core systems already implement equivalent logic: `NeedsSystem` (hunger/thirst/health decay — but note the day-tick vs. hourly-tick time-model mismatch called out in Step 2/3) and `RadiationSystem` (dose accumulation and gear multipliers — but note `RadiationSystem` has no direct single-shot "apply this dose" method today; its API is `Tick(gameHours)` over registered survivors via `ExposureContext`).
4. Produce a mapping table: `HoldfastRuntime.Method → Core.System.Method`, explicitly marking any row where no equivalent Core method currently exists and new Core surface is required (e.g. radiation decay-on-tick and a possible direct-dose helper).

**Verification:**
- Mapping table reviewed, no method left unclassified
- No code changes yet

**Done when:** Classification document exists with zero ambiguous entries, and every "move to Core" row either names an existing Core method or explicitly flags that new Core surface must be added in Step 2.

---

## Step 2 — Create HoldfastSurvivorAdapter in Core

**Goal:** Create `Assets/Ashfall.Core/Holdfast/HoldfastSurvivorAdapter.cs` — a thin coordinator that wraps `NeedsSystem` and `RadiationSystem` to provide the Holdfast-specific survival API without duplicating logic. Confirmed via search: no `Assets/Ashfall.Core/Holdfast/` directory or `Ashfall.Core.Holdfast` namespace exists yet — this is genuinely new code, not a rename of an existing adapter.

**Implementation:**
1. Create class in `Ashfall.Core/Holdfast/` namespace `Ashfall.Core.Holdfast`.
2. Constructor accepts: `NeedsSystem needs`, `RadiationSystem radiation`, `ISeededRng rng`. The adapter owns its own `NeedsSystem`/`RadiationSystem` instances constructed fresh for the Holdfast demo (per the corrected Rationale above) — it does **not** receive them from `SurvivorsHostSession`, which is a separately-wired, unrelated session.
3. Expose methods: `TickDay()`, `ConsumeFood(string survivorId, int amount)`, `ConsumeWater(...)`, `UseAntiRad(...)`, `ApplyLocationRadiation(string survivorId, float zoneRads, WornGear gear)`.
4. `TickDay`/`ConsumeFood`/`ConsumeWater` delegate to `NeedsSystem.Modify(survivor, NeedKind, delta)` (confirmed signature in `NeedsSystem.cs`). `ApplyLocationRadiation` must construct a Core `ExposureContext` (`ZoneRadLevel`, `WornGear` list) and call `RadiationSystem.Tick(gameHours)` on a registered `SurvivorRadState` — `RadiationSystem` has **no `ApplyDose` method**; do not write code assuming one exists. If a direct single-shot dose API is genuinely needed (rather than routing everything through the hourly `Tick` loop), add `RadiationSystem.ApplyDirectDose(SurvivorRadState, float msv, WornGear gear)` as a new, explicitly-scoped Core method in this step and document it as new surface, not reused surface.
5. `DetermineDeathCause(string survivorId)` queries NeedsSystem state to pick the narrative death cause.
6. No `UnityEngine` or `Godot` references — pure Core.
7. Explicitly decide and document the time-model mismatch: `HoldfastRuntimeSession.TickDay()` currently applies discrete per-day deltas (Hunger +8, Thirst +10), while `NeedsSystem.Tick(gameHours)` applies continuous per-hour rates (`hungerPerHour = 0.8`, `thirstPerHour = 1.2`, i.e. ~19.2/day and ~28.8/day at 24 hours — not 8/10). Either recalibrate a `NeedsProfile` for Holdfast's simpler day-tick cadence to match current output, or accept and document the behavior change. Do not silently pick numbers that happen to compile.

**Verification:**
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — compiles
- Write 3 unit tests: TickDay reduces needs (assert exact Hunger/Thirst delta against the chosen `NeedsProfile`), ConsumeFood restores hunger by the expected amount, location radiation applies dose (assert `SurvivorRadState.RadiationDose` or `LifetimeRadiationExposure` increases by the expected, gear-adjusted amount)

**Done when:** `HoldfastSurvivorAdapter` passes 3+ unit tests with no engine coupling, and the chosen day-tick-to-hourly-rate mapping is documented in a code comment with the exact numbers it reproduces.

---

## Step 3 — Write Characterization Tests for Current Behavior

**Goal:** Before changing `HoldfastRuntimeSession`, capture its exact current behavior in tests so the refactor can be verified as non-breaking.

**Implementation:**
1. Create `Ashfall.Core.Tests/HoldfastSurvivorAdapterCharacterizationTests.cs`.
2. Test scenarios matching HoldfastRuntimeSession's hardcoded values (confirmed against `src/Host/HoldfastRuntimeSession.cs`):
   - `MaxHealth=100`, `MaxHunger=100`, `MaxThirst=100` (confirmed constants)
   - Daily hunger increase of +8, daily thirst increase of +10 (confirmed in `TickDay()`; note this is per-day, not per-hour — see Step 2 time-model note)
   - Radiation decay of 7%/day when `Radiation > 0` (confirmed: `Radiation -= Radiation * 0.07f`) — this decay is not mentioned in the original Rationale and must be reproduced or explicitly dropped
   - Radiation with hazmat (0.3x multiplier) and gas mask (0.6x multiplier) — confirmed in `VisitLocation()`: `if (hazmatCount > 0) radExposure *= 0.3f; else if (maskCount > 0) radExposure *= 0.6f;` (hazmat and mask are **not additive** — hazmat takes priority, mask is only checked if hazmat count is 0)
   - HP damage thresholds: `StarvationThreshold=90f` → `hpLoss += (Hunger-90)*0.5`, `DehydrationThreshold=90f` → `hpLoss += (Thirst-90)*0.6`, `RadDamageThreshold=50f` → `hpLoss += (Radiation-50)*0.1` (all three are additive per tick, confirmed)
   - Death trigger conditions: `DetermineDeathCause()` checks `Thirst >= MaxThirst` (100) first, then `Hunger >= MaxHunger` (100), then `Radiation >= 200`, then `Radiation >= 100`, else generic message — confirmed exact order and thresholds in source
3. The adapter should reproduce these exact thresholds (configure via constructor params or constants).

**Verification:**
- `dotnet test` — all characterization tests pass against the new adapter
- Values match the hardcoded constants in HoldfastRuntimeSession, including the radiation decay and the non-additive gear-multiplier priority (hazmat overrides mask, not stacked)

**Done when:** 10+ characterization tests pass, covering all survival tick edge cases, including the radiation decay rate and the hazmat/mask priority (not additive) behavior.

---

## Step 4 — Rewire HoldfastRuntimeSession to Use Adapter

**Goal:** Replace all inline survival logic in `HoldfastRuntimeSession` with calls to `HoldfastSurvivorAdapter`.

**Risk/rollback:** Medium risk. `HoldfastRuntimeSession` is instantiated in `src/Main.cs` in at least 4 places (lines ~1450, ~4670, ~4761, ~4773 — confirmed via search) including two error-path tests (insufficient funds, insufficient stock) that construct fresh `CoreDemoSession`/`HoldfastRuntimeSession` pairs. Any signature change to the constructor or `Create()` must keep all 4 call sites compiling. Before merging, diff the return-string formats of `TickDay()` and `VisitLocation()` — the Holdfast terminal UI parses/displays these strings directly, so even numerically-equivalent behavior with different formatting is a regression. If adapter-routed behavior cannot match current output exactly, stop and confirm the deviation with the user rather than merging silently.

**Implementation:**
1. Add `HoldfastSurvivorAdapter` field to `HoldfastRuntimeSession`.
2. In the session's initialization (constructor or `Create()`), construct the adapter with **new, Holdfast-scoped** `NeedsSystem`/`RadiationSystem` instances (per Step 2's corrected design) — do not attempt to pull these from `SurvivorsHostSession`, which has no relationship to `HoldfastRuntimeSession` or `CoreDemoSession` today.
3. Replace `TickDay()` body with `_adapter.TickDay()`, preserving the exact returned summary string format (`"Day {Day}. HP:{Health} Hunger:{Hunger} Thirst:{Thirst} Rad:{Radiation:F0}mSv."`).
4. Replace `ConsumeFood()`, `ConsumeWater()`, `UseAntiRad()`, `Heal()` with adapter delegations. Note `Heal()` has no threshold logic today (`Health = Math.Min(MaxHealth, Health + amount)`) — this is a trivial clamp, not gameplay logic; confirm whether it's worth routing through the adapter at all or leaving as host-local presentation math.
5. Replace `VisitLocation()` radiation calculation with `_adapter.ApplyLocationRadiation(...)`, preserving the non-additive hazmat-overrides-mask priority confirmed in Step 3.
6. Replace `DetermineDeathCause()` with `_adapter.DetermineDeathCause(...)`, preserving the exact check order (Thirst → Hunger → Radiation≥200 → Radiation≥100 → generic) and exact message strings, since these are narrative text shown to the player.
7. Remove all hardcoded constants (MaxHealth, MaxHunger, MaxThirst, RadDamageThreshold, StarvationThreshold, DehydrationThreshold) from the session — they now live in Core. Note `DefaultStartingValue` (trade-related, not survival) stays in the host.

**Verification:**
- `dotnet build Ashfall.csproj` — 0 errors
- Holdfast self-test passes: `godot --headless --path . -- --holdfast-selftest` (verb confirmed present in `src/Host/HostCli.cs`)
- Characterization tests still pass (behavioral equivalence)
- Manually diff `TickDay()`/`VisitLocation()` output strings before and after the change for at least 3 representative game states (healthy, near-death from radiation, near-death from starvation)

**Done when:** `HoldfastRuntimeSession` contains zero gameplay arithmetic — only delegation and UI event firing — and all 4 known construction call sites in `src/Main.cs` still compile and pass their existing self-test assertions.

---

## Step 5 — Remove Dead Constants and Simplify

**Goal:** Clean up `HoldfastRuntimeSession` — remove dead fields, collapse trivial methods, ensure it's a thin host wrapper.

**Implementation:**
1. Remove any private constants that are now in Core (MaxHealth, radiation multipliers, etc.).
2. Remove any private helper methods that computed survival math (e.g. `FindLocation` stays — it's catalog lookup, not survival math; `DetermineDeathCause` moves to Core per Step 4).
3. Ensure the file is under 300 lines. **Note:** the file is currently 372 lines (confirmed via `wc -l`), and it also contains non-survival responsibilities (`TrySave`/`TryReload`/`ArchiveAndFreshStart` persistence orchestration, quest summary formatting) that this batch does not touch. Removing ~100-150 lines of survival math/constants gets the file to roughly 220-270 lines — achievable, but do not cut persistence or quest-summary code to hit the number artificially.
4. Add XML doc comments explaining the session's thin-wrapper role.

**Verification:**
- `dotnet build Ashfall.csproj` — 0 errors
- `wc -l src/Host/HoldfastRuntimeSession.cs` — under 300
- Full verification checklist passes

**Done when:** HoldfastRuntimeSession is a proper thin host session (line count under 300, verified by `wc -l`; zero inline survival-math expressions, verified by code review against the Step 1 classification table), H1 is resolved.

---

## Step 6 — Update AGENTS.md

**Goal:** Mark H1 as RESOLVED in the known issues table.

**Implementation:**
1. Change the H1 row in the "High" issues table (`AGENTS.md`, section `## KNOWN ISSUES`) to: `~~HoldfastRuntimeSession duplicates core survival mechanics~~ — RESOLVED`
2. Add note: "Refactored into `Ashfall.Core.Holdfast.HoldfastSurvivorAdapter`; host session is now a thin wrapper. Adapter owns dedicated `NeedsSystem`/`RadiationSystem` instances (not shared with `SurvivorsHostSession`, which remains a separate, unrelated demo panel)."
3. Do not overwrite or remove H2 (WornGear duplication) or other unrelated rows in the same table — this step touches only the H1 row.

**Verification:**
- AGENTS.md accurately reflects the new state
- No stale references to the old pattern
- Other rows in the Known Issues tables (H2–H12, C1–C6) are unchanged by this edit

**Done when:** H1 is struck through in AGENTS.md with resolution note, and a diff of AGENTS.md shows changes scoped only to the H1 row.

---

## Summary

| Step | Deliverable | Lines Changed |
|------|------------|---------------|
| 1 | Classification document | 0 (analysis) |
| 2 | HoldfastSurvivorAdapter.cs | ~120 new |
| 3 | Characterization tests | ~200 new |
| 4 | Rewired HoldfastRuntimeSession | ~300 modified |
| 5 | Cleanup | ~-200 removed |
| 6 | AGENTS.md update | 2 lines |

**End state:** Invariant 5 violation H1 is eliminated. All Holdfast survival logic lives in Core, testable without Godot, deterministic via ISeededRng.

---

## Review Notes (Corrected)

This file was adversarially reviewed against the actual repository at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War` and the following corrections were made:

1. **False claim — `InventorySystem` in Core.** The original Rationale claimed `NeedsSystem`, `RadiationSystem`, and `InventorySystem` in Core already provide the duplicated logic. There is no `InventorySystem` class in Core; `HoldfastRuntimeSession` uses `Trade.Inventory` (a `HoldfastTradeSession`-scoped object). Removed the false reference; inventory refactor is out of scope for this batch.

2. **Critical false premise — Step 4's adapter wiring.** The plan instructed constructing `HoldfastSurvivorAdapter` "with the existing NeedsSystem/RadiationSystem instances from `SurvivorsHostSession`." Verified via direct read of `src/Main.cs`, `src/Host/CoreDemoSession.cs`, and `src/Host/SurvivorsHostSession.cs`: `HoldfastRuntimeSession` is constructed from a `CoreDemoSession`, which has no `NeedsSystem`/`RadiationSystem` field and no relationship to `SurvivorsHostSession`. Both sessions are wired independently in `src/Main.cs` (~line 2497 vs. ~line 4670/1450). As written, Step 4 was not implementable. Corrected the plan to have the adapter own its own dedicated `NeedsSystem`/`RadiationSystem` instances rather than borrowing unrelated ones.

3. **False API claim — `RadiationSystem.ApplyDose`.** Step 2 assumed a method `RadiationSystem.ApplyDose(...)` exists. Verified via full read of `Assets/Ashfall.Core/Radiation/RadiationSystem.cs`: the only public tick entry point is `Tick(float gameHours)`, driven by delegate-based `ExposureContext`/`applyNeed` callbacks supplied at construction (see the constructor call in `SurvivorsHostSession.cs`). No direct single-shot dose-application method exists. Corrected Step 2 to either route through `Tick` with a constructed `ExposureContext`, or explicitly add a new `ApplyDirectDose` method as new (not reused) Core surface.

4. **Unverified/incomplete characterization data (Step 3).** The original Step 3 named the thresholds and multipliers to characterize but did not state their exact values or interactions. Verified exact values from source: `Radiation` decays 7%/day whenever positive (not mentioned in the original plan at all — an omission that would have caused the characterization tests to miss real behavior); hazmat (0.3x) and gas mask (0.6x) multipliers are **mutually exclusive with hazmat taking priority**, not additive/stacking as an unqualified reading of the original bullet could imply; `DetermineDeathCause()` has a fixed check order (Thirst → Hunger → Radiation≥200 → Radiation≥100 → generic) with exact narrative strings that must be preserved verbatim since they are player-visible text, not just logic.

5. **Missing time-model conflict (new finding, not in original plan).** `HoldfastRuntimeSession.TickDay()` applies discrete per-day deltas (+8 hunger, +10 thirst), while `NeedsSystem.Tick(gameHours)` applies continuous hourly rates (0.8/hr hunger, 1.2/hr thirst — non-equivalent to the day-tick numbers at 24 hours). The original plan asserted the refactor would be behavior-preserving without acknowledging this mismatch. Added an explicit requirement in Step 2 to either recalibrate a `NeedsProfile` to match current output or document the resulting behavior change and confirm it with the user — this is not a detail that can be resolved silently during implementation.

6. **Vague Done-when criteria tightened.** Step 1's "zero ambiguous entries" had no defined method inventory to check against — added the actual confirmed list of 16 methods/properties in the file. Step 5's "under 300 lines" had no baseline — added the confirmed current line count (372, via `wc -l`) so the target is verifiably meaningful rather than an arbitrary aspirational number. Step 6's Done-when now requires the AGENTS.md diff to be scoped only to the H1 row, since the same known-issues table contains other independent entries (H2–H12) that must not be touched by this batch.

7. **Added risk/rollback note (missing in original).** The plan was tagged "Risk: Medium" but had no rollback guidance anywhere in the 6 steps despite touching a live demo/tutorial flow with 4 independent construction call sites in `src/Main.cs`. Added explicit rollback guidance (branch/tag before starting) and a call-out that if adapter-routed output cannot match existing narrative strings and numeric behavior exactly, the deviation must be confirmed with the user rather than merged silently — this is a "different tone, different mechanic" risk the safety guidelines call out (dropping/changing user-visible behavior without confirmation).

8. **Confirmed accurate (no change needed):** `HoldfastRuntimeSession.cs` path, its hardcoded constants (`MaxHealth=100`, `MaxHunger=100`, `MaxThirst=100`), the existence of `NeedsSystem` at `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` and `RadiationSystem` at `Assets/Ashfall.Core/Radiation/RadiationSystem.cs`, the `--holdfast-selftest` CLI verb (confirmed present in `src/Host/HostCli.cs`), and the `ISeededRng` determinism requirement were all verified correct against the current codebase.
