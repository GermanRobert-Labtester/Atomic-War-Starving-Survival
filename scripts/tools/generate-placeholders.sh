#!/usr/bin/env bash
# ASHFALL — regenerate every placeholder art pack, then import into Godot.
#
# Reuses the deterministic Python generators:
#   generate-shelter-placeholders.py   interior lighting, rooms, props, tiles
#   generate-surface-placeholders.py   surface backdrops x lighting phases
#   generate-character-placeholders.py blockout character sheets
#
# Usage:
#   bash scripts/tools/generate-placeholders.sh
# or via launch.sh:
#   ./launch.sh --generate-placeholders
set -euo pipefail

DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
cd "$DIR"

if ! command -v python3 >/dev/null 2>&1; then
    echo "[placeholders] python3 is required to generate placeholder art." >&2
    exit 1
fi
if ! python3 -c "import PIL" >/dev/null 2>&1; then
    echo "[placeholders] Pillow is required. Install with: python3 -m pip install Pillow" >&2
    exit 1
fi

python3 scripts/tools/generate-shelter-placeholders.py
python3 scripts/tools/generate-surface-placeholders.py
python3 scripts/tools/generate-character-placeholders.py

if command -v godot >/dev/null 2>&1; then
    log="${TMPDIR:-/tmp}/ashfall-placeholder-import.log"
    if ! bash scripts/ci/run-godot-bounded.sh --path . --import >"$log" 2>&1; then
        echo "[placeholders] godot --import failed; see $log" >&2
        exit 1
    fi
    echo "[placeholders] regenerated and imported."
else
    echo "[placeholders] godot not on PATH; PNGs written but .import sidecars not regenerated." >&2
    echo "[placeholders] Run later: godot --headless --path . --import" >&2
fi
