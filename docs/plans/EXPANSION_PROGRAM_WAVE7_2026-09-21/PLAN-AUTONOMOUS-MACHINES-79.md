# PLAN-AUTONOMOUS-MACHINES-79 — Drones, Crawlers, Automata & Remote Operators

**Wave 7 · Kind:** MAJOR EXPANSION · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-ENERGY-NUCLEAR-48, PLAN-ELECTRONICS-COMPUTING-65,
PLAN-TRANSPORT-EXPEDITION-30.
**Implementation scaffold:** [`PLAN-AUTONOMOUS-MACHINES-79_APPENDIX-A_SCAFFOLD.md`](PLAN-AUTONOMOUS-MACHINES-79_APPENDIX-A_SCAFFOLD.md) — source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no real drone/weapon specs, no combat AI rewrite — this adds
**machines the player owns, fuels, maintains, and loses**.

## Outcome
The corpus already contains the machines: `ArmoredCrawlerExpeditionSystem` +
modules, `AmphibiousDraisineEngine`, `AviationSystem`, `AerialReconWindowEngine`,
`SkyDefenseBatterySystem`, `MineClearingFlailEngine`, `RadarEcmCatalog`,
`RunFlatTireEngine`, `ReconTelemetrySystem`, plus power/electronics owners.
Nothing coordinates them as an **automation portfolio**.

| Machine | Authority | Player action | Outcome |
|---|---|---|---|
| Recon drone | recon system + aviation windows | survey, mark | map intel, exposure risk |
| Cargo crawler | crawler + transport | haul, escort | cargo moved, breakdown risk |
| Sentry platform | sky defense + sensors | post, arm | area denial, false alarms |
| Utility automaton | electronics + driveline | assign task | labour saved, upkeep cost |
| Remote operator | comms array + control restoration | pilot remotely | skilled play, signal loss |
| Recovery rig | draisine/flail | clear, recover | route opened, machine wear |
| Fleet depot | maintenance + power | service, charge | uptime, spares |

## Evidence
- Core: `Expeditions/ArmoredCrawlerExpeditionSystem.cs`, `ArmoredCrawlerModuleCatalog`, `AmphibiousDraisineEngine`, `AviationSystem`, `AerialReconWindowEngine`, `MineClearingFlailEngine`, `RunFlatTireEngine`, `SkyDefense/*`, `ReconTelemetrySystem`.
- Data: `armored_crawler_modules.json`, `aircraft_parts.json`, `rail_grinding_catalog.json`, `sky_defense` catalogs, `recon_telemetry_probes.json`.
- Sealed prior: CF-P6 vehicle armor grades, `--advanced-industrial-recon-selftest`, `--vehicle-garage-selftest` 19/19, Plan 139/141 recon tech.
- Contracts: one vehicle/power authority; machines consume fuel/power/spares; no free automation.

## Packages
- **AM-79A** machine registry: same vehicle/equipment owners; each machine has condition, fuel/power, skill requirement.
- **AM-79B** drone recon loop: launch window, path, intel output (PLAN 49 fusion), loss/crash consequences.
- **AM-79C** crawler logistics: hauling contracts, breakdowns, recovery missions.
- **AM-79D** sentry platforms: placement, rules of engagement, false alarms, ammo draw.
- **AM-79E** utility automata: task assignment, upkeep, autonomy failure (they stop, they don't rampage).
- **AM-79F** remote piloting: control link quality, jamming (Plan 49), operator skill.
- **AM-79G** depot: spares inventory, service queue, charge scheduling (Power Board tie-in).
- **AM-79H** content volumes: +6 machines, +10 modules, +8 faults, +6 contracts; abstract/fictional.

## Acceptance & verification
- Machines never duplicate the vehicle/power stores; failure modes recoverable; determinism.
- `godot --headless --path . -- --advanced-industrial-recon-selftest`; `--vehicle-garage-selftest`; power suites.

## Risks
Automation deleting the survival loop → upkeep/fuel/skill costs keep machines a commitment, not a replacement.

---

## 6. Expanded census (2 files · 468 lines)

Scope: `Assets/Ashfall.Core/Crafting/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `AdvancedMachineContracts.cs` | 101 | Support | **yes** | 0 | 0 | 0 |
| `RoboticsSystem.cs` | 367 | System | — | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `robotics.json` | object[2 keys] |
| `shelter_machine_identities.json` | object[5 keys] |
| `drone_carrier_blackboxes.json` | array[8] |

**State surfaces:** `RoboticsSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Crafting/` |
| Test references | 3 name references across the test tree |
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

Domain files: 2. Other plans referencing their names: **2**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-ADVANCED-MACHINERY-CONTRACTS-TRUTH-140` | 1 |
| `PLAN-TRIO-FAMILY-TRUTH-280` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `AM-79A` | `AdvancedMachineContracts.cs` |
| `AM-79B` | no name match — resolve at claim time |
| `AM-79C` | `AdvancedMachineContracts.cs` |
| `AM-79D` | no name match — resolve at claim time |
| `AM-79E` | no name match — resolve at claim time |
| `AM-79F` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 11. Host files: **12** · Test files: **7** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 12 | `src/Audio/AudioCueCatalog.cs`, `src/Host/HostCli.cs`, `src/Host/RoboticsSaveStore.cs`, `src/Main.AdvancedShelterSystems.cs`, `src/Main.Lifecycle.cs` |
| Tests (`Ashfall.Core.Tests/`) | 7 | `Ashfall.Core.Tests/Crafting/RoboticsSystemTests.cs`, `Ashfall.Core.Tests/Expeditions/AviationSystemTests.cs`, `Ashfall.Core.Tests/FlagshipIntegrationIxSmokeTests.cs`, `Ashfall.Core.Tests/Integration/Plans182_185_CampaignContinuityTests.cs`, `Ashfall.Core.Tests/Integration/Plans198_201_LateGameSystemsIntegrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/shelter_machine_identities.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **26** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `armored_crawlers` |
| `aviation` |
| `campaign` |
| `campaign_day` |
| `chemical_recon` |
| `expanded_shelter` |
| `expedition` |
| `expedition_stealth` |
| `nuclear_core_lifecycle` |
| `rail_grinding` |
| `recon_telemetry` |
| `robotics` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **37** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--7-day-smoke-selftest` |
| `--advanced-industrial-recon-selftest` |
| `--audio-selftest` |
| `--audio-test` |
| `--campaign-journey-selftest` |
| `--deterministic-smoke-selftest` |
| `--expedition-encounter-bridge-selftest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--expedition-playtest-selftest` |
| `--expedition-selftest` |
| `--late-tech-mobility-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **16**.

| Event | First declaration |
|---|---|
| `OnCarrierHeard` | `Assets/Ashfall.Core/Verdict/ReckoningSystem.cs` |
| `OnChapterAdvanced` | `Assets/Ashfall.Core/Legacy/GenerationalSuccessionEngine.cs` |
| `OnDayAdvanced` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` |
| `OnExpeditionCompleted` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnExpeditionFailed` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnExpeditionStarted` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnExpeditionTick` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnQuestStageAdvanced` | `Assets/Ashfall.Core/DutyRoster/DutyRosterQuestRuntime.cs` |
| `OnShelterEncounterResolved` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterEncounterStarted` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterFalseAlarm` | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` |
| `OnSmokeZoneChanged` | `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **2**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/robotics.json` |
| `Assets/StreamingAssets/Data/shelter_machine_identities.json` |

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

Host files (`src/`) whose names share a domain token: **5**
(1 of them panels/HUD).

| Host file |
|---|
| `src/Host/HostCli.AdvancedIndustrialRecon.cs` |
| `src/Host/HostSessionContracts.cs` |
| `src/Host/RoboticsSaveStore.cs` |
| `src/Main.AdvancedShelterSystems.cs` |
| `src/UI/RoboticsWorkshopPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **1**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `robotics` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `advanced_mfg_ebpvd_coating` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **2**
(GAMEPLAY_CONSUMED 1, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `robotics.json` | GAMEPLAY_CONSUMED |
| `shelter_machine_identities.json` | UNRESOLVED |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 2
**Surface:** save sections 1 (laddered 0) · RNG streams 1 · host files 6 · catalogs 4 · test regions 0 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-AUTONOMOUS-MACHINES-79
wave: 7
status: PROPOSED — foreman claim required
packages: AM-79A, AM-79B, AM-79C, AM-79D, AM-79E, AM-79F, AM-79G, AM-79H
claim paths:
  - src/Host/HostCli.AdvancedIndustrialRecon.cs  # §19 candidate host surface
  - src/Host/HostSessionContracts.cs  # §19 candidate host surface
  - src/Host/RoboticsSaveStore.cs  # §19 candidate host surface
  - src/Main.AdvancedShelterSystems.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/robotics.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/shelter_machine_identities.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --7-day-smoke-selftest
dependencies:
  - coordinate: 2 other plan(s) name these artifacts (§12)
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
