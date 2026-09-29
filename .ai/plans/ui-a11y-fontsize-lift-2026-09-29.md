# UI A11Y P2.7 FONT-SIZE LABEL LIFT 11→12 — 2026-09-29

STATUS: APPROVED BY USER
Authorized: user session request "Continue with more work!" (2026-09-29).
Fourth package in the audit-fix series: audit §4/§9.7, the single-token
readability lift the audit named the largest available win.

## Bounded outcome

`Theme.FontSizeLabel` raised 11 → 12 (`Assets/Ashfall.Core/UI/Theme.cs:176`).

Reach: `MakeMetadata` (282 call sites), `MakeLabel(string)` default, ~32
direct `FontSizeLabel` references (VerdictPanel, TradeScreenGodotPanel,
MainMenuPanel metric cards, save-slot labels, weather/event footers).
Precedent: two 11→12 raises already shipped in place
(`AshfallDataGrid.cs` headers 2026-09-26, `AshfallSidebar.cs` hints) with
comments citing this audit line.

Test compatibility (verified):
- `AccessibilitySourceAuditTests.ThemeFontSizes_MeetAccessibilityFloors`
  asserts `FontSizeLabel >= 11` — a floor, passes at 12.
- `TradeThemeAndEconomyTests` asserts `FontSizeSmall >= FontSizeLabel` —
  12 >= 12 passes.
- `DiegeticHintSize` (11, Theme.cs:183) is unused in src/UI — left as is.

Snapshot note (recorded, not blocking): stored `snapshots/` goldens will
visually drift; both snapshot actions (`--ui-snapshot-uitest` diff,
`--ui-snapshot-regenerate`) are `headless_compatible: false` (need a real
display) and belong to the visual lane. Regen is a follow-up for that lane,
not part of this package.

## Exact files

- `Assets/Ashfall.Core/UI/Theme.cs` — 1 token + doc-comment line
- Governance: this plan, `WORKTREE_OWNERSHIP.md`, `.ai/state.md`

## MUST NOT

- No per-site font overrides, no other token changes (FontSizeSmall/Mono
  stay), no snapshot regeneration in this package.

## Verification

1. `dotnet build Ashfall.csproj` — 0 errors.
2. Scoped tests (Core theme/accessibility floors + economy theme gates).
3. `--ui-layout-selftest` (headless layout gate) + `--quit-after 2` boot.
