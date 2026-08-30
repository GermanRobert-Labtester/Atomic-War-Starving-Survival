# Plan 11 — World & Exploration: Deep Strata, Cipher Hunts & Living Geography

> **Theme:** Make the wasteland itself a content engine. Excavation, signal intelligence, and
> location memory are all implemented and barely used; the map has 261 nodes but static
> geography. This plan turns exploration into a renewable discovery loop.
>
> **Key evidence:** `ExcavationSystem` (depth/shoring/cave-ins) used only for starting rooms
> (registry §20); `SignalIntelligenceCatalog` has cipher data with no interactive hook;
> `LocationMemorySystem`/`LocationEvolutionSystem` exist; `damaged_map_zones.json` present.

---

## Task 11A — Deep-strata excavation expeditions

**Goal:** Turn the excavation system into a proper expedition tier: buried Cold War command
vaults and unmapped caverns, with cave-in and mold hazards (pairs with 9A's spore disease).

**Files:** new excavation-site data (extend `expeditions.json` or a `excavation_sites.json` —
confirm loader pattern first), `locations.json` (subterranean `loc_*`), read-only
`ExcavationSystem.cs`, `ExpeditionSystem.cs`.

**Substeps:**
1. Read `ExcavationSystem` (depth calc, shoring, cave-in rolls) to learn what a "site" needs.
2. Read how expeditions declare destinations in `expeditions.json` to reuse the dispatch grammar.
3. Author 5 excavation sites: a collapsed command vault, a utility tunnel network, a buried metro interchange, a mine shaft, a pre-war archive bunker.
4. Give each: depth profile, shoring material cost (timber/steel — existing items), cave-in risk curve, and a loot table weighted to relics (feeds Plan 04 reverse-engineering).
5. Wire the spore-mold disease (9A) as a depth hazard for unventilated digs.
6. Add per-site discovery text and a journal unlock on first breach.
7. Ensure excavation consumes existing resources and labor (duty-roster shifts) — no new currencies.
8. Validate ids; data-integrity selftest.
9. xUnit: depth/cave-in determinism, shoring cost application, loot-table seeding.
10. Expedition selftest + save round-trip for in-progress digs.

**Next steps:** procedural deep-vault generation (registry suggestion, later); excavation-uncovered echoes (06B).

---

## Task 11B — Cipher & number-station treasure hunts

**Goal:** Activate `SignalIntelligenceCatalog` (cipher dictionaries, signal logs, wiretap
transcripts) as interactive multi-stage quests that reward radio mastery with hidden locations.

**Files:** `signal intelligence` data catalogs, `questline_master.json` (new quest entries),
`radio.json` (number-station broadcasts), read-only `SignalIntelligenceCatalog.cs`,
`RadioTuner.cs`, `QuestlineSystem.cs`.

**Substeps:**
1. Read `SignalIntelligenceCatalog` to inventory existing cipher dictionaries/logs/transcripts and their structure.
2. Read `RadioTuner` to see what a "signal lock" yields and whether frequency data is exposed to quests.
3. Design the decode loop: intercept number-station broadcast → log the number groups → match against a cipher dictionary (found as loot) → derive coordinates → new `loc_*` appears on map.
4. Author 3 number-station broadcasts (eerie, restrained, procedural — no real-world station references).
5. Author 3 cipher-dictionary items as discoverable loot (tied to excavation/relic finds).
6. Author 3 hidden bunker `loc_*` destinations with unique loot/lore payoffs.
7. Wire the whole chain as questline entries with `flag_` progression (broadcast heard → dictionary held → decoded → location revealed).
8. Confirm flags are produced and consumed (no orphans) via dialog-graph lint.
9. Data-integrity selftest + narrative-continuity check.
10. xUnit: decode-step flag progression; coordinate-to-location resolution.

**Next steps:** the radio-oscilloscope mini-game (white space #16) becomes the *tactile* front-end for this loop.

---

## Task 11C — Living geography: map evolution & route blockades

**Goal:** Use `LocationEvolutionSystem` + `damaged_map_zones.json` to make the 261-node map
change over time — blockades, territorial shifts, degradation — so routes must be re-planned.

**Files:** `damaged_map_zones.json`, `locations.json`, faction-war location overrides (06C),
read-only `LocationEvolutionSystem.cs`, `WastelandMapSystem.cs`, `LandmarkDegradationSystem.cs`.

**Substeps:**
1. Read `LocationEvolutionSystem` + `LandmarkDegradationSystem` to learn what map mutation is already supported.
2. Inventory `damaged_map_zones.json` — how zones are declared and what they currently affect.
3. Define 4 evolution event types: route blockade (faction checkpoint), territory flip (war outcome), site degradation (lootable → stripped), hazard bloom (rad hotspot).
4. Author 10 evolution events keyed to days and to faction-war outcomes (06C flags).
5. Wire territory flips to warlord doctrine territory (10A) so the war visibly redraws the map.
6. Ensure blockades force route re-planning in `WastelandMapSystem` pathing (or surface as a travel warning if pathing is static — check).
7. Reflect evolution in the map panel (marker state) and in location override text (06C resolver).
8. Persist evolution state via the world save section (confirm `WorldHostSave` sub-fields capture it).
9. Data-integrity + world selftests; save round-trip for evolved map state.
10. xUnit: evolution triggers fire on day/flag, map state mutates deterministically, restore reproduces it.

**Next steps:** player counter-play (clear a blockade via combat or bribe — uses 10A/10B);
dynamic fog-of-war re-fogging on territory flip.
