# ASHFALL Visual Asset Forensic Audit

Date: 2026-09-03
Audit anchor: c09c3e67a6e88920690767899b586ab85fecb84c
Mode: inventory and analysis only; no assets moved, deleted, converted, or replaced

## Verdict

The visual asset pipeline is structurally organized and LFS/sidecar health is green, but content coverage is not release-ready. Only 512 of 1,295 catalog-backed visual IDs resolve through the production registry (39.54%), 715 active files are redundant exact copies, and several catalog IDs resolve to visibly generic placeholders. One tracked .png is base64 text rather than a PNG image.

The current hard gate samples 50 references. The complete 1,295-ID sweep is explicitly report-only, so CI can remain green while 783 visual references have no resolved asset.

## Inventory

| Area | Files | Bytes |
|---|---:|---:|
| assets/art | 1,762 | 65,008,819 |
| assets/sprites | 577 | 3,844,291 |
| assets/ui | 319 | 25,631,600 |
| assets/quarantine | 1,099 | 156,401,393 |
| Total | 3,757 | 250,886,103 |
| Active excluding quarantine | 2,658 | 94,484,710 |

Format census:

| Format | Files |
|---|---:|
| PNG | 1,083 |
| JPG/JPEG | 2,627 |
| SVG | 47 |

Quarantine contains about 149.2 MiB, or 62.3% of all visual source bytes, under the live Godot assets tree. No .gdignore or explicit export exclusion was found. The sources are unreferenced by production code but still contribute repository and import-scan burden.

## Production coverage

| Category | Catalog IDs | Resolved | Missing | Coverage |
|---|---:|---:|---:|---:|
| Items | 682 | 317 | 365 | 46.48% |
| Portraits | 256 | 108 | 148 | 42.19% |
| Locations | 309 | 50 | 259 | 16.18% |
| Factions | 48 | 37 | 11 | 77.08% |
| Total | 1,295 | 512 | 783 | 39.54% |

The previous dated visual report recorded 481 of 834 references (57.67%). The current tree gained 31 resolutions while catalog demand grew by 461 IDs, reducing coverage by 18.13 percentage points.

Production behavior:

- AssetRegistrySelfTest.Run defaults to topCount=50 at src/Host/AssetRegistry.cs:549.
- It allocates roughly one third each to item, portrait, and location checks.
- AssetCoverageReport labels the full sweep report-only at src/Host/AssetRegistry.cs:1045-1147.
- The sampled gate passed 50/50 while the full sweep reported 783 missing.

## Integrity and importability

The source/sidecar orphan sweep passed with 0 pair orphans. LFS health also passed: Git LFS 3.7.1, 3,747 LFS-managed files, successful fsck, and hydrated content.

Those checks do not validate file signatures. assets/sprites/Map/marker_safe.png is ASCII text containing an undecoded base64 payload beginning iVBOR rather than PNG binary bytes. Its .import sidecar records valid=false. No current production reference was found, so runtime impact is presently low, but it proves the integrity gate is incomplete.

The visual scanner listed 48 load errors. Forty-seven were SVG/Pillow incompatibilities and are not evidence of corrupt SVG sources. marker_safe.png was the only confirmed corrupt raster payload.

The canonical Godot import scan failed with exit 134 during the filesystem scan, both in the normal sandbox and after approved unrestricted retry. That failure blocks an importability certification for the full visual tree.

## Exact duplicates and placeholder families

| Scope | Duplicate groups | Files in groups | Redundant copies | Estimated redundant bytes |
|---|---:|---:|---:|---:|
| All visual sources | 131 | 1,629 | 1,498 | 23,586,221 |
| Active, excluding quarantine | 71 | 786 | 715 | 3,637,657 |
| assets/art only | 68 | 760 | 692 | 3,610,277 |

Large active groups include 66 encounter IDs sharing one triangle icon, 63 ammo IDs sharing one bullet image, and several groups of 38-41 unrelated IDs sharing generic rounded-rectangle art. Visual contact-sheet inspection confirmed that these are placeholder families, not merely metadata duplicates.

Six exact-pixel groups directly affect 18 catalog resolutions:

1. item_anchor_notes, item_suitcase_locked, undelivered_mail, blood_bag, and portrait marcus_olejnik.
2. item_teddy_bear/teddy_bear, photo_album, and item_car_keys.
3. pipe_wrench, spoiled_meat, and portrait elena_vasquez.
4. ammo_9x19 and ammo_762x54r.
5. childs_drawing and portrait suki_tanaka.
6. rural_gas_station and government_bunker.

The teddy-bear pair may be a legitimate alias. The cross-domain item/portrait and location pairs are semantically implausible and should be treated as confirmed placeholder collisions.

## Tooling discrepancies

### visual_asset_audit.py

File inventory, hashes, dimensions, and duplicate detection are reliable. Category counts are not: the classifier compares strings such as sprites/Items/Medical against individual Path.parts, collapsing many files into unclassified/general buckets.

### visual_wiring_trace.py

The tool strips item_, weapon_, ammo_, and med_ prefixes and invents extra candidates. Production AssetRegistry explicitly adds selected prefixes but does not apply the same stripping behavior. Its historical 814/1,114 resolution claim is therefore not equivalent to runtime resolution and should not be presented as production coverage.

### asset-orphan-sweep.sh

The sweep correctly proves that expected source/import pairs exist. It does not inspect file magic, decode payloads, or validate Godot import success, which is why marker_safe.png passes.

## Priority remediation

1. Make the Godot import scan stable and mandatory before further asset certification.
2. Gate the complete production registry sweep with an explicit minimum coverage threshold; locations are the weakest category.
3. Add signature and decode validation for raster sources, then repair or remove marker_safe.png according to its intended use.
4. Move quarantine outside the imported assets tree or exclude it explicitly after retention review.
5. Replace cross-domain and high-fan-out generic placeholders in player-visible priority order.
6. Align visual tooling candidate resolution with AssetRegistry and fix category classification.
7. Deduplicate only after reference-aware review; no automatic deletion should be used.

## Verification

| Check | Result |
|---|---|
| LFS health | PASS |
| Source/import sidecar pairs | PASS — 0 orphans |
| Sampled asset registry gate | PASS — 50/50 |
| Full catalog coverage | REPORT — 512/1,295 |
| Independent raster inspection | FAIL — 1 corrupt .png |
| Godot full import | FAIL — exit 134 |

No visual mutation was performed.
