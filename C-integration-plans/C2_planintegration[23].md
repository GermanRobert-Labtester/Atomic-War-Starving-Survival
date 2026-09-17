# C2 — Flagship Integration Plan [23]: Stateful Ambience, Mix Discipline, and a Deliberately Sparse Musical Arc

> **Deliverable:** `C2_planintegration[23].md`
> **Source scope:** Plan 52 — *The Sound of Scarcity: Ambience, Music, and Silence as State*
> **Wave:** Continuity Wave 8 — *The Presented Game*
> **Primary objective:** turn ambience into one state-driven audio authority that follows place, weather, season, power, occupancy, and exposure; establish mix discipline through ducking, priority, concurrency caps, settings, and measured loudness; then give the campaign a restrained musical shape that follows season, pressure, loss, and ending without smothering information or authored silence.
> **Required task order:** **52A → 52B → 52C**
> **Wave-8 interleave to preserve:** **50A → 51A → 52A → 51B → 50C → 52B → 51C → 52C → 53 → 54**
> **Dependencies:** Plan 17C silence gaps, Plan 20A/20C position/exposure/weather, Plan 23A/23B power state, Plan 31 semantic events, Plan 42 voice delivery, Plan 38A/38B seasons/calendar, Plan 41A grief/death, Plan 30A war tension, Plan 37C settings/accessibility, Plan 50A asset/provenance truth, Plan 51C visual weather/lighting ownership.
> **Scope discipline:** no new buses, no new weather/mechanic invented to justify audio, no per-frame ambience polling, no alert without visual/text parity, no music over gameplay-critical informational cues, no audio path that mutates simulation state, and no large music production batch before triggers/coverage are proven.

---

# 0. Executive Intent

ASHFALL’s audio infrastructure is already healthy at the catalog and routing layer:

- 70+ cues,
- 12 active buses,
- generated cue coverage,
- selftests resolving current paths,
- shelter loops driven by power/atmosphere state,
- radio/world voice assets,
- existing menu/gameplay/game-over music,
- user settings/recovery precedent.

Yet the actual game remains too quiet, too flat, and too muddy in the wrong places because state ownership is incomplete:

```text
surface ambience exists but is not started by gameplay
weather audio maps only part of the weather vocabulary
geiger exposure can start but lacks an explicit end event
no ducking policy exists
multiple alerts can stack without priority
silence-class weather has no authored acoustic treatment
music does not follow season/tension/loss
```

This plan does not solve that by “adding more audio.”

It solves it by making audio consume the same state authorities the rest of the game already trusts.

The intended architecture is:

```text
place / weather / season / power / occupancy / exposure
                     │
                     ▼
             Ambience State Resolver
                     │
                     ▼
           ShelterAudioController
                     │
            ┌────────┼────────┐
            ▼        ▼        ▼
         ambience  weather   device loops
            │        │        │
            └────────┼────────┘
                     ▼
              Mix Policy Layer
                     │
         ┌───────────┼────────────┐
         ▼           ▼            ▼
      ducking     priority      concurrency
         │           │            │
         └───────────┼────────────┘
                     ▼
              user bus settings
                     │
                     ▼
                  output
                     │
                     ▼
            sparse music state
```

The flagship player-facing outcome is:

> **The bunker sounds inhabited when it is inhabited, a storm sounds different from a blackout, the surface actually has a bed, geiger audio stops when exposure ends, alerts stay legible under pressure, and the game deliberately becomes quiet when silence is the authored state.**

---

# 1. Source Diagnosis

The source establishes several crucial facts:

- surface ambience is defined but never started by actual gameplay,
- no ducking/bus-volume control exists in audio code,
- only 14 of 22 weather kinds currently map to cues,
- geiger loop lacks a canonical exposure-end signal,
- shelter power-driven loops already demonstrate the correct state-driven model,
- music identity is minimal and not keyed to campaign arcs,
- survivor voice delivery will need explicit spoken/subtitled policy,
- user audio-settings recovery is still largely manual,
- the fiction already contains `Silence`, `SilentSpring`, and `FalseSpring`.

The architectural reading is:

```text
audio does not need a new event universe;
it needs to consume the existing one correctly.
```

---

# 2. Program-Level Success Criteria

C2[23] closes only when all of the following are true.

## 2.1 One ambience owner

A single controller owns:

- bunker ambience,
- surface ambience,
- weather overlays,
- season variation,
- occupancy layers,
- exposure loops,
- power-device audibility.

## 2.2 Ambience is state-driven

No one-off gameplay call is the long-term authority for the ambient state.

## 2.3 All weather kinds have an authored audio treatment

Treatment may be:

- cue family,
- parameter variation,
- or intentional silence.

## 2.4 Surface ambience starts and stops correctly

Position transitions are tested through gameplay state.

## 2.5 Geiger loop has an explicit stop path

Exposure start/end are both represented.

## 2.6 Silence is explicit

Silence-class states drop configured beds rather than simply “having no cue.”

## 2.7 Mix is prioritized

Alerts duck beds/SFX and obey caps/dedup rules.

## 2.8 Alert semantics are unique

Unrelated threats do not share indistinguishable cues.

## 2.9 User controls the mix

Bus/group sliders persist and recover safely.

## 2.10 Accessibility parity exists

Every gameplay-significant cue has visual/text equivalent.

## 2.11 Music follows state, not screens

Season, pressure, grief, and ending determine music.

## 2.12 Music remains sparse

No constant wallpaper loop.

## 2.13 Audio is determinism-neutral

Muted/unmuted execution yields identical simulation checksums.

## 2.14 Session replacement is clean

No orphan loops, duplicate players, or stale callbacks after New Game/Load.

## 2.15 Mix behavior is measurable

Seeded storm soak proves the pile-up policy holds.

---

# 3. Architectural Invariants

## 3.1 One ambience controller

Extend the existing `ShelterAudioController` or canonical equivalent.

Do not add a second competing ambience state machine.

## 3.2 Audio consumes state

Audio never owns:

- weather,
- exposure,
- power,
- occupancy,
- grief,
- war tension,
- season.

## 3.3 State changes drive transitions

No `_Process` polling for ambient facts.

## 3.4 Transitions are crossfades, not uncontrolled stacks

A state replacement should usually transition between beds.

## 3.5 Silence is a state

Silence-class weather and grief beats have an explicit mix policy.

## 3.6 Alerts are semantic classes

One player-learnable meaning per alert family.

## 3.7 Music is subordinate to information

Gameplay-critical alerts/VO retain clarity.

## 3.8 Audio failure does not break gameplay

Device loss/audio disable cannot block decisions.

## 3.9 Settings are presentation-only

Bus sliders, chaos reduction, and music preferences never alter simulation difficulty.

## 3.10 Cue/provenance identity comes from asset truth

Plan 50A remains the asset/provenance authority.

---

# 4. Dependency Graph

```text
20A position/exposure ─────────────┐
20C weather effects ───────────────┤
23A/23B power ─────────────────────┤
38A/38B season/calendar ───────────┤
occupancy/schedules ───────────────┤
                                  ▼
                          52A Ambience State
                                  │
                                  ▼
                          52B Mix Discipline
                                  │
                                  ▼
                          52C Music Shape

31A semantic events ─────────────► triggers
37C settings/accessibility ──────► bus controls / reduce chaos / parity
41A grief/death ─────────────────► silence/loss treatment
42B voice ───────────────────────► spoken/subtitled policy
30A war tension ─────────────────► music pressure
50A asset manifest ──────────────► cue provenance
51C weather visuals ─────────────► single weather-expression contract
```

Task order:

```text
52A → 52B → 52C
```

Wave order:

```text
50A → 51A → 52A → 51B → 50C → 52B → 51C → 52C → 53 → 54
```

---

# 5. Baseline Capture

Before changes record:

- cue count,
- bus count,
- current cue→bus mapping,
- current gameplay calls to ambience start/stop,
- weather kinds with audio mappings,
- weather kinds with no audio mapping,
- active loop start/stop paths,
- geiger loop start/stop coverage,
- duplicate source assets used by unrelated semantics,
- current per-bus volume capability,
- existing music cues,
- current settings schema,
- current audio selftest counts.

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --audio-selftest
bash scripts/ci/verify-fast.sh
```

Also capture current `SILENCE_AUDIT.md` and `AUDIO_QA_REPORT.md` status tables.

---

# 6. Workstream 52A — Ambience as a State Machine

## Goal

Make ambience follow place, weather, season, power, occupancy, and exposure through one event-driven state owner.

---

# 7. 52A Phase A — Draw the State Graph First

Create:

```text
docs/audio/AMBIENCE_STATE_MACHINE.md
```

Define base place states:

```text
bunker
surface
coast
transit
crowded_shelter
```

or actual live location categories.

Modifiers:

```text
weather
season
power
occupancy
exposure
```

For each transition define:

- source,
- destination,
- crossfade duration,
- preserved layers,
- stopped layers,
- priority.

---

# 8. 52A Phase B — Single Owner

Extend the existing shelter audio controller into the ambient authority.

Responsibilities:

```text
current acoustic state
desired acoustic state
layer start/stop
crossfade orchestration
session teardown
```

Do not let `Main.GameFlow` remain the long-term ambience policy owner.

---

# 9. 52A Phase C — Initial State Binding

At campaign/session setup:

```text
resolve current place
resolve weather
resolve season
resolve power
resolve occupancy
resolve exposure
→ calculate initial ambience
```

No “always bunker first” assumption if loading a state where another situation is valid.

---

# 10. 52A Phase D — Position-Driven Surface Ambience

Wire Plan 20A position/location model.

Transitions:

```text
indoors → surface
→ start/crossfade surface bed

surface → indoors
→ stop/crossfade surface bed
```

Test with actual gameplay state change.

---

# 11. 52A Phase E — Weather Family Mapping

Create one data/config table mapping all 22 weather kinds.

Use families such as:

```text
ash
acid
bio
blood
glass
hail
radiation
thermal
clear
storm
silence
spring
```

Do not create one new cue per weather kind if an existing bed + parameter variation expresses it.

---

# 12. 52A Phase F — Parameter Variation

Allowed modifiers:

- pitch,
- low-pass/high-pass,
- gain,
- reverb/space,
- layer intensity.

Keep bounded.

No effect setting should create clipping or runaway volume.

---

# 13. 52A Phase G — Silence Weather Family

For:

```text
Silence
SilentSpring
FalseSpring
```

define deliberate acoustic policy.

Potential rule:

```text
surface/weather beds → fade out
optional low mechanical interior remains only if fiction requires it
music → silence per 52C
```

Document exact exceptions.

“Nothing happened” is not the implementation.

---

# 14. 52A Phase H — Seasonal Surface Beds

When calendar/season authority is live, map season to variants.

Examples:

- winter wind pressure,
- thaw drip,
- summer dust.

Use existing buses.

No new bus.

---

# 15. 52A Phase I — Exposure-End Event

Add the missing Core signal/event for exposure end in the radiation authority.

Conceptually:

```text
ExposureStarted
ExposureBandChanged
ExposureEnded
```

Do not create an audio-specific radiation state.

---

# 16. 52A Phase J — Geiger Loop State

Geiger loop follows canonical dose-rate band.

Example:

```text
none → stopped
low → sparse
medium → active
high → intense
```

The exact implementation may use loop parameter/gain/pitch rather than distinct files.

At `ExposureEnded`:

```text
stop cleanly
```

---

# 17. 52A Phase K — Power-State Audibility

Existing generator/ventilation/air-filter loops remain canonical.

Extend to:

- load shedding,
- brownout,
- room silence,
- device cutout.

Do not create audio-only power state.

---

# 18. 52A Phase L — Occupancy Layer

Use schedules/roster/sleep assignment.

Example states:

```text
empty
light
normal
crowded
```

Map to density/intensity of shelter-life ambience.

Crossfade instead of stacking N survivor loops.

---

# 19. 52A Phase M — Event-Driven Updates

Subscribe to:

- position changes,
- weather changes,
- season changes,
- power changes,
- occupancy changes,
- exposure changes.

No per-frame scanning.

---

# 20. 52A Phase N — Transition Debounce

Rapid state changes should not restart/crossfade the same cue repeatedly.

Implement stable state comparison.

Example:

```text
desired state unchanged
→ no-op
```

---

# 21. 52A Phase O — Loop Ownership

Each loop has one owner instance.

Validate:

```text
start twice → still one active loop
stop → zero active loop
```

---

# 22. 52A Phase P — Session Replacement

On New Game/Load:

```text
unsubscribe old session
stop old loops
bind new authorities
resolve initial new state
```

No stale world/power/exposure event handlers.

---

# 23. 52A Phase Q — Shutdown

At app quit:

- fade/stop loops,
- detach subscriptions,
- no async hanging audio work.

---

# 24. 52A Phase R — Voice/Layer Budget

Define maximum simultaneous layers per bus.

Example categories:

- ambient base,
- weather overlay,
- machine loop,
- exposure loop,
- alerts.

Hard cap global and per bus.

---

# 25. 52A Phase S — Min-Spec Performance

Under worst reasonable state:

```text
storm
+ crowded shelter
+ generator
+ ventilation
+ radiation
```

measure:

- concurrent voices,
- CPU/audio cost,
- frame impact.

Use Plan 26C budget framework.

---

# 26. 52A Phase T — Determinism-Neutral Test

Run identical simulation:

```text
audio enabled
audio disabled/muted
```

Assert:

```text
same SaveChecksum
same campaign digest
```

Audio cannot consume simulation RNG or mutate game state.

---

# 27. 52A Phase U — Docs

`AMBIENCE_STATE_MACHINE.md` includes:

- graph,
- inputs,
- state resolution,
- crossfade rules,
- silence rule,
- voice caps,
- teardown lifecycle.

---

# 28. 52A Tests

- state resolver,
- indoor→surface,
- surface→indoor,
- all 22 weather kinds covered,
- silence class,
- season variation,
- power state,
- occupancy,
- exposure start/end,
- no double-start,
- session swap,
- shutdown,
- voice cap,
- checksum identity muted.

---

# 29. 52A Definition of Done

- [ ] state graph documented,
- [ ] one ambience owner,
- [ ] initial state binding,
- [ ] surface start/stop live,
- [ ] 22/22 weather treatment,
- [ ] silence explicit,
- [ ] seasonal variants,
- [ ] exposure-end event,
- [ ] geiger stop/bands,
- [ ] power audibility,
- [ ] occupancy,
- [ ] event-driven updates,
- [ ] no double starts,
- [ ] session-swap safety,
- [ ] voice/concurrency budget,
- [ ] min-spec budget,
- [ ] determinism-neutral proof.

---

# 30. Workstream 52B — Mix Discipline

## Goal

Make the audio mix legible under pressure through authored ducking, semantic priority, concurrency caps, user controls, and measured pile-up policy.

---

# 31. 52B Phase A — Mix Policy ADR

Create/update:

```text
docs/audio/MIX_POLICY.md
```

Define:

- bus hierarchy,
- alert classes,
- duck targets,
- duck amount/ramp,
- concurrency caps,
- dedup windows,
- silence-state policy,
- user override behavior.

---

# 32. 52B Phase B — Ducking

When high-priority alert plays:

```text
Alerts
→ duck Ambience/SFX by configured dB
→ attack ramp
→ hold
→ release ramp
```

Avoid instant hard cuts unless authored for silence state.

---

# 33. 52B Phase C — Ducking Ownership

One mix manager controls bus gain automation.

No cue-specific code manually changes other bus volumes.

---

# 34. 52B Phase D — Alert Priority

Define semantic priority.

Possible classes:

```text
critical danger
radiation
weather
medical
structure
status
```

Actual hierarchy must match game design.

---

# 35. 52B Phase E — Alert Concurrency Caps

For alert pile-up:

- max one critical foreground alert,
- lower-priority alerts queue/drop/defer according to policy,
- duplicate same cue within window collapses.

---

# 36. 52B Phase F — Same-Window Dedup

If the same alert fires repeatedly inside a short window:

```text
play once
record count if useful
```

Do not machine-gun.

---

# 37. 52B Phase G — Multi-Threat Collapse

For source scenario:

```text
storm
+ dose
+ klaxon
```

apply policy:

- loudest/most actionable foreground alert,
- ambient/weather remain ducked underneath,
- lower alert receives visual/text representation regardless.

---

# 38. 52B Phase H — Shared-Asset Semantic Collision Audit

Audit unrelated cue IDs sharing the same file.

Source examples include:

- contamination vs black rain,
- weather alert vs danger klaxon,
- pipe clang vs day transition.

Classify:

```text
intentional family
or
must become distinct
```

---

# 39. 52B Phase I — Produce/Assign Minimal Distinct Assets

Unlike 52A, 52B may require the limited source-approved de-dup batch.

Keep to only collision cases actually proven problematic.

No broad new production batch.

Record manifest/provenance via Plan 50A.

---

# 40. 52B Phase J — Loudness Baseline

For each bus/cue family record target:

```text
integrated/peak level
relative gain
intended foreground/background role
```

Use practical game-audio measurements available in the pipeline.

No opinion-only normalization.

---

# 41. 52B Phase K — Catalog Loudness QA

Update:

```text
docs/audio/AUDIO_QA_REPORT.md
```

with measured target/status per family.

Flag outliers.

---

# 42. 52B Phase L — User Bus Controls

Expose sliders for:

- Master,
- Music,
- Ambience,
- Alerts,
- Voice,
- SFX,

or all 12 buses if UX remains manageable.

Use grouped controls if 12 sliders are too granular.

---

# 43. 52B Phase M — Settings Persistence

Persist through existing `UserSettingsStore`.

Round-trip:

```text
set
→ save
→ restart
→ same bus levels
```

Defaults reproduce intended mix.

---

# 44. 52B Phase N — Automated Recovery Matrix

Convert manual recovery smoke into automated tests.

Cases:

- valid custom volumes,
- corrupted settings,
- missing settings,
- safe defaults,
- reset-to-default.

---

# 45. 52B Phase O — Semantic Alert Families

Every alert cue declares semantic class.

Validator rejects:

```text
two unrelated semantic classes
→ same exact file
```

unless explicit exception.

---

# 46. 52B Phase P — Visual/Text Parity

Every gameplay-significant alert has:

- text,
- icon/visual state,
- or both.

Audio is additive.

No deafness/accessibility soft lock.

---

# 47. 52B Phase Q — Reduce Audio Chaos

Add accessibility setting.

Effects may include:

- lower max simultaneous layers,
- stronger dedup,
- gentler alert stacking,
- optionally softer ducking/transitions.

Must not hide essential warnings.

Not a difficulty setting.

---

# 48. 52B Phase R — Silence Mix State

A dedicated silence state may be invoked by:

- silence weather,
- grief/memorial beats,
- game-over transition.

Rules define which buses remain:

- critical UI,
- required alerts,
- maybe voice/caption depending context.

---

# 49. 52B Phase S — Device Loss

Simulate audio-device disable/loss where feasible.

Expected:

- no exception spam,
- no broken state machine,
- visual warnings remain,
- re-enable recovers if platform permits.

---

# 50. 52B Phase T — Storm Soak

Seeded test logs:

```text
timestamp
active cues
bus
priority
duck state
dedup decision
peak/concurrency count
```

Assert policy held.

---

# 51. 52B Phase U — Pile-Up Budget

Define hard metrics:

```text
max foreground alerts
max total concurrent voices
max alert repetitions/window
max simultaneous full-volume layers
```

Use config/budget source.

---

# 52. 52B Phase V — Determinism-Neutral Mix Test

Ducking/settings/dedup decisions must not feed simulation.

Same state digest regardless of audio path.

---

# 53. 52B Phase W — SILENCE_AUDIT Closure

Update status column with:

```text
OPEN
PARTIAL
CLOSED
```

and evidence.

Do not leave stale gap claims.

---

# 54. 52B Tests

- duck engage,
- attack/release,
- priority,
- dedup,
- concurrency cap,
- semantic uniqueness,
- bus setting persistence,
- corrupted-setting recovery,
- reduce-chaos behavior,
- silence state,
- device loss,
- storm soak,
- checksum neutrality.

---

# 55. 52B Definition of Done

- [ ] mix policy,
- [ ] ducking,
- [ ] priority,
- [ ] dedup,
- [ ] bus caps,
- [ ] collision audit,
- [ ] limited distinct cue replacement,
- [ ] loudness pass,
- [ ] bus/group controls,
- [ ] settings persistence,
- [ ] automated recovery matrix,
- [ ] semantic-family validation,
- [ ] visual/text parity,
- [ ] reduce audio chaos,
- [ ] silence mix state,
- [ ] device-loss safety,
- [ ] measured storm soak,
- [ ] pile-up budgets,
- [ ] audit docs closed.

---

# 56. Workstream 52C — Score, Sting, and Session Shape

## Goal

Give ASHFALL a small musical identity that follows campaign state and uses silence intentionally.

---

# 57. 52C Phase A — Music Plan First

Create:

```text
docs/audio/MUSIC_PLAN.md
```

Define intended pieces and triggers before generating/commissioning.

Hard ceiling from source:

```text
3–5 additional pieces total
```

---

# 58. 52C Phase B — Music Role Vocabulary

Recommended roles:

```text
menu
shelter
surface_pressure
loss
ending
```

Existing gameplay cue may be reclassified.

No “music per panel.”

---

# 59. 52C Phase C — Music State Inputs

Potential inputs:

- war tension,
- active deadlines,
- storm pressure,
- brownout,
- dose/exposure pressure,
- season,
- grief,
- ending family.

All read existing authorities.

---

# 60. 52C Phase D — State, Not Screen

Opening inventory must not change track merely because screen changed.

Music changes only when underlying campaign state changes.

---

# 61. 52C Phase E — Pressure Bands

Define bounded pressure bands.

Example:

```text
low
medium
high
```

Derived from existing inputs.

Do not add a new hidden “music tension” gameplay variable that becomes another authority.

If an audio-only aggregate exists, it must be a pure projection.

---

# 62. 52C Phase F — Seasonal Variation

Prefer:

- instrumentation,
- filtering,
- transposition,
- arrangement variation

over unique track per season.

This creates temporal identity without content explosion.

---

# 63. 52C Phase G — Grief Through Subtraction

On survivor death/memorial:

- suppress active score,
- reduce beds according to 52B,
- optionally play one restrained held note/cue.

Do not use dramatic game-over sting for an individual death.

---

# 64. 52C Phase H — Game Over

Existing game-over cue remains distinct.

Coordinate with silence transition.

---

# 65. 52C Phase I — Ending Families

Map many ending permutations to a small tonal family set.

Source target:

```text
~3 outcome families
```

No bespoke piece per ending permutation.

---

# 66. 52C Phase J — Ending Mapping Data

Create data/config:

```text
ending family
→ music cue
```

Validated against ending IDs/families.

No giant switch.

---

# 67. 52C Phase K — Survivor Voice Policy

Decide:

```text
which lines are spoken audio
which remain subtitled/text only
```

Source scope implies bounded voice production.

All spoken content still has captions/subtitles.

---

# 68. 52C Phase L — One Playback Owner

Ambience/music mix ownership must coordinate.

No separate music manager fighting crossfades/ducking.

Audio manager may expose the playback implementation while ambience/music state resolver owns desired state.

---

# 69. 52C Phase M — Music Crossfade

Define:

- minimum dwell time,
- crossfade duration,
- re-entry cooldown,
- interruption rules.

Prevent track thrashing if tension oscillates.

---

# 70. 52C Phase N — Silence Priority

Declared silence state overrides music.

Examples:

- Silence weather,
- grief beat,
- specific ending/game-over transition phase.

Test explicitly.

---

# 71. 52C Phase O — Seamless Loop Verification

For looped music/beds:

- inspect sample boundaries,
- detect obvious discontinuity/click,
- document exceptions.

Automate simple sample-boundary checks where practical.

---

# 72. 52C Phase P — Loudness Continuity

Music target levels must coexist with 52B mix.

User increasing ambience must not unintentionally erase all score identity.

Test grouped settings combinations.

---

# 73. 52C Phase Q — Asset Manifest/Provenance

Every new/retained music asset:

- asset manifest row,
- provenance,
- license/AI declaration,
- import preset,
- loop status.

Plan 50A remains authority.

---

# 74. 52C Phase R — Production Batch Gate

Do not commission/generate 3–5 pieces until:

- trigger map approved,
- role set approved,
- current asset inventory checked,
- provenance workflow ready.

---

# 75. 52C Phase S — Content Acceptance

For each piece:

```text
role
trigger
duration
loop/non-loop
mix target
provenance
status
```

No unnamed loose audio files.

---

# 76. 52C Phase T — Deterministic Music Selection

Same campaign state:

```text
same desired music role/cue
```

Do not randomly rotate tracks unless a seeded deterministic selection contract is added.

---

# 77. 52C Phase U — Settings Recovery

Music volume/mute persists and recovers.

No state machine break when music bus volume is zero.

---

# 78. 52C Phase V — Session Shape Soak

Run a scripted campaign covering:

- menu,
- calm shelter,
- surface pressure,
- storm,
- high war tension,
- death/grief,
- ending.

Log desired/actual music states.

Assert:

- no screen-driven changes,
- no music during declared silence,
- no rapid thrash,
- correct ending family.

---

# 79. 52C Tests

- state→music mapping,
- season variant,
- pressure band,
- grief silence,
- game over,
- ending family,
- voice policy metadata,
- crossfade/re-entry,
- loop integrity,
- loudness/settings,
- provenance,
- determinism.

---

# 80. 52C Definition of Done

- [ ] MUSIC_PLAN,
- [ ] 3–5 piece ceiling,
- [ ] role vocabulary,
- [ ] state-driven triggers,
- [ ] pressure projection,
- [ ] seasonal variation,
- [ ] grief subtraction,
- [ ] game-over distinction,
- [ ] ending family mapping,
- [ ] voice spoken/subtitle policy,
- [ ] one playback owner,
- [ ] crossfade/re-entry rules,
- [ ] silence overrides music,
- [ ] loop integrity,
- [ ] loudness continuity,
- [ ] provenance,
- [ ] production batch gated by plan,
- [ ] deterministic music selection,
- [ ] session-shape soak.

---

# 81. Integrated Audio State Pipeline

```text
Simulation authorities
  │
  ├─ place
  ├─ weather
  ├─ season
  ├─ power
  ├─ occupancy
  ├─ exposure
  ├─ war tension
  ├─ grief
  └─ ending
  │
  ▼
Audio State Projection
  │
  ├─ ambience state
  ├─ alert priority
  ├─ silence state
  └─ music role
  │
  ▼
Audio Manager / Bus Mixer
  │
  ├─ crossfade
  ├─ duck
  ├─ dedup
  ├─ cap
  └─ user levels
  │
  ▼
Audio output
```

---

# 82. Audio State Contract

Audio state is a pure projection of authoritative simulation facts.

No audio state may become a gameplay truth source.

---

# 83. Place Contract

Position/location authority maps into one acoustic place family.

No UI screen decides ambience.

---

# 84. Weather Contract

All weather kinds map to:

```text
audio family
+ parameter set
+ silence rule if applicable
```

No unmapped weather.

---

# 85. Exposure Contract

Radiation authority emits a real end state.

Geiger loop start and stop both follow canonical exposure state.

---

# 86. Power Contract

Power loops use canonical power facts.

Load shedding is heard only because actual state changed.

---

# 87. Occupancy Contract

Occupancy is a projection from roster/schedule/location facts.

No audio-only headcount.

---

# 88. Alert Contract

Each alert answers:

```text
semantic class
priority
dedup window
duck amount
visual/text equivalent
cue id
```

---

# 89. Silence Contract

Silence is represented as an explicit desired mix state.

It may suppress:

- ambience,
- weather beds,
- music,

while preserving required alerts/UI cues.

---

# 90. User Settings Contract

User can control presentation mix.

Settings include:

- volume groups/buses,
- reduce-audio-chaos option,
- optional music mute.

These never change game difficulty.

---

# 91. Audio Failure Contract

If device/audio playback fails:

- simulation continues,
- visual/text warnings continue,
- diagnostics stay bounded,
- recovery does not duplicate loops.

---

# 92. Music Contract

Music is a low-bandwidth summary of campaign state.

It is not a screen soundtrack.

---

# 93. Voice Contract

Survivor/radio voice policy must coordinate with music ducking/captions.

Voice priority may duck music/ambience without suppressing critical alerts.

---

# 94. Provenance Contract

Audio assets use Plan 50A manifest/provenance.

No untracked generated composition enters release.

---

# 95. Determinism Contract

Audio:

- consumes no gameplay RNG,
- mutates no simulation state,
- changes no save checksum.

Test:

```text
muted digest == audible digest
```

---

# 96. Lifecycle Contract

Session replacement:

```text
stop/fade old audio
unsubscribe
bind new state
reconstruct desired mix
```

No orphan loops.

---

# 97. Performance Contract

Budgets:

- max voices/bus,
- max total voices,
- event transition rate,
- no per-frame state polling.

Measure on min-spec target.

---

# 98. Release Report Integration

Release report should include:

```text
AUDIO_CUES_TOTAL
WEATHER_AUDIO_COVERAGE
AUDIO_UNMAPPED_WEATHER
ORPHAN_AUDIO_LOOPS
MAX_STORM_CONCURRENT_VOICES
ALERT_PILEUP_VIOLATIONS
AUDIO_SETTINGS_RECOVERY
AUDIO_ASSET_PROVENANCE_MISSING
```

---

# 99. Failure Modes

## Surface still silent

Position event wiring missing.

## Geiger cannot stop

Exposure-end contract incomplete.

## Silence weather simply has no mapping

Implement explicit silence state.

## Three alerts play full-volume together

Priority/duck/cap policy failed.

## Two unrelated threats use same cue

Semantic uniqueness gate fails.

## Bus slider resets after restart

Settings round-trip fails.

## Device loss removes only warning channel

Visual parity failure.

## Music changes when opening a panel

Screen-driven routing bug.

## Grief plays dramatic music

Violates subtraction policy.

## Audio changes campaign checksum

Critical architecture failure.

---

# 100. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| audio state machine duplicates shelter controller | Medium | High | extend one owner |
| event subscriptions leak on load | Medium | High | lifecycle tests |
| weather mappings become 22 bespoke cases | Medium | Medium | family mapping |
| silence suppresses critical alert | Low–Med | High | bus allowlist |
| ducking creates pumping | Medium | Medium | authored attack/release |
| too many alert priorities | Medium | Medium | small semantic taxonomy |
| accessibility option hides warning | Low–Med | High | mandatory parity |
| music content scope expands | High | Medium | 3–5 piece hard ceiling |
| audio asset provenance missing | Medium | Medium | 50A manifest gate |
| loop seams click | Medium | Low–Med | boundary QA |
| min-spec voice count too high | Medium | Medium | caps + soak |

---

# 101. Commit Strategy

## C2[23].1 — baseline + ambience state ADR

## C2[23].2 — one ambience owner + initial state

## C2[23].3 — surface position transitions

## C2[23].4 — weather family mapping + silence

## C2[23].5 — seasonal/occupancy/power modifiers

## C2[23].6 — exposure-end + geiger bands

## C2[23].7 — lifecycle/perf/determinism tests

### Gate: 52A complete

## C2[23].8 — mix policy + ducking

## C2[23].9 — alert priority/dedup/caps

## C2[23].10 — semantic collision replacements

## C2[23].11 — loudness normalization

## C2[23].12 — bus settings + recovery

## C2[23].13 — accessibility/silence/device-loss

## C2[23].14 — storm soak + audit closure

### Gate: 52B complete

## C2[23].15 — MUSIC_PLAN + trigger map

## C2[23].16 — state-driven music projection

## C2[23].17 — season/grief/ending mappings

## C2[23].18 — voice policy + playback coordination

## C2[23].19 — loop/loudness/provenance gates

## C2[23].20 — session-shape soak

### Gate: 52C complete

## C2[23].21 — Wave‑8 audio closure

---

# 102. Verification Checklist

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --audio-selftest
bash scripts/ci/generate-audio-catalog.py --check
bash scripts/ci/asset-orphan-sweep.sh
bash scripts/ci/lfs-health-check.sh
bash scripts/ci/verify-fast.sh
```

Also run repository-canonical equivalents of:

```text
ashfall-audio-qa
automated AUDIO_AND_SETTINGS_RECOVERY matrix
seeded storm-soak mix log
audio-muted vs audible SaveChecksum parity
session-shape music soak
asset manifest/provenance check for new cues/music
```

---

# 103. Flagship Definition of Done

## 52A — Ambience

- [ ] state graph,
- [ ] single ambience owner,
- [ ] surface ambience live,
- [ ] 22/22 weather treatment,
- [ ] silence-class behavior,
- [ ] seasonal variants,
- [ ] explicit exposure-end event,
- [ ] geiger start/band/stop,
- [ ] power-state audibility,
- [ ] occupancy,
- [ ] event-driven transitions,
- [ ] no double-start,
- [ ] session-swap cleanup,
- [ ] voice caps,
- [ ] min-spec budget,
- [ ] determinism neutrality.

## 52B — Mix

- [ ] ducking,
- [ ] alert priority,
- [ ] dedup,
- [ ] concurrency caps,
- [ ] semantic collision audit,
- [ ] required cue de-dup assets,
- [ ] loudness report,
- [ ] user bus controls,
- [ ] settings recovery automated,
- [ ] semantic alert uniqueness,
- [ ] visual/text parity,
- [ ] reduce audio chaos,
- [ ] explicit silence mix state,
- [ ] device-loss safe,
- [ ] storm soak proves policy,
- [ ] SILENCE_AUDIT updated.

## 52C — Music

- [ ] 3–5 piece ceiling,
- [ ] state-driven roles,
- [ ] pressure inputs,
- [ ] season variation,
- [ ] grief subtraction,
- [ ] game-over distinction,
- [ ] ending families,
- [ ] voice spoken/subtitle policy,
- [ ] one coordinated playback owner,
- [ ] crossfade/re-entry rules,
- [ ] no music during silence,
- [ ] seamless loops,
- [ ] loudness continuity,
- [ ] provenance,
- [ ] deterministic state mapping,
- [ ] session-shape soak.

## Global

- [ ] no new buses,
- [ ] no invented mechanics/weather,
- [ ] no music over critical information,
- [ ] no alert without visual equivalent,
- [ ] no audio→simulation mutation,
- [ ] full verification green.

---

# 104. Closure Report Template

```markdown
## C2[23] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:

### Baseline
- Cues:
- Buses:
- Weather audio mappings:
- Surface ambience gameplay callers:
- Geiger stop path:
- Duplicate semantic cue assets:
- Music cues:
- Audio settings automation:

### 52A — Ambience
- State graph:
- Surface:
- Weather coverage:
- Silence:
- Seasons:
- Exposure end:
- Geiger:
- Power:
- Occupancy:
- Event-driven:
- Session swap:
- Max voices:
- Checksum parity:
- Result:

### 52B — Mix
- Ducking:
- Priority:
- Dedup:
- Caps:
- Collision replacements:
- Loudness:
- Settings:
- Recovery:
- Alert parity:
- Reduce chaos:
- Silence mix:
- Device loss:
- Storm soak:
- Audit closure:
- Result:

### 52C — Music
- Planned pieces:
- Produced/retained pieces:
- State inputs:
- Seasonal variation:
- Grief:
- Ending families:
- Voice policy:
- Crossfade:
- No-music silence:
- Loop integrity:
- Loudness:
- Provenance:
- Session-shape soak:
- Result:

### Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Bridge:
- Audio selftest:
- Catalog check:
- Asset orphan sweep:
- LFS:
- Audio QA:
- Settings recovery:
- Verify fast:

### Final Metrics
- AUDIO_CUES_TOTAL:
- WEATHER_AUDIO_COVERAGE:
- SURFACE_AMBIENCE_TRANSITIONS:
- ORPHAN_LOOPS:
- ALERT_PILEUP_VIOLATIONS:
- MAX_CONCURRENT_VOICES:
- BUS_SETTINGS_ROUNDTRIP_FAILURES:
- AUDIO_VISUAL_PARITY_GAPS:
- MUSIC_STATE_MISMATCHES:
- AUDIO_SIMULATION_CHECKSUM_DIFF:

### Remaining Debt
- Ambience:
- Alerts:
- Settings:
- Music:
- Voice:
- Assets:
```

---

# 105. Final Execution Directive

Execute Plan 52 as a **state-expression and mix-discipline** repair.

The critical sequence is:

```text
make ambience a real state machine
→ bind surface/weather/season/power/occupancy/exposure
→ author silence explicitly
→ prove loops start and stop
→ implement ducking/priority/caps
→ measure the storm pile-up
→ expose user bus controls
→ add only a small state-driven musical identity
→ preserve silence around grief and critical information
```

Do not add more buses.

Do not create new mechanics merely to justify audio.

Do not use a cue as the sole warning channel.

Do not let music follow UI screens.

Do not let audio consume simulation RNG or mutate state.

The strongest ambience rule is:

> **Audio follows the same place, weather, season, power, occupancy, and exposure authorities the simulation already trusts.**

The strongest mix rule is:

> **Under pressure, the player hears the most actionable information first; everything else ducks, deduplicates, defers, or becomes silence according to one authored policy.**

The strongest music rule is:

> **Music gives the session shape by appearing selectively; silence remains an authored part of the score.**

The flagship acceptance scenario is:

> **Walk from a powered crowded bunker into a storming irradiated surface, cross into a silence-class weather state, return during load shedding, trigger a high-priority alert, then progress through a survivor death and an ending. The audio state must start/stop the right beds, stop geiger exposure correctly, keep alert semantics legible, honor user settings, deliberately drop into silence at the authored moments, and leave the simulation checksum identical when all audio is muted.**
