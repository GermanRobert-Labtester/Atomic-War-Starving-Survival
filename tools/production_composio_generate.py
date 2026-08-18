#!/usr/bin/env python3
"""Phase 18 — Composio-backed production batch generator.

Generates staging assets for the production-art queue via the Composio
gateway, using:

  - GEMINI_GENERATE_IMAGE  (primary, via Composio)
  - OPENAI_CREATE_IMAGE    (secondary, via Composio)

The script:

  1. Reads the production-art manifest.
  2. Filters to the surfaced actionable rows (Phase 17).
  3. Builds an ASHFALL-style prompt per content_id (using the per-row
     prompt template produced by `production_prompt_composer.py`).
  4. Calls the Composio gateway to generate each prompt.
  5. Downloads the result to `assets/_staging_generated/<family>/<content_id>.png`.
  6. Records every attempt in the ledger.

The QA harness (`tools/production_qa.py`) is run separately after the
batch completes. Promotion (`tools/production_promote.py`) is gated on
QA-PASS.

The script is idempotent: re-running it skips already-staged content_ids.

Output:
  - assets/_staging_generated/<family>/<content_id>.png
  - docs/visual/PRODUCTION_ART_GENERATION_LEDGER.json (updated)
"""
import json
import os
import subprocess
import sys
import time
import urllib.request
from pathlib import Path
from collections import Counter

REPO = Path("/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War")
STAGING = REPO / "assets/_staging_generated"
MANIFEST = REPO / "docs/visual/PRODUCTION_ART_GENERATION_MANIFEST.json"
PROMPTS_DIR = REPO / "docs/visual/generated_prompts"
TOP_IDS = REPO / "docs/visual/runtime_context_top_ids.json"
LEDGER = REPO / "docs/visual/PRODUCTION_ART_GENERATION_LEDGER.json"

FAMILY_TO_SUBDIR = {
    "Inventory-Item": "items",
    "Survivor-Portrait": "portraits",
    "NPC-Portrait": "portraits",
    "Location-Art": "locations",
    "Faction-Art": "factions",
}


def load_surfaced_content_ids():
    """Return the set of content_ids surfaced by the runtime top-N."""
    if not TOP_IDS.exists():
        return set()
    data = json.loads(TOP_IDS.read_text())
    out = set()
    for cat in ("items", "survivors", "locations", "characters"):
        for cid in data.get("top_in_manifest", {}).get(cat, []):
            out.add(cid)
    return out


def load_manifest_rows():
    return json.loads(MANIFEST.read_text())


def load_prompt_for(content_id):
    """Load the per-content_id prompt JSON emitted by the prompt composer."""
    p = PROMPTS_DIR / f"{content_id}.json"
    if not p.exists():
        return None
    return json.loads(p.read_text())


def derive_subject(content_id, family, subfamily):
    """Translate the snake_case content_id into a plain-English subject hint.

    Many ASHFALL ids encode their semantic meaning directly:
      npc_cluster_teacher  -> a teacher
      loc_apiary_rows      -> rows of beehives / apiary
      loc_grange_hall      -> a grange hall (large agricultural barn)
    """
    cid = (content_id or "").lower()
    # Strip the family prefix
    for prefix in ("npc_", "loc_", "location_", "item_", "faction_", "survivor_"):
        if cid.startswith(prefix):
            cid = cid[len(prefix):]
            break
    # Replace underscores with spaces and tag as a noun phrase
    words = cid.replace("_", " ").strip()
    if not words:
        return content_id
    return words


def build_prompt(manifest_row, prompt_data):
    """Combine the manifest's semantic description, the prompt composer's
    structured template, the per-content_id subject, and the global ASHFALL
    style suffix into a single text prompt suitable for image generation.

    The Gemini text-prompt interface accepts a single string; we concatenate
    the structured fields in reading order with the subject prepended.

    Phase 18 fixes: avoid numerical camera instructions (some leaked into
    outputs as on-image labels), spell the no-text rule three times in
    different phrasings, and drop the "cyan-green contamination cue" wording
    because the model interpreted it as a radioactive symbol on every image.
    """
    parts = []
    subject = derive_subject(manifest_row.get("content_id", ""),
                              manifest_row.get("visual_family", ""),
                              manifest_row.get("subfamily", ""))
    family = manifest_row.get("visual_family", "")
    parts.append("CRITICAL: ZERO on-image text, labels, watermarks, signs, badges, "
                 "callouts, captions, or numerical annotations. The image must "
                 "contain no readable characters whatsoever.")
    if family == "NPC-Portrait":
        parts.append(f"WHAT: A head-and-shoulders painted portrait of '{subject}'. "
                     f"Survivor of a nuclear winter. Charcoal pencil underdrawing visible. "
                     f"Subject fills the canvas, eye-level view, slight 3/4 turn. "
                     f"Window light from upper-left, deep shadow on the opposite side. "
                     f"Cheeks hollow, eyes tired. No makeup, no anime, no glamour. "
                     f"Occupation cue through clothing detail only — no badge, no text, no label.")
    elif family == "Survivor-Portrait":
        parts.append(f"WHAT: A head-and-shoulders painted portrait of a {subject} survivor. "
                     f"Charcoal pencil underdrawing, dry gouache texture. "
                     f"Subject fills the canvas, eye-level view, slight 3/4 turn. "
                     f"Window light from upper-left, deep shadow on the opposite side. "
                     f"No anime, no fantasy hair, no glamour makeup.")
    elif family == "Location-Art":
        parts.append(f"WHAT: A painted ASHFALL location plate of '{subject}'. "
                     f"Three-quarter view at human eye height, scene-readable silhouette landmark. "
                     f"Subject is the LOCATION named, not generic post-apocalyptic decor. "
                     f"Concrete, rust, ash, condensation, decay. Soft overcast daylight. "
                     f"Margins reserved for UI overlay.")
    elif family == "Faction-Art":
        parts.append(f"WHAT: A faction emblem for '{subject}'. Flat, frontal, square crop. "
                     f"Single icon, strong silhouette, 65% of canvas. Diffuse lighting, "
                     f"faded blue-grey, rust brown, dirty bone. Aged metal plate. "
                     f"No text, no logo, no AI signature.")
    elif family == "Inventory-Item":
        sub = manifest_row.get("subfamily", "Other")
        parts.append(f"WHAT: A painted 2D inventory icon of '{subject}'. "
                     f"Subfamily: {sub}. Single subject, centered, isolated. "
                     f"Eye-level, slight 3/4 turn. Background: transparent-to-charcoal gradient. "
                     f"Soft window light from upper-left. Worn, dusty, post-apocalyptic functional. "
                     f"Readable at 32x32 in silhouette.")
    else:
        parts.append(f"WHAT: A painted 2D ASHFALL asset of '{subject}'.")
    parts.append("COMPOSITION: single subject dominates. NO environmental scenery "
                 "outside the subject plane. NO FOREGROUND text overlays.")
    parts.append("DO NOT INCLUDE: signposts, billboards, painted signs, wooden signs, "
                 "metal plaques, paperwork, book covers, magazine covers, newspaper clippings, "
                 "lettered papers, license plates, certificates, post-it notes, stickers, "
                 "graffiti with letters, screens with text, logbooks opened to visible pages. "
                 "These objects have a near-100% chance of generating accidental text. "
                 "Replace them with bark, cobwebs, soot, rain streaks, faded paint, "
                 "blank stained paper, sticky notes with doodles only, blank ledgers.")
    parts.append("CAMERA: human eye level, slight 3/4 turn, no overhead drones, no "
                 "extreme wide-angle, no cinematic letterbox bars.")
    parts.append("LIGHTING: diff use overcast; single soft key from upper-left; "
                 "subtle ambient occlusion; no hardsun.")
    parts.append("PALETTE: charcoal #2A2A2C, concrete grey #5C5F62, faded blue-grey "
                 "#708085, rust brown #6E4A2F, dirty bone #B5A88A. Rare muted amber "
                 "#A26E2C only as natural light cue. NO cyan-green accent symbols, "
                 "no radioactive trefoils, no luminous icons.")
    parts.append("MATERIALS: brushed metal, raw wood, dented tin, dirty plastic, "
                 "oxidised copper, worn leather, weathered concrete, peeling paint, "
                 "exposed rebar, broken glass. No polish, no varnish, no chrome.")
    parts.append("BACKGROUND: simple, dim, atmospheric — NOT a busy scene. Concrete "
                 "tone or sky haze, no companion objects, no horizon line for portraits.")
    parts.append("STYLE: original 2D hand-painted ASHFALL survival-management artwork. "
                 "Charcoal pencil underdrawing. Dry gouache / worn painted texture. "
                 "Grounded grim realism. No neon cyberpunk, no glossy sci-fi, no fantasy "
                 "ornament, no cartoon, no anime, no oversaturation, no stock-photo "
                 "photorealism. No gratuitous gore, no weapon glamour.")
    parts.append("RUNTIME: must read at small panel sizes — subject silhouette must "
                 "remain identifiable when downscaled.")
    parts.append("ABSOLUTELY NO: text, letters, numerals, logos, brands, watermarks, "
                 "flags, AI signatures, radioactive symbols, sci-fi HUDs, multiplayer name "
                 "tags, killstreak banners, energy bars, level numbers, watermarks, "
                 "nuclear trefoils, contamination runes, faction crests with letters.")
    return "\n".join(parts)


def extract_s3url(result_obj):
    """Walk the Composio response shape and pull the s3url / asset_url."""
    if not isinstance(result_obj, dict):
        return None
    # Common shapes:
    #   data.image.s3url
    #   data.results[i].response.data.image.s3url
    #   data.image.url
    d = result_obj.get("data", {})
    if isinstance(d, dict):
        img = d.get("image")
        if isinstance(img, dict):
            for k in ("s3url", "url", "asset_url"):
                if k in img:
                    return img[k]
        # OPENAI shape: data.images[].asset_url
        imgs = d.get("images")
        if isinstance(imgs, list) and imgs:
            for k in ("asset_url", "url", "b64_json"):
                v = imgs[0].get(k)
                if v:
                    return v
        # Direct url
        for k in ("url", "asset_url", "s3url"):
            if k in d:
                return d[k]
    # Fallback: results shape
    results = result_obj.get("results")
    if isinstance(results, list) and results:
        first = results[0]
        if isinstance(first, dict):
            return extract_s3url(first)
    return None


def call_composio_generate(prompt, model="gemini-2.5-flash-image", aspect_ratio="1:1",
                           image_size="1K", output_dir="/tmp/phase17_staging",
                           label="phase18"):
    """Run Composio generate, return the saved local file path."""
    os.makedirs(output_dir, exist_ok=True)
    payload = json.dumps({
        "prompt": prompt,
        "model": model,
        "aspect_ratio": aspect_ratio,
        "image_size": image_size,
    })
    try:
        result = subprocess.run(
            ["composio", "execute", "GEMINI_GENERATE_IMAGE", "-d", payload],
            capture_output=True, text=True, timeout=600,
        )
        if result.returncode != 0:
            return None, f"composio exit {result.returncode}: {result.stderr[:200]}"
        out = result.stdout
        try:
            data = json.loads(out)
        except json.JSONDecodeError:
            return None, f"non-json response: {out[:200]}"
        if not data.get("successful"):
            return None, f"composio unsuccessful: {out[:200]}"
        url = extract_s3url(data)
        if not url:
            return None, f"no url in: {out[:200]}"
        # Download
        local_path = os.path.join(output_dir, f"{label}.png")
        urllib.request.urlretrieve(url, local_path)
        return local_path, None
    except subprocess.TimeoutExpired:
        return None, "composio timeout"
    except Exception as e:
        return None, f"composio exception: {e}"


def main():
    surfaced = load_surfaced_content_ids()
    if not surfaced:
        print("WARN: no surfaced content_ids found; batch will run on all P1 actionable.")
    rows = load_manifest_rows()
    actionable = [r for r in rows if r.get("generation_status") == "PENDING"]
    if surfaced:
        actionable = [r for r in actionable if r.get("content_id") in surfaced]
    # Sort by priority band then importance
    band_order = {"P0": 0, "P1": 1, "P2": 2, "P3": 3, "P4": 4}
    actionable.sort(key=lambda r: (band_order.get(r.get("runtime_priority", "P4"), 9),
                                    -r.get("gameplay_importance", 0.0),
                                    r.get("content_id", "")))
    # Take a batch size
    batch_size = int(os.environ.get("PHASE18_BATCH_SIZE", "6"))
    batch = actionable[:batch_size]
    print(f"Batch: {len(batch)} of {len(actionable)} surfaced actionable rows")

    # Build staging dirs
    for subdir in FAMILY_TO_SUBDIR.values():
        (STAGING / subdir).mkdir(parents=True, exist_ok=True)

    # Skip already-staged
    skipped = []
    todo = []
    for r in batch:
        sub = FAMILY_TO_SUBDIR.get(r["visual_family"], "items")
        target = STAGING / sub / f"{r['content_id']}.png"
        if target.exists():
            skipped.append((r["content_id"], "already staged"))
        else:
            todo.append(r)
    print(f"Skipped (already staged): {len(skipped)}")
    print(f"To do: {len(todo)}")

    # Generate
    results = []
    for r in todo:
        cid = r["content_id"]
        prompt_data = load_prompt_for(cid)
        prompt = build_prompt(r, prompt_data)
        family = r["visual_family"]
        sub = FAMILY_TO_SUBDIR.get(family, "items")
        # Phase 17 dimension policy
        if family in ("Survivor-Portrait", "NPC-Portrait"):
            aspect_ratio = "1:1"
            image_size = "1K"
        elif family == "Location-Art":
            aspect_ratio = "16:9"
            image_size = "1K"
        else:
            aspect_ratio = "1:1"
            image_size = "1K"
        print(f"  generating {cid} ({family}/{r['subfamily']}) aspect={aspect_ratio}")
        local_path, err = call_composio_generate(
            prompt=prompt,
            model="gemini-2.5-flash-image",
            aspect_ratio=aspect_ratio,
            image_size=image_size,
            output_dir="/tmp/phase17_staging",
            label=cid,
        )
        if err:
            print(f"    FAIL: {err}")
            results.append({"content_id": cid, "status": "FAIL", "error": err})
            continue
        # Move to staging
        target = STAGING / sub / f"{cid}.png"
        target.parent.mkdir(parents=True, exist_ok=True)
        import shutil
        shutil.copy2(local_path, target)
        os.remove(local_path)
        print(f"    OK: {target}")
        results.append({
            "content_id": cid,
            "family": family,
            "subfamily": r["subfamily"],
            "staged_path": str(target.relative_to(REPO)),
            "status": "STAGED",
            "model": "gemini-2.5-flash-image",
            "aspect_ratio": aspect_ratio,
            "image_size": image_size,
        })
        time.sleep(1)

    # Append to ledger
    if LEDGER.exists():
        ledger = json.loads(LEDGER.read_text())
    else:
        ledger = []
    for r in results:
        r["phase"] = "phase18"
        r["timestamp"] = time.time()
    ledger.extend(results)
    LEDGER.write_text(json.dumps(ledger, indent=1))

    # Summary
    print()
    print(f"=== Batch 1 (Composio/Gemini) ===")
    print(f"Requested: {len(batch)}")
    print(f"Skipped (already staged): {len(skipped)}")
    print(f"Generated: {sum(1 for r in results if r.get('status') == 'STAGED')}")
    print(f"Failed: {sum(1 for r in results if r.get('status') == 'FAIL')}")
    print(f"Staging dir: {STAGING}")
    return results


if __name__ == "__main__":
    main()
