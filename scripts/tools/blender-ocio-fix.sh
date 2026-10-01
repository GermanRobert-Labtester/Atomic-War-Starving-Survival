#!/usr/bin/env bash
# SPDX-License-Identifier: MIT
#
# ASHFALL — Blender OpenColorIO workaround.
#
# This environment ships Blender 5.2 with an OCIO config declaring
# `ocio_profile_version: 2.5`, but the installed OpenColorIO library is 2.4.2
# and refuses to load it ("Color management disabled"), so Cycles renders come
# out flat / untonemapped. This script emits a 2.4-compatible copy of the same
# config (AgX / Filmic / ACES intact) with its LUT search_path repointed at the
# real Blender datafiles, so colour management works.
#
# Usage:
#   export OCIO="$(bash scripts/tools/blender-ocio-fix.sh)"
#   blender --background --python scripts/tools/bake-surface.py -- --out ...
#
# Prints the generated config path on stdout; progress on stderr.
set -euo pipefail

BLENDER_SHARE="${BLENDER_SHARE:-/usr/share/blender/5.2/datafiles/colormanagement}"
OUT_DIR="${OCIO_FIX_DIR:-/tmp/ashfall-ocio}"
SRC="$BLENDER_SHARE/config.ocio"
DST="$OUT_DIR/config.ocio"

if [[ ! -f "$SRC" ]]; then
    echo "blender-ocio-fix: source config not found: $SRC" >&2
    exit 1
fi

mkdir -p "$OUT_DIR/icc" "$OUT_DIR/luts" "$OUT_DIR/filmic"

# 1. Downgrade the profile version 2.5 -> 2.4 (BuiltinTransform needs >= 2.4;
#    the 2.4.2 library accepts <= its own version).
# 2. Repoint search_path at the real Blender LUT dirs so AgX/Filmic cubes resolve.
sed -e 's/^ocio_profile_version: 2\.5/ocio_profile_version: 2.4/' \
    -e "s#^search_path: \".*\"#search_path: \"$BLENDER_SHARE/icc:$BLENDER_SHARE/luts:$BLENDER_SHARE/filmic\"#" \
    "$SRC" > "$DST"

echo "blender-ocio-fix: wrote $DST (profile 2.4, LUTs -> $BLENDER_SHARE)" >&2
echo "$DST"
