# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> (Approved by user 2026-10-01 — "Continue with all the 5 suggested tasks!"; integrated and archived in the same session.)

# R1–R5 — route reachability wave + map data truthfulness + series commit

> **STATUS: APPROVED BY USER**
> (User-authorized 2026-10-01: "Continue with all the 5 suggested tasks!" —
> the five follow-ups proposed after the T14/T15/T16 integration.)

## Goal

1. **R1 — route reachability:** generalize the T14 finding; verify every
   player-navigable PanelRegistry route has an emitter surface, wire the
   gaps with host-panel deep links, and gate against regressions.
2. **R2 — LongWalkExpeditionPanel:** audit and resolve the "dead shell
   behind a redirect".
3. **R3 — route tags:** author the first real D16 tags on
   `wasteland_map_v1.json` routes so the T12 corridor rows carry qualitative
   conditions.
4. **R4 — real sub-layout data:** feed MapDetailPanel's sub-layout card from
   the standing-record authority (unlocked rooms) instead of always-empty.
5. **R5 — commit checkpoint:** pathspec/hunk-level commit of the UI
   truthfulness series (T10-adjacent through R4, my lineage's files).

## Evidence (premise audit, 2026-10-01)

- **R1 sweep (precise):** 222 live routes; emission = route id quoted in
  src/ excluding `PanelRegistryBootstrap.cs` (registration) and
  `Main.PlayerSurfaces.cs` (ConfigureActions wiring), or in briefing/data
  JSON. 25 candidates; verified false positives: `map_detail`,
  `faction_detail`, `quest_detail`, `fire_incident` (direct-call funnels in
  `Main.UiHandlers.cs`), `combat_hud` (auto-opened with combat at
  `Main.UiPanels.cs:754`). Genuine gaps: 11 wire-able now (hosts exist,
  unclaimed) + 8 pending foreman (claimed/blocked hosts — see gate).
- **R2:** `LongWalkExpeditionPanel` is 100% fabricated static telemetry
  ("1,240 KM", "42% CARRIER LOCK", "12.4 R/HR"), four buttons with NO
  Pressed handlers, `Bind(object?)` discards its session and forces
  `IsBound = true`. The dashboard "LONG WALK" nav already redirects to the
  real expeditions surface (`RedirectPrototypeRoute`). Verdict: RETIRE the
  mockup; the redirect route and nav stay (owned by ACTIVE foreign claims).
- **R3:** `wasteland_map_v1.json` (68 routes) authors zero tags; loader
  accepts the documented D16 vocabulary (`flooded`, `amphibious`,
  `hazard_high`) free-form. Node metadata (danger high/locked, flotilla) is
  authored truth to derive tags from.
- **R4:** `GetNodeIntel().LootDescription` is hardcoded `"Unknown"` — no
  real salvage authority exists (salvage card keeps its truthful empty
  state; yield authoring flagged as follow-up). Sub-layouts have a real
  owner: `LocationLayoutSystem` keeps `_byParent` defs
  (`standing_record_layouts.json`: 14 parents, authored rooms) and
  `State.parents[*].unlockedRoomIds` records what the player has unlocked.
- **R5:** files of my lineage are uncommitted alongside foreign dirty hunks
  (`Main.UiPanels.cs`, `CraftingPanel.cs` mixed) → hunk-level staging.

## Repair

- **R1 wiring** (host deep links, T14 pattern — event + button + one
  `OpenPlayerPanel("<id>")` wire line each):
  `map_atlas`/`maritime_atlas` → MapPanel; `faction_matrix`/
  `factions_narrative` → FactionsPanel; `survival_workstation` →
  CraftingPanel; `weather_sonde` → WeatherPanel; `quests_atlas` →
  QuestsPanel; `research_atlas` → ResearchPanel; `muster_atlas` →
  MusterPanel; `caravan_barter` → TradePanel; `geiger_calibration` →
  RadiationDetailPanel. Pending foreman (documented in gate with reasons):
  `verdict_dashboard`, `skill_matrix` (host C1-claimed), `brine_extraction`,
  `aquifer_treaty_concession`, `slurry_dewatering_sump` (natural launcher
  GameDashboardPanel pfgl-claimed), `combat_detail`, `combat_history`
  (combat seam actively foreign-dirty), `emergency_response`.
- **R2 retirement:** delete `src/UI/LongWalkExpeditionPanel.cs` (+ .uid);
  remove its field/instantiation in `Main.UiPanels.cs`, its lifecycle-list
  line in `Main.PanelLifecycle.cs` (pfgl-claimed — single dead-reference
  line), and its `ClickabilityExemptPanels` entry in
  `src/Host/HostCli.Command.RunUiLayoutSelfTest.cs`. Registry route,
  redirect, and dashboard nav unchanged (foreign-claimed, correct as-is).
- **R3 tags (data):** derive mechanically from authored node metadata —
  routes whose either endpoint has danger `high` or `locked` get
  `hazard_high`; routes touching `loc_black_flotilla_outpost` get
  `amphibious`. No other invention. Validate via the config validator and
  data-integrity selftest.
- **R4 sub-layouts:** add read-only
  `LocationLayoutSystem.GetLayoutDefinition(parentId)` (extends the current
  owner, engine-free); `OpenMapDetailPanel` resolves the location's
  unlocked rooms to display names and passes them as `subLayouts` (only
  rooms the player unlocked — no content leak); MapDetailPanel session
  overload threads the existing `subLayouts` parameter through.
- **R1 gate:** new `PanelRouteReachabilityGateTests.cs` — every
  player-navigable route must be emitted, direct-call reachable, or on the
  pinned pending-foreman list; the pending list can only shrink by explicit
  edit (new unreachable routes fail CI).

## Invariants

- Routing decisions stay in the host; panels only raise events.
- No edits to ACTIVE-claimed paths beyond the two single-line dead-reference
  removals justified above (`Main.PanelLifecycle.cs`), documented here.
- Core change limited to a read-only accessor on the existing owner.
- Tags are derived from authored node metadata only; JSON stays authority.
- Save/determinism/RNG untouched; no new save sections.

## Non-goals

- Salvage-yield authoring (no authority exists; flagged as follow-up).
- Wiring the 8 pending-foreman routes (blocked hosts; foreman decision).
- No full suite; foreign dirty hunks preserved (hunk-level staging only).

## Test plan

- New reachability gate + existing route/truthfulness/hygiene gates via
  `scripts/run_test.sh`; `dotnet build Ashfall.csproj`; headless
  `--expedition-panel-uitest`, `--ui-layout-selftest`,
  `--data-integrity-selftest` (data change), `--scene-binding-selftest`
  (unchanged chrome).

## Definition of done

Gates green, build clean, data validated, series committed (hunk-level),
plan marked FULLY INTEGRATED and archived, `.ai/state.md` updated, claim
released.

## Integration record (2026-10-01)

- **R1:** 12 previously-unreachable routes wired via host deep links:
  map_atlas + maritime_atlas → MapPanel; faction_matrix + factions_narrative
  → FactionsPanel; survival_workstation → CraftingPanel (tscn button +
  contract row); weather_sonde → WeatherPanel; quests_atlas → QuestsPanel;
  research_atlas → ResearchPanel; muster_atlas → MusterPanel; caravan_barter
  → TradeScreenGodotPanel; geiger_calibration → RadiationDetailPanel (tscn
  button + contract row); standing_record_atlas → JournalPanel. Consolidated
  wiring block in `Main.UiPanels.cs` (all hosts constructed before the
  block). New `PanelRouteReachabilityGateTests` (2 facts) keeps a living
  census: every player-navigable route must be emitted, direct-called
  (5 pinned: map/faction/quest detail, fire_incident, combat_hud), or on the
  pinned pending-foreman list (21 entries, each with a written reason —
  including 13 more unreachable routes the gate itself found on first and
  second run, several tied to known unwired debt like the Verdict tribunal).
- **R2:** `LongWalkExpeditionPanel` (100% fabricated telemetry, handler-less
  buttons, session-discarding Bind) deleted with its references; the
  registered route keeps its redirect to the real expeditions surface.
- **R3:** 52 of 68 `wasteland_map_v1.json` routes tagged mechanically from
  authored node metadata (`hazard_high` for high/locked endpoints,
  `amphibious` for flotilla routes); validate-json 714/714.
- **R4:** read-only `LocationLayoutSystem.GetLayoutDefinition` accessor;
  `OpenMapDetailPanel` passes unlocked-room display names (never sealed
  rooms) as `subLayouts`; MapDetailPanel threads the param. Salvage card
  keeps its truthful empty state — `GetNodeIntel().LootDescription` is
  hardcoded "Unknown" (no real salvage authority; flagged as follow-up).
- **R5:** hunk-level commit of the series (this record documents the
  boundary; see commit message).
- Verification: reachability 2/2, truthfulness 6/6, route 22/22, hygiene
  2/2, panel contract 1/1 — PASS; build 0 errors; headless data-integrity
  PASS (0 errors), scene-binding 25/25, ui-layout PASS, expedition-panel
  uitest 59/59. Foreign in-flight combat tests twice transiently broke the
  test-project compile (resolved by their owner stream; untouched here).
