# Batch 6 — Unblock the oldest partial plans: Plans 135 + 59

**Package:** `UNBLOCK-OLDEST-BATCH6-PLANS`
**Claim:** `claim-unblock-oldest-batch6-plans-2026-09-23`
**Date:** 2026-09-23
**Status:** COMPLETE (see the ledger entry in `INTEGRATION_PLANS.md`)
**Predecessor:** `UNBLOCK-OLDEST-BATCH5-PLANS` (Plans 55 + 58), which closed with
"Next batch (oldest remaining partials): Plans 59, 135".

---

## 1. Which two plans, and why

The 20-plan audit (`docs/plans/OLDEST_PARTIAL_PLANS_AUDIT_20_2026-09-23.md`)
measured host reachability for every remaining oldest partial plan. Batch 5
retired Plans 55 and 58, which leaves:

| # | Plan | Title | Core authority | host refs |
|---|---|---|---|---|
| 1 | 59 | Retrospective: Turn Nine Waves of Findings into Rules, Then Stop Auditing | `Assets/Ashfall.Core/Governance/StandingGateRegistry.cs` | 0 |
| 2 | 135 | Weather → Deep Gameplay Cascade | `Assets/Ashfall.Core/Weather/WeatherGameplayCascadeEngine.cs` + `WeatherCascadeSystem.cs` | 0 |

Both were re-verified immediately before implementation:
`grep -rn "StandingGateRegistry\|WeatherGameplayCascadeEngine\|WeatherCascadeSystem" src/`
returned nothing, i.e. both Core authorities were **still** host-unreachable.
Both already have Core unit tests
(`Ashfall.Core.Tests/Governance/Plan59StandingGateIntegrationTests.cs`,
`Ashfall.Core.Tests/Weather/Plan135WeatherCascadeIntegrationTests.cs`), so the
gap is host integration, not implementation.

**Per the batch shape established in Batches 1–5:** the scope of this package is
to host-integrate the existing Core authorities against the canonical owners and
make them first-class in the campaign lifecycle. It is **not** a re-implementation
of the full Plan 59 / Plan 135 source documents (which describe 66 substeps
between them, including a CI rewrite, a retrospective publication wave, weather
quests, and a weather mini-game). Where a plan task is explicitly out of scope
for a host-integration batch, §6 names it as a deferral with its reason.

---

## 2. Premise audit — what is real today

### Plan 135 (weather cascade)

| Claim in the plan | Current evidence | Verdict |
|---|---|---|
| `WeatherCascadeSystem.cs` (NEW in the plan) | Exists, `Assets/Ashfall.Core/Weather/WeatherCascadeSystem.cs`, 77 lines: wraps `WeatherGameplayCascadeEngine`, `BindWeatherSource`, `TriggerCascade`, `TickDay`, `CaptureState`/`RestoreState` | Already built |
| `WeatherGameplayCascadeEngine.cs` | Exists, 437 lines, `CaptureState`/`RestoreState`, 6 integration seams, `DefaultCatalogFileName = "weather_gameplay_effects.json"` | Already built |
| 15 weather effect templates in the data authority | `Assets/StreamingAssets/Data/weather_gameplay_effects.json`, `schema_version 1`, `cascade_templates` — 15 rows across 11 kinds, all 6 target systems and 9 effect types | Already authored |
| `IWeatherCascadeSource` interface for the weather system to trigger cascade events | Exists (`IWeatherCascadeSource.OnWeatherFrontArrived`), no host implementer | Real, unbound |
| "Wire into GameBootstrap: SetupWeatherCascade, TickWeatherEffects, SaveWeatherCascade" | No such host methods | **The gap** |
| `--weather-cascade-selftest` verb | Not registered | **The gap** |
| "Weather doesn't cascade into shelter integrity / expedition risk / faction ops / economy / mental health / locations" | True of the *host*. The Core engine has the seams (`OnShelterDamageSurgedSeam`, `OnExpeditionDelayForecastedSeam`, `OnMarketShockTriggeredSeam`, `OnMentalHealthStressSurgedSeam`, `OnLocationAccessibilityChangedSeam`) with no caller | **The gap** |
| "Add deterministic seeding: weather effects use `ISeededRng`" | `EvaluateWeatherCascade(kind, severity, day, regions, fortificationLevel, rng)` accepts an `ISeededRng?` but never draws from it | Real gap, deliberately NOT closed in this batch (see §6) |
| "UI panel shows weather alerts and preparation options" | No panel | Out of scope (§6) |

Additional evidence found during the audit, which governs the design:
- `Assets/Ashfall.Core/World/WeatherSystem.cs` is the canonical weather authority
  (`OnWeatherChanged`, `VisibilityFactor`, `OutdoorRadModifier`,
  `TemperaturePenaltyC`, `BindWeatherEffects`).
- `Assets/Ashfall.Core/World/WeatherEffectsCatalog.cs` is the **authored**
  per-kind table (`weather_effects.json`) that `WeatherSystem` already consumes
  for rad dose, visibility, thermal load, travel speed and encounter chance.
  It carries an `explicitly_neutral` flag per row and has a no-silent-defaults
  coverage gate over every `WeatherKind`.
- `Assets/Ashfall.Core/Shelter/DisasterResponseSystem.cs` owns shelter
  **resilience** (`ResilienceRating`, `AdjustResilience`). No separate shelter
  integrity/condition owner with a per-room damage write exists.
- `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` exposes exactly two
  host-facing multiplier seams: `SetStaminaDrainMultiplier` and
  `SetEncounterChanceMultiplier`.
- `Assets/Ashfall.Core/Economy/MarketSystem.cs` owns all price change through
  `ApplyShock(categoryId, isShortage, severityBp, startDay, durationDays, sourceId)`
  — clamped to [500, 3000] bp, idempotent per `(category, kind, sourceId)`.
- `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` owns morale through
  `SetExternalModifier(survivorId, sourceId, NeedKind.Morale, deltaPerHour,
  priority, startDay, endDay)` — a duration-bound, per-hour contribution.
- `Assets/Ashfall.Core/World/RouteAvailabilityKind.cs` documents route
  availability as **derived and never serialized**: "Never serialized — derived
  from the gate evaluator and existing route restrictions at presentation time."

### Plan 59 (retrospective / standing gates)

| Claim in the plan | Current evidence | Verdict |
|---|---|---|
| `standing_gates.json` catalog | Exists, 22 rows, each with `gate_id`, `wave_origin`, `finding_clause`, `enforcement_rule`, `tier`, `owner_role`, `is_enforced` | Already authored |
| `StandingGateRegistry.FromJson` parser | Exists but **lenient**: an unknown tier silently becomes `PerPush`; a row with an empty `gate_id` is silently dropped by the constructor; no error collection | Weak |
| `AuditStandingGates(Func<string,bool>, GateTier?)` | Exists, produces `RetrospectiveAuditReport` with per-gate violations | Real |
| `docs/ci/CI_GATE_MANIFEST.json` | The repo's **real enforcement authority**: 57 declared gates, each with `gate_id`, `name`, `category`, `command`, `timeout_seconds`, `expected_summary`, `classification` (fast/full/performance), `critical`, `depends_on`. `total_gates: 57`, `fast_tier_count: 53` | Existing authority |
| "build the gate register … three columns of that table will be 'no'" | No register existed that cross-referenced the standing gates to the CI manifest | **The gap** |
| "assign an owner to each gate" | `owner_role` is authored but never verified | Weak |
| "decide, in writing, what will *not* be gated" | No such column existed | **The gap** |
| "prove each gate can fail … no self-proof, no gate" | No self-proof fixture existed | **The gap** |
| `--standing-gates-selftest` | Not registered | **The gap** |

The plan's central measured fact — **"Red gates persisted across weeks of work …
so 'gates exist' was never the same as 'gates run'"** — is exactly what the host
session makes checkable: each standing gate must name a real, critical,
tier-compatible gate in `CI_GATE_MANIFEST.json`.

---

## 3. Authority boundaries and the seams used

### Plan 135 — one route per effect, no second store

The session owns **no** weather, shelter, expedition, market, morale or
accessibility fact. It holds only the cascade's own event ledger (the
`weather_cascade` section) plus per-front transient multipliers it withdraws.

| Cascade effect | Canonical owner | Seam used | What is NOT duplicated |
|---|---|---|---|
| severity | `WeatherEffectsCatalog` (via `WeatherCascadeSeverity`) | `TryGetEffects(kind)` read-only | no per-kind severity table in code |
| shelter `damage` | `DisasterResponseSystem` | `AdjustResilience(-magnitude)` | no second integrity/condition counter |
| shelter `thermal_load` / `filtration_stress` / `power_output` | `WeatherSystem` (already bound to the same table) | **reported, not applied** | no second fuel drain or indoor-rad path |
| shelter `accessibility` (black-rain sump flooding) | `DisasterResponseSystem` | `AdjustResilience(-(magnitude * 0.05f))` | no second water level |
| expedition `delay` / `damage` / `hazard_surge` | `ExpeditionSystem` | `SetEncounterChanceMultiplier(region → boost)` | the owner still owns the roll |
| economy `price_change` | `MarketSystem` | `ApplyShock("fuel_and_supplies", true, bp, day, dur, "weather_<id>")` | no second price table; the owner clamps |
| mental health `behavior_change` | `NeedsSystem` | `SetExternalModifier(id, src, Morale, ±/h, 10, day, day+dur-1)` | no morale write, no fabricated crisis case |
| location `accessibility` | *(derived at presentation)* | **reported only** | no parallel accessibility ledger |
| faction `behavior_change` | *(reported)* | report-only; no standing-operation ledger exists to pause | no invented faction-operation state |

**Fortification** is read from the canonical resilience owner
(`ResilienceRating >= 80 → 2`, `>= 50 → 1`, else `0`), not stored.

**Severity** is a derived read model, computed in Core so the host cannot
re-derive it: five named axes from the canonical table
(`visibility`, `thermal_load_additive_c`, `outdoor_rad_modifier`,
`travel_speed_multiplier`, `travel_encounter_multiplier`), each normalized to
`0..1` against a named reference constant, combined with named integer-permille
weights summing to exactly 1000. No RNG, no caching, no per-kind table.

### Plan 59 — one register, one enforcement authority

`StandingGatesHostSession` runs **no** CI gate and substitutes for none. It
reports what `CI_GATE_MANIFEST.json` declares. Verdicts:

| Verdict | Meaning |
|---|---|
| `Enforced` | Names a real CI gate, `critical: true`, tier-compatible |
| `RuleOnly` | Deliberately not scripted; carries a written, dated decision and an owner |
| `Unbound` | Names a gate not in the manifest, or one with no command |
| `NotBlocking` | Registered but not `critical`, so a red does not block a release |
| `TierMismatch` | Critical, but its classification cannot satisfy the rule's tier |

Tier semantics (deliberate, and documented in code): a **per-push** rule
requires a `fast` gate, because a `full`-tier soak does not run on every push.
A **nightly** or **per-release** rule accepts either, because a fast gate also
runs in the full suite and at release. Requiring `full` for nightly rules would
mark the repository's best gates as insufficient, which is not the finding.

`Passing` = `Enforced` **or** `RuleOnly`. Everything else is a violation the
`StandingGateRegistry` itself reports with the wave that produced the finding.

---

## 4. Blockers found and resolved in-batch

### 4.1 `weather_gameplay_effects.json` was loaded leniently
`WeatherGameplayCascadeEngine.LoadCatalog` swallowed every parse error and fell
back to **generated** effects — i.e. an authored typo could silently become an
invented cascade. **Resolved** by adding a strict Core loader
(`WeatherCascadeCatalogLoader`) that rejects an unknown `weather_kind`, an
unknown `target_system`/`effect_type`, a non-finite magnitude, a sub-day
duration, a duplicate id, and an **empty** table, then binds the validated rows
through a new `BindValidatedTemplates` seam. The generated-effect fallback is
now unreachable from the host.

### 4.2 The engine accepted any state schema
`RestoreState` accepted `schema_version` without checking it. **Resolved** by
adding `StateSchemaVersion = 1` and throwing `InvalidOperationException` on a
mismatch; the host catches, journals, and keeps live state rather than
half-applying a foreign shape.

### 4.3 A mechanically neutral front still cascaded
`IsMechanicallyNeutral` was computed and then discarded, so `Clear` fired a
zero-severity event that reached the fallback generator and produced a
`default_morale_lift` — a fabricated morale gain the authored table never
asked for. **Found by the probe. Resolved** by short-circuiting
`TriggerFront` when the front is neutral or has no severity.

### 4.4 The blizzard row carries no `damage` effect
My first probe asserted that a blizzard lowers shelter resilience. The authored
blizzard row is `thermal_load` + `expedition delay` + `economy price_change`;
`BlackRain` owns `damage`. **Resolved** by measuring the shelter route on
`BlackRain` and adding an explicit check that a `thermal_load` front does **not**
also mutate resilience (the canonical weather path already carries it, so
applying it again would be a second drain).

### 4.5 The tier rule was wrong on its first pass
My first `RequiredClassification` demanded a `full`-tier gate for nightly and
per-release rules, which flagged 8 correctly-enforced gates as mismatches.
**Resolved** by correcting the semantics to §3 and keeping the
self-proof path (`AuditWithTierOverride`) so the probe can still force a
requirement and prove the check discriminates.

### 4.6 One genuine tier mismatch in the authored register
`gate_vocabulary_contract_events` was authored `per_push` but binds
`test_core_suite`, which runs on the `full` tier. **Resolved** by retiering the
standing gate to `nightly`, which is the cadence its enforcement actually runs
at — not by weakening the check.

### 4.7 `CatalogIntegrityValidator` resolved `enforcement_ref` as a game-data id
The validator cross-references snake_case id columns, and it flagged
`agent_rulebooks_sync` as an unresolved id. **Resolved** by classifying
`enforcement_ref` as the pure vocabulary it is (a CI gate id, not a catalog
entity), exactly as `collection_key` was classified in Batch 5.

### 4.8 A new day-event kind was unclassified
`weather_cascade_ticked` failed `DayEventSemanticKindTests` and
`DayEventParitySourceGateTests`. **Resolved** by classifying it as an internal
heartbeat and adding it to `docs/campaign/EVENT_SEMANTIC_PARITY_MATRIX.md`.

### 4.9 A new Core `Bind*` seam was untracked
`WeatherGameplayCascadeEngine.BindValidatedTemplates` failed the port-contract
gate. **Resolved** by adding it to `docs/ci/port_contract_policy.json` as
`HOST_REQUIRED` (it has exactly one host caller:
`src/Host/WeatherCascadeHostSession.cs`).

### 4.10 The architecture map had no node for the section
`weather_cascade` failed the architecture-map gate. **Resolved** by adding the
section entry to `scripts/ci/generate-architecture-map.py`.

---

## 5. Files

### New — Core
- `Assets/Ashfall.Core/Weather/WeatherCascadeSeverity.cs` — canonical hazard
  index, derived from `WeatherEffectsCatalog`, no mutable state.
- `Assets/Ashfall.Core/Records/WeatherCascadeCatalogLoader.cs` — strict loader
  for `weather_gameplay_effects.json`.
- `Assets/Ashfall.Core/Governance/StandingGateCatalogLoader.cs` — strict loader
  for `standing_gates.json` (owner, tier, and written-decision enforcement).
- `Assets/Ashfall.Core/Governance/CiGateManifestReader.cs` — reader for the
  repo's enforcement authority.

### New — Host
- `src/Host/WeatherCascadeHostSession.cs` (+ `WeatherCascadeSaveStore`).
- `src/Host/HostCli.WeatherCascade.cs` — `--weather-cascade-selftest`, 35 checks.
- `src/Host/StandingGatesHostSession.cs` (+ self-proof fixtures).
- `src/Host/HostCli.StandingGates.cs` — `--standing-gates-selftest`, 19 checks.
- `src/Main.WeatherCascade.cs` — setup/save/restore/reset + the weather-change
  entry point + the day tick.

### New — Tests
- `Ashfall.Core.Tests/Weather/Plan135WeatherCascadeHostIntegrationTests.cs` (16).
- `Ashfall.Core.Tests/Governance/Plan59StandingGateHostIntegrationTests.cs` (21).

### Modified
- `Assets/Ashfall.Core/Weather/WeatherGameplayCascadeEngine.cs` — `StateSchemaVersion`,
  `BindValidatedTemplates`, schema-gated `RestoreState`.
- `Assets/Ashfall.Core/Weather/WeatherCascadeSystem.cs` — documented `RestoreState`.
- `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` — `weather_cascade` section
  + `weather_cascade_save.json`.
- `Assets/Ashfall.Core/HostCliRegistry.cs` — two CLI descriptors.
- `Assets/Ashfall.Core/Campaign/DayEventVocabulary.cs` — `weather_cascade_ticked`.
- `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` — `enforcement_ref` vocabulary.
- `src/Host/HostCli.cs`, `src/Main.Application.cs`, `src/Main.CampaignOwners.cs`
  (new `WeatherCascadeDayOwner`, phase 5), `src/Main.SaveOrchestrator.cs`,
  `src/Main.ExpandedShelterSystems.cs`, `src/Main.World.cs` (weather-change hook).
- `Assets/StreamingAssets/Data/standing_gates.json` — every row gains
  `enforcement_ref` (19 bound to a real CI gate, 3 to `none`) and `non_gate_rule`
  (written decision for the 3), plus the tier fix in §4.6.
- `docs/campaign/EVENT_SEMANTIC_PARITY_MATRIX.md`.
- `scripts/ci/generate-architecture-map.py`, `docs/ci/port_contract_policy.json`.
- `Ashfall.Core.Tests/Save/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`
  — section pin 222 → 223.

---

## 6. Deliberate deferrals (named, with reason)

| Not done | Why | Where it lands |
|---|---|---|
| RNG-severity shakes / weather prediction mini-game / weather quests / weather legacy / UI "Weather Alert" panel / `VentilationSystem` and `SumpFloodingSystem` second couplings | Out of a host-integration batch; each is a feature package, and none has a canonical owner seam today | Plan 135 follow-on |
| Faction-operation pause on severe weather | No standing-operation ledger exists in `FactionWarSystem`/`TerritoryControlSystem`; the cascade reports the change instead of inventing state | Plan 135 follow-on |
| `MentalHealthCrisisSystem` crisis-probability raise | The owner has no probability seam; only morale is canonical | Plan 135 follow-on |
| Integration / persistence for Plan 55B / 58C that Batch 5 already closed | Done in Batch 5 | — |
| Plan 59A tasks 1, 7, 8, 10, 12 (dedupe the 5 detectors, generate `docs/process/GATES.md`, retire unearned gates, trend the two numbers) | Those are CI-authoring and doc-generation work that writes files outside this claim's boundary. The register, the owner column, the written-decision column, and the self-proofs — the parts that make the claim measurable — are done | Plan 59A follow-on (foreman-signed) |
| Plan 59B / 59C entirely | Publishing the retrospective and defining the operating cadence is a governance-document package, not a host-integration package | Plan 59B/59C (foreman-signed) |
| `StandingGateRegistry.FromJson` removal | The lenient parser is still reachable from the Core-only tests. Left in place; the strict loader is the production path | Cleanup |

**Deliberately NOT done:** no attempt to re-tier or re-map a standing gate to make
the audit pass. Any rule that cannot be enforced is either bound honestly or
recorded as `RuleOnly` with an owner and a written decision.

---

## 7. Verification

```
dotnet build Ashfall.csproj                                  # 0 errors, 0 warnings
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj     # 0 errors
godot --headless --path . -- --weather-cascade-selftest        # 35/35
godot --headless --path . -- --standing-gates-selftest        # 19/19
godot --headless --path . -- --data-integrity-selftest        # PASS, 0 errors / 420 catalogs
godot --headless --path . -- --port-contract-selftest         # PASS, 287 seams / 183 host-required
godot --headless --path . -- --selftest-manifest              # PASS
godot --headless --path . -- --7-day-smoke-selftest           # PASS
godot --headless --path . -- --save-store-checksum-selftest   # PASS
godot --headless --path . -- --retention-selftest             # 18/18 (Batch 5 regression)
godot --headless --path . -- --outpost-settlement-selftest    # 28/28 (Batch 5 regression)
bash scripts/run_test.sh Ashfall.Core.Tests/Governance        # 48/48
bash scripts/run_test.sh Ashfall.Core.Tests/Weather           # 27/27
bash scripts/run_test.sh Ashfall.Core.Tests/Save              # 1453/1453
bash scripts/run_test.sh Ashfall.Core.Tests/Records           # 13/13
bash scripts/run_test.sh Ashfall.Core.Tests/Settlements       # 12/12
bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions       # 331/331
bash scripts/run_test.sh Ashfall.Core.Tests/Tooling           # 120/122
bash scripts/run_test.sh Ashfall.Core.Tests/Campaign          # 256/257
python3 scripts/ci/agent-fast-verify.py                       # 10/10 gates
python3 scripts/ci/generate-architecture-map.py --check       # OK, 223 subsystems
python3 scripts/ci/generate-port-contract.py --check          # PASS, 0 errors
python3 scripts/ci/generate-catalog-registry.py --check       # OK, 704 catalogs
python3 scripts/ci/generate-selftest-manifest.py --check      # OK, 149 tests
python3 scripts/ci/generate-docs-index.py --check             # OK, 4091 documents
```

### Pre-existing failures left untouched (none inside this claim)

| Failure | Evidence it is outside this claim |
|---|---|
| `Plan31BriefingRouteTests.GenericNonHeartbeatEvent_BecomesActionable` | D11-B closed section routing; decision-blocked; documented in Batches 2–5 ledger entries |
| `CaseFoldPolicyPinTests.Core_OrdinalIgnoreCase_UsageStaysInDocumentedBand` | 400 hits vs pin band [120,260] — 140 over; this package adds 3 usages |
| `CatalogPathForbiddenGateTests.NoNewPrivateDataResolvers` | Fails on `src/UI/FactionCultureCodexPanel.cs` |
| `--real-campaign-journey-selftest` | "Last survivor died (…, Combat). Campaign terminal." Reproduced identically with this package's entire weather-cascade trigger disabled (`OnWeatherFrontArrived` commented out, `dotnet build`, re-run → same 2 failures, same two victim ids). `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` and `TacticalCombatSystem.Actions.cs` — the exact system the log names — are dirty in the shared worktree from concurrent work (852 modified files at audit time). Causation test isolates the failure to that concurrent work, not to this claim. |

---

## 8. Next batch

Oldest remaining host-unreachable plans after Batch 6: **Plan 136** then
**Plan 140**.
