#!/usr/bin/env bash
# ASHFALL — launch the standalone Linux build without the Unity Editor.
# Run from anywhere: it resolves its own directory first.
set -euo pipefail

DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
BIN="$DIR/Builds/Linux/ASHFALL.x86_64"

if [[ ! -x "$BIN" ]]; then
    echo "ASHFALL build not found at: $BIN" >&2
    echo "Build it with:" >&2
    echo "  Unity -batchmode -quit -projectPath \"$DIR\" -executeMethod AtomicWar._Game.Editor.BuildScript.PerformBuildPipeline" >&2
    exit 1
fi

cd "$DIR/Builds/Linux"
exec "$BIN" "$@"
