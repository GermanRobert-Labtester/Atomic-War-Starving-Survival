#!/usr/bin/env python3
"""Generate the seven original, diegetic radio clips for Audio Plan 07B.

The script is deliberately no-overwrite. It uses the local eSpeak NG voice only
and then applies a narrow-band receiver treatment with ffmpeg. Runtime assets
are written only when --write-runtime is supplied.
"""

from __future__ import annotations

import argparse
import shutil
import subprocess
import tempfile
from pathlib import Path


ROOT = Path(__file__).resolve().parents[1]
OUTPUT_DIR = ROOT / "assets" / "audio" / "radio"
CLIPS = {
    "vo_verdict_meter.wav": "Machine register. Meter reads eleven forty-two. The count remains open.",
    "vo_verdict_eden.wav": "Eden was here. Record the weather. Do not erase the witness.",
    "vo_verdict_count.wav": "Office of Censuses. The count is open. Present every person in your care.",
    "vo_verdict_geophone.wav": "Geophone array one. Tap. Tap. Tap. The ground remembers the walkers.",
    "vo_verdict_reckoning.wav": "The Office of Censuses is convening. Hold the carrier for signature.",
    "vo_kind_hatch_relay.wav": "Hatch relay. Seal your intake. The carrier is still live.",
    "vo_kind_parley_beacon.wav": "Holdfast calling. Beacon active. Courier terms accepted.",
}


def run(command: list[str]) -> None:
    subprocess.run(command, check=True)


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--write-runtime", action="store_true",
                        help="write the reviewed WAV assets to assets/audio/radio")
    args = parser.parse_args()

    if not args.write_runtime:
        print("Dry run. Pass --write-runtime to create:")
        for filename in CLIPS:
            print(f"  {OUTPUT_DIR / filename}")
        return 0

    for command in ("espeak-ng", "ffmpeg"):
        if shutil.which(command) is None:
            raise SystemExit(f"Required command not found: {command}")

    OUTPUT_DIR.mkdir(parents=True, exist_ok=True)
    pending = [(filename, text) for filename, text in CLIPS.items()
               if not (OUTPUT_DIR / filename).exists()]
    if not pending:
        print("All requested runtime assets already exist; refusing to overwrite them.")
        return 0

    with tempfile.TemporaryDirectory(prefix="ashfall-radio-vo-") as temp_dir:
        temp_root = Path(temp_dir)
        for filename, text in pending:
            raw = temp_root / filename
            output = OUTPUT_DIR / filename
            run([
                "espeak-ng", "-v", "en-us", "-s", "135", "-p", "34", "-a", "118",
                "-w", str(raw), text,
            ])
            run([
                "ffmpeg", "-hide_banner", "-loglevel", "error", "-y", "-i", str(raw),
                "-af", "highpass=f=180,lowpass=f=4000,acompressor=threshold=-18dB:ratio=3:attack=10:release=80,loudnorm=I=-16:TP=-2:LRA=7",
                "-ar", "44100", "-ac", "1", "-c:a", "pcm_s16le", str(output),
            ])
            print(f"created {output.relative_to(ROOT)}")

    return 0


if __name__ == "__main__":
    raise SystemExit(main())
