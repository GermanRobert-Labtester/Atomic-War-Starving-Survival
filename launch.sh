#!/usr/bin/env bash
# ASHFALL — launch the Godot project or an exported Linux build.
# Run from anywhere: it resolves its own directory first.
#
# Flags consumed by this script (never forwarded to Godot):
#   --generate-placeholders   Regenerate the placeholder art packs + import,
#                             then launch normally.
#   --generate-visuals        Regenerate the deterministic Pillow visual pack,
#                             import it, then launch normally.
set -euo pipefail

DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

# Split launch.sh-only flags from the args forwarded to Godot.
generate_placeholders=0
generate_visuals=0
forward=()
for arg in "$@"; do
    case "$arg" in
        --generate-placeholders|--placeholders) generate_placeholders=1 ;;
        --generate-visuals|--generate-graphics|--graphics) generate_visuals=1 ;;
        *) forward+=("$arg") ;;
    esac
done
if (( ${#forward[@]} )); then
    set -- "${forward[@]}"
else
    set --
fi

# Hint when the generated packs are absent (e.g. fresh clone without LFS).
if [[ "$generate_placeholders" -eq 0 ]]; then
    for manifest in \
        "assets/sprites/Characters/PLACEHOLDER_MANIFEST.json" \
        "assets/sprites/Shelter/PLACEHOLDER_MANIFEST.json" \
        "assets/sprites/Surface/PLACEHOLDER_MANIFEST.json"; do
        if [[ ! -f "$DIR/$manifest" ]]; then
            echo "[launch.sh] Placeholder art packs missing. Regenerate with: ./launch.sh --generate-placeholders" >&2
            break
        fi
    done
else
    bash "$DIR/scripts/tools/generate-placeholders.sh"
fi

if [[ "$generate_visuals" -eq 0 ]]; then
    if [[ ! -f "$DIR/assets/sprites/Generated/ASHFALL_VISUAL_PACK_MANIFEST.json" ]]; then
        echo "[launch.sh] Procedural visual pack missing. Generate with: ./launch.sh --generate-visuals" >&2
    fi
else
    if ! command -v python3 >/dev/null 2>&1; then
        echo "[launch.sh] python3 is required to generate procedural visuals." >&2
        exit 1
    fi
    python3 "$DIR/scripts/tools/generate-visual-pack.py"
    if command -v godot >/dev/null 2>&1; then
        import_log="${TMPDIR:-/tmp}/ashfall-visual-pack-import.log"
        if ! bash "$DIR/scripts/ci/run-godot-bounded.sh" --path "$DIR" --import >"$import_log" 2>&1; then
            echo "[launch.sh] Godot import failed; see $import_log" >&2
            exit 1
        fi
        echo "[launch.sh] procedural visual pack generated and imported."
    else
        echo "[launch.sh] Godot is not on PATH; generated PNGs are ready but import sidecars were not refreshed." >&2
        echo "[launch.sh] Run later: godot --headless --path . --import" >&2
    fi
fi

test_mode=0
for arg in "$@"; do
    case "$arg" in
        --headless|--*-selftest|--*-uitest) test_mode=1 ;;
    esac
done

if command -v godot >/dev/null 2>&1; then
    if [[ "$test_mode" -eq 1 ]]; then
        exec bash "$DIR/scripts/ci/run-godot-bounded.sh" --path "$DIR" "$@"
    fi

    # Normal interactive play is intentionally not capped at 180s.
    exec godot --path "$DIR" "$@"
fi

BIN="$DIR/builds/linux/ashfall.x86_64"
if [[ ! -x "$BIN" ]]; then
    echo "Godot is not on PATH and exported build is missing: $BIN" >&2
    echo "Install Godot 4.7 .NET or build with: scripts/ci/godot-export-linux.sh" >&2
    exit 1
fi

if [[ "$test_mode" -eq 1 ]]; then
    exec timeout --foreground --signal=TERM --kill-after=5s 180s \
        env ASHFALL_DATA= "$BIN" --headless --fixed-fps 15 --max-fps 15 "$@"
fi

exec "$BIN" "$@"
