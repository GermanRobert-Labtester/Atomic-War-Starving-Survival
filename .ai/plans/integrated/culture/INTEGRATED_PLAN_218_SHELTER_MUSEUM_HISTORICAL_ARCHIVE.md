# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

> **FULLY INTEGRATED 2026-09-26** — user-authorized ("find 4 plans to fully integrate, don't leave as partials"). Archival performed this session. No commit.

> **Final acceptance evidence:** implementation live and archived previously; this session moved the `.ai` approved plan to the integrated archive and re-confirmed the mandatory header. No duplicate authority or save section introduced.

# Plan 218 — Shelter Museum & Historical Archive — Full Host Integration

**STATUS: APPROVED BY USER**
**Authorized by:** user directive in session ("find the next plan to fully integrate") — 2026-09-26.
**Claim:** `claim-plan218-shelter-museum-integration-2026-09-26`
**Signed design authority:** `DEC-203` (Shelter Museum & Historical Archive System, SIGNED) + the plan's PFGL Codex Luna 6 execution revision (2026-09-25).

## Bounded outcome

Convert the Core `ShelterMuseumSystem` (DEC-203) from a Core-only authority into
a fully wired host feature: one production host session and restore path under
its own `shelter_museum` save key, daily exhibition expiry riding the existing
`TickPlans46_49` world/day orchestration, explicit once-per-day survivor visits
with the morale delta applied exactly once through the canonical Needs owner,
and a read-only museum projection (plus the explicit visit command) on the
existing `archive_desk` route. Physical-inventory donation stays unexposed.

## Premise verification (evidence-first)

- The remaining census candidates were checked against live source: Plans 188
  (`SurvivorRoutineHostSession`), 176 (`AgingHostSession`), 199
  (`HumanMigrationHostSession`) are already hosted; Plan 191 is retired (C3);
  Plan 193 and Plan 197 require signed decisions; Plan 190 requires an
  item-instance identity architecture first. Plan 218 was the strongest
  remaining full-integration target (DEC-203 signed, Core 7/7 tests, unhosted).
- `VisitMuseum` mutates visitor counts on every call — the host path uses a new
  additive `TryVisitMuseum(visitorId, currentDay, out morale)` with an additive
  `VisitorLastDay` ledger on `ShelterMuseumState` (old saves restore to an
  empty ledger); a panel refresh can never increment visitors.
- `CulturalArchiveVaultSystem` keeps its own `cultural_archives` section; the
  museum gets its own `shelter_museum` key per the revision ("keep the museum
  save key separate from … the cultural vault").
- `CurateExhibition`'s last parameter is `durationDays` (EndDay = start +
  duration), not an absolute end day.

## Files changed

- Core (additive): `Assets/Ashfall.Core/Culture/ShelterMuseumSystem.cs`
  (`VisitorLastDay`, `TryVisitMuseum`, `GetAllTemplates`, `MuseumName`,
  capture/restore of the ledger), `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`
  (`shelter_museum` section), `Assets/Ashfall.Core/HostCliRegistry.cs`
  (`ShelterMuseumSelfTest` + descriptor).
- Host: `src/Host/ShelterMuseumHostSession.cs` (new; + `ShelterMuseumSaveStore`
  via `SaveStoreHub.Checksummed<ShelterMuseumState>` + `ShelterMuseumSnapshot`),
  `src/Main.ShelterMuseum.cs` (new), `src/Host/HostCli.ShelterMuseum.cs` (new,
  12 checks), `src/Host/HostCli.cs` (enum/parse/help), `src/Main.Application.cs`
  (dispatch), `src/Main.SaveOrchestrator.cs` (setup/save), `src/Main.Lifecycle.cs`
  (reset), `src/Main.Plans46_49.cs` (`TickShelterMuseum(day)` inside the
  existing daily orchestration — no edit to C1-owned `Main.CampaignOwners.cs`),
  `src/Main.ShelterBatch3.cs` (3 additive provider-binding lines),
  `src/UI/ArchiveDeskPanel.cs` (museum section: read-only inspection +
  survivor OptionButton + one explicit RECORD VISIT button).
- Tests: `Ashfall.Core.Tests/Culture/Plan218MuseumHostWiringTests.cs` (new).
- Generated: architecture map (+1 subsystem), save-store matrix, self-test
  manifest, CLI catalog, docs index.

## Acceptance (all verified)

1. Focused xUnit: `Plan218MuseumIntegrationTests` + `Plan218MuseumHostWiringTests` 13/13.
2. Adjacent gates: SaveSectionRegistry, HostCliActionParityGate,
   HostCliHelpContract, MainTriadDriftGate, Plan195, Plan204 — 51/51 combined.
3. Headless Godot: `--shelter-museum-selftest` 12/12; `--data-integrity-selftest`
   427/427 (0 errors); `--player-panels-uitest` PASS (21/21 lifecycle, Errors: 0).
4. Builds: Core, tests, host 0 errors.
5. Generators: save-store matrix 274 `--check` OK, self-test manifest 212 OK,
   CLI catalog 276 OK; architecture map regenerated (272 subsystems; the sole
   residual `clothing_warmth` validation error is the concurrent Plan 142
   lane's in-flight drift — deliberately untouched per Rule 6).

## Explicit non-goals

- No donation from physical inventory (custody bridge not transaction-safe).
- No second cultural archive store; no absorption into `cultural_archives`.
- No new day owner file edit (`Main.CampaignOwners.cs` stays C1-owned); the
  daily tick rides `TickPlans46_49` like Plan 211's expiry.
- No visit farming: one recorded visit per survivor per campaign day, morale
  applied exactly once through `Needs.Modify(NeedKind.Morale)`.
