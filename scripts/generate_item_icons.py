#!/usr/bin/env python3
"""Batch generate item icons via Composio GEMINI_GENERATE_IMAGE."""
import json, os, subprocess, sys, time, urllib.request

PROJECT_ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
GEN_DIR = os.path.join(PROJECT_ROOT, "generated_AIassets", "items")
ART_DIR = os.path.join(PROJECT_ROOT, "assets", "art")

TYPE_COLORS = {
    "Weapon": "red", "Ammo": "red", "Ammunition": "red", "AmmoComponent": "red",
    "Medical": "green", "Iodine": "green", "Drug": "green", "Stimulant": "green",
    "Device": "blue", "Tool": "blue", "Workbench": "blue", "Container": "blue",
    "Material": "gold amber", "Fuel": "gold amber", "Resource": "gold amber",
    "Scrap": "gold amber", "Chemical": "gold amber", "Construction": "gold amber",
    "Water": "gold amber", "Filter": "gold amber", "Power": "gold amber",
    "Equipment": "gold amber",
    "Quest": "purple", "Relic": "purple", "Document": "purple", "Artifact": "purple", "Data": "purple",
    "Food": "orange", "Consumable": "orange", "Drink": "orange", "Cooked": "orange",
    "Clothing": "silver", "Armor": "silver", "Protective": "silver",
    "Comfort": "gold amber", "Trade": "gold amber", "Currency": "gold amber", "Gem": "gold amber",
}

CATEGORY_LABELS = {
    "Weapon": "weapon", "Ammo": "ammunition", "Ammunition": "ammunition", "AmmoComponent": "ammunition component",
    "Medical": "medical supply", "Iodine": "medical iodine", "Drug": "pharmaceutical", "Stimulant": "stimulant",
    "Device": "device", "Tool": "tool", "Workbench": "workbench station", "Container": "container",
    "Material": "material", "Fuel": "fuel", "Resource": "resource", "Scrap": "scrap material",
    "Chemical": "chemical", "Construction": "construction material",
    "Water": "water container", "Filter": "filter", "Power": "power source", "Equipment": "equipment",
    "Quest": "quest document", "Relic": "pre-war relic", "Document": "document", "Artifact": "artifact", "Data": "data storage",
    "Food": "food", "Consumable": "consumable", "Drink": "beverage", "Cooked": "cooked meal",
    "Clothing": "clothing", "Armor": "armour", "Protective": "protective gear",
    "Comfort": "comfort item", "Trade": "trade goods", "Currency": "currency", "Gem": "gemstone",
}


def load_items():
    with open(os.path.join(PROJECT_ROOT, "Assets", "StreamingAssets", "Data", "items.json")) as f:
        data = json.load(f)
    return data if isinstance(data, list) else data.get("items", data.get("Items", []))


def get_generated():
    if not os.path.isdir(GEN_DIR):
        return set()
    return {os.path.splitext(f)[0] for f in os.listdir(GEN_DIR)}


def build_batch(items, batch_size=10):
    generated = get_generated()
    missing = []
    for item in items:
        iid = item.get("id", "")
        if iid and iid not in generated and "deprecated" not in iid:
            itype = item.get("type", "Material")
            color = TYPE_COLORS.get(itype, "gold amber")
            cat_label = CATEGORY_LABELS.get(itype, "item")
            missing.append({
                "id": iid,
                "name": item.get("displayName", iid),
                "type": itype,
                "color": color,
                "cat_label": cat_label,
                "desc": item.get("description", "")[:150],
            })
    batches = []
    for i in range(0, len(missing), batch_size):
        batches.append(missing[i : i + batch_size])
    return batches


def make_prompt(item):
    return (
        f"Game item icon: {item['name']}, {item['desc'][:80]}, "
        f"post-apocalyptic {item['cat_label']}, minimalist pixel-art style, "
        f"{item['color']} border on dark background, simple geometric design in circle frame, 1024x1024"
    )


def generate_one(item):
    prompt = make_prompt(item)
    cmd = [
        "composio", "execute", "GEMINI_GENERATE_IMAGE",
        "-d", json.dumps({"prompt": prompt, "model": "gemini-3-pro-image-preview"}),
        "-p",
    ]
    try:
        result = subprocess.run(cmd, capture_output=True, text=True, timeout=90)
        data = json.loads(result.stdout)
        if data.get("successful") and data["results"][0].get("successful"):
            return data["results"][0]["data"]["image"]["s3url"]
        else:
            print(f"  FAIL {item['id']}: {data['results'][0].get('error', 'unknown')}", file=sys.stderr)
            return None
    except Exception as e:
        print(f"  ERROR {item['id']}: {e}", file=sys.stderr)
        return None


def download(url, path):
    urllib.request.urlretrieve(url, path)


def process_batch(batch, batch_num, total_batches):
    print(f"\n{'='*60}")
    print(f"Batch {batch_num}/{total_batches} — {len(batch)} items")
    print(f"{'='*60}")
    for item in batch:
        print(f"  [{item['type']}] {item['id']}: {item['name']}")

    # Generate all in parallel via subprocess
    procs = []
    for item in batch:
        prompt = make_prompt(item)
        cmd = [
            "composio", "execute", "GEMINI_GENERATE_IMAGE",
            "-d", json.dumps({"prompt": prompt, "model": "gemini-3-pro-image-preview"}),
            "-p",
        ]
        proc = subprocess.Popen(cmd, stdout=subprocess.PIPE, stderr=subprocess.PIPE)
        procs.append((item, proc))

    # Collect results
    success = 0
    for item, proc in procs:
        try:
            stdout, stderr = proc.communicate(timeout=120)
            data = json.loads(stdout)
            if data.get("successful") and data["results"][0].get("successful"):
                s3url = data["results"][0]["data"]["image"]["s3url"]
                png_path = os.path.join(GEN_DIR, f"{item['id']}.png")
                jpg_path = os.path.join(ART_DIR, f"{item['id']}.jpg")
                download(s3url, png_path)
                # Copy to assets/art as jpg
                subprocess.run(["cp", png_path, jpg_path], check=True)
                success += 1
                print(f"  ✓ {item['id']}")
            else:
                err = data.get("results", [{}])[0].get("error", "unknown error")
                print(f"  ✗ {item['id']}: {err}")
        except Exception as e:
            print(f"  ✗ {item['id']}: {e}")

    print(f"\n  Result: {success}/{len(batch)} generated")
    return success


def main():
    start_batch = int(sys.argv[1]) if len(sys.argv) > 1 else 0
    end_batch = int(sys.argv[2]) if len(sys.argv) > 2 else None

    items = load_items()
    batches = build_batch(items)
    total = len(batches)

    if end_batch is None:
        end_batch = total

    print(f"Total items: {len(items)}")
    print(f"Missing icons: {sum(len(b) for b in batches)}")
    print(f"Batches: {total}")
    print(f"Processing batches {start_batch} to {end_batch - 1}")

    total_success = 0
    total_items = 0
    for i in range(start_batch, min(end_batch, total)):
        s = process_batch(batches[i], i + 1, total)
        total_success += s
        total_items += len(batches[i])

    print(f"\n{'='*60}")
    print(f"DONE: {total_success}/{total_items} icons generated across batches {start_batch+1}-{min(end_batch, total)}")
    print(f"{'='*60}")


if __name__ == "__main__":
    main()
