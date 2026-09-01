#!/usr/bin/env python3
"""Generate original, non-vocal disease-transition SFX with local SoX only."""

from pathlib import Path
import subprocess
import sys


ROOT = Path(__file__).resolve().parent.parent
OUTPUTS = {
    "sfx_med_quarantine_seal.wav": [
        "synth", "0.85", "sine", "132-58", "gain", "-13",
        "fade", "q", "0.008", "0.85", "0.22",
    ],
    "sfx_med_quarantine_clear.wav": [
        "synth", "0.62", "sine", "480-720", "gain", "-18",
        "fade", "q", "0.012", "0.62", "0.26",
    ],
}


def main() -> int:
    destination = ROOT / "assets" / "audio" / "sfx"
    destination.mkdir(parents=True, exist_ok=True)
    targets = [destination / name for name in OUTPUTS]
    existing = [path for path in targets if path.exists()]
    if existing:
        print("Refusing to overwrite existing asset(s):", file=sys.stderr)
        for path in existing:
            print(path, file=sys.stderr)
        return 1

    for path, effect in zip(targets, OUTPUTS.values()):
        subprocess.run(
            ["sox", "-n", "-r", "44100", "-c", "1", "-b", "16", str(path), *effect],
            check=True,
        )
        print(f"Wrote {path}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
