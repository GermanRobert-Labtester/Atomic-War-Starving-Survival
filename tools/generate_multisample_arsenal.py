#!/usr/bin/env python3
"""
tools/generate_multisample_arsenal.py — Generates 5-sample multi-sample pools for
gunshots, distance shots, explosions, interactions, and material footsteps (including granite).

Takes core ElevenLabs source files, derives 5 authentic acoustic variations per cue,
enforcing true peak <= -1.5 dBFS, and registers resource_paths pools.
"""

import json
import math
import os
import pathlib
import struct
import subprocess
import sys

REPO_ROOT = pathlib.Path(__file__).resolve().parent.parent
sys.path.insert(0, str(REPO_ROOT / "tools"))

from audio_pipeline import (
    AudioExporter,
    AudioMeasurer,
    PRESET_SFX,
    PRESET_UI,
    PRESET_AMBIENCE,
    DeliveryPreset
)

SAMPLE_RATE = 44100

def decode_mp3(filepath: pathlib.Path) -> list[float]:
    cmd = [
        "ffmpeg", "-y", "-i", str(filepath),
        "-ar", str(SAMPLE_RATE), "-ac", "1", "-f", "f32le", "-"
    ]
    proc = subprocess.run(cmd, capture_output=True, check=True)
    return [val[0] for val in struct.iter_unpack("<f", proc.stdout)]

def resample_pitch(samples: list[float], factor: float) -> list[float]:
    """Simple linear interpolation pitch shift."""
    if abs(factor - 1.0) < 0.0001:
        return list(samples)
    out_len = int(len(samples) / factor)
    res = [0.0] * out_len
    for i in range(out_len):
        src_idx = i * factor
        idx0 = int(src_idx)
        idx1 = min(idx0 + 1, len(samples) - 1)
        frac = src_idx - idx0
        res[i] = samples[idx0] * (1.0 - frac) + samples[idx1] * frac
    return res

def apply_eq(samples: list[float], hp_alpha: float = 0.0, lp_alpha: float = 1.0) -> list[float]:
    """Applies basic high-pass and low-pass filtering."""
    out = [0.0] * len(samples)
    lp_state = 0.0
    prev_in = 0.0
    hp_state = 0.0
    for i, s in enumerate(samples):
        # Low pass
        lp_state += lp_alpha * (s - lp_state)
        # High pass
        hp_state = hp_alpha * (hp_state + s - prev_in)
        prev_in = s
        out[i] = lp_state + hp_state
    return out

def add_slapback(samples: list[float], delay_ms: float = 32.0, decay: float = 0.35) -> list[float]:
    """Adds subtle room slapback reflection."""
    delay_samples = int(SAMPLE_RATE * (delay_ms / 1000.0))
    out = list(samples) + [0.0] * delay_samples
    for i in range(len(samples)):
        out[i + delay_samples] += samples[i] * decay
    return out

def generate_5_variants(base_samples: list[float]) -> list[list[float]]:
    """Generates exactly 5 acoustic variants from one base sound."""
    variants = []

    # Variant 1: Base master (unaltered)
    variants.append(list(base_samples))

    # Variant 2: Bright Transient (+2.5% pitch, slight high-shelf)
    v2 = resample_pitch(base_samples, 1.025)
    # boost transients
    v2 = [s * 1.05 if i < int(SAMPLE_RATE * 0.05) else s for i, s in enumerate(v2)]
    variants.append(v2)

    # Variant 3: Resonant Low-Mid Body (-3% pitch, warm low-end)
    v3 = resample_pitch(base_samples, 0.970)
    lp = 0.0
    for i in range(len(v3)):
        lp += 0.15 * (v3[i] - lp)
        v3[i] += lp * 0.4
    variants.append(v3)

    # Variant 4: Heavy Punch (saturation & compression)
    v4 = [math.tanh(s * 1.35) * 0.9 for s in base_samples]
    variants.append(v4)

    # Variant 5: Slapback / Room reflection
    v5 = add_slapback(base_samples, delay_ms=28.0, decay=0.30)
    variants.append(v5)

    return variants

# Configuration of families to generate
FAMILIES = [
    # ── Gunshots Arsenal ─────────────────────────────────────
    {
        "cue_id": "sfx_weapon_cz75_report",
        "source": pathlib.Path("/tmp/sfx_sharp_20260903_133101.mp3"),
        "dest_dir": REPO_ROOT / "assets/audio/sfx",
        "base_name": "sfx_weapon_cz75_report",
        "preset": PRESET_SFX,
        "bus": "SFX",
        "loop": False,
        "default_volume_db": -2.0
    },
    {
        "cue_id": "sfx_weapon_pipe_rifle_report",
        "source": pathlib.Path("/tmp/sfx_impro_20260903_133106.mp3"),
        "dest_dir": REPO_ROOT / "assets/audio/sfx",
        "base_name": "sfx_weapon_pipe_rifle_report",
        "preset": PRESET_SFX,
        "bus": "SFX",
        "loop": False,
        "default_volume_db": -2.0
    },
    {
        "cue_id": "sfx_weapon_scrap_shotgun_report",
        "source": pathlib.Path("/tmp/sfx_heavy_20260903_133110.mp3"),
        "dest_dir": REPO_ROOT / "assets/audio/sfx",
        "base_name": "sfx_weapon_scrap_shotgun_report",
        "preset": PRESET_SFX,
        "bus": "SFX",
        "loop": False,
        "default_volume_db": -1.0
    },
    {
        "cue_id": "sfx_weapon_bolt_rifle_report",
        "source": pathlib.Path("/tmp/sfx_loud__20260903_133115.mp3"),
        "dest_dir": REPO_ROOT / "assets/audio/sfx",
        "base_name": "sfx_weapon_bolt_rifle_report",
        "preset": PRESET_SFX,
        "bus": "SFX",
        "loop": False,
        "default_volume_db": -1.5
    },
    {
        "cue_id": "sfx_weapon_assault_rifle_burst",
        "source": pathlib.Path("/tmp/sfx_5.56__20260903_133120.mp3"),
        "dest_dir": REPO_ROOT / "assets/audio/sfx",
        "base_name": "sfx_weapon_assault_rifle_burst",
        "preset": PRESET_SFX,
        "bus": "SFX",
        "loop": False,
        "default_volume_db": -2.0
    },
    {
        "cue_id": "sfx_weapon_sniper_heavy_report",
        "source": pathlib.Path("/tmp/sfx_massi_20260903_133124.mp3"),
        "dest_dir": REPO_ROOT / "assets/audio/sfx",
        "base_name": "sfx_weapon_sniper_heavy_report",
        "preset": PRESET_SFX,
        "bus": "SFX",
        "loop": False,
        "default_volume_db": 0.0
    },
    # ── Distance & Explosions ────────────────────────────────
    {
        "cue_id": "sfx_distant_artillery_barrage",
        "source": pathlib.Path("/tmp/sfx_dista_20260903_133130.mp3"),
        "dest_dir": REPO_ROOT / "assets/audio/sfx",
        "base_name": "sfx_distant_artillery_barrage",
        "preset": PRESET_SFX,
        "bus": "SFX",
        "loop": False,
        "default_volume_db": -3.0
    },
    {
        "cue_id": "sfx_distant_gunfire_skirmish",
        "source": pathlib.Path("/tmp/sfx_dista_20260903_133134.mp3"),
        "dest_dir": REPO_ROOT / "assets/audio/sfx",
        "base_name": "sfx_distant_gunfire_skirmish",
        "preset": PRESET_SFX,
        "bus": "SFX",
        "loop": False,
        "default_volume_db": -4.0
    },
    {
        "cue_id": "danger_explosion",
        "source": pathlib.Path("/tmp/sfx_massi_20260903_133138.mp3"),
        "dest_dir": REPO_ROOT / "assets/audio/sfx",
        "base_name": "sfx_danger_explosion",
        "preset": PRESET_SFX,
        "bus": "SFX",
        "loop": False,
        "default_volume_db": 0.0
    },
    # ── Material-Specific Footsteps (Granite, Metal, Dirt, Glass, Wood) ─────
    {
        "cue_id": "footstep_granite",
        "source": pathlib.Path("/tmp/sfx_comba_20260903_133143.mp3"),
        "dest_dir": REPO_ROOT / "assets/audio/sfx",
        "base_name": "sfx_footstep_granite",
        "preset": PRESET_SFX,
        "bus": "SFX",
        "loop": False,
        "default_volume_db": -6.0
    },
    {
        "cue_id": "footstep_metal",
        "source": pathlib.Path("/tmp/sfx_comba_20260903_133150.mp3"),
        "dest_dir": REPO_ROOT / "assets/audio/sfx",
        "base_name": "sfx_footstep_metal",
        "preset": PRESET_SFX,
        "bus": "SFX",
        "loop": False,
        "default_volume_db": -6.0
    },
    {
        "cue_id": "footstep_dirt",
        "source": pathlib.Path("/tmp/sfx_comba_20260903_133154.mp3"),
        "dest_dir": REPO_ROOT / "assets/audio/sfx",
        "base_name": "sfx_footstep_dirt",
        "preset": PRESET_SFX,
        "bus": "SFX",
        "loop": False,
        "default_volume_db": -6.0
    },
    {
        "cue_id": "footstep_glass",
        "source": pathlib.Path("/tmp/sfx_comba_20260903_133159.mp3"),
        "dest_dir": REPO_ROOT / "assets/audio/sfx",
        "base_name": "sfx_footstep_glass",
        "preset": PRESET_SFX,
        "bus": "SFX",
        "loop": False,
        "default_volume_db": -5.0
    },
    {
        "cue_id": "footstep_wood",
        "source": pathlib.Path("/tmp/sfx_heavy_20260903_133203.mp3"),
        "dest_dir": REPO_ROOT / "assets/audio/sfx",
        "base_name": "sfx_footstep_wood",
        "preset": PRESET_SFX,
        "bus": "SFX",
        "loop": False,
        "default_volume_db": -6.0
    },
    # ── Location Ambience: Granite Quarry / Deep Cavern ───────
    {
        "cue_id": "amb_loc_granite_quarry",
        "source": pathlib.Path("/tmp/sfx_deep__20260903_133207.mp3"),
        "dest_dir": REPO_ROOT / "assets/audio/ambience",
        "base_name": "amb_location_granite_quarry",
        "preset": PRESET_AMBIENCE,
        "bus": "Ambience",
        "loop": True,
        "default_volume_db": -8.0
    },
    # ── Item Interactions ───────────────────────────────────
    {
        "cue_id": "action_item_pickup",
        "source": pathlib.Path("/tmp/sfx_quick_20260903_133216.mp3"),
        "dest_dir": REPO_ROOT / "assets/audio/sfx",
        "base_name": "sfx_action_item_pickup",
        "preset": PRESET_SFX,
        "bus": "SFX",
        "loop": False,
        "default_volume_db": -4.0
    },
    {
        "cue_id": "item_handling_ammo",
        "source": pathlib.Path("/tmp/sfx_brass_20260903_133220.mp3"),
        "dest_dir": REPO_ROOT / "assets/audio/sfx",
        "base_name": "sfx_item_handling_ammo",
        "preset": PRESET_SFX,
        "bus": "SFX",
        "loop": False,
        "default_volume_db": -4.0
    },
    {
        "cue_id": "item_handling_meds",
        "source": pathlib.Path("/tmp/sfx_plast_20260903_133235.mp3"),
        "dest_dir": REPO_ROOT / "assets/audio/sfx",
        "base_name": "sfx_item_handling_meds",
        "preset": PRESET_SFX,
        "bus": "SFX",
        "loop": False,
        "default_volume_db": -4.0
    },
    {
        "cue_id": "item_handling_ration",
        "source": pathlib.Path("/tmp/sfx_crink_20260903_133239.mp3"),
        "dest_dir": REPO_ROOT / "assets/audio/sfx",
        "base_name": "sfx_item_handling_ration",
        "preset": PRESET_SFX,
        "bus": "SFX",
        "loop": False,
        "default_volume_db": -4.0
    }
]

def main():
    print("=== Generating 5-Sample Multi-Sample Pools from ElevenLabs Masters ===")

    # Load audio_cues.json
    cues_json_path = REPO_ROOT / "Assets/StreamingAssets/Data/audio_cues.json"
    with open(cues_json_path, "r", encoding="utf-8") as f:
        cues_data = json.load(f)

    cues_map = {c["id"]: c for c in cues_data.get("cues", [])}
    generated_files = []

    for item in FAMILIES:
        cid = item["cue_id"]
        src = item["source"]
        dest_dir = item["dest_dir"]
        base_name = item["base_name"]
        preset = item["preset"]
        dest_dir.mkdir(parents=True, exist_ok=True)

        if not src.exists():
            print(f"  [SKIP] Source missing: {src}")
            continue

        base_samples = decode_mp3(src)
        variants = generate_5_variants(base_samples)

        resource_paths = []
        for idx, var_samples in enumerate(variants, 1):
            out_name = f"{base_name}_{idx:02d}.wav"
            out_file = dest_dir / out_name
            AudioExporter.export_wav(out_file, var_samples, preset)
            m = AudioMeasurer.measure(out_file)
            res_path = f"res://{out_file.relative_to(REPO_ROOT)}"
            resource_paths.append(res_path)
            generated_files.append(out_file)
            print(f"    -> {out_name}: {m['integrated_lufs']:.1f} LUFS, Peak {m['true_peak_dbfs']:.2f} dBFS")

        # Update or add to audio_cues.json
        if cid in cues_map:
            cues_map[cid]["resource_path"] = resource_paths[0]
            cues_map[cid]["resource_paths"] = resource_paths
            print(f"  [UPDATED CUE] {cid}: {len(resource_paths)} variations")
        else:
            new_cue = {
                "id": cid,
                "resource_path": resource_paths[0],
                "resource_paths": resource_paths,
                "bus": item["bus"],
                "loop": item["loop"],
                "default_volume_db": item["default_volume_db"],
                "volume_jitter_db": 0.8,
                "pitch_min": 0.96,
                "pitch_max": 1.04,
                "max_instances": 4,
                "priority": 50,
                "cooldown_seconds": 0.05,
                "fade_in_seconds": 0.0,
                "fade_out_seconds": 0.0,
                "fallback_cue_id": None
            }
            cues_data["cues"].append(new_cue)
            cues_map[cid] = new_cue
            print(f"  [ADDED NEW CUE] {cid}: {len(resource_paths)} variations")

    # Save updated audio_cues.json
    with open(cues_json_path, "w", encoding="utf-8") as f:
        json.dump(cues_data, f, indent=2)
    print(f"\nSaved {cues_json_path.name} with updated multi-sample pools.")
    print(f"Total files generated: {len(generated_files)}")

if __name__ == '__main__':
    main()
