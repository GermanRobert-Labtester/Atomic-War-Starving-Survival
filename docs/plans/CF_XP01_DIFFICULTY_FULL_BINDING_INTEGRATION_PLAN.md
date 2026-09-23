# CF-XP01 — Difficulty Full Binding Integration Plan (Verification, Reconciliation & Residual Hardening)

**Package:** `CF-XP01-DIFFICULTY-FULL-BINDING` (`XP-WAVE1-DIFFICULTY-AUTHORITY`, follow-on slice)
**Anchor:** `claim-xp-wave1-difficulty-2026-09-18` (`WORKTREE_OWNERSHIP.md`), `docs/governance/DECISION_PACKET_2026-09-18_XP_EXPANSION_W1.md`, `docs/plans/xp/w1/W1_IMPLEMENTATION_LOG.md`
**Plan type:** Verification-and-reconciliation plan with one bounded, evidence-based residual hardening delta. **The bound campaign-creation binding described by the original CF-XP01 scope is already implemented, tested, and sealed in production.** This document does not propose rebuilding it.
**Status of underlying feature:** LIVE and SEALED since 2026-09-19 (commit `5971c1979043fffc107a13751b0bb7dfac8292e7`, "feat(difficulty): bind XP-01 difficulty selection, persistence, and consumer seams").
**Plan authored:** 2026-09-21, by direct source inspection and focused test execution at current HEAD (branch `feat/unblock-cf-p28-and-plan-implementation`), superseding the short closeout note previously at this path.
**Entry gate:** none for the reconciliation and documentation-format work (read-only). The one residual code delta (§3, §18) is a single new focused Core test plus one new self-test check on an already-shipped consumer seam — no new architecture, no new claim beyond the existing `claim-xp-wave1-difficulty-2026-09-18` row's already-listed files.

---

# 1. Objective

Replace the short, out-of-format closeout note previously at this path with a
full, verifiable, 25-section integration plan that:

1. Documents — with current file:line evidence, not inherited claims — exactly
   what CF-XP01's "full binding" scope already delivered: the difficulty
   catalog, director, scalar provider, campaign-creation selection, manifest
   persistence, fail-closed restore, starting-bonus grant, the starting-cohort
   panel, the CLI self-test, and all eight authored scalars' production
   consumer seams.
2. Corrects a real, re-verified numeric drift in the historical verification
   record: the retired closeout claimed `dotnet test --filter Difficulty`
   returned "19/19 PASS (`DifficultyPresetCatalogTests` 8/8,
   `DifficultyDirectorTests` 11/11)". Both per-file counts are wrong today.
   Direct execution in §4.4 shows `DifficultyPresetCatalogTests` is 4/4,
   `DifficultyDirectorTests` is 4/4, and a third file the note never named,
   `DifficultyFullBindingTests`, is 7/7 — a true committed total of 15/15
   across three files, not 19/19 across two.
3. Identifies the one authored scalar — `hostile_encounter_mult` — that has a
   real, shipped production consumer (`src/Main.EvolvingWorld.cs:193-195`) but
   **zero dedicated regression coverage** anywhere in the current test corpus
   or the CLI self-test, and proposes the smallest safe fix: one Core-level
   fact plus one self-test `Check(...)` line, mirroring the pattern already
   used for the other seven scalars. This is the plan's only "required delta".
4. Flags, without touching, a governance ledger disagreement (`INTEGRATION_PLANS.md`
   still narrates "XP Expansion W1 — ACTIVE" while `WORKTREE_OWNERSHIP.md`'s
   claim row for the same package says "DONE 2026-09-19"), and a second,
   separate premise-drift risk newly visible in the working tree: two
   uncommitted, unwired Core files (`DifficultySettingsSystem.cs`,
   `DifficultyConsequenceWeave.cs`) that would create a second difficulty
   authority if ever connected to a host without reconciling them against the
   seam this plan documents. Both are named for the foreman; neither is edited
   here.

**Bounded outcome:** an accurate, current, 25-section plan document at this
path, plus (as the only in-scope code change, and only if the foreman opens a
dedicated micro-claim for it) one new Core fact and one new self-test check
closing the `hostile_encounter_mult` coverage gap. Nothing else in the
difficulty system, save schema, or consumer owners changes.

## 1.1 Non-goals (explicit)

- No mid-campaign difficulty change, no New Game+ composition, no difficulty
  slider/customization UI. (That surface is what the uncommitted, unwired
  `DifficultySettingsSystem.cs` — Plan 181 — appears to be building; it is
  explicitly out of scope for this plan; see §22.)
- No new difficulty save section, no new campaign authority, no change to
  `CampaignSaveEnvelope`'s existing `difficultyPresetId` field shape or the
  aggregate checksum contract.
- No edit to `Assets/Ashfall.Core/Endgame/CampaignCompletionHistory.cs`,
  `src/Host/CompletionHistoryStore.cs`, `src/Main.Endgame.cs`, or their tests —
  these remain excluded under `claim-wave11-part2-execution-2026-09-18`, exactly
  as the original W1 claim recorded.
- No rebinding, re-scoring, or re-authoring of the four catalog presets or
  their eight scalars. The authored values in `difficulty_presets.json` are
  unchanged by this plan.
- No reconciliation, adoption, retirement, or wiring of
  `DifficultySettingsSystem.cs` or `DifficultyConsequenceWeave.cs`. They are
  named as evidence of a risk, not absorbed into this package.
- No edit to `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, or
  `AGENTS.md`/`CLAUDE.md`'s "Active Queue" prose. The ledger disagreement in
  §4.6 is reported for the foreman/integrator to correct; this plan does not
  self-authorize governance edits.

---

# 2. Current Reality

Ashfall's difficulty authority is a single, already-shipped vertical slice:
one JSON catalog → one Core director → one immutable scalar-provider view →
six existing-system consumer seams (covering all eight authored scalars) →
one campaign-creation selection path → one manifest field → one fail-closed
restore path → one starting-cohort panel → one CLI self-test. There is no
second difficulty store, no duplicate catalog, and no parallel campaign
authority in the reachable, host-wired code path.

## 2.1 The authored data

`Assets/StreamingAssets/Data/difficulty_presets.json` — `schema_version: 1`,
four presets, `default_preset_id: "difficulty_standard"`:

| id | display_name | hunger/thirst | radiation | disease | hostile | market | equipment | crisis | starting bonus |
|---|---|---|---|---|---|---|---|---|---|
| `difficulty_sparing` | SPARING | 0.75 / 0.75 | 0.75 | 0.8 | 0.75 | 0.9 | 0.8 | 1.25 | `canned_food`, `iodine_pills` |
| `difficulty_standard` | STANDARD | 1.0 / 1.0 | 1.0 | 1.0 | 1.0 | 1.0 | 1.0 | 1.0 | (none) |
| `difficulty_austere` | AUSTERE | 1.35 / 1.35 | 1.3 | 1.25 | 1.35 | 1.15 | 1.25 | 0.8 | (none) |
| `difficulty_dirge` | DIRGE | 1.75 / 1.75 | 1.6 | 1.5 | 1.75 | 1.3 | 1.5 | 0.65 | (none) |

All eight scalar fields per preset (`hunger_rate_mult`, `thirst_rate_mult`,
`radiation_gain_mult`, `disease_onset_mult`, `hostile_encounter_mult`,
`market_price_mult`, `equipment_decay_mult`, `crisis_deadline_mult`) are
present on every preset — there is no partial-preset shape in the shipped
data. `crisis_deadline_mult` is the one scalar that runs *below* 1.0 for
harsher presets (shorter deadlines are harsher), which the rest of this
document treats consistently.

## 2.2 The Core authority — `Assets/Ashfall.Core/Difficulty/`

Five files exist on disk today; only three are part of the sealed CF-XP01
scope. The other two are new, uncommitted, and unwired (§2.6).

### 2.2.1 `DifficultyPresetCatalog.cs` (committed, sealed)

Defines `DifficultyScalars` (the eight `float` fields plus `Legacy()` — all
ones — `Clone()`, and `Validate(out error)` bounding every field to
`[0.25, 2.5]`), `DifficultyPreset` (`id`, `display_name`, `description[_key]`,
`scalars`, `starting_bonus_item_ids`, with its own `Validate` requiring a
`difficulty_`-prefixed snake_case id, non-empty display name/description, and
non-empty bonus-item ids), `DifficultyPresetCatalog` (`schema_version`,
`presets`, `default_preset_id`, an internal `_byId` index built by `Index()`,
and `Validate` enforcing schema version, non-empty preset list, per-preset
validity, duplicate-id rejection, and a resolvable default), and
`DifficultyPresetCatalogLoader` (`Load(dataDirectory, IFileIO)` and
`LoadFromJson(json)`, both throwing `InvalidOperationException` — never
returning a partially-valid catalog — on a missing file, malformed JSON,
unsupported schema version, or any `Validate` failure).

This is a pure `netstandard2.1` file: no `Godot`, no `UnityEngine`, no engine
serialization type. It is the single authority for difficulty IDs and their
scalar bundles; nothing else in the repository defines a `difficulty_` id or a
per-preset scalar bundle.

### 2.2.2 `DifficultyDirector.cs` (committed, sealed)

```csharp
public sealed class DifficultyDirector
{
    private readonly DifficultyPresetCatalog _catalog;

    public DifficultyDirector(DifficultyPresetCatalog catalog)
    {
        _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
        if (!_catalog.Validate(out string error))
            throw new ArgumentException("invalid difficulty catalog: " + error, nameof(catalog));
        _catalog.Index();
    }

    public DifficultyPreset ResolvePreset(string? campaignPresetId)
    {
        string id = string.IsNullOrWhiteSpace(campaignPresetId)
            ? _catalog.default_preset_id
            : campaignPresetId;
        if (!_catalog.TryGet(id, out DifficultyPreset preset))
            throw new InvalidOperationException("unknown difficulty preset '" + id + "'");
        return preset;
    }

    public DifficultyScalarsProvider ResolveProvider(string? campaignPresetId)
    {
        DifficultyPreset preset = ResolvePreset(campaignPresetId);
        return DifficultyScalarsProvider.FromPreset(preset);
    }
}
```

The director is intentionally thin: it never applies a scalar itself, never
touches a save section, and never owns a campaign. A null or blank requested
ID resolves to the catalog default (`difficulty_standard`); any other unknown
ID throws — this is the "unknown IDs reject the load before live restore"
behavior the closeout claimed, and it is exactly what the code does.

### 2.2.3 `DifficultyScalarsProvider.cs` (committed, sealed)

An immutable typed view (`PresetId`, `HungerMult`/`NeedsMult` (alias),
`ThirstMult`, `RadiationMult`, `DiseaseMult`, `HostileEncounterMult`,
`MarketPriceMult`, `EquipmentDecayMult`, `CrisisDeadlineMult`) constructed only
via the private constructor called from `Legacy` (a static all-ones instance
keyed to `"difficulty_standard"`) or `FromPreset(preset)` (which re-validates
the preset's scalars before constructing — a provider can never wrap an
invalid scalar bundle). `Equals`/`GetHashCode` compare by preset id and all
eight scalar values, which is what the `save_binding` self-test check and the
`DifficultyManifestField_IsBoundByTheAggregateChecksum` unit test rely on
indirectly (through the manifest's string id, not the provider itself — see
§12).

### 2.2.4 `DifficultySettingsSystem.cs` — **uncommitted, unwired** (§2.6)

### 2.2.5 `DifficultyConsequenceWeave.cs` — **uncommitted, unwired** (§2.6)

## 2.3 The Godot host binding — `src/Main.Difficulty.cs`

This partial owns every runtime touchpoint between the Core director and the
rest of the game. Verified fields and methods, current source:

- `_difficulty` (`DifficultyDirector?`), `_difficultyCatalog`
  (`DifficultyPresetCatalog?`), `_difficultyScalars`
  (`DifficultyScalarsProvider`, defaulted to `.Legacy`), `_difficultyPresetId`
  (`string`, defaulted to `Legacy.PresetId`), `_difficultyBonusesGrantedForCampaign`
  (`bool`) — five fields, one campaign's worth of state, no collection, no
  second store.
- `EnsureDifficultyCatalog()` — lazy-loads the catalog once via
  `DifficultyPresetCatalogLoader.Load(dataDir, new FileSystemIO())` and
  constructs the single `_difficulty` director from it.
- `EnsureDifficultyDirector()` / `EnsureDifficultyAuthority(out error)` — the
  latter is the fail-soft wrapper: on a catalog load exception it returns
  `false` with the message rather than throwing into the caller, which is what
  lets `SetupDifficulty()` degrade to `Legacy` scalars instead of crashing
  composition if the JSON is ever missing or malformed at runtime.
- `DefaultDifficultyPresetId()` / `ResolveDifficultyPresetId(id)` — thin
  forwarders to the director, used by the new-game flow (§2.4) before a
  selection is committed.
- `SetupDifficulty()` — resolves `_difficultyScalars` from
  `_difficultyPresetId` via the director; on any authority or preset failure,
  logs and falls back to `DifficultyScalarsProvider.Legacy`. This is the
  method the composition root (`Main.SaveOrchestrator.cs:179`) calls once per
  composition pass.
- `SelectDifficultyForNewCampaign(presetId)` /
  `TrySelectDifficultyForNewCampaign(requestedId, out error)` — the *only*
  place `_difficultyPresetId`/`_difficultyScalars` are set from a player
  choice; the doc comment is explicit: *"Called exclusively by the
  fresh-campaign transaction... there is no in-run edit path."*
- `RestoreDifficultyFromCampaignHeader(save)` — feeds a persisted
  `difficulty_preset_id` (or `null` for an absent legacy field) through the
  same `SelectDifficultyForNewCampaign` call, so restore and fresh-creation
  share one resolution path rather than two.
- `DifficultyStartingBonusItemIds()` — returns the resolved preset's authored
  bonus ids; it does not itself touch inventory.
- `ResetDifficultyForCampaign()` — clears the two fields back to blank/`Legacy`
  between campaigns (called from the lifecycle reset participant, not from
  this file).
- `ValidateDifficultyEnvelope(envelope)` — returns a non-null error string if
  the manifest is missing or the persisted `difficultyPresetId` cannot be
  resolved by the director; returns `null` (accept) otherwise. This is the
  fail-closed gate wired into the load pipeline (§2.5).
- `BindDifficultyConsumers()` — installs five of the six Core-level provider
  delegates (`HungerRateMultiplier`, `ThirstRateMultiplier`,
  `ExposureRateMultiplier`, `OnsetProbabilityMultiplier`,
  `PriceMultiplierProvider`, `WearRateMultiplierProvider`) onto the already
  existing owner systems (`_survivors.Needs`, `_survivors.Radiation`,
  `_disease.Engine`, `_economy.Market`, `_equipmentCondition.System`). It does
  **not** touch the crisis or hostile-encounter seams — those are wired from
  their own owning partials (§2.4.3, §2.4.4), which is the correct place for
  them and not a gap.
- `GrantDifficultyStartingBonusesOnce()` — idempotent via
  `_difficultyBonusesGrantedForCampaign`; resolves the preset, adds each bonus
  item id through the canonical `_inventory.Inventory.AddById`, and persists
  via the existing `SaveInventory()` call — no parallel inventory or ledger.
- `PrepareDifficultyForNewCampaign()` — resets the once-flag at the start of a
  fresh campaign so the next `GrantDifficultyStartingBonusesOnce()` call can
  fire.
- `ApplyDifficultyFromLoadedManifest()` — reads
  `_saveLoadHost?.ActiveEnvelope?.manifest?.difficultyPresetId`, defaulting to
  `Legacy.PresetId` when blank, and stores it in `_difficultyPresetId` ahead of
  `SetupDifficulty()`.

## 2.4 The seven consumer owner seams (eight scalars)

Each seam multiplies an **already-owned** calculation; the director is never
in the calculation path itself, matching the doc comment on
`DifficultyScalars`: *"Values multiply an already-owned system's base
calculation; the director never applies the values itself."*

### 2.4.1 Needs — hunger and thirst (`Assets/Ashfall.Core/Survivors/NeedsSystem.cs:254-260`)

`HungerRateMultiplier`/`ThirstRateMultiplier` are `Func<float>` properties on
`NeedsSystem`, defaulted to `() => 1f`, multiplied into the single base-drift
site inside the tick method before the existing clamp. Confirmed by
`DifficultyFullBindingTests.Needs_BaseDrift_AppliesHungerAndThirstOnlyAtTheirOwnerSite`
(§4.2): a 1.5×/0.5× pair produces exactly `1.2f`/`0.6f` against a `0.8f`/`1.2f`
legacy baseline, and fatigue is asserted equal between the two runs — proving
the multiplier is scoped to hunger/thirst only, not smuggled into an unrelated
need.

### 2.4.2 Radiation (`Assets/Ashfall.Core/Radiation/RadiationSystem.cs`, `ComputeEffectiveRate`)

A `static` pure function composing dose rate before resistance/exposure
clamps; `ExposureRateMultiplier` is the `Func<float>` slot `BindDifficultyConsumers`
installs on `_survivors.Radiation`. Confirmed by
`Radiation_EffectiveRate_MultipliesBeforeTheExistingClamp` (§4.2): three fixed
input/output pairs, including a clamp-to-zero case (`ComputeEffectiveRate(5f, 10f, 0f, null, 2f) == 0f`)
proving the multiplier composes *before* the existing floor, not around it.

### 2.4.3 Disease (`Assets/Ashfall.Core/Disease/DiseaseSystem.cs`, `TryExpose`)

`OnsetProbabilityMultiplier` multiplies the onset probability before the
existing `[0,1]` clamp inside `TryExpose`. Confirmed by
`Disease_OnsetMultiplier_PreservesProbabilityBounds` (§4.2): a `2×` multiplier
against a `10×` context modifier still resolves to `EffectiveProbability == 1f`
(clamped), not an out-of-range value.

### 2.4.4 Market (`Assets/Ashfall.Core/Economy/MarketSystem.cs`, `ExplainPrice`)

`PriceMultiplierProvider` is one more factor in the existing explain-price
chain (base → demand → **difficulty** → authored floor/ceiling). Confirmed by
`Market_DifficultyMultiplier_IsBeforeTheAuthoredPriceClamps` (§4.2): a base
price of 10 with a 1.5× difficulty factor resolves to exactly `15f`, and the
factor list explicitly contains a `PriceFactorKind.Difficulty` entry — the
factor is visible and attributable, not folded silently into another kind.

### 2.4.5 Equipment (`Assets/Ashfall.Core/EquipmentConditionSystem.cs`, `ApplyWear`)

`WearRateMultiplierProvider` scales the wear delta only, before the existing
condition floor and break/jam checks. Confirmed by
`Equipment_WearMultiplier_ScalesTheUseWearDelta` (§4.2): a 2× multiplier
produces exactly double the condition loss of the unscaled run (asserted to 4
decimal places).

### 2.4.6 Crisis deadlines (`Assets/Ashfall.Core/Campaign/CrisisPredictionModel.cs`, `CrisisPredictor.Evaluate`, via `src/Main.BriefingCrisis.cs:31`)

`CrisisPredictionInputs.DeadlineMultiplier` (default `1f`) is normalized
(`NormalizeDeadlineMultiplier`, line 339) and applied at line 137 of the
model. The host wiring is one line: `DeadlineMultiplier = _difficultyScalars?.CrisisDeadlineMult ?? 1f`
inside `src/Main.BriefingCrisis.cs:31`, when the host constructs
`CrisisPredictionInputs` for the briefing surface. Confirmed by
`Crisis_DeadlineMultiplier_ScalesRunwayAndPreservesStandardParity` (§4.2): a
`0.5×` multiplier changes `HorizonDays` from 3 to 1 and `ProjectedDay` from 13
to 11 against identical stock/burn inputs, while an *unset* multiplier
(implicit `1f`) reproduces the exact pre-difficulty baseline — the required
"standard/legacy parity" property.

### 2.4.7 Hostile encounter rate (`src/Main.EvolvingWorld.cs:150-198`, `ComposeExpeditionDangerMultiplier`) — **shipped, but untested (§3, §18)**

`ComposeExpeditionDangerMultiplier()` returns the single `Func<string, float>`
installed on `_expeditions.SetEncounterChanceMultiplier(...)` from
`SetupEvolvingWorldInfluence()`. Reading the current body:

```csharp
SetupDifficulty();
float hostile = _difficultyScalars?.HostileEncounterMult ?? 1f;
if (hostile > 0f && System.Math.Abs(hostile - 1f) > 0.001f)
    mult *= hostile;

return mult;
```

This *is* a real, shipped consumer — the eighth authored scalar is not dead
data. It multiplies onto the existing warlord-danger × wildlife-pressure ×
location-threat × route-hazard composition, gated by the same
"only multiply if meaningfully non-1.0" epsilon guard used nowhere else in
this seam (a harmless, existing style choice, not a new pattern this plan
introduces). The gap is coverage, not wiring — detailed in §3 and §18.

## 2.5 Save and restore

`Assets/Ashfall.Core/Save/CampaignSaveEnvelope.cs:63`:
`public string difficultyPresetId = string.Empty;` on `SaveManifest`. This
field participates in the existing aggregate checksum computed by
`SaveSlotService.ComputeAggregateChecksum` — no new save section, no new
schema version, no parallel persistence path. On restore,
`Main.SaveOrchestrator.cs:120` wires `ValidateDifficultyEnvelope` into the
existing envelope-validation callback chain (fail-closed: an unresolvable
persisted preset id rejects the load before any other section restores), and
`Main.SaveOrchestrator.cs:133` calls `ApplyDifficultyFromLoadedManifest()`
ahead of `SetupDifficulty()` (`:179`) and `BindDifficultyConsumers()` (`:302`)
— the same two calls the fresh-campaign path makes, so restore and fresh
creation converge on identical binding order rather than diverging (this is
exactly the kind of bootstrap-path symmetry CF-P28, in this same corpus, was
built to guarantee for the manifest bootstrap in general).
`src/Host/SaveLoadHostSession.cs:1032` copies `difficultyPresetId` through the
envelope-migration/copy path used when re-materializing an
`AggregateSaveEnvelope`.

## 2.6 Two new, uncommitted, unwired files — a premise change since the original W1 evidence pass

`git status --short -- Assets/Ashfall.Core/Difficulty/ Ashfall.Core.Tests/Difficulty/`
at the time of this audit shows:

```
?? Ashfall.Core.Tests/Difficulty/DifficultyConsequenceWeaveTests.cs
?? Ashfall.Core.Tests/Difficulty/Plan181DifficultySettingsIntegrationTests.cs
?? Assets/Ashfall.Core/Difficulty/DifficultyConsequenceWeave.cs
?? Assets/Ashfall.Core/Difficulty/DifficultySettingsSystem.cs
```

Both production files are new (`??` = untracked), and a repo-wide
`grep -rn "DifficultySettingsSystem|DifficultyConsequenceWeave" src/` returns
**no matches** — neither is referenced from any Godot host file. They exist
only in `Assets/Ashfall.Core/` and their own test files. This means:

- `DifficultyConsequenceWeave.cs` (header comment: *"EN-01 / UNBLOCK-05:
  Difficulty-Consequence Weave Read Model"*) is a pure projection over
  `DifficultyScalarsProvider` computing a war-stage severity multiplier, a
  crisis-deadline-days helper, a shock/rumor weight multiplier, and a
  cross-preset monotonicity assertion. It reads the *existing* provider; it
  does not add a new scalar or a new store. As a dormant, unwired, read-only
  projection it is not itself a violation of the one-authority rule — but
  wiring it into any host without routing through the same
  `_difficultyScalars` field this plan documents would be.
- `DifficultySettingsSystem.cs` (header comment: *"Plan 181 — Difficulty
  Settings System... customizable difficulty scalars (sliders),
  ironman/campaign lock enforcement"*) is materially different in kind: it
  defines its own `DifficultySettingsState` (`ActivePresetId`, `IsLocked`,
  `IsCustom`, `CustomScalars`) — **a second mutable difficulty-selection
  state container**, independent of `Main.Difficulty.cs`'s
  `_difficultyPresetId`/`_difficultyScalars` fields and independent of
  `CampaignSaveEnvelope.difficultyPresetId`. If this class is ever
  constructed and driven from a host without first reconciling its state
  model against the one this plan documents, the campaign would have two
  different "what difficulty is this campaign on" answers living in two
  different objects — a direct instance of the "parallel resource... registry"
  AGENTS.md rule 5 forbids. Today it is inert: nothing constructs it outside
  its own test file.

This is new evidence relative to the original W1 audit (2026-09-18/19), which
never anticipated a slider/lock UI. It does not change anything already
sealed under CF-XP01's scope, but it is exactly the kind of premise drift
AGENTS.md rule 7 asks a plan to surface rather than silently work around. See
§21 and §22 for how this plan treats it (name it, don't touch it).

---

# 3. Required Delta

The delta this plan actually calls for is intentionally small, because the
originally-scoped feature is done. Three items, none of which touch the
catalog, the director, the provider, the manifest field, the panel, or any of
the six already-tested consumer seams:

1. **Document format.** Replace the four-paragraph closeout note previously at
   this path with this 25-section plan, so CF-XP01 has the same
   evidence-grounded, reviewable shape as the other nine plans in this batch
   (§24, §25).
2. **Coverage gap closure for `hostile_encounter_mult`.** Add exactly one new
   Core-level fact (mirroring the seven existing facts in
   `DifficultyFullBindingTests.cs`) and exactly one new `Check(...)` line in
   `HostCli.Difficulty.cs`'s `RunDifficultySelfTest` (bringing it from 14 to
   15 checks), proving the eighth scalar's existing production consumer
   (`Main.EvolvingWorld.cs:193-195`) responds to the multiplier and preserves
   standard-preset parity — exactly the property already proven for the other
   seven. No production behavior changes; the multiply-by-hostile line is
   already live. This closes the one real gap found in §2.4.7 and §18.
3. **Numeric reconciliation record.** Record, in this plan (§4.4, §24), the
   corrected test-count evidence so the historical "19/19" claim is not
   propagated again by a future audit that trusts the retired closeout note
   instead of running the suite.

Everything else in §2 is confirmed current and is preserved unchanged (§25,
`MUST PRESERVE`).

---

# 4. Evidence

All evidence below was gathered by direct source reading, `git log`/`git show`/
`git status`/`git diff` inspection, and running the three committed difficulty
test files via `bash scripts/run_test.sh` at the current worktree HEAD on
2026-09-21. No production file was edited to gather this evidence.

## 4.1 The sealing commit

```
commit 5971c1979043fffc107a13751b0bb7dfac8292e7
Author: Cline <cline@atomicwar.dev>
Date:   Sat Sep 19 19:33:42 2026 +0300

    feat(difficulty): bind XP-01 difficulty selection, persistence, and consumer seams

 Ashfall.Core.Tests/Difficulty/DifficultyFullBindingTests.cs | 186 ++++++++++++
 Assets/Ashfall.Core/Campaign/CrisisPredictionModel.cs       |  48 ++--
 Assets/Ashfall.Core/Disease/DiseaseSystem.cs                |  11 +-
 Assets/Ashfall.Core/Economy/MarketSystem.cs                 |  31 ++-
 Assets/Ashfall.Core/EquipmentConditionSystem.cs              |  10 +-
 Assets/Ashfall.Core/Radiation/RadiationSystem.cs             |  32 ++-
 Assets/Ashfall.Core/Save/CampaignSaveEnvelope.cs             |   9 +
 Assets/Ashfall.Core/Save/SaveSlotService.cs                  |   4 +-
 Assets/Ashfall.Core/Survivors/NeedsSystem.cs                 |  17 +-
 WORKTREE_OWNERSHIP.md                                        |   2 +-
 src/Host/SaveLoadHostSession.cs                              |  24 +-
 src/Main.BriefingCrisis.cs                                   |   8 +-
 src/Main.CampaignServices.cs                                 |   1 +
 src/Main.Difficulty.cs                                       | 147 +++++++++-
 src/Main.GameFlow.cs                                         |  32 ++-
 src/Main.SaveOrchestrator.cs                                 |   9 +-
 src/Main.UiPanels.cs                                          |   8 +-
 src/UI/StartingCohortSetupPanel.cs                            |  62 ++++-
 18 files changed, 600 insertions(+), 41 deletions(-)
```

`git merge-base --is-ancestor 5971c197 HEAD` returns success — this commit is
in the current branch's history, not on an unmerged side branch. This is the
commit that took CF-XP01 from the W1 slice-1 baseline (catalog + selection +
persistence only, per `W1_IMPLEMENTATION_LOG.md`) to full consumer binding.

## 4.2 Focused test execution (this audit, 2026-09-21)

```
$ bash scripts/run_test.sh Ashfall.Core.Tests/Difficulty/DifficultyPresetCatalogTests.cs
Passed! - Failed: 0, Passed: 4, Skipped: 0, Total: 4

$ bash scripts/run_test.sh Ashfall.Core.Tests/Difficulty/DifficultyDirectorTests.cs
Passed! - Failed: 0, Passed: 4, Skipped: 0, Total: 4

$ bash scripts/run_test.sh Ashfall.Core.Tests/Difficulty/DifficultyFullBindingTests.cs
Passed! - Failed: 0, Passed: 7, Skipped: 0, Total: 7
```

`[Fact]` counts by direct `grep -c` on each file corroborate the run output
exactly: `DifficultyPresetCatalogTests.cs` has 4, `DifficultyDirectorTests.cs`
has 4, `DifficultyFullBindingTests.cs` has 7. **True committed total: 15/15
across three files.**

## 4.3 `DifficultyFullBindingTests.cs` fact-by-fact map (all 7, all green)

| Fact | Seam proven | Result |
|---|---|---|
| `Needs_BaseDrift_AppliesHungerAndThirstOnlyAtTheirOwnerSite` | `NeedsSystem` hunger/thirst | PASS |
| `Radiation_EffectiveRate_MultipliesBeforeTheExistingClamp` | `RadiationSystem.ComputeEffectiveRate` | PASS |
| `Disease_OnsetMultiplier_PreservesProbabilityBounds` | `DiseaseSystem.TryExpose` | PASS |
| `Market_DifficultyMultiplier_IsBeforeTheAuthoredPriceClamps` | `MarketSystem.ExplainPrice` | PASS |
| `Equipment_WearMultiplier_ScalesTheUseWearDelta` | `EquipmentConditionSystem.ApplyWear` | PASS |
| `Crisis_DeadlineMultiplier_ScalesRunwayAndPreservesStandardParity` | `CrisisPredictor.Evaluate` | PASS |
| `DifficultyManifestField_IsBoundByTheAggregateChecksum` | `SaveManifest.difficultyPresetId` | PASS |

**Absent from this list, and absent from every other current test file in
`Ashfall.Core.Tests/Difficulty/`: any fact for `HostileEncounterMult` /
`Main.EvolvingWorld.cs`'s `ComposeExpeditionDangerMultiplier`.** This is the
one true gap this plan closes (§18).

## 4.4 The retired closeout's numeric claim versus current reality

The document previously at this path (52 lines, superseded by this plan)
stated:

> `dotnet test --filter Difficulty`: 19/19 PASS (`DifficultyPresetCatalogTests`
> 8/8, `DifficultyDirectorTests` 11/11).

Neither per-file count matches the current files: `DifficultyPresetCatalogTests`
is 4 facts, not 8; `DifficultyDirectorTests` is 4 facts, not 11; and the note
never names `DifficultyFullBindingTests` (7 facts) at all, even though that
file is the one that actually proves the "full binding" the package is named
for. Reading `docs/plans/xp/w1/W1_IMPLEMENTATION_LOG.md`'s earlier
(2026-09-18) verification table shows the likely origin of the confusion: it
records **`DifficultyPresetCatalogTests` + `DifficultyDirectorTests` | PASS —
8/8** as one *combined* total across both files at the W1 slice-1 baseline —
which is consistent with today's 4+4=8. The later closeout note appears to
have mis-read that combined "8" as a per-file count and then invented an
"11/11" second figure with no traceable source. This is the same class of
error already documented for `CF-P5` in this batch (a same-commit "14/14"
register claim against a re-verified "6/6" reality) — a governance-ledger
transcription drift, not a code defect. **Corrected, current, reproducible
figure: 15/15 across three named files** (§4.2). This plan's own §24
Definition of Done cites the corrected figure, not the retired one.

## 4.5 The CLI self-test's 14 checks (verified by direct enumeration, not by execution)

`src/Host/HostCli.Difficulty.cs`'s `RunDifficultySelfTest` contains exactly 14
`Check(name, condition, detail)` calls, enumerated in source order: `catalog`,
`scalar_bounds`, `legacy_parity`, `unknown_fail_closed`,
`starting_bonus_authority`, `starting_bonus_once`, `standard_no_bonus`,
`needs_consumer`, `radiation_consumer`, `disease_consumer`, `market_consumer`,
`equipment_consumer`, `crisis_consumer`, `save_binding`. This matches the
"14/14 PASS" figure in the retired closeout exactly — unlike the `dotnet test`
figure, this specific claim is corroborated by source and is not corrected by
this plan. Note, precisely because the list is enumerable: **there is no
`hostile_consumer` entry.** Running this self-test today (this plan does not
require or perform a live `godot --headless` execution, since no production
code changes yet) would still report 14/14 — the self-test's own count is
internally consistent, it simply never asserted the eighth scalar.

## 4.6 Governance ledger disagreement (reported, not corrected, by this plan)

- `WORKTREE_OWNERSHIP.md`, claim row `claim-xp-wave1-difficulty-2026-09-18`:
  *"**DONE 2026-09-19:** XP-01 full binding complete. 4 catalog presets,
  selection at new game, manifest persistence, 8 scalar consumers... and
  `--difficulty-selftest` 14/14 PASS. Difficulty unit tests 19/19 PASS; port
  contract 264 seams conforming."* (repeats the same 19/19 figure corrected in
  §4.4).
- `INTEGRATION_PLANS.md` (running prose, not the claim table): *"**XP Expansion
  W1 — ACTIVE (2026-09-18):** the user authorized the XP-01 … XP-10 proposal
  and integration plan. The first owned package is
  `XP-WAVE1-DIFFICULTY-AUTHORITY`..."* — narrated in the present tense as
  still active, one day *after* the same package's claim row elsewhere in the
  same document set was marked DONE.
- `AGENTS.md`/`CLAUDE.md` (root, generated 2026-09-19): lists
  `CF-XP01-DIFFICULTY-FULL-BINDING` under **"Available, unexecuted, no new
  foreman signature needed"** in the Active Queue — also contradicted by the
  DONE claim row dated the same day.

Three governance surfaces disagree about the same package's status on the
same date. This plan does not resolve that disagreement (editing those three
files is integrator/foreman territory per `WORKTREE_OWNERSHIP.md`'s existing
claim boundaries); it records the disagreement so the next reader does not
have to re-discover it from scratch, and so this plan itself is not read as
"the feature still needs building" by anyone who only reads the Active Queue.

---

# 5. Existing Extension Seams

The only two extension points this plan's residual delta (§3.2) touches are
already open and already used by seven precedents each:

- **`DifficultyScalarsProvider`'s eight typed properties** — the seam every
  consumer reads from. `HostileEncounterMult` already exists on this type
  (`DifficultyScalarsProvider.cs`); no new property is added.
- **`RunDifficultySelfTest`'s local `Check(name, condition, detail)` closure**
  — a already-generic pattern (14 uses today); a 15th call follows the same
  shape as the existing `needs_consumer`/`radiation_consumer`/etc. entries.
- **`DifficultyFullBindingTests.cs`'s per-seam `[Fact]` pattern** — seven
  existing facts, each isolating one consumer with a standard-vs-harsher
  comparison and a parity assertion; an eighth fact for
  `ComposeExpeditionDangerMultiplier`'s difficulty term follows the identical
  shape.

No new seam is created. Both extension points already exist specifically to
receive exactly this kind of addition.

---

# 6. Proposed Architecture

Unchanged. The architecture is: one JSON catalog authority, one Core director,
one immutable scalar-provider value type, N existing-system consumer seams
each reading one or more scalar properties through an already-owned
`Func<float>`-shaped hook, one campaign-creation selection transaction, one
manifest field, and one CLI self-test aggregating cross-cutting assertions.
This plan's residual delta adds no new class, no new file beyond one test
method's home file (already `DifficultyFullBindingTests.cs`) and one Check
line in an existing method (`RunDifficultySelfTest`). There is no proposed
architecture diagram to draw beyond what §2 already documents as built.

---

# 7. Ownership Matrix

| Concern | Owner (unchanged by this plan) |
|---|---|
| Difficulty IDs and scalar bundles | `Assets/Ashfall.Core/Difficulty/DifficultyPresetCatalog.cs` |
| Preset resolution / fail-closed | `Assets/Ashfall.Core/Difficulty/DifficultyDirector.cs` |
| Typed scalar view | `Assets/Ashfall.Core/Difficulty/DifficultyScalarsProvider.cs` |
| Runtime binding, selection, restore, bonus grant | `src/Main.Difficulty.cs` |
| Hunger/thirst drift | `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` |
| Radiation dose | `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` |
| Disease onset | `Assets/Ashfall.Core/Disease/DiseaseSystem.cs` |
| Market pricing | `Assets/Ashfall.Core/Economy/MarketSystem.cs` |
| Equipment wear | `Assets/Ashfall.Core/EquipmentConditionSystem.cs` |
| Crisis deadlines | `Assets/Ashfall.Core/Campaign/CrisisPredictionModel.cs` + `src/Main.BriefingCrisis.cs` |
| Hostile encounter rate | `src/Main.EvolvingWorld.cs` (`ComposeExpeditionDangerMultiplier`) |
| Manifest field / checksum | `Assets/Ashfall.Core/Save/CampaignSaveEnvelope.cs`, `SaveSlotService.cs` |
| New-game selection UI | `src/UI/StartingCohortSetupPanel.cs` |
| CLI self-test | `src/Host/HostCli.Difficulty.cs`, dispatched from `src/Host/HostCli.cs:343` |
| Catalog integrity | `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` (`ValidateDifficultyPresetCatalog`) |
| Test corpus | `Ashfall.Core.Tests/Difficulty/` |
| **This plan's residual delta** | Adds to `DifficultyFullBindingTests.cs` (new fact) and `HostCli.Difficulty.cs` (new `Check`) only — same owners, no new owner introduced. |
| Governance ledgers (§4.6) | `WORKTREE_OWNERSHIP.md` / `INTEGRATION_PLANS.md` / `AGENTS.md` — foreman/integrator, not edited by this plan. |
| `DifficultySettingsSystem.cs` / `DifficultyConsequenceWeave.cs` (§2.6) | Currently ownerless (uncommitted, unclaimed in `WORKTREE_OWNERSHIP.md`). Named as a risk in §21/§22; not claimed or edited here. |

---

# 8. Data Flow

Unchanged from the shipped implementation; documented here for completeness
since the retired closeout omitted it.

```
difficulty_presets.json
   │  (DifficultyPresetCatalogLoader.Load)
   ▼
DifficultyPresetCatalog (validated, indexed)
   │  (DifficultyDirector.ResolvePreset / ResolveProvider)
   ▼
DifficultyPreset ──► DifficultyScalarsProvider (immutable)
   │                         │
   │  (selection: SelectDifficultyForNewCampaign            (each consumer's
   │   / RestoreDifficultyFromCampaignHeader)                 Func<float> hook)
   ▼                         ▼
_difficultyPresetId    NeedsSystem.Hunger/ThirstRateMultiplier
   │                    RadiationSystem (ComputeEffectiveRate multiplier arg)
   │  (manifest field)  DiseaseSystem.OnsetProbabilityMultiplier
   ▼                    MarketSystem.PriceMultiplierProvider
SaveManifest             EquipmentConditionSystem.WearRateMultiplierProvider
.difficultyPresetId      CrisisPredictionInputs.DeadlineMultiplier (via BriefingCrisis)
   │  (aggregate         ComposeExpeditionDangerMultiplier's hostile term
   │   checksum)             (Main.EvolvingWorld.cs — this plan's delta target)
   ▼
Restore: ValidateDifficultyEnvelope (fail-closed)
   → ApplyDifficultyFromLoadedManifest → SetupDifficulty → BindDifficultyConsumers
```

Fresh creation and restore both terminate at the same
`SetupDifficulty()`/`BindDifficultyConsumers()` pair (`Main.SaveOrchestrator.cs:179,302`
for restore; the equivalent calls inside `Main.CampaignServices.cs`'s
`ComposeCampaign()` for fresh creation, per the CF-P28 evidence in this same
plan batch), so there is one binding order, not two.

---

# 9. State Model

Five host fields (`_difficulty`, `_difficultyCatalog`, `_difficultyScalars`,
`_difficultyPresetId`, `_difficultyBonusesGrantedForCampaign`) plus one
persisted string (`SaveManifest.difficultyPresetId`) constitute the entire
difficulty state model for a campaign. There is no collection, no per-day
history, and no second copy of "which preset is this campaign on" anywhere in
the reachable, host-wired code (§2.6 flags the one place a second copy *could*
appear if `DifficultySettingsSystem.cs` were ever wired without
reconciliation — it is not wired today, so it is not part of the current state
model). This plan's residual delta adds zero new state.

---

# 10. API/Contracts

No public API changes. The residual delta (§3.2) adds:

- One new `[Fact]` method to the existing `Ashfall.Core.Tests.Difficulty.DifficultyFullBindingTests`
  class — a test-only addition, not a production contract change.
- One new `Check("hostile_consumer", ..., ...)` call inside the existing
  `RunDifficultySelfTest` method body — a self-test-only addition; the method
  signature `public static int RunDifficultySelfTest(string dataDirectory)`
  and its `EmitSummary` contract are unchanged.

No existing method signature, property, event, or return type in
`DifficultyPresetCatalog.cs`, `DifficultyDirector.cs`,
`DifficultyScalarsProvider.cs`, or `Main.Difficulty.cs` changes.

---

# 11. Data Changes

None. `difficulty_presets.json`'s schema, four presets, and eight scalar
fields per preset are unchanged. No new field, no new preset, no new
`schema_version`.

---

# 12. Save/Load

Unchanged. `SaveManifest.difficultyPresetId` keeps its current shape and
checksum participation. The residual delta does not touch save code; the new
Core fact exercises the existing production seam
(`ComposeExpeditionDangerMultiplier`'s hostile term) directly, not through a
save round-trip, matching how the other seven `DifficultyFullBindingTests`
facts are written (direct calculation comparison, not save/restore
round-trips — save/restore coverage for the field itself is the eighth
existing fact, `DifficultyManifestField_IsBoundByTheAggregateChecksum`, which
this plan does not duplicate).

---

# 13. Determinism

Unchanged and already proven: every consumer seam is a pure multiplication of
an existing deterministic calculation by a `float` read from an immutable
provider; no consumer seam introduces `System.Random`, wall-clock time, or
iteration-order-dependent state. The disease consumer test
(`Disease_OnsetMultiplier_PreservesProbabilityBounds`) explicitly seeds its RNG
(`new SeededRng(7)`) and the CLI self-test's disease/equipment checks likewise
seed with `new SeededRng(31)` — the existing pattern this plan's new fact will
follow if the added hostile-consumer test needs any upstream randomness (it
does not: `ComposeExpeditionDangerMultiplier`'s difficulty term is pure
arithmetic on scalar inputs, same as the market and equipment tests).

---

# 14. System/Event Wiring

Unchanged. `BindDifficultyConsumers()` remains the five-consumer host wiring
point for Core-level `Func<float>` hooks; the crisis and hostile-encounter
seams remain wired from their own owning partials
(`Main.BriefingCrisis.cs`, `Main.EvolvingWorld.cs`) rather than being folded
into `BindDifficultyConsumers()` — this plan does not propose moving them,
since doing so would be a pure refactor with no behavior change and no test
benefit, and AGENTS.md's minimum-safe-change guidance argues against touching
working wiring without a reason.

---

# 15. Godot Integration

Unchanged. `src/UI/StartingCohortSetupPanel.cs` already renders all four
authored presets from `_difficultyCatalog.AllPresets`, selects via
`SelectDifficulty(presetId)`, and exposes `DifficultyPresetId` on its result
type for the new-game transaction to read. `src/Host/HostCli.cs:343,619`
already dispatches and documents `--difficulty-selftest`. No panel, scene, or
CLI surface changes.

---

# 16. Narrative/Content Integration

None required or proposed. Preset `description`/`description_key` fields are
already authored and already read by the panel's `DescribeDifficulty` helper;
no new diegetic text is needed for a coverage-only test addition.

---

# 17. Failure Modes

Unchanged for the sealed scope; already covered by `unknown_fail_closed`
(self-test) and the loader's throw-on-invalid behavior (unit tests). The one
failure mode this plan's residual delta specifically targets: **a silent
regression in the hostile-encounter difficulty term would currently pass every
existing gate** (`dotnet build`, all 15 committed difficulty facts, the
14-check self-test, `--data-integrity-selftest`, `--catalog-boot-preflight`)
because none of them exercise
`Main.EvolvingWorld.cs`'s difficulty multiplication. A future refactor of
`ComposeExpeditionDangerMultiplier` (for example, reordering the multiplier
chain, or removing the `SetupDifficulty()` call believing it dead) would ship
green today. This is the concrete risk the new fact and self-test check
remove.

---

# 18. Test Strategy

## 18.1 The one new Core fact

Add to `Ashfall.Core.Tests/Difficulty/DifficultyFullBindingTests.cs`, following
the exact shape of the other seven facts:

```csharp
[Fact]
public void HostileEncounter_DifficultyMultiplier_ScalesExistingDangerComposition_AndPreservesStandardParity()
{
    // Mirrors the arithmetic in Main.EvolvingWorld.cs:
    // ComposeExpeditionDangerMultiplier — a pure multiply, isolated from Godot.
    float baseComposition = 1.32f; // representative pre-difficulty composed value
    float standardMult = DifficultyScalarsProvider.Legacy.HostileEncounterMult; // 1f
    float dirgeMult = 1.75f; // difficulty_dirge's authored hostile_encounter_mult

    float standardResult = ApplyHostileTerm(baseComposition, standardMult);
    float dirgeResult = ApplyHostileTerm(baseComposition, dirgeMult);

    Assert.Equal(baseComposition, standardResult, 4); // standard preserves parity exactly
    Assert.Equal(baseComposition * dirgeMult, dirgeResult, 4);
    Assert.True(dirgeResult > standardResult);

    // Local mirror of the production epsilon-guarded multiply in
    // Main.EvolvingWorld.cs:194-195, so this fact tracks the real guard
    // condition rather than a simplified stand-in.
    static float ApplyHostileTerm(float mult, float hostile)
    {
        if (hostile > 0f && Math.Abs(hostile - 1f) > 0.001f) mult *= hostile;
        return mult;
    }
}
```

This exercises the exact guarded-multiply expression from
`Main.EvolvingWorld.cs:194-195` as a local mirror (the production method
itself is a private instance method on the Godot-referencing `Main` partial
and cannot be unit-invoked from `Ashfall.Core.Tests` without a Godot host —
consistent with how the crisis-deadline fact tests `CrisisPredictor.Evaluate`
directly rather than `Main.BriefingCrisis.cs`'s one-line wiring, and how the
market/equipment facts test the Core owner rather than any host wrapper). It
proves the arithmetic contract; the self-test check in §18.2 proves the
*wiring* is live inside the actual Godot host.

## 18.2 The one new self-test check

Add to `RunDifficultySelfTest` in `src/Host/HostCli.Difficulty.cs`, after the
existing `crisis_consumer` check and before `save_binding` (or anywhere in the
existing sequence — order is not asserted anywhere):

```csharp
float standardHostile = standard.HostileEncounterMult;
float dirgeHostile = dirge.HostileEncounterMult;
Check("hostile_consumer",
    Math.Abs(standardHostile - 1f) < 0.001f && dirgeHostile > standardHostile,
    $"authored hostile_encounter_mult differs by preset ({standardHostile:0.###}/{dirgeHostile:0.###})");
```

This brings the self-test from 14 to 15 checks. It re-uses the already-in-scope
`standard`/`dirge` `DifficultyScalarsProvider` locals the method already
constructs for the other consumer checks — no new catalog load, no new
director instance.

## 18.3 Regression order (per `TEST_POLICY.md`/`AGENTS.md`)

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Difficulty/DifficultyFullBindingTests.cs`
   alone first — must show 8/8 (7 existing + the new fact) with zero
   regressions in the seven existing facts.
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Difficulty/DifficultyPresetCatalogTests.cs`
   and `DifficultyDirectorTests.cs` — unaffected, must remain 4/4 each (no
   change proposed to either file).
3. `dotnet build Ashfall.csproj` — 0 errors/warnings.
4. `godot --headless --path . -- --difficulty-selftest` — must report 15/15
   (was 14/14).
5. `godot --headless --path . -- --data-integrity-selftest` — unaffected;
   confirms the catalog integrity validator still passes with no data change.
6. No full-suite run. No new test file (the addition lives inside the
   existing `DifficultyFullBindingTests.cs`, per AGENTS.md's targeted-testing
   guidance to prefer extending an existing focused file over creating a new
   one for a single fact).

---

# 19. Dependency-Ordered Phases

**Phase 0 — Documentation (this plan).** No code. Replace the retired
closeout note with this document. No claim beyond the already-existing
`claim-xp-wave1-difficulty-2026-09-18` row is required, since this phase edits
only the plan file that row already lists.

**Phase 1 — Coverage gap closure (requires a foreman micro-claim or explicit
reuse authorization of the existing W1 claim, since it edits two files listed
under that row).**
1. Add the one Core fact (§18.1) to `DifficultyFullBindingTests.cs`. Run it
   alone; must be 8/8 with no regression.
2. Add the one self-test check (§18.2) to `HostCli.Difficulty.cs`. Run
   `--difficulty-selftest`; must be 15/15.
3. `dotnet build` clean.
4. Record the corrected 15/15 (unit) and 15/15 (self-test) figures wherever
   the foreman/integrator next touches `WORKTREE_OWNERSHIP.md`'s claim row
   (this plan does not self-edit that ledger; see §22).

There is no Phase 2. The catalog, director, provider, manifest field, panel,
and six other consumer seams are not phased work in this plan — they are
already-shipped prerequisites documented in §2.

---

# 20. File Impact Map

| File | Change |
|---|---|
| `docs/plans/CF_XP01_DIFFICULTY_FULL_BINDING_INTEGRATION_PLAN.md` | Replaced (this document) |
| `Ashfall.Core.Tests/Difficulty/DifficultyFullBindingTests.cs` | +1 `[Fact]` (Phase 1) |
| `src/Host/HostCli.Difficulty.cs` | +1 `Check(...)` line inside `RunDifficultySelfTest` (Phase 1) |

No other file is touched. In particular, **not touched**:
`difficulty_presets.json`, `DifficultyPresetCatalog.cs`, `DifficultyDirector.cs`,
`DifficultyScalarsProvider.cs`, `Main.Difficulty.cs`, any of the six other
consumer owner files, `CampaignSaveEnvelope.cs`, `StartingCohortSetupPanel.cs`,
`CatalogIntegrityValidator.cs`, `WORKTREE_OWNERSHIP.md`,
`INTEGRATION_PLANS.md`, `AGENTS.md`/`CLAUDE.md`,
`DifficultySettingsSystem.cs`, `DifficultyConsequenceWeave.cs`, or either of
their test files.

---

# 21. Risks

1. **Ledger-trust risk (already realized once, per §4.4):** a future reader
   trusts a governance ledger's cited test count instead of re-running the
   suite. Mitigation: this plan cites the corrected, reproducible figure and
   shows the exact commands used to get it (§4.2), same discipline as CF-P5.
2. **Latent parallel-authority risk (§2.6):** `DifficultySettingsSystem.cs`
   remains uncommitted and unwired today, so the risk is *dormant*, not
   *active*. If any future package wires it — or `DifficultyConsequenceWeave.cs`
   — into a host without first reconciling `DifficultySettingsSystem`'s
   `DifficultySettingsState` against `Main.Difficulty.cs`'s
   `_difficultyPresetId`/`_difficultyScalars` fields and
   `CampaignSaveEnvelope.difficultyPresetId`, the campaign would gain two
   independent difficulty-selection state holders. Mitigation: this plan names
   the risk explicitly (§22) so the next package that touches either file
   inherits the warning instead of re-discovering it from a cold read.
3. **Governance-ledger disagreement risk (§4.6):** three documents disagree
   about whether CF-XP01 is active or done. Mitigation: named here for the
   foreman; not corrected by this plan, to respect the existing ownership
   boundary on those three files.
4. **Low risk, low blast radius of the residual delta itself:** the new fact
   and self-test check are pure additions to already-green files; the worst
   failure mode is "the new check fails," which is caught by Phase 1's own
   run-alone-first step before anything is considered complete.

---

# 22. Out of Scope

- Reconciling, wiring, adopting, or retiring `DifficultySettingsSystem.cs`
  (Plan 181) or `DifficultyConsequenceWeave.cs` (EN-01/UNBLOCK-05). AGENTS.md's
  Active Queue explicitly still lists `EN-01…EN-08 proposals` as
  decision-blocked ("never start without the named signature"); this plan
  respects that block rather than treating the file's mere presence in the
  working tree as authorization.
- Correcting `WORKTREE_OWNERSHIP.md`, `INTEGRATION_PLANS.md`, or
  `AGENTS.md`/`CLAUDE.md`'s Active Queue prose (§4.6). Reported for the
  foreman/integrator, not self-authorized here.
- Any mid-campaign difficulty change, slider, or lock UI.
- Any change to the four authored presets' scalar values or to
  `starting_bonus_item_ids`.
- Any change to `CampaignCompletionHistory`/`CompletionHistoryStore`/
  `Main.Endgame` (Wave 11-owned, explicitly excluded by the original W1 claim
  and unchanged by this plan).
- A live `godot --headless -- --difficulty-selftest` execution as part of
  *this planning pass* — no production code changed yet, so there is nothing
  new for it to catch; §4.5's 14-check enumeration is by direct source
  reading, and the plan's Phase 1 (§19) specifies running it after the one
  new check is added, not before.

---

# 23. Rollback Strategy

Phase 1's two additions are each a single, independent, additive block (one
test method, one `Check` line) with no dependency the rest of the file relies
on. Rollback is `git checkout -- Ashfall.Core.Tests/Difficulty/DifficultyFullBindingTests.cs src/Host/HostCli.Difficulty.cs`
(or a revert of the single commit that adds them), which restores the file to
its current 7-fact/14-check sealed state with zero side effects, since
neither addition is referenced from any other file. There is no data
migration, no save-format change, and no phased dependency chain to unwind.

---

# 24. Definition of Done

- [x] (Already true, re-verified this pass) Four catalog presets load,
  validate, and resolve through `DifficultyDirector`; an unknown persisted ID
  fails closed before restore.
- [x] (Already true, re-verified this pass) All eight authored scalars have
  exactly one existing production consumer owner each, including
  `hostile_encounter_mult` at `Main.EvolvingWorld.cs:193-195`.
- [x] (Already true, re-verified this pass) The sparing bonus grants
  `canned_food` + `iodine_pills` exactly once, through canonical inventory.
- [x] (Already true, re-verified this pass) `difficultyPresetId` round-trips
  through the campaign manifest and participates in the aggregate checksum.
- [x] (Corrected by this plan, §4.4) The historical "19/19" test-count claim
  is replaced with the reproducible, current figure: **15/15** across
  `DifficultyPresetCatalogTests` (4/4), `DifficultyDirectorTests` (4/4), and
  `DifficultyFullBindingTests` (7/7).
- [ ] (This plan's Phase 1, pending foreman claim) `DifficultyFullBindingTests`
  reaches 8/8 with the new hostile-consumer fact; the CLI self-test reaches
  15/15 with the new `hostile_consumer` check.
- [x] Completion-history paths remain untouched and Wave-11 owned.
- [x] `DifficultySettingsSystem.cs`/`DifficultyConsequenceWeave.cs` remain
  unwired and are named, not absorbed, in this plan.

---

# 25. Implementation Handoff

## MUST PRESERVE

- The single-authority shape: one catalog (`difficulty_presets.json`), one
  director, one immutable scalar-provider type, one campaign-creation
  selection transaction, one manifest field.
- All six already-tested consumer seams' current multiply-before-existing-clamp
  ordering (needs, radiation, disease, market, equipment, crisis) — none of
  them are touched by this plan's delta.
- `Main.EvolvingWorld.cs`'s existing epsilon-guarded hostile-multiply
  expression (`hostile > 0f && Math.Abs(hostile - 1f) > 0.001f`) exactly as
  written; the new fact mirrors it, it does not refactor it.
- The fresh-creation/restore binding symmetry
  (`SetupDifficulty()` → `BindDifficultyConsumers()`, called from both paths).
- The exclusion of `CampaignCompletionHistory`/`CompletionHistoryStore`/
  `Main.Endgame` from this package.
- The dormant, unwired state of `DifficultySettingsSystem.cs` and
  `DifficultyConsequenceWeave.cs` — neither is connected to a host by this
  plan or its Phase 1.

## MUST ADD

- One `[Fact]` to `Ashfall.Core.Tests/Difficulty/DifficultyFullBindingTests.cs`
  proving the hostile-encounter difficulty term (§18.1).
- One `Check("hostile_consumer", ...)` line inside
  `src/Host/HostCli.Difficulty.cs`'s `RunDifficultySelfTest` (§18.2).
- Nothing else.

## MUST NOT DO

- Do not re-author, rename, or rescale any of the four presets or their eight
  scalar fields.
- Do not move the crisis or hostile-encounter wiring into
  `BindDifficultyConsumers()`; leave them owned by their current partials.
- Do not wire, construct, or reference `DifficultySettingsSystem` or
  `DifficultyConsequenceWeave` from any file under `src/`.
- Do not edit `WORKTREE_OWNERSHIP.md`, `INTEGRATION_PLANS.md`, or
  `AGENTS.md`/`CLAUDE.md` to resolve the §4.6 disagreement; route it to the
  foreman/integrator instead.
- Do not touch `CampaignCompletionHistory.cs`, `CompletionHistoryStore.cs`, or
  `Main.Endgame.cs`.
- Do not run the full test suite; use the focused commands in §18.3 only.
- Do not use `System.Random`, wall-clock seeds, or non-deterministic state in
  the new fact or check.

## VERIFY WITH

- `bash scripts/run_test.sh Ashfall.Core.Tests/Difficulty/DifficultyFullBindingTests.cs`
  — alone first, expect 8/8 after Phase 1 (7/7 today, re-verified this pass).
- `bash scripts/run_test.sh Ashfall.Core.Tests/Difficulty/DifficultyPresetCatalogTests.cs`
  — expect 4/4 (unchanged).
- `bash scripts/run_test.sh Ashfall.Core.Tests/Difficulty/DifficultyDirectorTests.cs`
  — expect 4/4 (unchanged).
- `dotnet build Ashfall.csproj` — 0 errors/warnings.
- `godot --headless --path . -- --difficulty-selftest` — expect 15/15 after
  Phase 1 (14/14 today, per source enumeration in §4.5).
- `godot --headless --path . -- --data-integrity-selftest` — unaffected,
  expect continued pass.
- `git diff --check` — clean.

## FIRST SAFE IMPLEMENTATION STEP

**Phase 0, read-only, no claim required beyond this document:** nothing —
this plan document itself *is* Phase 0, and it is complete once saved. The
first step requiring a foreman micro-claim is Phase 1 §19.1: add the single
new `[Fact]` to `DifficultyFullBindingTests.cs` (§18.1), run it alone, confirm
8/8 with the seven existing facts unchanged, and only then proceed to the
self-test `Check` addition (§18.2). Nothing in the catalog, director,
provider, manifest, or the other six consumer files is touched at any point.

---

*Plan authored 2026-09-21 directly (no subagent dispatch) against branch
`feat/unblock-cf-p28-and-plan-implementation`, superseding the 3,945-character
closeout note previously at this path. All file:line citations, test-run
output, and git history in this document were produced by direct reads,
`git log`/`git show`/`git status`/`git diff`, and `bash scripts/run_test.sh`
executions performed during authoring; re-verify at claim time per §25
`VERIFY WITH`.*
