# PLAN-ESPIONAGE-COUNTERINTEL-41 — Spy Networks, Dead Drops, Double Agents & Ciphers

**Wave:** 5 (2026-09-21) · **Kind:** MAJOR EXPANSION
**Status:** PROPOSED — not a claim.
**Depends on:** PLAN-WARLORDS-DIPLOMACY-29, PLAN-EVENT-WIRING-21,
PLAN-REFERENCE-INTEGRITY-34.
**Expanded appendix:** [`PLAN-ESPIONAGE-COUNTERINTEL-41_APPENDIX-A_ORPHAN_DOSSIERS.md`](PLAN-ESPIONAGE-COUNTERINTEL-41_APPENDIX-A_ORPHAN_DOSSIERS.md)
— domain-filtered orphan dossiers for this plan's espionage & counter-intelligence
systems (1 authorities), each mapped to its parent-plan mechanic row.

**Non-goals:** no real intelligence agencies or tactics, no surveillance of the
player outside the game, no second faction authority.

---

## 1. Outcome

Covert play is one authorized Core system (`FactionCovertOpsCoordinator`,
5 archetypes, suspicious bands) plus `InformantNetworkTradecraftEngine`,
`HiddenAgendaSystem`, `CounterIntelligence` host wiring and radio
interception — all below their potential. This plan builds the full loop:
**recruit → place → tradecraft → intercept → analyse → act or burn**.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Network | `InformantNetworkTradecraftEngine` | recruit, pay, task informants | report quality, risk, burn chance |
| Operations | `FactionCovertOpsCoordinator`, `espionage_operations.json` | infiltrate, steal, sabotage, propaganda, assassinate | success/detection, suspicion |
| Tradecraft | existing engine | dead drops, ciphers, safehouses | detection resistance, timing |
| Counter-intel | `CounterIntelligence` host path | sweep for leaks, interrogate | catch moles, false accusations |
| Intercepts | `SignalTriangulationSystem`, `radio_intercepts.json` | locate signals, decode | intel reports, countermeasures |
| Hidden agendas | `HiddenAgendaSystem` | uncover, confront, recruit | betrayal arcs, defection |
| Cover | `ShelterReputation`, front businesses | maintain legitimacy | suspicion cooldown, access |

---

## 2. Evidence

| Item | Detail |
|---|---|
| Core | `FactionCovertOpsCoordinator` (5 ops, suspicion bands), `Espionage/InformantNetworkTradecraftEngine.cs` (orphan), `HiddenAgendaSystem` (sealed), `SignalTriangulationSystem`, `CounterIntelligence` host setup |
| Data | `espionage_operations.json`, `espionage_missions.json`, `faction_intelligence.json`, `radio_intercepts.json` |
| Prior seals | Plan 153 espionage (5/5), Plan 167 consequence routing (4/4), Plan 132 hidden agendas (10/10), DEBT-PLAN167 sealed |
| Guarantees | deterministic resolution, agent compromise typed, suspicion cooling |

---

## 3. Packages

### ES-41A — Informant network
- `InformantNetworkTradecraftEngine` host session: recruit (contact, cost,
  leverage), task (watch a faction/site), report (freshness, reliability),
  burn (exposure → loss of the informant). Rides survivor relations and the
  economy for payment.
- **Acceptance:** informants are real survivors/NPCs; reports are actionable
  (named threat/opportunity); burn is recoverable; no duplicate NPC store.
- **Verify:** focused espionage tests + `--counter-intelligence-selftest`.

### ES-41B — Operations, tradecraft and cover
- Operations expose prep choices (agent, equipment, cover, timing); dead drops
  and ciphers modify detection bands; safehouses reduce capture consequences;
  cover businesses consume funds and upkeep.
- **Acceptance:** detection math explainable; capture → compromise → rescue or
  disavow choice; no new funds ledger.
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/Factions/` +
  `Plan153FactionEspionageIntegrationTests`.

### ES-41C — Counter-intelligence
- Sweeps find evidence of enemy agents, with false-positive bands; interrogation
  uses the justice layer (PLAN-JUSTICE-LAW-37) for due process; double agents
  can be turned.
- **Acceptance:** a sweep never reveals a name for certain without evidence;
  turning an agent is possible; own-network leaks are detectable.
- **Verify:** `--counter-intelligence-selftest` + verdict suites.

### ES-41D — Intercepts and analysis
- Radio intercepts feed an intelligence board: raw → verified → actionable,
  with countermeasures (jamming, frequency rotation) from the radio/EW owners.
- **Acceptance:** intercepts resolve to real map/faction facts; verification
  costs time; no duplicate intel store.
- **Verify:** radio focused suites.

### ES-41E — Hidden agendas integration
- Confrontation can now be pre-empted by intelligence: knowing an agenda early
  opens recruit/blackmail/isolate options; the existing confrontation path
  remains canonical.
- **Acceptance:** agenda knowledge gates options only; the sealed confrontation
  fallout (morale, standing, confiscation) is unchanged.
- **Verify:** `Plan132HiddenAgendaIntegrationTests` extends.

### ES-41F — Content volumes
- +10 operatives, +12 missions, +8 intercepts, +6 safehouse/cover rows; all
  fictional; consumer-bound.

---

## 4. Risks

| Risk | Mitigation |
|---|---|
| Espionage becomes a win button | detection/suspicion, failed ops cost agents and standing |
| Paranoia fatigue | bounded sweeps per period; false positives create real drama, not spam |
| Intel bypasses exploration | verified intel reveals direction, not contents; exploration still needed |
| Overlap with justice | interrogation delegates to the trial layer |

## 5. Verification

```bash
godot --headless --path . -- --counter-intelligence-selftest
bash scripts/run_test.sh Ashfall.Core.Tests/Factions/Plan153FactionEspionageIntegrationTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/Plan132HiddenAgendaIntegrationTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Radio/
```

---

## 6. Expanded census (5 files · 1,935 lines)

Scope: `Assets/Ashfall.Core/Espionage/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 1 · System 4

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `InformantNetworkTradecraftEngine.cs` | 222 | System | **yes** | 0 | 0 | 0 |
| `EspionageConsequenceRouter.cs` | 120 | Support | — | 0 | 0 | 2 |
| `EspionageSystem.cs` | 734 | System | — | 0 | 0 | 7 |
| `FactionCovertOpsCoordinator.cs` | 518 | System | **yes** | 0 | 0 | 2 |
| `ShelterEspionageSystem.cs` | 341 | System | — | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 4 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `espionage_missions.json` | object[3 keys] |
| `espionage_operations.json` | object[3 keys] |

**State surfaces:** `EspionageConsequenceRouter.cs`, `EspionageSystem.cs`, `FactionCovertOpsCoordinator.cs`, `ShelterEspionageSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Espionage/` |
| Test references | 13 name references across the test tree |
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

Domain method: plan-body `.cs` enumeration.
Domain files: 5. Other plans referencing them: **6**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-ESPIONAGE-SYSTEM-TRUTH-161` | 5 |
| `PLAN-FACTIONS-STATE-FAMILY-TRUTH-268` | 4 |
| `PLAN-INTERNAL-SECURITY-TRUTH-224` | 3 |
| `PLAN-ORPHAN-SEAL-01` | 2 |
| `EVIDENCE` | 2 |
| `PLAN-WARLORDS-DIPLOMACY-29` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `ES-41A` | `InformantNetworkTradecraftEngine.cs` |
| `ES-41B` | `FactionCovertOpsCoordinator.cs`, `InformantNetworkTradecraftEngine.cs` |
| `ES-41C` | no name match — resolve at claim time |
| `ES-41D` | no name match — resolve at claim time |
| `ES-41E` | no name match — resolve at claim time |
| `ES-41F` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 5. Host files: **4** · Test files: **8** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 4 | `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/EspionageHostSession.cs`, `src/Main.Plans166_169.cs`, `src/Main.Plans50_53.cs` |
| Tests (`Ashfall.Core.Tests/`) | 8 | `Ashfall.Core.Tests/Campaign/Plans50_53_SharedIntegrationTests.cs`, `Ashfall.Core.Tests/Espionage/InformantNetworkTradecraftEngineTests.cs`, `Ashfall.Core.Tests/Expeditions/PlanE1_29VehicleEspionageTests.cs`, `Ashfall.Core.Tests/Factions/Plan153FactionEspionageIntegrationTests.cs`, `Ashfall.Core.Tests/Factions/ShelterEspionageSystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 5 files; intra-domain edges: **2**; isolated: **3**.

| From | → To |
|---|---|
| `EspionageConsequenceRouter` | `EspionageSystem` |
| `EspionageSystem` | `EspionageConsequenceRouter` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **19** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `caravan_trade_network` |
| `espionage` |
| `expanded_shelter` |
| `faction_espionage` |
| `piezometer_network` |
| `shelter` |
| `shelter_assignment` |
| `shelter_atmosphere` |
| `shelter_barter` |
| `shelter_decor` |
| `shelter_fire` |
| `shelter_noise` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **15** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--faction-communique-board-selftest` |
| `--faction-ecology-selftest` |
| `--rumor-network-selftest` |
| `--shelter-actor-physics-selftest` |
| `--shelter-atmosphere-selftest` |
| `--shelter-decor-selftest` |
| `--shelter-hazard-loop-selftest` |
| `--shelter-hazard-selftest` |
| `--shelter-interior-selftest` |
| `--shelter-noise-selftest` |
| `--shelter-operations-selftest` |
| `--shelter-ops-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **8**.

| Event | First declaration |
|---|---|
| `OnConsequenceApplied` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnConsequenceDispatched` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |
| `OnFactionStandingChanged` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` |
| `OnFactionSuccession` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnFactionSurrender` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnShelterEncounterResolved` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterEncounterStarted` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterFalseAlarm` | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/espionage_missions.json` |
| `Assets/StreamingAssets/Data/espionage_operations.json` |
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

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **3** (89 files, 778 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Espionage` | 1 | 4 |
| `NarrativeConsequence` | 1 | 20 |
| `Shelter` | 87 | 754 |

**Verdict:** 778 cases sit under matching regions — run those first (`Espionage`, `NarrativeConsequence`, `Shelter`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **68**
(17 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Host/EspionageHostSession.cs` |
| `src/Host/EspionageSaveStore.cs` |
| `src/Host/FactionBranchHostSession.cs` |
| `src/Host/FactionIconLoader.cs` |
| `src/Host/HostCli.FactionCommuniqueSelfTests.cs` |
| `src/Host/NarrativeArcConsequenceAdapter.cs` |
| `src/Host/RumorNetworkHostSession.cs` |
| `src/Host/RumorNetworkSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **19**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `caravan_trade_network` | no |
| `espionage` | no |
| `expanded_shelter` | no |
| `faction_espionage` | no |
| `piezometer_network` | no |
| `shelter` | no |
| `shelter_assignment` | no |
| `shelter_atmosphere` | no |
| `shelter_barter` | no |
| `shelter_decor` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `shelter` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **28**
(CODEX_ONLY 5, GAMEPLAY_CONSUMED 16, UNRESOLVED 7).

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

**Verdict:** 7 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **2**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_chosen_faction_side` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY-WITH-NOTES (11/12) · **Class:** standard · **Coupling (incoming plans):** 6
**Surface:** save sections 19 (laddered 0) · RNG streams 1 · host files 15 · catalogs 22 · test regions 3 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-ESPIONAGE-COUNTERINTEL-41
wave: —
status: PROPOSED — foreman claim required
packages: ES-41A, ES-41B, ES-41C, ES-41D, ES-41E, ES-41F
claim paths:
  - src/Audio/AudioStateCoordinator.cs  # §19 candidate host surface
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/espionage_missions.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/espionage_operations.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Espionage/
  - godot --headless --path . -- --faction-communique-board-selftest
dependencies:
  - coordinate: 6 other plan(s) name these artifacts (§12)
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
