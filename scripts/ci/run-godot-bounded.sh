#!/usr/bin/env bash
# ASHFALL bounded Godot test/import/export launcher.
#
# Every headless Godot invocation used by verification goes through this
# wrapper. It enforces the project test policy: fixed 15 FPS and a hard 180s
# process limit. The cap is deliberately not configurable above 180s.
set -euo pipefail

MAX_SECONDS=180
KILL_GRACE_SECONDS=5

command -v godot >/dev/null 2>&1 || {
    echo "ERROR: godot is not available on PATH" >&2
    exit 127
}
command -v timeout >/dev/null 2>&1 || {
    echo "ERROR: coreutils timeout is required for bounded Godot runs" >&2
    exit 127
}

args=("$@")
has_headless=0
for arg in "${args[@]}"; do
    case "$arg" in
        --headless) has_headless=1 ;;
    esac
done

if [[ "$has_headless" -eq 0 ]]; then
    args=(--headless "${args[@]}")
fi
# Godot passes everything after `--` to the game. Normalize engine FPS flags
# only before that separator, then insert the policy values there so game
# arguments remain untouched and callers cannot override the 15 FPS rule.
separator_index=${#args[@]}
for index in "${!args[@]}"; do
    if [[ "${args[$index]}" == "--" ]]; then
        separator_index=$index
        break
    fi
done

engine_args=()
skip_next=0
for arg in "${args[@]:0:separator_index}"; do
    if [[ "$skip_next" -eq 1 ]]; then
        skip_next=0
        continue
    fi
    case "$arg" in
        --fixed-fps|--max-fps)
            skip_next=1
            ;;
        --fixed-fps=*|--max-fps=*)
            ;;
        *)
            engine_args+=("$arg")
            ;;
    esac
done

game_args=("${args[@]:separator_index}")
args=("${engine_args[@]}" --fixed-fps 15 --max-fps 15 "${game_args[@]}")

echo "[ashfall-godot] timeout=${MAX_SECONDS}s fixed_fps=15 max_fps=15" >&2
exec timeout --foreground --signal=TERM --kill-after="${KILL_GRACE_SECONDS}s" \
    "${MAX_SECONDS}s" godot "${args[@]}"
