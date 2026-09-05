# PLAN F21 — Discovery Selection-Context Extension (Season / Drought / Skill Weights)

**Class:** P2 follow-up from the F17–F20 flagship integration (flagship plan §8.10, §9.10, §10.10 — all three documented as "investigated, deferred").
**Status:** FILED — not started.
**Owner system:** Discovery / micro-location selection only. **No gameplay logic in the greenhouse, radio, or water systems.**

---

## 1. Goal (2 lines)

Give the narrative micro-location selector an explicit, deterministic selection context (season, drought/water-scarcity, survivor skill) so `micro_ruined_greenhouse`, `micro_water_source`, and `micro_radio_tower` can be weighted by authored world state — through the one canonical selector, with zero new RNG and zero per-system selection logic.

## 2. Evidence baseline (recon from F17–F20)

| Finding | Location |
|---|---|
| Narrative selection context today = (stance, dangerLevel, locationId) + weather-gate filter. **No season, drought, or skill input.** | `NarrativeEncounterSystem.GetEligibleCandidates` / `SelectEncounter` |
| The **patrol** path already receives `CurrentSeason` at the bridge; the **narrative** path does not | `ExpeditionEncounterBridge.cs:126-142` |
| Proven in-repo pattern for an explicit deterministic selection context ("never read from a global singleton") | `TravelEncounterSelectionContext` (`Narrative/TravelEncounterSelectionContext.cs`) |
| Season authority exists (JSON seasonal weather windows) | `WeatherSystem` + `SeasonWindowDef` |
| Host already holds and pushes `CurrentSeason` into the bridge | `ExpeditionHostSession._currentSeason` / `CurrentSeason` |
| Weight formula today: `baseWeight × stance multiplier`, floored at 0, danger/destination filters first | `EncounterDefinition.GetEffectiveWeight` |

## 3. Design contract

### 3.1 Context in — never global reads

Mirror `TravelEncounterSelectionContext`: introduce `NarrativeEncounterSelectionContext` (Core, `Ashfall.Core.Narrative`) carrying `Stance`, `DangerLevel`, `LocationId`, `CurrentSeason`, `DroughtLevel` (float 0–1 or enum tier from the water/economy authority), `SkillLevel` (survivor skill for the site's affinity domain, optional), `Rng` (the same shared campaign stream the caller already holds). `GetEligibleCandidates` gains an overload accepting the context; the existing 3-arg overload delegates with a neutral default context (all multipliers 1) so every existing caller and test is bit-identical until opted in.

### 3.2 Data-driven weights on `EncounterDefinition`

Optional, backward-compatible JSON fields (snake_case, `schema_version` bump per data authority rules):

```json
"seasonWeightMultipliers": { "growing_season": 1.35, "deep_freeze": 0.8 },
"droughtWeightMultipliers": { "mild": 1.0, "severe": 1.25 },
"affinitySkillId": "skill_signal_ear",
"skillWeightMultiplier": 1.5
```

- `micro_ruined_greenhouse` (F18): ×1.25–1.50 in `growing_season`, normal-to-reduced in `deep_freeze`. Exact numbers set by project balancing conventions, in data — not code.
- `micro_water_source` (F20): detection improves with drought severity (survivors actively search), **but** the authored grant (3 vs 2 `clean_water`) and one-shot depletion stay untouched — scarcity is never relieved by weighting, only findability.
- `micro_radio_tower` (F19): `skillWeightMultiplier` keyed to the tower's affinity skill when the survivor roster exposes one.

### 3.3 Determinism invariants (non-negotiable)

- Weights change only the candidate weight sum before the single existing `rng.NextDouble()` roll. No additional draws, no new RNG sources, no wall clock. `MicroLocationDeterminismTests`' zero-divergence harness must stay green with contexts applied.
- Same context + same state ⇒ same selection. The context is captured per selection call, never cached across ticks.
- Depleted encounters remain excluded before weighting (existing F1 rule).

### 3.4 Ownership boundaries

| System | Allowed | Forbidden |
|---|---|---|
| `NarrativeEncounterSystem` | Accept context, multiply weights | Read weather/season/drought state directly |
| `ExpeditionEncounterBridge` / host | Populate the context from `WeatherSystem`/season/economy authorities | Per-site special cases |
| Greenhouse / Radio / Water authorities | Nothing — they never learn which encounter granted an item | Any selection knowledge |

## 4. Implementation phases

1. **Core context type + overload** — `NarrativeEncounterSelectionContext`, neutral defaults, existing callers untouched. Tests: neutral context is bit-identical to today's outputs across the 64-seed harness.
2. **Weight resolution** — apply `seasonWeightMultipliers` → `droughtWeightMultipliers` → skill multiplier in one documented order; clamp at ≥ 0. Tests: monotonicity, zero-weight exclusion, ordinal determinism.
3. **Data** — add fields to the three flagship sites (+ any others the owner lists) in `micro_locations.json`; `--data-integrity-selftest` must stay at 0 errors.
4. **Host wiring** — `ExpeditionEncounterBridge` populates context from `CurrentSeason` (already flows), `WeatherSystem`/economy drought signal, and roster skill; document each source.
5. **Docs** — update the seasonal/drought/skill sections in `docs/discovery/MICRO_LOCATION_{GREENHOUSE,WATER,RADIO}.md` from "deferred" to "implemented via context".

## 5. Required tests

- Neutral-context parity: `GetEligibleCandidates(legacy) ≡ GetEligibleCandidates(neutral context)`.
- Season: greenhouse weight rises in `growing_season`, falls in `deep_freeze`, per authored data only.
- Drought: water-source weight rises with severity; grant quantities and one-shot semantics unchanged (`MicroLocationWaterIntegrationTests` stay green untouched).
- Skill: tower weight responds to the affinity skill; absent skill ⇒ multiplier 1.
- Determinism: 100-seed harness zero divergence with contexts applied; no new RNG draw count.
- Integrity: `--data-integrity-selftest` 0 errors; full `dotnet test` green.

## 6. Out of scope (explicitly)

- Changing any authored grant quantity or one-shot semantics.
- Seasonal seed pools (F18 §8.11 — separate decision).
- Reading selection context inside any downstream subsystem.
- Any new event bus topics (host wiring stays direct calls per EVENT SYSTEM rule).
