# C2[5] / Plan 21 — Baseline Condition-Authority Audit (Premise Corrections)

> Date: 2026-09-15. Evidence-first read-only sweep before any Plan 21 edit,
> per `AGENTS.md` rule 7 and `C2_planintegration[5].md` §5. The 20A prerequisite
> is satisfied (20A + 20B complete; Radiation 72/72, Shelter 588/588,
> Campaign 107/107, host build 0 errors, data-integrity PASS at time of sweep).

## 1. Plan premises that are STALE (already built in current source)

| Plan §1/§0 premise | Current source reality | Evidence |
|---|---|---|
| Worn-gear bridge hardcodes `DegradeRate = 0f` | **Repaired.** `Inventory.FillWornGear` projects `DegradeRate = Math.Clamp(Item.GetEffectiveDegradeRate(), 0, 100)` and binds `ConditionSink = this` | `Assets/Ashfall.Core/Inventory/Inventory.cs:939-960` |
| Radiation mutates a temporary projection; inventory never changes | **Repaired.** `WornGear.Degrade` → `ConditionSink.RecordWear(SourceEquipped, loss, "radiation")` → canonical `EquippedItem.CurrentDurability` + `OnInventoryChanged` | `Inventory.cs` (`WornGear.Degrade`, `RecordWear`); `RadiationSystem.DegradeWornGear` (ambient>0 gate + per-tick identity dedup) |
| Protection never decays | **Repaired.** `EffectiveProtection() = RadProtection × DurabilityFraction()` — zero condition ⇒ zero protection is live semantics | `Inventory.cs:24-52` |
| Weapon condition duplicated / combat reads a second store | **Repaired (21B Phase D already done for weapons).** `WeaponEquipmentBridge` owns NO state; it projects `EquipmentConditionSystem` (one 0–100 condition per instance) into combat tokens and writes wear back through the authority | `Assets/Ashfall.Core/Combat/WeaponEquipmentBridge.cs` header |
| Combat condition never reaches decisions | **Partially repaired.** `ExpeditionSystem.Estimate` consumes `weaponReadiness` + `weaponJamRisk` inputs | `ExpeditionSystem.cs:103-121, 524+` |
| Sanctioned duplicate `WornGear` bridge in two namespaces | **Stale.** Exactly one `WornGear` class exists (`Inventory.WornGear`); `RadiationSystem` reaches it via a using-alias to the same type | grep: single `class WornGear` |

## 2. Verified condition-authority inventory (§5.2)

| Family | Authority field | Owner | Save | Wear producer | Repair producer | Readers |
|---|---|---|---|---|---|---|
| Protective gear (rad-protection equippables) | `EquippedItem.CurrentDurability` (0..`Item.durability`) | `Inventory` | inventory save (verify field in 21A Phase G) | radiation exposure via `WornGear.Degrade`→sink; bulk `DegradeEquippedGear` | maintenance paths TBD (pin in 21B Phase H) | radiation projection; `GetTotalRadProtection` |
| Weapons / tools / clothing / watercraft | `EquipmentInstance.condition` (0–100) | `EquipmentConditionSystem` (profiles, jam/break thresholds, maintenance jobs) | `equipment_condition` section | `UseItem` wear; combat via bridge write-back | `StartMaintenance` | `WeaponEquipmentBridge` → combat; expedition `weaponReadiness`/`weaponJamRisk` |
| Vehicles | breakdown-risk model (no simple condition field found) | `VehicleGarageSystem` | vehicle save | travel/breakdown | existing | expedition estimate `breakdownRiskPerTick` |
| Shelter infrastructure | structurally-owned condition (pipes etc. under different names) | shelter/thermal systems | shelter save sections | use/environment | infrastructure | shelter UI — **stays disjoint per §3.2** |

## 3. REAL gaps (revised scope)

- **P1 — default wear rates are Core constants.** `ItemDefinitions.GetEffectiveDegradeRate()`:
  authored `degradeRate > 0` wins (data ✓), but the *defaults* (Face 1.0 / Body 0.5 / other
  0.25 per hour) are hardcoded in Core. §3.4/§9.1 want defaults data-authored or explicitly
  documented as canonical domain defaults.
- **P2 — no `gear_failed` transition semantics for protective gear.** Zero condition ⇒ zero
  protection is live, but no exactly-once failure event/audio exists (ECS has
  `OnItemBroken` for its families; inventory protective gear has nothing). §11.
- **P3 — no remaining-life projection surface.** No Core estimate function for
  "hours left at current exposure" and no inventory/expedition display of it. §12.
- **P4 — per-tick projection allocation.** `SurvivorsHostSession.CollectWornGear` allocates
  a fresh list per context build; §14 wants buffer reuse without stale-state bugs.
- **P5 — ECS↔Inventory protective overlap unverified.** `EquipmentConditionSystem`
  registers families incl. Clothing; whether any protective item is tracked in BOTH
  stores (Inventory durability AND ECS condition) must be pinned by the 21B Phase A
  matrix before any migration. Weapon side is already canonical.
- **P6 — expedition estimate lacks protective inputs.** `ExpeditionEstimate` has
  weapon/fuel/cargo/risk but no party protection, projected dose, projected gear wear,
  or mid-route failure prediction. 21C Phase A.
- **P7 — wear scaling with zone contamination is absent** (wear = rate × hours ×
  survivor-level `hazmatDegradeMultiplier`; the weather melt hook
  `BlackRainHazmatMeltMultiplier` exists but zone-intensity scaling is not modeled).
  §9.3 marks this optional ("may scale") — decision required, documented either way.

## 4. Revised execution order

Per plan §4 (`21A → 21B → 21C`) with corrected scope:

1. **21A remainder (tests-first):** pin the repaired chain with regression tests
   (projection is read-only, sink is authority, zero-durability ⇒ zero protection,
   save round-trip), then close P1 (data-authored defaults decision), P2 (failure
   transition event), P3 (Core remaining-life estimate + UI), P4 (buffer reuse).
2. **21B:** Phase A matrix first (P5 pin), then read/write convergence only where a
   genuine duplicate exists; weapons are DONE via the bridge — do not re-migrate.
3. **21C:** P6 (estimate protection/dose/wear inputs + warning/recommendation).

## 5. Pre-existing conditions (not owned by this package)

- In-flight worktree work (echo system, extensive uncommitted modifications) —
  `verify-fast.sh` whitespace gate already red on those files before this package.

---

## 6. 21A remainder execution (2026-09-15) — COMPLETE

Tests-first (`Ashfall.Core.Tests/Inventory/Plan21ProtectiveWearTests.cs`, 16/16):

**Repaired-chain pins (passing, regression-guarded):**
exposure wears the canonical `EquippedItem` through the sink; direct projection writes have
no authority; projection `Degrade` writes through the sink; zero durability ⇒ zero
protection; authored `degradeRate` overrides the family default (decision **D1**: the Core
Face/Body/other defaults remain documented domain fallbacks — authored data always wins;
full data authoring flagged to the foreman if desired); save round-trip preserves worn
durability; the `hazmatDegradeMultiplier` hook scales wear.

**New behavior (P2/P3/P7/P4):**
- **P2:** `Inventory.OnProtectiveGearFailed(item, cause)` — exactly-once transition event
  (>0 → 0) on the wear authority; `DegradeEquippedGear` now routes through `RecordWear`
  (one mutation API, cause attribution, failure semantics — no parallel direct writes).
- **P3:** `Inventory.TryEstimateWeakestProtectiveLife(multiplier, out estimate)` — Core
  remaining-life estimate (same data rate the simulation consumes); session
  `GetWeakestProtectiveLife()` (bound multiplier); `RadiationDetailPanel` shows the
  weakest protective item with condition + `~N h at current exposure` (×melt shown when
  active). Source-gated: panel carries no wear arithmetic.
- **P7:** `SurvivorsHostSession.BindHazmatWearMultiplier` wired in `Main.Survivors` to
  `WeatherSystem.HazmatDegradeMultiplier` — black-rain ×5 melt is live (was defined but
  never bound).
- **P4:** `CollectWornGear` reuses a cleared `_wornGearBuffer` (synchronous read-projection
  consumption; sink owns mutation) — no per-tick list allocation.

**Verification:** Inventory 64/64 (incl. pre-existing `ProtectiveGearDegradationTests`
8/8 — no regression from the RecordWear refactor) · journey 1/1 · Radiation 72/72 ·
host build 0 errors · data-integrity PASS · panel lifecycle PASS · ui-a11y PASS ·
survivors selftest PASS · triad PASS.

**Remaining for 21B/21C:** P5 (ECS↔Inventory overlap pin via the Phase A ownership
matrix — weapons already canonical via `WeaponEquipmentBridge`, do not re-migrate) and
P6 (expedition estimate protective inputs: party protection, projected dose/wear,
mid-route failure — 21C Phase A).

---

## 7. 21C execution (2026-09-15) — COMPLETE

**P6 closed** (`Ashfall.Core.Tests/Expeditions/Plan21EstimateProtectiveInputsTests.cs` 9/9,
`Ashfall.Core.Tests/Inventory/Plan21EndToEndJourneyTests.cs` 2/2):

- **Core:** `ExpeditionProtectiveInputs` (resolver ambient, canonical gear protection,
  weakest-gear data rate/durability, wear multiplier, hours-per-tick cadence) +
  `ExpeditionEstimate` additive fields (`partyProtection`, `unprotectedCount`,
  `projectedDosePerHour/Total`, `projectedTripHours`, `projectedGearWear`,
  `protectiveLifeHours`, `predictsMidRouteFailure`). The dose projection runs through
  `RadiationSystem.ComputeExposurePerHour` — no parallel arithmetic (plan §3.8).
  Null inputs ⇒ legacy estimate byte-identical.
- **Host:** `ExpeditionHostSession.SetEstimateProtectiveInputs` hook (same pattern as
  route modifiers) wired in `Main.Expeditions` to
  `SurvivorsHostSession.BuildProtectiveEstimateInputs` (exposure resolver + gear
  projection + bound melt multiplier).
- **Panel:** estimate line appends `dose ~X mSv (protection N / NO WORKING PROTECTION)`
  and `GEAR FAILS MID-ROUTE (~life h < trip h)` — display-only, agency preserved
  (plan §34/§35). Source-gated against panel-side dose math.
- **Journey (§42):** fresh mask → estimate predicts → 24 h exposure → exactly-once
  failure event → post-failure hourly dose rises (20 → 30 mSv/h, protection truly gone)
  → spare equipped (authored replacement path) → estimate reflects restored readiness.
  Route-shortening variant: shorter trip drops the failure prediction and reduces
  projected dose/wear.

**Verification:** Expeditions 243/243 · Inventory 66/66 · Radiation 72/72 · host build 0 ·
data-integrity PASS · panel lifecycle PASS · ui-a11y PASS · survivors selftest PASS ·
triad PASS.

---

## 8. D1 data tranche + §28 200-day soak (2026-09-15, user-authorized window)

- **D1 closed as full data authority:** all six remaining equipable rad-protective
  items in `items.json` now carry explicit behavior-preserving `degradeRate`
  (Body 0.5: ash_ghillie, lead_shielded_sample_cask, sealed_lead_pig,
  protective_childs_coat, water_sample_contaminated; Face 1.0: lead_visor) —
  surgical one-line insertions, values identical to the derived defaults they
  replace. `item_mycelium_bricks` intentionally unauthored (not equipable →
  zero-rate by derivation). Every equipable protective item is now data-authored;
  the Core family defaults remain as fallback only (pinned by test).
  **Foreman flag:** `water_sample_contaminated` is `isEquipable: true` with
  radProtection 20 — looks like a data quirk (a carried sample as wearable
  protection); left untouched to preserve behavior, flagged for review.
- **§28 soak (3/3, `Inventory/Plan21LongCampaignSoakTests.cs`):** 200-day
  deterministic campaign at the authored gas_mask values (dur 100, rate 1.0/h,
  radProtection 30) in a constant 30 mSv/h zone with a 60-spare pool:
  - 48 masks consumed (exactly durability/rate lifespan — authored gear lifespan
    is meaningful and replacement is necessary),
  - no durability underflow; acute dose saturates; lifetime accrues through the
    decay-scaling protection (the effective-protection curve is load-bearing),
  - day-100 save/load swap through the real codec state: **continuous run ==
    interrupted run** (same failure count, same lifetime trajectory, durability
    continuity at the swap) — the plan §23/§46 migration guarantee by construction,
  - paired-run fingerprint identical.
- **Verification:** Inventory 69/69 · host build 0 · data-integrity PASS ·
  triad PASS · docs index 2136.
