# ASHFALL Quality Roadmap — Batch 100

## Theme: Comprehensive Null Safety Cleanup — Nullable Is Already Enabled, 4,002 Warnings Are Hidden

| Field | Value |
|-------|-------|
| **Priority** | HIGH |
| **Risk** | Medium — fixing 4,000+ warnings touches most of the Core codebase; low risk of behavior change if done as pure annotation |
| **Scope** | `Ashfall.Core/Ashfall.Core.csproj`, `Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`, `Ashfall.csproj` (Godot host) |
| **Verified current warning count** | **4,002** nullable warnings on a clean rebuild of Core (see Step 1 — this is a measured fact, not an estimate) |
| **Dependency** | Benefits from Batch 99 (Contracts) but not blocked by it |
| **Verification** | `dotnet build -t:Rebuild` (not just `dotnet build`) + full test suite — see Step 1 for why incremental builds hide this problem |

---

## ⚠️ Original Premise Was False — Corrected Problem Statement

The original version of this plan assumed nullable reference types were
**disabled** across the project and needed to be turned on from scratch.
That is not true. Direct inspection of all three `.csproj` files shows
nullable is **already enabled everywhere**:

```
Ashfall.Core/Ashfall.Core.csproj:       <Nullable>enable</Nullable>   (net8.0)
Ashfall.Core.Tests/Ashfall.Core.Tests.csproj: <Nullable>enable</Nullable>   (net9.0)
Ashfall.csproj (Godot host):            <Nullable>enable</Nullable>   (net8.0)
```

So Steps 1-2 of the original plan ("audit which projects have nullable
disabled," "enable nullable on Core") are **already done** and describe
work that doesn't need to happen.

**The real, verified problem is different and arguably worse:** nullable
analysis is on, but nobody is looking at its output.

- `Ashfall.Core.Tests.csproj` has a `NoWarn` list that **specifically
  suppresses the nullable warning codes**: `CS8618;CS8603;CS8600;CS8601;
  CS8602;CS8604;CS8625` (confirmed at line 10 of the csproj). This doesn't
  disable nullable analysis — it disables the compiler's *complaints* about
  it, in the one project most likely to catch nullable regressions from test
  code.
- Running `dotnet build` normally (incremental) reports **0 warnings**
  because MSBuild's incremental build skips recompilation of unchanged
  files and doesn't re-emit warnings that were already reported (and
  silently accepted) in a prior build.
- Running a **clean rebuild** (`dotnet build -t:Rebuild`, or `dotnet clean`
  followed by `dotnet build`) reveals the truth: **4,002 nullable warnings**
  in `Ashfall.Core` alone, broken down by code:

  | Code | Count | Meaning |
  |------|-------|---------|
  | CS8618 | 2,166 | Non-nullable field/property is never initialized in the constructor |
  | CS8603 | 1,076 | Possible null reference return |
  | CS8625 | 586 | Cannot convert null literal to non-nullable reference type |
  | CS8600 | 96 | Converting null literal or possible null value to a non-nullable type |
  | CS8604 | 40 | Possible null reference argument |
  | CS8602 | 22 | Dereference of a possibly null reference |
  | CS8601 | 16 | Possible null reference assignment |
  | **Total** | **4,002** | verified via `dotnet build Ashfall.Core/Ashfall.Core.csproj -t:Rebuild` on this repo, this session |

  (Measured with `-t:Rebuild` and shared-compilation disabled to force a
  true from-scratch compile; the `Ashfall.Core.Tests.csproj` rebuild
  reports the same 4,002 from Core plus 2 additional `CS8620` warnings from
  test-only code.)

- CS8618 (uninitialized non-nullable field, 54% of all warnings) is almost
  certainly concentrated in the JSON-deserialization DTO classes (e.g.
  `CombatWeaponJson`, `GoodsCatalogLoader.RawGoodDefinition`,
  `CurrentsCatalogLoader.CurrentEntry`, `WitnessCatalogLoader.WitnessEntry`,
  `EpilogueMatrixLoader.EndingEntry` — all confirmed present via the same
  rebuild) — these are classes whose fields are populated by
  `System.Text.Json` reflection, never by a constructor, so the compiler
  correctly (if noisily) flags every one. **This is a real design tension**:
  the same classes also emit `CS0649` ("field is never assigned") warnings
  for the same reason under the ordinary compiler, and both categories are
  a natural consequence of the reflection-based deserialization pattern
  used throughout the catalog loaders — not something a spot-fix can solve.
  Any remediation plan must special-case this pattern (see Step 3) rather
  than trying to add real initializers to hundreds of pure-DTO classes.

This changes the batch's actual goal: it is **not** "enable nullable and
migrate the codebase" (already done), it is **"make 4,002 pre-existing,
silently-ignored nullable warnings visible in CI, then pay down the ones
that indicate real bugs, while deliberately suppressing the ones that are
an inherent, accepted cost of the JSON-DTO pattern."**

---

## Step 1 — Establish the Real Warning Baseline (Rebuild, Not Incremental Build)

### Goal

Get an authoritative, reproducible warning count and breakdown by forcing a
clean rebuild, since incremental builds hide the problem entirely — this is
itself a process bug worth fixing before anything else.

### Implementation

- Confirm the finding is reproducible:
  ```bash
  dotnet clean Ashfall.Core/Ashfall.Core.csproj
  dotnet build Ashfall.Core/Ashfall.Core.csproj -v:q /p:UseSharedCompilation=false -t:Rebuild \
    2>&1 | tee nullable-baseline.log
  grep -oE "warning CS8[0-9]{3}" nullable-baseline.log | sort | uniq -c | sort -rn
  ```
- Break the 4,002 warnings down **by file**, not just by code, to find concentration:
  ```bash
  grep -oE "warning CS8[0-9]{3}" -B0 nullable-baseline.log | wc -l   # sanity total
  grep "warning CS8" nullable-baseline.log | sed -E 's/.*\](.*)\(([0-9]+),.*/\1/' | sort | uniq -c | sort -rn | head -30
  ```
  (Adjust the `sed` pattern to the actual log line shape — verify against a
  sample line first; do not trust this exact regex blindly.)
- Separately measure the Godot host (`Ashfall.csproj`) and the test project's
  own code (excluding warnings that originate from `Ashfall.Core` via
  project reference) — these need their own counts since `NoWarn` in the
  test project currently masks its *own* nullable issues, not just Core's.
- Record the baseline in `docs/nullable-baseline.md` with: total count, top
  20 files by warning count, and the CS8618-in-DTO-classes carve-out
  identified above (estimate what fraction of the 2,166 CS8618s are in
  `*Json`/`*Entry`/`*Loader`-nested DTO classes vs. real application logic —
  this ratio determines how much of Step 3-5's effort is "annotate as
  intentional" vs. "fix a real gap").

### Verification

```
dotnet clean Ashfall.Core/Ashfall.Core.csproj && dotnet build Ashfall.Core/Ashfall.Core.csproj -t:Rebuild 2>&1 | tail -5
# Expect: "4002 Warning(s)" (or close — re-verify at execution time since the
# codebase will have changed since this document was written)
```

### Done when

- `docs/nullable-baseline.md` exists with the exact rebuild warning count, breakdown by code, and top-20-files-by-warning-count table
- The DTO-vs-real-logic ratio for CS8618 is estimated with a concrete number, not "some"
- The reproduction command (`dotnet clean && dotnet build -t:Rebuild`) is documented as the *only* trustworthy way to measure this — a note is added warning that plain `dotnet build` undercounts due to incremental caching

### Risk / Rollback

- **Risk:** none — this step is pure measurement, no code changes.
- **Rollback:** N/A.

---

## Step 2 — Decide and Document the DTO Suppression Policy

### Goal

Make an explicit, written decision about how to handle the large,
structurally-inherent CS8618 count in reflection-populated JSON DTO classes,
**before** touching any of the 4,002 warnings — otherwise Steps 3-5 will
either (a) waste enormous effort adding meaningless constructor
initializers to hundreds of pure-data classes, or (b) get abandoned
halfway through because the true scope was underestimated.

### Implementation

Two realistic options — pick one and document the choice, don't leave it
implicit:

**Option A — Targeted per-class suppression (recommended):**
- Add `#pragma warning disable CS8618 // Populated via JSON deserialization reflection, not a constructor` directly above each DTO class definition (or above the file's `#nullable` region if the whole file is DTOs-only, as several catalog loader files appear to be based on the Step 1 sample).
- This keeps the suppression visible and auditable per-class, rather than a blanket project-wide `NoWarn`.
- Estimated effort: mechanical, can be scripted (see Step 3), but every suppression needs a human glance to confirm the class really is a pure JSON DTO and not something with real initialization logic hiding CS8618 warnings that matter.

**Option B — `required` members (C# 11+, if the project's language version supports it):**
- Verify the project's `<LangVersion>` in `Ashfall.Core.csproj` supports `required` members before choosing this path.
- Mark DTO fields `public required string Id;` — this satisfies the compiler without suppression, but requires the deserializer to actually populate every `required` field or throw, which may not match `System.Text.Json`'s current lenient-missing-field behavior. **This could change runtime behavior** (a previously-silently-missing JSON field would now throw at deserialize time) — treat as a behavior change requiring test coverage, not a pure annotation fix.

Document the choice and rationale in `docs/nullable-baseline.md` from Step 1.
Do not silently mix both approaches without a stated reason per-file.

### Verification

- No code changes yet — this step produces a decision document.
- Peer review of the decision (the "Cross-Tool QA Rule" in AGENTS.md applies here: since this changes ≥2 coupled things — suppression scope and DTO deserialization behavior — get a second reviewer).

### Done when

- `docs/nullable-baseline.md` states which option was chosen, why, and which files/patterns qualify as "pure JSON DTO" vs. "needs a real fix"
- If Option B is chosen, a note documents the required behavior-change testing needed before rollout

### Risk / Rollback

- **Risk:** choosing Option B without verifying `System.Text.Json`'s missing-field behavior could turn "warning" into "runtime crash on malformed/older save data or hand-edited JSON" — a real regression risk for a project whose data authority already has known malformed/incomplete files per other batches' findings (see Batch 51/92's schema_version work). Recommend Option A unless a strong reason favors B.
- **Rollback:** this step is a decision document; no rollback needed.

---

## Step 3 — Fix Nullable Warnings in `Ports.cs` and `HostDefaults.cs`

### Goal

Make the foundational interfaces and their default implementations
nullable-clean first (these have the fewest warnings but the highest
blast radius if wrong, since every system depends on them).

### Implementation

- Re-verify `Ports.cs`'s actual current interface member list before writing annotations — do not assume the shape; earlier batches in this series found fabricated method names in other files, so read the file directly:
  ```bash
  grep -n "public\|interface" Ashfall.Core/../Assets/Ashfall.Core/Ports.cs 2>/dev/null || find . -iname "Ports.cs"
  ```
- For each of the 5 port interfaces (`IJsonSerializer`, `IFileIO`, `ILog`, `IClock`, `ISeededRng`), check the rebuild log from Step 1 for how many of the 4,002 warnings originate in these two files specifically — this determines real effort, not the "20-40" guess from the original draft.
- Annotate based on real semantics, verified against the actual implementation in `HostDefaults.cs`:
  - Deserialization methods that can fail on malformed JSON → `T?` return
  - File-read methods that can miss a file → `string?` return
  - Pure value-type returns (day counters, RNG outputs) → no nullable annotation needed
- Fix all callers within Core that don't handle a null return from these two files specifically (do not scope-creep into unrelated callers yet — that's Steps 4-5).

### Verification

```
dotnet build Ashfall.Core/Ashfall.Core.csproj -t:Rebuild 2>&1 | grep "Ports.cs\|HostDefaults.cs" | grep -c "warning CS8"
# Expect: 0 (down from whatever Step 1's baseline recorded for these 2 files)
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
```

### Done when

- Zero nullable warnings remain in `Ports.cs` and `HostDefaults.cs` on a clean rebuild
- Every interface method has explicit, verified-correct nullability matching its actual failure modes (not assumed ones)
- All existing tests pass unchanged

### Risk / Rollback

- **Risk:** Low — these are foundational but small files; changing a return type from `T` to `T?` can ripple into callers project-wide if the type is widely consumed. Grep for all call sites before changing a signature, not after.
- **Rollback:** revert the two files' diffs; caller fixes are additive null-checks and safe to leave even if the interface change is reverted (they just become dead defensive code).

---

## Step 4 — Fix Nullable Warnings in Top-20-by-Warning-Count Files

### Goal

Target the files that Step 1's real data identifies as the highest
contributors to the 4,002 total — **not** a guessed list of "top 20
systems by call frequency." The original draft's list (`NeedsSystem`,
`RadiationSystem`, `InventorySystem`, `MarketSystem`, `MoraleSystem`,
`AfflictionSystem`, `CombatSystem`, `PersonalQuestSystem`,
`DynamicEconomySystem`, ...) contained multiple **fabricated class names**
confirmed non-existent by Batch 99's review of the same codebase
(`InventorySystem`, `MoraleSystem`, `AfflictionSystem`,
`DynamicEconomySystem` — none exist; real names are `Inventory`,
`MoraleMarkSystem`/`MoralBranchingSystem`, no affliction system located
yet, and `MarketSystem` respectively). Do not reuse that list — use Step
1's measured top-20 file table instead.

### Implementation

- Pull the top 20 files from `docs/nullable-baseline.md` (Step 1's output), not from memory or a prior plan.
- Apply Step 2's chosen suppression policy to confirmed pure-DTO classes in this set.
- For files with real application logic (not DTOs), fix warnings properly:
  - Nullable field/property annotations matching real usage
  - Null-check before dereference at call sites
  - `TryGetValue` instead of indexer + hope for dictionary lookups
  - Null-forgiving (`!`) only with a comment proving why it's safe
- Cross-reference every class/method name against the live source before writing a fix — this document's own history (see Batch 99's corrections) shows fabricated signatures are a recurring failure mode in these plans.

### Verification

```
dotnet build Ashfall.Core/Ashfall.Core.csproj -t:Rebuild 2>&1 | grep -oE "warning CS8[0-9]{3}" | wc -l
# Expect: baseline (4002) minus however many this step's 20 files accounted for — record before/after
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
godot --headless --path . -- --data-integrity-selftest
```

### Done when

- The specific 20 files identified in Step 1 (not a guessed list) are at zero nullable warnings on rebuild, OR are documented as intentionally-suppressed per Step 2's policy
- Total warning count has measurably dropped by the amount attributable to these 20 files (recorded, not assumed)
- All existing tests pass

### Risk / Rollback

- **Risk:** Medium — touching 20 files with real application logic (as opposed to pure DTOs) means some fixes will change method signatures (`T` → `T?`), which ripples to every caller. Recommend one file per commit so a bad fix is easy to bisect and revert, rather than one 20-file commit.
- **Rollback:** per-file `git revert`.

---

## Step 5 — Fix Nullable Warnings in Remaining Files

### Goal

Drive the Core project's rebuild warning count to zero (or to
"zero-outside-documented-suppressions" if Step 2 chose targeted
suppression for DTO classes).

### Implementation

- Sweep remaining files by actual subsystem directory structure in the
  repo (verify the real directory names before writing this list — do not
  assume the original draft's guessed structure of
  `Economy/Journal/Inventory/Radiation/Medical/Quests/Combat/Weather/
  Expansion01-04/Save/Data` matches reality; list the real top-level
  directories under `Assets/Ashfall.Core/` first:
  ```bash
  find Assets/Ashfall.Core -maxdepth 1 -type d
  ```
  and iterate over what's actually there).
- Apply the same patterns as Step 4: DTOs get Step 2's suppression policy, real logic gets fixed.
- Track progress file-by-file against the Step 1 baseline log — re-run the rebuild count after each subsystem directory is finished, don't wait until the very end to discover the count didn't move.

### Verification

```
dotnet build Ashfall.Core/Ashfall.Core.csproj -t:Rebuild 2>&1 | grep -c "warning CS8"
# Target: 0, or the exact number of intentionally-suppressed-via-pragma warnings if Step 2 chose Option A
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj -t:Rebuild    # Godot host still compiles cleanly
```

### Done when

- `dotnet build Ashfall.Core/Ashfall.Core.csproj -t:Rebuild` reports 0 nullable warnings, or exactly the count of documented, per-class `#pragma`-suppressed DTO warnings from Step 2
- No blanket `#nullable disable` remains anywhere in `Assets/Ashfall.Core/` (per-class pragma suppression for a documented reason is acceptable; a whole-file or whole-project disable is not)
- Collections are initialized to empty, not left nullable-by-omission

### Risk / Rollback

- **Risk:** Medium — same as Step 4, scaled up. Recommend per-subsystem-directory commits.
- **Rollback:** per-directory `git revert`.

---

## Step 6 — Fix the Test Project's `NoWarn` Suppression

### Goal

Stop the test project from blanket-suppressing the exact nullable warning
codes this batch is trying to eliminate. Currently `Ashfall.Core.Tests.csproj`
has `<NoWarn>$(NoWarn);xUnit2013;xUnit2020;CS8618;CS8603;CS8600;CS8601;
CS8602;CS8604;CS8625</NoWarn>` — meaning even after Core is fully cleaned
up, any *new* nullable violation written directly in test code (helpers,
fakes, mock setups) would still compile silently.

### Implementation

- Remove the nullable warning codes from the test project's `NoWarn` list, keeping only the legitimate xUnit analyzer suppressions (`xUnit2013`, `xUnit2020`).
- Rebuild and measure how many *new* warnings appear that were previously hidden by this `NoWarn` entry (these are warnings in test code itself, separate from the 4,002 in Core — Core's warnings will still show up here too since the test project references Core, so isolate test-only files when counting).
- Fix test-code nullable warnings using the same patterns as Steps 3-5.
- For intentional null-testing patterns (e.g. `Assert.Throws<...>(() => system.Tick(null!))`), use the null-forgiving operator with a one-line comment explaining the test's intent — do not resurrect a blanket suppression to avoid annotating these.

### Verification

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj -t:Rebuild 2>&1 | grep -c "warning CS8"
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
```

### Done when

- The 7 nullable codes are removed from `Ashfall.Core.Tests.csproj`'s `NoWarn`
- Rebuild of the test project shows 0 nullable warnings originating from test-only files (Core-originated warnings are tracked separately per Steps 3-5 and should already be at/near zero by this point)
- All tests still pass

### Risk / Rollback

- **Risk:** Low — this is test-only code; a bad fix affects test reliability, not production behavior.
- **Rollback:** re-add the `NoWarn` entries if this step is abandoned partway; trivial one-line revert.

---

## Step 7 — Add CI Gate: Rebuild-Based Nullable Warning Count, Not Incremental

### Goal

Permanently close the loophole discovered in Step 1 — that incremental
builds hide nullable warnings — by making CI always perform a clean
rebuild and fail if the warning count regresses above the Step 5/6
end-state baseline.

### Implementation

- In CI (whatever pipeline currently exists — cross-reference this repo's actual CI setup rather than assuming; a prior batch in this same series found the checked-in CI workflows are Unity-oriented and may need separate remediation before a `dotnet`-based gate can even run there):
  ```bash
  dotnet clean Ashfall.Core/Ashfall.Core.csproj
  WARNING_COUNT=$(dotnet build Ashfall.Core/Ashfall.Core.csproj -t:Rebuild 2>&1 | grep -oE "warning CS8[0-9]{3}" | wc -l)
  ALLOWED=$(cat docs/nullable-warning-budget.txt)   # the documented Step 2 suppression count, or 0
  if [ "$WARNING_COUNT" -gt "$ALLOWED" ]; then
    echo "FAIL: $WARNING_COUNT nullable warnings exceeds budget of $ALLOWED"
    exit 1
  fi
  ```
- Do **not** use `dotnet build` without `-t:Rebuild` / without a preceding `clean` — Step 1 demonstrated this silently reports 0 regardless of the true count, which would make this CI gate worthless (a false sense of safety, arguably worse than no gate).
- Consider `<TreatWarningsAsErrors>` scoped to just the 7 nullable codes as a stronger alternative once Step 5/6 reach zero, but only after confirming the CI environment always does a clean checkout (so the incremental-build blind spot can't recur there either).
- Update the project's verification checklist (wherever it actually lives — verify current location rather than assuming a specific file) to note that nullable warning checks require a clean rebuild, with an explicit one-line warning about the incremental-build blind spot this batch discovered.

### Verification

```
dotnet clean Ashfall.Core/Ashfall.Core.csproj && dotnet build Ashfall.Core/Ashfall.Core.csproj -t:Rebuild 2>&1 | grep -c "warning CS8"
# Confirm this matches docs/nullable-warning-budget.txt before considering the gate "live"
```

### Done when

- CI (or a documented local pre-commit/pre-merge step, if CI is out of scope per other findings) performs a clean rebuild specifically to count nullable warnings, not an incremental build
- The allowed-warning-count budget is a checked-in file, not a hardcoded assumption, and matches Step 2's documented suppression policy
- A regression test proves the gate actually fails when a new nullable violation is introduced (write one deliberately in a scratch branch, confirm the gate catches it, then discard)

### Risk / Rollback

- **Risk:** Low, but a clean rebuild is slower than incremental — factor added CI time into the estimate rather than treating this as free.
- **Rollback:** remove the gate script/step; no source code changes are involved in this step itself.

---

## Summary Table

| Step | Deliverable | Real Scope (verified) | Risk |
|------|-------------|------------------------|------|
| 1 | Rebuild-based warning baseline + file breakdown | Measurement only — **4,002 warnings confirmed** | None |
| 2 | DTO suppression policy decision | Decision doc — determines true effort for 3-5 | None (decision only) |
| 3 | Fix `Ports.cs` + `HostDefaults.cs` | 2 files, warning count TBD from Step 1's real breakdown | Low |
| 4 | Fix top-20-by-warning-count files (from real data, not a guessed list) | 20 files, majority of the 4,002 total likely concentrated here per CS8618 pattern | Medium |
| 5 | Fix remaining files | Remainder of Core, directory list to be taken from the real repo tree | Medium |
| 6 | Remove nullable codes from test project's `NoWarn` | 1 file + however many test-only warnings surface | Low |
| 7 | CI gate using clean rebuild, not incremental build | CI config + budget file | Low |

**Verified baseline:** 4,002 nullable warnings in `Ashfall.Core` today (CS8618×2166, CS8603×1076, CS8625×586, CS8600×96, CS8604×40, CS8602×22, CS8601×16), invisible to normal `dotnet build` due to incremental-build caching.
**Breaking changes:** None if annotation-only; Step 2's Option B (`required` members) is the one path with genuine runtime-behavior risk and is flagged accordingly.
**Performance impact:** Zero at runtime; CI rebuild time increases (quantify during Step 7).

---

## Review Notes (Corrected)

This entire document was rewritten after adversarial verification against
the live repository, because the original draft's foundational premise was
false.

### The single most important correction

**Original claim:** "The ASHFALL Core layer and test project explicitly
suppress nullable reference type analysis... The Core project likely has
nullable disabled or unset."

**Verified reality:** All three `.csproj` files (`Ashfall.Core.csproj`,
`Ashfall.Core.Tests.csproj`, `Ashfall.csproj`) have
`<Nullable>enable</Nullable>` today. Nullable analysis is **not** disabled
anywhere. Verified by direct `grep`/`cat` of each file, this session.

This invalidates the original Steps 1 ("audit which projects have nullable
disabled") and 2 ("enable nullable on Core with warnings-as-errors") as
written — that work is already done. The real, previously-undocumented
problem is that:

1. The test project's `NoWarn` list explicitly suppresses the 7 nullable
   warning codes (verified at `Ashfall.Core.Tests.csproj:10`).
2. A clean rebuild of `Ashfall.Core` produces **4,002 real nullable
   warnings today**, verified via `dotnet build -t:Rebuild` with shared
   compilation disabled, this session, broken down by exact code.
3. An ordinary incremental `dotnet build` reports **0 warnings** for the
   same project — MSBuild's incremental build cache hides previously-seen
   warnings on unchanged files. This means the "0 errors, 0 warnings"
   signal every contributor sees day-to-day is misleading, and no CI gate
   exists that would have caught this (or if one does, it isn't using a
   clean rebuild either, since the count would otherwise already be
   visible).

### Other corrections made

- **Estimated warning count** ("300-800 typical for a 40k+ line codebase")
  replaced with the measured figure (4,002), which is 5-13x higher than the
  estimate — the estimate was not just imprecise but categorically wrong in
  magnitude, likely because it assumed nullable was being turned on for the
  first time rather than already-enabled-but-unaddressed.
- **Step 4's "top 20 systems" list** contained multiple non-existent class
  names (`InventorySystem`, `MoraleSystem`, `AfflictionSystem`,
  `DynamicEconomySystem`, `PersonalQuestSystem`, `ShelterSystem`,
  `DoseLedger`, `CombatSystem`) — cross-referenced against Batch 99's
  independently-verified real class list in this same review series
  (`Inventory`, `MoraleMarkSystem`/`MoralBranchingSystem`, no confirmed
  affliction system, `MarketSystem`, no `PersonalQuestSystem`/
  `ShelterSystem` located, etc.). Replaced the guessed list with an
  instruction to derive the real top-20 from Step 1's measured
  per-file breakdown instead of naming files that may not exist.
- **Identified and flagged a real, previously-unaddressed structural
  issue**: the large CS8618 count is substantially explained by
  reflection-populated JSON DTO classes (`CombatWeaponJson`,
  `GoodsCatalogLoader.RawGoodDefinition`, `CurrentsCatalogLoader.
  CurrentEntry`, etc. — all confirmed present via the same rebuild log)
  whose fields are never assigned in a constructor by design. The original
  plan's Step 4/5 implementation guidance ("annotate fields... fix all
  resulting warnings") would have wasted significant effort trying to add
  real initializers to hundreds of pure-data classes. Added Step 2 as a
  new, required decision gate before any fix work starts.
- **Added the missing risk callout that Step 2's "Option B" (`required`
  members) is a genuine behavior change**, not a pure annotation fix — if
  `System.Text.Json` currently tolerates missing JSON fields silently,
  marking them `required` would convert that into a deserialize-time
  exception, which interacts with this project's own documented history of
  malformed/incomplete data files (per other batches in this series
  covering `schema_version` rollout). This was entirely absent from the
  original plan, which presented nullable annotation as uniformly
  zero-risk.
- **Added Step 1 as a measurement step with a documented reproduction
  command**, since the incremental-vs-rebuild distinction is the single
  most important fact this review uncovered — any future contributor who
  runs plain `dotnet build` to "check the current warning count" will be
  silently lied to by the build cache.
- Renumbered the original 7 steps to 7 steps with a different, verified
  shape (baseline → policy decision → foundational files → top real files →
  remainder → test project → CI gate), since the original numbering
  ("enable nullable" as its own step) no longer applies.
- All verification commands throughout now explicitly use `-t:Rebuild` (or
  `dotnet clean &&`) rather than plain `dotnet build`, to avoid the
  incremental-build blind spot this review discovered.
