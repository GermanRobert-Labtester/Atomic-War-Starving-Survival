#!/usr/bin/env python3
"""ASHFALL — alpha identity-art waves (faction emblems + survivor portraits).

Registry-driven: reads artifacts/asset_registry.json, generates only ids whose
status is FALLBACK for the requested category, writes 512x512 JPGs to
assets/art/<id>.jpg (the canonical probe path), and is resumable.

Backend: Composio GEMINI_GENERATE_IMAGE (3.x model). Prompts are seeded from a
stable hash of the id so the corpus is deterministic in style and variety, and
are written to avoid text, letters, numbers, and real-world insignia.

Quota: the provider serves roughly 1-2 images per minute, so every call passes a
global rate limiter (default 40s spacing) before it is sent.
"""
import argparse
import concurrent.futures
import hashlib
import json
import os
import subprocess
import sys
import threading
import time
import urllib.request

PROJECT_ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
REGISTRY = os.path.join(PROJECT_ROOT, "artifacts", "asset_registry.json")
ART_DIR = os.path.join(PROJECT_ROOT, "assets", "art")
GEN_ROOT = os.path.join(PROJECT_ROOT, "generated_AIassets")

# 3.x image model. The Composio tool accepts 2.5-flash / 3-pro-preview / 2.0-exp;
# there is currently no 3.1-3.8 flash image variant in the tool schema.
MODEL = "gemini-3-pro-image-preview"

# Provider quota is 1-2 images/minute; the limiter enforces <=1 per interval.
RATE_MIN_INTERVAL_SECONDS = 40.0
_rate_lock = threading.Lock()
_rate_last_call = 0.0


def _rate_limit():
    global _rate_last_call
    with _rate_lock:
        wait = _rate_last_call + RATE_MIN_INTERVAL_SECONDS - time.monotonic()
        if wait > 0:
            time.sleep(wait)
        _rate_last_call = time.monotonic()


def _now():
    return time.strftime("%H:%M:%S")


FACTION_MOTIFS = [
    "three stacked chevrons", "concentric broken rings", "a single vertical bar through a circle",
    "two interlocking brackets", "a stepped ziggurat of four blocks", "a diamond split by a slit",
    "five vertical bars of unequal height", "a triangle lattice over a base line",
    "two crossed slashes inside a ring", "a keyway-shaped notch in a disc",
    "a gate of three uprights and a lintel", "a spiral of two turns",
    "a chevron above two dots", "a broken arrow shaft", "a half-circle resting on a bar",
    "three ascending blocks", "a bolt-head polygon with a slit", "a leaning square",
]
FACTION_FRAMES = ["hexagon", "shield outline", "roundel", "square plate", "diamond", "arched plaque"]

PORTRAIT_SIGNS = [
    "soot smudged on one cheek", "a faded scar across the brow", "a strip of cloth bandage on the jaw",
    "frost-nipped fingertips resting at the collar", "a respirator hanging loose at the neck",
    "dust in the hairline", "a small stitched repair on the shoulder seam", "no visible marks",
]
PORTRAIT_GARMENTS = [
    "a worn wool coat with a missing button", "canvas work jacket with a stencilled shelter number omitted",
    "grey shelter overalls", "a patched field shirt", "a knit cap and high collar",
    "a long oilskin duster over a sweater", "layered undershirt and suspenders",
]
PORTRAIT_LIGHT = ["flat archive lamp lighting", "dim fluorescent lighting", "cold window light", "single bulb overhead"]

LOCATION_MOODS = [
    "overcast ash-grey sky", "low winter sun through haze", "cold blue twilight",
    "flat white fog", "dusk with a single sodium lamp", "grey rain over concrete",
]
LOCATION_WEAR = [
    "cracked asphalt and drifted ash", "rubble spilling from a collapsed wall",
    "rusted railings and broken glass", "flooded basement stairs", "fire-scarred brick",
    "tangled overhead cables", "sand-drifted doorways", "abandoned vehicles under dust",
]
LOCATION_KINDS = [
    "a fortified shelter entrance", "a water treatment outfall", "a rail yard",
    "a clinic courtyard", "a market hall", "a radio tower base", "a school gymnasium",
    "a municipal archive", "a pump station", "a rooftop water tank",
]

ITEM_MATERIALS = [
    "scratched steel", "rusted iron", "scuffed aluminium", "clouded glass",
    "worn canvas", "tarnished brass", "brittle plastic", "stained faience",
]
ITEM_FORMS = [
    "worn hand tool", "sealed canister", "folded document", "small machine part",
    "glass vial", "coiled wire spool", "flat circuit board", "fabric patch kit",
    "hand-cranked device", "battery cell", "measuring instrument", "sealed envelope",
]


def seed_bytes(identifier: str) -> bytes:
    return hashlib.sha256(identifier.encode("utf-8")).digest()


def pick(seq, digest, offset=0):
    return seq[digest[offset % len(digest)] % len(seq)]


def faction_prompt(faction_id: str, display: str) -> str:
    d = seed_bytes(faction_id)
    motif = pick(FACTION_MOTIFS, d, 0)
    frame = pick(FACTION_FRAMES, d, 1)
    return (
        f"Post-nuclear bunker faction emblem for a faction called {display}. "
        f"Abstract geometric insignia: {motif} inside a {frame}. "
        "No text, no letters, no numbers, no real-world symbols or flags. "
        "Muted bone white and oxidised rust on dark charcoal, worn stencil print texture, "
        "chipped paint, flat graphic, centered, square composition, no border text."
    )


def portrait_prompt(survivor_id: str, display: str) -> str:
    d = seed_bytes(survivor_id)
    sign = pick(PORTRAIT_SIGNS, d, 0)
    garment = pick(PORTRAIT_GARMENTS, d, 1)
    light = pick(PORTRAIT_LIGHT, d, 2)
    return (
        f"Archival civil-defence identification photograph of a fictional survivor named {display}. "
        f"Head and shoulders, neutral exhausted expression, {sign}, wearing {garment}, {light}, "
        "plain dark charcoal backdrop, 1950s documentary photography, desaturated muted tones, "
        "fine film grain, slight print wear. No text, no letters, no numbers, no watermark, no logo."
    )


def location_prompt(location_id: str, display: str) -> str:
    d = seed_bytes(location_id)
    kind = pick(LOCATION_KINDS, d, 0)
    mood = pick(LOCATION_MOODS, d, 1)
    wear = pick(LOCATION_WEAR, d, 2)
    return (
        f"Post-nuclear wasteland location view of {display}, depicted as {kind}. "
        f"{wear}, {mood}, no people, no text, no letters, no numbers, no signage. "
        "Desaturated muted palette, documentary photography, fine grain, wide establishing shot."
    )


def item_prompt(item_id: str, display: str) -> str:
    d = seed_bytes(item_id)
    form = pick(ITEM_FORMS, d, 0)
    material = pick(ITEM_MATERIALS, d, 1)
    return (
        f"Salvaged inventory icon of {display}, a {form} in {material}. "
        "Single object centered on a plain dark charcoal backdrop, soft even studio light, "
        "muted desaturated palette, worn post-nuclear scavenge aesthetic, no text, no letters, "
        "no numbers, no watermark, no logo."
    )


def display_name(identifier: str) -> str:
    words = identifier.split("_")
    if words and words[0] in {"faction", "survivor", "location", "item"}:
        words = words[1:]
    return " ".join(w.capitalize() for w in words) or identifier


def load_targets(category: str, limit=None):
    with open(REGISTRY) as f:
        registry = json.load(f)
    ids = [e["id"] for e in registry["entries"]
           if e.get("category") == category and e.get("status") != "LOADED"]
    # Existing on disk are already produced (resume).
    todo = [i for i in ids if not os.path.exists(os.path.join(ART_DIR, f"{i}.jpg"))]
    if limit:
        todo = todo[:limit]
    return ids, todo


def generate_one(identifier: str, category: str):
    builders = {
        "faction": faction_prompt,
        "portrait": portrait_prompt,
        "location": location_prompt,
        "item": item_prompt,
    }
    prompt = builders[category](identifier, display_name(identifier))
    cmd = ["composio", "execute", "GEMINI_GENERATE_IMAGE",
           "-d", json.dumps({"prompt": prompt, "model": MODEL}), "-p"]
    _rate_limit()
    try:
        proc = subprocess.run(cmd, capture_output=True, text=True, timeout=300)
        data = json.loads(proc.stdout)
        result = data.get("results", [{}])[0]
        if not (data.get("successful") and result.get("successful")):
            return identifier, False, str(result.get("error", "unknown"))[:200]
        url = result["data"]["image"]["s3url"]
        gen_dir = os.path.join(GEN_ROOT, category)
        os.makedirs(gen_dir, exist_ok=True)
        png_path = os.path.join(gen_dir, f"{identifier}.png")
        req = urllib.request.Request(url, headers={"User-Agent": "Mozilla/5.0"})
        # Atomic write: Godot's importer scans generated_AIassets/ while the wave
        # runs, and a half-written PNG reads as ERR_FILE_CORRUPT.
        tmp_png = png_path + ".tmp"
        with urllib.request.urlopen(req, timeout=120) as resp, open(tmp_png, "wb") as out:
            out.write(resp.read())
        os.replace(tmp_png, png_path)
        # Canonical 512x512 JPG at the registry probe path.
        from PIL import Image
        image = Image.open(png_path).convert("RGB")
        side = min(image.size)
        left = (image.width - side) // 2
        top = (image.height - side) // 2
        image = image.crop((left, top, left + side, top + side)).resize((512, 512), Image.LANCZOS)
        # Atomic write: a reader (Godot import / snapshots) must never see a
        # half-written JPG, so write to a temp file and rename into place.
        final_path = os.path.join(ART_DIR, f"{identifier}.jpg")
        tmp_path = final_path + ".tmp"
        image.save(tmp_path, "JPEG", quality=88)
        os.replace(tmp_path, final_path)
        return identifier, True, None
    except Exception as exc:  # noqa: BLE001 - tool reports failures, not crashes
        return identifier, False, str(exc)[:200]


def main():
    global RATE_MIN_INTERVAL_SECONDS

    parser = argparse.ArgumentParser()
    parser.add_argument("category", choices=["faction", "portrait", "location", "item"])
    parser.add_argument("--limit", type=int, default=None)
    parser.add_argument("--workers", type=int, default=1)
    parser.add_argument("--min-interval", type=float, default=RATE_MIN_INTERVAL_SECONDS)
    args = parser.parse_args()

    RATE_MIN_INTERVAL_SECONDS = args.min_interval

    all_ids, todo = load_targets(args.category, args.limit)
    print(f"[art-wave] {_now()} {args.category}: {len(all_ids)} fallback ids, {len(todo)} to generate "
          f"(model={MODEL}, workers={args.workers}, interval={args.min_interval:.0f}s)", flush=True)
    if not todo:
        print("[art-wave] nothing to do", flush=True)
        return 0

    done = failed = 0
    failures = []
    with concurrent.futures.ThreadPoolExecutor(max_workers=args.workers) as pool:
        futures = {pool.submit(generate_one, i, args.category): i for i in todo}
        for index, future in enumerate(concurrent.futures.as_completed(futures), 1):
            identifier, ok, error = future.result()
            if ok:
                done += 1
                print(f"  [{index}/{len(todo)}] {_now()} OK {identifier}", flush=True)
            else:
                failed += 1
                failures.append(identifier)
                print(f"  [{index}/{len(todo)}] {_now()} FAIL {identifier}: {error}", flush=True)

    print(f"[art-wave] {_now()} done: {done} generated, {failed} failed", flush=True)
    if failures:
        print("[art-wave] failed ids:", ", ".join(failures), flush=True)
    return 0 if failed == 0 else 1


if __name__ == "__main__":
    sys.exit(main())
