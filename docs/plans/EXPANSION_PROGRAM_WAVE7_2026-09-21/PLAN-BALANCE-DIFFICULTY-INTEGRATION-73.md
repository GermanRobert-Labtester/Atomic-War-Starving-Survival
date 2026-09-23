# PLAN-BALANCE-DIFFICULTY-INTEGRATION-73 — Presets, Baselines & Seeded Sweeps

**Wave 7 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-UNBLOCK-03 U7 (XP-01), PLAN-RUNTIME-PERF-16,
PLAN-DETERMINISM-REPLAY-13.
**Expanded appendix:** [`PLAN-BALANCE-DIFFICULTY-INTEGRATION-73_APPENDIX-A_SCALAR_CATALOG.md`](PLAN-BALANCE-DIFFICULTY-INTEGRATION-73_APPENDIX-A_SCALAR_CATALOG.md)
— the difficulty scalar catalog: **4 presets** with every scalar and value (BD-73B consumer census input).

**Non-goals:** no balance-by-vibes; no change to `DEC-07` baselines without a
sweep; no new difficulty authority.

## Outcome
Difficulty is half-bound (XP-01 active: catalog + director + one consumer +
completion tag; no preset selection, no persistence, no panel) and balance
evidence exists in artifacts (`artifacts/balance/`, radiation sweeps,
`BalanceFedLoopTelemetryTests`, `Plan20BShieldingBalanceSweepTests`) but is not
a repeatable programme. This plan closes the binding and makes balance a
**measured, versioned practice**.

| Deliverable | Detail |
|---|---|
| Preset binding | selection at campaign creation, `difficulty_preset_id` persisted, all authored scalars consumed |
| Scalar census | every authored scalar has a consumer (ties Plan 22); hardcoded fallbacks retired |
| Baseline registry | `DEC-07` baselines + new sweeps recorded under `docs/balance/` with seed + commit |
| Sweep harness | seeded headless runs across presets (economy, radiation, needs, war) with pass bands |
| Monotonicity | higher difficulty never improves a player outcome; validated across presets |
| Panel | difficulty readout with consequences (weave from `DEC-40`) |
| CI | bounded sweep gate (nightly), not full-suite |

## Evidence
- XP-01 evidence: host resolves default preset only (`Main.Difficulty.cs`), one consumer (`HostileEncounterMult`), no save section, no panel.
- `DEC-40` `DifficultyConsequenceWeave` (host-pending), `DEC-21/38` body state, `DEC-07` baselines signed.
- Artifacts: `artifacts/balance/`, `balance-sim-expeditions.json`, `runtime-scale-results.json`.
- Skills: `ashfall-balance-sim` (seeded sweeps via dotnet + godot --headless).

## Packages
- **BD-73A** preset selection + persistence: creation UI/CLI; save field; legacy default.
- **BD-73B** scalar census: author → consumer table; hardcoded values replaced by scalars where authored.
- **BD-73C** baseline registry: each sweep documented with seed, commit, config, result bands.
- **BD-73D** sweep harness: one command per domain with assertions (e.g. starvation days, dose exceedance, war escalation).
- **BD-73D2** monotonicity probe across presets (outcome order).
- **BD-73E** panel readout: current preset + active modifiers via the weave read model.
- **BD-73F** nightly sweep gate: file regressions; bounded runtime.

## Acceptance & verification
- A campaign created on Hard persists and consumes every authored scalar; sweeps pass bands; monotonicity holds.
- `godot --headless --path . -- --difficulty-selftest`; `bash scripts/run_test.sh Ashfall.Core.Tests/Difficulty/`; balance harness command.

## Risks
Difficulty rebalancing invalidates old saves → additive defaults, no recalculation on load.

---

## 6. Expanded census (bespoke: difficulty & balance surface)

This plan binds difficulty scalars and balance baselines, so the census covers
the preset catalog and balance artifacts.

| Metric | Value |
|---|---:|
| Presets | 4 |
| Distinct scalars | 8 |
| Default preset | `difficulty_standard` |
| Balance/runtime artifacts | 6 |

**Scalar matrix:**

| Scalar | SPARING | STANDARD | AUSTERE | DIRGE |
|---|
| `crisis_deadline_mult` | 1.25 | 1.0 | 0.8 | 0.65 |
| `disease_onset_mult` | 0.8 | 1.0 | 1.25 | 1.5 |
| `equipment_decay_mult` | 0.8 | 1.0 | 1.25 | 1.5 |
| `hostile_encounter_mult` | 0.75 | 1.0 | 1.35 | 1.75 |
| `hunger_rate_mult` | 0.75 | 1.0 | 1.35 | 1.75 |
| `market_price_mult` | 0.9 | 1.0 | 1.15 | 1.3 |
| `radiation_gain_mult` | 0.75 | 1.0 | 1.3 | 1.6 |
| `thirst_rate_mult` | 0.75 | 1.0 | 1.35 | 1.75 |

## 7. Expanded surface: binding contract

| Rule | Detail |
|---|---|
| Live consumers | every scalar has a host consumer; an unconsumed scalar is reported |
| Monotonicity | each scalar is non-decreasing in difficulty order (per its direction) |
| Baseline | balance artifacts recorded per build; deltas reported |
| No hardcoded fallbacks | a missing scalar falls back to STANDARD with a visible note |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Consumer census | scalar ↔ consumer table complete |
| Monotonicity | matrix validated left-to-right |
| Sweep | one seeded sweep per preset records its baseline |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Scalar census and consumer table (this section).
2. Monotonicity validation.
3. Baseline sweep per preset.
4. Regression: deltas reported, not enforced silently.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Scalar | live consumer named; no dead scalar |
| Preset | monotonic per its declared direction |
| Baseline | recorded per preset; deltas visible |
| Fallback | STANDARD default with a note |

**Non-goals unchanged:** this expansion adds census and verification detail; balance tuning stays with the owning plans.

---

## 12. Cross-plan coupling

Domain method: plan-body artifact list.
Governed artifacts: 4. Other plans referencing them: **4**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-DETERMINISM-REPLAY-13` | 1 |
| `PLAN-RUNTIME-PERF-16` | 1 |
| `PLAN-TELEMETRY-PRIVACY-58` | 1 |
| `PLAN-AUTOMATED-QA-CAMPAIGNS-74` | 1 |

**Governed artifacts (first 12):**

| Artifact |
|---|
| `Main.Difficulty.cs` |
| `PLAN-BALANCE-DIFFICULTY-INTEGRATION-73_APPENDIX-A_SCALAR_CATALOG.md` |
| `balance-sim-expeditions.json` |
| `runtime-scale-results.json` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `BD-73A` | no name match — resolve at claim time |
| `BD-73B` | `PLAN-BALANCE-DIFFICULTY-INTEGRATION-73_APPENDIX-A_SCALAR_CATALOG.md` |
| `BD-73C` | `runtime-scale-results.json` |
| `BD-73D` | no name match — resolve at claim time |
| `BD-73D2` | no name match — resolve at claim time |
| `BD-73E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 4. Host files: **0** · Test files: **0** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 0 | — |
| Tests (`Ashfall.Core.Tests/`) | 0 | — |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no host attachment found for these symbols — candidate integration gap (confirm under alternate names); no test reference found — coverage risk; no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **1** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `expeditions` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **5** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--difficulty-selftest` |
| `--plans-122-125-balance-soak` |
| `--real-main-journey-selftest` |
| `--runtime-scale` |
| `--runtime-scale-selftest` |

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
| `Assets/StreamingAssets/Data/difficulty_presets.json` |
| `Assets/StreamingAssets/Data/expeditions.json` |
| `Assets/StreamingAssets/Data/narrative/honey_extractor_balance_reports.json` |
| `Assets/StreamingAssets/Data/narrative/wasteland_expeditions_master.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **10** (240 files, 1812 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Balance` | 1 | 5 |
| `Campaign` | 32 | 187 |
| `Difficulty` | 5 | 24 |
| `Economy` | 41 | 329 |
| `Expeditions` | 41 | 322 |
| `Integration` | 16 | 74 |
| `Radiation` | 10 | 78 |
| `Shelter` | 87 | 754 |
| `Telemetry` | 2 | 11 |

**Verdict:** 1812 cases sit under matching regions — run those first (`Audio`, `Balance`, `Campaign`, `Difficulty`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **149**
(5 of them panels/HUD).

| Host file |
|---|
| `src/Host/HostCli.Difficulty.cs` |
| `src/Main.AdvancedShelterSystems.cs` |
| `src/Main.Anomaly.cs` |
| `src/Main.Application.cs` |
| `src/Main.Audio.cs` |
| `src/Main.Bionics.cs` |
| `src/Main.BlackMarket.cs` |
| `src/Main.BriefingCrisis.cs` |
| `src/Main.Campaign.cs` |
| `src/Main.CampaignOwners.cs` |
| `src/Main.CampaignServices.cs` |
| `src/Main.Cascade.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **27**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `anomaly_hazard` | no |
| `bionics` | no |
| `black_market` | no |
| `black_projects_archive` | no |
| `campaign` | no |
| `campaign_day` | no |
| `daily_briefing` | no |
| `expanded_shelter` | no |
| `expeditions` | no |
| `mental_health_crisis` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **7**.

| Stream |
|---|
| `advanced_mfg_ebpvd_coating` |
| `anomaly_hazard` |
| `bionics` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `shelter` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **25**
(CODEX_ONLY 11, GAMEPLAY_CONSUMED 8, OPTIONAL 1, UNRESOLVED 5).

| Catalog | Classification |
|---|---|
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `expeditions.json` | GAMEPLAY_CONSUMED |
| `narrative/courier_dispatches_master.json` | CODEX_ONLY |
| `narrative/dweller_heirlooms_master.json` | CODEX_ONLY |
| `narrative/honey_extractor_balance_reports.json` | CODEX_ONLY |
| `narrative/shelter_notices_expansion.json` | CODEX_ONLY |
| `narrative/shelter_songs_expansion.json` | CODEX_ONLY |
| `narrative/wasteland_expeditions_master.json` | CODEX_ONLY |

**Verdict:** 5 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 4
**Surface:** save sections 27 (laddered 0) · RNG streams 7 · host files 19 · catalogs 14 · test regions 10 · flags 5

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-BALANCE-DIFFICULTY-INTEGRATION-73
wave: 7
status: PROPOSED — foreman claim required
packages: BD-73A, BD-73B, BD-73C, BD-73D, BD-73D2, BD-73E, BD-73F
claim paths:
  - src/Host/HostCli.Difficulty.cs  # §19 candidate host surface
  - src/Main.AdvancedShelterSystems.cs  # §19 candidate host surface
  - src/Main.Anomaly.cs  # §19 candidate host surface
  - src/Main.Application.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/difficulty_presets.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/expeditions.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --difficulty-selftest
dependencies:
  - coordinate: 4 other plan(s) name these artifacts (§12)
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
