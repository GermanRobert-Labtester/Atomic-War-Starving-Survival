# C1 — Flagship Integration Plan [32]: Difficulty Profiles, Assistive Tuning & Canonical Challenge Modifiers

> **Output:** `C1_planintegration[32].md`
>
> **Source baseline:** Plan 181 — Difficulty Settings System
>
> **Primary mission:** give ASHFALL a configurable challenge layer that supports accessible presets and advanced custom tuning while keeping radiation, needs, raids, economy, expeditions, weather, crafting, mortality consequences, and New Game+ modifiers inside their canonical authorities.
>
> **Primary architectural rule:** difficulty owns **player-selected challenge configuration, preset identity, lock policy, change history, and a typed read-only modifier profile**. It does not own radiation dose, needs decay, raid scheduling, market prices, expedition outcomes, weather damage, crafting time/cost, survivor death, loot generation, or NG+ challenge state.
>
> **Primary accessibility rule:** difficulty is not a synonym for accessibility. Players must be able to soften challenge through difficulty settings without hiding accessibility features behind Easy mode, and accessibility options must remain independently available at every difficulty.
>
> **Primary balance rule:** presets are curated configuration bundles, not separate gameplay rule forks. Custom mode changes the same typed parameters. Normal remains the canonical baseline profile with neutral values.
>
> **Mandatory execution order:** 181A authority + modifier-surface audit → 181B canonical difficulty profile and preset contract → 181C typed adapters into existing systems → 181D campaign lock/change semantics + NG+ composition → 181E UI/accessibility/preset comparison → 181F persistence/migration/determinism/telemetry/balance → 181G custom-profile sharing only if file/content-pack infrastructure supports it.
>
> **Critical re-baseline rule:** before creating `DifficultySettingsSystem`, inspect `CampaignCalendar`, `NeedsSystem`, `RadiationSystem`, raid scheduling/spawn authority, `MarketSystem`, `ExpeditionSystem`, weather authority, crafting authority, loot/resource generation, survivor death/grief/palliative consequences, Plan-175 NG+ challenge modifiers, Plan-34 accessibility parity, save orchestration, bootstrap/composition root, runtime settings UI, and any existing debug/tuning multipliers. Consolidate existing knobs instead of layering duplicates.
>
> **Critical source correction:** the source models every axis as a simple 0.5–2.0 multiplier and suggests direct subsystem multiplication. That is unsafe for several domains. Raid frequency, resource scarcity, death penalty, economic pressure, weather severity, and crafting difficulty may each require domain-specific typed parameters rather than one universal scalar. The flagship therefore exposes a **typed difficulty profile** whose fields are validated by the owning systems.
>
> **Guardrails:** no difficulty-owned simulation state; no direct price multiplication in difficulty code; no direct raid spawn chance mutation from arbitrary UI slider; no difficulty-owned loot table; no difficulty-owned death/grief system; no hardcoded 0.5–2.0 assumption for every domain; no settings that silently invalidate authored content; no live difficulty changes that retroactively reroll already-generated world state; no save-scumming through difficulty changes immediately before known outcomes unless deliberately allowed; no lock flag that can be bypassed through config files without validation if campaign integrity matters; no achievements tied to derogatory names; no difficulty selection as an accessibility gate; no HUD clutter if a persistent difficulty badge has no gameplay value; no per-frame modifier recomputation; no unversioned presets; no unseeded RNG; no direct duplication of Plan-175 NG+ modifiers.

---

# 0. Mission

ASHFALL currently runs at one fixed challenge level.

The source baseline reports:
- no `DifficultySetting`;
- no `DifficultyLevel`;
- no `GameDifficulty`;
- no `DifficultyModifier`;
- Plan 175 only adds New Game+ challenge modifiers;
- Plan 34 explicitly says difficulty presets must not be the only way to soften the game.

That is a real usability and replayability gap.

The target architecture is:

```text
CAMPAIGN SETUP / SETTINGS
         │
         ├── preset
         ├── custom profile
         └── optional lock
                │
                ▼
DifficultySettingsSystem
                │
                ├── selected profile
                ├── preset/custom identity
                ├── lock state
                ├── change history
                └── typed modifier read model
                │
                ▼
DIFFICULTY MODIFIER PROVIDER
                │
                ├────────► RadiationSystem
                ├────────► NeedsSystem
                ├────────► Raid authority
                ├────────► loot/resource authority
                ├────────► MarketSystem
                ├────────► ExpeditionSystem
                ├────────► WeatherSystem
                ├────────► CraftingSystem
                ├────────► death/grief consequence authority
                └────────► Plan-175 NG+ composition
```

Difficulty should answer:

> Which challenge profile did this campaign select, which modifiers are active, can the player change them, and what typed tuning inputs should each existing domain authority read?

Difficulty should not answer:

> What does radiation equal?
> When does a raid happen?
> What item drops?
> What does a survivor death do?
> What is the market price?
> What is the weather outcome?

Those remain the domain systems.

---

# 1. Source-Evidence Interpretation

## 1.1 No general difficulty system exists

The source reports zero Core matches for difficulty-setting concepts.

A dedicated configuration authority is justified.

## 1.2 Plan 175 is not a substitute

NG+ challenge modifiers are:
- meta-progression/challenge modifiers.

Base difficulty is:
- campaign configuration.

The two should compose through one modifier pipeline without becoming the same feature.

## 1.3 Plan 34 creates an accessibility obligation

Difficulty can change challenge.

Accessibility features should independently address:
- readability;
- input;
- timing presentation;
- reduced motion;
- cognitive load;
- warning clarity;
- assistive UI.

Do not force accessibility into Easy.

## 1.4 Not every domain is a scalar

Examples:

### Radiation
A scalar exposure-rate modifier may be reasonable.

### Needs
A decay-rate multiplier may be reasonable.

### Raid frequency
A scalar may be wrong if raids use:
- minimum spacing;
- event budgets;
- threat thresholds;
- story gates.

### Resource scarcity
May need:
- starting bundle factor;
- scavenging yield factor;
- merchant availability factor
rather than one division.

### Survivor death penalty
Death itself should not be scaled.
Possible affected consequences:
- morale/grief intensity;
- memorial/resource fallout
only if those systems explicitly expose challenge tuning.

### Weather severity
Could mean:
- event weights;
- intensity bands;
- damage coupling.
A universal multiplier may break content.

### Crafting difficulty
Time and cost are distinct.
One slider changing both may over-constrain player choice.

## 1.5 Presets should be data, but modifier semantics must be code-validated

`difficulty_presets.json` is appropriate.

However:
- field names must map to registered typed axes;
- no arbitrary dictionary keys silently ignored.

## 1.6 Custom configuration naming/sharing is follow-on scope

Saving a named custom profile locally is reasonable if settings persistence already supports it.

Sharing profiles requires:
- export/import;
- schema/version compatibility;
- security/validation.

That belongs under Plan 47/content-pack/file contract if not already present.

---

# 2. Non-Negotiable Difficulty Invariants

## INV-181.1 — One campaign difficulty authority

Preset/custom/lock state has one owner.

## INV-181.2 — Normal is canonical baseline

Neutral profile reproduces current tuned gameplay.

## INV-181.3 — Domain systems own simulation truth

Difficulty provides inputs only.

## INV-181.4 — No direct cross-domain mutation

Difficulty never writes:
- radiation dose;
- hunger;
- raid event;
- price;
- loot;
- weather damage;
- craft completion;
- death consequence.

## INV-181.5 — Every difficulty axis has one canonical consumer

No slider without a real downstream read.

## INV-181.6 — Difficulty axis semantics are typed

No universal `"modifier": 1.5` dictionary interpreted ad hoc.

## INV-181.7 — Presets and Custom use the same axis registry

No separate code paths.

## INV-181.8 — Preset selection is deterministic

No RNG.

## INV-181.9 — Difficulty changes do not rewrite past outcomes

They affect future evaluations unless explicitly documented.

## INV-181.10 — Locked campaigns reject changes in Core

UI disablement alone is insufficient.

## INV-181.11 — Lock state is persistent

## INV-181.12 — Old saves load as baseline Normal

No retroactive difficulty shock.

## INV-181.13 — NG+ modifiers compose once

No double application.

## INV-181.14 — Difficulty and accessibility remain independent

## INV-181.15 — Custom axes are bounded by domain-safe ranges

Not necessarily 0.5–2.0.

## INV-181.16 — Unsupported slider values fail validation

No silent clamp without user feedback during configuration import.

## INV-181.17 — Active profile is immutable read state during a simulation transaction

Avoid mid-operation changes.

## INV-181.18 — Mid-campaign changes are source-attributed

History records:
- old profile;
- new profile;
- campaign day.

## INV-181.19 — Difficulty identity does not control achievements by display name

Use stable profile fingerprint/preset ID.

## INV-181.20 — No per-frame difficulty work

Systems read cached/current profile.

---

# 3. Definition of Done

Plan 181 closes only when:

- all candidate difficulty axes are audited against real domain authorities;
- current hidden/debug/tuning multipliers are inventoried;
- one difficulty authority exists;
- preset catalog is versioned;
- Easy/Normal/Hard/Nightmare are authored as data;
- Custom uses the same schema;
- each axis has typed semantics and safe bounds;
- any source-proposed axis with no real consumer is deferred;
- Normal reproduces baseline gameplay within test tolerance;
- radiation tuning reaches RadiationSystem through one adapter;
- needs tuning reaches NeedsSystem through one adapter;
- raid tuning reaches the canonical raid scheduler/spawner through one adapter;
- resource scarcity reaches actual starting-resource/loot authorities without double-applying;
- economic pressure reaches MarketSystem through a supported policy/input seam rather than direct price multiplication;
- expedition danger reaches canonical expedition hazard/risk calculations;
- weather severity reaches the real weather/event/hazard authority;
- crafting difficulty reaches craft-time and/or material-cost authority through explicit separate parameters where needed;
- death consequence tuning affects only defined downstream consequences and does not change death truth;
- optional campaign lock is enforced in Core;
- unlocked campaigns may change settings only at safe transaction boundaries;
- changes cannot reroll already-committed outcomes;
- NG+ modifiers compose exactly once with base difficulty;
- old saves receive Normal baseline;
- profile fingerprints are stable;
- save/load round-trip passes;
- custom profile values persist;
- invalid preset keys/values fail integrity checks;
- UI clearly explains what every axis changes;
- accessibility options remain available independently;
- 0.5×/2.0× extremes are used only for axes where validated;
- 30/120/180-day simulations compare presets across survival, economy, raids, expeditions, weather, and crafting;
- `--difficulty-selftest` exists or equivalent;
- all presets are covered by deterministic golden snapshots;
- content/reward/quest reachability remains intact at every shipped preset.

---

# 4. Phase P0 — Difficulty Surface & Authority Audit

## P0.1 Capture baseline

Record:

```text
commit SHA
branch
dirty paths
CampaignCalendar
NeedsSystem
RadiationSystem
raid scheduling/spawn authority
loot/scavenge generation authority
starting inventory bootstrap
MarketSystem
ExpeditionSystem
WeatherSystem
CraftingSystem
death/grief/memorial authorities
Plan-175 NG+ modifier implementation
Plan-34 accessibility settings
game settings persistence
campaign save settings
composition root/bootstrap
debug tuning constants
content difficulty assumptions
```

## P0.2 Build difficulty authority matrix

Create:

`docs/difficulty/DIFFICULTY_AUTHORITY_MATRIX.md`

Columns:

```text
axis
canonical consumer
current tuning seam
current bounds
difficulty adapter
stateful side effects?
status
```

Rows:
- radiation exposure;
- need decay;
- raid cadence;
- raid severity if separate;
- starting resources;
- loot yield;
- economic pressure;
- price volatility;
- expedition hazard;
- weather intensity;
- crafting time;
- crafting material cost;
- survivor death consequence;
- recovery forgiveness if any;
- NG+ modifiers.

## P0.3 Find pre-existing knobs

Search:
- multipliers;
- debug config;
- environment constants;
- hardcoded tuning.

Disposition:
- promote to typed axis;
- leave domain-local;
- delete duplicate.

## P0.4 Baseline golden

Capture current Normal-like run:

```text
fixed seed
fixed campaign path
30 days
```

Metrics:
- needs;
- radiation;
- raids;
- loot;
- market;
- expedition incidents;
- weather;
- crafting.

This becomes the Normal parity baseline.

## P0.5 Accessibility separation audit

Inventory current accessibility controls.

Ensure difficulty does not become the only means to:
- slow information presentation;
- increase warnings;
- improve text;
- reduce motion;
- simplify input.

---

# TASK 181A — Difficulty Profile, Preset Catalog & Axis Registry

# 181A.0 Goal

Create a typed, versioned, immutable-at-read challenge profile.

## 181A.1 Proposed namespace

`Assets/Ashfall.Core/Difficulty/`

## 181A.2 Core owner

`DifficultySettingsSystem`

Owns:
- selected preset ID;
- custom values;
- lock state;
- change history;
- profile fingerprint.

## 181A.3 Avoid raw dictionary as runtime truth

Source proposes:

```text
modifiers: dict setting → value
```

Prefer typed DTO:

```text
DifficultyProfile
  radiation
  needs
  raids
  resources
  economy
  expedition
  weather
  crafting
  death_consequence
```

Each can itself be a typed sub-profile.

## 181A.4 Radiation profile

Possible:

```text
exposure_rate_multiplier
```

Only if RadiationSystem exposes it.

## 181A.5 Needs profile

Possible:

```text
hunger_decay_multiplier
thirst_decay_multiplier
fatigue_decay_multiplier
```

Could share one UI axis while internal profile expands safely.

## 181A.6 Raid profile

Possible:

```text
cadence_multiplier
threat_budget_multiplier
minimum_spacing_multiplier
```

Do not assume spawn chance only.

## 181A.7 Resource profile

Possible:

```text
starting_resource_multiplier
scavenge_yield_multiplier
rare_loot_weight_multiplier
```

Only actual consumers.

## 181A.8 Economy profile

Possible:

```text
scarcity_pressure_multiplier
volatility_multiplier
```

No raw final price multiplier unless MarketSystem explicitly models it.

## 181A.9 Expedition profile

Possible:

```text
hazard_weight_multiplier
incident_severity_multiplier
```

Only real risk dimensions.

## 181A.10 Weather profile

Possible:

```text
severe_event_weight_multiplier
hazard_intensity_multiplier
```

Avoid multiplying temperature itself blindly.

## 181A.11 Crafting profile

Separate:

```text
craft_time_multiplier
material_cost_multiplier
```

Source's one “crafting difficulty” slider may map to both through preset curves, but advanced Custom may expose them separately if UI budget permits.

## 181A.12 Death consequence profile

Audit.

Possible:
- grief intensity;
- memorial morale duration;
- recovery forgiveness.

Do not modify:
- whether dead survivor is dead.

## 181A.13 Axis registry

Create typed registry:

```text
DifficultyAxisDefinition
  axis_id
  display_key
  description_key
  min
  max
  neutral
  step
  consumer_id
  ui_group
  accessibility_note
```

## 181A.14 Axis ID stability

Stable IDs.

## 181A.15 Preset catalog

`Assets/StreamingAssets/Data/difficulty_presets.json`

## 181A.16 Preset DTO

```text
id
display_key
description_key
axis_values
recommendation_key
tags[]
```

Data dictionary is acceptable at the **catalog boundary** if loader validates every key against typed registry and materializes a typed profile.

## 181A.17 Four presets

IDs:

```text
easy
normal
hard
nightmare
```

Use a non-derogatory display name if Nightmare remains consistent with game tone.

## 181A.18 Normal

Every axis neutral.

## 181A.19 Easy

Use source values only as initial tuning candidates.

Do not freeze:
- 0.5 radiation;
- 0.5 raids;
- 1.5 resources;
etc.
until simulations pass.

## 181A.20 Hard

Same.

## 181A.21 Nightmare

Same.

## 181A.22 Custom

Custom is not a preset definition that copies a stale set.

It is:
- user-authored axis values;
- optional base preset provenance.

## 181A.23 Profile fingerprint

Stable canonical serialization hash of:
- axis IDs/values;
- preset base;
- NG+ challenge composition version if needed.

## 181A.24 Preset version

Store catalog/schema version.

## 181A.25 Unknown future axis

Old preset file:
- neutral fallback only through explicit migration/default rule.

No silent arbitrary values.

## 181A.26 Unknown preset ID

Save restores:
- resolved snapshot;
- warning/fallback.

## 181A.27 Read API

Example:

```text
GetProfile()
GetAxis<T>()
GetPresetId()
IsLocked
```

## 181A.28 Write API

Only:

```text
SelectPreset(...)
ApplyCustomProfile(...)
SetLockedAtCampaignStart(...)
TryChangeProfile(...)
```

## 181A.29 No subsystem setter spree

Systems should receive:
- provider/interface;
or immutable profile snapshot.

## 181A.30 Generated docs

Create:

`DIFFICULTY_AXIS_MATRIX.md`
`DIFFICULTY_PRESET_MATRIX.md`

### 181A DoD

Difficulty becomes one typed configuration authority with data-driven presets, validated axes, neutral baseline semantics, and no direct simulation ownership.

---

# TASK 181B — Typed Domain Adapters

# 181B.0 Goal

Apply challenge tuning once, at each real domain boundary.

---

# 181B-RAD — Radiation

## 181B.RAD1 Audit accumulation path

Find exact exposure formula.

## 181B.RAD2 Difficulty input

Prefer one multiplier at the canonical accumulation boundary.

## 181B.RAD3 No double application

Do not multiply:
- ambient rate;
- exposure result;
- health dose
all separately.

## 181B.RAD4 Shielding remains canonical

Difficulty changes environmental pressure, not player shielding stats.

## 181B.RAD5 Treatment unaffected unless a separate axis exists

## 181B.RAD6 Tests

- neutral parity;
- easy;
- hard;
- shielded;
- unshielded.

---

# 181B-NEEDS — Hunger / Thirst / Fatigue

## 181B.N1 Audit need decay authority

## 181B.N2 One input

If all needs share one difficulty axis:
- map once to a typed profile.

## 181B.N3 Preserve content ratios

Do not accidentally make thirst easier than hunger unless authored.

## 181B.N4 Recovery rate

Changing decay does not automatically multiply recovery.

Separate if needed.

## 181B.N5 Starvation/dehydration thresholds unchanged by default

Only pressure rate changes.

## 181B.N6 Tests

24h/72h trajectories across presets.

---

# 181B-RAID — Raid Frequency / Pressure

## 181B.R1 Identify canonical raid authority

Do not wire to nonexistent `RaidSpawner` by assumption.

## 181B.R2 Cadence semantics

Difficulty may modify:
- event hazard rate;
- cooldown;
- threat accumulation.

## 181B.R3 Minimum spacing

Maintain hard floor to prevent raid spam.

## 181B.R4 Story/scripted raids

Difficulty should not erase required authored raids.

Could modify:
- prep time;
- composition/severity
only if supported.

## 181B.R5 Random raid event

Seeded.

Difficulty modifies probability inputs, not RNG identity.

## 181B.R6 No double frequency+threat multiplication accidentally

## 181B.R7 Tests

Long-run raid counts and spacing.

---

# 181B-RES — Resource Scarcity

## 181B.S1 Split scarcity domains

Potential:
- starting supplies;
- scavenging yield;
- rare-loot availability.

## 181B.S2 Starting inventory

Apply exactly once at campaign bootstrap.

## 181B.S3 Loot generation

Apply when loot table resolves.

## 181B.S4 Existing placed loot

Do not retroactively delete when difficulty changes mid-campaign.

## 181B.S5 Crafted resources

Not affected by scarcity unless explicit axis.

## 181B.S6 Merchant stock

Market/economy axis owns if needed.

## 181B.S7 No “divide all resources”

Different item floors/minimums may be required.

## 181B.S8 Quest-critical items

Never zeroed by difficulty unless alternate route exists.

## 181B.S9 Tests

Content reachability at minimum resource profile.

---

# 181B-ECO — Economic Pressure

## 181B.E1 Market authority remains canonical

## 181B.E2 Difficulty may configure

- scarcity elasticity;
- volatility amplitude;
- merchant spread
if MarketSystem supports.

## 181B.E3 No final `price *= 2`

unless architecture explicitly defines it as canonical pressure input.

## 181B.E4 Bounded prices

Existing clamps remain.

## 181B.E5 Black market

Plan 155 reads same market state.

No duplicate difficulty modifier there.

## 181B.E6 Tests

Fixed-seed market path across presets.

---

# 181B-EXP — Expedition Danger

## 181B.X1 Audit hazard pipeline

## 181B.X2 Possible inputs

- encounter hazard weight;
- hazard severity;
- route exposure.

## 181B.X3 Travel time unchanged unless separate axis

## 181B.X4 Rewards not automatically scaled

Risk/reward tuning separate.

## 181B.X5 Dispatch preview parity

Preview uses same difficulty-adjusted model.

## 181B.X6 Save stability

Changing unlocked difficulty affects future unresolved checks only.

Do not reroll resolved expedition events.

## 181B.X7 Tests

Same route/team across profiles.

---

# 181B-WEA — Weather Severity

## 181B.W1 Audit weather authority

## 181B.W2 Event frequency vs severity

Separate where possible.

## 181B.W3 Avoid physics nonsense

Do not multiply:
- temperature;
- wind;
- precipitation
arbitrarily if systems are authored in bands.

## 181B.W4 Prefer event-weight/intensity profile

## 181B.W5 Hazard coupling

Radiation/weather interactions must not accidentally compound twice.

## 181B.W6 Tests

Seasonal event distribution and damage.

---

# 181B-CRAFT — Crafting Difficulty

## 181B.C1 Audit crafting authority

## 181B.C2 Time and material cost separate

Source says both multiply.

Treat as two axes internally.

## 181B.C3 Recipe integrity

Cost multiplier must preserve integer/material validity.

## 181B.C4 Minimum one

Nonzero requirements stay >=1 unless recipe allows zero.

## 181B.C5 Unique/quest recipe

Must remain craftable.

## 181B.C6 Queue state

Difficulty change does not rewrite already-started craft unless explicitly documented.

Preferred:
- snapshot cost/time at craft start.

## 181B.C7 Tests

Before/after setting change.

---

# 181B-DEATH — Survivor Death Consequence

## 181B.D1 Reframe source slider

“Survivor death penalty” is ambiguous.

Create ADR:

`ADR_DIFFICULTY_DEATH_CONSEQUENCES.md`

## 181B.D2 Death remains permanent

Unless a separate game mode changes permadeath.

Do not let scalar difficulty revive survivors.

## 181B.D3 Candidate tunable consequences

Only real:
- grief magnitude/duration;
- morale shock;
- material loss caused by death;
- recovery grace.

## 181B.D4 Palliative/death quality

Plan 60 remains authority.

## 181B.D5 Memorial

Unaffected except downstream morale if explicitly tuned.

## 181B.D6 No exploit

Lowering difficulty immediately after death cannot retroactively reduce already-applied consequence.

## 181B.D7 Rename UI axis

Prefer:
- `Loss Consequences`
or domain-specific explanation.

### 181B DoD

Every difficulty axis reaches one real domain seam exactly once, with domain-safe semantics instead of blind universal multiplication.

---

# TASK 181C — Campaign Lock, Mid-Campaign Changes & Temporal Semantics

# 181C.0 Goal

Make difficulty changes predictable, fair, and non-destructive.

## 181C.1 Campaign-start selection

Occurs before:
- starting resource bootstrap;
- initial world generation
for axes that affect those facts.

## 181C.2 Atomic campaign start

Finalize:
- profile;
- lock choice;
- NG+ challenge set;
then bootstrap world.

## 181C.3 Lock meaning

If locked:
- profile cannot change through normal campaign APIs.

## 181C.4 Lock immutability

Can lock:
- at campaign start.

Whether an unlocked campaign can later become locked:
- decide explicitly.

Recommended:
- yes, once;
- cannot unlock afterward
if integrity mode.

## 181C.5 No unlock button in locked campaign

## 181C.6 Core validation

`TryChangeProfile` returns:
- locked reason.

## 181C.7 Developer/debug override

Separate dev-only path.

Never player-facing.

## 181C.8 Mid-campaign change semantics

Apply to future evaluations.

Examples:

### Needs
New decay rate from change moment.

### Radiation
New exposure rate from change moment.

### Raid
Future scheduler decisions; do not cancel committed raid unless documented.

### Loot
Future generated loot; existing containers unchanged.

### Market
Next price evolution/quote according to market rules.

### Expedition
Future unresolved checks; accepted expedition snapshot policy documented.

### Crafting
New crafts; existing crafts snapshot old cost/time.

## 181C.9 Transaction boundary

Difficulty cannot change:
- mid-resolution;
- inside raid combat;
- while applying daily tick.

Queue change until safe boundary if UI permits.

## 181C.10 Change event

Record:
- day;
- previous fingerprint;
- new fingerprint;
- reason;
- locked state.

## 181C.11 No moral/narrative punishment for lowering difficulty

Source's “The Mercy” framing should be avoided.

Difficulty adjustment is a player configuration choice.

## 181C.12 Journal

Do not place ordinary settings changes in in-world survivor journal by default.

Use:
- campaign metadata/history.

## 181C.13 Challenge integrity

For achievements/challenges:
- track minimum/maximum/changes through metadata.

Do not shame player.

## 181C.14 Difficulty lowering

Allowed if unlocked.

## 181C.15 Difficulty raising

Allowed if unlocked.

## 181C.16 Profile history

Bounded:
- append changes;
- usually low volume.

## 181C.17 Configuration snapshot

Each critical generated object may snapshot relevant profile values if future changes should not alter it.

## 181C.18 Snapshot policy matrix

Create:

`DIFFICULTY_TEMPORAL_SEMANTICS_MATRIX.md`

Rows:
- raid;
- expedition;
- craft;
- loot container;
- market quote;
- weather event;
- quest;
- daily needs.

## 181C.19 No retroactive reroll

Hard invariant.

### 181C DoD

Difficulty can be locked or adjusted at explicit safe boundaries, with future-only effects and no retroactive rewriting/rerolling of committed world state.

---

# TASK 181D — Plan 175 New Game+ Composition

# 181D.0 Goal

Make base difficulty and NG+ challenge modifiers compose through one explicit pipeline.

## 181D.1 Audit Plan 175

Inventory modifiers:
- ironman;
- scarce;
- hostile;
- others.

## 181D.2 Conceptual separation

```text
Base difficulty
+ NG+ challenge modifiers
= EffectiveChallengeProfile
```

## 181D.3 Composition order

Define once.

Prefer:
1. baseline;
2. selected difficulty;
3. NG+ challenge delta;
4. domain clamp.

## 181D.4 Additive vs multiplicative

Axis-specific.

Do not multiply all profiles blindly.

## 181D.5 Example scarcity

Base Hard:
- scavenge yield 0.8.

NG+ Scarce:
- additional 0.8.

Effective may be:
- 0.64
if deliberately multiplicative;
or bounded curve.

Must be documented.

## 181D.6 Ironman

May not be a numeric difficulty axis at all.

It may modify:
- save/reload rules.

Keep in NG+ authority.

Difficulty UI can summarize it.

## 181D.7 Hostile

May modify:
- faction initial state;
- raid pressure.

Do not duplicate.

## 181D.8 One effective provider

Domain systems should receive one composed profile, not read:
- base difficulty;
- NG+ separately
and risk double apply.

## 181D.9 Composition diagnostics

Show:
- base;
- NG+ contribution;
- effective value.

## 181D.10 Fingerprint

Effective challenge fingerprint includes:
- difficulty preset/custom;
- NG+ challenge set;
- relevant schema versions.

## 181D.11 Achievement qualification

Use effective fingerprint/history.

## 181D.12 Tests

Preset × NG+ matrix.

### 181D DoD

Base difficulty and meta-progression challenge modifiers compose exactly once into a single effective profile consumed by domain systems.

---

# TASK 181E — Presets, Custom Mode, Recommendations & UI

# 181E.0 Goal

Make difficulty configuration understandable and accessible without forcing players to understand raw tuning math.

## 181E.1 Campaign start surface

Show presets first:
- Easy;
- Normal;
- Hard;
- Nightmare;
- Custom.

## 181E.2 Recommendation language

Use neutral descriptions.

Avoid:
- “for casuals”;
- “for real players”;
- “masochist.”

## 181E.3 Recommended default

Normal can be default if baseline testing supports it.

## 181E.4 Easy description

Explain:
- slower survival pressure;
- fewer/less frequent raids;
- more forgiving resource economy.

## 181E.5 Hard

Explain:
- faster pressure;
- harsher world;
- fewer resources.

## 181E.6 Nightmare

Explain:
- severe scarcity;
- frequent/high-pressure threats;
- tight survival margins.

## 181E.7 Custom

Advanced tuning.

## 181E.8 Preset comparison

Side-by-side categories, not raw 9-column tiny spreadsheet if accessibility suffers.

## 181E.9 Axis grouping

Suggested:

```text
Survival
Threats
Resources & Economy
Expeditions
Environment
Production
Loss
```

## 181E.10 Slider labels

Use player-facing semantics.

Avoid:
- “1.37× volatility.”

Offer exact values in advanced tooltip.

## 181E.11 Direction labels

Examples:

```text
Radiation pressure:
Lower ← Normal → Higher
```

## 181E.12 Resource scarcity direction

Avoid confusing multiplier inversion.

UI should say:
- More abundant;
- Normal;
- Scarcer.

Internal numeric can be yield multiplier.

## 181E.13 Death consequence label

Use precise downstream meaning.

## 181E.14 Crafting

If time and cost split internally, decide UI:
- one combined “Crafting Pressure” curve;
or
- advanced separate sliders.

## 181E.15 Preset → custom transition

Editing any preset value:
- becomes Custom;
- remembers base preset for comparison.

## 181E.16 Reset

Reset custom to:
- current base;
- Normal.

## 181E.17 Lock control

At campaign start:
- “Lock challenge settings for this campaign.”

Explain irreversibility.

## 181E.18 Mid-campaign UI

If unlocked:
- edit settings;
- clear “affects future outcomes” note.

If locked:
- read-only.

## 181E.19 HUD indicator

Source proposes persistent HUD name.

Audit value.

Preferred:
- settings/pause/campaign summary;
- optional subtle indicator.

Do not burn HUD space by default.

## 181E.20 Campaign metadata

Show:
- starting preset;
- current profile;
- locked/unlocked;
- changes;
- NG+ challenges.

## 181E.21 Tooltips

Every axis explains:
- exact affected systems;
- what is **not** affected.

## 181E.22 Accessibility independence

Difficulty screen may link to accessibility settings but never auto-toggle them.

## 181E.23 Difficulty assist suggestions

Optional.

If game detects repeated failure:
- may suggest settings review.

Do not auto-lower difficulty without consent.

## 181E.24 No “Mercy” event

Settings change is not an in-world judgement.

## 181E.25 Achievement/challenge wording

Use neutral:
- “Complete a campaign on Hard.”
- “Complete a locked Nightmare campaign.”

Do not use derogatory achievement names.

## 181E.26 Custom profile naming

Local settings feature if easy.

## 181E.27 Profile name

Presentation only.

Not ID/fingerprint.

## 181E.28 Custom profile save slots

If settings manager supports:
- local reusable presets.

## 181E.29 Sharing

Defer to 181G unless safe import/export exists.

## 181E.30 Keyboard/controller

All sliders/settings navigable.

## 181E.31 Slider precision

Coarse user steps.

Avoid accidental 0.01 micro-tuning unless advanced.

## 181E.32 Screen reader

Value + semantic direction.

## 181E.33 Color

No red/green-only “easy/hard.”

## 181E.34 Text scaling

Preset comparison remains readable.

### 181E DoD

Players can select or customize challenge confidently, understand every effect, lock settings if desired, and access all accessibility features independently.

---

# TASK 181F — Persistence, Migration, Determinism, Balance & CI

# 181F.0 Goal

Make difficulty configuration save-safe, deterministic, versionable, and proven across long simulations.

## 181F.1 Persist selected configuration

Suggested:

```text
schema_version
preset_id
resolved_axis_values
custom_mode
locked
profile_fingerprint
change_history[]
preset_catalog_version
```

## 181F.2 Persist resolved values

Important:
- if future preset tuning changes, existing campaign should not silently become harder/easier on load.

Store resolved campaign profile.

## 181F.3 Preset ID remains provenance

## 181F.4 New game uses latest preset values

## 181F.5 Old save

No difficulty section:
- resolved Normal baseline.

## 181F.6 Old save Normal parity

Must reproduce historical behavior closely.

## 181F.7 Unknown axis migration

New axis:
- neutral default unless migration specifies.

## 181F.8 Removed axis

Preserve old snapshot if required by active object semantics;
otherwise ignore with migration log.

## 181F.9 Invalid custom value

Fail safe:
- explicit error/recovery;
- do not silently load extreme value.

## 181F.10 Lock restore

Exact.

## 181F.11 Change history

No duplicate entry on restore.

## 181F.12 Determinism

Difficulty profile itself contains no RNG.

## 181F.13 RNG interaction

Domain system uses its normal deterministic seed with difficulty-adjusted probability/weights.

## 181F.14 No seed change merely from difficulty display name

## 181F.15 Headless

All profiles selectable via test/config API.

## 181F.16 Data integrity

Validate:
- preset IDs;
- axis keys;
- axis bounds;
- neutral profile;
- localization;
- duplicate presets;
- composition version.

## 181F.17 Selftest

Create:

```text
--difficulty-selftest
```

## 181F.18 Selftest cases

At least:
1. Normal baseline;
2. Easy;
3. Hard;
4. Nightmare;
5. Custom;
6. lock;
7. unlocked change;
8. locked change rejection;
9. old save;
10. unknown axis migration;
11. NG+ composition;
12. future-only semantics;
13. save/load;
14. headless.

---

# 181F-B — Preset Balance Simulation

## 181F.B1 Fixed-seed scenario suite

Run identical strategic inputs under:
- Easy;
- Normal;
- Hard;
- Nightmare.

## 181F.B2 30-day metrics

Track:
- average hunger/thirst/fatigue;
- cumulative radiation;
- raid count;
- loot yield;
- market pressure;
- expedition incidents;
- weather hazards;
- craft throughput;
- survivor deaths.

## 181F.B3 120-day metrics

Track:
- survival rate;
- resource reserves;
- faction/trade viability;
- expedition completion;
- shelter progression;
- casualty/grief load.

## 181F.B4 180-day metrics

Ensure:
- Nightmare remains hard but playable;
- Easy does not trivialize all systems;
- Normal stays baseline;
- Hard materially differs.

## 181F.B5 Custom min/max tests

Per axis, not all 0.5/2.0 simultaneously unless domain bounds allow.

## 181F.B6 All-min profile

Only as stress test.

## 181F.B7 All-max profile

Only as stress test.

## 181F.B8 Content reachability

Every preset must still allow:
- campaign-critical quests;
- essential crafting;
- medical recovery paths;
- expedition access.

## 181F.B9 Zero-resource trap

No preset can generate unwinnable start unless explicitly challenge-tagged.

## 181F.B10 Raid flood

No preset violates raid spacing/event budget.

## 181F.B11 Radiation runaway

No unintended multiplicative stacking with weather/zone modifiers.

## 181F.B12 Needs runaway

No duplicate multiplier with difficulty-like survivor traits/NG+.

## 181F.B13 Economy runaway

No price spiral beyond canonical clamps.

## 181F.B14 Crafting impossibility

Material cost rounding cannot make recipes unreachable.

## 181F.B15 Death consequence

No extreme grief loop makes campaign unrecoverable from one death unless intended.

---

# 181F-T — Telemetry / Diagnostics

## 181F.T1 Effective profile dump

Headless command prints:
- base preset;
- custom values;
- NG+;
- effective axis values.

## 181F.T2 Modifier provenance

Each axis can report:

```text
Normal 1.0
Hard preset 1.3
NG+ Scarce ×0.8
effective 1.04
```

or domain-specific equivalent.

## 181F.T3 No player surveillance requirement

Telemetry can be test/debug-only.

## 181F.T4 Balance report

Generate:

`DIFFICULTY_BALANCE_REPORT.md`

## 181F.T5 Preset diff

Generate machine-readable matrix.

---

# 181F-C — CI

## 181F.C1 Normal parity gate

Normal must match pre-difficulty baseline.

## 181F.C2 Axis-consumer gate

Every shipped axis has exactly one registered consumer.

## 181F.C3 Orphan-axis gate

No slider without consumer.

## 181F.C4 Duplicate-application gate

No axis read twice in one canonical formula.

## 181F.C5 Bounds gate

## 181F.C6 Locked-change gate

## 181F.C7 Future-only semantics gate

## 181F.C8 NG+ composition gate

## 181F.C9 Old-save gate

## 181F.C10 Preset golden snapshots

## 181F.C11 Content reachability by preset

## 181F.C12 Accessibility independence gate

No accessibility flag set by difficulty preset.

## 181F.C13 Source-scan gate

Detect:
- direct subsystem fields mutated from UI;
- difficulty-owned prices;
- difficulty-owned raid state;
- difficulty-owned loot state;
- duplicate NG+ reads.

## 181F.C14 Failure fixtures

Deliberately invalid:
- axis;
- preset;
- out-of-bounds value;
- duplicate consumer;
- unknown NG+ composition.

## 181F.C15 Generated docs

Create:
- `DIFFICULTY_ARCHITECTURE.md`;
- `DIFFICULTY_AUTHORITY_MATRIX.md`;
- `DIFFICULTY_AXIS_MATRIX.md`;
- `DIFFICULTY_PRESET_MATRIX.md`;
- `DIFFICULTY_TEMPORAL_SEMANTICS_MATRIX.md`;
- `DIFFICULTY_NGPLUS_COMPOSITION_MATRIX.md`;
- `DIFFICULTY_MIGRATION_MATRIX.md`;
- `DIFFICULTY_BALANCE_REPORT.md`;
- `ADR_DIFFICULTY_DEATH_CONSEQUENCES.md`.

### 181F DoD

Difficulty profiles remain stable across save/load and versions, preserve Normal parity, compose correctly with NG+, and produce measured rather than guessed challenge differences.

---

# TASK 181G — Custom Profile Export / Sharing: Explicit Follow-On

# 181G.0 Goal

Keep file-sharing concerns outside the core difficulty authority until a safe import/export contract exists.

## 181G.1 Local named profiles

Can ship if game-settings storage already supports user presets.

## 181G.2 Export

Only if:
- user data/export framework exists.

## 181G.3 Import

Must validate:
- schema;
- axis IDs;
- ranges;
- version;
- unknown fields.

## 181G.4 No executable content

Difficulty profiles are data only.

## 181G.5 Plan 47

If treated as shareable content pack:
- follow mod/content-pack security/version contract.

## 181G.6 Fingerprint

Imported profile gets stable fingerprint.

## 181G.7 Human-readable summary

Show before apply.

## 181G.8 No trust based on profile name

## 181G.9 Online sharing/leaderboards

Separate service feature.

## 181G.10 Weekly challenge rotation

Separate live-content/service feature.

### 181G DoD

Custom profile sharing remains a validated data-import/export extension rather than a requirement for the base difficulty system.

---

# 5. Difficulty Architecture

```text
PRESET / CUSTOM INPUT
        │
        ▼
Validated DifficultyProfile
        │
        ├── base values
        ├── lock policy
        └── fingerprint
        │
        ▼
NG+ Composition
        │
        ▼
EffectiveChallengeProfile
        │
        ├────────► radiation adapter
        ├────────► needs adapter
        ├────────► raid adapter
        ├────────► resources adapter
        ├────────► economy adapter
        ├────────► expedition adapter
        ├────────► weather adapter
        ├────────► crafting adapter
        └────────► loss-consequence adapter
```

One profile. Many typed consumers.

---

# 6. Preset Contract

Preset is:

```text
named curated axis values
```

It is not:
- a branch of simulation logic.

---

# 7. Custom Contract

Custom is:

```text
the same axes
+ player-selected values
```

No hidden custom-only mechanics.

---

# 8. Neutral Baseline Contract

For every axis:

```text
neutral value
```

must reproduce current baseline semantics.

Often:
- `1.0`.

But a typed enum/band may use:
- `normal`.

---

# 9. Axis Contract

Every axis answers:

```text
What exact gameplay fact does it tune?
Which system owns that fact?
What is neutral?
What is safe min/max?
When does a changed value take effect?
Does NG+ also modify it?
```

---

# 10. Radiation Contract

Difficulty influences:
- environmental exposure pressure.

RadiationSystem owns:
- dose accumulation;
- shielding;
- sickness.

---

# 11. Needs Contract

Difficulty influences:
- decay pressure.

NeedsSystem owns:
- hunger/thirst/fatigue state;
- thresholds;
- recovery.

---

# 12. Raid Contract

Difficulty influences:
- scheduler pressure/cadence/severity.

Raid authority owns:
- spawn;
- event;
- attackers;
- resolution.

---

# 13. Resource Contract

Difficulty influences:
- future resource generation/bootstrap.

Inventory owns:
- actual items.

Loot authority owns:
- generated drops.

---

# 14. Economy Contract

Difficulty influences:
- market pressure parameters.

MarketSystem owns:
- prices;
- stock;
- volatility.

---

# 15. Expedition Contract

Difficulty influences:
- future hazard/risk weights.

ExpeditionSystem owns:
- route;
- incident;
- outcome.

---

# 16. Weather Contract

Difficulty influences:
- severe-event weighting/intensity.

Weather authority owns:
- actual weather state and effects.

---

# 17. Crafting Contract

Difficulty influences:
- cost/time profiles.

CraftingSystem owns:
- recipe;
- queue;
- completion.

---

# 18. Death Consequence Contract

Difficulty may influence:
- aftermath intensity.

Lifecycle owns:
- death.

Relations/morale own:
- grief.

Memorial owns:
- remembrance.

---

# 19. Accessibility Contract

Accessibility options:
- never disabled by Hard/Nightmare;
- never enabled only by Easy.

Difficulty changes challenge, not interface rights.

---

# 20. Campaign Lock Contract

Lock state belongs to campaign difficulty config.

If locked:
- Core rejects mutation.

---

# 21. Mid-Campaign Change Contract

Changes apply to:

```text
future uncommitted calculations
```

not:
- past;
- already resolved;
- already generated snapshots where frozen.

---

# 22. Temporal Semantics Matrix

Each consumer classifies:

```text
LIVE
NEXT_TICK
NEXT_EVENT
SNAPSHOT_AT_CREATION
CAMPAIGN_START_ONLY
```

Example:
- starting resources = campaign-start-only;
- active craft = snapshot-at-creation;
- needs = next tick;
- raid scheduler = next event;
- market quote = next quote/evolution.

---

# 23. NG+ Composition Contract

Base difficulty and NG+ must not be independently read by domain systems.

Only:

```text
EffectiveChallengeProfile
```

is consumed.

---

# 24. Persistence Matrix

| Fact | Owner |
|---|---|
| preset ID | difficulty |
| custom values | difficulty |
| lock | difficulty |
| profile fingerprint | difficulty |
| change history | difficulty |
| effective composed profile | derived |
| NG+ challenges | Plan 175 |
| radiation state | RadiationSystem |
| needs state | NeedsSystem |
| raid state | raid authority |
| inventory | inventory |
| market prices | MarketSystem |
| expedition state | ExpeditionSystem |
| weather state | WeatherSystem |
| crafting queue | CraftingSystem |
| grief/morale | social/morale |

---

# 25. Old-Save Migration

Default:

```text
preset_id = normal
resolved_profile = neutral baseline
custom_mode = false
locked = false
change_history = []
```

This must preserve pre-feature gameplay.

---

# 26. Preset Versioning Contract

New preset tuning should affect:
- new campaigns.

Existing campaign:
- retains resolved values.

This avoids stealth balance changes on load.

---

# 27. Custom Profile Versioning

Persist:
- values by stable axis ID.

New axis:
- neutral.

Removed axis:
- ignored/migrated.

---

# 28. Profile Fingerprint Contract

Fingerprint uses:
- canonical sorted axis serialization;
- schema version;
- NG+ challenge configuration if computing effective challenge ID.

Not:
- user-facing profile name.

---

# 29. Achievement / Challenge Integrity

Track:
- starting profile;
- locked;
- whether settings changed;
- effective difficulty fingerprint.

Do not require insulting labels.

---

# 30. Failure Injection Matrix

## N181.1 Normal differs from pre-feature baseline
Expected: parity gate fails.

## N181.2 UI writes `RadiationSystem` internal multiplier directly
Expected: authority gate fails.

## N181.3 Resource scarcity deletes existing container loot after slider change
Expected: temporal semantics fail.

## N181.4 Active craft cost doubles after difficulty change
Expected: snapshot semantics fail unless explicitly intended.

## N181.5 Locked profile changes through runtime API
Expected: lock gate fails.

## N181.6 NG+ Scarce is read in loot system separately from composed difficulty
Expected: double-composition gate fails.

## N181.7 Weather severity blindly multiplies temperature by 2
Expected: domain-semantics gate fails.

## N181.8 Death penalty changes whether survivor death is permanent
Expected: lifecycle-authority gate fails.

## N181.9 Old save loads Hard because preset catalog changed default
Expected: migration gate fails.

## N181.10 Accessibility feature becomes disabled on Nightmare
Expected: accessibility-independence gate fails.

## N181.11 Custom profile contains unknown axis and silently ignores it
Expected: import/integrity fail.

## N181.12 Changing difficulty rerolls already committed expedition incident
Expected: future-only gate fails.

---

# 31. Determinism Contract

Difficulty configuration itself is deterministic.

Same:

```text
resolved profile
+ NG+ challenges
+ domain state
+ domain RNG seed
```

must yield same domain outcomes.

Changing difficulty changes inputs, not RNG infrastructure.

---

# 32. Long-Horizon Metrics

Track per profile:

```text
survivor-days
deaths
needs deficit
radiation dose
raid count
raid spacing
resource intake
resource reserve
market price index
expedition incident count
weather hazard count
craft throughput
quest completion
campaign completion
```

---

# 33. Preset Design Principles

## Easy

Should reduce pressure while preserving:
- systems;
- consequences;
- learning.

Do not remove entire mechanics by default.

## Normal

Reference baseline.

## Hard

Higher pressure across several domains without pure health sponge behavior.

## Nightmare

Extremely constrained but still structurally playable.

---

# 34. Custom Axis Design Principles

Avoid redundant sliders.

Example:

```text
resource scarcity
economic pressure
crafting cost
```

can interact heavily.

Document interactions so custom players can predict them.

---

# 35. Compound Difficulty Guardrails

All-max custom can create multiplicative collapse.

Use:
- domain clamps;
- warnings;
- optional “extreme combination” badge.

Do not silently normalize away player choice.

---

# 36. Accessibility Separation Checklist

Difficulty must never be the sole control for:
- pause behavior if accessible pause exists;
- UI font size;
- colorblind settings;
- reduced motion;
- input hold/toggle;
- audio captions;
- warning verbosity;
- tutorial hints.

---

# 37. UI Acceptance

## Campaign start
- presets;
- descriptions;
- comparison;
- custom;
- optional lock.

## Settings
- current profile;
- edit if unlocked;
- read-only if locked.

## Campaign summary
- profile;
- history;
- NG+.

## Custom
- clear axis groups;
- semantic labels;
- reset.

---

# 38. Recommendation Copy

Avoid categorical skill judgements.

Use:
- “Recommended starting point.”
- “More forgiving survival pressure.”
- “Higher scarcity and threat pressure.”
- “Extreme challenge with narrow margins.”

---

# 39. Custom Config Naming

If supported:
- local label;
- max length;
- escaped;
- not a gameplay identifier.

---

# 40. Content Acceptance Across Difficulty

Difficulty cannot make authored content silently unreachable.

Run content acceptance under:
- Easy;
- Normal;
- Hard;
- Nightmare.

Especially:
- quest-critical items;
- expedition destinations;
- medicine;
- crafting gates.

---

# 41. Reachability Matrix

For each preset:

```text
campaign start viable
food obtainable
water obtainable
medical path viable
crafting path viable
expedition path viable
quest-critical items reachable
raid recovery possible
```

---

# 42. Performance Guardrails

- profile cached;
- O(1) axis read;
- no per-frame config parsing;
- preset catalog loaded once;
- no reflection lookup in hot path.

---

# 43. CI / Gate Set

Recommended:

```text
difficulty_authority_single
difficulty_normal_parity
difficulty_axis_integrity
difficulty_axis_consumer_unique
difficulty_orphan_axis
difficulty_bounds
difficulty_lock
difficulty_temporal_semantics
difficulty_ngplus_composition
difficulty_old_save
difficulty_preset_goldens
difficulty_content_reachability
difficulty_accessibility_independence
difficulty_no_direct_domain_state
difficulty_ui_access
```

---

# 44. Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --difficulty-selftest
godot --headless --path . -- --real-campaign-journey-selftest
bash scripts/ci/content-acceptance-gate.sh
bash scripts/ci/verify-fast.sh
```

Use exact current command names if renamed.

---

# 45. Recommended Commit Breakdown

```text
181A-1 difficulty surface/authority audit
181A-2 Normal baseline golden
181A-3 typed axis registry
181A-4 difficulty profile DTO
181A-5 preset catalog/loader
181A-6 profile fingerprint/versioning
181A-7 custom profile state
181A-8 docs/integrity tests

181B-1 radiation adapter
181B-2 needs adapter
181B-3 raid adapter
181B-4 resource/bootstrap/loot adapters
181B-5 economy adapter
181B-6 expedition adapter
181B-7 weather/crafting adapters
181B-8 death-consequence ADR/adapter

181C-1 campaign-start selection
181C-2 lock enforcement
181C-3 safe profile-change transaction
181C-4 temporal semantics matrix
181C-5 committed-object snapshot policies
181C-6 metadata/change history
181C-7 anti-reroll tests
181C-8 docs

181D-1 Plan-175 modifier audit
181D-2 composition pipeline
181D-3 domain clamps/order
181D-4 NG+ profile diagnostics
181D-5 effective fingerprint
181D-6 preset×NG+ tests
181D-7 duplicate-read source scan
181D-8 docs

181E-1 campaign-start UI
181E-2 preset comparison
181E-3 custom grouped sliders
181E-4 mid-campaign settings UI
181E-5 lock UX
181E-6 accessibility independence
181E-7 custom naming/local profiles
181E-8 UI snapshots/tooltips

181F-1 save schema/resolved-profile persistence
181F-2 old-save migration
181F-3 preset-version migration
181F-4 difficulty selftest/goldens
181F-5 30-day simulation
181F-6 120/180-day preset simulations
181F-7 CI/failure fixtures/reachability
181F-8 final ship/no-ship report

181G-1 custom export/import disposition
181G-2 Plan-47 sharing integration if approved
```

---

# 46. Risk Register

## R181.1 Blind multipliers break domain semantics

Mitigation:
- typed sub-profiles;
- domain adapters.

## R181.2 Normal subtly changes baseline

Mitigation:
- pre-feature golden parity.

## R181.3 Custom combinations create impossible campaign

Mitigation:
- bounds;
- warnings;
- content reachability;
- stress tests.

## R181.4 Mid-campaign changes reroll world

Mitigation:
- temporal semantics matrix;
- snapshot committed objects.

## R181.5 NG+ modifiers double apply

Mitigation:
- one effective profile provider.

## R181.6 Difficulty replaces accessibility

Mitigation:
- explicit independence gate.

## R181.7 Preset updates alter old campaigns

Mitigation:
- persist resolved profile.

## R181.8 Death penalty undermines canonical lifecycle

Mitigation:
- narrow aftermath-only ADR.

---

# 47. Acceptance Checklist

## P0

- [ ] CampaignCalendar audited
- [ ] NeedsSystem audited
- [ ] RadiationSystem audited
- [ ] raid authority audited
- [ ] loot/scavenge authority audited
- [ ] starting inventory bootstrap audited
- [ ] MarketSystem audited
- [ ] ExpeditionSystem audited
- [ ] WeatherSystem audited
- [ ] CraftingSystem audited
- [ ] death/grief/memorial audited
- [ ] Plan 175 NG+ modifiers audited
- [ ] Plan 34 accessibility requirements audited
- [ ] settings persistence audited
- [ ] save settings audited
- [ ] composition root audited
- [ ] debug/tuning knobs inventoried
- [ ] Normal baseline golden captured
- [ ] difficulty authority matrix published
- [ ] accessibility separation inventory published

## 181A

- [ ] DifficultySettingsSystem has narrow ownership
- [ ] typed DifficultyProfile
- [ ] no raw runtime modifier dictionary
- [ ] radiation sub-profile
- [ ] needs sub-profile
- [ ] raid sub-profile
- [ ] resource sub-profile
- [ ] economy sub-profile
- [ ] expedition sub-profile
- [ ] weather sub-profile
- [ ] crafting time/cost semantics
- [ ] death consequence ADR
- [ ] typed axis registry
- [ ] stable axis IDs
- [ ] versioned preset catalog
- [ ] validated catalog dictionary boundary
- [ ] Easy preset
- [ ] Normal neutral preset
- [ ] Hard preset
- [ ] Nightmare preset
- [ ] source numeric values treated as tuning candidates
- [ ] Custom uses same axes
- [ ] stable fingerprint
- [ ] preset version
- [ ] unknown future axis migration
- [ ] unknown preset recovery
- [ ] immutable read API
- [ ] controlled write API
- [ ] no subsystem setter spree
- [ ] generated axis/preset matrices

## 181B — Radiation / Needs

- [ ] canonical radiation accumulation seam
- [ ] one radiation difficulty input
- [ ] no double radiation application
- [ ] shielding canonical
- [ ] treatment separate
- [ ] radiation tests
- [ ] canonical needs decay seam
- [ ] needs mapping once
- [ ] need ratios preserved
- [ ] recovery separate
- [ ] thresholds unchanged unless explicit
- [ ] trajectory tests

## 181B — Raid / Resources

- [ ] real raid authority identified
- [ ] cadence semantics chosen
- [ ] minimum spacing retained
- [ ] scripted raids preserved
- [ ] seeded RNG preserved
- [ ] no frequency/severity double application
- [ ] long-run raid tests
- [ ] resource domains split
- [ ] starting resources exactly once
- [ ] future loot generation only
- [ ] existing containers unchanged on setting change
- [ ] crafted resources separate
- [ ] merchant stock separate
- [ ] no blind divide-all
- [ ] quest-critical items protected
- [ ] minimum-resource reachability tests

## 181B — Economy / Expedition / Weather / Crafting

- [ ] MarketSystem remains price authority
- [ ] pressure input defined
- [ ] no raw final price multiplication
- [ ] price clamps preserved
- [ ] black-market path reads same market
- [ ] expedition hazard pipeline audited
- [ ] real danger inputs
- [ ] travel time separate
- [ ] rewards separate
- [ ] dispatch preview parity
- [ ] committed expedition semantics documented
- [ ] weather event/severity split
- [ ] no raw physical-value multiplication
- [ ] weather/radiation double-stack prevented
- [ ] crafting authority audited
- [ ] time/cost split
- [ ] recipe integer integrity
- [ ] quest recipes protected
- [ ] active craft snapshot policy
- [ ] adapter tests

## 181B — Death

- [ ] death-consequence ADR
- [ ] death remains canonical
- [ ] consequence axes have real consumers
- [ ] palliative remains Plan 60
- [ ] memorial remains canonical
- [ ] no retroactive consequence change
- [ ] precise UI wording

## 181C

- [ ] campaign-start profile before bootstrap
- [ ] atomic profile+lock+NG+ finalization
- [ ] Core lock enforcement
- [ ] lock persistence
- [ ] locked campaign cannot unlock via UI
- [ ] dev override isolated
- [ ] future-only needs semantics
- [ ] future-only radiation semantics
- [ ] raid committed-event semantics
- [ ] existing loot unchanged
- [ ] market next-evaluation semantics
- [ ] expedition unresolved-check semantics
- [ ] active craft snapshot
- [ ] safe transaction boundary
- [ ] source-attributed profile change
- [ ] no shame/moral framing
- [ ] campaign metadata instead of in-world journal by default
- [ ] challenge integrity tracked neutrally
- [ ] difficulty lower/raise supported when unlocked
- [ ] history bounded
- [ ] snapshot policy matrix
- [ ] no retroactive reroll

## 181D

- [ ] Plan 175 modifier list inventoried
- [ ] base vs NG+ conceptual separation
- [ ] one composition order
- [ ] per-axis additive/multiplicative semantics
- [ ] scarcity composition documented
- [ ] ironman remains NG+ save-rule authority
- [ ] hostile modifier not duplicated
- [ ] one EffectiveChallengeProfile consumer surface
- [ ] provenance diagnostics
- [ ] effective fingerprint
- [ ] achievement qualification uses stable metadata
- [ ] preset×NG+ matrix tests

## 181E — UI

- [ ] presets first
- [ ] neutral recommendation copy
- [ ] Normal default only if validated
- [ ] Easy description
- [ ] Hard description
- [ ] Nightmare description
- [ ] Custom
- [ ] accessible comparison
- [ ] grouped axes
- [ ] semantic slider labels
- [ ] resource direction not inverted/confusing
- [ ] death consequence precise
- [ ] crafting combined/split UI decision
- [ ] editing preset becomes Custom
- [ ] reset behavior
- [ ] lock explanation
- [ ] mid-campaign unlocked edit
- [ ] locked read-only
- [ ] HUD indicator audited rather than forced
- [ ] campaign metadata
- [ ] tooltips say affected/not affected
- [ ] accessibility never auto-toggled
- [ ] optional assist suggestions require consent
- [ ] no “Mercy” event
- [ ] neutral achievements
- [ ] custom profile names presentation-only
- [ ] local custom profile persistence if supported
- [ ] sharing deferred unless safe
- [ ] keyboard/controller
- [ ] coarse slider precision
- [ ] screen-reader semantic value
- [ ] no color-only difficulty
- [ ] text scale

## 181F — Persistence

- [ ] schema version
- [ ] preset ID
- [ ] resolved axis values
- [ ] custom mode
- [ ] lock
- [ ] fingerprint
- [ ] change history
- [ ] preset catalog version
- [ ] resolved values persisted for old campaign stability
- [ ] new games use latest preset
- [ ] old save → Normal baseline
- [ ] old-save parity
- [ ] new axis neutral migration
- [ ] removed axis migration
- [ ] invalid custom recovery
- [ ] exact lock restore
- [ ] no duplicate history
- [ ] no RNG in difficulty authority
- [ ] domain RNG preserved
- [ ] display name does not alter seed
- [ ] headless profile selection
- [ ] data integrity
- [ ] difficulty selftest
- [ ] all core selftest cases

## 181F — Balance

- [ ] fixed-seed preset suite
- [ ] 30-day metrics
- [ ] 120-day metrics
- [ ] 180-day metrics
- [ ] Easy remains mechanically complete
- [ ] Normal parity
- [ ] Hard materially different
- [ ] Nightmare severe but playable
- [ ] per-axis min/max tests
- [ ] all-min stress
- [ ] all-max stress
- [ ] content reachability
- [ ] no unwinnable start
- [ ] raid spacing preserved
- [ ] radiation runaway prevented
- [ ] needs double multiplier prevented
- [ ] economy runaway prevented
- [ ] crafting impossibility prevented
- [ ] death/grief runaway prevented

## 181F — CI / Diagnostics

- [ ] effective profile dump
- [ ] modifier provenance
- [ ] no player telemetry dependency
- [ ] balance report
- [ ] preset diff
- [ ] Normal parity gate
- [ ] axis-consumer unique gate
- [ ] orphan-axis gate
- [ ] duplicate-application gate
- [ ] bounds gate
- [ ] lock gate
- [ ] future-only gate
- [ ] NG+ composition gate
- [ ] old-save gate
- [ ] preset golden snapshots
- [ ] content reachability per preset
- [ ] accessibility independence gate
- [ ] source-scan gate
- [ ] failure fixtures
- [ ] generated docs
- [ ] verify-fast

## 181G

- [ ] local named profiles only if settings store supports
- [ ] export gated on file framework
- [ ] import validates schema/axes/ranges/version
- [ ] data-only format
- [ ] Plan 47 contract used if shared
- [ ] stable fingerprint
- [ ] preview before apply
- [ ] no trust in display name
- [ ] leaderboard/service features deferred
- [ ] weekly rotations deferred

---

# 48. Ship / No-Ship Gate

**SHIP** only if:

```text
difficulty_authorities == 1
AND normal_profile_baseline_parity == pass
AND orphan_difficulty_axes == 0
AND duplicate_axis_consumers == 0
AND direct_difficulty_owned_simulation_state == 0
AND direct_difficulty_owned_market_prices == false
AND direct_difficulty_owned_raid_state == false
AND direct_difficulty_owned_loot_state == false
AND direct_difficulty_owned_survivor_death_state == false
AND unsupported_universal_0_5_to_2_axes == 0
AND locked_profile_mutation_paths == 0
AND retroactive_difficulty_rerolls == 0
AND active_object_semantics_without_snapshot_policy == 0
AND duplicate_ngplus_modifier_application == 0
AND difficulty_changes_accessibility_flags == false
AND old_save_non_normal_migration == false
AND preset_update_changes_existing_campaign_profile == false
AND invalid_custom_values_silently_accepted == false
AND difficulty_old_save == pass
AND difficulty_save_roundtrip == pass
AND difficulty_lock == pass
AND difficulty_temporal_semantics == pass
AND difficulty_ngplus_composition == pass
AND difficulty_preset_goldens == pass
AND difficulty_content_reachability_easy == pass
AND difficulty_content_reachability_normal == pass
AND difficulty_content_reachability_hard == pass
AND difficulty_content_reachability_nightmare == pass
AND difficulty_30_day_balance == pass
AND difficulty_120_day_balance == pass
AND difficulty_180_day_balance == pass
AND difficulty_selftest == pass
AND data_integrity_selftest == pass
AND ui_access == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 49. Implementer Handoff

1. Audit every proposed slider against the real system that owns that pressure.
2. Capture a fixed-seed pre-feature baseline before writing difficulty code.
3. Make Normal reproduce that baseline.
4. Build one typed difficulty profile and one axis registry.
5. Keep presets in data, but validate every catalog key into typed fields.
6. Do not assume all axes safely use 0.5–2.0.
7. Treat source preset numbers as tuning seeds, not immutable design truth.
8. Feed radiation into one canonical exposure seam.
9. Feed needs into one canonical decay seam.
10. Feed raid difficulty into the actual scheduler/threat model, preserving event budgets and minimum spacing.
11. Split resource scarcity into the real generation/bootstrap seams rather than dividing all inventory.
12. Let MarketSystem interpret economic pressure; difficulty must not own final prices.
13. Let ExpeditionSystem interpret hazard pressure and preserve preview/runtime parity.
14. Let WeatherSystem interpret event severity/weights; do not multiply physical weather values blindly.
15. Split crafting time and material cost internally if the crafting authority requires distinct semantics.
16. Reframe “death penalty” as specific aftermath consequences; never scale whether death is real.
17. Finalize difficulty before campaign bootstrap when starting resources/world generation depend on it.
18. Enforce lock in Core, not only UI.
19. Apply unlocked changes only to future uncommitted calculations.
20. Produce and test a temporal semantics matrix for every domain.
21. Compose Plan-175 NG+ challenge modifiers exactly once into an `EffectiveChallengeProfile`.
22. Persist resolved campaign values so future preset balance patches do not rewrite existing saves.
23. Keep accessibility completely independent from difficulty.
24. Use neutral, non-judgmental preset and achievement language.
25. Prefer campaign/settings display over a permanent HUD difficulty badge unless playtests prove the badge useful.
26. Add deterministic profile goldens and per-preset content reachability.
27. Run 30/120/180-day fixed-seed comparisons before finalizing preset numbers.
28. Close only when Easy, Normal, Hard, Nightmare, and Custom are different configurations of the **same game**, not separate branches or scattered multipliers.

---

# 50. Final Outcome

When this plan is complete, ASHFALL gains a real challenge configuration layer without fracturing its simulation.

The player can choose a curated preset or build a custom profile. The choice is stored once as campaign configuration and exposed to the rest of the game as a validated, typed read model.

Normal reproduces the existing baseline.

Easy eases pressure while preserving the game's systems and consequences.

Hard raises scarcity and threat without simply multiplying every number.

Nightmare narrows survival margins while remaining structurally playable.

Custom lets experienced players tune specific domains without requiring them to understand the implementation details behind those domains.

Most importantly, each setting reaches the correct authority.

Radiation pressure changes the radiation system's exposure input.
Need pressure changes need decay.
Raid pressure changes the canonical raid scheduler.
Resource scarcity changes future starting/loot generation through the systems that actually create resources.
Economic pressure is interpreted by the market.
Expedition danger is interpreted by expedition risk.
Weather severity is interpreted by weather.
Crafting pressure is interpreted by crafting.
Loss consequences are interpreted by the systems that already own grief and aftermath.

Nothing is duplicated.

Mid-campaign changes are equally disciplined. A setting change does not erase loot already generated, rewrite a craft already started, reroll an expedition incident that already resolved, or retroactively soften a death. It changes future calculations at documented transaction boundaries.

New Game+ modifiers also stop being a separate modifier path. Base difficulty and Plan-175 challenges compose once into a single effective challenge profile. Domain systems read only that final profile.

The result is also more accessible without confusing difficulty with accessibility.

Players can choose less punishing survival pressure and still use every accessibility setting. Players can choose Nightmare and still use larger text, reduced motion, captions, clearer warnings, or alternative controls. Challenge and access remain independent.

Finally, the system becomes safe to balance over time.

Preset definitions can evolve for new campaigns while existing saves retain the resolved values they started with. Fixed-seed simulation and Normal-parity tests catch accidental tuning drift. Content-reachability tests make sure high difficulty does not quietly remove essential quest, medical, crafting, or expedition paths.

The result is not nine sliders scattered across nine systems.

It is one coherent campaign challenge profile interpreted correctly by the simulation ASHFALL already has.
