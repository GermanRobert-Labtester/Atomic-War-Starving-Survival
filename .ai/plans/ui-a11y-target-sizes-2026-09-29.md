# UI A11Y P2.10 INTERACTIVE TARGET SIZES ≥28PX — 2026-09-29

STATUS: APPROVED BY USER
Authorized: user session request "Continue doing more work!" (2026-09-29).
Fifth package in the audit-fix series: audit §5d/§9.10.

## Bounded outcome

All interactive controls (Button, OptionButton) in the player UI get a
minimum height of 28px; the shared default rises 27→28:

1. **Core token:** `Assets/Ashfall.Core/UI/Theme.cs` adds
   `public const int MinInteractiveHeight = 28;` (panel-sizing section) —
   the single authority for the threshold.
2. **Helper default:** `src/UI/AshfallUiHelpers.cs` `MakeButton` changes
   `CustomMinimumSize = new Vector2(0, Theme.FontSizeBody + Theme.SpacingMd)`
   (27px, and font-coupled) to `new Vector2(0, Theme.MinInteractiveHeight)`.
   Every MakeButton-built control in the tree (the majority of buttons)
   inherits this.
3. **Site sweep:** the 23 audit-enumerated files with hand-rolled sub-28px
   Button/OptionButton minimum sizes get their height literal raised to 28
   (widths preserved except ShelterBarter 24×22 → 28×28 and FeedbackPanel
   24×24 → 28×28, both tiny square buttons). Non-interactive minimum sizes
   (progress bars, meter fills, ColorRect dots, swatches, labels — e.g.
   BioFermentationPanel:276 "Process:" label) are explicitly excluded.

## Exact files

- `Assets/Ashfall.Core/UI/Theme.cs` (1 token), `src/UI/AshfallUiHelpers.cs`
  (1 line)
- Sweep (height literal only): ShelterBarterPanel (4× 24×22),
  FeedbackPanel (24×24), InventoryPanel (64×24, 82×24),
  PowerGridPanel (9 sites: 24/26 heights), RadioPanel (2× 26),
  DefenseGridPanel (2× 26), RoboticsWorkshopPanel (26), FungiCultivationBedPanel
  (2× 26), BioFermentationPanel (2× 26), JusticeTribunalPanel (26),
  ArchiveDeskPanel, ChemicalDependencyPanel, DecontaminationPanel,
  ContractorRosterPanel (2), MedicalWardPanel, LibraryStudyPanel,
  KitchenNutritionPanel, PhantomMemoryPanel, EquipmentConditionPanel,
  MentalHealthCrisisPanel, GreenhousePanel, SumpFloodingPanel,
  TravelingCaravanPanel (all single 24s)
- `Ashfall.Core.Tests/UI/UiA11yTargetSizeGateTests.cs` — new static gate
- Governance: this plan, `WORKTREE_OWNERSHIP.md`, `.ai/state.md`

## MUST NOT

- No non-interactive minimum-size changes; no width changes beyond the two
  square buttons; no Core changes beyond the one token; no reformatting.

## Verification

1. `dotnet build Ashfall.csproj` — 0 errors.
2. New gate tests + `scripts/run_test.sh` on the gate file.
3. `--ui-layout-selftest` (headless) + `--player-panels-uitest` runtime probe.
