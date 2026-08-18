# ASHFALL Day 1 First-Playable Vertical Slice

## Summary

Build a 10–15 minute Godot-only loop:

New Game → full-screen Holdfast interior → select survivor → click room to move → inspect five rooms → complete three Day 1 directives → open a four-node wasteland map → return home → save → Continue restores Day 1/Day 2 state.

Use original graphic-novel panels inspired by post-disaster survival fiction, without copying This War of Mine art, UI, names, text, or layouts.

Current blockers to clear first:

- dotnet build Ashfall.csproj is red from three existing ExpeditionEncounterBridge delegate mismatches in src/Main.cs.
- The current interior files are incomplete: hardcoded rooms/survivors, no Core binding, no dashboard route, and an invalid placeholder_survivor.png.
- Reproduce and fix the Godot headless user://logs crash before accepting the slice.
- Preserve unrelated dirty-worktree changes.

## Milestone 0 — Restore a Green Godot Baseline

1. Align Main.OnExpeditionEncounterSurfaced with ExpeditionEncounterBridge.EncounterSurfaced.
2. Remove duplicate using directives.
3. Retain the valid numeric window/size/mode setting in project.godot.
4. Ensure headless startup can create/use its log directory without crashing.
5. Run the canonical Core and Godot verification commands before adding new gameplay.

Deliverable: host build passes and headless data/bridge checks execute reliably.

## Milestone 1 — Repair the Holdfast Interior

Salvage and rework:

- src/World/HoldfastInteriorView.cs
- src/World/RoomHotspotView.cs
- src/World/SurvivorActorView.cs
- scenes/HoldfastInterior.tscn

Implementation rules:

- Instantiate the packed scene once when the interior opens; do not create it from UpdateHud().
- Bind to StartingLevelHostSession.System.State.rooms and SurvivorsHostSession.RosterState.
- Render all five canonical rooms: corridor, filtration stack, storage bay, bunk quarters, and radio tuner.
- Render the three starting survivors from the roster.
- Use view-owned room and actor anchors; do not add visual coordinates to Core save data.
- Select a survivor, then click a room. The selected actor moves there with a short tween and remains selected.
- Clicking a room always calls the existing InspectRoom(roomId) host method.
- Clicking a survivor displays health, needs, morale, and radiation status.
- Use transparent Godot Button/Control hotspots instead of relying on fragile physics picking.
- Add Back, Map, Save, and Protocol controls.
- Keep the existing OpeningProtocolModal and StartingLevelSystem as the authority for Day 1 decisions.
- Replace the invalid placeholder texture with a valid Flow cutout or a temporary procedural silhouette.

Required view events:

~~~csharp
Bind(StartingLevelHostSession startingLevel,
     SurvivorsHostSession survivors);

event Action<string> RoomInspectionRequested;
event Action<string> SurvivorSelected;
event Action MapRequested;
event Action BackRequested;
~~~

Main owns the state-changing calls; the view only presents state and movement.

## Milestone 2 — Google Flow Asset Production

Use Google Flow manually, with no MCP dependency.

First asset batch:

1. day1_holdfast_interior_bg
   - 1920×1080
   - original graphic-novel bunker cutaway
   - five readable room zones
   - no baked UI, labels, logos, flags, or text

2. day1_wasteland_map_bg
   - 1920×1080
   - Holdfast building as the home landmark
   - three distant route nodes connected by hand-drawn lines
   - no baked labels

3. Full-body transparent PNG sprites:
   - survivor_dr_sarah_chen
   - survivor_gunner_mikhail
   - elena_vasquez
   - 512×768 or similar vertical format
   - consistent feet/bottom pivot
   - neutral standing pose plus optional walk-frame variant

4. Optional Flow video:
   - 5–8 second silent bunker/ashfall atmosphere clip
   - store as reference or use FFmpeg to extract approved stills
   - do not make runtime video a Day 1 dependency

Handoff process:

generated_AIassets/flow/day1/<asset-id>/ → QA → approved manifest entry → assets/art or assets/sprites → Godot import → runtime check

Use:

- Krita for alpha cleanup and sprite separation.
- FFmpeg for video frame extraction.
- ImageMagick/Pillow for resizing and technical checks.
- Existing tools/production_qa.py, promotion, gallery, and wiring-trace scripts.
- Git LFS for large PNG assets.

Each asset receives a named manifest record containing source, purpose, dimensions, alpha policy, approval state, and runtime target.

## Milestone 3 — Full-Screen Wasteland Map

Add:

- scenes/WastelandMap.tscn
- src/World/WastelandMapView.cs
- a small map-node view component

Map nodes:

- Home: loc_bunker_holdfast
- loc_grange_hall
- loc_apiary_rows
- loc_seed_library_annex

Behavior:

- Full-screen graphic map with the Holdfast building clearly visible.
- Clicking Home returns to the interior.
- Clicking one of the three locations opens the existing location detail panel.
- No expedition, travel, combat, inventory consumption, or resource consequences yet.
- All non-home IDs must resolve through loaded catalog data; missing IDs fail the self-test instead of silently inventing content.

## Milestone 4 — Main Flow and Persistence

Update Main to provide explicit lifecycle methods:

- OpenHoldfastInterior()
- CloseHoldfastInterior()
- OpenDay1Map()
- CloseDay1Map()
- RefreshDay1Views()

StartNewGame() should:

1. Reset sessions.
2. Create starting level, survivors, inventory, and world state.
3. Open the Holdfast interior.
4. Show the opening protocol modal over the interior.

ContinueGame() should:

1. Restore existing save stores.
2. Rebuild the interior presentation.
3. Reset actors to authored visual anchors.
4. Preserve rooms, decisions, survivor needs, inventory, and day number.

The first playable loop ends when the player completes the three existing directives and advances through the existing day-tick path to Day 2.

## Verification and Acceptance

Add or extend a Godot host self-test in src/Host/HostCli.cs to verify:

- five room IDs are present;
- three starting survivor IDs are present;
- four map IDs resolve;
- interior and map scenes load;
- required textures exist and load;
- room inspection changes Core state;
- save/load restores Day 1/Day 2 state;
- actor coordinates remain presentation-only.

Manual acceptance:

1. Launch with godot --path .
2. Click New Game.
3. See the Holdfast interior full-screen.
4. Select each survivor and move them to rooms.
5. Inspect all five rooms.
6. Resolve ration, maintenance, and radio directives.
7. Open the map.
8. Click the building and three map nodes.
9. Save, return to menu, click Continue.
10. Confirm state returns without crashes or missing-asset warnings.

Canonical checks after each accepted deliverable:

~~~bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --day1-playable-selftest
godot --headless --path . -- --asset-registry-selftest
~~~

Each coupled system must receive an independent diff/code review by a different tool or review pass.

## Tool Roles

- Godot: scenes, input, runtime.
- .NET: build and Core tests.
- Krita: cutouts and cleanup.
- Flow: original backgrounds, sprite references, and atmosphere clips.
- FFmpeg: extract Flow video frames.
- ImageMagick/Pillow: technical QA.
- Tiled and Blender: defer until modular maps or 3D assets become necessary.

## Delivery Order

1. Baseline build/headless repair.
2. Interior scene and interaction shell.
3. First Flow batch wired into the interior.
4. Full-screen map and location inspection.
5. Save/Continue integration.
6. Manual playthrough and all verification gates.
7. Commit each accepted milestone separately.

## Next Implementation Prompt

~~~text
Implement ASHFALL’s Day 1 first-playable vertical slice from the approved plan.

Work Godot-only. Do not invoke Unity. Preserve unrelated dirty-worktree changes.

Start with Milestone 0: restore dotnet build success by fixing the existing ExpeditionEncounterBridge delegate mismatch, remove duplicate usings, and make Godot headless logging reliable.

Then complete Milestone 1 by salvaging the existing HoldfastInterior scene and view files. Bind them to StartingLevelHostSession and SurvivorsHostSession, render all five canonical rooms and three roster survivors, support select-survivor-then-click-room movement, route room inspection through StartingLevelSystem, add Back/Map/Save/Protocol controls, and remove the UpdateHud initialization side effect.

Do not add visual positions to Core save data. Use a valid runtime texture or procedural silhouette until the manually supplied Flow assets are approved. Add the smallest appropriate headless self-test for the interior contract and run the full canonical verification checklist.
~~~
