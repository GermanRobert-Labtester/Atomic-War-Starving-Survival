# PLAN-WORLD-EVOLUTION-TRUTH-227 — The World Between Visits: Slow Change & Drift

**Wave 17 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SPATIAL-SIM-AUTHORITY-95, PLAN-DISCOVERY-STATE-108, PLAN-SEISMIC-DYNAMICS-TRUTH-193, PLAN-ECOLOGY-WILDLIFE-26.
**Non-goals:** no map topology (Plan 95), no knowledge state (Plan 108), no
event generation (Plan 193).

## 1. Outcome
`World/WorldEvolutionEngine.cs` (**318 lines**) is reachable and unaddressed:
the world changes while the player is away — sites decay, wildlife shifts,
routes silt up. Each specific change has owners (Plans 26/95/193); the
**unattended-drift scheduler** that advances them off-screen is unowned, so the
world is either static or changes unpredictably.

| Deliverable | Detail |
|---|---|
| Drift table | per subject class (site, route, wildlife, hazard), the off-screen change rule and its owner |
| Cadence | drift evaluated at documented day boundaries, not per frame; catch-up after load is correct |
| Bounds | drift cannot create/destroy a site or route without an owner event; value changes only |
| Visibility | returning players see drift through existing surfaces (map, discovery, journal), never a hidden correction |
| Determinism | seeded where variance exists; paired runs equal |

## 2. Evidence
- `Assets/Ashfall.Core/World/WorldEvolutionEngine.cs` (318 lines; unaddressed — Wave 17 audit).
- Plans 26/95/108/193 own the subject classes this engine advances.
- `World/SeasonalEventSystem.cs` (200 lines, also unaddressed) is a likely input — named for the package.
- Plan 33's clock provides day boundaries.

## 3. Packages
- **WET-227A** drift table + owner map.
- **WET-227B** day-boundary cadence + catch-up tests.
- **WET-227C** bounds fixtures (no site/route creation or loss).
- **WET-227D** visibility test through existing surfaces.
- **WET-227E** paired-run determinism.

## 4. Acceptance & verification
- Drift applies only through owners; bounds hold in a long scripted absence.
- Catch-up after save/load matches a continuous run.
- `bash scripts/run_test.sh Ashfall.Core.Tests/World/`.

## 5. Risks
Hidden corrections → drift is owner-routed and visible on return.
Frame-time drift → day-boundary cadence is a fixture.

---

## 6. Expanded census (3 files · 585 lines)

Scope: `Assets/Ashfall.Core/World/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · System 2

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `SeasonalEventCatalog.cs` | 67 | Catalog | — | 0 | 0 | 0 |
| `SeasonalEventSystem.cs` | 200 | System | — | 0 | 0 | 2 |
| `WorldEvolutionEngine.cs` | 318 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 2 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `world_evolution_events.json` | object[2 keys] |
| `seasonal_events.json` | object[2 keys] |
| `world_evolution_seeds.json` | object[9 keys] |

**State surfaces:** `SeasonalEventSystem.cs`, `WorldEvolutionEngine.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/World/` |
| Test references | 4 name references across the test tree |
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

Domain files: 3. Other plans referencing their names: **1**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-WORLD-FAMILY-TRUTH-267` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `WET-227A` | no name match — resolve at claim time |
| `WET-227B` | no name match — resolve at claim time |
| `WET-227C` | no name match — resolve at claim time |
| `WET-227D` | no name match — resolve at claim time |
| `WET-227E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **2** · Test files: **3** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/Host/HostCli.DynamicWorld.cs`, `src/Host/HostCli.WorldExploration.cs` |
| Tests (`Ashfall.Core.Tests/`) | 3 | `Ashfall.Core.Tests/Combat/Plan10_11CombatExplorationIntegrationTests.cs`, `Ashfall.Core.Tests/World/Plan11ExplorationTests.cs`, `Ashfall.Core.Tests/World/Plan19DynamicWorldTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 3 files; intra-domain edges: **0**; isolated: **3**.

| From | → To |
|---|---|
| — | no intra-domain references found |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **1** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `host_event` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **0** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| — | no CLI flag shares a token with this domain |

**Verdict:** no CLI or selftest flag shares a token with this domain — the outcome is not operator-observable yet. Add coverage in the owning plan if it must be verifiable.

---

## 16. Event-route reachability

Events whose name shares a domain token: **2**.

| Event | First declaration |
|---|---|
| `OnCombatEvent` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnEventRaised` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **3**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/seasonal_events.json` |
| `Assets/StreamingAssets/Data/world_evolution_events.json` |
| `Assets/StreamingAssets/Data/world_evolution_seeds.json` |

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

Host files (`src/`) whose names share a domain token: **4**
(1 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioEventBridge.cs` |
| `src/Host/HostEventAdapter.cs` |
| `src/Host/HostEventSaveStore.cs` |
| `src/UI/EventDetailPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **1**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `host_event` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **2**.

| Stream |
|---|
| `black_market_debt_event` |
| `world_evolution` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **3**
(GAMEPLAY_CONSUMED 1, UNRESOLVED 2).

| Catalog | Classification |
|---|---|
| `seasonal_events.json` | UNRESOLVED |
| `world_evolution_events.json` | UNRESOLVED |
| `world_evolution_seeds.json` | GAMEPLAY_CONSUMED |

**Verdict:** 2 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

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
**Surface:** save sections 1 (laddered 0) · RNG streams 2 · host files 6 · catalogs 6 · test regions 0 · flags 0

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-WORLD-EVOLUTION-TRUTH-227
wave: 17
status: PROPOSED — foreman claim required
packages: WET-227A, WET-227B, WET-227C, WET-227D, WET-227E
claim paths:
  - src/Audio/AudioEventBridge.cs  # §19 candidate host surface
  - src/Host/HostEventAdapter.cs  # §19 candidate host surface
  - src/Host/HostEventSaveStore.cs  # §19 candidate host surface
  - src/UI/EventDetailPanel.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/seasonal_events.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/world_evolution_events.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh <focused-region>/  # resolve target at claim time
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
