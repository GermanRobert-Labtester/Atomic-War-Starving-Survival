# Plan 143 completion report

Verified 2026-09-09 against the current Godot host, Core library, data
authority, and save envelope.

## Delivered runtime

The 15 records in `narrative_arc_events.json` now load through the strict typed
`NarrativeArcEventCatalogLoader` and execute through the bounded
`NarrativeArcEventSystem`. The loader preserves the authored IDs, validates the
fixed Plan 143 event graph, converts effects into five typed whitelist classes,
and makes unsupported effects non-executable. There is no reflection,
expression evaluation, method-name dispatch, or arbitrary JSON command path.

The Godot host performs one deterministic daily candidate draw on the existing
`CampaignStreamIds.Narrative` stream, persists a pending event, and presents it
through `NarrativeArcModal`. Choice selection is separate from selection and
routes through `NarrativeArcConsequenceAdapter`. The existing narrative
encounter engine remains the authority for its older encounter catalog.

## Inventory and contract result

- Events: **15**
- Choices: **18**
- Effect records: **17**
- Effect verbs: **5**
- Survivor references: `aris_thorne`, `maya_lin`, `victor_vance`,
  `elena_rostov`
- Authored faction aliases: `iron_garrison` and `ash_militia`
- Location: `loc_missile_silo`
- Morale values: `-25` once, `-15` twice, `-10` twice, `-5` twice, `0`
  four times, `+5` three times, `+10` three times, `+15` once

Effect dispositions:

| Verb | Result |
|---|---|
| `advance_narrative_arc` | Bounded stage 1 → stage 2 transition in `NarrativeArcEventSystem` |
| `narrative_arc_branch` | One mutually exclusive `a`/`b` branch token, then stage 3 |
| `gain_faction_intel` | `JournalSystem.Knowledge.Discover` using a canonical namespaced key |
| `start_expedition` | Persisted destination offer; later dispatch uses normal expedition gates |
| `faction_standing` | One call to `FactionWarSystem.ModifyStanding` |
| `moraleDelta` | One call to `NeedsSystem.Modify` after all preflights pass |

Canonical aliases are resolved as `iron_garrison` →
`faction_central_garrison` and `ash_militia` → `faction_upland_militia`.
`loc_missile_silo` resolves through `locations_expansion3.json` as folded into
the expedition definition registry. Event and choice IDs remain unchanged.

## Arc graphs and activation

Aris Thorne, Maya Lin, Victor Vance, and Elena Rostov each use the explicit
three-stage graph `stage_1 → stage_2 (branch a or b) → stage_3`. Stage 2 cannot
be selected before stage 1 commits; stage 3 is terminal and acknowledged once.
Character events require the addressed survivor to be alive, resident, and in
the shelter interior at selection and resolution time. The three independent
events are one-shot daily-pool events with no survivor prerequisite.

All 15 records are registered in the production host and included in the
normal daily candidate pool. Character arcs remain correctly gated until their
canonical survivor is actually present in the live roster; the current default
three-member demo roster does not silently substitute those four named
survivors. No authored event is deferred because of an unsupported effect.
Forced expedition dispatch is deliberately deferred to the normal expedition
panel and command path after the narrative offer is recorded.

## Narrative and reference audit

All four survivor IDs resolve in `survivors.json`. Faction references resolve
through `FactionStandingIdResolver`. The missile-silo location resolves in the
active expedition catalog. No current authority supports an independent intel
score, physical cracked-floor damage, exact food-percentage mutation, medical
supply requisition, permanent Ash Rot cure, or new radio network, so the prose
now presents those details as context, uncertainty, research, or rumor. The
cult event keeps radiation-immunity claims unconfirmed. Victor's choices do not
recruit refugees, and turning away the garrison defector does not recruit them.

## Atomicity and persistence

`CanApplyChoice` validates the pending event, graph transition, survivor,
morale target, canonical faction, journal authority, and expedition location
before any mutation. The commit callbacks are void after this barrier; the
arc state is assigned only after the preflighted downstream commits complete.
Duplicate commits return `AlreadyCommitted`. Restore clones pending/completed
arc state and never invokes consequence callbacks.

The existing `NarrativeEncounterState.arcState` field stores only pending event
identity, completion IDs, four bounded progress records, offer tokens, and
choice resolution records. Morale, standing, knowledge, expedition sorties,
inventory, medical state, and survivor state remain in their owning sections.
Pre-Plan-143 saves may omit `arcState` and load with an empty arc ledger. A
late save does not replay missed outcomes; it may adopt only a valid future
stage 1 through ordinary selection. Catalog reordering does not affect pending
or completed IDs or the ordinal-sorted RNG candidate order. Missing optional
catalog data leaves the arc system inert.

## Negative fixtures and regression coverage

The focused suite covers unknown effects, duplicate event and choice IDs,
unknown survivor and faction payloads, invalid branch IDs, stage gaps,
negative/NaN weights, absent survivors, invalid expedition locations, failed
standing/expedition preflight, stage ordering, branch exclusivity, duplicate
commit, restore without replay, catalog-order independence, and a seeded
day-1–40 selection simulation. The focused Plan 143 suite passes **13/13**;
the combined Plan 143/save-wire filter passes **14/14**; and a complete
full-project run during this integration passed **10,285/10,285**. The latest
full-project retry was blocked before test execution by unrelated modified
Plan 147 tests in `Ashfall.Core.Tests/Narrative/ContrabandPlan147Tests.cs:473-485`,
which reference missing `Ashfall.Core.Tests.Medical.ChemicalDependencySystem`
and `ChemicalDependencyKind` types.

The required implementation documents are:

- `PLAN143_BASELINE.md`
- `PLAN143_EVENT_INVENTORY.md`
- `PLAN143_EFFECT_CONTRACT_MATRIX.md`
- `PLAN143_ARC_GRAPH.md`
- `PLAN143_REFERENCE_AUDIT.md`
- `PLAN143_CONSEQUENCE_AUTHORITY_MAP.md`
- `PLAN143_ATOMICITY_POLICY.md`
- `PLAN143_SAVE_COMPATIBILITY.md`
- `PLAN143_NARRATIVE_ACCURACY_AUDIT.md`
- `PLAN143_REGRESSION_MATRIX.md`

## Utilization and verification

The runtime content collector now opens, deserializes, registers, queries, and
exercises the Plan 143 catalog through the real typed system over days 1–40.
`--content-utilization-selftest` passed with **1,027** runtime utilization
events, **582** catalogs scanned, **222** gameplay-consumed catalogs, **0**
orphaned catalogs, and a passing CI utilization gate.

| Command | Result |
|---|---|
| `godot --headless --path . -- --data-integrity-selftest` | PASS — 299 catalogs, 0 errors, 0 warnings |
| `godot --headless --path . -- --content-utilization-selftest` | PASS — runtime collector and CI gate |
| `godot --headless --path . -- --narrative-selftest` | PASS — 10/10 |
| `godot --headless --path . -- --expedition-selftest` | PASS — 40/40 |
| `godot --headless --path . -- --medical-selftest` | PASS — 15/15 |
| `godot --headless --path . -- --year-of-ash-save-selftest` | PASS |
| `godot --headless --path . -- --expedition-encounter-bridge-selftest` | PASS |
| `godot --headless --path . -- --save-load-ui-failure-selftest` | PASS — 8/8 |
| `dotnet build Ashfall.Core/Ashfall.Core.csproj --no-restore --verbosity:minimal` | PASS — 0 warnings, 0 errors |
| `dotnet build Ashfall.csproj --no-restore --verbosity:minimal` | PASS — 0 warnings, 0 errors |
| `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --no-restore --verbosity:minimal` | PASS on prior complete run — 10,285/10,285; latest retry blocked before execution by unrelated Plan 147 compile errors at `ContrabandPlan147Tests.cs:473-485` |
| `python3 scripts/ci/run-gates.py --tier fast` | BLOCKED by unrelated trailing blank line in `Assets/Ashfall.Core/Narrative/BunkerCourtCatalog.cs:277` |

The fast gate result is a repository hygiene failure in concurrent Plan 146
work and is outside Plan 143's touched surface. Plan 143 code and focused
regressions are green. The full Core suite has one complete green run recorded
above; the latest retry is blocked by the unrelated Plan 147 test compilation
errors described above.
