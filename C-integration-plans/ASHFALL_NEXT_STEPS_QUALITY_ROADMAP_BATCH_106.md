# ASHFALL — Quality Roadmap Batch 106

## Theme: Command-Line Interface Expansion — Complete Headless Management Tool

| Field | Value |
|-------|-------|
| **Priority** | MEDIUM |
| **Risk** | Low for the *new* verbs (Steps 4, 5, purely additive read-only diagnostics) — **Medium-High for Step 2** (replaces the dispatch mechanism for all 75 existing `HostCliAction` values, corrected from "71" — see Review Notes, across 3 files (`HostCli.cs`, `HostCli.SelfTests.cs`, `HostCli.PanelTests.cs`) plus the `Main.cs` switch; a mistake there can silently break or mis-route an existing self-test used by CI/other AI clients) — **Medium for Step 3** (no unified save-slot model exists; real `user://` file overwrite risk) — **Medium for Step 6** (ticks real systems headlessly; hang/crash risk in CI; introduces a new, currently-nonexistent `scenarios/` data directory). See Review Notes. |
| **Category** | Developer Tooling / CI Infrastructure |
| **Blocked by** | None |
| **Blocks** | Advanced CI pipelines, automated QA scenarios, contributor onboarding |
| **Estimated scope** | ~1.5 weeks (framework + 12 new verbs + tests) — **underestimated, see Review Notes**: the corrected Step 2 scope (two dispatch layers across 3 files, not one), the Step 3 redesign around 28 independent save stores instead of a nonexistent numbered-slot model, the Step 6 requirement to design and create a new `scenarios/` JSON schema from scratch, and the Step 7 test-placement correction (Godot-host selftest verbs, not `dotnet test`) all add real time not accounted for in the original estimate. Budget ~2.5-3 weeks.

---

## Problem Statement

The project already has extensive CLI self-tests via `godot --headless`:
- `--data-integrity-selftest`
- `--bridge-selftest`
- `--holdfast-selftest`
- `--survivors-selftest`
- `--expedition-encounter-bridge-selftest`
- **75** `HostCliAction` enum values total (corrected in this pass from "71" — the prior correction undercounted; re-verified by counting every comma-terminated member plus the final member between `public enum HostCliAction {` and its closing brace in `src/Host/HostCli.cs`: 75, cross-checked against 74 `case HostCliAction.*:` arms in `src/Main.cs`'s dispatch switch — the one enum value with no case arm is `HostCliAction.Interactive`, the implicit no-args/default game-boot state, which is correctly never switched on), corresponding to **107** distinct recognized `--flag` strings (many verbs have 2-3 aliases, e.g. `--holdfast-runtime-uitest` / `--holdfast-runtime-ui-test` / `--holdfast-runtime-selftest` all map to the same action)

However, the current CLI infrastructure suffers from:

1. **No unified help system** — running `--help` does not list available verbs or describe their usage — **partially wrong, see Review Notes.** `--help`/`--host-help` already exist and are wired (`HostCliAction.Help` → `HostCli.PrintHelp()`), and `PrintHelp()` already documents 48 distinct `--flag` strings across 47 `GD.Print` calls (some calls document 2-3 aliases in one multi-line string; corrected in this pass from "~35," which undercounted — re-verified by extracting every `--flag-name` token printed inside `PrintHelp()` and de-duplicating: 48). The real gap is narrower: `PrintHelp()`'s list is hand-maintained and has drifted out of sync with the actual dispatch table in `Parse()` — verified: `Parse()` recognizes 107 distinct `--flag` strings (including aliases) across 75 `HostCliAction` enum values (corrected from "71," see Review Notes), but `PrintHelp()` documents only 48 of them, so more than half the real flags (about 59 of 107, concentrated in aliases and several newer expansion verbs like `--warlord-*`, `--radio-selftest`, `--expedition-panel-uitest`, `--ui-snapshot-uitest`) are undocumented — not that no help exists at all, and not "roughly half" as a round-number approximation (it is closer to 55%). There is also no per-verb `--help <verb>` (that part of the complaint is accurate).
2. **No argument parsing library** — verbs are hardcoded string comparisons — **location corrected, see Review Notes.** The string comparisons are not in `Main.cs`. They live in `src/Host/HostCli.cs`, in `HostCli.Parse(string[] args)`, which calls a `Has(args, "--flag")` helper repeatedly and maps the result to a `HostCliAction` enum value. `Main.cs` (7,014 lines, not ~6.5k as informally described elsewhere) then does a `switch (HostCli.Parse(...))` over that enum and dispatches to `HostCli.RunXxxSelfTest(...)` methods (themselves spread across two more partial files, `HostCli.SelfTests.cs` and `HostCli.PanelTests.cs`). So today's structure is already a two-stage design (string→enum, then enum→handler) — it is ad-hoc and stringly-typed, which is a legitimate complaint, but it is not literally "an if/else chain in `Main.cs`," and any implementation plan that edits `Main.cs` expecting to find the string comparisons there will be looking in the wrong file.
3. **No argument values** — most verbs are boolean flags; none accept `--key value` parameters — verified accurate; `Has(args, ...)` in `HostCli.cs` only checks flag presence, there is no key-value parsing anywhere in the file.
4. **Missing management operations** — common development tasks require manual steps:
   - Exporting a save as readable JSON for debugging
   - Validating a single data file without running full integrity suite
   - Running a single system tick to observe behavior
   - Benchmarking system performance
   - Advancing simulation days for testing late-game behavior
5. **No verb discovery** — new contributors must read `src/Host/HostCli.cs`'s `Parse()` method (and, for descriptions, its `PrintHelp()`) to find available commands and their aliases — corrected from "`Main.cs`"; `Main.cs` only shows which enum values exist, not which `--flag` strings trigger them
6. **No structured output** — all verbs print ad-hoc text; no `--json` option for CI parsing

The result: developers waste time on tasks that should be one-liners, CI pipelines cannot extract structured results, and new contributors cannot self-serve.

---

## Architecture Decision

**Approach:** Build a `CliRouter` in the Godot host (`src/CLI/`) that provides:
- A verb registry with metadata (name, description, arguments, examples)
- An argument parser supporting `--key value`, `--flag`, and positional arguments
- Auto-generated `--help` (global and per-verb)
- Structured output option (`--format json` for CI)

**Design constraints:**
- CLI framework lives in `src/CLI/` (Godot host), not Core — it depends on Godot's headless mode
- Core systems accessed via CLI must not gain Godot dependencies — the CLI calls into Core via existing interfaces
- Existing self-test verbs must continue working unchanged (backward compatibility)
- No external dependencies — the argument parser is hand-written (the project avoids NuGet for the Godot host)
- **Corrected constraint (see Review Notes):** the migration touches three existing files, not one — `src/Host/HostCli.cs` (326 lines: the `HostCliAction` enum + `Parse()` string matching), `src/Host/HostCli.SelfTests.cs` (681 lines: `RunXxxSelfTest` handler bodies), and `src/Host/HostCli.PanelTests.cs` (2,867 lines: UI-test handler bodies) — plus the `switch` statement in `src/Main.cs` (7,014 lines) that calls `HostCli.Parse()` and dispatches on the resulting enum. A migration plan that only mentions editing `Main.cs` will miss where the actual string-comparison logic and most handler bodies live.

**Not chosen:**
- System.CommandLine (NuGet) — adds external dependency for simple needs
- Reflection-based verb discovery — too magical; explicit registration is clearer
- Moving CLI to a separate console app — loses access to Godot scene tree for UI testing

---

## Steps

### Step 1: Design CLI Framework (Verb Registry + Argument Parser)

**Goal:** Define the internal architecture for verb registration, argument parsing, and help generation that all future verbs will use.

**Implementation:**
- Create `src/CLI/CliRouter.cs`:
  ```csharp
  namespace AtomicWar.GodotApp.CLI;

  public sealed class CliRouter
  {
      private readonly Dictionary<string, CliVerb> _verbs = new();

      public void Register(CliVerb verb) { ... }
      public int Execute(string[] args) { ... }
      public string GenerateHelp() { ... }
      public string GenerateVerbHelp(string verbName) { ... }
  }
  ```
- Create `src/CLI/CliVerb.cs`:
  ```csharp
  public sealed class CliVerb
  {
      public string Name { get; init; }           // e.g., "export-save"
      public string Description { get; init; }    // one-line summary
      public string LongDescription { get; init; } // multi-line help
      public CliArg[] Arguments { get; init; }    // declared args
      public string[] Examples { get; init; }     // usage examples
      public Func<CliContext, int> Handler { get; init; }
  }
  ```
- Create `src/CLI/CliArg.cs`:
  ```csharp
  public sealed class CliArg
  {
      public string Name { get; init; }        // e.g., "slot"
      public string ShortName { get; init; }   // e.g., "s"
      public string Description { get; init; }
      public bool Required { get; init; }
      public bool IsFlag { get; init; }        // true = no value expected
      public string DefaultValue { get; init; }
  }
  ```
- Create `src/CLI/CliContext.cs`:
  ```csharp
  public sealed class CliContext
  {
      public string VerbName { get; }
      public IReadOnlyDictionary<string, string> Options { get; }
      public IReadOnlyList<string> Positional { get; }
      public OutputFormat Format { get; }  // Text or Json
      public TextWriter Out { get; }       // stdout or capture buffer

      public string GetRequired(string name) { ... }
      public string GetOptional(string name, string fallback) { ... }
      public bool HasFlag(string name) { ... }
  }
  ```
- Argument parsing rules:
  - `--verb-name` triggers verb lookup
  - `--key value` or `--key=value` sets option
  - `-k value` short form
  - `--flag` (declared IsFlag) sets to "true"
  - Positional args collected in order after verb
  - Unknown args: warning to stderr, non-fatal
  - `--help` after verb: show verb-specific help
  - `--format json`: set output format to JSON

**Verification:**
```bash
dotnet build Ashfall.csproj   # Framework compiles (src/CLI/ is part of the Godot host project, not Ashfall.Core.Tests)
```
**Corrected — the filter test below will not find anything.** `CliRouter`/`CliVerb`/`CliArg`/`CliContext` are declared in `src/CLI/` inside `Ashfall.csproj` (the Godot host, `Godot.NET.Sdk/4.7.1`, `net8.0`). `Ashfall.Core.Tests.csproj` is a separate `net9.0` project with no reference to the Godot host assembly (verified: its `<PackageReference>` list contains only test-framework packages, and nothing in this plan adds a project reference from `Ashfall.Core.Tests` to `Ashfall.csproj`). Running `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "CliParser"` will simply report "no tests matched" rather than exercising the new parser — it will not fail loudly, which makes this a silent gap, not a build error. Either (a) add the CLI parser/router tests as a Godot-host-side test verb (e.g. `--cli-parser-selftest`, consistent with the project's existing headless-selftest pattern), or (b) if unit-test-style coverage via `dotnet test` is required, create a new test project that references `Ashfall.csproj` (or extract `CliRouter`/`CliVerb`/`CliArg`/`CliContext` into a Godot-independent library `src/CLI/` can wrap, since none of their declared members in this plan actually reference `Godot.*` types — only `Main.cs`'s call site does). Option (b) is cleaner long-term but is scope this plan does not currently budget for.
```bash
# If option (a): a real, runnable equivalent
godot --headless --path . -- --cli-parser-selftest
```

**Done when:**
- `CliRouter` can register verbs and dispatch to handlers
- Argument parser correctly handles `--key value`, `--key=value`, `-k value`, `--flag`, positional
- Help generator produces formatted output for global and per-verb help
- Error messages are clear for missing required args, unknown verbs
- Unit tests cover: valid args, missing required, unknown verb, flag vs value, short names

**Risk & Rollback:** Low risk — this step is purely additive (new classes in a new `src/CLI/` directory), with zero call sites until Step 2 wires it in. Rollback: delete the `src/CLI/` directory; nothing in the existing `HostCli.cs`/`Main.cs` dispatch changes until Step 2.

---

### Step 2: Implement CliRouter in Godot Host (Replace String Switch)

**Goal:** Replace the existing string-comparison verb dispatch (`HostCli.Parse()`) and enum-switch dispatch (`Main.cs`) with the new `CliRouter`, maintaining full backward compatibility.

**Implementation:**
- **Corrected — the "before" code below does not match `Main.cs` today.** `Main.cs` does not contain a raw `if (args.Contains(...))` chain; it contains `switch (HostCli.Parse(OS.GetCmdlineUserArgs()))` (confirmed at `src/Main.cs:279`) with one `case HostCliAction.XxxSelfTest:` arm per verb, each calling into `HostCli.RunXxxSelfTest(...)`. The actual `if (Has(args, "--flag")) return HostCliAction.Xxx;` chain lives in `src/Host/HostCli.cs`'s `Parse()` method. The migration must replace **both** layers — the string-matching in `HostCli.Parse()` and the enum-switch in `Main.cs` — not just one "giant if/else chain" in a single file:
  ```csharp
  // Before (src/Host/HostCli.cs, HostCli.Parse): string → enum
  if (Has(args, "--data-integrity-selftest")) return HostCliAction.DataIntegritySelfTest;
  else if (Has(args, "--bridge-selftest")) return HostCliAction.BridgeSelfTest;
  // ... 100+ more Has(args, ...) checks across 75 enum values (corrected from "71" — see Review Notes)

  // Before (src/Main.cs, around line 279): enum → handler
  switch (HostCli.Parse(OS.GetCmdlineUserArgs()))
  {
      case HostCliAction.DataIntegritySelfTest:
          GetTree().Quit(HostCli.RunDataIntegritySelfTest(_dataDir));
          break;
      // ... 70+ more cases
  }

  // After: unified router, replacing both layers
  var router = new CliRouter();
  RegisterAllVerbs(router);  // All existing + new verbs
  var exitCode = router.Execute(args);
  GetTree().Quit(exitCode);
  ```
- Migrate ALL existing `--*-selftest` verbs to `CliVerb` registrations:
  - Each keeps its exact `--verb-name` (no breaking changes)
  - Each gains a description and is visible in `--help`
  - Each returns exit code 0 (pass) or 1 (fail)
- Add global `--help` verb that lists all registered verbs grouped by category:
  ```
  ASHFALL CLI — Headless Management Tool

  SELF-TESTS:
    --data-integrity-selftest    Validate all JSON data files
    --bridge-selftest            Verify bridge removal notice (exits 0)
    --holdfast-selftest          Run Holdfast expansion smoke tests
    ...

  MANAGEMENT:
    --export-save <slot>         Export save slot as readable JSON
    --import-save <file>         Import external save file
    ...

  DIAGNOSTICS:
    --profile-tick               Profile each system's tick time
    --check-invariants           Run all architectural invariant checks
    ...

  Use --help <verb> for detailed help on any command.
  ```
- Add `--version` verb (prints project version, Godot version, .NET version)
- Ensure `Main.cs` partial file structure is respected — add `Main.CLI.cs` partial for CLI wiring
- **Corrected — this partial file plan is incomplete.** `Main.cs`'s `switch` is only one of the two things being replaced. The 107-flag/75-enum-value (corrected from "71" — see Review Notes) `Parse()` string-matching in `HostCli.cs` and the handler bodies in `HostCli.SelfTests.cs`/`HostCli.PanelTests.cs` (3,548 lines combined) also need a migration path: either (a) `RegisterAllVerbs(router)` calls into the *existing* `HostCli.RunXxxSelfTest(...)` methods unchanged (minimal risk — the router becomes a thin dispatcher in front of existing handler bodies, and `HostCli.cs`'s `Parse()`/`Has()` machinery is deleted once all 75 verbs are re-registered through the router), or (b) handler bodies are also moved/rewritten (higher risk, larger diff, no clear benefit stated in this plan). The plan should explicitly choose (a) and say so, since it is the only option consistent with "zero behavioral change."

**Verification:**
```bash
dotnet build Ashfall.csproj
godot --headless --path . -- --help                    # Lists all verbs
godot --headless --path . -- --data-integrity-selftest # Still works
godot --headless --path . -- --bridge-selftest         # Still works
godot --headless --path . -- --version                 # Prints versions
# Regression sweep: every one of the 107 existing --flag strings (with aliases) must still resolve
# to the same behavior. This plan does not currently script that sweep — add one before merging Step 2,
# e.g. a loop that runs each known flag with --validate-only/dry-run semantics where available.
```

**Done when:**
- All existing self-test verbs work identically to before (zero behavioral change) — verified against **all 107** known `--flag` strings (including aliases) across **75** `HostCliAction` enum values (corrected from "71" — see Review Notes), not just the 4 spot-checked above
- `--help` lists every registered verb with description
- `--help <verb>` shows detailed help for any verb
- `--version` prints project/engine/runtime versions
- No increase in `Main.cs` line count (CLI code in `Main.CLI.cs` partial) — **and no corresponding line-count blowup in `HostCli.cs`/`HostCli.SelfTests.cs`/`HostCli.PanelTests.cs`**; the original "no increase in `Main.cs`" criterion only guards one of the two files that actually contain the logic being replaced

**Risk & Rollback:** Medium-High risk — this is the step that actually touches all 75 existing verbs' dispatch path (corrected from "71" — see Review Notes), used today by CI, other AI clients (per AGENTS.md's "AI CLIENT / CLOUD RUNNER RULE"), and manual contributor workflows. A subtle bug (e.g. an alias silently dropped, an exit code changed) breaks existing automation rather than adding new capability. Rollback: land this step as its own commit, separate from Steps 3-6 — if a regression surfaces, this commit can be reverted independently, falling back to the original `HostCli.Parse()`/`Main.cs switch` without losing the new-verb work in later steps, provided those steps are also landed as independent commits that register through the router but don't depend on Step 2's specific internals surviving. Note Steps 3 and 6 are additive in the sense of "new verb registrations, no edits to existing dispatch code" but are not risk-free in their own right — see their own corrected Risk & Rollback sections for the real-I/O and real-tick risks those two steps introduce independently of Step 2.

---

### Step 3: Add Management Verbs (Save Export/Import)

**Goal:** Enable developers and QA to export saves as human-readable JSON and import external saves for testing.

**Corrected — the premise of a single numbered "save slot" does not match this codebase, see Review Notes.** There is no unified save-slot system to export from. The Godot host has **28 separate `*SaveStore` classes** in `src/Host/` (e.g. `HoldfastSaveStore`, `DutyRosterSaveStore`, `SurvivorsSaveStore`, `InventorySaveStore`, `EconomySaveStore`, `MedicalSaveStore`, `NarrativeSaveStore`, `WorldSaveStore`, `RadioSaveStore`, `CombatSaveStore`, and 18 more), each independently exposing a static `SavePath`/`Exists` under `user://` and each with its own `CaptureState()`/`RestoreState()` DTO — this matches AGENTS.md's SAVE/LOAD section describing per-system checksummed envelopes, not a single indexed save file. `--export-save <slot>` and `--list-saves` as specified assume a "slot 0 / slot 1 / auto" model that would require inventing a new aggregation layer over these 28 independent stores — a nontrivial new subsystem, not a thin CLI wrapper over something that already exists. Revised scope below.

**Implementation (revised):**
- **`--export-save`** verb — rescoped to name a specific system's store rather than a numbered slot:
  ```
  Usage: --export-save <system> [--output <path>] [--pretty] [--format json]

  Exports the named system's save store (one of the 28 *SaveStore classes, e.g.
  "holdfast", "survivors", "inventory", "economy" — see --list-saves for the
  full set) as formatted JSON to stdout or a file. Useful for debugging save
  corruption, comparing save states, and QA.

  Arguments:
    <system>       Save store name (see --list-saves for valid names)
    --output, -o   Output file path (default: stdout)
    --pretty       Pretty-print JSON with indentation (default: true)
  ```
  - Loads the named store via its existing static `SavePath`, using `IJsonSerializer` (not per-system ad-hoc `File.ReadAllText`/parsing)
  - There is no cross-system "--systems Needs,Radiation" filter to build here — `Needs`/`Radiation` are Core systems inside a single save's state tree (e.g. inside `SurvivorsSaveStore`'s payload), not separate stores; a filter at that granularity would require walking into each store's DTO shape, which is out of scope for this step and not attempted
  - Outputs valid JSON that can be round-tripped back via `--import-save`
  - Does not invent a synthetic "unified save" concept; exports exactly what the named `*SaveStore` already persists

- **`--import-save`** verb — same system-name correction:
  ```
  Usage: --import-save <system> <file> [--validate-only]

  Imports a JSON save file into the named system's save store (overwriting
  its user:// SavePath).

  Arguments:
    <system>         Save store name (see --list-saves)
    <file>           Path to JSON save file
    --validate-only  Parse and validate without writing (dry run)
    --force          Overwrite existing save without confirmation
  ```
  - Validates JSON structure against the target store's expected DTO shape before writing (each store's `RestoreState` already throws on malformed input per AGENTS.md's SAVE/LOAD versioned-migration pattern — reuse that, don't reimplement validation)
  - Checks `SaveChecksum` integrity for the 23 stores that have it; for the 5 stores AGENTS.md's Known Gaps lists as checksum-incomplete or on the legacy bare-state fallback path (`ExpeditionSaveStore`, `MedicalSaveStore`, `NarrativeSaveStore`, `WorldSaveStore`, `JournalSaveStore` — AGENTS.md states these "now ship checksummed envelopes," so re-verify their current checksum status directly in code at implementation time rather than trusting this list, since AGENTS.md itself may have drifted since it was last edited), degrade gracefully to the same bare-state fallback those stores already use, rather than hard-failing on a missing `Checksum` field
  - `--validate-only` mode for CI (verify a save file is well-formed for a specific store)

- **`--list-saves`** verb — becomes the store registry/discovery verb, replacing the assumed slot enumeration:
  ```
  Usage: --list-saves [--format json]

  Lists all 28 known save stores by name, each with: Exists (bool),
  SavePath, and — for stores with checksum support — the checksum
  algorithm version. Does not report "day, timestamp, checksum, size"
  as originally drafted, since those fields are store-specific DTO
  contents, not store-registry metadata; a store-specific summary
  (e.g. current in-game day) requires deserializing that store's
  particular DTO and is deferred to a per-store detail flag if needed
  later, not promised by this step.
  ```

**Verification:**
```bash
dotnet build Ashfall.csproj
godot --headless --path . -- --list-saves --format json
godot --headless --path . -- --export-save holdfast --output /tmp/holdfast_export.json
godot --headless --path . -- --import-save holdfast /tmp/holdfast_export.json --validate-only
```

**Done when:**
- `--list-saves` enumerates all 28 known `*SaveStore` classes by name with accurate `Exists`/`SavePath` info
- Export produces valid, readable JSON for any named store that currently exists on disk
- Import successfully loads exported JSON back into the named store
- Round-trip: export → import → export produces identical JSON for at least 3 different stores (not just one, since DTO shapes differ per store)
- `--validate-only` correctly catches malformed saves (exit code 1) for stores with checksum support, and correctly falls back to bare-state validation for the stores still on the legacy path
- JSON format option produces machine-parseable output
- Neither verb invents a numbered "slot" concept that does not exist in the underlying save architecture

**Risk & Rollback:** Medium risk — this step touches real save-file I/O (`--import-save --force` can overwrite an existing `user://` save with no undo), and the corrected per-store design means 28 distinct DTO shapes must each be handled, not one. A bug in `--import-save` could corrupt a real save store; a bug in export could produce JSON that looks valid but silently drops fields. Mitigations: (1) default `--force` to off, requiring explicit confirmation before overwrite; (2) implement and test against 3-4 representative stores first (e.g. one with full checksum support, one on the legacy bare-state path) before generalizing to all 28; (3) `--validate-only` must be exercised in CI before this step is considered done, not just documented as a nice-to-have. Rollback: these are new, additive verbs with no changes to the `*SaveStore` classes themselves (read/write via their existing public `SavePath`/`CaptureState`/`RestoreState` surface) — deleting the verb registrations fully reverts this step with no residual effect on save compatibility.

### Step 4: Add Diagnostic Verbs (Profile + Invariant Check)

**Goal:** Give developers visibility into system performance and architectural health from the command line.

**Implementation:**
- **`--profile-tick`** verb:
  ```
  Usage: --profile-tick [--days <n>] [--top <n>] [--format json]

  Advances simulation by <n> days (default: 10) and reports time spent in each system.

  Arguments:
    --days, -d     Number of days to simulate (default: 10)
    --top, -t      Show only top N slowest systems (default: all)
    --threshold    Only show systems taking > N ms per tick (default: 0)
    --seed         RNG seed for reproducibility (default: 42)
  ```
  - Creates a fresh game state with the given seed
  - Wraps each system's `Tick`/`TickSimDay` in a `Stopwatch`
  - Reports: system name, total ms, avg ms/tick, % of total, call count
  - Sorted by total time descending
  - Output example:
    ```
    System Performance (10 days, seed=42):
    ──────────────────────────────────────────────
    WeatherSystem          45.2ms  (4.5ms/tick)  32.1%
    NeedsSystem            28.7ms  (2.9ms/tick)  20.4%
    RadiationSystem        22.1ms  (2.2ms/tick)  15.7%
    ...
    ──────────────────────────────────────────────
    Total: 140.8ms for 10 ticks (14.1ms/tick avg)
    ```

- **`--check-invariants`** verb:
  ```
  Usage: --check-invariants [--format json]

  Runs all architectural invariant checkers and reports violations.

  Checks:
    - Invariant 1: No engine coupling in Core (scan for UnityEngine/Godot refs)
    - Invariant 2: Ports and Adapters (all interfaces have implementations)
    - Invariant 3: Cross-host save compatibility (see corrected checks below)
    - Invariant 4: Determinism (scan for System.Random, Guid.NewGuid)
    - Invariant 5: No gameplay logic in hosts (flag large host files)
    - Invariant 6: Data authority (no ScriptableObject as source of truth)
  ```
  - **Corrected — Invariant 3 was missing entirely, see Review Notes.** AGENTS.md defines exactly 6 invariants; the original draft of this verb checked 1, 2, 4, 5, 6 and silently skipped Invariant 3 (Cross-host save compatibility) — arguably the most load-bearing one, since AGENTS.md's own Critical Known Issue C1 (`JsonUtility` in Unity `SaveSystem` blocks cross-host saves) is a live, unresolved Invariant 3 violation today. A `--check-invariants` verb that omits the one invariant with a known, currently-true violation would report false confidence. Added: Invariant 3 check = scan `Assets/_Game/Core/SaveSystem.*.cs` and the 21 remaining `*CatalogLoader.cs` files (AGENTS.md C6) for `JsonUtility` call sites and report them as violations (expected non-zero today — this check should report the *known* count and only fail CI if the count *increases* beyond the current baseline, matching the grandfathering pattern used elsewhere in these two batches for exactly this reason).
  - Exit code 0: all invariants hold (or, for Invariant 3, violation count is at or below the recorded baseline)
  - Exit code 1: violations found (lists each with file:line)
  - JSON output includes violation count per invariant

- **`--system-graph`** verb:
  ```
  Usage: --system-graph [--format dot|json|text]

  Outputs the dependency graph between Core systems.
  Shows which systems reference which, event subscriptions, and tick order.
  ```

**Verification:**
```bash
dotnet build Ashfall.csproj
godot --headless --path . -- --profile-tick --days 5 --seed 42
godot --headless --path . -- --check-invariants
godot --headless --path . -- --system-graph --format text
```

**Done when:**
- `--profile-tick` reports per-system timing for any number of days
- Results are reproducible with same seed
- `--check-invariants` covers all **6** invariants from AGENTS.md, not 5 (corrected — see Review Notes)
- `--check-invariants` catches known violations (from AGENTS.md known issues), including the Invariant 3 / C1 `JsonUtility` violation, with a captured baseline count so existing known violations don't fail CI immediately
- `--check-invariants` exits 0 when no new violations are introduced beyond the baseline
- `--system-graph` outputs readable dependency information
- All verbs support `--format json` for CI consumption

**Risk & Rollback:** Low risk — these are new, read-only diagnostic verbs; `--profile-tick` and `--system-graph` only read/instrument existing systems and produce a report, `--check-invariants` is static/reflective analysis. The only real risk is `--profile-tick` creating a "fresh game state" — confirm this reuses the same bootstrap path as `Main.cs`'s normal startup (per H7's `SetupXxx`/`SaveXxx`/`FlushXxxIfDirty` triads) rather than a parallel, drifting bootstrap that could silently diverge from real game behavior over time. Rollback: delete the new verb registrations; no existing system code is modified by this step.

---

### Step 5: Add Data Verbs (File Validation + ID Listing)

**Goal:** Enable targeted data file operations without running the full integrity suite.

**Corrected — `CatalogIntegrityValidator.Validate` does not support single-file scope, see Review Notes.** Its only public entry point is `public static CatalogIntegrityReport Validate(string dataDirectory, IFileIO files)` — it takes a directory, not a file, because TIER-1/TIER-2 reference checks are inherently cross-file (an id defined in `items.json` may be referenced from `quests.json`, `recipes.json`, etc.), so validating "just one file" in isolation cannot correctly resolve references without also loading the rest of the catalog. `--validate-file <path>` as specified ("without running the full integrity suite") is not achievable as a pure subset operation without either (a) still loading the full directory internally and filtering the *report* to violations whose file matches `<path>` (registry/uniqueness checks scoped correctly, but TIER-1/TIER-2 reference checks still require the full catalog to resolve correctly — this is the only sound option), or (b) accepting that single-file mode only runs the file-local checks (REGISTRY, UNIQUENESS, RANGES) and explicitly documents that TIER-1/TIER-2 cross-references are NOT checked in this mode (weaker, but genuinely file-scoped and faster). Pick (a) and document the "not truly full-suite-free" caveat, since (b) risks giving a false-positive "valid" result for a file with a dangling reference to something outside it.

**Implementation:**
- **`--validate-file`** verb:
  ```
  Usage: --validate-file <path> [--strict] [--format json]

  Runs CatalogIntegrityValidator.Validate() against the full data directory
  (required for correct cross-file reference resolution) and filters the
  reported errors/warnings to those originating in <path>. This is faster
  to read, not faster to run — see corrected Architecture note above.

  Arguments:
    <path>       Path to JSON file (relative to StreamingAssets/Data/) to filter results to
    --strict     Fail on warnings, not just errors
  ```
  - **Corrected: dropped `--fix` (auto-fix trivial issues).** Not scoped in this plan — auto-mutating data-authority JSON files from a CLI flag is a meaningfully different, riskier feature (silent bulk edits to the authority) than reporting violations, and none of Steps 1-7 budget time to design safe auto-fix semantics (dry-run preview? backup before write? which "trivial" fixes are safe to automate?). Cut it from this batch; revisit as its own future-work item with its own design.
  - Uses existing `CatalogIntegrityValidator` infrastructure, run against the full directory, filtered to `<path>` in the report layer
  - Reports: REGISTRY hits, TIER-1 refs, TIER-2 refs, RANGE checks, UNIQUENESS — all scoped for display to the target file, but computed with full-catalog context
  - Exit code 0: valid, 1: errors found, 2: warnings only (with --strict: fail)

- **`--list-ids`** verb:
  ```
  Usage: --list-ids [<prefix>] [--file <path>] [--count] [--format json]

  Lists all registered IDs, optionally filtered by prefix or source file.

  Arguments:
    <prefix>     ID prefix filter (e.g., "item_", "quest_", "npc_")
    --file, -f   Only show IDs defined in this file
    --count      Show count per prefix instead of listing IDs
    --unused     Show IDs that are defined but never referenced
  ```
  - Reads from the CatalogIntegrity REGISTRY tier
  - `--count` mode: summarizes `item_: 247, quest_: 89, npc_: 45, ...` (illustrative example counts only — not verified against the live catalog; the real per-prefix counts will differ and should be generated by the tool itself, not hardcoded from this plan)
  - `--unused` mode: cross-references definitions against all TIER-1/TIER-2 references

- **`--schema-check`** verb:
  ```
  Usage: --schema-check [--missing-only] [--format json]

  Reports which JSON files have schema_version and which do not.
  Per AGENTS.md: only 35 of ~280 files have schema_version — this tracks progress.
  ```

- **`--data-stats`** verb:
  ```
  Usage: --data-stats [--format json]

  Summary statistics for the data authority: file count, total IDs, prefix distribution,
  files with/without schema_version, naming convention compliance (snake_case vs camelCase).
  ```

**Verification:**
```bash
dotnet build Ashfall.csproj
godot --headless --path . -- --validate-file items.json
godot --headless --path . -- --list-ids item_ --count
godot --headless --path . -- --list-ids --unused
godot --headless --path . -- --schema-check --missing-only
godot --headless --path . -- --data-stats --format json
```

**Done when:**
- `--validate-file` correctly filters full-catalog validation results to a single file's violations, and its help text/docs do not claim it skips loading the rest of the catalog (corrected — see Architecture note above)
- `--list-ids` shows all IDs with correct prefix filtering
- `--list-ids --unused` identifies orphaned IDs
- `--schema-check` accurately reports schema_version coverage
- `--data-stats` gives a useful project health summary
- All verbs exit with appropriate codes (0=ok, 1=errors, 2=warnings)

**Risk & Rollback:** Low risk — all four verbs are read-only wrappers around `CatalogIntegrityValidator` and the data authority; none of them write to `Assets/StreamingAssets/Data/` (note the `--fix` flag was cut for exactly this reason — see Architecture note above). Rollback: delete the new verb registrations; no data files or validator code are modified by this step.

---

### Step 6: Add Development Verbs (Day Advance + Scenario Runner)

**Goal:** Enable rapid testing of game progression and specific scenarios from the command line.

**Corrected — `--save-slot`/`--replay-save <file>` inherit Step 3's false save-slot premise, see Review Notes.** There is no single "the save" to load — a fresh game touches whichever subset of the 28 `*SaveStore` classes the run wires up (see `GameBootstrap`'s per-domain `SetupXxx`/`SaveXxx` triads, AGENTS.md H7). "Load from existing save" must mean "load whichever named stores currently exist under `user://`," using the same store-name vocabulary introduced in Step 3, not a single opaque save-slot argument.

**Implementation:**
- **`--advance-days`** verb:
  ```
  Usage: --advance-days <n> [--seed <s>] [--output <path>] [--systems <list>] [--load-existing]

  Creates a fresh game and advances n days headlessly. With --load-existing,
  loads whichever *SaveStore files currently exist under user:// (per Step 3's
  store registry) instead of starting fresh, rather than a single "--save-slot".
  Reports final state summary or full state dump.

  Arguments:
    <n>              Number of days to advance (1-3650)
    --seed, -s       RNG seed (default: 42)
    --load-existing  Load existing user:// save stores instead of a fresh game
                      (corrected from "--save-slot" — no slot model exists, see Step 3)
    --output, -o     Write final state to file (JSON)
    --systems        Comma-separated systems to tick (default: all)
    --quiet          Only print final summary, not per-day events
    --events         Print events raised during simulation
  ```
  - Useful for testing late-game systems (radiation accumulation over 100+ days)
  - With `--events`: prints timeline of all events raised (for debugging event chains)
  - With `--systems WeatherSystem,RadiationSystem`: isolate specific system behavior

- **`--scenario`** verb:
  ```
  Usage: --scenario <name> [--seed <s>] [--output <path>]

  Loads a predefined test scenario and runs it to completion.
  Scenarios are defined in Assets/StreamingAssets/Data/scenarios/ as JSON.

  Arguments:
    <name>         Scenario name (e.g., "radiation_crisis", "trade_embargo")
    --list         List all available scenarios
    --seed, -s     Override scenario seed
    --output, -o   Write result state to file
  ```
  - **Corrected — `Assets/StreamingAssets/Data/scenarios/` does not exist yet.** Verified: no `scenarios/` directory currently exists under `Assets/StreamingAssets/Data/`. This step must create the directory and its JSON schema as new work, not "load from" an existing location as the wording implies. Add `schema_version` from the start per AGENTS.md's data-authority guidance (only 35/~280 files currently have it — don't add file #281 without it).
  - Scenario format:
    ```json
    {
      "schema_version": 1,
      "name": "radiation_crisis",
      "description": "Heavy fallout day 1, test survivor response",
      "seed": 42,
      "initial_state": { "weather": "fallout_storm", "day": 1 },
      "actions": [
        { "day": 1, "action": "assign_task", "survivor": "npc_lead_01", "task": "seal_bunker" },
        { "day": 3, "action": "advance" }
      ],
      "assertions": [
        { "system": "Radiation", "field": "totalDose", "op": ">", "value": 0 }
      ]
    }
    ```
  - Assertions turn the scenario into an automated acceptance test
  - Exit code 0: all assertions pass, 1: assertion failure (with details)
  - `"action": "assign_task"` with a literal `npc_lead_01` id must resolve through the existing ID-registry rules in AGENTS.md (`npc_` prefix, validated by `CatalogIntegrityValidator`) — the scenario runner should fail loudly (not silently no-op) if a referenced id does not exist in the current data authority, consistent with the project's TIER-1/TIER-2 reference-resolution rules

- **`--replay-save`** verb:
  ```
  Usage: --replay-save <system> <file> [--days <n>]

  Loads a JSON file into the named system's save store (per Step 3's
  --import-save mechanism) and replays n additional days. Useful for
  reproducing bugs from a player-submitted save file for one specific
  system (corrected from an unscoped "<file>" implying one global save).
  ```

**Verification:**
```bash
dotnet build Ashfall.csproj
godot --headless --path . -- --advance-days 30 --seed 42 --quiet
godot --headless --path . -- --advance-days 5 --events --systems WeatherSystem
godot --headless --path . -- --scenario --list
godot --headless --path . -- --scenario radiation_crisis --seed 42
godot --headless --path . -- --replay-save holdfast /tmp/player_save.json --days 5
```

**Done when:**
- `--advance-days` correctly ticks all systems for N days with deterministic output
- Same seed + same days = identical final state (determinism verified)
- `--scenario` loads and executes scenario files with assertions
- Scenario assertion failures produce clear error messages with expected vs actual
- `--replay-save` loads an external save file into a named store and advances without crashing
- `--events` mode provides useful debugging output for event chain issues
- `Assets/StreamingAssets/Data/scenarios/` is created with `schema_version` on every file from day one

**Risk & Rollback:** Medium risk — `--advance-days`/`--scenario` construct a real in-memory game state and tick real systems; a bug here could hang (infinite loop in a system's tick) or crash headlessly in CI, which is worse than a normal failed test since it can stall a pipeline. Mitigation: enforce the documented `<n>` range (1-3650) and add a hard wall-clock timeout around the simulation loop so a runaway tick can't hang CI indefinitely. `--replay-save`'s dependence on Step 3's per-store import mechanism means it inherits Step 3's overwrite risk (`user://` store gets overwritten) — same mitigation (no silent `--force`, default to refusing overwrite). Rollback: these are new, additive verbs with no changes to existing gameplay systems' tick logic; deleting the verb registrations fully reverts this step.

---

</content>
</file>
Note: This read was limited to 85 lines and may not include the end of the file. To continue, call read_file with {"path":"/home/robertsrff/Desktop/luna)plans/ASHFALL_NEXT_STEPS_QUALITY_ROADMAP_BATCH_106.md","offset":545}.

### Step 7: Write CLI Tests (Parser + Verb Integration)

**Goal:** Comprehensive test coverage ensuring CLI reliability, correct argument parsing, and verb behavior.

**Corrected — every test file below is misplaced, see Review Notes.** This step's own predecessor (Step 1's Verification block) already established that `CliRouter`/`CliVerb`/`CliArg`/`CliContext` are declared inside `src/CLI/`, compiled as part of `Ashfall.csproj` (Godot host, `Godot.NET.Sdk/4.7.1`, `net8.0`), and that `Ashfall.Core.Tests.csproj` (`net9.0`) has no project reference to `Ashfall.csproj` — confirmed again directly by reading `Ashfall.Core.Tests.csproj`, whose only `<ProjectReference>` is `Ashfall.Core.csproj`. Every `[Fact]` test class drafted below (`CliParserTests.cs`, `CliRouterTests.cs`, and especially `CliVerbIntegrationTests.cs`, which ticks real systems via `--advance-days`/`--profile-tick`) references types that either don't exist in `Ashfall.Core.Tests`'s dependency graph at all (`CliRouter` et al.) or require a live Godot scene tree/bootstrap (`--advance-days`, `--profile-tick`, `--scenario`). Placed as drafted, none of these three files will compile in `Ashfall.Core.Tests.csproj`. This is the same class of defect Batch 105's independent review caught in its own Step 6 (`ResponsiveLayoutTests.cs` needing real Godot `Control` types) — that lesson was not carried over into this document.

**Implementation (revised placement):**
- Create `src/CLI/Tests/CliParserTests.cs` **inside the Godot host** (`Ashfall.csproj`, not `Ashfall.Core.Tests`), exercised via a new `--cli-parser-selftest` headless verb (consistent with Step 1's corrected verification approach and the project's existing `--*-selftest` pattern):
  ```csharp
  // Argument parsing checks, run as assertions inside the selftest verb body,
  // not xUnit [Fact]s (xUnit here would still need a net8.0-compatible test
  // host wired to Godot.NET.Sdk, which is a bigger structural change this
  // plan does not budget for; the project's existing pattern for Godot-host
  // logic is a --*-selftest verb that returns a nonzero exit code on failure)
  ParsesKeyValue(); ParsesKeyEqualsValue(); ParsesShortName(); ParsesBooleanFlag();
  CollectsPositionalArgs(); ReportsErrorForMissingRequired();
  HandlesUnknownArgsGracefully(); HelpFlagSkipsExecution(); FormatJsonSetsOutputFormat();
  ```
- Create `src/CLI/Tests/CliRouterTests.cs`, same placement and verb pattern (`--cli-router-selftest`, or fold into `--cli-parser-selftest` if the split isn't worth a second verb):
  ```csharp
  DispatchesToRegisteredVerb(); ReturnsErrorForUnknownVerb(); HelpListsAllVerbs();
  VerbHelpShowsArguments(); VerbHelpShowsExamples();
  ```
- Create `src/CLI/Tests/CliVerbIntegrationTests.cs`, same placement — these inherently need the Godot host's real system/bootstrap access (`--advance-days` ticks real `Ashfall.Core` systems via the Godot save stores), so there was never a version of this file that could live in `Ashfall.Core.Tests` even before the Step 1 placement correction:
  ```csharp
  ExportSave_ProducesValidJson(); ImportSave_RejectsInvalidJson();
  ValidateFile_FindsKnownErrors(); ListIds_FiltersByPrefix();
  AdvanceDays_IsDeterministic(); ProfileTick_ReportsAllSystems();
  Scenario_FailsOnAssertionViolation(); CheckInvariants_FindsKnownViolations();
  ```
  exercised via a `--cli-verb-integration-selftest` verb, or split per-domain if the combined runtime gets long
- The only genuinely `Ashfall.Core.Tests`-appropriate CLI-adjacent tests are ones with zero Godot dependency — none of the drafted files qualify, since even the pure parser logic lives inside a `src/CLI/` class compiled only into the Godot host per Step 1. If a future revision wants `dotnet test`-visible parser coverage, the parser classes would need to be extracted into a Godot-independent assembly `Ashfall.Core.Tests` can reference — out of scope here (same as Batch 105's equivalent finding for `ResponsiveLayoutTests.cs`).

- Create Godot-side smoke tests:
  ```bash
  # Verify all verbs are registered and --help works
  godot --headless --path . -- --help | grep "export-save"
  godot --headless --path . -- --help | grep "advance-days"
  # Verify exit codes
  godot --headless --path . -- --validate-file nonexistent.json; echo $?  # should be 1
  ```

- Add a CLI test to CI pipeline that runs all verbs with `--help` (ensures none crash on help)

**Verification:**
```bash
dotnet build Ashfall.csproj   # Godot-host CLI test files compile (not dotnet test — see placement correction above)
godot --headless --path . -- --cli-parser-selftest         # New: parser/router assertions
godot --headless --path . -- --cli-verb-integration-selftest  # New: per-verb integration assertions
godot --headless --path . -- --help   # Smoke: all verbs listed
```

**Done when:**
- Parser tests cover all argument forms (key-value, flag, positional, short, equals), run via `--cli-parser-selftest` (not `dotnet test`, per the placement correction above)
- Router tests verify dispatch, error handling, and help generation
- Integration tests verify each verb produces expected output
- Edge cases tested: empty args, duplicate flags, extremely long values, special characters
- CI gate: `--help` must list all registered verbs (catches registration omissions) — concretely: a CI step asserts every verb name registered in the router also appears in `--help`'s output, failing loudly if a verb is registered but undocumented
- All new selftest verbs return exit code 0 on pass, non-zero on any assertion failure, consistent with every other `--*-selftest` verb in this codebase

**Risk & Rollback:** Low risk — this step is purely additive test code with no production behavior change; its only defect (misplacement into a project that can't compile it) is corrected above. Rollback: delete the `src/CLI/Tests/` files and the new selftest verb registrations; nothing else depends on them.

---

## Summary Table

| Step | Title | Risk | Depends On | Output |
|------|-------|------|-----------|--------|
| 1 | Design CLI Framework | Low | None | `CliRouter.cs`, `CliVerb.cs`, `CliArg.cs`, `CliContext.cs` |
| 2 | Implement CliRouter in Host | **Medium-High (corrected from "Low" — see Review Notes)** | Step 1 | `Main.CLI.cs`, `--help`, `--version` |
| 3 | Management Verbs (Save) | **Medium (corrected from "Low" — see Review Notes: no unified save-slot model exists)** | Step 2 | `--export-save`, `--import-save`, `--list-saves` |
| 4 | Diagnostic Verbs | Low | Step 2 | `--profile-tick`, `--check-invariants` (now covers all 6 invariants, corrected — see Review Notes), `--system-graph` |
| 5 | Data Verbs | Low | Step 2 | `--validate-file` (full-catalog scan, filtered display — corrected), `--list-ids`, `--schema-check`, `--data-stats` |
| 6 | Development Verbs | Medium (corrected from "Low" — see Review Notes: real system ticks, no unified save concept) | Step 2 | `--advance-days`, `--scenario` (new `scenarios/` data directory), `--replay-save` (per-store, not per-file) |
| 7 | CLI Tests | Low | Steps 3-6 | Godot-host `src/CLI/Tests/*.cs` + `--cli-parser-selftest`/`--cli-verb-integration-selftest` (relocated per Step 7 correction — not `Ashfall.Core.Tests`) |

---

## Exit Criteria

- [ ] `--help` lists all verbs grouped by category with descriptions
- [ ] All **75** existing self-test verbs (corrected from "50+" — see Review Notes; 107 `--flag` strings including aliases) work identically (zero regression)
- [ ] All new verbs accept `--format json` for CI consumption
- [ ] Argument parser handles all documented forms correctly
- [ ] `--export-save` → `--import-save` round-trip produces identical state, for named save stores (corrected from an assumed numbered slot — see Review Notes)
- [ ] `--advance-days` is deterministic (same seed = same output)
- [ ] `--check-invariants` catches known violations listed in AGENTS.md, across all **6** invariants including Invariant 3 (corrected — see Review Notes)
- [ ] `dotnet build Ashfall.csproj` — 0 errors, 0 warnings
- [ ] `godot --headless --path . -- --cli-parser-selftest` and `--cli-verb-integration-selftest` pass (corrected from "`dotnet test` — all CLI tests pass" — the CLI framework and its tests live in the Godot host, not `Ashfall.Core.Tests`; see Step 1 and Step 7 corrections)
- [ ] New contributors can discover all CLI capabilities via `--help` alone

---

## Future Work (Not in This Batch)

- Shell completions (bash/zsh/fish) auto-generated from verb registry
- `--watch` mode for verbs (re-run on file change)
- `--interactive` mode (REPL-style system exploration)
- Remote management verbs (connect to running game instance)
- Plugin system: drop a DLL in `plugins/` to register custom verbs
- Benchmarking suite with historical comparison (track performance over time)
- Scenario library: community-contributed test scenarios


---

## Review Notes (Corrected)

Adversarial review performed against the live codebase at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`. This document arrived already containing extensive inline "corrected" annotations and six separate "See Review Notes" cross-references — but no `## Review Notes` section actually existed anywhere in the file, meaning every one of those cross-references was pointing at nothing. That broken-reference pattern is itself the first defect fixed by this pass. Every number and file reference below was independently re-derived with `grep`/`find`/`wc`/`sed`, not taken from the document's own prior in-text claims.

### Factual errors found and fixed

1. **`HostCliAction` enum count was wrong: "71" claimed, 75 actual.** The document's own in-text "corrections" (Problem Statement, Architecture Decision, Steps 2's implementation block and Risk & Rollback) all state 71 enum values, attributing that number to "counting enum members in `src/Host/HostCli.cs`." Recounting directly: the `public enum HostCliAction { ... }` block in `src/Host/HostCli.cs` contains **75** members (74 comma-terminated entries plus the final entry `UiSnapshotSelfTest`). Cross-checked independently against `src/Main.cs`: `grep -c 'case HostCliAction\.' src/Main.cs` → **74** — one less than the enum's 75, because `HostCliAction.Interactive` (the no-args default/menu state) has no case arm and is never referenced anywhere in `Main.cs`; it falls through to normal game boot rather than being dispatched. Fixed every occurrence of "71" (6 sites) to "75" with a note on why 74 switch-case arms is the correct, consistent number for `Main.cs` specifically.
2. **Two leftover "50+" references were never updated when the rest of the document was corrected to "71" (now 75).** The Problem Statement's own text explicitly says "corrected from '50+'," but the Exit Criteria and the header Priority/Risk table's precursor text both still said "50+ existing self-test verbs" — an internal inconsistency indicating a partial, abandoned prior edit pass (the same pattern independently found and fixed in Batch 105's Steps 6/7 missing-section issue). Fixed both remaining "50+" sites to 75.
3. **`PrintHelp()` verb-count claim understated: "~35" claimed, 48 actual.** Extracting every distinct `--flag-name` token printed inside `HostCli.PrintHelp()`'s body and de-duplicating gives exactly **48** distinct flags documented across **47** `GD.Print` calls (via `awk` isolating the method body + `grep -oE '\-\-[a-zA-Z0-9-]+' | sort -u | wc -l`), not "~35." Against the true dispatch total of 107 distinct `--flag` strings, that means **59 flags (55%) are undocumented**, not "roughly half" as a round-number approximation — close, but the plan's own logic ("verified: ... but `PrintHelp()` prints only ~35 lines") both undercounted the documented set and used "lines" as a proxy for "flags" when several `GD.Print` calls document 2-3 aliases in one multi-line string. Fixed the Problem Statement's item 1 with the exact figures and named several undocumented verb families (`--warlord-*`, `--radio-selftest`, `--expedition-panel-uitest`, `--ui-snapshot-uitest`) as concrete examples.
4. **`--check-invariants` (Step 4) silently omitted Invariant 3.** AGENTS.md defines exactly 6 invariants (`### Invariant 1` through `### Invariant 6`, confirmed by grepping the file directly). The verb as originally specified checked Invariants 1, 2, 4, 5, 6 — dropping Invariant 3 (Cross-host save compatibility) entirely. This is a substantive gap, not a cosmetic one: AGENTS.md's own Critical Known Issue **C1** (`JsonUtility` in the Unity `SaveSystem` blocks cross-host saves) is a live, currently-true Invariant 3 violation, so a "check all invariants" verb that never checks the one invariant with a known active violation would give false confidence to anyone running it. Fixed: added Invariant 3 to the check list, specified what it scans for (`JsonUtility` call sites in `Assets/_Game/Core/SaveSystem.*.cs` and the 21 remaining `*CatalogLoader.cs` files per AGENTS.md's C6), and added the same baseline-grandfathering pattern already used elsewhere in this plan (and in Batch 105) so the known, pre-existing violation doesn't immediately fail CI.
5. **Step 3 (`--export-save`/`--import-save`/`--list-saves`) was built on a save-slot model that does not exist in this codebase.** Verified: `grep -rl 'class \w*SaveStore' src/Host/ | wc -l` → **28** distinct `*SaveStore` classes (`HoldfastSaveStore`, `DutyRosterSaveStore`, `SurvivorsSaveStore`, `InventorySaveStore`, `EconomySaveStore`, `MedicalSaveStore`, `NarrativeSaveStore`, `WorldSaveStore`, `RadioSaveStore`, `CombatSaveStore`, and 18 more), each with an independent `SavePath` under `user://` and its own `CaptureState`/`RestoreState` DTO — consistent with AGENTS.md's SAVE/LOAD section, which describes per-system checksummed envelopes, not one indexed save file. There is no numbered "slot 0 / slot 1 / auto" concept anywhere in the codebase to export from or list. This was not a naming nitpick — a literal implementation of the original Step 3 spec would have required inventing a wholly new aggregation subsystem over the 28 stores just to produce a "slot list," which is a materially different (and unbudgeted) scope than "add a thin CLI wrapper around something that exists." Fixed: rewrote Step 3's three verbs around named stores (`--export-save <system>`, `--import-save <system> <file>`, `--list-saves` as a store registry listing), added a Risk & Rollback section (previously missing), and dropped the `--systems Needs,Radiation` cross-system filter idea from `--export-save` since `Needs`/`Radiation` are Core systems nested inside a single store's payload, not separate stores — filtering at that granularity needs per-DTO knowledge this step doesn't budget for.
6. **`Assets/StreamingAssets/Data/scenarios/` (Step 6's `--scenario` verb) does not exist.** Verified: `find Assets/StreamingAssets/Data -iname 'scenario*'` returns nothing. The original phrasing ("Scenarios are defined in `Assets/StreamingAssets/Data/scenarios/` as JSON") implies an existing location being read from; it must be created from scratch, including its JSON schema, as new work — the plan did not account for designing this format as a design task in its own right (it only showed an example payload). Fixed: called this out explicitly and added a requirement to include `schema_version` on every scenario file from the start, per AGENTS.md's data-authority guidance (only 35/~280 files currently have it).
7. **Step 6's `--advance-days --save-slot` and `--replay-save <file>` inherited the same false save-slot premise as Step 3.** Fixed to use the corrected named-store vocabulary from Step 3 (`--load-existing` loading whichever stores currently exist; `--replay-save <system> <file>`), and added a previously-missing Risk & Rollback section covering the hang/crash risk of ticking real systems headlessly in CI (mitigation: enforce the documented day-count range and add a wall-clock timeout).
8. **`--validate-file` (Step 5) assumed single-file validation is possible without loading the full catalog; it isn't.** `Assets/Ashfall.Core/CatalogIntegrityValidator.cs`'s only public entry point is `public static CatalogIntegrityReport Validate(string dataDirectory, IFileIO files)` — it takes a directory, because TIER-1/TIER-2 reference checks are inherently cross-file (an id defined in `items.json` can be referenced from `quests.json`). "Runs `CatalogIntegrityValidator` on a single file... without running the full integrity suite" as originally worded is not achievable as a true subset operation without either silently losing cross-file reference checking (a false-negative risk: a file could report "valid" while referencing a nonexistent id elsewhere) or still loading the full directory internally. Fixed: `--validate-file` now explicitly runs the full-directory validation and filters the *displayed report* to the target file, with the caveat stated up front that this is "faster to read, not faster to run." Also dropped the `--fix` auto-fix flag from this step's scope — auto-mutating the data authority from a CLI flag is a materially riskier, undesigned feature (no stated dry-run/backup semantics) that doesn't belong bundled into a read-only validation verb.
9. **Step 7's entire test-file layout would fail to compile as drafted, and this document's own Step 1 already contained the evidence.** Step 1's Verification block established that `CliRouter`/`CliVerb`/`CliArg`/`CliContext` are declared inside `src/CLI/`, part of `Ashfall.csproj` (Godot host, `net8.0`), and that `Ashfall.Core.Tests.csproj` (`net9.0`) has no reference to it — re-confirmed directly by reading `Ashfall.Core.Tests.csproj`'s `<ProjectReference>` list, which contains only `Ashfall.Core.csproj`. Step 7 nonetheless drafted `Ashfall.Core.Tests/CLI/CliParserTests.cs`, `CliRouterTests.cs`, and `CliVerbIntegrationTests.cs` (the last of which also needs a live Godot bootstrap to tick real systems for `AdvanceDays_IsDeterministic`/`ProfileTick_ReportsAllSystems`). None of the three would compile in that project. This is the exact same class of defect Batch 105's independent review caught in its own Step 6 (`ResponsiveLayoutTests.cs` needing real Godot `Control` types) — that lesson from the sibling document was not applied here. Fixed: relocated all three files to `src/CLI/Tests/` inside the Godot host, replaced `[Fact]`-based xUnit tests with assertion bodies inside new `--cli-parser-selftest`/`--cli-verb-integration-selftest` headless verbs (consistent with the project's existing `--*-selftest` pattern), and updated every "Verification"/"Done when" block downstream (Exit Criteria, Summary Table) that referenced `dotnet test` for CLI coverage.
10. **Scope estimate ("~1.5 weeks") did not account for any of the above.** The corrected Step 2 (two dispatch layers across 3 files, not a single `Main.cs` if/else chain), Step 3's redesign around 28 independent stores, Step 6's new-from-scratch `scenarios/` schema, and Step 7's Godot-host test relocation all add real, unbudgeted time. Revised to ~2.5-3 weeks in the header table, and the blanket per-step "Low" risk column in the original header/Summary Table was replaced with per-step risk levels (Step 2 Medium-High, Step 3 Medium, Step 6 Medium, Steps 1/4/5/7 Low) instead of a single "Low for new verbs" blanket statement that undersold Steps 3 and 6's real I/O/tick risk.

### Attacks that did not find a defect (confirmed sound)

- The line-count citations for `HostCli.cs` (326), `HostCli.SelfTests.cs` (681), `HostCli.PanelTests.cs` (2,867), their sum (3,548), and `Main.cs` (7,014) are all exactly correct, re-verified with `wc -l` directly against the live files.
- The claim that `Main.cs:279` contains `switch (HostCli.Parse(OS.GetCmdlineUserArgs()))` is exactly correct, confirmed by reading that line directly.
- The 107-distinct-`--flag`-string count (including aliases) is exactly correct: `grep -oE 'Has\(args, "--[a-zA-Z0-9-]+"\)' src/Host/HostCli.cs | sort -u | wc -l` → 107, with zero duplicate `Has(args, ...)` calls (107 total occurrences, 107 unique).
- The claim that `Ashfall.Core.Tests.csproj` targets `net9.0` with no Godot SDK reference, versus `Ashfall.csproj` targeting `net8.0` on `Godot.NET.Sdk/4.7.1`, is exactly correct — confirmed by reading both `.csproj` files directly.
- `Assets/Ashfall.Core/Ports.cs` does define `public interface IJsonSerializer`, and `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` and `Assets/Ashfall.Core/SaveChecksum.cs` both exist as claimed elsewhere in this plan's implementation notes.
- The "two-stage design (string→enum, then enum→handler)" characterization of the current `HostCli.Parse()` + `Main.cs switch` architecture is an accurate description of the real code, not a strawman simplification.
- The "Not chosen" alternatives (System.CommandLine NuGet dependency, reflection-based verb discovery, separate console app) are reasonable rejections with real stated justifications, not strawmen.
- Step 1's placement of `CliRouter`/`CliVerb`/`CliArg`/`CliContext` in `src/CLI/` as new, zero-call-site classes is genuinely additive and low-risk exactly as described; no defect found there beyond the downstream Step 7 test-placement issue already fixed above.
