# PLAN-SAVE-INTEGRITY-FUZZ-OPERATIONS-98 — Continuous Corruption Corpus & Recovery Paths

**Wave 8 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SAVE-GOVERNANCE-12, PLAN-SAVE-MIGRATION-CORRIDOR-87, PLAN-AUTOMATED-QA-CAMPAIGNS-74.
**Implementation scaffold:** [`PLAN-SAVE-INTEGRITY-FUZZ-OPERATIONS-98_APPENDIX-A_SCAFFOLD.md`](PLAN-SAVE-INTEGRITY-FUZZ-OPERATIONS-98_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-SAVE-MIGRATION-CORRIDOR-87` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no new save format, no always-on fuzzing in the default test
run, no unbounded corpus in git.

## 1. Outcome
Fuzz and corruption coverage already exists in focused files:
`Ashfall.Core.Tests/Save/CampaignEnvelopeFuzzTests.cs`,
`…/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`,
`…/World/WastelandMapFragmentPersistenceFuzzTests.cs`,
`…/DoseCollectibleSaveFuzzTests.cs`, on top of the checksummed envelope
(`SaveEnvelopeHelper` verifies on read; `SaveStore` owns slots). What is
missing is **operations**: a coverage matrix over the 204 registry sections,
a preserved mutation corpus with reproducible recipes, explicit recovery
behavior when verification fails, and bounded scheduling in the QA rotation.

| Deliverable | Detail |
|---|---|
| Coverage matrix | per section codec: has fuzz / has corruption case / has migration fixture (links Plan 87) / none |
| Corpus format | mutated artifacts stored under a bounded fixtures dir with a recipe (offset/field + expected failure class) |
| Recovery paths | per store: checksum fail → typed error and defined fallback (refuse, load-other-slot, quarantine) — never a half-loaded world |
| Scheduling | bounded fuzz slice per rotation run in Plan 74; new/changed codecs run alone first (TEST_POLICY) |
| Triage dump | first divergent byte/field + expected vs actual class, so a failure names its owner |

## 2. Evidence
- Existing fuzz set listed above (4 files); envelope checksum verified at `Save/SaveEnvelopeHelper.cs` and `Save/SaveStore.cs`.
- `SaveSectionRegistry`: 204 sections (Plan 87 Appendix goal), giving the matrix its rows.
- `TEST_POLICY.md`: new test files run alone first; focused runs bounded — operations must comply.
- Plan 74 owns the rotation; this plan supplies the fuzz slice and corpus.

## 3. Packages
- **SIF-98A** coverage matrix generator over registry + codecs; `--check` mode.
- **SIF-98B** corpus + recipes: bounded mutations for the highest-risk codecs (envelope, dose, map fragments, inventory).
- **SIF-98C** recovery behavior per store + focused tests; a failed load never yields partial state.
- **SIF-98D** rotation integration note (slice size, time budget) for Plan 74.
- **SIF-98E** triage dump format + one worked example.

## 4. Acceptance & verification
- Matrix lists every section with a coverage verdict; unknown codecs are listed as gaps, not guessed.
- Corpus replay is deterministic: same recipe → same failure class.
- Recovery tests: corrupt envelope refused with typed error; fallback path loads the alternate slot when configured.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Save/` (focused; fuzz slice bounded).

## 5. Risks
Corpus bloat → bounded fixture count and small artifacts; recipes, not dumps,
are the durable form. Fuzz flakiness → seeded mutations only.

---

## 6. Expanded census (5 files · 996 lines)

Scope: `Assets/Ashfall.Core/Save/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 5

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CampaignEnvelopeBuilder.cs` | 116 | Support | — | 0 | 0 | 0 |
| `CampaignSaveEnvelope.cs` | 180 | Support | — | 0 | 0 | 2 |
| `SaveEnvelopeHelper.cs` | 320 | Support | **yes** | 0 | 0 | 0 |
| `SaveStore.cs` | 319 | Support | **yes** | 0 | 0 | 10 |
| `SchemaVersionedEnvelope.cs` | 61 | Support | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 2 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** `CampaignSaveEnvelope.cs`, `SaveStore.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Save/` |
| Test references | 53 name references across the test tree |
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

Domain files: 4. Other plans referencing their names: **9**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-SAVE-MIGRATION-CORRIDOR-87` | 4 |
| `PLAN-CAMPAIGN-PORTABILITY-104` | 4 |
| `PLAN-SAVE-PREVIEW-METADATA-114` | 4 |
| `PLAN-VERTICAL-CULTURE-04` | 1 |
| `PLAN-SAVE-GOVERNANCE-12` | 1 |
| `PLAN-THREADING-ASYNCHRONY-72` | 1 |
| `PLAN-DETERMINISM-CROSS-HOST-89` | 1 |
| `PLAN-SAVE-SLOT-UX-105` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `SIF-98A` | no name match — resolve at claim time |
| `SIF-98B` | `CampaignEnvelopeBuilder.cs`, `CampaignSaveEnvelope.cs`, `SaveEnvelopeHelper.cs` |
| `SIF-98C` | no name match — resolve at claim time |
| `SIF-98D` | no name match — resolve at claim time |
| `SIF-98E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 5. Host files: **209** · Test files: **32** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 209 | `src/Host/AgricultureSaveStore.cs`, `src/Host/AirlockSecuritySaveStore.cs`, `src/Host/AmphibiousDraisineSaveStore.cs`, `src/Host/AmputationSaveStore.cs`, `src/Host/AnomalyHazardSaveStore.cs` |
| Tests (`Ashfall.Core.Tests/`) | 32 | `Ashfall.Core.Tests/BareSaveStoreSealTests.cs`, `Ashfall.Core.Tests/CollectibleDiscoveryPersistenceTests.cs`, `Ashfall.Core.Tests/CollectibleDiscoveryStateTests.cs`, `Ashfall.Core.Tests/Difficulty/DifficultyFullBindingTests.cs`, `Ashfall.Core.Tests/DutyRoster/DutyRosterSaveStoreTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 5 files; intra-domain edges: **3**; isolated: **1**.

| From | → To |
|---|---|
| `CampaignEnvelopeBuilder` | `SaveStore` |
| `SaveStore` | `SaveEnvelopeHelper` |
| `SchemaVersionedEnvelope` | `SaveStore` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **2** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `campaign` |
| `campaign_day` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **8** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--campaign-journey-selftest` |
| `--data-integrity-selftest` |
| `--operations-selftest` |
| `--propaganda-campaign-selftest` |
| `--real-campaign-journey-selftest` |
| `--save-store-checksum-selftest` |
| `--save-store-checksums-selftest` |
| `--shelter-operations-selftest` |

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

Catalog JSON files whose names share a domain token: **4**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/campaign_epilogues.json` |
| `Assets/StreamingAssets/Data/espionage_operations.json` |
| `Assets/StreamingAssets/Data/mod_manifest_schema.json` |
| `Assets/StreamingAssets/Data/store_capability_claims.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (32 files, 187 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Campaign` | 32 | 187 |

**Verdict:** 187 cases sit under matching regions — run those first (`Campaign`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **195**
(1 of them panels/HUD).

| Host file |
|---|
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Host/AgricultureSaveStore.cs` |
| `src/Host/AirlockSecuritySaveStore.cs` |
| `src/Host/AmphibiousDraisineSaveStore.cs` |
| `src/Host/AmputationSaveStore.cs` |
| `src/Host/AnomalyHazardSaveStore.cs` |
| `src/Host/ApprenticeshipSaveStore.cs` |
| `src/Host/AquaponicsSaveStore.cs` |
| `src/Host/ArchaeologySaveStore.cs` |
| `src/Host/ArmoredCrawlerSaveStore.cs` |
| `src/Host/AutopsySaveStore.cs` |
| `src/Host/AviationSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **2**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `campaign` | no |
| `campaign_day` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **0**.

| Stream |
|---|
| — | no seeded stream shares a token with this domain |

**Verdict:** no seeded stream shares a token — either the domain is deterministic without randomness (fine) or it draws from an unlisted source (check `System.Random`/time seeding before shipping).

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **0**
(none).

| Catalog | Classification |
|---|---|
| — | no catalog filename shares a token with this domain |

**Verdict:** no catalog matches — the domain is code-authoritative or its data lives in a broader catalog.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 9
**Surface:** save sections 2 (laddered 0) · RNG streams 0 · host files 12 · catalogs 4 · test regions 1 · flags 8

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-SAVE-INTEGRITY-FUZZ-OPERATIONS-98
wave: 8
status: PROPOSED — foreman claim required
packages: SIF-98A, SIF-98B, SIF-98C, SIF-98D, SIF-98E
claim paths:
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - src/Host/AgricultureSaveStore.cs  # §19 candidate host surface
  - src/Host/AirlockSecuritySaveStore.cs  # §19 candidate host surface
  - src/Host/AmphibiousDraisineSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/campaign_epilogues.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/espionage_operations.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/
  - godot --headless --path . -- --campaign-journey-selftest
dependencies:
  - coordinate: 9 other plan(s) name these artifacts (§12)
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
