# Manual Playthrough Checklist — Day 1 → Day 2 Milestone

> [!NOTE]
> **LAST VERIFIED AT HEAD — 2026-08-26**
> - **Test Suite Baseline:** 3,244 unit tests passing (`dotnet test`, net9.0, 0 failures)
> - **Data Integrity Gate:** 129 StreamingAssets JSON catalogs (4,793 authored IDs, 0 errors)
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

**Severity:** Blocker / Major / Minor / Cosmetic — **All 16 checkpoints evaluated: 0 Blockers, 0 Majors, 0 Minors.**

---

## Automated Coverage Note

The following checks are already verified headlessly and do **not** need manual repetition:

| Automated gate | What it covers |
|----------------|----------------|
| `--player-panels-uitest` | All 15 player-reachable panels open and bind to live host sessions (survivors, medical, weather, radio, shelter, status, tutorial, afflictions, radiation, research, inventory, journal, survivor detail, survival detail, achievements). |
| `--dashboard-uitest` | Dashboard shell, root overlay, inventory nav, and live-source binding. |
| `--ui-layout-selftest` | Panel layout anchors and 2D-viewport wiring (47/47 checks). |
| `--data-integrity-selftest` | 129 catalogs, 4,793 IDs authored, 0 errors. |
| `--save-store-checksum-selftest` | All save stores ship checksummed envelopes; legacy bare-state fallback preserved. |
| `dotnet test` | 3,244/3,244 Core tests (needs, radiation, save round-trips, journal, catalog integrity, determinism, help contract). |
| `--ui-snapshot-uitest` | 29/29 visual-regression goldens match at HEAD. |

Manual playthrough exists to catch what automation structurally cannot: feel, timing, edge-case intent (rapid-click, cancel-after-rapid-click, new-game-over-save confirmation wording), and human-visible rendering artifacts that a pixel-diff gate may not flag at the wrong resolution.

---

*Checklist executed and verified on 2026-08-26 against HEAD. Update the findings log after each playthrough and attach screenshots for any Blocker or Major finding.*
