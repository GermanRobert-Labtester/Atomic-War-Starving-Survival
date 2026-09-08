# Wildlife Trapping Integration Guarantees

This document records the architectural guarantees, validation bounds, determinism invariants, and catalog runtime authority established for the ASHFALL Wildlife Trapping subsystem across Tasks 5–8 of the Flagship Integration Plan.

---

## 1. Subsystem Overview & Architecture

Wildlife Trapping operates at the intersection of four core ASHFALL systems:
- **`Ashfall.Core.WildlifeTrappingSystem`**: Pure engine-agnostic deterministic simulation tracking trap sites, bait usage, daily interval ticks, durability depletion, and catch outcomes.
- **`Assets/StreamingAssets/Data/wildlife_trapping_catalog.json`**: Authoritative JSON catalog specifying trap definitions, prey species, and bait configurations.
- **`src/Host/WildlifeTrappingHostSession.cs`**: Thin Godot host adapter projecting trapping simulation state to player UI, dispatching disease contraction and radiation exposure through system delegates.
- **`Ashfall.Core.MedicalSystem` & `Ashfall.Core.RadiationSystem`**: Core downstream survival systems receiving disease and rad doses from butchered catches.

---

## 2. Disease Resolution Policy & Fallback Tier (Task 5)

When a captured animal is butchered, disease risk is evaluated and mapped to a concrete disease ID using a strict two-tier resolution policy:

### Resolution Precedence
1. **Explicit Catalog Override**: If `PreyDefinition.diseaseId` is defined and non-empty in `wildlife_trapping_catalog.json`, this ID is always used regardless of `diseaseRisk`.
   - `prey_mire_lurker` -> `disease_parasitic_worms` (risk 0.50)
   - `prey_irradiated_stag` -> `disease_chronic_wasting` (risk 0.60)
2. **Fallback Tier**: If `PreyDefinition.diseaseId` is `null` or empty, disease assignment falls back to risk thresholding:
   - `diseaseRisk <= 0.10f`: No disease contracted (`null`).
   - `diseaseRisk > 0.10f`: `disease_zoonotic_flu` (`PreyDefinition.FallbackDiseaseId`).

### Exact Boundary Guarantees
- `diseaseRisk = 0.1000f` (e.g., `prey_trench_weasel`) -> returns `null` (safe).
- `diseaseRisk = 0.1001f` -> returns `"disease_zoonotic_flu"` (infected).
- Once-per-butchery guarantee: `ResolveDiseaseId()` is executed exactly once per butchered animal. If disease check fails RNG, no disease is contracted. If disease check succeeds RNG, the resolved ID is passed to `MedicalSystem.ContractDisease()`.

---

## 3. Radiation Contamination Scale & Exposure Bounds (Task 6)

Every prey item yields an acute radiation dose upon butchery, representing environmental fallout bioaccumulation.

### Scale Invariants & Safety Bounds
- **Unit Scale**: 1 dose unit = 1 rad absorbed by the survivor.
- **Acute Threshold**: `RadiationSystem.AcuteThreshold = 80.0f` rads.
- **Prey Safety Bound**: Every authored prey species has `0 <= contaminationDose < 80.0f` rads. No single prey item can instantaneously trigger Acute Radiation Sickness (ARS).
- **Fallback Dose**: `PreyDefinition.FallbackContaminationDose = 2.0f` rads applied if a prey entry lacks explicit dose data.

### Authoritative Anchor Points
| Prey ID | Display Name | Contamination Dose (rads) | Fraction of Acute Limit |
|---|---|---|---|
| `prey_dune_fox` | Dune Fox | 5.0 | 6.25% |
| `prey_rad_hare` | Rad Hare | 15.0 | 18.75% |
| `prey_carrion_crow` | Carrion Crow | 20.0 | 25.0% |
| `prey_mole_rat` | Mole Rat | 22.0 | 27.5% |
| `prey_mire_lurker` | Mire Lurker | 35.0 | 43.75% |
| `prey_irradiated_stag` | Irradiated Stag | 40.0 | 50.0% |

### Cumulative Exposure Proof
- Multiple catches accumulate directly in the survivor's radiation pool.
- 4x `prey_carrion_crow` (4 x 20.0 rads = 80.0 rads) triggers `AcuteRadiationSickness` via `RadiationSystem.AddRadiation()`.

---

## 4. Deterministic RNG & Zero-Draw Invariants (Task 7)

Simulation determinism is strictly maintained via `ISeededRng` (xorshift64*):

### Zero-Draw Guarantees
1. **Broken Traps**: A trap with `isBroken == true` or `remainingDurability <= 0` consumes **0 random draws** during `CheckTraps()` / `TickDay()`.
2. **Absent Traps**: Sites with no trap deployed or an unpopulated simulation state consume **0 random draws**.
3. **Mixed-Set Equivalence**: Given identical PRNG state, a simulation containing $N$ healthy traps and $M$ broken traps consumes the exact same number of random draws and produces the identical outcome sequence as an isolated simulation containing only the $N$ healthy traps.
4. **Deterministic Break Schedule**: A trap with initial durability $D$ and check interval $I$ days breaks deterministically on day $1 + D \times I$.

---

## 5. Catalog Content Authority & Complete Census (Task 8)

The data authority is `Assets/StreamingAssets/Data/wildlife_trapping_catalog.json`.

### Catalog Inventory
- **Traps**: Exactly 10 trap types.
- **Prey**: Exactly 15 prey species.
- **Bait**: Exactly 6 bait types.

### Complete 15-Prey Authoritative Matrix
| ID | Name | Category | Disease Risk | Authored Disease | Resolved Disease | Contamination Dose |
|---|---|---|---|---|---|---|
| `prey_rad_hare` | Rad Hare | Small Mammal | 0.15 | *(none)* | `disease_zoonotic_flu` | 15 rads |
| `prey_mole_rat` | Mole Rat | Burrower | 0.35 | *(none)* | `disease_zoonotic_flu` | 22 rads |
| `prey_ash_grouse` | Ash Grouse | Avian | 0.08 | *(none)* | `null` (safe) | 8 rads |
| `prey_mutant_boar` | Mutant Boar | Large Game | 0.45 | *(none)* | `disease_zoonotic_flu` | 28 rads |
| `prey_carrion_crow` | Carrion Crow | Avian Scavenger | 0.25 | *(none)* | `disease_zoonotic_flu` | 20 rads |
| `prey_mire_lurker` | Mire Lurker | Amphibian | 0.50 | `disease_parasitic_worms` | `disease_parasitic_worms` | 35 rads |
| `prey_vault_rat` | Vault Rat | Vermin | 0.12 | *(none)* | `disease_zoonotic_flu` | 10 rads |
| `prey_dune_fox` | Dune Fox | Predator | 0.05 | *(none)* | `null` (safe) | 5 rads |
| `prey_cave_bat` | Cave Bat | Flyer | 0.40 | *(none)* | `disease_zoonotic_flu` | 18 rads |
| `prey_scavenger_dog` | Scavenger Dog | Pack Canine | 0.30 | *(none)* | `disease_zoonotic_flu` | 14 rads |
| `prey_spotted_skink` | Spotted Skink | Reptile | 0.02 | *(none)* | `null` (safe) | 4 rads |
| `prey_needle_badger` | Needle Badger | Defensive Burrower | 0.20 | *(none)* | `disease_zoonotic_flu` | 16 rads |
| `prey_dust_civet` | Dust Civet | Small Mammal | 0.06 | *(none)* | `null` (safe) | 7 rads |
| `prey_irradiated_stag` | Irradiated Stag | Apex Herbivore | 0.60 | `disease_chronic_wasting` | `disease_chronic_wasting` | 40 rads |
| `prey_trench_weasel` | Trench Weasel | Carnivore | 0.10 | *(none)* | `null` (safe boundary) | 9 rads |

### Authoritative Traps (10)
- `trap_snare` (Snare)
- `trap_deadfall` (Deadfall)
- `trap_pitfall` (Pitfall)
- `trap_cage` (Cage)
- `trap_box` (Box Trap)
- `trap_spring_spear` (Spring Spear)
- `trap_electrified_mesh` (Electrified Mesh)
- `trap_conibear` (Conibear)
- `trap_funnel_net` (Funnel Net)
- `trap_pressure_plate` (Pressure Plate)

### Authoritative Baits (6)
- `bait_grain_seeds` (Grain Seeds)
- `bait_scrap_meat` (Scrap Meat)
- `bait_insect_cluster` (Insect Cluster)
- `bait_canned_ration` (Canned Ration)
- `bait_fermented_mush` (Fermented Mush)
- `bait_sweet_sap` (Sweet Sap)

---

## 6. Verification Pipeline

The guarantees documented here are gated in continuous integration by the following test suites:

| Suite | File | Tests | Focus |
|---|---|---|---|
| **Disease Fallback** | `Ashfall.Core.Tests/WildlifeDiseaseFallbackTests.cs` | 7 | Exact 0.1 boundary, named overrides, full census, single evaluation |
| **Contamination Dose** | `Ashfall.Core.Tests/WildlifeContaminationDoseTests.cs` | 8 | `< 80` rad limit, anchors, 4x20 ARS trigger, delegate dispatch |
| **Trap Determinism** | `Ashfall.Core.Tests/WildlifeTrapDeterminismTests.cs` | 6 | Broken zero-draw budget, absent zero-draw, mixed-set equivalence |
| **Catalog Integration** | `Ashfall.Core.Tests/WildlifeTrappingCatalogIntegrationTests.cs` | 9 | 10/15/6 counts, cross-catalog references, registration, full cycle |

### Canonical Verification Commands
```bash
# Core xUnit test suites for wildlife trapping
dotnet test Ashfall.Core.Tests --filter "FullyQualifiedName~Wildlife"

# Catalog integrity & cross-system reference gate
godot --headless --path . -- --data-integrity-selftest

# UI and panel binding selftest
godot --headless --path . -- --scene-binding-selftest

# Scene and script lint gate
python3 scripts/ci/scene-lint.py
```
