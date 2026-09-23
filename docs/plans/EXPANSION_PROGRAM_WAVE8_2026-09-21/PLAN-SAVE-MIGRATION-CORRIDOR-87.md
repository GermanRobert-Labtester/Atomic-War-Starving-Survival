# PLAN-SAVE-MIGRATION-CORRIDOR-87 — Explicit Section Version Ladders & Migration Fixtures

**Wave 8 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SAVE-GOVERNANCE-12, PLAN-DETERMINISM-REPLAY-13.
**Implementation scaffold:** [`PLAN-SAVE-MIGRATION-CORRIDOR-87_APPENDIX-A_SCAFFOLD.md`](PLAN-SAVE-MIGRATION-CORRIDOR-87_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-SAVE-GOVERNANCE-12` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no format rewrite, no new store, no breaking existing saves; no
section gains a version it does not need.

## 1. Outcome
`Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` (599 lines) declares **204
sections** in `All`, **204** in `SectionFileNames` (the envelope whitelist), but
only **5** explicit ladders in `SchemaVersions` (`holdfast` 5, `year_of_ash` 4,
`dose_ledger` 2, `expansion_hub` 4, `weight_of_choices` 2). The remaining 199
sections carry unversioned `{ State, Checksum }` payloads by convention, not by
declaration. `SchemaVersionedEnvelope`, `CampaignEnvelopeBuilder`,
`SaveEnvelopeHelper` (checksum compute/verify), and `SaveStore` own the
envelope; existing fuzz tests cover the envelope, not per-section ladders.

| Deliverable | Detail |
|---|---|
| Version ladder table | the 199 implicit-v1 sections declared explicitly at 1 (or corrected where a codec already versions privately) |
| Migration fixtures | one minimal fixture per versioned section: vN file → load → vN+1 shape; round-trip checksum stable |
| Forward/backward matrix | newer-envelope-on-older-build and older-envelope-on-newer-build behavior documented and tested |
| Dual-read window | the retired-key/renamed-key read path (aliases in `LifecycleSectionAliases`) has an expiry policy |
| Ledger | one row per section: key, file, ladder, fixtures, retirement date (nullable) |

## 2. Evidence
- `SaveSectionRegistry.cs`: `All` (204), `SectionFileNames` (204), `SchemaVersions` (5), `LifecycleSectionAliases`, `CanonicalizeSectionKey`, `SectionKeysForLifecycleGroup`, `FileNameFor`.
- `Save/SchemaVersionedEnvelope.cs`, `Save/CampaignEnvelopeBuilder.cs`, `Save/SaveEnvelopeHelper.cs`, `Save/SaveStore.cs`; `SaveChecksum.Compute` verified in helper and store.
- Fuzz coverage exists at envelope level: `Ashfall.Core.Tests/Save/CampaignEnvelopeFuzzTests.cs`, `…/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`.
- `SessionDurabilityManager.cs` is host-unreachable (Plan 1 Appendix A); it cannot be the migration authority.

## 3. Packages
- **SMG-87A** ladder census: emit the 204-row table; classify implicit-v1 vs codec-versioned vs explicit.
- **SMG-87B** declared ladders: set implicit sections to 1 in `SchemaVersions` (or document the codec-owned exception per key).
- **SMG-87C** fixtures: per-versioned-section minimal old→new fixture + round-trip test.
- **SMG-87D** matrix: forward/backward scenario tests on the envelope; expected behavior table in docs.
- **SMG-87E** ledger doc: `docs/save/SECTION_MIGRATION_LEDGER.md` generated from the registry (owner-run generator, `--check` mode).

## 4. Acceptance & verification
- Every registry key appears in the ladder table with an owner; count = 204.
- A v1 payload loads under the current build; a bumped section's old fixture migrates; checksum stable across save→load→save.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Save/`; ledger `--check` green.

## 5. Risks
Over-versioning churn → only sections with a real codec change get a ladder
bump; fixtures are minimal. Silent coercion → checksum mismatch is a typed
failure, never a swallowed catch.

---

## 6. Expanded census (5 files · 1,277 lines)

Scope: `Assets/Ashfall.Core/Save/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 5

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CampaignEnvelopeBuilder.cs` | 116 | Support | — | 0 | 0 | 0 |
| `CampaignSaveEnvelope.cs` | 180 | Support | — | 0 | 0 | 2 |
| `SaveEnvelopeHelper.cs` | 320 | Support | — | 0 | 0 | 0 |
| `SaveSectionRegistry.cs` | 600 | Support | **yes** | 0 | 0 | 0 |
| `SchemaVersionedEnvelope.cs` | 61 | Support | **yes** | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `mod_manifest_schema.json` | object[9 keys] |

**State surfaces:** `CampaignSaveEnvelope.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Save/` |
| Test references | 49 name references across the test tree |
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

Domain files: 6. Other plans referencing their names: **12**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-CAMPAIGN-PORTABILITY-104` | 6 |
| `PLAN-SAVE-PREVIEW-METADATA-114` | 6 |
| `PLAN-SAVE-INTEGRITY-FUZZ-OPERATIONS-98` | 4 |
| `PLAN-SAVE-SLOT-UX-105` | 3 |
| `PLAN-VERTICAL-CULTURE-04` | 1 |
| `EVIDENCE` | 1 |
| `PLAN-SAVE-GOVERNANCE-12` | 1 |
| `PLAN-THREADING-ASYNCHRONY-72` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `SMG-87A` | `SchemaVersionedEnvelope.cs` |
| `SMG-87B` | no name match — resolve at claim time |
| `SMG-87C` | `SchemaVersionedEnvelope.cs` |
| `SMG-87D` | `CampaignEnvelopeBuilder.cs`, `CampaignSaveEnvelope.cs`, `SaveEnvelopeHelper.cs` |
| `SMG-87E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 6; intra-domain edges: **1**; isolated files:
**4**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `CampaignEnvelopeBuilder` | `SaveSectionRegistry` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `SaveSectionRegistry` | 1 |
| `CampaignEnvelopeBuilder` | 0 |
| `CampaignSaveEnvelope` | 0 |
| `SaveEnvelopeHelper` | 0 |
| `SchemaVersionedEnvelope` | 0 |
| `SessionDurabilityManager` | 0 |

**Class split:** hub 0 · sink 1 · source 1 · isolated 4.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 6. Host files: **121** · Test files: **43** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 121 | `src/Host/AirlockSecuritySaveStore.cs`, `src/Host/AmphibiousDraisineSaveStore.cs`, `src/Host/AmputationSaveStore.cs`, `src/Host/AnomalyHazardSaveStore.cs`, `src/Host/ApprenticeshipSaveStore.cs` |
| Tests (`Ashfall.Core.Tests/`) | 43 | `Ashfall.Core.Tests/CollectibleDiscoveryPersistenceTests.cs`, `Ashfall.Core.Tests/CollectibleDiscoveryStateTests.cs`, `Ashfall.Core.Tests/Difficulty/DifficultyFullBindingTests.cs`, `Ashfall.Core.Tests/Economy/Plan211BlackMarketHostWiringTests.cs`, `Ashfall.Core.Tests/Economy/Plan212EconomyHostWiringTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/store_capability_claims.json` |

**Verdict:** all three layers attach — surface is bound end to end.

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

Matching flags in `HostCliRegistry.cs`: **3** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--campaign-journey-selftest` |
| `--propaganda-campaign-selftest` |
| `--real-campaign-journey-selftest` |

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

Catalog JSON files whose names share a domain token: **6**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/campaign_epilogues.json` |
| `Assets/StreamingAssets/Data/mod_manifest_schema.json` |
| `Assets/StreamingAssets/Data/narrative/education_session_records.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes_batch_3.json` |

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

Host files (`src/`) whose names share a domain token: **136**
(1 of them panels/HUD).

| Host file |
|---|
| `src/Disease/DiseaseHostSession.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AirlockSecurityHostSession.cs` |
| `src/Host/AmphibiousDraisineHostSession.cs` |
| `src/Host/ApprenticeshipHostSession.cs` |
| `src/Host/ArchiveDeskHostSession.cs` |
| `src/Host/AutopsyHostSession.cs` |
| `src/Host/BallisticShieldHostSession.cs` |
| `src/Host/BioFermentationHostSession.cs` |
| `src/Host/BlackMarketHostSession.cs` |
| `src/Host/CampaignDayPersistenceAdapter.cs` |

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

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `wildlife_migration` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **4**
(CODEX_ONLY 4).

| Catalog | Classification |
|---|---|
| `narrative/education_session_records.json` | CODEX_ONLY |
| `narrative/therapist_session_notes.json` | CODEX_ONLY |
| `narrative/therapist_session_notes_batch_2.json` | CODEX_ONLY |
| `narrative/therapist_session_notes_batch_3.json` | CODEX_ONLY |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 12
**Surface:** save sections 2 (laddered 0) · RNG streams 1 · host files 13 · catalogs 10 · test regions 1 · flags 3

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-SAVE-MIGRATION-CORRIDOR-87
wave: 8
status: PROPOSED — foreman claim required
packages: SMG-87A, SMG-87B, SMG-87C, SMG-87D, SMG-87E
claim paths:
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - src/Foundry/SilentFoundryHostSession.cs  # §19 candidate host surface
  - src/Host/AgricultureHostSession.cs  # §19 candidate host surface
  - src/Host/AirlockSecurityHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/campaign_epilogues.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/mod_manifest_schema.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/
  - godot --headless --path . -- --campaign-journey-selftest
dependencies:
  - coordinate: 12 other plan(s) name these artifacts (§12)
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
