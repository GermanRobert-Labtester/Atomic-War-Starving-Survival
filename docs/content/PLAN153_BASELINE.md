# Plan 153 baseline — fringe cults

**Audit date:** 2026-09-09
**Authority:** `Assets/StreamingAssets/Data/narrative/*.json` plus live Core and Godot source.

The current typed `FringeCultsCatalog` loads exactly 30 records from four `schema_version: 1` catalogs:

| Family | File | Records | Authored fields with state-like values |
|---|---|---:|---|
| Cobalt liturgies | `narrative/cobalt_liturgies.json` | 8 | `sacred_rad_threshold_cpm` |
| Iron Synod canons | `narrative/iron_synod_canons.json` | 8 | `sacred_temperature_celsius` |
| Geophone hymnals | `narrative/geophone_hymnals.json` | 7 | `resonant_frequency_hz` |
| Wasteland epitaphs | `narrative/wasteland_grave_epitaphs.json` | 7 | cause-of-death testimony |
| **Total** | 4 files | **30** | |

The four source files are immutable authored content. The existing `narrative_discovery_manifest.json` now contains 153 entries, including 30 `disc_fringe_*` projections. The manifest is a producer map and presentation metadata layer; it is not a second source of cult doctrine.

## Runtime findings

- `FringeCultsCatalog` existed as a typed loader but had no player-facing production consumer.
- `NarrativeDiscoveryCatalog` and `JournalSystem.UnlockNarrativeDiscovered` are the existing Plan 135 discovery seam.
- `JournalCodex` is the existing Events-tab projection and `JournalBookUI` is the existing reader.
- `MemorialSystem` is the death/memorial authority. No epitaph adapter calls it.
- `RadiationSystem`, foundry systems, audio/geophone systems and faction standing systems have no Plan 153 write path.
- `cult_faction`, Synod chapter and monastery circle names do not resolve as canonical faction IDs. They remain authored institution/sect labels.
- The seven named epitaph identities have no exact canonical survivor/NPC identity match in the live catalogs. The mass-burial record is explicitly anonymous/local.
- The authored `timestamp_relative` fields are historical artifact chronology. They are retained in the reader and are not converted to universal campaign-day unlocks.

## Activation decision

Each record is discoverable through an explicit producer in the manifest. Producers are real location IDs or the authored shelter rooms `room_foundry`, `room_radio_tuner` and `room_memorial_wall`. The host calls the existing narrative discovery ledger when the associated surface is opened or a map detail is inspected. Discovery is idempotent and writes only a `knowledge_narrative_discovered_*` key through `JournalSystem`.

Technical values use safe presentation labels: “Sacred count named in the liturgy”, “Canon-prescribed furnace temperature”, “Hymnal frequency notation” and “Cause of death recorded on marker”. No value is presented as a safe dose, foundry setpoint, signal trigger or true mortality record.

## Source fingerprints

These hashes freeze the audited source snapshot. The discovery manifest hash includes the Plan 153 producer map.

| File | SHA-256 |
|---|---|
| `narrative/cobalt_liturgies.json` | `7ccc1afffbb03c3fb0bdba1ec7f4c07f8c445c4ec52ae1cbbd5d14d5f599c866` |
| `narrative/iron_synod_canons.json` | `1ac2b335ce781518a6cc259576dc9d72d16ccf00595c9d9eaba3cd5d0dcfbe08` |
| `narrative/geophone_hymnals.json` | `6af4ec8bad1cde5e89f881eaeaa25e47fe450adf3c3f075da7970abe0d6a11ce` |
| `narrative/wasteland_grave_epitaphs.json` | `09f68df3803d409e783a49116035ea5c3fca598495eae5735082462bd7becfcc` |
