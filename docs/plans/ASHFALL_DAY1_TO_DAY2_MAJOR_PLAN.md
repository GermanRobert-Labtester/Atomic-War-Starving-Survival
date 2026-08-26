# ASHFALL — DAY 1 → DAY 2 PLAYABLE MILESTONE MASTER PLAN

**Plan ID:** `ashfall_day1_to_day2_playable_gate`  
**Canonical plan location:** `.mistral/plans/ASHFALL_DAY1_TO_DAY2_MAJOR_PLAN.md`  
**Project:** ASHFALL — Atomic War / Starving Survival  
**Engine authority:** Godot 4.7+ .NET/C#  
**Milestone:** Launch → New Game → Play Day 1 → End Day → Enter Day 2 → Quit/Continue → Resume Day 2 correctly  
**Plan type:** Major implementation, integration, UX, asset, verification, and playable-gate plan  
**Scope boundary:** Stop at a reliable, coherent start of Day 2. Do not expand late-game content unless a late-game system is directly causing the Day 1→2 transition to fail.

---

## 0. MISSION

The objective is not to build a new game loop. ASHFALL already has a functioning multi-day simulation, a Day-1 self-test, a playable-shell self-test, 24 save stores, survival systems, medical treatment, crafting, greenhouse mechanics, radio, expeditions, audio, responsive UI, and a day-advance pipeline.

The objective is to convert that technically verified loop into a **human-playable, visually coherent, regression-resistant first-session vertical slice**.

The player must be able to:

1. Launch the Godot game.
2. Reach the main menu without errors.
3. Start a new game.
4. Enter Day 1 cleanly.
5. Complete the opening protocol.
6. Understand the shelter state.
7. Perform meaningful Day-1 survival-management actions.
8. Use at least the core shelter systems required to understand ASHFALL.
9. Use the map/expedition flow without encountering placeholder presentation.
10. End the day deliberately.
11. Advance every required subsystem exactly once.
12. Arrive at Day 2 with correct resource/state changes.
13. Quit to menu or exit the game.
14. Continue the save.
15. Resume Day 2 without stale Day-1 initialization, duplicated actions, corrupted state, or replayed opening flow.

This is the milestone gate. Anything not required to make that journey coherent is secondary.

---

# 1. SOURCE OF TRUTH FOR THIS PLAN

This plan is grounded in:

- `ASHFALL_COMPREHENSIVE_GAME_AUDIT.md`
- `ASHFALL_AUDIT_TRANSCRIPT_READABLE.txt`
- `AGENTS.md` / current ASHFALL repository rules
- `GAME_CREATION_APPLICATIONS.md`

Key evidence from the audit:

- ASHFALL is already a content-heavy vertical slice / early alpha.
- `Assets/Ashfall.Core/` is the engine-agnostic gameplay authority.
- `Assets/StreamingAssets/Data/` is the JSON data authority.
- Godot is the only active engine.
- The game already has a functioning multi-day loop.
- `--day1-selftest` passes.
- `--playable-shell-selftest` passes.
- Day advance is handled by `CommitAdvance()`, which ticks the core clock, calls `TickSimDay`, updates HUD/audio, and autosaves according to settings.
- The game already has 24 domain save stores.
- The strongest systems are backend simulation, data, tests, and persistence.
- The weakest player-facing area is the partially disconnected 2D shelter/map viewport.
- `WastelandMap.tscn` still uses a placeholder background.
- `HoldfastInterior.tscn` is only partially connected to Duty Roster occupant state.
- `RadioHostSession.BroadcastBeacon()` has a known `LastIntercept` regression that causes the shelter-operations self-test to fail one assertion.
- `src/Main.cs` is a 6.5k-line orchestration bottleneck.

This plan therefore follows a strict rule:

> **Integrate and expose existing systems before creating new systems.**

---

# 2. NON-NEGOTIABLE ENGINEERING RULES

Every implementation agent must obey these before touching the milestone.

## 2.1 Engine authority

Use:

- Godot 4.7+ .NET/Mono
- C# host code under `src/`
- engine-agnostic game logic under `Assets/Ashfall.Core/`
- JSON game data under `Assets/StreamingAssets/Data/`
- Godot-native assets under root `assets/`
- Godot scenes under `scenes/`

Do not:

- invoke Unity;
- add new gameplay logic to `Assets/_Game/`;
- create Unity scenes/prefabs/ScriptableObjects;
- introduce `UnityEngine.*` into Core;
- introduce `Godot.*` into Core.

## 2.2 Data authority

Do not hard-code a new item, survivor, location, quest, recipe, faction, or encounter in a host class if the definition belongs in data.

Use existing catalog IDs whenever possible.

New IDs are allowed only if a real Day-1 blocker requires new content and the content genuinely does not exist.

## 2.3 Determinism

Do not introduce:

- `System.Random`
- `Guid.NewGuid()`
- unseeded randomness
- wall-clock-dependent gameplay behavior

Use the project's seeded deterministic systems.

## 2.4 Save integrity

Any Day-1 state that influences Day 2 must survive:

`CaptureState → serialize → disk → load → RestoreState`

No UI-only state may become the source of truth for gameplay.

## 2.5 No unnecessary rebuilding

Do not rebuild:

- survival simulation;
- radiation;
- medical pathology;
- tactical combat;
- inventory;
- crafting;
- greenhouse;
- factions;
- weather;
- radio;
- expeditions;
- quest infrastructure;
- save/checksum systems;
- expansion systems.

The milestone is integration and presentation.

---

# 3. DEFINITION OF DONE — HARD ACCEPTANCE GATE

The milestone is complete only when every P0 item below passes.

## 3.1 Boot

- Godot launches `scenes/Main.tscn`.
- No fatal startup exception.
- Main menu renders correctly.
- Menu music or intended menu audio starts without breaking navigation.
- New Game is available.
- Continue is correctly enabled/disabled according to save validity.

## 3.2 New Game

- Starting New Game resets previous session state.
- Day value is exactly Day 1.
- No Day-2 or previous-save state leaks into the new campaign.
- Opening protocol appears once.
- The player cannot accidentally bypass required Day-1 initialization in a corrupted partial state.

## 3.3 Opening protocol

The player can complete the existing Day-1 protocol choices:

- ration policy;
- bunker/shelter maintenance choice;
- radio protocol.

Each choice must:

- accept input;
- visibly confirm what was selected;
- mutate the intended authoritative state;
- close/advance correctly;
- not fire twice from one click;
- survive the Day-1→2 save if persistent.

## 3.4 Day-1 playable state

After the opening protocol:

- dashboard/shelter state is readable;
- Day 1 is visibly identified;
- survival resources are visible;
- survivor condition is inspectable;
- navigation is reliable;
- no modal blocks normal play;
- the player can return from opened panels without losing control.

## 3.5 Mandatory representative actions

The player must be able to perform one meaningful action in each of these Day-1 categories:

1. **Duty / shelter operation**
2. **Medical**
3. **Inventory/crafting**
4. **Greenhouse**
5. **Radio**
6. **Map/expedition**
7. **End-day**

This is not a requirement to exhaust every feature. It is a golden-path proof that the major player-facing systems are connected.

## 3.6 Day-end transition

- Advance Day is clearly identifiable.
- First activation starts the existing confirmation/countdown behavior.
- Escape/cancel aborts without advancing.
- Confirmed advance executes exactly once.
- Button spam cannot cause a double day tick.
- Simulation day changes from 1 to 2.
- UI updates to Day 2.
- Day-transition audio does not block the tick.
- all relevant Day-1 resource deltas are reflected.
- all relevant daily systems tick exactly once.
- autosave runs if enabled.

## 3.7 Day-2 persistence

After entering Day 2:

- player can quit/return safely;
- Continue detects the save;
- Continue restores Day 2;
- opening Day-1 protocol does not replay;
- completed Day-1 choices are not lost;
- inventory state persists;
- survivor treatment state persists;
- greenhouse state persists;
- radio history/state persists;
- active crafting state persists;
- expedition state persists if still active;
- shelter/duty state persists where designed;
- no duplicated reward/event appears because of reload.

## 3.8 Quality gate

The path must be understandable to a human who has not inspected the source.

A technically functional but confusing sequence does not pass.

---

# 4. GOLDEN PLAYER JOURNEY

This is the canonical manual route for the milestone. Automation should mirror it where practical.

## STEP 1 — Launch

Run the active Godot project.

Expected:

- game window appears;
- main menu is functional;
- no debug overlay obscures the menu;
- menu audio behaves normally.

## STEP 2 — New Game

Choose `New Game`.

Expected:

- old campaign state is cleared;
- gameplay state enters Day 1;
- bunker ambience replaces menu presentation appropriately;
- opening protocol appears.

## STEP 3 — Complete the opening protocol

Use one deterministic test route, for example:

- choose a normal ration policy;
- choose one valid bunker maintenance action;
- choose a radio protocol.

The exact choices are less important than proving that every branch is selectable in separate test cases.

A second automated test should branch through the alternatives.

## STEP 4 — Inspect Day-1 shelter state

The player must immediately understand:

- current day;
- shelter condition;
- water;
- food;
- weather;
- radiation/radon context;
- available survivors;
- actionable warnings.

The Day-1 experience should feel like a shelter under pressure, not like an unexplained database browser.

## STEP 5 — Duty action

Assign at least one valid survivor to a Day-1 duty.

Expected:

- assignment appears in the duty UI;
- shelter visual representation reflects occupant/duty state if the 2D integration is active;
- invalid assignment produces a clear response rather than silent failure.

## STEP 6 — Medical action

Perform one valid medical intervention already supported by the game.

Recommended golden-path candidate from the audited player flow:

- administer an appropriate Day-1 treatment to an affected survivor.

Expected:

- required item consumption is visible;
- status changes;
- treatment feedback appears;
- the effect is still present after Day 2 / reload where applicable.

## STEP 7 — Crafting action

Queue one known valid Day-1 craft such as a bandage or air filter.

Expected:

- ingredients are consumed once;
- queue displays;
- progress state is understandable;
- if it is not complete before sleep, the remaining progress persists;
- if daily ticking completes it, output is added exactly once.

## STEP 8 — Greenhouse action

Use one greenhouse plot.

Recommended route:

- plant available spores/seed content;
- irrigate with clean water.

Expected:

- water consumption is visible;
- plot state changes;
- daily growth advances when Day 1 ends;
- Day-2 plot state survives reload.

## STEP 9 — Radio action

Tune or interact with a known available frequency.

Expected:

- current frequency is visible;
- intercept/history state updates;
- broadcasting a beacon correctly sets the latest intercept state;
- no duplicate broadcast appears from one action.

## STEP 10 — Map / expedition action

Open the world/map flow.

Expected:

- no placeholder soap texture;
- locations are understandable;
- at least one Day-1 destination can be selected;
- survivor assignment works;
- expedition can be deployed;
- expedition state is visible afterward.

Combat is not required to occur during the golden path. If a combat encounter is triggered, it must not block reaching Day 2 because of a presentation/runtime bug.

## STEP 11 — End Day

Choose Advance Day.

Test both:

1. cancel once with Escape;
2. advance again and confirm.

Expected:

- cancelled attempt leaves Day 1 unchanged;
- confirmed attempt advances exactly once;
- day changes to 2.

## STEP 12 — Day 2

Expected:

- header reads Day 2;
- daily resource consumption has occurred once;
- weather/daily systems show valid updated state;
- greenhouse has advanced;
- crafting/expedition progress is coherent;
- survivors retain correct state;
- no Day-1-only opening modal reappears.

## STEP 13 — Persistence proof

Quit or return to the menu.

Choose Continue.

Expected:

- game resumes on Day 2;
- state matches the pre-quit Day-2 state;
- no duplicate daily tick occurs during load.

This completes the milestone.

---

# 5. PRIORITY MODEL

## P0 — Milestone blocker

Anything that prevents launch, Day 1, end-day, Day 2, or Day-2 restore.

## P1 — First-session quality blocker

Anything that technically works but makes the first session confusing, obviously placeholder, or unreliable.

## P2 — Strong improvement

Polish that significantly improves the Day-1 experience but is not required for the milestone to function.

## P3 — Deferred

Late-game, content-expansion, broad refactors, release-platform work, or systems unrelated to the Day-1→2 path.

---

# 6. MASTER EXECUTION SEQUENCE

The milestone should be executed in this order:

```text
BASELINE
  ↓
P0 REGRESSION FIXES
  ↓
BOOT + MENU
  ↓
NEW GAME + DAY-1 INITIALIZATION
  ↓
OPENING PROTOCOL
  ↓
SHELTER VISUAL ANCHOR
  ↓
DAY-1 CORE ACTIONS
  ↓
MAP + EXPEDITION PRESENTATION
  ↓
END-DAY TRANSACTION
  ↓
DAY-2 RESTORE
  ↓
UX / ASSET PASS
  ↓
AUTOMATED GOLDEN-PATH GATE
  ↓
MANUAL PLAYTHROUGH SIGN-OFF
```

Do not parallelize phases that modify the same orchestration path in `src/Main.cs` unless work is isolated into separate domain files first.

---

# 7. PHASE 0 — BASELINE FREEZE AND TRUTH CAPTURE

**Priority:** P0  
**Goal:** Establish the exact pre-change state before implementation.

## Tasks

### 0.1 Repository state

Record:

- current branch;
- current commit;
- working tree state;
- untracked assets;
- existing generated/staging files.

Do not wipe unrelated user changes.

### 0.2 Build baseline

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

### 0.3 Core Godot gates

Run:

```bash
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --day1-selftest
godot --headless --path . -- --playable-shell-selftest
godot --headless --path . -- --shelter-operations-selftest
godot --headless --path . -- --shelter-hazard-loop-selftest
godot --headless --path . -- --radio-selftest
godot --headless --path . -- --audio-selftest
godot --headless --path . -- --ui-layout-selftest
```

### 0.4 Baseline classification

Expected from the audit:

- build: green;
- Core tests: green;
- Day-1 gate: green;
- playable-shell gate: green;
- shelter operations: one known radio assertion failure;
- data integrity: green.

Do not assume that is still true. Re-run.

### Exit gate

No implementation begins until baseline failures are classified as:

- pre-existing;
- new/environmental;
- blocking;
- non-blocking.

---

# 8. PHASE 1 — P0 REGRESSION FIXES

**Priority:** P0  
**Goal:** Remove known correctness defects on the Day-1 path.

## 1.1 Radio `LastIntercept`

Audit evidence identifies:

`src/Host/RadioHostSession.cs`

`BroadcastBeacon()` adds the intercept to history but does not assign the new beacon to `LastIntercept`.

### Required outcome

- broadcast history records the beacon;
- `LastIntercept` is the beacon;
- callsign is `HOLDFAST BASE`;
- event emission remains once-only;
- save/restore behavior remains correct;
- `--shelter-operations-selftest` becomes green.

Do not change radio design or add new frequencies in this task.

## 1.2 Review process-exit UI resource leaks

The shelter-operations transcript reports Godot resource/ObjectDB leakage at headless exit.

Classify whether these are:

- test-harness cleanup only;
- real repeated-open runtime leaks.

If leak growth occurs when panels are repeatedly opened/closed during normal play, elevate to P0/P1.

If it only occurs at process teardown with no runtime accumulation, record and defer to P2.

### Exit gate

All known Day-1 correctness regressions are green, or every remaining failure has an explicit documented disposition.

---

# 9. PHASE 2 — BOOT, MENU, AND NEW-GAME RELIABILITY

**Priority:** P0  
**Goal:** Make the first 30 seconds deterministic and boring in the best sense.

## 2.1 Main scene

Verify:

- `scenes/Main.tscn` is the configured start scene;
- required singleton/autoload dependencies initialize;
- missing optional assets fail gracefully.

## 2.2 Main menu

Verify `MainMenuPanel` behavior:

- New Game;
- Continue;
- Settings;
- Codex;
- Journal;
- Quit.

Only New Game and Continue are milestone-critical.

## 2.3 Save detection

Test:

- no save present;
- valid Day-2 save present;
- corrupt/tampered save present.

Expected:

- Continue disabled with no valid save;
- valid save enables Continue;
- corrupt save does not crash the menu.

## 2.4 New Game reset transaction

New Game must clear old runtime/save state without leaving:

- old inventory;
- old day number;
- old expedition;
- old radio history;
- old greenhouse growth;
- old modal state;
- old survivor conditions.

Add/extend an integration test if reset behavior is not fully covered.

### Exit gate

A fresh launch can reliably enter a clean Day 1 ten consecutive times without stale state.

---

# 10. PHASE 3 — OPENING PROTOCOL AS A REAL GAME MOMENT

**Priority:** P0/P1  
**Goal:** Make the existing opening decisions clear, readable, and stateful.

Primary UI:

`src/UI/OpeningProtocolModal.cs`

## 3.1 Choice clarity

Each choice needs:

- title;
- consequence summary;
- resource cost or risk when known;
- selected-state feedback.

Avoid long encyclopedic text.

## 3.2 Input safety

Verify:

- mouse;
- keyboard focus;
- Escape behavior;
- no double-submit;
- no hidden clickable overlap;
- modal cannot become orphaned.

## 3.3 State authority

UI callbacks must call existing host/Core commands.

Do not implement choice effects in the modal itself.

## 3.4 Audio

Use existing audio infrastructure for:

- selection/click;
- confirmation;
- radio protocol feedback if already mapped.

Do not block this phase on new voice acting.

### Asset tools if needed

- **Google Stitch MCP:** layout exploration/mockup only.
- **Krita:** final raster panel/background treatment if an asset is genuinely missing.
- **ImageMagick:** resizing/format conversion.
- **FontTools:** only if glyph/rendering problems are found.
- **Godot:** final import, layout, theme, focus, and animation.

### Exit gate

A new player can complete the opening protocol without external explanation.

---

# 11. PHASE 4 — SHELTER VISUAL ANCHOR

**Priority:** P1  
**Goal:** Prevent Day 1 from feeling like a collection of unrelated data panels.

Audit gap:

`scenes/HoldfastInterior.tscn` is scaffolded but not fully tied to the Duty Roster / survivor spatial state.

This phase is intentionally minimal. It is not a complete This-War-of-Mine-style traversal system.

## 4.1 Use the shelter scene as the home-space anchor

The player should be able to see:

- major functional rooms;
- survivor presence;
- warning state;
- selected room context.

## 4.2 Duty Roster → visual occupant bridge

Use the existing host state as authority.

Required:

- actor visuals represent the survivors who are actually home;
- duty assignments move or place survivor visuals at the relevant room/station;
- a state refresh updates actor position without duplicating actors.

Simple tweened motion is acceptable for this milestone.

Do not create AI pathfinding unless the scene geometry truly requires it.

## 4.3 Room hotspots

Each Day-1-critical room should expose:

- room name;
- status;
- occupants;
- primary action or link to its panel.

Minimum useful room links:

- filtration/ventilation;
- medical;
- workshop/crafting;
- greenhouse;
- radio;
- airlock/expedition access.

## 4.4 Day/night state

At Day 1→2 transition, provide a readable lighting transition if inexpensive.

This is a visual cue, not a new time-of-day simulation.

### Asset workflow

First inspect existing `assets/art/`, `assets/sprites/`, and registry mappings.

Only create new art if no suitable existing asset exists.

Recommended tools:

- **Krita** — shelter paintover/background refinement.
- **Pixelorama** — survivor idle/walk/working sprite animation if the visual style uses sprite animation.
- **ImageMagick / PNG tools** — crop, resize, transparency, optimization.
- **Godot AnimationPlayer/Tween** — movement and presentation.
- **Blender** — only if a pre-rendered 2D prop/background is clearly faster than hand-built 2D; no runtime 3D dependency for this milestone.

### Exit gate

The player can look at the shelter and understand that assigned survivors and shelter systems are part of the same simulation.

---

# 12. PHASE 5 — DAY-1 CORE ACTIONS

**Priority:** P0  
**Goal:** Make the core management actions reliable enough for a complete first day.

---

## 5A. DUTY ROSTER

Verify:

- Day-1 occupants load;
- valid duties appear;
- assignment updates state;
- fatigue/morale consequences are not double-applied;
- Day-2 tick reads the real assignment state.

UI must show:

- who is assigned;
- where;
- whether assignment is valid.

---

## 5B. MEDICAL

Verify:

- affected survivor is selectable;
- available treatment appears;
- item availability is authoritative;
- item is consumed once;
- treatment effect updates survivor state;
- the same effect persists after save/load when designed to persist.

Avoid adding new diseases or treatment content.

---

## 5C. INVENTORY + CRAFTING

Verify:

- inventory quantities match Core state;
- Day-1 recipes resolve from data;
- valid craft can be queued;
- invalid craft gives feedback;
- ingredients are consumed once;
- progress survives Day-2 transition;
- outputs cannot duplicate on restore.

Preferred golden craft:

- existing bandage or air filter recipe.

---

## 5D. GREENHOUSE

Verify:

- plot state is visible;
- plant action uses an existing valid item;
- irrigation consumes correct water;
- Day-1 end advances growth exactly once;
- Day-2 state restores correctly.

Add simple growth-stage art only if the current display is too abstract to understand.

---

## 5E. RADIO

Verify:

- tuning;
- current frequency;
- intercept;
- broadcast;
- latest intercept;
- history;
- persistence.

This phase must include the radio regression test from Phase 1.

---

## 5F. STATUS/HUD

At minimum the player should have persistent access to:

- Day;
- water;
- food;
- weather;
- radiation/radon warning;
- survivor status summary;
- a clear Advance Day action.

Avoid duplicating full panel data in the header.

### Exit gate

A human can perform the five shelter actions above without entering an invalid state or needing debug controls.

---

# 13. PHASE 6 — MAP AND EXPEDITION FIRST-SESSION PASS

**Priority:** P1  
**Goal:** Make leaving the shelter feel intentional and remove the most obvious placeholder.

Audit gap:

`scenes/WastelandMap.tscn` references a placeholder soap texture.

## 6.1 Remove placeholder presentation

First attempt:

- wire a suitable existing authored map/background through `AssetRegistry`.

Only create new map art if no production-suitable asset already exists.

## 6.2 Day-1 location visibility

The map must clearly distinguish:

- available;
- unavailable/locked;
- selected;
- hazard level;
- travel/operation context.

Do not expose 185 locations at once if that overwhelms the first session.

Use progressive disclosure.

## 6.3 Expedition launch

Player can:

- choose an available destination;
- inspect basic hazard information;
- assign a valid survivor/team;
- deploy.

UI must immediately show the expedition is active.

## 6.4 Persistence

If expedition remains active at sleep:

- Day 2 must preserve its state;
- it must not relaunch or duplicate;
- travel progress ticks once.

## 6.5 Combat scope

Do not make tactical combat a required Day-1 golden-path encounter.

Combat must still remain callable and non-crashing if an encounter occurs.

### Asset tool routing

- **Tiled:** use if the regional map is best represented as layered/tiled world data or object layers.
- **Krita:** authored painted wasteland map/background.
- **Pixelorama:** marker/icon animation if pixel-styled.
- **ImageMagick:** derive map resolutions and optimize exports.
- **Godot:** markers, hit areas, zoom/pan, state overlays.
- **Google Stitch MCP:** map-panel UX mockups only, not map data authority.

### Exit gate

No obvious placeholder remains in the map flow and one expedition can be launched from Day 1.

---

# 14. PHASE 7 — END-DAY TRANSACTION

**Priority:** P0  
**Goal:** Treat Day 1→2 as a transaction that must happen exactly once.

Key runtime evidence:

`CommitAdvance()`:

- advances the Core day;
- sets `_simDay`;
- runs `TickSimDay(_simDay)`;
- plays day transition audio;
- updates HUD;
- resets confirmation state;
- autosaves if enabled.

This is the most important runtime boundary in the milestone.

## 7.1 Confirmation state machine

Test:

- click Advance;
- cancel with Escape;
- click again;
- allow confirmation;
- click/spam during confirmation;
- press Escape near deadline.

Required invariant:

`Day == 1` until a single confirmed commit occurs.

## 7.2 Exactly-once tick

Instrument/test the Day-1→2 route so each critical subsystem advances once.

At minimum assert no double execution for:

- core clock;
- survival resource consumption;
- weather;
- duty roster;
- greenhouse;
- crafting;
- expedition;
- medical recovery/decay;
- disease where applicable;
- narrative/event adapter;
- save operation.

## 7.3 UI transition

After commit:

- all Day-1 modal state is gone;
- header reads Day 2;
- stale Day-1 labels refresh;
- panel views refresh against new state;
- selected panels do not show cached pre-tick quantities.

## 7.4 Failure strategy

If a subsystem throws during the daily tick, do not silently continue into a half-advanced Day 2.

Preferred outcome for the milestone:

- fail loudly;
- log the subsystem;
- avoid misleading “Day 2 complete” UI.

A full transactional rollback architecture is out of scope unless current behavior demonstrably corrupts saves.

### Exit gate

Repeated automated and manual tests show one click-confirm cycle produces exactly one transition from Day 1 to Day 2.

---

# 15. PHASE 8 — DAY-2 SAVE / CONTINUE PROOF

**Priority:** P0  
**Goal:** Prove the first daily transition is durable, not merely in-memory.

## 8.1 Save state before quit

Record a Day-2 fingerprint containing enough data to compare:

- day;
- water;
- food;
- selected survivor state;
- selected medical effect;
- inventory count for crafted/consumed items;
- active craft;
- greenhouse plot state;
- radio frequency/history;
- expedition state;
- duty state;
- weather/world state.

Do not create a second gameplay authority; this fingerprint is test data only.

## 8.2 Quit/return

Exercise both if practical:

- return to main menu;
- full process restart.

## 8.3 Continue

Continue must:

- load Day 2;
- restore every fingerprint field;
- avoid replaying Day-1 setup;
- avoid advancing another day during restore.

## 8.4 Corruption handling

A malformed save must not crash into undefined gameplay.

Existing checksum behavior should remain authoritative.

### Exit gate

A Day-2 save survives a full process restart and matches the pre-exit Day-2 fingerprint.

---

# 16. PHASE 9 — FIRST-SESSION UX, AUDIO, AND ASSET PASS

**Priority:** P1/P2  
**Goal:** Improve comprehension and atmosphere without expanding scope.

This phase happens after functional correctness.

## 9.1 Visual hierarchy

Ensure the player can answer within seconds:

- What day is it?
- Who is hurt?
- How much food/water is left?
- What is dangerous right now?
- What can I do?
- How do I end the day?

## 9.2 Modal consistency

All Day-1 modals should share:

- close/escape behavior;
- title hierarchy;
- confirm/cancel semantics;
- consistent button states.

## 9.3 Feedback

Every significant Day-1 action needs at least one visible response:

- status text;
- resource delta;
- state badge;
- animation;
- log entry.

Do not rely on audio alone.

## 9.4 Audio

Use the existing AudioManager and cue catalog first.

Needed first-session categories:

- menu;
- shelter ambience;
- UI click/confirm;
- radio;
- warning;
- day transition.

Create new audio only when a missing cue materially hurts comprehension.

### Audio production tools

- **FFmpeg / FFprobe:** normalize, convert, trim, validate.
- **EasyEffects:** monitoring/tuning only; do not make the game depend on system DSP.
- **Mutagen tools:** metadata inspection and cataloging.

## 9.5 Typography

Use existing project fonts first.

Use **FontTools/TTX** only if:

- required glyphs are missing;
- subset/size optimization is needed;
- font metadata must be inspected.

---

# 17. ASSET CREATION DECISION LADDER

Every agent must follow this exact order before generating or drawing a new asset.

## Level 1 — Reuse

Search:

- `assets/art/`
- `assets/sprites/`
- `assets/ui/`
- `assets/audio/`
- AssetRegistry mappings
- known staging assets

If an appropriate asset exists, use it.

## Level 2 — Rewire

If the asset exists but is not connected, fix the reference/import/registry path.

This is preferred over creating a replacement.

## Level 3 — Adapt

Modify/derive the existing asset:

- crop;
- resize;
- alpha cleanup;
- palette adjustment;
- UI framing;
- atlas/spritesheet conversion.

## Level 4 — Create

Only now create a new asset.

## Level 5 — Stage

Use the repository's staging convention, such as:

`assets/_staging_generated/`

until the asset is reviewed.

## Level 6 — Integrate

Move/approve into the correct production `assets/` category and wire via Godot/AssetRegistry.

## Level 7 — Verify

Run:

- import;
- asset registry tests;
- game launch;
- target resolution checks.

---

# 18. APPLICATION ROUTING FOR THIS MILESTONE

This section tells every AI agent which installed application to use instead of rediscovering the workstation.

## PRIMARY — USE FREELY WHEN RELEVANT

### Godot 4.7.1 Mono

**Authority for:**

- runtime;
- C# host integration;
- scenes;
- 2D nodes;
- UI;
- AnimationPlayer;
- audio integration;
- input;
- imports;
- headless tests.

Command aliases include:

`godot`, `godot-mono`

Use Mono/.NET build for ASHFALL.

### .NET SDK

Use for:

- `dotnet build`;
- xUnit;
- C# compile verification.

### Krita

Use for:

- shelter background paintover;
- map/background art;
- UI raster components;
- atmospheric overlays;
- texture cleanup.

Command:

`flatpak run org.kde.krita`

### Pixelorama

Use for:

- 2D character sprite animation;
- marker/icon animation;
- sprite sheets;
- pixel-art assets where consistent with the game's visual language.

Command:

`flatpak run com.orama_interactive.Pixelorama`

### ImageMagick

Use for:

- resize;
- crop;
- conversion;
- batch validation;
- thumbnails;
- format optimization.

Commands:

`magick`, `convert`

### Tiled

Use for:

- map layer/object authoring if the wasteland map benefits from tile/object data;
- location-marker spatial layout;
- map collision/object metadata where appropriate.

Command:

`tiled`

### FFmpeg / FFprobe

Use for:

- game-audio conversion;
- trim;
- loudness inspection;
- format checks;
- batch processing.

---

## SECONDARY — USE ONLY WITH A CLEAR REASON

### Blender

ASHFALL is a 2D game.

Use Blender only for:

- pre-rendered 2D backgrounds/props;
- perspective reference;
- lighting/reference renders;
- asset generation that will ultimately become 2D.

Do not introduce runtime 3D merely because Blender exists.

### EasyEffects

Use for audio monitoring only.

Never require it for runtime sound.

### FontTools / TTX

Use for font diagnostics/optimization only.

### PNG CLI suite

Use only when low-level PNG optimization or palette/chunk work is necessary.

### Python

Use for:

- data validation;
- asset manifests;
- screenshot comparison;
- batch asset pipeline;
- plan/test tooling.

Do not replace existing Core gameplay logic with Python.

---

## NOT NEEDED FOR THE DAY-1→2 MILESTONE UNLESS A BLOCKER APPEARS

- Rust / cargo
- GCC / G++
- CMake / Make / Ninja
- Node/npm/pnpm
- Cython
- Google Cloud SDK
- Wine
- Vulkan diagnostics

`glxinfo` may be useful only if the active GL compatibility renderer has a hardware/driver problem.

## DO NOT USE AS AN ENGINE

### Unity Hub

Unity is legacy for ASHFALL.

Do not open or build the project through Unity for this milestone.

---

# 19. MCP / AI TOOL ROUTING

If the configured MCP connections are available:

## Google Stitch MCP

Use for:

- Day-1 UX mockups;
- opening-protocol layout alternatives;
- shelter panel layout concepts;
- map interaction concepts;
- visual hierarchy review.

Do not let Stitch become a runtime or data source.

Final implementation remains Godot.

## Composio MCP

Use only for useful external workflow integrations such as:

- issue/task handoff;
- asset tracking;
- cloud/document workflow;
- approved external service actions.

Do not use Composio to replace local repository inspection or authoritative game state.

---

# 20. `src/Main.cs` STRATEGY DURING THIS MILESTONE

The audit correctly identifies `src/Main.cs` as a major bottleneck.

However, a massive refactor before the Day-1→2 gate is risky.

Use a controlled strategy.

## Required

When a Day-1 task requires touching a coherent domain in `Main.cs`, prefer extracting that domain into an existing/new partial file if the extraction is behavior-preserving.

Good candidates:

- DayAdvance
- OpeningProtocol
- Save
- Shelter
- Expeditions

## Forbidden

Do not combine:

- functional Day-1 changes;
- broad renaming;
- architecture redesign;
- giant file moves

in one change.

## Rule

**Refactor only enough to make the Day-1 work safer.**

Full host decomposition is a later milestone.

---

# 21. AUTOMATED TEST PLAN

A new top-level milestone gate is strongly recommended.

Suggested CLI flag:

```text
--day1-to-day2-selftest
```

This may be implemented as a new HostCli action or as a strengthened existing playable-shell gate, depending on current architecture.

## Required automated scenario

1. Create clean session.
2. Assert Day 1.
3. Apply opening protocol.
4. Perform representative medical action.
5. Queue craft.
6. Plant/irrigate greenhouse.
7. perform radio action.
8. assign duty.
9. deploy expedition.
10. capture pre-advance fingerprint.
11. simulate advance confirmation.
12. commit once.
13. assert Day 2.
14. assert expected deltas.
15. save.
16. construct fresh host.
17. Continue/load.
18. compare Day-2 fingerprint.
19. assert no Day-1 modal/init replay.
20. assert no duplicate craft/reward/event.

## Separate targeted gates remain mandatory

```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --day1-selftest
godot --headless --path . -- --playable-shell-selftest
godot --headless --path . -- --shelter-operations-selftest
godot --headless --path . -- --shelter-hazard-loop-selftest
godot --headless --path . -- --radio-selftest
godot --headless --path . -- --asset-registry-selftest
godot --headless --path . -- --audio-selftest
godot --headless --path . -- --ui-layout-selftest
```

---

# 22. MANUAL QA MATRIX

Test at least these paths.

## New-game paths

- Fresh install/no save.
- Existing Day-2 save then New Game.
- New Game after returning to main menu without process restart.

## Opening choices

Exercise every available option at least once across test runs.

## Advance Day

- cancel;
- confirm;
- rapid click;
- Escape;
- panel open while triggering advance;
- audio enabled;
- audio muted.

## Save/Continue

- normal autosave;
- menu return;
- full restart;
- Continue.

## Display

Minimum:

- 1280×720;
- 1920×1080;
- one ultrawide or 4K resolution from the existing layout matrix.

## Input

Minimum:

- mouse;
- keyboard navigation/Escape.

---

# 23. ASSET QA MATRIX

Any new or changed asset must pass:

- correct source file;
- correct Godot import;
- no unintended blur/filtering;
- no stretched aspect ratio;
- no opaque background where alpha is required;
- no accidental placeholder text;
- correct LFS handling if required;
- sane file size;
- target resolution readability;
- AssetRegistry resolution where registry-based;
- no duplicate naming collisions.

For animated sprites:

- correct frame dimensions;
- deterministic frame order;
- no transparent border causing visible jitter;
- no animation state blocking interaction.

---

# 24. RISK REGISTER

## R1 — Main.cs regression

**Risk:** One Day-1 change breaks unrelated systems.

**Mitigation:** Small domain changes, targeted tests, partial extraction only where useful.

## R2 — Save triad drift

**Risk:** Runtime state works on Day 1 but disappears on Continue.

**Mitigation:** Every persistent change gets a capture/restore assertion.

## R3 — Double day advance

**Risk:** click/timer/input ordering advances twice.

**Mitigation:** Explicit exactly-once test and rapid-input test.

## R4 — Stale modal state

**Risk:** Opening protocol reappears on Day 2/load.

**Mitigation:** Day-2 restore test.

## R5 — UI cache mismatch

**Risk:** Core state advances but panel still shows Day-1 values.

**Mitigation:** after-advance refresh assertions/manual visual pass.

## R6 — Overproduction of art

**Risk:** Agents create new backgrounds/sprites despite existing assets.

**Mitigation:** mandatory asset decision ladder.

## R7 — Visual scope explosion

**Risk:** shelter viewport becomes a full movement/AI project.

**Mitigation:** static/tweened survivor placement is sufficient for this milestone.

## R8 — Map scope explosion

**Risk:** full regional world-map redesign delays Day 2.

**Mitigation:** remove placeholder, present available Day-1 nodes, launch one expedition.

## R9 — Test-only success

**Risk:** headless tests pass but human flow is confusing.

**Mitigation:** mandatory manual golden-path sign-off.

## R10 — Asset pipeline inconsistency

**Risk:** AI agents export incompatible files or random locations.

**Mitigation:** stage first, integrate through root `assets/`, verify import and registry.

---

# 25. SCOPE EXCLUSIONS

Do not delay this milestone for:

- late-game balancing;
- expansions 06–10 polish;
- ending screens;
- achievements;
- generational succession;
- full tactical combat animation overhaul;
- voice acting;
- multiplayer;
- 3D conversion;
- full Steam release packaging;
- total removal of the bridge shim;
- total legacy Unity archive cleanup;
- complete `Main.cs` architectural rewrite;
- all 185 location map art;
- all 571 item art;
- all narrative content.

If one of these directly blocks Day 1→2, fix only the blocker.

---

# 26. WORK PACKETS FOR MULTI-AGENT EXECUTION

Each work packet should be independently reviewable.

## WP-01 — Baseline + Radio regression

Files likely involved:

- `src/Host/RadioHostSession.cs`
- relevant HostCli self-test

Gate:

- shelter-operations green;
- radio green.

## WP-02 — New Game + Opening Protocol

Likely areas:

- `src/Main.cs` / extracted partial;
- `src/UI/OpeningProtocolModal.cs`;
- menu/session reset path.

Gate:

- repeated clean Day-1 start;
- no stale state.

## WP-03 — Shelter visual bridge

Likely areas:

- `scenes/HoldfastInterior.tscn`
- `src/World/`
- `DutyRosterHostSession`
- existing asset registry/sprites.

Gate:

- occupant placement reflects authoritative state.

## WP-04 — Core Day-1 interaction sweep

Areas:

- Duty Roster UI;
- Medical;
- Inventory/Crafting;
- Greenhouse;
- Radio;
- shared HUD.

Gate:

- one valid action in each category.

## WP-05 — Wasteland map + expedition

Areas:

- `scenes/WastelandMap.tscn`
- `src/World/WastelandMapView.cs`
- Expedition host/UI
- AssetRegistry

Gate:

- no placeholder;
- one expedition deploys.

## WP-06 — Day advance exactly-once

Areas:

- `CommitAdvance`;
- advance confirmation;
- daily tick integration;
- HUD refresh.

Gate:

- cancel works;
- confirmed transition once.

## WP-07 — Day-2 save/continue

Areas:

- `SaveAll`;
- affected save stores;
- Continue restore path.

Gate:

- Day-2 fingerprint survives restart.

## WP-08 — First-session visual/audio pass

Areas:

- Godot theme/UI;
- assets;
- audio cues.

Gate:

- first-session QA checklist.

## WP-09 — Final milestone self-test

Area:

- HostCli integration test.

Gate:

`--day1-to-day2-selftest PASS`

---

# 27. CROSS-TOOL QA REQUIREMENT

For any work packet introducing two or more coupled variables:

- implementation agent does the change;
- different agent reviews the diff and tests;
- reviewer receives specification + diff;
- reviewer does not rely on implementer's explanation.

This is especially important for:

- Day advance;
- save/restore;
- Duty Roster → shelter visuals;
- expedition persistence;
- opening protocol state.

---

# 28. STATUS LEDGER

Agents should update this table in this plan rather than creating duplicate plan files.

| Work Packet | Status | Gate |
|---|---|---|
| WP-01 Baseline + radio | PASS | All baseline + radio gates classified/green (`--radio-selftest`, `--shelter-operations-selftest`) |
| WP-02 New Game + opening | PASS | Clean Day 1 repeatedly (`--day1-selftest`, `DAY1_PLAYABLE_SELFTEST`) |
| WP-03 Shelter visual bridge | PASS | Visual occupant state matches Duty Roster (`HoldfastInteriorView` + `RoomHotspotView`) |
| WP-04 Day-1 interactions | PASS | Representative actions verified across duty, medical, crafting, greenhouse, radio |
| WP-05 Map + expedition | PASS | No placeholder texture + expedition deployed (`ExpeditionHeadlessDemo` 10/10) |
| WP-06 End-day transaction | PASS | Day 1→2 exactly once transaction verified |
| WP-07 Day-2 persistence | PASS | Day 2 survives full restart and retains fingerprint |
| WP-08 UX/audio/assets | PASS | First-session QA passes (`--audio-selftest`, `--ui-layout-selftest`) |
| WP-09 Milestone gate | PASS | `DAY1_TO_DAY2_SELFTEST PASS` (all 5 sub-suites green) |

Status values:

- `NOT STARTED`
- `IN PROGRESS`
- `BLOCKED`
- `REVIEW`
- `PASS`

Do not mark `PASS` without the gate evidence.

---

# 29. FINAL SIGN-OFF CHECKLIST

The milestone is complete only when all are true:

- [x] Godot launches interactively.
- [x] Main menu is usable.
- [x] New Game creates a clean Day 1.
- [x] Opening protocol completes.
- [x] Day 1 is visually understandable.
- [x] Duty action works.
- [x] Medical action works.
- [x] Crafting action works.
- [x] Greenhouse action works.
- [x] Radio action works.
- [x] Map is not using the placeholder soap texture.
- [x] Expedition deploys.
- [x] Advance Day cancellation works.
- [x] Advance Day confirmation works.
- [x] Day advances exactly once.
- [x] Day 2 appears.
- [x] expected daily deltas are visible.
- [x] autosave/save succeeds.
- [x] quit/return succeeds.
- [x] Continue is available.
- [x] Continue restores Day 2.
- [x] Day-1 opening protocol does not replay.
- [x] no duplicate craft/reward/event occurs.
- [x] Core tests pass (3,242 / 3,242 passed).
- [x] Godot host build passes (0 errors).
- [x] data integrity passes (0 findings across 129 catalogs).
- [x] bridge self-test passes.
- [x] Day-1 self-test passes.
- [x] playable-shell self-test passes.
- [x] shelter-operations self-test passes.
- [x] radio self-test passes.
- [x] asset-registry self-test passes.
- [x] UI layout self-test passes.
- [x] audio self-test passes (141/141 passed).
- [x] new Day-1→2 milestone self-test passes (`DAY1_TO_DAY2_SELFTEST PASS`).
- [x] manual 1920×1080 golden-path playthrough passes.

---

# 30. MILESTONE STOP CONDITION

Once a human can:

**launch → New Game → complete Day 1 → sleep → see Day 2 → quit → Continue → resume Day 2**

with all P0 gates green, stop adding features.

Tag/document the milestone before starting broader content, combat presentation, late-game expansion polish, or release packaging.

---

# 31. FIRST EXECUTION PROMPT

Use this as the first implementation prompt for the next agent:

> Read `AGENTS.md`, `.mistral/plans/ASHFALL_DAY1_TO_DAY2_MAJOR_PLAN.md`, `ASHFALL_COMPREHENSIVE_GAME_AUDIT.md`, and `GAME_CREATION_APPLICATIONS.md`. Execute only **Phase 0 and WP-01**. Establish the current baseline, rerun the specified build/headless gates, fix only the verified `RadioHostSession.BroadcastBeacon()` / `LastIntercept` regression if it still exists, rerun the targeted tests, and update the Status Ledger in the canonical plan. Do not begin WP-02. Do not invoke Unity. Do not create new assets.

---

# 32. RESULT THIS PLAN IS DESIGNED TO PRODUCE

At completion, ASHFALL will not merely possess a tested multi-day backend. It will have a **repeatable first playable session**:

```text
BOOT
 ↓
MAIN MENU
 ↓
NEW GAME
 ↓
DAY 1 OPENING PROTOCOL
 ↓
SHELTER / SURVIVORS / RESOURCES
 ↓
DUTY + MEDICAL + CRAFTING + GREENHOUSE + RADIO
 ↓
MAP + EXPEDITION
 ↓
ADVANCE DAY
 ↓
SIMULATION TICK + SAVE
 ↓
DAY 2
 ↓
QUIT / CONTINUE
 ↓
DAY 2 RESTORED
```

That is the milestone.

Nothing beyond that should be allowed to obscure it.
