# Runtime Lore Provenance Taxonomy

**Date:** 2026-09-01
**Status:** Classification complete — baseline reference for Plan 17.

---

## Purpose

Every piece of lore in ASHFALL has a provenance — a reason it exists in-world and a reliability profile. This document classifies every major lore source by provenance type so that runtime systems (codex, archive, environmental text, journal) can present content with appropriate framing.

---

## Provenance Types

| Tier | Type | Reliability | Examples | Runtime framing |
|------|------|-------------|----------|-----------------|
| **A** | Canonical public record | High — official, verifiable | `world_history.json`, `bunker_blueprints_codex.json` | Presented as fact; no attribution needed |
| **B** | Restricted institutional | High within institution, biased | `faction_lore.json`, `bunker_wiretap_transcripts.json`, `cobalt_arming_directives.json` | Attributed to faction/institution; player sees one side |
| **C** | Eyewitness / field report | Variable — subjective, sensory | `expedition_field_reports.json`, `patrol_debriefs.json`, `survivor_letters_lost_kin.json` | First-person voice; may contradict other sources |
| **D** | Propaganda / ideological | Low factual value, high cultural value | `stencil_propaganda_smear_logs.json`, `currents_pamphlets.json`, `bunker_graffiti_postings.json` | Framed as partisan; player must judge |
| **E** | Personal document | High emotional value, narrow scope | `journal_entries_batch_*.json`, `unsent_letters_batch_2.json`, `therapist_session_notes.json` | Intimate voice; reveals character, not world-state |
| **F** | Material evidence | High — physical, non-verbal | `bunker_maintenance_logs.json`, `equipment_failure_logs.json`, `lead_wall_degradation_logs.json` | Presented as data/logs; no narrative voice |
| **G** | Rumor / oral tradition | Low — degraded, mythologized | `oral_lore_codex.json` (16 songs), `oral_lore_batch_2.json` (10 entries), `geophone_hymnals.json` | Attributed to "they say" / "old song"; may contain kernels of truth |
| **H** | Developer-only | N/A — not in-world | `ASH_FALL_CREATIVE_FRAMEWORK.md`, `IntelBible.md`, `02_THE_LIST.md` | **Never shown to player**; design authority only |

---

## Source Classification

### World History (`world_history.json` — 79 entries)

**Provenance: A (Canonical public record)**

Official timeline of events from 10 years before the Exchange through 5 years after. All entries treated as in-world fact. The mixed `loc_`/`location_` prefix issue (7 entries use `location_`, 5 use bare `player_shelter`) is a data-authority defect, not a provenance concern.

### Faction Lore (`faction_lore.json` — 23 entries)

**Provenance: B (Restricted institutional)**

Each entry represents a faction's self-description or doctrine. Inherently biased — factions describe themselves favorably. The 4 bare-name entries (`iron_garrison`, `ash_militia`, `cult_of_ash_sign`, `warlords_sector_4`, `raiders`) and the `ash_militia`/`faction_ash_militia` duplicate are data defects.

### Environmental Atmosphere (`environmental_atmosphere_expansion.json` — 152 entries)

**Provenance: F (Material evidence) / C (Eyewitness)**

Atmospheric descriptions keyed to conceptual location names (not `loc_` IDs). These are the "what it feels like to be there" texts — sensory, observational, non-narratorial. Currently **orphaned** (no runtime consumer).

### Environmental Texts (`environmental_texts_expansion_05.json` — 36 entries)

**Provenance: D (Propaganda) / E (Personal) / F (Material)**

Mixed provenance by type: `warning` and `notice` entries are material evidence; `diary` and `thank_you` are personal; `graffiti` is propaganda-adjacent. Currently **orphaned** (no runtime consumer).

### Narrative Documents (273 files in `narrative/`)

**Provenance: mixed — classified per file**

| File category | Provenance | Count |
|---------------|------------|-------|
| Bunker culture (graffiti, court verdicts, maintenance) | D / F | ~40 files |
| Expedition/field reports | C | ~15 files |
| Medical/therapist records | E / F | ~12 files |
| Radio scripts/broadcasts | B / D | ~10 files |
| Personal letters/journals | E | ~15 files |
| Technical/engineering logs | F | ~80 files |
| Faction directives/notices | B / D | ~8 files |
| Oral lore / songs | G | 2 files (26 entries) |
| Trade/economy records | F | ~10 files |
| Children's folklore/artwork | G / E | ~5 files |

### Codex Candidates (Dev Documents)

**Provenance: H (Developer-only) → must be reclassified before player exposure**

| Document | Lines | Candidate codex entries | Reclassification target |
|----------|-------|------------------------|------------------------|
| `ASH_FALL_CREATIVE_FRAMEWORK.md` | ~2000 | ~60 | A (public record) or B (institutional) |
| `ASHFALL_GAME_MASTER_DOCUMENT_UPDATED.md` | ~2500 | ~80 | A / B |
| `IntelBible.md` | ~300 | ~25 | B (restricted) |
| `02_THE_LIST.md` | ~250 | ~20 | **Tier 4 spoiler** — contains entire mystery arc |
| `03_LOCATIONS.md` | ~400 | ~15 | A (public geography) |

### Archive Inks (`archive_inks.json` — 3 entries)

**Provenance: F (Material evidence)**

Ink recipes are physical-crafting data. Each ink has a `required_item_id` — `charcoal` is missing from the item catalog, blocking 2 of 3 inks at the transcription station.

---

## Reliability Rules

1. **Type A sources are authoritative.** When a Type A and Type C source conflict, the player sees the conflict (both are presented), but the game's mechanical systems (radiation, economy, faction stance) resolve using Type A.
2. **Type B sources are faction-colored.** The codex must attribute these ("According to the Rebuilders...").
3. **Type C/E sources may contradict.** This is intentional — survivors disagree.
4. **Type D sources are never mechanically authoritative.** They inform tone, not systems.
5. **Type G sources degrade.** Oral lore entries should shift wording over repeated in-game years (future Plan 17 enhancement).
6. **Type H sources never reach the player.** They are design documents, not content.

---

## Cross-Reference Integrity

| Provenance pair | Expected agreement | Conflict handling |
|-----------------|-------------------|-------------------|
| A ↔ A | Must agree | Data bug — flag for fix |
| A ↔ B | B may omit or spin | Present both; A is mechanical truth |
| A ↔ C | C may misremember | Present both; C adds color |
| B ↔ B (same faction) | Must agree | Data bug |
| B ↔ B (different faction) | Expected to disagree | Present both; player judges |
| G ↔ A | G may mythologize A | Intentional; G is "what people say" |
