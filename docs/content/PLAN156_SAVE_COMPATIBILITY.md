# Plan 156 — Save Compatibility

The source catalogs are immutable and add no save section. Discovery uses the existing JournalSystem knowledge keys (narrative_discovered_<discovery_id>) and the existing checksummed campaign journal section.

Policy:

- New saves persist stable discovery IDs through the journal knowledge ledger.
- Existing saves load with zero fabricated Plan 156 discoveries. No migration marks records found merely because their authored time has passed.
- Reloading a save reconstructs codex visibility from journal knowledge; it does not replay a producer, source adapter or side effect.
- Reordering the manifest or source files does not alter discovery identity.
- Duplicate discovery attempts do not add a second codex unlock or journal notification.
- Source catalog definitions and transcripts are not copied into save state.
- Missing or deferred source files produce no discovery and do not create placeholder entities.

No doctrine, process measurement, item quantity, recipe, research state, faction state, document authenticity, medical state or propaganda state is persisted by this plan.
