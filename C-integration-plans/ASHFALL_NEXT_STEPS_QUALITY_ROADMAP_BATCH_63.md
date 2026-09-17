# ASHFALL Quality Roadmap — Batch 63

## Theme: Integration Test Suite — Cross-System Day-Advance Smoke Tests

**Priority:** HIGH — no integration tests exist; unit tests cannot catch cross-system interaction bugs
**Risk:** Low — test-only changes, no production code modifications
**Layer:** `Ashfall.Core.Tests/` (xUnit, `net9.0`)
**Engine dependency:** None — all tests run in Core via `dotnet test`

---

## Motivation

The project has ~2120 unit tests (current count via `[Fact]`/`[Theory]` attributes across 195 files in `Ashfall.Core.Tests/` — the commonly-cited "~1941" figure is a stale historical snapshot from `10LOOP_AUDIT_REPORT.md`, documenting a past incremental change, not the current total) covering individual systems in isolation. However, the Godot host's `TickSimDay(int day)` (confirmed real: `src/Main.cs:1643`) orchestrates roughly 20-24 subsystem tick sites sequentially (not "31+" — that figure appears to conflate this method with AGENTS.md's H7 note that `Main.cs` has "31 Setup / 24 Save + SaveAll / 17 Flush methods" across the *entire* ~6.5k-line file, which is a different count than the subsystems ticked inside `TickSimDay` specifically), and no test exercises that pipeline end-to-end. Cross-system failures — where System A's output corrupts System B's input on the same tick — are invisible to unit tests.

Known interaction surfaces at risk:

| Upstream system | Downstream system | Failure mode |
|---|---|---|
| Expedition returning | DutyRoster occupant snapshot | Stale occupant reference after expedition completes |
| Weather radiation | NeedsSystem health drain | Unguarded negative health value |
| Disease outbreak | Medical ward demand | Ward capacity overflow / null affliction |
| Economy price change | Caravan trade value | Stale price in active trade session |
| Crafting completion | Inventory → Expedition loadout | Item duplication or ghost slot |
| Day 180 YearOfAsh activation | Weather pattern shift | Missing null-check on inactive timeline |
| Day 260 Muster escalation | Coalition camp faction events | Faction lookup failure on empty registry |

---

## Step 1 — Design GameSessionTestHarness

### Goal

Define a lightweight test-only coordinator that constructs every Core system with default ports, wires cross-system dependencies, and exposes a single `TickDay(int day)` method that replicates the Godot host's tick order without any engine dependency.

### Implementation

- Create `Ashfall.Core.Tests/Integration/GameSessionTestHarness.cs`. Note: no `Integration/` subfolder currently exists in `Ashfall.Core.Tests/` (confirmed subfolders: `Campaign/`, `Economy/`, `Endgame/`, `Expeditions/`, `Foundry/`, `Medical/`, `Memorial/`, `Radio/`, `Shelter/`, `Survivors/`, `Warlords/`, `World/`; most existing tests sit flat at the project root) — this is a new directory, not an extension of existing test infrastructure.
- Constructor accepts optional overrides for `ISeededRng`, `IClock`, `IJsonSerializer`, `IFileIO`, `ILog`; defaults to the standalone adapter classes in `Assets/Ashfall.Core/HostDefaults.cs` (`SeededRng`, `SimClock`, `SystemTextJsonSerializer`, `FileSystemIO`, `ConsoleLog`/`NullLog` — note there is no single `HostDefaults` class to reference; these are independent classes living in that one file).
- Internally constructs all stateful systems in the same order as `src/Main.cs`'s `TickSimDay(int day)` (`src/Main.cs:1643`), **not** an assumed/generic order. The verified real sequence inside `TickSimDay`, in exact source order, is:
  1. World/Weather (`SetupWorld(); _world.TickDemo(24f);` — `Main.cs:1645-1646`)
  2. Caravans (`SetupCaravans(); _caravans.TickDemo();` — `1648-1649`)
  3. Medical (`SetupMedical(); _medical.TickDemo(24f);` — `1651-1652`)
  4. Expeditions (`SetupExpeditions(); _expeditions.TickDemoHours(24f);` — `1654-1655`)
  5. DutyRoster hatch-return bridge side effect (`1660-1675` — not a standalone system tick, a nested side effect)
  6. Crafting (`SetupCrafting(); _crafting.CompleteAll(24f);` — `1677-1678`)
  7. Maritime (`SetupMaritime(); if (_maritime.Dive.IsActive) _maritime.TickDiveDemo(60f);` — `1680-1682`)
  8. **DeepCoast** (`SetupDeepCoast(); _deepCoast.TickDaily(day, _core.Weather); ...` — `1683-1685`) — **this system was missing from earlier drafts of this batch's tick-order list; it sits between Maritime and Holdfast and must be included in the harness.**
  9. Holdfast (`if (_holdfastRuntime != null && !_holdfastRuntime.IsDead) _holdfastRuntime.TickDay();` — `1687-1688`)
  10. StartingLevel (`SetupStartingLevel(); _startingLevel.TickDay();` — `1690-1691`)
  11. Inventory ration-consumption side effect (`1693-1701` — direct manipulation, not a system tick call)
  12. Verdict (`TickVerdict(day, LivingDwellerCountEstimate());` — `1703`, delegates to a separate method at `Main.cs:3169`)
  13. YearOfAsh, gated (`if (day >= 180 && day <= 360) { SetupYearOfAsh(); _yearOfAsh.TickDay(day); }` — `1707-1710`)
  14. Muster, gated (`if (day >= 260) { SetupMuster(); _muster.Escalate(day); }` — `1712-1715` — note the method is `Escalate`, not a generic `Tick`)
  15. Expansions hub (`SetupExpansions();` then `_expansions.TickGreenhouse(day)`, `_expansions.Ledger.TickDaily(day)`, `_expansions.TickCrossingQuests(day)` — `1717-1720`; this bundles Greenhouse + Ledger + CrossingQuests under one Setup call)
  16. DutyRoster + IceRoad sync (`SetupDutyRoster(); _dutyRoster.TickDay(...); SetupIceRoad(); _dutyRoster.SyncHoldfastToDuty(...); ...` — `1725-1730`)
  17. SilentFoundry (`SetupSilentFoundry(); _silentFoundry.Engine.TickDaily(day); ...` — `1733-1736`)
  18. Disease (`SetupDisease(); _disease.TickDaily(day); ...` — `1741-1743`)
  19. Greenhouse, second/standalone call (`SetupGreenhouse(); _greenhouse.TickDay(day, growLightHours: 6f, ashContaminationRate: 0.04f);` — `1745-1746`) — **this is distinct from step 15's bundled `_expansions.TickGreenhouse(day)` call; the two are separate ticks on what may be separate Greenhouse system instances, and this one runs after Disease, not right after Muster.**
  20. PowerGrid (`TickPowerGrid(day);` — `1748`, delegates elsewhere)
  21. Phase0 (`SetupPhase0(); ...; _phase0.TickDay(day);` — `1752-1761`)
  22. EventTriggers (`SetupEventAdapter(); ...; _hostEventAdapter?.EvaluateTriggers(...);` — `1763-1767`)
  23. Final HUD refresh + save (`UpdateHud(); SaveAll();` — `1769-1770` — not a gameplay subsystem, exclude from the harness's system list but note it exists in the real pipeline)
- `TickDay(int day)` calls each system's tick/advance method in this canonical order. Do not compress the two distinct Greenhouse calls (steps 15 and 19) into one — they are separate call sites in the real code and may need to stay separate in the harness to faithfully reproduce cross-system timing.
- Expose `CaptureAllStates()` returning a dictionary of system name → serialized DTO for save verification.
- Expose `RestoreAllStates(Dictionary<string, string> states)` for round-trip testing.
- Do NOT reference `Godot.*` or `UnityEngine.*`.

### Risk / Rollback

None — this step only creates new test-only files with no production-code changes and no existing callers. If the harness's tick order later needs correction (e.g. if `Main.cs`'s real order changes), only this one file needs updating; no test written against it needs restructuring for anything short of a signature change.

### Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
# Must compile with 0 errors — no engine references
```

### Done when

- `GameSessionTestHarness` compiles cleanly.
- It constructs all Core systems without throwing.
- Its internal tick order matches the 23-step sequence verified above against `src/Main.cs:1643-1770`, including the DeepCoast system (step 8) and both distinct Greenhouse call sites (steps 15 and 19) — not a simplified or reordered approximation.
- It has zero references to `Godot`, `UnityEngine`, or `JsonUtility`.

---

## Step 2 — Implement Minimal GameSessionTestHarness

### Goal

Flesh out the harness with actual system construction, wiring, and tick orchestration so that subsequent tests can instantiate it in one line.

### Implementation

- Wire systems using constructor injection (ports + cross-system references).
- Systems that require catalog data load from `Assets/StreamingAssets/Data/` using `IFileIO` (relative path resolution via a `DataRoot` property, defaulting to `../../Assets/StreamingAssets/Data/` from the test project's output directory).
- Handle optional systems gracefully: if a catalog file is missing, the system initializes in an inert state rather than throwing.
- Add a `GameSessionTestHarness.CreateMinimal()` factory that constructs only the survival-critical subset (Weather, Needs, Radiation, Inventory, Medical, Expeditions, DutyRoster) for fast-running focused tests.
- Add a `GameSessionTestHarness.CreateFull()` factory that includes all ~20-24 systems confirmed in Step 1 (not "31+" — see the corrected Motivation section).
- Seed the `ISeededRng` with a fixed constant (`0xASHFA11` / `0xA5HFA11`) for reproducibility.

### Risk / Rollback

Medium — this is where the actual complexity risk of this batch lives, more than the summary table's blanket "None" for this step suggests. `src/Main.cs` constructs these systems inside a single ~6.5k-line partial class using `SetupXxx()` methods that likely have implicit ordering dependencies and rely on Godot-host-only state (node tree references, `_Ready()` timing) that a pure-C# test harness cannot replicate exactly. Some systems may not construct cleanly outside the Godot host without adaptation (e.g. if a `SetupXxx()` method reads from a `Node` field that only exists in the Godot scene tree). If a given system genuinely cannot be constructed standalone, document that exclusion explicitly in the harness's code comments rather than silently omitting it — a silent gap here would make `CreateFull()` misleadingly named. Rollback: since this is test-only infrastructure with zero production callers, any construction issue is fully contained; worst case, `CreateFull()` ships with a documented subset until the remaining systems are made testable.

### Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~GameSessionTestHarness"
```

### Done when

- `CreateMinimal()` and `CreateFull()` both return a harness instance without throwing, on a machine with no prior Godot import cache (i.e. this must work from a clean `dotnet test` invocation, not only after `godot --headless` has run once and populated `.godot/`).
- A trivial test (`[Fact] HarnessConstructs`) passes for each factory method.
- Determinism: two independently-constructed harnesses seeded identically produce byte-identical `CaptureAllStates()` output immediately after construction (before any `TickDay` call) — verified by comparing the captured dictionaries directly, not just "no throw."
- Every system present in the verified 23-step list from Step 1 is either (a) constructed and reachable in `CreateFull()`, or (b) explicitly excluded with a code comment naming the specific construction blocker. Zero systems may be silently absent — `CreateFull()`'s system count must be checked against the Step 1 list by an assertion in `HarnessConstructs`, not just eyeballed.
- **This step is the highest-complexity item in the entire batch and should not be treated as "just wiring."** Constructing ~20-24 systems that were written assuming a single Godot host process (implicit `SetupXxx()` ordering, possible reliance on other systems already being constructed, catalog files resolved relative to a Godot resource path rather than a test working directory) outside that host, in a test assembly, is a nontrivial adaptation exercise per system — not a mechanical constructor call. Budget this step at 2-3x the effort of any other single step in this batch, and expect it to be the step most likely to reveal that "the harness" needs host-side changes (e.g. extracting a constructor overload that doesn't require a `Node`), which would turn a test-only batch into one that also touches production code — if that happens, stop and flag it back to whoever scoped this batch as HIGH rather than absorbing the scope change silently.

---

## Step 3 — Day-1 Smoke Test (Fresh Game, No Crashes)

### Goal

Verify that a freshly initialized game can advance one day through the entire pipeline without any unhandled exception, null reference, or assertion failure.

### Implementation

- Create `Ashfall.Core.Tests/Integration/DayAdvanceSmokeTests.cs`.
- `[Fact] Day1_FullPipeline_NoCrash`: construct `CreateFull()`, call `TickDay(1)`, assert no exception.
- `[Fact] Day1_MinimalPipeline_NoCrash`: construct `CreateMinimal()`, call `TickDay(1)`, assert no exception.
- `[Fact] Day1_AllSystemsReturnValidState`: after tick, call `CaptureAllStates()`, assert every value is non-null and deserializable.

### Verification

```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~DayAdvanceSmokeTests"
```

### Done when

- All three Day-1 tests pass.
- No system throws during the first tick.
- Captured states are all valid JSON (parseable by `IJsonSerializer`).

---

## Step 4 — Day-30 Regression Test (Key Invariants Hold)

### Goal

Advance 30 days and verify structural invariants that should never be violated regardless of RNG outcomes.

### Implementation

- Add to `DayAdvanceSmokeTests.cs` or create `Ashfall.Core.Tests/Integration/DayAdvanceInvariantTests.cs`.
- `[Fact] Day30_NeedsNeverNegative`: advance 30 days, assert all survivor needs values are >= 0 (hunger, thirst, fatigue, warmth, morale, health, hygiene).
- `[Fact] Day30_RadiationNeverNegative`: assert accumulated radiation dose >= 0 for all survivors.
- `[Fact] Day30_NoOrphanedExpeditions`: assert every active expedition references survivors that still exist in the roster.
- `[Fact] Day30_InventorySlotConsistency`: assert no item appears in two distinct inventory slots simultaneously.
- `[Fact] Day30_DutyRosterOccupantsExist`: assert every duty-roster assigned survivor ID maps to a living survivor.
- `[Fact] Day30_NoCrash`: advance 30 days in a loop, assert no exception at any day.

### Verification

```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~Day30"
```

### Done when

- All Day-30 invariant tests pass with the fixed seed.
- Running with 3 additional seeds (parameterized via `[Theory]`) also passes — proving invariants hold regardless of RNG path.

---

## Step 5 — Day-180 YearOfAsh Activation Test

### Goal

Verify that the Year of Ash timeline system correctly activates at day 180 and triggers the expected weather pattern shift.

### Implementation

- Create `Ashfall.Core.Tests/Integration/TimelineActivationTests.cs`.
- **Correction:** there is no class literally named `YearOfAshSystem` and no `IsActive` property anywhere under `Assets/Ashfall.Core/YearOfAsh/` (confirmed via grep — zero matches). The real architecture splits Year of Ash across `YearOfAshTimelineSystem` (`Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs`, exposing `CurrentDay`, `CurrentPhase`, `ContinuityDecreeActive`, `FinalBroadcastsActive`) plus `YearOfAshDeepFreezeSystem` and `YearOfAshRadonSystem`. The Godot host wrapper is `YearOfAshHostSession` (`src/YearOfAsh/YearOfAshHostSession.cs:15`, constructed via `.Create(...)` at line 92). "Active" state in the real tick path is expressed structurally as the day-range gate `day >= 180 && day <= 360` in `Main.cs:1707` — there is no single boolean flag to assert against. Tests below are corrected to check `YearOfAshTimelineSystem.CurrentDay`/`CurrentPhase` reaching the expected state once the harness ticks into the gated range, not a nonexistent `IsActive` property.
- `[Fact] Day180_YearOfAshActivates`: advance from day 1 to day 180, assert the harness's `YearOfAshTimelineSystem` instance has received its first `TickDay(180)` call (i.e. the gated `if (day >= 180 && day <= 360)` branch fired) and reports `CurrentDay == 180`.
- `[Fact] Day179_YearOfAshNotYetActive`: advance to day 179, assert the YearOfAsh tick was never invoked (the harness's `TickDay` should skip the branch entirely below day 180 — verify via a call-count or last-ticked-day assertion on the system, not an `IsActive` flag).
- `[Fact] Day180_WeatherPatternShifts`: capture weather state at day 179, tick to day 180, assert weather parameters differ (nuclear winter intensification, fallout storm frequency increase, or temperature drop — whichever the system implements; confirm which of these three the real `WeatherSystem` actually exposes before writing the assertion, rather than assuming all three exist).
- `[Fact] Day180_NoNullReferenceOnActivation`: specifically guards against the "missing null-check on inactive timeline" risk noted in the interaction table.
- `[Fact] Day180to210_StabilityAfterActivation`: advance 30 more days post-activation, assert no crash and needs invariants still hold.

### Risk / Rollback

None — test-only additions. If the corrected `CurrentDay`/`CurrentPhase` assertions don't match the real system's actual public surface once inspected directly (this review did not exhaustively enumerate every member of `YearOfAshTimelineSystem`), that's a test-authoring correction at implementation time, not a production risk.

### Verification

```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~TimelineActivation"
```

### Done when

- YearOfAsh's day-180 gate firing is confirmed by test (`CurrentDay == 180` after the harness ticks to day 180, per the corrected API above — not an `IsActive` flag).
- Weather shift is observable in captured state.
- 30 days of post-activation simulation remain stable.

---

## Step 6 — Day-260 Muster Escalation Test

### Goal

Verify that the Muster system escalates at day 260+ and that faction/coalition systems handle the transition without lookup failures.

### Implementation

- Create `Ashfall.Core.Tests/Integration/MusterEscalationTests.cs`.
- The real system is `MusterSystem` (`Assets/Ashfall.Core/Muster/MusterSystem.cs:25`), with state DTO `MusterState` (`escalationDay`, `musterTriggered`, `records`). Actual public members confirmed via existing tests (`Ashfall.Core.Tests/MusterSystemTests.cs:101-107`): `SetEscalationDay(int)`, `EscalationDay` property, `MusterTriggered` property, `SelectApproach(QuestApproach)`, `IsResolved`. The Godot host wrapper is `MusterHostSession` (`src/Host/MusterHostSession.cs:15`), whose `.Escalate(day)` method is what `Main.cs:1714` actually calls (not a generic `.Tick()`), plus `.HydroBarons`/`.ColdCount` sub-state objects. Use `MusterTriggered`/`EscalationDay` in assertions below, matching the real API — not an assumed generic "escalated state" flag.
- `[Fact] Day260_MusterEscalates`: advance to day 260, assert `MusterTriggered == true` and `EscalationDay == 260` (mirroring the existing unit-test pattern at `MusterSystemTests.cs:101-107`, but exercised through the full day-tick pipeline rather than calling `SetEscalationDay` directly).
- `[Fact] Day259_MusterNotEscalated`: advance to day 259, assert `MusterTriggered == false`.
- `[Fact] Day260_FactionRegistryNonEmpty`: after escalation, assert the faction/coalition registry contains at least one entry (guards against the "empty registry lookup failure" risk).
- `[Fact] Day260_CoalitionCampEventsFireable`: assert that the event-trigger evaluation at day 260 can resolve coalition camp events without null references.
- `[Fact] Day260to300_PostEscalationStability`: advance 40 more days, assert no crash, needs invariants hold, and Muster state remains internally consistent.
- `[Theory] MusterEscalation_MultiSeed`: run with 3 seeds to confirm behavior is deterministic given seed.

### Risk / Rollback

None — test-only additions, mirroring an already-passing unit-test pattern (`MusterSystemTests.cs`) at the integration level.

### Verification

```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~MusterEscalation"
```

### Done when

- Muster's day-260 escalation is confirmed by test (`MusterTriggered == true`, `EscalationDay == 260`, per the corrected API above — not vague "activates" language).
- Faction registry is populated at escalation time.
- 40 days of post-escalation simulation are stable across multiple seeds.

---

## Step 7 — Save-at-Every-Day Stress Test (100 Days, Checksum Stability)

### Goal

Advance 100 days, calling `CaptureAllStates()` and computing `SaveChecksum` at each day. Verify that the same seed always produces the same checksum sequence (determinism) and that no checksum is null/empty (completeness).

### Implementation

- Create `Ashfall.Core.Tests/Integration/SaveStressTests.cs`.
- `SaveChecksum` is confirmed real: `Assets/Ashfall.Core/SaveChecksum.cs:36`, a `public static class` (not instantiated) — call it as `SaveChecksum.Compute(...)` or equivalent static method, not via an instance.
- `[Fact] Save100Days_NoChecksumNull`: advance 100 days, capture + checksum each day, assert none are null or empty.
- `[Fact] Save100Days_Deterministic`: run the same 100-day sequence twice with the same seed, assert the checksum sequence is identical element-by-element.
- `[Fact] Save100Days_DifferentSeedDifferentChecksums`: run with two distinct seeds, assert at least one checksum differs (proving the simulation actually diverges).
- `[Fact] Save100Days_RoundTrip`: at day 50, capture state, restore state into a fresh harness, advance to day 100 from the restored state, compare final checksum with the non-interrupted run — must match (save/load fidelity).
- `[Fact] Save100Days_NoExceptions`: the 100-day loop must complete without any unhandled exception.
- Timeout: mark long-running tests with `[Trait("Category", "Integration")]` and a generous xUnit timeout to avoid CI flakiness. **Note: this repository's existing ~2120 tests contain zero uses of `[Trait]` anywhere today (confirmed via repo-wide grep) — this introduces a brand-new convention, not an established one.** If adopting it, either commit to using it consistently for all new Integration-suite tests introduced by this batch (Steps 3-7), or drop the trait and rely solely on the `--filter "FullyQualifiedName~..."` pattern already used throughout this plan's own verification commands, which requires no new convention and is consistent with how the rest of the suite is organized (by namespace/class name, not by trait).

### Risk / Rollback

Low overall (test-only), but flag one real complexity risk: `Save100Days_RoundTrip` requires `RestoreAllStates` (from Step 1) to correctly reconstruct every one of the ~20-24 systems' internal state from `CaptureAllStates()`'s serialized DTOs. AGENTS.md's known-issues list names three classes with allegedly empty `CaptureState`/`RestoreState` bodies (`LocationEvolutionSaveable`, `WildlifeSaveable`, `LandmarkSaveable`, under a supposed `ISaveable` interface) — **this review searched the current codebase exhaustively (`grep` for the class names and for `ISaveable` itself, repo-wide) and found zero matches for any of the three, and zero matches for `ISaveable`.** This claim is copy-pasted verbatim across AGENTS.md, CLAUDE.md, CODEX.md, CRUSH.md, GOOSE.md, MIMOCODE.md, OPENSETUP.md, QWEN.md, VIBE.md, ANTIGRAVITY.md, and `REPO_REVIEW_REPORT.md` (as finding H3) — eleven files repeating the same claim does not make it independently verified; it looks like a single stale finding propagated by search-and-replace across bootstrap docs, never re-checked against the current tree. Either these classes were renamed/refactored/deleted since that finding was written, or the finding was wrong from the start. **Do not treat "LocationEvolutionSaveable/WildlifeSaveable/LandmarkSaveable have empty CaptureState/RestoreState" as a confirmed current fact when implementing this step.** If `Save100Days_RoundTrip` fails, do not assume it traces to these three specific (unverifiable) names — instead, grep the current tree for whatever classes actually implement capture/restore for location-evolution, wildlife, and landmark state (if they exist at all under different names) and inspect their real bodies before attributing the failure. Treat any failure as generically indicating an incomplete `CaptureState`/`RestoreState` pair somewhere in the ~20-24 harnessed systems until the specific offender is identified by reading its current source.

### Verification

```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~SaveStress"
```

### Done when

- All five stress tests pass, run individually and as part of the full suite (no order-dependent state leakage between tests sharing a harness).
- Determinism is confirmed: same seed → same checksum at every one of the 100 days, not just the final day (a bug that only manifests mid-run and self-corrects by day 100 would otherwise pass unnoticed).
- Round-trip fidelity is confirmed: save at day 50, restore into a fresh harness instance (not the same instance — reusing the same instance would not exercise deserialization), continue to day 100 → identical final checksum to the non-interrupted run.
- If `Save100Days_RoundTrip` fails, the failure is triaged to a specific system's `CaptureState`/`RestoreState` (identified by binary-searching which system's captured DTO differs pre/post restore) before this step is considered blocked — "the round trip fails somewhere" is not sufficient triage to close this item.
- No test exceeds 30 seconds on a standard developer machine; if any test does, that is itself a finding to report (likely indicates catalog data is being reloaded from disk on every day-tick rather than cached).

---

## Summary Table

| Step | Name | Tests added | Key assertion | Risk |
|------|------|-------------|---------------|------|
| 1 | Design GameSessionTestHarness | 0 (design only) | Compiles with zero engine refs; tick order matches the verified 23-step `TickSimDay` sequence | None |
| 2 | Implement GameSessionTestHarness | 2 (construction) | Both factories succeed | Medium — see Step 2 rollback note (systems may not construct cleanly outside the Godot host) |
| 3 | Day-1 Smoke Test | 3 | No crash on first tick | None |
| 4 | Day-30 Regression Test | 6+ | Structural invariants hold | None |
| 5 | Day-180 YearOfAsh Timeline Gate | 5 | `CurrentDay` reaches 180, weather shifts | None |
| 6 | Day-260 Muster Escalation | 6+ | `MusterTriggered`/`EscalationDay` update correctly | None |
| 7 | Save-at-Every-Day Stress (100 days) | 5 | Determinism + round-trip fidelity | Low-Medium — see Step 7 rollback note (may surface pre-existing incomplete `CaptureState`/`RestoreState` implementations) |

**Total new tests:** ~27+
**Files created:** 6 (`GameSessionTestHarness.cs`, `DayAdvanceSmokeTests.cs`, `DayAdvanceInvariantTests.cs`, `TimelineActivationTests.cs`, `MusterEscalationTests.cs`, `SaveStressTests.cs`)
**Production code changed:** 0 files — **conditionally.** If Step 2 discovers that any of the ~20-24 systems cannot be constructed standalone without a production-code change (e.g. adding a constructor overload that doesn't require a Godot `Node`), that finding must be escalated rather than absorbed, since it would turn "0 files" into a nonzero, unscoped production change. Do not silently patch production code to make the harness compile without calling this out first.
**Estimated effort:** 3-5 sessions for Steps 1, 3-7 combined; **Step 2 alone should be budgeted as its own 2-4 session effort**, not folded into the 3-5 total, given the standalone-construction risk called out in its Done-when section above. Total realistic range for the batch: 5-9 sessions, not 3-5.

---

## Verification (full suite after all steps)

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # 0 errors
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj    # All tests pass (existing ~2120 + new ~27+)
dotnet build Ashfall.csproj                                  # Godot host unaffected
```

---

## Next Prompt

After completing this batch:

> "Implement Step 1 and Step 2 of Batch 63: create GameSessionTestHarness.cs in Ashfall.Core.Tests/Integration/ with CreateMinimal() and CreateFull() factories, wiring all Core systems in the verified TickSimDay order from src/Main.cs:1643-1770 (including DeepCoast, which sits between Maritime and Holdfast, and both distinct Greenhouse call sites). Verify it compiles and the two construction smoke tests pass."

---

## Review Notes (Corrected)

This plan was adversarially reviewed against the real codebase at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`. The following errors were found and fixed:

1. **`TickSimDay(int day)` name and location confirmed correct** — real method at `src/Main.cs:1643`. No change needed to the core premise.
2. **Tick order was incomplete and had one ordering error.** The claimed order ("Weather → Caravans → Medical → Expeditions → Crafting → Maritime → Holdfast → StartingLevel → Verdict → YearOfAsh → Muster → Greenhouse → DutyRoster → SilentFoundry → Disease → PowerGrid → Phase0 → EventTriggers") omitted **DeepCoast**, a real system (`SetupDeepCoast()`/`_deepCoast.TickDaily`, `Main.cs:1683-1685`) that ticks between Maritime and Holdfast. It also collapsed two structurally distinct Greenhouse call sites into one "Greenhouse" step positioned right after Muster — in the real code, `_expansions.TickGreenhouse(day)` fires right after Muster as part of the bundled Expansions-hub tick (correct position for that one), but a second, separate `_greenhouse.TickDay(...)` call fires much later, after Disease and before PowerGrid — so "Muster → Greenhouse → DutyRoster" is wrong for that second call, which actually comes after DutyRoster, not before. Fixed: Step 1's implementation section now lists the full, verified 23-step sequence with exact `Main.cs` line citations, explicitly calling out DeepCoast's insertion point and the two distinct Greenhouse sites.
3. **"31+ subsystems" was wrong.** The actual number of distinct subsystem tick sites inside `TickSimDay` is ~20-24 depending on how bundled sub-ticks are counted — not 31+. That figure appears to have been transposed from AGENTS.md's H7 note about `Main.cs` having "31 Setup / 24 Save + SaveAll / 17 Flush methods" across the *entire* ~6.5k-line file (a count of all `SetupXxx` construction methods anywhere in `Main.cs`, unrelated to how many systems are actually ticked per day). Fixed throughout: Motivation section and Summary Table no longer cite "31+".
4. **Day 180 (YearOfAsh) and Day 260 (Muster) thresholds were VERIFIED EXACTLY CORRECT**, backed by both production code (`Main.cs:1707`, `Main.cs:1712`) and passing unit tests (`YearOfAshTimelineSystem.cs:34` `StartDay = 180`; `MusterSystemTests.cs:101-107`). No change needed to these two specific numbers — they were the plan's most solid claims.
5. **`YearOfAshSystem.IsActive` does not exist** — there is no class named `YearOfAshSystem` and no `IsActive` property anywhere under `Assets/Ashfall.Core/YearOfAsh/`. Fixed Step 5 to reference the real `YearOfAshTimelineSystem` class and its actual `CurrentDay`/`CurrentPhase` members, and to express "activation" as the day-range gate check it structurally is in the real code, not a boolean flag that doesn't exist.
6. **Muster's real API was under-specified.** The plan said "assert Muster system enters escalated state" without naming the actual class or members. Fixed Step 6 to cite the real `MusterSystem` class, its `MusterTriggered`/`EscalationDay`/`SetEscalationDay` members (confirmed via the existing `MusterSystemTests.cs:101-107`), and the host-side `MusterHostSession.Escalate(day)` method that `Main.cs:1714` actually calls (not a generic `.Tick()`).
7. **`Ashfall.Core.Tests/Integration/` and `GameSessionTestHarness` do not exist yet — confirmed via exhaustive grep, zero matches anywhere in the repo.** This was correctly treated as new work in the original plan, but the plan's phrasing didn't make clear that `Integration/` isn't an existing subfolder pattern being extended — real test subfolders are `Campaign/`, `Economy/`, `Endgame/`, `Expeditions/`, `Foundry/`, `Medical/`, `Memorial/`, `Radio/`, `Shelter/`, `Survivors/`, `Warlords/`, `World/` (most tests actually sit flat at the project root). Fixed Step 1 to note this explicitly.
8. **`HostDefaults` is not a single class** — it's a file (`Assets/Ashfall.Core/HostDefaults.cs`) containing several independent standalone adapter classes (`SeededRng`, `SimClock`, `SystemTextJsonSerializer`, `FileSystemIO`, `ConsoleLog`/`NullLog`, `CatalogLocator`). Fixed Step 2's implementation note to name the actual classes instead of implying one `HostDefaults` type.
9. **Test count ("~1941") was stale.** The actual current count is ~2120 `[Fact]`/`[Theory]` attributes across 195 files — "1941" is a specific historical snapshot from `10LOOP_AUDIT_REPORT.md:250` ("Suite: 1941 → 1949 tests (+8)"), documenting a past incremental change, not the current total. Fixed the Motivation section and full-suite verification comment to reflect the current count.
10. **Missing risk/rollback notes added.** The plan's Summary Table originally rated every step "None" or "Low (timeout tuning)" risk. Step 2 (constructing ~20-24 systems outside their native Godot host, where implicit ordering/node-tree dependencies may block clean standalone construction) and Step 7 (round-trip fidelity testing against systems AGENTS.md itself flags as having incomplete `CaptureState`/`RestoreState` — `LocationEvolutionSaveable`, `WildlifeSaveable`, `LandmarkSaveable`) both carry real, non-trivial risk that the original plan didn't surface. Added explicit Risk/Rollback subsections to Steps 1, 2, and 7, and corrected their Summary Table risk ratings.
11. **Minor: "Files created: 5" undercounted its own listed file names** — the plan named 6 files (`GameSessionTestHarness.cs`, `DayAdvanceSmokeTests.cs`, `DayAdvanceInvariantTests.cs`, `TimelineActivationTests.cs`, `MusterEscalationTests.cs`, `SaveStressTests.cs`) but the header said 5. Fixed to 6.
12. **`SaveChecksum` class location and static nature confirmed correct** (`Assets/Ashfall.Core/SaveChecksum.cs:36`, `public static class`) — added an explicit citation to Step 7's implementation so the static-call convention is unambiguous to whoever implements it.

---

## Review Notes (Corrected) — Second Pass

An independent second adversarial pass re-verified every claim in the plan above (including the first review's own corrections) directly against the live repository at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`, rather than trusting the first pass's citations. Findings:

13. **The first review pass's own verification holds up under re-checking.** Every `src/Main.cs` line citation (1643, 1683-1685, 1690-1691, 1707, 1712-1717, 1749-1752, 1757) was re-read directly from the file and matches exactly, including the tick order, the `day >= 180 && day <= 360` and `day >= 260` gates, and the `MusterHostSession.Escalate(day)` → `Engine.SetEscalationDay(day)` call chain. The `YearOfAshTimelineSystem.StartDay = 180` constant, `CurrentDay`/`CurrentPhase`/`ContinuityDecreeActive`/`FinalBroadcastsActive` properties, and `MusterSystem`'s `MusterTriggered`/`EscalationDay`/`SetEscalationDay` members were all independently re-confirmed by reading the source files directly, as was the exact test count (2120 `[Fact]`/`[Theory]` across 195 files) and the exact `10LOOP_AUDIT_REPORT.md:250` origin of the stale "1941" figure. This is a genuinely well-verified plan; the corrections below are refinements, not reversals.
14. **CRITICAL — the `LocationEvolutionSaveable`/`WildlifeSaveable`/`LandmarkSaveable` claim in Step 7 is not independently verifiable and should not have been presented as settled fact by the first review pass.** These three class names, and the `ISaveable` interface they supposedly implement, produce zero matches anywhere in the current repository (checked via repo-wide grep for each exact name and for `ISaveable`). The claim traces back to `REPO_REVIEW_REPORT.md`'s finding H3 and has been mechanically copy-pasted into at least ten other bootstrap docs (AGENTS.md, CLAUDE.md, CODEX.md, CRUSH.md, GOOSE.md, MIMOCODE.md, OPENSETUP.md, QWEN.md, VIBE.md, ANTIGRAVITY.md) without any of those copies re-checking it against current code. The first review pass cited AGENTS.md as if that made the claim confirmed; it does not — AGENTS.md is a planning/context document, not verified source. Fixed: Step 7's Risk/Rollback section above now says explicitly not to treat this as confirmed, and to triage any round-trip failure by reading the actual current source rather than assuming it's one of these three (possibly nonexistent) classes.
15. **Effort estimate was unweighted given the plan's own Step 2 risk callout.** The original "3-5 sessions" total didn't reflect that Step 2 (standalone construction of ~20-24 Godot-host-coupled systems) is qualitatively harder and riskier than Steps 3-7 (which are mechanical test-writing once the harness exists). Fixed: Step 2's Done-when section now explicitly flags it as the highest-complexity item in the batch with its own effort budget, and the summary now separates Step 2's estimate from the rest.
16. **`[Trait("Category", "Integration")]` in Step 7 has zero precedent in the existing suite.** A repo-wide grep for `[Trait(` across `Ashfall.Core.Tests/` returns no matches — none of the ~2120 existing tests use xUnit traits. Introducing one here is a new convention the plan didn't flag as such. Fixed: Step 7 now notes this and suggests relying on the filter-by-name pattern already used consistently elsewhere in this same plan instead, unless the team explicitly wants to adopt traits going forward.
17. **Done-when criteria in Steps 2 and 7 were too vague to be adversarially falsifiable.** "Constructs all Core systems without throwing" doesn't specify whether that includes every system in the verified 23-step list or a silently-reduced subset; "determinism... produce identical post-construction state" and "round-trip fidelity... identical outcome" don't specify what's compared or how. Fixed: both Done-when sections now require specific, checkable assertions (exact system-count parity with Step 1's list, direct `CaptureAllStates()` dictionary comparison, fresh-instance restoration, per-day checksum comparison rather than final-day-only, explicit failure-triage requirement).
