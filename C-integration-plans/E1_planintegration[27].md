---
PLAN_ID: E1-27
PLAN_FAMILY: planintegration
SEQUENCE_INDEX: 27
STATUS: READY_FOR_EXECUTION_AFTER_PLAN50_51_AUTHORITY_RECON
SOURCE_PLAN: "Plan 50/51 Follow-up — Garage UI, Vehicle Terrain Physics, Counter-Intelligence UI, Faction Retaliation"
SEQUENCE_FILENAME: "E1_planintegration[27].md"
PREVIOUS_FILENAME: "E1_planintegration[26].md"
NEXT_FILENAMES:
  - "E1_planintegration[28].md"
  - "E1_planintegration[29].md"
CATEGORY: UI+VEHICLES+EXPEDITIONS+ESPIONAGE+FACTIONS+COMBAT
PREMISE_VERIFICATION_REQUIRED: true
ONE_AUTHORITY_PER_FACT: true
SECOND_INVENTORY_LEDGER_FORBIDDEN: true
SECOND_MAINTENANCE_RESOLVER_FORBIDDEN: true
SECOND_ROUTE_PHYSICS_ENGINE_FORBIDDEN: true
SECOND_FUEL_LEDGER_FORBIDDEN: true
SECOND_RADIATION_ENGINE_FORBIDDEN: true
SECOND_RAID_RESOLVER_FORBIDDEN: true
SECOND_SECURITY_SUSPICION_GRAPH_FORBIDDEN: true
SECOND_HIDDEN_TRUTH_UI_MODEL_FORBIDDEN: true
SECOND_INTELLIGENCE_CURRENCY_FORBIDDEN: true
RNG_MUST_BE_KEYED_AND_PARTITIONED: true
UI_MUST_BE_READ_MODEL_PLUS_COMMANDS: true
RUNTIME_RISK: VERY_HIGH
SAVE_RISK: VERY_HIGH
DETERMINISM_RISK: VERY_HIGH
---

# E1 Plan Integration [27] — Garage UI, Vehicle Terrain Physics, Counter-Intelligence Dashboard, and Faction Retaliation Operations

> **Sequence rule:** this file is `E1_planintegration[27].md`. The next file is `E1_planintegration[28].md`.

## 0. Mission

This plan integrates four Plan 50/51 follow-ups:

1. GarageDetailPanel and workshop interaction.
2. Overland terrain, route wear, fuel, radiation, breakdown, and recovery.
3. Counter-intelligence UI and DailyBriefing integration.
4. Faction retaliation, sleeper extraction, double-agent operations, dead drops, and archive-intel rewards.

The implementation standard is:

**UI reads canonical projections and dispatches commands; route/world data defines terrain; VehicleGarage defines
vehicle-specific response; Expedition owns movement; fuel and radiation retain their canonical owners;
ShelterEspionage owns covert state; PerimeterDefense, Combat, Inventory, Faction, Journal, and Archive own their
respective consequences.**

The main failure modes to prevent are:

- Garage UI mutating inventory or wear directly.
- VehicleGarage and Expedition both applying the same terrain/fuel modifier.
- Planning fuel costs differing from runtime without an explicit cause.
- Vehicle shielding reducing dose in two different code paths.
- Breakdown generating duplicate recovery missions after reload.
- Espionage UI exposing actual sleeper/faction/loyalty truth before discovery.
- Counter-intelligence trend becoming a second suspicion/standing number.
- Extraction raids resolving combat or ammo inside ShelterEspionage.
- Double-agent daily risk rerolling on reload.
- Poisoned caches bypassing canonical contaminated-item/health systems.
- Cipher-fragment rewards creating a second archive currency.

## 1. Canonical Ownership Matrix

| Concern | Canonical owner | E1-27 role |
|---|---|---|
| Vehicle modification state | VehicleGarageSystem | query/command |
| Vehicle component wear | VehicleGarageSystem | mutate through canonical APIs |
| Garage UI | GarageDetailPanel | presentation + commands |
| Inventory/resource costs | Inventory / IPlayerInventoryPort | affordability + transactions |
| Maintenance servicing | VehicleGarage/Maintenance authority | atomic service operation |
| Terrain classification | route/world data | read |
| Expedition progression | ExpeditionSystem | consume modifiers |
| Fuel planning/runtime | ExpeditionResourceCostCalculator | consume multiplier |
| Crew radiation dose | Radiation/Health | consume shielding |
| Hazard collision | Expedition hazard owner | consume vehicle protection |
| Breakdown state | VehicleGarage + Expedition handoff | one incident |
| Recovery mission | canonical recovery/expedition authority | create/dispatch |
| Espionage secret truth | ShelterEspionageSystem | own |
| Player-known evidence | espionage/knowledge presentation layer | expose safely |
| Security posture | ShelterSecurity / guard systems | source input |
| Faction hostility/standing | faction authority | source/consequence |
| Perimeter breach | PerimeterDefense | resolve |
| Combat | CombatSystem | resolve |
| Ammo | Inventory/Combat | consume |
| Dead-drop location | world/espionage object | reference |
| Journal | JournalSystem | presentation/history |
| Cipher fragments | PrewarArchiveDecryptionSystem | canonical grant |
| Visible text | translation/localization catalog | own |

## 2. Cross-Cutting Invariants

- Every mutating player action uses a stable operation ID.
- UI never writes simulation fields directly.
- UI does not bind raw secret espionage DTOs.
- Derived projections are rebuilt from canonical state and are not save authority.
- Routine route modifier math is deterministic and preferably fixed-point/permille.
- RNG is used only for actual stochastic simulation and is keyed by stable context.
- Save/load must not replay costs, raids, breakdowns, rewards, or journal entries.
- One canonical action consumes each resource exactly once.
- Every new catalog ID must have a runtime consumer.
- Every new UI string uses localization.
- Empty/disabled states remain valid.
- All first slices must run headlessly.

## 3. Delivery Order

### Phase 1 — Recon and contracts
Complete ownership audit, presentation DTOs, command contracts, terrain authority, and espionage epistemic boundary.

### Phase 2 — Garage UI shell
Scene, contract, slots, gauges, projections, empty state, navigation, selftest.

### Phase 3 — Garage mutations
Install, uninstall, Service All, recovery dispatch, audio, transaction tests.

### Phase 4 — Terrain integration
Terrain tags, route resolution, wear/speed/fuel/radiation, breakdown, recovery, deterministic tests.

### Phase 5 — Espionage UI
Faction sub-tab, suspect rows, interrogation/flip commands, dead-drop tracker, DailyBriefing, localization.

### Phase 6 — Retaliation
Extraction scenarios, raid handoffs, disinformation, double-agent risk, poisoned cache, archive grants.

### Phase 7 — Hardening
Save migration, content utilization, 60-day lifecycle, snapshots, scene lint, long-run soak, docs.

---

## E1-27A — Repository recon and duplicate-authority map

1. Audit VehicleGarageSystem, VehicleGarageState, modification catalog, Inventory ports, maintenance APIs, ExpeditionSystem, route graph, locations terrain, ResourceCostCalculator, Radiation/crew dose, Stealth, hazards, recovery mission state, Journal, and Plan 50 UI patterns.
2. Audit FactionDetailPanel, DailyBriefingModal, ShelterEspionageSystem/state/save store, PerimeterDefense, Combat, Faction authority, Journal, PrewarArchiveDecryption, localization, catalogs, UI selftests, and Plan 51 docs.
3. Search for duplicate terrain, wear, fuel, shielding, suspicion, retaliation, raid, dead-drop, and cipher reward logic.
4. Create `docs/forensics/E1_27_PLAN50_51_AUTHORITY_RECON.md`.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27B — Garage panel ownership contract

1. Document GarageDetailPanel as presentation/read-model + command surface only.
2. List every displayed field and canonical owner.
3. List every allowed command and service dependency.
4. Prohibit authoritative UI caches.
5. Add `GarageUiAuthorityBoundaryTests`.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27C — Garage scene shell

1. Create `src/UI/GarageDetailPanel.cs` inheriting `PanelContainer` and implementing `IContractPanel`.
2. Create `assets/ui/panels/GarageDetailPanel.tscn` for 1920x1080 fixed viewport.
3. Follow current scene-lint container/anchor/margin rules.
4. Support loading, empty, active-vehicle, and error states.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27D — Garage node contract

1. Define required nodes for slot selector, gauges, install/uninstall, Service All, recovery list, stat projections, costs, and empty state.
2. Use checked node binding helpers consistent with current UI framework.
3. Fail headless selftest with actionable node-path/type diagnostics.
4. Version the panel contract dictionary if the framework supports it.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27E — Vehicle slot selector

1. Render Cargo, Protection, Mobility, Engine, Utility from canonical slot definitions when available.
2. Use AshfallUiHelpers for item/mod badge previews.
3. Show installed mod, compatible choices, and blocker reason.
4. Keep selection separate from installation command.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27F — Garage compatibility projection

1. Query VehicleGarageSystem for compatible modifications.
2. Return wrong-slot, prerequisite, exclusivity, missing-vehicle, and unavailable-item blockers.
3. Do not reimplement compatibility rules in UI.
4. Localize all labels.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27G — Analog condition gauges

1. Bind Chassis Stress, Engine Fouling, and Transmission Wear to AnalogConditionGauge.
2. Normalize permille semantics consistently.
3. Provide numeric tooltip plus severity text.
4. Cover 0, mid, degraded, critical, and 1000 thresholds in tests.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27H — Vehicle stat projection

1. Display net cargo capacity and delta, speed multiplier delta, fuel efficiency, and radiation protection.
2. Expose a canonical reason breakdown from installed modifications.
3. Do not calculate simulation math inside panel.
4. Refresh projections only on relevant state changes.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27I — Install modification

1. Dispatch canonical install command through VehicleGarageSystem.
2. Use IPlayerInventoryPort for affordability/transaction handling.
3. Show exact resource-cost tooltip before commit.
4. Use stable operation ID and disable double-submit.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27J — Uninstall modification

1. Dispatch canonical uninstall command.
2. Return real components/items to Inventory or canonical salvage output.
3. Do not invent salvage values in UI.
4. Handle capacity/full-inventory failures transactionally.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27K — Service All preview

1. Ask canonical maintenance/garage authority for all serviceable components and total cost.
2. Show line-item scrap/mechanical-part requirements.
3. Ensure preview version/state can be invalidated if inventory/vehicle changes.
4. Do not calculate repair cost locally.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27L — Service All atomic commit

1. Implement one atomic batch service transaction.
2. Reserve required resources once.
3. Apply all component service outcomes or roll back all mandatory changes.
4. Emit one coherent state-change event.
5. Add partial-failure and reload tests.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27M — Recovery mission UI

1. Show stranded vehicles, canonical location/coordinates, route distance, required fuel, towing requirements, and ETA ticks.
2. Use world route + ExpeditionResourceCostCalculator.
3. Dispatch through canonical recovery expedition command.
4. Handle no-route/no-fuel/already-active states.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27N — Garage empty state

1. Show explicit placeholder when no recovered expedition vehicle exists.
2. Disable install/service controls.
3. Do not fabricate a starter vehicle.
4. Link to a real salvage/recovery route only if one exists.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27O — Garage audio

1. Trigger `acue_door_bulkhead_seal` on committed bay-toggle presentation event.
2. Trigger ratchet cue after successful installation commit.
3. Use existing audio cue registry.
4. Respect mute/reduced-stimulation settings.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27P — Garage navigation registration

1. Bind in `src/Main.UiPanels.cs` using existing panel registry.
2. Add route hotkey and top navigation tab using existing router conventions.
3. Do not add a second navigation system.
4. Ensure subscriptions detach when panel closes/rebinds.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27Q — Garage Bind and lifecycle

1. Implement `Bind(VehicleGarageSystem, IPlayerInventoryPort)`.
2. Make rebind idempotent.
3. Subscribe only to relevant garage/inventory events.
4. Dispose subscriptions on campaign/panel teardown.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27R — Garage reactive refresh budget

1. Coalesce slot/gauge/stat/cost/recovery refresh after atomic operations.
2. Avoid `_Process` polling.
3. Add a test for refresh storms.
4. Keep disposable UI caches non-authoritative.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27S — Garage selftest verb

1. Add `--garage-panel-selftest` following current selftest verb conventions.
2. Load scene headlessly and verify IContractPanel.
3. Validate all required nodes/types.
4. Bind fixture services and exercise empty + populated vehicle states.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27T — Garage snapshot and scene lint

1. Add `Ashfall.Core.Tests/UI/` snapshot coverage.
2. Cover empty, normal, critical-wear, and recovery states.
3. Run `scene-lint.py` against GarageDetailPanel.tscn.
4. Use repository-standard structural/layout snapshots rather than brittle custom pixel tooling.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27U — Terrain affinity catalog

1. Extend `vehicle_modifications.json` with typed mud/rubble/highway/dunes affinity data.
2. Validate tags and ranges.
3. Keep values physically explainable and consumer-owned.
4. Add catalog integrity coverage.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27V — Terrain authority ADR

1. Determine exact canonical terrain source among route edges, sectors, locations, and fallback classifications.
2. Define precedence when sources disagree.
3. Treat weather/storm wetness as overlay, not rewritten terrain.
4. Create `docs/architecture/VEHICLE_TERRAIN_AUTHORITY.md`.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27W — Terrain modifier calculation

1. Add canonical terrain wear and speed multiplier calculation.
2. Prefer fixed-point/permille math.
3. Inputs: vehicle mods, component state, terrain, load, and canonical weather overlay.
4. Return a reason breakdown for tests/UI.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27X — No duplicate terrain application

1. VehicleGarage computes vehicle response; Expedition applies progression.
2. World defines terrain; Weather defines wetness/storm context.
3. Prevent any second terrain penalty in ExpeditionResourceCostCalculator or hazard code.
4. Add authority-boundary assertions.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27Y — Route segment terrain resolution

1. Resolve terrain for the actual traversed edge/segment.
2. Do not infer terrain from destination name.
3. Provide deterministic legacy fallback.
4. Create paved, mud, rubble, and dunes fixtures.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27Z — Expedition transit integration

1. Query one vehicle modifier result per transit step/segment.
2. Pass route/terrain/weather/load context.
3. Apply speed/wear/fuel/shielding outputs through their canonical owners.
4. Do not persist derived modifier results as route truth.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27AA — Chassis stress integration

1. Apply distance/terrain wear through VehicleGarage component state.
2. Clamp to documented range.
3. Do not double-add collision damage.
4. Add highway vs rubble/mud tests.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27AB — Engine fouling integration

1. Apply dust/mud/storm context only where current component model supports fouling.
2. Avoid double-counting generic terrain wear.
3. Document exact inputs and formula.
4. Add clean-highway/dunes/storm tests.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27AC — Transmission wear integration

1. Apply traction/load/terrain stress through one formula.
2. Include heavy cargo/flatbed contribution once.
3. Use deterministic fixed-point arithmetic.
4. Add light/heavy-load boundary tests.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27AD — Terrain speed integration

1. Expedition progression consumes canonical speed multiplier.
2. Keep route distance unchanged.
3. Apply safe floor/cap unless explicit immobilized state exists.
4. Use same projection for ETA.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27AE — Fuel planning/runtime integration

1. Feed terrain/vehicle fuel multiplier into ExpeditionResourceCostCalculator.
2. Planning and runtime must share formula version and units.
3. Track route/load/weather state changes as explicit estimate invalidators.
4. Add planned-vs-consumed reconciliation tests.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27AF — Fuel conservation boundary

1. VehicleGarage returns multiplier only; it owns no fuel balance.
2. Fuel is consumed by canonical Expedition/Inventory pathway.
3. Recovery mission planning uses identical fuel units.
4. Add no-free-fuel and no-double-charge tests.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27AG — Bullbar rubble collision adapter

1. Apply `vmod_reinforced_bullbar` only to qualifying rubble/collision hazard damage.
2. Hazard owner supplies impact magnitude/type.
3. VehicleGarage supplies bounded absorption.
4. Do not grant generic chassis invulnerability.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27AH — Expanded flatbed heavy-terrain penalty

1. Apply mud/slurry penalty only when installed flatbed/load/context justify it.
2. Use Weather/Climate wetness/storm state rather than hard-coded season strings where possible.
3. Any sink/immobilization event belongs to canonical hazard/event system.
4. Add dry/wet and light/heavy-load tests.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27AI — Vehicle radiation shielding

1. Expose vehicle radiation protection permille through typed shielding adapter.
2. Radiation/crew-dose system applies eligible external-dose reduction.
3. Do not reduce internal contamination or already accumulated dose.
4. Ensure Stealth cannot become a second dose owner.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27AJ — Crew dose integration test

1. Run identical route with protected and unprotected vehicle.
2. Assert dose owner receives one shielding modifier.
3. Verify no double stacking.
4. Verify deterministic per-crew results.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27AK — Catastrophic breakdown transition

1. At 1000 permille component wear, create one stable breakdown incident.
2. Persist breakdown before journal/recovery side effects.
3. Expedition halts/immobilizes vehicle according to canonical rule.
4. Prevent repeated incident generation on later ticks.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27AL — Breakdown location provenance

1. Capture canonical route node/edge/coordinates.
2. Do not create UI-only map coordinates.
3. Persist enough location identity for recovery after reload.
4. Add mid-edge and node fixtures.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27AM — Recovery mission creation

1. Create VehicleRecoveryMission exactly once from breakdown incident ID.
2. Reference stranded vehicle, location, tow requirements, fuel estimate, and status.
3. Do not recreate on restore.
4. Add mission-idempotency test.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27AN — Recovery towing test

1. Dispatch recovery through Expedition.
2. Validate towing vehicle/team/fuel/route.
3. On success return same vehicle to garage.
4. Apply post-recovery damaged state according to an explicit component wear policy.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27AO — 800-permille recovery ADR

1. Resolve whether 800 permille applies to catastrophic component, all components, chassis, or a derived recovery state.
2. Do not flatten distinct component wear silently.
3. Document chosen semantics before locking tests.
4. Update recovery fixture accordingly.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27AP — Breakdown journal entry

1. Publish one localized emergency journal entry after breakdown commit.
2. Reference canonical recovery location as player-known.
3. Use incident ID for dedupe.
4. Journal must not own recovery state.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27AQ — Vehicle deterministic replay

1. Run paired identical-seed sorties.
2. Compare wear, fuel, ETA, shielding inputs, breakdown incident, recovery mission, and journal IDs.
3. Use partitioned RNG only for genuinely stochastic hazards.
4. Add call-order perturbation test.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27AR — VehicleExpeditionTerrainTests

1. Create muddy vs paved wear differential test.
2. Add rubble/highway, dunes, storm-flatbed, bullbar, fuel, shielding, and threshold breakdown fixtures.
3. Keep expected values diagnostic.
4. Verify same calculator powers planning and runtime.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27AS — Architecture test map update

1. Update `docs/architecture/ARCHITECTURE_TEST_MAP.md` Section 2 with `vehicle_terrain_routing`.
2. Cite real tests only.
3. Include unit, integration, determinism, fuel, radiation, and recovery coverage.
4. Keep test names stable.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27AT — Espionage epistemic boundary

1. Define hidden truth, discovered facts, suspicion/evidence, and UI-safe presentation fields.
2. Never bind raw secret ShelterEspionage state to UI.
3. Create masked presentation DTO/adapter.
4. Add `EspionageUiNoHiddenTruthLeakTests`.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27AU — Faction espionage sub-tab

1. Extend FactionDetailPanel using current tab conventions.
2. Show suspects, investigations, dead drops, recent incidents, operations, and posture summary.
3. Support no-known-threat state.
4. Bind presentation adapter only.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27AV — SleeperAgentRowView

1. Create masked profile row with suspected alignment, evidence/suspicion gauge, status, and allowed actions.
2. Show confirmed identity only after canonical unmask event.
3. Use translation keys only.
4. Add row binding/snapshot tests.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27AW — Suspicion/evidence gauge semantics

1. Audit whether existing suspicion value is player-known evidence or hidden truth.
2. If hidden, derive a separate evidence/confidence presentation value.
3. Use qualitative bands where numeric certainty would mislead.
4. Document semantics in UI tooltip.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27AX — Covert surveillance toggle

1. Dispatch canonical surveillance policy/operation command.
2. Show cost/staffing requirements from simulation.
3. Do not increment suspicion directly.
4. Use stable operation ID.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27AY — Order Interrogation

1. Dispatch `InvestigateSuspect` or canonical interrogation action.
2. Simulation consumes security-chief skill/role modifier.
3. UI does not calculate outcome chance.
4. Respect custody/security/governance prerequisites.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27AZ — Flip Operative

1. Dispatch `AttemptTurnDoubleAgent`.
2. Use canonical Inventory transaction for scrap/medicine bribe.
3. Do not expose true loyalty in prompt.
4. Persist operation/outcome before follow-up rewards.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27BA — Flip/interrogation no-reroll

1. Key stochastic outcome by campaign seed + suspect + operation + attempt + policy version.
2. Persist result receipt.
3. Do not reroll on reload or UI reopen.
4. Bound repeat attempts.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27BB — Dead-drop tracker

1. Display discovered dead drops only.
2. Use canonical map/location IDs and campaign-time expiry.
3. Do not reveal hidden sites from secret state.
4. Add known/unknown/expired tests.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27BC — DailyBriefing sabotage aggregation

1. Use a UI-safe adapter instead of raw CaptureState if it contains secrets.
2. Aggregate recent incidents in a defined window.
3. Show localized severity/outcome/investigation state.
4. Deduplicate repeat display as intended.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27BD — Counter-intelligence trend

1. Derive posture/trend from guard assignments, armory security, surveillance state, recent foils/failures, and canonical Security/Espionage facts.
2. Do not create a second persistent CI score unless one already canonically exists.
3. Show reason breakdown.
4. Add consistency tests.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27BE — PREVENTED badge

1. Render only when canonical incident outcome is Foiled/Prevented.
2. Do not infer from zero damage.
3. Use localized text and diegetic styling.
4. Add snapshot test.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27BF — Espionage audio cues

1. Trigger suspense stinger on committed unmask/critical leak events.
2. Deduplicate by event ID.
3. Respect audio accessibility settings.
4. Do not fire on panel refresh.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27BG — Espionage localization

1. Translate all visible faction/operation/dead-drop/action labels.
2. Never display raw `fop_*`, `fdrop_*`, or faction IDs.
3. Add visible-label scanner test.
4. Provide actionable missing-key diagnostics.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27BH — FactionEspionageUiBindingTests

1. Instantiate panel/modal with safe presentation fixture.
2. Verify sub-tab, suspect row, interrogation/flip dispatch, dead-drop list, briefing aggregate, and null safety.
3. Assert hidden truth fields are absent from public adapter.
4. Run headless.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27BI — FactionDetailPanel scene lint

1. Run scene-lint.py after sub-tab changes.
2. Validate anchors, margins, containers, and minimum sizes.
3. Do not baseline existing/new lint failures.
4. Add lint to relevant CI step.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27BJ — Daily dawn refresh hooks

1. Update `Main.Plans50_53.cs` or canonical briefing wiring.
2. Refresh once from committed dawn briefing event.
3. Coalesce incident changes between briefings.
4. Do not poll every frame.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27BK — Espionage UI headless contract

1. Headlessly load FactionDetailPanel and DailyBriefingModal.
2. Validate safe node casts/adapters.
3. Exercise empty, suspect, unmasked, incident, and dead-drop states.
4. Fail if raw IDs or secret truth appear.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27BL — UI forensics update

1. Update `docs/forensics/UI_PANELS_UX_INVENTORY.md`.
2. Document espionage sub-tab, SleeperAgentRowView, DailyBriefing hooks, dead-drop tracker, commands, and tests.
3. Record hidden-truth masking boundary.
4. Record GarageDetailPanel entry too.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27BM — Extraction scenario catalog

1. Add four extraction scenario definitions in faction_intelligence.json after schema audit.
2. Each defines eligibility, faction capability, target asset, staging/handoff, localization, and result mapping.
3. Do not embed combat damage or ammo mutation.
4. Validate runtime consumers.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27BN — Retaliation threshold policy

1. Use existing faction hostility/suspicion/espionage state.
2. Do not create a second hostility number.
3. Return an explainable eligibility reason.
4. Add exact threshold/cooldown tests.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27BO — TriggerExtractionRaid orchestration

1. Create stable extraction operation/raid request ID.
2. Persist operation before handoff.
3. Reference faction, target operative, objective, timing window, and cause.
4. Do not resolve breach or combat inside ShelterEspionage.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27BP — PerimeterDefense extraction handoff

1. Route armed perimeter breach attempt into PerimeterDefense/ShelterSecurity.
2. Use real perimeter/topology/defense state.
3. Record breach result ref.
4. Do not teleport attackers inside.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27BQ — Combat extraction handoff

1. Use CombatSystem for participants, damage, retreat, capture, death, and outcome.
2. Espionage consumes committed outcome.
3. Do not create extraction-specific combat math.
4. Add result mapping tests.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27BR — Ammo conservation

1. Combat/Inventory owns ammo expenditure.
2. Do not subtract ammo inside ShelterEspionage.
3. Use IPlayerInventoryPort only if it is already the combat integration port.
4. Add extraction ammo reconciliation test.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27BS — Extraction lifecycle

1. Track only espionage-operation phases needed around external raid/combat refs.
2. Keep combat state external.
3. Prevent operation replay after save/load.
4. Archive resolved operations compactly.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27BT — Extraction outcomes

1. Support extracted, captured, killed, raid repelled, handler captured, or aborted only where canonical physical outcomes exist.
2. Operative state changes only after committed outcome.
3. Person/fate systems own death/capture.
4. Journal/faction consequences consume event.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27BU — Border retaliation boundary

1. Route retaliatory border/outpost raids through existing raid/world-control/defense systems.
2. Espionage supplies causal trigger only.
3. Do not create a second raid scheduler.
4. Feature-gate unsupported targets.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27BV — Double-agent poisoned-map operation

1. Represent disinformation as a typed espionage operation.
2. Target canonical faction intelligence/raid planning.
3. Use a bounded temporary modifier or false claim consumed by the real planning authority.
4. Do not directly decrement a raid-frequency scalar.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27BW — Raid-frequency owner audit

1. Identify the exact owner of raid scheduling/frequency.
2. Define what disinformation can influence: route confidence, target choice, readiness, or timing.
3. Apply the modifier once and with expiry.
4. Add scheduling before/after tests.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27BX — Double-agent discovery risk

1. Use existing loyalty/asset state.
2. Calculate risk from explicit canonical inputs.
3. Use keyed RNG per agent/day or operation and policy version.
4. Persist the result/receipt to prevent reload reroll.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27BY — Double-agent terminal outcome boundary

1. If hostile handlers execute/capture an agent, route person/fate outcome through canonical representation.
2. Do not silently delete asset.
3. Use neutral, non-graphic journal presentation.
4. Preserve historical operation refs.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27BZ — Poisoned supply cache operation

1. Implement `fdrop_poisoned_supply_cache` as an explicit player-authorized clandestine operation.
2. Use real cache/world location and Inventory items.
3. Represent contamination through canonical medicine/toxin/contamination authority.
4. Do not directly modify rival outpost health/strength.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27CA — Poisoned cache confirmation and consequences

1. Require explicit confirmation for the high-severity action.
2. Use canonical faction/moral/psychology consequence adapters if available.
3. Do not auto-run it as standing policy.
4. Test discovered, avoided, consumed, and failed-placement outcomes.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27CB — Dead-drop world integration

1. Use stable world site/location refs.
2. Store discovered state and expiry through espionage/world authority.
3. Do not duplicate coordinates in UI DTOs.
4. Time-skip expiry exactly once.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27CC — Espionage journal integration

1. Log unmask, critical leak, extraction, flip, compromise, dead-drop discovery, and major disinformation outcomes.
2. Use localized intercept-flavor templates.
3. Do not reveal secret truth the player does not know.
4. Deduplicate by source event ID.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27CD — Prewar archive cipher grant

1. Call a canonical `PrewarArchiveDecryptionSystem` grant API.
2. Use stable source grant ID.
3. Do not maintain cipher-fragment balance in espionage state.
4. Stop grants when asset/event is no longer eligible.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27CE — Cipher anti-farming policy

1. Prefer qualifying high-value intel receipts over unconditional daily rewards.
2. Bound grant frequency/value.
3. Do not reward replayed incidents.
4. Run long-run fragment economy test.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27CF — Retaliation causality and cooldown

1. Every extraction/retaliation operation cites a source incident/threshold reason.
2. Prevent repeated daily raid creation while threshold remains high.
3. Use active-operation/cooldown guard.
4. Do not clear underlying faction hostility automatically.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27CG — Counter-agent asset state

1. Validate one owner for Active, Turned, Compromised, Extracted, Captured, Dead, Retired states.
2. Do not duplicate person/faction status.
3. Operations reference asset state.
4. Add transition tests.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27CH — Espionage save migration

1. Version ShelterEspionage save for new active operations.
2. Preserve existing sleepers, evidence/suspicion, dead drops, and turned agents exactly.
3. Initialize new extraction/retaliation collections empty.
4. Do not retroactively spawn raids.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27CI — Active extraction round-trip

1. Save/restore staging, approach, breach-request, and resolved phases where applicable.
2. Restore PerimeterDefense/Combat refs safely.
3. Do not replay ammo, breach, rewards, or journal entries.
4. Add missing-ref fallback.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27CJ — Catalog integrity

1. Extend CatalogIntegrityRules for extraction operations/dead drops.
2. Validate faction refs, operation IDs, target constraints, localization, handoff capabilities, and payload refs.
3. Reject duplicate/orphan IDs.
4. Reject direct combat/resource mutation fields.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27CK — Content utilization

1. Run `--content-utilization-selftest`.
2. Ensure every new `fop_*` and `fdrop_*` has a gameplay consumer.
3. Do not count docs/tests alone as production utilization unless current rule explicitly permits it.
4. Fail dead content.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27CL — 60-day clandestine lifecycle test

1. Create `FactionRetaliationIntegrationTests.cs`.
2. Simulate suspicion/evidence, investigation, turning, disinformation, extraction trigger, perimeter defense, combat, dead drops, archive grants, and cooldown over 60 days.
3. Assert no duplicate raids/ammo/rewards.
4. Capture deterministic summary.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27CM — Espionage deterministic replay

1. Replay identical 60-day seed/state.
2. Compare unmasking, double-agent discovery, retaliation triggers, operation IDs, raid handoffs, intel totals, and journal IDs.
3. Partition RNG streams.
4. Add call-order perturbation test.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27CN — Hidden-truth save boundary

1. Persist canonical secret state only in espionage save.
2. Keep UI-safe presentation objects derived/non-authoritative.
3. Persist player-known discovery/evidence according to existing owner.
4. Add a save inspection test that UI caches never become truth.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27CO — Security chief modifier boundary

1. Read canonical security skill/role at operation evaluation point.
2. UI shows explanatory modifier text only.
3. Do not copy skill as an independent espionage stat.
4. Define snapshot semantics if long operations require deterministic historical input.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27CP — Vehicle + espionage navigation hardening

1. Verify Garage panel and espionage sub-tab coexist with navigation, modal, focus, and hotkey rules.
2. Run keyboard/controller focus traversal.
3. Verify 1920x1080 baseline and existing scaling support.
4. Prevent hidden modal focus traps.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27CQ — Accessibility

1. Condition gauges require textual values/severity.
2. Espionage evidence gauges require labels/confidence wording.
3. PREVENTED badge requires textual semantics.
4. Audio cues remain nonessential and have visual context.
5. Test localization expansion.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27CR — Performance budget

1. Garage refresh is event-driven.
2. Terrain calculator is allocation-free or tightly bounded on transit hot path.
3. DailyBriefing does not clone full secret state repeatedly.
4. Espionage lists use bounded/virtualized rendering if necessary.
5. Record median/p95 and allocation budgets.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27CS — Long-campaign soak

1. Run maximum campaign with repeated vehicle sorties, service, breakdown/recovery, espionage incidents, flips, dead drops, extraction raids, and daily briefings.
2. Track save growth, operation history, journal volume, recovery missions, cipher grants, UI list size, allocations, and repeated-event counts.
3. Verify bounded history and no duplication.
4. Add regression thresholds.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27CT — Documentation pack

1. Update ARCHITECTURE_TEST_MAP Section 2 with `vehicle_terrain_routing`.
2. Update UI_PANELS_UX_INVENTORY with Garage and espionage surfaces.
3. Update FACTION_ESPIONAGE_AUTHORITY_MAP for extraction/disinformation.
4. Add PLAN50_51_UI_COMMAND_BOUNDARIES.md and VEHICLE_TERRAIN_AUTHORITY.md.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

## E1-27CU — Release gate

1. Run dotnet build/tests, game build, data-integrity selftest, garage-panel selftest, content-utilization selftest, scene lint, snapshot tests, vehicle terrain/recovery/fuel/radiation/determinism tests, espionage UI binding/hidden-truth tests, retaliation migration/ammo/determinism tests, and 60-day lifecycle.
2. Verify every new catalog ID is consumed.
3. Verify no direct UI mutation, duplicate fuel/dose, duplicate raid/ammo, or secret-state leakage.
4. Mark DONE only after all four vertical slices pass together.

**Package gate:** canonical ownership, stable IDs, replay-safe save/load, relevant conservation, and hidden-truth masking must pass before merge.

---

# 5. Typed Command Contracts

## Garage modification

```text
GarageDetailPanel
-> InstallModificationCommand(operation_id, vehicle_id, slot_id, modification_id)
-> VehicleGarage validates compatibility
-> Inventory validates/reserves cost
-> VehicleGarage commits modification
-> Inventory consumes once
-> event emitted
-> UI refreshes
```

## Service All

```text
PreviewServiceAll(vehicle_id)
-> immutable cost/version projection

CommitServiceAll(operation_id, preview_version)
-> revalidate
-> reserve total resources
-> service all components
-> commit or rollback
```

## Interrogation / turn operation

```text
UI
-> canonical espionage command
-> validate actor/suspect/resources
-> persist operation
-> keyed decision
-> commit result
-> consume resources once
-> journal/UI side effects
```

# 6. Vehicle Terrain Result

```yaml
route_segment_id: "edge_087"
terrain: "mud"
weather_context: "storm_wet"
wear_multiplier_permille: 1380
speed_multiplier_permille: 720
fuel_multiplier_permille: 1260
radiation_protection_permille: 275
reasons:
  - "terrain_mud"
  - "heavy_flatbed"
  - "storm_wetness"
```

Derived only; do not persist as route truth.

# 7. Fuel Reconciliation Rule

For unchanged route, load, vehicle state, weather assumptions, and calculator version:

```text
planned_fuel == expected_runtime_fuel
```

Permitted differences must cite a real state change: reroute, cargo change, weather change, hazard detour, or
component-state change.

# 8. Radiation Rule

Vehicle shielding may reduce eligible **external** exposure.

It must not:

- erase previous dose;
- reduce internal contamination;
- alter dose in both Stealth and Radiation;
- stack twice with the same enclosure protection.

# 9. Breakdown Rule

```text
wear reaches threshold
-> one breakdown incident
-> expedition stops vehicle
-> location captured
-> one recovery mission
-> one journal event
```

All downstream IDs derive from the breakdown incident or reference it.

# 10. Espionage UI Contract

UI-safe fields may include:

- masked identity;
- known name if discovered;
- suspected alignment;
- confidence/evidence band;
- investigation status;
- known incidents;
- discovered dead drops;
- permitted actions.

UI-safe fields must exclude undiscovered:

- actual sleeper flag;
- true faction;
- true loyalty;
- true handler;
- hidden dead drops;
- hidden operation target;
- exact future retaliation roll.

# 11. Counter-Intelligence Trend

Prefer a derived read model with reasons:

```text
trend: improving
reasons:
  + guard coverage increased
  + armory security repaired
  - unresolved sabotage incident
```

Do not invent another persistent `counterIntelScore` unless audit proves that score is already canonical.

# 12. Extraction Raid Contract

```text
espionage/faction trigger
-> extraction operation
-> PerimeterDefense breach request
-> Combat encounter if needed
-> Combat/Inventory ammo use
-> committed battle result
-> operative outcome
-> faction/journal consequences
```

ShelterEspionage does not resolve battle damage.

# 13. Double-Agent RNG Key

Recommended:

```text
campaign_seed
+ operative_id
+ operation_or_day_id
+ discovery_policy_version
```

Persist or deterministically derive the result so reload cannot change it.

# 14. Poisoned Cache Boundary

The cache operation owns intent and provenance. Canonical systems own:

- cache/world object;
- medicine/item instances;
- contamination/toxin state;
- discovery/consumption;
- health effects;
- faction/outpost consequences.

No direct `enemy_outpost_strength -= X`.

# 15. Cipher Fragment Boundary

```text
qualifying intel receipt
-> PrewarArchiveDecryptionSystem.TryGrant(...)
-> stable source grant ID
-> fragment balance changes once
```

No cipher currency lives in ShelterEspionageState.

# 16. Exploit Matrix

| Failure | Required guard |
|---|---|
| Install double-click | operation ID |
| Uninstall duplicates mod | Inventory transaction |
| Service All partial failure | atomic batch |
| Terrain modifier applied twice | authority test |
| Planned fuel differs silently | reconciliation test |
| Shielding applies twice | dose integration test |
| Breakdown repeats each tick | incident ID |
| Recovery mission duplicates | breakdown-derived mission ID |
| UI leaks true sleeper faction | safe DTO |
| Interrogation rerolls | keyed/persisted result |
| Bribe spent twice | transaction ID |
| Extraction ammo spent twice | Combat ownership |
| High suspicion spawns daily raid | cooldown/active op |
| Double agent rerolls daily on reload | partitioned key |
| Poisoned cache directly damages target | canonical contamination/health |
| Cipher reward farms | grant source ID/cap |
| New catalog IDs dead | content utilization |

# 17. Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj

godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --garage-panel-selftest
godot --headless --path . -- --content-utilization-selftest
```

Also run:

- repository scene lint;
- UI contract verification;
- snapshot suite;
- `VehicleExpeditionTerrainTests`;
- `FactionEspionageUiBindingTests`;
- `FactionRetaliationIntegrationTests`;
- deterministic paired replays.

# 18. Completion Checklist

- [ ] GarageDetailPanel implements IContractPanel.
- [ ] Garage scene passes lint.
- [ ] Slot compatibility comes from VehicleGarage.
- [ ] Gauges bind real wear values.
- [ ] Install/uninstall conserve inventory.
- [ ] Service All is atomic.
- [ ] Recovery UI uses canonical route/fuel/ETA.
- [ ] Stat projection is not calculated in UI.
- [ ] Audio fires on committed actions.
- [ ] Garage selftest passes.
- [ ] Snapshot coverage passes.
- [ ] Terrain tags validate.
- [ ] Terrain authority is documented.
- [ ] Speed/wear/fuel use one modifier contract.
- [ ] Planning/runtime fuel reconcile.
- [ ] Bullbar collision adapter is bounded.
- [ ] Flatbed penalty is contextual.
- [ ] Radiation shielding applies once.
- [ ] Breakdown triggers once.
- [ ] Recovery mission triggers once.
- [ ] 800-permille recovery semantics are explicitly resolved.
- [ ] Paired vehicle replay is deterministic.
- [ ] Espionage UI exposes known evidence only.
- [ ] No raw operation/faction IDs appear.
- [ ] Interrogation/flip dispatch canonical commands.
- [ ] Bribe resources are transactional.
- [ ] Dead drops show discovered sites only.
- [ ] DailyBriefing reads safe presentation data.
- [ ] CI trend is derived.
- [ ] PREVENTED reflects canonical result.
- [ ] Extraction operations route through PerimeterDefense/Combat.
- [ ] Combat/Inventory consumes ammo once.
- [ ] Disinformation modifies canonical raid/intelligence policy.
- [ ] Double-agent discovery is keyed/no-reroll.
- [ ] Poisoned cache uses canonical contamination/item/health systems.
- [ ] Archive grants are idempotent.
- [ ] Save migration creates no retroactive retaliation.
- [ ] All new IDs pass content utilization.
- [ ] 60-day clandestine lifecycle passes.
- [ ] Long-campaign soak passes.
- [ ] `E1_planintegration[28].md` is next.

# 19. Final Directive

This integration should produce four visible improvements without four new islands of authority.

The garage should expose and command the real VehicleGarage state.
Terrain should change vehicle behavior through one deterministic route-response contract.
The counter-intelligence dashboard should show what the player has evidence to know—not what the secret state
knows.
Faction retaliation should culminate in real perimeter defense, combat, inventory expenditure, and faction
consequences.

The final architectural rule is:

**UI projects and commands; VehicleGarage owns vehicle state; Expedition owns transit; route/world data owns
terrain; fuel and radiation retain their canonical owners; ShelterEspionage owns covert operations; Security,
Faction, PerimeterDefense, Combat, Inventory, Journal, and Archive own their consequences.**

If any implementation directly mutates another owner's state, duplicates route/fuel/dose logic, leaks hidden
espionage truth, rerolls after reload, or creates a parallel raid/ammo/intelligence economy, stop and repair
the boundary before proceeding.
