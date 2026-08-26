#!/usr/bin/env bash
# godot-export-linux.sh — deterministic Linux export + self-contained data deployment.
# Preference order: executable-relative Assets/StreamingAssets/Data (self-contained, CI-friendly, Linux-safe)
# plus PCK fallback via res:// (future-proof). See src/Host/CatalogPath.cs for resolver hierarchy.
set -euo pipefail
DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
cd "$DIR"
echo "── godot-export-linux: building Linux/X11 release ──"
mkdir -p builds/linux
if ! godot --headless --path . --export-release "Linux/X11" builds/linux/ashfall.x86_64; then
  echo "EXPORT FAIL: godot export-release Linux/X11 failed" >&2
  exit 1
fi
if [[ ! -f builds/linux/ashfall.x86_64 ]]; then
  echo "EXPORT FAIL: builds/linux/ashfall.x86_64 missing" >&2
  exit 1
fi
if [[ ! -f builds/linux/ashfall.pck ]]; then
  echo "EXPORT FAIL: builds/linux/ashfall.pck missing" >&2
  exit 1
fi
echo "── deploying authoritative Data beside executable ──"
# Self-contained deployment: copy the JSON authority so isolated builds don't depend on the source checkout.
# This matches CatalogPath's precedence #2 (executable-relative) and is deterministic across Linux/Windows.
SRC="Assets/StreamingAssets/Data"
DST="builds/linux/Assets/StreamingAssets/Data"
if [[ ! -d "$SRC" ]]; then
  echo "EXPORT FAIL: source $SRC missing" >&2
  exit 1
fi
mkdir -p "$DST"
# Use cp -a to preserve case, then verify representative catalogs.
cp -r "$SRC"/. "$DST"/
for rep in items.json economy_goods.json locations.json; do
  if [[ ! -f "$DST/$rep" ]]; then
    echo "EXPORT FAIL: $DST/$rep missing after copy" >&2
    exit 1
  fi
done
count_src=$(ls -1 "$SRC"/*.json 2>/dev/null | wc -l)
count_dst=$(ls -1 "$DST"/*.json 2>/dev/null | wc -l)
echo "Data deployed: $count_dst json files (source $count_src) → $DST"
if [[ "$count_dst" -lt 10 ]]; then
  echo "EXPORT FAIL: suspiciously few json in $DST" >&2
  exit 1
fi
# Also ensure the .NET runtime folder is present (required for isolated run).
if [[ ! -d builds/linux/data_Ashfall_linuxbsd_x86_64 ]]; then
  echo "WARN: builds/linux/data_Ashfall_linuxbsd_x86_64 missing — mono runtime may not be bundled" >&2
fi
echo "godot-export-linux: OK — builds/linux/ashfall.x86_64 + ashfall.pck + Assets/StreamingAssets/Data"
ls -lh builds/linux/ashfall.x86_64 builds/linux/ashfall.pck | awk '{print $9, $5}'
du -sh "$DST" | awk '{print "Data size:", $1}'
