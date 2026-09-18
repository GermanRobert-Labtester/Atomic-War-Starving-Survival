# ASHFALL — GENERATION WAVE 12 — IMPLEMENTATION UNBLOCKER MASTER PLAN — PART 2
// SPDX-License-Identifier: MIT

**Document role:** execution-grade Part 2 master plan for Generation Wave 12, continuing directly from the verified Part 1 frontier (`4829104_ASHFALL_WAVE12_IMPLEMENTATION_UNBLOCKER_PLAN_PART1.md`).

**Purpose:** convert the remaining eight unsealed flagship plans in the Wave 12 queue into execution-ready unblocking instructions. Each task provides 20 procedural instructions and structured mini-tasks containing 4 mini-substeps each, maintaining rigorous authority boundaries, single ownership, determinism, and zero-warning build discipline.

**Wave 12 Part 2 Premise:** Part 1 targeted depth-content rails, spatial holdfast view, asset hygiene, longitudinal medicine, focus navigation, session soak, authored identity, and voice delivery. Part 2 attacks the systemic bridge layers: **persistent expedition discovery consequences, needs-to-performance cascade, combat-to-faction political bridge, affliction capability gating, social affinity effects, reproducible balance evidence, tagged release craft, and explicit asset truth**.

**Part 2 Execution Set (Exactly 8 Tasks):**
1. **Task C1 — C1[20] Expedition Discovery → Persistent World Consequences (Plan 133A/133B/133C)**
2. **Task C2 — C1[21] Needs → Performance Cascade: Survival Pressure in Work, Combat & Expeditions (Plan 137A/137B/137C/137D)**
3. **Task C3 — C1[22] Combat → Faction Standing Bridge: Political Consequences for Violence (Plan 139A/139B/139C/139D)**
4. **Task C4 — C1[23] Medical Afflictions → Quest, Work, Expedition & Combat Capability (Plan 143A/143B/143C/143D)**
5. **Task D1 — C2[19] Relationship Effects, Pair History & The Proven Inner-Life Loop (Plan 44A/44B/44C)**
6. **Task D2 — C2[20] Reproducible Balance Evidence, Playable Metrics & Difficulty Decisions (Plan 46A/46B/46C)**
7. **Task D3 — C2[21] Version Contracts, Release Tagging, Artifact Provenance & Save-Safe Hotfixes (Plan 48A/48B/48C)**
8. **Task D4 — C2[22] Asset Truth, Explicit ID-to-File Mapping & Screen Visual Proof (Plan 50A/50B/50C)**

---

# 0. OPERATING CONTRACT

## 0.1 Allowed Terminal States

Every task in Wave 12 Part 2 must terminate in one of these recognized states:

- **IMPLEMENTED** — Missing mechanism, bridge, read model, or consumer is landed through its canonical owner, and all required verification gates pass.
- **DECIDED-DEFERRED** — Product or architecture decision is signed by the foreman, explicitly defining deferred scope and preserving non-fabricated boundaries.
- **RETIRED** — A legacy shim, speculative system, or duplicate authority is deleted and unregistered with full reference validation.
- **VERIFIED-RESOLVED** — Investigation at repository `HEAD` proves the required contract is already sealed by prior work, skipping redundant implementation.
- **ROUTED-REPAIR** — A genuine production defect is discovered outside the task scope, isolated into a dedicated repair ticket with reproducible test.

## 0.2 Mandatory Non-Negotiable Rules

1. **Godot is authoritative; Unity is retired.** No Unity packages, namespaces, or references may be introduced.
2. **Core remains engine-free.** `Assets/Ashfall.Core/` (`netstandard2.1`) must never reference Godot or engine assemblies.
3. **JSON data is authoritative.** Authoritative data resides in `Assets/StreamingAssets/Data/` declaring `schema_version: 1`.
4. **One authority per concern.** Never duplicate faction standing, world state, affliction status, or inventory ledgers.
5. **Deterministic replay.** No unseeded `System.Random`, wall-clock time, or hash-iteration ordering in domain logic.
6. **Substeps are instructions, not tasks.** The 20 substeps per task represent ordered procedural instructions.
7. **Mini-tasks require 4 mini-substeps.** Any mini-task (e.g. `.1`, `.2`) must contain exactly 4 subsequent execution instructions.
8. **Claims before edits.** File claims must be registered in `WORKTREE_OWNERSHIP.md` before editing.
9. **Zero-warning build bar.** All compilation passes must complete with 0 errors and 0 warnings.
10. **Targeted verification first.** Use `bash scripts/run_test.sh <file>` to verify changed areas without running broad suites.

---

# 1. DETAILED TASK SPECIFICATIONS

## TASK C1 — C1[20] Expedition Discovery → Persistent World Consequences (Plan 133A/133B/133C)

- **Source Plan:** `C-integration-plans/C1_planintegration[20].md` (Plan 133)
- **Blocker Class:** DISCONNECTED SYSTEMS / WORLD CONTINUITY GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Expeditions/`, `Assets/Ashfall.Core/World/`
- **Target Subsystem:** Expedition discovery results, persistent location state, route modification, caravan awareness

### 20 Procedural Substeps:
1. Inspect `ExpeditionResult` and `LocationEvolutionSystem` to audit how expedition discoveries currently terminate.
2. Confirm the architectural invariant: discoveries are facts and events; do not create a second world-state simulation.
3. Claim `Assets/Ashfall.Core/Expeditions/ExpeditionDiscoveryBridge.cs` and `Assets/Ashfall.Core/World/LocationRegistry.cs`.
4. Author `ExpeditionDiscoveryBridge.cs` as an event-driven adapter translating discoveries into canonical world state mutations.
5. Connect resource cache discoveries to unlock persistent salvage and harvesting nodes at the target location.
6. Connect hazard site discoveries (radiation hot zone, collapsed tunnel) to alter travel risk on intersecting routes in `ExpeditionRouteSystem`.
7. Wire military bunker discoveries to register faction awareness through `FactionBranchCoordinator`.
8. Ensure newly revealed trading outposts register with `TravelingCaravanSystem`, opening regional caravan route stops.
9. Connect survivor encampment discoveries to unlock rescue missions in `DistressRescueMissionManager`.
10. Ensure discovery consequence application is strictly idempotent: re-visiting a discovered site never duplicates world mutations.
11. Implement discovery revelation decay: unexploited temporary resource caches expire after authored campaign days.
12. Ensure discovery facts emit canonical day events (`OnLocationDiscovered`, `OnRouteHazardAltered`) via `DayEventVocabulary`.
13. Update `ExpeditionReportPanel.cs` to present discovery consequences explicitly in the post-expedition debrief screen.
14. Bind wasteland map presentation in `WastelandMapView.cs` to refresh location icons when discoveries modify site tags.
15. Add comprehensive unit tests in `Ashfall.Core.Tests/Expeditions/Plan133ExpeditionDiscoveryTests.cs`.
16. Validate that discovery consequences persist across save/load cycles through existing `LocationSaveStore` without schema breaks.
17. Verify deterministic discovery outcomes: identical campaign seeds produce identical location mutation orders.
18. Run `--data-integrity-selftest` to ensure all newly referenced location tags validate across data catalogs.
19. Run focused expedition and world tests to confirm zero regressions in travel speed or fuel consumption.
20. Update `docs/systems/WORLD_DISCOVERY_CONSEQUENCES.md` with the discovery taxonomy and complete handoff.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### C1.1 Discovery Consequence Adapter Implementation
- (a) Define immutable record `DiscoveryConsequenceFact` containing discovery kind, target location ID, and modifier payload.
- (b) Author `ExpeditionDiscoveryBridge` consuming `ExpeditionCompletedEvent` and resolving consequence facts.
- (c) Route resource consequences directly to `LocationEvolutionSystem.ApplyResourceDiscovery`.
- (d) Author unit tests asserting that completed expeditions dispatch consequences to appropriate domain authorities.

#### C1.2 Route Hazard & Travel Pacing Modification
- (a) Extend `ExpeditionRouteSystem` to support additive hazard modifiers (`HazardModifierRecord`) on route segments.
- (b) Apply discovered landslide, bridge collapse, or fallout hot-spot modifiers to increase segment transit hours.
- (c) Wire the route travel time calculation in `ExpeditionHostSession` to consume active hazard modifiers.
- (d) Validate that clearing a hazard through an engineering expedition restores nominal travel time.

#### C1.3 Caravan & Economy Integration
- (a) Wire discovered settlement nodes to notify `RegionalPriceAtlas`, activating local commodity supply indices.
- (b) Update `TravelingCaravanSystem` to evaluate discovered routes when selecting trading itineraries.
- (c) Ensure newly opened caravan stops refresh the caravan radar display in `TravelingCaravanPanel.cs`.
- (d) Author integration tests verifying that discovering a settlement increases caravan arrival frequencies.

#### C1.4 Discovery State Persistence & Idempotency
- (a) Audit `LocationSaveStore` to ensure discovered tags, cleared hazards, and revealed nodes serialize cleanly.
- (b) Implement an exactly-once discovery receipt ledger preventing duplicate discovery rewards on reload.
- (c) Test mid-expedition save and reload, confirming that in-flight discoveries trigger only upon safe return.
- (d) Run deterministic replay tests asserting identical world state progression across 30 campaign days.

---

## TASK C2 — C1[21] Needs → Performance Cascade: Survival Pressure in Work, Combat & Expeditions (Plan 137A/137B/137C/137D)

- **Source Plan:** `C-integration-plans/C1_planintegration[21].md` (Plan 137)
- **Blocker Class:** UNCONNECTED DOMAINS / LACK OF SURVIVAL TRADEOFFS
- **Canonical Owner:** `Assets/Ashfall.Core/Survivors/`, `Assets/Ashfall.Core/DutyRoster/`
- **Target Subsystem:** Survivor needs projection, combat stat penalties, work yield degradation, travel fatigue

### 20 Procedural Substeps:
1. Review current `NeedsSystem` thresholds: hunger, thirst, fatigue, warmth, morale, and hygiene states.
2. Confirm the core principle: do not create a second survivor-condition authority; project from existing `NeedsSystem`.
3. Claim `Assets/Ashfall.Core/Survivors/NeedsPerformanceProjection.cs` and related performance consumers.
4. Author `NeedsPerformanceProjection.cs` as a pure, stateless mathematical read model evaluating current survivor needs.
5. Map hunger stages (Peckish, Hungry, Starving) to labor yield penalties (0%, -15%, -40%) in `WorkerProductivityContract`.
6. Map severe dehydration to combat accuracy and evasion debuffs in `CombatCalculationEngine`.
7. Map acute exhaustion and sleep deprivation to increased mistake rates and equipment wear during crafting.
8. Map hypothermia/freezing states to travel speed penalties and increased injury chances during wasteland expeditions.
9. Connect low morale/despair to reduced critical strike chances and increased retreat triggers in combat.
10. Ensure needs performance modifiers compose cleanly with Plan 24 fitness verdicts without double-counting penalties.
11. Implement positive performance bonuses for well-fed, hydrated, high-morale survivors (e.g., +10% work speed).
12. Ensure performance penalties are clearly exposed in UI: `DutyRosterPanel.cs` displays named reasons for reduced output.
13. Update `CombatPanel.cs` and combat telemetry to display survivor efficiency debuffs caused by active needs.
14. Ensure expedition preparation preflight checks in `ExpeditionHostSession` warn when departing with starving crew.
15. Add unit tests in `Ashfall.Core.Tests/Survivors/Plan137NeedsPerformanceCascadeTests.cs`.
16. Verify that performance penalties are pure derivations of persisted needs state and introduce 0 new save fields.
17. Validate deterministic performance math: identical need values yield identical penalty multipliers across all platforms.
18. Run `--panel-bind-lifecycle-selftest` to ensure extended UI projections do not leak event subscriptions.
19. Run full survivor and duty focused test suites to confirm zero regressions in existing assignment logic.
20. Document performance formulas in `docs/systems/NEEDS_PERFORMANCE_CASCADE.md` and complete handoff.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### C2.1 Pure Performance Projection Contract
- (a) Define immutable struct `SurvivorPerformanceMultipliers` (WorkSpeed, CombatEfficiency, TravelPacing, MistakeRisk).
- (b) Implement `NeedsPerformanceProjection.Evaluate(SurvivorNeedsState needs)` as a pure, allocation-free function.
- (c) Establish clamping boundaries: minimum performance floor 0.25x; maximum peak performance ceiling 1.25x.
- (d) Author unit tests verifying mathematical continuity across every need threshold boundary.

#### C2.2 Work & Crafting Integration
- (a) Wire `WorkerProductivityContract` to query `NeedsPerformanceProjection` when calculating daily production output.
- (b) Apply work speed penalties to workshop crafting progress, greenhouse harvest yield, and water filtration rate.
- (c) Wire acute exhaustion to roll mistake chances that consume extra raw materials without crafting failure.
- (d) Validate through unit tests that satisfying needs immediately restores baseline production rates.

#### C2.3 Tactical Combat Efficiency Linking
- (a) Connect combatant accuracy and weapon handling speed to current survivor thirst and fatigue levels.
- (b) Apply severe morale penalties to panic break-points, increasing the chance of survivor retreat under fire.
- (c) Ensure combat tooltips display active survival debuffs alongside weapon and cover modifiers.
- (d) Author combat simulation tests asserting that well-rested squads outperform exhausted squads under identical RNG.

#### C2.4 Expedition Pacing & Fatigue Cascade
- (a) Integrate needs performance projection into `ExpeditionState.weatherSpeedMultiplier` composition chain.
- (b) Increase daily travel hour requirements when expedition members suffer from untreated hypothermia or hunger.
- (c) Expose explicit pre-departure warnings in `ExpeditionPanel.cs` if any assigned survivor is in critical need.
- (d) Verify that expedition journey tests pass with predictable travel delays when crew needs are neglected.

---

## TASK C3 — C1[22] Combat → Faction Standing Bridge: Political Consequences for Violence (Plan 139A/139B/139C/139D)

- **Source Plan:** `C-integration-plans/C1_planintegration[22].md` (Plan 139)
- **Blocker Class:** ISOLATED COMBAT / MISSING DIPLOMATIC CONSEQUENCES
- **Canonical Owner:** `Assets/Ashfall.Core/Combat/`, `Assets/Ashfall.Core/Factions/`
- **Target Subsystem:** Combat resolution bridge, faction standing deltas, witness handling, bounty triggers

### 20 Procedural Substeps:
1. Inspect `CombatEncounter` and `FactionBranchCoordinator` to audit the current combat resolution lifecycle.
2. Confirm the core rule: do not create a second reputation engine; route through canonical faction authorities.
3. Claim `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and `src/Host/CombatHostSession.cs`.
4. Author `CombatFactionStandingBridge.cs` to capture combat end events and evaluate faction ramifications.
5. Check enemy combatant `faction_id`: combat with neutral or unaligned bandits produces zero faction consequences.
6. When fighting faction-aligned units, calculate standing penalties based on casualty severity (incapacitation vs death).
7. Implement witness mechanics: eliminate penalties if all enemy combatants are eliminated with zero radio distress call.
8. Wire successful radio distress calls during combat to guarantee incident reporting to the victim faction.
9. Route verified combat incidents to `FactionStanceEngine`, decrementing standing and shifting stance towards Hostile.
10. Trigger faction retaliation events: dropping below Hostile thresholds activates retaliatory bunker raids or embargoes.
11. Wire faction bounty contracts in `DynamicQuestlineSystem` if player kills prominent named faction leaders.
12. Ensure combat standing deltas are strictly exactly-once per combat encounter; prevent combat-reset farming.
13. Update `CombatVictoryPanel.cs` to clearly display diplomatic consequences ("-15 Vanguard Standing — Incident Reported").
14. Bind `FactionDetailPanel.cs` incident history to render recent hostile clashes and casualty counts.
15. Add unit tests in `Ashfall.Core.Tests/Combat/Plan139CombatFactionBridgeTests.cs`.
16. Validate that faction standing deltas persist cleanly in `FactionSaveStore` across save/load interruptions.
17. Verify deterministic consequence calculation: identical combat casualty reports produce identical standing drops.
18. Run `--data-integrity-selftest` to ensure all combatant faction references match IDs in `factions.json`.
19. Run faction and combat focused test suites to confirm zero regressions in combat turn resolution.
20. Document combat diplomatic rules in `docs/factions/COMBAT_FACTION_CONSEQUENCES.md` and complete handoff.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### C3.1 Combat Incident Bridge Implementation
- (a) Define record `CombatConsequenceRecord` containing encounter ID, faction ID, casualties, and reported flag.
- (b) Author `CombatFactionStandingBridge.ResolveIncident` evaluating casualty lists and radio transmission events.
- (c) Prevent double-application: maintain an encounter receipt hash set ensuring each battle resolves exactly once.
- (d) Author unit tests verifying that defeat, victory, and retreat emit proper consequence records.

#### C3.2 Witness & Radio Reporting Logic
- (a) Inspect combat state for completed enemy radio distress calls prior to squad wipeout.
- (b) Implement the "no witnesses" rule: wiping out an enemy patrol before radio completion conceals player identity.
- (c) Wire concealed ambushes to generate unverified rumors instead of immediate direct standing penalties.
- (d) Author unit tests validating that concealed victories protect faction standing from instant collapse.

#### C3.3 Faction Retaliation & Embargo Cascades
- (a) Connect combat standing drops below threshold -50 to trigger diplomatic embargoes in `TradeEmbargoSystem`.
- (b) Route severe hostile transitions to `SkyDefenseBatterySystem` as scheduled incoming aerial assault raids.
- (c) Cancel active friendly faction courier and trader visits when hostile combat engagement is confirmed.
- (d) Validate through integration tests that attacking a faction's patrol halts trade in that faction's territory.

#### C3.4 Post-Combat UI & Debrief Feedback
- (a) Extend `CombatVictoryPanel.cs` to show a diplomatic consequence summary section with faction emblems.
- (b) Render explicit text explaining why standing changed ("Vanguard patrol eliminated; distress signal broadcasted").
- (c) Update the campaign daily briefing to include intelligence reports regarding enemy faction mobilization.
- (d) Run `--ui-a11y-selftest` to ensure diplomatic consequence alerts adhere to accessible contrast standards.

---

## TASK C4 — C1[23] Medical Afflictions → Quest, Work, Expedition & Combat Capability (Plan 143A/143B/143C/143D)

- **Source Plan:** `C-integration-plans/C1_planintegration[23].md` (Plan 143)
- **Blocker Class:** DISCONNECTED AFFLICTIONS / INCOMPLETE CAPABILITY GATING
- **Canonical Owner:** `Assets/Ashfall.Core/Medical/`, `Assets/Ashfall.Core/Quests/`
- **Target Subsystem:** Affliction capability contract, duty disqualification, quest route predicates, expedition gating

### 20 Procedural Substeps:
1. Review current affliction catalog: trauma, fractures, acute radiation, chemical dependency, blindness, infection.
2. Confirm the core principle: affliction effects project from canonical medical state; do not create duplicate authorities.
3. Claim `Assets/Ashfall.Core/Medical/AfflictionCapabilityProjection.cs` and `Assets/Ashfall.Core/Quests/QuestPredicateEvaluator.cs`.
4. Author `AfflictionCapabilityProjection.cs` evaluating active afflictions to derive physical and cognitive capabilities.
5. Define capability flags: `CanPerformManualLabor`, `CanOperateMachinery`, `CanTravelExpedition`, `CanEngageCombat`.
6. Map severe physical trauma (compound fracture, spine injury) to disqualify survivors from expedition dispatch.
7. Map sensory and psychological trauma (flashbacks, acute delirium) to disqualify survivors from precision medical duty.
8. Wire `DutyRosterAssignmentEngine` to reject duty assignments when a survivor lacks required capability flags.
9. Wire `ExpeditionHostSession.ExecuteStart` to refuse party dispatch if any member has disqualifying mobility afflictions.
10. Connect affliction predicates to quest route choices: author alternate dialogue or paths for injured survivors.
11. Compose affliction capability penalties with Plan 137 needs performance cascade to ensure unified efficiency modifiers.
12. Ensure temporary light-duty assignments remain available for recovering survivors in `DutyRosterPanel.cs`.
13. Update `SurvivorDetailPanel.cs` to display capability badges ("Restricted: No Heavy Lifting", "Combat Ineligible").
14. Ensure recovering afflictions automatically restore capability flags upon discharge from `MedicalWardSystem`.
15. Add unit tests in `Ashfall.Core.Tests/Medical/Plan143AfflictionCapabilityTests.cs`.
16. Validate that capability projections are purely derived and require zero additional fields in save stores.
17. Verify deterministic capability derivation across repeated save/load cycles and session restarts.
18. Run `--panel-bind-lifecycle-selftest` to confirm that capability UI elements properly release event hooks.
19. Run medical and duty focused test suites to verify zero regressions in medical triage or roster assignments.
20. Document capability taxonomy in `docs/systems/AFFLICTION_CAPABILITY_GATING.md` and complete handoff.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### C4.1 Affliction Capability Projection Engine
- (a) Define enum `SurvivorCapabilityFlags` (ManualLabor, PrecisionWork, WastelandTravel, TacticalCombat, Leadership).
- (b) Implement `AfflictionCapabilityProjection.GetCapabilities(SurvivorAfflictionsState state)` as a pure query.
- (c) Map each entry in `afflictions.json` to its specific prohibited capability flags with severity thresholds.
- (d) Author unit tests asserting that compound fractures strictly prohibit `WastelandTravel` and `ManualLabor`.

#### C4.2 Duty Assignment Capability Enforcement
- (a) Update `DutyRosterAssignmentEngine.ValidateAssignment` to evaluate candidate survivor capability flags.
- (b) Block assignment to Foundry, Workshop, or Generator if `ManualLabor` or `PrecisionWork` is revoked.
- (c) Surface clear rejection reason strings in the UI ("Assignment Refused: Survivor has untreated fractured arm").
- (d) Author unit tests proving that assigning an impaired survivor without explicit waiver fails gracefully.

#### C4.3 Expedition Party Capability Preflight
- (a) Extend `ExpeditionPreflightCheck` to verify that all party members possess the `WastelandTravel` capability.
- (b) Prevent dispatching expeditions containing bedridden or quarantined survivors with explicit UI error text.
- (c) Allow specialized "Evacuation" or "Medical Transfer" missions to bypass travel restrictions when designed.
- (d) Author automated tests verifying that dispatch buttons remain disabled when invalid survivors are selected.

#### C4.4 Quest Route & Dialogue Predicate Linking
- (a) Extend `QuestPredicateEvaluator` to support `has_affliction`, `missing_capability`, and `has_prosthetic` tags.
- (b) Author alternate dialogue branches for quests when interacting NPCs notice visible injuries or cybernetics.
- (c) Ensure quest objectives requiring heavy clearing check that assigned party members possess `ManualLabor`.
- (d) Author unit tests validating quest progression across varying survivor physical condition profiles.

---

## TASK D1 — C2[19] Relationship Effects, Pair History & The Proven Inner-Life Loop (Plan 44A/44B/44C)

- **Source Plan:** `C-integration-plans/C2_planintegration[19].md` (Plan 44)
- **Blocker Class:** DISPLAY-ONLY SOCIAL STATS / LACK OF OPERATIONAL CONSEQUENCES
- **Canonical Owner:** `Assets/Ashfall.Core/Survivors/`, `Assets/Ashfall.Core/Social/`
- **Target Subsystem:** Pair affinity effects, social history log, work synergy, expedition morale stabilization

### 20 Procedural Substeps:
1. Review current survivor relations architecture: affinity scores, ideological friction, trauma bonds, and grudges.
2. Confirm the core objective: turn affinity from a displayed statistic into an operational input for duty and travel.
3. Claim `Assets/Ashfall.Core/Survivors/SurvivorRelationsSystem.cs` and `Assets/Ashfall.Core/Survivors/PairHistoryLedger.cs`.
4. Implement `PairHistoryLedger.cs` recording bounded, explainable relation transition events between survivor pairs.
5. Map high affinity bands (Kindred, Trusted) to operational bonuses when paired together on shared duty shifts (+15% output).
6. Map hostile affinity bands (Rival, Grudge) to increased friction, workplace accidents, and morale decay when co-assigned.
7. Connect pair relationships to expedition outcomes: paired friends stabilize psychological trauma during wasteland terror.
8. Connect pair relationships to caregiving: care administered by trusted companions accelerates medical recovery curves.
9. Implement mentorship synergy: assigning veteran craftsmen with apprentice friends speeds skill progression ticks.
10. Ensure relation events record clear causal reasons ("Shared near-death expedition experience", "Ration conflict").
11. Bound history storage: keep only the 5 most recent significant relationship events per pair to prevent memory bloat.
12. Ensure pair history events persist cleanly within `SurvivorRelationsSaveStore` without format regressions.
13. Update `SurvivorDetailPanel.cs` and `RelationsPanel.cs` to present explainable relation history lines.
14. Expose active pairing bonuses in `DutyRosterPanel.cs` when co-assigning compatible survivors to the same facility.
15. Add unit tests in `Ashfall.Core.Tests/Survivors/Plan44RelationshipEffectsTests.cs`.
16. Validate that identical campaign seeds produce identical social bond formations across identical work assignments.
17. Verify that survivor death triggers deep grief in surviving bonded pairs, routing through Plan 24 mourning actions.
18. Run `--panel-bind-lifecycle-selftest` to ensure relations presentation panels dispose subscriptions cleanly.
19. Run survivor and duty focused test suites to confirm zero regressions in social coordinator updates.
20. Document inner-life loop mechanics in `docs/systems/SURVIVOR_RELATIONSHIP_EFFECTS.md` and complete handoff.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### D1.1 Bounded Pair History Ledger Implementation
- (a) Define record `PairHistoryEntry` containing campaign day, event kind, affinity delta, and localized reason key.
- (b) Author `PairHistoryLedger` managing circular FIFO buffers capped at 5 entries per unique survivor pair key.
- (c) Record entries upon significant social interactions (combat rescue, shared crisis, meal sharing, brawl).
- (d) Author unit tests asserting that the 6th event cleanly evicts the oldest entry without memory leaks.

#### D1.2 Shared Duty Synergy & Friction Multipliers
- (a) Query pair affinity in `DutyRosterAssignmentEngine` when multiple survivors are assigned to the same room.
- (b) Apply synergy bonuses (+10% to +20% output) when bonded pairs work in tandem in Garden, Workshop, or MedBay.
- (c) Apply friction penalties and occasional brawling incidents when hostile survivors share enclosed workspaces.
- (d) Author unit tests proving that separating feuding workers eliminates workplace morale penalties.

#### D1.3 Expedition Psychological Mutual Aid
- (a) Evaluate expedition party pair affinities in `ExpeditionHostSession` during stress and combat encounters.
- (b) Allow bonded companions to absorb or mitigate panic effects when one member suffers a psychological trauma check.
- (c) Increase mutual affinity upon successful completion of dangerous, multi-day wasteland expeditions.
- (d) Author unit tests validating that bonded expedition squads experience lower average trauma gain.

#### D1.4 Explainable Social UI Projection
- (a) Update `RelationsPanel.cs` to render the pair history timeline beneath the mutual affinity bar.
- (b) Render actionable synergy indicators in `DutyRosterPanel.cs` when selecting assignment candidates.
- (c) Ensure high-contrast visual formatting and keyboard focus traversal across relation detail cards.
- (d) Run `--ui-a11y-selftest` to ensure relationship presentation surfaces pass all accessibility standards.

---

## TASK D2 — C2[20] Reproducible Balance Evidence, Playable Metrics & Difficulty Decisions (Plan 46A/46B/46C)

- **Source Plan:** `C-integration-plans/C2_planintegration[20].md` (Plan 46)
- **Blocker Class:** ARTIFACT PROVENANCE GAP / UNATTRIBUTED BALANCE CLAIMS
- **Canonical Owner:** `scripts/balance/`, `docs/balance/`
- **Target Subsystem:** Seeded balance simulation runner, metric output schema, difficulty presets, decision linkage

### 20 Procedural Substeps:
1. Review current balance artifacts in `artifacts/` and `docs/balance/` to inventory existing CSV outputs.
2. Identify the core defect: balance artifacts exist without explicit scenario, seed, policy, or git commit attribution.
3. Claim `scripts/balance/run_balance_sweep.py` and `Assets/Ashfall.Core/Balance/BalanceSimulationDriver.cs`.
4. Author `BalanceSimulationDriver.cs` executing headless, multi-day simulation scenarios with full metadata provenance.
5. Output structured simulation records: git commit SHA, scenario name, seed value, campaign length, and metrics table.
6. Instrument daily survival metrics: food runway, water runway, average radiation dose, survivor morbidity, mortality rate.
7. Instrument economic metrics: merchant turnover, scrap liquidity, trade embargo impact duration, inflation index.
8. Establish canonical difficulty presets in `difficulty_presets.json` (Story, Standard, Hardcore, Apocalyptic).
9. Ensure difficulty presets modify only declared multiplier fields (consumption rate, hazard frequency, recovery curve).
10. Implement difficulty integrity validation: difficulty presets must never alter core rules or bypass safety invariants.
11. Implement local, private, opt-in play session metrics recording player action frequencies and first-day survival rates.
12. Ensure zero network telemetry: all metrics are strictly written to local files in `user://telemetry/` or console output.
13. Ensure metrics records contain zero player identity, machine usernames, or privacy-sensitive file paths.
14. Add automated CI gate `scripts/ci/balance_reproducibility_check.sh` ensuring golden runs match committed reference CSVs.
15. Add unit tests in `Ashfall.Core.Tests/Balance/Plan46BalanceSweepTests.cs` verifying simulation driver determinism.
16. Validate that identical seed and preset inputs produce byte-identical metric output files across multiple runs.
17. Verify that balance simulations run headless via `godot --headless` or `dotnet test` in under 60 seconds.
18. Run `--data-integrity-selftest` to ensure `difficulty_presets.json` validates against catalog schema policy.
19. Re-run all existing balance simulation suites to confirm zero regressions in needs or radiation balance.
20. Document balance evidence procedures in `docs/balance/BALANCE_EVIDENCE_METHODOLOGY.md` and complete handoff.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### D2.1 Attributed Simulation Header & Provenance Schema
- (a) Define metadata header format: `build_sha`, `timestamp_utc`, `preset_id`, `rng_seed`, `simulation_days`.
- (b) Implement output serializing both machine-readable JSON and human-readable summary Markdown.
- (c) Validate that missing commit SHA or uncommitted git dirty status is explicitly flagged in simulation logs.
- (d) Author verification script asserting that no un-attributed balance CSV files exist in the repository.

#### D2.2 Standardized Survival Metric Instrumentation
- (a) Instrument `DailyBriefingReportBuilder` to aggregate daily calorie intake, water liters, and rad rem consumption.
- (b) Instrument `MedicalWardSystem` to record active bed occupancy rates and medical supply depletion curves.
- (c) Aggregate daily statistics into summary vectors: PeakMortalityDay, StarvationOnsetDay, EconomicStagnationIndex.
- (d) Author unit tests validating that simulated crisis conditions produce correct metric telemetry output.

#### D2.3 Canonical Difficulty Presets Architecture
- (a) Structure `difficulty_presets.json` defining named presets with explicit scalar multipliers.
- (b) Bind active preset in `GameBootstrap` and propagate multipliers through `WorldHostSession`.
- (c) Ensure difficulty multipliers clamp within sane boundaries (0.5x minimum, 2.5x maximum).
- (d) Author unit tests proving that switching difficulty adjusts multipliers without corrupting base data catalogs.

#### D2.4 Reproducibility CI Gate
- (a) Script `scripts/ci/verify_balance_golden.sh` running a 30-day headless simulation under fixed seed 424242.
- (b) Compare output metrics table against `artifacts/golden_balance_30day.json` using exact diff comparison.
- (c) Fail CI if unintended balance drift occurs; require deliberate golden file rebaselining with justification.
- (d) Verify the balance gate executes in under 15 seconds during continuous integration runs.

---

## TASK D3 — C2[21] Version Contracts, Release Tagging, Artifact Provenance & Save-Safe Hotfixes (Plan 48A/48B/48C)

- **Source Plan:** `C-integration-plans/C2_planintegration[21].md` (Plan 48)
- **Blocker Class:** RELEASE DISCIPLINE / COMPATIBILITY HAZARD
- **Canonical Owner:** `scripts/release/`, `docs/release/`
- **Target Subsystem:** Release automation, version tagging, export verification, save compatibility hotfix path

### 20 Procedural Substeps:
1. Review current version declarations across `project.godot`, `Ashfall.csproj`, `version.json`, and CI configs.
2. Confirm the core objective: establish one unified version authority, release tag pipeline, and save-safe hotfix path.
3. Claim `scripts/release/cut_release.py`, `scripts/release/verify_release_artifact.sh`, and `docs/release/`.
4. Create single version source of truth in `version.json` specifying Major, Minor, Patch, and PreRelease identifiers.
5. Implement `scripts/release/sync_versions.py` updating Godot, .NET, and documentation versions automatically.
6. Establish semantic versioning rules: Breaking Save Format = Major; Additive Systems = Minor; Fixes = Patch.
7. Enforce the hotfix rule: patch releases must NEVER modify save schemas or introduce breaking data changes.
8. Create `scripts/release/cut_release.py` automating git tag creation (`vX.Y.Z`), changelog extraction, and release manifest.
9. Implement machine-readable release manifest `artifacts/release_manifest.json` listing asset hashes and build commit.
10. Implement artifact smoke runner `verify_release_artifact.sh` executing headless boot of exported Linux/Windows binaries.
11. Verify that exported binary successfully boots, mounts PCK, loads JSON data, and advances campaign day 0 to 1.
12. Establish save compatibility test harness: verify that saves generated on `vX.Y.Z` load cleanly on `vX.Y.(Z+1)`.
13. Implement automated changelog generation extracting commit messages between release tags grouped by semantic type.
14. Ensure release scripts verify that working directory is clean and all CI gates pass before permitting tag creation.
15. Add unit tests in `Ashfall.Core.Tests/Release/Plan48ReleaseContractTests.cs` validating version string parsing.
16. Validate that save stores declare compatibility ranges matching active game version boundaries.
17. Verify that rollback procedures can restore previous release tags cleanly without corrupting user profile saves.
18. Run `--data-integrity-selftest` to ensure version metadata catalogs validate without errors.
19. Run export smoke selftest ensuring exported packages contain all required `StreamingAssets/Data` catalogs.
20. Document release engineering guidelines in `docs/release/RELEASE_PROCEDURE_GUIDE.md` and complete handoff.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### D3.1 Unified Version Source of Truth & Synchronization
- (a) Author `version.json` declaring `major`, `minor`, `patch`, `build_number`, and `version_string`.
- (b) Implement `sync_versions.py` updating `project.godot` (`config/version`), `Ashfall.csproj` (`<Version>`), and docs.
- (c) Author a CI check failing if any version declaration drifts from `version.json`.
- (d) Verify in test harness that `GameBootstrap` reads and logs the identical version string at boot.

#### D3.2 Automated Tagging & Changelog Pipeline
- (a) Author `scripts/release/cut_release.py` accepting `--type (major|minor|patch)` and updating versions.
- (b) Parse conventional git commit messages (`feat:`, `fix:`, `refactor:`) since previous tag to build changelog.
- (c) Create annotated signed git tag with changelog release notes following repository security policies.
- (d) Validate that release tagging rejects uncommitted changes or failing local test suites.

#### D3.3 Exported Artifact Headless Smoke Test
- (a) Author `scripts/release/verify_release_artifact.sh` targeting exported Godot engine executable.
- (b) Execute binary with `--headless -- --data-integrity-selftest` asserting exit code 0.
- (c) Execute binary with `--headless -- --7-day-smoke` asserting successful campaign initialization and progression.
- (d) Verify that PCK packaging includes all authoritative JSON catalogs and audio cue streams without missing files.

#### D3.4 Cross-Version Save Compatibility Proof
- (a) Maintain a fixture directory of golden saves generated from each minor release version.
- (b) Execute automated compatibility test loading legacy saves into current engine build.
- (c) Assert that all legacy survivors, inventories, and base facilities restore with zero data corruption.
- (d) Document safe hotfix rollback steps ensuring player saves survive emergency version rollbacks.

---

## TASK D4 — C2[22] Asset Truth, Explicit ID-to-File Mapping & Screen Visual Proof (Plan 50A/50B/50C)

- **Source Plan:** `C-integration-plans/C2_planintegration[22].md` (Plan 50)
- **Blocker Class:** CONVENTION GUESSING / ASSET AMBIGUITY GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Assets/`, `artifacts/`
- **Target Subsystem:** Explicit ID-to-asset mapping, fallback separation, orphan classification, visual screen proof

### 20 Procedural Substeps:
1. Review current asset management: `artifacts/asset_registry.json`, `AssetRegistry.cs`, and `generate-asset-registry.py`.
2. Confirm the core defect: game code uses convention-based filename guessing instead of explicit catalog mapping.
3. Claim `Assets/Ashfall.Core/Assets/AssetCatalog.cs` and `scripts/ci/generate-asset-registry.py`.
4. Author `AssetCatalog.cs` as the authoritative registry mapping semantic asset IDs to physical repository paths.
5. Separate fallback from success: queries returning fallback placeholder textures must explicitly flag `IsFallback = true`.
6. Enforce explicit mapping: every item in `items.json` must explicitly specify an `icon_id` declared in the asset catalog.
7. Enforce explicit audio mapping: every audio cue in `audio_cues.json` must resolve to real `.wav`/`.ogg` audio streams.
8. Audit all files in `assets/` to identify unmapped orphan assets and classify them: Active, Reserved, or Obsolete.
9. Generate machine-readable asset coverage report: calculate exact percentage of explicit versus fallback resolutions.
10. Update `AssetRegistry.cs` to eliminate silent string concatenation paths (e.g. `icon_path = "icons/" + id + ".png"`).
11. Implement strict validation in `CatalogIntegrityValidator.cs` verifying that all authored asset IDs resolve to disk files.
12. Extend snapshot test harness in `SnapshotHarness.cs` to capture fully populated screens rendering resolved assets.
13. Capture deterministic snapshots for all 28+ player-facing panels with fully populated data fixtures.
14. Ensure snapshot diff tests fail if an explicit asset inadvertently degrades into a generic fallback icon.
15. Verify that missing assets trigger clear editor/runtime warnings without halting game simulation execution.
16. Add unit tests in `Ashfall.Core.Tests/Assets/Plan50AssetTruthTests.cs` verifying explicit lookup performance.
17. Ensure asset catalog queries execute with O(1) complexity and zero memory allocations on cache hits.
18. Run `--data-integrity-selftest` and confirm that all asset mappings pass with 0 missing file errors.
19. Run `--audio-selftest` to ensure all 196+ audio cues resolve to explicit valid asset streams.
20. Update `artifacts/asset_registry.md` and `docs/assets/ASSET_TRUTH_MAP.md` and complete handoff.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### D4.1 Explicit Asset Catalog Authority
- (a) Define data schema `assets.json` declaring `id`, `category` (Icon, Texture, Audio, Font), `path`, `fallback_id`.
- (b) Parse `assets.json` into immutable `AssetCatalog` loaded during early engine initialization.
- (c) Provide typed lookup API `AssetCatalog.Resolve(string assetId)` returning path and resolution status.
- (d) Author unit tests proving that queries for non-existent IDs return explicit typed fallback records.

#### D4.2 Item & Audio Explicit Binding Sweep
- (a) Update `items.json` to replace missing or convention-inferred icons with explicit `icon_id` strings.
- (b) Update `audio_cues.json` to ensure every audio cue explicitly declares its primary and alternative file paths.
- (c) Remove legacy heuristic path formatters from UI panels and audio players.
- (d) Author integrity tests verifying that 100% of required gameplay items declare valid explicit asset IDs.

#### D4.3 Orphan Asset Classification & Hygiene
- (a) Script a scanner comparing disk files in `assets/` against all declared IDs in `assets.json`.
- (b) Classify unreferenced files into `docs/assets/ORPHAN_ASSET_CLASSIFICATION.md` (Keep-Reserved vs Safe-Archive).
- (c) Move abandoned prototype assets to `docs/archive/assets/` without breaking Godot `.import` linkages.
- (d) Validate that running `--data-integrity-selftest` after orphan pruning produces zero missing asset warnings.

#### D4.4 Populated Snapshot Verification Battery
- (a) Update `src/UI/SnapshotHarness.cs` with deterministic, fully populated test fixtures for all 28 covered surfaces.
- (b) Render and save golden reference images to `snapshots/` under fixed 1920x1080 resolution.
- (c) Implement image comparison tests asserting zero pixel drift on layout or text rendering across builds.
- (d) Document snapshot rebaselining protocol requiring manual review before accepting visual changes.

---

# 2. CROSS-TASK VERIFICATION MATRIX

| Task | Domain | Focused Test Suite | Build Gate | Data Integrity | Replay / Determinism | UI / Audio Lifecycle | Handoff Target |
|---|---|---|---|---|---|---|---|
| **C1** | Expedition Continuity | `Plan133ExpeditionDiscoveryTests.cs` | Required (0/0) | Required (333 catalogs) | Seeded discovery drift | N/A | `WORLD_DISCOVERY.md` |
| **C2** | Needs Cascade | `Plan137NeedsPerformanceCascadeTests.cs` | Required (0/0) | Unchanged | Pure math determinism | `--panel-bind-lifecycle` | `NEEDS_CASCADE.md` |
| **C3** | Combat Diplomacy | `Plan139CombatFactionBridgeTests.cs` | Required (0/0) | Required (factions.json) | Incident idempotency | `--ui-a11y-selftest` | `COMBAT_FACTIONS.md` |
| **C4** | Medical Capabilities | `Plan143AfflictionCapabilityTests.cs` | Required (0/0) | Required (afflictions.json)| Pure capability query | `--panel-bind-lifecycle` | `CAPABILITY_GATING.md` |
| **D1** | Relationship Effects | `Plan44RelationshipEffectsTests.cs` | Required (0/0) | Unchanged | Seeded social affinity | `--ui-a11y-selftest` | `RELATIONSHIPS.md` |
| **D2** | Reproducible Balance | `Plan46BalanceSweepTests.cs` | Required (0/0) | Required (presets.json) | 30-day golden match | Headless run <60s | `BALANCE_EVIDENCE.md` |
| **D3** | Release Discipline | `Plan48ReleaseContractTests.cs` | Required (0/0) | Required (version.json) | Cross-version save proof | Export smoke test | `RELEASE_GUIDE.md` |
| **D4** | Asset Truth | `Plan50AssetTruthTests.cs` | Required (0/0) | Required (assets.json) | O(1) lookup perf | `--audio-selftest` | `ASSET_TRUTH_MAP.md` |

---

# 3. TASK HANDOFF TEMPLATE

Every completed task must publish a handoff document adhering to this structure:

```markdown
## TASK <ID> HANDOFF: <TITLE>

### 1. Workspace & Authority
- Commit / HEAD tested:
- Claimed paths in `WORKTREE_OWNERSHIP.md`:
- Concurrent claims checked:

### 2. Premise Re-Verification
- Historical blocker statement:
- Verified state at execution start:
- Stale assumptions corrected:

### 3. Decisions & Signatures
- Signed decisions consumed:
- Deferred sub-scopes recorded:

### 4. Implementation Details
- Canonical authority modified:
- Old path / New path:
- Files created / modified / deleted:
- Generated assets regenerated:

### 5. Verification & Testing
- Focused test command & exact results:
- Headless selftest results (`--data-integrity`, `--audio`, `--ui-a11y`):
- Determinism / Replay fingerprint results:
- Build output (Errors: 0, Warnings: 0):

### 6. Ledger & Documentation Updates
- `INTEGRATION_PLANS.md` status:
- Architecture / Catalog maps updated:
- Next unblocked dependency:
```

---

# 4. WAVE-LEVEL FAILURE & STOP POLICY

Stop execution and escalate to the foreman/user immediately if any of the following occur:
1. **Missing Domain Owner:** A task requires behavior that lacks an existing canonical authority, prompting the creation of an unauthorized manager or UI-side calculation.
2. **Signature Boundary:** A balance multiplier, permanent deferral, or breaking contract requires approval and lacks a signed record.
3. **Claim Overlap:** A path required for implementation is actively claimed in `WORKTREE_OWNERSHIP.md`.
4. **Premise Invalidation:** Code inspection proves that the problem was already resolved or that current code contradicts the plan's architectural premise.
5. **Determinism Break:** Seeded simulations diverge across runs or save/load cycles.
6. **Engine Infiltration:** Godot or engine assemblies leak into `Assets/Ashfall.Core/`.

---

# 5. DEFINITION OF DONE FOR WAVE 12 PART 2

Wave 12 Part 2 is complete only when all eight tasks have achieved an allowed terminal state (`IMPLEMENTED` or `DECIDED-DEFERRED`), verified by:
- All 8 tasks have executed all 20 procedural substeps and structured mini-tasks without omissions.
- `dotnet build Ashfall.csproj` completes with **0 Errors and 0 Warnings**.
- `godot --headless --path . -- --data-integrity-selftest` passes with **0 Errors across 333+ catalogs**.
- `godot --headless --path . -- --audio-selftest` passes with **0 Failures**.
- `godot --headless --path . -- --panel-bind-lifecycle-selftest` and `--ui-a11y-selftest` pass cleanly.
- Golden balance simulations match committed reference baselines under fixed seeds.
- All modified markdown files and documentation indices are synchronized and free of trailing whitespace.
