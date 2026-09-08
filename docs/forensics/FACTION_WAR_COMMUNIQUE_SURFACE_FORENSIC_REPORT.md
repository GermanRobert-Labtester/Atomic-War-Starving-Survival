# Faction War Communiqué Player-Facing Surface — Forensic Report

Target of analysis: the missing player-facing renderer for
`faction_war_communiques.json` (Plan 133 §15 finding) — a day-gated surface over
`FactionWarContentCatalog.GetCommuniquesForFaction`, with `authorNote` provably
non-player-facing, no seen-state/history subsystem, no event-chain ownership changes.

---

## 1. Target

**Target capability**: render the 40 authored faction-war communiqués to the player,
day-gated, attributed by faction, in the existing Godot UI system — as institutional
public statements (not radio, not journal).

**Likely domains/synonyms searched**: communiqué, dispatch, bulletin, notice board,
faction statement, faction war UI, diplomacy panel, radio intercept, Year of Ash,
`GetCommuniquesForFaction`, `FactionWarContentCatalog`.

**What counts as equivalent**: any existing panel that renders catalog content by day
(`RadioBroadcastTerminal`), any `YearOfAshHostSession`→widget binding
(`FactionWarMapWidget`), any `IBindablePanel` + PanelRegistry route
(`FactionsNarrativePanel`), any day-gated unlock surface keyed off the campaign
calendar.

## 2. Executive Finding

1. **No player-facing surface exists for ANY faction-war narrative catalog** —
   communiqués, radio (faction_war), journal, dialogue and location overrides have
   zero non-test consumers. The only query API consumers are xUnit tests.
   Evidence:
   - `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs:117-129` (`GetCommuniquesForFaction`)
   - repo-wide grep: callers only in `Ashfall.Core.Tests/FactionWarContentCatalogTests.cs`,
     `FactionWarCommuniqueExpansionTests.cs`, `FactionWarDialogueExpansionTests.cs`
2. **The loaded catalog is unreachable from the host**: `FactionWarContentCatalogLoader`
   loads all six files into a `FactionWarContentCatalog` at
   `YearOfAshHostSession.Create` (`src/YearOfAsh/YearOfAshHostSession.cs:146-148`),
   which is handed to `FactionWarChainRunner` — whose `_catalog` field is **private
   with no public accessor** (`Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs:307,314`).
3. **Headline blocker (CRITICAL for planning)**: the faction-war content day axis
   (chains 480–607, communiqués 489–607, radio 480–600, journal 482–606) exceeds every
   runtime clock that could gate it:
   - `YearOfAshTimelineSystem` hard-clamps `StartDay=180, EndDay=360`
     (`Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs:35-36,62-63,169`);
   - the campaign day owner ticks Year of Ash only `if (day >= 180 && day <= 360)`
     (`src/Main.CampaignOwners.cs:1059-1062`);
   - the campaign calendar itself has **no day cap**
     (`Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs`, `CampaignCalendar(initialDay: 1)`),
     so extended play continues past 360 — but the war runner is never ticked there,
     and `Timeline.CurrentDay` freezes at 360.
   Consequence: **the war event chains never fire in any campaign**, and a communiqué
   renderer keyed to `Timeline.CurrentDay` can never display a single entry. A renderer
   keyed to the campaign calendar (`_simDay`, `src/Main.cs:55`) could display entries in
   extended play — but would then show statements about events whose chains never
   surfaced at runtime (coherence question for the planner, §18).
4. **A complete, safe template exists**: `src/YearOfAsh/RadioBroadcastTerminal.cs` is a
   thin, day-gated, no-seen-state, fixture-free widget rendering catalog text
   (`LoadBroadcasts(dataDir)` + `RefreshView(currentDay)`, day-gated filter, truthful
   empty state, memory-only audio-dedup that is NOT a seen-state). It is the exact
   architectural pattern to copy for a communiqué surface.
5. `authorNote` is provably leak-free today (zero consumers) and is mechanically
   guarded by two Plan 133 tests — including a **source-scan gate that fails if any
   `src/**.cs` file contains the string `authorNote`**
   (`Ashfall.Core.Tests/FactionWarCommuniqueExpansionTests.cs`, `AuthorNote_NoPlayerFacingConsumers_Gate`).
   A future renderer therefore cannot even reference the field by name — it must be
   built by field projection (`title`/`body`/`factionId`/`day` only), never
   copy-then-strip.

## 3. Evidence Summary

| Claim | Evidence |
|---|---|
| Communiqués loaded into the runner's private catalog | `src/YearOfAsh/YearOfAshHostSession.cs:146-149`; `FactionWarChainRunner.cs:307-317` |
| No query API consumer outside tests | repo grep for `GetCommuniquesForFaction`/`GetBroadcastsForDay`/`GetJournalForDay`/`GetDialogueForLocation` |
| War runner ticked only in 180–360 window | `src/Main.CampaignOwners.cs:1059-1062` (`NarrativeQuestsVerdictDayOwner`) |
| Timeline clamps 180–360 | `YearOfAshTimelineSystem.cs:35-36,60-66,169` |
| Campaign calendar uncapped | `CampaignDayCoordinator.cs:28,94-155`; `_simDay => _campaignDay?.Calendar?.CurrentDay ?? 1` (`src/Main.cs:55`) |
| Runner events (`OnStageSurfaced`/`OnStageResolved`/`OnChainResolved`) have zero src subscribers | `FactionWarChainRunner.cs:311-313`; grep — no `src/` subscription |
| Runner choice/visit APIs dead at runtime | `RecordWarLocationVisited` / `ResolveWarChoice` (`YearOfAshHostSession.cs:216,221`) have zero `src/` callers |
| Radio terminal is the day-gated template | `src/YearOfAsh/RadioBroadcastTerminal.cs:48,60-76,79-124` |
| Faction war widget binding template | `src/YearOfAsh/FactionWarMapWidget.cs:49-90` (`BindSession`/`UnbindSession`/`RefreshView`, event-driven refresh) |
| Year-of-Ash widget column exists in main HUD | `src/Main.YearOfAsh.cs:246-278` (`BuildYearOfAshPanel`, `_rightColumn`) |
| Panel route registry pattern | `Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs:43-44,123-124`; `src/Main.PlayerSurfaces.cs:520-535` (`ConfigureActions`) |
| Headless UI selftest template | `src/Host/HostCli.SelfTests.cs:472-545` (`RunWarlordUiSelfTest`) |
| Snapshot verification seam | `src/UI/SnapshotHarness.cs:80` (`PanelCtor` targets) |
| authorNote leak gates | `Ashfall.Core.Tests/FactionWarCommuniqueExpansionTests.cs` (`AuthorNote_NeverAppearsInPlayerFacingText`, `AuthorNote_NoPlayerFacingConsumers_Gate`) |
| Thematic but fixture-only equivalents | `src/UI/FactionsPanel.cs:393-399,459` (hardcoded "communiqués" rows); `src/UI/FactionDetailPanel.cs:98-105` (hardcoded "RECENT DISPATCHES" card) |

## 4. Architecture Placement

- **Core (engine-agnostic)**: `FactionWarContentCatalog` (+DTOs, loader, query APIs) —
  already complete; nothing missing for a renderer.
- **Godot host**: `YearOfAshHostSession` owns construction, day ticking, save; `Main`
  owns UI construction/routes; `src/YearOfAsh/*` owns widget presentation.
- **Data**: `faction_war_communiques.json` (40 entries, `schema_version: 1`).
- **Tests**: `Ashfall.Core.Tests` (catalog, expansion contract, authorNote gates).
- A communiqué surface belongs in the **Godot host presentation layer** reading
  Core query APIs; Core needs at most one additive accessor (§14).

## 5. Current Implementation (classification)

| Capability | Class | Notes |
|---|---|---|
| Communiqué data + catalog + query API | **LIVE_CORE** (loaded) / effectively **DATA_ONLY** (no runtime consumer) | Loaded into runner at `Create`; never queried outside tests |
| Faction war chain runner | **LIVE_CORE** (ticked) but narrative output dead | Ticked 180–360 in live campaign; stage events fire into the void (no subscribers) |
| War location visits / choices | **STUB at runtime** | host APIs exist (`RecordWarLocationVisited`, `ResolveWarChoice`) — zero callers |
| `RadioBroadcastTerminal` (Year of Ash radio) | **LIVE_GODOT** | different file (`year_of_ash_radio.json`, days 180+), same pattern |
| `FactionWarMapWidget` | **LIVE_GODOT** | sim numbers only (tension/standing/territory), no narrative text |
| FactionsPanel / FactionDetailPanel "communiqués/dispatches" sections | **DUPLICATED fixtures** | hardcoded text, not catalog consumers (AGENTS UI-19 family) |
| authorNote protection | **VERIFIED via tests** | two mechanical gates |
| Any communiqué renderer | **genuinely missing** | re-searched via names/synonyms — none |

## 6. Runtime Wiring

- `Main.SetupYearOfAsh()` (`src/Main.YearOfAsh.cs:99-127`) → `YearOfAshHostSession.Create(_dataDir)`:
  loads all six faction-war catalogs via `FactionWarContentCatalogLoader`
  (`YearOfAshHostSession.cs:146-148`) → `FactionWarChainRunner(warCatalog)` → `WireWarRunner()`
  (only `StandingDeltaApplier`, `YearOfAshHostSession.cs:62-68`).
- Campaign daily pipeline: `CampaignDayCoordinator` (phase-ordered owners) →
  `NarrativeQuestsVerdictDayOwner.TickDay` ticks `_yearOfAsh.TickDay(day)` **only for
  day 180–360** (`src/Main.CampaignOwners.cs:1059-1062`).
- `YearOfAshHostSession.TickDay` (`:160-167`): `Timeline.AdvanceDay` (clamped 180–360)
  → `FactionWar.SimulateDailyFriction` → `WarRunner.TickDay(day)`.
- War stage surfacing: runner raises `OnStageSurfaced` — **no host subscriber**
  (grep: zero subscriptions outside the runner itself).
- UI: `BuildYearOfAshPanel` (`Main.YearOfAsh.cs:246-278`) constructs
  `FactionWarMapWidget` + `RadioBroadcastTerminal` into the right-column HUD panel;
  radio terminal refreshed only at build time and from the demo tick button
  (`Main.YearOfAsh.cs:277,421`) — it also goes stale during live campaign days
  (no per-day refresh subscriber).

**Who creates / calls / observes**: catalog created by session factory; queried by
nobody; observed by nobody. The renderer gap is total, not partial.

## 7. Data Flow

`faction_war_communiques.json` → `FactionWarContentCatalogLoader.LoadCommuniques`
(`FactionWarContentCatalog.cs:406-421`, tolerant per-file, skips null/empty `id`) →
`FactionWarCommunique` DTOs → `catalog.GetCommuniquesForFaction(factionId, day)`
(`:117-129`, ordinal faction match, `c.day <= day`, file order) → **end of chain**.

Ordering: query returns file order; the shipped file is chronologically sorted
(pinned by `FactionWarCommuniqueExpansionTests.Existing18Ids_RemainInChronologicalFileOrder`).
No selector, grouping, dedup, or persistence exists — by design (§15/§23.13 of Plan 133).

## 8. State Ownership

- Communiqués are immutable content: no runtime state, no seen-state, no cooldowns.
  A renderer must remain a pure function of (catalog, currentDay) — matching the
  RadioBroadcastTerminal model.
- Runner state (`FactionWarChainRunnerState`, schemaVersion 1) owns chains/visits/
  morale/producedFlags only — communiqués are not in it and must not be added
  (no seen-state subsystem constraint).
- The radio terminal's `_playedAudioBroadcasts` is memory-only audio de-dup, not a
  persisted seen-state — an acceptable pattern only because it never gates what is
  *displayed*, only which voice-over re-fires. A communiqué surface needs nothing
  similar.

## 9. Save/Load

- Campaign envelope: `year_of_ash` v4 carries `factionWar` +
  `factionWarChainRunner` (`YearOfAshSave.cs:26,37,59,64,81,97,116,133,139-147`;
  capture/restore at `YearOfAshHostSession.cs:285-305`).
- Adding a renderer changes **no** save section. The only candidate Core change
  (exposing the catalog, §14) is a read-only accessor with zero save impact.
- `--year-of-ash-save-selftest` covers envelope round-trip; content reload is a plain
  catalog reload.

## 10. Determinism

- Catalog + query are pure day-arithmetic, ordinal comparisons, file order — no RNG,
  no time, no culture-sensitive sorting anywhere in the path.
- A renderer copying the RadioBroadcastTerminal pattern inherits this (rendering
  order = file order). Any future "select one communiqué of the day" logic would need
  deterministic selection (index or seeded via `ISeededRng` fork) — flagged for the
  planner; not required by the corpus contract.
- UI text: communiqués are catalog strings; only panel chrome labels are hardcoded
  English (consistent with the current non-localized UI tree).

## 11. UI / Player Feedback

Player-facing chain today: **none** for communiqués. Adjacent surfaces:
- `FactionWarMapWidget` — sim state numbers only (tension bar, standing rows).
- `RadioBroadcastTerminal` — Year of Ash radio (days 180+, different catalog).
- `FactionsPanel` "RECENT DIPLOMATIC COMMUNIQUES" / FactionDetailPanel "RECENT
  DISPATCHES" — hardcoded fixture rows (UI-19 family); do NOT consume this catalog.
- `_codexViewer` — text-dump surface (`Main.YearOfAsh.cs:423-433`) — a possible
  minimal-integration host, but panels are the sanctioned pattern.

## 12. Tests & Verification

| Test | Proves |
|---|---|
| `FactionWarContentCatalogTests` (8) | catalogs load; query day/faction filtering; faction ids resolve |
| `FactionWarCommuniqueExpansionTests` (29, Plan 133) | 40 entries; strict chain FK; chronology; branch gates; authorNote non-leak (cross-surface containment + src source-scan); loader failure policies; coverage/allocation/repetition pins |
| `DataWiringIntegrationTests` | the six files load together from the real data dir |
| `--year-of-ash-save-selftest` | Year of Ash save round-trip (runner state included) |
| `--warlord-ui-selftest` | headless widget+panel construction/bind/open pattern — the template for a communiqué UI selftest |
| `--content-utilization-selftest` | file is consumer-mapped (`FactionWarContentCatalog`) — stays green |

**Nothing today proves any communiqué is perceivable by a player** — that is the gap.

## 13. Duplicates / Legacy / Forks

- `FactionsPanel.cs:393-399` + `FactionDetailPanel.cs:98-105` — fixture "communiqués/
  dispatches" text that *thematically* duplicates the real corpus; a real surface
  should replace these fixtures at their homes or be a distinct panel (owner's
  no-relabel rule applies).
- `year_of_ash_radio.json` vs `faction_war_radio.json` — two distinct radio catalogs,
  two loaders (`YearOfAshCatalogLoader.LoadRadioBroadcasts` vs
  `FactionWarContentCatalogLoader`) — intentional separation, not a fork; but their
  DTO families are near-identical in shape (frequency/source/message/signalStrength).
- No Unity-era equivalent is relevant (faction war is post-migration content).

## 14. Existing Extension Seams (where integration SHOULD attach)

1. **Catalog accessor**: `FactionWarChainRunner._catalog` is private — smallest safe
   Core change is `public FactionWarContentCatalog Catalog => _catalog;` (read-only,
   no behavior/save impact). Alternative: keep a `WarCatalog` property on
   `YearOfAshHostSession` populated at `Create` (`:146-148`).
2. **Widget template**: `RadioBroadcastTerminal` (`LoadX(dataDir)` + `RefreshView(day)`,
   scrollable log, truthful empty state) — copy this shape for a
   `CommuniqueBoard`/`FactionDispatchPanel`.
3. **Binding template**: `FactionWarMapWidget.BindSession/UnbindSession/RefreshView`
   with event-driven refresh (`OnDayAdvanced`, `OnFactionStandingChanged`).
4. **Placement**: `BuildYearOfAshPanel` right column (`Main.YearOfAsh.cs:246-278`) or a
   registered panel via `PanelRegistryBootstrap.R(...)` + `PanelRegistry.ConfigureActions`
   (`Main.PlayerSurfaces.cs:520-535` pattern; `factions`/`faction_detail`/
   `factions_narrative` groups exist).
5. **Day source**: campaign calendar `_simDay` (`Main.cs:55`) — the only clock that can
   exceed 360 — vs `Timeline.CurrentDay` (frozen ≤ 360). This choice is the central
   design decision (§2.3, §18).
6. **Refresh trigger**: `CampaignDayCoordinator.OnDayAdvanced` event
   (`CampaignDayCoordinator.cs:58-62`) — currently used for the daily briefing modal;
   a surface refreshing on it stays live during extended play (unlike the radio
   terminal today).
7. **Verification seams**: headless UI selftest following `RunWarlordUiSelfTest`
   (`HostCli.SelfTests.cs:472-545`); snapshot target via `SnapshotHarness.cs:80`;
   the Plan 133 authorNote source-scan gate must stay green (build the renderer by
   field projection, never referencing `authorNote`).

## 15. Functional Equivalents

| Candidate | Verdict |
|---|---|
| FactionsPanel "communiqués" section | A. exists — but fixture-only (hardcoded); not a consumer |
| FactionDetailPanel "dispatches" card | same — fixture-only |
| RadioBroadcastTerminal | C. partial equivalent — different catalog + band; correct *pattern* |
| Journal panel / narrative codex | C. different content surfaces (private witness vs public record) |
| FactionsNarrativePanel | D. safe extension seam — bound panel with sidebar/grid/shell; could host a communiqué sub-card; its unbound-case **fixture rows are an anti-pattern to avoid** (truthful empty states required) |

## 16. Confirmed Gaps

1. No renderer/consumer for communiqués (re-searched: names, synonyms, query API,
   file literals) — **confirmed**.
2. Loaded catalog unreachable from host (private runner field) — **confirmed**.
3. War content day axis (480–607) unreachable by all gating clocks (timeline clamp
   180–360 + owner gate ≤360) — **confirmed**; the war chains never surface in any
   campaign; even the `OnTickYearOfAshClicked` demo path caps at
   `Math.Min(360, ...)` (`Main.YearOfAsh.cs:414`).
4. War stage events (OnStageSurfaced etc.) and runner choice/visit APIs are dead at
   runtime — **confirmed** (zero subscribers/callers).
5. The FactionWarSpineAudit claim "the chain fires in extended campaigns" is
   **contradicted by current source** (the ≤360 owner gate + timeline clamp);
   treat the audit statement as stale for wiring purposes.

## 17. Risks

- **CRITICAL** — Day-axis incoherence: shipping a day-gated communiqué renderer keyed
  to `Timeline.CurrentDay` displays nothing forever; keyed to `_simDay` it displays
  statements about events whose chains never surfaced (public memory of a war the
  runtime never ran). Any plan must resolve which clock owns the war arc's horizon —
  this is a campaign-scope decision, bigger than one panel.
- **HIGH** — Fixture duplication: if the new surface merely relabels
  FactionsPanel/FactionDetailPanel fixture sections, the owner's no-relabel rule and
  UI-19 are violated; fixtures must be replaced by real binding or the new panel kept
  distinct.
- **HIGH** — authorNote leakage regression: any renderer change must keep both
  Plan 133 gates green; the source-scan gate means the renderer may not contain the
  string `authorNote` at all — enforce field projection in review.
- **MEDIUM** — Stale-render UX: copying the radio terminal's build-time-only refresh
  would leave the board static during live play; refresh on `OnDayAdvanced` instead.
- **MEDIUM** — UI-17 overflow family: any new panel must respect descendant-bounds and
  scroll requirements (40 long-form entries are ~large text volume; a scrollable log
  like the radio terminal is the proven shape).
- **LOW** — No save impact; no determinism risk in the render path.

## 18. Constraints for Planning

1. Data-only corpus is complete and contract-pinned; a surface is presentation-only
   (`IBindablePanel`-style thin widget, no simulation logic — Invariant 5).
2. No seen-state/cooldown/persistence may be added (Plan 133 §23.13 + this report §8);
   the renderer must be a pure (catalog, day) function.
3. `authorNote` must remain un-referenced in `src/` (existing gate); project fields.
4. Do not touch event-chain ownership, trigger grammar, or runner state. Resolving the
   day-axis gap may legitimately extend the *host tick wiring* (the `day <= 360` gate
   and/or timeline clamps are host/Core wiring, not chain ownership) — but that is a
   separate, explicitly approved task; a communiqué panel must not silently widen the
   war arc's horizon as a side effect.
5. Follow the RadioBroadcastTerminal shape for content rendering and the
   FactionWarMapWidget shape for session binding; register any new panel through
   PanelRegistry with a real route (UI-09 contract) and truthful empty states (UI-19).
6. Verification: headless UI selftest (RunWarlordUiSelfTest pattern) +
   `--data-integrity-selftest` + `--content-utilization-selftest` + full dotnet suite;
   add a snapshot target if the panel is snapshot-classed.

## 19. Evidence Index

- `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs:117-129,236-246,406-421`
- `Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs:255-341,415,528`
- `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs:35-36,60-66,169`
- `Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs:28-94,140-155`
- `Assets/Ashfall.Core/YearOfAsh/YearOfAshSave.cs:26,37,133-147`
- `src/YearOfAsh/YearOfAshHostSession.cs:24-75,106-148,160-167,216-221,285-305`
- `src/YearOfAsh/RadioBroadcastTerminal.cs:48-124`
- `src/YearOfAsh/FactionWarMapWidget.cs:49-90,101-137`
- `src/Main.YearOfAsh.cs:33,99-127,246-283,405-421,423-433`
- `src/Main.CampaignOwners.cs:64,1044-1062`
- `src/Main.cs:55`
- `src/Main.PlayerSurfaces.cs:520-535`
- `src/Host/HostCli.SelfTests.cs:472-545`
- `src/UI/SnapshotHarness.cs:80`; `src/UI/FactionsPanel.cs:393-399,459`;
  `src/UI/FactionDetailPanel.cs:98-105`; `src/UI/FactionsNarrativePanel.cs:418`
- `Ashfall.Core.Tests/FactionWarCommuniqueExpansionTests.cs` (authorNote gates, contract pins)
- `Ashfall.Core.Tests/FactionWarContentCatalogTests.cs`; `DataWiringIntegrationTests.cs:40-55`
- `Assets/Ashfall.Core/YearOfAsh/YearOfAshCatalogLoader.cs:133,363-394` (separate radio loader)
- `Assets/Ashfall.Core/Endgame/EndgameSystem.cs:117-164`; `EpilogueMatrixRuntime.cs:26`

## 20. Confidence & Unknowns

- **High confidence** (direct source + call-site trace): no renderer exists; catalog
  unreachable; authorNote leak-free; save impact nil; templates enumerated.
- **High confidence** on the day-axis blocker (three independent clamps/gates verified).
- **Unknown (needs product decision, not forensics)**: whether the war arc's 480–607
  axis is intended for (a) a post-360 extended-campaign mode, (b) a future calendar
  re-banding, or (c) an epilogue-matrix evaluation horizon only. The authored corpus,
  the epilogue saga horizon (360→3,650 days), and the uncapped campaign calendar all
  point to (a), but no host clock implements it today.
- **Unknown**: whether the owner wants the communiqué surface as a standalone panel,
  a FactionsPanel sub-card replacing fixtures, or a HUD column widget — all three have
  evidence-backed templates above; choice is a design decision outside read-only scope.

---

*Read-only forensic pass per ashfall-analyze. No production code or data was modified;
this report is the only file created.*
