# PLAN-DETERMINISM-REPLAY-13 — Seeded Replay, Stream Governance & Golden Saves

**Program:** ASHFALL Expansion & Integration Program — Wave 2 (2026-09-21)
**Status:** PROPOSED — not a claim. Foreman claim required.
**Owner role on execution:** Integrator (stream registry, replay harness) with
Builders per tick they add.
**Depends on:** PLAN-INTEGRATION-KIT-02 (gates), PLAN-ORPHAN-SEAL-01 (daily
ticks), PLAN-VERTICAL-CULTURE-04 / BODY-INDUSTRY-05 (new roll sites).
**Expanded appendix:** [`PLAN-DETERMINISM-REPLAY-13_APPENDIX-A_STREAM_REGISTRY.md`](PLAN-DETERMINISM-REPLAY-13_APPENDIX-A_STREAM_REGISTRY.md)
— the RNG stream registry: **64 streams** with host/Core consumer counts (DR-13A gate input).

**Non-goals:** no change to existing stream values (fork-per-day contract), no
re-seeding of shipped campaigns, no determinism gate that runs the full suite.

---

## 1. Outcome

Every system wired by this programme rolls dice somewhere: festival selection,
market shocks, migration, treatment outcomes, terrain hazards, radio noise.
The repository's determinism contract (Invariant 4) says replay must be exact,
and the RNG source gate proves no `System.Random` leaks — but nothing today
proves that a *new* system forks a registered stream, uses the day as salt, or
survives a mid-run save/reload byte-for-byte.

Deliverables:

1. **stream governance** — a declared, snake_case stream per rolling system, a
   gate over new code, and a fork-per-day rule;
2. **replay harness** — extend the Plans174-177 pattern to every wired vertical:
   continuous 30-day run == `N days → save → reload → 30-N days` with a
   field-level fingerprint;
3. **golden saves** — a maintained corpus with an explicit rebaseline protocol;
4. **cross-host equality** — host, headless CLI and pure Core agree on the same
   seed;
5. **iteration-order audit** — no `GetHashCode`, no unordered dictionary
   iteration, no culture-sensitive formatting in simulation paths.

---

## 2. Premise evidence (2026-09-21)

| Fact | Value | Source |
|---|---:|---|
| Stream registry | `CampaignStreamIds` (~35 streams, snake_case) | `Assets/Ashfall.Core/Random/CampaignRngStream.cs` |
| RNG source gate | 2/2 green | `docs/ci/CI_GATE_MANIFEST.json` |
| `System.Random` in Core/src | 0 matches (non-comment) | audit grep 2026-09-21 |
| Replay precedent | `Plans174To177CampaignIntegrationTests` (30-day continuous == mid-reload, SHA-256) | INTEGRATION_PLANS ledger |
| Golden saves | `artifacts/golden_saves/` | artifacts listing |
| Runtime baseline evidence | 30-day advance median 1.29 s, p95 2.72 s, max 3.08 s, ~80 KB/run | `artifacts/runtime-scale-results.json` |
| New systems pending wiring | 99 authority files | PLAN-ORPHAN-SEAL-01 |
| Determinism-guard convention | fork-per-day, no shift of existing streams | `CampaignRngStream.cs` comments (Plans 122-125 precedent) |

---

## 3. Authority map

| Concern | Extend (do not create) |
|---|---|
| Stream ids | `CampaignStreamIds` (add constants only; never renumber) |
| Fork | `CampaignRngManager.Fork(stream, day, salt)` / `ISeededRng` |
| Fingerprints | existing save fingerprint/checksum utilities (`SaveChecksum`, invariant culture) |
| Replay tests | `Ashfall.Core.Tests/Plans174To177CampaignIntegrationTests.cs` pattern |
| Golden corpus | `artifacts/golden_saves/` + its capture script |
| Host equality | `--7-day-smoke-selftest`, `--real-campaign-journey-selftest`, `--campaign-fuzz-selftest` |

---

## 4. Packages

### DR-13A — Stream declaration and gate
- For every authority that consumes `ISeededRng`, require: a registered stream
  constant, a documented salt rule (day, system id, entity id in stable order),
  and a comment explaining the fork granularity (per-day vs per-event).
- Gate: a static script (`scripts/ci/rng-stream-gate.py`) that flags any
  `Fork(` call with a string literal not present in `CampaignStreamIds`, and any
  new `ISeededRng` consumer whose file has no stream reference.
- **Acceptance:** zero literal streams outside the registry; new constants are
  snake_case and land with the consuming package; the gate runs fast-tier.
- **Verify:** `python3 scripts/ci/rng-stream-gate.py --check`;
  `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/` (RNG source gate).

### DR-13B — Per-vertical replay harness
- For each wired vertical (shelter industry, economy, medical, world/weather,
  survivors, culture/broadsheet/post, polity/distance) add one replay test in
  the Plans174-177 shape: 30-day continuous vs 11-day + save/reload + 19-day,
  comparing a canonical fingerprint field-by-field and by hash.
- Fingerprint must include every authoritative field the vertical adds (state,
  counters, bounded logs, RNG cursor/day if persisted).
- **Acceptance:** each test proves (a) equality, (b) no re-roll on reload,
  (c) no wall-clock/culture use; failures print field-level diffs.
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/<vertical>/…IntegrationTests.cs`
  per vertical.

### DR-13C — Golden save corpus governance
- Define the corpus: one save per schema generation at a mid-campaign day,
  captured from a canonical seed. Rebasing requires a written diff summary and
  a `KNOWN_DEBT` note when the change is intentional.
- **Acceptance:** capture script is deterministic; corpus size bounded
  (one file per generation per family, not per system); CI compares loads and
  fingerprints.
- **Verify:** the corpus capture script + `bash scripts/run_test.sh Ashfall.Core.Tests/Save/`.

### DR-13D — Cross-host equality
- Pick one canonical seed and one 30-day run; execute it through (a) the Godot
  host headless tick loop, (b) the Core headless demo path, (c) the pure Core
  test harness (no host), and compare final fingerprints.
- **Acceptance:** three equal hashes; divergence prints the first differing
  field and owner; the probe is registered as one CLI verb.
- **Verify:** `godot --headless --path . -- --determinism-cross-host-selftest`.

### DR-13E — Iteration-order and culture audit
- Static + runtime checks for: `GetHashCode` in Core, unordered
  `Dictionary`/`HashSet` enumeration feeding RNG or ordering, `DateTime.Now`/
  wall-clock in Core, `ToString()` without invariant culture on numeric/date
  types in save/checksum paths.
- **Acceptance:** zero new findings; existing accepted findings either fixed or
  registered in the Core-only registry with a reason.
- **Verify:** `python3 scripts/ci/forbidden-api-gate.sh` (extend) +
  `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/`.

### DR-13F — Replay acceptance for the verticals
- Plans 04/05 packages may not claim "done" without a DR-13B test for their
  vertical; this plan publishes the exact template and the fingerprint field
  list.
- **Acceptance:** vertical closeout documents cite the replay test result.

---

## 5. Risk register

| Risk | Mitigation |
|---|---|
| Stream additions shift existing sequences | fork-per-day contract; new streams only; a fingerprint test on an untouched vertical proves non-interference |
| Replay harness cost grows quadratically | one test per vertical (7), bounded 30 days, cheap headless runs |
| Golden corpus churn | one file per generation; rebaseline needs a written reason |
| Cross-host divergence from Godot frame timing | runs are tick-driven, not frame-driven; the probe forbids frame-count salting |
| Iteration audit flags accepted patterns | registry rows for accepted findings with reasons |

## 6. Verification summary

```bash
python3 scripts/ci/rng-stream-gate.py --check
bash scripts/run_test.sh Ashfall.Core.Tests/Plans174To177CampaignIntegrationTests.cs
godot --headless --path . -- --determinism-cross-host-selftest
godot --headless --path . -- --campaign-fuzz-selftest
godot --headless --path . -- --7-day-smoke-selftest
```

## 7. Change control

`CampaignStreamIds` is integrator-owned; Builders add a constant only in the
same package as its consumer. No existing constant value may change. No new
`ISeededRng` consumption without a package-level salt rule and a replay test.

---

## 6. Expanded census (bespoke: determinism surface)

This plan gates determinism, so the census covers the RNG registry and the
banned-primitive surface.

| Metric | Value |
|---|---:|
| Registered RNG streams | 64 |
| Files with banned primitives | 43 |
| Banned-primitive references | 68 |

**Streams:** `weather`, `combat`, `disease`, `greenhouse`, `expedition`, `narrative`, `echo`, `economy`, `radio`, `social`, `moral_choice`, `shelter`, `duty_roster`, `muster`, `foundry`, `maritime`, `deep_coast`, `psychology`, `medical`, `events`, `low_background_metrology`, `insar_deformation`, `hydraulic_extrusion`, `runflat_tire` …

## 7. Expanded surface: determinism contract

| Rule | Detail |
|---|---|
| Streams | every rolling system forks from the registry; new literals added to it |
| Banned | wall-clock, `System.Random`, `Guid.NewGuid` in Core paths |
| Replay | same seed + same inputs → identical checksum |
| Order | stream draw order stable under composition changes |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Static scan | banned count above; target: 0 or justified |
| Paired run | same-seed checksum equality |
| Stream registry | every `Fork(` literal present |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census (this section).
2. Banned-primitive triage.
3. Stream-registry parity gate.
4. Paired replay fixture.
5. Regression: scan + paired run.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Stream | registered; consumers named |
| Banned ref | removed or justified in a deterministic wrapper |
| Replay | paired checksums equal |
| Gate | fails on a new unregistered fork |

**Non-goals unchanged:** this expansion adds census and verification detail.

---

## 12. Cross-plan coupling

This plan governs artifacts rather than a `.cs` domain; the domain set is the
plan's own backticked artifact list (7 files). Other plans referencing
those artifacts: **13**.

**Incoming plan edges (top 8):**

| Plan | Artifact mentions |
|---|---:|
| `PLAN-RUNTIME-PERF-16` | 2 |
| `PLAN-LAUNCH-FACE-06` | 1 |
| `PLAN-SAVE-GOVERNANCE-12` | 1 |
| `PLAN-DATA-AUTHORITY-14` | 1 |
| `PLAN-RELEASE-OPS-20` | 1 |
| `PLAN-SELFTEST-TRUTH-23` | 1 |
| `PLAN-INPUT-HARDENING-25` | 1 |
| `PLAN-ARCHITECTURE-BOUNDARY-31` | 1 |

**Artifacts (first 12):**

| Artifact |
|---|
| `Ashfall.Core.Tests/Plans174To177CampaignIntegrationTests.cs` |
| `Assets/Ashfall.Core/Random/CampaignRngStream.cs` |
| `CampaignRngStream.cs` |
| `PLAN-DETERMINISM-REPLAY-13_APPENDIX-A_STREAM_REGISTRY.md` |
| `artifacts/runtime-scale-results.json` |
| `docs/ci/CI_GATE_MANIFEST.json` |
| `scripts/ci/rng-stream-gate.py` |

**Package → candidate artifacts (heuristic by name overlap):**

| Package | Candidate artifacts |
|---|---|
| `DR-13A` | `Assets/Ashfall.Core/Random/CampaignRngStream.cs`, `CampaignRngStream.cs`, `PLAN-DETERMINISM-REPLAY-13_APPENDIX-A_STREAM_REGISTRY.md` |
| `DR-13B` | `PLAN-DETERMINISM-REPLAY-13_APPENDIX-A_STREAM_REGISTRY.md` |
| `DR-13C` | no name match — resolve at claim time |
| `DR-13D` | no name match — resolve at claim time |
| `DR-13E` | no name match — resolve at claim time |
| `DR-13F` | `PLAN-DETERMINISM-REPLAY-13_APPENDIX-A_STREAM_REGISTRY.md` |

**Reading:** incoming edges are coordination risk; artifacts are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 6. Host files: **3** · Test files: **5** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 3 | `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/HostCli.EvolvingWorld.cs`, `src/Host/HostCli.WorldPlaytest.cs` |
| Tests (`Ashfall.Core.Tests/`) | 5 | `Ashfall.Core.Tests/Campaign/CampaignRngStreamTests.cs`, `Ashfall.Core.Tests/EvolvingWorldActivationTests.cs`, `Ashfall.Core.Tests/Plans174To177CampaignIntegrationTests.cs`, `Ashfall.Core.Tests/Tooling/CiGateManifestDriftTests.cs`, `Ashfall.Core.Tests/WildlifeSeasonalCalendarTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **4** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `campaign` |
| `campaign_day` |
| `wildlife_ecosystem` |
| `wildlife_trapping` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **15** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--campaign-journey-selftest` |
| `--evolving-world-selftest` |
| `--expedition-playtest-selftest` |
| `--plans-122-125-balance-soak` |
| `--plans-122-125-selftest` |
| `--plans-139-141-selftest` |
| `--propaganda-campaign-selftest` |
| `--real-campaign-journey-selftest` |
| `--runtime-scale` |
| `--runtime-scale-selftest` |
| `--selftest-manifest` |
| `--social-drift-selftest` |

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

Catalog JSON files whose names share a domain token: **9**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/campaign_epilogues.json` |
| `Assets/StreamingAssets/Data/mod_manifest_schema.json` |
| `Assets/StreamingAssets/Data/narrative/blast_gate_mechanical_audits.json` |
| `Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json` |
| `Assets/StreamingAssets/Data/narrative/wildlife_field_encounter_logs.json` |
| `Assets/StreamingAssets/Data/narrative_discovery_manifest.json` |
| `Assets/StreamingAssets/Data/seasonal_events.json` |
| `Assets/StreamingAssets/Data/wildlife_ecosystem.json` |
| `Assets/StreamingAssets/Data/wildlife_trapping_catalog.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **11** (110 files, 843 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Bestiary` | 1 | 6 |
| `Campaign` | 32 | 187 |
| `Collectibles` | 11 | 72 |
| `Difficulty` | 5 | 24 |
| `Events` | 1 | 6 |
| `Flagship11` | 7 | 63 |
| `Integration` | 16 | 74 |
| `Narrative` | 26 | 293 |
| `NarrativeConsequence` | 1 | 20 |

**Verdict:** 843 cases sit under matching regions — run those first (`Audio`, `Bestiary`, `Campaign`, `Collectibles`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **84**
(5 of them panels/HUD).

| Host file |
|---|
| `src/Host/CampaignDayPersistenceAdapter.cs` |
| `src/Host/CampaignDaySaveStore.cs` |
| `src/Host/ContentUtilizationRuntimeCollector.cs` |
| `src/Host/ContentUtilizationSelfTest.cs` |
| `src/Host/HostCli.AdvancedIndustrialRecon.cs` |
| `src/Host/HostCli.Cartography.cs` |
| `src/Host/HostCli.Collectibles.cs` |
| `src/Host/HostCli.Difficulty.cs` |
| `src/Host/HostCli.DynamicWorld.cs` |
| `src/Host/HostCli.EvolvingWorld.cs` |
| `src/Host/HostCli.ExpansionDepth.cs` |
| `src/Host/HostCli.ExpeditionPlaytest.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **19**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `campaign` | no |
| `campaign_day` | no |
| `chemical_recon` | no |
| `collectible_discovery` | no |
| `dynamic_quests` | no |
| `encounter_choice` | no |
| `events` | no |
| `expedition` | no |
| `expedition_stealth` | no |
| `field_guide` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **9**.

| Stream |
|---|
| `advanced_mfg_ebpvd_coating` |
| `events` |
| `expedition` |
| `metrology_calibration_drift` |
| `narrative` |
| `wildlife_apex` |
| `wildlife_migration` |
| `wildlife_population` |
| `wildlife_taming` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **306**
(CODEX_ONLY 279, GAMEPLAY_CONSUMED 14, OPTIONAL 3, UNRESOLVED 10).

| Catalog | Classification |
|---|---|
| `anomalous_expedition_encounters.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `collectibles.json` | UNRESOLVED |
| `contagion_events.json` | UNRESOLVED |
| `desperation_events.json` | GAMEPLAY_CONSUMED |
| `dynamic_questlines.json` | GAMEPLAY_CONSUMED |
| `events.json` | GAMEPLAY_CONSUMED |
| `faction_war_events.json` | GAMEPLAY_CONSUMED |
| `field_guide.json` | UNRESOLVED |
| `muster_epilogues.json` | GAMEPLAY_CONSUMED |

**Verdict:** 10 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** free-start · **Coupling (incoming plans):** 0
**Surface:** save sections 19 (laddered 0) · RNG streams 9 · host files 21 · catalogs 19 · test regions 10 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-DETERMINISM-REPLAY-13
wave: —
status: PROPOSED — foreman claim required
packages: DR-13A, DR-13B, DR-13C, DR-13D, DR-13E, DR-13F
claim paths:
  - src/Host/CampaignDayPersistenceAdapter.cs  # §19 candidate host surface
  - src/Host/CampaignDaySaveStore.cs  # §19 candidate host surface
  - src/Host/ContentUtilizationRuntimeCollector.cs  # §19 candidate host surface
  - src/Host/ContentUtilizationSelfTest.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/campaign_epilogues.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/mod_manifest_schema.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --campaign-journey-selftest
dependencies:
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
