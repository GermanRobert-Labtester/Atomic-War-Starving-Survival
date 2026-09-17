# ASHFALL — Quality Roadmap Batch 71

## Theme: Configuration & Tuning Externalization — Move Hardcoded Constants to Data

| Field | Value |
|-------|-------|
| **Priority** | MEDIUM-HIGH |
| **Risk** | Low-to-Medium — Steps 1–4 and 7 are additive/isolated (Low); Steps 5–6 edit live gameplay systems and carry silent-balance-drift risk (Medium) — see per-step Risk/Rollback notes |
| **Depends on** | Existing `SystemTextJsonSerializer`, `CatalogIntegrityValidator`, data authority pipeline |
| **Blocks** | Live tuning tools, designer iteration speed, balance patches without recompilation |
| **Estimated scope** | 7 steps across Core + StreamingAssets/Data |

---

## Motivation

Many Core systems embed gameplay-tuning constants directly in C# code. **Verified against the actual codebase** (see Review Notes for the full correction log — several of the claims below were wrong in the original draft and are corrected here):

- `NeedsSystem` (`Assets/Ashfall.Core/Survivors/NeedsSystem.cs`) — decay rates are NOT in `NeedsSystem` itself. They live in a separate, already-injectable `NeedsProfile` class in the same file (fields: `hungerPerHour=0.8f`, `thirstPerHour=1.2f`, `fatiguePerHour=0.4f`, `warmthLossPerHourInCold=0.5f`, `warmthRestorePerHourNearHeat=3f`, `moraleLossPerHourWhileCritical=1f`, `healthLossFromHunger=0.4f`, `healthLossFromThirst=0.6f`, `healthLossFromCold=0.3f`, `hungerCritical=90f`, `thirstCritical=90f`, `warmthCritical=20f`). `NeedsSystem`'s constructor already accepts a `NeedsProfile` — this "migration" is mostly a rename/relocation of an existing seam, not new plumbing.
- `RadiationSystem` (`Assets/Ashfall.Core/Radiation/RadiationSystem.cs`) — dose thresholds exist as `public const float`: `AcuteThreshold=80f`, `ChronicLifetimeThreshold=400f`, `HealthLossPerHourAtAcute=5f`, `IodineResistanceHours=6f`, `RadResistanceFactor=0.5f`, `IodineWindowHours=24f`. There is **no shielding-multiplier constant and no decay-half-life constant** — shielding is a runtime parameter (`ExposureContext.ShelterShielding`, supplied by the host per-tick), not a hardcoded tuning value, and there is no radiation decay/half-life logic in this file at all (dose only accumulates or is reduced via `AdministerAntiRad`/`AdministerIodine`). Do not scope "shielding multipliers" or "decay half-life" into this system's migration — they don't exist here.
- `WeatherSystem` (`Assets/Ashfall.Core/World/WeatherSystem.cs`) — storm *probabilities* are already data-driven (`SeasonWindowDef.falloutStormWeight` etc., loaded from JSON, not hardcoded). What IS hardcoded as `public const float`: `FalloutStormOutdoorRadModifier=150f`, `BlackRainOutdoorRadModifier=250f`, `BlackRainHazmatMeltMultiplier=5f`, `BlizzardTemperaturePenaltyC=-15f`, `FalloutStormTemperaturePenaltyC=-5f`, `BlackRainTemperaturePenaltyC=-8f`, `BlizzardVisibilityFactor=0.4f`. Scope this step to the modifier constants, not "storm probabilities" (already externalized).
- `CombatTraumaSystem` (`Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs`) — confirmed hardcoded `public const float`: `HypervigilancePerCombat=0.05f`, `HypervigilanceDecayPerDay=0.02f`, `DefenseBonusPerHypervigilance=0.15f`, `FalseAlarmChancePerNight=0.30f`, `FalseAlarmMoraleHit=-5f`, `CompanionGroundingReduction=0.50f`, `MaxHypervigilance=1f`, `CombatDecayThresholdHours=72f`. Note: this class already uses `ISeededRng Rng` (a host-injected field), not `System.Random` — AGENTS.md's "Known Offenders" entry for this file (`CombatTraumaSystem.cs:53 — public System.Random Rng;`) is stale/out of date; do not re-flag it as a determinism violation in this batch.
- `DynamicEconomySystem` (Core portion) — **does not exist**. There is no `DynamicEconomySystem.cs` anywhere under `Assets/Ashfall.Core/`. The real Unity-side offender (`Assets/_Game/Economy/DynamicEconomySystem.cs`) is explicitly out of scope per AGENTS.md Invariant 3/5 (legacy Unity tree, read-only). The actual Core economy logic lives in `Assets/Ashfall.Core/Economy/MarketSystem.cs`, which already has named constants (e.g. `MinDemandMult=0.25f`, `MaxDemandMult=4f`) documented as "Unity parity constants (DynamicEconomySystem)". Retarget this line item to `MarketSystem.cs`.
- `HoldfastRuntimeSession` (`src/Host/HoldfastRuntimeSession.cs`) — confirmed: `public const int MaxHealth = 100;`, `MaxHunger = 100;`, `MaxThirst = 100;` exist exactly as claimed, alongside several more not mentioned in this list: `DefaultStartingValue=100`, `RadDamageThreshold=50f`, `StarvationThreshold=90f`, `DehydrationThreshold=90f`, plus inline magic numbers in method bodies (`Hunger + 8`, `Thirst + 10`, `Radiation * 0.07f` decay, `hpLoss` multipliers `0.5f`/`0.6f`/`0.1f`, `ConsumeFood`'s `30 * amount`, `ConsumeWater`'s `35 * amount`, `UseAntiRad`'s default `reduction = 20f`, death-cause radiation breakpoints `200`/`100`). **Important scope note:** `HoldfastRuntimeSession` lives in `src/Host/` (the Godot host, namespace `AtomicWar.GodotApp`), not in `Assets/Ashfall.Core/`. Per AGENTS.md Invariant 5, gameplay tuning values that matter for balance should arguably live in Core, not the host — moving them into `TuningConfig` is consistent with that invariant, but this step is host-code surgery, not Core-only, and should say so explicitly.
- `GreenhouseSystem` (`Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs`) — confirmed `public const float`: `MaxWater=100f`, `MaxContamination=100f`, `GrowingThreshold=33f`, `DroughtBlightRatePerDay=0.25f`, `OutbreakBlightStep=0.3f`, `BaseBlightChancePerDay=0.06f`, `TaintedWaterContaminationPerUnit=1.5f`, `ResidualContaminationAfterHarvest=0.5f`.

Today, tuning any of these requires: edit C# → recompile → test → commit. The project already has a mature data authority (`Assets/StreamingAssets/Data/`), a validated JSON pipeline, and the `SystemTextJsonSerializer` class (`Assets/Ashfall.Core/HostDefaults.cs:32`, implements `IJsonSerializer`). Moving constants to a `tuning_config.json` file enables designers and testers to iterate without code changes, and aligns with Invariant 6 (data authority is JSON).

---

## Step 1 — Audit Hardcoded Gameplay Constants Across Core Systems

### Goal
Produce a categorized inventory of every hardcoded numeric constant in `Assets/Ashfall.Core/` that controls gameplay tuning (not structural constants like array sizes or protocol versions).

### Implementation
- Grep Core for `const`, `static readonly`, and magic-number assignments in system classes.
- Categorize each by domain: Needs, Radiation, Weather, Combat, Economy, Medical, Shelter, Expansion.
- Record: file path, line number, current value, what it controls, whether it's referenced cross-system.
- Exclude structural constants (enum backing values, protocol versions, `schema_version`).
- Output a `docs/tuning_audit.md` table with columns: Domain | File | Line | Name | Value | Description.

### Verification
- `docs/tuning_audit.md` exists. Do not pre-commit to a "40–120" range from an unaudited guess: spot-checks in this batch's review already found ~25 named `const` fields across just 4 files (`RadiationSystem`: 7, `WeatherSystem`: 7, `CombatTraumaSystem`: 8, `GreenhouseSystem`: 8, `HoldfastRuntimeSession`: 6 named + ~10 inline magic numbers, `MarketSystem`: 2+). Set the acceptance count from the actual grep output, not a guess: "count returned by the Step 1 grep, with a human sign-off that no `Assets/Ashfall.Core/**/*.cs` file containing `NeedKind`, `Radiation`, `Weather`, `Combat`, `Economy`, `Medical`, `Shelter`, or `Holdfast` in its path was skipped."
- Every listed constant is confirmed present at the cited file:line (spot-check with `grep -n "public const" <file>` against the audit table — this is a mechanically checkable assertion, not a subjective review).
- No `UnityEngine.*` or `Godot.*` references in the audit scope (run `grep -rl "UnityEngine\.\|Godot\." Assets/Ashfall.Core/` — must return nothing under the audited files).

### Done when
- Audit document is complete, reviewed, and committed.
- Audit explicitly covers: `NeedsSystem.cs` (really: the co-located `NeedsProfile` class), `RadiationSystem.cs`, `WeatherSystem.cs`, `CombatTraumaSystem.cs`, `GreenhouseSystem.cs`, `MarketSystem.cs` (the real Core economy file — see Motivation correction; there is no `DynamicEconomySystem.cs` in Core), and `HoldfastRuntimeSession.cs` (flagged separately as host code, not Core — see Risk note below).
- Audit distinguishes "named `const`/`static readonly` field" from "inline magic number in a method body" as two separate table rows/categories — `HoldfastRuntimeSession.TickDay()` and `ConsumeFood`/`ConsumeWater`/`UseAntiRad` have several of the latter that a naive `grep "const"` will miss.

### Risk / Rollback
Risk: Low — this step only produces a document, no code changes. Rollback: delete `docs/tuning_audit.md` if the audit needs to be redone; no code is touched so there is nothing to revert.

---

## Step 2 — Design Tuning Data Schema (`tuning_config.json`)

### Goal
Define the JSON schema for `Assets/StreamingAssets/Data/tuning_config.json` following project conventions (snake_case, `schema_version`, domain sections).

### Implementation
- Create `Assets/StreamingAssets/Data/tuning_config.json` with top-level structure:
  ```json
  {
    "schema_version": 1,
    "needs": { ... },
    "radiation": { ... },
    "weather": { ... },
    "combat": { ... },
    "economy": { ... },
    "medical": { ... },
    "shelter": { ... },
    "holdfast": { ... }
  }
  ```
- Each domain section mirrors the constants found in Step 1.
- All keys are `snake_case`. All numeric values have a comment-field sibling (`_comment` or inline doc) explaining valid ranges.
- Include a `_defaults` key per section documenting the original hardcoded value (enables rollback).
- Validate that the schema is compatible with `CatalogIntegrityValidator` (no orphan IDs, valid structure).

### Verification
- `tuning_config.json` parses without error via `SystemTextJsonSerializer`.
- `godot --headless --path . -- --data-integrity-selftest` passes (0 errors) with the new file present.
- Schema review confirms all Step 1 constants are represented.

### Done when
- `tuning_config.json` is committed to `Assets/StreamingAssets/Data/`.
- Schema matches 1:1 with the audit from Step 1.
- File passes data integrity selftest.

---

## Step 3 — Create `TuningConfig` Class in Core With Typed Accessors and Defaults

### Goal
Implement `Assets/Ashfall.Core/Configuration/TuningConfig.cs` — a plain C# class providing typed, domain-grouped accessors with compile-time defaults that match today's hardcoded values.

### Implementation
- Namespace: `Ashfall.Core.Configuration`.
- No engine references (`UnityEngine.*`, `Godot.*`, `JsonUtility`) — pure C#.
- Nested classes per domain: `NeedsTuning`, `RadiationTuning`, `WeatherTuning`, `CombatTuning`, `EconomyTuning`, `MedicalTuning`, `ShelterTuning`, `HoldfastTuning`.
- Each field has a default value matching the current hardcoded constant (zero behavioral change on migration).
- Immutable after construction (constructor + `init` properties or readonly fields populated by loader).
- `TuningConfig.Default` static property returns an instance with all original hardcoded values.
- Add XML doc comments on every property explaining what it controls and valid range.

### Verification
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` compiles cleanly.
- `TuningConfig.Default` returns non-null with all fields populated.
- No `UnityEngine` / `Godot` / `JsonUtility` references in the new file.

### Done when
- `TuningConfig.cs` compiles in Core with zero warnings.
- `TuningConfig.Default` matches every value from the Step 1 audit.
- Class is engine-agnostic and follows Invariant 1.

---

## Step 4 — Create `TuningConfigLoader` (Follows CatalogLoader Pattern)

### Goal
Implement `Assets/Ashfall.Core/Configuration/TuningConfigLoader.cs` that deserializes `tuning_config.json` into a `TuningConfig` instance, with graceful fallback to defaults for missing fields.

### Implementation
- Namespace: `Ashfall.Core.Configuration`.
- Constructor accepts `IFileIO` and `IJsonSerializer` (ports pattern, Invariant 2).
- `Load(string path)` returns `TuningConfig`. If file missing or corrupt, returns `TuningConfig.Default` and logs warning via `ILog`.
- Per-field fallback: if a domain section is present but a field is missing, use the default for that field (partial override support).
- Validate `schema_version` — reject future versions (throw), migrate past versions (currently only V1, so just validate == 1).
- Log a summary on load: "TuningConfig loaded: {N} overrides from default" via `ILog.Info`.

### Verification
- Unit test: load valid `tuning_config.json` → all values match.
- Unit test: load file with missing `radiation` section → radiation values are defaults, others loaded.
- Unit test: load corrupt file → returns `TuningConfig.Default`, logs warning.
- Unit test: load file with `schema_version: 99` → throws with clear message.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — all pass.

### Done when
- Loader compiles, all four test cases pass.
- Loader uses only `IFileIO` + `IJsonSerializer` (no direct `System.IO` or `File.ReadAllText`).
- Follows existing CatalogLoader patterns (see `YearOfAshCatalogLoader`, `VerdictCatalogLoader`).

---

## Step 5 — Migrate NeedsSystem / RadiationSystem Constants to TuningConfig

### Goal
Replace hardcoded constants in `NeedsSystem` and `RadiationSystem` with values read from `TuningConfig`, maintaining identical behavior when using default config.

### Implementation
- Both systems receive `TuningConfig` (or their domain sub-object) via constructor injection.
- Replace every `const` / magic number identified in Step 1 for these two systems with a property read from `TuningConfig.Needs.*` or `TuningConfig.Radiation.*`.
- Remove the old `const` declarations (or keep as `[Obsolete]` temporarily if other systems reference them).
- Update `GameBootstrap` / `Main.cs` wiring to pass `TuningConfig` to these systems.
- If `HoldfastRuntimeSession` references Needs/Radiation constants, update those references too.

### Verification
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — all tests pass (repo currently has 1964 `[Fact]`/`[Theory]` methods across `Ashfall.Core.Tests/*.cs`; use the actual count from your checkout via `grep -rc '\[Fact\]\|\[Theory\]' Ashfall.Core.Tests/*.cs | awk -F: '{sum+=$2} END {print sum}'` rather than hardcoding "1941+" — that figure will drift as tests are added and become a stale, unverifiable assertion).
- `dotnet build Ashfall.csproj` — 0 errors, 0 warnings.
- Manual inspection: no hardcoded tuning constants remain in `NeedsSystem.cs` (check the co-located `NeedsProfile` class specifically, since that's where the real fields are, not `NeedsSystem` itself) or `RadiationSystem.cs`.
- Existing `Ashfall.Core.Tests/NeedsRadiationSystemTests.cs` pass without modification (proves defaults match). **Correction:** this file contains 5 test classes (`NeedsSystemTests`, `RadiationSystemTests`, `SaveRoundTripTests`, `GearProtectionBridgeTests`, `InventoryGearBridgeTests`) totaling 24 `[Fact]` methods as of this review, not "58 tests" — that figure (also repeated in `AGENTS.md`'s H10 entry) is stale/incorrect. Re-count with `grep -c '\[Fact\]\|\[Theory\]' Ashfall.Core.Tests/NeedsRadiationSystemTests.cs` before citing a number, and prefer asserting "all tests in this file pass" over citing a count that can drift.

### Done when
- Both systems read all tuning values from `TuningConfig`.
- Test suite passes unchanged (behavioral equivalence confirmed) — verified by re-running the exact `dotnet test` command above and diffing pass/fail counts before and after, not just "eyeballing green."
- No regressions in `godot --headless --path . -- --data-integrity-selftest`.

### Risk / Rollback
Risk: **Medium, not Low** — this is the first step that edits live, shipped gameplay systems (`NeedsSystem`, `RadiationSystem`) rather than adding new isolated files. A mistake here (e.g. a default that doesn't exactly match the original constant, a sign flip, a missed call site still reading the old `const`) silently changes balance rather than failing loudly, because there's no automated numeric-equivalence test beyond "existing tests still pass" — and existing tests may not exercise every field (e.g. `Hygiene` decay has no dedicated constant in `NeedsProfile` at all today, so verify whether hygiene decay is handled elsewhere before assuming full coverage).
- **Rollback plan:** do this step as a single isolated commit per system (`NeedsSystem` migration, then `RadiationSystem` migration, as two commits) so either can be reverted independently with `git revert` without touching the other. Keep the old `const` declarations in place but unused (or `[Obsolete]`) for one commit before deleting them, so a revert of the *consumer* change alone still compiles.
- Before merging, add one characterization test per migrated field that asserts `TuningConfig.Default.Needs.HungerPerHour == 0.8f` (etc., using the exact real values captured in the Motivation section above) — this is the actual regression guard; "tests still pass" alone does not prove the defaults are byte-for-byte identical to the pre-migration constants.

---

## Step 6 — Migrate Market / Combat / Weather Constants to TuningConfig

### Goal
Extend the externalization to remaining high-churn domains: `MarketSystem` (Core; **not** `DynamicEconomySystem`, which does not exist in `Assets/Ashfall.Core/` — see Motivation correction), `CombatTraumaSystem`, and `WeatherSystem`.

### Implementation
- Same pattern as Step 5: constructor injection of relevant `TuningConfig` sub-object.
- `CombatTraumaSystem` — the real named constants to migrate: `HypervigilancePerCombat`, `HypervigilanceDecayPerDay`, `DefenseBonusPerHypervigilance`, `FalseAlarmChancePerNight`, `FalseAlarmMoraleHit`, `CompanionGroundingReduction`, `MaxHypervigilance`, `CombatDecayThresholdHours` (confirmed at `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs`).
- `WeatherSystem` — the real named constants to migrate: `FalloutStormOutdoorRadModifier`, `BlackRainOutdoorRadModifier`, `BlackRainHazmatMeltMultiplier`, `BlizzardTemperaturePenaltyC`, `FalloutStormTemperaturePenaltyC`, `BlackRainTemperaturePenaltyC`, `BlizzardVisibilityFactor` (confirmed at `Assets/Ashfall.Core/World/WeatherSystem.cs`). Per-season storm *weights* stay in JSON catalog data (`SeasonWindowDef`) — they are already externalized and out of scope here; do not duplicate that data into `tuning_config.json`.
- `MarketSystem` (Core) — confirmed constants: `MinDemandMult=0.25f`, `MaxDemandMult=4f` (at `Assets/Ashfall.Core/Economy/MarketSystem.cs:72-73`, labeled "Unity parity constants (DynamicEconomySystem)"). Audit the rest of this file in Step 1 for the full constant list — do not assume the two confirmed here are the only ones.
- Update `GameBootstrap` / `Main.cs` wiring for all three systems.
- If any expansion systems (Holdfast, YearOfAsh, Verdict) reference these constants, update them to read from `TuningConfig` as well.

### Verification
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — all tests pass (see Step 5's note: verify the actual current count rather than citing a fixed number).
- `dotnet build Ashfall.csproj` — 0 errors, 0 warnings.
- Grep Core for remaining hardcoded tuning constants — only structural/non-tuning constants should remain: `grep -rn "public const float\|public const int" Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs Assets/Ashfall.Core/World/WeatherSystem.cs Assets/Ashfall.Core/Economy/MarketSystem.cs` should return nothing (or only non-tuning constants like `SystemId` string constants, which are identifiers, not tuning values, and should NOT be migrated).
- `godot --headless --path . -- --data-integrity-selftest` — 0 errors.

### Done when
- All three systems read tuning values from `TuningConfig`.
- No tuning-class constants remain hardcoded in `MarketSystem`, `CombatTraumaSystem`, or `WeatherSystem`.
- Full test suite passes unchanged.
- Per-field characterization tests (per Step 5's rollback note) exist for every migrated constant in this step too.

### Risk / Rollback
Risk: **Medium** (same reasoning as Step 5 — live system edits, silent-balance-drift failure mode). `WeatherSystem`'s `RollNextState` reseeds from `_seed` and `_state.rollCount` each roll (Invariant 4, determinism) — if a migrated constant changes the *shape* of a probability calculation rather than just its value, replays of existing saves could diverge even with `TuningConfig.Default` matching old values, because the migration touches code paths that feed the deterministic RNG. Add a determinism regression test: same seed + same `TuningConfig.Default` → identical `WeatherKind` sequence before and after migration, not just "value defaults match."
- **Rollback plan:** one commit per system (3 commits: Market, Combat, Weather), same reasoning as Step 5.

---

## Step 7 — Add Tuning Validation Tests (Ranges, No Negatives, Required Fields)

### Goal
Write a dedicated test class `TuningConfigValidationTests.cs` that validates any `tuning_config.json` meets sanity constraints — preventing broken configs from causing silent gameplay bugs.

### Implementation
- Namespace: `Ashfall.Core.Tests`.
- Test categories:
  - **Range tests**: per-field ranges must be derived from the actual confirmed values, not a blanket assumption. Correction: not all "rate" fields are bounded to (0, 1] — e.g. `RadiationSystem.ChronicLifetimeThreshold=400f` and `HealthLossPerHourAtAcute=5f` are absolute thresholds/rates, not normalized fractions, and `WeatherSystem.BlizzardTemperaturePenaltyC=-15f` is legitimately negative (a penalty). Write the range table per-field from the Step 1 audit (e.g. `hungerCritical`/`thirstCritical` ∈ (0, 100], `warmthCritical` ∈ [0, 100), probabilities like `FalseAlarmChancePerNight` ∈ [0, 1], temperature penalties ∈ (-50, 0]) rather than one universal rule applied to every field.
  - **No-negative tests**: apply only to fields that represent a physical quantity that cannot go negative (health, hunger, dose, durability) — explicitly exclude temperature-penalty and multiplier fields that are meaningfully negative or could exceed 1(e.g. `BlackRainHazmatMeltMultiplier=5f`).
  - **Required-field tests**: every domain section must be present; every field within each section must be present (no partial configs in production).
  - **Consistency tests**: `min_*` ≤ `max_*` for all paired fields (e.g. `MarketSystem.MinDemandMult=0.25f` ≤ `MaxDemandMult=4f`); sum of probability distributions ≤ 1.0 where applicable.
  - **Default-equivalence test**: `TuningConfig.Default` serialized → deserialized produces identical object (round-trip).
  - **Schema-version test**: current `tuning_config.json` in repo has `schema_version: 1`.
- Run as part of standard `dotnet test` — no special setup required.

### Verification
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — all new validation tests pass.
- Intentionally break `tuning_config.json` (set a rate to -1) → validation test fails with clear message.
- Restore file → all tests pass again.
- No engine references in test file.

### Done when
- `TuningConfigValidationTests.cs` committed with at least 12 test methods covering all categories.
- Tests run green against the production `tuning_config.json`.
- Negative/corrupt configs are detected and rejected with descriptive assertion messages.

---

## Summary Table

| Step | Title | Key Deliverable | Risk | Dependencies |
|------|-------|-----------------|------|--------------|
| 1 | Audit hardcoded constants | `docs/tuning_audit.md` | None | — |
| 2 | Design tuning schema | `tuning_config.json` | Low | Step 1 |
| 3 | Create `TuningConfig` class | `Assets/Ashfall.Core/Configuration/TuningConfig.cs` | Low | Steps 1, 2 |
| 4 | Create `TuningConfigLoader` | `Assets/Ashfall.Core/Configuration/TuningConfigLoader.cs` + 4 tests | Low | Step 3 |
| 5 | Migrate Needs/Radiation | Constants externalized, full test suite still green + new characterization tests | **Medium** | Step 4 |
| 6 | Migrate Market/Combat/Weather | Remaining constants externalized | **Medium** | Step 5 |
| 7 | Tuning validation tests | `TuningConfigValidationTests.cs` (12+ tests) | None | Step 6 |

---

## Exit Criteria (Batch 71 Complete)

- [ ] Zero hardcoded tuning constants remain in `NeedsProfile` (used by `NeedsSystem`), `RadiationSystem`, `WeatherSystem`, `CombatTraumaSystem`, `MarketSystem` (Core economy — corrected from the nonexistent `DynamicEconomySystem`), `HoldfastRuntimeSession`.
- [ ] `tuning_config.json` is the single authority for all gameplay tuning values.
- [ ] `TuningConfig.Default` exactly reproduces pre-migration behavior (no gameplay change).
- [ ] Full test suite passes: `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`.
- [ ] Godot host builds cleanly: `dotnet build Ashfall.csproj`.
- [ ] Data integrity selftest passes: `godot --headless --path . -- --data-integrity-selftest`.
- [ ] Validation tests catch invalid configs with clear error messages.
- [ ] Designers can change gameplay balance by editing one JSON file — no recompilation needed.


---

## Review Notes (Corrected)

This batch was adversarially reviewed against the real codebase at
`/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`. The following factual
errors, scope gaps, and vague criteria were found and fixed in place above:

### Factual errors (verified against source, cited file:line)
1. **`NeedsSystem` decay rates don't live in `NeedsSystem`** — they live in a sibling class,
   `NeedsProfile`, in the same file (`Assets/Ashfall.Core/Survivors/NeedsSystem.cs`), already
   passed into `NeedsSystem`'s constructor as an optional parameter. The original draft implied
   the constants were embedded directly in `NeedsSystem`'s method bodies; they are not — this
   step is closer to "rename `NeedsProfile` → make it load from JSON" than "extract constants
   from method bodies." Real field names/values recorded above for exact migration targets.
2. **`RadiationSystem` does not have "shielding multipliers" or "decay half-life" constants** —
   verified by reading the full file (`Assets/Ashfall.Core/Radiation/RadiationSystem.cs`).
   Shielding is a per-tick runtime parameter (`ExposureContext.ShelterShielding`), and there is
   no dose-decay/half-life mechanic in this system at all (dose accumulates and is only reduced
   via `AdministerAntiRad`). The real hardcoded constants are `AcuteThreshold=80f`,
   `ChronicLifetimeThreshold=400f`, `HealthLossPerHourAtAcute=5f`, `IodineResistanceHours=6f`,
   `RadResistanceFactor=0.5f`, `IodineWindowHours=24f`.
3. **`DynamicEconomySystem` does not exist in `Assets/Ashfall.Core/`** — confirmed via
   repo-wide search. It exists only as a Unity-legacy file (`Assets/_Game/Economy/
   DynamicEconomySystem.cs`), which is out of scope per AGENTS.md (legacy Unity tree,
   read-only). The real Core economy file with hardcoded constants is
   `Assets/Ashfall.Core/Economy/MarketSystem.cs` (confirmed constants: `MinDemandMult=0.25f`,
   `MaxDemandMult=4f`, explicitly commented as "Unity parity constants (DynamicEconomySystem)").
   All references retargeted to `MarketSystem`.
4. **`WeatherSystem`'s "storm probabilities" claim was backwards** — per-season storm weights
   (`SeasonWindowDef.falloutStormWeight` etc.) are already loaded from JSON catalog data, not
   hardcoded in C#. What IS hardcoded are the environmental *modifier* constants
   (`FalloutStormOutdoorRadModifier`, `BlizzardTemperaturePenaltyC`, etc.). Scope corrected.
5. **`CombatTraumaSystem` already uses `ISeededRng Rng`, not `System.Random`** — AGENTS.md's
   "Known Offenders" list (under Invariant 4) is stale for this file; it was not re-flagged as
   a determinism issue in this batch, but the original draft's implicit inheritance of that
   stale claim is called out here so a future reader doesn't reintroduce the "fix."
6. **`SystemTextJsonSerializer` is a real class**, confirmed at `Assets/Ashfall.Core/
   HostDefaults.cs:32`, implementing `IJsonSerializer` — this reference was accurate and is
   kept, just cited with its concrete location.
7. **The "58 tests" figure for `NeedsRadiationSystemTests.cs` was wrong.** Actual count: 24
   `[Fact]` methods across 5 test classes in that file (`NeedsSystemTests`,
   `RadiationSystemTests`, `SaveRoundTripTests`, `GearProtectionBridgeTests`,
   `InventoryGearBridgeTests`). This number is also wrong in `AGENTS.md`'s H10 entry — it is a
   pre-existing project-doc error that this batch inherited rather than caused, but it's now
   corrected everywhere it appeared in this file. The "1941+ total tests" figure was closer to
   reality (actual: 1964 `[Fact]`/`[Theory]` methods at time of review) but was replaced with a
   command to re-derive the count live, since a hardcoded number goes stale immediately.

### Scope / risk corrections
8. **Steps 5 and 6 were marked Risk: Low; corrected to Medium.** These are the only steps that
   edit live, shipped gameplay-system code rather than adding new isolated files. The stated
   verification ("existing tests pass") does not actually prove numeric equivalence for fields
   with no dedicated test coverage — added a requirement for explicit per-field
   characterization tests asserting `TuningConfig.Default.X == <real captured value>`, and
   flagged that `WeatherSystem`'s deterministic-reseed RNG path (Invariant 4) needs a sequence
   equivalence test, not just a "defaults match" test.
9. **No rollback plan existed for any step.** Added commit-granularity rollback guidance
   (one system per commit) for Steps 5 and 6 specifically, since those are the risky ones.
10. **`HoldfastRuntimeSession` scope was understated.** The original draft listed only
    `MaxHealth`/`MaxHunger`/`MaxThirst` (confirmed accurate) but missed `DefaultStartingValue`,
    `RadDamageThreshold`, `StarvationThreshold`, `DehydrationThreshold`, and roughly ten inline
    magic numbers in `TickDay()`/`ConsumeFood()`/`ConsumeWater()`/`UseAntiRad()`/
    `DetermineDeathCause()`. Also flagged that this file lives in `src/Host/`
    (`AtomicWar.GodotApp` namespace) — it's host code, not Core, so this step is not a pure
    Core-internal refactor as implied.

### Vague Done-when criteria tightened
11. Step 1's "at least 40 constants (expected 80–120)" was an unverified guess presented as a
    target; replaced with a mechanically-derivable count from the Step 1 grep itself, plus a
    concrete list of files that must be covered.
12. Step 7's "decay rates ∈ (0, 1]" blanket range rule is factually wrong for several confirmed
    real fields (`ChronicLifetimeThreshold=400f`, temperature penalties that are negative by
    design). Replaced with a requirement to derive per-field ranges from the actual audited
    values instead of applying one rule to every numeric field.

### What was already correct (not changed)
- `HoldfastRuntimeSession.MaxHealth/MaxHunger/MaxThirst = 100` — confirmed byte-for-byte accurate.
- `GreenhouseSystem` constants — confirmed accurate (`MaxWater`, `MaxContamination`,
  `GrowingThreshold`, `DroughtBlightRatePerDay`, `OutbreakBlightStep`, `BaseBlightChancePerDay`,
  `TaintedWaterContaminationPerUnit`, `ResidualContaminationAfterHarvest` all present as named
  `public const float` fields).
- The `CatalogLoader` pattern reference (`YearOfAshCatalogLoader`, `VerdictCatalogLoader`) —
  both files real, both take `IFileIO`/`IJsonSerializer` per the ports pattern, as claimed.
- The five verification commands (`dotnet build`/`dotnet test`/`dotnet build Ashfall.csproj`/
  `godot --headless ... --data-integrity-selftest`) are all real, runnable commands confirmed
  against `HostCli.cs`'s actual CLI verb list.
