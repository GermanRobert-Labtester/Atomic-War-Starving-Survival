# PLAN-TELEMETRY-PRIVACY-58 — Local Metrics Governance, Consent & Retention

**Wave 6 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SILENT-FAILURE-35, PLAN-SAVE-GOVERNANCE-12.
**Non-goals:** no network telemetry, no analytics SDK, no PII, no account IDs.

## Outcome
`PlaySessionRecorder` + `PlayableMetricsAggregationEngine` already implement
local-only JSONL metrics with zero network and zero PII (Plan 46). What is
missing is **governance**: an explicit field catalogue, retention/deletion
controls, opt-out, and a gate that prevents PII or network calls from being
added later.

| Deliverable | Detail |
|---|---|
| Field catalogue | every recorded field with purpose, type, retention, and sensitivity class |
| Consent | first-run notice + settings toggle; recording disabled entirely when off |
| Retention | bounded file rotation; deletion on demand (one button); no hidden copies |
| Gate | static scan: no network APIs, no PII-shaped fields (names, paths, ids beyond session-local counters) in the recorder |
| Reproducibility | metrics are deterministic per seed; no wall-clock values in event payloads |
| Support bundle | diagnostics export (Plan 35) may include a metrics summary only with the same consent |

## Evidence
- `Assets/Ashfall.Core/Telemetry/PlaySessionRecorder.cs`, `PlayableMetricsAggregationEngine.cs` (Plan 46: 7/7, 4/4).
- JSONL under `user://`, bounded buffer (max 2000 events), monotonic session clock, `JoinDay` consequences.
- Funnel: `FirstHourFunnel` 7 steps (Plan 55 consumes it).
- Artifacts: `artifacts/content-utilization*.json`, `balance/`, `runtime-scale-results.json` are build-time, not player telemetry.

## Packages
- **TP-58A** field catalogue: generated from the recorder/aggregator DTOs; every field classified.
- **TP-58B** consent + opt-out: first-run notice; toggle; off means no file created.
- **TP-58C** retention/deletion: bounded rotation (e.g. last N sessions), explicit delete; verified on disk.
- **TP-58D** privacy gate: scan for network/HTTP/socket APIs and PII patterns in telemetry code; fail on match.
- **TP-58E** docs: `docs/telemetry/LOCAL_METRICS_POLICY.md` with the catalogue, retention, and the no-network statement.

## Acceptance & verification
- Toggle-off creates no file; delete removes all session files; gate green; deterministic per seed.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Telemetry/`; privacy gate script; `--diagnostics-bundle` respects consent.

## Risks
Metrics creep → the catalogue is generated; a new field without a catalogue row fails the gate.

---

## 6. Expanded census (2 files · 588 lines)

Scope: `Assets/Ashfall.Core/Telemetry/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `PlaySessionRecorder.cs` | 402 | Support | **yes** | 0 | 0 | 0 |
| `PlayableMetricsAggregationEngine.cs` | 186 | System | **yes** | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `recon_telemetry_probes.json` | object[2 keys] |
| `orbital_kinetic_telemetry.json` | array[8] |

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Telemetry/` |
| Test references | 2 name references across the test tree |
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

Domain method: plan-body artifact list.
Governed artifacts: 7. Other plans referencing them: **11**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-HOST-EVENT-ARCHIVE-91` | 3 |
| `EVIDENCE` | 1 |
| `PLAN-DETERMINISM-REPLAY-13` | 1 |
| `PLAN-RUNTIME-PERF-16` | 1 |
| `PLAN-SIGNALS-REMOTE-SENSING-49` | 1 |
| `PLAN-ELECTRONICS-COMPUTING-65` | 1 |
| `PLAN-THREADING-ASYNCHRONY-72` | 1 |
| `PLAN-BALANCE-DIFFICULTY-INTEGRATION-73` | 1 |

**Governed artifacts (first 12):**

| Artifact |
|---|
| `Assets/Ashfall.Core/Telemetry/PlaySessionRecorder.cs` |
| `PlaySessionRecorder.cs` |
| `PlayableMetricsAggregationEngine.cs` |
| `docs/telemetry/LOCAL_METRICS_POLICY.md` |
| `orbital_kinetic_telemetry.json` |
| `recon_telemetry_probes.json` |
| `runtime-scale-results.json` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `TP-58A` | `Assets/Ashfall.Core/Telemetry/PlaySessionRecorder.cs`, `PlaySessionRecorder.cs` |
| `TP-58B` | no name match — resolve at claim time |
| `TP-58C` | no name match — resolve at claim time |
| `TP-58D` | `orbital_kinetic_telemetry.json`, `recon_telemetry_probes.json` |
| `TP-58E` | `docs/telemetry/LOCAL_METRICS_POLICY.md`, `PlayableMetricsAggregationEngine.cs`, `orbital_kinetic_telemetry.json` |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 6. Host files: **0** · Test files: **2** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 0 | — |
| Tests (`Ashfall.Core.Tests/`) | 2 | `Ashfall.Core.Tests/Telemetry/PlaySessionRecorderTests.cs`, `Ashfall.Core.Tests/Telemetry/PlayableMetricsAggregationEngineTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/store_capability_claims.json` |

**Verdict:** no host attachment found for these symbols — candidate integration gap (confirm under alternate names)

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **4** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `chemical_recon` |
| `kinetic_storage` |
| `recon_telemetry` |
| `unique_claims` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **8** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--advanced-industrial-recon-selftest` |
| `--day1-playable-selftest` |
| `--playable-loop-selftest` |
| `--playable-shell-selftest` |
| `--runtime-scale` |
| `--runtime-scale-selftest` |
| `--save-store-checksum-selftest` |
| `--save-store-checksums-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **4**.

| Event | First declaration |
|---|---|
| `OnPolicyChanged` | `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` |
| `OnTelemetryChanged` | `Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs` |
| `OnTelemetryLost` | `Assets/Ashfall.Core/World/WeatherSondeSystem.cs` |
| `OnTelemetryReceived` | `Assets/Ashfall.Core/World/WeatherSondeSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **6**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/narrative/education_session_records.json` |
| `Assets/StreamingAssets/Data/narrative/orbital_kinetic_telemetry.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes_batch_3.json` |
| `Assets/StreamingAssets/Data/recon_telemetry_probes.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (2 files, 11 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Telemetry` | 2 | 11 |

**Verdict:** 11 cases sit under matching regions — run those first (`Telemetry`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **130**
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
| `src/Host/CaregivingHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **1**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `recon_telemetry` | no |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **6**
(CODEX_ONLY 5, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `narrative/education_session_records.json` | CODEX_ONLY |
| `narrative/orbital_kinetic_telemetry.json` | CODEX_ONLY |
| `narrative/therapist_session_notes.json` | CODEX_ONLY |
| `narrative/therapist_session_notes_batch_2.json` | CODEX_ONLY |
| `narrative/therapist_session_notes_batch_3.json` | CODEX_ONLY |
| `recon_telemetry_probes.json` | UNRESOLVED |

**Verdict:** 1 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 11
**Surface:** save sections 1 (laddered 0) · RNG streams 0 · host files 12 · catalogs 12 · test regions 1 · flags 8

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-TELEMETRY-PRIVACY-58
wave: 6
status: PROPOSED — foreman claim required
packages: TP-58A, TP-58B, TP-58C, TP-58D, TP-58E
claim paths:
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - src/Foundry/SilentFoundryHostSession.cs  # §19 candidate host surface
  - src/Host/AgricultureHostSession.cs  # §19 candidate host surface
  - src/Host/AirlockSecurityHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/narrative/education_session_records.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/orbital_kinetic_telemetry.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Telemetry/
  - godot --headless --path . -- --advanced-industrial-recon-selftest
dependencies:
  - coordinate: 11 other plan(s) name these artifacts (§12)
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
