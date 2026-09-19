# Faction War Communiqué Player-Facing Surface — Integration Plan

> **STATUS: FULLY INTEGRATED & VERIFIED (2026-09-19)**
> - Phase A (Day-axis decision): Day tick gate uncapped (`day >= 180`), timeline self-clamps safely.
> - Phase B (Surface): `FactionCommuniqueBoardPanel` + `PanelRegistry` route + Dashboard button + `FactionsPanel` live data.
> - Zero `authorNote` leaks: mechanical source-scan gate PASS (31/31).
> - Selftests: `--faction-communique-board-selftest` PASS, `--warlord-ui-selftest` PASS, `--data-integrity-selftest` PASS (338 catalogs), `--real-campaign-journey-selftest` PASS. Build 0/0.

Built from `docs/forensics/FACTION_WAR_COMMUNIQUE_SURFACE_FORENSIC_REPORT.md` (read-only
forensic pass, this session) plus targeted follow-up evidence.

---

# 1. Objective

Make the 40 authored faction-war communiqués perceivable by the player as a day-gated,
faction-attributed public-statements board, and settle the war-arc day-axis decision
that governs when any extended-campaign content can surface — WITHOUT adding a
seen-state/history subsystem, without touching event-chain ownership, and with
`authorNote` provably excluded from every player surface.

Two-phase scope (as commissioned):

- **Phase A — Day-axis decision**: decide, with evidence, how the war arc's authored
  day axis (480–607) maps to runtime clocks; land the minimal wiring change that
  decision requires.
- **Phase B — Communiqué surface**: `CommuniqueBoard` panel + catalog accessor +
  route + headless selftest, following the `RadioBroadcastTerminal` pattern.

# 2. Current Reality

Evidence: forensic report §2–§7, §16. Summary:

1. The corpus (40 communiqués) is complete, contract-pinned
   (`FactionWarCommuniqueExpansionTests`, 29 tests), and loaded at
   `YearOfAshHostSession.Create` — into `FactionWarChainRunner._catalog`, which is
   **private with no accessor** (`FactionWarChainRunner.cs:307`).
2. Zero non-test consumers of any faction-war narrative query API. No renderer.
3. Runtime clocks: campaign calendar uncapped (`CampaignDayCoordinator`,
   `_simDay` = `Calendar.CurrentDay`, `Main.cs:55`); Year-of-Ash timeline hard-clamps
   180–360 (`YearOfAshTimelineSystem.cs:35-36,62-63`); the campaign owner ticks Year of
   Ash only `day >= 180 && day <= 360` (`Main.CampaignOwners.cs:1059`).
4. Consequence: war chains never fire; a renderer keyed to `Timeline.CurrentDay`
   would display nothing forever. A renderer keyed to `_simDay` CAN display in
   extended play (campaign runs past 360; epilogue is a view, not a forced end —
   `Main.GameFlow.cs:500`, `EndgameSystem.cs:148`).
5. `authorNote` is leak-free and mechanically gated — including a source-scan gate
   that fails if any `src/**.cs` contains the string `authorNote`.
6. Proven patterns exist: `RadioBroadcastTerminal` (day-gated, no-seen-state text
   widget), `FactionWarMapWidget` (Bind/Unbind/Refresh lifecycle),
   `--warlord-ui-selftest` (headless UI verification),
   `--real-campaign-journey-selftest` (production pipeline driver), PanelRegistry
   route + dashboard nav (`AddNavButton(container, label, routeId, …)`,
   `GameDashboardPanel.cs:405`), `faction_lore.json` display-name authority
   (`display_name` keyed by `faction_id`; loader pattern at
   `FactionsNarrativePanel.cs:60-99`).

# 3. Required Delta

**Existing behavior**: communiqués are inert loaded data.

**Requested behavior**: (1) a decided, wired relationship between the war arc's day
axis and runtime clocks; (2) a player-facing, day-gated communiqué board with truthful
empty states, real binding, a registered route, and headless verification.

**Delta** (minimum):
- one read-only Core accessor exposing the loaded catalog;
- one host tick-gate change (Phase A) + safety pins;
- one new panel class + route + nav entry;
- one headless UI selftest (+ gate allowlist for its audit assertion);
- replacement of the FactionsPanel fixture "communiqués" card with real data;
- snapshot target + manifest baseline.

No new systems, no new state, no schema changes, no selector/history subsystem.

# 4. Evidence

Primary: `docs/forensics/FACTION_WAR_COMMUNIQUE_SURFACE_FORENSIC_REPORT.md` (Evidence
Index §19). Follow-up evidence gathered for this plan:

- `YearOfAshTimelineSystem.PhaseForDay` (`:182-187`) — `day > 300` → Phase6 forever:
  clamped, sane for extended play.
- `YearOfAshDeepFreezeSystem.TickDailyThermal` (`:39-47`) — `day > 240` → convergent
  thaw branch (safe for any later day).
- `YearOfAshRadonSystem.TickDailyRadon` (`:42-55`) — `day >= 300` → thaw branch,
  convergent (safe).
- `TickWarlord` (`YearOfAshHostSession.cs:176-184`) — idempotent world-view
  computation from catalog adjacency; no day-band assumptions (selftest already ticks
  to 270+, `HostCli.SelfTests.cs:489`).
- `faction_lore.json` — 45 entries keyed `faction_id`, with `display_name`
  (`faction_central_garrison → "The Central Garrison"` etc.); the four communiqué
  factions all resolve.
- `--real-campaign-journey-selftest` (`Main.UiTests.RealCampaignJourney.cs:34-100`) —
  drives the production coordinator (`TickSimDay(targetDay)`), can jump days; the
  correct vehicle to pin Phase A end-to-end.
- Panel route plumbing: `PanelRegistryBootstrap.R(id, displayName, group, tags)`
  (`:191`); `PanelRegistry.ConfigureActions(bindAction, openAction, closeAction)`
  consumed at `Main.PlayerSurfaces.cs:520-535`; `IBindablePanel`
  (`IsBound` + `Unbind()`, `src/UI/IBindablePanel.cs`).
- Snapshot seam: `src/UI/SnapshotHarness.cs:80` `PanelCtor` target list.

# 5. Existing Extension Seams

| Seam | Location | Use in this plan |
|---|---|---|
| Runner catalog | `FactionWarChainRunner._catalog` (private) | expose via read-only `Catalog` property |
| Day-gated text widget | `src/YearOfAsh/RadioBroadcastTerminal.cs` | architectural template for the board |
| Widget binding lifecycle | `src/YearOfAsh/FactionWarMapWidget.cs:49-90` | Bind/Unbind/Refresh contract |
| Campaign day clock | `_simDay` (`Main.cs:55`), `CampaignDayCoordinator.OnDayAdvanced` (`:58-62`) | day source + optional live refresh |
| Panel registry + routes | `PanelRegistryBootstrap.cs:191`, `Main.PlayerSurfaces.cs:520-535` | route `faction_communique_board` |
| Dashboard nav | `GameDashboardPanel.cs:405` `AddNavButton` | entry point |
| Faction display names | `faction_lore.json` via `FactionsNarrativePanel.cs:60-99` loader | attribution labels |
| Headless UI selftest | `HostCli.SelfTests.cs:472-545` | `--faction-communique-board-selftest` |
| Production pipeline selftest | `Main.UiTests.RealCampaignJourney.cs` | Phase A pin |
| Snapshot pipeline | `SnapshotHarness.cs` | golden-image verification |

# 6. Proposed Architecture

## Phase A — Day-axis decision (recommended: Option C, "uncapped owner gate")

**The decision**: the war arc keeps its authored day axis (480–607) on the **campaign
calendar**, which is the single horizon authority (already uncapped). The Year-of-Ash
timeline remains a bounded *seasonal subsystem* (180–360 band: phases, temperatures,
deep-freeze/radon arcs) that self-clamps. The campaign owner stops skipping Year of
Ash ticks after day 360.

Options considered:

| Option | Verdict | Evidence |
|---|---|---|
| A. Extend `EndDay`/phases past 360 | REJECTED | touches phase enum, temperature curves, deep-freeze/radon arcs — a full seasonal-design change with wide gameplay blast radius; nothing in the authored content requires Year-of-Ash *phases* past the thaw |
| B. Re-band all war content (shift 480–607 → ≤360) | REJECTED | destroys the authored post-thaw war (~232 days of war after the thaw; ceasefire "by exhaustion" at 592), rewrites 6 data files + 40 communiqués + all cross-file day coherence + Plan 133 pins; massive data churn for zero design gain |
| **C. Uncap the owner tick gate only** | **ADOPTED** | 1-line host change; every sub-system is proven safe for `day > 360` (§4 evidence: timeline self-clamps, thermal/radon convergent, warlord idempotent); the campaign calendar is already the horizon authority; war content keeps its authored axis |

**Phase A change**: `Main.CampaignOwners.cs:1059`
`if (day >= 180 && day <= 360)` → `if (day >= 180)`.

Sub-behavior after the change (all pinned by tests, §18):
- `Timeline.CurrentDay`/phase/temperature hold at the day-360 thaw values (timeline
  self-clamps — by design, not by accident).
- Deep-freeze/radon hold their converged post-thaw states.
- `FactionWarSystem.SimulateDailyFriction` keeps running (tension saturates at its
  clamp; territorial-clash events continue every 15 days — existing sim semantics,
  watch item §21).
- The war chain runner receives true campaign days and its **day-gated** triggers
  become live for the 480–607 band. (Player-visit triggers remain dormant — location
  visits are never recorded at runtime. That is the separate war-surfacing task, §22.)

**Scope note**: the communiqué board (Phase B) does NOT depend on Phase A — it keys to
`_simDay`, not the runner. Phase A settles the clock contract so Phase B's day source
is a decision, and unblocks all future war surfacing. The phases are ordered A→B for
coherence, but B is not blocked by A.

## Phase B — Communiqué surface

A thin, code-built panel — **`FactionCommuniqueBoardPanel`** (`src/UI/`), implementing
`IBindablePanel`:

- **Data**: `_yearOfAsh.WarRunner.Catalog` (new accessor) — reads only
  `Communiques`, filtered client-side `c.day <= day` in file order. No new Core query
  API needed (the read-only list + a pure presentation filter is the minimal path).
- **Day source**: campaign day passed at Bind (`_simDay`); refreshed on `Open()`;
  optional live refresh on `CampaignDayCoordinator.OnDayAdvanced` while bound
  (subscribe in Bind, unsubscribe in Unbind — mirrors `FactionWarMapWidget`).
- **Projection discipline**: the panel code may reference ONLY `id`, `day`,
  `factionId`, `title`, `body`. It must not contain the string `authorNote`
  (Plan 133 source-scan gate enforces this mechanically).
- **Attribution**: `faction_lore.json` `display_name` lookup (reuse the
  `FactionsNarrativePanel` loader pattern, fallback to raw faction id if lore is
  missing).
- **Layout**: `AshfallDashboardShell`-style scrollable list (UI-17/overflow rules):
  one card per entry — `[DAY 537] THE CENTRAL GARRISON — "Continuity of Distribution,
  Formally Secured"` header + full `body` with word-wrap. Newest last (file order is
  chronological; consider reverse-chronological display so latest statement tops the
  list — decided at implementation; must be deterministic either way).
- **Truthful empty states** (UI-19 lesson): unbound → "District wire not connected."
  Bound, day < first entry → "No communiqués have been issued yet as of day N."
  Never fixtures.
- **No seen-state**: rendering is a pure function of (catalog, day). No marks, no
  cooldowns, no persistence, no history.

# 7. Ownership Matrix

| Concern | Owner |
|---|---|
| Communiqué content + day/faction/chain ids | `faction_war_communiques.json` (data authority) |
| Catalog load + DTOs + query APIs | `FactionWarContentCatalog` (Core, unchanged) |
| Loaded-catalog exposure | `FactionWarChainRunner.Catalog` (Core, new read-only property) |
| War arc day-axis horizon | campaign calendar (`CampaignDayCoordinator`) — decision + 1-line host wiring |
| Seasonal band (phases/temps) | `YearOfAshTimelineSystem` (unchanged; self-clamping is owned behavior) |
| Panel presentation, binding, empty states | `FactionCommuniqueBoardPanel` (Godot, new) |
| Route/nav registration | `PanelRegistryBootstrap` + `Main.PlayerSurfaces` + `GameDashboardPanel` |
| Display names | `faction_lore.json` (data authority) |
| authorNote exclusion | Plan 133 xUnit gates (+ audit allowlist entry, §10) |
| Verification | xUnit suite + `--faction-communique-board-selftest` + journey extension + snapshot |

No state ownership is created (no new persistent state).

# 8. Data Flow

```
faction_war_communiques.json
  → FactionWarContentCatalogLoader (existing)
  → FactionWarContentCatalog (inside FactionWarChainRunner, now exposed)
  → [campaign day: _simDay — passed at Bind/Refresh]
  → FactionCommuniqueBoardPanel (presentation filter: c.day <= day, file order)
  → player reads institutional claims
```

No CORE MUTATION exists in this flow — read-only projection. The only mutation in the
whole plan is Phase A's tick call widening (host wiring), which mutates existing
systems exactly as they already do inside 180–360.

# 9. State Model

**No new state.** The panel holds only bound-session + last-render-day references
(volatile, not persisted). The runner, timeline, friction states are pre-existing.
Explicit non-goal: seen/unlocked tracking (Plan 133 §23.13).

# 10. API / Contracts

1. **Core — `FactionWarChainRunner`**: add
   `public FactionWarContentCatalog Catalog => _catalog;` (read-only). Justification:
   the loaded catalog is otherwise unreachable; single consumer (host presentation);
   no new abstraction.
2. **Host — `FactionCommuniqueBoardPanel`** (new):
   - `bool IsBound { get; }` / `void Unbind()` (IBindablePanel)
   - `void Bind(YearOfAshHostSession session, int campaignDay)`
   - `void Open()` / `event Action? OnClose` (panel convention)
   - `void RefreshView()` (re-renders from bound session + day; idempotent)
3. **Host — Main** (wiring only): panel field, construction in the factions-panel
   family, `ConfigureActions("faction_communique_board", bind/open/close)` with
   bindAction lazy-invoking `SetupYearOfAsh()` (skill_matrix precedent,
   `Main.PlayerSurfaces.cs:533`).
4. **Core — `PanelRegistryBootstrap`**: `R("faction_communique_board",
   "Faction Communiqués", PanelGroup.Secondary, new[]{ "factions" })`.
5. **Test — gate amendment**: `AuthorNote_NoPlayerFacingConsumers_Gate` gains an
   explicit audit allowlist for exactly one file — the new
   `HostCli.FactionCommuniqueSelfTests.cs` — marked with an `AUTHORNOTE_AUDIT:`
   comment (mirrors the `DETERMINISM_ALLOWLIST` convention). Rationale: the gate's
   contract is "no player-facing consumer"; a selftest that reads the field ONLY to
   assert non-render is an auditor, not a consumer. The allowlist is one entry,
   commented, and fails CI if the file renames.

# 11. Data Changes

**None.** No schema changes, no new JSON, no ID changes. The corpus is complete
(Plan 133). `faction_lore.json` is consumed read-only (it already contains all four
`display_name` resolutions).

# 12. Save/Load

**No save-section changes.** The new catalog property is read-only; the panel has no
state; Phase A changes no DTO. `year_of_ash` v4 envelope and
`FactionWarChainRunnerState` are untouched. Old saves remain valid trivially
(`--year-of-ash-save-selftest` stays green). Content reload is the existing catalog
reload at session creation.

# 13. Determinism

- Render path: pure day-arithmetic over file order (chronological, pinned by
  `Existing18Ids_RemainInChronologicalFileOrder`). No RNG, no time reads, no
  culture-sensitive ordering. If reverse-chronological display is chosen, it is a
  deterministic reversal of the same list.
- Phase A: tick order unchanged (same owner, same phase); day-arithmetic only.
- No seeded RNG is introduced anywhere; none is needed.

# 14. System / Event Wiring

- Phase A: no new events. Existing `OnTerritorialClashOccurred` / standing events
  continue; friction saturation is pre-existing behavior (watch item §21).
- Phase B: panel subscribes to `CampaignDayCoordinator.OnDayAdvanced` in `Bind`,
  unsubscribes in `Unbind` (weak, optional live refresh; failure-safe: refresh also
  happens on `Open()` so a missed event never causes permanent staleness).
- No hidden bidirectional dependencies: the panel reads; nothing reads the panel.

# 15. Godot Integration

- `src/UI/FactionCommuniqueBoardPanel.cs` — code-built (no `.tscn`), consistent with
  the dominant panel pattern; `AshfallUiHelpers` chrome; scroll container; word-wrapped
  bodies (10–11px small text is an accessibility concern — follow the body-size tokens
  flagged in UI-23, do not introduce new sub-11px text).
- Route: registry id + ConfigureActions + one dashboard nav button
  (`AddNavButton(content, "COMMUNIQUÉS", "faction_communique_board")`) — nav is added
  in the same phase as the working route (UI-09: never advertise a dead route).
- Keyboard/close: standard `OnClose` + Esc handling per panel contract (UI-16 list
  discipline: add the panel to `CloseAllOverlayPanels` and `AnyOverlayPanelOpen` if it
  is overlay-classed — it is a secondary panel like `factions_narrative`; follow that
  sibling's lifecycle treatment).
- Fixture replacement: `FactionsPanel.cs:391-399` hardcoded
  "RECENT DIPLOMATIC COMMUNIQUES" rows → real day-gated top entries (or truthful
  empty), from the same bound session (FactionsPanel already receives
  `YearOfAshHostSession` — warlord selftest proves the binding path).

# 16. Narrative / Content Integration

The communiqué corpus IS the narrative content; the surface adds no new hooks. Radio
(`faction_war_radio.json`) remains un-surfaced and is explicitly out of scope (§22).
No quest/flag/journal hooks are added — communiqués are claims, never world truth
(Plan 133 invariant 3.1 preserved by rendering text verbatim as institutional claims).

# 17. Failure Modes

| Failure | Expected behavior |
|---|---|
| Catalog files missing | loader warns, empty catalog → board renders truthful empty state (no crash) |
| Session null / panel opened before campaign | `IsBound=false` → "District wire not connected." — never fixtures |
| Day < 489 (standard campaign) | truthful "No communiqués issued yet as of day N" |
| Day 1 (campaign start) | same empty state; no negative/zero-day logic needed (`c.day <= day` handles) |
| 40 long bodies in one panel | scroll container; deterministic order; no truncation (verbatim institutional claims) |
| Faction missing from faction_lore | fallback to raw `factionId` label (truthful) |
| Save/load mid-session | no panel state exists; re-bind on open |
| Host reload | SetupYearOfAsh idempotent; re-bind path identical |
| Extended play day 361–478 | board empty (first entry 489); Phase A ticking runs friction/warlord safely (convergent states) |
| Duplicate ids / bad data | pre-blocked by Plan 133 xUnit gates |
| Very large day values | day arithmetic is int-compare; timeline clamps itself; no overflow concerns below int.MaxValue |
| War chain events never fire (no visit recording) | BY DESIGN for this scope: communiqués are day-gated public claims, identical in kind to the day-gated radio broadcasts; the world's public record does not require the player to have witnessed the event (chain surfacing is the separate war-surfacing task) |
| Selftest environment without data dir | selftest skips data-dependent asserts with explicit PASS/FAIL message (warlord selftest precedent passes a real data dir) |

# 18. Test Strategy

**Core xUnit (extend the Plan 133 file + new small file):**
1. `Catalog` accessor returns the same instance for a constructed runner
   (`Assert.Same`), and a runner built with an empty catalog still loads (no crash).
2. Phase A Core pins (new test): `YearOfAshTimelineSystem.AdvanceDay(500)` →
   `CurrentDay == 360`, phase Phase6 (clamped); `DeepFreeze.TickDailyThermal(500, t)`
   and `Radon.TickDailyRadon(500, t)` converge/idempotent across repeated calls;
   `FactionWarChainRunner.TickDay(500)` surfaces day-gated stages without exception
   (likely covered by existing runner tests — extend only if not).
3. Gate amendment: authorNote source-scan allowlist — one entry, assert it matches
   exactly one file with the `AUTHORNOTE_AUDIT:` marker.

**Host headless selftest** — `--faction-communique-board-selftest`
(`HostCli.FactionCommuniqueSelfTests.cs`, warlord pattern):
- fresh session + panel `_Ready()` → `Bind` → `Open` at day 500: panel visible;
  known title rendered ("Continuity of Distribution, Formally Secured"); known body
  fragment rendered; attribution label contains "The Central Garrison".
- day 100: truthful empty-state string rendered.
- unbound: "District wire not connected."
- **Non-leak audit** (the allowlisted read): walk all rendered `Label` texts at day
  600; assert NONE contains any `authorNote` text from the catalog (normalized).
- refresh idempotence: `RefreshView()` twice; no exceptions; no duplicated rows.

**Phase A host pin** — extend `--real-campaign-journey-selftest`:
- jump the production coordinator to day 500 (`TickSimDay(500)` pattern already
  proven); assert `_campaignDay.Calendar.CurrentDay == 500`,
  `_yearOfAsh.Timeline.CurrentDay == 360` (timeline holds its band),
  war runner ticked without exception, and SaveAll+reload round-trips the year-of-ash
  envelope cleanly after extended-play ticks.

**Snapshot**: add `faction_communique_board_default` target; capture baseline.

**Gates**: `--data-integrity-selftest`, `--content-utilization-selftest` (unchanged
consumers), full dotnet suite, `dotnet build Ashfall.csproj` 0/0,
`run-gates.py --tier fast`.

# 19. Dependency-Ordered Phases

**Phase 0 — Baseline verification** (why: lock assumptions before any change)
- run full suite + the 5 canonical gates; snapshot the working-tree state.
- Files: none. Gate: all green (or concurrent-stream failures documented).
- Must NOT touch: anything.

**Phase 1 — Day-axis decision wiring (Phase A)**
- `Main.CampaignOwners.cs:1059`: drop `&& day <= 360`. Why: the decided horizon
  contract (campaign calendar owns the horizon; timeline owns its seasonal band).
- Add Core pin tests (§18.2). Extend journey selftest (§18).
- Files: `src/Main.CampaignOwners.cs`, `Ashfall.Core.Tests` (pin tests),
  `src/Main.UiTests.RealCampaignJourney.cs`.
- Gate: journey selftest green; timeline-hold pin green; suite green.
- Must NOT touch: timeline clamps, phase enum, war content data, chain triggers.

**Phase 2 — Core catalog accessor**
- `FactionWarChainRunner`: add `Catalog` property. Why: smallest read-only exposure;
  the loaded catalog is otherwise unreachable.
- Tests: accessor identity pin (§18.1).
- Gate: Core build + suite green.
- Must NOT touch: runner state/behavior, save codecs.

**Phase 3 — Panel implementation**
- `src/UI/FactionCommuniqueBoardPanel.cs` (CREATE): code-built, IBindablePanel,
  Bind(session, day), RefreshView, truthful empty states, lore display names,
  authorNote-free field projection.
- Files: one new file.
- Gate: compiles; no gameplay logic in panel (Invariant 5); grep proves zero
  `authorNote` references in src outside the allowlisted selftest.

**Phase 4 — Route + nav wiring**
- `PanelRegistryBootstrap.R(...)`, `Main.PlayerSurfaces.ConfigureActions(...)`
  (bind lazy-calls `SetupYearOfAsh()`; passes `_simDay`),
  dashboard `AddNavButton`, UI-16 close-list membership.
- Gate: route → bind → visible → content contract exercised by the selftest.

**Phase 5 — FactionsPanel fixture replacement**
- Replace hardcoded communiqué card rows with real day-gated rows from the bound
  session (warlord selftest constructs this panel — must stay green).
- Gate: `--warlord-ui-selftest` green; no fixture text remains in that card.

**Phase 6 — Headless selftest + gate allowlist + snapshot**
- `HostCli.FactionCommuniqueSelfTests.cs` (CREATE) + CLI verb registration +
  authorNote gate allowlist entry + snapshot target + baseline capture.
- Gate: selftest PASS; gate test green; snapshot manifest updated.

**Phase 7 — End-to-end verification**
- All canonical gates + fast tier; commit per phase (small reversible commits).

No data phase, no save phase (nothing to do — documented in §11/§12).

# 20. File Impact Map

| File/area | Action | Reason | Risk |
|---|---|---|---|
| `src/Main.CampaignOwners.cs` | MODIFY (1 line) | Phase A tick-gate uncapping | Medium — changes extended-play ticking; pinned by tests |
| `Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs` | MODIFY (1 property) | expose loaded catalog | Low — read-only accessor |
| `src/UI/FactionCommuniqueBoardPanel.cs` | CREATE | the board | Low — new, isolated |
| `Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs` | MODIFY (1 R entry) | route registration | Low |
| `src/Main.PlayerSurfaces.cs` | MODIFY | ConfigureActions + panel field | Low |
| `src/UI/GameDashboardPanel.cs` | MODIFY (1 nav button) | entry point (with working route, per UI-09) | Low |
| `src/UI/FactionsPanel.cs` | MODIFY (fixture card only) | replace fake communiqué rows with real data | Medium — shared fixture-heavy panel; warlord selftest guards |
| `src/Host/HostCli.FactionCommuniqueSelfTests.cs` | CREATE | headless verification | Low |
| `src/Host/HostCli.cs` | MODIFY (verb registration) | selftest entry | Low |
| `Ashfall.Core.Tests/FactionWarCommuniqueExpansionTests.cs` | MODIFY | gate allowlist (audited, one entry) | Low — documented rationale |
| `Ashfall.Core.Tests` (Phase A pin tests) | CREATE/MODIFY | timeline/thermal/radon/runner extended-day pins | Low |
| `src/Main.UiTests.RealCampaignJourney.cs` | MODIFY | extended-play end-to-end pin | Low |
| `src/UI/SnapshotHarness.cs` + snapshot manifest | MODIFY | new golden target | Low |
| `Assets/StreamingAssets/Data/**` | READ ONLY | no data changes | — |
| `faction_war_communiques.json` | READ ONLY | corpus complete (Plan 133) | — |
| Save codecs / stores | READ ONLY | no state added | — |

# 21. Risks

- **MEDIUM** — Phase A alters extended-play simulation envelope (friction saturation,
  territorial-clash events continue past 360; warlord ticks continue). Evidence-based
  safe (§4) but must be pinned by the journey extension before merge. If the owner
  wants extended play to remain Year-of-Ash-silent, Phase A can be deferred WITHOUT
  blocking Phase B (the board keys to `_simDay`).
- **MEDIUM** — FactionsPanel is fixture-heavy and concurrently touched; the fixture
  replacement must be a surgical card-level change guarded by the warlord selftest.
- **LOW-MEDIUM** — the authorNote gate allowlist is a contract-test change; it must
  remain a single explicit, commented entry (review-checked) or it erodes the gate.
- **LOW** — panel text volume (40 entries) → scroll/overflow discipline (UI-17/23
  rules); snapshot catches layout drift.
- **LOW** — route/nav added only when the panel works end-to-end (UI-09).

# 22. Out of Scope

1. **War chain surfacing** (location-visit recording, `OnStageSurfaced` UI, choice
   modals, `ResolveWarChoice` wiring) — a separate war-surfacing plan; the board
   deliberately does not depend on it (§17 failure-mode row).
2. **`faction_war_radio/journal/dialogue/location-overrides` surfaces** — same
   pattern family, separate tasks.
3. **FactionDetailPanel "dispatches/intelligence" fixtures** — different fixture
   family (broader than communiqués); own task.
4. **Timeline EndDay/phases extension** (Option A) — rejected with evidence.
5. **Content re-banding** (Option B) — rejected with evidence.
6. **Demo-button day cap** (`OnTickYearOfAshClicked` Math.Min(360,…)) — dev tool, out
   of scope.
7. **Seen-state/cooldowns/persistence for communiqués** — prohibited (Plan 133 §23.13).
8. **Tension rebalancing for the 361–480 gap** — existing sim semantics; balance is a
   separate tuning task.
9. The stale radio-terminal refresh during live campaign days — pre-existing, separate.

# 23. Rollback Strategy

- Every phase is one small reversible commit (plan phases 1–7 map 1:1).
- Phase A rollback: restore the gate line — no data/state migration involved; extended
  saves written while uncapped remain valid (runner state schema unchanged; any chain
  progress beyond 360 loads fine because the state DTO has no band assumptions).
- Phase B rollback: remove panel/route/nav/selftest files + revert the two 1-line
  Core/test edits; no save impact either direction.
- Gate allowlist rollback: single revert restores the strict gate.
- Test checkpoint: full suite + canonical gates after every phase before continuing.

# 24. Definition of Done

- The day-axis decision is documented (this plan), wired (Phase A), and pinned by
  tests including the extended-play journey assertion.
- `faction_communique_board` route → bind → visible → real content → truthful empty
  states all work through the normal dashboard nav path.
- `--faction-communique-board-selftest` PASSes including the authorNote non-leak audit;
  both Plan 133 authorNote gates remain green.
- The FactionsPanel communiqué card renders real day-gated data.
- Snapshot baseline captured; all canonical gates + fast tier green.
- No new persistent state, no schema changes, no event-chain ownership changes, no
  selector/history subsystem.

# 25. Implementation Handoff

## MUST PRESERVE
- All 40 communiqués and their file order; every Plan 133 xUnit contract (18 anchors,
  chronology, branch gates, authorNote gates, coverage/allocation pins).
- Year-of-Ash timeline band semantics (180–360 self-clamp is owned behavior).
- RadioBroadcastTerminal/FactionWarMapWidget behavior (templates only, not touched).
- Plan 133 invariant 3.1: rendered `body`/`title` are institutional CLAIMS — render
  verbatim, never annotate with truth status (that information lives only in
  `authorNote` and must stay out of every player surface).

## MUST ADD
- `FactionWarChainRunner.Catalog` read-only property (Core).
- Owner tick gate uncapping + extended-day pins + journey extension.
- `FactionCommuniqueBoardPanel` (field-projection: `id`/`day`/`factionId`/`title`/
  `body` only; lore display names; truthful empty states; no seen-state).
- Route `faction_communique_board` + dashboard nav + UI-16 close-list membership.
- `--faction-communique-board-selftest` + one-entry authorNote audit allowlist.
- FactionsPanel fixture-card replacement with real data.
- Snapshot target + baseline.

## MUST NOT DO
- Reference `authorNote` anywhere in `src/` except the single allowlisted audit
  selftest (gate-enforced).
- Add seen-state, cooldowns, persistence, or a communiqué history subsystem.
- Change event-chain ownership, trigger grammar, runner state schema, or any save
  section.
- Extend timeline phases/EndDay, or re-band any war content days.
- Replace fixtures with new fixtures; render invented data on unbound/empty paths.
- Touch concurrent-stream files outside the exact hunks listed in §20.

## VERIFY WITH
```
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --content-utilization-selftest
godot --headless --path . -- --faction-communique-board-selftest
godot --headless --path . -- --real-campaign-journey-selftest
godot --headless --path . -- --year-of-ash-save-selftest
godot --headless --path . -- --warlord-ui-selftest
python3 scripts/ci/run-gates.py --tier fast
```

## FIRST SAFE IMPLEMENTATION STEP
Phase 2 — the one-line `Catalog` accessor in `FactionWarChainRunner` with its
identity pin test. It is reversible, touches no behavior, unblocks the panel, and
fails loudly if the runner's constructor contract ever changes.
