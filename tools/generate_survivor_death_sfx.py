#!/usr/bin/env python3
"""Create ASHFALL's original non-vocal survivor-death audio cue.

This intentionally uses local SoX synthesis only. It refuses to overwrite the
runtime asset; rerunning it after production replacement is therefore safe.
"""

from pathlib import Path
import subprocess
import sys


ROOT = Path(__file__).resolve().parent.parent
OUTPUT = ROOT / "assets" / "audio" / "sfx" / "sfx_survivor_death.wav"


def run(*args: str) -> None:
    subprocess.run(args, check=True)


def main() -> int:
    if OUTPUT.exists():
        print(f"Refusing to overwrite existing asset: {OUTPUT}", file=sys.stderr)
        return 1

    OUTPUT.parent.mkdir(parents=True, exist_ok=True)
    # A restrained falling tone communicates finality without a graphic injury
    # or a survivor-specific voice.
    run(
        "sox", "-n", "-r", "44100", "-c", "1", "-b", "16", str(OUTPUT),
        "synth", "1.8", "sine", "96-38", "gain", "-14",
        "fade", "q", "0.01", "1.8", "0.45",
    )
    print(f"Wrote {OUTPUT}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
