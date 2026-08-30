# Plan 17 — Environmental Storytelling & Lore: Atmosphere, Documents & Gazetteer

> **Theme:** The *texture* of the world — what players read and absorb. There's a strong base
> (272 narrative docs, 152 environmental texts, 79 history entries) but thin archives, a tiny
> ink system, and lore docs that never reach the player. This plan makes the world legible.
>
> **Key evidence (verified):** `environmental_atmosphere_expansion.json` = 152 texts;
> `archive_inks.json` = **3 inks**; `docs/lore/01_GAZETTEER.md`, `03_LOCATIONS.md`,
> `04_ENCOUNTERS.md`, `IntelBible.md` exist but are dev-side; `world_history.json` = 79;
> `deep_lore_locations.json` = 10; `ArchiveDeskSystem`, `JournalCodex`, `LocationMemorySystem` live.

---

## Task 17A — Environmental text & atmosphere pass (per-location)

**Goal:** Ensure every visitable location has authored atmospheric text that reacts to its
state, so no two visits to the same ruin read identically.

**Files:** `environmental_atmosphere_expansion.json` (extend), `environmental_texts_expansion_05.json`,
`locations.json`, read-only `LocationMemorySystem.cs`, `LocationEvolutionSystem.cs`.

**Substeps:**
1. Read how environmental texts bind to locations (by `loc_*`, by tier, by tags) in the loader.
2. Map which of the 115 locations / 16A map nodes lack any atmospheric text.
3. Author state-aware text variants: first-visit, revisited, post-loot, post-strike (ties to 06C overrides).
4. Write 30 new texts for the top-exposure locations (faction hubs, quest sites, war sites) in the cold, restrained house voice (`ashfall-write`).
5. Write sensory variety: sight, sound, smell, temperature — never purple prose, always concrete.
6. Author 8 weather-reactive texts (the same location under fallout storm vs. clear cold).
7. Key text selection to `LocationMemorySystem` (has the player been here? what happened?).
8. Validate ids; data-integrity selftest; narrative-continuity (no contradiction with location lore).
9. xUnit: text selection by visit-state and weather; no orphan location refs.
10. Confirm rendering in the location/expedition panel (snapshot-diff if text surface changed).

**Next steps:** graffiti that references recent player events (12B bunker postings → surface version);
voice-over for the 10 most important (07B).

---

## Task 17B — Document & archive catalog expansion (inks 3 → 12)

**Goal:** Expand the discoverable-document layer (letters, manifests, logs, ledgers) and the
`ArchiveDeskSystem` ink/ transcription mechanic into a collectible lore economy.

**Files:** `archive_inks.json`, narrative docs (`documents_batch_2.json` family),
`world_history.json`, read-only `ArchiveDeskSystem.cs`, `ArchiveInkCatalogLoader.cs`,
`JournalCodex.cs`.

**Substeps:**
1. Read `ArchiveDeskSystem` + `ArchiveInkCatalogLoader` to learn the ink/transcription mechanic and how documents unlock.
2. Read 3–4 existing narrative docs to lock format and voice.
3. Author 9 new inks (lampblack, iron-gall, berry, soot, chemical, blood — grounded, each with a transcription cost/quality).
4. Author 15 new discoverable documents across types: a merchant's ledger, a soldier's unsent letter, a maintenance log, a death register, a child's workbook, a ration chit ledger.
5. Pin each document to a real location/expedition loot table (11A digs, 10C dives).
6. Make 5 documents *evidence* for the Verdict (15B) and 3 *cipher dictionaries* (11B).
7. Ensure transcription consumes an ink + time (existing mechanic) and unlocks a JournalCodex entry.
8. Validate ids; data-integrity selftest.
9. xUnit: ink consumption, document transcription unlock, codex entry creation.
10. Narrative-continuity across the 272-doc corpus (dates, names, factions).

**Next steps:** a "completed archive" milestone (transcribe N documents); documents as trade goods for scholar NPCs.

---

## Task 17C — Gazetteer & Intel Bible → in-game codex

**Goal:** Surface the rich dev-side lore (`docs/lore/`) to players as an in-game gazetteer/codex,
so worldbuilding that's currently invisible becomes a discovery reward.

**Files:** new/extended codex data (JournalCodex-backed JSON), `world_history.json`,
`deep_lore_locations.json`, read-only `JournalCodex.cs`, `docs/lore/01_GAZETTEER.md`,
`03_LOCATIONS.md`, `05_FACTIONS.md`, `IntelBible.md` (source only).

**Substeps:**
1. Read `JournalCodex` to learn how codex entries are structured and unlocked.
2. Read the `docs/lore/` files; extract player-safe world facts (no dev commentary, no spoilers of twist content).
3. Design codex categories: Regions, Factions, History, Fauna/Flora, Technology, The Exchange (the war).
4. Author 40 codex entries by converting gazetteer/lore content into in-world reference text (a survivor's almanac, not a wiki).
5. Gate entries behind discovery (visit a place, meet a faction, find a document) via existing unlock flags.
6. Author 10 "deep lore" entries for `deep_lore_locations.json` sites that only unlock on reaching those hard locations.
7. Ensure no real countries/wars/people — use the fictional Meridian Compact / Northern Coalition canon (`DataRuleComplianceTests` gates this).
8. Validate ids/flags; data-integrity selftest; run `DataRuleComplianceTests` green.
9. xUnit: codex unlock by flag, category paging, entry rendering.
10. Snapshot-diff the Journal/Codex panel; approve new golden image.

**Next steps:** codex completion % as a soft collect-a-thon; a printed "survivor's almanac" as
a New Game+ heirloom (15C).
