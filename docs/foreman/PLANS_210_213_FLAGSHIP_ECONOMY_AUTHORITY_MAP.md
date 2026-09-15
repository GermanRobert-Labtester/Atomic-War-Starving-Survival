# Flagship Economy Wave — Authority Map (Waste & Sanitation, Black Market, Dynamic Economy, Metallurgy)

**Status:** ACCEPTED — §4 defaults approved by foreman 2026-09-13 (D1–D7 as proposed)
**Package:** `PLANS-210-213-FLAGSHIP-FOUNDATION`
**Date:** 2026-09-13
**Supersedes:** nothing. **Precedence note:** this map governs the flagship
wave only; it does not alter any sealed family map or closeout.

---

## 1. PREMISE CORRECTION — plan-number collision (read first)

The flagship plan text numbers its four systems 170–173. Those numbers are
**already occupied** in the historical corpus and audited in
`docs/foreman/PLAN_170_199_FORENSIC_AUDIT.md`:

| Flagship system | User doc number | Historical plan with same number | Historical owner (audited) |
|---|---|---|---|
| Waste & Sanitation | "Plan 170" | 170 Seasonal events | `World/SeasonalEventSystem.cs` — CURRENT EQUIVALENT, sealed |
| Black Market & Syndicates | "Plan 171" | 171 Dynamic quests | `Quests/QuestRuntimeCoordinator.cs` — CURRENT EQUIVALENT, sealed |
| Dynamic Economy & Scarcity | "Plan 172" | 172 Radiation mutation | `Medical/MutationSystem.cs` — CURRENT EQUIVALENT, sealed |
| Advanced Metallurgy | "Plan 173" | 173 Radio production | **Sealed 2026-09-12** as `RadioProgramProductionSystem` + `radio_programs.json` (3 phases, all green) |

Per audit finding `F-170-199-NUMBER-DRIFT`: *plan numbers are not
implementation identity.* This wave therefore adopts **canonical IDs 210–213**
and treats the user-document numbers as descriptive only:

| Canonical ID | System |
|---|---|
| **Plan 210** | Waste Management & Sanitation |
| **Plan 211** | Black Market & Underworld Syndicates |
| **Plan 212** | Dynamic Local Economy & Scarcity |
| **Plan 213** | Advanced Crafting & Metallurgy Reconciliation |

All future ledgers, save sections, catalogs, tests, and closeouts for this
wave use 210–213. The repository's sealed Plan 173 (radio) is untouched.

---

## 2. CONFIRMED AUTHORITIES (file:line evidence, current branch)

### 2.1 Plan 210 — Waste & Sanitation

**Greenfield.** No `SanitationSystem`, waste ledger, hygiene meter, compost
authority, or cleaning job exists. Verified: zero matches for
`SanitationSystem|waste_accumulation|hygiene_permille` across
`Assets/Ashfall.Core/**` and `Assets/StreamingAssets/Data/**` (only
coincidental `process_chlor_alkali_sanitation_*` batch names in
`chlor_alkali_synthesis_catalog.json` and prose in narrative JSON).

Existing authorities Plan 210 must consume (never duplicate):

| Concern | Owner | Evidence |
|---|---|---|
| Shelter rooms/sectors | `Shelter/ShelterRoomCatalog.cs` (30.5 KB) | room definitions + `room_bp_*` ids |
| Zone hazards (spill template) | `Shelter/ShelterFireHazardSystem.cs` | `FireZoneState`/`FireIncidentState`; zone propagation, brigade, dampers |
| Flood/liquid incidents | `SumpFloodingSystem.cs` (`SystemId = "sump_flooding"`) | per-node `isFlooded`, `contaminationLevel`, `FloodIncident` log |
| **Greywater/sludge (existing waste plant!)** | `SumpFloodingSystem.cs` Plan 70 block | `SumpFloodingState.dewateredCakeKg`, `dewateredCakeProfileId`, `hazardousTailingsKg`, `unroutedGreywaterLiters`, `centrifugeCondition` |
| Water treatment intake | `WaterTreatmentSystem.TryAddWaterFromSource` | `SumpGreywaterSourceId = "source_sump_greywater"` is the routed-greywater seam (Plan 189 sealed) |
| Infection resolution | `Disease/DiseaseSystem.cs:428 TryExpose(DiseaseExposureContext)` | authored `exposure_sources` rows in `disease_catalog.json` (`source_id`, `base_probability`, `mitigating_trait_id`); sanitation adds rows + feeds `ProbabilityModifier` only |
| Strain layer | `Disease/PathogenStrainSystem.cs` | merge-on-attach over DiseaseSystem; no parallel state |
| Morale | `DutyRoster/MoraleMarkSystem.cs` | marks ledger (`SetMark`/`HasMark`/`OnMarkSet`); **not a numeric meter** — hygiene modifiers surface as marks/thresholds |
| Work assignment | `DutyRoster/DutyRosterSystem.cs` + `Shelter/ShelterAssignmentSystem.cs` | role rows `DutyRosterAssignmentEntry{role, survivorId}` |
| Sanitation chemicals supply | `Shelter/ChlorAlkaliSynthesisEngine.cs` | processes already emit `sanitation_output_units` (cleaning consumable exists) |
| Fertilizer/agriculture | `Greenhouse/GreenhouseSystem.cs`, `Shelter/BioFermentationEngine.cs` | compost output must route through a real fertilizer item id, consumed by greenhouse |
| Ventilation emissions | `VentilationSystem.cs` (registered-source pattern) | same seam foundry heavy batches use (`BindVentilation` + register/deactivate) |
| Radiation authority | `DoseLedgerSystem` / `ExposureEnvironment` | sealed waste is tracked, never dose-computed, by sanitation |

**Cross-contamination guard:** chemical/radioactive waste must be rejected by
the compost path at the Core API level (type check, not data convention).

### 2.2 Plan 211 — Black Market & Syndicates

| Concern | Owner | Evidence |
|---|---|---|
| **Bounties** | `Factions/FactionBountySystem.cs` — ALREADY EXISTS | `IssuePatrolBounty` (`PatrolBountyStandingThreshold = -10`), severity ladder None/Moderate/Severe/Extreme at deltas -10/-15/-20, `sourceResolutionId` dedup, `OnBountyIssued`/`OnBountyResolved`, full Capture/Restore |
| **Debt** | `LedgerDebtSystem.cs` + `DebtBountyRecord.cs` + `DebtConsequenceDispatcher.cs` + `DebtTemplateCatalog.cs` + host `Main.DebtCredit.cs` | `debt_ledger` save section; `DebtLedgerDayOwner` phase 4 |
| Mercenary bounty contracts | `Economy/MercenarySystem.cs` | `mercenary_bounties` save section — separate concern, untouched |
| Illicit barter route | `Narrative/ContrabandBrokerCaravan.cs` (Plan 147, sealed) | `use_canonical_item_values` + `ScarcityPremiumBp = 12500` — the pricing-shape precedent for syndicate stock |
| Contraband discovery | `contraband_stash` save section + `BunkerContrabandCatalog` | once-only activations gate broker stock |
| Faction standing | `FactionStanceEngine` / `IFactionStanceProvider` (`GetTrust/ModifyTrust/GetRaidAggression`) | `StandingMin/Max = ±100` (foundry mirror confirms the range) |
| Raid/encounter authority | `DutyRoster/ShelterEncounterSystem.cs`, `EncounterChoice`/`travel_encounters` sections | syndicate retaliation must request through here — no bespoke combat |
| Expedition risk | `Expeditions/ExpeditionSystem.cs`, `ExpeditionEncounterBridge.cs`, `expedition_stealth` section | smuggling adjusts encounter inputs, never bypasses |
| Addiction | `Medical/ChemicalDependencySystem.cs` (Opioid/Alcohol/Stimulant/Sedative; `DependencyThreshold 0.3`, `+0.15/dose`, `-0.05/day` clean) + `Medical/NarcoticsSystem.cs` (`tolerance_gain`, `dependency_pressure`, `withdrawal_profile_id`, `trade_value`) | illicit stimulants = new catalog rows; zero new dependency architecture |
| Caravan scheduling | `Economy/CaravanTradeNetworkSystem.cs` | syndicate contact arrival rides caravan/waystation cadence |

**Underworld reputation decision (proposed default, §4):** syndicates are real
faction IDs resolved through `FactionStandingIdResolver.ToSystemsId`; debt,
heat, and access tier ride a small scoped `UnderworldLedger` inside the black
market system — **not** a second faction engine and **not** a new generic
reputation dimension. `FactionStanceEngine` trust remains the standing
authority for syndicate factions; the ledger holds only what standing cannot
express (active debt rows, heat, access tier).

### 2.3 Plan 212 — Dynamic Economy

**`Economy/MarketSystem.cs` IS the dynamic economy.** A `DynamicEconomySystem`
class would be a competing price authority — forbidden.

Confirmed public contract (read in full, 540 lines):

| API | Behavior |
|---|---|
| `TickDay(day, ISeededRng)` | deterministic per-good volatility walk, clamped [0.25, 4.0] |
| `AdjustDemand(itemId, delta)` | scarcity nudge (clamped) |
| `GetPrice(itemId)` / `ExplainPrice(itemId, side)` | base × demand → floor/ceiling clamp at 0.25×/4× base; typed `PriceFactorRecord` trail |
| `Buy/Sell/Barter` | ledger booking at current price; barter books equal exchanged value with explicit remainder |
| `IsSuppliesShort()` | mean demand ≥ 1.35 |
| `CaptureState/RestoreState` | versioned (`MarketState.Version = 1`), first-wins dedupe, loud failure on newer saves |

Gap analysis vs the flagship requirements — Plan 212 **extends** MarketSystem
with:

1. **Category indices** (`commodity_baselines.json`): per-`GoodCategories`
   elasticity class, scarcity floor/ceiling multipliers, shock profiles.
   *No item base-price duplication — `economy_goods.json` stays the value
   authority.*
2. **Shock events with expiry** (crash/shortage): new typed state, persisted,
   deterministic seeding, bounded multipliers folded into `AdjustDemand`.
3. **Trade-pressure memory**: decayed buy/sell pressure derived from the
   existing `LedgerEntry` stream (already persisted!) — no second ledger.
4. **Weather/world event modifiers**: consumed via `IWeatherSeverityProvider`
   and existing event contexts; economy never polls world state directly.
5. **Inflation smoothing**: the current walk is unbounded within clamps;
   add a bounded daily pull toward the category target multiplier.
6. **Merchant stock response**: `CaravanTradeNetworkSystem`/`ShelterBarterSystem`
   query category indices for restock priority — stock refresh cadence stays
   with the caravan owners.

**Arbitrage controls (mandatory tests):** buy→sell round-trip must lose value
(spread + demand feedback); `ContrabandBrokerCaravan`'s 1.25× premium is the
existing precedent — syndicate premiums must be ≥ broker premium for the same
items.

### 2.4 Plan 213 — Metallurgy Reconciliation

**`Foundry/SilentFoundrySystem.cs` IS the metallurgy authority** (partials
`.Heat`, `.Metallurgy`, `.Glassworks`, `.TreatyLabor`). No new metallurgy
system, queue, heat sim, or save store.

Reconciliation matrix:

| Feature | Status | Owner | Plan 213 action |
|---|---|---|---|
| Heat stages | ✅ | `FoundryHeatStage` Idle→ChargeLoaded→Preheat→AtHeat→Tapped→Casting→Cooling→Complete; `CurrentPowerDemandKw`/`CurrentWasteHeatKw` authored per stage | reuse; no second heat sim |
| Heavy recipes | ✅ | `MetallurgyHeavyCatalog` + `metallurgy_recipes.json` (`metallurgy_*` ids, flux/slag/heat_tier/quality_target) | extend rows only |
| Slag/crucible | ✅ | `SilentFoundrySystem.Metallurgy.cs` (`metallurgySlag` 0..100, `IsHeavyBatchActive`) | reuse |
| Powder metallurgy | ✅ | `PowderMetallurgySystem` (`quality_floor/ceiling`, `wear_multiplier_*`, `PowderMetallurgyBatchRecord.quality01`) | reuse as the quality-pattern precedent |
| Cupola/extrusion | ✅ | `CupolaFoundryEngine`, `HydraulicExtrusionEngine` (Plans 90-93/140) | separate engines, untouched |
| **Purity tiers** | ❌ | — | new: `Poor/Standard/High/Exceptional` derived from existing `FoundryQualityTier` + pendingQuality path |
| **Forging interaction** | ❌ | — | new: deterministic command-sequence abstraction in Core (heat/shape/finish/inspect decisions → quality result); UI presents |
| **Item material provenance** | ❌ | — | new: minimal generalized component (see §5) |
| **Vehicle armor** | ❌ **no `ModularVehicleSystem` exists** (grep: zero matches) | — | BLOCKED for direct armor; ship material provenance + component handoff seam only; vehicle integration requires the vehicle owner to exist first |
| Advanced furnace tier | ⚠️ | Cupola exists as separate engine | treat as distinct facility; no merge into SilentFoundry |
| `alloys_and_ores.json` | ❌ | — | allowed **only** as material-family definitions; recipes stay in `metallurgy_recipes.json` (single recipe authority) |

---

## 3. DAILY TICK ORDERING (existing registry, `src/Main.CampaignOwners.cs`)

Registered owners (verified): phase 1 = holdfast_core, maritime_deep_coast,
power_grid, nuclear_core, geothermal_orc, weather_world, seismic_geology;
phase 2 = crafting_production, **economy_market**, greenhouse_foundry,
aeroponics, pneumatic_dispatch, cryo_vault, precision_metrology, aquaponics,
shelter_facilities, shelter_fire, starting_level_rations, plan_168_fluid;
phase 3 = duty_roster, **medical_disease**, phase0_psychology,
survivor_social, survivors_needs; phase 4 = expeditions_caravans,
narrative_quests_verdict, world_evolution, **debt_ledger**,
subterranean_network, psyops, radio_program_production,
psychology_arcs_162, plan_167_espionage, plan_169_procedural_narrative;
phase 5 = host_events, memorial, shelter_room_history.

New registrations (additive, within existing phases — never new phases):

| Proposed ownerId | Phase | Rationale |
|---|---|---|
| `hygiene` (sanitation state) | **3** | Within a phase owners tick **alphabetically by ownerId** — `medical_disease` (m) sorts before `sanitation` (s), so the section key cannot be the owner id. `hygiene` (h) sorts before `medical_disease`: burden → hygiene → exposure modifiers update before the disease daily tick. **Correction 2026-09-13:** the original wave-1 text claimed `medical_disease` sorts after `sanitation`; it does not — the owner id `hygiene` preserves the intended invariant instead. |
| `black_market` (ownerId `underworld_market`) | **4, after `debt_ledger`** | debt checks precede syndicate collection; stock refresh is daily-persistent. **Correction 2026-09-13:** `black_market` (b) sorts before `debt_ledger` (d) alphabetically, so the owner id is `underworld_market` (u > d) — the save section key stays `black_market`. |
| metallurgy forging state | **rides existing `greenhouse_foundry` phase-2 owner** | no new owner for the foundry's own state. |

Plan 212 needs **no new owner** — `economy_market` (phase 2) already ticks the
market daily; shock/decay logic is added inside `MarketSystem.TickDay`'s
existing contract.

---

## 4. DECISIONS PENDING FOREMAN APPROVAL

| # | Decision | Proposed default | Alternative |
|---|---|---|---|
| D1 | Canonical plan IDs | 210–213 as in §1 | renumber the historical corpus (rejected — sealed docs) |
| D2 | Waste distribution granularity | room-level `waste` map keyed by room id inside one `sanitation` state | global pool only (weaker UI/hazard story) |
| D3 | Underworld reputation | scoped `UnderworldLedger` (debt/heat/access) inside black-market system; standing stays in FactionStanceEngine | full separate reputation dimension (rejected — R3 duplicate risk) |
| D4 | Syndicate faction ids | real faction ids via `FactionStandingIdResolver` | hidden pseudo-factions (rejected) |
| D5 | Market extension shape | extend `MarketSystem` in place; new state fields additive (`MarketState.Version = 2` migration) | wrapper service (rejected — second authority) |
| D6 | Vehicle armor | defer until a vehicle-armor owner exists; ship provenance + handoff seam | build `ModularVehicleSystem` inside 213 (rejected — out of scope) |
| D7 | Compost item | new fertilizer item id consumed by greenhouse | reuse existing fertilizer item if one exists in items.json (verify before implement) |

---

## 5. SAVE STRATEGY (additive; 184 existing sections untouched)

| New section | Owner | Contents | Legacy default |
|---|---|---|---|
| `sanitation` | sanitation | waste per room/type, facility processing queues, active spill, cleaning targets/priorities, compost queue, cooldowns, hygiene inputs | clean baseline: zero waste, spill null, hygiene inputs neutral |
| `black_market` | black_market | discovered contacts, underworld ledger (debt rows w/ principal+due day+repaid, heat, access tier), daily stock snapshot, bounty/raid cooldowns, active smuggling contracts, fired-event ids | undiscovered, zero debt, no bounty, empty stock (regenerates deterministically) |
| `economy` (existing — extended) | economy | `MarketState` v2 additive fields: category indices, trade-pressure memory, active shocks (commodity, severity, start, expiry, source) | v1 fields read as neutral (multiplier 1.0, no shocks); no retroactive inflation |
| `silent_foundry` (existing — extended) | foundry | additive purity/quality provenance on batch records + item quality component; forging sequence progress on active batch | old items/batches get `Standard`/unknown quality; **never** recalculated retroactively |

Item provenance: minimal generalized component
(`material_profile_id`, `material_quality`, `craft_quality`) appended to the
item-instance record path — reusable by powder metallurgy and future systems;
no metallurgy-only float fields on every item.

---

## 6. EVENT IDS (typed events primary; string bus optional)

New Core events (namespaces follow owner):

```text
sanitation.spill_started        {roomId, wasteType, severity, day}
sanitation.spill_resolved       {roomId, day}
sanitation.hygiene_threshold    {roomId|shelter, band}   # Squalid/Hazardous crossings
sanitation.compost_ready        {roomId, fertilizerItemId, units, day}
black_market.contact_discovered {syndicateId, day}
black_market.debt_issued        {loanId, syndicateId, principal, dueDay}
black_market.debt_overdue       {loanId, syndicateId, daysOverdue}
black_market.debt_repaid        {loanId, amountRepaid}
black_market.bounty_placed      {bountyId, syndicateId, severity, reason, placedDay}
black_market.stock_refreshed    {syndicateId, day, entryCount}
economy.shock_started           {commodityId|category, kind(shortage|crash), severity, expiryDay}
economy.shock_expired           {commodityId|category, kind}
metallurgy.batch_completed      {batchId|jobId, productItemId, qualityTier, day}   # Godot: sparks/steam/audio
metallurgy.batch_failed         {batchId, reason, day}
metallurgy.forging_completed    {batchId, finalQuality, day}
```

`OnBountyPlaced` requested by the flagship doc maps to
`black_market.bounty_placed` for syndicate bounties; faction bounties continue
using `FactionBountySystem.OnBountyIssued` unchanged.

---

## 7. RNG STREAMS (additive to `CampaignStreamIds`, snake_case, StableHash-derived)

```text
sanitation_spill            # spill event selection
sanitation_pest             # vector/pest rolls
black_market_stock          # daily syndicate stock
black_market_bounty         # bounty escalation rolls
black_market_debt_event     # debt-collection variety
economy_shock               # explicit market events (pricing itself stays formulaic)
economy_merchant_stock      # merchant restock variety
metallurgy_quality          # forging quality bands
metallurgy_hazard           # furnace accident rolls
```

Fork-per-day pattern (`CampaignRngManager.Fork(streamId, day)`) as used by
Plans 122–125 — position-independent, cannot shift existing streams.

---

## 8. SHARED PATHS (integrator-only)

`src/Main.CampaignOwners.cs` (owner registration),
`Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` (+3 rows),
`Assets/Ashfall.Core/Random/CampaignStreamIds.cs` (+9 streams),
`Assets/Ashfall.Core/HostCliRegistry.cs` + `src/Host/HostCli.cs` +
`src/Main.Application.cs` (selftest verbs),
`scripts/ci/generate-architecture-map.py` (+nodes),
`Ashfall.Core.Tests/Save/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`
(section-count gate),
`docs/INDEX.md` (generated).

Builders claim disjoint Core/data/host files per package and never touch the
above except through the integrator.

---

## 9. EXECUTION ORDER (unchanged from flagship doc, renumbered)

1. Wave 1 — this map + ledger/claim registration *(current)*
2. Wave 2 — Plan 212 economy core (MarketSystem extension + commodity catalog)
3. Wave 3 — Plan 210 sanitation core (state + facilities catalog + tick)
4. Wave 4 — Plan 211 black market (contacts/stock/ledger/debt/bounties)
5. Wave 5 — Plan 213 metallurgy reconciliation (purity/forging/provenance)
6. Wave 6 — cross-plan integrations (sanitation↔economy, compost↔agriculture,
   syndicates↔patrols, metallurgy↔supply)
7. Wave 7 — UI/presentation (SanitationUI, BlackMarketUI, Economy ticker,
   Metallurgy UI) via the panel-registry pattern
8. Wave 8 — deterministic replay + flagship scenarios A–F
9. Wave 9 — full CI/content closure + closeout docs
   (`docs/shelter/PLAN_210_SANITATION_CLOSEOUT.md`,
   `docs/economy/PLAN_211_BLACK_MARKET_CLOSEOUT.md`,
   `docs/economy/PLAN_212_DYNAMIC_ECONOMY_CLOSEOUT.md`,
   `docs/crafting/PLAN_213_METALLURGY_RECONCILIATION_CLOSEOUT.md`)

Focused verification per package: `bash scripts/run_test.sh <focused-target>`;
new catalogs also walk `--data-integrity-selftest`.
