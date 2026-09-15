# PLAN 20B — Shelter Shielding Authority Map (C2[4], Phase A inventory)

> Read-only reconnaissance for `C2_planintegration[4].md` §21. Every contributor
> below was verified in current source (2026-09-15). Package: `C2-PLAN20B-SHIELDING-MODEL`
> (claim `claim-c2-plan20b-shielding-2026-09-15`). No behavior change in this document.

## 1. Inventory matrix (§21)

| Contributor | Existing system (source) | Existing state | Persisted? | Current radiation effect | 20B model role |
|---|---|---|---|---|---|
| Ceiling | `Shelter/MaterialShieldingSystem.cs` — wood 0.10 / dirt 0.50 / concrete 0.80 / lead 0.99; weakest roofed ceiling governs | `roomId→WallMaterial`, `GetWeakestCeilingAttenuation()` | Yes (`MaterialShieldingSave` via shelter save) | **YES** — `SurvivorsHostSession.ExposureResolver.ShelterAttenuationProvider = () => Shelter.GetWeakestCeilingAttenuation()` | base structural attenuation (already wired) |
| Walls / structure | No single "wall integrity" owner found. Candidates: `ShelterFireHazardSystem.structuralDamage`, `SeismicDynamicsSystem` dampener integrity, `KineticStorageSystem.room_damage_per_failure`, `NuclearCoreLifecycleSystem.shieldingIntegrity` (nuclear core, room-scoped) | mixed | mixed | none on exposure | **pin the canonical shelter-condition owner before adding a wall term — do not invent a second damage state** |
| Air filter | `StartingLevel/StartingLevelSystem.cs` — `State.airFilterHealthPercent` (0–100), bands healthy ≥75 / degraded ≥50 / critical >0, `PreviewMaintainAirFilter` + `item_air_filter_hepa` / `scrap_mechanical` | health %, maintenance command | yes (StartingLevel state) | none | airborne reduction term; **warning threshold must read this same value** (§23.4) |
| Ventilation | `VentilationSystem.cs` — `exhaustFilterSaturation` 0–100 (higher = clogged), `ductIntegrity` 100, `mainDuctOpen`, `emergencyRecirculationMode`, per-room valves, `smokeSootLevel`, `carbonMonoxidePpm`, `ozonePpm` | full duct state | yes | none | air-exchange / ingress modifier |
| Decontamination | `DecontaminationSystem.cs` — `DecontaminationState.shelterContaminated`, `shelterContaminationLevel`, effluent tank/filter, protocol stages, `SafeReleaseShelterDelta = −0.05` | shelter contamination level + active case stages | yes | none on interior ambient (only survivor surface dust + dose-before bookkeeping) | interior contamination source reduction term |
| Airlock | `AirlockSecuritySystem.cs` — `AirlockSecurityState.blastDoorIntegrity` 100, `alertness` 100, incidents, quarantine | blast door / incident state | yes | none | ingress modifier (sealed vs breach/incident) |
| Radon | **Shelter-live:** `StartingLevel/StartingLevelState.radonLevelBqm3` (default 12 Bq/m³, maintained by `StartingLevelSystem` cyclic air handling; feeds `airHazardWarning` >30). Also `YearOfAsh/YearOfAshRadonSystem.cs` (indoor 120 Bq/m³ baseline, scrubber/fissures, its own alpha ledger) for the Year-of-Ash narrative layer | both persisted | **none** — neither fed `RadiationSystem` before 20B | internal source term from the shelter-live field (plan §26.1: not multiplied by outdoor shielding); Year-of-Ash radon remains its own narrative authority |
| Flooding / sump | `SumpFloodingSystem.cs` — per-room `waterLevelCm`/`maxWaterLevelCm`, `hasSumpPump`, `pumpCondition`, `pumpPowered`, `hasFloatValve`, `hasSandbagMitigation`, `isFlooded`, `equipmentDisabled`, `contaminationLevel` 0–1 | full sump state + contamination | yes (`sump_flooding` section) | none | contamination transport into interior model; drain/decon must reduce it |

## 2. One-authority notes

- `MaterialShieldingSystem.GetRadiationBleed(exteriorRads)` already expresses
  "exterior × (1 − weakest ceiling)" — 20B's interior model should **extend this
  semantics**, not create a parallel "shielding percent."
- `RadiationSystem` owns dose; `ShelterRadQuery` (seam already pinned by
  `ShelterRadQuerySeamTests`, 5/5) is the integration point. 20B replaces the
  internal computation behind `ExposureEnvironment.ToExposureContext`, callers unchanged.
- The shelter interior baseline (`DefaultShelterInteriorRadRate = 2.0` Core const)
  is currently hardcoded; 20B moves it into the shield data file.
- Decon already consumes `AirlockSecuritySystem` (subject routing) — no new bridge needed.

## 3. Data file (proposed)

`shelter_shielding.json` (schema_version 1): structural attenuation scaling,
filter efficiency bands, clog thresholds, ventilation ingress multipliers,
airlock ingress multiplier, decon reduction/duration, radon contribution scaling,
flooding contamination scaling. Validated through `CatalogIntegrityValidator`
(additive hook, §27.1). No balance values in `switch (…)`.

## 4. Open items for the 20B implementation session (evidence-first)

1. **Pin the canonical shelter structure-damage owner** (candidates §1 row 2);
   if none is authoritative, the structural term stays ceiling-only and is documented
   as such — do not create a competing integrity store.
2. Confirm which ventilation/materials state is already persisted vs transient, and
   restore order per §50.1 **before** recomputing interior state.
3. Balance sizing: interior base is 2.0 mSv/h and intact shelter currently computes
   to exactly 0 (`BALANCE_SIM_radiation_exposure_20A.md` F3/F4) — 20B contributions
   must be sized against those curves, and radon must give maintenance a real floor.
4. `ExpeditionSystem.camp.radiationExposure` inert field (20A G4) — retire or leave
   documented; no behavior change.

## 6. Implementation status (2026-09-15)

**DONE (core):** `shelter_shielding.json` data authority (14 validated coefficients);
`ShelterShieldingCatalog` + `ShelterShieldingModel` (`Assets/Ashfall.Core/Shelter/ShelterShieldingModel.cs`);
`ExposureEnvironment.InteriorRadQuery` seam + resolver `ShelterInteriorRadQuery`;
`SurvivorsHostSession.BindShelterShieldingModel` (+ breakdown uses the same query);
host bindings to every canonical owner in §1 (lazy providers; unbound ⇒ nominal ⇒
legacy byte-identical); integrity-gate hook; 21 focused tests (model composition,
bounds, determinism, catalog validation, end-to-end seam, warning-parity source gates).

**Recorded behavior deltas (model bound in the live host):**
- Intact shelter interior no longer computes exactly 0 — the shelter-live radon floor
  (12 Bq/m³ × 0.005 = 0.06 mSv/h ≈ 1.44/day) now applies (plan F4: maintenance needs a floor).
- Clogged filter / duct breach / saturation / recirculation / open-or-breached airlock /
  airlock incident raise interior ingress, amplified by weather only when such defects exist.
- Shelter contamination (decon), sump contamination, and radon add internal sources;
  an active decon cycle halves internal sources (bounded, data-authored).

**Structure term:** remains ceiling-only (no general shelter-condition owner exists —
fire `structuralDamage` is incident-scoped). No competing integrity store was created.

**Remaining (next 20B package):** §29 semantic day events for filter/decon/shielding
changes (`DayEventVocabulary` canonical kinds + parity gate), §28 weakest-contributor
shelter UI line, §30 60-day shielding balance sweep, save/load regression pins for the
newly-consumed contributors.

## 7. Remainder status (2026-09-15, second pass) — 20B COMPLETE

All four remainder items landed:

- **§29 day events:** `ShelterFacilitiesDayOwner` detects pre/post transitions and emits
  canonical kinds (`shelter_filter_degraded` with band + %, `shelter_decon_started`,
  `shelter_decon_completed`, `shelter_hatch_unsealed` with door state). The briefing builder
  renders tailored text (Warnings / new "Shelter" section); parity matrix updated; the
  source parity gate passes 2/2. Filter *replacement* feedback remains the existing
  maintenance-command path (no day tick observes it) — documented, not invented.
- **§28 UI:** `ShelterShieldingModel.GetBreakdown` names the largest single contributor
  (defect share of the penalty portion, internal sources, or `ceiling attenuation` when
  structure dominates); `SurvivorsHostSession.GetShieldingBreakdown`;
  `RadiationDetailPanel` Protection section shows `Interior radiation X mSv/h · weakest: Y`
  with the decon-active flag. One arithmetic path (`ComputeInteriorRad` delegates to
  `GetBreakdown`).
- **§30 sweep:** `Plan20BShieldingBalanceSweepTests` (12 cases) + balance-report addendum
  (findings F5–F8): weather only matters through defects; clogged filter < open airlock in
  storm < flooding; decon bounded; intact radon floor 1.44 mSv/day.
- **Save/load pins:** the model is a pure function of already-persisted owner state
  (StartingLevel filter+radon, Ventilation, AirlockSecurity, SumpFlooding, Decontamination)
  whose owners have existing round-trip coverage; no new save section was added and the
  shelter-shielding data gate runs inside the data-integrity selftest. Determinism proven
  by the paired-run fingerprint.

**Verification:** focused 21+12+5; Shelter 588/588; Campaign 107/107; Radiation 72/72;
parity gate 2/2; host build 0; data-integrity PASS; panel lifecycle PASS; triad PASS.
**Note:** the parity matrix gained descriptive rows for two pre-existing in-flight
worktree emitters (`echo_consequence_due`, `echo_surfaced`) that had been stale before
this package; their producing code was not touched.