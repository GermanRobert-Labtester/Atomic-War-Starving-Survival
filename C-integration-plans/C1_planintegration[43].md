# C1 — Flagship Integration Plan [43]: Mental-Health UI & Catharsis, Shelter Acoustic Director, and Deep-Strata Archive Decryption UI

> **Scope:** Tasks 5–8 — Plan 52 UI Follow-up, Plan 52 Narrative Follow-up, Plan 53 Audio Follow-up, Plan 62 UI Follow-up
>
> **Primary mission:** integrate four already-conceived systems into the playable Godot host without creating parallel authorities: expose `SurvivorMentalHealthSystem` through survivor/affliction UI; add catharsis and crisis-intervention narrative rails while trauma truth remains mental-health-owned; connect `ShelterAcousticDirector` to real assets/Godot buses through a headless-safe bridge; and build `ArchiveDecryptionPanel` as a thin presentation surface over archive, inventory, assignment, research, tech-tree, audio, and localization authorities.
>
> **Architecture doctrine:** UI projects state and submits intents. Core owns truth. Adapters translate. Host orchestration coordinates. Audio presentation never mutates gameplay. Quest completion never bypasses mental-health recovery rules. Archive UI never unlocks research on its own.
>
> **Mandatory execution order:** P0 authority audit → Task 5 UI adapter/afflictions integration → Task 6 catharsis/interventions → Task 7 asset/bus reconciliation → Task 8 archive panel → persistence/migration → deterministic/headless tests → accessibility/localization → long-horizon/Monte-Carlo/performance → CI/SHIP gate.

## Executive Corrections

1. **Stress Floor remains trauma-derived truth.** A recovered heirloom may reduce current stress or provide temporary consolation, but cannot silently lower the base Stress Floor unless `SurvivorMentalHealthSystem` resolves/reclassifies the underlying trauma.
2. **Catharsis quests do not delete trauma.** `PersonalQuestSystem` owns quest completion; mental health consumes a recovery milestone and decides full/partial/no resolution.
3. **Shared Grief requires an actual social event.** Sharing the same fallen squadmate is eligibility, not an automatic affinity tick.
4. **Sedation is medical.** The crisis modal only offers it if medical/pharmaceutical authority validates drug, contraindications, authorization, and inventory.
5. **“Sensory Deprivation” is gated/reframed.** Ship only if already an authored, safe recovery action; otherwise use quiet low-stimulation observation with room/schedule/medical constraints.
6. **Hardened Veteran is gated.** Trait authority owns it if retained; do not award a blanket permanent `-20% all stress` simply for accumulating/resolving three traumas.
7. **Audio randomness is presentation-only.** Geiger click stochasticity must never influence radiation simulation or save determinism.
8. **Archive UI is presentation-only.** Solvent consumption, assignment, decryption ticks, research rewards, and tech unlocks commit through their canonical systems.

---

# 1. Cross-System Authority Invariants

- `SurvivorMentalHealthSystem` owns trauma tokens, trauma recovery state, crisis state, Stress Floor, therapy progression, and mental-health resolution.
- canonical stress/Needs authority owns current stress if current stress is not itself mental-health-owned.
- `PsychologicalTraumaCatalog` owns trauma/recovery definitions and stable localization keys.
- `ShelterScheduleSystem` owns room/time reservations and exclusivity.
- SkillProgression/Trait authority owns empathy, medical, counseling, intelligence, cryptography, and related qualifications.
- `PersonalQuestSystem` owns personal quest lifecycle.
- Plan 210 item/personal-belongings authority owns recovered relic identity, existence, ownership, and provenance.
- `SurvivorRelationsSystem` owns affinity/trust.
- TraitSystem owns permanent traits.
- medical/pharmaceutical authority owns sedation, contraindications, drug consumption, adverse outcomes, and medical eligibility.
- `JournalSystem` owns journal storage/history.
- `ShelterAcousticDirector` owns acoustic intent/snapshots; Godot `AudioServer`/bus effects belong only in host/presentation code.
- radiation authority owns `ambientRadiationMillisieverts`; Geiger audio only sonifies it.
- `PrewarArchiveDecryptionSystem` owns archive condition, oxide state, encryption state, decryption progress, fragment reveal, and reward readiness.
- `IPlayerInventoryPort` owns inventory consumption/transactions.
- canonical scheduler/assignment system owns cryptographer assignment.
- `ResearchSystem` owns research knowledge/unlocks.
- `TechTree` owns technology-node unlock state.
- translation catalogs own all player-facing strings.
- restore is side-effect free.
- consequential events are exactly once.
- no `Guid.NewGuid`, wall-clock time, or unseeded simulation RNG.

---

# 2. Definition of Done

The bundle closes only when:

- both survivor panels render mental-health state through `SurvivorMentalHealthUiAdapter`;
- stress bands and Stress Floor are distinct, accessible, and authority-backed;
- therapy dropdown comes from `PsychologicalTraumaCatalog.recovery_actions`;
- room/counselor/schedule prerequisites are queried, not hardcoded in UI;
- quiet-room action reserves canonical schedule/room state;
- scoped quiet-room audio presentation cannot suppress alerts;
- six catharsis definitions are data-driven and reachable;
- catharsis quest completion routes through mental-health recovery logic exactly once;
- heirloom consolation uses canonical personal-item references;
- Shared Grief applies relationship effects only through real bonding events;
- crisis intervention options validate medical, inventory, room, schedule, and autonomy prerequisites;
- 100+ seeded crisis simulations show no newly introduced arbitrary unpreventable death path;
- active catharsis linkage restores without replay;
- all 11 shelter cue IDs resolve to real assets or explicitly tracked development placeholders;
- buses `Master`, `Machinery`, `Atmosphere`, `Geiger`, `Events`, `Turntable` load correctly;
- acoustic bridge is headless-safe and allocation-free in steady hot-loop evaluation;
- ducking and low-pass behavior are bounded and reversible;
- Geiger sonification is continuous-rate/Poisson-based and simulation-independent;
- archive panel implements `IContractPanel` and current layout conventions;
- solvent action is transactional;
- cryptographer assignment cannot double-book;
- decryption progress is system-owned;
- research/tech unlocks are canonical and exactly once;
- transcript waveform is cached/precomputed or otherwise non-hot-loop;
- localization and accessibility gates pass;
- save/load, scene lint, data-integrity, focused tests, selftests, and `verify-fast` pass.

---

# 3. P0 — Forensic Authority Audit

Before editing target UI files, inspect and record:

## Mental health
- `SurvivorMentalHealthSystem`
- stress owner / `NeedsSystem`
- `PsychologicalTraumaCatalog`
- trauma DTOs/tokens
- crisis DTO/state
- recovery action schema
- therapy resolver
- `SurvivorMentalHealthSaveStore`
- `SaveSectionRegistry.cs`
- `ShelterScheduleSystem`
- room IDs and reservation APIs
- medical/pharmaceutical APIs
- survivor empathy/medical/counseling skill sources
- `PersonalQuestSystem`
- `SurvivorRelationsSystem`
- TraitSystem
- Plan 210 personal belongings
- `JournalSystem`

## UI
- `src/UI/AfflictionsPanel.cs`
- `src/UI/SurvivorDetailPanel.cs`
- existing `IContractPanel` implementations
- existing UI adapters
- translation lookup
- icon resources
- tooltip/accessibility helpers
- panel audio/listening-profile helpers
- layout/text-scale conventions

## Acoustic
- `ShelterAcousticDirector`
- `ShelterAudioCueCatalog`
- `shelter_audio_cues.json`
- `src/Audio/ShelterAcousticBridge.cs`
- `assets/audio/`
- `default_bus_layout.tres`
- radiation authority
- room/camera inspection source
- current headless detection/runtime path

## Archive
- `PrewarArchiveDecryptionSystem`
- archive DTO/state
- solvent APIs
- `IPlayerInventoryPort`
- assignment/scheduler APIs
- `ResearchSystem`
- `TechTree`
- archive reward events
- archive audio-log catalog
- room inspection IDs
- `Main.UiPanels.cs`
- `CatalogIntegrityValidator`

Create `docs/integration/TASKS_5_8_AUTHORITY_MATRIX.md` with:
`fact | owner | read API | command API | adapter role | persisted? | exactly-once event | status`.

Create/update ADRs:
- `ADR_MENTAL_HEALTH_UI_PRESENTATION_BOUNDARY.md`
- `ADR_CATHARSIS_QUEST_VS_TRAUMA_RECOVERY.md`
- `ADR_MENTAL_HEALTH_CRISIS_INTERVENTION_AUTHORITY.md`
- `ADR_ACOUSTIC_DIRECTOR_CORE_VS_GODOT_AUDIO.md`
- `ADR_AUDIO_RANDOMNESS_AND_HEADLESS_BEHAVIOR.md`
- `ADR_ARCHIVE_UI_VS_DECRYPTION_RESEARCH_AUTHORITY.md`

Baseline fixtures:
1. no-trauma survivor;
2. one combat-shock trauma;
3. multi-trauma survivor with nonzero floor;
4. active acute crisis;
5. quiet-room therapy;
6. eligible/ineligible counselor set;
7. archive with no solvent;
8. archive with solvent;
9. no cryptographer;
10. archive at 99%;
11. radiation 0/low/medium/high;
12. airlock event over generator ambience.

---

# 4. Task 5 — AfflictionsPanel & Psychological Ward UI

## 4.1 `SurvivorMentalHealthUiAdapter`

Create `src/UI/Adapters/SurvivorMentalHealthUiAdapter.cs`.

Responsibilities:
- read canonical mental-health state;
- read canonical current stress;
- resolve trauma presentation;
- resolve localization/icon keys;
- resolve recovery actions;
- query prerequisite/eligibility services;
- expose immutable presentation DTO;
- submit intents through command ports;
- never edit Core collections.

Suggested DTO:

```text
MentalHealthPresentation
  survivor_id
  stress_permille
  stress_band
  trauma_stress_floor_permille
  crisis optional
  trauma_tokens[]
  therapy_options[]
  counselor_candidates[]
  quiet_room_state
  warnings[]
  revision
```

Trauma token:

```text
TraumaTokenPresentation
  trauma_id
  trauma_type
  title_key
  description_key
  icon_key
  severity_band
  floor_contribution_permille
  recovery_state
  recovery_action_ids[]
  source_memory_ref optional
```

Recovery option:

```text
RecoveryActionPresentation
  action_id
  title_key
  instructions_key
  enabled
  disabled_reason_key optional
  room_requirements[]
  item_requirements[]
  counselor_requirement optional
  schedule_cost
  expected_duration
```

## 4.2 Afflictions section

Add **Psychological Traumas & Shell-Shock** alongside physical injuries.

Render each trauma token:
- high-contrast icon;
- localized title;
- severity band;
- localized short description;
- floor contribution;
- recovery state;
- details/action affordance.

IDs such as `trauma_combat_shock` and `trauma_survivor_guilt` are content/icon IDs, never display strings.

Use one catalog/resource mapping, not scattered switch statements.

## 4.3 Stress permille gauge

Requested thresholds:
- 0–400 Green;
- 401–749 Amber;
- 750–1000 Red.

Accessibility correction:
- add semantic labels (`Stable`, `Strained`, `Critical` or canonical equivalents);
- show numeric permille;
- mark 400 and 750 boundaries;
- theme resources own colors;
- color cannot be the only state signal.

Boundary tests:
- 0, 400, 401, 749, 750, 1000.

## 4.4 Stress Floor

Render a distinct marker/line:
- localized label;
- numeric permille;
- accessible explanation;
- optional per-trauma contribution details.

UI **must not recompute the floor**.
Read it from `SurvivorMentalHealthSystem`.

## 4.5 Acute crisis banner

Show:
- localized crisis title/type;
- remaining canonical duration;
- capability/work penalty read from canonical projection;
- intervention availability;
- urgency label/icon.

UI must not calculate or persist the work penalty.

## 4.6 Prescribe Therapy

Populate from `PsychologicalTraumaCatalog.recovery_actions`.

Adapter filters:
- trauma compatibility;
- active recovery state;
- room prerequisites;
- inventory/item prerequisites;
- counselor requirements;
- schedule availability;
- medical contraindications if applicable.

Room examples (`room_bunks`, `room_main`) are accepted only if the catalog/current repo declares them.
No hardcoded room logic in the panel.

Disabled options remain visible where useful and include a localized reason.

## 4.7 Counselor selector

Query:
```text
GetCounselorCandidates(survivorId, recoveryActionId)
```

Eligibility may use:
- empathy;
- medical skill;
- counseling specialization;
- relationship conflict;
- current duty;
- fatigue/illness;
- schedule availability.

UI does not implement raw skill thresholds.

## 4.8 Quiet-room quick action

Flow:
1. UI queries adapter eligibility.
2. adapter builds therapy/schedule intent.
3. schedule validates room/bunk/time.
4. reservation commits.
5. mental-health recovery action links to reservation.
6. UI refreshes from revision.

Handle:
- missing room;
- room full;
- treatment overlap;
- critical duty;
- survivor refusal if autonomy supports it.

## 4.9 Scoped soothing audio

When viewing a survivor in quiet-room therapy:
- request a scoped low-pass/high-frequency attenuation profile for **viewer UI audio presentation**;
- release on panel close, blur, survivor change, therapy end, or scene exit;
- do not alter simulation;
- do not suppress alarms/accessibility cues.

## 4.10 SurvivorDetail summary

Add compact:
- stress band;
- floor;
- trauma count;
- active crisis;
- active therapy;
- deep link to full afflictions/mental-health section.

Do not duplicate the whole section.

## 4.11 Headless smoke

Add:
```text
--afflictions-mental-health-selftest
```

Verify adapter presentation for:
- no trauma;
- one trauma;
- multiple trauma;
- crisis;
- missing room;
- counselor filtering;
- quiet-room eligibility;
- localization;
- no render/audio dependency.

## 4.12 Tests

`AfflictionsPanelMentalHealthTests.cs`:
- token ordering/population;
- localization;
- icon mapping;
- stress boundaries;
- floor marker;
- crisis banner/duration;
- therapy enable/disable;
- prerequisite tooltip;
- counselor candidates;
- quiet-room button;
- revision refresh;
- scoped audio release;
- no hardcoded strings.

Update `docs/ui/ACCESSIBILITY_REPORT.md` for contrast, non-color labels, floor gauge, focus order, keyboard/controller, and screen-reader sequence.

---

# 5. Task 6 — Catharsis Quests & Breakdown Interventions

## 5.1 Six catharsis breakthrough definitions

Extend `Assets/StreamingAssets/Data/psychological_trauma.json` with six data-driven definitions:

```text
breakthrough_id
compatible_trauma_tags[]
eligibility_predicates[]
personal_quest_template_id
completion_signal
recovery_request_id
localization_keys
memory_tags[]
cooldown_policy
```

Recommended archetypes:
1. **Face the Place** — confront/revisit a trauma-linked location or circumstance.
2. **Speak the Name** — acknowledge a fallen survivor in a safe social context.
3. **Return the Relic** — recover/externalize a canonical personal belonging.
4. **Make Amends** — address guilt through a real restitution/help action.
5. **Stand the Watch** — controlled mastery exposure for fear/combat trauma.
6. **Carry It Forward** — transform grief into memorial/helping action.

Final IDs must match actual trauma/content semantics.

## 5.2 PersonalQuest bridge

Flow:
```text
mental-health eligibility
→ PersonalQuestSystem candidate request
→ quest created/activated by quest authority
→ canonical objectives completed
→ quest completion event
→ one catharsis milestone
→ SurvivorMentalHealthSystem evaluates recovery
```

Never persist full quest state twice.

High stress alone is insufficient. Validate:
- unresolved trauma;
- active quest limit;
- content/location reachability;
- required item;
- required NPC/relation;
- medical readiness;
- cooldown.

## 5.3 Heirloom Consolation

Use Plan 210 canonical item instance.

Allowed effects:
- reduce current stress;
- temporary calming modifier;
- therapy willingness/recovery modifier;
- bounded crisis-escalation modifier if mental-health authority supports it.

Forbidden:
- direct mutation of trauma token floor contribution;
- permanent base Stress Floor reduction without actual trauma recovery.

If temporary floor relief is desired, model:
```text
effective_floor_offset
```
inside `SurvivorMentalHealthSystem`, with deterministic expiry and unchanged base trauma floor.

## 5.4 Shared Grief

Eligibility:
- same deceased/squadmate source;
- both survivors know/remember the loss;
- both available;
- actual social/bonding event occurs;
- cooldown.

Then:
```text
shared_grief_event
→ SurvivorRelationsSystem
→ affinity/trust consequence exactly once
```

No passive affinity tick from matching memory refs.

## 5.5 Acute-crisis intervention modal

Drive modal from canonical active-crisis state.
The requested `950 permille` threshold is only a UI/eligibility rule if it matches Core semantics.

Choices:

### Emergency Sedation
Requires:
- recognized pharmaceutical;
- stock available;
- contraindication check;
- medical authorization;
- canonical inventory transaction.

UI never directly clears crisis.

### Empathetic Vigil
Requires:
- companion;
- availability;
- relationship/skill suitability;
- canonical 24h (or configured duration) schedule reservation;
- no critical overlapping duty.

### Quiet Low-Stimulation Observation
Use this safer semantic unless `Sensory Deprivation` is already a validated authored therapy.
Requires:
- safe room;
- monitoring;
- schedule capacity;
- medical compatibility.

## 5.6 No-unpreventable-death invariant

Run at least 100 seeds, preferably 1,000 nightly:
- no counselor;
- no medication;
- no quiet room;
- high trauma;
- high stress;
- low resources;
- repeated crises.

New interventions must not introduce arbitrary unavoidable instant-death outcomes.

If another existing system contains self-harm/suicide mechanics, do not expand them in this task; test only that these new branches do not create unfair unpreventable paths.

## 5.7 Hardened Veteran disposition

**REWORK/GATE.**

Do not implement:
```text
if resolvedTraumas >= 3:
    globalStressGain *= 0.8
```

If retained:
- TraitSystem owns trait;
- mental health emits eligibility milestone;
- effect is contextual (e.g. familiar combat-stressor class), bounded, and non-global;
- no repeated trauma cycling;
- no incentive to acquire trauma intentionally.

## 5.8 Despair → acoustic ambience

Mental health can expose read-only aggregate:

```text
ShelterPsychologicalAmbienceObservation
  despair_band
  acute_crisis_count
  grieving_count
  revision
```

`ShelterAcousticDirector` may use it for subtle ambience.
No audio→stress feedback.
Critical alerts remain dominant.

## 5.9 Journal breakthrough

Mental health emits:
`survivor_catharsis_breakthrough`.

`JournalSystem` owns storage/text generation.
Use localized templates and survivor/trauma/item/person/location tokens.
Mental-health save stores references, not diary prose.

## 5.10 Catharsis tests

`SurvivorCatharsisQuestTests.cs` must test:
- six definitions valid;
- quest reachability;
- exactly-once completion;
- partial vs full recovery;
- Stress Floor recomputation from **remaining unresolved traumas**;
- consolation expiry;
- shared-grief relation event once;
- sedation transaction;
- vigil reservation;
- low-stimulation prerequisites;
- trait milestone gating;
- seed determinism;
- save roundtrip;
- duplicate completion protection.

Audit `SaveSectionRegistry.cs` before modifying it.
`SurvivorMentalHealthSaveStore` may persist catharsis linkage/processed milestone/temp consolation state, but not duplicate full quest/item/schedule/relation/journal state.

Update `docs/SURVIVOR_MENTAL_HEALTH_AUTHORITY_MAP.md`.


---

# 6. Task 7 — Shelter Acoustic Director Audio Asset Integration & Bus Balancing

## 6.1 Asset inventory before authoring

Inventory `assets/audio/` recursively before generating anything.

Parse all 11 IDs from `shelter_audio_cues.json` and produce:

`docs/audio/SHELTER_AUDIO_CUE_ASSET_MATRIX.md`

Columns:
```text
cue_id
catalog_path
resolved_path
exists
duration
channels
sample_rate
loop
target_bus
priority
placeholder
notes
```

For every missing cue:
1. reuse a suitable already-owned asset if one truly matches;
2. otherwise map a clearly marked development placeholder;
3. otherwise author/generate the final asset through the project audio pipeline;
4. update catalog;
5. run integrity validation.

Do not silently map several production cues to one placeholder without tracking it.

Target examples include:
- hydraulic groan;
- radiation tick;
- airlock pressurization;
- generator hum.

## 6.2 Godot bus layout

Configure `default_bus_layout.tres`:

```text
Master
├── Machinery
├── Atmosphere
├── Geiger
├── Events
└── Turntable
```

Semantics:
- **Master**: global output.
- **Machinery**: generators, pumps, hydraulics, ventilation machinery.
- **Atmosphere**: shelter beds, distant chatter, structural ambience, despair drones.
- **Geiger**: radiation sonification only.
- **Events**: airlock, radio burst, alarm-like/high-priority one-shots.
- **Turntable**: music/soothing playback.

Never hardcode bus numeric indices.
Resolve names once and cache handles/indices.

## 6.3 Core vs Godot authority

`ShelterAcousticDirector` should produce an immutable snapshot:

```text
ShelterAcousticSnapshot
  machinery_permille
  atmosphere_permille
  geiger_intensity
  turntable_permille
  active_cues[]
  room_filter_context
  priority_state
  revision
```

`ShelterAcousticBridge` converts snapshot to Godot presentation.

Core must not call:
- `AudioServer`;
- `AudioStreamPlayer`;
- Godot bus effects;
- scene-tree audio nodes.

## 6.4 Permille → dB mapping

Implement perceptual/logarithmic mapping.

Do not use:
```text
db = permille / 1000 * maxDb
```

Recommended form:
```text
if permille <= 0:
    return MIN_DB

linear = clamp(permille / 1000.0, epsilon, 1.0)
db = 20 * log10(linear)
return clamp(db, MIN_DB, 0)
```

Use project-appropriate `MIN_DB`, typically around -80 dB.

Lock expected behavior at:
- 0;
- 1;
- 10;
- 100;
- 250;
- 500;
- 750;
- 1000.

No `log10(0)`.
No NaN.
No positive overflow.

## 6.5 Smooth interpolation

Bridge maintains current and target bus state.

Requirements:
- no zipper noise;
- delta-time-based smoothing;
- deterministic target computation;
- no simulation feedback;
- headless path skips Godot calls.

Use one consistent domain:
- dB smoothing;
or
- linear gain smoothing then conversion.

Document choice.

## 6.6 Dynamic ducking

When a high-priority event such as `acue_airlock_cycling` fires:
- duck `Machinery` by 6 dB;
- optionally duck `Atmosphere` under a separate documented rule;
- leave `Events` clear.

Define:
- attack time;
- hold semantics;
- release time;
- overlapping cue behavior;
- cancellation behavior.

Use token/ref-count/priority mixer so overlapping events do not accidentally produce:
- -6;
- -12;
- -18 dB
unless policy explicitly stacks.

After final duck request ends:
- restore pre-duck target smoothly.

## 6.7 Poisson Geiger sonification

Input:
`ambientRadiationMillisieverts`

Radiation simulation remains sole owner.

Map radiation to click rate:
```text
lambda = f(mSv)
```

Requirements:
- monotonic;
- bounded;
- calibrated for intelligibility;
- near-zero radiation → sparse/no clicks;
- high radiation → saturating maximum;
- no implication that audible click count is exact dosimetry unless intentionally calibrated.

Prefer inter-arrival scheduling:
```text
dt = -ln(1-u) / lambda
```

Benefits:
- continuous Poisson process;
- no frame-rate dependence;
- avoids per-frame Bernoulli approximation.

Presentation RNG:
- separate from simulation RNG;
- stable/seedable in tests;
- never persisted as gameplay truth;
- never changes radiation state.

## 6.8 Machinery low-pass by room transition

When player/camera moves:
- generator room → living quarters,
smooth low-pass on `Machinery`.

Source room context must be canonical:
- selected room;
- player/camera location;
- inspection context.

Do not infer room from audio state.

Implementation:
- create/cache low-pass bus effect once;
- change cutoff parameter;
- smooth transition;
- never allocate/remove effect every frame.

Do not low-pass:
- `Events`;
- emergency alarms;
- accessibility cues.

## 6.9 10 Hz ambient reconciliation

`ShelterAcousticBridge` can run from `_Process`, but expensive snapshot reconciliation should be throttled:

```text
_Process(delta):
    smooth current targets
    accumulator += delta
    if accumulator >= 0.1:
        pull/apply ambient snapshot
        accumulator %= 0.1
```

High-priority one-shot cues:
- may dispatch immediately via event subscription;
- should not wait up to 100 ms if perceptibly wrong.

## 6.10 Headless runtime abstraction

Introduce/reuse:

```text
IAudioRuntime
  bool IsAvailable
  SetBusDb(...)
  SetLowPass(...)
  PlayCue(...)
  StopCue(...)
```

Godot implementation:
- resolves buses;
- plays streams.

Headless implementation:
- safe no-op;
- preserves command validation;
- never touches `AudioServer`.

No Core unit test should require initialized Godot audio.

## 6.11 Zero-allocation hot path

After warmup, target zero managed allocations for:
- acoustic snapshot evaluation;
- bus target calculation;
- ducking state update;
- low-pass update;
- Geiger scheduling.

Actions:
- cache cue lookup;
- cache bus indices;
- cache effect handles;
- reuse buffers;
- avoid LINQ in steady loop if allocating;
- avoid repeated string formatting;
- avoid transient dictionaries;
- no new event lists each frame;
- no per-click object allocation.

Benchmark:
- 60 FPS simulated bridge loop;
- 10 Hz snapshot updates;
- nested events;
- radiation sweep;
- room transitions.

Measure:
- allocations/evaluation;
- Gen0/1/2 collections;
- p50/p95 evaluation time.

## 6.12 Headless smoke test

Create:
`ShelterAcousticHeadlessSmokeTests.cs`

Test:
- construct director + unavailable runtime;
- apply all 11 cues;
- radiation sweep;
- room filters;
- ducking;
- no crash;
- no Godot audio server access;
- no null dereference.

## 6.13 Audio math tests

Create/update:
`AshfallAudioDirectorTests.cs`

Test:
- permille clamps;
- exact boundary mapping;
- smoothing convergence;
- duck attack/release;
- nested duck;
- restoration;
- radiation→lambda;
- seeded Poisson inter-arrivals;
- low-pass transitions;
- bus resolution;
- missing bus behavior;
- cue priority;
- snapshot revision handling;
- allocation budget.

## 6.14 Catalog integrity

Update `CatalogIntegrityRules.cs` so:
- every production `asset_path` exists;
- filesystem case matches exactly;
- extension supported;
- cue ID unique;
- bus name valid;
- loop metadata valid if required;
- placeholder explicitly marked;
- production build can reject placeholders if policy requires.

Run:
```bash
python3 scripts/ci/scene-lint.py
```

Validate:
- `default_bus_layout.tres`;
- stream resources;
- node/resource references;
- import paths.

## 6.15 Acoustic authority map

Update:
`docs/SHELTER_ACOUSTIC_AUTHORITY_MAP.md`

Include:
- Core snapshot;
- Godot bridge;
- bus tree;
- cue-routing matrix;
- permille→dB curve;
- ducking envelope;
- radiation→lambda curve;
- room low-pass curve;
- headless path;
- allocation/performance budgets.

---

# 7. Task 8 — Deep-Strata Library & Archive Decryption UI

## 7.1 Panel contract

Create:
`src/UI/ArchiveDecryptionPanel.cs`

Requirements:
- inherits `PanelContainer`;
- implements `IContractPanel`;
- follows existing bind/unbind lifecycle;
- no direct save-store access;
- no direct simulation tick;
- no direct inventory mutation;
- no direct research/tech mutation.

Inspect at least 2–3 existing contract panels and mirror:
- contract dictionary;
- lifecycle;
- localization refresh;
- signal wiring;
- disposal/unsubscribe;
- test hooks.

## 7.2 Scene

Create:
`assets/ui/panels/ArchiveDecryptionPanel.tscn`

Conform to 1920×1080 fixed-layout contract while still using current container/anchor/text-scale conventions.

Recommended hierarchy:

```text
ArchiveDecryptionPanel
├── Header
├── ArchiveReelSummary
├── ConditionColumn
│   ├── SpoolCondition
│   ├── OxideDegradation
│   └── EncryptionStrength
├── DecryptionColumn
│   ├── Progress
│   ├── FragmentList
│   └── TreatmentGauge
├── AssignmentColumn
│   ├── CryptographerSelector
│   └── AssignmentStatus
├── TranscriptColumn
│   ├── PlaybackControls
│   ├── Waveform
│   └── TranscriptText
├── RewardPreview
├── ActionBar
└── StatusWarnings
```

Avoid pixel-only layout that fails under text scaling.

## 7.3 Archive UI adapter

Create/reuse:
`ArchiveDecryptionUiAdapter`

Suggested presentation DTO:

```text
ArchiveDecryptionPresentation
  archive_id
  title_key
  spool_condition_band
  oxide_degradation_permille
  encryption_strength_band
  solvent_action
  assigned_cryptographer optional
  eligible_cryptographers[]
  progress_permille
  stage_key
  current_fragments[]
  transcript_state
  reward_preview[]
  blockers[]
  revision
```

`Bind(PrewarArchiveDecryptionSystem, IPlayerInventoryPort)` can remain externally compatible, but the panel should immediately delegate to adapter/controller logic.

## 7.4 Reel inspection

Display:
- spool condition;
- oxide degradation;
- encryption strength.

These values come from `PrewarArchiveDecryptionSystem`.

No UI-local formula.

Prefer:
- semantic band;
- optional numeric detail;
- localized explanation.

## 7.5 Apply Solvent transaction

Button flow:

```text
render eligibility
→ user clicks
→ submit ApplySolventIntent
→ archive authority revalidates
→ inventory port reserves/consumes chemical
→ archive treatment commits
→ semantic event emitted
→ UI refreshes from revision
```

Atomicity:
- if inventory changed before commit, fail cleanly;
- if archive no longer eligible, consume nothing;
- duplicate click/command ID cannot double-consume.

UI callback must not simply call `inventory.Remove()` and then mutate archive state.

## 7.6 Residue-removal animation

Presentation only.

The authoritative treatment value:
- commits before/through transaction.

UI may animate:
- old visual value → new committed value.

Animation completion must not perform consumption.

## 7.7 Cryptographer assignment

Do not hardcode:
```text
if survivor.Intelligence >= X
```

Query:
```text
GetCryptographerCandidates(archiveId)
```

Eligibility may include:
- intelligence/cognition;
- research/cryptography skill;
- specialization;
- health;
- fatigue;
- current duty;
- schedule;
- room access;
- active therapy/counseling/vigil;
- existing assignment.

Assignment:
```text
intent
→ scheduler validates
→ reservation/assignment commits
→ archive system receives canonical assignee ref
→ UI refresh
```

No schedule duplication.

## 7.8 Live decryption progress

UI displays:
- progress permille;
- stage;
- newly decoded fragments;
- optional canonical ETA.

No UI-driven ticks.

Use:
- revision/event subscription;
- bounded polling only if current panel framework requires it.

## 7.9 Knowledge fragments and codex

Reveal only fragment IDs marked decoded by archive authority.

Text:
- translation/codex catalog.

Do not expose:
- full future transcript;
- locked lore;
- reward internals prematurely.

## 7.10 Transcript playback

Use canonical archive audio asset.

Player controls:
- play/pause;
- seek if supported;
- volume/bus rules.

Route through established audio architecture.

Waveform:
- precomputed metadata preferred;
- otherwise one-time cached analysis;
- never per-frame FFT;
- not required for headless;
- transcript text remains accessibility source.

## 7.11 Research/TechTree unlock flow

Canonical sequence:

```text
archive reaches reward milestone
→ PrewarArchiveDecryptionSystem emits reward-ready event
→ ResearchSystem validates/applies research knowledge
→ TechTree validates/applies tech unlock
→ notification event
→ panel renders committed result
```

Forbidden:
- UI checks progress==1000 and writes tech node;
- UI grants research directly;
- restore fires reward again.

## 7.12 Acoustic cues

On canonical events:
- decryption advance;
- tape spool;
- recovered radio fragment;
- reward milestone,

request:
- `acue_radio_burst`;
- magnetic tape whir if cataloged.

Use `ShelterAcousticDirector`.
Do not bypass bus/cue catalog.

## 7.13 Registration

Register in:
`Main.UiPanels.cs`

Verify:
- one instance;
- one subscription set;
- proper close/unbind;
- rebind to different archive safely;
- no stale callbacks.

## 7.14 Room inspection

Wire from:
- `room_reading_quiet_room`;
- `room_library`;

But verify actual canonical IDs first.

If IDs differ:
- use repository truth;
- update mapping docs.

Inspection context must identify:
- room/workstation;
- selected archive;
not just open a global first item.

## 7.15 Contract tests

Create:
`ArchiveDecryptionPanelContractTests.cs`

Test:
- `IContractPanel`;
- contract dictionary;
- bind/unbind;
- event subscription exactly once;
- condition labels;
- solvent enabled/disabled;
- inventory race failure;
- assignment candidates;
- schedule conflict;
- progress refresh;
- fragment reveal;
- transcript playback intent;
- waveform fallback;
- reward notification only after canonical commit;
- no duplicate unlock after reopen/restore;
- localization;
- accessibility labels.

Run `scene-lint.py` and update:
`docs/PREWAR_ARCHIVE_AUTHORITY_MAP.md`.

---

# 8. Cross-Task Scheduling Contract

All human-time-consuming actions arbitrate through the same canonical schedule authority.

Mutually exclusive examples:
- survivor receiving quiet-room therapy;
- counselor assigned to therapy;
- companion on 24h empathetic vigil;
- cryptographer decrypting archive;
- normal work shift;
- guard duty;
- sleep;
- medical treatment.

Required scheduler APIs should support:
```text
CanReserve(...)
Reserve(...)
Release(...)
GetConflictReason(...)
```

No panel maintains its own “busy” flag.

Race handling:
1. UI renders survivor as available.
2. another action reserves them.
3. user clicks.
4. command revalidates.
5. command fails with localized reason.
6. no partial side effect.

---

# 9. Cross-Task Presentation Contracts

## 9.1 Quiet-room listening profile
- viewer-local;
- panel scoped;
- no simulation effect;
- alarms exempt;
- released reliably.

## 9.2 Despair ambience
- aggregate mental-health observation only;
- no individual diagnosis leakage;
- audio director consumes read-only band;
- no feedback into stress.

## 9.3 Archive audio
- canonical archive event requests cue;
- director resolves cue;
- bridge routes bus;
- UI only displays playback state.

## 9.4 Waveform
- supplemental visual;
- transcript text accessible;
- cached;
- no simulation coupling.

---

# 10. Persistence & Restore Matrix

| State | Owner | Persisted by this integration? | Rule |
|---|---|---:|---|
| trauma token | SurvivorMentalHealth | existing store | UI reads |
| current stress | Needs/mental health | existing | no UI duplicate |
| Stress Floor | mental health | canonical | UI reads |
| active crisis | mental health | canonical | no replay |
| therapy | mental health | canonical | reconcile schedule |
| counselor reservation | ShelterSchedule | scheduler | no duplicate |
| catharsis quest | PersonalQuest | quest save | mental-health link only |
| heirloom item | Inventory/Plan210 | item save | mental-health ref only |
| shared-grief affinity | SurvivorRelations | relation save | no duplicate |
| Hardened Veteran | TraitSystem | trait save | milestone ref only |
| journal | JournalSystem | journal save | no duplicate |
| acoustic snapshot | derived | no | rebuild |
| bus dB/filter | presentation | no | rebuild |
| Geiger timer | presentation | normally no | rebuild |
| archive state | PrewarArchiveDecryptionSystem | archive save | canonical |
| solvent inventory | Inventory | inventory save | no duplicate |
| cryptographer reservation | schedule | scheduler | reconcile |
| research unlock | ResearchSystem | research save | no duplicate |
| tech unlock | TechTree | tech save | no duplicate |
| panel state | UI | usually no | safe default |

Recommended restore sequence:
1. catalogs/localization;
2. survivors/traits/skills;
3. inventory/items;
4. rooms/facilities;
5. schedule;
6. mental health;
7. quests;
8. relations/journal;
9. archive;
10. research/tech;
11. derived acoustic state;
12. UI adapters/panels.

If current composition root differs, use post-restore reconciliation instead of creating cyclic dependencies.

Restore must not:
- complete therapy;
- consume medicine;
- consume solvent;
- apply affinity;
- grant trait;
- add journal entry;
- unlock research/tech;
- replay audio.

---

# 11. Stable IDs / Exactly Once

Recommended IDs:

```text
therapy-intent:<survivor>:<trauma>:<action>:<sequence>
quiet-room-reservation:<survivor>:<therapy>:<sequence>
catharsis-link:<survivor>:<trauma>:<quest>
catharsis-milestone:<quest-completion-event>
heirloom-consolation:<survivor>:<item-instance>:<sequence>
shared-grief:<deceased>:<survivor-a>:<survivor-b>:<sequence>
crisis-intervention:<crisis>:<choice>:<sequence>
adaptation-milestone:<survivor>:<source-event>
audio-cue:<source-event>:<cue-id>
audio-duck:<event-id>:<priority>
archive-solvent:<archive>:<sequence>
archive-assignment:<archive>:<survivor>:<sequence>
archive-progress:<archive>:<revision>
archive-reward:<archive>:<reward>
```

No GUIDs.
No wall clock.
Stable iteration.
Seeded simulation uncertainty only.

---

# 12. Failure Injection Matrix

## F43.1
UI deletes trauma token after catharsis quest.
**Expected:** authority gate fails.

## F43.2
Heirloom inspection sets `stressFloor = stressFloor - 100`.
**Expected:** Stress Floor authority gate fails.

## F43.3
AfflictionsPanel hardcodes therapy room logic.
**Expected:** catalog/adapter gate fails.

## F43.4
Counselor selector reads raw private skill fields.
**Expected:** UI-adapter gate fails.

## F43.5
Quiet-room button writes schedule dictionary.
**Expected:** scheduler gate fails.

## F43.6
Sedation callback removes drugs and crisis directly.
**Expected:** medical transaction gate fails.

## F43.7
Shared Grief applies affinity every tick.
**Expected:** relations idempotence fails.

## F43.8
Three resolved traumas directly give permanent -20% global stress.
**Expected:** trait/anti-farm gate fails.

## F43.9
Audio bridge calls `AudioServer` in headless.
**Expected:** headless gate fails.

## F43.10
0 permille enters `log10(0)`.
**Expected:** audio math test fails.

## F43.11
Three overlapping airlock cues unintentionally duck -18 dB.
**Expected:** ducking semantics fail.

## F43.12
Low-pass effect recreated every frame.
**Expected:** allocation/performance gate fails.

## F43.13
Geiger RNG changes radiation state.
**Expected:** presentation boundary fails.

## F43.14
Missing production cue silently passes.
**Expected:** catalog integrity fails.

## F43.15
Solvent is removed before archive validates.
**Expected:** atomicity fails.

## F43.16
Progress bar visually reaches 100% and UI unlocks tech.
**Expected:** research authority fails.

## F43.17
Panel reopen doubles subscriptions.
**Expected:** lifecycle test fails.

## F43.18
Waveform FFT runs every frame.
**Expected:** performance gate fails.

## F43.19
One survivor is simultaneously counselor and cryptographer.
**Expected:** schedule exclusivity fails.

## F43.20
Restore reapplies archive reward.
**Expected:** exactly-once gate fails.

---

# 13. Performance Budgets

## Mental-health UI
- snapshot build <1 ms after warmup for one survivor;
- no catalog parse per refresh;
- no icon resource reload;
- revision-driven refresh;
- no frame-loop rebuild.

## Catharsis
- evaluate eligibility on relevant event/day boundary;
- no frame scan across all survivors × traumas × quests;
- index shared grief by deceased/source reference.

## Audio
- steady snapshot evaluation: zero managed allocations after warmup;
- ambient reconciliation ~10 Hz;
- immediate high-priority event dispatch;
- cache bus/effect/cue handles;
- no repeated string lookup;
- no transient LINQ collections.

## Archive
- revision-driven refresh;
- no UI decryption tick;
- waveform cached/precomputed;
- no repeated full localization/catalog scans;
- candidate list refresh on schedule/skill/archive revision only.

---

# 14. Accessibility & Localization

## Mental health
- non-color labels for stress;
- numeric permille;
- Stress Floor label/value;
- trauma icon accessible name;
- crisis semantic urgency;
- disabled therapy reason;
- keyboard/controller;
- screen-reader order:
  `stress → floor → crisis → traumas → therapy → counselor → actions`.

## Crisis modal
- reduced-motion support;
- clear prerequisites;
- clear time/resource cost;
- focus trapping;
- safe cancel/dismiss semantics;
- no flashing-only urgency.

## Audio
- critical alerts bypass ambience filtering/ducking policy as needed;
- Geiger gameplay information requires visual/text equivalent if player decisions depend on radiation awareness.

## Archive
- transcript text is primary accessible information;
- waveform supplemental;
- progress/condition/encryption have text equivalents;
- layout survives text scale;
- disabled actions explain why.

All player-facing content comes from translation catalogs.


---

# 15. Detailed Test Strategy

## 15.1 `AfflictionsPanelMentalHealthTests.cs`

Required fixtures:
- no trauma;
- one trauma;
- multiple traumas;
- crisis;
- active therapy;
- missing room;
- missing counselor;
- full quiet room;
- localization fallback test;
- stress floor > current stress impossible-state validation.

Assertions:
- trauma list stable ordering;
- title/description localization;
- icon key resolution;
- stress 0/400/401/749/750/1000 bands;
- Stress Floor line position/value;
- crisis banner visible only when crisis active;
- remaining duration formatted from canonical game time;
- work penalty is read-only presentation;
- therapy dropdown mirrors recovery catalog;
- room prerequisite disables correctly;
- tooltip/disabled reason available without mouse hover;
- counselor list contains only eligible candidates;
- quiet-room button submits one intent;
- repeated click while pending does not submit twice;
- adapter revision updates labels;
- closing panel removes subscriptions;
- closing panel releases scoped low-pass listening profile.

## 15.2 `SurvivorCatharsisQuestTests.cs`

Fixtures:
- fully resolvable trauma;
- partially resolvable trauma;
- unreachable quest prerequisite;
- personal relic available/unavailable;
- two survivors with same deceased source;
- same memory but no bonding interaction;
- active crisis with each intervention path;
- no resource crisis;
- repeated completion event;
- save/load mid-quest.

Assertions:
- one quest trigger per eligibility episode;
- no duplicate active personal recovery objective if policy forbids it;
- quest state owned by `PersonalQuestSystem`;
- completion produces one milestone;
- mental-health system determines token recovery;
- Stress Floor recalculates from remaining unresolved tokens;
- HeirloomConsolation cannot alter base floor directly;
- temporary consolation expires deterministically;
- Shared Grief does nothing without interaction;
- Shared Grief relation event exactly once after interaction;
- sedation requires medical validation and inventory;
- medication consumed exactly once;
- vigil reserves companion exactly once;
- no schedule overlap;
- low-stimulation action requires valid room;
- trait milestone does not grant duplicate traits;
- repeated trauma cycling cannot farm trait;
- same seed yields same crisis/therapy outcomes.

## 15.3 Crisis Monte Carlo

Minimum CI-fast sweep: 100 seeds.
Recommended nightly: 1,000–10,000 if runtime permits.

Input dimensions:
- 1/3/5 trauma tokens;
- stress floor low/medium/high;
- current stress 750/900/950/1000;
- zero/one/many counselors;
- medication present/absent;
- quiet room present/full/missing;
- companion present/unavailable;
- hunger/fatigue states;
- active injury/illness;
- positive/negative relations;
- save/reload mid-crisis.

Metrics:
```text
crisis_trigger_day
crisis_duration
intervention availability
therapy success/failure
stress trajectory
floor trajectory
work incapacity duration
inventory consumed
schedule hours reserved
unpreventable death count
invalid stress count
negative duration count
duplicate event count
```

Hard assertion:
`unpreventable_death_count_from_new_task_paths == 0`.

## 15.4 Audio tests

### Mathematical sweep
For permille 0..1000:
- finite dB;
- monotonic;
- clamped;
- exact 0 handling.

### Ducking
- one event;
- nested equal priority;
- nested different priority;
- interrupted release;
- event ends out of order;
- ambient target changes while ducked.

### Geiger
- lambda monotonic;
- zero safe;
- saturation bounded;
- inter-arrivals positive;
- seeded fixture reproducible;
- audio event does not mutate radiation source.

### Low-pass
- generator room;
- living quarters;
- rapid room transition;
- filter cached;
- Events unaffected.

### Headless
- every cue;
- every bus target;
- ducking;
- filter;
- Geiger;
- no Godot API invocation.

### Allocation
After warmup:
- snapshot evaluation 100k iterations;
- target zero managed allocations;
- no Gen2 collection;
- report p50/p95.

## 15.5 Archive UI tests

### Contract
- scene loads;
- script binds;
- `IContractPanel`;
- required node dictionary;
- no missing node paths;
- bind/unbind/rebind.

### Solvent
- enough inventory;
- no inventory;
- wrong chemical;
- already treated;
- race: stock disappears before commit;
- duplicate intent;
- save during operation.

### Assignment
- high-skill available;
- high-skill busy;
- low-skill ineligible;
- medically blocked;
- already counselor;
- already vigil companion;
- currently sleeping;
- assignment release.

### Progress
- 0%;
- stage boundary;
- 99%;
- 100%;
- no UI-driven progression;
- fragment visibility.

### Reward
- archive reward event;
- research unlock;
- TechTree unlock;
- notification;
- reopen no duplicate;
- restore no duplicate.

### Transcript
- no audio;
- audio available;
- play/pause;
- waveform metadata missing fallback;
- transcript text;
- localization.

---

# 16. Content Acceptance & Reachability

## 16.1 Mental-health recovery action ladder

```text
DISCOVERED
LOADED
REGISTERED
TRAUMA_COMPATIBLE
ROOM_REQUIREMENT_RESOLVED
ITEM_REQUIREMENT_RESOLVED
COUNSELOR_POLICY_RESOLVED
UI_PRESENTED
COMMAND_REACHABLE
RECOVERY_EFFECT_OBSERVED
```

No recovery action may ship if it dies before `COMMAND_REACHABLE`.

## 16.2 Catharsis ladder

```text
BREAKTHROUGH_DISCOVERED
TRAUMA_MATCHED
QUEST_TEMPLATE_RESOLVED
OBJECTIVES_REACHABLE
QUEST_CREATED
QUEST_COMPLETED
MILESTONE_EMITTED
MENTAL_HEALTH_EVALUATED
RECOVERY_STATE_CHANGED_OR_EXPLICITLY_NOT_CHANGED
```

## 16.3 Audio cue ladder

```text
CUE_DISCOVERED
CATALOG_VALIDATED
ASSET_PATH_RESOLVED
FILE_EXISTS
IMPORT_VALID
BUS_RESOLVED
DIRECTOR_REQUEST_REACHABLE
BRIDGE_PLAY_REACHABLE
HEADLESS_SAFE
```

## 16.4 Archive action ladder

```text
ARCHIVE_DISCOVERED
PANEL_BINDABLE
STATE_PRESENTED
ACTION_ENABLED
INTENT_SUBMITTED
CANONICAL_PRECONDITION_REVALIDATED
TRANSACTION_COMMITTED
EVENT_EMITTED
UI_REFRESHED
```

## 16.5 Archive reward ladder

```text
REWARD_DEFINED
DECRYPTION_MILESTONE_REACHED
REWARD_EVENT_EMITTED
RESEARCH_CONSUMER_FOUND
TECHTREE_CONSUMER_FOUND
UNLOCK_COMMITTED
NOTIFICATION_EMITTED
SAVE_ROUNDTRIP_PROVEN
```

Any dead stage is a NO-SHIP defect.

---

# 17. Data-Integrity Rules

Add/extend validators for:

## Mental health
- trauma IDs unique;
- crisis IDs unique;
- recovery action IDs unique;
- every recovery action localization key exists;
- every recovery action room ID resolves;
- every required item ID resolves;
- counselor qualification refs resolve;
- catharsis quest template refs resolve;
- catharsis trauma tags resolve;
- no circular quest dependency.

## Audio
- all 11 IDs unique;
- paths exist;
- extensions valid;
- case exact;
- buses valid;
- placeholders flagged;
- production placeholder policy enforced;
- ducking priority values valid;
- loop metadata valid.

## Archive
- archive IDs unique;
- solvent item refs valid;
- cryptographer requirement refs valid;
- transcript/audio refs valid;
- fragment/codex refs valid;
- reward IDs valid;
- research IDs valid;
- TechTree IDs valid;
- room/workstation refs valid;
- translation keys valid.

All validation output must identify:
- file;
- ID;
- field;
- invalid reference;
- expected authority.

---

# 18. Observability & Diagnostics

Add structured diagnostics, debug-only where appropriate.

## 18.1 Mental health UI trace

```text
survivor_id
mental_health_revision
stress_permille
floor_permille
trauma_count
crisis_id
recovery_action_count
eligible_counselor_count
quiet_room_eligibility
```

Do not log sensitive narrative prose unnecessarily.

## 18.2 Catharsis trace

```text
survivor
trauma
breakthrough
quest
eligibility result
milestone
recovery resolution
processed event ID
```

## 18.3 Audio trace

```text
snapshot_revision
bus targets
duck tokens
room filter
radiation lambda
cue dispatch
headless availability
```

## 18.4 Archive trace

```text
archive_id
revision
treatment state
inventory transaction ID
cryptographer assignment
progress stage
reward event
research/tech consumers
```

Debug command output should make it possible to answer:
- why is therapy disabled?
- why is this counselor absent?
- why did Stress Floor not decrease?
- why is machinery still ducked?
- why is Geiger silent?
- why is solvent disabled?
- why did a tech not unlock?

---

# 19. UI State Machines

## 19.1 Mental-health panel

```text
UNBOUND
  → BOUND
  → PRESENTING
  → COMMAND_PENDING
  → PRESENTING
  → UNBOUND
```

Command pending:
- disable duplicate submission;
- retain cancellation semantics if command supports;
- refresh after commit/failure.

## 19.2 Crisis modal

```text
CLOSED
→ ACTIVE_CRISIS
→ OPTION_SELECTED
→ VALIDATING
→ COMMITTED / REJECTED
→ ACTIVE_CRISIS or RESOLVED
```

Never close permanently before canonical result.

## 19.3 Archive panel

```text
UNBOUND
→ BOUND_IDLE
→ ACTION_PENDING
→ BOUND_IDLE
→ UNBOUND
```

Transcript playback is a presentation substate and cannot block canonical archive updates.

---

# 20. Audio Routing Diagram

```text
ShelterAcousticDirector (Core)
        │
        ▼ immutable snapshot / cue intents
ShelterAcousticBridge (Godot host)
        │
        ├──────── Master
        │           │
        │           ├── Machinery
        │           │     └── cached low-pass effect
        │           │
        │           ├── Atmosphere
        │           │     └── shelter bed / despair layer
        │           │
        │           ├── Geiger
        │           │     └── Poisson click player
        │           │
        │           ├── Events
        │           │     └── airlock / radio burst
        │           │
        │           └── Turntable
        │                 └── music / soothing playback
        │
        └── headless → NullAudioRuntime
```

Ducking:
```text
Events priority token
→ DuckController
→ Machinery target -6 dB
→ attack
→ hold
→ release
→ previous Machinery target
```

---

# 21. Mental-Health Recovery Authority Flow

```text
PsychologicalTraumaCatalog
        │ defines recovery actions
        ▼
SurvivorMentalHealthSystem
        │ owns trauma/floor/crisis/recovery truth
        ▼
SurvivorMentalHealthUiAdapter
        │ presents + submits intents
        ▼
AfflictionsPanel / SurvivorDetailPanel
```

Therapy command:
```text
UI
→ adapter
→ mental-health eligibility
→ room/schedule eligibility
→ counselor/medical/inventory eligibility
→ canonical commit
→ semantic event
→ UI revision refresh
```

---

# 22. Catharsis Authority Flow

```text
unresolved trauma
→ mental-health eligibility
→ PersonalQuestSystem
→ canonical quest objectives
→ quest completion
→ catharsis milestone
→ SurvivorMentalHealthSystem
→ recovery decision
→ stress floor recompute
→ JournalSystem / relations / audio observations
```

No direct:
```text
quest_complete → trauma.Remove()
```

---

# 23. Archive Authority Flow

```text
PrewarArchiveDecryptionSystem
        │
        ├── state/progress/condition
        ├── action eligibility
        └── reward readiness
        │
        ▼
ArchiveDecryptionUiAdapter
        │
        ▼
ArchiveDecryptionPanel
```

Solvent:
```text
panel
→ adapter
→ archive intent
→ inventory reservation/consumption
→ archive commit
→ event
```

Assignment:
```text
panel
→ adapter
→ scheduler/assignment authority
→ archive assignee ref
```

Rewards:
```text
archive event
→ ResearchSystem
→ TechTree
→ notification
→ UI
```

---

# 24. Recommended Commit Breakdown

## Task 5
```text
43A-01 authority audit
43A-02 SurvivorMentalHealthUiAdapter
43A-03 trauma token presentation
43A-04 stress bar
43A-05 Stress Floor marker
43A-06 crisis banner
43A-07 therapy option projection
43A-08 room prerequisite bridge
43A-09 counselor candidate bridge
43A-10 quiet-room schedule command
43A-11 scoped quiet-room audio profile
43A-12 SurvivorDetail summary
43A-13 localization
43A-14 accessibility
43A-15 headless selftest
43A-16 UI tests
```

## Task 6
```text
43B-01 catharsis schema
43B-02 six breakthrough definitions
43B-03 quest eligibility bridge
43B-04 quest completion milestone
43B-05 HeirloomConsolation
43B-06 SharedGrief event
43B-07 crisis modal contract
43B-08 sedation medical transaction
43B-09 empathetic vigil schedule
43B-10 low-stimulation action
43B-11 HardenedVeteran disposition
43B-12 despair acoustic observation
43B-13 Journal breakthrough event
43B-14 save linkage/idempotence
43B-15 deterministic seed sweep
43B-16 authority-map docs
```

## Task 7
```text
43C-01 cue/asset inventory
43C-02 missing-asset disposition
43C-03 Godot bus layout
43C-04 acoustic snapshot contract
43C-05 permille→dB conversion
43C-06 smoothing
43C-07 duck controller
43C-08 Geiger Poisson scheduler
43C-09 room low-pass
43C-10 10Hz ambient reconciliation
43C-11 headless audio runtime
43C-12 hot-path allocation cleanup
43C-13 catalog path integrity
43C-14 headless smoke tests
43C-15 director math/performance tests
43C-16 scene lint
43C-17 acoustic docs
```

## Task 8
```text
43D-01 archive UI authority audit
43D-02 ArchiveDecryptionUiAdapter
43D-03 panel scene shell
43D-04 reel condition UI
43D-05 solvent transaction
43D-06 residue animation
43D-07 cryptographer candidate query
43D-08 scheduler assignment
43D-09 progress/fragments
43D-10 transcript playback
43D-11 waveform cache
43D-12 research reward bridge
43D-13 TechTree unlock bridge
43D-14 acoustic cue integration
43D-15 Main.UiPanels registration
43D-16 room inspection wiring
43D-17 localization
43D-18 contract tests
43D-19 scene lint/accessibility
43D-20 authority-map docs
```

## Final hardening
```text
43E-01 restore-order audit
43E-02 exactly-once audit
43E-03 shared schedule exclusivity
43E-04 no-hardcoded-string gate
43E-05 headless aggregate regression
43E-06 allocation/performance budgets
43E-07 100/1000-seed crisis sweep
43E-08 content reachability
43E-09 data-integrity
43E-10 verify-fast
43E-11 final SHIP/NO-SHIP report
```

---

# 25. Risk Register

## R43.1 UI becomes mental-health authority
**Mitigation:** adapter-only presentation and command submission; source-scan direct writes.

## R43.2 Catharsis bypasses recovery model
**Mitigation:** completion emits milestone only.

## R43.3 Heirloom falsifies Stress Floor
**Mitigation:** acute/temporary consolation separated from base trauma floor.

## R43.4 Hardened Veteran creates trauma-farming meta
**Mitigation:** trait-owned, contextual, bounded, gated.

## R43.5 Sedation bypasses medical safety
**Mitigation:** medical eligibility + inventory transaction.

## R43.6 Shared Grief farms affinity
**Mitigation:** real social interaction + stable event + cooldown.

## R43.7 Audio implementation leaks Godot into Core
**Mitigation:** immutable snapshot + runtime abstraction.

## R43.8 Headless audio crashes
**Mitigation:** null/unavailable runtime and explicit smoke test.

## R43.9 Log mapping invalid at zero
**Mitigation:** epsilon/mute floor and full sweep.

## R43.10 Ducking state sticks or stacks unexpectedly
**Mitigation:** tokenized priority controller + order-independent tests.

## R43.11 Geiger click scheduling allocates
**Mitigation:** numeric scheduler state, no per-click object.

## R43.12 Audio cue paths drift
**Mitigation:** catalog integrity and exact filesystem-case validation.

## R43.13 Waveform visualization becomes a hot loop
**Mitigation:** cached/precomputed metadata.

## R43.14 Solvent double-consumption
**Mitigation:** transactional stable operation ID.

## R43.15 Archive UI directly unlocks tech
**Mitigation:** reward event → Research/TechTree.

## R43.16 Cross-task double booking
**Mitigation:** single scheduler authority.

## R43.17 Restore replays side effects
**Mitigation:** processed IDs and side-effect-free restore.

## R43.18 Localization hardcodes spread through `.cs/.tscn`
**Mitigation:** catalog integrity + source scan.

---

# 26. Exhaustive Acceptance Checklist

## Task 5 — Mental-health UI

- [ ] `AfflictionsPanel.cs` inspected before edit
- [ ] `SurvivorDetailPanel.cs` inspected before edit
- [ ] mental-health authority documented
- [ ] stress authority documented
- [ ] `PsychologicalTraumaCatalog` documented
- [ ] `SurvivorMentalHealthUiAdapter` exists
- [ ] adapter owns presentation projection
- [ ] adapter owns eligibility queries
- [ ] adapter owns command submission boundary
- [ ] no UI direct trauma mutation
- [ ] no UI direct stress mutation
- [ ] psychological section added
- [ ] trauma icons catalog/resource-driven
- [ ] combat-shock icon resolves
- [ ] survivor-guilt icon resolves
- [ ] trauma title localized
- [ ] trauma description localized
- [ ] trauma instructions localized
- [ ] token ordering deterministic
- [ ] stress gauge renders 0
- [ ] stress gauge renders 400
- [ ] stress gauge renders 401
- [ ] stress gauge renders 749
- [ ] stress gauge renders 750
- [ ] stress gauge renders 1000
- [ ] Green/Amber/Red theme accessible
- [ ] state has non-color label
- [ ] numeric permille visible
- [ ] threshold ticks visible
- [ ] Stress Floor read from mental-health authority
- [ ] Stress Floor marker visible
- [ ] Stress Floor numeric value visible
- [ ] floor explanation localized
- [ ] floor detail can show source traumas if supported
- [ ] crisis banner canonical
- [ ] crisis title localized
- [ ] crisis duration canonical
- [ ] crisis work penalty not recalculated in UI
- [ ] therapy dropdown from `recovery_actions`
- [ ] incompatible therapy filtered
- [ ] unavailable therapy can remain visible/disabled
- [ ] room prerequisites adapter-driven
- [ ] item prerequisites adapter-driven
- [ ] counselor prerequisites adapter-driven
- [ ] schedule prerequisites adapter-driven
- [ ] missing-room reason localized
- [ ] counselor list canonical
- [ ] empathy qualification canonical
- [ ] medical qualification canonical
- [ ] counseling specialization supported if real
- [ ] counselor duty conflict checked
- [ ] counselor illness/fatigue checked
- [ ] quiet-room quick action uses schedule command
- [ ] room capacity checked
- [ ] schedule overlap checked
- [ ] survivor refusal supported if applicable
- [ ] duplicate command prevented
- [ ] quiet-room audio profile scoped
- [ ] high frequencies attenuated only in presentation
- [ ] alarms exempt
- [ ] audio profile released on close
- [ ] audio profile released on survivor switch
- [ ] audio profile released when therapy ends
- [ ] SurvivorDetail summary added
- [ ] summary does not duplicate full panel
- [ ] mental-health selftest added
- [ ] no-render headless path
- [ ] `AfflictionsPanelMentalHealthTests.cs`
- [ ] translation coverage verified
- [ ] accessibility report updated

## Task 6 — Catharsis

- [ ] six breakthrough definitions exist
- [ ] stable breakthrough IDs
- [ ] trauma tags valid
- [ ] quest refs valid
- [ ] localization refs valid
- [ ] cooldowns defined
- [ ] no circular objective
- [ ] high stress alone not enough
- [ ] unresolved trauma required
- [ ] quest limits respected
- [ ] required location reachable
- [ ] required item reachable
- [ ] required survivor reachable
- [ ] PersonalQuestSystem owns lifecycle
- [ ] mental health stores linkage only
- [ ] completion event stable
- [ ] completion processed once
- [ ] catharsis milestone emitted
- [ ] mental health decides recovery
- [ ] partial recovery supported
- [ ] full resolution supported
- [ ] failure/no-change supported
- [ ] Stress Floor recomputed from unresolved tokens
- [ ] no direct quest trauma deletion
- [ ] HeirloomConsolation uses canonical item instance
- [ ] ownership/relevance checked
- [ ] current stress relief canonical
- [ ] base Stress Floor not directly lowered
- [ ] temporary effective-floor offset only if explicitly designed
- [ ] temporary relief expiry deterministic
- [ ] Shared Grief shares same deceased source
- [ ] both survivors know loss
- [ ] actual interaction required
- [ ] relationship delta delegated
- [ ] no passive affinity tick
- [ ] cooldown/idempotence
- [ ] crisis modal canonical
- [ ] 950 threshold reconciled with Core
- [ ] sedation requires drug
- [ ] sedation checks contraindications
- [ ] sedation inventory transaction atomic
- [ ] sedation does not directly clear crisis in UI
- [ ] vigil companion eligibility
- [ ] vigil schedule reservation
- [ ] vigil duration canonical
- [ ] no double booking
- [ ] low-stimulation intervention safe
- [ ] unsupported sensory deprivation rejected/reworked
- [ ] no-unpreventable-death invariant
- [ ] 100-seed minimum
- [ ] 1000-seed nightly considered
- [ ] Hardened Veteran not mental-health-owned
- [ ] trait acquisition gated
- [ ] no blanket global -20% stress unless separately justified
- [ ] no trauma farming
- [ ] despair ambience aggregate only
- [ ] no audio→stress feedback
- [ ] JournalSystem owns entries
- [ ] diary localization template
- [ ] no diary prose in mental-health save
- [ ] catharsis tests
- [ ] seed determinism
- [ ] SaveSectionRegistry audited
- [ ] save roundtrip
- [ ] catalog integrity
- [ ] authority map updated

## Task 7 — Audio

- [ ] `assets/audio/` inventoried
- [ ] 11 catalog cue IDs counted
- [ ] all 11 mapped
- [ ] missing assets identified
- [ ] placeholder policy explicit
- [ ] hydraulic groan resolved
- [ ] radiation tick resolved
- [ ] airlock pressurization resolved
- [ ] generator hum resolved
- [ ] Master bus
- [ ] Machinery bus
- [ ] Atmosphere bus
- [ ] Geiger bus
- [ ] Events bus
- [ ] Turntable bus
- [ ] bus names resolved/cached
- [ ] no numeric bus hardcodes
- [ ] Core snapshot Godot-independent
- [ ] bridge owns Godot audio calls
- [ ] 0 permille safe mute
- [ ] 1 permille tested
- [ ] 10 permille tested
- [ ] 100 permille tested
- [ ] 250 permille tested
- [ ] 500 permille tested
- [ ] 750 permille tested
- [ ] 1000 permille tested
- [ ] mapping logarithmic/perceptual
- [ ] no NaN
- [ ] smoothing documented
- [ ] no zipper noise
- [ ] airlock cue duck rule
- [ ] -6 dB machinery duck
- [ ] attack defined
- [ ] release defined
- [ ] overlap defined
- [ ] no accidental cumulative duck
- [ ] restore previous target
- [ ] Geiger radiation source canonical
- [ ] radiation→lambda monotonic
- [ ] lambda bounded
- [ ] zero safe
- [ ] Poisson inter-arrival implementation
- [ ] presentation RNG separate
- [ ] no simulation feedback
- [ ] room context canonical
- [ ] Machinery low-pass only
- [ ] Events unaffected
- [ ] low-pass effect cached
- [ ] filter transition smooth
- [ ] ambient reconciliation ~10Hz
- [ ] urgent cue immediate
- [ ] `IAudioRuntime` or equivalent
- [ ] headless no-op runtime
- [ ] no `AudioServer` in Core
- [ ] headless smoke tests
- [ ] steady hot path zero allocations
- [ ] bus indices cached
- [ ] cue lookups cached
- [ ] no per-frame strings
- [ ] no per-click object allocation
- [ ] audio math tests
- [ ] ducking tests
- [ ] Geiger tests
- [ ] low-pass tests
- [ ] path integrity
- [ ] case-sensitive path validation
- [ ] bus validation
- [ ] placeholder validation
- [ ] scene lint
- [ ] acoustic authority map
- [ ] routing diagram
- [ ] attenuation curves

## Task 8 — Archive UI

- [ ] existing panels inspected
- [ ] `ArchiveDecryptionPanel.cs`
- [ ] inherits `PanelContainer`
- [ ] implements `IContractPanel`
- [ ] proper bind/unbind
- [ ] proper unsubscribe
- [ ] `ArchiveDecryptionPanel.tscn`
- [ ] 1920×1080 contract
- [ ] container/anchor layout
- [ ] text scale safe
- [ ] `ArchiveDecryptionUiAdapter`
- [ ] immutable presentation model
- [ ] spool condition system-owned
- [ ] oxide degradation system-owned
- [ ] encryption strength system-owned
- [ ] semantic bands localized
- [ ] Apply Solvent button
- [ ] solvent eligibility canonical
- [ ] inventory port used
- [ ] transaction revalidates
- [ ] no double consumption
- [ ] residue animation presentation-only
- [ ] candidate query canonical
- [ ] no raw intelligence threshold in panel
- [ ] skill/specialization policy canonical
- [ ] fatigue/health checked
- [ ] schedule availability checked
- [ ] assignment commits via scheduler
- [ ] no schedule duplicate
- [ ] progress system-owned
- [ ] no UI ticks
- [ ] fragment reveal canonical
- [ ] locked lore not leaked
- [ ] transcript asset canonical
- [ ] playback routed through audio architecture
- [ ] waveform precomputed/cached
- [ ] no per-frame FFT
- [ ] transcript accessible text
- [ ] ResearchSystem owns research unlock
- [ ] TechTree owns tech unlock
- [ ] reward event stable
- [ ] unlock exactly once
- [ ] no UI unlock at 100%
- [ ] radio burst via acoustic director
- [ ] tape whir via catalog if authored
- [ ] panel registered once
- [ ] no duplicate subscriptions
- [ ] room IDs verified
- [ ] selected archive context passed
- [ ] contract tests
- [ ] inventory race test
- [ ] assignment conflict test
- [ ] reward replay test
- [ ] scene lint
- [ ] all strings localized
- [ ] authority map updated

## Cross-system

- [ ] one schedule authority
- [ ] therapy and decryption cannot overlap illegally
- [ ] counselor and cryptographer cannot overlap illegally
- [ ] vigil and work cannot overlap illegally
- [ ] restore side-effect free
- [ ] processed IDs stable
- [ ] no GUID
- [ ] no wall clock
- [ ] no unseeded simulation RNG
- [ ] headless aggregate regression
- [ ] content acceptance
- [ ] data integrity
- [ ] accessibility
- [ ] `verify-fast`


---

# 27. SHIP / NO-SHIP Gate

**SHIP** only if:

```text
mental_health_authorities == 1
AND current_stress_authorities == 1
AND therapy_definition_authorities == 1
AND schedule_authorities == 1
AND quest_lifecycle_authorities == 1
AND relationship_authorities == 1
AND personal_item_authorities == 1
AND trait_authorities == 1
AND medical_pharmaceutical_authorities == 1
AND journal_authorities == 1
AND acoustic_core_authorities == 1
AND godot_audio_runtime_authorities == 1
AND radiation_authorities == 1
AND archive_decryption_authorities == 1
AND inventory_authorities == 1
AND research_authorities == 1
AND tech_tree_authorities == 1

AND ui_direct_trauma_mutations == 0
AND ui_direct_stress_mutations == 0
AND ui_direct_schedule_collection_mutations == 0
AND ui_hardcoded_recovery_rules == 0
AND ui_hardcoded_room_prerequisites == 0
AND ui_hardcoded_counselor_thresholds == 0
AND keepsake_direct_base_stress_floor_mutations == 0
AND quest_direct_trauma_deletions == 0
AND shared_grief_passive_affinity_ticks == 0
AND direct_global_hardened_veteran_stress_immunity == false
AND sedation_without_medical_validation == false
AND crisis_intervention_inventory_bypasses == 0
AND crisis_intervention_schedule_bypasses == 0
AND crisis_unpreventable_new_death_paths == 0

AND core_godot_audio_calls == 0
AND headless_audio_server_calls == 0
AND invalid_audio_catalog_paths == 0
AND unresolved_production_audio_placeholders == 0
AND audio_hot_loop_allocations == 0
AND unsafe_log10_zero_paths == 0
AND unintended_cumulative_ducking == 0
AND geiger_audio_affects_simulation == false
AND per_frame_bus_effect_allocations == 0
AND per_frame_audio_catalog_lookups == 0

AND archive_ui_direct_inventory_removals == 0
AND archive_ui_direct_archive_state_mutations == 0
AND archive_ui_direct_tech_unlocks == 0
AND archive_ui_direct_research_unlocks == 0
AND duplicate_archive_reward_replays == 0
AND archive_panel_duplicate_subscriptions == 0
AND cryptographer_schedule_double_bookings == 0
AND per_frame_waveform_analysis == 0
AND hardcoded_player_facing_strings == 0

AND afflictions_mental_health_selftest == pass
AND survivor_catharsis_tests == pass
AND crisis_seed_sweep == pass
AND mental_health_save_roundtrip == pass
AND acoustic_headless_smoke == pass
AND audio_director_tests == pass
AND audio_asset_integrity == pass
AND audio_zero_allocation_budget == pass
AND archive_panel_contract_tests == pass
AND archive_inventory_atomicity == pass
AND archive_reward_exactly_once == pass
AND archive_save_roundtrip == pass
AND shared_schedule_exclusivity == pass
AND scene_lint == pass
AND catalog_integrity == pass
AND determinism_fingerprints == pass
AND accessibility_review == pass
AND data_integrity_selftest == pass
AND content_acceptance_gate == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 28. Implementer Handoff

1. Start by writing the authority matrix. Do not open `AfflictionsPanel.cs` and begin adding gameplay logic before the ownership seams are proven.
2. Build `SurvivorMentalHealthUiAdapter` first. Both `AfflictionsPanel` and `SurvivorDetailPanel` should consume presentation DTOs rather than directly walking mental-health internals.
3. Render current stress and trauma Stress Floor as separate concepts. The Stress Floor must always come from the mental-health authority.
4. Preserve the requested 0–400 / 401–749 / 750–1000 stress bands, but add semantic labels so color is never the only indicator.
5. Pull trauma titles, descriptions, recovery actions, therapy instructions, and prerequisites from catalogs/localization rather than hardcoded UI text.
6. Query room prerequisites through recovery-action metadata. If `room_bunks` or `room_main` is not the current canonical ID, use repository truth.
7. Query counselor eligibility through a service/policy. Do not embed raw empathy/medical thresholds in panel code.
8. Make `Send to Quiet Room` a canonical schedule/therapy transaction with capacity, overlap, and autonomy checks.
9. Scope the soothing low-pass effect to the viewer’s panel/listening context. It must clean up reliably and never suppress alarms.
10. Keep `SurvivorDetailPanel` compact; deep-link to the full mental-health section instead of cloning it.
11. Implement catharsis as a PersonalQuest integration, not an alternate trauma state machine.
12. Add six data-driven catharsis definitions with reachable prerequisites and stable IDs.
13. On quest completion, emit exactly one catharsis milestone to `SurvivorMentalHealthSystem`.
14. Let mental health decide whether the affected trauma resolves, partially improves, or remains.
15. Recompute Stress Floor from remaining unresolved trauma only.
16. Treat `HeirloomConsolation` as current-stress/temporary recovery support over a Plan-210 canonical item. Never silently rewrite the base trauma floor.
17. Require a real Shared Grief social interaction before any affinity consequence.
18. Let `SurvivorRelationsSystem` own the affinity change.
19. Drive the crisis modal from canonical crisis state, not from a UI-local `stress >= 950` check alone.
20. Make Emergency Sedation a medical/pharmaceutical transaction with contraindication and inventory validation.
21. Make Empathetic Vigil reserve real companion time through the scheduler.
22. Reframe/gate Sensory Deprivation unless it is already a validated recovery action. Prefer safe low-stimulation observation semantics.
23. Gate `Hardened Veteran`. If it ships, TraitSystem owns it, and it should be contextual rather than a permanent global 20% stress shield.
24. Run 100 seeds in the normal gate and a larger nightly sweep to prove no newly introduced crisis branch creates arbitrary unavoidable death.
25. Feed only aggregate despair/grief bands to `ShelterAcousticDirector`, never individual diagnosis state unless the design explicitly exposes it.
26. Prevent audio ambience from feeding back into stress.
27. Inventory every existing audio file and every one of the 11 cue IDs before generating assets.
28. Produce a cue→path→bus→priority mapping document.
29. Resolve missing audio through reuse, explicit placeholder tracking, or the project’s asset-generation pipeline.
30. Create the six named buses and resolve them by name once.
31. Keep all Godot-specific audio calls in `ShelterAcousticBridge` or a host runtime abstraction.
32. Use safe logarithmic permille→dB conversion with a mute floor.
33. Define ducking attack/release and overlap behavior so event nesting cannot accumulate accidental -6 dB steps.
34. Bind Geiger rate to radiation through a bounded Poisson process that is presentation-only and frame-rate independent.
35. Cache the Machinery low-pass effect and smoothly modulate it from canonical room/camera context.
36. Throttle ambient snapshot reconciliation to about 10 Hz while letting urgent event cues dispatch immediately.
37. Make the steady acoustic path allocation-free after warmup.
38. Prove headless safety with explicit no-audio-runtime tests.
39. Add filesystem-path validation for every production cue.
40. Build `ArchiveDecryptionPanel` using current `IContractPanel` lifecycle patterns.
41. Use containers/anchors even under the 1920×1080 contract so text scaling remains viable.
42. Add `ArchiveDecryptionUiAdapter`; UI should receive immutable archive presentation state.
43. Make Apply Solvent a single atomic action spanning archive eligibility and `IPlayerInventoryPort` consumption.
44. Treat residue-removal animation as visual interpolation after canonical state commit.
45. Get cryptographer candidates through canonical qualification/schedule policy rather than hardcoded intelligence checks.
46. Use the same scheduler used by therapy/vigil work so the same survivor cannot be double-booked.
47. Never tick decryption from UI.
48. Reveal only canonical deciphered fragments.
49. Cache/precompute waveform data; transcript text is the accessible source of content.
50. Route archive sound through `ShelterAcousticDirector`.
51. Emit archive reward-ready events from the decryption system and let `ResearchSystem`/`TechTree` commit unlocks.
52. Ensure UI only displays a committed unlock notification.
53. Register the panel exactly once in `Main.UiPanels.cs`.
54. Verify the actual room IDs before wiring `room_reading_quiet_room` / `room_library`.
55. Make panel rebind/unbind idempotent.
56. Ensure all restore paths are side-effect free.
57. Persist references/linkage rather than duplicating quest, inventory, schedule, relation, research, tech, journal, or audio state.
58. Run focused tests, selftests, scene lint, catalog integrity, seed sweeps, allocation budgets, save/load roundtrips, and `verify-fast`.
59. Do not close any task because a UI screenshot “looks right.” Close only when canonical state and exactly-once behavior are proven.
60. Produce a final SHIP/NO-SHIP report containing the proving command output and unresolved deferrals.

---

# 29. Final Outcome

When this plan is complete, the four follow-up tasks stop being isolated feature requests and become one coherent integration layer.

A survivor’s psychological state will finally be visible where the player already manages injuries and health.

The player will see the trauma tokens that matter, the current stress value, the minimum Stress Floor imposed by unresolved trauma, active crisis state, and the therapies that are genuinely available. The interface will explain why a therapy is unavailable—missing room, unavailable counselor, conflicting schedule, missing medicine—without duplicating any of those rules inside the panel itself.

The player will be able to send a survivor to a quiet room, but that action will reserve a real room and real survivor time.

They will be able to assign a counselor, but that counselor will be an actually eligible survivor who is not simultaneously on guard duty, asleep, hospitalized, decrypting an archive, or already committed to another intervention.

The quiet-room view can sound softer and more intimate, but that audio filter will remain a viewer-side presentation effect. It will not secretly make therapy more successful, lower stress, or mask emergency alarms.

Catharsis will become a real progression rail rather than a shortcut.

A survivor may recover a personal relic and find temporary comfort in it because that relic is a canonical Plan-210 object with real ownership and history. They may share grief with another survivor because both remember the same dead squadmate and actually spend time together. They may complete a personal recovery objective because `PersonalQuestSystem` generated a reachable quest from a real unresolved trauma.

But the quest never owns the trauma.

When the objective is complete, `SurvivorMentalHealthSystem` receives a milestone and decides what recovery means. A trauma may resolve. It may soften. It may remain but contribute less. The Stress Floor changes only because the authoritative trauma state changed.

That preserves the meaning of the model.

Acute breakdowns also become playable without becoming arbitrary punishment.

When a severe crisis occurs, the player can choose among interventions that are actually available. Sedation requires real pharmaceuticals and medical eligibility. A 24-hour empathetic vigil consumes another survivor’s time. Quiet low-stimulation observation requires a suitable room. The interface cannot conjure any of these resources.

The seed-sweep gates ensure that these new crisis branches do not create unfair unavoidable death just because a random roll occurred.

The acoustic layer then makes all of this feel embedded in the shelter.

The shelter audio catalog will correspond to real files. A generator will have a known Machinery-bus loop. Airlock cycling will arrive on the Events bus and duck the machinery bed by a controlled six decibels. Radiation will become audible through a bounded Poisson Geiger process that represents the real ambient radiation value without altering it.

Moving from the generator room to living quarters can gradually roll off harsh machinery frequencies because the camera/player room context changed.

A shelter suffering chronic despair can gain a subtle low-frequency atmosphere layer because the mental-health system exported an aggregate ambiance observation.

None of those sounds becomes a new simulation rule.

The Core director says what should be heard.
The Godot bridge decides how to render it.
The headless server safely does nothing.

The archive system gains the same discipline.

A player can inspect an old reel, see its oxide damage and encryption strength, and decide whether to spend scarce chemicals restoring it. Clicking Apply Solvent does not instantly subtract an item and hope the backend agrees. The action is revalidated and committed atomically through the inventory and archive authorities.

A cryptographer can be assigned only if the survivor is actually qualified and available.

Progress advances because `PrewarArchiveDecryptionSystem` advanced it, not because a Godot progress bar ran `_Process`.

Decoded fragments become visible only when the archive authority exposes them.

A prewar audio recording can play with a waveform visualization, but the waveform remains a cached presentation aid and the transcript remains the accessible textual truth.

Most importantly, technological rewards stay transactional and canonical.

The archive system declares a reward ready.
`ResearchSystem` and `TechTree` commit it.
The UI then tells the player what unlocked.

Reloading does not unlock it again.

The same architectural principle now holds across all four tasks:

- trauma truth stays in mental health;
- quest truth stays in PersonalQuest;
- relationship truth stays in SurvivorRelations;
- item truth stays in Inventory/Plan 210;
- medical truth stays in medical systems;
- schedule truth stays in one scheduler;
- sound simulation intent stays in the acoustic director;
- Godot audio stays in the bridge;
- archive truth stays in the archive system;
- research truth stays in Research/TechTree;
- the UI stays thin.

The result is not merely four features that compile.

It is a coordinated slice of ASHFALL where psychological recovery, shelter scheduling, sound design, survivor availability, personal keepsakes, archival research, and UI presentation all interoperate without creating duplicate truth.

That is the shipping standard for this bundle.
