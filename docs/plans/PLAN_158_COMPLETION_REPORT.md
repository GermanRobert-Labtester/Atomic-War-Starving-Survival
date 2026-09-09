# PLAN 158 COMPLETION REPORT — Cordage, Cable & Technical Textiles (2026-09-09)

## Record counts by family

| Family | Source file | Records | Truth class |
|---|---|---|---|
| HempFiberHackling | hemp_fiber_hackling_logs.json | 8 | Process quality record |
| WireRopeStranding | wire_rope_stranding_assays.json | 8 | Specification assay |
| ManilaHawserBreakage | manila_hawser_breakage_reports.json | 7 | Failure report |
| TransmissionRopeSplicing | rope_transmission_splicing_audits.json | 7 | Maintenance audit |
| NeopreneGasketDegradation | neoprene_gasket_degradation_logs.json | 8 | Degradation log |
| AramidFiberRot | aramid_fiber_rot_reports.json | 8 | Degradation report |
| TireRetreading | tire_retreading_compound_logs.json | 7 | Workshop production log |
| CelluloidFilmDecomposition | celluloid_film_decomposition_records.json | 7 | Hazard assessment |
| **Total** | | **60** | |

## Canonical item/location links

10 records → 7 canonical item ids (all proven, all resolving):
`rope` (2 hemp records), `gas_mask`, `item_gas_mask_improved`,
`protective_rubber_gloves`, `item_hermetic_hatch_silicone_gasket`,
`rubber_hose` (5 gasket records), `film_reel` (2), `photographic_film` (1).

Rejected identities (fail-closed, test-pinned): all 8 aramid `armor_item_id`
values (PASGT/MK2/etc. — historical designations, no canonical vest/helmet
items exist), all 7 tire casing ids, all 8 mask model designations
(M17/CDV-715/…), all spool/coil/shaft labels. No duplicate items authored.

## Activated / deferred

- **Activated: 26 records** (3–5 per family; all 8 families represented)
  across **9 producer paths** (existing deep-lore sites only):
  drainage network (maritime hawsers), frozen wetland (rescue line),
  steelworks (hoist cable + drive audits), power substation (transmission
  drives), chemical plant (seal aging), ammunition depot (tire logs + cache
  aramid), police station (armor evidence), television studio (nitrate
  film), municipal library (safety film) + agricultural research (hemp).
- **Deferred: 34 records** — archival depth, no producer, fail-closed on
  direct discovery attempts (`DeferredRecordIds` pinned by test).

## Producer paths (domain coverage)

Maritime §10 ✓ (4 hawser records — foreshadowing only, no rope snapping) ·
Maintenance §11 ✓ (6 wire/transmission records; Plan 148 incident state
untouched) · Protective §12 ✓ (5 gasket/aramid records; item-inspection
query surface `RecordsForItem`; no protection change) · Vehicles §13 ✓ (3
tire records; no vehicle condition/speed subsystem) · Archives §14 ✓ (5
celluloid records; combustion stays descriptive — no fire mechanic).

## Measurement dispositions

All 18 measurement field classes classified in
TECHNICAL_MEASUREMENT_DISPOSITION.md: descriptive (12), display-only
comparison (5), typed-link-compatible (via proven canonical links only).
Breaking load never rendered as safe working load (test-pinned). Mixed
imperial/metric hawser units preserved as historical provenance — no silent
conversion. Timestamps never campaign time (state carries no timestamps).

## Technical corrections (Workstream J)

- SWL/breaking-load language enforced in projection summaries.
- No unit conversions invented; historical mixed units labeled as-authored.
- Real-world nomenclature (M17, PASGT, 11R20) flagged for the standing tone
  sweep — treated as pre-Exchange archival designations; not rewritten.
- Plausibility: cold-cure vulcanization (20–25 °C), aramid failure chemistry
  and orbital-free content all kept as authored; no contradictions found.

## Save behavior

`technical_material_archive` save section (SaveStoreHub checksummed
`SchemaVersionedEnvelope`, atomic write) — discovered record IDs only,
ordinal order, tolerant of unknown future IDs; old saves restore empty and
auto-discover nothing; reload cannot duplicate discoveries. Canonical links
and measurements are re-derived from current catalog data (never persisted).

## Files changed

- `Assets/Ashfall.Core/Narrative/TechnicalMaterialArchiveSystem.cs` (new —
  combined read-only projection + discovery + firewall)
- `src/Host/TechnicalMaterialArchiveSaveStore.cs` (new)
- `src/Main.Plans158.cs` (new — host triad: Ensure/Setup/Save, expedition
  location-discovery hook, journal first-discovery intel with per-family
  banner and ARCHIVAL presentation)
- `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` (+1 section)
- Contract gates: VersionReportContractTests,
  ComprehensiveSaveStoreCorruptionAndMigrationTests,
  ARCHITECTURE_TEST_MAP (+ generator graph entry),
  PersistentFilenameRegistryGate (passes — new store delegates via
  SaveStoreHub)
- `docs/content/`: CORPUS_INVENTORY, IDENTITY_CROSSWALK,
  AUTHORITY_MATRIX, PRODUCER_MAP, MEASUREMENT_DISPOSITION (5 deliverables)

## Tests added

`Ashfall.Core.Tests/Narrative/TechnicalMaterialArchiveTests.cs` — 20 tests:
parity (2), identity (3), producer map (3), firewall (5), discovery/save
(4), query surfaces (2), deferred accounting (1).

## Gates

| Gate | Result |
|---|---|
| `dotnet test` (TechnicalMaterialArchive filter) | 20/20 PASS |
| `dotnet test` full suite | 10488/10489 — the single failure is the concurrent stream's own untracked in-flight test (`PaperPrintingRuntimeActivationTests`, Plan 156/157 paper work); every Plan-158-owned test green |
| `dotnet build Ashfall.csproj` | Plan-158 files verified clean (tree-level build currently red from the same stream's in-flight `BlackProjectsArchivePanel` — excluded momentarily for verification and restored) |
| `--data-integrity-selftest` | PASS — 299 catalogs, 0 errors (12661 ids) |
| `--bridge-selftest` | PASS |
| Architecture map gate | PASS — 171 subsystems, 100% mechanical evidence |

## Follow-ons deliberately excluded

- Item-inspection UI links (query surface `RecordsForItem` is ready; panel
  work owns the display).
- Tire/vehicle condition, aramid armor degradation, rope SWL mechanics, fire
  hazards from celluloid, power from transmission rope — all require their
  owning systems to request records explicitly; none exist, none created.
- The remaining 34 deferred records await per-site provenance passes.
