# PLAN-RADIO-FAMILY-TRUTH-266 — Radio Data & Support Families (Distress Excluded)

**Wave 19 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-RADIO-MEDIA-42, PLAN-RADIO-STATION-TRUTH-209, PLAN-RADIO-RECORDING-TRUTH-258.
**Non-goals:** distress files are **excluded** — `CF-P1-DISTRESS-CONTENT-SEAL`
owns that seam; this plan must not touch those paths.

## 1. Outcome
**24 `Radio/` files** are referenced by no plan; several belong to the distress
family and are excluded. The remainder — `FactionRadioTypes`, `PsyOpsCatalog`,
`PsyOpsSave`, recording/media support — needs a wiring census so the station
(Plan 209), psyops (Plan 210), and recording (Plan 258) plans have their data
paths verified.

| Deliverable | Detail |
|---|---|
| Exclusion list | distress-family files named and excluded in writing |
| Remaining census | each remaining file → loader → consumer → test |
| PsyOps data | `PsyOpsCatalog`/`PsyOpsSave` wired to Plan 210's model |
| Faction radio types | DTOs validated and used by Plans 42/209 |
| Test presence | fixtures for un-tested files |

## 2. Evidence
- 24 `Radio/` basenames absent from every plan body (Wave 19 file-level audit); the distress subset is visible in that list.
- AGENTS.md queue: `CF-P1-DISTRESS-CONTENT-SEAL` is an available plan — the exclusion respects it.
- Plans 42/209/210/258 own the systems; this plan verifies their data.

## 3. Packages
- **RDF-266A** exclusion list + remaining census.
- **RDF-266B** PsyOps data wiring tests.
- **RDF-266C** faction radio DTO validation.
- **RDF-266D** fixture pass.

## 4. Acceptance & verification
- No distress path is modified; every remaining file has a consumer or a finding.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/`.

## 5. Risks
Boundary violation → the exclusion list is the first package and the claim must carry it.
Data drift → DTO validation binds shapes to their consumers.

---

## 6. Expanded census (37 family files · 10,249 lines)

Scope: files under `Assets/Ashfall.Core/Radio/` whose basename is referenced
by no plan body (the Wave 19 family definition). Class distribution: Support 15 · System 11 · Catalog 7 · Save 2 · DTO/Type 1 · Loader 1.

| File | Lines | Class | Banned refs | Empty catches | Capture/Restore |
|---|---:|---|---:|---:|---:|
| `AcousticDirectionFindingCatalog.cs` | 137 | Catalog | 0 | 0 | 0 |
| `DirectionFindingCatalog.cs` | 248 | Catalog | 0 | 0 | 0 |
| `DistressAudioCueResolver.cs` | 60 | Support | 0 | 0 | 0 |
| `DistressDestinationResolver.cs` | 418 | Support | 0 | 0 | 0 |
| `DistressFollowUpScheduler.cs` | 366 | Support | 0 | 0 | 2 |
| `DistressRescueMissionManager.cs` | 911 | System | 0 | 0 | 2 |
| `DistressStageResolver.cs` | 93 | Support | 0 | 0 | 0 |
| `FactionRadioEngine.cs` | 244 | System | 0 | 0 | 0 |
| `FactionRadioTypes.cs` | 82 | DTO/Type | 0 | 0 | 0 |
| `NvisCommunicationsSystem.cs` | 295 | System | 0 | 0 | 2 |
| `PatrolRadioHooks.cs` | 201 | Support | 0 | 0 | 2 |
| `PsyOpsCatalog.cs` | 69 | Catalog | 0 | 0 | 0 |
| `PsyOpsSave.cs` | 166 | Save | 0 | 0 | 0 |
| `PsyOpsSystem.cs` | 418 | System | 0 | 0 | 2 |
| `RadioBroadcastCatalog.cs` | 679 | Catalog | 0 | 0 | 0 |
| `RadioBroadcastModels.cs` | 360 | Support | 0 | 0 | 0 |
| `RadioDistressSystem.cs` | 835 | System | 0 | 0 | 2 |
| `RadioProgramCatalog.cs` | 119 | Catalog | 0 | 0 | 0 |
| `RadioProgramProductionSystem.cs` | 407 | System | 0 | 0 | 2 |
| `RadioPropagation.cs` | 233 | Support | 0 | 0 | 0 |
| `RadioReceiverPlan.cs` | 120 | Support | 0 | 0 | 0 |
| `RadioRecordingSystem.cs` | 107 | System | 0 | 0 | 2 |
| `RadioSave.cs` | 467 | Save | 0 | 0 | 0 |
| `RadioScheduleCoordinator.cs` | 427 | System | 0 | 0 | 0 |
| `RadioSignalLog.cs` | 149 | Support | 0 | 0 | 4 |
| `RadioStationCatalog.cs` | 163 | Catalog | 0 | 0 | 0 |
| `RadioStationCatalogLoader.cs` | 123 | Loader | 0 | 0 | 0 |
| `RadioTuner.cs` | 257 | Support | 0 | 0 | 0 |
| `RescueDispatchPreflight.cs` | 42 | Support | 0 | 0 | 0 |
| `RescuedArcProjection.cs` | 153 | Support | 0 | 0 | 0 |
| `ShelterRadioStationSystem.cs` | 430 | System | 0 | 0 | 2 |
| `SignalAuthenticityEvaluator.cs` | 158 | Support | 0 | 0 | 0 |
| `SignalTriangulationSystem.cs` | 694 | System | 0 | 0 | 2 |
| `SignalTrustAvailability.cs` | 107 | Support | 0 | 0 | 0 |
| `SignalTrustLedger.cs` | 179 | Support | 0 | 0 | 2 |
| `UvCoronaDetectionCatalog.cs` | 127 | Catalog | 0 | 0 | 0 |
| `UvCoronaDetectionEngine.cs` | 205 | System | 0 | 0 | 2 |

**Census totals:** 0 banned nondeterministic references · 0 empty-catch sites · 13 files with capture/restore methods.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `faction_war_radio.json` | object[2 keys] |
| `radio_intercepts.json` | object[2 keys] |
| `year_of_ash_radio.json` | object[2 keys] |
| `faction_radio_corpus.json` | object[7 keys] |
| `radio_stations.json` | object[2 keys] |
| `radio.json` | object[2 keys] |

**State surfaces (capture/restore present):**

- `DistressFollowUpScheduler.cs`
- `DistressRescueMissionManager.cs`
- `NvisCommunicationsSystem.cs`
- `PatrolRadioHooks.cs`
- `PsyOpsSystem.cs`
- `RadioDistressSystem.cs`
- `RadioProgramProductionSystem.cs`
- `RadioRecordingSystem.cs`
- `RadioSignalLog.cs`
- `ShelterRadioStationSystem.cs`
- `SignalTriangulationSystem.cs`
- `SignalTrustLedger.cs`

## 8. Expanded verification

| Check | Baseline to establish at claim time |
|---|---|
| Focused region | `Ashfall.Core.Tests/Radio/` |
| Family files referenced by tests | 125 name references across the test tree |
| Determinism scan | 0 banned references to fix or justify |
| Failure scan | 0 empty-catch sites to route through Plan 35's rules |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census first (section 6) — classify every family file; no edits in this step.
2. Catalogs and loaders: prove a consumer or report the file as inert.
3. DTO/Type files: round-trip or consume-only proof; unknown values fail typed.
4. System files: confirm the single owner per state; remove duplicated stores.
5. Demo/tooling files: resolve to a real CLI verb or retire (Plan 86 pattern).
6. Regression: focused region plus this family census regenerated.

## 10. Acceptance matrix

| File class | Acceptance |
|---|---|
| Catalog | resolves through a loader; malformed fixture fails typed with the field named |
| Loader | valid/invalid fixture pair; unknown id names the field |
| DTO/Type | round-trip or consume-only proof; no orphan type |
| Save | capture/restore round-trip; key per Plan 1 Appendix Q |
| System | one owner per state; no parallel store |
| Demo | resolves to an existing verb or is retired |
| Support | consumed by a system or reported ownerless |

**Non-goals unchanged:** this expansion adds census and verification detail; it
does not widen the plan's scope or create new authorities.

---

## 12. Cross-plan coupling

This is a family-survey plan; the domain set is the plan's own `.cs` enumeration
(37 files). Other plans referencing those names: **12**.

**Incoming plan edges (top 8):**

| Plan | Family-file mentions |
|---|---:|
| `PLAN-RADIO-MEDIA-42` | 21 |
| `PLAN-SIGNALS-REMOTE-SENSING-49` | 5 |
| `PLAN-RADIO-STATION-TRUTH-209` | 5 |
| `PLAN-HELIOGRAPH-TRUTH-235` | 5 |
| `PLAN-PSYOPS-TRUTH-210` | 3 |
| `EVIDENCE` | 2 |
| `PLAN-UV-CORONA-DETECTION-TRUTH-250` | 2 |
| `PLAN-ORPHAN-SEAL-01` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `RDF-266A` | no name match — resolve at claim time |
| `RDF-266B` | `PsyOpsCatalog.cs`, `PsyOpsSave.cs`, `PsyOpsSystem.cs` |
| `RDF-266C` | `FactionRadioEngine.cs`, `FactionRadioTypes.cs`, `PatrolRadioHooks.cs` |
| `RDF-266D` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidate files are a starting point for the touch map, not a decision.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 37; intra-domain edges: **30**; isolated files:
**13**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `DistressAudioCueResolver` | `DistressStageResolver` |
| `DistressDestinationResolver` | `RadioDistressSystem` |
| `DistressFollowUpScheduler` | `DistressRescueMissionManager` |
| `DistressFollowUpScheduler` | `RadioDistressSystem` |
| `DistressRescueMissionManager` | `DistressDestinationResolver` |
| `DistressRescueMissionManager` | `RadioDistressSystem` |
| `DistressRescueMissionManager` | `RescueDispatchPreflight` |
| `DistressRescueMissionManager` | `SignalAuthenticityEvaluator` |
| `DistressRescueMissionManager` | `SignalTrustLedger` |
| `DistressStageResolver` | `RadioPropagation` |
| `DistressStageResolver` | `RadioTuner` |
| `PsyOpsCatalog` | `PsyOpsSystem` |
| `RadioBroadcastCatalog` | `FactionRadioEngine` |
| `RadioBroadcastCatalog` | `RadioScheduleCoordinator` |
| `RadioBroadcastCatalog` | `RadioStationCatalog` |
| `RadioDistressSystem` | `DistressFollowUpScheduler` |
| `RadioDistressSystem` | `SignalTrustLedger` |
| `RadioProgramProductionSystem` | `PsyOpsSystem` |
| `RadioProgramProductionSystem` | `RadioProgramCatalog` |
| `RadioProgramProductionSystem` | `RadioStationCatalog` |
| `RadioPropagation` | `DistressStageResolver` |
| `RadioScheduleCoordinator` | `RadioBroadcastCatalog` |
| `RadioScheduleCoordinator` | `RadioStationCatalog` |
| `RadioStationCatalog` | `RadioStationCatalogLoader` |
| `RadioStationCatalogLoader` | `RadioStationCatalog` |
| `RadioTuner` | `DistressStageResolver` |
| `RadioTuner` | `RadioDistressSystem` |
| `SignalTriangulationSystem` | `DirectionFindingCatalog` |
| `SignalTrustLedger` | `DistressRescueMissionManager` |
| `UvCoronaDetectionEngine` | `UvCoronaDetectionCatalog` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `RadioDistressSystem` | 4 |
| `RadioStationCatalog` | 4 |
| `DistressStageResolver` | 3 |
| `DistressRescueMissionManager` | 2 |
| `PsyOpsSystem` | 2 |
| `SignalTrustLedger` | 2 |
| `DirectionFindingCatalog` | 1 |
| `DistressDestinationResolver` | 1 |
| `DistressFollowUpScheduler` | 1 |
| `FactionRadioEngine` | 1 |

**Class split:** hub 12 · sink 7 · source 5 · isolated 13.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 37. Host files: **25** · Test files: **64** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 25 | `src/Audio/AudioSelfTest.cs`, `src/Audio/ShelterOperationsAudioBridge.cs`, `src/Host/HostCli.AdvancedIndustrialRecon.cs`, `src/Host/HostCli.NpcArcSelfTest.cs`, `src/Host/HostCli.PanelTests.cs` |
| Tests (`Ashfall.Core.Tests/`) | 64 | `Ashfall.Core.Tests/ApicultureAndTriangulationIntegrationTests.cs`, `Ashfall.Core.Tests/Balance/ShelterOperationsBalanceSim.cs`, `Ashfall.Core.Tests/Campaign/Plan33_38IntelCalendarIntegrationTests.cs`, `Ashfall.Core.Tests/Economy/TradeScreenPresenterSnapshotTests.cs`, `Ashfall.Core.Tests/FactionRadioBroadcastExpansionTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **23** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `dose_ledger` |
| `expanded_shelter` |
| `faction_espionage` |
| `nvis_communications` |
| `pneumatic_dispatch` |
| `radio` |
| `radio_program_production` |
| `radio_station` |
| `schedule` |
| `shelter` |
| `shelter_assignment` |
| `shelter_atmosphere` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **23** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--audio-selftest` |
| `--audio-test` |
| `--direction-finding-selftest` |
| `--dose-ledger-selftest` |
| `--faction-communique-board-selftest` |
| `--faction-ecology-selftest` |
| `--ledger-debt-selftest` |
| `--patrol-encounter-selftest` |
| `--radio-catalog-selftest` |
| `--radio-selftest` |
| `--shelter-actor-physics-selftest` |
| `--shelter-atmosphere-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **21**.

| Event | First declaration |
|---|---|
| `OnBroadcast` | `Assets/Ashfall.Core/Muster/ColdCountSystem.cs` |
| `OnCulturalBroadcast` | `Assets/Ashfall.Core/VinylMoraleSystem.cs` |
| `OnFactionStandingChanged` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` |
| `OnFactionSuccession` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnFactionSurrender` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnLedgerCalibrated` | `Assets/Ashfall.Core/DoseLedgerSystem.cs` |
| `OnLedgerTampered` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnProductionCompleted` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnProductionTick` | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` |
| `OnQuestStageAdvanced` | `Assets/Ashfall.Core/DutyRoster/DutyRosterQuestRuntime.cs` |
| `OnQuestStageChanged` | `Assets/Ashfall.Core/HoldfastQuestSystem.cs` |
| `OnScheduleChanged` | `Assets/Ashfall.Core/ShelterScheduleSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/acoustic_triangulation_catalog.json` |
| `Assets/StreamingAssets/Data/audio_accessibility_cues.json` |
| `Assets/StreamingAssets/Data/audio_cues.json` |
| `Assets/StreamingAssets/Data/audio_logs_expansion_05.json` |
| `Assets/StreamingAssets/Data/communications_networks.json` |
| `Assets/StreamingAssets/Data/direction_finding_catalog.json` |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |
| `Assets/StreamingAssets/Data/faction_intelligence.json` |
| `Assets/StreamingAssets/Data/faction_lore.json` |
| `Assets/StreamingAssets/Data/faction_radio_corpus.json` |
| `Assets/StreamingAssets/Data/faction_territory.json` |
| `Assets/StreamingAssets/Data/faction_war_communiques.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **5** (143 files, 1154 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Communications` | 1 | 6 |
| `Production` | 3 | 12 |
| `Radio` | 47 | 354 |
| `Shelter` | 87 | 754 |

**Verdict:** 1154 cases sit under matching regions — run those first (`Audio`, `Communications`, `Production`, `Radio`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **88**
(24 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioConditionHostBridge.cs` |
| `src/Audio/AudioCueCatalog.cs` |
| `src/Audio/AudioEventBridge.cs` |
| `src/Audio/AudioManager.cs` |
| `src/Audio/AudioSelfTest.cs` |
| `src/Audio/AudioSettings.cs` |
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Audio/ExpansionAudioBridge.cs` |
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Host/DoseLedgerHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **23**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `dose_ledger` | yes |
| `expanded_shelter` | no |
| `faction_espionage` | no |
| `nvis_communications` | no |
| `pneumatic_dispatch` | no |
| `radio` | no |
| `radio_program_production` | no |
| `radio_station` | no |
| `schedule` | no |
| `shelter` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **3**.

| Stream |
|---|
| `acoustic_detection` |
| `radio` |
| `shelter` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **55**
(CODEX_ONLY 18, GAMEPLAY_CONSUMED 22, OPTIONAL 2, UNRESOLVED 13).

| Catalog | Classification |
|---|---|
| `acoustic_triangulation_catalog.json` | UNRESOLVED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `faction_lore.json` | GAMEPLAY_CONSUMED |
| `faction_radio_corpus.json` | GAMEPLAY_CONSUMED |
| `faction_territory.json` | UNRESOLVED |
| `faction_war_communiques.json` | GAMEPLAY_CONSUMED |
| `faction_war_dialogue.json` | GAMEPLAY_CONSUMED |
| `faction_war_events.json` | GAMEPLAY_CONSUMED |
| `faction_war_journal.json` | GAMEPLAY_CONSUMED |

**Verdict:** 13 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **5**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_betrayed_trust` |
| `flag_chosen_faction_side` |
| `flag_ignored_distress` |
| `flag_responded_distress` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 12
**Surface:** save sections 23 (laddered 1) · RNG streams 3 · host files 20 · catalogs 22 · test regions 5 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-RADIO-FAMILY-TRUTH-266
wave: 19
status: PROPOSED — foreman claim required
packages: RDF-266A, RDF-266B, RDF-266C, RDF-266D
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Audio/AudioCueCatalog.cs  # §19 candidate host surface
  - src/Audio/AudioEventBridge.cs  # §19 candidate host surface
  - src/Audio/AudioManager.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/acoustic_triangulation_catalog.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/audio_accessibility_cues.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --audio-selftest
dependencies:
  - coordinate: 12 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
  - touches 1 versioned save ladder(s) — extend, never fork
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
