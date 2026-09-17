# ASHFALL Quality Roadmap — Batch 101

## Theme: Immutable State DTOs — Prevent Accidental State Mutation

| Field | Value |
|-------|-------|
| **Priority** | MEDIUM-HIGH |
| **Risk** | Medium — changes DTO definitions for an audit-confirmed subset of systems, requires serialization compatibility, and requires a `SaveChecksum` fix (Step 2a) before any record migration is safe |
| **Scope** | `Assets/Ashfall.Core/` (all `CaptureState`/`RestoreState` DTOs) |
| **Estimated DTOs affected** | Up to 82+ systems exist in principle, but the real number needing *action* (not just audit) is unknown until Step 1 completes — do not treat 82+ as the migration count |
| **Dependency** | Should follow Batch 100 (nullable) — records benefit from nullable annotations. **Also now depends on new Step 2a** (SaveChecksum property support), which gates Step 4 |
| **Verification** | `dotnet test` + save round-trip tests + `SaveChecksum` stability (verified against a real regression baseline, not just "no test failures") |

---

## Problem Statement

**Corrected (see Review Notes):** Core systems implement the `CaptureState`/`RestoreState` pattern with `[Serializable]` DTO classes. These DTOs universally use **bare public mutable fields** (e.g. `public float Hunger;`), not `{ get; set; }` properties. This is deliberate and documented in `SaveChecksum.cs`'s class remarks: "Scope: public instance fields only, matching the save DTOs, which are deliberately all plain public fields." `SystemTextJsonSerializer` sets `IncludeFields = true` specifically because of this. The mutation hazards below are real, but the mechanism is public fields, not setters — this distinction matters because it changes which migration approach is safe (see Step 2/4).

This creates three distinct mutation hazards:

### Hazard 1: Post-Capture Mutation

```csharp
var state = system.CaptureState();
state.Health = -999;  // Mutates the object system returned
// If system kept a reference to the same object, system internals are now corrupted
```

### Hazard 2: Post-Restore External Mutation

```csharp
var state = LoadFromDisk();
system.RestoreState(state);
state.Health = -999;  // If system stored the reference (not a copy), system is corrupted
```

### Hazard 3: Cross-System Leakage

```csharp
var state = systemA.CaptureState();
systemB.IngestExternalState(state);
state.Survivors.Clear();  // Both systemA's captured snapshot AND systemB's imported data are gone
```

**Corrected (see Review Notes):** Defensive copying is NOT absent — it is the dominant, well-established pattern in the sampled code. A spot-check of 15+ `CaptureState`/`RestoreState` pairs (`DoseLedgerSystem`, `BrineWaterSystem`, `CensusClaimSystem`, `CohortSystem`, `LedgerDebtSystem`, `HoldfastQuestSystem`, `IceRoadSystem`, `District8DeepCoastSystem`, `WeatherSystem`, `CrossingArbitrationSystem`, and others) shows `CaptureState` consistently building a **fresh copy** ("Fresh copy, ordinal-ordered: never return the live state to the envelope") and `RestoreState` consistently copying data in with an explicit comment ("Deep-copy: the deserialized DTO must not become the live state"). The real gap, if any, is **partial/unverified coverage across all 82+ systems**, not a systemic absence of defensive copying. Step 1's audit must record actual per-system findings instead of assuming every system is unsafe by default — do not budget Steps 5/6 as if 82 systems need copy logic added; budget them as an audit-and-patch-the-exceptions pass.

The save stores with checksummed envelopes: `grep -rl "public static class .*SaveStore" src/` returns **30 files** as of this writing (verify current count before citing a number in status reports — this document previously cited both "27" and "22" in different sections, which was internally inconsistent and is corrected here).

**BLOCKING RISK — verify before Step 2 is approved:** `SaveChecksum.Compute` (`Assets/Ashfall.Core/SaveChecksum.cs`) hashes over `type.GetFields(BindingFlags.Public | BindingFlags.Instance)` — public **fields** only, never properties. Read directly from `SaveChecksum.cs` (`WriteObject`, line ~150) and confirmed by the class's own remarks: "Scope: public instance fields only, matching the save DTOs, which are deliberately all plain public fields." A C# `record` with `{ get; init; }` auto-properties is backed by compiler-generated **private** fields.
- Consequence: `SaveChecksum.Compute(recordInstance)` produces the *same* hash (`"{}"` canonical form) **regardless of the record's actual data**. Any DTO migrated to a record silently loses all integrity-hash coverage — no exception, no test failure signature beyond a checksum that stops varying. This would defeat the entire `SaveStoreChecksumSweepTests` contract (mutated-state-changes-hash) for any migrated type.
- This is not a hypothetical edge case — it is the default outcome of the record pattern proposed in Step 2/Option A/Step 4, and it silently passes compilation and JSON round-trip tests while breaking integrity hashing. **Do not proceed past Step 2 until `SaveChecksum` itself is changed to also reflect over public properties (or the record migration is dropped in favor of Option B).**
- This blocking risk directly contradicts Step 2's own "Done when" line ("`SaveChecksum` confirmed to work with record-based DTOs (reflection still finds properties)") and Step 4's "Done when" line ("`SaveChecksum` produces identical hashes for identical data (no regression)"). Both are corrected below — records do NOT work with `SaveChecksum` today, full stop, until `SaveChecksum` itself is changed to also walk properties. This document treated that fix as optional; it is not. It is a prerequisite for every subsequent step and must be its own step (see corrected Step 2/4 below), not a footnote.
- Secondary risk: `Ashfall.Core.csproj` has `<Nullable>enable</Nullable>` and does **not** suppress `CS8618` (unlike `Ashfall.csproj` and `Ashfall.Core.Tests.csproj`, which both suppress it). A `record` with a non-nullable reference-type `init` property that has no default value (e.g. `public IReadOnlyList<string> ActiveEffects { get; init; }` without `= Array.Empty<string>()`) will emit `CS8618` ("non-nullable property must contain a non-null value when exiting constructor") in `Ashfall.Core` even though the same pattern would be silently allowed in the Godot host or in tests. Every migrated record property must have an explicit default, or the build breaks in Core specifically — this was previously buried in an example's code comment ("defensive copy into immutable snapshot") but never called out as a real, csproj-verified build risk. It is now.

---

## Step 1 — Audit Current DTO Patterns

### Goal

Catalog every DTO used in `CaptureState`/`RestoreState` across all 82+ systems. Classify each by its current mutability pattern and serialization requirements.

### Implementation

- Scan `Assets/Ashfall.Core/` for all classes/structs that are:
  - Returned by a `CaptureState()` method
  - Accepted by a `RestoreState(T state)` method
  - Nested within another state DTO (e.g., `SurvivorState` containing `List<AfflictionState>`)
- For each DTO, record:
  - File location and line number
  - Property count and types
  - Whether members are bare `public` mutable fields (the actual, near-universal pattern — e.g. `public float Hunger;`), `{ get; set; }` properties (rare; found mainly on non-state catalog/config DTOs like `HoldfastFactionEntry`, not on `CaptureState`/`RestoreState` DTOs), `{ get; init; }` (init-only, does not exist anywhere in `Ashfall.Core` today), or `{ get; }` (readonly)
  - Whether the DTO contains reference-type collections (`List<T>`, `Dictionary<K,V>`, arrays)
  - Whether the DTO has a parameterless constructor (required by some serializers)
  - Current `[Serializable]` attribute presence
  - Whether `SaveChecksum` covers this DTO (hash sensitivity)
- Produce a ranked list: DTOs with mutable collections are highest risk (clearing a list affects all holders of the reference)
- Document findings in a working spreadsheet or markdown table

### Verification

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # baseline: still compiles
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj    # baseline: still passes
```

### Done when

- Complete catalog of all state DTOs (expected: 82+ primary, 150+ including nested)
- Each DTO classified: fully mutable / partially mutable / already immutable
- Risk ranking established (mutable collections > mutable value properties > already safe)
- Serialization requirements documented per DTO (which serializer, which format)
- Clear picture of the migration surface area

---

## Step 2 — Design Immutable DTO Strategy

### Goal

Choose the right immutability mechanism for ASHFALL's constraints: must work with `System.Text.Json` deserialization, `SaveChecksum` reflection-based hashing, `net8.0` (Core — `Ashfall.Core.csproj` confirms `TargetFramework=net8.0`; the top-of-document "Dependency" line elsewhere in this batch is correct, but readers should not assume `netstandard2.1` from the AGENTS.md stack table without checking the actual csproj) and `net8.0` (Godot host), and the existing save format (backward-compatible deserialization of old saves).

### Implementation

- Evaluate three candidate approaches:

  **Option A: C# Records with `init` setters**
  ```csharp
  public record NeedsState
  {
      public float Hunger { get; init; }
      public float Thirst { get; init; }
      public IReadOnlyList<string> ActiveEffects { get; init; } = Array.Empty<string>();
  }
  ```
  - Pros: concise, built-in equality, `with` expression for copies, `init` blocks mutation after construction
  - Cons: requires C# 9+ (`Ashfall.Core.csproj` pins `<LangVersion>9.0</LangVersion>`, which supports `init` — do not use C# 10+ record features like primary-constructor positional records without bumping `LangVersion` first), `System.Text.Json` supports `init` setters natively since .NET 5
  - **SaveChecksum compatibility: BROKEN today, not "unaffected."** `SaveChecksum` reflects over `GetFields`, not properties. `init`-only auto-properties compile to private backing fields that this call cannot see. Every record migrated under Option A silently produces a checksum that never changes, regardless of the record's actual content, until `SaveChecksum` is updated to also walk public properties (tracked as new Step 2a below). This is corrected from the original "SaveChecksum compatibility: records work with reflection (properties are still properties)" line, which was checked against the actual `SaveChecksum.cs` source and found to be false.

  **Option B: Deep-Clone on Boundary**
  ```csharp
  public SystemState CaptureState() => DeepClone(_internalState);
  public void RestoreState(SystemState state) { _internalState = DeepClone(state); }
  ```
  - Pros: no DTO changes needed, works with existing mutable DTOs
  - Cons: allocation cost, clone logic must be maintained, doesn't prevent mutation of the clone itself
  - SaveChecksum compatibility: unchanged (same objects, same fields, hashing behavior is unaffected) — this option's compatibility claim is accurate as originally written.

  **Option C: Frozen Wrapper**
  ```csharp
  public FrozenState<NeedsState> CaptureState() => FrozenState.Freeze(_state);
  // FrozenState<T>.Value is readonly; any attempt to mutate through reflection throws
  ```
  - Pros: explicit freeze semantics, runtime enforcement
  - Cons: complex, reflection-based enforcement is fragile, wrapping changes the API

- **Recommended: Option B (deep-clone) as the default, immediate action** — it requires zero `SaveChecksum` changes and matches the defensive-copy pattern already present in most of the codebase (see Problem Statement corrections above). **Option A (records) is a longer-term goal gated on Step 2a** (fixing `SaveChecksum` to walk properties) and should not be scheduled for Step 4 until Step 2a is done and verified with its own passing test. The original recommendation ("Option A primary, Option B transitional") is inverted here because Option A is currently unsafe to ship — it would compile, pass JSON round-trip tests, and still silently break integrity checking.
- Design the `init`-based pattern (for later, once Step 2a lands):
  - Value properties: `{ get; init; }`
  - Collection properties: `IReadOnlyList<T>` with `init` (backed by array or immutable list)
  - Nested DTOs: also records (recursive immutability)
  - Parameterless constructor: still available (required for deserialization) — `init` allows setting during construction/deserialization but not after
- Design the deep-clone utility for transitional DTOs:
  - `StateCloner.Clone<T>(T state)` — serialize to JSON, deserialize back (guaranteed deep copy, reuses existing `IJsonSerializer`)
  - Performance: acceptable for save/load (not per-frame)

### Verification

```
# Prototype with one simple DTO
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
```

</content>

---

## Step 2a — Fix `SaveChecksum` to Cover Public Properties (NEW — gating step, added by this review)

### Goal

Close the blocking risk identified above: make `SaveChecksum.Compute` see data on `{ get; init; }` auto-properties, so record-based DTOs (Option A) are safe to ship without silently losing integrity coverage. This step did not exist in the original plan — the plan treated the fix as an optional footnote inside Step 2/4's "Done when" lists, which is not viable given the reflection facts confirmed in this review.

### Implementation

- Modify `Assets/Ashfall.Core/SaveChecksum.cs`, `WriteObject`: in addition to `type.GetFields(BindingFlags.Public | BindingFlags.Instance)`, also enumerate `type.GetProperties(BindingFlags.Public | BindingFlags.Instance)` where `CanRead == true` and the property has no index parameters (skip indexers).
- Decide and document the ordering rule when a type has both fields and properties (existing DTOs have only fields; new records will have only properties) — simplest correct rule: merge both member lists into one ordinal-sorted-by-name sequence so field/property mixing on one type still produces a stable, swap-detecting hash.
- Preserve every existing normalization (`NonSerializedAttribute` skip, null-string-as-empty, null-collection-as-empty, `ChecksumFieldName` root skip) for the new property path.
- Add a regression test that pins the *existing* field-only hash values for at least 3 real DTOs (e.g. `MarketState`, `DoseLedgerSystemState`, `WorldWeatherState`) unchanged before/after this change — this step must NOT alter hashes for any DTO that has no properties, or every on-disk save becomes "corrupt" on next load.
- Add a new test proving a record with only `init` properties (no public fields) now produces a hash that changes when its data changes.

### Verification

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj    # includes new SaveChecksum property-coverage tests
```

### Done when

- `SaveChecksum.Compute` produces a data-sensitive hash for a property-only record (no public fields).
- `SaveChecksum.Compute` produces byte-identical output to the pre-change implementation for every existing field-only DTO (regression-pinned, not just "no test failures" — actual hash string equality against a captured baseline).
- `SaveStoreChecksumSweepTests` (the existing 12-test suite: clean round-trip / mutated-state-changes-hash / null-checksum-rejected, 3 per store) still passes unmodified.

### Risk / Rollback

- **Risk:** Every one of the 30 save stores with checksummed envelopes depends on `SaveChecksum` producing a *stable* hash across app versions for existing saves. A bug in the merged field+property enumeration (e.g. wrong sort order, double-counting a backing field and its property) changes hashes for saves that have nothing to do with this batch, and every existing player save on disk fails `SaveStoreChecksumSweepTests`'s "checksum field missing (corrupt save)" style guard on next load.
- **Rollback:** This step touches exactly one file (`SaveChecksum.cs`). Revert to the field-only implementation via `git revert` of this step's commit; no DTOs need to change back because Step 4 (record migration) is gated on this step completing first, so no record DTO should exist yet when this step is reverted.
- **Mitigation:** Land the regression-pinned baseline test *before* the implementation change (test-first), so a broken implementation fails CI instead of silently shipping a hash drift.

---

### Done when

- Strategy document written with chosen approach (**Option B deep-clone as the immediate default; Option A records deferred until Step 2a lands**)
- Prototype DTO converted and verified with `System.Text.Json` round-trip
- ~~`SaveChecksum` confirmed to work with record-based DTOs (reflection still finds properties)~~ — **removed: this is false as stated.** Replaced with: "`SaveChecksum`'s field-only limitation for record DTOs is documented and Step 2a is scheduled before any record migration proceeds."
- Backward compatibility confirmed (old JSON format deserializes into new record)
- `StateCloner.Clone<T>` utility implemented and tested

---

## Step 3 — Create `ImmutableState<T>` Wrapper and `StateCloner`

### Goal

Build the infrastructure that enables immutable state: a deep-clone utility for transitional DTOs and a compile-time-friendly pattern for new DTOs.

### Implementation

- Create `Assets/Ashfall.Core/State/StateCloner.cs`:
  ```csharp
  public static class StateCloner
  {
      /// <summary>
      /// Deep-clones a state DTO by round-tripping through JSON.
      /// Use at CaptureState/RestoreState boundaries to prevent shared references.
      /// </summary>
      public static T Clone<T>(T state, IJsonSerializer serializer)
      {
          var json = serializer.Serialize(state);
          return serializer.Deserialize<T>(json)
              ?? throw new InvalidOperationException($"Clone failed: deserialization returned null for {typeof(T).Name}");
      }
  }
  ```
- Create `Assets/Ashfall.Core/State/ImmutableList.cs`:
  ```csharp
  /// <summary>
  /// Lightweight read-only list wrapper for state DTOs.
  /// Prevents Add/Remove/Clear after construction.
  /// </summary>
  public sealed class ImmutableList<T> : IReadOnlyList<T>
  {
      private readonly T[] _items;
      public ImmutableList(IEnumerable<T> source) => _items = source.ToArray();
      public T this[int index] => _items[index];
      public int Count => _items.Length;
      // IEnumerable implementation...
  }
  ```
- Create `Assets/Ashfall.Core/State/StateRecord.cs` — base record with common patterns:
  ```csharp
  /// <summary>
  /// Marker interface for state DTOs that are immutable after construction.
  /// Records implementing this must use init-only properties and IReadOnlyList collections.
  /// </summary>
  public interface IStateRecord { }
  ```
- Add JSON converter support if needed (ensure `System.Text.Json` can deserialize `IReadOnlyList<T>` properties backed by arrays)
- Add `StateCloner` tests: clone produces equal but distinct objects (mutating clone doesn't affect original)

### Verification

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
```

### Done when

- `StateCloner.Clone<T>` works for all existing DTO patterns (flat, nested, with collections)
- `ImmutableList<T>` prevents mutation (no `Add`, `Remove`, `Clear`, or index setter)
- Cloned objects pass `SaveChecksum` equivalence (same hash as original)
- Cloned objects are reference-distinct (mutating clone doesn't affect source)
- Infrastructure is zero-dependency (no new NuGet packages)

---

## Step 4 — Migrate 10 Critical DTOs to Records

### Goal

Convert the 10 highest-risk DTOs (those covered by `SaveChecksum` and involved in the most cross-system data flow) from mutable classes to C# records with `init`-only setters.

**Corrected (see Review Notes):** The DTO names below were verified against the real codebase and corrected. Three of the original ten names (`NeedsState`, `RadiationState`, `WeatherState`) do not exist anywhere in `Assets/Ashfall.Core/` and would fail `search_symbols`/compile immediately. The real names, and their real shape, are noted per item. `MoraleState`, `AfflictionState`, and `ShelterState` (original items 5, 6, 8) also do not exist as class names in the codebase — no such types were found by search. Before this step is scheduled, re-run Step 1's audit and replace these three placeholder names with real, `grep`-verified DTO names, or drop them from the "critical 10" if no clear top-level system DTO matches the described role.

### Implementation

Target DTOs (ordered by save-integrity risk):

1. **`SurvivorNeedsState`** (not `NeedsState`) — per-survivor hunger/thirst/fatigue/warmth values, defined in `Assets/Ashfall.Core/Survivors/NeedsSystem.cs`. **Verify before migrating:** this class is held in an in-memory `List<SurvivorNeedsState>` inside `NeedsSystem` and does not appear to have its own top-level `CaptureState()`/`RestoreState()` pair in the pattern this batch assumes — confirm the actual save-boundary owner (likely a parent system) before treating it as a Step-4 record target.
2. **`SurvivorRadState`** (not `RadiationState`) — per-survivor dose/exposure values, defined in `Assets/Ashfall.Core/Radiation/RadiationSystem.cs`. Same caveat as #1: confirm the real `CaptureState`/`RestoreState` owner before migrating; `DoseLedgerSystemState` (in `DoseLedgerSystem.cs`) is a distinct, separate top-level DTO that already has explicit deep-copy `CaptureState`/`RestoreState` and may be the more relevant "radiation dose" migration target.
3. **`InventoryState`** — item lists (mutable `List<ItemEntry>` → `IReadOnlyList<ItemEntry>`). Verify the exact class name and file (`grep -r "class InventoryState" Assets/Ashfall.Core/`) before starting; not directly confirmed during this review.
4. **`WorldWeatherState`** (not `WeatherState`) — current conditions, defined in `Assets/Ashfall.Core/World/WeatherSystem.cs`. Verified: `WeatherSystem.CaptureState()` already returns a freshly constructed `WorldWeatherState` (defensive copy already present) — the record migration here is about preventing mutation of the *returned* object, not about adding missing copy logic that doesn't exist.
5. **`MoraleState`** — **not found in codebase.** No class of this name exists under `Assets/Ashfall.Core/`. Re-identify the real morale-modifier DTO (candidates seen during this review: `MoraleMarkSystemState` in `Assets/Ashfall.Core/DutyRoster/MoraleMarkSystem.cs`) before scheduling this item.
6. **`AfflictionState`** — **not found in codebase.** No class of this name exists. Re-identify the real medical/affliction DTO (candidates: `DiseaseSystemState`, `DiseaseEntryState`, `DiseaseInfectionState` in `Assets/Ashfall.Core/Disease/DiseaseSystem.cs`) before scheduling this item.
7. **`MarketState`** — price records, verified: `Assets/Ashfall.Core/Economy/MarketSystem.cs`, a plain `[Serializable] class` with bare public mutable fields (`public List<DemandEntry> demand = new List<DemandEntry>();`), matching the plan's general claim about DTO shape. `MarketSystem.CaptureState()`/`RestoreState()` already build/consume a fresh copy — confirmed by reading the source.
8. **`ShelterState`** — **not found in codebase.** No class of this name exists. Re-identify the real shelter/room DTO (candidates: `ShelterEncounterSystemState` in `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs`) before scheduling this item.
9. **`CombatState`** — encounter snapshots, verified: `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs`, `CaptureState()` already builds `var copy = new CombatState { ... }` (defensive copy already present).
10. **`ExpeditionState`** — **shape mismatch.** `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` defines `ExpeditionState` for a *single* expedition, but the system's actual save boundary is `public List<ExpeditionState> CaptureState()` / `public void RestoreState(List<ExpeditionState> saved)` — a **list of** `ExpeditionState`, not a single instance. The record-migration example code in this step (`new NeedsState { ... }` single-object pattern) does not directly apply; migrating this DTO means making `ExpeditionState` itself an immutable record while the list-returning wrapper method stays as-is (return `IReadOnlyList<ExpeditionState>` instead of `List<ExpeditionState>`).

For each DTO:
- Convert from `class` to `record`:
  ```csharp
  // Before:
  [Serializable]
  public class NeedsState
  {
      public float Hunger { get; set; }
      public List<string> ActiveEffects { get; set; } = new();
  }

  // After:
  public record NeedsState : IStateRecord
  {
      public float Hunger { get; init; }
      public IReadOnlyList<string> ActiveEffects { get; init; } = Array.Empty<string>();
  }
  ```
- Update `CaptureState()` to construct the record:
  ```csharp
  public NeedsState CaptureState() => new NeedsState
  {
      Hunger = _hunger,
      ActiveEffects = _activeEffects.ToArray() // defensive copy into immutable snapshot
  };
  ```
- Update `RestoreState(NeedsState state)` to read from the record:
  ```csharp
  public void RestoreState(NeedsState state)
  {
      _hunger = state.Hunger;
      _activeEffects = new List<string>(state.ActiveEffects); // own copy, not shared reference
  }
  ```
- Verify JSON round-trip: `Serialize(state) → Deserialize<NeedsState>` produces equivalent record
- Verify `SaveChecksum` stability: same data → same hash before and after migration

### Verification

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj    # all save round-trip tests pass
godot --headless --path . -- --data-integrity-selftest
```

### Done when

- **Precondition (unskippable): Step 2a is complete and its regression-pinned tests pass** — do not start this step otherwise, or the migrated DTOs ship with silently broken integrity hashes.
- The 3 target DTOs whose real names/shapes were confirmed during this review (`WorldWeatherState`, `MarketState`, `CombatState`) are migrated to records with `init`-only setters — treat the remaining 7 slots as "to be filled after Step 1's corrected audit identifies real replacement DTOs for the 3 non-existent names (`MoraleState`, `AfflictionState`, `ShelterState`) and confirms the real save-boundary owner for the 2 per-survivor DTOs (`SurvivorNeedsState`, `SurvivorRadState`)," not as a fixed list to migrate blindly.
- All collection properties use `IReadOnlyList<T>` (not `List<T>`)
- `CaptureState` returns a snapshot that cannot be mutated externally
- `RestoreState` copies data from the record (doesn't hold the record reference)
- All existing save round-trip tests pass
- `SaveChecksum.Compute` on a migrated record produces a hash that changes when the record's data changes (verified by an explicit test per migrated DTO — a passing `dotnet test` run alone does not prove this; a test must assert two different instances hash differently)
- JSON deserialization of old save format works (backward compatible) — verified by loading at least one real pre-migration save fixture, not only round-tripping freshly-serialized data

---

## Step 5 — Add Defensive Copying in `CaptureState`

### Goal

**Corrected (see Review Notes):** The original framing assumed all 82+ systems lack defensive copying in `CaptureState`. The spot-check in this review's Problem Statement correction found the *opposite* for every system sampled — copying is already present and commented as deliberate. Reframe this step's actual goal: **run Step 1's audit to completion, then add defensive copying only to the systems the audit flags as missing it** — not a blanket pass over 72+ files. Budget and file counts below are corrected to reflect an audit-and-patch model, not a rewrite-everything model.

### Implementation

- Identify all systems where `CaptureState` returns a reference that shares mutable state with the system's internals:
  ```csharp
  // DANGEROUS: caller mutating the returned list mutates system internals
  public SystemState CaptureState() => new SystemState { Items = _items };

  // SAFE: caller gets their own copy
  public SystemState CaptureState() => new SystemState { Items = new List<ItemEntry>(_items) };
  ```
- Apply `StateCloner.Clone` for complex DTOs with deep nesting:
  ```csharp
  public ComplexState CaptureState()
  {
      var state = new ComplexState
      {
          Entries = _entries.Select(e => new EntryState
          {
              Id = e.Id,
              SubItems = e.SubItems.ToList() // copy nested collection
          }).ToList()
      };
      return state; // all references are owned by the returned object, not shared
  }
  ```
- For DTOs with only value-type properties (int, float, bool, string, enum), no copy is needed — value types are copied on assignment
- For DTOs with reference-type collections: `.ToList()`, `.ToArray()`, or `new List<T>(source)`
- For DTOs with nested DTOs: recursive copy or `StateCloner.Clone`
- Document each system's copy strategy in a code comment:
  ```csharp
  // CaptureState: defensive copy — collections are cloned, caller cannot mutate internals
  ```

### Verification

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
```

### Done when

- Every `CaptureState` method that Step 1's audit flagged as unsafe (not "every" method unconditionally — most already comply) now returns a fully detached snapshot
- Mutating the returned DTO in a test does NOT affect the system's behavior — verified with an explicit test per patched system, not by assertion in this document
- No shared mutable references between system internals and returned state, for the patched systems
- All save round-trip tests still pass (correctness preserved)
- Performance validated: measure actual wall-clock time for a full `SaveAll` pass before and after this step's changes (e.g. via a `Stopwatch`-wrapped test or the existing save benchmark, if one exists — this document does not currently know of one and that gap should be flagged, not silently assumed away) and confirm no regression beyond a stated threshold (e.g. +10ms), rather than the unquantified "copy overhead is acceptable" claim in the original Summary Table

---

## Step 6 — Add Defensive Copying in `RestoreState`

### Goal

**Corrected (see Review Notes):** Same correction as Step 5 — most `RestoreState` implementations sampled during this review already copy from the input DTO and explicitly comment "Deep-copy: the deserialized DTO must not become the live state." Scope this step to audit-confirmed exceptions, not a blanket 82-system rewrite.

For all systems flagged by Step 1's audit, ensure `RestoreState` copies data from the input DTO into internal fields — the system does not hold a reference to the caller's DTO. After `RestoreState` returns, the caller can freely modify or discard the DTO without affecting the system.

### Implementation

- Identify all systems where `RestoreState` stores a reference to the passed DTO:
  ```csharp
  // DANGEROUS: if caller mutates 'state' after this call, system internals change
  public void RestoreState(SystemState state) { _state = state; }

  // SAFE: system owns its own copy
  public void RestoreState(SystemState state)
  {
      _items = new List<ItemEntry>(state.Items);
      _health = state.Health;
  }
  ```
- Apply defensive copying patterns:
  - Value-type properties: direct assignment (safe by default)
  - String properties: direct assignment (strings are immutable in C#)
  - Collection properties: `new List<T>(state.Collection)` or `.ToArray()`
  - Nested DTO properties: `new NestedState { ... }` (copy fields) or `StateCloner.Clone`
  - Dictionary properties: `new Dictionary<K,V>(state.Dict)`
- For record-based DTOs (migrated in Step 4): `RestoreState` already copies values out of the record — verify this pattern holds
- Add an internal test helper:
  ```csharp
  // Test pattern: restore, then mutate the input DTO, assert system unaffected
  var state = new SystemState { Health = 100 };
  system.RestoreState(state);
  state.Health = -999;  // mutate the input
  Assert.Equal(100, system.GetHealth());  // system is unaffected
  ```

### Verification

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
godot --headless --path . -- --data-integrity-selftest
```

### Done when

- Every `RestoreState` method flagged by the audit as non-compliant now copies data from the DTO (does not store the DTO reference)
- Mutating the input DTO after `RestoreState` does NOT affect the system, for every patched system
- All save round-trip tests pass
- **Corrected:** all **30** save stores with checksummed envelopes (`grep -rl "public static class .*SaveStore" src/`; the original "22" figure is stale and was inconsistent with this same document's "27" elsewhere) produce correct checksums
- Pattern documented in code: `// RestoreState: copies from DTO, does not hold reference`

---

## Step 7 — Write Mutation-Safety Tests

### Goal

Create a dedicated test class that proves the immutability contract: captured state cannot be corrupted by external mutation, and restored state cannot be corrupted by input mutation.

**Corrected (see Review Notes):** The example code below uses `NeedsSystem`/`NeedsState`, which do not exist by that name (real names: `NeedsSystem` class exists in `Assets/Ashfall.Core/Survivors/NeedsSystem.cs`, but the per-survivor DTO is `SurvivorNeedsState`, and — per the Step 4 correction — it is not confirmed to have a standalone top-level `CaptureState()`/`RestoreState()` pair matching this example's shape). Replace `NeedsSystem`/`NeedsState` below with a real, audit-confirmed system before implementing — e.g. `MarketSystem`/`MarketState`, which this review directly verified has the assumed `CaptureState()`/`RestoreState()` shape.

### Implementation

- Create `Ashfall.Core.Tests/StateMutationSafetyTests.cs`
- Test categories:

  **Category A: Capture Isolation (returned DTO is detached)**
  ```csharp
  [Fact]
  public void NeedsSystem_CaptureState_MutatingReturnedDto_DoesNotAffectSystem()
  {
      var system = CreateNeedsSystem(hunger: 0.5f);
      var captured = system.CaptureState();
      captured.Hunger = 0.0f;  // mutate the captured DTO
      var recaptured = system.CaptureState();
      Assert.Equal(0.5f, recaptured.Hunger);  // system unaffected
  }
  ```

  **Category B: Restore Isolation (input DTO is not held)**
  ```csharp
  [Fact]
  public void NeedsSystem_RestoreState_MutatingInputDto_DoesNotAffectSystem()
  {
      var state = new NeedsState { Hunger = 0.8f };
      var system = CreateNeedsSystem();
      system.RestoreState(state);
      state.Hunger = 0.0f;  // mutate the input after restore
      var captured = system.CaptureState();
      Assert.Equal(0.8f, captured.Hunger);  // system holds 0.8, not 0.0
  }
  ```

  **Category C: Collection Isolation**
  ```csharp
  [Fact]
  public void InventorySystem_CaptureState_ClearingReturnedList_DoesNotAffectSystem()
  {
      var system = CreateInventoryWithItems("item_bandage", "item_iodine");
      var captured = system.CaptureState();
      captured.Items.Clear();  // Should throw (IReadOnlyList) OR not affect system
      var recaptured = system.CaptureState();
      Assert.Equal(2, recaptured.Items.Count);  // system still has both items
  }
  ```

  **Category D: Round-Trip Stability**
  ```csharp
  [Fact]
  public void AllSystems_CaptureRestore_ProducesSameChecksum()
  {
      var system = CreatePopulatedSystem();
      var state1 = system.CaptureState();
      system.RestoreState(state1);
      var state2 = system.CaptureState();
      Assert.Equal(SaveChecksum.Compute(state1), SaveChecksum.Compute(state2));
  }
  ```

  **Category E: Record Immutability (for migrated DTOs)**
  ```csharp
  [Fact]
  public void MarketState_InitOnlyProperties_CannotBeSetAfterConstruction()
  {
      // Corrected: uses MarketState (real, verified DTO), not the fictional NeedsState.
      // Also corrected: this test is only meaningful AFTER Step 2a lands and MarketState
      // has actually been migrated to a record in Step 4 — until then MarketState is a
      // plain mutable class and this test would not compile.
      var state = new MarketState { day = 5 };
      var prop = typeof(MarketState).GetProperty(nameof(MarketState.day));
      // Reflection SetValue on a true init-only property throws at runtime
      // (the compiler blocks direct assignment; this proves the runtime shape too).
      Assert.Throws<InvalidOperationException>(() => prop!.SetValue(state, 0));
  }
  ```

- Minimum test count is scaled to the number of systems actually migrated/patched by Steps 4-6 (audit-driven, per corrections above), not a fixed "40+" — state the real number once Step 1's audit is complete. Do not treat 40 as a target to hit for its own sake.
- Tests must run fast (no I/O, no file system, < 3 seconds total)

### Verification

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj    # all pass including new mutation-safety tests
```

### Done when

- `StateMutationSafetyTests.cs` contains a passing test for every system patched in Steps 4-6 (count determined by the audit, not fixed at 40 in advance)
- Every system that was migrated to records has at least one capture-isolation and one restore-isolation test
- Collection-based DTOs have list-mutation tests (proving Clear/Add/Remove doesn't leak)
- Round-trip checksum stability is verified for every DTO actually migrated in Step 4 (not "10 critical DTOs" — that number assumed names that don't exist; see Step 4 corrections)
- Test class serves as living documentation of the immutability contract
- No test takes more than 100ms individually (fast, no I/O)

---

## Summary Table

| Step | Deliverable | Files Created/Modified | Tests Added | Risk |
|------|-------------|----------------------|-------------|------|
| 1 | DTO audit + risk catalog | 0 (documentation only) | 0 | None |
| 2 | Immutability strategy + prototype | 1-2 design docs, 1 prototype DTO | 5 | None |
| 2a | **(NEW)** Fix `SaveChecksum` to cover public properties | 1 file (`SaveChecksum.cs`) + regression baseline tests | 5-8 | **Medium — every existing save's hash depends on this being byte-identical for field-only DTOs** |
| 3 | `StateCloner` + `ImmutableList<T>` + `IStateRecord` | 3 new files in `State/` | 15 | Low |
| 4 | Critical DTOs → records (count TBD by audit; originally stated as 10, but 3 of the 10 named DTOs don't exist and 2 more have unconfirmed save-boundary ownership — see corrections) | Realistically 3-5 confirmed DTOs to start (`WorldWeatherState`, `MarketState`, `CombatState`, plus others the corrected audit identifies), not "10-20 files" | Scales with DTO count, not fixed at 20 | Medium |
| 5 | Defensive copy in `CaptureState` — **audit-confirmed exceptions only** | Unknown until Step 1 audit completes; likely far fewer than 72+ given the spot-check finding that most systems already comply | Scales with exception count | Low-Medium (downgraded from Medium — most systems already comply) |
| 6 | Defensive copy in `RestoreState` — **audit-confirmed exceptions only** | Same caveat as Step 5 | Scales with exception count | Low-Medium |
| 7 | Mutation-safety test suite | `StateMutationSafetyTests.cs` | Scales with Steps 4-6 output, not fixed at 40+ | None |

**Total estimated new tests:** Cannot be fixed at ~100 until Step 1's audit produces real numbers; treat all counts in this table as placeholders pending that audit.
**Total files created:** 3-5 new infrastructure files, plus 1 file modified in Step 2a
**Total files modified:** Unknown until audit completes — likely a small fraction of "82+ system files," not all of them, given that most sampled systems already implement defensive copying
**Breaking changes:** DTO property setters change from `set` to `init` (compile error if external code mutates after construction — this is intentional) — applies only to the DTOs actually migrated to records, not to all 82+ systems
**Performance impact:** Minimal — defensive copies occur only at save/load boundaries (not per-frame); `StateCloner` JSON round-trip adds roughly 1ms per complex DTO (this figure is an estimate, not a measurement — measure it against a real DTO in Step 3's tests before repeating it as fact)
**Serialization compatibility:** Maintained — `System.Text.Json` supports `init` setters for deserialization; old save files deserialize into new records without issue

### Migration Priority Matrix

| DTO Risk Level | Characteristics | Action | Timeline |
|---------------|-----------------|--------|----------|
| **Critical** | Has mutable collections + used in SaveChecksum + flows between 3+ systems | Migrate to record immediately (Step 4, gated on Step 2a) | First |
| **High** | Has mutable collections OR used in SaveChecksum | Add defensive copying (Steps 5-6, only if audit finds it missing), schedule record migration | Second |
| **Medium** | Has mutable reference-type properties but limited cross-system flow | Add defensive copying (only if audit finds it missing) | Third |
| **Low** | Value-type properties only, single-system use | Document as safe, no action needed | — |

### Compatibility Notes

- **Old saves loading into new code:** `System.Text.Json` deserializes into records via `init` setters during construction — no format change needed
- **New saves loading into old code:** If old code uses `{ get; set; }` DTOs, the JSON is identical — compatible
- **SaveChecksum:** ~~Records have the same properties as classes — reflection-based hashing is unaffected~~ — **false, corrected:** `SaveChecksum` hashes public *fields* only (verified directly in `SaveChecksum.cs`). Records expose public *properties*, backed by private fields invisible to that reflection call. Hashing is **not** unaffected — it silently stops being data-sensitive for any migrated record until Step 2a's fix lands. This was the single most consequential factual error in the original document; it appeared in three separate places (Step 2, Step 4, and here) and is corrected in all three.
- **Cross-host:** Both Godot host and (legacy) Unity host see the same DTO shapes — cross-host save compatibility maintained. Note per AGENTS.md: the Unity host (`Assets/_Game/`) still uses `JsonUtility`, which is a distinct, already-tracked issue (C1 in AGENTS.md's Known Issues) and out of scope for this batch — do not conflate "cross-host DTO shape compatibility" (this batch's concern) with "cross-host serializer compatibility" (C1's concern, blocked on a separate `IJsonSerializer` adapter for Unity).


---

## Review Notes (Corrected)

This section records the adversarial review performed against the real codebase at
`/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`, what was found wrong, and what was fixed in place above. Read this alongside the inline corrections — the inline corrections are the actionable fixes; this section is the audit trail.

### Factual errors found and fixed

1. **False premise — "no defensive copying exists."** The Motivation/Hazard section claimed systems universally lack defensive copying. A direct read of `DoseLedgerSystem.cs`, `BrineWaterSystem.cs`, `CensusClaimSystem.cs`, `CohortSystem.cs`, `LedgerDebtSystem.cs`, `HoldfastQuestSystem.cs`, `IceRoadSystem.cs`, `District8DeepCoastSystem.cs`, `WeatherSystem.cs`, `CrossingArbitrationSystem.cs`, `MarketSystem.cs`, and `TacticalCombatSystem.cs` shows the *opposite*: `CaptureState()` builds a fresh copy in essentially every sampled case, and `RestoreState()` explicitly comments "Deep-copy: the deserialized DTO must not become the live state." This is not a hypothetical nuance — it inverts the entire premise Steps 5 and 6 were built on (a full 82-system rewrite vs. an audit-and-patch-exceptions pass). Fixed by reframing Steps 5-6 as audit-gated, and by correcting the Motivation section.

2. **Fabricated DTO names in Step 4's "critical 10" list.** `NeedsState`, `RadiationState`, and `WeatherState` do not exist anywhere in `Assets/Ashfall.Core/` — confirmed by `grep`/symbol search returning zero matches. `MoraleState`, `AfflictionState`, and `ShelterState` (items 5, 6, 8) likewise do not exist. That is 6 of the original 10 target DTOs either misnamed or nonexistent. The real names are `SurvivorNeedsState`, `SurvivorRadState`, and `WorldWeatherState` respectively for the first three, and no direct replacement was found by search for the other three without further investigation. Fixed by replacing the list with verified names, flagging unconfirmed ones, and making the list provisional pending a corrected Step 1 audit rather than treating it as final.

3. **The single most consequential error: `SaveChecksum` + records claimed to be compatible.** `SaveChecksum.WriteObject` (confirmed by reading `SaveChecksum.cs` directly) calls `type.GetFields(BindingFlags.Public | BindingFlags.Instance)` — it never calls `GetProperties`. A `record` with `{ get; init; }` properties has no public fields for this to find. The original document asserted the opposite ("records work with reflection," "SaveChecksum produces identical hashes... no regression," "reflection-based hashing is unaffected") in three separate places (Step 2, Step 4's Done-when, and the Compatibility Notes), even though the document's own "BLOCKING RISK" callout in the Problem Statement already stated the correct, contradictory fact. This is an internal contradiction in the original document, not just an error against the codebase. Fixed by: correcting all three false statements, adding a new gating Step 2a to actually fix `SaveChecksum`, and making Step 4 explicitly dependent on Step 2a.

4. **Confirmed true, unchanged:** the claim that DTOs use bare public mutable fields (not `{ get; set; }` properties) is accurate — verified in `MarketState`, `DoseLedgerSystemState`, and others, which all use fields like `public List<DemandEntry> demand = new List<DemandEntry>();`. The claim that `SystemTextJsonSerializer.Options` sets `IncludeFields = true` and `PropertyNameCaseInsensitive = true` is also accurate — verified directly in `Assets/Ashfall.Core/HostDefaults.cs`.

5. **Confirmed true: all three csproj files have `Nullable enable`.** `Ashfall.Core.csproj`, `Ashfall.Core.Tests.csproj`, and `Ashfall.csproj` all set `<Nullable>enable</Nullable>`. This matters for the immutability plan because `Ashfall.Core.csproj` does **not** suppress `CS8618` (missing-non-null-init warning-as-error under some configs), unlike the other two projects, which do suppress it. A migrated record with a non-nullable reference-type `init` property lacking a default value will fail to build in Core specifically. This was not called out as a build risk in the original document and is now added as a Secondary Risk in the Problem Statement.

6. **Internally inconsistent save-store counts.** The original document cited "27 stores" in the Motivation section and "22 save stores" in Step 6's Done-when — two different numbers for the same thing, neither verified. `grep -rl "public static class .*SaveStore" src/` returns **30** files as of this review. Fixed by using the verified count consistently and noting the grep command used, so the number can be re-verified cheaply as the codebase changes rather than trusted as a fixed constant.

7. **Framework version error.** The Problem Statement (and Step 2's constraints list) said Core targets `netstandard2.1`. `Ashfall.Core.csproj` actually specifies `<TargetFramework>net8.0</TargetFramework>`. This matches the AGENTS.md stack table's "Core" row being stale relative to the real csproj — worth flagging upstream, since other plans may repeat the same stale assumption. Fixed inline.

8. **`ExpeditionState` shape mismatch.** Step 4 target #10 assumed a single-object DTO matching the `NeedsState`-style example code, but `ExpeditionSystem.CaptureState()` actually returns `List<ExpeditionState>` — a collection, not a single instance. The generic "convert class to record" example code in Step 4 does not directly transfer. Fixed by calling this out explicitly in the target list.

### Structural / process issues found and fixed

9. **Vague "Done when" criteria replaced with falsifiable ones.** Several Done-when bullets ("Performance validated: copy overhead is acceptable," "SaveChecksum produces identical hashes... no regression") had no way to fail — "acceptable" and "no regression" are not verifiable without a stated baseline or threshold. Fixed by requiring an explicit before/after measurement with a numeric threshold, and by requiring an explicit test that asserts two different record instances hash *differently* (proving the hash is data-sensitive, not just "unchanged").

10. **Illogical step ordering.** The document's own Problem Statement flagged a blocking risk that should gate Step 4, but the actual Steps 2-4 as written treat records as the "recommended" approach without a gating mechanism — a reader following the steps in order would ship broken integrity hashing before ever seeing the blocking-risk paragraph acted upon. Fixed by inserting Step 2a between Step 2 and Step 3/4, with an explicit "Precondition (unskippable)" line added to Step 4's Done-when.

11. **Scope creep from unverified assumptions.** Steps 5 and 6 budgeted "72+ system files" each for defensive-copy additions under the assumption that defensive copying is absent. Given the audit finding in point 1 above, this could be a 10x overestimate of real work, or it could be an underestimate if the untested 60+ systems not sampled during this review are worse than the sample — the honest answer is "unknown until Step 1's audit runs." Fixed by making Steps 5-6's scope explicitly conditional on the audit's findings rather than a fixed file count.

12. **Missing risk/rollback for the new Step 2a.** Added an explicit Risk/Rollback subsection to Step 2a since it is a new, single-point-of-failure change to a load-path-critical file (`SaveChecksum.cs`) that every one of the 30 save stores depends on.

### What was verified true and left unchanged

- `SystemTextJsonSerializer.Options` config (`IncludeFields = true`, `PropertyNameCaseInsensitive = true`, `WriteIndented = false`) — verified in `HostDefaults.cs`.
- `SaveChecksum`'s null-string-as-empty and null-collection-as-empty normalizations — verified in `SaveChecksum.cs`'s `WriteNull`.
- `SaveChecksum.MaxDepth = 32` (used as the recursion guard) — verified; Batch 102's claimed "max depth 20" for its own diff engine is a different, unrelated number for a different subsystem — do not conflate the two documents' depth limits.
- The general shape of DTOs as `[Serializable]` classes with bare public mutable fields, exemplified accurately by `MarketState`/`DemandEntry`/`LedgerEntry`.
