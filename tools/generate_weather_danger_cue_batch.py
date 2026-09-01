#!/usr/bin/env python3
"""Generate original weather and infrastructure alerts with local SoX only."""

from pathlib import Path
import subprocess
import sys


ROOT = Path(__file__).resolve().parent.parent
OUTPUTS = {
    # Dense, high rain texture with a slow unsettled pulse; not a radiation alarm.
    "sfx_weather_black_rain.wav": [
        "synth", "2.80", "pinknoise", "gain", "-1",
        "bandpass", "1550", "1200", "tremolo", "5", "24",
        "fade", "q", "0.03", "2.80", "0.38",
    ],
    # Wide low wind bed for a storm onset, distinct from the short gust cue.
    "sfx_weather_blizzard.wav": [
        "synth", "5.60", "brownnoise", "gain", "-4",
        "lowpass", "1050", "tremolo", "0.17", "58",
        "fade", "q", "0.08", "5.60", "0.70",
    ],
    # A short rising mechanical warning for infrastructure danger, not weather.
    "sfx_danger_alarm_klaxon.wav": [
        "synth", "1.45", "sine", "510-910", "gain", "-14",
        "fade", "q", "0.01", "1.45", "0.18", "repeat", "2",
    ],
}


def main() -> int:
    requested = set(sys.argv[1:])
    unknown = requested.difference(OUTPUTS)
    if unknown:
        print("Unknown output name(s): " + ", ".join(sorted(unknown)), file=sys.stderr)
        return 2
    outputs = {name: effect for name, effect in OUTPUTS.items() if not requested or name in requested}
    destination = ROOT / "assets" / "audio" / "sfx"
    destination.mkdir(parents=True, exist_ok=True)
    targets = [destination / name for name in outputs]
    existing = [path for path in targets if path.exists()]
    if existing:
        print("Refusing to overwrite existing asset(s):", file=sys.stderr)
        for path in existing:
            print(path, file=sys.stderr)
        return 1

    for path, effect in zip(targets, outputs.values()):
        subprocess.run(
            ["sox", "-n", "-r", "44100", "-c", "1", "-b", "16", str(path), *effect],
            check=True,
        )
        print(f"Wrote {path}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
