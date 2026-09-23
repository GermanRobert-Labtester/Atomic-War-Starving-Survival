# PLAN-RADIO-MEDIA-42 — Stations, Programs, Audiences, Jamming & Print

**Wave:** 5 (2026-09-21) · **Kind:** MAJOR EXPANSION
**Status:** PROPOSED — not a claim.
**Depends on:** PLAN-VERTICAL-CULTURE-04 (broadsheet), PLAN-WARLORDS-DIPLOMACY-29,
PLAN-SIGNALS-REMOTE-SENSING-49.
**Non-goals:** no real stations/persons/frequencies, no broadcast licensing
model, no second faction influence store.

---

## 1. Outcome

Radio is already the game's main information organ: `RadioHostSession`,
`RadioProgramProductionSystem` (Plan 173, sealed), `FactionRadioEngine`,
`PatrolRadioHooks`, `Distress*` stack, `AcousticDirectionFindingCatalog`,
`DirectionFindingCatalog`, `PsyOps`, `PropagandaSystem`, and 10+ radio data
files. What is missing is the **empire layer**: owning a schedule, growing an
audience, winning a war of words, and being jammed for it.

Player loop: **build station → programme a schedule → grow audience → fight
propaganda → survive jamming → publish the paper**.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Station & schedule | `RadioHostSession`, `radio_stations.json`, `radio_programs.json` | build/upgrade, schedule slots | reach, reliability, prestige |
| Programs | `RadioProgramProductionSystem` | produce, present, cancel | audience, morale, faction reaction |
| Persuasion | `PsyOps`, `PropagandaSystem`, `propaganda_*.json` | run campaigns | standing swings, unrest, counter-propaganda |
| Audience | new read model over existing reach | read listeners/trust | programme feedback, requests |
| Jamming & DF | `FactionRadioEngine`, direction finding | counter, relocate, rotate | coverage loss, enemy triangulation |
| Print | `PublicBroadsheetPressEngine` | publish issues | morale, faction perception |
| Distress | `Distress*` stack | answer/bait/intel | rescue, traps, intelligence |

---

## 2. Evidence

| Item | Detail |
|---|---|
| Core | `Radio/` (15+ files), `RadioProgramProductionSystem`, `PsyOps`, `Propaganda/PropagandaSystem.cs`, `PatrolRadioHooks`, `Distress*` (7 files) |
| Data | `radio.json`, `radio_programs.json`, `radio_stations.json`, `radio_intercepts.json`, `faction_radio_corpus.json`, `faction_war_radio.json`, `year_of_ash_radio.json`, `verdict_radio.json`, `propaganda_campaigns.json`, `propaganda_templates.json`, `radio_distress_signals*.json` |
| Sealed prior | Plan 173 radio program production (10/10 + host + panel), Plan 52 sound of scarcity, Plan 169 audio accessibility, Plan 107 distress signals (25 broadcasts) |
| Contracts | `DEC-06` SignalTrust retired; `DEC-15` forecast authority; `DEC-27` station identity seam |
| Audio | cue catalogs + Plan 169 visual notifications for critical cues |

---

## 3. Packages

### RD-42A — Station, schedule and reach
- Station tiers with power/antenna requirements; schedule slots consume
  production and staff; reach derives from propagation (PLAN 49), weather, and
  jamming.
- **Acceptance:** one schedule authority; no duplicate radio store; reach
  explainable in a panel row.
- **Verify:** `--radio-selftest` + focused.

### RD-42B — Program production and presenters
- Extend Plan 173: programme types (news, music, drama, teaching, memorial),
  presenter skill effects, prep costs, and content-utilization claims for every
  produced program.
- **Acceptance:** programs consume authored templates; a cancelled program
  refunds prep; no shadow content.
- **Verify:** `Plan173RadioProgramProductionTests` extends.

### RD-42C — Persuasion and counter-propaganda
- `PsyOps` + `PropagandaSystem` campaigns target factions/shelter morale within
  bounded bands; counter-campaigns defend standing; detection reveals the
  sponsor at high intensity.
- **Acceptance:** influence bounded, reversible, attributable; no second
  influence store; tone rules respected.
- **Verify:** psyops + faction suites.

### RD-42D — Audience read model
- Listeners/trust derived from reach, program quality, faction alignment, and
  events; feedback arrives as letters/requests (narrative owner).
- **Acceptance:** read-only projection; no mutation; feedback is authored and
  consumer-bound.
- **Verify:** focused radio + narrative.

### RD-42E — Jamming, direction finding and EW
- Enemy jamming reduces coverage; direction finding locates transmitters;
  countermeasures (frequency rotation, relocation, decoys) from the EW/radio
  owners.
- **Acceptance:** deterministic; counterplay exists; no infinite jamming.
- **Verify:** direction-finding selftests + focused.

### RD-42F — Print integration
- The broadsheet (PLAN-VERTICAL-CULTURE-04) and radio share the audience/news
  pipeline: one event vocabulary feeds both; publication and broadcast act on
  the same facts.
- **Acceptance:** no duplicate news store; issue and broadcast can't contradict
  canonical state.
- **Verify:** culture suite + `--culture-selftest`.

### RD-42G — Content volumes
- +8 stations, +20 programs, +12 campaigns, +10 intercepts, +16 feedback lines;
  fictional; consumer-bound.

---

## 4. Risks

| Risk | Mitigation |
|---|---|
| Propaganda trivializes factions | bands, cooldowns, attribution, counter-campaigns |
| Radio becomes idle-waiting | prep/opportunity costs; broadcast windows matter |
| Audio/voice scope creep | text-first; audio cues via existing catalogs |
| Duplicate news with broadsheet | one event vocabulary, two surfaces |

## 5. Verification

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Radio/
godot --headless --path . -- --radio-selftest
godot --headless --path . -- --audio-selftest
godot --headless --path . -- --data-integrity-selftest
```

---

## 6. Expanded census (18 files · 5,403 lines)

Scope: `Assets/Ashfall.Core/Radio/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 3 · DTO/Type 1 · Loader 1 · Save 1 · Support 6 · System 6

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `FactionRadioEngine.cs` | 244 | System | — | 0 | 0 | 0 |
| `FactionRadioTypes.cs` | 82 | DTO/Type | — | 0 | 0 | 0 |
| `PatrolRadioHooks.cs` | 201 | Support | — | 0 | 0 | 2 |
| `RadioBroadcastCatalog.cs` | 679 | Catalog | — | 0 | 0 | 0 |
| `RadioBroadcastModels.cs` | 360 | Support | — | 0 | 0 | 0 |
| `RadioDistressSystem.cs` | 835 | System | — | 0 | 0 | 2 |
| `RadioProgramCatalog.cs` | 119 | Catalog | — | 0 | 0 | 0 |
| `RadioProgramProductionSystem.cs` | 407 | System | — | 0 | 0 | 2 |
| `RadioPropagation.cs` | 233 | Support | — | 0 | 0 | 0 |
| `RadioReceiverPlan.cs` | 120 | Support | — | 0 | 0 | 0 |
| `RadioRecordingSystem.cs` | 107 | System | — | 0 | 0 | 2 |
| `RadioSave.cs` | 467 | Save | — | 0 | 0 | 0 |
| `RadioScheduleCoordinator.cs` | 427 | System | — | 0 | 0 | 0 |
| `RadioSignalLog.cs` | 149 | Support | — | 0 | 0 | 4 |
| `RadioStationCatalog.cs` | 163 | Catalog | — | 0 | 0 | 0 |
| `RadioStationCatalogLoader.cs` | 123 | Loader | — | 0 | 0 | 0 |
| `RadioTuner.cs` | 257 | Support | — | 0 | 0 | 0 |
| `ShelterRadioStationSystem.cs` | 430 | System | — | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 6 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `faction_war_radio.json` | object[2 keys] |
| `radio_intercepts.json` | object[2 keys] |
| `year_of_ash_radio.json` | object[2 keys] |
| `faction_radio_corpus.json` | object[7 keys] |
| `radio_stations.json` | object[2 keys] |
| `radio.json` | object[2 keys] |

**State surfaces:** `PatrolRadioHooks.cs`, `RadioDistressSystem.cs`, `RadioProgramProductionSystem.cs`, `RadioRecordingSystem.cs`, `RadioSignalLog.cs`, `ShelterRadioStationSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Radio/` |
| Test references | 79 name references across the test tree |
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

Domain files: 23. Other plans referencing their names: **7**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-RADIO-FAMILY-TRUTH-266` | 23 |
| `PLAN-RADIO-STATION-TRUTH-209` | 5 |
| `EVIDENCE` | 1 |
| `PLAN-SIGNALS-REMOTE-SENSING-49` | 1 |
| `PLAN-THREADING-ASYNCHRONY-72` | 1 |
| `PLAN-HELIOGRAPH-TRUTH-235` | 1 |
| `PLAN-RADIO-RECORDING-TRUTH-258` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `RD-42A` | `DistressFollowUpScheduler.cs`, `RadioScheduleCoordinator.cs`, `RadioStationCatalog.cs` |
| `RD-42B` | `RadioProgramProductionSystem.cs`, `RadioProgramCatalog.cs` |
| `RD-42C` | no name match — resolve at claim time |
| `RD-42D` | `RadioBroadcastModels.cs` |
| `RD-42E` | no name match — resolve at claim time |
| `RD-42F` | no name match — resolve at claim time |
| `RD-42G` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 19; intra-domain edges: **12**; isolated files:
**9**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `DistressFollowUpScheduler` | `RadioDistressSystem` |
| `RadioBroadcastCatalog` | `FactionRadioEngine` |
| `RadioBroadcastCatalog` | `RadioScheduleCoordinator` |
| `RadioBroadcastCatalog` | `RadioStationCatalog` |
| `RadioDistressSystem` | `DistressFollowUpScheduler` |
| `RadioProgramProductionSystem` | `RadioProgramCatalog` |
| `RadioProgramProductionSystem` | `RadioStationCatalog` |
| `RadioScheduleCoordinator` | `RadioBroadcastCatalog` |
| `RadioScheduleCoordinator` | `RadioStationCatalog` |
| `RadioStationCatalog` | `RadioStationCatalogLoader` |
| `RadioStationCatalogLoader` | `RadioStationCatalog` |
| `RadioTuner` | `RadioDistressSystem` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `RadioStationCatalog` | 4 |
| `RadioDistressSystem` | 2 |
| `DistressFollowUpScheduler` | 1 |
| `FactionRadioEngine` | 1 |
| `RadioBroadcastCatalog` | 1 |
| `RadioProgramCatalog` | 1 |
| `RadioScheduleCoordinator` | 1 |
| `RadioStationCatalogLoader` | 1 |
| `FactionRadioTypes` | 0 |
| `PatrolRadioHooks` | 0 |

**Class split:** hub 6 · sink 2 · source 2 · isolated 9.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 19. Host files: **18** · Test files: **49** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 18 | `src/Audio/AudioSelfTest.cs`, `src/Audio/ShelterOperationsAudioBridge.cs`, `src/Host/HostCli.NpcArcSelfTest.cs`, `src/Host/HostCli.PanelTests.cs`, `src/Host/HostCli.PlansB86_B89.cs` |
| Tests (`Ashfall.Core.Tests/`) | 49 | `Ashfall.Core.Tests/Balance/ShelterOperationsBalanceSim.cs`, `Ashfall.Core.Tests/Campaign/Plan33_38IntelCalendarIntegrationTests.cs`, `Ashfall.Core.Tests/Economy/TradeScreenPresenterSnapshotTests.cs`, `Ashfall.Core.Tests/FactionRadioBroadcastExpansionTests.cs`, `Ashfall.Core.Tests/FactionRadioCorpusTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **20** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `expanded_shelter` |
| `faction_espionage` |
| `radio` |
| `radio_program_production` |
| `radio_station` |
| `schedule` |
| `shelter` |
| `shelter_assignment` |
| `shelter_atmosphere` |
| `shelter_barter` |
| `shelter_decor` |
| `shelter_fire` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **17** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--faction-communique-board-selftest` |
| `--faction-ecology-selftest` |
| `--patrol-encounter-selftest` |
| `--radio-catalog-selftest` |
| `--radio-selftest` |
| `--shelter-actor-physics-selftest` |
| `--shelter-atmosphere-selftest` |
| `--shelter-decor-selftest` |
| `--shelter-hazard-loop-selftest` |
| `--shelter-hazard-selftest` |
| `--shelter-interior-selftest` |
| `--shelter-noise-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **12**.

| Event | First declaration |
|---|---|
| `OnBroadcast` | `Assets/Ashfall.Core/Muster/ColdCountSystem.cs` |
| `OnCulturalBroadcast` | `Assets/Ashfall.Core/VinylMoraleSystem.cs` |
| `OnFactionStandingChanged` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` |
| `OnFactionSuccession` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnFactionSurrender` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnProductionCompleted` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnProductionTick` | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` |
| `OnScheduleChanged` | `Assets/Ashfall.Core/ShelterScheduleSystem.cs` |
| `OnShelterEncounterResolved` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterEncounterStarted` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterFalseAlarm` | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` |
| `OnStationStateChanged` | `Assets/Ashfall.Core/WeatherStationSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |
| `Assets/StreamingAssets/Data/faction_intelligence.json` |
| `Assets/StreamingAssets/Data/faction_lore.json` |
| `Assets/StreamingAssets/Data/faction_radio_corpus.json` |
| `Assets/StreamingAssets/Data/faction_territory.json` |
| `Assets/StreamingAssets/Data/faction_war_communiques.json` |
| `Assets/StreamingAssets/Data/faction_war_dialogue.json` |
| `Assets/StreamingAssets/Data/faction_war_events.json` |
| `Assets/StreamingAssets/Data/faction_war_journal.json` |
| `Assets/StreamingAssets/Data/faction_war_location_overrides.json` |
| `Assets/StreamingAssets/Data/faction_war_radio.json` |
| `Assets/StreamingAssets/Data/food_types.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **3** (137 files, 1120 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Production` | 3 | 12 |
| `Radio` | 47 | 354 |
| `Shelter` | 87 | 754 |

**Verdict:** 1120 cases sit under matching regions — run those first (`Production`, `Radio`, `Shelter`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **73**
(20 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Host/FactionBranchHostSession.cs` |
| `src/Host/FactionIconLoader.cs` |
| `src/Host/HostCli.FactionCommuniqueSelfTests.cs` |
| `src/Host/LoaderWiringSelfTest.cs` |
| `src/Host/RadioCatalogSelfTest.cs` |
| `src/Host/RadioHostSession.cs` |
| `src/Host/RadioProgramProductionHostSession.cs` |
| `src/Host/RadioProgramProductionSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **20**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `expanded_shelter` | no |
| `faction_espionage` | no |
| `radio` | no |
| `radio_program_production` | no |
| `radio_station` | no |
| `schedule` | no |
| `shelter` | no |
| `shelter_assignment` | no |
| `shelter_atmosphere` | no |
| `shelter_barter` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **2**.

| Stream |
|---|
| `radio` |
| `shelter` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **45**
(CODEX_ONLY 14, GAMEPLAY_CONSUMED 22, UNRESOLVED 9).

| Catalog | Classification |
|---|---|
| `faction_lore.json` | GAMEPLAY_CONSUMED |
| `faction_radio_corpus.json` | GAMEPLAY_CONSUMED |
| `faction_territory.json` | UNRESOLVED |
| `faction_war_communiques.json` | GAMEPLAY_CONSUMED |
| `faction_war_dialogue.json` | GAMEPLAY_CONSUMED |
| `faction_war_events.json` | GAMEPLAY_CONSUMED |
| `faction_war_journal.json` | GAMEPLAY_CONSUMED |
| `faction_war_location_overrides.json` | GAMEPLAY_CONSUMED |
| `faction_war_radio.json` | GAMEPLAY_CONSUMED |
| `foundry_faction.json` | GAMEPLAY_CONSUMED |

**Verdict:** 9 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **4**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_chosen_faction_side` |
| `flag_ignored_distress` |
| `flag_responded_distress` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY-WITH-NOTES (11/12) · **Class:** standard · **Coupling (incoming plans):** 7
**Surface:** save sections 20 (laddered 0) · RNG streams 2 · host files 18 · catalogs 22 · test regions 3 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-RADIO-MEDIA-42
wave: —
status: PROPOSED — foreman claim required
packages: RD-42A, RD-42B, RD-42C, RD-42D, RD-42E, RD-42F, RD-42G
claim paths:
  - src/Audio/AudioStateCoordinator.cs  # §19 candidate host surface
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/faction_combat_thresholds.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/faction_intelligence.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Production/
  - godot --headless --path . -- --faction-communique-board-selftest
dependencies:
  - coordinate: 7 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | **no** |
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

**Pre-claim actions:** author or confirm: wave.
