---
PLAN_ID: E1-28
PLAN_FAMILY: planintegration
SEQUENCE_INDEX: 28
STATUS: READY_FOR_EXECUTION_AFTER_PLANS63_64_65_129_AUTHORITY_RECON
SOURCE_PLAN: "Plan 63/64/65/129 Follow-up — Captive Detention UI, Food Preservation UI, Campaign Epilogue UI, Foundry Balance & Metallurgy Integration"
SEQUENCE_FILENAME: "E1_planintegration[28].md"
PREVIOUS_FILENAME: "E1_planintegration[27].md"
NEXT_FILENAMES:
  - "E1_planintegration[29].md"
  - "E1_planintegration[30].md"
CATEGORY: UI+CAPTIVES+FOOD+EPILOGUE+FOUNDRY+ECONOMY
PRIMARY_INTENT: "Ship four follow-up slices while preserving singular ownership for captive lifecycle/security/health/recruitment, food cohorts/spoilage/inventory/power/thermal, epilogue facts and campaign completion, and foundry heat/material/trade/vehicle-recipe state."
PREMISE_VERIFICATION_REQUIRED: true
ONE_AUTHORITY_PER_FACT: true
SECOND_CAPTIVE_LIFECYCLE_FORBIDDEN: true
SECOND_HEALTH_NEEDS_SYSTEM_FORBIDDEN: true
SECOND_SECURITY_ESCAPE_SYSTEM_FORBIDDEN: true
SECOND_RECRUITMENT_PIPELINE_FORBIDDEN: true
SECOND_FOOD_INVENTORY_FORBIDDEN: true
SECOND_SPOILAGE_CLOCK_FORBIDDEN: true
SECOND_POWER_OR_THERMAL_SYSTEM_FORBIDDEN: true
SECOND_CAMPAIGN_TRUTH_OR_EPILOGUE_METRIC_LEDGER_FORBIDDEN: true
SECOND_FOUNDRY_INVENTORY_OR_HEAT_LEDGER_FORBIDDEN: true
RNG_MUST_BE_KEYED_AND_PARTITIONED: true
UI_MUST_BE_READ_MODEL_PLUS_COMMANDS: true
RUNTIME_RISK: VERY_HIGH
SAVE_RISK: VERY_HIGH
BALANCE_RISK: VERY_HIGH
DETERMINISM_RISK: VERY_HIGH
ETHICAL_PRESENTATION_RISK: HIGH
---

# E1 Plan Integration [28] — Captive Detention UI, Food Preservation UI, Campaign Epilogue, and Foundry 20-Product Balance/Metallurgy Integration

> **Sequence rule:** this file is `E1_planintegration[28].md`. The next file is `E1_planintegration[29].md`.

## 0. Mission

This plan turns Tasks 9–12 into one implementation-grade programme covering four distinct but integration-heavy
areas:

1. **Captive Detention Cells & Parole Tribunal UI** — a player-facing management surface over existing captive,
   security, health, interrogation, labor, parole, and recruitment authorities.
2. **Food Pantry, Curing Vats & Spoilage Prevention UI** — a sustenance UI over canonical food cohorts,
   spoilage clocks, curing jobs, refrigeration, ration policy, power, thermal state, and kitchen consumption.
3. **Grand Epilogue Cinematic & Campaign Chronicle UI** — a full-screen endgame presentation over canonical
   campaign-completion facts, historical metrics, survivor outcomes, memorial records, and campaign chronicle.
4. **Foundry 20-Product Balance Sweep & Metallurgy Tree Integration** — a content/balance pass across all foundry
   products while preserving one heat model, one production queue, one inventory ledger, one trade economy, and
   one vehicle-recipe authority.

These bundles are deliberately integrated here because they share the same failure modes: UI duplicating
simulation truth, derived scores becoming new authorities, effects bypassing canonical resource/health/security
owners, and save/replay ambiguity.

The governing rule is:

**presentation systems expose authoritative read models and dispatch commands; canonical simulation systems own
state; cross-system effects use typed handoffs; deterministic operations are keyed, conserved, and replay-safe.**

## 1. Cross-Plan Architectural Corrections

### 1.1 Captive UI is not a captive simulation

`PrisonerManagementPanel` should display captives and dispatch actions. Captive health/starvation remains Health/
Needs-owned. Escape risk must be derived from actual guard/security/cell conditions. Parole/recruitment must call
canonical tribunal/governance/recruitment systems.

### 1.2 Interrogation must not hide coercion behind generic optimization

"Rapport Building", "Evidence Confrontation", and "Ration Deprivation" are mechanically different approaches.
Any deprivation/coercion path should have explicit costs, ethical consequences, health constraints, guard
psychology consequences, and governance/security checks. The UI should never directly reduce resistance or
health.

### 1.3 Food preservation UI must not create another food ledger

Spoilage cohorts, freshness, storage conditions, curing jobs, refrigeration, and ration policy must be
projections over Inventory/FoodPreservation/Kitchen/Power/Thermal state. The panel should not own copied food
amounts or independent spoilage timers.

### 1.4 Kitchen consumption policy needs one owner

"Consume Oldest First", "Prioritize Fresh", and "Emergency Deep Freeze" should become an explicit policy
consumed by the canonical Kitchen/Rationing/FoodPreservation allocator. The UI must not secretly choose stacks.

### 1.5 Epilogue metrics must be sourced, not invented at game end

Every counter and vignette must point to a canonical campaign fact or immutable historical record. The
epilogue engine composes narrative; it does not recalculate deaths, treaties, relics, or survivor trauma from
guesswork.

### 1.6 Survivor epilogue outcomes must not diagnose from raw hidden scores

Life outcomes can reference canonical mental-health/trauma history and final state, but should use authored
outcome rules and uncertainty-aware language rather than turning a numeric trauma score into a deterministic
medical diagnosis.

### 1.7 Foundry heat model must remain singular

`SilentFoundrySystem.Heat.cs` can own foundry thermal state if that is the existing authority. Structural
cracking/fire consequences must route through canonical shelter condition/fire/hazard systems rather than
creating a foundry-local structural damage universe.

### 1.8 "Realistic" thermal dissipation means calibrated simulation, not false precision

Active crucible count, power, insulation/cooling capability, ambient room thermal state, and duty cycle can
drive heat. Do not pretend an exact real-world thermodynamic model exists unless the repository has the
necessary material/geometry parameters.

### 1.9 Foundry economy must preserve conservation

Scavenged scrap -> foundry input -> processed product -> vehicle/workshop/trade output must reconcile through
Inventory and Market. No product should be simultaneously created by recipe output and granted again by a
vehicle integration hook.

### 1.10 100-day simulation is a calibration harness, not production authority

The balance sweep script should reveal bottlenecks, throughput, heat saturation, power burden, trade value, and
resource deficits. It should not silently rewrite live catalog values.

## 2. Canonical Ownership Matrix

| Concern | Canonical owner | E1-28 role |
|---|---|---|
| Captive identity/lifecycle | Captive/Prisoner system | UI projection |
| Captive health/starvation | Health/Needs | UI projection |
| Interrogation resolution | Captive/Interrogation authority | command |
| Cell/security state | ShelterSecurity/cell system | projection |
| Escape outcome | ShelterSecurity | consume/display |
| Parole/amnesty | Governance/tribunal authority | command |
| Recruitment | Recruitment/SurvivorCatalog | handoff |
| Captive labor | Duty/Labor/Production | command/ref |
| Guard trauma | E1-15 Psychology/Needs | consequence handoff |
| Food inventory/cohorts | Inventory/FoodPreservation | projection |
| Spoilage clock | FoodPreservation/item freshness owner | projection |
| Curing recipe/job | FoodPreservation + Duty/Inventory | command |
| Freezer power | PowerGrid | query |
| Freezer temperature integrity | Thermal/FoodPreservation | projection |
| Kitchen recipe sourcing | KitchenNutrition/Inventory allocator | policy consumer |
| Campaign completion | CampaignCompletion/Victory authority | trigger |
| Epilogue composition | CampaignEpilogueEngine | presentation |
| Historical metrics | canonical campaign/history systems | source |
| Fallen survivors | E1-23/Memorial | source |
| Exported chronicle | epilogue exporter | derived document |
| Foundry queue/products | SilentFoundrySystem/catalog | own |
| Foundry heat | SilentFoundrySystem.Heat | own if confirmed |
| Structural cracking/fire | shelter structure/fire authority | handoff |
| Foundry materials | Inventory | consume/produce |
| Trade value | Market/E1-19 | consume catalog/economy |
| Vehicle recipe consumption | VehicleGarage/recipe authority | consume foundry products |
| Audio | ShelterAcousticDirector/AudioManager | presentation |

## 3. Delivery Slices

### Slice A — Captive panel shell
Scene, contract, captive list/profile, guard/security projection, localization, selftests.

### Slice B — Captive actions
Interrogation, parole tribunal, provisional recruitment handoff, guarded labor, psychological consequence routing.

### Slice C — Food preservation panel shell
Cohorts, spoilage warnings, storage tiers, curing catalog, power/temperature projection.

### Slice D — Food actions
Curing jobs, freezer power request, ration policy, Kitchen integration, alarms.

### Slice E — Epilogue shell
Vignettes, metrics, roster, memorials, audio, trigger, full-screen contract.

### Slice F — Epilogue export/post-game
Chronicle export, main-menu return, final-shelter inspection, localization, endgame tests.

### Slice G — Foundry content/economy
20-product audit, vehicle recipe wiring, trade tables, 100-day simulation.

### Slice H — Foundry thermal hardening
Crucible heat model, cooling/power tests, runaway hazard, save/determinism.

### Slice I — Cross-plan release gate
Headless/UI tests, scene lint, catalog integrity, content utilization, save migration, performance, and docs.

---

## E1-28A — Repository premise and authority reconnaissance

1. Audit Plan 63 captive runtime, prisoner/captive DTOs, cell capacity/security, interrogation APIs, parole/governance, Recruitment, SurvivorCatalog, Health, Needs, E1-15 Psychology, Duty/labor, acoustic cues, localization, and UI panel conventions.
2. Audit FoodPreservationSystem, food cohort/freshness model, Inventory, KitchenNutritionPanel, rationing allocator, curing recipes, PowerGrid, Thermal, freezer state, day-tick events, acoustic alarms, and UI contracts.
3. Audit CampaignEpilogueEngine, campaign victory/extinction trigger, historical metrics, survivor roster, E1-23/Memorial, campaign_epilogues.json, AudioManager, export/file utilities, main-menu routing, final-state inspection, and localization.
4. Audit foundry_production.json, SilentFoundrySystem, heat code, save store, Inventory, scavenging inputs, vehicle recipes, Market/trade tables, cooling/power APIs, hazard/fire/structure owners, and current tests.
5. Create `docs/forensics/E1_28_PLANS63_64_65_129_AUTHORITY_RECON.md`.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28B — Captive UI ownership contract

1. Define PrisonerManagementPanel as presentation + command surface only.
2. List every displayed captive/security/health/interrogation field and its owner.
3. List every allowed action and destination API.
4. Prohibit copied health, starvation, escape, resistance, hostility, parole, labor, or recruitment truth inside UI state.
5. Add authority-boundary tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28C — PrisonerManagementPanel scene shell

1. Create `src/UI/PrisonerManagementPanel.cs` implementing IContractPanel.
2. Create `assets/ui/panels/PrisonerManagementPanel.tscn` with cell overview, inmate list, profile pane, action pane, guard/security pane, and tribunal flow.
3. Follow current fixed 1920x1080 scene-lint conventions.
4. Support empty/no-captives state.
5. Use current panel navigation/focus patterns.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28D — Captive profile read model

1. Expose player-known origin faction, resistance/evidence state, hostility presentation, health/starvation summary, cell assignment, security restrictions, and interrogation/parole eligibility.
2. Health/Needs values are read-only projections.
3. Do not expose hidden loyalty/faction truth beyond canonical knowledge.
4. Use translated labels.
5. Add masked/known profile tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28E — Resistance and hostility gauge semantics

1. Audit whether resistance and hostility are distinct canonical facts.
2. Define gauge ranges, direction, thresholds, and uncertainty.
3. Do not show false numerical precision if values are hidden/qualitative.
4. Provide text labels/tooltips.
5. Add 0/threshold/max tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28F — Interrogation approach selector

1. Render Rapport Building, Evidence Confrontation, and Ration Deprivation as localized policies/commands.
2. UI shows prerequisites, projected resource/time cost, governance/health restrictions, and known risks.
3. Do not directly modify resistance.
4. Disable unavailable approaches with explicit reasons.
5. Use canonical interrogation command.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28G — Rapport-building integration

1. Route through interrogation/social authority.
2. Consume time, staff, evidence, and relationship/context inputs only as canonical owner permits.
3. Do not grant guaranteed intelligence.
4. Persist operation ID before stochastic outcome.
5. Add deterministic/no-reroll tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28H — Evidence-confrontation integration

1. Require real evidence refs from Security/Journal/Intelligence.
2. Interrogation owner determines effect/outcome.
3. Do not consume evidence by UI unless canonical rules say so.
4. Show contradiction/evidence strength without leaking hidden truth.
5. Add missing/invalid evidence tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28I — Ration-deprivation ethical and health boundary

1. Treat deprivation as a coercive/high-severity action requiring explicit player confirmation and policy permission.
2. Needs/Health owns food deprivation and physiological consequences.
3. Interrogation system may consume a canonical deprivation state/effect, not directly subtract food or health.
4. Block or escalate medically dangerous conditions according to canonical rules.
5. Emit guard moral-stress/trauma context to E1-15/Needs where appropriate.
6. Add save/load and consequence tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28J — Interrogation topic tree

1. Represent topics as canonical intelligence objectives: weapon caches, troop movements, cipher keys, and other catalogued subjects.
2. Topic unlock depends on real evidence/interrogation state.
3. Do not hard-code reward effects in UI.
4. Use localized topic IDs.
5. Add catalog integrity tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28K — Topic extraction state

1. Interrogation authority owns topic progress/resolution.
2. UI may render locked/available/revealed/exhausted states.
3. Revealed intelligence becomes canonical E1-3/Faction/Archive information through typed handoff.
4. Do not duplicate intel truth in panel.
5. Add repeated-extraction anti-farm tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28L — Guard assignment widget

1. Query canonical guard assignments and cell coverage.
2. Display derived detention security posture and escape-risk projection.
3. Do not persist a second security score.
4. Allow assignment request through Duty/Security.
5. Add no-guard/understaffed/secure states.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28M — Escape-risk read model

1. Derive from cell condition, guards, prisoner state, access, tools, recent incidents, and security policy.
2. Escape outcome remains ShelterSecurity-owned.
3. Do not roll escape from UI.
4. Expose reason breakdown.
5. Add consistency tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28N — Parole tribunal command

1. Implement 'Convene Parole Tribunal' as a Governance/tribunal command.
2. Validate captive status, evidence, health, security, and tribunal prerequisites.
3. Open a decision flow for amnesty, continued detention, supervised parole, or recruitment referral if canonical rules support them.
4. Do not convert prisoner status from UI directly.
5. Use stable tribunal operation ID.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28O — Amnesty/parole outcome handoff

1. Governance owns legal/status decision.
2. Security owns release/escort/access policy.
3. Captive system closes or changes detention status only from committed tribunal result.
4. Inventory owns returned/confiscated possessions.
5. Add double-submit/save tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28P — Provisional settler recruitment handoff

1. Tribunal may refer an eligible captive to Recruitment.
2. Recruitment/SurvivorCatalog decides and performs permanent conversion.
3. Do not create survivor record inside prisoner UI.
4. Define provisional status if canonical recruitment supports it.
5. Ensure one active identity after conversion.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28Q — Captive labor dispatch

1. Route quarry/agricultural assignment through Duty/Labor/Production authority.
2. Require legal/parole policy, guard oversight if required, work capability, and site access.
3. Do not calculate production in captive UI.
4. Do not let labor assignment bypass food/health/safety requirements.
5. Add assignment/interruption tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28R — Guard oversight requirement

1. Security/Duty owns escort/guard staffing.
2. Labor job references required oversight.
3. Production only credits completed canonical work.
4. Do not infer safety from UI security score.
5. Add no-guard and interrupted-shift tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28S — Guard psychological consequence handoff

1. Harsh/coercive interrogation emits typed moral-injury/stress context to E1-15 Psychology/Needs for participating staff.
2. E1-28 does not directly assign trauma diagnoses.
3. Use severity/provenance from committed interrogation event.
4. Prevent duplicate consequence on reload.
5. Add no-participant/no-harsh-action tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28T — Captive acoustic integration

1. Trigger steel-door clang on committed cell-door/bay interaction.
2. Trigger interrogation desk ambience only while relevant scene/activity is active.
3. Use ShelterAcousticDirector.
4. Audio must not gate interrogation or security state.
5. Respect audio accessibility settings.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28U — Captive panel navigation/cross-links

1. Register in Main.UiPanels.cs with existing hotkey/router.
2. Add bunk/cell assignment cross-links to canonical housing/cell panel.
3. Do not create second route registry.
4. Ensure modal tribunal flow restores focus.
5. Add navigation test.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28V — PrisonerManagementPanelTests

1. Instantiate panel with safe read-model/command stubs.
2. Verify captive list/profile, approach selector, topic tree, guard widget, tribunal button, labor dispatch, and empty state.
3. Verify button handlers dispatch commands rather than mutate state.
4. Run headless.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28W — Captive scene lint and localization

1. Run scene-lint.py with zero warnings/errors.
2. Verify all interrogation topics/dialogue/action labels use translation keys.
3. Reject raw faction/captive/internal IDs in visible labels.
4. Update docs/CAPTIVE_SYSTEM_AUTHORITY_MAP.md with UI contract.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28X — Food UI ownership contract

1. Define FoodPreservationPanel as projection + command only.
2. Inventory/FoodPreservation own cohorts/quantities/spoilage.
3. PowerGrid owns power; Thermal/FoodPreservation owns freezer integrity.
4. Kitchen/Rationing owns food selection/consumption.
5. Add authority-boundary tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28Y — FoodPreservationPanel scene shell

1. Create `src/UI/FoodPreservationPanel.cs` implementing IContractPanel.
2. Create `assets/ui/panels/FoodPreservationPanel.tscn` with storage overview, cohort timeline, curing station, freezer section, ration policy, and warnings.
3. Represent ambient pantry, root cellar, salt vats, and cryogenic lockers as canonical storage capabilities rather than decorative duplicate inventories.
4. Support empty/no-preservation-facility states.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28Z — Food storage tier read model

1. Query pantry/root-cellar/vat/freezer facilities and associated canonical inventory containers.
2. Display capacity, occupancy, temperature/storage policy, power dependency, and condition from owners.
3. Do not store copied stock totals in UI.
4. Add unavailable/degraded storage states.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28AA — Food cohort timeline

1. Render canonical food cohorts with item/food type, quantity, freshness/spoilage state, storage location, and derived days remaining.
2. Use existing spoilage owner/time source.
3. Do not create panel-local countdown state.
4. Sort deterministically and virtualize if cohort count grows.
5. Add time-skip consistency tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28AB — 48-hour spoilage warning

1. Show visual warning for cohorts projected to expire within 48 hours.
2. Use campaign-time projection.
3. Do not alter rationing automatically unless policy says so.
4. Provide accessible text/icon semantics.
5. Add boundary tests at 49h, 48h, 1h, expired.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28AC — Curing catalog UI

1. Read FoodPreservationCatalog.curing_recipes.
2. Display inputs, salt/chemical requirements, time, facility, cook/staff requirement, output, preservation benefit, and blockers.
3. Do not calculate recipe truth in UI.
4. Localize all recipe names/descriptions.
5. Add missing-recipe/reference tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28AD — Start Curing Job transaction

1. Dispatch canonical curing job.
2. Reserve raw meat, salt/chemicals, facility capacity, and cook assignment through owner systems.
3. Consume inputs at the canonical point of no return.
4. Produce output once on completion.
5. Use stable job ID.
6. Add cancel/reload/conservation tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28AE — Curing job staffing

1. Duty owns cook assignment.
2. FoodPreservation owns curing process state.
3. Skills may modify outcome/time only through canonical rule.
4. Do not maintain a second cook roster in panel.
5. Add no-cook/reassignment tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28AF — Cryogenic freezer power projection

1. Show requested/active wattage from PowerGrid.
2. Toggle should dispatch canonical enable/disable/load request.
3. Do not write power state directly.
4. Display power-denied/brownout status.
5. Add power-toggle tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28AG — Freezer temperature integrity

1. Define temperature-integrity permille as a canonical FoodPreservation/Thermal read model.
2. Do not make it a second room temperature.
3. Derive from actual freezer temperature relative to safe band and outage duration.
4. Add healthy/rising/failed/recovered tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28AH — Freezer failure alarm

1. Trigger ShelterAcousticDirector alarm when canonical temperature-risk threshold is crossed after power loss.
2. Deduplicate by incident/state transition.
3. Provide visual alert for muted audio.
4. Do not alarm merely because UI is open.
5. Add transition tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28AI — Auto-ration policy contract

1. Implement Consume Oldest First, Prioritize Fresh, Emergency Deep Freeze as policy IDs consumed by canonical Kitchen/Rationing allocator.
2. Do not choose food stacks inside UI.
3. Define deterministic tie-breaking.
4. Explain consequences in tooltip.
5. Add policy-switch tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28AJ — Kitchen integration boundary

1. KitchenNutritionPanel and recipe execution must query the same canonical inventory/cohort allocator.
2. Resolve the user-specified 'highest-freshness cohorts' requirement against the selected ration policy: do not hard-code freshest-first if policy says oldest-first.
3. Expose selected policy and source selection reason.
4. Add Kitchen/FoodPreservation consistency tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28AK — Spoilage clock singularity

1. Audit all freshness/spoilage timers.
2. Ensure curing, refrigeration, pantry storage, and Kitchen read the same owner.
3. Do not decrement freshness in both Inventory and FoodPreservation.
4. Add duplicate-clock detection test.
5. Document migration if older saves store freshness elsewhere.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28AL — Food panel binding/reactivity

1. Implement Bind(FoodPreservationSystem, IPlayerInventoryPort) plus canonical Power/Thermal adapters if current panel pattern supports composition.
2. Refresh on day tick plus relevant inventory/power/temperature/job events.
3. Do not rely only on day tick for urgent freezer failure.
4. Coalesce refreshes.
5. Add rebind/dispose tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28AM — Food panel registration

1. Register under shelter sustenance navigation group in Main.UiPanels.cs.
2. Follow existing routing/hotkey conventions.
3. Add cross-link to KitchenNutritionPanel.
4. Do not create separate sustenance navigation.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28AN — FoodPreservationPanelTests

1. Headlessly render cohort list, warning state, storage facilities, curing recipes, freezer state, ration policy, and empty state.
2. Verify command dispatch.
3. Test 48-hour warning boundary.
4. Run scene lint with zero errors.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28AO — Food authority documentation

1. Update `docs/FOOD_PRESERVATION_AUTHORITY_MAP.md` with panel read-model contract, curing workflow, power/thermal ownership, spoilage owner, and Kitchen selection policy.
2. Document cohort identity and save ownership.
3. Document emergency freezer alarm path.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28AP — Epilogue ownership contract

1. Define CampaignEpilogueModal as full-screen presentation over CampaignEpilogueEngine output plus canonical history/metrics.
2. CampaignCompletion/Victory authority owns when the campaign ends.
3. Epilogue engine composes outcomes but does not invent canonical facts.
4. UI owns animation/navigation/export request only.
5. Add authority-boundary tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28AQ — CampaignEpilogueModal scene shell

1. Create `src/UI/CampaignEpilogueModal.cs` inheriting Control.
2. Create full-screen `assets/ui/modals/CampaignEpilogueModal.tscn` with vignette carousel, summary, parchment frame, roster, memorials, controls.
3. Use 1920x1080 anchor stretch and scene-lint conventions.
4. Support reduced-motion mode.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28AR — Epilogue evaluation snapshot

1. Call CampaignEpilogueEngine.EvaluateEpilogue() exactly once per committed endgame context/version unless explicit recompute is requested for inspection.
2. Store/read an immutable epilogue evaluation snapshot if canonical architecture supports it.
3. Do not reroll narrative vignettes after reload.
4. Use stable campaign completion ID.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28AS — Vignette binding

1. Bind demographics, governance, technology, and sustenance pillars from typed epilogue output.
2. Allow missing/optional pillar fallback without null refs.
3. Use translation/catalog IDs.
4. Do not display raw internal metric IDs.
5. Add all-slot and sparse-output tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28AT — Narrative pacing animation

1. Implement fade-and-scroll presentation using UI animation only.
2. Support skip/fast-forward/reduced-motion.
3. Animation timing must not alter campaign state.
4. Do not block export or final-state inspection once evaluation exists.
5. Add deterministic UI-state tests where practical.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28AU — Historical metric authority map

1. For Total Days Survived, Starvation Deaths Avoided, Relics Rescued, Treaties Ratified, identify exact canonical source and definition.
2. Reject counters with no authoritative historical data.
3. Do not reconstruct 'deaths avoided' from guesses at epilogue time.
4. Rename/omit unsupported metrics rather than fabricate them.
5. Document in EPILOGUE_METRIC_AUTHORITY_MAP.md.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28AV — Total Days Survived metric

1. Use CampaignCalendar/campaign completion day.
2. Define whether Day 0 counts.
3. Ensure paused wall-clock time is irrelevant.
4. Add boundary tests.
5. Localize formatting.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28AW — Starvation Deaths Avoided semantic audit

1. Determine whether the repository actually records prevented starvation deaths.
2. If no explicit counter/event exists, replace with a supportable metric such as starvation deaths, hunger crises survived, or days without starvation deaths.
3. Do not infer counterfactual lives saved from low hunger.
4. Document decision.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28AX — Relics Rescued metric

1. Identify canonical relic/museum/archive/recovery event owner.
2. Count unique rescued relic IDs once.
3. Do not count re-accession/re-display as rescue.
4. Integrate E1-26 only if its accession history is the canonical rescue source.
5. Add dedupe tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28AY — Treaties Ratified metric

1. Use E1-21 treaty ratification records.
2. Count unique ratified treaty IDs according to explicit scope.
3. Do not count proposals or temporary negotiation drafts.
4. Add save/load and revoked-treaty policy tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28AZ — Final survivor roster roll-call

1. Use final SurvivorCatalog/fate state.
2. Bind final roles, site, health status, key history, and authored life-outcome ID.
3. Do not mutate survivors during epilogue.
4. Support deceased/departed/recruited distinctions.
5. Add large-roster test.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28BA — Mental-health/trauma life-outcome boundary

1. Use E1-15/Psychology historical context as one input to authored outcome rules.
2. Do not convert a raw trauma score into an unsupported diagnosis.
3. Allow resilient/recovering/struggling/uncertain narrative bands where supported.
4. Respect sensitive-content language.
5. Add outcome-rule tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28BB — Fallen survivor memorial section

1. Use E1-23 death records and MemorialSystem.
2. Show verified name/date/cause/location/memorial context only according to canonical knowledge.
3. Do not duplicate death records.
4. Handle missing bodies/unknown causes respectfully.
5. Add zero/one/many fallen tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28BC — Epilogue audio

1. Fade in somber acoustic theme via AudioManager.
2. Respect music volume/mute/accessibility.
3. Do not make audio required for progression.
4. Fade out on return/inspection transitions.
5. Add no-audio test.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28BD — Export Chronicle data contract

1. Export a derived markdown/text chronicle from canonical epilogue snapshot and selected campaign history.
2. Do not expose hidden debug IDs or secret information the player never learned unless export is explicitly a developer report.
3. Use stable section ordering.
4. Include provenance-friendly dates/labels.
5. Support UTF-8/localized text.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28BE — Export Chronicle file operation

1. Use repository-safe export/file API.
2. Sanitize filename.
3. Do not block endgame if filesystem write fails.
4. Return visible success/failure path/message.
5. Add write-failure and content test.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28BF — Return to Main Menu

1. Route through existing game-state/main-menu transition.
2. Ensure epilogue audio and UI subscriptions clean up.
3. Do not mutate completed campaign record.
4. Prevent double transition.
5. Add route test.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28BG — Examine Final Shelter State

1. Provide read-only post-game inspection mode over frozen/completed campaign state.
2. Do not resume simulation unless the product explicitly supports post-victory sandbox.
3. Disable mutating commands in inspection mode.
4. Clearly label post-game state.
5. Add mutation-block tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28BH — Epilogue trigger wiring

1. Wire trigger in Main.Plans62_65.cs only from committed campaign victory/extinction/end-state event.
2. Do not poll victory condition every UI frame.
3. Use completion ID for idempotency.
4. Handle extinction and victory as distinct evaluated contexts.
5. Add trigger-once tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28BI — Campaign epilogue catalog integrity

1. Validate all vignette IDs in campaign_epilogues.json.
2. Validate translation keys and parameter schemas.
3. Validate every evaluator output references a real vignette.
4. Fail missing text at data-integrity selftest.
5. Add content-utilization report if appropriate.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28BJ — CampaignEpilogueModalTests

1. Headlessly bind all vignette slots.
2. Test sparse and full output.
3. Test large survivor roster, memorial section, metrics, export enabled, return, inspection, and reduced motion.
4. Assert no raw internal IDs.
5. Run scene lint.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28BK — Epilogue documentation

1. Create/update `docs/EPILOGUE_METRIC_AUTHORITY_MAP.md`.
2. Map each metric and vignette input to exact owner.
3. Document campaign completion trigger and snapshot semantics.
4. Document export and final-state inspection boundary.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28BL — Foundry 20-product catalog audit

1. Review every entry in `Assets/StreamingAssets/Data/foundry_production.json` across Tier 1–4.
2. Create a table of input materials, output quantity, time, heat target, power, facility/tool requirements, byproducts, consumer systems, and trade value.
3. Identify dead/duplicate/unconsumed products.
4. Do not rebalance before confirming units and real consumers.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28BM — Foundry product unit normalization

1. Verify all material amounts use consistent canonical units.
2. Normalize ambiguous scrap/ingot/plate/wafer quantities.
3. Ensure stack sizes and recipe outputs reconcile with Inventory item definitions.
4. Add data-integrity assertions.
5. Document any migration.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28BN — Vehicle recipe wiring for new products

1. Wire hardened_track_pins, radiation_ballast_plate, and microfluidic_wafer_blank into actual vehicle modification/service recipes where physically and mechanically justified.
2. Use canonical recipe references rather than hard-coded checks in VehicleGarage.
3. Ensure each product has at least one gameplay consumer.
4. Add recipe-consumption tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28BO — Hardened track pin consumer design

1. Identify a Mobility/heavy-duty vehicle mod or maintenance recipe that logically consumes hardened_track_pins.
2. Do not add track pins to wheeled vehicles without a reason.
3. Allow repair/fabrication use where applicable.
4. Validate item/recipe IDs.
5. Document consumer.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28BP — Radiation ballast plate consumer design

1. Use as Protection/chassis shielding material if the vehicle architecture supports mass/radiation tradeoffs.
2. Route radiation benefit through the same vehicle shielding system defined in E1-27.
3. Account for mass/speed/fuel cost if canonical design supports it.
4. Do not grant shielding twice.
5. Add integration test.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28BQ — Microfluidic wafer blank consumer design

1. Use in advanced sensors/control/electronics or a vehicle subsystem only if current technology tree supports it.
2. Do not force-fit into unrelated mechanical recipes merely to satisfy content utilization.
3. If no legitimate vehicle consumer exists, wire to another real advanced workshop/research consumer and document the exception.
4. Keep catalog utilization truthful.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28BR — Foundry heat ownership audit

1. Confirm SilentFoundrySystem.Heat.cs is the sole foundry thermal owner.
2. Map PowerGrid, room Thermal/Ventilation, cooling jacket, crucible count, facility condition, and hazard outputs.
3. Prevent duplicate heat values in production queue or UI.
4. Create `docs/foundry/FOUNDRY_HEAT_AUTHORITY.md`.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28BS — Active crucible heat-input model

1. Calculate heat input/load from active crucible count plus recipe/process class.
2. Use calibrated game coefficients with explicit units.
3. Do not claim real-world precision without geometry/material parameters.
4. Support idle, one-crucible, and max-crucible fixtures.
5. Use deterministic arithmetic.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28BT — Thermal dissipation model

1. Model dissipation from current foundry heat, ambient/room thermal sink, cooling capability, ventilation where applicable, and facility condition.
2. Use stable timestep-independent integration or validated fixed-step semantics.
3. Prevent negative heat and numerical explosion.
4. Add equilibrium/cooling tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28BU — Cooling jacket mechanics

1. Define PowerGrid draw, cooling capacity, condition dependency, and failure behavior.
2. PowerGrid owns electricity; E1-17/maintenance owns hardware condition if integrated.
3. Foundry heat owner consumes effective cooling capability.
4. Add power-loss/brownout/degraded-jacket tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28BV — Foundry power draw

1. Calculate active furnace/crucible/cooling power through canonical PowerGrid load contracts.
2. Do not deduct power as inventory.
3. Expose forecast/actual draw.
4. Add multiple-crucible and cooling-jacket tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28BW — 48-hour max-heat exposure tracker

1. Define 'max heat' threshold as an explicit temperature/heat band, not exactly one floating-point equality.
2. Accumulate continuous exposure time while above threshold.
3. Reset/decay according to documented policy.
4. Use campaign simulation time.
5. Persist accumulator for save compatibility.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28BX — Thermal runaway hazard

1. After >48h above critical band, submit a hazard eligibility event to canonical structure/fire systems.
2. Do not directly crack shelter walls or start fire inside SilentFoundrySystem if another owner exists.
3. Use keyed RNG for hazard occurrence, partitioned by foundry/site/day or incident ID.
4. Persist decision/incident receipt to prevent reroll.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28BY — Structural cracking handoff

1. Route structural damage to E1-17/structure/condition authority.
2. Use foundry heat exposure severity as input.
3. Do not store duplicate structural crack state.
4. Add no-owner fallback that disables this consequence rather than inventing parallel state.
5. Add repair integration test.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28BZ — Fire hazard handoff

1. Route fire ignition to canonical fire/hazard system.
2. Use location, severity, fuel context, and source incident provenance.
3. Do not directly apply survivor damage.
4. Add prevented/ignited/extinguished scenario tests.
5. Journal/alarms consume committed fire event.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28CA — Foundry acoustic integration

1. Add heavy blast-furnace drone and crucible clangs through ShelterAcousticDirector.
2. Loop only while canonical foundry operating state warrants it.
3. Deduplicate starts/stops.
4. Audio never drives heat or production.
5. Respect mute/accessibility.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28CB — 100-day foundry balance simulation

1. Create `scripts/sim/foundry_balance_sweep.py`.
2. Simulate representative continuous manufacturing over 100 days using catalog values.
3. Report throughput, scrap/ore consumption, power use, heat distribution, cooling saturation, queue utilization, product stock, trade value, and starvation of downstream consumers.
4. Do not write catalog values automatically.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28CC — Foundry simulation scenarios

1. Run at least low-input survival, balanced midgame, aggressive vehicle production, trade-focused, and max-throughput scenarios.
2. Vary crucible count and cooling availability.
3. Record when critical heat exposure occurs.
4. Compare Tier 1–4 bottlenecks.
5. Export CSV/markdown summary.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28CD — Scavenging input equilibrium audit

1. Measure canonical scrap/metal inflow from representative scavenging/expedition rates.
2. Compare with foundry demand for top-tier products and vehicle/workshop consumers.
3. Do not require exact 1:1 equilibrium if scarcity is intentional.
4. Define sustainable, strained, and impossible regimes.
5. Document expected player tradeoffs.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28CE — Product cost balance

1. Review all 20 recipes for material conservation, tier progression, time, heat, power, and downstream value.
2. Flag dominated products and obvious arbitrage loops.
3. Keep advanced products materially more constrained without making them unreachable.
4. Use simulation evidence for changes.
5. Record before/after values.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28CF — Processed ingot trade integration

1. Wire processed ingots/products into canonical faction trading tables through E1-19/Market.
2. Premium barter rates must come from trade valuation policy, not foundry-local price fields unless catalog architecture says otherwise.
3. Prevent buy-smelt-sell infinite arbitrage.
4. Add price-floor/ceiling and loop tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28CG — Faction demand differentiation

1. Only factions with plausible industrial demand should pay premiums for relevant processed goods.
2. Use faction trade preferences rather than one universal multiplier.
3. Do not duplicate faction standing/economy.
4. Add representative faction fixtures.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28CH — Foundry-to-vehicle economic integration test

1. Create `FoundryVehicleEconomicIntegrationTests.cs`.
2. Craft required foundry product through canonical queue.
3. Move output through Inventory.
4. Consume it in VehicleGarage modification/repair recipe.
5. Assert item conservation and no duplicate reward.
6. Verify trade alternative uses same item.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28CI — Foundry queue determinism

1. Seed identical queue/input/power/cooling scenarios.
2. Compare completion days, heat curves, power requests, hazard decisions, outputs, and queue order.
3. Use keyed RNG only for true hazards.
4. Add call-order perturbation tests.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28CJ — Foundry save compatibility

1. Round-trip a save with all 20 product recipes queued or represented across queue/history where supported.
2. Persist heat state, critical exposure accumulator, active jobs, reservations, and stable IDs according to owner contracts.
3. Do not replay material consumption or outputs.
4. Add old-version migration fixtures.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28CK — Foundry data integrity

1. Run --data-integrity-selftest for all 20 item IDs, recipe keys, input/output refs, facility/heat requirements, trade refs, and vehicle consumers.
2. Reject duplicate IDs and missing items.
3. Reject recipes with negative/zero invalid quantities.
4. Provide actionable diagnostics.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28CL — Foundry content utilization

1. Run --content-utilization-selftest.
2. Require every foundry product to have a real gameplay consumer or an explicitly documented catalog-only/deferred classification.
3. Do not fake utilization with tests/docs alone.
4. Prefer removing or rewiring dead products over keeping decorative content.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28CM — FoundryHeatRegulationTests

1. Add tests for cooling jacket, power draw, one/multiple crucibles, dissipation, equilibrium, power loss, degraded condition, threshold exposure accumulation, and hazard eligibility.
2. Use deterministic expected values.
3. Cover timestep boundaries.
4. Fail on NaN/overflow/negative heat.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28CN — Foundry product documentation

1. Update `docs/foundry/FOUNDRY_PRODUCT_CATALOG.md` with all 20 recipes, tiers, inputs, outputs, process time, heat/power needs, consumers, trade role, and balance notes.
2. Document new vehicle integrations.
3. Document hazard and cooling assumptions.
4. Reference balance-sweep evidence.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28CO — Cross-plan save/replay audit

1. Test captive interrogation/tribunal, curing/freezer, epilogue trigger/export state, and foundry queue/hazard across save boundaries.
2. Verify no action cost, recruitment conversion, curing output, epilogue trigger, or foundry output duplicates.
3. Use stable operation/event IDs.
4. Add corruption fallback tests where state references are missing.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28CP — Cross-plan localization audit

1. Scan prisoner topics/dialogue, food recipes/storage labels, epilogue vignettes/metrics, and foundry product UI labels.
2. Ensure zero raw internal IDs leak into normal UI.
3. Validate parameter placeholders.
4. Fail missing critical translations.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28CQ — Cross-plan accessibility audit

1. Provide text equivalents for hostility/resistance/temperature/spoilage/heat gauges.
2. Support keyboard/controller focus.
3. Support reduced motion in epilogue.
4. Ensure alarms have visible semantic alerts.
5. Do not require audio to understand captive/freezer/foundry events.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28CR — Cross-plan performance budget

1. Captive and food panels refresh event-driven, not per frame.
2. Cohort lists handle realistic maximum inventory counts without expensive full rebuilds.
3. Epilogue large roster/history rendering is bounded and can lazily populate sections.
4. Foundry heat update remains cheap at maximum crucible count.
5. Record median/p95 and allocation budgets.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28CS — Long-campaign integrated soak

1. Run maximum supported campaign with captives, parole/recruitment, food curing/freezer outages, endgame metric accumulation, and foundry production/trade/vehicle consumption.
2. Track save size, queued jobs, cohorts, captive histories, metric counters, foundry heat/hazards, and UI list sizes.
3. Verify bounded history and no duplicate operations.
4. Capture performance summary.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28CT — Documentation pack

1. Update `docs/CAPTIVE_SYSTEM_AUTHORITY_MAP.md` with parole tribunal UI and guard/labor/psychology boundaries.
2. Update `docs/FOOD_PRESERVATION_AUTHORITY_MAP.md` with pantry UI, curing, power, thermal, and ration policy.
3. Update `docs/EPILOGUE_METRIC_AUTHORITY_MAP.md` with exact metric owners.
4. Update `docs/foundry/FOUNDRY_PRODUCT_CATALOG.md` and create/update heat authority notes.
5. Add UI panel inventory entries for PrisonerManagementPanel, FoodPreservationPanel, and CampaignEpilogueModal.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

## E1-28CU — Release gate

1. Run dotnet build/tests and game build.
2. Run scene-lint for both panels and epilogue modal.
3. Run captive UI, food UI, epilogue UI, foundry heat/economic integration, save migration, data-integrity, content-utilization, and determinism tests.
4. Run the 100-day foundry simulation and maximum-campaign soak.
5. Verify no duplicate captive health/security/recruitment, food spoilage/power/thermal, epilogue history, or foundry inventory/heat/trade authority exists.
6. Mark DONE only when all four slices pass together.

**Package gate:** preserve canonical ownership, stable operation/event IDs, deterministic replay, save/load idempotency, and relevant conservation. UI remains projection + command only; sensitive/coercive actions require explicit canonical consequence routing rather than hidden direct stat edits.

---

# 4. Captive UI Contract

A UI-safe captive presentation DTO should contain only canonical or player-known fields:

```yaml
captive_id: "captive_014"
display_name_key: "captive_unknown_raider"
origin_faction_label_key: "faction_iron_raiders"

resistance:
  display_band: "HIGH"
  canonical_permille: 760

hostility:
  display_band: "HOSTILE"
  canonical_permille: 830

health_summary:
  band: "STABLE"
starvation_summary:
  band: "HUNGRY"

cell_id: "cell_b_03"
security:
  posture: "ADEQUATE"
  escape_risk_permille: 180
```

If resistance/hostility values are not player-known, replace numeric display with evidence/qualitative bands.

# 5. Interrogation Action Boundary

```text
PrisonerManagementPanel
-> InterrogationCommand(operation_id, captive_id, approach_id, topic_id, actor_id)
-> captive/interrogation authority validates
-> canonical resources/time/evidence/health restrictions validated
-> operation persisted
-> deterministic or keyed outcome
-> intelligence handoff
-> psychological/security consequences
-> UI refresh
```

The panel never subtracts resistance, food, health, or morale directly.

# 6. Parole Tribunal Boundary

```text
UI requests tribunal
-> Governance/tribunal validates
-> evidence + security + health + policy considered
-> committed result
   -> continued detention
   -> amnesty/release
   -> supervised parole
   -> recruitment referral
-> downstream owners execute physical/status transitions
```

Recruitment referral is not survivor creation.

# 7. Captive Labor Boundary

```text
parole/legal status permits work
-> Duty assignment
-> guard/escort requirement
-> Production job
-> completed work output
```

No production is credited merely because the captive UI shows "assigned".

# 8. Food Cohort Contract

A food cohort projection should reference canonical inventory/freshness state:

```yaml
cohort_id: "food_cohort_044"
item_id: "food_meat_raw"
quantity: 18
storage_container_id: "root_cellar_01"
freshness_permille: 615
estimated_spoilage_hours: 39
warning_48h: true
```

The countdown is derived from the spoilage owner and current storage environment.

# 9. Curing Transaction

```text
UI selects recipe
-> FoodPreservation validates recipe/facility
-> Inventory reserves meat + salt/chemicals
-> Duty assigns cook
-> job starts
-> inputs consumed at canonical commit point
-> time/process advances
-> preserved output created once
```

# 10. Freezer Boundary

PowerGrid owns electrical supply.
Thermal/FoodPreservation owns freezer temperature/integrity.
Inventory/FoodPreservation owns food preservation effect.

The panel owns none of these.

# 11. Ration Policy Semantics

### Consume Oldest First
Prefer cohorts closest to spoilage, subject to recipe/safety constraints.

### Prioritize Fresh
Prefer highest-freshness cohorts when quality is the goal.

### Emergency Deep Freeze
Prefer protecting long-term/freezer reserves and consume non-freezer/perishable cohorts according to canonical
policy.

The Kitchen panel and execution pipeline must use the same policy owner.

# 12. Epilogue Metric Provenance

Every displayed metric must have:

- metric ID;
- canonical source;
- definition;
- time window;
- dedupe key;
- display formatter.

If a metric cannot be supported—especially a counterfactual such as "starvation deaths avoided"—rename or omit
it rather than fabricate it.

# 13. Epilogue Survivor Outcome Rule

Inputs can include:

- alive/dead/departed/recruited status;
- final role/site;
- canonical health;
- E1-15 psychological history;
- relationship/family history;
- major quest/achievement history;
- campaign outcome.

Output is an authored narrative vignette ID.

It must not diagnose a medical condition from one numeric score.

# 14. Chronicle Export Structure

```text
# ASHFALL Campaign Chronicle
## Campaign Outcome
## Pillar Vignettes
## Historical Metrics
## Final Survivor Roll-Call
## Fallen Survivors
## Major Treaties
## Relics / Cultural History
## Shelter Final State Summary
```

All facts remain derived from canonical campaign state/history.

# 15. Foundry Heat Model

A useful deterministic abstraction:

```text
heat_next =
heat_current
+ process_heat_input(active_crucibles, recipes)
- cooling_dissipation(delta_to_ambient, cooling_capability, ventilation, condition)
```

Use calibrated coefficients and stable timestep semantics.

Avoid false thermodynamic precision unless the repository actually models mass, heat capacity, geometry, and
heat-transfer coefficients.

# 16. Critical-Heat Exposure

```text
if heat >= critical_band:
    critical_exposure += dt
else:
    critical_exposure = reset_or_decay(policy)
```

After the configured duration threshold (source target: >48h), the foundry becomes eligible to submit a
structural/fire hazard event. The hazard outcome uses keyed RNG and canonical hazard owners.

# 17. Foundry Conservation

```text
Inventory inputs
-> queued foundry job
-> consumed inputs
-> processed outputs
-> Inventory
-> vehicle/workshop/trade consumer
```

No hidden product grant occurs at any integration edge.

# 18. Foundry 20-Product Balance Worksheet

For every product capture:

| Field | Required |
|---|---|
| Product ID | yes |
| Tier | 1–4 |
| Inputs | IDs + quantities |
| Output quantity | canonical units |
| Process time | campaign time |
| Target heat | canonical units |
| Power | canonical load |
| Crucible/facility | capability |
| Cooling sensitivity | if relevant |
| Byproducts | if any |
| Primary consumer | gameplay system |
| Secondary consumer | optional |
| Trade demand | faction/market policy |
| 100-day production | simulation result |
| Bottleneck | material/time/heat/power |
| Balance action | keep/tune/rewire/remove |

# 19. New Foundry Product Integration Rules

### hardened_track_pins
Use only where tracked/heavy mobility or mechanically justified repair/fabrication consumes them.

### radiation_ballast_plate
Use as real shielding/chassis material and route protection through E1-27's canonical radiation shielding path.

### microfluidic_wafer_blank
Use only where advanced electronics/sensors/medical/vehicle control systems plausibly consume it. Do not force
it into a vehicle recipe merely to satisfy utilization.

# 20. Foundry Trade Anti-Arbitrage

Test:

```text
buy raw scrap
-> smelt/refine
-> sell processed good
```

Profit may exist, but it must account for:

- power;
- labor;
- time;
- heat/cooling burden;
- facility wear;
- risk;
- market spread;
- faction demand.

Reject risk-free infinite loops.

# 21. Cross-Plan Exploit Matrix

| Failure | Guard |
|---|---|
| UI reduces captive resistance directly | command-only boundary |
| Ration deprivation edits health in UI | Needs/Health handoff |
| Tribunal creates survivor directly | Recruitment handoff |
| Captive labor credits output without job | Duty/Production receipt |
| Guard trauma applies twice | interrogation event ID |
| Food panel duplicates stock | canonical Inventory cohorts |
| Two spoilage clocks tick | one freshness owner |
| Freezer toggle writes wattage directly | PowerGrid command |
| Kitchen ignores ration policy | shared allocator |
| Epilogue invents unsupported metric | metric authority map |
| Epilogue rerolls life outcome on reload | completion snapshot |
| Chronicle leaks hidden debug data | export filter |
| Foundry creates product twice | Inventory transaction |
| Heat integrated in two systems | heat authority audit |
| 48h runaway rerolls after reload | hazard decision receipt |
| Structural cracking stored in foundry | structure/E1-17 handoff |
| Trade loop mints value | 100-day/economic tests |
| Vehicle recipe grants foundry item again | consumer-only recipe |
| Dead foundry IDs pass docs-only utilization | content selftest |

# 22. Required Test Families

Captive:
- `PrisonerManagementPanelTests`
- `CaptiveUiAuthorityBoundaryTests`
- `CaptiveInterrogationCommandTests`
- `CaptiveTopicExtractionTests`
- `CaptiveSecurityProjectionTests`
- `ParoleTribunalIntegrationTests`
- `CaptiveRecruitmentHandoffTests`
- `CaptiveLaborDispatchTests`
- `CaptiveGuardPsychologyHandoffTests`

Food:
- `FoodPreservationPanelTests`
- `FoodPreservationUiAuthorityBoundaryTests`
- `FoodCohortTimelineTests`
- `FoodSpoilageWarningTests`
- `FoodCuringJobTests`
- `FoodFreezerPowerTests`
- `FoodFreezerTemperatureTests`
- `FoodRationPolicyTests`
- `KitchenFoodCohortIntegrationTests`
- `FoodSpoilageClockSingularityTests`

Epilogue:
- `CampaignEpilogueModalTests`
- `EpilogueAuthorityBoundaryTests`
- `EpilogueMetricAuthorityTests`
- `EpilogueVignetteBindingTests`
- `EpilogueSurvivorOutcomeTests`
- `EpilogueMemorialIntegrationTests`
- `EpilogueTriggerIdempotencyTests`
- `EpilogueChronicleExportTests`
- `FinalShelterInspectionModeTests`

Foundry:
- `FoundryProductCatalogTests`
- `FoundryVehicleEconomicIntegrationTests`
- `FoundryHeatRegulationTests`
- `FoundryThermalRunawayTests`
- `FoundryStructuralHazardHandoffTests`
- `FoundryFireHazardHandoffTests`
- `FoundryTradeIntegrationTests`
- `FoundryQueueDeterminismTests`
- `FoundrySaveCompatibilityTests`
- `FoundryContentUtilizationTests`

# 23. Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj

godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --content-utilization-selftest
```

Also run:

```bash
python scripts/scene-lint.py assets/ui/panels/PrisonerManagementPanel.tscn
python scripts/scene-lint.py assets/ui/panels/FoodPreservationPanel.tscn
python scripts/scene-lint.py assets/ui/modals/CampaignEpilogueModal.tscn
python scripts/sim/foundry_balance_sweep.py
```

Use the repository's actual invocation path/options if these scripts accept different CLI arguments.

# 24. Completion Checklist

## Captives
- [ ] PrisonerManagementPanel implements IContractPanel.
- [ ] Scene passes lint with zero warnings.
- [ ] Captive profile binds canonical health/security data.
- [ ] Resistance/hostility presentation semantics are documented.
- [ ] Interrogation approaches dispatch commands only.
- [ ] Ration deprivation cannot bypass Needs/Health.
- [ ] Topic extraction routes into canonical intelligence.
- [ ] Guard widget derives security/escape risk.
- [ ] Tribunal routes through Governance.
- [ ] Recruitment routes through Recruitment/SurvivorCatalog.
- [ ] Labor routes through Duty/Production.
- [ ] Guard trauma routes through E1-15/Needs.
- [ ] Acoustic cues are presentation-only.
- [ ] All interrogation text is localized.

## Food preservation
- [ ] FoodPreservationPanel implements IContractPanel.
- [ ] Scene passes lint.
- [ ] Food cohorts are canonical.
- [ ] Spoilage countdown is derived.
- [ ] 48-hour warnings are correct.
- [ ] Curing job is transactional.
- [ ] Freezer power is PowerGrid-owned.
- [ ] Temperature integrity is canonical.
- [ ] Failure alarm is deduped.
- [ ] Ration policy uses one allocator.
- [ ] Kitchen and panel use the same cohort-selection policy.
- [ ] No duplicate spoilage clock exists.

## Epilogue
- [ ] CampaignEpilogueModal fills full screen correctly.
- [ ] Campaign end trigger is idempotent.
- [ ] EvaluateEpilogue output binds all vignette slots.
- [ ] Reduced-motion/skip exists.
- [ ] Every metric has an authority definition.
- [ ] Unsupported counterfactual metrics are renamed/removed.
- [ ] Survivor outcomes are authored and non-diagnostic.
- [ ] Memorial section uses E1-23.
- [ ] Audio respects settings.
- [ ] Chronicle export is deterministic and filtered.
- [ ] Final-state inspection is read-only.
- [ ] campaign_epilogues.json and translations validate.

## Foundry
- [ ] All 20 products audited.
- [ ] Units normalized.
- [ ] Three new products have legitimate consumers.
- [ ] Vehicle integrations use canonical recipes.
- [ ] Heat owner is singular.
- [ ] Active crucible count influences heat.
- [ ] Dissipation is timestep-safe.
- [ ] Cooling jacket and power tests pass.
- [ ] 48h critical exposure persists through save.
- [ ] Structural/fire hazards route to canonical owners.
- [ ] Audio is presentation-only.
- [ ] 100-day balance sweep completes.
- [ ] Scavenging/consumption equilibrium is documented.
- [ ] Trade premiums do not create arbitrage.
- [ ] FoundryVehicleEconomicIntegrationTests passes.
- [ ] Queue replay is deterministic.
- [ ] Save with all 20 products round-trips.
- [ ] Data-integrity selftest passes.
- [ ] Content-utilization selftest passes.
- [ ] Foundry product documentation updated.

- [ ] `E1_planintegration[29].md` is the next sequence filename.

# 25. Final Directive

These four follow-ups should deepen the game without multiplying authority.

The captive panel should expose real detention, security, health, interrogation, parole, labor, and recruitment
state while keeping sensitive/coercive actions explicit and consequential. The food panel should expose real
cohorts, preservation jobs, power, temperature, and ration policy without becoming a second pantry database.
The epilogue should transform canonical campaign history into a coherent ending without fabricating metrics or
diagnoses. The foundry should become economically and thermally legible without duplicating Inventory, Power,
structural damage, fire, trade, or vehicle crafting.

The architectural standard is:

**UI projects and commands; canonical domain systems own state; history is sourced; material flows conserve;
stochastic outcomes are keyed; save/load cannot replay consequences.**

If any panel starts mutating simulation truth directly, if the epilogue invents unsupported facts, if foundry
heat or spoilage exists in two places, or if a coercive captive action bypasses Health/Security/Psychology/
Governance, stop and restore the boundary before proceeding.
