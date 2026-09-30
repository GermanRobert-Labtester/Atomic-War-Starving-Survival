# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**

# UI A11y — Final Raw-Color-Literal Sweep + Central Text-Overrun Precision

STATUS: FULLY INTEGRATED

Date: 2026-09-29
Lane: UI precision/correction (audit series package 9; follows
`docs/ui/ACCESSIBILITY_REPORT_2026-09-29.md` §2b/§2c follow-through)

---

## 0. Framing — The Last Colour

> *"A color literal is a word somebody said once in a room where nobody was keeping the dictionary."*

This plan is a dictionary closing. Every raw literal in UI chrome was a decision frozen in code with
no name — a gray chosen by eye at midnight, a red darkened until it felt right — and the sweep does
not so much change them as *give them their names*: `Muted`, `Critical`, `Entropy`, `Ink`, `Hot`.
Nothing gets brighter. Nothing gets louder. The interface simply acquires a grammar.

The plan's most professional act is what it **declines** to sweep. Sprite `Modulate` grays, day-phase
light tints, font shadows, a brightness adjust — these are *art*, not chrome, and mapping them to
text tokens would have changed what they mean. A boundary that distinguishes "what the UI says"
from "what the picture shows" is worth more than any one color mapping.

**Tone & register.** Curatorial, precise, quiet. The vocabulary is the design system: *token,
literal, chrome, tier, seam, factory*. Prose should read like a conservator's notes on a restoration
— what was touched, what was deliberately left, and why the untouched list is longer than anyone
expected.

**The second layer.** Part B is the same instinct applied to *behaviour*: `clip_text` and
`TrimEllipsis` are politeness encoded — text that draws past its rectangle collides with its
neighbours, and an ellipsis says "there is more, and I will not shout it." Moving that correction
to the `FinishLabel` seam means the ~480 raw `new Label` sites can migrate *incrementally*, which
is how a house gets rewired without anyone sleeping outside.

**Texture (second prose pass — commentary only).**

- The `TimeCapsulePanel` note font maps to `Muted`: the colour of something set aside on purpose.
- The crisis backdrop is `Critical` channels × 0.12 — a red so dark it is nearly the *memory* of red.
- The fallback `ColorRect` matched `Ink` exactly. The fallback was already speaking the language; the sweep only wrote it down.
- The two adjudicated retractions stay retracted. A sweep that re-litigates closed findings is a sweep that will never end.

*Deliberate limits: the Non-goals below and the "Deliberately NOT swept" list are this plan's
register — binding, documented, and deliberately unenlarged. No new open items are created here.*

**The polish layer (third prose pass — commentary only).**

*(Non-contractual: craft commentary only. No scope, ownership, token, acceptance criterion or
register row changes.)*

- The plan's real artefact is the untouched list. A conservator is measured by what they decline
  to restore, and here the art stays art while the chrome acquires a grammar.
- `TrimEllipsis` is manners in a property: *there is more here than we could show, and we are
  telling you so rather than pretending the name ended.*
- The `FinishLabel` seam is rewiring the house room by room without making anyone sleep outside —
  ~480 sites may arrive when they arrive.

> "Nothing gets brighter. Nothing gets louder. The interface simply acquires a grammar."

---

## Outcome

Two bounded parts, no behavior or layout redesign:

**Part A — last raw color literals on UI chrome → theme tokens (8 files, 9 sites).**
Each literal was re-verified in source and mapped to the nearest existing Core
token; none introduce new tokens or change contrast tier:

| File | Site | Old literal | New mapping |
|---|---|---|---|
| `src/UI/TimeCapsulePanel.cs` | capsule note font | `(0.8, 0.8, 0.8)` | `Muted` |
| `src/UI/EmergencyResponseHud.cs` | crisis backdrop (Severe+) | `(0.12, 0.02, 0.02, 0.95)` | `Critical` channels × 0.12, alpha 0.95 |
| `src/UI/BlackProjectsArchivePanel.cs` | classification banner bg | `(0.12, 0.04, 0.04, 0.85)` | `Critical` channels × 0.15, alpha 0.85 |
| `src/UI/ExpeditionPanel.cs` | encounter banner bg | `(0.10, 0.07, 0.04, 0.94)` | `Entropy` channels × 0.12, alpha 0.94 |
| `src/UI/UiBackgroundCarousel.cs` | fallback ColorRect | `(0.035, 0.043, 0.047, 1)` | `Ink` (exact match) |
| `src/UI/BackdropArt.cs` | placeholder dim overlay | `(0.04, 0.05, 0.06, dimAlpha)` | `Ink` channels + existing dimAlpha clamp |
| `src/World/RoomHotspotView.cs` | badge bg / hover bg / hover font | `(0.08,0.1,0.12,0.75)` / `(0.18,0.25,0.32,0.9)` / `(0.95,0.85,0.4)` | `SurfaceCard`+alpha / `Lethe` × 0.5 + alpha / `Hot` |

Deliberately NOT swept (documented, not missed):
- `src/World/MapLocationMarkerView.cs` gray `Modulate`s and
  `src/World/HoldfastInteriorView.cs` day-phase light tints — sprite/lighting
  art factors, not color chrome; mapping them to text tokens would change
  art semantics.
- `src/UI/SnapshotOrchestrator.cs` snapshot bg — test tooling.
- `src/Host/HoldfastTerminalPanel.cs` font shadow — shadow, not a token color.
- `src/Settings/UserSettings.cs` 1.15 brightness modulate — brightness adjust.
- `src/UI/GameDashboardPanel.cs:327` and `MapDetailPanel.cs:227` — already
  adjudicated in the audit (item 11 retraction; §3 stack-dependent, visual lane).

**Part B — central text-overrun precision in `src/UI/AshfallUiHelpers.cs`.**
Godot Labels default to `clip_text = false`: long text draws past its rect and
collides with neighbors. All non-autowrap label factories gain
`ClipText = true` + `TextOverrunBehavior.TrimEllipsis` via one private
`FinishLabel` seam applied per factory (autowrap factories keep wrapping);
`MakeButton` gains `ClipText = true` + `TrimEllipsis` (content-sized buttons
are unaffected; fixed-width buttons truncate with ellipsis instead of
overflowing). This is the same correction already applied ad hoc to
TradeScreenGodotPanel, AshfallMetricCard, SurvivorsPanel, GameDashboardPanel,
AshfallDataGrid, AshfallDashboardShell in earlier packages — now the default
at the factory seam so the ~480 direct `new Label` call sites can be migrated
incrementally without another sweep.

## Non-goals

- No new Core tokens; no contrast-tier changes; no layout changes.
- No migration of direct `new Label` call sites to `FinishLabel` (future).
- No visual-lane work (§3 scrim stack contrast, snapshot regeneration).

## Verification

1. `dotnet build` host target — 0 errors.
2. New static gate `Ashfall.Core.Tests/UI/UiA11yFinalColorGateTests.cs`:
   old literals absent + token reference present per Part A file; helpers
   contain `FinishLabel` + ClipText on MakeButton; token values pinned
   (SurfaceCard/Ink/Lethe/Hot/Muted/Critical/Entropy) to catch token drift.
3. Headless probes: `--ui-layout-selftest`, `--player-panels-uitest`,
   boot `--quit-after 2`.
4. Governance: claim row in `WORKTREE_OWNERSHIP.md`, `.ai/state.md` entry,
   pathspec commit, memory update.
