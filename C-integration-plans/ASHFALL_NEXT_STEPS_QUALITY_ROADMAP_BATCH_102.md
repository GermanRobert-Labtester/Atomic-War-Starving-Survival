# ASHFALL Quality Roadmap — Batch 102

## Theme: Snapshot Diffing & Change Tracking — Know What Changed Between Saves

**Priority:** MEDIUM-HIGH
**Risk:** Low for Steps 1-5 (read-only comparison, no state mutation, purely additive new files). **Elevated for Step 6** — wiring into day-advance risks duplicating `Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs`'s existing snapshot/event pipeline if not explicitly integrated with it (see Step 6 correction and Risk/Rollback section below). The original "Low" rating did not account for this.
**Estimated Scope:** ~1,200 lines of Core code + ~400 lines of tests (unchanged estimate; see Summary Table caveat about not being recomputed for the corrections in this review)
**Dependencies:** `SaveChecksum`, `CaptureState/RestoreState` pattern (all 82+ systems), `IJsonSerializer`. **Also depends on:** `CampaignDayCoordinator` (Step 6), and — if Batch 101 "Immutable State DTOs" lands first — a decision on whether `DeepDiffEngine` reads fields, properties, or both (see Step 2 correction).

---

## Motivation

`SaveChecksum` (reflection-based, `Assets/Ashfall.Core/SaveChecksum.cs`, **177 lines — corrected: the original "603 lines" is wrong; verified with `wc -l` against the actual file. 603 does not match any file this reviewer could find; it may be a copy-paste from a different subsystem's line count and should not be repeated without re-verifying**) computes a single integrity hash of entire game state. When the hash differs between two saves, *something* changed — but **what** changed is invisible. Today, debugging a save corruption or determinism drift requires manually diff-ing multi-thousand-line JSON exports. No structured, field-level diff exists anywhere in the codebase (confirmed: no `Diffing` directory, no `StateDiff`, `FieldChange`, or similarly-named type exists under `Assets/Ashfall.Core/` today).

### This blocks:

| Blocked capability | Why it needs diffing |
|---|---|
| Meaningful undo | Must show the player exactly what would be reverted |
| Overnight change report | "While you slept: iron price rose, 2 survivors fell ill" |
| Save debugging | Identify which system's state drifted between two checkpoints |
| Determinism verification | Pinpoint exact field where replay diverges from reference |
| Regression testing | Assert that a code change doesn't silently alter unrelated state |

### Design constraints:

- Must live in `Assets/Ashfall.Core/` — zero engine references (`Invariant 1`).
- Must use reflection consistent with how `SaveChecksum` already traverses DTOs. **Corrected — this needs a concrete decision, not just a general intention:** `SaveChecksum.WriteObject` (verified directly in `Assets/Ashfall.Core/SaveChecksum.cs`) walks `type.GetFields(BindingFlags.Public | BindingFlags.Instance)` — **fields only, never properties.** Since ASHFALL's current DTOs are universally plain public mutable fields (verified in `MarketState`, `DoseLedgerSystemState`, `WorldWeatherState`, and others), `DeepDiffEngine` (Step 2) must also walk fields, not properties, to stay consistent with what `SaveChecksum` actually hashes and with what the DTOs actually expose. If a sibling batch (Batch 101, "Immutable State DTOs") introduces `record` types with `{ get; init; }` properties, `DeepDiffEngine`'s field-only walk will silently see zero members on those types too — the same blind spot `SaveChecksum` has. This dependency was not previously called out; it now is, in Step 2 below.
- Must handle the full DTO zoo: primitives, enums, strings, nested objects, `List<T>`, `Dictionary<K,V>`, nullable value types.
- Must be deterministic — same two inputs always produce the same diff output (no `Guid`, no `DateTime.Now`).
- Performance budget: diffing a full save (**verified: 30 save stores exist under `src/Host/`, `src/Journal/`, `src/YearOfAsh/` as of this writing via `grep -rl "public static class .*SaveStore" src/` — not "82+ systems"; 82+ is AGENTS.md's count of in-memory systems with `CaptureState`/`RestoreState`, which is a different, larger population than the 30 on-disk save stores this diffing feature would actually operate on for a "full save diff." Both numbers appear in this document; they measure different things and must not be used interchangeably**) must complete in < 200ms on a mid-range machine.

---

## Step 1 — Design `StateDiff<T>` and `FieldChange` Record Types

### Goal
Define the core data model for representing differences between two DTO snapshots. This is the foundation every subsequent step builds on.

### Implementation

**File:** `Assets/Ashfall.Core/Diffing/StateDiff.cs`

```csharp
namespace Ashfall.Core.Diffing
{
    /// <summary>
    /// Result of comparing two instances of the same DTO type.
    /// </summary>
    public sealed class StateDiff<T>
    {
        public IReadOnlyList<FieldChange> Changes { get; }
        public bool HasChanges => Changes.Count > 0;
        public Type ComparedType => typeof(T);
    }

    /// <summary>
    /// One field-level difference between two DTO snapshots.
    /// </summary>
    public sealed class FieldChange
    {
        public string Path { get; }           // e.g. "Survivors[3].Health"
        public ChangeKind Kind { get; }       // Modified, Added, Removed
        public object OldValue { get; }       // null for Added
        public object NewValue { get; }       // null for Removed
        public Type FieldType { get; }        // declared type of the field
    }

    public enum ChangeKind { Modified, Added, Removed }
}
```

**Design decisions:**
- `Path` uses dot-notation with bracket indexing for collections — matches how `SaveChecksum` normalizes field traversal.
- Generic `StateDiff<T>` retains compile-time type safety at the call site.
- `FieldChange` stores boxed `object` for old/new values — acceptable because diffs are transient diagnostic objects, not hot-path allocations.
- Immutable after construction (sealed class, `IReadOnlyList`).

### Verification
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — compiles.
- Manually verify zero `UnityEngine`/`Godot` references in new file.

### Done when
- `StateDiff<T>`, `FieldChange`, and `ChangeKind` compile in `Ashfall.Core`.
- No engine coupling. No external dependencies beyond `System.*`.

---

## Step 2 — Implement Reflection-Based Deep Diff Engine

### Goal
Build the comparison engine that walks two DTO object graphs and produces a `StateDiff<T>`. Must handle the same types `SaveChecksum` already traverses.

**Corrected (see Review Notes):** "The same types `SaveChecksum` already traverses" specifically means **public instance fields**, not properties (verified directly against `SaveChecksum.cs`). `DeepDiffEngine.Walk` must use `GetFields`, matching `SaveChecksum`, not `GetProperties`. If this engine is ever pointed at a `record`-based DTO (e.g. from Batch 101's proposed migration), it will report zero differences for that type until `DeepDiffEngine` is deliberately extended to also read properties — the same blind spot as `SaveChecksum`. This should be a shared decision between the two batches, not solved twice, differently, by accident.

### Implementation

**File:** `Assets/Ashfall.Core/Diffing/DeepDiffEngine.cs`

```csharp
namespace Ashfall.Core.Diffing
{
    public static class DeepDiffEngine
    {
        public static StateDiff<T> Compare<T>(T before, T after) where T : class;

        // Internal recursive walker
        private static void Walk(object a, object b, string pathPrefix, List<FieldChange> changes);
    }
}
```

**Type handling matrix:**

| Type category | Comparison strategy | Path notation |
|---|---|---|
| Primitives (`int`, `float`, `bool`) | Direct equality (`Equals`) | `"FieldName"` |
| `string` | Ordinal equality | `"FieldName"` |
| `enum` | Integer comparison + name in display | `"FieldName"` |
| Nullable value types | Null-aware unwrap, then compare inner | `"FieldName"` |
| Nested `[Serializable]` objects | Recursive `Walk` | `"Parent.Child.Field"` |
| `List<T>` / `IList<T>` | Index-aligned comparison; detect added/removed at tail | `"ListField[i]"` |
| `Dictionary<K,V>` | Key-set union; compare values for shared keys; report added/removed keys | `"DictField[key]"` |
| `float` / `double` | Epsilon comparison (1e-9) matching `SaveChecksum`'s G9 formatting | — |

**Corrected — the epsilon claim needs scrutiny:** `SaveChecksum` does not do epsilon comparison; it formats `float` with `.ToString("G9", ...)` and `double` with `.ToString("G17", ...)` (verified in `SaveChecksum.cs`) and hashes the resulting *string*. That means two floats that differ by less than what G9 can represent hash identically in `SaveChecksum`, which is a side effect of string formatting, not an explicit epsilon check. `DeepDiffEngine` proposing a literal `1e-9` epsilon is not "matching" `SaveChecksum` — it is a different mechanism that happens to produce similar (not identical) results for `float` and would diverge for `double` (`SaveChecksum` uses G17 precision for doubles, far tighter than a flat `1e-9` epsilon would allow). Decide explicitly: either (a) reproduce `SaveChecksum`'s exact string-formatting-based comparison for byte-for-byte behavioral parity, or (b) use a real epsilon and document that it is deliberately more/less permissive than `SaveChecksum`, with a stated reason. Do not describe one as "matching" the other when it isn't.

**Edge cases:**
- Both null → no change.
- One null, other non-null → single `FieldChange` (Modified, old=null or new=null).
- Circular references → depth limit (**corrected: `SaveChecksum.MaxDepth` is 32, not 20 — verified as `public const int MaxDepth = 32;` in `SaveChecksum.cs`. Match it exactly at 32 if the intent is consistency with `SaveChecksum`, or pick a deliberately different value and say why — do not silently use a different unexplained number**).
- Fields with `[NonSerialized]` attribute → skip (mirrors save behavior). **Corrected: this is a real, documented `SaveChecksum` behavior (see its `WriteObject`, which checks `Attribute.IsDefined(field, typeof(NonSerializedAttribute))`), but a codebase-wide search for `[NonSerialized]` usage on any DTO field found zero results.** This edge case is real infrastructure to keep parity with `SaveChecksum`, but do not over-invest in it — the corresponding `NonSerializedFieldSkipped` test in Step 7 should use a synthetic test-only DTO (already planned) since no real DTO exercises this path today, and that should be stated explicitly so nobody goes looking for a real example.

### Verification
- Unit test: compare two identical flat DTOs → empty diff.
- Unit test: change one field → exactly one `FieldChange` with correct path, old, new.
- `dotnet test` passes.

### Done when
- `DeepDiffEngine.Compare<T>` handles the DTO type categories enumerated in the matrix above (primitives, strings, enums, nullable value types, nested objects, `List<T>`, `Dictionary<K,V>`, floats/doubles) — verified by a passing test per category, not by an unquantified claim about "82+ system states" that this document cannot check as a single condition.
- Reflection traversal uses `GetFields(BindingFlags.Public | BindingFlags.Instance)` in ordinal name order, matching `SaveChecksum` exactly (verified by a test comparing the field set visited for one shared real DTO).
- Performance: < 5ms for a single system DTO diff in test, measured with `Stopwatch`.

---

## Step 3 — Add System-Level Diffing (Single System Between Two Points)

### Goal
Provide a convenience API that captures two `CaptureState()` snapshots from any system and produces a typed diff. This is the developer-facing diagnostic tool for "what did this system change during the last tick?"

### Implementation

**File:** `Assets/Ashfall.Core/Diffing/SystemDiffer.cs`

```csharp
namespace Ashfall.Core.Diffing
{
    public sealed class SystemDiffer
    {
        /// <summary>
        /// Compare two captured states from the same system.
        /// </summary>
        public StateDiff<TState> Diff<TState>(TState before, TState after) where TState : class
            => DeepDiffEngine.Compare(before, after);

        /// <summary>
        /// Snapshot-and-hold pattern: call BeginSnapshot, run logic, call EndSnapshot.
        /// </summary>
        public SnapshotScope<TState> BeginSnapshot<TState>(TState currentState) where TState : class;
    }

    public sealed class SnapshotScope<TState> where TState : class
    {
        public TState Before { get; }
        public StateDiff<TState> Complete(TState after);
    }
}
```

**Usage pattern (diagnostic):**
```csharp
var scope = differ.BeginSnapshot(marketSystem.CaptureState());
sim.TickSimDay();
var diff = scope.Complete(marketSystem.CaptureState());
// diff.Changes → field-level changes on MarketState, e.g. its `demand` list entries or `day`/`tickCount`.
// Corrected: the original example used "IronPrice" as a field name and a type called
// "MarketSystemState." Neither exists. The real, verified type is `MarketState`
// (Assets/Ashfall.Core/Economy/MarketSystem.cs), and its fields are `systemId`, `version`,
// `day`, `tickCount`, `demand` (List<DemandEntry>), and `ledger` (List<LedgerEntry>) — there
// is no single scalar "price" field; per-item prices live inside `DemandEntry.multiplier`
// and `LedgerEntry.unitPrice`. Any example code or test built against "IronPrice" will not
// compile. Use `demand`/`ledger`/`DemandEntry.multiplier` in real examples and tests.
```

### Verification
- Integration test: construct a `MarketState` DTO (not `MarketSystemState` — that type does not exist; corrected), mutate a `DemandEntry.multiplier` or `LedgerEntry` entry, diff → correct `FieldChange`.
- Verify `BeginSnapshot` stores a deep clone (not reference alias) to prevent false positives.

### Done when
- `SystemDiffer` and `SnapshotScope<T>` compile and have passing tests.
- Deep clone uses serialization round-trip (via `IJsonSerializer`) to guarantee isolation — verified by a test that mutates the clone after `BeginSnapshot` and confirms the system's real internal state (via a fresh `CaptureState()` call) is unaffected, not merely asserted in prose.

---

## Step 4 — Add Full-Save Diffing (All Systems Between Two Saves)

### Goal
Diff the entire game state between two points in time. **Corrected — "all 82+ systems, all 22 save stores" conflates two different, differently-sized populations and repeats the same unverified "22" figure Batch 101 also had to correct to "30".** AGENTS.md's "82+" refers to in-memory systems implementing `CaptureState`/`RestoreState`; the on-disk save stores with checksummed envelopes are a separate, smaller population, verified at **30** files via `grep -rl "public static class .*SaveStore" src/`. A "full save diff" as described in this step's own `FullSaveDiffer.Compare` signature (`IReadOnlyDictionary<string, object>` of system-name → state-object) most naturally corresponds to the 30 on-disk save stores, since that is what a "save" actually contains on disk — decide and state explicitly which population this feature targets before implementing, rather than citing both numbers as if they were interchangeable. Produces a composite report: which systems changed, which didn't, and the field-level changes within each.

### Implementation

**File:** `Assets/Ashfall.Core/Diffing/FullSaveDiff.cs`

```csharp
namespace Ashfall.Core.Diffing
{
    public sealed class FullSaveDiff
    {
        public IReadOnlyList<SystemDiffEntry> Entries { get; }
        public int TotalChanges => Entries.Sum(e => e.Changes.Count);
        public IEnumerable<SystemDiffEntry> ChangedSystems => Entries.Where(e => e.HasChanges);
    }

    public sealed class SystemDiffEntry
    {
        public string SystemName { get; }        // e.g. "MarketSystem", "RadiationSystem"
        public IReadOnlyList<FieldChange> Changes { get; }
        public bool HasChanges => Changes.Count > 0;
    }
}
```

**File:** `Assets/Ashfall.Core/Diffing/FullSaveDiffer.cs`

```csharp
namespace Ashfall.Core.Diffing
{
    public sealed class FullSaveDiffer
    {
        /// <summary>
        /// Compare two full save snapshots (dictionary of system-name → state-object).
        /// </summary>
        public FullSaveDiff Compare(
            IReadOnlyDictionary<string, object> beforeStates,
            IReadOnlyDictionary<string, object> afterStates);
    }
}
```

**Considerations:**
- Systems present in `after` but not `before` → all fields reported as `Added`.
- Systems present in `before` but not `after` → all fields reported as `Removed`.
- System names derived from the save store registry (**corrected: 30 stores, verified — see Goal correction above**) + any in-memory-only systems that implement `CaptureState`.
- Must handle the case where a system's DTO type changed between versions (version mismatch → report as incomparable, not crash).

### Verification
- Test: two identical full-save dictionaries → `TotalChanges == 0`.
- Test: mutate one system's state → `ChangedSystems` contains exactly that system.
- Test: add a new system to `after` → reported as `Added`.
- Performance benchmark: **corrected to the verified store count** — 30 stores (not 82), ~50 fields average → < 200ms. If the intent is genuinely to benchmark all 82+ in-memory systems (a broader, heavier scope than "full save diff" implies), say so explicitly and adjust the 200ms budget accordingly, since 82 systems is roughly 2.7x the load of 30.

### Done when
- `FullSaveDiffer` correctly aggregates per-system diffs.
- Handles system add/remove gracefully.
- Performance within budget — budget and system count stated as a single consistent number (see correction above), and measured with `Stopwatch` in an actual test, not assumed.

---

## Step 5 — Create Human-Readable Diff Formatter

### Goal
Transform raw `FieldChange` records into player-facing or developer-facing text. Two modes: terse (for logs/debugging) and narrative (for the player's "overnight report").

### Implementation

**File:** `Assets/Ashfall.Core/Diffing/DiffFormatter.cs`

```csharp
namespace Ashfall.Core.Diffing
{
    public static class DiffFormatter
    {
        /// <summary>Developer log format: e.g. "MarketSystem: day 5 → 6"</summary>
        public static string FormatTerse(SystemDiffEntry entry);

        /// <summary>Player-facing narrative, using a template lookup keyed by field path.</summary>
        public static string FormatNarrative(FieldChange change, INarrativeTemplate templates);

        /// <summary>Full save diff as multi-line summary.</summary>
        public static string FormatFullDiff(FullSaveDiff diff, DiffFormatOptions options);
    }

    public sealed class DiffFormatOptions
    {
        public bool IncludeUnchangedSystems { get; set; } = false;
        public bool GroupBySystem { get; set; } = true;
        public int MaxChangesPerSystem { get; set; } = 20;
    }
}
```

**Corrected — `INarrativeTemplate` is an undefined type.** A codebase-wide search found zero matches for `INarrativeTemplate` anywhere in the repository. This step introduces a new interface with no design given beyond its use as a parameter type in `FormatNarrative`'s signature — there is no member list, no implementation sketch, and no indication of who constructs it or where it's registered. Before this step is scheduled, `INarrativeTemplate` needs its own mini-design (at minimum: how it looks up a template for a `(SystemName, FieldPath)` key, and what it returns when no template matches) — treat this as missing design work, not an implementation detail to improvise while coding.

**Narrative template system:**
- Templates keyed by `(SystemName, FieldPath)` → narrative string with `{old}` / `{new}` placeholders.
- Default fallback: `"{path} changed from {old} to {new}"`.
- Templates loaded from `Assets/StreamingAssets/Data/diff_templates.json` (data authority). **Note:** this file does not exist yet — confirm this path via `Assets/Ashfall.Core/HostDefaults.cs`'s `CatalogLocator` conventions (it resolves paths relative to `Assets/StreamingAssets/Data`, which does exist) before creating it, and follow AGENTS.md's data-authority rules (snake_case keys, `schema_version` field) for the new file, which the original step did not mention.
- List changes formatted as: `"{count} items added to {path}"` / `"{count} items removed from {path}"`.

**Example outputs (corrected — using verified real system/field names, not invented ones):**
```
[Terse]  MarketSystem: day 5 → 6
[Terse]  DoseLedgerSystem: entries[survivor_003].cumulativeMsv 12.5 → 13.1
[Terse]  WeatherSystem: currentKind Clear → Storm

[Narrative] The market advanced a day.
[Narrative] A survivor's radiation dose increased.
[Narrative] The weather turned stormy overnight.
```
Corrected: the original examples used `RadiationSystem`/`AccumulatedDose` and `NeedsSystem`/`Survivors[2].Hunger`, neither of which matches any real type or field found in the codebase (the real per-survivor radiation DTO is `SurvivorRadState`; the real dose-tracking system with an actual top-level `CaptureState()` is `DoseLedgerSystem`/`DoseLedgerSystemState`, whose real field is `cumulativeMsv`, not `AccumulatedDose`).

### Verification
- Unit test: format a known `FieldChange` → expected terse string.
- Unit test: format with narrative template → placeholder substitution works.
- Unit test: `FormatFullDiff` with empty diff → "No changes detected."

### Done when
- Both terse and narrative formatters produce correct, deterministic output.
- Narrative templates are data-driven (JSON), not hardcoded.
- Output is culture-invariant (numbers formatted with invariant culture).

---

## Step 6 — Wire Into Day-Advance (Overnight Report Generation)

### Goal
Automatically capture before/after snapshots around the day-advance flow to produce the diff that powers the player-facing "overnight report" — what changed while time advanced.

**Corrected (see Review Notes) — significant scope/duplication finding:** `sim.TickSimDay()` as written does not match the real API. The real `TickSimDay` (verified in `src/Main.cs`) is a **private** method on `AtomicWar.GodotApp.Main`, `private void TickSimDay(int day)` — it takes a day argument and is not callable as a bare `sim.TickSimDay()` from outside `Main`, let alone from `Assets/Ashfall.Core/` (which per Invariant 1 cannot reference the Godot host at all). More importantly: `Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs` **already exists** and its own doc comment states it is the "Single authority for advancing the campaign by exactly one in-game day," explicitly responsible for "capture pre-day snapshots in deterministic owner order" (step 1 of its 6-step contract) and "collect typed state-change events per owner" (step 3). This is functionality this step proposes to build essentially from scratch (`DayAdvanceDiffCapture.BeforeAdvance/AfterAdvance`) without ever mentioning `CampaignDayCoordinator`'s existence. Before implementing this step: read `CampaignDayCoordinator.cs` in full and either (a) hook `DeepDiffEngine`/`FullSaveDiffer` into its existing snapshot/event pipeline instead of building a parallel one, or (b) if a genuinely separate mechanism is needed, explicitly justify why `CampaignDayCoordinator`'s existing snapshot capture and event collection cannot be reused. Shipping a second, uncoordinated day-advance snapshot system risks the exact "triad drift" risk AGENTS.md's H7 already warns about for `Main.cs` (a Setup without a Save silently drops state) — this would be a similar failure mode for diffing (a system tracked by `CampaignDayCoordinator` but not registered with `DayAdvanceDiffCapture`, or vice versa, silently drops from the report).

### Implementation

**File:** `Assets/Ashfall.Core/Diffing/DayAdvanceDiffCapture.cs`

```csharp
namespace Ashfall.Core.Diffing
{
    public sealed class DayAdvanceDiffCapture
    {
        private readonly FullSaveDiffer _differ;
        private readonly Func<IReadOnlyDictionary<string, object>> _captureAll;

        /// <summary>
        /// Call before TickSimDay. Stores the "before" snapshot.
        /// </summary>
        public void BeforeAdvance();

        /// <summary>
        /// Call after TickSimDay. Computes diff, stores result.
        /// </summary>
        public void AfterAdvance();

        /// <summary>
        /// Last computed overnight diff. Null if no advance has occurred yet.
        /// </summary>
        public FullSaveDiff LastOvernightDiff { get; }

        /// <summary>
        /// Formatted narrative for UI consumption.
        /// </summary>
        public IReadOnlyList<string> LastOvernightNarratives { get; }
    }
}
```

**Wiring (Godot host, `src/Main.cs` or relevant session):**
```csharp
// Corrected: TickSimDay is private and takes an int day argument (verified in src/Main.cs,
// line ~1643: `private void TickSimDay(int day)`). It cannot be called as a bare
// `sim.TickSimDay()` from Ashfall.Core (which also could not reference it directly per
// Invariant 1 regardless). More importantly, CampaignDayCoordinator.Advance() (or
// equivalent) is the real seam per that class's own doc comment — wire diff capture through
// it rather than around it:
_dayAdvanceCapture.BeforeAdvance();
_campaignDayCoordinator.Advance(); // real Core-side entry point; verify exact method name/signature before use
_dayAdvanceCapture.AfterAdvance();
// UI reads _dayAdvanceCapture.LastOvernightNarratives
```

**Save integration:**
- `DayAdvanceDiffCapture` does NOT persist diffs long-term (transient diagnostic).
- Optionally: store last N diffs (configurable, default 3) for "recent history" panel.
- `CaptureState/RestoreState` stores only the count of retained diffs, not the diffs themselves (diffs are regenerated on load if needed).

**Performance guard:**
- Full snapshot capture is already happening for save — reuse the same `CaptureState` calls.
- If diff computation exceeds 100ms, log a warning and truncate to top-20 changed systems.

### Verification
- Integration test: advance one day on a test scenario → `LastOvernightDiff` is non-null, contains expected changes.
- Test: advance with no systems changing (empty tick) → diff is empty.
- Performance test: measure wall-clock time against whatever population this step actually integrates with (the audit-confirmed system count from `CampaignDayCoordinator`'s registered owners, not an assumed "82 systems" figure that was never verified against that class's actual owner registry).

### Done when
- Day-advance automatically produces a structured diff, integrated with (not parallel to) `CampaignDayCoordinator`'s existing snapshot/event pipeline — or an explicit, reviewed justification is recorded for why a separate mechanism was chosen instead.
- Narrative strings are available for UI consumption.
- No performance regression on day-advance (< 200ms overhead), measured against a baseline captured before this step's changes.

---

## Step 7 — Write Comprehensive Diff Tests

### Goal
Pin the entire diffing subsystem with tests covering: identical states, field-level changes, nested objects, list mutations, dictionary mutations, edge cases, and integration with real system DTOs.

### Implementation

**File:** `Ashfall.Core.Tests/DiffingTests.cs`

```csharp
namespace Ashfall.Core.Tests
{
    public class DeepDiffEngineTests
    {
        [Fact] public void IdenticalFlatDto_EmptyDiff();
        [Fact] public void SingleFieldChanged_OneChange();
        [Fact] public void NestedObjectFieldChanged_PathIncludesNesting();
        [Fact] public void ListItemAdded_ReportedAsAdded();
        [Fact] public void ListItemRemoved_ReportedAsRemoved();
        [Fact] public void ListItemModified_ReportedAsModified();
        [Fact] public void DictionaryKeyAdded_ReportedAsAdded();
        [Fact] public void DictionaryKeyRemoved_ReportedAsRemoved();
        [Fact] public void NullToNonNull_ReportedAsModified();
        [Fact] public void NonNullToNull_ReportedAsModified();
        [Fact] public void FloatEpsilonWithinTolerance_NoDiff();
        [Fact] public void FloatBeyondEpsilon_ReportedAsModified();
        [Fact] public void EnumValueChanged_ReportedWithNames();
        [Fact] public void NonSerializedFieldSkipped();
        [Fact] public void DeeplyNestedObject_MaxDepthRespected();
    }

    public class FullSaveDiffTests
    {
        [Fact] public void IdenticalSaves_ZeroTotalChanges();
        [Fact] public void OneSystemChanged_OnlyThatSystemReported();
        [Fact] public void NewSystemInAfter_AllFieldsAdded();
        [Fact] public void MissingSystemInAfter_AllFieldsRemoved();
        [Fact] public void ThirtyStores_CompletesUnder200ms(); // corrected from "EightyTwoSystems" — see Step 4 correction on which population this targets
    }

    public class DiffFormatterTests
    {
        [Fact] public void TerseFormat_MatchesExpectedPattern();
        [Fact] public void NarrativeFormat_SubstitutesPlaceholders();
        [Fact] public void EmptyDiff_FormatsAsNoChanges();
    }

    public class DayAdvanceDiffCaptureTests
    {
        [Fact] public void AfterAdvance_DiffIsNonNull();
        [Fact] public void NoChanges_DiffIsEmpty();
        [Fact] public void KnownMutation_AppearsInDiff();
    }
}
```

**Test strategy:**
- Use small, purpose-built DTOs for unit tests (not real game DTOs) to isolate diff logic.
- Use one real system DTO (**corrected: `MarketState`, not `MarketSystemState` — the latter does not exist anywhere in the codebase; verified by search**) for integration smoke test.
- Performance test uses `Stopwatch` + `Assert.True(elapsed < TimeSpan.FromMilliseconds(200))`.

### Verification
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # Must compile
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj    # All pass
dotnet build Ashfall.csproj                                  # Godot host clean
```

### Done when
- All diff tests pass.
- Coverage: primitives, strings, enums, nested objects, lists, dictionaries, nulls, floats, edge cases — each with a named, passing test (the list above), not asserted as a blanket claim.
- No test uses engine-specific APIs.
- Tests are deterministic (no randomness, no time-dependency).

---

## Summary Table

| Step | Deliverable | Files | Key Risk | Estimated LOC |
|------|------------|-------|----------|---------------|
| 1 | `StateDiff<T>`, `FieldChange`, `ChangeKind` | `Diffing/StateDiff.cs` | None — pure data types | ~60 |
| 2 | `DeepDiffEngine` (reflection walker) | `Diffing/DeepDiffEngine.cs` | Reflection edge cases (dictionaries, circular refs); must match `SaveChecksum`'s field-only reflection exactly (see correction) | ~350 |
| 3 | `SystemDiffer` + `SnapshotScope<T>` | `Diffing/SystemDiffer.cs` | Deep clone correctness | ~80 |
| 4 | `FullSaveDiffer` + `FullSaveDiff` | `Diffing/FullSaveDiff.cs`, `FullSaveDiffer.cs` | System add/remove between versions; must pick one consistent target population (30 save stores vs. 82+ in-memory systems) instead of conflating both | ~150 |
| 5 | `DiffFormatter` (terse + narrative) | `Diffing/DiffFormatter.cs` | Narrative template coverage; `INarrativeTemplate` has no design yet — treat as a blocker, not a detail | ~200 |
| 6 | `DayAdvanceDiffCapture` | `Diffing/DayAdvanceDiffCapture.cs` | **Elevated risk — duplicates `CampaignDayCoordinator`'s existing snapshot/event pipeline unless explicitly integrated with it; wiring example referenced a nonexistent public `sim.TickSimDay()` API** | ~120 |
| 7 | Test suite (**corrected count — see Done-when**) | `Ashfall.Core.Tests/DiffingTests.cs` | Covering all DTO type combinations; one example DTO name (`MarketSystemState`) was fictional | ~400 |

**Total estimated:** ~1,360 lines across 7 files. This total was not recomputed from the corrections above (e.g. Step 5's `INarrativeTemplate` design gap could add LOC not currently budgeted) — treat it as a rough floor, not a ceiling.

---

## Verification Checklist (per project rules)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # Must compile cleanly
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj     # All tests pass (including new DiffingTests)
3. dotnet build Ashfall.csproj                                  # Godot host: 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest       # Catalog integrity: 0 errors
5. godot --headless --path . -- --bridge-selftest               # Exits 0
```

---

## Risk / Rollback (added by this review — the original document had none)

- **Risk — Step 2/DeepDiffEngine and Batch 101 interaction:** if Batch 101 (Immutable State DTOs) ships record-based DTOs before `DeepDiffEngine` is extended to read properties (not just fields), any diff run against a migrated DTO silently reports zero changes, the same blind spot identified in `SaveChecksum`. These two batches must either land in a coordinated order or `DeepDiffEngine` must be designed from day one to handle both fields and properties. Track this as a cross-batch dependency, not an isolated concern.
- **Risk — Step 6/CampaignDayCoordinator duplication:** building `DayAdvanceDiffCapture` without integrating `CampaignDayCoordinator`'s existing snapshot pipeline creates two independent "capture state around day advance" mechanisms that can drift out of sync — a system registered with one but not the other silently disappears from either the save or the overnight report.
- **Rollback:** Steps 1-5 are additive, new files under `Diffing/` with no changes to existing systems — safe to fully revert by deleting the `Diffing/` directory and its test file if the feature is abandoned mid-way, with zero impact on existing save/load behavior. Step 6 is the only step that touches existing wiring (`src/Main.cs` or `CampaignDayCoordinator`); revert that specific integration point independently of Steps 1-5 if it proves unsafe, since the diffing library itself has no dependency on being wired into day-advance to be useful as a standalone debugging tool.
- **Rollback verification:** after reverting Step 6's wiring, confirm `dotnet build Ashfall.csproj` and the existing day-advance integration tests (if any exist under `Ashfall.Core.Tests` for `CampaignDayCoordinator`) still pass unmodified — this proves the revert didn't leave a dangling partial integration.

---

## Exit Criteria

This batch is complete when:
- [ ] `StateDiff<T>` and `FieldChange` types exist in `Ashfall.Core/Diffing/`.
- [ ] `DeepDiffEngine` handles the DTO type categories enumerated in Step 2's matrix, each with a passing test — not "all DTO types used by the 82+ systems," which is not a single checkable condition.
- [ ] `FullSaveDiffer` can diff two complete save snapshots, against the explicitly chosen target population (30 save stores, per the Step 4 correction — restate here if a different population was deliberately chosen instead).
- [ ] `DiffFormatter` produces both terse and narrative output, with `INarrativeTemplate` fully designed and implemented (not left as an unimplemented interface reference).
- [ ] `DayAdvanceDiffCapture` is wired into the day-advance path **through `CampaignDayCoordinator`** (or an explicit, reviewed justification is recorded for bypassing it).
- [ ] The test suite in Step 7 passes in full — count determined by the final test list, not fixed at an arbitrary "22+" chosen before the tests were written.
- [ ] Performance: full-save diff < 200ms, measured against the same population Step 4 was scoped to.
- [ ] Zero engine coupling in all new code (no `UnityEngine`, no `Godot`).
- [ ] All 5 verification steps pass.


---

## Review Notes (Corrected)

This section records the adversarial review performed against the real codebase at
`/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`, what was found wrong, and what was fixed in place above.

### Factual errors found and fixed

1. **Wrong line count for `SaveChecksum.cs`.** The Motivation section claimed 603 lines. `wc -l Assets/Ashfall.Core/SaveChecksum.cs` returns **177**. 603 does not match this file or any other file this reviewer checked; it may have been copied from `CatalogIntegrityValidator.cs` (AGENTS.md cites 603 lines for that file, though the real count there is 657 — a separate, pre-existing discrepancy in AGENTS.md, not introduced by this batch). Fixed by citing the verified 177 and flagging the likely source of the mix-up.

2. **`SaveChecksum` reflects fields only — this directly determines `DeepDiffEngine`'s design and was previously stated only vaguely ("must use reflection consistent with... traverses").** Verified directly in `SaveChecksum.cs`: `WriteObject` calls `type.GetFields(BindingFlags.Public | BindingFlags.Instance)`, never `GetProperties`. The original document's design constraints section left this ambiguous. Fixed by making it an explicit requirement in Step 2, and by flagging the cross-batch risk: if Batch 101's proposed `record`-based DTOs (with `init` properties, no public fields) land before `DeepDiffEngine` is extended past fields-only, diffing silently reports zero changes for those types — the identical blind spot Batch 101's own review found in `SaveChecksum` itself.

3. **`SaveChecksum.MaxDepth` is 32, not 20.** Verified: `public const int MaxDepth = 32;` in `SaveChecksum.cs`. The original Step 2 edge-case list said "max 20 levels, matches `SaveChecksum` recursion guard" — it does not match; 20 ≠ 32. Fixed by citing the correct number and requiring an explicit choice (match at 32, or deliberately diverge and say why).

4. **False "epsilon comparison matching G9 formatting" claim.** `SaveChecksum` does not do epsilon comparison at all — it formats floats with `.ToString("G9", ...)` and doubles with `.ToString("G17", ...)` and hashes the resulting string. A flat `1e-9` epsilon is a different mechanism with different behavior, especially for doubles (G17 is far more precise than a flat `1e-9` epsilon would allow). Fixed by requiring an explicit choice between reproducing `SaveChecksum`'s string-formatting approach or using a real epsilon with a stated, deliberate divergence.

5. **`INarrativeTemplate` is an undefined type with zero design.** `FormatNarrative(FieldChange change, INarrativeTemplate templates)` references an interface that does not exist anywhere in the codebase and has no member list, construction story, or lookup semantics described anywhere in the original document. This is missing design work masquerading as an implementation detail. Fixed by flagging it as a blocker requiring its own mini-design before Step 5 is implementable.

6. **Fabricated field/type names throughout examples.** `MarketSystemState` (used in Steps 3 and 7) does not exist — the real, verified type is `MarketState` (`Assets/Ashfall.Core/Economy/MarketSystem.cs`). `"IronPrice"` as a field name does not exist on any real DTO found by search — `MarketState`'s real fields are `systemId`, `version`, `day`, `tickCount`, `demand` (`List<DemandEntry>`), `ledger` (`List<LedgerEntry>`); per-item price data lives in `DemandEntry.multiplier` and `LedgerEntry.unitPrice`, not a scalar top-level price. Step 5's example outputs used `RadiationSystem`/`AccumulatedDose` and `NeedsSystem`/`Survivors[2].Hunger`, neither of which matches a real type or field — the closest real analogues are `SurvivorRadState` (no confirmed top-level save boundary) and `DoseLedgerSystem`/`DoseLedgerSystemState.entries[...].cumulativeMsv` (which does have a confirmed, deep-copying `CaptureState`/`RestoreState` pair). Fixed by replacing every fabricated name with a verified real one, or flagging it as unconfirmed where no clear replacement exists.

7. **Inconsistent, unreconciled "82+ systems" vs. "22 save stores" vs. reality.** The Motivation/Dependencies section cites "82+ systems" (an AGENTS.md figure for in-memory `CaptureState`/`RestoreState` implementers) while Step 4 and Step 6 cite "22 save stores" for the on-disk save-store population — two different, differently-sized things treated as interchangeable within the same document. `grep -rl "public static class .*SaveStore" src/` verifies **30** files for the on-disk population, matching the same correction Batch 101 required (which had the same "22" error). Fixed throughout by using the verified 30 and requiring the document to explicitly state which population ("all in-memory systems" vs. "all on-disk save stores") a given step actually targets, since `FullSaveDiffer.Compare`'s own signature (`IReadOnlyDictionary<string, object>` of system-name → state-object, meant to represent "two full save snapshots") most naturally corresponds to the smaller, on-disk population.

8. **`sim.TickSimDay()` does not exist as a callable public API — this is a real integration-breaking error, not cosmetic.** Verified in `src/Main.cs`: `TickSimDay` is `private void TickSimDay(int day)` on `AtomicWar.GodotApp.Main`, not a parameterless public method on some `sim` object. More significantly: `Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs` already exists, and its own doc comment describes it as the "Single authority for advancing the campaign by exactly one in-game day," explicitly already doing "capture pre-day snapshots in deterministic owner order" and "collect typed state-change events per owner" — functionality that substantially overlaps with what Step 6's `DayAdvanceDiffCapture` proposes to build from scratch, with no mention of `CampaignDayCoordinator` anywhere in the original document. This was the most significant scope-and-duplication finding of this review. Fixed by rewriting Step 6's goal, wiring example, and risk rating to require investigating and integrating with `CampaignDayCoordinator` before building a parallel mechanism, and by elevating this batch's overall risk rating for Step 6 specifically from "Low" to "Elevated."

### Structural / process issues found and fixed

9. **No Risk/Rollback section existed anywhere in the original document**, despite the header's own "Estimated Scope" line implying multi-step, multi-file work. Given finding #8 (potential duplication of `CampaignDayCoordinator`), this is not a low-risk feature end-to-end even though Steps 1-5 genuinely are additive and safe. Added a full Risk/Rollback section distinguishing the safe, purely-additive Steps 1-5 from the riskier Step 6 integration point, with a concrete rollback path for each.

10. **Vague, unfalsifiable Done-when criteria replaced.** "Handles all DTO types used by the 82+ systems," "22+ tests pass covering all diff scenarios," and "Performance within budget" are not independently checkable — they describe aspiration, not a pass/fail gate. Fixed by tying each Done-when bullet to the specific, named test list already present in Step 7's own code sketch, and by requiring `Stopwatch`-measured thresholds rather than prose assurance.

11. **Missing dependency on Batch 101.** The two batches under review both touch reflection-based traversal of the same DTOs (`SaveChecksum`-adjacent). Batch 101 proposes migrating DTOs to `record` types; Batch 102's `DeepDiffEngine` would silently stop working correctly on any DTO so migrated, for the same field-vs-property reason `SaveChecksum` itself breaks (per Batch 101's own review). This cross-batch dependency was not stated in either original document. Fixed by adding it to Batch 102's Dependencies line and Risk/Rollback section; the equivalent note has been added to Batch 101's Review Notes for symmetry.

### What was verified true and left unchanged

- The core motivation (no structured field-level diff tool exists today; a codebase-wide search for `StateDiff`, `FieldChange`, or a `Diffing` directory under `Assets/Ashfall.Core/` found none) is accurate.
- `SaveChecksum`'s null-string-as-empty and null-collection-as-empty normalizations, and its `[NonSerialized]` skip behavior, are real and accurately described — though no real DTO in the codebase currently uses `[NonSerialized]`, so the corresponding test should be treated as synthetic-only coverage, not a real-world scenario check.
- The general shape of the proposed `StateDiff<T>`/`FieldChange`/`ChangeKind` data model (Step 1) is sound, engine-agnostic, and does not depend on any of the corrections above — it was left unchanged.
- `IJsonSerializer`/`SystemTextJsonSerializer` exists and is usable for the `StateCloner`-style deep-clone pattern referenced in Step 3, consistent with `HostDefaults.cs`.
