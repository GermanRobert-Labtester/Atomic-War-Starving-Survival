# Verdict Knowledge-Key Inventory

## Authority and resolution

The Verdict ladder has no separate knowledge registry in the current runtime.
`JournalCatalogData.LoadVerdictHistory` creates journal event rows directly
from `world_history_ladder`, using each `knowledge_key` as the event ID.
`Main.UnlockVerdictLore` is the current producer for the six original keys.

## Keys

| Key | Source row | Producer status |
|---|---:|---|
| `lore_verdict_geophone_one` | 1 | Existing host producer |
| `lore_verdict_shift_charters` | 2 | Existing host producer |
| `lore_verdict_standard` | 3 | Existing host producer |
| `lore_verdict_the_hold` | 4 | Existing host producer |
| `lore_verdict_the_call` | 5 | Existing host producer |
| `lore_verdict_the_count` | 6 | Existing host producer |
| `lore_verdict_second_geophone` | 7 | New journal row; no producer |
| `lore_verdict_cable_run_east` | 8 | New journal row; no producer |
| `lore_verdict_counting_house_origin` | 9 | New journal row; no producer |
| `lore_verdict_first_halt` | 10 | New journal row; no producer |
| `lore_verdict_last_hand` | 11 | New journal row; no producer |
| `lore_verdict_open_count` | 12 | New journal row; no producer |

The six new IDs are unique, use the established `lore_verdict_` namespace,
and resolve to journal catalog rows. They are not claimed to resolve through
an EvidenceLedger API because `EvidenceLedger` stores evidence IDs only.
