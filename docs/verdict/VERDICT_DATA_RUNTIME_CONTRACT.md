# Verdict Data Runtime Contract

**Authority:** `Assets/StreamingAssets/Data/verdict_data.json`
**Scope:** Plan 127 corruption corpus and world-history ladder expansion

## Catalog shape

The file is a schema-versioned object with these relevant arrays:

- `corruption_corpus`: plain strings selected by `MachineLogSystem`.
- `world_history_ladder`: rows with `layer`, `knowledge_key`, `title`,
  `discovery_location_id`, and `body_summary`.

The existing `currencies`, `readout_steps`, `facets`, and `endings` arrays are
unchanged by Plan 127.

## Corruption semantics

`VerdictCatalogLoader.LoadCorruptionCorpus` loads the strings without parsing
or normalizing them. `VerdictHostSession.TickCorruption` passes the complete
list to `MachineLogSystem.InsertCorruptionMarker`, which selects one entry
with the host's seeded RNG and stores the selected text in the machine-log
save state. Corpus order is therefore deterministic-input data: Plan 127
preserves indices 0–7 and appends indices 8–24.

The strings are display text, not a command language. Brackets, timestamps,
repeated fragments, and em dashes have no special runtime meaning.

## Ladder semantics

`JournalCatalogData.LoadVerdictHistory` reads the ladder and exposes each
`knowledge_key` as a journal event ID. `JournalCodex` renders the title and
body when that event has been fired. `discovery_location_id` is retained in
the source data but is not currently used as a location gate.

The active host unlock path in `Main.UnlockVerdictLore` remains hardcoded to
the original six keys. Consequently, layers 7–12 are valid catalog and
codex rows, but they do not become reachable through dynamic ladder
progression under the pure-data scope. Enabling that reachability requires a
separate host/domain integration change and is intentionally deferred.

## Append-only rules

1. Preserve existing corpus text and order.
2. Preserve ladder layers 1–6, keys, locations, and order.
3. Add only ascending ladder layers 7–12.
4. Use existing committed location IDs.
5. Do not imply save-schema, evidence-ledger, quest, or location-gating
   behavior that the current runtime does not provide.
