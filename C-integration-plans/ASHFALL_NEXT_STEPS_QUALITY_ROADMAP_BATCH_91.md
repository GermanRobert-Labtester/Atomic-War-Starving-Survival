# ASHFALL — Quality Roadmap Batch 91

## Theme: Notification & Alert System — Unified Player Attention Management

**Priority:** MEDIUM-HIGH (many systems generate typed C# events but no unified notification pipeline)
**Risk:** Low-Medium — additive UI/UX layer for Steps 1-2 and 5-7; Step 3 registers a new `notif_` ID prefix in the shared `CatalogIntegrityValidator`, and Step 4 adds constructor parameters to 10 existing systems, which is a real (if small) modification to existing code, not purely additive
**Batch:** 91
**Depends on:** None (additive layer)
**Unlocks:** Tutorial hints system, achievement popups, radio broadcast alerts, expedition live updates

---

## Problem Statement

The game has many systems producing player-relevant events: critical hunger, radiation dose-threshold crossings, expedition completion, crafting completion, disease outbreaks, trade caravan arrivals, quest progress, survivor death, morale crises, power shortfalls. Currently each system's events are handled ad-hoc by the Godot host (`Main.cs` dirty flags triggering panel refreshes). There is no unified notification queue, no priority system, no toast/banner display, no notification history, and no "while you were away" summary. Players can miss critical events because there is no attention management layer — a survivor death notification is as invisible as a completed crafting job.

**Corrected scope note (see Review Notes below):** the original draft cited "82+ systems" and named several classes (`DiseaseOutbreakSystem`, `MoraleSystem`, `ShelterPowerSystem`) that do not exist under those names in `Assets/Ashfall.Core/`. The real source systems and their actual event surfaces are enumerated in Step 4 and in Review Notes. This does not change the design of the notification service itself, only the wiring targets in Step 4.

---

## Step 1 — Design INotificationService in Core

**Goal:** Define the engine-agnostic notification contract with priority levels, category tags, deduplication keys, and TTL metadata so all 82+ systems can emit notifications through a single interface.

**Implementation:**

- Create `Assets/Ashfall.Core/Notifications/INotificationService.cs`
- Define `NotificationPriority` enum: `Critical`, `Warning`, `Info`, `Flavor`
- Define `NotificationCategory` enum: `Survival`, `Expedition`, `Economy`, `Social`, `Medical`, `Combat`, `Shelter`, `Narrative`, `System`
- Define `NotificationEntry` struct:
  - `string Id` (unique, generated from `ISeededRng` or deterministic hash)
  - `NotificationPriority Priority`
  - `NotificationCategory Category`
  - `string TemplateKey` (maps to display text/icon in catalog)
  - `Dictionary<string, string> Params` (template interpolation)
  - `string DeduplicationKey` (optional — same key within TTL window deduplicates)
  - `int TtlSeconds` (auto-dismiss timer; 0 = persistent until acknowledged)
  - `int IssuedOnDay` (from `IClock`)
  - `string SourceSystemId` (originating system identifier)
- Define `INotificationService` interface:
  - `void Push(NotificationEntry entry)`
  - `IReadOnlyList<NotificationEntry> PeekActive()`
  - `void Acknowledge(string notificationId)`
  - `void AcknowledgeAll()`
  - `IReadOnlyList<NotificationEntry> GetHistory(int maxCount)`
  - `event Action<NotificationEntry> OnNotificationPushed`
  - `event Action<string> OnNotificationAcknowledged`

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # compiles cleanly
```

**Done when:** `INotificationService.cs` and supporting types compile with zero engine references, follow `Ashfall.Core.Notifications` namespace, use no `UnityEngine.*` or `Godot.*`.

---

## Step 2 — Implement NotificationQueue (Bounded, Priority-Sorted)

**Goal:** Build the concrete notification queue with bounded capacity, priority-based sorting, TTL expiry, deduplication, and overflow eviction so the system never grows unbounded and critical notifications always surface.

**Implementation:**

- Create `Assets/Ashfall.Core/Notifications/NotificationQueue.cs`
- Constructor accepts: `int maxActive` (default 8), `int maxHistory` (default 200), `IClock clock`
- Internal state:
  - `List<NotificationEntry> _active` — sorted by priority descending, then issue time ascending
  - `Queue<NotificationEntry> _history` — ring buffer of dismissed/expired notifications
  - `HashSet<string> _deduplicationWindow` — active dedup keys
- `Push` logic:
  1. Check deduplication — if `DeduplicationKey` is non-null and already in window, discard
  2. If queue full, evict lowest-priority oldest entry (move to history)
  3. Insert at correct sorted position
  4. Add dedup key to window
  5. Fire `OnNotificationPushed`
- `Tick(int currentDay)` method:
  - Expire entries whose `IssuedOnDay + TtlSeconds/86400` < currentDay (day-granularity TTL)
  - Move expired to history
  - Remove their dedup keys
- `Acknowledge(id)` — remove from active, add to history, fire event
- Implements `CaptureState()` / `RestoreState()` for save/load (both active and history are serializable)

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~Notification"
```

**Done when:** `NotificationQueue` handles push, eviction, dedup, TTL expiry, and acknowledge, verified by the specific tests named in Step 7 (`Push_QueueFull_EvictsLowestPriority`, `Push_DuplicateKey_WithinWindow_Discards`, `Tick_ExpiredTtl_MovesToHistory`, `Acknowledge_RemovesFromActive_AddsToHistory`) plus a save/load round-trip test, all passing under `dotnet test`. "All operations are O(n) or better" is not independently verified by this batch and should be removed as a done-when criterion unless a benchmark test is added — vague performance claims are not testable by the listed verification command.

---

## Step 3 — Define Notification Catalog (Event-to-Template Mapping)

**Goal:** Create a data-driven notification catalog in the JSON data authority that maps system events to notification templates with display metadata (icon key, color category, text template, sound cue key).

**Implementation:**

- Create `Assets/StreamingAssets/Data/notifications.json`:
  ```json
  {
    "schema_version": 1,
    "notifications": [
      {
        "template_key": "notif_survivor_died",
        "priority": "critical",
        "category": "survival",
        "title_template": "{survivor_name} has died",
        "body_template": "Cause: {cause_of_death}. Day {day}.",
        "icon_key": "icon_death",
        "color_key": "color_critical",
        "sound_cue": "sfx_alert_critical",
        "ttl_seconds": 0,
        "dedup_key_pattern": "death_{survivor_name}"
      }
    ]
  }
  ```
- Define 20+ templates covering the highest-priority events across all categories
- Create `Assets/Ashfall.Core/Notifications/NotificationCatalog.cs`:
  - Loads from JSON via `IFileIO` + `IJsonSerializer`
  - `NotificationEntry Build(string templateKey, Dictionary<string,string> params, int currentDay)`
  - Validates template keys against known prefixes (`notif_`)
- Register `notif_` prefix in `CatalogIntegrityValidator` known prefixes

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
godot --headless --path . -- --data-integrity-selftest   # must report 0 errors; confirms notifications.json and the new notif_ prefix validate cleanly
```

**Done when:** `notifications.json` passes `--data-integrity-selftest` with 0 errors (not just "validates cleanly" — the command must be run and its exit/output checked). `NotificationCatalog.Build` returns a populated `NotificationEntry` for every `template_key` defined in the JSON file, verified by a test that iterates the catalog. The `notif_` prefix is added to `CatalogIntegrityValidator.IdPrefixes` (`Assets/Ashfall.Core/CatalogIntegrityValidator.cs:75-81`) — this is an edit to a shared validation file used by every other data category, so the diff for this step must be reviewed for accidental changes to unrelated prefix entries.

---

## Step 4 — Wire 10 Highest-Priority System Events to Notifications

**Goal:** Connect the 10 most critical game events to the notification service so players never miss life-threatening or mission-critical information.

**Implementation:**

Wire the following events (in priority order). Class and event names below were verified against `Assets/Ashfall.Core/` — see Review Notes for the corrections made to the original draft (which referenced three non-existent class names: `DiseaseOutbreakSystem`, `MoraleSystem`, `ShelterPowerSystem`):

1. **Survivor death** — `NeedsSystem.OnDied` (`Assets/Ashfall.Core/Survivors/NeedsSystem.cs`) → `notif_survivor_died` (Critical)
2. **Critical hunger/thirst** — `NeedsSystem.OnNeedCritical` (same file) → `notif_need_critical` (Critical)
3. **Radiation dose threshold crossed** — `RadiationSystem.OnStatusGained` (`Assets/Ashfall.Core/Radiation/RadiationSystem.cs`), filtered to statuses at/above the acute threshold — **not** a dedicated "spike" event; the host must derive the threshold crossing itself since `RadiationSystem` only exposes `OnDoseChanged` (fires every tick) and `OnStatusGained`/`OnStatusLost` (fires on status transitions) → `notif_radiation_spike` (Critical)
4. **Disease outbreak** — `DiseaseSystem.OnOutbreakDeclared` (`Assets/Ashfall.Core/Disease/DiseaseSystem.cs`) — outbreak logic lives inside `DiseaseSystem`, there is no separate `DiseaseOutbreakSystem` class → `notif_disease_outbreak` (Warning)
5. **Expedition completed** — `ExpeditionSystem.OnExpeditionCompleted` (`Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs`) → `notif_expedition_returned` (Info)
6. **Shelter integrity alert** — needs a named source system; no `ShelterSystem` class with a degradation-threshold event was found during verification. Before implementing this line item, confirm the actual source (candidates in `Assets/Ashfall.Core/Shelter/`, e.g. `PowerGridSystem` state or a shielding/air-filtration degradation system) and update this row with the real class/event name → `notif_shelter_alert` (Warning)
7. **Trade caravan arrived** — `TravelingCaravanSystem.OnCaravanArrivedAtNode` (`Assets/Ashfall.Core/TravelingCaravanSystem.cs`) — not `DynamicEconomySystem`, which is a separate pricing/trade-attitude system with no caravan-arrival event → `notif_caravan_arrived` (Info)
8. **Morale crisis** — verified class is `MoraleMarkSystem` (`Assets/Ashfall.Core/DutyRoster/MoraleMarkSystem.cs`), not `MoraleSystem`. Confirm `MoraleMarkSystem` exposes a critical-threshold event (or add one) before wiring → `notif_morale_crisis` (Warning)
9. **Power shortfall** — verified class is `PowerGridSystem` (`Assets/Ashfall.Core/Shelter/PowerGridSystem.cs`), which the class doc-comment says already raises "a typed `PowerGridEvent` that the host listens to" — confirm this event covers the insufficient-power case, don't assume it's named `OnBrownout` → `notif_power_brownout` (Warning)
10. **Quest stage complete** — verified event is `CrossingQuestSystem.OnQuestStageChanged` (`Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs`) for the Crossing expansion; if a generic cross-expansion `QuestSystem` is intended instead, name the specific class before implementation — do not wire against an assumed generic type → `notif_quest_progress` (Info)

- Each system calls `INotificationService.Push(catalog.Build(templateKey, params, clock.Day))`
- Systems receive `INotificationService` via constructor injection (Ports pattern)
- No system depends on notification service for correctness — push is fire-and-forget
- Items 6 and 8 are **not shovel-ready** — the exact source event must be confirmed (or added) before wiring. Do not mark this step done with placeholder/guessed event names.

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
```

**Done when:** all 10 systems listed above emit notifications through the unified service, using their *verified* real class and event names (not placeholders). Each wiring has a unit test that raises the real source event and asserts `INotificationService.Push` was called with the expected `templateKey`. If items 6 or 8 required adding a new event to an existing system, that addition is called out explicitly in the PR description since it modifies a file outside `Notifications/`.

---

## Step 5 — Create Notification Toast UI in Godot Host

**Goal:** Build a slide-in/out toast panel in the Godot host that displays active notifications with auto-dismiss behavior, click-to-open-relevant-panel interaction, and visual priority differentiation.

**Implementation:**

- Create `src/UI/NotificationToastPanel.cs` (Godot Node, `AtomicWar.GodotApp.UI` namespace)
- Layout:
  - Anchored top-right, vertical stack (max 4 visible toasts)
  - Each toast: icon (left), title + body (center), dismiss button (right), category color bar (left edge)
  - Priority coloring: Critical = red pulse, Warning = amber, Info = neutral, Flavor = muted
- Animation:
  - Slide in from right (0.3s ease-out)
  - Auto-dismiss: fade out after TTL (0.5s fade)
  - Manual dismiss: click X or swipe right
- Interaction:
  - Click toast body → open the panel most relevant to that notification category (e.g., Medical panel for disease notifications)
  - Panel mapping defined in a dictionary: `NotificationCategory → string panelId`
- Subscribe to `INotificationService.OnNotificationPushed` event
- Queue display if >4 arrive simultaneously (show next when one dismisses)
- Accessibility: screen-reader text for each toast, keyboard navigation support

**Verification:**
```
dotnet build Ashfall.csproj   # Godot host compiles
godot --headless --path . -- --bridge-selftest   # exits 0
```

**Done when:** the toast panel is instantiated in a running scene and manually (or via a headless UI test, if the project has a pattern for one) confirmed to display a pushed notification with correct priority coloring, auto-dismiss on TTL expiry, and a working click-to-open-panel interaction for at least one category. `--bridge-selftest` exiting 0 only confirms the build didn't crash on startup — it does not exercise the toast UI at all, since it "prints the removal notice and exits 0 rather than booting into the app loop" (per AGENTS.md). Do not treat a passing `--bridge-selftest` as evidence the toast panel works; it is only evidence the build compiles and the binary starts.

---

## Step 6 — Add Notification History Panel

**Goal:** Build a scrollable notification history panel accessible from the main HUD so players can review past notifications, filter by category, and understand what happened while they were focused elsewhere.

**Implementation:**

- Create `src/UI/NotificationHistoryPanel.cs` (Godot Node)
- Layout:
  - Full-width overlay panel (slides from bottom or opens as tab)
  - Header: title "Notification Log" + filter buttons (one per category + "All")
  - Body: scrollable list of past notifications (newest first)
  - Each entry: timestamp (day), icon, title, body, category badge
  - Footer: "Clear History" button, "Mark All Read" button
- Filtering:
  - Category filter buttons toggle visibility
  - Active filter state persists across panel open/close (not across sessions)
- Data source: `INotificationService.GetHistory(maxCount)` with optional category filter
- "While you were away" mode:
  - On session load, if notifications accumulated during offline (from save timestamp to current), show a summary banner: "X critical, Y warnings occurred since your last session"
  - Uses `NotificationQueue.RestoreState()` history entries with `IssuedOnDay > lastSessionDay`
- HUD integration: notification bell icon in top bar with unread count badge

**Verification:**
```
dotnet build Ashfall.csproj
godot --headless --path . -- --bridge-selftest
```

**Done when:** the history panel is verified the same way as Step 5 — actual display behavior confirmed, not just a successful `--bridge-selftest` exit. History panel displays past notifications, supports category filtering, shows an unread-count badge on the HUD, and surfaces a "while you were away" summary on session resume. The "while you were away" day-window comparison (`IssuedOnDay > lastSessionDay`) requires `lastSessionDay` to be captured at save time and restored at load — confirm this field exists on the relevant save DTO or add it as part of this step; do not assume it's already available.

---

## Step 7 — Write Notification Tests (Behavior + Integration)

**Goal:** Comprehensive test coverage ensuring the notification system behaves correctly under all conditions: priority ordering, deduplication, TTL expiry, queue overflow, save/load round-trip, and catalog validation.

**Implementation:**

- Create `Ashfall.Core.Tests/NotificationQueueTests.cs`:
  - `Push_Critical_AlwaysAboveWarning` — priority ordering
  - `Push_DuplicateKey_WithinWindow_Discards` — deduplication
  - `Tick_ExpiredTtl_MovesToHistory` — TTL expiry
  - `Push_QueueFull_EvictsLowestPriority` — overflow eviction
  - `Push_QueueFull_EvictsOldestAtSamePriority` — tie-breaking
  - `Acknowledge_RemovesFromActive_AddsToHistory` — acknowledgment
  - `AcknowledgeAll_ClearsActive` — bulk clear
  - `SaveLoadRoundTrip_PreservesActiveAndHistory` — persistence
  - `SaveLoadRoundTrip_Checksum_ChangesOnMutation` — integrity
- Create `Ashfall.Core.Tests/NotificationCatalogTests.cs`:
  - `Build_ValidTemplate_ReturnsPopulatedEntry` — template building
  - `Build_UnknownTemplate_Throws` — invalid key handling
  - `Build_MissingParam_LeavesPlaceholder` — graceful degradation
  - `AllTemplates_HaveValidPriority` — catalog consistency
  - `AllTemplates_HaveValidCategory` — catalog consistency
- Create `Ashfall.Core.Tests/NotificationIntegrationTests.cs`:
  - `NeedsSystem_CriticalHunger_EmitsNotification` — wiring verification
  - `RadiationSystem_DoseSpike_EmitsNotification` — wiring verification
  - `MultipleSystemEvents_SameTick_AllQueued` — concurrent emission

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~Notification"
```

**Done when:** All notification tests pass. Coverage spans: priority ordering, deduplication, TTL, overflow, save/load, catalog validation, and system wiring integration.

---

## Summary Table

| Step | Deliverable | Layer | Key Files | Risk |
|------|-------------|-------|-----------|------|
| 1 | `INotificationService` interface + types | Core | `Assets/Ashfall.Core/Notifications/INotificationService.cs` | None |
| 2 | `NotificationQueue` implementation | Core | `Assets/Ashfall.Core/Notifications/NotificationQueue.cs` | None |
| 3 | Notification catalog (JSON + loader) | Core + Data | `Assets/StreamingAssets/Data/notifications.json`, `NotificationCatalog.cs` | Low |
| 4 | Wire 10 critical system events | Core | Various system files (constructor injection) | Low |
| 5 | Toast UI panel | Godot Host | `src/UI/NotificationToastPanel.cs` | Low |
| 6 | History panel + HUD badge | Godot Host | `src/UI/NotificationHistoryPanel.cs` | Low |
| 7 | Test suite (queue + catalog + integration) | Tests | `Ashfall.Core.Tests/Notification*Tests.cs` | None |

---

## Architecture Notes

- **Engine-agnostic:** All notification logic (queue, catalog, dedup, TTL) lives in `Ashfall.Core`. The Godot host only handles visual presentation (toast panel, history panel).
- **Deterministic:** Notification IDs derived from `ISeededRng` or deterministic hash of (templateKey + params + day). No `Guid.NewGuid()`.
- **Save-compatible:** `NotificationQueue` implements `CaptureState/RestoreState`. Active and history entries survive save/load. Checksum covers notification state.
- **Non-invasive:** Systems emit notifications fire-and-forget. If `INotificationService` is null or stubbed, systems function identically. No behavioral coupling.
- **Bounded:** Queue never exceeds `maxActive` (8) + `maxHistory` (200). Memory footprint is constant.

---

## Next Prompt

```
Implement Step 1 of Batch 91: Design INotificationService in Assets/Ashfall.Core/Notifications/
with NotificationPriority, NotificationCategory, NotificationEntry, and the INotificationService
interface. Follow Invariant 1 (zero engine coupling). Verify with dotnet build.
```

---

## Review Notes (Corrected)

This batch was adversarially reviewed against the live codebase at
`/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`. Findings and fixes applied in place:

**1. Factual errors in Step 4's system list (fixed):**
Three class names in the original draft do not exist anywhere in `Assets/Ashfall.Core/`:
- `DiseaseOutbreakSystem` — outbreak logic is part of `DiseaseSystem` (`Assets/Ashfall.Core/Disease/DiseaseSystem.cs`), which raises `OnOutbreakDeclared`. There is no separate outbreak class.
- `MoraleSystem` — the real class is `MoraleMarkSystem` (`Assets/Ashfall.Core/DutyRoster/MoraleMarkSystem.cs`).
- `ShelterPowerSystem` — the real class is `PowerGridSystem` (`Assets/Ashfall.Core/Shelter/PowerGridSystem.cs`).

Additionally, `SurvivorSystem` (cited for survivor death) does not exist as a class; the death event (`OnDied`) is raised by `NeedsSystem` (`Assets/Ashfall.Core/Survivors/NeedsSystem.cs`). `DynamicEconomySystem` (cited for caravan arrival) is a real class but has no caravan-arrival event — caravan arrival is `TravelingCaravanSystem.OnCaravanArrivedAtNode` (`Assets/Ashfall.Core/TravelingCaravanSystem.cs`), a separate system. Step 4 has been rewritten with verified file paths and event names, and flags items 6 (shelter integrity) and 8 (morale crisis) as needing a confirmed source event before implementation rather than a guessed one.

**2. "Radiation spike" does not map to a single event (fixed):**
`RadiationSystem` (`Assets/Ashfall.Core/Radiation/RadiationSystem.cs:157-159`) exposes `OnDoseChanged` (fires every tick, not just on spikes), `OnStatusGained`, and `OnStatusLost` — there is no dedicated "spike" or threshold-crossing event. Wiring item 3 now says explicitly that the host must derive the threshold crossing from `OnStatusGained` rather than assuming a matching event exists.

**3. Event pattern claim is accurate — no fix needed:**
The premise that systems raise typed C# `event Action<...>` delegates (not a generic pub/sub bus) is correct and is explicitly called "established convention" in code comments (e.g. `DiseaseSystem.cs:152`, `SilentFoundrySystem.cs:297`). `INotificationService` fits this convention. Verified across `CrossingQuestSystem`, `DiseaseSystem`, `ExpeditionSystem`, `SilentFoundrySystem`, `ChemicalDependencySystem`, `RespiratoryDegenerationSystem`, `WarlordDoctrineSystem`, `DutyRosterSystem`, `GreenhouseSystem`, `LedgerDebtSystem`, `TravelingCaravanSystem`, and `RadiationPhaseProgression` — all use the same typed-event pattern.

**4. "82+ systems" is an unverified round number (softened):**
No attempt was made to enumerate every system in this review; the number is plausible but unverified and was previously stated as a hard number rather than an estimate. Problem Statement now avoids asserting it as fact.

**5. Risk level corrected:**
Step 3 modifies `CatalogIntegrityValidator.cs` (a file shared by every other data category's ID validation) and Step 4 modifies constructor signatures of 10 existing systems. Neither is "no existing system modification required" as the original Risk line claimed. Risk raised from Low to Low-Medium with the specific reason stated.

**6. Vague/unrunnable Done-when criteria (fixed):**
- Step 2's "O(n) or better" performance claim had no backing test — removed as a done-when criterion.
- Step 5 and Step 6's done-when criteria treated `--bridge-selftest` exiting 0 as evidence the UI works. Per this project's own AGENTS.md, `--bridge-selftest` "prints the removal notice and exits 0 rather than booting into the app loop" — it cannot verify any UI behavior. Both steps now require actual display/interaction verification and note that the selftest only confirms the build starts.
- Step 3's verification command was missing the actual assertion ("must report 0 errors") — added.
- Step 6's "while you were away" feature silently assumes a `lastSessionDay` field exists on a save DTO; flagged as needing confirmation or explicit addition.
