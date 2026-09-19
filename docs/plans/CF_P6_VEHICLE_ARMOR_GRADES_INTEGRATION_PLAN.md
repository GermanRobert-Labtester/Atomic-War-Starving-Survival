# CF-P6 — Vehicle Armor Grades Integration Plan

**Package:** `CF-P6-VEHICLE-ARMOR-GRADES` (completion-first program "Plan 04")
**Anchor:** Plan 213 decision D6 (approved deferral, 2026-09-13) · Plan 50 vehicle garage seam (landed Wave 8 B2, 2026-09-17) · deep-dive Part B armor ladder (2026-09-18) · eight-plan execution program §C.3 (2026-09-19)
**Document class:** Premise note + implementation-ready integration plan. This file *is* the P0 entry gate the active queue requires ("new claim + premise note" — `docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md` row 3).
**Status:** INTEGRATED — CF-P6 implementation and focused acceptance gates completed 2026-09-19; closeout evidence is recorded below.
**Evidence date:** 2026-09-19. Every load-bearing claim below was re-verified against current source on that date; section 4 lists the exact files, lines, and grep results.
**Closeout:** `Plan213VehicleArmorGradeTests` 21/21; existing garage contracts 6/6 + 5/5 + 3/3; Godot vehicle self-test 27/27; data-integrity 0 errors + 5 pinned warnings; content-utilization/deep-chain PASS; catalog boot 338/338; panel lifecycle 17/17; UI accessibility 5/5; Core/host builds 0 warnings/0 errors; architecture/catalog/docs generator checks PASS. The only acceptance correction was the validator's tier-prefix rule, fixed to match the authored `grade_<tier>_...` IDs.

---

# 1. Objective

Author and wire **four vehicle armor grade tiers** on top of the already-sealed Plan 50 vehicle garage decoration seam, so that:

1. A player can fit exactly one bounded armor grade to an owned, serviceable garage vehicle through the existing garage command surface, paying real canonical-inventory materials.
2. A fitted grade reduces the vehicle's own travel risk channel (`breakdownChancePerTick` on the expedition profile) by a bounded, authored permille — never to zero, never to immunity — and redirects an authored fraction of chassis-bound trip wear into a sacrificial plate integrity pool.
3. The Silent Foundry's Plan 213 material-quality handoff (`TryGetLatestMaterialQuality*`, built for exactly this deferral and unconsumed since) finally gets its vehicle consumer: foundry provenance stamps the plate's integrity pool at install time.
4. The whole feature is data-driven (one new authoritative JSON catalog), engine-free in Core, deterministic (zero new RNG), save-safe (additive fields with legacy-neutral defaults), and presented truthfully on `VehicleGaragePanel`.

**Non-goals (explicit):** no combat damage model (no live system damages overland vehicles in combat — see §4.4); no second vehicle authority; no new save section; no new item ids; no radiation protection change (owned by protection-slot mods); no multi-plate stacking (rejected, §4.5); no Unity anything.

---

# 2. Current Reality

Everything in this section was read in current source, not inferred from docs.

## 2.1 The vehicle owner exists and is live

`Assets/Ashfall.Core/Expeditions/VehicleGarageSystem.cs` is the Core authority for overland vehicle customization and maintenance:

- **State types** (all `[Serializable]`, plain fields, engine-free):
  - `VehicleCustomizationRecord`: `vehicleId`, `installedSlots` (`Dictionary<string,string>`, ordinal), `chassisStressPermille`, `engineFoulingPermille`, `transmissionWearPermille`, `isImmobilized`, `immobilizedReason`.
  - `VehicleRecoveryMission`: `missionId`, `strandedVehicleId`, `locationId`, `requiredFuelUnits = 10`, `progressTicks`, `requiredTicks = 120`, `isComplete`.
  - `VehicleGarageState`: `systemId = "vehicle_garage"`, `vehicleRecords`, `activeRecoveries`, `nextRecoveryCounter = 1`.
- **Slot vocabulary** (constants on the system): `cargo`, `protection`, `mobility`, `engine`, `utility`.
- **Public surface** (verified against the file): `LoadCatalog`, `HasModification`, `GetModification`, `GetAllModifications`, `GetRecord` (read-only, null when no record), `IsImmobilized`, `ActiveRecoveries`, `GetInstalledSlots`, `DecorateProfile` (documented "read-only over persisted state; zero RNG"), `AdvanceRecoveries`, `GetOrCreateRecord`, `HasVehicleRecord`, `CanInstallModification`, `InstallModification`, `UninstallModification` (50% scrap-line refund), `GetEffectiveCargoCapacityDelta`, `GetEffectiveSpeedMultiplierDelta`, `GetEffectiveFuelConsumptionMultiplier`, `GetEffectiveWearRateMultiplier`, `GetEffectiveRadiationProtectionPermille` (clamped [0, 950]), `RecordTripWear`, `ServiceChassis`/`ServiceEngine`/`ServiceTransmission`, `RegisterRecoveryMission`, `AdvanceRecoveryMission`, `CompleteRecoveryMission` (clamps wear to 800 and clears immobilization), `CaptureState`/`RestoreState` (both round-trip through `SystemTextJsonSerializer` — additive record fields persist with **zero codec work**, a fact that shapes §12).
- **Wear model** (`RecordTripWear`): `baseWear = round(distanceKm * 2 * wearMult)` where `wearMult = GetEffectiveWearRateMultiplier * max(0.5, roadRoughness)`; chassis `+baseWear`, engine `+round(baseWear*0.8)`, transmission `+round(baseWear*0.9)`; any component reaching 1000 permille immobilizes with reason `"Critical component catastrophic failure during overland transit."` Service commands consume `scrap_metal` (chassis, 1 per 50 permille) or `mechanical_parts` (engine/transmission, 1 per 100 permille) through `IPlayerInventoryPort` with atomic `TryConsume`. `CheckClearImmobilization` clears the flag when all three components fall below 900.
- **Install gate** (`CanInstallModification`): vehicle id, slot type, catalog membership, slot-type match, immobilized refusal, install-cost sufficiency. **Notably it does NOT enforce `compatible_vehicle_tags`** — the catalog authors them, no code consumes them (grep-verified). This matters for §10: the armor grade gate gets its own enforceable compatibility rule instead of copying an unenforced one.

## 2.2 Catalogs

- `Assets/StreamingAssets/Data/vehicle_modifications.json` (`schema_version: 1`): 8 mods. Two already occupy the `protection` slot: `vmod_lead_lined_cab` (radiation 400‰, speed −0.1, fuel ×1.1, tags include `"armor"`) and `vmod_reinforced_bullbar` (wear ×0.85, radiation 50‰). **Neither is a damage-mitigation armor grade** — one is cab radiation shielding, the other a wear-rate mod. The word "armor" appears as a *tag* only; no grade semantics exist.
- `Assets/StreamingAssets/Data/vehicles.json` (`schema_version: 1`): 8 vehicles (`vehicle_utility_quad`, `vehicle_dirt_bike`, `vehicle_cargo_truck`, `vehicle_steam_halftrack`, `vehicle_armored_mobile_base`, `vehicle_salvage_dredger`, `vehicle_scout_motorcycle`, `vehicle_ambulance_rig`) with `terrain_type` ∈ {`rough`, `road`, `coastal`} plus one `track_gear` row. Vehicle rows carry **no tags array** — `terrain_type` is the only class discriminator the data actually contains.
- Catalog DTO/loader pattern (`VehicleGarageCatalog.cs`): `VehicleGarageCatalog` → `List<VehicleModificationDefinition>` (`id`, `display_name`, `description`, `slot_type`, `compatible_vehicle_tags`, `install_cost`, `install_labor_ticks`, `effects`, `tags`); `VehicleModificationEffects` carries the five effect fields with float defaults (`fuel_consumption_multiplier = 1.0`, `wear_rate_multiplier = 1.0`); loader is a thin `VehicleGarageCatalogLoader.Load(json, IJsonSerializer)`.

## 2.3 Host wiring (the seam is complete; armor needs almost nothing new here)

- `src/Host/ExpeditionHostSession.cs`: optional `Garage` property ("Unbound ⇒ legacy behaviour is byte-identical"); `BuildProfile` = `Vehicles.CreateExpeditionProfile(vehicleId, KmPerTravelTick)` then `Garage?.DecorateProfile(profile)`; `PrepareVehicleForDispatch` refuses immobilized vehicles, burns fuel, calls `Vehicles.PrepareForExpedition`, then feeds `Garage.RecordTripWear(vehicleId, distanceKm)` and auto-registers a recovery mission when the trip immobilizes.
- `src/Main.Plans50_53.cs`: `EnsureVehicleGarage()` constructs with RNG `_campaignDay.Rng.Fork("vehicle_garage")` (fallback `SeededRng(50)`), loads `vehicle_modifications.json` from `_dataDir`, restores from `VehicleGarageSaveStore.TryLoad()`; `SaveVehicleGarage()` captures via `CaptureSection("vehicle_garage", payload)`; dirty-flag flush in `FlushPlans50To53`.
- `src/Main.CampaignOwners.cs` (line ~903): the campaign day owner calls `EnsureVehicleGarage().AdvanceRecoveries(24)` per day — recovery advances over campaign days, exactly as the claim row states.
- `src/Host/VehicleGarageSaveStore.cs`: `SaveStoreHub.Checksummed<VehicleGarageState>("vehicle_garage_save.json", …, allowLegacyBareState: false)`, section `vehicle_garage`.
- `src/UI/VehicleGaragePanel.cs`: presentation-only panel (`IBindablePanel`), binds `(VehicleGarageSystem, ExpeditionVehicleSystem, Inventory)`; all math and inventory mutation stay in Core; status rail cards vehicles/immobilized/recoveries/fitted; install/uninstall/service/recovery commands forwarded verbatim.
- Selftest: `src/Host/HostCli.VehicleGarage.cs` — `--vehicle-garage-selftest`, 19 `Check(...)` gates (catalog load, acquire, install ×4, profile decoration ×2, uninstall, trip wear, service, immobilize, recovery ×3, save round-trip, panel bind/unbind), registered in `docs/ci/SELFTEST_MANIFEST.json` (`vehicle_garage_selftest`, 30 s timeout). Ledger confirms 19/19 PASS.
- Tests: `Ashfall.Core.Tests/Expeditions/VehicleGarageSystemTests.cs` (6 cases: install deducts/applies, install fails on slot/materials, trip wear immobilizes, service consumes/restores, recovery lifecycle, state round-trip) and `Ashfall.Core.Tests/Expeditions/Plan50VehicleGarageIntegrationTests.cs` (5 cases: decoration applies to profile, null/unrecorded no-op, slot map reflects installs/removal, catastrophic wear → recovery → clear, non-positive advance no-ops). Plus `Ashfall.Core.Tests/Campaign/Plans50_53_SharedIntegrationTests.cs` reads the real catalog file.

## 2.4 The risk channels armor can honestly touch

- `ExpeditionVehicleProfile` (`ExpeditionSystem.cs` lines 89–100): `vehicleId`, `speedMultiplier`, `cargoCapacityKg`, `breakdownChancePerTick`, `fuelPerTravelTick`. `CreateExpeditionProfile` derives `breakdownChancePerTick = clamp((100 − condition)/100 * 0.15 * gearBreakdownMult, 0, 1)`.
- `ExpeditionSystem.Start` samples the profile once into `exp.vehicleBreakdownChancePerTick`; `ExpeditionSystem.Estimate` reads the same profile field. **One sampled value feeds estimate and runtime** — the established share-the-sampled-value parity pattern. Decorating this field preserves estimate==runtime by construction.
- `RecordTripWear` is the only chassis-wear writer.
- **No combat system damages overland vehicles today.** `TravelEncounterCombatBinder` binds hostile travel encounters to *party* tactical combat; `RailwaySystem` ambush is the rail authority; the mortar/armor language in `C-integration-plans/C1_planintegration[44].md` is aspirational planning text with no live route (grep: no mortar→vehicle call path). The honest mitigation targets are therefore exactly two: the vehicle's mechanical breakdown risk channel, and chassis-bound trip wear. Anything more would fabricate a combat authority.

## 2.5 The foundry handoff built for this package

`SilentFoundrySystem.Material.cs` ships the D6 seam, unconsumed since 2026-09-13:

- `readonly struct FoundryMaterialQuality { MaterialProfileId; Purity; DurabilityModifierBp; ArmorModifierBp; CorrosionModifierBp; }` — the doc comment says "Material quality handoff for consumers (equipment/vehicle seam — D6)."
- `TryGetLatestMaterialQualityAny(out FoundryMaterialQuality)` and `TryGetLatestMaterialQuality(outputItemId, out …)`: latest completed provenance-bearing batch; false when the foundry never produced one or old saves carry no provenance.
- `FoundryPurityTier`: `Poor=0, Standard=1, High=2, Exceptional=3`.
- `alloys_and_ores.json`: 6 material profiles, `armor_modifier_bp` ∈ {700, 700, 800, 950, 1100, 1200} (bounds enforced by `MaterialProfileCatalogLoader.ModifierMinBp/MaxBp`). `ArmorModifierBp` currently has **zero consumers** — this package is its first.
- Host reachability: `Main.ExpansionHub.cs` owns `_silentFoundry` (`SilentFoundryHostSession`, `.Engine` → `SilentFoundrySystem`), so a bridge delegate can reach the query without `Main.Plans50_53.cs` taking a foundry dependency (§15).

## 2.6 Validation and registry infrastructure

- `CatalogIntegrityValidator` (`Assets/Ashfall.Core/CatalogIntegrityValidator.cs`) exposes per-catalog public static validators (`ValidateDifficultyPresetCatalog`, `ValidateWildlifeTrappingCatalog`, `ValidateDistressSignalStages`, `ValidateTradeEmbargoRules`, …) aggregated into the `--data-integrity-selftest` report (`src/Host/HostCli.SelfTests.cs` → `CatalogIntegrityValidator.Validate(dataDirectory, files)`). New-catalog rule hook = one more public static validator following the XP-WAVE1 difficulty-catalog precedent (which touched the validator for "difficulty catalog validation only" — same bounded pattern).
- `ContentUtilizationScanner.cs` registers `vehicle_modifications.json` in three places (known-files list ~line 127, loader map ~line 519, consumer map ~line 1185). A new JSON catalog without scanner rows is flagged as dead content — the rows are mandatory, not optional.
- `scripts/ci/generate-architecture-map.py` (line ~1995) owns the `vehicle_garage` node (`core: [VehicleGarageSystem]`, `catalog: [vehicle_modifications.json]`, `tests: [VehicleGarageSystemTests, Plan50VehicleGarageIntegrationTests]`, `store: [VehicleGarageSaveStore]`, `ui: [VehicleGaragePanel]`, `cli: [--vehicle-garage-selftest]`); regenerated docs (`ARCHITECTURE_TEST_MAP.md`, `docs/data/CATALOG_REGISTRY.md` via `generate-catalog-registry.py`, `docs/INDEX.md` via `generate-docs-index.py`) all run with `--check` gates.

---

# 3. Required Delta

Minimum coherent change set, in dependency order (detail in §19):

1. **New authoritative catalog** `Assets/StreamingAssets/Data/vehicle_armor_grades.json` — 4 fitted tiers + 1 authored neutral default row, full field set (§11), every install/reforge input an existing item id.
2. **New Core data file** `Assets/Ashfall.Core/Expeditions/VehicleArmorGradeCatalog.cs` — DTOs + strict loader (collected errors, snake_case, schema envelope) following the `MaterialProfileCatalogLoader`/`GoodsCatalogLoader` pattern. Data types only; no system.
3. **Additive record state** on `VehicleCustomizationRecord`: armor grade id, integrity permille, max integrity permille, provenance stamp (material profile id + purity name). Legacy default = no armor.
4. **Core grade resolution** on `VehicleGarageSystem` (extended, never forked): `LoadArmorCatalog`, read model `GetArmorProfile(vehicleId)`, gate/command pair `CanInstallArmorGrade`/`InstallArmorGrade`, upkeep command `ReforgeArmorPlate`, condition band helper.
5. **Decoration extension** inside the existing `DecorateProfile` (speed/fuel deltas + bounded breakdown mitigation) and **absorption extension** inside the existing `RecordTripWear` (chassis/plate split). No new seams; two method bodies grow.
6. **Validator hook** `CatalogIntegrityValidator.ValidateVehicleArmorGradeCatalog` + `ContentUtilizationScanner` rows.
7. **Host wiring**: load the armor catalog in `EnsureVehicleGarage`; bridge the optional material-quality provider to the foundry from `Main.ExpansionHub.cs` (lazy, provider-unset-neutral). **Zero changes to `ExpeditionHostSession`** — `BuildProfile` and `PrepareVehicleForDispatch` already route through the extended methods.
8. **Panel strip** on `VehicleGaragePanel`: grade, mitigation, integrity in words + numbers, REFIT/UPGRADE commands.
9. **Persistence**: nothing new — additive record fields ride the existing `CaptureState`/`RestoreState` SystemTextJson round-trip and the existing checksumed store; section count unchanged.
10. **Tests**: new focused suite (alone first), extended `--vehicle-garage-selftest`, legacy-parity pins, determinism fingerprint, 30-day seeded soak.
11. **Governance**: claim row, ledger row, regenerated `--check` docs, closeout.

---

# 4. Evidence

## 4.1 The false premise this package corrects

The deferral record, verbatim:

- `INTEGRATION_PLANS.md` line 238 (PLANS-210-213 batch premise): *"vehicle armor deferred (no vehicle owner exists)."*
- `docs/foreman/PLANS_210_213_FLAGSHIP_ECONOMY_AUTHORITY_MAP.md` §2 table (line 153): *"Vehicle armor | ❌ no `ModularVehicleSystem` exists (grep: zero matches) | BLOCKED for direct armor; ship material provenance + component handoff seam only; vehicle integration requires the vehicle owner to exist first."*
- Same document, §4 decision row D6 (line 196): *"Vehicle armor | defer until a vehicle-armor owner exists; ship provenance + handoff seam | build `ModularVehicleSystem` inside 213 (rejected — out of scope)."*
- `docs/crafting/PLAN_213_METALLURGY_RECONCILIATION_CLOSEOUT.md` line 22: *"Vehicle armor (D6): shipped as the read-only `TryGetLatestMaterialQuality(Any)` handoff query only — no vehicle authority exists, none invented."*

**The deferral condition is now false.** It was true on 2026-09-13 (D6 was a correct, approved deferral — nothing in this plan rewrites that history). On 2026-09-17 the Wave 8 B2 player-routes claim (`claim-wave8-b2-player-routes-2026-09-17`, DONE) landed `VehicleGarageSystem`'s player-facing completion: mod effects decorate the expedition profile, trip distance feeds component wear, immobilized vehicles are refused dispatch, recovery advances over campaign days, `--vehicle-garage-selftest` 19/19. `VehicleGarageSystem` **is** the vehicle owner D6 waited for — not a `ModularVehicleSystem` by name, but the authority by function: per-vehicle records, install gates, wear ledger, save section, panel, selftest. The correct action is to **extend it**, exactly as D6's "vehicle integration requires the vehicle owner to exist first" clause anticipated.

## 4.2 No armor-grade implementation exists

Grep `armor_grade|ArmorGrade|armorGrade|vehicle_armor` across the repository: zero hits in `Assets/`, `src/`, or `Ashfall.Core.Tests/` production/test code. Hits are confined to planning documents (the two Seal-steps programs), playtest artifacts (a table row naming `vehicle_armored_mobile_base` — a vehicle, not armor), sprite manifests, and these docs: `docs/expeditions/VEHICLE_LOGISTICS_MATRIX.md` / `VEHICLE_ROLE_MATRIX.md` (mission-profile prose), `docs/combat/PLAN10_COMPLETION_REPORT.md`, `Assets/StreamingAssets/Data/vehicles.json` (the vehicle id itself). No catalog, no slot type, no formula, no state.

## 4.3 Collision check — what already exists adjacent to "armor"

| Existing thing | What it actually is | Relationship to this package |
|---|---|---|
| `vmod_lead_lined_cab` (protection slot) | Cab radiation shielding (400‰), tagged `"armor"` | Independent. Radiation stays owned by protection-slot mod effects. A vehicle may fit both; effects compose through the existing multiplicative pipeline. |
| `vmod_reinforced_bullbar` (protection slot) | Wear-rate ×0.85 chassis-adjacent mod | Independent. Multiplies into `GetEffectiveWearRateMultiplier`; armor absorption runs *after* that product (order pinned, §10). |
| `ArmoredCrawlerExpeditionSystem.armorModifier` | Separate crawler authority's module stat | Out of scope; crawler is not a garage vehicle (different profile producer). No shared code path. |
| `RunFlatTireEngine` | Wheel hazard-resistance engine: bounded reduction never immunity (`loss = max(1, …)`), compatible-only install, real inventory gate | **The design precedent this plan copies** (bounded mitigation, never zero; severe classes still bite). |
| `MaterialProfileDefinition.armor_modifier_bp` | Foundry material stat, zero consumers since 2026-09-13 | Becomes the install-time integrity stamp input (its designed purpose per the D6 comment). |
| `vehicle_armored_mobile_base` | A vehicle id | Nothing to do; may wear grades like any road vehicle. |

Conclusion: no duplicate architecture risk. Extension is viable; no parallel system is proposed anywhere in this document.

## 4.4 What armor can mitigably act on (verified channels)

Only two live channels express "vehicle gets hurt" for overland garage vehicles: (a) `breakdownChancePerTick` on the expedition profile (mechanical failure risk while travelling), and (b) chassis stress from `RecordTripWear`. No combat→vehicle damage route exists (§2.4). Therefore "damage mitigation" in this package means exactly: bounded reduction of (a) and bounded redirection of (b) into a sacrificial plate pool. This is stated plainly so no reviewer reads "armor" as a combat stat.

## 4.5 Prior design authorities — reconciliation (two exist, they differ, this plan picks deliberately)

| Axis | Deep-dive Part B (2026-09-18) | Eight-plan program §C.3 (2026-09-19, newer) | **This plan adopts** |
|---|---|---|---|
| Shape | Up to 3 plates per vehicle, total mitigation capped at top grade | One `armorGradeId` per vehicle; install replaces | **Single grade per vehicle** — newer authority, anti-stacking by construction, smallest state, smallest save surface. Multi-plate stacking is **explicitly rejected** (stacking-cap validator burden, save complexity, and the cap rule means plates were really "one grade with extra steps"). |
| Mitigation ladder | −1000/−1500/−2000/−2500 bp (= 10/15/20/25%) | "bounded damage-reduction (permille, clamped)" | The ladder, restated as **permille of 1000: 100/150/200/250** (identical physical numbers; garage naming convention is permille — `chassisStressPermille`, `radiation_protection_permille`). |
| Wear absorption | 20/25/30/35% redirected to plate | "wear-decay multipliers (clamped)" | Deep-dive absorption fractions (200/250/300/350‰) — redirection keeps hull wear the majority (≥65%), preserving Plan 50 wear semantics. |
| Integrity pool | 100/140/180/240 | not specified | Adopted, in permille units, 1 permille absorbed = 1 integrity lost. |
| Recipes | scrap staircase + uncommon/rare inputs, no new ids | "existing item ids + workshop time" | Deep-dive staircase with **concrete verified ids** (§11): `scrap_metal`, `mechanical_parts`, `item_metallurgy_iron_ingot`, `item_ebpvd_ceramic_target_ingot` (all grep-verified in `items.json`). |
| Re-forge | 50% of scrap input, no rare re-input | — | Adopted (rare input = license, scrap = running cost). |
| Foundry quality | foundry difficulty per grade | "Material quality input: SilentFoundrySystem query (read-only)" | Quality stamps **integrity pool size** at install (bounded ±), never the mitigation permille — mitigation is an authored constant so balance can't drift with forge luck. |
| Mass penalty | not specified | — | **Added** (speed delta / fuel multiplier per grade, grounded against `vmod_lead_lined_cab`'s −0.1/×1.1): free armor would violate the scarcity thesis. |

Where the deep-dive marks numbers **[proposed]**, this plan inherits that status: the ladder is the signed program's design basis, but final authored values ship in the catalog PR and are falsifiable through the §18 soak.

## 4.6 Verified recipe inputs

From `items.json` (id presence grep-verified 2026-09-19): `scrap_metal` (Material, common scavenging tier, qty 1–3 urban tables per the deep-dive's grounded reading), `mechanical_parts` (Material), `item_metallurgy_iron_ingot`, `item_metallurgy_copper_ingot`, `item_ebpvd_ceramic_target_ingot` (rare industrial ingot), plus neighbors `item_sealed_lead_pig`, `metal_pipe`, `steel_rebar` (not used). No new item ids are invented anywhere in this plan.

## 4.7 Briefing cross-check

The parent briefing's claims were re-verified and hold, with four refinements worth recording:

1. `CanInstallModification` does **not** enforce `compatible_vehicle_tags` (briefing implied the install gate was complete) → armor defines its own enforced terrain gate (§10).
2. `CaptureState`/`RestoreState` round-trip through `SystemTextJsonSerializer` → additive record fields persist with no codec work (strengthens §12 beyond the briefing's "save additive fields").
3. `VehicleGarageSaveStore` uses `allowLegacyBareState: false` → all loads come through the schema-versioned envelope; legacy mapping is a field-defaults question, not an envelope question.
4. `ExpeditionHostSession` needs **zero edits** — the garage seam (`BuildProfile`, `PrepareVehicleForDispatch`) already routes every armor-relevant call. The program doc's file map listed it as MODIFY(profile consumption); current evidence narrows that to NO-CHANGE.

---

# 5. Existing Extension Seams

| Seam | Where | How armor uses it |
|---|---|---|
| Decorator seam | `VehicleGarageSystem.DecorateProfile` (doc: "The garage decorates; the expedition core stays decoupled"; zero RNG) | Second decoration block for grade speed/fuel/mitigation, after the mod block. |
| Wear ledger seam | `RecordTripWear` | Chassis/plate split at the single `baseWear` computation point; engine/transmission lines untouched. |
| Command pair pattern | `CanInstallModification`/`InstallModification` (gate-then-do, `out string reason`, atomic `TryConsumeBill`) | `CanInstallArmorGrade`/`InstallArmorGrade`, `ReforgeArmorPlate` mirror it exactly. |
| Refund precedent | `UninstallModification` (50% of scrap line, `max(1, amount/2)`) | Replacing a fitted grade refunds 50% of the *old* grade's scrap line. |
| Save round-trip | `CaptureState`/`RestoreState` (SystemTextJson deep copy) | New record fields persist free; legacy absence → defaults. |
| Provider-unset pattern | `Garage` property itself; `RoomPowerProvider` (Plan 210 followups) | `ArmorMaterialQualitySource` delegate: unset/false → Standard/1000 neutral stamp, never recalculated. |
| Foundry D6 handoff | `TryGetLatestMaterialQualityAny` | Provider target; first consumer of `ArmorModifierBp`. |
| Estimate/runtime parity | `Start` samples profile once; `Estimate` reads same field | Mitigation decorates the field pre-sampling → parity by construction; pinned by test T-15. |
| Selftest harness | `HostCli.VehicleGarage.cs` `Check(...)` | +8 gates (§18). |
| Panel bind pattern | `IBindablePanel`, `RefreshView` read-model rendering, words+numbers a11y | One ARMOR section (§15). |
| Validator pattern | per-catalog `Validate*` public statics on `CatalogIntegrityValidator` | `ValidateVehicleArmorGradeCatalog`. |

---

# 6. Proposed Architecture

**One sentence:** the armor grade is one more authored decoration the single vehicle owner (`VehicleGarageSystem`) applies through its two existing mutation/decoration points, with state carried as additive fields on the record it already persists.

No new system, manager, registry, ledger, save section, RNG stream, or event bus is created. The only new *file* in Core is a data-types-and-loader file (`VehicleArmorGradeCatalog.cs`), matching the one-catalog-one-loader convention already used by `VehicleGarageCatalog.cs`.

## 6.1 The damage-mitigation formula (complete, with derivation)

**Definitions.** All persistent quantities are integer permille (‰, base 1000). Floats appear only as catalog effect constants, exactly like the existing `VehicleModificationEffects` floats, and never enter save state.

- `M` = grade `mitigation_permille` ∈ [0, 250] authored; code clamps to [0, 400] hard ceiling (validator enforces authored ≤ 250; the ceiling exists so a bad catalog can never approach immunity even if validation is bypassed).
- `A` = grade `wear_absorption_permille` ∈ [0, 350] (validator), hard ceiling 500.
- `P` = plate integrity pool (permille), stamped at install ∈ [50%, 130%] of grade base pool.
- `I` = current plate integrity ∈ [0, P].

**Channel 1 — breakdown mitigation (in `DecorateProfile`, after the mod block):**

```
if (profile.breakdownChancePerTick > 0f and I > 0 and grade is fitted):
    profile.breakdownChancePerTick = clamp(profile.breakdownChancePerTick * (1000 − M)/1000f, 0f, 1f)
```

Residual risk is always ≥ (1000 − 400)/1000 = 60% of the pre-armor value at the hard ceiling, ≥ 75% at authored max — **never zero, never immunity** (run-flat precedent: `RunFlatTireEngine.ApplyHazard` floors loss at 1; here the floor is structural: a multiplier < 1 of a positive chance). When `I = 0` (depleted plate) or no grade is fitted, the line is a no-op: stock parity is *byte-identical*, not approximately equal.

Worked numbers (base formula `clamp((100−condition)/100 * 0.15 * gearMult, 0, 1)`, gear = standard ⇒ 1.0):

| Condition | Base p/tick | 11-tick sortie expectation `1−(1−p)^11` | G1 (M=100) | G2 (M=150) | G3 (M=200) | G4 (M=250) |
|---|---|---|---|---|---|---|
| 90/100 | 0.015 | 15.3% | 13.9% | 13.2% | 12.4% | 11.6% |
| 60/100 | 0.060 | 49.4% | 45.7% | 43.0% | 40.3% | 37.6% |
| 30/100 | 0.105 | 70.5% | 66.8% | 63.0% | 59.2% | 55.4% |

Each tier buys roughly 3–4 percentage points of survival per sortie at mid-condition — felt, not transformative; the ladder stays inside the deep-dive's "25% increment per tier on the vehicle-flagged risk slice."

**Channel 2 — chassis wear absorption (in `RecordTripWear`):**

```
baseWear = round(distanceKm * 2 * wearMult)          // unchanged
if (I > 0 and grade fitted):
    absorbed = min(I, round(baseWear * A / 1000))
    chassis += baseWear − absorbed
    I −= absorbed
else:
    chassis += baseWear                               // legacy path, byte-identical
engine    += round(baseWear * 0.8)                    // unchanged — armor plates the hull, not the powertrain
transmission += round(baseWear * 0.9)                 // unchanged
```

Design notes: (a) absorption applies to **chassis only** — plates are hull armor; engine fouling and transmission wear remain Plan-50-exact, which keeps three independent immobilization paths honest and the diff to two lines. (b) Hull retains the majority share: even at authored max A=350, chassis takes ≥65% — the deep-dive's signed constraint ("hull wear — the immobilization authority — always retains the majority share"). (c) The split is computed from the *mod-adjusted* `baseWear`, so `vmod_reinforced_bullbar`'s wear multiplier and armor absorption compose multiplicatively in a pinned order (mods → absorption).

Worked numbers (60 km medium route, `wearMult = 1.0` ⇒ `baseWear = 120`):

| Grade | Absorbed/trip | Chassis/trip | Trips to plate depletion | Chassis after 5 trips (vs unarmored 600) |
|---|---|---|---|---|
| none | 0 | 120 | — | 600 |
| G1 (A=200, P=100) | 24 | 96 | 4 full (96), 5th partial 4 | 500 |
| G2 (A=250, P=140) | 30 | 90 | 4 full (120), 5th partial 20 | 470 |
| G3 (A=300, P=180) | 36 | 84 | 5 full (180) | 420 |
| G4 (A=350, P=240) | 42 | 78 | 5 full (210), 6th partial 30 | 390 |

G4 defers 210 permille of chassis wear across five trips ≈ 1.75 extra medium trips before catastrophic — a real maintenance economy, bounded by the pool, renewable only through the re-forge cost loop.

**Stamp formula (install time only):**

```
P = clamp(round(basePool * armorModifierBp/1000 * purityBp/1000), ceil(basePool*0.5), ceil(basePool*1.3))
purityBp: Poor 850 / Standard 1000 / High 1100 / Exceptional 1200     // inside Plan 213's 850–1200 purity band
```

Provider unset, or foundry has no provenance ⇒ `armorModifierBp = 1000, purityBp = 1000` (neutral; Plan 213's "old saves = Standard/unknown, NEVER recalculated" precedent). The stamp is written to the record once and **never recomputed** on restore, decoration, or wear — determinism and reload-safety by construction.

## 6.2 The grade ladder (design values; [grounded] = repo data, [proposed] = design awaiting authored confirmation)

| Tier | id | Display | M (‰) | A (‰) | Pool (‰) | Speed Δ | Fuel × | Install cost (verified ids) | Labor | Re-forge cost |
|---|---|---|---|---|---|---|---|---|---|---|
| 0 | `grade_0_stock` | Stock Plating | 0 | 0 | 0 | 0 | 1.00 | — | 0 | — |
| 1 | `grade_1_scrap_plate` | Scrap Plate | 100 | 200 | 100 | −0.02 | 1.02 | 4× `scrap_metal` + 2× `mechanical_parts` | 240 | 2× `scrap_metal` |
| 2 | `grade_2_sheet_plate` | Sheet Plate | 150 | 250 | 140 | −0.04 | 1.05 | 6× `scrap_metal` + 2× `mechanical_parts` + 1× `item_metallurgy_iron_ingot` | 300 | 3× `scrap_metal` + 1× `mechanical_parts` |
| 3 | `grade_3_composite_plate` | Composite Plate | 200 | 300 | 180 | −0.07 | 1.08 | 8× `scrap_metal` + 3× `mechanical_parts` + 2× `item_metallurgy_iron_ingot` + 1× `item_ebpvd_ceramic_target_ingot` | 420 | 4× `scrap_metal` + 1× `mechanical_parts` |
| 4 | `grade_4_alloyed_heavy_plate` | Alloyed Heavy Plate | 250 | 350 | 240 | −0.10 | 1.12 | 12× `scrap_metal` + 4× `mechanical_parts` + 3× `item_metallurgy_iron_ingot` + 2× `item_ebpvd_ceramic_target_ingot` | 600 | 6× `scrap_metal` + 2× `mechanical_parts` |

Derivation and balance rationale:

- **Mitigation staircase (100/150/200/250).** Direct restatement of the deep-dive's −1000/−1500/−2000/−2500 bp ladder in garage permille. Equal 50‰ steps keep every upgrade a *felt* but not transformative improvement; the 250 cap leaves ≥75% residual risk (never immunity). G1 at 100‰ is deliberately meaningful so the foundry→garage loop matters from the first unlock.
- **Absorption vs pool coupling.** Pool ÷ absorption-per-trip ≈ 4–6 medium trips for every grade — plate lifetime is *flat across tiers*; higher tiers protect more per trip, not longer. That keeps the re-forge cadence (a scrap sink) constant while protection quality scales, separating the two scarcity axes exactly as the deep-dive intends ("rare metals the license, scrap the running cost").
- **Mass penalty.** G4's −0.10 speed / ×1.12 fuel matches `vmod_lead_lined_cab`'s mass profile (the heaviest existing fitted object) — internally consistent. A quad (speed 1.3) wearing G4 drops to ≈1.17 effective speed pre-mod; armor is a logistics *choice*, not a default.
- **Economy grounding.** Materials are the low-elasticity commodity class [grounded: `commodity_baselines.json` materials band 600–1600‰, 0.4× response] — 12-scrap G4 demand cannot be price-spiked away in a day, and sustained demand slowly walks the index toward the 1600‰ ceiling: the long-run brake on armor proliferation. Scrap is common-tier 1–3 per urban scavenging trip [grounded: `scavenging_tables.json`] ⇒ G4's scrap line ≈ 4–12 trips of yield: armor routes through the exploration loop instead of short-circuiting it. The rare ingot is a CVD-process-adjacent industrial item, so top-tier plates compete with the diamond/metallurgy program for the same foundry pipeline [proposed interlock, per deep-dive B.3].
- **Re-forge = 50% of the scrap line, no rare re-input** [deep-dive B.3]: rare inputs are shaping costs consumed at first forge; upkeep is scrap-only.

## 6.3 Why single-grade, not multi-plate (rejected alternative, recorded)

The deep-dive proposed up to 3 plates with a total-mitigation cap. Rejected because: (1) the eight-plan program (2026-09-19, the newer signed authority) specifies one `armorGradeId`; (2) single-grade makes stacking exploits *impossible by construction* instead of validator-policed; (3) the cap rule reduced the multi-plate design to "one grade plus bookkeeping" — extra save fields, extra failure modes, zero extra decisions; (4) the panel stays a one-line truth. If a future package wants layered plating, the migration path is clean (record field → list), and this plan's Failure Modes table already documents the swap semantics that future work must preserve.

---

# 7. Ownership Matrix

| Concern | Owner (exact) | Notes |
|---|---|---|
| Armor grade catalog content | `Assets/StreamingAssets/Data/vehicle_armor_grades.json` | Authoritative; snake_case; `schema_version: 1`. |
| Catalog DTO + strict load | `Assets/Ashfall.Core/Expeditions/VehicleArmorGradeCatalog.cs` (new) | Data types only; collected-error loader; **not a system**. |
| Armor state, resolution, commands, mitigation, absorption | `VehicleGarageSystem` (extended in place) | The one vehicle authority. No fork, no sibling. |
| Catalog cross-validation | `CatalogIntegrityValidator.ValidateVehicleArmorGradeCatalog` (new static) | Wired into the data-integrity aggregation like the difficulty-preset rule. |
| Material-quality input | `SilentFoundrySystem.TryGetLatestMaterialQualityAny` via new `VehicleGarageSystem.ArmorMaterialQualitySource` delegate | Read-only query; provider-unset neutral; garage never calls foundry directly. |
| Inventory mutation | canonical `Inventory` through `IPlayerInventoryPort` | Atomic `TryConsumeBill`; refunds via `TryProduce`. |
| Dispatch/trip consumption | existing `ExpeditionHostSession` garage seam | **No edits** (§4.7 refinement 4). |
| Persistence | existing `VehicleGarageSaveStore` + `vehicle_garage` section | Additive record fields; no new store/section/file. |
| Day tick | existing `Main.CampaignOwners.cs` recovery tick | Unchanged; armor has no day tick. |
| Panel | `src/UI/VehicleGaragePanel.cs` (extended) | Presentation only; no gameplay math. |
| Selftest | `src/Host/HostCli.VehicleGarage.cs` (extended) | +8 gates. |
| Architecture map / catalog registry / docs index | owning generators + `--check` | Regenerated, never hand-edited. |
| Ledger/claims | foreman/integrator only | `WORKTREE_OWNERSHIP.md`, `INTEGRATION_PLANS.md` rows. |

Ambiguity check: every state/behavior above has exactly one owner. The one historical ambiguity — "who owns vehicle damage mitigation?" (nobody, per D6) — is **resolved by this plan**: `VehicleGarageSystem`, bounded to the two channels in §6.1. Combat damage to vehicles remains unowned and is therefore out of scope, not implicitly created.

---

# 8. Data Flow

**Dispatch (consumption path):**
`ExpeditionHostSession.BuildProfile(vehicleId)` → `ExpeditionVehicleSystem.CreateExpeditionProfile` (base fields from condition/gear) → `Garage.DecorateProfile(profile)` → [mod block: cargo/speed/fuel — unchanged] → **[armor block: speed/fuel deltas; breakdownChance × (1000−M)/1000 when I>0]** → profile returned → `ExpeditionSystem.Estimate` (UI preview) and `ExpeditionSystem.Start` (samples into `exp.vehicleBreakdownChancePerTick`) consume the *same decorated field* → estimate ≡ runtime (share-the-sampled-value).

**Trip wear (consumption path):**
dispatch prep → `RecordTripWear(vehicleId, distanceKm)` → `wearMult` from mods (unchanged) → `baseWear` (unchanged) → **chassis/plate split (new)** → engine/transmission lines (unchanged) → immobilization check (unchanged; a plate can hit 0 the same trip the chassis immobilizes — both truths recorded independently).

**Install (command path):**
panel REFIT/UPGRADE → `CanInstallArmorGrade(vehicleId, gradeId, inventory, out reason)` (vehicle real, grade known, not stock-on-stock no-op, not immobilized, terrain compatible, bill sufficient) → `InstallArmorGrade` → atomic `TryConsumeBill(new bill)` → 50% scrap refund of *old* grade if any → stamp integrity from provider (once) → record updated → panel re-renders from `GetArmorProfile`.

**Re-forge (command path):**
panel RE-FORGE → `ReforgeArmorPlate(vehicleId, inventory, out reason)` (grade fitted, I < P, bill sufficient) → consume re-forge bill → `I = P` (pool restored to the *stamped* max, not the catalog base — provenance is kept).

**Save/restore:**
`SaveVehicleGarage` (existing) → `CaptureState` deep-copies records including armor fields → checksumed envelope → disk. Load → `RestoreState` → fields present or defaulted → **no recomputation** of stamps, no events replayed.

**Foundry quality (read path):**
install command → `ArmorMaterialQualitySource?.Invoke(out q)` → host bridge → `SilentFoundrySystem.TryGetLatestMaterialQualityAny(out q)` → stamp. The garage holds no foundry reference; the delegate is the entire coupling.

---

# 9. State Model

## 9.1 Record delta (before → after)

```
VehicleCustomizationRecord (current):
  vehicleId, installedSlots{}, chassisStressPermille, engineFoulingPermille,
  transmissionWearPermille, isImmobilized, immobilizedReason

VehicleCustomizationRecord (after — additive, [Serializable], defaults shown):
+ armorGradeId = ""                    // empty ⇒ grade_0_stock (legacy parity)
+ armorIntegrityPermille = 0
+ armorIntegrityMaxPermille = 0
+ armorMaterialProfileId = ""          // provenance stamp; "" = none/unknown
+ armorPurity = "standard"             // string, matching record style; ∈ {poor, standard, high, exceptional}
```

No changes to `VehicleGarageState`, `VehicleRecoveryMission`, or the store envelope.

## 9.2 Invariants (each pinned by a named test in §18)

1. `armorGradeId` is empty or resolves in the loaded armor catalog; empty ⇔ stock ⇔ `armorIntegrityMaxPermille == 0`.
2. `0 ≤ armorIntegrityPermille ≤ armorIntegrityMaxPermille`; max ∈ [50%, 130%] of the grade's authored base pool.
3. `armorIntegrityPermille == 0` ⇒ effective mitigation 0 and effective absorption 0 (depleted plates are inert, not removed).
4. `armorIntegrityMaxPermille` is set exactly once per install (the stamp); restore, wear, and re-forge never recompute it (re-forge restores *to* it).
5. Armor state never flips `isImmobilized`; immobilization logic is byte-identical when armor is absent.
6. Legacy records (no armor fields in JSON) deserialize to the defaults ⇒ stock profile ⇒ `DecorateProfile`/`RecordTripWear` outputs byte-identical to pre-package behavior.

## 9.3 Plate lifecycle

```
none ──install──► fitted (I = P stamped) ──trip wear──► worn (0 < I < P)
fitted ──trip wear──► depleted (I = 0; M = A = 0; grade stays fitted)
depleted ──re-forge──► fitted (I = P)
any fitted ──install other grade──► fitted-new (old scrap-line 50% refunded, old plate gone)
```

Fitting is a garage-state command only — never mid-expedition (dispatch-time state; the deep-dive's "no hot-swap" constraint holds trivially because expedition runtime never calls install).

---

# 10. API/Contracts

All additions to `VehicleGarageSystem` (Core, engine-free). Existing public members are untouched.

```csharp
// Catalog binding (beside LoadCatalog)
public void LoadArmorCatalog(VehicleArmorGradeCatalog catalog);   // null → ArgumentNullException; replaces prior
public bool HasArmorGrade(string gradeId);
public VehicleArmorGradeDefinition? GetArmorGrade(string gradeId);
public IReadOnlyDictionary<string, VehicleArmorGradeDefinition> GetAllArmorGrades();

// Material-quality provider (optional; bridge to foundry). Unset or false ⇒ neutral stamp.
public delegate bool TryGetArmorMaterialQuality(out FoundryMaterialQuality quality);
public TryGetArmorMaterialQuality? ArmorMaterialQualitySource { get; set; }

// Read model — total function: unknown/unrecorded/stock vehicles all resolve to a
// profile whose IsDefault is true and whose effective values are 0.
public sealed class VehicleArmorProfile            // new, in VehicleArmorGradeCatalog.cs? NO — beside the system
{
    public string GradeId;                 // resolved id (default row id when stock)
    public string DisplayName;
    public int Tier;
    public bool IsDefault;                 // stock
    public int MitigationPermille;         // authored (effective == authored when I>0 else 0)
    public int WearAbsorptionPermille;
    public int IntegrityPermille;          // I
    public int IntegrityMaxPermille;       // P
    public string MaterialProfileId;       // provenance, may be ""
    public string Purity;                  // provenance, "standard" default
    public string ConditionBand;           // "nominal" | "worn" | "critical" | "depleted" | "none"
}
public VehicleArmorProfile GetArmorProfile(string vehicleId);
public static string ArmorConditionBand(int integrityPermille, int maxPermille);
// bands: max==0 → "none"; i==0 → "depleted"; i ≥ 75% → "nominal"; i ≥ 25% → "worn"; else "critical"

// Commands (gate-then-do, reason strings, atomic inventory)
public bool CanInstallArmorGrade(string vehicleId, string gradeId, IPlayerInventoryPort? inventory, out string reason);
public bool InstallArmorGrade(string vehicleId, string gradeId, IPlayerInventoryPort? inventory, out string reason);
public bool ReforgeArmorPlate(string vehicleId, IPlayerInventoryPort? inventory, out string reason);
```

Contract details:

- `CanInstallArmorGrade` refuses, in order, with specific reasons: empty vehicle id ("Invalid vehicle ID."); unknown grade ("Armor grade '<id>' not found in catalog."); stock grade id ("Stock plating is the absence of armor — nothing to install."); no change ("Vehicle already wears grade '<id>'."); immobilized ("Vehicle '<id>' is immobilized and cannot be modified until recovered." — same wording family as the mod gate); terrain incompatible ("Grade '<id>' cannot be fitted to '<terrain>' terrain vehicles."); insufficient materials ("Insufficient material: requires <n> of '<item_id>'.").
- `InstallArmorGrade` re-checks the gate, consumes the new bill via one `TryConsumeBill`, then refunds 50% of the *old* grade's scrap lines (`max(1, amount/2)`) via `TryProduce`, then stamps (grade id, P from §6.1, I = P, provenance from provider-or-neutral). Ordering guarantee: if the bill fails, nothing mutates (atomicity identical to `InstallModification`).
- `ReforgeArmorPlate` refuses: no record, stock grade, `I == P` ("Plate is already at full integrity."), insufficient re-forge bill. On success: consume bill; `I = P`.
- `GetEffectiveMitigationPermille(vehicleId)` / `GetEffectiveWearAbsorptionPermille(vehicleId)` private helpers return 0 when `I == 0`, stock, or unrecorded — the single point where depletion gates effect (avoids the "dead plate still protects" bug class).
- **Terrain gate (deliberate, evidence-based):** grade rows author `compatible_terrain_types` ⊆ {`road`, `rough`, `coastal`} — the vocabulary `vehicles.json` actually contains. The gate is enforced by the command (unlike the mods' unenforced `compatible_vehicle_tags`, §2.1), and the validator enforces vocabulary membership. The garage learns terrain through a new optional resolver the host sets once: `public Func<string, string?>? VehicleTerrainResolver { get; set; }` → `_expeditions.Vehicles.GetDefinition(vehicleId)?.terrain_type`; unset resolver ⇒ gate passes (provider-unset neutrality, so pure-Core tests need no vehicle catalog).

**Decoration diff (exact, inside `DecorateProfile` after the three existing lines):**

```csharp
// CF-P6 armor grade decoration (additive; no-op at stock/depleted ⇒ legacy byte-identical)
profile.speedMultiplier = Math.Max(0.01f, profile.speedMultiplier * (1f + GetArmorSpeedDelta(vehicleId)));
profile.fuelPerTravelTick = Math.Max(0f, profile.fuelPerTravelTick * GetArmorFuelMultiplier(vehicleId));
if (profile.breakdownChancePerTick > 0f)
{
    int m = GetEffectiveMitigationPermille(vehicleId);   // 0 when stock/depleted
    if (m > 0)
        profile.breakdownChancePerTick = Math.Clamp(profile.breakdownChancePerTick * (1000 - m) / 1000f, 0f, 1f);
}
```

**Wear diff (exact, inside `RecordTripWear`, replacing only the chassis line):**

```csharp
int absorbed = 0;
int a = GetEffectiveWearAbsorptionPermille(vehicleId);   // 0 when stock/depleted
if (a > 0)
{
    var rec = record; // already fetched
    absorbed = Math.Min(rec.armorIntegrityPermille, (int)Math.Round(baseWear * a / 1000.0));
    rec.armorIntegrityPermille -= absorbed;
}
record.chassisStressPermille = Math.Clamp(record.chassisStressPermille + baseWear - absorbed, 0, 1000);
// engine/transmission lines unchanged, computed from baseWear as today
```

Note the decoration applies grade speed/fuel even when the plate is depleted (mass doesn't disappear when the plate is dead) but mitigation/absorption gate on `I > 0`. That asymmetry is deliberate, documented on the panel ("depleted plating still weighs"), and pinned by test T-18.

---

# 11. Data Changes

## 11.1 New file: `Assets/StreamingAssets/Data/vehicle_armor_grades.json` (complete content)

```json
{
  "schema_version": 1,
  "default_grade_id": "grade_0_stock",
  "grades": [
    {
      "id": "grade_0_stock",
      "display_name": "Stock Plating",
      "description": "Factory sheet metal and hope. Every vehicle leaves the bay with this; it stops weather, not consequences.",
      "tier": 0,
      "is_default": true,
      "mitigation_permille": 0,
      "wear_absorption_permille": 0,
      "integrity_pool_permille": 0,
      "speed_multiplier_delta": 0.0,
      "fuel_consumption_multiplier": 1.0,
      "compatible_terrain_types": ["road", "rough", "coastal"],
      "install_cost": [],
      "install_labor_ticks": 0,
      "reforge_cost": [],
      "tags": ["armor", "default"]
    },
    {
      "id": "grade_1_scrap_plate",
      "display_name": "Scrap Plate",
      "description": "Door skins and road sign bolted over the soft spots. It rattles, it drags a little, and once in a while it eats the hit that would have killed the axle.",
      "tier": 1,
      "is_default": false,
      "mitigation_permille": 100,
      "wear_absorption_permille": 200,
      "integrity_pool_permille": 100,
      "speed_multiplier_delta": -0.02,
      "fuel_consumption_multiplier": 1.02,
      "compatible_terrain_types": ["road", "rough", "coastal"],
      "install_cost": [
        { "item_id": "scrap_metal", "amount": 4 },
        { "item_id": "mechanical_parts", "amount": 2 }
      ],
      "install_labor_ticks": 240,
      "reforge_cost": [
        { "item_id": "scrap_metal", "amount": 2 }
      ],
      "tags": ["armor", "plate", "tier_1"]
    },
    {
      "id": "grade_2_sheet_plate",
      "display_name": "Sheet Plate",
      "description": "Cut sheet, welded seams, a real work order in the log. The crew stops flinching at every stone strike.",
      "tier": 2,
      "is_default": false,
      "mitigation_permille": 150,
      "wear_absorption_permille": 250,
      "integrity_pool_permille": 140,
      "speed_multiplier_delta": -0.04,
      "fuel_consumption_multiplier": 1.05,
      "compatible_terrain_types": ["road", "rough", "coastal"],
      "install_cost": [
        { "item_id": "scrap_metal", "amount": 6 },
        { "item_id": "mechanical_parts", "amount": 2 },
        { "item_id": "item_metallurgy_iron_ingot", "amount": 1 }
      ],
      "install_labor_ticks": 300,
      "reforge_cost": [
        { "item_id": "scrap_metal", "amount": 3 },
        { "item_id": "mechanical_parts", "amount": 1 }
      ],
      "tags": ["armor", "plate", "tier_2"]
    },
    {
      "id": "grade_3_composite_plate",
      "display_name": "Composite Plate",
      "description": "Layered stock from the foundry's good days, ceramic face over iron back. Heavy enough to need its own line in the fuel ledger.",
      "tier": 3,
      "is_default": false,
      "mitigation_permille": 200,
      "wear_absorption_permille": 300,
      "integrity_pool_permille": 180,
      "speed_multiplier_delta": -0.07,
      "fuel_consumption_multiplier": 1.08,
      "compatible_terrain_types": ["road", "rough"],
      "install_cost": [
        { "item_id": "scrap_metal", "amount": 8 },
        { "item_id": "mechanical_parts", "amount": 3 },
        { "item_id": "item_metallurgy_iron_ingot", "amount": 2 },
        { "item_id": "item_ebpvd_ceramic_target_ingot", "amount": 1 }
      ],
      "install_labor_ticks": 420,
      "reforge_cost": [
        { "item_id": "scrap_metal", "amount": 4 },
        { "item_id": "mechanical_parts", "amount": 1 }
      ],
      "tags": ["armor", "plate", "composite", "tier_3"]
    },
    {
      "id": "grade_4_alloyed_heavy_plate",
      "display_name": "Alloyed Heavy Plate",
      "description": "The heaviest set the bay can hang on a frame. Slow, thirsty, and the closest thing this world has to a promise.",
      "tier": 4,
      "is_default": false,
      "mitigation_permille": 250,
      "wear_absorption_permille": 350,
      "integrity_pool_permille": 240,
      "speed_multiplier_delta": -0.10,
      "fuel_consumption_multiplier": 1.12,
      "compatible_terrain_types": ["road", "rough"],
      "install_cost": [
        { "item_id": "scrap_metal", "amount": 12 },
        { "item_id": "mechanical_parts", "amount": 4 },
        { "item_id": "item_metallurgy_iron_ingot", "amount": 3 },
        { "item_id": "item_ebpvd_ceramic_target_ingot", "amount": 2 }
      ],
      "install_labor_ticks": 600,
      "reforge_cost": [
        { "item_id": "scrap_metal", "amount": 6 },
        { "item_id": "mechanical_parts", "amount": 2 }
      ],
      "tags": ["armor", "plate", "heavy", "tier_4"]
    }
  ]
}
```

## 11.2 Field documentation

| Field | Type | Meaning / rules |
|---|---|---|
| `schema_version` | int | Must be `1` (validator). |
| `default_grade_id` | string | Must resolve to a row with `is_default: true`; exactly one such row. |
| `id` | string | Unique, snake_case, `grade_` prefix, tier suffix matches `tier`. |
| `display_name` / `description` | string | Non-empty for non-default rows; tone per §16. |
| `tier` | int | Unique, 0–4, contiguous. |
| `is_default` | bool | Exactly one `true`; the default row must be all-neutral (M=0, A=0, pool=0, speed=0, fuel=1.0, empty costs). |
| `mitigation_permille` | int | [0, 250] authored; code ceiling 400. Non-decreasing in tier (validator). |
| `wear_absorption_permille` | int | [0, 350]; code ceiling 500. Non-decreasing in tier. |
| `integrity_pool_permille` | int | [0, 400]. Non-decreasing in tier. |
| `speed_multiplier_delta` | float | [−0.5, 0]; non-increasing protection costs mass (may be 0 only on default). |
| `fuel_consumption_multiplier` | float | [1.0, 2.0] for non-default; exactly 1.0 on default. |
| `compatible_terrain_types` | string[] | ⊆ {`road`,`rough`,`coastal`}; empty = fits nothing (refused). |
| `install_cost` / `reforge_cost` | {item_id, amount}[] | item ids resolve in `items.json`; amounts ≥ 1; re-forge total scrap ≤ install scrap (economy guard); re-forge may not contain the rare id (`item_ebpvd_ceramic_target_ingot`) — license/running-cost separation. |
| `install_labor_ticks` | int | ≥ 0; informational until a labor consumer binds (recorded honestly; matches `install_labor_ticks` on mods, which also has no live consumer today). |
| `tags` | string[] | Free vocabulary, `armor` required on all rows. |

## 11.3 Registry/generator updates (mandatory, with reasons)

- `CatalogIntegrityValidator.cs`: add `ValidateVehicleArmorGradeCatalog` enforcing every §11.2 rule, errors naming catalog/row/field (the established message shape).
- `ContentUtilizationScanner.cs`: three rows for `vehicle_armor_grades.json` (known files; loader map → `VehicleArmorGradeCatalogLoader`; consumer map → `VehicleGarageSystem`) — otherwise the utilization gate flags the new authority as dead content.
- `scripts/ci/generate-architecture-map.py`: `vehicle_garage` node gains the catalog and the new test file; regenerate `ARCHITECTURE_TEST_MAP.md` and pass `--check`.
- `generate-catalog-registry.py` / `generate-docs-index.py`: regenerate `docs/data/CATALOG_REGISTRY.md` and `docs/INDEX.md`; pass `--check`.
- `docs/ci/SELFTEST_MANIFEST.json`: description string updated to mention armor gates (hygiene; the manifest's own contract tests keep passing since the id/flag/timeout are unchanged).

---

# 12. Save/Load

- **No new save section, store, file, or envelope.** Armor state lives on `VehicleCustomizationRecord`, which `VehicleGarageState.vehicleRecords` already carries through `VehicleGarageSaveStore` (`vehicle_garage_save.json`, section `vehicle_garage`, checksumed, `allowLegacyBareState: false`).
- **Mechanism:** `CaptureState`/`RestoreState` round-trip via `SystemTextJsonSerializer`; new `[Serializable]` record fields serialize with zero codec changes (verified mechanism in §2.1 — this is how `installedSlots` persists today).
- **Legacy saves** (pre-package envelopes): armor fields absent ⇒ System.Text.Json defaults (`""`, `0`, `"standard"`) ⇒ invariant 9.2.1 maps to stock ⇒ decoration and wear are byte-identical to legacy. No migration, no recompute, no fabrication — the Plans 146–149 additive-field precedent.
- **Frozen-shape compliance:** shape grows additively at the leaf record; envelope version untouched; section-count gate (185) unchanged; `docs/saves/SAVE_STORE_CONTRACT_MATRIX.md` row unchanged (same store).
- **Restore semantics:** stamps are data, not recomputed (§6.1). `RestoreState` after `CaptureState` mid-wear resumes the exact integrity — pinned by round-trip test T-22 and legacy-parity test T-23.
- **Rollback of saves:** if the package is reverted, saves written *with* armor fields load under old code with the extra JSON properties ignored (System.Text.Json default behavior) — vehicles silently return to stock; recorded as acceptable data-drop on rollback, stated in §23.

---

# 13. Determinism

- **Zero new RNG.** No `System.Random`, no new `ISeededRng` consumption, no fork of the existing `vehicle_garage` stream. Every armor number is a deterministic function of (catalog, record state, stamped provenance). This matches `DecorateProfile`'s documented "zero RNG" and keeps the garage's seeded stream untouched (stream-order stability ⇒ replay compatibility).
- **Command-time sampling only.** The foundry quality provider is invoked exactly once per install command — a player-visible, ledgered action — never on tick, estimate, decorate, or restore. Same save ⇒ same stamps ⇒ same outcomes.
- **Integer permille in persistence** (integrity ints; purity as string); floats exist only as catalog constants (same class as existing mod effect floats) and in the transient profile. No wall-clock, no hash-order iteration (`StringComparer.Ordinal` dictionaries, matching existing style).
- **Culture invariance:** serialization stays inside the existing SystemTextJson round-trip; no `ToString("0.##")` in state.
- **Proof shape:** fingerprint test T-24 runs two identical systems through the same op sequence (install → trips → depletion → re-forge → save/restore) and asserts serialized state equality; the existing `Plan50VehicleGarageIntegrationTests` save round-trip remains green unchanged.

---

# 14. System/Event Wiring

- **No new day events, no `DayEventVocabulary` additions.** The garage is a command-driven system today: install/service/recovery-complete surface through panel feedback strings, not the briefing (verified: `VehicleGarageSystem` declares no events; `Main.Plans50_53.cs` wires none). Armor keeps that parity — installing a plate is a garage transaction, not a chronicle entry. (If a future package wants a "plate saved the axle" digest line, it must reuse the absorption facts at the trip-wear call site through the existing event seam — out of scope here, noted so no builder improvises one.)
- **Recovery tick unchanged:** `AdvanceRecoveries(24)` per campaign day; armor neither accelerates nor gates recovery.
- **Dispatch gate unchanged:** immobilized refusal stays in `PrepareVehicleForDispatch`; armor adds no new dispatch gate (a depleted plate is *not* a dispatch blocker — the vehicle is merely unprotected; that decision is deliberate and panel-visible).
- **Provider bridge:** one additive assignment in host setup (§15); unbinds cleanly on session teardown with the garage itself.

---

# 15. Godot Integration

**`src/Main.Plans50_53.cs` (additive ~6 lines in `EnsureVehicleGarage`):** load `vehicle_armor_grades.json` from `_dataDir` beside the mod catalog (same try/catch + `GD.PrintErr` pattern), call `_vehicleGarage.LoadArmorCatalog(...)`; set `_vehicleGarage.VehicleTerrainResolver = id => …` once the expedition vehicle system is reachable (bind order: `SetupExpeditions` precedes garage use on every route — verify at implementation; if order is inverted, set the resolver lazily in `EnsureVehicleGaragePanel` instead, still provider-unset-safe).

**`src/Main.ExpansionHub.cs` (additive ~3 lines, at the end of the foundry/hub setup where `_silentFoundry` is known):**

```csharp
// CF-P6: garage armor stamps consume the Plan 213 material-quality handoff (read-only).
if (_vehicleGarage != null)
    _vehicleGarage.ArmorMaterialQualitySource =
        (out FoundryMaterialQuality q) => _silentFoundry?.Engine != null
            && _silentFoundry.Engine.TryGetLatestMaterialQualityAny(out q);
```

Provider unset (foundry session never built) ⇒ neutral stamps; the garage never references the foundry assembly directly beyond the existing `Ashfall.Core.Foundry` types.

**`src/UI/VehicleGaragePanel.cs` (one new section, presentation only):** between MAINTENANCE and RECOVERY OPERATIONS:

- Section header `ARMOR PLATING`.
- Truth line from `GetArmorProfile`: e.g. `"Composite Plate — mitigation 200‰, absorption 300‰, integrity 96/180 — worn (material_machine_steel, high)"`; stock: `"Stock Plating — no armor fitted."`; depleted: `"… integrity 0/180 — DEPLETED. Mitigation inactive; mass penalty remains. Re-forge to restore."` All words + numbers, never color-only (a11y rule; `ConditionBand` supplies the word).
- Grade `OptionButton` (fitted grades only, sorted by tier) + cost preview line (same pattern as the mod selector) + `FIT PLATE` / `RE-FORGE` buttons forwarding to the Core commands; disabled states mirror `CanInstallArmorGrade`/integrity-full logic via the same reasons shown as feedback (`SetResult` reuse).
- No new status-rail card (fitted armor is visible in the section; rail stays at four cards).

**`src/Host/HostCli.VehicleGarage.cs`:** +8 `Check` gates (§18.2), reusing the existing `EnsureMaterials` helper.

**Routing/registry:** none — the panel already has its route; no new panel, no new CLI command (the armor gates ride the existing `--vehicle-garage-selftest`).

---

# 16. Narrative/Content Integration

- **Tone:** restrained, human, bureaucratic, scarcity-aware (DESIGN.md). Descriptions in §11.1 are authored to that register: no heroics ("the closest thing this world has to a promise"), no real countries/wars/people, no copied text or UI.
- **Names** come from the deep-dive ladder (Scrap / Sheet / Composite / Alloyed Heavy) — workshop vocabulary, not military designations. `grade_0_stock`'s "Factory sheet metal and hope" sets the baseline the ladder climbs from.
- **Panel language** is ledger-speak ("permille", "integrity", "re-forge") consistent with the garage's existing "SERVICEABLE / IMMOBILIZED" register; condition bands (`nominal/worn/critical/depleted`) are words before numbers per the a11y gate.
- **No new diegetic text surfaces** (no radio, quests, or chronicle lines) — §14 records the deliberate silence.

---

# 17. Failure Modes

Complete matrix over {grade state} × {vehicle state} × {event}. "Guard" names the §18 test or validator rule.

| # | Grade | Vehicle state | Event | Required behavior | Guard |
|---|---|---|---|---|---|
| 1 | none (legacy) | any | decorate | byte-identical profile (parity 1.0) | T-10, soak |
| 2 | none | any | trip wear | byte-identical wear lines | T-10, soak |
| 3 | none | any | save→load | legacy envelope loads; stock profile; no fabricated armor | T-23 |
| 4 | G1–G4 | serviceable | install | gate passes; bill consumed atomically; stamp once; refund 50% scrap of replaced grade | T-11, T-14 |
| 5 | G1–G4 | serviceable | install, insufficient materials | refusal; zero mutation (no partial stamp, no partial consume) | T-11 |
| 6 | any | immobilized | install/re-forge | refusal with immobilized reason (same family as mod gate) | T-12 |
| 7 | any | in recovery (mission active) | install/re-forge | refusal (vehicle is immobilized by definition) | T-12 |
| 8 | G3/G4 | coastal vehicle (`vehicle_salvage_dredger`) | install | refusal: terrain incompatible (authored rows exclude `coastal`) | T-13, V-rules |
| 9 | any | serviceable, resolver unset | install | terrain gate passes (provider-unset neutrality); documented | T-13 |
| 10 | G1–G4, I>0 | any | decorate | breakdown ×(1000−M)/1000; speed/fuel deltas applied after mods | T-15, T-25 |
| 11 | G1–G4, I>0 | any | estimate vs runtime | same decorated field sampled once ⇒ identical | T-15 |
| 12 | G4 | condition→0 (worst case) | decorate | residual risk ≥ 75% of base; never 0 | T-15 bounds |
| 13 | G1–G4, I>0 | serviceable | trip wear | chassis += baseWear − absorbed; I -= absorbed; engine/transmission from full baseWear | T-16, T-17 |
| 14 | G1–G4, I>0 | serviceable | trip wear larger than I | absorbed clamps at I; remainder hits chassis; I = 0 exactly | T-16 |
| 15 | any fitted | any | trip wear immobilizes chassis | immobilized + reason as legacy; armor state preserved (plate neither prevents nor causes immobilization) | T-16, FM-restate |
| 16 | depleted (I=0) | any | decorate | mitigation 0; speed/fuel penalties persist | T-18 |
| 17 | depleted | serviceable | re-forge | bill consumed; I restored to stamped P (not catalog base) | T-19, T-21 |
| 18 | depleted | serviceable | re-forge at I=P | refusal ("already at full integrity") | T-19 |
| 19 | fitted | any | replace with other grade | old plate destroyed (50% scrap refund), new stamp; integrity does NOT carry over | T-14 |
| 20 | fitted | any | save mid-wear → load | exact I/P/provenance restored; no recompute | T-22, T-24 |
| 21 | fitted | foundry never ran | install | neutral stamp (1000/1000 ⇒ P = base pool) | T-20 |
| 22 | fitted | provider returns Poor/Exceptional | install | P scales within [50%, 130%] clamps; mitigation unchanged | T-20 |
| 23 | fitted | provider muted after install | restore | stamped P persists (never recalculated) | T-21 |
| 24 | any | any | catalog row invalid (bounds/ids/ladder) | data-integrity selftest FAILS with catalog/row/field named | V-rules |
| 25 | any | naval/crawler profile | decorate | garage has no record for those ids ⇒ no-op (existing guard) | T-26 |
| 26 | any | unknown vehicle id | GetArmorProfile | default profile (total function); install refuses | T-26 |
| 27 | any | immobilized→recovered | `CompleteRecoveryMission` | wear clamped to 800 as legacy; armor untouched (I/P unchanged) | T-16 extension |
| 28 | G4 + `vmod_lead_lined_cab` + bullbar | serviceable | decorate/wear | all compose in pinned order (mods → armor); no double-count of mitigation vs radiation (separate channels) | T-25 |
| 29 | any | any | package reverted with armor-saved file | extra JSON fields ignored; vehicles revert to stock; load succeeds | §23 note |
| 30 | any | panel unbound | refresh | no crash; section hidden/neutral (IBindablePanel lifecycle) | panel lifecycle gate |

---

# 18. Test Strategy

Per `TEST_POLICY.md`: focused targets only, new file runs alone first, `bash scripts/run_test.sh <path>` (180 s cap), no full-suite by default; aggregation allowed only for the homogeneous validator rows.

## 18.1 New suite `Ashfall.Core.Tests/Expeditions/Plan213VehicleArmorGradeTests.cs` (runs alone first)

Catalog/loader (strict, collected errors — aggregate-able rows with per-row messages):

- **T-01** Real catalog loads: 5 rows, default resolves, tiers 0–4 unique/contiguous.
- **T-02** Default row neutrality (M=0, A=0, pool=0, costs empty, fuel 1.0).
- **T-03..T-09** Validator negatives (one theory row each, named errors): duplicate id; mitigation > 250; absorption > 350; unknown item id in cost; unknown terrain type; non-monotone ladder; re-forge scrap > install scrap; re-forge contains rare id; missing/extra default row.

Core behavior (independent cases — save/load, determinism, mutation, and state-transition cases are never aggregated):

- **T-10** Legacy parity: no armor ⇒ `DecorateProfile` output and `RecordTripWear` deltas byte-identical to a pre-armor fixture run (goldens computed in-test from the same base inputs).
- **T-11** Install: gate reasons ordered; atomic consume (bill all-or-nothing — poison one line, assert zero mutation); record stamped (grade id, I = P, provenance fields).
- **T-12** Immobilized refusal (install and re-forge), reason text pinned.
- **T-13** Terrain gate: dredger (coastal) refuses G3; resolver unset ⇒ pass-through.
- **T-14** Replace: G1→G3 consumes G3 bill, refunds 50% of G1 scrap (`max(1, 4/2)=2`), integrity resets to G3 stamp (no carry-over).
- **T-15** Mitigation: breakdownChancePerTick scaled exactly by (1000−M)/1000; never 0 for any base in (0,1]; estimate path (`ExpeditionSystem.Estimate`) and start path sample the same decorated value (parity pin).
- **T-16** Absorption: chassis split exact (worked table §6.1: 60 km ⇒ G1 absorbs 24, chassis +96); oversize trip clamps absorption at I; immobilization semantics unchanged when chassis crosses 1000 mid-split.
- **T-17** Engine/transmission byte-identical with and without armor (computed from full `baseWear`).
- **T-18** Depletion: drive I to 0 ⇒ mitigation 0 and absorption 0 while speed/fuel penalties persist; grade remains fitted.
- **T-19** Re-forge: consumes re-forge bill, restores I to stamped P; refusal at full integrity; refusal at stock.
- **T-20** Provider: unset ⇒ P = base; Poor (800 bp armor profile × 850 purity) and Exceptional (1200 × 1200) stamps land inside [50%, 130%] clamps; mitigation identical across stamps.
- **T-21** No-recompute: stamp with provider; mute provider; `CaptureState`→`RestoreState`; P unchanged.
- **T-22** Round-trip: armor fields survive `CaptureState`/`RestoreState` exactly (mid-wear I, provenance strings).
- **T-23** Legacy envelope: hand-authored JSON state without armor fields ⇒ defaults ⇒ stock parity (frozen-shape proof).
- **T-24** Determinism fingerprint: two systems, same seed, same op script (install G2 → 3 trips → depletion → re-forge → save) ⇒ identical serialized state strings.
- **T-25** Composition order: bullbar + G2 ⇒ wear computed from mod-adjusted baseWear then split; lead cab + G2 ⇒ radiation permille untouched by mitigation (channel separation).
- **T-26** Totality: `GetArmorProfile` on unknown/naval-style ids ⇒ default profile, `IsDefault == true`; install on unknown id refuses.

## 18.2 Selftest extension (`--vehicle-garage-selftest`, 19 → 27 gates)

`armor_catalog_loaded` (real file, 5 rows) · `armor_install_g1` · `armor_stamp_neutral_without_foundry` · `armor_mitigation_bounded` (decorated breakdown < base, > 0) · `armor_wear_absorption` (chassis reduced, I consumed by exact amount) · `armor_depleted_then_reforge` · `armor_legacy_parity` (fresh garage: profile + wear identical with catalog loaded but nothing fitted) · `armor_save_roundtrip`. Manifest description updated (§11.3); timeout unchanged (gates are all in-memory + two panel calls).

## 18.3 Regression and gates (per phase, never full-suite by default)

- `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/Plan213VehicleArmorGradeTests.cs` (alone, first).
- `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/VehicleGarageSystemTests.cs` and `.../Plan50VehicleGarageIntegrationTests.cs` — unchanged, must stay 6/6 and 5/5 (proof the extension is additive).
- `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/Plans50_53_SharedIntegrationTests.cs` (real-catalog reader).
- `godot --headless --path . -- --vehicle-garage-selftest` (27/27), `--data-integrity-selftest` (PASS, new catalog walked), `--catalog-boot-preflight` (clean), `--panel-bind-lifecycle-selftest`, ui-a11y selftest.
- `dotnet build Ashfall.csproj` 0/0; generator `--check` trio (architecture map, catalog registry, docs index).

## 18.4 Balance soak (P5; one focused test, not a harness)

Seeded 30-day loop in the new suite: per day, dispatch one medium route per grade cohort (stock/G1/G2/G4) against a deterministic condition schedule; assert: (a) stock cohort fingerprint equals a no-catalog control run (parity); (b) per-cohort cumulative chassis wear strictly ordered stock > G1 > G2 > G4 at every day boundary (monotonic, bounded); (c) every integrity value ∈ [0, P] (no underflow/overflow); (d) breakdown counts ordered and no cohort at zero; (e) re-forge cadence ≈ pool ÷ per-trip absorption ±1 (the §6.2 flat-lifetime claim, falsified if wrong). Deterministic by construction (zero RNG in armor); runtime well under the 180 s cap.

---

# 19. Dependency-Ordered Phases

- **P0 — Premise note + claim (this document; governance only).** Record the claim row `claim-cf-p6-vehicle-armor-grades-2026-09-19` in `WORKTREE_OWNERSHIP.md` with the §20 exact paths; cite this file as the premise note correcting D6. *Gate:* claim row exists; foreman acknowledgement. *No code.*
- **P1 — Catalog + loader + validator + registry rows.** §11 JSON; `VehicleArmorGradeCatalog.cs`; `ValidateVehicleArmorGradeCatalog` wired into the data-integrity aggregation; `ContentUtilizationScanner` rows; T-01…T-09. *Depends on:* P0. *Gate:* new test file alone green; `--data-integrity-selftest` PASS; content-utilization PASS; build 0/0. **No behavior changes yet** — the catalog is loaded by nothing outside tests.
- **P2 — Core state + resolution + commands + decoration/wear + persistence proofs.** Record fields; `LoadArmorCatalog`/read model/provider/resolver; `Can/Install/Reforge`; the two method diffs (§10); T-10…T-26. *Depends on:* P1 (types). *Gate:* armor suite green; the three legacy garage suites green untouched (additive proof).
- **P3 — Host wiring.** Catalog load in `EnsureVehicleGarage`; terrain resolver; foundry bridge in expansion-hub setup. No `ExpeditionHostSession` edits (§4.7). *Depends on:* P2. *Gate:* build 0/0; save round-trip through the real store (selftest gate `armor_save_roundtrip` passes pre-extension via a temporary probe or lands with P4 — implementer's call, recorded in handoff).
- **P4 — Panel + selftest extension.** §15 UI; +8 selftest gates; manifest description. *Depends on:* P3 (provider visible end-to-end). *Gate:* `--vehicle-garage-selftest` 27/27; panel-bind-lifecycle PASS; ui-a11y PASS.
- **P5 — Soak + closeout.** §18.4 soak; ledger row in `INTEGRATION_PLANS.md` (package DONE with evidence); regenerated `--check` docs; closeout note appended to this file's header (Status: INTEGRATED with command evidence). *Depends on:* P4. *Gate:* soak green; all P1–P4 gates re-run green in one focused window.

Rollback boundaries align with phases (§23); P1 and P2 are independently revertible, P3–P5 are revertible as a set.

---

# 20. File Impact Map

| File | Action | Reason | Risk |
|---|---|---|---|
| `Assets/StreamingAssets/Data/vehicle_armor_grades.json` | CREATE | authoritative grade catalog | low |
| `Assets/Ashfall.Core/Expeditions/VehicleArmorGradeCatalog.cs` | CREATE | DTOs + strict loader (data types only) | low |
| `Assets/Ashfall.Core/Expeditions/VehicleGarageSystem.cs` | MODIFY (additive: record fields, ~6 methods, 2 method-body diffs) | the owner extends its own seams | medium (shared wear/decoration methods — mitigated by legacy-parity pins T-10/T-15/T-17) |
| `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` | MODIFY (one new public static + aggregation hook) | catalog validation | low (pattern-precedented) |
| `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` | MODIFY (3 rows) | dead-content gate compliance | low |
| `src/Main.Plans50_53.cs` | MODIFY (~6 additive lines) | armor catalog load + terrain resolver | low |
| `src/Main.ExpansionHub.cs` | MODIFY (~3 additive lines) | foundry quality bridge | low |
| `src/UI/VehicleGaragePanel.cs` | MODIFY (one ARMOR section + selector/buttons) | truthful presentation of existing commands | low |
| `src/Host/HostCli.VehicleGarage.cs` | MODIFY (+8 gates) | selftest extension | low |
| `Ashfall.Core.Tests/Expeditions/Plan213VehicleArmorGradeTests.cs` | CREATE | new contracts (alone first) | low |
| `docs/ci/SELFTEST_MANIFEST.json` | MODIFY (description text) | honest manifest | low |
| `scripts/ci/generate-architecture-map.py` | MODIFY (vehicle_garage node rows) | map truth | low |
| `docs/architecture/ARCHITECTURE_TEST_MAP.md`, `docs/data/CATALOG_REGISTRY.md`, `docs/INDEX.md` | REGENERATE via owning scripts | generated, never hand-edited | low |
| `WORKTREE_OWNERSHIP.md`, `INTEGRATION_PLANS.md` | MODIFY (claim + completion rows — foreman/integrator only) | governance | low |
| **Explicitly untouched:** `src/Host/ExpeditionHostSession.cs` (§4.7), `ExpeditionSystem.cs`, `ExpeditionVehicleSystem.cs`, `vehicles.json`, `vehicle_modifications.json`, `VehicleGarageSaveStore.cs`, `Main.CampaignOwners.cs`, all foundry files (read-only consumer of an existing query), `SaveSectionRegistry.cs` | NO-CHANGE | seams already suffice | — |

---

# 21. Risks

| Risk | Class | Evidence | Mitigation |
|---|---|---|---|
| Behavior drift in shared `RecordTripWear`/`DecorateProfile` | HIGH | both methods are on the dispatch hot path | diffs limited to the §10 blocks; legacy-parity pins T-10/T-15/T-17 prove byte-identity at stock; P2 gate runs all three garage suites |
| Estimate/runtime divergence | HIGH | repo history treats this as a named failure mode (weather/amputation precedents) | mitigation decorates the single sampled field; T-15 parity pin |
| Immunity illusion / balance runaway | MEDIUM | run-flat precedent: "reduced, never eliminated" | authored cap 250‰ + code ceiling 400‰; residual-risk bound test; soak monotonicity |
| Stacking exploit via replace/refund | MEDIUM | refund-on-replace creates a scrap loop if mispriced | refund is 50% of the *old* scrap line only; net-negative loop by construction; validator forbids re-forge ≥ install scrap |
| Foundry coupling creep | MEDIUM | first garage→foundry touch | coupling is one optional delegate over an existing read-only query; provider-unset neutrality tested (T-20) |
| Save drift / migration debt | LOW | additive-leaf pattern (Plans 146–149) proven in this codebase | T-22/T-23; no envelope change |
| Terrain gate novelty | LOW | vehicles.json has no tags; resolver is new surface | resolver optional with neutral fallback; vocabulary validator-enforced; failure mode #8/#9 covered |
| Generated-doc drift | LOW | three `--check` gates exist | P5 re-runs them; never hand-edit generated files |
| Scope creep into combat armor | MEDIUM | C-integration docs fantasize mortar→vehicle armor | §22 hard boundary; §4.4 evidence that no live route exists; any future combat route must be its own signed package |

---

# 22. Out of Scope

- Any combat damage model for vehicles (no live combat→vehicle damage route exists; `DefenseSystem` remains the combat authority untouched).
- Multi-plate stacking / partial plate coverage (rejected, §6.3).
- Radiation protection changes (owned by protection-slot mod effects; channel separation pinned by T-25).
- Armored crawler, naval, rail, aviation armor (separate authorities/profiles; garage records never exist for those ids — failure mode #25).
- New item ids, market/economy tuning, scavenging-table changes (the ladder consumes existing ids only).
- Encounter-chance modification, expedition event content, day-event vocabulary additions (§14).
- A labor consumer for `install_labor_ticks` (field authored for honesty/parity with mods; no consumer today, none added).
- Unity, in all forms.

---

# 23. Rollback Strategy

- **P1 (catalog/loader/validator):** delete the three artifacts + scanner rows; zero behavior shipped. Independent revert.
- **P2 (Core):** revert the single commit; legacy-parity tests (T-10/T-23) prove pre/post equivalence at stock; saves written during P2 load under old code with armor fields ignored (System.Text.Json), vehicles reverting to stock — an accepted, documented data-drop (no corruption path: no checksum/envelope change ever occurs).
- **P3–P5 (host/panel/selftest/governance):** revert as a set; the provider bridge deletes cleanly (delegate unset ⇒ neutral); panel section deletes cleanly (presentation only); selftest returns to 19 gates.
- No phase writes to shared integrator seams (`ExpeditionHostSession`, save registry, orchestrator), so rollback never collides with concurrent claims.

---

# 24. Definition of Done

| # | Outcome | Proof |
|---|---|---|
| 1 | Premise corrected on the record | claim row + this document cited from `WORKTREE_OWNERSHIP.md`; D6 row annotated as condition-false in the next ledger touch |
| 2 | 4 authored tiers + neutral default load, validate, cross-reference | T-01…T-09; `--data-integrity-selftest` PASS; `--catalog-boot-preflight` clean; content-utilization PASS |
| 3 | One owner extended, nothing duplicated | file map honored; no new system/save section/RNG stream (source-grepable) |
| 4 | Mitigation bounded, monotonic, never zero | T-15, soak (§18.4) |
| 5 | Wear absorption exact; hull majority preserved; plates sacrificial | T-16…T-19, worked-table agreement |
| 6 | Foundry handoff consumed read-only, neutral without provenance | T-20/T-21 |
| 7 | Legacy saves byte-identical in behavior | T-10/T-23; garage suites 6/6 + 5/5 untouched |
| 8 | Estimate ≡ runtime | T-15 parity pin |
| 9 | Panel truthful, words+numbers | P4 gates: selftest 27/27, panel lifecycle, ui-a11y |
| 10 | Determinism preserved | T-24 fingerprint; no RNG added |
| 11 | Governance complete | ledger DONE row with command evidence; `--check` docs in sync; closeout stamped on this file |

---

# 25. Implementation Handoff

Builder: execute P0→P5 strictly in order; claim exact §20 paths before editing; run §18.3 focused commands only; preserve unrelated dirty worktree state (the repo currently carries untracked Seal-steps artifacts and one docs/plans file — leave them); report per `AI_AGENT_WORKFLOW.md` with commands/results/limitations.

## MUST PRESERVE

- `VehicleGarageSystem` as the single vehicle authority; `DefenseSystem` as the combat authority; `Inventory` as the only item ledger.
- Plan 50 wear semantics: immobilization thresholds (1000), service costs, recovery clamp-to-800, engine/transmission wear formulas — byte-identical at stock.
- `DecorateProfile`'s zero-RNG, read-only-over-state contract; the estimate/runtime share-the-sampled-value parity.
- The `vehicle_garage` save envelope, section count, and checksum store shape; legacy save byte-parity (T-23).
- The existing 19 selftest gates and all existing garage tests, unmodified and green.
- Panel a11y (words never color-only), bind/unbind lifecycle, keyboard/controller close behavior.
- The foundry's determinism: garage reads quality; it never mutates foundry state.

## MUST ADD

- `vehicle_armor_grades.json` (5 rows: 4 tiers + neutral default) with every §11.2 validator rule enforced.
- `VehicleArmorGradeCatalog.cs` loader types; `VehicleGarageSystem` additive armor surface (§10 signatures); record fields with legacy defaults.
- The two §10 method diffs (decoration block; chassis/plate split) and nothing else in those methods.
- `ArmorMaterialQualitySource` + `VehicleTerrainResolver` optional providers with unset-neutral behavior; the expansion-hub bridge.
- Panel ARMOR PLATING section with `ConditionBand` words, cost preview, FIT PLATE / RE-FORGE commands.
- 8 selftest gates; `Plan213VehicleArmorGradeTests.cs` (T-01…T-26 + soak); scanner/map/registry/manifest updates; claim and ledger rows.

## MUST NOT DO

- No new Core system, manager, registry, save section/store, event bus, or RNG stream; no `System.Random`.
- No multi-plate stacking, no mitigation > 250‰ authored (400‰ hard ceiling), no immunity paths, no armor-on-immobilized installs.
- No edits to `ExpeditionHostSession.cs`, `ExpeditionSystem.cs`, `ExpeditionVehicleSystem.cs`, `vehicles.json`, `vehicle_modifications.json`, save infrastructure, or foundry internals.
- No radiation-protection coupling, no encounter/combat math, no day-event fabrication, no new item ids.
- No full-suite runs by default; no hand-editing generated docs; no reviving the "no vehicle owner exists" premise; no Unity.

## VERIFY WITH

- `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/Plan213VehicleArmorGradeTests.cs` (alone first, then with):
- `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/VehicleGarageSystemTests.cs` · `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/Plan50VehicleGarageIntegrationTests.cs` · `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/Plans50_53_SharedIntegrationTests.cs`
- `godot --headless --path . -- --vehicle-garage-selftest` (27/27) · `--data-integrity-selftest` · `--catalog-boot-preflight` · `--panel-bind-lifecycle-selftest`
- `dotnet build Ashfall.csproj` (0/0); `python3 scripts/ci/generate-architecture-map.py --check`; `generate-catalog-registry.py --check`; `generate-docs-index.py --check`

## FIRST SAFE IMPLEMENTATION STEP

P0, read-only: record the claim row `claim-cf-p6-vehicle-armor-grades-2026-09-19` in `WORKTREE_OWNERSHIP.md` (exact §20 paths, this document as premise note), then re-run `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/Plan50VehicleGarageIntegrationTests.cs` and `godot --headless --path . -- --vehicle-garage-selftest` to re-confirm the 5/5 and 19/19 baseline before any edit. If either baseline is not green on claim day, stop and report — the premise has moved.
