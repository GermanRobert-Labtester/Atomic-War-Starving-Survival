# Plan 153 regression matrix

| Area | Verification | Expected invariant |
|---|---|---|
| Source load | `FringeCultsCatalog` against four narrative files | 8 + 8 + 7 + 7 = 30 |
| Projection | `NarrativeDiscoveryCatalog` against the manifest | 153 total projected records; 30 `disc_fringe_*` records |
| IDs | duplicate scan and negative fixture | all 30 source IDs and discovery IDs unique |
| Group identity | faction/catalog cross-reference | unresolved sect, chapter or monastery labels remain display-only |
| Producers | map and shelter-room resolution | every Plan 153 producer resolves to a canonical location or explicit room ID |
| Ordering | reversed minimal manifest fixture | projected order is unchanged |
| Discovery | first and repeated producer inspection | first call unlocks once; repeats return no change |
| Save/load | Journal capture/restore fixture | restored keys suppress replay |
| Numeric firewall | projection and authority review | sacred cpm, °C and Hz remain presentation claims |
| Epitaph firewall | memorial fixture | reading an epitaph does not add a memorial or alter a survivor |
| Reader | JournalCodex and JournalBookUI | truth/provenance labels appear; links use bounded route prefixes |
| Missing data | source/related-target degradation | absent records do not crash or create phantom entities |
| Utilization | scanner + runtime collector | all four activated source files have loader, consumer, UI and runtime evidence |
| Data integrity | Godot headless selftest | no producer, source or related-ID integrity errors |

## Negative fixtures

Covered in Core tests and validator paths: duplicate discovery ID, missing source record, unsupported producer, malformed manifest entry, duplicate source ID in `FringeCultsCatalog`, missing related discovery ID, and the absence of any memorial or simulation adapter on discovery.
