# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**

# UI A11Y §2d PANEL-SCRIM TOKEN CONSOLIDATION — 2026-09-29

STATUS: FULLY INTEGRATED
Authorized: user session request "Continue doing more UI correction and UI
precision work!" (2026-09-29). Seventh package in the audit-fix series:
audit §2d, "the single largest palette-hygiene win in the tree."

> **Editorial polish (prose pass, non-contractual):** the Framing section below is commentary on
> intent and craft only. It changes no scope, no ownership, no token and no acceptance criterion.
> **MUST NOT** below remains binding — including the deliberate exclusions.

---

## 0. Framing — Fifty-Eight Ways to Draw the Same Dark

> *"Every panel in this game hand-mixed its own background. Fifty-eight times, fifty-eight slightly
> different greys, and nobody noticed because they all sat on top of the same black."*

A scrim is the least visible thing in an interface: the darkening behind a panel that lets text
read against what is underneath. It is invisible when it is right and faintly wrong when it is not
— and because each one was re-derived by hand, the tree had accumulated **58 near-grey variants**
with grey jitter from 0.02 to 0.07 and alpha jitter from 0.85 to 0.96.

The fix is not 58 fixes. It is **one authority**: `Theme.InkPanelStrong`, alpha 0.92 — the *mode*
of the drifted cluster, measured rather than invented — plus one accessor that every panel can
reach without a single new `using`.

**Tone & register.** Metrological, patient, quietly satisfying. The vocabulary is the instrument:
*scrim, tier, jitter, mode, cluster, authority, normalize*. Prose should read like a calibrator
who has spent a week measuring fifty-eight near-identical things and is now putting the ruler away.

**The interesting restraint.** Tinted scrims are **deliberately excluded** — the crisis HUD's red,
the expedition amber, the black-projects card. Those are *semantic*, not decorative. A rule that
normalised them too would be tidiness masquerading as correctness.

**The second layer.** Fifty-eight near-greys is fifty-eight acts of unrecorded authorship —
someone, once, hand-mixing a value that every later hand quietly copied wrong. The fix's elegance
is that the authority was *measured, not invented*: the mode of the drifted cluster, taken with a
ruler. And the excluded tints are the plan's conscience — the crisis red and the expedition amber
are semantic, and a rule that normalised them would be tidiness wearing correctness as a costume.

**Texture (second prose pass — commentary only).**

- Grey jitter 0.02–0.07: the fingerprint of a value re-derived by hand fifty-eight times.
- "Invisible when it is right and faintly wrong when it is not." The scrim is the interface clearing its throat.
- "The ruler is being put away." Calibration is finished; that is the whole satisfaction.

*Register below unchanged — these fragments are texture, not new recorded items.*

**The polish layer (third prose pass — commentary only).**

*(Non-contractual: craft commentary only. No scope, ownership, token, acceptance criterion or
register row changes. The register below is unchanged.)*

- The authority was *taken from the cluster, not from the air*: alpha 0.92 is the mode of what
  fifty-eight hands were already reaching for. Normalisation here is archaeology with a ruler.
- The excluded tints are the plan's conscience. The crisis red and the expedition amber are
  vocabulary, and a rule that swallowed them would be tidiness wearing correctness as a costume.
- Fifty-eight near-greys is fifty-eight acts of unrecorded authorship — each one reasonable on its
  afternoon, each one a spelling the dictionary had not yet fixed.

> "Invisible when it is right, faintly wrong when it is not. The scrim is the interface clearing
> its throat."

---

## Bounded outcome

The ~58 hand-rolled near-grey panel scrims (`new Color(0.02–0.07, …, 0.85–0.96)`)
that re-derive the panel background by hand are replaced with one authority:

1. **Core token:** `Theme.InkPanelStrong = (0.035f, 0.043f, 0.047f, 0.92f)` —
   same hue as `Ink`/`InkPanel`, alpha 0.92 = the observed modal/dense-panel
   tier in the drifted literals (16+8+5+… sites cluster at 0.88–0.96; 0.92 is
   the mode). Documented as the dense-panel/modal scrim tier; `InkPanel`
   (0.86) remains the lighter panel tier.
2. **Host accessor:** `AshfallUiHelpers.PanelScrim()` returns
   `ToColor(DesignTheme.InkPanelStrong)` — same namespace as every src/UI
   panel, so no using changes are needed.
3. **Sweep:** all 57 assignment sites matching
   `new Color(0.0[2-7]f, 0.0[2-7]f, 0.0[2-7]f, 0.(8[5-9]|9[0-6])f)` in
   `src/UI/*.cs` become `AshfallUiHelpers.PanelScrim()` (or the equivalent
   inline where the literal feeds a stylebox/ColorRect background).

**Deliberately excluded (semantic or separate concern, kept as-is):**
- Tinted scrims that the near-grey regex does not match: EmergencyResponseHud
  crisis red, ExpeditionPanel amber banner, BlackProjectsArchivePanel red card.
- Sub-0.85 stack-dependent scrims: MapDetailPanel (0.74 — §3 follow-up),
  MainMenuPanel (0.55) and GameOverPanel (0.80) carousel overlays,
  UiBackgroundCarousel.
- Token-derived raw-alpha composites (§2c lower tier): sidebar rows, meters,
  DataGrid state tints — those derive from named tokens already.

Visual effect: grey-channel jitter (0.02–0.07) and alpha jitter (0.85–0.96)
normalize to one value; all panels sit over opaque Ink so Pale-text contrast
(≈14:1) is unaffected; per-panel look changes are imperceptible except the
0.85/0.88 tier becomes 0.07 more opaque (slightly better text isolation).

## Exact files

- `Assets/Ashfall.Core/UI/Theme.cs` (1 token + doc), `src/UI/AshfallUiHelpers.cs`
  (1 accessor), ~58 src/UI panel files (mechanical literal replacement),
  `Ashfall.Core.Tests/UI/UiA11yScrimTokenGateTests.cs` (new gate)
- Governance: this plan, `WORKTREE_OWNERSHIP.md`, `.ai/state.md`

## MUST NOT

- Touch tinted/sub-0.85/carousel overlays, Core beyond the one token, any
  layout or sizing; no new usings (same-namespace accessor).

## Verification

1. `dotnet build Ashfall.csproj` — 0 errors.
2. Gate test: regex finds zero remaining near-grey scrim literals in
   src/UI outside the exclusion allowlist.
3. `--ui-layout-selftest` + `--player-panels-uitest` headless.

## Open Items & Deliberate Limits (register — not acceptance criteria)

Drawn from this plan's **deliberately excluded** list and its own annotations. Not defects — the
exclusions are the design. Any later pass that raises one must re-check the semantic reading.

| # | Open item | Why it is deliberately open | Who may resolve it (later, separately verified) |
|---|---|---|---|
| UI6-OM-1 | **Why 0.92?** | The **mode** of the observed 0.88–0.96 cluster — measured, not invented. Whether the mode is *correct* is not asserted. | A readability study, if one is ever commissioned. |
| UI6-OM-2 | Tinted scrims (crisis red, expedition amber, black-projects card). | Explicitly excluded as **semantic**, not decorative. Normalising them would be tidiness masquerading as correctness. | Never — a rule, not a gap. |
| UI6-OM-3 | Sub-0.85 stack-dependent scrims: MapDetailPanel 0.74, MainMenuPanel 0.55, GameOverPanel 0.80. | They are **stack-dependent** — a different layer with different requirements. 0.74 is a §3 follow-up. | The §3 follow-up package. |
| UI6-OM-4 | Where did the 58 near-greys come from? | Each was hand-mixed at a different time. The plan measures the cluster and normalises it; it does not reconstruct the history. | Never — texture by omission. |
| UI6-OM-5 | Does the 0.85 tier becoming 0.07 more opaque change anything? | Described as *imperceptible except slightly better text isolation*. Asserted, not snapshot-verified here. | The visual lane's snapshot goldens. |
| UI6-OM-6 | Are ~58 sites all the sites there are? | The sweep matches one regex (`0.0[2-7]…0.(8[5-9]|9[0-6])`). A scrim outside that band is invisible to it. | The gate test's allowlist review. |
