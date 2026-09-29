#!/usr/bin/env bash
# export-build.sh — Plan VIII · Task 23 one-command Linux shipping export.
# Stages: preflight → dotnet build → Godot import → export release →
#         normalize artifact layout → version stamp → packaged parity →
#         exported runtime smoke + development harness against packaged data.
# Fails on the first release-critical error (set -euo pipefail).
#
# Usage: scripts/ci/export-build.sh [--skip-smoke]
set -euo pipefail
DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
cd "$DIR"
GODOT_RUNNER=(bash "$DIR/scripts/ci/run-godot-bounded.sh")
MAX_SECONDS=180
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
"${GODOT_RUNNER[@]}" --path . --import >/dev/null 2>&1 || \
  echo "WARN: --import returned nonzero (already-imported tree is fine)"

# 4. Export release (reuses the canonical exporter: PCK staging + loose Data) --
step "godot export release (Linux/X11)"
scripts/ci/godot-export-linux.sh

# Shipping assemblies must not accidentally restore host harness payloads.
# These names are CLR metadata strings, independent of debug/PDB generation.
step "shipping host symbol gate"
SHIPPING_ASSEMBLY="builds/linux/data_Ashfall_linuxbsd_x86_64/Ashfall.dll"
[[ -f "$SHIPPING_ASSEMBLY" ]] \
  || { echo "EXPORT FAIL: shipping Ashfall.dll missing" >&2; exit 1; }
# AssetRegistrySelfTest is also a retained Core action ID, and
# SnapshotOrchestrator is a production runtime service. Gate on host-only
# implementation symbols instead of these shared names.
if LC_ALL=C grep -aEq 'RunDashboardUiTestAndQuit|RunDay1PlayableSelfTest|RunAssetRegistrySelfTest|RunAssetCoverageReport|SevenDayDeterministicSmokeTest|SnapshotCaptureHarness' "$SHIPPING_ASSEMBLY"; then
  echo "EXPORT FAIL: development host symbols remain in shipping Ashfall.dll" >&2
  exit 1
fi

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
"${GODOT_RUNNER[@]}" --path . -- --export-parity-selftest --parity-target "$DIR/builds/linux"

# 7. Shipping boot + development probes against packaged data ---------------
# ExportRelease excludes host selftest sources. The development assembly
# remains the diagnostic harness and explicitly resolves the deployed Data.
if [[ "$SKIP_SMOKE" -eq 0 ]]; then
  step "exported runtime smoke + development probes against packaged data"
  EXE="builds/linux/ashfall.x86_64"
  chmod +x "$EXE" 2>/dev/null || true
  run_exported() {
    timeout --foreground --signal=TERM --kill-after=5s "${MAX_SECONDS}s" \
      env ASHFALL_DATA= "$EXE" --headless --fixed-fps 15 --max-fps 15 "$@"
  }
  run_exported --quit-after 60 >/dev/null 2>&1 \
    || { echo "EXPORT FAIL: exported build did not boot headlessly" >&2; exit 1; }
  echo "boot smoke: OK (60 frames)"
  run_development_probe() {
    env ASHFALL_DATA="$DIR/builds/linux/Assets/StreamingAssets/Data" \
      "${GODOT_RUNNER[@]}" --path "$DIR" -- "$@"
  }
  run_development_probe --bridge-selftest >/dev/null
  echo "bridge-selftest (development harness): OK"
  run_development_probe --data-integrity-selftest >/dev/null \
    || { echo "EXPORT FAIL: packaged data-integrity-selftest failed" >&2; exit 1; }
  echo "data-integrity-selftest (packaged data): OK"
  run_development_probe --research-catalog-selftest >/dev/null \
    || { echo "EXPORT FAIL: packaged research-catalog-selftest failed" >&2; exit 1; }
  echo "research-catalog-selftest (packaged data): OK"
  run_development_probe --export-parity-selftest --parity-target "$DIR/builds/linux" >/dev/null
  echo "export-parity-selftest (development harness): OK"
fi

step "DONE — builds/linux/ashfall.x86_64 (+ .pck + Assets/StreamingAssets/Data + RELEASE_STAMP.txt)"
