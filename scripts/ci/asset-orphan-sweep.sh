#!/usr/bin/env bash
# asset-orphan-sweep.sh — first line of defence against missing Godot
# importer sidecars on the LFS-routed assets/ tree.
#
# Background
# ----------
# Each Godot `assets/art/foo.jpg`, `assets/sprites/.../*.png`, and `assets/audio/*`
# file needs a sibling `foo.jpg.import` (or `.png.import`, etc.) sidecar. The
# sidecar maps the user-facing `res://assets/...` path to the cached
# `.godot/imported/...-{md5}.ctex` artefact that ResourceLoader.Load returns
# at runtime. Without the sidecar, the asset is invisible to the runtime even
# though it sits on disk — silent regression.
#
# Lane A (Aug-2026, commit d88bd8a3) swept ten such orphans that had landed
# in the working tree after the previous sidecar-sweep commit 1485c53a. After
# Lane A landed, 50+ more untracked jpgs continued to accumulate (working
# tree mtimes 2026-08-19 11:33:06) without their sidecars. This chore is the
# standing protection: every dotnet build / lane compile that touches the
# asset registry should run this first, and fail when orphans sneak in.
#
# Scope (deliberately narrow)
# ---------------------------
# This script reads ONLY the Godot-active trees:
#
#   assets/art/      (production item + location + survivor + faction icons)
#   assets/sprites/  (any PNG: items, portraits, locations, factions, characters)
#   assets/ui/       (any PNG / SVG: UI textures)
#   assets/audio/    (any wav/mp3/ogg: audio assets)
#   assets/fonts/    (any ttf/otf: fonts)
#
# It deliberately does NOT touch Assets/ (uppercase, Unity legacy — AGENTS.md
# treats that tree as read-only legacy). Orphan sidecars without matching
# source are also flagged (e.g. dangling `.import` files left behind after
# an asset move).
#
# Exit codes
# ----------
#   0  -- no orphans; safe to proceed
#   1  -- orphans detected; commit blocked by the gate
#   2  -- usage error (no assets/ tree, etc.)
#
# Usage
# -----
#   scripts/ci/asset-orphan-sweep.sh                # summary + exit-code
#   scripts/ci/asset-orphan-sweep.sh --verbose      # per-orphan listing
#   scripts/ci/asset-orphan-sweep.sh --list         # emit all orphan paths
#
# The `--list` mode is what the asset gate CI uses to enumerate the offenders
# when failing, so the user sees exactly which files need fixing.

set -euo pipefail

DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
cd "$DIR"

# Sidecar-bearing extensions Godot ships an importer for on this project.
# Mirrors what the asset-registry selftest probes — keep in sync if the
# importer graph changes.
readonly SOURCED_EXTS=(jpg jpeg png svg ttf otf wav mp3 ogg m4a)

# Roots we police. `assets/` is the Godot-active tree; lying low-case is
# intentional (see AGENTS.md ↔ Assets/ is the Unity legacy tree).
readonly SWEEP_ROOTS=(
    assets/art
    assets/sprites
    assets/ui
    assets/audio
    assets/fonts
)

usage() {
    sed -n '/^# Usage/,/^$/p' "$0" | sed '/^#/d;/^$/d'
    exit "${1:-0}"
}

mode="summary"
case "${1:-}" in
    "")          mode="summary" ;;
    --verbose)   mode="verbose" ;;
    --list)      mode="list" ;;
    --help|-h)   usage 0 ;;
    *)           echo "unknown arg: $1" >&2; usage 2 ;;
esac

orphan_source=()   # art files lacking sidecar sibling
orphan_sidecar=()  # sidecar files lacking art sibling

for root in "${SWEEP_ROOTS[@]}"; do
    if [[ ! -d "$root" ]]; then
        # asset root missing — not an error, but warn in verbose mode
        if [[ "$mode" != "list" ]] && [[ "$mode" != "summary" ]]; then
            echo "warn: root not present: $root" >&2
        fi
        continue
    fi

    # 1) orphan-source scan: art files whose sidecar is missing
    while IFS= read -r -d '' pkg; do
        if [[ ! -f "${pkg}.import" ]]; then
            orphan_source+=("$pkg")
        fi
    done < <(find "$root" -type f -regextype posix-extended \
                -regex "${root#/}/.*\\.($(IFS='|'; echo "${SOURCED_EXTS[*]}"))$" \
                -print0 2>/dev/null)

    # 2) orphan-sidecar scan: sidecars whose source is missing
    while IFS= read -r -d '' sidecar; do
        src="${sidecar%.import}"
        if [[ ! -f "$src" ]]; then
            orphan_sidecar+=("$sidecar")
        fi
    done < <(find "$root" -type f -name '*.import' -print0 2>/dev/null)
done

total=$((${#orphan_source[@]} + ${#orphan_sidecar[@]}))

case "$mode" in
    list)
        for f in "${orphan_source[@]}";   do printf '%s\n' "$f"; done
        for f in "${orphan_sidecar[@]}";  do printf '%s\n' "$f"; done
        ;;

    verbose|summary)
        echo "[asset-orphan-sweep] roots=${SWEEP_ROOTS[*]}"
        echo "[asset-orphan-sweep] orphan sources (need .import sidecar): ${#orphan_source[@]}"
        echo "[asset-orphan-sweep] orphan sidecars (need source file):     ${#orphan_sidecar[@]}"
        echo "[asset-orphan-sweep] total: $total"

        if [[ "$mode" == "verbose" ]] && [[ $total -gt 0 ]]; then
            echo
            echo "── orphan sources ──"
            printf '  %s\n' "${orphan_source[@]:-(none)}"
            echo
            echo "── orphan sidecars ──"
            printf '  %s\n' "${orphan_sidecar[@]:-(none)}"
            echo
            echo "Fix path:"
            echo "  godot --headless --no-window --path . --import"
            echo "  # will regenerate every sidecar above in one pass."
        fi
        ;;
esac

if [[ $total -gt 0 ]]; then
    exit 1
elif [[ ! -d assets ]]; then
    echo "[asset-orphan-sweep] FATAL: no assets/ tree at $DIR" >&2
    exit 2
fi

exit 0
