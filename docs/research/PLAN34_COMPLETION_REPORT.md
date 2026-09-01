# Plan 34 Completion Report — Research Tree Externalization (2026-09-01)

## Summary

| Metric | Plan 34 target | Delivered |
|---|---|---|
| Authoritative catalog | one JSON file, schema-versioned | `Assets/StreamingAssets/Data/research_knowledge.json`, `schema_version: 1` (name per repository truth — see reconciliation) |
| Node count | 40 | **56** (target exceeded by the landed content; quality audited instead) |
| Original nodes preserved | 15 IDs, value/behavior parity | 15/15 IDs + 16 blueprint nodes: 31/31 field-identical, behavior parity proven |
| Hardcoded definitions | removed | `RegisterDefaults()` deleted from Core; zero production definitions outside JSON |
| Loader | reused or minimal + justified | **Reused** `ResearchKnowledgeCatalogLoader` (justification Q&A below) |
| New validator work | DAG + cross-refs fail early | `ValidateDag` wired into new `--research-catalog-selftest` gate; cross-catalog refs checked |
| Save compatibility | old saves intact, round-trip | ID-based saves unmoved; **research persistence added** (previously none existed) |
| Categories | six substantive | survival 11, engineering 13, medical 9, science 8, combat 8, scavenging 7 |

## Plan 26A reconciliation

The externalization half of Plan 34 had already landed under `research_knowledge.json` /
`ResearchKnowledgeCatalogLoader`. Per the plan's critical overlap rule this plan did **not** fork a
second catalog; it completed the landed authority. Full delta matrix:
[PLAN26A_PLAN34_RECONCILIATION.md](PLAN26A_PLAN34_RECONCILIATION.md). What Plan 34 added:

1. **Authority repair** — deleted `RegisterDefaults()` (31 defs) and the loader's silent fallback;
   `research_knowledge.json` is now the sole production authority (§1.1, §1.10).
2. **Live wiring** — `LoadCatalog` had no production caller; the game ran on hardcoded nodes from
   `CraftingHostSession`'s ctor side effect. Now: `Main.EnsureSharedResearch()` (single shared engine,
   catalog-loaded), `ResearchHostSession.Create(dataDir, engine)` wraps the shared instance,
   `CraftingHostSession.Create` loads the catalog when it owns the instance.
3. **Breakthrough grants** — `breakthrough_item` was logged but never awarded. Added
   `ResearchSystem.OnResearchCompleted` (transition-only) → `CraftingHostSession` grants the item
   once; restore never re-grants (§34C.15).
4. **Runtime fabrication deleted** — `WorkshopReverseEngineeringSystem` no longer invents research
   defs for unknown relic unlock IDs; it warns (data defect) instead (§1.1).
5. **Research save persistence** — previously research progress was never saved at all. Added
   `ResearchSaveStore` + `SaveSectionRegistry` entry + `SaveResearch()` + restore-on-create, with
   unknown-ID preservation on capture (§34D.5).
6. **Gates** — new `--research-catalog-selftest` (count, DAG, original-15 presence, breakthrough
   items, relic/manual/autopsy cross-refs); permanent parity + integration test suites.
7. **UI truth** — atlas status rail now reports live catalog counts (was hardcoded fixture numbers);
   phantom `res_rad_mapping` ID in the UI selftest replaced with a real catalog ID.

## New-system justification (§7)

1. **Did a compatible loader exist?** Yes — `ResearchKnowledgeCatalogLoader`; reused unchanged
   except removing its silent fallback.
2. **Why is a new loader necessary?** Not applicable (no new loader).
3. **What does the loader explicitly not own?** Progress, eligibility beyond structural parsing,
   material consumption, unlock application (host grants items), save state, UI, timing.
4. **Additional mechanical systems added?** No new engines. Additions are: one Core completion
   event (`OnResearchCompleted`), one host save-store façade (`ResearchSaveStore`, the standard
   `SaveStore<T>` pattern), and host wiring. Breakthrough granting lives in the host because Core
   has no inventory dependency.

## Graph evidence

- Roots 25 · leaves 40 · max depth 2 · DAG-valid (unit-tested + gate).
- Categories: six, all substantive (min 7 nodes).
- Cross-category prerequisites exist (e.g. cloud seeding ← radio + shielding) — selective, per §34B.13.
- Reachability: all prerequisites resolve; no cycles; no orphans (validator + tests).

## Unlock evidence

- 32 `breakthrough_item` refs — 32/32 resolve and are now actually granted on completion.
- 15 distinct relic `research_unlock_id`s — all resolve (contract tests + gate).
- 12 manual knowledge_unlocks, 4 autopsy knowledge grants — all resolve via real consumers
  (`LibraryStudySystem.UnlockManual`, `AutopsySystem`).
- Unresolved targets: **0**. Advanced nodes without a live unlock: **0**.

## Skill gates (§34C.4) — Case C

No `skill_*` gates exist in the catalog and `SkillProgressionSystem` has no research-eligibility
hook. No speculative skill references were authored. Documented as a Plan 33 follow-on.

## Balance evidence (§34E.6–7, right-sized)

Flat day-costs (5–18 days), single unlock type (breakthrough item), no global multipliers — no
dominant-branch mechanism exists. Cost tiers: foundations 5–8 days; chains 10–16; advanced
convergences 12–18. Dead-node review: every node either feeds a prerequisite chain or grants a
breakthrough item; the 16 blueprint nodes are consumed by the live relic workshop loop; mid-tier
nodes are consumed as prerequisites by deeper nodes. No dead nodes found.

## Verification (exact commands, final state)

| Gate | Result |
|---|---|
| `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | PASS — 0 errors |
| `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **PASS — 6008/6008** (incl. 33+ research tests: parity fixture, loader, save integration, workshop contract) |
| `dotnet build Ashfall.csproj` | PASS — 0 errors (pre-existing warnings in unrelated concurrent-stream files only) |
| `godot --headless --path . -- --data-integrity-selftest` | PASS — 0 findings across 172 catalogs |
| `godot --headless --path . -- --bridge-selftest` | PASS |
| `godot --headless --path . -- --research-catalog-selftest` (new) | PASS — "56 nodes, DAG valid, cross-refs resolve" |
| `godot --headless --path . -- --panel-bind-lifecycle-selftest` | PASS — Gate 5 ResearchPanel lifecycle with JSON catalog |

## Remaining known limitations / follow-ons

1. **Day-progress research has no player-facing start UI.** `ResearchSystem.StartResearch` +
   `Tick` are exercised by tests and host plumbing but no production surface starts a node; only
   the workshop relic loop and manual/autopsy grants advance the tree today. A "queue research"
   action on the research panel is the natural follow-on.
2. **ResearchAtlasPanel deep content is still a fixture shell** (grids/dossier rows) — the status
   rail is now live, but the grid population belongs to the UI dashboard stream's atlas work.
3. **Skill gates** (Plan 33 Case C), **manual cost-reduction integration**, and any future
   relic/blueprint research links remain deferred per the plan's non-goals.
4. `field_guide` (another stream's concurrent registry addition) was missing its
   ARCHITECTURE_TEST_MAP row; a minimal row was added to keep the shared gate green — that stream
   may want to enrich it.

## Concurrent-stream note

The repository is worked by multiple concurrent AI streams. During this implementation:
`ShelterDecorSelfTest`/`field_guide` registry entries landed mid-flight; the Core
`HostCliAction` enum gained `ResearchCatalogSelfTest` from a parallel edit; the save-section count
contract moved twice; and two transient foreign compile breaks (WildlifeTrapping, FactionTerritory)
self-healed. All final gates above were run against the settled tree.
