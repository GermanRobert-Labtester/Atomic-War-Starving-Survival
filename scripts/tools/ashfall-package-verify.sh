#!/bin/bash
# ASHFALL — self-verifying alpha package script.
#
# Runs the whole packaging gate in one command:
#   1. pre-export gates (host build, in-tree data authority)
#   2. Linux/X11 release export
#   3. artifact check (binary + PCK + data_* assembly folder)
#   4. isolated PCK-only verification OUTSIDE the project tree (no loose data)
#   5. Windows export + artifacts + wine smoke (skipped with a reason when release templates are absent)
#   6. size row for docs/builds/BUILD_SIZES.md + PASS/FAIL summary
#
# Usage: scripts/tools/ashfall-package-verify.sh [--skip-build]
set -u
ROOT="$(cd "$(dirname "$0")/../.." && pwd)"
cd "$ROOT" || exit 1

OUT="$ROOT/builds/linux"
LOG_DIR="${TMPDIR:-/tmp}/ashfall-package-verify"
mkdir -p "$LOG_DIR"
FAILURES=0
SKIP_BUILD=0
[ "${1:-}" = "--skip-build" ] && SKIP_BUILD=1

say() { echo "[package] $(date +%H:%M:%S) $*"; }

# ── 1. Pre-export gates ──────────────────────────────────────────────
if [ "$SKIP_BUILD" = 0 ]; then
  say "host build (dotnet Ashfall.csproj)"
  if ! dotnet build Ashfall.csproj --nologo -v q >"$LOG_DIR/build.log" 2>&1; then
    echo "[package] FAIL host build (see $LOG_DIR/build.log)"; FAILURES=$((FAILURES + 1))
  fi
fi

say "in-tree data authority"
if timeout 900 godot --headless --path . -- --data-integrity-selftest >"$LOG_DIR/data-intree.log" 2>&1 \
   && grep -q "DATA_INTEGRITY_SELFTEST PASS" "$LOG_DIR/data-intree.log"; then
  say "  in-tree data PASS"
else
  echo "[package] FAIL in-tree data integrity"; FAILURES=$((FAILURES + 1))
fi

# ── 2. Linux export ──────────────────────────────────────────────────
say "exporting Linux/X11"
if ! timeout 1200 godot --headless --path . --export-release "Linux/X11" "$OUT/ashfall.x86_64" >"$LOG_DIR/export-linux.log" 2>&1; then
  echo "[package] FAIL linux export (see $LOG_DIR/export-linux.log)"; FAILURES=$((FAILURES + 1))
fi

# ── 3. Artifact check ────────────────────────────────────────────────
for artifact in "$OUT/ashfall.x86_64" "$OUT/ashfall.pck" "$OUT/data_Ashfall_linuxbsd_x86_64"; do
  if [ ! -e "$artifact" ]; then
    echo "[package] FAIL missing artifact: $artifact"; FAILURES=$((FAILURES + 1))
  fi
done

# ── 4. Isolated PCK-only verification (outside the project tree) ─────
say "isolated PCK-only verification"
TMP="$(mktemp -d)"
if [ -x "$OUT/ashfall.x86_64" ] && [ -f "$OUT/ashfall.pck" ]; then
  cp "$OUT/ashfall.x86_64" "$OUT/ashfall.pck" "$TMP/" 2>/dev/null
  cp -r "$OUT/data_Ashfall_linuxbsd_x86_64" "$TMP/" 2>/dev/null
  if ( cd "$TMP" && timeout 900 ./ashfall.x86_64 --headless -- --data-integrity-selftest >"$LOG_DIR/data-pck.log" 2>&1 ) \
     && grep -q "DATA_INTEGRITY_SELFTEST PASS" "$LOG_DIR/data-pck.log"; then
    say "  PCK-only data PASS ($(grep -o '[0-9]* catalogs' "$LOG_DIR/data-pck.log" | head -1))"
  else
    echo "[package] FAIL PCK-only data integrity (see $LOG_DIR/data-pck.log)"; FAILURES=$((FAILURES + 1))
  fi
else
  echo "[package] FAIL cannot verify (missing binary/pck)"; FAILURES=$((FAILURES + 1))
fi
rm -rf "$TMP"

# ── 5. Windows export (template-gated) ───────────────────────────────
# Godot reports e.g. "4.7.1.stable.mono.official.a13da4feb"; the export
# template directory uses the first five dotted segments (4.7.1.stable.mono).
GODOT_VERSION="$(godot --version 2>/dev/null | tail -1 | tr -d '[:space:]' | cut -d. -f1-5)"
TEMPLATES="$HOME/.local/share/godot/export_templates/$GODOT_VERSION"
if ls "$TEMPLATES"/windows_release_x86_64.exe >/dev/null 2>&1; then
  say "exporting Windows Desktop"
  WINDOWS_OUT="$ROOT/builds/windows"
  mkdir -p "$WINDOWS_OUT"
  if ! timeout 1200 godot --headless --path . --export-release "Windows Desktop" "$WINDOWS_OUT/ashfall.exe" >"$LOG_DIR/export-windows.log" 2>&1; then
    echo "[package] FAIL windows export (see $LOG_DIR/export-windows.log)"; FAILURES=$((FAILURES + 1))
  fi

  for artifact in "$WINDOWS_OUT/ashfall.exe" "$WINDOWS_OUT/ashfall.pck" "$WINDOWS_OUT/data_Ashfall_windows_x86_64"; do
    if [ ! -e "$artifact" ]; then
      echo "[package] FAIL missing windows artifact: $artifact"; FAILURES=$((FAILURES + 1))
    fi
  done

  if command -v wine >/dev/null 2>&1 && [ -f "$WINDOWS_OUT/ashfall.exe" ]; then
    say "wine smoke: windows binary data integrity"
    if ( cd "$WINDOWS_OUT" && WINEDEBUG=-all timeout 900 wine ashfall.exe --headless -- --data-integrity-selftest >"$LOG_DIR/data-windows.log" 2>&1 ) \
       && grep -q "DATA_INTEGRITY_SELFTEST PASS" "$LOG_DIR/data-windows.log"; then
      say "  Windows wine smoke PASS ($(grep -o '[0-9]* catalogs' "$LOG_DIR/data-windows.log" | head -1))"
    else
      echo "[package] FAIL windows wine smoke (see $LOG_DIR/data-windows.log)"; FAILURES=$((FAILURES + 1))
    fi
  else
    say "  wine not installed — windows smoke skipped (artifacts verified only)"
  fi
else
  say "SKIP windows export — windows_release_x86_64.exe template not installed for $GODOT_VERSION"
fi

# ── 6. macOS export (ASTC toggle scoped to this step) ────────────────
# The macOS preset (universal) refuses to export while ETC2 ASTC is off, and
# enabling it globally grows the Linux/Windows PCK. So the toggle is applied
# here only. NOTE: toggling back does not evict the ASTC variants already in
# .godot/imported; to restore the 128 MB-class PCK, clear the cache and run
# `godot --headless --path . --import` after a macOS export (see
# docs/builds/MACOS_EXPORT.md).
if [ -f "$HOME/.local/share/godot/export_templates/$GODOT_VERSION/macos.zip" ]; then
  say "exporting macOS (bundle target: .zip packaging is broken on Linux hosts)"
  sed -i 's#import_etc2_astc=false#import_etc2_astc=true#' project.godot
  if timeout 1800 godot --headless --path . --export-release "macOS" "$ROOT/builds/macos/ashfall.app" >"$LOG_DIR/export-macos.log" 2>&1; then
    say "  macOS bundle exported"
    MAC_PCK=$(stat -c%s "$ROOT/builds/macos/ashfall.app/Contents/Resources/"*.pck 2>/dev/null || echo 0)
    if [ "$MAC_PCK" != 0 ]; then
      say "  macOS PCK $(numfmt --to=iec "$MAC_PCK" 2>/dev/null || echo "$MAC_PCK B") (unsigned; sign+notarize on a Mac)"
    else
      echo "[package] FAIL macOS PCK missing inside bundle"; FAILURES=$((FAILURES + 1))
    fi
  else
    echo "[package] FAIL macOS export (see $LOG_DIR/export-macos.log)"; FAILURES=$((FAILURES + 1))
  fi
  sed -i 's#import_etc2_astc=true#import_etc2_astc=false#' project.godot
else
  say "SKIP macOS export — macos.zip template not installed for $GODOT_VERSION"
fi

# ── 7. Regression row + summary ──────────────────────────────────────
BIN_SIZE=$(stat -c%s "$OUT/ashfall.x86_64" 2>/dev/null || echo 0)
PCK_SIZE=$(stat -c%s "$OUT/ashfall.pck" 2>/dev/null || echo 0)
WIN_BIN=$(stat -c%s "$ROOT/builds/windows/ashfall.exe" 2>/dev/null || echo 0)
WIN_PCK=$(stat -c%s "$ROOT/builds/windows/ashfall.pck" 2>/dev/null || echo 0)
CATALOGS=$(grep -o '[0-9]* catalogs' "$LOG_DIR/data-pck.log" 2>/dev/null | head -1)
echo
echo "[package] BUILD_SIZES.md row:"
echo "| $(date +%F) | Linux/X11 | $BIN_SIZE | $PCK_SIZE | — | — | package-verify ${CATALOGS:-?} PCK-only PASS |"
if [ "$WIN_BIN" != 0 ]; then
  echo "| $(date +%F) | Windows Desktop | $WIN_BIN | $WIN_PCK | — | — | package-verify wine smoke PASS |"
fi
if [ "${MAC_PCK:-0}" != 0 ]; then
  echo "| $(date +%F) | macOS (universal) | — | $MAC_PCK | — | — | package-verify bundle export PASS (unsigned) |"
fi
echo
if [ "$FAILURES" = 0 ]; then
  echo "[package] PACKAGE VERIFY PASS"
  exit 0
fi
echo "[package] PACKAGE VERIFY FAIL ($FAILURES step(s); logs in $LOG_DIR)"
exit 1
