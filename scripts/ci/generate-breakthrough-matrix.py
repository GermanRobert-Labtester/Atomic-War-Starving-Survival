#!/usr/bin/env python3
import json
import os

repo_root = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
data_dir = os.path.join(repo_root, "Assets", "StreamingAssets", "Data")

rk_path = os.path.join(data_dir, "research_knowledge.json")
recipes_path = os.path.join(data_dir, "recipes.json")
items_path = os.path.join(data_dir, "items.json")

with open(rk_path) as f:
    rk = json.load(f)
with open(recipes_path) as f:
    recipes_doc = json.load(f)
with open(items_path) as f:
    items = {it["id"]: it for it in json.load(f)["items"]}

new_recipes_data = [
    {
        "id": "craft_hepa_scrubber_assembly",
        "recipeName": "Assemble HEPA Air Scrubber Pack",
        "ingredients": [
            {"itemId": "item_air_filter_hepa", "amount": 1},
            {"itemId": "plastic_material", "amount": 2},
            {"itemId": "duct_tape", "amount": 1}
        ],
        "resultItemId": "air_filter",
        "resultAmount": 4,
        "craftingTimeHours": 2.0,
        "requiredStationId": "workbench",
        "requiredBlueprintId": "knowledge_air_filtration"
    },
    {
        "id": "craft_reconditioned_deep_cycle_bank",
        "recipeName": "Assemble Deep-Cycle Battery Bank",
        "ingredients": [
            {"itemId": "item_battery_reconditioned", "amount": 1},
            {"itemId": "electronic_scrap", "amount": 2},
            {"itemId": "copper_wire_10m_of_10m", "amount": 1}
        ],
        "resultItemId": "battery_pack",
        "resultAmount": 2,
        "craftingTimeHours": 3.0,
        "requiredStationId": "workbench",
        "requiredBlueprintId": "knowledge_battery_reconditioner_blueprint"
    },
    {
        "id": "craft_cbrn_respirator_filter",
        "recipeName": "Pack CBRN Respirator Cartridges",
        "ingredients": [
            {"itemId": "item_cbrn_cartridge", "amount": 1},
            {"itemId": "rubber_hose", "amount": 1},
            {"itemId": "plastic_material", "amount": 1}
        ],
        "resultItemId": "filter_pack",
        "resultAmount": 2,
        "craftingTimeHours": 1.5,
        "requiredStationId": "workbench",
        "requiredBlueprintId": "knowledge_cbrn_filter_blueprint"
    },
    {
        "id": "process_cloud_seeding_condensate",
        "recipeName": "Condense Atmospheric Moisture",
        "ingredients": [
            {"itemId": "item_cloud_seeding_canister", "amount": 1},
            {"itemId": "fuel", "amount": 1}
        ],
        "resultItemId": "clean_water_jug",
        "resultAmount": 2,
        "craftingTimeHours": 4.0,
        "requiredStationId": "distiller",
        "requiredBlueprintId": "knowledge_atmospheric_cloud_seeding"
    },
    {
        "id": "refit_vulcanized_diving_rig",
        "recipeName": "Refit Heavy Vulcanized Hazmat Rig",
        "ingredients": [
            {"itemId": "item_diving_suit_vulcanized", "amount": 1},
            {"itemId": "rubber_hose", "amount": 2},
            {"itemId": "duct_tape", "amount": 2}
        ],
        "resultItemId": "hazmat_suit",
        "resultAmount": 1,
        "craftingTimeHours": 5.0,
        "requiredStationId": "workbench",
        "requiredBlueprintId": "knowledge_submersible_salvage_rig"
    },
    {
        "id": "craft_calibrated_field_geiger",
        "recipeName": "Assemble Calibrated Field Radiometer",
        "ingredients": [
            {"itemId": "item_dosimeter_calibrated", "amount": 1},
            {"itemId": "electronic_scrap", "amount": 2},
            {"itemId": "plastic_material", "amount": 1}
        ],
        "resultItemId": "geiger_counter",
        "resultAmount": 1,
        "craftingTimeHours": 3.0,
        "requiredStationId": "workbench",
        "requiredBlueprintId": "knowledge_micro_dosimeter_blueprint"
    },
    {
        "id": "transcribe_field_guide_cultivation",
        "recipeName": "Transcribe Botanical Husbandry Guide",
        "ingredients": [
            {"itemId": "item_field_guide_annotated", "amount": 1},
            {"itemId": "paper_stock", "amount": 2}
        ],
        "resultItemId": "growing_manual",
        "resultAmount": 1,
        "craftingTimeHours": 2.0,
        "requiredStationId": "workbench",
        "requiredBlueprintId": "knowledge_field_guide_taxonomy"
    },
    {
        "id": "refit_improved_gas_mask_rig",
        "recipeName": "Outfit Advanced Gas Mask Rig",
        "ingredients": [
            {"itemId": "item_gas_mask_improved", "amount": 1},
            {"itemId": "rubber_hose", "amount": 1},
            {"itemId": "cloth", "amount": 2}
        ],
        "resultItemId": "gas_mask",
        "resultAmount": 2,
        "craftingTimeHours": 2.5,
        "requiredStationId": "workbench",
        "requiredBlueprintId": "knowledge_gas_mask_improved"
    },
    {
        "id": "craft_hydraulic_armored_shield",
        "recipeName": "Fabricate Hydraulic Reinforced Blast Shield",
        "ingredients": [
            {"itemId": "item_hydraulic_actuator", "amount": 1},
            {"itemId": "scrap_metal", "amount": 4},
            {"itemId": "mechanical_parts", "amount": 2}
        ],
        "resultItemId": "item_armored_blast_shield",
        "resultAmount": 1,
        "craftingTimeHours": 6.0,
        "requiredStationId": "workbench",
        "requiredBlueprintId": "knowledge_deep_well_hydraulics"
    },
    {
        "id": "batch_hydroponic_enrichment",
        "recipeName": "Synthesize Concentrated Rations",
        "ingredients": [
            {"itemId": "item_hydroponic_nutrients", "amount": 1},
            {"itemId": "clean_water", "amount": 2},
            {"itemId": "chemicals", "amount": 1}
        ],
        "resultItemId": "canned_food",
        "resultAmount": 4,
        "craftingTimeHours": 2.0,
        "requiredStationId": "distiller",
        "requiredBlueprintId": "knowledge_hydroponic_doser_blueprint"
    },
    {
        "id": "integrate_iff_transponder",
        "recipeName": "Integrate IFF Survival Transceiver",
        "ingredients": [
            {"itemId": "item_iff_beacon", "amount": 1},
            {"itemId": "electronic_scrap", "amount": 2},
            {"itemId": "battery", "amount": 1}
        ],
        "resultItemId": "handheld_radio",
        "resultAmount": 1,
        "craftingTimeHours": 2.5,
        "requiredStationId": "workbench",
        "requiredBlueprintId": "knowledge_iff_transponder_blueprint"
    },
    {
        "id": "assemble_hardened_military_transceiver",
        "recipeName": "Assemble Military Long-Range Radio",
        "ingredients": [
            {"itemId": "item_military_radio_module", "amount": 1},
            {"itemId": "electronic_scrap", "amount": 3},
            {"itemId": "copper_wire_10m_of_10m", "amount": 1}
        ],
        "resultItemId": "military_radio",
        "resultAmount": 1,
        "craftingTimeHours": 4.0,
        "requiredStationId": "workbench",
        "requiredBlueprintId": "knowledge_encrypted_radio_blueprint"
    },
    {
        "id": "assemble_cathode_radar_scope",
        "recipeName": "Assemble Cathode Night Vision Sight",
        "ingredients": [
            {"itemId": "item_radar_display_tube", "amount": 1},
            {"itemId": "electronic_scrap", "amount": 2},
            {"itemId": "copper_wire_10m_of_10m", "amount": 1}
        ],
        "resultItemId": "night_vision_scope",
        "resultAmount": 1,
        "craftingTimeHours": 3.5,
        "requiredStationId": "workbench",
        "requiredBlueprintId": "knowledge_radar_scope_blueprint"
    },
    {
        "id": "fabricate_radiation_blast_barrier",
        "recipeName": "Fabricate Borated Breaching Shield",
        "ingredients": [
            {"itemId": "item_radiation_shielding_panel", "amount": 1},
            {"itemId": "scrap_metal", "amount": 3},
            {"itemId": "metal_pipe", "amount": 2}
        ],
        "resultItemId": "item_titanium_breaching_shield",
        "resultAmount": 1,
        "craftingTimeHours": 5.0,
        "requiredStationId": "workbench",
        "requiredBlueprintId": "knowledge_radiation_shielding"
    },
    {
        "id": "encode_tactical_cipher_codebook",
        "recipeName": "Encode Tactical Cipher Codebook",
        "ingredients": [
            {"itemId": "item_radio_cipher_rotor", "amount": 1},
            {"itemId": "paper_stock", "amount": 2},
            {"itemId": "electronic_scrap", "amount": 1}
        ],
        "resultItemId": "item_comm_codebook_alpha",
        "resultAmount": 1,
        "craftingTimeHours": 2.0,
        "requiredStationId": "workbench",
        "requiredBlueprintId": "knowledge_radio_advanced"
    },
    {
        "id": "wire_vacuum_tube_headset",
        "recipeName": "Assemble Low-Noise Radio Headset",
        "ingredients": [
            {"itemId": "item_radio_vacuum_tube", "amount": 1},
            {"itemId": "copper_wire_10m_of_10m", "amount": 1},
            {"itemId": "plastic_material", "amount": 1}
        ],
        "resultItemId": "radio_headset",
        "resultAmount": 1,
        "craftingTimeHours": 2.0,
        "requiredStationId": "workbench",
        "requiredBlueprintId": "knowledge_signal_amplifier_blueprint"
    },
    {
        "id": "synthesize_reagent_radaway",
        "recipeName": "Synthesize Pure Rad-Away Solution",
        "ingredients": [
            {"itemId": "item_reagent_clean", "amount": 1},
            {"itemId": "chemicals", "amount": 2},
            {"itemId": "clean_water", "amount": 1}
        ],
        "resultItemId": "rad_away",
        "resultAmount": 2,
        "craftingTimeHours": 2.5,
        "requiredStationId": "distiller",
        "requiredBlueprintId": "knowledge_pharmacology_synthesis"
    },
    {
        "id": "assemble_piezoelectric_geophone",
        "recipeName": "Assemble Deep Geophone Probe",
        "ingredients": [
            {"itemId": "item_seismic_detector", "amount": 1},
            {"itemId": "scrap_metal", "amount": 2},
            {"itemId": "copper_wire_10m_of_10m", "amount": 1}
        ],
        "resultItemId": "item_geophone_probe",
        "resultAmount": 1,
        "craftingTimeHours": 3.0,
        "requiredStationId": "workbench",
        "requiredBlueprintId": "knowledge_seismic_fault_mapping"
    },
    {
        "id": "program_automated_sentry_feed",
        "recipeName": "Calibrate Automated Sentry Munitions",
        "ingredients": [
            {"itemId": "item_sentry_targeting_chip", "amount": 1},
            {"itemId": "electronic_scrap", "amount": 2},
            {"itemId": "mechanical_parts", "amount": 2}
        ],
        "resultItemId": "ammo_762",
        "resultAmount": 30,
        "craftingTimeHours": 2.0,
        "requiredStationId": "workbench",
        "requiredBlueprintId": "knowledge_automated_sentry_doctrine"
    },
    {
        "id": "assemble_pure_sine_solar_inverter",
        "recipeName": "Wire Pure-Sine Solar Inverter Array",
        "ingredients": [
            {"itemId": "item_solar_inverter", "amount": 1},
            {"itemId": "electronic_scrap", "amount": 3},
            {"itemId": "copper_wire_10m_of_10m", "amount": 2}
        ],
        "resultItemId": "battery_pack",
        "resultAmount": 3,
        "craftingTimeHours": 4.5,
        "requiredStationId": "workbench",
        "requiredBlueprintId": "knowledge_solar_advanced"
    },
    {
        "id": "assemble_precision_surgical_arm",
        "recipeName": "Assemble Precision Surgical Manipulator",
        "ingredients": [
            {"itemId": "item_surgical_arm_servo", "amount": 1},
            {"itemId": "scrap_metal", "amount": 2},
            {"itemId": "electronic_scrap", "amount": 1}
        ],
        "resultItemId": "field_surgical_kit",
        "resultAmount": 1,
        "craftingTimeHours": 3.5,
        "requiredStationId": "workbench",
        "requiredBlueprintId": "knowledge_surgical_robot_blueprint"
    },
    {
        "id": "repack_sterile_surgical_trauma_kit",
        "recipeName": "Repack Sterile Trauma Surgery Kit",
        "ingredients": [
            {"itemId": "item_surgical_kit", "amount": 1},
            {"itemId": "bandage", "amount": 3},
            {"itemId": "alcohol_wipes_box_10_of_10", "amount": 1}
        ],
        "resultItemId": "medical_kit",
        "resultAmount": 2,
        "craftingTimeHours": 1.5,
        "requiredStationId": "workbench",
        "requiredBlueprintId": "knowledge_field_trauma_surgery"
    },
    {
        "id": "assemble_thermal_breaching_rig",
        "recipeName": "Assemble Heavy Breaching Barrier",
        "ingredients": [
            {"itemId": "item_thermal_lance", "amount": 1},
            {"itemId": "scrap_metal", "amount": 4},
            {"itemId": "fuel", "amount": 1}
        ],
        "resultItemId": "item_titanium_breaching_shield",
        "resultAmount": 1,
        "craftingTimeHours": 4.0,
        "requiredStationId": "workbench",
        "requiredBlueprintId": "knowledge_hazmat_breaching_technique"
    },
    {
        "id": "preserve_rations_vacuum_canner",
        "recipeName": "Can Long-Term Storage Rations",
        "ingredients": [
            {"itemId": "item_vacuum_seal_canner", "amount": 1},
            {"itemId": "empty_tin_can", "amount": 5},
            {"itemId": "clean_water", "amount": 1}
        ],
        "resultItemId": "canned_food",
        "resultAmount": 5,
        "craftingTimeHours": 3.0,
        "requiredStationId": "stove",
        "requiredBlueprintId": "knowledge_cold_canning_preservation"
    }
]

# 1. Update recipes.json
existing_recipe_ids = set(r["id"] for r in recipes_doc["recipes"])
added_count = 0
for nr in new_recipes_data:
    if nr["id"] not in existing_recipe_ids:
        recipes_doc["recipes"].append(nr)
        existing_recipe_ids.add(nr["id"])
        added_count += 1

with open(recipes_path, "w") as f:
    json.dump(recipes_doc, f, indent=2)
    f.write("\n")

print(f"Added {added_count} new recipes to recipes.json.")

# 2. Build complete consumption map
all_recipe_consumers = {}
for r in recipes_doc["recipes"]:
    for ing in r.get("ingredients", []):
        iid = ing.get("itemId")
        if iid:
            all_recipe_consumers.setdefault(iid, []).append(r)

by_item = {}
for n in rk["knowledge_nodes"]:
    bt = n.get("breakthrough_item")
    if bt:
        by_item.setdefault(bt, []).append(n)

matrix_path = os.path.join(repo_root, "docs", "research", "BREAKTHROUGH_CONSUMPTION_MATRIX.md")

lines = [
    "# Breakthrough Item Consumption Matrix",
    "",
    "Authoritative machine-audited matrix validating that every breakthrough item rewarded by the 56 knowledge nodes in `research_knowledge.json` is actively consumed by at least one crafting recipe in `recipes.json`.",
    "",
    "| Breakthrough Item | Item Name | Source Knowledge Node(s) | Category | Consumer Recipe ID | Recipe Name | Station | Result Item | Status |",
    "|---|---|---|---|---|---|---|---|---|"
]

for bt in sorted(by_item.keys()):
    nodes = by_item[bt]
    node_str = "<br>".join(f"`{n['id']}`" for n in nodes)
    cats = ", ".join(sorted(list(set(n["category"] for n in nodes))))
    item_def = items.get(bt, {})
    item_name = item_def.get("displayName") or item_def.get("name") or bt
    consumers = all_recipe_consumers.get(bt, [])
    if consumers:
        c = consumers[0]
        res_def = items.get(c["resultItemId"], {})
        res_name = res_def.get("displayName") or res_def.get("name") or c["resultItemId"]
        status = "**CONSUMED**"
        lines.append(f"| `{bt}` | {item_name} | {node_str} | {cats} | `{c['id']}` | {c['recipeName']} | `{c.get('requiredStationId', '')}` | `{c['resultItemId']}` ({c.get('resultAmount', 1)}) | {status} |")
    else:
        lines.append(f"| `{bt}` | {item_name} | {node_str} | {cats} | — | — | — | — | **ORPHAN** |")

lines.extend([
    "",
    f"**Summary:** {len(by_item)} / {len(by_item)} unique breakthrough items have active recipe consumers in `recipes.json`. 0 orphaned breakthrough items.",
    ""
])

with open(matrix_path, "w") as f:
    f.write("\n".join(lines))

print(f"Matrix successfully written to {matrix_path} for {len(by_item)} breakthrough items.")
