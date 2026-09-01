# Muster Witness Runtime Contract (Plan 25 · 25B.1)

> Verified 2026-09-01. Records the current witness architecture (the canonical pattern new witnesses follow) and the v2 extension Plan 25 ships.

## 1. Current architecture (v1)

### Data — `Assets/StreamingAssets/Data/muster_witnesses.json`
`{schema_version: 1, witnesses: [ ... ]}` — **3 entries**, exactly these fields and no others:

```json
{
  "id": "witness_1_checkpoint_conscript",
  "witness_name": "The Checkpoint Conscript",
  "location_id": "loc_garrison_checkpoint_gamma",
  "knowledge_key": "history_checkpoint_conscripts_confession",
  "day_min": 241,
  "body": "Three drinks past careful, ..."
}
```

Entries: `witness_1_checkpoint_conscript`, `witness_2_quartermaster_paperwork`, `witness_3_signals_intercept`. Ordering = file order. No faction, no flags, no variants, no priority.

### Core — `Assets/Ashfall.Core/Muster/WitnessCatalog.cs`
- DTO `WitnessDefinition` (L8–16): `id, witnessName, locationId, knowledgeKey, dayMin, body` (mirrors JSON 1:1 via private snake_case `WitnessEntry`, L83–91).
- Loader `WitnessCatalogLoader.LoadWitnesses(dataDir, IFileIO, IJsonSerializer)` (L30). Missing file → empty list. **`schema_version > CurrentSchemaVersion (1)` → empty list** (L49) — a schema bump without a loader update silently discards everything.
- **Core never evaluates witnesses.** No eligibility, ordering, or cap logic exists.

### Host presentation
- `src/Muster/JournalWitnessPanel.cs` — ScrollContainer; gates `if (day < w.dayMin) continue;` (L67); frames via `JournalVoice.ComposeFullText(w.knowledgeKey, authorBias, day)` (L69).
- `src/UI/MusterPanel.cs:124-128` — static "WITNESS DOSSIERS" section.
- `src/Main.Muster.cs:215-217` — "Three accounts: {n} loaded" copy (hardcoded).
- `src/Main.UiTests.Muster.cs:40` — asserts `_muster.Witnesses.Count == 3`.

### Epilogue relationship — none today
Witnesses do **not** feed `muster_epilogues.json` (12 entries, `{ending_key, title, prose}`). Epilogue selection is purely approach-driven (`MusterSystem.SelectApproachFor` → `MusterRecord.endingKey` → `EndingKeyForAny` → `EpilogueMatrix`). `knowledge_key` routes only to journal voice framing. Task 25B's witness→epilogue wiring is new.

### Muster activation
`MusterSystem.SetEscalationDay(day)` → `musterTriggered = day >= 260`. Host feeds from the Year-of-Ash clock (`src/Main.Muster.cs:328-336`). No quest-flag or war-state input.

## 2. v2 extension (Seam S2 — the canonical pattern for all new witnesses)

### Schema (schema_version 2, back-compat)
```json
{
  "id": "witness_scavenger_claimant",
  "witness_name": "The Scavenger Claimant",
  "location_id": "loc_scavenger_guildhall",
  "faction_id": "faction_scavenger_guild",
  "subject_id": "npc_or_survivor_id_or_empty",
  "day_min": 200,
  "priority": 30,
  "knowledge_key": "history_witness_scavenger_claimant",
  "testimonies": [
    { "variant_id": "helped",  "requires_all_flags": ["flag_favor_scavenger_arbitration_fair"], "body": "..." },
    { "variant_id": "failed",  "requires_all_flags": ["flag_grievance_scavenger_claim_dispute"], "body": "..." },
    { "variant_id": "absent",  "body": "..." }
  ]
}
```
- v1 `body` = one unconditional testimony (loader synthesizes a single variant; **v1 files load forever**).
- `requires_any_flags` / `requires_all_flags` / `forbids_flags` — all optional; first matching testimony in **authored order** wins; a variant with no conditions is the fallback.
- `priority` — optional int; ordering = priority desc, then id ordinal (never dictionary/accidental order).
- `subject_id` — optional NPC/survivor id for alive/dead eligibility; empty = institutional witness (faction reps may be summoned regardless of personal encounter; documented exception per plan §25B.9).

### Core
- `WitnessCatalog.CurrentSchemaVersion` → 2; DTO gains `FactionId`, `SubjectId`, `Priority`, `Testimonies: List<WitnessTestimony>`; v1 fallback preserved.
- New port `IWitnessEligibility` (Core, engine-agnostic): `bool IsFlagSet(string flag)`, `bool IsSubjectAlive(string subjectId)`, `bool IsFactionPresent(string factionId)`. Host binds flag ledger + survivor census + Muster systems.
- New `WitnessSelector.Select(witnesses, day, eligibility, maxCount)`: deterministic — day gate → alive/dead → encounter/flag eligibility → first-match variant → order (priority desc, id ordinal) → optional cap preserving faction/personal diversity. No RNG.
- Results `witness_id → {variant_id, delivered_day}` persisted in `MusterHostSave` (additive field, null-tolerant) → stable epilogue/Verdict surface (Plan 15A/15B consume results; they never re-derive eligibility).

### Known breakage fixed in Seam S2
- `MusterContentCatalogTests.cs:52-55` (count+ids) → v2-aware assertions.
- `src/Main.UiTests.Muster.cs:40` (`== 3`) → threshold assertion.
- `src/Main.Muster.cs:215-217` ("Three accounts") → dynamic copy.
