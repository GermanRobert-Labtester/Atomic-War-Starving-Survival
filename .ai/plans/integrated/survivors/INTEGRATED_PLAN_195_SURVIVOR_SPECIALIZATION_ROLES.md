# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**

# Plan 195 — Survivor Specialization Roles — Full Host Integration

**STATUS: APPROVED BY USER**
**Authorized by:** user directive in session ("Find a plan to start integrating, fully integrate, no partial") — 2026-09-26.
**Claim:** `claim-plan195-survivor-roles-integration-2026-09-26`
**Signed design authority:** `DEC-185` (Survivor Specialization Roles System, SIGNED) + the plan's PFGL Codex Luna 6 execution revision (2026-09-25).

## Bounded outcome

Convert the Core `SurvivorRoleSystem` (DEC-185, sealed 2026-09-21) from a DEC-185
Core-only authority into a fully wired host feature: one production host session
and restore path for role identity, discipline-gated assignment, earned role
practice, and a read-only specialization readout on the existing survivor detail
route. No second roster, no operational auto-actions, no new gameplay authority.

## Premise corrections (evidence-first)

- The audited 2026-09-19 queue is drained: CF-P1/P5/P6/P28, Plan 37, Plan 48,
  CF-XP01 all sealed; E1/Plan 53 has an ACTIVE claim (not raced). Plan 195 was
  selected from the 2026-09-24 closeout custody table + PFGL 2026-09-25 revision.
- The authored `required_skills` keys (medicine, negotiation, firearms, …) do
  **not** exist in `skills.json`; the SkillProgression owner's truth is six
  disciplines. Social/leadership/trade milestone skills are authored under the
  `survival` discipline. Eligibility is therefore enforced additively through
  new `required_discipline` / `required_discipline_level` catalog fields read
  from the skill owner's normalized 0–100 discipline levels; the legacy
  `required_skills` dict path and its tests remain untouched.
- The named work-completion producer is `SkillProgressionSystem.OnXpGained`
  (fires exactly once per verified completed-work fact). Role XP: fixed +10 per
  matching-discipline fact; deterministic, no RNG.

## Files changed

- Core: `Assets/Ashfall.Core/Survivors/SurvivorRoleSystem.cs` (additive DTO
  fields + `CanAssignRoleByDiscipline`), `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`
  (`survivor_roles` section), `Assets/Ashfall.Core/HostCliRegistry.cs`
  (`SurvivorRolesSelfTest` + descriptor).
- Data: `Assets/StreamingAssets/Data/survivor_roles.json` (schema_version 2,
  discipline gates per role; legacy `required_skills` preserved).
- Host: `src/Host/SurvivorRoleHostSession.cs` (+ `SurvivorRoleSaveStore`,
  `SaveStoreHub.Checksummed<SurvivorRoleState>`), `src/Main.SurvivorRoles.cs`,
  `src/Host/HostCli.SurvivorRoles.cs`, `src/Host/HostCli.cs` (enum/parse/help),
  `src/Main.Application.cs` (dispatch), `src/Main.SaveOrchestrator.cs`
  (setup/save), `src/Main.Lifecycle.cs` (reset), `src/Main.PlayerSurfaces.cs`
  (RoleProvider bind), `src/UI/SurvivorDetailPanel.cs` (read-only row).
- Tests: `Ashfall.Core.Tests/Survivors/Plan195SurvivorRoleWiringTests.cs` (new).
- Gate repairs (pre-existing red at HEAD, outside this claim, documented):
  `Ashfall.Core.Tests/Survivors/Plan204RecruitmentIntegrationTests.cs`
  (`admittedCand.Status` → current `IsRecruited` API) and the recruitment help
  line in `src/Host/HostCli.cs` (parsed flag undocumented).
- Generated: architecture map (+1 subsystem), save-store matrix, self-test
  manifest, CLI catalog, catalog registry, docs index.

## Acceptance (all verified)

1. Focused xUnit: `Plan195SurvivorRoleIntegrationTests` + `Plan195SurvivorRoleWiringTests` 13/13.
2. Adjacent gates: SaveSectionRegistry 5/5, HostCliActionParityGate,
   HostCliHelpContract, MainTriadDriftGate, Plan204Recruitment — 36/36 combined.
3. Headless Godot: `--survivor-roles-selftest` 12/12; `--data-integrity-selftest`
   427/427 catalogs 0 errors; `--player-panels-uitest` PASS (21/21 lifecycle);
   `--content-utilization-selftest` PASS (CI + deep-chain; survivor_roles shares
   the pre-existing UNRESOLVED manifest class of host-loaded catalogs — not a
   regression).
4. Builds: Core, tests, host 0 errors / 0 new warnings.
5. Generators `--check`: architecture map 270 OK, save-store matrix 272 OK,
   CLI catalog 274 OK, self-test manifest 210 OK.
