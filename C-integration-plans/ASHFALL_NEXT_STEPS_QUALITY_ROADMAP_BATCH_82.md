# ASHFALL — Quality Roadmap Batch 82

## Theme: Tutorial & Onboarding System — Progressive Disclosure for New Players

**Priority:** MEDIUM (retention — new players face 82+ systems with no guidance)
**Risk:** Medium — additive overlay system with no existing behavior modified, but the panel-visibility portion (Step 5) touches presentation wiring across a large, inconsistently-named set of panels (see Step 5 scope note). Treat Step 5 as the highest-risk step in this batch, not "Low" uniformly across all seven steps.
**Batch:** 82
**Systems affected:** Main.cs (panel visibility), JournalSystem (leveraged for hints), all UI panels (tutorial awareness). **Correction:** `StartingLevelSystem` removed from this list — verified against source, it performs Day-1-only Holdfast simulation (ration/maintenance/radio directives) and has no difficulty or complexity-gating role; see Context below.
**Depends on:** None (standalone, though benefits from Input Rebinding [Batch 81] for tutorial key prompts — note Batch 81 as reviewed is itself only a data-model/UI scaffold that does not yet change actual panel key handling, so "benefits from" should read as "can reference Batch 81's canonical action names once available," not "requires Batch 81's rebinding to be functionally complete")
**Unlocks:** New player retention, difficulty scaling, guided early-game experience, demo/press build

---

## Context

The game has 82+ interconnected systems: needs, radiation, crafting, expeditions, economy, factions, medical, weather, maritime, greenhouse, holdfast trade, year of ash, combat, journal, duty roster, muster, and more. Currently:

- **No tutorial exists.** New players are dropped into full complexity on Day 1.
- **No progressive disclosure.** All panels and systems are available immediately, overwhelming new players with information they can't contextualize.
- **Existing systems to leverage — verified against source, with one correction:**
  - `JournalSystem` (Core, `Assets/Ashfall.Core/Journal/JournalSystem.cs`) — **confirmed accurate.** Entries have `Day`/`Hour` timestamps (`entry.Timestamp = JournalVoice.FormatTimestamp(day, hour)`), a `KnowledgeKey` per entry used for dedup/tagging (`entry.KnowledgeKey = knowledgeKey`), and events (`OnEntryAdded`, `OnNotificationPing`, `OnCodexUnlocked`) that a tutorial system could subscribe to for "write a journal entry on step completion" integration. It is capped at `MaxEntries = 64` (oldest entries evict) — worth noting for Step 3's "journal integration" design, since a long-lived tutorial run must not assume old tutorial-authored entries stay around forever.
  - `StartingLevelSystem` (Core, `Assets/Ashfall.Core/StartingLevel/StartingLevelSystem.cs`) — **factual correction: this system does NOT gate system/panel complexity.** It is a narrative/mechanical simulation scoped specifically to Day 1 in the Holdfast (ration policy, maintenance directives, radio protocol, room inspection, air-filter degradation). It has no concept of "difficulty," no notion of other systems (crafting, expeditions, trade, etc.), and exposes no API a tutorial system could call to "unlock" anything outside its own Day-1 room/directive state. The original claim that it "configures initial difficulty; could gate system complexity" is not supported by the code — there is no difficulty field, no complexity tier, and no cross-system gating hook anywhere in this file. **Remove `StartingLevelSystem` as a tutorial dependency/leverage point.** If Day-1 tutorial steps want to reference Holdfast-specific state (e.g., "inspect the filtration bay"), they can listen to `StartingLevelSystem.OnDirectiveLogged`/`OnStateChanged`, but that is a narrow, single-purpose integration, not a general complexity-gating mechanism as originally implied. The real mechanism for progressive disclosure has to be built from scratch in this batch (`PanelVisibilityManager`, Step 5) — it is new work, not something `StartingLevelSystem` already does.
  - `NarrativeSystem` — delivers story beats on schedule; tutorial triggers could use the same infrastructure. (Not independently verified in this review pass — treat as unconfirmed until Step 1 implementation checks its actual API.)
  - Day counter (`IClock`) — natural trigger for time-based tutorial progression.

Design philosophy:
- Tutorial is *opt-out*, not opt-in (new players get guidance by default, veterans skip).
- Tutorial is *non-modal* — never blocks gameplay, uses overlays and highlights.
- Tutorial progression is *save-aware* — persists across save/load, doesn't repeat completed steps.
- Tutorial is *data-driven* — steps defined in JSON, not hardcoded (enables iteration without recompile).
- Tutorial is *integrated* — uses the journal for permanent reference, not a separate "tutorial log," subject to the `MaxEntries = 64` eviction cap noted above.

---

## Step 1 — Design Tutorial Architecture

**Goal:** Define the `TutorialSystem` in Core with the step interface, progression model, and integration points with existing systems.

**Implementation:**
- Create `Assets/Ashfall.Core/Tutorial/ITutorialStep.cs`:
  ```csharp
  namespace Ashfall.Core.Tutorial;

  public interface ITutorialStep
  {
      string StepId { get; }                    // e.g., "tutorial_needs_basics"
      string StageId { get; }                   // e.g., "stage_day_1"
      int DisplayOrder { get; }
      string Title { get; }                     // "Survival Needs"
      string Body { get; }                      // instructional text
      string TargetPanel { get; }               // panel to highlight (nullable)
      TutorialTrigger TriggerCondition { get; } // when to show
      TutorialCompletion CompletionCondition { get; } // when to dismiss
      bool IsOptional { get; }                  // false = blocks stage progression
  }
  ```
- Create `Assets/Ashfall.Core/Tutorial/TutorialTrigger.cs`:
  ```csharp
  public sealed class TutorialTrigger
  {
      public TriggerType Type { get; set; }     // DayReached, PanelOpened, ActionPerformed, SystemUnlocked
      public string Parameter { get; set; }     // e.g., "3" for day 3, "inventory" for panel
  }

  public enum TriggerType
  {
      DayReached,        // fires when IClock.Day >= parameter
      PanelOpened,       // fires when player opens specified panel
      ActionPerformed,   // fires when player performs action (craft, trade, etc.)
      SystemUnlocked,    // fires when a system becomes active
      ItemAcquired,      // fires when player obtains specific item
      EventOccurred      // fires on specific game event
  }
  ```
- Create `Assets/Ashfall.Core/Tutorial/TutorialCompletion.cs`:
  ```csharp
  public sealed class TutorialCompletion
  {
      public CompletionType Type { get; set; }  // Dismissed, ActionPerformed, TimeElapsed, ConditionMet
      public string Parameter { get; set; }
  }

  public enum CompletionType
  {
      Dismissed,         // player clicks dismiss
      ActionPerformed,   // player does the suggested action
      TimeElapsed,       // auto-dismiss after N seconds
      ConditionMet,      // arbitrary condition (e.g., hunger above 50%)
      PanelClosed        // step completes when highlighted panel is closed
  }
  ```
- Create `Assets/Ashfall.Core/Tutorial/TutorialState.cs` (serializable DTO for save/load):
  ```csharp
  [Serializable]
  public sealed class TutorialState
  {
      public bool TutorialEnabled { get; set; } = true;
      public string CurrentStageId { get; set; } = "stage_day_1";
      public List<string> CompletedStepIds { get; set; } = new();
      public List<string> SkippedStageIds { get; set; } = new();
      public int SchemaVersion { get; set; } = 1;
  }
  ```
- Create `Assets/Ashfall.Core/Tutorial/TutorialSystem.cs`:
  - Implements `CaptureState()` / `RestoreState()` (Invariant: stateful systems must).
  - Loads steps from JSON (data authority: `Assets/StreamingAssets/Data/tutorial/`).
  - Evaluates triggers each tick → queues steps for display.
  - Tracks completion → marks steps done → advances stage when all required steps complete.
  - Exposes `SkipTutorial()`, `SkipStage(stageId)`, `ResetTutorial()`.
- Ensure zero engine coupling — no `Godot.*` or `UnityEngine.*` in any tutorial Core file.

**Verification:**
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` compiles cleanly.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` passes.
- `TutorialSystem` has `CaptureState()`/`RestoreState()` that round-trips correctly.
- No engine references in `Assets/Ashfall.Core/Tutorial/` (grep confirms).
- Architecture document (inline XML docs) explains integration points.

**Done when:** Tutorial architecture is defined in Core, all interfaces/classes compile, `CaptureState`/`RestoreState` exist, and no engine coupling is present.

---

## Step 2 — Define Tutorial Stages and Content

**Goal:** Design the complete tutorial progression from Day 1 through Day 30+, with specific steps for each major system, authored as data-driven JSON.

**Implementation:**
- Create `Assets/StreamingAssets/Data/tutorial/tutorial_stages.json`:
  ```json
  {
    "schema_version": 1,
    "stages": [
      {
        "stage_id": "stage_day_1",
        "display_name": "First Light",
        "unlock_day": 1,
        "description": "Basic survival — keeping your people alive"
      },
      {
        "stage_id": "stage_day_3",
        "display_name": "Getting Established",
        "unlock_day": 3,
        "description": "Crafting and resource management"
      },
      {
        "stage_id": "stage_day_7",
        "display_name": "Beyond the Bunker",
        "unlock_day": 7,
        "description": "Expeditions and the outside world"
      },
      {
        "stage_id": "stage_day_14",
        "display_name": "Trade and Diplomacy",
        "unlock_day": 14,
        "description": "Economy, factions, and social systems"
      },
      {
        "stage_id": "stage_day_30",
        "display_name": "Long-term Survival",
        "unlock_day": 30,
        "description": "Advanced medicine, expansion, endgame systems"
      }
    ]
  }
  ```
- Create `Assets/StreamingAssets/Data/tutorial/tutorial_steps.json` with 20–30 steps:
  - **Stage Day 1 (4–5 steps):** Needs overview (hunger/thirst/fatigue), shelter basics (radiation shielding), time advancement (how days work), first assignment (assign survivor to task), dosimeter reading (understanding radiation).
  - **Stage Day 3 (4–5 steps):** Crafting introduction, recipe discovery, resource gathering, water purification, inventory management.
  - **Stage Day 7 (4–5 steps):** Expedition planning, hazard zones, gear preparation (gas mask/hazmat), map navigation, expedition return + loot.
  - **Stage Day 14 (4–5 steps):** Trade basics, faction reputation, dynamic pricing, barter vs currency, trade stances.
  - **Stage Day 30 (4–5 steps):** Medical system, affliction pipeline, long-term radiation effects, expansion systems, endgame goals.
- Each step has: `step_id`, `stage_id`, `display_order`, `title`, `body` (≤150 words), `target_panel`, `trigger`, `completion`, `is_optional`, `journal_tag` (for journal integration).
- **Ordering dependency (added on review):** `target_panel` values here and `TargetPanel` in Step 4's overlay-pointer logic both depend on the real panel-id enumeration that Step 5 performs. As drafted, Step 2 (content) precedes Step 5 (panel visibility, where the real ids get enumerated), so authoring `target_panel` values in Step 2 risks using the same invented ids (`needs_panel`, `trade_panel`, etc.) that Step 5's scope note found to be wrong. **Do the `Main.cs` `OpenPlayerPanel` id enumeration first** (pull it forward as a prerequisite sub-step before Step 2's content authoring, even though it's formally written up under Step 5), so Step 2's JSON uses real ids from the start instead of being rewritten later.
- Body text follows tone rules: cold, exhausted, human, restrained. No exclamation marks, no cheerfulness.
- All IDs use `snake_case` per project convention.
- `schema_version: 1` on all JSON files.

**Verification:**
- `godot --headless --path . -- --data-integrity-selftest` passes (new JSON validates).
- `CatalogIntegrityValidator` accepts tutorial IDs — **confirmed via source read of `Assets/Ashfall.Core/CatalogIntegrityValidator.cs`: `tutorial_` is not currently in the `IdPrefixes` array** (verified list includes `item_`, `loc_`, `quest_`, `npc_`, `stage_`, `step_`, etc., but no `tutorial_`). This is a required change, not a conditional one — add `"tutorial_"` to `IdPrefixes` in this step; do not defer it or gate it behind "if needed." Note `stage_` and `step_` prefixes already exist in the list, so `stage_day_1`/generic step ids may partially validate today, but `tutorial_`-prefixed ids (e.g. a hypothetical `tutorial_needs_basics` step id used as a Tier-1 cross-reference) will not resolve until this array is updated.
- All step IDs are unique.
- All `stage_id` references in steps resolve to defined stages.
- All `target_panel` values correspond to actual panel names in the host — **use the verified `OpenPlayerPanel` panelId list from Step 5's scope note, not invented ids.** Cross-check this now, in Step 2, rather than discovering the mismatch later in Step 5.
- JSON is valid, snake_case, with schema_version.

**Done when:** Tutorial content JSON is complete (20–30 steps across 5 stages), validates with data integrity, and follows all project data conventions.

---

## Step 3 — Implement TutorialSystem Core Logic

**Goal:** Build the runtime `TutorialSystem` that loads steps from JSON, evaluates triggers each tick, manages progression, and exposes events for the UI layer.

**Implementation:**
- Implement `TutorialSystem.cs` fully:
  ```csharp
  namespace Ashfall.Core.Tutorial;

  public sealed class TutorialSystem
  {
      private TutorialState _state;
      private List<TutorialStepData> _allSteps;
      private List<TutorialStageData> _allStages;
      private readonly IFileIO _fileIO;
      private readonly IJsonSerializer _json;
      private readonly IClock _clock;

      // Events for host UI
      public event Action<TutorialStepData> OnStepTriggered;
      public event Action<TutorialStepData> OnStepCompleted;
      public event Action<TutorialStageData> OnStageCompleted;
      public event Action OnTutorialCompleted;

      public void LoadContent(string tutorialDataPath);
      public void Tick(TutorialContext context);  // called each sim tick
      public void CompleteStep(string stepId);
      public void DismissStep(string stepId);
      public void SkipTutorial();
      public void SkipStage(string stageId);
      public void ResetTutorial();

      public TutorialState CaptureState() => _state;
      public void RestoreState(TutorialState state) => _state = state;

      public TutorialStepData GetActiveStep();     // currently displayed step (null if none)
      public IReadOnlyList<TutorialStepData> GetPendingSteps(); // triggered but not completed
      public float GetProgressPercent();           // 0.0–1.0 overall completion
  }
  ```
- Create `TutorialContext.cs` — snapshot passed to `Tick()`:
  ```csharp
  public sealed class TutorialContext
  {
      public int CurrentDay { get; set; }
      public HashSet<string> OpenPanels { get; set; }
      public HashSet<string> PerformedActions { get; set; }  // actions this tick
      public HashSet<string> OwnedItems { get; set; }
      public HashSet<string> ActiveSystems { get; set; }
  }
  ```
- Trigger evaluation logic: each tick, iterate uncompleted steps in current stage → check if trigger condition is met → fire `OnStepTriggered` for first matching step not yet triggered.
- Stage advancement: when all required (non-optional) steps in a stage are completed → fire `OnStageCompleted` → advance `CurrentStageId` to next stage.
- Tutorial completion: when all stages are completed (or skipped) → fire `OnTutorialCompleted`.
- Journal integration: on `OnStepCompleted`, optionally write a journal entry (via event, not direct dependency) summarizing what the player learned.

**Verification:**
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` compiles cleanly.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` passes.
- New tests in `Ashfall.Core.Tests/Tutorial/TutorialSystemTests.cs`:
  - `Tick_DayReached_TriggersStep` — step fires when day threshold met.
  - `CompleteStep_AdvancesStage_WhenAllRequiredDone` — stage progression works.
  - `SkipTutorial_CompletesAllSteps` — skip marks everything done.
  - `CaptureRestore_RoundTrip` — state survives save/load.
  - `OptionalSteps_DontBlockProgression` — optional steps can be ignored.
- No engine coupling (grep for `Godot.*` and `UnityEngine.*` returns 0 hits in tutorial files).

**Done when:** TutorialSystem processes triggers, manages progression, fires events, save/loads correctly, and all tests pass.

---

## Step 4 — Create Tutorial Overlay UI

**Goal:** Build the Godot UI layer that displays tutorial steps as non-modal overlays pointing at relevant panels, with dismiss/complete interactions.

**Implementation:**
- Create `src/UI/TutorialOverlay.cs` (Godot `CanvasLayer` + `Control`):
  - Renders above all game UI (highest CanvasLayer).
  - Displays current tutorial step as a tooltip/card:
    - Title (BarlowCondensed, bold, 18pt).
    - Body text (ShareTechMono, 14pt, max 3 lines visible, scrollable if longer).
    - Dismiss button ("Got it" or "Skip").
    - Progress indicator (dots or "2/5" showing step within stage).
  - **Pointer arrow** — visual arrow/line pointing from the tooltip to the `TargetPanel` (if specified). **Scope-bounding note (added on review):** pointing at a panel requires knowing that panel's on-screen `Control` node/rect at overlay-render time. Since Step 5 bounds visibility gating to the single `OpenPlayerPanel` choke point rather than per-sub-panel, the pointer arrow should likewise only ever target the small set of top-level panel ids enumerated there (13+ ids, not 91 files) — do not attempt to build generic positioning logic that can point at arbitrary sub-panels/Detail/History variants, since most of those aren't reachable until their parent is already open (at which point the tutorial step for that stage has already been satisfied by opening the parent). If a future step genuinely needs to point at a nav button rather than an open panel (e.g., "click here to open Crafting" before Crafting is open), get the button's rect from `GameDashboardPanel`'s nav button list (see `AddNavButton`, `src/UI/GameDashboardPanel.cs:559`), not from the target panel itself — the panel isn't instantiated/visible yet at that point.
  - **Panel highlight** — subtle glow/border on the target panel to draw attention. Same scoping applies: only for the top-level panel or its corresponding nav button, never a sub-panel.
  - **Positioning:** tooltip appears near but not overlapping the target panel or nav button. Auto-positions to avoid screen edges.
  - **Animation:** fade-in on trigger, fade-out on dismiss (200ms, no jarring pops).
- Create `src/UI/TutorialProgressBar.cs`:
  - Small persistent indicator (top-right or bottom-right) showing overall tutorial progress.
  - Clicking opens a summary of completed/pending stages.
  - Hidden when tutorial is complete or disabled.
- Wire into `Main.cs`:
  - Subscribe to `TutorialSystem.OnStepTriggered` → show overlay.
  - Subscribe to `TutorialSystem.OnStepCompleted` → hide overlay, show next if queued.
  - Pass `TutorialContext` each tick (populate from host state: open panels, owned items, current day).
- Ensure overlay doesn't block gameplay input (click-through except on the tooltip itself).
- Ensure overlay respects pause state (still visible when paused).

**Verification:**
- `dotnet build Ashfall.csproj` compiles cleanly.
- `godot --headless --path . -- --data-integrity-selftest` passes.
- Manual testing (documented test plan):
  - Day 1: first tutorial step appears with pointer to needs panel.
  - Dismiss: tooltip fades out, next step appears (if any).
  - Pointer correctly identifies target panel location.
  - Overlay doesn't block clicks on game elements outside the tooltip.
  - Progress bar updates as steps complete.
  - Overlay renders at correct CanvasLayer (above panels, below modal dialogs).
- Fonts match project spec (BarlowCondensed + ShareTechMono).

**Done when:** Tutorial overlay displays steps, points at targets, dismisses correctly, and doesn't interfere with gameplay.

---

## Step 5 — Add Tutorial-Aware Panel Visibility

**Goal:** Implement progressive panel disclosure — advanced panels are hidden until the tutorial unlocks their stage, reducing initial cognitive load for new players.

**Scope warning (added on review) — read before implementing:** The original draft's `panel_visibility.json` invents ten clean one-system-to-one-panel ids (`needs_panel`, `crafting_panel`, `trade_panel`, `faction_panel`, `medical_panel`, `expansion_panel`, etc.). Verified against the real codebase, this mapping does not hold:
- There are **91 files matching `*Panel*.cs`** in `src/`, not ~10.
- Several "systems" have *multiple* panels: factions alone has `FactionsPanel`, `FactionDetailPanel`, `FactionHistoryPanel`, `FactionMatrixPanel`, `FactionsNarrativePanel` (5 files). The same is true for combat, crafting, economy, and others (each has a base panel plus `*DetailPanel`/`*HistoryPanel` variants).
- The runtime panel-opening code in `Main.cs` (`OpenPlayerPanel(string panelId)`, `src/Main.cs:5636` onward) already has its own string-id scheme — `"survivors"`, `"inventory"`, `"crafting"`, `"medical"`, `"expeditions"`, `"weather"`, `"radio"`, `"map"`, `"shelter"`, `"factions"`, `"quests"`, `"journal"`, `"protocol"`, etc. **None of these match** the invented `needs_panel`/`crafting_panel`/`trade_panel`/`expedition_panel` ids in the original draft's JSON example. There is no `"needs_panel"` or `"trade_panel"` id anywhere in the codebase today (verified via grep — 0 matches).
- The economy overlay panel's class is `EconomyOverlayPanel`, not `EconomyPanel` — another mismatch between the plan's assumed naming and the real class names.

**Bounding the scope, per this review:** "Unlock progressive panels" must be scoped to the panels reachable via `Main.cs`'s existing `OpenPlayerPanel(string panelId)` switch — that switch is the actual single choke point where a panel becomes visible/openable to the player, and it already uses a canonical id string per top-level panel. Do **not** attempt to gate the 91 sub-panels (Detail/History/Matrix variants) individually; they are opened *from within* their parent panel and are already inaccessible if the parent is hidden. Concretely:
1. Step 5's `panel_visibility.json` must use the **real** `panelId` strings from `Main.cs`'s `OpenPlayerPanel` switch (survivors, inventory, crafting, medical, expeditions, weather, radio, map, shelter, factions, quests, journal, protocol, and any others found by reading the full switch statement — enumerate it exhaustively as an implementation sub-step before writing the JSON, don't assume the list from this note is complete).
2. `PanelVisibilityManager.IsVisible(panelId, state)` gates entry into `OpenPlayerPanel`, not the individual panel `Control` nodes' constructors — this keeps the blast radius to one call site in `Main.cs` instead of 91.
3. Sub-panels (Detail/History/etc.) inherit visibility from their parent automatically since they're unreachable without opening the parent first — do not write separate visibility rules for them, and do not treat their omission from `panel_visibility.json` as a coverage gap.
4. If the tutorial design genuinely wants finer-grained gating (e.g., hide `FactionMatrixPanel` specifically while allowing `FactionsPanel`), that is new scope beyond "progressive disclosure of top-level systems" as described in this batch's own Context section, and should be split into a separate follow-up rather than silently expanded here.

**Implementation:**
- Create `Assets/Ashfall.Core/Tutorial/PanelVisibilityRule.cs`:
  ```csharp
  public sealed class PanelVisibilityRule
  {
      public string PanelId { get; set; }           // matches panel name in host
      public string RequiredStageId { get; set; }    // panel hidden until this stage is reached
      public bool HideWhenTutorialDisabled { get; set; } = false; // always show for veterans
  }
  ```
- Create `Assets/StreamingAssets/Data/tutorial/panel_visibility.json` — **using the verified `Main.cs` `OpenPlayerPanel` panelId strings, not the placeholder ids below.** The table below is illustrative only; replace with the exhaustively-enumerated real ids as the first implementation sub-step:
  ```json
  {
    "schema_version": 1,
    "rules": [
      { "panel_id": "shelter", "required_stage_id": "stage_day_1" },
      { "panel_id": "crafting", "required_stage_id": "stage_day_3" },
      { "panel_id": "inventory", "required_stage_id": "stage_day_3" },
      { "panel_id": "expeditions", "required_stage_id": "stage_day_7" },
      { "panel_id": "map", "required_stage_id": "stage_day_7" },
      { "panel_id": "factions", "required_stage_id": "stage_day_14" },
      { "panel_id": "medical", "required_stage_id": "stage_day_30" }
    ]
  }
  ```
  (Note: there is no dedicated "needs panel" or "trade panel" id in `Main.cs` today — "needs" display appears to live inside a HUD/dashboard element rather than an `OpenPlayerPanel`-routed overlay, and trade is reached via `"factions"`/the Holdfast terminal rather than a standalone `"trade"` id. Confirm actual routing for these two before finalizing the JSON; do not carry over the placeholder ids from the original draft.)
- Create `Assets/Ashfall.Core/Tutorial/PanelVisibilityManager.cs`:
  - `IsVisible(string panelId, TutorialState state)` → bool.
  - If `state.TutorialEnabled == false`, all panels visible (veteran mode).
  - If stage is reached or skipped, panel becomes visible.
  - Panels from completed/skipped stages remain visible permanently.
- Wire into Godot host at the single `OpenPlayerPanel(string panelId)` choke point in `Main.cs` (per the scope-bounding note above — do not wire into individual panel constructors):
  - On tutorial state change, update panel visibility (e.g., disable/hide the corresponding nav button in `GameDashboardPanel`, and short-circuit `OpenPlayerPanel` for a not-yet-unlocked id).
  - Hidden panels: either `Visible = false` on their nav entry or collapsed in the panel selector/menu.
  - Newly revealed panel: brief "NEW" indicator or pulse animation to draw attention.
- Ensure panel visibility persists correctly through save/load (derives from `TutorialState`).

**Verification:**
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` compiles cleanly.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` passes.
- New tests:
  - `PanelVisibility_Day1_OnlyBasicPanelsVisible` — using the real `OpenPlayerPanel` ids, confirm e.g. `crafting`/`expeditions`/`factions` are gated on day 1.
  - `PanelVisibility_StageReached_UnlocksPanel` — reaching stage_day_3 reveals `crafting`.
  - `PanelVisibility_TutorialDisabled_AllVisible` — veteran mode shows everything.
  - `PanelVisibility_SkippedStage_RevealsPanel` — skipping a stage still unlocks its panels.
  - `PanelVisibility_AllRuleIds_MatchKnownOpenPlayerPanelIds` — new: asserts every `panel_id` in `panel_visibility.json` is one of the ids actually handled by `Main.cs`'s `OpenPlayerPanel` switch, catching drift if that switch changes later.
- `godot --headless --path . -- --data-integrity-selftest` passes (new JSON validates).
- Manual test: new game → only day-1-stage panels' nav entries visible/enabled → advance to day 3 → crafting nav entry appears.

**Done when:** Panel visibility responds to tutorial state at the `OpenPlayerPanel` choke point (not per sub-panel), all `panel_visibility.json` ids are verified against the real `Main.cs` switch, veterans see everything, and progressive disclosure works through save/load.

---

## Step 6 — Add Skip Tutorial Option

**Goal:** Allow veteran players to skip the tutorial entirely (during new game setup or at any point during gameplay), immediately revealing all panels and suppressing all tutorial prompts.

**Implementation:**
- Add skip option to new game flow:
  - In the new game / difficulty selection screen, add checkbox: "Skip tutorial (recommended for experienced players)".
  - If checked, `TutorialSystem` initializes with `TutorialEnabled = false`.
  - All panels immediately visible, no tutorial overlays ever appear.
- Add skip option during gameplay:
  - In settings/options menu: "Disable tutorial" toggle.
  - Toggling off mid-game: all pending steps marked complete, all panels revealed, progress bar hidden.
  - Toggling on mid-game: resumes from current progress (doesn't repeat completed steps).
- Add per-stage skip:
  - Tutorial overlay shows "Skip this section" link (smaller, less prominent than dismiss).
  - Skipping a stage: marks all steps in that stage as skipped (not completed — distinction for analytics).
  - Skipped stages still unlock their panels.
- Add reset option:
  - In settings: "Reset tutorial" button (with confirmation).
  - Resets all progress, hides advanced panels again, restarts from stage_day_1.
  - Only available if tutorial is enabled.
- Persist all tutorial preferences in the game save (not separate user prefs — tutorial state is per-playthrough, not per-player).

**Verification:**
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` compiles cleanly.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` passes.
- New tests:
  - `SkipTutorial_NewGame_AllPanelsVisible` — skip at start reveals everything.
  - `SkipTutorial_MidGame_StopsPrompts` — no more OnStepTriggered events after skip.
  - `SkipStage_UnlocksPanel_MarksSkipped` — stage skip reveals panels, steps show as skipped not completed.
  - `ResetTutorial_RestartsProgression` — reset returns to stage_day_1 state.
  - `ToggleOff_ThenOn_ResumesProgress` — re-enabling doesn't repeat completed steps.
- `godot --headless --path . -- --data-integrity-selftest` passes.
- Manual test: skip at new game → no tutorial ever appears → all panels available.

**Done when:** Skip works at new game and mid-game, per-stage skip exists, reset works, all states persist through save/load.

---

## Step 7 — Write Tutorial Progression Tests

**Goal:** Comprehensive test coverage for the full tutorial lifecycle: stage transitions, skip behavior, save/load preservation, edge cases, and integration with existing systems.

**Implementation:**
- Create `Ashfall.Core.Tests/Tutorial/TutorialProgressionTests.cs`:
  - **Test: FullProgression_Day1ToDay30** — Simulate 30 days of ticks with appropriate actions → all stages complete in order → `OnTutorialCompleted` fires.
  - **Test: StageTransition_RequiresAllRequiredSteps** — Stage doesn't advance until every non-optional step is completed.
  - **Test: OptionalSteps_CanBeSkipped_WithoutBlocking** — Optional steps don't prevent stage advancement.
  - **Test: SaveLoad_PreservesProgress** — `CaptureState` at day 15 → `RestoreState` → tutorial resumes at correct stage with correct completed steps.
  - **Test: SaveLoad_PreservesSkippedStages** — Skipped stages remain skipped after restore.
  - **Test: DuplicateTrigger_DoesNotRefire** — A completed step never triggers again even if conditions re-match.
  - **Test: MultipleStepsTriggered_OnlyShowsFirst** — When multiple steps trigger simultaneously, only one is active (queue behavior).
  - **Test: StageUnlock_OnExactDay** — Stage triggers on exact day boundary, not off-by-one.
  - **Test: TutorialDisabled_NoTriggersEvaluated** — When disabled, `Tick()` short-circuits (performance: no trigger evaluation).
  - **Test: InvalidStepId_CompleteStep_Ignored** — Completing a non-existent step ID doesn't crash.
  - **Test: EmptyTutorialData_GracefulNoOp** — If tutorial JSON is missing/empty, system operates without error (no tutorial shown).
  - **Test: JournalIntegration_CompletedStepCreatesEntry** — On step completion, the journal event is raised with correct tag.
- Create `Ashfall.Core.Tests/Tutorial/TutorialDataValidationTests.cs`:
  - **Test: AllStepIds_AreSnakeCase** — Convention enforcement.
  - **Test: AllStepIds_AreUnique** — No duplicates across all step files.
  - **Test: AllStageReferences_ResolveToDefinedStages** — No step references a non-existent stage.
  - **Test: AllTargetPanels_AreKnownPanels** — No step references a panel that doesn't exist.
  - **Test: StepOrderWithinStage_IsContiguous** — Display orders within a stage are 1, 2, 3... (no gaps).
- Run full verification suite:
  ```bash
  dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
  dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
  dotnet build Ashfall.csproj
  godot --headless --path . -- --data-integrity-selftest
  godot --headless --path . -- --bridge-selftest
  ```

**Verification:**
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` compiles cleanly.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — all 17 new tests pass alongside all existing tests.
- No flaky tests across 5 consecutive runs.
- Test coverage: every public method on `TutorialSystem` is exercised at least once.
- Data validation tests catch intentional errors (add a malformed test fixture and verify it's detected).

**Done when:** All 17 tests pass, full verification suite is green, tutorial system has comprehensive coverage for progression, edge cases, and data validation.

---

## Summary

| Step | Deliverable | Risk | New Tests | Key Files |
|------|-------------|------|-----------|-----------|
| 1 | `TutorialSystem` architecture + interfaces in Core | Low | 0 (compile only) | `Assets/Ashfall.Core/Tutorial/` (6 files) |
| 2 | Tutorial content JSON (stages + 20–30 steps) | Low — but do the Step 5 panel-id enumeration first (see ordering note in Step 2) to avoid rewriting `target_panel` values later | 0 (data integrity validates) | `Assets/StreamingAssets/Data/tutorial/` |
| 3 | `TutorialSystem` runtime implementation | Low | 5 | `Assets/Ashfall.Core/Tutorial/TutorialSystem.cs` |
| 4 | `TutorialOverlay` + `TutorialProgressBar` UI | Medium — pointer/highlight positioning must be scoped to top-level panels/nav buttons only (see Step 4 scope note), not all 91 panel files | 0 (manual test plan) | `src/UI/TutorialOverlay.cs`, `src/UI/TutorialProgressBar.cs` |
| 5 | `PanelVisibilityManager` + visibility rules | Medium — must gate at the single `OpenPlayerPanel` choke point in `Main.cs` using verified real panelId strings, not invented per-panel ids (see Step 5 scope note) | 5 (4 original + 1 id-drift regression test) | `Assets/Ashfall.Core/Tutorial/PanelVisibilityManager.cs` |
| 6 | Skip/reset tutorial controls (new game + mid-game) | Low | 5 | `TutorialSystem.cs` (extended), `src/UI/` |
| 7 | Comprehensive progression + data validation tests | None | 17 | `Ashfall.Core.Tests/Tutorial/` |

**Total new tests:** 32 (31 original + 1 added id-drift regression test in Step 5)
**Total new files:** ~14 (Core tutorial model + JSON data + Godot UI + test files)
**Estimated effort:** 5–7 days for Steps 1-7 as bounded above (top-level `OpenPlayerPanel` panels only). If the tutorial's design goal expands to gating individual sub-panels (Detail/History/Matrix variants across the 91 panel files), treat that as separate, additional scope — it is not covered by this estimate.
**Exit criteria:** All existing tests pass (2,120 `[Fact]`/`[Theory]` tests as of this review — verify the live count with `dotnet test` before starting rather than trusting a number carried over from a prior batch), 32 new tutorial tests pass, tutorial overlay displays correctly and only targets verified top-level panel ids, progressive panel disclosure works and is gated at the `OpenPlayerPanel` choke point, skip/reset functional, full verification suite green, tutorial content follows project tone and data conventions, and all `target_panel`/`panel_id` values in JSON have been cross-checked against the real `Main.cs` switch (not assumed).

## Risk & Rollback

- **Risk — panel id drift.** `panel_visibility.json` and the tutorial step JSON's `target_panel` fields hardcode string ids that mirror `Main.cs`'s `OpenPlayerPanel` switch. If that switch is refactored later (new panel added, id renamed) without updating the tutorial JSON, panels could silently stay hidden forever or the overlay could point at nothing. Mitigated by the new `PanelVisibility_AllRuleIds_MatchKnownOpenPlayerPanelIds` test (Step 5) — keep this test in CI permanently, not just at initial implementation, since it is the only guard against this class of drift.
- **Risk — tutorial content blocking progression.** If a required (non-optional) step's `TriggerCondition`/`CompletionCondition` is misconfigured (e.g., references an `ActionPerformed` type the host never actually raises), the stage can never advance and the affected panels stay hidden indefinitely for real players. Mitigate with the `TutorialDataValidationTests` in Step 7, but also add a host-side escape hatch: the "Skip this section" / "Disable tutorial" controls in Step 6 must work even if a step is stuck, so a broken step is a UX annoyance, not a soft-lock. Confirm this explicitly in Step 6's manual test plan (try skipping while a broken/never-firing step is active).
- **Risk — journal entry flood.** `JournalSystem.MaxEntries = 64` with oldest-entry eviction. If tutorial-authored journal entries (Step 3's "journal integration") interleave with regular gameplay discoveries, a long tutorial run risks evicting real narrative entries, or the tutorial's own step-completion entries get evicted before the player reads them. Flag this as a design decision to make explicitly in Step 3 (e.g., tutorial entries could skip the journal integration entirely, or use a distinct low-priority path) rather than an unstated side effect.
- **Rollback:** All seven steps are additive (`Assets/Ashfall.Core/Tutorial/`, `Assets/StreamingAssets/Data/tutorial/`, `src/UI/TutorialOverlay.cs`, `src/UI/TutorialProgressBar.cs`, `Ashfall.Core.Tests/Tutorial/`) plus one call site in `Main.cs`'s `OpenPlayerPanel` (per the Step 5 scope-bounding) and one subscription block wiring `TutorialSystem` events. Rollback is a deletion of the new directories/files plus reverting the single `Main.cs` call site — low blast radius, consistent with the Low/Medium risk ratings above. The Medium-risk items (Steps 4 and 5) are Medium because of *scope-creep risk during implementation* (accidentally touching more of the 91 panel files than intended), not because rollback itself is hard — if the scope-bounding notes above are followed, rollback stays a clean deletion.


## Review Notes (Corrected)

Adversarial review performed against the real codebase at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War` (Godot host). Findings and fixes applied in place above:

1. **Factual error — `StartingLevelSystem` does not gate complexity.** The original draft listed `StartingLevelSystem` under "Systems affected" as "(difficulty gating)" and under "Existing systems to leverage" as "configures initial difficulty; could gate system complexity." Read the full source (`Assets/Ashfall.Core/StartingLevel/StartingLevelSystem.cs`, 280 lines): it is a Day-1-only Holdfast simulation (ration policy, maintenance directives, radio protocol, room inspection, air filter degradation). There is no difficulty field, no complexity tier, and no API for gating other systems anywhere in the class. This was the most significant factual error in the plan — it would have led an implementer to go looking for a "difficulty gating" hook that doesn't exist. Removed `StartingLevelSystem` from the systems-affected list and corrected the Context section to state plainly that progressive disclosure is new work built in this batch (`PanelVisibilityManager`), not something reused from an existing system.

2. **Factual claim confirmed accurate — `JournalSystem`.** The claim that `JournalSystem` "tracks entries with tags, day-stamps, and narrative context" is correct: verified `entry.Day`/`entry.Hour` timestamps, `entry.KnowledgeKey` as the tag/dedup mechanism, and `OnEntryAdded`/`OnCodexUnlocked` events. Added one caveat the original missed: `MaxEntries = 64` with oldest-first eviction, which matters for a long tutorial run competing with regular gameplay journal entries for the same finite buffer. Flagged as a design decision to make explicitly (see Risk & Rollback) rather than an unstated side effect.

3. **Scope problem — panel visibility (this is the issue the request specifically asked about).** The original `panel_visibility.json` invents ten one-system-to-one-panel ids (`needs_panel`, `crafting_panel`, `trade_panel`, `faction_panel`, `medical_panel`, `expansion_panel`, etc.). None of these ids exist anywhere in the codebase (verified via grep, 0 matches). The real routing mechanism is `Main.cs`'s `OpenPlayerPanel(string panelId)` switch, which uses different, already-established ids (`"survivors"`, `"crafting"`, `"factions"`, etc.), and several "systems" have multiple panel files (factions alone has 5: `FactionsPanel`, `FactionDetailPanel`, `FactionHistoryPanel`, `FactionMatrixPanel`, `FactionsNarrativePanel`). There are 91 files matching `*Panel*.cs` in `src/`, not ~10. Un-bounded, "unlock progressive panels" could be read as needing per-file gating logic across all 91 — that is not viable as a single batch. **Fix applied:** bounded Step 5 to gate only at the single `OpenPlayerPanel` choke point using the real, verified panelId strings; explicitly stated that Detail/History/Matrix sub-panels inherit visibility from their parent and must not get separate rules; added a regression test (`PanelVisibility_AllRuleIds_MatchKnownOpenPlayerPanelIds`) so future drift between the JSON and the real switch is caught automatically instead of silently breaking. Also identified and flagged that Step 4's pointer-arrow/highlight logic has the same scoping need and must target only the same bounded id set.

4. **Ordering/logic issue.** Step 2 (tutorial content authoring) writes `target_panel` values before Step 5 (where the real panel ids get enumerated and verified) — as originally sequenced, Step 2 would author against the same invented ids that Step 5's review found to be wrong, requiring a rewrite later. Added an explicit note to pull the `Main.cs` id enumeration forward as a prerequisite before Step 2's content authoring, even though it's formally documented under Step 5.

5. **Verification criterion was conditional where it should be definitive.** Step 2's original verification said "add `tutorial_` prefix to known prefixes if needed." Read `CatalogIntegrityValidator.cs`'s `IdPrefixes` array directly — confirmed `tutorial_` is absent today. Changed "if needed" to a definitive instruction, since the conditional phrasing could be misread as "check first, maybe skip."

6. **Stale test count.** "1941+ existing tests" is unverifiable against this snapshot; actual count via `grep -rc "\[Fact\]\|\[Theory\]"` is 2,120. Replaced the hardcoded number with an instruction to verify the live count before starting.

7. **Missing Risk & Rollback section.** Added one, covering panel-id drift (mitigated by the new regression test), tutorial steps that could soft-lock progression if misconfigured (mitigated by ensuring skip/disable controls work even on a stuck step — added as an explicit Step 6 manual-test item), and the journal `MaxEntries` eviction interaction. Also clarified that the "Medium" risk on Steps 4/5 is about scope-creep risk during implementation (touching more of the 91 panel files than intended), not rollback difficulty — rollback stays a clean deletion if the scope-bounding notes are followed.

8. **Cross-batch consistency check.** Verified Step 6's choice to persist `TutorialState` inside the game save (not `user://` prefs) is a deliberate and correct distinction from Batch 81's `user://`-based input-scheme persistence — tutorial progress is per-playthrough state, appropriately captured via `CaptureState()`/`RestoreState()`, not a per-player preference. No fix needed here; confirmed as correct by design.

All corrections above are integrated into the step-by-step content above, not just listed here.
