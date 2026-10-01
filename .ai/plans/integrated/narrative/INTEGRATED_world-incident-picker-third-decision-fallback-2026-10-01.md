# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> (Approved by user: "Well do the flag!" / "continue with the flag!" — the
> flagged open item from the week-1 decision reachability task: wire a
> weighted runtime picker for events.json.)

# World incidents (events.json) — weighted runtime picker as the third decision fallback

Date: 2026-10-01
Claim: `claim-world-incident-picker-2026-10-01`

## Bounded outcome

The 240 authored events in `events.json` had no runtime picker: weight,
maxDay, conditions, and choices were dead data (only id/title/bodyText/minDay
were read by the Phase0 prose dispatch, the journal codex, and the events
log read-model). This plan wires the catalog into the EXISTING per-day
decision stream as the third fallback (narrative arc, then field echo, then
world incident) — no parallel manager, no new modal, no new save architecture.

## Non-goals

- No edits to `events.json` (its diff is foreign week-1 tuning).
- No new typed-effect owners: the 33 authored effect kinds stay fail-closed
  (only `faction_standing` is bound in v1); trust/trait gates and
  string-valued world flags stay non-executable with validation errors.
- No new modal surface: incidents reuse `NarrativeArcModal` via the
  presentation-only constructor (the echo pattern).
- Week-1 behavior unchanged (pinned by `WeekOneDecisionReachabilityTests`).

## Design decisions

1. **Third fallback, same owner.** `NarrativeQuestsVerdictDayOwner.TickDay`
   draws an incident only when arc == null && echo == null, using a new
   `CampaignStreamIds.WorldIncident` fork (snake_case, StableHash-derived, so
   no other stream's derived seed shifts). Pending incident auto-opens after
   the daily briefing (`OnBriefingAcknowledged` else-chain) and routes through
   `OnNarrativeArcChoiceSelected` by pending-id match (echo prefix routing
   stays first).
2. **Core engine, engine-free.** `Ashfall.Core/Narrative/WorldIncidentSystem.cs`:
   day-window/condition gating, deterministic weighted selection,
   once-only resolution, scheduled follow-ups, informational auto-resolve
   (no-choice incidents resolve on surface and never block the pending slot),
   capture/restore. Weight 0 is honored as authored schedule-only intent
   (silent_knock parts 2/3, emissary_returns) — excluded from the random pool,
   still eligible via schedule.
3. **Fail-closed consequence port.** `IWorldIncidentConsequencePort`
   (preflight-then-apply, the arc-system pattern). The host port binds:
   morale/needs via `Needs.Modify` on living shelter residents, the authored
   need id `radiation` to the dose ledger (`RadiationSystem.AdjustDose` on
   shelter residents — the deltas are dose-scale 0..100), items via
   `AddById`/`RemoveById` (negative amounts route through `RemoveById`),
   world flags via `_consequenceLedger`, weather via `_world.Weather.Current`,
   faction standing via the existing `CanApplyNarrativeStanding`/
   `ApplyNarrativeStanding` seam. Unmapped needs and unbound owners reject
   with reasons.
4. **Save section.** `world_incidents` (checksummed envelope,
   `world_incidents_save.json`) mirroring `echoes` exactly: registry row,
   filename map, `SetupWorldIncidents` in `RestoreAllSubsystemsFromDisk`,
   `SaveWorldIncidents` in `SaveAll`. Section pins 314→315, 308→309.

## Files

New: `Assets/Ashfall.Core/Narrative/WorldIncidentSystem.cs`,
`src/Host/WorldIncidentHostSession.cs`, `src/Host/WorldIncidentSaveStore.cs`,
`src/Main.WorldIncidents.cs`, `src/Host/HostCli.WorldIncidents.cs`,
`Ashfall.Core.Tests/Narrative/WorldIncidentSystemTests.cs`.

Edited: `Assets/Ashfall.Core/Random/CampaignRngStream.cs` (new stream id),
`src/Main.CampaignOwners.cs` (third draw), `src/Main.Campaign.cs` (briefing
else-chain), `src/Main.Narrative.cs` (choice routing),
`src/Main.SaveOrchestrator.cs` (setup/save calls),
`Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` (row + filename map),
`Assets/Ashfall.Core/HostCliRegistry.cs` (enum member + descriptor),
`src/Host/HostCli.cs` (enum member + parse + help),
`src/Main.Application.cs` (dispatch case), section-pin test files
(`ComprehensiveSaveStoreCorruptionAndMigrationTests`, `VersionReportContractTests`),
`docs/ci/SELFTEST_MANIFEST.json` (regenerated 316→317).

## Verification

- `dotnet build Ashfall.csproj`: 0 errors (6 pre-existing warnings in untouched HostCli files).
- `bin/run-scoped-tests WorldIncidentSystemTests`: 11/11 PASS.
- Scoped: `WeekOneDecisionReachabilityTests` 6/6, save-corruption pins,
  `VersionReportContractTests`, `HostCliActionParityGateTests` 4/4,
  `MainTriadDriftGateTests`, `CampaignRngSourceGateTests`,
  `Plan138LowBackgroundHostWiringTests`, `HostCliHelpContractTests` 2/2 — all PASS.
- Headless: `--world-incidents-selftest` 12/12 PASS; `--data-integrity-selftest`
  430/430; `--seven-day-smoke-selftest` PASS; `--day1-selftest` PASS;
  `--real-campaign-journey-selftest` PASS; `--reasonable-player-selftest` PASS;
  `bin/ashfall-dev validate-json` 714/714; manifest `--check` OK (317 cataloged).

## Known limitations (flagged, not fixed)

- 33 typed effect kinds (remove_water, add_trait, combat_encounter, ...) and
  trust/trait gates remain non-executable with validation errors; binding them
  is future work behind this seam.
- String-valued world flags (schooling rows, worldFlagValue 'letters' etc.)
  fail closed: the campaign ledger is bool-only.
- If every choice of a pending incident is refused at runtime (e.g. negative
  item grant without stock), the incident stays pending — same accepted
  contract as arcs/echoes.
- Pre-existing help-contract gap fixed additively: the foreign uncommitted
  probe aliases (`--gameover-restart-selftest`, `--reasonable-player-bot-selftest`,
  `--play-on-selftest`, `--chapter-selftest`) were parsed but undocumented;
  their help lines now name them.

No commit (repo convention); full suite not run.
