# PLAN-CONTENT-ACCEPTANCE-FAMILY-TRUTH-274 — Acceptance Ladders & Utilization Gates

**Wave 19 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-CONTENT-PIPELINE-QA-77, PLAN-DATA-AUTHORITY-14, PLAN-TEST-WELFARE-17.
**Non-goals:** no content edits; the family is audited as tooling with
invocation truth.

## 1. Outcome
**12 `Content/` files** are referenced by no plan: `ContentAcceptanceLadder`,
`ContentAcceptancePipeline`, `ContentAcceptanceRung`, `ContentUtilizationGate`,
`ContentDeepChainGate`, `CatalogDefinitionCounter`,
`CollectibleCatalogIntegrityValidator`, `ContentExemption`. These are content
QA gates — valuable only if something **runs** them. An uninvoked gate is worse
than none because it implies coverage.

| Deliverable | Detail |
|---|---|
| Invocation truth | every gate/resolver names where it is invoked (CI script, test, loader) |
| Uninvoked report | gates with no invocation are listed and routed (wire or retire) |
| Counter/ladder semantics | documented thresholds and rung transitions |
| Exemption audit | exemptions are enumerated with reasons; no blanket bypass |
| CI tie-in | Plan 20's gate manifest includes any newly wired check |

## 2. Evidence
- 12 `Content/` basenames absent from every plan body (Wave 19 file-level audit).
- Plan 14's classification and Plan 22's consumer reports are the gate inputs.
- Plan 77 owns the pipeline stages; Plan 20's manifest is the CI registry.

## 3. Packages
- **CAF-274A** invocation table + uninvoked report.
- **CAF-274B** ladder/rung semantics doc + tests.
- **CAF-274C** exemption enumeration test.
- **CAF-274D** CI manifest entry for newly wired checks.

## 4. Acceptance & verification
- Every gate is invoked or reported; exemptions are listed; wired checks appear in the manifest.
- `bash scripts/run_test.sh` on the content-tooling region.

## 5. Risks
Coverage theater → invocation truth is the first deliverable.
Exemption creep → enumeration with reasons.

---

## 6. Expanded census (13 files in scope · 4,714 lines · 0 still unmentioned)

Scope: files under `Assets/Ashfall.Core/Content/`. The plan's premise was the family **at authoring time**; later
plans have since referenced some files, so the table marks each row
`still unmentioned` or `since-mentioned`. Both are in scope for this plan: the
since-mentioned files are re-verified for owner discipline, and the still-
unmentioned ones are the original audit target. Class distribution:
Support 12 · System 1.

| File | Lines | Class | Banned | Empty catches | Capture/Restore | Status |
|---|---:|---|---:|---:|---:|---|
| `CatalogDefinitionCounter.cs` | 89 | Support | 0 | 0 | 0 | since-mentioned |
| `CollectibleCatalogIntegrityValidator.cs` | 433 | Support | 0 | 0 | 0 | since-mentioned |
| `ContentAcceptanceLadder.cs` | 102 | Support | 0 | 0 | 0 | since-mentioned |
| `ContentAcceptancePipeline.cs` | 150 | Support | 0 | 0 | 0 | since-mentioned |
| `ContentAcceptanceRung.cs` | 40 | Support | 0 | 0 | 0 | since-mentioned |
| `ContentDeepChainGate.cs` | 445 | Support | 0 | 0 | 0 | since-mentioned |
| `ContentExemption.cs` | 150 | Support | 0 | 0 | 0 | since-mentioned |
| `ContentOrphanCertificationEngine.cs` | 136 | System | 0 | 0 | 0 | since-mentioned |
| `ContentUtilizationGate.cs` | 247 | Support | 0 | 0 | 0 | since-mentioned |
| `ContentUtilizationGraph.cs` | 399 | Support | 0 | 0 | 0 | since-mentioned |
| `ContentUtilizationInstrumentation.cs` | 223 | Support | 0 | 0 | 0 | since-mentioned |
| `ContentUtilizationManifest.cs` | 227 | Support | 0 | 0 | 0 | since-mentioned |
| `ContentUtilizationScanner.cs` | 2073 | Support | 0 | 0 | 0 | since-mentioned |

**Census totals:** 0 banned nondeterministic references · 0 empty-catch sites · 0 files with capture/restore methods.

## 7. Expanded data & state surface

No catalog in this family's name space; the family is code/support, so no data binding is implied.

**State surfaces (capture/restore present):**

None — no save work is implied by this family.

## 8. Expanded verification

| Check | Baseline to establish at claim time |
|---|---|
| Focused region | `Ashfall.Core.Tests/Content/` |
| Files referenced by tests | 24 name references across the test tree |
| Determinism scan | 0 banned references to fix or justify |
| Failure scan | 0 empty-catch sites to route through Plan 35's rules |
| Drift | 13 of 13 files became plan-referenced since authoring — re-verify their owners |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census first (section 6) — classify every file in scope; no edits.
2. Still-unmentioned files: the original audit target — consumer or ownerless verdict.
3. Since-mentioned files: confirm the new plan's claim actually owns them; no double ownership.
4. Catalogs and loaders: justify or report inert.
5. Systems and saves: one owner per state; keys per Plan 1 Appendix Q.
6. Regression: focused region plus this census regenerated.

## 10. Acceptance matrix

| File class | Acceptance |
|---|---|
| Catalog | resolves through a loader; malformed fixture fails typed with the field named |
| Loader | valid/invalid fixture pair; unknown id names the field |
| DTO/Type | round-trip or consume-only proof; no orphan type |
| Save | capture/restore round-trip; key per Plan 1 Appendix Q |
| System | one owner per state; no parallel store |
| Demo | resolves to an existing verb or is retired |
| Support | consumed by a system or reported ownerless |

**Non-goals unchanged:** this expansion adds census and verification detail; it
does not widen the plan's scope or create new authorities.

---

## 12. Cross-plan coupling

This is a family-survey plan; the domain set is the plan's own `.cs` enumeration
(13 files). Other plans referencing those names: **8**.

**Incoming plan edges (top 8):**

| Plan | Family-file mentions |
|---|---:|
| `PLAN-CONTENT-PIPELINE-QA-77` | 12 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `PLAN-UNBLOCK-03` | 1 |
| `PLAN-LAUNCH-FACE-06` | 1 |
| `EVIDENCE` | 1 |
| `PLAN-CORE-ONLY-REGISTRY-11` | 1 |
| `PLAN-ARCHITECTURE-BOUNDARY-31` | 1 |
| `PLAN-MORTUARY-MEMORIAL-TRUTH-123` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `CAF-274A` | no name match — resolve at claim time |
| `CAF-274B` | `ContentAcceptanceLadder.cs` |
| `CAF-274C` | `ContentExemption.cs` |
| `CAF-274D` | `ContentUtilizationManifest.cs` |

**Reading:** incoming edges are coordination risk; candidate files are a starting point for the touch map, not a decision.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 13; intra-domain edges: **11**; isolated files:
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
| `ContentUtilizationScanner` | `CatalogDefinitionCounter` |
| `ContentUtilizationScanner` | `ContentUtilizationGraph` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `ContentUtilizationGraph` | 6 |
| `ContentAcceptanceRung` | 3 |
| `CatalogDefinitionCounter` | 1 |
| `ContentAcceptanceLadder` | 1 |
| `CollectibleCatalogIntegrityValidator` | 0 |
| `ContentAcceptancePipeline` | 0 |
| `ContentDeepChainGate` | 0 |
| `ContentExemption` | 0 |
| `ContentOrphanCertificationEngine` | 0 |
| `ContentUtilizationGate` | 0 |

**Class split:** hub 2 · sink 2 · source 7 · isolated 2.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 13. Host files: **4** · Test files: **14** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 4 | `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/ContentUtilizationSelfTest.cs`, `src/Host/HostCli.SelfTests.cs`, `src/UI/FeedbackPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 14 | `Ashfall.Core.Tests/BoneHornRuntimeActivationTests.cs`, `Ashfall.Core.Tests/Content/CatalogDefinitionCounterTests.cs`, `Ashfall.Core.Tests/Content/ContentAcceptanceLadderTests.cs`, `Ashfall.Core.Tests/Content/ContentAcceptancePipelineTests.cs`, `Ashfall.Core.Tests/Content/ContentOrphanCertificationEngineTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **3** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `collectible_discovery` |
| `counter_intelligence` |
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

Host files (`src/`) whose names share a domain token: **13**
(1 of them panels/HUD).

| Host file |
|---|
| `src/Host/AssetCoverageScanner.cs` |
| `src/Host/CollectibleDiscoverySaveStore.cs` |
| `src/Host/CollectibleEffectDispatcher.cs` |
| `src/Host/ContentUtilizationRuntimeCollector.cs` |
| `src/Host/ContentUtilizationSelfTest.cs` |
| `src/Host/CounterIntelligenceHostSession.cs` |
| `src/Host/CounterIntelligenceSaveStore.cs` |
| `src/Host/DeepCoastHostSession.cs` |
| `src/Host/DeepWellHostSession.cs` |
| `src/Host/DeepWellSaveStore.cs` |
| `src/Host/HostCli.SelfTestManifest.cs` |
| `src/Main.DeepWell.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **3**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `collectible_discovery` | no |
| `counter_intelligence` | no |
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
**Surface:** save sections 3 (laddered 0) · RNG streams 1 · host files 13 · catalogs 12 · test regions 0 · flags 7

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-CONTENT-ACCEPTANCE-FAMILY-TRUTH-274
wave: 19
status: PROPOSED — foreman claim required
packages: CAF-274A, CAF-274B, CAF-274C, CAF-274D
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
