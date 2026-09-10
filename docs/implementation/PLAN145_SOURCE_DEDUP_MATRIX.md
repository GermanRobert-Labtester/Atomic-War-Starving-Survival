# Plan 145 Source Deduplication & Reconciliation Matrix

Analysis of ID namespaces, content duplicates, schema formats, and reconciliation policy across the repository's graffiti sources.

## 1. Inventory of Potential Sources

| Path | Format / Schema | Count | Overlap with Canonical | Disposition |
|---|---|---:|---|---|
| `Assets/StreamingAssets/Data/narrative/bunker_graffiti_postings.json` | `BunkerGraffitiFile` (`posting_id`, `recorded_day`, etc.) | 36 | Primary Canonical | Kept as canonical base corpus |
| `Assets/StreamingAssets/Data/narrative/graffiti_expansion.json` | `BunkerGraffitiFile` (`posting_id`, `recorded_day`, etc.) | 40 | Exact schema match; 0 ID collisions | Kept as expansion corpus, loaded additively |
| `Assets/StreamingAssets/Data/bunker_graffiti_postings.json` (root) | Plan 12B Flag-trigger (`id`, `title`, `text`, `triggerWorldFlag`) | 10 | Completely different schema & IDs (`graffiti_*`) | Kept separate; owned by `Plan12BFrictionTests` |

## 2. ID Namespace Audit

- **Canonical Base IDs (36):**
  - Prefix pattern: `graf_{01..36}_{slug}` (e.g. `graf_01_the_first_stoker_rule` to `graf_36_the_final_slate_greeting`).
  - Uniqueness: 36/36 unique (Ordinal and OrdinalIgnoreCase).
- **Expansion IDs (40):**
  - Prefix patterns:
    - `graf_dir_{01..06}_{slug}` (Directional/Practical)
    - `graf_warn_{07..14}_{slug}` (Warnings/Hazards)
    - `graf_acc_{15..19}_{slug}` (Accusations)
    - `graf_joke_{20..25}_{slug}` (Jokes/Dark Humor)
    - `graf_grief_{26..31}_{slug}` (Grief/Remembrance)
    - `graf_mark_{32..37}_{slug}` (Tallies/Practical marks)
    - `graf_long_{38..40}_{slug}` (Extended wall scrawls)
  - Uniqueness: 40/40 unique (Ordinal and OrdinalIgnoreCase).
- **Cross-Source Collisions:**
  - `Base ∩ Expansion`: **0 collisions** (empty set).
  - Case-insensitive collisions: **0 collisions**.

## 3. Content Duplicate Audit

- Normalized content fingerprints (lowercase, stripped whitespace):
  - Total distinct texts: 76 / 76.
  - Duplicate texts: 0.
- Semantic thematic continuity:
  - Base `graf_01_the_first_stoker_rule` (Day 3) and expansion `graf_joke_21_stoker` (Day 3): Both authored by "Fyodor the Stoker" on the boiler steam pipe regarding wet gloves on the draft lever, written from slightly different angles (the official warning vs the sarcastic scrawl). This is rich environmental worldbuilding, not data duplication.
  - Base `graf_02_ration_biscuit_warning` (Day 12) and expansion `graf_joke_20_biscuit` (Day 12): Both by "Boris the Baker" regarding the Tuesday hardtack speckles (weevil meal), reinforcing the historical event.
  - Base `graf_36_the_final_slate_greeting` (Day 3650) and expansion `graf_grief_27_names` / `graf_long_40_wall` (Day 62): The plaza tally wall starts on Day 62 and culminates in the final opening of the outer blast door on Day 3650.

## 4. Loader Reconciliation Strategy (Workstream 145A)

Rather than merging the physical JSON files into a single monolith (which would disrupt existing diff history and test fixtures), `BunkerGraffitiCatalog` uses **multi-source additive loading with strict in-memory deduplication**:

1. `BunkerGraffitiCatalog.Load(json, serializer)` parses and validates entries.
2. If `posting_id` already exists in `_byId`, the loader ignores the duplicate and does NOT append to `_allPostings`.
3. `LoadFromDirectory(dataDir, fileIo, serializer)` loads `narrative/bunker_graffiti_postings.json` followed by `narrative/graffiti_expansion.json`.
4. Result: 76 stable, validated postings loaded deterministically into runtime memory.
