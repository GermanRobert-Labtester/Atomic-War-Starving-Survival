# ORAL LORE SCHEMA RECONCILIATION — Plan 155 Task A (+ Task H quest audit)

## Decision: Preferred A — Canonical DTO extension

The canonical `OralLoreEntry` DTO was extended so both source files normalize
to ONE in-memory record. No adapter layer, no second catalog, no host DTO
clones, no content copying.

## The split, as found

| | Batch 1 (`narrative/oral_lore_codex.json`) | Batch 2 (`narrative/oral_lore_batch_2.json`) |
|---|---|---|
| Root array key | `songs` | `entries` |
| Tempo | numeric `tempo_bpm` (all 16 authored) | textual `tempo` (all 10 authored) |
| Meter | `meter` authored (all 16) | absent |
| ID namespace | `song_01…song_16` | `oral_b2_*` |
| Count | 16 | 10 |

Before Plan 155, Batch 2 loaded **zero** entries: the typed
`OralLoreCodexFile` bound only `songs`, so deserializing Batch 2 produced a
null list and `Load()` returned silently. The host
(`OralLoreHostSession.LoadCatalogs`) loaded both files but only ever held 16.

## Normalization contract (implemented in `OralLoreCatalog`)

- Both root keys (`songs` / `entries`) bind through one raw DTO.
- Batch 1: `tempo_bpm` numeric bound as authored; `meter` kept;
  `tempo_descriptor` empty.
- Batch 2: the textual `tempo` label is preserved **verbatim** in the new
  `tempo_descriptor` field. A numeric BPM is extracted ONLY when the label
  itself makes an explicit BPM claim (`(\d+)_?BPM` — e.g.
  `STEADY_MARCH_120_BPM` → 120). Rhythmic adjectives (`SLOW_MINOR`,
  `FREE_TIME`, `STEADY_4_BEAT`, `THREE_FOUR_TIME`, …) are **never** converted:
  `tempo_bpm` stays 0 and the descriptor is the whole truth. No BPM was
  fabricated from any label.
- `meter` for Batch 2 is empty — the descriptor carries the rhythm
  (`TempoSummary()` renders descriptor-when-present, else "N BPM — meter",
  else "tempo unspecified").
- `source_file` provenance records which authority each record normalized
  from (runtime-only; not persisted in saves).

## Task B — lifecycle hardening (implemented)

1. **Idempotent loads:** identical source payloads are detected by a stable
   hash and skipped — repeated loads (or host re-setup) never duplicate
   enumeration.
2. **Duplicate-ID precedence:** explicit first-load-wins; later duplicates are
   skipped and reported (`OralLoreLoadResult.duplicateIds`,
   `duplicateSkippedCount`). No silent overwrite.
3. **Deterministic ordering:** `AllSongs` is ordinal-sorted by `lore_id`
   after every load, independent of load order.
4. **ID stability:** authored IDs are immutable data; the unified loader
   changes no ID. Original 16 IDs and Batch-2 IDs remain exactly as authored.
5. **Exported builds:** both files live under
   `Assets/StreamingAssets/Data/narrative/` and resolve through the same
   data-dir resolution every catalog uses (verified by the Core tests, which
   load via the repository data directory, and the host session, which loads
   via `res://…`-resolved `_dataDir`).

## Task H — `quest_oral_lore_01` disposition

`questline_master.json` entry 504-wide corpus:
```json
{ "id": "quest_oral_lore_01", "title": "Oral Lore 01", "synopsis": "",
  "factionTag": "", "status": "active" }
```

**Classification: integrity placeholder.** Generic numbered title, empty
synopsis, empty faction tag — a numbered stub slot, not authored quest
content. Disposition: **left untouched** — not auto-completed, not deleted,
not built upon. Plan 155 activates oral lore through discovery, not through a
quest contract. If a future task authors a real oral-lore quest (e.g.
preserving a threatened cultural piece), it should reuse this stable ID
through the existing quest runtime and REPLACE the placeholder fields with
authored content; until then the stub stays inert (the quest runtime does not
surface synopsis-less quests as real content).

## Continuity & cultural QA highlights (Task J)

- **Canonical performers verified:** Bram Ostrowski (the mapmaker;
  `npc_bram_ostrowski` in characters.json) is canon; the Salt Freeholders
  (`faction_salt_freeholders` in faction_lore.json) are canon; Dr. Irina Vel
  (`npc_dr_irina_vel`) is canon (Vel's Keys hymn); Mara Veln (crypt
  epitaphs) is canon. "The Therapist", "The Surgeon" and "The Priest" are
  generic role references — treated as role prose, NOT resolved to specific
  NPCs (no inference invented).
- **Harrow-4 vs Olympus:** song_14 references a "Harrow-4 orbital decay pass"
  on 88.4 MHz Pirate Free-Net — a distinct platform family from Plan 152's
  OLYMPUS platforms. Cross-plan note only: no record unification, no shared
  countdown; the song remains a radio-archived cultural piece.
- **Salt Freeholders' territorial lyrics** are faction perspective (performed
  "so anyone within earshot knows the land is occupied") — presentation
  metadata, never map ownership.
- **Deliberate variation:** Batch 1 and Batch 2 share no duplicate songs; no
  conflicting versions of one piece exist, so no variant labeling is needed.
  Oral unreliability lives in the prose (e.g. the cartographer attribution
  "attributed to"), which is preserved as authored.
