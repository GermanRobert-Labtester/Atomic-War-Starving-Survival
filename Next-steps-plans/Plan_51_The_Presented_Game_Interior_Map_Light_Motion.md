# Plan 51 — The Presented Game: From Panels to a Place You Look At

> **Wave:** Continuity Wave 8 — *The Presented Game* (Plans 50–54)
> **Depends on:** 50A (asset manifest — a world view with unmapped art is a slideshow), 32A/32B
> (the travel graph the map should render), 23A/20B (the shelter state the interior should render),
> 24A/24B/41A (who is where, and who is missing).
>
> **Theme:** the game has **0 shaders, 0 `CreateTween` calls, 0 TileMap nodes**, and four scenes that
> nothing loads. Its entire world is runtime-built `Control` UI (164 classes; 19 migrated to panel
> scenes). A presentation layer already exists in `src/World/` — a 430-line holdfast interior view
> that renders *inside a panel*, and a 193-line wasteland map view plus a 150-line marker view that
> **nothing instantiates at all**. This plan does not add a genre; it lets the game show the state it
> already simulates.

---

## Evidence Inventory (re-verified @ `ccac926e`)

| # | Fact | Evidence |
|---|---|---|
| 1 | Main scene is an empty shell | `scenes/Main.tscn` = 12 lines, **1 node** (`Control` + `src/Main.cs`); everything else is constructed in code |
| 2 | The other scenes are orphaned stubs | `scenes/HoldfastInterior.tscn` — 5 nodes: a background sprite, `ForegroundOverlay` with **`texture = null`**, and **empty** `SurvivorActors` / `RoomHotspot` containers; `scenes/WastelandMap.tscn` 3 nodes; `scenes/CSharpTest.tscn` 1 node. `grep -rn "res://scenes/" src/` → nothing loads them |
| 3 | **No motion, no shaders, no tiles** | `grep -rn "CreateTween" src/` → **0**; `find . -name "*.gdshader"` → **0**; `grep -c TileMap scenes/*.tscn` → 0 everywhere; `AnimationPlayer`-style nodes in scenes → 0 |
| 4 | A world view exists but is a panel guest | `src/UI/ShelterPanel.cs:45,249` constructs `HoldfastInteriorView` (430 lines, `Node2D`) **inside a UI panel** — the bunker is an inset image within a form |
| 5 | Two world views are entirely unwired | `src/World/WastelandMapView.cs` (193 lines): `refs_in_src = 0`; `src/World/MapLocationMarkerView.cs` (150 lines, has a `.tscn`): `refs_in_src = 1` (only from the dead view) |
| 6 | Panel-scene migration exists and is gated | 19 `assets/ui/panels/*.tscn`; `src/Host/SceneBindingSelfTest.cs` verifies scene↔class binding ("if a scene is regenerated or restructured, the matching …" check fails), and `docs/scenes.ownership.manifest.json` tracks ext_resources/unique names per panel scene |
| 7 | The state a view needs is already simulated | shelter rooms/facilities (`ShelterScheduleSystem`, `PowerGridSystem` rooms, `SkyLayerArmorSystem`, `SumpFloodingSystem`, `ShelterDecorSystem`), the travel graph (32A: 6 nodes / 7 routes to be populated), dose/zone (20A), fitness (24A), memorial/keepsakes (41A) |
| 8 | Art exists to draw with | `assets/art/bg_bunker_corridor.jpg`, 1,686 art stems, 437 icon files, `assets/ui/Screens` 6.5 MB, `assets/ui/Textures` 2 MB — but 1,189 art files are unreferenced (Wave 8's 50A), so the material is there and unmapped |
| 9 | Renderer and stretch are fixed and simple | `project.godot`: `gl_compatibility`, 1920×1080, `stretch=canvas_items`, `keep_height` — compatible with 2D shaders/lighting without a renderer change |
| 10 | Tooling intent exists with no artifact | skills `ashfall-shader-expansion-fx`, `ashfall-shader-material-lint`, `ashfall-tilemap-expansion-kit`, `ashfall-tilemap-world-qa`, `ashfall-godot-scene-lint`, `ashfall-foundry` describe exactly this layer; `docs/visual/ASSET_GALLERY.md`, `FALLBACK_VISUAL_ASSETS.md` exist |
| 11 | Snapshot suite will be the safety net | 30 golden images at 1280×800 + `ashfall-snapshot-diff` (Wave 8's 50C) — visual work can be gated rather than argued |

---

## Task 51A — The holdfast as a place: mount the interior view and make it readable

**Goal:** the bunker becomes something you look at rather than a list of panels — fed by the
authorities that already exist, with no new simulation.

**Files:** `scenes/HoldfastInterior.tscn`, `src/World/HoldfastInteriorView.cs`,
`RoomHotspotView.cs`, `SurvivorActorView.cs`, `src/UI/ShelterPanel.cs`,
`ShelterScheduleSystem.cs` (phases), `PowerGridSystem.cs` (room power),
`ShelterDecorSystem.cs`, `SumpFloodingSystem.cs`, `SkyLayerArmorSystem.cs`,
`ShelterThermalSystem.cs`, 50A's manifest, 52's lighting cues.

### Substeps

1. **Decide the mount** first (document, then implement): an in-panel viewport (`SubViewport` +
   `TextureRect`) or a scene-level view that panels open over. Pick one; the current
   view-inside-a-panel is neither, and it is why the map views ended up orphaned.
2. **Delete the orphan scenes or load them** — `scenes/HoldfastInterior.tscn`'s
   `ForegroundOverlay.texture = null` and empty containers say someone intended this; either finish
   the intent or remove the file (Wave 1's 16A rule applied to scenes: no hollow affordances).
3. **Drive rooms from authority**: room list, power state (23A `IsRoomPowered`), occupancy from the
   schedule/`SleepAssignment`, and décor placement (`ShelterDecorSystem` slots) — the view renders
   state, never keeps its own copy (Invariant 5).
4. **Show the three things a player must read at a glance**: who is where (and their condition via
   24A's verdict), which rooms are dark/unpowered/flooding, and what is being produced right now
   (35's jobs). Everything else is a click into an existing panel.
5. **Hotspots go to panels**: clicking a room opens the panel that owns it, via the same
   `OpenPlayerPanel` seam (17A/31B) and the keyboard-focus rules (37B) — no bespoke navigation.
6. **Bind actors to memory**: a survivor at a bunk whose pair died shows it (41A grief, 44B pair
   history) — the cheapest way to make a room feel occupied.
7. **Map 50A's assets explicitly** through the manifest (`asset_registry.json` rows per room/sprite)
   so nothing resolves by filename luck.
8. **One motion vocabulary, introduced here**: transitions between shelter/map/panel states via
   shared helpers (fade/slide at a fixed duration and easing, currently 0 tweens in the codebase) so
   motion arrives as a rule, not a scatter. Respect 37C's reduce-motion setting from day one.
9. **Perf budget**: the view must not redraw per frame what changes per day — drive visuals from
   day events and state-change signals; verify inside 26C's budget.
10. **Snapshots** of the interior at four states (calm, brownout, flooding, crisis) as the visual
    contract for all later art work.
11. **Tests**: view reads live authority (not a copy), hotspot→panel routing, no-double-instantiation
    across rebind (16C), motion respects the reduce-motion flag, perf within budget.
12. **Run the checklist** + `--asset-registry-selftest` + `verify-fast.sh`.

**DoD:** opening the shelter shows a bunker, and every element of it is a fact from an authority.

---

## Task 51B — The map as a place: wire the wasteland view to the travel graph

**Goal:** make the world spatially legible — nodes, routes, control, knowledge, and danger rendered
from the graph Wave 4's 32A defines, replacing an orphaned 193-line view.

**Files:** `src/World/WastelandMapView.cs`, `MapLocationMarkerView.cs(.tscn)`,
`scenes/WastelandMap.tscn`, `src/UI/MapAtlasPanel.cs` / `MapPanel.cs` / `map_detail`,
`WastelandMapSystem` (32A), `FactionWarMapWidget` (30B), `TriangulationPanel`,
`asset_registry.json`, `ashfall-tilemap-world-qa` / `ashfall-godot-scene-lint`.

### Substeps

1. **Start from the dead code**: `WastelandMapView` has zero references and `MapLocationMarkerView`
   is referenced only by it — decide per-class whether to mount, rewrite, or delete (a 193-line
   orphan is a design decision someone already made; don't make it twice).
2. **Render the graph**: nodes (with knowledge rung from 32C: Unknown→…→Mapped), edges (distance,
   terrain, one-way), and the Holdfast at the centre — all from `WastelandMapSystem`, no coordinates
   invented in the view (store layout in data or derive it deterministically from graph position).
3. **Danger and dose on the map**: the 6 danger tiers (32A step 6) and current ambient dose (20A)
   must be visible where the player decides routes — colour plus shape plus number (37B accessibility).
4. **Control by faction** from 30B's territory percentages, with the *change* highlighted (a border
   that moved since last week), not a static atlas.
5. **Scars**: collapsed landmarks, graves/memorials (41B), and route closures (32B step 9) draw on
   the map as events with dates.
6. **Dispatch from the map**: selecting a destination on the map opens the dispatch flow pre-filled
   with the 32B path preview (hours, fuel, projected dose, risk) — map → decision in one click, which
   is the map's whole justification.
7. **Unrevealed must look unrevealed**: no precise values for places the player hasn't surveyed
   (32C step 3), and no control overlay for factions the player has no channel about (Wave 4's rule).
8. **Motion**: pan/zoom/reveal transitions via 51A step 8's shared vocabulary; respect reduce-motion.
9. **Performance**: 20–40 nodes plus edges is nothing; draw cheaply anyway — no per-frame layout
   recomputation, and cull offscreen detail.
10. **Reconcile with the existing panel map** so there is one spatial surface, not two
    (`MapPanel` + `MapAtlasPanel` + `map_detail` + this view): decide ownership explicitly, retire
    duplicates per Wave 1's 16A discipline.
11. **Tests**: view reads live graph, knowledge gating (no precision leaks), route closure
    rendering, dispatch prefill correctness, rebind safety, and a scene-binding check in
    `SceneBindingSelfTest` style so a regenerated `.tscn` can't silently desync.
12. **Snapshots** of the map at four knowledge depths; run `ashfall-godot-scene-lint` over the
    result.
13. **Run the checklist** + gates.

**DoD:** the map is where route, dose, territory, and ignorance become one readable picture.

---

## Task 51C — Light, weather, and motion: the shader/atmosphere layer (bounded)

**Goal:** the smallest visual layer that makes state *felt* — a handful of shaders and a shared
motion/grade vocabulary — introduced with lint and budget, not as an effects program.

**Files:** new `assets/shaders/*.gdshader` (count: 0 today), `assets/ui/materials/`,
`src/UI/DesignTheme`/theme resources, viewport effects for the shelter/map views,
`WeatherSystem`/`PowerGridSystem`/`ShelterThermalSystem` as inputs, `WeatherPanel.cs`,
`ashfall-shader-expansion-fx` + `ashfall-shader-material-lint` as the review procedures,
`docs/visual/`, `docs/perf/BUDGETS.md`.

### Substeps

1. **Set a ceiling before writing anything**: ≤6 shaders total, each with a named state input and a
   measured cost. Shader sprawl is the visual twin of fake consoles.
2. **The three that earn their keep first**: interior light falloff tied to power/lamps (23A
   `lightingDemand`), surface weather grade tied to `WeatherKind` (20C), and a contamination/dust
   overlay tied to dose/shielding (20A/20B). Each maps a mechanic to a sensation the player can
   already name.
3. **State-driven, never time-driven**: uniforms update on day/state change, not per frame; a
   shader that animates must declare its cost in 26C's budget.
4. **Colour science with discipline**: a single palette/grade resource so weather and lighting don't
   fight the UI palette (`DESIGN_SYSTEM_RULES.md`), and contrast stays measurable after grading
   (37C, 50C).
5. **Reduce-motion / accessibility parity**: every grade and animation has a reduced mode; flashing
   limits respected (photosensitivity is a real shipping constraint).
6. **`.import`/material lint**: run `ashfall-shader-material-lint` (filter/mip/compression) and
   `ashfall-godot-scene-lint` (ext_resource/UID/orphan nodes) as gates on new material work, since
   the Unity→Godot port explicitly required porting import settings.
7. **Snapshot the graded states**: the 50C snapshot set gains lit/unlit, clear/storm, contaminated
   variants — a regression in a shader is otherwise invisible to tests.
8. **No post-processing chain** on `gl_compatibility` beyond what each effect earns; measure frame
   time on the minimum-spec target before merging, not after.
9. **Ambience handoff**: define where light ends and sound begins (Plan 52 owns beds, stings, and
   ducking) so weather isn't expressed twice, inconsistently.
10. **Document each shader** in `docs/visual/`: purpose, inputs, cost, reduced-motion behaviour,
    owner — and cite the file:line that proves the state binding (Wave 3's 29B, applied to art).
11. **Tests**: uniform updates on state change (not per frame), budget assertions, lint gates clean,
    reduce-motion fallback, snapshot diffs.
12. **Run the checklist** + `--runtime-scale-selftest` + asset gate.

**DoD:** ≤6 shaders, each proving a mechanic, each measurable, none gratuitous.

---

## Cross-Task Dependencies

```
50A (asset manifest) ──► 51A step 7, 51B step 2   45A (ladder) ──► "used art" is now measurable
32A/32B/32C (graph, routes, knowledge) ──► 51B   23A/20A/20B (power, dose, shielding) ──► 51A/51C
24A/24B/41A/44B (who, how, grieving) ──► 51A step 6   37B/37C (focus, reduce-motion) ──► all
50C (snapshots) ──► every step 10/12 above        52 (sound) ◄──► 51C step 9 (one weather expression)
```

**Execution order:** 50A → 51A → 51B → 51C. Wave 8 order: 50A → 50B → **51A** → 52A → **51B** →
50C → **51C** → 52B → 53 → 52C → 54 (visual and audio tracks interleave; both need 50A's mapping).

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. bash scripts/ci/godot-asset-gate.sh                           # assets + expansions
7. bash scripts/ci/scene-lint.py (ashfall-godot-scene-lint)      # new scenes/materials
8. ashfall-shader-material-lint + ashfall-tilemap-world-qa       # (51B/51C)
9. godot --headless --path . -- --runtime-scale-selftest         # frame/day budget after motion
10. ashfall-snapshot-diff: lit/unlit, calm/crisis, knowledge depths
11. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Code | Scenes/Art | Shaders | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|
| 51A | 2–3 | mount + finish 1 scene | 0–1 | 8–12 | Medium | LOW (a view is presentation) |
| 51B | 2 (mostly wiring dead views) | 1 scene | 0 | 8–12 | Medium | LOW–MED (two map surfaces must merge) |
| 51C | 1–2 | materials | ≤6 | 6–10 + snapshots | Medium | MEDIUM (perf/contrast) |

**Guardrails:** no new simulation in a view (Invariant 5 — views read authorities, never own state);
no renderer change; no 3D, no parallax scenery, no particle systems without a named mechanic; no
second spatial surface; no shader whose state input can't be pointed at in file:line terms; and no
motion that can't be turned off — a management game earns its legibility before its spectacle.
