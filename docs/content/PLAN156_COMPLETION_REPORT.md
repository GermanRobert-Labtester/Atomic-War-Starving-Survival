# Plan 156 — Completion Report

Plan 156 activates the two existing paper and printing knowledge corpora through the shared narrative discovery and JournalCodex seam. The source catalogs remain separate and read-only.

## Delivered

- 60 source records retained: 30 PaperMakingCatalog and 30 PaperPrintingCatalog.
- Eight source files mapped to one bounded PaperPrintSourceAdapter.
- 60 stable disc_paper_* projections added to the narrative discovery manifest.
- Four explicit cross-catalog related-record pairs retained as links.
- Eight canonical producer contexts wired: four shelter workshop/foundry contexts, three map locations and the existing archive-desk bunker context.
- Fourteen paper-making records have alternate explicit producer IDs; twelve bridge room and location/archive context classes as covered by tests.
- Existing JournalSystem discovery state and JournalCodex presentation reused.
- No save section, paper/ink item, recipe, research node, live template, faction effect, propaganda effect or mechanical measurement consumer added.
- Seven overconfident chemical or service-life claims were softened in the authored prose; all measurements remain display-only observations.

## Source and family counts

| Source catalog | Records | Families |
|---|---:|---|
| PaperMakingCatalog | 30 | Hollander 8; deckle 8; screw press 7; tub sizing 7 |
| PaperPrintingCatalog | 30 | rag pulp 8; iron-gall ink 8; typographic lead wear 7; stencil artifact 7 |

## Overlap and identity decisions

The two pulping corpora are complementary authored observations, not one runtime production table. The Year 02 linen/Hollander pair, Year 07 sizing/feathering pair, calcium-buffer pair and press/felt pair are linked as corroborating or related observations. All other shared vocabulary remains distinct. Source catalog and source record identity are preserved in every projection.

No station identifier was promoted to a live machine or location ID. The producer matrix uses only existing room and location IDs proven in the repository. Missing paper, rag, ink, gelatin, alum and tannin item identities were not manufactured.

## Technical and narrative accuracy corrections

The following prose-only corrections preserve the authored records and their measurements while removing unsupported certainty:

| Source record | Correction |
|---|---|
| hollander_beater_calcium_carbonate_buffer_loading | Replaced the five-century guarantee with a conditional service-life observation dependent on dry, stable storage. |
| rag_pulp_wood_ash_caustic_delignification | Replaced the chemically over-specific “saturated potassium hydroxide” claim with alkaline liquor boiled from boiler-grate ash; the recorded pH values remain unchanged. |
| rag_pulp_calcium_carbonate_alkaline_buffering | Recast the three-century lifespan as a projection recorded by the log rather than a guarantee. |
| ink_assay_oak_gall_tannic_acid_maceration | Replaced “pure gallic acid” and an overbroad cellulose-bond claim with gallic acid and related compounds, and ink fixing as it oxidized. |
| ink_assay_copperas_ferrous_sulfate_ratio | Replaced the unsupported free-sulfuric-acid attribution with the observed acidic liquor attacking metal. |
| ink_assay_paper_acid_burnthrough_corrosion | Replaced the specific sulfuric-acid byproduct and total charring claim with acidic byproducts damaging fibers along many strokes. |
| ink_assay_soot_lampblack_carbon_ink_dispersion | Replaced “inert”, “completely immune” and “permanent” with a near-neutral, comparatively resistant archival-use observation. |

## Presentation and authority

The JournalCodex Events surface renders each discovered record with its family, facility/station provenance, authored timestamp, prose and a safe technical summary. Numeric claims use the label “Authored measurement — not a live production value”. No adapter path calls inventory, crafting, research, faction, economy, medical, authenticity or propaganda authorities.

The existing UndergroundPrintingPressPanel remains deferred because it is a hardcoded prototype with unbound paper/ink/morale/leaflet claims. A generated current-state form is deferred until an existing typed live owner can supply one.

## Persistence

Discovery is persisted through the existing JournalSystem knowledge keys and checked campaign journal section. Source definitions and transcripts are not copied into save state. Old saves do not receive retroactive discoveries; loading reconstructs discovered IDs and never replays discovery side effects. Reordered source files or manifest rows do not change identity.

## Verification

| Command/check | Result |
|---|---|
| Source JSON parse and 60-row inventory check | PASS |
| Manifest parse, 213 total projections, 213 unique discovery IDs | PASS |
| dotnet build Ashfall.Core/Ashfall.Core.csproj --no-restore --verbosity:minimal | PASS, 0 warnings, 0 errors |
| Full xUnit suite, clean compilation before concurrent test drift | PASS, 10,491/10,491 |
| Prebuilt narrative and Plan 156 tests | PASS, 13/13 |
| Prebuilt PaperPrintingCatalog + Plan 156 + content-utilization tests | PASS, 47/47 |
| Current focused test compilation | BLOCKED by LeatherworkArchiveTests.cs referencing missing DeepLoreLocationCatalogLoader from concurrent Plan 159/related work |
| godot --headless --path . -- --data-integrity-selftest | PASS, 0 findings across 299 catalogs |
| godot --headless --path . -- --content-utilization-selftest | PASS, 583 catalogs, 0 orphaned |
| godot --headless --path . -- --journal-selftest | PASS, 25/25 on the last successfully built host binary |
| python3 scripts/ci/run-gates.py --tier fast | BLOCKED at gate 1/47 by the pre-existing blank-EOF finding in BunkerCourtCatalog.cs |
| dotnet build Ashfall.csproj --no-restore --verbosity:minimal | BLOCKED by nine pre-existing BlackProjectsArchivePanel API errors from concurrent work |

The Godot self-tests were run against the last successfully built host binary because the concurrent host build is blocked by BlackProjectsArchivePanel. Core scanner tests, the earlier full Core suite, the prebuilt focused subset and the source compiler cover the new Plan 156 scanner/projection code. A later current-source test compile is blocked by the unrelated missing DeepLoreLocationCatalogLoader reference in LeatherworkArchiveTests; both blockers are recorded rather than folded into this plan. The new source-level JournalSelfTest assertions therefore still require a clean host build before they can execute in Godot.

## Files

Runtime: Assets/Ashfall.Core/Narrative/NarrativeDiscoveryCatalog.cs, Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs, Assets/StreamingAssets/Data/narrative_discovery_manifest.json, src/Journal/JournalCatalogData.cs, src/Journal/JournalCodex.cs, src/Journal/JournalSelfTest.cs, src/Host/ContentUtilizationRuntimeCollector.cs, src/Main.Narrative.cs, src/Main.UiHandlers.cs and src/Main.ExpandedShelterSystems.cs.

Tests: Ashfall.Core.Tests/PaperPrintingRuntimeActivationTests.cs.

Documentation: PLAN156_BASELINE.md, PAPER_PRINTING_CORPUS_INVENTORY.md, PAPER_PRINTING_OVERLAP_MATRIX.md, PAPER_PRINTING_AUTHORITY_MAP.md, PAPER_PRINTING_DISCOVERY_PRODUCERS.md, PAPER_PRINTING_PROJECTION_MATRIX.md, PLAN156_SAVE_COMPATIBILITY.md, PLAN156_REGRESSION_MATRIX.md and this completion report.
