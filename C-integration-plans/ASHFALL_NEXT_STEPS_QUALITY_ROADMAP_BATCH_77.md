# ASHFALL — Quality Roadmap Batch 77

## Theme: UI Component Library Formalization — Design System Documentation & Widget Catalog

**Priority:** MEDIUM
**Risk:** None — documentation and standardization only
**Estimated Effort:** 5–7 sessions
**Prerequisites:** None (all widgets already exist and are in use)

---

### Motivation

The project has roughly 85–95 programmatically-built UI panels (exact count depends on whether
overlays/modals are included; see Step 1) sharing five custom widgets in `src/UI/`
(`AshfallDashboardShell`, `AshfallDataGrid`, `AshfallMetricCard`, `AshfallSidebar`,
`AshfallStatusRail`) and a theme palette (`Ashfall.Core.UI.Theme`, in
`Assets/Ashfall.Core/UI/Theme.cs`). **Note:** only `AshfallDashboardShell` and `AshfallDataGrid`
are directly `new`'d by panels. `AshfallSidebar`, `AshfallStatusRail`, and `AshfallMetricCard`
are never constructed directly by panel code — panels obtain them via
`AshfallDashboardShell.SetSidebar()` / `.SetStatusRail()` and `AshfallStatusRail.AddCard()`,
which construct them internally. Treat these three as internal implementation details of the
shell/rail, not independently-adopted widgets, when auditing "usage."

Fonts are `BarlowCondensed` (4 weight variants: Regular/Medium/SemiBold/Bold, plus
MediumItalic) and `ShareTechMono` (Regular only), loaded lazily per-widget via
`src/UI/AshfallUiHelpers.cs` from `res://assets/fonts/*.ttf` — there is no project-wide default
theme font override in `project.godot`; each widget loads fonts itself.

**Important — this design system partially already exists.** `docs/ui/DESIGN_SYSTEM_RULES.md`
already documents a token mapping table (12 of the 18 real `Theme.cs` colors), a spacing rhythm,
a typography scale, and sizing constraints for at least one panel family (trade/radio). Steps
2–4 of this batch must reconcile with — not duplicate or contradict — that existing document.
Critically, `DESIGN_SYSTEM_RULES.md` documents a **4px base grid** (`Xs=4, Sm=8, Md=12, Lg=16,
Xl=24`), matching the constants already defined in `Theme.cs` (`SpacingXs=4` .. `SpacingXl=24`).
Step 2 below proposes an **8px grid** with different values and different names
(`SpaceSm=8, SpaceMd=16, SpaceLg=24...`). Shipping both would create two incompatible spacing
systems. See Step 2 for the required resolution.

Despite shared primitives, each panel author independently invents spacing, margins, font sizes,
and layout patterns beyond what `Theme.cs` already defines. This leads to visual inconsistency,
slower development, and fragile maintenance. A formalized design system — tokens, guidelines,
catalog — eliminates this drift without changing any runtime behavior, provided it extends
`Theme.cs`/`DESIGN_SYSTEM_RULES.md` rather than forking a second, conflicting token set.

---

## Step 1 — Audit All Custom Widgets

**Goal:** Produce a complete inventory of every reusable UI widget, its public API, parameters,
visual variants, and current usage count across the panel corpus.

**Implementation:**
- Grep `src/UI/` for all classes inheriting Godot UI base types (`Control`, `Panel`, `Container`,
  `MarginContainer`, `VBoxContainer`, etc.). As a starting point, `src/UI/` contains 97 `.cs`
  files; roughly 89–94 declare a panel/screen class (naming and counting method — filename glob
  vs. class-name suffix vs. base-type grep — all give slightly different totals, so the
  inventory must state which method it used and show its raw count, not just assert "~85").
- Distinguish **directly-instantiated** widgets (`AshfallDashboardShell`, `AshfallDataGrid` —
  panels call `new` on these) from **internally-constructed** widgets (`AshfallSidebar`,
  `AshfallStatusRail`, `AshfallMetricCard` — panels never call `new` on these; they call
  `AshfallDashboardShell.SetSidebar()`/`.SetStatusRail()` or `AshfallStatusRail.AddCard()`,
  which construct them internally). Report usage counts for both categories separately —
  "usage count" is meaningless for the internally-constructed group unless you count call
  sites of the factory method instead of `new` call sites.
- For each widget, document:
  - Class name and file path (all five widgets live in `src/UI/`, namespace
    `AtomicWar.GodotApp.UI`).
  - Constructor parameters / configuration properties.
  - Visual variants (e.g., `AshfallMetricCard.Criticality` enum: Normal/Caution/Warn/Critical).
  - Panels that instantiate it directly, or call the factory method that instantiates it.
- Record undocumented behaviors (implicit defaults, magic numbers for sizing).

**Verification:**
- The resulting inventory matches the actual widget count found by grep, and the doc states
  which counting method (filename / class-name-suffix / base-type) produced the number.
- Every widget in the inventory has at least: name, file, parameters, variant list, usage count,
  and a note on whether usage is direct (`new`) or indirect (factory method).

**Done when:** A `docs/ui/WIDGET_INVENTORY.md` file exists listing every shared widget with full
API documentation and usage statistics, cross-checked against a documented, reproducible count
command (e.g. the exact `grep`/`find` invocation used), not an eyeballed estimate.

---

## Step 2 — Reconcile Spacing & Sizing System with Existing 4px Grid

**Goal:** Formally document the spatial rhythm system so all panels share consistent margins,
padding, gaps, and element sizing — **by extending the grid that already exists**, not by
introducing a second, conflicting one.

**Risk — corrected from original draft:** The original version of this step proposed a new 8px
grid (`SpaceSm=8, SpaceMd=16, SpaceLg=24, SpaceXl=32, SpaceXxl=48`). This directly conflicts
with the grid already live in code: `Assets/Ashfall.Core/UI/Theme.cs` defines
`SpacingXs=4, SpacingSm=8, SpacingMd=12, SpacingLg=16, SpacingXl=24`, and
`docs/ui/DESIGN_SYSTEM_RULES.md` §1 already states "Strict 4px base grid." Shipping an 8px
scale alongside the existing 4px scale would give the codebase two incompatible spacing
vocabularies with overlapping names at different pixel values (e.g. a future `SpaceMd` at 16px
vs. the existing `Theme.SpacingMd` at 12px) — worse than the drift this batch is meant to fix.

**Implementation:**
- Treat `Theme.cs`'s existing constants as the base unit and canonical scale: `1u = 4px`.
  `SpacingXs=4 (1u)`, `SpacingSm=8 (2u)`, `SpacingMd=12 (3u)`, `SpacingLg=16 (4u)`,
  `SpacingXl=24 (6u)`.
- Do not introduce new spacing constant names. Document the existing five values, their
  current usage sites, and identify any *gaps* in the scale that real panels are working around
  with raw literals (e.g. is there a widespread need for a 32px or 48px step not yet covered?).
  Only propose new tokens for gaps that are demonstrated by actual panel code, not speculative
  values.
- Document existing element heights actually used in `src/UI/` widgets (grep for
  `MinHeight`/`CustomMinimumSize` literals in `AshfallDashboardShell.cs`, `AshfallDataGrid.cs`,
  `AshfallStatusRail.cs`, `AshfallMetricCard.cs`, `AshfallSidebar.cs` and report the real
  values found — do not assert `HeightButton=40`/`HeightRow=48`/`HeightHeader=56` without
  confirming them against the widget source, since these were not verified against code in the
  original draft).
- Document the grid system with annotated diagrams showing common layouts.

**Verification:**
- All documented tokens are the ones that already exist in `Theme.cs` — no new constant names
  are introduced without a demonstrated real-code gap.
- At least 3 existing panels are retrospectively annotated showing how they already map (or
  fail to map) to `Theme.cs`'s existing tokens.

**Done when:** `docs/ui/SPACING_SYSTEM.md` exists with the complete token table (matching
`Theme.cs` exactly), usage rules, and annotated layout examples. This file supplements —
and cross-links to — the existing `docs/ui/DESIGN_SYSTEM_RULES.md` §1/§3 rather than
duplicating or contradicting it.

**Rollback:** Documentation-only; delete the new file if superseded. No risk to runtime code.

---

## Step 3 — Create Color Usage Guidelines

**Goal:** Document when and why each theme color is used, preventing arbitrary color choices and
ensuring semantic consistency across all panels.

**Correction — the color list in the original draft was incomplete.** `Assets/Ashfall.Core/UI/Theme.cs`
defines **18 color fields**, not 7. The original draft only covered `Ink, Warm, Pale, Hot, Muted,
Dim, InkPanel`. It omitted: `Line`, `LineSoft`, `Exclusive`, `Critical`, `Entropy`, `Lethe`,
`Ozone`, `Ghost`, `EntropyGlow`, `LetheAmber`, `LetheRed`. Several of these already have
documented semantic roles in `docs/ui/DESIGN_SYSTEM_RULES.md` §2 (e.g. `Critical` = "critical
warning / short offer status", `Entropy` = "rob stance / structural wear", `Lethe` = "memory
stratum / sight-gauge"). This step must cover all 18, and must reconcile with — not
re-litigate — the 12 colors `DESIGN_SYSTEM_RULES.md` already assigns roles to.

**Implementation:**
- Map each of the 18 `Ashfall.Core.UI.Theme` colors to its semantic role, starting from
  `DESIGN_SYSTEM_RULES.md` §2's existing table for the 12 it already covers, and adding the
  6 undocumented ones (`Line`, `LineSoft`, `Ghost`, `EntropyGlow`, `LetheAmber`, `LetheRed`):
  - `Ink` — primary text, high-contrast foreground / near-black background (confirm which —
    `Theme.cs`'s own doc comment says "near-black background," not "primary text"; the
    original draft's claim that `Ink` is foreground text contradicts the source comment and
    must be corrected against actual usage in `AshfallDashboardShell`/other widgets).
  - `Warm` — interactive elements (buttons, links, selected states), primary accent.
  - `Pale` — primary readable body text (per `DESIGN_SYSTEM_RULES.md` §2), not merely
    "secondary text" as the original draft stated.
  - `Hot` — alerts, critical warnings, radiation danger, health-critical values, highlight/emphasis.
  - `Muted` — disabled states, tertiary information, secondary labels & neutral stance.
  - `Dim` — subtle backgrounds, separator lines, inactive tabs, disabled controls.
  - `InkPanel` — panel/card/modal background surfaces.
  - `Line` / `LineSoft` — default/soft borders and dividers.
  - `Critical` — hard danger states (distinct from `Hot`; clarify the boundary between the two
    since both currently read as "warning" colors — this ambiguity must be resolved, not
    inherited into the new doc unexamined).
  - `Exclusive`, `Entropy`, `Lethe`, `Ozone`, `Ghost`, `EntropyGlow`, `LetheAmber`, `LetheRed` —
    expansion-specific tokens (per `Theme.cs`'s own "Expansion IV tokens" section comment);
    document them as such rather than as general-purpose palette colors.
- Define usage rules, reconciled with `DESIGN_SYSTEM_RULES.md` (do not restate conflicting rules):
  - Never use `Hot` or `Critical` for decoration — reserve for danger/urgency only.
  - `Warm` is the primary accent for interactive affordances.
  - Disabled elements use `Muted` foreground on `Dim` background.
- Define contrast requirements (WCAG AA: 4.5:1 for normal text, 3:1 for large text). Compute
  actual contrast ratios for the real hex values in `Theme.cs` (e.g. `Pale` #E6E0D2 on
  `InkPanel` rgba(9,11,12,0.86)) rather than asserting compliance without the arithmetic.
- Provide do/don't examples for each of the 18 colors.

**Verification:**
- Every one of the 18 colors in `Theme.cs` has exactly one documented semantic role, and any
  color already documented in `DESIGN_SYSTEM_RULES.md` matches that document's wording or
  explicitly notes a correction with rationale.
- Contrast ratios between foreground/background pairings are computed from the real hex values
  and meet WCAG AA, or are flagged as a follow-up issue if they don't.
- At least 5 do/don't examples are provided across the full 18-color set.

**Done when:** `docs/ui/COLOR_GUIDELINES.md` exists with semantic mapping for all 18 colors,
a computed contrast table, and annotated examples, cross-linked with
`docs/ui/DESIGN_SYSTEM_RULES.md` rather than duplicating it.

**Rollback:** Documentation-only; no runtime risk.

---

## Step 4 — Reconcile Typography Scale with Existing Font Size Constants

**Goal:** Formalize the fixed set of text styles (size, weight, font, line-height) that panels
should use, **by documenting and closing gaps in the existing scale**, not by inventing a
parallel one with different pixel values.

**Risk — corrected from original draft:** The original proposed scale (`Display=32, H1=24,
H2=20, H3=16, Body=14, Caption=12, DataLarge=20, DataMedium=16, DataSmall=12, Button=14`) does
not match the font sizes already defined as constants in `Theme.cs`
(`FontSizeH1=28, FontSizeH2=22, FontSizeH3=18, FontSizeBody=14, FontSizeSmall=11,
FontSizeMono=12, FontSizeLabel=10`) or the scale already published in
`docs/ui/DESIGN_SYSTEM_RULES.md` §1 (`Label=10, Small=11, Mono=12, Body=14, H3=18, H2=22,
H1=28`). Only `Body=14` and the `H3`/`Mono`-family sizes are consistent with existing constants
by coincidence; `H1`/`H2` and the proposed `Display` size have no equivalent in code today.
Publishing the original draft's scale as-is would create a second, incompatible typography
system alongside the one already partially enforced by `Theme.cs` constants.

**Implementation:**
- Start from the real, already-defined sizes: `FontSizeLabel=10, FontSizeSmall=11,
  FontSizeMono=12, FontSizeBody=14, FontSizeH3=18, FontSizeH2=22, FontSizeH1=28`, plus the
  diegetic HUD-specific sizes (`DiegeticTitleSize=12, DiegeticStatusSize=11,
  DiegeticBodySize=11, DiegeticHintSize=10`).
- Confirm font-family pairing per size by reading actual widget code (`AshfallUiHelpers.cs`
  font loaders + call sites in the five widgets), not by assumption. Document only weights
  that are actually loaded: `BarlowCondensed-Regular/Medium/SemiBold/Bold/MediumItalic` and
  `ShareTechMono-Regular` (no bold/italic mono variant is loaded — do not document one).
  the doc must state that the underlying `.ttf` binaries live under `assets/fonts/` and are
  the same files used by both widget code and the Godot editor's imported font cache.
- Only propose a new size (e.g. a "Display" size larger than `FontSizeH1=28`) if a real panel
  currently uses a raw literal above 28px that isn't covered — verify this against actual
  `SetFontSize`/`AddThemeFontSizeOverride` call sites in `src/UI/` before adding it.
- Define line-height multipliers actually used by Godot's `Label`/`RichTextLabel` in this
  codebase (if none are explicitly set today, say so, and propose one instead of asserting a
  value as if it already existed).
- Document when to use each style.

**Verification:**
- Every text style in the scale matches a size already defined in `Theme.cs` or
  `DESIGN_SYSTEM_RULES.md`, or is justified by a cited real call site that needs a new size.
- No two styles overlap in purpose.

**Done when:** `docs/ui/TYPOGRAPHY_SCALE.md` exists with the complete scale reconciled against
`Theme.cs`'s real constants, usage rules, and font pairing guidance based on the fonts actually
loaded by `AshfallUiHelpers.cs`.

**Rollback:** Documentation-only; no runtime risk.

---

## Step 5 — Create Widget Catalog with Code Snippets

**Goal:** Produce a developer-facing catalog document showing every widget with visual description,
configuration options, code snippets, and composition patterns.

**Implementation:**
- For each widget from Step 1, create a catalog entry with:
  - **Purpose:** one-sentence description.
  - **Visual description:** annotated breakdown of the widget's structure.
  - **Parameters:** table of configurable properties with types, defaults, and constraints.
  - **Variants:** list of visual/behavioral variants with when to use each (e.g.
    `AshfallMetricCard.Criticality`: Normal/Caution/Warn/Critical).
  - **Code snippet:** minimal C# construction example (programmatic, no `.tscn`), and note
    whether the widget is directly constructible (`AshfallDashboardShell`, `AshfallDataGrid`)
    or must be obtained via a factory method (`AshfallSidebar`/`AshfallStatusRail`/
    `AshfallMetricCard` — see Step 1 correction).
  - **Composition:** how widgets nest in practice. Correct composition example, matching the
    real API: `AshfallMetricCard` instances are not manually nested inside
    `AshfallDashboardShell`'s sidebar — they are added to the status rail via
    `AshfallStatusRail.AddCard(...)`, and the sidebar itself is populated with
    `AshfallSidebar.Item[]` entries via `AshfallDashboardShell.SetSidebar(...)`, not with
    `AshfallMetricCard` instances directly.
  - **Do/Don't:** common misuse patterns and corrections.
- Include a "Choosing the Right Widget" decision flowchart.
- Include a "Standard Panel Skeleton" showing the canonical panel structure, corrected to match
  the real composition relationship:
  ```
  AshfallDashboardShell (constructed directly by panel)
  ├── AshfallSidebar (obtained via shell.SetSidebar(items, headerLabel, initialSelectedId))
  │   └── AshfallSidebar.Item[] entries (navigation, not AshfallMetricCard)
  ├── main content area (obtained via shell.SetContent<T>(content))
  │   └── AshfallDataGrid (constructed directly) or custom content
  └── AshfallStatusRail (obtained via shell.SetStatusRail())
      └── AshfallMetricCard instances (added via statusRail.AddCard(key, label, value, criticality))
  ```

**Verification:**
- Every widget from the Step 1 inventory has a catalog entry.
- Every code snippet is extracted into (or checked against) a real, compiling example file —
  e.g. add snippets as `[Fact]`-free helper methods in a new
  `Ashfall.Core.Tests`-adjacent Godot-side sample file, or state explicitly that snippets are
  verified by manual compilation against the current widget API at doc-writing time, since
  `Ashfall.Core.Tests` (a `netstandard2.1`/`net9.0` project with `noEngineReferences: true`)
  **cannot** compile any code referencing `Godot.*`-derived widgets like `AshfallDashboardShell`
  — the original draft's "verifiable by inclusion in a test" claim is not achievable in the
  Core test project and must instead target a Godot-buildable location if automated.
- The decision flowchart covers all five widgets.

**Done when:** `docs/ui/WIDGET_CATALOG.md` exists with complete entries for all five widgets,
code snippets checked against the real constructor/method signatures listed in Step 1, and the
decision flowchart.

**Rollback:** Documentation-only; no runtime risk.

---

## Step 6 — Extend `Theme.cs` Rather Than Creating a Parallel Token Class

**Goal:** Encode any spacing/sizing/typography gaps identified in Steps 2–4 as compile-time
constants, without creating a second source of truth alongside the constants that already
live in `Theme.cs`.

**Risk — corrected from original draft:** The original draft proposed a brand-new
`Assets/Ashfall.Core/UI/AshfallLayoutTokens.cs` static class duplicating spacing
(`SpaceXxs/SpaceXs/SpaceSm/...`), element heights, and font sizes
(`FontDisplay/FontH1/FontH2/...`) that overlap in purpose — but not in value or name — with
constants **already defined and in use** in `Assets/Ashfall.Core/UI/Theme.cs`
(`SpacingXs/Sm/Md/Lg/Xl`, `FontSizeH1/H2/H3/Body/Small/Mono/Label`). Confirmed via file search:
`AshfallLayoutTokens.cs` does not exist yet, so this is net-new work, but introducing it
verbatim as originally drafted would fork the token system in the same file's own directory —
two static classes in `Assets/Ashfall.Core/UI/` defining overlapping concepts under different
names is worse than the undocumented-magic-numbers problem this batch sets out to fix.

**Implementation:**
- Do not create a new class if Steps 2–4 conclude (as expected) that `Theme.cs`'s existing
  constants already cover the spacing/typography scale. In that case, this step reduces to:
  add any XML doc comments to `Theme.cs`'s existing constants that are missing them, referencing
  the new `docs/ui/SPACING_SYSTEM.md` / `docs/ui/TYPOGRAPHY_SCALE.md` documents.
- Only add new constants to `Theme.cs` itself (not a new class) for gaps identified with
  evidence in Step 2 (e.g. if no border-radius or border-width constant exists yet and panels
  need one — `Theme.cs` already has `RadiusSm/Md/Lg`, so verify before assuming a gap).
- If a genuine architectural reason emerges to split tokens into a new file (e.g. `Theme.cs`
  becomes too large), that split must preserve exactly the existing constant names and values
  from `Theme.cs` — it is a move, not a redefinition.
- Ensure any change has no engine dependencies (`Theme.cs` already lives in Core,
  `netstandard2.1`, with zero `UnityEngine`/`Godot` references — preserve this).

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # Must compile cleanly
dotnet build Ashfall.csproj                                  # Godot host: 0 errors
```
- `Theme.cs` (or its successor) compiles in both Core and Godot host.
- No `UnityEngine.*` or `Godot.*` references.
- No duplicate constant exists under a different name for the same pixel value.

**Done when:** Spacing/typography/sizing tokens exist in exactly one place (`Theme.cs`, extended
as needed), compile cleanly, and match the documented scales from Steps 2–4 with no parallel
or conflicting class introduced.

**Rollback:** If new constants are added to `Theme.cs` and prove wrong, revert the specific
added lines — `Theme.cs` is small and the diff is additive, so this is low-risk and easily
reversible via `git revert` on the single commit for this step.

---

## Step 7 — Write UI Consistency Test

**Goal:** Create an automated test that scans all panel source files for raw pixel values
(magic numbers) that should be layout tokens, flagging violations for gradual migration.

**Correction:** References `AshfallLayoutTokens` in the original draft; per the Step 6
correction above, compare against `Ashfall.Core.UI.Theme`'s constants instead (no new class).

**Implementation:**
- Create `Ashfall.Core.Tests/UIConsistencyTests.cs`. **Caveat:** this test project targets
  `net9.0`/`netstandard2.1` with `noEngineReferences: true` and cannot reference `Godot.*`
  types — it can still work by treating `src/UI/*.cs` as plain text/source files (via
  `System.IO` + regex or a lightweight C# parser), not by compiling or instantiating the
  widgets. State this explicitly so the test isn't designed assuming it can `import` or
  construct `AshfallDashboardShell` etc.
  - Scan all `.cs` files in `src/UI/` as text.
  - Use regex to find patterns like:
    - `new Vector2(N, N)` where N is not a token constant.
    - `CustomMinimumSize = new Vector2(N, N)` with raw literals.
    - `AddThemeConstantOverride("margin_*", N)` with raw literals.
    - `SetFontSize(N)` / `AddThemeFontSizeOverride(..., N)` with raw literals not matching a
      `Theme.cs` constant value.
  - Compare found literals against `Theme.cs`'s actual constant values (read them at test time
    via reflection on `Ashfall.Core.UI.Theme`, or hardcode the known set with a comment noting
    it must be kept in sync — reflection is preferred to avoid drift).
  - Report violations with file, line, and suggested token replacement.
- The test starts as a warning/advisory (does not fail the build initially) — implement this
  as a `[Fact]` that writes a report to test output but does not `Assert.Empty(violations)` at
  first, since the existing ~85-95 panels almost certainly contain many pre-existing raw
  literals; a hard-fail assertion on day one would break the build for unrelated reasons and
  contradicts this batch's own "no runtime/build regressions" premise.
- Add a separate `[Fact]` that asserts zero violations in a curated "clean" panel subset (panels
  already migrated to tokens) to prevent regression once such a subset exists. If no panel has
  been migrated yet, this fact should be skipped or omitted until Step 6 or a follow-up batch
  produces at least one migrated panel — do not assert against an empty set, which would pass
  vacuously and prove nothing.

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # Must compile
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj    # All tests pass
```
- The test runs and reports findings. "No false positives" cannot be verified by the test
  itself asserting it has no false positives — it must be manually spot-checked against a
  sample of real matches before merging, and that spot-check should be noted in the PR/commit
  description, not silently assumed.
- At least one panel is migrated to tokens as proof the system works, or this sub-goal is
  deferred explicitly to a follow-up batch (see Notes).

**Done when:** `UIConsistencyTests.cs` exists, compiles, runs, and reports raw-literal findings
in `src/UI/` cross-referenced against `Theme.cs`'s real constants — without asserting a
zero-violation gate on the full corpus (that gate is future work once panels are migrated).

**Rollback:** New test file only; disable/delete if it proves too noisy. No production code
touched.

---

## Summary Table

| Step | Deliverable | Type | Risk | Depends On |
|------|-------------|------|------|------------|
| 1 | `docs/ui/WIDGET_INVENTORY.md` | Documentation | None | — |
| 2 | `docs/ui/SPACING_SYSTEM.md` (reconciled w/ `Theme.cs` 4px grid) | Documentation | None | — |
| 3 | `docs/ui/COLOR_GUIDELINES.md` (all 18 colors) | Documentation | None | — |
| 4 | `docs/ui/TYPOGRAPHY_SCALE.md` (reconciled w/ `Theme.cs` sizes) | Documentation | None | — |
| 5 | `docs/ui/WIDGET_CATALOG.md` | Documentation | None | Steps 1–4 |
| 6 | `Theme.cs` extension (no new class, unless a gap is proven) | Code (Core) | Low — additive const changes only | Steps 2, 4 |
| 7 | `Ashfall.Core.Tests/UIConsistencyTests.cs` (advisory only) | Test | None | Step 6 |

---

## Notes

- Steps 1–4 are independent and can be done in parallel.
- Step 5 synthesizes all previous documentation into the catalog.
- Step 6 is the only code change — extending the existing `Theme.cs`, not introducing a
  second static class. Risk is "Low" rather than "None" because it touches a file already
  consumed by all five widgets and by USS mirroring comments — an incorrect constant edit
  could shift real panel layout. Any change must be additive (new constants only) unless a
  value in `Theme.cs` is independently proven wrong.
- Step 7 provides ongoing enforcement without blocking existing panels, and is explicitly
  advisory/non-blocking until a migrated-panel baseline exists.
- No runtime behavior changes are expected from Steps 1–5 and 7. No UI regressions possible
  from documentation-only work. Step 6 carries low but non-zero risk (see above) and should be
  reviewed by a second person or tool per the project's cross-tool QA rule, since it touches a
  file with ≥2 coupled constants (spacing + typography) even though the change is narrow.
- Existing panels are NOT modified in this batch — migration to tokens is a follow-up batch.
- **Scope-creep guard:** this batch must not expand into rewriting `DESIGN_SYSTEM_RULES.md` or
  auditing panels beyond the sampling needed for Steps 1–4. If Step 7's scan reveals hundreds
  of raw-literal violations, resist the urge to fix them in this batch — that migration is
  explicitly deferred to a follow-up batch per the original scope.

## Review Notes (Corrected)

This document was adversarially reviewed against the real codebase at
`/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War` and corrected in place. Summary
of factual errors found and fixed:

1. **Color list was incomplete.** `Assets/Ashfall.Core/UI/Theme.cs` defines 18 color fields, not
   the 7 originally listed (`Ink, Warm, Pale, Hot, Muted, Dim, InkPanel`). The 11 omitted:
   `Line, LineSoft, Exclusive, Critical, Entropy, Lethe, Ozone, Ghost, EntropyGlow, LetheAmber,
   LetheRed`. Step 3 rewritten to cover all 18 and reconcile with the semantic roles already
   published in `docs/ui/DESIGN_SYSTEM_RULES.md` §2.
2. **Proposed spacing grid conflicted with an existing, shipped grid.** The original Step 2
   proposed a new 8px scale (`SpaceSm=8...SpaceXxl=48`) that collides in name and diverges in
   value from the 4px scale already live in `Theme.cs` (`SpacingXs=4...SpacingXl=24`) and
   documented in `docs/ui/DESIGN_SYSTEM_RULES.md` §1 ("Strict 4px base grid"). Rewritten to
   extend the existing grid instead of forking a second one.
3. **Proposed typography scale conflicted with existing font-size constants.** `Theme.cs`
   already defines `FontSizeH1=28, FontSizeH2=22, FontSizeH3=18, FontSizeBody=14,
   FontSizeSmall=11, FontSizeMono=12, FontSizeLabel=10` — different values from the original
   draft's `H1=24, H2=20, H3=16, Caption=12`. Rewritten to reconcile rather than duplicate.
4. **`AshfallLayoutTokens.cs` (Step 6) would have created a second source of truth.** Confirmed
   via file search that the file does not currently exist, but creating it as originally
   specified (with its own `SpaceXxs/SpaceXs/SpaceSm/...` and `FontDisplay/FontH1/...` names)
   would duplicate `Theme.cs`'s existing constants under different names/values in the same
   directory. Rewritten to extend `Theme.cs` directly instead.
5. **Widget composition example was wrong.** The original Step 5 skeleton showed
   `AshfallMetricCard` nested inside `AshfallSidebar` and both sidebar and metric card as if
   directly `new`'d by panels. In the real API, `AshfallSidebar` and `AshfallStatusRail` are
   only ever constructed internally by `AshfallDashboardShell.SetSidebar()`/`.SetStatusRail()`,
   and `AshfallMetricCard` is only ever constructed internally by
   `AshfallStatusRail.AddCard()`. Panels never call `new` on these three. Corrected in Steps 1
   and 5.
6. **"~85 panels" is an approximation, not a verified count** — actual counts range from ~83
   (filename glob) to 94 (class-name-suffix grep) depending on method; the number is in the
   right order of magnitude but Step 1 now requires the inventory to state its counting method
   instead of asserting a bare number.
7. **Step 5's "verifiable by inclusion in a test" claim was unrunnable as stated** — the Core
   test project (`Ashfall.Core.Tests`, `noEngineReferences: true`) cannot compile code
   referencing `Godot.*`-derived widget classes. Corrected to clarify snippets must be verified
   by manual compilation or a Godot-buildable location, not the Core test project.
8. **Step 7's "no false positives" was an unverifiable self-referential claim** — a test cannot
   prove it has no false positives by passing itself. Corrected to require a manual spot-check
   and to make the initial gate advisory rather than a hard assertion against a corpus of
   ~85-95 panels that almost certainly already contains many raw literals.
9. **Missing acknowledgment of overlapping existing documentation** — `docs/ui/` already
   contains `DESIGN_SYSTEM_RULES.md`, which documents colors, spacing, and typography for at
   least the trade/radio panel family. The original draft's motivation section implied a
   design system did not yet exist at all. Corrected throughout to position this batch as
   extending/formalizing existing partial documentation, not creating it from scratch, and to
   require cross-linking rather than duplication.
10. **Font weight claims tightened** — confirmed `BarlowCondensed` ships as
    Regular/Medium/SemiBold/Bold/MediumItalic and `ShareTechMono` ships Regular-only (no bold/
    italic mono variant); Step 4 corrected to not invent unloaded weights.
