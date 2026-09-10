# Plan 160 Completion Report

Plan 160 activates the authored BoneHornCarvingCatalog through the existing narrative discovery and JournalCodex seam. The activation is deliberately a read-only projection: the catalog can explain low-resource material culture, but it cannot create an item, kill an animal, alter a recipe, or change a tool stat.

Counts:

- 30 source records total.
- 8 bone degreasing/preparation logs.
- 8 antler/horn sawing records.
- 7 scraping/polishing reports.
- 7 needle/awl/hook assays.
- 30 stable narrative discovery projections.
- 243 total records in the current combined narrative manifest after adding Plan 160.
- Seven producer contexts with Plan 160 records.

Implementation:

- Added BoneHornRuntimeContract and BoneHornSourceAdapter in the Core narrative projection layer.
- Added BoneHornCarving to JournalCatalogs and CatalogJsonLoader.
- Added 30 producer-backed entries to narrative_discovery_manifest.json.
- Added workshop, location-inspection and archive-desk calls in the existing Godot routes.
- Added Plan 160 loader/registry/consumer/UI evidence to ContentUtilizationScanner and runtime collection.
- Added JournalSelfTest source/projection checks.
- Added BoneHornRuntimeActivationTests for counts, projection labels, producer reachability, dog/shed safeguards, exact-once discovery, restore, reload and duplicate IDs.

Identity and continuity decisions:

- No animal source maps to a canonical individual or living companion.
- dog is generic historical material; antler_horn_001 records an old shed and does not imply deer death.
- rat and goat labels have semantically related catalog entries but no exact identity match, so no wildlife link is configured.
- saw_tool_id and bone_blank_id are unresolved historical labels. Similar canonical items were not aliased.
- No process chain is asserted. Compatible names across files are not stable authored foreign keys.

Activated records:

- All 30 source records are reachable through the Journal discovery projection.
- All four families have producer-backed records.
- Workshop, heavy workshop, precision workshop, automated abattoir, forestry compound, St. Brigid’s Almshouse and government bunker provide real routes.

Deferred or intentionally absent:

- Item inspection integration is deferred because no exact bone, horn, antler, blank, hacksaw or generic needle/awl/hook IDs resolve in the current item authority.
- Crafting, hunting, butchery, scavenging loot, fishing, sewing bonuses, research unlocks, trade pricing, condition, combat and medical effects remain deferred by authority contract.
- No new paper, bone, horn, carcass, wildlife or cultural item was added.

Save behavior:

- No new save store or catalog state was added.
- JournalSystem knowledge keys persist discoveries; old saves do not auto-discover missed records.
- Restore reconstructs discovery state and never replays an effect because there are no Plan 160 effects.

Source content was preserved. The runtime labels prep days and point angles as authored observations and explicitly avoids sterility, quality, damage, fishing, repair and current-resource claims. No prose rewrite was required.

Verification:

- Source and manifest JSON were parsed after the activation: 30 Plan 160 entries, 243 combined entries, 243 unique discovery IDs, 40 source catalogs.
- Focused Plan 160 tests pass 8/8, the shared narrative projection tests pass 20/20, and the full Core suite passes 10,530/10,530. The test project compiled successfully; the pre-existing oral-lore xUnit analyzer warning remains.
- Godot data-integrity-selftest passes with 0 findings across 299 catalogs. The fast gate passes hygiene, JSON policy, Core build and Core tests before stopping at the host build.
- The Godot host build is currently blocked by nine unrelated BlackProjectsArchivePanel.cs API errors. The earlier fast-gate blank-EOF finding in BunkerCourtCatalog.cs was corrected as a hygiene-only change; the gate then reached the unrelated host blocker.
