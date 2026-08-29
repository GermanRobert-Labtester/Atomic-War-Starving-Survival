#!/usr/bin/env python3
"""
generate-audio-catalog.py — Audio Cue Architecture Catalog Generator & Drift Gate

Extracts all audio cue definitions, bus assignments, and resource mappings
from src/Audio/AudioCueCatalog.cs and generates docs/audio/AUDIO_CUE_CATALOG.md.

Usage:
  python3 scripts/ci/generate-audio-catalog.py          # Regenerates docs/audio/AUDIO_CUE_CATALOG.md
  python3 scripts/ci/generate-audio-catalog.py --check  # Verifies 0 drift in CI
"""

import re
import sys
import pathlib
from datetime import datetime, timezone

REPO_ROOT = pathlib.Path(__file__).resolve().parent.parent.parent
SOURCE_FILE = REPO_ROOT / "src" / "Audio" / "AudioCueCatalog.cs"
OUTPUT_FILE = REPO_ROOT / "docs" / "audio" / "AUDIO_CUE_CATALOG.md"


def parse_audio_cues():
    if not SOURCE_FILE.is_file():
        print(f"Error: {SOURCE_FILE} not found.", file=sys.stderr)
        sys.exit(1)

    content = SOURCE_FILE.read_text(encoding="utf-8")

    # Match Reg(Id, "res://...", Bus, ...)
    # Reg(UiClick, "res://assets/audio/ui/ui_click.wav", AudioBusNames.Ui, cooldown: 0.05f);
    pattern = re.compile(
        r'Reg\(\s*(?P<id_var>\w+)\s*,\s*"(?P<path>[^"]+)"\s*,\s*(?:AudioBusNames\.)?(?P<bus>\w+)(?P<extras>[^)]*)\);'
    )

    # Match constant definitions to resolve variable names to string values
    const_pattern = re.compile(r'public\s+const\s+string\s+(?P<var>\w+)\s*=\s*"(?P<val>[^"]+)";')
    id_map = {}
    for match in const_pattern.finditer(content):
        id_map[match.group("var")] = match.group("val")

    cues = []
    for match in pattern.finditer(content):
        id_var = match.group("id_var")
        cue_id = id_map.get(id_var, id_var)
        res_path = match.group("path")
        bus = match.group("bus")
        extras = match.group("extras")

        loop = "loop: true" in extras
        vol_match = re.search(r'vol:\s*(-?[\d\.]+)f?', extras)
        vol = f"{vol_match.group(1)} dB" if vol_match else "0 dB"
        cd_match = re.search(r'cooldown:\s*([\d\.]+)f?', extras)
        cooldown = f"{cd_match.group(1)}s" if cd_match else "—"

        # Check local asset file resolution
        local_rel = res_path.replace("res://", "")
        local_path = REPO_ROOT / local_rel
        resolved = local_path.is_file()

        cues.append({
            "id": cue_id,
            "var": id_var,
            "res_path": res_path,
            "bus": bus,
            "loop": "Yes" if loop else "No",
            "vol": vol,
            "cooldown": cooldown,
            "resolved": "✅ Exists" if resolved else "⚠️ Fallback/Missing"
        })

    return cues


def generate_catalog_markdown(cues) -> str:
    lines = [
        "# ASHFALL Audio Cue Architecture Catalog",
        "",
        "> **Living Architecture Authority**: Documents all registered audio cues, target Godot audio buses, asset resource paths, loop behavior, volume trim, and cooldown timers in `src/Audio/AudioCueCatalog.cs`.",
        "",
        f"**Total Registered Cues:** `{len(cues)}`<br>",
        f"**Last Verified:** `{datetime.now(timezone.utc).strftime('%Y-%m-%d')}`<br>",
        "**Drift Gated:** `python3 scripts/ci/generate-audio-catalog.py --check`",
        "",
        "---",
        "",
        "## 1. Audio Bus Architecture Overview",
        "",
        "ASHFALL organizes sound design into 12 dedicated audio buses with independent volume controls and sidechain compression:",
        "",
        "| Audio Bus | Purpose | Default Route |",
        "|---|---|---|",
        "| `Master` | Main audio output and final limiting | Hardware Out |",
        "| `Music` | Dynamic score, title theme, exploration underscore | Master |",
        "| `Ambience` | Bunker ventilation hum, wind, weather loop | Master |",
        "| `SFX` | Environmental interactions, explosions, physical items | Master |",
        "| `UI` | Tactile interface clicks, tab switching, confirmations | Master |",
        "| `Voice` | Radio chatter, distress calls, narrator cues | Master |",
        "| `Alerts` | Critical radiation alarms, crisis sirens, warning klaxons | Master |",
        "| `Generator` | Shelter generator rumble and fuel burn | Ambience |",
        "| `Ventilation` | Air intake fan rotation, filter scrubbers | Ambience |",
        "| `Radio` | Tuner static, signal locks, Morse broadcasts | Voice |",
        "| `Medical` | Heartbeat pulse, trauma monitor, resuscitation | Alerts |",
        "| `Surface` | Wasteland dust storms, exterior wind howling | Ambience |",
        "",
        "---",
        "",
        "## 2. Master Audio Cue Register",
        "",
        "| Cue ID | Target Bus | Resource Path | Loop | Volume Trim | Cooldown | Asset Status |",
        "|---|---|---|---|---|---|---|",
    ]

    for c in sorted(cues, key=lambda x: (x["bus"], x["id"])):
        lines.append(
            f"| `{c['id']}` | `{c['bus']}` | `{c['res_path']}` | {c['loop']} | {c['vol']} | {c['cooldown']} | {c['resolved']} |"
        )

    lines.append("")
    lines.append("---")
    lines.append("")
    lines.append("## 3. Cue Playback Integration Protocol")
    lines.append("")
    lines.append("```csharp")
    lines.append("// Canonical playback in Godot Host views and presentation nodes:")
    lines.append("AudioManager.Instance.PlayCue(AudioCueCatalog.UiClick);")
    lines.append("AudioManager.Instance.PlayCue(AudioCueCatalog.RadGeigerBurst, pitchScale: 1.1f);")
    lines.append("```")

    text = "\n".join(lines).rstrip() + "\n"
    return text


def main():
    check_mode = "--check" in sys.argv
    cues = parse_audio_cues()
    generated_md = generate_catalog_markdown(cues)

    if check_mode:
        if not OUTPUT_FILE.is_file():
            print(f"FAIL: {OUTPUT_FILE} does not exist. Run python3 scripts/ci/generate-audio-catalog.py", file=sys.stderr)
            sys.exit(1)
        current_md = OUTPUT_FILE.read_text(encoding="utf-8")
        if current_md.strip() != generated_md.strip():
            print(f"FAIL: {OUTPUT_FILE} is out of date. Run python3 scripts/ci/generate-audio-catalog.py", file=sys.stderr)
            sys.exit(1)
        print(f"OK: {OUTPUT_FILE} is in sync with AudioCueCatalog.cs ({len(cues)} cues).")
        sys.exit(0)

    OUTPUT_FILE.parent.mkdir(parents=True, exist_ok=True)
    OUTPUT_FILE.write_text(generated_md, encoding="utf-8")
    print(f"Wrote {OUTPUT_FILE} ({len(cues)} audio cues cataloged).")


if __name__ == "__main__":
    main()
