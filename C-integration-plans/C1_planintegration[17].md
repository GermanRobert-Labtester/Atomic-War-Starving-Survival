# C1 — Flagship Integration Plan [17]: The Presented Game — Holdfast, Wasteland Map & Bounded Visual State

> **Output:** `C1_planintegration[17].md`
>
> **Source baseline:** Plan 51 — The Presented Game: From Panels to a Place You Look At
>
> **Wave:** Continuity Wave 8 — *The Presented Game*
>
> **Depends on:** Plan 50A asset manifest; Plan 32A/32B/32C travel graph, routes and knowledge; Plan 23A shelter power; Plan 20A/20B/20C dose, shielding and weather; Plan 24A/24B survivor condition/needs; Plan 35 production jobs; Plan 41A memorials/keepsakes; Plan 44B pair history; Plan 37B/37C keyboard/focus/reduced-motion; Plan 50C snapshot safety net; Plan 52 ambience/audio handoff.
>
> **Mandatory execution order:** 50A → 51A → 51B → 51C.
>
> **Wave-8 sequencing:** 50A → 50B → **51A** → 52A → **51B** → 50C → **51C** → 52B → 53 → 52C → 54.
>
> **Primary architectural rule:** views read existing authorities and never own simulation state.
>
> **Primary presentation rule:** one spatial shelter surface and one spatial world-map surface. Existing panel routes may remain as detail views, but no duplicate spatial truth may survive.
>
> **Primary visual rule:** every shader, tween, transition, grade, overlay, hotspot, actor marker and room state must be attributable to a named campaign fact or accessibility requirement.
>
> **Guardrails:** no renderer change; no 3D; no new simulation in views; no second map; no parallax scenery system; no particle system without a named mechanic; no per-frame recomputation for day-scale state; no shader with an untraceable input; no animation that cannot be reduced/disabled; no orphan `.tscn`; no filename-luck asset resolution.

---

# 0. Mission

ASHFALL already simulates a world but mostly presents it as forms and panels.

The source baseline shows:
- an almost-empty main scene;
- orphaned scene stubs;
- no tween usage;
- no shader files;
- no TileMap use;
- a large runtime-built `Control` UI surface;
- a 430-line holdfast interior view mounted awkwardly inside a panel;
- a wasteland map view and marker view that are not instantiated at all;
- many existing art assets, most currently unmapped;
- a real travel graph and shelter state ready to render.

The task is therefore not to invent a new genre or simulation layer.

The task is to let the player **see the simulation that already exists**.

The target presentation architecture is:

```text
                         CAMPAIGN AUTHORITIES
    ┌───────────────────────────────────────────────────────────────┐
    │ Shelter / Power / Thermal / Flooding / Decor / Schedule      │
    │ Survivors / Fitness / Grief / Production                     │
    │ World Graph / Routes / Knowledge / Dose / Territory          │
    │ Weather / Shielding / Memorial / Asset Registry              │
    └───────────────────────────────────────────────────────────────┘
                           │
                           ▼
                  PRESENTATION READ MODELS
               shelter view     map view
                    │              │
                    ▼              ▼
           HoldfastInterior    WastelandMap
                    │              │
             hotspots → panels   route→dispatch
                    │              │
                    └──────┬───────┘
                            ▼
                  shared motion vocabulary
                            │
                            ▼
                     bounded shader layer
```

The visual layer must answer:

### Shelter
- Who is where?
- Who is missing?
- Which rooms are dark?
- Which rooms are flooding?
- What is running?
- What is being produced?
- What traces of loss remain?

### Wasteland
- What do we know?
- What do we not know?
- Which routes are open?
- What is the dose/risk?
- Who controls the route?
- What changed?
- Where can we dispatch?

### Atmosphere
- Is the shelter powered?
- Is the weather severe?
- Is the air contaminated?
- Is the view readable and accessible?
- Can every visual effect be disabled/reduced?

---

# 1. Source-Evidence Interpretation

## 1.1 The main scene is effectively a shell

The source reports `scenes/Main.tscn` as essentially one root `Control` with `src/Main.cs`, while nearly all presentation is built in C#.

This means Wave 8 should not assume an existing scene hierarchy will solve ownership automatically.

51A must decide a mounting model explicitly.

## 1.2 Existing world scenes are hollow/orphaned

`HoldfastInterior.tscn` and `WastelandMap.tscn` exist but are not meaningfully loaded.

A hollow scene is a false implementation signal.

Each scene must be:
- loaded and owned;
- or deleted.

## 1.3 Holdfast visual code exists but is trapped inside a panel

The current holdfast view is already substantial.

This is evidence to refactor/mount, not rebuild.

## 1.4 Wasteland visual code exists but has zero runtime ownership

The map view and marker view need a mount-or-delete decision.

No more orphan visual classes.

## 1.5 The simulation state already exists

Rooms, power, schedule, flooding, thermal, decor, routes, dose, territory, memorials, and survivor condition already have authorities.

Views must consume those facts.

## 1.6 Asset supply is large but unmapped

The art library is not a guarantee of usable mapping.

Plan 50A's asset manifest is mandatory input.

## 1.7 The renderer is already sufficient

The current compatibility renderer and canvas stretch support bounded 2D shader/lighting work.

No renderer migration is justified.

## 1.8 Snapshot infrastructure is the safety net

51A–51C can move visual presentation aggressively only if every key state is snapshot-gated.

---

# 2. Non-Negotiable Presentation Invariants

## INV-51.1 — Views never own gameplay state

No view stores authoritative copies of:
- room power;
- occupancy;
- flooding;
- production;
- route state;
- dose;
- territory;
- survivor condition.

## INV-51.2 — One shelter spatial surface

There is one canonical visual holdfast surface.

Other shelter panels are detail/control surfaces.

## INV-51.3 — One world spatial surface

There is one canonical world map visualization.

No parallel `MapPanel`, `MapAtlasPanel`, scene map and dead world view all pretending to be authoritative.

## INV-51.4 — Scene or delete

Every presentation scene in scope must be:
- loaded/owned;
- test-referenced;
- or removed.

## INV-51.5 — Assets resolve through manifest

No room sprite, portrait, background, overlay or icon is chosen by filename convention alone.

## INV-51.6 — Motion has one vocabulary

Transitions use shared timing/easing helpers.

No scattered magic tween durations.

## INV-51.7 — Motion is optional

Reduced-motion mode replaces or removes non-essential motion.

## INV-51.8 — Spatial values come from data/authority

Map layout and room identity come from authoritative layout data or deterministic derivation.

Views do not invent coordinates that change gameplay meaning.

## INV-51.9 — Knowledge gating applies visually

Unknown/unsurveyed world data remains imprecise or hidden.

## INV-51.10 — Accessibility is redundant-channel

Danger, dose, territory and power states use:
- color;
- shape;
- text/number/icon
where relevant.

## INV-51.11 — Day-scale facts do not poll every frame

Visual state updates from:
- state-change signal;
- day event;
- bound read-model refresh.

## INV-51.12 — Shader count is bounded

≤6 total introduced under 51C unless future plan explicitly raises the budget.

## INV-51.13 — Shader input is traceable

Every uniform/grade input can be traced to:
- authority;
- field;
- update trigger.

## INV-51.14 — No shader-only warning

Critical state must remain legible without effects.

## INV-51.15 — Snapshot states are contracts

Calm, crisis, knowledge, power and contamination states have reviewed snapshots.

---

# 3. Definition of Done

Plan 51 closes only when:

- one holdfast mount architecture is documented and implemented;
- hollow holdfast scene is either completed or deleted;
- `HoldfastInteriorView` binds campaign-owned state;
- room list/power/flooding/thermal/production/occupancy render from authority;
- survivor actor view displays who is where;
- dead/missing survivors do not remain visually present;
- grief/memory can affect actor/sleep-space presentation without owning grief state;
- room hotspots route through `OpenPlayerPanel`;
- keyboard focus and back navigation work;
- room/actor assets resolve via `asset_registry.json`;
- shared motion helper exists;
- reduced-motion behavior exists from first tween;
- shelter view is event/state-change driven rather than per-frame recomputed;
- calm/brownout/flooding/crisis snapshots are approved;
- no duplicate holdfast view exists after rebind;
- orphan holdfast scene state is gone;
- wasteland map view is either rehabilitated or replaced intentionally;
- one map spatial surface remains;
- map nodes/edges come from `WastelandMapSystem`;
- knowledge rung controls precision;
- route terrain/distance/direction render correctly;
- danger and dose are visible redundantly;
- territory/control is visible only when known;
- changes in control can be highlighted;
- scars/closures/graves render from real events;
- destination selection pre-fills dispatch using 32B route preview;
- hidden data does not leak;
- pan/zoom/reveal use shared motion vocabulary;
- map layout does not recompute every frame;
- map surface passes scene binding checks;
- four knowledge-depth snapshots exist;
- ≤6 shaders total are introduced;
- every shader has purpose/input/cost/reduced-mode documentation;
- interior light responds to power/lighting demand;
- weather grade responds to weather state;
- contamination/dust overlay responds to dose/shielding;
- palette/grade resource is singular;
- shader uniforms update from state changes;
- material/import/scene lint is clean;
- minimum-spec frame-time budget is measured;
- snapshot suite contains lit/unlit, clear/storm, contamination states;
- visual/audio responsibility boundary is documented with Plan 52;
- no renderer change or unnecessary effects chain occurs;
- all builds/selftests/asset gates/scene lint/runtime-scale/snapshot diffs pass.

---

# 4. Phase P0 — Presentation Ownership Audit

## P0.1 Capture baseline

Record:

```text
commit SHA
branch
dirty paths
scene files
scene load references
world view classes
view instantiation call sites
map panel classes
asset registry state
unreferenced visual assets count
CreateTween count
shader count
TileMap count
AnimationPlayer count
snapshot count
renderer/mode/stretch
reduce-motion setting
focus/navigation seam
```

## P0.2 Build presentation ownership matrix

Create:

`docs/visual/PRESENTATION_OWNERSHIP.md`

Columns:

```text
surface
scene
view class
mount owner
authority inputs
route
asset manifest rows
snapshot set
status
```

Rows:
- main shell;
- holdfast interior;
- wasteland map;
- map detail;
- shelter detail;
- world markers;
- survivor actors;
- room hotspots.

## P0.3 Classify every scene

Status:

```text
LIVE
MOUNT_PENDING
ORPHAN_DELETE
TEST_ONLY
```

No unknown scene.

## P0.4 Classify world view classes

For each:
- mount;
- rewrite;
- delete.

Preserve useful logic where possible.

## P0.5 Baseline performance

Capture:
- current shelter panel open time;
- map panel open time;
- idle frame time;
- day-advance frame spike.

Used for regression.

---

# TASK 51A — The Holdfast as a Place

# 51A.0 Goal

Opening the shelter should show a spatial bunker whose visible state is a direct projection of campaign authorities.

---

## 51A.1 Decide the mount before coding

Evaluate exactly two supported patterns:

### Option A — scene-level holdfast view
```text
Main presentation root
→ HoldfastInterior scene/view
→ panels overlay it
```

### Option B — controlled SubViewport mount
```text
ShelterPanel
→ SubViewport
→ HoldfastInterior world
→ TextureRect display
```

Document chosen option and why.

Do not retain the current ambiguous "Node2D inside arbitrary form hierarchy" pattern.

---

## 51A.2 Mount decision criteria

Evaluate:
- focus/navigation;
- lifecycle/rebind;
- snapshot capture;
- scaling;
- performance;
- panel overlay;
- future ambience/lighting;
- asset ownership.

Prefer the smallest architecture that supports both shelter and future visual state cleanly.

---

## 51A.3 Scene ownership

If `scenes/HoldfastInterior.tscn` is retained:
- load it from one named owner;
- register it in scene ownership manifest;
- add binding selftest.

If not:
- delete it;
- remove docs/refs.

---

## 51A.4 Remove hollow nodes

No retained node may exist only as an empty promise.

`ForegroundOverlay.texture = null` is acceptable only if runtime binding is explicit and tested.

Empty actor/hotspot containers are acceptable only if runtime population is the intended owner.

Document it.

---

## 51A.5 Holdfast read model

Create a presentation-only immutable read model:

```text
rooms[]
  room_id
  room_type
  powered
  thermal_state
  flooding_state
  current_job_summary
  occupants[]
  decor[]
  hazard_flags[]
shelter_state
active_alerts
```

The read model is derived from existing systems.

---

## 51A.6 Do not persist read model

It is recomputed from authorities.

No save section.

---

## 51A.7 Room inventory

Source room list from:
- shelter configuration;
- schedule/power room registry;
- canonical room IDs.

Do not hardcode visual room list separately.

---

## 51A.8 Room visual manifest

For each room:
- background/plate;
- hotspot bounds;
- optional powered overlay;
- optional flooded overlay;
- optional decor slots.

Resolve via Plan 50A manifest.

---

## 51A.9 Missing room asset behavior

If mapped asset missing:
- show intentional fallback;
- asset gate warns/fails according to 50A rules.

No filename guess.

---

## 51A.10 Room power

Use `PowerGridSystem.IsRoomPowered(roomId)` or canonical equivalent.

Visual effects:
- light/dim state;
- lamp indicator;
- equipment state.

No view-side watt logic.

---

## 51A.11 Lighting demand

If 51C interior shader uses `lightingDemand`, it reads Plan 23A's real value.

51A only exposes state.

---

## 51A.12 Occupancy

Use schedule / sleep assignment / duty assignment.

For each survivor:
- current room/location;
- role;
- availability.

---

## 51A.13 Survivor actor binding

`SurvivorActorView` receives:
- survivor ID;
- display identity;
- current condition summary;
- current room;
- visual asset ID.

It does not store survivor gameplay state.

---

## 51A.14 Fitness visual state

Use Plan 24A verdict:

```text
Fit
Impaired
Unfit
Incapacitated
```

Translate to:
- posture marker;
- icon;
- text on focus/inspection.

Do not infer from raw health independently.

---

## 51A.15 Missing/dead survivors

A survivor removed by:
- death;
- departure;
- expedition;
- hospitalization
must not remain in previous room because a view cached them.

Refresh on relevant events.

---

## 51A.16 Production-in-room state

Use Plan 35 current jobs.

At-a-glance:
- machine active;
- paused;
- blocked;
- completed output awaiting storage.

No production arithmetic.

---

## 51A.17 Flooding

Use `SumpFloodingSystem`.

Visual:
- floor overlay/marker;
- hazard icon;
- room-state text.

Critical hazard still present in owning panel/briefing.

---

## 51A.18 Sky-layer armor/damage

Use `SkyLayerArmorSystem`.

Only render if physically relevant to interior/shelter shell.

Do not invent damage art state unsupported by authority.

---

## 51A.19 Thermal state

Use `ShelterThermalSystem`.

Render:
- cold/hot state;
- condensation/frost overlay only if mapped and accessible.

Do not turn temperature into decorative noise.

---

## 51A.20 Decor

Use `ShelterDecorSystem` slots.

The view shows:
- installed decor;
- memorial objects;
- keepsakes;
- carvings.

---

## 51A.21 Memorial memory

Plan 41A:
- memorialized keepsake;
- wall carving;
- bunk context.

A room can feel changed after a death through existing decor/history.

---

## 51A.22 Pair-history visual hint

If a survivor remains in a bunk/room associated with a deceased partner:
- show a subtle existing-state indicator if Plan 44B/41A provides it.

Do not create new grief logic.

---

## 51A.23 Actor density

Do not show dozens of tiny avatars if unreadable.

Use:
- grouping;
- selected/focused actors;
- room occupant count.

Test 1280×800.

---

## 51A.24 Hotspots

Each room hotspot has:
- room ID;
- target live panel route;
- accessible label.

---

## 51A.25 Navigation seam

Hotspot activation:

```text
focus/click room
→ OpenPlayerPanel(route)
```

Use existing navigation owner.

---

## 51A.26 No room-specific UI router

Do not build a switch in the view that opens bespoke controls.

Use route metadata/table.

---

## 51A.27 Keyboard navigation

Hotspots:
- tab/arrow reachable;
- Enter/Space opens;
- Escape/back returns;
- focus visible.

---

## 51A.28 Focus restoration

After detail panel closes:
- restore focus to originating room hotspot.

---

## 51A.29 Actor focus

Selecting an actor may open:
- survivor detail
through existing route and subject ID.

No new actor inspector.

---

## 51A.30 Shared motion vocabulary

Create a small helper:

```text
PresentationMotion
```

or current style equivalent.

Tokens:
- short;
- standard;
- long;
- easing.

Avoid arbitrary per-call values.

---

## 51A.31 Initial motion set

Only:
- fade;
- slide;
- pan/zoom helper if needed by map;
- reveal emphasis.

No bounce/spring spectacle.

---

## 51A.32 Reduced-motion behavior

For each transition:
- instant;
- crossfade;
- reduced distance
according to 37C.

---

## 51A.33 Motion setting read

Read existing accessibility setting.

No duplicate toggle.

---

## 51A.34 Motion timing tests

Assert:
- helper durations;
- reduced mode bypass.

Avoid screenshot timing flakiness by snapping to final state for snapshot capture.

---

## 51A.35 Event-driven refresh

Subscribe to:
- day advanced;
- power state changed;
- room state changed;
- survivor assignment/fate;
- production job changed;
- decor changed.

Use exact current events.

---

## 51A.36 No per-frame state sweep

`_Process` should not scan:
- all rooms;
- all survivors;
- all jobs
every frame.

---

## 51A.37 Coalesced refresh

If multiple day events arrive:
- rebuild read model once after batch.

---

## 51A.38 Rebind safety

Open/close shelter repeatedly.

Assert:
- one view;
- one subscription per event;
- no doubled actor nodes;
- no doubled hotspots.

---

## 51A.39 Scene binding selftest

Adopt `SceneBindingSelfTest` pattern.

Verify:
- expected node names;
- ext resources;
- unique-name bindings.

---

## 51A.40 Asset registry selftest

Every room/actor/decor visual requested by view:
- manifest-resolvable;
- fallback policy known.

---

## 51A.41 Calm snapshot

State:
- power stable;
- no flooding;
- routine jobs;
- mixed occupants.

---

## 51A.42 Brownout snapshot

State:
- selected rooms unpowered;
- dim markers;
- production halted where appropriate.

---

## 51A.43 Flooding snapshot

State:
- sump/room flood warning;
- readable without color only.

---

## 51A.44 Crisis snapshot

State:
- unpowered;
- injured survivor;
- blocked production;
- memorial/grief context;
- alert priority.

---

## 51A.45 Snapshot invariants

Across all:
- no overlap;
- labels readable;
- hotspot focus visible;
- no missing textures;
- no stale actor.

---

## 51A.46 Performance budget

Measure:
- first mount;
- refresh on day advance;
- crisis refresh;
- idle frame.

Compare to Plan 26C.

---

## 51A.47 No hidden simulation

Search new 51A files for:
- resource mutation;
- needs mutation;
- power mutation;
- production mutation.

View may only issue navigation/explicit user action through existing panel.

---

## 51A.48 Docs

Create:

`docs/visual/HOLDFAST_VIEW.md`

Include:
- mount;
- authority inputs;
- update triggers;
- route mapping;
- asset mapping;
- snapshots;
- perf budget;
- reduced-motion behavior.

### 51A DoD

Opening the shelter shows a real bunker state, and every visible fact is traceable to an existing campaign authority.

---

# TASK 51B — The Wasteland Map as the One Spatial World Surface

# 51B.0 Goal

Turn the existing travel graph, knowledge, dose, territory and route state into one map that supports actual expedition decisions.

---

## 51B.1 Start with dead-code decision

Audit:
- `WastelandMapView`;
- `MapLocationMarkerView`;
- `WastelandMap.tscn`.

For each:
- retain/mount;
- rewrite;
- delete.

Do not preserve dead code for sentiment.

---

## 51B.2 Map surface ownership ADR

Create:

`docs/architecture/ADR_WORLD_MAP_SURFACE.md`

Inventory:
- `MapPanel`;
- `MapAtlasPanel`;
- `map_detail`;
- `WastelandMapView`.

Choose:
- canonical spatial surface;
- detail surface;
- dispatch handoff.

---

## 51B.3 One spatial map

Recommended model:

```text
WastelandMapView
  = spatial nodes/routes/control/dose

Map detail
  = selected node facts/history

Dispatch
  = route execution
```

Retire duplicate spatial atlas if redundant.

---

## 51B.4 Map read model

Presentation-only:

```text
nodes[]
  id
  layout_position
  knowledge_rung
  display_name
  danger_tier
  dose
  control
  scars
  route_status_summary

edges[]
  from
  to
  distance
  terrain
  one_way
  open
  travel_time
  projected_dose
  risk
```

---

## 51B.5 No persistence

Derived from existing authorities.

---

## 51B.6 Graph source

Use Plan 32A `WastelandMapSystem`.

No local duplicate node list.

---

## 51B.7 Node coordinates

Preferred:
- explicit layout metadata in data;
- or deterministic graph layout with stable seed.

Do not use random layout each boot.

---

## 51B.8 Layout vs gameplay coordinates

Presentation coordinates are not gameplay distance.

Document distinction.

---

## 51B.9 Holdfast center

If design says Holdfast central:
- render that intentionally.

Do not modify graph distance to make picture prettier.

---

## 51B.10 Knowledge rung

Use Plan 32C:

```text
Unknown
Detected
Located
Surveyed
Mapped
```

or actual current terminology.

---

## 51B.11 Unknown visual

Unknown:
- vague region/marker;
- no exact stats.

---

## 51B.12 Detected visual

Detected:
- approximate marker;
- limited text.

---

## 51B.13 Surveyed/Mapped visual

Higher knowledge:
- precise route;
- dose;
- danger;
- control
only if those channels are known.

---

## 51B.14 No precision leak

Snapshot/test:
- unknown location cannot show exact hours, dose or faction percentage.

---

## 51B.15 Route rendering

Render:
- connection;
- direction;
- open/closed;
- terrain;
- distance.

---

## 51B.16 One-way routes

Use directional visual marker + text.

---

## 51B.17 Closed route

Visible only if player knows closure.

If unknown:
- show uncertain/unavailable state per knowledge rules.

---

## 51B.18 Route closure cause

Optional detail:
- weather;
- collapse;
- territory;
- seasonal ice road
if known.

---

## 51B.19 Danger tiers

Use Plan 32A six-tier authority.

Visual channels:
- shape/border;
- label/number;
- color supplemental.

---

## 51B.20 Ambient dose

Use Plan 20A.

Show:
- numeric/graded value if known;
- coarse band if partial knowledge.

---

## 51B.21 Projected travel dose

From Plan 32B route preview.

Do not calculate independently in map.

---

## 51B.22 Territory/control

Use Plan 30B.

Render only if information channel exists.

---

## 51B.23 Unknown faction control

No overlay if player lacks intel.

Could show:
- unknown;
- contested?
only if known fact.

---

## 51B.24 Control change emphasis

Track last-known/current known control state through existing history/event data.

Highlight:
- changed border;
- marker;
- date.

Do not create new territory history authority.

---

## 51B.25 Scars

Render from Plan 41B:
- grave/memorial;
- collapse;
- battle scar;
- route closure;
- known event.

---

## 51B.26 Scar date

Use real event date.

No invented "recent" labels if date missing.

---

## 51B.27 Marker view

`MapLocationMarkerView` receives only presentation data.

It does not query global singletons independently.

---

## 51B.28 Marker scene binding

If `.tscn` retained:
- bind via scene ownership manifest;
- selftest node names.

---

## 51B.29 Node selection

Keyboard/mouse selects node.

Selection updates:
- map detail;
- route preview;
- dispatch action availability.

---

## 51B.30 Dispatch prefill

Map destination action:

```text
select node
→ request 32B route preview
→ open dispatch
→ destination pre-filled
→ hours/fuel/dose/risk visible
```

---

## 51B.31 No direct expedition mutation

Map does not start expedition silently.

Dispatch surface remains authority for confirmation.

---

## 51B.32 Unreachable destination

If no route:
- action disabled;
- reason shown.

---

## 51B.33 Seasonal route state

Use Plan 38B:
- ice road;
- thaw;
- mud.

Render real route resolver result.

---

## 51B.34 Weather route modifier

Selected weather may alter current route.

Same route resolver provides final preview.

---

## 51B.35 Pan/zoom

Use shared motion/input helper.

No map-specific tween vocabulary.

---

## 51B.36 Reduced motion

Reveal transitions:
- immediate or reduced.

Pan/zoom direct user input remains responsive.

---

## 51B.37 Keyboard map navigation

Provide:
- next node;
- route neighbors;
- zoom keys;
- focus current;
- open detail.

Use existing input conventions.

---

## 51B.38 Screen-reader/accessibility

Selected marker announces:
- location;
- knowledge;
- risk;
- dose;
- route status
as known.

---

## 51B.39 Offscreen culling

Do not instantiate/render expensive detail for offscreen markers.

At 20–40 nodes this is simple but still disciplined.

---

## 51B.40 No per-frame layout recompute

Node positions computed:
- load;
- graph change;
- resize if necessary.

---

## 51B.41 Graph change refresh

World evolution:
- node revealed;
- route closed;
- control changed
updates map via events.

---

## 51B.42 Scene ownership

`WastelandMap.tscn`:
- mount and register;
- or delete.

No orphan scene remains.

---

## 51B.43 Duplicate map retirement

Retired map surface:
- route removed;
- docs updated;
- no dead panel registry entry.

---

## 51B.44 Map detail remains supporting surface

If `map_detail` has useful data:
- retain;
- make selection-driven.

Do not make it a second map.

---

## 51B.45 Knowledge-depth snapshots

Four states:
1. unknown;
2. detected;
3. surveyed;
4. mapped/high intelligence.

---

## 51B.46 Route crisis snapshot

Include:
- closure;
- high dose;
- hostile control;
- alternative path.

---

## 51B.47 Scene lint

Run Godot scene lint:
- ext resources;
- UIDs;
- orphans;
- naming.

---

## 51B.48 Tilemap-world QA

Even if no TileMap is used, run world QA over:
- node IDs;
- route mapping;
- layout references
as supported by tooling.

---

## 51B.49 Performance

Measure:
- initial render;
- pan/zoom;
- knowledge update;
- route change;
- control change.

---

## 51B.50 Docs

Create:

`docs/visual/WASTELAND_MAP_VIEW.md`

Include:
- ownership ADR link;
- authority inputs;
- knowledge rules;
- dispatch flow;
- accessibility;
- snapshots;
- performance.

### 51B DoD

The map becomes the one place where route, ignorance, dose, territory, scars and destination choice are spatially legible.

---

# TASK 51C — Bounded Lighting, Weather Grade & Motion

# 51C.0 Goal

Introduce the smallest visual-effects vocabulary that converts known mechanics into felt state without compromising readability, performance or accessibility.

---

## 51C.1 Set hard shader ceiling

Before implementation:

```text
max shaders introduced by Plan 51 = 6
```

Any seventh effect:
- defer;
- merge;
- reject.

---

## 51C.2 Shader registry

Create:

`docs/visual/SHADER_REGISTRY.md`

Columns:

```text
shader
purpose
state authority
input fields
update trigger
surfaces
cost
reduced mode
snapshot
owner
```

---

## 51C.3 Shader 1 — interior light falloff

Purpose:
- communicate powered vs unpowered shelter.

Inputs:
- room power;
- lighting demand;
- optional lamp state.

No independent light simulation.

---

## 51C.4 Interior-light uniforms

Possible:

```text
power_factor
ambient_factor
emergency_factor
```

Values derived from Plan 23A read model.

---

## 51C.5 Brownout state

Brownout visual:
- dim;
- not unreadable.

UI text/controls remain legible.

---

## 51C.6 Shader 2 — weather grade

Purpose:
- make exterior/map/weather state perceptible.

Input:
- canonical `WeatherKind`;
- severity.

---

## 51C.7 Weather grade is not forecast authority

It reflects current known weather.

It does not select weather.

---

## 51C.8 Shader 3 — contamination/dust overlay

Purpose:
- communicate environmental dose/contamination context.

Inputs:
- dose band;
- shielding;
- contamination/hazard state.

---

## 51C.9 No false precision

If exact dose unknown:
- visual uses coarse known band;
- never leaks exact hidden value.

---

## 51C.10 Additional shader candidates

Only if justified:
- flooding/wetness;
- frost/condensation;
- map reveal/fog;
- UI focus grade.

Each must earn place against ≤6 ceiling.

---

## 51C.11 Reject decorative-only effect

If shader has no named mechanic/accessibility purpose:
- reject.

---

## 51C.12 Single palette/grade resource

Create or extend one:
- design palette;
- grade parameters;
- contrast constraints.

Weather and power do not each create unrelated color science.

---

## 51C.13 UI palette protection

Gameplay scene grading must not:
- lower text contrast below accessibility threshold;
- tint critical UI labels into ambiguity.

---

## 51C.14 Contrast test

Snapshot/color sampling or UI-access test:
- text remains readable in each grade.

---

## 51C.15 State-driven updates

Uniform values update on:
- day/weather change;
- power state;
- dose/shielding change.

Not every frame.

---

## 51C.16 Animated effects exception

If a shader animates:
- name reason;
- declare frequency;
- measure cost;
- reduced-motion version.

---

## 51C.17 No gratuitous shader time

Avoid `TIME`-driven noise just because it is easy.

---

## 51C.18 Motion vocabulary from 51A

All presentation transitions use shared helper.

51C formalizes docs/tests.

---

## 51C.19 Motion tokens

Example:

```text
instant
fast
standard
slow
```

Mapped to one duration/easing table.

---

## 51C.20 Surface transitions

Use only:
- fade;
- slide;
- focus/reveal;
- map pan/zoom.

---

## 51C.21 No motion for every button

The motion vocabulary is for:
- spatial/context transitions;
- state reveal.

Not decorative micro-animation everywhere.

---

## 51C.22 Reduced-motion contract

When enabled:
- no large slides;
- no animated contamination noise;
- no flashing;
- map reveals snap or crossfade;
- state still fully legible.

---

## 51C.23 Photosensitivity

Set explicit limits:
- no rapid flash;
- no high-frequency contrast pulse.

Document.

---

## 51C.24 Renderer stays compatibility

No project renderer migration.

---

## 51C.25 No post-processing chain

Each shader/material effect must justify itself independently.

No layered stack of full-screen passes by default.

---

## 51C.26 Material ownership

Materials live in:
- `assets/ui/materials/`;
- appropriate scene resources.

Avoid runtime material duplication if shared.

---

## 51C.27 Import settings

Run shader/material lint for:
- filtering;
- mipmaps;
- compression;
- texture flags.

---

## 51C.28 Ported-asset settings

Where Unity-origin assets require changed import settings:
- document explicitly.

---

## 51C.29 State-binding evidence

For each shader docs include:
- authority;
- code path;
- file:line evidence generated/current at implementation time.

Do not hardcode stale line numbers into long-lived handwritten docs if Plan 29 can generate them.

---

## 51C.30 Shelter shader snapshots

States:
- lit;
- brownout;
- blackout;
- flood/cold if implemented.

---

## 51C.31 Weather snapshots

States:
- clear;
- storm;
- snow/ash/rain according to current weather set.

---

## 51C.32 Contamination snapshots

States:
- low;
- elevated;
- severe;
with no-warning parity.

---

## 51C.33 Reduced-motion snapshots

Verify same information remains readable.

---

## 51C.34 Minimum-spec budget

Use Plan 26C target.

Measure:
- baseline;
- shelter with effects;
- map with effects;
- worst combined state.

---

## 51C.35 Frame-time acceptance

Define explicit maximum regression before merge.

Use current performance budgets, not an invented arbitrary number if one already exists.

---

## 51C.36 CPU update budget

Uniform update/event binding must be negligible.

No per-frame state traversal.

---

## 51C.37 GPU fill budget

Full-screen grades:
- count;
- resolution;
- blend cost.

Keep bounded.

---

## 51C.38 Ambience/audio handoff

Create:

`docs/visual/VISUAL_AUDIO_STATE_BOUNDARY.md`

Example:
- storm visual grade = 51C;
- storm ambience bed/sting = Plan 52;
- weather authority = Plan 20C.

---

## 51C.39 Avoid duplicate expression conflicts

If weather severity changes:
- visual and audio both read same authority;
- no separate weather interpretation.

---

## 51C.40 Accessibility fallback

If shaders disabled entirely:
- power/dose/weather state still present in text/icons.

---

## 51C.41 Debug disable switch

Development setting can disable effects for comparison/perf.

Not necessarily player-exposed beyond reduced-motion/graphics settings.

---

## 51C.42 Lint gates

Run:
- shader-material lint;
- Godot scene lint;
- asset gate.

---

## 51C.43 Runtime scale selftest

Include:
- repeated map updates;
- repeated shelter state changes;
- shader uniform updates.

---

## 51C.44 Shader unit/binding tests

Assert:
- power event updates power uniform;
- weather event updates weather uniform;
- dose event updates contamination uniform.

---

## 51C.45 No per-frame binding test

Instrument update counters.

Idle 60 seconds:
- no state rebuilds without changes.

---

## 51C.46 Reduce-motion test

Toggle:
- active tweens stop/replace safely;
- state snaps correctly;
- no stale halfway position.

---

## 51C.47 Snapshot diff policy

Visual changes to shader snapshots require:
- reviewed intent;
- not auto-update.

---

## 51C.48 Docs per shader

Each shader entry includes:
- purpose;
- inputs;
- cost;
- fallback;
- owner;
- snapshot.

---

## 51C.49 Effect removal rule

If effect cannot meet:
- contrast;
- performance;
- accessibility;
- traceability
then delete it.

---

## 51C.50 No shader completeness metric

Do not aim to hit 6.

Zero-to-three may be sufficient.

Ceiling is not target.

### 51C DoD

A small, measurable, accessibility-safe visual vocabulary makes power, weather and contamination felt while preserving the same simulation and UI clarity.

---

# 5. Cross-Task Dependency Graph

```text
50A asset manifest
      │
      ▼
51A holdfast mount
      │
      ▼
51B map mount/consolidation
      │
      ▼
51C bounded effects
```

Supporting:

```text
23A power ───────────────► room state / lighting
20A/20B dose/shield ─────► map dose / contamination
20C weather ─────────────► map/weather grade
24A/24B fitness/needs ───► actor state
35 production ───────────► room job state
41A memorial ────────────► keepsakes/decor
44B pair history ────────► grief context
32A/B/C graph/route/info ► map
37B/C access/motion ─────► every surface
50C snapshots ───────────► visual regression
52 audio ────────────────► state-expression handoff
```

---

# 6. Presentation Authority Matrix

Create/maintain:

| Visible fact | Authority | Presentation |
|---|---|---|
| room powered | PowerGrid | holdfast light/icon |
| room temperature | ShelterThermal | room state |
| flooding | SumpFlooding | overlay/icon |
| occupant | schedule/duty | actor |
| condition | fitness/health | actor icon/text |
| production | production job authority | machine/job marker |
| memorial/keepsake | memorial/decor | room object |
| node knowledge | world knowledge | marker precision |
| route | travel graph | edge |
| dose | radiation | map label/grade |
| control | faction territory | border/overlay |
| scar | location memory | dated marker |
| weather | WeatherSystem | grade |
| contamination | radiation/shield | overlay |

Any visible fact without an authority is rejected.

---

# 7. Scene Ownership Contract

Every retained `.tscn` in Wave 8 needs:

```text
one loading owner
one view/controller
one scene-binding test
one manifest row
one lifecycle path
```

No orphan.

---

# 8. View Lifecycle Contract

```text
instantiate
→ bind authorities/read model
→ subscribe
→ render
→ refresh on event
→ unbind
→ unsubscribe
→ free/reuse
```

Rebind must not duplicate subscriptions/nodes.

---

# 9. Asset Manifest Contract

Visual lookup:

```text
semantic asset id
→ asset_registry.json
→ resolved resource
```

Never:

```text
"room_" + roomId + ".png"
```

unless manifest generator itself owns that convention and validates it.

---

# 10. Motion Contract

Motion is a presentation primitive, not gameplay timing.

Never use tween completion to:
- finish a production job;
- trigger travel;
- apply damage;
- gate save.

Gameplay state is already decided.

---

# 11. Reduced-Motion Contract

Reduced-motion mode must not change:
- interaction availability;
- navigation order;
- state timing;
- information.

Only presentation.

---

# 12. Spatial Surface Contract

## Holdfast
Spatially locates:
- people;
- rooms;
- active conditions.

## Map
Spatially locates:
- world nodes;
- routes;
- known risk/control.

Panels remain:
- detailed information;
- controls;
- confirmation.

---

# 13. Holdfast Readability Priorities

At a glance, priority order:

1. critical room hazard;
2. survivor presence/condition;
3. power/flood state;
4. active production;
5. decor/memory;
6. atmosphere.

Do not let decor outrank crisis.

---

# 14. Map Readability Priorities

1. selected destination;
2. route viability;
3. projected risk/dose;
4. knowledge uncertainty;
5. control;
6. scars/history;
7. flavor.

---

# 15. Knowledge Precision Contract

Example:

```text
Unknown     → no exact value
Detected    → approximate location
Located     → route exists but coarse risk
Surveyed    → exact danger/dose where known
Mapped      → full navigation detail
```

Use actual Plan 32C semantics.

---

# 16. Map Dispatch Contract

The map does not start expeditions.

It prepares the decision.

```text
map select
→ route preview
→ dispatch panel
→ player confirms
→ expedition authority mutates
```

---

# 17. Territory Visibility Contract

Known control may come from:
- scouting;
- radio;
- caravan;
- previous visit.

No channel:
- no exact overlay.

---

# 18. Visual Scar Contract

A scar shown on map/interior must have:
- source event;
- date if available;
- known-state visibility.

No invented history.

---

# 19. Shader Input Contract

Every shader uniform belongs to one category:

```text
mechanical state
accessibility setting
presentation constant
```

No hidden pseudo-random gameplay state.

---

# 20. Shader Cost Contract

For each shader measure:
- CPU uniform update;
- GPU/frame delta;
- overdraw scope;
- low-end result.

---

# 21. Palette Contract

A single palette resource defines:
- base luminance;
- warning contrast;
- disabled state;
- weather grade constraints;
- contamination grade constraints.

No effect may invalidate UI contrast.

---

# 22. Snapshot Matrix

## Holdfast
- calm;
- brownout;
- flooding;
- crisis.

## Map
- unknown;
- detected;
- surveyed;
- mapped;
- route crisis.

## Effects
- lit;
- unlit;
- clear;
- storm;
- contaminated;
- reduced-motion/fallback where capture meaningful.

---

# 23. Snapshot Review Rules

A golden update requires:
- reason;
- intended state change;
- accessibility check;
- no missing assets;
- no clipping.

---

# 24. Performance Measurement Matrix

Measure:

```text
main idle
holdfast open
holdfast refresh
map open
map pan
map update
shader worst case
day advance with views mounted
```

Compare median/p95.

---

# 25. Event-Driven Refresh Strategy

Preferred event sources:
- `DayAdvanced`;
- power state;
- room state;
- survivor state/fate;
- production job;
- world reveal;
- route state;
- territory change;
- weather/dose.

If authority lacks events:
- refresh on day batch rather than per frame.

---

# 26. Failure Injection Matrix

## N51.1 Holdfast scene retained but never loaded
Expected: ownership gate fails.

## N51.2 View caches room power and misses brownout
Expected: live-authority snapshot/test fails.

## N51.3 Panel reopen doubles survivor actors
Expected: rebind test fails.

## N51.4 Missing room texture silently guessed
Expected: asset registry gate fails.

## N51.5 Hotspot opens dead/shelved panel
Expected: route liveness test fails.

## N51.6 Unknown map node reveals exact dose
Expected: knowledge leak test fails.

## N51.7 Map and route preview disagree
Expected: dispatch-prefill integration fails.

## N51.8 Two spatial map surfaces remain
Expected: ownership ADR/gate fails.

## N51.9 Shader updates every frame despite unchanged state
Expected: update-counter test fails.

## N51.10 Reduced-motion still runs long slide
Expected: accessibility test fails.

## N51.11 Shader hides critical text contrast
Expected: snapshot/UI-access test fails.

## N51.12 Seventh shader added
Expected: shader registry budget fails unless plan changed explicitly.

---

# 27. Test Pyramid

## Tier 1 — Read model
- authority mapping;
- knowledge precision;
- route preview.

## Tier 2 — Scene binding
- node names;
- resources;
- lifecycle.

## Tier 3 — UI interaction
- hotspots;
- map selection;
- dispatch prefill;
- keyboard.

## Tier 4 — Snapshot
- shelter/map/effects.

## Tier 5 — Performance
- runtime scale;
- frame budgets.

## Tier 6 — Export smoke
- scene/resource load;
- shader compile;
- no missing assets.

---

# 28. CI / Gate Set

Use/reinforce:

```text
godot_asset_gate
scene_binding_selftest
godot_scene_lint
shader_material_lint
asset_registry_selftest
runtime_scale_selftest
snapshot_diff
ui_access
verify_fast
```

Avoid creating duplicate visual gates unless a unique invariant lacks an owner.

---

# 29. Verification Commands

Run per task:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
bash scripts/ci/godot-asset-gate.sh
bash scripts/ci/scene-lint.py
godot --headless --path . -- --runtime-scale-selftest
bash scripts/ci/verify-fast.sh
```

Also run current equivalents of:

```text
ashfall-shader-material-lint
ashfall-tilemap-world-qa
ashfall-snapshot-diff
asset-registry-selftest
ui-access
```

---

# 30. Recommended Commit Breakdown

```text
51A-1 presentation ownership audit + mount ADR
51A-2 holdfast scene ownership/binding
51A-3 shelter read model
51A-4 room/actor/production rendering
51A-5 hotspots/navigation/focus
51A-6 asset manifest integration
51A-7 shared motion/reduced motion
51A-8 snapshots/perf/rebind/docs

51B-1 map dead-code decision + ADR
51B-2 map read model + graph binding
51B-3 knowledge/dose/danger rendering
51B-4 territory/scars/closures
51B-5 node selection + dispatch prefill
51B-6 motion/accessibility
51B-7 duplicate map retirement + scene binding
51B-8 snapshots/perf/docs

51C-1 shader registry + cost ceiling
51C-2 interior lighting shader
51C-3 weather grade
51C-4 contamination overlay
51C-5 palette/contrast/reduced motion
51C-6 material/import/scene lint
51C-7 snapshot/perf/binding tests
51C-8 visual/audio boundary + docs
```

---

# 31. Risk Register

## R51.1 Visual view becomes second simulation

Mitigation:
- immutable read models;
- mutation search;
- authority matrix.

## R51.2 Scene mounting creates lifecycle leaks

Mitigation:
- one mount owner;
- bind/unbind tests;
- exact subscription counts.

## R51.3 Map consolidation breaks existing routes

Mitigation:
- explicit ADR;
- preserve detail/dispatch routes;
- route tests.

## R51.4 Knowledge leaks through pretty map

Mitigation:
- precision matrix;
- snapshots at four knowledge depths.

## R51.5 Shader work harms compatibility renderer

Mitigation:
- ≤6 ceiling;
- perf budget;
- simple canvas shaders only.

## R51.6 Accessibility regresses after grading

Mitigation:
- contrast checks;
- no color-only state;
- reduced-motion.

## R51.7 Art mapping becomes filename heuristics

Mitigation:
- Plan 50A manifest only.

## R51.8 Snapshot churn becomes unreviewable

Mitigation:
- bounded canonical states;
- no auto-refresh.

---

# 32. Acceptance Checklist

## P0

- [ ] presentation ownership matrix
- [ ] every scene classified
- [ ] every world view class classified
- [ ] baseline scene refs captured
- [ ] baseline motion/shader counts captured
- [ ] baseline performance captured

## 51A — Holdfast

- [ ] mount architecture documented
- [ ] one mount implemented
- [ ] hollow scene loaded or deleted
- [ ] empty-node intent documented
- [ ] holdfast read model
- [ ] read model not persisted
- [ ] room list from authority
- [ ] room asset manifest rows
- [ ] missing asset fallback/gate
- [ ] power from PowerGrid
- [ ] no duplicate watt logic
- [ ] occupancy from schedule/duty
- [ ] survivor actor binding
- [ ] fitness verdict consumed
- [ ] dead/missing actors disappear
- [ ] production jobs visible
- [ ] flooding visible
- [ ] shell/armor state used only if relevant
- [ ] thermal state visible
- [ ] decor from ShelterDecor
- [ ] memorial/keepsake visible
- [ ] pair-history/grief hint uses existing state
- [ ] actor density readable
- [ ] hotspot route metadata
- [ ] OpenPlayerPanel reused
- [ ] no bespoke room router
- [ ] keyboard hotspot navigation
- [ ] focus restoration
- [ ] actor detail route
- [ ] shared motion helper
- [ ] motion token table
- [ ] reduced-motion behavior
- [ ] existing accessibility setting used
- [ ] motion tests
- [ ] event-driven refresh
- [ ] no per-frame full sweep
- [ ] refresh coalescing
- [ ] no duplicate instantiation/rebind
- [ ] scene binding selftest
- [ ] asset registry selftest
- [ ] calm snapshot
- [ ] brownout snapshot
- [ ] flooding snapshot
- [ ] crisis snapshot
- [ ] snapshot invariants
- [ ] performance budget
- [ ] no hidden simulation mutation
- [ ] HOLDFAST_VIEW.md

## 51B — Map

- [ ] dead map classes decided
- [ ] map ADR
- [ ] one spatial map surface
- [ ] map read model
- [ ] no map save copy
- [ ] graph from WastelandMapSystem
- [ ] deterministic layout
- [ ] presentation coordinates separate
- [ ] holdfast rendered intentionally
- [ ] knowledge rung authority
- [ ] unknown visual
- [ ] detected visual
- [ ] surveyed/mapped precision
- [ ] no exact-data leak
- [ ] routes rendered
- [ ] one-way route rendering
- [ ] known route closures
- [ ] closure cause if known
- [ ] danger tiers redundant-channel
- [ ] ambient dose
- [ ] projected dose from route preview
- [ ] territory from Plan 30B
- [ ] no unknown control leak
- [ ] control change emphasis
- [ ] scars from place memory
- [ ] scar dates real
- [ ] marker view presentation-only
- [ ] marker scene binding
- [ ] node selection
- [ ] dispatch prefill
- [ ] map does not auto-start expedition
- [ ] unreachable destination reason
- [ ] seasonal route state
- [ ] weather route state
- [ ] shared pan/zoom motion
- [ ] reduced-motion map reveal
- [ ] keyboard map navigation
- [ ] accessibility labels
- [ ] offscreen detail culling
- [ ] no per-frame layout
- [ ] graph change refresh
- [ ] map scene loaded or deleted
- [ ] duplicate map retired
- [ ] map detail remains supporting
- [ ] four knowledge snapshots
- [ ] route crisis snapshot
- [ ] scene lint
- [ ] world QA
- [ ] performance
- [ ] WASTELAND_MAP_VIEW.md

## 51C — Effects

- [ ] shader ceiling = 6
- [ ] shader registry
- [ ] interior light shader justified
- [ ] light inputs authoritative
- [ ] brownout remains readable
- [ ] weather grade justified
- [ ] weather not selected by view
- [ ] contamination overlay justified
- [ ] no dose precision leak
- [ ] additional shaders individually justified
- [ ] no decorative-only shader
- [ ] single palette/grade resource
- [ ] UI palette protected
- [ ] contrast checks
- [ ] state-driven uniform updates
- [ ] animated effects declare cost
- [ ] no gratuitous TIME animation
- [ ] shared motion vocabulary finalized
- [ ] tokenized motion
- [ ] bounded transition set
- [ ] no button-motion sprawl
- [ ] reduced-motion complete
- [ ] photosensitivity limits
- [ ] compatibility renderer retained
- [ ] no post-processing chain
- [ ] material ownership clear
- [ ] import settings lint
- [ ] ported settings documented
- [ ] state-binding evidence
- [ ] shelter shader snapshots
- [ ] weather snapshots
- [ ] contamination snapshots
- [ ] reduced-mode snapshot/check
- [ ] minimum-spec budget
- [ ] frame-time budget
- [ ] CPU update budget
- [ ] fill-rate budget
- [ ] visual/audio boundary doc
- [ ] visual/audio same state authority
- [ ] shader-off fallback legible
- [ ] debug effects disable
- [ ] lint gates
- [ ] runtime-scale selftest
- [ ] uniform binding tests
- [ ] idle no-refresh test
- [ ] reduced-motion transition test
- [ ] snapshot update policy
- [ ] per-shader docs
- [ ] delete any effect that fails constraints
- [ ] shader ceiling treated as ceiling, not quota

---

# 33. Ship / No-Ship Gate

**SHIP** only if:

```text
presentation_views_own_simulation_state == false
AND holdfast_spatial_surfaces == 1
AND world_map_spatial_surfaces == 1
AND retained_orphan_scenes == 0
AND holdfast_authority_identity == true
AND map_graph_authority_identity == true
AND unmapped_required_view_assets == 0
AND duplicate_view_subscriptions == 0
AND stale_survivor_actor_instances == 0
AND hotspot_routes_to_nonlive_panels == 0
AND map_hidden_precision_leaks == 0
AND map_dispatch_prefill_matches_route_authority == true
AND shader_count <= 6
AND shaders_without_named_state_input == 0
AND shader_only_critical_warnings == 0
AND per_frame_day_state_recomputations == 0
AND reduced_motion_supported == true
AND renderer_changed == false
AND visual_contrast_regressions == 0
AND scene_binding_selftests == pass
AND asset_registry_selftest == pass
AND godot_asset_gate == pass
AND scene_lint == pass
AND shader_material_lint == pass
AND runtime_scale_selftest == pass
AND snapshot_diff == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 34. Implementer Handoff

1. Start by deciding mount ownership, not by redrawing art.
2. Retain or delete every orphan scene—no hollow middle state.
3. Keep `HoldfastInteriorView` as presentation, never simulation.
4. Render room power, occupancy, flooding, production and decor from live authorities.
5. Route hotspots through `OpenPlayerPanel`.
6. Resolve all visual assets through Plan 50A's manifest.
7. Introduce motion once through a shared helper and reduced-motion contract.
8. Make holdfast refresh event-driven.
9. Audit the dead wasteland map code before rewriting it.
10. Choose one spatial map and retire duplicate spatial surfaces.
11. Render nodes/routes from the real travel graph.
12. Preserve knowledge uncertainty visually.
13. Pull dose/risk/control from their real authorities.
14. Let map selection prefill dispatch; never auto-dispatch.
15. Keep map layout deterministic and presentation-only.
16. Set the shader ceiling before adding shaders.
17. Add only effects with named mechanical inputs.
18. Keep the compatibility renderer.
19. Protect text/UI contrast under every grade.
20. Ensure every motion/effect has a reduced/disabled fallback.
21. Measure frame cost on the minimum-spec target.
22. Use snapshot states as reviewed visual contracts.
23. Document visual/audio ownership with Plan 52.
24. Close only when every presented fact can be traced back to campaign state.

---

# 35. Final Outcome

When this plan is complete, ASHFALL stops presenting its simulation as a wall of disconnected forms.

Opening the shelter shows an actual holdfast. People occupy rooms because the schedule says they do. Dark rooms are dark because the power grid says they are. Flooded spaces show flooding because the sump system says so. Production machinery shows work because a real job is running. Memorial objects and empty spaces remain because people died, left, or were remembered.

Opening the world map shows one spatial truth. Nodes and routes come from the travel graph. Unknown places stay uncertain. Dose, danger and hostile control become visible only when the player has earned that knowledge. Route closures and scars remain visible as consequences of real events. Selecting a destination moves directly into the existing dispatch decision with the correct time, fuel, dose and risk.

A small presentation vocabulary then makes state felt. Lighting falls away during brownouts. Weather shifts the world grade. Contamination changes the air. Motion makes transitions readable without becoming spectacle, and reduced-motion mode removes it without removing information.

None of this creates a second game inside the views.

The simulation remains exactly where it was.

Wave 8 simply gives the player a place to look at it.
