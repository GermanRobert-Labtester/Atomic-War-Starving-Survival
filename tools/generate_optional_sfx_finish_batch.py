#!/usr/bin/env python3
"""Generate the final optional weather, radiation, and surface-audio assets."""

from pathlib import Path
import subprocess
import sys


ROOT = Path(__file__).resolve().parent.parent
OUTPUTS = {
    "sfx_radiation_chronic_alarm.wav": (
        "sfx",
        ["synth", "1.90", "sine", "265-340", "gain", "-5",
         "tremolo", "2.4", "45", "fade", "q", "0.015", "1.90", "0.30"],
    ),
    "sfx_weather_emp_storm.wav": (
        "sfx",
        ["synth", "1.65", "square", "85-42", "gain", "-10",
         "tremolo", "13", "55", "fade", "q", "0.01", "1.65", "0.28"],
    ),
    "sfx_weather_glass_storm.wav": (
        "sfx",
        ["synth", "2.50", "pinknoise", "bandpass", "4300", "2300",
         "tremolo", "7", "30", "gain", "6",
         "fade", "q", "0.02", "2.50", "0.36"],
    ),
    "sfx_weather_corrosive_precipitation.wav": (
        "sfx",
        ["synth", "2.40", "brownnoise", "bandpass", "850", "1050",
         "tremolo", "8", "38", "gain", "6",
         "fade", "q", "0.03", "2.40", "0.32"],
    ),
    "amb_surface_storm.wav": (
        "ambience",
        ["synth", "12.0", "brownnoise", "gain", "-4",
         "lowpass", "1200", "tremolo", "0.13", "48",
         "fade", "q", "0.12", "12.0", "0.85"],
    ),
}


def main() -> int:
    requested = set(sys.argv[1:])
    unknown = requested.difference(OUTPUTS)
    if unknown:
        print("Unknown output name(s): " + ", ".join(sorted(unknown)), file=sys.stderr)
        return 2
    outputs = {name: spec for name, spec in OUTPUTS.items() if not requested or name in requested}
    paths = []
    for name, (folder, _) in outputs.items():
        destination = ROOT / "assets" / "audio" / folder
        destination.mkdir(parents=True, exist_ok=True)
        paths.append(destination / name)
    existing = [path for path in paths if path.exists()]
    if existing:
        print("Refusing to overwrite existing asset(s):", file=sys.stderr)
        for path in existing:
            print(path, file=sys.stderr)
        return 1

    for path, (_, effect) in zip(paths, outputs.values()):
        subprocess.run(
            ["sox", "-n", "-r", "44100", "-c", "1", "-b", "16", str(path), *effect],
            check=True,
        )
        print(f"Wrote {path}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
