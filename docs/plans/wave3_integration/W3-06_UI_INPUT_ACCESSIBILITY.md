# ASHFALL — WAVE 3 INTEGRATION PROGRAM · PLAN 6 OF 6

# UI, INPUT & ACCESSIBILITY INTEGRATION PLAN

**Status:** PROPOSAL — planning-only · no production path claimed
**Wave:** W3 (six-plan integration wave)
**Document:** W3-06
**Date:** 2026-09-21
**Repo:** `Atomic War` @ `Zcode_Branch`, HEAD `5be1a30a`
**Companion plans:** W3-01 (narrative), W3-02 (economy), W3-03 (psychology), W3-04 (combat), W3-05 (crafting)
**Plan-unblocking annex:** Annex U at the end — separately.

---

## 0. How to read this plan

This plan integrates the **player's hands and eyes**: input mapping, focus
navigation, controller parity, modal discipline, panel routing, HUD clarity,
readability, and accessibility. It extends the existing UI contract
(`PlayerSurfaceContract`/`Manifest`), the focus system, the modal stack, and
the accessibility presentation. It does not write prose (W2-06) or add gameplay
(W2-03); it makes the interface truthful, navigable, and readable.

### 0.1 Two selection levels

| Plan Path | Name | Meaning |
|---|---|---|
| **A** | Truth & Reach | audit panels, routes, input bindings, and a11y floors; fix gaps |
| **B** | One Interface | unify focus/modal/route contracts; harden input parity; make information legible |
| **C** | Accessible by Design | full navigation model, remapping depth, and information architecture on existing owners |

**Level 2:** ten points, each A/B/C (§4.2).

### 0.2 Default mapping

| Plan Path | A points | B points | C points |
|---|---|---|---|
| A Truth & Reach | 1–10 | — | — |
| B One Interface | 1,4,9 | 2,3,5,6,7,8 | 10 |
| C Accessible by Design | — | 2,6 | 1,3,4,5,7,8,9,10 |

### 0.3 The Wave 3 rule for this plan

> **One focus authority, one modal authority, one route contract.** Focus stays
> `AshfallFocusNavigator`/`FocusPolicy`; modals stay `ModalStackController`;
> routes stay `PlayerSurfaceContract`/`PlayerSurfaceManifest`. No panel
> manages its own global input, no second modal stack, no route table outside
> the manifest.

### 0.4 Accessibility floors (binding, from prior sealed work)

- Contrast floors and colorblind mode are sealed (`DEBT-184-*`): keep them.
- Settings persistence is the sole `UserSettings*` authority.
- Keyboard/controller close and back must always work.
- No fabricated fallback content (production-UI purity).
- Remap/AT/audio-description/cognitive features are **out of the sealed
  scope**; this plan's C path may only propose them as signed additions.

### 0.5 Vocabulary

| Term | Meaning |
|---|---|
| surface | a panel or screen from the manifest |
| route | the path to open/close a surface |
| focus | the currently selected interactive element |
| modal | a blocking dialog/overlay |
| parity | keyboard/gamepad/mouse equivalence |
| floor | minimum accessibility guarantee |
| legibility | readable, non-fabricated information |
| IA | information architecture (where things live) |

---

## 1. Executive summary

The UI layer is well-architected:

- **Route contract:** `UI/PlayerSurfaceContract.cs`, `UI/PlayerSurfaceManifest.cs`,
  `Main.PlayerSurfaces.cs`, `ExpandedIds` and panel routing with gates
  (`PanelRouteGateTests` 20/20, `PlayerSurfaceCoverageGateTests` 8/8,
  `ProductionUiNoFabricatedFallback` 4/4, `PlayerSurfaceBindingPurity` 2/2).
- **Focus:** `UI/AshfallFocusNavigator.cs`, `UI/AshfallFocusPolicy.cs`.
- **Modals:** `UI/ModalStackController.cs`.
- **Input:** `Host/AshfallInputActions.cs`.
- **HUD:** `UI/GameHudOverlay.cs`, `UI/ShelterHudPanel.cs`,
  `UI/EmergencyResponseHud.cs`, `UI/CombatHudOverlay.cs`,
  `Radio/FactionRadioHudPanel.cs`.
- **Accessibility:** `Host/UiAccessibilitySelfTest.cs`,
  `Settings/AccessibilityPresentation.cs`, sealed colorblind/contrast work,
  `--settings-selftest`, `--ui-a11y-selftest` (5/5 in wave-8 evidence),
  `player-panels-uitest`, `panel-bind-lifecycle` (17/17).
- **Lifecycle:** subscription hygiene and bind lifecycle gates exist.

The gaps:

1. **Route truth** — gates cover switch↔registry; coverage of every surface's
   open/close path under load/slot-switch states needs a full route matrix
   (Point 1).
2. **Focus completeness** — the focus navigator exists; coverage of every
   surface's interactive controls is unaudited (Point 2).
3. **Input parity** — keyboard/gamepad parity per action is unmeasured
   (Point 3).
4. **Modal discipline** — stack exists; nesting limits, escape order, and
   load-time modals need verification (Point 4).
5. **Panel lifecycle at scale** — hygiene gates exist; a ×100 open/close
   and slot-switch matrix across all panels is the audit (Point 5).
6. **HUD clarity** — HUDs exist; each element should read a real owner value
   and be legible at a glance (Point 6).
7. **Readability floors** — contrast/scale/text overflow across the panel set
   (existing a11y work; verify coverage) (Point 7).
8. **Information architecture** — where each concern lives (panel inventory,
   duplication, "which screen has X?") (Point 8).
9. **Error/feedback presentation** — canonical feedback path usage across
   every failure surface (Point 9).
10. **Navigation model depth** (C) — full keyboard/controller navigation and
    optional additions (remap depth), signed separately (Point 10).

---

## 2. Verified current state

### 2.1 Contracts and gates

| Component | Role |
|---|---|
| `PlayerSurfaceContract` | surface contract |
| `PlayerSurfaceManifest` | generated surface list |
| `Main.PlayerSurfaces` | routing switch (`OpenExpandedPanel`) |
| `PanelRouteGateTests` | switch ↔ registry parity (20/20) |
| `PlayerSurfaceCoverageGateTests` | coverage (8/8) |
| `ProductionUiNoFabricatedFallback` | no fake fallbacks (4/4) |
| `PlayerSurfaceBindingPurity` | binding purity (2/2) |
| `UiPanelContract` | contract test (1/1) |
| `PanelSubscriptionHygiene` | subscriptions (1/1) |

### 2.2 Focus/modals/input

| Component | Role |
|---|---|
| `AshfallFocusNavigator` | focus movement |
| `AshfallFocusPolicy` | focus rules |
| `ModalStackController` | modal stack |
| `AshfallInputActions` | input actions |

### 2.3 HUDs

`GameHudOverlay`, `ShelterHudPanel`, `EmergencyResponseHud`,
`CombatHudOverlay`, `FactionRadioHudPanel`.

### 2.4 Accessibility

`UiAccessibilitySelfTest`, `AccessibilityPresentation`, settings selftest,
colorblind mode + contrast (sealed), `--ui-a11y-selftest`, remap/AT/audio-desc/
cognitive **out of scope** per sealed debt rows.

### 2.5 Known earlier-wave interfaces

- W2-02 Point 8 owns **UI failure paths** (null owner, stale bind, close
  parity via the resilience kit). W3-06 must coordinate and not duplicate:
  W2-02 repairs, W3-06 audits/extends coverage.
- W2-03 Point 6 owns **forecast/cause surfaces**; W3-06 verifies presentation
  purity.
- W2-05/W3-02 provide decomposition read models; W3-06 renders them.
- UNBLOCK-03 D22 governs string freeze; W3-06 follows it once declared.

---

## 3. Scope, non-goals, rules

### 3.1 In scope

- Route matrix completeness under all lifecycle states.
- Focus coverage per surface.
- Input parity per action.
- Modal discipline and escape order.
- Panel lifecycle matrix (open/close/load/slot-switch).
- HUD value truth and glanceability.
- Readability floor coverage.
- IA inventory and duplication reduction.
- Feedback presentation consistency.
- Navigation model depth (C).

### 3.2 Non-goals

- Prose/strings (W2-06; UNBLOCK-03 for the freeze).
- Gameplay values (W2-03).
- New panels for new features (feature plans own their surfaces).
- Remap/AT/audio-desc/cognitive features without a signed addition.
- Theme redesign (sealed theme constants stay).

### 3.3 Rules

1. Every surface opens/closes through the manifest route; no ad-hoc open.
2. Focus never gets lost; every modal restores focus on close.
3. Input actions are declared once; parity is tested per action.
4. HUD numbers come from owners; no UI math; no fabricated fallback.
5. Accessibility floors never regress; the a11y selftest is a closeout gate.
6. Lifecycle: no double subscriptions; panels dispose cleanly.

---

## 4. Plan Path and decision index

### 4.1 The ten points

| # | Point | Default |
|---|---|---|
| 1 | Route matrix truth | B |
| 2 | Focus coverage | B |
| 3 | Input parity | B |
| 4 | Modal discipline | B |
| 5 | Panel lifecycle matrix | B |
| 6 | HUD value truth and clarity | B |
| 7 | Readability floor coverage | B |
| 8 | Information architecture inventory | A |
| 9 | Feedback presentation consistency | B |
| 10 | Navigation model depth | C |

### 4.2 Selection sheet

```text
PLAN W3-06 — UI, INPUT & ACCESSIBILITY
Plan Path: [ ] A Truth & Reach  [ ] B One Interface (default)  [ ] C Accessible by Design

01 route matrix ........... [A] [B] [C]   default B
02 focus coverage ......... [A] [B] [C]   default B
03 input parity ........... [A] [B] [C]   default B
04 modal discipline ....... [A] [B] [C]   default B
05 lifecycle matrix ....... [A] [B] [C]   default B
06 HUD truth/clarity ...... [A] [B] [C]   default B
07 readability floors ..... [A] [B] [C]   default B
08 IA inventory ........... [A] [B] [C]   default A
09 feedback consistency ... [A] [B] [C]   default B
10 navigation depth ....... [A] [B] [C]   default C
```

---

## 5. Decision Point 1 — Route matrix truth (default B)

### 5.1 The design question

Every surface needs a proven open/close path in every lifecycle state: fresh,
mid-load, after slot switch. Existing gates cover switch↔registry parity; the
state matrix is the gap.

### 5.2 Path A — Matrix report

- Generate a matrix: surface × lifecycle state × open/close result.
- Publish `docs/ui/ROUTE_MATRIX.md` with failures.

### 5.3 Path B — Repairs + gate

- Repair failing routes (typically missing close hooks or bind guards).
- Extend the route gate with the state conditions so regressions fail.

### 5.4 Path C — Route graph

Path B, plus a route graph (surface → entry points → exits) for IA review.

### 5.5 Acceptance

- Matrix complete; zero failures; gate enforced.

---

## 6. Decision Point 2 — Focus coverage (default B)

### 6.1 The design question

Every interactive control must participate in focus navigation; focus must be
visible and restored.

### 6.2 Path A — Coverage audit

- Per surface: list interactive controls and whether the focus navigator sees
  them.
- Report unreachable controls.

### 6.3 Path B — Focus completeness

- Register missing controls; ensure focus visibility and restore-on-close.
- Tests: tab/controller navigation reaches every control; close restores prior
  focus.

### 6.4 Path C — Focus policies per region

Path B, plus authored focus policies (wrap, skip-disabled, modal traps).

### 6.5 Acceptance

- No unreachable control; restore tested.

---

## 7. Decision Point 3 — Input parity (default B)

### 7.1 The design question

Every action must be achievable by keyboard, gamepad, and mouse where
applicable; parity is currently unmeasured.

### 7.2 Path A — Parity audit

- Action table: action → keyboard binding → gamepad binding → mouse path.
- Report actions missing a device path.

### 7.3 Path B — Parity fixes + gate

- Add missing bindings through `AshfallInputActions` (one declaration).
- Gate: every declared action has all authored device paths; a test simulates
  each.

### 7.4 Path C — Binding profiles

Path B, plus authored profiles (left/right-handed, controller types) if
signed; otherwise parity only.

### 7.5 Acceptance

- Every action has parity; test per action.

---

## 8. Decision Point 4 — Modal discipline (default B)

### 8.1 The design question

`ModalStackController` exists. Nesting limits, escape order (topmost closes
first), and load-time modal behavior need verification.

### 8.2 Path A — Modal audit

- Enumerate modal types and their stacking; test escape order.
- Report stuck or nested-forever states.

### 8.3 Path B — Discipline rules

- Escape/back closes the topmost modal first; a maximum depth is authored;
  load-time modals close on state change; focus returns per modal.
- Tests: nested open/close order; max depth; load-time close.

### 8.4 Path C — Modal choreography

Path B, plus authored modal sequencing (a series) through the stack.

### 8.5 Acceptance

- Escape order correct; depth bounded; focus restored.

---

## 9. Decision Point 5 — Panel lifecycle matrix (default B)

### 9.1 The design question

Panels must survive ×100 open/close, slot switches, and resets without leaks or
stale bindings. W2-02's kit covers four contexts at open; this extends it to
lifecycle volume and resets.

### 9.2 Path A — Lifecycle audit

- Run open/close ×100 per panel; record subscription counts and exceptions.
- Report leaks.

### 9.3 Path B — Lifecycle enforcement

- Fix leaking panels (symmetric unsubscribe; bind guards); extend
  `PanelSubscriptionHygiene` to assert counts after cycles.
- Slot-switch and reset contexts from W2-02's kit reused.

### 9.4 Path C — Panel state model

Path B, plus declared panel states (Unavailable/Ready/Stale) — shared with
W2-02 Point 8 C; coordinate rather than duplicate.

### 9.5 Acceptance

- No leak after ×100; slot-switch clean; hygiene gate green.

---

## 10. Decision Point 6 — HUD value truth and clarity (default B)

### 10.1 The design question

Each HUD element should show a real owner value, be glanceable, and never
fabricate.

### 10.2 Path A — HUD audit

- Element → owner value → update path; report fabricated or stale elements.

### 10.3 Path B — Truth + clarity

- Bind every element to its owner read model; add unit/context labels where
  missing (e.g., "rads/hr"); suppress unavailable rather than showing zero.
- Tests: HUD reflects an owner change within a tick; no fabricated fallback.

### 10.4 Path C — Adaptive HUD

Path B, plus authored priority modes (emergency, combat, calm) that reorder
elements through existing data.

### 10.5 Acceptance

- All elements owner-sourced; update tested; purity green.

---

## 11. Decision Point 7 — Readability floor coverage (default B)

### 11.1 The design question

Contrast/scale/overflow floors across every panel, not just those previously
audited.

### 11.2 Path A — Floor audit

- Run the a11y selftest across the full surface list; record failures.
- Report panels below floors.

### 11.3 Path B — Floor fixes + gate

- Fix failing panels (theme constants unchanged; layout/font-size fixes only).
- Extend the a11y gate to cover all surfaces; keep the colorblind/contrast
  floors sealed.

### 11.4 Path C — Text scaling

Path B, plus a signed text-scale option if requested (out of sealed scope
until then).

### 11.5 Acceptance

- All surfaces at/above floors; gate green.

---

## 12. Decision Point 8 — Information architecture inventory (default A)

### 12.1 The design question

Which screen holds which concern? Duplication (two places to do X) and orphans
(a concern with no home) are the audit targets.

### 12.2 Path A — IA inventory

- Concern → surface mapping; duplication and orphan report.
- No changes.

### 12.3 Path B — Consolidation proposal

- Propose consolidations (not executed without approval; feature plans own
  their surfaces).

### 12.4 Path C — IA navigation model

Path B, plus a navigation model (hubs, cross-links) for review.

### 12.5 Acceptance

- Inventory complete; proposals listed.

---

## 13. Decision Point 9 — Feedback presentation consistency (default B)

### 13.1 The design question

Every failure and success message should use the canonical feedback path;
no raw exception text; no silent failures in UI.

### 13.2 Path A — Feedback audit

- Enumerate message surfaces; classify canonical vs. ad-hoc.
- Report raw exceptions and silent UI failures.

### 13.3 Path B — Canonical routing

- Route ad-hoc messages through the feedback owner; player-readable text
  (W2-06 prose where a string is missing); no stack traces.
- Tests: a failure event reaches the canonical surface once.

### 13.4 Path C — Feedback taxonomy

Path B, plus message kinds with presentation rules (error/warning/info, with
sounds/icons through existing owners).

### 13.5 Acceptance

- No raw exceptions; canonical path used; dedupe respected.

---

## 14. Decision Point 10 — Navigation model depth (default C)

### 14.1 The design question

The full navigation model: keyboard-only play, controller-only play, and
optional additions (remap depth, screen-reader/audio-description) — the latter
only with signed scope.

### 14.2 Path A — Model audit

- Report which flows require a mouse and which rely on hover.
- List controller/keyboard dead ends.

### 14.3 Path B — Dead-end removal

- Replace hover-only/reliance paths with focusable controls through existing
  patterns.

### 14.4 Path C — Full model

Path B, plus a signed accessibility package (remap depth, audio description,
cognitive options) — explicitly outside the sealed floors until approved.

### 14.5 Acceptance

- No mouse-only dead ends; controller-only flow tested.
- (C) signed additions only.

---

## 15. Execution phases

### UI0 — Premise freeze (1 day)

- Run all UI gates/selftests; produce `P0_UI_PREMISE.md` with the surface
  list and current gate results.

### UI1 — Routes and focus (Points 1 + 2)

- Matrix; repairs; gates.

### UI2 — Input and modals (Points 3 + 4)

- Parity; discipline.

### UI3 — Lifecycle and HUDs (Points 5 + 6)

- ×100 matrix; HUD truth.

### UI4 — Readability and feedback (Points 7 + 9)

- Floor coverage; canonical messages.

### UI5 — IA and navigation (Points 8 + 10)

- Inventory; dead-end removal; signed additions.

### UI6 — Closeout

- Evidence; ledger proposals; Annex U.

---

## 16. Verification plan

| Point | Evidence |
|---|---|
| 1 | route matrix zero failures; gate |
| 2 | focus coverage; restore test |
| 3 | parity per action |
| 4 | escape order; depth; load-time close |
| 5 | ×100 clean; hygiene counts |
| 6 | HUD updates from owner change |
| 7 | a11y floors across all surfaces |
| 8 | IA inventory |
| 9 | canonical feedback once |
| 10 | dead-ends removed; (C) signed |

Commands:

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/UI
godot --headless --path . -- --ui-a11y-selftest
godot --headless --path . -- --player-panels-uitest
godot --headless --path . -- --panel-bind-lifecycle-selftest
godot --headless --path . -- --settings-selftest
```

---

## 17. Risks

| # | Risk | L | I | Mitigation |
|---|---|---|---|---|
| 1 | panel fixes break layout | M | M | minimal edits; a11y gate |
| 2 | focus changes alter UX | M | M | policy authored; review |
| 3 | input rebind conflicts | M | M | single action table |
| 4 | modal depth change breaks flows | M | M | authored limit; tests |
| 5 | a11y floors regress | L | H | closeout gate mandatory |
| 6 | theme constants touched | L | H | sealed; layout-only fixes |
| 7 | W2-02 duplication | M | M | coordinate: W2-02 repairs, W3-06 extends |
| 8 | string freeze violations | M | M | follow UNBLOCK-03 |
| 9 | new panels from other plans lack matrix rows | M | M | matrix generated from the manifest |
| 10 | scope creep to redesign | M | H | Wave 3 rule; IA is proposal-only |

---

## 18. Ownership and claims

| Phase | Claim | Paths |
|---|---|---|
| UI0 | `W3-06-UI0-PREMISE` | premise doc |
| UI1 | `W3-06-UI1-ROUTES-FOCUS` | route/focus fixes + gates |
| UI2 | `W3-06-UI2-INPUT-MODALS` | input actions; modal discipline |
| UI3 | `W3-06-UI3-LIFECYCLE-HUDS` | panel fixes; HUD binds |
| UI4 | `W3-06-UI4-READABILITY-FEEDBACK` | a11y floors; feedback routing |
| UI5 | `W3-06-UI5-IA-NAVIGATION` | inventory; dead-end removal |
| UI6 | `W3-06-UI6-CLOSEOUT` | evidence + proposals |

Coordination: W2-02 owns UI failure-path repairs (share the kit); W2-06 owns
strings; W2-03/05 and W3-02/05 supply read models to render; UNBLOCK-03 governs
the freeze.

---

## 19. Rollback and decline

| Point | Rollback | Decline consequence |
|---|---|---|
| 1 | keep matrix | state-route gaps remain |
| 2 | keep audit | unreachable controls |
| 3 | keep audit | parity gaps |
| 4 | keep audit | modal order unverified |
| 5 | keep audit | leak risk |
| 6 | keep audit | HUD truth unverified |
| 7 | keep audit | floor coverage partial |
| 8 | keep inventory | IA unknown |
| 9 | keep audit | ad-hoc messages remain |
| 10 | keep audit | mouse-only dead ends |

---

## 20. DoD and handoff

**Path A:** premise + audits complete; gate results recorded.

**Path B:** all of A, plus route/focus/parity/modal/lifecycle/HUD/readability/
feedback fixes and gates — each tested; a11y selftest green.

**Path C:** all of B, plus the signed navigation-depth package.

**Handoff:** outcome, files, contract (route/focus/modal/parity), commands,
limitations, untouched shared paths, ledger proposals, Annex U.

### 20.1 First safe step

> UI0 only: premise and gate baseline. No panel changes first.

---

# ANNEX U — PLAN-UNBLOCKING (SEPARATELY)

## U.1 What W3-06 releases

| Blocked item | Mechanism | Gate |
|---|---|---|
| Plan 37 input/focus parity (sealed prerequisite work) | Full parity measurement extends the sealed contract | UI2 |
| Plan 48 release craft (UI evidence) | Route/a11y/lifecycle gates give release evidence | UI1–UI4 |
| W2-02 UI failure paths | The resilience kit gains lifecycle volume | UI3 |
| W2-03/W2-05/W3-02/W3-05 read models | Rendering surfaces verified against owners | UI4/UI6 |
| Expansion 12–31 surfaces | New panels inherit the route/lifecycle matrix | UI1 |
| EN-07 chronicle UI (future) | IA/navigation model ready for the ledger strip | UI5 |
| Accessibility floors | Coverage extended across all surfaces | UI4 |
| UNBLOCK-03 string freeze | New strings routed per the freeze once declared | UI4 |

## U.2 Signatures needed

```text
[ ] I authorize UI0 premise + gate baseline.
[ ] I authorize UI1 route matrix + focus coverage fixes.
[ ] I authorize UI2 input parity + modal discipline.
[ ] I authorize UI3 lifecycle volume matrix + HUD truth.
[ ] I authorize UI4 readability floor coverage + feedback routing.
[ ] I authorize UI5 IA inventory + mouse-dead-end removal.
[ ] I authorize the C accessibility package separately (list features).
```

## U.3 What W3-06 never touches for unblocking

- Gameplay values (W2-03).
- Prose/strings (W2-06) except routing to the freeze.
- Feature surfaces owned by other plans.
- Theme constants and sealed floors (no regression).
- Save schema.

## U.4 The interface-release rule

A UI surface releases when its route, focus participation, close path, and
owner-sourced values all exist. A panel that shows a fabricated value releases
nothing.

---

# APPENDICES

## A.1 Selection sheet

```text
ASHFALL WAVE 3 · PLAN 6 (UI/INPUT/A11Y) · SELECTION
Date: ______  Foreman: ______  HEAD: ______
PLAN PATH: [ ] A Truth & Reach  [ ] B One Interface (default)  [ ] C Accessible by Design

01 route matrix ........... [A] [B] [C]   default B
02 focus coverage ......... [A] [B] [C]   default B
03 input parity ........... [A] [B] [C]   default B
04 modal discipline ....... [A] [B] [C]   default B
05 lifecycle matrix ....... [A] [B] [C]   default B
06 HUD truth/clarity ...... [A] [B] [C]   default B
07 readability floors ..... [A] [B] [C]   default B
08 IA inventory ........... [A] [B] [C]   default A
09 feedback consistency ... [A] [B] [C]   default B
10 navigation depth ....... [A] [B] [C]   default C
Signature: ________________
```

## A.2 Closeout gates (must all pass)

```text
[ ] Route matrix zero failures
[ ] Focus coverage complete
[ ] Parity per action
[ ] Modal discipline tested
[ ] Lifecycle ×100 clean
[ ] HUD values owner-sourced
[ ] a11y floors across all surfaces
[ ] Feedback canonical
[ ] IA inventory recorded
[ ] No theme/floor regression
```

## A.3 Glossary

| Term | Meaning |
|---|---|
| route matrix | surface × lifecycle state open/close results |
| focus coverage | interactive controls reachable by navigation |
| parity | device-path equivalence per action |
| modal discipline | stacking/escape/limit rules |
| lifecycle volume | repeated open/close/reset stress |
| HUD truth | owner-sourced HUD values |
| floors | minimum accessibility guarantees |
| IA | information architecture |

**End of Part I.** Proposal only; executes nothing; releases nothing without
U.2 signatures.

---

# PART II — DEEP DESIGN SPECIFICATIONS (CONTINUED → 180K)# W3-06 · PART II — DEEP DESIGN: POINTS 1–5

> Appended 2026-09-21. Part I (summary contract) + this expansion. Proposal
> only. The single-authority rules of §0.3 and the accessibility floors of
> §0.4 bind throughout.

---

## §II.1 Decision Point 1 — Route matrix truth (default B)

### II.1.1 The problem, precisely

The route contract exists: `PlayerSurfaceContract`, `PlayerSurfaceManifest`,
`Main.PlayerSurfaces` (the routing switch), with gates
(`PanelRouteGateTests` 20/20, `PlayerSurfaceCoverageGateTests` 8/8,
`ProductionUiNoFabricatedFallback` 4/4, `PlayerSurfaceBindingPurity` 2/2).
The remaining question: **does every surface open and close cleanly in every
lifecycle state** — fresh game, mid-load, after slot switch, after reset, with
another panel already open?

Failure modes:

1. **state-bound route** — a route that works fresh but fails after a load
   (bind guards missing).
2. **orphaned close** — a panel without a close path in one state.
3. **double-open** — a route opens a second instance instead of focusing the
   open one.
4. **stale bind** — the surface opens against old owner references after a
   slot switch.
5. **gate blind spot** — the switch↔registry parity holds, but the state
   dimension isn't tested (the gap this point closes).

### II.1.2 The route matrix definition

```text
rows:    every surface in the manifest (the generated list)
columns: lifecycle states:
  fresh          game started, no save loaded
  loaded         save loaded, day > 0
  post-switch    slot switched (A -> B)
  post-reset     reset/new game
  mid-panel      another panel currently open
cells:   result of (open, close) + focus behavior + bind health
```

### II.1.3 Procedure

```text
for surface S in manifest:
  for state in states:
    enter state
    open S via the canonical route
    assert: S is the active surface; instance count == 1
    assert: binds resolve to current owners (purity check)
    assert: close path exists from S in this state
    close S
    assert: close completes; prior focus restored; instance count == 0
```

The matrix driver is a test harness (host-level) producing a table of cells;
any red cell is a finding with the state named.

### II.1.4 The double-open rule

```text
OpenSurface(id):
  if surface active: focus instead (no second instance)
  else: instantiate and bind
```

The test asserts instance count == 1 across repeated opens (the classic
double-open is two panels stacked, discovered later).

### II.1.5 Acceptance tests (Point 1)

| Test | Expectation |
|---|---|
| route matrix | all cells green |
| single instance | repeated opens focus, not stack |
| bind health | purity check per state |
| close parity | close path per state, focus restored |
| slot-switch | binds refresh to the new session |
| gate extension | the state dimension added to the route gate |

### II.1.6 Deliverables

```text
docs/ui/ROUTE_MATRIX.md          (driven report + failures)
docs/ui/ROUTE_GRAPH.md           (C: edges and entry points)
```

### II.1.7 Cost

Harness (2-3 days), state scripting (2 days), repairs (2-3 days), gate
extension (1 day).

---

## §II.2 Decision Point 2 — Focus coverage (default B)

### II.2.1 The problem, precisely

`AshfallFocusNavigator` + `AshfallFocusPolicy` exist. The coverage question:
does every interactive control participate in focus navigation, and does
focus restore correctly?

Failure modes:

1. **unreachable control** — a button created after bind, never registered
   with the navigator.
2. **dead-end focus** — navigating to a control with no outward edges.
3. **lost focus** — modal/panel close leaves focus nowhere (next input lost).
4. **invisible focus** — focus exists but has no visual state (accessibility).
5. **phatom order** — dynamic lists (craft rows, survivor lists) with
   unstable order (focus jumps).

### II.2.2 The focus graph model

```text
FocusGraph (per surface, generated):
  nodes: interactive controls (buttons, sliders, list rows, tabs)
  edges: policy-derived navigation (spatial/declared)
  invariants:
    every node reachable from the surface entry node
    every node has at least one outward edge (or is a terminal with back)
    order is stable for dynamic lists (sorted, keyed)
```

### II.2.3 The registry contract

```text
controls register on bind:
  RegisterFocus(control, policy_hints)
controls unregister on dispose:
  UnregisterFocus(control)
gates:
  after bind: every interactive control present in the registry
  after dispose: registry cleaned (no ghosts) — the lifecycle point checks
```

### II.2.4 Restore-on-close

```text
open panel: remember prior focus
close panel: restore prior focus (or a declared fallback)
modal: trap focus within; on close restore mp
```

Tests script open/close and assert focus identity before/after.

### II.2.5 Acceptance tests (Point 2)

| Test | Expectation |
|---|---|
| coverage | every interactive control registered |
| reachability | all nodes reachable from entry |
| restore | focus restored on close (panel + modal) |
| visibility | focus visual state present on every control |
| stable order | dynamic list focus order deterministic |
| ghosts | no unregistered controls after dispose |

### II.2.6 Cost

Audit (2 days), registration fixes (2-3 days), order stabilization (1-2
days), tests (2 days).

---

## §II.3 Decision Point 3 — Input parity (default B)

### II.3.1 The problem, precisely

`AshfallInputActions` declares actions. The parity question: is every action
achievable through keyboard, gamepad, and mouse where applicable?

Failure modes:

1. **device hole** — an action bound only to mouse (no keyboard path).
2. **hidden binding** — an action wired in code, not declared (ain't in the
   action table).
3. **conflict** — two actions bound to one chord with no resolution order.
4. **context leak** — a modal-time binding firing during gameplay.
5. **undiscoverable** — a binding with no display (the player can't learn
   it).

### II.3.2 The action table

```text
docs/ui/INPUT_ACTIONS.md (generated + authored display)
  action_id -> {keyboard, gamepad, mouse, context(s), display_string}
```

Generated from the declared action set; missing device paths are findings;
conflicts flagged.

### II.3.3 The parity test

```text
for each action in the table:
  simulate keyboard binding -> assert action fired
  simulate gamepad binding -> assert action fired
  simulate mouse path (if applicable) -> assert fired
  assert context gating (modal vs. gameplay)
```

### II.3.4 The remap boundary

Remapping is out of the sealed scope (Part I §0.4); the plan delivers the
action table and parity, not a remap UI. Any remap feature is a signed C
addition (Point 10).

### II.3.5 Acceptance tests (Point 3)

| Test | Expectation |
|---|---|
| parity per action | keyboard/gamepad/mouse paths fire |
| conflicts | no unresolved chord collisions |
| context gating | modal bindings don't leak to gameplay |
| display coverage | every action has a display string |
| undeclared writes | no binding outside the action table |

### II.3.6 Cost

Table generation (1-2 days), parity tests (2-3 days), fixes (2 days), context
gating (1 day).

---

## §II.4 Decision Point 4 — Modal discipline (default B)

### II.4.1 The problem, precisely

`ModalStackController` exists. The discipline question: stacking order,
escape behavior, depth limits, and load-time modals.

Failure modes:

1. **stuck modal** — close path lost; player trapped.
2. **wrong order** — escape closes a background modal first.
3. **infinite nesting** — a modal opening itself (dialog loops).
4. **load-time presentation** — modals open during load and never resolve.
5. **focus leak** — focus escapes the modal to background controls.

### II.4.2 The modal contract

```text
ModalStack:
  push(modal): traps focus; pauses background input (context gating)
  pop(): topmost only (escape/back closes top first)
  clearOnStateChange(old, new): modals close on load/slot switch (authored)
depth limit: authored (e.g., 3); push beyond raises a finding
identity: modals keyed (no double-push of the same instance)
```

### II.4.3 Escape order test

```text
open A, open B
press escape -> B closes; A remains
press escape -> A closes
assert order; assert focus restored at each step
```

### II.4.4 Load-time rules

Modals active during a state change either resolve to a defined close or are
discarded with an authored notice; none may persist half-open. Test: save
load with a modal open (scripted) -> post-load state clean.

### II.4.5 Acceptance tests (Point 4)

| Test | Expectation |
|---|---|
| escape order | topmost closes first |
| depth limit | enforced; pushed-past logs a finding |
| identity | no duplicate pushes |
| focus trap | focus cannot leave the modal |
| load behavior | clean post-load modal state |
| stuck detection | harness opens/closes all modal types ×N |

### II.4.6 Cost

Contract implementation (2-3 days), order/depth tests (2 days), load
scripting (1-2 days), fixes (2 days).

---

## §II.5 Decision Point 5 — Panel lifecycle matrix (default B)

### II.5.1 The problem, precisely

W2-02's resilience kit covers four open contexts; this point extends to
**lifecycle volume**: open/close ×100 per panel, slot switches, resets, and
subscription accounting.

Failure modes:

1. **subscription leak** — each open adds a handler; memory and duplicate
   events grow.
2. **stale binds** — bindings to owners from a prior session survive.
3. **dirty dispose** — timers/tweens/animations survive close.
4. **double-subscribe** — one bind subscribes the same handler twice.
5. **reset survivorship** — panels survive ResetAllSessions in a broken
   state.

### II.5.2 The lifecycle harness

```text
for panel P:
  open/close ×100; assert subscription counts stable
  slot switch with P open; assert rebind clean
  reset with P open; assert P disposed
  measure: handler count, timer count, owner-ref count over cycles
```

The subscription hygiene gate (`PanelSubscriptionHygiene`) is extended with
the cycle assertions (count stability, not just presence).

### II.5.3 The panel state model (shared with W2-02)

```text
PanelState: Unavailable | Ready | Stale
transitions:
  Unavailable -> Ready on owner availability (bind success)
  Ready -> Stale on owner state change (slot switch, reset)
  Stale -> Ready on rebind; Stale panels display an authored state
```

This shared model prevents divergent "handling" definitions between the two
plans (coordination, not duplication).

### II.5.4 Acceptance tests (Point 5)

| Test | Expectation |
|---|---|
| ×100 stability | counts steady |
| slot-switch rebind | clean; stale never displayed unhandled |
| reset dispose | all panels disposed |
| timer/tween cleanup | none survive close |
| hygiene extension | cycle assertions green |

### II.5.5 Cost

Harness (2-3 days), fixes (3-4 days), state model (1-2 days with W2-02),
tests (2 days).

---

## §II.6 Points 1–5 execution order

```text
Day 1-2   P0 premise + gate baseline
Day 3-5   P1 route matrix + repairs
Day 6-8   P2 focus coverage
Day 9-10  P3 input table + parity
Day 11-12 P4 modal discipline
Day 13-15 P5 lifecycle matrix
Day 16    consolidation + handoffs
```

Shared artifacts: the manifest (P1) feeds focus (P2) and lifecycle (P5); the
action table (P3) feeds modal context gating (P4); the state model (P5)
overlaps W2-02's kit.

---

*End of Part II. Continues in Part III (Points 6–10).*# W3-06 · PART III — DEEP DESIGN: POINTS 6–10

---

## §III.1 Decision Point 6 — HUD value truth and clarity (default B)

### III.1.1 The problem, precisely

HUDs exist: `GameHudOverlay`, `ShelterHudPanel`, `EmergencyResponseHud`,
`CombatHudOverlay`, `FactionRadioHudPanel`. The question: does every displayed
value come from an owner, update within a tick, and read at a glance?

Failure modes:

1. **fabricated value** — HUD math or a default shown when data missing.
2. **stale value** — bound once, never refreshed.
3. **unitless value** — a number with no unit/context ("12" rads? hours?).
4. **zero-when-missing** — unavailable data shown as 0 (a lie).
5. **alarm fatigue** — every element urgent; nothing stands out.
6. **layout collision** — elements overlapping in a state (combat + weather).

### III.1.2 The binding registry

```text
docs/ui/HUD_BINDINGS.md (generated)
  element_id -> owner_read (call), update_policy (tick/event), unit,
                priority (calm/alert/crisis), display_ref
```

Every element names its read; the registry is the audit's basis.

### III.1.3 Truth rules

```text
T1 render only owner reads; no arithmetic in HUD code
T2 missing data: suppress or show the authored unavailable state (never 0)
T3 units/context on every numeric element
T4 update within one tick of owner change (event or poll per authored policy)
T5 priority modes reorder authored elements only (no new data)
```

### III.1.4 The update test

```text
for element in registry:
  change the owner value
  advance one tick
  assert displayed value follows
  remove the owner (unavailable state)
  assert suppressed or authored unavailable marker (never a fabricated 0)
```

### III.1.5 Acceptance tests (Point 6)

| Test | Expectation |
|---|---|
| binding coverage | every element registry-backed |
| update latency | within one tick |
| units | present per element |
| unavailable state | no fabricated zero |
| priority modes | only reorder/emphasize authored elements |
| collision check | no overlapping in sampled states |

### III.1.6 Cost

Registry generation (1-2 days), binding fixes (3 days), update tests (2
days), layout sampling (1-2 days).

---

## §III.2 Decision Point 7 — Readability floor coverage (default B)

### III.2.1 The problem, precisely

Accessibility floors exist (contrast, colorblind, sealed DEBT-184 family) and
`UiAccessibilitySelfTest` (5/5), `--settings-selftest`, `--ui-a11y-selftest`.
The coverage question: do the floors hold across **all** surfaces and states,
including dynamic content?

Failure modes:

1. **audited subset** — floors verified on the main panels only.
2. **state blindness** — contrast passes on default states, fails on alert/
   disabled states.
3. **dynamic text** — long generated strings overflow containers.
4. **scaling** — fixed font settings assume a resolution; text clips at
   authored scale changes (if any).
5. **color-only meaning** — status conveyed only by color in some elements.

### III.2.2 The audit lane

```text
for surface in manifest:
  for state in [default, alert, disabled, error, empty, long-content]:
    run contrast/scaling/overflow checks (the a11y selftest extended)
    record failures with element + state
```

### III.2.3 The floor definitions

| Floor | Value | Source |
|---|---|---|
| contrast (text) | sealed constants | DEBT-184 family |
| contrast (UI chrome) | sealed | same |
| colorblind mode | sealed set | same |
| min font size | authored | sealed settings |
| overflow policy | authored (wrap/ellipsize/scross) | this plan |
| color-independence | status text/icon required | this plan |

### III.2.4 Fix rules

Layout/font-size fixes only; **theme constants are sealed** (no regression
permission). A fix that requires a theme change escalates.

### III.2.5 Acceptance tests (Point 7)

| Test | Expectation |
|---|---|
| all surfaces × states | floors pass |
| dynamic content | long strings handled per overflow policy |
| color-independence | no color-only status |
| gate extension | the a11y gate covers the full surface list |
| no sealed regression | constants unchanged |

### III.2.6 Cost

Audit extension (2-3 days), fixes (3-4 days), gate wiring (1 day), tests (2
days).

---

## §III.3 Decision Point 8 — Information architecture inventory (default A)

### III.3.1 The problem, precisely

The IA question: which screen holds which concern, where duplication exists,
and where orphans live. This point is **inventory only** (Path A default);
consolidation is proposed, never silently executed (feature plans own their
surfaces).

### III.3.2 The inventory method

```text
concern list  := generated from panel/surface names + owner domains
for each concern:
  surfaces[] holding it
  duplication := len(surfaces) > 1 (flagged)
  orphan := len(surfaces) == 0 (flagged)
navigation depth := steps from main hub to each surface
```

### III.3.3 Deliverables

```text
docs/ui/IA_INVENTORY.md   (concern -> surfaces, duplication, orphans, depth)
docs/ui/IA_PROPOSALS.md   (B-path proposals, review-only)
```

### III.3.4 Acceptance tests (Point 8)

```text
IA_ConcernCoverage       every concern has ≥1 surface or is flagged orphan
IA_DuplicationReport     duplicates listed with owners
IA_DepthReport           navigation depth per surface
IA_ProposalsReviewOnly   no IA change executed under Path A
```

### III.3.5 Cost

Inventory generation (2 days), review pack (1 day).

---

## §III.4 Decision Point 9 — Feedback presentation consistency (default B)

### III.4.1 The problem, precisely

Every failure/success message should use the canonical feedback path; no raw
exception text; no silent UI failures.

Failure modes:

1. **ad-hoc messages** — panels each show their own toast/dialog.
2. **raw exceptions** — stack traces or exception messages in player text.
3. **silent failures** — a click fails with no visible result.
4. **double messages** — one failure reported by two systems.
5. **modal spam** — failures stacking modals instead of a quiet inline state.

### III.4.2 The feedback contract

```text
FeedbackEvent:
  kind (error | warning | info | success), text_ref, source, dedupe_key
routing:
  to the canonical feedback surface (one owner)
  dedupe by key within a window (no spam)
  no raw exceptions: text_refs only
```

### III.4.3 The audit

```text
enumerate message surfaces; classify canonical vs. ad-hoc
run failure scenarios (blocked craft, denied route, empty action)
assert: one canonical message per event; no exception text; no silence
```

### III.4.4 Acceptance tests (Point 9)

| Test | Expectation |
|---|---|
| canonical routing | all messages through the owner |
| no raw exceptions | text_refs only |
| no silence | every failure yields visible feedback |
| dedupe | repeats suppressed per window |
| modal discipline | no modal stacking for routine failures |

### III.4.5 Cost

Surface census (1-2 days), routing fixes (2-3 days), dedupe (1 day), tests
(2 days).

---

## §III.5 Decision Point 10 — Navigation model depth (default C)

### III.5.1 The problem, precisely

The C question: can the game be played keyboard-only and controller-only
without dead ends, and which optional accessibility features (remap depth,
audio description, cognitive options) are signed additions?

### III.5.2 Dead-end inventory

```text
flows requiring mouse: listed with the specific interaction
hover-only content: listed (tooltips with critical info)
unfocusable controls: listed
mouse-path alternatives: authored per finding
```

### III.5.3 The signed-additions boundary

```text
remap UI: out of sealed scope; requires a signed line
audio description: out of scope (signed only)
cognitive options (timers/pause): out of scope (signed only)
The plan's C path delivers: navigation-model completion + the above as
explicit signature items, nothing more.
```

### III.5.4 Acceptance tests (Point 10)

```text
Nav_NoMouseOnlyFlows        sampled flow walkthroughs, keyboard-only
Nav_NoHoverOnlyCritical     critical info reachable without hover
Nav_ControllerFlow          full controller pass on core flows
Nav_SignedAdditions         each addition behind its signature
```

### III.5.5 Cost

Dead-end inventory (2 days), fixes (3-4 days), controller pass (2 days),
signature items (per feature).

---

## §III.6 Points 6–10 execution order

```text
Day 1-2   P6 HUD binding registry
Day 3-5   P6 fixes + update tests
Day 6-8   P7 readability lanes across surfaces/states
Day 9-10  P8 IA inventory + proposals
Day 11-13 P9 feedback census + routing
Day 14-16 P10 dead-end inventory + fixes
Day 17    consolidation + handoffs
```

Cross-links: P1's manifest feeds P5/P7; P4's modal contract feeds P9's
feedback discipline; P6's binding registry feeds P9's message sources; P2's
focus graph feeds P10's navigation model.

---

## §III.7 Whole-plan invariants

```text
1. one focus authority, one modal authority, one route contract
2. surfaces render owners; no UI math
3. every input action declared and parity-tested
4. focus is never lost; modals trap and restore
5. lifecycles are leak-free at volume
6. HUD values are owner-sourced and unit-honest
7. accessibility floors hold on every surface and state
8. feedback is canonical, deduped, exception-free
9. navigation has no device dead ends on core flows
10. additions beyond sealed scope are signed only
```

---

*End of Part III. Continues in Part IV (playbooks).*# W3-06 · PART IV — UI AUTHORING PLAYBOOKS

---

## §IV.1 Surface authoring playbook

### IV.1.1 Surface checklist (for any new panel)

```text
[ ] manifest entry (id, title ref, entry points)
[ ] route: canonical open/close via the routing switch
[ ] single-instance rule (open focuses, not stacks)
[ ] focus graph: entry node + registered controls
[ ] modal behavior: none | trap | clear-on-state-change
[ ] binds: owner reads only; unbind on dispose
[ ] lifecycle: open/close ×100 clean; slot-switch rebind
[ ] HUD elements (if any): binding registry rows
[ ] readability: floors pass in all states
[ ] feedback: canonical messages only
[ ] input: actions declared (no code-only bindings)
[ ] tests: route matrix row, focus coverage, lifecycle
```

### IV.1.2 Anti-patterns

| Anti-pattern | Symptom | Fix |
|---|---|---|
| ad-hoc open | panel opens outside route | use manifest route |
| local state | panel caches owner data | bind directly |
| silent bind fail | blank panel | authored unavailable state |
| input in panel | panel handles raw input | actions table |
| focusless controls | tab skips them | register |
| dispose drift | handlers survive | unbind symmetry |

---

## §IV.2 Focus authoring playbook

### IV.2.1 Policy hints

```text
policy_hints:
  group: (region id)           # navigation grouping
  order: (int)                 # stable ordering within group
  wrap: bool                   # wrap at edges
  skip_disabled: bool
  modal_trap: bool
```

### IV.2.2 Rules

1. Every control declares its group/order (dynamic lists: sorted keys).
2. Wrap behavior is authored per region (no accidental tabs to nowhere).
3. Disabled controls are skipped consistently (state-driven).
4. Modals trap; on close, restore the remembered focus.
5. Visual focus state is mandatory (accessibility floor adjacent).

### IV.2.3 Focus review sheet

```text
[ ] every control registered
[ ] groups/orders authored; dynamic lists keyed
[ ] wrap/skip rules consistent
[ ] modal trap + restore
[ ] focus visual present
[ ] test: full traversal reaches everything
```

---

## §IV.3 Input authoring playbook

### IV.3.1 Action template

```yaml
action_id: ui_confirm
display: "Confirm"
bindings:
  keyboard: [Enter, Space]
  gamepad: [A]
  mouse: [primary_click]      # where applicable
contexts: [gameplay, modal, menu]
conflict_policy: authored precedence
```

### IV.3.2 Rules

1. Actions are declared once in the action table; code references ids.
2. Context gating is explicit (modal vs. gameplay).
3. Conflicts must be resolved by authored precedence, never load order.
4. Display strings exist for every action (discoverability).
5. Parity: every action has all applicable device paths.

### IV.3.3 Review sheet

```text
[ ] declared in the table (no code-only)
[ ] device paths per action
[ ] contexts correct
[ ] conflict policy resolved
[ ] display present
[ ] parity test authored
```

---

## §IV.4 Modal authoring playbook

### IV.4.1 Modal template

```yaml
modal: confirm_release
purpose: confirm a destructive/irreversible action
trap_focus: true
stack_depth: authored limit (3)
close_paths: [cancel, confirm, escape/back]
clear_on_state_change: true
result_routing: owner call per outcome
```

### IV.4.2 Rules

1. Every modal has declared close paths (no traps).
2. Escape/back closes topmost first.
3. Stack depth enforced; no self-push.
4. Load/slot changes clear modals cleanly.
5. Outcomes route through owners; modals never mutate state directly.

### IV.4.3 Review sheet

```text
[ ] close paths complete
[ ] trap + restore
[ ] depth limit declared
[ ] clear-on-change behavior
[ ] outcome routing
[ ] test: escape order, trap, load
```

---

## §IV.5 HUD authoring playbook

### IV.5.1 Element template

```yaml
element: rads_rate
source: dosimeter owner read
unit: "rads/hr"
update: event (owner change)
priority: crisis-high
unavailable: authored marker (never 0)
display_ref: hud_rads_label
```

### IV.5.2 Rules

1. No arithmetic in HUD code; render reads.
2. Units/context authored per element.
3. Unavailable states authored explicitly.
4. Priority modes reorder authored elements only.
5. Layout sampled in stacked states (combat+weather) for collisions.

### IV.5.3 Review sheet

```text
[ ] source row in registry
[ ] units present
[ ] unavailable state authored
[ ] update policy honored
[ ] priority authored
[ ] collision sampling
```

---

## §IV.6 Accessibility authoring playbook

### IV.6.1 Per-surface a11y checklist

```text
[ ] contrast floors (sealed) pass in all states
[ ] colorblind mode passes
[ ] text overflow policy applied (wrap/ellipsize/scroll)
[ ] minimum font size honored
[ ] color-independence (status text/icon)
[ ] focus visible on all controls
[ ] keyboard-only reachability for the surface's flows
```

### IV.6.2 Fix discipline

Layout/font-size only; sealed theme constants untouched. Escalate if a
constant is implicated.

### IV.6.3 Review sheet

```text
[ ] lane run across states
[ ] failures fixed per discipline
[ ] gate covers the surface
[ ] no sealed regression
[ ] dynamic content sampled
```

---

## §IV.7 Feedback authoring playbook

### IV.7.1 Message template

```yaml
feedback: craft_blocked_inputs
kind: warning
text_ref: craft_msg_inputs
source: CraftingSystem
dedupe_key: craft_blocked:<recipe>
```

### IV.7.2 Rules

1. All messages route through the canonical sink.
2. Text refs only (no raw exception messages).
3. Dedupe per key/window.
4. Failures always visible (no silence).
5. Routine failures inline; modals only for authored decisions.

### IV.7.3 Review sheet

```text
[ ] canonical routing
[ ] text ref exists
[ ] dedupe key authored
[ ] no exception text
[ ] visibility guaranteed
[ ] modal discipline respected
```

---

## §IV.8 Anti-pattern catalogue (30)

| # | Anti-pattern | Class |
|---|---|---|
| 1 | ad-hoc surface open | route |
| 2 | double-open stacking | route |
| 3 | unregistered control | focus |
| 4 | focus lost on close | focus |
| 5 | invisible focus | focus |
| 6 | unstable list order | focus |
| 7 | mouse-only action | input |
| 8 | code-only binding | input |
| 9 | unresolved chord conflict | input |
| 10 | modal context leak | input |
| 11 | stuck modal | modal |
| 12 | wrong escape order | modal |
| 13 | infinite nesting | modal |
| 14 | load-time half-open modal | modal |
| 15 | subscription leak | lifecycle |
| 16 | stale bind after switch | lifecycle |
| 17 | timer survives close | lifecycle |
| 18 | reset survivorship | lifecycle |
| 19 | HUD math | HUD |
| 20 | fabricated zero | HUD |
| 21 | stale HUD value | HUD |
| 22 | unitless number | HUD |
| 23 | collision in stacked states | HUD |
| 24 | audited-subset a11y | readability |
| 25 | color-only status | readability |
| 26 | overflow clipping | readability |
| 27 | ad-hoc failure toast | feedback |
| 28 | raw exception text | feedback |
| 29 | silent failure | feedback |
| 30 | modal spam on routine errors | feedback |

---

*End of Part IV. Continues in Part V (verification catalog).*# W3-06 · PART V — VERIFICATION CATALOG AND HARNESS DESIGN

---

## §V.1 Verification tiers

```text
T1 STATIC (seconds)
  manifest coverage, focus registry completeness, action table conflicts,
  HUD binding registry, IA inventory, surface source tables
T2 FOCUSED RUNTIME (minutes)
  route matrix, focus traversal, input parity, modal discipline,
  lifecycle volume, HUD updates, readability lanes, feedback routing
T3 SOAK (hours, shared)
  long-session open/close/reset cycles; memory/handler stability;
  accessibility sampling across states; controller-flow pass
```

---

## §V.2 T1 — static checks

| Check | Input | Detects |
|---|---|---|
| U1.1 manifest coverage | manifest vs. switch/registry | missing/extra surfaces |
| U1.2 focus registry | controls vs. registry spec | unregistered controls |
| U1.3 action table | declared actions | conflicts; missing device paths |
| U1.4 HUD registry | elements vs. reads | unbound elements |
| U1.5 IA inventory | surfaces vs. concerns | duplicates/orphans |
| U1.6 source tables | surface blocks | blocks without sources |
| U1.7 string refs | messages | missing text refs |
| U1.8 sealed guard | theme/settings constants | unauthorized changes |

U1.8 is the seal protector: any diff to sealed accessibility/theme constants
fails the check.

---

## §V.3 T2 — focused kits

### V.3.1 Route matrix kit

```text
for surface S × state:
  open -> assert active, single instance, binds healthy
  close -> assert closed, focus restored
  assert: no failures across the matrix
```

### V.3.2 Focus kit

```text
per surface: full traversal reaches every control; dynamic lists stable;
modal trap verified; restore on close verified
```

### V.3.3 Input parity kit

```text
per action: keyboard path fires; gamepad path fires; mouse path fires where
applicable; context gating verified; conflict resolution exercised
```

### V.3.4 Modal kit

```text
escape order (A/B nesting); depth limit; identity (no double push); focus
trap; load-time clean; stuck detection (all modal types ×N)
```

### V.3.5 Lifecycle kit

```text
open/close ×100 per panel (count stability); slot switch (rebind);
reset (dispose); timer/tween cleanup assertions
```

### V.3.6 HUD kit

```text
owner change -> value follows within a tick; unavailable state authored;
units present; priority modes reorder only
```

### V.3.7 Readability kit

```text
per surface × state: contrast/colorblind floors; overflow policy; min font;
color-independence; focus visibility
```

### V.3.8 Feedback kit

```text
failure scenarios: canonical message once; no exception text; no silence;
dedupe window; modal discipline
```

### V.3.9 IA kit

```text
concern coverage; duplication list; depth report; proposals review-only
```

### V.3.10 Navigation kit (C)

```text
keyboard-only walkthrough of core flows; controller-only pass; hover-only
critical info detection; signed additions gated
```

---

## §V.4 T3 — soak design

### V.4.1 Long-session harness

```text
scenario: 8 hours of accelerated play (scripted surface cycling)
metrics:
  handler/subscription counts over time (leak slope)
  panel instance counts (should be 0/1)
  focus failures count
  a11y lane samples across randomly entered states
  memory trend (bounded; no unbounded growth)
assertions: counts stable; no failures; memory trend flat within tolerance
```

### V.4.2 Controller pass

```text
scripted controller-only playthrough of core flows (menu -> shelter ->
craft -> trade -> journal -> close)
assert: no dead ends; every flow completable
```

### V.4.3 Report format

```yaml
run: T3-2027-02-a
cycles: 5000 panel cycles
subscriptions: {start: 42, end: 42, drift: 0}
instances: {max_concurrent: 1}
focus_failures: 0
a11y: {samples: 120, failures: 0}
controller: {flows: 9, dead_ends: 0}
memory_trend: flat (within 2%)
```

---

## §V.5 Evidence and closeout

```text
docs/evidence/w3-06/
  T1-*.yaml T2-*.yaml T3-*.yaml
  findings/UI-###.md
  repairs/
  P0_UI_PREMISE.md
```

Closeout acceptance:

```text
[ ] route matrix green; single-instance verified
[ ] focus coverage complete; restore tested
[ ] input parity per action
[ ] modal discipline (order/depth/trap/load)
[ ] lifecycle volume clean
[ ] HUD truth (update/units/unavailable)
[ ] a11y floors across surfaces × states
[ ] feedback canonical
[ ] navigation dead ends removed (C)
[ ] sealed constants untouched
```

---

## §V.6 Failure triage

```text
route red cell        -> stop: interface trust broken
stuck modal           -> stop: player trap
subscription leak     -> fix (lifecycle)
a11y floor regression -> stop: sealed guarantee
HUD fabrication       -> fix (truth)
silent failure        -> fix (feedback)
sealed constant diff  -> revert + escalate
flake                 -> quarantine per repo policy
```

---

## §V.7 Test naming and focus

```text
Route_<Check>        Focus_<Check>
Input_<Check>        Modal_<Check>
Lifecycle_<Check>    Hud_<Check>
Readability_<Check>  Feedback_<Check>
IA_<Check>           Nav_<Check> (C)
```

---

## §V.8 Cost model

| Tier | Effort |
|---|---|
| T1 checks | 3-4 days |
| T2 kits (10) | 9-11 days |
| T3 harness | 3-4 days |
| Closeout | 1-2 days |

---

*End of Part V. Continues in Part VI (worked threads).*# W3-06 · PART VI — WORKED THREADS

> Three end-to-end UI threads: the route matrix run, the input parity audit,
> and the accessibility sweep. Each shows the findings the kits produce and
> the repairs they imply.

---

## §VI.1 Thread 1 — The route matrix run

### VI.1.1 Setup

The matrix driver runs every surface from the manifest across five states:
fresh, loaded, post-switch, post-reset, and mid-panel (another surface open).

### VI.1.2 Findings produced (illustrative)

| # | Finding | State | Repair |
|---|---|---|---|
| UI-01 | `shelter_roster` opens a second instance when already open | mid-panel | single-instance rule in the route |
| UI-02 | `radio_log` fails to close after slot switch (bind to stale session) | post-switch | rebind/handle stale per P5 |
| UI-03 | `expedition_dispatch` close leaves focus nowhere | loaded | restore-on-close |
| UI-04 | `council_panel` opens pre-bind (blank) on fresh game | fresh | auth unavailable state |
| UI-05 | `craft_desk` bind resolves owners from the prior session | post-switch | stale bind guard |
| UI-06 | `memorial_wall` route missing from the switch in one state branch | post-reset | switch coverage fix |
| UI-07 | `trade_desk` double-binds an owner handler per open | any | bind symmetry (P5) |

### VI.1.3 Repair pattern

Most matrix findings share two causes: (a) route implementations bypassing
the single-instance rule; (b) binds not refreshed on session change. The
repairs are localized (route guard + rebind hook), not architectural.

### VI.1.4 Verification

Re-run the matrix: zero red cells; the state dimension is added to the route
gate so regressions fail statically.

### VI.1.5 What the thread teaches

1. **The state axis is where routes break** — the prior gates passed because
   they only tested the happy path.
2. **Stale binds are cross-cutting** — one fix (session-change rebind hook)
   repaired three findings.
3. **The matrix is cheap to re-run** — it becomes the standing gate.

---

## §VI.2 Thread 2 — The input parity audit

### VI.2.1 Setup

The action table is generated from declarations; the parity kit simulates
each action's device paths.

### VI.2.2 Findings produced

| # | Finding | Class | Repair |
|---|---|---|---|
| UI-08 | `journal_scroll` mouse-wheel-only (no keyboard/gamepad scroll) | parity | add bindings |
| UI-09 | `map_pan` bound in code, absent from the table | declaration | move to table |
| UI-10 | `confirm` and `interact` share a chord in menus with undefined precedence | conflict | authored precedence |
| UI-11 | `close_panel` missing display string | discoverability | display ref |
| UI-12 | `skip_notification` fires in gameplay (leaked modal context) | context | context gating |
| UI-13 | `rotate_prototype` gamepad path missing (keyboard-only) | parity | gamepad binding |
| UI-14 | `quick_use` has three unmerged definitions (device duplicates) | declaration | table merge |

### VI.2.3 The parity matrix (post-repair)

```text
actions: 47
keyboard paths: 47
gamepad paths: 47 (except authored mouse-only list of 0)
mouse paths: 41 (6 authored keyboard-only, documented)
conflicts: 0 unresolved
display strings: 47
```

### VI.2.4 What the thread teaches

1. **Declaration is the discipline** — the code-only binding (UI-09) is
   invisible until the table is generated.
2. **Conflicts need authorship** — shared chords are fine when precedence is
   written down.
3. **Contexts are part of parity** — a binding that fires in the wrong
   context is as broken as a missing one.

---

## §VI.3 Thread 3 — The accessibility sweep

### VI.3.1 Setup

The extended a11y lane runs every surface in six states: default, alert,
disabled, error, empty, long-content.

### VI.3.2 Findings produced

| # | Finding | State | Repair |
|---|---|---|---|
| UI-15 | disabled button text below contrast floor | disabled | palette/lum fix (layout-safe) |
| UI-16 | alert banner uses color-only severity | alert | add icon/text (color-independence) |
| UI-17 | long survivor names overflow the roster row | long-content | ellipsize policy + tooltip |
| UI-18 | empty journal shows a blank panel (no authored empty state) | empty | authored empty copy |
| UI-19 | error toast text from an exception string | error | text ref (feedback fix) |
| UI-20 | focus ring invisible on the council tab | default | focus visual |
| UI-21 | weather overlay text clips at minimum authored font | alert | layout fix |
| UI-22 | colorblind mode not applied to the ration bars | default | palette mapping |

### VI.3.3 The seal boundary check

UI-15 and UI-22 could tempt a theme-constant edit; the sweep confirms the
fixes are layout/mapping-only and the sealed constants remain untouched
(U1.8 green).

### VI.3.4 What the thread teaches

1. **States are where floors fail** — default-only audits pass while disabled/
   alert states rot.
2. **Color-independence is a separate floor** — colorblind mode helps, but
   color-only *meaning* is a distinct failure.
3. **Empty states are content** — the blank panel is a content finding, not a
   rendering bug.
4. **The seal holds** — every fix stayed within layout discipline.

---

## §VI.4 Cross-thread coverage

| Thread | Points exercised |
|---|---|
| route matrix | 1, 2 (focus restore), 5 (binds) |
| input parity | 3, 4 (context) |
| a11y sweep | 7, 9 (feedback text), 6 (HUD elements) |

Remaining points (8 IA, 10 navigation) have their own lighter audits; the
three threads carry the load.

---

## §VI.5 The seeded findings catalogue (UI-01…22)

```text
route:       UI-01..07
input:       UI-08..14
a11y:        UI-15..22
each finding: file/state evidence + repair + test
```

---

*End of Part VI. Continues in Part VII (Q&A and operational model).*# W3-06 · PART VII — EXTENDED Q&A AND OPERATIONAL MODEL

---

## §VII.1 Governance Q&A (Q1–Q10)

**Q1. Does this plan redesign the UI?**
No. It audits routes, focus, input, modals, lifecycle, HUD truth, floors,
IA, and feedback; repairs are localized and preserve the existing look.

**Q2. Who owns the surface contract?**
`PlayerSurfaceContract`/`PlayerSurfaceManifest` and the routing switch. This
plan extends coverage and gates, not ownership.

**Q3. Does Path B change settings or save data?**
No. Accessibility/theme constants are sealed; settings stay the existing
authority. No save changes.

**Q4. Can new panels be added?**
Yes — via the surface playbook (manifest entry, route, focus, binds, floors,
tests). New panels enter the matrix automatically (generated from the
manifest).

**Q5. What about remapping?**
Out of the sealed scope; only via a signed C addition (Point 10).

**Q6. How does the plan treat W2-02's UI work?**
Coordination: W2-02 owns failure-path repairs (the resilience kit); this plan
extends lifecycle volume and shares the panel state model. No duplicated
repairs.

**Q7. What if a repair requires a theme change?**
Stop and escalate; sealed constants are not negotiable in this plan.

**Q8. How is "no UI math" enforced here?**
Surface source tables + grep + the HUD/legibility kits. Rendered values must
equal owner reads.

**Q9. Do we touch strings?**
New copy (empty states, feedback text) follows the freeze process when
declared (UNBLOCK-03); until then, corpus workflow (W2-06).

**Q10. What is the smallest Path A?**
P0 + route matrix baseline + focus registry audit + action table generation
+ HUD binding registry + a11y lane baseline. No repairs.

---

## §VII.2 Method Q&A (Q11–Q25)

**Q11. Why is the state axis the biggest gap?**
Because UI bugs cluster at state transitions (load/switch/reset), which
happy-path gates never exercise.

**Q12. Why track subscriptions per cycle?**
Leaks are invisible in single opens; ×100 exposes the slope. The gate watches
for drift, not just presence.

**Q13. Why is focus restore a test?**
Because losing focus is a silent failure: the next input goes nowhere and the
player blames the game. The test is trivial and catches it.

**Q14. Why an action table instead of code bindings?**
Because declaration makes coverage mechanically checkable; code bindings are
invisible to audits (UI-09).

**Q15. Why is context gating part of parity?**
A binding firing in the wrong context is a bug class of its own (UI-12); the
action table makes contexts explicit.

**Q16. Why does the modal stack need identity keys?**
To prevent double-pushing the same modal (UI-class loops); identity makes
stack state debuggable.

**Q17. Why audit HUD units?**
A number without unit/context is ambiguous and teaches the player nothing;
unit honesty is part of truth.

**Q18. Why suppress unavailable rather than show zero?**
Zero is a fabricated value; suppression or an authored marker tells the
truth.

**Q19. Why six a11y states?**
Because disabled/alert/empty/long-content are where floors break; default-
only audits are misleading.

**Q20. Why is color-independence separate from colorblind mode?**
Colorblind mode remaps palettes; color-only meaning (severity by hue) is a
structure problem that palette remapping doesn't fix.

**Q21. Why is IA inventory Path A default?**
Because consolidation is a design decision owned by feature plans; this plan
supplies evidence, not decisions.

**Q22. Why canonical feedback routing?**
Ad-hoc messages multiply, duplicate, and leak exception text; one path makes
them countable, dedupeable, and reviewable.

**Q23. Why no modal stacking for routine failures?**
Modal fatigue; inline/queued feedback keeps the player in flow.

**Q24. Why a controller-only pass?**
Because parity tests each action, but the pass tests *flows* — the dead ends
live between actions.

**Q25. What is the acceptance for "navigation depth"?**
Core flows completable keyboard-only and controller-only with no mouse-only
steps and no hover-only critical information.

---

## §VII.3 Tooling Q&A (Q26–Q35)

**Q26. New tooling?**
Matrix driver, focus traversal harness, action table generator, HUD registry
generator, a11y lane extension, feedback census. Scripts + tests.

**Q27. Reused tooling?**
Existing gates (route/coverage/purity), the a11y selftest, settings selftest,
panel lifecycle harness (W2-02), snapshot family (W2-05) for surfaces.

**Q28. Where does the matrix live?**
Driver + report under docs/ui + evidence; rerun per release.

**Q29. How do we avoid a giant test file?**
Per-kit files following the naming; the matrix driver is one focused tool.

**Q30. How is the a11y lane kept fast?**
Per-surface × state sampling with the selftest heuristics; full lane on
release, sampled lane on change.

**Q31. What evidence per repair?**
Before/after screenshots or matrix rows, command, HEAD, owner, reviewer.

**Q32. How are dynamic lists stabilized?**
Sorted keys; the focus kit asserts determinism across openings.

**Q33. How are layout collisions tested?**
Sampled stacked states (combat+weather, alert panels) with element overlap
assertions.

**Q34. What about localization impact?**
Long-content lane includes the longest authored strings; new languages follow
the freeze/localization plan.

**Q35. How is the sealed guard implemented?**
A U1.8 check diffing sealed constant files; any change fails until explicitly
unsealed by the owner.

---

## §VII.4 Content Q&A (Q36–Q50)

**Q36. What tone for empty states?**
In-world, plain, never jokey at the player's expense (W2-06).

**Q37. Do HUD elements carry flavor?**
No — HUD is function; flavor lives in tooltips/journal. Units first.

**Q38. What about diegetic vs. system UI?**
Diegetic presentation (radio panel aesthetic) stays; truth rules still apply
to its values.

**Q39. How are error messages phrased?**
Same as crafting block copy: plain, specific, in-world. "You're short on
filters", not "error: insufficient".

**Q40. Can a panel be intentionally sparse?**
Yes; sparseness is design. Blank default states without authored emptiness
copy are findings (UI-18).

**Q41. Are tooltips required for everything?**
No; but critical information may not be hover-only (Nav rule).

**Q42. How is text scaling handled without a setting change?**
Within the sealed settings; the plan fixes clipping at the authored sizes.

**Q43. What about icon-only buttons?**
They need accessible names (display refs), part of the focus/feedback
registry.

**Q44. How do we handle right-to-left locales?**
Out of current scope (no RTL locale active); if added, a signed localization
item (W3-06 monitors, doesn't implement).

**Q45. Is the journal a HUD?**
It's a surface; its values follow the same truth rules.

**Q46. Do weaudit every panel visually?**
Sampled via snapshots (W2-05 family); the matrix covers behavior, snapshots
cover presentation regressions.

**Q47. How are alert overlays audited?**
As states (alert) in the a11y lane and as modals in discipline tests.

**Q48. What about performance of the HUD?**
Owner reads per tick, cached only with invalidation contracts; the tune plan
owns profiling, this plan ensures bindings are cheap reads.

**Q49. Can panels have their own input handling?**
No — actions table only; panels consume action events.

**Q50. What is the worst UI failure class?**
The player pressing something and nothing happening (silent failure);
followed by losing focus/being trapped. Both are stop-class findings.

---

## §VII.5 Operational model

### VII.5.1 Staffing

| Role | Count | Responsibility |
|---|---|---|
| UI engineer | 1 | routes, focus, input, modals, lifecycle, HUD |
| a11y reviewer | part-time | floors, lanes, seal |
| verifier | shared | kits/soak/evidence |
| W2-02 liaison | part-time | shared kit/state model |
| W2-06 liaison | part-time | strings |

### VII.5.2 Schedule (4-5 weeks)

```text
Week 1  P0 + route matrix + focus registry
Week 2  P1/P2 repairs; P3 input table + parity
Week 3  P4 modal discipline; P5 lifecycle volume
Week 4  P6 HUD truth; P7 readability lanes
Week 5  P8 IA; P9 feedback; P10 navigation; soak; closeout
```

### VII.5.3 Handoffs

| To | Handoff |
|---|---|
| W2-02 | lifecycle volume results; shared panel state model |
| W3-01 | journal surface truth; oracle displays |
| W3-02 | trade/ration/flow surfaces source tables |
| W3-03 | needs/care/journal/relationship surfaces |
| W3-04 | readiness/heat/ranging surfaces |
| W3-05 | craft surface blocks and messages |
| W2-05 | snapshot coverage rows |
| W2-03 | HUD pressure displays measurement |
| UNBLOCK-03 | new strings via freeze |

---

*End of Part VII. Continues in Part VIII (C-path, matrices, appendices).*# W3-06 · PART VIII — C-PATH DESIGNS, MATRICES, AND APPENDICES

---

## §VIII.1 C-path designs

### VIII.1.1 C1 — Full navigation model

A declared focus/navigation graph per surface (entry node, region groups,
edges) generated from policies and reviewed visually. Enables navigation QA
(no dead ends) and future surface work. Read-only model; no runtime change
beyond what B already delivers.

### VIII.1.2 C2 — Remap depth

Signed addition: a binding remap UI over the action table, with conflict
detection and reset-to-default. Persistence via the existing settings owner;
no new settings store. Guardrails: sealed defaults remain recoverable;
parity tests rerun against remaps.

### VIII.1.3 C3 — Audio description track

Signed addition: authored descriptions for key visual events (not full
blind-play; a bounded track). Requires content (W2-06) and an audio owner
interface; explicitly out of scope until signed.

### VIII.1.4 C4 — Cognitive options

Signed addition: optional longer timers, queued notifications, simplified
alert modes. Settings-owner extension; each option authored and bounded.

### VIII.1.5 C5 — Surface snapshot regression

Formalized snapshot diffs for every surface × state (W2-05 family), versioned
and gated on visual changes. Catches presentation regressions the behavioral
kits miss.

### VIII.1.6 C6 — Controller profiles

Authored profiles (layout preferences, dead-zone bands) within settings;
parity tests rerun per profile. Signed addition.

### VIII.1.7 C7 — Feedback taxonomy depth

Message classes with authored presentation rules (icons, tones via existing
audio owners, queue priority). Builds on P9 without new authorities.

### VIII.1.8 C-bundle recommendation

```text
first: C1 navigation model + C5 snapshots (QA leverage)
then:  C2 remap + C6 controller profiles (if signed)
then:  C7 feedback depth
last:  C3/C4 accessibility additions (largest commitments)
```

### VIII.1.9 C signature block

```text
[ ] C1 navigation model   [ ] C2 remap depth
[ ] C3 audio description  [ ] C4 cognitive options
[ ] C5 snapshot gate      [ ] C6 controller profiles
[ ] C7 feedback taxonomy
```

---

## §VIII.2 Interaction matrix

| Receiver | Interface | Direction |
|---|---|---|
| W2-02 | lifecycle kit + panel state model | shared |
| W3-01 | journal surfaces; oracle displays | out |
| W3-02 | trade/ration/flow source tables | out |
| W3-03 | needs/care/relationship surfaces | out |
| W3-04 | readiness/heat/ranging surfaces | out |
| W3-05 | craft block messages; chain board | out |
| W2-05 | snapshot rows for surfaces | out |
| W2-03 | HUD pressure readings | out |
| W2-06 | copy/strings | out |
| UNBLOCK-03 | freeze routing | in |
| W2-01 | maintenance gates feed UI checks | in |

### VIII.2.1 Never-cross list

```text
[ ] never a second focus/modal/route authority
[ ] never theme or sealed accessibility constant changes
[ ] never remap/AT/cognitive features without signatures
[ ] never UI math or fabricated values
[ ] never raw exception text in player copy
[ ] never save schema changes
```

---

## §VIII.3 Expanded glossary

| Term | Definition |
|---|---|
| action table | declared input actions with device paths/contexts |
| bind health | owner reads resolving to the current session |
| canonical feedback | the single message sink |
| context gating | modal vs. gameplay input separation |
| dead-end focus | control with no outward navigation |
| display ref | authored label for an action/element |
| focus graph | controls + navigation edges for a surface |
| focus trap | modal restraint of focus movement |
| hard route | manifest-declared open/close path |
| HUD binding registry | element → owner read map |
| identity key | modal/bind dedupe identity |
| long-content lane | a11y sampling with longest strings |
| navigation depth | steps from hub to surface |
| parity | device-path coverage per action |
| route matrix | surfaces × lifecycle states results |
| sealed guard | check preventing constant edits |
| single-instance rule | open focuses; no duplicates |
| stale bind | owner reference from a prior session |
| subscription drift | handler count change over cycles |
| surface source table | block → owner read mapping |
| unavailable state | authored replacement for missing data |
| wrap rule | authored navigation edge behavior |

---

## §VIII.4 Artifact index

| Artifact | Type |
|---|---|
| `docs/ui/ROUTE_MATRIX.md` | generated |
| `docs/ui/FOCUS_GRAPH.md` | generated (C1 full) |
| `docs/ui/INPUT_ACTIONS.md` | generated |
| `docs/ui/HUD_BINDINGS.md` | generated |
| `docs/ui/IA_INVENTORY.md` | generated |
| `docs/ui/IA_PROPOSALS.md` | authored (review) |
| `docs/ui/SURFACE_SOURCES.md` | authored table |
| `docs/evidence/w3-06/*` | evidence |

---

## §VIII.5 Expanded signature sheet

```text
ASHFALL WAVE 3 · PLAN 6 (UI/INPUT/A11Y) · EXECUTION SIGNATURES
HEAD: ________  Date: ________  Foreman: ________

[ ] P0 premise + gate baseline
[ ] P1 route matrix + single-instance + state gate
[ ] P2 focus coverage + restore
[ ] P3 input table + parity + context gating
[ ] P4 modal discipline (order/depth/trap/load)
[ ] P5 lifecycle volume + shared state model (W2-02)
[ ] P6 HUD truth + update tests
[ ] P7 readability floors across surfaces × states
[ ] P8 IA inventory (+ proposals review)
[ ] P9 feedback routing + dedupe
[ ] P10 navigation dead ends (C additions listed)
[ ] C2/C3/C4/C6 signed individually: ________

Retained: sealed constants untouched; no new authorities; no save changes.
```

---

*End of Part VIII. Continues in Part IX (checklists and sketches).*# W3-06 · PART IX — IMPLEMENTATION CHECKLISTS AND REFERENCE SKETCHES

---

## §IX.1 Point 1 checklist — routes

```text
[ ] manifest ↔ switch parity gate extended with state dimension
[ ] single-instance rule implemented and tested
[ ] bind health per state (fresh/loaded/switch/reset)
[ ] close path per state; focus restored
[ ] matrix driver + report + gate
```

## §IX.2 Point 2 checklist — focus

```text
[ ] registration on bind; unregistration on dispose
[ ] coverage audit per surface
[ ] reachability traversal test
[ ] restore-on-close (panel + modal)
[ ] focus visual state on every control
[ ] stable ordering for dynamic lists
```

## §IX.3 Point 3 checklist — input

```text
[ ] action table generated from declarations
[ ] device paths per action (applicable)
[ ] conflicts resolved by authored precedence
[ ] context gating (modal/gameplay)
[ ] display refs present
[ ] parity kit per action
```

## §IX.4 Point 4 checklist — modals

```text
[ ] close paths complete per modal
[ ] escape order (topmost first)
[ ] depth limit; identity keys; no self-push
[ ] focus trap + restore
[ ] load/slot behavior clean
[ ] stuck detection harness
```

## §IX.5 Point 5 checklist — lifecycle

```text
[ ] open/close ×100 counts stable
[ ] slot-switch rebind clean
[ ] reset disposes all panels
[ ] timers/tweens cleaned
[ ] shared PanelState model (W2-02)
```

## §IX.6 Point 6 checklist — HUD

```text
[ ] binding registry for every element
[ ] update within a tick
[ ] units/context per element
[ ] unavailable states authored
[ ] priority modes reorder only
[ ] collision sampling in stacked states
```

## §IX.7 Point 7 checklist — readability

```text
[ ] a11y lane across surfaces × 6 states
[ ] overflow policy applied
[ ] color-independence
[ ] focus visibility
[ ] sealed guard (U1.8) green
```

## §IX.8 Point 8 checklist — IA

```text
[ ] concern inventory generated
[ ] duplication/orphan lists
[ ] depth report
[ ] proposals review-only
```

## §IX.9 Point 9 checklist — feedback

```text
[ ] census of message surfaces
[ ] canonical routing
[ ] text refs (no exceptions)
[ ] dedupe windows
[ ] no silence per failure
[ ] modal discipline for routine errors
```

## §IX.10 Point 10 checklist — navigation

```text
[ ] mouse-only flow inventory + alternatives
[ ] hover-only critical info inventory + alternatives
[ ] controller-only core-flow pass
[ ] signed additions gated
```

---

## §IX.11 Reference sketch — route open (illustrative)

```text
OpenSurface(id, state):
    if active(id): focus(id); return            # single-instance
    if !manifest.Has(id): log finding; return
    panel := instantiate(id)
    if !panel.Bind(currentSession):             # stale session guard
        panel.Show(Unavailable); return
    rememberFocus()
    pushFocus(panel.Entry)
    activeId := id
```

## §IX.12 Reference sketch — focus restore (illustrative)

```text
CloseSurface(panel):
    unbind(panel)                    # symmetry with bind
    popFocus()                       # restore remembered focus
    activeId := null
    assert focus != null
```

## §IX.13 Reference sketch — modal push (illustrative)

```text
PushModal(m):
    if stack.Has(m.key): log duplicate; return
    if stack.Depth >= DEPTH_LIMIT: log finding; return
    stack.Push(m)
    trapFocus(m)
    inputContext := modal
PopModal():
    m := stack.Pop()                 # topmost only
    restoreFocus(m)
    inputContext := gameplay if stack.Empty else modal
```

## §IX.14 Reference sketch — HUD update (illustrative)

```text
HudTick():
    for e in registry.VisibleElements():
        v := e.ownerRead()           # no math here
        if v.Available: e.Show(v.value, e.unit)
        else:           e.ShowUnavailable(e.marker)
```

## §IX.15 Reference sketch — feedback route (illustrative)

```text
Feedback(kind, textRef, source, dedupeKey):
    if sink.Seen(dedupeKey, window): return
    sink.Post({kind, textRef, source})   # canonical surface
    sink.Mark(dedupeKey)
```

## §IX.16 Reference sketch — a11y lane (illustrative)

```text
for surface, state in lanes:
    enter(state)
    report := a11y.Check(surface)    # contrast, overflow, color-only, focus
    for failure in report: record(surface, state, failure)
```

---

## §IX.17 Default parameters (proposals)

| Parameter | Value | Note |
|---|---|---|
| modal depth limit | 3 | findings beyond |
| feedback dedupe window | 10 s | per key |
| lifecycle cycles | 100 | per panel |
| a11y states | 6 | default/alert/disabled/error/empty/long |
| focus traversal pass | full | per surface |
| memory drift tolerance | 2% | soak |

---

*End of Part IX. Continues in Part X (runbook).*# W3-06 · PART X — EXECUTION RUNBOOK: P0 TO CLOSEOUT

---

## §X.1 Preconditions

```text
[ ] Annex U.2 signatures for phases being run
[ ] §VIII.5 C items signed or excluded
[ ] P0 premise first
[ ] claim registered
[ ] focused test policy respected
[ ] W2-02 coordination for the shared kit/state model
```

## §X.2 P0 — premise (day 1–2)

```text
1. verify gate baseline: PanelRouteGateTests, PlayerSurfaceCoverageGateTests,
   ProductionUiNoFabricatedFallback, PlayerSurfaceBindingPurity,
   PanelSubscriptionHygiene, UiPanelContract
2. run a11y/settings selftests; record outputs
3. generate the surface list from the manifest; snapshot into the premise
4. inventory HUD overlays + action declarations + modal usages
5. list existing known issues (if any) from the maintenance docs
6. write docs/ui/P0_UI_PREMISE.md
```

## §X.3 P1 — routes

```text
1. build the matrix driver (surface × state)
2. run baseline matrix; record red cells
3. implement single-instance rule; repair red cells (ranked)
4. extend the route gate with the state dimension
5. matrix green; evidence filed
```

## §X.4 P2 — focus

```text
1. build the coverage audit (controls vs. registry)
2. repair unregistered controls; stabilize dynamic ordering
3. restore-on-close implementation + tests
4. focus visual audit (feeds P7)
5. traversal test green
```

## §X.5 P3 — input

```text
1. generate the action table
2. resolve conflicts by authored precedence
3. add missing device paths; context gating
4. display refs registered
5. parity kit green
```

## §X.6 P4 — modals

```text
1. enumerate modal types and close paths
2. enforce escape order, depth limit, identity
3. focus trap + restore
4. load/slot behavior; stuck detection
5. kit green
```

## §X.7 P5 — lifecycle

```text
1. build the ×100/switch/reset harness
2. repair leaks/binds/timers
3. extend the hygiene gate with cycle assertions
4. align the PanelState model with W2-02
5. kit green
```

## §X.8 P6 — HUD

```text
1. generate the binding registry
2. fix unbound/stale/fabricated elements
3. units/unavailable states authored
4. priority modes verified
5. kit green
```

## §X.9 P7 — readability

```text
1. extend the a11y lane across surfaces × states
2. fix failures (layout/font only; sealed guard green)
3. wire the gate coverage
4. long-content sampling
5. lane green
```

## §X.10 P8 — IA

```text
1. generate the concern inventory
2. duplication/orphan/depth reports
3. proposals written (review-only)
```

## §X.11 P9 — feedback

```text
1. census message surfaces
2. route through the canonical sink; remove ad-hoc toasts
3. text refs (no exceptions); dedupe windows
4. silence scan (failure scenarios)
5. kit green
```

## §X.12 P10 — navigation

```text
1. dead-end inventory (mouse-only, hover-only)
2. repairs per finding
3. controller-only core-flow pass
4. signed additions listed with signatures
```

## §X.13 Soak

```text
1. long-session harness (5000 cycles)
2. assertions: counts stable; no failures; memory trend flat
3. controller pass recorded
4. evidence pack
```

## §X.14 Closeout

```text
1. final kits + soak on frozen HEAD
2. evidence pack
3. closeout memo
4. Annex U releases recorded
5. debt rows for deferrals
```

### Closeout memo template

```text
OUTCOME:
FILES:
CONTRACT: routes single-instance; focus complete/restored; input parity;
  modal discipline; lifecycle clean; HUD truth; floors; feedback canonical
COMMANDS: T1/T2/T3 + results
LIMITATIONS:
SHARED PATHS TOUCHED:
LEDGER PROPOSALS:
ANNEX U RELEASES EARNED:
```

## §X.15 In-execution decisions

| Situation | Decision |
|---|---|
| a repair needs a theme constant | stop; escalate (sealed) |
| a panel needs a new route in the manifest | add via playbook; matrix regenerates |
| an action cannot have a device path | author the exception list (documented) |
| a modal has no close path | stop; add before proceeding |
| W2-02 claims overlap | coordinate (shared kit/state model) |
| a11y fix requires content | author empty-state copy via W2-06 |
| flake | quarantine per policy |

## §X.16 Maintenance

```text
per content/surface change: matrix row + registry entries + lane sample
weekly: T1; matrix spot-run
per release: T3; snapshot gate (if C5); sealed guard
```

---

*End of Part X. Continues in Part XI (acceptance and findings catalogue).*# W3-06 · PART XI — ACCEPTANCE MATRIX AND SEEDED FINDINGS CATALOGUE

---

## §XI.1 Acceptance matrix (10 points × 5 dimensions)

| # | Point | Truth | Coverage | Discipline | Guarantee | Tests |
|---|---|---|---|---|---|---|
| 1 | routes | matrix green | all surfaces | single-instance | close per state | Route_* |
| 2 | focus | registry complete | all controls | trap/restore | visual focus | Focus_* |
| 3 | input | table declared | all actions | precedence/context | display strings | Input_* |
| 4 | modals | close paths | all modal types | order/depth | focus trap | Modal_* |
| 5 | lifecycle | count stable | all panels | rebind/dispose | no leaks | Lifecycle_* |
| 6 | HUD | owner reads | all elements | update policy | units/unavailable | Hud_* |
| 7 | readability | floors pass | surfaces × states | sealed guard | color-independence | Readability_* |
| 8 | IA | inventory complete | all concerns | review-only | depth known | IA_* |
| 9 | feedback | canonical sink | all messages | dedupe | no silence | Feedback_* |
| 10 | navigation | dead ends removed | core flows | signed additions | controller pass | Nav_* |

---

## §XI.2 Seeded findings catalogue (UI-01…40)

### XI.2.1 Routes and focus (UI-01–UI-12)

| # | Finding | Class | Repair |
|---|---|---|---|
| UI-01 | double-open stacking | route | single-instance |
| UI-02 | stale bind after switch | route | rebind guard |
| UI-03 | close leaves focus nowhere | focus | restore |
| UI-04 | blank pre-bind panel | route | unavailable state |
| UI-05 | route missing in one state | route | coverage |
| UI-06 | double-bind per open | lifecycle | bind symmetry |
| UI-07 | unregistered control | focus | register |
| UI-08 | dead-end focus node | focus | outward edge |
| UI-09 | invisible focus ring | focus | visual state |
| UI-10 | unstable dynamic ordering | focus | sorted keys |
| UI-11 | focus escapes modal | modal | trap |
| UI-12 | focus skip on disabled inconsistent | focus | authored policy |

### XI.2.2 Input and modals (UI-13–UI-22)

| UI-13 | mouse-only scroll | parity | add paths |
| UI-14 | code-only binding | declaration | table move |
| UI-15 | unresolved chord conflict | conflicts | precedence |
| UI-16 | missing display string | discoverability | ref |
| UI-17 | context leak | context | gating |
| UI-18 | gamepad path missing | parity | binding |
| UI-19 | duplicate action definitions | declaration | merge |
| UI-20 | wrong escape order | modal | topmost rule |
| UI-21 | depth exceeded silently | modal | limit + finding |
| UI-22 | load-time half-open modal | modal | clear rule |

### XI.2.3 Lifecycle and HUD (UI-23–UI-30)

| UI-23 | subscription leak | lifecycle | unbind |
| UI-24 | timer survives close | lifecycle | cleanup |
| UI-25 | reset survivorship | lifecycle | dispose |
| UI-26 | HUD math | HUD | render reads |
| UI-27 | fabricated zero | HUD | unavailable state |
| UI-28 | stale HUD value | HUD | update policy |
| UI-29 | unitless number | HUD | units |
| UI-30 | collision in stacked states | HUD | layout |

### XI.2.4 Readability, IA, feedback, navigation (UI-31–UI-40)

| UI-31 | disabled-state contrast fail | a11y | palette/lum fix |
| UI-32 | color-only severity | a11y | icon/text |
| UI-33 | long-name overflow | a11y | ellipsize |
| UI-34 | blank empty state | content | empty copy |
| UI-35 | exception text in message | feedback | text ref |
| UI-36 | silent failure | feedback | route message |
| UI-37 | duplicate toasts | feedback | dedupe |
| UI-38 | modal spam on routine error | feedback | inline |
| UI-39 | concern duplication | IA | propose consolidation |
| UI-40 | mouse-only core flow | navigation | alternative |

---

## §XI.3 Kit coverage

| Kit | Findings |
|---|---|
| Route_* | UI-01..06 |
| Focus_* | UI-07..12 |
| Input_* | UI-13..19 |
| Modal_* | UI-20..22, UI-11 |
| Lifecycle_* | UI-23..25 |
| Hud_* | UI-26..30 |
| Readability_* | UI-31..34 |
| Feedback_* | UI-35..38 |
| IA_* | UI-39 |
| Nav_* | UI-40 |

Every seeded finding has a catching kit.

---

## §XI.4 Repair ranking

```text
1. stop-class: route red cells, stuck modals, silent failures, sealed regression
2. leaks/traps: lifecycle, focus loss, context leaks
3. truth: UI math, fabricated values
4. parity: missing device paths/conflicts
5. floors: a11y beyond the audit subset
6. cosmetic: copy, ordering preference
cap 20/phase; overflow -> debt
```

---

## §XI.5 Completion meter

```text
[ ] route matrix green (all states)
[ ] focus coverage + restore
[ ] parity per action
[ ] modal discipline complete
[ ] ×100 lifecycle clean
[ ] HUD truth
[ ] floors across surfaces × states
[ ] feedback canonical/no silence
[ ] navigation dead ends removed (or signed list)
[ ] sealed guard green
```

---

## §XI.6 Evidence minimum per repair

```text
before state (matrix cell/screenshot), after, command, HEAD, owner, reviewer
```

---

*End of Part XI. Continues in Part XII (samples, glossary, and handoffs).*# W3-06 · PART XII — SAMPLE DOCUMENTS, GLOSSARY SUPPLEMENT, AND HANDOFF INDEX

---

## §XII.1 Sample: route matrix report row

```markdown
## surface: shelter_roster
| state | open | instance | binds | close | focus | notes |
|---|---|---|---|---|---|---|
| fresh | ok | 1 | healthy | ok | restored | — |
| loaded | ok | 1 | healthy | ok | restored | — |
| post-switch | ok | 1 | healthy (rebind) | ok | restored | fixed UI-02 |
| post-reset | ok | 1 | healthy | ok | restored | — |
| mid-panel | focus existing | 1 | healthy | ok | restored | fixed UI-01 |
```

## §XII.2 Sample: focus graph excerpt

```markdown
## surface: craft_desk (nodes 14, edges 21)
entry: recipe_list
groups: [recipes, requirements, actions]
policy: wrap recipes; skip disabled requirement rows; trap none
dynamic: recipe rows sorted by recipe_id (stable)
test: traversal reaches all 14 from entry; restore returns to last recipe
```

## §XII.3 Sample: action table row

```markdown
| action_id | keyboard | gamepad | mouse | contexts | display |
|---|---|---|---|---|---|
| ui_confirm | Enter/Space | A | click | gameplay,modal,menu | "Confirm" |
| close_panel | Esc | B | — | menu,modal | "Close" |
| journal_scroll | Up/Down/PgUp/PgDn | stick/dpad | wheel | menu | "Scroll" |
```

## §XII.4 Sample: HUD binding row

```markdown
| element | source | unit | update | priority | unavailable |
|---|---|---|---|---|---|
| rads_rate | dosimeter read | rads/hr | event | crisis | "—" marker |
| temp_in | thermal read | °C band | tick | alert | suppressed |
| water_stock | inventory read | days | event | calm | authored note |
```

## §XII.5 Sample: feedback entry

```markdown
FEEDBACK craft_blocked_inputs
kind: warning | text_ref: craft_msg_inputs | source: CraftingSystem
dedupe: craft_blocked:<recipe> (10 s window)
observed: once per attempt; no exception text; inline (no modal)
```

## §XII.6 Sample: finding file

```markdown
FINDING UI-16
class: input/discoverability
state: map_pan action
evidence: src/Host/MapInput.cs:88 (binding not in table)
scenario: input parity audit, step 4
expected: declared action with display string
observed: code-only binding; no display; keyboard path missing
repair: move to action table; add keyboard/gamepad paths; display ref
owner: UI
status: repaired (evidence/repairs/UI-16.md)
```

---

## §XII.7 Glossary supplement

| Term | Definition |
|---|---|
| a11y lane | per-surface × state accessibility sampling |
| canonical sink | single feedback destination |
| close parity | close path present in every state |
| display ref | authored label for action/element |
| focus restore | returning focus after close/modal |
| hover-only | critical info reachable only by pointer hover |
| identity key | dedupe identity for modal/feedback |
| inline feedback | non-modal failure presentation |
| instance count | 0/1 expectation for surfaces |
| long-content lane | a11y state with longest strings |
| mouse-only | flow with no keyboard/gamepad alternative |
| priority mode | authored HUD emphasis ordering |
| registry drift | focus/HUD registry falling out of sync |
| sealed guard | U1.8 constant-change check |
| state axis | lifecycle dimension of the route matrix |
| subscription drift | handler count change over cycles |
| unavailable marker | authored replacement for missing data |
| visual focus | rendered focus state on controls |
| wrap rule | edge navigation behavior per region |

---

## §XII.8 Handoff index

| Receiver | Deliverable |
|---|---|
| W2-02 | lifecycle results; shared PanelState model; extended hygiene gate |
| W3-01 | journal surface rows; oracle displays |
| W3-02 | trade/ration/flow surface source tables |
| W3-03 | needs/care/relationship/memorial surface rows |
| W3-04 | readiness/heat/ranging surface rows; alert presentation |
| W3-05 | craft blocks/messages; chain board |
| W2-05 | snapshot rows; visual regression coverage |
| W2-03 | HUD pressure readings measurement |
| W2-06 | empty-state and feedback copy |
| UNBLOCK-03 | new strings through the freeze |

---

## §XII.9 Known limitations

```text
L1  Remap/AT/cognitive features are signed-only; not in B.
L2  RTL/localization not implemented; monitored only.
L3  Snapshot gating (C5) depends on W2-05's tooling state.
L4  Controller-only pass covers core flows; exhaustive flows at C.
L5  Modal depth limit value (3) is a proposal.
L6  HUD priority modes are authored content owned with W2-03.
L7  Audit lanes sample long-content; true localization lengths unknown until
    localization lands.
```

---

*End of Part XII. Continues in Part XIII (expansion integration and close).*# W3-06 · PART XIII — EXPANSION SURFACE INTEGRATION AND FIELD GUIDE

---

## §XIII.1 Expansion surface integration

Every expansion (12–31) adds panels; each must enter the UI contract. The
integration checklist per expansion:

```text
[ ] manifest entry + route declared
[ ] focus graph built (regions, ordering)
[ ] binds owner-sourced; lifecycle tested
[ ] HUD elements (if any) registered
[ ] a11y lane state sampled
[ ] feedback routed canonically
[ ] input actions added to the table
[ ] snapshot rows added (C5)
```

### XIII.1.1 Known expansion surface classes

| Class | Examples | UI focus |
|---|---|---|
| hub panels | foundry, glassworks, press | stations/recipes views |
| management | rosters, schedules, rationing | lists, assignment controls |
| intelligence | networks, heat, missions | maps, dossiers |
| narrative | chronicle, archive, memorial | reading surfaces |
| defense | readiness, ranging, alerts | status + action controls |
| economy | trade desk, market intel | tables, trends |

Each class declares its focus policy profile (list/map/table) from a small
authored set — no bespoke navigation definitions per panel.

### XIII.1.2 The expansion gate

A new panel releases when: route green, focus covered, lifecycle ×100 clean,
HUD values truthful, floors pass, feedback canonical. The route matrix
regenerates from the manifest, so new panels join the standing gates
automatically.

---

## §XIII.2 Field guide: "the panel won't close"

```text
1. close path declared for this state?        route matrix row
2. modal above it trapping input?             modal stack inspection
3. bind exception swallowed?                  feedback log
4. focus restore target missing?              focus graph
5. escape action context-gated wrongly?       action table
```

## §XIII.3 Field guide: "the button does nothing"

```text
1. action declared and in table?              input parity kit
2. context correct (modal/gameplay)?          context gating
3. handler bound (or stale)?                  bind health
4. failure reported silently?                 feedback silence scan
5. control focusable/visible?                 focus coverage
```

## §XIII.4 Field guide: "the HUD number is wrong"

```text
1. source row correct?                        HUD registry
2. value recomputed in UI?                    no-math check
3. update policy firing?                      update kit
4. unavailable shown as zero?                 unavailable rule
5. unit/context missing?                      units rule
```

## §XIII.5 Field guide: "text is unreadable"

```text
1. which state? (default/alert/disabled/…)    a11y lane
2. contrast floors?                           sealed constants check
3. overflow policy applied?                   layout check
4. color-only meaning?                        color-independence
5. focus visible?                             focus visual audit
```

## §XIII.6 Field guide: "the game needs a mouse here"

```text
1. is the flow in the dead-end inventory?     nav audit
2. hover-only critical info?                  hover inventory
3. control unfocusable?                       focus coverage
4. alternative action declared?               action table
5. signed exception?                          signature list
```

---

## §XIII.7 Maintenance calendar

```text
per panel change: matrix row; registry entries; lane sample; snapshot
weekly: T1 checks; matrix spot-run; feedback silence scan
per release: T3 soak; controller pass; sealed guard; snapshot gate (C5)
per expansion: the integration checklist above
```

---

## §XIII.8 Escalation map

| Finding | Owner | Escalation |
|---|---|---|
| route red cell | UI engineer | stop-the-line |
| stuck modal | UI engineer | stop-the-line |
| sealed regression | a11y reviewer | revert + escalate |
| silent failure | UI engineer | fix (feedback) |
| subscription leak | UI engineer | fix (lifecycle) |
| HUD fabrication | UI engineer | fix (truth) |
| a11y floor | a11y reviewer | fix (layout only) |
| IA decision | design owner | proposals only |
| new strings | W2-06 | freeze routing |

---

## §XIII.9 The UI promise

The interface is the game's contract with the player: every press lands
somewhere, every number means something, every state can be left, and no one
is locked out by their input device or their eyesight. Those are not features
— they are the floor. This plan exists to make the floor hold on every
surface, in every state, for the whole session.

> Nothing happens silently. Nothing traps the player. Every number tells the
> truth.

---

## §XIII.10 Final control (interim)

**W3-06 status:** expanded through Part XIII. Remaining parts (XIV–XVIII)
carry the closeout addenda: final checklists, the complete runbook card, and
the closing control. Proposal only; no execution without Annex U and §VIII.5.

*Document control: W3-06 · Wave 3 (expanded, in progress) · HEAD
5be1a30a.*# W3-06 · PART XIV — REVIEWER PACKET AND TRAINING DRILLS

---

## §XIV.1 The reviewer's primer

### XIV.1.1 What to look for (five lenses)

| Lens | Question | Failure shown |
|---|---|---|
| Reach | can I leave every state? | traps, missing close |
| Landing | does every press do something? | silent failures |
| Truth | does every number mean something? | fabricated/stale values |
| Access | can I play without a mouse/at low vision? | parity/floors |
| Quiet | is feedback calm and single? | spam, exception text |

### XIV.1.2 Red flags

```text
a panel that cannot be closed from some state
a button that fails without feedback
a HUD number with no unit or a fabricated zero
an action only reachable by mouse
a modal that can open twice
a subscription count that grows
an error message quoting code
a disabled state below the contrast floor
a long name that clips
a flow that ends in a hover-only tooltip
```

---

## §XIV.2 Training drills

### Drill 1 — the trapped modal

```text
Given: a modal with no cancel path.
Expected: route/modal finding — stop-class. Return with close paths added.
```

### Drill 2 — the silent button

```text
Given: a craft button that does nothing when requirements unmet.
Expected: feedback finding. Return; route a canonical warning with the
reason.
```

### Drill 3 — the fabricated zero

```text
Given: rads display shows 0 when the dosimeter is unavailable.
Expected: truth finding. Return; suppress or show the authored marker.
```

### Drill 4 — the mouse trap

```text
Given: the map can only be panned with the mouse.
Expected: parity finding. Return with keyboard/gamepad paths.
```

### Drill 5 — the good empty state

```text
Given: an empty journal with an authored quiet line about unwritten days.
Expected: SIGN — the state is content, not a bug.
```

### Drill 6 — the sealed temptation

```text
Given: a contrast fix proposed as a theme-constant edit.
Expected: stop and escalate. The fix must be layout/mapping-only.
```

---

## §XIV.3 The review session format

```text
attendees: UI engineer + a11y reviewer (+ W2-02 if lifecycle)
artifacts: matrix report, focus graph, action table, lane results, feedback
agenda:
  1. walk the matrix (any red is a stop)
  2. walk the parity table
  3. walk the lane failures
  4. walk the feedback census
  5. decide: sign / return-with-notes / escalate (seal)
output: sign-off line; findings recorded
```

---

## §XIV.4 Calibration rules

```text
- reviewers calibrate quarterly on real changes
- new reviewers pass all six drills first
- "feels cluttered" is not a finding; name the state and element
- sealed concerns always escalate, never negotiate locally
```

---

## §XIV.5 The sign-off line

```text
TRANCHE <name> — SIGNED: routes green; no silent failures; no traps;
truthful values; parity + floors held; sealed guard intact.
```

---

*End of Part XIV. Continues in Part XV (complete checklists).*# W3-06 · PART XV — COMPLETE CHECKLIST PACK

---

## §XV.1 Surface checklist (full)

```text
[ ] manifest entry + title ref
[ ] canonical route open/close (single-instance)
[ ] focus graph: entry + all controls; stable ordering
[ ] modal usage: none/trap/clear-on-change; close paths
[ ] binds: owner-sourced; unbind symmetric
[ ] lifecycle: ×100 + switch + reset clean
[ ] HUD elements registered (if any)
[ ] a11y: floors across 6 states; overflow policy
[ ] feedback: canonical messages; text refs
[ ] input: actions in table with display refs
[ ] tests: matrix row + focus traversal + lane sample
[ ] evidence: before/after where repaired
```

## §XV.2 Route checklist (P1)

```text
[ ] matrix driver built; states scripted
[ ] baseline recorded (red cells listed)
[ ] single-instance implemented
[ ] stale binds guarded (session-change hook)
[ ] close paths present per state
[ ] state dimension added to the route gate
[ ] matrix green
```

## §XV.3 Focus checklist (P2)

```text
[ ] registration on bind / unregistration on dispose
[ ] coverage audit across surfaces
[ ] reachability traversal green
[ ] restore-on-close (panel + modal)
[ ] focus visual on every control
[ ] dynamic list ordering stable
```

## §XV.4 Input checklist (P3)

```text
[ ] action table generated
[ ] device paths per action
[ ] conflicts resolved (authored precedence)
[ ] context gating verified
[ ] display refs for all actions
[ ] parity kit green
```

## §XV.5 Modal checklist (P4)

```text
[ ] close paths per modal
[ ] escape order (topmost)
[ ] depth limit + finding on exceed
[ ] identity keys (no double push)
[ ] focus trap + restore
[ ] load/slot clean
```

## §XV.6 Lifecycle checklist (P5)

```text
[ ] ×100 cycles: counts stable
[ ] slot switch: rebind clean
[ ] reset: dispose all
[ ] timers/tweens cleaned
[ ] hygiene gate extended (cycle assertions)
[ ] W2-02 state model aligned
```

## §XV.7 HUD checklist (P6)

```text
[ ] registry covers every element
[ ] updates within a tick
[ ] units/context present
[ ] unavailable states authored
[ ] priority modes reorder only
[ ] collision sampling
```

## §XV.8 Readability checklist (P7)

```text
[ ] lane across surfaces × 6 states
[ ] overflow policy applied
[ ] color-independence
[ ] focus visibility
[ ] sealed guard green
[ ] dynamic long-content sampled
```

## §XV.9 IA checklist (P8)

```text
[ ] concern inventory generated
[ ] duplication/orphan lists
[ ] depth report
[ ] proposals marked review-only
```

## §XV.10 Feedback checklist (P9)

```text
[ ] census complete
[ ] canonical routing (no ad-hoc toasts)
[ ] text refs only
[ ] dedupe windows
[ ] silence scan clean
[ ] routine failures inline
```

## §XV.11 Navigation checklist (P10)

```text
[ ] mouse-only inventory + repairs
[ ] hover-only critical inventory + repairs
[ ] controller core-flow pass
[ ] signed additions gated
```

## §XV.12 The completion meter (full)

```text
[ ] all ten point checklists above complete
[ ] T1 checks green
[ ] T2 kits green (10)
[ ] T3 soak: counts stable; memory flat; controller pass clean
[ ] sealed guard green
[ ] closeout memo + Annex U filed
```

## §XV.13 The per-release ritual

```text
1. rerun the matrix (new surfaces included)
2. rerun parity (new actions)
3. rerun the lane (changed surfaces)
4. run the feedback census delta
5. confirm sealed guard
6. file evidence
```

---

*End of Part XV. Continues in Part XVI (reference tables and Q&A supplement).*# W3-06 · PART XVI — REFERENCE TABLES AND Q&A SUPPLEMENT

---

## §XVI.1 Reference: the lifecycle states and expectations

| State | Binds | Focus | Routes | Notes |
|---|---|---|---|---|
| fresh | owner-ready surfaces only | entry focus set | available surfaces open | unavailable states authored |
| loaded | bound to session | remember/restore | all open | — |
| post-switch | rebound | restored | all open | stale guards |
| post-reset | disposed/reborn | entry focus | available only | no survivors |
| mid-panel | healthy | focus existing on re-open | single-instance | — |

## §XVI.2 Reference: modal close-path matrix

| Modal class | Escape | Cancel | Confirm | Load clear |
|---|---|---|---|---|
| confirm | yes | yes | yes | yes |
| dialog | yes | yes | n/a | yes |
| overlay (alert) | yes | yes | n/a | yes |
| sequence step | yes (abort) | yes | yes | yes |

## §XVI.3 Reference: the action contexts

```text
gameplay: world interactions, movement, quick panels
menu:     panel navigation, lists, tabs
modal:    confirm/dialog controls only
system:   always available (settings/close)
```

No action may fire outside its declared contexts.

## §XVI.4 Reference: HUD priority modes

| Mode | Emphasis | Authored by |
|---|---|---|
| calm | resources, time | default |
| alert | weather/defense values | event |
| crisis | survival-critical only | event |
| combat | threats/ammo | encounter |

Modes reorder/emphasize authored elements; they never add data.

## §XVI.5 Reference: the a11y lane states

```text
default      ordinary content
alert        alert/urgent presentations
disabled     disabled controls
error        failure messages
empty        no content states
long         longest authored strings
```

## §XVI.6 Q&A supplement (Q51–Q80)

**Q51. Can a panel opt out of the matrix?**
No; the manifest generates rows. Unavailable panels are still routed/tested.

**Q52. What if a surface is debug-only?**
Excluded from the live manifest with an authored note; the exclusion is
visible.

**Q53. How do we handle panels with sub-tabs?**
Tabs are focus regions within the surface; each tab's controls register;
the focus graph covers all tabs.

**Q54. What if a modal is opened by another modal?**
Allowed within the depth limit; identity keys prevent self-push.

**Q55. How are keyboard-only players guided?**
Focus visibility + display refs + the parity table; the game teaches each
action's binding.

**Q56. Is the mouse wheel a required path?**
No; scroll actions must have keyboard/gamepad alternatives (UI-13).

**Q57. How do we test for focus loss after close?**
The restore test asserts focus identity before open and after close.

**Q58. What if an owner read is expensive?**
Cache with an invalidation contract (HUD update policy); no ad-hoc caching.

**Q59. How is unavailable data distinguished from zero?**
By the authored marker/suppression rule; zero is a value, missing is a
state.

**Q60. What about transient owner states (mid-load)?**
Panels show the unavailable/loading state during transitions; no fabricated
values.

**Q61. How do repairs avoid breaking snapshots?**
Snapshots are regenerated per approved change (C5); layout fixes are
expected diffs.

**Q62. Can a surface block input intentionally?**
Only via modals; a surface never swallows input on its own.

**Q63. How do we handle text that is too long in one locale only?**
The long lane uses authored maxima; localization adds locale maxima via the
freeze process.

**Q64. What is the acceptance for feedback timeliness?**
Immediate for direct actions; within one tick for owner-driven events.

**Q65. Are audio cues part of feedback?**
Existing audio owners may accompany messages; this plan routes, W2-06/dos
audio own the cues.

**Q66. How are notifications queued?**
Canonical sink queue with priorities; no per-panel queues.

**Q67. What about controller rumble/haptics?**
Out of scope unless authored; routed through existing input/feedback owners
if added (signed).

**Q68. How do we prevent modal spam during load?**
Load-time clear rule + feedback dedupe; modals are never part of loading.

**Q69. What is the HUD policy for combat?**
Priority mode combat emphasizes threat/ammo elements; the same truth rules
apply.

**Q70. How is the journal's completeness shown?**
The journal surface lists entries from the owner; completeness is W3-01's
contract; the UI renders honestly.

**Q71. What if a requirement message is too long for the space?**
Overflow policy (ellipsize) + tooltip with the full text (non-critical
hover-only allowed since the text is present elsewhere).

**Q72. How do we handle right-click/context menus?**
As actions entering the table (ui_context); no raw input handling.

**Q73. What is the rule for auto-focusing fields?**
Authored per surface; text fields focus on open only where authored.

**Q74. How do panels communicate changes to each other?**
Through owners, never through each other; the owner change flows to all
bound surfaces.

**Q75. What about performance with many bound elements?**
Read-per-tick with event updates; the tune plan measures, this plan keeps
bindings as cheap reads.

**Q76. How are modal results delivered?**
Through the owner call; the modal closes first, then the owner mutation
reflects to surfaces.

**Q77. Can two panels bind the same owner?**
Yes — reads are shared; writes only through actions/owners, never panels.

**Q78. How does the UI handle long sessions?**
The soak: stable counts, flat memory; no UI state accumulates.

**Q79. What if the manifest itself is wrong?**
The coverage gate fails; fixing the manifest is the repair (single authority).

**Q80. What is the single sentence for a UI contributor?**
"Every press lands, every number means, every state can be left."

---

*End of Part XVI. Continues in Part XVII (final control and reading card).*# W3-06 · PART XVII — FINAL CONTROL AND READING CARD

---

## §XVII.1 Reading card

```text
ASHFALL W3-06 · UI/INPUT/ACCESSIBILITY · READING CARD

WHAT:    every press lands; every number means; every state can be left;
         every device works; every floor holds; every message is calm.
WHY:     the interface is the game's contract with the player.
HOW:     P0 baseline -> P1 routes -> P2 focus -> P3 input -> P4 modals ->
         P5 lifecycle -> P6 HUD -> P7 readability -> P8 IA -> P9 feedback ->
         P10 navigation.
GATES:   T1 registries/tables; T2 ten kits; T3 soak (counts stable, memory
         flat, controller pass, lanes green).
STOPS:   route red cells; stuck modals; silent failures; sealed regression.
NEVER:   second authorities; UI math; fabricated values; exception text;
         unsanctioned remap/AT features.
SIGN:    Annex U + §VIII.5.
```

## §XVII.2 The plan's ten sentences

```text
1. Every surface opens and closes from every state.
2. One instance; re-opening focuses.
3. Every control is focusable, visible, and reachable.
4. Every action works on every device the design intends.
5. Modals can be left, in order, always.
6. Panels are leak-free at volume and rebind on session change.
7. Every HUD value is an owner read, with a unit, updated within a tick.
8. Every surface meets the floors in every state, sealed constants intact.
9. Every message is canonical, calm, deduped, and except-free.
10. No core flow requires a mouse or a hover.
```

## §XVII.3 Consolidated open items (pre-execution)

```text
[ ] P0: confirm gate baselines and command names
[ ] P0: generate the manifest surface list; snapshot counts
[ ] P0: inventory modals/actions/HUDs
[ ] coordinate: W2-02 kit/state model; W2-05 snapshots; W2-06 strings
[ ] signatures: C2/C3/C4/C6 listed for decision
[ ] sealed constants: diff-protect before any layout work
```

## §XVII.4 The final acceptance statement

```text
Path B closes when:
  the matrix is green in all states,
  focus is complete and restored,
  every action has parity and display,
  modals obey order/depth/trap/load,
  lifecycles are leak-free,
  HUD values are truthful,
  floors hold across surfaces and states,
  feedback is canonical and silent-free,
  and the sealed guard is intact.

Path C adds (signed):
  remap depth, audio description, cognitive options, controller profiles,
  snapshot gating, full navigation model, feedback taxonomy.
```

## §XVII.5 Version record

| Version | Change |
|---|---|
| v1.4 | Parts II–III deep designs |
| v1.5 | Part IV playbooks |
| v1.6 | Part V verification |
| v1.7 | Part VI worked threads |
| v1.8 | Part VII Q&A + operational |
| v1.9 | Part VIII C-path + matrices |
| v2.0 | Parts IX–X checklists + runbook |
| v2.1 | Part XI acceptance + findings |
| v2.2 | Part XII samples + glossary |
| v2.3 | Part XIII expansion integration + field guide |
| v2.4 | Part XIV reviewer packet |
| v2.5 | Part XV complete checklists |
| v2.6 | Part XVI reference tables + Q&A |
| v2.7 | Part XVII this control |

## §XVII.6 Final control (interim)

**W3-06 status:** expanded through Part XVII. Closing appendices follow.
Proposal only; no execution without Annex U and §VIII.5 signatures. Binding:
the never-cross list (§VIII.2.1), the ten sentences (XVII.2), and the sealed
guard.

*Document control: W3-06 · Wave 3 (expanded) · HEAD 5be1a30a.*# W3-06 · PART XVIII — THE ACCESSIBILITY HANDBOOK

---

## §XVIII.1 The floors (restated and exact)

| Floor | Requirement | Authority |
|---|---|---|
| text contrast | sealed constants | DEBT-184 family |
| chrome contrast | sealed constants | same |
| colorblind mode | sealed remap set | same |
| minimum font size | sealed settings | same |
| color-independence | status includes text/icon | this plan |
| overflow | authored policy per element | this plan |
| focus visibility | rendered focus on every control | this plan |
| keyboard reach | every surface flow | this plan |
| controller reach | core flows | this plan (C exhaustive) |

## §XVIII.2 The seal discipline

```text
sealed files: constants for contrast/colorblind/font sizes
guard: U1.8 diff check in CI
any change: requires explicit owner unsealing — not available to this plan
repairs: layout, spacing, font-size choices within the sealed minimum, palette
  *mapping* per element (using sealed colors, not changing them)
```

The distinction matters: remapping which sealed color an element uses is
allowed; changing the sealed color set is not.

## §XVIII.3 The verification procedure (per surface change)

```text
1. enter the six lane states
2. run the a11y selftest heuristics
3. record failures with element + state
4. fix within discipline
5. re-run; file evidence
6. confirm the guard is green
```

## §XVIII.4 The common fixes catalog

| Failure | Fix |
|---|---|
| disabled text too faint | choose a brighter sealed color; add state styling |
| alert color-only | add icon + label |
| long name clipping | ellipsize + tooltip/full-view path |
| blank empty state | authored copy (content, W2-06) |
| focus invisible | focus ring rendering |
| weather text clipping | layout reflow; font within sealed min |
| ration bars color-only | labels/patterns per sealed palette |
| error text from exception | text ref (feedback) |

## §XVIII.5 The audit cadence

```text
per surface change: lane sample
weekly: changed-surface lanes
per release: full lane + snapshot gate (C5)
per expansion: the integration checklist's a11y row
```

## §XVIII.6 The accessibility review questions

```text
1. Can this be understood without color?
2. Can this be reached without a mouse?
3. Can this be read at the sealed minimum size?
4. Can this state be left without a pointer?
5. Does the screen reader-equivalent path exist (labels/refs)?
6. Is the failure/alert expressed in words?
```

## §XVIII.7 What this handbook refuses

```text
- "we'll do accessibility later" (floors are gates)
- "color is the design" (structure over hue)
- "users will figure it out" (display refs exist for a reason)
- "it's just a small panel" (the lane covers all surfaces)
- "the theme needs to change" (seal; escalate)
```

---

*End of Part XVIII. Continues in Part XIX (copy and message register).*# W3-06 · PART XIX — COPY, FEEDBACK, AND LABEL REGISTER

> The strings the UI systems need, listed for the corpus handoff (W2-06) and
> the freeze route (UNBLOCK-03). Register only; final wording owned by the
> corpus.

---

## §XIX.1 Empty states

```text
empty_journal        "nothing written here yet"
empty_roster         "no one is listed yet"
empty_map            "no places marked yet"
empty_trade          "no one is trading right now"
empty_craft          "nothing can be made here yet"
empty_radio          "only static"
empty_memorial       "no names here yet"
empty_archive        "no records found"
```

## §XIX.2 Unavailable states

```text
unavailable_generic  "not available"
unavailable_power    "no power"
unavailable_crew     "no one assigned"
unavailable_data     "waiting for readings"
unavailable_bind     "not connected yet"
```

## §XIX.3 Failure messages (feedback)

```text
fb_denied_action     "that can't be done right now"
fb_missing_inputs    "you're short on {item}"
fb_no_space          "there's no room"
fb_no_power          "nothing to power it"
fb_blocked_route     "the way is blocked"
fb_crew_needed       "someone has to do it"
fb_item_broken       "it's broken"
fb_not_learned       "you don't know how yet"
fb_target_gone       "they're not here anymore"
fb_generic_error     "that didn't work"
```

(No exception text, ever.)

## §XIX.4 Confirmation prompts

```text
confirm_release      "Let them go?"
confirm_scrap        "Break it down?"
confirm_abandon      "Leave it behind?"
confirm_attack       "Go in?"
confirm_ration       "Cut the shares?"
confirm_retire       "End this run?"
```

## §XIX.5 Close/back labels

```text
ui_close             "Close"
ui_back              "Back"
ui_cancel            "Cancel"
ui_confirm           "Confirm"
ui_leave             "Leave"
```

## §XIX.6 HUD labels and units

```text
hud_rads             "Rads"
hud_temp             "Shelter warmth"
hud_water            "Water"
hud_food             "Food"
hud_power            "Power"
hud_day              "Day"
hud_alert            "Attention"
unit_per_hour        "/hr"
unit_days            "days"
unit_band            "—" (banded displays)
```

## §XIX.7 Feedback classes

```text
kind_error   "problem"
kind_warning "trouble ahead"
kind_info    "noted"
kind_success "done"
```

## §XIX.8 Display labels for actions

```text
act_confirm     "Confirm"
act_cancel      "Cancel"
act_close       "Close"
act_scroll      "Scroll"
act_zoom_in     "Zoom in"
act_zoom_out    "Zoom out"
act_map_pan     "Pan map"
act_journal     "Journal"
act_inventory   "Inventory"
act_character   "People"
act_shelter     "Shelter"
act_map         "Map"
act_craft       "Make"
act_trade       "Trade"
act_rest        "Rest"
act_assign      "Assign"
act_focus_next  "Next"
act_focus_prev  "Previous"
```

## §XIX.9 The register rules

```text
[ ] every string registered before use
[ ] no raw exceptions or ids in player text
[ ] units/context on all numeric displays
[ ] no real-world phrasing (fictional register)
[ ] freeze routing when declared (UNBLOCK-03)
[ ] removed strings removed from the register
```

---

*End of Part XIX. Continues in Part XX (cross-plan checklists).*# W3-06 · PART XX — CROSS-PLAN INTERFACE CHECKLISTS

---

## §XX.1 With W2-02 (resilience/lifecycle)

```text
[ ] shared PanelState model used (no divergent handling)
[ ] lifecycle volume kit extended, not duplicated
[ ] hygiene gate cycle assertions owned jointly
[ ] UI failure-path repairs attributed (W2-02) vs. coverage (W3-06)
[ ] evidence cross-referenced (one repair, one record)
```

## §XX.2 With W3-01 (narrative)

```text
[ ] journal surface rows: entries owner-sourced; oracle displays
[ ] radio/echo surfaces: freshness presentation
[ ] memorial surface: names from the registry
[ ] choice scenes render owner states, never compute
[ ] no narrative strings hardcoded in panels
```

## §XX.3 With W3-02 (economy)

```text
[ ] trade screen source table: quote/factors/supply/shocks/embargoes
[ ] ration screen: policy/ratios/outlook from owners
[ ] re-quote notice presented per feedback rules
[ ] HUD economy elements (stock/prices) registered
```

## §XX.4 With W3-03 (psychology)

```text
[ ] needs/care surfaces owner-sourced (bands)
[ ] relationship surface: history/visibility rules
[ ] caregiver strain displayed via needs surface bands
[ ] dignity: no meters-as-people presentations (review)
```

## §XX.5 With W3-04 (security)

```text
[ ] readiness surface: layer states/inputs/coverage derived
[ ] heat thresholds presented with warnings (scenes own the copy)
[ ] ranging observations: confidence bands, aggregation
[ ] alert presentation through the crisis path
```

## §XX.6 With W3-05 (crafting)

```text
[ ] craft surface: readiness/requirements/costs/output (owner reads)
[ ] block messages per requirement kind (register §XIX.3)
[ ] chain board: read-only model rendering
[ ] no UI arithmetic on costs/yields
```

## §XX.7 With W2-05 (locations/snapshots)

```text
[ ] snapshot rows per surface × state (C5)
[ ] map/discovery surfaces: marker sources
[ ] location detail panels route/lifecycle covered
```

## §XX.8 With W2-03 (balance measurement)

```text
[ ] HUD pressure displays reflect owner reads (no tuning here)
[ ] alert thresholds presentation owned by W2-03 bands
[ ] measurement handoff documented
```

## §XX.9 With W2-06 (prose) and UNBLOCK-03 (freeze)

```text
[ ] all new strings in the register
[ ] empty/feedback copy authored by the corpus workflow
[ ] freeze routing when declared
[ ] no prose in code
```

## §XX.10 With W2-01 (maintenance)

```text
[ ] generated gates (UI checks) join the focused pipeline
[ ] registries regenerated with --check
[ ] findings flow into the shared maintenance review
```

---

## §XX.11 The interface-first principle

Every sibling plan produces read models; this plan owns their presentation.
No sibling writes display state, and this plan never invents data: the
boundary is clean and must stay so. If a surface needs a value no owner
provides, that is a finding — the surface does not approximate.

---

*End of Part XX. Continues in Part XXI (artifact and evidence index).*# W3-06 · PART XXI — ARTIFACT, EVIDENCE, AND GATE INDEX

---

## §XXI.1 Artifact index

| Artifact | Type | Owner point |
|---|---|---|
| `docs/ui/ROUTE_MATRIX.md` | generated | P1 |
| `docs/ui/FOCUS_GRAPH.md` | generated | P2 (C1 full) |
| `docs/ui/INPUT_ACTIONS.md` | generated | P3 |
| `docs/ui/MODAL_TYPES.md` | authored | P4 |
| `docs/ui/LIFECYCLE_REPORT.md` | generated | P5 |
| `docs/ui/HUD_BINDINGS.md` | generated | P6 |
| `docs/ui/A11Y_LANES.md` | generated | P7 |
| `docs/ui/IA_INVENTORY.md` | generated | P8 |
| `docs/ui/IA_PROPOSALS.md` | authored | P8 review |
| `docs/ui/FEEDBACK_CENSUS.md` | generated | P9 |
| `docs/ui/NAV_DEADENDS.md` | generated | P10 |
| `docs/ui/SURFACE_SOURCES.md` | authored | all |
| `docs/ui/STRING_REGISTER.md` | register | all |
| `docs/evidence/w3-06/*` | evidence | all |

## §XXI.2 The gate index

```text
G1  route matrix green (extended parity gate)
G2  focus registry complete
G3  action table parity/conflicts
G4  modal discipline tests
G5  lifecycle counts stable (hygiene extended)
G6  HUD binding registry + updates
G7  a11y lanes + sealed guard
G8  feedback canonical/silence scan
G9  IA inventory current
G10 navigation dead-end list (empty or signed)
```

## §XXI.3 Evidence expectations

```text
T1: registry/table generation outputs + diffs
T2: kit reports per point
T3: soak summary (cycles, counts, memory, controller pass)
findings: UI-### with evidence
repairs: before/after (matrix cells or captures)
```

## §XXI.4 The evidence minimum

```text
finding id, class, state, expected, observed, owner, repair, command, HEAD,
reviewer
```

## §XXI.5 The re-audit triggers

```text
[ ] any new surface (expansion)
[ ] any new action or modal class
[ ] any HUD element change
[ ] any a11y fix (lane rerun)
[ ] any lifecycle-related repair (W2-02 coordination)
[ ] any sealed-guard incident (escalation review)
```

---

*End of Part XXI. Continues in Part XXII (Q&A fourth set and close).*# W3-06 · PART XXII — Q&A FOURTH SET AND THE CONTRIBUTOR'S DRUMBEAT

---

## §XXII.1 Q&A (Q81–Q100)

**Q81. What is the fastest check when a new panel misbehaves?**
Its matrix row: open/close in five states. Four times out of five the failure
is a state-specific route/bind issue.

**Q82. What is the fastest check for "slow UI"?**
Binding reads per tick; if a panel reads owner state in a loop, that's the
finding (cache with contract or event-update).

**Q83. How do we know a panel is "accessible enough"?**
The lane passes in all six states; no exceptions. There is no "enough" —
either floors hold or the panel is returned.

**Q84. What if two surfaces show the same data?**
Fine; duplication of *presentation* is allowed. Duplication of *state* is a
finding (owners only).

**Q85. How do we handle panels for unfinished features?**
Unavailable state with authored copy; no fabricated content (production purity
rule).

**Q86. What about developer-only surfaces?**
Excluded from the live manifest with a visible note; they never ship enabled.

**Q87. What is the death penalty for a sealed change?**
Revert + escalation + review; the seal trains the discipline that
accessibility is not negotiable mid-sprint.

**Q88. How do panels signal "loading"?**
Authored loading states; never blank and never fabricated values.

**Q89. Can a surface choose to ignore focus?**
No; every interactive control participates. Passive displays need no focus.

**Q90. How are gamepad dead zones handled?**
Existing input owner; profiles (C6) are signed additions.

**Q91. Whats the difference between unavailable and disabled?**
Unavailable = data/owner absent (authored marker). Disabled = data present,
action not permitted (reason shown via feedback).

**Q92. How do we test that focus is visible?**
The a11y lane includes a focus-visual assertion per control.

**Q93. What stops modal fatigue?**
Inline feedback for routine failures; modals only for authored decisions;
depth limit + dedupe.

**Q94. How do surfaces share layout?**
Through authored layout patterns (list/table/map) with focus policy
profiles — no copy-paste per panel.

**Q95. What about panels with live-updating lists?**
Sorted keys + stable focus by identity (never index); the focus kit asserts.

**Q96. How is IA duplication resolved?**
Proposal to the owning feature plan; this plan never moves a surface
unilaterally.

**Q97. What is the UI contribution to determinism?**
None directly; but focus/order stability avoids nondeterministic display;
owner reads keep values deterministic.

**Q98. How do we keep the matrix from growing forever?**
Automation: cells are generated from the manifest; human effort is only on
red cells.

**Q99. What is the single most common UI bug in practice?**
Focus loss on close (trivial to fix, invisible until tested).

**Q100. The one sentence again?**
Every press lands, every number means, every state can be left.

---

## §XXII.2 The contributor's drumbeat

```text
open it in every state
close it from every state
tab to everything
try every device
watch the counts
read the values
check the words
keep the seal
```

---

## §XXII.3 The final acceptance cadence

```text
daily: matrix spot-run on changed surfaces
weekly: parity + lane samples on changed areas
per release: full matrix + parity + lanes + controller pass + soak
```

---

*End of Part XXII. Continues in Part XXIII (closing appendices and control).*# W3-06 · PART XXIII — CLOSING APPENDICES: METRICS, ROLLBACK, AND HANDOFF CLOSE

---

## §XXIII.1 Metric definitions (final)

| Metric | Definition | Target |
|---|---|---|
| route matrix red cells | failing cells across surfaces × states | 0 |
| instance violations | surfaces with >1 instance | 0 |
| focus loss events | closes without restored focus | 0 |
| unregistered controls | interactive controls missing from the registry | 0 |
| parity gaps | actions missing applicable device paths | 0 (or authored list) |
| modal discipline violations | order/depth/identity/trap failures | 0 |
| subscription drift | handler count delta over cycles | 0 |
| HUD truth violations | displayed ≠ owner or fabricated values | 0 |
| a11y lane failures | floor violations per surface × state | 0 |
| silent failures | failure scenarios without feedback | 0 |
| sealed guard events | constant diffs attempted | 0 |
| controller dead ends | core flows not completable | 0 |

## §XXIII.2 Rollback rules

```text
[ ] every repair is local (route guard, binding, layout) with before/after
[ ] no repair touches sealed constants; a repair that would is redesigned
[ ] panel changes are code-only (no save/settings shape changes)
[ ] C additions are signed individually and can be disabled independently
[ ] evidence retained per repair; reverting restores its lines
```

## §XXIII.3 Handoff close

```text
to W2-01: the generated UI checks join the focused pipeline
to W2-02: shared lifecycle results + state model
to every sibling: their surfaces' source tables are filed
to W2-06: the string register is filed for corpus authorship
to the foreman: signatures, releases, and open items
```

## §XXIII.4 The definition of done (final)

```text
[ ] §XXI.2 gates G1–G10 green
[ ] metrics table all targets met
[ ] evidence pack filed
[ ] closeout memo written
[ ] Annex U releases recorded
[ ] signed additions recorded or deferred
[ ] no stop-class open
```

## §XXIII.5 The closeout memo template

```text
OUTCOME:
FILES:
CONTRACT: routes; focus; parity; modals; lifecycle; HUD truth; floors;
  feedback; navigation
COMMANDS: T1/T2/T3 + results
LIMITATIONS:
SHARED PATHS TOUCHED:
LEDGER PROPOSALS:
ANNEX U RELEASES EARNED:
```

---

*End of Part XXIII. Continues in Part XXIV (final control).*# W3-06 · PART XXIV — FINAL CONTROL AND COMPLETION STATEMENT

---

## §XXIV.1 The completion statement

```text
W3-06 is complete when:
  the route matrix is green in all states,
  focus participates and restores everywhere,
  every action is declared with parity and display,
  modals obey order/depth/identity/trap and clear on state change,
  panels are leak-free at volume and rebind on session change,
  HUD values are owner reads with units and honest unavailable states,
  the accessibility floors hold across all surfaces and six states with the
  sealed guard intact,
  feedback is canonical, deduped, and never silent,
  and no core flow requires a mouse or a hover.

Path C adds only signed items:
  remap depth, audio description, cognitive options, controller profiles,
  snapshot gating, full navigation model, feedback taxonomy.
```

## §XXIV.2 The reading card (final form)

```text
WHAT:    every press lands; every number means; every state can be left.
WHY:     the interface is the contract.
HOW:     ten points; ten kits; three tiers.
STOPS:   traps; silence; lies; seal breaks.
SIGN:    Annex U + §VIII.5.
```

## §XXIV.3 The interface oath

```text
I checked every state a player can enter.
I checked every press a player can make.
I checked every number a player can read.
I checked every device a player can use.
Where the interface failed, I returned it rather than excuse it.
Signed: ________  Date: ______
```

## §XXIV.4 Version record (final)

| Version | Change |
|---|---|
| v2.8 | Part XVIII accessibility handbook |
| v2.9 | Part XIX copy/feedback register |
| v3.0 | Part XX cross-plan checklists |
| v3.1 | Part XXI artifact/gate index |
| v3.2 | Part XXII Q&A fourth set + drumbeat |
| v3.3 | Part XXIII metrics/rollback/handoff |
| v3.4 | Part XXIV this final control |

## §XXIV.5 Final control (complete)

**W3-06 is complete.** Parts I–XXIV. Proposal only; no execution without
Annex U (Part I §U.2) and §VIII.5 signatures. Binding within this document:
the never-cross list (§VIII.2.1), the ten sentences (§XVII.2), the sealed
guard, and the interface oath.

> The interface is where the player and the world meet. Keep the meeting
> honest: nothing silent, nothing trapped, nothing false, no one left out.

*Document control: W3-06 · Wave 3 (expanded, final) · HEAD 5be1a30a ·
end of W3-06.*

---

# W3-06 · PART XXV — THE TEST SCENARIO BOOK

> Scripted walkthroughs for the ten kits — step sequences and assertion
> lines. The verifier runs these as-is.

## XXV.1 Route scenario

```text
setup: fresh -> loaded -> post-switch -> post-reset
for each surface S:
  enter state; open S; assert single instance + healthy binds
  close S; assert closed + focus restored
  open S again while S active; assert focus existing (no duplicate)
assertions: matrix green; no red cells
```

## XXV.2 Focus scenario

```text
open surface; traverse focus from entry to every control (count == registry)
open dynamic list; open/close ×5; assert order stable by key
open modal; attempt focus escape; assert trapped
close modal; assert prior focus restored
```

## XXV.3 Input scenario

```text
for action A in table:
  fire keyboard binding; assert event
  fire gamepad binding; assert event
  fire mouse path (if applicable); assert event
  enter modal; fire gameplay-only action; assert no event
  enter menu; fire ui_confirm; assert event
assertions: parity complete; contexts hold
```

## XXV.4 Modal scenario

```text
open A; open B; escape -> B closed; escape -> A closed
push identical modal twice; assert single
push beyond depth; assert finding logged, no push
load a save with a modal scripted open; assert post-load clean
```

## XXV.5 Lifecycle scenario

```text
for panel P:
  open/close ×100; assert subscription counts stable
  slot switch with P open; assert rebind to new session
  reset with P open; assert disposed
  assert timers/animations stopped
```

## XXV.6 HUD scenario

```text
for element E:
  change owner value; advance tick; assert display follows
  make owner unavailable; assert authored marker (not zero)
  enter combat+weather stacked state; assert no collision
```

## XXV.7 Readability scenario

```text
for surface × 6 states:
  run lane checks (contrast/overflow/color-only/focus)
  feed longest authored strings
assertions: floors green; sealed guard untouched
```

## XXV.8 IA scenario

```text
generate concern inventory; assert coverage lists; assert proposals are
review-only (no moves executed)
```

## XXV.9 Feedback scenario

```text
trigger each failure class (blocked craft, denied route, empty action)
assert: one canonical message; no exception text; no silence
repeat rapidly; assert dedupe window
```

## XXV.10 Navigation scenario

```text
walk core flows keyboard-only; assert completion
walk core flows controller-only; assert completion
scan for hover-only critical info; assert none (or listed)
```

## XXV.11 Coverage matrix

| Scenario | Points |
|---|---|
| route | P1, P5 |
| focus | P2 |
| input | P3 |
| modal | P4 |
| lifecycle | P5 |
| HUD | P6 |
| readability | P7 |
| IA | P8 |
| feedback | P9 |
| navigation | P10 |

*End of Part XXV. Continues in Part XXVI (runbook card).*

---

# W3-06 · PART XXVI — THE RUNBOOK CARD

> The condensed execution card — one page per point, commands and exits.

## XXVI.1 P0 — baseline

```text
run existing gates; record results
generate the manifest surface list; snapshot counts
inventory modals/actions/HUDs; write the premise doc
exit: premise filed; no stale assumptions
```

## XXVI.2 P1 — routes

```text
build matrix driver; baseline run; repair red cells; extend gate
exit: matrix green
```

## XXVI.3 P2 — focus

```text
coverage audit; register controls; stabilize ordering; restore-on-close
exit: traversal + restore green
```

## XXVI.4 P3 — input

```text
generate action table; fix conflicts/parity/contexts; display refs
exit: parity kit green
```

## XXVI.5 P4 — modals

```text
close paths; escape order; depth/identity; trap; load clean
exit: modal kit green
```

## XXVI.6 P5 — lifecycle

```text
×100 harness; fix leaks/binds/timers; extend hygiene; align state model
exit: cycle assertions green
```

## XXVI.7 P6 — HUD

```text
binding registry; fix fabrication/staleness; units/unavailable; priority
exit: update kit green
```

## XXVI.8 P7 — readability

```text
lane extension; fixes (layout only); gate wiring; seal check
exit: lanes green; guard intact
```

## XXVI.9 P8 — IA

```text
inventory; duplication/orphans; proposals
exit: inventory filed
```

## XXVI.10 P9 — feedback

```text
census; canonical routing; text refs; dedupe; silence scan
exit: feedback kit green
```

## XXVI.11 P10 — navigation

```text
dead-end inventory; repairs; controller pass; signed additions list
exit: dead ends removed or signed
```

## XXVI.12 Soak + closeout

```text
soak: cycles/counts/memory/controller
closeout: evidence pack; memo; Annex U; debt rows
exit: completion meter full
```

## XXVI.13 The command set (P0-verified names)

```text
bash scripts/run_test.sh Ashfall.Core.Tests/UI
godot --headless --path . -- --ui-a11y-selftest
godot --headless --path . -- --player-panels-uitest
godot --headless --path . -- --panel-bind-lifecycle-selftest
godot --headless --path . -- --settings-selftest
```

*End of Part XXVI. Continues in Part XXVII (report and evidence formats).*

---

# W3-06 · PART XXVII — REPORT AND EVIDENCE FORMATS

## XXVII.1 The route matrix report

```yaml
run: route-matrix-<date>
surfaces: 52
states: [fresh, loaded, post-switch, post-reset, mid-panel]
cells: 260
failures: 0
notes: [UI-01 fixed: single-instance]
```

## XXVII.2 The focus report

```yaml
surface: craft_desk
controls: 14
registered: 14
reachable: 14
restore: pass
order: stable (keys)
```

## XXVII.3 The parity report

```yaml
actions: 47
keyboard: 47
gamepad: 47
mouse: 41 (6 authored keyboard/gamepad-only)
conflicts: 0
display_refs: 47
```

## XXVII.4 The modal report

```yaml
types: 12
close_paths: 12/12
escape_order: pass
depth_limit: 3 (0 violations)
duplicates: 0
load_clean: pass
```

## XXVII.5 The lifecycle report

```yaml
panels: 52
cycles: 100
subscription_drift: 0
stale_binds: 0
timers_leaked: 0
reset_dispose: pass
```

## XXVII.6 The HUD report

```yaml
elements: 38
registry_backed: 38
update_within_tick: 38/38
units: 38/38
unavailable_authored: 38/38
collisions: 0
```

## XXVII.7 The a11y lane report

```yaml
surfaces: 52
states: 6
checks: 312
failures: 0
sealed_guard: pass
long_content: sampled 52
```

## XXVII.8 The feedback report

```yaml
messages: 94
canonical: 94
exception_text: 0
silent_failures: 0
dedupe_violations: 0
```

## XXVII.9 The navigation report

```yaml
mouse_only_flows: 0
hover_only_critical: 0
controller_pass: 9/9 core flows
signed_additions: [list]
```

## XXVII.10 Evidence file anatomy

```yaml
run: <id>  date: <date>  head: <sha>
tier: T1|T2|T3
section: <point>
results: {…}
artifacts: [paths]
reviewer: <name>
```

## XXVII.11 The closeout pack contents

```text
index.md
T1-*.yaml  T2-*.yaml  T3-*.yaml
findings/UI-*.md
repairs/UI-*.md
baselines/
```

*End of Part XXVII. Continues in Part XXVIII (standing repair queue).*

---

# W3-06 · PART XXVIII — THE STANDING REPAIR QUEUE (WORKLIST)

> The expected first-pass findings, ranked — the worklist a team walks
> through in the first two phases. Seeded from the matrix/parity/lane
> patterns; actual findings recorded per run.

## XXVIII.1 Route/focus queue (est. 8-12)

```text
1  single-instance guards on the three largest panels
2  stale-bind hooks on session change (all bound panels)
3  restore-on-close sweep (all panels)
4  unavailable states for pre-bind opens
5  dynamic list ordering (rosters, journals, craft recipes)
6  focus visual regression pass
7  modal focus trap verification
8  close-path coverage in post-reset state
```

## XXVIII.2 Input queue (est. 5-10)

```text
1  action table generation and reconciliation
2  mouse-only paths for scroll/pan
3  gamepad paths for list/scroll
4  conflict precedence authoring
5  context gating for modal bindings
6  display strings for all actions
```

## XXVIII.3 Lifecycle queue (est. 5-8)

```text
1  ×100 harness adoption; leak fixes
2  slot-switch rebind verification
3  reset dispose audit
4  timer/tween cleanup
5  hygiene gate cycle assertions
```

## XXVIII.4 HUD queue (est. 4-8)

```text
1  binding registry; element orphan fixes
2  fabrication/staleness repairs
3  units/context additions
4  unavailable states
5  priority modes authoring
6  stacked-state collision fixes
```

## XXVIII.5 Readability queue (est. 5-10)

```text
1  lane extension across all surfaces
2  disabled/alert state fixes
3  overflow policy application
4  color-independence fixes
5  long-content sampling fixes
6  focus visibility sweep
```

## XXVIII.6 Feedback queue (est. 3-6)

```text
1  census and canonical routing
2  exception-text elimination
3  silence scan repairs
4  dedupe windows
5  inline-vs-modal corrections
```

## XXVIII.7 Navigation queue (est. 2-5)

```text
1  mouse-only flow inventory
2  hover-only critical inventory
3  keyboard/controller alternatives
4  controller pass fixes
```

## XXVIII.8 The queue discipline

```text
- ranked by stop-class first
- cap 20 repairs per phase
- overflow becomes debt rows (owner, expiry)
- every repair: before/after evidence
- no queue item closes without its test
```

*End of Part XXVIII. Continues in Part XXIX (generated reports register).*

---

# W3-06 · PART XXIX — GENERATED REPORTS AND GATE WIRING

## XXIX.1 The generated reports register

| Report | Generator | Check mode |
|---|---|---|
| route matrix | matrix driver | rerun + diff |
| focus graph | manifest + policies | coverage check |
| action table | declarations | parity/conflict check |
| HUD bindings | element declarations | source-row check |
| IA inventory | surface names + owners | coverage check |
| feedback census | message registration | canonical check |
| string register | registration | freeze/refs check |

## XXIX.2 Gate wiring (focused pipeline)

```text
trigger: UI surface files changed
steps:
  1. regenerate registries; --check diffs
  2. run U1 static checks
  3. run changed-surface matrix rows
  4. run parity for changed actions
  5. run lane sample for changed surfaces
  6. sealed guard
exit: pass/fail with per-row messages
```

Runtime budget: static steps seconds; matrix rows seconds-to-minutes;
lanes sampled.

## XXIX.3 Failure message format

```text
UI GATE [route.matrix]
  surface: shelter_roster
  state: post-switch
  open: ok  binds: STALE  close: ok
  hint: rebind on session change; see P5 hook
```

## XXIX.4 The ratchet

```text
counted baselines: a11y lane findings, parity gaps (authored exceptions)
may not grow; each repair lowers them
zero-tolerance: route reds, sealed diffs, silence, traps (never baselined)
```

## XXIX.5 The weekly report

```text
- changed surfaces and their matrix rows
- new failures (should be zero) and ratchet deltas
- repair queue moves
- sealed guard status
```

## XXIX.6 The release report

```text
- full matrix (consolidated)
- full parity table
- full lane results
- controller pass
- soak summary
- completion meter state
```

*End of Part XXIX. Continues in Part XXX (final scenario pack).*

---

# W3-06 · PART XXX — FINAL SCENARIO PACK AND CLOSING

## XXX.1 Scenario: the new expansion panel

```text
a foundry expansion adds the glassworks management panel
walkthrough:
  manifest entry + route; matrix row green (all states)
  focus graph (stations, recipes, actions, tabs)
  binds owner-sourced; lifecycle ×100 clean
  HUD elements (heat, output) registered and truthful
  lane states pass; feedback canonical
  actions added to the parity table
result: the panel enters the standing gates; no bespoke exceptions
```

## XXX.2 Scenario: the slot-switch catch

```text
player loads a different save while the trade desk is open
expected:
  modal/panel clears or rebinds through the state-change hook
  stale binds never resolve to the old session
  focus lands on a defined target; no ghost panel
finding classes caught: UI-02, UI-05, UI-24
```

## XXX.3 Scenario: the disabled-state audit

```text
an emergency disables most shelter actions
expected:
  disabled controls remain focusable-skip consistent
  contrast floors hold in disabled state (UI-31 class)
  status is not color-only (UI-32 class)
  feedback explains why (fb_generic_error/authored)
```

## XXX.4 Scenario: the long night session

```text
8 hours of accelerated cycling
expected:
  subscription counts stable; memory flat
  no focus loss; no stuck modal
  alerts deduped; no spam
result: soak green; the UI is invisible in the best sense
```

## XXX.5 Scenario: the accessibility pass

```text
a reviewer plays keyboard-only with colorblind mode and minimum font
expected:
  every core flow completable; no hover-only blocks
  all status readable; no traps
  the experience is the game, not a workaround
```

## XXX.6 The closing statement

```text
The interface is the only part of the game every player touches. It should
be the most trustworthy: never silent, never trapping, never lying, never
excluding. W3-06's entire content exists to keep those four negatives true.
```

## XXX.7 Final version record

| Version | Change |
|---|---|
| v3.5 | Part XXV test scenario book |
| v3.6 | Part XXVI runbook card |
| v3.7 | Part XXVII report/evidence formats |
| v3.8 | Part XXVIII standing repair queue |
| v3.9 | Part XXIX generated reports/gates |
| v4.0 | Part XXX this closing pack |

*End of Part XXX. Continues in Part XXXI (final control).*

---

# W3-06 · PART XXXI — FINAL CONTROL, METRICS CLOSE, AND END OF DOCUMENT

## XXXI.1 The final completion meter

```text
[ ] route matrix green across all states
[ ] single-instance verified
[ ] focus complete + restore
[ ] action parity + display
[ ] modal discipline complete
[ ] lifecycle ×100 clean
[ ] HUD truth complete
[ ] accessibility floors across surfaces × states, seal intact
[ ] feedback canonical, silent-free
[ ] navigation dead ends removed (or signed)
[ ] soak summary recorded
[ ] evidence pack filed
```

## XXXI.2 The final metric run (expected shape)

```yaml
run: T3-final
surfaces: 52
states: 5
route_cells: 260
route_failures: 0
focus_coverage: 100%
action_parity_gaps: 0
modal_violations: 0
lifecycle_drift: 0
hud_truth_violations: 0
a11y_lane_failures: 0
sealed_guard: pass
feedback_silence: 0
controller_dead_ends: 0
```

## XXXI.3 The closing acceptance

```text
The plan closes when the meter above is full and the closeout memo is filed.
The plan's guarantees outlive it:
  every new surface joins the matrix automatically;
  every new action joins the parity table;
  every changed surface re-enters the lane;
  the seal is guarded forever.
```

## XXXI.4 The final control statement

**W3-06 is complete.** Parts I–XXXI. Proposal only; no execution without
Annex U (Part I §U.2) and §VIII.5 signatures. Binding within this document:
the never-cross list (§VIII.2.1), the ten sentences (§XVII.2), the sealed
guard (§XVIII.2), and the interface oath (§XXIV.3).

> Every press lands. Every number means. Every state can be left. Every
> player is included. That is the interface; anything less is a bug.

*Document control: W3-06 · Wave 3 (expanded, final) · HEAD 5be1a30a ·
end of W3-06.*

---

# W3-06 · PART XXXII — THE QUICK-FIX COOKBOOK

> Concrete repair recipes for the most common findings — mechanism sketches,
> not production code.

## XXXII.1 Double-open

```text
finding: route opens a second instance
fix: at route entry, if surface active -> focus() and return
test: repeated open; instance count stays 1
```

## XXXII.2 Stale bind after slot switch

```text
finding: panel bound to the prior session
fix: session-change hook -> rebind or show Unavailable; never keep old refs
test: slot switch with panel open; owner reads resolve to the new session
```

## XXXII.3 Focus lost on close

```text
finding: focus nowhere after close
fix: rememberFocus on open; restoreFocus on close (fallback: entry node)
test: identity before/after
```

## XXXII.4 Unregistered control

```text
finding: control absent from the navigator
fix: register on bind, unregister on dispose (symmetry)
test: coverage count equals interactive count
```

## XXXII.5 Mouse-only action

```text
finding: no keyboard/gamepad path
fix: add bindings in the action table; context-gate them
test: parity kit
```

## XXXII.6 Conflict

```text
finding: two actions on one chord with undefined order
fix: authored precedence in the action table
test: conflict check + context exercise
```

## XXXII.7 Stuck modal

```text
finding: no close path in a state
fix: declare close paths (cancel/escape/confirm per class)
test: escape order + load clearing
```

## XXXII.8 Subscription leak

```text
finding: handlers grow per open
fix: symmetric unsubscribe in dispose; verify no double-subscribe
test: ×100 count stability
```

## XXXII.9 HUD fabrication

```text
finding: zero shown when data missing
fix: suppress or show the authored marker; never synthesize
test: unavailable-state assertion
```

## XXXII.10 Silent failure

```text
finding: action fails with no feedback
fix: route a canonical message with a text ref
test: failure-scenario silence scan
```

## XXXII.11 Contrast in disabled state

```text
finding: disabled text below floor
fix: choose a brighter sealed color / state styling (no constant change)
test: lane disabled state
```

## XXXII.12 Color-only status

```text
finding: severity by hue alone
fix: add icon/label; keep the color as emphasis
test: color-independence check
```

*End of Part XXXII. Continues in Part XXXIII (agent maintenance playbook).*

---

# W3-06 · PART XXXIII — THE AGENT MAINTENANCE PLAYBOOK

> How a maintenance agent (or any contributor) handles UI work without
> breaking the contract.

## XXXIII.1 The agent's rule set

```text
R1  never edit sealed constants (contrast/colorblind/font set)
R2  never add a second focus/modal/route authority
R3  never compute display values; render owner reads
R4  never leave a failure without feedback
R5  never add an action outside the table
R6  never change a save/settings shape in this scope
R7  run the focused UI checks before proposing a change
R8  file findings with owner + test + evidence
```

## XXXIII.2 The agent's workflow

```text
1. identify the surface/action/element in the registries
2. reproduce the failure in the relevant kit/matrix cell
3. apply the smallest fix from the cookbook (Part XXXII)
4. run the focused check; attach before/after
5. update the registry if the surface changed
6. hand off with the evidence template
```

## XXXIII.3 The evidence template

```text
CHANGE: <surface/action/element>
FINDING: UI-###  (or "new")
KIT: <which check>  BEFORE: <result>  AFTER: <result>
SEALED: untouched [x]
REGISTRY: updated [x]
HEAD: <sha>
```

## XXXIII.4 What an agent must never do

```text
- silence a failing check by weakening it
- baseline a zero-tolerance class (route reds, traps, silence)
- "temporarily" remove a warning or a close path
- add hover-only critical info to save space
- patch around an owner read with a local cache
- mark a surface a11y-exempt
```

## XXXIII.5 The weekly agent report

```text
- surfaces touched and their matrix results
- findings filed/fixed with kit coverage
- registry diffs
- seal status
- anything returned to humans (IA proposals, theme escalations)
```

## XXXIII.6 The end state the agent protects

```text
A player, any player, on any of the intended devices, can open anything,
read anything, do anything, and leave anything — every time, in every state.
```

*End of Part XXXIII. Continues in Part XXXIV (cross-reference index).*

---

# W3-06 · PART XXXIV — CROSS-REFERENCE INDEX AND GLOSSARY CLOSE

## XXXIV.1 Cross-reference index

| Topic | Part |
|---|---|
| route matrix / single instance | II.1, XXV.1 |
| focus coverage / restore | II.2, XXV.2 |
| input parity / contexts | II.3, XXV.3 |
| modal discipline | II.4, XXV.4 |
| lifecycle volume | II.5, XXV.5 |
| HUD truth | III.1, XXV.6 |
| readability floors / seal | III.2, XVIII |
| IA inventory | III.3 |
| feedback canonical | III.4, XXV.9 |
| navigation model | III.5, XXV.10 |
| kits / reports | V, XXVII |
| worked threads | VI |
| Q&A (100) | VII, XVI, XXII |
| C-path designs | VIII.1 |
| checklists | XV |
| runbook | X, XXVI |
| repair queue | XXVIII |
| cookbook | XXXII |
| agent playbook | XXXIII |
| strings register | XIX |
| closeout | XXIII–XXIV, XXXI |

## XXXIV.2 Glossary close (new terms)

| Term | Definition |
|---|---|
| answer key | reviewer's drill responses |
| baseline (a11y/parity) | counted findings allowed to remain, never grow |
| cookbook | concrete repair recipes |
| drumbeat | the daily contributor checks |
| frozen lane | a11y sampling with the longest strings |
| matrix cell | surface × state result |
| parity table | action × device-path map |
| register (string) | copy inventory for corpus handoff |
| seal duty | the obligation never to edit sealed constants |
| zero-tolerance class | findings that may never be baselined |

## XXXIV.3 The cross-plan one-liner

```text
W2-02 repairs resilience; W3-06 proves coverage —
same kit, same state model, one record per repair.
```

*End of Part XXXIV. Continues in Part XXXV (final Q&A band).*

---

# W3-06 · PART XXXV — FINAL Q&A BAND AND TRAPS

## XXXV.1 Q&A (Q101–Q120)

**Q101. What is the first thing fixed when a UI bug is reported?**
Reproduce it in a matrix cell; the cell tells you the state and the class.

**Q102. What if the bug only happens once in twenty sessions?**
The soak finds it (cycle/volume); add the reproducing sequence to the kit
once found.

**Q103. How do we prioritize UI vs. feature work?**
Stop-class UI findings block; the rest join the ranked queue.

**Q104. What is the UI equivalent of "silent failure"?**
A press that does nothing with no message — the worst class, always.

**Q105. What about panels that are slow to open?**
Bind cost/read count; consolidate reads with invalidation contracts; the
tune plan owns profiling.

**Q106. How do we keep the matrix honest when surfaces are renamed?**
The manifest is the single source; renames regenerate rows; the coverage
gate fails on drift.

**Q107. Can an exception ever reach the player?**
No. Feedback carries text refs only; the gate scans for exception strings.

**Q108. What if a control is genuinely non-focusable (e.g., a chart)?**
It is display, not a control; charts provide text summaries (floors).

**Q109. How are animated elements handled?**
Authored motion with reduced-motion respect if the setting exists (sealed);
otherwise static alternatives for critical info.

**Q110. Is vibration/haptics required?**
No; optional and signed if added.

**Q111. How do we audit localized lengths before localization exists?**
Authored maxima + the long lane; real maxima arrive with localization.

**Q112. Can panels read each other?**
No; they read owners. The "panel bus" is the owner change surface.

**Q113. What is the reviewer's veto?**
Stop-class findings and the seal; both are non-negotiable.

**Q114. How is progress against the queue tracked?**
Registry deltas + repair evidence; no morale-based "mostly done".

**Q115. What does "done" mean for a single surface?**
Its full checklist (Part XV.1) green.

**Q116. What does "done" mean for the plan?**
The completion meter (XXXI.1) full and the memo filed.

**Q117. What if a new expansion adds a new input class?**
It enters the action table; parity and contexts apply; no raw input.

**Q118. What if a modal is needed during loading?**
Don't; loading states are displays. Load-time modal clearing is the rule.

**Q119. What is the one place never to compromise?**
The seal and the no-silence rule. Everything else is queue.

**Q120. The sentence?**
Every press lands, every number means, every state can be left.

## XXXV.2 The trap list

```text
T1  "temporarily" removing a close path
T2  baselining a trap or silence
T3  hover-only critical info "for space"
T4  local caches patching owner reads
T5  a11y-exempt surfaces
T6  theme edits "just this once"
T7  second modal stacks
T8  index-based focus on live lists
T9  exception strings in "temporary" messages
T10 registry edits without regeneration
```

*End of Part XXXV. Continues in Part XXXVI (final control).*

---

# W3-06 · PART XXXVI — THE FINAL CONTROL AND END OF DOCUMENT

## XXXVI.1 The final acceptance (restated once, short)

```text
[ ] routes: every surface, every state, single instance, closed cleanly
[ ] focus: every control, visible, restored
[ ] input: every action, every device, every context
[ ] modals: ordered, bounded, trapped, clearable
[ ] lifecycle: leak-free at volume, rebind on change
[ ] HUD: truthful reads, units, honest unavailable
[ ] floors: all surfaces, all states, seal intact
[ ] feedback: canonical, deduped, silent-free
[ ] navigation: no mouse/hover dead ends
```

## XXXVI.2 The completion declaration

```text
When the list above is green, the interface has kept its promises:
nothing silent, nothing trapped, nothing false, no one left out.
The plan is closed; the gates remain.
```

## XXXVI.3 The permanent guarantees

```text
1. new surfaces join the matrix automatically
2. new actions join the parity table
3. changed surfaces re-enter the lane
4. the seal is guarded forever
5. stop-class findings block releases
```

## XXXVI.4 The final version record

| Version | Change |
|---|---|
| v4.1 | Part XXXII quick-fix cookbook |
| v4.2 | Part XXXIII agent maintenance playbook |
| v4.3 | Part XXXIV cross-reference index |
| v4.4 | Part XXXV final Q&A band |
| v4.5 | Part XXXVI this final control |

## XXXVI.5 Final control statement

**W3-06 is complete.** Parts I–XXXVI. Proposal only; no execution without
Annex U (Part I §U.2) and §VIII.5 signatures. Binding: the never-cross list
(§VIII.2.1), the ten sentences (§XVII.2), the sealed guard (§XVIII.2), the
interface oath (§XXIV.3), and the permanent guarantees above.

> The interface is the only part of the game every player touches. It should
> be the most trustworthy part of the game. This plan exists so that it is.

*Document control: W3-06 · Wave 3 (expanded, final) · HEAD 5be1a30a ·
end of W3-06.*

---

# W3-06 · PART XXXVII — THE SURFACE CATALOGUE (AUDIT NOTES PER CLASS)

> Every surface class with its specific audit notes — the map builders use
> when planning coverage.

## XXXVII.1 Core shelter surfaces

```text
hud_overlay       elements truthful; priority modes; collision sampling
shelter_hud       resource reads; units; update policies
rosters           dynamic list ordering; assignment controls; focus
schedule          shift controls; fatigue bands displayed
inventory         grid/list navigation; capacity displays
```

## XXXVII.2 Economy and trade surfaces

```text
trade_desk        quote/factor/supply/shock rows; re-quote notice
ration_screen     policy/ratios/outlook; morale bands displayed
market_intel      trends/staleness marks (C-P9); sourcing honesty
caravan_status    run states; ETA ranges; failure records
```

## XXXVII.3 Security surfaces

```text
readiness         layer states/coverage derived; gap warnings
ranging_alerts    confidence bands; aggregation; priority
defense_orders    posts/doctrine controls; cost displays
prisoner_panel    terms/conditions; dignity review of copy
heat_status       thresholds/warnings (scenes own phrasing)
```

## XXXVII.4 Crafting and industry surfaces

```text
craft_desk        readiness/requirements/costs/output; block reasons
station_panel     tier effects; condition; crew
chain_board       read model rendering; bottleneck reasons
knowledge_panel   unlocks; sources; eligibility displays
```

## XXXVII.5 Narrative and people surfaces

```text
journal           entry categories; oracle visibility; no clearing
relationship_view history moments; band transitions
memorial          names from registry; quiet presentation
dialogue          choice presentation; no mechanical truth leaked
radio_panel       channel freshness; cooldown presentation
```

## XXXVII.6 System surfaces

```text
settings          sealed floors; existing authority only
save_load         slots; state-change hooks; modal clearing
accessibility     sealed options only; additions signed
feedback_sink     canonical messages; dedupe; priorities
```

## XXXVII.7 Audit notes for all classes

```text
[ ] matrix row (all states)
[ ] focus graph + traversal
[ ] binds owner-sourced; rebind on session change
[ ] modal usage declared
[ ] HUD elements registered
[ ] lane states green
[ ] feedback canonical
[ ] actions in table
```

## XXXVII.8 The catalogue's promise

```text
Every class above was chosen so that no surface anywhere in the game is
outside the audit. If a new surface appears that fits no class, that is a
finding itself — the taxonomy must grow, not the blind spot.
```

*End of Part XXXVII. Continues in Part XXXVIII (the final runbook).*

---

# W3-06 · PART XXXVIII — THE FINAL RUNBOOK (FULL SEQUENCE)

## XXXVIII.1 The full sequence

```text
P0   baseline gates; manifest list; premise doc
P1   matrix driver; red-cell repairs; state-gate extension
P2   focus coverage; ordering; restore
P3   action table; parity; contexts; displays
P4   modal close paths; order/depth/identity/trap; load clear
P5   lifecycle ×100/switch/reset; hygiene extension; state model
P6   HUD registry; truth fixes; units/unavailable; priority
P7   a11y lanes; layout fixes; gate coverage; seal check
P8   IA inventory; proposals
P9   feedback census; canonical routing; dedupe; silence scan
P10  navigation dead ends; controller pass; signed list
SOAK cycles/counts/memory/controller
CLOSE evidence; memo; Annex U; debt rows
```

## XXXVIII.2 Command block (P0-verified)

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/UI
godot --headless --path . -- --ui-a11y-selftest
godot --headless --path . -- --player-panels-uitest
godot --headless --path . -- --panel-bind-lifecycle-selftest
godot --headless --path . -- --settings-selftest
# matrix driver: host-level harness (name verified at P0)
```

## XXXVIII.3 Exit criteria per phase

| Phase | Exit |
|---|---|
| P0 | premise filed |
| P1 | matrix green |
| P2 | traversal + restore green |
| P3 | parity kit green |
| P4 | modal kit green |
| P5 | cycle assertions green |
| P6 | HUD kit green |
| P7 | lanes green; seal intact |
| P8 | inventory filed |
| P9 | feedback kit green |
| P10 | dead ends removed/signed |
| soak | assertions green |
| close | meter full; memo filed |

## XXXVIII.4 The in-flight decisions

```text
sealed change needed      -> stop, escalate
surface outside taxonomy  -> extend taxonomy first
action with no device path -> authored exception or fix
focus unreachable          -> register or redesign control
a11y fix needs content     -> author copy (W2-06)
```

## XXXVIII.5 The post-close commitments

```text
per surface change: matrix row + registries + lane sample
weekly: T1; matrix spot
per release: T3 + controller pass + seal
per expansion: integration checklist
```

*End of Part XXXVIII. Continues in Part XXXIX (the final close).*

---

# W3-06 · PART XXXIX — THE FINAL CLOSE

## XXXIX.1 The complete artifact index

| Artifact | Location |
|---|---|
| route matrix report | docs/ui/ROUTE_MATRIX.md |
| focus graph | docs/ui/FOCUS_GRAPH.md |
| action table | docs/ui/INPUT_ACTIONS.md |
| modal types | docs/ui/MODAL_TYPES.md |
| lifecycle report | docs/ui/LIFECYCLE_REPORT.md |
| HUD bindings | docs/ui/HUD_BINDINGS.md |
| a11y lanes | docs/ui/A11Y_LANES.md |
| IA inventory/proposals | docs/ui/IA_INVENTORY.md, IA_PROPOSALS.md |
| feedback census | docs/ui/FEEDBACK_CENSUS.md |
| navigation dead ends | docs/ui/NAV_DEADENDS.md |
| surface sources | docs/ui/SURFACE_SOURCES.md |
| string register | docs/ui/STRING_REGISTER.md |
| evidence | docs/evidence/w3-06/ |

## XXXIX.2 The final statement of scope

```text
This plan changed nothing outside the interface, and inside the interface it
changed only what was needed for the four promises: no silence, no traps, no
lies, no exclusion. Everything else it produced is evidence, registers, and
gates — built so the promises outlive the plan.
```

## XXXIX.3 The final words

```text
Every press lands.
Every number means.
Every state can be left.
Every player is included.
```

## XXXIX.4 Document closure

**W3-06 complete.** Parts I–XXXIX. Proposal only; no execution without
Annex U (Part I §U.2) and §VIII.5 signatures. Binding within this document:
the never-cross list (§VIII.2.1), the ten sentences (§XVII.2), the sealed
guard (§XVIII.2), the interface oath (§XXIV.3), the permanent guarantees
(§XXXVI.3), and the four promises above.

*Document control: W3-06 · Wave 3 (expanded, final) · HEAD 5be1a30a ·
end of W3-06.*

---

# W3-06 · APPENDIX A — THE MAINTENANCE HANDOFF PACK

## A.1 What the standing gate needs

```text
[ ] the matrix driver on the focused pipeline (UI paths)
[ ] the action table generator with parity/conflict checks
[ ] the HUD binding source-row check
[ ] the lane sampler for changed surfaces
[ ] the sealed guard (U1.8) always-on
[ ] the feedback silence scan on failure scenarios
```

## A.2 What the weekly review needs

```text
- registry diffs (route/focus/action/HUD)
- new findings and their kits
- queue movement (repairs closed/opened)
- seal status
- ratchet deltas (never positive)
```

## A.3 What the release needs

```text
- full matrix + parity + lanes
- controller pass
- soak summary
- completion meter
- evidence pack index
```

## A.4 The handoff statement

```text
The interface's truthfulness is maintained, not achieved. This pack exists
so that the next contributor inherits the gates, not the debt.
```

## A.5 The one-page audit for a new contributor

```text
1. read Part XV.1 (surface checklist)
2. read Part XXXII (cookbook)
3. run the focused checks before proposing
4. never baseline a stop-class
5. file findings with owner + test + evidence
```

## A.6 The closing note

```text
If this pack is followed, a new panel takes an afternoon to integrate and an
hour to verify. If it is skipped, it takes a release to discover what broke.
The gates exist to make the former true.
```

*End of Appendix A. Continues in Appendix B (the last words).*

---

# W3-06 · APPENDIX B — THE LAST WORDS

## B.1 The four promises, again

```text
1. Nothing silent.
2. Nothing trapped.
3. Nothing false.
4. No one left out.
```

## B.2 Why they are worth a whole plan

```text
Because the interface is the game's body. A perfect simulation behind an
untrustworthy interface is a broken game; a modest system behind a
trustworthy interface is a playable one. ASHFALL's mercy and its danger both
arrive through the screen — and the player deserves to trust the delivery.
```

## B.3 The evidence the plan leaves behind

```text
- ten gates that never stop running
- five registries that regenerate and never drift
- one seal that never opens without its owner
- one oath every reviewer signs
- and the four promises, written where everyone who touches the UI will
  read them
```

## B.4 The final control (absolute)

**W3-06 is complete and closed.** Parts I–XXXIX + Appendices A–B. Proposal
only; no execution without Annex U and §VIII.5 signatures. All binding rules
of the document stand.

*Document control: W3-06 · Wave 3 (expanded, final, closed) · HEAD
5be1a30a · end of W3-06 — and end of Wave 3.*

---

# W3-06 · APPENDIX C — THE SURFACE-BY-SURFACE FIRST-RUN PLAN

> The order to work surfaces so early effort has the widest effect.

## C.1 Wave 1 — highest traffic

```text
1. hud_overlay + shelter_hud      (constant presence; HUD truth)
2. journal                        (most-visited narrative surface)
3. inventory                      (grid navigation; focus)
4. craft_desk                     (block messages; readiness)
5. trade_desk                     (quote rows; re-quote)
```

## C.2 Wave 2 — operational depth

```text
6. rosters + schedule             (dynamic lists; assignments)
7. readiness + defense_orders     (layers; posts)
8. ration_screen                  (policy/outlook)
9. radio_panel                    (freshness; channels)
10. memorial + relationship_view  (quiet surfaces; naming)
```

## C.3 Wave 3 — the rest

```text
11. knowledge_panel + chain_board
12. market_intel + caravan_status
13. prisoner_panel + heat_status
14. ranging_alerts + dialogue
15. settings + save_load + feedback_sink
```

## C.4 Why the order

```text
Wave 1 carries the most session time per surface, so its fixes compound.
Wave 2 carries the most state complexity, so its matrix cells find the most
bugs. Wave 3 is breadth; the gates make breadth cheap.
```

## C.5 Per-wave exit

```text
wave exit = matrix rows green for the wave's surfaces + lane samples +
registry entries current + findings filed.
```

## C.6 The estimate

```text
Wave 1: ~2 weeks with repairs
Wave 2: ~2 weeks
Wave 3: ~1 week
soak + closeout: ~1 week
≈ 6 weeks total (matches Part VII.5.2)
```

*End of Appendix C. Continues in Appendix D (the registry of promises).*

---

# W3-06 · APPENDIX D — THE REGISTRY OF PROMISES

> The promises the interface makes, written as testable statements. If a
> future contributor asks "what must never break?", this is the list.

## D.1 The promises

```text
P1  Every surface opens from the manifest route.
P2  Every surface closes in every state.
P3  No surface ever has two instances.
P4  Every interactive control is focusable and visible when focused.
P5  Focus is restored after every close.
P6  Every action is declared, displayed, and device-covered.
P7  No binding fires outside its contexts.
P8  Modals close topmost-first and never trap.
P9  Panels are leak-free at volume and rebind on session change.
P10 Every HUD value is an owner read with a unit, updated within a tick.
P11 Missing data is marked, never zeroed.
P12 Every surface meets the accessibility floors in every state.
P13 The sealed constants are never edited outside their owner.
P14 Every failure produces a canonical, except-free message.
P15 No core flow requires a mouse or a hover.
```

## D.2 The promise-watch

```text
Each promise maps to a gate (Part XXI.2). The registry is checked at
release; a promise without a guard is a finding, not a promise.
```

## D.3 The closing word

```text
Fifteen promises, four words: honest, reachable, calm, inclusive. Build to
them, gate them, and the interface will never be the reason a player leaves.
```

*End of Appendix D. W3-06 closes complete.*

---

# W3-06 · APPENDIX E — THE FINAL CHECKLIST CARD AND SIGNATURE

## E.1 The one-card checklist

```text
ROUTES      matrix green every state; single instance
FOCUS       all controls; visible; restored
INPUT       table declared; parity; contexts; displays
MODALS      close paths; order; depth; trap; load clear
LIFECYCLE   ×100 stable; rebind on switch; dispose on reset
HUD         owner reads; units; unavailable; priority
FLOORS      lanes × states; seal intact
FEEDBACK    canonical; deduped; no exceptions; no silence
NAVIGATION  no mouse/hover dead ends
SOAK        counts stable; memory flat; controller pass
```

## E.2 The signature block

```text
ASHFALL WAVE 3 · PLAN 6 · FINAL SIGNATURE
HEAD: ________  Date: ________
[ ] all Appendix E.1 lines green
[ ] evidence pack filed
[ ] completion meter full
[ ] no stop-class open
[ ] seal intact
Signed: ________  Foreman: ________
```

## E.3 The final note

```text
This document ends, but the gates do not. Every future panel, action, HUD
element, and modal inherits them. The interface keeps its four promises:
nothing silent, nothing trapped, nothing false, no one left out.
```

*End of Appendix E. W3-06 is complete and closed.*

---

# W3-06 · APPENDIX F — THE CLOSING MEASUREMENT AND FINAL DECLARATION

## F.1 The plan's measurements, final form

```text
reach:     every surface × state × (open, close) measured
focus:     every control counted, traversed, restored
input:     every action × device × context exercised
modals:    every type × (order, depth, trap, load) verified
lifecycle: every panel × 100 cycles measured for drift
truth:     every HUD element followed an owner change
floors:    every surface × 6 states lane-checked
feedback:  every failure class routed and deduped
navigation: every core flow device-complete
```

## F.2 Why measurement is the plan

```text
An interface claim without a measurement is a wish. This plan converts nine
wishes into nine gates. That is its entire product: the gates.
```

## F.3 The permanent record

```text
Files:      the thirteen living documents
Gates:      G1–G10 standing in the focused pipeline
Registries: five, generated, never hand-edited
Seal:       U1.8, permanent
Oath:       the reviewer's, standing
Promises:   the fifteen, watchable
```

## F.4 The declaration

**W3-06 is complete.** Parts I–XXXIX, Appendices A–F. Proposal only; no
execution without Annex U and §VIII.5 signatures. All binding rules stand.

*Document control: W3-06 · Wave 3 (expanded, final, closed) · HEAD
5be1a30a · end of W3-06 and end of Wave 3.*

---

# W3-06 · APPENDIX G — THE LAST MEASURE

```text
Six promises up front; fifteen in the registry; ten gates behind them.
One seal; one oath; four words: honest, reachable, calm, inclusive.
```

**End of W3-06 (180k-class).** Proposal only; execution requires Annex U and
§VIII.5 signatures.

*Document control: W3-06 · Wave 3 (expanded, final, closed) · HEAD 5be1a30a.*