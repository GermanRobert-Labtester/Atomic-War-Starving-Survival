# Plan 160 Regression Matrix

| Area | Verification | Expected result |
|---|---|---|
| Source baseline | BoneHornCarvingCatalog loads four files | 8 / 8 / 7 / 7 and total 30 |
| Projection | NarrativeDiscoveryCatalog loads allowlisted sources | 30 unique Plan 160 projections |
| Identity | animal, saw and blank crosswalk | unresolved labels stay display-only |
| Dog safeguard | source dog records | no survivor or companion binding |
| Shed safeguard | antler_horn_001 | old shed does not create death/harvest state |
| Producer map | seven contexts | each returns records; no duplicate IDs |
| Vertical slice | workshop producer → JournalSystem | first discovery succeeds, second returns false |
| Restore | JournalSystem CaptureState/RestoreState | discovered key survives; no replay |
| Reload | LoadFromFiles twice | combined count stays 243; Plan 160 stays 30 |
| Duplicate fixture | duplicate discovery_id manifest entries | one projected record, no silent second row |
| Authority firewall | summaries and numeric labels | no inventory, wildlife, crafting, condition, combat, fishing or trade mutation path |
| UI surface | JournalCodex Events row | unlocked body is readable; metadata labels authored provenance |
| Exported data | packaged narrative directory | four files and manifest are present and loadable |
| Content utilization | scanner and runtime collector | exact four files have loader, registry, query and codex evidence |

Negative fixtures covered by the implementation/test plan include unknown source paths, duplicate discovery IDs, missing source records, nonexistent producer lookup, unresolved item labels, generic dog labels, and reloading an already-discovered record.
