#!/usr/bin/env python3
"""Phase 14F-G — Prompt composer.

For every row of PRODUCTION_ART_GENERATION_MANIFEST.json this composes a
structured generation prompt template with explicit WHAT/STYLE/COMPOSITION/
CAMERA/LIGHTING/PALETTE/MATERIALS/BACKGROUND/RUNTIME-READABILITY/NEGATIVE/
OUTPUT-FORMAT sections derived from the family / subfamily rules in
ART_FAMILY_REFERENCE_GUIDE.md.

Storage: docs/visual/generated_prompts/<content_id>.json (one file per target).

The composer is unit-only; actual generation is gated on a +gen endpoint
wiring. A subsequent pipeline run with the endpoint wired consumes these
prompt-JSON files directly.
"""
import json
from pathlib import Path
from collections import defaultdict

REPO = Path("/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War")
MANIFEST = json.load(open(REPO / "docs/visual/PRODUCTION_ART_GENERATION_MANIFEST.json"))

# Family / subfamily rule templates
PALETTE = (
    "charcoal #2A2A2C, concrete grey #5C5F62, faded blue-grey #708085, "
    "rust brown #6E4A2F, dirty bone #B5A88A. Accents limited to muted "
    "amber #A26E2C (1-2% of pixels) and subtle cyan-green #4F7461 only for "
    "contamination iconography."
)
NEGATIVE = (
    "no real-world text, letters, or numerals; no logos or watermark; "
    "no AI signature glyphs; no anime-style eyes outside portrait family; "
    "no fantasy ornament or fleurons; no neon cyberpunk glow borders; "
    "no glossy 3D chromatic rendering; no crystalline particles; "
    "no stock-photo backdrop; no pristine factory-fresh finish on tools "
    "or weapons; no gratuitous gore; no real-country flags; no real "
    "brand marks. No rendered UI text — that is authored in Godot."
)

FAMILY_RULES = {
    "Inventory-Item": {
        "WHAT_HINT": "A single photographed {subject} in centre frame, "
                     "occupying ~60% of the canvas.",
        "STYLE_HINT": "2D painted survival-management icon, dry-gouache / charcoal underlayer, ASHFALL palette, hand-painted texture brushwork, no outer dark outline, soft anti-aliased edges, low-rotation.",
        "COMPOSITION_HINT": "Single subject, no scene. Negative space surrounding. Composition must remain legible when the icon is downscaled to 32×32 and 64×64.",
        "CAMERA_HINT": "Eye-level, slight 3/4 turn. Never overhead. Never pointing at camera.",
        "LIGHTING_HINT": "Diffuse overcast with a single soft key light from upper-left ~30°. Subtle ambient occlusion in recesses.",
        "MATERIALS_HINT": "Materials must read as: brushed metal, raw wood, dented tin, dirty plastic, oxidised copper, worn leather. No polish, no varnish, no chrome.",
        "BACKGROUND_HINT": "Soft transparent-to-charcoal gradient. No setting, no horizon, no companion objects.",
        "RUNTIME_HINT": "This asset renders at 64×64 inside inventory slots. Subject must be recognisable in silhouette at 32×32.",
        "OUTPUT_FORMAT_HINT": "{w}x{h} PNG with alpha channel (RGBA). JPG fallback allowed only when alpha is not required.",
    },
    "Survivor-Portrait": {
        "WHAT_HINT": "Head-and-shoulders portrait of a single human survivor "
                     "in worn cold-weather post-apocalyptic clothing.",
        "STYLE_HINT": "2D painted survival-portrait, dry-gouache / charcoal. ASHFALL palette. Same scaling and brushwork as inventory items.",
        "COMPOSITION_HINT": "Subject centred, ~60-70% of canvas. Slightly weathered face, slight off-camera gaze.",
        "CAMERA_HINT": "Eye-level, heads-on. Three-quarter turn optional. Focal length equivalent ~85mm.",
        "LIGHTING_HINT": "Soft single-source key light from upper-left ~30°, ~85% intensity. Cool rim light from upper-right. No dramatic chiaroscuro.",
        "MATERIALS_HINT": "Skin naturalistically weathered, no makeup, no glamorisation. Hair charcoal / dark brown / grey. Clothing: charcoal, faded blue-grey, rust brown leather / canvas.",
        "BACKGROUND_HINT": "Soft blurred bunker or forward-station environment. Out-of-focus concrete grey walls, with subtle haze. Not studio backdrop.",
        "RUNTIME_HINT": "Reads at 128×128 survivor roster card. Silhouette retains character at 64×64.",
        "OUTPUT_FORMAT_HINT": "{w}x{h} JPG / PNG opaque.",
    },
    "NPC-Portrait": {
        "WHAT_HINT": "Head-and-shoulders portrait of a single named NPC. Same composition rules as Survivor Portrait but more interior-scene context allowed.",
        "STYLE_HINT": "2D painted survival-portrait. ASHFALL palette. Continuity with Survivor-Portrait, but slightly broader expressive range permitted.",
        "COMPOSITION_HINT": "Same as Survivor Portrait but background may carry a doorway or wall fragment.",
        "CAMERA_HINT": "Eye-level, heads-on. Focal length equivalent ~85mm.",
        "LIGHTING_HINT": "Same as Survivor Portrait, may carry a slightly warmer key light indicating warmth source (candle, low fire).",
        "MATERIALS_HINT": "Same as Survivor Portrait.",
        "BACKGROUND_HINT": "Slight scene context (doorway, wall, frost-rimed doorway). Soft-bokeh beyond depth-of-field.",
        "RUNTIME_HINT": "Reads at 128×128 NPC roster card. Silhouette retains character at 64×64.",
        "OUTPUT_FORMAT_HINT": "{w}x{h} JPG / PNG opaque.",
    },
    "Location-Art": {
        "WHAT_HINT": "A location art plate of a post-apocalyptic interior / exterior. Gameplay-functional, NOT a concept-art poster.",
        "STYLE_HINT": "2D painted survival environments, dry-gouache. ASHFALL palette. Architectural geometry first, decoration second.",
        "COMPOSITION_HINT": "Architectural layout readable at a glance. ~15% margin at top + bottom for UI overlay. Negative space preserved.",
        "CAMERA_HINT": "Eye-level human-equivalent (~130 cm). NEVER aerial/drone. NEVER extreme wide-angle.",
        "LIGHTING_HINT": "Diffuse overcast by default. Strong directional only with documented reason. Soft shadow.",
        "MATERIALS_HINT": "Concrete grey walls, scrubby bent metal, rusted I-beams, frosted-pane windows, condensation drips. Functional decay.",
        "BACKGROUND_HINT": "Limited depth — the location is legible in foreground / mid / background. No endless horizon.",
        "RUNTIME_HINT": "16:9 crop usable for expedition panel. Reads at 512×256 in-game. Negative-space margins preserved.",
        "OUTPUT_FORMAT_HINT": "{w}x{h} JPG opaque.",
    },
    "Faction-Art": {
        "WHAT_HINT": "A monochrome-style faction emblem. No characters, no setting. Pure iconography.",
        "STYLE_HINT": "Single-colour emblem against a charcoal background. ASHFALL palette. Limited accent.",
        "COMPOSITION_HINT": "Emblem symmetrically placed in a square canvas.",
        "CAMERA_HINT": "N/A. Flat iconography.",
        "LIGHTING_HINT": "N/A. Flat emblem.",
        "MATERIALS_HINT": "N/A. Flat.",
        "BACKGROUND_HINT": "Charcoal or near-black background. No horizon.",
        "RUNTIME_HINT": "Reads at 64×64 roster chip. Silhouette retains meaning at 32×32.",
        "OUTPUT_FORMAT_HINT": "{w}x{h} JPG / PNG. Optional alpha.",
    },
}

WORD_BANK = {
    # Light semantic hints per ID token
    "weapon": ["weapon", "firearm", "melee", "edge"],
    "pistol": ["handgun", "sidearm", "compact pistol"],
    "rifle": ["long arm", "full-length rifle", "carbine"],
    "shotgun": ["scatter gun", "pump-action shotgun"],
    "ammo": ["cartridge", "round", "ammunition"],
    "med": ["medical satchel", "trauma pouch", "first-aid accessory"],
    "bandage": ["sterile dressing", "rolled cloth bandage"],
    "iodine": ["iodine tablet bottle"],
    "morphine": ["syrette", "morphine syrette"],
    "syringe": ["hypodermic syringe", "auto-injector"],
    "splint": ["rigid splint", "padded board splint"],
    "suture": ["suture kit", "thread-and-needle kit"],
    "crafting": ["hand-tool component", "fabrication spare"],
    "scrap": ["salvaged scrap piece", "offcut of industrial metal"],
    "electronic": ["electronic part", "soldered component"],
    "metal": ["metal offcut", "scrap metal fragment"],
    "weapon_": ["weapon", "tactical armament"],
    "ammo_": ["ammunition round"],
    "loot": ["contained salvage"],
    "foundry": ["foundry input", "smelter feedstock"],
    "faction_": ["faction emblem motif", "ideographic mark"],
    "survivor_": ["survivor", "vault dweller"],
    "npc_": ["named NPC", "faction-affiliated stranger"],
    "loc_": ["location", "site"],
    "food": ["ration pack", "preserved food unit"],
    "water": ["water container", "fresh water store"],
    "canned": ["tin can", "hermetically sealed food can"],
    "ration": ["ration packet"],
    "mre": ["military ration pack"],
    "dried": ["dried foodstuffs"],
    "bottle": ["bottle", "sealed container"],
    "scavenged": ["salvaged", "scavenged"],
    "boots": ["boots", "leather boots"],
    "vest": ["tactical vest", "ballistic vest"],
    "mask": ["gas mask", "respirator"],
    "weapon_pipe": ["scratch-built rifle", "improvised pipe firearm"],
    "weapon_revolver": ["revolver", "wheelgun"],
    "weapon_ak": ["AK-pattern rifle"],
    "rifle_ak": ["AK-pattern rifle"],
    "rifle_m4": ["M4-pattern carbine"],
    "shotgun_pump": ["pump-action shotgun"],
    "smg": ["submachine gun", "compact automatic"],
    "lmg": ["light machine gun", "belt-fed automatic"],
}


def subject_hint(content_id: str, family: str, sub: str) -> str:
    """Best-effort one-line subject description based on token bag."""
    cid = content_id.lower()
    hints = []
    for token, replacements in WORD_BANK.items():
        if token in cid:
            hints.append(replacements[0])
            break
    if not hints:
        # Generic subject
        if family == "Inventory-Item":
            hints.append(f"post-apocalyptic {sub.lower()} item")
        elif family in ("Survivor-Portrait", "NPC-Portrait"):
            hints.append("a single human figure")
        elif family == "Location-Art":
            hints.append("a single post-apocalyptic location interior/exterior")
        elif family == "Faction-Art":
            hints.append("a single faction emblem motif")
        else:
            hints.append("a single representative subject")
    return hints[0]


def compose_prompt(row):
    family = row["visual_family"]
    sub = row["subfamily"]
    rules = FAMILY_RULES.get(family, FAMILY_RULES["Inventory-Item"])
    w = row["target_width"]
    h = row["target_height"]
    subject = subject_hint(row["content_id"], family, sub)
    return {
        "content_id": row["content_id"],
        "visual_family": family,
        "subfamily": sub,
        "model_hint": row.get("model_hint", "auto"),
        "prompt_template": {
            "WHAT": rules["WHAT_HINT"].format(subject=subject),
            "STYLE": rules["STYLE_HINT"],
            "COMPOSITION": rules["COMPOSITION_HINT"],
            "CAMERA": rules["CAMERA_HINT"],
            "LIGHTING": rules["LIGHTING_HINT"],
            "PALETTE": PALETTE,
            "MATERIALS": rules["MATERIALS_HINT"],
            "BACKGROUND": rules["BACKGROUND_HINT"],
            "RUNTIME_READABILITY": rules["RUNTIME_HINT"],
            "NEGATIVE": NEGATIVE,
            "OUTPUT_FORMAT": rules["OUTPUT_FORMAT_HINT"].format(w=w, h=h),
        },
        "meta": {
            "source_catalog": row["source_catalog"],
            "reference_assets": row["reference_assets"],
            "canonical_filename": row["target_filename"],
            "target_directory": row["target_directory"],
            "alpha_requirement": row["alpha_requirement"],
            "dimensions": [w, h],
            "aspect_ratio": row["aspect_ratio"],
            "priority_band": row["runtime_priority"],
            "gameplay_importance": row["gameplay_importance"],
        },
        "semantic_description": row["semantic_description"],
        "forbidden_objects": row["forbidden_objects"],
    }


out_dir = REPO / "docs/visual/generated_prompts"
out_dir.mkdir(exist_ok=True, parents=True)

prompts_dir = out_dir
manifest_dir = out_dir / "_manifest"
manifest_dir.mkdir(exist_ok=True)

written = 0
for row in MANIFEST:
    if row.get("generation_status") == "SKIP_REFERENCE_ONLY":
        continue
    prompt = compose_prompt(row)
    fp = prompts_dir / f"{row['content_id']}.json"
    fp.write_text(json.dumps(prompt, indent=1))
    written += 1

# Aggregate manifest
agg = {r["content_id"]: compose_prompt(r) for r in MANIFEST
       if r.get("generation_status") != "SKIP_REFERENCE_ONLY"}
manifest_dir.joinpath("prompts.json").write_text(json.dumps(agg, indent=1))
print(f"→ wrote {written} prompt files to docs/visual/generated_prompts/")
print(f"→ wrote aggregate manifest (prompts.json)")
