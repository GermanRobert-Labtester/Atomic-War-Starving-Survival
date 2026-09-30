# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**

# UI A11Y P2.10 INTERACTIVE TARGET SIZES ≥28PX — 2026-09-29

STATUS: FULLY INTEGRATED
Authorized: user session request "Continue doing more work!" (2026-09-29).
Fifth package in the audit-fix series: audit §5d/§9.10.

> **Editorial polish (prose pass, non-contractual):** the Framing section below is commentary on
> intent and craft only. It changes no scope, no ownership, no literal and no acceptance criterion.
> **MUST NOT** below remains binding.

---

## 0. Framing — The Extra Pixel

> *"27 pixels is a button. 28 pixels is a button you can hit."*

One pixel, shared default, 23 hand-rolled sites swept — and the reason it matters has nothing to do
with aesthetics. A 27px target is reachable by a steady hand on a mouse. It is not reliably
reachable by a hand that is tired, shaking, gloved, cold, older, on a controller D-pad, or
operating a stick through a thumb that does not stop moving.

The fix is not to resize 23 panels. It is to add **one Core token** —
`Theme.MinInteractiveHeight = 28` — and point the shared `MakeButton` default at it, so the
majority of buttons in the tree inherit the threshold without a single per-site edit. The sweep
then handles only the controls that rolled their own.

**Tone & register.** Ergonomic, plain, quietly political. The vocabulary is the target: *minimum,
threshold, height literal, hand-rolled, interactive, inherit*. Prose should read like an occupational
therapist who has been given access to the source.

**The interesting distinction.** Non-interactive minimum sizes are **explicitly excluded** —
progress bars, meter fills, `ColorRect` dots, swatches, and labels such as
`BioFermentationPanel:276 "Process:"`. A label is not a target. Making everything bigger is not
accessibility; making the *right things* bigger is.

**The second layer.** One pixel is the distance between a hand that works and a hand that does not.
The plan's politics are in its vocabulary: 27px was specified by someone imagining a *steady hand*,
and 28px is specified by everyone else — tired, gloved, cold, older, thumbing a stick that will not
hold still. And the excluded list is what keeps the change honest: labels are not targets, and an
interface where everything is big is not an interface where the *right things* are reachable.

**Texture (second prose pass — commentary only).**

- "27 pixels is a button. 28 pixels is a button you can hit." The plan's thesis and its entire argument in fourteen words.
- One token, one shared default, and the majority of the tree inherits the threshold — the sweep only touches what rolled its own.
- "A label is not a target." The sentence that separates accessibility from bulk.

*Register below unchanged — these fragments are texture, not new recorded items.*

**The polish layer (third prose pass — commentary only).**

*(Non-contractual: craft commentary only. No scope, ownership, token, acceptance criterion or
register row changes. The register below is unchanged.)*

- The extra pixel is invisible in a screenshot and decisive under a thumb. Target size is the one
  accessibility measure that lives on the body rather than the eye.
- Font-coupled sizing is a quiet cruelty: the button shrinks precisely when the player is asking
  to see more clearly. The floor breaks that coupling on purpose.
- A 28px promise is a physical fact in a digital room — it says *a hand may land here*, and every
  promise made to fingers should be kept in the same unit it was made in.

> "A target is not a shape. It is an invitation, and invitations have sizes."

---

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

## Open Items & Deliberate Limits (register — not acceptance criteria)

Drawn from this plan's **MUST NOT** list and its exclusions. Not defects — deliberate non-changes.

| # | Open item | Why it is deliberately open | Who may resolve it (later, separately verified) |
|---|---|---|---|
| UIT-OM-1 | **Why 28 and not 32 or 44?** | 28 is the threshold chosen and the single authority for it is one token. Larger platform standards are not cited and are not claimed. | A platform-conformance pass, if store targets ever require one. |
| UIT-OM-2 | What about width? | Widths preserved except two tiny square buttons (24×22 → 28×28, 24×24 → 28×28). Height is the fixed constraint; horizontal density is a layout decision. | The layout lane. |
| UIT-OM-3 | Are the 23 enumerated files the whole surface? | They are the **audit-enumerated** sites. A rolled-own control outside that list would not be swept. | A source scan for `CustomMinimumSize` on interactive types. |
| UIT-OM-4 | Why were non-interactive sizes excluded? | A label is not a target. Progress bars, meters, dots, swatches and labels are explicitly out. Making everything bigger is not accessibility. | Never — a rule, not a gap. |
| UIT-OM-5 | Did the two square buttons get harder to click? | Both grew 24→28 in **both** dimensions. Density loss in those two panels was accepted without measurement. | A density check on ShelterBarter/Feedback if anyone objects. |
| UIT-OM-6 | Does 28px help a controller? | The gate covers Button/OptionButton minimums in the player UI. D-pad reachability is a nav matter (see P3) and is not re-tested here. | A controller runtime probe. |
