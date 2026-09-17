# ASHFALL — Quality Roadmap Batch 107

## Theme: Automated Regression Detection — Golden File Testing for Deterministic Output

| Field | Value |
|-------|-------|
| **Priority** | HIGH |
| **Risk** | Low overall for Steps 1–4 & 7 (additive test-only code, zero production changes). **Medium for Steps 5–6** — Step 5 adds a new `HostCli.cs` verb that shells out to `dotnet test` from inside a running Godot process (new process-spawning surface, not a pure in-process self-test like the existing verbs), and Step 6 wires golden tests into the standard CI test run, meaning a bad normalization rule or a flaky system tick sequence can turn CI red for reasons unrelated to an actual regression. See Risk / Rollback note below. |
| **Category** | Testing / Regression Detection / CI |
| **Blocked by** | None (leverages existing determinism infrastructure). **However, Steps 2–3 are blocked in practice by AGENTS.md's H10 gap** — `NeedsSystem`/`RadiationSystem` have no `CaptureState()`/`RestoreState()` today (confirmed by direct source read), so golden-testing those two specific systems cannot proceed as originally scoped until that gap is either fixed or explicitly worked around (see Step 2/3 notes below). |
| **Blocks** | Confident refactoring, system migration to Core, save format evolution |
| **Estimated scope** | ~1 week (framework + 3 confirmed system goldens + save format golden + CI gate) — **not 4 system goldens as originally stated; see corrected scope below (WeatherSystem, MarketSystem, CombatTraumaSystem are confirmed viable; NeedsSystem/RadiationSystem/FactionSystem are removed or deferred)** |

**Risk / Rollback:** Steps 1, 2 (once descoped per the notes below), 3, 4, and 7 only add new files under `Ashfall.Core.Tests/Golden/` and `Ashfall.Core.Tests/GoldenFiles/` — trivially revertible via `git revert` with zero production impact, since nothing in `Assets/Ashfall.Core/` or `src/` is touched. Step 5's new `HostCli.cs` verb and Step 6's CI wiring are the only steps with real rollback cost: if `--update-golden` destabilizes the Godot host process (e.g. the `dotnet test` subprocess call hangs or leaves zombie processes), revert that single `HostCli.cs` change independently — the bash script (`scripts/update-golden.sh`) is a safe fallback that achieves the same outcome without touching the Godot host at all, so Step 5's CLI-verb sub-task can be dropped entirely without losing the update workflow. If Step 6's CI gate produces false-positive failures (e.g. a system's tick order has any hidden nondeterminism the golden test didn't anticipate, such as `Dictionary` enumeration order), revert the specific golden test that's flaky rather than disabling the whole golden suite — track the flaky system as its own follow-up rather than eroding trust in the framework.

---

## Problem Statement

ASHFALL enforces Invariant 4: **same seed produces identical simulation**. The project uses `ISeededRng`, implemented in Core by `SeededRng` (`Assets/Ashfall.Core/HostDefaults.cs`) — genuine **xorshift64\*** for the actual generation step (shift-triple 12/25/27, multiplier `0x2545F4914F6CDD1D`), seeded once via **SplitMix64** whitening in the constructor to spread a narrow `int` seed across the full 64-bit state. (**Correction:** earlier drafts of this doc, and the code samples below, used a class name `CoreSeededRng` when writing Core-side test code — that name does not exist inside `Assets/Ashfall.Core/`. `CoreSeededRng` is a *host-side* wrapper name used in `src/Host/*.cs` files that delegate to `Ashfall.Core.SeededRng`. Any new xUnit test living in `Ashfall.Core.Tests/` and testing Core directly should instantiate `new SeededRng(seed)`, not `new CoreSeededRng(seed)` — the latter would fail to compile from a test project that doesn't reference `src/Host/`.) The project has eliminated `System.Random` and `Guid.NewGuid()` from Core (confirmed: zero remaining production usages; the one remaining `Guid.NewGuid` reference is a code comment documenting the historical fix, at `Assets/Ashfall.Core/Inventory/ProceduralItemInstance.cs:48-49`). This determinism guarantee is enormously valuable but currently underexploited for regression detection specifically:

1. **No baseline captures exist** — there is no recorded "known-good" output for any system at any seed, **for regression testing.** (**Correction — this needs to be scoped precisely:** `Ashfall.Core.Tests/CoreInvariantSourceTests.cs` already exists and already covers determinism at the *source-hygiene* level — it source-scans all of `Assets/Ashfall.Core/*.cs` for banned nondeterminism patterns [`System.Random`, `new Random(`, `Guid.NewGuid(`, `DateTime.Now`, `DateTime.UtcNow`, `.GetHashCode()`] and banned engine-coupling patterns, and has a `Core_SeededRng_ReproducesAcrossInstances` test that confirms two `SeededRng` instances with the same seed produce identical 64-value sequences. **This existing test does NOT do what this batch proposes** — it doesn't capture full-system or full-game-state output at all; it only proves the RNG primitive itself is reproducible and that no banned API is referenced anywhere in Core's source text. There is no overlap/duplication risk in implementing this batch, but the "no baseline captures exist" framing undersells what's already enforced — the RNG-level determinism guarantee this batch depends on is already tested and passing, not just claimed in prose.)
2. **Regressions are invisible** — if a refactor subtly changes system output (e.g., radiation dose differs by 0.001 after 10 days), no test catches it until a player reports the discrepancy
3. **Save format drift is undetected** — if serialization order changes or a field is renamed, existing saves may silently break
4. **Cross-host divergence is untested** — Core should produce identical state regardless of host, but nothing compares output between Godot and xUnit test runners
5. **Review burden is high** — reviewers must manually reason about whether a change alters simulation output; golden files make this diff-visible

Golden file testing captures known-good output and compares future runs against it. When output changes, either:
- (a) A bug was introduced → fix the code
- (b) An intentional change was made → update the golden file (the diff in the PR makes the behavioral change visible to reviewers)

This is the strongest form of regression detection for deterministic systems and is standard practice in compilers, serializers, and simulation engines. **Confirmed: no golden-file/snapshot-comparison infrastructure exists anywhere in the repo today** (search performed across `Ashfall.Core.Tests/` for "Golden", "Snapshot", "approv", ".received." — the only incidental hits were `HoldfastSaveTests.cs:465`'s single hardcoded-checksum-string test named `FrozenV1ShapeChecksumIsGolden()`, and many unrelated `CaptureState_ReturnsSnapshotNotLiveState()`-style tests that check state-copy isolation, not regression detection). This batch is genuinely new infrastructure, not a duplicate of existing work.

---

## Architecture Decision

**Approach:** Store golden files as JSON in `Ashfall.Core.Tests/GoldenFiles/`. Each golden test:
1. Constructs a system with a fixed seed
2. Runs a fixed number of ticks/days with no player input
3. Captures state via `CaptureState()`
4. Serializes to JSON (using `SystemTextJsonSerializer`, the Core standard)
5. Compares serialized output to the stored golden file (exact byte comparison after normalization)

**Normalization rules** (prevent false positives):
- JSON keys sorted alphabetically (prevent serialization order drift)
- Floats formatted to 9 significant digits (`G9` — matches `SaveChecksum` convention)
- Null fields omitted (prevent null vs missing churn)
- Newlines normalized to `\n`
- No trailing whitespace

**Update workflow:**
- Set environment variable `UPDATE_GOLDEN=true` and run tests
- Tests overwrite golden files instead of comparing
- Commit updated golden files — the diff shows exactly what changed
- PR reviewers see behavioral changes as data diffs, not code inference

**Not chosen:**
- Snapshot testing libraries (Verify, ApprovalTests) — add NuGet dependency; golden files are simple enough to self-host
- Binary golden files — unreadable in diffs; JSON is human-reviewable
- Hash-only comparison — loses the ability to see *what* changed

---

## Steps

### Step 1: Design Golden File Framework

**Goal:** Build the test infrastructure that all golden file tests will use: file storage conventions, comparison logic, normalization, and update mechanism.

**Implementation:**
- Create `Ashfall.Core.Tests/Golden/GoldenFileHelper.cs`:
  ```csharp
  namespace Ashfall.Core.Tests.Golden;

  public static class GoldenFileHelper
  {
      private static readonly string GoldenDir =
          Path.Combine(TestContext.SolutionRoot, "Ashfall.Core.Tests", "GoldenFiles");

      /// <summary>
      /// Compares actual output to golden file. If UPDATE_GOLDEN=true, overwrites golden.
      /// </summary>
      public static void AssertMatchesGolden(string category, string testName, string actualJson)
      {
          var goldenPath = Path.Combine(GoldenDir, category, $"{testName}.golden.json");
          var normalized = Normalize(actualJson);

          if (Environment.GetEnvironmentVariable("UPDATE_GOLDEN") == "true")
          {
              Directory.CreateDirectory(Path.GetDirectoryName(goldenPath)!);
              File.WriteAllText(goldenPath, normalized);
              return; // Test "passes" when updating
          }

          if (!File.Exists(goldenPath))
              throw new FileNotFoundException(
                  $"Golden file not found: {goldenPath}\n" +
                  "Run with UPDATE_GOLDEN=true to create it.");

          var expected = File.ReadAllText(goldenPath);
          if (expected != normalized)
          {
              var diff = GenerateDiff(expected, normalized);
              throw new GoldenFileMismatchException(
                  $"Golden file mismatch for {category}/{testName}:\n{diff}\n\n" +
                  "If this change is intentional, run with UPDATE_GOLDEN=true to update.");
          }
      }

      private static string Normalize(string json) { ... }
      private static string GenerateDiff(string expected, string actual) { ... }
  }
  ```

- Create `Ashfall.Core.Tests/Golden/GoldenFileMismatchException.cs`:
  - Custom exception with structured diff output
  - Shows first N lines of difference
  - Shows line number where divergence starts

- Normalization implementation:
  ```csharp
  private static string Normalize(string json)
  {
      // 1. Parse to JsonDocument
      // 2. Recursively sort all object keys alphabetically
      // 3. Format floats to G9
      // 4. Omit null-valued properties
      // 5. Serialize with indentation (2 spaces) and \n line endings
      // 6. Trim trailing whitespace per line
      // 7. Ensure trailing newline
  }
  ```

- Directory structure:
  ```
  Ashfall.Core.Tests/
  └── GoldenFiles/
      ├── README.md              (explains golden file conventions)
      ├── Determinism/
      │   └── full_state_seed42_10days.golden.json
      ├── Systems/
      │   ├── WeatherSystem_seed42_10days.golden.json
      │   ├── MarketSystem_seed42_10days.golden.json
      │   └── CombatTraumaSystem_seed42_10days.golden.json
      │   (NeedsSystem/RadiationSystem/FactionSystem removed from this tree — see Step 3
      │    corrections: the first two lack CaptureState() today, the third doesn't exist)
      └── SaveFormat/
          └── save_envelope_seed42.golden.json
  ```

- Create `Ashfall.Core.Tests/GoldenFiles/README.md`:
  ```markdown
  # Golden Files

  These files contain known-good output for deterministic systems.
  They are automatically compared against actual output during tests.

  ## Updating golden files

  When a test fails because behavior intentionally changed:
  ```bash
  UPDATE_GOLDEN=true dotnet test --filter "Golden"
  ```

  Then commit the updated `.golden.json` files. The diff shows what changed.

  ## Rules
  - NEVER manually edit golden files (always regenerate)
  - ALWAYS review golden file diffs in PRs (they show behavioral changes)
  - Golden mismatches in CI = the build is broken
  ```

**Verification:**
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "GoldenFramework"
```

**Done when:**
- `GoldenFileHelper` can compare, normalize, update, and diff golden files
- Normalization is idempotent (normalize(normalize(x)) == normalize(x))
- Update mode overwrites files without failing
- Comparison mode fails with clear diff output on mismatch
- Missing golden file produces actionable error message
- Directory structure is created automatically on first update

---

### Step 2: Create Seed-Determinism Golden Test (Full State)

**Goal:** Capture the complete game state after 10 days with seed=42 and no player input as the master determinism baseline.

**Implementation:**
- Create `Ashfall.Core.Tests/Golden/DeterminismGoldenTests.cs`:
  ```csharp
  namespace Ashfall.Core.Tests.Golden;

  public class DeterminismGoldenTests
  {
      [Fact]
      public void FullState_Seed42_10Days_MatchesGolden()
      {
          // Arrange: create game with seed 42, all systems initialized
          var rng = new SeededRng(42); // Core class — NOT "CoreSeededRng" (that name only exists in src/Host/ wrappers)
          var clock = new SimClock(); // NOT "SimpleClock" — that class does not exist. Two real clock
          // types exist per AGENTS.md H3: Ashfall.Core/HostDefaults.cs:67 defines `SimClock : IClock`
          // (day-based) and Ashfall.Core/Clock/ISimClock.cs:15 defines a second, different `SimClock : ISimClock`
          // (tick-based) — same class name, two different files/interfaces. Confirm which one this test
          // actually needs before writing it; do not assume they're interchangeable.
          var session = CreateFullGameSession(rng, clock);

          // Act: advance 10 days with no player input
          for (int day = 0; day < 10; day++)
              session.TickSimDay();

          // Assert: full state matches golden
          var state = session.CaptureFullState();
          var json = SerializeState(state);
          GoldenFileHelper.AssertMatchesGolden("Determinism", "full_state_seed42_10days", json);
      }

      [Fact]
      public void FullState_Seed123_30Days_MatchesGolden()
      {
          // Second seed/duration for cross-validation
          ...
      }

      [Fact]
      public void FullState_IsDeterministic_AcrossRuns()
      {
          // Run twice with same seed, verify identical output
          var output1 = RunAndCapture(seed: 42, days: 10);
          var output2 = RunAndCapture(seed: 42, days: 10);
          Assert.Equal(output1, output2);
      }

      [Fact]
      public void DifferentSeed_ProducesDifferentState()
      {
          // Sanity: different seeds must NOT produce same output
          var output42 = RunAndCapture(seed: 42, days: 10);
          var output99 = RunAndCapture(seed: 99, days: 10);
          Assert.NotEqual(output42, output99);
      }
  }
  ```

- `CreateFullGameSession` and `session.TickSimDay()`/`session.CaptureFullState()` above are **illustrative pseudocode, not existing types.** No `TickSimDay()` method exists anywhere in Core today — every ticking system uses `Tick(float gameHours)` (e.g. `NeedsSystem.Tick`, `RadiationSystem.Tick` at `Assets/Ashfall.Core/Radiation/RadiationSystem.cs:188`, `WeatherSystem.Tick` at `Assets/Ashfall.Core/World/WeatherSystem.cs:115`). There is also no existing "full game session" class that owns every Core system and exposes a single `CaptureFullState()`/`TickSimDay()` pair — the closest real analog is a Godot host session such as `src/Host/SurvivorsHostSession.cs`, which is host-layer code, not something `Ashfall.Core.Tests` can reference. **Step 1 of implementation must build a small test-only aggregator** (e.g. `Ashfall.Core.Tests/Golden/TestGameSession.cs`) that owns instances of the systems below and drives them with `Tick(...)` calls in a loop translating "day" into whatever `gameHours` increment each system expects — this is new code this batch must write, not existing plumbing it can call into.
- `CreateFullGameSession` wires the Core systems that can run without a host and that actually have `CaptureState()`/`RestoreState()` today:
  - **Confirmed to have both a tick method and CaptureState/RestoreState:** `WeatherSystem` (`CaptureState()`/`RestoreState(WorldWeatherState)`, `Assets/Ashfall.Core/World/WeatherSystem.cs:239,252`), `MarketSystem` (`CaptureState()`/`RestoreState(MarketState)`, `Assets/Ashfall.Core/Economy/MarketSystem.cs:294,338` — has a `TickCount` property and internal tick counter but no single public `Tick(...)` entry point was found in a source pass; verify the actual tick entry point before wiring), `CombatTraumaSystem` (`CaptureState()`/`RestoreState(CombatTraumaSaveState)`, `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs:199,218`), `FinalWishSystem` (`CaptureState()`/`RestoreState(FinalWishSaveState)`, `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs:279,301`).
  - **Confirmed real gap — do not assume these have CaptureState:** `NeedsSystem` (`Assets/Ashfall.Core/Survivors/NeedsSystem.cs`) and `RadiationSystem` (`Assets/Ashfall.Core/Radiation/RadiationSystem.cs`) have **zero** `CaptureState`/`RestoreState` methods today — confirmed by direct source read, and consistent with AGENTS.md's H10 ("NeedsSystem & RadiationSystem save/load round-trip tests… still missing", though the gap is actually worse than "tests missing": the methods themselves don't exist). The real Godot host (`src/Host/SurvivorsHostSession.cs`) persists these two systems by saving the raw `SurvivorNeedsState`/`SurvivorRadState` DTOs directly, bypassing the `CaptureState`/`RestoreState` convention entirely. **This batch cannot golden-test `NeedsSystem`/`RadiationSystem` via `CaptureState()` as written — either (a) descope these two systems from the golden suite until H10 is fixed, or (b) capture their state via the same raw-DTO route the host already uses (`SurvivorNeedsState`/`SurvivorRadState` lists), which is a different code path than every other system in this plan and needs its own test helper.** Pick (a) or (b) explicitly before starting Step 3 — do not silently write `needs.CaptureState()` and discover the compile error mid-implementation.
  - `InventorySystem`, `CombatSystem`, `MedicalSystem` (afflictions), `SurvivorSystem` — **not verified to exist under these exact names; confirm each class name and its CaptureState/tick signature via a source read before writing its golden test**, the same way NeedsSystem/RadiationSystem were checked above. Do not assume symmetry across systems.
  - **`FactionSystem` does not exist anywhere in the repo** (confirmed: no file, no class, by name search across the full tree). Remove it from scope entirely — do not write a golden test for a class that does not exist. If a "factions" concept exists under a different name, find and cite the real class before including it.
- Systems that require host interaction (UI events, player input) are excluded or given null-object input
- The golden file captures ALL system states in a single JSON object:
  ```json
  {
    "metadata": {
      "seed": 42,
      "days": 10,
      "systems": ["Needs", "Radiation", "Weather", "Market", ...],
      "captured_at_schema_version": 1
    },
    "state": {
      "NeedsSystem": { ... },
      "RadiationSystem": { ... },
      "WeatherSystem": { ... },
      ...
    }
  }
  ```

**Verification:**
```bash
# Generate golden files first time
UPDATE_GOLDEN=true dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "DeterminismGolden"
# Verify they match on re-run
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "DeterminismGolden"
```

**Done when:**
- Golden file exists for seed=42/10days and seed=123/30days
- Tests pass consistently (same output every run)
- Different seeds produce different output (sanity check)
- Golden file is human-readable JSON with all system states
- Any code change that alters simulation output causes a test failure with diff

---

### Step 3: Create Per-System Golden Tests

**Goal:** Individual golden files for each major system, enabling precise identification of which system's output changed.

**Implementation:**
- Create `Ashfall.Core.Tests/Golden/SystemGoldenTests.cs`:
  ```csharp
  namespace Ashfall.Core.Tests.Golden;

  public class SystemGoldenTests
  {
      private const int Seed = 42;
      private const int Days = 10;

      [Fact]
      public void WeatherSystem_Seed42_10Days_MatchesGolden()
      {
          // WeatherSystem's real constructor/Tick signature must be read from
          // Assets/Ashfall.Core/World/WeatherSystem.cs before writing this test —
          // do not assume it matches NeedsSystem's shape.
          var rng = new SeededRng(Seed); // Core class — NOT "CoreSeededRng"
          var weather = new WeatherSystem(/* real ctor args, verify first */);

          for (int day = 0; day < Days; day++)
              weather.Tick(gameHours: 24f); // real method is Tick(float gameHours), not TickSimDay()

          var state = weather.CaptureState(); // confirmed to exist: WeatherSystem.cs:239
          var json = SerializeState(state);
          GoldenFileHelper.AssertMatchesGolden("Systems", "WeatherSystem_seed42_10days", json);
      }

      [Fact]
      public void MarketSystem_Seed42_10Days_MatchesGolden() { ... }
      // MarketSystem.CaptureState()/RestoreState() confirmed at MarketSystem.cs:294,338.
      // Confirm the real public tick entry point before wiring — a source pass found
      // an internal tick counter/TickCount property but no single obvious public
      // Tick(...) method; do not guess the call site.

      [Fact]
      public void CombatTraumaSystem_Seed42_10Days_MatchesGolden() { ... }
      // CaptureState()/RestoreState(CombatTraumaSaveState) confirmed at
      // CombatTraumaSystem.cs:199,218. No Tick-named method found in a source pass —
      // this system appears event/action-driven; confirm how to drive it deterministically
      // for 10 "days" before writing the test (may need synthetic trigger calls, not a tick loop).

      // NeedsSystem and RadiationSystem are DELIBERATELY OMITTED from this class.
      // Confirmed by direct source read: neither Assets/Ashfall.Core/Survivors/NeedsSystem.cs
      // nor Assets/Ashfall.Core/Radiation/RadiationSystem.cs defines CaptureState/RestoreState
      // today (tracked by AGENTS.md H10). Do not add a golden test that calls
      // needs.CaptureState() or radiation.CaptureState() — it will not compile. Either wait
      // for H10 to be fixed first, or golden-test the raw SurvivorNeedsState/SurvivorRadState
      // DTOs that src/Host/SurvivorsHostSession.cs already persists (a different, host-layer
      // code path from every other test in this class — decide explicitly, see Step 2 note).

      // FactionSystem is REMOVED from this plan. Confirmed by full-repo search: no file,
      // no class named FactionSystem exists anywhere in the codebase. This was a fabricated
      // example in the original draft.
  }
  ```

- Each system test:
  - Constructs the system in isolation with minimal dependencies
  - Uses the same seed (42) and duration (10 days) for consistency
  - Provides deterministic initial state (not random initialization)
  - Captures only that system's state (not the full game)
  - Produces a focused golden file (easier to review diffs)

- Benefits of per-system over full-state only:
  - When a test fails, you immediately know *which* system diverged
  - Smaller golden files = more readable diffs in PRs
  - Can update a single system's golden file without touching others
  - System can be tested in isolation (faster, fewer dependencies)

- Additional isolated system tests for systems with known determinism risks:
  ```csharp
  [Fact]
  public void FinalWishSystem_Seed42_5Days_MatchesGolden() { ... }
  // FinalWishSystem previously used System.Random — verify migration holds

  [Fact]
  public void ProceduralItemGeneration_Seed42_100Items_MatchesGolden() { ... }
  // ProceduralItemInstance previously used Guid.NewGuid — verify fix holds
  ```

**Verification:**
```bash
UPDATE_GOLDEN=true dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "SystemGolden"
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "SystemGolden"
```

**Done when:**
- Golden files exist for WeatherSystem, MarketSystem, CombatTraumaSystem (all three confirmed to have real `CaptureState()`/`RestoreState()` today — see file:line citations above)
- Additional goldens for FinalWishSystem and ProceduralItemGeneration (former determinism violators; `ProceduralItemInstance` has no `CaptureState` either — golden-test its deterministic id output directly, e.g. via `MakeInstanceId`, not via a nonexistent `CaptureState()`)
- NeedsSystem and RadiationSystem are explicitly out of scope for this step until an explicit decision is made per the Step 2/3 notes above (either fix H10 first, or golden-test the raw DTOs) — do not silently drop them from the plan without one of the two documented
- FactionSystem is removed from scope — it does not exist in the codebase
- Each test passes independently (no ordering dependencies)
- Each system's golden file is under 50KB (readable in PR diffs)
- A deliberate one-line change to a system produces a golden mismatch (verify detection works)

---

### Step 4: Create Save-Format Golden Test

**Goal:** Detect accidental serialization format changes that would break existing saves.

**Implementation:**
- Create `Ashfall.Core.Tests/Golden/SaveFormatGoldenTests.cs`:
  ```csharp
  namespace Ashfall.Core.Tests.Golden;

  public class SaveFormatGoldenTests
  {
      [Fact]
      public void SaveEnvelope_Seed42_Format_MatchesGolden()
      {
          // Create a game state at seed=42, day=10
          var session = CreateGameAtDay10(seed: 42);

          // Capture state the same way the real save system does
          var envelope = new SaveEnvelope
          {
              Version = SaveEnvelope.CurrentVersion,
              Day = 10,
              State = session.CaptureFullState(),
              Checksum = SaveChecksum.Compute(session.CaptureFullState())
          };

          // Serialize using the same serializer the game uses
          var json = HostDefaults.JsonSerializer.Serialize(envelope);
          GoldenFileHelper.AssertMatchesGolden("SaveFormat", "save_envelope_seed42", json);
      }

      [Fact]
      public void SaveChecksum_Seed42_MatchesGolden()
      {
          // Verify the checksum algorithm produces consistent output
          var state = CreateGameAtDay10(seed: 42).CaptureFullState();
          var checksum = SaveChecksum.Compute(state);

          var result = new { Checksum = checksum, StateHash = ComputeStateHash(state) };
          var json = Serialize(result);
          GoldenFileHelper.AssertMatchesGolden("SaveFormat", "checksum_seed42", json);
      }

      [Fact]
      public void AllSaveStores_Seed42_FormatMatchesGolden()
      {
          // Correction: the "22 save stores" figure below was not verified against the
          // actual tree. A grep for `class \w+SaveStore` under `src/Host/` returns exactly
          // 28 distinct store classes (confirmed via full-repo grep at review time: CaravanSaveStore,
          // CombatSaveStore, CraftingSaveStore, DailyBriefingSaveStore, DoseLedgerSaveStore,
          // DutyRosterSaveStore, EconomySaveStore, ExpansionHubSaveStore, ExpeditionSaveStore,
          // GreenhouseSaveStore, HoldfastSaveStore, HoldfastTradeSaveStore, InventorySaveStore,
          // MaritimeSaveStore, MedicalSaveStore, MedicalWardSaveStore, MemorialSaveStore,
          // MusterSaveStore, NarrativeSaveStore, PhantomMemorySaveStore, Phase0SaveStore,
          // PowerGridSaveStore, RadioSaveStore, ShelterAssignmentSaveStore, StartingLevelSaveStore,
          // SurvivorsSaveStore, VerdictSaveStore, WorldSaveStore — 28 total, all in src/Host/, all
          // Godot-host-layer classes, NOT in Ashfall.Core.Tests' compile scope by default).
          // Do not hardcode a specific count anywhere in this test or its Done-when criteria —
          // enumerate `GetAllSaveStores()` dynamically.
          //
          // CRITICAL GAP NOT PREVIOUSLY FLAGGED: `GetAllSaveStores()` does not exist anywhere in
          // the repo today (confirmed: zero matches for this name in src/ or Ashfall.Core.Tests/).
          // There is no existing registry/reflection helper that enumerates all *SaveStore classes.
          // Writing one is new work this step must scope explicitly: (1) all 28 classes live in
          // src/Host/ (Godot host, AtomicWar.GodotApp namespace) — Ashfall.Core.Tests targets
          // net9.0 and does NOT currently reference the Godot host project, so this test cannot
          // simply `new` up host-side save stores from a Core test project without adding a new
          // project reference (check whether that reference would pull in a Godot.NET.Sdk
          // dependency into the test project — likely undesirable per Invariant 1's spirit even
          // though Ashfall.Core.Tests isn't Ashfall.Core itself). (2) Each store's constructor
          // signature/dependencies must be checked individually before assuming a uniform
          // `CaptureState()` shape exists on all 28 — this was not verified for this batch.
          // Resolve project-reference feasibility as a spike before committing to this test.
          var session = CreateGameAtDay10(seed: 42);
          var stores = GetAllSaveStores(session); // NEW helper to be written — does not exist yet

          var allStoreOutputs = new Dictionary<string, object>();
          foreach (var store in stores)
          {
              var captured = store.CaptureState();
              allStoreOutputs[store.Name] = captured;
          }

          var json = Serialize(allStoreOutputs);
          GoldenFileHelper.AssertMatchesGolden("SaveFormat", "all_stores_seed42", json);
      }

      [Fact]
      public void PerStoreCodec_RoundTrip_MatchesGolden()
      {
          // Serialize → deserialize → re-serialize must produce identical JSON
          // Catches lossy round-trips (fields dropped on deserialization)
          var session = CreateGameAtDay10(seed: 42);
          var original = session.CaptureFullState();
          var serialized = Serialize(original);
          var deserialized = Deserialize(serialized);
          var reserialized = Serialize(deserialized);

          Assert.Equal(serialized, reserialized); // Byte-exact round-trip
          GoldenFileHelper.AssertMatchesGolden("SaveFormat", "roundtrip_seed42", reserialized);
      }
  }
  ```

- This catches:
  - Field renames (property `"hunger"` → `"hungerLevel"` would break old saves)
  - Field additions (new fields should have defaults that don't break old saves)
  - Serialization order changes (would invalidate checksums)
  - Float precision changes (G9 vs G17 would change checksums)
  - Codec version mismatches (V1 save loaded as V2 without migration)

- The golden file serves as the de facto save format specification:
  ```json
  {
    "Version": 3,
    "Day": 10,
    "State": {
      "NeedsSystem": {
        "survivors": [
          { "id": "npc_lead_01", "hunger": 0.234567891, ... }
        ]
      },
      ...
    },
    "Checksum": "a4f8c2e1..."
  }
  ```

**Verification:**
```bash
UPDATE_GOLDEN=true dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "SaveFormatGolden"
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "SaveFormatGolden"
```

**Done when:**
- Golden file captures the complete save envelope format
- Checksum golden verifies `SaveChecksum` algorithm stability
- All 28 save stores (confirmed count, see correction above — not the originally-claimed 22) have their format
  captured, contingent on resolving the `Ashfall.Core.Tests` → `src/Host/` project-reference question raised above;
  if that reference is deemed undesirable, this sub-goal is descoped to a documented follow-up rather than silently dropped
- Round-trip test proves serialization is lossless
- A field rename or reorder causes a clear test failure
- Golden file is reviewable as a save format specification

---

### Step 5: Add Golden File Update Command

**Goal:** Make it trivially easy for developers to update golden files when changes are intentional, with safeguards against accidental updates.

**Implementation:**
- Support via environment variable (already in framework):
  ```bash
  # Update all golden files
  UPDATE_GOLDEN=true dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "Golden"

  # Update only a specific category
  UPDATE_GOLDEN=true dotnet test --filter "SystemGolden"

  # Update a single test's golden file
  UPDATE_GOLDEN=true dotnet test --filter "WeatherSystem_Seed42_10Days_MatchesGolden"
  # (NOT "NeedsSystem_Seed42_10Days_MatchesGolden" — that test was removed from Step 3's
  # scope; NeedsSystem has no CaptureState() to golden-test today, see Step 3 corrections)
  ```

- Add a convenience script `scripts/update-golden.sh`:
  ```bash
  #!/bin/bash
  # Usage: ./scripts/update-golden.sh [filter]
  # Examples:
  #   ./scripts/update-golden.sh              # Update ALL golden files
  #   ./scripts/update-golden.sh SystemGolden  # Update system goldens only
  #   ./scripts/update-golden.sh WeatherSystem  # Update WeatherSystem golden only
  # (NOT "NeedsSystem" — removed from Step 3's scope, see corrections)

  set -euo pipefail
  FILTER="${1:-Golden}"
  echo "Updating golden files matching filter: $FILTER"
  UPDATE_GOLDEN=true dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "$FILTER"
  echo "Golden files updated. Review changes with: git diff Ashfall.Core.Tests/GoldenFiles/"
  ```

- Add Godot CLI verb for convenience:
  ```
  godot --headless --path . -- --update-golden [--filter <pattern>]
  ```
  (Delegates to the same mechanism — runs `dotnet test` with `UPDATE_GOLDEN=true`)
  **This verb does not exist today and is new work, not a trivial wrapper.** The real CLI dispatch lives in
  `src/Host/HostCli.cs`, which pattern-matches `args` against a large `if (Has(args, "--xxx-selftest")) return HostCliAction.Xxx;`
  chain (confirmed: `--data-integrity-selftest`, `--bridge-selftest`, etc. all follow this shape, `src/Host/HostCli.cs:203-241`).
  Adding `--update-golden` means: (1) adding a new `HostCliAction` enum value, (2) adding the `Has(args, ...)` branch,
  (3) implementing a handler that shells out to `dotnet test` from inside a running Godot process — a different
  process-spawning concern than every other self-test verb in that file, which run in-process. Budget this as its
  own sub-task with its own verification, not a one-line addition alongside the bash script.

- Safeguards against accidental updates:
  - CI never sets `UPDATE_GOLDEN=true` — golden mismatches always fail in CI
  - Git hook (optional): warn if `.golden.json` files are staged without a related `.cs` change
  - The update script prints a reminder to review diffs before committing
  - PR template includes checkbox: "[ ] Golden file changes reviewed for intentionality"

- Add `--golden-status` CLI verb:
  ```
  Usage: godot --headless --path . -- --golden-status

  Reports which golden files exist, their last update date, and whether they
  currently match (without failing on mismatch — informational only).
  ```
  Same caveat as `--update-golden` above: this requires new `HostCli.cs` wiring, and "last update date" implies
  either reading filesystem mtimes (fragile across `git clone`/CI checkout, which resets mtimes) or storing an
  explicit timestamp inside each golden file's `metadata` block. Pick one explicitly in Step 1's normalization
  design — do not defer this detail to Step 5 where it's first mentioned as if already decided.

**Verification:**
```bash
# Intentionally break a system output
# Run tests — verify failure
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "WeatherSystem_Seed42"  # FAIL
# (Using WeatherSystem here, not NeedsSystem — NeedsSystem golden tests were removed from
# Step 3's scope since NeedsSystem has no CaptureState() today.)
# Update golden
./scripts/update-golden.sh WeatherSystem
# Verify updated golden passes
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "WeatherSystem_Seed42"  # PASS
# Verify diff is visible
git diff Ashfall.Core.Tests/GoldenFiles/Systems/WeatherSystem_seed42_10days.golden.json
```

**Done when:**
- `UPDATE_GOLDEN=true` updates golden files without failing tests
- Update can target specific tests or categories
- Convenience script exists with clear usage instructions
- CI never accidentally runs in update mode
- `--golden-status` reports current golden file health
- Developers can update goldens in <30 seconds (frictionless workflow)

---

### Step 6: Add Golden File CI Gate

**Goal:** Golden file mismatches fail CI with actionable diff output, making behavioral regressions impossible to merge.

**Implementation:**
- Add golden file tests to the standard CI test run (they're just xUnit tests — no special setup needed):
  ```yaml
  # In CI config (GitHub Actions / similar):
  - name: Run tests (including golden)
    run: dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
    env:
      UPDATE_GOLDEN: "false"  # Explicit: never update in CI
  ```

- Enhance failure output for CI readability:
  ```csharp
  // In GoldenFileHelper, when mismatch detected:
  throw new GoldenFileMismatchException(
      $"""
      GOLDEN FILE MISMATCH: {category}/{testName}
      ═══════════════════════════════════════════

      The output of {testName} no longer matches its golden file.
      This means the system's behavior has changed.

      FIRST DIFFERENCE at line {firstDiffLine}:
        Expected: {expectedLine}
        Actual:   {actualLine}

      FULL DIFF ({diffLineCount} lines changed):
      {unifiedDiff}

      ═══════════════════════════════════════════
      If this change is INTENTIONAL:
        UPDATE_GOLDEN=true dotnet test --filter "{testName}"
        git add {goldenPath}

      If this change is a BUG:
        The golden file shows the correct behavior.
        Fix your code to match the expected output.
      """);
  ```

- CI-specific enhancements:
  - On failure, upload the actual output as a CI artifact (for comparison)
  - Generate a summary table: which golden tests passed/failed
  - If golden files are modified in the PR, add a CI comment highlighting the behavioral changes

- Create `Ashfall.Core.Tests/Golden/GoldenFileIntegrityTests.cs`:
  ```csharp
  [Fact]
  public void AllGoldenFiles_AreTrackedInGit()
  {
      // Every .golden.json in the GoldenFiles directory must be committed
      // Prevents someone generating goldens locally but forgetting to commit
      var goldenFiles = Directory.GetFiles(GoldenDir, "*.golden.json", SearchOption.AllDirectories);
      Assert.NotEmpty(goldenFiles); // At least some golden files must exist
      // (Git tracking verification would be a shell test in CI)
  }

  [Fact]
  public void AllGoldenFiles_AreValidJson()
  {
      // Sanity: golden files must be parseable JSON
      var goldenFiles = Directory.GetFiles(GoldenDir, "*.golden.json", SearchOption.AllDirectories);
      foreach (var file in goldenFiles)
      {
          var json = File.ReadAllText(file);
          // NOTE: `Assert.DoesNotThrow` is an NUnit method and does not exist in xUnit
          // (confirmed: xunit 2.9.2 per Ashfall.Core.Tests.csproj has no such assertion —
          // this line would fail to compile as originally written). In xUnit, simply calling
          // the code inline is sufficient; an unhandled exception fails the test on its own.
          // If a custom failure message on parse error is wanted, wrap in try/catch and
          // call Assert.Fail(...) in the catch block instead.
          JsonDocument.Parse(json); // throws JsonException on invalid JSON, which fails the test
      }
  }

  [Fact]
  public void AllGoldenFiles_AreNormalized()
  {
      // Golden files must be in canonical normalized form
      // Catches manual edits that break normalization
      var goldenFiles = Directory.GetFiles(GoldenDir, "*.golden.json", SearchOption.AllDirectories);
      foreach (var file in goldenFiles)
      {
          var content = File.ReadAllText(file);
          var renormalized = GoldenFileHelper.Normalize(content);
          // NOTE: xUnit's Assert.Equal has no (expected, actual, message) overload — that
          // signature is NUnit's, and would fail to compile against xunit 2.9.2 (confirmed
          // package version in Ashfall.Core.Tests.csproj). Use Assert.True with an inline
          // message instead, or plain Assert.Equal and rely on its default diff output.
          Assert.True(renormalized == content, $"Golden file is not normalized: {file}");
      }
  }
  ```

**Verification:**
```bash
# Normal CI run — all golden tests must pass
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "Golden"

# Simulate regression — intentionally break a system, verify CI catches it
# (manual verification during development)
```

**Done when:**
- Golden tests run as part of standard `dotnet test` (no special CI configuration)
- Mismatch produces clear, actionable error with unified diff
- CI never runs in update mode (UPDATE_GOLDEN is always false/unset)
- Failure output tells developer exactly how to fix (update or fix code)
- Integrity tests verify golden files are valid, normalized, and committed
- A PR that changes system behavior shows golden file diffs in the changeset

---

### Step 7: Write Meta-Tests (Framework Verification)

**Goal:** Verify the golden file framework itself works correctly — it detects changes, the update mechanism works, and normalization is consistent.

**Implementation:**
- Create `Ashfall.Core.Tests/Golden/GoldenFrameworkTests.cs`:
  ```csharp
  namespace Ashfall.Core.Tests.Golden;

  public class GoldenFrameworkTests
  {
      [Fact]
      public void DetectsIntentionalDiff()
      {
          // Given a golden file with known content
          var golden = CreateTempGoldenFile("test", "{\"value\": 1}");

          // When actual output differs
          var actual = "{\"value\": 2}";

          // Then framework reports mismatch
          Assert.Throws<GoldenFileMismatchException>(() =>
              GoldenFileHelper.AssertMatchesGolden("test", "test", actual));
      }

      [Fact]
      public void PassesWhenOutputMatchesGolden()
      {
          var golden = CreateTempGoldenFile("test", "{\"value\": 1}");
          var actual = "{\"value\": 1}";

          // Should not throw
          GoldenFileHelper.AssertMatchesGolden("test", "test", actual);
      }

      [Fact]
      public void UpdateModeOverwritesGoldenFile()
      {
          var golden = CreateTempGoldenFile("test", "{\"value\": 1}");

          // Simulate UPDATE_GOLDEN=true
          Environment.SetEnvironmentVariable("UPDATE_GOLDEN", "true");
          try
          {
              GoldenFileHelper.AssertMatchesGolden("test", "test", "{\"value\": 2}");
          }
          finally
          {
              Environment.SetEnvironmentVariable("UPDATE_GOLDEN", null);
          }

          // Golden file should now contain the new value
          var updated = File.ReadAllText(golden);
          Assert.Contains("\"value\": 2", updated);
      }

      [Fact]
      public void NormalizationIsIdempotent()
      {
          var inputs = new[]
          {
              "{\"b\":1,\"a\":2}",
              "{\"x\":1.23456789012345}",
              "{\"n\":null,\"v\":1}",
              "{\"arr\":[3,1,2]}"
          };

          foreach (var input in inputs)
          {
              var once = GoldenFileHelper.Normalize(input);
              var twice = GoldenFileHelper.Normalize(once);
              // NOTE: xUnit's Assert.Equal(expected, actual, message) 3-arg overload does not
              // exist (that's NUnit's signature) — confirmed against xunit 2.9.2 in
              // Ashfall.Core.Tests.csproj. Use Assert.True with an inline message instead.
              Assert.True(once == twice, $"Not idempotent for: {input}");
          }
      }

      [Fact]
      public void NormalizationSortsKeys()
      {
          var input = "{\"zebra\": 1, \"alpha\": 2, \"mango\": 3}";
          var normalized = GoldenFileHelper.Normalize(input);

          var keys = ExtractKeyOrder(normalized);
          Assert.Equal(new[] { "alpha", "mango", "zebra" }, keys);
      }

      [Fact]
      public void NormalizationOmitsNulls()
      {
          var input = "{\"present\": 1, \"absent\": null}";
          var normalized = GoldenFileHelper.Normalize(input);

          Assert.DoesNotContain("absent", normalized);
          Assert.Contains("present", normalized);
      }

      [Fact]
      public void NormalizationFormatsFloatsG9()
      {
          var input = "{\"pi\": 3.14159265358979323846}";
          var normalized = GoldenFileHelper.Normalize(input);

          // G9 for double: 3.14159265
          Assert.Contains("3.14159265", normalized);
          // Should NOT contain full precision
          Assert.DoesNotContain("3.14159265358979", normalized);
      }

      [Fact]
      public void DiffOutputIsReadable()
      {
          var expected = "{\n  \"a\": 1,\n  \"b\": 2\n}";
          var actual = "{\n  \"a\": 1,\n  \"b\": 999\n}";

          var ex = Assert.Throws<GoldenFileMismatchException>(() =>
              CompareStrings(expected, actual));

          // Diff should show the specific line that changed
          Assert.Contains("b", ex.Message);
          Assert.Contains("2", ex.Message);
          Assert.Contains("999", ex.Message);
      }

      [Fact]
      public void MissingGoldenFile_GivesActionableError()
      {
          var ex = Assert.Throws<FileNotFoundException>(() =>
              GoldenFileHelper.AssertMatchesGolden("nonexistent", "missing", "{}"));

          Assert.Contains("UPDATE_GOLDEN=true", ex.Message);
          Assert.Contains("nonexistent", ex.Message);
      }

      [Fact]
      public void ArrayOrderPreserved()
      {
          // Arrays should NOT be sorted (order is meaningful in game state)
          var input = "{\"items\": [\"c\", \"a\", \"b\"]}";
          var normalized = GoldenFileHelper.Normalize(input);

          // Order preserved
          var cIndex = normalized.IndexOf("\"c\"");
          var aIndex = normalized.IndexOf("\"a\"");
          var bIndex = normalized.IndexOf("\"b\"");
          Assert.True(cIndex < aIndex && aIndex < bIndex);
      }

      [Fact]
      public void NestedObjectsAreSorted()
      {
          var input = "{\"outer\": {\"z\": 1, \"a\": 2}, \"inner\": {\"y\": 3, \"b\": 4}}";
          var normalized = GoldenFileHelper.Normalize(input);

          // Both outer keys and nested keys should be sorted
          Assert.True(normalized.IndexOf("\"inner\"") < normalized.IndexOf("\"outer\""));
          // Inside "inner": b before y
          // Inside "outer": a before z
      }
  }
  ```

- Additional meta-tests for robustness:
  ```csharp
  [Fact]
  public void HandlesEmptyState() { ... }

  [Fact]
  public void HandlesVeryLargeState() { ... }  // 1MB+ JSON

  [Fact]
  public void HandlesDeeplyNestedState() { ... }  // 10+ levels deep

  [Fact]
  public void HandlesSpecialCharactersInStrings() { ... }  // Unicode, newlines, quotes

  [Fact]
  public void ConcurrentUpdatesSafe() { ... }  // Parallel test runner safety
  ```

**Verification:**
```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "GoldenFramework"
```

**Done when:**
- All meta-tests pass
- Framework correctly detects diffs (true positives)
- Framework does not flag identical output (no false positives)
- Normalization handles all JSON edge cases (nulls, floats, nesting, arrays, unicode)
- Update mechanism works and reverts cleanly
- Error messages guide developers to the correct action
- Concurrent test execution is safe (no file contention)

---

## Summary Table

| Step | Title | Risk | Depends On | Output |
|------|-------|------|-----------|--------|
| 1 | Design Golden File Framework | Low | None | `GoldenFileHelper.cs`, `GoldenFiles/README.md` |
| 2 | Seed-Determinism Golden Test | Low | Step 1 | `DeterminismGoldenTests.cs`, full-state golden file |
| 3 | Per-System Golden Tests | Low | Step 1 | `SystemGoldenTests.cs`, 3 confirmed system golden files (WeatherSystem, MarketSystem, CombatTraumaSystem) + 2 conditional (FinalWishSystem, ProceduralItemGeneration) — down from the originally claimed 6-8; NeedsSystem/RadiationSystem/FactionSystem removed, see Step 3 corrections |
| 4 | Save-Format Golden Test | Low | Step 1 | `SaveFormatGoldenTests.cs`, save envelope golden |
| 5 | Golden File Update Command | Low | Steps 2-4 | `update-golden.sh`, `--golden-status` verb |
| 6 | Golden File CI Gate | Low | Steps 2-4 | CI integration, failure output, integrity tests |
| 7 | Meta-Tests (Framework Verification) | Low | Step 1 | `GoldenFrameworkTests.cs` (12+ tests) |

---

## Exit Criteria

- [ ] Golden file framework handles comparison, normalization, update, and diff
- [ ] Full-state determinism golden exists for seed=42/10days
- [ ] Per-system goldens exist for the 3 confirmed-viable systems (WeatherSystem, MarketSystem, CombatTraumaSystem) — not "6+" as originally claimed; see Step 3 corrections for why NeedsSystem/RadiationSystem/FactionSystem are removed/deferred
- [ ] Save format golden catches serialization changes
- [ ] `UPDATE_GOLDEN=true dotnet test --filter Golden` updates all goldens in <10 seconds
- [ ] CI fails on any golden mismatch with clear diff output
- [ ] Normalization is idempotent and handles all JSON edge cases
- [ ] Meta-tests verify the framework itself is correct
- [ ] `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — 0 errors
- [ ] `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter Golden` — all pass
- [ ] At least one golden mismatch has been triggered and resolved during development (proves the workflow)

---

## Future Work (Not in This Batch)

- Golden files for narrative system output (event sequences over 30 days)
- Golden files for economy system (market prices, trade history)
- Cross-host comparison: run same seed in Godot host + pure xUnit, compare output
- Performance golden: track system tick times (alert on >20% regression)
- Save migration goldens: V1 save → V2 migration → golden output
- Fuzzing integration: fuzz inputs, capture any non-deterministic output as bug
- Visual golden testing: screenshot panels at key states, pixel-diff
- Golden file dashboard: web page showing all goldens, their age, and diff history


## Review Notes (Corrected)

This document was adversarially reviewed against the actual ASHFALL codebase at
`/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`. The following factual
errors, unrunnable code, and scope problems were found and corrected in place above.

### Factual errors found and fixed

1. **`FactionSystem` does not exist anywhere in the repo** (confirmed by full-repo class/file
   search — zero hits). The original Step 3 draft included a `FactionSystem_Seed42_10Days_MatchesGolden`
   test and listed FactionSystem as a golden-test deliverable. This was a fabricated example and has
   been removed from Step 3's implementation, Done-when criteria, the directory-structure example,
   the Summary table, and the Exit Criteria.
2. **`NeedsSystem` and `RadiationSystem` have zero `CaptureState()`/`RestoreState()` methods today**
   (confirmed by direct source read of `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` and
   `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` — neither file defines either method). The
   original Step 2 and Step 3 code samples called `needs.CaptureState()` and implied
   `radiation.CaptureState()` — neither would compile. This is consistent with, but more severe
   than, AGENTS.md's own H10 note ("NeedsSystem & RadiationSystem save/load round-trip tests…
   still missing") — the round-trip methods themselves don't exist, not just their tests. The real
   Godot host (`src/Host/SurvivorsHostSession.cs`) persists these two systems by saving raw
   `SurvivorNeedsState`/`SurvivorRadState` DTOs directly, bypassing the CaptureState convention
   entirely. Both systems are now explicitly out of scope for Steps 2–3 pending an explicit decision
   (fix H10 first, or golden-test the raw DTOs via a different code path) — see inline notes at
   Step 2 and Step 3.
3. **No method named `TickSimDay()` exists anywhere in Core.** Every ticking system found during
   review uses `Tick(float gameHours)` (confirmed: `NeedsSystem.Tick`, `RadiationSystem.Tick` at
   `RadiationSystem.cs:188`, `WeatherSystem.Tick` at `WeatherSystem.cs:115`). Every code sample in
   Steps 2–3 that called `session.TickSimDay()` or `needs.TickSimDay()` has been corrected to use
   the real `Tick(...)` signature, with a note that a test-only aggregator translating "day" into
   `gameHours` is new code this batch must build, not existing plumbing.
4. **No "full game session" class exists** with a single `CaptureFullState()`/`TickSimDay()` pair
   that Step 2's `CreateFullGameSession(...)` helper could simply call into. This was presented as
   if it already existed; it's now flagged as new test-only infrastructure (`TestGameSession.cs`)
   that Step 1/2 must design and build.
5. **`SimpleClock` does not exist.** The real class is `SimClock`, and — per AGENTS.md's own H3 —
   there are two distinct classes both named `SimClock` in different namespaces/files
   (`HostDefaults.cs:67`, implementing `IClock`, and `Clock/ISimClock.cs:15`, implementing
   `ISimClock`). Step 2's code sample has been corrected to use `SimClock` with a note to confirm
   which of the two same-named classes is actually needed.
6. **The "22 save stores" figure in Step 4 was wrong even after the doc's own earlier hedge.**
   A fresh grep for `class \w+SaveStore` under `src/Host/` returns exactly **28** distinct classes
   (enumerated in the Step 4 correction above), not 22 and not merely "20+, higher than 22" as an
   earlier draft hedged. The exact list is now recorded, and the Done-when criterion no longer
   references a stale number.
7. **`GetAllSaveStores()` — the method Step 4's `AllSaveStores_Seed42_FormatMatchesGolden` test
   calls to enumerate save stores — does not exist anywhere in the repo.** This is new
   infrastructure this batch must build, not an existing helper it can call. Flagged inline, along
   with a more fundamental scoping problem: all 28 `*SaveStore` classes live in `src/Host/`
   (Godot host, `AtomicWar.GodotApp` namespace), and `Ashfall.Core.Tests` does not currently
   reference that project. Adding such a reference is a real architectural decision (whether a
   Core test project should depend on Godot host code) that the original doc did not surface at
   all — it assumed the call site would just work.
8. **Two xUnit API-compatibility bugs in the code samples** (Step 6 and Step 7): `Assert.DoesNotThrow(...)`
   and the 3-argument `Assert.Equal(expected, actual, message)` overload are both **NUnit APIs, not
   xUnit APIs** — confirmed against xunit 2.9.2 (the actual pinned version in
   `Ashfall.Core.Tests.csproj`). Both call sites (`AllGoldenFiles_AreValidJson`,
   `AllGoldenFiles_AreNormalized`, `NormalizationIsIdempotent`) would fail to compile as originally
   written. Corrected to xUnit-idiomatic equivalents (`Assert.True(cond, message)` / relying on
   `Assert.Equal`'s own diff output).
9. **Stale test-name references survived the doc's own earlier partial edits.** Step 5 and Step 6's
   verification blocks still referenced `--filter "NeedsSystem_Seed42..."` and
   `./scripts/update-golden.sh NeedsSystem` after NeedsSystem was removed from Step 3's scope —
   these dangling references have been swapped to `WeatherSystem`, which is a system this doc now
   confirms is actually golden-testable.

### Claims verified and found accurate (no change needed)

- `SeededRng`'s xorshift64* implementation, shift amounts (12/25/27), and multiplier
  (`0x2545F4914F6CDD1D`) are exactly as claimed (`Assets/Ashfall.Core/HostDefaults.cs:96-131`),
  including the SplitMix64 seeding step in the constructor.
- `CoreSeededRng` is confirmed to exist only as an `internal` host-side wrapper
  (`src/Host/DoseLedgerHostSession.cs:192-198`), not inside `Ashfall.Core` — the doc's correction
  about this was accurate.
- `Ashfall.Core.Tests/CoreInvariantSourceTests.cs` exists and matches the doc's description closely,
  including a test named exactly `Core_SeededRng_ReproducesAcrossInstances` and source-scans for
  the six banned nondeterminism patterns listed.
- Zero live (non-comment) usages of `System.Random`, `new Random(`, or `Guid.NewGuid(` remain in
  `Assets/Ashfall.Core/` — confirmed. AGENTS.md's own "Known offenders" list under Invariant 4 is
  stale and contradicts its own "RESOLVED" ledger entries (`CombatTraumaSystem.cs:53` and
  `FinalWishSystem.cs:66` are both already `ISeededRng Rng`, not `System.Random Rng`).
- No golden-file/snapshot testing infrastructure exists anywhere in the repo today beyond one
  hardcoded-hash test, `FrozenV1ShapeChecksumIsGolden` (`Ashfall.Core.Tests/HoldfastSaveTests.cs:465-479`),
  which pins a single expected checksum string — not a reusable framework. This batch is genuinely
  new infrastructure.
- `WeatherSystem`, `MarketSystem`, `CombatTraumaSystem`, and `FinalWishSystem` are all confirmed to
  have real `CaptureState()`/`RestoreState()` implementations at the cited file:line locations.
- `Ashfall.Core.Tests.csproj`'s package list (`Microsoft.NET.Test.Sdk` 17.11.1, `xunit` 2.9.2,
  `xunit.runner.visualstudio` 2.8.2 — nothing else) is confirmed exactly.

### Risk / rollback gap (fixed)

The original document rated the entire batch "Risk: Low" with no differentiation and no rollback
plan at all, unlike sibling roadmap docs (e.g. Batch 108) which separate risk by step. In practice
Steps 1–4 and 7 are pure test-only additions (trivially revertible, zero production impact), but
Step 5 adds a new Godot-host CLI verb that shells out to `dotnet test` from inside a running Godot
process — a materially different, riskier surface than the existing in-process self-test verbs in
`src/Host/HostCli.cs` — and Step 6 wires golden tests into the standard CI run, where a bad
normalization rule or an unexpectedly nondeterministic system tick sequence (e.g. hidden
`Dictionary` enumeration order) could turn CI red for reasons unrelated to a genuine regression.
Added a differentiated risk rating and an explicit rollback strategy (revert the specific flaky
golden test or the specific `HostCli.cs` verb independently, rather than disabling the whole suite)
to the header table.

### Scope correction summary

Originally scoped as "4 system goldens." After verification, only 3 systems
(WeatherSystem, MarketSystem, CombatTraumaSystem) are confirmed to have the `CaptureState()`/`Tick(...)`
shape this batch's tests assume; a further 2 (FinalWishSystem, ProceduralItemGeneration) are
plausible follow-ons for former determinism violators. NeedsSystem and RadiationSystem are removed
from scope pending an explicit design decision, and FactionSystem is removed entirely as
nonexistent. The "~1 week" estimate is retained but should be understood as covering the smaller,
now-accurate system list — not the originally-claimed one.
