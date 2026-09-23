# PLAN-PERF-HARNESS-FAMILY-TRUTH-279 — Performance Harness: Sessions, Statistics & Workloads

**Wave 19 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-RUNTIME-PERF-16, PLAN-DEV-TOOLING-TRUTH-75, PLAN-AUTOMATED-QA-CAMPAIGNS-74.
**Non-goals:** no performance targets change; the harness is audited for
measurement truth.

## 1. Outcome
**9 `Performance/` files** are referenced by no plan: `PerfSession`,
`PerfStopwatch`, `PerfStatistics`, `PerfResult`, `PerfSample`,
`PerfTestMarker`, `PerfWorkloadContext`, `ScaleTier`. A measurement harness
whose own accuracy is unverified produces false confidence; this plan verifies
it before Plan 16's budgets lean on it.

| Deliverable | Detail |
|---|---|
| Timer truth | `PerfStopwatch` monotonic; no wall-clock or frame-count substitution (fixture) |
| Statistics | percentiles/aggregation documented and unit-tested on known samples |
| Session lifecycle | sessions start/stop cleanly; leaked sessions reported |
| Workload context | contexts tag runs; scale tiers map to Plan 16's budgets |
| Marker hygiene | markers disable in release paths (no runtime cost in player builds) |

## 2. Evidence
- 9 `Performance/` basenames absent from every plan body (Wave 19 file-level audit).
- Plan 16 owns budgets; Plan 74 the campaign rotation that consumes results.
- Plan 75 owns dev-tool surfaces.

## 3. Packages
- **PHF-279A** timer monotonicity fixture.
- **PHF-279B** statistics unit tests (known samples).
- **PHF-279C** session lifecycle/leak tests.
- **PHF-279D** scale-tier mapping check against Plan 16.
- **PHF-279E** marker release-mode audit.

## 4. Acceptance & verification
- Statistics match known samples; no leaked sessions; markers are absent from release paths.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Performance/`.

## 5. Risks
False confidence → unit-tested statistics.
Runtime cost → release-mode marker audit.

---

## 6. Expanded census (10 family files · 1,091 lines)

Scope: files under `Assets/Ashfall.Core/Performance/` whose basename is referenced
by no plan body (the Wave 19 family definition). Class distribution: Support 10.

| File | Lines | Class | Banned refs | Empty catches | Capture/Restore |
|---|---:|---|---:|---:|---:|
| `PerfResult.cs` | 57 | Support | 0 | 0 | 0 |
| `PerfSample.cs` | 44 | Support | 0 | 0 | 0 |
| `PerfSession.cs` | 165 | Support | 0 | 0 | 0 |
| `PerfStatistics.cs` | 123 | Support | 0 | 0 | 0 |
| `PerfStopwatch.cs` | 58 | Support | 0 | 0 | 0 |
| `PerfTestMarker.cs` | 10 | Support | 0 | 0 | 0 |
| `PerfWorkloadContext.cs` | 47 | Support | 0 | 0 | 0 |
| `ScaleTier.cs` | 100 | Support | 0 | 0 | 0 |
| `WorkloadProfile.cs` | 83 | Support | 0 | 0 | 0 |
| `PerformanceCampaignHarness.cs` | 404 | Support | 0 | 0 | 3 |

**Census totals:** 0 banned nondeterministic references · 0 empty-catch sites · 1 files with capture/restore methods.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `needs_performance.json` | object[4 keys] |

**State surfaces (capture/restore present):**

- `PerformanceCampaignHarness.cs`

## 8. Expanded verification

| Check | Baseline to establish at claim time |
|---|---|
| Focused region | `Ashfall.Core.Tests/Performance/` |
| Family files referenced by tests | 24 name references across the test tree |
| Determinism scan | 0 banned references to fix or justify |
| Failure scan | 0 empty-catch sites to route through Plan 35's rules |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census first (section 6) — classify every family file; no edits in this step.
2. Catalogs and loaders: prove a consumer or report the file as inert.
3. DTO/Type files: round-trip or consume-only proof; unknown values fail typed.
4. System files: confirm the single owner per state; remove duplicated stores.
5. Demo/tooling files: resolve to a real CLI verb or retire (Plan 86 pattern).
6. Regression: focused region plus this family census regenerated.

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
(10 files). Other plans referencing those names: **1**.

**Incoming plan edges (top 8):**

| Plan | Family-file mentions |
|---|---:|
| `PLAN-RUNTIME-PERF-16` | 8 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `PHF-279A` | no name match — resolve at claim time |
| `PHF-279B` | `PerfStatistics.cs` |
| `PHF-279C` | `PerfSession.cs` |
| `PHF-279D` | `ScaleTier.cs` |
| `PHF-279E` | `PerfTestMarker.cs` |

**Reading:** incoming edges are coordination risk; candidate files are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 10. Host files: **1** · Test files: **6** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 1 | `src/Host/PerformanceSelfTest.cs` |
| Tests (`Ashfall.Core.Tests/`) | 6 | `Ashfall.Core.Tests/Performance/PerformanceCampaignWorkloadTests.cs`, `Ashfall.Core.Tests/Performance/PerformanceDayAdvanceTests.cs`, `Ashfall.Core.Tests/Performance/PerformanceFrameworkTests.cs`, `Ashfall.Core.Tests/Performance/PerformanceLifecycleTests.cs`, `Ashfall.Core.Tests/Performance/PerformanceMemoryTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

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

Matching flags in `HostCliRegistry.cs`: **7** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--campaign-journey-selftest` |
| `--perf-selftest` |
| `--performance-selftest` |
| `--propaganda-campaign-selftest` |
| `--real-campaign-journey-selftest` |
| `--runtime-scale` |
| `--runtime-scale-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **2**.

| Event | First declaration |
|---|---|
| `OnCraftResultOverflow` | `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` |
| `OnNarrativeMarker` | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **7**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/campaign_epilogues.json` |
| `Assets/StreamingAssets/Data/narrative/education_session_records.json` |
| `Assets/StreamingAssets/Data/narrative/leather_harness_conditioning_audits.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes_batch_3.json` |
| `Assets/StreamingAssets/Data/needs_performance.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **2** (42 files, 234 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Campaign` | 32 | 187 |
| `Performance` | 10 | 47 |

**Verdict:** 234 cases sit under matching regions — run those first (`Campaign`, `Performance`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **139**
(0 of them panels/HUD).

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

Matching seeded streams in `Random/CampaignRngStream.cs`: **0**.

| Stream |
|---|
| — | no seeded stream shares a token with this domain |

**Verdict:** no seeded stream shares a token — either the domain is deterministic without randomness (fine) or it draws from an unlisted source (check `System.Random`/time seeding before shipping).

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **5**
(CODEX_ONLY 5).

| Catalog | Classification |
|---|---|
| `narrative/education_session_records.json` | CODEX_ONLY |
| `narrative/leather_harness_conditioning_audits.json` | CODEX_ONLY |
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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 2 (laddered 0) · RNG streams 0 · host files 12 · catalogs 12 · test regions 2 · flags 7

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-PERF-HARNESS-FAMILY-TRUTH-279
wave: 19
status: PROPOSED — foreman claim required
packages: PHF-279A, PHF-279B, PHF-279C, PHF-279D, PHF-279E
claim paths:
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - src/Foundry/SilentFoundryHostSession.cs  # §19 candidate host surface
  - src/Host/AgricultureHostSession.cs  # §19 candidate host surface
  - src/Host/AirlockSecurityHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/campaign_epilogues.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/education_session_records.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/
  - godot --headless --path . -- --campaign-journey-selftest
dependencies:
  - coordinate: 1 other plan(s) name these artifacts (§12)
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
