# ASHFALL Quality Roadmap — Batch 99

## Theme: Contracts & Preconditions Library — Design-by-Contract for Core Systems

| Field | Value |
|-------|-------|
| **Priority** | HIGH |
| **Risk** | Low — additive guard clauses, no behavioral changes to existing logic |
| **Scope** | `Assets/Ashfall.Core/` (engine-agnostic layer) |
| **Estimated systems affected** | ~82 per AGENTS.md's informal count — UNVERIFIED, no canonical system inventory exists; get an exact count via `grep -rl "public class.*System" Assets/Ashfall.Core/` before treating this as a completion metric |
| **Dependency** | None — purely additive |
| **Verification** | `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` + `dotnet test` |

---

## Problem Statement

**⚠️ "82+ systems" and "1941 tests" are both unverified round numbers carried
over from AGENTS.md/prior batches — as of this review the test suite has
2120 `[Fact]`/`[Theory]` cases (`dotnet test --list-tests`), and no one has
enumerated an authoritative "82+ systems" list. Treat both figures as
approximate; re-count before using either as a completion gate (see Step 3
and Step 4 for how this affects "done when" criteria).

Core systems accept parameters without formal validation. A null survivor ID, a negative day count, an empty item list, or a `NaN` radiation dose can propagate silently through the simulation until it corrupts save data or produces an impossible game state. The current defense is the test suite — but tests exercise known paths, not arbitrary call sites. No systematic precondition checking exists:

- System constructors don't validate injected dependencies (`ILog`, `ISeededRng`, `IClock` could be null).
- Public methods don't guard inputs (negative values, empty strings, out-of-range enums).
- State-mutating methods don't verify postconditions (output could violate system invariants).
- Invalid state silently flows between systems until `SaveChecksum` detects corruption — far too late.

Design-by-contract adds runtime guards **at system boundaries**, failing fast with clear messages instead of propagating corruption.

---

## Step 1 — Design `Contracts` Static Class in Core

### Goal

Create a lightweight, zero-dependency static class that provides expressive precondition/postcondition/invariant checks with clear exception messages. **Must compile under the Core project's actual target framework, `net8.0`** (verified in `Ashfall.Core/Ashfall.Core.csproj:3` — not `netstandard2.1` as AGENTS.md's stack table states; AGENTS.md is stale on this point and the plan should not repeat the stale value). No engine references (`UnityEngine.*`/`Godot.*`) regardless of TFM.

### Implementation

- Create `Assets/Ashfall.Core/Contracts/Contracts.cs`
- Static methods: `Requires(bool condition, string message)`, `Ensures(bool condition, string message)`, `Invariant(bool condition, string message)`
- Each throws a distinct exception type: `PreconditionException`, `PostconditionException`, `InvariantException` (all derive from `ContractException : InvalidOperationException`)
- Include overloads with `Func<string>` for expensive message construction (lazy evaluation)
- Include `Requires<TException>(bool, string)` generic overload for callers who need a specific exception type
- All methods are `[MethodImpl(MethodImplOptions.AggressiveInlining)]` when the condition passes (hot path)
- `#if DEBUG` guard option: allow projects to compile out contracts in release builds via `CONTRACTS_ENABLED` define (default: always enabled in Core)

### Verification

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # compiles
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj    # existing tests still pass
```

### Done when

- `Contracts.Requires(false, "msg")` throws `PreconditionException` with the message
- `Contracts.Ensures(false, "msg")` throws `PostconditionException` with the message
- `Contracts.Invariant(false, "msg")` throws `InvariantException` with the message
- Passing `true` to any method is a no-op (no allocation, no side effects)
- Class has zero engine references, compiles under the Core project's real TFM (`net8.0`)

---

## Step 2 — Define Standard Precondition Patterns

### Goal

Build a library of composable, domain-aware validation methods that encode ASHFALL's specific constraints (valid IDs, valid day ranges, valid dose values, etc.).

### Implementation

- Create `Assets/Ashfall.Core/Contracts/Require.cs` — static class with domain preconditions:
  - `Require.NotNull<T>(T value, string paramName)` — throws if null
  - `Require.NotEmpty(string value, string paramName)` — throws if null or whitespace
  - `Require.NotEmpty<T>(IReadOnlyList<T> list, string paramName)` — throws if null or count == 0
  - `Require.Positive(int value, string paramName)` — throws if <= 0
  - `Require.NonNegative(int value, string paramName)` — throws if < 0
  - `Require.NonNegative(float value, string paramName)` — throws if < 0 or NaN
  - `Require.InRange(int value, int min, int max, string paramName)` — inclusive bounds
  - `Require.InRange(float value, float min, float max, string paramName)` — inclusive, rejects NaN/Infinity
  - `Require.ValidId(string id, string expectedPrefix, string paramName)` — checks not-null, not-empty, starts with prefix (e.g., `item_`, `loc_`, `npc_`), snake_case only
  - `Require.ValidEnum<TEnum>(TEnum value, string paramName)` — checks `Enum.IsDefined`
  - `Require.That(bool condition, string paramName, string message)` — general-purpose with param context
- Each method returns the validated value (fluent pattern): `var id = Require.NotEmpty(survivorId, nameof(survivorId));`
- All throw `PreconditionException` with a message format: `"Precondition failed for '{paramName}': {detail}"`

### Verification

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
```

### Done when

- Every `Require.*` method has a unit test confirming it throws on invalid input and passes on valid input
- `Require.ValidId("item_bandage", "item_", "id")` passes; `Require.ValidId("bandage", "item_", "id")` throws
- `Require.NonNegative(float.NaN, "dose")` throws (NaN is not non-negative)
- Fluent return pattern works: `this.id = Require.NotEmpty(id, nameof(id));`

---

## Step 3 — Add Preconditions to Top 10 Most-Called System Methods

### Goal

Retrofit precondition checks to the ten Core system methods with the highest call frequency and the most dangerous failure modes if given invalid input.

### Implementation

**⚠️ CORRECTED — every signature below was verified against the real source in
`Assets/Ashfall.Core/`. The original list was fabricated (wrong parameter
names, wrong types, and at least one method/class that does not exist). See
"Review Notes (Corrected)" at the end of this document for the verification
trail.

Target methods (ordered by impact), using **real, verified signatures**:

1. `NeedsSystem.Tick(SurvivorNeedsState survivor, float gameHours)` (`Assets/Ashfall.Core/Survivors/NeedsSystem.cs:106`) — `Require.NotNull(survivor, nameof(survivor))`, `Require.That(gameHours > 0f, nameof(gameHours), "must be positive")`. Note: there is also a parameterless-per-tick overload `Tick(float gameHours)` (line 100) that iterates all registered survivors — that overload only needs the `gameHours` guard.
2. `RadiationSystem.Tick(float gameHours)` (`Assets/Ashfall.Core/Radiation/RadiationSystem.cs:186`) — there is **no `ApplyDose(string survivorId, float dose)` method**. Dose is applied internally by a private `Expose(...)` call driven by injected delegates (`Func<SurvivorRadState, ExposureContext>`, `Action<SurvivorRadState, float> onExposed`) supplied to the constructor. The guardable public entry points are `Tick(float gameHours)` — `Require.That(gameHours > 0f, ...)` (the method already early-returns on `<= 0`, so this becomes a defensive precondition, not a behavior change) — and `Register(SurvivorRadState survivor)` — `Require.NotNull(survivor, nameof(survivor))`.
3. `MarketSystem.Buy(string itemId, int quantity, int day, string counterparty = "market")` and `MarketSystem.Sell(string itemId, int quantity, int day, string counterparty = "market")` (`Assets/Ashfall.Core/Economy/MarketSystem.cs:186,191`) — there is **no `Trade(string buyerId, string itemId, int quantity)` method**. Both real methods delegate to a shared private `Transact(...)`. Validate: `Require.ValidId(itemId, "item_", nameof(itemId))`, `Require.Positive(quantity, nameof(quantity))`, `Require.NonNegative(day, nameof(day))`, `Require.NotEmpty(counterparty, nameof(counterparty))`.
4. `Inventory.Add(ItemDefinition item, int amount)` (`Assets/Ashfall.Core/Inventory/Inventory.cs:245`) — there is **no `InventorySystem` class**; the class is `Inventory`, and it takes an `ItemDefinition` object, not an `itemId` string. Validate: `Require.NotNull(item, nameof(item))`, `Require.Positive(amount, nameof(amount))`.
5. `Inventory.Remove(ItemDefinition item, int amount)` (`Assets/Ashfall.Core/Inventory/Inventory.cs:288`) — same shape as above, same validation.
6. `WeatherSystem.Tick(float gameHours)` (`Assets/Ashfall.Core/World/WeatherSystem.cs:115`) — **not** `Tick(int day)`; the parameter is a `float` number of elapsed game-hours, matching every other `Tick` in Core. `Require.That(gameHours > 0f, nameof(gameHours), "must be positive")`.
7. `MoraleMarkSystem` (`Assets/Ashfall.Core/DutyRoster/MoraleMarkSystem.cs:27`) or `MoralBranchingSystem` (`Assets/Ashfall.Core/Survivors/MoralBranchingSystem.cs:61`) — there is **no `MoraleSystem` class and no `ApplyModifier(string survivorId, float delta)` method** anywhere in Core. Before writing this step, pick whichever of the two real morale-adjacent systems the team actually intends to harden and re-derive the target method from its real public API (read the file first) — do not guess a method name.
8. `CombatTraumaSystem` (`Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs`) — there is **no `ApplyTrauma(string survivorId, string traumaId)` method**. Real public methods are `RegisterSurvivor(string survivorId)` (line 75), `SetGroundedByCompanion(string survivorId, bool grounded)` (line 84), and `OnCombatSurvived(string survivorId)` (line 98). Validate `survivorId` on all three with `Require.NotEmpty`.
9. **REMOVED** — there is **no `AfflictionSystem` class and no `Inflict` method anywhere in `Assets/Ashfall.Core/`** (verified by workspace-wide symbol search). The only "affliction" surface in the repo is a UI panel (`src/UI/AfflictionsPanel.cs`) and a Unity-legacy `MedicalSystem` (`Assets/_Game/Medical/MedicalSystem.cs`, out of scope per AGENTS.md Invariant 3 — hosts get no new gameplay logic). If affliction contracts are wanted, the actual owning system must be identified first (likely inside `Assets/Ashfall.Core/Medical/` or `Disease/` — confirm by reading those directories) before any precondition is written. Do not implement this item until the real target is confirmed.
10. `FinalWishSystem.RegisterWish(string archetypeId, string wishType)` (`Assets/Ashfall.Core/Survivors/FinalWishSystem.cs:81`) — parameters are `archetypeId, wishType`, **not** `survivorId, wishId` as originally stated. Validate: `Require.NotEmpty(archetypeId, nameof(archetypeId))`, `Require.NotEmpty(wishType, nameof(wishType))`. (`FinalWishSystem` also exposes `DeclareTerminalPrognosis`, `AdvanceWishStep`, `OnPrognosisExpired`, and `Tick(string survivorId, float gameHours, bool isAlive)` — worth validating alongside `RegisterWish` since they share the file and the `survivorId`-shaped contract.)

- Each method gets preconditions as the first lines (before any logic)
- Existing tests must still pass — if a test was passing `null` intentionally, add a separate negative test and fix the call site
- **Before implementing item 7 and item 9 above, re-verify the target method against the live source file — do not carry forward a name from this document without opening the file, since this document has already been shown to contain fabricated signatures once.**

### Verification

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj    # baseline test count: 2120 [Fact]/[Theory] cases as of this review (verified via `dotnet test --list-tests`); re-count at implementation time rather than trusting this or the original "1941+" figure, since the count drifts every batch
godot --headless --path . -- --data-integrity-selftest      # catalog integrity unaffected
```

### Done when

- All 10 methods (using the **corrected** signatures above, re-verified against source at implementation time) reject null/empty/negative/NaN inputs with `PreconditionException`
- No silent corruption possible through these entry points
- All existing tests pass without modification (or with minimal test-setup fixes)
- New negative-case tests cover each precondition, and each test's expected exception message names the actual verified parameter (`item`, `amount`, `archetypeId`, `wishType`, etc.) — not the fabricated names originally listed

### Risk / Rollback

- **Risk:** `RadiationSystem`, `NeedsSystem`, and `WeatherSystem` are driven by injected delegates and hot per-tick loops (see `Tick` bodies above); a precondition thrown mid-tick on one survivor/zone will unwind the whole tick loop instead of skipping just the bad entry, which could turn "one malformed save entry" into "the sim stops ticking for everyone." Prefer per-item validation with a caught/logged skip inside loops (e.g. inside `NeedsSystem.Tick(float gameHours)`'s `for` loop) over a hard `Require.*` throw at the outer tick boundary, unless the team wants tick-level fail-fast.
- **Rollback:** each guard is a small additive diff at the top of one method. Revert per-file via `git revert`/manual removal of the added `Require.*` line; no data migration or save-format change is involved since Step 3 is guard clauses only, not state shape changes.

---

## Step 4 — Add Constructor Preconditions to All Systems

### Goal

Guarantee that no system can be constructed with null dependencies. Fail at construction time, not at first use (which may be many ticks later).

### Implementation

- First, produce an exact, named inventory of Core system classes (do not rely on the "82+" estimate — it is unverified, see Review Notes). A reasonable starting query: `grep -rn "public class.*System" Assets/Ashfall.Core/ --include=*.cs`, manually pruned of DTOs/state classes that happen to have "System" in the name but take no constructor dependencies (e.g. pure data classes). Record the resulting list in the tracking doc from Step 1's sibling artifact or a new `contracts-constructor-sweep.md`.
- **Caution:** several real constructors take optional delegates with `= null` defaults, not required object dependencies — e.g. `RadiationSystem`'s constructor (`Assets/Ashfall.Core/Radiation/RadiationSystem.cs:161`) takes `Func<...>`/`Action<...>` parameters that are all individually optional (each defaults to `null` and is guarded with `!= null` checks at call sites, e.g. `_exposureContext != null ? ... : null`). Adding a blanket `Require.NotNull` to these would break the by-design "delegate not supplied" path and change runtime behavior — this is a **required exception carve-out**, not an oversight to fix. Enumerate which constructor parameters are genuinely optional-by-design before applying `Require.NotNull` mechanically.
- For **required** (non-optional, non-defaulted) dependencies, add `Require.NotNull(log, nameof(log))` (or equivalent):
  - `ILog` — where present
  - `ISeededRng` — systems using randomness
  - `IClock` / `ISimClock` — systems using time
  - `IJsonSerializer` — systems doing serialization
  - `IFileIO` — systems accessing files
  - Domain dependencies passed as required constructor args (not optional delegates)
- Pattern: assign after validation: `_log = Require.NotNull(log, nameof(log));`
- For systems with optional dependencies (the delegate-injection pattern above), use `[MaybeNull]`/leave unguarded and add an explicit code comment stating "intentionally optional — see Ports.cs pattern", rather than skipping silently with no trace

### Verification

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
```

### Done when

- Every system constructor in the sweep inventory (produced in this step, not the unverified "82+" estimate) has been triaged: required deps validated, optional deps explicitly documented as intentional
- Constructing any system with a null *required* dependency throws `PreconditionException`
- Constructors with optional delegate parameters (confirmed pattern in `RadiationSystem` and likely others — re-check each) are explicitly exempted with a comment, and a test confirms the optional-null path still works as before (regression guard against accidentally breaking the "no host callback supplied" case)
- Wiring errors surface immediately at bootstrap, not at runtime tick N
- Test bootstrap helpers (fakes/mocks) still function correctly

### Risk / Rollback

- **Risk:** this step touches every system constructor across the codebase in one pass. Mechanically applying `Require.NotNull` to constructor parameters that are legitimately optional (the delegate-injection pattern seen in `RadiationSystem`) will throw at construction time for hosts that intentionally omit a callback, which is a **behavioral regression**, not a bug fix. This is the highest-blast-radius step in the whole batch — recommend splitting it into smaller per-subsystem-directory PRs (e.g. one PR for `Survivors/`, one for `Economy/`, one for `Radiation/`) rather than one 82-file commit, so a bad guard is easy to bisect and revert.
- **Rollback:** revert per-file; since each constructor guard is additive and isolated to its own file, `git revert` of an individual commit (if split per subsystem as recommended) fully undoes that subsystem's change without affecting others.

---

## Step 5 — Add Postconditions to State-Mutating Methods

### Goal

Verify that state-mutating methods leave the system in a valid state after execution. Catch logic errors that produce impossible values (negative health, radiation above lethal max stored incorrectly, inventory counts below zero).

### Implementation

**⚠️ CORRECTED — re-derive target methods from real class names.** The
original list referenced non-existent classes (`InventorySystem`,
`MoraleSystem`, and implied a `Trade` method on `MarketSystem`). Use the
verified names from Step 3:

- Identify state-mutating methods (methods that modify internal fields and are called externally)
- Add `Contracts.Ensures(...)` checks after mutation logic:
  - `NeedsSystem`: after `Tick(SurvivorNeedsState, float)`, all need values in their documented valid range (read `NeedsProfile`/`SurvivorNeedsState` field comments for the actual bounds — do not assume `[0.0, 1.0]` without checking, since `RadiationDose` elsewhere in Core is documented as a 0..100 scale, not 0..1)
  - `RadiationSystem`: after `Tick(float gameHours)`, `SurvivorRadState.RadiationDose` >= 0 and within whatever the acute/chronic thresholds imply (`AcuteThreshold = 80f`, `ChronicLifetimeThreshold = 400f` are the real constants — verified in source — use these, not an invented `MAX_LETHAL_DOSE`)
  - `Inventory`: after `Add`/`Remove`, slot counts >= 0 (the class is `Inventory`, not `InventorySystem`)
  - `MoraleMarkSystem` and/or `MoralBranchingSystem` (whichever is chosen in Step 3 item 7 after reading its real API): after mutation, morale-adjacent state within valid bounds
  - `WeatherSystem`: after `Tick(float gameHours)`, temperature/fallout intensity within physical bounds
  - `MarketSystem`: after `Buy`/`Sell`, no negative currency/quantity results
- Postconditions run only on `#if DEBUG || CONTRACTS_ENABLED` to avoid perf cost in release
- If a postcondition fires, it indicates a logic bug — the exception message includes the actual invalid value

### Verification

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
godot --headless --path . -- --data-integrity-selftest
```

### Done when

- At least 15 state-mutating methods (drawn from the **verified** class list above, not the fabricated one) have postcondition checks
- A synthetically-injected logic error (e.g., subtracting too much health) triggers `PostconditionException` in tests
- Normal gameplay paths never trigger postconditions (all existing tests pass)
- Postconditions document the valid output range in code, sourced from the actual constants/comments in each file (e.g. `RadiationSystem.AcuteThreshold`/`ChronicLifetimeThreshold`), not assumed generic bounds

### Risk / Rollback

- **Risk:** postcondition checks that assume the wrong valid range (e.g. `[0,1]` for a field that's actually `0..100`) will false-positive on every normal tick, which would make this step look like it "found bugs" when it actually just encoded a wrong assumption. Verify each bound against the real field/constant before writing the check.
- **Rollback:** postconditions are gated behind `#if DEBUG || CONTRACTS_ENABLED`, so disabling `CONTRACTS_ENABLED` (or building Release without `DEBUG`) fully disables this step's checks without touching source; full rollback is deleting the added `Contracts.Ensures` lines.

---

## Step 6 — Add Compile-Time `[ContractRequired]` Attribute

### Goal

Provide a compile-time marker that documents which parameters have contract requirements, enabling future Roslyn analyzers or IDE tooling to flag unchecked calls.

### Implementation

- Create `Assets/Ashfall.Core/Contracts/ContractRequiredAttribute.cs`:
  ```csharp
  [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = true)]
  public sealed class ContractRequiredAttribute : Attribute
  {
      public string Constraint { get; }
      public ContractRequiredAttribute(string constraint) => Constraint = constraint;
  }
  ```
- Usage: `public void ApplyDose([ContractRequired("NotEmpty")] string survivorId, [ContractRequired("NonNegative")] float dose)`
- Create `Assets/Ashfall.Core/Contracts/ContractEnsuresAttribute.cs` for return value postconditions
- Annotate the 10 methods from Step 3 and any additional high-traffic methods
- Document the attribute in a `Contracts/README.md` (internal, not user-facing)
- Future work: Roslyn analyzer that warns if a `[ContractRequired]` parameter is passed without a prior `Require.*` check

### Verification

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
```

### Done when

- Attribute compiles and applies to parameters without runtime cost
- At least 20 parameters across 10+ methods carry `[ContractRequired]` annotation
- Attribute is discoverable via reflection (test confirms `GetCustomAttribute` finds it)
- No behavioral change to any system (attribute is metadata only)

---

## Step 7 — Write Contract Violation Tests

### Goal

Build a dedicated test class that verifies contracts fire correctly on invalid input and that meaningful exceptions (not `NullReferenceException` or silent corruption) result from bad data.

### Implementation

- Create `Ashfall.Core.Tests/ContractViolationTests.cs`
- Test categories (use the **verified** class/method names from Step 3's Review Notes, not the originally-listed fabricated ones):
  - **Null injection**: construct the systems inventoried in Step 4 with null *required* dependencies — assert `PreconditionException`; explicitly test that optional delegate parameters (e.g. `RadiationSystem`'s constructor callbacks) still accept null without throwing, as a regression guard
  - **Invalid IDs**: call methods with malformed IDs (wrong prefix, empty, whitespace, non-snake_case) — assert `PreconditionException`
  - **Out-of-range values**: pass negative day counts, NaN doses, int overflow quantities — assert `PreconditionException`
  - **Empty collections**: pass empty survivor lists, empty item batches — assert `PreconditionException`
  - **Postcondition traps**: use internal test hooks to force a logic error, verify `PostconditionException`
  - **Valid inputs pass**: confirm that all valid-input variants do NOT throw (regression guard)
- Each test has a descriptive name using the real method name, e.g. `RadiationSystem_Tick_NegativeGameHours_ThrowsPreconditionException` (not `NeedsSystem_Tick_NegativeDay_ThrowsPreconditionException`, since `Tick` takes `gameHours`, not `day`)
- Use `Assert.Throws<PreconditionException>()` pattern
- Minimum 50 test cases across all categories

### Verification

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj    # all tests pass including new ones
```

### Done when

- `ContractViolationTests.cs` contains 50+ passing tests
- Every `Require.*` method is exercised in at least one positive and one negative test
- No test relies on `NullReferenceException` — all contract violations produce `PreconditionException` or `PostconditionException`
- Tests run in < 2 seconds total (no I/O, no file system)
- The test class serves as living documentation of each system's input contract

---

## Summary Table

| Step | Deliverable | Files Created/Modified | Tests Added | Risk |
|------|-------------|----------------------|-------------|------|
| 1 | `Contracts` static class + exception types | `Contracts/Contracts.cs`, `Contracts/ContractException.cs` | ~10 | None |
| 2 | `Require` precondition library | `Contracts/Require.cs` | ~25 | None |
| 3 | Preconditions on top 10 methods | 10 system files | ~20 negative-case | Low (additive) |
| 4 | Constructor validation on all 82+ systems | 82+ system files | ~20 null-injection | Low (fail-fast) |
| 5 | Postconditions on state-mutating methods | 15+ system files | ~15 postcondition | Low (debug-only) |
| 6 | `[ContractRequired]` attribute | `Contracts/ContractRequiredAttribute.cs` | ~5 reflection | None |
| 7 | Contract violation test suite | `ContractViolationTests.cs` | 50+ | None |

**Total estimated new tests:** ~145<br>
**Total files created:** 4-5 new files in `Contracts/`<br>
**Total files modified:** ~82 system files (unverified count, see Review Notes — get an exact number before treating "82+" as a completion gate) — additive one-liners at method/constructor entry<br>
**Breaking changes:** Mostly zero, with one caveat — see Risk note below on optional constructor delegates<br>
**Performance impact:** Negligible — one boolean check + branch per method entry; postconditions gated behind `#if`

### Risk callout: "zero behavioral change" is not quite true

The original summary claimed contracts are purely additive guards with zero breaking changes. That holds for genuinely-required parameters, but **not** for the optional delegate-injection pattern confirmed in `RadiationSystem`'s constructor (`Assets/Ashfall.Core/Radiation/RadiationSystem.cs:161`), where every constructor parameter defaults to `null` and callers legitimately omit callbacks. A blanket `Require.NotNull` sweep (Step 4) would turn "host chose not to wire this callback" into a crash — that is a real behavioral change, not a fail-fast improvement. Audit for this pattern in every system before applying constructor guards mechanically.



---

## Review Notes (Corrected)

Adversarial review performed against the real repository at
`/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`. All findings
below were confirmed by reading source files and running `grep`/symbol
search — not inferred from this document's own claims.

### Factual errors found and fixed in place

| Claim in original plan | Verified reality | Where |
|---|---|---|
| `NeedsSystem.Tick(int day)` | Real signatures: `Tick(float gameHours)` and `Tick(SurvivorNeedsState survivor, float gameHours)`. No `int day` parameter exists on any `Tick` overload. | `Assets/Ashfall.Core/Survivors/NeedsSystem.cs:100,106` |
| `RadiationSystem.ApplyDose(string survivorId, float dose)` | **No `ApplyDose` method exists.** Dose application is internal (`Expose(...)`, private) driven by constructor-injected delegates. Public entry points are `Tick(float gameHours)` and `Register(SurvivorRadState survivor)`. | `Assets/Ashfall.Core/Radiation/RadiationSystem.cs:139-373` |
| `MarketSystem.Trade(string buyerId, string itemId, int quantity)` | **No `Trade` method exists.** Real methods: `Buy(string itemId, int quantity, int day, string counterparty = "market")` and `Sell(...)` — both delegate to private `Transact(...)`. | `Assets/Ashfall.Core/Economy/MarketSystem.cs:186,191,196` |
| `InventorySystem.AddItem(string ownerId, string itemId, int count)` / `RemoveItem(...)` | **No `InventorySystem` class exists.** The class is `Inventory`, and its methods are `Add(ItemDefinition item, int amount)` / `Remove(ItemDefinition item, int amount)` — they take an `ItemDefinition` object, not an `itemId` string. | `Assets/Ashfall.Core/Inventory/Inventory.cs:245,288` |
| `WeatherSystem.Tick(int day)` | Real signature: `Tick(float gameHours)`. | `Assets/Ashfall.Core/World/WeatherSystem.cs:115` |
| `MoraleSystem.ApplyModifier(string survivorId, float delta)` | **No `MoraleSystem` class exists.** Closest real classes are `MoraleMarkSystem` (`DutyRoster/MoraleMarkSystem.cs`) and `MoralBranchingSystem` (`Survivors/MoralBranchingSystem.cs`); neither has an `ApplyModifier` method matching this shape. | verified via workspace symbol search, no match |
| `CombatTraumaSystem.ApplyTrauma(string survivorId, string traumaId)` | **No `ApplyTrauma` method exists.** Real public methods: `RegisterSurvivor(string survivorId)`, `SetGroundedByCompanion(string survivorId, bool grounded)`, `OnCombatSurvived(string survivorId)`, `GetDefenseMultiplier(string survivorId)`, `GetHypervigilanceLevel(string survivorId)`. | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs:75,84,98,115,124` |
| `AfflictionSystem.Inflict(string survivorId, string afflictionId)` | **No `AfflictionSystem` class and no `Inflict` method exist anywhere in the repository.** Only affliction-adjacent artifact is a UI panel (`src/UI/AfflictionsPanel.cs`) and a legacy Unity `MedicalSystem`. This item cannot be implemented as written; the real owning system must be identified first. | confirmed via full-workspace symbol search |
| `FinalWishSystem.RegisterWish(string survivorId, string wishId)` | Real signature: `RegisterWish(string archetypeId, string wishType)` — different parameter names and different semantic meaning (archetype-scoped registration, not per-survivor). | `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs:81` |
| "1941 tests" (repeated in Steps 3, 7, and Summary) | As of this review, `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --list-tests` plus a `[Fact]`/`[Theory]` grep both land near **2120** test cases, not 1941. The figure is stale and drifts every batch — treat it as approximate, re-count at execution time rather than hardcoding a number in "Done when" criteria. | measured directly, see command above |
| "82+ Core systems" (Scope, Problem Statement, Step 4) | Unverified. No canonical enumeration of "Core systems" exists in the repo or in this plan. AGENTS.md's own count language ("82+ systems") appears to be the unverified source. Before Step 4 treats "all 82+ systems" as a completion gate, produce an actual named list. | no authoritative source found |
| `netstandard2.1` target for Core (Step 1) | `Assets/Ashfall.Core/Ashfall.Core.csproj` does not exist as a standalone project — the actual buildable project is `Ashfall.Core/Ashfall.Core.csproj` at repo root, which targets **`net8.0`**, not `netstandard2.1`. (AGENTS.md's stack table is stale on this point; the plan should target the real csproj, not repeat AGENTS.md's stale claim.) | `Ashfall.Core/Ashfall.Core.csproj:3` |

### Structural issues found and fixed

- **Underestimated complexity / hidden behavioral risk (Step 4):** the plan
  originally described constructor validation as uniformly safe
  ("fail-fast, no behavior change"). This is false for the confirmed
  delegate-injection pattern in `RadiationSystem`'s constructor, where every
  parameter is optional and defaults to `null` by design. A mechanical
  `Require.NotNull` sweep across "all 82+ systems" would break that pattern
  wherever it recurs. Added an explicit carve-out and recommended splitting
  Step 4 into per-subsystem-directory commits rather than one file-count-82
  commit, to bound blast radius and make a bad guard easy to bisect.
- **Missing risk/rollback:** none of the original 7 steps had a risk or
  rollback subsection. Added them to Steps 3, 4, and 5 (the steps with real
  behavioral risk); Steps 1, 2, 6, and 7 remain low-risk additive/test-only
  work and did not need one added.
- **Unrunnable/circular verification:** Step 3 and Step 7's "Done when"
  criteria referenced test names and method names that don't exist in the
  codebase (e.g. `NeedsSystem_Tick_NegativeDay_ThrowsPreconditionException`
  implies a `day` parameter that isn't real). Corrected to use verified
  method signatures so the eventual test names are accurate.
- **Ordering:** Steps 1→2→3→4→5→6→7 are logically sound (library →
  primitives → apply to hot methods → apply to constructors → postconditions
  → metadata → tests) and were not reordered. The dependency direction is
  correct; only the target identifiers within Step 3–5 were wrong.
- **Scope creep:** none found beyond what's already flagged above — the
  batch stays within `Assets/Ashfall.Core/` as scoped, correctly excludes
  Unity/Godot host changes, and doesn't drift into unrelated systems.

### Unresolved item requiring a decision before implementation

Item 9 (originally "AfflictionSystem.Inflict") has no real target. Before
implementing Step 3 item 9, someone must read `Assets/Ashfall.Core/Medical/`
and `Assets/Ashfall.Core/Disease/` (both exist as directories in Core per the
repo listing) to find the actual affliction-application entry point, or
drop this item from the top-10 list and promote an eleventh real candidate
instead. This document does not resolve that choice — it flags it.
