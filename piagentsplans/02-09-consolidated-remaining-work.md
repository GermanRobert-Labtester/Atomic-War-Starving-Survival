# Plans 02–09 — Consolidated Remaining Work

> Canonical execution plan for the unfinished work formerly distributed across Plans 02–09.
> The source plans remain in `piagentsplans/` for traceability. Completed implementation is
> intentionally excluded from the work sections below.

## Objective

Finish only the verified gaps across catalog loading, data authority, relic research unlocks,
vinyl discovery, narrative activation, audio production, visual coverage, and medical care.
Every workstream must reuse the existing authority and runtime seam identified below; no second
catalog, duplicate system, or parallel host implementation may be introduced.

## Closed scope — do not repeat

| Source plan | Completed work excluded from this plan |
|---|---|
| Plan 02 | Loader failure hardening: no bare `catch { }` remains in the two audited loaders; malformed-file logging and regression coverage already exist. |
| Plan 03 | The root-catalog `schema_version` sweep and validator gate are complete. Do not bulk-edit the 138 versioned root catalogs again. |
| Plan 04 | The relic recipe expansion is complete: `relic_recipes.json` already contains 30 entries and required component/yield references resolve. |
| Plan 05 | The 30-record vinyl archive, `VinylMoraleSystem`, host adapter, and save path already exist. Do not author another album archive. |
| Plan 06 | Existing letter/cassette/war source content and the Core faction-war runner are not to be re-authored as duplicate catalogs. |
| Plan 07 | Existing audio registration, event bridge foundations, controllers, and current broadcast audio references are not to be rebuilt. |
| Plan 08 | No visual production is marked complete by the current evidence; this workstream remains active after a fresh coverage baseline. |
| Plan 09 | The disease catalog is already at 15 entries; detox/relapse hooks, palliative state, vigil state, and memorial grief behavior already exist and must be extended, not replaced. |

## Ownership rules

| Concern | Sole owner | Consumers |
|---|---|---|
| Cassette rows, parts, cassette itemization, and cassette playback | Plan 67 / its cassette authority | Plan 06 narrative integration, Plan 07 audio cues, Plan 47 collectible presentation |
| Moral-choice echo quest rows | Plan 109 / `moral_choice_chains.json` | Narrative and audio surfaces |
| Vinyl records and playback effects | Plan 05 / `vinyl_record_archive.json` + `VinylMoraleSystem` | Collectible presentation and scavenging |
| Disease/pathogen definitions | Plan 112 / `disease_catalog.json` | This plan’s diagnosis and care flow |
| Final wishes | Plan 65 | Letter, palliative, memorial, and epilogue consumers |
| Faction-war narrative data | Existing `FactionWarContentCatalog` | This plan’s host activation work |
| Audio assets and cue registration | This plan’s audio workstream | Narrative, weather, combat, medical, and UI events |
| Visual assets and asset registry entries | This plan’s visual workstream | Content systems and panels |

Plans that consume another authority must reference its stable IDs. They must not copy rows into
a new file or create a second loader.

## Phase 0 — Baseline and change safety

1. Record the current working-tree state before touching any plan-owned file. Plans 06 and 09,
   plus the audio source/catalog documentation, have active edits and must be merged carefully.
2. Re-run the narrow inventory checks for each workstream rather than relying on the stale counts
   in the original plans. In particular, refresh catalog counts, audio cue/reference counts, and
   visual coverage before selecting production work.
3. Keep all verification on the canonical Godot/.NET path. Do not invoke Unity tooling.

## Workstream A — Data authority residuals and relic unlocks

### A1. Finish only the remaining Plan 03 hygiene

- Preserve the completed root-catalog `schema_version` state and its validator tests.
- Inventory remaining camelCase-to-snake_case drift and file the per-family migration notes
  required by the data-authority policy.
- Do not rename keys in this consolidation unless the owning loader and compatibility tests are
  updated in the same change. This is documentation/targeted migration work, not another sweep.

### A2. Make the 30 existing relic recipes resolve to authored research

- Add the 16 relic research unlock definitions to the existing research authority used by
  `ResearchSystem`; do not rely on `WorkshopReverseEngineeringSystem` dynamically inventing a
  knowledge node at completion time.
- Preserve the existing 30 recipe rows, teardown behavior, and item references. Only fill the
  missing static knowledge definitions and their meaningful downstream craft/research links.
- Add coverage that every non-empty `research_unlock_id` in `relic_recipes.json` resolves before
  a workshop completion, and that each definition can be unlocked and saved/restored.
- Keep Plan 87’s provenance, study, trade, and memorial responsibilities separate; it must
  consume relic results rather than modify the recipe authority.

**Primary files:** `Assets/Ashfall.Core/Research/`, `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs`,
the existing research data authority, `Assets/StreamingAssets/Data/relic_recipes.json`, and focused
Core tests.

## Workstream B — Vinyl discovery and playback completion

- Treat the existing 30 records in `narrative/vinyl_record_archive.json` as the only vinyl content
  authority. Do not create a second `vinyl_records.json` or duplicate them in a generic
  collectibles catalog.
- Give each existing record a stable acquisition path. Prefer a single explicit mapping from
  `record_id` to the existing item/collectible representation, then wire the mapping into the
  existing scavenge/loot authority.
- Replace the one hardcoded playback route in `VinylMoralePanel` with a data-driven list of
  acquired records. Preserve the current host adapter and save format.
- Preserve the existing per-record catalog fields and use them for distinct playback effects;
  do not add a parallel morale or culture system.
- Add tests for archive-to-runtime mapping, loot/acquisition, dynamic UI selection, save/load, and
  record-specific effect application.

**Boundary:** Plan 47 may present vinyl as a collectible category, but it must reference these
record IDs and may not create another vinyl row set or morale authority.

## Workstream C — Narrative activation without duplicate catalogs

### C1. Last-letter delivery state

- Add persistent delivery/response state for the existing letter IDs: found, addressed,
  delivered, withheld, or unanswered.
- Resolve the addressee through existing survivor relationships and emit only modest,
  data-driven relationship, morale, grief, or narrative-hook effects.
- Connect capture/restore and a post-load delivery test. Missing or legacy state must normalize
  safely.
- Do not add final-wish rows or reinterpret a letter as a second final-wish system; Plan 65 owns
  that catalog.

### C2. Echo/cassette integration only

- Remove cassette expansion and new cassette item authoring from this workstream.
- Consume the cassette catalog and item IDs owned by Plan 67 when available, connecting findings
  to existing locations, people, flags, journal entries, and audio cues.
- Restrict “echo” here to recoverable environmental/letter/audio integration. Moral-choice echo
  quest rows remain owned by Plan 109.
- Test reachability, one-time/replay behavior, and save/load restoration without creating a new
  cassette or echo loader.

### C3. Activate the existing faction-war runner in Godot

- Wire `FactionWarContentCatalog` and `FactionWarChainRunner` into the existing
  `YearOfAshHostSession` lifecycle: load, tick, capture, restore, and resolution.
- Use the existing authored faction-war events, journal, radio, dialogue, communiques, and
  location overrides. Do not author a second war model or duplicate radio corpus.
- Connect the runner to existing standing, radio, map/location, and epilogue consumers through
  adapters or resolvers already used by the host.
- Add deterministic host-level coverage for trigger, escalation, choice, resolution, save/load,
  and legacy-save behavior. Core runner tests remain regression coverage, not proof of host wiring.

**Primary files:** existing letter/narrative data, `Assets/Ashfall.Core/YearOfAsh/` war catalog and
runner, `src/YearOfAsh/YearOfAshHostSession.cs`, `src/Main.YearOfAsh.cs`, and focused host tests.

## Workstream D — Audio gap closure

### D1. Refresh the silence baseline

- Regenerate the audio catalog documentation from the current `AudioCueCatalog` source and
  reconcile the active documentation edits before ranking gaps.
- Rebuild `SILENCE_AUDIT.md` from current call sites and event surfaces. Do not carry forward the
  stale 49-cue/zero-reference claims; the current source has 75 registered cues and existing
  broadcast references.
- Verify resource paths, bus routing, cooldown/stacking behavior, and the current audio selftest.

### D2. Produce only confirmed priority gaps

- Select the top missing cues from the refreshed matrix. Do not reproduce already registered
  weather, combat, medical, UI, or broadcast cues.
- Register new files through the existing `AudioCueCatalog` and `AudioEventBridge`; keep all
  audio logic presentation-only and engine-specific to the Godot host.
- Preserve existing broadcast `audio_cue` references and add only newly confirmed high-value
  references. Narrative plans own the broadcast text and selection.
- Add orphan/cue/path integrity coverage and keep the catalog generator check green.

### D3. Complete the reactive ambience layer

- Extend the existing `ShelterAudioController`, `SurfaceAmbienceController`, and
  `AudioManager`; do not create replacement controllers.
- Add a small host-side evaluator with hysteresis for power strain, sickness burden, weather,
  threat/combat proximity, and surface/shelter state.
- Add only the ambience/music beds and transition cues required by the evaluator. Audio state is
  presentation-only: it is not saved and must not affect simulation determinism.
- Test every evaluator state resolves to an existing cue and does not flap around thresholds.

## Workstream E — Visual coverage completion

1. Generate a fresh asset-coverage baseline before selecting IDs; the 2026-08-26 report is a
   planning baseline, not proof that every count is current.
2. Produce the highest-exposure missing location art in batches, using the existing art-family,
   Godot import, LFS, and registry conventions. Content plans own location/faction definitions;
   this workstream owns only visual assets and registration.
3. Complete named survivor portraits first, then use approved archetype mappings for procedural
   cohorts only if the coverage resolver explicitly counts those mappings.
4. Produce the remaining item icons and real faction emblems after rechecking that each ID still
   exists. Remap dead aliases instead of drawing art for them.
5. Run asset-registry, coverage, LFS, shader/material, and relevant snapshot checks after each
   batch. Do not turn report-only coverage into a hard gate until the current resolver semantics
   and thresholds are verified.

**Boundary:** Plan 06/other content plans may identify important locations or characters, but they
   do not author duplicate art files or registry entries.

## Workstream F — Medical diagnosis and care integration

### F1. Data-backed diagnosis and outbreak response

- Consume the 15 existing Plan 112 disease definitions; do not add pathogens, vectors, or a
  second disease loader here.
- Identify the existing authoritative disease/treatment fields. Where diagnostic tells, test
  availability, isolation guidance, or treatment references are missing, extend that authority
  or add one explicitly owned medical-protocol shape rather than scattering constants across
  systems.
- Preserve suspected-versus-confirmed masking and route outbreak facts through `DiseaseSystem`
  and existing medical/event paths.
- Test diagnostic progression, protocol reference validity, outbreak source adapters, and legacy
  saves without diagnosis records.

### F2. Finish detox-clinic surfaces around existing dependency hooks

- Keep `ChemicalDependencySystem`, tolerance/withdrawal/craving behavior, stress relapse hooks,
  detox items, and save contracts as the existing authority.
- Fill only the missing staffing, support-choice, UI, and relapse-response surfaces; do not create
  a parallel dependency or affliction system.
- Test recovery, relapse response, detox exclusions, persistence, and host presentation.

### F3. Link palliative care, vigils, consent, and memorial outcomes

- Connect the existing palliative assignment and `VigilStateMachine` through an explicit,
  persistent care flow with consent/capacity rules.
- Reuse existing relationship, memorial, grief, medical, and Plan 65 final-wish surfaces. Do not
  create another death, grief, or end-of-life data model.
- Test care-choice consequences, vigil progression, grief quality, save/load, and null/legacy
  relationship cases end to end.

## Verification gates

Run focused tests after each workstream, then the full canonical pipeline:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --asset-registry-selftest
godot --headless --path . -- --bridge-selftest
python3 scripts/ci/generate-audio-catalog.py --check
```

Focused regression areas should include catalog/research, vinyl, faction-war host wiring,
audio cue integrity, asset registry/coverage, disease diagnosis, dependency relapse, and
memorial/vigil behavior. Report failures against the current baseline; do not silently mark
stale source-plan claims as complete.

## Completion criteria

- Plans 02 and the completed schema portion of Plan 03 remain closed and are not reimplemented.
- Every remaining workstream has one clear data/system owner and no duplicate catalog or host
  path.
- Existing 30 relic recipes, 30 vinyl records, 15 diseases, faction-war content, and current
  audio foundations are extended in place rather than recreated.
- Cassette expansion, moral-choice echo quests, final wishes, disease rows, and broadcast prose
  remain delegated to their established plans.
- All focused and canonical verification gates pass, with active working-tree changes preserved.
