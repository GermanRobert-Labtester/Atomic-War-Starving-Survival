# Plan 29 — The Shelter as a Character: Rooms, Machines & Decay

> **Theme:** The bunker itself as a lived-in, aging *character*. There's deep machinery (power,
> water, air, foundry, ventilation) but the shelter's *spatial and material* story — its rooms,
> its wear, its history — is thin. This plan makes the home feel old, specific, and loved.
>
> **Key evidence (verified):** `PowerGridSystem`, `WaterTreatmentSystem`, `VentilationSystem`,
> `MaterialShieldingSystem`, `ExcavationSystem` live; 272 narrative docs incl.
> `bunker_maintenance_glitches.json`, `bunker_blueprints_codex.json`,
> `bunker_graffiti_postings.json`; shelter decor is white space #3 (12C builds the mechanic).

---

## Task 29A — Room identity & shelter history

**Goal:** Give each shelter room a name, a history, and a story so the bunker is a place with a
past, not a grid of workstations.

**Files:** shelter/room data, narrative docs (extend `bunker_blueprints_codex.json`,
`bunker_maintenance_glitches.json`), read-only `ShelterAssignmentSystem`, `StartingLevelSystem`.

**Substeps:**
1. Read `ShelterAssignmentSystem` + `StartingLevelSystem` to enumerate the actual rooms and their functions.
2. Read `bunker_blueprints_codex.json` to learn the bunker's canonical layout/origin (what was it before?).
3. Author a name + one-line history for each room (the Ward was a storeroom; the Foundry was the vehicle bay) surfaced on hover/inspection.
4. Author 8 room-history vignettes (who died here, what was built here, a pre-war remnant) discoverable by examining a room.
5. Author 6 "original fixture" details per major room (a stenciled warning, a faded mural, a bolted-down pre-war object) as inspectable lore.
6. Wire room discovery to the codex (17C) and to phantom memory (21A — a room that "remembers").
7. Author the bunker's origin reveal (what it was built for, by whom) as a multi-part archive thread (17B).
8. Validate ids; data-integrity selftest; narrative-continuity with the blueprint codex.
9. xUnit: room history unlock, codex entry, no orphan refs.
10. Snapshot-diff any room-inspection UI.

**Next steps:** room-specific decor (12C) keyed to history; a "restore the room" renovation
arc; the bunker's original AI/logs as a Verdict thread (15B).

---

## Task 29B — Machine personality & maintenance decay

**Goal:** Give the shelter's machines *character* — named, aging, with quirks — so maintenance
is caretaking, and `bunker_maintenance_glitches.json` becomes lived reality.

**Files:** `bunker_maintenance_glitches.json` (extend), machine/system state, read-only
`PowerGridSystem`, `WaterTreatmentSystem`, `VentilationSystem`, `SilentFoundrySystem`,
`DutyRosterSystem` (maintenance shifts).

**Substeps:**
1. Read the glitch codex + the major systems to map which machines exist (generator, filters, pumps, cupola, fans).
2. Name the key machines (the generator "Old Reliable," the main filter "the Lung") — a light, humanizing touch consistent with the tone.
3. Author 10 quirks (the generator coughs on cold starts, the Lung rattles when the filter's due, a pump that needs a percussive tap) keyed to real condition state.
4. Wire quirks to actual maintenance state (a dirty filter → the Lung's rattle → a tell before failure) so quirks are *diagnostic*, not cosmetic.
5. Author 8 glitch events from the codex (a light that won't die, a door that seals itself, a phantom draft) — some harmless, some a real fault.
6. Add a "learning the machine" skill thread (26B) — a survivor who knows Old Reliable's moods gets a maintenance bonus.
7. Wire a beloved machine's final failure to a small grief/memorial beat (21A/12C).
8. Validate ids; data-integrity selftest.
9. xUnit: quirk reflects condition state, glitch event fires, maintenance resolves the tell.
10. Audio hook: give each machine a distinct loop/cue (07B) so quirks are *heard*.

**Next steps:** a machine-whisperer specialist; cannibalizing one machine to save another
(a real triage choice); a "she's still running" epilogue line (15A).

---

## Task 29C — Shelter wear, decay & renovation arcs

**Goal:** Model the shelter's slow decay and the player's renovation choices — the long-arc
material story of keeping a home alive.

**Files:** shelter condition data, `items.json` (repair materials), read-only
`MaterialShieldingSystem`, `ExcavationSystem` (expansion), `SkyLayerArmorSystem`, `DutyRosterSystem`.

**Substeps:**
1. Read `MaterialShieldingSystem` + `SkyLayerArmorSystem` to learn existing degradation (roof armor, shielding) — build on it, don't duplicate.
2. Author a wear model for the *rest* of the shelter (walls, seals, wiring, bunks) as condition state if not present (check — may be a small Core extension; if so flag it).
3. Author 8 decay events (a weeping wall, a corroded seal, a wiring short, a sagging bunk) with repair choices.
4. Author repair material sinks (sealant, scrap, wiring, timber) using existing items + 22A foundry products.
5. Author 4 renovation arcs (turn the storeroom into a proper ward; insulate the bunk room; a real kitchen) — multi-day projects via duty roster.
6. Author the trade-offs: renovate (labor + materials + disruption) vs. live with decay (morale/health cost).
7. Wire renovation completion to room morale (12C decor synergy) and codex (29A).
8. Validate ids; data-integrity selftest.
9. xUnit: decay accrual, repair resolves, renovation project advances by labor, condition saved.
10. Balance sim: decay must pressure without a death spiral; cross-tool QA (condition×labor×materials).

**Next steps:** a grand renovation endgame (make it a true home — epilogue line 15A); decay
during a siege (19B strikes accelerate it); a "the shelter outlived us" melancholy beat.
