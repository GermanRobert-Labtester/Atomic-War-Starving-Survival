# PLAN-ORPHAN-SEAL-01 — Appendix AJ: Appendix Maintenance Map

**Generated:** 2026-09-21. A maintenance index for this kit: each appendix, what
it derives from, and the event that invalidates it. A successor agent re-runs
the listed derivation after the trigger rather than trusting a stale table.
**Rule:** regeneration always precedes claim promotion when the trigger listed
here has occurred.

| Appendix | Content | Derives from | Re-run trigger |
|---|---|---|---|
| A | Orphan dossiers | reachability audit + tests/catalog scan | after any seal batch or audit change |
| B | Wave packages | manual (authoring) | when a wave package lands or splits |
| C | Integration patterns | manual (authoring) | when a pattern is proven or rejected |
| D | Save ownership | orphan sources (capture/restore + registry refs) | after save-registry changes |
| E | Determinism audit | orphan sources (banned primitives) | after any orphan edit |
| F | Dependency clusters | intra-orphan reference graph | after any orphan edit |
| G | Integration points | host partials + registry + CLI flags | after host/registry growth |
| H | API surface | orphan sources (size/members) | after any orphan edit |
| I | Provenance | git log per orphan file | before promoting any claim |
| J | Test coverage | test files referencing orphans | after test additions |
| K | API signatures | orphan sources (public members) | after any orphan edit |
| L | Risk scorecard | D/E/G/H/J inputs | after any input appendix refresh |
| M | Catalog binding | Data files + loader scan | after data additions |
| N | Surface routes | Main.PlayerSurfaces route ids | after route additions |
| O | Verification commands | test regions + CLI flags | after test/flag additions |
| P | Inbound references | reachability closure re-run | after any orphan seal |
| Q | Save-key collisions | registry keys/aliases | after save-registry changes |
| R | Catalog shapes | matched catalog JSON | after catalog edits |
| S | Test regions | test files per region | after test additions |
| T | Worked exemplars | derived from A–U | after any input appendix refresh |
| U | Data references | JSON literals vs recursive data tree | after data additions |
| V | Master worklist | L/D/Q/M/N/O inputs | after any input appendix refresh |
| W | Data ids | id literals vs 4,750 data ids | after data additions |
| X | Static hazards | orphan static fields | after any orphan edit |
| Y | Batch plan | risk-sorted distribution | after L changes |
| Z | Shared shapes | public member shapes across orphans | after any orphan edit |
| AA | Time coupling | day/hour step methods | after any orphan edit |
| AB | Batch-plan links | batches × plan bodies | after new plans land |
| AC | Save signatures | capture/restore signatures | after any orphan edit |
| AD | Batch verification | Y + O inputs | after Y or O changes |
| AE | Surface decisions | G/N inputs | after host/route growth |
| AF | Seal order | AB/Y/L inputs | after AB/Y/L changes |
| AG | Loader gaps | M + loader scan | after data or loader changes |
