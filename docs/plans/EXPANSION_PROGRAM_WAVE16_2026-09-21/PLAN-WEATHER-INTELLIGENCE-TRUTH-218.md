# PLAN-WEATHER-INTELLIGENCE-TRUTH-218 — Forecast Fusion & Structural Hardening

**Wave 16 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-WEATHER-ATMOSPHERE-28, PLAN-WEATHER-SONDE-TRUTH-168, PLAN-SHELTER-ARCHITECTURE-40, PLAN-SEISMIC-DYNAMICS-TRUTH-193.
**Non-goals:** no weather simulation (Plan 28), no instrument model (Plan 168),
no construction costs (Plan 40).

## 1. Outcome
Two reachable systems are unaddressed: `World/WeatherIntelligenceCoordinator.cs`
(**356 lines**) and `World/WeatherHardeningSystem.cs` (**351 lines**). Plan 168
owns individual instrument readings; this coordinator **fuses** them into one
picture, and hardening is what the holdfast does about it. Neither contract is
stated, so forecasts are either per-instrument noise or a single perfect number.

| Deliverable | Detail |
|---|---|
| Fusion rule | multiple readings combine per documented weighting into one forecast picture per horizon; a degraded source lowers confidence, not truth |
| Confidence output | the fused forecast carries an explicit confidence band; consumers (Plan 80 alerts) read it |
| Hardening model | hardening measures per structure class with documented cost and damage reduction (Plan 40); applies against Plan 28/193 hazards |
| Inspection link | hardening condition decays via Plan 119; inspection discovers degradation |
| Save truth | fused state and hardening conditions restore; no re-roll on load |

## 2. Evidence
- `Assets/Ashfall.Core/World/WeatherIntelligenceCoordinator.cs` (356 lines) and `World/WeatherHardeningSystem.cs` (351 lines) — both unaddressed (Wave 16 audit).
- Plan 168's error bands feed the fusion; Plan 80 reads confidence.
- Plan 40 owns structures hardening modifies; Plan 193's seismic damage interacts.
- Plan 119 supplies condition decay.

## 3. Packages
- **WIT-218A** fusion rule + weighting table.
- **WIT-218B** confidence-band tests (degraded source → lower confidence).
- **WIT-218C** hardening cost/damage-reduction fixtures per structure class.
- **WIT-218D** inspection/decay link.
- **WIT-218E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Fused confidence falls with source degradation; a lost source is visible, not silent.
- Hardening measurably reduces damage in a scripted event; condition decays per contract.
- Save/load preserves state.
- `bash scripts/run_test.sh Ashfall.Core.Tests/World/`.

## 5. Risks
Perfect forecast → confidence band is always carried.
Hardening as flat buff → damage-reduction is per-class and conditioned by inspection.

---

## 6. Expanded census (5 files · 845 lines)

Scope: `Assets/Ashfall.Core/World/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · DTO/Type 1 · Loader 1 · System 2

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `WeatherHardeningCatalog.cs` | 65 | Catalog | — | 0 | 0 | 0 |
| `WeatherHardeningCatalogLoader.cs` | 34 | Loader | — | 0 | 0 | 0 |
| `WeatherHardeningState.cs` | 39 | DTO/Type | — | 0 | 0 | 0 |
| `WeatherHardeningSystem.cs` | 351 | System | **yes** | 0 | 0 | 2 |
| `WeatherIntelligenceCoordinator.cs` | 356 | System | **yes** | 0 | 0 | 10 |

**Totals:** 0 banned refs · 0 empty catches · 2 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `weather_hardening_upgrades.json` | object[2 keys] |

**State surfaces:** `WeatherHardeningSystem.cs`, `WeatherIntelligenceCoordinator.cs`.

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

Domain files: 5. Other plans referencing their names: **3**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-WEATHER-ATMOSPHERE-28` | 5 |
| `PLAN-WORLD-FAMILY-TRUTH-267` | 3 |
| `PLAN-WEATHER-SONDE-TRUTH-168` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `WIT-218A` | no name match — resolve at claim time |
| `WIT-218B` | no name match — resolve at claim time |
| `WIT-218C` | `WeatherHardeningCatalog.cs`, `WeatherHardeningCatalogLoader.cs`, `WeatherHardeningState.cs` |
| `WIT-218D` | no name match — resolve at claim time |
| `WIT-218E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 5. Host files: **6** · Test files: **4** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 6 | `src/Host/HostCli.DynamicWorld.cs`, `src/Host/WeatherHardeningHostSession.cs`, `src/Host/WeatherHardeningSaveStore.cs`, `src/Host/WorldHostSession.cs`, `src/Main.ShelterInfrastructure.cs` |
| Tests (`Ashfall.Core.Tests/`) | 4 | `Ashfall.Core.Tests/Campaign/CrisisPredictionTests.cs`, `Ashfall.Core.Tests/WeatherIntelligenceCoordinatorTests.cs`, `Ashfall.Core.Tests/World/Plan19DynamicWorldTests.cs`, `Ashfall.Core.Tests/World/WeatherHardeningSystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 5 files; intra-domain edges: **3**; isolated: **1**.

| From | → To |
|---|---|
| `WeatherHardeningCatalogLoader` | `WeatherHardeningCatalog` |
| `WeatherHardeningSystem` | `WeatherHardeningCatalog` |
| `WeatherHardeningSystem` | `WeatherHardeningState` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **2** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `counter_intelligence` |
| `weather_hardening` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **2** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--journal-weather-panel-selftest` |
| `--weather-save-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **2**.

| Event | First declaration |
|---|---|
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

Host files (`src/`) whose names share a domain token: **17**
(7 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Host/CounterIntelligenceHostSession.cs` |
| `src/Host/CounterIntelligenceSaveStore.cs` |
| `src/Host/FactionIconLoader.cs` |
| `src/Host/LoaderWiringSelfTest.cs` |
| `src/Host/WeatherHardeningHostSession.cs` |
| `src/Host/WeatherHardeningSaveStore.cs` |
| `src/Host/WeatherHostSession.cs` |
| `src/Host/WeatherSaveSelfTest.cs` |
| `src/Host/WeatherSaveStore.cs` |
| `src/UI/PanelSceneLoader.cs` |
| `src/UI/RadioIntelligencePanel.cs` |

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
**Surface:** save sections 2 (laddered 0) · RNG streams 1 · host files 13 · catalogs 11 · test regions 1 · flags 2

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-WEATHER-INTELLIGENCE-TRUTH-218
wave: 16
status: PROPOSED — foreman claim required
packages: WIT-218A, WIT-218B, WIT-218C, WIT-218D, WIT-218E
claim paths:
  - src/Audio/AudioStateCoordinator.cs  # §19 candidate host surface
  - src/Host/CounterIntelligenceHostSession.cs  # §19 candidate host surface
  - src/Host/CounterIntelligenceSaveStore.cs  # §19 candidate host surface
  - src/Host/FactionIconLoader.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/faction_intelligence.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/weather_almanac_expansion.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Weather/
  - godot --headless --path . -- --journal-weather-panel-selftest
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
