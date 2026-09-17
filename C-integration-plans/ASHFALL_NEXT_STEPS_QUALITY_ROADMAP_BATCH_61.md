# ASHFALL — Quality Roadmap Batch 61

## Theme: Save Migration Framework — Versioned Codec Pipeline for the Host's Save Stores

**Priority:** HIGH (prevents save corruption on format changes)
**Risk:** Medium — changes save/load paths
**Batch:** 61
**Depends on:** Existing checksummed envelope pattern; 11 pre-existing static `*SaveCodec` classes as reference implementations (`HoldfastSaveCodec` V5, `YearOfAshSaveCodec` V3, `DoseLedgerSaveCodec` V2, `DailyBriefingSaveCodec` V1, `ExpansionHubSaveCodec` V4, `RadioSaveCodec` V1, `VerdictSaveCodec` V3, `MedicalWardSaveCodec` V1, `DutyRosterSaveCodec` V3, `ShelterAssignmentSaveCodec` V1, `PowerGridSaveCodec` V1 — corrected list, see Context)

---

## Context

**Re-verified against the actual codebase; the plan's own prior "Review Notes" correction pass (below) was itself incomplete.** This pass found the first correction undercounted both the number of save stores AND — more importantly — the number of pre-existing codecs, and missed that the pre-existing codecs are architecturally incompatible with the instance-based `ISaveCodec<T>` interface this plan proposes to build. Specifics:

- **Save store count is 27, not 25 (and not 22).** Exhaustive search (`find . -iname "*SaveStore.cs"`, excluding `.uid` sidecars) across the *entire* repo, not just `src/Host/`, finds 27 files: 25 in `src/Host/`, plus `src/Journal/JournalSaveStore.cs` and **`src/YearOfAsh/YearOfAshSaveStore.cs`** — a third location the prior correction pass did not check even though it explicitly flagged `JournalSaveStore`'s non-`Host/` location as a risk. This is exactly the kind of miss that plan's own warning ("do not hardcode a count") was meant to prevent, and it happened anyway on the very next attempt. Treat 27 as a floor confirmed only for `*SaveStore.cs` filename matches; `SaveAll()` in `src/Main.cs` calls 29 `SaveXxx()` methods (verified: `SaveJournal` + 28 others, confirmed by counting call sites inside the `SaveAll()` body), and not every `SaveXxx()` method necessarily has a same-named `*SaveStore.cs` file (e.g. `YearOfAshSaveStore` is not called from `Main.cs`'s `SaveAll()` at all — Year of Ash appears to save through a separate host session path; confirm this during Step 3's audit rather than assuming `SaveAll()`'s 29 calls map 1:1 onto the 27 store files).
- **The plan's premise that only 3 codecs exist is false.** Direct symbol search across `Assets/Ashfall.Core/` finds **11 pre-existing `*SaveCodec` types**, not 3: `HoldfastSaveCodec` (V5), `YearOfAshSaveCodec` (V3), `DoseLedgerSaveCodec` (V2), `DailyBriefingSaveCodec` (V1), `ExpansionHubSaveCodec` (V4), `RadioSaveCodec` (V1), `VerdictSaveCodec` (V3), `MedicalWardSaveCodec` (V1), `DutyRosterSaveCodec` (V3), `ShelterAssignmentSaveCodec` (V1), `PowerGridSaveCodec` (V1). `CurrentSaveVersion` values confirmed by reading each source file directly (e.g. `Assets/Ashfall.Core/Shelter/PowerGridSave.cs:14`, `Assets/Ashfall.Core/ExpansionHubSave.cs:27`). This changes the shape of the whole batch: instead of "3 codecs exist, ~19-22 need stubs," the real state is "11 codecs exist, up to 16 stores (27 minus the 11 whose domain already has a codec type, modulo the `SaveAll()`-vs-file-count discrepancy above) have no codec at all" — Step 3's audit must establish the exact remainder, not assume it.
- **Architectural incompatibility, not just a count error:** every one of these 11 existing codecs (confirmed by reading `PowerGridSaveCodec`, `HoldfastSaveCodec`, `YearOfAshSaveCodec`, `DoseLedgerSaveCodec` in full) is a **`public static class`** with `static Encode(...)`/`static Decode(...)` methods taking an explicit `IJsonSerializer` parameter and computing/validating a `SaveChecksum` inline — not an instance class implementing an interface. `MemorialSave.cs` (`Assets/Ashfall.Core/Memorial/MemorialSave.cs`) has a save DTO with `CurrentSaveVersion = 1` but **no `MemorialSaveCodec` type at all** — the store (`MemorialSaveStore.cs`) must serialize the DTO some other way; confirm during Step 3. Retrofitting 11 static-method codec classes to instead be instances of an abstract `SaveCodecPipeline<T> : ISaveCodec<T>` base class (Steps 1, 2, and 4 as originally written) is a bigger, more invasive rewrite than "register N migrations" — every call site (`XxxSaveStore.TrySave`/`TryLoad`, and any direct callers like `src/Host/HostCli.SelfTests.cs` which calls `YearOfAshSaveCodec.Encode/Decode` statically) must change from static calls to instance construction/injection. This is corrected in Steps 1, 2, and 4 below: the pipeline is designed as a composable helper the existing static codecs can *call into* for the migration-chain part only, rather than a base class they must inherit from — preserving the existing static call-site contract and avoiding an unscoped, repo-wide call-site rewrite.

**Action required before Step 3 begins:** run `find . -iname "*SaveStore.cs" -not -name "*.uid"` across the whole repo (not just `src/Host/`) AND cross-reference every `SaveXxx()` call inside `src/Main.cs`'s `SaveAll()` AND grep `Assets/Ashfall.Core/` for `class \w+SaveCodec\b` to produce three definitive, exhaustive lists (stores, `SaveAll()` call sites, existing codecs) before hardcoding any count into test assertions (see Step 7). Treat every number in this document as a floor pending that audit, including this section's own "27" and "11."

Without a universal, shared migration-chain pattern, any schema change to the stores that currently have no codec (or whose codec is a bare identity pass-through) risks silent data loss or load failures. The 11 existing codecs each implement their own migration logic with duplicated boilerplate (checksum compute/validate, version-range checks, malformed-JSON error wrapping) and inconsistent error message formats — this part of the original motivation is accurate. The fix is a shared migration-chain helper the existing static codecs opt into, not a mandatory base-class inheritance that would force rewriting 11 already-working, tested codecs and their call sites in one batch.

---

## Step 1 — Design a Migration-Chain Helper (Not a Mandatory Base Interface)

**Goal:** Define a reusable migration-chain helper that the 11 pre-existing `static class XxxSaveCodec` types can opt into for the ordered-migration part of their logic only, without requiring them to change from static methods to instances or to implement a shared interface. Also define the same contract for new codecs going forward.

**Correction from original design:** the original Step 1 proposed `ISaveCodec<TState>` as an *interface* with instance members (`CurrentVersion` property, `Migrate` instance method, etc.), and Step 2 proposed an *abstract base class* `SaveCodecPipeline<TState>` that codecs would inherit from. Both assume the 3 "reference" codecs are instance classes. They are not: all 11 pre-existing codecs (`HoldfastSaveCodec`, `YearOfAshSaveCodec`, `DoseLedgerSaveCodec`, `DailyBriefingSaveCodec`, `ExpansionHubSaveCodec`, `RadioSaveCodec`, `VerdictSaveCodec`, `MedicalWardSaveCodec`, `DutyRosterSaveCodec`, `ShelterAssignmentSaveCodec`, `PowerGridSaveCodec`) are `public static class` types with `static Encode(TState, IJsonSerializer)` / `static Decode(string, IJsonSerializer)` methods that also inline their own checksum validation and version-range checks (confirmed by reading `PowerGridSaveCodec`, `HoldfastSaveCodec` in full). Forcing all 11 to become instances of an abstract base class means rewriting every call site across the host (`XxxSaveStore.TrySave`/`TryLoad`, plus direct static callers such as `src/Host/HostCli.SelfTests.cs` which calls `YearOfAshSaveCodec.Encode`/`Decode` statically) — a much larger, higher-risk blast radius than this batch's stated goal, and *not* something the original plan's risk section accounted for. The design below keeps the existing static-call-site contract intact.

**Implementation:**
- Create `Assets/Ashfall.Core/Save/SaveMigrationChain.cs` with a small **stateless static helper**, not an interface codecs must implement:
  ```csharp
  public static class SaveMigrationChain
  {
      // Applies ordered migration steps to a raw deserialized object graph until
      // it reaches targetVersion. Existing static codecs call this from inside
      // their own Decode() method instead of hand-rolling a version-check ladder.
      public static TState Migrate<TState>(
          TState state,
          int fromVersion,
          int targetVersion,
          IReadOnlyList<(int fromVersion, Func<TState, TState> step)> migrations);
  }
  ```
- Define `SaveMigrationStep<TState>` delegate: `TState Migrate(TState previous)` — unchanged from original design, still useful as a named type for the `migrations` list above.
- Define `SaveCodecResult<T>` envelope: `{ T State, int OriginalVersion, int FinalVersion, bool WasMigrated }` — returned by the helper for codecs that want structured migration metadata (optional; existing codecs are not required to adopt this return shape immediately).
- Define `SaveMigrationException` (moved up from Step 2) alongside the helper: thrown by `SaveMigrationChain.Migrate` on a missing/out-of-order migration step or future-version input, with step index and from/to versions in the message.
- Document version detection strategy: existing codecs already read a `saveVersion` int field from the deserialized DTO (confirmed: `PowerGridSave.saveVersion`, `HoldfastSave.saveVersion`, etc.) rather than sniffing raw JSON — the helper takes the already-deserialized `fromVersion` as a plain `int` parameter rather than re-implementing JSON version-sniffing, matching the existing pattern instead of replacing it.
- No `UnityEngine.*`, no `Godot.*` — pure C# in Core.
- **New codecs only** (i.e. codecs for stores that currently have none, per Step 5) may optionally be written as instances implementing a lightweight `ISaveCodec<TState>` marker interface if that's convenient for that specific store, but this is not required and the 11 existing codecs are NOT retrofitted to it — see Step 4's corrected scope.

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # compiles
```

**Done when:** `SaveMigrationChain`, `SaveMigrationStep<T>`, `SaveCodecResult<T>`, and `SaveMigrationException` exist in `Assets/Ashfall.Core/Save/`, compile cleanly, have XML doc comments, and neither requires any existing `static class XxxSaveCodec` to change its public signature to adopt the helper.

---

## Step 2 — Implement SaveMigrationChainBuilder Helper in Core

**Goal:** Provide a small, optional-adoption builder that chains migration steps, validates version ordering, and produces audit-friendly results — usable from inside an existing static `Decode()` method without changing that method's signature.

**Correction from original design:** the original Step 2 proposed `SaveCodecPipeline<TState> : ISaveCodec<TState>`, an abstract base class codecs must inherit from. Since all 11 existing codecs are static classes (see Step 1's correction), "inherit from an abstract base" is not applicable to them without a call-site rewrite this batch does not have license to do unscoped. Replaced with a builder object that a static codec constructs locally inside its own `Decode()` method:

**Implementation:**
- Create `Assets/Ashfall.Core/Save/SaveMigrationChainBuilder.cs`:
  ```csharp
  public sealed class SaveMigrationChainBuilder<TState>
  {
      private readonly List<(int fromVersion, Func<TState, TState> step)> _migrations = new();

      public SaveMigrationChainBuilder<TState> RegisterMigration(int fromVersion, Func<TState, TState> step)
      {
          _migrations.Add((fromVersion, step));
          return this;
      }

      // Applies registered steps in order starting at `fromVersion` up to `targetVersion`.
      // Throws SaveMigrationException if a required step is missing or a step throws.
      public TState Run(TState state, int fromVersion, int targetVersion);
  }
  ```
- A static codec adopts this by building the chain once (e.g. as a `static readonly` field or inline in `Decode`) and calling `.Run(deserialized, save.saveVersion, CurrentSaveVersion)` — no change to the codec's own `public static Decode(string, IJsonSerializer)` signature, no change to callers.
- Error handling: wrap each step in try/catch; on failure, throw `SaveMigrationException` (defined in Step 1) with step index, from/to versions, and inner exception.
- Rollback: explicitly **out of scope for this batch** — none of the 11 existing codecs support rollback today (confirmed: all reject `saveVersion > CurrentSaveVersion` by throwing, forward-only), and adding rollback semantics to a shared helper without any existing codec exercising it is speculative scope creep. If a future batch needs rollback, it should be scoped and designed against a real requirement, not added here as an unused option.

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "SaveMigrationChain"
```

**Done when:** `SaveMigrationChainBuilder<T>` compiles, has at least one unit test proving a two-step migration chain runs in order, throws `SaveMigrationException` on step failure, and — critically — a test proves an existing static codec pattern (a minimal reproduction, not a production codec) can adopt it via local construction inside a static method without changing that method's public signature.

---

## Step 3 — Exhaustive Store/Codec Audit, Then Add Version Field to the Stores That Lack One

**Goal:** Produce the definitive store/codec inventory this batch depends on, then ensure every save envelope written by the game includes a `saveVersion`/`Version` field at the root — for the codecs that don't already do this.

**Correction:** the original Step 3 assumed nearly every store needs a new `Version` field added. That's false for at least 11 domains: `HoldfastSave`, `YearOfAshSave`, `DoseLedgerSave`, `DailyBriefingSave`, `ExpansionHubSave`, `RadioSave`, `VerdictSave`, `MedicalWardSave`, `DutyRosterSave`, `ShelterAssignmentSave`, and `PowerGridSave` DTOs **already have a `public int saveVersion` field** written and checked on every save/load (confirmed by reading `PowerGridSave.cs:17`, `HoldfastSave.cs:25`, etc.), and every one of the 11 codecs already computes and validates a `Checksum` field (confirmed: `PowerGridSaveCodec.Encode` calls `SaveChecksum.Compute`; `Decode` rejects mismatched or missing checksums). Introducing a new, separate `VersionedSaveEnvelope<T>` wrapper type as originally proposed would create a **second, parallel envelope shape** alongside the 11 existing ones rather than unifying them — this is corrected below to reuse the existing per-domain `saveVersion`/`Checksum` field convention instead of inventing a new generic wrapper.

**Implementation:**
- **First action in this step:** mechanically produce three exhaustive lists (do not proceed against a guessed number):
  1. Every `*SaveStore.cs` file repo-wide: `find . -iname "*SaveStore.cs" -not -name "*.uid"` (confirmed floor: 27, across `src/Host/`, `src/Journal/`, and `src/YearOfAsh/` — re-run this yourself, do not trust this document's number without re-checking, since a fourth location is exactly the kind of miss this document's own review found once already).
  2. Every `SaveXxx()` method called from `src/Main.cs`'s `SaveAll()` (confirmed floor: 29 call sites at the time of this review) — cross-reference against list 1; note discrepancies explicitly (e.g. `YearOfAshSaveStore` is not called from `Main.cs`'s `SaveAll()` — Year of Ash appears to persist via a separate host-session path (`src/YearOfAsh/YearOfAshHostSession.cs`), not the main save-all sweep; confirm and document this rather than assuming full 1:1 coverage).
  3. Every existing `class \w+SaveCodec` under `Assets/Ashfall.Core/` (confirmed floor: 11, listed in Context above) — for each, record whether it already has a version field + checksum (all 11 confirmed yes) or is missing entirely for a domain that has a save DTO but no codec (confirmed example: `MemorialSave.cs` has `CurrentSaveVersion = 1` but **no `MemorialSaveCodec` type exists** — `MemorialSaveStore` must serialize the DTO through some other path; find and document it).
- For domains identified in list 3 that have neither a version field nor a codec (the true "unversioned" set — re-derive the exact count and names from lists 1–3, do not assume a number from a prior draft of this document), add a `saveVersion` int field to that domain's save DTO (matching the existing per-domain convention, not a new generic `VersionedSaveEnvelope<T>` type) plus a `Checksum` string field, following the exact pattern already used by e.g. `PowerGridSave`/`PowerGridSaveCodec`.
- Legacy fallback: if `saveVersion` is absent (field missing from older JSON), treat as V1 (identity, no migration needed) — this matches the existing `MigrationFromVersion` convention seen in `PowerGridSave.cs`.
- Maintain backward compatibility: saves without a version field still load via the existing bare-state fallback path already present in each store.
- **Rollback note:** this step writes an additive field only to domains that currently lack one — a save written after this step still loads correctly in a build that predates it (older code simply doesn't read the new field). Rolling back requires no data migration. Confirm this by round-tripping a save through both the old and new build during manual testing before merging.

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --bridge-selftest
```

**Done when:** The three exhaustive lists (stores, `SaveAll()` call sites, existing codecs) are recorded in `docs/save-codec-audit.md` with exact counts and file names. Every domain identified as truly unversioned now has a `saveVersion` + `Checksum` field on its save DTO, following the existing per-domain convention (no new generic envelope type introduced). Old saves without a version field still load successfully (verified with an actual pre-existing save file from before this change, not just a synthetic one). Zero test regressions.

---

## Step 4 — Adopt the Migration-Chain Helper Inside Existing Codecs (No Inheritance Change)

**Goal:** Have `HoldfastSaveCodec`, `YearOfAshSaveCodec`, and `DoseLedgerSaveCodec` — the three with genuine multi-step migration chains — call `SaveMigrationChainBuilder<T>` internally from their existing static `Decode` methods, eliminating bespoke version-ladder logic, **without changing their public static signature or forcing callers to change.**

**Correction:** the original Step 4 said to replace these three codecs with new types (`HoldfastSaveCodecV2 : SaveCodecPipeline<...>`, etc.) that callers would have to switch to. Since all three are `public static class` types with call sites elsewhere in the host (`XxxSaveStore.TrySave`/`TryLoad`, and direct static callers such as `src/Host/HostCli.SelfTests.cs`'s calls to `YearOfAshSaveCodec.Encode`/`Decode`), introducing a `V2` type would require finding and updating every call site — a repo-wide rewrite this batch did not scope or budget for. The corrected approach: keep `HoldfastSaveCodec`, `YearOfAshSaveCodec`, `DoseLedgerSaveCodec` as the same static classes with the same public method signatures; internally, their `Decode` method builds a `SaveMigrationChainBuilder<TState>` and calls `.Run(...)` instead of a hand-written if/else version ladder. Zero call sites change.

Verified version counts (unchanged from Context section): Holdfast is V1→V2→V3→V4→V5 (4 migration steps), YearOfAsh is V1→V2→V3 (2 steps), DoseLedger is V1→V2 (1 step). Each retrofit registers the correct number of migrations for that codec specifically — do not copy a uniform template across all three.

**Implementation:**
- Inside `HoldfastSaveCodec.Decode` (`Assets/Ashfall.Core/HoldfastSave.cs`): replace the existing version-ladder branches with a `SaveMigrationChainBuilder<HoldfastSave>` registering 4 steps (V1→V2, V2→V3, V3→V4, V4→V5), built once and called via `.Run(deserialized, save.saveVersion, HoldfastSave.CurrentSaveVersion)`. Preserve the existing migration logic verbatim as the lambda bodies — this is a mechanical extraction, not a rewrite of the migration semantics.
- Inside `YearOfAshSaveCodec.Decode` (`Assets/Ashfall.Core/YearOfAsh/YearOfAshSave.cs`): register 2 migrations (V1→V2, V2→V3).
- Inside `DoseLedgerSaveCodec.Decode` (`Assets/Ashfall.Core/DoseLedgerSave.cs`): register 1 migration (V1→V2).
- The public `Encode`/`Decode` static method signatures, their checksum validation, and their exception messages for malformed/future-version input are preserved exactly — only the *internal* version-ladder mechanism changes from hand-rolled branches to the shared builder. This is verifiable by diffing the public API surface (should be empty) versus the method bodies (should show the extraction).
- **Do not** mark anything `[Obsolete]` or create `V2` type names — there is no old/new type pair in this corrected design, just an internal refactor of three existing methods.
- **Risk:** `HoldfastSaveCodec` is the deepest migration chain in the codebase (5 versions) and has existing test coverage in `Ashfall.Core.Tests/HoldfastSaveTests.cs` and `Ashfall.Core.Tests/District8DeepCoastTests.cs` that exercises specific version-migration behavior (e.g. `District8DeepCoastTests.cs` asserts a v4 JSON payload migrates correctly). Retrofitting this codec is the highest-risk item in this step; do it last, after the YearOfAsh and DoseLedger retrofits have proven the builder pattern works, and run the full test suite after each of the three individually before moving to the next.

**Verification:**
```
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "Holdfast|YearOfAsh|DoseLedger"
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # full suite
```

**Done when:** All three codecs' `Decode` methods internally use `SaveMigrationChainBuilder<T>` with the correct, version-accurate number of registered migrations (4 for Holdfast, 2 for YearOfAsh, 1 for DoseLedger). Public method signatures are unchanged (verify: `grep -n "public static" HoldfastSave.cs` before/after shows an identical signature list). Existing tests (`HoldfastSaveTests.cs`, `District8DeepCoastTests.cs`, `YearOfAshTests.cs`, `DoseLedgerSystemTests.cs`, `DoseQuestOwnershipTests.cs`) pass without modification — this is the key regression check proving the refactor is behavior-preserving. No caller (including `HostCli.SelfTests.cs`) needs any change.

---

## Step 5 — Add Codec Stubs (V1 Identity) for the Remaining Stores That Have None

**Goal:** Every save store has an explicit, discoverable codec, even if it currently performs no migration (identity V1). This ensures future migrations have a registration point, following the same static-class convention as the 11 existing codecs rather than introducing a second, inconsistent pattern.

**Correction:** the original Step 5 said "≈22 unversioned stores" based on a 25-store, 3-codec baseline. The corrected baseline (see Context and Step 3's audit) is: 27 stores total, 11 of which already have a codec (`Holdfast`, `YearOfAsh`, `DoseLedger`, `DailyBriefing`, `ExpansionHub`, `Radio`, `Verdict`, `MedicalWard`, `DutyRoster`, `ShelterAssignment`, `PowerGrid`), leaving up to 16 without one — but this number must be re-derived from Step 3's exhaustive audit, not assumed, since the `SaveAll()`-vs-store-file discrepancy (YearOfAsh not called from `SaveAll()`) means the mapping between "stores" and "domains needing a codec" is not perfectly 1:1. Additionally, `MemorialSave.cs` has a version field but **no codec at all** — that domain needs a codec created even though it's not fully "unversioned."
- **New stub codecs follow the existing static-class convention**, not a new instance-based pattern: `public static class <SystemName>SaveCodec` with `static Encode(<SystemName>Save, IJsonSerializer)` / `static Decode(string, IJsonSerializer)`, matching `PowerGridSaveCodec`'s shape exactly (checksum compute on encode, checksum validate + version-range check on decode). This keeps all codecs — old and new — consistent, rather than having 11 static ones and N instance-based ones side by side.

**Implementation:**
- For each store identified in the Step 3 audit as lacking a codec, create:
  ```
  Assets/Ashfall.Core/<Domain>/<SystemName>SaveCodec.cs   (co-located with its Save DTO, matching existing layout — e.g. Assets/Ashfall.Core/Shelter/PowerGridSave.cs, not a new centralized Save/Codecs/ folder that would split the convention)
  ```
  Example, matching the real `PowerGridSaveCodec` shape:
  ```csharp
  public static class InventorySaveCodec
  {
      public static string EncodeToString(InventorySave save, IJsonSerializer json) { /* compute checksum, serialize */ }
      public static InventorySave Decode(string jsonText, IJsonSerializer json) { /* validate checksum, version check, deserialize */ }
      // No migrations registered yet — V1 identity.
      // When schema changes: build a SaveMigrationChainBuilder<InventorySave> inside Decode and register steps.
  }
  ```
- Wire each new codec into its corresponding save store's `TryLoad`/`TrySave`, replacing whatever ad hoc serialization it currently uses.
- Naming convention: `<Domain>SaveCodec`, co-located with the domain's existing files (matches current layout — do not introduce a new `Ashfall.Core.Save.Codecs` namespace/folder that no existing codec uses).
- Each new codec file includes a comment block documenting the save file name and current state shape, matching the doc-comment style already used in `PowerGridSave.cs`.

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
```

**Done when:** One codec class exists per store discovered in Step 3's audit as lacking one (11 pre-existing + N new stubs + the 1 Memorial gap-fill, where N is the audited count, not a hardcoded 22 or 19). All compile, follow the existing static-class convention, and are co-located with their domain's Save DTO. All save stores route through a codec. No test regressions.

---

## Step 6 — Write Migration Chain Tests

**Goal:** Prove the pipeline correctly chains multi-step migrations for representative stores, including edge cases (corrupt data, unknown future version, skipped versions).

**Implementation:**
- Create `Ashfall.Core.Tests/SaveCodecPipelineTests.cs` with:
  - **Test group A — InventorySaveCodec (simulated V1→V2):**
    - Register a fake V1→V2 migration that adds a new field with a default value.
    - Assert: V1 JSON input produces V2 state with the default populated.
    - Assert: V2 JSON input passes through unchanged.
  - **Test group B — EconomySaveCodec (simulated V1→V2→V3):**
    - Two chained migrations: rename a field (V1→V2), split a field into two (V2→V3).
    - Assert: V1 input correctly migrates through both steps to V3.
    - Assert: `SaveCodecResult.WasMigrated == true`, `OriginalVersion == 1`, `FinalVersion == 3`.
  - **Test group C — Error cases:**
    - Future version (higher than `CurrentVersion`) → throws `SaveMigrationException`.
    - Corrupt JSON (not deserializable) → throws with meaningful message.
    - Null/empty input → throws `ArgumentException`.
  - **Test group D — Round-trip:**
    - For each of the 3 representative codecs: `Serialize(Migrate(raw, 1))` produces valid JSON that `DetectVersion` returns `CurrentVersion`.

**Verification:**
```
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "SaveCodecPipeline"
```

## Step 6 — Write Migration Chain Tests

**Goal:** Prove the migration-chain builder correctly chains multi-step migrations for representative domains, including edge cases (corrupt data, unknown future version, skipped versions).

**Correction:** test names below referenced `InventorySaveCodec`/`EconomySaveCodec` as if they'd become pipeline-based subclasses. Since new stub codecs (Step 5) are static classes matching the existing convention, these tests exercise `SaveMigrationChainBuilder<T>` directly with synthetic state types, and separately exercise one real adopted codec (from Step 4) to prove the builder integrates correctly in practice — this is a more accurate test structure than testing against `InventorySaveCodec`'s absent instance-based API.

**Implementation:**
- Create `Ashfall.Core.Tests/SaveMigrationChainTests.cs` with:
  - **Test group A — Synthetic V1→V2 (builder unit test):**
    - Register a fake V1→V2 migration that adds a new field with a default value, using a plain test-only `TState` (not tied to any production save DTO).
    - Assert: V1 input produces V2 state with the default populated.
    - Assert: V2 input passes through unchanged (`Run` with `fromVersion == targetVersion` is a no-op).
  - **Test group B — Synthetic V1→V2→V3 (chained):**
    - Two chained migrations: rename a field (V1→V2), split a field into two (V2→V3).
    - Assert: V1 input correctly migrates through both steps to V3.
    - Assert: `SaveCodecResult.WasMigrated == true`, `OriginalVersion == 1`, `FinalVersion == 3` (if the codec under test opts into returning this result shape).
  - **Test group C — Error cases:**
    - Future version (higher than `targetVersion`) → throws `SaveMigrationException`.
    - Missing/out-of-order registered step for the requested `fromVersion` → throws `SaveMigrationException` naming the gap.
    - Null state input → throws `ArgumentNullException`.
  - **Test group D — Real-codec integration (proves Step 4 adoption works):**
    - For `DoseLedgerSaveCodec` (1 migration, lowest risk): construct a V1 JSON payload, call the real `Decode`, assert the result is V2 with expected defaults, and assert the checksum validates.
    - For `YearOfAshSaveCodec` (2 migrations): same shape, asserting a V1 payload migrates through to V3.
    - Do not include `HoldfastSaveCodec` in this new test file — its existing dedicated test files (`HoldfastSaveTests.cs`, `District8DeepCoastTests.cs`) already cover its 4-step chain in depth; re-asserting the same behavior here would be redundant coverage, not new signal.

**Verification:**
```
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "SaveMigrationChain"
```

**Done when:** At least 12 tests pass covering groups A–D. Migration chains execute in order. Error cases throw expected exceptions. Group D proves at least 2 real, already-shipped codecs work correctly after Step 4's internal refactor.

---

## Step 7 — Add Save-Format-Version CI Gate

**Goal:** Prevent a save domain from silently losing its version/checksum discipline. Any save codec that writes JSON without a `saveVersion`/`Checksum` field fails CI.

**Correction:** the original Step 7 said to reflect over "all types implementing `ISaveCodec<T>`" — that interface no longer exists in this corrected design (Step 1 replaced it with a stateless helper, not a marker interface every codec implements), and even if it did, 11 of the codecs are static classes with no instantiable type to reflect-construct. Rewritten to check the actual shape that exists.

**Implementation:**
- Create `Ashfall.Core.Tests/SaveVersionGateTests.cs`:
  - Maintain a single, explicitly named, documented list of `(domain name, save-DTO type, codec type)` tuples — sourced directly from Step 3's audit document (`docs/save-codec-audit.md`), not a hardcoded literal count. The list itself is the "source of truth" file this test reads or mirrors; a comment at the top must cite the exact command used to regenerate it (`grep -rln "class \w+SaveCodec\b" Assets/Ashfall.Core/ --include=*.cs`) so the list can be regenerated and diffed, not silently drift.
  - For each entry: use reflection to confirm the save-DTO type has a `saveVersion`/`CurrentSaveVersion` field or property, and that the codec type has `Encode`/`Decode` (or `EncodeToString`) static or instance methods.
  - For each entry: instantiate a default/minimal instance of the save DTO, round-trip it through `Encode`→`Decode`, and assert the version and checksum fields are present and non-empty in the serialized output.
- Create `Ashfall.Core.Tests/SaveEnvelopeFormatTests.cs`:
  - For each save store (from the Step 3 audit's store list), invoke `TrySave` with a default state, deserialize the output, assert the version field is present and equals the codec's `CurrentSaveVersion`.
  - Assert the checksum field is present and non-empty.
- Add a comment in `scripts/ci/` (or equivalent CI config) documenting that `dotnet test --filter "SaveVersionGate|SaveEnvelopeFormat"` must pass before merge.
- If any store/codec pair is found writing without a version or checksum field, the test fails with a message naming the offending domain.

**Verification:**
```
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "SaveVersionGate|SaveEnvelopeFormat"
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # full suite
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
```

**Done when:** CI gate tests pass. Removing a version or checksum field from any domain's codec causes a named test failure. Adding a new save domain without updating the documented, regeneratable tuple list causes a named test failure (not a silent pass, and not a test requiring a hardcoded count edit — see implementation note above). All 5 verification steps pass.

---

## Summary

| Step | Deliverable | Location | Test Coverage |
|------|-------------|----------|---------------|
| 1 | `SaveMigrationChain`, `SaveMigrationStep<T>`, `SaveCodecResult<T>`, `SaveMigrationException` | `Assets/Ashfall.Core/Save/` | compile-only |
| 2 | `SaveMigrationChainBuilder<T>` helper (no inheritance requirement) | `Assets/Ashfall.Core/Save/` | unit tests (chain + error) |
| 3 | Exhaustive store/codec/`SaveAll()` audit (`docs/save-codec-audit.md`) + version field added only to domains that lack one | `Assets/Ashfall.Core/<Domain>/` (co-located, no new generic envelope type) | round-trip + legacy fallback |
| 4 | Internal builder-adoption inside Holdfast (V1→V5, 4 migrations)/YearOfAsh (V1→V3, 2 migrations)/DoseLedger (V1→V2, 1 migration) — public API unchanged | `Assets/Ashfall.Core/HoldfastSave.cs`, `YearOfAsh/YearOfAshSave.cs`, `DoseLedgerSave.cs` | existing codec tests pass unmodified, including `District8DeepCoastTests.cs` (v4→v5 migration) |
| 5 | New static-class identity codecs for domains with none (count = audited total minus 11, plus the Memorial gap-fill) | co-located with each domain's Save DTO | compile + integration |
| 6 | Migration chain tests (12+ tests, 4 groups, including 2 real-codec integration checks) | `Ashfall.Core.Tests/SaveMigrationChainTests.cs` | A–D groups pass |
| 7 | CI gate (documented tuple list + envelope format assertions, no `ISaveCodec<T>` reflection since that interface doesn't exist in this design) | `Ashfall.Core.Tests/SaveVersionGateTests.cs` | named failures on violation |

**Total estimated effort:** 5–6 focused sessions (revised up again from 4–5 — the corrected design requires an internal-refactor pass on 3 already-tested codecs plus new codecs for up to 16 domains rather than 3 clean retrofits + ~19 trivial stubs; the per-domain "co-locate with existing file" requirement in Steps 3 and 5 also means touching more distinct files than the original centralized-`Save/Codecs/`-folder design implied)
**Risk mitigation:** Step 1 is pure additive (no existing code changes). Step 2 is additive (new helper type, not yet wired to anything). Step 3 requires the exhaustive audit before any store is touched; the version/checksum field addition is backward-compatible and additive-only for domains that lack it. Step 4 is an internal-only refactor of 3 already-tested codecs with an explicit "public signature must not change" acceptance test — treat Holdfast (5 versions, existing v4-migration test coverage, highest blast radius if the refactor introduces a subtle behavior change) as the highest-risk item and do it last, one codec at a time with full-suite verification between each. Step 5 is mechanical once Step 3's audit is final, and safer than the original design because it doesn't touch the 11 already-shipped codecs. Step 6 proves correctness. Step 7 prevents regression without a hardcoded magic number or a reflection scheme that doesn't fit the actual (mixed static/instance) codec shapes in this codebase.
**Rollback:** Steps 1–2 can be deleted wholesale with no impact (nothing depends on them yet). Step 3's field additions are additive-only — rollback is "ignore the new field," no data migration. Step 4's internal refactor is the step most likely to need a revert: commit each of the 3 codec's internal refactors separately (DoseLedger, then YearOfAsh, then Holdfast) so a subtle regression in one can be `git revert`-ed without unpicking the other two from a combined diff. Step 5's new codecs are additive per-domain; a broken new codec can be reverted per-file without affecting the 11 pre-existing ones since none of them share a base type.

---

## Review Notes (Corrected)

This batch was adversarially reviewed against the actual codebase at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`. This document already contained a prior self-correction pass (store count 22→25, codec versions V1→V2→V3 uniform→non-uniform) before this review began; that prior pass is itself incomplete and is corrected further here, since it repeated the same category of error (undercounting via an incomplete search) that it had just flagged as a risk.

1. **Save store count — CORRECTED AGAIN, from 25 to 27 (still a floor).** The prior correction found 25 by checking only `src/Host/*SaveStore.cs` plus noting `JournalSaveStore.cs` lives in `src/Journal/`. An exhaustive repo-wide `find . -iname "*SaveStore.cs"` finds a **third** location the prior pass missed entirely: `src/YearOfAsh/YearOfAshSaveStore.cs`. This is the same class of mistake (checking a subset of locations) the prior pass had explicitly warned against, and it happened again. Additionally, `SaveAll()` in `src/Main.cs` calls **29** `SaveXxx()` methods (verified by direct count), not 28 as the prior pass stated, and `YearOfAshSaveStore` is not among the methods `SaveAll()` calls — Year of Ash persists through a separate host-session path (`YearOfAshHostSession`), which the prior pass's "cross-reference `SaveAll()`'s calls" action item would have caught if it had actually been executed rather than just recommended.

2. **The count of pre-existing codecs was wrong by more than 3x, and this is the most consequential finding in this review.** The prior pass's "Codec versions are not uniform" correction only re-examined the same 3 codecs the original plan named (`HoldfastSaveCodec`, `YearOfAshSaveCodec`, `DoseLedgerSaveCodec`) and corrected their version numbers, but never asked whether *other* codecs already existed beyond those 3. A direct symbol search of `Assets/Ashfall.Core/` finds **11** pre-existing `*SaveCodec` types: the 3 already named, plus `DailyBriefingSaveCodec` (V1), `ExpansionHubSaveCodec` (V4), `RadioSaveCodec` (V1), `VerdictSaveCodec` (V3), `MedicalWardSaveCodec` (V1), `DutyRosterSaveCodec` (V3), `ShelterAssignmentSaveCodec` (V1), and `PowerGridSaveCodec` (V1). This means the batch's core premise — "3 codecs exist as bespoke one-offs, ~19-22 stores need a codec built from scratch" — was wrong about the single most important number in the plan: how much of this problem is already solved.

3. **Architectural incompatibility, not just a count error — this changes Steps 1, 2, 4, and 5's designs, not just their numbers.** All 11 pre-existing codecs are confirmed (by reading `PowerGridSaveCodec`, `HoldfastSaveCodec` source in full) to be `public static class` types with `static Encode`/`static Decode` methods, checksum validation inlined, and no shared interface or base class. The original Steps 1–2 proposed an *instance-based* `ISaveCodec<TState>` interface and an *abstract base class* `SaveCodecPipeline<TState>` that codecs would inherit from — neither is compatible with 11 static classes without rewriting every call site across the host (store classes, plus direct static callers like `HostCli.SelfTests.cs`). This review rewrote Steps 1, 2, 4, and 5 to use a stateless `SaveMigrationChainBuilder<T>` helper that existing static codecs call *internally*, preserving their public static signatures and avoiding an unscoped, repo-wide call-site rewrite that the original plan's risk section never accounted for.

4. **`MemorialSave.cs` has a version field but no codec at all** — confirmed by direct read: `CurrentSaveVersion = 1` exists on the DTO, but no `MemorialSaveCodec` type exists anywhere in the tree. `MemorialSaveStore` must be serializing this domain some other way (direct `IJsonSerializer` call, presumably) — this is a genuine gap Step 5 must close, and neither the original plan nor the prior correction pass identified it, because neither actually enumerated the codec list exhaustively before writing the "3 exist, N are unversioned" framing.

5. **Codec file location — the prior correction's claim was right but incomplete.** Confirmed: all `*SaveCodec` types (all 11) live in `Assets/Ashfall.Core/`, co-located with their domain's Save DTO in the domain's own subfolder (`Shelter/PowerGridSave.cs`, `DutyRoster/DutyRosterSave.cs`, etc.) — not in a centralized `Save/Codecs/` folder as Steps 4–5 originally proposed. This review corrected Step 5 to follow the existing co-located convention rather than introducing a second, inconsistent file-layout pattern alongside the 11 existing ones.

6. **Hardcoded magic number in CI gate — still a real risk, now fixed against the corrected design.** Step 7's reflection scheme ("find all types implementing `ISaveCodec<T>`") no longer applies since that interface was removed from the design; rewritten to use an explicitly documented, regeneratable tuple list instead, which also resolves the original magic-number risk the prior pass had already (correctly) flagged.

7. **Ordering is logically sound and unchanged:** Steps 1–2 (helper types) before Step 3 (audit + envelope versioning for the gap domains) before Step 4 (internal adoption in the 3 multi-step codecs) before Step 5 (new codecs for domains with none) before Step 6 (tests) before Step 7 (CI gate) remains the correct dependency order under the corrected design.

8. **Process note for future review passes on this document:** this is the second correction pass, and it found the first correction pass under-verified in exactly the way the first pass's own text warned about (partial-location search, assumed-complete enumeration). Any future edit to this plan should re-run the full `find`/`grep` enumeration from scratch rather than trusting either this document's or the prior pass's stated counts — including this pass's own "27" and "11," which should be treated as re-verifiable floors, not permanent facts, since the codebase continues to change.
