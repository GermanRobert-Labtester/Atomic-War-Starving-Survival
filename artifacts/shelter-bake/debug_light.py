import bpy, math
bpy.ops.wm.read_factory_settings(use_empty=True)
sc = bpy.context.scene
col = sc.collection
# 7.6m wide plane facing camera at z=0
m = bpy.data.meshes.new("p"); ob = bpy.data.objects.new("p", m); col.objects.link(ob)
import bmesh
bm = bmesh.new()
vs = [bm.verts.new(c) for c in [(-3.8,0,2.1),(3.8,0,2.1),(3.8,0,-2.1),(-3.8,0,-2.1)]]
bm.faces.new(vs); bm.to_mesh(m); bm.free()
mat = bpy.data.materials.new("t"); mat.use_nodes = True
bsdf = mat.node_tree.nodes.get("Principled BSDF")
bsdf.inputs["Base Color"].default_value = (0.5, 0.5, 0.5, 1.0)
ob.data.materials.append(mat)
w = bpy.data.worlds.new("W"); w.use_nodes = True
w.node_tree.nodes["Background"].inputs[0].default_value = (0,0,0,1)
w.node_tree.nodes["Background"].inputs[1].default_value = 0.0
sc.world = w
ld = bpy.data.lights.new("L","POINT"); ld.energy = 1000.0
lo = bpy.data.objects.new("L", ld); lo.location=(0,-3,1.5); col.objects.link(lo)
cd = bpy.data.cameras.new("C"); cd.type="ORTHO"; cd.ortho_scale=7.6
cam = bpy.data.objects.new("C",cd); cam.location=(0,-6,0); cam.rotation_euler=(math.radians(90),0,0); col.objects.link(cam)
sc.camera = cam
sc.render.engine="CYCLES"; sc.cycles.samples=16; sc.cycles.device="CPU"
sc.render.resolution_x=380; sc.render.resolution_y=210
sc.render.filepath="artifacts/shelter-bake/debug_light.png"
sc.view_settings.view_transform="Standard"
bpy.ops.render.render(write_still=True)
print("debug done")
