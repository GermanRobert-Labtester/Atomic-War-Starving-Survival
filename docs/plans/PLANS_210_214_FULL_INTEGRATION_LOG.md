# Plans 210 + 214 — Full Integration Log

Date: 2026-09-23
Status: implemented and verified on the current worktree
Scope: close the "Partial — Core-only" gap on two signed Core authorities by
adding the missing host reachability, persistence, UI, and verification seams.

## Plan 210 — Survivor Personal Belongings & Effects

`PersonalBelongingsSystem` already held the claim/sentiment/favorite/gift/loss
authority and persisted inside the `survivor_social` aggregate, but the live
acquisition path was unreachable: the authored catalog was never loaded and no
surface could create or transfer a claim.

What was added:

- `src/Host/PersonalBelongingsHostSession.cs` — host session over the Core
  authority. Validates claims against the canonical inventory (`InventoryCount`)
  and the living roster (`SurvivorAlive`), and exposes claim, template grant,
  gift, favorite, and loss commands. Physical stacks and capacity stay with
  `Inventory`; inheritance stays with the death orchestrator.
- `src/UI/PersonalBelongingsPanel.cs` — dashboard surface to claim an inventory
  item definition, grant an authored keepsake template, toggle favorites, gift
  a claim to another survivor, and report theft/loss.
- `src/Main.PersonalBelongings.cs` — binds the session to the live inventory,
  roster, and campaign day; opens the panel; resets on new game.
- `src/Main.SurvivorSocial.cs` — loads `personal_belongings.json` into the
  coordinator's belongings system so template grants resolve against the JSON
  authority.
- `src/Main.Plans163_210.cs` — the existing public commands now delegate to the
  single host session instead of duplicating validation.
- `src/Host/PersonalBelongingsSelfTest.cs` — bounded headless verification.

No new save section: claims remain captured/restored by `survivor_social`.

## Plan 214 — Visitor Integration & Temporary Housing

`VisitorIntegrationSystem` was Core-only with zero `src/` references. The
missing middle between airlock admission and permanent residency is now wired.

What was added:

- Stable source identity: `VisitorRecord.SourceVisitorId` makes the airlock
  admission handoff idempotent and survives save/restore
  (`GetVisitorBySource`).
- `src/Host/VisitorIntegrationHostSession.cs` — opens one stay per admitted
  source id, auto-seeds the standard requirement set, draws daily rations
  through the canonical inventory delegate, and performs the recruitment
  handoff through a roster delegate. A refused handoff leaves the stay active.
  Routine processing uses no RNG.
- `src/Host/VisitorIntegrationSaveStore.cs` + `visitor_integration` registration
  in `SaveSectionRegistry` (`visitor_integration_save.json`, expanded-shelter
  lifecycle group).
- `src/UI/VisitorIntegrationPanel.cs` — active stays, housing allocation,
  requirement satisfaction, recruitment, and departure receipts.
- `src/Main.VisitorIntegration.cs` — binds rations to `canned_food`/`clean_water`
  via `Inventory.HasSufficient`+`TryConsume`, recruits through
  `SurvivorsHostSession.AddSurvivor`, and consumes the committed
  `AirlockSecuritySystem.OnIncidentResolved` admission exactly once (excluding
  `decon_subject`).
- `src/Host/VisitorIntegrationSelfTest.cs` — bounded headless verification.

## Shared wiring

- `Main.ExpandedShelterSystems` setup/save/tick/open/reset enrollment.
- `GameDashboardPanel` navigation: `VISITORS`, `PERSONAL EFFECTS`.
- `Main.PlayerSurfaces` expanded-panel ids.
- `HostCli`/`HostCliRegistry`: `--visitor-integration-selftest`,
  `--personal-belongings-selftest`, registered in both action enums and the
  generated `docs/ci/SELFTEST_MANIFEST.json`.
- `docs/architecture/ARCHITECTURE_TEST_MAP.md` regenerated from
  `scripts/ci/generate-architecture-map.py` (also repaired a missing comma that
  blocked the generator, and added the `visitor_integration` evidence row).

## Verification

- `dotnet build Ashfall.csproj` — succeeded, 0 warnings, 0 errors.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Visitors/Plan214VisitorIntegrationTests.cs` — **10/10 PASS**
- `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/Plan210PersonalBelongingsIntegrationTests.cs` — **7/7 PASS**
- `bash scripts/run_test.sh Ashfall.Core.Tests/SurvivorSocialCoordinatorTests.cs` — **9/9 PASS**
- `godot --headless --path . -- --visitor-integration-selftest` — **23/23 PASS**
- `godot --headless --path . -- --personal-belongings-selftest` — **20/20 PASS**
- Gate tests: `MainTriadDriftGateTests` 7/7, `SelfTestManifestGateTests` 4/4,
  `ArchitectureTestMapGateTests` 5/5, `PortContractGateTests` 8/8,
  `SaveStateRoundTripCoverageGateTests` 2/2, `UiAccessibilityGateTests` 3/3.
- `python3 scripts/ci/generate-architecture-map.py --check` — OK (243 subsystems).

## Boundaries preserved

- AirlockSecurity owns admission; SurvivorCatalog/SurvivorsHostSession owns
  permanent residency; Inventory owns physical stacks and rations; the
  SurvivorSocialCoordinator aggregate owns belongings persistence; the death
  orchestrator owns inheritance. Visitor monitoring remains a bounded
  administrative reference, not a second suspicion system.
