# PLAN-RUNTIME-PERF-16 — Composition Cost, Day-Tick Budget & Soak Discipline

**Program:** ASHFALL Expansion & Integration Program — Wave 2 (2026-09-21)
**Status:** PROPOSED — not a claim. Foreman claim required.
**Owner role on execution:** Integrator (instrumentation + budget gates) with
Builders per measured hotspot.
**Depends on:** PLAN-INTEGRATION-KIT-02 (manifest enumeration enables
per-subsystem instrumentation); PLAN-ORPHAN-SEAL-01 (wiring doubles the
composition and tick surface).
**Non-goals:** no speculative micro-optimization, no frame-rate regression, no
engine changes, no new threading model.

---

## 1. Outcome

The audit baseline is good — a 30-day advance costs a median 1.29 s
(p95 2.72 s) with ~80 KB per run — but the programme is about to add ~99
authorities, ~220 manifest entries (target), and 5–7 new day owners. This plan
keeps performance a measured budget rather than a post-programme emergency:
composition cost, per-owner tick cost, allocation trend, soak behavior, and a
CI regression gate.

Deliverables:

1. a **composition profile**: cost of every `Setup*` in `ComposeCampaign()`,
   with an eager/lazy policy;
2. a **day-tick budget per owner** (owner timing in the existing day-advance
   benchmark report);
3. **measured debloat** for the top allocation sources found;
4. a **soak protocol** (7-day and 30-day seeded runs) tracking memory, save
   size, log size, and UI object count;
5. **budget gates** in the fast tier that fail on meaningful regression;
6. a written **lowest practical hardware target** and a headless proxy for it.

---

## 2. Premise evidence (2026-09-21)

| Fact | Value | Source |
|---|---:|---|
| Day-advance 30d benchmark | median 1.287 s, mean 1.623 s, p95 2.723 s, max 3.075 s | `artifacts/runtime-scale-results.json` |
| Allocations | total 399,056 B / 5 runs; median 79,360 B/run | same |
| Host `Setup*` methods | 226 | grep audit |
| Host files | 780 | `ls src/Host` |
| Composition root | `ComposeCampaign()` with 40+ direct calls + manifest bootstrap | `src/Main.CampaignServices.cs` |
| Manifest entries | 18 (target ~120–180) | `SubsystemManifest.cs` |
| Headless tick policy | 15 FPS for runtime sessions | `AGENTS.md` |
| Perf gate present | "Runtime Scale Performance Budget Self-Test (Task 130)" | `CI_GATE_MANIFEST.json` |
| Perf artifacts | `runtime-scale-results.json`, `balance/`, playtest reports | `artifacts/` |
| Skills available | `ashfall-tune`, `ashfall-performance-analyzer`, `ashfall-debloater-forensic` | `.agents/skills` |

---

## 3. Authority map

| Concern | Extend (do not create) |
|---|---|
| Benchmark harness | `RuntimeScaleBench` / `--runtime-scale-selftest` path (existing artifact producer) |
| Day owners | `src/Main.CampaignOwners.cs` ordering; add timing wrappers |
| Composition | `ComposeCampaign()` + `ExecuteSubsystemManifestBootstrap()` |
| Logs/retention | `RollingLog<T>` (Plan 55) |
| Save growth | `SaveSectionRegistry` size budget (PLAN-SAVE-GOVERNANCE-12) |
| Memory evidence | existing soak/smoke selftests (`--7-day-smoke-selftest`) |

---

## 4. Packages

### PF-16A — Composition profile
- Instrument `ComposeCampaign()` to record per-`Setup*` elapsed time and
  allocation (Stopwatch + `GC.GetAllocatedBytesForCurrentThread`) into a report
  artifact. Classify each as `eager` (needed before first frame) or `lazy`
  (first open).
- **Acceptance:** report lists all 226 setups with a budget; lazy candidates
  identified with a measured saving; startup budget written (e.g. ≤ X ms
  headless); no setup order change without a dependency note.
- **Verify:** `godot --headless --path . -- --composition-profile-selftest`
  (new) + `bash scripts/run_test.sh Ashfall.Core.Tests/Orchestration/`.

### PF-16B — Day-tick budget per owner
- Wrap each day owner in the benchmark with a timing scope and emit
  `owner | ms | alloc` rows. Set per-owner budgets from the baseline; flag any
  owner over budget in the report.
- **Acceptance:** report enumerates every owner; the sum matches the day
  advance time within tolerance; a newly wired owner cannot land without a
  measured row in the report.
- **Verify:** `godot --headless --path . -- --runtime-scale-selftest` (extended)
  — compare against `artifacts/runtime-scale-results.json`.

### PF-16C — Measured debloat
- From the profile, fix the top sources: per-tick LINQ/`ToList`, per-tick
  string interpolation, per-tick catalog lookups (build dictionaries once at
  load), boxing in loops, and per-tick `new List<>` without capacity.
- Rules: only fix what the profile shows; each fix carries before/after numbers
  in the handoff; no behavior change; determinism preserved (no reordering of
  state mutations).
- **Acceptance:** ≥ 25% reduction on the flagged top-3 sources, or a written
  reason the cost is inherent.
- **Verify:** the benchmark before/after; focused tests for each touched system.

### PF-16D — Soak protocol
- Define and run two bounded soaks: 7-day (fast tier) and 30-day (release tier,
  explicit window per TEST_POLICY). Track: process RSS, GC heap, UI object
  count (tree node count), save file size, journal/log size, and pending
  timers/subscriptions. Leak criterion: monotonic growth across days after
  warm-up beyond the declared slack.
- **Acceptance:** both soaks complete under budget with no monotonic leak;
  report artifact committed; the 100-orphan shutdown signature stays absent.
- **Verify:** `godot --headless --path . -- --7-day-smoke-selftest`;
  `godot --headless --path . -- --campaign-soak-selftest --days=30`
  (existing or extended).

### PF-16E — CI budget gates
- Add fast-tier gates: (a) day-advance median ≤ baseline × 1.25; (b) startup
  composition ≤ budget; (c) soak slack respected. Gates compare against the
  committed artifact and update only with an explicit `--rebaseline` flag plus
  a closeout note.
- **Acceptance:** a deliberate regression fails the gate with the offending
  owner named; no flake across three consecutive runs (median-based).
- **Verify:** `python3 scripts/ci/agent-fast-verify.py`.

### PF-16F — Lowest practical hardware target
- Write `docs/performance/LOW_SPEC_TARGET.md`: CPU/RAM/GPU class, 1920×1080
  fixed viewport, target frame pacing outside headless, and the headless proxy
  workloads that approximate it. Verify on the lowest available runner
  (bounded, opt-in release-tier run).
- **Acceptance:** the proxy workload passes on the target class with the
  documented frame-time margin; the doc is linked from `AGENTS.md` by the
  foreman.

---

## 5. Risk register

| Risk | Mitigation |
|---|---|
| Instrumentation itself costs time | compile-time flag; disabled in release; measured before adoption |
| Budget gates flake on shared CI | median-of-5, 25% margin, artifact-based comparison |
| Lazy setup changes visible order | lazy only for first-open surfaces; gameplay-critical authorities stay eager |
| Soak becomes a full-suite run | bounded days and focused probes; release-tier only for 30-day |
| Debloat regressions | before/after numbers + focused tests + determinism replay |

## 6. Verification summary

```bash
godot --headless --path . -- --composition-profile-selftest
godot --headless --path . -- --runtime-scale-selftest
godot --headless --path . -- --7-day-smoke-selftest
godot --headless --path . -- --campaign-soak-selftest --days=30
python3 scripts/ci/agent-fast-verify.py
```

## 7. Change control

No optimization lands without a measured before/after and a determinism replay
for the touched system. Budget rebaselines require a closeout note and a
`KNOWN_DEBT` row if accepted as debt.

---

## 6. Expanded census (8 files · 908 lines)

Scope: `Assets/Ashfall.Core/Performance/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 8

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `PerfResult.cs` | 57 | Support | — | 0 | 0 | 0 |
| `PerfSample.cs` | 44 | Support | — | 0 | 0 | 0 |
| `PerfSession.cs` | 165 | Support | — | 0 | 0 | 0 |
| `PerfStatistics.cs` | 123 | Support | — | 0 | 0 | 0 |
| `PerfStopwatch.cs` | 58 | Support | — | 0 | 0 | 0 |
| `PerfTestMarker.cs` | 10 | Support | — | 0 | 0 | 0 |
| `PerfWorkloadContext.cs` | 47 | Support | — | 0 | 0 | 0 |
| `PerformanceCampaignHarness.cs` | 404 | Support | — | 0 | 0 | 3 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `needs_performance.json` | object[4 keys] |

**State surfaces:** `PerformanceCampaignHarness.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Performance/` |
| Test references | 17 name references across the test tree |
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

The domain set is the plan's own `.cs` enumeration (9 files).
Other plans referencing those names: **9**.

**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-PERF-HARNESS-FAMILY-TRUTH-279` | 8 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `PLAN-INTEGRATION-KIT-02` | 1 |
| `EVIDENCE` | 1 |
| `PLAN-SELFTEST-TRUTH-23` | 1 |
| `PLAN-LIFECYCLE-SEALING-32` | 1 |
| `PLAN-RUNTIME-RESILIENCE-57` | 1 |
| `PLAN-MORTUARY-MEMORIAL-TRUTH-123` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `PF-16A` | no name match — resolve at claim time |
| `PF-16B` | no name match — resolve at claim time |
| `PF-16C` | no name match — resolve at claim time |
| `PF-16D` | no name match — resolve at claim time |
| `PF-16E` | no name match — resolve at claim time |
| `PF-16F` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidate files are a starting point for the touch map, not a decision.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 9; intra-domain edges: **10**; isolated files:
**2**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `PerfResult` | `PerfSample` |
| `PerfResult` | `PerfStatistics` |
| `PerfResult` | `PerfWorkloadContext` |
| `PerfSession` | `PerfResult` |
| `PerfSession` | `PerfSample` |
| `PerfSession` | `PerfStatistics` |
| `PerfSession` | `PerfStopwatch` |
| `PerfSession` | `PerfWorkloadContext` |
| `PerformanceCampaignHarness` | `PerfStopwatch` |
| `PerformanceCampaignHarness` | `PerfWorkloadContext` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `PerfWorkloadContext` | 3 |
| `PerfSample` | 2 |
| `PerfStatistics` | 2 |
| `PerfStopwatch` | 2 |
| `PerfResult` | 1 |
| `PerfSession` | 0 |
| `PerfTestMarker` | 0 |
| `PerformanceCampaignHarness` | 0 |
| `SubsystemManifest` | 0 |

**Class split:** hub 1 · sink 4 · source 2 · isolated 2.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 9. Host files: **2** · Test files: **8** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/Host/PerformanceSelfTest.cs`, `src/Main.Lifecycle.cs` |
| Tests (`Ashfall.Core.Tests/`) | 8 | `Ashfall.Core.Tests/Orchestration/SubsystemManifestTests.cs`, `Ashfall.Core.Tests/Performance/PerformanceCampaignWorkloadTests.cs`, `Ashfall.Core.Tests/Performance/PerformanceDayAdvanceTests.cs`, `Ashfall.Core.Tests/Performance/PerformanceFrameworkTests.cs`, `Ashfall.Core.Tests/Performance/PerformanceLifecycleTests.cs` |
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
| `--selftest-manifest` |
| `--test-manifest` |

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

Catalog JSON files whose names share a domain token: **9**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/campaign_epilogues.json` |
| `Assets/StreamingAssets/Data/mod_manifest_schema.json` |
| `Assets/StreamingAssets/Data/narrative/education_session_records.json` |
| `Assets/StreamingAssets/Data/narrative/leather_harness_conditioning_audits.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes_batch_3.json` |
| `Assets/StreamingAssets/Data/narrative_discovery_manifest.json` |
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

Host files (`src/`) whose names share a domain token: **140**
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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 9
**Surface:** save sections 2 (laddered 0) · RNG streams 0 · host files 12 · catalogs 14 · test regions 2 · flags 7

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-RUNTIME-PERF-16
wave: —
status: PROPOSED — foreman claim required
packages: PF-16A, PF-16B, PF-16C, PF-16D, PF-16E, PF-16F
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
