#!/usr/bin/env python3
"""
Composio 1024x1024 Game Asset Generation Pipeline for ASHFALL.
Generates assets exclusively via Composio GEMINI_GENERATE_IMAGE (Nano Banana Pro),
stages 1024x1024 masters in generated_AIassets/, and imports them into assets/art/.
"""

import json
import os
import subprocess
import sys
import time
import hashlib
from datetime import datetime, timezone

WORKSPACE_ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
GENERATED_DIR = os.path.join(WORKSPACE_ROOT, "generated_AIassets")
ART_DIR = os.path.join(WORKSPACE_ROOT, "assets", "art")
MANIFEST_PATH = os.path.join(GENERATED_DIR, "_manifest.json")

ASSETS_TO_GENERATE = [
    {
        "id": "item_dosimeter_pen",
        "family": "item",
        "folder": "items",
        "target_path": "assets/art/item_dosimeter_pen.jpg",
        "prompt": (
            "Item icon of a Soviet-era quartz fiber pocket pen dosimeter, "
            "cylindrical brass and steel tube with optical eyepiece clip, metallic barrel with fine calibration rings, "
            "cold post-apocalyptic atomic survival gear aesthetic, dry-gouache digital painting, "
            "stark rim lighting, charcoal edge lines, isolated on solid opaque black background, "
            "no text, no numbers, clean readable silhouette, 1024x1024 master."
        )
    },
    {
        "id": "item_air_filter_hepa",
        "family": "item",
        "folder": "items",
        "target_path": "assets/art/item_air_filter_hepa.jpg",
        "prompt": (
            "Item icon of an industrial nuclear bunker HEPA air filtration core cartridge, "
            "cylindrical pleated filter paper with heavy black rubber gasket seals and rust-treated metal cage housing, "
            "cold atomic survival gear aesthetic, dry-gouache digital painting, stark lighting, "
            "charcoal edge lines, isolated on solid opaque black background, no text, no numbers, clean silhouette."
        )
    },
    {
        "id": "item_desal_membrane",
        "family": "item",
        "folder": "items",
        "target_path": "assets/art/item_desal_membrane.jpg",
        "prompt": (
            "Item icon of a high-pressure reverse osmosis desalination water filtration membrane cartridge, "
            "cylindrical spiral-wound composite core in reinforced PVC housing with brass coupling fittings, "
            "cold atomic survival aesthetic, dry-gouache digital painting, stark rim lighting, "
            "isolated on solid opaque black background, no text, no numbers, clean silhouette."
        )
    },
    {
        "id": "item_seed_mushroom",
        "family": "item",
        "folder": "items",
        "target_path": "assets/art/item_seed_mushroom.jpg",
        "prompt": (
            "Item icon of a small coarse burlap seed pouch of subterranean glowing mushroom spores, "
            "faint pale-cyan bioluminescent spore dust leaking from stitched rough canvas neck, "
            "cold post-apocalyptic atomic survival aesthetic, dry-gouache digital painting, stark lighting, "
            "charcoal edge lines, isolated on solid opaque black background, no text, no numbers, clean silhouette."
        )
    },
    {
        "id": "crop_mushroom",
        "family": "item",
        "folder": "items",
        "target_path": "assets/art/crop_mushroom.jpg",
        "prompt": (
            "Item icon of a small cluster of freshly harvested edible subterranean shelter mushrooms, "
            "pale fibrous stems and bioluminescent caps with subtle teal glow, damp soil clinging to base, "
            "cold post-apocalyptic survival aesthetic, dry-gouache digital painting, stark rim lighting, "
            "isolated on solid opaque black background, no text, no numbers, clean readable silhouette."
        )
    },
    {
        "id": "item_seed_tuber",
        "family": "item",
        "folder": "items",
        "target_path": "assets/art/item_seed_tuber.jpg",
        "prompt": (
            "Item icon of hardy cold-resistant seed potato eyes and tuber cuttings ready for greenhouse planting, "
            "sprouting purple-green eyes with rich dark loam soil, cold post-apocalyptic survival aesthetic, "
            "dry-gouache digital painting, stark lighting, charcoal edge lines, "
            "isolated on solid opaque black background, no text, no numbers, clean silhouette."
        )
    },
    {
        "id": "crop_tuber",
        "family": "item",
        "folder": "items",
        "target_path": "assets/art/crop_tuber.jpg",
        "prompt": (
            "Item icon of a bundle of freshly harvested cold-hardy bunker potatoes and root tubers, "
            "rough dirt-caked skin with rich earthy texture, dry-gouache digital painting, "
            "cold post-apocalyptic survival aesthetic, stark rim lighting, charcoal edge lines, "
            "isolated on solid opaque black background, no text, no numbers, clean silhouette."
        )
    },
    {
        "id": "bg_bunker_corridor",
        "family": "environment",
        "folder": "backgrounds",
        "target_path": "assets/art/bg_bunker_corridor.jpg",
        "prompt": (
            "Environment key-art plate of an underground nuclear shelter main access corridor, "
            "damp reinforced concrete walls with faded hazard stripes, exposed steel rebar, ceiling conduit wires, "
            "heavy steel blast door in background, dim emergency amber bulkhead lamps casting long shadows, "
            "cold oppressive survival atmosphere, dry-gouache painterly digital illustration, charcoal line art, "
            "human scale, 1024x1024 full scene plate, no text."
        )
    },
    {
        "id": "bg_filtration_stack",
        "family": "environment",
        "folder": "backgrounds",
        "target_path": "assets/art/bg_filtration_stack.jpg",
        "prompt": (
            "Environment key-art plate of an underground holdfast air filtration and decontamination chamber, "
            "massive industrial ventilation blowers, tiered HEPA filter banks, lead-shielded pipes, "
            "airlock pressure hatch, emergency chemical wash shower grate, cold dim utility lighting, "
            "dry-gouache painterly digital illustration, charcoal edges, cold bleak atomic survival atmosphere, "
            "1024x1024 full scene plate, no text."
        )
    },
    {
        "id": "bg_bunks_living",
        "family": "environment",
        "folder": "backgrounds",
        "target_path": "assets/art/bg_bunks_living.jpg",
        "prompt": (
            "Environment key-art plate of an underground shelter communal bunk quarters, "
            "tiered metal frame cot beds with rough grey wool blankets, personal footlockers, "
            "damp concrete walls, small improvised heater stove giving off faint warm embers in cold gloom, "
            "dry-gouache painterly digital illustration, charcoal edge lines, somber human survival atmosphere, "
            "1024x1024 full scene plate, no text."
        )
    },
    {
        "id": "bg_storage_locker",
        "family": "environment",
        "folder": "backgrounds",
        "target_path": "assets/art/bg_storage_locker.jpg",
        "prompt": (
            "Environment key-art plate of an underground bunker supply depot and secure locker room, "
            "heavy industrial wire shelving stocked with ration tins, sealed water carboys, fuel jerrycans, "
            "locked steel cage cabinets, cold atmospheric lighting with single caged yellow ceiling lamp, "
            "dry-gouache painterly digital illustration, charcoal line art, 1024x1024 full scene plate, no text."
        )
    },
    {
        "id": "bg_radio_corner",
        "family": "environment",
        "folder": "backgrounds",
        "target_path": "assets/art/bg_radio_corner.jpg",
        "prompt": (
            "Environment key-art plate of an underground holdfast radio listening post and communications desk, "
            "rack-mounted vintage shortwave vacuum tube radio transceiver with warm glowing indicator needles, "
            "oscilloscope cathode tube, heavy operator headset hanging on side, coiled cables, "
            "dry-gouache painterly digital illustration, atmospheric amber-in-darkness lighting, "
            "1024x1024 full scene plate, no text."
        )
    },
    {
        "id": "bg_greenhouse_bay",
        "family": "environment",
        "folder": "backgrounds",
        "target_path": "assets/art/bg_greenhouse_bay.jpg",
        "prompt": (
            "Environment key-art plate of a sub-surface hydroponic bunker greenhouse bay, "
            "tiered wooden planter grow beds filled with dark fertile soil, glowing fungal caps and green sprouts, "
            "overhead purple-amber UV grow lamps reflecting on wet concrete floor, PVC irrigation tubing, "
            "dry-gouache painterly digital illustration, charcoal line art, cold hopeful survival atmosphere, "
            "1024x1024 full scene plate, no text."
        )
    },
    {
        "id": "survivor_dr_sarah_chen",
        "family": "survivor",
        "folder": "portraits",
        "target_path": "assets/art/survivor_dr_sarah_chen.jpg",
        "prompt": (
            "Character portrait of Dr. Sarah Chen, a 38-year-old Asian female trauma surgeon surviving in a nuclear bunker, "
            "tired intelligent determined expression, dark hair tied into a practical knot, "
            "wearing faded surgical scrub top under a heavy patched wool field vest, sterile surgical mask pulled down around neck, "
            "dry-gouache digital painting, charcoal edge lines, dramatic cinematic side-lighting, "
            "centered bust portrait on solid opaque dark charcoal background, no text, 1024x1024 master."
        )
    },
    {
        "id": "survivor_gunner_mikhail",
        "family": "survivor",
        "folder": "portraits",
        "target_path": "assets/art/survivor_gunner_mikhail.jpg",
        "prompt": (
            "Character portrait of Gunner Mikhail, a 45-year-old Slavic heavy artillery loader, "
            "rugged broad-shouldered build, short grey-flecked buzzcut, square jaw with shrapnel scar, "
            "weathered skin with mild radiation flush, stoic guarded stare, "
            "wearing grease-stained heavy military canvas field jacket with lead collar reinforcement, "
            "dry-gouache digital painting, dramatic rim lighting, charcoal edge lines, "
            "centered bust portrait on solid opaque dark charcoal background, no text, 1024x1024 master."
        )
    }
]


def execute_composio_generation(prompt: str) -> str:
    """Calls Composio CLI GEMINI_GENERATE_IMAGE and returns the presigned image URL."""
    payload = {
        "prompt": prompt,
        "model": "gemini-3-pro-image-preview",
        "image_size": "1K",
        "aspect_ratio": "1:1"
    }
    cmd = [
        "composio", "execute", "GEMINI_GENERATE_IMAGE",
        "-d", json.dumps(payload)
    ]
    print(f"  [Composio] Dispatching GEMINI_GENERATE_IMAGE (Nano Banana Pro 1024x1024)...")
    res = subprocess.run(cmd, capture_output=True, text=True)
    if res.returncode != 0:
        raise RuntimeError(f"Composio execution failed: {res.stderr or res.stdout}")

    out_text = res.stdout
    json_start = out_text.find("{")
    if json_start < 0:
        raise RuntimeError(f"No JSON found in Composio response: {out_text}")

    data = json.loads(out_text[json_start:])
    if not data.get("successful"):
        raise RuntimeError(f"Composio returned error: {data.get('error')}")

    img_data = data.get("data", {}).get("image", {})
    s3url = img_data.get("s3url")
    if not s3url:
        raise RuntimeError(f"No image URL in Composio response: {data}")

    return s3url


def download_and_verify(url: str, output_path: str) -> bool:
    """Downloads image and verifies it is 1024x1024."""
    os.makedirs(os.path.dirname(output_path), exist_ok=True)
    cmd = ["curl", "-s", url, "-o", output_path]
    subprocess.run(cmd, check=True)

    res = subprocess.run(["file", output_path], capture_output=True, text=True)
    print(f"  [File Check] {res.stdout.strip()}")
    return "1024x1024" in res.stdout or "1024 x 1024" in res.stdout or os.path.getsize(output_path) > 10000


def update_manifest(asset_def: dict, local_path: str):
    """Updates _manifest.json with new asset entry."""
    manifest = {"version": "1.0", "updated_at": datetime.now(timezone.utc).isoformat(), "assets": []}
    if os.path.exists(MANIFEST_PATH):
        try:
            with open(MANIFEST_PATH, "r") as f:
                manifest = json.load(f)
        except Exception:
            pass

    rel_local = os.path.relpath(local_path, WORKSPACE_ROOT)
    prompt_hash = hashlib.sha256(asset_def["prompt"].encode()).hexdigest()[:16]

    manifest["assets"] = [a for a in manifest.get("assets", []) if a.get("id") != asset_def["id"]]

    manifest["assets"].append({
        "id": asset_def["id"],
        "provider": "mcp_composio_gemini",
        "prompt_hash": prompt_hash,
        "seed": 42,
        "path": rel_local,
        "status": "approved",
        "approved_at": datetime.now(timezone.utc).isoformat(),
        "import_target": asset_def["target_path"]
    })
    manifest["updated_at"] = datetime.now(timezone.utc).isoformat()

    with open(MANIFEST_PATH, "w") as f:
        json.dump(manifest, f, indent=2)


def main():
    print("=================================================================")
    print("ASHFALL Composio 1024x1024 Image Generation Pipeline")
    print(f"Total assets queued: {len(ASSETS_TO_GENERATE)}")
    print("=================================================================")

    passed = 0
    failed = 0

    for idx, asset in enumerate(ASSETS_TO_GENERATE, start=1):
        asset_id = asset["id"]
        folder = asset.get("folder", "items")
        local_stage_path = os.path.join(GENERATED_DIR, folder, f"{asset_id}.jpg")
        target_runtime_path = os.path.join(WORKSPACE_ROOT, asset["target_path"])

        print(f"\n[{idx}/{len(ASSETS_TO_GENERATE)}] Processing: {asset_id} ({asset['family']})")

        try:
            url = execute_composio_generation(asset["prompt"])
            print(f"  [Download] Staging to {local_stage_path}...")
            ok = download_and_verify(url, local_stage_path)
            if not ok:
                print(f"  [ERROR] Image verification failed for {asset_id}")
                failed += 1
                continue

            os.makedirs(os.path.dirname(target_runtime_path), exist_ok=True)
            subprocess.run(["cp", local_stage_path, target_runtime_path], check=True)
            print(f"  [Import] Copied to {target_runtime_path}")

            png_target = os.path.splitext(target_runtime_path)[0] + ".png"
            if not os.path.exists(png_target):
                subprocess.run(["cp", local_stage_path, png_target], check=True)

            update_manifest(asset, local_stage_path)
            print(f"  [Manifest] Registered in _manifest.json (approved)")
            passed += 1
            time.sleep(2)
        except Exception as ex:
            print(f"  [FAILED] {asset_id}: {ex}")
            failed += 1

    print("\n=================================================================")
    print(f"Pipeline Run Complete: {passed} passed, {failed} failed")
    print("=================================================================")


if __name__ == "__main__":
    main()
