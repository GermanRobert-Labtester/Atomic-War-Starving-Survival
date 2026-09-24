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

## Post-integration programmatic audit + UI design system (2026-09-24)

Two new source-derived generators were added and gated in CI:

- `scripts/ci/generate-plan-integration-audit.py` →
  `docs/plans/RECENT_PLAN_INTEGRATIONS_AUDIT.md` + `recent_plan_integration_audit.json`.
  Programmatically re-verifies 44 recently integrated plans against live source
  (Core type declarations, `src/` reachability, `SaveSectionRegistry` triad,
  save stores, UI panels, routes, `HostCli` + `SELFTEST_MANIFEST.json`, test
  fixtures). Verdict: **44/44 INTEGRATED**, including both plans in this log.
- `scripts/ci/generate-ui-design-map.py` →
  `docs/ui/UI_DESIGN_MAP.md` + `ui_design_map.json`. Derives the UI design
  system from source: Theme tokens (23 colors, 5-step spacing, 6-step type
  scale), 241 panels (110 dashboard shells, 114 bindable, 22 scene-backed),
  dashboard IA, 135 PanelRegistry bindings, 61 expanded-surface ids, and four
  measured design invariants.

Findings surfaced by the design map (reported, not repaired here):

1. **Unbind lifecycle asymmetry (low severity):** 8 bindable panels
   (`DutyRosterDetailPanel`, `EpiloguePanel`, `EventsLogPanel`,
   `FactionDetailPanel`, `MaritimeAtlasPanel`, `OnboardingHintPanel`,
   `OpeningProtocolModal`, `WeatherSondePanel`) expose `Bind` without `Unbind`.
   Spot-check shows all are snapshot-store-and-refresh binds; the only panel
   with a live event subscription (`WeatherSondePanel`) unsubscribes before
   rebinding and detaches at teardown, so no active leak was found — this is a
   naming/lifecycle-consistency gap for a future sweep, not a runtime defect.
2. **Concurrency drift guard value:** during the audit, the
   `visitor_integration` graph entry was found concurrently normalized to
   inaccurate values by another in-flight agent and was corrected with an
   assert-guarded patch (see earlier section); the new drift gates make such
   drift fail CI instead of passing silently.

CI wiring: `plan_integration_audit_drift` and `ui_design_map_drift` added to
`docs/ci/CI_GATE_MANIFEST.json` (schema 1.1.1, 59 gates / 55 fast), both
verified through `scripts/ci/run-gates.py --gate` (16.7s / 0.09s);
`docs-regen.yml` regenerates both docs; `docs/INDEX.md` regenerated (5041
documents); `CiGateManifestDriftTests` 7/7 passing.
