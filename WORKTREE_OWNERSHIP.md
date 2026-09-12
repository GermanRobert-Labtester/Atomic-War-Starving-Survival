# ASHFALL Worktree Ownership

The foreman is the sole writer of this ledger. Builders and reviewers must read
it before editing. A path claim prevents accidental agent races; it is not a
claim of permanent subsystem ownership.

## Active claims

| Claim | Package | Owner | Exact paths | Status |
|---|---|---|---|---|
| claim-plans-138-phase1-2026-09-12 | `PLANS-138-METROLOGY-PHASE1` | Builder (this session) | `Assets/StreamingAssets/Data/low_background_lead_catalog.json`; `Assets/Ashfall.Core/Radiation/LowBackgroundLeadEngine.cs`; `Ashfall.Core.Tests/Radiation/Plan138LowBackgroundLeadEngineTests.cs` | HANDED_OFF |
| claim-plans-138-phase2-2026-09-12 | `PLANS-138-METROLOGY-PHASE2` | Builder (this session) | `src/Host/LowBackgroundMetrologyHostSession.cs`; `src/Host/LowBackgroundMetrologySaveStore.cs`; `src/Main.LowBackgroundMetrology.cs`; `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`; `src/Main.SaveOrchestrator.cs`; `Assets/Ashfall.Core/Random/CampaignRngStream.cs`; `Ashfall.Core.Tests/Radiation/Plan138LowBackgroundHostWiringTests.cs` | HANDED_OFF |
| claim-plans-138-phase3-2026-09-12 | `PLANS-138-METROLOGY-PHASE3` | Builder (this session) | `src/UI/LowBackgroundLeadPanel.cs`; `src/Main.LowBackgroundMetrology.cs`; `src/Main.ExpandedShelterSystems.cs` (one case); `src/Main.GameFlow.cs` (one case); `docs/architecture/ARCHITECTURE_TEST_MAP.md` (Plan 138 row) | HANDED_OFF |

## Recently closed claims

| Claim | Package | Owner | Exact paths | Closed | Status |
|---|---|---|---|---|---|
| claim-debt-170-199-remaining-maps-2026-09-12 | `DEBT-170-199-REMAINING-FAMILY-MAPS` | Integrator (this session) | Remaining PARTIAL family authority maps under `docs/`; `docs/INDEX.md`; governance ledgers | 2026-09-12 | HANDED_OFF |
| claim-debt-189-water-source-map-2026-09-12 | `DEBT-189-WATER-SOURCE-AUTHORITY-MAP` | Integrator (this session) | `docs/water/PLAN_189_WATER_SOURCE_AUTHORITY_MAP.md`; `docs/INDEX.md` (generator); governance ledgers | 2026-09-12 | HANDED_OFF |
| claim-debt-test-quarantine-holdfast-npc-2026-09-12 | `DEBT-TEST-QUARANTINE-HOLDFAST-NPC` | Integrator (this session) | `Ashfall.Core.Tests/Narrative/HoldfastNpcCatalogTests.cs`; `Ashfall.Core.Tests.csproj` (one Compile Remove); governance ledgers | 2026-09-12 | HANDED_OFF |
| claim-debt-test-quarantine-archiveinks-2026-09-12 | `DEBT-TEST-QUARANTINE-ARCHIVEINKS` | Integrator (this session) | `Ashfall.Core.Tests/ArchiveInksCatalogTests.cs`; `Ashfall.Core.Tests.csproj` (one Compile Remove); `Assets/Ashfall.Core/ArchiveDeskSystem.cs` (InkMaterialDefinition JsonPropertyName); governance ledgers | 2026-09-12 | HANDED_OFF |
| claim-debt-test-quarantine-utilityai-2026-09-12 | `DEBT-TEST-QUARANTINE-UTILITYAI` | Integrator (this session) | `Ashfall.Core.Tests/UtilityAiExpandedCatalogTests.cs`; `Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` (one Compile Remove); governance ledgers | 2026-09-12 | HANDED_OFF |
| claim-debt-184-a11y-colorblind-2026-09-12 | `DEBT-184-A11Y-COLORBLIND` | Integrator (this session) | UserSettingsData/Codec colorblind_mode; ColorblindColorMapper; AshfallUiHelpers.ToColor; AccessibilityPresentation; SettingsPanel; settings selftest; recovery tests; authority map; governance ledgers | 2026-09-12 | HANDED_OFF |
| claim-debt-184-a11y-bridge-2026-09-12 | `DEBT-184-A11Y-PREFERENCE-BRIDGE` | Integrator (this session) | UserSettingsStore/AccessibilityPresentation/AudioManager; Main refresh; settings selftest; authority map note; governance ledgers | 2026-09-12 | HANDED_OFF |
| claim-debt-184-a11y-map-2026-09-12 | `DEBT-184-EXPANDED-A11Y-AUTHORITY-MAP` | Integrator (this session) | `docs/ui/PLAN_184_EXPANDED_ACCESSIBILITY_AUTHORITY_MAP.md`; governance ledgers | 2026-09-12 | HANDED_OFF |
| claim-debt-173-radio-panel-2026-09-12 | `DEBT-173-RADIO-PROGRAM-PANEL` | Integrator (this session) | RadioProgram catalog/system equipment+cost; radio_programs.json; Main.RadioProgramProduction inventory wire; RadioPanel strip + bind sites; Plan173 tests; adapter map §11; ARCHITECTURE_TEST_MAP; governance ledgers | 2026-09-12 | HANDED_OFF |
| claim-arch-test-map-gate-2026-09-12 | `DEBT-ARCH-TEST-MAP-GATE` | Integrator (this session) | `docs/architecture/ARCHITECTURE_TEST_MAP.md`; governance ledgers | 2026-09-12 | HANDED_OFF |
| claim-debt-173-radio-host-2026-09-12 | `DEBT-173-RADIO-PROGRAM-HOST` | Integrator (this session) | Host session+save store+Main.RadioProgramProduction; SaveSectionRegistry; SaveOrchestrator; CampaignOwners day owner; ContentUtilizationScanner claim; ARCHITECTURE_TEST_MAP; adapter map §9; governance ledgers | 2026-09-12 | HANDED_OFF |
| claim-debt-173-radio-production-2026-09-12 | `DEBT-173-RADIO-PROGRAM-PRODUCTION` | Integrator (this session) | RadioProgramProduction Core+catalog+JSON; Plan173 tests; authority map; governance ledgers | 2026-09-12 | HANDED_OFF |
| claim-debt-173-radio-adapter-2026-09-12 | `DEBT-173-RADIO-PROGRAM-ADAPTER-MAP` | Integrator (this session) | `docs/radio/PLAN_173_RADIO_PROGRAM_ADAPTER_MAP.md`; governance ledgers | 2026-09-12 | HANDED_OFF |
| claim-debt-196-food-seam-2026-09-12 | `DEBT-196-FOOD-TYPE-TEMP-SEAM` | Integrator (this session) | FoodPreservation Core+catalog+JSON; Plans62_65 host temp projection; Plan196 tests; integrity non-ref key; authority map; governance ledgers | 2026-09-12 | HANDED_OFF |
| claim-debt-196-food-boundary-2026-09-12 | `DEBT-196-FOOD-SPOILAGE-BOUNDARY` | Integrator (this session) | `docs/shelter/PLAN_196_FOOD_SPOILAGE_AUTHORITY_MAP.md`; governance ledgers | 2026-09-12 | HANDED_OFF |
| claim-debt-166-169-reload-2026-09-12 | `DEBT-166-169-RELOAD-REPLAY` | Integrator (this session) | `Ashfall.Core.Tests/Plan166_169ReloadReplayTests.cs`; governance ledgers | 2026-09-12 | HANDED_OFF |
| claim-debt-167-168-2026-09-12 | `DEBT-167-CONSEQUENCE-ROUTING` + `DEBT-168-WATER-DELIVERY` | Integrator (this session) | Plan 167 router/consumers/host; Plan 168 topology/delivery applicator/host; focused tests; authority maps; governance ledgers | 2026-09-12 | HANDED_OFF |
| claim-plans-146-149-med-2026-09-12 | `PLANS-146-149-MED-SEAL` | Builder (this session) | Travel stretch Core+host; coated install→PowerGrid; assay patient picker; focused tests; seal log | 2026-09-12 | HANDED_OFF |
| claim-plans-150-153-2026-09-12 | `PLANS-150-153-NARRATIVE-ACTIVATION` | Builder (this session) | Narrative discovery contracts/host Discover* wiring; BP producer destination bridge + arrival/map-detail; fringe de-theater; focused activation tests; seal log | 2026-09-12 | HANDED_OFF |
| claim-partial-rebase-2026-09-12 | `PARTIAL-REBASE-01` | Foreman | Partial rebase report, forensic audit, governance ledgers, and generated docs index | 2026-09-12 | ACCEPTED |
| claim-forensic-170-199-2026-09-12 | `FORENSIC-170-199` | Foreman | Plan audit, governance ledgers, and generated docs index | 2026-09-12 | ACCEPTED |
| claim-hardening-167-2026-09-12 | `HARDEN-167-HOST-ACTION` | Foreman | `src/Host/EspionageHostSession.cs`; `Ashfall.Core.Tests/Plan167EspionageTests.cs`; governance ledger/debt files | 2026-09-12 | ACCEPTED |
| claim-declutter-build-cache-2026-09-12 | `DECLUTTER-IGNORED-BUILD-CACHE` | Foreman | Four ignored Core/test `bin/obj` directories; dated Twin archive and manifest | 2026-09-12 | ACCEPTED |
| claim-foreman-2026-09-12 | FOREMAN-00..03 | Foreman | Agent rulebooks, governance documents, current authority map, sync script, and rulebook archive | 2026-09-12 | ACCEPTED |

## Unregistered worktree safety

The repository already contains substantial user and other-agent changes that
predate this batch. They are not available for opportunistic cleanup, rebasing,
formatting, or conflict resolution. A package may touch only its listed paths.

## Claim lifecycle

- `ACTIVE`: owner may edit exact paths.
- `HANDED_OFF`: owner stopped; integrator or named successor may proceed.
- `BLOCKED`: no edits until foreman records a new decision.
- `ACCEPTED` / `REVOKED`: claim is closed and paths become available.

Claims name exact files or narrow directories. Shared composition roots,
registries, and generated authority files belong to the integrator, never to
parallel builders.
