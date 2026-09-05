# ASHFALL — 60-Issue Flagship Repository Remediation Plan

**File:** `20074ashfall_60_issue_flagship_remediation_plan.md`
**Repository:** `GermanRobert-Labtester/Atomic-War-Starving-Survival`
**Audited baseline:** `main` @ `9b4985d0122d707c31f6078050df5877b69b607b`
**Scope:** Whole-game repository remediation across Core, Godot host, UI, gameplay causality, data authority, dead/unwired code, event lifecycle, save/load, determinism, and compiler-warning hygiene.

---

## 0. Mission

Convert the current ASHFALL repository from a system-rich but frequently under-connected codebase into a repository where every player-facing capability proves an end-to-end live path:

`JSON / Core authority → composition → host session → player route → command → canonical mutation → feedback → save/load → regression gate`

This plan intentionally prioritizes **liveness and authority integrity over adding more content**. The dominant failure mode is not lack of systems; it is systems, panels, loaders, and tests that exist independently without a fully closed gameplay loop.

### Flagship success condition

A feature is considered **SHIPPED** only when all of the following are true:

1. It has one canonical authority.
2. Production composition creates or loads that authority.
3. A host/read-model surface exposes it without duplicating state.
4. A player route can reach it.
5. At least one meaningful player action calls a typed command.
6. That command changes canonical state.
7. The consequence is observable in another system or player-facing read model.
8. Save/load preserves the result.
9. Deterministic inputs remain deterministic.
10. A gate or integration test prevents the seam from becoming unwired again.

A panel that only opens is **not shipped**.
A loader with passing unit tests but no production caller is **not shipped**.
A command that only changes a UI label is **not shipped**.
A gameplay mechanic backed by a second throwaway authority is **not shipped**.

---

## 1. Severity Model

- **Critical (P0):** breaks a core survival loop, mutates the wrong authority, causes player actions to do nothing, makes endings/choices false, or permits systemic state divergence.
- **High (P1):** player-routable false affordance, serious event/lifecycle leak, dead progression seam, dual data authority, or major content/system reachability failure.
- **Medium (P2):** dead subsystem with limited current player impact, warning suppression, maintainability issue, or incomplete feature that should either be wired or removed.

---

## 2. Execution Order

### Wave A — P0 Causal Spine
Execute first. These determine whether ASHFALL is mechanically honest.

- 31 Moral-choice decision spine
- 32 Derived epilogue (RESOLVED via FX-01)
- 39 Fire incident authority ownership
- 42 Environmental radiation
- 43–44 Protective-gear wear
- 45–47 One consumption authority
- 50 Trapping outputs
- 51 Water authority
- 53 Work-fitness gate

### Wave B — P1 System Integration
Close important under-connected systems.

- 33–38 UI lifecycle/reachability
- 40–41 faction authority duplication
- 48–49 kitchen and skill progression
- 52 water-ration seam disposition
- 54 craft attribution
- 55 radio JSON authority
- 59 eulogy death pipeline

### Wave C — Flagship Console Triage
Issues 1–30 are handled with one rule:

> **Wire it completely or remove it from player navigation.**

Do not preserve a player-routable console merely because the panel file exists or looks polished.

### Wave D — Dead-Code / Hygiene Closure
- 56 Atmosphere text
- 57 Environmental text
- 58 Debt templates
- 60 CS8618 suppression

---

## 3. Repository-Wide Guardrails

### 3.1 One-authority rule
No UI route may instantiate a gameplay authority with `new SomeSystem()` unless the object is explicitly a short-lived command/value object. Long-lived campaign state must come from composition.

### 3.2 Player-route liveness gate
For every registered player-routable panel ID, record:

- panel class,
- authoritative Core/system owner,
- host/read model,
- bind method,
- at least one typed action,
- expected state mutation,
- save section,
- journey/integration test.

A route with no authority or no state-mutating command must be classified `READ_ONLY_INTENTIONAL`, `DEV_ONLY`, or removed.

### 3.3 Loader liveness gate
Every retained production catalog loader must have:
- at least one production call site, or
- an explicit dormant/extension allowlist entry with owner and rationale.

### 3.4 No throwaway gameplay authorities in UI
Add a source gate for suspicious route-level constructions such as:
- `new *System(`
- `new *Engine(`
- `new *HostSession(`

under `src/Main.PlayerSurfaces.cs` and other route registries, with an allowlist only for stateless helpers.

### 3.5 Stable event subscription policy
No `+= _ => ...` paired with `-= _ => ...` for long-lived events. Named or cached delegates only.

### 3.6 Canonical consume contract
Food, water, medicine, iodine, anti-rad, contaminated consumables, and cooked meals must converge on one effect application path.

### 3.7 Production-output sink
Any system that creates goods must deliver through a shared capacity-aware sink or explicitly retain pending output.

### 3.8 Save-state rule
Every new state mutation introduced by this plan must be checked against capture/restore and checksum/gate coverage before task closeout.

---

## 4. Definition of Done — Per Task

Every task below must close with:

1. **Evidence read:** exact files inspected.
2. **Authority decision:** state owner identified.
3. **Implementation:** no duplicate authority introduced.
4. **Unit tests:** local domain behavior.
5. **Integration test:** host/player path reaches behavior.
6. **Save/load test:** when stateful.
7. **Determinism test:** when RNG/time ordering involved.
8. **UI test:** when player-facing.
9. **Negative-path test:** invalid/blocked command.
10. **Repo sweep:** confirm old stub/fallback/dead path is gone.
11. **Build:** zero errors and zero new warnings.
12. **Selftests:** relevant Godot headless gates pass.
13. **Evidence note:** update current architecture/authority docs rather than stale planning docs.

---

## 5. Detailed Remediation Tasks

### Task 01 — Anaerobic Biogas Digester console is a false affordance

**Severity:** High
**Primary files/surfaces:** `src/UI/AnaerobicBiogasDigesterPanel.cs; src/Main.PlayerSurfaces.cs; Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs`

**Objective:** Replace literal telemetry and feedback-only actions with a real digester authority/host contract, or remove the player route until the mechanic is live.

**Implementation steps**
1. Create or identify the canonical anaerobic digestion domain state.
2. Add a host session/read model that exposes live batch, temperature, pH, feedstock, gas output, faults, and power state.
3. Bind panel actions to typed commands instead of status strings.
4. Add a route-liveness test proving at least one panel action mutates campaign state.

**Acceptance criteria**
- Opening the panel displays campaign-owned values.
- At least one player command changes canonical state and survives save/load.
- No literal operational telemetry remains except deliberate demo/test fixtures.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 02 — Subterranean Cartography console is unwired

**Severity:** High
**Primary files/surfaces:** `src/UI/SubterraneanCartographyPanel.cs; src/Main.PlayerSurfaces.cs`

**Objective:** Bind the screen to canonical map/discovery state or remove it from player routing.

**Implementation steps**
1. Use the existing expedition/map authority as the source of discovered sectors and annotations.
2. Replace local/static map rows with a host read model.
3. Wire map actions to real reveal/annotation/route operations.
4. Add a route test that proves the panel reflects a changed map state.

**Acceptance criteria**
- No duplicated map authority.
- Panel state changes after a real discovery event.
- Route remains hidden if no live contract exists.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 03 — Underground Printing Press console is unwired

**Severity:** High
**Primary files/surfaces:** `src/UI/UndergroundPrintingPressPanel.cs; src/Main.PlayerSurfaces.cs`

**Objective:** Create a production contract for printing or quarantine the UI.

**Implementation steps**
1. Define inputs, recipes, power cost, wear, outputs, and queue state.
2. Reuse inventory/power authorities rather than local counters.
3. Bind all buttons to commands with failure reasons.
4. Persist active jobs if printing is meant to span time.

**Acceptance criteria**
- Every visible control has a domain command.
- Output reaches canonical inventory.
- Power/resource failure states are visible and tested.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 04 — Silicon Ingot Slicing console is unwired

**Severity:** High
**Primary files/surfaces:** `src/UI/SiliconIngotSlicingPanel.cs; src/Main.PlayerSurfaces.cs`

**Objective:** Wire slicing into canonical production, inventory, power, and equipment state.

**Implementation steps**
1. Define the ingot-to-wafer recipe contract.
2. Connect input reservation and capacity-aware output delivery.
3. Model machine wear/power dependency if the system is retained.
4. Remove route if production scope is not approved.

**Acceptance criteria**
- No feedback-only buttons.
- Material conservation is testable.
- Output appears in canonical inventory.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 05 — Geothermal Steam Turbine console is unwired

**Severity:** High
**Primary files/surfaces:** `src/UI/GeothermalSteamTurbinePanel.cs; src/Main.PlayerSurfaces.cs; Assets/Ashfall.Core/Shelter/PowerGridSystem.cs`

**Objective:** Make the turbine a real generator feeding the shared power grid.

**Implementation steps**
1. Define generation capacity, startup conditions, faults, maintenance, and fuel/heat dependencies.
2. Feed generated power into PowerGridSystem rather than a UI-local number.
3. Expose brownout/fault state through the panel.
4. Add deterministic generation tests.

**Acceptance criteria**
- Power-grid supply changes when turbine state changes.
- State survives save/load.
- Panel cannot claim generation while the grid receives zero watts.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 06 — War Dog Kennel console is unwired

**Severity:** High
**Primary files/surfaces:** `src/UI/WarDogKennelPanel.cs; src/Main.PlayerSurfaces.cs`

**Objective:** Either implement a canonical kennel/animal system or remove the active route.

**Implementation steps**
1. Define animals, handlers, condition, feeding, training, and assignment.
2. Connect kennel costs to inventory and duty roster.
3. Expose real consequences to patrol/expedition/security systems.
4. Add one end-to-end assignment test.

**Acceptance criteria**
- Kennel actions consume/produce real state.
- Assigned animals affect at least one gameplay system.
- No duplicate survivor/animal roster.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 07 — Isotope Separator console is unwired

**Severity:** High
**Primary files/surfaces:** `src/UI/IsotopeSeparatorPanel.cs; src/Main.PlayerSurfaces.cs`

**Objective:** Bind to real radiological production state.

**Implementation steps**
1. Define canonical input/output materials and contamination risk.
2. Connect power and equipment condition.
3. Add typed start/abort/service commands.
4. Surface hazardous failure states through the existing alert/event layer.

**Acceptance criteria**
- Outputs reach inventory.
- Hazard state can be reproduced by tests.
- Panel values come from one authority.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 08 — Plasma Arc Smelting console is unwired

**Severity:** High
**Primary files/surfaces:** `src/UI/PlasmaArcSmeltingPanel.cs; src/Main.PlayerSurfaces.cs`

**Objective:** Wire smelting to the existing production stack.

**Implementation steps**
1. Use canonical recipes and inventory item IDs.
2. Model power draw and machine condition.
3. Deliver products via capacity-aware output sink.
4. Add balance/mass-conservation assertions.

**Acceptance criteria**
- No local resource counters.
- Grid outages stop work.
- Inputs/outputs balance.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 09 — Borehole Seismograph console is unwired

**Severity:** High
**Primary files/surfaces:** `src/UI/BoreholeSeismographPanel.cs; src/Main.PlayerSurfaces.cs`

**Objective:** Connect it to a real world-information producer.

**Implementation steps**
1. Define what seismic data can reveal and who owns that knowledge.
2. Bind readings to world/map state.
3. Ensure scanning has time/power/cost where appropriate.
4. Persist discovered intelligence, not raw UI state.

**Acceptance criteria**
- A completed scan changes canonical world knowledge.
- Panel refreshes from that knowledge after reload.
- No hard-coded discovery text.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 10 — Heavy Logistics Airlock console is unwired

**Severity:** High
**Primary files/surfaces:** `src/UI/HeavyLogisticsAirlockPanel.cs; src/Main.PlayerSurfaces.cs`

**Objective:** Connect airlock logistics to inventory/expedition/vehicle state.

**Implementation steps**
1. Define inbound/outbound manifests.
2. Bind loading/unloading to capacity and ownership rules.
3. Surface blocked reasons for weight, hazard, security, and unavailable vehicle.
4. Add a transfer conservation test.

**Acceptance criteria**
- Transfers change canonical inventories only once.
- Capacity limits are enforced.
- Save/load preserves in-progress logistics if supported.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 11 — Cryogenic Permafrost Core console renders literal telemetry and non-operational controls

**Severity:** High
**Primary files/surfaces:** `src/UI/CryogenicPermafrostCorePanel.cs; src/Main.PlayerSurfaces.cs`

**Objective:** Replace the mock console with a live authority or quarantine it.

**Implementation steps**
1. Remove literal depth/temperature data.
2. Define the domain model for core storage/analysis.
3. Wire all actions to typed commands.
4. Add state transition and snapshot tests.

**Acceptance criteria**
- No operational value is hard-coded.
- At least one action produces a real consequence.
- Unimplemented controls are disabled/hidden, never fake-success.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 12 — Basal Radon Migration console is unwired

**Severity:** High
**Primary files/surfaces:** `src/UI/BasalRadonMigrationPanel.cs; src/Main.PlayerSurfaces.cs`

**Objective:** Bind to environmental/radiation state.

**Implementation steps**
1. Derive radon risk from canonical world/shelter state.
2. Connect mitigation to ventilation/power where appropriate.
3. Emit health/exposure consequences through canonical systems.
4. Add deterministic exposure tests.

**Acceptance criteria**
- Displayed risk matches domain state.
- Mitigation changes real exposure.
- No separate radiation authority.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 13 — Trauma Bonding Cohort console is unwired

**Severity:** High
**Primary files/surfaces:** `src/UI/TraumaBondingCohortPanel.cs; Assets/Ashfall.Core/Survivors/*; src/Main.PlayerSurfaces.cs`

**Objective:** Bind to the survivor-social authority.

**Implementation steps**
1. Use canonical relationship/trauma state.
2. Remove local fake cohesion metrics.
3. Route any interventions through survivor-social commands.
4. Make outcomes visible to duty/expedition/caregiving consumers where designed.

**Acceptance criteria**
- Panel reflects real pair/cohort state.
- Interventions affect canonical relationships.
- No duplicate relationship graph.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 14 — Clandestine Insurgency console is unwired

**Severity:** High
**Primary files/surfaces:** `src/UI/ClandestineInsurgencyPanel.cs; src/Main.PlayerSurfaces.cs`

**Objective:** Create a real faction/insurgency contract before presenting the screen.

**Implementation steps**
1. Define authoritative cells, pressure, intelligence, actions, and consequences.
2. Reuse faction/world systems rather than UI-local variables.
3. Gate unavailable mechanics until their backing system exists.
4. Add campaign-state mutation tests.

**Acceptance criteria**
- No player action is feedback-only.
- Faction/world state changes are persisted.
- Route can be automatically withheld when feature disabled.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 15 — Subterranean Debt Ledger console is unwired

**Severity:** High
**Primary files/surfaces:** `src/UI/SubterraneanDebtLedgerPanel.cs; Assets/Ashfall.Core/*Debt*; src/Main.PlayerSurfaces.cs`

**Objective:** Bind to the canonical economy/debt model.

**Implementation steps**
1. Unify debt template/catalog ownership.
2. Expose balances, due dates, collateral, standing, and enforcement from one host session.
3. Wire repayment/renegotiation/default commands.
4. Add save and enforcement integration tests.

**Acceptance criteria**
- Ledger values derive from campaign state.
- Repayment changes economy state.
- Default can trigger its intended consequences.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 16 — Surface Shrapnel Aegis console is unwired

**Severity:** High
**Primary files/surfaces:** `src/UI/SurfaceShrapnelAegisPanel.cs; src/Main.PlayerSurfaces.cs`

**Objective:** Connect fortification state to real shelter defence.

**Implementation steps**
1. Define construction level, condition, material cost, coverage, and repair.
2. Connect defence modifiers to attack/hazard resolution.
3. Wire maintenance to inventory.
4. Add damage/repair round-trip tests.

**Acceptance criteria**
- Fortification changes actual damage outcomes.
- Materials are consumed exactly once.
- Condition persists.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 17 — Long Walk Expedition console is an unbound duplicate surface

**Severity:** High
**Primary files/surfaces:** `src/UI/LongWalkExpeditionPanel.cs; src/Main.PlayerSurfaces.cs; existing expedition host/session`

**Objective:** Either bind it to the canonical expedition system or remove the duplicate route.

**Implementation steps**
1. Map every displayed field to existing expedition state.
2. Reuse expedition commands for dispatch/cancel/return.
3. Avoid new travel authority.
4. Add parity tests against the primary ExpeditionPanel.

**Acceptance criteria**
- Both expedition surfaces show the same authoritative expedition.
- Commands have identical consequences.
- No shadow expedition state.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 18 — Sonic Rupture Drill console is unwired

**Severity:** High
**Primary files/surfaces:** `src/UI/SonicRuptureDrillPanel.cs; src/Main.PlayerSurfaces.cs`

**Objective:** Bind drilling to excavation, inventory, power, wear, and hazards.

**Implementation steps**
1. Define job lifecycle and output sink.
2. Use canonical power and equipment condition.
3. Model failure/hazard events.
4. Add deterministic yield and interruption tests.

**Acceptance criteria**
- Outages/faults stop production.
- Output reaches inventory.
- No local pseudo-progress.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 19 — Vault Door Breaching console is unwired

**Severity:** High
**Primary files/surfaces:** `src/UI/VaultDoorBreachingPanel.cs; src/Main.PlayerSurfaces.cs`

**Objective:** Connect breach progress to tools, hazards, and world state.

**Implementation steps**
1. Define canonical target/door state.
2. Consume tool durability/resources.
3. Route success/failure to location/world progression.
4. Add irreversible-state protection tests.

**Acceptance criteria**
- A breached door stays breached after reload.
- Tool/resource costs are authoritative.
- Panel cannot breach a non-existent target.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 20 — Iron Cenotaph Memorial console is unwired

**Severity:** High
**Primary files/surfaces:** `src/UI/IronCenotaphMemorialPanel.cs; death/journal/memorial systems`

**Objective:** Bind memorial presentation to actual deaths and memory state.

**Implementation steps**
1. Use canonical survivor death records.
2. Integrate eulogy/heirloom/memorial data where retained.
3. Persist memorial entries, not UI-local text.
4. Remove route if feature remains purely aspirational.

**Acceptance criteria**
- Every memorial entry corresponds to a real campaign event.
- State survives reload.
- No fabricated casualty rows.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 21 — Aquifer Treaty Concession console is unwired

**Severity:** High
**Primary files/surfaces:** `src/UI/AquiferTreatyConcessionPanel.cs; treaty/faction/water systems`

**Objective:** Bind to real treaty, standing, and water consequences.

**Implementation steps**
1. Map concessions to canonical treaty state.
2. Apply water/resource effects through real authorities.
3. Add faction-reaction consequences.
4. Test accept/reject/default branches.

**Acceptance criteria**
- Treaty choice changes at least two authoritative systems.
- Effects persist.
- No local treaty ledger.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 22 — Crossing Safe-Conduct Vouch console is unwired

**Severity:** High
**Primary files/surfaces:** `src/UI/CrossingSafeConductVouchPanel.cs; crossing/vouch systems`

**Objective:** Use the actual Crossing/Vouch authority.

**Implementation steps**
1. Bind route to current vouch state.
2. Wire issue/revoke/use commands.
3. Consume standing/resources where designed.
4. Add crossing-entry integration test.

**Acceptance criteria**
- A valid vouch changes traversal outcome.
- Revocation is reflected immediately.
- No duplicate vouch state.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 23 — Mechanical Prosthetics Lathe console is unwired

**Severity:** High
**Primary files/surfaces:** `src/UI/MechanicalProstheticsLathePanel.cs; medical/crafting/inventory systems`

**Objective:** Wire prosthetic fabrication to medical eligibility and inventory.

**Implementation steps**
1. Define recipe authority.
2. Require a valid patient/need before installation.
3. Consume materials and power.
4. Persist fabricated/installed prosthetics.

**Acceptance criteria**
- Cannot install without a valid recipient.
- Materials balance.
- Installed prosthetic affects canonical survivor state.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 24 — Fungal Protein Fermenter console is unwired

**Severity:** High
**Primary files/surfaces:** `src/UI/FungalProteinFermenterPanel.cs; food/production/power systems`

**Objective:** Make fermentation a real production pipeline.

**Implementation steps**
1. Define batch recipe and contamination/spoilage risk.
2. Connect power/temperature if intended.
3. Deliver food to canonical inventory/kitchen.
4. Add spoilage/production tests.

**Acceptance criteria**
- Finished batch becomes usable food.
- Failure produces real loss/risk.
- No UI-only yield.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 25 — Ultrasonic Decontamination Airlock console is unwired

**Severity:** High
**Primary files/surfaces:** `src/UI/UltrasonicDecontaminationAirlockPanel.cs; contamination/inventory/power systems`

**Objective:** Connect decontamination to actual equipment/person state.

**Implementation steps**
1. Select a canonical target.
2. Apply contamination reduction through the owning system.
3. Consume power/filters/resources.
4. Add before/after contamination assertions.

**Acceptance criteria**
- Decontamination changes canonical contamination.
- Resource cost is enforced.
- Target identity is explicit.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 26 — Tropospheric Radio Relay console is unwired

**Severity:** High
**Primary files/surfaces:** `src/UI/TroposphericRadioRelayPanel.cs; radio/weather/intelligence systems`

**Objective:** Reuse existing radio and weather intelligence authorities.

**Implementation steps**
1. Bind relay health and signal state to radio host.
2. Make relay actions affect range/reliability/intelligence reach.
3. Consume power/maintenance resources.
4. Add radio reach integration tests.

**Acceptance criteria**
- Relay state changes real radio outcomes.
- No parallel radio model.
- Panel remains read-only if no command contract is approved.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 27 — Induction Cupola Furnace console is unwired

**Severity:** High
**Primary files/surfaces:** `src/UI/InductionCupolaFurnacePanel.cs; production/inventory/power systems`

**Objective:** Connect to production rails.

**Implementation steps**
1. Define recipes and batch lifecycle.
2. Use shared power and inventory.
3. Model wear/faults.
4. Add mass-balance and outage tests.

**Acceptance criteria**
- Outputs are canonical.
- Power loss stops processing.
- Inputs cannot be duplicated.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 28 — Heavy Marine Diesel Generator console is unwired

**Severity:** High
**Primary files/surfaces:** `src/UI/HeavyMarineDieselGeneratorPanel.cs; PowerGridSystem; fuel inventory`

**Objective:** Bind generator output, fuel use, condition, and faults to the power grid.

**Implementation steps**
1. Create/identify generator domain state.
2. Consume canonical fuel.
3. Feed output into grid supply.
4. Add fuel-to-energy and failure tests.

**Acceptance criteria**
- Fuel decreases while running.
- Grid supply changes.
- Generator cannot run at zero fuel.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 29 — Slurry Dewatering Sump console is unwired

**Severity:** High
**Primary files/surfaces:** `src/UI/SlurryDewateringSumpPanel.cs; SumpFloodingSystem; PowerGridSystem`

**Objective:** Reuse sump/flooding state instead of a separate UI shell.

**Implementation steps**
1. Bind to real flooding nodes.
2. Wire pump commands to shared power.
3. Connect output/waste if relevant.
4. Add outage/flood progression tests.

**Acceptance criteria**
- Panel reflects canonical water level.
- Power state affects pumping.
- No duplicate sump state.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 30 — Magnetic Drum Archive console is unwired

**Severity:** High
**Primary files/surfaces:** `src/UI/MagneticDrumArchivePanel.cs; archive/research systems`

**Objective:** Bind archive interactions to canonical knowledge/research state.

**Implementation steps**
1. Define read/write/archive commands.
2. Use existing archive/research catalogs.
3. Persist discovered/decoded information.
4. Remove route if no live mechanic is approved.

**Acceptance criteria**
- Archive actions alter canonical knowledge state.
- Reload preserves discoveries.
- No fake records.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 31 — Moral choices cannot be resolved through the player UI

**Severity:** Critical
**Primary files/surfaces:** `src/Main.MoralChoice.cs; src/UI/FactionsPanel.cs; moral-choice catalogs`

**Objective:** Add a real moral-choice decision surface that calls TryResolveMoralChoice.

**Implementation steps**
1. Create a modal/panel for unresolved choices.
2. List authored options without exposing hidden morality score.
3. Call the canonical resolver once on confirmation.
4. Surface branch lockouts/faction/journal consequences.
5. Add a seeded player-journey test.

**Acceptance criteria**
- At least one authored choice can be completed from UI.
- Resolution persists and cannot double-fire.
- Journal/consequence state updates from the same action.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.
- Add a regression gate or seeded journey test so the defect cannot silently return.

---

### Task 32 — Epilogue is bound to hard-coded outcome flags

**Severity:** Critical
**Primary files/surfaces:** `src/Main.GameFlow.cs; epilogue system/panel; campaign save authorities`

**Objective:** Replace literal epilogue arguments with a derived evaluation context.

**Implementation steps**
1. Define EpilogueEvaluationContext as a read model.
2. Derive every field from canonical campaign state.
3. Remove literal `0, true, true, true, true, true` bindings.
4. Create distinct seeded outcome tests.
5. Verify save/load does not change ending classification.

**Acceptance criteria**
- At least three materially different playthrough states produce different endings.
- Every epilogue flag cites a campaign source.
- No literal success booleans remain in production binding.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.
- Add a regression gate or seeded journey test so the defect cannot silently return.

---

### Task 33 — Persisted onboarding hint overlay lacks a canonical reopen route

**Severity:** High
**Primary files/surfaces:** `src/UI/OnboardingHintPanel.cs; src/Main.Onboarding.cs; src/Main.PlayerSurfaces.cs`

**Objective:** Make guidance intentionally reachable after startup.

**Implementation steps**
1. Decide whether `help` opens tutorial, persisted guidance, or a unified help surface.
2. Add a route/hotkey/affordance to reopen the persisted overlay.
3. Preserve assistance level and checklist state.
4. Add open-close-reopen test.

**Acceptance criteria**
- Player can reopen guidance at any time.
- No duplicate help systems compete.
- Visibility state behaves correctly after panel close/reset.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 34 — WeatherHistoryPanel leaks OnStateChanged subscriptions on rebind

**Severity:** High
**Primary files/surfaces:** `src/UI/WeatherHistoryPanel.cs`

**Objective:** Use stable delegate identity.

**Implementation steps**
1. Add a named handler or cached Action.
2. Unsubscribe before session replacement.
3. Optionally detach in `_ExitTree`.
4. Add repeated-bind regression test.

**Acceptance criteria**
- One state event produces one refresh after N rebinds.
- Old session no longer reaches the panel.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 35 — GeigerCalibrationPanel leaks subscriptions on rebind

**Severity:** High
**Primary files/surfaces:** `src/UI/GeigerCalibrationPanel.cs`

**Objective:** Use a stable OnStateChanged delegate.

**Implementation steps**
1. Replace fresh lambdas in subscribe/unsubscribe.
2. Detach from previous dose host.
3. Add two-host swap test.

**Acceptance criteria**
- No duplicated refreshes.
- Old calibration host has zero live panel handlers after rebind.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 36 — FireIncidentPanel leaks subscriptions on rebind

**Severity:** High
**Primary files/surfaces:** `src/UI/FireIncidentPanel.cs`

**Objective:** Correct event lifecycle.

**Implementation steps**
1. Replace anonymous lambda pair with stable handler.
2. Detach during host swap and panel destruction.
3. Test repeated open/rebind.

**Acceptance criteria**
- One event equals one refresh.
- No stale fire session retains panel reference.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 37 — TriangulationPanel leaks OnStateChanged subscriptions

**Severity:** High
**Primary files/surfaces:** `src/UI/TriangulationPanel.cs`

**Objective:** Store and remove the exact delegate.

**Implementation steps**
1. Use named/cached handler.
2. Detach before assigning new RadioHostSession.
3. Add repeated bind regression.

**Acceptance criteria**
- No duplicate triangulation refresh.
- Old host no longer affects current panel.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 38 — TriangulationPanel never removes OnLocationRevealed subscription

**Severity:** High
**Primary files/surfaces:** `src/UI/TriangulationPanel.cs`

**Objective:** Detach all event surfaces on rebind.

**Implementation steps**
1. Store the location-revealed handler.
2. Unsubscribe it from the old triangulation system.
3. Detach in `_ExitTree` if needed.
4. Test session swap.

**Acceptance criteria**
- Old session cannot emit discoveries into a panel bound to another session.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 39 — Fire Incident route constructs a throwaway ShelterFireHazardSystem and fixture incident id

**Severity:** Critical
**Primary files/surfaces:** `src/Main.PlayerSurfaces.cs; src/UI/FireIncidentPanel.cs; shelter fire host/domain`

**Objective:** Bind the panel to the campaign-owned fire authority and actual active incident.

**Implementation steps**
1. Move fire-system ownership into composition root/host session.
2. Resolve active incident by canonical ID.
3. Remove `new ShelterFireHazardSystem()` from route code.
4. Remove `inc_default` from production route.
5. Add panel-to-live-incident integration test.

**Acceptance criteria**
- Panel observes the same incident that simulation ticks.
- Actions mutate that incident.
- No fresh domain system created from UI routing.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.
- Add a regression gate or seeded journey test so the defect cannot silently return.

---

### Task 40 — Faction Matrix route constructs a throwaway FactionStanceEngine

**Severity:** High
**Primary files/surfaces:** `src/Main.PlayerSurfaces.cs; faction/economy composition`

**Objective:** Inject the shared faction stance authority.

**Implementation steps**
1. Find canonical campaign instance.
2. Bind route to it.
3. Remove `new FactionStanceEngine()` from UI route.
4. Add mutation-reflection test.

**Acceptance criteria**
- Changes elsewhere appear in matrix immediately.
- Matrix actions, if any, affect shared state.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 41 — Factions Narrative route creates a second throwaway FactionStanceEngine

**Severity:** High
**Primary files/surfaces:** `src/Main.PlayerSurfaces.cs; factions narrative panel`

**Objective:** Use the same canonical faction authority as every other faction surface.

**Implementation steps**
1. Replace construction with injected shared instance.
2. Verify both faction panels agree on stance.
3. Add cross-panel consistency test.

**Acceptance criteria**
- One faction stance authority exists at runtime.
- Both panels display identical shared values.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 42 — Radiation ZoneRadLevel is hard-coded by survivor identity

**Severity:** Critical
**Primary files/surfaces:** `src/Host/SurvivorsHostSession.cs; WeatherSystem; location/fallout/shelter systems`

**Objective:** Derive dose from environment, not identity.

**Implementation steps**
1. Define ExposureEnvironment resolver using position/location, weather, fallout/zone contamination, shelter shielding and activity state.
2. Remove Mikhail-specific branch.
3. Feed the same environmental exposure into all survivors.
4. Record exposure reason/source for UI/debug trace.
5. Add seeded location/weather exposure matrix tests.

**Acceptance criteria**
- Same survivor receives different dose in different environments.
- Different survivors in same environment receive same base zone rate before gear/traits.
- No survivor ID literal controls radiation.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.
- Add a regression gate or seeded journey test so the defect cannot silently return.

---

### Task 43 — Projected worn gear has DegradeRate = 0

**Severity:** Critical
**Primary files/surfaces:** `Assets/Ashfall.Core/Inventory/Inventory.cs; item definitions`

**Objective:** Make protective equipment degradable.

**Implementation steps**
1. Author/derive degradation rate from item condition data.
2. Populate WornGear with non-zero rate where appropriate.
3. Clamp and validate rates.
4. Add item-level degradation tests.

**Acceptance criteria**
- Protective gear loses condition under exposure.
- Different gear can degrade at different rates.
- Zero-rate items are intentional and documented.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.
- Add a regression gate or seeded journey test so the defect cannot silently return.

---

### Task 44 — Radiation gear degradation mutates throwaway WornGear copies

**Severity:** Critical
**Primary files/surfaces:** `src/Host/SurvivorsHostSession.cs; Assets/Ashfall.Core/Inventory/Inventory.cs; RadiationSystem`

**Objective:** Write degradation back to canonical EquippedItem state.

**Implementation steps**
1. Define an equipment-condition sink/port.
2. Have radiation calculation report wear deltas rather than mutating a read projection, or move degradation into Inventory.
3. Apply deltas exactly once.
4. Persist resulting durability.
5. Add 100-hour exposure round-trip test.

**Acceptance criteria**
- EquippedItem.CurrentDurability decreases.
- Reload preserves degraded value.
- No duplicate wear application per tick.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.
- Add a regression gate or seeded journey test so the defect cannot silently return.

---

### Task 45 — Inventory consumption removes items while all gameplay-effect callbacks are null

**Severity:** Critical
**Primary files/surfaces:** `src/Host/InventoryHostSession.cs; Assets/Ashfall.Core/Inventory/Inventory.cs; NeedsSystem; RadiationSystem`

**Objective:** Create one canonical consumption service.

**Implementation steps**
1. Wire applyNeed, applyRadCleanse, applyIodine, and applyContamination.
2. Resolve the target survivor explicitly.
3. Return structured effects for UI feedback.
4. Route all consumable UIs through this service.
5. Add food/water/iodine/anti-rad/contaminated-item tests.

**Acceptance criteria**
- Consuming an item changes exactly the authored effects.
- Item removal and effects are atomic.
- Failed effects do not silently delete items.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.
- Add a regression gate or seeded journey test so the defect cannot silently return.

---

### Task 46 — Holdfast consumption removes food/water from the trade inventory

**Severity:** Critical
**Primary files/surfaces:** `src/Host/HoldfastRuntimeSession.cs; InventoryHostSession; trade inventory`

**Objective:** Eliminate the second pantry authority.

**Implementation steps**
1. Decide canonical player/crew inventory.
2. Replace Trade.GetHeld/Trade.Inventory.RemoveItem with canonical inventory access.
3. Add migration for any saved trade-stock semantics if required.
4. Update UI counts to use same authority.

**Acceptance criteria**
- All food/water surfaces show and consume the same quantities.
- No item can be duplicated/lost by cross-ledger drift.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.
- Add a regression gate or seeded journey test so the defect cannot silently return.

---

### Task 47 — Holdfast consumption applies fixed -30 hunger / -35 thirst

**Severity:** High
**Primary files/surfaces:** `src/Host/HoldfastRuntimeSession.cs; item data`

**Objective:** Use authored item effects.

**Implementation steps**
1. Delete fixed restoration constants.
2. Delegate to canonical Consume service.
3. Preserve survivor target.
4. Expose exact effect summary to UI.

**Acceptance criteria**
- Two foods with different authored values produce different need changes.
- Contaminated/medical effects are not bypassed.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 48 — KitchenNutrition ServeMeal pipeline exists but gameplay does not reach it

**Severity:** High
**Primary files/surfaces:** `Assets/Ashfall.Core/KitchenNutritionSystem.cs; src/Host/KitchenNutritionHostSession.cs; kitchen UI`

**Objective:** Expose and wire meal serving as the intended cooked-food path.

**Implementation steps**
1. Bind kitchen UI/player command to ServeMeal.
2. Resolve serving survivor or crew allocation.
3. Integrate pantry/inventory ownership.
4. Connect cellar/refrigeration setters to real shelter systems if retained.
5. Add meal→needs→inventory end-to-end test.

**Acceptance criteria**
- A prepared meal can be served in play.
- Needs change according to recipe.
- Pantry portion count decreases exactly once.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 49 — WildlifeTrapping SetHunterSkill has no live progression feed

**Severity:** High
**Primary files/surfaces:** `Assets/Ashfall.Core/WildlifeTrappingSystem.cs; skill progression; duty roster`

**Objective:** Feed real assigned-hunter skill into trapping.

**Implementation steps**
1. Resolve hunter assignment from canonical roster.
2. Read canonical skill/proficiency.
3. Call/update trapping skill when assignment or progression changes.
4. Remove permanent fallback-floor behavior for staffed traps.
5. Add low/high-skill yield comparison tests.

**Acceptance criteria**
- Hunter progression measurably changes trapping outcomes.
- Unstaffed fallback is explicit and documented.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 50 — Trapping catch state never becomes canonical goods

**Severity:** Critical
**Primary files/surfaces:** `Assets/Ashfall.Core/WildlifeTrappingSystem.cs; wildlife host; InventoryHostSession`

**Objective:** Deliver catches through a capacity-aware output sink.

**Implementation steps**
1. Map species/carcass/hide outputs to canonical item IDs.
2. On butchery completion, attempt delivery once.
3. Handle toxic/contaminated outputs explicitly.
4. Handle full inventory with a recoverable pending-output state.
5. Add mass-conservation and save/load tests.

**Acceptance criteria**
- Successful catch produces real inventory goods.
- Full storage never silently deletes output.
- Reload cannot duplicate a pending catch.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.
- Add a regression gate or seeded journey test so the defect cannot silently return.

---

### Task 51 — Water exists as litres in WaterTreatmentSystem and as clean_water inventory items

**Severity:** Critical
**Primary files/surfaces:** `Assets/Ashfall.Core/WaterTreatmentSystem.cs; src/Host/WaterTreatmentHostSession.cs; InventoryHostSession`

**Objective:** Create a single water authority with an explicit packaging boundary.

**Implementation steps**
1. Write ADR deciding bulk-water authority.
2. Make InventoryHost dependency non-null where conversion is required.
3. Implement bottle/fill/draw conversion.
4. Ensure thirst consumption uses one path.
5. Add water mass-balance tests.

**Acceptance criteria**
- One litre cannot exist simultaneously in both authorities without explicit packaging.
- All water-consuming systems use the approved boundary.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.
- Add a regression gate or seeded journey test so the defect cannot silently return.

---

### Task 52 — WaterTreatmentSystem.ConsumeRation is implemented and tested but has no production caller

**Severity:** High
**Primary files/surfaces:** `Assets/Ashfall.Core/WaterTreatmentSystem.cs; day/ration owner`

**Objective:** Either wire it into crew consumption or delete/merge it.

**Implementation steps**
1. Determine whether the water plant owns ration draw.
2. If yes, call from day/ration owner and route exposure consequences.
3. If no, move clean→dirty fallback semantics into canonical consume service and remove dead method.
4. Add liveness gate.

**Acceptance criteria**
- No tested production method remains with zero runtime path without explicit disposition.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 53 — Duty roster ignores health, needs, afflictions, quarantine, and fatigue

**Severity:** Critical
**Primary files/surfaces:** `Assets/Ashfall.Core/DutyRoster/DutyRosterAssignmentEngine.cs; survivor health/needs/medical systems`

**Objective:** Introduce a canonical work-fitness verdict.

**Implementation steps**
1. Define WorkFitnessResult with blocking/warning reasons.
2. Consult hunger/thirst/fatigue, radiation, injury, sickness, quarantine and missing/dead state.
3. Apply to manual and auto-assignment.
4. Expose reasons in roster UI.
5. Add deterministic assignment tests.

**Acceptance criteria**
- Unfit survivors cannot be assigned to prohibited work.
- Auto-assign respects the same policy.
- Reasons are player-visible.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.
- Add a regression gate or seeded journey test so the defect cannot silently return.

---

### Task 54 — Trade-specialty progression is still missing production craft attribution

**Severity:** High
**Primary files/surfaces:** `src/Host/Phase0HostSession.cs; crafting system; duty/workbench assignment; trade specialties`

**Objective:** Carry survivor identity through craft completion.

**Implementation steps**
1. Add crafter/worker ID to canonical craft command/event.
2. Resolve profession/skill from canonical survivor.
3. Call TradeSpecialtySystem on real production completion.
4. Remove debug hard-coded survivor dependence.
5. Add craft→mastery integration test.

**Acceptance criteria**
- Real crafting can advance the correct survivor's specialty.
- No debug-only identity is required.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 55 — RadioStationCatalog hard-codes six station definitions despite JSON authority

**Severity:** High
**Primary files/surfaces:** `Assets/Ashfall.Core/Radio/RadioStationCatalog.cs; Assets/StreamingAssets/Data/radio*.json; RadioHostSession`

**Objective:** Restore single data authority.

**Implementation steps**
1. Create/confirm JSON schema for station definitions.
2. Load definitions through a catalog loader.
3. Keep only stable ID constants in code.
4. Delete RegisterDefaults production path.
5. Add parity/missing-catalog tests.

**Acceptance criteria**
- Station frequency/name/persona come from JSON.
- No hidden code defaults can diverge from authored data.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 56 — AtmosphereCatalogLoader + AtmosphereTextSystem are dead/unwired

**Severity:** Medium
**Primary files/surfaces:** `Assets/Ashfall.Core/*Atmosphere*; environmental_atmosphere_expansion.json`

**Objective:** Either surface the content or retire it.

**Implementation steps**
1. Choose consumer: expedition briefing, journal, location detail, or codex.
2. Wire loader in production.
3. Bind to actual location context.
4. Add reachability test.

**Acceptance criteria**
- Every retained loader has a production caller or explicit dormant disposition.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 57 — EnvironmentalTextCatalogLoader + EnvironmentalTextSystem are dead/unwired

**Severity:** Medium
**Primary files/surfaces:** `Assets/Ashfall.Core/*EnvironmentalText*; environmental_texts_expansion_05.json`

**Objective:** Integrate environmental text into exploration presentation or remove the subsystem.

**Implementation steps**
1. Choose one canonical presentation venue.
2. Load once at startup/session setup.
3. Resolve text by real location/event.
4. Add content-reachability test.

**Acceptance criteria**
- Authored text is reachable from play or removed.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 58 — DebtTemplateCatalogLoader + DebtTemplateCatalog are dead/unwired

**Severity:** Medium
**Primary files/surfaces:** `Assets/Ashfall.Core/*DebtTemplate*; ledger_debt_templates.json`

**Objective:** Wire templates into the debt/economy system.

**Implementation steps**
1. Load templates in economy setup.
2. Validate IDs against factions/items/currencies.
3. Use them to instantiate real obligations.
4. Add debt-template→obligation integration tests.

**Acceptance criteria**
- Templates can produce a live debt record.
- Dead/unreachable templates fail a liveness gate.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 59 — ProceduralEulogyEngine is dead production code

**Severity:** High
**Primary files/surfaces:** `Assets/Ashfall.Core/Journal/ProceduralEulogyEngine.cs; survivor-death pipeline; journal/memorial systems`

**Objective:** Wire eulogy generation to canonical death handling or remove it.

**Implementation steps**
1. Instantiate from composition.
2. Feed lifetime/death context.
3. Generate exactly once per canonical death.
4. Persist/archive output and surface it in journal/memorial.
5. Add death→eulogy→reload test.

**Acceptance criteria**
- Every eligible death can produce one stable eulogy.
- Reload does not regenerate a different text.
- Dead engine no longer has zero production references.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

### Task 60 — ProceduralItemInstance suppresses CS8618 and permits invalid parameterless objects

**Severity:** Medium
**Primary files/surfaces:** `Assets/Ashfall.Core/Inventory/ProceduralItemInstance.cs`

**Objective:** Remove blanket nullability suppression and enforce valid construction/deserialization.

**Implementation steps**
1. Remove `#pragma warning disable CS8618`.
2. Initialize InstanceId/ItemId or use required/nullable fields consistent with serializer needs.
3. Validate restored instances.
4. Add invalid-deserialization test.
5. Keep zero-warning build gate.

**Acceptance criteria**
- No blanket CS8618 suppression in this file.
- Default/deserialized object cannot enter inventory with missing identity.

**Required validation**
- Add/extend focused unit coverage.
- Add at least one production-path integration or liveness test.
- Re-run repository-wide search to prove the superseded dead/duplicate path is gone.

---

## 6. Cross-Task Architecture Work Packages

### Package 6A — Panel Liveness Manifest

Create a generated or hand-maintained machine-readable manifest for every player-routable panel:

```text
panel_id
panel_class
classification = LIVE | READ_ONLY_INTENTIONAL | DEV_ONLY | QUARANTINED
authority
host_session
bind_method
primary_command
mutation_target
save_section
integration_test
```

CI must fail when:
- a `LIVE` panel has no authority,
- a `LIVE` panel has no bind/read model,
- a `LIVE` interactive panel has no state-mutating command,
- a `QUARANTINED` panel is routed to the player,
- a registered route is absent from the manifest.

This package closes the systemic root cause behind Tasks 1–30.

### Package 6B — Port/Seam Contract Gate

Extend the existing loader-wiring idea to public integration-shaped APIs. Candidate verbs:

`Bind*`, `Set*`, `Wire*`, `Register*`, `Apply*`, `Enable*`, `Configure*`, `Serve*`, `Consume*`, `Resolve*`, `Deliver*`.

Each seam must be one of:
- `LIVE_HOST_CALLER`
- `LIVE_VIA_CORE`
- `TEST_ONLY_INTENTIONAL`
- `EXTENSION_DORMANT`
- `DEPRECATED`

An unclassified public seam fails CI.

### Package 6C — One Production Inventory

Define canonical inventory ownership and adapters for:
- personal/crew inventory,
- trade stock,
- bulk water,
- kitchen pantry,
- trapping output,
- foundry/manufacturing output.

No system may silently maintain a second fungible stock for the same resource.

### Package 6D — Campaign Event Attribution

Every state-changing action should be able to answer:
- who did it,
- what command caused it,
- what authority changed,
- what resource moved,
- what consequence was emitted.

This enables correct journal/day-summary feedback and makes integration tests much easier to write.

---

## 7. Mandatory Regression Suite

Create or extend tests covering these journeys.

### Journey J1 — Moral Choice
1. Load one unresolved authored choice.
2. Open decision UI.
3. Resolve an option.
4. Verify moral state changes.
5. Verify journal consequence.
6. Save.
7. Reload.
8. Verify it cannot resolve twice.

### Journey J2 — Radiation Exposure
1. Same survivor in shelter.
2. Same survivor in contaminated exterior.
3. Change weather.
4. Equip protection.
5. Advance 100 exposure hours.
6. Verify dose differs by environment.
7. Verify gear durability decreases.
8. Save/reload and verify exact durability.

### Journey J3 — Consumption
1. Consume food.
2. Consume water.
3. Consume iodine.
4. Consume anti-rad medication.
5. Consume contaminated food.
6. Verify exact authored effects and one inventory decrement each.
7. Repeat from Holdfast route and prove identical authority/effects.

### Journey J4 — Work Fitness
1. Healthy survivor can be assigned.
2. Starving survivor is blocked/warned according to policy.
3. Quarantined survivor is blocked.
4. Auto-assign obeys identical rules.
5. Recovery makes survivor eligible again.

### Journey J5 — Trapping
1. Assign novice hunter.
2. Run seeded trap.
3. Assign skilled hunter.
4. Run same seed with skill difference.
5. Complete butchery.
6. Verify canonical food/hide delivery.
7. Fill inventory and verify pending output is not lost.
8. Save/reload pending output and claim exactly once.

### Journey J6 — Epilogue
Create at least three campaign snapshots:
- survivalist/authoritarian path,
- humanitarian/cooperative path,
- catastrophic/failed path.

Each must derive a distinct context and ending without literal production booleans.

### Journey J7 — Panel Rebind
For Weather History, Geiger Calibration, Fire Incident, and Triangulation:
1. Bind Session A.
2. Bind Session B.
3. Fire event on A → panel must not refresh.
4. Fire event on B → exactly one refresh.
5. Repeat bind 10 times → still one refresh/event.

---

## 8. CI / Headless Gate Additions

Recommended new or extended gates:

1. `panel-liveness-gate`
2. `no-ui-throwaway-authority-gate`
3. `integration-seam-classification-gate`
4. `production-loader-reachability-gate`
5. `consumption-authority-gate`
6. `production-output-sink-gate`
7. `event-subscription-identity-gate`
8. `epilogue-derived-state-gate`
9. `radiation-environment-contract-gate`
10. `work-fitness-contract-gate`
11. `zero-new-warning-suppression-gate`
12. `seeded-core-journey-gate`

Do not create gates that merely count files or methods. Each gate should prove **liveness, authority, or causal effect**.

---

## 9. Recommended Implementation Batches

### Batch 1 — Stop the game from lying
Tasks: 31, 32, 39, 42, 45, 46, 53

Outcome:
- choices work,
- ending remembers the campaign,
- fire UI shows the real fire,
- radiation comes from the world,
- consuming items changes the survivor,
- there is one pantry,
- unfit people cannot be assigned as if healthy.

### Batch 2 — Make scarcity persistent
Tasks: 43, 44, 47, 48, 50, 51, 52

Outcome:
- protective gear wears,
- cooked food matters,
- catches become goods,
- water has one accounting model.

### Batch 3 — Close progression seams
Tasks: 49, 54, 55, 59

Outcome:
- skills affect work,
- crafting advances the real worker,
- radio content has one authority,
- deaths trigger the authored memory system.

### Batch 4 — Repair UI lifecycle
Tasks: 33–38, 40–41

Outcome:
- guidance is reachable,
- panels do not leak handlers,
- faction/fire panels stop reading fabricated state.

### Batch 5 — Console triage
Tasks: 1–30

For each console choose exactly one:
- **WIRE NOW**
- **READ-ONLY INTENTIONAL**
- **DEV-ONLY**
- **QUARANTINE**
- **DELETE**

Do not leave any in an ambiguous “looks live” state.

### Batch 6 — Dead-code disposition
Tasks: 56–58, 60

Outcome:
- dormant loaders are intentional,
- invalid nullable object states are eliminated,
- warning suppression is reduced.

---

## 10. Console Triage Decision Matrix

Use this matrix for Tasks 1–30.

| Question | If YES | If NO |
|---|---|---|
| Does a canonical Core authority already exist? | Reuse it | Decide whether feature is worth creating |
| Does a host session/read model exist? | Bind it | Create thin host adapter only |
| Does a real player command exist? | Wire button | Add typed command or make screen read-only |
| Does the command mutate canonical state? | Continue | Reject fake action |
| Is mutation saved? | Continue | Add capture/restore |
| Does another system consume the outcome? | Shippable candidate | Close causal seam first |
| Is the feature wanted this release? | Wire | Quarantine/remove route |

---

## 11. Required Documentation Updates

At closeout update only living authority documents:

- architecture / composition map,
- player-surface registry,
- save-section registry,
- content/loader reachability map,
- UI route/liveness map,
- CI gate catalog.

For every corrected stale audit statement, mark it **RESOLVED with commit evidence** rather than silently deleting history.

---

## 12. Completion Metrics

The remediation is not complete until these metrics are achieved:

- Player-routable unbacked flagship consoles: **30 → 0**
- Moral choices resolvable from player UI: **0 → all eligible authored choices**
- Hard-coded epilogue outcome bindings: **1 production path → 0**
- Route-created long-lived gameplay authorities: **≥3 → 0**
- Known lambda unsubscribe identity defects: **4 panels / 5 paths → 0**
- Survivor radiation zone-rate literals based on identity: **2 constants → 0**
- Protective gear degradation rate forced to zero: **1 path → 0**
- Protective gear wear write-back gaps: **1 → 0**
- Consumable effect callbacks omitted in live inventory path: **4 → 0**
- Competing food inventory authorities: **2 → 1**
- Trapping catches with no inventory delivery: **1 pipeline → 0**
- Competing water authorities without explicit conversion: **2 → 1 + packaging boundary**
- Duty assignment health/medical awareness: **status-only → canonical fitness verdict**
- Craft specialty attribution debug-only: **1 debug path → real production attribution**
- Hard-coded radio station definitions: **6 → 0 production definitions in code**
- Fully dead retained loader/system pairs in this plan: **3 → 0 or explicit dormant disposition**
- ProceduralEulogyEngine production callers: **0 → ≥1**
- Blanket `CS8618` suppression in ProceduralItemInstance: **1 → 0**

---

## 13. Final Repository Sweep

Before declaring the flagship remediation complete:

1. Search for `new .*System(` and `new .*Engine(` in player-route code.
2. Search for `+= _ =>` and matching `-= _ =>` in long-lived UI.
3. Search for `DegradeRate = 0f`.
4. Search for hard-coded survivor IDs in exposure logic.
5. Search for `TryResolveMoralChoice` and prove a player call site.
6. Search epilogue bindings for literal booleans.
7. Search production consumable calls for omitted effect callbacks.
8. Search every retained `LoadAndRegister`/catalog loader for a production caller.
9. Search all 30 flagship panel IDs and verify final classification.
10. Run zero-warning build.
11. Run full xUnit suite.
12. Run data-integrity selftest.
13. Run content-utilization/liveness selftests.
14. Run panel route/liveness gate.
15. Run the seeded journey suite.
16. Boot the Godot project headlessly.
17. If an export gate exists, boot the exported artifact too.

---

## 14. Flagship Closeout Standard

The plan is complete only when an independent audit can no longer produce the sentence:

> “The system exists, is unit-tested, and never happens in play.”

The desired repository state is:

> “Every retained player-facing system has one authority, one production path, one causal effect, one persisted outcome, and one regression gate proving it stays alive.”
