# Plan 145 Baseline — Bunker Graffiti & Wall-Text Runtime Activation

Status: Reconnaissance complete; baseline established; implementation ready.

## 1. Runtime Authority

- **Class:** `Ashfall.Core.Narrative.BunkerGraffitiCatalog` (`Assets/Ashfall.Core/Narrative/BunkerGraffitiCatalog.cs`)
- **Entry Type:** `Ashfall.Core.Narrative.BunkerGraffitiEntry`
  - `posting_id` (string): Unique identifier.
  - `recorded_day` (int): Authored day of mark and unlock day.
  - `location` (string): Authored physical placement description.
  - `medium` (string): Inscription tool / material.
  - `author_signature` (string): In-world author attribution.
  - `category` (string): Descriptive thematic classification.
  - `content` (string): Exact in-world text / inscription.
  - `morale_effect` (string): Descriptive tone metadata (NOT mechanically executed).
  - `tags` (string[]): Ambient keywords.
- **Container Type:** `Ashfall.Core.Narrative.BunkerGraffitiFile` (`schema_version: 1`, `collection_id: string`, `postings: List<BunkerGraffitiEntry>`)

## 2. Source Census & Authority

| Source Path | Postings Count | Schema Match | Current Utilization Status | Role |
|---|---|---|---|---|
| `Assets/StreamingAssets/Data/narrative/bunker_graffiti_postings.json` | 36 | `BunkerGraffitiFile` V1 | `CODEX_ONLY` | Original canonical 36 postings (Day 3 to Day 3650) |
| `Assets/StreamingAssets/Data/narrative/graffiti_expansion.json` | 40 | `BunkerGraffitiFile` V1 | `CODEX_ONLY` | Expansion 40 postings (Day 1 to Day 100) |
| `Assets/StreamingAssets/Data/bunker_graffiti_postings.json` (root) | 10 | Plan 12B/30 Flag schema | `UNRESOLVED` | Unrelated flag-gated friction graffiti (`id`, `title`, `text`, `triggerWorldFlag`). Separate owner (`Plan12BFrictionTests`). |

**Total Plan 145 Corpus:** 76 postings across two compatible authored JSON files in `Data/narrative/`.

## 3. Duplicate & Collision Census

- **Exact ID Collisions:** 0 across all 76 entries (36 base + 40 expansion).
- **Case-Insensitive ID Collisions:** 0.
- **Exact Content Duplicates:** 0.
- **Semantic Overlap:** The two files represent complementary narrative eras:
  - Base corpus (36 entries): Long-horizon chronicle spanning the entire 10-year bunker isolation (Day 3 to Day 3650).
  - Expansion corpus (40 entries): Tactical, ground-level markings from the early crisis weeks (Day 1 to Day 100: directional arrows, utility warnings, grief tallies, canteen jokes).

## 4. Loader Lifecycle Defect

- **Identified Defect:** `BunkerGraffitiCatalog.Load` appends entries directly to `_allPostings` without checking if `_byId` already contains the ID. Calling `Load()` twice duplicates enumeration, causing `AllPostings.Count` to double while dictionary lookup overwrites by ID.
- **Remediation:**
  1. Add `if (_byId.ContainsKey(p.posting_id)) continue;` in `Load()`.
  2. Add `Clear()` / `Reset()` method to reset internal state when an explicit full reload is desired.
  3. Validate null/empty `posting_id`, null/empty `content`, and non-negative `recorded_day`.
  4. Provide `LoadFromDirectory(string dataDir, IFileIO fileIo, IJsonSerializer serializer)` helper following the `DwellerMedicalCatalog` pattern.

## 5. Vocabulary & Semantics

- **`morale_effect`:** Verified 100% descriptive authoring tone / metadata. There is zero mechanical parsing or consumption in Core or Host. Reading wall text will NOT alter morale, health, or stats.
- **`recorded_day`:** Dual-role: authored timeline timestamp and minimum player campaign day threshold (`recorded_day <= currentDay`).
- **`author_signature`:** Descriptive environmental flair (e.g. `the teacher`, `Fyodor the Stoker`, `Nurse Anya`). Preserved as authoring text; never instantiates or alters survivor entities.
- **`location`:** Free authored placement descriptions. Projected onto canonical game spaces via `BunkerGraffitiProjection` without altering source data.

## 6. Target UI Surfaces

1. **`ShelterPanel` & `HoldfastInteriorView`:**
   - Shelter corridor, rooms, and infrastructure markings displayed during room inspection and in the Shelter Layout view.
2. **`MapDetailPanel`:**
   - World location inspection: displays exterior markings, warning signs, and wasteland scrawls for explored sectors.
