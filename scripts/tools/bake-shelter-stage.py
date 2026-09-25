#!/usr/bin/env python3
# SPDX-License-Identifier: MIT
"""ASHFALL — Holdfast shelter stage 2D bake (Blender headless).

Bakes the shelter interior stage art from a purpose-built 3D scene into the
2D sprites the Godot runtime consumes (same filenames, same sizes):

  assets/sprites/Shelter/shelter_interior_{day1_7,dawn,dusk,night}.png  (760x420)
  assets/sprites/Shelter/prop_{supply_crate,water_barrel,hatch_door}.png (128x128 RGBA)

Construction is a "theater flat": a front-facing orthographic camera at 2x
render scale, with wall / floor / ceiling panels mapped 1:1 onto the 760x420
pixel grid, so every feature lands at an exact pixel row:

  - floor line (actor feet, HoldfastInteriorView.FloorStandY = 332) at py 330
  - ceiling band  py 0..70      (conduits + pendant lamps)
  - wall band     py 70..330    (calm zone py 140..265 left clean for the
                                 dynamic room-hotspot badges/icons)
  - floor band    py 330..420

The three overlay props are baked as isolated transparent sprites with baked
contact shadows so they composite onto any phase backdrop, and the runtime
CanvasModulate keeps tinting them per lighting phase.

Usage (from repo root):
  blender --background --python scripts/tools/bake-shelter-stage.py -- \
      --out artifacts/shelter-bake/raw [--only backdrops,props] [--phases all]

Deterministic: fixed geometry, fixed materials, fixed Cycles seed. Post-
process (downscale to final size, palette grade) is done with ImageMagick by
the caller, not in Blender.
"""

import math
import sys

import bmesh
import bpy

# ── pixel grid ────────────────────────────────────────────────────────────
W, H = 760, 420
FLOOR_PY = 330          # actor feet line (FloorStandY 332, physics floor 344)
CEIL_PY = 70
FLOOR_ANG = math.radians(35.0)   # floor flat tilt toward camera
CEIL_ANG = math.radians(40.0)    # ceiling fascia tilt
RENDER_SCALE = 2                  # 1520x840 renders, downscaled later

# Scene authored in pixel units, scaled to meters at creation time so Cycles
# light falloff behaves like a real room (760 px -> 7.6 m).
S = 0.01


def sc(v):
    return v * S


def sc3(t):
    return (t[0] * S, t[1] * S, t[2] * S)

# Overlay prop anchor x positions (runtime AddBackgroundProp) that the
# backdrop must leave calm: crate x~64, barrel x~694, hatch x~714 y~226.
OVERLAY_X = (64, 694, 714)

CTX = bpy.context


def link(ob):
    """Link into the CURRENT scene collection (valid across scene resets)."""
    CTX.scene.collection.objects.link(ob)
    return ob

# ── palette (DESIGN.md: charcoal / green-white #c7dcd0 / rust accents) ────
HEX = {
    "wall":        (0x24, 0x27, 0x2E),
    "wall_dark":   (0x1B, 0x1E, 0x24),
    "wall_rib":    (0x2F, 0x34, 0x3C),
    "floor":       (0x30, 0x35, 0x3F),
    "floor_worn":  (0x3C, 0x42, 0x4D),
    "floor_dark":  (0x1F, 0x22, 0x29),
    "ceil":        (0x20, 0x23, 0x29),
    "steel":       (0x45, 0x4B, 0x54),
    "steel_dark":  (0x34, 0x39, 0x40),
    "rust":        (0x6E, 0x3F, 0x28),
    "rust_light":  (0x8A, 0x4A, 0x2B),
    "wood":        (0x4A, 0x3A, 0x2C),
    "wood_dark":   (0x39, 0x2C, 0x22),
    "hazard":      (0x8F, 0x72, 0x2C),
    "paper":       (0x9C, 0x96, 0x86),
    "lamp_glow":   (0xC7, 0xDC, 0xD0),
    "stain":       (0x17, 0x19, 0x1E),
}


def rgb(name):
    return tuple(c / 255.0 for c in HEX[name])


# ── mesh helpers (explicit coordinates, no RNG) ───────────────────────────
def make_mat(name, base, rough=0.95, metal=0.0, emission=None, emit_strength=1.0):
    mat = bpy.data.materials.get(name)
    if mat:
        return mat
    mat = bpy.data.materials.new(name)
    mat.use_nodes = True
    bsdf = mat.node_tree.nodes.get("Principled BSDF")
    bsdf.inputs["Base Color"].default_value = (*base, 1.0)
    bsdf.inputs["Roughness"].default_value = rough
    if "Metallic" in bsdf.inputs:
        bsdf.inputs["Metallic"].default_value = metal
    if emission is not None:
        bsdf.inputs["Emission Color"].default_value = (*emission, 1.0)
        bsdf.inputs["Emission Strength"].default_value = emit_strength
    return mat


def mesh_from_corners(name, corners, mat):
    me = bpy.data.meshes.new(name)
    ob = bpy.data.objects.new(name, me)
    link(ob)
    bm = bmesh.new()
    verts = [bm.verts.new(sc3(c)) for c in corners]
    bm.faces.new(verts)
    bm.normal_update()
    bm.to_mesh(me)
    bm.free()
    ob.data.materials.append(mat)
    return ob


def box(name, center, size, mat):
    cx, cy, cz = sc3(center)
    sx, sy, sz = (s * S / 2.0 for s in size)
    corners = [
        (cx - sx, cy - sy, cz - sz), (cx + sx, cy - sy, cz - sz),
        (cx + sx, cy + sy, cz - sz), (cx - sx, cy + sy, cz - sz),
        (cx - sx, cy - sy, cz + sz), (cx + sx, cy - sy, cz + sz),
        (cx + sx, cy + sy, cz + sz), (cx - sx, cy + sy, cz + sz),
    ]
    faces_idx = [
        (0, 1, 2, 3), (4, 7, 6, 5), (0, 4, 5, 1),
        (2, 6, 7, 3), (1, 5, 6, 2), (0, 3, 7, 4),
    ]
    me = bpy.data.meshes.new(name)
    ob = bpy.data.objects.new(name, me)
    link(ob)
    bm = bmesh.new()
    v = [bm.verts.new(c) for c in corners]
    for f in faces_idx:
        bm.faces.new((v[i] for i in f))
    bm.normal_update()
    bm.to_mesh(me)
    bm.free()
    ob.data.materials.append(mat)
    return ob


def cylinder(name, loc, radius, depth, mat, axis="Z", verts=24):
    bpy.ops.mesh.primitive_cylinder_add(vertices=verts, radius=sc(radius),
                                        depth=sc(depth), location=sc3(loc),
                                        end_fill_type="NGON")
    ob = bpy.context.active_object
    ob.name = name
    ob.rotation_euler = {
        "Z": (0, 0, 0),
        "Y": (math.radians(90), 0, 0),
        "X": (0, math.radians(90), 0),
    }[axis]
    ob.data.materials.append(mat)
    return ob


def cone(name, loc, r_bottom, r_top, depth, mat, verts=20):
    bpy.ops.mesh.primitive_cone_add(vertices=verts, radius1=sc(r_bottom),
                                    radius2=sc(r_top), depth=sc(depth),
                                    location=sc3(loc), end_fill_type="NGON")
    ob = bpy.context.active_object
    ob.name = name
    ob.data.materials.append(mat)
    return ob


def sphere(name, loc, radius, mat):
    bpy.ops.mesh.primitive_uv_sphere_add(radius=sc(radius), location=sc3(loc),
                                         segments=16, ring_count=10)
    ob = bpy.context.active_object
    ob.name = name
    ob.data.materials.append(mat)
    return ob


# ── stage geometry (py = pixel row from top; z = H - py) ─────────────────
# Depth convention: the camera sits at -Y looking +Y. The back wall is the
# farthest surface (y=0); everything room-side (floor, lights, fixtures,
# proud details) extends toward the camera at NEGATIVE y. Elements built at
# positive y would be hidden behind the wall.
def wall_point(px, py, y=0.0):
    return (float(px), float(y), float(H - py))


def floor_point(px, d, lift=0.0):
    """Point on the floor flat; px across, d = distance from wall hinge."""
    y = -d * math.sin(FLOOR_ANG) - lift * math.cos(FLOOR_ANG)
    z = (H - FLOOR_PY) - d * math.cos(FLOOR_ANG) + lift * math.sin(FLOOR_ANG)
    return (float(px), float(y), float(z))


def ceil_point(px, d, lift=0.0):
    y = -d * math.sin(CEIL_ANG) - lift * math.cos(CEIL_ANG)
    z = (H - CEIL_PY) + d * math.cos(CEIL_ANG) + lift * math.sin(CEIL_ANG)
    return (float(px), float(y), float(z))


def build_stage():
    mat_wall = make_mat("ash_wall", rgb("wall"))
    mat_wall_dark = make_mat("ash_wall_dark", rgb("wall_dark"))
    mat_wall_rib = make_mat("ash_wall_rib", rgb("wall_rib"))
    mat_floor = make_mat("ash_floor", rgb("floor"))
    mat_floor_worn = make_mat("ash_floor_worn", rgb("floor_worn"), rough=0.9)
    mat_floor_dark = make_mat("ash_floor_dark", rgb("floor_dark"))
    mat_ceil = make_mat("ash_ceil", rgb("ceil"))
    mat_steel = make_mat("ash_steel", rgb("steel"), rough=0.6, metal=0.35)
    mat_steel_dark = make_mat("ash_steel_dark", rgb("steel_dark"), rough=0.7, metal=0.2)
    mat_rust = make_mat("ash_rust", rgb("rust"), rough=0.95)
    mat_rust_l = make_mat("ash_rust_light", rgb("rust_light"), rough=0.95)
    mat_wood = make_mat("ash_wood", rgb("wood"))
    mat_wood_dark = make_mat("ash_wood_dark", rgb("wood_dark"))
    mat_hazard = make_mat("ash_hazard", rgb("hazard"), rough=0.9)
    mat_paper = make_mat("ash_paper", rgb("paper"), rough=1.0)
    mat_stain = make_mat("ash_stain", rgb("stain"))
    mat_bulb = make_mat("ash_bulb", rgb("lamp_glow"), emission=rgb("lamp_glow"), emit_strength=28.0)

    # Back wall (corners wound to face the camera at -Y)
    mesh_from_corners("wall", [wall_point(0, H), wall_point(W, H),
                               wall_point(W, 0), wall_point(0, 0)], mat_wall)
    # Ceiling fascia: hinge py70 at wall, rises to py0
    d_c = 70.0 / math.cos(CEIL_ANG)
    mesh_from_corners("ceiling_fascia",
                      [ceil_point(0, 0), ceil_point(W, 0), ceil_point(W, d_c), ceil_point(0, d_c)],
                      mat_ceil)
    # Floor flat: hinge py330 at wall, descends to py420
    d_f = 90.0 / math.cos(FLOOR_ANG)
    mesh_from_corners("floor_flat",
                      [floor_point(0, 0), floor_point(W, 0), floor_point(W, d_f), floor_point(0, d_f)],
                      mat_floor)

    # Wall / floor junction shadow + skirting
    mesh_from_corners("junction_shadow",
                      [floor_point(0, 0.2), floor_point(W, 0.2),
                       floor_point(W, 9), floor_point(0, 9)], mat_floor_dark)
    box("skirting", (W / 2, -0.8, H - FLOOR_PY + 5), (W, 1.6, 10), mat_wall_rib)

    # Structural wall ribs (vertical panel joints, proud of the wall)
    for i, px in enumerate((95, 190, 285, 475, 570, 665)):
        box(f"rib_{px}", (px, -1.2, H - (CEIL_PY + FLOOR_PY) / 2),
            (7, 2.4, FLOOR_PY - CEIL_PY - 10), mat_wall_rib)

    # Ceiling conduits (py 16..40) — steel + one rust run
    for py, r, m in ((16, 3.2, mat_steel), (24, 4.0, mat_rust_l), (34, 2.6, mat_steel_dark)):
        cylinder(f"conduit_{py}", (W / 2, -8, H - py), r, W, m, axis="Y")

    # Vertical pipe drops (kept off hotspot x columns)
    for px, r, m in ((46, 5.0, mat_rust), (722, 5.0, mat_steel)):
        cylinder(f"vpipe_{px}", (px, -6, (H - FLOOR_PY + H - CEIL_PY) / 2), r, FLOOR_PY - CEIL_PY, m)

    # Pendant lamps (x 180 / 380 / 580 — above calm zone)
    for px in (180, 380, 580):
        cone(f"lamp_shade_{px}", (px, -26, H - 60), 13, 5, 16, mat_steel)
        cylinder(f"lamp_stem_{px}", (px, -26, H - 49), 1.6, 24, mat_steel)
        sphere(f"lamp_bulb_{px}", (px, -26, H - 72), 5.0, mat_bulb)

    # Notice board (py 96..136) — mounted BETWEEN lamps (x~280) so papers
    # stay readable instead of blowing out under a nearby bulb
    box("board", (280, -1.8, H - 116), (56, 1.2, 40), mat_wood)
    for dx, dy, wdt, hgt in ((-18, 4, 16, 20), (2, 0, 14, 24), (19, 6, 12, 16)):
        box(f"paper_{dx}", (280 + dx, -2.6, H - 116 + dy), (wdt, 0.6, hgt), mat_paper)

    # Vent grilles (py ~118, flanking) with slats
    for vx in (136, 596):
        box(f"vent_{vx}", (vx, -1.6, H - 120), (34, 1.2, 22), mat_steel_dark)
        for s in range(4):
            box(f"ventslat_{vx}_{s}", (vx, -2.4, H - 129 + s * 6), (28, 0.6, 2.2), mat_steel)

    # Kicked door frame (kitchen entry, x 400..462) reaching the floor line
    box("doorframe_l", (403, -1.6, H - 240), (6, 3.2, 180), mat_wall_dark)
    box("doorframe_r", (459, -1.6, H - 240), (6, 3.2, 180), mat_wall_dark)
    box("doorframe_t", (431, -1.6, H - 146), (62, 3.2, 6), mat_wall_dark)
    box("door_leaf", (431, 0.8, H - 241), (50, 1.2, 178), mat_wall_dark)
    cylinder("door_handle", (450, -2.2, H - 262), 1.6, 3, mat_steel, axis="Y")

    # Wall stains / moisture (calm, low contrast)
    for sx, sy, wdt, hgt in ((230, 296, 60, 34), (612, 306, 44, 24), (70, 286, 34, 44)):
        box(f"stain_{sx}", (sx, -0.4, H - sy), (wdt, 0.4, hgt), mat_stain)

    # Worn walk path on the floor + control joints + drain
    mesh_from_corners("walkpath",
                      [floor_point(150, 22, 0.06), floor_point(610, 22, 0.06),
                       floor_point(610, 34, 0.06), floor_point(150, 34, 0.06)], mat_floor_worn)
    for jx in (250, 430, 610):
        mesh_from_corners(f"joint_{jx}",
                          [floor_point(jx, 2, 0.08), floor_point(jx + 3, 2, 0.08),
                           floor_point(jx + 3, d_f - 4, 0.08), floor_point(jx, d_f - 4, 0.08)],
                          mat_floor_dark)
    cylinder("drain", floor_point(560, 42, 0.1), 7, 0.8, mat_floor_dark)
    bpy.data.objects["drain"].rotation_euler = (FLOOR_ANG, 0, 0)

    # Muted painted hazard line on the floor front edge (worn)
    mesh_from_corners("hazard_line",
                      [floor_point(430, d_f - 16, 0.07), floor_point(560, d_f - 16, 0.07),
                       floor_point(560, d_f - 10, 0.07), floor_point(430, d_f - 10, 0.07)],
                      mat_hazard)

    return {"lamps": [180, 380, 580]}


# ── lighting rigs ─────────────────────────────────────────────────────────
LAMP_X = (180, 380, 580)


def clear_lights():
    for ob in list(bpy.data.objects):
        if ob.type == "LIGHT":
            bpy.data.objects.remove(ob, do_unlink=True)


def lamp_lights(strength, color):
    lights = []
    for px in LAMP_X:
        ld = bpy.data.lights.new(f"lamp_light_{px}", "POINT")
        ld.energy = strength
        ld.color = color
        ld.shadow_soft_size = sc(3.0)
        ob = bpy.data.objects.new(f"lamplight_{px}", ld)
        # hang BELOW the bulb surface, on the camera side of the wall
        ob.location = sc3((px, -26, H - 86))
        link(ob)
        lights.append(ob)
    return lights


def area_light(name, loc, size, energy, color, rot):
    ld = bpy.data.lights.new(name, "AREA")
    ld.shape = "SQUARE"
    ld.size = sc(size)
    ld.energy = energy
    ld.color = color
    ob = bpy.data.objects.new(name, ld)
    ob.location = sc3(loc)
    ob.rotation_euler = rot
    link(ob)
    return ob


GLOW = (0.78, 0.86, 0.80)     # green-white #c7dcd0
WARM = (0.90, 0.74, 0.52)     # restrained amber
RUSTD = (0.82, 0.62, 0.46)    # rust dusk
COOLB = (0.55, 0.62, 0.74)    # blue-grey night

PHASES = {
    "day1_7": dict(world=(0.42, 0.44, 0.47), world_str=0.40,
                   lamps=60.0, lamp_col=GLOW, bounce=24.0,
                   key=("left_cool", (-300, -120, 420), 700, 200, (0.72, 0.76, 0.82))),
    "dawn":   dict(world=(0.47, 0.38, 0.29), world_str=0.22,
                   lamps=75.0, lamp_col=GLOW, bounce=18.0,
                   key=("dawn_key", (-420, -60, 200), 620, 380, WARM)),
    "dusk":   dict(world=(0.36, 0.30, 0.26), world_str=0.20,
                   lamps=95.0, lamp_col=GLOW, bounce=15.0,
                   key=("dusk_key", (1080, -80, 240), 560, 260, RUSTD)),
    "night":  dict(world=(0.10, 0.12, 0.16), world_str=0.12,
                   lamps=150.0, lamp_col=(0.80, 0.88, 0.84),
                   key=("night_fill", (380, -500, 520), 1200, 60, COOLB), bounce=8.0),
}

BOUNCE = (0.85, 0.82, 0.78)  # warm neutral ceiling bounce (wall reflection)


def apply_phase(name):
    cfg = PHASES[name]
    lights = ensure_lights()
    world = bpy.data.worlds["StageWorld"]
    import os
    if os.environ.get("BAKE_WORLD_OFF"):
        world.node_tree.nodes["Background"].inputs[0].default_value = (0, 0, 0, 1.0)
        world.node_tree.nodes["Background"].inputs[1].default_value = 0.0
    else:
        world.node_tree.nodes["Background"].inputs[0].default_value = (*cfg["world"], 1.0)
        world.node_tree.nodes["Background"].inputs[1].default_value = cfg["world_str"]
    for px in LAMP_X:
        ld = lights[px].data
        ld.energy = cfg["lamps"]
        ld.color = cfg["lamp_col"]
    kn, kloc, ksize, kenergy, kcol = cfg["key"]
    key = lights["phase_key"]
    key.location = sc3(kloc)
    key.rotation_euler = (math.radians(90), 0, 0)
    key.data.size = sc(ksize)
    key.data.energy = kenergy
    key.data.color = kcol
    # soft bounce off the floor toward the ceiling band so conduits/lamps
    # read against the dark fascia without adding a second visible source
    bounce = lights["ceil_bounce"]
    bounce.location = sc3((W / 2, -200, 40))
    bounce.rotation_euler = (math.radians(180), 0, 0)
    bounce.data.size = sc(400)
    bounce.data.energy = cfg.get("bounce", 40.0)
    bounce.data.color = BOUNCE


# ── render setup ──────────────────────────────────────────────────────────
def setup_render(transparent=False):
    scene = CTX.scene
    scene.render.engine = "CYCLES"
    scene.cycles.device = "CPU"
    scene.cycles.samples = 96
    scene.cycles.seed = 20260925
    scene.cycles.use_denoising = True
    scene.render.resolution_x = W * RENDER_SCALE
    scene.render.resolution_y = H * RENDER_SCALE
    scene.render.film_transparent = transparent
    scene.render.image_settings.file_format = "PNG"
    scene.render.image_settings.color_mode = "RGBA"
    scene.view_settings.view_transform = "Standard"
    scene.view_settings.look = "None"


def add_camera():
    cam_data = bpy.data.cameras.new("StageCam")
    cam_data.type = "ORTHO"
    cam_data.ortho_scale = sc(W)
    cam = bpy.data.objects.new("StageCam", cam_data)
    cam.location = sc3((W / 2, -600, H / 2))
    cam.rotation_euler = (math.radians(90), 0, 0)
    link(cam)
    CTX.scene.camera = cam
    return cam


def new_scene():
    bpy.ops.wm.read_factory_settings(use_empty=True)
    scene = CTX.scene
    scene.name = "Bake"
    world = bpy.data.worlds.new("StageWorld")
    world.use_nodes = True
    scene.world = world
    _LIGHTS.clear()


_LIGHTS = {}


def ensure_lights():
    """Create the five stage lights ONCE; phases mutate energy/color/position.

    Deleting and re-creating light objects between renders does not reliably
    propagate to the renderer in one background session, so the rig is built
    once and only its parameters change per phase.
    """
    if _LIGHTS:
        return _LIGHTS
    for px in LAMP_X:
        ld = bpy.data.lights.new(f"lamp_light_{px}", "POINT")
        ld.shadow_soft_size = sc(3.0)
        ob = bpy.data.objects.new(f"lamplight_{px}", ld)
        # hang BELOW the bulb surface, on the camera side of the wall
        ob.location = sc3((px, -26, H - 86))
        link(ob)
        _LIGHTS[px] = ob
    for nm in ("phase_key", "ceil_bounce"):
        ld = bpy.data.lights.new(nm, "AREA")
        ld.shape = "SQUARE"
        ob = bpy.data.objects.new(nm, ld)
        link(ob)
        _LIGHTS[nm] = ob
    return _LIGHTS


# ── prop bakes ────────────────────────────────────────────────────────────
def build_props():
    """Isolated overlay props on a floor patch (contact shadows baked)."""
    mat_wood = make_mat("ash_wood", rgb("wood"))
    mat_wood_dark = make_mat("ash_wood_dark", rgb("wood_dark"))
    mat_steel = make_mat("ash_steel", rgb("steel"), rough=0.6, metal=0.35)
    mat_steel_dark = make_mat("ash_steel_dark", rgb("steel_dark"), rough=0.7, metal=0.2)
    mat_rust = make_mat("ash_rust", rgb("rust"), rough=0.95)
    mat_rust_l = make_mat("ash_rust_light", rgb("rust_light"), rough=0.95)
    mat_floor = make_mat("ash_floor", rgb("floor"))

    mesh_from_corners("prop_floor",
                      [(-80, -110, -70), (80, -110, -70), (80, 0, 0), (-80, 0, 0)], mat_floor)

    # supply crate: two stacked wooden crates (top crate lighter so the
    # silhouette separates at 64px) + wide rust strap
    mat_wood_l = make_mat("ash_wood_light", (0x5A, 0x48, 0x36))
    box("crate_lo", (0, -14, 14), (40, 30, 27), mat_wood)
    box("crate_lo_edge1", (0, -14, 14), (41, 31, 3), mat_wood_dark)
    box("crate_hi", (4, -10, 40), (32, 24, 22), mat_wood_l)
    box("crate_hi_edge1", (4, -10, 40), (33, 25, 2.6), mat_wood_dark)
    box("crate_strap", (4, -10, 27), (33.6, 25.6, 4.5), mat_rust_l)

    # water barrel: rust-banded drum, top rim
    cylinder("barrel", (0, -12, 27), 16, 54, mat_steel, axis="Z", verts=28)
    cylinder("barrel_rim", (0, -12, 54), 16.6, 3, mat_steel_dark)
    cylinder("barrel_band_lo", (0, -12, 12), 16.8, 4, mat_rust)
    cylinder("barrel_band_hi", (0, -12, 42), 16.8, 4, mat_rust_l)
    cylinder("barrel_cap", (0, -12, 55.4), 6, 1.6, mat_steel_dark)

    # hatch door: wall-mounted circular steel plate + wheel; the rim is a
    # bold ring (no small bolts — they read as artifacts at game scale) and
    # an inner darker disc gives the plate slight depth
    cylinder("hatch_plate", (0, 0, 0), 34, 7, mat_steel, axis="Y", verts=32)
    cylinder("hatch_rim", (0, -3.8, 0), 35.2, 3.6, mat_steel_dark, axis="Y", verts=32)
    cylinder("hatch_inner", (0, -3.7, 0), 29, 1.2, mat_steel_dark, axis="Y", verts=32)
    bpy.ops.mesh.primitive_torus_add(location=sc3((0, -6.5, 0)), major_radius=sc(12),
                                     minor_radius=sc(1.7), major_segments=28, minor_segments=10)
    wheel = bpy.context.active_object
    wheel.name = "hatch_wheel"
    wheel.rotation_euler = (math.radians(90), 0, 0)
    wheel.data.materials.append(mat_rust)
    for ang in (0, 60, 120):
        a = math.radians(ang)
        cylinder(f"hatch_spoke_{ang}", (0, -6.5, 0), 1.1, 24, mat_rust, axis="X")
        spoke = bpy.data.objects[f"hatch_spoke_{ang}"]
        spoke.rotation_euler = (0, a, 0)
    cylinder("hatch_hub", (0, -6.5, 0), 2.6, 4, mat_steel_dark, axis="Y")


PROPS = {
    "prop_supply_crate": dict(center=(0, 28), objs=("crate",), span=110),
    "prop_water_barrel": dict(center=(0, 27), objs=("barrel",), span=118),
    "prop_hatch_door": dict(center=(0, 0), objs=("hatch",), span=112),
}


def bake_props(out_dir):
    new_scene()
    add_camera()
    build_props()
    setup_render(transparent=True)
    # prop lighting: neutral stage light + soft fill, day-authoritative
    clear_lights()
    area_light("prop_key", (-160, -140, 220), 180, 110, (0.80, 0.83, 0.87),
               (math.radians(50), math.radians(18), 0))
    area_light("prop_fill", (220, -60, 160), 140, 30, (0.60, 0.66, 0.72),
               (math.radians(80), 0, 0))
    world = bpy.data.worlds["StageWorld"]
    world.node_tree.nodes["Background"].inputs[0].default_value = (0.05, 0.05, 0.06, 1.0)
    world.node_tree.nodes["Background"].inputs[1].default_value = 0.4

    keep_prefix = {"prop_supply_crate": "crate", "prop_water_barrel": "barrel",
                   "prop_hatch_door": "hatch"}
    for name, spec in PROPS.items():
        prefix = keep_prefix[name]
        keep_floor = name != "prop_hatch_door"  # wall-mounted: no contact shadow
        for ob in CTX.scene.collection.objects:
            ob.hide_render = not (ob.name.startswith(prefix)
                                  or (keep_floor and ob.name == "prop_floor"))
        cam = bpy.data.objects["StageCam"]
        # tight ortho camera around the prop, front view
        cam.location = sc3((spec["center"][0], -300, spec["center"][1]))
        cam.data.ortho_scale = sc(spec["span"])
        CTX.scene.render.resolution_x = 256
        CTX.scene.render.resolution_y = 256
        CTX.scene.render.filepath = f"{out_dir}/{name}_raw.png"
        bpy.ops.render.render(write_still=True)
        print(f"[bake] prop {name} -> {CTX.scene.render.filepath}")


# ── backdrop bakes ────────────────────────────────────────────────────────
def bake_backdrops(out_dir, phases):
    new_scene()
    add_camera()
    stage = build_stage()
    setup_render(transparent=False)
    for phase in phases:
        apply_phase(phase)
        if bpy.app.debug_value == 1:
            bg = bpy.data.worlds["StageWorld"].node_tree.nodes["Background"]
            tot = sum(o.data.energy for o in CTX.scene.objects if o.type == "LIGHT")
            print(f"[phase] {phase}: world_str={bg.inputs[1].default_value} "
                  f"world_col={tuple(round(c,3) for c in bg.inputs[0].default_value[:3])} "
                  f"lights={sum(1 for o in CTX.scene.objects if o.type == 'LIGHT')} total_energy={tot}")
        if bpy.app.debug_value == 1:
            for ob in CTX.scene.objects:
                if ob.type == "MESH" and ob.name in ("wall", "floor_flat", "lamp_bulb_380", "door_leaf"):
                    ws = [ob.matrix_world @ Vertex for Vertex in ob.bound_box] if False else \
                         [tuple(ob.matrix_world @ __import__("mathutils").Vector(c)) for c in ob.bound_box]
                    print(f"[bbox] {ob.name}: min={tuple(round(min(v[i] for v in ws),3) for i in range(3))} "
                          f"max={tuple(round(max(v[i] for v in ws),3) for i in range(3))}")
            cam = CTX.scene.camera
            print(f"[cam] loc={tuple(round(c,3) for c in cam.location)} rot={tuple(round(math.degrees(r),1) for r in cam.rotation_euler)} "
                  f"ortho={cam.data.ortho_scale} type={cam.data.type} clip={cam.data.clip_start}..{cam.data.clip_end}")
            print(f"[scene] world={CTX.scene.world.name} camera={CTX.scene.camera.name if CTX.scene.camera else None}")
        CTX.scene.render.resolution_x = W * RENDER_SCALE
        CTX.scene.render.resolution_y = H * RENDER_SCALE
        CTX.scene.render.filepath = f"{out_dir}/shelter_interior_{phase}_raw.png"
        bpy.ops.render.render(write_still=True)
        print(f"[bake] backdrop {phase} -> {CTX.scene.render.filepath}")


# ── main ──────────────────────────────────────────────────────────────────
def main():
    args = sys.argv[sys.argv.index("--") + 1:] if "--" in sys.argv else []
    out_dir = "artifacts/shelter-bake/raw"
    only = "backdrops,props"
    phases = list(PHASES.keys())
    it = iter(args)
    for a in it:
        if a == "--out":
            out_dir = next(it)
        elif a == "--only":
            only = next(it)
        elif a == "--phases":
            v = next(it)
            phases = list(PHASES.keys()) if v == "all" else [p for p in v.split(",") if p in PHASES]

    import os
    os.makedirs(out_dir, exist_ok=True)
    if "backdrops" in only:
        bake_backdrops(out_dir, phases)
    if "props" in only:
        bake_props(out_dir)
    print("[bake] done")


main()
