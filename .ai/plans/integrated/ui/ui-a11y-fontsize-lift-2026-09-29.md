# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**

# UI A11Y P2.7 FONT-SIZE LABEL LIFT 11→12 — 2026-09-29

STATUS: FULLY INTEGRATED
Authorized: user session request "Continue with more work!" (2026-09-29).
Fourth package in the audit-fix series: audit §4/§9.7, the single-token
readability lift the audit named the largest available win.

> **Editorial polish (prose pass, non-contractual):** the Framing section below is commentary on
> intent and craft only. It changes no scope, no ownership, no token and no acceptance criterion.
> **MUST NOT** below remains binding.

---

## 0. Framing — One Point

> *"The largest available accessibility win in the entire audit was one integer, in one file, in
> one constant that 282 call sites were already reading."*

`Theme.FontSizeLabel`, 11 → 12. That is the whole change. And the reason it is the whole change is
the reason design systems exist: the correct fix was not 282 individual fixes. It was one token and
a system disciplined enough that raising it reaches every label at once.

There is a smaller thing worth noticing. The change is a **floor raise**, not a reflow — the
accessibility test asserts `FontSizeLabel >= 11` and passes at 12; the economy test asserts
`FontSizeSmall >= FontSizeLabel` and 12 >= 12 holds. The plan checked both before touching the
token. And `DiegeticHintSize` (also 11) is unused in `src/UI`, so it is left alone — an unused
constant is not a readability problem.

**Tone & register.** Minimal, exact, almost whispered. The vocabulary is the design system:
*token, floor, call site, reach, precedent*. Prose should match the change: short, and only what is
needed.

**The interesting cost.** Stored `snapshots/` goldens will **visually drift** — and both snapshot
actions are `headless_compatible: false`, needing a real display. So the drift is recorded, not
fixed, and regen is handed to the visual lane. A one-point change has consequences that only a
rendering pipeline can settle.

**The second layer.** One integer is the largest accessibility win in the audit because the design
system made it so: 282 call sites were already reading one constant, and discipline turned a
282-fix problem back into a one-fix problem. The plan's manners are in the margins — both tests
checked *before* the token was touched, and an unused constant left alone because an unused
constant is not a readability problem. Even the drift is handled with grace: the goldens will move,
and the move is recorded rather than rushed.

**Texture (second prose pass — commentary only).**

- 11 → 12. The whole change. The reason it is the whole change is the reason design systems exist.
- "A floor raise, not a reflow." The tests were read first; the token was raised second.
- "A one-point change has consequences that only a rendering pipeline can settle." Smallness does not exempt anything from consequences.

*Register below unchanged — these fragments are texture, not new recorded items.*

**The polish layer (third prose pass — commentary only).**

*(Non-contractual: craft commentary only. No scope, ownership, token, acceptance criterion or
register row changes. The register below is unchanged.)*

- A floor raise is a promise that no call site may undercut — which is why one integer could be
  the largest win in the audit, and why the plan checked both tests before touching it.
- The snapshot drift is recorded, not hidden. A golden that has never drifted is usually a test
  that has never noticed.
- An unused constant is not a readability problem. `DiegeticHintSize` is left alone, and leaving
  things alone is this plan's second-smallest kindness.

> "The system did most of the work before the plan arrived. The plan's job was to notice."

---

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

## Open Items & Deliberate Limits (register — not acceptance criteria)

Drawn from this plan's own notes. Not defects — recorded consequences and deliberate non-changes.

| # | Open item | Why it is deliberately open | Who may resolve it (later, separately verified) |
|---|---|---|---|
| UIF-OM-1 | **Snapshot goldens will visually drift.** | Recorded, not blocking. Both snapshot actions are `headless_compatible: false` and need a real display. | The visual lane (`--ui-snapshot-regenerate`), as a follow-up. |
| UIF-OM-2 | Is 12 enough? | 12 is the *largest available win* the audit named, not a claim of compliance. The floor is 11 and the raise is one point. | A readability study, if one is ever commissioned. |
| UIF-OM-3 | Why is `DiegeticHintSize` still 11? | It is **unused in `src/UI`**. An unused constant is not a readability problem. | Removal or use — a separate decision. |
| UIF-OM-4 | Do the two earlier 11→12 raises count as precedent or drift? | `AshfallDataGrid.cs` headers and `AshfallSidebar.cs` hints shipped first with comments citing this audit line. The token now catches up to them. | Never — the history reads as convergence. |
| UIF-OM-5 | What about the 282 call sites that were not inspected? | Reach is asserted by call-site count, not by per-site rendering. `--ui-layout-selftest` covers layout, not legibility. | A rendered-sweep comparison at 26 px. |
