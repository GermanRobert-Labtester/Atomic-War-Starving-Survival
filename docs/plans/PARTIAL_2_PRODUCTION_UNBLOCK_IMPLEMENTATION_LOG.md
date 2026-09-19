# Partial-plan production unblock implementation log — follow-up

Date: 2026-09-19
Authority: direct user request following `PARTIAL_15_PRODUCTION_UNBLOCK_INTEGRATION_PLAN.md`

## Bounded outcome

Complete the next two partially integrated plans through their current owners:

1. Plan 133 / C1[20] — Expedition Discovery Consequences.
2. Plan 171 / C1[30] — Dynamic Quest Generation.

Non-goals: no new save section, no duplicate map/caravan/quest lifecycle owner,
no Unity restoration, no JSON authority fork, and no foreman-ledger edits.

## Ownership and files

The implementation extends the existing expedition aggregate/host, campaign
consequence ledger, procedural narrative host, and quest runtime. Unrelated
worktree changes were preserved; `INTEGRATION_PLANS.md`,
`WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, and `KNOWN_DEBT.md` remain untouched.

### Plan 133

- `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs`
- `Assets/Ashfall.Core/Expeditions/ExpeditionAggregate.cs`
- `src/Host/ExpeditionHostSession.cs`
- `src/Main.Expeditions.cs`
- `Ashfall.Core.Tests/Expeditions/DiscoveryConsequenceSystemTests.cs`

### Plan 171

- `Assets/Ashfall.Core/Quests/DynamicQuestGenerator.cs`
- `src/Host/ProceduralNarrativeHostSession.cs`
- `src/Main.Plans166_169.cs`
- `src/Main.NarrativeQuestlines.cs`
- `src/Main.UiPanels.cs`
- `src/UI/QuestsPanel.cs`
- `Ashfall.Core.Tests/Quests/DynamicQuestGeneratorTests.cs`

## Premise and authority decisions

- ExpeditionSystem remains the authority for whether a destination is known.
  `OnLocationDiscovered` is projected once into the consequence aggregate using
  the stable `expedition_discovery_<location>` ID.
- Discovery consequences persist inside the existing `expedition` aggregate;
  the host forwards typed outcomes to the existing campaign consequence ledger
  and only routes a non-zero typed faction delta to Year of Ash.
- The expedition status surface now reports discovery/consequence counts and
  the current caravan-safety projection. No panel recomputes consequence math.
- Plan 171 production no longer uses DynamicQuestGenerator's legacy local
  lifecycle. Its production adapter asks `ProceduralNarrativeSystem` to select
  a JSON-backed deterministic candidate and registers it in the existing
  `QuestRuntimeCoordinator`, which is already captured by
  `procedural_narrative` save state. The legacy instance API remains only for
  compatibility with its focused contract tests and is not composed by Main.

## Phase log

### Phase 1 — Plan 133 / C1[20]

- Added idempotent `RegisterExpeditionDiscovery(locationId, day)` with a stable
  discovery ID and one persistent provenance consequence.
- Added `DiscoveryConsequenceState` to `ExpeditionAggregateState`; the host
  captures/restores it through `ExpeditionHostSession` and raises a typed host
  event for canonical consequence consumers.
- Bound the canonical expedition discovery event in Main and recorded the
  resulting outcome in `CampaignConsequenceLedger`; standing changes remain
  owned by Year of Ash and are applied only for typed outcomes that carry a
  non-zero delta.
- Legacy aggregates without the new field safely restore with an empty
  projection; existing known-location truth is not rewritten during load.

### Phase 2 — Plan 171 / C1[30]

- Added `TryGenerateAndRegisterCanonicalCandidate(...)` as the narrow bridge
  from JSON template authority to `QuestRuntimeCoordinator`.
- Updated `ProceduralNarrativeHostSession.GenerateAndRegister` to use that
  bridge while preserving deterministic RNG ownership and rejection feedback.
- Added Main's `TryGenerateProceduralQuest()` command. It builds a sorted,
  read-only snapshot from live survivors, known expedition locations, the item
  catalog, and the canonical quest read model, then forks the narrative stream
  by day and a fixed action lane.
- Added the request button and live runtime summary to the existing Quests
  panel; the panel emits a command and reads back the canonical runtime rather
  than owning generation or lifecycle state.

## Verification

Baseline and post-change focused checks:

- `DiscoveryConsequenceSystemTests.cs` — **7/7 passed**.
- `DynamicQuestGeneratorTests.cs` — **7/7 passed**.
- `Plan169ProceduralNarrativeTests.cs` — **6/6 passed**.
- `DynamicQuestlineTests.cs` — **4/4 passed**.
- `dotnet build Ashfall.csproj --no-restore -p:BuildInParallel=false -v:minimal` — **0 warnings, 0 errors**.
- `bash scripts/ci/run-godot-bounded.sh --path . --log-file /tmp/ashfall-plan133-171-narrative.log -- --narrative-selftest` — **10/10 passed** at the required 15 FPS.
- `bash scripts/ci/run-godot-bounded.sh --path . --log-file /tmp/ashfall-plan133-expedition.log -- --expedition-selftest` — **40/40 passed** at the required 15 FPS.

The first Godot invocation used the default `user://logs` destination and
crashed before project startup because that directory was unavailable in the
managed environment. The same bounded runtime check passed with an explicit
`/tmp` log path; no project assertion failed.

## Handoff and limitations

- No shared ledger or ownership file was changed.
- Existing untracked/dirty work from other packages was not reset or cleaned.
- Plan 133's expedition-destination projection currently records provenance;
  it does not invent a safety or faction effect when the source fact carries
  no such typed delta. Existing threat-clearing outcomes retain their Core
  safety calculation and are observable through the same aggregate surface.
- Plan 171's compatibility-only legacy generator methods still exist for the
  focused tests; production composition uses only the canonical JSON/runtime
  bridge described above.
