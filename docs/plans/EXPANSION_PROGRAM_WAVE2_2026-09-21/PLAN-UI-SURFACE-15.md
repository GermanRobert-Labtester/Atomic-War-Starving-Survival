# PLAN-UI-SURFACE-15 — Panel Inventory, Liveness Proofs & Accessibility Pass

**Program:** ASHFALL Expansion & Integration Program — Wave 2 (2026-09-21)
**Status:** PROPOSED — not a claim. Foreman claim required.
**Owner role on execution:** Integrator (surfaces + gates) with Builders per
panel region.
**Depends on:** PLAN-LAUNCH-FACE-06 F6-1 (input/focus) and F6-4 (surface
coverage); PLAN-ORPHAN-SEAL-01 waves produce the systems that need surfaces.
**Expanded appendix:** [`PLAN-UI-SURFACE-15_APPENDIX-A_ROUTE_INVENTORY.md`](PLAN-UI-SURFACE-15_APPENDIX-A_ROUTE_INVENTORY.md)
— all **192 route ids** from `Main.PlayerSurfaces.cs` with their status
(`EXPANDED` · `REFERENCED` · `DECLARED_ONLY`), which host files reference each
one, and the count of `DECLARED_ONLY` routes needing classification. UP-15A/15B
work from this inventory.
**Non-goals:** no redesign of the visual language; no new navigation concept; no
per-system panels where a region suffices.

---

## 1. Outcome

The surface layer is already large and already governed — 192 route strings,
69 golden snapshots, panel route/coverage/liveness gates, a 237-file UI tree —
which is exactly why the programme must be disciplined about it. Wiring 99
systems must not become "99 new panels".

Deliverables:

1. a **surface inventory** that maps every route to its panel, read model,
   commands, lifecycle, and owning subsystem;
2. a **consolidation map** that routes new systems into existing panels as
   regions/lists, with a hard budget for new routes;
3. **liveness proofs** for every newly reachable system (the panel shows live
   state, not a stale copy);
4. an **accessibility pass** over the touched panels (contrast, text scale,
   reduce motion, colorblind-safe status, keyboard reachability);
5. **snapshot QA** that keeps the 69-panel corpus meaningful instead of
   churning.

---

## 2. Premise evidence (2026-09-21)

| Fact | Value | Source |
|---|---:|---|
| Route strings in `Main.PlayerSurfaces.cs` | 192 unique quoted ids | `grep -rhoP '"[a-z0-9_]+"' src/Main.PlayerSurfaces.cs \| sort -u \| wc -l` |
| Expanded surface list | `expandedIds` array + descriptor registration | `Main.PlayerSurfaces.cs` |
| Snapshot panels | 69 | `ls snapshots/ \| wc -l` |
| Panel route gate | `PanelRouteGateTests` (switch ↔ registry parity) | `KNOWN_DEBT` seal 2026-09-12 |
| Coverage gate | `PlayerSurfaceCoverageGateTests` 8/8 | debt evidence |
| Liveness gate | `PlayerSurfaceLivenessGateTests` | test tree |
| A11y selftest | `--ui-a11y-selftest`, 237 UI files checked | INTEGRATION_PLANS closeout |
| Panel lifecycle probe | `--panel-bind-lifecycle-selftest` 17/17 | DEC-27 evidence |
| Focus reality | 0 `FocusMode`/`MoveFocus` uses; focus grabbed in 2 places | PLAN-LAUNCH-FACE-06 §2.1 |
| New surfaces expected | culture vertical ~4 regions, body/industry ~6 regions | Plans 04/05 |

---

## 3. Authority map

| Concern | Extend (do not create) |
|---|---|
| Routing | `Main.PlayerSurfaces.cs`, `Main.PanelLifecycle.cs`, `OpenExpandedPanel` switch |
| Panels | existing `src/UI/*.cs`; add regions, not panels |
| Read models | the owning system's projection (`*Slate`, `*Projection`, read-only getters) |
| Accessibility | `AccessibilityPresentation`, `UserSettings`, `ColorblindColorMapper` |
| Snapshots | `snapshot-capture` tooling + `snapshots/` corpus |
| Gate | `PanelRouteGateTests`, coverage + liveness gates, `--ui-a11y-selftest` |

---

## 4. Packages

### UP-15A — Surface inventory (generated)
- Generate `docs/ui/SURFACE_INVENTORY.md`: `route | panel class | panel file |
  read model | commands | subsystem | lifecycle risk | snapshot?`.
- Sources: `Main.PlayerSurfaces.cs` descriptors, `PanelRegistryBootstrap`,
  panel classes, the liveness gate list.
- **Acceptance:** every route appears once; no route without a panel; no panel
  without a route or an explicit modal/stream classification.
- **Verify:** extend `generate-ui-panel-catalog.py --check` (exists) to emit
  the inventory.

### UP-15B — Consolidation map and route budget
- Group the inventory by domain; name the target panel per new vertical:
  - Culture → `ChroniclePanel` (already routed) + one new `culture` region in
    the shelter surface for calendar/exhibits; broadsheet as a modal read.
  - Body → the medical panel gains a clinic region; prosthetics/rehab use the
    existing survivor detail surface.
  - Industry → the power/foundry surfaces gain a Power Board region and
    production queue region.
  - Market → the trade panel gains route cards; black-market keeps its surface.
  - Polity/Distance → the map/expedition surface gains outpost and comms
    regions; vehicle garage gains modules.
- **Budget:** new top-level routes require a written justification; default
  is a region inside an existing routed panel.
- **Acceptance:** the consolidation map is the reference for Plans 04/05
  packages; no package adds a top-level route without an integrator sign-off.
- **Verify:** `PanelRouteGateTests`; coverage gate; design review in the
  package handoff.

### UP-15C — Liveness and lifecycle proofs
- For each region added: prove the panel's refresh reads the live authority
  (not a snapshot captured at open), and that close/dispose releases
  subscriptions and timers. Reuse the liveness gate pattern; add the new routes
  to its list.
- **Acceptance:** liveness gate green over the expanded surface set;
  `--panel-bind-lifecycle-selftest` extended with a bind/unbind/rebind probe
  per new region; no orphan control on shutdown (the 100-orphan signature from
  2026-09-20 must not return).
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/UI/`;
  `godot --headless --path . -- --panel-bind-lifecycle-selftest`.

### UP-15D — Accessibility pass
- Over touched panels: contrast checks on all status colors (success/warn/fail
  bands), text size floors (existing Plan 80 floors), colorblind-safe
  alternatives for status-only signals, reduce-motion honored in any new
  animation, keyboard reachability of every interactive control (ties to
  F6-1 focus work), screen-reader labels for icon-only buttons.
- **Acceptance:** a11y selftest green; a written per-panel checklist for the
  new regions; no "color-only" information.
- **Verify:** `godot --headless --path . -- --ui-a11y-selftest`;
  `godot --headless --path . -- --settings-selftest`.

### UP-15E — Snapshot corpus QA
- Re-render only the changed panels; review diffs; replace a snapshot only with
  a written reason; add snapshots for the new regions (bounded: ≤ 8 new).
- **Acceptance:** snapshot suite exits clean; no mass re-render; each changed
  image has a reviewer note.
- **Verify:** `bash scripts/ci/run-snapshot-diff.sh` (or the existing snapshot
  skill command).

### UP-15F — Modal/briefing hygiene
- Ensure the daily briefing/modal surface consumes the culture and industry
  events rather than a parallel feed; modal focus trap and close behavior
  verified (ties to F6-1).
- **Acceptance:** one briefing composer; no duplicate day-event lists; modal
  closes with cancel/back on keyboard/controller.

---

## 5. Risk register

| Risk | Mitigation |
|---|---|
| Region sprawl inside a mega-panel | each region has a header + collapse; inventory counts controls per panel and flags panels over a control budget |
| Liveness probe gives false confidence (refresh reads a copy) | probe asserts identity of the read source (same instance/hash-day) |
| Snapshot churn from font/contrast changes | change batches grouped; review before/after |
| Focus work conflicts with F6-1 | focus policy is F6-1's; UP only declares neighbors and labels |
| A11y findings become a rewrite | fix touched panels only; accepted findings registered with reason |

## 6. Verification summary

```bash
python3 scripts/ci/generate-ui-panel-catalog.py --check
bash scripts/run_test.sh Ashfall.Core.Tests/UI/
godot --headless --path . -- --panel-bind-lifecycle-selftest
godot --headless --path . -- --ui-a11y-selftest
godot --headless --path . -- --settings-selftest
```

## 7. Change control

`Main.PlayerSurfaces.cs` and the panel registry are integrator paths. Panels may
call commands and read projections only; they never own gameplay state. A new
top-level route needs the consolidation map updated first.

---

## 6. Expanded census (4 files · 834 lines)

Scope: `Assets/Ashfall.Core/UI/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 4

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `PanelRegistry.cs` | 283 | Support | — | 0 | 0 | 0 |
| `PanelRegistryBootstrap.cs` | 274 | Support | — | 0 | 0 | 0 |
| `PlayerSurfaceContract.cs` | 72 | Support | — | 0 | 0 | 0 |
| `PlayerSurfaceManifest.cs` | 205 | Support | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `surface_dragline_ruins.json` | array[8] |
| `surface_radiation_topo_sheets.json` | array[8] |

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/UI/` |
| Test references | 28 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 0 empty-catch sites routed through Plan 35's rules |
| Premise | the **premise** file(s) above must match the plan's stated line counts before edits |

## 9. Rollout sequence

1. Premise re-check: premise files unchanged since authoring (hash/mtime), or update the plan.
2. Interfaces: wire through the named owner; do not add a parallel store.
3. State: if capture/restore exists, register per Plan 1 Appendix Q; else state the system is stateless.
4. Data: resolve domain catalogs or report the loader path.
5. Verification: focused region + the plan's own acceptance table.
6. Regression: re-run this census; a changed file is a finding.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Premise file | matches its stated surface; no scope drift |
| Interface/wiring | one owner per state, no parallel store |
| State/save | round-trip or explicit stateless verdict |
| Data binding | catalog resolves or loader path documented |
| Verification | focused region green; census delta recorded |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not widen the plan's scope.

---

## 12. Cross-plan coupling

The domain set is the plan's own `.cs` enumeration (4 files).
Other plans referencing those names: **2**.

**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-UI-CONTRACT-FAMILY-TRUTH-277` | 4 |
| `PLAN-MORTUARY-MEMORIAL-TRUTH-123` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `UP-15A` | `PlayerSurfaceContract.cs`, `PlayerSurfaceManifest.cs` |
| `UP-15B` | no name match — resolve at claim time |
| `UP-15C` | no name match — resolve at claim time |
| `UP-15D` | no name match — resolve at claim time |
| `UP-15E` | no name match — resolve at claim time |
| `UP-15F` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidate files are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 4. Host files: **18** · Test files: **12** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 18 | `src/Main.Application.cs`, `src/Main.Campaign.cs`, `src/Main.GameFlow.cs`, `src/Main.PlayerSurfaces.cs`, `src/Main.UiPanels.cs` |
| Tests (`Ashfall.Core.Tests/`) | 12 | `Ashfall.Core.Tests/Campaign/Plan31BriefingRouteTests.cs`, `Ashfall.Core.Tests/Journeys/EndToEndPlayerJourneyTests.cs`, `Ashfall.Core.Tests/Orchestration/SubsystemManifestTests.cs`, `Ashfall.Core.Tests/Plan12CDecorTests.cs`, `Ashfall.Core.Tests/Tooling/InputMapContractTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 4 files; intra-domain edges: **5**; isolated: **0**.

| From | → To |
|---|---|
| `PanelRegistry` | `PanelRegistryBootstrap` |
| `PanelRegistryBootstrap` | `PanelRegistry` |
| `PlayerSurfaceManifest` | `PanelRegistry` |
| `PlayerSurfaceManifest` | `PanelRegistryBootstrap` |
| `PlayerSurfaceManifest` | `PlayerSurfaceContract` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **0** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| — | no section key shares a token with this domain |

**Verdict:** no section key shares a token with this domain — either the authority is derived/stateless, or its persistence key is named after a different owner. Not a conclusion; verify in the owning system.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **9** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--journal-weather-panel-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |
| `--port-contract-selftest` |
| `--selftest-manifest` |
| `--test-manifest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **4**.

| Event | First declaration |
|---|---|
| `OnContractForgiven` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractPaid` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractRenegotiated` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractSigned` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **2**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/mod_manifest_schema.json` |
| `Assets/StreamingAssets/Data/narrative_discovery_manifest.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **0** (0 files, 0 cases).

| Region | Files | Cases |
|---|---:|---:|
| — | no test region shares a token with this domain |

**Verdict:** no test region shares a token with this domain. Region coverage is directory-based, so check root-level test files too (560 exist) before concluding coverage is absent.

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **231**
(229 of them panels/HUD).

| Host file |
|---|
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |
| `src/Host/HoldfastTerminalPanel.cs` |
| `src/Host/HostCli.PanelTests.cs` |
| `src/Host/HostCli.SelfTestManifest.cs` |
| `src/Host/PanelBindLifecycleSelfTest.cs` |
| `src/Host/PortContractSelfTest.cs` |
| `src/Main.PanelLifecycle.cs` |
| `src/Muster/FactionActionPanel.cs` |
| `src/Muster/JournalWitnessPanel.cs` |
| `src/Radio/FactionRadioHudPanel.cs` |
| `src/UI/AchievementsPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **0**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| — | no section key shares a token with this domain |

**Verdict:** no save section matches — persistence is owned under a differently-named section, or absent.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **0**.

| Stream |
|---|
| — | no seeded stream shares a token with this domain |

**Verdict:** no seeded stream shares a token — either the domain is deterministic without randomness (fine) or it draws from an unlisted source (check `System.Random`/time seeding before shipping).

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **0**
(none).

| Catalog | Classification |
|---|---|
| — | no catalog filename shares a token with this domain |

**Verdict:** no catalog matches — the domain is code-authoritative or its data lives in a broader catalog.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 2
**Surface:** save sections 0 (laddered 0) · RNG streams 0 · host files 12 · catalogs 2 · test regions 0 · flags 9

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-UI-SURFACE-15
wave: —
status: PROPOSED — foreman claim required
packages: UP-15A, UP-15B, UP-15C, UP-15D, UP-15E, UP-15F
claim paths:
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Economy/TradeScreenGodotPanel.cs  # §19 candidate host surface
  - src/Host/HoldfastTerminalPanel.cs  # §19 candidate host surface
  - src/Host/HostCli.PanelTests.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/mod_manifest_schema.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative_discovery_manifest.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --expedition-panel-lifecycle
dependencies:
  - coordinate: 2 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | yes |
| depends | yes |
| non_goals | yes |
| outcome | yes |
| evidence | yes |
| packages | yes |
| acceptance | yes |
| risks | yes |
| verification | yes |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** none — claim-ready.
