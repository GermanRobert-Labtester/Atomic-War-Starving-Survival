# CONTRABAND MECHANICS AUTHORITY MATRIX — Plan 147 Task A.5

One row per authored mechanics key (45 total: 14 typed + 31 silently-dropped),
each classified **LIVE-MAPPED**, **PRESENTATION-ONLY**, **DESCRIPTIVE (risk
flavor)** or **NON-EXECUTABLE / DEFERRED**, with the single owning authority or
the explicit statement that no owner exists. Rule of construction: *a
contraband field may combine existing systems in dangerous ways; it may not
become a replacement implementation of them.*

**Validation enforcement:** every key below is frozen in
`ContrabandCatalogValidator.KnownMechanicsKeys`. A new mechanics key fails
catalog validation until a disposition row is added here. Range classes are
mechanically enforced (probabilities [0,1], multipliers > 0, quantities ≥ 0,
NaN/Infinity rejected at parse level).

## Tier 1 — LIVE-MAPPED (executed only through an existing owner)

| Field | Owner | Compatible API | Units / range | Persistence owner | Decision |
|---|---|---|---|---|---|
| `morale_delta` (sugar brick only, in slice) | Canonical item `sugar.moraleEffect` (=1) applied by the host item-use pipeline (`Needs.Modify(Morale)`) | Item-use event only | item morale points | items.json (item authority) | **LIVE-MAPPED via canonical item.** The contraband row's own `morale_delta=12` is NOT executed. Authorized morale comes from the canonical sugar granted by the stash, applied once per use through the existing pipeline. |
| `agricultural_yield_multiplier` (seed vial, in slice) | `GreenhouseExpansionCatalog.CropCatalog` (`item_seed_wheat` → `crop_wheat`) | Planting the canonical seed item | crop BaseYield units | greenhouse save section | **LIVE-MAPPED via canonical seed.** The JSON `2.0` multiplier is NOT executed; the crop catalog's wheat yield is the agriculture authority. |

## Tier 2 — PRESENTATION-ONLY / DESCRIPTIVE (no executable consumer, kept as risk/flavor metadata)

| Field | Would-be domain | Real owner found? | Decision |
|---|---|---|---|
| `tribunal_suspicion_rate` (all 20 entries) | justice/tribunal | **None.** `JusticeSystem` (Plans 190–193) has no suspicion meter; no standing/suspicion state accepts a rate. Plan §2.8 forbids creating one. | **DESCRIPTIVE risk profile.** Non-executable. Rendered (if ever) only as player-knowable flavor from `risk_profile`, never as a hidden polled probability. |
| `trade_value_bonus` (condenser coil) | economy | None that accepts a global buy/sell bonus; barter valuation is per-transaction base-value math in `ShelterBarterSystem`. | **DESCRIPTIVE.** Not a pricing authority (see TRADE_AND_ARBITRAGE_AUDIT). |
| `trade_multiplier` (coffee) | economy | None. | **DESCRIPTIVE.** |
| `market_price_scrip` (entry-level, all 20) | currency | **No scrip currency runtime exists.** | **DESCRIPTIVE.** Canonical trade value = item `tradeValue` owned by items.json. Never converted 1:1 (audit doc). |
| `morale_delta` (17 non-slice entries) | morale | Owner exists (item-use pipeline) but these entries have no canonical consumable identity yet. | **DEFERRED.** Executable only after per-record identity review maps each to a canonical item; never on load/view. |
| `calorie_surplus_kcal`, `hunger_restore_value` (sugar brick, lard tin) | needs | Canonical items own `hungerRestore`; kitchen nutrition system consumes items. Contraband rows are not item definitions. | **DEFERRED / DESCRIPTIVE.** No parallel calorie pool may exist. If lard tin gets an item identity later, hunger flows from that item's `hungerRestore`. |
| `blackout_illumination_hours` (candles) | lighting/power | No illumination-hour mechanic in shelter systems. | **DESCRIPTIVE.** |
| `faction_influence_rebuilder`, `faction_influence_tempest` (pamphlet) | factions | Faction stance engines exist but no typed action maps a pamphlet reading to influence deltas. | **DEFERRED.** Needs a real narrative encounter/choice event as producer; never a load-time effect. |
| `dweller_loyalty_boost` (tattoo rig) | social | No loyalty meter. | **DESCRIPTIVE.** |
| `infection_risk_percentage` (tattoo rig) | medical/disease | Disease infection routes exist (`DiseaseSystem.Infect`) but no item-use → infection hook for tattooing. | **DEFERRED.** |
| `scrip_gambling_win_rate_boost`, `barter_brawl_chance` (card deck) | gambling/social | No gambling minigame; no brawl encounter trigger keyed to cards. | **DESCRIPTIVE** (the "marked deck" advantage is prose, not a mechanic). |
| `fermentation_accelerator` (sugar) | crafting | Distillation/crafting recipes exist; no recipe input modifier of this shape. | **DESCRIPTIVE.** |
| `waterproof_boot_grease_uses` (lard) | equipment | Equipment condition system has no lard-grease interaction. | **DESCRIPTIVE.** |
| `battery_recharge_slots`, `noise_generation_db`, `emergency_power_storage_kwh`, `recharge_cycle_durability`, `acid_spill_hazard` (dynamo, NiCd cell) | power | Power systems (`PowerSupplyContext` family) have no player-attachable battery-cell contract matching these. | **DEFERRED.** |
| `alcohol_proof_precision`, `distillation_yield_bonus` (hydrometer) | crafting | Still/distillation content exists narratively; no yield-modifier API. | **DESCRIPTIVE.** |
| `diesel_theft_liters_per_use`, `fuel_sabotage_potential` (siphon) | fuel theft | Fuel inventory exists; no theft-action contract. | **DEFERRED** (would need an explicit expedition/shelter action with costs & consequences). |
| `rad_detection_precision_multiplier`, `early_storm_warning_minutes` (geiger crystal) | detection/weather | Dosimeter/geiger items exist; no range/precision multiplier mechanic. | **DESCRIPTIVE.** |
| `rad_resistance_delta`, `air_flow_ease_factor` (modified filter) | radiation/ventilation | `WornGear` rad protection is owned by `Inventory.FillWornGear` + item defs. A gutted canister would need its own item def to be live. | **DEFERRED** — and negative protection must route through a real item definition, never through this row. |
| `fatigue_reduction_hours` (coffee) | needs | Fatigue is a canonical need; no coffee item exists in items.json. | **DEFERRED.** |
| `requires_clean_water_liters` (condenser coil) | water | `clean_water` inventory items are the authority (units = items, ~0.5 l each). "Litres" here have no live conversion. | **DESCRIPTIVE.** If the coil gains a use action later, its cost must be canonical `clean_water` item counts, not a litre pool. |
| `radio_range_boost_km` (triode tube) | radio | **No radio-range mechanic exists** in the radio systems. | **NON-EXECUTABLE.** |
| `emp_shielded` (triode tube) | electronics | Item defs have an `empShielded` flag; this row is not an item. | **DESCRIPTIVE** (would transfer to an item def if identity review makes it one). |
| `scrip_purchasing_falsification`, `vending_machine_jam_chance` (lead slugs) | currency fraud | **No scrip currency, no vending machines, no counterfeit economy.** Plan §10 explicitly forbids inventing one. | **NON-EXECUTABLE / REMOVE-CANDIDATE.** Keep only as prose support for the `currency` risk profile. |
| `ration_chit_forgery_success_rate`, `shift_absence_concealment` (muster stamp) | ration/muster forgery | Muster/duty-roster systems have no forgery action. | **NON-EXECUTABLE / DEFERRED.** |
| `instant_pain_relief_hp`, `trauma_suppression_duration_days` (morphine) | health | **Plan 147 follow-up: LIVE for pain relief** — canonical `morphine` item now exists in items.json (`healthEffect` 35), applied by the host item-use pipeline (`Inventory.Consume` → `Needs.Modify(Health)`). The JSON `40` is NOT executed. `trauma_suppression_duration_days` remains DESCRIPTIVE (no trauma-suppression-duration mechanic). | **LIVE (pain relief) via canonical item; trauma duration DESCRIPTIVE.** |
| `chemical_dependency_risk` (morphine) | dependency | **Plan 147 follow-up: LIVE-MAPPED.** Host `InventoryHostSession.OnConsumed` hook (Main.Plans147) routes committed consumptions of dependency-catalog items into `ChemicalDependencySystem.OnSubstanceConsumed(survivorId, itemId, kind)` — exactly one call (one dose, `DependencyIncreasePerDose`=0.15) per committed consumption; kind resolved from `chemical_dependency_items.json` (`morphine` → opioid). The **system's** probability contract is authoritative; the JSON `0.35` is NOT executed. | **LIVE-MAPPED via the dependency authority.** Exactly one dose per authorized consumption event; pinned by `ChemicalDependency_Morphine_ExactlyOneDosePerAuthorizedConsumptionEvent`. |
| `blast_door_override_success_rate`, `alarm_trigger_chance` (keycard flasher) | doors/security | `AirlockSecuritySystem` has **no override success-rate extension point.** Plan §2.14 requires an explicit extension or deferral. | **NON-EXECUTABLE / DEFERRED.** No UI-owned door bypass may be invented. |
| `genetic_integrity_score`, `century_tree_viability` (seed vial) | agriculture | No such metrics in the crop catalog. | **DESCRIPTIVE.** |
| `shift_absence_concealment` — see muster row above | | | |

## Enforcement summary

1. **No generic executor exists or may be added.** The only executable bridge is
   `ContrabandStashSystem`'s activation map (entry → canonical item grant), and
   its grant path is `InventoryBill` over items.json ids.
2. **No mechanics field is read by runtime code at all.**
   `ContrabandMechanics` remains a loaded-but-inert typed shape; the two
   LIVE-MAPPED effects are executed by the *canonical item's own* fields
   (`sugar.moraleEffect`, crop catalog wheat yield), proven by
   `AuthorizedEffect_*` tests.
3. **Fail-closed validation:** unknown keys, out-of-range values, NaN/Infinity,
   negative prices, invalid tiers and duplicate ids are hard validation errors
   (`ContrabandCatalogValidator`), not silent drops.
