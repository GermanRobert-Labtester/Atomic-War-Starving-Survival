# Plan 156 — Regression Matrix

| Area | Coverage |
|---|---|
| Source integrity | 30 PaperMakingCatalog and 30 PaperPrintingCatalog records; 8/8/7/7 family counts; 60 globally unique source IDs. |
| Projection | 60 source projections, exact allowlist, family/facility/summary/provenance fields populated. |
| Overlap | Four explicit corroborating/related edges; no destructive merge. |
| Producers | Eight canonical contexts; fourteen paper-making records with alternate producer IDs; all producer IDs validated against the known context set. |
| Discovery | First discovery succeeds; repeat discovery returns false; alternate producer lookup returns the same record once. |
| Save/reload | Journal state round-trips and does not replay discovery. Old journal knowledge without Plan 156 remains undiscovered. |
| UI | Existing JournalCodex displays family, authored measurement label, facility provenance, prose and related links. No new printing panel is used. |
| Authority firewall | Adapter contains no effect fields or calls into inventory, crafting, research, faction, economy, medical or propaganda systems. |
| Negative fixtures | Duplicate manifest ID is rejected; absent producer returns no records; missing records fail closed; source reload clears and rebuilds rather than appending. |

Known repository-wide baseline failures from concurrent Plan 154/155 work remain outside this plan: save-section count drift, the missing oral_lore architecture map entry, machine-specific links in the Plan 154 report and the fast-gate blank-EOF finding in BunkerCourtCatalog.cs. Plan 156 focused tests and builds are reported separately from those unrelated findings.
