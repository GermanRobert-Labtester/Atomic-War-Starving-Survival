# CONSISTENCY_ANCHORS

## A01 — Inventory icon family (locked)

- **Seed reference:** `generated_AIassets/ammo_545x39_jhp_ap.png` controls black field, object scale, top-left light and material wear only. It does not control another object's geometry.
- **Frame:** 1:1; object occupies 72–82% of width and height; 8% minimum edge clearance; no cast-off props unless quantity defines the item.
- **Camera:** centered three-quarter view. Use exact side profile for firearms and long attachments; shallow top-down only for papers, kits and trays.
- **Background:** opaque flat black, no horizon, floor plane, vignette, texture or drop shadow.
- **Rendering:** dry-gouache material blocks, restrained ink edge, no soft photographic bokeh.
- **Light:** top-left rim/key; weak warm bounce; contact darkening stays inside the silhouette.
- **Wear:** one functional wear pattern: grip polish, edge chips, ash in seams, mineral scale, soot or oxidation.
- **Text:** no readable labels, brands, serials, flags or insignia. Labels may be blank color fields.
- **Output path:** raw model output to `generated_AIassets/<item_id>.png`; human-painted result to `Assets/Resources/Art/Items/<item_id>.png`.

## A02 — Quantity and fill-state families (locked)

- Reuse identical container geometry, camera, label block, cap and lighting across a family.
- Change only fill height, count, damage or listed tier components.
- Show fill states exactly: 0.5/1 L = half; 0.5/2 L = quarter; 1/2 L = half; 1.5/2 L = three quarters.
- Counted items show the stated count when practical; dense tablets use a visibly full/half/empty bottle plus the manifest count.
- Generate the anchor before any derivative that names it as a reference.

## A03 — Medical family (locked)

- Dull cream, amber glass, off-white gauze, brushed steel and one muted blue indicator patch.
- Clinical wear, not filth: creased wrappers, chipped caps, sterilization marks. No blood, red-cross emblem or readable pharmaceutical branding.

## A04 — Radiation and filtration family (locked)

- Instruments use olive/charcoal cases, small blank gauges or displays, recessed controls and repair tape.
- Filters expose pleats, ceramic, charcoal media or gaskets; contamination appears as dust/mineral staining, never green fantasy glow.
- Protective equipment remains empty and unmodeled; lenses are scratched or lightly fogged, not opaque.

## A05 — Tool and shelter-device family (locked)

- Tools show the working end and grip. Devices show input, control and output components without cutaway diagrams.
- Basic tier: complete civilian construction. Improvised tier: welded scrap and mismatched fasteners. Advanced tier: added guards, gauges and reinforcement while preserving the anchor footprint.

## A06 — Existing approved bases

- The 30 high-resolution ammunition PNGs in `generated_AIassets/` are production bases; preserve them and perform the required human paintover/import pass.
- `generated_AIassets/crowbar_item.jpg` is the `crowbar` base; preserve its silhouette and relink after human paintover.
- `geiger_counter_icon.jpg`, `iodine_pills_box.jpg` and `hazmat_suit_prop.jpg` control object identity only; Nano Banana Pro removes baked text/background and normalizes the family treatment.

## Deferred anchors

The main-menu reference remains the environment style seed. Character, location, faction and weather anchors are intentionally not locked until runtime image destinations exist and one human-approved production asset is available.
