# ASHFALL Quality Roadmap — Batch 56
## Theme: Dual Clock Consolidation & Simulation Timing Architecture

**Priority:** MEDIUM (Architectural debt — `ISimClock` vs `IClock` confusion, compounded by a **duplicate-name collision** — see Rationale)
**Risk:** HIGH — touches timing across all systems, and the two "SimClock" symbols in this codebase are not the same type, which the original version of this plan did not distinguish.
**Prerequisite:** Batches 49-52 (save round-trip tests provide safety net). **Additionally required (added by this review): a full, verified consumer list — Step 1 below — must be completed and reviewed before Step 3's design is finalized**, because the original plan's design invented property names that do not match the real `ISimClock` API.

---

## Rationale (corrected)

The project has two clock interfaces that create confusion and potential timing bugs — confirmed by reading `Assets/Ashfall.Core/Ports.cs` and `Assets/Ashfall.Core/Clock/ISimClock.cs` directly:

- **`IClock`** (in `Ports.cs`) — Day-based counter. Actual members: `int Day { get; }`, `void AdvanceDays(int days)`, `void SetDay(int day)`. Doc comment: "Simulation calendar. Never DateTime.Now." Used by most systems for day-progression logic.
- **`ISimClock`** (in `Clock/ISimClock.cs`) — Tick-based (60 ticks/hour × 24 hours = 1440 ticks/day). Actual members: `long CurrentTick { get; }`, `int DayIndex { get; }`, `int HourOfDay { get; }`, `void AdvanceTicks(long ticks)`, `void AdvanceHours(int hours)`, `void AdvanceDays(int days)`.

**Corrected finding — a name collision the original plan missed entirely:** there are **two different concrete classes both named `SimClock`**, in two different namespaces:
- `Ashfall.Core.HostDefaults.SimClock : IClock` — day-only (`public int Day { get; private set; }`, constructor `SimClock(int day = 1)`).
- `Ashfall.Core.Clock.SimClock : ISimClock` — tick-based (`public long CurrentTick { get; private set; }`, constructor `SimClock(long initialTick = 0)`).

These are unrelated types that happen to share a bare name. Any migration step that says "replace the `SimClock` instance" (as the original Step 4 did) is ambiguous without a namespace qualifier, and an implementer skimming the code could easily edit the wrong one. This is worth calling out explicitly rather than leaving implicit, because it is exactly the kind of thing that turns a "low risk, interface-compatible" step into a silent behavior change.

**Corrected finding — the plan's proposed `IGameClock` API does not match either real interface's property names.** The original Step 2/3 design proposes `TicksIntoDay` and `TotalTicks` as property names. The real `ISimClock` uses `CurrentTick` (not `TotalTicks`) and has no `TicksIntoDay` property at all — the closest equivalent is `HourOfDay` (hour granularity, not tick-within-day granularity) or a derived `CurrentTick % TicksPerDay` (tick granularity). Any migration script or find/replace based on the original plan's names would not compile against real call sites.

**Corrected finding — `ISimClock` has exactly 3 production consumers, all in the census/radio broadcast domain, not weather/combat/needs.** Grepping the full non-test, non-obj codebase for `ISimClock` finds:
- `Assets/Ashfall.Core/Radio/CensusBroadcastScheduler.cs` — constructor parameter `ISimClock clock`.
- `Assets/Ashfall.Core/Verdict/VerdictCensusBroadcast.cs` — constructor parameter `ISimClock clock`.
- `Assets/Ashfall.Core/Verdict/VerdictRadioSystem.cs` — constructor parameter `ISimClock clock = null`.
- `src/Host/VerdictHostSession.cs` — passes an `ISimClock` through to `VerdictHostSession.Create(...)`.
- `Ashfall.Core.Tests/VerdictSystemTests.cs` — a test-only `StubClock : ISimClock`.

The original plan's "Expected findings" for Step 1 speculated that `WeatherSystem`, `TacticalCombatSystem`, and `NeedsSystem` "likely" need tick-level timing. This was verified false by reading each file: `WeatherSystem.cs` has no `IClock`/`ISimClock` reference at all (it reseeds a fresh RNG from `seed + rollCount` per roll instead of using any clock), `TacticalCombatSystem.cs` has no clock reference (combat uses internal rounds, exactly as the original plan's parenthetical guessed — but that guess wasn't verified until now), and `NeedsSystem.cs` has no clock reference either. **The real risk surface for this migration is much smaller than "all systems" — it is 3 production classes, all census/radio broadcast, plus one host wiring site.** This changes the batch from "touches timing across all systems" to "touches timing across three related broadcast systems plus every day-only `IClock` consumer's wiring," which is still real work but a materially different (and more tractable) scope than the original framing implied.

`IClock` (the day-only interface) is consumed far more broadly — confirmed consumers include `HoldfastSave.cs` (4 call sites), `DutyRoster/DutyRosterSave.cs` (2 call sites), `YearOfAsh/YearOfAshSave.cs` (1 call site), plus test-only implementations (`Ashfall.Core.Tests/YearOfAshTests.cs`'s `ManualClock : IClock`). These are save/capture-state call sites that take `IClock` as a parameter to timestamp saves — not itself evidence of a further, wider "day-only" application graph; a full accounting of every `IClock`-typed field/parameter across `Assets/Ashfall.Core/` and `src/` is still needed and is why Step 1 below is expanded, not shortened.

AGENTS.md's H3 entry (as currently written) says: "not actually a duplicate: `Ashfall.Core/HostDefaults.cs:67` is `IClock` (day-based), `Ashfall.Core/Clock/ISimClock.cs:15` is `ISimClock` (tick-based); both still used." This is consistent with what was verified here — H3 is correctly describing two distinct interfaces with distinct purposes, not a naive duplicate. The **new** thing this review adds is the duplicate-*class*-name (`SimClock`/`SimClock`) risk, which AGENTS.md's H3 note does not mention and which is a separate, sharper hazard for exactly the kind of "wiring-level swap" the original Step 4 proposed.

This batch designs and implements the consolidation, corrected for the above.

---

## Step 1 — Map All Clock Consumers (expanded scope)

**Goal:** Identify every system that consumes `IClock` or `ISimClock` and document how they use it — including which concrete `SimClock` class (there are two) backs each call site today.

**Implementation:**
1. Grep for `IClock` usage (excluding `ISimClock`) — list every constructor/method parameter and every `new SimClock(...)` call site, and for each `new SimClock(...)` site, confirm **which namespace's `SimClock`** is actually in scope (check the file's `using` directives — `Ashfall.Core` vs `Ashfall.Core.Clock`). Do not assume from the argument type alone; a file could have both `using Ashfall.Core;` and `using Ashfall.Core.Clock;` in scope, in which case `new SimClock(...)` would be ambiguous and fail to compile — if that's true anywhere, that call site already needs a qualifier and is worth flagging as a pre-existing latent naming hazard independent of this migration.
2. Grep for `ISimClock` usage — list every consumer. **Confirmed for this review:** `CensusBroadcastScheduler.cs`, `VerdictCensusBroadcast.cs`, `VerdictRadioSystem.cs` (all constructor-injected), `src/Host/VerdictHostSession.cs` (passthrough), and the test-only `StubClock` in `VerdictSystemTests.cs`. Re-run this grep before implementing, in case new consumers were added since this review.
3. Classify each usage:
   - **Day-only:** system only cares about `clock.Day` (confirmed: `HoldfastSave.cs`, `DutyRosterSave.cs`, `YearOfAshSave.cs` — these are save-capture call sites, not gameplay-tick consumers)
   - **Tick-required:** system needs sub-day timing (confirmed: `CensusBroadcastScheduler`, `VerdictCensusBroadcast`, `VerdictRadioSystem` — all broadcast/radio scheduling, not weather/combat/needs as originally guessed)
   - **Both:** none confirmed in this pass — flag if found during a full sweep.
4. Produce a consumer matrix as a table in this file (not just prose), with columns: File | Interface | Concrete type in scope | Day-only/Tick-required | Notes.

**Corrected "expected findings":** the original plan's guesses (`WeatherSystem` needs ticks; `TacticalCombatSystem` uses internal rounds; `NeedsSystem` might tick sub-day) have been checked against the actual source:
- `WeatherSystem.cs`: **no `IClock`/`ISimClock` reference at all.** It reseeds per-roll from `seed + rollCount`, not from a clock.
- `TacticalCombatSystem.cs`: **no `IClock`/`ISimClock` reference.** Uses internal round counters, confirming the original guess, but this was previously unverified speculation, not a finding.
- `NeedsSystem.cs`: **no `IClock`/`ISimClock` reference.**
- The real tick-required set is the 3 broadcast/radio classes named above.

**Verification:**
- Every `IClock`/`ISimClock` consumer identified via grep, with each `new SimClock(...)` site's concrete type disambiguated by namespace.
- No ambiguous cases left unclassified — including the "which `SimClock`" ambiguity, which the original plan did not check for at all.

**Done when:** A consumer matrix table exists in this document (see template row below) covering every confirmed call site, and the day-only vs. tick-required classification is backed by an actual file read, not a guess.

| File | Interface | Concrete type in scope | Classification | Notes |
|---|---|---|---|---|
| `Assets/Ashfall.Core/HoldfastSave.cs` | `IClock` | `Ashfall.Core.HostDefaults.SimClock` (verify per call site) | Day-only | Save-capture parameter, 4 call sites |
| `Assets/Ashfall.Core/DutyRoster/DutyRosterSave.cs` | `IClock` | verify | Day-only | Save-capture parameter, 2 call sites |
| `Assets/Ashfall.Core/YearOfAsh/YearOfAshSave.cs` | `IClock` | verify | Day-only | Save-capture parameter, 1 call site |
| `Assets/Ashfall.Core/Radio/CensusBroadcastScheduler.cs` | `ISimClock` | `Ashfall.Core.Clock.SimClock` (verify) | Tick-required | Constructor-injected |
| `Assets/Ashfall.Core/Verdict/VerdictCensusBroadcast.cs` | `ISimClock` | verify | Tick-required | Constructor-injected |
| `Assets/Ashfall.Core/Verdict/VerdictRadioSystem.cs` | `ISimClock` | verify | Tick-required | Constructor-injected, defaults to `null` |
| `src/Host/VerdictHostSession.cs` | `ISimClock` | verify | Tick-required (passthrough) | Host wiring site |

*(This table is a starting scaffold from this review's grep pass, not a claim of completeness — re-run the greps before treating it as final, per AGENTS.md's own caution about re-verifying before large refactors.)*

---

## Step 2 — Design Unified Clock Interface (corrected)

**Goal:** Design a single `IGameClock` interface that satisfies both day-level and tick-level consumers, using property/method names that are actually compatible with existing call sites — not invented names that require every call site to be rewritten.

**Implementation:**
Design options:

**Option A — Compose (recommended), corrected to match real APIs:**
```csharp
public interface IGameClock : IClock
{
    // Inherited from IClock: int Day { get; }; void AdvanceDays(int days); void SetDay(int day);

    long CurrentTick { get; }      // matches ISimClock.CurrentTick — NOT a new "TotalTicks" name
    int HourOfDay { get; }         // matches ISimClock.HourOfDay
    void AdvanceTicks(long ticks); // matches ISimClock.AdvanceTicks(long), NOT AdvanceTicks(int)
    void AdvanceHours(int hours);  // matches ISimClock.AdvanceHours — original design omitted this entirely
}
```
Rationale for the correction: `ISimClock`'s 3 real consumers (`CensusBroadcastScheduler`, `VerdictCensusBroadcast`, `VerdictRadioSystem`) call `AdvanceTicks(long)`, `AdvanceHours(int)`, `CurrentTick`, `DayIndex`, `HourOfDay` today. A design that renames these to `TicksIntoDay`/`TotalTicks`/`AdvanceTicks(int)` (as the original plan proposed) is **not a drop-in replacement** for those 3 consumers — it is a breaking rename that requires touching every call site in the same commit as the interface change, which contradicts the original plan's own Step 4 claim that day-only consumers need "no code changes" (that claim was only ever true for `IClock` consumers, never for `ISimClock` consumers, and the original plan conflated the two).

Note also: `ISimClock.DayIndex` is computed as `CurrentTick / TicksPerDay`, which is conceptually the same value as `IClock.Day` but is a **derived, read-only** property on `ISimClock`, whereas `IClock.Day` is directly settable via `SetDay`/`AdvanceDays`. A unified `IGameClock : IClock` needs to decide whether `Day` and `DayIndex` are the *same* backing field (recommended — avoids the original dual-clock desync bug this batch exists to fix) or intentionally different concepts that happen to look similar. **This decision must be made explicitly in this design step, not left implicit**, because it's the crux of the "day advance via IClock doesn't advance ISimClock" bug the Rationale describes.

**Option B — Keep separate, add synchronization:**
- Keep both interfaces.
- Add an `IClockSync` that ensures `IClock.Day` and `ISimClock.CurrentTick`/`DayIndex` are always consistent (e.g. a small adapter that wraps one `GameClock` and exposes it as both interfaces, forwarding `IClock.Day` reads to `ISimClock.DayIndex` and `IClock.AdvanceDays` to `ISimClock.AdvanceDays`).
- Given the actual scope found in Step 1 (3 tick consumers, all radio/census, vs. a broader set of day-only save-capture consumers), Option B's blast radius is much smaller than the original plan implied, because the systems that would need synchronizing are a short, specific list rather than "all systems."
- Less disruptive but leaves the architectural debt AGENTS.md's H3 note describes.

**Decision:** Option A is cleaner long-term but requires call-site changes at the 3 `ISimClock` consumers (not zero changes, contrary to the original plan's blanket claim). Document both, recommend A given the now-confirmed small consumer count, implement in phases, and **get explicit user sign-off on the property-name mapping in the corrected interface above** before Step 3, since getting `AdvanceTicks(long)` vs `AdvanceTicks(int)` wrong will not compile against real call sites.

**Verification:**
- Design document (this section) includes the corrected API, the real property-name mapping to existing `ISimClock`/`IClock` members, and an explicit `Day`/`DayIndex` unification decision.
- No code changes yet.

**Done when:** Design reviewed and approved (user confirms Option A or B, and confirms the `Day`/`DayIndex` backing-field decision specifically — this is a new, separate confirmation the original plan never asked for).

---

## Step 3 — Implement GameClock (Core), corrected

**Goal:** Create `Assets/Ashfall.Core/Clock/GameClock.cs` implementing the unified interface from Step 2, with real, call-site-compatible member names and signatures.

**Implementation:**
```csharp
namespace Ashfall.Core.Clock;

public interface IGameClock : IClock
{
    long CurrentTick { get; }
    int HourOfDay { get; }
    void AdvanceTicks(long ticks);
    void AdvanceHours(int hours);
}

public sealed class GameClock : IGameClock
{
    public const long TicksPerHour = 60;
    public const long TicksPerDay = TicksPerHour * 24; // 1440 — matches Clock.SimClock's existing constant

    private long _currentTick;

    public long CurrentTick => _currentTick;
    public int Day => (int)(_currentTick / TicksPerDay) + 1; // +1 to match IClock's existing 1-based Day convention (HostDefaults.SimClock defaults Day to 1, not 0) — confirm this convention explicitly rather than silently picking 0-based or 1-based
    public int HourOfDay => (int)((_currentTick % TicksPerDay) / TicksPerHour);

    public void AdvanceDays(int days)
    {
        if (days < 0) throw new System.ArgumentOutOfRangeException(nameof(days)); // HostDefaults.SimClock.AdvanceDays throws on negative input — preserve that contract
        _currentTick += days * TicksPerDay;
    }

    public void SetDay(int day)
    {
        if (day < 0) throw new System.ArgumentOutOfRangeException(nameof(day)); // HostDefaults.SimClock.SetDay throws on negative input — preserve that contract
        _currentTick = (long)(day - 1) * TicksPerDay + (_currentTick % TicksPerDay); // preserves current tick-of-day rather than silently resetting it — decide explicitly whether SetDay should reset ticks-of-day to 0 (matches Clock.SimClock's SetTick which has no day concept) or preserve them; the original plan's GameClock.SetDay reset _ticksIntoDay = 0, which is a real behavior decision, not a default
    }

    public void AdvanceTicks(long ticks)
    {
        if (ticks < 0) throw new System.ArgumentOutOfRangeException(nameof(ticks)); // Clock.SimClock.AdvanceTicks clamps negative to 0 via Math.Max instead of throwing — this is a real behavioral divergence between the two existing clocks that the unified type must pick one side of, not silently inherit both
        _currentTick += ticks;
    }

    public void AdvanceHours(int hours) => AdvanceTicks(hours * TicksPerHour);
}
```

**Corrections made to the original design:**
- Removed the invented `TicksIntoDay`/`TotalTicks` names; used `CurrentTick`/`HourOfDay` to match the real `ISimClock` surface that 3 production classes already call.
- `AdvanceTicks` takes `long`, not `int`, matching `ISimClock.AdvanceTicks(long ticks)` exactly — a signature mismatch here would silently work for small values and then either overflow-truncate or fail overload resolution depending on how the original `int`-based design was actually wired in; do not narrow the type.
- Called out that `HostDefaults.SimClock` throws `ArgumentOutOfRangeException` on negative `days`/`day`, while `Clock.SimClock.AdvanceTicks` clamps negative input to 0 via `Math.Max(0, ticks)` instead of throwing. **These are different error-handling contracts today.** A unified `GameClock` must pick one behavior and document the change — this is exactly the kind of "hidden complexity underestimated" the original plan's Summary table glossed over by marking this step "Low" risk.
- Made the `Day` 1-based-vs-0-based question explicit, since `HostDefaults.SimClock` defaults to `Day = 1` (constructor `SimClock(int day = 1)`) while a naive `tick / TicksPerDay` calculation is 0-based. Getting this wrong shifts every day number off by one relative to existing saves.

**Verification:**
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — compiles.
- Write unit tests covering: `AdvanceDays` matches `HostDefaults.SimClock`'s exception-on-negative contract; `AdvanceTicks` — decide and test whether it throws or clamps on negative input (this is now an explicit design decision, not an afterthought); tick overflow into day rollover; `SetDay` — decide and test whether it resets `HourOfDay`/tick-of-day to 0 or preserves it; `CurrentTick` monotonic under repeated `AdvanceTicks`; **new test:** a value written via `HostDefaults.SimClock(day: N)` and a value written via `GameClock` advanced to day `N` produce the same `Day` for the same `N` (the 1-based/0-based regression check).

**Done when:** `GameClock` passes all of the above tests (5 from the original plan, plus the day-numbering-convention regression test and the negative-input-contract test this review adds), and implements both `IClock` semantics and tick semantics using names that match existing `ISimClock` call sites.

---

## Step 4 — Migrate Day-Only Consumers (Low Risk) — clarified

**Goal:** Replace the day-only `IClock` wiring with `IGameClock` in the host, being explicit about which concrete `SimClock` class is being replaced.

**Implementation:**
Since `IGameClock : IClock`, any system accepting `IClock` already works with a `GameClock` instance. No code changes needed in the *day-only save-capture systems themselves* (`HoldfastSave.cs`, `DutyRosterSave.cs`, `YearOfAshSave.cs`) — this part of the original claim holds, verified against the actual method signatures (`IClock clock` parameters, not concrete `SimClock` parameters, so callers can substitute freely).

The migration is at the **wiring level** (host):
1. In the Godot host, identify every place that constructs `new Ashfall.Core.HostDefaults.SimClock(...)` (the day-only one — **not** `Ashfall.Core.Clock.SimClock`, which is a different class and is handled in Step 5) and replace it with `new GameClock()`.
2. Pass the same `GameClock` instance to all systems that accept `IClock`.
3. Systems that need ticks can now accept `IGameClock` instead — but per Step 1's findings, that's only the 3 broadcast/radio classes, handled explicitly in Step 5, not "systems in general."
4. **New, explicit check this review adds:** grep the test suite (`Ashfall.Core.Tests/`) for direct construction of `HostDefaults.SimClock` (e.g. `HoldfastSaveTests.cs`, `DutyRosterSaveTests.cs`, `DutyRosterIntegrationTests.cs` all construct `new SimClock(...)` — confirm at implementation time whether these resolve to `HostDefaults.SimClock` or `Clock.SimClock` by checking each test file's `using` directives, since both are plausible given the surrounding code uses `IClock`-typed helpers). Tests that construct the concrete class directly will need to be updated to construct `GameClock` instead, or the concrete `SimClock` class must be kept alongside `GameClock` (at least temporarily) if tests aren't in scope for this batch.

**Verification:**
- `dotnet build Ashfall.csproj` — 0 errors.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — all tests pass, specifically including `HoldfastSaveTests.cs`, `DutyRosterSaveTests.cs`, `DutyRosterIntegrationTests.cs`, `YearOfAshTests.cs` (all confirmed to construct clock instances directly in test setup).
- Day-advance still works identically — add a regression assertion comparing `Day` output before/after the swap for a fixed sequence of `AdvanceDays` calls, not just "tests pass."

**Done when:** A single `GameClock` instance is used in the host wherever `HostDefaults.SimClock` was previously constructed, all day-only systems work unchanged, and every test file confirmed above either compiles unchanged against `GameClock` (since it satisfies `IClock`) or has been explicitly updated — not silently left constructing the old concrete type.

---

## Step 5 — Migrate Tick Consumers (corrected: exactly 3 classes + 1 host wiring site)

**Goal:** Convert the 3 confirmed `ISimClock` consumers (`CensusBroadcastScheduler`, `VerdictCensusBroadcast`, `VerdictRadioSystem`) plus the `VerdictHostSession` wiring site to use `IGameClock.AdvanceTicks`/`CurrentTick`/`HourOfDay` — not a generic "identify all ISimClock consumers," since Step 1 already identified them.

**Implementation:**
1. Change `CensusBroadcastScheduler`'s constructor parameter from `ISimClock clock` to `IGameClock clock`.
2. Change `VerdictCensusBroadcast`'s constructor parameter from `ISimClock clock` to `IGameClock clock`.
3. Change `VerdictRadioSystem`'s constructor parameter from `ISimClock clock = null` to `IGameClock clock = null` — **check what the `null` default does downstream** (does the class construct its own default clock internally when `null` is passed? If so, that internal default also needs updating from `new SimClock()`/`Ashfall.Core.Clock.SimClock` to `new GameClock()`).
4. Update `src/Host/VerdictHostSession.cs`'s `Create(...)` factory to accept/pass `IGameClock` instead of `ISimClock`.
5. Because the corrected `IGameClock` in Step 3 preserves `CurrentTick`, `HourOfDay`, and `AdvanceTicks(long)`/`AdvanceHours(int)` names exactly, the internal logic of these 3 classes should not need further changes beyond the parameter type — this is the actual "type-compatible swap" the original plan claimed for *all* consumers, correctly scoped down to just these 3.
6. Update `Ashfall.Core.Tests/VerdictSystemTests.cs`'s `StubClock : ISimClock` to also implement `IGameClock` (or be replaced by a lightweight `GameClock`/fake, whichever is less test-code churn) so the 3 migrated classes still have a working test double.

**Verification:**
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — compiles.
- `dotnet test` — all tests pass, specifically `VerdictSystemTests.cs`'s census-broadcast tests (the ones exercising `VerdictCensusBroadcast`/`VerdictRadioSystem`/`StubClock`).
- **Corrected from original:** "Weather system still transitions within a day" is **not a valid verification check for this step** — `WeatherSystem` was confirmed in Step 1 to have no clock dependency at all, so it cannot regress from a clock migration and testing it here checks nothing real. Replaced with: census/radio broadcast scheduling still fires at the correct simulated tick/hour (assert against `CensusBroadcastScheduler`'s actual scheduling logic, not an unrelated system).

**Done when:** Zero consumers of the bare `ISimClock` interface remain in `Assets/Ashfall.Core/` and `src/` (re-run the Step 1 grep to confirm), and the 3 migrated classes' existing tests pass against `IGameClock`.

---

## Step 6 — Remove ISimClock and Old SimClock (corrected: two concrete classes to consider, not one)

**Goal:** Delete the now-unused `ISimClock` interface and its concrete `Ashfall.Core.Clock.SimClock` implementation — while explicitly deciding what happens to the *other*, unrelated `Ashfall.Core.HostDefaults.SimClock` class, which the original plan's Step 6 wording ("Remove the old `SimClock : IClock` from `HostDefaults.cs`") does correctly target, but only because it got lucky — the original plan never flagged that this is a distinct class from the one deleted from `Clock/ISimClock.cs` in the same step, which reads as if there's one `SimClock` being removed in two places.

**Implementation:**
1. Delete `Assets/Ashfall.Core/Clock/ISimClock.cs` in full — this removes **both** the `ISimClock` interface **and** its co-located `Ashfall.Core.Clock.SimClock : ISimClock` concrete class (confirmed: both are defined in the same file). Mark as `[Obsolete]` for one release first instead of deleting outright if any external (e.g. save-file-format-adjacent, or Unity-legacy-tree) consumers exist that this review did not check — this review only checked `Assets/Ashfall.Core/` and `src/`, not the full `Assets/_Game/` legacy tree, which AGENTS.md says is read-only but still compiled/referenced in some CI paths (see Batch 55's findings on `ci.yml`'s Unity EditMode/PlayMode tests, which do execute against `Assets/_Game/`).
2. Separately, remove `Ashfall.Core.HostDefaults.SimClock : IClock` from `HostDefaults.cs`, replaced by `GameClock` — this is a different class in a different file and should be called out as its own bullet, not conflated with step 1's deletion.
3. Keep the `IClock` interface in `Ports.cs` (it's the base of `IGameClock`) — unchanged from original plan, still correct.
4. Update AGENTS.md's H3 entry: the current text ("not actually a duplicate... both still used") becomes inaccurate once this migration completes, since after this step there is only one clock type (`GameClock`/`IGameClock`) and `ISimClock` no longer exists. Rewrite H3 to describe the *resolved* state, and also fold in this review's duplicate-class-name finding as a "lesson learned" note so a future contributor doesn't recreate the same two-classes-same-name hazard.

**Verification:**
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — compiles.
- `dotnet test` — all pass.
- `dotnet build Ashfall.csproj` — 0 errors.
- No references to `ISimClock` remain in codebase (re-run Step 1's grep across `Assets/`, `src/`, and `Ashfall.Core.Tests/` — **not** just `Assets/Ashfall.Core/`, since `src/Host/VerdictHostSession.cs` and the test suite both reference it too, and the original plan's verification list didn't say where to grep).
- **New check this review adds:** confirm `Assets/_Game/` (the read-only legacy Unity tree) does not itself reference `ISimClock` or either `SimClock` class before deleting — if it does, deleting the Core-side types breaks a tree AGENTS.md says should remain buildable-as-is until explicitly migrated, and Unity CI (per Batch 55's findings) is still actively compiling and testing that tree today.

**Done when:** Single clock type (`GameClock`) exists, zero confusion, zero remaining `ISimClock`/duplicate-`SimClock` references across the *entire* repo (not just `Ashfall.Core/`), AGENTS.md H3 updated to reflect the resolved state, and the `Assets/_Game/` legacy-tree check above has been explicitly performed and recorded.

---

## Step 7 — Add Clock Save/Load Contract

**Goal:** Ensure `GameClock` state round-trips through saves (day + tick-of-day + total ticks), consistent with the project's `CaptureState`/`RestoreState` convention (AGENTS.md's SAVE/LOAD section).

**Implementation:**
1. Add to `GameClock`:
   ```csharp
   public GameClockState CaptureState() => new() { CurrentTick = _currentTick };
   public void RestoreState(GameClockState state) { _currentTick = state.CurrentTick; }
   ```
   Corrected from the original plan's `{ Day, TicksIntoDay, TotalTicks }` three-field DTO: since `Day` and `HourOfDay` are both derived from the single `_currentTick` backing field in the corrected Step 3 design, persisting all three would be redundant and — worse — would let them drift out of sync on a hand-edited or partially-migrated save (exactly the "day advance doesn't advance sub-day state" bug class this whole batch exists to fix). Persist the single source of truth (`CurrentTick`) only.
2. Per AGENTS.md's SaveChecksum convention, `GameClockState` should be a plain `[Serializable]` DTO with no engine references, consistent with every other `CaptureState`/`RestoreState` pair in the codebase (e.g. `HoldfastSave.cs`'s pattern).
3. Wire into host save/load (include in the world or top-level save envelope) — identify the specific envelope this belongs in by checking how `HostDefaults.SimClock`'s `Day` is currently persisted today (via `IClock clock` parameters into `HoldfastSaveCodec`/`DutyRosterSaveCodec`/etc. — confirmed those codecs take `IClock` directly rather than a separate clock-state DTO), and decide whether `GameClock`'s state should be captured once at the top level and referenced everywhere, or captured redundantly per-codec the way `IClock.Day` currently is. **This is a real design decision the original plan skipped** — it just said "include in the world or top-level save envelope" as if that were a single obvious choice.
4. Add a round-trip test, and — since Step 6 deletes the old `Clock.SimClock`/`ISimClock` types — add a **migration test**: an old save produced before this batch (with only a bare `Day` int persisted via the old `HostDefaults.SimClock`-backed codecs) must still load correctly into the new `GameClock`-backed system, per AGENTS.md's versioned-migration convention (V1→V2→V3, "throw on future, migrate on past"). The original plan's Step 7 verification only checked a fresh save/load round-trip and never checked backward compatibility with saves written before this migration — that gap is exactly the kind of cross-host/cross-version save-compatibility risk Invariant 3 in AGENTS.md is about.

**Verification:**
- `dotnet test` — clock round-trip test passes (fresh `GameClock`, capture, mutate, restore, compare).
- Save game, advance 3 days + 720 ticks, load, verify exact `CurrentTick` (and derived `Day`/`HourOfDay`) match.
- **New:** load a save file written by the pre-migration `HostDefaults.SimClock`-backed system (a bare day integer) and confirm it still loads into `GameClock` with the correct `Day` and `HourOfDay == 0` (since pre-migration saves have no sub-day tick data) — this is the backward-compatibility case the original plan omitted.

**Done when:** Clock state persists across save/load including sub-day position, **and** a pre-migration save still loads correctly (not just a same-version round-trip).

---

## Rollback Plan (new — the original plan had none)

Given this batch is explicitly marked HIGH RISK, each step should be its own commit (per AGENTS.md's "one system per task" rule), specifically so any step can be reverted independently:

- **Steps 1–2 (analysis/design):** no code changes; nothing to roll back beyond discarding the design doc.
- **Step 3 (`GameClock` addition):** purely additive — a new file, no existing code deleted or changed. Revert = delete the new file. Zero risk to existing systems since nothing references it yet.
- **Step 4 (day-only host wiring swap):** revert by reverting the wiring commit; `HostDefaults.SimClock` should **not** be deleted until Step 6, so this step can be reverted without needing to resurrect a deleted class.
- **Step 5 (tick consumer migration):** revert by reverting the 3-class constructor-signature change commit; keep `Ashfall.Core.Clock.SimClock`/`ISimClock` undeleted until Step 6 for exactly this reason — the original plan's Step ordering already does this correctly (delete last), which is good and should be preserved.
- **Step 6 (deletion):** the highest-risk, least-reversible step — once `ISimClock.cs` and `HostDefaults.SimClock` are deleted, reverting requires `git revert` of that specific commit (not a `git reset --hard`, which is unnecessarily destructive for a single-commit undo). **Do not delete in the same commit as Step 5's migration** — keep them separate so a Step-6 revert doesn't also undo Step 5's working migration.
- **Step 7 (save contract):** additive DTO + codec wiring; if the backward-compatibility migration test (added by this review) fails, this step blocks Step 6 from being considered complete, since Step 6's deletion is only safe once old saves are proven loadable under the new system.

**Circuit breaker:** if, during Step 5, any of the 3 migrated classes' existing tests reveal behavior that depends on `ISimClock`'s specific semantics differing from `IClock` (e.g. `AdvanceTicks`'s clamp-vs-throw difference found in Step 3), stop and re-run Step 2's design decision rather than patching around it — that is precisely the scenario the original plan's "Important caveat" warned about, and this review's Step 3 correction (documenting the clamp-vs-throw divergence explicitly) is meant to surface that risk before Step 5, not after.

---

## Summary

| Step | Deliverable | Risk Level | Corrected? |
|------|------------|-----------|---|
| 1 | Consumer matrix — corrected to 3 tick consumers (census/radio only), not weather/combat/needs | None (analysis) | Yes — scope corrected |
| 2 | Unified design — corrected property names, explicit Day/DayIndex unification decision | None (document) | Yes — API corrected |
| 3 | GameClock implementation — corrected names/signatures, explicit error-contract decision | Low, but with real decisions to make (not purely mechanical) | Yes |
| 4 | Day-only consumer migration — clarified which concrete `SimClock` is being replaced | Low (interface-compatible for the save-capture systems themselves) | Yes — disambiguated |
| 5 | Tick consumer migration — scoped to the exact 3 classes + 1 host site | Medium | Yes — scope corrected, weather-system check removed as invalid |
| 6 | ISimClock + both concrete SimClock classes' removal — explicit two-class handling, legacy-tree check | Medium-High | Yes — duplicate-class hazard flagged |
| 7 | Clock save/load contract — single source of truth, backward-compat migration test added | Low-Medium | Yes — backward-compat gap closed |

**End state:** Single `IGameClock`/`GameClock` type serves all timing needs, using property/method names compatible with the 3 real existing `ISimClock` consumers rather than an invented API. No dual-clock confusion, and no duplicate-class-name confusion either (the original risk AGENTS.md's H3 note didn't mention). Sub-day timing is first-class. Clock state round-trips, including for pre-migration saves. `ISimClock` and both `SimClock` classes are gone (pending the legacy-tree check in Step 6). AGENTS.md H3 resolved with an accurate description, not just deleted.

**Important caveat (retained and sharpened):** This batch is HIGH RISK. The original caveat about "any system subtly depending on `ISimClock` behavior differing from `IClock`" is confirmed to be real and specific, not hypothetical: `ISimClock.AdvanceTicks` clamps negative input via `Math.Max(0, ticks)` while `IClock.AdvanceDays`/`SetDay` throw `ArgumentOutOfRangeException` on negative input. A unified type must pick one contract, and whichever is chosen is a behavior change for the losing side's existing callers. Do not treat Step 3 as "just write the interface" — the error-handling contract decision is the actual risk this batch's HIGH designation is about, and it was undocumented in the original version of this plan.

---

## Review Notes (Corrected)

This plan was adversarially reviewed against the real codebase at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`, specifically `Assets/Ashfall.Core/Clock/ISimClock.cs`, `Assets/Ashfall.Core/Ports.cs`, and `Assets/Ashfall.Core/HostDefaults.cs`, plus a full-repo grep for every `IClock`/`ISimClock`/`SimClock` reference. The following were wrong or missing in the original version and have been fixed above:

1. **Missed duplicate-class-name collision — the most important finding.** The original plan refers to "the `SimClock` (IClock) instance" in Step 4 as if there is exactly one `SimClock` class. There are **two**, in two different namespaces: `Ashfall.Core.HostDefaults.SimClock : IClock` (day-only) and `Ashfall.Core.Clock.SimClock : ISimClock` (tick-based). This is confirmed by reading both source files directly. An implementer following the original plan's wording could plausibly edit or delete the wrong one, especially in Step 6 where both get removed in the same step without the plan ever distinguishing them as separate classes in separate files. **Fix:** every step (4, 5, 6) now explicitly names which `SimClock` (with full namespace) is in scope, and Step 6 treats the two deletions as two separate bullets in two separate files with two separate risk profiles.

2. **Invented API property names that don't match the real `ISimClock` surface.** The original Step 2/3 design proposed `IGameClock` with `TicksIntoDay` and `TotalTicks`. Reading `ISimClock.cs` shows the real members are `CurrentTick`, `DayIndex`, `HourOfDay`, `AdvanceTicks(long)`, `AdvanceHours(int)`, `AdvanceDays(int)` — no `TicksIntoDay`/`TotalTicks` exist anywhere in the current codebase. A migration built on invented names would not be a "type-compatible swap" for the 3 real `ISimClock` consumers; it would require rewriting every call site's property access in the same commit as the interface change — directly contradicting the original plan's own claim that the migration is low-risk/no-code-change for existing consumers. **Fix:** Step 2/3's design now uses the real member names and signatures (including `long` vs `int` for `AdvanceTicks`, which the original plan also got wrong).

3. **Wrong consumer classification — speculation presented as findings.** The original Step 1's "Expected findings" guessed that `WeatherSystem`, `TacticalCombatSystem`, and `NeedsSystem` "likely" need `ISimClock`/tick-level timing. Reading all three files directly (`WeatherSystem.cs`, `TacticalCombatSystem.cs`, `NeedsSystem.cs`) shows **none of them reference `IClock` or `ISimClock` at all.** The actual 3 `ISimClock` consumers are `CensusBroadcastScheduler.cs`, `VerdictCensusBroadcast.cs`, and `VerdictRadioSystem.cs` — all census/radio-broadcast scheduling, an entirely different domain than the original guess. **Fix:** Step 1 now states the confirmed consumer list with file paths, and Step 5's verification section removes the original's "weather system still transitions within a day" check, which tested a system with zero relationship to this migration.

4. **Understated risk: no rollback plan existed at all**, despite the batch being marked HIGH RISK in its own header. **Fix:** added a dedicated Rollback Plan section with a per-step revert strategy and an explicit "circuit breaker" condition tied to the real clamp-vs-throw behavioral divergence found in Step 3.

5. **Missing edge case: backward compatibility with pre-migration saves.** The original Step 7 verification only checked a same-version save/load round-trip. Given Step 6 deletes the old clock types entirely, any save file written before this migration (with only a bare `Day` integer, via the old `HostDefaults.SimClock`) needs an explicit load path — this is exactly what AGENTS.md's SAVE/LOAD section's versioned-migration convention ("throw on future, migrate on past") exists for, and the original plan never mentioned it. **Fix:** Step 7 adds a required backward-compatibility migration test, and Step 6's Done-when criterion is now gated on that test passing, not just on a fresh round-trip.

6. **Missing edge case: the legacy `Assets/_Game/` Unity tree.** The original plan never checked whether the Unity-legacy tree (which Batch 55's review confirms is still actively built/tested by the current, Unity-based `ci.yml`) references any of the types being deleted in Step 6. **Fix:** Step 6 adds an explicit check of `Assets/_Game/` before deletion, since deleting a still-referenced type there would break a tree that AGENTS.md says must remain buildable until explicitly migrated.

7. **Undocumented behavioral divergence between the two clocks' error handling.** `ISimClock.AdvanceTicks` clamps negative input to 0 (`Math.Max(0, ticks)`); `IClock.AdvanceDays`/`SetDay` (via `HostDefaults.SimClock`) throw `ArgumentOutOfRangeException` on negative input. The original plan's `GameClock` implementation snippet didn't handle negative input at all for `AdvanceTicks`, silently inheriting neither contract. **Fix:** Step 3 now calls this out explicitly as a design decision that must be made (and documents the current behavior of both real classes so the decision is informed), rather than leaving it as an implicit gap.

8. **Loose Done-when criteria tightened.** Several original Done-when statements ("Consumer matrix complete, design decision possible," "Zero consumers of `ISimClock` remain") were restated with concrete, checkable conditions — e.g. Step 6's Done-when now requires the grep to be re-run across the *whole* repo (not just `Ashfall.Core/`, since `src/Host/VerdictHostSession.cs` and the test suite also reference `ISimClock`, which the original plan's own verification commands would have missed since they only mention `dotnet build`/`dotnet test`, not a repo-wide grep).

9. **Step ordering was already sound and is preserved.** One thing the original plan got right: deleting `ISimClock`/`SimClock` last (Step 6), after all consumers are migrated (Steps 4–5), rather than deleting first and fixing compile errors after. This ordering is kept unchanged in the corrected plan and called out explicitly in the Rollback Plan as a reason Step 5 and Step 6 must remain separate commits.
