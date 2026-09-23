# PLAN-VERTICAL-BODY-INDUSTRY-05 — New Mechanics Vertical: Body, Care, Industry & Trade

**Program:** ASHFALL Expansion & Integration Program (2026-09-21)
**Status:** PROPOSED — not a claim. Foreman claim required.
**Owner role on execution:** Builder per package; Integrator for medical/economy
day owners and any new save section.
**Depends on:** PLAN-ORPHAN-SEAL-01 Waves 2–6 and 8; PLAN-UNBLOCK-03 U1
(host-pending signed artifacts); PLAN-INTEGRATION-KIT-02 (gates).
**Expanded appendix:** [`PLAN-VERTICAL-BODY-INDUSTRY-05_APPENDIX-A_ORPHAN_DOSSIERS.md`](PLAN-VERTICAL-BODY-INDUSTRY-05_APPENDIX-A_ORPHAN_DOSSIERS.md)
— domain-filtered orphan dossiers for this plan's medical/table/industry/
economy/logistics systems, each mapped to its parent-plan mechanic row.
**Non-goals:** no new medical authority (`MedicalWardSystem`,
`MedicalPipelineCoordinator`, `DiseaseSystem`, `SickListSystem`, `NeedsSystem`,
`RadiationSystem` stay canonical); no new economy ledger (`MarketSystem`,
`FundsLedger`, `BlackMarketSystem`, `LedgerDebtSystem` stay canonical); no real
weapons/ammunition modeling (the Powder Metallurgy authority's own safety
comment is honored — quality bands and reliability only).

---

## 1. Outcome

Make the 40+ host-unreachable body/care/industry/economy authorities playable
as five interlocking loops, each one day-tick, one save path, one player
surface, and one bounded consequence. This is where the game gains its middle
game: the bunker stops being a list of panels and becomes a workshop, a clinic,
a foundry, a market, and a small state.

| # | Loop | Authorities (existing) | Player action | Observable outcome |
|---|---|---|---|---|
| 1 | Clinic Continuum | `ClinicalWardTriageEngine`, `RehabilitationProgressionEngine`, `ProstheticConditionWearEngine`, `SurgicalGraftRejectionEngine`, `PalliativeCareDignityEngine`, `DependencyTaperWithdrawalEngine`, `SurvivorBodyState` | triage → treat → rehabilitate → fit/maintain prosthesis → palliative/withdrawal | bed occupancy, recovery days, graft/wear failures, comfort, taper schedule |
| 2 | Table & Water | `WaterQualityProfileEngine`, `WaterSourceSystem`, `FoodTypeSystem`, `CommonTableRationingEngine`, `CookingSystem`, `ClothingWarmthSystem`, `SleepAcousticRestEngine`, `LyophilizationEngine` | draw/treat water, set ration tier, cook, preserve, layer clothing, set rest acoustics | daily needs deltas, spoilage, dysentery exposure, warmth/rest quality |
| 3 | Shelter Engine | `CupolaFoundryEngine`, `KilnFiringEngine`, `ChemicalReagentSynthesisEngine`, `MechanicalPowerDrivelineEngine`, `PowerLoadSheddingEngine`, `EmergencyMusterReadinessEngine`, `ShelterMaintenanceSystem`, `ShelterExpansionSystem`, `DisasterResponseSystem` | start a heat campaign, fire a kiln, run synthesis, schedule power, hold muster, repair/expand | produced materials, power draw, stability/condition, emergency readiness |
| 4 | Market Depth | `RestockAllocationEngine`, `TradeRouteRiskBindingEngine`, `TradeRouteMonopolyEngine`, `SeasonalHumanMigrationEngine`, `MigrationConsequenceEngine`, `BlackMarketContrabandEngine`, `BlackMarketHeatAttentionEngine`, `ChitPurityAssayEngine`, `LoanSharkEnforcerEngine`, `SurvivorBarterSystem`, `FundsLedger`, `TradeRouteContract` | allocate restock, sign a route contract, move caravans/refugees, trade contraband, assay chits, service debt | stock tiers, route reliability/tariff, heat/attention, premiums, default events |
| 5 | Polity & Distance | `ColonySystem`, `OutpostSettlementSystem`, `AerialReconWindowEngine`, `RailwayInterlockEngine`, `ModalTravelDispatchEngine`, `RadioPropagationEngine`, `NvisC4ISystem`, `CommunicationsSystem`, `TerritoryControlSystem`, `FactionDiplomacySystem`, `VehicleCustomizationSystem` | establish an outpost, run a convoy/rail slot, open a channel, contest territory, negotiate | outpost supply/starvation, travel time/risk, message range, control/suspicion |

---

## 2. Premise evidence

- All listed authorities are host-unreachable (PLAN-ORPHAN-SEAL-01 §2), with
  package-local tests already passing; the missing half is host/save/route.
- Signed dispositions that these loops complete: DEC-21/38/41/42 (body),
  DEC-22/26/37/39 (funds/restock), DEC-30 (trade routes), DEC-34 (migration),
  DEC-43 (underground pressure).
- Canonical owners and patterns already live: `MedicalWardSystem` staffing
  (`ward` role, sealed), `SurvivorBodyState` (`body_state` envelope),
  `PowerGridSystem`/`NuclearCorePowerGridPublish`, `MarketSystem` v2 shocks,
  `ShelterBarterSystem.ComputeItemPriorityScore`, `TravelingCaravanSystem`
  graph hops, `WastelandMapSystem` fog/travel, `VehicleGarageSystem`
  decoration seam, `RadioHostSession` station seam.
- Prohibited restorations: no `WaterSourceSystem`-vs-`water_sources.json`
  fork was authorized historically (DEBT-189), so `WaterSourceSystem` here is
  the *intake advisory projection*, not a new water store.
- Content volume is real: the systems have catalogs or authored seeds already
  (e.g. `colony_blueprints.json`, `trade routes`, `disease_catalog.json`,
  `shelter_construction.json`, `vehicle_modules.json`).

---

## 3. Seam and save map

| Concern | Extend |
|---|---|
| Day owners | `medical_disease`, `hygiene`, `economy_market`, `underworld_market`, `duty_roster`, plus a new `shelter_industry` owner (phase-ordered after `power_grid`) |
| Medical | `MedicalWardSystem`, `MedicalPipelineCoordinator`, `SurvivorBodyState`, `SickListSystem` |
| Needs/food | `NeedsSystem`, `FoodPreservationSystem`, `KitchenNutritionSystem`, `WaterTreatmentSystem`, `SumpFloodingSystem` |
| Power | `PowerGridSystem` + `PowerLoadSheddingEngine` as the demand advisor only (never a second grid) |
| Economy | `MarketSystem`, `FundsLedger`, `BlackMarketSystem`, `LedgerDebtSystem`, `FactionBountySystem`, `HoldfastTradeSession`, `ShelterBarterSystem` |
| World/logistics | `WastelandMapSystem`, `TravelingCaravanSystem`, `ExpeditionSystem`, `RailSystem`, `RadioHostSession`, `FactionWarSystem`, `VehicleGarageSystem` |
| Save | ride existing sections where possible: `medical`, `body_state`, `greenhouse`/`agriculture`, `power_grid`, `economy`, `black_market`, `expedition`, `caravan`, `radio`, `faction_*`, `vehicle_garage`; new sections (`shelter_industry`, `communications`) require integrator sign-off + matrix regeneration |
| RNG | streams: `industry_kiln`, `industry_synthesis`, `power_shed`, `route_risk`, `migration`, `contraband_heat`, `comms_propagation`, `territory_contest` |
| Panels | extend existing medical/economy/power/expedition panels; new `industry` and `comms` surfaces only if route coverage requires them |

---

## 4. Packages

### B5-1 — Clinic Continuum (medical)
- Bind six engines to `MedicalHostSession`/`MedicalWardHostSession`; read
  `SurvivorBodyState` as the single body model; expose one clinic panel region
  with triage severity, plan, rehab phase, prosthetic condition, taper clock.
- Acceptance: no double-charging of limb surgery (existing authority remains
  sole owner); rehab is never instant (fitting 3–5d, adaptation 10–20d,
  mastery permanent per DEC-41); wear/graft failures are deterministic and
  recoverable; palliative dignity is a bounded comfort modifier, not a cure.
- Verify: `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/`;
  `godot --headless --path . -- --medical-selftest`.

### B5-2 — Table & Water
- `WaterQualityProfileEngine` + `WaterSourceSystem` become the intake advisory
  projection over the piezometer/WT/sump owners (DEBT-189 contract);
  `FoodTypeSystem` gates preservation by food type + temperature;
  `CommonTableRationingEngine` sets a ration tier with needs-consequence;
  `CookingSystem` + `LyophilizationEngine` close the raw→cooked→preserved chain;
  `ClothingWarmthSystem` + `SleepAcousticRestEngine` tune warmth/rest quality.
- Acceptance: the starvation/quality loop is bounded and explainable; no
  parallel food or water store; dysentery exposure routes through
  `DiseaseSystem.TryExpose` (the authored `foul_water_draw` precedent).
- Verify: `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/` (sanitation,
  kitchen), `bash scripts/run_test.sh Ashfall.Core.Tests/Nutrition/`.

### B5-3 — Shelter Engine
- Foundry heat campaign (`CupolaFoundryEngine` → `SilentFoundrySystem`),
  kiln firing, reagent synthesis, driveline wear, load-shedding advisor,
  muster readiness, maintenance/expansion, disaster response.
- New player loop: the **Power Board** — per-circuit priority (life support,
  clinic, workshop, lighting, growth) resolved by `PowerLoadSheddingEngine`,
  with brownout consequences routed to existing systems (Plan 51 crisis band).
- Acceptance: power is a demand/shed decision over the canonical grid; no
  second power model; disaster protocols reduce damage only (no free repair);
  expansion obeys the existing stability < 40% alert.
- Verify: `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`,
  `godot --headless --path . -- --power-grid-selftest`.

### B5-4 — Market Depth
- `RestockAllocationEngine` replaces ad-hoc restock math (DEC-37/CF-P5).
- Trade routes: `TradeRouteContract` + `TradeRouteRiskBindingEngine` +
  `TradeRouteMonopolyEngine` produce route cards (reliability tier, tariff,
  exclusive good, 30-day cooldown) consumed by caravans; risk binding applies
  authored piracy/weather losses to the existing caravan resolution.
- Migration: `SeasonalHumanMigrationEngine` + `MigrationConsequenceEngine`
  move regional population weights and produce refugee/visitor pressure.
- Underworld: contraband/heat/chit/loan-shark bind to `BlackMarketSystem`
  (no second ledger, bounty escalation stays with `FactionBountySystem`).
- Acceptance: every price/stock change is explainable from one of the canonical
  factors; arbitrage guard (black-market floor ≥ canonical+25%) preserved;
  determinism across reload; DEC-05 restock tests stay green.
- Verify: `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/`,
  `godot --headless --path . -- --economy-selftest`.

### B5-5 — Polity & Distance
- `ColonySystem` + `OutpostSettlementSystem`: establish/garrision/supply/
  risk/overrun/abandon without duplicating population or inventory stores.
- Travel: `ModalTravelDispatchEngine`, `RailwayInterlockEngine`,
  `AerialReconWindowEngine` choose and cost travel modes over the canonical
  map graph (fog/Unknown gating preserved).
- Comms: `RadioPropagationEngine`, `NvisC4ISystem`, `CommunicationsSystem`
  become antenna/range/jam/cryptanalysis surfaces over `RadioHostSession`;
  `TerritoryControlSystem` + `FactionDiplomacySystem` add contest and
  negotiation over `FactionWarSystem`/standing owners.
- `VehicleCustomizationSystem` module installation rides the Plan 50
  `VehicleGarageSystem` seam (CF-P6 armor grades land here).
- Acceptance: no duplicate supply store; outpost starvation/risk deterministic;
  travel time/risk matches map truth; jamming/range are bounded modifiers;
 vehicle module changes persist and never bypass inventory costs.
- Verify: `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/`,
  `bash scripts/run_test.sh Ashfall.Core.Tests/World/`,
  `godot --headless --path . -- --expeditions-selftest`.

### B5-6 — Farms and Fabrics (small binders)
- `SoilReclamationProfileEngine`, `OilseedPressingEngine`,
  `PrecisionGlassworksOpticsEngine`, `GarmentLayeringThermalEngine` bind to
  greenhouse/workshop/optics surfaces already present.
- Acceptance: outputs use existing items only; no new economy goods without
  catalog rows and a consumer.

---

## 5. Content additions (initial)

| Area | Add | Rules |
|---|---:|---|
| Clinic | +8 procedure rows, +6 rehab plan rows | fictional medicine, no real drug recipes |
| Water/food | +12 preservation rows, +6 ration tiers | derived from existing food catalog |
| Industry | +10 kiln loads, +10 synthesis routes, +6 driveline parts | materials must exist in `items.json` |
| Trade | +12 route contracts, +8 monopoly conditions, +8 contraction events | authored, deterministic |
| Migration | +8 push/pull factors | no real-world countries/peoples |
| Comms | +6 channels, +8 intercept archetypes | fictional stations only |
| Outposts | reuse `outposts.json` (4 canonical) | extend only with a live consumer |

All through `CatalogIntegrityValidator` + `--content-utilization-selftest`.

---

## 6. Risk register

| Risk | Mitigation |
|---|---|
| Duplicate power/economy/medical authority | seam map above is binding; kit gate detects a second store |
| Difficulty spike from combined loops | DEC-07 baselines + difficulty scalars; balance sim evidence (`ashfall-balance-sim`) before tuning changes |
| Save-section sprawl | default ride existing; two new sections max, each with matrix + count gate |
| Determinism across long campaigns | 30-day replay tests per loop (the Plans174-177 precedent) |
| Real-world weapon/medical detail | keep quality/reliability abstraction; no propellant/ammo or drug recipes |
| Content volume outruns consumers | kit content-utilization gate; no orphan catalogs |

## 7. Focused verification (program level)

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Medical/
bash scripts/run_test.sh Ashfall.Core.Tests/Economy/
bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/
bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/
bash scripts/run_test.sh Ashfall.Core.Tests/World/
godot --headless --path . -- --medical-selftest
godot --headless --path . -- --economy-selftest
godot --headless --path . -- --power-grid-selftest
godot --headless --path . -- --data-integrity-selftest
```

## 8. Handoff notes

Each B5-N package uses the standard handoff block and cites the existing
authority it extends. The vertical is "integrated" when the kit gate lists
each authority, the day owners tick, one player surface resolves per loop, and
a 30-day seeded replay is identical across a mid-run save/reload.

---

## 6. Expanded census (64 files · 17,610 lines)

Domain: Medical, Needs, Nutrition, Kitchen, Cooking, Farming.

| Metric | Value |
|---|---:|
| Files | 64 |
| Lines | 17,610 |
| Banned refs | 2 |
| Save surfaces | 27 |
| Matched catalogs | 8 |

**Largest files:** `AgricultureSystem.cs` 905, `BionicsSystem.cs` 805, `MedicalPipelineCoordinator.cs` 747, `PharmaceuticalTabletEngine.cs` 721, `FungiCultivationSystem.cs` 654, `MicrofluidicDiagnosticEngine.cs` 609, `ChemicalDependencySystem.cs` 536, `AmputationSystem.cs` 528

## 7. Expanded surface: vertical contract

| Rule | Detail |
|---|---|
| Owners | each mechanic names an existing owner; the vertical composes, never duplicates |
| Data | catalogs resolve through loaders; consumable content is reachable |
| Determinism | seeded variance only; no wall clock |
| Save | state rides registered sections; keys per Plan 1 Appendix Q |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Medical/` |
| Consumer proof | one fixture per newly wired catalog |
| Determinism | scanned; counts above |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census (this section).
2. Wire owners; no parallel state.
3. Catalog consumer fixtures.
4. Regression: focused region + census.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Mechanic | composes owners; no duplicate authority |
| Catalog | consumer proven |
| State | registered; round-trips |
| Fixture | focused and green |

**Non-goals unchanged:** the vertical composes; it does not add authorities.

---

## 12. Cross-plan coupling

Domain method: plan-body `.cs` enumeration.
Domain files: 8. Other plans referencing them: **9**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-MEDICAL-FAMILY-TRUTH-263` | 6 |
| `PLAN-ORPHAN-SEAL-01` | 2 |
| `PLAN-WATER-AGRICULTURE-46` | 2 |
| `PLAN-BIONICS-ENHANCEMENT-78` | 2 |
| `PLAN-SCIENCE-EDUCATION-38` | 1 |
| `PLAN-PANDEMIC-PUBLIC-HEALTH-47` | 1 |
| `PLAN-THREADING-ASYNCHRONY-72` | 1 |
| `PLAN-PHARMACEUTICAL-TRUTH-167` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `B5-1` | `MedicalPipelineCoordinator.cs` |
| `B5-2` | `PharmaceuticalTabletEngine.cs` |
| `B5-3` | `MicrofluidicDiagnosticEngine.cs`, `PharmaceuticalTabletEngine.cs` |
| `B5-4` | no name match — resolve at claim time |
| `B5-5` | no name match — resolve at claim time |
| `B5-6` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 8; intra-domain edges: **2**; isolated files:
**5**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `BionicsSystem` | `AmputationSystem` |
| `BionicsSystem` | `MedicalPipelineCoordinator` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `AmputationSystem` | 1 |
| `MedicalPipelineCoordinator` | 1 |
| `AgricultureSystem` | 0 |
| `BionicsSystem` | 0 |
| `ChemicalDependencySystem` | 0 |
| `FungiCultivationSystem` | 0 |
| `MicrofluidicDiagnosticEngine` | 0 |
| `PharmaceuticalTabletEngine` | 0 |

**Class split:** hub 0 · sink 2 · source 1 · isolated 5.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 8. Host files: **23** · Test files: **35** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 23 | `src/Host/AgricultureHostSession.cs`, `src/Host/ChemicalDependencyHostSession.cs`, `src/Host/HostCli.PanelTests.cs`, `src/Host/HostCli.Plans162_165.cs`, `src/Host/MedicalHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 35 | `Ashfall.Core.Tests/AgriculturePersistenceTests.cs`, `Ashfall.Core.Tests/AgricultureSystemTests.cs`, `Ashfall.Core.Tests/ChemicalDependencyCommandTests.cs`, `Ashfall.Core.Tests/ChemicalDependencySystemTests.cs`, `Ashfall.Core.Tests/Expeditions/C2AmputationTravelTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/narrative/dweller_dependency_backstories.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **12** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `agriculture` |
| `amputation` |
| `bionics` |
| `chemical_dependency` |
| `chemical_recon` |
| `chemical_synthesis` |
| `fungi_cultivation` |
| `industry` |
| `medical` |
| `medical_pipeline` |
| `medical_ward` |
| `microfluidic_diagnostic` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **6** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--agriculture-selftest` |
| `--chemical-dependency-save-selftest` |
| `--medical-selftest` |
| `--medical-ward-save-selftest` |
| `--microfluidic-diagnostic-selftest` |
| `--microfluidic-diagnostic-uitest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **4**.

| Event | First declaration |
|---|---|
| `OnDependencyFormed` | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` |
| `OnDependencyReFormedByStress` | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` |
| `OnDependencyRisk` | `Assets/Ashfall.Core/PharmaLabSystem.cs` |
| `OnMedicalProcessingCompleted` | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/agriculture_items.json` |
| `Assets/StreamingAssets/Data/bionics.json` |
| `Assets/StreamingAssets/Data/chemical_dependency_items.json` |
| `Assets/StreamingAssets/Data/chemical_syntheses.json` |
| `Assets/StreamingAssets/Data/chemical_weapons.json` |
| `Assets/StreamingAssets/Data/medical_record_templates.json` |
| `Assets/StreamingAssets/Data/medical_texts.json` |
| `Assets/StreamingAssets/Data/microfluidic_diagnostic_catalog.json` |
| `Assets/StreamingAssets/Data/narrative/dweller_dependency_backstories.json` |
| `Assets/StreamingAssets/Data/narrative/dweller_medical_casebook.json` |
| `Assets/StreamingAssets/Data/narrative/greenhouse_cultivation_logs.json` |
| `Assets/StreamingAssets/Data/narrative/medical_documents_expansion.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **2** (50 files, 441 cases).

| Region | Files | Cases |
|---|---:|---:|
| `BodyMind` | 1 | 6 |
| `Medical` | 49 | 435 |

**Verdict:** 441 cases sit under matching regions — run those first (`BodyMind`, `Medical`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **33**
(8 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AgricultureSaveStore.cs` |
| `src/Host/AmputationSaveStore.cs` |
| `src/Host/BionicsSaveStore.cs` |
| `src/Host/ChemicalDependencyHostSession.cs` |
| `src/Host/ChemicalDependencySaveSelfTest.cs` |
| `src/Host/ChemicalDependencySaveStore.cs` |
| `src/Host/ChemicalReconHostSession.cs` |
| `src/Host/ChemicalReconSaveStore.cs` |
| `src/Host/ChemicalSynthesisHostSession.cs` |
| `src/Host/ChemicalSynthesisSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **12**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `amputation` | no |
| `bionics` | no |
| `chemical_dependency` | no |
| `chemical_recon` | no |
| `chemical_synthesis` | no |
| `fungi_cultivation` | no |
| `industry` | no |
| `medical` | no |
| `medical_pipeline` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **8**.

| Stream |
|---|
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `bionics` |
| `medical` |
| `medical_microfluidic_diagnostics` |
| `mineral_chemical` |
| `vertical_ascent` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **10**
(CODEX_ONLY 5, GAMEPLAY_CONSUMED 3, OPTIONAL 1, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `chemical_syntheses.json` | UNRESOLVED |
| `chemical_weapons.json` | GAMEPLAY_CONSUMED |
| `medical_texts.json` | OPTIONAL |
| `narrative/dweller_dependency_backstories.json` | CODEX_ONLY |
| `narrative/dweller_medical_casebook.json` | CODEX_ONLY |
| `narrative/greenhouse_cultivation_logs.json` | CODEX_ONLY |
| `narrative/medical_documents_expansion.json` | CODEX_ONLY |
| `narrative/underground_fungi_flora.json` | CODEX_ONLY |
| `toxic_chemical_catalog.json` | GAMEPLAY_CONSUMED |

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

**Readiness:** READY-WITH-NOTES (10/12) · **Class:** standard · **Coupling (incoming plans):** 9
**Surface:** save sections 12 (laddered 0) · RNG streams 8 · host files 20 · catalogs 22 · test regions 2 · flags 6

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-VERTICAL-BODY-INDUSTRY-05
wave: —
status: PROPOSED — foreman claim required
packages: author package list at claim time
claim paths:
  - src/Audio/AudioStateCoordinator.cs  # §19 candidate host surface
  - src/Host/AgricultureHostSession.cs  # §19 candidate host surface
  - src/Host/AgricultureSaveStore.cs  # §19 candidate host surface
  - src/Host/AmputationSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/agriculture_items.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/bionics.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/BodyMind/
  - godot --headless --path . -- --agriculture-selftest
dependencies:
  - coordinate: 9 other plan(s) name these artifacts (§12)
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
| packages | **no** |
| acceptance | yes |
| risks | yes |
| verification | yes |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** author or confirm: wave, packages.
