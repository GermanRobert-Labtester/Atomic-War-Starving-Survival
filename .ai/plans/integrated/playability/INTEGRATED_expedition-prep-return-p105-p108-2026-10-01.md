# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Expedition prep, dispatch projection & return consequences — P105–P108 (2026-10-01)

Source rows P105–P108 of `docs/plans/PLAYABILITY_200_SUGGESTIONS_2026-10-01.md`
(Group 14 — Expeditions & map travel).

## Bounded outcome

- **P105** — an expedition prep checklist (food / water / rad meds / weapon /
  light) computed from the shelter inventory and the live dispatch estimate.
- **P106** — expected duration, supply burn, and risk grade shown before
  dispatch, in the same panel as the existing estimate.
- **P107** — a return-loot ceremony: the loot payload is itemised and a
  summary is surfaced (and one authority deposits it).
- **P108** — failed sorties surface their consequence (injured / lost) through
  the existing health, fate, and journal owners.

Non-goals: no new save section, no new gameplay resource, no parallel
simulation, no invented "missing" lifecycle state, no travel/food/water
consumption mechanic.

## Evidence (pre-change)

- `ExpeditionSystem.Fail` had exactly two sources — tick exhaustion and
  overnight-camp collapse — and both simply ended the sortie. No health, fate,
  journal, or feedback consequence was applied; a failed sortie looked like a
  no-op and its salvage vanished silently.
- `ExpeditionPanel.OnExpeditionCompleted` deposited loot into the inventory
  session **and** `Main.SetupExpeditions`'s own `OnExpeditionCompleted` handler
  deposited the same payload into the same session — a double deposit on the
  production path (the panel was bound with `_inventory`).
- No pre-dispatch checklist, supply burn, or failure-aftermath surface existed.
- `SurvivorLifecycleState` docs explicitly reject a `Missing` state: "no
  missing-survivor concept exists anywhere in Core or the host. Adding one
  would be inventing gameplay." `SurvivorDeathCause.Expedition` already reads
  "lost on expedition", which is the codebase's existing record for a survivor
  who did not come back.

## Change

### Core (pure, engine-free)

- **New** `Assets/Ashfall.Core/Expeditions/ExpeditionPrepPlan.cs`:
  - `ExpeditionPrepPlanner.Build(def, estimate, countById, weaponReady, hasLight)`
    returns an `ExpeditionPrepPlan` with the five-row checklist, supply burn,
    projected duration, and four-band `ExpeditionRiskGrade`.
  - Food/water burn derives from the estimate's projected hours and the
    authored camp consumption rate (2 units per 12-hour overnight window);
    rad meds are only recommended when the canonical estimate projects a dose.
  - `FailureHealthLoss`, `ResolveFailureOutcome` classify a failed sortie as
    injured or lost (fatal only when the injury would take health to ≤ 0).
  - Mutates nothing; no RNG; no state.

### Host / panel

- `src/UI/ExpeditionPanel.cs`
  - `_prepLabel` renders the checklist/burn/risk line on every estimate update.
  - `OnExpeditionCompleted` no longer deposits: it renders an itemised
    **return-loot ceremony** from the event payload and raises the existing
    `OnLootDeposited` notification. `Main` is the single depositor.
  - `OnExpeditionFailed` (new engine subscription) renders a **SORTIE
    AFTERMATH** line naming the reason and any salvage lost in the field.
  - Observability: `PrepSummaryText`, `LastReturnSummary`; test seams
    `PresentReturnForTest` / `PresentFailureForTest` (matching the existing
    `*ForTest` panel convention).
- `src/Main.Expeditions.cs`
  - Subscribes `Engine.OnExpeditionFailed`.
  - `OnExpeditionFailed` applies the consequence through existing owners:
    health loss through `NeedsSystem` (injury), `SurvivorFateSystem.ReportDeath`
    with `SurvivorDeathCause.Expedition` **before** the health mutation so the
    fate ledger names the expedition (first-write-wins), health-history log,
    journal entry, and a persistent advance-feedback toast.
- `assets/l10n/strings.csv`: three new keys (`ui.expedition.prep_header`,
  `ui.expedition.return_header`, `ui.expedition.failure_header`).

### Tests

- **New** `Ashfall.Core.Tests/Expeditions/ExpeditionPrepPlanTests.cs` — 18
  cases: fixed row order, empty/stocked readiness, multi-variant counting,
  burn math, dose-gated rad meds, equipment rows, risk-band theory, failure
  classification, non-mutation.
- `src/Main.UiTests.Expeditions.cs` — `--expedition-panel-uitest` now asserts
  the prep projection renders, the return ceremony itemises its payload, and
  the failure aftermath names the reason verbatim.
- Regenerated `docs/architecture/ARCHITECTURE_TEST_MAP.md` (315 subsystems).

## Verification (exact commands)

- `dotnet build Ashfall.csproj -c Debug` → **0 Warning(s), 0 Error(s)**.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/ExpeditionPrepPlanTests.cs`
  → **18/18 passed**.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/ExpeditionLootIntegrityTests.cs`
  → **10/10 passed**.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/Plan21EstimateProtectiveInputsTests.cs`
  → **9/9 passed**.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Localization/StringsCsvLocaleGateTests.cs`
  → **4/4 passed**.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/LocalizationRatchetTests.cs`
  → **2/2 passed**.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/ArchitectureTestMapGateTests.cs`
  → **7/7 passed**.
- `python3 scripts/ci/generate-architecture-map.py --check` → **OK (315)**.
- `bash scripts/ci/run-godot-bounded.sh --path . -- --expedition-panel-uitest`
  → **SELFTEST PASS: expedition_panel_uitest**.

## Audit / repair loops

1. **Double loot deposit** — panel + Main both deposited on the production
   path. Repaired: the panel is presentation; `Main` is the single depositor.
2. **Fate cause race** — applying health first would let the needs cascade
   record a collapse as `Needs` instead of `Expedition`. Repaired: the fatality
   path reports the fate record before mutating health.
3. **Silent failed-sortie salvage** — a failed sortie dropped its loot with no
   trace. Repaired: the failure aftermath names lost salvage and the journal
   keeps the day.
4. **Localization ratchet** — new panel strings routed through `strings.csv`;
   locale gate and ratchet both green.
5. **Architecture map drift** — regenerated by the owning generator; `--check`
   green.
6. **Ceremony/depositor quantity mismatch** — the panel skipped zero-quantity
   loot rows while `Main` deposited them with a `Math.Max(1, …)` floor, so the
   ceremony could name fewer units than the inventory received. Repaired: both
   sides now use the same floor and both skip only empty item ids; the ceremony
   falls back to an explicit "no salvage recovered" line.
7. **Latent NRE in the host feedback bridge** —
   `ExpeditionHostSession`'s `OnExpeditionCompleted`/`Started`/`Failed`
   handlers dereferenced the event state without a null guard and read
   `s.loot.Count` (null after a malformed save). Hardened to null-safe reads.
8. **Rescue-mission failure bridge (found, not repaired)** — a dispatched
   distress-rescue sortie that fails before reaching its destination leaves the
   `DistressRescueMission` at `Dispatched` (it is not expired by `TickDaily`,
   which only ages `Heard`/`Identified`). Surfacing this through the existing
   `TerminalFailed` stage / `RescueFailed` follow-up trigger would require a
   radio-domain design decision (retry vs. terminal, and the exact
   `ArrivalResolved` semantics for the follow-up scheduler), so it is recorded
   here rather than improvised. It is adjacent to, but outside, the bounded
   P105–P108 change.

## Wave 2 — ten follow-ups, all integrated (2026-10-01)

User-directed ("Continue with these small tasks completely finish all of them").

1. **Failure selftest** — the `--expedition-panel-uitest` gate now drives a real
   tick-collapse end-to-end and asserts the fate owner records
   `SurvivorDeathCause.Expedition`, the host session keeps the aftermath, and
   the panel surfaces it. (Folded into the existing flag rather than adding a
   new CLI flag, to avoid selftest-manifest/CLI-catalog churn.)
2. **Rescue-mission failure bridge** — `DistressRescueMissionManager.RecordExpeditionFailed`
   transitions Dispatched → TerminalFailed exactly once (never `ArrivalResolved`,
   so no salvage is granted for a failed run); `Main.BridgeDistressRescueOnFailure`
   wires it from `OnExpeditionFailed`.
3. **Truthful light row** — `ExpeditionPrepChecklistItem.Advisory`; the light row
   is advisory (no mechanic consumes `hasFlashlight`) and never gates readiness.
4. **Persistent return summary** — `ExpeditionHostSession.LastReturnSummary` /
   `LastReturnWasFailure` own the text; the panel syncs from the host on every
   refresh, so the ceremony survives an unbound window and a newer sortie
   replaces a stale one. Shared formatter `ExpeditionReturnReport` (Core).
5. **Per-survivor selector** — the dispatch prep row now selects which living
   survivor the estimate and dispatch target, instead of always the first.
6. **Localization** — five remaining `ExpeditionPanel` literals + the new
   survivor/injury strings routed through `strings.csv`; ratchet lowered 536 → 528.
7. **Injured chip** — `StatusPanel` binds `HealthHistoryHostSession` (optional,
   live-subscribed) and renders the latest `expedition_failure*` event per living
   survivor.
8. **Risk driver** — `ExpeditionPrepPlan.RiskNote` names the dominant risk
   contributor (`danger lvl N` / encounter / breakdown / dose / mid-route).
9. **Failed-sortie metric** — `RecordPlayMetricExpeditionFailed` →
   `PlayMetricsHostSession.RecordExpeditionFailed` (`expedition.failed`).
10. **Camp burn** — the projection adds the authored overnight burn
    (`CampFirewoodPerSegment`/`CampWaterPerSegment`/`CampFoodPerSegment` ×
    `CampNightSegments` × nights); tent/bedroll are **not** modeled as inventory
    items and are deliberately not fabricated.

### Wave-2 verification

- `dotnet build Ashfall.csproj -c Debug` → **0 warnings / 0 errors**.
- `ExpeditionPrepPlanTests` **23/23**; `ExpeditionRescueFailureBridgeTests` **4/4**;
  `LocalizationRatchetTests` **2/2**; `StringsCsvLocaleGateTests` **4/4**.
- Radio regression: `RescueSignalRuntimePersistenceTests` **5/5**;
  `SignalTrustTests` **21/21**; `DistressSignalTasks912ReplayTests` **5/5**;
  `RescueSignalSalvagePreflightTests` **13/13**.
- `--expedition-panel-uitest` → **PASS (73 assertions, 0 fail)**, including
  "failure routed to the fate owner as lost on expedition".
- Arch map `--check` **OK (315)**.

### Wave-2 audit loops

1. **Stale ceremony on rebind** — the panel only adopted the host summary when
   its own was empty, so a sortie that ended while the panel was closed left an
   older ceremony on screen. Hardened: the panel now syncs whenever the host
   text differs.
2. **Panel subscription hygiene** — `StatusPanel` holds the health-history
   session and now detaches/attaches its `StateChanged` in `Bind`, so the injury
   chip refreshes live without leaking a handler across rebinds.
3. **Null-flow warning** — capturing the host into a local keeps the production
   `CS8602` gate at zero (host build 0/0).
4. **Dead field** — removed the orphaned `_selectedSurvivorId` after the selector
   took over the survivor choice.
5. **Fingerprint safety** — the rescue-failure transition deliberately does not
   touch `ArrivalResolved` or the mission fingerprint, so existing saves still
   load (the dead-arrival salvage branch cannot fire for a failed run).

## Wave 3 — fifteen follow-ups, all integrated (2026-10-02)

User-directed ("Continue with these small tasks completely finish all of them").

1. Rescue-failure no-op on a completed rescue (Core test).
2. `ExpeditionReturnReport` null-state returns empty (Core test).
3. `FailureHealthLoss` blank reasons (Core test).
4. `HumanizeToken` stable-id cases (Core test).
5–9. Localized `SCOUT:` / `DANGER:` / `STAMINA` / `DISPATCH STEALTH` / `DISPATCH SPEED`.
10. `ExpeditionPanel.ClearReturnSummary` + `ResetExpeditionFamily` call.
11. `HealthHistorySystem.GetLatestEventOfTypePrefix` (one canonical lookup) used by StatusPanel.
12. `ExpeditionInjuryDigest` (living-only rule) used by StatusPanel + Core test.
13. `MapPanel` active-sortie card names the dominant risk driver.
14. `PlayMetricsHostSession.FailedSortieCount` read model.
15. `ExpeditionPrepPlanner.MaxOvernightNights` clamp + test.

### Wave-3 audit loops

1. **Dead using** — `using Ashfall.Core.Medical;` in `StatusPanel` became unused after
   the digest refactor; removed.
2. **Uncovered helper** — `GetLatestEventOfTypePrefix` had no direct test; added one
   (latest-wins, prefix filter, blank inputs) plus digest null-input coverage.
3. **Format-throw hardening** — the localized `SCOUT`/`DANGER`/`STAMINA` rows used
   `string.Format` directly, which throws on a malformed catalog row; routed through
   a `TrFmt` helper that falls back to the raw template. Also added a
   `DescribeStateRisk(null)` assertion.

### Wave-3 verification

- `dotnet build Ashfall.csproj -c Debug` → **0 warnings / 0 errors**.
- `ExpeditionPrepPlanTests` **36/36**; `ExpeditionRescueFailureBridgeTests` **5/5**;
  `StringsCsvLocaleGateTests` **4/4**; `LocalizationRatchetTests` **2/2**;
  `StatusPanelThresholdTests` **106/106**; `Plan198HealthHistoryIntegrationTests` **6/6**.
- `--expedition-panel-uitest` **PASS (73/0)**; `--player-panels-uitest` **PASS (22/0)**.
- Arch map `--check` **OK (315)**.

## Wave 4 — fifteen follow-ups, all integrated (2026-10-02)

User-directed ("Continue with these small tasks completely finish all of them").

1. `HostCli.PlayMetrics` observes `FailedSortieCount`.
2–4, 12, 15. Core tests: injury-digest roster order; later-same-day event wins;
   overnight-clamp boundary; cross-survivor isolation; zero/negative encounter.
5–9. Localized the remaining panel headers/buttons (active sorties, pending,
   dispatch prep, destinations, estimate, advance, return, push-luck, order-return).
10. `ExpeditionRadarPanel` active rows name the dominant risk driver.
11. `--player-panels-uitest` re-binds StatusPanel with health history (re-bind safe).
13. New `ExpeditionLocaleKeysTests` pins every expedition/status key (present,
    German differs, placeholder parity).
14. `docs/CURRENT_AUTHORITY.md` maps the prep/return/injury read models.

### Wave-4 audit loops

1. **Magic-string suppression** — the radar compared `riskNote == "low danger"`
   literally; promoted to `ExpeditionPrepPlanner.LowRiskNote` and applied the same
   suppression in `MapPanel` so low-danger rows stay quiet everywhere.
2. **Unpinned contract** — added a test asserting `DescribeStateRisk` returns the
   `LowRiskNote` constant (non-empty), so a future rename cannot silently break
   the suppression.
3. **Subscription/refresh hygiene** — ran `PanelSubscriptionHygieneTests` and
   `PanelLiveRefreshGateTests` after adding the StatusPanel health-history
   subscription; both green (no lambda unsubscribe, no leaked refresh handler).

### Wave-4 verification

- `dotnet build Ashfall.csproj -c Debug` → **0 warnings / 0 errors**.
- `ExpeditionPrepPlanTests` **41/41**; `ExpeditionLocaleKeysTests` **1/1**;
  `StringsCsvLocaleGateTests` **4/4**; `LocalizationRatchetTests` **2/2**;
  `StatusPanelThresholdTests` **132/132**; `PanelSubscriptionHygieneTests` **2/2**;
  `PanelLiveRefreshGateTests` **2/2**.
- `--playable-metrics-selftest` **21/0**; `--expedition-panel-uitest` **PASS (73/0)**;
  `--player-panels-uitest` **PASS (22/0)**.
- Arch map `--check` **OK (315)**.

## Wave 5 — fifteen follow-ups, all integrated (2026-10-02)

User-directed ("Continue with these small tasks completely finish all of them").

1. Localized the remaining ExpeditionPanel buttons/header (`RESOLVE`, `DISMISS ALL`,
   `OK`, `DECIDE LATER`, `TACTICAL APPROACH SELECTION`).
2. Localized the ExpeditionRadarPanel column headers (active + target grids).
3. Localized the radar sidebar filters + hints and the section headers.
4–6, 13, 15. Core tests: vehicle-disabled-only, high-encounter override,
   null-description tolerance, null `countById`, zero/negative encounter.
7–8. `--playable-metrics-selftest`: fresh-session `FailedSortieCount == 0` and
   failed-sortie-does-not-harvest.
9. `PanelSubscriptionHygieneTests` gates `StatusPanel.Bind` health-history detach.
10. `ui.expedition.estimate_tooltip` (and all new keys) pinned by the locale gate.
11. Localized the `Fitness factors:` prefix (both occurrences).
12. `CURRENT_AUTHORITY.md` row for the play-session metrics read models.
14. Localized the radar selection hint.

### Wave-5 audit loops

1. **CSV field integrity** — the German selection hint contained a comma and was
   unquoted (would split the row into five fields); quoted it, and fixed a
   `TACTICAL APPROVAL` → `TACTICAL APPROACH` typo. The full-catalog gate then passed.
2. **Suppression consistency** — re-verified that the `LowRiskNote` driver is
   suppressed in both `MapPanel` and `ExpeditionRadarPanel` (no "low danger" noise).
3. **Coverage hardening** — pinned all 31 expedition keys and the StatusPanel
   health-history detach; ran the subscription-hygiene, locale, status-threshold
   and ratchet gates green.

### Wave-5 verification

- `dotnet build Ashfall.csproj -c Debug` → **0 warnings / 0 errors**.
- `ExpeditionPrepPlanTests` **45/45**; `ExpeditionLocaleKeysTests` **1/1**;
  `StringsCsvLocaleGateTests` **4/4**; `LocalizationRatchetTests` **2/2**;
  `StatusPanelThresholdTests` **182/182**; `PanelSubscriptionHygieneTests` **3/3**.
- `--playable-metrics-selftest` **23/0**; `--expedition-panel-uitest` **PASS (73/0)**;
  `--player-panels-uitest` **PASS (22/0)**.
- Arch map `--check` **OK (315)**.

## Wave 6 — fifteen follow-ups, all integrated (2026-10-02)

User-directed ("Continue with these small tasks completely finish all of them").

1–3. Radar phase cell values, status-rail card labels, range filter and shell
   title localized; new shared `ExpeditionPhaseText` helper.
4–6, 11–12. Core tests: `GradeRisk(null,null)`, `ChecklistLine` advisory marker,
   zero-quantity loot floor, survivor-id case-insensitivity, blank-id skip.
7–10, 13. Panel summary / Esc hint / no-active / `[ONE-TIME]` / fitness states localized.
14. German smoke assertions for the new expedition strings in the UI test.
15. `CURRENT_AUTHORITY.md` row for the expedition panel routes.

### Wave-6 audit loops

1. **Remaining phase-display drift** — the encounter modal and autoplay banner still
   formatted `((ExpeditionPhase)…).ToString().ToUpperInvariant()`; routed through the
   shared `ExpeditionPhaseText` helper so all phase text is localized consistently.
2. **Grid-vs-detail label drift** — the radar grid headers were localized but the
   detail-box field labels were still hardcoded; localized `Scout`/`Travel`/`Cargo`/
   `Encounter/hr`/`Route` and reused the grid keys for the rest.
3. **Coverage hardening** — pinned the five new detail keys in the locale gate and
   re-ran the architecture-map and subscription-hygiene gates green.

### Wave-6 verification

- `dotnet build Ashfall.csproj -c Debug` → **0 warnings / 0 errors**.
- `ExpeditionPrepPlanTests` **50/50**; `ExpeditionLocaleKeysTests` **1/1**;
  `StringsCsvLocaleGateTests` **4/4**; `LocalizationRatchetTests` **2/2**;
  `StatusPanelThresholdTests` **200/200**; `PanelSubscriptionHygieneTests` **3/3**;
  `ArchitectureTestMapGateTests` **7/7**.
- `--expedition-panel-uitest` **PASS (0 fail)**; `--player-panels-uitest` **PASS (22/0)**.
- Arch map `--check` **OK (315)**.

## Limitations / honest notes

- **"Missing" is not integrated as a distinct state.** ASHFALL has no
  missing-survivor concept and its lifecycle docs explicitly forbid inventing
  one. A survivor lost on a sortie is recorded through the existing fate owner
  with `SurvivorDeathCause.Expedition` ("lost on expedition"). P108 is fully
  integrated for the injured and lost/dead arms; the missing arm is
  deliberately not fabricated.
- No commit; full suite not run. Foreign dirty-worktree changes were preserved
  (including `src/UI/GameHudSnapshotFixture.cs`, written concurrently).
