#!/usr/bin/env python3
# SPDX-License-Identifier: MIT
"""ASHFALL — Surface/exterior backdrop 2D bake (Blender headless).

Replaces the Pillow placeholder pack (generate-surface-placeholders.py) with
real 3D-baked final art at the same filenames / sizes the runtime consumes:

  assets/sprites/Surface/wasteland_sky_{day1_7,dawn,dusk,night}.png            1920x1080
  assets/sprites/Surface/surface_hatch_approach_{day1_7,dawn,dusk,night}.png  1280x720
  assets/sprites/Surface/expedition_departure_{day1_7,dawn,dusk,night}.png    1280x720

Mirrors the proven bake-shelter-stage.py pipeline: deterministic Cycles render
(fixed seed 20260925), perspective camera for landscape depth, a gradient sky
backdrop + emissive sun/moon + a SUN key light per phase so the ground and
structures get real light/shadow. Intact world (days 1-7): no craters, no
mushroom clouds. Output is downsampled to the final size by the caller with
ImageMagick (post-surface-bake.py), not in Blender.

Usage (from repo root):
  # 1. Work around the broken Blender OCIO (config 2.5 vs lib 2.4.2):
  export OCIO="$(bash scripts/tools/blender-ocio-fix.sh)"
  # 2. Bake:
  blender --background --python scripts/tools/bake-surface.py -- \\
      --out artifacts/surface-bake/raw [--scenes sky,hatch,departure] [--phases all]
  python3 scripts/tools/post-surface-bake.py artifacts/surface-bake/raw
"""

import math
import os
import sys

import bmesh
import bpy

CTX = bpy.context

# Scene pixel targets (final). Render at 2x then downscale in post.
SIZES = {
    "sky": (1920, 1080),
    "hatch": (1280, 720),
    "departure": (1280, 720),
}
RENDER_SCALE = 1
S = 0.01  # pixel -> metre


def sc(v):
    return v * S


def sc3(t):
    return (t[0] * S, t[1] * S, t[2] * S)


def link(ob):
    CTX.scene.collection.objects.link(ob)
    return ob


def rgb(h):
    return tuple(((h >> shift) & 0xFF) / 255.0 for shift in (16, 8, 0))


# ── palette (DESIGN.md: charcoal / green-white / rust) ────────────────────
PAL = {
    "ground": rgb(0x201D18),
    "ground_dark": rgb(0x2A2722),
    "ridge_far": rgb(0x6C7278),
    "ridge_mid": rgb(0x4E5256),
    "ridge_near": rgb(0x36383A),
    "steel": rgb(0x454B54),
    "steel_dark": rgb(0x343940),
    "rust": rgb(0x6E3F28),
    "concrete": rgb(0x55524B),
    "structure": rgb(0x3C4046),
}


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


def _resolve_mat(name, m):
    return m if not isinstance(m, tuple) else make_mat("auto_" + name, m)


def box(name, center, size, mat):
    mat = _resolve_mat(name, mat)
    cx, cy, cz = sc3(center)
    sx, sy, sz = (s * S / 2.0 for s in size)
    corners = [
        (cx - sx, cy - sy, cz - sz), (cx + sx, cy - sy, cz - sz),
        (cx + sx, cy + sy, cz - sz), (cx - sx, cy + sy, cz - sz),
        (cx - sx, cy - sy, cz + sz), (cx + sx, cy - sy, cz + sz),
        (cx + sx, cy + sy, cz + sz), (cx - sx, cy + sy, cz + sz),
    ]
    faces_idx = [(0, 1, 2, 3), (4, 7, 6, 5), (0, 4, 5, 1),
                 (2, 6, 7, 3), (1, 5, 6, 2), (0, 3, 7, 4)]
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


def cylinder(name, loc, radius, depth, mat, verts=32):
    mat = _resolve_mat(name, mat)
    bpy.ops.mesh.primitive_cylinder_add(vertices=verts, radius=sc(radius),
                                        depth=sc(depth), location=sc3(loc),
                                        end_fill_type="NGON")
    ob = bpy.context.active_object
    ob.name = name
    ob.data.materials.append(mat)
    return ob


def sphere(name, loc, radius, mat):
    mat = _resolve_mat(name, mat)
    bpy.ops.mesh.primitive_uv_sphere_add(radius=sc(radius), location=sc3(loc),
                                         segments=24, ring_count=16)
    ob = bpy.context.active_object
    ob.name = name
    ob.data.materials.append(mat)
    return ob


# ── gradient sky backdrop + emissive sun/moon ────────────────────────────
def sky_backdrop(w, h, top, horizon, depth=1200.0):
    """Large vertical-gradient plane behind the scene (camera looks +Y)."""
    mat = bpy.data.materials.new("SkyGradient")
    mat.use_nodes = True
    nt = mat.node_tree
    for n in list(nt.nodes):
        nt.nodes.remove(n)
    out = nt.nodes.new("ShaderNodeOutputMaterial")
    emit = nt.nodes.new("ShaderNodeEmission")
    ramp = nt.nodes.new("ShaderNodeValToRGB")
    tex = nt.nodes.new("ShaderNodeTexCoord")
    sep = nt.nodes.new("ShaderNodeSeparateXYZ")
    nt.links.new(tex.outputs["Generated"], sep.inputs["Vector"])
    nt.links.new(sep.outputs["Z"], ramp.inputs["Fac"])
    nt.links.new(ramp.outputs["Color"], emit.inputs["Color"])
    nt.links.new(emit.outputs["Emission"], out.inputs["Surface"])
    ramp.color_ramp.elements[0].position = 0.0
    ramp.color_ramp.elements[0].color = (*horizon, 1.0)
    ramp.color_ramp.elements[1].position = 1.0
    ramp.color_ramp.elements[1].color = (*top, 1.0)
    emit.inputs["Strength"].default_value = 1.0

    # tall plane at far +Y, its local Y runs bottom->top (Generated Y = 0..1)
    me = bpy.data.meshes.new("SkyPlane")
    ob = bpy.data.objects.new("SkyPlane", me)
    link(ob)
    bm = bmesh.new()
    hw = sc(w * 1.6)
    y = sc(depth)
    for x, z in [(-hw, 0), (hw, 0), (hw, sc(h * 1.6)), (-hw, sc(h * 1.6))]:
        bm.verts.new((x, y, z))
    bm.faces.new(bm.verts)
    bm.normal_update()
    bm.to_mesh(me)
    bm.free()
    ob.data.materials.append(mat)
    return ob


def sun_disc(loc, radius, color, strength=18.0):
    mat = make_mat(f"Sun{int(radius)}", color, emission=color, emit_strength=strength)
    return sphere("SunMoon", loc, radius, mat)


# ── phases: sun position + sky colours + key light ────────────────────────
# (sky_top, sky_horizon, sun_colour, sun_elev_deg, sun_x, key_energy, ambient)
PHASES = {
    "day1_7": dict(top=rgb(0x36404E), hor=rgb(0x9C9C94), sun=rgb(0xD6CEB4),
                   elev=58, sx=0.62, key=3.2, amb=rgb(0x3A3E44), ambstr=0.5, exposure=0.0),
    "dawn": dict(top=rgb(0x2E3A54), hor=rgb(0xC98A55), sun=rgb(0xE8A867),
                 elev=12, sx=0.18, key=2.2, amb=rgb(0x3A3340), ambstr=0.42, exposure=0.2),
    "dusk": dict(top=rgb(0x2A2C42), hor=rgb(0xB5663C), sun=rgb(0xD9784A),
                 elev=8, sx=0.82, key=2.0, amb=rgb(0x3A2E33), ambstr=0.4, exposure=0.3),
    "night": dict(top=rgb(0x121724), hor=rgb(0x27303F), sun=rgb(0xC7D2DE),
                  elev=-8, sx=0.5, key=5.5, amb=rgb(0x2A3550), ambstr=0.9, exposure=2.0),
}


def apply_phase(name, w, h):
    cfg = PHASES[name]
    sx = cfg["sx"]
    elev = cfg["elev"]
    world = bpy.data.worlds["SkyWorld"]
    nt = world.node_tree
    bg = nt.nodes["Background"]
    bg.inputs["Strength"].default_value = cfg["ambstr"] * 0.7
    sky = next((n for n in nt.nodes if n.bl_idname == "ShaderNodeTexSky"), None)
    if sky:
        try:
            sky.sun_elevation = math.radians(max(elev, -6))
            sky.sun_rotation = math.radians((sx - 0.5) * 120)
            sky.sun_intensity = 0.5 if name != "night" else 0.1
        except Exception:
            pass
    # key SUN light (angle from elevation) — lights ground + structures
    sun = bpy.data.objects["KeySun"]
    sun.rotation_euler = (math.radians(90 - elev), 0, math.radians((sx - 0.5) * 60))
    sun.data.energy = cfg["key"]
    sun.data.color = cfg["sun"]
    sun.data.angle = math.radians(2.0 if name != "night" else 6.0)
    # per-phase exposure lift (night/moonlit needs a big boost)
    CTX.scene.view_settings.exposure = cfg.get("exposure", 0.0)


# ── scene geometry ───────────────────────────────────────────────────────
def base_scene(w, h):
    bpy.ops.wm.read_factory_settings(use_empty=True)
    scene = CTX.scene
    scene.name = "SurfaceBake"
    world = bpy.data.worlds.new("SkyWorld")
    world.use_nodes = True
    nt = world.node_tree
    bg = nt.nodes["Background"]
    sky = nt.nodes.new("ShaderNodeTexSky")
    try:
        sky.sky_type = "NISHITA"
        sky.sun_disc = True
        sky.sun_size = math.radians(1.2)
    except Exception:
        pass
    nt.links.new(sky.outputs["Color"], bg.inputs["Color"])
    scene.world = world

    # level camera -> horizon at frame centre (sky above, ground below)
    cam_data = bpy.data.cameras.new("Cam")
    cam_data.lens = 40
    cam = bpy.data.objects.new("Cam", cam_data)
    cam.location = sc3((w * 0.5, -6000, 900))
    cam.rotation_euler = (math.radians(85), 0, 0)
    link(cam)
    scene.camera = cam

    # key sun (directional light + shadows on ground/structures)
    sd = bpy.data.lights.new("KeySun", "SUN")
    so = bpy.data.objects.new("KeySun", sd)
    so.rotation_euler = (math.radians(50), 0, 0)
    link(so)

    # ground plane
    gm = make_mat("Ground", PAL["ground"], rough=1.0)
    bpy.ops.mesh.primitive_plane_add(size=sc(w * 40), location=sc3((w * 0.5, 8000, 0)))
    gp = bpy.context.active_object
    gp.name = "Ground"
    gp.data.materials.append(gm)


def ridgeline(width, base_y, height, colour, y_depth, seed):
    rnd = _Rnd(seed)
    me = bpy.data.meshes.new(f"Ridge{seed}")
    ob = bpy.data.objects.new(f"Ridge{seed}", me)
    link(ob)
    bm = bmesh.new()
    pts = []
    x = -width * 0.6
    while x < width * 1.6:
        pts.append((x, base_y + rnd.rand() * height))
        x += width * (0.05 + rnd.rand() * 0.08)
    for (px, pz) in pts:
        bm.verts.new((sc(px), sc(y_depth), sc(pz)))
    for (px, pz) in reversed(pts):
        bm.verts.new((sc(px), sc(y_depth), sc(-50)))
    bm.faces.new(bm.verts)
    bm.normal_update()
    bm.to_mesh(me)
    bm.free()
    ob.data.materials.append(make_mat(f"Ridge{seed}", colour, rough=1.0))
    return ob


class _Rnd:
    def __init__(self, seed):
        self.s = seed & 0xFFFFFFFF

    def rand(self):
        self.s = (1103515245 * self.s + 12345) & 0x7FFFFFFF
        return self.s / 0x7FFFFFFF


def build_sky(w, h):
    base_scene(w, h)
    # far ridges sit ON the ground (base z=0) at far y so they silhouette on the horizon
    ridgeline(w * 2, 0, 1600, PAL["ridge_far"], 16000, 11)
    ridgeline(w * 2, 0, 1150, PAL["ridge_mid"], 11000, 22)
    ridgeline(w * 2, 0, 780, PAL["ridge_near"], 7000, 33)
    # distant intact structures (silhouettes on the horizon)
    box("Tower", (w * 0.68, 5200, 750), (220, 220, 1500), PAL["structure"])
    box("TowerHead", (w * 0.68, 5200, 1560), (360, 360, 240), PAL["structure"])
    cylinder("WaterTower", (w * 0.30, 4200, 620), 320, 640, PAL["structure"])
    for lx in (-210, 210):
        box(f"Leg{lx}", (w * 0.30 + lx, 4200, 180), (70, 70, 720), PAL["steel_dark"])


def build_hatch(w, h):
    base_scene(w, h)
    ridgeline(w * 2, 0, 1400, PAL["ridge_far"], 16000, 11)
    ridgeline(w * 2, 0, 980, PAL["ridge_mid"], 11000, 22)
    # circular hatch on the near ground (z=0 base, framed mid-ground)
    cylinder("HatchPad", (w * 0.5, 2500, 30), 760, 60, PAL["concrete"])
    cylinder("HatchRing", (w * 0.5, 2500, 95), 600, 70, PAL["steel"])
    cylinder("HatchLid", (w * 0.5, 2500, 165), 500, 60, PAL["steel_dark"])
    cylinder("HatchWheel", (w * 0.5, 2500, 235), 150, 50, PAL["rust"])
    for a in range(0, 360, 45):
        box(f"Spoke{a}", (w * 0.5 + 260 * math.cos(math.radians(a)),
                          2500 + 260 * math.sin(math.radians(a)), 240),
            (320, 26, 22), PAL["rust"])
    # approach path + bollards
    box("Path", (w * 0.5, 1700, 12), (1500, 2200, 24), PAL["ground_dark"])
    for bx in (w * 0.5 - 820, w * 0.5 + 820):
        cylinder(f"Bollard{bx}", (bx, 1500, 220), 55, 440, PAL["steel_dark"], verts=12)


def build_departure(w, h):
    base_scene(w, h)
    ridgeline(w * 2, 0, 1500, PAL["ridge_far"], 17000, 11)
    # departure threshold: concrete gate frame + ramp + tracks + crates (z=0 base)
    box("GateL", (w * 0.30, 3200, 950), (140, 140, 1900), PAL["concrete"])
    box("GateR", (w * 0.70, 3200, 950), (140, 140, 1900), PAL["concrete"])
    box("GateTop", (w * 0.5, 3200, 1830), (1900, 140, 170), PAL["concrete"])
    box("Ramp", (w * 0.5, 2100, 22), (1500, 2100, 44), PAL["ground_dark"])
    for ty in (1500, 2100, 2700):
        box(f"TrackL{ty}", (w * 0.5 - 420, ty, 55), (60, 320, 40), PAL["steel_dark"])
        box(f"TrackR{ty}", (w * 0.5 + 420, ty, 55), (60, 320, 40), PAL["steel_dark"])
    box("Crate1", (w * 0.22, 2600, 250), (420, 420, 500), PAL["rust"])
    box("Crate2", (w * 0.26, 2050, 190), (360, 360, 380), PAL["steel"])
    cylinder("Barrel", (w * 0.78, 2500, 300), 210, 600, PAL["rust"])


BUILDERS = {"sky": build_sky, "hatch": build_hatch, "departure": build_departure}


def setup_render(w, h):
    scene = CTX.scene
    scene.render.engine = "CYCLES"
    scene.cycles.device = "CPU"
    scene.cycles.samples = 28
    scene.cycles.seed = 20260925
    scene.cycles.use_denoising = True
    scene.render.resolution_x = w * RENDER_SCALE
    scene.render.resolution_y = h * RENDER_SCALE
    scene.render.film_transparent = False
    scene.render.image_settings.file_format = "PNG"
    scene.render.image_settings.color_mode = "RGB"
    scene.view_settings.view_transform = "AgX"
    scene.view_settings.look = "None"
    scene.view_settings.exposure = 0.0


def parse_args():
    out = "artifacts/surface-bake/raw"
    scenes = list(BUILDERS.keys())
    phases = list(PHASES.keys())
    argv = sys.argv[sys.argv.index("--") + 1:] if "--" in sys.argv else []
    i = 0
    while i < len(argv):
        if argv[i] == "--out":
            out = argv[i + 1]; i += 2
        elif argv[i] == "--scenes":
            scenes = argv[i + 1].split(","); i += 2
        elif argv[i] == "--phases":
            phases = PHASES.keys() if argv[i + 1] == "all" else argv[i + 1].split(","); i += 2
        else:
            i += 1
    return out, scenes, phases


def main():
    out, scenes, phases = parse_args()
    os.makedirs(out, exist_ok=True)
    for scene_key in scenes:
        w, h = SIZES[scene_key]
        for phase in phases:
            BUILDERS[scene_key](w, h)
            setup_render(w, h)
            apply_phase(phase, w, h)
            path = os.path.join(out, f"{scene_name(scene_key)}_{phase}.png")
            CTX.scene.render.filepath = path
            bpy.ops.render.render(write_still=True)
            print(f"[surface-bake] rendered {path}")


def scene_name(key):
    return {"sky": "wasteland_sky",
            "hatch": "surface_hatch_approach",
            "departure": "expedition_departure"}[key]


if __name__ == "__main__":
    main()
