# Spoiler & Discovery Boundaries

**Date:** 2026-09-01
**Status:** Tier classification complete — Plan 17 gating reference.

---

## Purpose

Define spoiler tiers for all player-discoverable lore content, map unlock conditions, and flag extreme-spoiler design documents that must never leak into runtime.

---

## Spoiler Tier Definitions

| Tier | Name | Definition | Player awareness | Example |
|------|------|------------|------------------|---------|
| **0** | Common knowledge | Every survivor knows this; no discovery needed | Available from game start | "The Exchange happened", "radiation is dangerous", "the bunker has air filtration" |
| **1** | Observable fact | Learnable through normal play without special effort | First 3 days of gameplay | Location layouts, faction names, basic crafting recipes, shelter systems |
| **2** | Earned knowledge | Requires deliberate exploration, quest progress, or NPC trust | Days 3–30 | Faction backstories, quest branching conditions, medical lore, expedition route details |
| **3** | Deep lore | Requires archive transcription, deep-lore location visits, or late-quest unlocks | Days 30–90 | Pre-Exchange political history, Cult of the Cobalt Flame doctrine, Memory Vault contents, faction secret histories |
| **4** | Endgame revelation | Final-act reveals that recontextualize earlier content | Days 90+ / endgame | The List (mystery arc), true nature of the Exchange, player shelter origin, epilogue chronicle conditions |

---

## Content Classification by Tier

### Tier 0 — Common Knowledge

| Source | Entries | Unlock condition |
|--------|---------|------------------|
| `world_history.json` (pre_exchange era) | 20 | Available from start via `JournalCodex` Places tab |
| `faction_lore.json` (display_name, description) | 23 | Visible on faction stance UI |
| `item_description_texts.json` | 184 | Visible on item inspect |
| `holdfast_flavor.json` | 43 | Visible in Holdfast UI panels |
| `feedback_messages.json` | ~200 | System UI (not lore) |

### Tier 1 — Observable Fact

| Source | Entries | Unlock condition |
|--------|---------|------------------|
| `world_history.json` (hour_zero era) | 13 | Discovered via location visit + `LocationMemorySystem` |
| `world_history.json` (black_sky era) | 14 | Discovered via location visit |
| `environmental_texts_expansion_05.json` | 36 | **Currently orphaned** — Plan 17: wire to location entry |
| `environmental_atmosphere_expansion.json` (sensory cues) | ~60 | **Currently orphaned** — Plan 17: wire to location atmosphere |
| `bunker_graffiti_postings.json` | 36 | Bunker location visit |
| `bunker_maintenance_logs_batch_*.json` | ~3 batches | Bunker location visit |
| `journal_voice_prose.json` | 19 categories | Journal system auto-entry |

### Tier 2 — Earned Knowledge

| Source | Entries | Unlock condition |
|--------|---------|------------------|
| `world_history.json` (ashfall era) | 21 | Quest progress + location visits |
| `expedition_field_reports.json` | ~20 | Expedition dispatch + return |
| `expedition_field_reports_batch_2.json` | ~20 | Second expedition cycle |
| `patrol_debriefs.json` | ~15 | Patrol system |
| `survivor_letters_lost_kin.json` | ~10 | Survivor relationship threshold |
| `therapist_session_notes.json` (+ batch 2, 3) | ~30 | Medical system + trust threshold |
| `medical_texts.json` | 84 | Medical ward access |
| `guilt_sources.json` | 20 | Guilt system trigger |
| `confession_secrets.json` | 8 | Confession system unlock |
| `journal_entries_batch_*.json` | 3 batches | Journal auto-entry on event |
| `courier_mission_logs.json` (+ batch 2) | ~20 | Courier quest completion |
| `cobalt_liturgies.json` (+ batch 2) | ~16 | Cult of the Cobalt Flame questline |

### Tier 3 — Deep Lore

| Source | Entries | Unlock condition |
|--------|---------|------------------|
| `world_history.json` (post_exchange era) | 11 | Late-game quest + archive transcription |
| `deep_lore_locations.json` | 10 | `DeepLoreLocationCatalogLoader` — requires `book` item (**currently missing**) |
| `deep_lore_survivor_fields.json` | 4 | Survivor keepsake items (**4 currently missing**) |
| `archive_inks.json` | 3 | Archive desk + ink crafting (`charcoal` item **currently missing**) |
| `oral_lore_codex.json` | 16 songs | **Currently orphaned** — `OralLoreCatalog` has no host consumer |
| `oral_lore_batch_2.json` | 10 entries | **Currently orphaned** |
| `bunker_wiretap_transcripts.json` (+ batch 2) | ~20 | Faction trust threshold + archive transcription |
| `relic_provenance_dossiers.json` | ~15 | Relic discovery + archive analysis |
| `bunker_blueprints_codex.json` | ~10 | Bunker upgrade path + archive |
| `bunker_court_verdicts_codex.json` | ~10 | Bunker governance questline |
| `iron_synod_canons.json` | ~10 | Iron Garrison faction questline |
| `dead_hand_directives.json` | ~5 | Military bunker deep-lore |
| `cobalt_arming_directives.json` | ~5 | Cult militarization |
| `library_manuals.json` | 3 | Library study system |
| `lost_tech_manuals.json` | ~10 | Deep-lore location discovery |
| Narrative technical logs (~80 files) | ~400 | Archive desk transcription |

### Tier 4 — Endgame Revelation

| Source | Entries | Unlock condition |
|--------|---------|------------------|
| `epilogue_chronicle.json` | 5 | Endgame trigger (all placeholder art refs) |
| `final_wishes.json` | 8 | Final wish system (5 required items **missing**) |
| `02_THE_LIST.md` | ~20 concepts | **EXTREME SPOILER — DEV-ONLY** |

---

## EXTREME Spoiler Risk: `02_THE_LIST.md`

**File:** `docs/lore/02_THE_LIST.md` (12.1 KB)

**Risk level:** CRITICAL — contains the entire mystery arc of the game.

**Contents at risk:**
- The true nature and origin of the Exchange
- Player shelter's hidden purpose and origin
- The identity and motivation of key NPCs
- Endgame branching conditions and consequences
- Recontextualization of all prior lore tiers

**Runtime exposure rules:**
1. This file must **never** be loaded by any runtime system.
2. No text from this file may appear in any codex entry, journal entry, environmental text, or archive transcription.
3. Dev documents that reference The List must be classified Tier 4 and excluded from codex candidate extraction.
4. If any Plan 17 codex entry derives from The List, it must be flagged and removed before merge.

---

## Discovery Boundary Rules

### What the player can learn vs. when

| Game day | Max tier accessible | Mechanism |
|----------|--------------------|-----------|
| Day 1 | Tier 0–1 | Starting knowledge + observable environment |
| Day 3 | Tier 1–2 | First expeditions, NPC contact |
| Day 7 | Tier 2 | Quest progress, faction trust |
| Day 14 | Tier 2–3 | Archive transcription begins, deep-lore locations |
| Day 30 | Tier 3 | Full archive access, oral lore, faction secrets |
| Day 60+ | Tier 3–4 | Endgame triggers, final wishes, epilogue |

### Codex tab gating (JournalCodex)

| Tab | Max tier | Gating |
|-----|----------|--------|
| Log | 0–2 | Auto-populated from events |
| Items | 0–1 | Item inspect (always visible) |
| People | 0–2 | Survivor relationship threshold |
| Places | 0–3 | Location visit + archive transcription |
| Events | 0–3 | Quest/event completion |

---

## Regression Risks

1. **Dev document leak:** Automated codex extraction from `docs/lore/*.md` must exclude `02_THE_LIST.md`, `IntelBible.md`, and any file tagged as Tier 4.
2. **Orphan content premature unlock:** When wiring `environmental_atmosphere_expansion.json` and `environmental_texts_expansion_05.json`, ensure Tier 2+ texts are gated behind appropriate discovery conditions.
3. **Archive transcription bypass:** The archive desk must not transcribe Tier 3+ content until the player has met the discovery prerequisite (location visit, quest flag, faction trust).
