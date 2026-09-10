# Plan 156 — Read-Only Projection Matrix

PaperPrintSourceAdapter is a bounded adapter registered in NarrativeDiscoveryCatalog. It accepts exactly the eight Plan 156 source paths and no arbitrary catalog or command name.

| Projection field | Source | Presentation contract |
|---|---|---|
| DiscoveryId | Manifest | Stable disc_paper_making_* or disc_paper_printing_*; discovery identity only. |
| SourceCatalog, SourceRecordId | Manifest | Preserves source provenance and prevents cross-catalog ambiguity. |
| RecordFamily | Exact source path | Paper-making or printing family label. |
| FacilityOrStationLabel | Typed facility/station field | Authored label; not a resolved live room or machine. |
| TechnicalSummary | Typed numeric/string fields | Prefixed Authored measurements, Authored assay, Authored wear record or Authored print artifact. |
| BodyText | prose | Authored text shown verbatim as record prose. |
| TruthClass | Manifest | Historical Observation for the current corpus. |
| NumericClaimLabel | Manifest | Authored measurement — not a live production value. |
| ProducerIds | Manifest | One primary and optional explicit alternate contexts; no fuzzy lookup. |
| RelatedDiscoveryIds | Manifest | Only the four audited cross-catalog relationships in the overlap matrix. |

Family-specific display labels are: “Authored measurements — beating duration / freeness”, “wire mesh / sheet format”, “post / pressing force / moisture removed”, “gelatin / alum / Cobb absorption”, “Canadian freeness / hydration”, “measured pH / tannin / pigment”, “impressions / metal / wear phenomenon” and “matrix / pigment / smear”. None is rendered as a required setting, safe value, recipe or live telemetry.

There is no generated current-report schema in Plan 156. If one is added later, it must be a distinct typed record supplied by an existing live owner and visually separated from these archival observations.
