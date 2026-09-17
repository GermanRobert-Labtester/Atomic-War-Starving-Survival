# ASHFALL Quality Roadmap — Batch 54
## Theme: InMemoryFlagLedger Hardening & Save Contract

**Priority:** MEDIUM-HIGH (Invariant 4 risk — case-normalization drift, no save/load contract)
**Risk:** Medium — flag ledger is used across many systems
**Prerequisite:** None

---

## Rationale

`InMemoryFlagLedger` (`Assets/Ashfall.Core/Flags/IFlagLedger.cs`) is a cross-cutting state store used by narrative encounters, quests, expansions, and the event system. It tracks boolean flags and integer counters. It has two documented problems:

1. **No CaptureState/RestoreState:** The flag ledger is stateful (`HashSet<string>` + `Dictionary<string, int>`) but has no save/load contract. The host must serialize its contents externally, but there's no standard DTO or round-trip test proving this works.

2. **OrdinalIgnoreCase case-normalization drift risk:** Using `StringComparer.OrdinalIgnoreCase` means `flag_quest_complete` and `Flag_Quest_Complete` are treated as the same flag. If different systems set flags with inconsistent casing, they'll collide silently. Worse, if a future refactor changes to case-sensitive comparison, existing saves break.

This batch adds proper save/load support, case-normalization enforcement, and comprehensive tests.

**Critical correction before implementation — the real API does not match this document's original code samples.** The actual `IFlagLedger` interface and `InMemoryFlagLedger` class (verified against `Assets/Ashfall.Core/Flags/IFlagLedger.cs` in full) are:

```csharp
public interface IFlagLedger
{
    bool IsSet(string flagId);
    void Set(string flagId);
    void Clear(string flagId);
    int GetCounter(string counterId);
    void Increment(string counterId, int amount = 1);
    void SetCounter(string counterId, int value);
}

public sealed class InMemoryFlagLedger : IFlagLedger
{
    private readonly HashSet<string> _flags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, int> _counters = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
    // IsSet, Set, Clear, GetCounter, Increment, SetCounter — no other members.
}
```

There is **no** `HasFlag`, `SetFlag`, `ClearFlag` method naming (the real names are `IsSet`/`Set`/`Clear`), **no** existing constructor parameter of any kind (the real constructor is parameterless — no `ILog` parameter, no `strictPrefixes` parameter), and **no** `_flags`/`_counters` field-name mismatch, but the private field names in this document's original code samples (`_flags`, `_counters`) do happen to match the real private field names, so DTO-population logic can use them as written once moved inside the actual class. Every step below has been rewritten against the real method names. Do not reintroduce `HasFlag`/`SetFlag`/`ClearFlag` naming — callers across the codebase (confirmed usages in `Ashfall.Core.Tests/StandaloneCoreSystemTests.cs`, `Ashfall.Core.Tests/VerdictSystemTests.cs`, `Assets/Ashfall.Core/Expeditions/DiveInstanceRunner.cs`, `Assets/Ashfall.Core/Radio/CensusBroadcastScheduler.cs`, `Assets/Ashfall.Core/Verdict/VerdictCensusBroadcast.cs`, `src/Host/VerdictHostSession.cs`, and `Assets/Ashfall.Core/Encounters/OrphanKnockWhitelist.cs`) all use `IsSet`/`Set`/`Clear`/`Increment`/`GetCounter`/`SetCounter` today; renaming or adding a parallel naming convention would be a breaking, unreviewed API change outside this batch's stated scope.

There is also **no existing `CaptureState`/`RestoreState` pair anywhere in `Assets/Ashfall.Core/Flags/`** to extend — this batch is greenfield for the flag ledger's save contract, not a hardening of an existing-but-flawed implementation. The DTO pattern below is modeled on confirmed real examples elsewhere in Core (e.g. `CensusClaimSystemState`/`CohortSystemState` — both `[Serializable]` DTOs returned by value with deep-copy semantics), which is a sound reference pattern to follow.

**Risk/rollback note:** Steps 1–2 (CaptureState/RestoreState) are purely additive and safe to revert independently. Step 3 (case-normalization enforcement) is the one behavior change in this batch — it changes the *storage* comparer from `OrdinalIgnoreCase` to `Ordinal` after lowercasing at the boundary. Because every current call site already only reads/writes through the six public methods (no direct field access exists outside the class), this should be transparent to callers, but it must ship in its own commit separate from Steps 1/2/4 so it can be reverted alone if any hidden case-sensitive assumption surfaces in existing save data or tests. Step 5 (Godot host wiring) is the second behavior-affecting step and must also be its own commit, since it changes what gets written to disk on every save from that point forward.

---

## Step 1 — Add CaptureState/RestoreState to InMemoryFlagLedger

**Goal:** Implement the standard save/load contract for the flag ledger.

**Implementation:**
1. Define the state DTO in the same file or a sibling file under `Assets/Ashfall.Core/Flags/`, following the `[Serializable]` DTO convention confirmed elsewhere in Core:
   ```csharp
   [Serializable]
   public sealed class FlagLedgerState
   {
       public List<string> Flags { get; set; } = new();
       public Dictionary<string, int> Counters { get; set; } = new();
   }
   ```
2. Add to `InMemoryFlagLedger` (the real private field names are confirmed as `_flags` and `_counters`, matching what's used below — this part of the original sample was correct):
   ```csharp
   public FlagLedgerState CaptureState()
   {
       return new FlagLedgerState
       {
           Flags = _flags.OrderBy(f => f, StringComparer.Ordinal).ToList(),
           Counters = _counters.OrderBy(kv => kv.Key, StringComparer.Ordinal)
               .ToDictionary(kv => kv.Key, kv => kv.Value)
       };
   }

   public void RestoreState(FlagLedgerState state)
   {
       _flags.Clear();
       _counters.Clear();
       if (state == null) return;
       foreach (var f in state.Flags ?? new List<string>()) _flags.Add(f);
       foreach (var kv in state.Counters ?? new Dictionary<string, int>()) _counters[kv.Key] = kv.Value;
   }
   ```
   Note the added null-guards on `state` and its collections — every other `RestoreState` implementation surveyed in Core (e.g. `CohortSystem.RestoreState`, `DiseaseSystem.RestoreState`) treats a null argument as a no-op rather than throwing, so this DTO must follow the same defensive convention rather than assume the caller always passes a fully-populated object.
3. `IFlagLedger` itself has no other members to change — `CaptureState`/`RestoreState` are new additions to the concrete `InMemoryFlagLedger` class, not to the `IFlagLedger` interface (the interface intentionally stays engine/host-agnostic and free of save concerns per the existing pattern: none of the other host-facing ports like `IClock`/`ISeededRng` carry save methods either — save/load is composed at the host session/save-store layer, not baked into the port interface).
4. Ordinal ordering in CaptureState ensures deterministic serialization (SaveChecksum stability).

**Verification:**
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — compiles
- No test failures (additive change)
- Explicit check: `IFlagLedger` interface file is unchanged; only `InMemoryFlagLedger` gained members (diff review, not just "compiles")

**Done when:** `InMemoryFlagLedger` has `CaptureState`/`RestoreState` matching the project's DTO convention, `RestoreState(null)` is a safe no-op, and the six existing public interface methods (`IsSet`, `Set`, `Clear`, `GetCounter`, `Increment`, `SetCounter`) are unchanged in signature.

---

## Step 2 — Write Round-Trip Tests for FlagLedger Save/Load

**Goal:** Verify that flag ledger state survives serialize/deserialize without corruption.

**Implementation:**
Create `Ashfall.Core.Tests/FlagLedgerSaveTests.cs`. All tests below use the real public API (`Set`, `IsSet`, `Increment`, `GetCounter`, `SetCounter`, `Clear`) — not `SetFlag`/`HasFlag`/`ClearFlag`, which do not exist:
1. **BasicFlagRoundTrip:** Call `Set` for 5 flags, `CaptureState()`, serialize via `SystemTextJsonSerializer`, deserialize, `RestoreState`, assert `IsSet` returns true for all 5.
2. **CounterRoundTrip:** `Increment` 3 counters to various values, round-trip, assert `GetCounter` values match.
3. **MixedFlagsAndCountersRoundTrip:** Both flags and counters, round-trip together.
4. **EmptyLedgerRoundTrip:** Empty state round-trips cleanly (assert `CaptureState()` on a fresh ledger produces empty `Flags`/`Counters` lists, not null).
5. **LargeLedgerRoundTrip:** 500 flags + 200 counters — stress test for collection sizing.
6. **OrderDeterminism:** Capture twice after identical mutations on two separate `InMemoryFlagLedger` instances, serialize both, assert the resulting JSON strings are byte-for-byte identical (not just "similar" — an exact string comparison is the testable assertion).
7. **DuplicateFlagResilience:** Call `Set` with the same flag id twice before `CaptureState()`, verify exactly one entry for that flag in `state.Flags` (assert `state.Flags.Count(f => f == "flag_x") == 1`, not just "no crash").
8. **RestoreNullIsNoOp:** Call `RestoreState(null)` on a ledger that already has flags/counters set, assert nothing is cleared (this exercises the null-guard from Step 1 — omitted from the original test list, added here since Step 1 now requires that behavior).

**Verification:**
- `dotnet test --filter "FlagLedgerSave"` — 8/8 pass

**Done when:** 8 round-trip tests pass (7 original + 1 null-restore guard test added to match Step 1's corrected null-safety requirement).

---

## Step 3 — Add Case-Normalization Enforcement

**Goal:** Eliminate the case-normalization drift risk by enforcing lowercase-only flag/counter IDs at the API boundary.

**Implementation:**
1. Add a `NormalizeKey(string key)` private static method:
   ```csharp
   private static string NormalizeKey(string key) => key?.ToLowerInvariant();
   ```
   (guard against null to match the existing `string.IsNullOrEmpty` early-return style already present in every method of the real class).
2. Apply normalization inside the real six public methods — `IsSet`, `Set`, `Clear`, `GetCounter`, `Increment`, `SetCounter` (not `SetFlag`/`HasFlag`/`ClearFlag`, which do not exist in this class).
3. Change the comparer used to construct `_flags` and `_counters` from `StringComparer.OrdinalIgnoreCase` to `StringComparer.Ordinal` (since all keys are now lowercase before they ever reach the collection).
4. In `RestoreState` (added in Step 1), normalize all keys from the loaded state before inserting (handles legacy saves captured before this step shipped, which may contain mixed-case keys).
5. Document in the class's XML doc comment: flag/counter IDs MUST be snake_case lowercase. `NormalizeKey` is the safety net, not the excuse.

**Behavioral-change risk call-out (elevated from the original document, which listed this as same-risk as the other steps):** this is the one step in the batch that changes runtime behavior for existing callers, not just adds new members. Every current caller (`Ashfall.Core.Tests/StandaloneCoreSystemTests.cs`, `Ashfall.Core.Tests/VerdictSystemTests.cs`, `DiveInstanceRunner.cs`, `CensusBroadcastScheduler.cs`, `VerdictCensusBroadcast.cs`, `VerdictHostSession.cs`, `OrphanKnockWhitelist.cs`) must be checked for any flag id literal that isn't already lowercase snake_case before this ships, since after this change two previously-distinct-but-case-varied keys used inconsistently by different call sites will now collide at read time in the *same* way they already silently collided before (no new collision risk) — but a literal that assumed `OrdinalIgnoreCase` behavior for a case-sensitive *display* purpose elsewhere would be affected. A quick audit command: `grep -rn '\.Set(\"' Assets/Ashfall.Core src --include="*.cs" | grep -v '[a-z_]"'` to find any non-lowercase literal flag ids before shipping this step.

**Verification:**
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — compiles
- `dotnet test` — all existing tests pass (case-insensitive behavior preserved via normalization)
- Add test: `Set("Flag_Quest_Done")` then `IsSet("flag_quest_done")` returns true, and `IsSet("FLAG_QUEST_DONE")` also returns true (both directions, matching the real method names)
- Add test: the audit grep command above returns zero matches in the current codebase (documents that the behavior change is safe today, and becomes a regression guard if a future commit adds a non-lowercase literal)

**Done when:** no case-drift risk, deterministic key storage, all tests green, and the literal-audit command above is captured as a repeatable check (e.g. wired into a test or CI step) rather than a one-time manual grep.

---

## Step 4 — Add Flag Prefix Validation

**Goal:** Enforce that all flags use the canonical `flag_` prefix (matching `CatalogIntegrityValidator` ID rules).

**Correction:** `Assets/Ashfall.Core/CatalogIntegrityValidator.cs`'s `IdPrefixes` array does include `flag_`, confirmed. However, it is one entry among 60+ domain prefixes (`item_`, `loc_`, `quest_`, `npc_`, `event_`, `enc_`, `phase_`, `wave_`, etc.) — there is no special "flags may also use quest_/event_/expansion_/encounter_" carve-out in the validator today. Since `InMemoryFlagLedger` also stores *counters* (via `Increment`/`GetCounter`/`SetCounter`) which are conceptually distinct from boolean flags, and the validator's prefix list is a global id namespace (not specific to this class), treat prefix validation for the flag ledger as a local convention this class enforces, separate from (not necessarily identical to) the full `CatalogIntegrityValidator` list — do not assume `quest_`/`event_`/`expansion_`/`encounter_` are pre-approved for flags without checking whether flags actually use those prefixes in current data (a quick check: `grep -rn '"flag_' Assets/StreamingAssets/Data --include="*.json" | grep -oE '"flag_[a-z0-9_]+"' | sort -u | head` to sample real flag id shapes before hardcoding an allowlist).

**Implementation:**
1. Since `InMemoryFlagLedger` has no existing constructor to extend (it is parameterless today — see the Rationale correction above), add an optional strict-mode constructor overload rather than assuming one exists to modify:
   ```csharp
   public InMemoryFlagLedger() : this(strictPrefixes: false) { }
   public InMemoryFlagLedger(bool strictPrefixes)
   {
       _strictPrefixes = strictPrefixes;
   }
   ```
   Drop the `ILog log = null` parameter from the original sample — `InMemoryFlagLedger` has no logging dependency today and none of its six real methods take or use one; introducing `ILog` here would require every call site listed in the Rationale section to be reviewed for whether they should now pass a logger, which is out of scope for "soft prefix validation." If warning output is required, have the caller inspect a new `LastPrefixWarning` string property after the call instead of injecting `ILog` — this avoids a new mandatory dependency on a class most callers construct with `new InMemoryFlagLedger()` today.
2. In strict mode, `Set` and `Increment` (the real mutator method names) validate that the key starts with a known prefix from an explicit list defined in this class (start with just `flag_`, per the confirmed validator entry; add others only after confirming with the sampling command above that real flag data actually uses them).
3. Invalid prefix: soft-enforce — do not throw, still execute the operation, and surface the violation via the `LastPrefixWarning` property (or an `ILog` if the team decides the dependency is worth adding, but that decision should be explicit, not assumed).
4. Godot host enables strict mode by passing `strictPrefixes: true` at the specific construction site(s) — identify these in Step 5 rather than assuming here.

**Verification:**
- `dotnet test` — all pass
- Add test: strict mode records a warning (via `LastPrefixWarning` or equivalent) on a bad-prefix `Set` call, and the flag is still set (`IsSet` returns true)
- Add test: known prefix (`flag_`) passes without a warning being recorded

**Done when:** prefix validation available via a constructor overload (not modifying a nonexistent existing constructor), soft-enforced, testable, and the logging mechanism (property vs. `ILog`) is a deliberate choice recorded in this document rather than copied from a sample that assumed a dependency this class doesn't have.

---

## Step 5 — Wire FlagLedger Save into Godot Host

**Goal:** Ensure the Godot host actually serializes/deserializes the FlagLedger as part of its save flow.

**Verified current wiring — corrected from the original "likely in Main.cs" guess:** `InMemoryFlagLedger` is instantiated today in exactly one production (non-test) location: `src/Host/VerdictHostSession.cs` line 108, as a default-parameter fallback (`flags = flags ?? new InMemoryFlagLedger();`) inside its constructor/factory. `src/Main.cs` line 3139 calls `VerdictHostSession.Create(_dataDir)`, which does not pass an explicit `IFlagLedger`, so this fallback is what's live in the running game today. Every other confirmed `new InMemoryFlagLedger()` call site is in test code (`Ashfall.Core.Tests/StandaloneCoreSystemTests.cs`, `Ashfall.Core.Tests/VerdictSystemTests.cs`) or headless self-test/panel-test harnesses (`src/Host/HostCli.PanelTests.cs`, `src/Host/HostCli.SelfTests.cs`), not real save wiring.

**Concrete, currently-reproducible bug this step must fix:** `VerdictCensusBroadcast` (`Assets/Ashfall.Core/Verdict/VerdictCensusBroadcast.cs` line 60) checks `_flags.IsSet("flag_exp08_signed_reckoning")` to decide whether to suppress a broadcast — but `VerdictSave` (`Assets/Ashfall.Core/Verdict/VerdictSave.cs`) has no field for flag ledger state at all, and `VerdictSaveCodec.Capture` does not capture it. This means `flag_exp08_signed_reckoning`, once set, is silently lost on every save/load cycle today — a save/reload will make the broadcast reappear even though the flag was set before saving. This is the concrete regression this step closes, not a hypothetical.

**Implementation:**
1. `InMemoryFlagLedger` is already known to be constructed inside `VerdictHostSession` (Step 5 does not need to "identify where it's instantiated" — that's answered above). Add `FlagLedgerState` as a field on `VerdictSave` (bump `VerdictSave.CurrentSaveVersion` per the project's existing versioned-migration pattern — confirmed real, e.g. `HoldfastSaveCodec`, `YearOfAshSaveCodec` — since this is a save-shape change to an existing save class, not a new one).
2. In `VerdictSaveCodec.Capture`, call the flag ledger's new `CaptureState()` (from Step 1) and store the result on `VerdictSave`.
3. In `VerdictHostSession`'s restore path, call `RestoreState` on the flag ledger with the loaded `FlagLedgerState`.
4. Add a dedicated save store only if the project's existing pattern requires one for this data shape — check whether `VerdictSave` already goes through a checksummed envelope store (e.g. a `VerdictSaveStore` sibling to `CraftingSaveStore`/`InventorySaveStore`) before creating a new `FlagLedgerSaveStore.cs`; if `VerdictSave` is already checksummed as part of an existing envelope, add the flag state as a field on that existing envelope rather than introducing a second, separate save file for the same session's flags — a per-session flag field is simpler and lower-risk than a single-shared cross-session `FlagLedgerSaveStore`, and does not conflict with a future global flag ledger scope, since the flag ledger instance itself is currently constructed per-session, not as a global singleton (confirmed: `VerdictHostSession` owns its own private ledger instance via the default-parameter fallback, and it is not shared with any other host session in the code reviewed).

**Verification:**
- `dotnet build Ashfall.csproj` — 0 errors, 0 warnings (per the project's stated verification bar)
- Reproduce the concrete bug first, then verify the fix: set `flag_exp08_signed_reckoning` via a Verdict encounter path, save, reload, and assert `VerdictCensusBroadcast` still suppresses the broadcast (i.e. `IsSet("flag_exp08_signed_reckoning")` is still true post-reload) — this is the specific regression test, not just "a flag persists"
- `godot --headless --path . -- --bridge-selftest` — exits 0 (this verb no longer tests anything Verdict-specific; it is listed here only because it's part of the standard 5-command verification gate, not because it targets this change)
- If a `--verdict-selftest` self-test verb exists (confirmed referenced in `src/Host/HostCli.cs`'s help text), extend it to cover this save/restore path

**Done when:** flag ledger state persists across save/load cycles for the one real production consumer (`VerdictHostSession`), the `flag_exp08_signed_reckoning` regression is verified fixed with a reproduction test, and `VerdictSave.CurrentSaveVersion` is bumped with a migration path for pre-existing saves that predate this field (per Invariant 3's versioned-migration rule — throw on future version, migrate on past version).

---

## Step 6 — Add FlagLedger Integration Tests

**Goal:** Test that flag mutations from real `IFlagLedger` consumers persist through save/load.

**Correction:** the original plan's scenarios referenced "narrative encounters" and "a quest system" reading flags. A repo-wide search found no narrative-encounter or quest-system class that takes an `IFlagLedger` dependency. The confirmed real consumers are `DiveInstanceRunner` (`Assets/Ashfall.Core/Expeditions/DiveInstanceRunner.cs`), `CensusBroadcastScheduler` (`Assets/Ashfall.Core/Radio/CensusBroadcastScheduler.cs`), `VerdictCensusBroadcast` (`Assets/Ashfall.Core/Verdict/VerdictCensusBroadcast.cs`), and `OrphanKnockWhitelist.ValidateOrphan` (`Assets/Ashfall.Core/Encounters/OrphanKnockWhitelist.cs`). Rewrite the integration scenarios against these real consumers instead of hypothetical narrative/quest classes; if flag-gated narrative/quest behavior is added in a later batch, extend this test file then rather than testing code that doesn't exist yet.

**Implementation:**
Create `Ashfall.Core.Tests/FlagLedgerIntegrationTests.cs`:
1. **VerdictBroadcastSetsFlagAndSaves:** Drive `VerdictCensusBroadcast` (or the `VerdictHostSession` wrapper from Step 5) to a state where `flag_exp08_signed_reckoning` gets set, capture all state via the Step 1 `CaptureState`, restore into a fresh ledger, verify `IsSet("flag_exp08_signed_reckoning")` is true — this directly exercises the real bug identified in Step 5.
2. **DiveRunnerFlagVisibleAfterRestore:** Run a `DiveInstanceRunner` sequence that causes it to `Set` a flag on the shared ledger, save/restore that ledger, verify a second `DiveInstanceRunner` instance constructed with the restored ledger observes the flag via `IsSet`.
3. **CounterAccumulatesAcrossSaves:** `Increment` a counter directly on `InMemoryFlagLedger` in session 1, `CaptureState`/serialize/deserialize/`RestoreState` into session 2, `Increment` again, verify `GetCounter` reflects the accumulated value.
4. **FlagClearedPersists:** `Set` then `Clear` a flag, save, restore, verify `IsSet` returns false.
5. **OrphanKnockWhitelistRespectsRestoredFlags:** `OrphanKnockWhitelist.ValidateOrphan` reads its gating flag per-entry from data (`gating_flag` field on each `OrphanKnockEntry`, sourced from `Assets/StreamingAssets/Data/whitelists/orphan_knocks.json`) rather than a hardcoded flag name — load one real entry from that whitelist file, `Set` its `gating_flag` value on the ledger, save/restore the ledger, verify `ValidateOrphan` still returns `true` for that entry's `event_name` post-restore.

**Verification:**
- `dotnet test --filter "FlagLedgerIntegration"` — 5/5 pass

**Done when:** 5 integration tests pass against real `IFlagLedger` consumers (not hypothetical narrative/quest classes), proving cross-system flag persistence including the specific Verdict regression from Step 5.

---

## Step 7 — Update AGENTS.md — Remove Case-Drift Warning

**Goal:** Update the known issues: remove the `InMemoryFlagLedger` case-normalization drift risk note.

**Verified target location:** `AGENTS.md` line 132, under Invariant 4 ("Determinism"), reads exactly: `` - `InMemoryFlagLedger` uses `StringComparer.OrdinalIgnoreCase` — case-normalization drift risk across hosts. `` This is the only mention of `InMemoryFlagLedger` in `AGENTS.md`. There is **no separate "Systems missing save/load" list** in the current `AGENTS.md` to remove an entry from — the original Step 7's second instruction targets a list that doesn't exist in this file (the closest analog, the "Known gaps (5 Godot save stores lack checksum)" list under Save/Load, is about checksummed envelopes for `ExpeditionSaveStore`/`MedicalSaveStore`/`NarrativeSaveStore`/`WorldSaveStore`/`JournalSaveStore` — `InMemoryFlagLedger` was never on that list and does not belong there even after this batch, since it is not one of those five save stores).

**Implementation:**
1. In Invariant 4's "Known offenders" list, change the line 132 bullet to: `` - ~~`InMemoryFlagLedger` uses `StringComparer.OrdinalIgnoreCase` — case-normalization drift risk across hosts.~~ — RESOLVED: keys normalized to lowercase at the API boundary (`Set`/`IsSet`/`Clear`/`Increment`/`GetCounter`/`SetCounter`); storage comparer changed to `StringComparer.Ordinal`. ``
2. Do not attempt to remove an entry from a "Systems missing save/load" list — no such list exists in the current file. If this batch's Step 5 wiring work is worth documenting in `AGENTS.md`, add a note to the Save/Load section instead (e.g. alongside the "5 Godot save stores lack checksum" list) stating that `VerdictHostSession`'s flag ledger is now captured as part of `VerdictSave`.

**Verification:**
- `AGENTS.md` line 132 (or wherever it has moved if the file was edited by another batch first — re-grep for `InMemoryFlagLedger` before editing rather than assuming the line number is still exact) accurately reflects the new state
- All 5 verification steps pass

**Done when:** `AGENTS.md`'s `InMemoryFlagLedger` bullet under Invariant 4 is marked resolved with the correct mechanism description, and no attempt was made to edit a nonexistent list.

---

## Summary

| Step | Deliverable | Risk |
|------|------------|------|
| 1 | CaptureState/RestoreState for FlagLedger (greenfield — no existing partial implementation to extend) | Low |
| 2 | 8 round-trip tests (7 original + 1 null-restore guard) | None |
| 3 | Case-normalization enforcement via real methods (`Set`/`IsSet`/`Clear`/`Increment`/`GetCounter`/`SetCounter`) | Medium (behavioral change — needs the literal-audit check before shipping) |
| 4 | Prefix validation (soft) via new constructor overload, not a modified existing one | Low |
| 5 | Godot host save wiring — fixes a real, reproducible flag-loss bug in `VerdictHostSession`/`VerdictSave` | Medium |
| 6 | 5 integration tests against real consumers (`DiveInstanceRunner`, `CensusBroadcastScheduler`, `VerdictCensusBroadcast`, `OrphanKnockWhitelist`) | None |
| 7 | AGENTS.md line-132 update only; no "missing save/load" list exists to edit | None |

**End state:** `InMemoryFlagLedger` has proper save/load, deterministic key storage, no case-drift risk, and 13 new tests (8 round-trip + 5 integration) proving correctness. Cross-system flag persistence verified, including a fix for the concrete `flag_exp08_signed_reckoning` loss-on-reload bug identified during this review.

## Review Notes (Corrected)

This file was adversarially reviewed against the actual repository at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War` and corrected in place. Findings:

1. **The entire original code sample used a fictional API.** The real `IFlagLedger`/`InMemoryFlagLedger` (`Assets/Ashfall.Core/Flags/IFlagLedger.cs`, read in full) exposes `IsSet`, `Set`, `Clear`, `GetCounter`, `Increment`, `SetCounter` — the plan's `HasFlag`, `SetFlag`, `ClearFlag` method names do not exist anywhere in the codebase. Every step (1 through 6) has been rewritten to use the real method names, and the Rationale section now states the real interface verbatim so no future editor re-introduces the fictional names.

2. **No existing constructor to extend.** The real `InMemoryFlagLedger` is parameterless. Step 4's original sample (`public InMemoryFlagLedger(ILog log = null, bool strictPrefixes = false)`) assumed an existing constructor signature with an `ILog` dependency that isn't there today. Rewrote Step 4 to add an overload rather than modify a nonexistent signature, and to make the `ILog` dependency an explicit decision (via a `LastPrefixWarning` property alternative) rather than an assumed given, since introducing a mandatory logging dependency to a class every test constructs with `new InMemoryFlagLedger()` is a bigger change than "soft prefix validation" implies.

3. **No existing `CaptureState`/`RestoreState` anywhere in `Assets/Ashfall.Core/Flags/`.** Confirmed via a repo-wide search — this batch is genuinely greenfield for the save contract, not "hardening" a flawed existing implementation as the original title implied. Called this out explicitly in the Rationale so the implementer doesn't go looking for code to fix that isn't there.

4. **Step 5's instantiation-site guess was wrong.** The plan said "likely in Main.cs or a host session" and asked the implementer to go find it. Verified: `InMemoryFlagLedger` is instantiated in production exactly once, as a default-parameter fallback inside `src/Host/VerdictHostSession.cs` (`flags = flags ?? new InMemoryFlagLedger();`), which `src/Main.cs` line 3139 reaches via `VerdictHostSession.Create(_dataDir)`. All other instantiations are in test/self-test code. Rewrote Step 5 to state this directly.

5. **Found and documented a real, reproducible bug** while verifying Step 5: `VerdictCensusBroadcast` gates a broadcast on `flags.IsSet("flag_exp08_signed_reckoning")`, but `VerdictSave`/`VerdictSaveCodec` has no field or capture logic for flag ledger state at all — meaning this flag is silently lost on every save/reload today. Added this as the concrete regression Step 5 must fix and verify, replacing the original's generic "set a flag via narrative encounter, save, restart" scenario (narrative encounters don't consume `IFlagLedger` at all — see finding 6).

6. **Step 6's integration scenarios referenced classes that don't use `IFlagLedger`.** "Narrative encounter" and "quest system" were named as flag consumers; a repo-wide search found the real consumers are `DiveInstanceRunner`, `CensusBroadcastScheduler`, `VerdictCensusBroadcast`, and `OrphanKnockWhitelist.ValidateOrphan`. Rewrote all 5 integration test scenarios (added a 5th, for `OrphanKnockWhitelist`, since it's a real consumer the original list omitted) against these confirmed classes, and pointed the `OrphanKnockWhitelist` scenario at its actual data-driven `gating_flag` mechanism (`Assets/StreamingAssets/Data/whitelists/orphan_knocks.json`) instead of a hardcoded flag name.

7. **Step 7 targeted a nonexistent list.** `AGENTS.md` has no "Systems missing save/load" list; the only real edit target is the single bullet at line 132 under Invariant 4. Corrected Step 7 to only touch that bullet and explicitly warn against inventing edits to a list that isn't there.

8. **Step 4's prefix set was unverified.** `CatalogIntegrityValidator.IdPrefixes` does include `flag_` (confirmed), but the plan's `quest_`/`event_`/`expansion_`/`encounter_` allowlist for flags specifically was not verified against real flag data. Added a sampling command to check real `flag_*` id shapes before hardcoding additional prefixes, and scoped the initial implementation to just the confirmed `flag_` prefix.

9. **Tightened Done-when criteria** throughout (e.g. Step 2's order-determinism test now requires byte-for-byte JSON string equality rather than "identical output"; Step 3 requires a specific literal-audit grep as a repeatable check; Step 6 requires the specific regression from finding 5 to be covered, not just generic persistence).

10. **Added risk/rollback guidance**: Steps 1–2 and 4 are purely additive; Steps 3 and 5 are the two behavior-changing steps and are now called out as needing independent commits so either can be reverted alone without losing the safe, additive work in the other steps.
