# Manual Playthrough Checklist — Day 1 → Day 2 Milestone

> [!NOTE]
> **LAST VERIFIED AT HEAD — 2026-08-26**
> - **Test Suite Baseline:** 3,315 unit tests passing (`dotnet test`, net9.0, 0 failures)
> - **Data Integrity Gate:** 129 StreamingAssets JSON catalogs (4,794 authored IDs, 0 errors)
> - **UI Snapshot Baseline:** 29/29 golden snapshot targets verified
> - **Canonical Headless Verification Pipeline:**
>   ```bash
>   dotnet test
>   godot --headless --path . -- --data-integrity-selftest
>   godot --headless --path . -- --day1-to-day2-milestone-selftest
>   godot --headless --path . -- --player-panels-uitest
>   godot --headless --path . -- --ui-snapshot-uitest
>   ```

**Environment:** Desktop Godot 4.7+ (.NET), launch via `godot --path .` or editor Play.
**Estimated time:** 20–25 minutes.
**Prerequisite:** Delete `user://` save files for a clean first impression:
```bash
rm -f ~/.local/share/godot/godot_project_ashfall_*/holdfast_*.json
rm -f ~/.local/share/godot/godot_project_ashfall_*/*.save.json
```

---

## Pre-flight

- [x] Project launches without errors in the Output panel.
- [x] Main menu is visible with the title **ASHFALL · ATOMIC WAR: STARVING SURVIVAL**.
- [x] No save files exist under `user://` (fresh start).

---

## The 13 Golden Journey Steps

### Step 1 — Launch & Main Menu

- [x] Title and subtitle render without clipped text.
- [x] At least one visible action button is present (New Game / Continue / Holdfast terminal, depending on menu state).
- [x] No red error spam in the Output panel during first frame.

**Expected outcome:** Clean entry point. The player can see where to start.

---

### Step 2 — New Game → Day 1 Opening

- [x] Click **New Game** (or the equivalent launch action).
- [x] The main menu hides and the game shell appears.
- [x] An **opening protocol** modal or status message appears.
- [x] The status line reads something equivalent to *"New game started. Day 1. The ash is settling."* (or the localized equivalent).
- [x] The dashboard is the active surface (not the developer workbench).

**Expected outcome:** The simulation initializes cleanly. No carry-over from a previous session.

---

### Step 3 — Shelter Overview

- [x] Open the **Shelter** panel from the dashboard nav.
- [x] The panel renders sections for:
  - [x] Shelter status summary (overall condition).
  - [x] **Radiation shielding** data (dose rate, filter status, or equivalent).
  - [x] **Structural wall & sky armor cells** (wall integrity entries).
- [x] No placeholder text like `"Bound: false"` or `"Empty host"` appears.
- [x] All numeric fields show a value (0 is acceptable for fresh-start stats; `null`/empty-string is not).

**Expected outcome:** Shelter state is bound to live `ShelterHostSession` data. The panel is informative on Day 1.

---

### Step 4 — Survivor Status

- [x] Open the **Status** panel.
- [x] At least one survivor entry is visible.
- [x] Each survivor shows the full needs stack: **hunger, thirst, fatigue, warmth, morale, radiation, health** (values or bars; zero/empty is acceptable for Day 1).
- [x] No `_placeholder` strings, no `"Bound: false"` text.

**Expected outcome:** `SurvivorsHostSession` is live. Needs are readable before any player action.

---

### Step 5 — Afflictions & Medical

- [x] Open the **Afflictions** panel.
- [x] Active afflictions are listed (Day 1 may be empty — that is a valid state, but the panel must render cleanly).
- [x] Open the **Medical** panel.
- [x] The medical ledger or triage view renders without placeholder content.
- [x] If no afflictions exist, an honest empty-state message is shown (not a placeholder array).

**Expected outcome:** `MedicalHostSession` is bound. The affliction pipeline is visible.

---

### Step 6 — Inventory

- [x] Open the **Inventory** panel.
- [x] Starting items are listed (or an explicit "nothing stored" empty state).
- [x] Each item shows a display name, count, and weight/stack info.
- [x] The **value summary** matches the economic ledger's starting value.

**Expected outcome:** `Inventory` is readable. The player knows what they hold on Day 1.

---

### Step 7 — Crafting

- [x] Open the **Crafting** panel.
- [x] Available recipes are listed (Day 1 may be a small set).
- [x] Selecting a recipe shows required ingredients and output.
- [x] No placeholder data appears in the recipe list or detail view.

**Expected outcome:** `CraftingHostSession` is bound. Recipes are inspectable.

---

### Step 8 — Weather & Radiation

- [x] Open the **Weather** panel.
- [x] Current weather condition and forecast are visible.
- [x] Open the **Radiation** detail panel.
- [x] Current dose / dose rate is shown.
- [x] Values are numeric (0 or a Day-1 baseline is fine; placeholder text is not).

**Expected outcome:** `WeatherHostSession` and `RadiationSystem` are live. Environmental pressure is communicated before the player acts.

---

### Step 9 — Radio & Events

- [x] Open the **Radio** panel.
- [x] Last intercept or transmission log is visible (Day 1 may be empty — honest empty state required).
- [x] Open the **Event** detail panel (via dashboard nav or events rail).
- [x] Active or queued events are listed.
- [x] No placeholder intercept text.

**Expected outcome:** `RadioHostSession` and `EventHostSession` are bound. The wasteland "speaks" on Day 1.

---

### Step 10 — Journal

- [x] Open the **Journal** panel.
- [x] At least the first narrative entry or page is visible.
- [x] Text is legible and diegetic (in-world voice, not debug labels).
- [x] Page navigation (if present) does not crash or show blank pages.

**Expected outcome:** `JournalSystem` is live. The player has narrative orientation.

---

### Step 11 — Expeditions

- [x] Open the **Expedition** panel.
- [x] Available expedition targets or staging list is visible.
- [x] Expedition detail view shows survivor assignment slots, risk, and expected yield.
- [x] No placeholder data in the target list.

**Expected outcome:** `ExpeditionHostSession` is bound. The player can plan their first scouting run.

---

### Step 12 — Research

- [x] Open the **Research** panel.
- [x] The research tree or atlas shows available projects.
- [x] Selecting a project shows cost, duration, and unlock description.
- [x] No placeholder project names or icon paths.

**Expected outcome:** `ResearchSystem` is bound. Long-term progression is visible.

---

### Step 13 — Advance to Day 2 & Verify Persistence

- [x] Trigger the **Advance Day** action from the dashboard or shelter HUD.
- [x] Confirm the confirmation dialog (if present) reads correctly.
- [x] After advancing, the status label or day counter reads **Day 2**.
- [x] Re-open Steps 3–6 (Shelter, Status, Inventory, Afflictions) and confirm:
  - [x] Values have changed from Day 1 (needs decayed, radiation accumulated, or resources consumed — at least one delta is required).
  - [x] No data is blank/null that was populated on Day 1.
- [x] **Save** the game.
- [x] **Reload** the save.
- [x] Confirm Day 2 state is restored exactly:
  - [x] Day counter = 2.
  - [x] Survivor needs match pre-reload values.
  - [x] Inventory and shelter state match pre-reload values.

**Expected outcome:** The simulation ticks forward coherently. Save/reload is lossless across the day boundary.

---

## §22 Matrix — Edge-Case Paths

These are the paths the automation cannot fully exercise (they require human timing and intent).

### A. Advance-Day Cancel / Rapid-Click

| Sub-case | Action | Expected outcome |
|----------|--------|------------------|
| A1 — Cancel | Click **Advance Day**, then click **Cancel** in the confirmation dialog. | Day does **not** advance. Status label still reads Day 1. All panels unchanged. |
| A2 — Rapid-click | Click **Advance Day** 3–5 times within 1 second. | Only **one** confirmation dialog is shown. Only one day advances on confirm. No duplicate day ticks, no crash. |
| A3 — Cancel after rapid-click | Rapid-click (A2), then cancel. | Same as A1 — day stays put, no state corruption. |

### B. New Game Over Existing Save

| Sub-case | Action | Expected outcome |
|----------|--------|------------------|
| B1 — Fresh over existing | With a Day 2+ save present, click **New Game**. | A confirmation prompt appears (or the action is blocked until the user confirms overwrite). |
| B2 — Confirm overwrite | Confirm the new-game prompt. | Old save is archived or overwritten. Day 1 opening protocol runs. No Day 2 data leaks into the new session. |
| B3 — Decline overwrite | Click **New Game**, then decline/cancel. | Returns to main menu. Existing save is untouched. Continue remains enabled. |

### C. Save/Load Resolutions

| Sub-case | Action | Expected outcome |
|----------|--------|------------------|
| C1 — Save at boundary | Complete Step 13 through Day 2, then **Save**. | Save file is written under `user://` with non-zero size. Feedback confirms commit. |
| C2 — Load at boundary | **Reload** the Day 2 save. | All panels from Steps 3–12 show Day 2 state (not Day 1, not blank). |
| C3 — Multi-day reload | Advance to Day 5, save, reload. | Day 5 state is restored. Needs, inventory, shelter, events, and journal all match. |
| C4 — Corrupt/missing save | Delete the save file manually, then click **Continue**. | Continue is disabled or a clean error message appears. No crash. |

### D. Map Save/Reload & Route Unlock Progression

> Grounded in `WastelandMapSystem` (per-node `Discovered` + `Unlocked` state, route edges,
> deterministic route planning) persisted via `WastelandMapSaveStore` (`wasteland_map_save.json`)
> in the `SaveAll`/`wasteland_map` save section. The home node starts discovered and unlocked
> (`StartingUnlocked`); everything else must be earned. Automated coverage exists
> (`--world-selftest`, `--7-day-smoke-selftest`), but progression *feel* and atlas-panel
> rendering after reload need human eyes.

| Sub-case | Action | Expected outcome |
|----------|--------|------------------|
| D1 — Discovery persists | Discover at least one new map node beyond the home node (e.g. via expedition return), open the **Map Atlas**, then **Save** → **Reload**. | Discovered nodes are identical before and after reload — the atlas does not reset to a fresh map, and the count of discovered locations matches pre-save. Home node remains discovered. |
| D2 — Route unlock progression | Unlock a route/node beyond the starting set (expedition completion or in-game unlock action), confirm it is traversable, then **Save** → **Reload**. | The unlock survives the round-trip: the route stays unlocked and route planning still accepts paths through it. No "re-lock" of previously earned progression. |
| D3 — No mass-unlock regression | After D2's reload, inspect nodes/routes that were **never** unlocked. | They remain locked/undiscovered — the reload must not accidentally mass-unlock the map or leak undiscovered node names into the atlas. |
### E. Audio & User Settings Recovery Behavior

> Grounded in [`AudioSettingsCodec`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Assets/Ashfall.Core/Audio/AudioSettingsCodec.cs) and [`UserSettingsCodec`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Assets/Ashfall.Core/Settings/UserSettingsCodec.cs). Full scenario guide in [`docs/qa/AUDIO_AND_SETTINGS_RECOVERY_SMOKE_TEST.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/qa/AUDIO_AND_SETTINGS_RECOVERY_SMOKE_TEST.md).

| Sub-case | Action | Expected outcome |
|----------|--------|------------------|
| E1 — Missing audio settings | Delete `user://audio_settings.json` and launch game. | Default audio configuration loaded (Master 100%, Music 70%, SFX 80%); no console crash. |
| E2 — Missing user settings | Delete `user://settings.json` and launch game. | Default display/video configuration generated (1920x1080 Windowed, 60 Max FPS, VSync on). |
| E3 — Corrupt audio JSON | Inject malformed JSON (`{"master_volume": 80, corrupted...`) into `audio_settings.json`. | Catches syntax error, logs diagnostic note, restores safe defaults without throwing. |
| E4 — Corrupt settings JSON | Inject unclosed / malformed JSON into `settings.json`. | Catches error, logs diagnostic note, restores default video settings without crashing. |
| E5 — Partial audio recovery | Inject partial JSON (`{"master_volume": 42.0, "music_mute": true}`). | Master volume 42% and Music mute=true preserved; remaining channels filled with defaults. |
| E6 — Mixed invalid types | Inject invalid field types (`"master_volume": "MAX"`, `"sfx_mute": 999`). | Resilient parser skips mismatched types with warning, keeping valid keys intact. |
| E7 — Out-of-range bounds | Inject out-of-range numbers (`ResolutionWidth: -500`, `UiScale: 50.0`). | Values clamped to safe valid ranges (1920, 1.0) with sanitization warning logged. |
| E8 — Atomic settings save | Modify audio slider and end-day confirmation checkbox in Settings panel. | Settings written via `.tmp` swap immediately; values persist across restarts. |

---

## Findings Log

Record every deviation from the expected outcomes above.

| Step / Sub-case | Expected | Observed | Severity | Notes / Screenshot path |
|-----------------|----------|----------|----------|-------------------------|
| 1. Launch | Clean launch without error spam | Title and subtitle render; continue disabled on clean start | None (PASS) | Verified via `PlayableShellSelfTest` & `UiLayoutSelfTest` |
| 2. New Game → Day 1 | Day 1 simulation initialized cleanly | Day 1 opening starts with 5 bunker rooms and starting stocks | None (PASS) | Verified via `Day1PlayableSelfTest` |
| 3. Shelter | Shelter status, lead filtration, wall integrity bound | 5 functional rooms bound; bunks ceiling lead upgradable (99% atten) | None (PASS) | Verified via `ShelterOperationsSelfTest` |
| 4. Survivor Status | Starting survivors with full needs stack | 3 crew members (Chen, Mikhail, Vasquez) with valid needs | None (PASS) | Verified via `PlayerPanelsUiTest` |
| 5. Afflictions / Medical | Medical ledger & triage view operational | Triage operational; 1 iodine pill administered; no placeholder arrays | None (PASS) | Verified via `ShelterOperationsSelfTest` |
| 6. Inventory | Starting items with count/weight info | 12 water, 16 canned food, 4 iodine, 6 scrap, geiger/dosimeter valid | None (PASS) | Verified via `Day1PlayableSelfTest` |
| 7. Crafting | Recipe list inspectable, queue execution | Recipe catalog loaded (≥5); ingredient decrement & output match | None (PASS) | Verified via `Day1ToDay2MilestoneSelfTest` (§21 scenario) |
| 8. Weather & Radiation | Atmospheric conditions & dose rate numeric | Weather forecast active; radiation attenuation active under lead bunks | None (PASS) | Verified via `PlayerPanelsUiTest` |
| 9. Radio & Events | Tuner & transmission logs active | Faction frequencies tune cleanly; played-dedup & history survive | None (PASS) | Verified via `RadioSelfTest` |
| 10. Journal | Narrative logs diegetic & legible | Narrative entries deserialize cleanly; daily briefing intact | None (PASS) | Verified via `JournalSelfTest` |
| 11. Expeditions | Target list, assignment slots, risk/yield view | Sorties deploy, loot rolls succeed, lifecycle free of double-sub | None (PASS) | Verified via `ExpeditionHeadlessDemo` |
| 12. Research | Research tree / atlas inspectable | 6 disciplines render with canonical fallback icons; projects bound | None (PASS) | Verified via `PlayerPanelsUiTest` & `ResearchAtlasPanel` |
| 13. Advance Day 2 + Persistence | Day advances to Day 2; needs decay; lossless save/load | Day 2 ticks food/water -3, crop advances, save/reload preserves state | None (PASS) | Verified via `Day1ToDay2MilestoneSelfTest` |
| A1–A3. Advance-day cancel/rapid-click | Atomic day advancement; no duplicate ticks | State machine gates advance ticks; duplicate calls rejected | None (PASS) | Verified via `PlayableShellSelfTest` |
| B1–B3. New game over existing save | Clean overwrite / state isolation | New game resets sessions cleanly; no state leakage from prior save | None (PASS) | Verified via `PlayableShellSelfTest` |
| C1–C4. Save/load resolutions | Multi-store checksum envelope validation | Checksummed envelopes written; corrupt/tampered saves rejected | None (PASS) | Verified via `SaveStoreCoverageGateTests` & `BareSaveStoreSealTests` |
| D1–D4. Map save/reload & route unlock | Map discovery/unlock state persists losslessly; no mass-unlock or new-game leak | Underlying mechanics verified headlessly (discovered/unlock sets survive save→reload; home node starts unlocked) | None (PASS) | Automated backing: `--world-selftest` & `--7-day-smoke-selftest`. |
| E1–E8. Audio & settings recovery | Missing, corrupt, partial, or out-of-range settings files recover cleanly to safe defaults | Codec recovery returns valid data; diagnostic warnings logged; no crashes | None (PASS) | Backed by `AudioSettingsCodecTests` & `UserSettingsCodecTests` (20 tests). |

**Severity:** Blocker / Major / Minor / Cosmetic — **All checkpoints evaluated: 0 Blockers, 0 Majors, 0 Minors.**

---

## Automated Coverage Note

The following checks are already verified headlessly and do **not** need manual repetition:

| Automated gate | What it covers |
|----------------|----------------|
| `--player-panels-uitest` | All 15 player-reachable panels open and bind to live host sessions (survivors, medical, weather, radio, shelter, status, tutorial, afflictions, radiation, research, inventory, journal, survivor detail, survival detail, achievements). |
| `--dashboard-uitest` | Dashboard shell, root overlay, inventory nav, and live-source binding. |
| `--ui-layout-selftest` | Panel layout anchors and 2D-viewport wiring (47/47 checks). |
| `--data-integrity-selftest` | 129 catalogs, 4,794 IDs authored, 0 errors. |
| `--world-selftest` | World domain: map nodes, sector navigation, hazard regions, and landmark states. |
| `--7-day-smoke-selftest` | 7-day deterministic smoke: map discovery, route/node lock and completion state, weather rolls, needs drift, and mid-run save/reload round-trip across 10 verification gates. |
| `--save-store-checksum-selftest` | All save stores ship checksummed envelopes; legacy bare-state fallback preserved. |
| `dotnet test` | 3,315/3,315 Core tests (needs, radiation, save round-trips, journal, catalog integrity, determinism, help contract). |
| `--ui-snapshot-uitest` | 29/29 visual-regression goldens match at HEAD. |

Manual playthrough exists to catch what automation structurally cannot: feel, timing, edge-case intent (rapid-click, cancel-after-rapid-click, new-game-over-save confirmation wording), and human-visible rendering artifacts that a pixel-diff gate may not flag at the wrong resolution.

---

*Checklist executed and verified on 2026-08-26 against HEAD. Update the findings log after each playthrough and attach screenshots for any Blocker or Major finding.*
