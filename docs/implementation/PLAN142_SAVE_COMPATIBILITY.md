# Plan 142 Save Compatibility

## Preserved contract

Plan 142 does not add fields to `JournalEntry`, `JournalSave`, or the
campaign envelope. Existing saves continue to load through
`JournalSaveStore` and the `"journal"` `SaveSectionRegistry` entry.

Persisted state remains:

- entries in newest-first order;
- `KnowledgeBase` discovered keys;
- next sequence number;
- unread and ping state;
- ping count;
- HUD visibility;
- active tab and last-seen arrays;
- codex unlock count.

## Authored record behavior across save/load

An authored key that was already activated is present in both the entry array
and `KnowledgeBase`. After restore, a producer retry reaches the same dedup
gate and creates no second entry. Authored metadata is immutable catalog data,
not campaign state, so it does not require migration or a save section.

Restore remains event-silent. Catalog loading and metadata lookup do not
mutate a restored journal or fire UI notifications.

## Compatibility matrix

| Save | Expected result |
|---|---|
| pre-Plan-142 journal save | loads unchanged; authored catalog is optional read-only input |
| Plan-142 save with authored entry | loads body, stable ID, author, day/hour, and knowledge key unchanged |
| repeated producer after reload | deduped by restored knowledge key |
| missing optional authored source | existing save still loads and generated producers still work |
| corrupt journal envelope | existing `JournalSaveStore` checksum handling remains authoritative |
