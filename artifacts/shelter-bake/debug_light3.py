import bpy, math, bmesh
bpy.ops.wm.read_factory_settings(use_empty=True)
sc = bpy.context.scene
col = sc.collection
m = bpy.data.meshes.new("p"); ob = bpy.data.objects.new("p", m); col.objects.link(ob)
bm = bmesh.new()
vs = [bm.verts.new(c) for c in [(-3.8,0,2.1),(3.8,0,2.1),(3.8,0,-2.1),(-3.8,0,-2.1)]]
bm.faces.new(vs); bm.to_mesh(m); bm.free()
mat = bpy.data.materials.new("t"); mat.use_nodes = True
bsdf = mat.node_tree.nodes.get("Principled BSDF")
bsdf.inputs["Base Color"].default_value = (0.5, 0.5, 0.5, 1.0)
ob.data.materials.append(mat)
w = bpy.data.worlds.new("StageWorld"); w.use_nodes = True
w.node_tree.nodes["Background"].inputs[0].default_value = (0.42,0.44,0.47,1)
w.node_tree.nodes["Background"].inputs[1].default_value = 0.55
sc.world = w
ld = bpy.data.lights.new("L","POINT"); ld.energy = 70.0; ld.shadow_soft_size = 0.03
lo = bpy.data.objects.new("L", ld); lo.location=(0,-3,1.5); col.objects.link(lo)
cd = bpy.data.cameras.new("C"); cd.type="ORTHO"; cd.ortho_scale=7.6
cam = bpy.data.objects.new("C",cd); cam.location=(0,-6,0); cam.rotation_euler=(math.radians(90),0,0); col.objects.link(cam)
sc.camera = cam
# exact setup_render() settings from the bake script
sc.render.engine="CYCLES"; sc.cycles.device="CPU"
sc.cycles.samples=96; sc.cycles.seed=20260925; sc.cycles.use_denoising=True
sc.render.film_transparent=False
sc.render.image_settings.file_format="PNG"; sc.render.image_settings.color_mode="RGBA"
sc.view_settings.view_transform="Standard"; sc.view_settings.look="None"
sc.render.resolution_x=380; sc.render.resolution_y=210
sc.render.filepath="artifacts/shelter-bake/debug_light3.png"
bpy.ops.render.render(write_still=True)
print("debug3 done")
