# ASHFALL Data Gap Audit

> Generated from repository state at `de1c0a1` + working-tree sweep (2,932 uncommitted files).
> Baseline: `--data-integrity-selftest` PASS (0 errors, 3,600 IDs, 102 catalogs).

## 1. Method

Counted entries in every top-level JSON catalog, identified C# consumers per catalog,
cross-referenced orphan files (data with no loader), identified thin catalogs
(few entries relative to system expectations), and checked `schema_version` coverage.

## 2. Catalog Density Map (Top 20)

| Entries | File | Consumer |
|---|---|---|
| 499 | items.json | ItemCatalogLoader + many systems |
| 194 | questline_master.json | **ORPHAN** — no C# loader |
| 105 | locations.json | LocationCatalog + expeditions + travel |
| 102 | survivors.json | SurvivorSystem + panels |
| 79 | world_history.json | WorldHistorySystem (INDIRECT) |
| 77 | events.json | EventSystem |
| 72 | expansion_survivor_fields.json | **ORPHAN** |
| 68 | door_encounters.json | DoorEncounterSystem |
| 67 | expansion_item_tags.json | **ORPHAN** |
| 66 | year_of_ash_locations.json | YearOfAshCatalogLoader |
| 57 | year_of_ash_items.json | YearOfAshCatalogLoader |
| 52 | year_of_ash_events.json | YearOfAshCatalogLoader |
| 50 | year_of_ash_radio.json | YearOfAshCatalogLoader |
| 50 | radio.json | RadioSystem (INDIRECT, 66 refs) |
| 43 | duty_roster_marks.json | DutyRosterCatalog (>=5 asserted) |
| 40 | holdfast_items.json | HoldfastCatalog |
| 38 | standing_record_memory.json | StandingRecordCatalog (>=30 asserted) |
| 38 | holdfast_locations.json | HoldfastCatalog |
| 36 | year_of_ash_survivors.json | YearOfAshCatalogLoader |
| 36 | characters.json | CharacterSystem |

## 3. Orphan Catalogs — 24 Files With No C# Consumer

These JSON files exist in the data authority but **no C# code loads them**. They are
dead content: written, possibly authored with care, but invisible to the game.

### Critical orphans (large content, clearly intended for gameplay)

| File | Entries | What it contains | Why it matters |
|---|---|---|---|
| questline_master.json | 194 | Master quest graph (entries + schema_version) | The largest single content file in the repo. 194 quest entries with zero runtime reachability. |
| expansion_survivor_fields.json | 72 | belief_profile_id, keepsake, phantom_background, profession per survivor | Rich character depth data, unused. |
| expansion_item_tags.json | 67 | item_id → tags mapping | Tag-based filtering/crafting/trade hooks, unused. |
| echoes.json | 23 | Narrative echoes with choices/conditions/minDay | Story content with player choices, unused. |
| guilt_sources.json | 20 | choice_pattern, severity, description, title | Moral weight system data, unused. |
| faction_war_events.json | dict (chains) | Faction war event chains | Faction conflict narrative, unused. |
| faction_war_journal.json | dict (entries) | Journal entries for faction war | Faction war journaling, unused. |
| faction_war_radio.json | dict (broadcasts) | Radio broadcasts for faction war | Faction war radio, unused. |
| faction_war_dialogue.json | dict (snippets) | Dialogue snippets | Faction war dialogue, unused. |
| faction_war_communiques.json | dict (communiques) | Official communiques | Faction war flavor, unused. |
| narrative_arc_events.json | 15 | Narrative events with choices/weight/minDay | Story events, unused. |

### Moderate orphans (small but complete content)

| File | Entries | What it contains |
|---|---|---|
| final_wishes.json | 8 | archetype_id, buff_id, completion_text, morale_bonus, steps |
| confession_secrets.json | 8 | archetype_id, forgiveness/grudge outcomes |
| narrative_questlines.json | 4 | survivor-linked questlines with stages |
| trade_specialties.json | 4 | profession mastery bonuses and narrative |
| cassette_sets.json | 4 | Collectible cassette sets with hidden caches |
| radio_distress_signals.json | 5 | Traceable distress signals with knowledge_points |
| wall_carving_templates.json | 3 | Morale-band-gated carving templates |
| damaged_map_zones.json | 3 | Map fragments revealing hidden installations |
| deep_lore_survivor_fields.json | 4 | belief/keepsake/phantom/profession per survivor |
| antigravity_survivor_fields.json | 11 | manifesto_law_code + stance per survivor |

### Minor orphans

| File | Entries | What it contains |
|---|---|---|
| dynamic_questlines.json | 2 | Procedural questline templates |
| epilogue_chronicle.json | dict (slides) | Epilogue slide definitions |
| trade_screen_scenarios.json | dict (scenarios) | Trade screen test scenarios |

## 4. Thin Catalogs — Loaded but Underpopulated

These have C# consumers but very few entries, limiting system variety.

| File | Entries | Consumer | Minimum Asserted | Gap |
|---|---|---|---|---|
| disease_catalog.json | 4 | DiseaseCatalog | >= 4 (exactly met) | **At floor.** One disease per category; no room for variety or progression. |
| weather_seasons.json | 3 | WeatherSystem | — | Only 3 seasons defined. If the game has 4+ seasonal transitions, some are hardcoded or missing. |
| dose_locations.json | 3 | DoseContentCatalog | — | 3 locations for the dose ledger system. Thin exploration loop. |
| dose_items.json | 5 | DoseContentCatalog | — | 5 items for a system about radiation tracking and chelation. |
| dose_quests.json | 4 | DoseContentCatalog | — | 4 quests. |
| utility_actions.json | 4 | UtilityAiSystem | == 4 (exactly met) | **At floor.** Only 4 AI actions. The utility AI system is severely content-starved. |
| autopsy_procedures.json | 3 | AutopsySystem | — | 3 procedures. Medical depth limited. |
| archive_inks.json | 3 | ArchiveDeskSystem | — | 3 ink types. Writing/archival variety minimal. |
| narrative_encounters.json | 3 | NarrativeEncounterSystem | == 2 (exceeded) | 3 encounters for the narrative encounter system. |
| dive_sites.json | 4 | DiveSiteCatalog | — | 4 dive sites for the maritime system. |
| verdict_locations.json | 4 | VerdictCatalogLoader | — | 4 locations for the Verdict expansion. |
| verdict_npcs.json | 6 | VerdictNpcSystem | — | 6 NPCs. |
| relic_recipes.json | 6 | (INDIRECT) | — | 6 relic recipes. |
| phantom_triggers.json | 7 | (INDIRECT) | — | 7 phantom trigger types. |
| library_manuals.json | 3 | LibraryStudySystem | — | 3 manuals. Knowledge system has almost no content. |
| holdfast_flavor.json | 3 | HoldfastFlavorCatalog | — | 3 flavor entries. |
| crossing_factions.json | 3 | CrossingCatalog | — | 3 factions for the Crossing expansion. |
| holdfast_factions.json | 3 | HoldfastCatalog | — | 3 factions for the Holdfast expansion. |
| standing_record_factions.json | 1 | StandingRecordCatalog | — | **1 faction.** Standing Record is nearly faction-less. |

## 5. Schema Version Coverage

**44/102 files** have `schema_version`. **58 files lack it.**

AGENTS.md flags this as a known issue ("Only 35 of ~280 JSON files have schema_version"
— the count has improved to 44 but 58 remain). Files without `schema_version` cannot
participate in versioned migration and their format is implicit.

Key files missing `schema_version` that are actively loaded:
- `items.json` (499 entries, the core item authority)
- `locations.json` (105 entries)
- `survivors.json` (102 entries)
- `recipes.json`
- `events.json` (77 entries)
- `radio.json` (50 entries)
- `faction_lore.json` (19 entries)
- `characters.json` (36 entries)
- All holdfast/crossing/dose/duty_roster/year_of_ash item+location+quest files

## 6. Findings Summary

### Finding A — 24 orphan catalogs (24% of top-level JSON)

**Impact:** Dead content. These files were authored (some with `schema_version`, some
without) but never wired to a loader. They represent completed creative work that is
invisible to players. The largest single file in the data authority
(`questline_master.json`, 194 entries) is orphaned.

**Recommendation:** For each orphan, either:
1. Write a Core catalog loader + host wiring + selftest (brings content to life), or
2. Move to `docs/deprecated_data/` if the content is superseded (removes dead weight).

The `faction_war_*` cluster (5 files) looks like a coherent planned feature that was
never wired. The `expansion_*` cluster (2 files, 139 entries) looks like character/
item enrichment that was authored but never connected.

### Finding B — utility_actions.json and disease_catalog.json at exact floor

Both are asserted at their exact count (`== 4`, `>= 4`). This means the selftest
passes but there is zero headroom. Adding one entry to either catalog would require
updating the assertion; removing one would break the build. These are content-starved
systems.

**Recommendation:** Add 2-4 entries to each to create headroom, then relax assertions
to `>= N` with margin.

### Finding C — standing_record_factions.json has 1 entry

A system called "Standing Record" with 1 faction is functionally faction-less. Either
the system is designed for a single faction (in which case the catalog is fine) or
it was intended to have multiple standing relationships.

**Recommendation:** Check the StandingRecord system's design intent. If multi-faction,
this is a content gap. If single-faction, the catalog is correct.

### Finding D — 58 files missing schema_version

Including the most critical files: `items.json`, `locations.json`, `survivors.json`,
`recipes.json`, `events.json`. These are the backbone of the data authority and they
have no version field, meaning any format change to them is undetectable at load time.

**Recommendation:** Add `schema_version` to all 58 files. This is mechanical:
add `"schema_version": 1` to the root object (or to each entry for list-format files,
matching the existing convention per domain).

### Finding E — Faction_war cluster is a complete unwired feature

Five files (`faction_war_events`, `faction_war_journal`, `faction_war_radio`,
`faction_war_dialogue`, `faction_war_communiques`) all have `schema_version` and
structured content. They form a coherent faction-war narrative system with events,
journal entries, radio broadcasts, dialogue, and official communiques. This is a
complete feature waiting for a Core system + host session + panel.

**Recommendation:** This is the highest-value orphan cluster. If the faction war
system is planned, wiring these five files brings a complete narrative layer to life
with zero new content authoring.

## 7. Recommended Action Order

1. **Wire `questline_master.json`** — 194 quest entries is the largest single content
   pool in the game. Even partial wiring (read + validate + expose to quest system)
   unlocks enormous content.
2. **Wire the `faction_war_*` cluster** — 5 files, complete feature, zero authoring
   needed.
3. **Wire `expansion_survivor_fields.json` + `expansion_item_tags.json`** — 139 entries
   of character/item enrichment.
4. **Wire `echoes.json` + `narrative_arc_events.json`** — 38 narrative events with
   player choices.
5. **Add `schema_version` to the 12 most critical files** — items, locations,
   survivors, recipes, events, radio, faction_lore, characters, and the four largest
   expansion catalogs.
6. **Add headroom to `utility_actions.json` and `disease_catalog.json`** — 2-4 new
   entries each.
7. **Deprecate or wire the small orphans** — `trade_screen_scenarios`,
   `dynamic_questlines`, `epilogue_chronicle` may be superseded.

## 8. Evidence Index

- Catalog counts: `python3` JSON parse of all 102 top-level files.
- Consumer mapping: `grep -rl` for filename strings in `Assets/Ashfall.Core/` and `src/`.
- Orphan detection: zero C# hits for filename or stem.
- Minimum assertions: `grep` for `Check(` in `*HeadlessDemo.cs` files.
- Schema version: `python3` scan for `schema_version` key in root or first list entry.
- Data integrity: `godot --headless --path . -- --data-integrity-selftest` PASS.
