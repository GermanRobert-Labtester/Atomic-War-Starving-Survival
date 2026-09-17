# C1 — Flagship Integration Plan [42]: Shelter Atmosphere, Ambiance, Environmental Character & Presentation Read-Model

> **Output:** `C1_planintegration[42].md`
>
> **Source baseline:** Plan 220 — Shelter Atmosphere & Ambiance System
>
> **Primary mission:** make ASHFALL’s shelter feel like a living place whose lighting, sound, air, temperature, cleanliness, activity, social behavior, decoration, crisis state and routine collectively produce a recognizable atmosphere—without creating a second environmental, morale, health, sleep, productivity, social, or crisis simulation.
>
> **Primary architectural rule:** `ShelterAtmosphereSystem` owns **derived atmosphere interpretation and presentation state**: normalized component projections, atmosphere tags/profiles, descriptive mood bands, trend/hysteresis, player-facing explanation, ambient presentation cues, and semantic “atmosphere changed” milestones. It does not own temperature, air quality, power, lighting supply, smoke, fire, noise, sanitation, survivor relationships, morale, stress, sleep, health, work efficiency, activity scheduling, decorations as physical objects, or shelter identity.
>
> **Primary correction to the source plan:** the source proposes nine component values, a 0–100 overall mood, profiles with direct morale/productivity/health modifiers, persistent `AtmosphereEffect` objects, and an old-save migration that initializes every component to 50. That design risks double-counting almost every underlying system. The flagship therefore makes atmosphere primarily a **read-model / interpretive layer**. Its job is to summarize and present existing shelter conditions coherently, optionally provide one bounded environmental-comfort context to canonical psychology/sleep/work systems, and drive audiovisual ambiance. It must never independently reproduce the same penalties already applied by thermal, ventilation, noise, sanitation, fire, Needs, mental-health, or work-performance systems.
>
> **Primary projection rule:** source systems remain authoritative. Atmosphere never stores a second mutable “airQuality = 42” if `VentilationSystem` already knows the real air state. It reads a canonical source signal and projects it into atmosphere semantics such as `stale`, `fresh`, `smoky`, `comfortable`, `harsh`, `busy`, `quiet`, `welcoming`, or `oppressive`.
>
> **Primary effect rule:** avoid a universal `profile → morale/productivity/health` modifier table. Most physiological consequences already happen at the source. Example: bad air already affects health through ventilation/medical systems; noise already affects sleep/stress through Plan 205; temperature already affects Needs/health. Atmosphere may provide only **non-duplicative integrative context** such as aesthetic comfort, environmental coherence, sense of home, social warmth, or ambient stress—after an authority audit proves those concepts are not already represented.
>
> **Primary mood rule:** “overallMood” is a derived presentation summary, not a canonical gameplay stat. It may be useful for UI and presentation, but consumers should prefer typed atmosphere facets/tags over one scalar.
>
> **Primary profile rule:** Industrial, Sterile, Lived-In, Warm, Cold, Chaotic, Serene and similar profiles are **descriptive pattern matches**, not rigid archetypes that grant global buffs. A shelter can be `industrial + orderly + tense` or `lived-in + crowded + warm`; the model should support multiple simultaneous tags rather than force one mutually exclusive profile.
>
> **Primary identity rule:** Plan 166 / shelter identity remains owner of persistent shelter name, emblem, doctrine, and intentional identity. Atmosphere describes the **current lived environment** and can inform identity presentation, but does not replace it.
>
> **Primary crisis rule:** Plan 158 disaster response and Plan 194 emergency alerts remain crisis authorities. Fire, smoke, flooding, blackouts, radiation emergencies and disease outbreaks can temporarily dominate atmosphere presentation, but atmosphere cannot create a second emergency state or suppress real alerts.
>
> **Primary presentation rule:** the highest-value output of Plan 220 is sensory integration: ambient lighting cues, soundscape selection, UI tone, room ambience, descriptive text, and clear explanation of why the shelter “feels” the way it does.
>
> **Mandatory execution order:** 220A source-system/consumer authority audit → 220B atmosphere observation DTOs and source adapters → 220C facet normalization and typed tags → 220D composite mood/profile read-model → 220E environmental-comfort effect boundary → 220F presentation/audio/lighting/UX integration → 220G shelter identity/decoration/social integration → 220H crisis/seasonal/temporal atmosphere → 220I persistence/migration/idempotence → 220J deterministic trend/hysteresis/anti-thrash → 220K long-horizon balance, accessibility, performance and CI → 220L advanced designer/legacy/trading concepts only as follow-on.
>
> **Critical re-baseline rule:** before creating `ShelterAtmosphereSystem.cs`, inspect `ShelterThermalSystem`, `VentilationSystem`, `PowerGridSystem`, lighting presentation adapters, `ShelterFireHazardSystem`, Plan 205 `ShelterNoiseSystem`, Plan 201 `SanitationSystem`, `DutyRosterSystem`, Plan 188 routines, `SurvivorRelationsSystem`, Needs/mental-health/sleep systems, work-performance projection, Plan 137, Plan 158 emergency response, Plan 194 alerts, Plan 166 shelter identity, decoration/furnishing systems, audiovisual presentation layer, day/night/season/weather state, semantic event bus, save order and shelter UI.
>
> **Guardrails:** no duplicate thermal state; no duplicate air-quality truth; no duplicate noise truth; no duplicate sanitation truth; no duplicate fire/smoke truth; no duplicate survivor relationship score; no duplicate activity simulation; no duplicate decoration inventory; no duplicate morale/stress/sleep/health state; no direct universal productivity multiplier; no direct universal health multiplier; no source component initialized to fake 50 when the real source exists; no `AtmosphereEffect` persistence for consequences owned elsewhere; no forced single-profile classification if multiple facets coexist; no atmosphere event every day; no profile thrashing around thresholds; no per-frame simulation scan; no unseeded RNG; no `Guid.NewGuid`; no wall clock; no quest rewards for repeatedly toggling lights/profile states; no ambient presentation that hides emergency-critical information.

---

# 0. Mission

ASHFALL already has many systems that determine what the shelter physically and socially feels like, but those systems operate independently.

The source baseline identifies:
- `ShelterThermalSystem` for temperature;
- `VentilationSystem` for air quality;
- `PowerGridSystem` for power and lighting availability;
- `ShelterFireHazardSystem` for fire/smoke;
- Plan 205 for noise;
- Plan 201 for sanitation/cleanliness;
- DutyRoster for activity;
- SurvivorRelations for social context;
- no unified atmosphere interpretation layer;
- no persistent shelter “feel” or ambiance profile.

Current shape:

```text
THERMAL ──────────────┐
VENTILATION ──────────┤
POWER/LIGHTING ───────┤
FIRE/SMOKE ───────────┤
NOISE ────────────────┤
SANITATION ───────────┤
DUTIES/ACTIVITY ──────┤
RELATIONS/SOCIAL ─────┤
DECORATION ───────────┤
                      │
                      └── each affects gameplay separately
                          but no coherent shelter atmosphere
```

Target shape:

```text
CANONICAL SOURCE SYSTEMS
        │
        ▼
AtmosphereObservationSet
        │
        ├── thermal comfort
        ├── air freshness
        ├── acoustic character
        ├── lighting character
        ├── cleanliness/order
        ├── activity rhythm
        ├── social warmth/tension
        ├── decoration/lived-in character
        ├── crisis overlays
        └── temporal/seasonal context
        │
        ▼
ShelterAtmosphereSystem
        │
        ├── typed facets
        ├── descriptive tags
        ├── composite mood band
        ├── profile/read-model
        ├── trend/hysteresis
        ├── explanation/provenance
        └── presentation directives
        │
        ▼
PRESENTATION + BOUNDED CONSUMERS
        ├────────► Shelter UI
        ├────────► ambient audio
        ├────────► lighting/color-temperature presentation
        ├────────► environmental descriptions
        ├────────► Plan 166 shelter identity read-model
        ├────────► psychology comfort context (if non-duplicative)
        └────────► Plan 171 narrative hooks
```

The atmosphere layer should answer:

> Given the shelter’s real current conditions, what does the environment feel like, which factors dominate that feeling, how is it changing, and how should the game present that character to the player?

It should not answer:

> What is the shelter temperature?
> Is the air medically hazardous?
> How loud is the generator?
> Is the room sanitary?
> How stressed is Survivor A?
> What is the final work-efficiency multiplier?
> Is a fire emergency active?
> Who likes whom?

Those remain canonical systems.

---

# 1. Source-Evidence Interpretation

## 1.1 A unified ambiance interpretation layer is genuinely absent

The source reports zero Core matches for:
- `AtmosphereSystem`;
- `AmbianceSystem`;
- `ShelterMood`;
- `EnvironmentalMood`;
- `ShelterPersonality`;
- `ShelterCharacter`;
- `AmbientEffect`;
- `ShelterVibe`;
- `AtmosphericEffect`;
- `MoodLighting`.

A new read-model/presentation coordinator is justified.

## 1.2 The source correctly identifies environmental fragmentation

The shelter can be:
- warm;
- clean;
- noisy;
- smoky;
- socially tense;
- brightly lit
all at the same time.

Players currently lack a coherent summary.

## 1.3 A raw 0–100 component mirror is not automatically useful

If each source already has meaningful states:
- cold;
- overheating;
- stale air;
- blackout;
- loud machinery;
- filthy sanitation,
the atmosphere layer should map those semantic states rather than duplicate normalized storage.

## 1.4 The source’s direct profile effects risk double penalties

Example:

```text
noisy shelter
→ ShelterNoiseSystem reduces sleep
→ atmosphere profile = Chaotic
→ atmosphere applies sleep penalty again
```

That must be prevented.

## 1.5 Profiles should be descriptive, not prescriptive

`Industrial` should not mean:
- automatic productivity bonus;
- automatic morale penalty.

Instead it can describe:
- utilitarian lighting;
- high machine activity;
- sparse decoration;
- orderly functional space.

Whether that helps work or harms morale should come from canonical systems or an explicitly audited non-duplicative comfort adapter.

## 1.6 “Social energy” must not be average relationship affinity

A shelter can have:
- many positive relations;
- active grief;
- a few severe conflicts;
- high social activity.

Use a richer observation from relations/conflict/routines.

## 1.7 “Activity level” should not equate busyness with goodness

High activity can mean:
- productive;
- overcrowded;
- chaotic;
- emergency response.

Atmosphere should describe activity, not reward it blindly.

## 1.8 “Lighting quality” is context-dependent

Bright light is not always positive.

Examples:
- work area bright = useful;
- sleeping area bright at night = disruptive;
- warm low lighting in leisure area = comfortable.

If room-level lighting exists, prefer context-aware projection.

---

# 2. Non-Negotiable Atmosphere Invariants

## INV-220.1 — One thermal authority

## INV-220.2 — One ventilation/air authority

## INV-220.3 — One power authority

## INV-220.4 — One fire/smoke authority

## INV-220.5 — One noise authority

## INV-220.6 — One sanitation authority

## INV-220.7 — One relationship/social-state authority

## INV-220.8 — One Needs/mental-health authority

## INV-220.9 — One sleep authority

## INV-220.10 — One work-performance authority

## INV-220.11 — Atmosphere is derived from source observations

## INV-220.12 — Atmosphere source values are rebuildable

## INV-220.13 — Overall mood is presentation summary, not master stat

## INV-220.14 — Tags/profiles are descriptive

## INV-220.15 — Source penalties are never duplicated

## INV-220.16 — Atmosphere effects require explicit non-duplication proof

## INV-220.17 — Crisis state remains external

## INV-220.18 — Shelter identity remains external

## INV-220.19 — Presentation directives cannot conceal hazards

## INV-220.20 — Threshold transitions use hysteresis

## INV-220.21 — Old saves derive from current source systems

## INV-220.22 — No fake historical atmosphere events on migration

## INV-220.23 — No per-frame simulation ownership

## INV-220.24 — Headless atmosphere projection is deterministic

---

# 3. Definition of Done

Plan 220 closes only when:

- every atmosphere source has a documented canonical owner;
- every proposed downstream effect has a documented canonical consumer;
- thermal/air/noise/sanitation/fire/power values are read rather than duplicated;
- lighting semantics distinguish availability from presentation quality;
- activity semantics distinguish rhythm from productivity;
- social atmosphere uses relations/conflict/routines without averaging one affinity score blindly;
- decoration reads real furnishing/improvement state;
- crisis overlays read Plan 158/194/fire/hazard state;
- atmosphere facets are typed and explainable;
- overall mood is derived, optional and presentation-only;
- multiple simultaneous atmosphere tags are supported;
- profiles have hysteresis and do not thrash daily;
- atmosphere does not independently reapply health penalties already caused by air, temperature, fire or sanitation;
- atmosphere does not independently reapply sleep penalties already caused by noise/light/Needs;
- atmosphere does not independently reapply work penalties already caused by Needs/Plan 137;
- any comfort/stress effect uses a single bounded environmental-comfort input after double-count audit;
- atmosphere events are semantic milestone changes, not source-value chatter;
- old saves derive atmosphere immediately from real current state rather than setting every component to 50;
- no retroactive history is fabricated;
- source-state changes produce deterministic atmosphere projection;
- UI explains dominant causes;
- ambient audio/lighting/presentation changes are reversible and deterministic;
- emergency warnings always override cosmetic presentation;
- 30/120/180/400-day simulations show stable atmosphere changes without effect stacking or profile thrash;
- `--shelter-atmosphere-selftest` exists or equivalent;
- profile/tag data passes content and source reachability.

---

# 4. Phase P0 — Source-System & Consumer Authority Audit

## P0.1 Capture baseline

Record:

```text
commit SHA
branch
dirty paths
ShelterThermalSystem
VentilationSystem
PowerGridSystem
lighting presentation
ShelterFireHazardSystem
Plan 205 ShelterNoiseSystem
Plan 201 SanitationSystem
DutyRosterSystem
Plan 188 routines
SurvivorRelationsSystem
Plan 202 conflicts
NeedsSystem
mental-health/stress
sleep system
Plan 137 performance
Plan 143 capability
Plan 158 emergency response
Plan 194 alerts
Plan 166 shelter identity
decoration/furniture/improvement system
day/night clock
season/weather
ambient audio layer
camera/lighting/visual presentation
semantic event bus
save order
shelter UI
```

## P0.2 Build atmosphere authority matrix

Create:

`docs/atmosphere/SHELTER_ATMOSPHERE_AUTHORITY_MATRIX.md`

Columns:

```text
fact
canonical owner
source API/event
atmosphere role
consumer
persisted?
status
```

Rows:
- temperature;
- thermal comfort;
- air quality;
- smoke;
- power;
- lighting availability;
- lighting mood/presentation;
- noise;
- sanitation;
- cleanliness;
- activity;
- duty busyness;
- social warmth;
- conflict;
- decoration;
- shelter identity;
- crisis;
- overall atmosphere;
- mood band;
- profile/tag;
- comfort context;
- ambient audio;
- UI presentation.

## P0.3 Atmosphere-vs-source ADR

Create:

`docs/architecture/ADR_ATMOSPHERE_AS_DERIVED_READ_MODEL.md`

## P0.4 Effect-boundary ADR

Create:

`ADR_ATMOSPHERE_EFFECTS_AND_DOUBLE_COUNTING.md`

## P0.5 Profile ADR

Create:

`ADR_ATMOSPHERE_TAGS_VS_EXCLUSIVE_PROFILES.md`

## P0.6 Old-save ADR

Create:

`ADR_ATMOSPHERE_OLD_SAVE_DERIVATION.md`

## P0.7 Baseline proof

Capture screenshots/logs of:
- blackout;
- comfortable shelter;
- noisy shelter;
- smoky fire;
- clean but socially tense shelter.

Show that source systems change independently but no unified character exists.

---

# TASK 220A — Atmosphere Observation Contract

# 220A.0 Goal

Define a stable read-only bridge from source systems into atmosphere.

## 220A.1 Proposed owner

`Assets/Ashfall.Core/Shelter/ShelterAtmosphereSystem.cs`

## 220A.2 Observation DTO

Suggested:

```text
AtmosphereObservationSet
  day
  shelter_id
  thermal_observation
  air_observation
  acoustic_observation
  lighting_observation
  cleanliness_observation
  activity_observation
  social_observation
  decoration_observation
  crisis_observation
  temporal_observation
  source_revisions[]
```

## 220A.3 Source revision

Useful for:
- cache invalidation.

## 220A.4 Do not copy full source state

Only read needed projections.

## 220A.5 Thermal observation

Suggested:

```text
thermal_band
comfort_distance
occupied_area_fraction_affected
source_ref
```

## 220A.6 Air observation

```text
air_band
smoke_present
contamination/staleness flags
occupied_area_fraction_affected
```

## 220A.7 Acoustic observation

```text
quiet/moderate/noisy/disruptive
night_noise
intermittent/continuous
```

## 220A.8 Lighting observation

Separate:
- powered availability;
- functional task lighting;
- ambient warmth;
- nighttime darkness/rest suitability.

## 220A.9 Cleanliness observation

From sanitation:
- clean;
- lived-in;
- dirty;
- hazardous.

## 220A.10 Activity observation

From DutyRoster/routines:
- dormant;
- quiet;
- steady;
- busy;
- overloaded;
- emergency.

## 220A.11 Social observation

From relations/conflict:
- isolated;
- subdued;
- cordial;
- warm;
- tense;
- hostile;
- grieving.

Allow multiple flags.

## 220A.12 Decoration observation

From real objects/improvements:
- bare;
- functional;
- personal;
- decorated;
- memorial-heavy;
- cluttered
if data supports.

## 220A.13 Crisis observation

Read:
- emergency severity;
- active hazards.

## 220A.14 Temporal observation

- day/night;
- season;
- special event.

## 220A.15 No RNG

Observation is deterministic.

### 220A DoD

Atmosphere consumes compact semantic observations from each canonical source without copying or owning their underlying simulation state.

---

# TASK 220B — Facet Normalization & Typed Atmosphere Tags

# 220B.0 Goal

Translate heterogeneous source observations into a coherent descriptive language.

## 220B.1 Facets

Recommended:

```text
comfort
order
social_warmth
activity_rhythm
sensory_harshness
homeliness
safety_feel
crowding/oppression if supported
```

## 220B.2 Do not map every source to “good/bad”

Example:
- high activity can be vibrant or chaotic.

## 220B.3 Comfort

Derived from:
- thermal comfort;
- air;
- noise;
- lighting/rest suitability;
- cleanliness.

But do not replace source effects.

## 220B.4 Order

Derived from:
- sanitation;
- maintenance;
- clutter;
- work rhythm
if supported.

## 220B.5 Social warmth

Derived from:
- positive interaction;
- conflict;
- grief;
- isolation;
- communal activity.

## 220B.6 Activity rhythm

Descriptive:
- stagnant;
- quiet;
- steady;
- bustling;
- frantic.

## 220B.7 Sensory harshness

- loud;
- smoky;
- glaring;
- dark;
- cold;
- mechanical.

## 220B.8 Homeliness

- decoration;
- personal effects;
- memorials;
- lived-in routines;
- communal spaces.

## 220B.9 Safety feel

Perceived ambient safety:
- alarms;
- smoke;
- emergency state;
- security lockdown
if known.

Not actual combat safety score.

## 220B.10 Tags

Examples:
- industrial;
- sterile;
- lived_in;
- warm;
- cold;
- chaotic;
- serene;
- tense;
- welcoming;
- oppressive;
- smoky;
- dim;
- bustling;
- quiet;
- grieving;
- celebratory.

## 220B.11 Multiple tags

Mandatory.

## 220B.12 Tag activation

Rule-based scoring.

## 220B.13 Tag confidence

Optional.

## 220B.14 Tag hysteresis

Enter threshold > exit threshold.

## 220B.15 Contradictory tags

Some can coexist:
- industrial + welcoming;
- sterile + serene;
- lived_in + chaotic.

## 220B.16 Mutually exclusive tags

Only where semantically impossible.

## 220B.17 Data file

`Assets/StreamingAssets/Data/atmosphere_profiles.json`

Prefer contents:
- tag/profile match rules;
- presentation directives;
- localization.

Not source-state truth.

### 220B DoD

Atmosphere becomes a typed multidimensional interpretation of real shelter conditions instead of a single good/bad average.

---

# TASK 220C — Overall Mood Read-Model

# 220C.0 Goal

Provide a useful top-level summary without creating a gameplay master stat.

## 220C.1 Source `overallMood`

Keep only as:
- derived presentation index.

## 220C.2 Suggested name

```text
atmosphere_summary_score
```

or
```text
ambient_comfort_index
```

if semantics justify.

## 220C.3 Avoid false precision

UI bands:
- bleak;
- tense;
- neutral;
- comfortable;
- welcoming;
- vibrant.

## 220C.4 “Vibrant” is not necessarily best

A serene shelter can be excellent without high vibrancy.

## 220C.5 Mood category

May be derived from multiple facets, not raw average.

## 220C.6 Pattern classifier

Example:

```text
comfortable + social_warmth high + homeliness high
→ welcoming

comfort moderate + activity high + order low
→ chaotic/busy

comfort low + crisis high
→ tense/bleak
```

## 220C.7 No universal monotonic 0→100 hierarchy

Prefer semantic categories.

## 220C.8 Trend

Derived over:
- meaningful daily/weekly samples.

## 220C.9 Trend hysteresis

Avoid:
- improving/declining flip every day.

## 220C.10 Overall score persistence

Do not persist if fully rebuildable.

## 220C.11 History

Store bounded samples if UI graph requires.

### 220C DoD

Players receive a concise atmosphere summary while all actual gameplay effects continue to originate in specific source systems and typed facets.

---

# TASK 220D — Profile & Shelter Character Matching

# 220D.0 Goal

Turn source profile concepts into descriptive pattern archetypes rather than buff packages.

## 220D.1 Industrial

Potential pattern:
- machinery;
- functional lighting;
- sparse decoration;
- high productive activity;
- mechanical noise.

## 220D.2 Sterile

- high cleanliness;
- bright functional lighting;
- low clutter;
- low personal decoration;
- controlled air.

## 220D.3 Lived-In

- personal effects;
- moderate wear;
- communal activity;
- decoration;
- routine occupancy.

## 220D.4 Warm

Use carefully.

Could mean:
- socially warm;
- visually warm;
- thermally warm.

Split:
- `welcoming`;
- `warm_lighting`;
- `thermally_warm`.

## 220D.5 Cold

Same ambiguity.

Prefer:
- `austere`;
- `thermally_cold`.

## 220D.6 Chaotic

- high noise;
- irregular activity;
- clutter;
- conflict/emergency.

## 220D.7 Serene

- low noise;
- stable lighting;
- clean air;
- low conflict;
- orderly routines.

## 220D.8 Profile name

Localization only.

## 220D.9 Profile effects

Remove direct global modifiers from profile DTO.

## 220D.10 Presentation package

Profiles can supply:
- ambient audio family;
- UI descriptive text;
- color-temperature guidance;
- environmental flavor tags.

## 220D.11 Multi-profile

Allow:
- primary + secondary tags.

## 220D.12 Change threshold

Require sustained pattern or significant event.

## 220D.13 No profile min-max quest exploit

Changing one lamp should not flip entire shelter profile.

### 220D DoD

Atmosphere profiles become stable, descriptive environmental identities with presentation value rather than hidden buff/debuff loadouts.

---

# TASK 220E — Environmental Comfort Effect Boundary

# 220E.0 Goal

Decide whether atmosphere contributes any gameplay effect without double-counting.

## 220E.1 Default stance

Presentation-only until proven otherwise.

## 220E.2 Audit each source effect

### Morale
Does Needs/psychology already respond to:
- temperature;
- noise;
- social state;
- sanitation;
- light?

If yes:
- do not reapply.

### Productivity
Does Plan 137/work system already consume:
- fatigue;
- temperature;
- noise;
- illness;
- lighting?

If yes:
- no atmosphere productivity modifier.

### Health
Does medical already consume:
- air;
- sanitation;
- temperature;
- smoke?

If yes:
- no atmosphere health modifier.

### Stress
Does mental-health already consume:
- noise;
- conflict;
- emergency?

If yes:
- no duplicate stress modifier.

### Sleep
Does sleep already consume:
- noise;
- light;
- temperature?

If yes:
- no atmosphere sleep modifier.

## 220E.3 Allowed integrative concept

Potential:
- `environmental_comfort_context`.

Only if it represents non-duplicated aesthetic/social homeliness.

## 220E.4 Example valid sources

- decoration;
- personal effects;
- coherent communal space;
- non-hazardous ambient presentation.

## 220E.5 Bounded magnitude

Small.

## 220E.6 No linear sum of source systems

## 220E.7 Consumer-owned interpretation

Psychology/Needs receives:
- context.

Atmosphere does not mutate morale directly.

## 220E.8 Persistence

No `AtmosphereEffect` state for permanent modifiers.

## 220E.9 Effect provenance

Diagnostics must show:
- source facets;
- duplicate exclusions.

## 220E.10 If no unique effect remains

Ship atmosphere as:
- presentation/read-model only.

That is acceptable.

### 220E DoD

Atmosphere either stays presentation-only or contributes one explicitly non-duplicative, bounded environmental-comfort context through canonical consumers.

---

# TASK 220F — Lighting & Visual Presentation Integration

# 220F.0 Goal

Make atmosphere visually tangible without altering power or sleep truth.

## 220F.1 PowerGrid owns power availability

## 220F.2 Lighting system owns real light fixtures if one exists

## 220F.3 Atmosphere presentation can choose

- color-temperature family;
- local intensity accents;
- vignette/ambient tint;
- emergency override palette;
- flicker profile when canonical power state says unstable.

## 220F.4 No fake lighting

If power is off:
- ambiance cannot pretend lights are on.

## 220F.5 No global brightness that hides hazards

## 220F.6 Day/night

Use canonical clock.

## 220F.7 Sleep hours

Presentation can dim leisure areas if fixtures/policy support.

## 220F.8 Emergency

Plan 194 alert visual language overrides atmosphere.

## 220F.9 Fire/smoke

Hazard visibility priority.

## 220F.10 Accessibility

Avoid color-only hazard vs ambiance distinction.

## 220F.11 Performance

No expensive whole-scene relighting every simulation tick.

Use:
- event-based presentation updates.

### 220F DoD

Atmosphere visibly changes the shelter’s presentation while power, real lighting state, hazards and accessibility remain authoritative.

---

# TASK 220G — Ambient Audio & Soundscape Integration

# 220G.0 Goal

Make the shelter’s acoustic character coherent without duplicating Plan 205 noise simulation.

## 220G.1 Plan 205 owns gameplay noise

## 220G.2 Atmosphere audio layer owns presentation mix selection

Examples:
- machinery hum;
- quiet ventilation;
- distant chatter;
- crowded work rhythm;
- tense silence;
- storm muffling;
- emergency alarm override.

## 220G.3 No acoustic gameplay penalty here

## 220G.4 Noise level source

Read canonical noise.

## 220G.5 Social chatter

Based on:
- activity;
- social warmth;
- population presence.

## 220G.6 Crisis

Emergency audio overrides.

## 220G.7 Dynamic mix

Crossfade by tags.

## 220G.8 No random audio event simulation in Core

Presentation layer can choose deterministic/seeded variants.

## 220G.9 Save

Do not persist current audio clip.

Rebuild from atmosphere.

## 220G.10 Accessibility

Critical alerts distinct from ambience.

### 220G DoD

Ambient sound makes atmosphere perceptible while Plan 205 remains the only authority for acoustic gameplay effects.

---

# TASK 220H — Cleanliness, Order & Sanitation Integration

# 220H.0 Goal

Represent visual/social cleanliness without duplicating Plan 201 health effects.

## 220H.1 Plan 201 owns sanitation

## 220H.2 Atmosphere reads

- clean;
- dirty;
- hazardous;
- clutter/odor if exposed.

## 220H.3 Health penalties

Stay Plan 201/medical.

## 220H.4 Visual presentation

Can show:
- clean surfaces;
- clutter;
- trash;
- grime
if asset system supports.

## 220H.5 “Sterile”

Requires:
- cleanliness + presentation pattern.

Not direct health bonus.

## 220H.6 “Lived-in”

Can coexist with clean.

Avoid:
- lived-in = dirty.

## 220H.7 Orderliness

Separate from sanitation if storage/clutter exists.

### 220H DoD

Cleanliness contributes to atmosphere character and visuals while all sanitation-related disease/health effects remain owned by sanitation/medical systems.

---

# TASK 220I — Activity, Routine & Work Rhythm Integration

# 220I.0 Goal

Describe how busy or stagnant the shelter feels without equating busyness with productivity.

## 220I.1 DutyRoster owns assignments

## 220I.2 Plan 188 owns routines

## 220I.3 Activity observation

Measure:
- awake active survivors;
- scheduled work;
- leisure;
- emergency activity;
- idle population.

## 220I.4 Activity labels

- dormant;
- quiet;
- steady;
- bustling;
- overloaded;
- emergency.

## 220I.5 High activity

Not automatically positive.

## 220I.6 Low activity

Could be:
- peaceful;
- nighttime;
- illness;
- collapse.

Context matters.

## 220I.7 Productivity

Do not derive from atmosphere.

## 220I.8 Temporal weighting

Night should naturally be quieter.

## 220I.9 Shift changes

Can alter ambiance presentation.

## 220I.10 No daily activity reward

### 220I DoD

Atmosphere reflects the shelter’s real daily rhythm without duplicating work efficiency or rewarding constant busyness.

---

# TASK 220J — Social Energy, Relations & Conflict Integration

# 220J.0 Goal

Make interpersonal climate visible without creating a second social score.

## 220J.1 SurvivorRelations owns affinity/trust

## 220J.2 Plan 202 owns active conflict

## 220J.3 Mental health owns stress/grief

## 220J.4 Social observation

Can include:
- positive communal interaction rate;
- severe conflicts;
- isolation;
- grief;
- celebration;
- communal meals/events.

## 220J.5 No average affinity = social energy

## 220J.6 Social warmth

Derived from:
- actual social events;
- conflict load;
- relation context.

## 220J.7 Tense

Could arise from:
- conflict;
- crisis;
- fear.

Use provenance.

## 220J.8 Grieving

Could be a tag when:
- recent death;
- memorial period
if canonical memory/mental health supports.

## 220J.9 Celebratory

Requires:
- real event.

## 220J.10 No direct morale effect

Unless unique comfort adapter is approved.

## 220J.11 No feedback loop

Atmosphere should not:
- social warmth → relations increase → more social warmth
without real event.

### 220J DoD

Atmosphere communicates the shelter’s social climate through canonical relations, conflict and social events without storing or mutating a second relationship/morale state.

---

# TASK 220K — Decoration, Personal Effects & Homeliness

# 220K.0 Goal

Make decoration and lived-in character matter as uniquely atmospheric inputs.

## 220K.1 Decoration authority

Audit:
- furniture;
- improvements;
- cosmetic objects;
- Plan 210 personal belongings;
- memorials.

## 220K.2 Decoration observation

Potential:
- bare;
- functional;
- personalized;
- decorated;
- commemorative;
- cluttered.

## 220K.3 Physical objects remain canonical

Atmosphere stores no decoration inventory.

## 220K.4 Personal effects

Plan 210 can increase:
- homeliness/personalization
when displayed or used in rooms.

Do not count items hidden in storage equally.

## 220K.5 Memorials

May add:
- solemn;
- commemorative;
not simple positive decoration.

## 220K.6 Art/decor

Can support non-duplicative environmental comfort.

## 220K.7 Diminishing returns

Ten identical posters ≠ 10× homeliness.

## 220K.8 Room distribution

If room-level system exists:
- local decoration matters locally.

## 220K.9 Global fallback

If shelter is not room-simulated:
- shelter-wide aggregate is acceptable.

## 220K.10 No decoration grind

### 220K DoD

Decoration and personal effects provide a real, non-duplicative homeliness input while all physical furnishings remain in their canonical systems.

---

# TASK 220L — Shelter Identity / Plan 166 Integration

# 220L.0 Goal

Connect current atmosphere to persistent shelter identity without merging them.

## 220L.1 Plan 166 owns

- shelter name;
- emblem;
- intentional identity;
- doctrine/branding if any.

## 220L.2 Atmosphere owns

- current lived feel.

## 220L.3 Identity can influence presentation choices

Example:
- named sanctuary may favor welcoming decor
only if physically present.

## 220L.4 Atmosphere can contradict identity

A self-declared “Sanctuary” can currently feel:
- cold;
- overcrowded;
- tense.

Important.

## 220L.5 UI

Show:
- Identity: Sanctuary of Ash
- Current atmosphere: crowded, tense, lived-in

## 220L.6 No automatic identity effect

Name does not grant atmosphere.

### 220L DoD

The shelter’s intended identity and its actual lived atmosphere remain distinct, allowing meaningful mismatch and evolution.

---

# TASK 220M — Crisis, Emergency & Hazard Overlays

# 220M.0 Goal

Let emergencies dominate the shelter’s feel without duplicating emergency state.

## 220M.1 Plan 158 owns response

## 220M.2 Plan 194 owns alerts

## 220M.3 Fire system owns fire/smoke

## 220M.4 Crisis overlay

Derived tags:
- emergency;
- smoky;
- blackout;
- evacuation;
- medical crisis;
- lockdown.

## 220M.5 Crisis dominance

Presentation may temporarily override normal profile.

## 220M.6 Example

Normal:
- serene + lived-in.

During fire:
- emergency + smoky + chaotic.

After:
- damaged + subdued + recovering.

## 220M.7 No duplicate emergency timer

## 220M.8 No duplicate panic

Mental health/autonomy owns.

## 220M.9 No duplicate productivity penalty

Response/work systems own.

## 220M.10 Recovery atmosphere

Can be presentation/narrative tag after incident.

## 220M.11 Alert precedence

Emergency alert UI/audio > atmosphere.

### 220M DoD

Atmosphere responds visibly to crises while emergency, hazard, panic and work consequences remain entirely canonical.

---

# TASK 220N — Temporal, Seasonal & Weather Atmosphere

# 220N.0 Goal

Make ambiance vary with time and season without creating a second weather/calendar simulation.

## 220N.1 Calendar owns day/time

## 220N.2 Weather owns external weather

## 220N.3 Season owns seasonal state

## 220N.4 Atmosphere presentation can incorporate

- night quiet;
- winter dimness;
- storm acoustics;
- summer ventilation;
- nuclear-winter gloom.

## 220N.5 No gameplay weather modifier here

## 220N.6 Interior dependence

External weather only matters indoors if:
- audible;
- visible;
- thermal/power systems transmit effect.

## 220N.7 Seasonal decoration

If real decor/event system exists.

## 220N.8 Day/night

Can influence:
- lighting/audio presentation;
- activity-rhythm interpretation.

## 220N.9 No wall-clock

Campaign time only.

### 220N DoD

Atmosphere presentation reflects canonical time, season and weather without duplicating any environmental mechanics.

---

# TASK 220O — Trend, Hysteresis & Anti-Thrash

# 220O.0 Goal

Prevent atmosphere labels and presentation from flipping constantly around thresholds.

## 220O.1 Source source-values may fluctuate often

Example:
- generator turns on/off;
- noise changes by shift;
- temperature oscillates.

## 220O.2 Smoothing

Use:
- EMA;
- sustained threshold;
- minimum dwell time
for descriptive profile changes.

## 220O.3 Emergencies bypass smoothing

Immediate.

## 220O.4 Tag entry/exit thresholds

Different.

## 220O.5 Profile dwell

Example:
- primary profile must persist 1–3 game days
unless major event.

## 220O.6 UI trend

Weekly/smoothed.

## 220O.7 Audio crossfade

Presentation smoothing.

## 220O.8 No gameplay delay

Do not delay actual hazard consequences.

Only atmosphere label/presentation.

## 220O.9 Event emission

Only:
- meaningful band/tag/profile change.

## 220O.10 No daily “improved by 1 point” event.

### 220O DoD

Atmosphere remains stable and readable under noisy source inputs while urgent hazards still update immediately.

---

# TASK 220P — Atmosphere Events & Semantic History

# 220P.0 Goal

Record meaningful environmental-character transitions without duplicating every source event.

## 220P.1 Semantic events

Candidate:

```text
shelter_atmosphere_tag_activated
shelter_atmosphere_tag_deactivated
shelter_atmosphere_profile_changed
shelter_atmosphere_comfort_band_changed
shelter_atmosphere_crisis_overlay_changed
```

## 220P.2 Source event names

- The Shift;
- The Profile;
- The Improvement;
- The Decline;
- The Comfort;
- The Tension;
- The Serenity;
- The Chaos.

Treat as narrative candidates.

## 220P.3 Avoid duplicate source events

If:
- fire starts,
fire system emits event.
Atmosphere need not emit another “air changed” event every tick.

## 220P.4 Narrative trigger

Could emit:
- “Shelter became Serene”
only after sustained state.

## 220P.5 History

Store:
- profile/tag milestones;
- dominant causes;
- source refs.

## 220P.6 No localized descriptions persisted

## 220P.7 Bounded

Weekly/monthly or event-based.

### 220P DoD

Atmosphere history captures meaningful changes in environmental character rather than mirroring every underlying system transition.

---

# TASK 220Q — UI, Explanation & Cause Trace

# 220Q.0 Goal

Make the shelter’s feel legible and actionable without presenting another management dashboard of duplicate numbers.

## 220Q.1 Atmosphere panel

Show:
- current descriptive summary;
- dominant tags;
- trend;
- top positive/negative contributors.

## 220Q.2 Example

```text
Current atmosphere:
Lived-in · Tense · Dim

Why:
+ warm sleeping areas
+ personal decoration
- generator noise
- unresolved conflicts
- low evening lighting
```

## 220Q.3 Component detail

Prefer source semantics:
- temperature comfortable;
- air stale;
- noise disruptive.

Do not show duplicate normalized 0–100 unless useful/debug.

## 220Q.4 Profile display

Primary descriptive profile + secondary tags.

## 220Q.5 Effect display

Only if real unique comfort context exists.

Show:
- bounded effect;
- source;
- excluded duplicate effects.

## 220Q.6 Deep links

To:
- thermal;
- ventilation;
- sanitation;
- power;
- noise;
- relations/conflict;
- decoration.

## 220Q.7 Trend graph

Weekly summary, not noisy daily raw score.

## 220Q.8 Alerts

Only:
- major profile shift;
- severe deterioration;
- major recovery.

## 220Q.9 No “air changed” atmosphere alert

Source alert handles hazardous air.

## 220Q.10 Tutorial

First sustained atmosphere classification or first meaningful change.

## 220Q.11 Tooltips

Not hover-only.

## 220Q.12 Accessibility

- no color-only positive/negative;
- text tags;
- keyboard/controller;
- text scaling;
- screen-reader cause order.

## 220Q.13 Debug

Optional raw facet scores.

### 220Q DoD

Players can understand what the shelter feels like and why, then navigate to the real systems responsible for improving it.

---

# TASK 220R — Ambient Presentation Contract

# 220R.0 Goal

Define a stable bridge from simulation atmosphere to presentation systems.

## 220R.1 DTO

Suggested:

```text
AtmospherePresentationContext
  primary_profile
  tags[]
  lighting_theme
  soundscape_theme
  ambient_activity_theme
  visual_clutter_theme
  urgency_overlay
  transition_duration
  provenance_refs[]
```

## 220R.2 Read-only

Presentation cannot mutate simulation.

## 220R.3 Lighting theme

Guidance only.

## 220R.4 Soundscape theme

Guidance only.

## 220R.5 Ambient NPC behavior

If decorative animations exist:
- choose idle/chatter/busy presets.

Do not alter actual duties.

## 220R.6 Visual clutter

Only if real assets/objects support.

## 220R.7 Emergency overlay

Canonical alert priority.

## 220R.8 Deterministic selection

Profile/tag hash can choose stable variation.

## 220R.9 No save dependency

Rebuild.

### 220R DoD

Simulation exports a compact immutable ambiance context that presentation layers can consume without changing gameplay state.

---

# TASK 220S — Persistence, Old Saves & Restore

# 220S.0 Goal

Persist only atmosphere-specific history/configuration while deriving current state from sources.

## 220S.1 Persist

Suggested:

```text
schema_version
feature_activation_day
profile_hysteresis_state
tag_hysteresis_state
last_significant_transition_day
bounded_history_samples[]
player_atmosphere_ui_preferences optional
processed_transition_ids[]
```

## 220S.2 Do not persist duplicate

- temperature;
- air quality;
- power;
- smoke/fire;
- noise;
- sanitation;
- duty activity;
- relations;
- Needs;
- sleep;
- health;
- productivity;
- decoration inventory.

## 220S.3 Current atmosphere

Recompute after restore.

## 220S.4 Old save

Do not set every component to 50.

Instead:
1. restore canonical source systems;
2. build current observations;
3. derive current atmosphere;
4. initialize hysteresis to current stable state;
5. no historical atmosphere log.

## 220S.5 Behavioral parity

No gameplay effect until unique effect audit passes.

## 220S.6 Missing source

Mark facet:
- unavailable;
not `50`.

## 220S.7 New source added later

Projection can incorporate.

## 220S.8 Restore ordering

After source systems.

## 220S.9 No transition spam on restore

Initial derivation is silent.

## 220S.10 Side-effect replay

None.

### 220S DoD

Atmosphere restore reconstructs current character from canonical shelter state, preserving old-save behavior and avoiding fake neutral values or transition spam.

---

# TASK 220T — Determinism & Exploit Prevention

# 220T.0 Goal

Prevent profile toggling, save-scumming and source-state gaming.

## 220T.1 Projection deterministic

Same source observations:
- same facets/tags/profile.

## 220T.2 No RNG needed for simulation

## 220T.3 Presentation variations

If randomized:
- stable seeded presentation layer only.

## 220T.4 Profile hysteresis

Prevents lamp-toggle farming.

## 220T.5 Quest exploit

Changing one component briefly cannot count as:
- sustained profile.

## 220T.6 Maintain-duration quests

Use:
- committed stable state duration.

## 220T.7 “Transformer” quest

Source proposes change profile 5 times.

Reject by default:
- incentivizes toggling.

## 220T.8 “Optimizer”

Do not reward hidden productivity buff optimization.

## 220T.9 All-components-above-70

Avoid if components are not canonical 0–100.

Use semantic quality conditions.

## 220T.10 Save reload

No profile reroll.

## 220T.11 Event exactly once

Stable transition ID.

## 220T.12 No GUID

## 220T.13 No wall clock

### 220T DoD

Atmosphere progression and narrative milestones cannot be farmed by rapid source toggles, reloads or arbitrary score targets.

---

# TASK 220U — Performance & Scaling

# 220U.0 Goal

Keep atmosphere cheap and presentation-friendly.

## 220U.1 Event-driven invalidation

Preferred.

## 220U.2 Daily fallback

Acceptable.

## 220U.3 No per-frame Core recompute

## 220U.4 Source revisions

Recompute only changed facets.

## 220U.5 Presentation update

On meaningful atmosphere context change.

## 220U.6 History bounded

## 220U.7 Room-level future

If room-level atmosphere is added later:
- spatial index required.

Not MVP.

## 220U.8 Large shelter

Cost should not scale with:
- every item;
- every relationship pair
per tick.

Use source summaries.

## 220U.9 Social source

Relations system supplies aggregate observation.

## 220U.10 Decoration source

Decoration system supplies aggregate.

## 220U.11 Benchmark

1, 10, 100 source changes/day.

## 220U.12 Presentation crossfade performance

Separate profiling.

### 220U DoD

Atmosphere costs scale with meaningful source changes and compact aggregate observations rather than scanning the full shelter state every frame.

---

# TASK 220V — Long-Horizon Balance & Simulation

# 220V.0 Goal

Prove atmosphere remains descriptive, stable and non-duplicative over long campaigns.

## 220V.1 30-day neutral shelter

Moderate:
- temp;
- air;
- noise;
- sanitation;
- social state.

Expected:
- stable neutral/lived-in profile;
- no hidden penalties.

## 220V.2 30-day industrial shelter

High machinery;
sparse decor;
steady work.

Expected:
- industrial tags;
- no automatic productivity bonus.

## 220V.3 30-day serene shelter

Quiet;
clean;
stable;
socially calm.

Expected:
- serene;
- no duplicated sleep/stress bonus.

## 220V.4 30-day chaotic shelter

Noise;
crowding/activity;
conflict.

Expected:
- chaotic/tense;
- source systems own consequences.

## 220V.5 120-day evolving shelter

Bare → lived-in → crisis → recovery.

Verify:
- transitions stable;
- history meaningful.

## 220V.6 180-day power instability

Frequent outages.

Verify:
- no profile thrash;
- source power consequences only once.

## 220V.7 180-day sanitation problem

No duplicate health penalty.

## 220V.8 180-day noise problem

No duplicate sleep/stress penalty.

## 220V.9 180-day thermal extremes

No duplicate health/work penalty.

## 220V.10 180-day social conflict

No duplicate morale/relations penalty.

## 220V.11 400-day mature shelter

Rich decoration;
changing population;
seasonal variation.

Verify:
- bounded history;
- stable identity/atmosphere distinction.

## 220V.12 Fire emergency

Immediate crisis overlay.

## 220V.13 Alert priority

Emergency presentation wins.

## 220V.14 Old save

Derived instantly from real source state.

## 220V.15 Missing source

Facet unavailable, not fake neutral.

## 220V.16 Presentation-only mode

All gameplay outcomes identical to source-only baseline.

## 220V.17 Optional comfort adapter mode

Only one bounded non-duplicative difference.

### 220V DoD

Long-horizon simulation proves atmosphere adds coherent character and presentation without multiplying existing environmental or social penalties.

---

# TASK 220W — Testing & CI

# 220W.0 Goal

Make source authority, double-count prevention, determinism and presentation derivation continuously verifiable.

## 220W.1 Data integrity

Validate:
- tag IDs;
- profile IDs;
- source adapters;
- localization;
- presentation themes;
- mutually exclusive rules;
- hysteresis thresholds.

## 220W.2 Selftest

Create:

```text
--shelter-atmosphere-selftest
```

## 220W.3 Selftest scenarios

At least:

1. neutral/current shelter;
2. blackout;
3. noisy shelter;
4. smoky fire;
5. dirty shelter;
6. socially tense shelter;
7. decorated/lived-in shelter;
8. industrial pattern;
9. sterile pattern;
10. serene pattern;
11. chaotic pattern;
12. multi-tag profile;
13. hysteresis;
14. emergency bypass;
15. no duplicate noise effect;
16. no duplicate thermal effect;
17. no duplicate sanitation effect;
18. no duplicate air effect;
19. old save derived state;
20. missing source;
21. save/load;
22. headless.

## 220W.4 Source-scan authority gate

Detect:
- local persisted temperature;
- local persisted air quality;
- local persisted noise;
- local persisted sanitation;
- direct morale mutation;
- direct stress mutation;
- direct sleep mutation;
- direct health mutation;
- direct productivity mutation;
- per-frame Core scan;
- unseeded RNG.

## 220W.5 Content acceptance

Profile ladder:

```text
DISCOVERED
LOADED
REGISTERED
SOURCE_RULES_RESOLVED
OBSERVATIONS_CREATED
TAGS_MATCHED
PRESENTATION_CONTEXT_CREATED
UI/PRESENTATION_CONSUMER_OBSERVED
```

## 220W.6 Dead-profile gate

No profile with:
- impossible match;
- no presentation meaning.

## 220W.7 Dead-tag gate

No tag without source path or UI/presentation meaning.

## 220W.8 Double-count regression gate

Source consequence with atmosphere enabled:
- same final physiological/work outcome
unless approved unique comfort context.

## 220W.9 Old-save derivation gate

## 220W.10 Hysteresis gate

## 220W.11 Emergency priority gate

## 220W.12 Deterministic golden fixtures

Fixed observation set:
- exact tags/profile/summary.

## 220W.13 Performance benchmark

## 220W.14 Generated docs

Create:
- `SHELTER_ATMOSPHERE_ARCHITECTURE.md`;
- `SHELTER_ATMOSPHERE_AUTHORITY_MATRIX.md`;
- `ATMOSPHERE_SOURCE_ADAPTER_MATRIX.md`;
- `ATMOSPHERE_FACET_MATRIX.md`;
- `ATMOSPHERE_TAG_PROFILE_MATRIX.md`;
- `ATMOSPHERE_EFFECT_DOUBLE_COUNT_MATRIX.md`;
- `ATMOSPHERE_PRESENTATION_MATRIX.md`;
- `ATMOSPHERE_MIGRATION_MATRIX.md`;
- `ATMOSPHERE_BALANCE_REPORT.md`;
- `ADR_ATMOSPHERE_AS_DERIVED_READ_MODEL.md`;
- `ADR_ATMOSPHERE_EFFECTS_AND_DOUBLE_COUNTING.md`;
- `ADR_ATMOSPHERE_TAGS_VS_EXCLUSIVE_PROFILES.md`;
- `ADR_ATMOSPHERE_OLD_SAVE_DERIVATION.md`.

### 220W DoD

Every atmosphere label can be traced to canonical source observations and every gameplay consequence can prove it is not duplicating a source-system effect.

---

# TASK 220X — Narrative Events & Quest Hooks

# 220X.0 Goal

Use atmosphere as an emergent storytelling signal without incentivizing profile toggling.

## 220X.1 Plan 171 owns dynamic quest generation

## 220X.2 Expose semantic predicates

- atmosphere_tag_sustained;
- atmosphere_profile_sustained;
- comfort_band_changed;
- crisis_to_recovery_transition;
- homeliness_threshold;
- social_tension_threshold.

## 220X.3 Source quest ideas

- Interior Designer;
- Engineer;
- Peacemaker;
- Host;
- Optimizer;
- Caretaker;
- Transformer.

Re-evaluate.

## 220X.4 Interior Designer

Better:
- establish a genuinely welcoming, personalized shelter zone
if decoration system supports.

## 220X.5 Engineer

Do not reward “Industrial for 100 days” if it encourages bad living conditions.

Could:
- maintain functional shelter under high machine demand.

## 220X.6 Peacemaker

Conflict system should own peace.

Atmosphere can be a supporting condition.

## 220X.7 Host

Could:
- maintain welcoming atmosphere + visitor satisfaction.

## 220X.8 Optimizer

Reject if based on hidden atmosphere productivity modifier.

## 220X.9 Caretaker

Use semantic conditions:
- comfortable;
- clean;
- breathable;
- orderly.

## 220X.10 Transformer

Reject raw profile-switch count.

## 220X.11 Narrative events

- The Shift;
- The Profile;
- The Comfort;
- The Tension;
- The Serenity;
- The Chaos.

Use only meaningful sustained transitions.

### 220X DoD

Narrative content responds to sustained, meaningful environmental character rather than rewarding arbitrary atmosphere-score or profile toggling.

---

# TASK 220Y — Advanced Interior Design, Atmosphere Managers & External Services: Explicit Follow-On

# 220Y.0 Goal

Prevent the base read-model from expanding into unrelated economy/skills systems.

## 220Y.1 Interior designer role

Use:
- SkillProgression;
- Plan 195 roles;
- decoration/build system.

## 220Y.2 Atmosphere manager

Not needed if automation/presentation is passive.

## 220Y.3 Seasonal atmosphere

Can use:
- Plan 170 events;
- weather;
- decoration.

## 220Y.4 Famous atmosphere legacy

Plan 162/archive/Plan 207 reputation if outsiders experience shelter.

## 220Y.5 Atmosphere trading

Source proposes:
- trade atmosphere technology.

Reframe as:
- lighting fixtures;
- ventilation equipment;
- acoustic insulation;
- furnishings;
- design services
through existing trade/crafting systems.

## 220Y.6 External visitors

Plan 207 reputation may consume:
- visitor-observed shelter atmosphere.

Do not directly export global reputation.

## 220Y.7 Room-level atmosphere

Future expansion.

Requires:
- spatial room model;
- per-room sources.

## 220Y.8 Dynamic decorative NPC behavior

Presentation follow-on.

## 220Y.9 Music system

Presentation/audio follow-on.

## 220Y.10 Scent/odor model

Only if sanitation/air systems support.

## 220Y.11 Architectural style

Build/decor system.

### 220Y DoD

Advanced interior-design and external-service ideas remain integrations over existing roles, trade, reputation and presentation systems instead of turning atmosphere into a new economy or skill tree.

---

# 5. Core Atmosphere Lifecycle

```text
CANONICAL SOURCE CHANGE
        │
        ▼
AtmosphereObservationSet
        │
        ▼
Facet Projection
        │
        ├── comfort
        ├── order
        ├── social warmth
        ├── activity rhythm
        ├── sensory harshness
        ├── homeliness
        └── crisis
        │
        ▼
Tags / Summary / Profile
        │
        ▼
Presentation Context
        │
        ├── UI
        ├── audio
        ├── lighting
        ├── ambient animation
        └── narrative hook
```

---

# 6. Atmosphere vs Environment Contract

Atmosphere:
- interprets.

Environment systems:
- simulate.

---

# 7. Atmosphere vs Morale Contract

Atmosphere:
- may expose comfort context.

Psychology/Needs:
- owns morale/stress.

---

# 8. Atmosphere vs Productivity Contract

Atmosphere:
- describes environmental character.

Plan 137/work:
- owns productivity.

---

# 9. Atmosphere vs Health Contract

Atmosphere:
- can say stale/smoky/dirty.

Medical/environment source:
- owns health consequence.

---

# 10. Atmosphere vs Sleep Contract

Atmosphere:
- can describe noisy/bright/comfortable.

Sleep/Needs:
- owns sleep consequence.

---

# 11. Atmosphere vs Noise Contract

Plan 205:
- gameplay noise.

Atmosphere:
- acoustic presentation.

---

# 12. Atmosphere vs Sanitation Contract

Plan 201:
- sanitation truth/health.

Atmosphere:
- cleanliness character.

---

# 13. Atmosphere vs Power Contract

PowerGrid:
- availability.

Atmosphere:
- presentation from power state.

---

# 14. Atmosphere vs Fire Contract

Fire:
- hazard.

Atmosphere:
- smoke/crisis overlay.

---

# 15. Atmosphere vs Social Contract

Relations/Plan 202:
- social state.

Atmosphere:
- social-climate summary.

---

# 16. Atmosphere vs Identity Contract

Plan 166:
- intended identity.

Atmosphere:
- current lived character.

---

# 17. Profile Contract

Profile:
- descriptive pattern.

Not:
- gameplay loadout.

---

# 18. Overall Mood Contract

Overall mood:
- optional summary.

Not:
- canonical scalar.

---

# 19. Lighting Contract

Real light:
- source/presentation system.

Atmosphere:
- theme guidance.

---

# 20. Soundscape Contract

Noise simulation:
- Plan 205.

Ambient mix:
- presentation.

---

# 21. Decoration Contract

Physical decoration:
- build/furniture/item system.

Atmosphere:
- homeliness projection.

---

# 22. Activity Contract

DutyRoster:
- actual work.

Atmosphere:
- rhythm/busyness.

---

# 23. Crisis Contract

Plan 158/194:
- emergency.

Atmosphere:
- crisis presentation overlay.

---

# 24. Time Contract

Clock/weather:
- canonical.

Atmosphere:
- contextual presentation.

---

# 25. Trend Contract

Trend:
- smoothed descriptive change.

No source consequence delay.

---

# 26. Event Contract

Atmosphere events:
- milestone interpretation only.

No duplicate source event.

---

# 27. Persistence Contract

Current atmosphere:
- rebuildable.

Persist only:
- hysteresis/history/preferences.

---

# 28. Old-Save Contract

Derive from real current sources.

No fake 50 values.

---

# 29. Persistence Matrix

| Fact | Owner |
|---|---|
| temperature | ShelterThermalSystem |
| air quality | VentilationSystem |
| power | PowerGridSystem |
| smoke/fire | ShelterFireHazardSystem |
| noise | Plan 205 |
| sanitation | Plan 201 |
| duty activity | DutyRoster |
| routines | Plan 188 |
| relations | SurvivorRelations |
| conflict | Plan 202 |
| morale/stress | Needs/mental health |
| sleep | Needs/sleep |
| productivity | Plan 137/work |
| shelter identity | Plan 166 |
| decorations | build/furniture/item system |
| atmosphere observations | derived |
| facets/tags/profile | ShelterAtmosphere |
| presentation context | ShelterAtmosphere |
| emergency alerts | Plan 194 |
| emergency response | Plan 158 |

---

# 30. Old-Save Migration Matrix

```text
feature_activation_day = current_day
restore source systems
derive current observations
derive current tags/profile
initialize hysteresis
history = []
no gameplay side effects
```

---

# 31. Exactly-Once Transition Identity

Examples:

```text
atmos-tag:<tag>:<activation-sequence>
atmos-profile:<profile>:<sequence>
atmos-crisis-overlay:<incident-id>:<state>
atmos-history:<transition-id>
```

---

# 32. Failure Injection Matrix

## N220.1 Noise causes sleep penalty in Plan 205 and Atmosphere applies another
Expected: double-count gate fails.

## N220.2 Air quality stored independently in atmosphere save
Expected: authority gate fails.

## N220.3 Old save sets all components to 50 despite active blackout/fire
Expected: migration derivation gate fails.

## N220.4 One light switch flips entire shelter Industrial→Warm→Industrial repeatedly
Expected: hysteresis gate fails.

## N220.5 Industrial profile directly gives +15% productivity
Expected: work authority gate fails.

## N220.6 Sterile profile directly gives health bonus despite Sanitation already doing so
Expected: health authority gate fails.

## N220.7 Chaotic profile directly adds stress on top of Noise/Conflict penalties
Expected: stress double-count gate fails.

## N220.8 Atmosphere audio masks emergency alarm
Expected: emergency priority gate fails.

## N220.9 Social energy is raw mean relationship affinity
Expected: social-model semantics gate fails.

## N220.10 High activity is always positive
Expected: activity-semantics gate fails.

## N220.11 Fire event produces dozens of atmosphere events per tick
Expected: event-budget gate fails.

## N220.12 Headless atmosphere requires rendering layer
Expected: architecture gate fails.

---

# 33. Determinism Contract

Same:

```text
source observations
+ profile/tag rules
+ hysteresis state
+ campaign time
```

must produce same:
- facets;
- tags;
- summary;
- profile;
- presentation context.

Simulation RNG should be unnecessary.

---

# 34. Long-Horizon Metrics

Track:

```text
source invalidations
facet recalculations
tag activations
tag deactivations
profile changes
crisis overlays
hysteresis-suppressed flips
atmosphere alerts
presentation-context updates
duplicate-effect detections
history entries
state bytes
daily processing time
```

---

# 35. Balance Guardrails

Atmosphere should:
- increase immersion;
- improve legibility;
- give shelter character.

It should not:
- become another mandatory optimization layer.

---

# 36. Comfort Guardrails

Comfort may matter.

But:
- environmental hazards already matter more.

---

# 37. Profile Guardrails

Profiles should feel:
- descriptive;
- stable;
- emergent.

Not:
- gear-set bonuses.

---

# 38. Social Guardrails

A tense shelter:
- should be tense because real social state is bad.

Atmosphere should not create the tension.

---

# 39. Crisis Guardrails

Emergency:
- immediately overrides normal ambience.

---

# 40. Decoration Guardrails

Decoration can improve homeliness.

It should not erase:
- smoke;
- disease;
- freezing temperatures.

---

# 41. Lighting Guardrails

Lighting presentation should:
- reflect power state;
- respect sleep/rest;
- preserve hazard visibility.

---

# 42. Audio Guardrails

Ambient audio:
- never competes with critical alerts.

---

# 43. UI Acceptance

## Summary
- current atmosphere;
- dominant tags;
- trend.

## Causes
- top contributors.

## Actions
- deep links to source systems.

## Effects
- only unique, bounded comfort effect if approved.

---

# 44. Accessibility

- no color-only mood meaning;
- text labels;
- keyboard/controller;
- readable cause hierarchy;
- screen-reader;
- critical alerts distinct from ambiance.

---

# 45. Localization

Profile/tag descriptions:
- localization keys.

No localized strings persisted.

---

# 46. Content Acceptance

Atmosphere profile ladder:

```text
DISCOVERED
LOADED
REGISTERED
SOURCE_RULES_BOUND
OBSERVATION_FIXTURE_REACHED
TAG_MATCHED
PRESENTATION_CONTEXT_EMITTED
UI/PRESENTATION_CONSUMER_OBSERVED
```

---

# 47. Reachability

Every tag/profile:
- at least one reachable source-state fixture.

No decorative count padding.

---

# 48. Performance Guardrails

- source-revision driven;
- no frame loop;
- compact aggregates;
- bounded history;
- deterministic cache.

---

# 49. CI / Gate Set

Recommended:

```text
shelter_atmosphere_authority_matrix
shelter_atmosphere_derived_read_model
shelter_atmosphere_no_duplicate_thermal
shelter_atmosphere_no_duplicate_air
shelter_atmosphere_no_duplicate_noise
shelter_atmosphere_no_duplicate_sanitation
shelter_atmosphere_no_duplicate_health
shelter_atmosphere_no_duplicate_sleep
shelter_atmosphere_no_duplicate_stress
shelter_atmosphere_no_duplicate_productivity
shelter_atmosphere_hysteresis
shelter_atmosphere_emergency_priority
shelter_atmosphere_old_save_derivation
shelter_atmosphere_determinism
shelter_atmosphere_long_horizon
shelter_atmosphere_performance
shelter_atmosphere_ui_access
```

---

# 50. Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --shelter-atmosphere-selftest
godot --headless --path . -- --real-campaign-journey-selftest
bash scripts/ci/content-acceptance-gate.sh
bash scripts/ci/verify-fast.sh
```

Use exact current command names if renamed.

---

# 51. Recommended Commit Breakdown

```text
220A-1 source/consumer authority audit
220A-2 derived-read-model ADR
220A-3 effect/double-count ADR
220A-4 tag/profile ADR
220A-5 old-save derivation ADR
220A-6 observation DTOs
220A-7 source revision/cache contract
220A-8 docs/tests

220B-1 facet schema
220B-2 thermal/air/noise mappings
220B-3 lighting/cleanliness mappings
220B-4 activity/social mappings
220B-5 decoration/crisis mappings
220B-6 tag rules
220B-7 hysteresis
220B-8 matrix/tests

220C-1 overall summary semantics
220C-2 mood bands
220C-3 trend smoothing
220C-4 no-master-stat gate
220C-5 bounded history
220C-6 UI derivation tests

220D-1 profile data schema
220D-2 industrial pattern
220D-3 sterile pattern
220D-4 lived-in pattern
220D-5 serene/chaotic patterns
220D-6 ambiguous warm/cold refactor
220D-7 multi-profile support
220D-8 no-buff-package gate

220E-1 duplicate-effect audit
220E-2 morale boundary
220E-3 productivity boundary
220E-4 health boundary
220E-5 stress boundary
220E-6 sleep boundary
220E-7 optional comfort context
220E-8 regression tests/docs

220F-1 lighting presentation context
220F-2 power-state binding
220F-3 day/night binding
220F-4 emergency override
220F-5 hazard visibility
220F-6 performance/accessibility tests

220G-1 ambient soundscape context
220G-2 Plan-205 binding
220G-3 social/activity layers
220G-4 crisis override
220G-5 deterministic variation
220G-6 accessibility/audio-priority tests

220H-1 Plan-201 sanitation adapter
220H-2 cleanliness/order distinction
220H-3 visual cleanliness hooks
220H-4 no-health-duplicate gate
220H-5 tests/docs

220I-1 DutyRoster adapter
220I-2 Plan-188 routine adapter
220I-3 activity-rhythm bands
220I-4 temporal weighting
220I-5 no-productivity semantics
220I-6 tests/docs

220J-1 Relations aggregate adapter
220J-2 Plan-202 conflict adapter
220J-3 grief/celebration source hooks
220J-4 social warmth semantics
220J-5 no-feedback-loop gate
220J-6 tests/docs

220K-1 decoration source audit
220K-2 Plan-210 personal-effects hooks
220K-3 memorial hooks
220K-4 homeliness facet
220K-5 diminishing duplicate decor
220K-6 room/global decision
220K-7 tests/docs

220L-1 Plan-166 identity adapter
220L-2 identity-vs-atmosphere UI
220L-3 contradiction support
220L-4 no-name-effect gate
220L-5 tests/docs

220M-1 Plan-158 crisis adapter
220M-2 Plan-194 alert adapter
220M-3 fire/smoke overlay
220M-4 blackout/medical/lockdown overlays
220M-5 recovery-state presentation
220M-6 emergency-priority tests

220N-1 time/day-night adapter
220N-2 season/weather adapter
220N-3 nuclear-winter presentation
220N-4 seasonal decoration hook
220N-5 no-weather-duplicate gate
220N-6 tests/docs

220O-1 EMA/dwell-time implementation
220O-2 entry/exit thresholds
220O-3 emergency bypass
220O-4 profile dwell
220O-5 UI trend smoothing
220O-6 event-budget tests

220P-1 semantic atmosphere events
220P-2 history provenance
220P-3 source-event dedupe
220P-4 bounded history
220P-5 Plan-171 hook surface
220P-6 tests/docs

220Q-1 atmosphere panel
220Q-2 cause trace
220Q-3 source-system deep links
220Q-4 trend graph
220Q-5 alert budget
220Q-6 accessibility/localization
220Q-7 tutorial/snapshots

220R-1 presentation-context DTO
220R-2 lighting theme
220R-3 soundscape theme
220R-4 ambient NPC animation theme
220R-5 emergency overlay
220R-6 deterministic presentation variants
220R-7 no-simulation-write gate

220S-1 persistence schema
220S-2 old-save source derivation
220S-3 missing-source handling
220S-4 restore ordering
220S-5 silent initial projection
220S-6 no-side-effect replay
220S-7 history migration
220S-8 docs/tests

220T-1 deterministic projection
220T-2 hysteresis anti-toggle
220T-3 sustained-duration milestone logic
220T-4 quest anti-farm
220T-5 stable transition IDs
220T-6 no-RNG/no-wall-clock gates

220U-1 source-revision invalidation
220U-2 partial facet recompute
220U-3 aggregate source APIs
220U-4 1/10/100-change benchmark
220U-5 history/state-size gate
220U-6 presentation profiling

220V-1 30-day neutral
220V-2 30-day industrial
220V-3 30-day serene
220V-4 30-day chaotic
220V-5 120-day evolution
220V-6 180-day power/noise/sanitation/thermal
220V-7 180-day social tension
220V-8 400-day mature shelter
220V-9 crisis/old-save/presentation-only tests
220V-10 balance report

220W-1 selftest
220W-2 source-scan authority gates
220W-3 double-count regressions
220W-4 content acceptance
220W-5 failure fixtures
220W-6 deterministic goldens
220W-7 performance/accessibility
220W-8 final ship/no-ship report

220X-1 narrative atmosphere hooks
220X-2 Plan-171 integration
220X-3 quest anti-toggle review
220X-4 milestone policy

220Y-1 advanced interior-design/service/room-level follow-on disposition
```

---

# 52. Risk Register

## R220.1 Atmosphere double-counts source penalties

Mitigation:
- default presentation-only;
- explicit effect matrix.

## R220.2 Composite score hides nuance

Mitigation:
- typed facets/tags;
- overall mood presentation-only.

## R220.3 Profiles become min-max buff presets

Mitigation:
- profiles descriptive, no direct modifiers.

## R220.4 Source mirroring creates stale state

Mitigation:
- derived observations;
- no duplicated source values.

## R220.5 Frequent source changes cause profile thrash

Mitigation:
- hysteresis;
- dwell time;
- emergency bypass.

## R220.6 Old saves start fake-neutral despite bad conditions

Mitigation:
- derive from restored source state.

## R220.7 Ambiance hides real hazards

Mitigation:
- emergency visual/audio priority.

## R220.8 Social-energy metric oversimplifies relationships

Mitigation:
- event/conflict/relations aggregate.

## R220.9 Decoration becomes mandatory stat grind

Mitigation:
- bounded homeliness;
- diminishing returns.

## R220.10 Presentation becomes expensive

Mitigation:
- event-driven context;
- cached themes.

---

# 53. Acceptance Checklist

## P0

- [ ] ShelterThermalSystem audited
- [ ] VentilationSystem audited
- [ ] PowerGridSystem audited
- [ ] lighting presentation audited
- [ ] ShelterFireHazardSystem audited
- [ ] Plan 205 noise audited
- [ ] Plan 201 sanitation audited
- [ ] DutyRoster audited
- [ ] Plan 188 routines audited
- [ ] SurvivorRelations audited
- [ ] Plan 202 conflict audited
- [ ] NeedsSystem audited
- [ ] mental-health/stress audited
- [ ] sleep system audited
- [ ] Plan 137 audited
- [ ] Plan 143 audited
- [ ] Plan 158 audited
- [ ] Plan 194 audited
- [ ] Plan 166 audited
- [ ] decoration/furniture audited
- [ ] day/night audited
- [ ] weather/season audited
- [ ] ambient audio audited
- [ ] visual presentation audited
- [ ] semantic event bus audited
- [ ] save order audited
- [ ] shelter UI audited
- [ ] authority matrix published
- [ ] derived-read-model ADR
- [ ] effect/double-count ADR
- [ ] tag/profile ADR
- [ ] old-save ADR
- [ ] baseline visual cases captured

## 220A — Observations

- [ ] AtmosphereObservationSet
- [ ] day/shelter refs
- [ ] thermal observation
- [ ] air observation
- [ ] acoustic observation
- [ ] lighting observation
- [ ] cleanliness observation
- [ ] activity observation
- [ ] social observation
- [ ] decoration observation
- [ ] crisis observation
- [ ] temporal observation
- [ ] source revisions
- [ ] no full source-state duplication
- [ ] thermal semantic bands
- [ ] occupied-area weighting if supported
- [ ] air smoke/staleness semantics
- [ ] noise continuity/night semantics
- [ ] lighting availability vs mood separated
- [ ] cleanliness semantic bands
- [ ] activity rhythm
- [ ] social multi-flag state
- [ ] decoration categories
- [ ] crisis severity
- [ ] no RNG

## 220B — Facets/Tags

- [ ] comfort facet
- [ ] order facet
- [ ] social warmth facet
- [ ] activity rhythm facet
- [ ] sensory harshness facet
- [ ] homeliness facet
- [ ] safety-feel facet
- [ ] no simple good/bad mapping
- [ ] typed tags
- [ ] Industrial
- [ ] Sterile
- [ ] Lived-In
- [ ] Serene
- [ ] Chaotic
- [ ] Tense
- [ ] Welcoming
- [ ] Oppressive if supported
- [ ] Smoky
- [ ] Dim
- [ ] Bustling
- [ ] Quiet
- [ ] Grieving
- [ ] Celebratory
- [ ] multiple tags
- [ ] deterministic rules
- [ ] hysteresis
- [ ] contradiction/coexistence rules
- [ ] profile data contains presentation rules only

## 220C — Overall Summary

- [ ] overall score presentation-only
- [ ] semantic category
- [ ] bleak
- [ ] tense
- [ ] neutral
- [ ] comfortable
- [ ] welcoming
- [ ] vibrant
- [ ] vibrant not universally best
- [ ] pattern-based classification
- [ ] no monotonic good/bad truth
- [ ] trend smoothing
- [ ] no trend flip spam
- [ ] no unnecessary persistence
- [ ] bounded history

## 220D — Profiles

- [ ] Industrial semantic pattern
- [ ] Sterile semantic pattern
- [ ] Lived-In semantic pattern
- [ ] Warm ambiguity resolved
- [ ] Cold ambiguity resolved
- [ ] Chaotic semantic pattern
- [ ] Serene semantic pattern
- [ ] localization-only name
- [ ] no global profile modifiers
- [ ] presentation package
- [ ] primary + secondary tags
- [ ] sustained change threshold
- [ ] one-light profile flip impossible

## 220E — Effect Boundary

- [ ] default presentation-only
- [ ] morale duplicate audit
- [ ] productivity duplicate audit
- [ ] health duplicate audit
- [ ] stress duplicate audit
- [ ] sleep duplicate audit
- [ ] unique comfort concept identified or rejected
- [ ] aesthetic/homeliness source only
- [ ] bounded magnitude
- [ ] no linear source sum
- [ ] consumer owns final effect
- [ ] no persistent AtmosphereEffect consequence state
- [ ] provenance diagnostics
- [ ] acceptable to ship with zero gameplay modifier

## 220F — Lighting

- [ ] power remains authority
- [ ] real fixture state remains authority
- [ ] color-temperature guidance
- [ ] ambient intensity accents
- [ ] emergency palette override
- [ ] unstable-power flicker only from source
- [ ] blackout respected
- [ ] hazard visibility preserved
- [ ] day/night canonical
- [ ] rest-hour presentation safe
- [ ] Plan 194 precedence
- [ ] fire/smoke visibility
- [ ] accessibility
- [ ] event-based updates

## 220G — Audio

- [ ] Plan 205 gameplay noise preserved
- [ ] presentation mix separate
- [ ] machinery hum
- [ ] ventilation
- [ ] chatter
- [ ] busy work rhythm
- [ ] tense silence
- [ ] storm layer
- [ ] emergency alarm override
- [ ] no gameplay acoustic penalty
- [ ] noise source canonical
- [ ] social chatter context
- [ ] deterministic variation
- [ ] no clip persistence
- [ ] alerts distinct

## 220H — Cleanliness

- [ ] Plan 201 authority
- [ ] semantic cleanliness bands
- [ ] no health duplicate
- [ ] visual grime/clutter if supported
- [ ] Sterile requires pattern
- [ ] Lived-In != dirty
- [ ] order vs sanitation separated

## 220I — Activity

- [ ] DutyRoster owner
- [ ] Plan 188 owner
- [ ] awake active observation
- [ ] work observation
- [ ] leisure observation
- [ ] emergency observation
- [ ] dormant
- [ ] quiet
- [ ] steady
- [ ] bustling
- [ ] overloaded
- [ ] emergency
- [ ] high activity not automatically positive
- [ ] low activity context-aware
- [ ] no productivity derivation
- [ ] nighttime weighting
- [ ] shift-change presentation
- [ ] no activity reward

## 220J — Social

- [ ] Relations authority preserved
- [ ] Plan 202 authority preserved
- [ ] mental health/grief preserved
- [ ] communal interaction aggregate
- [ ] severe conflict aggregate
- [ ] isolation
- [ ] grief
- [ ] celebration
- [ ] no mean-affinity shortcut
- [ ] social warmth derived
- [ ] tension provenance
- [ ] grieving source-backed
- [ ] celebratory source-backed
- [ ] no direct morale mutation
- [ ] no feedback loop

## 220K — Decoration/Homeliness

- [ ] decoration authority audited
- [ ] bare
- [ ] functional
- [ ] personalized
- [ ] decorated
- [ ] commemorative
- [ ] cluttered if real
- [ ] no decoration inventory duplicate
- [ ] Plan 210 personal-effects integration
- [ ] displayed vs stored distinction
- [ ] memorial semantics
- [ ] art/decor comfort only if non-duplicative
- [ ] diminishing duplicates
- [ ] room/global decision
- [ ] no decoration grind

## 220L — Identity

- [ ] Plan 166 remains identity owner
- [ ] atmosphere remains current lived feel
- [ ] identity may guide presentation only
- [ ] mismatch supported
- [ ] identity + atmosphere UI
- [ ] name does not change atmosphere automatically

## 220M — Crisis

- [ ] Plan 158 response owner
- [ ] Plan 194 alert owner
- [ ] fire owner
- [ ] emergency tags
- [ ] smoky
- [ ] blackout
- [ ] evacuation
- [ ] medical crisis
- [ ] lockdown if real
- [ ] crisis dominance
- [ ] no emergency timer duplicate
- [ ] no panic duplicate
- [ ] no productivity duplicate
- [ ] recovery presentation
- [ ] alert precedence

## 220N — Time/Season

- [ ] campaign clock owner
- [ ] weather owner
- [ ] season owner
- [ ] night quiet presentation
- [ ] winter dimness
- [ ] storm acoustics
- [ ] summer ventilation context
- [ ] nuclear-winter presentation
- [ ] no weather gameplay modifier
- [ ] exterior influence only when transmitted
- [ ] seasonal decor if real
- [ ] no wall clock

## 220O — Hysteresis

- [ ] noisy sources acknowledged
- [ ] smoothing strategy
- [ ] sustained threshold
- [ ] minimum dwell
- [ ] emergency bypass
- [ ] separate entry/exit
- [ ] profile dwell
- [ ] weekly UI trend
- [ ] audio crossfade
- [ ] no gameplay consequence delay
- [ ] meaningful transition events only
- [ ] no +1 event spam

## 220P — Events

- [ ] tag activation event
- [ ] tag deactivation event
- [ ] profile change event
- [ ] comfort band change
- [ ] crisis overlay change
- [ ] source narrative names treated as content
- [ ] no source event duplication
- [ ] sustained milestone
- [ ] history source refs
- [ ] no localized state text
- [ ] history bounded

## 220Q — UI

- [ ] descriptive summary
- [ ] dominant tags
- [ ] trend
- [ ] top positive contributors
- [ ] top negative contributors
- [ ] semantic component detail
- [ ] no unnecessary 0–100 mirrors
- [ ] primary/secondary profiles
- [ ] unique effects only
- [ ] thermal deep link
- [ ] ventilation deep link
- [ ] sanitation deep link
- [ ] power deep link
- [ ] noise deep link
- [ ] social/conflict deep link
- [ ] decoration deep link
- [ ] smoothed graph
- [ ] high-value alerts
- [ ] no duplicate hazard alert
- [ ] tutorial
- [ ] no hover-only
- [ ] no color-only
- [ ] keyboard/controller
- [ ] text scale
- [ ] screen-reader
- [ ] debug raw facet scores optional

## 220R — Presentation Contract

- [ ] immutable presentation DTO
- [ ] primary profile
- [ ] tags
- [ ] lighting theme
- [ ] soundscape theme
- [ ] ambient activity theme
- [ ] clutter theme
- [ ] urgency overlay
- [ ] transition duration
- [ ] provenance refs
- [ ] no simulation mutation
- [ ] decorative NPC behavior does not alter duties
- [ ] emergency overlay canonical
- [ ] deterministic variation
- [ ] rebuildable after load

## 220S — Persistence

- [ ] schema version
- [ ] activation day
- [ ] profile hysteresis
- [ ] tag hysteresis
- [ ] last significant transition
- [ ] bounded history
- [ ] UI prefs optional
- [ ] processed transitions
- [ ] no temperature duplicate
- [ ] no air duplicate
- [ ] no power duplicate
- [ ] no fire duplicate
- [ ] no noise duplicate
- [ ] no sanitation duplicate
- [ ] no duty duplicate
- [ ] no relation duplicate
- [ ] no Needs duplicate
- [ ] no sleep duplicate
- [ ] no health duplicate
- [ ] no productivity duplicate
- [ ] no decoration inventory duplicate
- [ ] current atmosphere recomputed
- [ ] old save not 50-all
- [ ] source systems restore first
- [ ] hysteresis initialized silently
- [ ] no history fabrication
- [ ] missing source unavailable
- [ ] restore no transition spam
- [ ] no side-effect replay

## 220T — Determinism/Exploit

- [ ] deterministic projection
- [ ] no simulation RNG
- [ ] presentation RNG stable if used
- [ ] profile hysteresis
- [ ] lamp-toggle farm blocked
- [ ] sustained duration committed
- [ ] Transformer count quest rejected/reworked
- [ ] Optimizer buff quest rejected/reworked
- [ ] semantic quality conditions instead of fake 70-score
- [ ] reload no profile reroll
- [ ] events exactly once
- [ ] no GUID
- [ ] no wall clock

## 220U — Performance

- [ ] event-driven invalidation
- [ ] daily fallback only
- [ ] no per-frame Core recompute
- [ ] source revision partial recompute
- [ ] presentation update only on meaningful change
- [ ] bounded history
- [ ] room-level deferred
- [ ] no item scan
- [ ] no relation-pair scan
- [ ] source aggregate APIs
- [ ] 1-change benchmark
- [ ] 10-change benchmark
- [ ] 100-change benchmark
- [ ] presentation crossfade profiled

## 220V/W — Simulation/CI

- [ ] 30-day neutral
- [ ] 30-day industrial
- [ ] 30-day serene
- [ ] 30-day chaotic
- [ ] 120-day bare→lived-in→crisis→recovery
- [ ] 180-day power instability
- [ ] 180-day sanitation issue
- [ ] 180-day noise issue
- [ ] 180-day thermal extremes
- [ ] 180-day social conflict
- [ ] 400-day mature shelter
- [ ] fire emergency
- [ ] alert priority
- [ ] old save
- [ ] missing source
- [ ] presentation-only parity
- [ ] optional comfort-context bounded
- [ ] data integrity
- [ ] shelter-atmosphere selftest
- [ ] source-scan authority gate
- [ ] content acceptance
- [ ] dead-profile gate
- [ ] dead-tag gate
- [ ] double-count regression
- [ ] old-save derivation
- [ ] hysteresis
- [ ] emergency priority
- [ ] deterministic goldens
- [ ] performance
- [ ] generated docs
- [ ] verify-fast

## 220X/Y — Narrative/Follow-On

- [ ] Plan 171 owns dynamic quests
- [ ] sustained tag predicates exposed
- [ ] Interior Designer goal contextualized
- [ ] Engineer goal not profile-grind
- [ ] Peacemaker uses conflict authority
- [ ] Host integrates visitors
- [ ] Optimizer raw buff quest rejected
- [ ] Caretaker uses semantic conditions
- [ ] Transformer toggle quest rejected
- [ ] narrative events sustained only
- [ ] interior designer uses canonical skill/role
- [ ] no unnecessary atmosphere manager role
- [ ] seasonal atmosphere uses existing events/weather
- [ ] famous atmosphere uses archive/reputation
- [ ] “atmosphere trading” reframed as real goods/services
- [ ] visitor-observed atmosphere can feed Plan 207 only through observation
- [ ] room-level atmosphere deferred
- [ ] dynamic NPC decoration behavior presentation-only
- [ ] music system presentation follow-on
- [ ] scent model only if source systems exist
- [ ] architectural style owned by build/decor

---

# 54. Ship / No-Ship Gate

**SHIP** only if:

```text
thermal_authorities == 1
AND air_authorities == 1
AND power_authorities == 1
AND fire_smoke_authorities == 1
AND noise_authorities == 1
AND sanitation_authorities == 1
AND social_state_authorities == 1
AND needs_stress_authorities == 1
AND sleep_authorities == 1
AND work_performance_authorities == 1
AND shelter_identity_authorities == 1
AND atmosphere_persisted_duplicate_thermal_state == 0
AND atmosphere_persisted_duplicate_air_state == 0
AND atmosphere_persisted_duplicate_noise_state == 0
AND atmosphere_persisted_duplicate_sanitation_state == 0
AND atmosphere_direct_health_mutation == false
AND atmosphere_direct_sleep_mutation == false
AND atmosphere_direct_stress_mutation == false
AND atmosphere_direct_morale_mutation == false
AND atmosphere_direct_productivity_mutation == false
AND atmosphere_profile_global_buff_packages == 0
AND atmosphere_overall_mood_used_as_master_gameplay_stat == false
AND old_save_fake_50_component_initialization == false
AND atmosphere_source_effect_double_counts == 0
AND atmosphere_profile_threshold_thrashing == 0
AND atmosphere_emergency_alert_suppression == false
AND atmosphere_social_energy_equals_mean_affinity == false
AND atmosphere_high_activity_always_positive == false
AND atmosphere_per_frame_core_processing == 0
AND atmosphere_unseeded_simulation_rng == 0
AND dead_atmosphere_profiles == 0
AND dead_atmosphere_tags == 0
AND shelter_atmosphere_old_save_derivation == pass
AND shelter_atmosphere_save_roundtrip == pass
AND shelter_atmosphere_source_authority == pass
AND shelter_atmosphere_double_count_regression == pass
AND shelter_atmosphere_hysteresis == pass
AND shelter_atmosphere_emergency_priority == pass
AND shelter_atmosphere_determinism == pass
AND shelter_atmosphere_30_day_balance == pass
AND shelter_atmosphere_120_day_balance == pass
AND shelter_atmosphere_180_day_balance == pass
AND shelter_atmosphere_400_day_soak == pass
AND shelter_atmosphere_performance == pass
AND shelter_atmosphere_selftest == pass
AND data_integrity_selftest == pass
AND content_acceptance_gate == pass
AND ui_access == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 55. Implementer Handoff

1. Audit thermal, ventilation, power, fire, noise, sanitation, relations, Needs, sleep, work, emergency and identity authorities before creating atmosphere state.
2. Implement Atmosphere as a **derived interpretation layer**, not a second shelter-condition simulator.
3. Read canonical source observations; do not persist duplicate temperature, air, noise, cleanliness or power values.
4. Split atmosphere into typed facets and descriptive tags rather than one universal good/bad score.
5. Keep overall mood as UI summary only.
6. Treat Industrial, Sterile, Lived-In, Serene, Chaotic and related profiles as descriptive patterns, not buff packages.
7. Remove direct profile morale/productivity/health modifiers unless an explicit authority audit proves a unique non-duplicative effect.
8. Default to presentation-only behavior if no unique gameplay effect remains.
9. If an environmental-comfort input is retained, keep it small, bounded, and focused on aesthetic/homeliness factors not already modeled.
10. Let thermal, air, sanitation, fire and noise systems continue to own their physiological consequences.
11. Let Plan 137/work systems continue to own productivity.
12. Let Needs/mental-health/sleep continue to own morale, stress and sleep quality.
13. Model activity as rhythm/busyness, not inherent productivity.
14. Model social climate from aggregate social events/conflict/relations, not raw average affinity.
15. Let Plan 210 personal effects and real decoration contribute homeliness only when physically present/displayed.
16. Keep Plan 166 shelter identity distinct from current atmosphere so aspirational identity can conflict with lived reality.
17. Let Plan 158/194 emergency state override normal atmosphere presentation instantly.
18. Build stable lighting/audio/presentation contexts that never hide fire, smoke, alert, or accessibility information.
19. Add hysteresis and minimum dwell time so normal source fluctuations do not thrash profiles.
20. Persist only hysteresis/history/preferences and rebuild current atmosphere from canonical sources after load.
21. Migrate old saves by deriving current atmosphere from restored real conditions, never by setting every component to 50.
22. Suppress initial migration transition events.
23. Emit atmosphere semantic events only for meaningful sustained profile/tag changes.
24. Reject quests that reward profile toggling, arbitrary score thresholds, or hidden productivity buffs.
25. Build UI around “what it feels like” and “why,” with deep links to the actual systems the player must fix.
26. Maintain emergency audio/visual priority above all ambiance.
27. Run presentation-only parity tests proving source-system gameplay outcomes are unchanged.
28. Run explicit duplicate-effect tests for noise, thermal, sanitation, air, sleep, stress, health and work.
29. Run 30/120/180/400-day shelter evolution simulations to validate profile stability, history bounds and no effect stacking.
30. Close only when the shelter gains a coherent, changing character that players can perceive immediately without introducing another hidden layer of duplicated survival modifiers.

---

# 56. Final Outcome

When this plan is complete, ASHFALL’s shelter will stop feeling like a collection of independent meters.

A player will not merely know that temperature is acceptable, ventilation is functional, the generator is loud and the floors are clean.

They will feel that the shelter is industrial, dim, busy and lived-in.

Or sterile, quiet and controlled.

Or warm, crowded and welcoming.

Or smoky, chaotic and tense after an emergency.

That character will emerge from the systems already running the shelter.

A blackout changes the lighting atmosphere because the power grid actually failed.
Stale air makes the shelter feel oppressive because ventilation actually degraded.
A loud generator contributes a mechanical, harsh acoustic profile because Plan 205 says the shelter is noisy.
A memorial wall and personal effects make a sleeping area feel lived-in because those objects actually exist.
A shelter filled with unresolved interpersonal conflict feels tense because Plan 202 and SurvivorRelations say the social climate is tense.

The atmosphere system does not invent those truths.

It interprets them.

That distinction prevents one of the biggest risks in a composite-systems feature: double penalties.

If noise already damages sleep, Atmosphere does not apply another sleep penalty because the profile became Chaotic.
If poor sanitation already increases disease risk, Sterile/Clean profiles do not add a second health model.
If thermal extremes already reduce capability, the atmosphere score does not multiply work efficiency again.

Instead, the new system gives all of those conditions one coherent sensory and descriptive language.

The UI can tell the player:

“Lived-In · Tense · Dim”

and explain why.

The ambient audio can shift from machinery and chatter to subdued silence after a death.

The visual presentation can become colder and harsher during a blackout without overriding the actual power state.

A fire can immediately push the shelter into a smoky emergency atmosphere, while Plan 194’s warning remains visually and acoustically dominant.

Once the fire is contained, the atmosphere can gradually shift into a recovering, damaged, subdued state without creating a second disaster system.

This also creates meaningful tension between identity and reality.

A shelter named and presented as a sanctuary can still feel overcrowded, tense and underlit if conditions deteriorate.

A brutal industrial bunker can become unexpectedly welcoming as survivors decorate it, form relationships and build routines.

A clinically clean shelter can feel sterile and socially distant.

A visibly worn shelter can be deeply comfortable and personal.

That is shelter character.

Most importantly, the feature can deliver substantial player value even if its gameplay modifiers are nearly zero.

The presentation itself matters.

Better audio.
Better lighting.
Better descriptive feedback.
Better environmental storytelling.
Better explanation of how individual systems combine.

If a small unique “homeliness/comfort” effect survives the double-count audit, it can be applied carefully through the canonical psychology system.

But the plan does not depend on that effect to justify itself.

The success condition is simpler:

when the player opens the shelter view, they should immediately understand not only whether the bunker is functioning, but **what it currently feels like to live there**.

ASHFALL already has the systems that create that feeling.

Plan 220 makes them speak with one voice.
