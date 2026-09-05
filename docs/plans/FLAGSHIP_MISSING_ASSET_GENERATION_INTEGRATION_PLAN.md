# 1. Objective

Deliver a release-grade ASHFALL asset library through one flagship, dependency-ordered production program, with audio as the first and highest-priority workstream.

The program must:

- make the current 150-source audio library safe, loadable, correctly wired, and consistently mastered;
- close every confirmed live audio silence before adding speculative cues;
- establish one authoritative, deterministic audio cue manifest;
- generate expansion, combat, shelter, weather, radio, UI, and location sound families from runtime demand;
- raise visual resolution from 512/1,295 catalog IDs to complete runtime-reachable coverage, then close the remaining catalog backlog;
- replace cross-domain placeholder collisions with semantically correct original art;
- validate actual import, decoding, playback, semantic fit, and player-visible use rather than file presence alone;
- preserve ASHFALL’s original visual and sonic identity and avoid copying another game’s art, characters, UI, names, writing, or audio.

Plan date: 2026-09-03

Repository evidence:

- forensic audit anchor: c09c3e67a6e88920690767899b586ab85fecb84c;
- live planning revision observed: 32edc1a80fddf7b77cff487393e77807599f9be9;
- the worktree was actively changing during both audits and planning, so Phase 0 must establish a stable execution baseline.

# 2. Current Reality

## Audio

ASHFALL currently has:

- 150 independently decodable audio sources;
- 82 WAV, 63 MP3, and 5 OGG files;
- 501.532 seconds of source material;
- 145 runtime cue registrations: 144 hardcoded registrations plus bio_mutation_pulse loaded from JSON;
- 12 configured audio buses;
- a passing 490/490 audio self-test;
- five generation scripts plus the original audio generator and tactile UI generator;
- a detailed but aspirational 20-task audio roadmap.

The current audio library is not release-ready:

- ExpansionAudioBridge subscriptions are added every frame and never removed.
- Three live expansion events are silent.
- AudioManager’s direct WAV fallback assigns a complete RIFF file to a raw PCM field.
- All 52 currently untracked generated WAVs reach exactly 0.000 dBFS sample peak.
- 69 of 150 sources exceed 0 dBTP.
- 21 sources have absolute DC offset at or above 0.01.
- Eight tracked alert/weather WAVs have severe positive DC bias and true-peak overs.
- Godot’s full import scan crashes with exit 134 in the observed snapshot.
- AudioSelfTest often proves path presence or object type, not correct decoding.
- generate-audio-catalog.py embeds the current date and omits JSON-loaded cues.
- Multiple generation scripts normalize every render to full scale and manufacture .import sidecars.

The key conclusion is that ASHFALL does not primarily need more audio files yet. It first needs a trustworthy audio production and integration pipeline.

## Visual

The production registry currently resolves:

| Category | Catalog IDs | Resolved | Missing | Coverage |
|---|---:|---:|---:|---:|
| Items | 682 | 317 | 365 | 46.48% |
| Portraits | 256 | 108 | 148 | 42.19% |
| Locations | 309 | 50 | 259 | 16.18% |
| Factions | 48 | 37 | 11 | 77.08% |
| Total | 1,295 | 512 | 783 | 39.54% |

Additional visual reality:

- 3,757 visual source files occupy about 239.3 MiB.
- 1,099 quarantine visuals occupy about 149.2 MiB inside the live assets tree.
- 715 active files are redundant exact copies across 71 hash groups.
- Several unrelated items, portraits, and locations resolve to identical flat placeholders.
- assets/sprites/Map/marker_safe.png is base64 text rather than a valid PNG.
- the sampled 50-reference asset gate passes while the full 1,295-ID sweep remains report-only;
- existing generation manifests provide 300 actionable visual candidates, but runtime surfacing evidence is incomplete and some helper tooling disagrees with production AssetRegistry behavior.

# 3. Required Delta

The minimum safe delta is a governed asset production pipeline:

CURRENT RUNTIME DEMAND
→ AUTHORITATIVE MANIFEST
→ ORIGINAL GENERATION BRIEF
→ DETERMINISTIC/TRACEABLE GENERATION
→ TECHNICAL MASTERING
→ HUMAN CREATIVE REVIEW
→ GODOT IMPORT
→ REGISTRY/CUE WIRING
→ HEADLESS LOAD TEST
→ PLAYER-SURFACE TEST
→ ACCEPTED ASSET LEDGER

For audio, the delta begins with rehabilitation and cue closure. For visuals, it begins with authoritative missing-ID enumeration and player-priority batching. Neither workstream may measure progress by raw file count.

# 4. Evidence

Primary evidence sources:

- docs/debug/10LOOP_WHOLE_REPOSITORY_ASSET_GAP_AUDIT.md
- docs/audio/AUDIO_QA_REPORT.md
- docs/visual/VISUAL_ASSET_FORENSIC_AUDIT_2026-09-03.md
- docs/audio/AUDIO_CUE_CATALOG.md
- docs/audio/AUDIO_NEXT_20_TASKS.md
- docs/visual/PRODUCTION_ART_GENERATION_MANIFEST.json
- docs/visual/runtime_context_top_ids.json
- src/Audio/AudioCueCatalog.cs
- src/Audio/AudioManager.cs
- src/Audio/ExpansionAudioBridge.cs
- src/Audio/AudioSelfTest.cs
- src/Host/AssetRegistry.cs
- scripts/ci/godot-asset-gate.sh
- scripts/ci/asset-orphan-sweep.sh
- tools/generate_audio.py
- tools/generate_phase2_audio.py through tools/generate_phase5_audio.py
- tools/generate_tactile_ui.py

Critical evidence constraints:

- the five mandatory project checks can pass while full importability, full visual coverage, and live audio closure remain broken;
- godot-asset-gate.sh currently logs an import problem and continues instead of failing the gate;
- visual and content helper tools cannot override production registry behavior;
- no asset is accepted solely because a source file and .import sibling exist.

# 5. Existing Extension Seams

Use and strengthen these seams:

| Seam | Use |
|---|---|
| AudioCueCatalog | Stable cue IDs, parsed cue definitions, lookup, cooldown metadata |
| Assets/StreamingAssets/Data/audio_cues.json | Single authoritative cue metadata catalog |
| AudioManager | Godot playback, buses, caching, one-shots, loops |
| AudioEventBridge | Existing domain-event-to-cue translation |
| ExpansionAudioBridge | Expansion domain-event translation after lifecycle repair |
| AudioStateCoordinator | Presentation-only loop/crossfade state |
| ShelterAudioController | Shelter ambience projection |
| SurfaceAmbienceController | Weather/location ambience projection |
| AssetRegistry | Production visual ID resolution authority |
| PRODUCTION_ART_GENERATION_MANIFEST.json | Visual production batch input after reconciliation |
| runtime_context_top_ids.json | Priority evidence, not resolution authority |
| CatalogIntegrityValidator | Canonical ID/reference validation |
| asset-orphan-sweep.sh | Source/sidecar pair validation |
| Godot import pipeline | The only valid generator of import cache artifacts and final sidecar state |

Do not create a second audio manager, a second visual registry, per-expansion asset loaders, or an asset database outside the existing JSON/registry seams.

# 6. Proposed Architecture

## 6.1 Asset demand registry

Create a generated, reviewable demand manifest for each workstream:

- audio demand is derived from stable cue IDs, domain-event mappings, player actions, loop states, and narrative/radio references;
- visual demand is derived from AssetRegistry’s exact production candidate rules plus catalog IDs and runtime surfacing priority;
- every row has one status: MISSING, PLACEHOLDER, GENERATED_UNREVIEWED, TECH_QA_PASS, CREATIVE_QA_PASS, WIRED, RUNTIME_PROVEN, or EXEMPT_APPROVED;
- every exemption carries an owner, reason, and expiration/review date.

The demand manifests are planning/build artifacts. Runtime authority remains JSON catalogs plus AssetRegistry/AudioCueCatalog.

## 6.2 Audio authority

Migrate all cue metadata to Assets/StreamingAssets/Data/audio_cues.json:

- C# retains stable cue constants and schema/parser types;
- JSON owns resource path, bus, loop flag, gain trim, cooldown, fallback, and optional tags;
- duplicate cue IDs or paths become validation failures;
- documentation is generated from JSON, never from the wall clock;
- missing JSON is an explicit audio-degraded state, not permission to silently restore a duplicate hardcoded catalog.

## 6.3 Audio production toolchain

Replace per-script full-scale normalization and manual sidecar writing with a shared render/master/export library:

1. deterministic synthesis or licensed/original source ingest;
2. high-resolution internal processing;
3. DC removal;
4. fade/edge cleanup;
5. loop-seam construction where required;
6. class-specific loudness and peak control;
7. export to the project delivery format;
8. independent decode and measurement;
9. Godot-generated import;
10. cue and runtime validation.

Existing generators may call the shared library after migration. They must not independently implement normalization or .import content.

## 6.4 Visual production toolchain

Build visual batches from production-resolved missing IDs:

- derive size, aspect ratio, alpha, and import preset from the consuming Godot Control/scene;
- generate original art from ASHFALL’s approved family guide and per-ID semantics;
- retain prompt, seed/model/tool, reference list, date, and reviewer result in the generation ledger;
- run file-signature, decode, dimensions, alpha, perceptual-duplicate, and forbidden-content checks;
- import through Godot;
- prove resolution through AssetRegistry;
- inspect contact sheets and representative in-engine panels.

# 7. Ownership Matrix

| Concern | Authoritative owner |
|---|---|
| Gameplay state and events | Assets/Ashfall.Core domain systems |
| Audio cue metadata | Assets/StreamingAssets/Data/audio_cues.json |
| Stable cue ID constants | src/Audio/AudioCueCatalog.cs |
| Audio playback and buses | src/Audio/AudioManager.cs |
| Domain-to-audio presentation mapping | AudioEventBridge and ExpansionAudioBridge |
| Loop/crossfade presentation state | AudioStateCoordinator and ambience controllers |
| Audio source masters/delivery files | assets/audio |
| Visual source assets | assets/art, assets/sprites, assets/ui |
| Visual runtime resolution | src/Host/AssetRegistry.cs |
| Generation recipes and QA history | docs/audio and docs/visual ledgers |
| Import cache and imported sidecars | Godot importer |
| Technical gates | scripts/ci |
| Final creative acceptance | project owner or assigned audio/art reviewer |

No asset metadata or presentation node may become a gameplay authority.

# 8. Data Flow

## Audio event flow

CORE STATE MUTATION
→ CORE C# EVENT
→ IDEMPOTENT HOST BRIDGE
→ STABLE CUE ID
→ JSON CUE DEFINITION
→ AUDIO MANAGER
→ NAMED BUS
→ IMPORTED AUDIO STREAM
→ PLAYER HEARS FEEDBACK

Audio failure must log the cue ID, resource path, event source, and fallback decision once. It must never mutate Core state or change deterministic simulation outcomes.

## Audio production flow

RUNTIME DEMAND
→ AUDIO GENERATION LEDGER ROW
→ CREATIVE BRIEF
→ GENERATOR/RECORDING
→ MASTERING GATE
→ HUMAN AUDITION
→ DELIVERY WAV/OGG
→ GODOT IMPORT
→ JSON REGISTRATION
→ SELF-TEST
→ EVENT PLAYTHROUGH
→ ACCEPTED

## Visual production flow

CATALOG ID
→ PRODUCTION ASSETREGISTRY MISS
→ RUNTIME PRIORITY
→ VISUAL MANIFEST ROW
→ GENERATION PROMPT
→ TECHNICAL QA
→ CREATIVE QA
→ GODOT IMPORT
→ REGISTRY RESOLUTION
→ PANEL SNAPSHOT
→ ACCEPTED

# 9. State Model

No new gameplay save state is required.

Production ledger row fields:

| Field | Purpose |
|---|---|
| asset_id / cue_id | Stable canonical identity |
| demand_source | Catalog, event, panel, narrative, or system evidence |
| asset_family | Audio class or visual family |
| runtime_priority | P0 through P3 |
| target_path | Exact res:// delivery path |
| generation_method | Procedural, recorded, commissioned, or generated |
| recipe_version | Generator/prompt/tool version |
| seed | Reproducible generation seed when applicable |
| provenance | License/originality and source-reference record |
| tech_qa | Machine validation result |
| creative_qa | Human review result |
| wiring_status | Cue/registry binding state |
| runtime_status | Headless and player-surface proof |
| replacement_of | Prior placeholder or superseded asset |
| reviewer | Acceptance owner |

Lifecycle:

MISSING
→ GENERATED_UNREVIEWED
→ TECH_QA_PASS
→ CREATIVE_QA_PASS
→ WIRED
→ RUNTIME_PROVEN

Any failure returns the row to the earliest failed stage. Rejected outputs stay outside active assets or are moved to an explicitly ignored review area.

# 10. API/Contracts

Proposed minimal contracts:

## Audio cue JSON schema

- schema_version: integer
- cues: array
- id: required snake_case string
- resource_path: required res://assets/audio path
- bus: required existing bus name
- loop: required boolean
- default_volume_db: finite bounded number
- cooldown_seconds: non-negative finite number
- fallback_cue_id: optional existing cue ID
- tags: optional sorted string array
- max_instances: optional positive integer
- fade_in_seconds / fade_out_seconds: optional non-negative values for loops

## Validation APIs

- AudioCueCatalog.LoadFromJson returns a structured result with loaded count, errors, and warnings.
- AudioSelfTest performs real stream loading and format validation.
- An audio asset QA command emits JSON and Markdown from the same measurements.
- AssetRegistry full coverage returns a machine-readable report and supports explicit threshold arguments.
- Visual signature validation distinguishes SVG parser limitations from corrupt raster payloads.

Avoid introducing Core interfaces for audio playback. Core should emit domain events and remain unaware of cues, buses, files, or Godot.

# 11. Data Changes

## Audio

Modify Assets/StreamingAssets/Data/audio_cues.json to hold all accepted cues.

Required migration:

1. export all 144 hardcoded registrations;
2. merge the existing JSON-only bio_mutation_pulse definition;
3. add action_interrogation_slam and hazard_toxic_sizzle using their existing files;
4. add train_screech_crash only after its source passes QA;
5. reconcile the six uncataloged files;
6. identify cue aliases that intentionally share one asset;
7. validate every fallback target and reject cycles;
8. preserve stable cue IDs and resource paths unless a reviewed migration changes them.

Do not create a second audio JSON catalog per expansion.

## Visual

Do not alter gameplay JSON merely to match a convenient filename. Asset names and registry candidates must adapt to canonical IDs.

Regenerate the missing-visual manifest from current catalogs at every batch boundary. The 783-gap figures are the audit baseline, not a permanently hardcoded target.

# 12. Save/Load

Audio and visual generation add no gameplay state.

Preserve:

- existing audio settings persistence;
- campaign envelope formats;
- save section registry;
- checksum behavior;
- old-save compatibility.

Presentation loop state should be reconstructed from current Core/world state after load. Do not serialize active AudioStreamPlayer objects, transient cue cooldown timers, import paths, or visual resource objects.

If cue IDs are renamed, retain aliases until every saved setting, user preference, data reference, and documentation consumer is migrated. Prefer not to rename accepted cue IDs.

# 13. Determinism

Gameplay determinism requirements:

- audio and visual selection cannot consume the simulation RNG stream;
- pitch/volume variation remains presentation-only;
- any variant selection used by snapshot or headless tests uses a separate stable seed;
- replace host uses of string.GetHashCode with a stable ordinal hash when those paths affect simulation;
- sort manifests, cues, paths, and validation output ordinally.

Asset generation reproducibility:

- every procedural generator uses an explicit fixed seed;
- generators record seed and recipe version;
- identical inputs produce byte-identical delivery output where the codec permits;
- generated documentation never embeds a volatile date in drift comparisons;
- visual generation records enough metadata to reproduce or intentionally revise an accepted result.

# 14. System/Event Wiring

Before new audio generation:

1. make AudioEventBridge and ExpansionAudioBridge provider-aware and idempotent;
2. retain stable delegate references;
3. unsubscribe on provider changes and Dispose;
4. bind only when provider identity changes, not once per frame;
5. add maximum-instance and cooldown protection for high-frequency cues;
6. treat loops as desired-state reconciliation rather than repeated Play calls;
7. make failures one-shot logged and observable to self-tests.

Confirmed cue closure:

| Domain event | Required cue | Action |
|---|---|---|
| DesperationSystem.OnTabooBroken | action_interrogation_slam | Register existing source |
| MutationSystem.OnMutationAcquired | bio_mutation_pulse | Preserve JSON registration |
| ChemWarfareSystem.OnHazardDeployed | hazard_toxic_sizzle | Register existing source |
| RailwaySystem.OnDerailment | train_screech_crash | Generate, master, register, and test |

Every future generation brief must name the exact event, player action, loop state, or narrative beat that consumes the asset.

# 15. Godot Integration

Godot host work is limited to:

- correct stream loading;
- bus routing;
- lifecycle-safe event bridging;
- loop and crossfade presentation;
- resource caching;
- import settings;
- thin panel feedback;
- accessibility settings and user volume controls.

Required corrections:

- use container-aware WAV loading;
- fail the asset gate when godot --import fails;
- run import in an isolated user-data directory in CI;
- let Godot generate sidecars/cache artifacts;
- validate loop begin/end and audible seam behavior;
- validate stereo/mono and sample rate after ResourceLoader load;
- keep gameplay calculations out of AudioManager and UI panels.

# 16. Narrative/Content Integration

Audio generation should follow narrative importance:

1. crisis comprehension: radiation, weather, fire, breach, medical, and combat alerts;
2. action confirmation: inventory, crafting, treatment, repair, trade, and UI;
3. shelter identity: generator, ventilation, water, workshop, infirmary, doors;
4. expedition identity: vehicles, camp, biome, danger, location ambience;
5. survivor interiority: flashbacks, trauma, mutation, morale, final wishes;
6. radio and faction identity: original broadcasts, numbers stations, beacons, faction motifs;
7. expansion signatures: derailment, chemical warfare, desperation, Verdict, Holdfast, maritime, greenhouse, and future expansions.

Voice and radio rules:

- use only original ASHFALL scripts;
- maintain casting/voice continuity in a voice bible;
- disclose synthetic voice usage as required by project policy;
- prohibit imitation of identifiable performers;
- retain clean voice masters separately from radio-filtered delivery renders;
- caption every intelligible line and provide a transcript key;
- avoid critical gameplay information that exists only in audio.

Visual narrative work must use actual faction, survivor, location, and item state. No generic portrait may stand in for an unrelated item, and no location plate may represent two semantically distinct sites without an approved intentional variant relationship.

# 17. Failure Modes

| Failure | Required behavior |
|---|---|
| Godot import crashes | Stop the batch; do not accept or wire new assets |
| Missing cue JSON | Enter explicit degraded-audio mode and fail CI |
| Duplicate cue ID | Reject catalog load |
| Missing resource path | Fail cue closure gate |
| Invalid WAV/PNG signature | Quarantine output before import |
| True peak/DC outside target | Fail technical QA; do not compensate only with bus trim |
| Loop seam clicks | Reject loop or revise fade/crossfade |
| Event subscribed twice | Lifecycle test fails |
| Cue fires with no player-relevant state change | Remove/re-map the trigger |
| Generated asset resembles protected third-party material | Reject and regenerate |
| Watermark, logo, real flag, or rendered label appears | Reject visual |
| Perceptual duplicate crosses unrelated IDs | Flag for human review |
| Catalog changes during batch | Regenerate demand manifest before merge |
| Old save references renamed ID | Resolve alias or restore old ID |
| UI not mounted | Audio may play only if the domain event is globally relevant |
| User disables a bus | Respect setting; tests validate state without audible output |
| Very high event rate | Cooldown/max-instance prevents stacking |
| Missing reviewer | Asset remains GENERATED_UNREVIEWED |

# 18. Test Strategy

## Audio unit and host tests

- Cue JSON schema, duplicate ID, duplicate-path alias, fallback existence, and fallback-cycle tests.
- Real load test for every accepted resource path.
- WAV format assertions for sample rate, channels, bit depth, frame count, and duration.
- Loop flag and seam-boundary tests.
- ExpansionAudioBridge bind-twice, provider-change, dispose, and callback-count tests.
- AudioEventBridge lifecycle tests.
- Direct-load equivalence tests for WAV, MP3, and OGG.
- Missing/corrupt file behavior and one-shot logging tests.
- Cue-to-bus validity tests for all rows.
- Static event-to-cue closure plus runtime event probes.

## Audio technical QA

- independent decode;
- non-zero frame count;
- no NaN/invalid samples;
- DC offset;
- sample peak;
- true peak;
- integrated/short-term loudness as appropriate;
- phase correlation and mono compatibility for stereo;
- leading/trailing silence;
- loop seam delta;
- clipped-sample count;
- duration bounds per asset class.

Initial delivery targets, to be ratified through in-engine audition:

| Class | Loudness guidance | True peak ceiling | DC ceiling |
|---|---|---:|---:|
| Ambience and continuous machinery | -24 to -18 LUFS-I | -2 dBTP | 0.005 |
| Radio and intelligible voice | -20 to -16 LUFS-I | -1 dBTP | 0.005 |
| UI and tactile foley | Short-term/peak matched within family | -2 dBTP | 0.005 |
| Combat, impact, and alerts | Preserve transient contrast; do not loudness-match blindly | -1 dBTP | 0.005 |
| Music | Program-level review against dialogue/alerts | -1 dBTP | 0.005 |

Short one-shots must not fail merely because integrated LUFS is negative infinity under EBU gating. Use peak, short-term loudness, clipped samples, and family-relative comparison.

## Visual tests

- file signature and decode;
- dimensions/aspect/alpha contract;
- color-space/import preset;
- exact and perceptual duplicate scan;
- AssetRegistry full resolution;
- no cross-domain placeholder collision;
- contact-sheet review;
- representative Godot panel snapshots;
- small-size readability at 32, 64, and 128 pixels;
- accessibility review for contrast and color-independent meaning.

## Canonical verification

Every completed implementation batch must report:

1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj
4. godot --headless --path . -- --data-integrity-selftest
5. godot --headless --path . -- --bridge-selftest

Asset batches additionally require:

- godot --headless --path . --import, with a hard failure on nonzero exit;
- audio asset QA gate;
- godot --headless --path . -- --audio-selftest;
- source/sidecar orphan sweep;
- LFS health check;
- full asset coverage report;
- relevant panel snapshot/playthrough probe.

# 19. Dependency-Ordered Phases

## Phase 0 — Freeze and baseline

Purpose: stop planning against a moving target.

Actions:

- choose a clean revision or dedicated asset branch;
- capture current audio and visual hashes;
- regenerate production demand manifests;
- preserve all existing untracked audio before any generator rerun;
- reproduce and diagnose the Godot import crash;
- record baseline build, test, import, cue, and visual-coverage results.

Completion gate:

- stable HEAD and unchanged manifest during the run;
- no unknown file overwrites;
- import failure has a reproducible cause and owner.

Do not generate new files yet.

## Phase 1 — Import and validation foundation

Dependencies: Phase 0.

Actions:

- fix the Godot import crash;
- make godot-asset-gate.sh fail when import fails;
- add raster/audio signature and decode gates;
- separate SVG tooling limitations from actual corruption;
- repair or intentionally remove marker_safe.png;
- stop generation scripts from fabricating .import files.

Completion gate:

- fresh isolated import exits 0;
- corrupt payload fixture fails;
- valid audio and visual fixtures pass.

Do not expand cue or art counts yet.

## Phase 2 — Audio runtime safety

Dependencies: Phase 1.

Actions:

- make audio bridge subscriptions idempotent/disposable;
- remove per-frame resubscription;
- repair container-aware WAV loading;
- require actual stream load in AudioSelfTest;
- add callback-count and teardown tests.

Completion gate:

- repeated refresh produces exactly one callback;
- disposed providers produce zero callbacks;
- every current cue loads through a valid parser.

Do not accept newly generated audio before this phase passes.

## Phase 3 — Audio authority consolidation

Dependencies: Phase 2.

Actions:

- migrate 144 hardcoded registrations into audio_cues.json;
- merge the JSON-only cue;
- keep stable C# constants;
- implement schema and reference validation;
- make the catalog documentation generator deterministic;
- generate documentation and QA ledgers from the same authority.

Completion gate:

- runtime, generated documentation, and JSON report the same cue count and IDs;
- no duplicate, missing, or cyclic fallback;
- date changes alone cannot fail drift checks.

## Phase 4 — Rescue and remaster the current library

Dependencies: Phase 3.

Actions:

- preserve original copies/hashes;
- remove DC from the eight severe tracked alert/weather sources;
- re-master the 52 full-scale generated WAVs with class-appropriate headroom;
- repair other files over the agreed true-peak/DC limits;
- inspect the tinnitus and extremely quiet assets in context;
- validate all loop seams;
- normalize within semantic families, not the entire library to one LUFS target.

Completion gate:

- zero assets exceed the approved true-peak ceiling;
- zero assets exceed the DC ceiling;
- no clipped samples;
- all files decode and load through Godot;
- creative reviewer accepts A/B comparisons.

## Phase 5 — Close confirmed live audio gaps

Dependencies: Phase 4.

Actions:

- register action_interrogation_slam;
- register hazard_toxic_sizzle;
- generate and accept train_screech_crash;
- preserve bio_mutation_pulse;
- classify the other uncataloged files as alias, superseded, accepted, or rejected;
- execute each corresponding Core event through the host.

Completion gate:

- four expansion probes each produce exactly one intended cue;
- zero confirmed live silent events;
- no orphaned delivery sources.

## Phase 6 — Shared audio generation pipeline

Dependencies: Phases 1-5.

Actions:

- extract shared seeded synthesis, mastering, export, measurement, and ledger functions;
- migrate the phase2-phase5 and tactile generators;
- define named delivery presets per asset class;
- prohibit full-scale peak normalization;
- use Godot import rather than sidecar templates;
- add byte-reproducibility checks for procedural renders.

Completion gate:

- two identical seeded runs produce identical delivery hashes;
- every output passes technical QA before entering assets/audio;
- no generator writes .import content.

## Phase 7 — Flagship audio generation waves

Dependencies: Phase 6.

Generate only from a runtime-backed demand row.

### Wave A — Survival-critical feedback

- radiation dose stages and Geiger intensity;
- fire, breach, contamination, weather, medical, and power alerts;
- interaction confirmation and invalid-action cues;
- accessibility-safe redundant visual/text feedback.

Gate: every high-severity gameplay state has distinct, non-fatiguing, correctly routed feedback.

### Wave B — Living shelter

- generator load states;
- ventilation/recycler states;
- filtration, sump, pipe, and cistern states;
- workshop and infirmary loops;
- doors, airlocks, decontamination, and maintenance foley.

Gate: desired-state loop reconciliation works across load, pause, panel changes, and teardown.

### Wave C — Combat and equipment

- one coherent family per weapon/material class;
- dry-fire, jam, reload, casing, hit-material, downed, victory, and defeat layers;
- improvised weapon and condition-state variants;
- strict maximum-instance and transient headroom.

Gate: the expanded weapon catalog has explicit audio mapping or an approved family fallback; no event storm clips the output.

### Wave D — Expeditions, vehicles, and biomes

- vehicle family loops, start/stop, strain, breakdown, repair, and refuel;
- camp and travel layers;
- location/biome ambience prioritized by reachable route frequency;
- hazard, wildlife, structure, and discovery stingers.

Gate: representative expedition routes transition without loop leaks, silence, or abrupt seams.

### Wave E — Radio, voice, and narrative

- faction signatures;
- original radio intercepts;
- cassette logs and echo textures;
- survivor/final-wish voice only where casting and transcript continuity are approved.

Gate: transcript/caption closure, intelligibility, voice continuity, provenance, and radio-filtered/clean master pairs.

### Wave F — Expansion signature library

- one reusable sonic identity per shipped expansion;
- event-specific cues only where a live event consumes them;
- no new parallel bridge.

Gate: expansion self-tests and targeted playthroughs prove every signature path.

## Phase 8 — Visual authority and gate repair

Dependencies: Phase 1; may run alongside later audio waves.

Actions:

- reconcile visual helper tools with AssetRegistry;
- generate the live missing-ID list;
- correct size/aspect metadata from consuming UI;
- turn the full sweep into a thresholded gate;
- classify intentional aliases separately from placeholders;
- exclude or relocate quarantine after retention approval.

Completion gate:

- one production coverage number;
- every missing row has runtime priority and target contract;
- gate fails on corrupt sources and unapproved placeholder collisions.

## Phase 9 — Visual generation waves

Dependencies: Phase 8.

### Wave V1 — Player-surfaced gold set

Generate the top runtime-reachable locations, portraits, items, factions, and UI indicators in batches of at most 25 assets.

Gate: 100% resolution and creative acceptance for the selected gold set; snapshots pass.

### Wave V2 — Locations

Close the 259-location audit backlog, recalculated at phase start. Use coherent biome/location families and unique landmark silhouettes.

Gate: 100% reachable location coverage; no two semantically distinct locations share an unapproved exact image.

### Wave V3 — Portraits and named characters

Close the 148-portrait audit backlog and the currently unsurfaced named-character rows. Maintain age, injury, clothing, faction, and identity continuity.

Gate: 100% reachable portrait coverage and identity/contact-sheet review.

### Wave V4 — Factions

Close the 11-faction audit backlog with original emblems, no real flags, no text dependence, and small-size readability.

Gate: 100% faction coverage.

### Wave V5 — Items and equipment

Close the 365-item audit backlog by functional family. Intentional ammo/consumable variants must remain distinguishable at inventory scale.

Gate: 100% reachable item coverage and no cross-domain placeholder collision.

### Wave V6 — Residual catalog closure

Regenerate the full report and either generate every remaining runtime asset or grant a reviewed explicit exemption.

Gate: zero unexplained missing IDs.

## Phase 10 — Integrated flagship polish

Dependencies: all accepted audio and visual waves.

Actions:

- run a first-hour playthrough and representative late-game/expansion routes;
- mix against dialogue, UI, alerts, and music;
- inspect visual readability under every major panel state;
- verify accessibility and reduced-audio alternatives;
- replace fatigue-inducing or tonally inconsistent assets;
- lock release manifests and hashes.

Completion gate:

- technical, creative, runtime, accessibility, and provenance sign-off.

# 20. File Impact Map

| File/area | Action | Reason | Risk |
|---|---|---|---|
| Assets/StreamingAssets/Data/audio_cues.json | MODIFY | Single cue metadata authority | Medium |
| src/Audio/AudioCueCatalog.cs | MODIFY | Retain constants/parser; remove duplicate hardcoded metadata | High |
| src/Audio/AudioManager.cs | MODIFY | Correct stream load and binding cadence | High |
| src/Audio/AudioEventBridge.cs | MODIFY | Lifecycle-safe event translation | High |
| src/Audio/ExpansionAudioBridge.cs | MODIFY | Idempotent subscribe/unsubscribe | High |
| src/Audio/AudioSelfTest.cs | MODIFY | Real decode/load and closure assertions | Medium |
| src/Audio/AudioStateCoordinator.cs | MODIFY | Desired-state loop reconciliation | Medium |
| src/Audio/ShelterAudioController.cs | MODIFY | Shelter loop states only | Medium |
| src/Audio/SurfaceAmbienceController.cs | MODIFY | Weather/location crossfades only | Medium |
| tools/generate_audio.py | MIGRATE | Shared headroom/mastering behavior | Medium |
| tools/generate_phase2_audio.py through generate_phase5_audio.py | MIGRATE | Remove full-scale normalization and sidecar fabrication | Medium |
| tools/generate_tactile_ui.py | MIGRATE | Same production contract | Medium |
| tools/audio generation shared library | CREATE | One seeded render/master/export implementation | Medium |
| scripts/ci/audio-asset-gate.sh | CREATE | Decode, peak, DC, format, seam, and manifest gate | Medium |
| scripts/ci/generate-audio-catalog.py | MODIFY | JSON authority and deterministic output | Low |
| scripts/ci/godot-asset-gate.sh | MODIFY | Import failure becomes fatal | Medium |
| scripts/ci/asset-orphan-sweep.sh | EXTEND | Pair validation remains; signature validation delegates to new gate | Low |
| assets/audio | MODIFY/CREATE | Re-master accepted sources and add demand-backed cues | High |
| docs/audio/AUDIO_CUE_CATALOG.md | GENERATED | Human-readable catalog | Low |
| docs/audio/audio generation ledger | CREATE | Provenance and acceptance history | Low |
| src/Host/AssetRegistry.cs | MODIFY | Machine-readable full coverage and threshold | Medium |
| tools/visual_asset_audit.py | MODIFY | Correct category classification | Low |
| tools/visual_wiring_trace.py | MODIFY/DEPRECATE | Match production candidate rules or stop using as authority | Low |
| docs/visual/PRODUCTION_ART_GENERATION_MANIFEST.json | REGENERATE | Current demand and correct target contracts | Medium |
| assets/art, assets/sprites, assets/ui | CREATE/MODIFY | Close prioritized visual gaps | High |
| assets/sprites/Map/marker_safe.png | REPAIR/REMOVE | Invalid payload; decision based on intended use | Low |
| assets/quarantine | READ ONLY initially | Retention decision after import diagnosis | Medium |
| Assets/Ashfall.Core gameplay systems | READ ONLY by default | Preserve engine-agnostic domain ownership | High |
| save codecs and campaign envelope | READ ONLY | Asset work does not require save changes | High |

# 21. Risks

| Risk | Mitigation |
|---|---|
| Generation volume overwhelms review | Batches capped at 25 visuals and one coherent audio family |
| Existing untracked assets are overwritten | Hash and preserve before any generator execution |
| Loudness targets flatten dynamic range | Class-specific targets plus in-engine A/B review |
| New files pass presence checks but fail import | Import and ResourceLoader gates precede acceptance |
| Procedural audio sounds synthetic or fatiguing | Human audition, variant design, cooldowns, layered recordings where needed |
| AI visuals drift from ASHFALL identity | Family references, forbidden content, contact sheets, human acceptance |
| Third-party style or identity imitation | Originality review and explicit prohibition |
| Runtime catalog changes invalidate counts | Regenerate demand manifest per batch |
| JSON migration breaks cue lookup | Preserve IDs, parity tests, atomic migration |
| Event fixes change playback timing | Callback-count and scenario tests |
| Quarantine movement causes hidden references to break | Reference scan and reversible isolated commit |
| LFS/repository growth | Batch size, compression review, LFS gate, reject unused outputs |
| Visual 100% target encourages meaningless variants | Allow only reviewed semantic aliases/fallbacks |
| Voice generation creates continuity/legal concerns | Voice bible, provenance, disclosure, no identifiable imitation |

# 22. Out of Scope

Do not bundle:

- new gameplay mechanics created merely to consume an asset;
- Core architecture refactors unrelated to domain event exposure;
- save-format changes;
- Unity scene, prefab, ScriptableObject, or editor work;
- migration from Godot back to Assets/_Game;
- localization-system redesign;
- wholesale UI layout redesign;
- automatic deletion of duplicate or quarantine assets;
- replacement of every intentional icon alias;
- speculative soundtrack expansion without a live use case;
- copying the audio, art direction, UI, or characters of an existing survival game.

The ExpeditionCatalogLoader dangerLevel defect, disconnected fire/faction UI state, and string.GetHashCode determinism defects remain important but should be repaired in their own focused implementation unless they directly block an asset playthrough gate.

# 23. Rollback Strategy

- Use one reversible commit per foundation change and one commit per accepted asset batch.
- Preserve pre-master source hashes and originals outside active delivery paths.
- Migrate audio cue metadata atomically: JSON parity tests must pass before hardcoded registrations are removed.
- Retain stable cue IDs during source replacements.
- Do not delete superseded sources in the same commit that introduces replacements; deprecate, verify, then remove in a later reviewed commit.
- Keep quarantine movement separate from asset generation.
- If an imported asset breaks runtime loading, revert its batch without reverting pipeline fixes.
- If mastering reduces creative quality, restore the preserved source and revise the preset.
- Every phase ends at a green verification checkpoint before the next begins.

# 24. Definition of Done

## Audio flagship completion

- Godot full import exits 0 from a clean checkout.
- One authoritative JSON catalog describes every accepted cue.
- Catalog, generated documentation, and runtime counts match exactly.
- Zero live event-to-cue silences.
- Expansion/audio bridges emit exactly one callback per event and unsubscribe cleanly.
- Every accepted source independently decodes and loads through Godot.
- Zero sources exceed approved true-peak, clipped-sample, or DC limits.
- All loops pass seam and lifecycle tests.
- Every cue routes to an existing bus and respects settings/cooldowns.
- Every generated asset has provenance, recipe/seed where applicable, and technical plus creative acceptance.

## Visual flagship completion

- Full AssetRegistry sweep is gating, not report-only.
- All runtime-reachable catalog IDs resolve to accepted assets.
- Every residual non-reachable miss has an approved exemption or queued production row.
- No corrupt raster payload remains in active assets.
- No unapproved cross-domain exact placeholder collision remains.
- Priority locations, portraits, factions, items, and UI pass in-engine snapshot review.
- Quarantine is explicitly excluded from import/export or moved through a reviewed migration.

## Project completion

- The five canonical dotnet/Godot checks pass.
- Import, audio QA, LFS, orphan, full coverage, and snapshot gates pass.
- A representative first-hour and expansion playthrough passes.
- No Unity tool or Unity asset workflow was introduced.

# 25. Implementation Handoff

## MUST PRESERVE

- Assets/Ashfall.Core as engine-agnostic gameplay authority.
- Godot as the only active host/editor.
- Stable cue and catalog IDs wherever practical.
- Existing save-envelope and settings compatibility.
- Current dirty/untracked user assets until explicitly inventoried and preserved.
- Original ASHFALL identity and accessibility fallbacks.

## MUST ADD

- A hard-failing clean import gate.
- Real decode/load validation.
- Lifecycle-safe audio subscriptions.
- Container-aware WAV loading.
- One JSON cue authority.
- Shared seeded audio render/master/export tooling.
- Technical and creative QA ledgers.
- Runtime-derived audio and visual demand manifests.
- Full visual coverage gating.
- Provenance and originality review for every generated asset.

## MUST NOT DO

- Do not run Unity.
- Do not extend Assets/_Game or create Unity assets.
- Do not normalize every source to 0 dBFS.
- Do not fabricate .import sidecars in generation scripts.
- Do not generate assets without an identified runtime consumer.
- Do not treat file existence as successful import or playback.
- Do not consume simulation RNG for presentation variation.
- Do not delete duplicates or quarantine content automatically.
- Do not copy another game’s art, audio, interface, narrative, or characters.

## VERIFY WITH

- dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
- dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
- dotnet build Ashfall.csproj
- godot --headless --path . --import
- godot --headless --path . -- --data-integrity-selftest
- godot --headless --path . -- --bridge-selftest
- godot --headless --path . -- --audio-selftest
- scripts/ci/asset-orphan-sweep.sh
- scripts/ci/lfs-health-check.sh
- full production AssetRegistry coverage gate
- relevant UI snapshot and playthrough gates

## FIRST SAFE IMPLEMENTATION STEP

Create a dedicated asset-pipeline branch from a stable revision, hash and preserve every current untracked audio source, then reproduce the Godot import crash without changing any generator output. Do not regenerate, normalize, register, or delete an asset until that baseline and failure cause are recorded.
