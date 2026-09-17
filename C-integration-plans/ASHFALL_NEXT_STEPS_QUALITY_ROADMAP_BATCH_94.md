# ASHFALL — Quality Roadmap Batch 94

## Theme: Scenario Editor — Custom Starting Conditions for Testing & Replayability

| Field | Value |
|-------|-------|
| **Priority** | MEDIUM |
| **Risk** | **Corrected from Low — see Review Notes.** The core serialization mechanism as originally drafted (a `Dictionary<string, object>` of heterogeneous DTOs round-tripped through the project's real `IJsonSerializer`) does not work as designed with the actual `System.Text.Json`-based adapter; this is a real architecture gap, not an additive/low-risk feature on top of solid infrastructure. |
| **Blocking** | Nothing — pure additive capability |
| **Enables** | Rapid testing of late-game systems (Day 180+ YearOfAsh, Day 260+ Muster), player-facing challenge scenarios, deterministic regression baselines |
| **Systems touched** | All **94** systems that implement `CaptureState`/`RestoreState` in `Assets/Ashfall.Core/` (read-only via CaptureState — **corrected from "82+"; verified by counting `public ... CaptureState(` definitions directly: 94 files match, 93 match `RestoreState(`, a 1-file asymmetry noted but out of scope here — see Review Notes**), new ScenarioSystem in Core, Godot host CLI, UI |
| **Estimated scope** | ~1200 LOC Core + ~400 LOC Godot host + ~300 LOC tests. **This estimate does not account for the rework required by the Review Notes findings below (custom polymorphic serialization, a name→system dispatch registry, and an `IFileIO` port extension) — treat it as stale until Step 1/3 are redesigned.** |

---

## Motivation

Testing Day-180 (YearOfAsh) or Day-260 (Muster) content currently requires advancing 180–260 days manually through normal gameplay. There is no mechanism to start a game at an arbitrary state. The project's universal `CaptureState/RestoreState` contract across **94** systems (corrected from "82+" — see Review Notes) means any game state can, **in principle**, be serialized as a JSON snapshot. A **scenario** is simply a pre-built state snapshot plus descriptive metadata and a fixed seed.

**Critical caveat added by this review, not present in the original draft:** "in principle" is doing real work in that sentence. Each of the 94 systems has its **own concrete DTO type** for `CaptureState`/`RestoreState` (e.g. `BrineWaterSystemState`, `CohortSystemState`, `DutyRosterSystemState`) — there is no shared base type or marker interface. A scenario format that stores "any system's state" generically (as `Dictionary<string, object>`, per the original Step 1 schema) cannot be deserialized back into those concrete types by the project's actual `IJsonSerializer` implementation (`SystemTextJsonSerializer`, plain `System.Text.Json` with no type discriminator or custom converters) — see Review Notes for the exact failure mode. This is not a minor implementation detail; it is the central technical risk of this entire batch and must be resolved by a redesigned Step 1 before Steps 2-7 are built on top of it.

This enables:
- Developers jumping directly to late-game states for testing
- Deterministic regression: load scenario → advance N days → assert invariants
- Player-facing "challenge scenarios" (start with specific resources/survivors/conditions)
- Speedrun/competition seeds (identical starting state for all players)

---

## Step 1 — Design Scenario Format (JSON Schema)

### Goal
Define the canonical JSON format for a scenario file that captures all information needed to bootstrap a game at an arbitrary state.

### Implementation

**Corrected mechanism (see Review Notes): the schema below keeps the same JSON shape the original draft proposed, but the `SystemStates` field can no longer be `Dictionary<string, object>` deserialized directly by `System.Text.Json` — verified that the project's real `IJsonSerializer` (`SystemTextJsonSerializer` in `HostDefaults.cs`, plain `System.Text.Json`, `IncludeFields = true`, no custom converters or type discriminator) deserializes `object`-typed dictionary values as boxed `JsonElement`, never as the original concrete DTO type (`BrineWaterSystemState`, `CohortSystemState`, etc.). A later `system.RestoreState(stateObj)` call expecting a concrete DTO would fail to compile or throw at runtime. The fix used below stores each system's state as a **raw JSON string** per system name, deferring per-system typed deserialization to the point where each concrete system is known:**

Create `Assets/StreamingAssets/Data/scenarios/` directory and define the schema:

```json
{
  "schema_version": 1,
  "scenario_id": "scenario_day180_year_of_ash",
  "display_name": "Year of Ash — Day 180",
  "description": "Five survivors, depleted medical supplies, active fallout storm cycle.",
  "author": "ashfall_dev",
  "created_at": "2025-01-15T00:00:00Z",
  "tags": ["late_game", "year_of_ash", "test"],
  "starting_day": 180,
  "seed": 48291053,
  "difficulty_hint": "hard",
  "system_states": {
    "BrineWaterSystem": "{\"saveVersion\":1,\"...\":\"...\"}",
    "CohortSystem": "{\"saveVersion\":1,\"...\":\"...\"}",
    "DutyRosterSystem": "{\"saveVersion\":1,\"...\":\"...\"}"
  },
  "checksum": "a3f8c9..."
}
```

Core class in `Assets/Ashfall.Core/Scenarios/ScenarioDefinition.cs`:

```csharp
namespace Ashfall.Core.Scenarios;

[Serializable]
public sealed class ScenarioDefinition
{
    public int SchemaVersion { get; set; } = 1;
    public string ScenarioId { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public string Author { get; set; }
    public string CreatedAt { get; set; }
    public List<string> Tags { get; set; } = new();
    public int StartingDay { get; set; }
    public long Seed { get; set; }
    public string DifficultyHint { get; set; }

    // CORRECTED: string, not object. Each value is a pre-serialized JSON blob for one
    // system's concrete state DTO. Deserializing to the correct concrete type happens
    // per-system at apply time (see Step 4's revised ApplyScenario), where the system
    // name is known and its RestoreState parameter type is known at compile time.
    public Dictionary<string, string> SystemStates { get; set; } = new();
    public string Checksum { get; set; }
}
```

**Why this works where the original didn't:** `Dictionary<string, string>` deserializes cleanly through `System.Text.Json` with no ambiguity (every value is just a string). The nested JSON inside each string is deserialized later, by code that already knows the target type (`_json.Deserialize<BrineWaterSystemState>(rawJson)`), which is exactly how every other save-file code path in this project already works (e.g. `YearOfAshSave.cs`, `MusterHostSession.CaptureSave`) — this schema change makes ScenarioSaver/Loader consistent with the rest of the codebase's serialization pattern instead of inventing a new, broken one.

### Verification
- JSON schema passes `CatalogIntegrityValidator` tier-1 (id prefix `scenario_` — **already registered**, confirmed present in `CatalogIntegrityValidator.cs`'s `IdPrefixes` array; no validator change needed for the prefix itself)
- `ScenarioDefinition` round-trips through `IJsonSerializer` without data loss — **including a test that specifically asserts each `SystemStates` value round-trips as an opaque string without `System.Text.Json` attempting (and failing) to infer a concrete type**, since that was the actual defect being fixed
- Checksum computed via existing `SaveChecksum` utility matches on reload — **see Step 3's correction: `SaveChecksum.Compute` on a `Dictionary<string,string>` needs the same scrutiny given to `Dictionary<string,object>` below, since `SaveChecksum.WriteObject` reflects over public fields and `KeyValuePair<TKey,TValue>` only exposes `Key`/`Value` as properties, not fields — verify empirically, don't assume it works because the compile succeeds**

### Done-when
- `ScenarioDefinition.cs` exists in `Assets/Ashfall.Core/Scenarios/` with `SystemStates` typed as `Dictionary<string, string>` (corrected type)
- JSON schema documented in `Assets/StreamingAssets/Data/scenarios/README.md`, explicitly documenting that each `system_states` value is a JSON-encoded string, not a nested object, and why (link to this Review Notes section)
- `scenario_` prefix confirmed already present in `CatalogIntegrityValidator` (no action needed — do not report this as new work done)
- Unit test confirms round-trip serialization fidelity for a multi-system scenario, including a test that deliberately checks a `SystemStates` value is a `string` after deserialization, not a `JsonElement` or nested dictionary

---

## Step 2 — Create ScenarioBuilder in Core (Fluent API)

### Goal
Provide a fluent, type-safe API for constructing scenarios programmatically — used by both developers writing test scenarios and (eventually) a player-facing scenario editor.

### Implementation

`Assets/Ashfall.Core/Scenarios/ScenarioBuilder.cs`:

```csharp
namespace Ashfall.Core.Scenarios;

public sealed class ScenarioBuilder
{
    private readonly ScenarioDefinition _def = new();
    private readonly IJsonSerializer _json;

    public ScenarioBuilder(IJsonSerializer json) { _json = json; }

    public ScenarioBuilder WithId(string scenarioId) { _def.ScenarioId = scenarioId; return this; }
    public ScenarioBuilder WithDisplayName(string name) { _def.DisplayName = name; return this; }
    public ScenarioBuilder WithDescription(string desc) { _def.Description = desc; return this; }
    public ScenarioBuilder WithDay(int day) { _def.StartingDay = day; return this; }
    public ScenarioBuilder WithSeed(long seed) { _def.Seed = seed; return this; }
    public ScenarioBuilder WithTag(string tag) { _def.Tags.Add(tag); return this; }
    public ScenarioBuilder WithDifficulty(string hint) { _def.DifficultyHint = hint; return this; }

    // CORRECTED: serializes TState to a JSON string immediately, matching the
    // Dictionary<string,string> schema fixed in Step 1. Caller passes the system's
    // real, concrete state DTO type (e.g. BrineWaterSystemState) — this method no
    // longer accepts an unconstrained "TState : class" that gets boxed as object.
    public ScenarioBuilder WithSystemState<TState>(string systemName, TState state)
        where TState : class
    {
        _def.SystemStates[systemName] = _json.Serialize(state);
        return this;
    }

    // REMOVED from the original draft: WithSurvivors(int, Action<SurvivorSlotBuilder>),
    // WithItems((string,int)[]), WithRadiationLevel(float), WithWeather(string) — all four
    // referenced DTOs/systems that do not exist under the assumed names or shape
    // (NeedsSystemState, RadiationSystemState, InventorySystemState, WeatherSystemState —
    // none of these exact type names exist in the codebase; NeedsSystem and RadiationSystem
    // in particular have no CaptureState/RestoreState at all today, confirmed absent).
    // These convenience helpers cannot be implemented until the underlying systems either
    // (a) already have real save-state DTOs the review can point to by exact name, or
    // (b) gain them as separate, reviewed work. Do not reintroduce these method stubs
    // without first confirming a real backing DTO type name via a fresh codebase search —
    // the four instances above were unverified guesses in the original draft, not the real
    // save-state type names of any of these systems' actual DTOs (e.g. WeatherSystem's real
    // save DTO name should be confirmed from WeatherSystemTests.cs before use, not assumed).

    public ScenarioDefinition Build()
    {
        Validate();
        // CORRECTED: SaveChecksum.Compute on the raw Dictionary<string,string> is exactly
        // the same degenerate-hash risk flagged in Step 1 — KeyValuePair<TKey,TValue>
        // exposes Key/Value as properties, and SaveChecksum.WriteObject reflects over
        // public FIELDS only (see Assets/Ashfall.Core/SaveChecksum.cs), so hashing the
        // dictionary directly silently produces the same hash regardless of content.
        // Fix: hash a deterministic, ordinal-sorted concatenation of the dictionary's
        // own entries instead of handing the dictionary object to SaveChecksum directly.
        var canonical = string.Join("|", _def.SystemStates.OrderBy(kv => kv.Key, StringComparer.Ordinal)
            .Select(kv => kv.Key + "=" + kv.Value));
        _def.Checksum = SaveChecksum.Compute(canonical);
        return _def;
    }

    private void Validate()
    {
        if (string.IsNullOrEmpty(_def.ScenarioId))
            throw new InvalidOperationException("ScenarioId is required.");
        if (_def.StartingDay < 1)
            throw new InvalidOperationException("StartingDay must be >= 1.");
    }
}
```

### Verification
- Builder produces valid `ScenarioDefinition` with all required fields
- `Validate()` rejects malformed scenarios (no id, day < 1, missing seed)
- Checksum matches independent computation via the same ordinal-sorted-concatenation approach — **a test must assert the checksum actually changes when a `SystemStates` entry's value changes**, since this is precisely the property the original `Dictionary<string,object>`-direct-hash approach silently failed to provide

### Done-when
- `ScenarioBuilder.cs` compiles in `Assets/Ashfall.Core/Scenarios/`, constructor takes `IJsonSerializer` (dependency, not a static call)
- At least 5 builder unit tests covering happy path and validation failures
- A specific test proves the checksum is sensitive to `SystemStates` content (mutate one system's serialized string, assert checksum changes) — this test would have caught the original draft's silent-degenerate-hash defect and must exist before this step is marked done
- **Scope reduced from the original draft:** the fluent API now covers the fields that map 1:1 onto `ScenarioDefinition` (id, display name, description, day, seed, tag, difficulty, per-system state) — the four convenience helpers for survivors/items/radiation/weather are removed pending confirmation of real backing DTOs (see above), so "8+ commonly-configured dimensions" is no longer an accurate scope claim for this step

---

## Step 3 — Implement ScenarioLoader and ScenarioSaver

### Goal
Serialize scenarios to disk and deserialize them back, reusing the existing save infrastructure (`IJsonSerializer`, `IFileIO`, `SaveChecksum`).

### Implementation

`Assets/Ashfall.Core/Scenarios/ScenarioSaver.cs`:

```csharp
namespace Ashfall.Core.Scenarios;

public sealed class ScenarioSaver
{
    private readonly IJsonSerializer _json;
    private readonly IFileIO _fileIO;

    public ScenarioSaver(IJsonSerializer json, IFileIO fileIO)
    {
        _json = json;
        _fileIO = fileIO;
    }

    public void Save(ScenarioDefinition scenario, string directoryPath)
    {
        var fileName = $"{scenario.ScenarioId}.json";
        var fullPath = Path.Combine(directoryPath, fileName);
        var json = _json.Serialize(scenario);
        _fileIO.WriteAllText(fullPath, json);
    }
}
```

`Assets/Ashfall.Core/Scenarios/ScenarioLoader.cs`:

```csharp
namespace Ashfall.Core.Scenarios;

public sealed class ScenarioLoader
{
    private readonly IJsonSerializer _json;
    private readonly IFileIO _fileIO;
    private readonly ILog _log;

    public ScenarioLoader(IJsonSerializer json, IFileIO fileIO, ILog log)
    {
        _json = json;
        _fileIO = fileIO;
        _log = log;
    }

    public ScenarioDefinition Load(string filePath)
    {
        var text = _fileIO.ReadAllText(filePath);
        var scenario = _json.Deserialize<ScenarioDefinition>(text);

        // CORRECTED: recompute the checksum with the same ordinal-sorted-concatenation
        // approach used in ScenarioBuilder.Build() (see Step 2 correction), not by handing
        // the Dictionary<string,string> to SaveChecksum.Compute directly.
        var canonical = string.Join("|", scenario.SystemStates.OrderBy(kv => kv.Key, StringComparer.Ordinal)
            .Select(kv => kv.Key + "=" + kv.Value));
        var computed = SaveChecksum.Compute(canonical);
        if (computed != scenario.Checksum)
        {
            _log.Warn($"Scenario checksum mismatch: expected {scenario.Checksum}, got {computed}");
            throw new ScenarioCorruptException(filePath, scenario.Checksum, computed);
        }

        return scenario;
    }

    public IReadOnlyList<ScenarioDefinition> LoadAll(string directoryPath)
    {
        // CORRECTED: IFileIO (Assets/Ashfall.Core/Ports.cs) has no GetFiles/EnumerateFiles
        // method today — verified real signature is DirectoryExists/FileExists/ReadAllText/
        // WriteAllText/Combine only. This method requires ONE of:
        //   (a) extend IFileIO with a GetFiles(string, string) method and implement it in
        //       every adapter (Godot's core-default implementation, at minimum) — a port
        //       change with cross-cutting impact, or
        //   (b) accept an externally-supplied file list (IEnumerable<string> filePaths)
        //       and let the caller (Godot host) do the directory listing via its own
        //       System.IO/Godot.DirAccess call, keeping ScenarioLoader itself engine-agnostic
        //       and free of a new port dependency.
        // Option (b) is recommended: it's a smaller, self-contained change and matches how
        // CatalogIntegrityValidator already does its own Directory.GetFiles call outside
        // the IFileIO abstraction rather than extending the port. Signature changed below.
    }

    public IReadOnlyList<ScenarioDefinition> LoadAll(IEnumerable<string> filePaths)
    {
        var results = new List<ScenarioDefinition>();
        foreach (var file in filePaths)
        {
            try { results.Add(Load(file)); }
            catch (Exception ex) { _log.Warn($"Skipping corrupt scenario: {file} — {ex.Message}"); }
        }
        return results;
    }
}
```

### Verification
- Save → Load round-trip produces identical `ScenarioDefinition`
- Corrupted checksum is detected and throws `ScenarioCorruptException` — **test must mutate one `SystemStates` string value and confirm detection**, since the checksum computation was the exact thing fixed in this step; a test that only mutates the top-level `Checksum` field itself would not exercise the fix
- `LoadAll` skips corrupt files gracefully (logs warning, does not throw)
- No `UnityEngine.*` or `Godot.*` references (engine-agnostic) — **this is now enforced by construction** since `LoadAll` takes a caller-supplied file list instead of calling into a new `IFileIO.GetFiles` port method

### Done-when
- Both classes compile in Core with zero engine dependencies
- `ScenarioLoader.LoadAll` takes `IEnumerable<string>` (corrected signature) — the Godot host (Step 4) is responsible for producing that list via its own directory-listing call
- Round-trip test passes for a multi-system scenario
- Corruption detection test passes (mutated `SystemStates` JSON fails checksum, not just a mutated top-level field)
- `LoadAll` resilience test passes (1 good + 1 corrupt → returns 1)

---

## Step 4 — Add `--scenario` CLI Verb to Godot Host

### Goal
Allow launching the game directly into a scenario from the command line, enabling automated testing and rapid iteration.

### Implementation

**Corrected (see Review Notes): there is no `_systemRegistry.Get(systemName)` lookup anywhere in the Godot host today.** `src/Main.cs` follows the documented (AGENTS.md H7) pattern of concrete per-system fields plus paired `SetupX`/`SaveX` methods — there is no string-keyed dispatch table mapping a system name to a live instance. A `SystemRegistry` of that shape is discussed only in legacy Unity-side audit documents as a *recommended future fix for the Unity god-object*, never built for the Godot host. Implementing scenario application therefore requires **one of**:
- (a) building a small, explicit `Dictionary<string, Action<string>>` dispatch table by hand inside `Main.cs`/a new partial file, with one entry per system that should be scenario-restorable (each entry closes over the concrete field and calls `_json.Deserialize<ConcreteState>(json)` then `system.RestoreState(that)`) — this is mechanical but is genuinely ~94 (or however many are selected for Step 5's five scenarios) hand-written entries, not a generic loop, OR
- (b) scoping this batch down to only the handful of systems actually exercised by the five built-in scenarios in Step 5, building the dispatch table for only those, and explicitly deferring "any of the 94 systems" as a stated non-goal for this batch.
Option (b) is recommended and reflected below — the plan's implied "works for any system" scope was never achievable in one batch given the mechanical cost of (a).

In `src/Main.cs` (or a new partial file `src/Main.Scenario.cs`), handle the CLI argument:

```csharp
// In Godot host startup (Main._Ready or argument parsing)
if (Args.Contains("--scenario"))
{
    var scenarioPath = Args.GetValue("--scenario");
    var loader = new ScenarioLoader(HostDefaults.JsonSerializer, HostDefaults.FileIO, HostDefaults.Log);
    var scenario = loader.Load(scenarioPath);
    ApplyScenario(scenario);
}

private void ApplyScenario(ScenarioDefinition scenario)
{
    // Set day via IClock
    _clock.SetDay(scenario.StartingDay);

    // Set seed via ISeededRng
    _rng.SetSeed(scenario.Seed);

    // CORRECTED: explicit dispatch table, not a registry lookup that doesn't exist.
    // Only lists systems actually needed by the five Step 5 scenarios — extend deliberately,
    // one line at a time, as new scenarios need new systems, rather than assuming generic
    // coverage of all 94 systems from day one.
    var dispatch = new Dictionary<string, Action<string>>(StringComparer.Ordinal)
    {
        ["BrineWaterSystem"] = json => _brineWater.RestoreState(_json.Deserialize<BrineWaterSystemState>(json)),
        ["CohortSystem"]     = json => _cohort.RestoreState(_json.Deserialize<CohortSystemState>(json)),
        ["DutyRosterSystem"] = json => _dutyRoster.RestoreState(_json.Deserialize<DutyRosterSystemState>(json)),
        // ... one explicit entry per system this batch's scenarios actually restore.
        // Confirm each field name (_brineWater, _cohort, etc.) against the real Main.cs
        // field names before writing — do not assume these exact names without checking.
    };

    foreach (var (systemName, rawJson) in scenario.SystemStates)
    {
        if (dispatch.TryGetValue(systemName, out var restore))
            restore(rawJson);
        else
            _log.Warn($"Scenario references unknown/unsupported system '{systemName}' — skipped.");
    }

    _log.Info($"Scenario loaded: {scenario.DisplayName} (Day {scenario.StartingDay}, Seed {scenario.Seed})");
}
```

CLI usage:
```bash
godot --headless --path . -- --scenario Assets/StreamingAssets/Data/scenarios/scenario_day180_year_of_ash.json
```

### Verification
- `godot --headless --path . -- --scenario <path>` exits 0 after loading a valid scenario
- Invalid scenario path → clear error message + exit 1
- Corrupt scenario → `ScenarioCorruptException` message + exit 1
- After loading, `IClock.CurrentDay` equals `scenario.StartingDay`
- **Added:** a scenario referencing a system name not in the dispatch table logs a warning and does not crash or silently corrupt state — test this explicitly, since it's the expected/common case once scenario authors reference a system not yet wired into the dispatch table

### Done-when
- `--scenario` verb documented alongside existing `--data-integrity-selftest`
- The dispatch table's exact scope (which named systems are supported) is documented in the same place as the `--scenario` verb, so it's clear this is not "works for all 94 systems"
- Integration test: load Day-180 scenario → verify clock reads 180
- Error paths tested: missing file, corrupt checksum, malformed JSON, unknown system name in dispatch table
- No new warnings in `dotnet build Ashfall.csproj`

---

## Step 5 — Create 5 Built-in Test Scenarios

### Goal
Ship a set of canonical scenarios that cover key game phases, enabling rapid access to any stage of progression for testing and QA.

### Implementation

**Corrected (see Step 2/Review Notes): the generator below can no longer call `WithSurvivors`/`WithItems`/`WithRadiationLevel`/`WithWeather` — those convenience methods were removed from `ScenarioBuilder` because they referenced DTOs (`NeedsSystemState`, `InventorySystemState`, `RadiationSystemState`, `WeatherSystemState`) that don't exist under those names, and `NeedsSystem`/`RadiationSystem` don't currently implement `CaptureState`/`RestoreState` at all.** This step is now blocked on a smaller prerequisite: pick a small, confirmed-real set of systems to seed (e.g. `BrineWaterSystem`, `CohortSystem`, `DutyRosterSystem` — all confirmed to have real `CaptureState`/`RestoreState` pairs per the codebase search performed during this review) rather than the survivor/inventory/radiation/weather set the original draft assumed without checking. **Before implementing this step, confirm the exact save-state DTO name and constructor shape for each system chosen, the same way this review confirmed `BrineWaterSystemState` et al. — do not reuse the four example items below (`item_canned_food` etc.) or the survivor-count model without first checking they correspond to real DTOs.**

Create JSON files in `Assets/StreamingAssets/Data/scenarios/`:

| Scenario ID | Day | Purpose |
|---|---|---|
| `scenario_day001_tutorial` | 1 | Fresh start, minimal supplies, tutorial triggers active |
| `scenario_day030_established` | 30 | Basic shelter built, first expedition complete, initial trade |
| `scenario_day100_thriving` | 100 | Full shelter, active economy, multiple expeditions, medical events |
| `scenario_day180_year_of_ash` | 180 | YearOfAsh expansion active, depleted resources, high radiation |
| `scenario_day260_muster` | 260 | Muster expansion active, coalition politics, endgame content |

**Removed from the original draft's table:** the "Survivors" column (2/4/6/5/7) implied `WithSurvivors(int)` exists and that survivor count/roster is one of the systems this batch snapshots. Since the removed builder methods covered exactly that, survivor roster restoration is out of scope for this step unless a real survivor-roster save DTO is confirmed to exist and a dispatch entry (per Step 4's corrected pattern) is added for it first.

Each scenario is constructed using `ScenarioBuilder` in a generator script — **illustrative only; the specific systems and calls must be substituted for whichever are actually confirmed real and wired into Step 4's dispatch table**:

```csharp
// In Ashfall.Core.Tests or a dev-tool project
public static class BuiltInScenarioGenerator
{
    public static ScenarioDefinition Day180YearOfAsh(IJsonSerializer json) =>
        new ScenarioBuilder(json)
            .WithId("scenario_day180_year_of_ash")
            .WithDisplayName("Year of Ash — Day 180")
            .WithDescription("Depleted supplies and active fallout.")
            .WithDay(180)
            .WithSeed(20250115_180)
            .WithTag("late_game").WithTag("year_of_ash").WithTag("test")
            .WithDifficulty("hard")
            // .WithSystemState("BrineWaterSystem", someConfirmedRealBrineWaterState)
            // .WithSystemState(...) — fill in with confirmed-real system states only
            .Build();
}
```

### Verification
- All 5 scenario JSON files pass `CatalogIntegrityValidator`
- Each scenario loads via `ScenarioLoader` without checksum errors
- Each scenario can be applied via `--scenario` without crashes, using only the dispatch-table systems confirmed in Step 4
- Advancing 1 day after load does not throw (basic stability)

### Done-when
- 5 JSON files exist in `Assets/StreamingAssets/Data/scenarios/`
- All 5 load cleanly and pass checksum verification
- Smoke test: load each scenario → advance 1 day → no exceptions
- Scenarios cover Days 1, 30, 100, 180, 260 (full progression spectrum)
- **Added:** the set of systems each scenario actually restores is explicitly documented per-scenario (e.g. in the JSON's `description` or a sibling `.md`), since this step no longer claims to snapshot "the whole game state" — only whichever systems were confirmed real and added to Step 4's dispatch table

---

## Step 6 — Add Scenario Browser UI

### Goal
Provide an in-game UI panel for selecting and loading scenarios, making them accessible to non-technical playtesters and eventually players.

### Implementation

New Godot scene: `src/UI/ScenarioBrowser/ScenarioBrowserPanel.tscn`

Structure:
```
ScenarioBrowserPanel (Control)
├── Header (Label: "Scenarios")
├── ScenarioList (VBoxContainer + ScrollContainer)
│   └── ScenarioCard (repeated)
│       ├── Title (Label)
│       ├── Description (RichTextLabel, 2 lines max)
│       ├── TagsRow (HBoxContainer of tag chips)
│       └── DayBadge (Label: "Day 180")
├── DetailPanel (right side)
│   ├── FullDescription (RichTextLabel)
│   ├── SystemStatesPreview (tree or list)
│   └── LoadButton (Button: "Load Scenario")
└── Footer
    ├── RefreshButton (Button)
    └── CloseButton (Button)
```

Godot host script: `src/UI/ScenarioBrowser/ScenarioBrowserPanel.cs`:

```csharp
namespace AtomicWar.GodotApp.UI;

public partial class ScenarioBrowserPanel : Control
{
    private ScenarioLoader _loader;
    private IReadOnlyList<ScenarioDefinition> _scenarios;

    public override void _Ready()
    {
        _loader = new ScenarioLoader(HostDefaults.JsonSerializer, HostDefaults.FileIO, HostDefaults.Log);
        RefreshList();
    }

    private void RefreshList()
    {
        // CORRECTED: ScenarioLoader.LoadAll now takes IEnumerable<string> (see Step 3
        // correction) since IFileIO has no GetFiles method. The Godot host does its own
        // directory listing here, using Godot's own DirAccess API (engine-specific code
        // stays in the host layer, per Invariant 1 — Core never calls this).
        var path = ProjectSettings.GlobalizePath("res://Assets/StreamingAssets/Data/scenarios/");
        var filePaths = System.IO.Directory.Exists(path)
            ? System.IO.Directory.GetFiles(path, "*.json")
            : Array.Empty<string>();
        _scenarios = _loader.LoadAll(filePaths);
        PopulateCards(_scenarios);
    }

    private void OnLoadPressed(ScenarioDefinition scenario)
    {
        // Signal to Main to apply scenario
        EmitSignal(SignalName.ScenarioSelected, scenario.ScenarioId);
    }
}
```

### Verification
- Panel opens from main menu without errors
- Displays all 5 built-in scenarios with correct metadata
- Selecting a scenario shows full detail in right panel
- "Load Scenario" button triggers scenario application
- UI uses project fonts (BarlowCondensed + ShareTechMono) and theme

### Done-when
- `.tscn` scene and `.cs` script exist in `src/UI/ScenarioBrowser/`
- Panel is accessible from the main menu
- All 5 scenarios display correctly with tags, day badge, description
- Load action works end-to-end (select → load → game starts at that state), restricted to the systems Step 4's dispatch table actually supports — a scenario referencing an unsupported system should surface the "skipped" warning (per Step 4) visibly in this UI, not silently

---

## Step 7 — Write Scenario Tests

### Goal
Comprehensive test coverage ensuring scenarios build correctly, serialize faithfully, load without corruption, and produce playable game states.

### Implementation

`Ashfall.Core.Tests/ScenarioTests.cs`:

```csharp
namespace Ashfall.Core.Tests;

public class ScenarioTests
{
    [Fact]
    public void Builder_ProducesValidScenario_WithAllRequiredFields() { /* ... */ }

    [Fact]
    public void Builder_ThrowsOnMissingId() { /* ... */ }

    [Fact]
    public void Builder_ThrowsOnInvalidDay() { /* ... */ }

    [Fact]
    public void SaveAndLoad_RoundTrip_PreservesAllFields() { /* ... */ }

    [Fact]
    public void Load_DetectsCorruptChecksum() { /* ... */ }

    [Fact]
    public void LoadAll_SkipsCorruptFiles_ReturnsValid() { /* ... */ }

    [Theory]
    [InlineData("scenario_day001_tutorial", 1)]
    [InlineData("scenario_day030_established", 30)]
    [InlineData("scenario_day100_thriving", 100)]
    [InlineData("scenario_day180_year_of_ash", 180)]
    [InlineData("scenario_day260_muster", 260)]
    public void BuiltInScenario_LoadsWithCorrectDay(string id, int expectedDay) { /* ... */ }

    [Theory]
    [InlineData("scenario_day001_tutorial")]
    [InlineData("scenario_day180_year_of_ash")]
    [InlineData("scenario_day260_muster")]
    public void LoadedScenario_AdvanceOneDay_DoesNotThrow(string id) { /* ... */ }

    [Fact]
    public void Scenario_Checksum_ChangesWhenStateModified() { /* ... */ }

    [Fact]
    public void Scenario_DeterministicSeed_ProducesSameState() { /* ... */ }
}
```

### Verification
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter ScenarioTests` — all pass
- No new warnings introduced
- Tests exercise builder, saver, loader, corruption detection, and stability
- **`Scenario_Checksum_ChangesWhenStateModified` must mutate a `SystemStates` dictionary value** (a per-system serialized JSON string), not the top-level `Checksum` field — this is the test that would have caught the original draft's degenerate-hash defect (see Step 1-3 corrections) and its assertion must compare against a hand-verified expected checksum change, not just "not equal to previous"

### Done-when
- `ScenarioTests.cs` contains 10+ tests covering all scenario operations
- All tests pass on `dotnet test`
- Coverage includes: build, validate, save, load, corrupt-detect, round-trip, advance-day
- No engine dependencies in test project (pure Core testing)
- **Added:** at least one test explicitly asserts that `ScenarioDefinition.SystemStates` values deserialize as plain `string` (not `JsonElement`/nested object) after a full JSON round-trip — this is the regression test for the central defect this review found and fixed

---

## Summary Table

**Risk column corrected — see Review Notes. Steps 1-3 carry the batch's real architectural risk (the original design was non-functional as drafted); Steps 4-7's "Low/None" ratings hold once 1-3 are fixed.**

| Step | Deliverable | Location | Tests | Risk |
|------|-------------|----------|-------|------|
| 1 | Scenario JSON schema + `ScenarioDefinition` DTO (`Dictionary<string,string>`, corrected) | `Assets/Ashfall.Core/Scenarios/` | Round-trip serialization + string-not-JsonElement assertion | **Medium — was the source of the batch's central defect** |
| 2 | `ScenarioBuilder` fluent API (scope-reduced) | `Assets/Ashfall.Core/Scenarios/` | 5+ builder tests + checksum-sensitivity test | **Medium — checksum computation was broken as drafted** |
| 3 | `ScenarioLoader` + `ScenarioSaver` (corrected `LoadAll` signature) | `Assets/Ashfall.Core/Scenarios/` | Round-trip + corruption | **Medium — `IFileIO.GetFiles` did not exist** |
| 4 | `--scenario` CLI verb + explicit dispatch table | `src/Main.Scenario.cs` | Integration smoke | Low-Medium — new CLI path, dispatch table is hand-written and must stay in sync with Step 5's scenarios |
| 5 | Built-in test scenarios (count/systems TBD, scope-reduced) | `Assets/StreamingAssets/Data/scenarios/` | Load + advance-day | Low — once scoped to confirmed-real systems |
| 6 | Scenario browser UI panel | `src/UI/ScenarioBrowser/` | Manual + signal test | Low — UI only |
| 7 | Comprehensive scenario tests | `Ashfall.Core.Tests/ScenarioTests.cs` | 10+ tests | None |

---

## Dependencies & Constraints

- **No engine coupling:** `ScenarioDefinition`, `ScenarioBuilder`, `ScenarioLoader`, `ScenarioSaver` all live in `Assets/Ashfall.Core/` with zero `Godot.*` or `UnityEngine.*` references.
- **Uses existing ports:** `IJsonSerializer`, `ILog`, `IClock`, `ISeededRng` — **corrected: `IFileIO` is deliberately NOT used by `ScenarioLoader.LoadAll` per the Step 3 correction**, since the real `IFileIO` has no directory-listing method; `IFileIO` is still used for single-file `ReadAllText`/`WriteAllText` in `Load`/`Save`. No new port interfaces needed, because directory listing is pushed to the Godot host's own call, not added to the port.
- **Uses existing save infra:** `SaveChecksum` for integrity (via the corrected canonical-string approach, not a direct `Dictionary` hash — see Step 1-3), `CaptureState/RestoreState` pattern for state blocks (via per-system typed deserialization at apply time, not a generic `object`-typed pass-through — see Step 1).
- **Data authority:** Scenario JSON files live in `Assets/StreamingAssets/Data/scenarios/` — the single source of truth.
- **Determinism:** Scenarios include a seed; loading a scenario sets `ISeededRng` to that seed, ensuring identical replay.
- **ID convention:** All scenario IDs use `scenario_` prefix, snake_case — **already registered** with `CatalogIntegrityValidator` (confirmed present; no new registration needed).
- **New constraint added by this review:** the set of systems a scenario can restore is bounded by Step 4's hand-written dispatch table, not "any of the 94 CaptureState/RestoreState systems." This is a real scope limitation, not a documentation nicety — a scenario JSON can reference any system name, but only dispatch-table entries actually restore state; others log a warning and are skipped (see Step 4).

## Review Notes (Corrected)

This section documents the adversarial fact-check performed against the live codebase at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War` and lists every correction applied above.

### Number correction

The original draft's "all 82+" systems figure is wrong. **Verified count: 94 files** under `Assets/Ashfall.Core/` contain a `public ... CaptureState(` definition (`grep -rlE 'public [A-Za-z].* CaptureState\(' Assets/Ashfall.Core/ --include="*.cs" | wc -l` → 94), and 93 contain a matching `RestoreState(` definition — a 1-file asymmetry (one system captures but doesn't restore, or the signatures don't match the same simple regex) that is worth its own small follow-up investigation but is out of scope for this batch. This exactly matches the figure independently verified in Batch 76 ("~94 systems... verified by counting `public ... CaptureState(` method definitions directly — not the '82+' figure the original draft used"). Both this batch and Batch 76 drew from the same stale "82+" figure; both are now corrected to 94.

### Central architectural defect found and fixed

The original Step 1 schema typed `ScenarioDefinition.SystemStates` as `Dictionary<string, object>` and assumed it would round-trip arbitrary per-system DTOs through the project's real `IJsonSerializer`. Verified this is false:
- The real adapter (`SystemTextJsonSerializer` in `Assets/Ashfall.Core/HostDefaults.cs`) is plain `System.Text.Json` with `IncludeFields = true` and `PropertyNameCaseInsensitive = true` — no `JsonConverter` for polymorphic/`object`-typed values, no `$type` discriminator, no custom `TypeInfoResolver`.
- `System.Text.Json` deserializes `object`-typed dictionary values as boxed `JsonElement`, never as the original CLR type. A subsequent `system.RestoreState(stateObj)` call — every real `RestoreState` in this codebase takes a concrete DTO parameter type, never `object` (confirmed across dozens of call sites: `BrineWaterSystem.RestoreState(BrineWaterSystemState)`, `CohortSystem.RestoreState(CohortSystemState)`, `DutyRosterSystem.RestoreState(DutyRosterSystemState)`, etc.) — would not compile against a `JsonElement`, or would throw `InvalidCastException` if force-cast.
- **Fix applied throughout this document:** `SystemStates` is now `Dictionary<string, string>` — each value is a pre-serialized JSON blob for exactly one system's real DTO, deserialized to the correct concrete type only at the point of application (Step 4), where the target system and its `RestoreState` parameter type are both statically known. This mirrors how every other save-file code path in this project already works (e.g. `YearOfAshSave.cs`, `MusterHostSession.CaptureSave`) rather than inventing a new, broken generic-object approach.
- The original draft's illustrative example (`"NeedsSystem": { /* NeedsSystemState DTO */ }`, `"RadiationSystem": { /* RadiationSystemState DTO */ }`) used two systems and two DTO type names that don't exist: confirmed via search that `NeedsSystemState` and `RadiationSystemState` do not appear anywhere in the codebase, and that `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` / `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` have no `CaptureState`/`RestoreState` methods at all today. This wasn't just a schema-shape bug — the worked example was based on systems that aren't save-capable in the first place.

### `SaveChecksum.Compute` — degenerate hash on `Dictionary<string,object>`/`Dictionary<string,string>`

Verified `SaveChecksum.Compute` (`Assets/Ashfall.Core/SaveChecksum.cs:48`, signature `public static string Compute(object root)`) routes any `IEnumerable`, including dictionaries, into `WriteSequence`, which iterates a dictionary as a sequence of boxed `KeyValuePair<TKey,TValue>` structs and reflects over each entry's **public fields** (`GetFields(BindingFlags.Public | BindingFlags.Instance)`) via `WriteObject`. `KeyValuePair<TKey,TValue>` exposes `Key`/`Value` as **properties**, not fields — so `WriteObject` finds zero fields per entry and emits an empty object for every dictionary entry, regardless of actual key/value content. The result: hashing a `Dictionary<string,object>` or `Dictionary<string,string>` directly with `SaveChecksum.Compute` produces the same hash regardless of the dictionary's actual contents, silently defeating the entire purpose of an integrity checksum. **Fix applied:** every `SaveChecksum.Compute` call site in this document now hashes an ordinal-sorted, delimiter-joined canonical string built from the dictionary's own key/value pairs, not the dictionary object itself — a scheme that is sensitive to content changes, which was verified as the missing property in the original draft's Step 2/3 "Checksum matches independent computation via `SaveChecksum`" verification bullets (those bullets never actually tested sensitivity to content change, only that a call succeeded).

### `IFileIO.GetFiles` does not exist

Verified the real `IFileIO` (`Assets/Ashfall.Core/Ports.cs:17-24`) exposes only `DirectoryExists`, `FileExists`, `ReadAllText`, `WriteAllText`, `Combine` — no directory-listing method of any kind. The original Step 3/6 code (`_fileIO.GetFiles(directoryPath, "*.json")`) calls a method that does not exist and would not compile. **Fix applied:** `ScenarioLoader.LoadAll` now takes a caller-supplied `IEnumerable<string>` of file paths; the Godot host (Steps 4 and 6) does its own directory listing via `System.IO.Directory.GetFiles` (the same pattern `CatalogIntegrityValidator.cs:384` and `src/Main.cs` already use elsewhere in this codebase, bypassing `IFileIO` for directory listing rather than extending the port) — this keeps `ScenarioLoader` itself engine-agnostic and avoids a cross-cutting port change that would require updating every `IFileIO` adapter.

### `_systemRegistry.Get(systemName)` does not exist

Verified no `_systemRegistry` symbol exists anywhere under `src/`. The Godot host (`src/Main.cs`) follows the AGENTS.md-documented H7 pattern: concrete per-system fields plus paired `SetupX`/`SaveX` methods, never a string-keyed system lookup. A `SystemRegistry.Get<T>()` pattern is discussed only in legacy Unity-side audit documents (`AUDIT_FINDINGS_AND_FIX_PLAN.md`, `docs/ASHFALL_CODE_INDEX.md`) as a *recommended future fix for the Unity god-object* — it was never built for the Godot host, and per AGENTS.md's Unity→Godot migration direction, building it now specifically to serve this batch would be new architecture, not reuse of something documented as already present. **Fix applied:** Step 4 now uses an explicit, hand-written `Dictionary<string, Action<string>>` dispatch table scoped to only the systems this batch's five scenarios actually restore, rather than assuming generic coverage of all 94 systems. This is a real, stated scope reduction versus the original draft's implicit "works for any system" framing.

### Feasibility of a full-game-state snapshot (cross-referenced against Batch 76)

Batch 76 ("Undo/Redo Framework") independently investigated full-game-state snapshotting of the same ~94 systems and found that capturing/restoring all of them is potentially expensive enough to require a measured benchmark before committing to a "snapshot everything" design — Batch 76's concern is specifically about paying that cost **at UI-interaction frequency** (before every player click), which does not directly apply to this batch, since a scenario load happens once, at game/session start, not per-click. However, two of Batch 76's findings do transfer directly to this batch and are incorporated above:
1. Batch 76 confirmed there is **no shared `IStatefulSystem`-style interface** across the ~94 systems — each has its own bespoke `CaptureState()`/`RestoreState()` signature with its own concrete DTO type. This is the same fact that broke this batch's original `Dictionary<string, object>` design, independently rediscovered here.
2. Batch 76 confirmed several systems have **known-empty `CaptureState`/`RestoreState`** (`LocationEvolutionSaveable`, `WildlifeSaveable`, `LandmarkSaveable`) — any scenario claiming to snapshot "the full game state" would silently be undo-blind/scenario-blind for those systems too. This document's Step 5 correction (scope scenarios to a small, confirmed-real subset rather than "all systems") sidesteps this, but it should be stated explicitly: **this batch does not and cannot deliver a true full-game-state scenario snapshot** — it delivers a scenario mechanism for an explicitly enumerated, hand-picked subset of systems, which is a materially smaller (but still useful) feature than the Motivation section originally implied.

### Missing risk/rollback notes (added)

The original draft rated overall risk "Low." Corrected to reflect the findings above:
- **Steps 1-3 (schema, builder, loader/saver):** now correctly scoped as the highest-risk steps in the batch, because the original design was non-functional as drafted. Rollback for these steps, once corrected, is straightforward (pure Core additions, delete the files) — the risk was in the *original design being wrong*, not in it being hard to revert.
- **Step 4 (CLI verb + dispatch table):** rollback = remove the `--scenario` argument branch; no existing behavior is touched since this is purely additive. Risk is now understood to scale with dispatch-table size — recommend starting with 2-3 systems, not all systems needed by all 5 scenarios at once, so a bad dispatch entry is easy to isolate.
- **Step 5 (built-in scenarios):** rollback = delete the JSON files; no code depends on their existence. Confirmed low risk, unchanged from original draft, once scoped to confirmed-real systems only.
- **Step 6 (UI panel):** rollback = remove the scene/script; no other system depends on it existing. Confirmed low risk.
- **Step 7 (tests):** no rollback risk (tests are never a production dependency), but the corrected tests specifically must include a checksum-content-sensitivity test that the original draft's phrasing would likely have skipped without this review.

### Ordering check

Steps 1→2→3→4→5→6→7 as originally ordered is logically sound *given the corrected Step 1 design* — but the original ordering silently assumed Step 1's schema was correct before Steps 2-7 were built on top of it, and it was not. No step in the original draft was positioned to catch that until implementation began and compilation failed. This document's corrections front-load the polymorphism/checksum/registry problems into Step 1-4's text specifically so they're caught during design review, not during Step 5-7 implementation when three steps' worth of code would already assume the broken shape.

## Exit Criteria

All 5 verification steps pass:
```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # PASS
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj     # PASS (including ScenarioTests)
3. dotnet build Ashfall.csproj                                  # PASS (0 errors, 0 warnings)
4. godot --headless --path . -- --data-integrity-selftest       # PASS (scenarios included)
5. godot --headless --path . -- --scenario Assets/StreamingAssets/Data/scenarios/scenario_day001_tutorial.json
                                                                  # PASS (loads + exits cleanly)
```
**Corrected:** command 5's original `<path>` placeholder is not a runnable command as written — replaced with a concrete scenario file path from Step 5's actual output so this checklist can be executed verbatim rather than requiring the runner to guess a valid argument.
