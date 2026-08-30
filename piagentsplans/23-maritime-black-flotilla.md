# Plan 23 — Maritime & Black Flotilla: The Drowned Coast

> **Theme:** The sea. The Black Flotilla / Maritime expansion has dive systems, stealth, a
> procedural scavenge engine, and 24 items — but only 4 dive sites and thin faction texture.
> This plan makes the coast a full content region.
>
> **Key evidence (verified):** `black_flotilla_items.json` = 24; `dive_sites.json` = 4;
> `currents.json` = 17; Core `Maritime/` holds `MaritimeDiveSystem`, `StealthDiveInstance`,
> `ProceduralScavengeSystem`, `PsychologicalContaminationSystem`, `SafeCrackingSystem`,
> `VariableLootNode`, `DiveSiteCatalog` — all live. `District8DeepCoastSystem` (667 lines) live.

---

## Task 23A — Black Flotilla faction & item depth

**Goal:** Flesh out the Black Flotilla as a faction (fleet culture, ranks, trade goods, codes)
so the drowned coast has a society, not just dive sites.

**Files:** `black_flotilla_items.json`, `faction_lore.json`, `faction_radio_corpus.json`,
`characters.json` (flotilla NPCs), read-only: locate the flotilla faction engine + `FactionRadioEngine`.

**Substeps:**
1. Locate the flotilla faction system/loader and read its schema (ranks, fleets, codes, trade).
2. Read the 24 flotilla items to learn the faction's material culture (marine, salvage, code-ribbon).
3. Author the flotilla's structure: 3 fleets (salvage, escort, deep-dive) with distinct dispositions toward the player.
4. Author 12 new flotilla items (diving gear, salvage tools, code-ribbons, preserved sea rations, a ship's bell) across trade/quest/gear roles.
5. Author 6 named flotilla NPCs (a fleet-master, a dive-chief, a code-keeper, a deserter) for 20B's NPC layer.
6. Author 8 flotilla radio broadcasts (`faction_radio_corpus.json` / flotilla band) — coded, maritime, terse.
7. Give the flotilla a standing track and a trade specialty (marine salvage) via `FactionStanceEngine`/`TradeSpecialtySystem`.
8. Validate ids; data-integrity selftest; narrative-continuity.
9. xUnit: faction load, standing track, trade specialty, radio broadcast delivery.
10. Maritime/flotilla selftest green.

**Next steps:** flotilla alliance vs. blockade arc (16C treaties); a flotilla quest to raise a
specific wreck (23B); code-ribbon collectibles that decrypt flotilla radio (11B).

---

## Task 23B — Dive-site & wreck expansion (4 → 14)

**Goal:** Fill the dive layer — use `MaritimeDiveSystem`, `StealthDiveInstance`,
`SafeCrackingSystem`, and `VariableLootNode` to author 10 new dive sites with distinct mechanics.

**Files:** `dive_sites.json`, `black_flotilla_items.json` (dive gear), `locations.json`
(underwater `loc_*`), read-only `MaritimeDiveSystem.cs`, `StealthDiveInstance.cs`,
`SafeCrackingSystem.cs`, `VariableLootNode.cs`, `DiveSiteCatalog.cs`.

**Substeps:**
1. Read the dive/stealth/safe-crack/variable-loot systems to learn the per-site schema (depth, air, noise, loot nodes, safes).
2. Map the 4 existing sites' mechanics to find unused features (e.g. `SafeCrackingSystem` may have no site using it).
3. Author 3 wreck dives (a sunken submarine — registry suggestion, a cargo hulk, a warship) with air-supply and noise constraints.
4. Author 3 structure dives (flooded metro, drowned refinery, submerged bunker) with tight-space stealth.
5. Author 2 safe-cracking dives (a bank vault, a munitions safe) that finally exercise `SafeCrackingSystem`.
6. Author 2 deep/hazard dives (psychological-contamination + narcosis-analog pressure) using `PsychologicalContaminationSystem`.
7. Give each site `VariableLootNode` tables weighted to flotilla/relic/dossier loot (23A, 04, 15B).
8. Validate ids; data-integrity selftest; maritime selftest.
9. xUnit: air/noise constraint enforcement, safe-crack path, variable loot determinism (`ISeededRng`), psych-contamination accrual.
10. Balance sim: dive risk vs. reward; dive-gear condition (10C) gates the deep sites.

**Next steps:** a "deepest wreck" capstone dive; dive-crew injuries feeding 09 medical; a wreck
that is a faction-war grave (06C) with a memorial choice (12C).

---

## Task 23C — Currents, tides & coastal dynamics

**Goal:** Make `currents.json` (17 entries) and the deep-coast system drive dynamic coastal
gameplay: currents that help/hazard, tides that open/close dives, and storm surges.

**Files:** `currents.json`, `dive_sites.json` (tide windows), coastal `locations.json`,
read-only `CurrentsCatalog.cs`, `District8DeepCoastSystem.cs`, `WeatherSystem` (storm surge).

**Substeps:**
1. Read `CurrentsCatalog` + `District8DeepCoastSystem` to learn how currents/tides affect travel and dives.
2. Map the 17 currents to coastal routes; identify which aid travel (fair current) vs. hazard (rip).
3. Author tidal windows for 6 dive sites (23B): a wreck only diveable at slack tide — a timing decision.
4. Author 4 current-riding travel bonuses (a fair current cuts coastal travel time) and 3 rip-current hazards.
5. Author 3 storm-surge crisis events (ties to 19C seasons + 13C weather crises) that flood coastal sites temporarily.
6. Wire surge/flood state to the 11C map-evolution system so a flooded site shows as changed.
7. Author 6 coastal flavor texts (17A) for tide/surge states.
8. Validate ids; data-integrity selftest; maritime selftest.
9. xUnit: tide window gating, current travel modifier, surge event state change, determinism.
10. xUnit/save: tide/surge state captured & restored.

**Next steps:** a tide-table item the player can acquire (removes guesswork); a storm-surge
that strands an expedition (rescue mission); coastal foraging at low tide (13A goods).
