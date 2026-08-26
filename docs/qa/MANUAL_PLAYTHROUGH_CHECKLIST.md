# Manual Playthrough Checklist — Day 1 → Day 2 Milestone

**Environment:** Desktop Godot 4.7+ (.NET), launch via `godot --path .` or editor Play.
**Estimated time:** 20–25 minutes.
**Prerequisite:** Delete `user://` save files for a clean first impression:
```bash
rm -f ~/.local/share/godot/godot_project_ashfall_*/holdfast_*.json
rm -f ~/.local/share/godot/godot_project_ashfall_*/*.save.json
```

---

## Pre-flight

- [ ] Project launches without errors in the Output panel.
- [ ] Main menu is visible with the title **ASHFALL · ATOMIC WAR: STARVING SURVIVAL**.
- [ ] No save files exist under `user://` (fresh start).

---

## The 13 Golden Journey Steps

### Step 1 — Launch & Main Menu

- [ ] Title and subtitle render without clipped text.
- [ ] At least one visible action button is present (New Game / Continue / Holdfast terminal, depending on menu state).
- [ ] No red error spam in the Output panel during first frame.

**Expected outcome:** Clean entry point. The player can see where to start.

---

### Step 2 — New Game → Day 1 Opening

- [ ] Click **New Game** (or the equivalent launch action).
- [ ] The main menu hides and the game shell appears.
- [ ] An **opening protocol** modal or status message appears.
- [ ] The status line reads something equivalent to *"New game started. Day 1. The ash is settling."* (or the localized equivalent).
- [ ] The dashboard is the active surface (not the developer workbench).

**Expected outcome:** The simulation initializes cleanly. No carry-over from a previous session.

---

### Step 3 — Shelter Overview

- [ ] Open the **Shelter** panel from the dashboard nav.
- [ ] The panel renders sections for:
  - [ ] Shelter status summary (overall condition).
  - [ ] **Radiation shielding** data (dose rate, filter status, or equivalent).
  - [ ] **Structural wall & sky armor cells** (wall integrity entries).
- [ ] No placeholder text like `"Bound: false"` or `"Empty host"` appears.
- [ ] All numeric fields show a value (0 is acceptable for fresh-start stats; `null`/empty-string is not).

**Expected outcome:** Shelter state is bound to live `ShelterHostSession` data. The panel is informative on Day 1.

---

### Step 4 — Survivor Status

- [ ] Open the **Status** panel.
- [ ] At least one survivor entry is visible.
- [ ] Each survivor shows the full needs stack: **hunger, thirst, fatigue, warmth, morale, radiation, health** (values or bars; zero/empty is acceptable for Day 1).
- [ ] No `_placeholder` strings, no `"Bound: false"` text.

**Expected outcome:** `SurvivorsHostSession` is live. Needs are readable before any player action.

---

### Step 5 — Afflictions & Medical

- [ ] Open the **Afflictions** panel.
- [ ] Active afflictions are listed (Day 1 may be empty — that is a valid state, but the panel must render cleanly).
- [ ] Open the **Medical** panel.
- [ ] The medical ledger or triage view renders without placeholder content.
- [ ] If no afflictions exist, an honest empty-state message is shown (not a placeholder array).

**Expected outcome:** `MedicalHostSession` is bound. The affliction pipeline is visible.

---

### Step 6 — Inventory

- [ ] Open the **Inventory** panel.
- [ ] Starting items are listed (or an explicit "nothing stored" empty state).
- [ ] Each item shows a display name, count, and weight/stack info.
- [ ] The **value summary** matches the economic ledger's starting value.

**Expected outcome:** `Inventory` is readable. The player knows what they hold on Day 1.

---

### Step 7 — Crafting

- [ ] Open the **Crafting** panel.
- [ ] Available recipes are listed (Day 1 may be a small set).
- [ ] Selecting a recipe shows required ingredients and output.
- [ ] No placeholder data appears in the recipe list or detail view.

**Expected outcome:** `CraftingHostSession` is bound. Recipes are inspectable.

---

### Step 8 — Weather & Radiation

- [ ] Open the **Weather** panel.
- [ ] Current weather condition and forecast are visible.
- [ ] Open the **Radiation** detail panel.
- [ ] Current dose / dose rate is shown.
- [ ] Values are numeric (0 or a Day-1 baseline is fine; placeholder text is not).

**Expected outcome:** `WeatherHostSession` and `RadiationSystem` are live. Environmental pressure is communicated before the player acts.

---

### Step 9 — Radio & Events

- [ ] Open the **Radio** panel.
- [ ] Last intercept or transmission log is visible (Day 1 may be empty — honest empty state required).
- [ ] Open the **Event** detail panel (via dashboard nav or events rail).
- [ ] Active or queued events are listed.
- [ ] No placeholder intercept text.

**Expected outcome:** `RadioHostSession` and `EventHostSession` are bound. The wasteland "speaks" on Day 1.

---

### Step 10 — Journal

- [ ] Open the **Journal** panel.
- [ ] At least the first narrative entry or page is visible.
- [ ] Text is legible and diegetic (in-world voice, not debug labels).
- [ ] Page navigation (if present) does not crash or show blank pages.

**Expected outcome:** `JournalSystem` is live. The player has narrative orientation.

---

### Step 11 — Expeditions

- [ ] Open the **Expedition** panel.
- [ ] Available expedition targets or staging list is visible.
- [ ] Expedition detail view shows survivor assignment slots, risk, and expected yield.
- [ ] No placeholder data in the target list.

**Expected outcome:** `ExpeditionHostSession` is bound. The player can plan their first scouting run.

---

### Step 12 — Research

- [ ] Open the **Research** panel.
- [ ] The research tree or atlas shows available projects.
- [ ] Selecting a project shows cost, duration, and unlock description.
- [ ] No placeholder project names or icon paths.

**Expected outcome:** `ResearchSystem` is bound. Long-term progression is visible.

---

### Step 13 — Advance to Day 2 & Verify Persistence

- [ ] Trigger the **Advance Day** action from the dashboard or shelter HUD.
- [ ] Confirm the confirmation dialog (if present) reads correctly.
- [ ] After advancing, the status label or day counter reads **Day 2**.
- [ ] Re-open Steps 3–6 (Shelter, Status, Inventory, Afflictions) and confirm:
  - [ ] Values have changed from Day 1 (needs decayed, radiation accumulated, or resources consumed — at least one delta is required).
  - [ ] No data is blank/null that was populated on Day 1.
- [ ] **Save** the game.
- [ ] **Reload** the save.
- [ ] Confirm Day 2 state is restored exactly:
  - [ ] Day counter = 2.
  - [ ] Survivor needs match pre-reload values.
  - [ ] Inventory and shelter state match pre-reload values.

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
| 1. Launch | | | | |
| 2. New Game → Day 1 | | | | |
| 3. Shelter | | | | |
| 4. Survivor Status | | | | |
| 5. Afflictions / Medical | | | | |
| 6. Inventory | | | | |
| 7. Crafting | | | | |
| 8. Weather & Radiation | | | | |
| 9. Radio & Events | | | | |
| 10. Journal | | | | |
| 11. Expeditions | | | | |
| 12. Research | | | | |
| 13. Advance Day 2 + Persistence | | | | |
| A1–A3. Advance-day cancel/rapid-click | | | | |
| B1–B3. New game over existing save | | | | |
| C1–C4. Save/load resolutions | | | | |

**Severity:** Blocker / Major / Minor / Cosmetic

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
| `dotnet test` | 3,242/3,242 Core tests (needs, radiation, save round-trips, journal, catalog integrity, determinism). |
| `--ui-snapshot-uitest` | 29/29 visual-regression goldens match at HEAD. |

Manual playthrough exists to catch what automation structurally cannot: feel, timing, edge-case intent (rapid-click, cancel-after-rapid-click, new-game-over-save confirmation wording), and human-visible rendering artifacts that a pixel-diff gate may not flag at the wrong resolution.

---

*Checklist prepared at HEAD `9b687193` + `d3b38385` (dormant sweep). Update the findings log after each playthrough and attach screenshots for any Blocker or Major finding.*
