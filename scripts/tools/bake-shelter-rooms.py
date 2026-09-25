#!/usr/bin/env python3
# SPDX-License-Identifier: MIT
"""ASHFALL — shelter room pictogram + remaining prop bake (Blender headless).

Continues the 2026-09-25 stage bake (scripts/tools/bake-shelter-stage.py)
to the remaining Plan 139 placeholders, replacing them with final 2D art at
identical paths and sizes:

  assets/sprites/Shelter/room_<id>.png  (23 files, 128x128 RGBA)
      Runtime consumer: RoomHotspotView.UpdateIcon draws each pictogram at
      0.5 scale (64x64) above the room badge in the calm band, so every
      vignette is authored to read as a bold silhouette at 64px.
  assets/sprites/Shelter/prop_ceiling_lamp.png   (128x128 RGBA)
  assets/sprites/Shelter/prop_pipe_bundle.png    (128x128 RGBA)
      Manifest-tracked props, not yet wired to runtime anchors.

Each room is a small vignette (3-10 primitives) built from the stage
palette and baked with the same neutral day-authoritative rig the three
shipped props use: front ortho camera, transparent film, contact shadow on
a shared floor patch (hidden for wall/ceiling-mounted pieces).

Usage (from repo root):
  blender --background --python scripts/tools/bake-shelter-rooms.py -- \
      --out artifacts/shelter-bake-rooms/raw

Deterministic: fixed geometry, fixed materials, Cycles seed 20260925.
Post-process (crop/fit to 128 RGBA) and the seam-safe tiles are produced by
scripts/tools/post-shelter-rooms-bake.py, not here.
"""

import math
import sys

import bmesh
import bpy

S = 0.01          # px -> m so Cycles falloff behaves at game scale
CTX = bpy.context

# Room-ID truth: Assets/StreamingAssets/Data/shelter_rooms.json (verified
# 2026-09-26 — this is exactly the set that renders hotspots).


def sc(v):
    return v * S


def sc3(t):
    return (t[0] * S, t[1] * S, t[2] * S)


def link(ob):
    CTX.scene.collection.objects.link(ob)
    return ob


# ── palette (DESIGN.md: charcoal / green-white #c7dcd0 / rust accents) ────
HEX = {
    "wall":        (0x24, 0x27, 0x2E),
    "wall_dark":   (0x1B, 0x1E, 0x24),
    "wall_rib":    (0x2F, 0x34, 0x3C),
    "floor":       (0x30, 0x35, 0x3F),
    "floor_dark":  (0x1F, 0x22, 0x29),
    "steel":       (0x45, 0x4B, 0x54),
    "steel_dark":  (0x34, 0x39, 0x40),
    "rust":        (0x6E, 0x3F, 0x28),
    "rust_light":  (0x8A, 0x4A, 0x2B),
    "wood":        (0x4A, 0x3A, 0x2C),
    "wood_light":  (0x5A, 0x48, 0x36),
    "wood_dark":   (0x39, 0x2C, 0x22),
    "hazard":      (0x8F, 0x72, 0x2C),
    "paper":       (0x9C, 0x96, 0x86),
    "cloth":       (0x5E, 0x6B, 0x63),   # worn olive-grey bedding
    "cloth_dark":  (0x49, 0x54, 0x4C),
    "clinical":    (0xAE, 0xB4, 0xAE),   # pale ward surfaces
    "leaf":        (0x5F, 0x7A, 0x5A),   # desaturated greenhouse green
    "lamp_glow":   (0xC7, 0xDC, 0xD0),
    "amber":       (0xD9, 0xA5, 0x6A),   # restrained forge/beacon warmth
}


def rgb(name):
    return tuple(c / 255.0 for c in HEX[name])


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


def M(name, **kw):
    return make_mat("ash_" + name, rgb(name), **kw)


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


def torus(name, loc, major, minor, mat, rot_x=90):
    bpy.ops.mesh.primitive_torus_add(location=sc3(loc), major_radius=sc(major),
                                     minor_radius=sc(minor), major_segments=24,
                                     minor_segments=10)
    ob = bpy.context.active_object
    ob.name = name
    ob.rotation_euler = (math.radians(rot_x), 0, 0)
    ob.data.materials.append(mat)
    return ob


# ── shared rig ─────────────────────────────────────────────────────────────
# Common floor patch (same geometry the three shipped props cast onto).
FLOOR_CORNERS = [(-80, -110, -70), (80, -110, -70), (80, 0, 0), (-80, 0, 0)]


def common_floor():
    mesh_from_corners("common_floor", FLOOR_CORNERS, M("floor"))


def wall_mark(name, cx, width, mat, z=0.6, depth=2.0):
    """Flat marking lying on the back ground line (feet of the vignette)."""
    return box(name, (cx, -depth / 2.0 - 1.0, z), (width, depth, 1.2), mat)


def bunks(rid, tiers, span, cam_z):
    st, wd, cl = M("steel"), M("wood_dark"), M("cloth")
    for px in (-34, 34):
        box(f"{rid}|post_{px}", (px, -18, 40), (4, 3, 80), st)
        box(f"{rid}|foot_{px}", (px, -18, 0), (10, 6, 3), st)
    for i in range(tiers):
        z = 18 + i * (52 // max(1, tiers - 1)) if tiers > 1 else 26
        box(f"{rid}|frame_{i}", (0, -18, z), (64, 5, 3), wd)
        box(f"{rid}|mattress_{i}", (0, -18, z + 5), (58, 4, 5), cl)
    for r in range(3):  # access ladder, right side
        box(f"{rid}|rung_{r}", (34, -24, 30 + r * 14), (3, 14, 2), st)
    return cam_z, span


def build_room_bunks():
    return bunks("room_bunks", 2, 116, 40)


def build_room_bunks_crowded():
    return bunks("room_bunks_crowded", 3, 118, 40)


def build_room_quarters_private():
    wd, wd_l, cl = M("wood"), M("wood_light"), M("cloth")
    rid = "room_quarters_private"
    box(f"{rid}|bed_frame", (-14, -18, 8), (60, 8, 7), wd)
    box(f"{rid}|mattress", (-14, -18, 14), (54, 7, 5), cl)
    box(f"{rid}|pillow", (-33, -18, 18), (12, 6, 4), M("clinical"))
    box(f"{rid}|blanket", (-4, -18, 17), (26, 7.6, 3), M("cloth_dark"))
    box(f"{rid}|cabinet", (36, -16, 14), (20, 14, 28), wd_l)
    box(f"{rid}|cabinet_top", (36, -16, 29), (22, 15, 2), wd)
    box(f"{rid}|shelf", (36, -12, 44), (26, 4, 2), wd)
    box(f"{rid}|shelfbox", (32, -12, 47), (8, 6, 6), wd_l)
    return 26, 108


def build_room_bunker_corridor():
    rid = "room_bunker_corridor"
    wd, wr, st = M("wall_dark"), M("wall_rib"), M("steel")
    box(f"{rid}|wall_l", (-46, -6, 40), (10, 4, 80), wd)
    box(f"{rid}|wall_r", (46, -6, 40), (10, 4, 80), wd)
    for i, (w, z) in enumerate(((104, 76), (84, 62), (66, 50))):
        box(f"{rid}|rib_{i}", (0, -4 - i * 8, z), (w, 3, 7), wr)
    cylinder(f"{rid}|pipe", (0, -30, 44), 3.4, 96, st, axis="X")
    cylinder(f"{rid}|pipe_small", (0, -34, 52), 2.0, 96, M("rust_light"), axis="X")
    wall_mark(f"{rid}|joint", 0, 60, M("floor_dark"))
    return 40, 116


def build_room_workshop():
    rid = "room_workshop"
    wd, st, sd = M("wood"), M("steel"), M("steel_dark")
    box(f"{rid}|bench_top", (0, -18, 34), (72, 10, 4), wd)
    for px in (-30, 30):
        box(f"{rid}|leg_{px}", (px, -18, 16), (4, 8, 32), st)
    box(f"{rid}|board", (0, -30, 58), (54, 2, 38), M("wood_light"))
    for i, (dx, w, h) in enumerate(((-18, 5, 22), (-4, 4, 26), (10, 6, 18))):
        box(f"{rid}|tool_{i}", (dx, -28, 58), (w, 2, h), sd)
    box(f"{rid}|vise_base", (24, -18, 38), (10, 6, 4), sd)
    box(f"{rid}|vise_jaw", (24, -18, 42), (7, 8, 7), st)
    box(f"{rid}|vise_screw", (31, -18, 42), (6, 2, 2), sd)
    return 40, 118


def build_room_workshop_heavy():
    rid = "room_workshop_heavy"
    sd, st, ru = M("steel_dark"), M("steel"), M("rust")
    # anvil is the hero, centered and enlarged (was read as a blank panel)
    box(f"{rid}|anvil_base", (8, -18, 7), (28, 14, 14), sd)
    box(f"{rid}|anvil_waist", (8, -18, 19), (13, 10, 10), sd)
    box(f"{rid}|anvil_body", (8, -18, 28), (36, 11, 8), st)
    cylinder(f"{rid}|anvil_horn", (31, -18, 28), 5.0, 16, ru, axis="X")
    box(f"{rid}|tongs", (-6, -13, 3), (22, 3, 2), ru)
    box(f"{rid}|forge_body", (-32, -18, 17), (24, 14, 34), M("wall_dark"))
    box(f"{rid}|forge_top", (-32, -18, 35), (26, 15, 3), sd)
    box(f"{rid}|forge_crack", (-32, -10, 18), (14, 1, 5),
        make_mat("ash_amber_emit", rgb("amber"), emission=rgb("amber"), emit_strength=16.0))
    cylinder(f"{rid}|chimney", (-38, -18, 54), 4.5, 38, sd)
    cylinder(f"{rid}|quench", (36, -18, 8), 6.5, 16, M("rust_light"), verts=20)
    return 30, 116


def build_room_workshop_precision():
    rid = "room_workshop_precision"
    cl, st, sd = M("clinical"), M("steel"), M("steel_dark")
    box(f"{rid}|bench_top", (0, -18, 34), (64, 9, 3), cl)
    for px in (-26, 26):
        box(f"{rid}|leg_{px}", (px, -18, 16), (3, 7, 32), st)
    box(f"{rid}|drawers", (-20, -18, 14), (18, 8, 26), sd)
    for d in range(3):
        box(f"{rid}|drawer_{d}", (-20, -13, 8 + d * 8), (16, 1, 6), st)
    cylinder(f"{rid}|lamp_arm", (22, -18, 46), 1.4, 22, st)
    cone(f"{rid}|lamp_head", (22, -18, 57), 7, 3, 8, sd)
    sphere(f"{rid}|lamp_bulb", (22, -18, 52), 3.0,
           make_mat("ash_lamp_bulb", rgb("lamp_glow"), emission=rgb("lamp_glow"), emit_strength=8.0))
    box(f"{rid}|tray", (2, -18, 37), (16, 8, 1.6), M("paper"))
    for i, dx in enumerate((-4, 0, 4)):
        box(f"{rid}|part_{i}", (dx, -18, 38.6), (2, 2, 2), st)
    return 36, 110


def cot(rid, cx, mat_frame=None, mat_sleep=None, w=46):
    st = mat_frame or M("steel")
    sl = mat_sleep or M("cloth")
    for px in (cx - w / 2 + 3, cx + w / 2 - 3):
        box(f"{rid}|leg_{px:+.0f}", (px, -18, 5), (3, 5, 10), st)
    box(f"{rid}|frame_{cx:+.0f}", (cx, -18, 12), (w, 6, 2.5), st)
    box(f"{rid}|pad_{cx:+.0f}", (cx, -18, 15), (w - 4, 5.5, 4), sl)


def build_room_clinic():
    rid = "room_clinic"
    cot(rid, -8)
    st = M("steel")
    cylinder(f"{rid}|iv_pole", (30, -18, 30), 2.0, 60, st)
    cylinder(f"{rid}|iv_cross", (30, -18, 58), 1.4, 16, st, axis="X")
    box(f"{rid}|iv_pouch", (24, -18, 50), (5, 3, 9), M("clinical"))
    cylinder(f"{rid}|iv_hook", (24, -18, 55), 0.6, 4, st, axis="X")
    wall_mark(f"{rid}|base", -4, 34, M("floor_dark"))
    return 34, 104


def build_room_ward_clinical():
    rid = "room_ward_clinical"
    cot(rid, -26, mat_sleep=M("clinical"), w=36)
    cot(rid, 12, mat_sleep=M("clinical"), w=36)
    fr = M("steel")
    # tall divider screen + IV stand give the ward a vertical read
    box(f"{rid}|screen_pole_a", (44, -16, 30), (3, 3, 60), fr)
    box(f"{rid}|screen_pole_b", (44, -34, 30), (3, 3, 60), fr)
    box(f"{rid}|screen_cloth", (44, -25, 32), (2, 22, 52), M("cloth_dark"))
    cylinder(f"{rid}|iv_pole", (34, -16, 28), 1.8, 56, fr)
    cylinder(f"{rid}|iv_cross", (34, -16, 55), 1.2, 14, fr, axis="X")
    box(f"{rid}|iv_pouch", (29, -16, 47), (5, 3, 9), M("clinical"))
    wall_mark(f"{rid}|base", -4, 52, M("floor_dark"))
    return 32, 118


def build_room_ward_quarantine():
    rid = "room_ward_quarantine"
    cot(rid, -22)
    cl_dark, hz = M("cloth_dark"), M("hazard")   # dark drape: palette-coherent
    mesh_from_corners(f"{rid}|tent_l", [(4, -30, 0), (24, -30, 0), (24, -30, 52), (4, -30, 78)], cl_dark)
    mesh_from_corners(f"{rid}|tent_r", [(44, -30, 0), (24, -30, 0), (24, -30, 52), (44, -30, 78)], cl_dark)
    box(f"{rid}|tent_seam", (24, -30, 60), (1.4, 1.4, 30), M("cloth"))
    wall_mark(f"{rid}|hazard", 14, 52, hz)
    return 32, 112


def build_room_kitchen():
    rid = "room_kitchen"
    sd, st, wd = M("steel_dark"), M("steel"), M("wood")
    box(f"{rid}|counter", (-12, -18, 16), (46, 12, 32), sd)
    box(f"{rid}|counter_top", (-12, -18, 33), (50, 14, 3), wd)
    box(f"{rid}|counter_door", (-12, -11, 16), (38, 1, 24), st)
    cylinder(f"{rid}|pot", (-22, -18, 40), 9, 12, st, verts=20)
    cylinder(f"{rid}|pot_lid", (-22, -18, 47), 9.4, 1.6, sd, verts=20)
    cylinder(f"{rid}|pot_knob", (-22, -18, 48.6), 1.8, 2, sd)
    box(f"{rid}|stove", (30, -18, 15), (26, 14, 30), M("wall_dark"))
    box(f"{rid}|stove_top", (30, -18, 31), (28, 15, 3), sd)
    cylinder(f"{rid}|burner", (30, -18, 33.4), 7, 1.2, M("rust"), verts=18)
    cylinder(f"{rid}|stove_pipe", (30, -18, 50), 3, 34, sd)
    return 32, 116


def build_room_storage_bay():
    rid = "room_storage_bay"
    st, wd, wd_l = M("steel"), M("wood"), M("wood_light")
    for px in (-32, 32):
        box(f"{rid}|post_{px}", (px, -18, 36), (3, 3, 72), st)
    for i, z in enumerate((14, 38, 62)):
        box(f"{rid}|shelf_{i}", (0, -18, z), (66, 8, 3), wd)
    for i, (sx, sz, s) in enumerate(((-24, 20, 12), (-2, 20, 10), (20, 20, 14),
                                     (-16, 44, 10), (12, 44, 12), (26, 68, 11))):
        box(f"{rid}|crate_{i}", (sx, -18, sz), (s, 7, s), wd_l if i % 2 else wd)
    return 38, 118


def build_room_storage_secure():
    rid = "room_storage_secure"
    st, sd = M("steel"), M("steel_dark")
    box(f"{rid}|locker", (0, -26, 22), (28, 10, 44), sd)
    box(f"{rid}|locker_handle", (0, -20, 26), (3, 2, 2), st)
    # tight cage: mid rail + more bars so the frame visually closes
    for px in (-36, 36):
        box(f"{rid}|post_{px}", (px, -18, 33), (4, 4, 66), st)
    for rz in (3, 33, 63):
        box(f"{rid}|rail_{rz}", (0, -18, rz), (76, 3, 3), st)
    for i, bx in enumerate((-12, 0, 12)):
        cylinder(f"{rid}|bar_{i}", (bx, -18, 33), 1.3, 66, st)
    torus(f"{rid}|lock_wheel", (-36, -13, 30), 5, 1.2, M("rust"))
    return 33, 114


def build_room_greenhouse_shelter():
    rid = "room_greenhouse_shelter"
    st, wd, lf = M("steel"), M("wood"), M("leaf")
    for px in (-32, 32):
        box(f"{rid}|post_{px}", (px, -18, 28), (3, 3, 56), st)
    for i, z in enumerate((16, 42)):
        box(f"{rid}|shelf_{i}", (0, -18, z), (66, 10, 3), wd)
        for t, tx in enumerate((-22, -8, 6, 20)):
            box(f"{rid}|tray_{i}_{t}", (tx, -18, z + 4), (11, 8, 5), lf)
    cylinder(f"{rid}|grow_tube", (0, -18, 56), 2.2, 64,
             make_mat("ash_grow_emit", rgb("lamp_glow"), emission=rgb("lamp_glow"), emit_strength=6.0), axis="X")
    return 34, 114


def build_room_radio_tuner():
    rid = "room_radio_tuner"
    wd, st, sd = M("wood"), M("steel"), M("steel_dark")
    box(f"{rid}|desk_top", (0, -18, 30), (60, 10, 3.5), wd)
    for px in (-25, 25):
        box(f"{rid}|leg_{px}", (px, -18, 14), (3, 8, 28), st)
    # hero chassis: big enough to anchor the icon at 64px (vision QA fix)
    box(f"{rid}|chassis", (-6, -18, 43), (40, 14, 24), sd)
    cylinder(f"{rid}|dial", (-16, -10, 43), 7, 2, M("paper"), axis="Y", verts=20)
    cylinder(f"{rid}|dial_pin", (-16, -9, 43), 1.2, 2.5, M("rust"), axis="Y")
    for k, kx in enumerate((2, 9)):
        cylinder(f"{rid}|knob_{k}", (kx, -10, 39), 2.5, 1.6, st, axis="Y")
    box(f"{rid}|speaker", (7, -10, 45), (14, 1, 14), M("wall_dark"))
    for s in range(3):
        box(f"{rid}|slat_{s}", (7, -9.4, 41 + s * 4.5), (12, 0.8, 1.4), st)
    ant = cylinder(f"{rid}|antenna", (22, -22, 50), 1.8, 44, st)
    ant.rotation_euler = (0, math.radians(18), 0)   # lean in the XZ screen plane
    box(f"{rid}|logbook", (24, -18, 32.6), (10, 7, 1.6), M("paper"))
    return 40, 112


def build_room_armory_munitions():
    rid = "room_armory_munitions"
    wd, wd_d, hz, st = M("wood"), M("wood_dark"), M("hazard"), M("steel")
    box(f"{rid}|crate_lo", (-16, -18, 11), (44, 14, 22), wd_d)
    box(f"{rid}|strap_lo", (-16, -18, 11), (45.6, 15, 4), hz)
    box(f"{rid}|crate_hi", (-18, -18, 29), (34, 12, 16), wd)
    box(f"{rid}|strap_hi", (-18, -18, 29), (35.4, 13, 3.4), hz)
    for px in (36, 50):
        box(f"{rid}|rack_post_{px}", (px, -18, 26), (3, 3, 52), st)
    box(f"{rid}|rack_top", (43, -18, 50), (18, 3, 3), st)
    box(f"{rid}|rack_bottom", (43, -18, 3), (18, 3, 3), st)
    for i, tx in enumerate((38, 43, 48)):
        box(f"{rid}|tube_{i}", (tx, -18, 27), (4, 4, 44), M("wall_dark"))
    return 28, 116


def build_room_laboratory_research():
    rid = "room_laboratory_research"
    cl, st, pp = M("clinical"), M("steel"), M("paper")
    box(f"{rid}|bench_top", (0, -18, 34), (66, 10, 3), cl)
    for px in (-27, 27):
        box(f"{rid}|leg_{px}", (px, -18, 16), (3, 8, 32), st)
    box(f"{rid}|cupboard", (0, -18, 14), (56, 8, 22), M("steel_dark"))
    for i, (fx, mat) in enumerate(((-18, pp), (-8, cl), (4, pp))):
        cone(f"{rid}|flask_{i}", (fx, -18, 39.5), 5, 1.2, 9, mat)
        cylinder(f"{rid}|flask_neck_{i}", (fx, -18, 45.5), 1.2, 4, mat)
    box(f"{rid}|rack", (-8, -18, 36.8), (26, 8, 1.4), st)
    box(f"{rid}|shelf", (26, -26, 52), (26, 3, 2), st)
    for i, sz in enumerate((46, 54)):
        box(f"{rid}|jar_{i}", (26, -26, sz), (14, 6, 5), pp if i else cl)
    box(f"{rid}|scope", (22, -18, 39), (6, 6, 8), st)
    cylinder(f"{rid}|scope_tube", (22, -18, 44), 1.6, 8, M("steel_dark"))
    return 34, 112


def build_room_common_mess_hall():
    rid = "room_common_mess_hall"
    wd, st = M("wood_light"), M("steel")
    box(f"{rid}|table_top", (0, -14, 30), (92, 12, 4), wd)
    for px in (-34, 34):
        box(f"{rid}|pedestal_{px}", (px, -14, 14), (5, 8, 28), st)
        box(f"{rid}|foot_{px}", (px, -14, 1.5), (14, 10, 3), st)
    # overhead pot rack: vertical mass so the silhouette reads above the
    # low table instead of smudging at icon size (vision QA 2026-09-26)
    box(f"{rid}|rack_bar", (0, -22, 52), (58, 3, 2.5), M("steel_dark"))
    for hx in (-40, 40):
        cylinder(f"{rid}|rack_stem_{hx}", (hx, -22, 44), 1.6, 18, M("steel_dark"))
    for i, hx in enumerate((-16, 0, 16)):
        cylinder(f"{rid}|hook_{i}", (hx, -22, 49), 0.9, 7, st)
        cylinder(f"{rid}|pot_{i}", (hx, -22, 43), 4.5, 8, st if i % 2 else M("rust_light"))
    box(f"{rid}|bench_back", (0, -34, 9), (76, 7, 3), M("wood"))
    for bx in (-28, 28):
        box(f"{rid}|bench_leg_b_{bx}", (bx, -34, 4), (3, 5, 8), st)
    box(f"{rid}|bench_front", (0, 2, 9), (76, 7, 3), M("wood"))
    for bx in (-28, 28):
        box(f"{rid}|bench_leg_f_{bx}", (bx, 2, 4), (3, 5, 8), st)
    box(f"{rid}|tray", (-12, -14, 33), (12, 8, 1.4), M("steel_dark"))
    cylinder(f"{rid}|mug", (12, -14, 35.5), 2.6, 4, M("paper"))
    return 30, 124


def build_room_reading_quiet_room():
    rid = "room_reading_quiet_room"
    wd, wd_l, cl = M("wood"), M("wood_light"), M("cloth")
    box(f"{rid}|shelf_sides", (-32, -24, 36), (2, 3, 72), wd)
    box(f"{rid}|shelf_side_r", (-10, -24, 36), (2, 3, 72), wd)
    for i, z in enumerate((12, 30, 48, 66)):
        box(f"{rid}|shelf_{i}", (-21, -24, z), (22, 4, 2), wd_l)
    book_mats = (M("paper"), M("rust_light"), M("cloth_dark"), M("hazard"))
    b = 0
    for z in (14, 32, 50):
        for sx in (-27, -20, -14):
            box(f"{rid}|book_{b}", (sx, -24, z + 5), (3.4, 5, 9), book_mats[b % 4])
            b += 1
    box(f"{rid}|chair_seat", (26, -18, 12), (24, 20, 6), cl)
    box(f"{rid}|chair_back", (36, -18, 27), (6, 20, 26), cl)
    for ax in (16, 36):
        box(f"{rid}|chair_arm_{ax}", (ax, -18, 17), (4, 20, 3), M("wood_dark"))
    cylinder(f"{rid}|lamp_pole", (2, -30, 26), 1.2, 52, M("steel"))
    cone(f"{rid}|lamp_shade", (2, -30, 54), 8, 3, 8, M("steel_dark"))
    sphere(f"{rid}|lamp_bulb", (2, -30, 50), 2.6,
           make_mat("ash_reading_bulb", rgb("lamp_glow"), emission=rgb("lamp_glow"), emit_strength=7.0))
    return 36, 122


def build_room_airlock():
    rid = "room_airlock"
    st, sd, hz = M("steel"), M("steel_dark"), M("hazard")
    for px in (-22, 22):
        box(f"{rid}|door_{px}", (px, -16, 30), (30, 5, 60), st)
        box(f"{rid}|door_frame_{px}", (px, -14, 30), (33, 2, 64), sd)
    box(f"{rid}|header", (0, -15, 62), (80, 4, 6), sd)
    torus(f"{rid}|wheel", (-22, -9, 30), 8, 1.6, M("rust"))
    cylinder(f"{rid}|wheel_hub", (-22, -9, 30), 1.8, 3, sd, axis="Y")
    for a in (0, 90):
        sp = cylinder(f"{rid}|spoke_{a}", (-22, -9, 30), 1.0, 16, M("rust"), axis="X")
        sp.rotation_euler = (0, math.radians(a), 0)   # sweep in the XZ screen plane
    wall_mark(f"{rid}|hazard", 0, 78, hz, z=1.4)
    sphere(f"{rid}|status_lamp", (0, -12, 58), 2.2,
           make_mat("ash_airlock_lamp", rgb("amber"), emission=rgb("amber"), emit_strength=10.0))
    return 32, 116


def build_room_generator():
    rid = "room_generator"
    sd, st, wd_d = M("steel_dark"), M("steel"), M("wood_dark")
    box(f"{rid}|skid", (0, -18, 2), (52, 16, 4), wd_d)
    box(f"{rid}|block", (-6, -18, 16), (32, 14, 24), st)
    cylinder(f"{rid}|exhaust", (14, -18, 34), 3.4, 40, sd)
    cylinder(f"{rid}|muffler", (14, -18, 56), 5.0, 14, sd, axis="X")
    box(f"{rid}|panel", (-30, -18, 20), (10, 4, 22), sd)
    for i, pz in enumerate((24, 30)):
        sphere(f"{rid}|lamp_{i}", (-30, -15, pz), 1.6,
               make_mat(f"ash_gen_lamp_{i}", rgb("lamp_glow") if i else rgb("amber"),
                        emission=rgb("lamp_glow") if i else rgb("amber"), emit_strength=6.0))
    cylinder(f"{rid}|fan", (-6, -10, 16), 7, 1.4, sd, axis="Y", verts=18)
    box(f"{rid}|battery", (-30, -18, 6), (12, 9, 8), M("rust"))
    return 28, 112


def build_room_filtration():
    rid = "room_filtration"
    st, sd, ru = M("steel"), M("steel_dark"), M("rust")
    for i, fx in enumerate((-24, 0, 24)):
        cylinder(f"{rid}|filter_{i}", (fx, -20, 24), 9, 48, st, verts=20)
        cylinder(f"{rid}|band_lo_{i}", (fx, -20, 8), 9.4, 4, ru, verts=20)
        cylinder(f"{rid}|band_hi_{i}", (fx, -20, 42), 9.4, 4, M("rust_light"), verts=20)
        cylinder(f"{rid}|cap_{i}", (fx, -20, 49), 6, 3, sd, verts=18)
    cylinder(f"{rid}|manifold", (0, -20, 56), 3.0, 60, sd, axis="X")
    cylinder(f"{rid}|gauge", (-34, -14, 50), 5, 2, M("paper"), axis="Y", verts=18)
    cylinder(f"{rid}|gauge_stem", (-34, -17, 50), 1.4, 4, sd, axis="Y")
    for px in (-40, 40):
        cylinder(f"{rid}|drop_{px}", (px, -20, 32), 2.0, 56, ru)
    return 30, 118


def build_room_hope_beacon():
    rid = "room_hope_beacon"
    st, sd = M("steel"), M("steel_dark")
    box(f"{rid}|base", (0, -18, 3), (26, 18, 6), sd)
    cylinder(f"{rid}|mast", (0, -18, 42), 3.2, 78, st)
    for cz in (52, 66):
        cylinder(f"{rid}|cross_{cz}", (0, -18, cz), 2.0, 26, sd, axis="X")
    cone(f"{rid}|housing", (0, -18, 84), 8, 4, 10, sd)
    sphere(f"{rid}|lamp", (0, -18, 78), 5.5,
           make_mat("ash_beacon_lamp", rgb("amber"), emission=rgb("amber"), emit_strength=30.0))
    box(f"{rid}|guy_a", (-20, -18, 1.5), (2, 2, 30), sd)
    box(f"{rid}|guy_b", (20, -18, 1.5), (2, 2, 30), sd)
    return 42, 118


# ── the two remaining props (wall/ceiling-mounted: no floor patch) ─────────
def build_prop_ceiling_lamp():
    rid = "prop_ceiling_lamp"
    st, sd = M("steel"), M("steel_dark")
    cylinder(f"{rid}|stem", (0, 0, 44), 1.6, 44, st)
    cone(f"{rid}|canopy", (0, 0, 66), 5, 1.6, 5, sd)
    cone(f"{rid}|shade", (0, 0, 30), 13, 4.5, 16, st)
    cylinder(f"{rid}|rim", (0, 0, 23), 12.4, 2.0, sd, verts=24)
    sphere(f"{rid}|bulb", (0, 0, 21), 5.0,
           make_mat("ash_clamp_bulb", rgb("lamp_glow"), emission=rgb("lamp_glow"), emit_strength=16.0))
    return 44, 92


def build_prop_pipe_bundle():
    rid = "prop_pipe_bundle"
    st, sd, ru = M("steel"), M("steel_dark"), M("rust")
    cylinder(f"{rid}|pipe_rust", (-13, 0, 36), 5.0, 72, ru)
    cylinder(f"{rid}|pipe_steel", (1, 0, 36), 6.0, 72, st)
    cylinder(f"{rid}|pipe_dark", (14, 0, 36), 3.8, 72, sd)
    cylinder(f"{rid}|flange_lo", (1, 0, 2), 7.2, 3, M("rust_light"), verts=20)
    cylinder(f"{rid}|flange_hi", (1, 0, 70), 7.2, 3, M("rust_light"), verts=20)
    for sz in (18, 48):
        box(f"{rid}|strap_{sz}", (0, -7, sz), (38, 3, 6), sd)
        for bx in (-17, 17):
            cylinder(f"{rid}|bolt_{sz}_{bx}", (bx, -8, sz), 1.0, 2, M("rust"), axis="Y")
    return 38, 100


# id -> (builder, grounded) ; builder returns (camera_center_z, span)
ROOMS = {
    "room_airlock": build_room_airlock,
    "room_armory_munitions": build_room_armory_munitions,
    "room_bunker_corridor": build_room_bunker_corridor,
    "room_bunks": build_room_bunks,
    "room_bunks_crowded": build_room_bunks_crowded,
    "room_clinic": build_room_clinic,
    "room_common_mess_hall": build_room_common_mess_hall,
    "room_quarters_private": build_room_quarters_private,
    "room_filtration": build_room_filtration,
    "room_generator": build_room_generator,
    "room_greenhouse_shelter": build_room_greenhouse_shelter,
    "room_hope_beacon": build_room_hope_beacon,
    "room_kitchen": build_room_kitchen,
    "room_laboratory_research": build_room_laboratory_research,
    "room_radio_tuner": build_room_radio_tuner,
    "room_reading_quiet_room": build_room_reading_quiet_room,
    "room_storage_bay": build_room_storage_bay,
    "room_storage_secure": build_room_storage_secure,
    "room_ward_clinical": build_room_ward_clinical,
    "room_ward_quarantine": build_room_ward_quarantine,
    "room_workshop": build_room_workshop,
    "room_workshop_heavy": build_room_workshop_heavy,
    "room_workshop_precision": build_room_workshop_precision,
}

# wall/ceiling-mounted: no contact shadow floor in frame
UNGROUNDED = {"prop_ceiling_lamp", "prop_pipe_bundle", "room_hope_beacon"}

PROPS_EXTRA = {
    "prop_ceiling_lamp": build_prop_ceiling_lamp,
    "prop_pipe_bundle": build_prop_pipe_bundle,
}

ENTRIES = {**ROOMS, **PROPS_EXTRA}


# ── render setup (mirrors bake-shelter-stage.py) ───────────────────────────
def new_scene():
    bpy.ops.wm.read_factory_settings(use_empty=True)
    scene = CTX.scene
    scene.name = "RoomBake"
    world = bpy.data.worlds.new("StageWorld")
    world.use_nodes = True
    scene.world = world


def add_camera():
    cam_data = bpy.data.cameras.new("StageCam")
    cam_data.type = "ORTHO"
    cam = bpy.data.objects.new("StageCam", cam_data)
    # front view: look along +Y (the stage convention) — without this the
    # camera points down -Z at empty space and every render comes back blank
    cam.rotation_euler = (math.radians(90), 0, 0)
    link(cam)
    CTX.scene.camera = cam
    return cam


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


def setup_render():
    scene = CTX.scene
    scene.render.engine = "CYCLES"
    scene.cycles.device = "CPU"
    scene.cycles.samples = 96
    scene.cycles.seed = 20260925
    scene.cycles.use_denoising = True
    scene.render.resolution_x = 256
    scene.render.resolution_y = 256
    scene.render.film_transparent = True
    scene.render.image_settings.file_format = "PNG"
    scene.render.image_settings.color_mode = "RGBA"
    scene.view_settings.view_transform = "Standard"
    scene.view_settings.look = "None"


def main():
    args = sys.argv[sys.argv.index("--") + 1:] if "--" in sys.argv else []
    out_dir = "artifacts/shelter-bake-rooms/raw"
    only = None
    it = iter(args)
    for a in it:
        if a == "--out":
            out_dir = next(it)
        elif a == "--only":
            only = {v for v in next(it).split(",") if v in ENTRIES}

    import os
    os.makedirs(out_dir, exist_ok=True)

    new_scene()
    add_camera()
    specs = {}
    for rid, builder in ENTRIES.items():
        cam_z, span = builder()
        specs[rid] = (cam_z, span)
    common_floor()

    setup_render()
    # neutral day-authoritative rig, identical to the shipped prop bake
    area_light("rig_key", (-160, -140, 220), 180, 110, (0.80, 0.83, 0.87),
               (math.radians(50), math.radians(18), 0))
    area_light("rig_fill", (220, -60, 160), 140, 30, (0.60, 0.66, 0.72),
               (math.radians(80), 0, 0))
    world = bpy.data.worlds["StageWorld"]
    world.node_tree.nodes["Background"].inputs[0].default_value = (0.05, 0.05, 0.06, 1.0)
    world.node_tree.nodes["Background"].inputs[1].default_value = 0.4

    cam = bpy.data.objects["StageCam"]
    for rid in ENTRIES:
        if only and rid not in only:
            continue
        cam_z, span = specs[rid]
        grounded = rid not in UNGROUNDED
        for ob in CTX.scene.collection.objects:
            if ob.name in ("StageCam", "rig_key", "rig_fill"):
                ob.hide_render = False
            elif ob.name == "common_floor":
                ob.hide_render = not grounded
            else:
                ob.hide_render = not ob.name.startswith(rid + "|")
        cam.location = sc3((0, -300, cam_z))
        cam.data.ortho_scale = sc(span)
        CTX.scene.render.filepath = f"{out_dir}/{rid}_raw.png"
        bpy.ops.render.render(write_still=True)
        print(f"[bake] {rid} span={span} cam_z={cam_z} -> {CTX.scene.render.filepath}")
    print("[bake] done")


main()
