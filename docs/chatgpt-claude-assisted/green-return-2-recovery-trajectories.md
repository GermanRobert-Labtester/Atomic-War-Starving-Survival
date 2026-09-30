# GR-2 — Recovery Trajectories and Field Evidence

STATUS: DRAFT — proposal for review; depends on GR-1 premise review; no approval or ownership claim.

## 1. Objective

Describe what the game can responsibly say about change in land condition across time, and how observation improves confidence in that report. This plan is about a **readable trend over canonical facts**. It does not introduce a second environmental simulation, visit ledger, survey reward loop, or explanation of ecological cause.

**Completion means:** a P0 audit establishes which time-stamped observations actually exist, whether they are comparable, and what the player may know. A future implementation may show a trajectory only when two or more valid comparable facts support it; otherwise it must report “unknown” or “single reading.”

## 2. Current Reality

`LocationMutationRecord` supplies a current contamination value, loot depletion, cleared/ruined flags, and `lastVisitedDay`. The live `LocationEvolutionSystem.TickDay` changes contamination according to the day's current weather/radiation input and can recover depletion under a quiet-site rule. The location state contains no series of historic contamination samples, cleanup work log, or field survey record. `lastVisitedDay` is the latest visit timestamp; it is not evidence that the visitor measured contamination.

The phase-4 `EvolvingWorldDayOwner` in `src/Main.CampaignOwners.cs` samples the live weather inputs into the location update once for each campaign day and sends expedition outcomes to `MarkCleared` or `MarkVisited`. It restores pre-day snapshots if day advancement rolls back. No separate Green Return owner exists and none is needed for this plan.

There is an adjacent, persisted evidence pattern in `WildlifeEcosystemSystem`: `WildlifeObservation` records have species, sector, day, and confidence; `RecordObservation` adds observations and bounds the log with `ObservationLogCapacity`. This is wildlife knowledge, not land survey evidence, and must not be repurposed as a generic visit or soil log. Its existence proves only that domain-specific observation persistence is possible under an existing owner.

## 3. Required Delta

Current values support “current measured/simulated condition” but not a reliable time series. The minimum delta is to define trajectory semantics over facts already retained—or conclude that there is not enough history for a trajectory. The plan asks for three user-facing states only after evidence review:

- **improving:** comparable readings over a defined interval move toward lower contamination or disturbance;
- **stable:** comparable readings remain within a declared tolerance;
- **worsening:** comparable readings move away from the reference condition;
- **unknown / insufficient evidence:** no pair of comparable readings, stale source, or invalid data.

These names are candidates, not a committed ecological claim. “Improving” must describe only the measured dimension. A falling contamination number does not prove biodiversity, crop health, safe drinking water, or recovered soil.

## 4. Evidence

Snapshot date: 2026-09-29. All details require a current P0 re-check before implementation.

| Claim | Source evidence | Constraint |
|---|---|---|
| Location owner stores latest state, not a full history | `LocationMutationRecord` in `Assets/Ashfall.Core/LocationEvolutionSystem.cs` | Do not derive historical trend from `lastVisitedDay`. |
| Environmental changes are tick-driven and weather-informed | `LocationEvolutionInputs` and `TickDay` in `LocationEvolutionSystem.Live.cs` | Inputs arrive as a daily value; this file does not persist each daily sample. |
| One campaign-day order is the active route | phase-4 `EvolvingWorldDayOwner` in `src/Main.CampaignOwners.cs` | No second tick, observation cadence, or scheduler. |
| Wildlife observations have their own schema and retention | `WildlifeEcosystemSystem.RecordObservation`, `WildlifeObservation`, `ObservationLogCapacity` | Wildlife knowledge is not land measurement and stays in that owner. |
| Existing panel/map uses location state for selected outcomes | `Main.EvolvingWorld.cs`, `WastelandMapView`, `ExpeditionPanel` | UI reports must use a real location source and preserve unknown. |
| Reconstruction Tree concerns teaching/recovered knowledge | `.ai/plans/reconstruction-tree-2026-09-29.md` | Research completion cannot stand in for a measurement. |

P0 should search data and the whole source tree for any already-live site survey, measurement instrument, contamination sample, or expedition observation event. Content mention alone is not a persistent observation API.

## 5. Existing Extension Seams

Use the location owner's current state and the existing campaign day. An existing survey, expedition debrief, radiation measurement, or event log may provide a genuine sample only if it has a stable location ID, day, measured dimension, source, and restore path. If none exists, a trend cannot honestly be computed without additional history.

P0 should evaluate three non-overlapping options:

1. **No new history:** present only current level and date, with no trajectory label.
2. **Owner-approved compact baseline:** persist only a baseline/previous comparable measurement inside the location owner, if the owner accepts the data and its migration contract.
3. **Existing instrument owner:** read a current, already-persisted location observation seam and derive a trend without copying its data.

The preferred solution is the smallest option that supports the intended player interpretation. Option 2 is an architecture change: it cannot be assumed merely because it seems small.

## 6. Proposed Architecture

### 6.1 Observation before trajectory

Each reading must answer: what was measured; at which canonical location; on which campaign day; by which existing owner/source; with what quality; and whether it is comparable to the previous reading. A narrative line, map visit, expedition completion, or `lastVisitedDay` alone is not a measurement.

### 6.2 Trend is derived

The trajectory projector should be pure: accept two or more canonical observations (or a documented baseline plus current fact) and return direction, dimension, interval, and evidence grade. It does not update measurements or influence `LocationEvolutionSystem`.

### 6.3 Confidence is bounded by source

The UI should not turn an estimate into a fact. A firsthand sample can be higher confidence than an old secondhand report only if the existing data actually distinguishes those qualities. Do not invent “heard / told / seen” values here; that grading belongs to any owner that already defines it. Unknown is preferable to a fabricated confidence score.

### 6.4 No unearned causality

The projection can say that contamination decreased after a clear-weather interval if source facts support it; it cannot claim that a steward's work, a species return, or a research node caused the change unless a canonical causal event exists. Correlation is not intervention credit.

## 7. Ownership Matrix

| Concern | Owner | GR-2 boundary |
|---|---|---|
| Current location environmental facts | `LocationEvolutionSystem` | Read-only; possible approved baseline extension only after P0 |
| Historical wildlife sightings | `WildlifeEcosystemSystem` | Do not borrow or duplicate |
| Weather, radiation, day | Existing weather/campaign-day owners | Read existing snapshots only |
| Expedition phase/outcome | Existing expedition engine and day owner | Completion/visit is not a survey unless a measurement is already emitted |
| Crop state | `AgricultureSystem` | No yield-based proxy for wild land recovery |
| Knowledge unlocks | Reconstruction Tree's existing research owners | No unlock, progress, or restored manual state here |
| Display | Existing location/map surface | Read-only projection; no cached mutable trend state |

## 8. Data Flow

If sufficient observations already exist: source owner creates measurement → existing persistence captures it → GR-2 obtains a stable read snapshot → validator rejects non-comparable or invalid samples → pure projector derives dimension-specific direction and interval → current location detail presents source day and confidence.

If they do not exist: location owner current state → current-state projection only → display explicitly reports no trajectory. A newly proposed sample path must identify who creates the measurement and what action makes it valid before code is planned. Do not fabricate measurements when a map opens.

## 9. State Model

No new state is the baseline. If P0 justifies a persisted baseline, the owning team must decide the smallest shape before implementation. Candidates include one last comparable reading per dimension, or a bounded observation list if an existing contract already needs multiple samples. Do not select a structure here.

Any accepted state must satisfy:

- one canonical location ID and one owner per observation;
- campaign day is monotonic within a campaign, with duplicate-day samples handled idempotently;
- measurements have explicit dimension and units/range;
- observation provenance is immutable after commit or amended through an explicit owner command;
- sample ordering is deterministic by day then stable ID;
- history has a justified retention bound and dropping history cannot silently rewrite the current measurement;
- `CaptureState` deep-clones mutable collections and `RestoreState` treats legacy absent fields as empty/unknown.

No parallel `LandSurveyState` save store is proposed. If the canonical owner rejects the added state, stop and retain current-only reporting.

## 10. API / Contracts

No concrete API is approved. The future contract should avoid leaking mutable DTO references. A candidate transient result would contain dimension, trajectory (`Unknown`, `Improving`, `Stable`, `Worsening`), observation start/end day, evidence quality, and reason when unknown. The observation source contract must be owned by the actual instrument/expedition/location owner; a generic global observation bus is unjustified for this single feature.

The projector needs documented comparability rules: exact dimension and unit match, ordered days, allowed maximum age/gap, and a tolerance for stable. These are design and balance decisions, not values to infer from available constants. No wall-clock date or locale string can enter the contract.

## 11. Data Changes

No data change is proposed before P0. Inspect current survey/exploration catalogs and their loader/validator pairs. If thresholds/tolerances are authored, they require schema version, snake_case IDs, range validation, duplicate/reference checks, a real Core loader, and an explicit missing-catalog behavior. If the only values are three generic direction labels and signed thresholds, embedding them in a pure projector may be clearer; do not create a data file for decorative configuration.

Any field-note text must be authored separately from numeric conditions and only use facts that the reader can cite. Do not generate one line per possible reading if a compact template works, and do not repeat generic caveats throughout content rows.

## 12. Save / Load

Derived trajectories are never serialized. If they depend on existing observations, recompute after restore. If an accepted baseline is required, extend exactly one canonical location save owner only after approved migration design; the default for legacy saves is insufficient history, not “stable” or “healthy.” Capture/restore must preserve duplicate-day semantics and not append a second sample while restoring.

Before implementation, inspect `WorldSaveStore`, `WorldHostSession`, the save-section registration for world state, and restore ordering. Existing per-owner save may already wrap `LocationEvolutionSaveState`; do not register a second section. Any checksum or schema count change must be owned by the integrator and the generated save-store matrix workflow. No hand-edited generated output.

Required persistence tests, if new state is approved: new-format round trip; legacy absent-field load; deep-copy isolation; duplicate observation restore; same-day replay; corrupted payload follows existing save recovery policy; and old save does not claim a trend until a second valid observation exists.

## 13. Determinism

Trajectory math needs no RNG. Stable ordering is by campaign day, then ordinal ID. Avoid unordered dictionary iteration, floating point text comparisons, wall-clock timestamps, localized parsing, or hash-derived IDs. A deterministic replay with the same observation facts must yield the same trajectory; a save/reload between observations must not duplicate or advance the series.

Do not call `LocationEvolutionSystem.TickDay` from the projection. Do not recompute a hypothetical history from today's weather, because weather has moved on and the prior daily inputs are not a time series in this owner.

## 14. System / Event Wiring

The existing phase-4 owner advances location and wildlife facts and handles expedition consequences. GR-2 adds no owner and should read only after that owner has committed a day. If a new field observation comes from an expedition, P0 must place it at the canonical completion point and prove it runs once under rollback/retry. An expedition's own exactly-once identifier should be reused if applicable; do not add a GR-specific processed-expedition set.

Any mutation or observation event must have an exactly-once behavior, day ordering, and rollback semantics defined at its current owner. An event subscriber that appends a trend sample before the source owner can roll back is unsafe. If there is no committed-event seam, defer the observation integration.

## 15. Godot Integration

An existing selected-location detail is the candidate surface, subject to P0 route audit. The readout should say “contamination trend: unknown; one current reading” when evidence is insufficient, or give direction, dimension, and the endpoints/days when supported. A trend icon alone is inadequate. No tooltip may state that the land or ecosystem is “healing” based only on one contamination value.

UI should not store trend history or calculate the slope. Refresh only on the existing route's state-change/selection lifecycle; release subscriptions on exit. Preserve controller focus and back navigation. Do not change WastelandMapView's danger ladder as a side effect of this readout.

## 16. Narrative / Content Integration

Field notes are optional flavor attached to an already-valid evidence result. Keep them factual and dimension-bounded. Examples of acceptable tone include “The meter reads lower than the last recorded visit” or “No comparable reading on file.” Avoid implying which action caused a change unless the source owner recorded it. Do not unlock Reconstruction Tree nodes, radio knowledge, wildlife observations, or journal entries from a passive map display.

## 17. Failure Modes

| Case | Behavior |
|---|---|
| Only one valid reading | Unknown trend / single reading; show current value only. |
| No survey API exists | Keep current-only view; record as blocker for any requested field-report feature. |
| `lastVisitedDay` is newer than sample | Visit freshness and measurement freshness remain distinct. |
| Different dimensions/units | Not comparable; do not compute delta. |
| Same-day duplicate | Idempotent source key or reject; never count twice. |
| Reversed day order | Reject as invalid; do not sort away a corrupted ownership sequence unless contract explicitly allows it. |
| Stale sample gap | Unknown or stale per signed rule; do not extrapolate. |
| Missing legacy history | Unknown until enough post-upgrade evidence exists. |
| Missing catalog/threshold | No classification beyond current raw fact. |
| Invalid confidence/range | Unknown and report validation issue; never clamp into high-confidence result. |
| Restore/day rollback overlap | Snapshot rollback removes uncommitted sample with source owner; retries remain idempotent. |
| UI refresh during day tick | Show last committed view or defer refresh. |

## 18. Test Strategy

No tests are run for this draft. In an approved package, use existing tests before adding new ones and run only targets for changed files through `bin/run-scoped-tests`.

- projector table tests: insufficient samples, improvement, stable tolerance boundaries, worsening, stale gap, invalid range, mismatched dimension;
- deterministic order and repeated-call purity tests;
- observation owner tests: commit-once, duplicate-day, rollback/retry, deep capture/restore;
- save compatibility: old state defaults to no trend; new state round-trips without growth on restore;
- host test proves expedition completion or survey action uses the source owner exactly once;
- UI provider test proves unknown state is visible and no data mutation occurs on open/refresh;
- headless runtime only if host route is changed.

Do not add paired tests for both the same DTO and projector unless each protects a distinct contract. Do not run broad world tests by default.

## 19. Dependency-Ordered Phases

### Phase 0 — observation-source census

Read current location, survey, expedition, exploration, weather, wildlife observation, save, and UI sources. Find every `lastVisitedDay` writer and determine whether any caller actually records a measured land value. Audit Second Nature, Living Region, Reconstruction Tree, and prior expansion material for a duplicate baseline/trend proposal.

**Gate:** a table lists each existing observation with fields, owner, persistence, and exact reader. If no comparable site sample exists, only the current-state label remains in scope.

### Phase 1 — evidence semantics

Define measurable dimension, provenance, freshness, confidence, comparability, same-day behavior, and minimum sample count. Decide whether existing evidence suffices or a canonical owner must retain one extra fact.

**Gate:** reviewers accept the semantics and legacy-save behavior. Any unresolved ownership goes to foreman; no implementation workaround.

### Phase 2 — Core trend contract

If sufficient data exists, implement the pure projection in the existing owning module or the smallest justified Core file. If persistence is approved, update its one DTO/owner and migration path here only after an explicit approved implementation package.

**Gate:** unit and persistence test plan is accepted; no simulation math or wildlife state is duplicated.

### Phase 3 — host observation binding

Only connect a genuine existing survey source. Keep expedition completion, wildlife sighting, and map visit distinct unless the owning systems emit a specifically defined sample.

**Gate:** exact-once behavior through rollback and retry is demonstrated in a focused host test.

### Phase 4 — presentation

Add the readout to an audited selected-location surface. Bind all values through a read provider; no UI thresholds, event writes, or state cache.

**Gate:** focused provider/UI review, accessibility checks, and refresh/disposal behavior pass.

### Phase 5 — scoped verification and handoff

Run only directly affected targets, report exact command/output, save compatibility, open blockers, and untouched shared paths.

**Gate:** no trend is shown beyond the evidence; open architecture decisions remain explicit.

## 20. File Impact Map

All entries are proposed areas, not claims. Re-audit before assigning ownership.

| File / area | Action | Reason | Risk |
|---|---|---|---|
| `Assets/Ashfall.Core/LocationEvolutionSystem.cs` | READ ONLY; conditional MODIFY | Current per-location owner; possible baseline only if signed | High: save contract |
| `Assets/Ashfall.Core/LocationEvolutionSystem.Live.cs` | READ ONLY | Canonical day change, not historical observation storage | High: runtime behavior |
| `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` | READ ONLY | Verify domain-specific observation contract; never generalize implicitly | Medium: save/history boundary |
| Expedition observation/host files | READ ONLY; conditional MODIFY | Candidate real-world sampling seam | High: exactly-once day lifecycle |
| `src/Main.CampaignOwners.cs` | READ ONLY; conditional integrator edit only | Phase-4 committed event order | High shared path |
| `src/World/WastelandMapView.cs` / selected-location surface | READ ONLY; conditional MODIFY | Candidate display | Medium: route/accessibility |
| `WorldSaveStore`, `WorldHostSession`, save registry | READ ONLY; conditional owner edit | Locate existing persistence; change only for signed state | High: migration/checksum |
| Existing tests | READ ONLY; conditional focused test | Cover new contract only | Low/medium |
| New generic observation service | NO CHANGE | One feature does not justify generic framework | High duplication risk |

## 21. Risks

1. **Invented longitudinal knowledge:** latest value plus visit day looks like a trend but is not one. The P0 source census is a hard gate.
2. **False causality:** successive readings cannot attribute improvement to stewardship without a canonical action/result link.
3. **Unbounded save growth:** storing every daily sample is unnecessary and risky. Prefer no new history or a bounded owner-approved fact.
4. **Cross-domain leakage:** wildlife observation logs include sector/species knowledge. Reusing them as land surveys would violate ownership and misstate evidence.
5. **Rollback duplication:** day owner retries may replay an observation. Source identity and transaction boundary must be explicit.
6. **Stale confidence:** later day advancement does not make a previous measurement current. Display source day and stale/unknown status.
7. **Overlapping plans:** wildlife return pressure and food web belong to Second Nature; settlement health and regional pressure to Living Region; restored manuals and knowledge to Reconstruction Tree.

## 22. Out of Scope

No daily sample logger, new generic observation bus, exploration reward ledger, survey minigame, instrument inventory, land restoration quest, contamination cure, wildlife census, farming yield indicator, research unlock, regional population or settlement condition. No retuning contamination drift, changing day order, or converting wildlife sightings into new land effects.

## 23. Rollback Strategy

For a pure projector or readout, rollback removes the additive projection and UI binding; persistent world state stays untouched. If an owner-approved baseline was added, retain backward-compatible deserialization and make absence mean “no trend.” Reverting the feature must not delete existing location or wildlife observations. Revert any authored thresholds and their loader together. If migration cannot roll back without discarding legitimate facts, the state proposal must return to review before implementation.

## 24. Definition of Done

- P0 identifies existing observation APIs and distinguishes them from visits, rumors, and narrative content.
- Trend can be derived only from comparable, dated source facts; no sample means no trend.
- At most one canonical owner persists each observation/baseline.
- Restore, rollback, duplicate day, retention, and legacy behavior are specified and covered if state changes.
- Trajectory math is pure, deterministic, dimension-specific, and does not attribute cause.
- Existing route displays source/freshness and unknown without mutating on open.
- Changed targets pass focused scoped verification; no full suite unless the exact user phrase authorizes it.

## 25. Implementation Handoff

### MUST PRESERVE

The location owner and phase-4 day route; the wildlife owner's distinct observation model; explicit separation between visit, measurement, and rumor; truthful unknown behavior.

### MUST ADD

Only a pure dimension-specific trend projection if current evidence supports it; only a bounded persisted baseline if the canonical owner and integrator approve the schema and restore path; a visible readout through an existing location surface if verified.

### MUST NOT DO

Infer history from `lastVisitedDay`; replay weather; create a new visit/survey ledger or generic observation bus; copy wildlife observation state; add a second day owner; imply ecological safety, crop health, or causality; change wildlife or knowledge progression.

### VERIFY WITH

Focused Core/persistence/provider targets through `bin/run-scoped-tests`, selected from changed paths under the current policy. Include Godot headless only for a changed runtime path. Report exact commands; no broad run by default.

### FIRST SAFE IMPLEMENTATION STEP

Produce a read-only observation-source matrix with exact APIs, persistence owner, day semantics, and comparable measured fields. If that matrix cannot identify two valid land readings, stop at a current-state-only display proposal.

## Appendix A — Candidate evidence source matrix

This matrix distinguishes sources that can look like a land observation from sources that actually prove one. P0 must replace assumptions with exact current call paths.

| Source candidate | Facts visible now | What it may prove | What it cannot prove without another API |
|---|---|---|---|
| Location daily tick | Input includes outdoor radiation modifier and hazard-weather boolean; record retains current contamination | The canonical location owner updated its state on a campaign day | A historical series, direct measurement, causal treatment result, field confidence, or exact radiation dose at a site |
| `lastVisitedDay` | Most recent location visit day | A visit-like event was recorded through `MarkVisited` or `MarkCleared` | Visitor identity, equipment, measured dimension, observation quality, or that the site was surveyed |
| Expedition completion | Phase-4 owner recognizes complete/failed outcome and updates clear/visit/threat facts | The sortie has a committed outcome affecting location owner | A contamination measurement, exact work performed, or ecological sampling |
| World evolution event | Authored trigger day, target ID, mutation deltas/threats, event prose | A catalog event was applied exactly once according to its owner | That event prose is an independent sensor reading or that a player action caused later change |
| Wildlife observation | Species, sector, day, confidence in `WildlifeObservation` | Wildlife owner has a reported species/sector sighting | Soil condition, precise location, population census, or comprehensive biodiversity |
| Greenhouse soil state | Managed plot soil/fertility inputs and evaluation result in the farming path | The measured/authored plot's cultivated soil evaluation | A wilderness or map location's soil condition, safe wild planting, or regional recovery |
| Location narrative | Authored description, quest text, event prose | Fictional context for the content | Live value, current trend, visit history, or confidence |
| Map selection | Selected canonical node ID | The player opened or highlighted a location | Any new environmental knowledge or valid observation |

### Required observation identity

If P0 discovers an existing land survey, the following fields must be accounted for before using it:

| Attribute | Why it matters | Acceptable minimum |
|---|---|---|
| Canonical subject ID | Prevents cross-location aggregation by display name | Stable location ID, or explicit sector ID with sector-level UI |
| Measurement dimension | Prevents comparing unlike values | Named dimension and units |
| Campaign day | Makes temporal order deterministic | Non-negative campaign day from canonical clock |
| Source | Distinguishes meter reading from event-derived state | Existing owner/event ID, not freeform UI label |
| Value and bounds | Ensures comparison has defined meaning | Range/finite-value validation at source or loader |
| Quality/confidence | Stops weak evidence from appearing definitive | Existing domain field or explicitly unknown |
| Identity/idempotency | Prevents repeated event from adding duplicate sample | Existing owner key or deterministic source ID |
| Persistence owner | Makes restore/replay meaningful | Current save section and capture/restore path |

If two sources differ in any required field, they are not automatically comparable. A single “confidence” float cannot repair an unknown unit or location mapping.

## Appendix B — Worked evidence outcomes

### Scenario 1: one seeded or first-created location record

The user selects a named location whose current `LocationMutationRecord` has a contamination value but no historical sample. The readout can show that one current owner value and identify its source day only if that day is available. Trend remains `unknown / one current state`. The system must not backfill earlier samples from the current weather or fabricate them from the seed catalog; seed values are initial conditions, not readings collected by the player.

### Scenario 2: repeated clear-weather ticks

The weather owner sends clear conditions on consecutive campaign days. The location owner reduces positive contamination each day by its clear decay rule. The live save retains the current resulting contamination and latest evolution day, but not each intermediate daily value. A trend could be mathematically reconstructed if exact prior state, every day, every input, every event, and every replay were retained; those prerequisites are not established by GR-2 and would amount to reproducing the simulation. This plan rejects that reconstruction as a substitute for observations.

If an approved owner stores only a baseline and current reading, it must be explicit whether the output describes “recorded change since baseline” or an estimated rate. This plan prefers the first; a rate adds assumptions about days with missing samples.

### Scenario 3: hazard event and weather tick on same day

`WorldEvolutionEngine` has an authored event that applies a contamination delta, while `LocationEvolutionSystem.TickDay` also changes contamination from daily weather. P0 must determine exact ordering and transaction boundary in the live host. A valid post-day snapshot may reflect both effects, but a single resulting value does not isolate either cause. The UI should not say “weather raised” or “event cleared” unless the canonical event record preserves that causal distinction and its restore behavior.

### Scenario 4: location visited after a long gap

`MarkVisited` writes `lastVisitedDay`. If no sensor action or measurement API accompanies the visit, the new visit day only updates contact/freshness context; it cannot refresh a measurement. The report can say “visited on day N; land sample not recorded.” A newer visit must not reset a stale sample's age.

### Scenario 5: an expedition fails

The current host marks the location visited and may add a threat after a failed sortie. This may make the site less safe to access, but it does not constitute a worsening environmental measurement. The trajectory remains whatever valid land readings support; the threat state can be presented separately by its owner.

### Scenario 6: fauna sighting at a sector adjacent to a location

Wildlife observation includes species and sector, not named-site sampling. Even a high-confidence observation does not become a land sample. The map can show the wildlife owner’s sighting at sector precision where an existing consumer permits; the land trend stays unknown unless its own evidence exists. This is the main guard against using biodiversity as a proxy for low contamination or vice versa.

### Scenario 7: future sample survives restore

If an approved owner records a sample at day N, saves, restores, and replays day N, the sample must remain one entry. A later valid sample at N+1 can then produce a trend. If restore duplicates day N, the owner needs a stable key or append guard before any presentation work proceeds. A duplicate log entry is not just a UI nuisance; it can shift confidence/retention thresholds and alter story unlocks.

## Appendix C — Trajectory contract decision table

These are questions for design review, not thresholds to silently choose during implementation.

| Decision | Option A | Option B | Current recommendation | Evidence needed to change it |
|---|---|---|---|---|
| Minimum readings | one baseline plus current | two observations from an explicit sample owner | Require two comparable records; do not count a seed as player observation by default | Source API and product intent |
| Stable tolerance | dimension-specific tolerance | exact equality | Defer until units/ranges are known | Current domain measurement semantics |
| Maximum sample age | fixed authored days | source-specific freshness window | Show age and mark stale; avoid a global value without owner input | Clock cadence and source reliability |
| Sample cadence | daily | action/event-driven | Prefer existing event-driven sample; do not add daily history by default | Existing capture trigger and save size |
| Missing day | interpolate | preserve gap | Preserve gap; no interpolation in first version | Validated simulation/measurement policy |
| Measurement correction | overwrite | append correction with provenance | Use current source's correction convention; never erase silently | Existing source edit model |
| Confidence | one universal score | preserve source-specific grade | Preserve source vocabulary; no new generic score | Source owner and UI needs |
| Causal language | infer from time order | report association only | Association only unless event owner names a direct effect | Canonical cause record |

## Appendix D — Acceptance examples by owner

| Owner boundary | Passing observation | Failing observation |
|---|---|---|
| Location state | Projection reports restored current contamination with source freshness | Presenter writes a missing record or resets a field while calculating |
| Weather/day | Day N value is attributed only to source facts that exist | UI replays weather history that was never saved |
| Expedition | Completion emits one documented measurement through its owner, if such command exists | Any completed mission automatically counts as a clean survey |
| Wildlife | Bestiary observation remains species/sector evidence | Fauna sighting increments a Green Return sample count |
| Soil reclamation | Greenhouse evaluation remains plot-level | Its fertility tier is copied into wilderness condition |
| Save | Legacy state shows no trend; new history round-trips exactly once | Old save receives a fabricated baseline or restore appends a sample |
| UI | Unknown and stale are legible and source day is shown | A colored upward arrow appears with no evidence details |

## Appendix E — Candidate input/output contract

All field names and numeric examples below are **PROPOSED / VERIFY**. The plan does not assert an existing land-observation API. This contract becomes implementable only after P0 names one.

### E.1 Candidate observation value

An observation suitable for trajectory comparison would minimally include:

| Field | Candidate type | Required validation | Notes |
|---|---|---|---|
| `subject_id` | stable string | resolves to one canonical location or sector | Never mix site and sector precision |
| `dimension_id` | stable enum/string | known measurement domain | e.g. contamination vs water clarity are incomparable |
| `value` | finite numeric value or owner-native fixed-point integer | within source-specific bounds | Preserve canonical unit and precision |
| `unit_id` | stable unit key | one agreed unit per dimension | Do not convert implicitly in UI |
| `campaign_day` | nonnegative integer | source clock; no future day | Equal-day samples require source policy |
| `source_owner_id` | stable key | known current owner/event | Prevent prose from posing as sensor |
| `source_record_id` | stable key | unique/idempotent | Used to deduplicate replay |
| `quality` | existing source enum or unknown | valid source range | Do not invent quality from visit count |

The Core trend projector should accept observations as immutable values and return a separate output. It should not inspect filesystem, call `Main`, access global campaign state, or retrieve data through mutable singletons. The host is responsible for supplying values from the canonical owner at a safe commit boundary.

### E.2 Candidate projection output

| Field | Candidate meaning | Required rule |
|---|---|---|
| `status` | insufficient, comparable, invalid, stale | Distinguish lack of data from a stable trend |
| `dimension` | observed dimension | Preserve source dimension; no aggregate “nature health” |
| `direction` | improving, stable, worsening, unknown | “Improving” is direction relative to dimension semantics, not always lower numeric value |
| `from_day`, `to_day` | actual evidence interval | No synthetic interval from first boot date |
| `from_value`, `to_value` | endpoints used | Display only if product needs exact values and units are legible |
| `quality` | source quality or conservative combined grade | Must be defined by source owner; no average-confidence trick |
| `reason` | why no conclusion is available | Stable enum; examples: one sample, stale, different unit |

If the UI only needs a current value and a direction icon, it should still receive the status/reason; otherwise the presenter risks displaying a trend when the projector did not establish one.

## Appendix F — Numeric trajectory examples

The examples are **PROPOSED / VERIFY** illustrations to expose design questions. They do not set game balance or provide an implementation threshold.

Assume hypothetically that an approved land sensor reports normalized contamination `0.00–1.00` and that smaller values mean lower contamination. If two valid samples at days 40 and 50 are `0.62` and `0.54`, the raw delta is `-0.08`. A proposed output could be “contamination reading decreased over 10 days.” It could not say “the land recovered,” “the site is safe,” or “stewardship worked.” Those broader statements need separate owners and criteria.

If the same hypothetical source reports `0.62` then `0.619`, the numeric change may be within instrument noise. A stable tolerance such as `±0.02` is only an example and must be **PROPOSED / VERIFY** against source resolution. Without a defined tolerance, exact equality would turn sensor noise into alternating up/down arrows. Conversely, an overly wide tolerance hides meaningful change.

Potential rule forms for review:

| Candidate rule | Example | Benefit | Risk / required evidence |
|---|---|---|---|
| Exact sign of delta | `new < old` is improving | Simple and transparent | No measurement-noise model; UI may flicker between tiny deltas |
| Fixed absolute tolerance | `delta < -ε` improves; `|delta| ≤ ε` stable | Easy to explain | Must be calibrated to source units/resolution |
| Source-defined tolerance | Owner reports whether movement is meaningful | Semantically closest to instrument | Requires an existing source contract; do not invent a parallel one |
| Multi-sample monotonic rule | Require repeated movement over three samples | Resists single noisy observation | Needs more persisted history and delays feedback |

No smoothing window is approved. If designers prefer multi-sample confirmation, first prove that the owner retains those samples or that an existing event log can supply them after restore. A projection cannot safely pretend that missing intermediate days were flat.

### F.1 Boundary tests to define after source approval

Let `ε` mean the accepted source tolerance, not a hardcoded value in this draft. Tests should cover:

| Old/new relation | Expected state after the rule is signed |
|---|---|
| Exact same value | Stable, if exact equality is the selected rule |
| Delta exactly `+ε` / `-ε` | Explicit inclusive/exclusive behavior |
| Just inside tolerance | Stable under fixed tolerance |
| Just outside tolerance | Improving/worsening according to dimension direction |
| Reversed days | Invalid or rejected according to owner order contract |
| Equal days, same source ID | Duplicate/idempotent, not a second sample |
| Equal days, different source IDs | Source-specific policy; do not silently choose last list item |
| Very large gap | Stale/insufficient if freshness rule says so |
| Dimension matches but unit differs | Not comparable |
| Value is NaN/infinite/out of range | Invalid evidence, not a capped reading |

Tests should derive examples around the real source's range and unit. The numbers above are only templates; do not copy them into production JSON or source as thresholds.

## Appendix G — Day, save, and replay traces

### G.1 Existing current-state tick is not a sample stream

Day N calls the location tick with the day's weather input. The input is used to mutate a current contamination field. The location save retains that resulting current state and `lastEvolutionDay`; the inspected state type does not retain the weather sample for each previous day. A future trend feature must not repeatedly capture the same current value on each day and call those distinct measurements. That would manufacture a time series from simulation outputs without provenance.

Also note that the location tick's current overload writes `lastEvolutionDay` but is not itself an observed same-day guard in the inspected implementation. The host/day owner is the intended exactly-once coordinator. If a future observation is emitted from this path, the implementer must prove its event runs once in the host transaction and rolls back with location state; GR-2 must not mask duplicate tick calls with an observation-local guard.

### G.2 First measurement after upgrading an old save

1. Restore legacy `LocationEvolutionSaveState` with its current values and no GR-2 history.
2. The presenter shows current condition if available and “trend unavailable; no prior comparable reading.”
3. An approved real survey action records sample `S1` through its canonical owner.
4. A save/reload preserves `S1` exactly once.
5. A later distinct survey records `S2`; only now may the projector compare them.

Do not treat the legacy current value as `S0` unless the owner can prove its source day, dimension, and comparability. Backfilling `S0` on load is a migration and must be explicit, stable under repeated restore, and tested as such.

### G.3 Save between two observations

At campaign day N a legitimate source records `S1`. The campaign saves. After restart the observation collection must restore with the same stable ID and sequence. At day N+1, the source records `S2`. The result uses `(S1,S2)`. If save restore runs the record callback a second time, source key deduplication prevents `(S1,S1,S2)`. The UI cannot repair duplicated persisted inputs by grouping equal display values; two legitimate measurements can have equal numbers.

### G.4 Day rollback after observation

The source action commits a measurement during day advancement, then another owner fails and day rollback restores the pre-day snapshot. On retry, the same deterministic action key is processed. The final saved observation list has one row for that committed action and none for the rolled-back attempt. A map-side cache of the rolled-back reading is invalidated by owner refresh. If the source and location owners do not share rollback atomicity, this integration stops.

### G.5 Out-of-order import or late radio report

A field report from day N arrives after a report from day N+2. The project must decide whether late measurements are insertable, whether they revise a prior trend, and how the player sees correction. No stable ordering can be inferred from arrival order. If this feature has only local immediate observations, late import can remain out of scope; if remote reporting is intended, it requires a source owner and correction contract before UI is planned.

### G.6 Replaying the same campaign seed

The projection itself consumes no RNG. For identical persisted observation facts, it returns identical output. A test should hash or compare the observation owner state and the projection result across a paired deterministic run, but only if a new sample path is implemented. Do not add new random survey quality rolls without an existing command contract and a named campaign stream.

## Appendix H — History storage alternatives and rejection criteria

| Storage choice | Fit | Why it may be rejected |
|---|---|---|
| No history | Best when only current map condition is required | Cannot show trajectory, so UI must state that plainly |
| One baseline plus latest | Compact, supports “changed since baseline” | Baseline replacement rules and comparability need owner authority |
| Fixed-size recent window | Supports smoothing and recent direction | Adds retention/migration semantics and can drop evidence silently |
| Full history | Good for audit/story chronology | Potentially unbounded save growth; likely unnecessary for a compact map readout |
| Reconstruct from deterministic simulation | Avoids storing samples in theory | Requires every past input, event order, versioned rule, seed stream and migration compatibility; not established |
| Reuse wildlife observation list | No | Different subject, dimension, owner and confidence meaning |
| Store in panel/provider | No | Not durable, duplicated, and easily stale after restore |

Default recommendation: no new history until product requires a trend and P0 finds a legitimate measurement source. If one extra fact is required, prefer the smallest owner-approved bounded shape, but do not choose it before source provenance is known.

## Appendix I — Focused validation design

The future package should map each risk to the smallest changed target:

| Change slice | Focused check | Evidence protected |
|---|---|---|
| Pure projector | Core table test target | Dimension, boundary, stale/unknown rules |
| Observation append/dedupe | Source owner tests | Same action cannot write twice |
| Capture/restore DTO | Owner save tests | Deep clone, legacy defaults, stable ID |
| Host event bridge | Host integration target | Only committed owner events become samples |
| Day rollback | One targeted day-order test if absent | No ghost observation or duplicate after retry |
| UI provider | Provider/UI test | No writes on selection or refresh; unknown is visible |
| Runtime route | Narrow Godot headless probe if changed | Bind/unbind, focus, selected location correctness |

Before adding a target, search for equivalent tests. Do not write both a projector and a host test for the same pure comparison. Run through `bin/run-scoped-tests`, record exact selector/command and outcome, and stop if the scoped runner rejects an excluded target. A full suite is never implied by this plan.

These examples define review behavior without forcing a new observation architecture. If implementation cannot meet the pass examples using current source seams, the correct outcome is a recorded blocker and revised plan, not a hidden cache.

## Appendix J — Existing repeat-pass systems and what their trends mean

The duplicate audit found two established sources that must be checked before anyone proposes a second survey store. They answer different questions from Green Return and therefore do not, by themselves, satisfy its missing land-condition history.

| Existing system | Concrete evidence | What its trend can support | Why it is not ecological recovery evidence |
|---|---|---|---|
| `InSarDeformationEngine` (`Assets/Ashfall.Core/World/InSarDeformationEngine.cs`) | `RecordSurveyPass` retains bounded `SurveyPass` rows; `ProcessSector` requires two compatible reference-geometry passes with positive day span, computes coherence, relative displacement in mm, velocity, classification, confidence, and last processed day. `MaxPasses` is 256. | A deformation summary for a sector, including low-coherence/insufficient-pass refusal, stable or subsidence classification, displacement, and travel/excavation warnings. | The measured dimension is relative ground displacement from phase/backscatter quality. There is no land contamination, biomass, soil fertility, species presence, water safety, or recovery-cause field in that contract. |
| `InSarMappingHostSession` (`src/Host/InSarMappingHostSession.cs`) and `Main.InSarMapping.cs` | Host `RecordSurveyPass` supplies day and quality inputs; `ProcessSector` supplies processing skill and terrain tags. The main route exposes the survey and process calls. `insar_deformation` is registered in the save section registry. | Persisted repeat-pass evidence and a deterministic/seeded processing path under the InSAR owner. | A sector-scale deformation observation cannot be re-labeled as a location contamination sample or proof that land is biologically recovering. |
| `CartographySystem.ProjectCanonicalMap` (`Assets/Ashfall.Core/Exploration/CartographySystem.cs`) | Returns `CanonicalMapSurvey` rows with node ID, display name, fog state, survey quality/tier, last-confirmed day, provenance source, and knowledge-source kind. | What map nodes are rumored, surveyed, or visited and how current map knowledge is sourced. | Map knowledge and visit status describe discovery/provenance, not a physical environmental measurement. A surveyed node is not a soil reading. |

This audit sharpens the earlier extension options. InSAR is a real repeat-pass time series, but its unit and result vocabulary are deformation-specific. GR-2 must neither copy its passes nor cast its output into a Green Return `improving` label. It can be linked as a separately named geophysical context row only if the existing UI route already supports that meaning. Cartography can supply node identity and map-knowledge freshness, but it cannot manufacture comparable environmental samples. Neither system owns ecological recovery because neither computes or persists the ecological dimensions GR-2 would need; wildlife ecology remains with its wildlife owner, managed soil remediation with `SoilReclamationProfileEngine`, and wild-location current condition with `LocationEvolutionSystem`.

### J.1 Source-specific boundaries for any future comparison

| Candidate comparison | Valid only if | Must refuse when |
|---|---|---|
| Two InSAR summaries for one sector | Both summaries preserve compatible geometry/measurement semantics, day ordering, and their own confidence and unit | Geometry differs, coherence is low, span is zero, summary is absent, or caller has only a map visit |
| Current contamination with InSAR deformation | UI presents two distinct dimensions side by side with separate source dates and labels | A combined score, common “recovery” arrow, causal statement, or inferred contamination delta is proposed |
| Cartography node and location condition | A canonical node/location mapping is proven by authored/source contract | IDs are heuristically transformed or a visited/surveyed state is used as measurement time |
| Wildlife observation and land condition | Each remains independently attributed, and no cross-domain conversion is performed | A sighting is treated as a contamination sample, or lower contamination is treated as proof of a species return |

### J.2 Numeric interpretation and reproducible review example

For review only, suppose the existing InSAR owner has two already-processed summaries for a sector: day 80 reports `-4.0 mm` relative displacement over its own repeat-pass window, and day 100 reports `-5.5 mm`. This may support a deformation comparison only after checking that both summaries use compatible geometry and the same baseline interpretation. It does not say contamination rose by 1.5, nor that a slope is unsafe without the existing travel-risk rule. The proposed UI should show the owner label, unit, observation day/window, confidence, and source route. Values in this example are **PROPOSED / VERIFY**, not catalog thresholds.

Now suppose `LocationEvolutionSystem` currently reports a contamination value on day 100 but no prior comparable sample. The correct GR-2 result remains “current value; trend unavailable,” even if the InSAR sector has a valid multi-pass history. Borrowing the deformation dates would produce a false land-condition series. The same rule applies if Cartography reports that the node was visited on day 80: a visit date supplies no measurement dimension or instrument provenance.

### J.3 Reviewer evidence tasks

Before choosing one of the original history options, record the exact DTO fields and save owner for any candidate source, trace all callers that create the row, establish whether restore preserves the original day/units/source, and compare the candidate's measured variable to the player wording. P0 should inspect the current SaveSectionRegistry entry and owner capture/restore methods for InSAR, then inspect the canonical map projector and its `MapDiscovery` inputs. If the UI wants both land status and deformation, keep them in independent rows. If design requires a single “ecological recovery” score, stop: no inspected owner provides the required cross-domain semantics.

## Appendix K — Observation admission gate

A future observation candidate should pass every gate below before it enters a trajectory calculation. A failed gate does not make the source useless; it means GR-2 cannot compare it as the same measurement without a source-owner contract.

| Gate | Required evidence | Pass example | Refusal example |
|---|---|---|---|
| Subject identity | Stable canonical site or sector ID and explicit mapping | Location record ID resolves through authored mapping | Display-name match or string-prefix conversion |
| Measured dimension | Named physical/domain value and unit | Owner labels a sample “contamination, unit X” | Generic `quality`, `survey`, or `condition` with no conversion contract |
| Measurement time | Source observation day with restore semantics | Sample retains source day after reload | UI-open day substituted for missing date |
| Measurement authority | Named owner and creator call path | Existing command writes a sample DTO | Panel creates one on selection or narrative flags imply one |
| Comparability | Same dimension, unit, method/baseline, and compatible quality | Two same-instrument samples under compatible settings | Cartography discovery compared with sensor output |
| Valid numeric domain | Owner range and malformed-value policy | Finite value inside catalog-defined bounds | NaN, infinity, or corrupt value treated as measured |
| Ordering | Deterministic order and duplicate policy | Stable sample IDs plus day and source key | Arrival-order iteration silently picks the last row |
| Persistence | Existing capture/restore or explicit stateless proof | Same rows survive round trip exactly once | Panel cache survives only until scene closes |
| Rollback | Shared transaction or defined replay rule | Failed campaign transaction removes the sample | Sample survives while its causal world action rolled back |
| Correction | Source can revise or invalidate a bad sample transparently | Superseded fact retains provenance | Old sample overwritten without a correction trace |

### K.1 Three-source admission example

Suppose a future build contains these rows for one selected wild location: a Cartography confirmation on day 12, a location contamination value on day 15, and an InSAR summary for the surrounding sector on day 16. Admission produces three independent facts, not a three-point series. Cartography has no contamination dimension. Location state has one current value but no historic sample. InSAR measures deformation. Eligible contamination sample count is therefore one—or zero if the location value cannot be attributed to a measurement action. The trajectory remains `unknown / insufficient evidence`.

If a later approved measurement action records contamination on day 24 using the same declared method and unit, only the two source-owned contamination readings may enter comparison. InSAR and Cartography may appear in neighboring context rows, but do not alter classifier inputs. This remains true when all three values happen to share a numeric range; accidental numeric compatibility is not semantic compatibility.

### K.2 Rejection and recovery behavior

| Detected condition | Projector behavior | Owner or UI recovery |
|---|---|---|
| One eligible sample only | Current value plus insufficient-history state | Explain the missing second comparable reading; do not fabricate a baseline |
| All samples have incompatible geometry/method | Incomparable state | Invite a compatible owner survey if one exists; preserve records |
| Duplicate source key | Apply source dedupe contract; otherwise reject ambiguity | Preserve canonical rows and surface a diagnostic |
| Old sample arrives after newer sample | Apply signed late-arrival rule or refuse trend | Keep corrected ordering visible; never use arrival order |
| Source ID no longer exists in current catalog | Preserve historic fact as unresolved | Do not reinterpret with a similar catalog row |
| Invalid row among otherwise valid samples | Exclude only under explicit rule | Never clamp invalid data into a plausible measurement |
| Restore repeats a stable sample ID | Treat as persistence defect | Fix owner round-trip before calling trend valid |

No recovery case authorizes GR-2 to mutate the location, source observation, or save. Evidence repair belongs to the owner that created the row.

## Appendix M — Irregular evidence windows and provisional classifier cases

The current location simulation can change contamination each day, while `LocationMutationRecord` retains only current state and `lastEvolutionDay`. A future measurement source may sample irregularly. The projection must define what it does with unequal intervals, weather gaps, events between measurements, corrections, and values close to source noise. This appendix sets review cases, not numeric production thresholds.

| Evidence pattern | Example | Provisional result | Required review decision |
|---|---|---|---|
| Two same-method readings, short interval | Day 8 `0.42`, day 9 `0.40` | Direction only if source resolution/tolerance supports a meaningful change | Minimum interval and noise model; numbers **PROPOSED / VERIFY** |
| Two same-method readings, long interval | Day 8 `0.42`, day 40 `0.35` | Direction over 32 days, with no claim about unobserved daily path | Maximum age/gap before stale label |
| Hazard/weather event between samples | Day 8 `0.42`, weather-driven world state changes, day 15 `0.49` sensor sample | Measured delta; cause unknown unless owner records causal link | Whether to show known intervening events as context, never automatic cause |
| World tick changes current field but no sample is made | Day 8 sample `0.42`; current field evolves on days 9–14 | Latest measured sample remains day 8; current simulated field may be a separate row | Whether UI can present current and measured dimensions without confusion |
| Same-day repeated sample | Two command attempts both claim day 15 | One or two measurements only according to owner command identity; not inferred from day alone | Stable source key and duplicate policy |
| Late imported sample | Day 20 report arrives after day 25 sample | Reorder by observed day if owner allows, or report incomplete | Correction/late-arrival semantics and visible update |
| Source corrected old value | Day 12 `S2` revised by source owner | Recompute from effective corrected lineage | How original/corrected provenance is displayed |
| Equal-value readings | Day 8 `0.42`, day 15 `0.42` | Stable only if exact/source tolerance semantics say so | Equality, tolerance, and precision rules |
| Outlier reading | Day 8 `0.42`, day 15 `0.91`, day 22 `0.43` | Do not discard by GR heuristics; follow measurement owner quality/review | Who owns outlier rejection and whether samples are validated |
| Different instrument profile | Profile A at day 8, profile B at day 15 | Incomparable unless owner declares a conversion/reference | Cross-instrument calibration authority |

### M.1 Proposed output vocabulary matrix

| Candidate output | Minimum evidence | Interpretation limit |
|---|---|---|
| `no_measurement` | No eligible source row | There is no measurement; current owner state may still exist separately |
| `single_reading` | One valid observation | Current/most recent sample only; direction unknown |
| `improving` | At least two comparable, ordered observations plus accepted negative/positive direction for the chosen dimension | The named measurement moved in the named direction; no broad habitat claim |
| `stable` | Comparable observations and owner/design-signed tolerance | Values fall within chosen tolerance; exact rule and units must be shown in docs |
| `worsening` | Comparable observations and dimension-specific orientation | Named measurement moved in adverse direction; do not infer human cause |
| `mixed` | Three or more valid points with reversals, or source-defined instability | Window summary differs by interval; no cherry-picking |
| `incomparable` | Multiple points exist but method, unit, geometry, or reference differs | Evidence exists but cannot support a delta |
| `stale` | Owner-approved age/gap policy marks otherwise comparable evidence old | Old sample remains historical, not erased |
| `corrected` | Source has correction/supersession lineage | Display effective result and correction provenance |
| `invalid` | Source/owner validation rejects value or identity | Do not show a plausible capped number |

The labels should not be encoded in a Green Return save field. They are derived from evidence and a versioned rule. If the rule changes, the projection can be recomputed without changing the observations. If a durable historical label is required for narrative continuity, the source/owner must state why a recalculated derived view is insufficient and own its migration.

### M.2 Source/day join acceptance

For each candidate sample pair, reviewer should answer these concrete questions before accepting a trend row:

1. Do both IDs resolve to the same canonical location, rather than two locations in one sector?
2. Are both values produced by the same measurement owner or a reviewed compatible-method contract?
3. Are both units identical after explicit conversion, with conversion version recorded?
4. Does each `observed_day` reflect measurement time rather than import, save, or UI-open time?
5. If a world event mutated contamination between the dates, is that event displayed as context without claiming causality?
6. If the second sample is a correction, does it replace the earlier effective sample or add another observation by the source’s rule?
7. Are the retention window and maximum evidence gap documented and tested?
8. Does capture/restore preserve IDs and order so that replay returns exactly the same result?
9. Does the projector avoid campaign RNG and owner mutation?
10. Can the player distinguish trend from present simulated state in plain language?

Any “unknown” answer blocks the affected comparison. It does not block a direct current-state map card if that card can be sourced independently.

## Appendix N — InSAR numerical contract and separation of classifications

For future reviewers who need to display the existing sector deformation source, its actual contract is more specific than a generic “survey quality” reading. `InSarDeformationEngine` defines `MinCompatiblePasses = 2`, `MinCoherence = 0.35`, `StableVelocityMmPerDay = 0.05`, `AbruptDisplacementMm = 25.0`, `AcceleratingRatio = 1.5`, and confidence bounds `[0.05, 0.97]`. Its summary stores coherence, relative LOS displacement in millimeters, velocity in millimeters/day, class, compatible pass count, last processed day, average weather quality, and a note. Its authoring description says this is abstract sensing, not a real radar engineering specification.

| InSAR input/result | Exact contract in current engine | GR-2 treatment |
|---|---|---|
| Pass quality | `QualityBp` is clamped to 0–100; despite the name, source comments describe observation quality on 0–100 | Preserve source unit/name and do not call it contamination basis points |
| Reference geometry | Engine selects the most recent pass’s geometry and filters to exact ordinal matches | Different reference geometry is a hard incompatibility, not evidence of change |
| Minimum pass count | At least two compatible observations | One pass returns a blocked insufficient-pass result |
| Time span | Latest compatible day minus earliest must be positive | Same-day/equal-day stack is insufficient regardless of count |
| Coherence | Starts at 0.95; subtracts time decay, weather decorrelation, and terrain-tag effects; clamps to 0–0.99 | Below 0.35 stores low-confidence summary and returns blocked low-coherence |
| Displacement | Quality is converted relative to 50 using sensor detectable-deformation scale | Millimeter displacement is geophysical, not concentration or recovery percentage |
| Classification | Abrupt if abs displacement ≥25 mm; with ≥3 passes accelerating subsidence if late negative rate exceeds early negative magnitude ×1.5; otherwise slow subsidence at velocity ≤−0.05 mm/day; else stable | Use only the named deformation class and its own qualifiers |
| Confidence | Computed from coherence, average quality, skill modifier; clamped; optional seeded jitter if RNG is bound | Confidence belongs to this process output; do not equate with wildlife observation confidence |
| Historical retention | Pass list bounded to 256, oldest pass removed first | A long-term trend beyond retained passes is unavailable unless source owner archives it |
| Processed status | Summary replacement is by sector; compatible passes marked processed | Summary is a derived current sector result, not a full preserved trajectory history |

The sector summary can itself be updated after additional processing; it does not expose a guaranteed immutable series of every historical summary. Therefore a GR trend should not calculate “InSAR is worsening over time” by reading only the latest current summary twice or combining values without retaining the prior summary’s valid provenance. If a consumer compares two summaries, it must verify both are separately preserved under the InSAR owner or supplied from an accepted archive. `GetSummary(sectorId)` returns the current summary for one sector, not a summary history API.

### N.1 Worked InSAR acceptance versus contamination readout

Suppose one sensor profile reports two compatible passes, quality 50 on day 10 and quality 40 on day 20, in the same geometry, with adequate coherence. The exact displacement depends on the profile’s `minimum_detectable_deformation_mm` and the quality-to-displacement conversion. The owner may produce a deformation class and LOS displacement. The map may report “sector deformation: [class], [mm], confidence [x]” if the existing route can access it. It may not translate the 10-point quality change to a 10% contamination change. The sensor profile’s minimum detectable value is not a unit conversion into contamination.

If `ProcessSector` runs again with three passes and an optional bound `ISeededRng`, confidence can include a deterministic jitter draw. A Green Return map query must not call `ProcessSector` to manufacture current data: doing so mutates summaries, increments totals, marks passes processed, logs, and may consume owner RNG. It may only read the already-owned summary through an approved bridge. This is a concrete purity test: snapshot InSAR state and its relevant RNG stream before/after viewing the map; both must remain unchanged.

### N.2 Distinguish four superficially similar “quality” fields

| Field | Owner and scale | Correct user question | Invalid equivalence |
|---|---|---|---|
| Cartography `SurveyQuality` | Map knowledge projection, 0–100-style presentation tiers | How detailed/confirmed is the map node? | Physical site measurement |
| Wildlife `confidence` | Observation confidence, clamped 0–1 | How confident is this species/sector/day report? | Contamination sensor accuracy or land quality |
| InSAR `QualityBp` | Per-pass phase/backscatter quality clamped to 0–100 | What sensor input quality fed deformation processing? | Contamination basis points or a recovery score |
| Location `contaminationLevel` | Current location owner scalar, range/unit requires current contract audit | What current contamination value does the location owner store? | Map survey quality, species confidence, or displacement |

No common normalization operation is present in the inspected sources. A combined “confidence in recovery” would therefore be fabricated unless a future domain owner defines the target, sample lineage, calibration, and acceptance threshold.

## Appendix L — Worked regional evidence ledger across a save boundary

This scenario demonstrates how to reason about evidence without claiming the current game already has land samples. The location is the authored `loc_grange_hall` seed in `sector_4_hinterlands`. Its initial contamination value is `0.1`; the exact sample IDs, instrument, unit, and values below are **PROPOSED / VERIFY** examples for a future owner-approved field measurement contract.

| Campaign moment | Existing world fact | Candidate measurement fact | Trajectory eligibility |
|---|---|---|---|
| Day 0 new campaign | Seeder creates initial location record only if absent | None | No sample; no trend |
| Day 4 player opens map | Current location value reflects day ticks | None; UI opening is not a survey | No new sample; projection is side-effect free |
| Day 6 expedition passes nearby and calls `MarkVisited` | `lastVisitedDay = 6` | None; method records passage, not measurement | Still no sample; visit cannot be promoted |
| Day 8 hypothetical approved sensor command at site | Existing current record remains owned by location system | Candidate `S1`: site, dimension, unit, day 8, source device/method, quality, stable source key | One sample; current value plus “trend unavailable” |
| Save at end of day 8 | Current owner save and measurement owner save capture | `S1` must restore exactly once with original source day | Still one eligible sample after restore |
| Day 12 hazard weather tick | Existing owner changes contamination using `LocationEvolutionInputs`; no sample appended by tick | None | `S1` remains one sample; current simulated value may differ from `S1` |
| Day 15 hypothetical repeat sensor command | Current record reflects evolution/event history | Candidate `S2`, same dimension/unit/method and compatible calibration | Two samples can be compared if source says method is compatible |
| Save/reload before map reopening | Both owners restore from canonical save route | IDs and times unchanged; no event re-fired by projection | Same direction and source pair after reload |

The distinction between current simulation and a direct sample is deliberate. If a sample reads `0.12` on day 8 while current state drifts to `0.16` by day 12, the system cannot claim a day-12 measurement unless a source recorded one. It may show current simulated owner state separately from the most recent measured sample, with dates on both. A one-day dynamic value is not an observation merely because the underlying field changes daily.

### L.1 Candidate record contract, field by field

If P0 finds no existing suitable observation owner and a separate approved decision authorizes one, the source owner—not this read-model plan—must define a schema at least as explicit as the following candidate. Field names and retention are illustrative only.

| Candidate field | Meaning | Required invariant | Migration/corruption behavior |
|---|---|---|---|
| `sample_id` | Stable identity of one accepted sample | Unique and deterministic under source contract; retries do not create second row | Duplicate ID is an owner validation error or idempotent replay of same row |
| `location_id` | Canonical site measured | Resolves through location catalog/map contract | Unknown ID preserved as unresolved history or rejected at write |
| `dimension_id` | What was measured | Stable authored ID; distinct from display text | Unknown dimension is not remapped by name |
| `value` | Numeric measurement | Finite, within owner-defined bounds | Invalid value is rejected/quarantined by owner; UI never clamps |
| `unit_id` | Value unit | Required and compatible with dimension contract | Missing/changed unit makes samples incomparable |
| `observed_day` | Campaign day measurement occurred | Nonnegative and not replaced by arrival day | Future day/corrupt order follows explicit owner repair policy |
| `source_id` | Instrument or observation method | Resolves to versioned method/provenance | Missing source prevents strong trend label |
| `method_version` | Measurement interpretation version | Stable rules for calibration/comparability | Unknown version retained but marked unresolved |
| `quality` | Source-provided quality/confidence | Defined by the measurement owner; bounded | No GR-2 generated confidence score |
| `reference_id` | Calibration/baseline identity when relevant | Required for methods that need a baseline | Incompatible reference produces no comparison |
| `action_key` | Idempotency/transaction reference, if source needs it | Same command retry resolves to one sample | Missing key handled per command owner contract |
| `supersedes_sample_id` | Correction lineage when sample revised | Points to earlier sample, never silently overwrites it | Broken link means correction unavailable, not new truth |

The Green Return projection should consume an immutable owner read DTO with these meanings, not gain direct mutation or persistence methods. Do not add fields to this plan's UI model and then let the model become the save owner by convenience.

### L.2 Worked numeric comparison, explicitly provisional

For arithmetic review only, suppose `S1` on day 8 is `0.42` and `S2` on day 15 is `0.37`, both from the same hypothetical source, unit, method version, and compatible reference. The raw delta is `-0.05` across seven days. A proposed output could be “the measured contamination reading decreased by 0.05 over seven days.” It could not be “land health improved by 12%,” because division by a baseline would invent a percent scale; nor “stewardship reduced contamination,” because neither sample encodes an intervention causal link. Values, normalized scale, units, and exact copy are **PROPOSED / VERIFY**.

Now let `S3` on day 16 read `0.38`. A two-point difference says decrease, while the latest adjacent difference says increase. Without a signed trend window rule, GR-2 must not cherry-pick the most favorable pair. Potential source-owned choices include comparing first/latest in a fixed retention window, requiring a monotonic run, or showing “mixed readings.” Each has a distinct interpretation and save requirement. It is unacceptable to pick whichever pair produces a recovery badge.

| Available ordered samples | Candidate truthful state | Why |
|---|---|---|
| none | No measurements | No source record exists |
| `S1` only | One reading; trend unavailable | No comparison partner |
| `S1,S2` compatible | Direction over named interval, subject to approved tolerance | Exactly one pair supports the delta |
| `S1,S2,S3` with reversal | Mixed/variable or rule-defined interval summary | Direction depends on chosen window |
| `S1` and `S2` differ in unit | Incomparable | Values cannot be subtracted across units |
| `S1` reference differs from `S2` | Incomparable | Calibration basis changed |
| `S2` corrected by source | Recompute from corrected lineage | Original must not remain double-counted |

### L.3 Old-save rollout acceptance

| Save cohort | Required behavior | Evidence to collect |
|---|---|---|
| Save predates any sample schema | Load existing location state unchanged; show trend unavailable | Old fixture remains byte/semantic compatible under owning migration |
| Save has location state but no source history | Do not backfill `lastEvolutionDay` as sample day | UI explicitly says no prior comparable sample |
| Save has one sample | Keep exact provenance; one-reading status remains | Capture/restore equality for ID, unit, day, source, quality |
| Save has two valid samples | Stable ordering and same projection after restore | Paired same-state projection result |
| Save has duplicate/corrupt sample IDs | Owner migration or refusal policy resolves; GR projector does not guess | Diagnostic identifies source owner and affected records |
| Save contains sample from removed method catalog entry | Retain or quarantine according to owner version policy | No silent remap to new method |
| Day transaction rolled back after proposed sample write | Sample and related owner state roll back together | Retry produces one accepted sample, no ghost row |

Only the owner that persists samples can pass the last cases. If no current owner can do so, retain the map-only capability and stop the trajectory feature at the missing-history decision.

## Appendix O — Owner-to-reader contract and verification handoff

This mapping makes the transition from source observation to user wording explicit. It is not implementation authorization; all future fields and thresholds remain subject to owner review.

| Layer | Current fact | Required future contract | Acceptance gate |
|---|---|---|---|
| Measurement producer | No general wild-land sample API was verified in `LocationEvolutionSystem` | Record dimension, unit, canonical site, observed day, method/source, quality, correction identity | Trace a real gameplay command to the owner write; visit or narrative text is insufficient |
| Current location state | `LocationMutationRecord` retains current contamination and `lastVisitedDay` | Separate simulated current value from a measured sample | Daily tick changes current state without appending a sample |
| Wildlife observations | Ecology stores species/sector/day/confidence, bounded to 200 rows | Keep as wildlife-only evidence | No contamination comparison or generic conversion |
| InSAR passes | Sector/reference/day/quality/weather; compatible-pass processing | Preserve deformation as distinct dimension | Geometry/pass gate and units survive host bridge |
| Host/event bridge | `InSarMappingHostSession` writes/processes through InSAR; campaign owner orders location/wildlife ticks | Any land observation is emitted by its canonical owner after commit | Rollback and retry yield one persisted source row |
| Projection | No existing Green Return trend projector verified | Pure deterministic function over immutable validated observations | Same input returns same result without mutation or RNG |
| Save/load | InSAR and wildlife have separate owner state; location retains current value only | Every sample persists through its source owner | Old save does not backfill; current and historical facts remain distinct |
| Map/UI | Cartography projects knowledge; map has location danger/detail | Show variable, interval, dates, and owner-qualified uncertainty | Unknown/incomparable/mixed outcomes are visible |
| Narrative | `quest_the_irradiated_soil` is a remediation story | Narrative may contextualize but cannot create measurement or causal fact | Quest completion alone does not change a trajectory |

### O.1 Minimal pure-projector contract

Candidate input is one canonical subject, one measured dimension and unit, an immutable validated observation set, and a rule version. Conceptually:

```text
ProjectTrend(subjectId, dimensionId, unitId, observations, ruleVersion) -> TrendReport
```

Candidate result fields are `subject_id`, `dimension_id`, `unit_id`, `direction`, `first_observed_day`, `last_observed_day`, `eligible_count`, `excluded_count_by_reason`, `source_refs`, source-defined quality summary if any, and `rule_version`. Exact C# type shape requires owner review. If a field cannot be sourced or deterministically derived, omit it.

The projector must not accept a mutable world object or read `LocationEvolutionSystem.State` directly. Mixed dimensions are rejected before numeric math. If same-day samples are allowed, ordering uses observed day plus stable sample ID, not arrival order. The calculation uses a signed, documented tolerance and window rule. It must avoid hash order, local time, culture-sensitive parsing, hidden float rounding, and fresh RNG. The representation (fixed point, decimal, or double) depends on source precision and existing serializer contract and remains a P0 decision.

### O.2 Observable result matrix

| Projection | UI must state | Focused assertion |
|---|---|---|
| Improving/decreasing | Named dimension decreased between named days; current value can differ | Correct dimension-specific sign and tolerance |
| Stable | Values fall inside accepted source tolerance/window | Exact inclusive/exclusive boundary and precision |
| Worsening/increasing | Named dimension moved in the adverse direction | Direction is correct for that dimension |
| Mixed | Values reverse under the selected multi-point rule | Deterministic all-point result; no favorable pair cherry-pick |
| Insufficient | No eligible pair | Empty and single-reading cases differ |
| Incomparable | Unit, method, geometry, or reference mismatch | No delta is produced |
| Stale | Evidence exceeds source-approved age/gap | Boundary age behavior is deterministic |
| Corrected | Owner exposes supersession/correction lineage | Effective reading recalculates without double count |
| Invalid | Schema/owner rejects value or identity | Invalid data never becomes a plausible capped number |

Do not persist these derived labels in Green Return state. If a narrative requires a historically frozen conclusion, the source owner must explain why recomputation is insufficient and own the migration.

### O.3 Bounded handoff checklist

A future implementer records exact writer path/caller, source schema and catalog, save owner, sample retention/correction rule, measurement unit and calibration, pure projector API/rule version, exact path claim, focused targets via `bin/run-scoped-tests`, old-save fixture if schema changes, replay evidence if state or RNG changes, route evidence if UI changes, rollback path, and remaining label/threshold decisions. A rejected or absent owner seam is handed back as a blocker; it is not replaced by a local sample cache. Full test-suite execution is not implied.
