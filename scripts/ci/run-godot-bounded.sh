#!/usr/bin/env bash
# ASHFALL bounded Godot test/import/export launcher.
#
# Every headless Godot invocation used by verification goes through this
# wrapper. It enforces the project test policy: fixed 15 FPS and a hard 180s
# process limit. The cap is deliberately not configurable above 180s.
#
# Environment:
#   ASHFALL_SKIP_BUILD_STALENESS=1   skip the compiled-assembly staleness guard
#                                    (unusual workflows only; stale runs are a
#                                    false-green risk)
#   ASHFALL_EXPECT_ARTIFACT=<path>   after the run, fail when <path> is missing
#                                    or empty. Callers that promise a
#                                    machine-readable artifact set this so a
#                                    silent non-write cannot pass as green.
set -euo pipefail

# --check-staleness-only runs just the compiled-assembly staleness guard and
# exits, so CI can gate on freshness without booting Godot.
STALENESS_ONLY=0
for arg in "$@"; do
    if [[ "$arg" == "--check-staleness-only" ]]; then
        STALENESS_ONLY=1
    fi
done

MAX_SECONDS=180
KILL_GRACE_SECONDS=5
ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"

# ── Build-staleness guard ─────────────────────────────────────────────────
# Generators and gates that shell out through this wrapper query the *compiled*
# host (--host-help, --selftest-manifest, ...). If C# was edited without a
# rebuild they read stale code, and their --check reports "OK" against stale
# data — the failure is invisible because both sides are equally stale
# (observed 2026-09-30 on generate-selftest-manifest.py, which reported
# "in sync" while silently omitting four newly authored aliases).
#
# Conservative by design: only blocks when staleness is *provable* (the loaded
# assembly exists AND is older than the newest C# source). A missing assembly is
# not proof, so this can never false-positive into blocking a valid run.
# Escape hatch for unusual workflows: ASHFALL_SKIP_BUILD_STALENESS=1.
if [[ "${ASHFALL_SKIP_BUILD_STALENESS:-0}" != "1" ]]; then
    ASSEMBLY="$ROOT/.godot/mono/temp/bin/Debug/Ashfall.dll"
    if [[ -f "$ASSEMBLY" ]]; then
        newest_src=$(find "$ROOT/src" "$ROOT/Assets/Ashfall.Core" -name '*.cs' -printf '%T@\n' 2>/dev/null | sort -rn | head -1 || true)
        dll_mtime=$(stat -c %Y "$ASSEMBLY" 2>/dev/null || echo 0)
        if [[ -n "${newest_src:-}" ]] && awk -v s="$newest_src" -v d="$dll_mtime" 'BEGIN{exit !(s>d)}'; then
            echo "ERROR: compiled host is STALE — C# sources are newer than" >&2
            echo "       $ASSEMBLY" >&2
            echo "       This run would execute, and verify, stale code." >&2
            echo "Fix:   dotnet build Ashfall.csproj   # then re-run" >&2
            echo "       (or set ASHFALL_SKIP_BUILD_STALENESS=1 to override)" >&2
            exit 2
        fi
    fi
fi

if [[ "$STALENESS_ONLY" == "1" ]]; then
    echo "[ashfall-godot] staleness check OK" >&2
    exit 0
fi

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
status=0
timeout --foreground --signal=TERM --kill-after="${KILL_GRACE_SECONDS}s" \
    "${MAX_SECONDS}s" godot "${args[@]}" || status=$?

# Task 14 (ninth wave) — optional artifact-presence contract. A runner that
# exits 0 without writing the artifact it promised is a false green. Callers
# that promise an artifact set ASHFALL_EXPECT_ARTIFACT to its repo-relative path.
if [[ -n "${ASHFALL_EXPECT_ARTIFACT:-}" ]]; then
    if [[ ! -s "$ASHFALL_EXPECT_ARTIFACT" ]]; then
        echo "ERROR: expected artifact was not written: $ASHFALL_EXPECT_ARTIFACT" >&2
        exit 1
    fi
fi
exit "$status"
