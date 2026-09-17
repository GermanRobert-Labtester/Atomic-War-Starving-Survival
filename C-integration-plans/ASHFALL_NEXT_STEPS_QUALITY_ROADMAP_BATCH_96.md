# ASHFALL — Quality Roadmap Batch 96

## Theme: Telemetry Consent & Analytics Architecture for Playtesting

| Field | Value |
|-------|-------|
| **Priority** | LOW-MEDIUM |
| **Risk** | Low-Medium — opt-in, local-first, privacy-respecting, no cloud dependencies; risk is not zero because Step 2 requires extending (or adding a sibling to) the shared `IFileIO` port that every save/load system in the game already depends on |
| **Blocking** | Nothing — future playtesting support |
| **Enables** | Data-driven balancing, identifying dead content, session analytics, playtest reports |
| **Systems touched** | New `TelemetryEvent`/`TelemetryCollector`/`TelemetryConsent`/`TelemetryEvents`/`TelemetryExporter` classes in `Assets/Ashfall.Core/Telemetry/` (there is no single "`TelemetrySystem`" class as the original plan named it — it's five cooperating classes), the `IFileIO` port (extended or given a sibling), consent UI, and telemetry call sites across several real Core/host classes identified in the corrected Step 1/5 tables (not "15 high-value event emitters across existing systems" as a clean count — two of the original six named target systems don't exist under those names) |
| **Estimated scope** | ~900 LOC Core + ~300 LOC Godot host + ~350 LOC tests (unchanged from original estimate, but this excludes the `InMemoryFileIO`-equivalent test double and the `IFileIO` port extension, both of which are additional new work not counted in the original figure) |

---

## Motivation

The game is singleplayer and offline. There is no player account, no cloud backend, and no tracking infrastructure. **Verified: a grep across the entire codebase for `telemetry`/`analytics`/`Telemetry`/`Analytics` turns up zero actual systems, classes, or event emitters.** The only hits are (a) cosmetic UI mockup strings in `assets/ui/HtmlBundles/*.html` design files (e.g. a font token named `"telemetry-bold"`, a nav label "Thermal Telemetry" — pure visual design artifacts, not code), and (b) in-fiction narrative flavor text in `Assets/StreamingAssets/Data/narrative/*.json` (radio "ghost transmission" content describing fictional satellite/weather-buoy telemetry as story content). Neither is an analytics system. The motivation's premise — that this data does not exist today — is confirmed accurate.

But during playtesting, developers need quantitative data to answer critical design questions:

- Where do players die most often? (Day? Cause? System state?)
- Which UI panels are never opened? (Dead features?)
- What is the average session length? (Engagement?)
- Which encounters are chosen vs. skipped? (Content value?)
- Which items are never crafted? (Recipe balance?)
- How many days do players survive on average? (Difficulty curve?)

Currently this data does not exist — all balancing is done by feel and manual observation.

**No first-run consent UI pattern exists to model after.** A grep for `consent`/`FirstRun`/`opt-in`/`privacy` across the codebase found no matching UI pattern. The closest structural analog in the active Godot host is `src/UI/OpeningProtocolModal.cs` (wired in `src/Main.cs:977-994`) — a Day-1 gameplay modal (ration triage / maintenance directive / radio transmission choices) that follows an event-based pattern (`OnClose`, `OnRationPolicySelected`, etc.) similar to what a consent dialog would need, but it is gameplay content, not a settings/consent primitive. `src/UI/SettingsPanel.cs` exists and has toggle/option-row patterns (`MakeSettingRow`, `OptionButton`) that Step 3's "Settings toggle" should reuse rather than inventing new UI scaffolding — the plan did not previously reference either of these existing files.

The architecture is:
- **Local-first:** Events write to a JSON Lines file on disk. No network. No cloud.
- **Opt-in:** A consent screen on first run. Telemetry is disabled until explicitly enabled.
- **Anonymized:** No player identity, no hardware fingerprints, no IP addresses. Session ID is a random token regenerated each session.
- **Exportable:** Playtesters share `.jsonl` files manually (email, Discord, file drop).
- **Deletable:** Player can clear all telemetry data from settings at any time.

---

## Step 1 — Design TelemetryEvent Schema

### Goal
Define the canonical event format that captures gameplay telemetry without any personally identifiable information (PII).

### Implementation

`Assets/Ashfall.Core/Telemetry/TelemetryEvent.cs`:

```csharp
namespace Ashfall.Core.Telemetry;

[Serializable]
public sealed class TelemetryEvent
{
    /// <summary>Snake_case event type identifier (e.g., "session_start", "survivor_died").</summary>
    public string EventType { get; set; }

    /// <summary>UTC timestamp in ISO-8601 format.</summary>
    public string Timestamp { get; set; }

    /// <summary>In-game day when event occurred.</summary>
    public int Day { get; set; }

    /// <summary>Seconds since session start (gameplay time, not wall time).</summary>
    public double SessionElapsedSeconds { get; set; }

    /// <summary>Random session identifier (regenerated each launch, no PII).</summary>
    public string SessionId { get; set; }

    /// <summary>Arbitrary key-value payload specific to this event type.</summary>
    public Dictionary<string, string> Payload { get; set; } = new();
}
```

Event type registry (canonical list):

| Event Type | Payload Keys | Emitter (real class, verified) |
|---|---|---|
| `session_start` | `seed`, `scenario_id` (if any), `build_version` | Host startup (`src/Main.cs`) |
| `session_end` | `total_days`, `total_seconds`, `end_reason` | Host shutdown (`src/Main.cs`) |
| `day_advanced` | `day_number`, `survivor_count`, `total_items` | `TickSimDay` (`src/Main.cs:1643`) |
| `survivor_died` | `survivor_id`, `cause`, `day`, `age_days` | `Ashfall.Core.Survivors.NeedsSystem` (real class, verified: `Assets/Ashfall.Core/Survivors/NeedsSystem.cs`) — combat-caused deaths route through `CombatHostSession`/`TacticalCombatSystem` (see below), not a `CombatSystem` class, which does not exist |
| `item_crafted` | `recipe_id`, `item_id`, `quantity` | `Ashfall.Core.Crafting.CraftingSystem` (real class, verified: `Assets/Ashfall.Core/Crafting/CraftingSystem.cs`) |
| `expedition_started` | `expedition_id`, `destination`, `party_size` | `Ashfall.Core.Expeditions.ExpeditionSystem` (real class, verified) via `ExpeditionHostSession` |
| `expedition_completed` | `expedition_id`, `outcome`, `loot_count` | same as above |
| `encounter_choice` | `encounter_id`, `choice_index`, `choice_id` | **Corrected — no `EncounterSystem` class exists.** The real classes are `Ashfall.Core.Narrative.NarrativeEncounterSystem`/`EncounterCatalog` (general narrative encounters) and `Ashfall.Core.YearOfAsh.DoorEncounterSystem` (Year of Ash door encounters) — two separate systems, not one. This event needs two emission points, not one. |
| `trade_completed` | `partner_id`, `items_given`, `items_received` | **Corrected — no `TradeSystem` class exists.** Trade lives in `HoldfastTradeSession` (per `src/UI/FactionsPanel.cs:33`) and `Ashfall.Core` economy types referenced in `AGENTS.md` (`EconomySystem.GetTrust(factionId)`, per `CODE_AUDIT_REPORT.md`). Confirm the exact emission point during implementation — this plan does not have enough verified detail to name it precisely. |
| `medical_treatment` | `affliction_id`, `treatment_id`, `outcome` | **Corrected — no bare `MedicalSystem` class exists in the active Core/Godot path.** `MedicalSystem` per `AGENTS.md` Invariant 5 is explicitly a **Unity-legacy offender** (`Assets/_Game/Medical/MedicalSystem.cs`, 1287 lines, 0 core refs) that this project is migrating *away* from — instrumenting it would add telemetry to code this project's own steering rules mark for deletion, not extension. The Godot-side equivalent is `MedicalHostSession` (`src/UI/MedicalPanel.cs:31`); emit from there or its underlying Core engine instead. |
| `combat_resolved` | `enemy_type`, `outcome`, `damage_taken` | **Corrected — no bare `CombatSystem` class exists.** Real class is `Ashfall.Core...TacticalCombatSystem`, wrapped by `CombatHostSession` (`src/Host/CombatHostSession.cs:17,33`) |
| `panel_opened` | `panel_id`, `open_duration_ms` | UI host (Godot panels, e.g. `MedicalPanel`, `ExpeditionPanel`) |
| `panel_closed` | `panel_id`, `open_duration_ms` | UI host |
| `radiation_threshold` | `level`, `total_dose`, `source` | `Ashfall.Core.Radiation.RadiationSystem` (real class, verified: `Assets/Ashfall.Core/Radiation/RadiationSystem.cs`) |
| `save_performed` | `stores_succeeded`, `stores_failed`, `checksum` | `SaveAll` (`src/Main.cs:6227`) — **cross-reference note:** if Batch 95 (Exception Safety) ships first, `SaveAll` returns a `SaveResult` with `Succeeded`/`Failed` counts that this event's payload should read directly rather than recomputing; if Batch 96 ships first, `SaveAll` is still `void` and this event can only report `stores_succeeded = 29, stores_failed = 0` unconditionally, which is misleading given the payload's own field names imply partial-failure awareness that doesn't exist yet. Sequence these two batches deliberately, or have this event silently report data it can't actually observe yet. |

**This correction reflects a real naming-pattern mismatch in the original plan, not a nitpick:** the original table asserted six systems (`ExpeditionSystem`, `EncounterSystem`, `TradeSystem`, `MedicalSystem`, `CombatSystem`, `RadiationSystem`) as if they were uniform, directly-instrumentable classes. Only two (`ExpeditionSystem`, `RadiationSystem`) exist as named. The other four either don't exist, are split across multiple real classes, or are explicitly legacy code this project is actively migrating away from. Step 5's wiring table below is corrected to match.

Design principles:
- **No PII:** No username, no hardware ID, no file paths, no IP. Session ID is a random 16-char hex string.
- **Flat payload:** All values are strings to avoid type ambiguity across analysis tools.
- **Extensible:** New event types added by registering a new string; no enum needed.
- **snake_case:** Consistent with project data authority conventions.

### Verification
- `TelemetryEvent` round-trips through `IJsonSerializer` without loss
- Payload dictionary handles arbitrary key-value pairs
- No PII fields exist in the schema (code review + test assertion)
- Event types follow `snake_case` convention

### Done-when
- `TelemetryEvent.cs` exists in `Assets/Ashfall.Core/Telemetry/`
- Event type registry documented in code comments (15 canonical types), with the emitter column corrected to real class names per the table above (or explicitly marked "needs investigation" for `trade_completed`, which this review could not resolve to one verified class)
- Round-trip serialization test passes
- Schema has zero PII fields (verified by naming convention + review)

---

## Step 2 — Implement TelemetryCollector in Core

### Goal
Create the in-memory event buffer that collects telemetry events during a session and flushes them to disk on demand or at session end.

### Port interface mismatch — verified against `Assets/Ashfall.Core/Ports.cs`

**The real `IFileIO` interface has only 5 methods:**
```csharp
public interface IFileIO
{
    bool DirectoryExists(string path);
    bool FileExists(string path);
    string ReadAllText(string path);
    void WriteAllText(string path, string contents);
    string Combine(params string[] parts);
}
```

It has **no** `EnsureDirectoryExists`, `AppendAllText`, `DeleteDirectoryContents`, `GetFiles`, or `GetFileSize`. The original plan's `TelemetryCollector` and (in Step 6) `TelemetryExporter` call all five of these nonexistent methods. This is not a minor naming mismatch — it means the code as originally written does not compile against the real port. Also, `InMemoryFileIO` (used throughout the plan's proposed test suite) does not exist anywhere in the codebase and would need to be written from scratch as test infrastructure, which the plan never calls out as a deliverable.

Two ways to close this gap (pick one during implementation, this plan defaults to option A for minimal footprint):

**Option A — extend `IFileIO` with the missing methods.** Adds `EnsureDirectoryExists`, `AppendAllText`, `DeleteDirectoryContents`, `GetFiles`, `GetFileSize` to the shared port interface and implements them in whatever the Godot host's concrete `IFileIO` adapter class is (must be located and updated — not identified by this plan; locate it before starting Step 2). This is a broader change than "add a telemetry system" — it modifies a port every other save/load system in the game depends on, and needs its own regression pass (existing `IFileIO` consumers must be unaffected).

**Option B — give telemetry its own narrower file-access port** (e.g. `ITelemetryFileIO` with only the 5 extra methods telemetry actually needs), implemented once for the Godot host and left out of the shared `IFileIO` entirely. Smaller blast radius, doesn't touch the port every save store depends on, but introduces a second file-access abstraction living next to the first.

**This plan's Done-when criteria below assume Option A but flag Option B as the lower-risk alternative** — whichever is chosen, the original code samples calling directory/append/delete/enumerate methods on `IFileIO` as if they already existed must be corrected first, and the concrete Godot adapter class implementing `IFileIO` must be located and extended (or a new port implemented) as part of this step, not assumed to already exist.

### Implementation

`Assets/Ashfall.Core/Telemetry/TelemetryCollector.cs`:

```csharp
namespace Ashfall.Core.Telemetry;

public sealed class TelemetryCollector
{
    private readonly List<TelemetryEvent> _buffer = new();
    private readonly IJsonSerializer _json;
    private readonly IFileIO _fileIO;
    private readonly IClock _clock;
    private readonly ILog _log;
    private readonly object _lock = new();

    private string _sessionId;
    private DateTime _sessionStart;
    private bool _enabled;

    public bool IsEnabled => _enabled;
    public int BufferedEventCount => _buffer.Count;
    public string SessionId => _sessionId;

    public const int MaxBufferSize = 10_000; // flush if buffer exceeds this
    public const string TelemetryDirectory = "telemetry";
    public const string FileExtension = ".jsonl";

    public TelemetryCollector(IJsonSerializer json, IFileIO fileIO, IClock clock, ILog log)
    {
        _json = json;
        _fileIO = fileIO;
        _clock = clock;
        _log = log;
    }

    public void Enable()
    {
        _enabled = true;
        _sessionId = GenerateSessionId();
        _sessionStart = DateTime.UtcNow;
        _log.Info("[Telemetry] Enabled. Session: " + _sessionId);
    }

    public void Disable()
    {
        _enabled = false;
        _log.Info("[Telemetry] Disabled.");
    }

    public void Record(string eventType, Dictionary<string, string> payload = null)
    {
        if (!_enabled) return;

        var evt = new TelemetryEvent
        {
            EventType = eventType,
            Timestamp = DateTime.UtcNow.ToString("O"),
            Day = _clock.CurrentDay,
            SessionElapsedSeconds = (DateTime.UtcNow - _sessionStart).TotalSeconds,
            SessionId = _sessionId,
            Payload = payload ?? new Dictionary<string, string>()
        };

        lock (_lock)
        {
            _buffer.Add(evt);
            if (_buffer.Count >= MaxBufferSize)
            {
                FlushInternal();
            }
        }
    }

    public void Flush()
    {
        lock (_lock)
        {
            FlushInternal();
        }
    }

    private void FlushInternal()
    {
        if (_buffer.Count == 0) return;

        var fileName = $"session_{_sessionId}_{_sessionStart:yyyyMMdd_HHmmss}{FileExtension}";
        var filePath = Path.Combine(TelemetryDirectory, fileName);

        _fileIO.EnsureDirectoryExists(TelemetryDirectory);

        // Append mode: each flush appends to the session file
        var lines = _buffer.Select(evt => _json.Serialize(evt));
        var content = string.Join("\n", lines) + "\n";
        _fileIO.AppendAllText(filePath, content);

        _log.Info($"[Telemetry] Flushed {_buffer.Count} events to {filePath}");
        _buffer.Clear();
    }

    public void ClearAllData()
    {
        lock (_lock)
        {
            _buffer.Clear();
        }
        _fileIO.DeleteDirectoryContents(TelemetryDirectory);
        _log.Info("[Telemetry] All telemetry data cleared.");
    }

    private static string GenerateSessionId()
    {
        // 16 hex chars from cryptographic random — no PII
        var bytes = new byte[8];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
    }
}
```

Design decisions:
- **In-memory buffer:** Events accumulate in RAM. Flush on session end, on buffer overflow (10k events), or on explicit request.
- **JSON Lines format:** One JSON object per line — simple, streamable, tool-compatible.
- **Append mode:** Multiple flushes within one session append to the same file.
- **Thread-safe:** Lock around buffer operations (Godot main thread + potential async events).
- **No-op when disabled:** `Record()` exits immediately when telemetry is off. Zero overhead.
- **Session ID from CSPRNG:** Not `ISeededRng` (that's for gameplay determinism). Telemetry session IDs are random and ephemeral.

### Verification
- `Record()` when disabled → buffer stays empty
- `Record()` when enabled → buffer grows
- `Flush()` writes JSON Lines to expected file path
- `ClearAllData()` removes telemetry directory contents
- Buffer overflow triggers automatic flush at 10k events
- Session ID is 16 hex characters, different each time

### Done-when
- `IFileIO` (or a new narrower telemetry-specific port, per the Option A/B decision above) exposes the directory/append/delete operations `TelemetryCollector` actually needs — verified by confirming the Godot host's concrete adapter class compiles with the new methods implemented, not just the interface declaration
- `TelemetryCollector.cs` exists in `Assets/Ashfall.Core/Telemetry/`
- A test double (`InMemoryFileIO` or equivalent) implementing the chosen port is written as new test infrastructure — it does not exist in the codebase today and must be added, not assumed available
- 8+ unit tests covering enable/disable, record, flush, overflow, clear
- No engine dependencies (`System.Security.Cryptography` is netstandard2.1)
- Flush produces valid JSON Lines (one JSON object per line, parseable)

---

## Step 3 — Add Consent Management

### Goal
Implement a consent system that respects player choice: telemetry is OFF by default, requires explicit opt-in, and can be revoked at any time with data deletion.

### Note on the file I/O port

`TelemetryConsent.Save()` below calls `_fileIO.EnsureDirectoryExists("telemetry")`, which has the same `IFileIO` gap flagged in Step 2 — resolve that port decision (Option A or B) once, and both `TelemetryCollector` and `TelemetryConsent` use the same resolution.

### Implementation

`Assets/Ashfall.Core/Telemetry/TelemetryConsent.cs`:

```csharp
namespace Ashfall.Core.Telemetry;

public enum ConsentStatus
{
    NotAsked,    // First run — haven't shown prompt yet
    Granted,     // Player said yes
    Denied,      // Player said no
    Revoked      // Player previously granted, then revoked
}

public sealed class TelemetryConsent
{
    private readonly IFileIO _fileIO;
    private readonly IJsonSerializer _json;
    private readonly ILog _log;

    private const string ConsentFile = "telemetry/consent.json";

    public ConsentStatus Status { get; private set; } = ConsentStatus.NotAsked;
    public string ConsentTimestamp { get; private set; }

    public TelemetryConsent(IFileIO fileIO, IJsonSerializer json, ILog log)
    {
        _fileIO = fileIO;
        _json = json;
        _log = log;
        Load();
    }

    public bool IsConsented => Status == ConsentStatus.Granted;
    public bool NeedsPrompt => Status == ConsentStatus.NotAsked;

    public void Grant()
    {
        Status = ConsentStatus.Granted;
        ConsentTimestamp = DateTime.UtcNow.ToString("O");
        Save();
        _log.Info("[TelemetryConsent] Consent granted.");
    }

    public void Deny()
    {
        Status = ConsentStatus.Denied;
        ConsentTimestamp = DateTime.UtcNow.ToString("O");
        Save();
        _log.Info("[TelemetryConsent] Consent denied.");
    }

    public void Revoke(TelemetryCollector collector)
    {
        Status = ConsentStatus.Revoked;
        ConsentTimestamp = DateTime.UtcNow.ToString("O");
        collector.Disable();
        collector.ClearAllData();
        Save();
        _log.Info("[TelemetryConsent] Consent revoked. All data cleared.");
    }

    private void Load()
    {
        if (!_fileIO.FileExists(ConsentFile)) return;
        var text = _fileIO.ReadAllText(ConsentFile);
        var data = _json.Deserialize<ConsentData>(text);
        Status = Enum.TryParse<ConsentStatus>(data.Status, out var s) ? s : ConsentStatus.NotAsked;
        ConsentTimestamp = data.Timestamp;
    }

    private void Save()
    {
        _fileIO.EnsureDirectoryExists("telemetry");
        var data = new ConsentData { Status = Status.ToString(), Timestamp = ConsentTimestamp };
        _fileIO.WriteAllText(ConsentFile, _json.Serialize(data));
    }

    [Serializable]
    private sealed class ConsentData
    {
        public string Status { get; set; }
        public string Timestamp { get; set; }
    }
}
```

UI flow (Godot host):
1. On first launch, `TelemetryConsent.NeedsPrompt` is true
2. Show consent dialog (Step 3 UI below) — **model the dialog's structure after the existing `src/UI/OpeningProtocolModal.cs`** (wired in `src/Main.cs:977-994`), which is the only existing Godot host modal with a comparable "present choice(s), fire an event on selection, close" shape (`OnClose`, `OnRationPolicySelected`, `OnMaintenanceDirectiveSelected` events). A consent dialog is not gameplay content, but the event-wiring pattern (`_modal.OnXxxSelected += ...` set up once during host init) is directly reusable and should not be reinvented.
3. Player chooses "Yes, help improve the game" or "No thanks"
4. Choice persisted to `telemetry/consent.json`
5. Settings menu always shows a toggle + "Delete my data" button — **reuse the existing `src/UI/SettingsPanel.cs` row pattern** (`MakeSettingRow(...)` + `OptionButton`/toggle wiring, as used for e.g. Window Mode at `SettingsPanel.cs:143-150`) rather than building a new settings-row abstraction from scratch.

Consent dialog text (player-facing):
> **Help Improve ASHFALL**
>
> We can collect anonymous gameplay data during your session to help balance the game. This data is stored locally on your computer and never sent anywhere automatically.
>
> What's collected: play time, survival days, which systems you interact with, and how your session ends. No personal information is ever recorded.
>
> You can disable this and delete all data at any time from Settings.

### Verification
- Fresh install: `Status == NotAsked`, `NeedsPrompt == true`
- After `Grant()`: `Status == Granted`, `IsConsented == true`, file persisted
- After `Deny()`: `Status == Denied`, `IsConsented == false`
- After `Revoke()`: collector disabled, all data cleared, status persisted
- Consent survives app restart (loaded from file)
- No telemetry events recorded when `Status != Granted`

### Done-when
- `TelemetryConsent.cs` exists in `Assets/Ashfall.Core/Telemetry/`
- Consent dialog `.tscn` + `.cs` exist in `src/UI/Telemetry/`, structured after `OpeningProtocolModal`'s event pattern (`OnGranted`/`OnDenied` events, not a blocking modal call) — vague "exists" replaced with: dialog shows exactly once per install until a choice is made, dismissing without choosing (e.g. Escape key, per the existing `_UnhandledInput` Escape-closes-overlay convention at `src/Main.cs`) does NOT count as either Grant or Deny and re-prompts next launch
- Settings toggle added to `src/UI/SettingsPanel.cs` using the existing `MakeSettingRow` pattern, wired to `Grant()`/`Revoke()`
- "Delete my data" button calls `ClearAllData()` and the settings panel confirms deletion (row updates to reflect `BufferedEventCount == 0` / no session files) rather than just firing-and-forgetting the call
- 6+ unit tests covering all consent transitions
- No telemetry is ever recorded without explicit consent — enforced by a test that constructs a `TelemetryCollector` wired to a `TelemetryConsent` in `Denied`/`NotAsked` state and asserts `Record()` is a no-op regardless of caller intent (i.e. consent gating is enforced at the collector/consent boundary, not left to each of the 15 call sites in Step 5 to check individually)

---

## Step 4 — Define Core Telemetry Events (15 High-Value Events)

### Goal
Establish the canonical set of telemetry events that provide maximum insight with minimum instrumentation. Focus on events that answer real design questions.

### Implementation

`Assets/Ashfall.Core/Telemetry/TelemetryEvents.cs` — static helper for consistent event construction:

```csharp
namespace Ashfall.Core.Telemetry;

/// <summary>
/// Factory methods for canonical telemetry events.
/// Ensures consistent event_type strings and payload keys across all emitters.
/// </summary>
public static class TelemetryEvents
{
    public static void SessionStart(TelemetryCollector c, long seed, string scenarioId, string buildVersion)
    {
        c.Record("session_start", new Dictionary<string, string>
        {
            ["seed"] = seed.ToString(),
            ["scenario_id"] = scenarioId ?? "",
            ["build_version"] = buildVersion
        });
    }

    public static void SessionEnd(TelemetryCollector c, int totalDays, double totalSeconds, string endReason)
    {
        c.Record("session_end", new Dictionary<string, string>
        {
            ["total_days"] = totalDays.ToString(),
            ["total_seconds"] = totalSeconds.ToString("F1"),
            ["end_reason"] = endReason
        });
    }

    public static void DayAdvanced(TelemetryCollector c, int dayNumber, int survivorCount, int totalItems)
    {
        c.Record("day_advanced", new Dictionary<string, string>
        {
            ["day_number"] = dayNumber.ToString(),
            ["survivor_count"] = survivorCount.ToString(),
            ["total_items"] = totalItems.ToString()
        });
    }

    public static void SurvivorDied(TelemetryCollector c, string survivorId, string cause, int day, int ageDays)
    {
        c.Record("survivor_died", new Dictionary<string, string>
        {
            ["survivor_id"] = survivorId,
            ["cause"] = cause,
            ["day"] = day.ToString(),
            ["age_days"] = ageDays.ToString()
        });
    }

    public static void ItemCrafted(TelemetryCollector c, string recipeId, string itemId, int quantity)
    {
        c.Record("item_crafted", new Dictionary<string, string>
        {
            ["recipe_id"] = recipeId,
            ["item_id"] = itemId,
            ["quantity"] = quantity.ToString()
        });
    }

    public static void ExpeditionStarted(TelemetryCollector c, string expeditionId, string destination, int partySize)
    {
        c.Record("expedition_started", new Dictionary<string, string>
        {
            ["expedition_id"] = expeditionId,
            ["destination"] = destination,
            ["party_size"] = partySize.ToString()
        });
    }

    public static void ExpeditionCompleted(TelemetryCollector c, string expeditionId, string outcome, int lootCount)
    {
        c.Record("expedition_completed", new Dictionary<string, string>
        {
            ["expedition_id"] = expeditionId,
            ["outcome"] = outcome,
            ["loot_count"] = lootCount.ToString()
        });
    }

    public static void EncounterChoice(TelemetryCollector c, string encounterId, int choiceIndex, string choiceId)
    {
        c.Record("encounter_choice", new Dictionary<string, string>
        {
            ["encounter_id"] = encounterId,
            ["choice_index"] = choiceIndex.ToString(),
            ["choice_id"] = choiceId
        });
    }

    public static void TradeCompleted(TelemetryCollector c, string partnerId, int itemsGiven, int itemsReceived)
    {
        c.Record("trade_completed", new Dictionary<string, string>
        {
            ["partner_id"] = partnerId,
            ["items_given"] = itemsGiven.ToString(),
            ["items_received"] = itemsReceived.ToString()
        });
    }

    public static void MedicalTreatment(TelemetryCollector c, string afflictionId, string treatmentId, string outcome)
    {
        c.Record("medical_treatment", new Dictionary<string, string>
        {
            ["affliction_id"] = afflictionId,
            ["treatment_id"] = treatmentId,
            ["outcome"] = outcome
        });
    }

    public static void CombatResolved(TelemetryCollector c, string enemyType, string outcome, int damageTaken)
    {
        c.Record("combat_resolved", new Dictionary<string, string>
        {
            ["enemy_type"] = enemyType,
            ["outcome"] = outcome,
            ["damage_taken"] = damageTaken.ToString()
        });
    }

    public static void PanelOpened(TelemetryCollector c, string panelId)
    {
        c.Record("panel_opened", new Dictionary<string, string>
        {
            ["panel_id"] = panelId
        });
    }

    public static void PanelClosed(TelemetryCollector c, string panelId, long openDurationMs)
    {
        c.Record("panel_closed", new Dictionary<string, string>
        {
            ["panel_id"] = panelId,
            ["open_duration_ms"] = openDurationMs.ToString()
        });
    }

    public static void RadiationThreshold(TelemetryCollector c, string level, float totalDose, string source)
    {
        c.Record("radiation_threshold", new Dictionary<string, string>
        {
            ["level"] = level,
            ["total_dose"] = totalDose.ToString("F2"),
            ["source"] = source
        });
    }

    public static void SavePerformed(TelemetryCollector c, int storesSucceeded, int storesFailed, string checksum)
    {
        c.Record("save_performed", new Dictionary<string, string>
        {
            ["stores_succeeded"] = storesSucceeded.ToString(),
            ["stores_failed"] = storesFailed.ToString(),
            ["checksum"] = checksum ?? ""
        });
    }
}
```

Design questions each event answers:

| Event | Design Question |
|---|---|
| `session_start` / `session_end` | How long do players play? When do they quit? |
| `day_advanced` | How far do players get? Where is the difficulty wall? |
| `survivor_died` | What kills survivors? Is a specific cause too dominant? |
| `item_crafted` | Which recipes are popular? Which are never used? |
| `expedition_started/completed` | Are expeditions rewarding enough? Too risky? |
| `encounter_choice` | Which encounter options do players prefer? |
| `trade_completed` | Is the economy functional? Are trades balanced? |
| `medical_treatment` | Which afflictions are treated? Which are ignored (too expensive)? |
| `combat_resolved` | Is combat too deadly? Too trivial? |
| `panel_opened/closed` | Which UI panels are used? Which are dead weight? |
| `radiation_threshold` | How often do players hit dangerous rad levels? |
| `save_performed` | Are partial saves occurring? (Links to Batch 95 resilience) |

### Verification
- All 15 event factory methods produce valid `TelemetryEvent` instances
- Event types are consistent snake_case strings
- Payload values are all strings (no type ambiguity)
- No PII in any payload (no survivor names, no file paths)
- Events are no-ops when collector is disabled

### Done-when
- `TelemetryEvents.cs` exists with 15 static factory methods
- Each method documents which design question it answers
- Unit test verifies all 15 methods produce events with correct `EventType`
- Payload keys are consistent and documented

---

## Step 5 — Wire 15 High-Value Events into Core Systems

### Goal
Instrument existing Core systems to emit telemetry events at the right moments. Each instrumentation point is a single method call — minimal intrusion into existing logic.

### Implementation

Wiring strategy: inject `TelemetryCollector` (or null-object when disabled) into each system that needs to emit events.

**Corrected wiring table** — the original table named `CombatSystem`, `EncounterSystem`, `TradeSystem`, `MedicalSystem` as if they were single instrumentable classes. As documented in Step 1's corrected event table, none of those four exist as named; see Step 1 for full detail. Corrected mapping:

| System (real, verified) | Event | Emission Point |
|---|---|---|
| Host startup (`src/Main.cs`) | `session_start` | After initialization complete |
| Host shutdown (`src/Main.cs`) | `session_end` | Before flush/exit |
| `TickSimDay` (`src/Main.cs:1643`) | `day_advanced` | After all 23 steps complete (see Batch 95 if guards land first — `AnyDegraded` state should probably be reflected in this event's payload, e.g. an extra `degraded_count` key, though that's a cross-batch decision this plan alone can't make) |
| `Ashfall.Core.Survivors.NeedsSystem` | `survivor_died` | Inside `EvaluateDeath(SurvivorNeedsState survivor)` (`NeedsSystem.cs:193-197`), when `survivor.Health <= 0f && !survivor.IsDead` — NOT a generic `Tick()` loop as the original sample implied; the real method takes a single `SurvivorNeedsState`, not a collection |
| `TacticalCombatSystem` / `CombatHostSession` | `survivor_died` | After a combat resolution that kills a survivor — exact hook needs to be located inside `TacticalCombatSystem`, not assumed; `CombatHostSession` (`src/Host/CombatHostSession.cs`) is presentation-only per its own doc comment ("No gameplay rules live here") so the emission point should be in Core, not the host session |
| `CraftingSystem` (`Assets/Ashfall.Core/Crafting/CraftingSystem.cs`) | `item_crafted` | After successful craft |
| `ExpeditionSystem` (`Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs`) | `expedition_started` | When expedition launches |
| `ExpeditionSystem` | `expedition_completed` | When expedition returns |
| `NarrativeEncounterSystem` **and** `DoorEncounterSystem` (two separate systems) | `encounter_choice` | When player selects an option — needs two emission points, one per system, not one |
| Trade path — **exact class TBD during implementation**, likely `HoldfastTradeSession` or an `EconomySystem`-family class; not verified by this review | `trade_completed` | After trade finalized |
| Medical path — **do not instrument the legacy `Assets/_Game/Medical/MedicalSystem.cs`** (marked for migration-away per `AGENTS.md` Invariant 5); instrument `MedicalHostSession` (`src/UI/MedicalPanel.cs:31`) or its underlying Core engine instead | `medical_treatment` | After treatment applied |
| `TacticalCombatSystem` | `combat_resolved` | After combat round resolves |
| `Ashfall.Core.Radiation.RadiationSystem` (`Assets/Ashfall.Core/Radiation/RadiationSystem.cs`) | `radiation_threshold` | When dose crosses threshold |
| UI host (Godot panels) | `panel_opened/closed` | Panel show/hide signals |
| `SaveAll` (`src/Main.cs:6227`) | `save_performed` | After save completes |

Instrumentation pattern (minimal intrusion) — corrected to match the real `NeedsSystem` method signature:

```csharp
// In NeedsSystem.cs — inside the existing EvaluateDeath method (NeedsSystem.cs:193-197),
// NOT a rewritten Tick() loop. The real method already receives one SurvivorNeedsState;
// the original plan's sample invented a foreach-loop shape that doesn't match this file.
private void EvaluateDeath(SurvivorNeedsState survivor)
{
    if (survivor.Health <= 0f && !survivor.IsDead)
    {
        if (TryDeferDeath != null && TryDeferDeath(survivor)) return;
        // ... existing death-processing logic ...

        // NEW: single telemetry call, added at the end of the existing branch
        TelemetryEvents.SurvivorDied(_telemetry, survivor.Id, "needs_exhaustion",
            _clock.CurrentDay, survivor.DaysSurvived);
    }
}
```

Injection approach:
- Systems that already accept dependencies via constructor get `TelemetryCollector` added
- `TelemetryCollector` is null-safe: if `null` is passed, `Record()` is never called (guard in factory methods)
- Alternatively, use a `NullTelemetryCollector` (no-op implementation) for systems that don't opt in

### Verification
- Each instrumented system still passes all existing tests (telemetry is additive)
- With telemetry enabled: events appear in buffer after triggering condition
- With telemetry disabled: zero overhead, no events recorded
- No new constructor parameters break existing test setups (use optional parameter or overload)

### Done-when
- Emission points wired across all real systems identified in the corrected table above; count is no longer a clean "15 across 10+" since `encounter_choice` needs two emission points (`NarrativeEncounterSystem` + `DoorEncounterSystem`) and `trade_completed`'s exact class must be resolved during implementation (not assumed to be a single `TradeSystem`)
- Existing test suite passes unchanged (no regressions)
- Integration test: play one simulated day → verify `day_advanced` event in buffer
- Integration test: simulate survivor death via `NeedsSystem.EvaluateDeath` → verify `survivor_died` event (not via a nonexistent bare `Tick()` loop)
- No engine dependencies introduced into Core systems
- Do not add telemetry calls into `Assets/_Game/Medical/MedicalSystem.cs` or any other file under `Assets/_Game/` — per this project's `AGENTS.md`, that tree is legacy/read-only and gameplay logic changes (including telemetry instrumentation) belong in `Ashfall.Core`/`src/`, not there

---

## Step 6 — Create Telemetry Export Format

### Goal
Define and implement the export format that playtesters use to share their telemetry data with developers. Must be simple, portable, and importable into common analysis tools.

### Same port gap as Step 2

`TelemetryExporter.ListSessions()` below calls `_fileIO.GetFiles(...)` and `_fileIO.GetFileSize(...)`, neither of which exist on the real `IFileIO` (see Step 2's port-mismatch note). This must be resolved via the same Option A/B decision made in Step 2 — do not resolve it independently per-step, or the codebase ends up with two different extended file-access surfaces.

### Implementation

Export format: **JSON Lines** (`.jsonl`) — one JSON object per line.

```
{"event_type":"session_start","timestamp":"2025-01-15T10:00:00Z","day":1,"session_elapsed_seconds":0.0,"session_id":"a1b2c3d4e5f6g7h8","payload":{"seed":"48291053","scenario_id":"","build_version":"0.9.1"}}
{"event_type":"day_advanced","timestamp":"2025-01-15T10:00:05Z","day":1,"session_elapsed_seconds":5.2,"session_id":"a1b2c3d4e5f6g7h8","payload":{"day_number":"1","survivor_count":"3","total_items":"42"}}
{"event_type":"survivor_died","timestamp":"2025-01-15T10:02:30Z","day":12,"session_elapsed_seconds":150.8,"session_id":"a1b2c3d4e5f6g7h8","payload":{"survivor_id":"survivor_elena","cause":"radiation_poisoning","day":"12","age_days":"12"}}
```

Export utilities in `Assets/Ashfall.Core/Telemetry/TelemetryExporter.cs`:

```csharp
namespace Ashfall.Core.Telemetry;

public sealed class TelemetryExporter
{
    private readonly IFileIO _fileIO;
    private readonly IJsonSerializer _json;
    private readonly ILog _log;

    public TelemetryExporter(IFileIO fileIO, IJsonSerializer json, ILog log)
    {
        _fileIO = fileIO;
        _json = json;
        _log = log;
    }

    /// <summary>
    /// Lists all session files available for export.
    /// </summary>
    public IReadOnlyList<TelemetrySessionInfo> ListSessions()
    {
        if (!_fileIO.DirectoryExists(TelemetryCollector.TelemetryDirectory))
            return Array.Empty<TelemetrySessionInfo>();

        var files = _fileIO.GetFiles(TelemetryCollector.TelemetryDirectory, "*.jsonl");
        return files.Select(f => new TelemetrySessionInfo
        {
            FilePath = f,
            FileName = Path.GetFileName(f),
            FileSize = _fileIO.GetFileSize(f),
            EventCount = CountLines(f)
        }).ToList();
    }

    /// <summary>
    /// Exports all sessions to a single consolidated file for sharing.
    /// </summary>
    public string ExportAll(string outputPath)
    {
        var sessions = ListSessions();
        if (sessions.Count == 0)
        {
            _log.Warn("[TelemetryExporter] No sessions to export.");
            return null;
        }

        var allContent = new StringBuilder();
        allContent.AppendLine($"# ASHFALL Telemetry Export — {sessions.Count} sessions");
        allContent.AppendLine($"# Exported: {DateTime.UtcNow:O}");
        allContent.AppendLine();

        foreach (var session in sessions)
        {
            var content = _fileIO.ReadAllText(session.FilePath);
            allContent.Append(content);
        }

        _fileIO.WriteAllText(outputPath, allContent.ToString());
        _log.Info($"[TelemetryExporter] Exported {sessions.Count} sessions to {outputPath}");
        return outputPath;
    }

    /// <summary>
    /// Exports a single session file (copy to specified destination).
    /// </summary>
    public string ExportSession(string sessionFile, string outputPath)
    {
        var content = _fileIO.ReadAllText(sessionFile);
        _fileIO.WriteAllText(outputPath, content);
        return outputPath;
    }

    /// <summary>
    /// Produces a summary report from all sessions (aggregate stats).
    /// </summary>
    public TelemetrySummary GenerateSummary()
    {
        var sessions = ListSessions();
        var summary = new TelemetrySummary
        {
            TotalSessions = sessions.Count,
            TotalEvents = sessions.Sum(s => s.EventCount),
            TotalSizeBytes = sessions.Sum(s => s.FileSize),
            OldestSession = sessions.MinBy(s => s.FileName)?.FileName,
            NewestSession = sessions.MaxBy(s => s.FileName)?.FileName
        };
        return summary;
    }

    private int CountLines(string filePath)
    {
        var content = _fileIO.ReadAllText(filePath);
        return content.Split('\n', StringSplitOptions.RemoveEmptyEntries).Length;
    }
}

[Serializable]
public sealed class TelemetrySessionInfo
{
    public string FilePath { get; set; }
    public string FileName { get; set; }
    public long FileSize { get; set; }
    public int EventCount { get; set; }
}

[Serializable]
public sealed class TelemetrySummary
{
    public int TotalSessions { get; set; }
    public int TotalEvents { get; set; }
    public long TotalSizeBytes { get; set; }
    public string OldestSession { get; set; }
    public string NewestSession { get; set; }
}
```

Compatibility with analysis tools:
- **Python/pandas:** `pd.read_json("export.jsonl", lines=True)` — immediate DataFrame
- **jq:** `cat export.jsonl | jq 'select(.event_type == "survivor_died")'`
- **Excel/Sheets:** Import as JSON, or convert to CSV with `jq -r '[.event_type, .day, .payload.cause] | @csv'`
- **Custom dashboard:** Read line-by-line, parse JSON, aggregate

### Verification
- `ListSessions()` correctly discovers all `.jsonl` files in telemetry directory
- `ExportAll()` consolidates multiple session files into one
- Export file is valid JSON Lines (each line parseable independently) — **this contradicts the `ExportAll()` sample above, which prepends `#`-prefixed comment header lines (`# ASHFALL Telemetry Export — N sessions`) directly into the exported file.** A `#`-prefixed line is not valid JSON; `pd.read_json(path, lines=True)` (cited two paragraphs above as a compatibility target) will throw a `JSONDecodeError` on it, not silently skip it as the original plan claimed. Either strip the header before shipping the file players actually share, write it to a separate sidecar file/README, or verify empirically that the specific `pandas`/`jq` versions used in practice do skip `#`-prefixed lines (they generally do not for `pd.read_json`). This is a real bug in the sample as written, not a hypothetical.
- `GenerateSummary()` produces accurate counts

### Done-when
- `TelemetryExporter.cs` exists in `Assets/Ashfall.Core/Telemetry/`
- `TelemetrySessionInfo` and `TelemetrySummary` DTOs defined
- Export produces valid JSON Lines importable by pandas and jq — verified with an actual `pd.read_json(..., lines=True)` call against a generated export file (not just asserted), given the comment-header bug flagged above
- 5+ tests covering list, export-all, export-single, summary, empty-state
- No engine dependencies
- Same `IFileIO` port resolution as Step 2 (Option A or B) — not solved independently here

---

## Step 7 — Write Telemetry Tests

### Goal
Comprehensive test coverage ensuring telemetry collection respects consent, produces valid output, handles edge cases, and has zero overhead when disabled.

### Implementation

`Ashfall.Core.Tests/TelemetryTests.cs`:

```csharp
namespace Ashfall.Core.Tests;

public class TelemetryCollectorTests
{
    [Fact]
    public void WhenDisabled_RecordIsNoOp()
    {
        var collector = CreateCollector(enabled: false);
        collector.Record("test_event", new Dictionary<string, string> { ["key"] = "value" });
        Assert.Equal(0, collector.BufferedEventCount);
    }

    [Fact]
    public void WhenEnabled_RecordAddsToBuffer()
    {
        var collector = CreateCollector(enabled: true);
        collector.Record("test_event");
        Assert.Equal(1, collector.BufferedEventCount);
    }

    [Fact]
    public void Flush_WritesJsonLinesToDisk()
    {
        var fileIO = new InMemoryFileIO();
        var collector = CreateCollector(enabled: true, fileIO: fileIO);

        collector.Record("event_a");
        collector.Record("event_b");
        collector.Flush();

        Assert.Equal(0, collector.BufferedEventCount);
        var files = fileIO.GetFiles("telemetry", "*.jsonl");
        Assert.Single(files);

        var content = fileIO.ReadAllText(files[0]);
        var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        Assert.Equal(2, lines.Length);
    }

    [Fact]
    public void Flush_ProducesValidJson_PerLine()
    {
        var fileIO = new InMemoryFileIO();
        var collector = CreateCollector(enabled: true, fileIO: fileIO);

        collector.Record("survivor_died", new Dictionary<string, string>
        {
            ["survivor_id"] = "survivor_elena",
            ["cause"] = "radiation"
        });
        collector.Flush();

        var content = fileIO.ReadAllText(fileIO.GetFiles("telemetry", "*.jsonl")[0]);
        var line = content.Split('\n', StringSplitOptions.RemoveEmptyEntries)[0];
        var parsed = JsonSerializer.Deserialize<TelemetryEvent>(line);
        Assert.Equal("survivor_died", parsed.EventType);
        Assert.Equal("survivor_elena", parsed.Payload["survivor_id"]);
    }

    [Fact]
    public void BufferOverflow_AutoFlushes()
    {
        var fileIO = new InMemoryFileIO();
        var collector = CreateCollectorWithMaxBuffer(5, enabled: true, fileIO: fileIO);

        for (int i = 0; i < 5; i++)
            collector.Record($"event_{i}");

        // Buffer should have auto-flushed at capacity
        Assert.Equal(0, collector.BufferedEventCount);
        Assert.Single(fileIO.GetFiles("telemetry", "*.jsonl"));
    }

    [Fact]
    public void ClearAllData_RemovesEverything()
    {
        var fileIO = new InMemoryFileIO();
        var collector = CreateCollector(enabled: true, fileIO: fileIO);

        collector.Record("event_a");
        collector.Flush();
        collector.ClearAllData();

        Assert.Equal(0, collector.BufferedEventCount);
        Assert.Empty(fileIO.GetFiles("telemetry", "*.jsonl"));
    }

    [Fact]
    public void SessionId_IsDifferentEachEnable()
    {
        var collector = CreateCollector(enabled: false);
        collector.Enable();
        var id1 = collector.SessionId;
        collector.Disable();
        collector.Enable();
        var id2 = collector.SessionId;
        Assert.NotEqual(id1, id2);
    }

    [Fact]
    public void SessionId_Is16HexChars()
    {
        var collector = CreateCollector(enabled: true);
        Assert.Matches("^[0-9a-f]{16}$", collector.SessionId);
    }
}

public class TelemetryConsentTests
{
    [Fact]
    public void FreshInstall_StatusIsNotAsked()
    {
        var consent = CreateConsent(existingFile: false);
        Assert.Equal(ConsentStatus.NotAsked, consent.Status);
        Assert.True(consent.NeedsPrompt);
    }

    [Fact]
    public void AfterGrant_StatusIsGranted()
    {
        var consent = CreateConsent(existingFile: false);
        consent.Grant();
        Assert.Equal(ConsentStatus.Granted, consent.Status);
        Assert.True(consent.IsConsented);
    }

    [Fact]
    public void AfterDeny_StatusIsDenied()
    {
        var consent = CreateConsent(existingFile: false);
        consent.Deny();
        Assert.Equal(ConsentStatus.Denied, consent.Status);
        Assert.False(consent.IsConsented);
    }

    [Fact]
    public void AfterRevoke_CollectorDisabledAndDataCleared()
    {
        var collector = CreateCollector(enabled: true);
        collector.Record("test");
        var consent = CreateConsent(existingFile: false);
        consent.Grant();
        consent.Revoke(collector);

        Assert.Equal(ConsentStatus.Revoked, consent.Status);
        Assert.False(collector.IsEnabled);
        Assert.Equal(0, collector.BufferedEventCount);
    }

    [Fact]
    public void ConsentPersistsAcrossRestarts()
    {
        var fileIO = new InMemoryFileIO();
        var consent1 = CreateConsent(fileIO: fileIO);
        consent1.Grant();

        // Simulate restart: new instance, same file system
        var consent2 = CreateConsent(fileIO: fileIO);
        Assert.Equal(ConsentStatus.Granted, consent2.Status);
    }
}

public class TelemetryEventsFactoryTests
{
    [Theory]
    [InlineData("session_start")]
    [InlineData("session_end")]
    [InlineData("day_advanced")]
    [InlineData("survivor_died")]
    [InlineData("item_crafted")]
    [InlineData("expedition_started")]
    [InlineData("expedition_completed")]
    [InlineData("encounter_choice")]
    [InlineData("trade_completed")]
    [InlineData("medical_treatment")]
    [InlineData("combat_resolved")]
    [InlineData("panel_opened")]
    [InlineData("panel_closed")]
    [InlineData("radiation_threshold")]
    [InlineData("save_performed")]
    public void EventFactory_ProducesCorrectEventType(string expectedType)
    {
        var collector = CreateCollector(enabled: true);
        InvokeFactory(collector, expectedType);
        Assert.Equal(1, collector.BufferedEventCount);
        // Verify the buffered event has the correct type
    }

    [Fact]
    public void NoEventContainsPII()
    {
        // Verify none of the 15 event factories include PII-like keys
        var forbiddenKeys = new[] { "username", "email", "ip", "hardware_id", "name", "address" };
        // ... verify all factory payloads
    }
}

public class TelemetryExporterTests
{
    [Fact]
    public void ListSessions_ReturnsAllJsonlFiles() { /* ... */ }

    [Fact]
    public void ExportAll_ConsolidatesMultipleSessions() { /* ... */ }

    [Fact]
    public void ExportAll_EmptyDirectory_ReturnsNull() { /* ... */ }

    [Fact]
    public void GenerateSummary_AccurateCounts() { /* ... */ }

    [Fact]
    public void ExportedFile_IsValidJsonLines() { /* ... */ }
}
```

### Verification
- `dotnet test --filter "TelemetryCollectorTests|TelemetryConsentTests|TelemetryEventsFactoryTests|TelemetryExporterTests"` — all pass
- Tests verify: consent flow, enable/disable, recording, flushing, export, PII absence
- All tests use a test double for `IFileIO` (in-memory, no real disk I/O) — **this test double does not exist yet** and is new infrastructure this step (or Step 2, wherever it's introduced first) must write, not something already available to import
- Zero engine dependencies in tests
- The sample test code in this step has two gaps that must be resolved, not left as-is:
  1. `EventFactory_ProducesCorrectEventType` calls an undefined `InvokeFactory(collector, expectedType)` helper and ends with a comment (`// Verify the buffered event has the correct type`) instead of an actual assertion. As written, this test would compile-fail and, even if a dispatch helper were added, asserts nothing about the buffered event's `EventType` field — it needs a real `Assert.Equal(expectedType, ...)` against the buffered event, not just a count check.
  2. All five `TelemetryExporterTests` methods are empty stubs (`{ /* ... */ }`). These are placeholders, not tests — "5+ tests covering list, export-all, export-single, summary, empty-state" in Step 6's Done-when is not satisfied by shipping this file as-is.

### Done-when
- `TelemetryTests.cs` contains 20+ *implemented, assertion-bearing* tests across 4 test classes — not stubs; the sample in this plan is a skeleton, not a deliverable
- All tests pass on `dotnet test`
- Coverage includes: consent lifecycle, collector enable/disable/record/flush/clear, event factories, exporter
- PII audit test confirms no personal data fields
- No flaky tests (deterministic, no timing, no real I/O)

---

## Summary Table

| Step | Deliverable | Location | Tests | Risk |
|------|-------------|----------|-------|------|
| 1 | `TelemetryEvent` schema | `Assets/Ashfall.Core/Telemetry/` | Round-trip serialization | None |
| 2 | `TelemetryCollector` (buffer + flush) + `IFileIO` extension (or new port) | `Assets/Ashfall.Core/Telemetry/` + `Assets/Ashfall.Core/Ports.cs` | 8+ collector tests | Low-Medium — extending `IFileIO` touches a port every save/load system depends on; see Option A/B in Step 2 |
| 3 | `TelemetryConsent` + consent UI | `Assets/Ashfall.Core/Telemetry/` + `src/UI/` | 6+ consent tests | None |
| 4 | `TelemetryEvents` static factories (15 events) | `Assets/Ashfall.Core/Telemetry/` | Factory + PII tests | None |
| 5 | Wire events into real Core/host systems (not a clean "15 across 10+" — see Step 5's corrected table) | Various Core systems, several requiring investigation to locate the correct real class | Integration tests | Low — additive only, but two emission points (`trade_completed`, legacy `MedicalSystem`) need class identification before they can be wired at all |
| 6 | `TelemetryExporter` + JSON Lines format | `Assets/Ashfall.Core/Telemetry/` | 5+ export tests (stubs in this plan — must be implemented, not shipped empty) | Low — but fix the `#`-comment-header/JSON-Lines-validity bug noted in Step 6 before calling this done |
| 7 | Comprehensive telemetry test suite | `Ashfall.Core.Tests/TelemetryTests.cs` | 20+ tests (skeleton in this plan has unimplemented assertions — see Step 7 note) | None |

---

## Dependencies & Constraints

- **No engine coupling:** All telemetry classes live in `Assets/Ashfall.Core/Telemetry/` with zero `Godot.*` or `UnityEngine.*` references.
- **Uses existing ports — corrected:** `IJsonSerializer`, `IClock`, `ILog` are usable as-is. `IFileIO` is **not** usable as-is — it has only 5 methods (`DirectoryExists`, `FileExists`, `ReadAllText`, `WriteAllText`, `Combine`) and needs either extension (Option A) or a narrower sibling port (Option B) before `TelemetryCollector`/`TelemetryExporter` compile. This is a real, non-trivial dependency the original plan missed by asserting "no new port interfaces needed."
- **Privacy by design:** No PII. No network. No cloud. No hardware fingerprinting. Session IDs are ephemeral random tokens.
- **Consent-first — implementation gap noted:** Telemetry is designed to be OFF by default and revocable, but as sketched in Steps 2-3, `TelemetryCollector.Record()` only checks its own internal `_enabled` flag — there is no shown wiring making `TelemetryConsent.Status` the single source of truth `Record()` consults. `Revoke()` calls `collector.Disable()`, which is one correct wiring path, but `Grant()`/`Deny()` never call `collector.Enable()`/`Disable()` in the sketched code. Whoever wires the host must ensure `TelemetryCollector.Enable()` is only ever called as a direct consequence of `TelemetryConsent.Grant()` succeeding — this is a design gap in the plan's code samples, not just a documentation gap.
- **Zero overhead when disabled:** `Record()` exits on first instruction when `_enabled == false`. No allocation, no locking.
- **Data authority:** Telemetry output files live in `telemetry/` (gitignored — confirm this directory is actually added to `.gitignore` as part of this work; the plan assumes it without adding it).
- **Format compatibility:** JSON Lines is importable by pandas, jq, Excel, custom tools — no proprietary format, **except for the `#`-comment-header bug in `ExportAll()` flagged in Step 6**, which breaks the pandas compatibility claim as currently sketched.
- **Determinism unaffected:** Telemetry uses `System.Security.Cryptography.RandomNumberGenerator` for session IDs (not `ISeededRng`). Game determinism is preserved.

## Privacy Checklist

**Corrected framing:** the table below described these as already "Enforced," but nothing is enforced yet — no telemetry code exists in this codebase (verified by grep, see Motivation). These are design intentions to be validated once Steps 1-7 are implemented and tested, not current guarantees. Re-labeled accordingly:

| Requirement | Design intent (verify once implemented — not yet enforced) |
|---|---|
| No player name or username | No field exists in the `TelemetryEvent` schema (Step 1) |
| No hardware ID or fingerprint | Session ID is random (Step 2), not device-derived |
| No IP address or network info | No network calls anywhere in the design (Steps 2, 6 — local file I/O only) |
| No file paths from player's system | Paths are relative/internal only (`telemetry/session_*.jsonl`) |
| Opt-in consent required | `ConsentStatus.NotAsked` → must prompt (Step 3) — **contingent on closing the `Record()`/`Consent` wiring gap noted above** |
| Data deletable by player | `ClearAllData()` + UI button (Step 3) — contingent on the `IFileIO`/`DeleteDirectoryContents` port gap being resolved (Step 2) |
| No automatic transmission | No network code anywhere in this design; manual file sharing only |
| Consent persisted and respected | `consent.json` survives restart (Step 3) — contingent on the same port gap |
| Session ID regenerated each launch | New random token on each `Enable()` call (Step 2) |

## Exit Criteria

All 5 verification steps pass:
```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # PASS
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj     # PASS (including TelemetryTests)
3. dotnet build Ashfall.csproj                                  # PASS (0 errors, 0 warnings)
4. godot --headless --path . -- --data-integrity-selftest       # PASS
5. godot --headless --path . -- --bridge-selftest               # PASS
```

These 5 commands verify the codebase still builds and existing self-tests still pass — they do **not** verify telemetry-specific behavior (consent gating, JSON Lines validity, PII absence, export correctness). That verification is covered by the `dotnet test` run in step 2 picking up `TelemetryTests.cs`, provided the stub tests flagged in Step 7 have actually been implemented by the time this is run.


## Review Notes (Corrected)

This file was adversarially reviewed against the real codebase at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War` and corrected in place. Summary of what was wrong and what was fixed:

### Factual claims verified as accurate

1. **Zero telemetry/analytics infrastructure exists.** A codebase-wide grep for `telemetry`/`analytics`/`Telemetry`/`Analytics` found no actual systems, classes, save stores, or event emitters. The only hits are cosmetic Tailwind/HTML design-mockup tokens (`assets/ui/HtmlBundles/*.html`, e.g. a font style literally named `"telemetry-bold"`) and in-fiction narrative flavor text (`Assets/StreamingAssets/Data/narrative/*.json` — radio "ghost transmission" content describing a fictional satellite's telemetry as story content, not a real system). The plan's premise is correct.
2. **No first-run consent UI pattern exists.** A grep for `consent`/`FirstRun`/`opt-in`/`privacy` found nothing resembling a consent flow. The closest structural analog is `src/UI/OpeningProtocolModal.cs` (a Day-1 gameplay modal with a similar event-wiring shape) and `src/UI/SettingsPanel.cs` (has a reusable settings-row pattern). Neither was referenced in the original plan; both are now cited as the patterns to reuse rather than inventing new UI scaffolding from scratch.

### Factual errors found and corrected

3. **`IFileIO` does not have the methods the plan's code requires.** The real port (`Assets/Ashfall.Core/Ports.cs`) has exactly 5 methods: `DirectoryExists`, `FileExists`, `ReadAllText`, `WriteAllText`, `Combine`. The plan's `TelemetryCollector`, `TelemetryConsent`, and `TelemetryExporter` call `EnsureDirectoryExists`, `AppendAllText`, `DeleteDirectoryContents`, `GetFiles`, and `GetFileSize` — none of which exist. As originally written, none of this plan's core classes compile against the real port. This is the single most consequential factual error in the file, since it affects Steps 2, 3, and 6 simultaneously. Corrected by adding an explicit "Option A (extend `IFileIO`) vs. Option B (add a narrower sibling port)" decision to Step 2, flagged as a shared dependency for Steps 3 and 6, and called out in the Summary Table's risk column (this is the only non-trivial risk in an otherwise low-risk plan).
4. **`InMemoryFileIO` does not exist.** The plan's proposed test suite (Step 7, and the test samples in Steps 2/3) constructs `new InMemoryFileIO()` throughout, as if it were existing test infrastructure. It is not — grepped for and confirmed absent. This needs to be written as new test infrastructure, which the plan never called out as its own deliverable. Flagged in Steps 2 and 7's Done-when.
5. **Six of the plan's named "emitter systems" don't match real classes.** The original event-wiring table (Steps 1 and 5) named `ExpeditionSystem`, `EncounterSystem`, `TradeSystem`, `MedicalSystem`, `CombatSystem`, `RadiationSystem` as if they were six uniform, directly-instrumentable classes. Verified against the real codebase:
   - `ExpeditionSystem` and `RadiationSystem` are real, confirmed classes (`Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs`, `Assets/Ashfall.Core/Radiation/RadiationSystem.cs`).
   - `EncounterSystem` does not exist as one class — there are two separate encounter systems (`NarrativeEncounterSystem` and `DoorEncounterSystem`), meaning `encounter_choice` needs two emission points, not one.
   - `TradeSystem` does not exist under that name; the real trade path is split across `HoldfastTradeSession` and economy-family classes referenced elsewhere in this project's own steering docs (`EconomySystem.GetTrust(factionId)`). This review could not resolve the exact emission point with confidence and flags it as needing investigation during implementation, rather than guessing a second time.
   - `MedicalSystem` **does exist, but only as a Unity-legacy class explicitly marked for migration-away** in this project's `AGENTS.md` (Invariant 5: "Known offenders — do not grow these"). Instrumenting it would add new code to a file this project's own rules mark for deletion. Corrected to point at `MedicalHostSession`/its underlying Core engine instead.
   - `CombatSystem` does not exist as one bare class; the real class is `TacticalCombatSystem`, wrapped for host use by `CombatHostSession` (which explicitly documents itself as presentation-only, "no gameplay rules live here" — so the actual emission point belongs in `TacticalCombatSystem`, not the host session).
   - The Godot-host naming convention this project actually uses is `XxxHostSession` (`CombatHostSession`, `MedicalHostSession`, `ExpeditionHostSession`, etc.) wrapping a Core engine class — a pattern the original plan's tables never matched.
6. **`NeedsSystem`'s real method shape doesn't match the plan's instrumentation sample.** The plan showed a `public void Tick() { foreach (var survivor in _survivors) { ... if (survivor.Health <= 0) ... } }` shape. The real method is `Tick(float gameHours)` for the batch tick and a separate `EvaluateDeath(SurvivorNeedsState survivor)` (`NeedsSystem.cs:193-197`) that actually contains the `Health <= 0f` check. Corrected the instrumentation sample to hook into the real method.
7. **A genuine bug in the plan's own `TelemetryExporter` sample:** `ExportAll()` prepends `#`-prefixed comment header lines directly into the exported `.jsonl` file, then the plan's own Verification section claims the file is "valid JSON Lines (each line parseable independently)" and that tools "skip" comment lines. They generally do not — `pd.read_json(path, lines=True)`, the plan's own cited compatibility target, will throw on a non-JSON line. This is a real correctness bug in the sample code, not a hypothetical; flagged with a fix requirement (strip the header, or move it to a sidecar file, or verify the specific parser actually tolerates it).
8. **"Uses existing ports: ... no new port interfaces needed" was false**, given finding #3 above. Corrected in Dependencies & Constraints.
9. **The Privacy Checklist asserted requirements were "Enforced"** when zero telemetry code exists yet — nothing can be enforced by code that hasn't been written. Re-labeled as design intentions to verify post-implementation, and cross-referenced against a real wiring gap in the plan's own sketched code: `TelemetryCollector.Record()` only consults its own internal `_enabled` flag; the sample code never shows `Grant()`/`Deny()` calling `collector.Enable()`/`Disable()`, only `Revoke()` does. Consent enforcement is not automatically wired just because both classes exist — an implementer following the plan's code samples literally could ship a collector that's enabled independently of consent state.

### Vague Done-when criteria tightened

- Step 3's "Consent dialog `.tscn` exists" had no acceptance detail (what happens on dismiss-without-choosing, whether it re-prompts) — tightened with explicit re-prompt behavior and delegated "delete my data" verification (must reflect actual empty state, not just fire-and-forget).
- Step 5's "15 emission points wired across 10+ systems" was falsified by finding #5 above (some events need 2 emission points, one emission point's target class is unresolved) — corrected to describe the actual, uneven mapping.
- Step 7's sample tests included at least one non-functional test (`EventFactory_ProducesCorrectEventType` ends in a comment instead of an assertion and calls an undefined `InvokeFactory` helper) and five fully-empty stub methods in `TelemetryExporterTests`. Flagged explicitly: shipping the sample as-is does not satisfy "5+ tests" or "20+ tests" — those are unimplemented skeletons, not deliverables.

### Scope / sequencing note (not present in the original)

Batch 96's `save_performed` event payload (`stores_succeeded`, `stores_failed`) only makes sense once `SaveAll` can actually report partial-failure state — which is exactly what Batch 95 (Exception Safety) introduces via `SaveResult`. If Batch 96 ships before Batch 95, this one event can only ever report `stores_succeeded = 29, stores_failed = 0` unconditionally, which contradicts what its own payload field names promise. This cross-batch dependency was not identified in either original plan and is now called out in Step 1's event table.
