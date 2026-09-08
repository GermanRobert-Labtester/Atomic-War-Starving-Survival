# Plan 85 UI Surface — UI-21 Re-Audit (progress feedback + dispatch block reason)

Scope: re-audit of the Plan 85 damaged-map player surface against the UI-21
acceptance criteria (route → bind → visible → select → command → state delta →
feedback → save/reload proof; fail on engine exceptions). Read-only audit plus
one thin host feedback wiring; no new UI subsystem (per §85C.8 and the UI-05
anti-pattern register).

## Chain verification

| Step | Status | Evidence |
|---|---|---|
| Route | PASS | 12 pre-authored `expeditions.json` destinations (id == map node id); dispatch gated in Core |
| Bind | PASS | `WorldHostSession.Create` → `DamagedMapCatalogLoader.CreateSystem(dataDir, session.WastelandMap)`; attached to the expedition engine via `Main.AttachDamagedMapIfReady` (reference-guarded, order-independent) |
| Visible (pre-reveal) | PASS | Installation nodes authored `discoverable: false`, `startingUnlocked: false`, danger `locked`; destination refuses dispatch (`ExpeditionSystem.Start` → `IsDestinationLocked`, pinned by `ExpeditionSystem_StartRefused_WhileDestinationLocked`) |
| Select / command | PASS | Standard expedition dispatch on the revealed destination; fragment acquisition is a normal Plan 46 scavenging roll (`PerformLootRoll` → `RegisterFragment`) |
| State delta | PASS | `WastelandMapState.RegisteredMapFragments` + `Discovered`/`Unlocked` (save-verified by the save-fuzz battery) |
| **Feedback** | **GAP → CLOSED (this pass)** | See below |
| Save/reload proof | PASS | `WastelandMapFragmentPersistenceFuzzTests` (9 tests) + `DamagedMapSystemTests.Reveal_PersistsThroughCaptureRestore_AndSurvivesReload` |
| Engine exceptions | PASS | Full-suite runs green; no `ObjectDisposedException`/`NullReferenceException` in the damaged-map path (the UI-18-class defect pattern does not exist here — no panel-local label churn) |

## Finding 1 (HIGH, fixed this pass) — fragment discovery was silent

**Before:** `DamagedMapSystem` raised `OnFragmentRegistered` / `OnZoneCompleted`
/ `OnInstallationRevealed`, and `ExpeditionSystem.PerformLootRoll` registered
fragments — but **zero subscribers existed anywhere in `src/`**. A player could
find a map fragment and receive no feedback at all: no expedition outcome text,
no `LastEvent`, no journal entry. The state delta happened invisibly. This is
exactly the UI-21 "command → state delta → feedback" break.

**Fix (thin, presentation-only):** `ExpeditionHostSession.AttachDamagedMapFeedback`
(`src/Host/ExpeditionHostSession.cs`), called from
`Main.AttachDamagedMapIfReady` when the damaged-map layer binds to the
expedition engine:

- fragment found → `LastEvent = "Map fragment found: <label> (n/total — <zone name>)."`;
- map completed → `LastEvent = "Map completed: <zone> — <installation> revealed on the world map."`.

This uses the existing single-feedback-strip convention (`LastEvent` +
`RaiseStateChanged()`), subscribes at most once per session, and keeps all
rules in Core (headless-safe: Core events are unchanged). Host build: 0 errors,
0 warnings.

## Finding 2 (MEDIUM, verified working) — dispatch block reason

`ExpeditionHostSession.GetBlockReason` reports
**"Map incomplete — location unidentified"** for locked installations
(`ExpeditionHostSession.cs:93`), composed after the crossing/extra gates and
before the F4 clue gate. The reason flows through the same
`IsLocationBlocked`/`GetBlockReason` seam the dispatch UI already renders for
other gates. Verified: the reason string is reached for every installation
destination (all 12 are registered in `_zonesByDestination` via
`ResolveRevealNodeId`), and never for non-installation locations
(pinned by `DestinationGate_BlocksUntilRevealed_NeverGatesOtherLocations`).

## Finding 3 (MEDIUM, deferred by design) — no dedicated progress panel

No UI surface lists per-zone fragment progress (2/3, 3/3, …) outside the
`LastEvent` strip. This is the **documented deferral** from the Plan 85
completion report: a dedicated damaged-map panel is a new UI subsystem,
forbidden in a data plan (§85C.8) and exactly the shape the UI-05 register
rejects when built as a shell. The player-visible minimum is now met:
per-fragment feedback (Finding 1), completion feedback, world-map marker
transition (Locked → Discovered via standard node rendering), and the
dispatch block reason. A zone-progress list panel remains legitimate
follow-on work and must be designed as a real domain workflow.

## Cross-reference

- Save-side battery: `docs/saves/SAVE_FUZZ_REPORT.md`.
- Deferral provenance: `docs/cartography/PLAN85_COMPLETION_REPORT.md` ("Deferred follow-ons").
- UI-21 criteria source: AGENTS.md → UI PANELS & UX AUDIT → UI-21.
