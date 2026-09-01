# Plan 26A ↔ Plan 34 Reconciliation — delta matrix (2026-09-01)

Plan 34's critical overlap rule: *"Do not implement Plan 26A and Plan 34 as separate catalogs,
loaders, validators, or trees. Repository truth wins. If one has already landed, the other becomes
reconciliation, coverage, content-quality, and missing-integration work against the landed authority."*

Repository truth found at baseline: the externalization half of Plan 34 **already landed** — under the
file name `research_knowledge.json` (not `research_catalog.json`) with loader
`ResearchKnowledgeCatalogLoader`. Per the overlap rule this plan does **not** create a second catalog;
it hardens and completes the landed one. `research_knowledge.json` is the single research authority.

## Delta matrix — Plan 34 requirement vs repository truth

| Plan 34 requirement | Repository truth at baseline | Remaining work |
|---|---|---|
| Create `research_catalog.json`, schema-versioned | `research_knowledge.json` exists, `schema_version: 1` | None — reuse; do not fork names |
| Minimal loader | `ResearchKnowledgeCatalogLoader` exists, engine-agnostic ports | Remove silent `RegisterDefaults` fallback (§1.10) |
| 15→15 parity before removal | 31 C# defs (15 original + 16 blueprint) all value-identical in JSON (0 mismatches) | Pin parity permanently in tests; then delete `RegisterDefaults` |
| Remove hardcoded production definitions | `RegisterDefaults` still live in Core + 4 host call sites | Delete; move defs to test fixture |
| Wire through current composition | `LoadCatalog` exists but is **never called in production**; two parallel instances | Wire `dataDir`-based load into host sessions/Main; unify on `_sharedResearch` |
| DAG validation, orphans, duplicates | `ValidateDag` exists + unit tests | Run it in a headless selftest gate; cover empty catalog |
| Expand to 40 nodes, six substantive categories | **56 nodes**, six substantive categories, DAG-valid | None — target exceeded; quality rules audited instead |
| Every advanced node unlocks a real target | 32/32 `breakthrough_item` resolve | **Grant the item on completion** (chain had no consumer) |
| Skill gates (Plan 33) — Case C if not landed | No skill gates in catalog; `SkillProgressionSystem` uncoupled | None — document Case C; no speculative `skill_*` refs |
| Library manuals (§34C.9) | `library_manuals.json` knowledge_unlocks all resolve; `LibraryStudySystem.UnlockManual` is a live consumer | None — integration is real; document |
| Relic blueprints (Plan 04) / Plan 22 / Plan 10 / Plan 27 | Relic `research_unlock_id` all resolve; autopsy knowledge grants all resolve | Delete runtime def fabrication in workshop; validate refs in selftest |
| Unknown save-node policy (§34D.5) | `CaptureState` **silently drops** non-catalog IDs | Preserve unknown IDs; test |
| Save round-trip matrix (§34D.11) | **Research state is never persisted at all** | Add save store + section + restore; round-trip tests |
| Ordering determinism (§34D.2) | UI sorts ordinal (`OrderBy(key)`); save lists follow registration (JSON file) order — deterministic | Document; keep |
| Export packaging (§34D.8) | `research_knowledge.json` ships under `StreamingAssets/Data` like all catalogs; integrity selftest scans it | Covered by data-integrity selftest; no per-file work |
| Failure policy (§1.10) | Loader warns on parse errors; missing file = silent empty; host treats 0 nodes as OK | Host warns on empty; selftest gate fails on empty/invalid catalog |
| Balance sim / dominance (§34E.6-7) | n/a | Lightweight reachability/cost audit in the completion report; no dominant-branch mechanism exists (flat day costs, single unlock type) |
| Content utilization (§34E.11) | Scanner maps `research_knowledge.json` → loader → system → panel | None |

## Decisions

1. **Authority:** `research_knowledge.json` is the sole authored research authority. `RegisterDefaults()`
   is deleted from Core; its 31 defs survive only as a test fixture for parity regression.
2. **Composition:** one shared `ResearchSystem` per campaign (`_sharedResearch`), created once, loaded
   from `_dataDir`; `ResearchHostSession` wraps it instead of owning a second instance.
   `CraftingHostSession.Create` loads the catalog when it owns the instance (mirrors its relic/pharma loads).
3. **Unlock semantics:** `breakthrough_item` becomes a real grant (host-side, transition-only, never on restore).
4. **Persistence:** research state joins the campaign envelope as a `SaveStore<T>` section (same pattern as
   every other host store), with unknown-ID preservation on capture.
5. **No schema change:** the landed wire schema (`knowledge_nodes`, snake_case, `breakthrough_item`) is kept.
