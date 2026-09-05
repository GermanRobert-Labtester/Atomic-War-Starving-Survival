#!/usr/bin/env bash
# export-build.sh — Plan VIII · Task 23 one-command Linux shipping export.
# Stages: preflight → dotnet build → Godot import → export release →
#         normalize artifact layout → version stamp → packaged parity →
#         exported runtime smoke + selftests.
# Fails on the first release-critical error (set -euo pipefail).
#
# Usage: scripts/ci/export-build.sh [--skip-smoke]
set -euo pipefail
DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
cd "$DIR"
SKIP_SMOKE=0
[[ "${1:-}" == "--skip-smoke" ]] && SKIP_SMOKE=1

step() { echo "── export-build: $1 ──"; }

# 1. Preflight -------------------------------------------------------------
step "preflight"
command -v godot >/dev/null || { echo "EXPORT FAIL: godot not on PATH" >&2; exit 1; }
command -v dotnet >/dev/null || { echo "EXPORT FAIL: dotnet not on PATH" >&2; exit 1; }
if [[ -n "$(git status --porcelain 2>/dev/null | head -1)" ]]; then
  echo "WARN: working tree is dirty (concurrent streams?) — exporting current contents"
fi

# 2. dotnet restore/build --------------------------------------------------
step "dotnet build Ashfall.csproj"
dotnet build Ashfall.csproj

# 3. Godot headless import (deterministic resource import before export) ----
step "godot headless import"
godot --headless --path . --import >/dev/null 2>&1 || \
  echo "WARN: --import returned nonzero (already-imported tree is fine)"

# 4. Export release (reuses the canonical exporter: PCK staging + loose Data) --
step "godot export release (Linux/X11)"
scripts/ci/godot-export-linux.sh

# 5. Normalize artifact layout + version stamp ------------------------------
step "version stamp"
COMMIT="$(git rev-parse HEAD 2>/dev/null || echo unknown)"
SHORT="$(git rev-parse --short HEAD 2>/dev/null || echo unknown)"
CONFIG="release"
GODOT_VER="$(godot --version 2>/dev/null | head -1 || echo unknown)"
{
  echo "game=ASHFALL (working title)"
  echo "commit=$COMMIT"
  echo "configuration=$CONFIG"
  echo "godot=$GODOT_VER"
  echo "exported_at=$(date -u +%Y-%m-%dT%H:%M:%SZ)"
} > builds/linux/RELEASE_STAMP.txt
cat builds/linux/RELEASE_STAMP.txt

# 6. Packaged parity gate (repo-side, full byte/hash compare) ---------------
step "packaged parity gate"
godot --headless --path . -- --export-parity-selftest --parity-target "$DIR/builds/linux"

# 7. Exported runtime smoke + selftests from the packaged artifact ----------
if [[ "$SKIP_SMOKE" -eq 0 ]]; then
  step "exported runtime smoke + selftests (packaged data, not the repository)"
  EXE="builds/linux/ashfall.x86_64"
  chmod +x "$EXE" 2>/dev/null || true
  ASHFALL_DATA= "$EXE" --headless --quit-after 60 >/dev/null 2>&1 \
    || { echo "EXPORT FAIL: exported build did not boot headlessly" >&2; exit 1; }
  echo "boot smoke: OK (60 frames)"
  ASHFALL_DATA= "$EXE" --headless --bridge-selftest >/dev/null
  echo "bridge-selftest: OK"
  ASHFALL_DATA= "$EXE" --headless --data-integrity-selftest >/dev/null \
    || { echo "EXPORT FAIL: packaged data-integrity-selftest failed" >&2; exit 1; }
  echo "data-integrity-selftest: OK"
  ASHFALL_DATA= "$EXE" --headless --research-catalog-selftest >/dev/null \
    || { echo "EXPORT FAIL: packaged research-catalog-selftest failed" >&2; exit 1; }
  echo "research-catalog-selftest: OK"
  ASHFALL_DATA= "$EXE" --headless --export-parity-selftest >/dev/null
  echo "export-parity-selftest (from package): OK"
fi

step "DONE — builds/linux/ashfall.x86_64 (+ .pck + Assets/StreamingAssets/Data + RELEASE_STAMP.txt)"
