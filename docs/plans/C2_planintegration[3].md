# C2 — Flagship Integration Plan [3]: Living Content, Consequence, and Utilization Integrity

> **Deliverable:** `C2_planintegration[3].md`
> **Source scope:** Plan 18 — *Living Content: From Readable to Consequential*
> **Wave:** Continuity Wave 1
> **Hard prerequisites:** Plan 15A and Plan 15C
> **Execution order:** **18A → 18B → 18C**
> **Expansion freeze:** pause generic catalog-expansion batches while 18A/18B are active.
> **Core standard:** authored gameplay content must progress from definition → loader → runtime selection → real effect → player-visible consequence → persistence/evidence → CI protection.

---

## 0. Executive Intent

ASHFALL does not currently have a content shortage; it has a consequence shortage. The source baseline identifies 411 catalogs and 4,808 authored definitions, while only four catalogs were recorded at `EFFECT_PRODUCED`. This plan converts a bounded slice into genuinely causal content and then changes the gates so the same backlog cannot silently regrow.

The target lifecycle is:

```text
content definition
→ loader
→ runtime consumer
→ selection
→ effect application
→ save/day ownership where stateful
→ player surface/feedback
→ runtime evidence
→ CI ratchet
```

The program succeeds when “present,” “loaded,” “queried,” or “named in a scanner table” are no longer accepted as evidence that gameplay content is alive.

---

## 1. Historical Baseline to Re-measure

The source reports the following historical metrics from `artifacts/content-utilization.json`:

| Metric | Historical value |
|---|---:|
| Total catalogs | 411 |
| Total definitions | 4,808 |
| Gameplay-consumed catalogs | 110 |
| Codex-only catalogs | 272 |
| Catalogs with zero `consumerSystems` | 300 |
| Definitions in zero-consumer catalogs | 2,067 |
| `DISCOVERED` only | 271 |
| `QUERIED` | 133 |
| `EFFECT_PRODUCED` | 4 |
| Runtime-evidence classifications | 9 |

Re-run the current scanner before implementation. Historical numbers are comparison points, not fixed assertions.

Record before/after:

- catalog count,
- definition count,
- zero-consumer catalogs and definitions,
- stage distribution,
- runtime-evidence count,
- `EFFECT_PRODUCED`,
- exemptions by reason,
- root-array count failures,
- dead fields after 18C inventory exists.

The source warns that `DATA_GAP_AUDIT.md` is partly stale. Every orphan/consumer claim must be re-verified against current source before code is removed or added.

---

## 2. Architectural Invariants

1. **One wiring pattern.** 18A defines the standard; 18B reuses it.
2. **Existing systems first.** Attach content to a live system when the domain already exists.
3. **No fourth resolver.** Echo choices reuse the established choice/effect application idiom.
4. **No new condition language.** Echo conditions use the flag ledger/current predicate semantics.
5. **Deterministic selection.** Use `ISeededRng`; never rely on dictionary order or unstable hashes.
6. **Save through existing infrastructure.** Use `SaveSectionRegistry`/`SaveStoreHub` conventions.
7. **Existing player surfaces.** Reuse journal/codex routes; no `echo_console`.
8. **Runtime evidence matters.** Static naming alone never proves effect production.
9. **Ratchets shrink debt.** Baseline exemptions/dead fields may stay equal or decrease, never silently grow.
10. **No authoring before consumer.** Pause generic content-expansion batches until this continuity wave is closed.

---

## 3. Preflight Verification

Run before edits:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --content-utilization-selftest
bash scripts/ci/triad-drift-gate.sh
bash scripts/ci/verify-fast.sh
```

Also run the repository narrative graph/continuity gates where available.

Stop and classify drift before implementation if:

- tests are already failing,
- `echoes.json` no longer matches the source premise,
- an Echo runtime already exists,
- Plan 15A is not live,
- Plan 15C liveness gate is absent,
- save registry is undergoing conflicting migration,
- Plan 31 semantic event kinds are being changed concurrently.

---

# Workstream 18A — Close One Narrative Chain End to End

## 4. Objective

Make `echoes.json` a complete causal loop and use it as the reusable standard for future content families.

Target chain:

```text
echoes.json
→ EchoDefinition
→ EchoCatalogLoader
→ EchoSystem
→ availability + deterministic selection
→ existing journal surface
→ existing effect applier
→ guilt/dose/morale/flag/map consequence
→ day event + audio
→ save/load
→ runtime evidence
→ exemption removed
```

The source identifies 23 authored echoes with choices, conditions, and minimum-day gating. Re-count at execution time.

## 5. Data-first reconnaissance

Read the current JSON before writing DTOs. Record exact current fields, including source-listed candidates:

- `id`,
- `title`,
- `body`,
- `choices[]`,
- `conditions[]`,
- `minDay`,
- `factionId`,
- `schema_version`,
- choice effect payloads,
- any localization/reference fields.

Validate every echo for:

- unique sanctioned `echo_` ID,
- valid choice identities/order,
- valid conditions,
- valid `minDay`,
- valid faction/reference IDs,
- at least one real effect per authored choice after mapping.

Do not design the data contract from plan prose if source JSON differs.

## 6. Echo entity and loader

Implement an engine-agnostic `EchoDefinition` contract in Core.

Requirements:

- snake_case mapping where the authority uses it,
- `schema_version` respected,
- no engine references,
- no `JsonUtility`,
- no UI types,
- no mutable runtime services embedded in DTOs.

Implement `EchoCatalogLoader` using existing loader patterns such as `SystemTextJsonSerializer` and `CatalogDiagnostics.Warn(path, shape, ex)`.

Malformed data must produce a visible diagnostic rather than silently collapsing to an empty catalog.

Tests:

- valid current file loads all current definitions,
- malformed root handled explicitly,
- malformed choice handled explicitly,
- duplicate IDs rejected,
- unsupported schema path tested,
- sanctioned ID prefix enforced through the existing integrity mechanism.

## 7. EchoSystem runtime responsibilities

`EchoSystem` owns:

- availability,
- deterministic selection,
- surfaced/resolved state,
- exactly-once resolution,
- capture/restore,
- `OnEchoSurfaced`,
- `OnEchoResolved`.

It does not own:

- a new condition language,
- a new guilt or morale subsystem,
- UI rendering,
- a new generic effects engine.

Availability should derive from real campaign state:

```text
current day >= minDay
AND existing flag-ledger conditions pass
AND repeatability/resolution state permits selection
```

Selection must use `ISeededRng` and stable candidate ordering.

Tests:

- `minDay - 1` unavailable,
- `minDay` available,
- flag condition false/true boundaries,
- same seed/state selects same echo,
- invalid/no candidates handled deterministically.

## 8. Reuse existing effect resolution

Adapt echo choices into the same effect-application model already used by working choice systems such as the source-listed expedition, door-encounter, and duty-roster paths.

Do not add a fourth resolver implementation.

Permitted real effects, when authored and supported, include:

- setting/checking flags,
- adding canonical guilt records consumed by `GuiltInsomniaSystem`,
- adjusting morale through the existing morale channel,
- dose changes through the existing radiation/medical owner,
- map-node reveal,
- other already-supported effect descriptors.

Every authored choice must either map to at least one valid effect or fail validation. Logging “resolved” while applying nothing is prohibited.

Exactly-once requirements:

- no duplicate guilt,
- no duplicate morale delta,
- no duplicate flag effect,
- no duplicate map reveal,
- no duplicate journal entry,
- no duplicate day event,
- no duplicate audio cue.

## 9. Persistence

Register an `echoes` save section only if the existing save ownership model requires a distinct section. Route storage through `SaveStoreHub` and the campaign envelope.

Persist authoritative state only, such as surfaced/resolved IDs and any pending one-time state. Reconstruct title/body from catalog IDs.

Tests:

```text
surface → save → load → same pending state
resolve → save → load → same resolved state
load resolved echo → no effect replay
```

Add checksum/round-trip coverage using repository conventions.

## 10. Day-loop integration

Either:

- register a dedicated `IDayAdvanceOwner`, or
- attach to `narrative_quests_verdict` if current phase ordering and ownership fit.

Prefer attachment when it avoids unnecessary owner proliferation.

Emit `DayStateChangeEvent` using the corrected semantic vocabulary from Plan 31 / the Plan 17 erratum path. Do not reuse stale 17A kind assumptions.

Integration test:

```text
echo surfaces
→ semantic event emitted
→ event recognized by consumer
→ briefing/event history receives it where intended
```

## 11. Player surface

Reuse `JournalPanel` / `journal_detail` or the current equivalent.

Player-facing requirements:

- echo title/body visible,
- choices rendered in stable order,
- unavailable reasons visible when applicable,
- action reaches live campaign authority,
- resolved state visible,
- history remains accessible under existing journal semantics.

The route must pass Plan 15C’s liveness gate: real authority, real mutation, no fixture IDs, no fresh campaign subsystem.

## 12. Audio/feedback

Subscribe `AudioEventBridge` to `OnEchoSurfaced` using an existing appropriate `radio_*`/`amb_*` cue.

No new audio family. Follow Plan 17C exactly-once ownership if that work has landed.

Do not play “new echo” audio during save restoration unless the current audio lifecycle explicitly models persistent ambience.

## 13. Remove exemption and update graph reachability

Delete `exempt_echoes_future` once the expiry condition is genuinely met.

Update narrative graph docs/lint so `echo_` nodes are reachable from a real producer/consumer pair.

Re-run:

```bash
godot --headless --path . -- --data-integrity-selftest
```

and the narrative graph/continuity gates.

## 14. Document the pattern

Create:

```text
docs/narrative/ECHO_WIRING_PATTERN.md
```

Document the source’s five core artifacts:

1. entity,
2. loader,
3. system,
4. effect applier integration,
5. save section/day-owner integration where stateful.

Also document required player surface, runtime evidence, and test/gate obligations around those artifacts.

Future `piagentsplans` expansion batches must cite this pattern before adding entries.

## 15. 18A DoD

- [ ] current echoes schema verified,
- [ ] EchoDefinition implemented,
- [ ] loader implemented with malformed-data diagnostics,
- [ ] EchoSystem implemented,
- [ ] deterministic selection,
- [ ] existing condition/effect semantics reused,
- [ ] every choice has a real effect,
- [ ] exactly-once resolution,
- [ ] save/load round-trip,
- [ ] day-loop integration,
- [ ] semantic event integration,
- [ ] existing journal surface used,
- [ ] audio feedback wired,
- [ ] `exempt_echoes_future` removed,
- [ ] runtime utilization evidence reaches selection/effect,
- [ ] pattern document landed.

---

# Workstream 18B — Retire `exempt_no_source_evidence`

## 16. Objective

Convert every current catalog in the generic no-source-evidence bucket into one of three legal states:

```text
WIRED
DELETED
QUARANTINED WITH OWNER + TICKET + EXPIRY CONDITION
```

Target:

```text
exempt_no_source_evidence = 0
```

## 17. Tighten scanner evidence

A scanner table that says “consumer = WeatherSystem” is not sufficient evidence.

Static evidence should distinguish:

- consumer declaration,
- construction/composition,
- catalog query,
- effect-producing call.

Classification must not jump directly to gameplay consumption because a hardcoded table names a class.

Tests:

- named-only consumer → insufficient,
- declaration-only → insufficient for effect,
- constructed but unused → insufficient for effect,
- query → `QUERIED`,
- real state mutation → eligible for `EFFECT_PRODUCED`.

## 18. Generic exemption becomes a failure

After a reviewed grace baseline, a new catalog entering `exempt_no_source_evidence` must fail the gate.

A legitimate quarantine must instead contain:

- catalog,
- owner,
- ticket/plan,
- reason,
- expected consumer,
- expiry condition.

Do not rename the generic exemption and keep the loophole.

## 19. Fix root-array counting blind spot first

Re-verify source-listed bare-array catalogs such as:

- `cassette_sets.json`,
- `guilt_sources.json`,
- `confession_secrets.json`,
- `damaged_map_zones.json`,
- `trade_specialties.json`,
- `final_wishes.json`,
- `deep_lore_survivor_fields.json`,
- `antigravity_survivor_fields.json`.

Fix both:

- `ContentUtilizationScanner`,
- catalog integrity counting.

Required invariant:

```text
supported root shape → definitions counted consistently by both systems
```

Tests cover object roots, bare arrays, empty arrays, malformed roots, and parity.

## 20. Wire the two largest source targets

### 20.1 `environmental_atmosphere_expansion.json`

Attach to existing weather/starting-level/hazard presentation context where current architecture supports it.

A definition should be selected because of real weather/environment state, not because a UI randomly wants flavor text.

Where Plan 17C is live, atmosphere data may inform existing ambience/weather cue choice only if the semantic relationship is real.

If a definition is intentionally descriptive only, classify it honestly as presentation content rather than falsely claiming simulation effect.

### 20.2 `medical_texts.json`

Attach to `MedicalWardSystem` and current procedure/autopsy/sick-list surfaces.

Selection should derive from actual patient/procedure state.

Tests must prove real runtime selection and, where appropriate, effect production or explicit presentation-only classification.

## 21. Encounter cluster

Re-verify and integrate the source cluster:

- `narrative_encounters_expansion.json`,
- `environmental_texts_expansion_05.json`,
- `narrative_arc_events.json`,
- `narrative_questlines.json`.

Attach to the existing NarrativeEncounter path used by working systems such as `DoorEncounterSystem` / `FactionWarChainRunner`.

Do not build one runtime per file.

Shared obligations:

- existing condition semantics,
- existing resolver/effect path,
- existing surface routes,
- runtime selection evidence,
- exactly-once effects,
- save ownership where stateful.

## 22. Memory cluster

Integrate source targets:

- `journal_entries_expansion_05.json`,
- `memorials_expansion_05.json`.

Use current `JournalSystem`, `MemorialSystem`, `DeathQuality`, and any live `MemorialOutcome` / `IGriefSink` contracts present in the repository.

Goal: real survivor/death state influences what memory content is selected and, where authored, grief/morale/flag consequences.

Presentation-only memorial text must be classified honestly rather than inflated into `EFFECT_PRODUCED`.

## 23. Moral-choice stubs

For `moral_choice_quest_stubs.json`, choose one:

1. restore into the live Plan 15A moral-choice resolver with complete valid data, or
2. remove it from authoritative gameplay content.

Unconsumed stubs are prohibited.

## 24. Small-catalog sweep

For source-listed families such as:

- `trade_specialties.json`,
- `confession_secrets.json`,
- `final_wishes.json`,
- `damaged_map_zones.json`,
- `cassette_sets.json`,
- `*_survivor_fields.json`,

produce a PR/closure table:

| Catalog | Count | Root shape | Current consumer | Decision | Target consumer/effect | Owner | Evidence |
|---|---:|---|---|---|---|---|---|

Decision must be `WIRE`, `DELETE`, or explicit `QUARANTINE`.

## 25. Runtime utilization collector

Extend `ContentUtilizationRuntimeCollector` so the real campaign boot/use path can record lifecycle stages equivalent to:

```text
LOAD
DESERIALIZE
REGISTER
SELECT
EFFECT
```

Use actual repository stage names if they differ.

Requirements:

- observational only,
- no gameplay mutation,
- stable IDs,
- low overhead,
- real campaign evidence, not synthetic-only success,
- enough consumer/source identity to diagnose dead wiring.

Target metrics for the wired families:

```text
SELECTED rises
EFFECT_PRODUCED rises
```

## 26. Repin utilization baseline and add monotonic rules

Update `artifacts/content-utilization-baseline.json` with current before/after values.

Ratchets should ensure, at minimum:

```text
definitions_wired_new >= definitions_wired_baseline
zero_consumer_definitions_new <= zero_consumer_definitions_baseline
```

Report deletions separately so the metric cannot be gamed by deleting content and calling it “wired.”

## 27. Repair stale data-gap audit

Update `docs/data/DATA_GAP_AUDIT.md` after source verification.

Correct stale rows such as the source-mentioned `questline_master.json` orphan claim if still stale.

Where document format allows, record verification commit/date.

## 28. 18B DoD

- [ ] scanner no longer self-attests consumer evidence,
- [ ] generic no-source-evidence bucket no longer accepted,
- [ ] root-array blind spot fixed in scanner and integrity validator,
- [ ] atmosphere family wired/classified,
- [ ] medical family wired/classified,
- [ ] encounter cluster wired,
- [ ] memory cluster wired,
- [ ] moral stubs wired or deleted,
- [ ] small catalog decisions documented,
- [ ] runtime collector observes real campaign stages,
- [ ] `SELECTED` improves,
- [ ] `EFFECT_PRODUCED` improves,
- [ ] utilization baseline repinned,
- [ ] `exempt_no_source_evidence = 0`,
- [ ] stale audit rows repaired.

---

# Workstream 18C — Make Data Fields Load-Bearing

## 29. Objective

Move from catalog-level evidence to field-level evidence.

Known proof case:

```text
regional_supply
```

exists in `economy_goods.json` but the source reports zero C# references.

The new invariant:

> every authored gameplay field either changes behavior, has an explicit presentation/schema policy role, or is removed.

## 30. Field-level inventory

Parse gameplay catalogs and enumerate:

- root keys,
- definition property keys,
- nested configuration/effect keys where practical,
- schema metadata.

Compare against both:

- DTO/property mappings,
- raw JSON access patterns (`JsonNode["key"]`, `GetRawText`, etc.).

Report two separate concepts:

1. field is parsed/read,
2. field demonstrably influences behavior.

A DTO property alone does not make a knob load-bearing.

## 31. Triage fields

Use the source’s four buckets:

- `WIRE`,
- `CONSUME-AS-TEXT`,
- `DELETE`,
- `POLICY`.

For every `WIRE` field, record:

- owner,
- target system,
- test,
- observable.

Policy fields include legitimate schema metadata such as `schema_version`; do not use POLICY as a generic dead-field exemption.

## 32. Wire `regional_supply`

Make regional supply influence authoritative `MarketSystem` price/band behavior.

If Plan 14A’s `TradeEmbargoSystem` is already live and is the canonical owner, reuse it where appropriate. Otherwise use the source’s minimal approach inside `MarketSystem`, such as a region × good supply modifier.

Do not create a disconnected economy subsystem.

Required consistency:

```text
same authoritative supply model
→ price calculation
→ caravan/trade UI
→ any briefing explanation
```

Behavioral test:

```text
same seed/state + supply A → P1
same seed/state + supply B → P2
assert intended direction and determinism
```

## 33. One knob, one test, one observable

For each WIRE field:

```text
change field
→ run owning simulation path
→ output changes
→ player can observe or infer the consequence through an existing surface/briefing
```

Potential observables include price, availability, hazard probability, route state, morale/dose result, or another canonical output.

## 34. Mod-contract and schema safety

Before deleting a field:

- determine whether it is published/mod-facing,
- update mod-contract documentation,
- apply `schema_version` discipline for breaking changes,
- provide migration/default behavior if required.

Do not silently remove a public field simply because internal code ignored it.

## 35. Field-utilization integrity tier

Integrate field utilization into existing `CatalogIntegrityValidator` / checker architecture.

It must run through:

```bash
godot --headless --path . -- --data-integrity-selftest
```

Policy whitelists may include genuine metadata/presentation fields, not arbitrary balance knobs.

## 36. Dead-field baseline and ratchet

Create:

```text
docs/data/DEAD_FIELDS_BASELINE.md
```

Each allowed current offender must name:

- catalog,
- field,
- classification,
- owner,
- follow-up task,
- reason,
- exit condition.

Ratchet:

```text
current_dead_field_allowance <= landed_baseline
```

Prefer exact-entry matching plus non-growth if practical.

## 37. Balance revalidation

After `regional_supply` and any other economy knobs become live, run the relevant balance workflows and record before/after price curves and regional variation.

The goal is not a broad economy redesign; it is to prove that the newly live authored knob behaves plausibly and does not destabilize the loop.

## 38. Catalog registry

Update `docs/data/CATALOG_REGISTRY.md` with a consumed-fields representation.

Suggested columns:

| Catalog | Consumer | Consumed fields | Presentation fields | Policy fields | Dead fields | Runtime evidence |
|---|---|---|---|---|---|---|

## 39. 18C DoD

- [ ] field inventory generated,
- [ ] parsed vs behaviorally-used fields distinguished,
- [ ] every field triaged,
- [ ] `regional_supply` changes authoritative prices,
- [ ] UI reads the same authoritative result,
- [ ] WIRE fields have tests and observables,
- [ ] mod contract/schema discipline applied,
- [ ] field-utilization tier runs in data-integrity selftest,
- [ ] dead-field baseline landed,
- [ ] baseline cannot grow silently,
- [ ] balance curves recorded,
- [ ] catalog registry lists consumed fields,
- [ ] dead-fields found/remaining published.

---

## 40. Cross-task dependency graph

```text
15A live choice route
      │
      ▼
18A Echo proof chain
      │
      ├──► reusable wiring pattern
      ▼
18B catalog evidence repair
      │
      ├──► runtime evidence collector
      └──► honest catalog consumption
              │
              ▼
18C field-level integrity
```

Additional dependencies:

```text
Plan 15C liveness gate ──► Echo journal route must be live
Plan 31 semantic kinds ──► Echo day events/briefing compatibility
```

Mandatory order: **18A → 18B → 18C**.

---

## 41. Commit strategy

### C2[3].1 — baseline + verification
- regenerate utilization metrics,
- re-verify audit rows,
- verify echo schema/exemptions.

### C2[3].2 — Echo DTO + loader
- entity,
- loader,
- diagnostics,
- loader tests.

### C2[3].3 — Echo runtime
- availability,
- deterministic selection,
- state/events.

### C2[3].4 — Echo effects + persistence
- existing effect adapter,
- exactly-once,
- SaveStoreHub/registry.

### C2[3].5 — Echo day/UI/audio
- semantic event,
- journal surface,
- existing cue,
- integration tests.

### C2[3].6 — exemption removal + pattern doc
- remove future exemption,
- graph lint,
- document pattern.

**Gate: 18A complete.**

### C2[3].7 — evidence rule + root-array fix
- scanner semantics,
- integrity parity,
- tests.

### C2[3].8 — atmosphere + medical wiring

### C2[3].9 — encounter + memory clusters

### C2[3].10 — moral stubs + small-catalog triage

### C2[3].11 — runtime collector + baseline ratchet

**Gate: 18B complete.**

### C2[3].12 — field inventory + failing utilization test

### C2[3].13 — regional supply wiring

### C2[3].14 — field integrity tier + dead-field baseline

### C2[3].15 — balance + registry closure

**Gate: 18C complete.**

---

## 42. Unified test matrix

| Layer | Required proof |
|---|---|
| data | definitions counted correctly |
| loader | malformed input visible |
| runtime | content reaches selection |
| effect | selection changes authoritative state |
| persistence | state survives save/load |
| idempotence | retry/load does not duplicate effect |
| event | consequence reaches semantic event layer |
| UI | existing live surface presents content |
| audio | feedback exactly once where applicable |
| utilization | runtime collector reaches SELECT/EFFECT |
| exemptions | expired exemptions removed |
| scanner | named-only consumer cannot self-attest |
| root arrays | counts visible to scanner and validator |
| fields | new dead gameplay field fails CI |
| balance | newly live knobs change outputs plausibly |

---

## 43. Failure modes

### Echo loads but never surfaces
Trace day-loop ownership, availability, and selection. Do not fake a preview route to satisfy utilization.

### Echo resolves but no effect occurs
Treat as a hard defect. Fix data or the adapter; unsupported effects must not silently succeed.

### Effect duplicates after load
Ensure restore reconstructs state without replaying one-time resolution side effects.

### Scanner says GAMEPLAY because it named a class
Reject self-attestation; require source/runtime evidence.

### Bare-array catalog still counts zero
Fix the supported-root-shape logic in both scanner and integrity validation, not by exemption.

### Atmosphere text is random decoration
Either bind it to actual weather/environment state or classify it as presentation-only.

### Runtime evidence only exists in synthetic tests
Instrument real campaign boot/use.

### Field gate flags schema metadata
Add narrow POLICY handling; never blanket-exempt gameplay fields.

### `regional_supply` changes a label but not prices
Still dead. The authoritative market output must move.

### Dead-field allow-file grows
CI must fail unless baseline change is explicitly reviewed.

---

## 44. Risk register

| Risk | Impact | Mitigation |
|---|---:|---|
| 18B starts before 18A standard | High | hard sequence gate |
| stale audit drives wrong deletion | High | re-verify source/runtime |
| duplicate narrative runtime | High | repo-wide search before adding EchoSystem |
| duplicate effects resolver | High | adapter to existing applier |
| save replays consequences | High | idempotence + restore tests |
| generic exemption loophole survives | High | legal outcome schema + zero target |
| root-array fix shifts metrics | Medium | explicit before/after repin |
| runtime collector overhead | Medium | stage-transition IDs only |
| decorative content mislabeled effectful | Medium | runtime effect/role tests |
| field scanner false positives | Medium | DTO + raw access + policy evidence |
| published field deleted unsafely | High | mod contract + schema version |
| regional supply destabilizes economy | Medium | before/after balance curves |

---

## 45. Documentation deliverables

Required:

- `docs/narrative/ECHO_WIRING_PATTERN.md`,
- `docs/data/DATA_GAP_AUDIT.md` corrections,
- `artifacts/content-utilization-baseline.json` update,
- `docs/data/DEAD_FIELDS_BASELINE.md`,
- `docs/data/CATALOG_REGISTRY.md` consumed-fields update,
- mod-contract update for public field changes.

---

## 46. Final verification checklist

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --content-utilization-selftest
bash scripts/ci/triad-drift-gate.sh
bash scripts/ci/verify-fast.sh
```

Run narrative graph/continuity gates and relevant balance workflows after 18C.

---

## 47. Flagship Definition of Done

### 18A
- [ ] echoes schema verified,
- [ ] loader/runtime live,
- [ ] deterministic selection,
- [ ] real effect for every choice,
- [ ] existing resolver/effect path reused,
- [ ] exactly-once,
- [ ] persistence,
- [ ] day event,
- [ ] existing journal surface,
- [ ] existing audio cue,
- [ ] exemption removed,
- [ ] runtime evidence,
- [ ] pattern documented.

### 18B
- [ ] scanner evidence rule fixed,
- [ ] root-array blind spot fixed,
- [ ] large catalogs wired/classified,
- [ ] encounter/memory clusters integrated,
- [ ] moral stubs resolved,
- [ ] small catalogs triaged,
- [ ] runtime collector expanded,
- [ ] baseline ratchet landed,
- [ ] `exempt_no_source_evidence = 0`,
- [ ] stale audit repaired.

### 18C
- [ ] field inventory,
- [ ] all fields triaged,
- [ ] `regional_supply` changes prices,
- [ ] field gate in data-integrity selftest,
- [ ] dead-field baseline ratchet,
- [ ] mod/schema safety,
- [ ] balance revalidation,
- [ ] consumed-fields registry.

### Program
- [ ] `EFFECT_PRODUCED` and runtime evidence rise,
- [ ] zero-consumer/dead-content metrics improve or are explicitly deleted/quarantined,
- [ ] no expansion batch increased inert content during execution,
- [ ] full verification green,
- [ ] before/after metrics published.

---

## 48. Closure report template

```markdown
# C2[3] Closure Report

## Repository
- Start commit:
- End commit:
- Branch:

## Before Metrics
- Total catalogs:
- Total definitions:
- Zero-consumer catalogs:
- Zero-consumer definitions:
- EFFECT_PRODUCED:
- Runtime evidence:
- No-source-evidence exemptions:

## 18A Echo Chain
- Schema/count:
- Loader:
- System:
- Selection:
- Effects:
- Save:
- Day event:
- Journal:
- Audio:
- Exemption removed:
- Runtime stages:

## 18B Evidence Repair
- Scanner rule:
- Root-array fix:
- Atmosphere:
- Medical:
- Encounter cluster:
- Memory cluster:
- Moral stubs:
- Small catalogs:
- Runtime collector:
- New baseline:
- Final no-source-evidence count:

## 18C Field Integrity
- Fields scanned:
- Dead fields found:
- WIRE:
- CONSUME-AS-TEXT:
- DELETE:
- POLICY:
- regional_supply result:
- Integrity tier:
- Baseline size:
- Dead fields remaining:
- Balance result:

## Verification
- Core.Tests build:
- Core tests:
- Host build:
- data-integrity:
- bridge-selftest:
- content-utilization:
- triad-drift-gate:
- verify-fast:
- narrative gates:

## Headline After Metrics
- EFFECT_PRODUCED:
- Runtime evidence:
- Zero-consumer definitions:
- Generic exemptions:
- Dead fields:
```

---

## 49. Final Execution Directive

Treat gameplay content as executable design intent, not inventory.

Do not accept:

```text
JSON exists
loader exists
DTO exists
scanner names a consumer
panel can display text
```

as evidence of liveness.

Require:

```text
authored definition
→ real runtime selection
→ authoritative effect or explicit policy role
→ persistence where stateful
→ player-visible consequence
→ runtime evidence
→ CI protection
```

Use `echoes.json` to prove the pattern, apply it to the no-source-evidence backlog, then descend to field-level integrity so dead knobs such as `regional_supply` cannot silently invalidate future balancing work.

The wave is complete when ASHFALL measures **how much content bites**, not merely how much content exists.

---

## Appendix A — Source Plan Traceability

This flagship plan is an expanded integration execution of Plan 18. The authoritative source plan remains the basis for target catalogs, sequencing, guardrails, and verification requirements. Re-verify every historical metric and stale audit claim against the current repository before modifying code.
