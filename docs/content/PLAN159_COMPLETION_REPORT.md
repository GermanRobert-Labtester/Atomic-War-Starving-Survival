# PLAN 159 COMPLETION REPORT — Tanning & Leather Material Provenance and Workshop Knowledge Runtime Activation

## 1. Deliverables

| # | Deliverable | Status |
|---|---|---|
| 1 | `docs/content/TANNING_LEATHER_CORPUS_INVENTORY.md` | ✅ — all 30 records inventoried (id, family, facility, materials, measurements, timestamp, tags, prose topic) + chronology (§A1) + vocabulary normalization (§A2) + truth classes |
| 2 | `docs/content/TANNING_LEATHER_ITEM_CROSSWALK.md` | ✅ — Workstream B item audit + Workstream C hide-production-loop audit |
| 3 | `docs/content/TANNING_LEATHER_LOCATION_CROSSWALK.md` | ✅ — Workstream D facility-label audit + Workstream F producer map |
| 4 | `docs/content/TANNING_LEATHER_AUTHORITY_MATRIX.md` | ✅ — full ownership matrix + firewall enforcement + persistence contract |
| 5 | `docs/content/TANNING_LEATHER_MEASUREMENT_DISPOSITION.md` | ✅ — every field dispositioned + Workstream J chemical-safety review |
| 6 | Read-only leather-process projection | ✅ — `LeatherworkArchiveSystem` (Core, `Ashfall.Core.Narrative`), modeled on the Plan 157/158 archive precedent |
| 7 | ≥5 real discovery producers | ✅ — 10 site producers + 3 item producers; all four families covered |
| 8 | Integration with existing surfaces | ✅ — expedition location discovery, item inspection (`InventoryDetailPanel` provenance rows), journal first-discovery feedback |
| 9 | Technical/chemical content review | ✅ — Workstream J table in measurement disposition; no unverified health claims exposed mechanically |
| 10 | Save/idempotence/export tests + report | ✅ — this document; 25 new tests |

## 2. Records, families, producers

- **30/30 records** load (8 oak-bark + 8 mineral + 7 bating + 7 currying); IDs unique; all four families present.
- **29 records producer-assigned** to 10 existing deep-lore sites; **1 deferred** (`mineral_tan_formaldehyde_synthetic_oil_tannage` — no aviation/fuel-filter site exists; left archival-depth rather than force a mismatch).
- Producers: `location_upland_logging_camp` (4), `location_chemical_plant` (6), `location_automated_abattoir` (7), `location_ammunition_depot` (3), `location_steelworks` (3), `location_drainage_network` (1), `location_agricultural_research` (2), `location_police_station` (1), `location_metro_station` (1), `location_frozen_wetland` (1).
- No settlement concentration: records spread across woodland, industrial, chemical, hide-source and travel sites; regional/process specialization preserved.

## 3. Identity resolutions

- **Canonical item links (4, fail-closed, test-pinned):**
  - `mineral_tan_potassium_alum_white_tawing` → `gas_mask`
  - `rawhide_bate_salt_stain_calcium_phosphate_speck` → `item_preservation_salt`
  - `leather_harness_neatsfoot_oil_cold_stuffing` → `leather_strap`
  - `leather_harness_sulfur_gas_red_rot_powdering` → `leather_strap`
- **Locations created: 0.** Facility labels (vats/pits/benches/drums) remain provenance strings; never promoted to `loc_*`.
- **Items created: 0.** No raw-hide, tannin-bark, fatliquor, chamois or belt item invented for integrity.
- **No hide-production loop exists** in current systems (Workstream C audit) — Plan 159 stays discovery/provenance content; the production gap is recorded for a future dedicated plan.

## 4. Descriptive vs mechanical fields

**30/30 records, 100% of fields DESCRIPTIVE.** Barkometer, steep months, pH,
shrink temperature, oil %, tensile PSI, formulas, chemical agents and
botanical sources render as archival summaries only. No mechanical consumer
was added; measurements are never persisted (they re-derive from catalog
data each run).

## 5. Technical corrections / review notes

- No factual errors in prose required correction. Review notes: "verdigris"
  (#27) is strictly copper acetate; the record already hedges with "copper
  oleate" and was left as authored. Cr(VI) "chrome hole" dermatitis (#15) is
  accurate; presented strictly as historical occupational context — no
  disease/affliction entry created.
- UI guard text explicitly separates batch history from item quality:
  "(ARCHIVAL BATCH — NOT THIS ITEM'S MEASURED QUALITY)".

## 6. Deferred gaps

1. **Hide economy** — no hide/skin production loop exists (hunting yields
   meat only). A dedicated future plan would own any hide/butchery loop.
2. **Shelter-room producer** — Plan 157's `room_workshop` producer hook is
   not currently invoked by any runtime path; leather records were not
   attached to a dead producer. Revisit when shelter-room inspection goes live.
3. **Manuals/research** — no typed manual node corresponds to tanning
   (Plan 80/34/33 authority); `DiscoverRecord(recordId)` is ready when one exists.
4. **Deferred record** — chamois fuel-filter tannage needs an
   aviation/fuel-filter site.

## 7. Save behavior

- New section `leatherwork_archive` (canonical file
  `leatherwork_archive_save.json`), thin façade over Core `SaveStore<T>` /
  `SaveStoreHub` — checksummed `{SchemaVersion, State, Checksum}` envelope,
  atomic write; enrolled in the single `campaign.json` aggregate envelope
  (Initiative #42) via `CaptureSection`.
- Discovered **record IDs only**; ordinal-ordered; unknown future IDs
  tolerated; no index-based state.
- Old saves: section absent ⇒ empty ledger ⇒ **nothing auto-discovers**.
- Reload: no duplicate discovery rewards (idempotent by contract, test-pinned).

## 8. Tests & gates

| Gate | Result |
|---|---|
| `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | ✅ 0 errors (1 pre-existing xUnit analyzer warning in `OralLorePlan155Tests` — concurrent file, not this plan) |
| `dotnet test Ashfall.Core.Tests` | ✅ **10530/10530 PASS** (full suite, no exclusions — concurrent streams reconciled their in-flight drift mid-session), incl. 25 new `LeatherworkArchiveTests` + `TanningLeatherCatalogTests` (4) + updated section-count gates |
| `dotnet build Ashfall.csproj` | ✅ **0 errors, 0 warnings** — includes the repaired `BlackProjectsArchivePanel.cs` (8 API-conformance fixes: `SetStatusRail()` factory pattern, `StatusRail.Set` instead of `SetCardValue`, `Criticality.Warn` instead of `Warning`, `Theme.Pale` instead of removed `Phosphor`, Control font overrides for LineEdit/ItemList, `TryAddRawEntry` null-author + `JournalEntry?` return handling) |
| `godot --headless -- --data-integrity-selftest` | ✅ PASS — 299 catalogs, 0 errors, 0 warnings (fresh assembly verified: contains `LeatherworkArchiveSystem`) |
| `godot --headless -- --bridge-selftest` | ✅ PASS (exit 0) |
| `godot --headless -- --save-store-checksum-selftest` | ✅ PASS — Gate A includes the new `leatherwork_archive` store |
| Contract matrices | `SaveSectionRegistry` 171→172; `VersionReportContractTests` 166 envelopes; `ARCHITECTURE_TEST_MAP.md` regenerated (172 subsystems, 100% mechanical evidence) via `scripts/ci/generate-architecture-map.py` |
| Review hardening (cross-tool QA) | `Firewall_ProjectionSurface_HasNoMechanicalAPIs` strengthened from denylist to **mutator allowlist + read-only property assertion** during review |

## 9. Final invariant — verified

**Leather records preserve how survivors learned to turn scarce hides into
durable goods; they never manufacture the hides, define item condition, or
replace the systems that own crafting and equipment.**
