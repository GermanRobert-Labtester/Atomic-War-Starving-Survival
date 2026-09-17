# ASHFALL Quality Roadmap — Batch 53
## Theme: Error Handling & Resilience — Bare Catch Blocks & Loader Hardening (H4)

**Priority:** HIGH (H4 — swallowed exceptions hide data corruption)
**Risk:** Medium — changing error flow can surface previously-hidden bugs
**Prerequisite:** None

---

## Rationale

**Corrected count (verified by direct grep against the current repository):** the codebase does not have "13 bare `catch { }` blocks across `VerdictCatalogLoader.cs` (3) and `YearOfAshCatalogLoader.cs` (7+)." The actual counts, verified with `grep -n "catch"` against both files plus a full-repo sweep of `Assets/Ashfall.Core/`, are:

- `VerdictCatalogLoader.cs`: 4 total catch blocks. 1 is truly empty (`catch { }`, line 166, in `LoadCorruptionCorpus`). The other 3 (lines 51, 105, 141) are `catch { return result; }` — they have a body (an explicit early return of the already-built partial result), so they are not literally "no body," but they are still silent: they swallow the exception with no logging, matching the spirit of H4 even though they don't match the letter of "bare catch with empty body."
- `YearOfAshCatalogLoader.cs`: 12 total catch blocks. 6 are truly empty (`catch { }`, lines 152, 183, 255, 263, 294, 325). The other 6 (lines 158, 189, 269, 300, 331, 362) are `catch { return new List<...>(); }` / `catch { return new List<QuestlineDefinition>(); }` — same pattern as above: silent, but with an explicit body.
- Two more files elsewhere in `Assets/Ashfall.Core/` also contain truly-empty `catch { }` blocks not mentioned anywhere in the original plan or in AGENTS.md's H4 entry: `Assets/Ashfall.Core/Warlords/WarlordDoctrineCatalog.cs:363` (1 instance) and `Assets/Ashfall.Core/DoseContentCatalog.cs:113,132` (2 instances).

Total truly-empty `catch { }` blocks across Core: **10** (1 Verdict + 6 YearOfAsh + 1 Warlords + 2 DoseContentCatalog). Total silent catches (empty-body OR "catch and return a default with no logging," which is the behavior AGENTS.md's H4 is actually complaining about): **4 (Verdict) + 12 (YearOfAsh) + 1 (Warlords) + 2 (DoseContentCatalog) = 19**, spread across 4 files, not 2 files plus "3 more in other files" as vaguely stated. AGENTS.md's own H4 row ("13 bare catch blocks... `YearOfAshCatalogLoader.cs` (7), `VerdictCatalogLoader.cs` (3) — unchanged") is itself imprecise against the current source and should be corrected alongside this batch's work (see Step 7).

The behavioral problem AGENTS.md and this plan are actually pointing at is broader than "catch blocks with a literally empty body": it is **any catch block in a catalog loader that discards the exception without logging**, whether the body is empty or just does `return <default>`. This batch's scope is corrected to fix all silent catches (19, per the count above), not just the subset that happens to have zero statements in the braces.

Additionally, catalog loaders currently return empty data on failure with no diagnostic trail. This batch upgrades error handling to fail-loud with clear diagnostics while preserving graceful degradation for optional/addon content.

---

## Step 1 — Audit All Bare Catch Blocks in Core

**Goal:** Identify every `catch { }` and `catch (Exception) { }` with no body or only a comment in `Assets/Ashfall.Core/`, **and** every `catch { return <default>; }` that discards the exception without logging (the broader, behaviorally-relevant category — see corrected Rationale above).

**Implementation:**
1. Grep for `catch\s*\{\s*\}` (empty-body) and separately for `catch\s*\{[^}]*return[^}]*\}` (silent-default-return) in `Assets/Ashfall.Core/`.
2. Classify each: (a) critical path (loader/save), (b) optional path (UI flavor, non-essential).
3. Produce a prioritized fix list.

**Verified results (by direct grep against the current repository, superseding the estimate in AGENTS.md):**
- `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs` — 1 empty `catch { }` (line 166) + 3 `catch { return result; }` (lines 51, 105, 141) = 4 total silent catches, all in loader/critical-path methods (`LoadLocations`, `LoadItems`, `LoadRadio`, `LoadCorruptionCorpus`).
- `Assets/Ashfall.Core/YearOfAsh/YearOfAshCatalogLoader.cs` — 6 empty `catch { }` (lines 152, 183, 255, 263, 294, 325) + 6 `catch { return new List<...>(); }` (lines 158, 189, 269, 300, 331, 362) = 12 total silent catches, all in loader/critical-path methods (`LoadItems`, `LoadEvents`, `LoadQuests`, `LoadLocations`, `LoadRadioBroadcasts`, `LoadSurvivors`).
- `Assets/Ashfall.Core/Warlords/WarlordDoctrineCatalog.cs` — 1 empty `catch { }` (line 363), not mentioned in the original plan.
- `Assets/Ashfall.Core/DoseContentCatalog.cs` — 2 empty `catch { }` (lines 113, 132), not mentioned in the original plan.

Total: 19 silent catches across 4 files (10 empty-body, 9 catch-and-return-default). This supersedes AGENTS.md's "13... plus 3 more in other files" estimate.

**Verification:**
- Complete inventory produced, no catches missed — cross-check with `grep -c "catch" <file>` per file to make sure no catch block (empty or not) was skipped during manual review.

**Done when:** Audit table with file, line, and classification for all 19 silent catches (not 13) exists, and the table's per-file counts match the verified numbers above exactly.

---

## Step 2 — Fix VerdictCatalogLoader Bare Catches

**Goal:** Replace all 4 silent catch blocks in `VerdictCatalogLoader.cs` with diagnostic logging (corrected from "the 3 bare `catch { }` blocks" — there are 4 silent catches: 1 truly empty at line 166, and 3 `catch { return result; }` at lines 51, 105, 141).

**Implementation:**
For each of the 4 catches:
1. Accept `ILog` parameter in the loader method (if not already present). Confirmed: none of `LoadLocations`, `LoadItems`, `LoadRadio`, `LoadCorruptionCorpus` currently accept an `ILog` parameter — this is a public API signature change on 4 static methods. Check `Ashfall.Core.Tests/` and any Godot host call site (`src/Host/`) for existing callers of these 4 methods before changing signatures, since adding a required parameter breaks all call sites; prefer an overload or an optional parameter (`ILog log = null`, no-op if null) to avoid a breaking change across the whole call graph, unless a full audit of callers confirms it's safe to make it required.
2. Replace `catch { }` (line 166, inside `LoadCorruptionCorpus`) with:
   ```csharp
   catch (Exception ex)
   {
       log?.Warn($"VerdictCatalogLoader: failed to parse {DataFile}: {ex.Message}");
       // Return whatever was accumulated so far; continue loading others
   }
   ```
3. Replace the 3 `catch { return result; }` blocks (lines 51, 105, 141 — inside `LoadLocations`, `LoadItems`, `LoadRadio` respectively) with:
   ```csharp
   catch (Exception ex)
   {
       log?.Error($"VerdictCatalogLoader: failed to load {fileNameForThisMethod}: {ex.Message}");
       return result; // empty, safe default — behavior unchanged, now diagnosed
   }
   ```
   Note these three already return an empty/partially-built `result` list, not a fresh `new VerdictCatalog()` (there is no `VerdictCatalog` class in this file — the loader returns typed lists per data kind, e.g. `List<VerdictLocationEntry>`). Do not invent a `VerdictCatalog()` constructor call that does not exist in the source.
4. Ensure the loader's public contract (return type, null vs empty) is unchanged — all 4 methods currently return an empty (never null) collection on any failure path; this must not change.

**Verification:**
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — compiles
- `dotnet test --filter "Verdict"` — all Verdict tests pass. Confirm the actual test class name/namespace exists under this filter before relying on it (verify with `dotnet test --list-tests` or an equivalent discovery step if the filter returns 0 tests unexpectedly)
- Manually corrupt a verdict JSON field, verify the warning is logged (not silently swallowed)
- `godot --headless --path . -- --data-integrity-selftest` — 0 errors

**Done when:** All 4 silent catches in `VerdictCatalogLoader.cs` (not 3) are replaced with logged diagnostics, all tests green, and no method's public return type or empty-vs-null contract changed.

---

## Step 3 — Fix YearOfAshCatalogLoader Bare Catches

**Goal:** Replace all 12 silent catch blocks in `YearOfAshCatalogLoader.cs` with diagnostic logging (corrected from "the 7 bare `catch { }` blocks" — there are 12 silent catches: 6 truly empty at lines 152, 183, 255, 263, 294, 325, and 6 `catch { return new List<...>(); }` at lines 158, 189, 269, 300, 331, 362).

**Implementation:**
Same pattern as Step 2:
1. Accept `ILog` in the loader signature — as an optional parameter (`ILog log = null`) unless a caller audit confirms all 6 public static methods (`LoadItems`, `LoadEvents`, `LoadQuests`, `LoadLocations`, `LoadRadioBroadcasts`, `LoadSurvivors`) can safely take a new required parameter across every call site.
2. Empty catches (the 6 "primary container" catches, e.g. line 152 in `LoadItems`): log warning noting the primary container-deserialize attempt failed, then fall through to the existing flat-list fallback deserialize attempt that already follows each one in source — do not skip an entry, since the existing code already retries with a different shape (container-wrapped vs. bare list) rather than "skip entry, continue" as the original plan implied; there is no per-entry loop here to skip within.
3. Catch-and-return-default catches (the 6 fallback catches, e.g. line 158 in `LoadItems`): log error (this is the final fallback — after this, the method gives up and returns empty), then return the same empty list as today.
4. Preserve public contract (return type unchanged, e.g. `List<YearOfAshItemEntry>`, never null).

**Note on "per-entry" wording:** the original plan's Step 3 description ("Per-entry catches: log warning, skip entry, continue") does not match this file's actual structure. `YearOfAshCatalogLoader` has no loop-based per-entry catch — each load method has exactly two sequential try/catch pairs: first try the wrapped-container shape, catch and fall through; then try the flat-list shape, catch and return empty. Both catches are file-level/shape-level, not entry-level. Correct the implementation guidance accordingly rather than describing a per-entry skip behavior that doesn't exist in this loader.

**Verification:**
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — compiles
- `dotnet test --filter "YearOfAsh"` — all tests pass (verify the filter actually matches an existing test class before relying on it)
- Corrupt a `year_of_ash_*.json` entry (verify against an actual file under `Assets/StreamingAssets/Data/` before writing the test, since "a YAML entry" in the original plan is wrong — this loader is JSON, not YAML), verify logged
- `godot --headless --path . -- --data-integrity-selftest` — 0 errors

**Done when:** All 12 silent catches (not 7) replaced, H4 partially resolved for this file.

---

## Step 4 — Fix Remaining Bare Catches (2 other files, not 3)

**Goal:** Address the remaining silent catches found in Step 1: `Assets/Ashfall.Core/Warlords/WarlordDoctrineCatalog.cs` (1 empty catch, line 363) and `Assets/Ashfall.Core/DoseContentCatalog.cs` (2 empty catches, lines 113 and 132). Corrected from the original "3 other files" — the verified sweep found exactly 2 other files, not 3, and neither was named in the original plan.

**Implementation:**
- Same logging pattern: warn on shape/parse failure with fallback, error on final-fallback-and-give-up.
- Accept `ILog` where not already threaded — check existing call sites in `Ashfall.Core.Tests/` and `src/Host/` for both files before deciding between an optional parameter and a required one.
- If a catch legitimately needs to swallow (e.g., probing for optional file existence), add explicit comment: `// Intentional: file is optional, absence is not an error`. Read each of the 3 remaining catch sites' surrounding code before assuming this exception — `WarlordDoctrineCatalog.cs:363` and `DoseContentCatalog.cs:113,132` must each be individually inspected for whether they're probing-for-optional-file catches or genuine error-swallowing before choosing the fix pattern; do not apply the same template to all three without reading the surrounding method first.

**Verification:**
- `dotnet build` — compiles
- `dotnet test` — all pass
- Zero silent (empty-body or catch-and-return-default-with-no-log) catches remaining in `Assets/Ashfall.Core/`, verified by re-running the two grep patterns from Step 1, not just the single empty-body pattern from the original plan (`grep -rn "catch.*{" Assets/Ashfall.Core/ | grep -v "//"` alone would not have caught the "catch { return X; }" cases, which is exactly how the original 13-count undercounted the true 19).

**Done when:** Both grep patterns from Step 1 (`catch\s*\{\s*\}` and `catch\s*\{[^}]*return[^}]*\}` with no `log.` call inside) find zero unlogged silent catches in `Assets/Ashfall.Core/`, H4 fully resolved.

---

## Step 5 — Add Loader Diagnostic Tests

**Goal:** Write tests that verify loaders produce meaningful diagnostics on malformed input (not silent empty returns).

**Implementation:**
Create `Ashfall.Core.Tests/CatalogLoaderDiagnosticTests.cs` (confirmed: this file does not currently exist under `Ashfall.Core.Tests/`, so this is new work, not a rename):
1. **VerdictLoader_MalformedJson_LogsWarning:** Feed garbage JSON, assert `ILog.Warn`/`Error` was called (per the `log?.Warn`/`log?.Error` split introduced in Step 2 — assert on whichever level the specific call site now uses).
2. **VerdictLoader_MissingFile_ReturnsEmpty:** Feed nonexistent path, assert empty list + no exception thrown (the current code already returns empty silently for a *missing* file via the `!fileIO.FileExists(path)` guard *before* any try/catch — this path never throws or logs today, and this batch does not add logging to the missing-file guard clause unless the plan is amended to require it; clarify this distinction in the test name/assertion so it doesn't imply a log call that the implementation doesn't make).
3. **YearOfAshLoader_MalformedShape_LogsAndFallsBackToFlatList:** Feed a JSON payload that fails the primary container-shape deserialize but succeeds as a flat list, assert the fallback still loads AND a warning was logged for the first attempt (corrected from "MalformedEntry_LogsAndContinues" — per Step 3's finding, this loader has no per-entry loop, so there is no "one bad entry, others load" scenario; the real fallback behavior is shape-level, not entry-level).
4. **YearOfAshLoader_MissingFile_ReturnsEmpty:** Same as above for missing file — same caveat as test 2 applies (the file-existence guard runs before any try/catch and never logs today).
5. **GenericLoader_EmptyFile_LogsWarning:** Feed empty string, verify appropriate diagnostic — note the `string.IsNullOrWhiteSpace(raw)` guard in both loaders also runs before any try/catch and returns empty silently today; decide explicitly whether this batch adds a log call to that guard clause too (it's a distinct silent-failure path not counted in the original "13 bare catches," but it has the same observability problem) and update the audit table in Step 1 to include it if so.

Use a test `ILog` implementation (`CapturingLog`, see Step 6) that captures messages for assertion.

**Verification:**
- `dotnet test --filter "CatalogLoaderDiagnostic"` — 5/5 pass
- Diagnostics are testable (ILog is injectable, not static)

**Done when:** 5 diagnostic tests pass, proving loaders are observable, and each test's name/assertion accurately reflects this loader's actual shape-level (not per-entry) fallback structure.

---

## Step 6 — Add CapturingLog Test Helper

**Goal:** Create a reusable `CapturingLog : ILog` test helper for asserting on log output. Confirmed via search: no `CapturingLog` class exists anywhere in the repository today — this is new work, not a "if not already present" conditional as the original plan hedged.

**Implementation:**
Create `Ashfall.Core.Tests/Helpers/CapturingLog.cs` (confirmed: `Ashfall.Core.Tests/Helpers/` — verify this directory exists before assuming the path; if it doesn't exist, create it):
```csharp
using System.Collections.Generic;
using Ashfall.Core;

namespace Ashfall.Core.Tests.Helpers
{
    public sealed class CapturingLog : ILog
    {
        public List<string> Infos { get; } = new();
        public List<string> Warnings { get; } = new();
        public List<string> Errors { get; } = new();

        public void Info(string message) => Infos.Add(message);
        public void Warn(string message) => Warnings.Add(message);
        public void Error(string message) => Errors.Add(message);
    }
}
```
Note the constructor signature matches `ILog`'s confirmed interface (`Assets/Ashfall.Core/Ports.cs:26-31`: `Info(string message)`, `Warn(string message)`, `Error(string message)` — parameter name is `message`, not `msg`; matched here for consistency with the real interface, though C# doesn't require matching parameter names).

**Verification:**
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — compiles
- Step 5 tests use CapturingLog and pass

**Done when:** Helper available for all future diagnostic tests, and its namespace/using matches how `Ashfall.Core.Tests/CatalogLoaderDiagnosticTests.cs` actually references it (no namespace mismatch at compile time).

---

## Step 7 — Document Error Handling Policy

**Goal:** Add a section to AGENTS.md or a `docs/error_handling_policy.md` that codifies the error handling expectations, and correct AGENTS.md's existing H4 row, which currently understates the true count.

**Implementation:**
Document:
- Catalog loaders MUST accept `ILog` (optional parameter acceptable for non-breaking rollout) and MUST log on parse failure
- Shape/format-fallback failures (e.g. container-shape vs. flat-list, as seen in `YearOfAshCatalogLoader`): log warning, fall through to the next shape attempt
- Final-fallback failures (all shape attempts exhausted): log error, return empty/default collection
- Never use bare `catch { }`, and never use `catch { return <default>; }` without a preceding log call — minimum is `catch (Exception ex) { log?.Warn(...); }` or `log?.Error(...)` before the return
- Save/load failures: log error, return failure indicator (not silent fallback)
- The only acceptable empty/unlogged catch is for probing optional file existence (must have comment) — and even then, prefer checking existence via `IFileIO.FileExists` before the try block (as the current loaders already do) over catching a file-not-found exception
- **Correct AGENTS.md's C-table/H-table H4 row**, which currently reads "13 bare catch blocks... `YearOfAshCatalogLoader.cs` (7), `VerdictCatalogLoader.cs` (3) — unchanged." Update it to reflect the verified counts from this batch (10 empty-body + 9 catch-and-return-default = 19 silent catches across 4 files: Verdict 4, YearOfAsh 12, WarlordDoctrineCatalog 1, DoseContentCatalog 2) before marking it resolved, so the historical record in AGENTS.md matches what was actually found and fixed.

**Verification:**
- Policy document committed
- Consistent with actual implementation
- AGENTS.md's H4 row is edited in the same commit/PR as the policy document, not left showing stale numbers after this batch closes

**Done when:** Policy documented, H4 fully resolved, AGENTS.md updated to mark H4 resolved with the corrected counts (not the original, undercounted "13").

---

## Summary

| Step | Target | Catches Fixed | Tests Added |
|------|--------|--------------|-------------|
| 1 | Audit | 0 (analysis) | 0 |
| 2 | VerdictCatalogLoader | 4 (corrected from 3) | 0 |
| 3 | YearOfAshCatalogLoader | 12 (corrected from 7) | 0 |
| 4 | Remaining files (`WarlordDoctrineCatalog.cs`, `DoseContentCatalog.cs` — 2 files, corrected from vague "3 other files") | 3 (file count corrected: 2 files not 3, catch count of 3 happens to match original estimate) | 0 |
| 5 | Diagnostic tests | 0 | 5 |
| 6 | CapturingLog helper | 0 | 0 |
| 7 | Policy document + AGENTS.md correction | 0 | 0 |

**End state:** Zero silent (unlogged empty-body or catch-and-return-default) catch blocks in Core — 19 fixed in total, not the originally estimated 13. All loader failures are logged with context. Diagnostic tests prove observability. H4 fully resolved, and AGENTS.md's own count is corrected to match.

---

## Review Notes (Corrected)

This file was adversarially reviewed against the actual repository at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War` and the following corrections were made:

1. **Wrong catch count — VerdictCatalogLoader.cs.** Original plan: "3 bare `catch { }` blocks." Verified via direct read and grep: there is exactly 1 truly-empty `catch { }` (line 166), plus 3 `catch { return result; }` blocks (lines 51, 105, 141) that have a body and thus don't match "bare catch with no body" literally, though they're equally silent (no logging). Corrected the plan's scope to cover all 4 silent catches in this file, and renamed the category from "bare catch" to "silent catch" throughout to avoid the ambiguity that caused the undercount.

2. **Wrong catch count — YearOfAshCatalogLoader.cs.** Original plan: "7 bare `catch { }` blocks." Verified: 6 truly-empty `catch { }` (lines 152, 183, 255, 263, 294, 325) plus 6 `catch { return new List<...>(); }` (lines 158, 189, 269, 300, 331, 362) = 12 silent catches, not 7. Corrected Step 3's scope and Done-when criterion accordingly.

3. **False structural claim — "per-entry" catches in YearOfAshCatalogLoader.** The original Step 3 described "per-entry catches: log warning, skip entry, continue," implying a loop over individual array entries where one bad entry is skipped and others still load. Verified via full read of the file: there is no per-entry loop in any of the 6 load methods. Each method has exactly two sequential try/catch pairs — first attempting a wrapped-container JSON shape, then falling back to a flat-list shape — both are shape-level, not entry-level. Corrected Step 3's implementation guidance and Step 5's test names/descriptions (`YearOfAshLoader_MalformedEntry_LogsAndContinues` renamed to `YearOfAshLoader_MalformedShape_LogsAndFallsBackToFlatList`) to match the actual control flow, since a test asserting "one bad entry, others load" would not compile against any real failure mode in this file.

4. **Wrong file count for Step 4 — "3 other files."** Original plan assumed 3 additional files beyond Verdict/YearOfAsh contain bare catches, without naming them. A full-repo grep of `Assets/Ashfall.Core/` for empty catch blocks found exactly 2 additional files: `Assets/Ashfall.Core/Warlords/WarlordDoctrineCatalog.cs` (1 instance, line 363) and `Assets/Ashfall.Core/DoseContentCatalog.cs` (2 instances, lines 113 and 132) — 3 catches total across 2 files, not 3 files. Named both files explicitly in the corrected Step 4 so the task is directed rather than requiring a re-audit to discover what "Step 1" was supposed to have already found.

5. **Wrong error-detection command in original Step 4's Done-when.** `grep -rn "catch.*{" Assets/Ashfall.Core/ | grep -v "//"` only matches catch blocks textually on one line with an open brace — it would not reliably find catches with the body on a following line, and critically, it would never distinguish a logged catch (`catch (Exception ex) { log.Warn(...); }`, itself matching `catch.*{`) from an unlogged one, making the stated verification command nearly useless as a completion gate. Replaced with the two explicit patterns from the corrected Step 1 (empty-body and catch-and-return-with-no-log-call), which is what the original 13-vs-19 discrepancy was actually caused by.

6. **Incorrect data format assumption.** Step 3's verification said "Corrupt a YAML entry, verify logged." `YearOfAshCatalogLoader` loads JSON exclusively (see `ItemsFile = "year_of_ash_items.json"`, etc., and `IJsonSerializer json` parameter) — there is no YAML anywhere in this loader or its data files. Corrected to "Corrupt a `year_of_ash_*.json` entry."

7. **Unverified assumption — CapturingLog "if not already present."** The original Step 6 hedged with "if not already present," implying uncertainty about whether this helper exists. Verified via repository-wide search: no `CapturingLog` class exists anywhere in the codebase today. Removed the hedge — this is definitely new work — and added the exact confirmed `ILog` interface signature (`Assets/Ashfall.Core/Ports.cs:26-31`) so the helper's method signatures are guaranteed to compile against the real interface rather than the plan's guessed `Info(string msg)` parameter naming.

8. **Missing risk/rollback and breaking-change call-out (new finding, not in original plan).** Steps 2–4 change public static method signatures on catalog loaders (adding an `ILog` parameter). The original plan did not address whether this is a breaking change to existing callers. Added an explicit instruction to audit callers in `Ashfall.Core.Tests/` and `src/Host/` before deciding between a required parameter and an optional (`ILog log = null`) one, since a required-parameter change would break every existing call site and constitutes a wider blast radius than "add logging" implies.

9. **AGENTS.md itself is stated inaccurately and this batch would have perpetuated it.** AGENTS.md's H4 row says "13 bare catch blocks... unchanged," which — per the verified count above — undercounts the real number (19) and omits 2 files (`WarlordDoctrineCatalog.cs`, `DoseContentCatalog.cs`) entirely. Added an explicit requirement in Step 7 to correct AGENTS.md's own historical count, not just strike through the row, so the project's own known-issues ledger doesn't end up permanently recording the wrong number even after the underlying bug is fixed.

10. **Confirmed accurate (no change needed):** the `ILog` interface shape (`Info`/`Warn`/`Error`, each `(string message)`) at `Assets/Ashfall.Core/Ports.cs`, the `Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` project path used in verification commands, the `--data-integrity-selftest` CLI verb (confirmed present in `src/Host/HostCli.cs`), and the general fail-loud-but-graceful-degradation policy direction were all verified sound.
