# PLAN-CONTENT-PIPELINE-QA-77 — Author, Validate, Consume, Present, Extract

**Wave 7 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-DATA-AUTHORITY-14, PLAN-NARRATIVE-GRAPH-18,
PLAN-LOCALIZATION-READINESS-52.
**Implementation scaffold:** [`PLAN-CONTENT-PIPELINE-QA-77_APPENDIX-A_SCAFFOLD.md`](PLAN-CONTENT-PIPELINE-QA-77_APPENDIX-A_SCAFFOLD.md) — source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no new authoring tool; no runtime writing of catalogs.

## Outcome
Content enters the repo through many shapes (items, recipes, quests, narrative,
weather, faction, audio, assets) and the validation is spread across the
data-integrity selftest, catalog registry, narrative continuity, field
consumption, and asset gates. There is no single **pipeline definition** that
a content author follows, and no PR-level check that every content type passed
all five stages.

| Stage | Check |
|---|---|
| Author | schema envelope, snake_case, unique ids, no placeholders |
| Validate | data-integrity + family rules + reference graph (dangling fails) |
| Consume | loader + consumer exists (catalog registry + field consumption) |
| Present | at least one surface: journal/panel/radio/audio with a key |
| Extract | localization-ready (no concatenation, context available) |

## Evidence
- Gates: data-integrity (318+ catalogs), catalog registry (~605), content-utilization, narrative continuity (882 warnings), asset registry, audio catalogs.
- Field-level gap already proven (three sealed cases) → PLAN-DATA-CONSUMER-22.
- Flag-level gap proven (832 set / 8 read) → PLAN-NARRATIVE-GRAPH-18.
- Extraction tooling: `extract_l10n_inventory.py`; freeze policy exists.

## Packages
- **CP-77A** pipeline doc: one page per content type with the five stages and the exact commands.
- **CP-77B** per-type checklists in `docs/content/<type>.md` (items, recipes, quests, narrative, weather, factions, audio, assets).
- **CP-77C** PR template additions: author attests the five stages; CI verifies what is verifiable.
- **CP-77D** unified report: one command prints all content findings for a diff (changed catalogs only), aggregated per row.
- **CP-77E** exemplar content changes: one tiny valid change per type committed as a reference and tested.
- **CP-77F** authoring guard: placeholder/TODO/lorem scan and a duplicate-id scan run on changed files.

## Acceptance & verification
- A deliberately broken row in each type fails the pipeline with the exact stage named.
- `godot --headless --path . -- --data-integrity-selftest`; `--content-utilization-selftest`; `python3 scripts/ci/narrative-continuity.py --check`; `extract_l10n_inventory.py --check`.

## Risks
Process weight → checklists are short; the unified report runs on changed files only.

---

## 6. Expanded census (12 files · 4,625 lines)

Scope: `Assets/Ashfall.Core/Content/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 11 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CollectibleCatalogIntegrityValidator.cs` | 433 | Support | — | 0 | 0 | 0 |
| `ContentAcceptanceLadder.cs` | 102 | Support | — | 0 | 0 | 0 |
| `ContentAcceptancePipeline.cs` | 150 | Support | — | 0 | 0 | 0 |
| `ContentAcceptanceRung.cs` | 40 | Support | — | 0 | 0 | 0 |
| `ContentDeepChainGate.cs` | 445 | Support | — | 0 | 0 | 0 |
| `ContentExemption.cs` | 150 | Support | — | 0 | 0 | 0 |
| `ContentOrphanCertificationEngine.cs` | 136 | System | — | 0 | 0 | 0 |
| `ContentUtilizationGate.cs` | 247 | Support | — | 0 | 0 | 0 |
| `ContentUtilizationGraph.cs` | 399 | Support | — | 0 | 0 | 0 |
| `ContentUtilizationInstrumentation.cs` | 223 | Support | — | 0 | 0 | 0 |
| `ContentUtilizationManifest.cs` | 227 | Support | — | 0 | 0 | 0 |
| `ContentUtilizationScanner.cs` | 2073 | Support | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `weather_route_gates.json` | object[2 keys] |
| `standing_gates.json` | object[2 keys] |
| `blast_gate_mechanical_audits.json` | array[8] |

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Content/` |
| Test references | 23 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 0 empty-catch sites routed through Plan 35's rules |
| Premise | the **premise** file(s) above must match the plan's stated line counts before edits |

## 9. Rollout sequence

1. Premise re-check: premise files unchanged since authoring (hash/mtime), or update the plan.
2. Interfaces: wire through the named owner; do not add a parallel store.
3. State: if capture/restore exists, register per Plan 1 Appendix Q; else state the system is stateless.
4. Data: resolve domain catalogs or report the loader path.
5. Verification: focused region + the plan's own acceptance table.
6. Regression: re-run this census; a changed file is a finding.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Premise file | matches its stated surface; no scope drift |
| Interface/wiring | one owner per state, no parallel store |
| State/save | round-trip or explicit stateless verdict |
| Data binding | catalog resolves or loader path documented |
| Verification | focused region green; census delta recorded |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not widen the plan's scope.

---

## 12. Cross-plan coupling

Domain method: plan-body `.cs` enumeration.
Domain files: 12. Other plans referencing them: **8**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-CONTENT-ACCEPTANCE-FAMILY-TRUTH-274` | 12 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `PLAN-UNBLOCK-03` | 1 |
| `PLAN-LAUNCH-FACE-06` | 1 |
| `EVIDENCE` | 1 |
| `PLAN-CORE-ONLY-REGISTRY-11` | 1 |
| `PLAN-ARCHITECTURE-BOUNDARY-31` | 1 |
| `PLAN-MORTUARY-MEMORIAL-TRUTH-123` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `CP-77A` | `ContentAcceptancePipeline.cs`, `ContentAcceptanceLadder.cs`, `ContentAcceptanceRung.cs` |
| `CP-77B` | `ContentAcceptanceLadder.cs`, `ContentAcceptancePipeline.cs`, `ContentAcceptanceRung.cs` |
| `CP-77C` | no name match — resolve at claim time |
| `CP-77D` | `ContentAcceptanceLadder.cs`, `ContentAcceptancePipeline.cs`, `ContentAcceptanceRung.cs` |
| `CP-77E` | `CollectibleCatalogIntegrityValidator.cs`, `ContentAcceptanceLadder.cs`, `ContentAcceptancePipeline.cs` |
| `CP-77F` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 12; intra-domain edges: **10**; isolated files:
**2**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `ContentAcceptanceLadder` | `ContentAcceptanceRung` |
| `ContentAcceptancePipeline` | `ContentAcceptanceLadder` |
| `ContentAcceptancePipeline` | `ContentAcceptanceRung` |
| `ContentDeepChainGate` | `ContentUtilizationGraph` |
| `ContentExemption` | `ContentUtilizationGraph` |
| `ContentUtilizationGate` | `ContentUtilizationGraph` |
| `ContentUtilizationGraph` | `ContentAcceptanceRung` |
| `ContentUtilizationInstrumentation` | `ContentUtilizationGraph` |
| `ContentUtilizationManifest` | `ContentUtilizationGraph` |
| `ContentUtilizationScanner` | `ContentUtilizationGraph` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `ContentUtilizationGraph` | 6 |
| `ContentAcceptanceRung` | 3 |
| `ContentAcceptanceLadder` | 1 |
| `CollectibleCatalogIntegrityValidator` | 0 |
| `ContentAcceptancePipeline` | 0 |
| `ContentDeepChainGate` | 0 |
| `ContentExemption` | 0 |
| `ContentOrphanCertificationEngine` | 0 |
| `ContentUtilizationGate` | 0 |
| `ContentUtilizationInstrumentation` | 0 |

**Class split:** hub 2 · sink 1 · source 7 · isolated 2.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 12. Host files: **4** · Test files: **13** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 4 | `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/ContentUtilizationSelfTest.cs`, `src/Host/HostCli.SelfTests.cs`, `src/UI/FeedbackPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 13 | `Ashfall.Core.Tests/BoneHornRuntimeActivationTests.cs`, `Ashfall.Core.Tests/Content/ContentAcceptanceLadderTests.cs`, `Ashfall.Core.Tests/Content/ContentAcceptancePipelineTests.cs`, `Ashfall.Core.Tests/Content/ContentOrphanCertificationEngineTests.cs`, `Ashfall.Core.Tests/Content/Plan41_45MemoryAcceptanceIntegrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **2** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `collectible_discovery` |
| `deep_well` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **7** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--data-integrity-selftest` |
| `--deep-coast-host-selftest` |
| `--deep-coast-playthrough` |
| `--deep-coast-route-selftest` |
| `--deep-coast-selftest` |
| `--selftest-manifest` |
| `--test-manifest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **0**.

| Event | First declaration |
|---|---|
| — | no event name shares a token with this domain |

**Verdict:** no event name shares a token with this domain — the domain is command-polled, data-driven, or silently unreachable. Confirm against the event catalog before treating it as a gap.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **7**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/deep_lore_locations.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |
| `Assets/StreamingAssets/Data/mod_manifest_schema.json` |
| `Assets/StreamingAssets/Data/narrative/blast_gate_mechanical_audits.json` |
| `Assets/StreamingAssets/Data/narrative/deep_lore_texts.json` |
| `Assets/StreamingAssets/Data/narrative_discovery_manifest.json` |
| `Assets/StreamingAssets/Data/whitelists/orphan_knocks.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **0** (0 files, 0 cases).

| Region | Files | Cases |
|---|---:|---:|
| — | no test region shares a token with this domain |

**Verdict:** no test region shares a token with this domain. Region coverage is directory-based, so check root-level test files too (560 exist) before concluding coverage is absent.

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **11**
(1 of them panels/HUD).

| Host file |
|---|
| `src/Host/AssetCoverageScanner.cs` |
| `src/Host/CollectibleDiscoverySaveStore.cs` |
| `src/Host/CollectibleEffectDispatcher.cs` |
| `src/Host/ContentUtilizationRuntimeCollector.cs` |
| `src/Host/ContentUtilizationSelfTest.cs` |
| `src/Host/DeepCoastHostSession.cs` |
| `src/Host/DeepWellHostSession.cs` |
| `src/Host/DeepWellSaveStore.cs` |
| `src/Host/HostCli.SelfTestManifest.cs` |
| `src/Main.DeepWell.cs` |
| `src/UI/DeepCoastPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **2**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `collectible_discovery` | no |
| `deep_well` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `deep_coast` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **5**
(CODEX_ONLY 2, GAMEPLAY_CONSUMED 1, OPTIONAL 2).

| Catalog | Classification |
|---|---|
| `deep_lore_locations.json` | GAMEPLAY_CONSUMED |
| `deep_lore_survivor_fields.json` | OPTIONAL |
| `narrative/blast_gate_mechanical_audits.json` | CODEX_ONLY |
| `narrative/deep_lore_texts.json` | CODEX_ONLY |
| `whitelists/orphan_knocks.json` | OPTIONAL |

**Verdict:** matched catalogs are classified as gameplay-consumed — the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 8
**Surface:** save sections 2 (laddered 0) · RNG streams 1 · host files 12 · catalogs 12 · test regions 0 · flags 7

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-CONTENT-PIPELINE-QA-77
wave: 7
status: PROPOSED — foreman claim required
packages: CP-77A, CP-77B, CP-77C, CP-77D, CP-77E, CP-77F
claim paths:
  - src/Host/AssetCoverageScanner.cs  # §19 candidate host surface
  - src/Host/CollectibleDiscoverySaveStore.cs  # §19 candidate host surface
  - src/Host/CollectibleEffectDispatcher.cs  # §19 candidate host surface
  - src/Host/ContentUtilizationRuntimeCollector.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/deep_lore_locations.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/deep_lore_survivor_fields.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --data-integrity-selftest
dependencies:
  - coordinate: 8 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | yes |
| depends | yes |
| non_goals | yes |
| outcome | yes |
| evidence | yes |
| packages | yes |
| acceptance | yes |
| risks | yes |
| verification | yes |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** none — claim-ready.
