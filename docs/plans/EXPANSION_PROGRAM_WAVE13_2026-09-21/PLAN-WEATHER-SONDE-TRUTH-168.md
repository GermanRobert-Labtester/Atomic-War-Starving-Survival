# PLAN-WEATHER-SONDE-TRUTH-168 — Measurement, Forecast Inputs & Instrument Maintenance

**Wave 13 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-WEATHER-ATMOSPHERE-28, PLAN-CRISIS-DISASTER-RESPONSE-80, PLAN-MAINTENANCE-DECAY-TRUTH-119.
**Implementation scaffold:** [`PLAN-WEATHER-SONDE-TRUTH-168_APPENDIX-A_SCAFFOLD.md`](PLAN-WEATHER-SONDE-TRUTH-168_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-WEATHER-INTELLIGENCE-TRUTH-218` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no weather simulation (Plan 28 owns it), no forecast UI (Plan 80's
alerts consume it), no new instrument catalog.

## 1. Outcome
`World/WeatherSondeSystem.cs` (692 lines) is reachable and unaddressed. Plan 28
produces weather; Plan 80 reacts to it. A sonde is the **measurement layer** —
what the holdfast can know before weather arrives, how instrument condition
degrades that knowledge, and how a forecast's accuracy is derived. Without a
contract, forecasts are either perfect or random.

| Deliverable | Detail |
|---|---|
| Instrument model | sondes with condition (Plan 119 contract), calibration state, and coverage |
| Measurement truth | a reading is the true value plus documented error bands determined by condition/calibration; seeded noise from a registered stream |
| Forecast derivation | forecast accuracy derives from the reading error band; a worn instrument narrows warning time, not the eventual weather |
| Coverage rule | no sonde → documented fallback (visual signs) with reduced lead time; never a hidden perfect forecast |
| Save truth | instrument state and the latest reading restore; a load never re-measures |

## 2. Evidence
- `Assets/Ashfall.Core/World/WeatherSondeSystem.cs` (692 lines; unmentioned in every plan body — Wave 13 audit).
- Plan 28 owns the weather values the sonde reads.
- Plan 80's alerts consume forecast lead time.
- Plan 119 supplies the decay contract for instrument condition.

## 3. Packages
- **WST-168A** instrument model + condition/calibration table.
- **WST-168B** measurement error-band tests (condition vs band).
- **WST-168C** forecast derivation + lead-time fixture per condition class.
- **WST-168D** no-sonde fallback test (reduced lead time, never perfect).
- **WST-168E** save round-trip; no re-measure on load.

## 4. Acceptance & verification
- Error bands widen as condition falls; same seed → same reading.
- Forecast lead time changes with the band; the eventual weather (Plan 28) is unchanged.
- Absent sonde yields the documented fallback, not an error and not perfection.
- `bash scripts/run_test.sh Ashfall.Core.Tests/World/`.

## 5. Risks
Forecast becoming authority → it is a reading of Plan 28's state; the eventual weather test enforces it.
Instrument neglect being invisible → condition and calibration are surfaced in the instrument view.

---

## 6. Expanded census (2 files · 1,048 lines)

Scope: `Assets/Ashfall.Core/World/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 2

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `WeatherIntelligenceCoordinator.cs` | 356 | System | — | 0 | 0 | 10 |
| `WeatherSondeSystem.cs` | 692 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 2 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** `WeatherIntelligenceCoordinator.cs`, `WeatherSondeSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/World/` |
| Test references | 7 name references across the test tree |
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

Domain files: 2. Other plans referencing their names: **3**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-WEATHER-ATMOSPHERE-28` | 2 |
| `PLAN-SIGNALS-REMOTE-SENSING-49` | 1 |
| `PLAN-WEATHER-INTELLIGENCE-TRUTH-218` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `WST-168A` | no name match — resolve at claim time |
| `WST-168B` | no name match — resolve at claim time |
| `WST-168C` | no name match — resolve at claim time |
| `WST-168D` | `WeatherSondeSystem.cs` |
| `WST-168E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **5** · Test files: **7** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 5 | `src/Host/HostCli.DynamicWorld.cs`, `src/Host/WeatherHostSession.cs`, `src/Host/WorldHostSession.cs`, `src/UI/WeatherForecastPanel.cs`, `src/UI/WeatherSondePanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 7 | `Ashfall.Core.Tests/Campaign/CrisisPredictionTests.cs`, `Ashfall.Core.Tests/CampaignContinuityFlagshipB70_B73Tests.cs`, `Ashfall.Core.Tests/Core/DeterminismSeedSweepTests.cs`, `Ashfall.Core.Tests/ProductionGameplayApiTests.cs`, `Ashfall.Core.Tests/WeatherIntelligenceCoordinatorTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **7** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `campaign` |
| `campaign_day` |
| `counter_intelligence` |
| `dynamic_quests` |
| `mental_health_crisis` |
| `radio_program_production` |
| `weather_hardening` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **11** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--campaign-journey-selftest` |
| `--checksum-sweep-selftest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--journal-weather-panel-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |
| `--propaganda-campaign-selftest` |
| `--real-campaign-journey-selftest` |
| `--weather-save-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **10**.

| Event | First declaration |
|---|---|
| `OnCrisisResolved` | `Assets/Ashfall.Core/MentalHealthCrisisSystem.cs` |
| `OnEnvironmentalCrisisTriggered` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` |
| `OnForecastConfidenceChanged` | `Assets/Ashfall.Core/World/WeatherSondeSystem.cs` |
| `OnForecastUpdated` | `Assets/Ashfall.Core/WeatherStationSystem.cs` |
| `OnProductionCompleted` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnProductionTick` | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` |
| `OnSondeFailed` | `Assets/Ashfall.Core/World/WeatherSondeSystem.cs` |
| `OnSondeRecovered` | `Assets/Ashfall.Core/World/WeatherSondeSystem.cs` |
| `OnWeatherChanged` | `Assets/Ashfall.Core/World/WeatherSystem.cs` |
| `OnWeatherFrontArrived` | `Assets/Ashfall.Core/Weather/WeatherCascadeSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **7**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/faction_intelligence.json` |
| `Assets/StreamingAssets/Data/narrative/weather_almanac_expansion.json` |
| `Assets/StreamingAssets/Data/weather_effects.json` |
| `Assets/StreamingAssets/Data/weather_gameplay_effects.json` |
| `Assets/StreamingAssets/Data/weather_hardening_upgrades.json` |
| `Assets/StreamingAssets/Data/weather_route_gates.json` |
| `Assets/StreamingAssets/Data/weather_seasons.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (2 files, 11 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Weather` | 2 | 11 |

**Verdict:** 11 cases sit under matching regions — run those first (`Weather`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **14**
(6 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Host/CounterIntelligenceHostSession.cs` |
| `src/Host/CounterIntelligenceSaveStore.cs` |
| `src/Host/WeatherHardeningHostSession.cs` |
| `src/Host/WeatherHardeningSaveStore.cs` |
| `src/Host/WeatherHostSession.cs` |
| `src/Host/WeatherSaveSelfTest.cs` |
| `src/Host/WeatherSaveStore.cs` |
| `src/UI/RadioIntelligencePanel.cs` |
| `src/UI/WeatherDetailPanel.cs` |
| `src/UI/WeatherForecastPanel.cs` |
| `src/UI/WeatherHistoryPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **2**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `counter_intelligence` | no |
| `weather_hardening` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `weather` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **4**
(CODEX_ONLY 1, GAMEPLAY_CONSUMED 2, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `narrative/weather_almanac_expansion.json` | CODEX_ONLY |
| `weather_hardening_upgrades.json` | GAMEPLAY_CONSUMED |
| `weather_route_gates.json` | UNRESOLVED |
| `weather_seasons.json` | GAMEPLAY_CONSUMED |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 3
**Surface:** save sections 2 (laddered 0) · RNG streams 1 · host files 13 · catalogs 11 · test regions 1 · flags 11

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-WEATHER-SONDE-TRUTH-168
wave: 13
status: PROPOSED — foreman claim required
packages: WST-168A, WST-168B, WST-168C, WST-168D, WST-168E
claim paths:
  - src/Audio/AudioStateCoordinator.cs  # §19 candidate host surface
  - src/Host/CounterIntelligenceHostSession.cs  # §19 candidate host surface
  - src/Host/CounterIntelligenceSaveStore.cs  # §19 candidate host surface
  - src/Host/WeatherHardeningHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/faction_intelligence.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/weather_almanac_expansion.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Weather/
  - godot --headless --path . -- --campaign-journey-selftest
dependencies:
  - coordinate: 3 other plan(s) name these artifacts (§12)
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
