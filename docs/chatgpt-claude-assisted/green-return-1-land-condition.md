# GR-1 — Land Condition and Recovery Baseline

STATUS: DRAFT — proposal for review only; no approval, claim, or implementation is implied.

## 1. Objective

Define a truthful land-condition read model over facts already owned by `LocationEvolutionSystem`. The player should be able to distinguish observed contamination, site disturbance, and uncertainty without receiving a new mutable ecology simulation. The plan's principal product is a stable, evidence-backed classification contract that later Green Return plans may consume.

**Completion means:** the P0 audit has measured the existing fields and all readers; the team has either accepted a pure projection with an explicit threshold source or recorded that current evidence cannot support classification; and each accepted output can be traced to canonical saved facts. It does not mean the land has acquired new gameplay effects.

## 2. Current Reality

`LocationMutationRecord` is already the persistent per-location mutation fact in `Assets/Ashfall.Core/LocationEvolutionSystem.cs`. It stores `contaminationLevel`, `lootDepletionFactor`, `isCleared`, `isRuined`, `lastVisitedDay`, `activeThreats`, `discoveredCaches`, and `currentOwner`. `LocationEvolutionSystem` provides `TryGetRecord` for non-creating reads, `GetOrCreateRecord` for deliberate mutation, `MarkVisited`, `MarkCleared`, `MarkDepleted`, `AddThreat`, `RemoveThreat`, `SetLocationOwner`, `TickDay`, `CaptureState`, and `RestoreState`.

The live overload `TickDay(int, in LocationEvolutionInputs, ISeededRng?)` updates contamination from the weather inputs, handles a sticky ruined flag, recovers loot depletion for cleared sites after a quiet period, and applies seeded threat sprout/decay. Its constants currently include `ContaminationHazardGain`, `ContaminationClearDecay`, `RuinedContaminationThreshold`, `ThreatSproutChance`, and `ThreatDecayChance`. These are actual rules, not evidence that a broader soil or ecosystem model exists.

`EvolvingWorldDayOwner` in `src/Main.CampaignOwners.cs` owns phase-4 orchestration. It snapshots location, wildlife, and landmark state for day rollback; calls location evolution with current outdoor radiation and hazard-weather facts; and forks the campaign's `WorldEvolution` random stream. It then ticks migration/ecosystem owners and later applies expedition and faction consequences. This plan does not add a second clock or reorder that owner.

Known consumers include `Main.EvolvingWorld.cs`, `Main.WorldPlaytest.cs`, `WorldHostSession` / `WorldSaveStore`, `WastelandMapView`, `ExpeditionPanel`, ecological infestation targeting, and the host evolving-world probes. `WastelandMapView.WorldEscalatedDanger` currently raises authored danger for active threats or `isRuined`; the expedition danger composer reads contamination above its existing threshold. Neither surface currently claims a complete land-quality classification.

## 3. Required Delta

The requested direction needs a compact answer to “what do we know about this site's land condition?” Existing data has several useful facts but mixes contamination, disturbance, and site encounter state. The minimum delta is therefore a **projection contract**, not a new field collection or simulation.

Candidate output dimensions for audit and review:

- contamination band derived from `contaminationLevel`;
- disturbance band derived from `lootDepletionFactor`, `isCleared`, and `isRuined`;
- evidence freshness derived from `lastVisitedDay` and the canonical evolution day;
- an explicit `unknown` result when there is no record or no sufficiently current observation.

These dimensions should remain separate. Collapsing them into one scalar would imply, for example, that a cleared site is chemically safe or that a low-loot site is ecologically barren. No such implication is supported by the current model.

## 4. Evidence

Evidence snapshot checked 2026-09-29; implementation must repeat the premise audit and capture exact path/line references before a package is claimed.

| Fact | Current evidence | Planning consequence |
|---|---|---|
| Location state is persisted and deep-cloned | `LocationEvolutionSaveState`, `LocationMutationRecord`, `CaptureState` / `RestoreState` in `LocationEvolutionSystem.cs` | Reuse its owner; do not add a parallel location record or save section. |
| Live contamination depends on weather | `LocationEvolutionInputs` and live `TickDay` in `LocationEvolutionSystem.Live.cs` | Report the resulting saved value; do not reconstruct or re-simulate weather in the read model. |
| Day ordering and RNG are canonical | `EvolvingWorldDayOwner` in `src/Main.CampaignOwners.cs` | A projection should be pure and should not register a day owner. |
| Map already exposes danger escalation from threat/ruin | `WorldEscalatedDanger` in `src/World/WastelandMapView.cs` | Add a distinct condition detail only if an existing map presenter has a suitable extension point. Do not redefine danger semantics. |
| Expedition danger consumes contamination | `ComposeExpeditionDangerMultiplier` in `src/Main.EvolvingWorld.cs` | A condition label must not feed this or any other gameplay calculation without a separate signed plan. |
| World-event catalog can mutate location records | `WorldEvolutionEngine.ApplyEvent` in `Assets/Ashfall.Core/World/WorldEvolutionEngine.cs`; `world_evolution_events.json` | Audit direct writes as well as method calls; catalog events are an existing source of contamination/depletion changes. |
| Wildlife and agricultural owners exist | `WildlifeMigrationSystem`, `WildlifeEcosystemSystem`, `AgricultureSystem` | Their values are outside this read model unless a reviewed provider seam is later approved. |

The evidence does **not** establish a canonical soil condition, nutrient state, site-specific cleanup history, climate series, or comprehensive land survey. Do not write plan language as though those fields already exist.

## 5. Existing Extension Seams

The read-only seam is `TryGetRecord(locationId)`, which returns null instead of creating an empty record. The owning record and `CaptureState` are the save seam. The existing map and expedition presenters are possible display seams but must be inspected in P0 for route ownership, refresh lifecycle, and accessibility before any UI claim. `LocationEvolutionSystem.OnLocationMutated` can notify existing host consumers, but it is not a historical observation log and must not be treated as one.

If P0 finds that readers need one consistent set of bands, the safest first contract is a pure Core projector taking an immutable copy of the already-available facts plus authored thresholds only if a current catalog validator and consumer can be demonstrated. Avoid an interface solely to serve one call site. A one-off projection method on the existing owner may be smaller, subject to Core-owner review.

## 6. Proposed Architecture

### 6.1 Projection only

The projector receives an existing location record or an immutable value snapshot. It returns independent fields for contamination, disturbance, ruin, evidence freshness, and confidence/unknown state. It does not mutate the input, create a location record, advance time, generate events, consume RNG, or call other owners.

### 6.2 Classification is a report, not an effect

Classification must not modify map danger, route passability, migration, wildlife density, food production, trade value, settlement condition, radiation, or hazard resolution. A separate integration proposal is required for any mechanical consumer. The four-plan family reserves Settlement / price pressure for Living Region, wildlife and food-web state for Second Nature, and knowledge progression for Reconstruction Tree.

### 6.3 Missing and contradictory facts

No `LocationMutationRecord` means `unknown`; it does not mean pristine. Non-finite numbers, out-of-range contamination/depletion, invalid day values, or malformed IDs should be surfaced to validation or treated as unknown for display. The projector must not silently clamp saved authority and then present the result as verified. Whether loader validation already rejects those cases is a P0 question.

### 6.4 Open design decision

No final labels or thresholds are approved here. P0 should compare three options: keep raw values with plain-language evidence; add authored bands only if schema/validator/consumer are real; or defer classification and document why. If the first option is adequate, do not create a JSON catalog merely to make the plan appear data-driven.

## 7. Ownership Matrix

| Concern | Sole owner | GR-1 role |
|---|---|---|
| Per-location contamination, depletion, cleared/ruined, visit day | `LocationEvolutionSystem` | Read only |
| Weather and outdoor radiation | Existing weather owner | Consume only the value already incorporated into location state; no recomputation |
| Wildlife counts, population, movement, ecosystem pressure | `WildlifeMigrationSystem` / `WildlifeEcosystemSystem` | Excluded |
| Crop condition and production | `AgricultureSystem` and its current host | Excluded |
| Settlement population, health, price pressure | Living Region's existing owners as verified there | Excluded |
| Research / restored knowledge | Reconstruction Tree owners | Excluded |
| Map navigation and marker lifecycle | Existing Godot map owner | Optional read-only rendering only |
| New persistent condition facts | None in this plan | Do not add |

## 8. Data Flow

`LocationEvolutionSystem.State` → `TryGetRecord(locationId)` → validated immutable input → pure classification → existing location detail/map presenter → visible label plus source facts and freshness.

The map selection path must never write to `LocationEvolutionSystem`. A player action is not in this plan. Any action that changes land must enter through its actual owning command and produce a canonical mutation before the displayed condition changes. Presentation refresh should observe owner events or the existing panel refresh seam; it should not poll every frame or keep its own evolving cache.

## 9. State Model

No new state is proposed. Inputs are the existing record facts and, if available without a new authority, the owning world's `lastEvolutionDay`. Output is transient and reproducible. Proposed invariants:

1. Projection does not alter `CaptureState()` output.
2. Null record produces unknown, not a constructed default record.
3. Condition dimensions cannot infer one another: clearing does not reset contamination, and low depletion does not prove recovery of soil or species.
4. `isRuined` remains the existing sticky ruin fact; GR-1 does not clear or reinterpret it.
5. Day freshness cannot be negative or greater than the current campaign day without an explicit “invalid/stale data” result.
6. Outputs are stable under input collection order and culture settings.

If P0 finds a necessary irreducible observation fact that is not held by the location owner, stop and seek an ownership decision; do not propose a local cache in a presenter.

## 10. API / Contracts

No concrete public API is approved. P0 should inspect existing Core naming conventions and consumers before proposing any additive contract. A minimal candidate shape, only if repeated consumers justify it, is a value result such as `LocationConditionProjection` with explicit `Known`, contamination band, disturbance band, ruin status, source day, and `UnknownReason`. Prefer enums with stable serialized names only if serialization is actually needed; transient projection enums need not become save schema.

The contract must specify that its methods are pure, invariant under repeated calls, free of I/O and RNG, and do not create records. It must not expose mutable `LocationMutationRecord` references to UI. No event is needed if the display can use the current host's established state notification.

## 11. Data Changes

No JSON modification is initially proposed. P0 should inspect `locations.json`, `world_evolution_seeds.json`, and `world_evolution_events.json` to establish canonical IDs and any authored environmental descriptors. Existing authored terrain or biome labels may be displayed as authored descriptors but cannot be reinterpreted as dynamic condition without evidence.

Only add a catalog if all of the following are true: more than one owner needs the same thresholds; current `CatalogIntegrityValidator` can validate IDs/ranges/references; the runtime has a real loader and consuming API; missing data has a deliberate neutral behavior; and duplicate thresholds are not already owned by another data family. No proposal should modify generated catalogs or add decorative thresholds unused by gameplay.

## 12. Save / Load

No new save state and no section registration are expected. Existing location mutation facts already participate through `WorldSaveStore` / `WorldHostSession`. The read model must use the post-restore owner record. It must not serialize labels or calculated bands; they are recomputed from restored facts and current authored thresholds.

P0 must verify old saves where `mutations` is empty, records contain null collections, `schema_version` is older, or the new catalog is missing. Existing clone code normalizes null `activeThreats` and `discoveredCaches`, but any new code must not assume input arrays are always non-null. If a new persistent fact becomes necessary, this plan's no-new-state premise fails and the work stops for a revised save-ownership proposal, including schema migration, checksum, restore order, and deep-copy tests.

## 13. Determinism

The projector uses no randomness. For identical facts and threshold data, it returns identical output across processes, locales, and collection order. Numeric classification must use explicit finite-value handling and threshold comparisons; formatting for UI uses invariant numeric formatting only where a numeric value is shown. A label-only UI must not serialize locale-specific output.

Do not change `LocationEvolutionSystem.TickDay`, its `ISeededRng` input, `CampaignStreamIds.WorldEvolution`, or world-owner ordering in this plan. Existing save snapshots are the facts from one completed tick; the report must not replay a tick to calculate a result.

## 14. System / Event Wiring

No day owner, event bridge, or new event vocabulary is proposed. `OnLocationMutated` currently fires on selected mutations, not every field write: P0 must audit direct catalog writes and any non-event mutations before relying on it for UI refresh. If event completeness is lacking, use the existing host refresh lifecycle for a read; do not broaden event semantics under this draft without a separately scoped review.

Report ordering follows completed Core mutation, then day owner completion, then presentation refresh. Reading mid-tick must not expose a partially computed compound classification. If current lifecycle offers no safe snapshot boundary, defer display work rather than adding a second scheduler.

## 15. Godot Integration

Potential surface: existing selected-location detail or map marker tooltip. P0 must verify exact node, route, navigation, ownership, and current refresh methods. Prefer a secondary detail row with plain-language value and “as of day N / not surveyed” context; do not overload `MapNodeDanger`, which already has authored and live danger semantics. Do not add a new map layer, panel, data cache, or modal solely for GR-1.

If visible, the result must fit the fixed 1920×1080 UI, remain readable at established font sizes, expose focusable controls only when controls exist, preserve keyboard/controller back behavior, and clear subscriptions on disposal. Unknown is a real visible state. Color cannot be the only channel. UI code binds to a provider and contains no thresholds or domain math.

## 16. Narrative / Content Integration

No new narrative text is needed to establish the condition contract. If a data-backed label is later added, it must describe observations (“recorded contamination remains high”) rather than unsupported causes (“the ground is poisoned by the factory”). Journal, radio, fauna bestiary, or research lines must use current owner facts and their existing persistence; GR-1 does not enqueue an event or grant knowledge.

## 17. Failure Modes

| Case | Required behavior |
|---|---|
| Location ID absent or unknown | Return unknown; do not call `GetOrCreateRecord`. |
| No location record yet | Show “no recorded condition” / neutral unknown; never infer healthy. |
| Missing data catalog | Keep existing behavior; no invented fallback bands. |
| Duplicate or unresolved authored location ID | Fail catalog validation or suppress classification; do not merge records. |
| NaN/infinite/out-of-range saved value | Do not crash or report precision; mark invalid/unknown and surface validation evidence. |
| Stale observation day | Keep the fact but mark it stale; do not imply that today's weather was surveyed. |
| Record changes during panel refresh | Build from a coherent snapshot or refresh again after owner completion; never mutate. |
| Corrupt location save | Follow current save recovery policy; do not silently create a competing blank ledger. |
| Ruined site later has lower contamination | Show ruin and current contamination as distinct facts; do not clear sticky ruin. |
| Site was cleared but remains contaminated | Preserve both statements; cleanup completion is not chemical recovery. |

## 18. Test Strategy

This is a draft and authorizes no test run. A future approved implementation should use `bin/run-scoped-tests` and only changed targets. Define tests before coding:

- table-driven boundary tests for each proposed band after thresholds are signed;
- null/missing record returns unknown and leaves captured owner state byte-equivalent;
- repeated projection and permuted source list produce identical results;
- invariant-culture versus alternate-culture runs produce identical enum/value outputs;
- stale/future day and NaN/range cases follow the failure table;
- presenter/provider test proves labels are projections and map danger value remains unchanged;
- existing save round-trip test remains the authority for mutation persistence; add one targeted integration assertion only if no equivalent exists.

No full suite is implied. No speculative duplicate tests. A Godot headless check is relevant only if the selected map/detail route is changed.

## 19. Dependency-Ordered Phases

### Phase 0 — premise and duplicate audit (read only)

Re-read `LocationEvolutionSystem`, its live partial, seeder, world-event mutations, every source/data consumer, save registration, and map detail route. Search for other land condition / contamination projections and check the Green Return, Living Region, Second Nature, Reconstruction Tree, and existing expansion-plan corpora. Record path/line evidence and all dirty/claimed paths.

**Gate:** confirm one owner, no duplicate projection, a real display seam, and whether existing facts support useful labels. Stop if the premise differs.

### Phase 1 — contract review

Agree on separate dimensions, unknown semantics, freshness, and whether labels are needed. If a pure projector is justified, define its input/output and tests without assigning gameplay effects.

**Gate:** named reviewer accepts a written contract; thresholds and any authored fields have an identified validator and consumer.

### Phase 2 — Core projection, if warranted

Implement the smallest engine-free pure value projection or owner read method. Do not edit the day owner, save DTO, wildlife/economy owners, or map danger algorithm.

**Gate:** Core-focused tests prove purity, edge behavior, and output stability.

### Phase 3 — authored threshold data, conditional

Only if Phase 1 selected a data-driven rule, extend an existing validated catalog or add the smallest necessary catalog and loader. Do not author data before there is a real consumer.

**Gate:** integrity validation accepts every ID/range and missing data retains current behavior.

### Phase 4 — read-only host binding

Bind a provider to the existing location detail or marker tooltip after route/lifecycle audit. No new panel or save owner.

**Gate:** focused provider/UI check confirms unknown state, accessibility, refresh/disposal, and no owner mutation.

### Phase 5 — narrow integration verification and handoff

Run changed-file scoped targets; document exact commands/results, baseline failures, limitation, and untouched shared paths. A gameplay effect requires a separate approved plan.

**Gate:** no added authority; scoped verification passes; plan remains draft until a user/foreman explicitly authorizes implementation.

## 20. File Impact Map

Proposed paths are contingent and **not claims**. Exact ownership must be recorded before implementation.

| File / area | Action | Reason | Risk |
|---|---|---|---|
| `Assets/Ashfall.Core/LocationEvolutionSystem.cs` | READ ONLY; possibly MODIFY additively | Existing state and non-creating read owner | High if DTO/semantics change |
| `Assets/Ashfall.Core/LocationEvolutionSystem.Live.cs` | READ ONLY | Existing daily contamination/depletion behavior | High; avoid day-rule changes |
| `Assets/Ashfall.Core/World/WorldEvolutionEngine.cs` | READ ONLY | Existing event-driven location changes | Medium; catalog is another mutation source |
| `src/Main.CampaignOwners.cs` | READ ONLY | Phase order, save rollback, deterministic RNG | High shared integration seam |
| `src/World/WastelandMapView.cs` | READ ONLY; conditional MODIFY | Candidate read-only label binding | Medium; fixed UI and danger semantics |
| `src/UI/ExpeditionPanel.cs` | READ ONLY | Existing location detail consumer | Medium; route and refresh must be verified |
| `Assets/StreamingAssets/Data/locations.json`, `world_evolution_seeds.json`, `world_evolution_events.json` | READ ONLY; conditional MODIFY | Canonical IDs and existing authored environmental changes | Medium; integrity and reference risk |
| Existing Core test project | READ ONLY; conditional CREATE one focused test file | Add only tests not already covered | Low/medium; avoid duplicates |
| Save registry / day event vocabulary | NO CHANGE | No new owner, section, or event required | Any change would indicate scope expansion |

## 21. Risks

- **False certainty:** current fields capture only selected pressures; labels can make a partial record look like a complete survey. Mitigation: dimensioned output, visible unknown/freshness, explicit scope text.
- **Semantic collision:** `isRuined` and danger are already consumed as hazards. Mitigation: never replace or demote authored danger; condition is a separate detail.
- **Hidden mutation sources:** world evolution events may directly update the record. Mitigation: include direct writes and event catalog in P0.
- **Stale read:** player sees a last-known fact after the world has advanced. Mitigation: report source day and freshness rather than recomputing.
- **Catalog debt:** adding a threshold catalog without a consumer creates inert data. Mitigation: schema and consumer must be accepted together.
- **Plan overlap:** migration or wildlife effects could duplicate Second Nature; regional conditions could duplicate Living Region. Mitigation: respect the boundary table and stop on cross-owner requirements.

## 22. Out of Scope

No new soil, water table, nutrient, seed bank, toxin source, cleanup progress, weather memory, wildlife carrying capacity, crop yield, land ownership, settlement condition, price, map navigation, world clock, exploration ledger, or research tree. No changes to contamination decay/gain, ruin stickiness, expedition danger, route hazard, mutation IDs, wildlife spawning, or Unity-era assets. No balancing pass and no narrative explanation of causality.

## 23. Rollback Strategy

The desired first implementation has no persistent changes, so rollback is deletion/revert of the isolated projector/provider and removal of the additive readout. Keep it behind the existing host binding or a neutral provider default if a toggle already exists; do not add a new global feature-flag system.

If authored thresholds are required, missing/invalid catalog must fall back to current raw-state presentation or “unknown” while preserving gameplay. Revert the catalog and loader together. Do not rewrite saved location records. Any implementation that needs save migration or changes a gameplay consumer is beyond GR-1 and must return for a revised plan before that work begins.

## 24. Definition of Done

- P0 has current path/line evidence and a duplicate/collision result.
- One owner remains responsible for all persistent location facts.
- Read output is explicit about unknown, stale, contamination, disturbance, and ruin semantics.
- Projection is pure, deterministic, and does not create records or affect gameplay.
- Any data is schema-validated and actually consumed; otherwise no catalog is added.
- Any touched surface preserves route, refresh, focus, contrast, and disposal behavior.
- Only scoped tests are run through `bin/run-scoped-tests`; exact results are reported.
- No unrelated path, save owner, event vocabulary, day owner, or companion plan is changed.

## 25. Implementation Handoff

### MUST PRESERVE

`LocationEvolutionSystem` as the sole location mutation owner; its live tick and RNG stream; the phase-4 day order; existing map danger and expedition hazard semantics; unknown for absent records.

### MUST ADD

Only an evidence-backed, pure read projection if the existing fields support a useful and honest report; a focused test for uncovered contract behavior; optionally one read-only map/location detail binding after P0.

### MUST NOT DO

Create a land ledger, day owner, save section, survey cache, ecology simulator, settlement condition, wildlife population, farming mechanic, cleanup command, gameplay modifier, research node, UI-owned threshold, or unsupported causal prose. Do not edit the Living Region, Second Nature, Reconstruction Tree, or shared integration seams without claims and approval.

### VERIFY WITH

The exact targets selected under current `TEST_POLICY.md` using `bin/run-scoped-tests`; include a Godot headless check only if a runtime route or scene changes. Do not run the full suite unless the user types exactly `RUN FULL TESTS`.

### FIRST SAFE IMPLEMENTATION STEP

Read-only P0 census of all writes and reads of `LocationMutationRecord` plus map/detail lifecycle, then submit the evidence and candidate contract for review before editing.

## Appendix A — Field-to-claim matrix

This matrix is a review aid for preventing a displayed claim from outrunning its data. “Supported” here means the source has a fact with the right semantics, not that a new user-facing label has already been approved.

| Proposed statement | Existing fact that could support it | Current gap | Safe wording boundary |
|---|---|---|---|
| “Contamination recorded at this site is elevated” | `contaminationLevel` on the canonical location record | No confirmed measurement instrument, unit or observation provenance in this DTO | State that the system's location record holds a contamination level; do not call it a recent field measurement unless a source confirms that. |
| “Contamination is lower than the previous report” | Requires two dated, comparable land samples | Location save retains the latest value, not a sample series | GR-1 cannot make this comparison; defer to GR-2 and its source audit. |
| “The site has been cleared” | `isCleared`, written by `MarkCleared` after a completed expedition outcome | “Cleared” refers to encounter/site sweep semantics, not remediation | Preserve the existing term; do not shorten it to “land restored.” |
| “Loot is depleted” | `lootDepletionFactor` and depletion methods/events | Does not measure biomass, soil fertility or ecosystem pressure | Use “salvage depletion” or an equivalent accurate domain label. |
| “The site is ruined” | Sticky `isRuined` set by contamination threshold in the live tick | Ruin's exact player-facing semantics and consumers need current audit | Report the saved ruin flag distinctly from current contamination. |
| “Threats are present” | `activeThreats` | Threat list is not a hazard exposure survey | Existing threat/danger presentation only. |
| “Wildlife can use this site” | No land-owner field establishes this | Sector-level pack location and migration graph do not map to site suitability | Unknown unless an existing wildlife owner exposes that meaning. |
| “The land is healthy” | No current field set is sufficient | No soil, hydrology, vegetation or biodiversity truth in the location DTO | Do not display this statement in GR-1. |
| “Recovery is underway” | Would need explicit dimension-specific longitudinal evidence | No historic environmental samples in location save | Reserve for a GR-2 projection with valid comparison. |

The matrix should be revisited for every new label. A synonym does not repair a semantic mismatch: “clean,” “safe,” “restored,” “fertile,” and “alive” each assert different evidence than a contamination number or a depleted salvage factor.

## Appendix B — Worked read-only cases

### Case 1: authored location with no mutation record

The map knows the authored location and can show its name, description, and authored danger. `TryGetRecord` returns null. GR-1 returns `unknown / no live location record`; it does not call `GetOrCreateRecord`, write zero contamination, or infer that an untouched site is safe. The map remains usable, and its authored danger remains unchanged. This is a useful case because seed coverage is not equivalent to save coverage: the world seed catalog documents forty seeded location rows, while authored `locations.json` includes a broader location list.

### Case 2: world event adds a contamination fact

`WorldEvolutionEngine.ApplyEvent` can call `GetOrCreateRecord` for an event target and add `contamination_delta`; the catalog `event_evolution_rad_hotspot_bloom` targets `loc_cut_radiation_zone_alpha` and authors a delta of `0.35`. Its narrative describes a fallout plume. This creates a live location fact but not a sample history. The report may show the resulting current record after the owning update; it must not claim a field team measured it, identify a remediation cause, or extrapolate the next week's trend from that single delta.

The source’s catalog prose is not itself the numeric authority. The record value plus current day-tick result is. P0 should verify when this engine ticks relative to `LocationEvolutionSystem.TickDay` and whether the host captures it in the same world save; this plan does not prescribe order from the catalog alone.

### Case 3: clear site, contamination remains

An expedition completes. The existing phase-4 owner marks its location cleared, sets visit day, removes ownership to `none`, and raises loot depletion. If the record's contamination remains above zero, the readout reports both “cleared” and the current contamination value/status. Neither field overwrites the other. A completed expedition cannot be described as land treatment.

### Case 4: clear-weather drift lowers current contamination

On days where the live input is not hazard weather, the current implementation reduces positive contamination by `ContaminationClearDecay`, bounded at zero. This change is owned by the existing location tick. GR-1 may classify the current value at the next stable read; it does not accumulate daily observations, set an `improving` trajectory, or credit a steward. That comparison belongs to GR-2 and is only possible if an observation source retains comparable history.

### Case 5: ruin flag remains after a lower current value

`isRuined` is documented as sticky. If it was set at the contamination threshold, later lower contamination does not clear it. A compound display must therefore show both facts. GR-1 cannot reinterpret the persisted ruin fact as “currently contaminated” or silently unruin a site because the new band falls below a threshold.

### Case 6: map danger is authored “locked”

The map danger projection has a locked state that should not be crossed or replaced by condition bands. Even if a location condition is known, route accessibility and land status remain separate dimensions. The condition row must not participate in node locking, danger demotion, or selection behavior.

## Appendix C — Review checklist for any proposed labels

For each visible field, the implementer must be able to answer all rows below with one owner and a concrete source:

| Review question | Pass condition | Fail response |
|---|---|---|
| Which exact saved or authored value produces it? | Field and reader are cited by current source path/line | Keep it unknown or remove the label |
| Is the value a state, event, observation, or narrative statement? | Type and semantics match the sentence | Rewrite the sentence; never coerce a story line into state |
| Does it refer to a location or sector? | ID vocabulary and crosswalk are verified | Remove location-level wildlife/region implication |
| Is it current or historical? | Source day and record freshness are known | Label stale/currently unknown |
| Does it imply safety, yield or biodiversity? | A canonical owner exposes that exact outcome | Do not show implication |
| Can displaying it mutate state? | `CaptureState` is unaffected by open/refresh | Fix provider/UI before acceptance |
| Does it duplicate a neighbor plan? | Living Region / Second Nature / Reconstruction Tree boundary is preserved | Re-scope or stop |
| Are thresholds traceable? | Threshold owner, loader and validator exist | Do not add a data file or hard-code a hidden scale |

These rows are not a new validator or a test suite. They are review questions that make the report auditable without inflating the land model.

## Appendix D — Candidate projection contract and numeric review

The following is an implementation discussion aid. Names, bands, thresholds, tolerances, and error behavior remain **PROPOSED / VERIFY**. It is not an endorsed schema or balance decision.

### D.1 Input contract

| Input | Candidate representation | Validation before classification | Source owner |
|---|---|---|---|
| Stable location ID | non-empty ordinal string | Must resolve in current location catalog or location-seed references; display name is not a key | Existing location/map data |
| Record presence | explicit `HasRecord` plus immutable copy | `false` means unknown and must not synthesize a record | `LocationEvolutionSystem.TryGetRecord` |
| Contamination | finite normalized float expected in `[0,1]` by current live mutation rule | Non-finite/out of range is invalid evidence, not a band to clamp | `LocationEvolutionSystem` |
| Salvage depletion | finite normalized float expected in `[0,1]` by current `MarkDepleted`/recovery rules | Validate separately from contamination | `LocationEvolutionSystem` |
| Cleared / ruined | booleans | Do not infer one from the other; preserve sticky ruin | `LocationMutationRecord` |
| Current owner | authored/live owner string | Only relevant if the selected map surface displays control; do not use it as ecological health | Location/faction integration |
| Threat IDs | immutable ordinal set/list | Validate vocabulary via existing threat consumers; threat count is not contamination | Location owner |
| Source day | existing `lastEvolutionDay` or other confirmed source day | `-1` means unavailable; future relative to campaign clock is invalid/stale | Location and campaign clock |
| Threshold definition | optional validated config | No config means no new band unless a reviewed neutral behavior is chosen | Existing catalog loader only if real |

The first implementation should avoid passing the mutable `LocationMutationRecord` instance across Core/UI boundaries. If there is no suitable existing immutable query pattern, P0 decides whether a small value snapshot is justified. A new object for every location every frame would be wasteful; the map selection and detail path should build only the selected site's result.

### D.2 Output contract

Candidate output fields, only if a real consumer justifies them:

| Output | Meaning | Never means |
|---|---|---|
| `recordStatus` | present, absent, invalid | absent = clean |
| `contaminationBand` | bucket of the existing normalized contamination record | measured radiation dosage, water safety, plant safety |
| `disturbanceBand` | bucket of existing salvage/clear facts | ecological diversity or soil damage |
| `ruinStatus` | copied canonical sticky flag | current value crossed threshold this day |
| `threatSummary` | existing threat list summarized through its owner | biological population or land suitability |
| `sourceDay` | age anchor for the source data | exact day each field was last measured |
| `freshness` | current/stale/unavailable according to reviewed rule | confidence in an instrument reading |
| `invalidReason` | stable diagnostic enum for malformed source | a gameplay state or player-facing exception |

Outputs should carry source provenance at dimension level when source updates can differ. A single `updatedDay` is safe only if the owner proves all fields are changed under the same snapshot contract. Current location events and direct `TickDay` writes can touch different fields on different days; do not casually assert atomic measurement freshness.

### D.3 Numeric review examples — not approved thresholds

Current code clamps contamination to `0..1` during live tick and has `RuinedContaminationThreshold = 1f`. It reduces clear-weather contamination by `0.01f` and increases it on hazard days by `0.02f` multiplied by a radiation modifier factor. Existing event data includes a `0.35` contamination delta for `event_evolution_rad_hotspot_bloom`. These are simulation values; they do **not** imply usable player-facing low/medium/high thresholds.

For review only, an analyst might ask whether candidate bands such as `[0, 0.25)`, `[0.25, 0.60)`, `[0.60, 1]` would be legible. This is a **PROPOSED / VERIFY numeric example**, not a tuning recommendation. Before accepting any cuts, compare:

1. actual seeded contamination distribution;
2. the frequency of day-to-day crossings under clear/hazard weather;
3. values contributed by authored event deltas;
4. threshold stability across a save/replay;
5. existing consumers of `> 0.6`, `>= 1`, and any UI presentation;
6. whether a band explains a player decision or merely decorates the map.

If the distribution spends most of the campaign in one band, the band is not a useful read model. If a value oscillates around a proposed boundary, hysteresis may be considered only as a derived display policy with a real owner; a hysteresis counter must not be persisted in a panel. GR-1 does not add smoothing.

For depletion, a candidate `[0, 0.3)`, `[0.3, 0.7)`, `[0.7, 1]` also remains **PROPOSED / VERIFY**. The current system can increase depletion after a clear and can reduce it by `0.05` after the quiet interval. That means a label such as “salvage remains” could cross a band repeatedly over many days. P0 should decide if the raw value is more truthful and less fragile.

### D.4 Numeric acceptance table

| Input condition | Required projection behavior | Gameplay state affected |
|---|---|---|
| `0` | Candidate lowest contamination band or explicit raw zero after decision | None |
| Exact proposed boundary | One documented inclusive/exclusive rule; same result on all runtimes | None |
| `1` | Highest candidate band plus independent existing ruin flag | None |
| `-0.001` | Invalid/unknown and diagnostic; no silent “low” classification | None |
| `1.001` | Invalid/unknown and diagnostic; do not clamp for display as if canonical | None |
| `NaN`, `+∞`, `-∞` | Invalid/unknown; projection must not throw or stringify platform-specific text | None |
| `isRuined=true`, contamination below candidate top band | Preserve both facts | None |
| high depletion, zero contamination | Show separate dimensions | None |
| missing record | Unknown even though default DTO values would be zero | None |

For any selected threshold, tests should use the exact boundary and nearest representable values on each side; do not add dozens of random numbers. The intended test is the comparison contract, not a clone of the implementation's `if` statements.

## Appendix E — Event, day, and save traces

The following timelines are planning traces for P0 and future focused integration tests. Day numbers are symbolic except where an existing authored event is named. They must not be mistaken for a newly asserted live runtime order.

### E.1 Fresh campaign seed

1. Host loads `world_evolution_seeds.json` and calls `EvolvingWorldSeeder.Seed`.
2. The seeder only creates location records for authored `location_seeds` whose IDs have no current record; restored/player-touched records are not overwritten.
3. The map can now distinguish a known seeded record from an authored location with no live record.
4. The projector reports facts; it does not call the seeder or use display opening to fill missing locations.
5. Save captures the current world record through its existing owner.

Acceptance: calling the projection before and after repeated `Seed` calls cannot alter the record list, field values, or serialized capture. Seeding itself remains idempotent under existing behavior.

### E.2 One campaign day through normal host order

1. Campaign day coordinator begins day `N` and snapshots pre-day location state.
2. Phase-4 `EvolvingWorldDayOwner` reads weather kind and outdoor radiation modifier.
3. It calls `LocationEvolutionSystem.TickDay(N, inputs, fork(WorldEvolution,N,0))` once in the expected host path.
4. It processes world/expedition/faction consequences according to the rest of the day owner.
5. A post-commit map read takes one snapshot and reports the current facts with their available source day.

P0 must verify the “once” premise: the Core `TickDay` overload updates `lastEvolutionDay` but the source reviewed here does not itself reject a repeated call for the same day. The owner/coordinator is therefore the exactly-once boundary. A duplicate host call can apply another clear decay, another hazard gain, and extra seeded RNG draws. GR-1 must not paper over that by suppressing its own projection or adding a second daily guard.

### E.3 Authored hazard event around a day transition

`event_evolution_rad_hotspot_bloom` is authored for `loc_cut_radiation_zone_alpha`, adds `0.35` contamination, and adds a radiation threat. P0 traces its trigger from its registered world-event host through save snapshot and location mutation to the map refresh point. There are at least three possible ordering questions: event before location daily tick; event after tick; or event applied in another host step. Each produces a different current value if hazard weather also changes contamination. The read projection should report the final owner state and should not reverse-engineer cause order.

Focused future verification should create an input state with no location record and one with an existing record, invoke the actual event path, capture and restore, and compare the projected result. Tests should be written only if no current event/save test covers that precise sequence. Do not invoke a full world evolution suite just to test a pure band function.

### E.4 Day rollback after downstream failure

1. Pre-day snapshot contains contamination `C`, depletion `D`, ruin state `R`, and threat list `T`.
2. Phase 4 applies environmental tick.
3. A downstream failure causes the campaign owner to call `RestorePreDaySnapshot`.
4. World restore replaces location state from the captured copy; the projector then sees `C,D,R,T` again.
5. Retry of day N runs once under the same per-day RNG fork and produces the same result.

Acceptance: displayed state after rollback is derived from restored owner, not held in a view cache. The projection must not maintain an unrolled-back last-seen state. If capture/restore does not deep-copy fields used by the projection, that is an owner defect to report; do not add a second GR snapshot.

### E.5 Save taken after a mutation

1. A committed event or expedition mutation changes one or more location facts.
2. Existing save orchestrator captures `WorldHostSession` state under its established section.
3. Restart creates/loads the world host, restores location state, and binds data.
4. Seed initialization must not overwrite the restored record.
5. Projection result after load equals the pre-save result, except for an explicit, separately specified freshness input such as current campaign day.

The comparison should account for derived freshness: numeric condition and ruin must round-trip exactly; an “age” label may naturally advance with the campaign clock. Do not serialize a string such as “stale” or a calculated band.

### E.6 Malformed and partial saves

`RestoreState(null)` currently returns without changing state. `CloneState` expects a non-null `mutations` list when allocating its clone; JSON deserialization or corruption may produce null despite the DTO initializer depending on serializer behavior. It also skips null mutation rows and normalizes null threat/cache lists. P0 should inspect serializer, `WorldSaveStore`, and save recovery behavior before describing null top-level list safety as established.

Future acceptance cases: valid old state with no added fields; absent record; null row; null child list; unknown schema; malformed number; checksum failure; truncated file; fallback generation. The projection should be robust for any owner state the current restore contract intentionally accepts. It must not conceal invalid source by adding a synthetic “clean” record. If the owner rejects corruption before exposure, GR-1 should rely on that and avoid duplicate validation.

## Appendix F — Narrow implementation review protocol

1. Start from the P0 source census, not this draft's paths alone.
2. Write a one-page value contract with one row per output and one source per row.
3. Show three example inputs to a reviewer: missing record, seeded/normal record, and event-mutated record.
4. Mark every illustrative threshold `PROPOSED / VERIFY` until the owner and designer accept it.
5. Verify map output does not collide with authored danger, locked status, or route state.
6. Inspect old-save and capture/restore behavior before adding any field.
7. Implement only the projector/provider slice first; no content strings or gameplay hooks in the same change.
8. Use focused tests named for domain invariants; avoid a test for every sample output.
9. Add authored catalog only after loader, validator, and active consumer are identified.
10. Hand off unresolved threshold, UI route, and source freshness decisions rather than choosing them during code review.

An implementation that stops at a report of current state is complete if it is truthful. It need not add a trajectory, numeric score, or action to justify its existence.

## Appendix G — Write and read provenance inventory

This targeted source search was run during the continued audit on 2026-09-29 for location-owner methods and direct assignments to contamination, depletion, and ruin under `Assets/Ashfall.Core/` and `src/`. It is a search result to guide the next premise audit, not a guarantee against reflection, generated code, serializer writes, or unsearched project folders.

| Writer / reader | Observed operation | Provenance classification | GR-1 treatment |
|---|---|---|---|
| `EvolvingWorldSeeder.Seed` in `Assets/Ashfall.Core/EvolvingWorldCatalog.cs` | Creates a record for a seed only when `TryGetRecord` is null; writes owner and initial contamination; appends threats | authored initial condition | Baseline fact; do not overwrite restored state |
| `LocationEvolutionSystem.MarkCleared` | sets `isCleared`, `lastVisitedDay`, `currentOwner = none`, increases depletion | completed expedition consequence | Site-clear/salvage semantics only |
| `LocationEvolutionSystem.MarkVisited` | sets `lastVisitedDay` | visit fact | Not a measurement |
| `LocationEvolutionSystem.MarkDepleted` | adds nonnegative amount with clamp | scavenging pressure | Salvage only |
| `LocationEvolutionSystem.AddThreat` / `RemoveThreat` | edits active threat IDs and emits mutation notification on change | threat lifecycle | Existing danger input; not condition |
| `LocationEvolutionSystem.SetLocationOwner` | writes current owner | world/faction control | Ownership only |
| `LocationEvolutionSystem.TickDay` overloads | updates day; live version mutates depletion, contamination, ruin and threats | canonical day simulation | Sole daily condition update; report only after commit |
| `WorldEvolutionEngine.ApplyEvent` | mutates owner/depletion/threat and direct contamination field | authored world event | Include direct field write in source audit |
| `EvolvingWorldDayOwner` in `src/Main.CampaignOwners.cs` | calls tick; applies completed/failed expedition effects and dominance owner changes | host orchestration | Exactly-once and rollback boundary |
| `WastelandMapView.WorldEscalatedDanger` | reads threat count and ruin flag | presentation-derived danger | Existing map status, separate from GR-1 bands |
| `Main.EvolvingWorld.ComposeExpeditionDangerMultiplier` | reads active threats and contamination threshold | gameplay danger consumer | Do not feed GR-1 projection back into this seam |
| `Main.WorldPlaytest.cs`, `ExpeditionPanel.cs`, `Main.EcologicalInfestations.cs` | read location record for diagnostics/target qualification | adjacent consumers | Verify exact meaning and freshness |

### G.1 Similar field names do not identify the same owner

The search also found unrelated `contaminationLevel` fields in `SumpFloodingSystem`, `District8DeepCoastSystem`, and `HydroponicBiomeSystem`; cultivated plot soil values in the soil-reclamation/farming path; and location text describing contaminated ground. These are distinct state owners and units. A shared identifier spelling does not make them convertible. GR-1 must not read a sump node, greenhouse rack, or coastal campaign scalar to populate a `LocationMutationRecord` band unless a reviewed adapter gives an explicit canonical relationship.

### G.2 Reader inventory obligations

For each reader, P0 should note whether it does any of the following: creates a record; uses absent as zero; assumes a normalized range; uses the field to decide survival/travel/action; caches a result across load; formats it for the player; or depends on mutation events. Read-only projection safety is proven only when these access patterns are understood, not merely when the DTO compiles.

## Appendix H — Worked acceptance packet for a future reviewer

A review packet can be concise while proving the feature is coherent. Include:

1. a screenshot or text rendering of one site with no live record;
2. a captured `LocationMutationRecord` with current contamination plus threat list;
3. a separate record with `isRuined=true` and contamination below the top proposed band;
4. output for each under the candidate projector, with no numeric thresholds hidden;
5. before/after `CaptureState` hashes around map open and refresh;
6. a save/restore comparison for the same record;
7. the existing authored danger value and dynamic danger value for the same node, proving GR-1 has not overwritten either;
8. a source line for every row and every threshold;
9. explicit text for assumptions still unresolved;
10. a short test selector list showing why each target is in scope.

The reviewer can reject the plan if a line cannot be sourced. Do not expand the UI to compensate for missing data; reduce the claim instead.

## Appendix I — Candidate input adjudication and owner vocabulary

This table is a decision aid for a future source audit. It separates fields already present on `LocationMutationRecord` from tempting substitutions discovered in adjacent systems. The final label set must be an explicit projection over verified fields, not a general claim that a place has “healed.”

| Input or signal | Current owner / evidence | Candidate read treatment | Rejection reason or proof required |
|---|---|---|---|
| `contaminationLevel` | `LocationMutationRecord`; seeded by `EvolvingWorldSeeder`, changed by live location evolution and certain world events | Show current owner value with its actual range/meaning after source confirms normalization | Do not infer crop safety, water potability, dose safety, ecological return, or cleanup cause |
| `isRuined` | Location owner; set through site/event evolution | Separate structural disturbance indicator | Do not equate ruin with high contamination; both may differ |
| `isCleared` | Location owner; expedition/site-clear result | Label “cleared” only in its expedition semantics | Not restored, safe, cultivated, or uncontaminated |
| `lootDepletionFactor` | Location owner; `MarkDepleted` and quiet-site recovery logic | Keep out of land-condition rating unless product explicitly asks for salvage pressure | Do not turn scavenging into ecological damage or regrowth |
| `activeThreats` | Location owner; threat add/remove and map danger reader | Keep in existing danger surface; optionally link to report | Threat removal is not ecological improvement |
| `lastVisitedDay` | Location owner; set by visit/clear calls | Display only as a visit timestamp if a consumer needs it | It is not a sample timestamp and does not prove observation |
| `currentOwner` | Location owner / faction update path | Show jurisdiction only if map already shows it | It is not stewardship, public access, or habitat ownership |
| `CanonicalMapSurvey.LastConfirmedDay` | Cartography `ProjectCanonicalMap` | Freshness of map knowledge | No environmental dimension or instrument source exists in this DTO |
| InSAR `LastProcessedDay` / deformation summary | InSAR owner | A separately labeled geophysical result, if consumer is approved | Units and classification concern deformation, not land recovery |
| Wildlife `WildlifeObservation` | Wildlife ecology owner | A fauna observation with species/sector/day/confidence | Cannot be generalized as land condition or a universal field sample |

### I.1 Classification acceptance cases

The following examples intentionally use symbolic bands because the source contract and design thresholds still require review. `C` means the current contamination value after verifying source units; `R` means ruin; `T` means active threats; `V` means visited. No numeric cutoff is accepted by this appendix.

| Record | Proposed report | Acceptance reason | Rejected wording |
|---|---|---|---|
| Missing record | “Condition not recorded” | Absence remains visible and cannot be normalized to zero | “Clean” |
| `C` exists, `R=false`, no prior sample | “Current contamination record available; trend unknown” | Distinguishes a current fact from missing history | “Recovering” |
| `R=true`, `C` below a future contamination band | Two separate facts: structural ruin and current contamination band | Avoids one dimension overriding another | “Heavily polluted ruins” unless both predicates independently support it |
| `T` non-empty, `C` low | Condition and danger rows remain separate | Current danger reader already uses threat/ruin semantics | “Safe land” |
| `V` day known, `C` absent | Visit recency may be shown as map context; condition remains unavailable | Prevents visit timestamp from impersonating a sample | “Surveyed on day V” |
| Seeded contamination and a later direct event delta | Current result is reported with owner provenance if available | Event mutation can change current state without providing history | “Recovered by event” unless event is explicitly a remediation fact |

### I.2 Source-drift invalidation checklist

Re-run the premise check if a source changes the contamination range, event mutation path, call order, save DTO, or writer list. A classification derived from old ranges can invert meaning even while it still compiles. Recheck map consumers if danger and condition rows are merged, if the map begins caching node DTOs, or if a new survey fact is introduced. Any new direct writer to `contaminationLevel` requires owner attribution, a rollback rule, and a reviewer decision about whether it supplies an environmental observation. Until then, the safest accepted output is a direct current-value/context report with an explicit unknown state.

## Appendix J — Worked regional condition lifecycle

The scenario uses authored seed `loc_grange_hall` in `sector_4_hinterlands` (`world_evolution_seeds.json`), with seed contamination `0.1`, owner `none`, and no seeded threats. Values illustrate the existing tick formula; they are not a new Green Return rule. Assume a freshly seeded record, no world event, and one successful call to the existing location owner per campaign day.

| Day transition | Existing input | Existing contamination result | Other owner fact | Permitted report |
|---|---|---:|---|---|
| Seed day 0 | `EvolvingWorldSeeder` creates only if no record exists | `0.10` | Sector is authored in seed data | Current value exists; this is not yet a field-survey series |
| Day 1 | Clear weather (`LocationEvolutionInputs.Clear`) | `0.09` | Owner updates `lastEvolutionDay` | Current owner value; no intervention credit |
| Day 2 | Clear weather | `0.08` | No historic daily sample is appended | Current value; trend unsupported |
| Day 3 | Hazard weather, outdoor radiation modifier 300 | `0.12` (`+0.02 × max(1,300/150) = +0.04`) | Threats may independently sprout/decay on seeded rolls | Current contamination and hazard context may be separate rows |
| Day 4 | Clear weather | `0.11` | No cleanup event | Current value; no remediation claim |
| Day 5 | Clear weather | `0.10` | `lastEvolutionDay` advances | Equal to seed value, but not evidence of restoration |

The live tick mutates one current record; it does not append a measurement row for each day above. A day-5 save containing `contaminationLevel = 0.10` and `lastEvolutionDay = 5` cannot prove the intermediate values, player visitation, or sample day. Tick input and stored current state are not interchangeable.

### J.1 Alternate branch: event and visit have different meanings

The audited `WorldEvolutionEngine.ApplyEvent` path can mutate contamination for `event_evolution_rad_hotspot_bloom` at `loc_cut_radiation_zone_alpha` using an authored `contamination_delta`, and can add a threat. If this event changes a hypothetical value from `0.40` to `0.75`, the map may report current value and event-linked threat only where provenance is exposed. It cannot infer a player measurement. A later `MarkVisited(locationId, 6)` updates `lastVisitedDay` without adding a reading. `MarkCleared(locationId, 7)` sets cleared state, visit day, owner to none, and increases depletion; it still does not mean contaminated ground was treated. Any combined UI summary must preserve these distinctions.

### J.2 Candidate read-card field contract

This is a proposed presentation DTO, not a new save record. Each field must come from an existing public owner seam or be omitted.

| Proposed output | Input required | Interpretation | Missing-input result |
|---|---|---|---|
| `location_id` | Selected canonical location ID | Stable lookup key; not display name | No selection; do not query |
| `display_name` | Existing map/location presenter | Localized label only | Existing fallback policy |
| `record_available` | `TryGetRecord(locationId)` | Presence test; read must not create | False; dependent values unavailable |
| `contamination_current` | `contaminationLevel` | Current owner value; format only after range/unit audit | Unknown, never zero |
| `ruin_state` | `isRuined` | Owner structural/status fact | Unknown if no record |
| `expedition_cleared` | `isCleared` | Cleared status in expedition semantics | Unknown if no record |
| `active_threats` | `activeThreats` | Existing danger facts; not ecology grade | Unknown if absent record; empty only for present record with empty list |
| `last_visited_day` | `lastVisitedDay` | Visit recency only | No recorded visit when negative; never sample date |
| `condition_band` | Reviewed projection over actual fields | Versioned display classification | Unclassified until thresholds are accepted |
| `source_day` | Current owner day, if reliably exposed | Last canonical update, not sample day | Omit if not provable |
| `trend` | Approved comparable samples only | Direction or insufficient evidence | Insufficient evidence; no inference from `lastEvolutionDay` |
| `provenance` | Existing owner/source metadata | Names fact source or event when available | “Source not exposed,” not an inferred cause |

Do not add `habitat_score`, `restoration_percent`, `ecological_health`, `safe_to_harvest`, or `stewardship_effect` as convenient aggregate fields. Each would require a separate domain rule and owner absent from this read model.

### J.3 Malformed and migration acceptance cases

| Input anomaly | Projection treatment for review | Acceptance evidence |
|---|---|---|
| Null record list after restore | Follow existing owner recovery; UI must not instantiate it | Focused restore/provider test confirms handling |
| Duplicate `locationId` rows | Do not select arbitrary first/last | Owner validation/migration defines canonical resolution |
| Negative contamination | Invalid/unavailable unless current contract explicitly permits | Verified range; presenter does not silently clamp |
| Contamination above 1 | Invalid only after normalized range is reverified | Owner contract proves allowed range |
| Null threat list | Threat state unavailable unless restore already normalizes | No corrupt-null-as-no-threat inference |
| `lastVisitedDay > currentDay` | Stale/corrupt time input | Owner save validation or correction rule |
| Authored seed has no runtime record | Uninitialized/absent, distinct from zero condition | Seeder and restore order is proven |
| Record references unknown authored ID | Source mismatch/unknown location | View neither fabricates node nor drops save row |

These are future provider acceptance cases, not instructions for UI-side save repair. If the owner trusts malformed shapes without validation, record that gap for the owner instead of adding a Green Return normalizer.

## Appendix K — Save ownership, seeding, and rollback contract

The current owner state is `LocationEvolutionSaveState` (`schema_version = 1`, `systemId = location_evolution`, `lastEvolutionDay`, and `mutations`). `LocationMutationRecord` stores the fields listed in Appendix I. `CaptureState` deep-clones the list and nested threat/cache lists; `RestoreState` deep-clones a supplied state and returns without action for null. `EvolvingWorldDayOwner.CapturePreDaySnapshot` captures a location snapshot before its world tick; `RestorePreDaySnapshot` restores that snapshot and clears processed expedition IDs. These are the factual starting points for any provider or future owner change.

| Lifecycle boundary | Existing behavior | Green Return requirement |
|---|---|---|
| Fresh seed | `EvolvingWorldSeeder.Seed` adds a location record only if `TryGetRecord` is null | Read after seeding completes; do not race seed construction or write a default record from UI |
| Existing save plus current seed catalog | Existing record is retained; seeder does not overwrite it | New catalog defaults must not be displayed as the current saved value when record exists |
| Pre-day snapshot | Host captures location, wildlife, and landmark owner snapshots | Any read projection during transaction must be treated as provisional until commit |
| Daily tick | Location owner writes `lastEvolutionDay`, mutates contamination/depletion/ruin/threats; host supplies actual weather/radiation and day-specific seeded RNG | Map projection reads after successful day commit; projection consumes no RNG |
| Expedition consequence | Day owner applies `MarkCleared`/`MarkVisited` and threat/depletion outcomes | Project actual owner result; visit and clear do not imply measured remediation |
| Rollback | `RestorePreDaySnapshot` restores location owner state and clears processed expedition IDs | Discard stale UI projection and refresh from restored record |
| Save capture | Owner capture includes current state fields, not per-day contamination history | No trajectory may be reconstructed from `lastEvolutionDay` or current value |
| Restore | Deep clone preserves owner fields and nested lists | No migration-generated sample, condition band, or stewardship action is inserted |

### K.1 Proposed read DTO migration policy

The presentation DTO should be ephemeral unless an existing map cache already has an explicit transient owner and invalidation route. It should not add a second save section. On each map bind or invalidation, the provider queries the current location owner and makes a fresh projection. If an existing UI requires caching for performance, the cache key must include canonical location ID and owner version/revision if exposed; otherwise clear it on owner mutation, campaign load, day commit, rollback, seed completion, and selected-node change. This is a candidate integration rule, not evidence that current APIs expose a revision counter.

| Proposed UI/cache state | Lifetime | Persist? | Invalidated by |
|---|---|---|---|
| Selected canonical node ID | Existing map route lifetime | Follow current UI policy only | Node change/route close |
| Projected condition card | One owner snapshot/version | No new persistence | Owner mutation, day commit, rollback, load, seed completion |
| Source strings/localized labels | Existing catalog/UI lifecycle | Authored source remains authoritative | Catalog reload/localization refresh |
| User-selected display filter | Existing UI settings policy | Only if that UI already saves preferences | Route/settings lifecycle |
| Sample history | Measurement owner only, if separately approved | Never in GR-1 projection | Owner's correction/retention rule |

### K.2 Rollback acceptance fixtures

| Test fixture | Expected state after operation | Projection acceptance |
|---|---|---|
| Day tick succeeds; map refreshed | Location snapshot after tick | New report may reflect committed current value |
| Day tick executes; later day owner fails; host restores pre-day snapshot | Exact pre-day location state | Previous report returns; provisional state is discarded |
| Expedition clear applies, later phase fails and day rolls back | `isCleared`, owner, visit day, depletion revert with snapshot | No “cleared” card survives rollback |
| Mutation event fires during tick then rollback occurs | Core snapshot returns, observer/UI may already have seen transient callback depending on event bus | Future integration must prove transaction-aware refresh; do not preserve transient card as truth |
| Save occurs after successful tick and reloads | Deep-cloned state restored | Same projected output for same state and projection version |
| Map open during restore | Owner may not yet be ready | Show loading/unknown until restore completes; never call `GetOrCreateRecord` |
| New authored seed added after old save | Seeder adds only missing record under existing behavior | New record appears when owner seeding completes; old rows stay untouched |

One complication deserves explicit source review before code changes: `OnLocationMutated` notifications can occur as the live tick mutates threats/ruin, but the reviewed code does not demonstrate a transaction-aware event buffer in this plan. A projection should not subscribe directly and treat every intermediate event as committed truth until the host's event semantics are audited. A read-after-commit route is safer than adding another event subscriber that caches partial state.

### K.3 Determinism boundary

Condition projection itself should be pure and order independent except for stable display ordering. The location tick uses caller-supplied day RNG for threat sprout/decay; the day owner forks the existing campaign stream with `WorldEvolution` and discriminator 0 for location evolution and discriminator 1 for wildlife migration. GR-1 must not call tick, draw from either fork, seed a new RNG, sort-and-write the owner's records, or let dictionary iteration choose a band. A paired same-state test for a future projector should assert identical output and unchanged owner state/RNG snapshots. If the display adds a trend, the source owner must prove the observations are stable after replay; the read model itself should remain RNG-free.

## Appendix L — Existing consumers and presentation compatibility

The source audit found concrete consumers that should constrain any land-condition card.

| Consumer | Current read and meaning | Compatibility requirement for GR-1 |
|---|---|---|
| `WastelandMapView.WorldEscalatedDanger` (`src/World/WastelandMapView.cs`) | Reads `TryGetRecord(node.Id)` and raises authored danger one step for active threats and one for ruin, capped at high; it never alters `locked` semantics | Keep condition separate from danger; do not derive danger from a Green Return band or demote authored state |
| `Main.EvolvingWorld.ComposeExpeditionDangerMultiplier` (`src/Main.EvolvingWorld.cs`) | Applies a multiplier for active threats and adds `1.1` when current contamination is greater than `0.6f` | GR-1 is display-only; do not feed its projection back into this formula |
| `ExpeditionPanel.BuildWorldStateLine` (`src/UI/ExpeditionPanel.cs`) | Shows owner/unclaimed, depletion as “spoilage,” ruin and threat count, plus flavor; untouched record falls back to flavor | Preserve meanings and add condition as its own source-labeled detail; do not rename spoilage |
| Location target qualification/playtest consumers | Read specific fields for existing route or diagnostic purposes | P0 records their meaning and player visibility before broad map rollout |

This inventory does not establish that a Green Return condition label already exists. It identifies where existing fields affect behavior and where a new presentation could confuse meanings. Before implementation, compare selected location ID normalization: the map calls the API with `node.Id`, while expedition detail calls it with a target `locationId`. The catalog must prove those are the same canonical key; matching display names are insufficient.

### L.1 Map report compatibility cases

| Input case | Existing output that must remain | New output candidate | Regression rejection |
|---|---|---|---|
| Authored danger `low`, one active threat | Existing ladder may rise one step | Separate threat fact | Condition band must not add another danger step |
| Authored danger `low`, ruined, no threats | Existing ladder may rise for ruin | Separate ruin and condition rows | Ruin cannot be interpreted as contamination |
| Authored danger `locked`, no threat | Remains locked | Condition can be inspected only if existing route allows | Green Return must not unlock node |
| Contamination greater than `0.6f` | Expedition multiplier stays at existing 1.1 behavior | Show owner current value with verified meaning | Do not reuse threshold as a safety classification |
| Depletion `0.4`, contamination `0.1` | Expedition line uses existing “spoilage” wording | Condition is a separate line | Do not call salvage depletion ecological damage |
| No owner record, authored map node exists | Existing danger/flavor remain | Candidate “condition unrecorded” status | No `GetOrCreateRecord` call from map read |

The `0.6f` threshold is a gameplay-consumer detail, not an approved Green Return threshold. It cannot be copied into a safety label without domain review. If this consumer changes after the plan snapshot, rerun the source audit rather than treating this table as authority over gameplay.

## Appendix M — End-to-end condition evidence and acceptance mapping

Every displayed condition statement should be traceable through an owner fact, host read route, presentation field, and one observable acceptance check. The following map is deliberately small enough to audit on a single selected location.

| Claim displayed | Core source and method | Host/presentation route to verify | One high-signal acceptance check | Stop if |
|---|---|---|---|---|
| “A location record exists” | `LocationEvolutionSystem.TryGetRecord(locationId)` | Map selection route calls this read on canonical ID | Missing ID returns null and leaves save/RNG hashes unchanged | Existing route only offers mutating `GetOrCreateRecord` |
| “Current contamination value is X” | `LocationMutationRecord.contaminationLevel`; seed and `TickDay` writers | Map detail provider or existing location detail | Seed known value, tick controlled input, and assert displayed raw owner value maps correctly | Units/range/culture formatting are unknown or guessed |
| “This site is ruined” | `isRuined` set by existing threshold and sticky rule | `WastelandMapView.WorldEscalatedDanger` already reads it for danger | Ruin row appears while authored locked/danger semantics remain unchanged | UI claims contamination solely from ruin |
| “Site is cleared” | `MarkCleared` writes `isCleared`, owner, visit day, depletion | `ExpeditionPanel.BuildWorldStateLine` adjacent display | Test confirms cleared remains expedition status, not remediation | Existing copy implies cleanup and cannot be disambiguated |
| “Current threat exists” | `activeThreats` with add/remove/tick writers | Map danger and expedition target display | One threat changes existing danger as before; GR label adds no second escalation | New projection mutates threat list or danger ladder |
| “Player visited on day D” | `lastVisitedDay` set by `MarkVisited`/`MarkCleared` | Existing world state line or proposed evidence qualifier | Visit displays only as visit; no sample row created | UI presents visit day as environmental measurement date |
| “Condition is improving” | No current history in location record | No existing route identified in this audit | Must remain unavailable until two comparable measurements come from a real owner | Only latest value and `lastEvolutionDay` are available |

### M.1 Minimal fixture design

One targeted Core fixture can cover the pure projection: create a known location owner state through current APIs, call the proposed projector, and verify the output does not mutate `CaptureState`. A separate host fixture is needed only if a new adapter or route is added. The Core fixture should include:

1. a missing record and assert unknown rather than zero;
2. one seeded-like record with independent contamination, ruin, threat, visit, and depletion fields;
3. an event-mutated state to prove the projector reports final current facts without claiming event causality;
4. same input evaluated twice and compared for stable field ordering;
5. a state snapshot before/after proving no writes;
6. the `locked` map case if UI route code changes;
7. one old-save fixture if the underlying schema changes, otherwise no new migration test is warranted.

Do not create separate tests for every display string or repeat `TryGetRecord` behavior already covered by owner tests. The purpose is to verify the new projection boundary, not to duplicate location evolution, map danger, or save-store suites.

### M.2 Review receipt example

For `loc_grange_hall`, a reviewer should be able to follow the chain: `world_evolution_seeds.json` authors contamination `0.1` and sector `sector_4_hinterlands`; `EvolvingWorldSeeder.Seed` only inserts if no record exists; `LocationEvolutionSystem.TryGetRecord` reads without creating; current day tick modifies the live scalar based on weather; the provider returns that value with unknown trend; Cartography contributes map knowledge separately; wildlife contributes sector-scale context separately. If any link is not provable in the current checkout, the displayed statement is removed or marked unavailable.

The acceptance packet records exact source revision/date, source row, field mapping, caller route, save behavior, no-mutation evidence, accessible result, and all excluded claims. It does not require screenshots if the outcome is purely data-layer; if a visual surface changes, include one screenshot plus the state assertions that a screenshot cannot establish.
