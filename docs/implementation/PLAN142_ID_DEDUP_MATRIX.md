# Plan 142 ID and Dedup Matrix

## Source-level census

| Comparison | Result | Policy |
|---|---:|---|
| Expansion-05 IDs | 28 unique | retain authored IDs |
| Narrative ambient IDs | 40 unique | retain authored IDs |
| Canonical batch IDs | 121 unique | retain authored IDs |
| Ambient ↔ canonical ID overlap | 0 | no alias table required |
| Canonical knowledge-key overlap | 0 | one key maps to at most one authored record |
| Normalized body overlap | 0 | no semantic duplicate suppression needed beyond key/ID |

## Runtime dedup

`KnowledgeBase` remains the single dedup gate. The adapter never inserts
directly into `_entries`, never bypasses `TryAddRawEntry`, and never clears or
rebuilds journal state.

For an authored record:

1. use its stable source ID as the persisted entry ID;
2. use its canonical knowledge key as the dedup key;
3. reject a second record with the same source ID or knowledge key at catalog
   load time;
4. let an already-known key return `null` without a second event or
   notification.

For existing generated producers, the current sequence-based
`journal_{sequence}_{knowledgeKey}` ID behavior is preserved.

## Cross-source precedence

There is no precedence tie in the current census. If a future source adds an
ID or knowledge-key collision, loading fails deterministically with the source
paths and rows; the runtime must not silently choose one body.
