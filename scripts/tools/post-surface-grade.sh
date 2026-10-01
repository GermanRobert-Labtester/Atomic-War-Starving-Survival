#!/usr/bin/env bash
# SPDX-License-Identifier: MIT
#
# ASHFALL — Surface backdrop cinematic grade (ImageMagick).
# Grades the Blender-baked backdrops: gentle S-curve contrast, a slight
# exposure trim, colour separation, a corner vignette, and fine grain so the
# raw Cycles output reads with depth and mood. Vignette is scaled to image
# height so 1080p and 720p get a consistent falloff.
#
# Usage: bash scripts/tools/post-surface-grade.sh <pristine-bake-dir> <out-dir>
set -euo pipefail
SRC="${1:?pristine bake dir}"; DST="${2:?output dir}"
mkdir -p "$DST"
for f in "$SRC"/*.png; do
    b="$(basename "$f")"
    [[ "$b" == *MANIFEST* ]] && continue
    h="$(magick identify -format '%h' "$f")"
    sig="$(awk "BEGIN{printf \"%d\", $h*0.055}")"
    magick "$f" \
        -modulate 97,120,100 \
        -sigmoidal-contrast 2.5,50% \
        -background black -vignette "0x${sig}+0+0" \
        -attenuate 0.03 +noise Gaussian \
        "$DST/$b"
    echo "[grade] $b  1920x? -> vignette 0x$sig"
done
echo "graded -> $DST"
