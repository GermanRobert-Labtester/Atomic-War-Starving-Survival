#!/usr/bin/env bash
# =============================================================================
# ASHFALL CI — Scene Binding Truth Gate (P096)
# =============================================================================
# Prevents the four defect classes behind the P089–P095 findings. All checks are
# static (no build, no engine), so the gate is fast-tier.
#
#   1. STUB CONTENT SCRIPT — every src/**/*Content.cs whose type body is empty
#      must be a *proven live scene root binding*: some .tscn must declare AND
#      assign that script (`script = ExtResource("<id>")`). An empty content
#      class nothing attaches is a stub, not a shim.
#   2. UNASSIGNED SCRIPT EXT_RESOURCE — a .tscn that declares
#      [ext_resource type="Script"] but never assigns it instantiates as a plain
#      Control while looking script-bound. This is what made five scenes lie.
#   3. DANGLING SCRIPT EXT_RESOURCE — a declared script path that does not exist
#      on disk breaks scene load.
#   4. REQUEST/ROOT TYPE MISMATCH — every production
#      PanelSceneLoader.Load<T>("res://…") whose T is concrete must be satisfied
#      by the scene root's attached script. PackedScene.Instantiate<T> is an
#      `unbox.any` hard cast, so a mismatch throws InvalidCastException at
#      runtime with no scene context. This is the DailyBriefingModal defect.
#   5. PRE-HIDDEN CONTENT SCENE — a scene bound through a *Content type is
#      embedded by its host (AshfallDashboardShell.SetContent does not reset
#      Visible), so `visible = false` on its root renders a blank surface while
#      every binding still resolves. This is the WaterTreatmentPanel defect.
#
# Exit codes:
#   0 - Clean
#   1 - Violations detected
# =============================================================================

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"
cd "${REPO_ROOT}"

echo "=== Scene Binding Truth Gate ==="

violations=0
report() {
    echo "  FAIL [$1] $2"
    violations=$((violations + 1))
}

# res://path -> filesystem path
res_to_fs() { printf '%s' "${1#res://}"; }

# ---------------------------------------------------------------------------
# Build the scene index once: for each .tscn, its root script (assigned, not
# merely declared), its declared-but-unassigned scripts, and root visibility.
# ---------------------------------------------------------------------------
declare -A SCENE_ROOT_SCRIPT=()      # tscn -> assigned root script res:// path ("" if none)
declare -A SCENE_ROOT_HIDDEN=()      # tscn -> 1 when root has `visible = false`

while IFS= read -r scene; do
    if [ -z "$scene" ]; then continue; fi

    # id -> script path for every declared Script ext_resource
    declare -A declared_ids=()
    while IFS= read -r line; do
        sid="$(printf '%s' "$line" | sed -n 's/.*path="\([^"]*\)".*id="\([^"]*\)".*/\2/p')"
        spath="$(printf '%s' "$line" | sed -n 's/.*path="\([^"]*\)".*id="\([^"]*\)".*/\1/p')"
        if [ -n "$sid" ]; then declared_ids["$sid"]="$spath"; fi
    done < <(grep '^\[ext_resource type="Script"' "$scene" || true)

    assigned=""
    for sid in "${!declared_ids[@]}"; do
        if grep -q "^script = ExtResource(\"$sid\")" "$scene"; then
            assigned="${declared_ids[$sid]}"
        else
            report "UNASSIGNED_SCRIPT" "$scene declares [ext_resource type=\"Script\" id=\"$sid\" -> ${declared_ids[$sid]}] but never assigns it via 'script = ExtResource(\"$sid\")'; the scene root instantiates as a plain Control."
        fi
    done

    # Check 3 — dangling declared script paths
    for sid in "${!declared_ids[@]}"; do
        p="$(res_to_fs "${declared_ids[$sid]}")"
        if [ ! -f "$p" ]; then
            report "DANGLING_SCRIPT" "$scene declares script ${declared_ids[$sid]} which does not exist at $p"
        fi
    done

    SCENE_ROOT_SCRIPT["$scene"]="$assigned"

    # Root visibility: the first [node ...] block with no parent= is the root.
    hidden=0
    in_root=0
    while IFS= read -r line; do
        case "$line" in
            '[node '*)
                if printf '%s' "$line" | grep -q 'parent='; then
                    in_root=0
                else
                    in_root=1
                fi
                ;;
            *)
                if [ "$in_root" -eq 1 ] && printf '%s' "$line" | grep -qE '^visible = false$'; then
                    hidden=1
                fi
                ;;
        esac
    done < "$scene"
    SCENE_ROOT_HIDDEN["$scene"]=$hidden
    unset declared_ids
done < <(find assets src -name "*.tscn" -type f | sort)

# ---------------------------------------------------------------------------
# Check 1 — stub *Content.cs must be a proven live scene root binding
# ---------------------------------------------------------------------------
while IFS= read -r cs; do
    if [ -z "$cs" ]; then continue; fi

    # Strip comments/blank lines; count the remaining non-brace statements.
    # `|| true` is required: a pure shim has every line filtered away, so the
    # final grep -v exits 1 and pipefail would otherwise abort the gate.
    body_lines="$( { sed -e 's://.*::' -e '/^[[:space:]]*$/d' "$cs" \
        | grep -vE '^[[:space:]]*[{}];?[[:space:]]*$' \
        | grep -vE '^[[:space:]]*(using |namespace |public partial class |public class )' \
        | wc -l; } || true)"

    # Has real members; not a stub.
    if [ "$body_lines" -gt 0 ]; then continue; fi

    live=0
    for scene in "${!SCENE_ROOT_SCRIPT[@]}"; do
        s="${SCENE_ROOT_SCRIPT[$scene]}"
        if [ -z "$s" ]; then continue; fi
        if [ "$(res_to_fs "$s")" = "$cs" ]; then
            live=1
            # Check 5 — a bound content scene must not ship pre-hidden
            if [ "${SCENE_ROOT_HIDDEN[$scene]}" = "1" ]; then
                report "PRE_HIDDEN_CONTENT" "$scene binds $cs but its root has 'visible = false'; embedded content renders blank because AshfallDashboardShell.SetContent does not reset Visible."
            fi
        fi
    done

    if [ "$live" -eq 0 ]; then
        report "STUB_CONTENT" "$cs is an empty content class that no .tscn attaches as its root script. Implement it, attach it to the scene that needs it, or delete it."
    fi
done < <(find src -name "*Content.cs" -type f | sort)

# ---------------------------------------------------------------------------
# Check 4 — production PanelSceneLoader.Load<T>(scene) request/root agreement
# ---------------------------------------------------------------------------
# Class name (and optional direct base) declared by a C# file. Comments are
# stripped first so prose containing the word "class" is not read as a
# declaration (`// … this class is …` previously matched as `class is`).
declared_class() {
    sed -e 's://.*::' "$1" \
        | sed -n 's/.*class[[:space:]]\{1,\}\([A-Za-z_][A-Za-z0-9_]*\)\([[:space:]]*:[[:space:]]*\([A-Za-z_][A-Za-z0-9_]*\)\)\{0,1\}.*/\1 \3/p' \
        | head -1
}

while IFS= read -r hit; do
    if [ -z "$hit" ]; then continue; fi
    file="${hit%%:*}"
    rest="${hit#*:}"
    want="$(printf '%s' "$rest" | sed -n 's/.*PanelSceneLoader\.Load<\([A-Za-z_][A-Za-z0-9_]*\)>("\([^"]*\)".*/\1/p')"
    scene_res="$(printf '%s' "$rest" | sed -n 's/.*PanelSceneLoader\.Load<\([A-Za-z_][A-Za-z0-9_]*\)>("\([^"]*\)".*/\2/p')"
    if [ -z "$want" ] || [ -z "$scene_res" ]; then continue; fi

    # Generic probes (selftests / layout harnesses) legitimately ask for a base type.
    case "$want" in Node|Control|CanvasItem) continue ;; esac

    scene="$(res_to_fs "$scene_res")"
    if [ ! -f "$scene" ]; then
        report "MISSING_SCENE" "$file requests Load<$want>(\"$scene_res\") but no such scene exists"
        continue
    fi

    root_script="${SCENE_ROOT_SCRIPT[$scene]:-}"
    if [ -z "$root_script" ]; then
        report "ROOT_TYPE_MISMATCH" "$file requests PanelSceneLoader.Load<$want>(\"$scene_res\") but that scene root has NO script attached, so Instantiate<$want> throws InvalidCastException (unbox.any hard cast). Attach a script deriving from $want, or construct $want directly."
        continue
    fi

    script_fs="$(res_to_fs "$root_script")"
    if [ ! -f "$script_fs" ]; then continue; fi   # already reported as DANGLING_SCRIPT
    got=""; base=""
    read -r got base <<<"$(declared_class "$script_fs")" || true
    if [ "$got" != "$want" ] && [ "$base" != "$want" ]; then
        report "ROOT_TYPE_MISMATCH" "$file requests PanelSceneLoader.Load<$want>(\"$scene_res\") but the scene root script is $root_script (class ${got:-?} : ${base:-?}); Instantiate<$want> throws InvalidCastException."
    fi
done < <(grep -rn 'PanelSceneLoader\.Load<' src --include='*.cs' || true)

echo
if [ "$violations" -gt 0 ]; then
    echo "Scene Binding Truth Gate: FAIL ($violations violation(s))"
    exit 1
fi

scenes_scanned="$(printf '%s\n' "${!SCENE_ROOT_SCRIPT[@]}" | grep -c . || true)"
echo "Scene Binding Truth Gate: PASS (0 violations across $scenes_scanned scenes)"
exit 0
