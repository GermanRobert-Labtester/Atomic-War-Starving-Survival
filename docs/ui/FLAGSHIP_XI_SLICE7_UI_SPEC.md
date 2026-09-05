# ASHFALL — Flagship XI Slice 7 UI Spec — SubterraneanMapPanel & PsyOps Relay Console

Status: **PLANNED — awaiting Stitch design generation + implementation.**
Owner: Flagship XI follow-up. Format follows `docs/ui/GREENHOUSE_UI_GAP_SPEC.md`.
Design authority: **google-stitch** (AGENTS.md policy). Stitch output is a
**layout proposal only** until reconciled with the runtime theme
(`AshfallUiHelpers` / `Ashfall.Core.UI.Theme`), the
`state → blocker → cost → consequence` panel standard, and Core binding
discipline (presentation-only panels; `LastEvent` is the single feedback strip).

> **google-stitch connection note (2026-09-06):** the Stitch MCP accepted
> project creation (`projects/12009254180198459888`) but two
> `generate_screen_from_text` calls timed out with no screen landing (polled
> `list_screens` for ~6 minutes, always empty). Recorded per the AGENTS.md MCP
> failure policy; probing stopped. The **Stitch prompt skeletons** below are
> paste-ready: run them against the project above (or a successor) when the
> connection is healthy, then reconcile per §4 before implementing.

---

## 1. Panel register (this slice)

| Panel id | Class | Current state | Target state |
|---|---|---|---|
| `subterranean_map` | `src/UI/SubterraneanMapPanel.cs` (**new**) | does not exist | Bound panel: `Bind(SubterraneanHostSession)`; vertical depth-tier map of DISCOVERED nodes only; domain actions |
| `tropospheric_radio_relay` | `src/UI/TroposphericRadioRelayPanel.cs` (exists) | **stub** — `Bind(object?)` ignores the session, `IsBound=true` hardcoded, literal flavor text, buttons without handlers | Typed `Bind(PsyOpsHostSession)`; campaign schedule, reach/pressure readouts, jamming/counter commands, intercept feed |

AGENTS.md register implications: `TroposphericRadioRelayPanel` is removed from
the missing/stub table **only after** route→bind→visible→select→command→
state-delta→feedback→close is verified (closure rule 3) — file replacement
alone does not clear it.

---

## 2. SubterraneanMapPanel — binding contract (all verified at current HEAD)

### 2.1 Host API (source of truth: `src/Host/SubterraneanHostSession.cs`, `src/Host/Plan-156 Main.Subterranean.cs`)

```csharp
// Read state
SubterraneanSystem System { get; }               // session.System
System.State.nodes                               // List<SubterraneanNodeState>
   .nodeId / .depthTier / .discovered / .blocked /
   .structuralIntegrity (0..100) / .shoringLevel (0..3) /
   .oxygenLevel (0..100) / .ventilationInstalled / .waterLevel (0..100)
System.Zones                                     // id -> SubterraneanZoneDef
   (display_name, description, zone_type, oxygen_class,
    flood_susceptibility, surface_anchor_id, connection_rules[to, kind])
System.Find(nodeId)                              // node or null
string LastEvent                                 // single feedback strip
// Commands (return player-facing string; empty string = silent success)
string ShoreNode(string nodeId);                 // null error => success line
string InstallVentilation(string nodeId);
string ClearBlockage(string nodeId);
void   OnSurfaceLocationReached(string surfaceLocationId);   // discovery bridge
```

Rules: never render raw `subnode_*` ids — use `Zones[id].display_name`.
Undiscovered nodes are **never** shown (no placeholders, no "??" markers —
they are simply absent). Events `OnCaveIn/OnNodeFlooded/OnForcedRetreat/
OnNodeDiscovered` arrive via session `StateChanged` → `RefreshView()`;
the feedback strip shows `LastEvent` only.

### 2.2 Screen structure (Stitch proposal to reconcile)

- Left rail: depth tiers 1–3 (Tie 1 "Shallow" … Tier 3 "The Deep"), discovered
  node chips per tier, zone-type glyphs.
- Center: vertical cross-section; strata bands by `depthTier`; discovered
  chambers as outlined boxes connected by labeled passage lines
  (`connection_rules[].kind`: blast_door / stairwell / winze / overflow_scuttle…).
  Per-chamber readouts, each **value + word** (accessibility, never color-only):
  `AIR: <oxygen_class> <oxygenLevel>%`, `STRUCTURE: <word> <integrity>%`
  (word bands: ≥70 sound, 40–69 cracked, <40 failing),
  `WATER: <word> <waterLevel>%` (0 dry, >0 damp, ≥50 seeping, ≥80 flooded),
  `SHORE: <shoringLevel tick marks>`; blocked → hatched overlay +
  "BLOCKED — structure unsound". Surface anchors along the top edge from
  `surface_anchor_id`.
- Right operations column: selected chamber detail (display_name, one
  description line, three actions with cost lines — see 2.3), "CREWS BELOW"
  (survivors whose active expedition `locationId` == node — host supplies via
  `ExpeditionHostSession.Engine.Active`), bottom `LastEvent` strip.
- Top bar: title "THE UNDER-MAP", close (`OnClose` event → registry CLOSE),
  note: undiscovered passages stay unmapped.

### 2.3 Action routes (each: state → blocker → cost → consequence)

| Button | Session call | Blocker (button disabled + reason line) | Cost line | Consequence |
|---|---|---|---|---|
| SET SHORING | `ShoreNode(id)` | node unknown / `shoring_maxed` | "4 scrap wood, 2 steel rebar (+2 scrap metal from the second set)" | integrity +25 (cap 100), shoring tick shown; returned string → feedback strip |
| RUN A PUMP LINE | `InstallVentilation(id)` | `already_installed` | "3 scrap metal, 1 battery" | AIR readout gains recovery; strip line |
| CLEAR THE BLOCKAGE | `ClearBlockage(id)` | `not_blocked`; `structure_unsound` | "2 scrap wood" | hatch removed; strip line |

Failure strings from the session already name the blocker — render them in the
strip verbatim (they are authored, player-facing, no raw ids). After every
command call `RefreshView()` via the session `StateChanged` subscription
(pattern: `WeatherSondePanel` — subscribe in `Bind`, unsubscribe in re-Bind
and `_ExitTree`).

### 2.4 Registration + route (normal player entry)

1. `PanelRegistryBootstrap.RegisterAll`: `R("subterranean_map", "The Under-Map", PanelGroup.Secondary, new[] { "expeditions" });`
2. `src/Main.UiPanels.cs`: field + `new SubterraneanMapPanel { Visible = false }` + `AddChild`.
3. `src/Main.PlayerSurfaces.cs`: `PanelRegistry.ConfigureActions("subterranean_map",
   bindAction: () => { SetupSubterranean(); _subterraneanMapPanel.Bind(_subterranean); },
   open/close/availability per MapAtlasPanel precedent)`.
4. Entry point: nav button on the expedition/map console (see UI-09/UI-17 — the
   route MUST be registered in the same commit; a Live descriptor with no
   destination is a filed finding, and the dashboard nav overflow problem must
   not be worsened: prefer adding the route to the existing map surface).

### 2.5 Stitch prompt skeleton (paste-ready)

```
Full-screen desktop game UI panel, 1920x1080, for a bleak post-nuclear survival
management game. Tone: cold, exhausted, restrained. Dark charcoal/ash palette,
muted amber and rust accents, condensed sans headings, monospace data text,
hairline borders, compact utilitarian spacing.
PANEL: "THE UNDER-MAP" — cross-section of tunnels and chambers beneath the
settlement.
Layout: LEFT rail (280px): three stacked depth tiers ("Tier 1 — Shallow",
"Tier 2 — Sloped", "Tier 3 — The Deep"), each listing discovered zones as small
node chips with zone-type glyphs (cave, metro, tunnel, bunker, mine, collapsed
facility). Undiscovered zones are simply absent. CENTER (flexible): vertical
cross-section, dark strata bands, discovered chambers as outlined boxes joined
by labeled passage lines (labels: blast door, stairwell, winze, overflow
scuttle). Each chamber card: four text readouts pairing value plus word —
"AIR: thin 42%", "STRUCTURE: cracked 58%", "WATER: damp 20%", "SHORE: I I".
Blocked chambers get hatching and "BLOCKED — structure unsound". Surface
anchors along the top edge show which surface location each shaft hangs under.
RIGHT column (360px): selected chamber detail (name, one restrained flavor
sentence, three stacked action buttons: "SET SHORING", "RUN A PUMP LINE",
"CLEAR THE BLOCKAGE", each with a small monospace cost line like "costs 4 scrap
wood, 2 steel rebar"); below it a "CREWS BELOW" list of survivors currently
underground; bottom a one-line event feedback strip in muted amber ("timber and
steel set into Slope Galleries. It will hold a while longer."). Top bar: title
"THE UNDER-MAP", close button, note that undiscovered passages stay unmapped.
No neon, no radar, no fantasy.
```

---

## 3. TroposphericRadioRelayPanel revival — binding contract

### 3.1 Host API (source of truth: `src/Host/PsyOpsHostSession.cs`, `src/Main.PsyOps.cs`)

```csharp
Bind(PsyOpsHostSession session)                  // typed; replaces Bind(object?)
string LastEvent                                 // feedback strip
System.Campaigns                                 // running + expired instances
   .campaignId / .targetFactionId / .messageTheme / .baseReach /
   .receptiveness / .durationDays / .daysElapsed /
   .status  // 0 Active, 1 Stalled (transmitter down), 2 Expired
System.PressureOn(factionId)                     // -100..100 ideological pressure
System.TransmitterReady()                        // comms array tier+power gate
System.StartCampaignById(string campaignId, int day)   // guarded by one-per-target
System.StartJammingOn(string factionId, float strength, int days, int day)
System.CounterPropagandaOn(string campaignId, int days, int day)
event BroadcastIntercepted(campaignId, factionId, confidence, day)  // intel feed
Catalog catalog: PsyOpsCatalogLoader.Load(_dataDir, new FileSystemIO(),
   new SystemTextJsonSerializer())               // 8 authored campaigns, display names
```

### 3.2 Screen structure (to reconcile with Stitch proposal once generated)

- Status rail (`AshfallStatusRail`): `ON AIR` (campaigns active / stalled),
  `TRANSMITTER` (READY / NO ARRAY / BROWNOUT — from `TransmitterReady()`),
  `DAY`, `PRESSURE` (strongest |PressureOn| faction + value).
- Campaign table (`AshfallDataGrid`): one row per authored catalog campaign —
  display name, target faction (display name), theme, status
  (Not started / On air day X of Y / STALLED — no power / Ended), reach bar +
  number, pressure value with word (warmer / colder / neutral). Row selection
  enables the command buttons (keyboard-focusable rows — see UI-23; do not
  ship mouse-only selection).
- Commands (state → blocker → cost → consequence):
  - START BROADCAST → `StartCampaignById(selected, SimDay)`; blocker: already
    running / target channel taken / transmitter absent (still allow arming?
    NO — require `TransmitterReady()` and show why); consequence: status flips
    to On air, strip shows returned string.
  - JAM TARGET → `StartJammingOn(selected.targetFactionId, 1f, 3, SimDay)`;
    blocker: no target selected; cost note: "3 days of transmitter time";
    consequence: jamming listed in the static column.
  - COUNTER-PROGRAM → `CounterPropagandaOn(selectedRunning.campaignId, 2, SimDay)`;
    blocker: selected campaign not Active / already countered; consequence:
    reach suppressed (×0.25) shown in the row.
- Intercept feed: last N `BroadcastIntercepted` lines (campaign display name +
  faction display name + confidence word: faint/partial/clear) — surfaced via
  the session event; keep as text lines, newest first, capped.
- Feedback strip: `LastEvent` only.
- Theme note: this panel shares the radio console family (`RadioPanel` shell)
  but MUST NOT be a relabeled copy — its body, selected entity, state, blockers,
  costs and commands are psyops-specific (AGENTS.md owner requirement; UI-01
  anti-pattern explicitly forbidden).

### 3.3 Registration + route

The panel already exists in `PanelRegistryBootstrap` (stub lineage). Keep its
descriptor id, verify the configured route reaches it, and wire
`ConfigureActions(...)` bind/open/close in `Main.PlayerSurfaces.cs` with
`bindAction: () => { SetupPsyOps(); _psyopsRelayPanel.Bind(_psyops); }`.
If no route exists today, register one on the radio console surface in the
same commit (closure rule 3) — and fix `Bind(object?)` → typed, remove the
hardcoded `IsBound`, delete the literal flavor text, attach the three button
handlers.

### 3.4 Stitch prompt skeleton (paste-ready)

```
Full-screen desktop game UI panel, 1920x1080, for a bleak post-nuclear survival
management game. Tone: cold, restrained, wartime-radio. Dark charcoal palette,
muted amber highlights, monospace readouts, hairline borders.
PANEL: "TROPOSCATTER RELAY — BROADCAST CONSOLE" — the shelter's propaganda
operations desk.
Layout: LEFT (300px) status rail: four meter cards — "ON AIR" (n campaigns),
"TRANSMITTER" (READY / NO POWER), "STATIC" (jammed targets), "PRESSURE" (a
faction name and -100..100 value with word warmer/colder). CENTER: a table of
eight broadcast campaigns — columns: campaign name, target faction, theme chip
(UNITY / HOPE / FEAR / AID / COUNTER-RUMOR), status (NOT STARTED / ON AIR day
3 of 6 / STALLED — NO POWER / ENDED), reach bar 0-100, pressure word. One row
selected. BELOW the table: three command buttons in a row — "START BROADCAST",
"JAM TARGET (3 days)", "COUNTER-PROGRAM (2 days)" — each with a one-line
blocker note under the selected row when unavailable (e.g. "already on air",
"transmitter down"). RIGHT (320px): "INTERCEPT LOG" — newest-first text lines
like "someone answered Truce Over the Rails — partial confidence" above a
single-line feedback strip in muted amber. Top bar: panel title, day counter,
close button. Restrained wartime-propaganda-office feel: paper lists, stamp
marks for status, no neon, no glory.
```

---

## 4. Reconciliation requirements (before ANY implementation merges)

1. Colors come from the runtime tuples in `Ashfall.Core.UI.Theme`, not hex in
   these prompts (UI-23: rendered tuples differ from some documented hexes).
   Contrast: readable text ≥ 4.5:1 vs its background; keep non-color status
   words (READY/STALLED/BLOCKED) — color alone never carries state.
2. Build through shared helpers (`AshfallUiHelpers.MakeTitle/MakeBody/
   MakeButton`, `AshfallStatusRail`, `AshfallDataGrid`); panels are
   `partial : Control, IBindablePanel` with `OnClose`, subscribe
   `StateChanged` in `Bind`, unsubscribe in re-Bind and `_ExitTree`.
3. No panel-local gameplay state, no second save store, no fixture-success
   messages (closure rules 5–6): every number rendered comes from the session;
   empty state renders as an honest "nothing to show" line.
4. Verification ladder per closure rules 3–4: registered route → bind →
   visible → select → command → Core/inventory delta → feedback →
   keyboard/controller close → save/reload. Headless check must fail on any
   engine exception, verify descendant bounds and focusable grid selection.
5. Content-utilization/integrity gates stay green (no new catalogs needed for
   Slice 7; `subterranean_zones.json` / `propaganda_campaigns.json` edges
   already registered).

## 5. Suggested commit slices

1. `SubterraneanMapPanel` + registration + route + headless smoke (tests pin:
   undiscovered nodes hidden, actions route to session, blockers disabled).
2. `TroposphericRadioRelayPanel` typed revival + route verification + AGENTS.md
   stub-table removal note.
3. Snapshot + UI health pass (bounds/contrast/focus per closure rule 4).
